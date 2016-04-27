#include <nginx.h>
#include <ngx_config.h>
#include <ngx_core.h>
#include <ngx_http.h>
#include "src/pattern/51Degrees.h"

// Define default settings.
#define FIFTYONEDEGREES_DEFAULTFILE "51Degrees.dat"
#define FIFTYONEDEGREES_DEFAULTCACHE 10000
#define FIFTYONEDEGREES_DEFAULTPOOL 20

#ifndef FIFTYONEDEGREES_MAX_PROPERTIES
#define FIFTYONEDEGREES_MAX_PROPERTIES 20
#endif
#ifndef FIFTYONEDEGREES_MAX_STRING
#define FIFTYONEDEGREES_MAX_STRING FIFTYONEDEGREES_MAX_PROPERTIES * 20
#endif

// Module config functions to enable matching in selected locations.
static char *ngx_http_51D_set(ngx_conf_t* cf, ngx_command_t *cmd, void *conf);

// Module declaration.
ngx_module_t ngx_http_51D_module;
static ngx_shm_zone_t *ngx_http_51D_shm_zone;

// Configuration function declarations.
static void *ngx_http_51D_create_main_conf(ngx_conf_t *cf);
static void *ngx_http_51D_create_loc_conf(ngx_conf_t *cf);
static char *ngx_http_51D_merge_loc_conf(ngx_conf_t *cf, void *parent, void *child);

// Handler declaration.
static ngx_int_t ngx_http_51D_handler(ngx_http_request_t *r);
static ngx_shm_zone_t *ngx_http_51D_shm_zone;

static ngx_int_t ngx_http_51D_init_shm_zone(ngx_shm_zone_t *shm_zone, void *data);

typedef struct {
    ngx_uint_t multi;
    ngx_uint_t count;
    ngx_str_t **property;
    ngx_str_t name;
    ngx_str_t lower_name;
} ngx_http_51D_match_t;

// Module location config.
typedef struct {
    int count;
    ngx_http_51D_match_t **match;
} ngx_http_51D_loc_conf_t;

// Module main config.
typedef struct {
	char properties[FIFTYONEDEGREES_MAX_STRING];
    ngx_uint_t cacheSize;
    ngx_uint_t poolSize;
    ngx_str_t dataFile;
    fiftyoneDegreesProvider *provider;
} ngx_http_51D_main_conf_t;

// Module post config. Added module handler to main config.
static ngx_int_t
ngx_http_51D_post_conf(ngx_conf_t *cf)
{
	ngx_http_handler_pt *h;
	ngx_http_core_main_conf_t *cmcf;
	ngx_http_51D_main_conf_t *fdmcf;

	cmcf = ngx_http_conf_get_module_main_conf(cf, ngx_http_core_module);
	fdmcf = ngx_http_conf_get_module_main_conf(cf, ngx_http_51D_module);

	h = ngx_array_push(&cmcf->phases[NGX_HTTP_ACCESS_PHASE].handlers);
	if (h == NULL) {
		return NGX_ERROR;
	}

	*h = ngx_http_51D_handler;

	return NGX_OK;
}

// Create main config. Allocates memory to the configuration and initialises
// integers to -1.
static void *
ngx_http_51D_create_main_conf(ngx_conf_t *cf)
{
    ngx_http_51D_main_conf_t  *conf;
	ngx_str_t name;
	name.data = "fiftyoneDegreesProvider";
	name.len = ngx_strlen(name.data);

    conf = ngx_pcalloc(cf->pool, sizeof(ngx_http_51D_main_conf_t));
    if (conf == NULL) {
        return NULL;
    }
	conf->cacheSize = NGX_CONF_UNSET_UINT;
	conf->poolSize = NGX_CONF_UNSET_UINT;

	ngx_http_51D_shm_zone = ngx_shared_memory_add(cf, &name, 900000000, (void*)&ngx_http_51D_module);
	ngx_http_51D_shm_zone->init = ngx_http_51D_init_shm_zone;

    return conf;
}

// Create location config. Allocates memory to the configuration and initialises
// integers to -1.
static void *
ngx_http_51D_create_loc_conf(ngx_conf_t *cf)
{
    ngx_http_51D_loc_conf_t *conf;

    conf = ngx_pcalloc(cf->pool, sizeof(ngx_http_51D_loc_conf_t));
    if (conf == NULL) {
        return NULL;
    }
	conf->count = 0;
	conf->match = (ngx_http_51D_match_t**)ngx_palloc(cf->pool, sizeof(ngx_http_51D_match_t*)*10);

    return conf;
}

// Merges location config. Either gets the value set, or sets to the default
// of 0.
static char *
ngx_http_51D_merge_loc_conf(ngx_conf_t *cf, void *parent, void *child)
{
	ngx_http_51D_loc_conf_t  *prev = parent;
	ngx_http_51D_loc_conf_t  *conf = child;

	ngx_conf_merge_uint_value(conf->count, prev->count, 0);

	return NGX_CONF_OK;
}

// Throws an error if data set initialisation fails.
static ngx_int_t reportDatasetInitStatus(ngx_cycle_t *cycle, fiftyoneDegreesDataSetInitStatus status,
										const char* fileName) {
	switch (status) {
	case DATA_SET_INIT_STATUS_INSUFFICIENT_MEMORY:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Insufficient memory to load '%s'.", fileName);
		break;
	case DATA_SET_INIT_STATUS_CORRUPT_DATA:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Device data file '%s' is corrupted.", fileName);
		break;
	case DATA_SET_INIT_STATUS_INCORRECT_VERSION:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Device data file '%s' is not correct version.", fileName);
		break;
	case DATA_SET_INIT_STATUS_FILE_NOT_FOUND:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Device data file '%s' not found.", fileName);
		break;
	case DATA_SET_INIT_STATUS_NULL_POINTER:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Null pointer to the existing dataset or memory location.");
		break;
	case DATA_SET_INIT_STATUS_POINTER_OUT_OF_BOUNDS:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Allocated continuous memory containing 51Degrees data file "
			"appears to be smaller than expected. Most likely because the"
			" data file was not fully loaded into the allocated memory.");
		break;
	default:
		ngx_log_error(NGX_LOG_ERR, cycle->log, 0, "Device data file '%s' could not be loaded.", fileName);
		break;
	}
	return NGX_ERROR;
}
static ngx_int_t ngx_http_51D_init_shm_zone(ngx_shm_zone_t *shm_zone, void *data)
{
	ngx_slab_pool_t *shpool;
	fiftyoneDegreesProvider *provider;
	fiftyoneDegreesDataSetInitStatus status;
	shpool = (ngx_slab_pool_t *) ngx_http_51D_shm_zone->shm.addr;
	if (data) {
		shm_zone->data = data;
		fiftyoneDegreesProviderReloadFromFile((fiftyoneDegreesProvider*)shm_zone->data);
		return NGX_OK;

	}
	provider = (fiftyoneDegreesProvider*)ngx_slab_alloc_locked(shpool, sizeof(fiftyoneDegreesProvider));
	shm_zone->data = provider;
	if (provider == NULL) {
		ngx_log_stderr(0, "51Degrees shared memory could not be allocated for Provider.");
		return NGX_ERROR;
	}
	ngx_log_debug1(NGX_LOG_DEBUG_ALL, shm_zone->shm.log, 0, "51Degrees initialised shared memory with size %d.", shm_zone->shm.size);

	return NGX_OK;
}

void *ngx_http_51D_shm_alloc(size_t __size)
{
	void *ptr;
	ngx_slab_pool_t *shpool;
	shpool = (ngx_slab_pool_t *) ngx_http_51D_shm_zone->shm.addr;
	ptr = ngx_slab_alloc_locked(shpool, __size);
	if (ptr == NULL) {
		ngx_log_error(NGX_LOG_ERR, ngx_cycle->log, 0, "51Degrees failed to allocate memory, not enough shared memory.");
	}
	return ptr;
}

void *ngx_http_51D_shm_calloc(size_t __nmemb, size_t __size)
{
	void *ptr;
	ptr = ngx_http_51D_shm_alloc(__nmemb * __size);
	ngx_memzero(ptr, __size*__nmemb);
	return ptr;
}

void ngx_http_51D_shm_free(void *__ptr)
{
	ngx_slab_pool_t *shpool;
	shpool = (ngx_slab_pool_t *) ngx_http_51D_shm_zone->shm.addr;
	if ((u_char *) __ptr < shpool->start || (u_char *) __ptr > shpool->end) {
		free(__ptr);
	}
	else {
		ngx_slab_free_locked(shpool, __ptr);
	}
}
void *(ALLOC_CALL_CONV *fiftyoneDegreesCalloc)(size_t __nmemb, size_t __size) = ngx_http_51D_shm_calloc;
void *(ALLOC_CALL_CONV *fiftyoneDegreesMalloc)(size_t __size) = ngx_http_51D_shm_alloc;
void (ALLOC_CALL_CONV *fiftyoneDegreesFree)(void *__ptr) = ngx_http_51D_shm_free;

// Initialises the provider on process start.
static ngx_int_t
ngx_http_51D_init_process(ngx_cycle_t *cycle)
{
	fiftyoneDegreesDataSetInitStatus status;
	ngx_http_51D_main_conf_t *fdmcf;

	// Get module main config.
	fdmcf = ngx_http_cycle_get_module_main_conf(cycle, ngx_http_51D_module);
	fdmcf->provider = (fiftyoneDegreesProvider*)ngx_http_51D_shm_zone->data;

	// If a setting is not set, set the default.
	if ((int)fdmcf->dataFile.len < 0) {
		fdmcf->dataFile.data = FIFTYONEDEGREES_DEFAULTFILE;
		fdmcf->dataFile.len = strlen(fdmcf->dataFile.data);
	}
	if ((int)fdmcf->cacheSize < 0) {
		fdmcf->cacheSize = FIFTYONEDEGREES_DEFAULTCACHE;
	}
	if ((int)fdmcf->poolSize < 0) {
		fdmcf->poolSize = FIFTYONEDEGREES_DEFAULTPOOL;
	}
	if(fdmcf->provider->activePool == NULL) {
		status = fiftyoneDegreesInitProviderWithPropertyString((const char*)fdmcf->dataFile.data, fdmcf->provider, (const char*)fdmcf->properties, fdmcf->poolSize, fdmcf->cacheSize);
		if (status != DATA_SET_INIT_STATUS_SUCCESS) {
			return reportDatasetInitStatus(cycle, status, (const char*)fdmcf->dataFile.data);
		}
		ngx_log_debug2(NGX_LOG_DEBUG_ALL, cycle->log, 0, "51Degrees initialised Provider from file '%s' with properties '%s'.", (char*)fdmcf->dataFile.data, fdmcf->properties);
	}
	else {
		if (ngx_strcmp(fdmcf->dataFile.data, fdmcf->provider->activePool->dataSet->fileName) != 0
		|| (int)fdmcf->poolSize != (int)fdmcf->provider->activePool->size
		|| (int)fdmcf->cacheSize != (int)fdmcf->provider->activePool->cache->total) {
			ngx_log_error(NGX_LOG_NOTICE, cycle->log, 0, "51Degrees reloading with a different file name, pool size or cache size requires a full restart.");
		}
	}
	return NGX_OK;
}

// Frees the provider on process close.
static void
ngx_http_51D_exit_process(ngx_cycle_t *cycle)
{
	ngx_slab_pool_t *shpool;
	shpool = (ngx_slab_pool_t*) ngx_http_51D_shm_zone->shm.addr;
	// Free the provider.
	fiftyoneDegreesProviderFree((fiftyoneDegreesProvider*)ngx_http_51D_shm_zone->data);
	ngx_slab_free(shpool, ngx_http_51D_shm_zone->data);
	ngx_log_debug0(NGX_LOG_DEBUG_ALL, cycle->log, 0, "51Degrees freed Provider.");
}

// Definitions of functions which can be called in 'nginx.conf'
// --51D_single takes one string argument, a comma separated
// list of properties to be returned. Is called within location
// block. Enables User-Agent matching.
// --51D_multi takes one string argument, a comma separated
// list of properties to be returned. Is called within location
// block. Enables multiple http header matching.
// --51D_filePath takes one string argument, the path to a
// 51Degrees data file. Is called within server block.
// --51D_cache takes one integer argument, the size of the
// 51Degrees cache.
// --51D_pool takes one integer argument, the size of the
// 51Degrees pool.
static ngx_command_t  ngx_http_51D_commands[] = {

	{ ngx_string("51D_single"),
	NGX_HTTP_LOC_CONF|NGX_CONF_TAKE2,
	ngx_http_51D_set,
	NGX_HTTP_LOC_CONF_OFFSET,
    0,
	NULL },

	{ ngx_string("51D_all"),
	NGX_HTTP_LOC_CONF|NGX_CONF_TAKE2,
	ngx_http_51D_set,
	NGX_HTTP_LOC_CONF_OFFSET,
    0,
	NULL },

	{ ngx_string("51D_filePath"),
	NGX_HTTP_MAIN_CONF|NGX_CONF_TAKE1,
	ngx_conf_set_str_slot,
	NGX_HTTP_MAIN_CONF_OFFSET,
	offsetof(ngx_http_51D_main_conf_t, dataFile),
	NULL },

	{ ngx_string("51D_cache"),
	NGX_HTTP_MAIN_CONF|NGX_CONF_TAKE1,
	ngx_conf_set_num_slot,
	NGX_HTTP_MAIN_CONF_OFFSET,
	offsetof(ngx_http_51D_main_conf_t, cacheSize),
	NULL },

	{ ngx_string("51D_pool"),
	NGX_HTTP_MAIN_CONF|NGX_CONF_TAKE1,
	ngx_conf_set_num_slot,
	NGX_HTTP_MAIN_CONF_OFFSET,
	offsetof(ngx_http_51D_main_conf_t, poolSize),
	NULL },

	ngx_null_command
};

// 51Degres module context.
static ngx_http_module_t ngx_http_51D_module_ctx = {
	NULL,                          /* preconfiguration */
	ngx_http_51D_post_conf,        /* postconfiguration */

	ngx_http_51D_create_main_conf, /* create main configuration */
	NULL,                          /* init main configuration */

	NULL,                          /* create server configuration */
	NULL,                          /* merge server configuration */

	ngx_http_51D_create_loc_conf,  /* create location configuration */
	ngx_http_51D_merge_loc_conf    /* merge location configuration */
};

// 51Degrees module definition.
ngx_module_t ngx_http_51D_module = {
	NGX_MODULE_V1,
	&ngx_http_51D_module_ctx,      /* module context */
	ngx_http_51D_commands,         /* module directives */
	NGX_HTTP_MODULE,               /* module type */
	NULL,                          /* init master */
	ngx_http_51D_init_process,     /* init module */
	NULL,                          /* init process */
	NULL,                          /* init thread */
	NULL,                          /* exit thread */
	NULL,                          /* exit process */
	ngx_http_51D_exit_process,     /* exit master */
	NGX_MODULE_V1_PADDING
};

// Used when matching multiple http headers to find important headers.
// See:
// https://www.nginx.com/resources/wiki/start/topics/examples/headers_management
static ngx_table_elt_t *
search_headers_in(ngx_http_request_t *r, u_char *name, size_t len) {
    ngx_list_part_t            *part;;
    ngx_table_elt_t            *h;
    ngx_uint_t                  i;

    part = &r->headers_in.headers.part;
    h = part->elts;

    for (i = 0; /* void */ ; i++) {
        if (i >= part->nelts) {
            if (part->next == NULL) {
                break;
            }

            part = part->next;
            h = part->elts;
            i = 0;
        }

        if (len != h[i].key.len || ngx_strcasecmp(name, h[i].key.data) != 0) {
            continue;
        }

        return &h[i];
    }

    return NULL;
}

void add_value(char *val, char *buffer)
{
	if (buffer[0] != '\0') {
		strcat(buffer, ",");
	}
	strcat(buffer, val);
}

void ngx_http_51D_get_match(fiftyoneDegreesWorkset *ws, ngx_http_request_t *r, int multi)
{
	int headerIndex;
	ngx_table_elt_t *searchResult;

	// If single requested, match for single User-Agent.
	if (multi == 0) {
		if (r->headers_in.user_agent)
			fiftyoneDegreesMatch(ws, (const char*)r->headers_in.user_agent[0].value.data);
		else
			fiftyoneDegreesMatch(ws, "");
	}
	// If multi requested, match for multiple http headers.
	else if (multi == 1) {
		ws->importantHeadersCount = 0;
		for (headerIndex = 0; headerIndex < ws->dataSet->httpHeadersCount; headerIndex++) {
			searchResult = search_headers_in(r, (u_char*)ws->dataSet->httpHeaders[headerIndex].headerName, strlen(ws->dataSet->httpHeaders[headerIndex].headerName));
			if (searchResult) {
				ws->importantHeaders[ws->importantHeadersCount].header = ws->dataSet->httpHeaders + headerIndex;
				ws->importantHeaders[ws->importantHeadersCount].headerValue = searchResult->value.data;
				ws->importantHeaders[ws->importantHeadersCount].headerValueLength = searchResult->value.len;
				ws->importantHeadersCount++;
			}
			fiftyoneDegreesMatchForHttpHeaders(ws);
		}
	}
}

void ngx_http_51D_get_value(fiftyoneDegreesWorkset *ws, char *values_string, const char *requiredPropertyName)
{
	char *methodName, *propertyName;
	char buffer[FIFTYONEDEGREES_MAX_STRING];
	int i, found = 0;
	if (strcmp("Method", requiredPropertyName) == 0) {
		switch(ws->method) {
			case EXACT: methodName = "Exact"; break;
			case NUMERIC: methodName = "Numeric"; break;
			case NEAREST: methodName = "Nearest"; break;
			case CLOSEST: methodName = "Closest"; break;
			default:
			case NONE: methodName = "None"; break;
		}
		add_value(methodName, values_string);
		found = 1;
	}
	else if (strcmp("Difference", requiredPropertyName) == 0) {
		sprintf(buffer, "%d", ws->difference);
		add_value(buffer, values_string);
		found = 1;
	}
	else if (strcmp("Rank", requiredPropertyName) == 0) {
		sprintf(buffer, "%d", fiftyoneDegreesGetSignatureRank(ws));
		add_value(buffer, values_string);
		found = 1;
	}
	else if (strcmp("DeviceId", requiredPropertyName) == 0) {
			fiftyoneDegreesGetDeviceId(ws, buffer, 24);
			add_value(buffer, values_string);
			found = 1;

	}
	else {
		for (i = 0; i < ws->dataSet->requiredPropertyCount; i++) {
			propertyName = (char*)fiftyoneDegreesGetPropertyName(ws->dataSet, ws->dataSet->requiredProperties[i]);
			if (strcmp(propertyName, requiredPropertyName) == 0) {
				fiftyoneDegreesSetValues(ws, i);
				add_value((char*)fiftyoneDegreesGetValueName(ws->dataSet, *ws->values), values_string);
				found = 1;
				break;
			}
		}
		if (!found) {
			add_value("NA", values_string);
		}
	}
}

// Module handler, gets a match using either the User-Agent or multiple http
// headers, then sets properties as headers.
static ngx_int_t
ngx_http_51D_handler(ngx_http_request_t *r)
{
	ngx_http_51D_main_conf_t *fdmcf;
	ngx_http_51D_loc_conf_t *fdlcf;
	fiftyoneDegreesWorkset *ws;
	int matchIndex;
	char **values_string;

	if (r->main->internal) {
		return NGX_DECLINED;
	}
	r->main->internal = 1;

	// Get 51Degrees location config.
	fdlcf = ngx_http_get_module_loc_conf(r, ngx_http_51D_module);

	// Get 51Degrees main config.
	fdmcf = ngx_http_get_module_main_conf(r, ngx_http_51D_module);

	values_string = (char**)ngx_palloc(r->pool, fdlcf->count*sizeof(char*));

	ngx_table_elt_t *h[fdlcf->count];

	// Get a workset from the pool.
	ws = fiftyoneDegreesProviderWorksetGet(fdmcf->provider);

	for (matchIndex=0;matchIndex<fdlcf->count;matchIndex++)
	{
		values_string[matchIndex] = (char*)ngx_palloc(r->pool, FIFTYONEDEGREES_MAX_STRING*sizeof(char));
		values_string[matchIndex][0] = '\0';

		// Get a match.
		ngx_http_51D_get_match(ws, r, fdlcf->match[matchIndex]->multi);

		// For each property, set the value in values_string_array.
		int property_index;
        for (property_index=0;property_index<fdlcf->match[matchIndex]->count; property_index++) {
			ngx_http_51D_get_value(ws, values_string[matchIndex], fdlcf->match[matchIndex]->property[property_index]->data);
        }

		// For each property value pair, set a new header name and value.
		h[matchIndex] = ngx_list_push(&r->headers_in.headers);
		h[matchIndex]->hash = matchIndex;
		h[matchIndex]->key.data = (u_char*)fdlcf->match[matchIndex]->name.data;
		h[matchIndex]->key.len = ngx_strlen(h[matchIndex]->key.data);
		h[matchIndex]->value.data = (u_char*)values_string[matchIndex];
		h[matchIndex]->value.len = ngx_strlen(h[matchIndex]->value.data);
		h[matchIndex]->lowcase_key = (u_char*)fdlcf->match[matchIndex]->lower_name.data;
	}

	// Release the workset back to the pool.
	fiftyoneDegreesWorksetRelease(ws);

	return NGX_DECLINED;

}

// Set properties function. Is passed a comma separated
// list of properties from nginx.conf and assigns
// each to an element in properties.
// It then initialises the detector.
void
ngx_http_51D_set_match(ngx_conf_t *cf, ngx_http_51D_match_t *match, ngx_str_t *value, ngx_http_51D_main_conf_t *fdmcf)
{
	match->count = 0;
	// Set the name of the match.
	match->name.data = (u_char*)ngx_palloc(cf->pool, sizeof(value[1]));
	match->lower_name.data = (u_char*)ngx_palloc(cf->pool, sizeof(value[1]));
	match->name.data = value[1].data;
	match->name.len = value[1].len;
	ngx_strlow(match->lower_name.data, match->name.data, match->name.len);
	match->lower_name.len = match->name.len;

    char *properties_string = (char*)value[2].data;

	char *tok = strtok((char*)properties_string, (const char*)",");
	while (tok != NULL) {
		match->property[match->count] = (ngx_str_t*)ngx_palloc(cf->pool, sizeof(ngx_str_t));
		match->property[match->count]->data = (u_char*)ngx_palloc(cf->pool, sizeof(u_char)*(ngx_strlen(tok)));
		match->property[match->count]->data = (u_char*)tok;
		match->property[match->count]->len = ngx_strlen(match->property[match->count]->data);
		if (ngx_strstr(fdmcf->properties, tok) == NULL) {
			add_value(tok, fdmcf->properties);
		}
		match->count++;
		tok = strtok(NULL, ",");
	}

}

// Enables User-Agent matching in the selected location with the properties
// string in data.
static char *ngx_http_51D_set(ngx_conf_t* cf, ngx_command_t *cmd, void *conf)
{
	ngx_http_51D_main_conf_t *fdmcf;
	ngx_http_51D_loc_conf_t *fdlcf;
	ngx_str_t *value;
	value = cf->args->elts;
	// Get the modules location and main config.
	fdmcf = ngx_http_conf_get_module_main_conf(cf, ngx_http_51D_module);
	fdlcf = ngx_http_conf_get_module_loc_conf(cf, ngx_http_51D_module);

	fdlcf->match[fdlcf->count] = (ngx_http_51D_match_t*)ngx_palloc(cf->pool, sizeof(ngx_http_51D_match_t));
	fdlcf->match[fdlcf->count]->property = (ngx_str_t**)ngx_palloc(cf->pool, sizeof(ngx_str_t*)*FIFTYONEDEGREES_MAX_PROPERTIES);

	// Enable single User-Agent matching.
	if (strcmp(cmd->name.data, "51D_single") == 0) {
		fdlcf->match[fdlcf->count]->multi = 0;
	}
	else if (strcmp(cmd->name.data, "51D_all") == 0) {
		fdlcf->match[fdlcf->count]->multi = 1;
	}

	// Set the properties for the selected location.
	ngx_http_51D_set_match(cf, fdlcf->match[fdlcf->count], value, fdmcf);
	fdlcf->count++;

	return NGX_OK;
}

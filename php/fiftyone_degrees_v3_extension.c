#ifdef HAVE_CONFIG_H
#include "config.h"
#endif
#include <stdio.h>
#include <SAPI.h>
#include "php.h"
#include "src/pattern/51Degrees.h"

#define PHP_EXTENSION_VERSION "3.2"
#define PHP_EXTENSION_EXTNAME "FiftyOne_Degrees_Detector"

extern zend_module_entry fiftyone_degrees_detector_module_entry;
#define phpext_my_extension_ptr &fiftyone_degrees_detector_module_entry

/* Declaration of 51Degrees functions exposed to PHP. */
PHP_FUNCTION(fiftyone_degrees_test_function);
PHP_FUNCTION(fiftyone_degrees_get_properties);
PHP_FUNCTION(fiftyone_match);
PHP_FUNCTION(fiftyone_info);
PHP_FUNCTION(fiftyone_http_headers);
PHP_FUNCTION(fiftyone_match_with_headers);

/* Declaration of Init and Shutdown functions for module and request. */
PHP_MINIT_FUNCTION(fiftyone_degrees_detector_init);
PHP_MSHUTDOWN_FUNCTION(fiftyone_degrees_detector_shutdown);

PHP_RINIT_FUNCTION(fiftyone_degrees_request_init);
PHP_RSHUTDOWN_FUNCTION(fiftyone_degrees_request_shutdown);

// list of custom PHP functions provided by this extension
// set {NULL, NULL, NULL} as the last record to mark the end of list
static zend_function_entry fiftyone_degrees_detector_functions[] = {
  PHP_FE(fiftyone_degrees_test_function, NULL)
  PHP_FE(fiftyone_degrees_get_properties, NULL)
  PHP_FE(fiftyone_match, NULL)
  PHP_FE(fiftyone_info, NULL)
  PHP_FE(fiftyone_http_headers, NULL)
  PHP_FE(fiftyone_match_with_headers, NULL)
  {NULL, NULL, NULL}
};

// the following code creates an entry for the module and registers it with Zend.
zend_module_entry fiftyone_degrees_detector_module_entry = {
#if ZEND_MODULE_API_NO >= 20010901
  STANDARD_MODULE_HEADER,
#endif
  PHP_EXTENSION_EXTNAME,
  fiftyone_degrees_detector_functions,
  PHP_MINIT(fiftyone_degrees_detector_init),
  PHP_MSHUTDOWN(fiftyone_degrees_detector_shutdown),
  PHP_RINIT(fiftyone_degrees_request_init), // name of the RINIT function or NULL if not applicable
  PHP_RSHUTDOWN(fiftyone_degrees_request_shutdown), // name of the RSHUTDOWN function or NULL if not applicable
  NULL, // name of the MINFO function or NULL if not applicable
#if ZEND_MODULE_API_NO >= 20010901
  PHP_EXTENSION_VERSION,
#endif
  STANDARD_MODULE_PROPERTIES
};

ZEND_GET_MODULE(fiftyone_degrees_detector)

/* Resources that will be used for detection. */

fiftyoneDegreesDataSet* dataSet;
fiftyoneDegreesDataSetInitStatus initStatus;
fiftyoneDegreesResultsetCache* cache;
fiftyoneDegreesWorksetPool* pool;
fiftyoneDegreesWorkset *wSet;

/* Implementation of functions exposed to PHP */

/**
 * To test the detecor is working.
 */
PHP_FUNCTION(fiftyone_degrees_test_function)
{
  RETURN_STRING("HELLO WORLD", 1);
}

/**
 * Function replaces the fiftyone_degrees_get_properties function
 * as the name is shorter and moe logical. Function takes in a
 * user agent string from $_SERVER['HTTP_USER_AGENT'] and returns
 * an array with device properties.
 * @return a PHP array of property:value pairs.
 */
PHP_FUNCTION(fiftyone_match) {
    //Stop if there was a problem initialising the dataset.
    if (datasetInitSuccess() == 0)
    {
        php_printf("Dataset initialisation failed.");
        return;
    }

    char* useragent;
    int useragent_len;
    //Check that user agent was passed to this function.
    if (zend_parse_parameters(ZEND_NUM_ARGS() TSRMLS_CC, "s", &useragent, &useragent_len) == SUCCESS) {
        //Initialise array for returned values.
        array_init(return_value);
        //Match the provided user agent.
        fiftyoneDegreesMatch(wSet, useragent);

        int propertyIndex;
        for(propertyIndex = 0; propertyIndex < wSet->dataSet->requiredPropertyCount; propertyIndex++) {
            char* propertyName = fiftyoneDegreesGetPropertyName(wSet->dataSet, *(wSet->dataSet->requiredProperties + propertyIndex));
            int valueCount = fiftyoneDegreesSetValues(wSet, propertyIndex);
            if (valueCount == 1) {
                char *valueString = fiftyoneDegreesGetValueName(wSet->dataSet, *wSet->values);
                add_assoc_string(return_value, propertyName, valueString, 1);
            } else if (valueCount > 1) {
                zval* valueArray;
                ALLOC_INIT_ZVAL(valueArray);
                array_init(valueArray);
                add_assoc_zval(return_value, propertyName, valueArray);
                int valueIndex;
                for(valueIndex = 0; valueIndex < valueCount; valueIndex++) {
                    char *valueString = fiftyoneDegreesGetValueName(wSet->dataSet, *(wSet->values + valueIndex));
                    add_next_index_string(valueArray, valueString, 1);
                }
            }
        }

        //Add signature rank information.
        int32_t sigRank = getSignatureRank(wSet);
        add_assoc_long(return_value, "SignatureRank", sigRank);
        add_assoc_long(return_value, "Difference", wSet->difference);

        //Add method that was used for detection to the results array.
        char* methodString;
        switch(wSet->method){
            case NONE: methodString = "None"; break;
            case EXACT: methodString = "Exact"; break;
            case NUMERIC: methodString = "Numeric"; break;
            case NEAREST: methodString = "Nearest"; break;
            case CLOSEST: methodString = "Closest"; break;
            default: methodString = "Unknown"; break;
        }
        add_assoc_string(return_value, "Method", methodString, 1);

    }
    else
    {
        php_error(E_WARNING, "Could not parse arguments.");
    }
}

/**
 * Experimental function to get access to the HTTP headers directly from this
 * module. Do not use.
 */
PHP_FUNCTION(fiftyone_http_headers)
{

    int entries = zend_llist_count(&SG(sapi_headers).headers);
    php_printf("Zend header Entries: %d<br/>", entries);

    zend_llist_position pos;
    sapi_header_struct* h;

    for (h = (sapi_header_struct*)zend_llist_get_first_ex(&SG(sapi_headers).headers, &pos);
         h;
         h = (sapi_header_struct*)zend_llist_get_next_ex(&SG(sapi_headers).headers, &pos))
    {
        php_printf("SAPI %.*s <br/>", h->header_len, h->header);
    }
}

/**
 * Function generates a report with some useful information about the data set.
 * Information includes things like the number of device combinations, the
 * published and the next update date for the data file.
 */
PHP_FUNCTION(fiftyone_info)
{
    //Stop if there was a problem initialising the dataset.
    if (datasetInitSuccess() == 0)
    {
        php_printf("Dataset initialisation failed.");
        return;
    }

    php_printf("<h3>Dataset</h3>");
    php_printf("<p>Init status %d</p>", initStatus);
    php_printf("<p>Published:\t\t%d/%d/%d\r\n</p>",
            dataSet->header.published.day,
            dataSet->header.published.month,
            dataSet->header.published.year);
    php_printf("<p>Next Update:\t\t%d/%d/%d\r\n</p>",
            dataSet->header.nextUpdate.day,
            dataSet->header.nextUpdate.month,
            dataSet->header.nextUpdate.year);
    php_printf("<p>Signatures:\t\t%d\r\n</p>", dataSet->header.signatures.count);
    php_printf("<p>Device Combinations:\t%d\r\n</p>", dataSet->header.deviceCombinations);
    php_printf("<p>Dataset full version:\t%d.%d.%d.%d\n</p>",
               dataSet->header.versionMajor,
               dataSet->header.versionMinor,
               dataSet->header.versionBuild,
               dataSet->header.versionRevision);
    php_printf("<p>Data set short version:\t%s\r\n</p>",
               &(fiftyoneDegreesGetString(dataSet, dataSet->header.formatOffset)->firstByte));
}

/**
 * Function performs device matching based on the multiple HTTP user agents.
 * @param headers: a string of HTTP header:value pairs where each line is
 * separated by the new line character. The header name is separated from the
 * header value either by space or by colon.
 * @return a PHP array of property:value entries.
 */
PHP_FUNCTION(fiftyone_match_with_headers)
{
    //Stop if there was a problem initialising the dataset.
    if (datasetInitSuccess() == 0)
    {
        return;
    }

    char *headers;
    int headers_len;
    if (zend_parse_parameters(ZEND_NUM_ARGS() TSRMLS_CC, "s", &headers, &headers_len) == SUCCESS) {
        //Initialise array for returned values.
        array_init(return_value);
        //Match the provided user agent.
        fiftyoneDegreesMatchWithHeadersString(wSet, headers, strlen(headers));

        int propertyIndex;
        for(propertyIndex = 0; propertyIndex < wSet->dataSet->requiredPropertyCount; propertyIndex++) {
            char* propertyName = fiftyoneDegreesGetPropertyName(wSet->dataSet, *(wSet->dataSet->requiredProperties + propertyIndex));
            int valueCount = fiftyoneDegreesSetValues(wSet, propertyIndex);
            if (valueCount == 1) {
                char *valueString = fiftyoneDegreesGetValueName(wSet->dataSet, *wSet->values);
                add_assoc_string(return_value, propertyName, valueString, 1);
            } else if (valueCount > 1) {
                zval* valueArray;
                ALLOC_INIT_ZVAL(valueArray);
                array_init(valueArray);
                add_assoc_zval(return_value, propertyName, valueArray);
                int valueIndex;
                for(valueIndex = 0; valueIndex < valueCount; valueIndex++) {
                    char *valueString = fiftyoneDegreesGetValueName(wSet->dataSet, *(wSet->values + valueIndex));
                    add_next_index_string(valueArray, valueString, 1);
                }
            }
        }

        //Add signature rank information.
        int32_t sigRank = getSignatureRank(wSet);
        add_assoc_long(return_value, "SignatureRank", sigRank);
        add_assoc_long(return_value, "Difference", wSet->difference);

        //Add method that was used for detection to the results array.
        char* methodString;
        switch(wSet->method){
            case NONE: methodString = "None"; break;
            case EXACT: methodString = "Exact"; break;
            case NUMERIC: methodString = "Numeric"; break;
            case NEAREST: methodString = "Nearest"; break;
            case CLOSEST: methodString = "Closest"; break;
            default: methodString = "Unknown"; break;
        }
        add_assoc_string(return_value, "Method", methodString, 1);

    }
    else
    {
        php_error(E_WARNING, "Could not parse arguments.");
    }
}

/**
 * Function creates a new workset for each request and does not care for the
 * existence of pool or cache. Deprecated and should not be used.
 * @deprecated instead use fiftyone_match or
 *             fiftyone_match_with_headers.
 */
PHP_FUNCTION(fiftyone_degrees_get_properties)
{
    //Stop if there was a problem initialising the dataset.
    if (datasetInitSuccess() == 0)
    {
        php_printf("Dataset initialisation failed.");
        return;
    }

    char* useragent;
    int useragent_len;

    if (zend_parse_parameters(ZEND_NUM_ARGS() TSRMLS_CC, "s", &useragent, &useragent_len) == SUCCESS) {
        //Create a new workset.
        fiftyoneDegreesWorkset* ws = NULL;
        ws = fiftyoneDegreesCreateWorkset(dataSet);

        if (ws != NULL) {
            array_init(return_value);
            fiftyoneDegreesMatch(ws, useragent);
            int propertyIndex;
            for(propertyIndex = 0; propertyIndex < ws->dataSet->requiredPropertyCount; propertyIndex++) {
                char* propertyName = fiftyoneDegreesGetPropertyName(ws->dataSet, *(ws->dataSet->requiredProperties + propertyIndex));
                int valueCount = fiftyoneDegreesSetValues(ws, propertyIndex);
                if (valueCount == 1) {
                    char *valueString = fiftyoneDegreesGetValueName(ws->dataSet, *ws->values);
                    add_assoc_string(return_value, propertyName, valueString, 1);
                } else if (valueCount > 1) {
                    zval* valueArray;
                    ALLOC_INIT_ZVAL(valueArray);
                    array_init(valueArray);
                    add_assoc_zval(return_value, propertyName, valueArray);
                    int valueIndex;
                    for(valueIndex = 0; valueIndex < valueCount; valueIndex++) {
                        char *valueString = fiftyoneDegreesGetValueName(ws->dataSet, *(ws->values + valueIndex));
                        add_next_index_string(valueArray, valueString, 1);
                    }
                }
            }

            int32_t sigRank = getSignatureRank(ws);
            add_assoc_long(return_value, "SignatureRank", sigRank);
            add_assoc_long(return_value, "Difference", ws->difference);

            char* methodString;
            switch(ws->method){
                case NONE: methodString = "None"; break;
                case EXACT: methodString = "Exact"; break;
                case NUMERIC: methodString = "Numeric"; break;
                case NEAREST: methodString = "Nearest"; break;
                case CLOSEST: methodString = "Closest"; break;
                default: methodString = "Unknown"; break;
            }

            add_assoc_string(return_value, "Method", methodString, 1);
            fiftyoneDegreesFreeWorkset(ws);
        } else {
            php_error(E_WARNING, "The workset could not be initialised.");
        }
    } else {
        php_error(E_WARNING, "Could not parse arguments.");
    }
}

/* Support functions */

/**
 * Function returns the rank of the signature that was matched to user agent.
 * @return integer representing signature rank.
 */
int32_t getSignatureRank(fiftyoneDegreesWorkset *ws) {
	int32_t rank;
	int32_t signatureIndex = (int32_t)(ws->signature - ws->dataSet->signatures) / ws->dataSet->sizeOfSignature;
	for (rank = 0; rank < ws->dataSet->header.signatures.count; rank++) {
		if (ws->dataSet->rankedSignatureIndexes[rank] == signatureIndex) {
			return rank;
		}
	}
	return INT32_MAX;
}

/**
 * Function examines the DataSet init status and returns 0 if there was a
 * problem or 1 otherwise.
 * @return 1 if dataset successfully initialised, 0 otherwise.
 */
int datasetInitSuccess()
{
    switch (initStatus) {
    case DATA_SET_INIT_STATUS_INSUFFICIENT_MEMORY:
          php_error(E_WARNING, "51Degrees data set could not be created as not enough memory could be allocated.");
          return 0;
    case DATA_SET_INIT_STATUS_CORRUPT_DATA:
          php_error(E_WARNING, "51Degrees data set could not be created as a corrupt data file has been provided. Check it is uncompressed.");
          return 0;
    case DATA_SET_INIT_STATUS_INCORRECT_VERSION:
          php_error(E_WARNING, "51Degrees data set could not be created as the data file is an unsupported version. Check you have the latest version of the data and api.");
          return 0;
    case DATA_SET_INIT_STATUS_FILE_NOT_FOUND:
          php_error(E_WARNING, "51Degrees data set could not be created as a data file could not be found.");
          return 0;
    case DATA_SET_INIT_STATUS_SUCCESS: // do nothing
        return 1;
    }
}

/* PHP Zend init and shutdown functions */

/**
 * This section is responsible for loading settigns from the php.ini file.
 */
PHP_INI_BEGIN()
PHP_INI_ENTRY("fiftyone_degrees.data_file", "/usr/lib/php5/51Degrees-Lite.dat", PHP_INI_ALL, NULL)
PHP_INI_ENTRY("fiftyone_degrees.number_worksets", "10", PHP_INI_ALL, NULL)
PHP_INI_ENTRY("fiftyone_degrees.cache_size", "100", PHP_INI_ALL, NULL)
PHP_INI_END()

/**
 * Module startup function. All fo the resources with long lifespan get
 * initialised here. First memory allocation takes place and a new dataset
 * structure gets created. Then cach and pool get created. Cache is used
 * to speed up detections by storing the most frequent requests in memory.
 * The pool holds several Workset structures which are used for device
 * detection.
 */
PHP_MINIT_FUNCTION(fiftyone_degrees_detector_init)
{
    REGISTER_INI_ENTRIES();
    char* dataFilePath = INI_STR("fiftyone_degrees.data_file");
    int number_worksets = INI_INT("fiftyone_degrees.number_worksets");
    int cache_size = INI_INT("fiftyone_degrees.cache_size");
    dataSet = (fiftyoneDegreesDataSet*)malloc(sizeof(fiftyoneDegreesDataSet));
    initStatus = fiftyoneDegreesInitWithPropertyString((char*)dataFilePath, dataSet, NULL);
    cache = fiftyoneDegreesResultsetCacheCreate(dataSet, cache_size);
    pool = fiftyoneDegreesWorksetPoolCreate(dataSet, cache, number_worksets);
    return SUCCESS;
}

/**
 * Module shutdown function. Used to free all of the resources with
 * long lifespan.
 */
PHP_MSHUTDOWN_FUNCTION(fiftyone_degrees_detector_shutdown)
{
  UNREGISTER_INI_ENTRIES();
  if (initStatus == SUCCESS) {
    fiftyoneDegreesWorksetPoolFree(pool);
    fiftyoneDegreesResultsetCacheFree(cache);
    fiftyoneDegreesDataSetFree(dataSet);
  }
  return SUCCESS;
}

/**
 * Request startup function. Called upon each request.
 */
PHP_RINIT_FUNCTION(fiftyone_degrees_request_init) {
    wSet = fiftyoneDegreesWorksetPoolGet(pool);
    return SUCCESS;
}

/**
 * Request shutdown function. Called at the end of each request.
 */
PHP_RSHUTDOWN_FUNCTION(fiftyone_degrees_request_shutdown) {
    fiftyoneDegreesWorksetPoolRelease(pool, wSet);
    return SUCCESS;
}

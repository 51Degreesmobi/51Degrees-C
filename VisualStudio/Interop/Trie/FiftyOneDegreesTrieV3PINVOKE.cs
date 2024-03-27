//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (https://www.swig.org).
// Version 4.1.1
//
// Do not make changes to this file unless you know what you are doing - modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace FiftyOne.Mobile.Detection.Provider.Interop.Trie {

class FiftyOneDegreesTrieV3PINVOKE {

  protected class SWIGExceptionHelper {

    public delegate void ExceptionDelegate(string message);
    public delegate void ExceptionArgumentDelegate(string message, string paramName);

    static ExceptionDelegate applicationDelegate = new ExceptionDelegate(SetPendingApplicationException);
    static ExceptionDelegate arithmeticDelegate = new ExceptionDelegate(SetPendingArithmeticException);
    static ExceptionDelegate divideByZeroDelegate = new ExceptionDelegate(SetPendingDivideByZeroException);
    static ExceptionDelegate indexOutOfRangeDelegate = new ExceptionDelegate(SetPendingIndexOutOfRangeException);
    static ExceptionDelegate invalidCastDelegate = new ExceptionDelegate(SetPendingInvalidCastException);
    static ExceptionDelegate invalidOperationDelegate = new ExceptionDelegate(SetPendingInvalidOperationException);
    static ExceptionDelegate ioDelegate = new ExceptionDelegate(SetPendingIOException);
    static ExceptionDelegate nullReferenceDelegate = new ExceptionDelegate(SetPendingNullReferenceException);
    static ExceptionDelegate outOfMemoryDelegate = new ExceptionDelegate(SetPendingOutOfMemoryException);
    static ExceptionDelegate overflowDelegate = new ExceptionDelegate(SetPendingOverflowException);
    static ExceptionDelegate systemDelegate = new ExceptionDelegate(SetPendingSystemException);

    static ExceptionArgumentDelegate argumentDelegate = new ExceptionArgumentDelegate(SetPendingArgumentException);
    static ExceptionArgumentDelegate argumentNullDelegate = new ExceptionArgumentDelegate(SetPendingArgumentNullException);
    static ExceptionArgumentDelegate argumentOutOfRangeDelegate = new ExceptionArgumentDelegate(SetPendingArgumentOutOfRangeException);

    [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="SWIGRegisterExceptionCallbacks_FiftyOneDegreesTrieV3")]
    public static extern void SWIGRegisterExceptionCallbacks_FiftyOneDegreesTrieV3(
                                ExceptionDelegate applicationDelegate,
                                ExceptionDelegate arithmeticDelegate,
                                ExceptionDelegate divideByZeroDelegate, 
                                ExceptionDelegate indexOutOfRangeDelegate, 
                                ExceptionDelegate invalidCastDelegate,
                                ExceptionDelegate invalidOperationDelegate,
                                ExceptionDelegate ioDelegate,
                                ExceptionDelegate nullReferenceDelegate,
                                ExceptionDelegate outOfMemoryDelegate, 
                                ExceptionDelegate overflowDelegate, 
                                ExceptionDelegate systemExceptionDelegate);

    [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_FiftyOneDegreesTrieV3")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_FiftyOneDegreesTrieV3(
                                ExceptionArgumentDelegate argumentDelegate,
                                ExceptionArgumentDelegate argumentNullDelegate,
                                ExceptionArgumentDelegate argumentOutOfRangeDelegate);

    static void SetPendingApplicationException(string message) {
      SWIGPendingException.Set(new global::System.ApplicationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArithmeticException(string message) {
      SWIGPendingException.Set(new global::System.ArithmeticException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingDivideByZeroException(string message) {
      SWIGPendingException.Set(new global::System.DivideByZeroException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIndexOutOfRangeException(string message) {
      SWIGPendingException.Set(new global::System.IndexOutOfRangeException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidCastException(string message) {
      SWIGPendingException.Set(new global::System.InvalidCastException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingInvalidOperationException(string message) {
      SWIGPendingException.Set(new global::System.InvalidOperationException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingIOException(string message) {
      SWIGPendingException.Set(new global::System.IO.IOException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingNullReferenceException(string message) {
      SWIGPendingException.Set(new global::System.NullReferenceException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOutOfMemoryException(string message) {
      SWIGPendingException.Set(new global::System.OutOfMemoryException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingOverflowException(string message) {
      SWIGPendingException.Set(new global::System.OverflowException(message, SWIGPendingException.Retrieve()));
    }
    static void SetPendingSystemException(string message) {
      SWIGPendingException.Set(new global::System.SystemException(message, SWIGPendingException.Retrieve()));
    }

    static void SetPendingArgumentException(string message, string paramName) {
      SWIGPendingException.Set(new global::System.ArgumentException(message, paramName, SWIGPendingException.Retrieve()));
    }
    static void SetPendingArgumentNullException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentNullException(paramName, message));
    }
    static void SetPendingArgumentOutOfRangeException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentOutOfRangeException(paramName, message));
    }

    static SWIGExceptionHelper() {
      SWIGRegisterExceptionCallbacks_FiftyOneDegreesTrieV3(
                                applicationDelegate,
                                arithmeticDelegate,
                                divideByZeroDelegate,
                                indexOutOfRangeDelegate,
                                invalidCastDelegate,
                                invalidOperationDelegate,
                                ioDelegate,
                                nullReferenceDelegate,
                                outOfMemoryDelegate,
                                overflowDelegate,
                                systemDelegate);

      SWIGRegisterExceptionCallbacksArgument_FiftyOneDegreesTrieV3(
                                argumentDelegate,
                                argumentNullDelegate,
                                argumentOutOfRangeDelegate);
    }
  }

  protected static SWIGExceptionHelper swigExceptionHelper = new SWIGExceptionHelper();

  public class SWIGPendingException {
    [global::System.ThreadStatic]
    private static global::System.Exception pendingException = null;
    private static int numExceptionsPending = 0;
    private static global::System.Object exceptionsLock = null;

    public static bool Pending {
      get {
        bool pending = false;
        if (numExceptionsPending > 0)
          if (pendingException != null)
            pending = true;
        return pending;
      } 
    }

    public static void Set(global::System.Exception e) {
      if (pendingException != null)
        throw new global::System.ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + pendingException.ToString() + ")", e);
      pendingException = e;
      lock(exceptionsLock) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(exceptionsLock) {
            numExceptionsPending--;
          }
        }
      }
      return e;
    }

    static SWIGPendingException() {
      exceptionsLock = new global::System.Object();
    }
  }


  protected class SWIGStringHelper {

    public delegate string SWIGStringDelegate(string message);
    static SWIGStringDelegate stringDelegate = new SWIGStringDelegate(CreateString);

    [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="SWIGRegisterStringCallback_FiftyOneDegreesTrieV3")]
    public static extern void SWIGRegisterStringCallback_FiftyOneDegreesTrieV3(SWIGStringDelegate stringDelegate);

    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_FiftyOneDegreesTrieV3(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static FiftyOneDegreesTrieV3PINVOKE() {
  }


  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_MapStringString__SWIG_0___")]
  public static extern global::System.IntPtr new_MapStringString__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_MapStringString__SWIG_1___")]
  public static extern global::System.IntPtr new_MapStringString__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_size___")]
  public static extern uint MapStringString_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_empty___")]
  public static extern bool MapStringString_empty(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_Clear___")]
  public static extern void MapStringString_Clear(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_getitem___")]
  public static extern string MapStringString_getitem(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_setitem___")]
  public static extern void MapStringString_setitem(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, string jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_ContainsKey___")]
  public static extern bool MapStringString_ContainsKey(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_Add___")]
  public static extern void MapStringString_Add(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, string jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_Remove___")]
  public static extern bool MapStringString_Remove(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_create_iterator_begin___")]
  public static extern global::System.IntPtr MapStringString_create_iterator_begin(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_get_next_key___")]
  public static extern string MapStringString_get_next_key(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.IntPtr jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_MapStringString_destroy_iterator___")]
  public static extern void MapStringString_destroy_iterator(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.IntPtr jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_delete_MapStringString___")]
  public static extern void delete_MapStringString(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Clear___")]
  public static extern void VectorString_Clear(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Add___")]
  public static extern void VectorString_Add(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_size___")]
  public static extern uint VectorString_size(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_capacity___")]
  public static extern uint VectorString_capacity(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_reserve___")]
  public static extern void VectorString_reserve(global::System.Runtime.InteropServices.HandleRef jarg1, uint jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_VectorString__SWIG_0___")]
  public static extern global::System.IntPtr new_VectorString__SWIG_0();

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_VectorString__SWIG_1___")]
  public static extern global::System.IntPtr new_VectorString__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_VectorString__SWIG_2___")]
  public static extern global::System.IntPtr new_VectorString__SWIG_2(int jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_getitemcopy___")]
  public static extern string VectorString_getitemcopy(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_getitem___")]
  public static extern string VectorString_getitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_setitem___")]
  public static extern void VectorString_setitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, string jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_AddRange___")]
  public static extern void VectorString_AddRange(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_GetRange___")]
  public static extern global::System.IntPtr VectorString_GetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Insert___")]
  public static extern void VectorString_Insert(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, string jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_InsertRange___")]
  public static extern void VectorString_InsertRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_RemoveAt___")]
  public static extern void VectorString_RemoveAt(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_RemoveRange___")]
  public static extern void VectorString_RemoveRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Repeat___")]
  public static extern global::System.IntPtr VectorString_Repeat(string jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Reverse__SWIG_0___")]
  public static extern void VectorString_Reverse__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Reverse__SWIG_1___")]
  public static extern void VectorString_Reverse__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_SetRange___")]
  public static extern void VectorString_SetRange(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, global::System.Runtime.InteropServices.HandleRef jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Contains___")]
  public static extern bool VectorString_Contains(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_IndexOf___")]
  public static extern int VectorString_IndexOf(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_LastIndexOf___")]
  public static extern int VectorString_LastIndexOf(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_VectorString_Remove___")]
  public static extern bool VectorString_Remove(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_delete_VectorString___")]
  public static extern void delete_VectorString(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_delete_Match___")]
  public static extern void delete_Match(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValues__SWIG_0___")]
  public static extern global::System.IntPtr Match_getValues__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValues__SWIG_1___")]
  public static extern global::System.IntPtr Match_getValues__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValue__SWIG_0___")]
  public static extern string Match_getValue__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValue__SWIG_1___")]
  public static extern string Match_getValue__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsBool__SWIG_0___")]
  public static extern bool Match_getValueAsBool__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsBool__SWIG_1___")]
  public static extern bool Match_getValueAsBool__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsInteger__SWIG_0___")]
  public static extern int Match_getValueAsInteger__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsInteger__SWIG_1___")]
  public static extern int Match_getValueAsInteger__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsDouble__SWIG_0___")]
  public static extern double Match_getValueAsDouble__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getValueAsDouble__SWIG_1___")]
  public static extern double Match_getValueAsDouble__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getDeviceId___")]
  public static extern string Match_getDeviceId(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getRank___")]
  public static extern int Match_getRank(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getDifference___")]
  public static extern int Match_getDifference(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getMethod___")]
  public static extern int Match_getMethod(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Match_getUserAgent___")]
  public static extern string Match_getUserAgent(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_Provider__SWIG_0___")]
  public static extern global::System.IntPtr new_Provider__SWIG_0(string jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_Provider__SWIG_1___")]
  public static extern global::System.IntPtr new_Provider__SWIG_1(string jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_Provider__SWIG_2___")]
  public static extern global::System.IntPtr new_Provider__SWIG_2(string jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_delete_Provider___")]
  public static extern void delete_Provider(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getHttpHeaders___")]
  public static extern global::System.IntPtr Provider_getHttpHeaders(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getAvailableProperties___")]
  public static extern global::System.IntPtr Provider_getAvailableProperties(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetName___")]
  public static extern string Provider_getDataSetName(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetFormat___")]
  public static extern string Provider_getDataSetFormat(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetPublishedDate___")]
  public static extern string Provider_getDataSetPublishedDate(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetNextUpdateDate___")]
  public static extern string Provider_getDataSetNextUpdateDate(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetSignatureCount___")]
  public static extern int Provider_getDataSetSignatureCount(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getDataSetDeviceCombinations___")]
  public static extern int Provider_getDataSetDeviceCombinations(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatch__SWIG_0___")]
  public static extern global::System.IntPtr Provider_getMatch__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatch__SWIG_1___")]
  public static extern global::System.IntPtr Provider_getMatch__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatchWithTolerances__SWIG_0___")]
  public static extern global::System.IntPtr Provider_getMatchWithTolerances__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, int jarg3, int jarg4);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatchWithTolerances__SWIG_1___")]
  public static extern global::System.IntPtr Provider_getMatchWithTolerances__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, int jarg3, int jarg4);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatchJson__SWIG_0___")]
  public static extern string Provider_getMatchJson__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getMatchJson__SWIG_1___")]
  public static extern string Provider_getMatchJson__SWIG_1(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_setDrift___")]
  public static extern void Provider_setDrift(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_setDifference___")]
  public static extern void Provider_setDifference(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_reloadFromFile___")]
  public static extern void Provider_reloadFromFile(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_reloadFromMemory___")]
  public static extern void Provider_reloadFromMemory(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, int jarg3);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_Provider_getIsThreadSafe___")]
  public static extern bool Provider_getIsThreadSafe(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("FiftyOne.Mobile.Detection.Provider.Trie.dll", EntryPoint="CSharp_FiftyOnefMobilefDetectionfProviderfInteropfTrie_new_Provider__SWIG_3___")]
  public static extern global::System.IntPtr new_Provider__SWIG_3(string jarg1, string jarg2, bool jarg3);
}

}

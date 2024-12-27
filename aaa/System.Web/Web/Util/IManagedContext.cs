using System;
using System.Runtime.InteropServices;

namespace System.Web.Util
{
	// Token: 0x02000756 RID: 1878
	[Guid("a1cca730-0e36-4870-aa7d-ca39c211f99d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IManagedContext
	{
		// Token: 0x06005AFE RID: 23294
		[return: MarshalAs(UnmanagedType.I4)]
		int Context_IsPresent();

		// Token: 0x06005AFF RID: 23295
		void Application_Lock();

		// Token: 0x06005B00 RID: 23296
		void Application_UnLock();

		// Token: 0x06005B01 RID: 23297
		[return: MarshalAs(UnmanagedType.BStr)]
		string Application_GetContentsNames();

		// Token: 0x06005B02 RID: 23298
		[return: MarshalAs(UnmanagedType.BStr)]
		string Application_GetStaticNames();

		// Token: 0x06005B03 RID: 23299
		object Application_GetContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B04 RID: 23300
		void Application_SetContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] object obj);

		// Token: 0x06005B05 RID: 23301
		void Application_RemoveContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B06 RID: 23302
		void Application_RemoveAllContentsObjects();

		// Token: 0x06005B07 RID: 23303
		object Application_GetStaticObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B08 RID: 23304
		[return: MarshalAs(UnmanagedType.BStr)]
		string Request_GetAsString([MarshalAs(UnmanagedType.I4)] [In] int what);

		// Token: 0x06005B09 RID: 23305
		[return: MarshalAs(UnmanagedType.BStr)]
		string Request_GetCookiesAsString();

		// Token: 0x06005B0A RID: 23306
		[return: MarshalAs(UnmanagedType.I4)]
		int Request_GetTotalBytes();

		// Token: 0x06005B0B RID: 23307
		[return: MarshalAs(UnmanagedType.I4)]
		int Request_BinaryRead([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] bytes, int size);

		// Token: 0x06005B0C RID: 23308
		[return: MarshalAs(UnmanagedType.BStr)]
		string Response_GetCookiesAsString();

		// Token: 0x06005B0D RID: 23309
		void Response_AddCookie([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B0E RID: 23310
		void Response_SetCookieText([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string text);

		// Token: 0x06005B0F RID: 23311
		void Response_SetCookieSubValue([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string key, [MarshalAs(UnmanagedType.LPWStr)] [In] string value);

		// Token: 0x06005B10 RID: 23312
		void Response_SetCookieExpires([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.R8)] [In] double dtExpires);

		// Token: 0x06005B11 RID: 23313
		void Response_SetCookieDomain([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string domain);

		// Token: 0x06005B12 RID: 23314
		void Response_SetCookiePath([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string path);

		// Token: 0x06005B13 RID: 23315
		void Response_SetCookieSecure([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.I4)] [In] int secure);

		// Token: 0x06005B14 RID: 23316
		void Response_Write([MarshalAs(UnmanagedType.LPWStr)] [In] string text);

		// Token: 0x06005B15 RID: 23317
		void Response_BinaryWrite([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] byte[] bytes, int size);

		// Token: 0x06005B16 RID: 23318
		void Response_Redirect([MarshalAs(UnmanagedType.LPWStr)] [In] string url);

		// Token: 0x06005B17 RID: 23319
		void Response_AddHeader([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string value);

		// Token: 0x06005B18 RID: 23320
		void Response_Pics([MarshalAs(UnmanagedType.LPWStr)] [In] string value);

		// Token: 0x06005B19 RID: 23321
		void Response_Clear();

		// Token: 0x06005B1A RID: 23322
		void Response_Flush();

		// Token: 0x06005B1B RID: 23323
		void Response_End();

		// Token: 0x06005B1C RID: 23324
		void Response_AppendToLog([MarshalAs(UnmanagedType.LPWStr)] [In] string entry);

		// Token: 0x06005B1D RID: 23325
		[return: MarshalAs(UnmanagedType.BStr)]
		string Response_GetContentType();

		// Token: 0x06005B1E RID: 23326
		void Response_SetContentType([MarshalAs(UnmanagedType.LPWStr)] [In] string contentType);

		// Token: 0x06005B1F RID: 23327
		[return: MarshalAs(UnmanagedType.BStr)]
		string Response_GetCharSet();

		// Token: 0x06005B20 RID: 23328
		void Response_SetCharSet([MarshalAs(UnmanagedType.LPWStr)] [In] string charSet);

		// Token: 0x06005B21 RID: 23329
		[return: MarshalAs(UnmanagedType.BStr)]
		string Response_GetCacheControl();

		// Token: 0x06005B22 RID: 23330
		void Response_SetCacheControl([MarshalAs(UnmanagedType.LPWStr)] [In] string cacheControl);

		// Token: 0x06005B23 RID: 23331
		[return: MarshalAs(UnmanagedType.BStr)]
		string Response_GetStatus();

		// Token: 0x06005B24 RID: 23332
		void Response_SetStatus([MarshalAs(UnmanagedType.LPWStr)] [In] string status);

		// Token: 0x06005B25 RID: 23333
		[return: MarshalAs(UnmanagedType.I4)]
		int Response_GetExpiresMinutes();

		// Token: 0x06005B26 RID: 23334
		void Response_SetExpiresMinutes([MarshalAs(UnmanagedType.I4)] [In] int expiresMinutes);

		// Token: 0x06005B27 RID: 23335
		[return: MarshalAs(UnmanagedType.R8)]
		double Response_GetExpiresAbsolute();

		// Token: 0x06005B28 RID: 23336
		void Response_SetExpiresAbsolute([MarshalAs(UnmanagedType.R8)] [In] double dtExpires);

		// Token: 0x06005B29 RID: 23337
		[return: MarshalAs(UnmanagedType.I4)]
		int Response_GetIsBuffering();

		// Token: 0x06005B2A RID: 23338
		void Response_SetIsBuffering([MarshalAs(UnmanagedType.I4)] [In] int isBuffering);

		// Token: 0x06005B2B RID: 23339
		[return: MarshalAs(UnmanagedType.I4)]
		int Response_IsClientConnected();

		// Token: 0x06005B2C RID: 23340
		[return: MarshalAs(UnmanagedType.Interface)]
		object Server_CreateObject([MarshalAs(UnmanagedType.LPWStr)] [In] string progId);

		// Token: 0x06005B2D RID: 23341
		[return: MarshalAs(UnmanagedType.BStr)]
		string Server_MapPath([MarshalAs(UnmanagedType.LPWStr)] [In] string logicalPath);

		// Token: 0x06005B2E RID: 23342
		[return: MarshalAs(UnmanagedType.BStr)]
		string Server_HTMLEncode([MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x06005B2F RID: 23343
		[return: MarshalAs(UnmanagedType.BStr)]
		string Server_URLEncode([MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x06005B30 RID: 23344
		[return: MarshalAs(UnmanagedType.BStr)]
		string Server_URLPathEncode([MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x06005B31 RID: 23345
		[return: MarshalAs(UnmanagedType.I4)]
		int Server_GetScriptTimeout();

		// Token: 0x06005B32 RID: 23346
		void Server_SetScriptTimeout([MarshalAs(UnmanagedType.I4)] [In] int timeoutSeconds);

		// Token: 0x06005B33 RID: 23347
		void Server_Execute([MarshalAs(UnmanagedType.LPWStr)] [In] string url);

		// Token: 0x06005B34 RID: 23348
		void Server_Transfer([MarshalAs(UnmanagedType.LPWStr)] [In] string url);

		// Token: 0x06005B35 RID: 23349
		[return: MarshalAs(UnmanagedType.I4)]
		int Session_IsPresent();

		// Token: 0x06005B36 RID: 23350
		[return: MarshalAs(UnmanagedType.BStr)]
		string Session_GetID();

		// Token: 0x06005B37 RID: 23351
		[return: MarshalAs(UnmanagedType.I4)]
		int Session_GetTimeout();

		// Token: 0x06005B38 RID: 23352
		void Session_SetTimeout([MarshalAs(UnmanagedType.I4)] [In] int value);

		// Token: 0x06005B39 RID: 23353
		[return: MarshalAs(UnmanagedType.I4)]
		int Session_GetCodePage();

		// Token: 0x06005B3A RID: 23354
		void Session_SetCodePage([MarshalAs(UnmanagedType.I4)] [In] int value);

		// Token: 0x06005B3B RID: 23355
		[return: MarshalAs(UnmanagedType.I4)]
		int Session_GetLCID();

		// Token: 0x06005B3C RID: 23356
		void Session_SetLCID([MarshalAs(UnmanagedType.I4)] [In] int value);

		// Token: 0x06005B3D RID: 23357
		void Session_Abandon();

		// Token: 0x06005B3E RID: 23358
		[return: MarshalAs(UnmanagedType.BStr)]
		string Session_GetContentsNames();

		// Token: 0x06005B3F RID: 23359
		[return: MarshalAs(UnmanagedType.BStr)]
		string Session_GetStaticNames();

		// Token: 0x06005B40 RID: 23360
		object Session_GetContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B41 RID: 23361
		void Session_SetContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [In] object obj);

		// Token: 0x06005B42 RID: 23362
		void Session_RemoveContentsObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		// Token: 0x06005B43 RID: 23363
		void Session_RemoveAllContentsObjects();

		// Token: 0x06005B44 RID: 23364
		object Session_GetStaticObject([MarshalAs(UnmanagedType.LPWStr)] [In] string name);
	}
}

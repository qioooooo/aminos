using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200009D RID: 157
	[Guid("9556DC99-828C-11CF-A37E-00AA003240C7")]
	[InterfaceType(1)]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemServices_Old
	{
		// Token: 0x06000476 RID: 1142
		[PreserveSig]
		int OpenNamespace_([MarshalAs(UnmanagedType.BStr)] [In] string strNamespace, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemServices ppWorkingNamespace, [In] IntPtr ppCallResult);

		// Token: 0x06000477 RID: 1143
		[PreserveSig]
		int CancelAsyncCall_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink);

		// Token: 0x06000478 RID: 1144
		[PreserveSig]
		int QueryObjectSink_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemObjectSink ppResponseHandler);

		// Token: 0x06000479 RID: 1145
		[PreserveSig]
		int GetObject_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppObject, [In] IntPtr ppCallResult);

		// Token: 0x0600047A RID: 1146
		[PreserveSig]
		int GetObjectAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x0600047B RID: 1147
		[PreserveSig]
		int PutClass_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x0600047C RID: 1148
		[PreserveSig]
		int PutClassAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x0600047D RID: 1149
		[PreserveSig]
		int DeleteClass_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x0600047E RID: 1150
		[PreserveSig]
		int DeleteClassAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x0600047F RID: 1151
		[PreserveSig]
		int CreateClassEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x06000480 RID: 1152
		[PreserveSig]
		int CreateClassEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x06000481 RID: 1153
		[PreserveSig]
		int PutInstance_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x06000482 RID: 1154
		[PreserveSig]
		int PutInstanceAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x06000483 RID: 1155
		[PreserveSig]
		int DeleteInstance_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x06000484 RID: 1156
		[PreserveSig]
		int DeleteInstanceAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x06000485 RID: 1157
		[PreserveSig]
		int CreateInstanceEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x06000486 RID: 1158
		[PreserveSig]
		int CreateInstanceEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x06000487 RID: 1159
		[PreserveSig]
		int ExecQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x06000488 RID: 1160
		[PreserveSig]
		int ExecQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x06000489 RID: 1161
		[PreserveSig]
		int ExecNotificationQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x0600048A RID: 1162
		[PreserveSig]
		int ExecNotificationQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x0600048B RID: 1163
		[PreserveSig]
		int ExecMethod_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutParams, [In] IntPtr ppCallResult);

		// Token: 0x0600048C RID: 1164
		[PreserveSig]
		int ExecMethodAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000041 RID: 65
	internal static class WmiNetUtilsHelper
	{
		// Token: 0x06000255 RID: 597
		[DllImport("kernel32.dll")]
		internal static extern IntPtr LoadLibrary(string fileName);

		// Token: 0x06000256 RID: 598
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

		// Token: 0x06000257 RID: 599 RVA: 0x0000C428 File Offset: 0x0000B428
		static WmiNetUtilsHelper()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			intPtr2 = WmiNetUtilsHelper.LoadLibrary(WmiNetUtilsHelper.myDllPath);
			if (intPtr2 != IntPtr.Zero)
			{
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "ResetSecurity");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.ResetSecurity_f = (WmiNetUtilsHelper.ResetSecurity)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.ResetSecurity));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "SetSecurity");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.SetSecurity_f = (WmiNetUtilsHelper.SetSecurity)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.SetSecurity));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "BlessIWbemServices");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.BlessIWbemServices_f = (WmiNetUtilsHelper.BlessIWbemServices)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.BlessIWbemServices));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "BlessIWbemServicesObject");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.BlessIWbemServicesObject_f = (WmiNetUtilsHelper.BlessIWbemServicesObject)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.BlessIWbemServicesObject));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetPropertyHandle");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetPropertyHandle_f27 = (WmiNetUtilsHelper.GetPropertyHandle)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetPropertyHandle));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "WritePropertyValue");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.WritePropertyValue_f28 = (WmiNetUtilsHelper.WritePropertyValue)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.WritePropertyValue));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Clone");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Clone_f12 = (WmiNetUtilsHelper.Clone)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Clone));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "VerifyClientKey");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.VerifyClientKey_f = (WmiNetUtilsHelper.VerifyClientKey)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.VerifyClientKey));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetQualifierSet");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetQualifierSet_f = (WmiNetUtilsHelper.GetQualifierSet)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetQualifierSet));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Get");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Get_f = (WmiNetUtilsHelper.Get)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Get));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Put");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Put_f = (WmiNetUtilsHelper.Put)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Put));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Delete");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Delete_f = (WmiNetUtilsHelper.Delete)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Delete));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetNames");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetNames_f = (WmiNetUtilsHelper.GetNames)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetNames));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "BeginEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.BeginEnumeration_f = (WmiNetUtilsHelper.BeginEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.BeginEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Next");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Next_f = (WmiNetUtilsHelper.Next)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Next));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "EndEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.EndEnumeration_f = (WmiNetUtilsHelper.EndEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.EndEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetPropertyQualifierSet");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetPropertyQualifierSet_f = (WmiNetUtilsHelper.GetPropertyQualifierSet)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetPropertyQualifierSet));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Clone");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Clone_f = (WmiNetUtilsHelper.Clone)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Clone));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetObjectText");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetObjectText_f = (WmiNetUtilsHelper.GetObjectText)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetObjectText));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "SpawnDerivedClass");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.SpawnDerivedClass_f = (WmiNetUtilsHelper.SpawnDerivedClass)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.SpawnDerivedClass));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "SpawnInstance");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.SpawnInstance_f = (WmiNetUtilsHelper.SpawnInstance)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.SpawnInstance));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "CompareTo");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.CompareTo_f = (WmiNetUtilsHelper.CompareTo)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.CompareTo));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetPropertyOrigin");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetPropertyOrigin_f = (WmiNetUtilsHelper.GetPropertyOrigin)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetPropertyOrigin));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "InheritsFrom");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.InheritsFrom_f = (WmiNetUtilsHelper.InheritsFrom)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.InheritsFrom));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetMethod");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetMethod_f = (WmiNetUtilsHelper.GetMethod)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetMethod));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "PutMethod");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.PutMethod_f = (WmiNetUtilsHelper.PutMethod)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.PutMethod));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "DeleteMethod");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.DeleteMethod_f = (WmiNetUtilsHelper.DeleteMethod)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.DeleteMethod));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "BeginMethodEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.BeginMethodEnumeration_f = (WmiNetUtilsHelper.BeginMethodEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.BeginMethodEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "NextMethod");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.NextMethod_f = (WmiNetUtilsHelper.NextMethod)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.NextMethod));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "EndMethodEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.EndMethodEnumeration_f = (WmiNetUtilsHelper.EndMethodEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.EndMethodEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetMethodQualifierSet");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetMethodQualifierSet_f = (WmiNetUtilsHelper.GetMethodQualifierSet)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetMethodQualifierSet));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetMethodOrigin");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetMethodOrigin_f = (WmiNetUtilsHelper.GetMethodOrigin)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetMethodOrigin));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_Get");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierGet_f = (WmiNetUtilsHelper.QualifierSet_Get)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_Get));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_Put");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierPut_f = (WmiNetUtilsHelper.QualifierSet_Put)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_Put));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_Delete");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierDelete_f = (WmiNetUtilsHelper.QualifierSet_Delete)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_Delete));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_GetNames");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierGetNames_f = (WmiNetUtilsHelper.QualifierSet_GetNames)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_GetNames));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_BeginEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierBeginEnumeration_f = (WmiNetUtilsHelper.QualifierSet_BeginEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_BeginEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_Next");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierNext_f = (WmiNetUtilsHelper.QualifierSet_Next)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_Next));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "QualifierSet_EndEnumeration");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.QualifierEndEnumeration_f = (WmiNetUtilsHelper.QualifierSet_EndEnumeration)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.QualifierSet_EndEnumeration));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetCurrentApartmentType");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetCurrentApartmentType_f = (WmiNetUtilsHelper.GetCurrentApartmentType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetCurrentApartmentType));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetDemultiplexedStub");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetDemultiplexedStub_f = (WmiNetUtilsHelper.GetDemultiplexedStub)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetDemultiplexedStub));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "CreateInstanceEnumWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.CreateInstanceEnumWmi_f = (WmiNetUtilsHelper.CreateInstanceEnumWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.CreateInstanceEnumWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "CreateClassEnumWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.CreateClassEnumWmi_f = (WmiNetUtilsHelper.CreateClassEnumWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.CreateClassEnumWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "ExecQueryWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.ExecQueryWmi_f = (WmiNetUtilsHelper.ExecQueryWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.ExecQueryWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "ExecNotificationQueryWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.ExecNotificationQueryWmi_f = (WmiNetUtilsHelper.ExecNotificationQueryWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.ExecNotificationQueryWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "PutInstanceWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.PutInstanceWmi_f = (WmiNetUtilsHelper.PutInstanceWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.PutInstanceWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "PutClassWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.PutClassWmi_f = (WmiNetUtilsHelper.PutClassWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.PutClassWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "CloneEnumWbemClassObject");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.CloneEnumWbemClassObject_f = (WmiNetUtilsHelper.CloneEnumWbemClassObject)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.CloneEnumWbemClassObject));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "ConnectServerWmi");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.ConnectServerWmi_f = (WmiNetUtilsHelper.ConnectServerWmi)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.ConnectServerWmi));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "GetErrorInfo");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.GetErrorInfo_f = (WmiNetUtilsHelper.GetErrorInfo)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.GetErrorInfo));
				}
				intPtr = WmiNetUtilsHelper.GetProcAddress(intPtr2, "Initialize");
				if (intPtr != IntPtr.Zero)
				{
					WmiNetUtilsHelper.Initialize_f = (WmiNetUtilsHelper.Initialize)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WmiNetUtilsHelper.Initialize));
				}
				WmiNetUtilsHelper.Initialize_f(CompatSwitches.AllowIManagementObjectQI);
			}
		}

		// Token: 0x04000186 RID: 390
		internal static string myDllPath = RuntimeEnvironment.GetRuntimeDirectory() + "\\wminet_utils.dll";

		// Token: 0x04000187 RID: 391
		internal static WmiNetUtilsHelper.ResetSecurity ResetSecurity_f;

		// Token: 0x04000188 RID: 392
		internal static WmiNetUtilsHelper.SetSecurity SetSecurity_f;

		// Token: 0x04000189 RID: 393
		internal static WmiNetUtilsHelper.BlessIWbemServices BlessIWbemServices_f;

		// Token: 0x0400018A RID: 394
		internal static WmiNetUtilsHelper.BlessIWbemServicesObject BlessIWbemServicesObject_f;

		// Token: 0x0400018B RID: 395
		internal static WmiNetUtilsHelper.GetPropertyHandle GetPropertyHandle_f27;

		// Token: 0x0400018C RID: 396
		internal static WmiNetUtilsHelper.WritePropertyValue WritePropertyValue_f28;

		// Token: 0x0400018D RID: 397
		internal static WmiNetUtilsHelper.GetQualifierSet GetQualifierSet_f;

		// Token: 0x0400018E RID: 398
		internal static WmiNetUtilsHelper.Get Get_f;

		// Token: 0x0400018F RID: 399
		internal static WmiNetUtilsHelper.Put Put_f;

		// Token: 0x04000190 RID: 400
		internal static WmiNetUtilsHelper.Delete Delete_f;

		// Token: 0x04000191 RID: 401
		internal static WmiNetUtilsHelper.GetNames GetNames_f;

		// Token: 0x04000192 RID: 402
		internal static WmiNetUtilsHelper.BeginEnumeration BeginEnumeration_f;

		// Token: 0x04000193 RID: 403
		internal static WmiNetUtilsHelper.Next Next_f;

		// Token: 0x04000194 RID: 404
		internal static WmiNetUtilsHelper.EndEnumeration EndEnumeration_f;

		// Token: 0x04000195 RID: 405
		internal static WmiNetUtilsHelper.GetPropertyQualifierSet GetPropertyQualifierSet_f;

		// Token: 0x04000196 RID: 406
		internal static WmiNetUtilsHelper.Clone Clone_f;

		// Token: 0x04000197 RID: 407
		internal static WmiNetUtilsHelper.GetObjectText GetObjectText_f;

		// Token: 0x04000198 RID: 408
		internal static WmiNetUtilsHelper.SpawnDerivedClass SpawnDerivedClass_f;

		// Token: 0x04000199 RID: 409
		internal static WmiNetUtilsHelper.SpawnInstance SpawnInstance_f;

		// Token: 0x0400019A RID: 410
		internal static WmiNetUtilsHelper.CompareTo CompareTo_f;

		// Token: 0x0400019B RID: 411
		internal static WmiNetUtilsHelper.GetPropertyOrigin GetPropertyOrigin_f;

		// Token: 0x0400019C RID: 412
		internal static WmiNetUtilsHelper.InheritsFrom InheritsFrom_f;

		// Token: 0x0400019D RID: 413
		internal static WmiNetUtilsHelper.GetMethod GetMethod_f;

		// Token: 0x0400019E RID: 414
		internal static WmiNetUtilsHelper.PutMethod PutMethod_f;

		// Token: 0x0400019F RID: 415
		internal static WmiNetUtilsHelper.DeleteMethod DeleteMethod_f;

		// Token: 0x040001A0 RID: 416
		internal static WmiNetUtilsHelper.BeginMethodEnumeration BeginMethodEnumeration_f;

		// Token: 0x040001A1 RID: 417
		internal static WmiNetUtilsHelper.NextMethod NextMethod_f;

		// Token: 0x040001A2 RID: 418
		internal static WmiNetUtilsHelper.EndMethodEnumeration EndMethodEnumeration_f;

		// Token: 0x040001A3 RID: 419
		internal static WmiNetUtilsHelper.GetMethodQualifierSet GetMethodQualifierSet_f;

		// Token: 0x040001A4 RID: 420
		internal static WmiNetUtilsHelper.GetMethodOrigin GetMethodOrigin_f;

		// Token: 0x040001A5 RID: 421
		internal static WmiNetUtilsHelper.QualifierSet_Get QualifierGet_f;

		// Token: 0x040001A6 RID: 422
		internal static WmiNetUtilsHelper.QualifierSet_Put QualifierPut_f;

		// Token: 0x040001A7 RID: 423
		internal static WmiNetUtilsHelper.QualifierSet_Delete QualifierDelete_f;

		// Token: 0x040001A8 RID: 424
		internal static WmiNetUtilsHelper.QualifierSet_GetNames QualifierGetNames_f;

		// Token: 0x040001A9 RID: 425
		internal static WmiNetUtilsHelper.QualifierSet_BeginEnumeration QualifierBeginEnumeration_f;

		// Token: 0x040001AA RID: 426
		internal static WmiNetUtilsHelper.QualifierSet_Next QualifierNext_f;

		// Token: 0x040001AB RID: 427
		internal static WmiNetUtilsHelper.QualifierSet_EndEnumeration QualifierEndEnumeration_f;

		// Token: 0x040001AC RID: 428
		internal static WmiNetUtilsHelper.GetCurrentApartmentType GetCurrentApartmentType_f;

		// Token: 0x040001AD RID: 429
		internal static WmiNetUtilsHelper.VerifyClientKey VerifyClientKey_f;

		// Token: 0x040001AE RID: 430
		internal static WmiNetUtilsHelper.Clone Clone_f12;

		// Token: 0x040001AF RID: 431
		internal static WmiNetUtilsHelper.GetDemultiplexedStub GetDemultiplexedStub_f;

		// Token: 0x040001B0 RID: 432
		internal static WmiNetUtilsHelper.CreateInstanceEnumWmi CreateInstanceEnumWmi_f;

		// Token: 0x040001B1 RID: 433
		internal static WmiNetUtilsHelper.CreateClassEnumWmi CreateClassEnumWmi_f;

		// Token: 0x040001B2 RID: 434
		internal static WmiNetUtilsHelper.ExecQueryWmi ExecQueryWmi_f;

		// Token: 0x040001B3 RID: 435
		internal static WmiNetUtilsHelper.ExecNotificationQueryWmi ExecNotificationQueryWmi_f;

		// Token: 0x040001B4 RID: 436
		internal static WmiNetUtilsHelper.PutInstanceWmi PutInstanceWmi_f;

		// Token: 0x040001B5 RID: 437
		internal static WmiNetUtilsHelper.PutClassWmi PutClassWmi_f;

		// Token: 0x040001B6 RID: 438
		internal static WmiNetUtilsHelper.CloneEnumWbemClassObject CloneEnumWbemClassObject_f;

		// Token: 0x040001B7 RID: 439
		internal static WmiNetUtilsHelper.ConnectServerWmi ConnectServerWmi_f;

		// Token: 0x040001B8 RID: 440
		internal static WmiNetUtilsHelper.GetErrorInfo GetErrorInfo_f;

		// Token: 0x040001B9 RID: 441
		internal static WmiNetUtilsHelper.Initialize Initialize_f;

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x06000259 RID: 601
		internal delegate int ResetSecurity(IntPtr hToken);

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x0600025D RID: 605
		internal delegate int SetSecurity([In] [Out] ref bool pNeedtoReset, [In] [Out] ref IntPtr pHandle);

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x06000261 RID: 609
		internal delegate int BlessIWbemServices([MarshalAs(UnmanagedType.Interface)] IWbemServices pIUnknown, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, IntPtr password, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority, int impersonationLevel, int authenticationLevel);

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x06000265 RID: 613
		internal delegate int BlessIWbemServicesObject([MarshalAs(UnmanagedType.IUnknown)] object pIUnknown, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, IntPtr password, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority, int impersonationLevel, int authenticationLevel);

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000269 RID: 617
		internal delegate int GetPropertyHandle(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszPropertyName, out int pType, out int plHandle);

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x0600026D RID: 621
		internal delegate int WritePropertyValue(int vFunc, IntPtr pWbemClassObject, [In] int lHandle, [In] int lNumBytes, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x06000271 RID: 625
		internal delegate int GetQualifierSet(int vFunc, IntPtr pWbemClassObject, out IntPtr ppQualSet);

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x06000275 RID: 629
		internal delegate int Get(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000279 RID: 633
		internal delegate int Put(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pVal, [In] int Type);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x0600027D RID: 637
		internal delegate int Delete(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x06000281 RID: 641
		internal delegate int GetNames(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQualifierName, [In] int lFlags, [In] ref object pQualifierVal, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x06000285 RID: 645
		internal delegate int BeginEnumeration(int vFunc, IntPtr pWbemClassObject, [In] int lEnumFlags);

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x06000289 RID: 649
		internal delegate int Next(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string strName, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x0600028D RID: 653
		internal delegate int EndEnumeration(int vFunc, IntPtr pWbemClassObject);

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x06000291 RID: 657
		internal delegate int GetPropertyQualifierSet(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszProperty, out IntPtr ppQualSet);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x06000295 RID: 661
		internal delegate int Clone(int vFunc, IntPtr pWbemClassObject, out IntPtr ppCopy);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x06000299 RID: 665
		internal delegate int GetObjectText(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrObjectText);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x0600029D RID: 669
		internal delegate int SpawnDerivedClass(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, out IntPtr ppNewClass);

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x060002A1 RID: 673
		internal delegate int SpawnInstance(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, out IntPtr ppNewInstance);

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x060002A5 RID: 677
		internal delegate int CompareTo(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [In] IntPtr pCompareTo);

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x060002A9 RID: 681
		internal delegate int GetPropertyOrigin(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x060002AD RID: 685
		internal delegate int InheritsFrom(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string strAncestor);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x060002B1 RID: 689
		internal delegate int GetMethod(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, out IntPtr ppInSignature, out IntPtr ppOutSignature);

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x060002B5 RID: 693
		internal delegate int PutMethod(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] IntPtr pInSignature, [In] IntPtr pOutSignature);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x060002B9 RID: 697
		internal delegate int DeleteMethod(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x060002BD RID: 701
		internal delegate int BeginMethodEnumeration(int vFunc, IntPtr pWbemClassObject, [In] int lEnumFlags);

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x060002C1 RID: 705
		internal delegate int NextMethod(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out IntPtr ppInSignature, out IntPtr ppOutSignature);

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x060002C5 RID: 709
		internal delegate int EndMethodEnumeration(int vFunc, IntPtr pWbemClassObject);

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x060002C9 RID: 713
		internal delegate int GetMethodQualifierSet(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethod, out IntPtr ppQualSet);

		// Token: 0x0200005F RID: 95
		// (Invoke) Token: 0x060002CD RID: 717
		internal delegate int GetMethodOrigin(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethodName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x060002D1 RID: 721
		internal delegate int QualifierSet_Get(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int plFlavor);

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x060002D5 RID: 725
		internal delegate int QualifierSet_Put(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] ref object pVal, [In] int lFlavor);

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x060002D9 RID: 729
		internal delegate int QualifierSet_Delete(int vFunc, IntPtr pWbemClassObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x060002DD RID: 733
		internal delegate int QualifierSet_GetNames(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x02000064 RID: 100
		// (Invoke) Token: 0x060002E1 RID: 737
		internal delegate int QualifierSet_BeginEnumeration(int vFunc, IntPtr pWbemClassObject, [In] int lFlags);

		// Token: 0x02000065 RID: 101
		// (Invoke) Token: 0x060002E5 RID: 741
		internal delegate int QualifierSet_Next(int vFunc, IntPtr pWbemClassObject, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out object pVal, out int plFlavor);

		// Token: 0x02000066 RID: 102
		// (Invoke) Token: 0x060002E9 RID: 745
		internal delegate int QualifierSet_EndEnumeration(int vFunc, IntPtr pWbemClassObject);

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x060002ED RID: 749
		internal delegate int GetCurrentApartmentType(int vFunc, IntPtr pComThreadingInfo, out WmiNetUtilsHelper.APTTYPE aptType);

		// Token: 0x02000068 RID: 104
		// (Invoke) Token: 0x060002F1 RID: 753
		internal delegate void VerifyClientKey();

		// Token: 0x02000069 RID: 105
		// (Invoke) Token: 0x060002F5 RID: 757
		internal delegate int GetDemultiplexedStub([MarshalAs(UnmanagedType.IUnknown)] [In] object pIUnknown, [In] bool isLocal, [MarshalAs(UnmanagedType.IUnknown)] out object ppIUnknown);

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x060002F9 RID: 761
		internal delegate int CreateInstanceEnumWmi([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x060002FD RID: 765
		internal delegate int CreateClassEnumWmi([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x06000301 RID: 769
		internal delegate int ExecQueryWmi([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x06000305 RID: 773
		internal delegate int ExecNotificationQueryWmi([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x06000309 RID: 777
		internal delegate int PutInstanceWmi([In] IntPtr pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x0600030D RID: 781
		internal delegate int PutClassWmi([In] IntPtr pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pCurrentNamespace, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x02000070 RID: 112
		// (Invoke) Token: 0x06000311 RID: 785
		internal delegate int CloneEnumWbemClassObject([MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum, [In] int impLevel, [In] int authnLevel, [MarshalAs(UnmanagedType.Interface)] [In] IEnumWbemClassObject pCurrentEnumWbemClassObject, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority);

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x06000315 RID: 789
		internal delegate int ConnectServerWmi([MarshalAs(UnmanagedType.BStr)] [In] string strNetworkResource, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strLocale, [In] int lSecurityFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IWbemServices ppNamespace, int impersonationLevel, int authenticationLevel);

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x06000319 RID: 793
		internal delegate IntPtr GetErrorInfo();

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x0600031D RID: 797
		internal delegate int Initialize([In] bool AllowIManagementObjectQI);

		// Token: 0x02000074 RID: 116
		internal enum APTTYPE
		{
			// Token: 0x040001BB RID: 443
			APTTYPE_CURRENT = -1,
			// Token: 0x040001BC RID: 444
			APTTYPE_STA,
			// Token: 0x040001BD RID: 445
			APTTYPE_MTA,
			// Token: 0x040001BE RID: 446
			APTTYPE_NA,
			// Token: 0x040001BF RID: 447
			APTTYPE_MAINSTA
		}
	}
}

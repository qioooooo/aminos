using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000078 RID: 120
	internal class SecurityIWbemServicesHandler
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000D708 File Offset: 0x0000C708
		internal SecurityIWbemServicesHandler(ManagementScope theScope, IWbemServices pWbemServiecs)
		{
			this.scope = theScope;
			this.pWbemServiecsSecurityHelper = pWbemServiecs;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000D720 File Offset: 0x0000C720
		internal int OpenNamespace_(string strNamespace, int lFlags, ref IWbemServices ppWorkingNamespace, IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000D734 File Offset: 0x0000C734
		internal int CancelAsyncCall_(IWbemObjectSink pSink)
		{
			return this.pWbemServiecsSecurityHelper.CancelAsyncCall_(pSink);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000D758 File Offset: 0x0000C758
		internal int QueryObjectSink_(int lFlags, ref IWbemObjectSink ppResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.QueryObjectSink_(lFlags, out ppResponseHandler);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000D77C File Offset: 0x0000C77C
		internal int GetObject_(string strObjectPath, int lFlags, IWbemContext pCtx, ref IWbemClassObjectFreeThreaded ppObject, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (!object.ReferenceEquals(ppCallResult, IntPtr.Zero))
			{
				num = this.pWbemServiecsSecurityHelper.GetObject_(strObjectPath, lFlags, pCtx, out ppObject, ppCallResult);
			}
			return num;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000D7BC File Offset: 0x0000C7BC
		internal int GetObjectAsync_(string strObjectPath, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.GetObjectAsync_(strObjectPath, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000D7E4 File Offset: 0x0000C7E4
		internal int PutClass_(IWbemClassObjectFreeThreaded pObject, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.PutClassWmi_f(pObject, lFlags, pCtx, ppCallResult, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000D874 File Offset: 0x0000C874
		internal int PutClassAsync_(IWbemClassObjectFreeThreaded pObject, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.PutClassAsync_(pObject, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000D8A0 File Offset: 0x0000C8A0
		internal int DeleteClass_(string strClass, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (!object.ReferenceEquals(ppCallResult, IntPtr.Zero))
			{
				num = this.pWbemServiecsSecurityHelper.DeleteClass_(strClass, lFlags, pCtx, ppCallResult);
			}
			return num;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000D8E0 File Offset: 0x0000C8E0
		internal int DeleteClassAsync_(string strClass, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.DeleteClassAsync_(strClass, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000D908 File Offset: 0x0000C908
		internal int CreateClassEnum_(string strSuperClass, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.CreateClassEnumWmi_f(strSuperClass, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000D994 File Offset: 0x0000C994
		internal int CreateClassEnumAsync_(string strSuperClass, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.CreateClassEnumAsync_(strSuperClass, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000D9BC File Offset: 0x0000C9BC
		internal int PutInstance_(IWbemClassObjectFreeThreaded pInst, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.PutInstanceWmi_f(pInst, lFlags, pCtx, ppCallResult, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000DA4C File Offset: 0x0000CA4C
		internal int PutInstanceAsync_(IWbemClassObjectFreeThreaded pInst, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.PutInstanceAsync_(pInst, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000DA78 File Offset: 0x0000CA78
		internal int DeleteInstance_(string strObjectPath, int lFlags, IWbemContext pCtx, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (!object.ReferenceEquals(ppCallResult, IntPtr.Zero))
			{
				num = this.pWbemServiecsSecurityHelper.DeleteInstance_(strObjectPath, lFlags, pCtx, ppCallResult);
			}
			return num;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000DAB8 File Offset: 0x0000CAB8
		internal int DeleteInstanceAsync_(string strObjectPath, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.DeleteInstanceAsync_(strObjectPath, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000DAE0 File Offset: 0x0000CAE0
		internal int CreateInstanceEnum_(string strFilter, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.CreateInstanceEnumWmi_f(strFilter, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000DB6C File Offset: 0x0000CB6C
		internal int CreateInstanceEnumAsync_(string strFilter, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.CreateInstanceEnumAsync_(strFilter, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000DB94 File Offset: 0x0000CB94
		internal int ExecQuery_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.ExecQueryWmi_f(strQueryLanguage, strQuery, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000DC20 File Offset: 0x0000CC20
		internal int ExecQueryAsync_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecQueryAsync_(strQueryLanguage, strQuery, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000DC48 File Offset: 0x0000CC48
		internal int ExecNotificationQuery_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, ref IEnumWbemClassObject ppEnum)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.ExecNotificationQueryWmi_f(strQueryLanguage, strQuery, lFlags, pCtx, out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pWbemServiecsSecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000DCD4 File Offset: 0x0000CCD4
		internal int ExecNotificationQueryAsync_(string strQueryLanguage, string strQuery, int lFlags, IWbemContext pCtx, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecNotificationQueryAsync_(strQueryLanguage, strQuery, lFlags, pCtx, pResponseHandler);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000DCFC File Offset: 0x0000CCFC
		internal int ExecMethod_(string strObjectPath, string strMethodName, int lFlags, IWbemContext pCtx, IWbemClassObjectFreeThreaded pInParams, ref IWbemClassObjectFreeThreaded ppOutParams, IntPtr ppCallResult)
		{
			int num = -2147217407;
			if (!object.ReferenceEquals(ppCallResult, IntPtr.Zero))
			{
				num = this.pWbemServiecsSecurityHelper.ExecMethod_(strObjectPath, strMethodName, lFlags, pCtx, pInParams, out ppOutParams, ppCallResult);
			}
			return num;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000DD44 File Offset: 0x0000CD44
		internal int ExecMethodAsync_(string strObjectPath, string strMethodName, int lFlags, IWbemContext pCtx, IWbemClassObjectFreeThreaded pInParams, IWbemObjectSink pResponseHandler)
		{
			return this.pWbemServiecsSecurityHelper.ExecMethodAsync_(strObjectPath, strMethodName, lFlags, pCtx, pInParams, pResponseHandler);
		}

		// Token: 0x040001C8 RID: 456
		private IWbemServices pWbemServiecsSecurityHelper;

		// Token: 0x040001C9 RID: 457
		private ManagementScope scope;
	}
}

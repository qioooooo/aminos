using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000076 RID: 118
	internal class SecuredIEnumWbemClassObjectHandler
	{
		// Token: 0x0600033F RID: 831 RVA: 0x0000D4C2 File Offset: 0x0000C4C2
		internal SecuredIEnumWbemClassObjectHandler(ManagementScope theScope, IEnumWbemClassObject pEnumWbemClassObject)
		{
			this.scope = theScope;
			this.pEnumWbemClassObjectsecurityHelper = pEnumWbemClassObject;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000D4D8 File Offset: 0x0000C4D8
		internal int Reset_()
		{
			return this.pEnumWbemClassObjectsecurityHelper.Reset_();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000D4F8 File Offset: 0x0000C4F8
		internal int Next_(int lTimeout, uint uCount, IWbemClassObject_DoNotMarshal[] ppOutParams, ref uint puReturned)
		{
			return this.pEnumWbemClassObjectsecurityHelper.Next_(lTimeout, uCount, ppOutParams, out puReturned);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000D520 File Offset: 0x0000C520
		internal int NextAsync_(uint uCount, IWbemObjectSink pSink)
		{
			return this.pEnumWbemClassObjectsecurityHelper.NextAsync_(uCount, pSink);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000D544 File Offset: 0x0000C544
		internal int Clone_(ref IEnumWbemClassObject ppEnum)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				num = WmiNetUtilsHelper.CloneEnumWbemClassObject_f(out ppEnum, (int)this.scope.Options.Authentication, (int)this.scope.Options.Impersonation, this.pEnumWbemClassObjectsecurityHelper, this.scope.Options.Username, password, this.scope.Options.Authority);
				Marshal.ZeroFreeBSTR(password);
			}
			return num;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000D5CC File Offset: 0x0000C5CC
		internal int Skip_(int lTimeout, uint nCount)
		{
			return this.pEnumWbemClassObjectsecurityHelper.Skip_(lTimeout, nCount);
		}

		// Token: 0x040001C5 RID: 453
		private IEnumWbemClassObject pEnumWbemClassObjectsecurityHelper;

		// Token: 0x040001C6 RID: 454
		private ManagementScope scope;
	}
}

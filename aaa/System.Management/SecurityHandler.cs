using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000079 RID: 121
	internal class SecurityHandler
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000DD72 File Offset: 0x0000CD72
		internal SecurityHandler(ManagementScope theScope)
		{
			this.scope = theScope;
			if (this.scope != null && this.scope.Options.EnablePrivileges)
			{
				WmiNetUtilsHelper.SetSecurity_f(ref this.needToReset, ref this.handle);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000DDB2 File Offset: 0x0000CDB2
		internal void Reset()
		{
			if (this.needToReset)
			{
				this.needToReset = false;
				if (this.scope != null)
				{
					WmiNetUtilsHelper.ResetSecurity_f(this.handle);
				}
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000DDDC File Offset: 0x0000CDDC
		internal void Secure(IWbemServices services)
		{
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				int num = WmiNetUtilsHelper.BlessIWbemServices_f(services, this.scope.Options.Username, password, this.scope.Options.Authority, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
				Marshal.ZeroFreeBSTR(password);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000DE6C File Offset: 0x0000CE6C
		internal void SecureIUnknown(object unknown)
		{
			if (this.scope != null)
			{
				IntPtr password = this.scope.Options.GetPassword();
				int num = WmiNetUtilsHelper.BlessIWbemServicesObject_f(unknown, this.scope.Options.Username, password, this.scope.Options.Authority, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
				Marshal.ZeroFreeBSTR(password);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x040001CA RID: 458
		private bool needToReset;

		// Token: 0x040001CB RID: 459
		private IntPtr handle;

		// Token: 0x040001CC RID: 460
		private ManagementScope scope;
	}
}

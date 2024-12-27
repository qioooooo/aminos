using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000077 RID: 119
	internal class SecuredConnectHandler
	{
		// Token: 0x06000345 RID: 837 RVA: 0x0000D5EE File Offset: 0x0000C5EE
		internal SecuredConnectHandler(ManagementScope theScope)
		{
			this.scope = theScope;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000D600 File Offset: 0x0000C600
		internal int ConnectNSecureIWbemServices(string path, ref IWbemServices pServices)
		{
			int num = -2147217407;
			if (this.scope != null)
			{
				bool flag = false;
				IntPtr zero = IntPtr.Zero;
				try
				{
					if (this.scope.Options.EnablePrivileges && !CompatSwitches.AllowIManagementObjectQI)
					{
						WmiNetUtilsHelper.SetSecurity_f(ref flag, ref zero);
					}
					IntPtr password = this.scope.Options.GetPassword();
					num = WmiNetUtilsHelper.ConnectServerWmi_f(path, this.scope.Options.Username, password, this.scope.Options.Locale, this.scope.Options.Flags, this.scope.Options.Authority, this.scope.Options.GetContext(), out pServices, (int)this.scope.Options.Impersonation, (int)this.scope.Options.Authentication);
					Marshal.ZeroFreeBSTR(password);
				}
				finally
				{
					if (flag)
					{
						flag = false;
						WmiNetUtilsHelper.ResetSecurity_f(zero);
					}
				}
			}
			return num;
		}

		// Token: 0x040001C7 RID: 455
		private ManagementScope scope;
	}
}

using System;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x02000005 RID: 5
	internal class AdminApiDsp
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000027BC File Offset: 0x000009BC
		private IAdminApiDsp getApiFactory()
		{
			if (this.adminApiDsp == null)
			{
				if (AdminApiDsp.isRunningOnMono() == 0 && AdminApiDsp.isSystemArchWin32())
				{
					if (this.isVendorCodePresent)
					{
						this.adminApiDsp = new AdminApiDspEmb32();
					}
					else
					{
						this.adminApiDsp = new AdminApiDspWin32();
					}
				}
				else if (this.isVendorCodePresent)
				{
					this.adminApiDsp = new AdminApiDspEmb64();
				}
				else
				{
					this.adminApiDsp = new AdminApiDspWin64();
				}
			}
			return this.adminApiDsp;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000284C File Offset: 0x00000A4C
		internal static int isRunningOnMono()
		{
			int num = 1;
			if (Environment.OSVersion.ToString().IndexOf("Windows") >= 0)
			{
				num = 0;
			}
			else
			{
				string text = Environment.OSVersion.ToString();
				int num2 = text.IndexOf('.');
				int num3 = text.IndexOf(' ');
				if (num2 >= 0 && num3 >= 0)
				{
					string text2 = text.Substring(num3, num2 - num3);
					int num4 = int.Parse(text2);
					if (num4 >= 4)
					{
						num = 2;
					}
				}
			}
			return num;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000028DC File Offset: 0x00000ADC
		private static bool isSystemArchWin32()
		{
			return IntPtr.Size == 4;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028F8 File Offset: 0x00000AF8
		internal AdminStatus sntl_admin_context_new_scope(string scope)
		{
			return this.getApiFactory().sntl_admin_context_new_scope(scope);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002918 File Offset: 0x00000B18
		internal AdminStatus sntl_admin_context_new(string hostname, ushort port, string password)
		{
			return this.getApiFactory().sntl_admin_context_new(hostname, port, password);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002938 File Offset: 0x00000B38
		internal AdminStatus sntl_admin_context_delete()
		{
			return this.getApiFactory().sntl_admin_context_delete();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002958 File Offset: 0x00000B58
		internal AdminStatus sntl_admin_set(string action, ref string return_status)
		{
			return this.getApiFactory().sntl_admin_set(action, ref return_status);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002978 File Offset: 0x00000B78
		internal AdminStatus sntl_admin_get(string scope, string format, ref string info)
		{
			return this.getApiFactory().sntl_admin_get(scope, format, ref info);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002998 File Offset: 0x00000B98
		internal bool setVendorCodeFlag(bool p)
		{
			this.isVendorCodePresent = p;
			return this.isVendorCodePresent;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029B7 File Offset: 0x00000BB7
		internal void setLibPath(string path)
		{
			AdminHelper.SetDllDirectory(path);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029C1 File Offset: 0x00000BC1
		internal void setLibPathApiDsp(string path)
		{
			this.getApiFactory().setLibPath(path);
		}

		// Token: 0x0400000A RID: 10
		private IAdminApiDsp adminApiDsp;

		// Token: 0x0400000B RID: 11
		private bool isVendorCodePresent = false;
	}
}

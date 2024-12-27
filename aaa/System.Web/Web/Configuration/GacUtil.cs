using System;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001E3 RID: 483
	internal sealed class GacUtil : IGac
	{
		// Token: 0x06001ACB RID: 6859 RVA: 0x0007C198 File Offset: 0x0007B198
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public void GacInstall(string assemblyPath)
		{
			IAssemblyCache assemblyCache = null;
			int num = NativeMethods.CreateAssemblyCache(out assemblyCache, 0U);
			if (num == 0)
			{
				num = assemblyCache.InstallAssembly(0U, assemblyPath, IntPtr.Zero);
			}
			if (num != 0)
			{
				throw new Exception(SR.GetString("Failed_gac_install"));
			}
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0007C1D4 File Offset: 0x0007B1D4
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public bool GacUnInstall(string assemblyName)
		{
			IAssemblyCache assemblyCache = null;
			uint num = 0U;
			int num2 = NativeMethods.CreateAssemblyCache(out assemblyCache, 0U);
			if (num2 == 0)
			{
				num2 = assemblyCache.UninstallAssembly(0U, assemblyName, IntPtr.Zero, out num);
				if (num == 3U)
				{
					return false;
				}
			}
			if (num2 != 0)
			{
				throw new Exception(SR.GetString("Failed_gac_uninstall"));
			}
			return true;
		}
	}
}

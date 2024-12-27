using System;
using System.Configuration.Install;

namespace System.Management.Instrumentation
{
	// Token: 0x020000AB RID: 171
	public class DefaultManagementProjectInstaller : Installer
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x00023C24 File Offset: 0x00022C24
		public DefaultManagementProjectInstaller()
		{
			ManagementInstaller managementInstaller = new ManagementInstaller();
			base.Installers.Add(managementInstaller);
		}
	}
}

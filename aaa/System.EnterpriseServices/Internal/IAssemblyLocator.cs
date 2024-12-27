using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000B5 RID: 181
	[Guid("391ffbb9-a8ee-432a-abc8-baa238dab90f")]
	[ComImport]
	internal interface IAssemblyLocator
	{
		// Token: 0x06000455 RID: 1109
		string[] GetModules(string applicationDir, string applicationName, string assemblyName);
	}
}

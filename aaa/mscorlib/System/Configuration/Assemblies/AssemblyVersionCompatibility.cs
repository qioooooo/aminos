using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Assemblies
{
	// Token: 0x02000849 RID: 2121
	[ComVisible(true)]
	[Serializable]
	public enum AssemblyVersionCompatibility
	{
		// Token: 0x04002830 RID: 10288
		SameMachine = 1,
		// Token: 0x04002831 RID: 10289
		SameProcess,
		// Token: 0x04002832 RID: 10290
		SameDomain
	}
}

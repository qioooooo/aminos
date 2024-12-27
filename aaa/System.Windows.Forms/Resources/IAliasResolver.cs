using System;
using System.Reflection;

namespace System.Resources
{
	// Token: 0x02000149 RID: 329
	internal interface IAliasResolver
	{
		// Token: 0x06000527 RID: 1319
		AssemblyName ResolveAlias(string alias);

		// Token: 0x06000528 RID: 1320
		void PushAlias(string alias, AssemblyName name);
	}
}

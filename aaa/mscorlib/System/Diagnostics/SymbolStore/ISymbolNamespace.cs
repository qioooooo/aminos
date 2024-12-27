using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BA RID: 698
	[ComVisible(true)]
	public interface ISymbolNamespace
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001B77 RID: 7031
		string Name { get; }

		// Token: 0x06001B78 RID: 7032
		ISymbolNamespace[] GetNamespaces();

		// Token: 0x06001B79 RID: 7033
		ISymbolVariable[] GetVariables();
	}
}

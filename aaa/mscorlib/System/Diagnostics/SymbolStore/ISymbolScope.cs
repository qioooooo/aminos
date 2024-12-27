using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BC RID: 700
	[ComVisible(true)]
	public interface ISymbolScope
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001B84 RID: 7044
		ISymbolMethod Method { get; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001B85 RID: 7045
		ISymbolScope Parent { get; }

		// Token: 0x06001B86 RID: 7046
		ISymbolScope[] GetChildren();

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001B87 RID: 7047
		int StartOffset { get; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001B88 RID: 7048
		int EndOffset { get; }

		// Token: 0x06001B89 RID: 7049
		ISymbolVariable[] GetLocals();

		// Token: 0x06001B8A RID: 7050
		ISymbolNamespace[] GetNamespaces();
	}
}

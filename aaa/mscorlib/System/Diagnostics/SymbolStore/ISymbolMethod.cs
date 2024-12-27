using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002B9 RID: 697
	[ComVisible(true)]
	public interface ISymbolMethod
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001B6D RID: 7021
		SymbolToken Token { get; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001B6E RID: 7022
		int SequencePointCount { get; }

		// Token: 0x06001B6F RID: 7023
		void GetSequencePoints(int[] offsets, ISymbolDocument[] documents, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001B70 RID: 7024
		ISymbolScope RootScope { get; }

		// Token: 0x06001B71 RID: 7025
		ISymbolScope GetScope(int offset);

		// Token: 0x06001B72 RID: 7026
		int GetOffset(ISymbolDocument document, int line, int column);

		// Token: 0x06001B73 RID: 7027
		int[] GetRanges(ISymbolDocument document, int line, int column);

		// Token: 0x06001B74 RID: 7028
		ISymbolVariable[] GetParameters();

		// Token: 0x06001B75 RID: 7029
		ISymbolNamespace GetNamespace();

		// Token: 0x06001B76 RID: 7030
		bool GetSourceStartEnd(ISymbolDocument[] docs, int[] lines, int[] columns);
	}
}

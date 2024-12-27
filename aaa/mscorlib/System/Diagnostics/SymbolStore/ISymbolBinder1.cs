using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002B6 RID: 694
	[ComVisible(true)]
	public interface ISymbolBinder1
	{
		// Token: 0x06001B60 RID: 7008
		ISymbolReader GetReader(IntPtr importer, string filename, string searchPath);
	}
}

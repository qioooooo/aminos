using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BB RID: 699
	[ComVisible(true)]
	public interface ISymbolReader
	{
		// Token: 0x06001B7A RID: 7034
		ISymbolDocument GetDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06001B7B RID: 7035
		ISymbolDocument[] GetDocuments();

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001B7C RID: 7036
		SymbolToken UserEntryPoint { get; }

		// Token: 0x06001B7D RID: 7037
		ISymbolMethod GetMethod(SymbolToken method);

		// Token: 0x06001B7E RID: 7038
		ISymbolMethod GetMethod(SymbolToken method, int version);

		// Token: 0x06001B7F RID: 7039
		ISymbolVariable[] GetVariables(SymbolToken parent);

		// Token: 0x06001B80 RID: 7040
		ISymbolVariable[] GetGlobalVariables();

		// Token: 0x06001B81 RID: 7041
		ISymbolMethod GetMethodFromDocumentPosition(ISymbolDocument document, int line, int column);

		// Token: 0x06001B82 RID: 7042
		byte[] GetSymAttribute(SymbolToken parent, string name);

		// Token: 0x06001B83 RID: 7043
		ISymbolNamespace[] GetNamespaces();
	}
}

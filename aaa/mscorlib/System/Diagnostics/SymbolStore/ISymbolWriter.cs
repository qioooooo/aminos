using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BE RID: 702
	[ComVisible(true)]
	public interface ISymbolWriter
	{
		// Token: 0x06001B94 RID: 7060
		void Initialize(IntPtr emitter, string filename, bool fFullBuild);

		// Token: 0x06001B95 RID: 7061
		ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06001B96 RID: 7062
		void SetUserEntryPoint(SymbolToken entryMethod);

		// Token: 0x06001B97 RID: 7063
		void OpenMethod(SymbolToken method);

		// Token: 0x06001B98 RID: 7064
		void CloseMethod();

		// Token: 0x06001B99 RID: 7065
		void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x06001B9A RID: 7066
		int OpenScope(int startOffset);

		// Token: 0x06001B9B RID: 7067
		void CloseScope(int endOffset);

		// Token: 0x06001B9C RID: 7068
		void SetScopeRange(int scopeID, int startOffset, int endOffset);

		// Token: 0x06001B9D RID: 7069
		void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset);

		// Token: 0x06001B9E RID: 7070
		void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001B9F RID: 7071
		void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001BA0 RID: 7072
		void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06001BA1 RID: 7073
		void Close();

		// Token: 0x06001BA2 RID: 7074
		void SetSymAttribute(SymbolToken parent, string name, byte[] data);

		// Token: 0x06001BA3 RID: 7075
		void OpenNamespace(string name);

		// Token: 0x06001BA4 RID: 7076
		void CloseNamespace();

		// Token: 0x06001BA5 RID: 7077
		void UsingNamespace(string fullName);

		// Token: 0x06001BA6 RID: 7078
		void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn);

		// Token: 0x06001BA7 RID: 7079
		void SetUnderlyingWriter(IntPtr underlyingWriter);
	}
}

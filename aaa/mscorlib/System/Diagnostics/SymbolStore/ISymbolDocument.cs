using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002B7 RID: 695
	[ComVisible(true)]
	public interface ISymbolDocument
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001B61 RID: 7009
		string URL { get; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001B62 RID: 7010
		Guid DocumentType { get; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001B63 RID: 7011
		Guid Language { get; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001B64 RID: 7012
		Guid LanguageVendor { get; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001B65 RID: 7013
		Guid CheckSumAlgorithmId { get; }

		// Token: 0x06001B66 RID: 7014
		byte[] GetCheckSum();

		// Token: 0x06001B67 RID: 7015
		int FindClosestLine(int line);

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001B68 RID: 7016
		bool HasEmbeddedSource { get; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001B69 RID: 7017
		int SourceLength { get; }

		// Token: 0x06001B6A RID: 7018
		byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn);
	}
}

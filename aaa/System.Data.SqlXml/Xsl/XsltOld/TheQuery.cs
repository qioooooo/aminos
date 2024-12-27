using System;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200019D RID: 413
	internal sealed class TheQuery
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x000545FF File Offset: 0x000535FF
		internal CompiledXpathExpr CompiledQuery
		{
			get
			{
				return this._CompiledQuery;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00054607 File Offset: 0x00053607
		internal TheQuery(CompiledXpathExpr compiledQuery, InputScopeManager manager)
		{
			this._CompiledQuery = compiledQuery;
			this._ScopeManager = manager.Clone();
		}

		// Token: 0x04000BCD RID: 3021
		internal InputScopeManager _ScopeManager;

		// Token: 0x04000BCE RID: 3022
		private CompiledXpathExpr _CompiledQuery;
	}
}

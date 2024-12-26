using System;

namespace Microsoft.JScript
{
	// Token: 0x020000BB RID: 187
	internal class RecoveryTokenException : ParserException
	{
		// Token: 0x06000886 RID: 2182 RVA: 0x00040E2D File Offset: 0x0003FE2D
		internal RecoveryTokenException(JSToken token, AST partialAST)
		{
			this._token = token;
			this._partiallyComputedNode = partialAST;
		}

		// Token: 0x04000481 RID: 1153
		internal JSToken _token;

		// Token: 0x04000482 RID: 1154
		internal AST _partiallyComputedNode;
	}
}

using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200008D RID: 141
	internal sealed class IdentifierLiteral : AST
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x0002E33E File Offset: 0x0002D33E
		internal IdentifierLiteral(string identifier, Context context)
			: base(context)
		{
			this.identifier = identifier;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002E34E File Offset: 0x0002D34E
		internal override object Evaluate()
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002E35D File Offset: 0x0002D35D
		internal override AST PartiallyEvaluate()
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002E36C File Offset: 0x0002D36C
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002E374 File Offset: 0x0002D374
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x040002FF RID: 767
		private string identifier;
	}
}

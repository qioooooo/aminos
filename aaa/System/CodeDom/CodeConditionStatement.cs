using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200004D RID: 77
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeConditionStatement : CodeStatement
	{
		// Token: 0x060002FE RID: 766 RVA: 0x00012FCF File Offset: 0x00011FCF
		public CodeConditionStatement()
		{
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00012FED File Offset: 0x00011FED
		public CodeConditionStatement(CodeExpression condition, params CodeStatement[] trueStatements)
		{
			this.Condition = condition;
			this.TrueStatements.AddRange(trueStatements);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0001301E File Offset: 0x0001201E
		public CodeConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements)
		{
			this.Condition = condition;
			this.TrueStatements.AddRange(trueStatements);
			this.FalseStatements.AddRange(falseStatements);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0001305B File Offset: 0x0001205B
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00013063 File Offset: 0x00012063
		public CodeExpression Condition
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0001306C File Offset: 0x0001206C
		public CodeStatementCollection TrueStatements
		{
			get
			{
				return this.trueStatments;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000304 RID: 772 RVA: 0x00013074 File Offset: 0x00012074
		public CodeStatementCollection FalseStatements
		{
			get
			{
				return this.falseStatments;
			}
		}

		// Token: 0x0400080D RID: 2061
		private CodeExpression condition;

		// Token: 0x0400080E RID: 2062
		private CodeStatementCollection trueStatments = new CodeStatementCollection();

		// Token: 0x0400080F RID: 2063
		private CodeStatementCollection falseStatments = new CodeStatementCollection();
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005D RID: 93
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeIterationStatement : CodeStatement
	{
		// Token: 0x06000370 RID: 880 RVA: 0x00013857 File Offset: 0x00012857
		public CodeIterationStatement()
		{
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001386A File Offset: 0x0001286A
		public CodeIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, params CodeStatement[] statements)
		{
			this.InitStatement = initStatement;
			this.TestExpression = testExpression;
			this.IncrementStatement = incrementStatement;
			this.Statements.AddRange(statements);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0001389F File Offset: 0x0001289F
		// (set) Token: 0x06000373 RID: 883 RVA: 0x000138A7 File Offset: 0x000128A7
		public CodeStatement InitStatement
		{
			get
			{
				return this.initStatement;
			}
			set
			{
				this.initStatement = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000138B0 File Offset: 0x000128B0
		// (set) Token: 0x06000375 RID: 885 RVA: 0x000138B8 File Offset: 0x000128B8
		public CodeExpression TestExpression
		{
			get
			{
				return this.testExpression;
			}
			set
			{
				this.testExpression = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000376 RID: 886 RVA: 0x000138C1 File Offset: 0x000128C1
		// (set) Token: 0x06000377 RID: 887 RVA: 0x000138C9 File Offset: 0x000128C9
		public CodeStatement IncrementStatement
		{
			get
			{
				return this.incrementStatement;
			}
			set
			{
				this.incrementStatement = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000138D2 File Offset: 0x000128D2
		public CodeStatementCollection Statements
		{
			get
			{
				return this.statements;
			}
		}

		// Token: 0x04000837 RID: 2103
		private CodeStatement initStatement;

		// Token: 0x04000838 RID: 2104
		private CodeExpression testExpression;

		// Token: 0x04000839 RID: 2105
		private CodeStatement incrementStatement;

		// Token: 0x0400083A RID: 2106
		private CodeStatementCollection statements = new CodeStatementCollection();
	}
}

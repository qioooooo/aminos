using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200007A RID: 122
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTryCatchFinallyStatement : CodeStatement
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x00014860 File Offset: 0x00013860
		public CodeTryCatchFinallyStatement()
		{
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001488C File Offset: 0x0001388C
		public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses)
		{
			this.TryStatements.AddRange(tryStatements);
			this.CatchClauses.AddRange(catchClauses);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000148D8 File Offset: 0x000138D8
		public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements)
		{
			this.TryStatements.AddRange(tryStatements);
			this.CatchClauses.AddRange(catchClauses);
			this.FinallyStatements.AddRange(finallyStatements);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00014930 File Offset: 0x00013930
		public CodeStatementCollection TryStatements
		{
			get
			{
				return this.tryStatments;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00014938 File Offset: 0x00013938
		public CodeCatchClauseCollection CatchClauses
		{
			get
			{
				return this.catchClauses;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00014940 File Offset: 0x00013940
		public CodeStatementCollection FinallyStatements
		{
			get
			{
				return this.finallyStatments;
			}
		}

		// Token: 0x0400087A RID: 2170
		private CodeStatementCollection tryStatments = new CodeStatementCollection();

		// Token: 0x0400087B RID: 2171
		private CodeStatementCollection finallyStatments = new CodeStatementCollection();

		// Token: 0x0400087C RID: 2172
		private CodeCatchClauseCollection catchClauses = new CodeCatchClauseCollection();
	}
}

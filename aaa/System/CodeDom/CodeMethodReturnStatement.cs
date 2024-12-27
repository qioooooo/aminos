using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000065 RID: 101
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeMethodReturnStatement : CodeStatement
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00013C8D File Offset: 0x00012C8D
		public CodeMethodReturnStatement()
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013C95 File Offset: 0x00012C95
		public CodeMethodReturnStatement(CodeExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00013CA4 File Offset: 0x00012CA4
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00013CAC File Offset: 0x00012CAC
		public CodeExpression Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		// Token: 0x04000851 RID: 2129
		private CodeExpression expression;
	}
}

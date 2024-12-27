using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000059 RID: 89
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeExpressionStatement : CodeStatement
	{
		// Token: 0x0600035D RID: 861 RVA: 0x0001374C File Offset: 0x0001274C
		public CodeExpressionStatement()
		{
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00013754 File Offset: 0x00012754
		public CodeExpressionStatement(CodeExpression expression)
		{
			this.expression = expression;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00013763 File Offset: 0x00012763
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0001376B File Offset: 0x0001276B
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

		// Token: 0x04000831 RID: 2097
		private CodeExpression expression;
	}
}

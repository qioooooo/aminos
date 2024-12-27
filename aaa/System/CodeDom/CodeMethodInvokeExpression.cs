using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000063 RID: 99
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMethodInvokeExpression : CodeExpression
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x00013B68 File Offset: 0x00012B68
		public CodeMethodInvokeExpression()
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013B7B File Offset: 0x00012B7B
		public CodeMethodInvokeExpression(CodeMethodReferenceExpression method, params CodeExpression[] parameters)
		{
			this.method = method;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00013BA1 File Offset: 0x00012BA1
		public CodeMethodInvokeExpression(CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
		{
			this.method = new CodeMethodReferenceExpression(targetObject, methodName);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00013BCD File Offset: 0x00012BCD
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00013BE8 File Offset: 0x00012BE8
		public CodeMethodReferenceExpression Method
		{
			get
			{
				if (this.method == null)
				{
					this.method = new CodeMethodReferenceExpression();
				}
				return this.method;
			}
			set
			{
				this.method = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00013BF1 File Offset: 0x00012BF1
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0400084C RID: 2124
		private CodeMethodReferenceExpression method;

		// Token: 0x0400084D RID: 2125
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}

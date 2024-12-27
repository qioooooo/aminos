using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000053 RID: 83
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeDelegateInvokeExpression : CodeExpression
	{
		// Token: 0x06000330 RID: 816 RVA: 0x0001342E File Offset: 0x0001242E
		public CodeDelegateInvokeExpression()
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00013441 File Offset: 0x00012441
		public CodeDelegateInvokeExpression(CodeExpression targetObject)
		{
			this.TargetObject = targetObject;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001345B File Offset: 0x0001245B
		public CodeDelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters)
		{
			this.TargetObject = targetObject;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00013481 File Offset: 0x00012481
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00013489 File Offset: 0x00012489
		public CodeExpression TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00013492 File Offset: 0x00012492
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0400082B RID: 2091
		private CodeExpression targetObject;

		// Token: 0x0400082C RID: 2092
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}

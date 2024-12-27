using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000054 RID: 84
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeDirectionExpression : CodeExpression
	{
		// Token: 0x06000336 RID: 822 RVA: 0x0001349A File Offset: 0x0001249A
		public CodeDirectionExpression()
		{
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000134A2 File Offset: 0x000124A2
		public CodeDirectionExpression(FieldDirection direction, CodeExpression expression)
		{
			this.expression = expression;
			this.direction = direction;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000338 RID: 824 RVA: 0x000134B8 File Offset: 0x000124B8
		// (set) Token: 0x06000339 RID: 825 RVA: 0x000134C0 File Offset: 0x000124C0
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600033A RID: 826 RVA: 0x000134C9 File Offset: 0x000124C9
		// (set) Token: 0x0600033B RID: 827 RVA: 0x000134D1 File Offset: 0x000124D1
		public FieldDirection Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		// Token: 0x0400082D RID: 2093
		private CodeExpression expression;

		// Token: 0x0400082E RID: 2094
		private FieldDirection direction;
	}
}

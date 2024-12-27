using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200016C RID: 364
	public sealed class RootContext
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x000376A1 File Offset: 0x000366A1
		public RootContext(CodeExpression expression, object value)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.expression = expression;
			this.value = value;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x000376D3 File Offset: 0x000366D3
		public CodeExpression Expression
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x000376DB File Offset: 0x000366DB
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000F15 RID: 3861
		private CodeExpression expression;

		// Token: 0x04000F16 RID: 3862
		private object value;
	}
}

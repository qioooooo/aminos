using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006D RID: 109
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodePrimitiveExpression : CodeExpression
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x000144D8 File Offset: 0x000134D8
		public CodePrimitiveExpression()
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000144E0 File Offset: 0x000134E0
		public CodePrimitiveExpression(object value)
		{
			this.Value = value;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000144EF File Offset: 0x000134EF
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000144F7 File Offset: 0x000134F7
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04000868 RID: 2152
		private object value;
	}
}

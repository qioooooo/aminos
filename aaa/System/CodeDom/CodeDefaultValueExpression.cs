using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000051 RID: 81
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDefaultValueExpression : CodeExpression
	{
		// Token: 0x06000324 RID: 804 RVA: 0x00013370 File Offset: 0x00012370
		public CodeDefaultValueExpression()
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00013378 File Offset: 0x00012378
		public CodeDefaultValueExpression(CodeTypeReference type)
		{
			this.type = type;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00013387 File Offset: 0x00012387
		// (set) Token: 0x06000327 RID: 807 RVA: 0x000133A7 File Offset: 0x000123A7
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04000827 RID: 2087
		private CodeTypeReference type;
	}
}

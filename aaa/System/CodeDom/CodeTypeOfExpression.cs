using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000080 RID: 128
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeOfExpression : CodeExpression
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x00014E84 File Offset: 0x00013E84
		public CodeTypeOfExpression()
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00014E8C File Offset: 0x00013E8C
		public CodeTypeOfExpression(CodeTypeReference type)
		{
			this.Type = type;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00014E9B File Offset: 0x00013E9B
		public CodeTypeOfExpression(string type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014EAF File Offset: 0x00013EAF
		public CodeTypeOfExpression(Type type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00014EC3 File Offset: 0x00013EC3
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x00014EE3 File Offset: 0x00013EE3
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

		// Token: 0x0400088B RID: 2187
		private CodeTypeReference type;
	}
}

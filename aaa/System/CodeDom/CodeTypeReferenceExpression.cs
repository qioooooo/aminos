using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000086 RID: 134
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeReferenceExpression : CodeExpression
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00015768 File Offset: 0x00014768
		public CodeTypeReferenceExpression()
		{
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00015770 File Offset: 0x00014770
		public CodeTypeReferenceExpression(CodeTypeReference type)
		{
			this.Type = type;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001577F File Offset: 0x0001477F
		public CodeTypeReferenceExpression(string type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00015793 File Offset: 0x00014793
		public CodeTypeReferenceExpression(Type type)
		{
			this.Type = new CodeTypeReference(type);
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000157A7 File Offset: 0x000147A7
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x000157C7 File Offset: 0x000147C7
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

		// Token: 0x0400089A RID: 2202
		private CodeTypeReference type;
	}
}

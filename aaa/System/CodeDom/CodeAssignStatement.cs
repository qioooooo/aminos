using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200003B RID: 59
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeAssignStatement : CodeStatement
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0001266D File Offset: 0x0001166D
		public CodeAssignStatement()
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00012675 File Offset: 0x00011675
		public CodeAssignStatement(CodeExpression left, CodeExpression right)
		{
			this.Left = left;
			this.Right = right;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0001268B File Offset: 0x0001168B
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00012693 File Offset: 0x00011693
		public CodeExpression Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0001269C File Offset: 0x0001169C
		// (set) Token: 0x0600027C RID: 636 RVA: 0x000126A4 File Offset: 0x000116A4
		public CodeExpression Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		// Token: 0x040007DF RID: 2015
		private CodeExpression left;

		// Token: 0x040007E0 RID: 2016
		private CodeExpression right;
	}
}

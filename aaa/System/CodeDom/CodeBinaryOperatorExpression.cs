using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000042 RID: 66
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeBinaryOperatorExpression : CodeExpression
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00012A70 File Offset: 0x00011A70
		public CodeBinaryOperatorExpression()
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00012A78 File Offset: 0x00011A78
		public CodeBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
		{
			this.Right = right;
			this.Operator = op;
			this.Left = left;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00012A95 File Offset: 0x00011A95
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00012A9D File Offset: 0x00011A9D
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

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00012AA6 File Offset: 0x00011AA6
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00012AAE File Offset: 0x00011AAE
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00012AB7 File Offset: 0x00011AB7
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00012ABF File Offset: 0x00011ABF
		public CodeBinaryOperatorType Operator
		{
			get
			{
				return this.op;
			}
			set
			{
				this.op = value;
			}
		}

		// Token: 0x040007E8 RID: 2024
		private CodeBinaryOperatorType op;

		// Token: 0x040007E9 RID: 2025
		private CodeExpression left;

		// Token: 0x040007EA RID: 2026
		private CodeExpression right;
	}
}

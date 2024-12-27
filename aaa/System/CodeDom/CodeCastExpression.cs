using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000044 RID: 68
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCastExpression : CodeExpression
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00012AC8 File Offset: 0x00011AC8
		public CodeCastExpression()
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00012AD0 File Offset: 0x00011AD0
		public CodeCastExpression(CodeTypeReference targetType, CodeExpression expression)
		{
			this.TargetType = targetType;
			this.Expression = expression;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00012AE6 File Offset: 0x00011AE6
		public CodeCastExpression(string targetType, CodeExpression expression)
		{
			this.TargetType = new CodeTypeReference(targetType);
			this.Expression = expression;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00012B01 File Offset: 0x00011B01
		public CodeCastExpression(Type targetType, CodeExpression expression)
		{
			this.TargetType = new CodeTypeReference(targetType);
			this.Expression = expression;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00012B1C File Offset: 0x00011B1C
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00012B3C File Offset: 0x00011B3C
		public CodeTypeReference TargetType
		{
			get
			{
				if (this.targetType == null)
				{
					this.targetType = new CodeTypeReference("");
				}
				return this.targetType;
			}
			set
			{
				this.targetType = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00012B45 File Offset: 0x00011B45
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00012B4D File Offset: 0x00011B4D
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

		// Token: 0x040007FD RID: 2045
		private CodeTypeReference targetType;

		// Token: 0x040007FE RID: 2046
		private CodeExpression expression;
	}
}

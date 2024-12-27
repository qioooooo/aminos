using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000039 RID: 57
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeArrayIndexerExpression : CodeExpression
	{
		// Token: 0x0600026D RID: 621 RVA: 0x000125C4 File Offset: 0x000115C4
		public CodeArrayIndexerExpression()
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000125CC File Offset: 0x000115CC
		public CodeArrayIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
		{
			this.targetObject = targetObject;
			this.indices = new CodeExpressionCollection();
			this.indices.AddRange(indices);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600026F RID: 623 RVA: 0x000125F2 File Offset: 0x000115F2
		// (set) Token: 0x06000270 RID: 624 RVA: 0x000125FA File Offset: 0x000115FA
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00012603 File Offset: 0x00011603
		public CodeExpressionCollection Indices
		{
			get
			{
				if (this.indices == null)
				{
					this.indices = new CodeExpressionCollection();
				}
				return this.indices;
			}
		}

		// Token: 0x040007DA RID: 2010
		private CodeExpression targetObject;

		// Token: 0x040007DB RID: 2011
		private CodeExpressionCollection indices;
	}
}

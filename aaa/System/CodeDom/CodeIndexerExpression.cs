using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005C RID: 92
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeIndexerExpression : CodeExpression
	{
		// Token: 0x0600036B RID: 875 RVA: 0x000137FD File Offset: 0x000127FD
		public CodeIndexerExpression()
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00013805 File Offset: 0x00012805
		public CodeIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
		{
			this.targetObject = targetObject;
			this.indices = new CodeExpressionCollection();
			this.indices.AddRange(indices);
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0001382B File Offset: 0x0001282B
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00013833 File Offset: 0x00012833
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0001383C File Offset: 0x0001283C
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

		// Token: 0x04000835 RID: 2101
		private CodeExpression targetObject;

		// Token: 0x04000836 RID: 2102
		private CodeExpressionCollection indices;
	}
}

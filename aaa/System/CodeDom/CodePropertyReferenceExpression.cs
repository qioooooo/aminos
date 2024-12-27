using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006E RID: 110
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodePropertyReferenceExpression : CodeExpression
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x00014500 File Offset: 0x00013500
		public CodePropertyReferenceExpression()
		{
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00014513 File Offset: 0x00013513
		public CodePropertyReferenceExpression(CodeExpression targetObject, string propertyName)
		{
			this.TargetObject = targetObject;
			this.PropertyName = propertyName;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00014534 File Offset: 0x00013534
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0001453C File Offset: 0x0001353C
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00014545 File Offset: 0x00013545
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0001455B File Offset: 0x0001355B
		public string PropertyName
		{
			get
			{
				if (this.propertyName != null)
				{
					return this.propertyName;
				}
				return string.Empty;
			}
			set
			{
				this.propertyName = value;
			}
		}

		// Token: 0x04000869 RID: 2153
		private CodeExpression targetObject;

		// Token: 0x0400086A RID: 2154
		private string propertyName;

		// Token: 0x0400086B RID: 2155
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}

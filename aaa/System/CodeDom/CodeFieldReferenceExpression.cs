using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005A RID: 90
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeFieldReferenceExpression : CodeExpression
	{
		// Token: 0x06000361 RID: 865 RVA: 0x00013774 File Offset: 0x00012774
		public CodeFieldReferenceExpression()
		{
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001377C File Offset: 0x0001277C
		public CodeFieldReferenceExpression(CodeExpression targetObject, string fieldName)
		{
			this.TargetObject = targetObject;
			this.FieldName = fieldName;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00013792 File Offset: 0x00012792
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0001379A File Offset: 0x0001279A
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000137A3 File Offset: 0x000127A3
		// (set) Token: 0x06000366 RID: 870 RVA: 0x000137B9 File Offset: 0x000127B9
		public string FieldName
		{
			get
			{
				if (this.fieldName != null)
				{
					return this.fieldName;
				}
				return string.Empty;
			}
			set
			{
				this.fieldName = value;
			}
		}

		// Token: 0x04000832 RID: 2098
		private CodeExpression targetObject;

		// Token: 0x04000833 RID: 2099
		private string fieldName;
	}
}

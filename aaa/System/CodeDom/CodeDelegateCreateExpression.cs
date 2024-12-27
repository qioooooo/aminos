using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000052 RID: 82
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeDelegateCreateExpression : CodeExpression
	{
		// Token: 0x06000328 RID: 808 RVA: 0x000133B0 File Offset: 0x000123B0
		public CodeDelegateCreateExpression()
		{
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000133B8 File Offset: 0x000123B8
		public CodeDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName)
		{
			this.delegateType = delegateType;
			this.targetObject = targetObject;
			this.methodName = methodName;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000133D5 File Offset: 0x000123D5
		// (set) Token: 0x0600032B RID: 811 RVA: 0x000133F5 File Offset: 0x000123F5
		public CodeTypeReference DelegateType
		{
			get
			{
				if (this.delegateType == null)
				{
					this.delegateType = new CodeTypeReference("");
				}
				return this.delegateType;
			}
			set
			{
				this.delegateType = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000133FE File Offset: 0x000123FE
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00013406 File Offset: 0x00012406
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001340F File Offset: 0x0001240F
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00013425 File Offset: 0x00012425
		public string MethodName
		{
			get
			{
				if (this.methodName != null)
				{
					return this.methodName;
				}
				return string.Empty;
			}
			set
			{
				this.methodName = value;
			}
		}

		// Token: 0x04000828 RID: 2088
		private CodeTypeReference delegateType;

		// Token: 0x04000829 RID: 2089
		private CodeExpression targetObject;

		// Token: 0x0400082A RID: 2090
		private string methodName;
	}
}

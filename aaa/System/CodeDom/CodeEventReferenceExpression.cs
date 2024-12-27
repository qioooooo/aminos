using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000057 RID: 87
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeEventReferenceExpression : CodeExpression
	{
		// Token: 0x0600034A RID: 842 RVA: 0x000135F0 File Offset: 0x000125F0
		public CodeEventReferenceExpression()
		{
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000135F8 File Offset: 0x000125F8
		public CodeEventReferenceExpression(CodeExpression targetObject, string eventName)
		{
			this.targetObject = targetObject;
			this.eventName = eventName;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0001360E File Offset: 0x0001260E
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00013616 File Offset: 0x00012616
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0001361F File Offset: 0x0001261F
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00013635 File Offset: 0x00012635
		public string EventName
		{
			get
			{
				if (this.eventName != null)
				{
					return this.eventName;
				}
				return string.Empty;
			}
			set
			{
				this.eventName = value;
			}
		}

		// Token: 0x0400082F RID: 2095
		private CodeExpression targetObject;

		// Token: 0x04000830 RID: 2096
		private string eventName;
	}
}

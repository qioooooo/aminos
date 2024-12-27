using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000123 RID: 291
	public sealed class DesignerActionPropertyItem : DesignerActionItem
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x0002D4D6 File Offset: 0x0002C4D6
		public DesignerActionPropertyItem(string memberName, string displayName, string category, string description)
			: base(displayName, category, description)
		{
			this.memberName = memberName;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002D4E9 File Offset: 0x0002C4E9
		public DesignerActionPropertyItem(string memberName, string displayName)
			: this(memberName, displayName, null, null)
		{
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002D4F5 File Offset: 0x0002C4F5
		public DesignerActionPropertyItem(string memberName, string displayName, string category)
			: this(memberName, displayName, category, null)
		{
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0002D501 File Offset: 0x0002C501
		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0002D509 File Offset: 0x0002C509
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0002D511 File Offset: 0x0002C511
		public IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
			set
			{
				this.relatedComponent = value;
			}
		}

		// Token: 0x04000E45 RID: 3653
		private string memberName;

		// Token: 0x04000E46 RID: 3654
		private IComponent relatedComponent;
	}
}

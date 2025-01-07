using System;

namespace System.ComponentModel.Design
{
	public sealed class DesignerActionPropertyItem : DesignerActionItem
	{
		public DesignerActionPropertyItem(string memberName, string displayName, string category, string description)
			: base(displayName, category, description)
		{
			this.memberName = memberName;
		}

		public DesignerActionPropertyItem(string memberName, string displayName)
			: this(memberName, displayName, null, null)
		{
		}

		public DesignerActionPropertyItem(string memberName, string displayName, string category)
			: this(memberName, displayName, category, null)
		{
		}

		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

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

		private string memberName;

		private IComponent relatedComponent;
	}
}

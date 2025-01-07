using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace System.ComponentModel.Design
{
	public abstract class DesignerActionItem
	{
		public DesignerActionItem(string displayName, string category, string description)
		{
			this.category = category;
			this.description = description;
			this.displayName = ((displayName == null) ? null : Regex.Replace(displayName, "\\(\\&.\\)", ""));
		}

		internal DesignerActionItem()
		{
		}

		public bool AllowAssociate
		{
			get
			{
				return this.allowAssociate;
			}
			set
			{
				this.allowAssociate = value;
			}
		}

		public virtual string Category
		{
			get
			{
				return this.category;
			}
		}

		public virtual string Description
		{
			get
			{
				return this.description;
			}
		}

		public virtual string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		public IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new HybridDictionary();
				}
				return this.properties;
			}
		}

		private bool allowAssociate;

		private string displayName;

		private string description;

		private string category;

		private IDictionary properties;
	}
}

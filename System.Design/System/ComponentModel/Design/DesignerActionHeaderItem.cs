using System;

namespace System.ComponentModel.Design
{
	public sealed class DesignerActionHeaderItem : DesignerActionTextItem
	{
		public DesignerActionHeaderItem(string displayName)
			: base(displayName, displayName)
		{
		}

		public DesignerActionHeaderItem(string displayName, string category)
			: base(displayName, category)
		{
		}
	}
}

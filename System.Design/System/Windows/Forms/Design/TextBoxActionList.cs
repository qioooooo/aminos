using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class TextBoxActionList : DesignerActionList
	{
		public TextBoxActionList(TextBoxDesigner designer)
			: base(designer.Component)
		{
		}

		public bool Multiline
		{
			get
			{
				return ((TextBox)base.Component).Multiline;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["Multiline"].SetValue(base.Component, value);
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionPropertyItem("Multiline", SR.GetString("MultiLineDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("MultiLineDescription"))
			};
		}
	}
}

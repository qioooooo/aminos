using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class LocalizeDesigner : LiteralDesigner
	{
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			EditableDesignerRegion editableDesignerRegion = new EditableDesignerRegion(this, "Text");
			editableDesignerRegion.Description = SR.GetString("LocalizeDesigner_RegionWatermark");
			editableDesignerRegion.Properties[typeof(Control)] = base.Component;
			regions.Add(editableDesignerRegion);
			return string.Format(CultureInfo.InvariantCulture, "<span {0}=0></span>", new object[] { DesignerRegion.DesignerRegionAttributeName });
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			return (string)propertyDescriptor.GetValue(base.Component);
		}

		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			string text = content;
			try
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				Control[] array = ControlParser.ParseControls(designerHost, content);
				text = string.Empty;
				foreach (Control control in array)
				{
					LiteralControl literalControl = control as LiteralControl;
					if (literalControl != null)
					{
						text += literalControl.Text;
					}
				}
			}
			catch
			{
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			propertyDescriptor.SetValue(base.Component, text);
		}

		protected override void PostFilterProperties(IDictionary properties)
		{
			base.HideAllPropertiesExceptID(properties);
			base.PostFilterAttributes(properties);
		}

		private const string DesignTimeHtml = "<span {0}=0></span>";
	}
}

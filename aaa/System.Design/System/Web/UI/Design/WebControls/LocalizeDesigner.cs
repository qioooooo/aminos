using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000464 RID: 1124
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class LocalizeDesigner : LiteralDesigner
	{
		// Token: 0x060028DD RID: 10461 RVA: 0x000E0548 File Offset: 0x000DF548
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			EditableDesignerRegion editableDesignerRegion = new EditableDesignerRegion(this, "Text");
			editableDesignerRegion.Description = SR.GetString("LocalizeDesigner_RegionWatermark");
			editableDesignerRegion.Properties[typeof(Control)] = base.Component;
			regions.Add(editableDesignerRegion);
			return string.Format(CultureInfo.InvariantCulture, "<span {0}=0></span>", new object[] { DesignerRegion.DesignerRegionAttributeName });
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000E05B4 File Offset: 0x000DF5B4
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			return (string)propertyDescriptor.GetValue(base.Component);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000E05E8 File Offset: 0x000DF5E8
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

		// Token: 0x060028E0 RID: 10464 RVA: 0x000E068C File Offset: 0x000DF68C
		protected override void PostFilterProperties(IDictionary properties)
		{
			base.HideAllPropertiesExceptID(properties);
			base.PostFilterAttributes(properties);
		}

		// Token: 0x04001C35 RID: 7221
		private const string DesignTimeHtml = "<span {0}=0></span>";
	}
}

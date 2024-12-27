using System;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048D RID: 1165
	[Obsolete("The recommended alternative is PanelContainerDesigner because it uses an EditableDesignerRegion for editing the content. Designer regions allow for better control of the content being edited. http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class PanelDesigner : ReadWriteControlDesigner
	{
		// Token: 0x06002A4D RID: 10829 RVA: 0x000E8AC8 File Offset: 0x000E7AC8
		protected override void MapPropertyToStyle(string propName, object varPropValue)
		{
			if (propName == null || varPropValue == null)
			{
				return;
			}
			if (varPropValue != null)
			{
				try
				{
					if (propName.Equals("BackImageUrl"))
					{
						string text = Convert.ToString(varPropValue, CultureInfo.InvariantCulture);
						if (text != null && text.Length != 0)
						{
							text = "url(" + text + ")";
							this.BehaviorInternal.SetStyleAttribute("backgroundImage", true, text, true);
						}
					}
					else if (propName.Equals("HorizontalAlign"))
					{
						string text2 = string.Empty;
						if ((HorizontalAlign)varPropValue != HorizontalAlign.NotSet)
						{
							text2 = Enum.Format(typeof(HorizontalAlign), varPropValue, "G");
						}
						this.BehaviorInternal.SetStyleAttribute("textAlign", true, text2, true);
					}
					else
					{
						base.MapPropertyToStyle(propName, varPropValue);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000E8B90 File Offset: 0x000E7B90
		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override void OnBehaviorAttached()
		{
			base.OnBehaviorAttached();
			Panel panel = (Panel)base.Component;
			string backImageUrl = panel.BackImageUrl;
			if (backImageUrl != null)
			{
				this.MapPropertyToStyle("BackImageUrl", backImageUrl);
			}
			HorizontalAlign horizontalAlign = panel.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				this.MapPropertyToStyle("HorizontalAlign", horizontalAlign);
			}
		}
	}
}

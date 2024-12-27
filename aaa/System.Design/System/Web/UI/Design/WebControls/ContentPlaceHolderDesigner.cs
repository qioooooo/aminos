using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200040F RID: 1039
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ContentPlaceHolderDesigner : ControlDesigner
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000CC338 File Offset: 0x000CB338
		public override bool AllowResize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000CC33C File Offset: 0x000CB33C
		private string CreateDesignTimeHTML()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			Font captionFont = SystemFonts.CaptionFont;
			Color controlText = SystemColors.ControlText;
			Color control = SystemColors.Control;
			string text = base.Component.GetType().Name + " - " + base.Component.Site.Name;
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<table cellspacing=0 cellpadding=0 style=\"border:1px solid black; width:100%; height:200px;\">\r\n            <tr>\r\n              <td style=\"width:100%; height:25px; font-family:Tahoma; font-size:{2}pt; color:{3}; background-color:{4}; padding:5px; border-bottom:1px solid black;\">\r\n                &nbsp;{0}\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td style=\"width:100%; height:175px; vertical-align:top;\" {1}=\"0\">\r\n              </td>\r\n            </tr>\r\n          </table>", new object[]
			{
				text,
				DesignerRegion.DesignerRegionAttributeName,
				captionFont.SizeInPoints,
				ColorTranslator.ToHtml(controlText),
				ColorTranslator.ToHtml(control)
			}));
			return stringBuilder.ToString();
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000CC3EC File Offset: 0x000CB3EC
		public override string GetDesignTimeHtml()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!(designerHost.RootComponent is MasterPage))
			{
				throw new InvalidOperationException(SR.GetString("ContentPlaceHolder_Invalid_RootComponent"));
			}
			return base.GetDesignTimeHtml();
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000CC434 File Offset: 0x000CB434
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!(designerHost.RootComponent is MasterPage))
			{
				throw new InvalidOperationException(SR.GetString("ContentPlaceHolder_Invalid_RootComponent"));
			}
			regions.Add(new EditableDesignerRegion(this, "Content"));
			return this.CreateDesignTimeHTML();
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000CC48C File Offset: 0x000CB48C
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			if (this._content == null)
			{
				this._content = base.Tag.GetContent();
			}
			if (this._content == null)
			{
				return string.Empty;
			}
			return this._content.Trim();
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000CC4C0 File Offset: 0x000CB4C0
		public override string GetPersistenceContent()
		{
			return this._content;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000CC4C8 File Offset: 0x000CB4C8
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			this._content = content;
			base.Tag.SetDirty(true);
		}

		// Token: 0x040019F9 RID: 6649
		private const string designtimeHTML = "<table cellspacing=0 cellpadding=0 style=\"border:1px solid black; width:100%; height:200px;\">\r\n            <tr>\r\n              <td style=\"width:100%; height:25px; font-family:Tahoma; font-size:{2}pt; color:{3}; background-color:{4}; padding:5px; border-bottom:1px solid black;\">\r\n                &nbsp;{0}\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td style=\"width:100%; height:175px; vertical-align:top;\" {1}=\"0\">\r\n              </td>\r\n            </tr>\r\n          </table>";

		// Token: 0x040019FA RID: 6650
		private string _content;
	}
}

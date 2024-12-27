using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020003FF RID: 1023
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HtmlContainerControl : HtmlControl
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x000DC9F4 File Offset: 0x000DB9F4
		protected HtmlContainerControl()
			: this("span")
		{
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000DCA01 File Offset: 0x000DBA01
		public HtmlContainerControl(string tag)
			: base(tag)
		{
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x000DCA0C File Offset: 0x000DBA0C
		// (set) Token: 0x06003272 RID: 12914 RVA: 0x000DCAAB File Offset: 0x000DBAAB
		[Browsable(false)]
		[HtmlControlPersistable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string InnerHtml
		{
			get
			{
				if (base.IsLiteralContent())
				{
					return ((LiteralControl)this.Controls[0]).Text;
				}
				if (this.HasControls() && this.Controls.Count == 1 && this.Controls[0] is DataBoundLiteralControl)
				{
					return ((DataBoundLiteralControl)this.Controls[0]).Text;
				}
				if (this.Controls.Count == 0)
				{
					return string.Empty;
				}
				throw new HttpException(SR.GetString("Inner_Content_not_literal", new object[] { this.ID }));
			}
			set
			{
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl(value));
				this.ViewState["innerhtml"] = value;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x000DCADA File Offset: 0x000DBADA
		// (set) Token: 0x06003274 RID: 12916 RVA: 0x000DCAE7 File Offset: 0x000DBAE7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[HtmlControlPersistable(false)]
		public virtual string InnerText
		{
			get
			{
				return HttpUtility.HtmlDecode(this.InnerHtml);
			}
			set
			{
				this.InnerHtml = HttpUtility.HtmlEncode(value);
			}
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000DCAF5 File Offset: 0x000DBAF5
		protected override ControlCollection CreateControlCollection()
		{
			return new ControlCollection(this);
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000DCB00 File Offset: 0x000DBB00
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				base.LoadViewState(savedState);
				string text = (string)this.ViewState["innerhtml"];
				if (text != null)
				{
					this.InnerHtml = text;
				}
			}
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000DCB37 File Offset: 0x000DBB37
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.RenderBeginTag(writer);
			this.RenderChildren(writer);
			this.RenderEndTag(writer);
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000DCB4E File Offset: 0x000DBB4E
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			this.ViewState.Remove("innerhtml");
			base.RenderAttributes(writer);
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000DCB67 File Offset: 0x000DBB67
		protected virtual void RenderEndTag(HtmlTextWriter writer)
		{
			writer.WriteEndTag(this.TagName);
		}
	}
}

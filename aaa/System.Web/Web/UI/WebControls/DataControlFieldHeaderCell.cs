using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000530 RID: 1328
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataControlFieldHeaderCell : DataControlFieldCell
	{
		// Token: 0x0600414C RID: 16716 RVA: 0x0010EF85 File Offset: 0x0010DF85
		public DataControlFieldHeaderCell(DataControlField containingField)
			: base(HtmlTextWriterTag.Th, containingField)
		{
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x0010EF90 File Offset: 0x0010DF90
		// (set) Token: 0x0600414E RID: 16718 RVA: 0x0010EFBD File Offset: 0x0010DFBD
		public virtual string AbbreviatedText
		{
			get
			{
				object obj = this.ViewState["AbbrText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["AbbrText"] = value;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x0010EFD0 File Offset: 0x0010DFD0
		// (set) Token: 0x06004150 RID: 16720 RVA: 0x0010EFF9 File Offset: 0x0010DFF9
		public virtual TableHeaderScope Scope
		{
			get
			{
				object obj = this.ViewState["Scope"];
				if (obj != null)
				{
					return (TableHeaderScope)obj;
				}
				return TableHeaderScope.NotSet;
			}
			set
			{
				this.ViewState["Scope"] = value;
			}
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x0010F014 File Offset: 0x0010E014
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			TableHeaderScope scope = this.Scope;
			if (scope != TableHeaderScope.NotSet)
			{
				if (scope == TableHeaderScope.Column)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Scope, "col");
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Scope, "row");
				}
			}
			string abbreviatedText = this.AbbreviatedText;
			if (!string.IsNullOrEmpty(abbreviatedText))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Abbr, abbreviatedText);
			}
		}
	}
}

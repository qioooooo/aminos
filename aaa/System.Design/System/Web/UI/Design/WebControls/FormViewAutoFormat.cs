using System;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200044D RID: 1101
	internal sealed class FormViewAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x000DB304 File Offset: 0x000DA304
		public FormViewAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000DB33D File Offset: 0x000DA33D
		public override void Apply(Control control)
		{
			if (control is FormView)
			{
				this.Apply(control as FormView);
			}
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000DB354 File Offset: 0x000DA354
		private void Apply(FormView view)
		{
			view.HeaderStyle.ForeColor = ColorTranslator.FromHtml(this.headerForeColor);
			view.HeaderStyle.BackColor = ColorTranslator.FromHtml(this.headerBackColor);
			view.HeaderStyle.Font.Bold = (this.headerFont & 1) != 0;
			view.HeaderStyle.Font.Italic = (this.headerFont & 2) != 0;
			view.HeaderStyle.Font.ClearDefaults();
			view.FooterStyle.ForeColor = ColorTranslator.FromHtml(this.footerForeColor);
			view.FooterStyle.BackColor = ColorTranslator.FromHtml(this.footerBackColor);
			view.FooterStyle.Font.Bold = (this.footerFont & 1) != 0;
			view.FooterStyle.Font.Italic = (this.footerFont & 2) != 0;
			view.FooterStyle.Font.ClearDefaults();
			view.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			switch (this.gridLines)
			{
			case 0:
				view.GridLines = GridLines.None;
				break;
			case 1:
				view.GridLines = GridLines.Horizontal;
				break;
			case 2:
				view.GridLines = GridLines.Vertical;
				break;
			case 3:
				view.GridLines = GridLines.Both;
				break;
			default:
				view.GridLines = GridLines.None;
				break;
			}
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				view.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				view.BorderStyle = BorderStyle.NotSet;
			}
			view.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			view.CellPadding = this.cellPadding;
			view.CellSpacing = this.cellSpacing;
			view.ForeColor = ColorTranslator.FromHtml(this.foreColor);
			view.BackColor = ColorTranslator.FromHtml(this.backColor);
			view.RowStyle.ForeColor = ColorTranslator.FromHtml(this.rowForeColor);
			view.RowStyle.BackColor = ColorTranslator.FromHtml(this.rowBackColor);
			view.RowStyle.Font.Bold = (this.itemFont & 1) != 0;
			view.RowStyle.Font.Italic = (this.itemFont & 2) != 0;
			view.RowStyle.Font.ClearDefaults();
			view.EditRowStyle.ForeColor = ColorTranslator.FromHtml(this.editRowForeColor);
			view.EditRowStyle.BackColor = ColorTranslator.FromHtml(this.editRowBackColor);
			view.EditRowStyle.Font.Bold = (this.editRowFont & 1) != 0;
			view.EditRowStyle.Font.Italic = (this.editRowFont & 2) != 0;
			view.EditRowStyle.Font.ClearDefaults();
			view.PagerStyle.ForeColor = ColorTranslator.FromHtml(this.pagerForeColor);
			view.PagerStyle.BackColor = ColorTranslator.FromHtml(this.pagerBackColor);
			view.PagerStyle.Font.Bold = (this.pagerFont & 1) != 0;
			view.PagerStyle.Font.Italic = (this.pagerFont & 2) != 0;
			view.PagerStyle.HorizontalAlign = (HorizontalAlign)this.pagerAlign;
			view.PagerStyle.Font.ClearDefaults();
			view.PagerSettings.Mode = (PagerButtons)this.pagerButtons;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000DB6B0 File Offset: 0x000DA6B0
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000DB6E8 File Offset: 0x000DA6E8
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000DB720 File Offset: 0x000DA720
		public override Control GetPreviewControl(Control runtimeControl)
		{
			Control previewControl = base.GetPreviewControl(runtimeControl);
			if (previewControl != null)
			{
				IDesignerHost designerHost = (IDesignerHost)runtimeControl.Site.GetService(typeof(IDesignerHost));
				FormView formView = previewControl as FormView;
				if (formView != null && designerHost != null)
				{
					TemplateBuilder templateBuilder = formView.ItemTemplate as TemplateBuilder;
					if ((templateBuilder != null && templateBuilder.Text.Length == 0) || formView.ItemTemplate == null)
					{
						string text = "####&nbsp;&nbsp;####<br/>####&nbsp;&nbsp;####<br/>####&nbsp;&nbsp;####<br/>####&nbsp;&nbsp;####";
						formView.ItemTemplate = ControlParser.ParseTemplate(designerHost, text);
						formView.RowStyle.HorizontalAlign = HorizontalAlign.Center;
					}
					formView.HorizontalAlign = HorizontalAlign.Center;
					formView.Width = new Unit(80.0, UnitType.Percentage);
				}
			}
			return previewControl;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000DB7C8 File Offset: 0x000DA7C8
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000DB7FC File Offset: 0x000DA7FC
		private void Load(DataRow schemeData)
		{
			this.foreColor = this.GetStringProperty("ForeColor", schemeData);
			this.backColor = this.GetStringProperty("BackColor", schemeData);
			this.borderColor = this.GetStringProperty("BorderColor", schemeData);
			this.borderWidth = this.GetStringProperty("BorderWidth", schemeData);
			this.borderStyle = this.GetIntProperty("BorderStyle", -1, schemeData);
			this.cellSpacing = this.GetIntProperty("CellSpacing", schemeData);
			this.cellPadding = this.GetIntProperty("CellPadding", -1, schemeData);
			this.gridLines = this.GetIntProperty("GridLines", -1, schemeData);
			this.rowForeColor = this.GetStringProperty("RowForeColor", schemeData);
			this.rowBackColor = this.GetStringProperty("RowBackColor", schemeData);
			this.itemFont = this.GetIntProperty("RowFont", schemeData);
			this.editRowForeColor = this.GetStringProperty("EditRowForeColor", schemeData);
			this.editRowBackColor = this.GetStringProperty("EditRowBackColor", schemeData);
			this.editRowFont = this.GetIntProperty("EditRowFont", schemeData);
			this.headerForeColor = this.GetStringProperty("HeaderForeColor", schemeData);
			this.headerBackColor = this.GetStringProperty("HeaderBackColor", schemeData);
			this.headerFont = this.GetIntProperty("HeaderFont", schemeData);
			this.footerForeColor = this.GetStringProperty("FooterForeColor", schemeData);
			this.footerBackColor = this.GetStringProperty("FooterBackColor", schemeData);
			this.footerFont = this.GetIntProperty("FooterFont", schemeData);
			this.pagerForeColor = this.GetStringProperty("PagerForeColor", schemeData);
			this.pagerBackColor = this.GetStringProperty("PagerBackColor", schemeData);
			this.pagerFont = this.GetIntProperty("PagerFont", schemeData);
			this.pagerAlign = this.GetIntProperty("PagerAlign", schemeData);
			this.pagerButtons = this.GetIntProperty("PagerButtons", 1, schemeData);
		}

		// Token: 0x04001BA9 RID: 7081
		private const int FONT_BOLD = 1;

		// Token: 0x04001BAA RID: 7082
		private const int FONT_ITALIC = 2;

		// Token: 0x04001BAB RID: 7083
		private string headerForeColor;

		// Token: 0x04001BAC RID: 7084
		private string headerBackColor;

		// Token: 0x04001BAD RID: 7085
		private int headerFont;

		// Token: 0x04001BAE RID: 7086
		private string footerForeColor;

		// Token: 0x04001BAF RID: 7087
		private string footerBackColor;

		// Token: 0x04001BB0 RID: 7088
		private int footerFont;

		// Token: 0x04001BB1 RID: 7089
		private string borderColor;

		// Token: 0x04001BB2 RID: 7090
		private string borderWidth;

		// Token: 0x04001BB3 RID: 7091
		private int borderStyle = -1;

		// Token: 0x04001BB4 RID: 7092
		private int gridLines = -1;

		// Token: 0x04001BB5 RID: 7093
		private int cellSpacing;

		// Token: 0x04001BB6 RID: 7094
		private int cellPadding = -1;

		// Token: 0x04001BB7 RID: 7095
		private string foreColor;

		// Token: 0x04001BB8 RID: 7096
		private string backColor;

		// Token: 0x04001BB9 RID: 7097
		private string rowForeColor;

		// Token: 0x04001BBA RID: 7098
		private string rowBackColor;

		// Token: 0x04001BBB RID: 7099
		private int itemFont;

		// Token: 0x04001BBC RID: 7100
		private string editRowForeColor;

		// Token: 0x04001BBD RID: 7101
		private string editRowBackColor;

		// Token: 0x04001BBE RID: 7102
		private int editRowFont;

		// Token: 0x04001BBF RID: 7103
		private string pagerForeColor;

		// Token: 0x04001BC0 RID: 7104
		private string pagerBackColor;

		// Token: 0x04001BC1 RID: 7105
		private int pagerFont;

		// Token: 0x04001BC2 RID: 7106
		private int pagerAlign;

		// Token: 0x04001BC3 RID: 7107
		private int pagerButtons;
	}
}

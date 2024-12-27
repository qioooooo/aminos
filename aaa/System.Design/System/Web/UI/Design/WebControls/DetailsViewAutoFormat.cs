using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000448 RID: 1096
	internal sealed class DetailsViewAutoFormat : DesignerAutoFormat
	{
		// Token: 0x060027C1 RID: 10177 RVA: 0x000D91C7 File Offset: 0x000D81C7
		public DetailsViewAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000D9200 File Offset: 0x000D8200
		public override void Apply(Control control)
		{
			if (control is DetailsView)
			{
				this.Apply(control as DetailsView);
			}
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000D9218 File Offset: 0x000D8218
		private void Apply(DetailsView view)
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
				view.GridLines = GridLines.Both;
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
			view.AlternatingRowStyle.ForeColor = ColorTranslator.FromHtml(this.alternatingRowForeColor);
			view.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml(this.alternatingRowBackColor);
			view.AlternatingRowStyle.Font.Bold = (this.alternatingRowFont & 1) != 0;
			view.AlternatingRowStyle.Font.Italic = (this.alternatingRowFont & 2) != 0;
			view.AlternatingRowStyle.Font.ClearDefaults();
			view.CommandRowStyle.ForeColor = ColorTranslator.FromHtml(this.commandRowForeColor);
			view.CommandRowStyle.BackColor = ColorTranslator.FromHtml(this.commandRowBackColor);
			view.CommandRowStyle.Font.Bold = (this.commandRowFont & 1) != 0;
			view.CommandRowStyle.Font.Italic = (this.commandRowFont & 2) != 0;
			view.CommandRowStyle.Font.ClearDefaults();
			view.FieldHeaderStyle.ForeColor = ColorTranslator.FromHtml(this.fieldHeaderForeColor);
			view.FieldHeaderStyle.BackColor = ColorTranslator.FromHtml(this.fieldHeaderBackColor);
			view.FieldHeaderStyle.Font.Bold = (this.fieldHeaderFont & 1) != 0;
			view.FieldHeaderStyle.Font.Italic = (this.fieldHeaderFont & 2) != 0;
			view.FieldHeaderStyle.Font.ClearDefaults();
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

		// Token: 0x060027C4 RID: 10180 RVA: 0x000D96DC File Offset: 0x000D86DC
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000D9714 File Offset: 0x000D8714
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000D974C File Offset: 0x000D874C
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000D9780 File Offset: 0x000D8780
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
			this.alternatingRowForeColor = this.GetStringProperty("AltRowForeColor", schemeData);
			this.alternatingRowBackColor = this.GetStringProperty("AltRowBackColor", schemeData);
			this.alternatingRowFont = this.GetIntProperty("AltRowFont", schemeData);
			this.commandRowForeColor = this.GetStringProperty("CommandRowForeColor", schemeData);
			this.commandRowBackColor = this.GetStringProperty("CommandRowBackColor", schemeData);
			this.commandRowFont = this.GetIntProperty("CommandRowFont", schemeData);
			this.fieldHeaderForeColor = this.GetStringProperty("FieldHeaderForeColor", schemeData);
			this.fieldHeaderBackColor = this.GetStringProperty("FieldHeaderBackColor", schemeData);
			this.fieldHeaderFont = this.GetIntProperty("FieldHeaderFont", schemeData);
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

		// Token: 0x04001B69 RID: 7017
		private const int FONT_BOLD = 1;

		// Token: 0x04001B6A RID: 7018
		private const int FONT_ITALIC = 2;

		// Token: 0x04001B6B RID: 7019
		private string headerForeColor;

		// Token: 0x04001B6C RID: 7020
		private string headerBackColor;

		// Token: 0x04001B6D RID: 7021
		private int headerFont;

		// Token: 0x04001B6E RID: 7022
		private string footerForeColor;

		// Token: 0x04001B6F RID: 7023
		private string footerBackColor;

		// Token: 0x04001B70 RID: 7024
		private int footerFont;

		// Token: 0x04001B71 RID: 7025
		private string borderColor;

		// Token: 0x04001B72 RID: 7026
		private string borderWidth;

		// Token: 0x04001B73 RID: 7027
		private int borderStyle = -1;

		// Token: 0x04001B74 RID: 7028
		private int gridLines = -1;

		// Token: 0x04001B75 RID: 7029
		private int cellSpacing;

		// Token: 0x04001B76 RID: 7030
		private int cellPadding = -1;

		// Token: 0x04001B77 RID: 7031
		private string foreColor;

		// Token: 0x04001B78 RID: 7032
		private string backColor;

		// Token: 0x04001B79 RID: 7033
		private string rowForeColor;

		// Token: 0x04001B7A RID: 7034
		private string rowBackColor;

		// Token: 0x04001B7B RID: 7035
		private int itemFont;

		// Token: 0x04001B7C RID: 7036
		private string alternatingRowForeColor;

		// Token: 0x04001B7D RID: 7037
		private string alternatingRowBackColor;

		// Token: 0x04001B7E RID: 7038
		private int alternatingRowFont;

		// Token: 0x04001B7F RID: 7039
		private string commandRowForeColor;

		// Token: 0x04001B80 RID: 7040
		private string commandRowBackColor;

		// Token: 0x04001B81 RID: 7041
		private int commandRowFont;

		// Token: 0x04001B82 RID: 7042
		private string fieldHeaderForeColor;

		// Token: 0x04001B83 RID: 7043
		private string fieldHeaderBackColor;

		// Token: 0x04001B84 RID: 7044
		private int fieldHeaderFont;

		// Token: 0x04001B85 RID: 7045
		private string editRowForeColor;

		// Token: 0x04001B86 RID: 7046
		private string editRowBackColor;

		// Token: 0x04001B87 RID: 7047
		private int editRowFont;

		// Token: 0x04001B88 RID: 7048
		private string pagerForeColor;

		// Token: 0x04001B89 RID: 7049
		private string pagerBackColor;

		// Token: 0x04001B8A RID: 7050
		private int pagerFont;

		// Token: 0x04001B8B RID: 7051
		private int pagerAlign;

		// Token: 0x04001B8C RID: 7052
		private int pagerButtons;
	}
}

﻿using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000450 RID: 1104
	internal sealed class GridViewAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x000DCDF4 File Offset: 0x000DBDF4
		public GridViewAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 260;
			base.Style.Height = 240;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000DCE62 File Offset: 0x000DBE62
		public override void Apply(Control control)
		{
			if (control is GridView)
			{
				this.Apply(control as GridView);
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000DCE78 File Offset: 0x000DBE78
		private void Apply(GridView grid)
		{
			grid.HeaderStyle.ForeColor = ColorTranslator.FromHtml(this.headerForeColor);
			grid.HeaderStyle.BackColor = ColorTranslator.FromHtml(this.headerBackColor);
			grid.HeaderStyle.Font.Bold = (this.headerFont & 1) != 0;
			grid.HeaderStyle.Font.Italic = (this.headerFont & 2) != 0;
			grid.HeaderStyle.Font.ClearDefaults();
			grid.FooterStyle.ForeColor = ColorTranslator.FromHtml(this.footerForeColor);
			grid.FooterStyle.BackColor = ColorTranslator.FromHtml(this.footerBackColor);
			grid.FooterStyle.Font.Bold = (this.footerFont & 1) != 0;
			grid.FooterStyle.Font.Italic = (this.footerFont & 2) != 0;
			grid.FooterStyle.Font.ClearDefaults();
			grid.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			switch (this.gridLines)
			{
			case 0:
				grid.GridLines = GridLines.None;
				goto IL_0147;
			case 1:
				grid.GridLines = GridLines.Horizontal;
				goto IL_0147;
			case 2:
				grid.GridLines = GridLines.Vertical;
				goto IL_0147;
			}
			grid.GridLines = GridLines.Both;
			IL_0147:
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				grid.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				grid.BorderStyle = BorderStyle.NotSet;
			}
			grid.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			grid.CellPadding = this.cellPadding;
			grid.CellSpacing = this.cellSpacing;
			grid.ForeColor = ColorTranslator.FromHtml(this.foreColor);
			grid.BackColor = ColorTranslator.FromHtml(this.backColor);
			grid.RowStyle.ForeColor = ColorTranslator.FromHtml(this.itemForeColor);
			grid.RowStyle.BackColor = ColorTranslator.FromHtml(this.itemBackColor);
			grid.RowStyle.Font.Bold = (this.itemFont & 1) != 0;
			grid.RowStyle.Font.Italic = (this.itemFont & 2) != 0;
			grid.RowStyle.Font.ClearDefaults();
			grid.AlternatingRowStyle.ForeColor = ColorTranslator.FromHtml(this.alternatingItemForeColor);
			grid.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml(this.alternatingItemBackColor);
			grid.AlternatingRowStyle.Font.Bold = (this.alternatingItemFont & 1) != 0;
			grid.AlternatingRowStyle.Font.Italic = (this.alternatingItemFont & 2) != 0;
			grid.AlternatingRowStyle.Font.ClearDefaults();
			grid.SelectedRowStyle.ForeColor = ColorTranslator.FromHtml(this.selectedItemForeColor);
			grid.SelectedRowStyle.BackColor = ColorTranslator.FromHtml(this.selectedItemBackColor);
			grid.SelectedRowStyle.Font.Bold = (this.selectedItemFont & 1) != 0;
			grid.SelectedRowStyle.Font.Italic = (this.selectedItemFont & 2) != 0;
			grid.SelectedRowStyle.Font.ClearDefaults();
			grid.PagerStyle.ForeColor = ColorTranslator.FromHtml(this.pagerForeColor);
			grid.PagerStyle.BackColor = ColorTranslator.FromHtml(this.pagerBackColor);
			grid.PagerStyle.Font.Bold = (this.pagerFont & 1) != 0;
			grid.PagerStyle.Font.Italic = (this.pagerFont & 2) != 0;
			grid.PagerStyle.HorizontalAlign = (HorizontalAlign)this.pagerAlign;
			grid.PagerStyle.Font.ClearDefaults();
			grid.PagerSettings.Mode = (PagerButtons)this.pagerButtons;
			grid.EditRowStyle.ForeColor = ColorTranslator.FromHtml(this.editItemForeColor);
			grid.EditRowStyle.BackColor = ColorTranslator.FromHtml(this.editItemBackColor);
			grid.EditRowStyle.Font.Bold = (this.editItemFont & 1) != 0;
			grid.EditRowStyle.Font.Italic = (this.editItemFont & 2) != 0;
			grid.EditRowStyle.Font.ClearDefaults();
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000DD2BC File Offset: 0x000DC2BC
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000DD2F4 File Offset: 0x000DC2F4
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000DD32C File Offset: 0x000DC32C
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000DD360 File Offset: 0x000DC360
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
			this.itemForeColor = this.GetStringProperty("ItemForeColor", schemeData);
			this.itemBackColor = this.GetStringProperty("ItemBackColor", schemeData);
			this.itemFont = this.GetIntProperty("ItemFont", schemeData);
			this.alternatingItemForeColor = this.GetStringProperty("AltItemForeColor", schemeData);
			this.alternatingItemBackColor = this.GetStringProperty("AltItemBackColor", schemeData);
			this.alternatingItemFont = this.GetIntProperty("AltItemFont", schemeData);
			this.selectedItemForeColor = this.GetStringProperty("SelItemForeColor", schemeData);
			this.selectedItemBackColor = this.GetStringProperty("SelItemBackColor", schemeData);
			this.selectedItemFont = this.GetIntProperty("SelItemFont", schemeData);
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
			this.editItemForeColor = this.GetStringProperty("EditItemForeColor", schemeData);
			this.editItemBackColor = this.GetStringProperty("EditItemBackColor", schemeData);
			this.editItemFont = this.GetIntProperty("EditItemFont", schemeData);
		}

		// Token: 0x04001BE7 RID: 7143
		private const int FONT_BOLD = 1;

		// Token: 0x04001BE8 RID: 7144
		private const int FONT_ITALIC = 2;

		// Token: 0x04001BE9 RID: 7145
		private string headerForeColor;

		// Token: 0x04001BEA RID: 7146
		private string headerBackColor;

		// Token: 0x04001BEB RID: 7147
		private int headerFont;

		// Token: 0x04001BEC RID: 7148
		private string footerForeColor;

		// Token: 0x04001BED RID: 7149
		private string footerBackColor;

		// Token: 0x04001BEE RID: 7150
		private int footerFont;

		// Token: 0x04001BEF RID: 7151
		private string borderColor;

		// Token: 0x04001BF0 RID: 7152
		private string borderWidth;

		// Token: 0x04001BF1 RID: 7153
		private int borderStyle = -1;

		// Token: 0x04001BF2 RID: 7154
		private int gridLines = -1;

		// Token: 0x04001BF3 RID: 7155
		private int cellSpacing;

		// Token: 0x04001BF4 RID: 7156
		private int cellPadding = -1;

		// Token: 0x04001BF5 RID: 7157
		private string foreColor;

		// Token: 0x04001BF6 RID: 7158
		private string backColor;

		// Token: 0x04001BF7 RID: 7159
		private string itemForeColor;

		// Token: 0x04001BF8 RID: 7160
		private string itemBackColor;

		// Token: 0x04001BF9 RID: 7161
		private int itemFont;

		// Token: 0x04001BFA RID: 7162
		private string alternatingItemForeColor;

		// Token: 0x04001BFB RID: 7163
		private string alternatingItemBackColor;

		// Token: 0x04001BFC RID: 7164
		private int alternatingItemFont;

		// Token: 0x04001BFD RID: 7165
		private string selectedItemForeColor;

		// Token: 0x04001BFE RID: 7166
		private string selectedItemBackColor;

		// Token: 0x04001BFF RID: 7167
		private int selectedItemFont;

		// Token: 0x04001C00 RID: 7168
		private string pagerForeColor;

		// Token: 0x04001C01 RID: 7169
		private string pagerBackColor;

		// Token: 0x04001C02 RID: 7170
		private int pagerFont;

		// Token: 0x04001C03 RID: 7171
		private int pagerAlign;

		// Token: 0x04001C04 RID: 7172
		private int pagerButtons;

		// Token: 0x04001C05 RID: 7173
		private string editItemForeColor;

		// Token: 0x04001C06 RID: 7174
		private string editItemBackColor;

		// Token: 0x04001C07 RID: 7175
		private int editItemFont;
	}
}

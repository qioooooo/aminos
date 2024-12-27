using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200043D RID: 1085
	internal sealed class DataGridAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002743 RID: 10051 RVA: 0x000D6454 File Offset: 0x000D5454
		public DataGridAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000D648D File Offset: 0x000D548D
		public override void Apply(Control control)
		{
			if (control is DataGrid)
			{
				this.Apply(control as DataGrid);
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000D64A4 File Offset: 0x000D54A4
		private void Apply(DataGrid grid)
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
			grid.ItemStyle.ForeColor = ColorTranslator.FromHtml(this.itemForeColor);
			grid.ItemStyle.BackColor = ColorTranslator.FromHtml(this.itemBackColor);
			grid.ItemStyle.Font.Bold = (this.itemFont & 1) != 0;
			grid.ItemStyle.Font.Italic = (this.itemFont & 2) != 0;
			grid.ItemStyle.Font.ClearDefaults();
			grid.AlternatingItemStyle.ForeColor = ColorTranslator.FromHtml(this.alternatingItemForeColor);
			grid.AlternatingItemStyle.BackColor = ColorTranslator.FromHtml(this.alternatingItemBackColor);
			grid.AlternatingItemStyle.Font.Bold = (this.alternatingItemFont & 1) != 0;
			grid.AlternatingItemStyle.Font.Italic = (this.alternatingItemFont & 2) != 0;
			grid.AlternatingItemStyle.Font.ClearDefaults();
			grid.SelectedItemStyle.ForeColor = ColorTranslator.FromHtml(this.selectedItemForeColor);
			grid.SelectedItemStyle.BackColor = ColorTranslator.FromHtml(this.selectedItemBackColor);
			grid.SelectedItemStyle.Font.Bold = (this.selectedItemFont & 1) != 0;
			grid.SelectedItemStyle.Font.Italic = (this.selectedItemFont & 2) != 0;
			grid.SelectedItemStyle.Font.ClearDefaults();
			grid.PagerStyle.ForeColor = ColorTranslator.FromHtml(this.pagerForeColor);
			grid.PagerStyle.BackColor = ColorTranslator.FromHtml(this.pagerBackColor);
			grid.PagerStyle.Font.Bold = (this.pagerFont & 1) != 0;
			grid.PagerStyle.Font.Italic = (this.pagerFont & 2) != 0;
			grid.PagerStyle.HorizontalAlign = (HorizontalAlign)this.pagerAlign;
			grid.PagerStyle.Font.ClearDefaults();
			grid.PagerStyle.Mode = (PagerMode)this.pagerMode;
			grid.EditItemStyle.ForeColor = ColorTranslator.FromHtml(this.editItemForeColor);
			grid.EditItemStyle.BackColor = ColorTranslator.FromHtml(this.editItemBackColor);
			grid.EditItemStyle.Font.Bold = (this.editItemFont & 1) != 0;
			grid.EditItemStyle.Font.Italic = (this.editItemFont & 2) != 0;
			grid.EditItemStyle.Font.ClearDefaults();
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000D68E8 File Offset: 0x000D58E8
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000D6920 File Offset: 0x000D5920
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000D6958 File Offset: 0x000D5958
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000D698C File Offset: 0x000D598C
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
			this.pagerMode = this.GetIntProperty("PagerMode", schemeData);
			this.editItemForeColor = this.GetStringProperty("EditItemForeColor", schemeData);
			this.editItemBackColor = this.GetStringProperty("EditItemBackColor", schemeData);
			this.editItemFont = this.GetIntProperty("EditItemFont", schemeData);
		}

		// Token: 0x04001AE8 RID: 6888
		private const int FONT_BOLD = 1;

		// Token: 0x04001AE9 RID: 6889
		private const int FONT_ITALIC = 2;

		// Token: 0x04001AEA RID: 6890
		private string headerForeColor;

		// Token: 0x04001AEB RID: 6891
		private string headerBackColor;

		// Token: 0x04001AEC RID: 6892
		private int headerFont;

		// Token: 0x04001AED RID: 6893
		private string footerForeColor;

		// Token: 0x04001AEE RID: 6894
		private string footerBackColor;

		// Token: 0x04001AEF RID: 6895
		private int footerFont;

		// Token: 0x04001AF0 RID: 6896
		private string borderColor;

		// Token: 0x04001AF1 RID: 6897
		private string borderWidth;

		// Token: 0x04001AF2 RID: 6898
		private int borderStyle = -1;

		// Token: 0x04001AF3 RID: 6899
		private int gridLines = -1;

		// Token: 0x04001AF4 RID: 6900
		private int cellSpacing;

		// Token: 0x04001AF5 RID: 6901
		private int cellPadding = -1;

		// Token: 0x04001AF6 RID: 6902
		private string foreColor;

		// Token: 0x04001AF7 RID: 6903
		private string backColor;

		// Token: 0x04001AF8 RID: 6904
		private string itemForeColor;

		// Token: 0x04001AF9 RID: 6905
		private string itemBackColor;

		// Token: 0x04001AFA RID: 6906
		private int itemFont;

		// Token: 0x04001AFB RID: 6907
		private string alternatingItemForeColor;

		// Token: 0x04001AFC RID: 6908
		private string alternatingItemBackColor;

		// Token: 0x04001AFD RID: 6909
		private int alternatingItemFont;

		// Token: 0x04001AFE RID: 6910
		private string selectedItemForeColor;

		// Token: 0x04001AFF RID: 6911
		private string selectedItemBackColor;

		// Token: 0x04001B00 RID: 6912
		private int selectedItemFont;

		// Token: 0x04001B01 RID: 6913
		private string pagerForeColor;

		// Token: 0x04001B02 RID: 6914
		private string pagerBackColor;

		// Token: 0x04001B03 RID: 6915
		private int pagerFont;

		// Token: 0x04001B04 RID: 6916
		private int pagerAlign;

		// Token: 0x04001B05 RID: 6917
		private int pagerMode;

		// Token: 0x04001B06 RID: 6918
		private string editItemForeColor;

		// Token: 0x04001B07 RID: 6919
		private string editItemBackColor;

		// Token: 0x04001B08 RID: 6920
		private int editItemFont;
	}
}

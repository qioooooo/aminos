using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class DataGridAutoFormat : DesignerAutoFormat
	{
		public DataGridAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		public override void Apply(Control control)
		{
			if (control is DataGrid)
			{
				this.Apply(control as DataGrid);
			}
		}

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

		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

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

		private const int FONT_BOLD = 1;

		private const int FONT_ITALIC = 2;

		private string headerForeColor;

		private string headerBackColor;

		private int headerFont;

		private string footerForeColor;

		private string footerBackColor;

		private int footerFont;

		private string borderColor;

		private string borderWidth;

		private int borderStyle = -1;

		private int gridLines = -1;

		private int cellSpacing;

		private int cellPadding = -1;

		private string foreColor;

		private string backColor;

		private string itemForeColor;

		private string itemBackColor;

		private int itemFont;

		private string alternatingItemForeColor;

		private string alternatingItemBackColor;

		private int alternatingItemFont;

		private string selectedItemForeColor;

		private string selectedItemBackColor;

		private int selectedItemFont;

		private string pagerForeColor;

		private string pagerBackColor;

		private int pagerFont;

		private int pagerAlign;

		private int pagerMode;

		private string editItemForeColor;

		private string editItemBackColor;

		private int editItemFont;
	}
}

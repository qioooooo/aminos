using System;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class DataListAutoFormat : DesignerAutoFormat
	{
		public DataListAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		public override void Apply(Control control)
		{
			if (control is DataList)
			{
				this.Apply(control as DataList);
			}
		}

		private void Apply(DataList list)
		{
			list.HeaderStyle.ForeColor = ColorTranslator.FromHtml(this.headerForeColor);
			list.HeaderStyle.BackColor = ColorTranslator.FromHtml(this.headerBackColor);
			list.HeaderStyle.Font.Bold = (this.headerFont & 1) != 0;
			list.HeaderStyle.Font.Italic = (this.headerFont & 2) != 0;
			list.HeaderStyle.Font.ClearDefaults();
			list.FooterStyle.ForeColor = ColorTranslator.FromHtml(this.footerForeColor);
			list.FooterStyle.BackColor = ColorTranslator.FromHtml(this.footerBackColor);
			list.FooterStyle.Font.Bold = (this.footerFont & 1) != 0;
			list.FooterStyle.Font.Italic = (this.footerFont & 2) != 0;
			list.FooterStyle.Font.ClearDefaults();
			list.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			switch (this.gridLines)
			{
			case 0:
				list.GridLines = GridLines.None;
				break;
			case 1:
				list.GridLines = GridLines.Horizontal;
				break;
			case 2:
				list.GridLines = GridLines.Vertical;
				break;
			case 3:
				list.GridLines = GridLines.Both;
				break;
			default:
				list.GridLines = GridLines.None;
				break;
			}
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				list.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				list.BorderStyle = BorderStyle.NotSet;
			}
			list.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			list.CellPadding = this.cellPadding;
			list.CellSpacing = this.cellSpacing;
			list.ForeColor = ColorTranslator.FromHtml(this.foreColor);
			list.BackColor = ColorTranslator.FromHtml(this.backColor);
			list.ItemStyle.ForeColor = ColorTranslator.FromHtml(this.itemForeColor);
			list.ItemStyle.BackColor = ColorTranslator.FromHtml(this.itemBackColor);
			list.ItemStyle.Font.Bold = (this.itemFont & 1) != 0;
			list.ItemStyle.Font.Italic = (this.itemFont & 2) != 0;
			list.ItemStyle.Font.ClearDefaults();
			list.AlternatingItemStyle.ForeColor = ColorTranslator.FromHtml(this.alternatingItemForeColor);
			list.AlternatingItemStyle.BackColor = ColorTranslator.FromHtml(this.alternatingItemBackColor);
			list.AlternatingItemStyle.Font.Bold = (this.alternatingItemFont & 1) != 0;
			list.AlternatingItemStyle.Font.Italic = (this.alternatingItemFont & 2) != 0;
			list.AlternatingItemStyle.Font.ClearDefaults();
			list.SelectedItemStyle.ForeColor = ColorTranslator.FromHtml(this.selectedItemForeColor);
			list.SelectedItemStyle.BackColor = ColorTranslator.FromHtml(this.selectedItemBackColor);
			list.SelectedItemStyle.Font.Bold = (this.selectedItemFont & 1) != 0;
			list.SelectedItemStyle.Font.Italic = (this.selectedItemFont & 2) != 0;
			list.SelectedItemStyle.Font.ClearDefaults();
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

		public override Control GetPreviewControl(Control runtimeControl)
		{
			Control previewControl = base.GetPreviewControl(runtimeControl);
			if (previewControl != null)
			{
				IDesignerHost designerHost = (IDesignerHost)runtimeControl.Site.GetService(typeof(IDesignerHost));
				DataList dataList = previewControl as DataList;
				if (dataList != null && designerHost != null)
				{
					TemplateBuilder templateBuilder = dataList.ItemTemplate as TemplateBuilder;
					if ((templateBuilder != null && templateBuilder.Text.Length == 0) || dataList.ItemTemplate == null)
					{
						string text = "####";
						dataList.ItemTemplate = ControlParser.ParseTemplate(designerHost, text);
						dataList.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
					}
					dataList.HorizontalAlign = HorizontalAlign.Center;
					dataList.Width = new Unit(80.0, UnitType.Percentage);
				}
			}
			return previewControl;
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
	}
}

using System;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000440 RID: 1088
	internal sealed class DataListAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002760 RID: 10080 RVA: 0x000D72B2 File Offset: 0x000D62B2
		public DataListAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000D72EB File Offset: 0x000D62EB
		public override void Apply(Control control)
		{
			if (control is DataList)
			{
				this.Apply(control as DataList);
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000D7304 File Offset: 0x000D6304
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

		// Token: 0x06002763 RID: 10083 RVA: 0x000D763C File Offset: 0x000D663C
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000D7674 File Offset: 0x000D6674
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000D76AC File Offset: 0x000D66AC
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

		// Token: 0x06002766 RID: 10086 RVA: 0x000D7754 File Offset: 0x000D6754
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000D7788 File Offset: 0x000D6788
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

		// Token: 0x04001B19 RID: 6937
		private const int FONT_BOLD = 1;

		// Token: 0x04001B1A RID: 6938
		private const int FONT_ITALIC = 2;

		// Token: 0x04001B1B RID: 6939
		private string headerForeColor;

		// Token: 0x04001B1C RID: 6940
		private string headerBackColor;

		// Token: 0x04001B1D RID: 6941
		private int headerFont;

		// Token: 0x04001B1E RID: 6942
		private string footerForeColor;

		// Token: 0x04001B1F RID: 6943
		private string footerBackColor;

		// Token: 0x04001B20 RID: 6944
		private int footerFont;

		// Token: 0x04001B21 RID: 6945
		private string borderColor;

		// Token: 0x04001B22 RID: 6946
		private string borderWidth;

		// Token: 0x04001B23 RID: 6947
		private int borderStyle = -1;

		// Token: 0x04001B24 RID: 6948
		private int gridLines = -1;

		// Token: 0x04001B25 RID: 6949
		private int cellSpacing;

		// Token: 0x04001B26 RID: 6950
		private int cellPadding = -1;

		// Token: 0x04001B27 RID: 6951
		private string foreColor;

		// Token: 0x04001B28 RID: 6952
		private string backColor;

		// Token: 0x04001B29 RID: 6953
		private string itemForeColor;

		// Token: 0x04001B2A RID: 6954
		private string itemBackColor;

		// Token: 0x04001B2B RID: 6955
		private int itemFont;

		// Token: 0x04001B2C RID: 6956
		private string alternatingItemForeColor;

		// Token: 0x04001B2D RID: 6957
		private string alternatingItemBackColor;

		// Token: 0x04001B2E RID: 6958
		private int alternatingItemFont;

		// Token: 0x04001B2F RID: 6959
		private string selectedItemForeColor;

		// Token: 0x04001B30 RID: 6960
		private string selectedItemBackColor;

		// Token: 0x04001B31 RID: 6961
		private int selectedItemFont;
	}
}

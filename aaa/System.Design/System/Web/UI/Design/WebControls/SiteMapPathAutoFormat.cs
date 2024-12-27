using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004B0 RID: 1200
	internal sealed class SiteMapPathAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002B73 RID: 11123 RVA: 0x000EFAD4 File Offset: 0x000EEAD4
		public SiteMapPathAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 400;
			base.Style.Height = 100;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000EFB2A File Offset: 0x000EEB2A
		public override void Apply(Control control)
		{
			if (control is SiteMapPath)
			{
				this.Apply(control as SiteMapPath);
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000EFB40 File Offset: 0x000EEB40
		private void Apply(SiteMapPath siteMapPath)
		{
			siteMapPath.Font.Name = this._fontName;
			siteMapPath.Font.Size = this._fontSize;
			siteMapPath.Font.ClearDefaults();
			siteMapPath.NodeStyle.Font.Bold = this._nodeStyleFontBold;
			siteMapPath.NodeStyle.ForeColor = this._nodeStyleForeColor;
			siteMapPath.NodeStyle.Font.ClearDefaults();
			siteMapPath.RootNodeStyle.Font.Bold = this._rootNodeStyleFontBold;
			siteMapPath.RootNodeStyle.ForeColor = this._rootNodeStyleForeColor;
			siteMapPath.RootNodeStyle.Font.ClearDefaults();
			siteMapPath.CurrentNodeStyle.ForeColor = this._currentNodeStyleForeColor;
			siteMapPath.PathSeparatorStyle.Font.Bold = this._pathSeparatorStyleFontBold;
			siteMapPath.PathSeparatorStyle.ForeColor = this._pathSeparatorStyleForeColor;
			siteMapPath.PathSeparatorStyle.Font.ClearDefaults();
			if (this._pathSeparator != null && this._pathSeparator.Length == 0)
			{
				this._pathSeparator = null;
			}
			siteMapPath.PathSeparator = this._pathSeparator;
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000EFC58 File Offset: 0x000EEC58
		private bool GetBoolProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			return obj != null && !obj.Equals(DBNull.Value) && bool.Parse(obj.ToString());
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000EFC8C File Offset: 0x000EEC8C
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000EFCC0 File Offset: 0x000EECC0
		private void Load(DataRow schemeData)
		{
			if (schemeData == null)
			{
				return;
			}
			this._fontName = this.GetStringProperty("FontName", schemeData);
			this._fontSize = new FontUnit(this.GetStringProperty("FontSize", schemeData), CultureInfo.InvariantCulture);
			this._pathSeparator = this.GetStringProperty("PathSeparator", schemeData);
			this._nodeStyleFontBold = this.GetBoolProperty("NodeStyleFontBold", schemeData);
			this._nodeStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("NodeStyleForeColor", schemeData));
			this._rootNodeStyleFontBold = this.GetBoolProperty("RootNodeStyleFontBold", schemeData);
			this._rootNodeStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("RootNodeStyleForeColor", schemeData));
			this._currentNodeStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("CurrentNodeStyleForeColor", schemeData));
			this._pathSeparatorStyleFontBold = this.GetBoolProperty("PathSeparatorStyleFontBold", schemeData);
			this._pathSeparatorStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("PathSeparatorStyleForeColor", schemeData));
		}

		// Token: 0x04001D84 RID: 7556
		private string _fontName;

		// Token: 0x04001D85 RID: 7557
		private FontUnit _fontSize;

		// Token: 0x04001D86 RID: 7558
		private string _pathSeparator;

		// Token: 0x04001D87 RID: 7559
		private bool _nodeStyleFontBold;

		// Token: 0x04001D88 RID: 7560
		private Color _nodeStyleForeColor;

		// Token: 0x04001D89 RID: 7561
		private bool _rootNodeStyleFontBold;

		// Token: 0x04001D8A RID: 7562
		private Color _rootNodeStyleForeColor;

		// Token: 0x04001D8B RID: 7563
		private Color _currentNodeStyleForeColor;

		// Token: 0x04001D8C RID: 7564
		private bool _pathSeparatorStyleFontBold;

		// Token: 0x04001D8D RID: 7565
		private Color _pathSeparatorStyleForeColor;
	}
}

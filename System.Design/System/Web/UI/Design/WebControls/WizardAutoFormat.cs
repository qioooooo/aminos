using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class WizardAutoFormat : DesignerAutoFormat
	{
		public WizardAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 350;
			base.Style.Height = 200;
		}

		public override void Apply(Control control)
		{
			if (control is Wizard)
			{
				this.Apply(control as Wizard);
			}
		}

		private void Apply(Wizard wizard)
		{
			wizard.Font.Name = this.FontName;
			wizard.Font.Size = this.FontSize;
			wizard.BackColor = this.BackColor;
			wizard.BorderColor = this.BorderColor;
			wizard.BorderWidth = this.BorderWidth;
			wizard.BorderStyle = this.BorderStyle;
			wizard.Font.ClearDefaults();
			wizard.NavigationButtonStyle.BorderWidth = this.NavigationButtonStyleBorderWidth;
			wizard.NavigationButtonStyle.Font.Name = this.NavigationButtonStyleFontName;
			wizard.NavigationButtonStyle.Font.Size = this.NavigationButtonStyleFontSize;
			wizard.NavigationButtonStyle.BorderStyle = this.NavigationButtonStyleBorderStyle;
			wizard.NavigationButtonStyle.BorderColor = this.NavigationButtonStyleBorderColor;
			wizard.NavigationButtonStyle.ForeColor = this.NavigationButtonStyleForeColor;
			wizard.NavigationButtonStyle.BackColor = this.NavigationButtonStyleBackColor;
			wizard.NavigationButtonStyle.Font.ClearDefaults();
			wizard.StepStyle.BorderWidth = this.StepStyleBorderWidth;
			wizard.StepStyle.BorderStyle = this.StepStyleBorderStyle;
			wizard.StepStyle.BorderColor = this.StepStyleBorderColor;
			wizard.StepStyle.ForeColor = this.StepStyleForeColor;
			wizard.StepStyle.BackColor = this.StepStyleBackColor;
			wizard.StepStyle.Font.Size = this.StepStyleFontSize;
			wizard.StepStyle.Font.ClearDefaults();
			wizard.SideBarButtonStyle.Font.Underline = this.SideBarButtonStyleFontUnderline;
			wizard.SideBarButtonStyle.Font.Name = this.SideBarButtonStyleFontName;
			wizard.SideBarButtonStyle.ForeColor = this.SideBarButtonStyleForeColor;
			wizard.SideBarButtonStyle.BorderWidth = this.SideBarButtonStyleBorderWidth;
			wizard.SideBarButtonStyle.BackColor = this.SideBarButtonStyleBackColor;
			wizard.SideBarButtonStyle.Font.ClearDefaults();
			wizard.HeaderStyle.ForeColor = this.HeaderStyleForeColor;
			wizard.HeaderStyle.BorderColor = this.HeaderStyleBorderColor;
			wizard.HeaderStyle.BackColor = this.HeaderStyleBackColor;
			wizard.HeaderStyle.Font.Size = this.HeaderStyleFontSize;
			wizard.HeaderStyle.Font.Bold = this.HeaderStyleFontBold;
			wizard.HeaderStyle.BorderWidth = this.HeaderStyleBorderWidth;
			wizard.HeaderStyle.HorizontalAlign = this.HeaderStyleHorizontalAlign;
			wizard.HeaderStyle.BorderStyle = this.HeaderStyleBorderStyle;
			wizard.HeaderStyle.Font.ClearDefaults();
			wizard.SideBarStyle.BackColor = this.SideBarStyleBackColor;
			wizard.SideBarStyle.VerticalAlign = this.SideBarStyleVerticalAlign;
			wizard.SideBarStyle.Font.Size = this.SideBarStyleFontSize;
			wizard.SideBarStyle.Font.Underline = this.SideBarStyleFontUnderline;
			wizard.SideBarStyle.Font.Strikeout = this.SideBarStyleFontStrikeout;
			wizard.SideBarStyle.BorderWidth = this.SideBarStyleBorderWidth;
			wizard.SideBarStyle.Font.ClearDefaults();
		}

		private bool GetBooleanProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			return obj != null && !obj.Equals(DBNull.Value) && bool.Parse(obj.ToString());
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
			if (schemeData == null)
			{
				return;
			}
			this.FontName = this.GetStringProperty("FontName", schemeData);
			this.FontSize = new FontUnit(this.GetStringProperty("FontSize", schemeData), CultureInfo.InvariantCulture);
			this.BackColor = ColorTranslator.FromHtml(this.GetStringProperty("BackColor", schemeData));
			this.BorderColor = ColorTranslator.FromHtml(this.GetStringProperty("BorderColor", schemeData));
			this.BorderWidth = new Unit(this.GetStringProperty("BorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.SideBarStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("SideBarStyleBackColor", schemeData));
			this.SideBarStyleVerticalAlign = (VerticalAlign)this.GetIntProperty("SideBarStyleVerticalAlign", schemeData);
			this.BorderStyle = (BorderStyle)this.GetIntProperty("BorderStyle", schemeData);
			this.NavigationButtonStyleBorderWidth = new Unit(this.GetStringProperty("NavigationButtonStyleBorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.NavigationButtonStyleFontName = this.GetStringProperty("NavigationButtonStyleFontName", schemeData);
			this.NavigationButtonStyleFontSize = new FontUnit(this.GetStringProperty("NavigationButtonStyleFontSize", schemeData), CultureInfo.InvariantCulture);
			this.NavigationButtonStyleBorderStyle = (BorderStyle)this.GetIntProperty("NavigationButtonStyleBorderStyle", schemeData);
			this.NavigationButtonStyleBorderColor = ColorTranslator.FromHtml(this.GetStringProperty("NavigationButtonStyleBorderColor", schemeData));
			this.NavigationButtonStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("NavigationButtonStyleForeColor", schemeData));
			this.NavigationButtonStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("NavigationButtonStyleBackColor", schemeData));
			this.StepStyleBorderWidth = new Unit(this.GetStringProperty("StepStyleBorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.StepStyleBorderStyle = (BorderStyle)this.GetIntProperty("StepStyleBorderStyle", schemeData);
			this.StepStyleBorderColor = ColorTranslator.FromHtml(this.GetStringProperty("StepStyleBorderColor", schemeData));
			this.StepStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("StepStyleForeColor", schemeData));
			this.StepStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("StepStyleBackColor", schemeData));
			this.StepStyleFontSize = new FontUnit(this.GetStringProperty("StepStyleFontSize", schemeData), CultureInfo.InvariantCulture);
			this.SideBarButtonStyleFontUnderline = this.GetBooleanProperty("SideBarButtonStyleFontUnderline", schemeData);
			this.SideBarButtonStyleFontName = this.GetStringProperty("SideBarButtonStyleFontName", schemeData);
			this.SideBarButtonStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("SideBarButtonStyleForeColor", schemeData));
			this.SideBarButtonStyleBorderWidth = new Unit(this.GetStringProperty("SideBarButtonStyleBorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.SideBarButtonStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("SideBarButtonStyleBackColor", schemeData));
			this.HeaderStyleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("HeaderStyleForeColor", schemeData));
			this.HeaderStyleBorderColor = ColorTranslator.FromHtml(this.GetStringProperty("HeaderStyleBorderColor", schemeData));
			this.HeaderStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("HeaderStyleBackColor", schemeData));
			this.HeaderStyleFontSize = new FontUnit(this.GetStringProperty("HeaderStyleFontSize", schemeData), CultureInfo.InvariantCulture);
			this.HeaderStyleFontBold = this.GetBooleanProperty("HeaderStyleFontBold", schemeData);
			this.HeaderStyleBorderWidth = new Unit(this.GetStringProperty("HeaderStyleBorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.HeaderStyleHorizontalAlign = (HorizontalAlign)this.GetIntProperty("HeaderStyleHorizontalAlign", schemeData);
			this.HeaderStyleBorderStyle = (BorderStyle)this.GetIntProperty("HeaderStyleBorderStyle", schemeData);
			this.SideBarStyleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("SideBarStyleBackColor", schemeData));
			this.SideBarStyleVerticalAlign = (VerticalAlign)this.GetIntProperty("SideBarStyleVerticalAlign", schemeData);
			this.SideBarStyleFontSize = new FontUnit(this.GetStringProperty("SideBarStyleFontSize", schemeData), CultureInfo.InvariantCulture);
			this.SideBarStyleFontUnderline = this.GetBooleanProperty("SideBarStyleFontUnderline", schemeData);
			this.SideBarStyleFontStrikeout = this.GetBooleanProperty("SideBarStyleFontStrikeout", schemeData);
			this.SideBarStyleBorderWidth = new Unit(this.GetStringProperty("SideBarStyleBorderWidth", schemeData), CultureInfo.InvariantCulture);
		}

		private string FontName;

		private FontUnit FontSize;

		private Color BackColor;

		private Color BorderColor;

		private Unit BorderWidth;

		private BorderStyle BorderStyle;

		private Unit NavigationButtonStyleBorderWidth;

		private string NavigationButtonStyleFontName;

		private FontUnit NavigationButtonStyleFontSize;

		private BorderStyle NavigationButtonStyleBorderStyle;

		private Color NavigationButtonStyleBorderColor;

		private Color NavigationButtonStyleForeColor;

		private Color NavigationButtonStyleBackColor;

		private Unit StepStyleBorderWidth;

		private BorderStyle StepStyleBorderStyle;

		private Color StepStyleBorderColor;

		private Color StepStyleForeColor;

		private Color StepStyleBackColor;

		private FontUnit StepStyleFontSize;

		private bool SideBarButtonStyleFontUnderline;

		private string SideBarButtonStyleFontName;

		private Color SideBarButtonStyleForeColor;

		private Unit SideBarButtonStyleBorderWidth;

		private Color SideBarButtonStyleBackColor;

		private Color HeaderStyleForeColor;

		private Color HeaderStyleBorderColor;

		private Color HeaderStyleBackColor;

		private FontUnit HeaderStyleFontSize;

		private bool HeaderStyleFontBold;

		private Unit HeaderStyleBorderWidth;

		private HorizontalAlign HeaderStyleHorizontalAlign;

		private BorderStyle HeaderStyleBorderStyle;

		private Color SideBarStyleBackColor;

		private VerticalAlign SideBarStyleVerticalAlign;

		private FontUnit SideBarStyleFontSize;

		private bool SideBarStyleFontUnderline;

		private bool SideBarStyleFontStrikeout;

		private Unit SideBarStyleBorderWidth;
	}
}

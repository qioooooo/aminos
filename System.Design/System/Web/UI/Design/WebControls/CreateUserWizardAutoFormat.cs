using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class CreateUserWizardAutoFormat : DesignerAutoFormat
	{
		public CreateUserWizardAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 500;
			base.Style.Height = 400;
		}

		public override void Apply(Control control)
		{
			if (control is CreateUserWizard)
			{
				this.Apply(control as CreateUserWizard);
			}
		}

		private void Apply(CreateUserWizard createUserWizard)
		{
			createUserWizard.StepStyle.Reset();
			createUserWizard.BackColor = ColorTranslator.FromHtml(this.backColor);
			createUserWizard.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			createUserWizard.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				createUserWizard.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				createUserWizard.BorderStyle = BorderStyle.NotSet;
			}
			createUserWizard.Font.Size = new FontUnit(this.fontSize, CultureInfo.InvariantCulture);
			createUserWizard.Font.Name = this.fontName;
			createUserWizard.Font.ClearDefaults();
			createUserWizard.TitleTextStyle.BackColor = ColorTranslator.FromHtml(this.titleTextBackColor);
			createUserWizard.TitleTextStyle.ForeColor = ColorTranslator.FromHtml(this.titleTextForeColor);
			createUserWizard.TitleTextStyle.Font.Bold = (this.titleTextFont & 1) != 0;
			createUserWizard.TitleTextStyle.Font.ClearDefaults();
			createUserWizard.StepStyle.BorderWidth = this.StepStyleBorderWidth;
			createUserWizard.StepStyle.BorderStyle = this.StepStyleBorderStyle;
			createUserWizard.StepStyle.BorderColor = this.StepStyleBorderColor;
			createUserWizard.StepStyle.ForeColor = this.StepStyleForeColor;
			createUserWizard.StepStyle.BackColor = this.StepStyleBackColor;
			createUserWizard.StepStyle.Font.Size = this.StepStyleFontSize;
			createUserWizard.StepStyle.Font.ClearDefaults();
			createUserWizard.SideBarButtonStyle.Font.Underline = this.SideBarButtonStyleFontUnderline;
			createUserWizard.SideBarButtonStyle.Font.Name = this.SideBarButtonStyleFontName;
			createUserWizard.SideBarButtonStyle.ForeColor = this.SideBarButtonStyleForeColor;
			createUserWizard.SideBarButtonStyle.BorderWidth = this.SideBarButtonStyleBorderWidth;
			createUserWizard.SideBarButtonStyle.BackColor = this.SideBarButtonStyleBackColor;
			createUserWizard.SideBarButtonStyle.Font.ClearDefaults();
			createUserWizard.NavigationButtonStyle.BorderWidth = this.NavigationButtonStyleBorderWidth;
			createUserWizard.NavigationButtonStyle.Font.Name = this.NavigationButtonStyleFontName;
			createUserWizard.NavigationButtonStyle.Font.Size = this.NavigationButtonStyleFontSize;
			createUserWizard.NavigationButtonStyle.BorderStyle = this.NavigationButtonStyleBorderStyle;
			createUserWizard.NavigationButtonStyle.BorderColor = this.NavigationButtonStyleBorderColor;
			createUserWizard.NavigationButtonStyle.ForeColor = this.NavigationButtonStyleForeColor;
			createUserWizard.NavigationButtonStyle.BackColor = this.NavigationButtonStyleBackColor;
			createUserWizard.NavigationButtonStyle.Font.ClearDefaults();
			createUserWizard.ContinueButtonStyle.BorderWidth = this.NavigationButtonStyleBorderWidth;
			createUserWizard.ContinueButtonStyle.Font.Name = this.NavigationButtonStyleFontName;
			createUserWizard.ContinueButtonStyle.Font.Size = this.NavigationButtonStyleFontSize;
			createUserWizard.ContinueButtonStyle.BorderStyle = this.NavigationButtonStyleBorderStyle;
			createUserWizard.ContinueButtonStyle.BorderColor = this.NavigationButtonStyleBorderColor;
			createUserWizard.ContinueButtonStyle.ForeColor = this.NavigationButtonStyleForeColor;
			createUserWizard.ContinueButtonStyle.BackColor = this.NavigationButtonStyleBackColor;
			createUserWizard.ContinueButtonStyle.Font.ClearDefaults();
			createUserWizard.CreateUserButtonStyle.BorderWidth = this.NavigationButtonStyleBorderWidth;
			createUserWizard.CreateUserButtonStyle.Font.Name = this.NavigationButtonStyleFontName;
			createUserWizard.CreateUserButtonStyle.Font.Size = this.NavigationButtonStyleFontSize;
			createUserWizard.CreateUserButtonStyle.BorderStyle = this.NavigationButtonStyleBorderStyle;
			createUserWizard.CreateUserButtonStyle.BorderColor = this.NavigationButtonStyleBorderColor;
			createUserWizard.CreateUserButtonStyle.ForeColor = this.NavigationButtonStyleForeColor;
			createUserWizard.CreateUserButtonStyle.BackColor = this.NavigationButtonStyleBackColor;
			createUserWizard.CreateUserButtonStyle.Font.ClearDefaults();
			createUserWizard.HeaderStyle.ForeColor = this.HeaderStyleForeColor;
			createUserWizard.HeaderStyle.BorderColor = this.HeaderStyleBorderColor;
			createUserWizard.HeaderStyle.BackColor = this.HeaderStyleBackColor;
			createUserWizard.HeaderStyle.Font.Size = this.HeaderStyleFontSize;
			createUserWizard.HeaderStyle.Font.Bold = this.HeaderStyleFontBold;
			createUserWizard.HeaderStyle.BorderWidth = this.HeaderStyleBorderWidth;
			createUserWizard.HeaderStyle.HorizontalAlign = this.HeaderStyleHorizontalAlign;
			createUserWizard.HeaderStyle.BorderStyle = this.HeaderStyleBorderStyle;
			createUserWizard.HeaderStyle.Font.ClearDefaults();
			createUserWizard.SideBarStyle.BackColor = this.SideBarStyleBackColor;
			createUserWizard.SideBarStyle.VerticalAlign = this.SideBarStyleVerticalAlign;
			createUserWizard.SideBarStyle.Font.Size = this.SideBarStyleFontSize;
			createUserWizard.SideBarStyle.Font.Underline = this.SideBarStyleFontUnderline;
			createUserWizard.SideBarStyle.Font.Strikeout = this.SideBarStyleFontStrikeout;
			createUserWizard.SideBarStyle.BorderWidth = this.SideBarStyleBorderWidth;
			createUserWizard.SideBarStyle.Font.ClearDefaults();
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
			this.backColor = this.GetStringProperty("BackColor", schemeData);
			this.borderColor = this.GetStringProperty("BorderColor", schemeData);
			this.borderWidth = this.GetStringProperty("BorderWidth", schemeData);
			this.borderStyle = this.GetIntProperty("BorderStyle", -1, schemeData);
			this.fontSize = this.GetStringProperty("FontSize", schemeData);
			this.fontName = this.GetStringProperty("FontName", schemeData);
			this.titleTextBackColor = this.GetStringProperty("TitleTextBackColor", schemeData);
			this.titleTextForeColor = this.GetStringProperty("TitleTextForeColor", schemeData);
			this.titleTextFont = this.GetIntProperty("TitleTextFont", schemeData);
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

		private const int FONT_BOLD = 1;

		private string backColor;

		private string borderColor;

		private string borderWidth;

		private int borderStyle = -1;

		private string fontSize;

		private string fontName;

		private string titleTextBackColor;

		private string titleTextForeColor;

		private int titleTextFont;

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

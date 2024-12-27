using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000413 RID: 1043
	internal sealed class CreateUserWizardAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002613 RID: 9747 RVA: 0x000CCFA4 File Offset: 0x000CBFA4
		public CreateUserWizardAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 500;
			base.Style.Height = 400;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000CD004 File Offset: 0x000CC004
		public override void Apply(Control control)
		{
			if (control is CreateUserWizard)
			{
				this.Apply(control as CreateUserWizard);
			}
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000CD01C File Offset: 0x000CC01C
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

		// Token: 0x06002616 RID: 9750 RVA: 0x000CD4EC File Offset: 0x000CC4EC
		private bool GetBooleanProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			return obj != null && !obj.Equals(DBNull.Value) && bool.Parse(obj.ToString());
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000CD520 File Offset: 0x000CC520
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000CD558 File Offset: 0x000CC558
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000CD590 File Offset: 0x000CC590
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000CD5C4 File Offset: 0x000CC5C4
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

		// Token: 0x04001A08 RID: 6664
		private const int FONT_BOLD = 1;

		// Token: 0x04001A09 RID: 6665
		private string backColor;

		// Token: 0x04001A0A RID: 6666
		private string borderColor;

		// Token: 0x04001A0B RID: 6667
		private string borderWidth;

		// Token: 0x04001A0C RID: 6668
		private int borderStyle = -1;

		// Token: 0x04001A0D RID: 6669
		private string fontSize;

		// Token: 0x04001A0E RID: 6670
		private string fontName;

		// Token: 0x04001A0F RID: 6671
		private string titleTextBackColor;

		// Token: 0x04001A10 RID: 6672
		private string titleTextForeColor;

		// Token: 0x04001A11 RID: 6673
		private int titleTextFont;

		// Token: 0x04001A12 RID: 6674
		private Unit NavigationButtonStyleBorderWidth;

		// Token: 0x04001A13 RID: 6675
		private string NavigationButtonStyleFontName;

		// Token: 0x04001A14 RID: 6676
		private FontUnit NavigationButtonStyleFontSize;

		// Token: 0x04001A15 RID: 6677
		private BorderStyle NavigationButtonStyleBorderStyle;

		// Token: 0x04001A16 RID: 6678
		private Color NavigationButtonStyleBorderColor;

		// Token: 0x04001A17 RID: 6679
		private Color NavigationButtonStyleForeColor;

		// Token: 0x04001A18 RID: 6680
		private Color NavigationButtonStyleBackColor;

		// Token: 0x04001A19 RID: 6681
		private Unit StepStyleBorderWidth;

		// Token: 0x04001A1A RID: 6682
		private BorderStyle StepStyleBorderStyle;

		// Token: 0x04001A1B RID: 6683
		private Color StepStyleBorderColor;

		// Token: 0x04001A1C RID: 6684
		private Color StepStyleForeColor;

		// Token: 0x04001A1D RID: 6685
		private Color StepStyleBackColor;

		// Token: 0x04001A1E RID: 6686
		private FontUnit StepStyleFontSize;

		// Token: 0x04001A1F RID: 6687
		private bool SideBarButtonStyleFontUnderline;

		// Token: 0x04001A20 RID: 6688
		private string SideBarButtonStyleFontName;

		// Token: 0x04001A21 RID: 6689
		private Color SideBarButtonStyleForeColor;

		// Token: 0x04001A22 RID: 6690
		private Unit SideBarButtonStyleBorderWidth;

		// Token: 0x04001A23 RID: 6691
		private Color SideBarButtonStyleBackColor;

		// Token: 0x04001A24 RID: 6692
		private Color HeaderStyleForeColor;

		// Token: 0x04001A25 RID: 6693
		private Color HeaderStyleBorderColor;

		// Token: 0x04001A26 RID: 6694
		private Color HeaderStyleBackColor;

		// Token: 0x04001A27 RID: 6695
		private FontUnit HeaderStyleFontSize;

		// Token: 0x04001A28 RID: 6696
		private bool HeaderStyleFontBold;

		// Token: 0x04001A29 RID: 6697
		private Unit HeaderStyleBorderWidth;

		// Token: 0x04001A2A RID: 6698
		private HorizontalAlign HeaderStyleHorizontalAlign;

		// Token: 0x04001A2B RID: 6699
		private BorderStyle HeaderStyleBorderStyle;

		// Token: 0x04001A2C RID: 6700
		private Color SideBarStyleBackColor;

		// Token: 0x04001A2D RID: 6701
		private VerticalAlign SideBarStyleVerticalAlign;

		// Token: 0x04001A2E RID: 6702
		private FontUnit SideBarStyleFontSize;

		// Token: 0x04001A2F RID: 6703
		private bool SideBarStyleFontUnderline;

		// Token: 0x04001A30 RID: 6704
		private bool SideBarStyleFontStrikeout;

		// Token: 0x04001A31 RID: 6705
		private Unit SideBarStyleBorderWidth;
	}
}

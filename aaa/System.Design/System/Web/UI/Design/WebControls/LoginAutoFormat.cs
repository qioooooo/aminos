using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000465 RID: 1125
	internal sealed class LoginAutoFormat : DesignerAutoFormat
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000E06A4 File Offset: 0x000DF6A4
		public LoginAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 300;
			base.Style.Height = 200;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000E070B File Offset: 0x000DF70B
		public override void Apply(Control control)
		{
			if (control is Login)
			{
				this.Apply(control as Login);
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000E0724 File Offset: 0x000DF724
		private void Apply(Login login)
		{
			login.BackColor = ColorTranslator.FromHtml(this.backColor);
			login.ForeColor = ColorTranslator.FromHtml(this.foreColor);
			login.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			login.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				login.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				login.BorderStyle = BorderStyle.NotSet;
			}
			login.Font.Size = new FontUnit(this.fontSize, CultureInfo.InvariantCulture);
			login.Font.Name = this.fontName;
			login.Font.ClearDefaults();
			login.TitleTextStyle.BackColor = ColorTranslator.FromHtml(this.titleTextBackColor);
			login.TitleTextStyle.ForeColor = ColorTranslator.FromHtml(this.titleTextForeColor);
			login.TitleTextStyle.Font.Bold = (this.titleTextFont & 1) != 0;
			login.TitleTextStyle.Font.Size = new FontUnit(this.titleTextFontSize, CultureInfo.InvariantCulture);
			login.TitleTextStyle.Font.ClearDefaults();
			login.BorderPadding = this.borderPadding;
			if (this.textLayout > 0)
			{
				login.TextLayout = LoginTextLayout.TextOnTop;
			}
			else
			{
				login.TextLayout = LoginTextLayout.TextOnLeft;
			}
			login.InstructionTextStyle.ForeColor = ColorTranslator.FromHtml(this.instructionTextForeColor);
			login.InstructionTextStyle.Font.Italic = (this.instructionTextFont & 2) != 0;
			login.InstructionTextStyle.Font.ClearDefaults();
			login.TextBoxStyle.Font.Size = new FontUnit(this.textboxFontSize, CultureInfo.InvariantCulture);
			login.TextBoxStyle.Font.ClearDefaults();
			login.LoginButtonStyle.BackColor = ColorTranslator.FromHtml(this._loginButtonBackColor);
			login.LoginButtonStyle.ForeColor = ColorTranslator.FromHtml(this._loginButtonForeColor);
			login.LoginButtonStyle.Font.Size = new FontUnit(this._loginButtonFontSize, CultureInfo.InvariantCulture);
			login.LoginButtonStyle.Font.Name = this._loginButtonFontName;
			login.LoginButtonStyle.BorderColor = ColorTranslator.FromHtml(this._loginButtonBorderColor);
			login.LoginButtonStyle.BorderWidth = new Unit(this._loginButtonBorderWidth, CultureInfo.InvariantCulture);
			if (this._loginButtonBorderStyle >= 0 && this._loginButtonBorderStyle <= 9)
			{
				login.LoginButtonStyle.BorderStyle = (BorderStyle)this._loginButtonBorderStyle;
			}
			else
			{
				login.LoginButtonStyle.BorderStyle = BorderStyle.NotSet;
			}
			login.LoginButtonStyle.Font.ClearDefaults();
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000E09C4 File Offset: 0x000DF9C4
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000E09FC File Offset: 0x000DF9FC
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000E0A34 File Offset: 0x000DFA34
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000E0A68 File Offset: 0x000DFA68
		private void Load(DataRow schemeData)
		{
			this.backColor = this.GetStringProperty("BackColor", schemeData);
			this.foreColor = this.GetStringProperty("ForeColor", schemeData);
			this.borderColor = this.GetStringProperty("BorderColor", schemeData);
			this.borderWidth = this.GetStringProperty("BorderWidth", schemeData);
			this.borderStyle = this.GetIntProperty("BorderStyle", -1, schemeData);
			this.fontSize = this.GetStringProperty("FontSize", schemeData);
			this.fontName = this.GetStringProperty("FontName", schemeData);
			this.instructionTextForeColor = this.GetStringProperty("InstructionTextForeColor", schemeData);
			this.instructionTextFont = this.GetIntProperty("InstructionTextFont", schemeData);
			this.titleTextBackColor = this.GetStringProperty("TitleTextBackColor", schemeData);
			this.titleTextForeColor = this.GetStringProperty("TitleTextForeColor", schemeData);
			this.titleTextFont = this.GetIntProperty("TitleTextFont", schemeData);
			this.titleTextFontSize = this.GetStringProperty("TitleTextFontSize", schemeData);
			this.borderPadding = this.GetIntProperty("BorderPadding", 1, schemeData);
			this.textLayout = this.GetIntProperty("TextLayout", schemeData);
			this.textboxFontSize = this.GetStringProperty("TextboxFontSize", schemeData);
			this._loginButtonBackColor = this.GetStringProperty("SubmitButtonBackColor", schemeData);
			this._loginButtonForeColor = this.GetStringProperty("SubmitButtonForeColor", schemeData);
			this._loginButtonFontSize = this.GetStringProperty("SubmitButtonFontSize", schemeData);
			this._loginButtonFontName = this.GetStringProperty("SubmitButtonFontName", schemeData);
			this._loginButtonBorderColor = this.GetStringProperty("SubmitButtonBorderColor", schemeData);
			this._loginButtonBorderWidth = this.GetStringProperty("SubmitButtonBorderWidth", schemeData);
			this._loginButtonBorderStyle = this.GetIntProperty("SubmitButtonBorderStyle", -1, schemeData);
		}

		// Token: 0x04001C36 RID: 7222
		private const int FONT_BOLD = 1;

		// Token: 0x04001C37 RID: 7223
		private const int FONT_ITALIC = 2;

		// Token: 0x04001C38 RID: 7224
		private string backColor;

		// Token: 0x04001C39 RID: 7225
		private string foreColor;

		// Token: 0x04001C3A RID: 7226
		private string borderColor;

		// Token: 0x04001C3B RID: 7227
		private string borderWidth;

		// Token: 0x04001C3C RID: 7228
		private int borderStyle = -1;

		// Token: 0x04001C3D RID: 7229
		private string fontSize;

		// Token: 0x04001C3E RID: 7230
		private string fontName;

		// Token: 0x04001C3F RID: 7231
		private string titleTextBackColor;

		// Token: 0x04001C40 RID: 7232
		private string titleTextForeColor;

		// Token: 0x04001C41 RID: 7233
		private int titleTextFont;

		// Token: 0x04001C42 RID: 7234
		private string titleTextFontSize;

		// Token: 0x04001C43 RID: 7235
		private int textLayout;

		// Token: 0x04001C44 RID: 7236
		private int borderPadding;

		// Token: 0x04001C45 RID: 7237
		private string instructionTextForeColor;

		// Token: 0x04001C46 RID: 7238
		private int instructionTextFont;

		// Token: 0x04001C47 RID: 7239
		private string textboxFontSize;

		// Token: 0x04001C48 RID: 7240
		private string _loginButtonBackColor;

		// Token: 0x04001C49 RID: 7241
		private string _loginButtonForeColor;

		// Token: 0x04001C4A RID: 7242
		private string _loginButtonFontSize;

		// Token: 0x04001C4B RID: 7243
		private string _loginButtonFontName;

		// Token: 0x04001C4C RID: 7244
		private string _loginButtonBorderColor;

		// Token: 0x04001C4D RID: 7245
		private string _loginButtonBorderWidth;

		// Token: 0x04001C4E RID: 7246
		private int _loginButtonBorderStyle = -1;
	}
}

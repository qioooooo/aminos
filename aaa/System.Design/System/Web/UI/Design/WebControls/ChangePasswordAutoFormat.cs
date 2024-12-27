using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000401 RID: 1025
	internal sealed class ChangePasswordAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002596 RID: 9622 RVA: 0x000CA924 File Offset: 0x000C9924
		public ChangePasswordAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 400;
			base.Style.Height = 250;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000CA992 File Offset: 0x000C9992
		public override void Apply(Control control)
		{
			if (control is ChangePassword)
			{
				this.Apply(control as ChangePassword);
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000CA9A8 File Offset: 0x000C99A8
		private void Apply(ChangePassword changePassword)
		{
			changePassword.BackColor = ColorTranslator.FromHtml(this._backColor);
			changePassword.BorderColor = ColorTranslator.FromHtml(this._borderColor);
			changePassword.BorderWidth = new Unit(this._borderWidth, CultureInfo.InvariantCulture);
			if (this._borderStyle >= 0 && this._borderStyle <= 9)
			{
				changePassword.BorderStyle = (BorderStyle)this._borderStyle;
			}
			else
			{
				changePassword.BorderStyle = BorderStyle.NotSet;
			}
			changePassword.Font.Size = new FontUnit(this._fontSize, CultureInfo.InvariantCulture);
			changePassword.Font.Name = this._fontName;
			changePassword.Font.ClearDefaults();
			changePassword.TitleTextStyle.BackColor = ColorTranslator.FromHtml(this._titleTextBackColor);
			changePassword.TitleTextStyle.ForeColor = ColorTranslator.FromHtml(this._titleTextForeColor);
			changePassword.TitleTextStyle.Font.Bold = (this._titleTextFont & 1) != 0;
			changePassword.TitleTextStyle.Font.Size = new FontUnit(this._titleTextFontSize, CultureInfo.InvariantCulture);
			changePassword.TitleTextStyle.Font.ClearDefaults();
			changePassword.BorderPadding = this._borderPadding;
			changePassword.InstructionTextStyle.ForeColor = ColorTranslator.FromHtml(this._instructionTextForeColor);
			changePassword.InstructionTextStyle.Font.Italic = (this._instructionTextFont & 2) != 0;
			changePassword.InstructionTextStyle.Font.ClearDefaults();
			changePassword.TextBoxStyle.Font.Size = new FontUnit(this._textboxFontSize, CultureInfo.InvariantCulture);
			changePassword.TextBoxStyle.Font.ClearDefaults();
			changePassword.ChangePasswordButtonStyle.BackColor = ColorTranslator.FromHtml(this._buttonBackColor);
			changePassword.ChangePasswordButtonStyle.ForeColor = ColorTranslator.FromHtml(this._buttonForeColor);
			changePassword.ChangePasswordButtonStyle.Font.Size = new FontUnit(this._buttonFontSize, CultureInfo.InvariantCulture);
			changePassword.ChangePasswordButtonStyle.Font.Name = this._buttonFontName;
			changePassword.ChangePasswordButtonStyle.BorderColor = ColorTranslator.FromHtml(this._buttonBorderColor);
			changePassword.ChangePasswordButtonStyle.BorderWidth = new Unit(this._buttonBorderWidth, CultureInfo.InvariantCulture);
			if (this._buttonBorderStyle >= 0 && this._buttonBorderStyle <= 9)
			{
				changePassword.ChangePasswordButtonStyle.BorderStyle = (BorderStyle)this._buttonBorderStyle;
			}
			else
			{
				changePassword.ChangePasswordButtonStyle.BorderStyle = BorderStyle.NotSet;
			}
			changePassword.ChangePasswordButtonStyle.Font.ClearDefaults();
			changePassword.ContinueButtonStyle.BackColor = ColorTranslator.FromHtml(this._buttonBackColor);
			changePassword.ContinueButtonStyle.ForeColor = ColorTranslator.FromHtml(this._buttonForeColor);
			changePassword.ContinueButtonStyle.Font.Size = new FontUnit(this._buttonFontSize, CultureInfo.InvariantCulture);
			changePassword.ContinueButtonStyle.Font.Name = this._buttonFontName;
			changePassword.ContinueButtonStyle.BorderColor = ColorTranslator.FromHtml(this._buttonBorderColor);
			changePassword.ContinueButtonStyle.BorderWidth = new Unit(this._buttonBorderWidth, CultureInfo.InvariantCulture);
			if (this._buttonBorderStyle >= 0 && this._buttonBorderStyle <= 9)
			{
				changePassword.ContinueButtonStyle.BorderStyle = (BorderStyle)this._buttonBorderStyle;
			}
			else
			{
				changePassword.ContinueButtonStyle.BorderStyle = BorderStyle.NotSet;
			}
			changePassword.ContinueButtonStyle.Font.ClearDefaults();
			changePassword.CancelButtonStyle.BackColor = ColorTranslator.FromHtml(this._buttonBackColor);
			changePassword.CancelButtonStyle.ForeColor = ColorTranslator.FromHtml(this._buttonForeColor);
			changePassword.CancelButtonStyle.Font.Size = new FontUnit(this._buttonFontSize, CultureInfo.InvariantCulture);
			changePassword.CancelButtonStyle.Font.Name = this._buttonFontName;
			changePassword.CancelButtonStyle.BorderColor = ColorTranslator.FromHtml(this._buttonBorderColor);
			changePassword.CancelButtonStyle.BorderWidth = new Unit(this._buttonBorderWidth, CultureInfo.InvariantCulture);
			if (this._buttonBorderStyle >= 0 && this._buttonBorderStyle <= 9)
			{
				changePassword.CancelButtonStyle.BorderStyle = (BorderStyle)this._buttonBorderStyle;
			}
			else
			{
				changePassword.CancelButtonStyle.BorderStyle = BorderStyle.NotSet;
			}
			changePassword.CancelButtonStyle.Font.ClearDefaults();
			changePassword.PasswordHintStyle.ForeColor = ColorTranslator.FromHtml(this._passwordHintForeColor);
			changePassword.PasswordHintStyle.Font.Italic = (this._passwordHintFont & 2) != 0;
			changePassword.PasswordHintStyle.Font.ClearDefaults();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000CAE0C File Offset: 0x000C9E0C
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000CAE44 File Offset: 0x000C9E44
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000CAE7C File Offset: 0x000C9E7C
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000CAEB0 File Offset: 0x000C9EB0
		private void Load(DataRow schemeData)
		{
			this._backColor = this.GetStringProperty("BackColor", schemeData);
			this._borderColor = this.GetStringProperty("BorderColor", schemeData);
			this._borderWidth = this.GetStringProperty("BorderWidth", schemeData);
			this._borderStyle = this.GetIntProperty("BorderStyle", -1, schemeData);
			this._fontSize = this.GetStringProperty("FontSize", schemeData);
			this._fontName = this.GetStringProperty("FontName", schemeData);
			this._titleTextBackColor = this.GetStringProperty("TitleTextBackColor", schemeData);
			this._titleTextForeColor = this.GetStringProperty("TitleTextForeColor", schemeData);
			this._titleTextFont = this.GetIntProperty("TitleTextFont", schemeData);
			this._titleTextFontSize = this.GetStringProperty("TitleTextFontSize", schemeData);
			this._instructionTextForeColor = this.GetStringProperty("InstructionTextForeColor", schemeData);
			this._instructionTextFont = this.GetIntProperty("InstructionTextFont", schemeData);
			this._borderPadding = this.GetIntProperty("BorderPadding", 1, schemeData);
			this._textboxFontSize = this.GetStringProperty("TextboxFontSize", schemeData);
			this._buttonBackColor = this.GetStringProperty("ButtonBackColor", schemeData);
			this._buttonForeColor = this.GetStringProperty("ButtonForeColor", schemeData);
			this._buttonFontSize = this.GetStringProperty("ButtonFontSize", schemeData);
			this._buttonFontName = this.GetStringProperty("ButtonFontName", schemeData);
			this._buttonBorderColor = this.GetStringProperty("ButtonBorderColor", schemeData);
			this._buttonBorderWidth = this.GetStringProperty("ButtonBorderWidth", schemeData);
			this._buttonBorderStyle = this.GetIntProperty("ButtonBorderStyle", -1, schemeData);
			this._passwordHintForeColor = this.GetStringProperty("PasswordHintForeColor", schemeData);
			this._passwordHintFont = this.GetIntProperty("PasswordHintFont", schemeData);
		}

		// Token: 0x040019C8 RID: 6600
		private const int FONT_BOLD = 1;

		// Token: 0x040019C9 RID: 6601
		private const int FONT_ITALIC = 2;

		// Token: 0x040019CA RID: 6602
		private string _backColor;

		// Token: 0x040019CB RID: 6603
		private string _borderColor;

		// Token: 0x040019CC RID: 6604
		private string _borderWidth;

		// Token: 0x040019CD RID: 6605
		private int _borderStyle = -1;

		// Token: 0x040019CE RID: 6606
		private string _fontSize;

		// Token: 0x040019CF RID: 6607
		private string _fontName;

		// Token: 0x040019D0 RID: 6608
		private string _titleTextBackColor;

		// Token: 0x040019D1 RID: 6609
		private string _titleTextForeColor;

		// Token: 0x040019D2 RID: 6610
		private int _titleTextFont;

		// Token: 0x040019D3 RID: 6611
		private string _titleTextFontSize;

		// Token: 0x040019D4 RID: 6612
		private int _borderPadding = 1;

		// Token: 0x040019D5 RID: 6613
		private string _instructionTextForeColor;

		// Token: 0x040019D6 RID: 6614
		private int _instructionTextFont;

		// Token: 0x040019D7 RID: 6615
		private string _textboxFontSize;

		// Token: 0x040019D8 RID: 6616
		private string _buttonBackColor;

		// Token: 0x040019D9 RID: 6617
		private string _buttonForeColor;

		// Token: 0x040019DA RID: 6618
		private string _buttonFontSize;

		// Token: 0x040019DB RID: 6619
		private string _buttonFontName;

		// Token: 0x040019DC RID: 6620
		private string _buttonBorderColor;

		// Token: 0x040019DD RID: 6621
		private string _buttonBorderWidth;

		// Token: 0x040019DE RID: 6622
		private int _buttonBorderStyle = -1;

		// Token: 0x040019DF RID: 6623
		private string _passwordHintForeColor;

		// Token: 0x040019E0 RID: 6624
		private int _passwordHintFont;
	}
}

using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200049D RID: 1181
	internal sealed class PasswordRecoveryAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002AC9 RID: 10953 RVA: 0x000ECD0C File Offset: 0x000EBD0C
		public PasswordRecoveryAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 500;
			base.Style.Height = 300;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000ECD7A File Offset: 0x000EBD7A
		public override void Apply(Control control)
		{
			if (control is PasswordRecovery)
			{
				this.Apply(control as PasswordRecovery);
			}
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000ECD90 File Offset: 0x000EBD90
		private void Apply(PasswordRecovery passwordRecovery)
		{
			passwordRecovery.BackColor = ColorTranslator.FromHtml(this.backColor);
			passwordRecovery.BorderColor = ColorTranslator.FromHtml(this.borderColor);
			passwordRecovery.BorderWidth = new Unit(this.borderWidth, CultureInfo.InvariantCulture);
			if (this.borderStyle >= 0 && this.borderStyle <= 9)
			{
				passwordRecovery.BorderStyle = (BorderStyle)this.borderStyle;
			}
			else
			{
				passwordRecovery.BorderStyle = BorderStyle.NotSet;
			}
			passwordRecovery.Font.Size = new FontUnit(this.fontSize, CultureInfo.InvariantCulture);
			passwordRecovery.Font.Name = this.fontName;
			passwordRecovery.Font.ClearDefaults();
			passwordRecovery.TitleTextStyle.BackColor = ColorTranslator.FromHtml(this.titleTextBackColor);
			passwordRecovery.TitleTextStyle.ForeColor = ColorTranslator.FromHtml(this.titleTextForeColor);
			passwordRecovery.TitleTextStyle.Font.Bold = (this.titleTextFont & 1) != 0;
			passwordRecovery.TitleTextStyle.Font.Size = new FontUnit(this.titleTextFontSize, CultureInfo.InvariantCulture);
			passwordRecovery.TitleTextStyle.Font.ClearDefaults();
			passwordRecovery.BorderPadding = this.borderPadding;
			passwordRecovery.InstructionTextStyle.ForeColor = ColorTranslator.FromHtml(this.instructionTextForeColor);
			passwordRecovery.InstructionTextStyle.Font.Italic = (this.instructionTextFont & 2) != 0;
			passwordRecovery.InstructionTextStyle.Font.ClearDefaults();
			passwordRecovery.TextBoxStyle.Font.Size = new FontUnit(this.textboxFontSize, CultureInfo.InvariantCulture);
			passwordRecovery.TextBoxStyle.Font.ClearDefaults();
			passwordRecovery.SubmitButtonStyle.BackColor = ColorTranslator.FromHtml(this.submitButtonBackColor);
			passwordRecovery.SubmitButtonStyle.ForeColor = ColorTranslator.FromHtml(this.submitButtonForeColor);
			passwordRecovery.SubmitButtonStyle.Font.Size = new FontUnit(this.submitButtonFontSize, CultureInfo.InvariantCulture);
			passwordRecovery.SubmitButtonStyle.Font.Name = this.submitButtonFontName;
			passwordRecovery.SubmitButtonStyle.BorderColor = ColorTranslator.FromHtml(this.submitButtonBorderColor);
			passwordRecovery.SubmitButtonStyle.BorderWidth = new Unit(this.submitButtonBorderWidth, CultureInfo.InvariantCulture);
			if (this.submitButtonBorderStyle >= 0 && this.submitButtonBorderStyle <= 9)
			{
				passwordRecovery.SubmitButtonStyle.BorderStyle = (BorderStyle)this.submitButtonBorderStyle;
			}
			else
			{
				passwordRecovery.SubmitButtonStyle.BorderStyle = BorderStyle.NotSet;
			}
			passwordRecovery.SubmitButtonStyle.Font.ClearDefaults();
			passwordRecovery.SuccessTextStyle.ForeColor = ColorTranslator.FromHtml(this.successTextForeColor);
			passwordRecovery.SuccessTextStyle.Font.Bold = (this.successTextFont & 1) != 0;
			passwordRecovery.SuccessTextStyle.Font.ClearDefaults();
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000ED048 File Offset: 0x000EC048
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000ED080 File Offset: 0x000EC080
		private int GetIntProperty(string propertyTag, int defaultValue, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000ED0B8 File Offset: 0x000EC0B8
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000ED0EC File Offset: 0x000EC0EC
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
			this.titleTextFontSize = this.GetStringProperty("TitleTextFontSize", schemeData);
			this.instructionTextForeColor = this.GetStringProperty("InstructionTextForeColor", schemeData);
			this.instructionTextFont = this.GetIntProperty("InstructionTextFont", schemeData);
			this.borderPadding = this.GetIntProperty("BorderPadding", 1, schemeData);
			this.textboxFontSize = this.GetStringProperty("TextboxFontSize", schemeData);
			this.submitButtonBackColor = this.GetStringProperty("SubmitButtonBackColor", schemeData);
			this.submitButtonForeColor = this.GetStringProperty("SubmitButtonForeColor", schemeData);
			this.submitButtonFontSize = this.GetStringProperty("SubmitButtonFontSize", schemeData);
			this.submitButtonFontName = this.GetStringProperty("SubmitButtonFontName", schemeData);
			this.submitButtonBorderColor = this.GetStringProperty("SubmitButtonBorderColor", schemeData);
			this.submitButtonBorderWidth = this.GetStringProperty("SubmitButtonBorderWidth", schemeData);
			this.submitButtonBorderStyle = this.GetIntProperty("SubmitButtonBorderStyle", -1, schemeData);
			this.successTextForeColor = this.GetStringProperty("SuccessTextForeColor", schemeData);
			this.successTextFont = this.GetIntProperty("SuccessTextFont", schemeData);
		}

		// Token: 0x04001D2F RID: 7471
		private const int FONT_BOLD = 1;

		// Token: 0x04001D30 RID: 7472
		private const int FONT_ITALIC = 2;

		// Token: 0x04001D31 RID: 7473
		private string backColor;

		// Token: 0x04001D32 RID: 7474
		private string borderColor;

		// Token: 0x04001D33 RID: 7475
		private string borderWidth;

		// Token: 0x04001D34 RID: 7476
		private int borderStyle = -1;

		// Token: 0x04001D35 RID: 7477
		private string fontSize;

		// Token: 0x04001D36 RID: 7478
		private string fontName;

		// Token: 0x04001D37 RID: 7479
		private string titleTextBackColor;

		// Token: 0x04001D38 RID: 7480
		private string titleTextForeColor;

		// Token: 0x04001D39 RID: 7481
		private int titleTextFont;

		// Token: 0x04001D3A RID: 7482
		private string titleTextFontSize;

		// Token: 0x04001D3B RID: 7483
		private int borderPadding = 1;

		// Token: 0x04001D3C RID: 7484
		private string instructionTextForeColor;

		// Token: 0x04001D3D RID: 7485
		private int instructionTextFont;

		// Token: 0x04001D3E RID: 7486
		private string textboxFontSize;

		// Token: 0x04001D3F RID: 7487
		private string submitButtonBackColor;

		// Token: 0x04001D40 RID: 7488
		private string submitButtonForeColor;

		// Token: 0x04001D41 RID: 7489
		private string submitButtonFontSize;

		// Token: 0x04001D42 RID: 7490
		private string submitButtonFontName;

		// Token: 0x04001D43 RID: 7491
		private string submitButtonBorderColor;

		// Token: 0x04001D44 RID: 7492
		private string submitButtonBorderWidth;

		// Token: 0x04001D45 RID: 7493
		private int submitButtonBorderStyle = -1;

		// Token: 0x04001D46 RID: 7494
		private string successTextForeColor;

		// Token: 0x04001D47 RID: 7495
		private int successTextFont;
	}
}

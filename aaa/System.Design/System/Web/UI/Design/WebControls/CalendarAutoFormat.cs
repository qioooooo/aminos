using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F5 RID: 1013
	internal sealed class CalendarAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x000C854C File Offset: 0x000C754C
		public CalendarAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 430;
			base.Style.Height = 280;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000C85A5 File Offset: 0x000C75A5
		public override void Apply(Control control)
		{
			if (control is global::System.Web.UI.WebControls.Calendar)
			{
				this.Apply(control as global::System.Web.UI.WebControls.Calendar);
			}
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000C85BC File Offset: 0x000C75BC
		private void Apply(global::System.Web.UI.WebControls.Calendar calendar)
		{
			calendar.Width = this.Width;
			calendar.Height = this.Height;
			calendar.Font.Name = this.FontName;
			calendar.Font.Size = this.FontSize;
			calendar.ForeColor = this.ForeColor;
			calendar.BackColor = this.BackColor;
			calendar.BorderColor = this.BorderColor;
			calendar.BorderWidth = this.BorderWidth;
			calendar.BorderStyle = this.BorderStyle;
			calendar.ShowGridLines = this.ShowGridLines;
			calendar.CellPadding = this.CellPadding;
			calendar.CellSpacing = this.CellSpacing;
			calendar.DayNameFormat = this.DayNameFormat;
			calendar.TitleFormat = this.TitleFormat;
			calendar.NextPrevFormat = this.NextPrevFormat;
			calendar.Font.ClearDefaults();
			calendar.NextPrevStyle.BackColor = this.NextPrevBackColor;
			calendar.NextPrevStyle.Font.Bold = (this.NextPrevFont & 1) != 0;
			calendar.NextPrevStyle.Font.Italic = (this.NextPrevFont & 2) != 0;
			calendar.NextPrevStyle.Font.Underline = (this.NextPrevFont & 4) != 0;
			calendar.NextPrevStyle.Font.Size = this.NextPrevFontSize;
			calendar.NextPrevStyle.ForeColor = this.NextPrevForeColor;
			calendar.NextPrevStyle.VerticalAlign = this.NextPrevVerticalAlign;
			calendar.NextPrevStyle.Font.ClearDefaults();
			calendar.TitleStyle.BackColor = this.TitleBackColor;
			calendar.TitleStyle.BorderColor = this.TitleBorderColor;
			calendar.TitleStyle.BorderStyle = this.TitleBorderStyle;
			calendar.TitleStyle.BorderWidth = this.TitleBorderWidth;
			calendar.TitleStyle.Font.Bold = (this.TitleFont & 1) != 0;
			calendar.TitleStyle.Font.Italic = (this.TitleFont & 2) != 0;
			calendar.TitleStyle.Font.Underline = (this.TitleFont & 4) != 0;
			calendar.TitleStyle.Font.Size = this.TitleFontSize;
			calendar.TitleStyle.ForeColor = this.TitleForeColor;
			calendar.TitleStyle.Height = this.TitleHeight;
			calendar.TitleStyle.Font.ClearDefaults();
			calendar.DayStyle.BackColor = this.DayBackColor;
			calendar.DayStyle.Font.Bold = (this.DayFont & 1) != 0;
			calendar.DayStyle.Font.Italic = (this.DayFont & 2) != 0;
			calendar.DayStyle.Font.Underline = (this.DayFont & 4) != 0;
			calendar.DayStyle.Font.Size = this.DayFontSize;
			calendar.DayStyle.ForeColor = this.DayForeColor;
			calendar.DayStyle.Width = this.DayWidth;
			calendar.DayStyle.Font.ClearDefaults();
			calendar.DayHeaderStyle.BackColor = this.DayHeaderBackColor;
			calendar.DayHeaderStyle.Font.Bold = (this.DayHeaderFont & 1) != 0;
			calendar.DayHeaderStyle.Font.Italic = (this.DayHeaderFont & 2) != 0;
			calendar.DayHeaderStyle.Font.Underline = (this.DayHeaderFont & 4) != 0;
			calendar.DayHeaderStyle.Font.Size = this.DayHeaderFontSize;
			calendar.DayHeaderStyle.ForeColor = this.DayHeaderForeColor;
			calendar.DayHeaderStyle.Height = this.DayHeaderHeight;
			calendar.DayHeaderStyle.Font.ClearDefaults();
			calendar.TodayDayStyle.BackColor = this.TodayDayBackColor;
			calendar.TodayDayStyle.Font.Bold = (this.TodayDayFont & 1) != 0;
			calendar.TodayDayStyle.Font.Italic = (this.TodayDayFont & 2) != 0;
			calendar.TodayDayStyle.Font.Underline = (this.TodayDayFont & 4) != 0;
			calendar.TodayDayStyle.Font.Size = this.TodayDayFontSize;
			calendar.TodayDayStyle.ForeColor = this.TodayDayForeColor;
			calendar.TodayDayStyle.Font.ClearDefaults();
			calendar.SelectedDayStyle.BackColor = this.SelectedDayBackColor;
			calendar.SelectedDayStyle.Font.Bold = (this.SelectedDayFont & 1) != 0;
			calendar.SelectedDayStyle.Font.Italic = (this.SelectedDayFont & 2) != 0;
			calendar.SelectedDayStyle.Font.Underline = (this.SelectedDayFont & 4) != 0;
			calendar.SelectedDayStyle.Font.Size = this.SelectedDayFontSize;
			calendar.SelectedDayStyle.ForeColor = this.SelectedDayForeColor;
			calendar.SelectedDayStyle.Font.ClearDefaults();
			calendar.OtherMonthDayStyle.BackColor = this.OtherMonthDayBackColor;
			calendar.OtherMonthDayStyle.Font.Bold = (this.OtherMonthDayFont & 1) != 0;
			calendar.OtherMonthDayStyle.Font.Italic = (this.OtherMonthDayFont & 2) != 0;
			calendar.OtherMonthDayStyle.Font.Underline = (this.OtherMonthDayFont & 4) != 0;
			calendar.OtherMonthDayStyle.Font.Size = this.OtherMonthDayFontSize;
			calendar.OtherMonthDayStyle.ForeColor = this.OtherMonthDayForeColor;
			calendar.OtherMonthDayStyle.Font.ClearDefaults();
			calendar.WeekendDayStyle.BackColor = this.WeekendDayBackColor;
			calendar.WeekendDayStyle.Font.Bold = (this.WeekendDayFont & 1) != 0;
			calendar.WeekendDayStyle.Font.Italic = (this.WeekendDayFont & 2) != 0;
			calendar.WeekendDayStyle.Font.Underline = (this.WeekendDayFont & 4) != 0;
			calendar.WeekendDayStyle.Font.Size = this.WeekendDayFontSize;
			calendar.WeekendDayStyle.ForeColor = this.WeekendDayForeColor;
			calendar.WeekendDayStyle.Font.ClearDefaults();
			calendar.SelectorStyle.BackColor = this.SelectorBackColor;
			calendar.SelectorStyle.Font.Bold = (this.SelectorFont & 1) != 0;
			calendar.SelectorStyle.Font.Italic = (this.SelectorFont & 2) != 0;
			calendar.SelectorStyle.Font.Underline = (this.SelectorFont & 4) != 0;
			calendar.SelectorStyle.Font.Name = this.SelectorFontName;
			calendar.SelectorStyle.Font.Size = this.SelectorFontSize;
			calendar.SelectorStyle.ForeColor = this.SelectorForeColor;
			calendar.SelectorStyle.Width = this.SelectorWidth;
			calendar.SelectorStyle.Font.ClearDefaults();
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000C8CE4 File Offset: 0x000C7CE4
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000C8D1C File Offset: 0x000C7D1C
		private int GetIntProperty(string propertyTag, DataRow schemeData, int defaultValue)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000C8D54 File Offset: 0x000C7D54
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000C8D88 File Offset: 0x000C7D88
		private string GetStringProperty(string propertyTag, DataRow schemeData, string defaultValue)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return defaultValue;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000C8DB8 File Offset: 0x000C7DB8
		private void Load(DataRow schemeData)
		{
			if (schemeData == null)
			{
				return;
			}
			this.Width = new Unit(this.GetStringProperty("Width", schemeData), CultureInfo.InvariantCulture);
			this.Height = new Unit(this.GetStringProperty("Height", schemeData), CultureInfo.InvariantCulture);
			this.FontName = this.GetStringProperty("FontName", schemeData);
			this.FontSize = new FontUnit(this.GetStringProperty("FontSize", schemeData), CultureInfo.InvariantCulture);
			this.ForeColor = ColorTranslator.FromHtml(this.GetStringProperty("ForeColor", schemeData));
			this.BackColor = ColorTranslator.FromHtml(this.GetStringProperty("BackColor", schemeData));
			this.BorderColor = ColorTranslator.FromHtml(this.GetStringProperty("BorderColor", schemeData));
			this.BorderWidth = new Unit(this.GetStringProperty("BorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.BorderStyle = (BorderStyle)Enum.Parse(typeof(BorderStyle), this.GetStringProperty("BorderStyle", schemeData, "NotSet"));
			this.ShowGridLines = bool.Parse(this.GetStringProperty("ShowGridLines", schemeData, "false"));
			this.CellPadding = this.GetIntProperty("CellPadding", schemeData, 2);
			this.CellSpacing = this.GetIntProperty("CellSpacing", schemeData);
			this.DayNameFormat = (DayNameFormat)Enum.Parse(typeof(DayNameFormat), this.GetStringProperty("DayNameFormat", schemeData, "Short"));
			this.NextPrevBackColor = ColorTranslator.FromHtml(this.GetStringProperty("NextPrevBackColor", schemeData));
			this.NextPrevFont = this.GetIntProperty("NextPrevFont", schemeData);
			this.NextPrevFontSize = new FontUnit(this.GetStringProperty("NextPrevFontSize", schemeData), CultureInfo.InvariantCulture);
			this.NextPrevForeColor = ColorTranslator.FromHtml(this.GetStringProperty("NextPrevForeColor", schemeData));
			this.NextPrevFormat = (NextPrevFormat)Enum.Parse(typeof(NextPrevFormat), this.GetStringProperty("NextPrevFormat", schemeData, "CustomText"));
			this.NextPrevVerticalAlign = (VerticalAlign)Enum.Parse(typeof(VerticalAlign), this.GetStringProperty("NextPrevVerticalAlign", schemeData, "NotSet"));
			this.TitleFormat = (TitleFormat)Enum.Parse(typeof(TitleFormat), this.GetStringProperty("TitleFormat", schemeData, "MonthYear"));
			this.TitleBackColor = ColorTranslator.FromHtml(this.GetStringProperty("TitleBackColor", schemeData));
			this.TitleBorderColor = ColorTranslator.FromHtml(this.GetStringProperty("TitleBorderColor", schemeData));
			this.TitleBorderStyle = (BorderStyle)Enum.Parse(typeof(BorderStyle), this.GetStringProperty("BorderStyle", schemeData, "NotSet"));
			this.TitleBorderWidth = new Unit(this.GetStringProperty("TitleBorderWidth", schemeData), CultureInfo.InvariantCulture);
			this.TitleFont = this.GetIntProperty("TitleFont", schemeData);
			this.TitleFontSize = new FontUnit(this.GetStringProperty("TitleFontSize", schemeData), CultureInfo.InvariantCulture);
			this.TitleForeColor = ColorTranslator.FromHtml(this.GetStringProperty("TitleForeColor", schemeData));
			this.TitleHeight = new Unit(this.GetStringProperty("TitleHeight", schemeData), CultureInfo.InvariantCulture);
			this.DayBackColor = ColorTranslator.FromHtml(this.GetStringProperty("DayBackColor", schemeData));
			this.DayFont = this.GetIntProperty("DayFont", schemeData);
			this.DayFontSize = new FontUnit(this.GetStringProperty("DayFontSize", schemeData), CultureInfo.InvariantCulture);
			this.DayForeColor = ColorTranslator.FromHtml(this.GetStringProperty("DayForeColor", schemeData));
			this.DayWidth = new Unit(this.GetStringProperty("DayWidth", schemeData), CultureInfo.InvariantCulture);
			this.DayHeaderBackColor = ColorTranslator.FromHtml(this.GetStringProperty("DayHeaderBackColor", schemeData));
			this.DayHeaderFont = this.GetIntProperty("DayHeaderFont", schemeData);
			this.DayHeaderFontSize = new FontUnit(this.GetStringProperty("DayHeaderFontSize", schemeData), CultureInfo.InvariantCulture);
			this.DayHeaderForeColor = ColorTranslator.FromHtml(this.GetStringProperty("DayHeaderForeColor", schemeData));
			this.DayHeaderHeight = new Unit(this.GetStringProperty("DayHeaderHeight", schemeData), CultureInfo.InvariantCulture);
			this.TodayDayBackColor = ColorTranslator.FromHtml(this.GetStringProperty("TodayDayBackColor", schemeData));
			this.TodayDayFont = this.GetIntProperty("TodayDayFont", schemeData);
			this.TodayDayFontSize = new FontUnit(this.GetStringProperty("TodayDayFontSize", schemeData), CultureInfo.InvariantCulture);
			this.TodayDayForeColor = ColorTranslator.FromHtml(this.GetStringProperty("TodayDayForeColor", schemeData));
			this.SelectedDayBackColor = ColorTranslator.FromHtml(this.GetStringProperty("SelectedDayBackColor", schemeData));
			this.SelectedDayFont = this.GetIntProperty("SelectedDayFont", schemeData);
			this.SelectedDayFontSize = new FontUnit(this.GetStringProperty("SelectedDayFontSize", schemeData), CultureInfo.InvariantCulture);
			this.SelectedDayForeColor = ColorTranslator.FromHtml(this.GetStringProperty("SelectedDayForeColor", schemeData));
			this.OtherMonthDayBackColor = ColorTranslator.FromHtml(this.GetStringProperty("OtherMonthDayBackColor", schemeData));
			this.OtherMonthDayFont = this.GetIntProperty("OtherMonthDayFont", schemeData);
			this.OtherMonthDayFontSize = new FontUnit(this.GetStringProperty("OtherMonthDayFontSize", schemeData), CultureInfo.InvariantCulture);
			this.OtherMonthDayForeColor = ColorTranslator.FromHtml(this.GetStringProperty("OtherMonthDayForeColor", schemeData));
			this.WeekendDayBackColor = ColorTranslator.FromHtml(this.GetStringProperty("WeekendDayBackColor", schemeData));
			this.WeekendDayFont = this.GetIntProperty("WeekendDayFont", schemeData);
			this.WeekendDayFontSize = new FontUnit(this.GetStringProperty("WeekendDayFontSize", schemeData), CultureInfo.InvariantCulture);
			this.WeekendDayForeColor = ColorTranslator.FromHtml(this.GetStringProperty("WeekendDayForeColor", schemeData));
			this.SelectorBackColor = ColorTranslator.FromHtml(this.GetStringProperty("SelectorBackColor", schemeData));
			this.SelectorFont = this.GetIntProperty("SelectorFont", schemeData);
			this.SelectorFontName = this.GetStringProperty("SelectorFontName", schemeData);
			this.SelectorFontSize = new FontUnit(this.GetStringProperty("SelectorFontSize", schemeData), CultureInfo.InvariantCulture);
			this.SelectorForeColor = ColorTranslator.FromHtml(this.GetStringProperty("SelectorForeColor", schemeData));
			this.SelectorWidth = new Unit(this.GetStringProperty("SelectorWidth", schemeData), CultureInfo.InvariantCulture);
		}

		// Token: 0x0400197E RID: 6526
		private const int FONT_BOLD = 1;

		// Token: 0x0400197F RID: 6527
		private const int FONT_ITALIC = 2;

		// Token: 0x04001980 RID: 6528
		private const int FONT_UNDERLINE = 4;

		// Token: 0x04001981 RID: 6529
		private Unit Width;

		// Token: 0x04001982 RID: 6530
		private Unit Height;

		// Token: 0x04001983 RID: 6531
		private string FontName;

		// Token: 0x04001984 RID: 6532
		private FontUnit FontSize;

		// Token: 0x04001985 RID: 6533
		private Color ForeColor;

		// Token: 0x04001986 RID: 6534
		private Color BackColor;

		// Token: 0x04001987 RID: 6535
		private Color BorderColor;

		// Token: 0x04001988 RID: 6536
		private Unit BorderWidth;

		// Token: 0x04001989 RID: 6537
		private BorderStyle BorderStyle;

		// Token: 0x0400198A RID: 6538
		private bool ShowGridLines;

		// Token: 0x0400198B RID: 6539
		private int CellPadding;

		// Token: 0x0400198C RID: 6540
		private int CellSpacing;

		// Token: 0x0400198D RID: 6541
		private DayNameFormat DayNameFormat;

		// Token: 0x0400198E RID: 6542
		private Color NextPrevBackColor;

		// Token: 0x0400198F RID: 6543
		private int NextPrevFont;

		// Token: 0x04001990 RID: 6544
		private FontUnit NextPrevFontSize;

		// Token: 0x04001991 RID: 6545
		private Color NextPrevForeColor;

		// Token: 0x04001992 RID: 6546
		private NextPrevFormat NextPrevFormat;

		// Token: 0x04001993 RID: 6547
		private VerticalAlign NextPrevVerticalAlign;

		// Token: 0x04001994 RID: 6548
		private TitleFormat TitleFormat;

		// Token: 0x04001995 RID: 6549
		private Color TitleBackColor;

		// Token: 0x04001996 RID: 6550
		private Color TitleBorderColor;

		// Token: 0x04001997 RID: 6551
		private BorderStyle TitleBorderStyle;

		// Token: 0x04001998 RID: 6552
		private Unit TitleBorderWidth;

		// Token: 0x04001999 RID: 6553
		private int TitleFont;

		// Token: 0x0400199A RID: 6554
		private FontUnit TitleFontSize;

		// Token: 0x0400199B RID: 6555
		private Color TitleForeColor;

		// Token: 0x0400199C RID: 6556
		private Unit TitleHeight;

		// Token: 0x0400199D RID: 6557
		private Color DayBackColor;

		// Token: 0x0400199E RID: 6558
		private int DayFont;

		// Token: 0x0400199F RID: 6559
		private FontUnit DayFontSize;

		// Token: 0x040019A0 RID: 6560
		private Color DayForeColor;

		// Token: 0x040019A1 RID: 6561
		private Unit DayWidth;

		// Token: 0x040019A2 RID: 6562
		private Color DayHeaderBackColor;

		// Token: 0x040019A3 RID: 6563
		private int DayHeaderFont;

		// Token: 0x040019A4 RID: 6564
		private FontUnit DayHeaderFontSize;

		// Token: 0x040019A5 RID: 6565
		private Color DayHeaderForeColor;

		// Token: 0x040019A6 RID: 6566
		private Unit DayHeaderHeight;

		// Token: 0x040019A7 RID: 6567
		private Color TodayDayBackColor;

		// Token: 0x040019A8 RID: 6568
		private int TodayDayFont;

		// Token: 0x040019A9 RID: 6569
		private FontUnit TodayDayFontSize;

		// Token: 0x040019AA RID: 6570
		private Color TodayDayForeColor;

		// Token: 0x040019AB RID: 6571
		private Color SelectedDayBackColor;

		// Token: 0x040019AC RID: 6572
		private int SelectedDayFont;

		// Token: 0x040019AD RID: 6573
		private FontUnit SelectedDayFontSize;

		// Token: 0x040019AE RID: 6574
		private Color SelectedDayForeColor;

		// Token: 0x040019AF RID: 6575
		private Color OtherMonthDayBackColor;

		// Token: 0x040019B0 RID: 6576
		private int OtherMonthDayFont;

		// Token: 0x040019B1 RID: 6577
		private FontUnit OtherMonthDayFontSize;

		// Token: 0x040019B2 RID: 6578
		private Color OtherMonthDayForeColor;

		// Token: 0x040019B3 RID: 6579
		private Color WeekendDayBackColor;

		// Token: 0x040019B4 RID: 6580
		private int WeekendDayFont;

		// Token: 0x040019B5 RID: 6581
		private FontUnit WeekendDayFontSize;

		// Token: 0x040019B6 RID: 6582
		private Color WeekendDayForeColor;

		// Token: 0x040019B7 RID: 6583
		private Color SelectorBackColor;

		// Token: 0x040019B8 RID: 6584
		private int SelectorFont;

		// Token: 0x040019B9 RID: 6585
		private string SelectorFontName;

		// Token: 0x040019BA RID: 6586
		private FontUnit SelectorFontSize;

		// Token: 0x040019BB RID: 6587
		private Color SelectorForeColor;

		// Token: 0x040019BC RID: 6588
		private Unit SelectorWidth;
	}
}

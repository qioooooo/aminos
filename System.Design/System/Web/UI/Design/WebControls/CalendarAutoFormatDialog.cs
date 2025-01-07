using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[ToolboxItem(false)]
	[Obsolete("Use of this type is not recommended because the AutoFormat dialog is launched by the designer host. The list of available AutoFormats is exposed on the ControlDesigner in the AutoFormats property. http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public partial class CalendarAutoFormatDialog : Form
	{
		public CalendarAutoFormatDialog(Calendar calendar)
		{
			this.calendar = calendar;
			this.InitForm();
		}

		protected void DoDelayLoadActions()
		{
			this.schemePreview.CreateTrident();
			this.schemePreview.ActivateTrident();
		}

		private Calendar GetPreviewCalendar()
		{
			Calendar calendar = new Calendar();
			calendar.ShowTitle = this.calendar.ShowTitle;
			calendar.ShowNextPrevMonth = this.calendar.ShowNextPrevMonth;
			calendar.ShowDayHeader = this.calendar.ShowDayHeader;
			calendar.SelectionMode = this.calendar.SelectionMode;
			CalendarAutoFormatDialog.WCScheme wcscheme = (CalendarAutoFormatDialog.WCScheme)this.schemeNameList.SelectedItem;
			wcscheme.Apply(calendar);
			return calendar;
		}

		private void InitForm()
		{
			this.schemeNameLabel = new global::System.Windows.Forms.Label();
			this.schemeNameList = new global::System.Windows.Forms.ListBox();
			this.schemePreviewLabel = new global::System.Windows.Forms.Label();
			this.schemePreview = new MSHTMLHost();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Button button = new global::System.Windows.Forms.Button();
			this.schemeNameLabel.SetBounds(8, 10, 154, 16);
			this.schemeNameLabel.Text = SR.GetString("CalAFmt_SchemeName");
			this.schemeNameLabel.TabStop = false;
			this.schemeNameLabel.TabIndex = 1;
			this.schemeNameList.TabIndex = 2;
			this.schemeNameList.SetBounds(8, 26, 150, 100);
			this.schemeNameList.UseTabStops = true;
			this.schemeNameList.IntegralHeight = false;
			this.schemeNameList.Items.AddRange(new object[]
			{
				new CalendarAutoFormatDialog.WCSchemeNone(),
				new CalendarAutoFormatDialog.WCSchemeStandard(),
				new CalendarAutoFormatDialog.WCSchemeProfessional1(),
				new CalendarAutoFormatDialog.WCSchemeProfessional2(),
				new CalendarAutoFormatDialog.WCSchemeClassic(),
				new CalendarAutoFormatDialog.WCSchemeColorful1(),
				new CalendarAutoFormatDialog.WCSchemeColorful2()
			});
			this.schemeNameList.SelectedIndexChanged += this.OnSelChangedScheme;
			this.schemePreviewLabel.SetBounds(165, 10, 92, 16);
			this.schemePreviewLabel.Text = SR.GetString("CalAFmt_Preview");
			this.schemePreviewLabel.TabStop = false;
			this.schemePreviewLabel.TabIndex = 3;
			this.schemePreview.SetBounds(165, 26, 270, 240);
			this.schemePreview.TabIndex = 4;
			this.schemePreview.TabStop = false;
			button.Location = new Point(360, 276);
			button.Size = new Size(75, 23);
			button.TabIndex = 7;
			button.Text = SR.GetString("CalAFmt_Help");
			button.FlatStyle = FlatStyle.System;
			button.Click += this.OnClickHelp;
			this.okButton.Location = new Point(198, 276);
			this.okButton.Size = new Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = SR.GetString("CalAFmt_OK");
			this.okButton.DialogResult = DialogResult.OK;
			this.okButton.FlatStyle = FlatStyle.System;
			this.okButton.Click += this.OnOKClicked;
			this.cancelButton.Location = new Point(279, 276);
			this.cancelButton.Size = new Size(75, 23);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = SR.GetString("CalAFmt_Cancel");
			this.cancelButton.FlatStyle = FlatStyle.System;
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.Text = SR.GetString("CalAFmt_Title");
			base.Size = new Size(450, 336);
			base.AcceptButton = this.okButton;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.CancelButton = this.cancelButton;
			base.Icon = null;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			base.Activated += this.OnActivated;
			base.HelpRequested += this.OnHelpRequested;
			Font dialogFont = UIServiceHelper.GetDialogFont(this.calendar.Site);
			if (dialogFont != null)
			{
				this.Font = dialogFont;
			}
			base.Controls.Clear();
			base.Controls.AddRange(new Control[] { this.schemePreview, this.schemePreviewLabel, this.schemeNameList, this.schemeNameLabel, this.okButton, this.cancelButton, button });
		}

		protected void OnActivated(object source, EventArgs e)
		{
			if (!this.firstActivate)
			{
				return;
			}
			this.schemeDirty = false;
			this.DoDelayLoadActions();
			this.schemeNameList.SelectedIndex = 0;
			this.firstActivate = false;
		}

		private void ShowHelp()
		{
			ISite site = this.calendar.Site;
			IHelpService helpService = (IHelpService)site.GetService(typeof(IHelpService));
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("net.Asp.Calendar.AutoFormat");
			}
		}

		private void OnClickHelp(object sender, EventArgs e)
		{
			this.ShowHelp();
		}

		private void OnHelpRequested(object sender, HelpEventArgs e)
		{
			this.ShowHelp();
		}

		protected void OnSelChangedScheme(object source, EventArgs e)
		{
			this.schemeDirty = true;
			this.UpdateSchemePreview();
		}

		protected void OnOKClicked(object source, EventArgs e)
		{
			this.SaveComponent();
		}

		protected void SaveComponent()
		{
			if (this.schemeDirty)
			{
				CalendarAutoFormatDialog.WCScheme wcscheme = (CalendarAutoFormatDialog.WCScheme)this.schemeNameList.SelectedItem;
				wcscheme.Apply(this.calendar);
				this.schemeDirty = false;
			}
		}

		private void UpdateSchemePreview()
		{
			Calendar previewCalendar = this.GetPreviewCalendar();
			IDesigner designer = TypeDescriptor.CreateDesigner(previewCalendar, typeof(IDesigner));
			designer.Initialize(previewCalendar);
			CalendarDesigner calendarDesigner = (CalendarDesigner)designer;
			string designTimeHtml = calendarDesigner.GetDesignTimeHtml();
			NativeMethods.IHTMLDocument2 document = this.schemePreview.GetDocument();
			NativeMethods.IHTMLElement body = document.GetBody();
			body.SetInnerHTML(designTimeHtml);
		}

		private global::System.Windows.Forms.Label schemeNameLabel;

		private global::System.Windows.Forms.ListBox schemeNameList;

		private global::System.Windows.Forms.Label schemePreviewLabel;

		private global::System.Windows.Forms.Button cancelButton;

		private global::System.Windows.Forms.Button okButton;

		private MSHTMLHost schemePreview;

		private Calendar calendar;

		private bool schemeDirty;

		private bool firstActivate = true;

		private abstract class WCScheme
		{
			public abstract string GetDescription();

			public abstract void Apply(Calendar wc);

			public override string ToString()
			{
				return this.GetDescription();
			}

			public static void ClearCalendar(Calendar wc)
			{
				wc.TitleStyle.Reset();
				wc.NextPrevStyle.Reset();
				wc.DayHeaderStyle.Reset();
				wc.SelectorStyle.Reset();
				wc.DayStyle.Reset();
				wc.OtherMonthDayStyle.Reset();
				wc.WeekendDayStyle.Reset();
				wc.TodayDayStyle.Reset();
				wc.SelectedDayStyle.Reset();
				wc.ControlStyle.Reset();
			}
		}

		private class WCSchemeNone : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Default");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.Short;
				wc.NextPrevFormat = NextPrevFormat.CustomText;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 2;
				wc.CellSpacing = 0;
				wc.ShowGridLines = false;
			}
		}

		private class WCSchemeStandard : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Simple");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.FirstLetter;
				wc.NextPrevFormat = NextPrevFormat.CustomText;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 4;
				wc.CellSpacing = 0;
				wc.ShowGridLines = false;
				wc.Height = Unit.Pixel(180);
				wc.Width = Unit.Pixel(200);
				wc.BorderColor = Color.FromArgb(153, 153, 153);
				wc.ForeColor = Color.Black;
				wc.BackColor = Color.White;
				wc.Font.Name = "Verdana";
				wc.Font.Size = FontUnit.Point(8);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.BorderColor = Color.Black;
				wc.TitleStyle.BackColor = Color.FromArgb(153, 153, 153);
				wc.NextPrevStyle.VerticalAlign = VerticalAlign.Bottom;
				wc.DayHeaderStyle.Font.Bold = true;
				wc.DayHeaderStyle.Font.Size = FontUnit.Point(7);
				wc.DayHeaderStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.SelectorStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.TodayDayStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.TodayDayStyle.ForeColor = Color.Black;
				wc.SelectedDayStyle.BackColor = Color.FromArgb(102, 102, 102);
				wc.SelectedDayStyle.ForeColor = Color.White;
				wc.SelectedDayStyle.Font.Bold = true;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(128, 128, 128);
				wc.WeekendDayStyle.BackColor = Color.FromArgb(255, 255, 204);
			}
		}

		private class WCSchemeProfessional1 : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Professional1");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.Short;
				wc.NextPrevFormat = NextPrevFormat.FullMonth;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 2;
				wc.CellSpacing = 0;
				wc.ShowGridLines = false;
				wc.Height = Unit.Pixel(190);
				wc.Width = Unit.Pixel(350);
				wc.BorderColor = Color.White;
				wc.BorderWidth = Unit.Pixel(1);
				wc.ForeColor = Color.Black;
				wc.BackColor = Color.White;
				wc.Font.Name = "Verdana";
				wc.Font.Size = FontUnit.Point(9);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.BorderColor = Color.Black;
				wc.TitleStyle.BorderWidth = Unit.Pixel(4);
				wc.TitleStyle.ForeColor = Color.FromArgb(51, 51, 153);
				wc.TitleStyle.BackColor = Color.White;
				wc.TitleStyle.Font.Size = FontUnit.Point(12);
				wc.NextPrevStyle.Font.Bold = true;
				wc.NextPrevStyle.Font.Size = FontUnit.Point(8);
				wc.NextPrevStyle.VerticalAlign = VerticalAlign.Bottom;
				wc.NextPrevStyle.ForeColor = Color.FromArgb(51, 51, 51);
				wc.DayHeaderStyle.Font.Bold = true;
				wc.DayHeaderStyle.Font.Size = FontUnit.Point(8);
				wc.TodayDayStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.SelectedDayStyle.BackColor = Color.FromArgb(51, 51, 153);
				wc.SelectedDayStyle.ForeColor = Color.White;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(153, 153, 153);
			}
		}

		private class WCSchemeProfessional2 : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Professional2");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.Short;
				wc.NextPrevFormat = NextPrevFormat.ShortMonth;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 2;
				wc.CellSpacing = 1;
				wc.ShowGridLines = false;
				wc.Height = Unit.Pixel(250);
				wc.Width = Unit.Pixel(330);
				wc.BackColor = Color.White;
				wc.BorderColor = Color.Black;
				wc.BorderStyle = global::System.Web.UI.WebControls.BorderStyle.Solid;
				wc.ForeColor = Color.Black;
				wc.Font.Name = "Verdana";
				wc.Font.Size = FontUnit.Point(9);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.ForeColor = Color.White;
				wc.TitleStyle.BackColor = Color.FromArgb(51, 51, 153);
				wc.TitleStyle.Font.Size = FontUnit.Point(12);
				wc.TitleStyle.Height = Unit.Point(12);
				wc.NextPrevStyle.Font.Bold = true;
				wc.NextPrevStyle.Font.Size = FontUnit.Point(8);
				wc.NextPrevStyle.ForeColor = Color.White;
				wc.DayHeaderStyle.ForeColor = Color.FromArgb(51, 51, 51);
				wc.DayHeaderStyle.Font.Bold = true;
				wc.DayHeaderStyle.Font.Size = FontUnit.Point(8);
				wc.DayHeaderStyle.Height = Unit.Point(8);
				wc.DayStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.TodayDayStyle.BackColor = Color.FromArgb(153, 153, 153);
				wc.TodayDayStyle.ForeColor = Color.White;
				wc.SelectedDayStyle.BackColor = Color.FromArgb(51, 51, 153);
				wc.SelectedDayStyle.ForeColor = Color.White;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(153, 153, 153);
			}
		}

		private class WCSchemeClassic : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Classic");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.FirstLetter;
				wc.NextPrevFormat = NextPrevFormat.FullMonth;
				wc.TitleFormat = TitleFormat.Month;
				wc.CellPadding = 2;
				wc.CellSpacing = 0;
				wc.ShowGridLines = false;
				wc.Height = Unit.Pixel(220);
				wc.Width = Unit.Pixel(400);
				wc.BackColor = Color.White;
				wc.BorderColor = Color.Black;
				wc.ForeColor = Color.Black;
				wc.Font.Name = "Times New Roman";
				wc.Font.Size = FontUnit.Point(10);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.ForeColor = Color.White;
				wc.TitleStyle.BackColor = Color.Black;
				wc.TitleStyle.Font.Size = FontUnit.Point(13);
				wc.TitleStyle.Height = Unit.Point(14);
				wc.NextPrevStyle.ForeColor = Color.White;
				wc.NextPrevStyle.Font.Size = FontUnit.Point(8);
				wc.DayHeaderStyle.Font.Bold = true;
				wc.DayHeaderStyle.Font.Size = FontUnit.Point(7);
				wc.DayHeaderStyle.Font.Name = "Verdana";
				wc.DayHeaderStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.DayHeaderStyle.ForeColor = Color.FromArgb(51, 51, 51);
				wc.DayHeaderStyle.Height = Unit.Pixel(10);
				wc.SelectorStyle.BackColor = Color.FromArgb(204, 204, 204);
				wc.SelectorStyle.ForeColor = Color.FromArgb(51, 51, 51);
				wc.SelectorStyle.Font.Bold = true;
				wc.SelectorStyle.Font.Size = FontUnit.Point(8);
				wc.SelectorStyle.Font.Name = "Verdana";
				wc.SelectorStyle.Width = Unit.Percentage(1.0);
				wc.DayStyle.Width = Unit.Percentage(14.0);
				wc.TodayDayStyle.BackColor = Color.FromArgb(204, 204, 153);
				wc.SelectedDayStyle.BackColor = Color.FromArgb(204, 51, 51);
				wc.SelectedDayStyle.ForeColor = Color.White;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(153, 153, 153);
			}
		}

		private class WCSchemeColorful1 : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Colorful1");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.FirstLetter;
				wc.NextPrevFormat = NextPrevFormat.CustomText;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 2;
				wc.CellSpacing = 0;
				wc.ShowGridLines = true;
				wc.Height = Unit.Pixel(200);
				wc.Width = Unit.Pixel(220);
				wc.BackColor = Color.FromArgb(255, 255, 204);
				wc.BorderColor = Color.FromArgb(255, 204, 102);
				wc.BorderWidth = Unit.Pixel(1);
				wc.ForeColor = Color.FromArgb(102, 51, 153);
				wc.Font.Name = "Verdana";
				wc.Font.Size = FontUnit.Point(8);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.Font.Size = FontUnit.Point(9);
				wc.TitleStyle.BackColor = Color.FromArgb(153, 0, 0);
				wc.TitleStyle.ForeColor = Color.FromArgb(255, 255, 204);
				wc.NextPrevStyle.ForeColor = Color.FromArgb(255, 255, 204);
				wc.NextPrevStyle.Font.Size = FontUnit.Point(9);
				wc.DayHeaderStyle.BackColor = Color.FromArgb(255, 204, 102);
				wc.DayHeaderStyle.Height = Unit.Pixel(1);
				wc.SelectorStyle.BackColor = Color.FromArgb(255, 204, 102);
				wc.SelectedDayStyle.BackColor = Color.FromArgb(204, 204, 255);
				wc.SelectedDayStyle.Font.Bold = true;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(204, 153, 102);
				wc.TodayDayStyle.ForeColor = Color.White;
				wc.TodayDayStyle.BackColor = Color.FromArgb(255, 204, 102);
			}
		}

		private class WCSchemeColorful2 : CalendarAutoFormatDialog.WCScheme
		{
			public override string GetDescription()
			{
				return SR.GetString("CalAFmt_Scheme_Colorful2");
			}

			public override void Apply(Calendar wc)
			{
				CalendarAutoFormatDialog.WCScheme.ClearCalendar(wc);
				wc.DayNameFormat = DayNameFormat.FirstLetter;
				wc.NextPrevFormat = NextPrevFormat.CustomText;
				wc.TitleFormat = TitleFormat.MonthYear;
				wc.CellPadding = 1;
				wc.CellSpacing = 0;
				wc.ShowGridLines = false;
				wc.Height = Unit.Pixel(200);
				wc.Width = Unit.Pixel(220);
				wc.BackColor = Color.White;
				wc.BorderColor = Color.FromArgb(51, 102, 204);
				wc.BorderWidth = Unit.Pixel(1);
				wc.ForeColor = Color.FromArgb(0, 51, 153);
				wc.Font.Name = "Verdana";
				wc.Font.Size = FontUnit.Point(8);
				wc.TitleStyle.Font.Bold = true;
				wc.TitleStyle.Font.Size = FontUnit.Point(10);
				wc.TitleStyle.BackColor = Color.FromArgb(0, 51, 153);
				wc.TitleStyle.ForeColor = Color.FromArgb(204, 204, 255);
				wc.TitleStyle.BorderColor = Color.FromArgb(51, 102, 204);
				wc.TitleStyle.BorderStyle = global::System.Web.UI.WebControls.BorderStyle.Solid;
				wc.TitleStyle.BorderWidth = Unit.Pixel(1);
				wc.TitleStyle.Height = Unit.Pixel(25);
				wc.NextPrevStyle.ForeColor = Color.FromArgb(204, 204, 255);
				wc.NextPrevStyle.Font.Size = FontUnit.Point(8);
				wc.DayHeaderStyle.BackColor = Color.FromArgb(153, 204, 204);
				wc.DayHeaderStyle.ForeColor = Color.FromArgb(51, 102, 102);
				wc.DayHeaderStyle.Height = Unit.Pixel(1);
				wc.SelectorStyle.BackColor = Color.FromArgb(153, 204, 204);
				wc.SelectorStyle.ForeColor = Color.FromArgb(51, 102, 102);
				wc.SelectedDayStyle.BackColor = Color.FromArgb(0, 153, 153);
				wc.SelectedDayStyle.ForeColor = Color.FromArgb(204, 255, 153);
				wc.SelectedDayStyle.Font.Bold = true;
				wc.OtherMonthDayStyle.ForeColor = Color.FromArgb(153, 153, 153);
				wc.TodayDayStyle.ForeColor = Color.White;
				wc.TodayDayStyle.BackColor = Color.FromArgb(153, 204, 204);
				wc.WeekendDayStyle.BackColor = Color.FromArgb(204, 204, 255);
			}
		}
	}
}

using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	public class DateTimeEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					using (DateTimeEditor.DateTimeUI dateTimeUI = new DateTimeEditor.DateTimeUI())
					{
						dateTimeUI.Start(windowsFormsEditorService, value);
						windowsFormsEditorService.DropDownControl(dateTimeUI);
						value = dateTimeUI.Value;
						dateTimeUI.End();
					}
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private class DateTimeUI : Control
		{
			public DateTimeUI()
			{
				this.InitializeComponent();
				base.Size = this.monthCalendar.SingleMonthSize;
				this.monthCalendar.Resize += this.MonthCalResize;
			}

			public object Value
			{
				get
				{
					return this.value;
				}
			}

			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			private void MonthCalKeyDown(object sender, KeyEventArgs e)
			{
				Keys keyCode = e.KeyCode;
				if (keyCode != Keys.Return)
				{
					return;
				}
				this.OnDateSelected(sender, null);
			}

			private void InitializeComponent()
			{
				this.monthCalendar.DateSelected += this.OnDateSelected;
				this.monthCalendar.KeyDown += this.MonthCalKeyDown;
				base.Controls.Add(this.monthCalendar);
			}

			private void MonthCalResize(object sender, EventArgs e)
			{
				base.Size = this.monthCalendar.Size;
			}

			private void OnDateSelected(object sender, DateRangeEventArgs e)
			{
				this.value = this.monthCalendar.SelectionStart;
				this.edSvc.CloseDropDown();
			}

			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.monthCalendar.Focus();
			}

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				if (value != null)
				{
					DateTime dateTime = (DateTime)value;
					this.monthCalendar.SetDate(dateTime.Equals(DateTime.MinValue) ? DateTime.Today : dateTime);
				}
			}

			private MonthCalendar monthCalendar = new DateTimeEditor.DateTimeUI.DateTimeMonthCalendar();

			private object value;

			private IWindowsFormsEditorService edSvc;

			private class DateTimeMonthCalendar : MonthCalendar
			{
				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Return || base.IsInputKey(keyData);
				}
			}
		}
	}
}

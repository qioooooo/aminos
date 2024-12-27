using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	// Token: 0x020000FB RID: 251
	public class DateTimeEditor : UITypeEditor
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x00028DC8 File Offset: 0x00027DC8
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

		// Token: 0x06000A73 RID: 2675 RVA: 0x00028E34 File Offset: 0x00027E34
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x020000FC RID: 252
		private class DateTimeUI : Control
		{
			// Token: 0x06000A75 RID: 2677 RVA: 0x00028E40 File Offset: 0x00027E40
			public DateTimeUI()
			{
				this.InitializeComponent();
				base.Size = this.monthCalendar.SingleMonthSize;
				this.monthCalendar.Resize += this.MonthCalResize;
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00028E8C File Offset: 0x00027E8C
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06000A77 RID: 2679 RVA: 0x00028E94 File Offset: 0x00027E94
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x06000A78 RID: 2680 RVA: 0x00028EA4 File Offset: 0x00027EA4
			private void MonthCalKeyDown(object sender, KeyEventArgs e)
			{
				Keys keyCode = e.KeyCode;
				if (keyCode != Keys.Return)
				{
					return;
				}
				this.OnDateSelected(sender, null);
			}

			// Token: 0x06000A79 RID: 2681 RVA: 0x00028EC8 File Offset: 0x00027EC8
			private void InitializeComponent()
			{
				this.monthCalendar.DateSelected += this.OnDateSelected;
				this.monthCalendar.KeyDown += this.MonthCalKeyDown;
				base.Controls.Add(this.monthCalendar);
			}

			// Token: 0x06000A7A RID: 2682 RVA: 0x00028F14 File Offset: 0x00027F14
			private void MonthCalResize(object sender, EventArgs e)
			{
				base.Size = this.monthCalendar.Size;
			}

			// Token: 0x06000A7B RID: 2683 RVA: 0x00028F27 File Offset: 0x00027F27
			private void OnDateSelected(object sender, DateRangeEventArgs e)
			{
				this.value = this.monthCalendar.SelectionStart;
				this.edSvc.CloseDropDown();
			}

			// Token: 0x06000A7C RID: 2684 RVA: 0x00028F4A File Offset: 0x00027F4A
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.monthCalendar.Focus();
			}

			// Token: 0x06000A7D RID: 2685 RVA: 0x00028F60 File Offset: 0x00027F60
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

			// Token: 0x04000D8D RID: 3469
			private MonthCalendar monthCalendar = new DateTimeEditor.DateTimeUI.DateTimeMonthCalendar();

			// Token: 0x04000D8E RID: 3470
			private object value;

			// Token: 0x04000D8F RID: 3471
			private IWindowsFormsEditorService edSvc;

			// Token: 0x020000FD RID: 253
			private class DateTimeMonthCalendar : MonthCalendar
			{
				// Token: 0x06000A7E RID: 2686 RVA: 0x00028FA8 File Offset: 0x00027FA8
				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Return || base.IsInputKey(keyData);
				}
			}
		}
	}
}

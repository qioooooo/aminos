using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Design
{
	internal class BindingFormattingWindowsFormsEditorService : Panel, IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		public BindingFormattingWindowsFormsEditorService()
		{
			this.BackColor = SystemColors.Window;
			this.Text = SR.GetString("DataGridNoneString");
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.Selectable, true);
			base.SetStyle(ControlStyles.UseTextForAccessibility, true);
			base.AccessibleRole = AccessibleRole.DropList;
			base.TabStop = true;
			this.button = new BindingFormattingWindowsFormsEditorService.DropDownButton(this);
			this.button.FlatStyle = FlatStyle.Popup;
			this.button.Image = this.CreateDownArrow();
			this.button.Padding = new Padding(0);
			this.button.BackColor = SystemColors.Control;
			this.button.ForeColor = SystemColors.ControlText;
			this.button.Click += this.button_Click;
			this.button.Size = new Size(SystemInformation.VerticalScrollBarArrowHeight, this.Font.Height + 2);
			this.button.AccessibleName = SR.GetString("BindingFormattingDialogDataSourcePickerDropDownAccName");
			base.Controls.Add(this.button);
		}

		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new BindingFormattingWindowsFormsEditorService.BindingFormattingWindowFormsEditorAccessibleObject(this);
		}

		private Bitmap CreateDownArrow()
		{
			Bitmap bitmap = null;
			try
			{
				Icon icon = new Icon(typeof(BindingFormattingDialog), "BindingFormattingDialog.Arrow.ico");
				bitmap = icon.ToBitmap();
				icon.Dispose();
			}
			catch
			{
				bitmap = new Bitmap(16, 16);
			}
			return bitmap;
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, this.PreferredHeight, specified);
			int num = base.Height - 2;
			int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
			int num2 = base.Width - horizontalScrollBarThumbWidth - 2;
			int num3 = 1;
			if (this.RightToLeft == RightToLeft.No)
			{
				this.button.Bounds = new Rectangle(num3, num2, horizontalScrollBarThumbWidth, num);
				return;
			}
			this.button.Bounds = new Rectangle(num3, 2, horizontalScrollBarThumbWidth, num);
		}

		private int PreferredHeight
		{
			get
			{
				return TextRenderer.MeasureText("j^", this.Font, new Size(32767, (int)((double)base.FontHeight * 1.25))).Height + SystemInformation.BorderSize.Height * 8 + base.Padding.Size.Height;
			}
		}

		public ITypeDescriptorContext Context
		{
			set
			{
				this.context = value;
			}
		}

		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				if (this.ownerComponent == null)
				{
					return null;
				}
				ISite site = this.ownerComponent.Site;
				if (site == null)
				{
					return null;
				}
				return site.Container;
			}
		}

		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this.ownerComponent;
			}
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		void ITypeDescriptorContext.OnComponentChanged()
		{
			if (this.context != null)
			{
				this.context.OnComponentChanged();
			}
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return this.context == null || this.context.OnComponentChanging();
		}

		object IServiceProvider.GetService(Type type)
		{
			if (type == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			if (this.context != null)
			{
				return this.context.GetService(type);
			}
			return null;
		}

		void IWindowsFormsEditorService.CloseDropDown()
		{
			this.dropDownHolder.SetComponent(null);
			this.dropDownHolder.Visible = false;
			this.button.Focus();
		}

		void IWindowsFormsEditorService.DropDownControl(Control ctl)
		{
			if (this.dropDownHolder == null)
			{
				this.dropDownHolder = new DropDownHolder(this);
			}
			this.dropDownHolder.SetComponent(ctl);
			this.dropDownHolder.Location = base.PointToScreen(new Point(0, base.Height));
			try
			{
				this.dropDownHolder.Visible = true;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), -8, new HandleRef(this, base.Handle));
				this.dropDownHolder.FocusComponent();
				this.dropDownHolder.DoModalLoop();
			}
			finally
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this.dropDownHolder, this.dropDownHolder.Handle), -8, new HandleRef(null, IntPtr.Zero));
			}
		}

		DialogResult IWindowsFormsEditorService.ShowDialog(Form form)
		{
			return form.ShowDialog();
		}

		public Binding Binding
		{
			get
			{
				return this.binding;
			}
			set
			{
				if (this.binding == value)
				{
					return;
				}
				this.binding = value;
				if (this.binding != null)
				{
					this.Text = BindingFormattingWindowsFormsEditorService.ConstructDisplayTextFromBinding(this.binding);
				}
				else
				{
					this.Text = SR.GetString("DataGridNoneString");
				}
				base.Invalidate();
			}
		}

		public DataSourceUpdateMode DefaultDataSourceUpdateMode
		{
			set
			{
				this.defaultDataSourceUpdateMode = value;
			}
		}

		public IComponent OwnerComponent
		{
			set
			{
				this.ownerComponent = value;
			}
		}

		public string PropertyName
		{
			set
			{
				this.propertyName = value;
			}
		}

		public event EventHandler PropertyValueChanged
		{
			add
			{
				this.propertyValueChanged = (EventHandler)Delegate.Combine(this.propertyValueChanged, value);
			}
			remove
			{
				this.propertyValueChanged = (EventHandler)Delegate.Remove(this.propertyValueChanged, value);
			}
		}

		private void button_Click(object sender, EventArgs e)
		{
			this.DropDownPicker();
		}

		private static string ConstructDisplayTextFromBinding(Binding binding)
		{
			string text;
			if (binding.DataSource == null)
			{
				text = SR.GetString("DataGridNoneString");
			}
			else if (binding.DataSource is IComponent)
			{
				IComponent component = binding.DataSource as IComponent;
				if (component.Site != null)
				{
					text = component.Site.Name;
				}
				else
				{
					text = "";
				}
			}
			else if (binding.DataSource is IListSource || binding.DataSource is IList || binding.DataSource is Array)
			{
				text = SR.GetString("BindingFormattingDialogList");
			}
			else
			{
				string text2 = TypeDescriptor.GetClassName(binding.DataSource);
				int num = text2.LastIndexOf(".");
				if (num != -1)
				{
					text2 = text2.Substring(num + 1);
				}
				text = string.Format(CultureInfo.CurrentCulture, "({0})", new object[] { text2 });
			}
			return text + " - " + binding.BindingMemberInfo.BindingMember;
		}

		private void DropDownPicker()
		{
			if (this.designBindingPicker == null)
			{
				this.designBindingPicker = new DesignBindingPicker();
				this.designBindingPicker.Width = base.Width;
			}
			DesignBinding designBinding = null;
			if (this.binding != null)
			{
				designBinding = new DesignBinding(this.binding.DataSource, this.binding.BindingMemberInfo.BindingMember);
			}
			DesignBinding designBinding2 = this.designBindingPicker.Pick(this, this, true, true, false, null, string.Empty, designBinding);
			if (designBinding2 == null)
			{
				return;
			}
			Binding binding = this.binding;
			Binding binding2 = null;
			string text = ((binding != null) ? binding.FormatString : string.Empty);
			IFormatProvider formatProvider = ((binding != null) ? binding.FormatInfo : null);
			object obj = ((binding != null) ? binding.NullValue : null);
			DataSourceUpdateMode dataSourceUpdateMode = ((binding != null) ? binding.DataSourceUpdateMode : this.defaultDataSourceUpdateMode);
			if (designBinding2.DataSource != null && !string.IsNullOrEmpty(designBinding2.DataMember))
			{
				binding2 = new Binding(this.propertyName, designBinding2.DataSource, designBinding2.DataMember, true, dataSourceUpdateMode, obj, text, formatProvider);
			}
			this.Binding = binding2;
			bool flag = binding2 == null || binding != null || (binding2 != null && binding == null) || (binding2 != null && binding != null && (binding2.DataSource != binding.DataSource || !binding2.BindingMemberInfo.Equals(binding.BindingMemberInfo)));
			if (flag)
			{
				this.OnPropertyValueChanged(EventArgs.Empty);
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			base.Select();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);
			string text = this.Text;
			if (ComboBoxRenderer.IsSupported)
			{
				Rectangle rectangle = new Rectangle(base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width, base.ClientRectangle.Height);
				SolidBrush solidBrush;
				SolidBrush solidBrush2;
				ComboBoxState comboBoxState;
				if (!base.Enabled)
				{
					solidBrush = (SolidBrush)SystemBrushes.ControlDark;
					solidBrush2 = (SolidBrush)SystemBrushes.Control;
					comboBoxState = ComboBoxState.Disabled;
				}
				else if (base.ContainsFocus)
				{
					solidBrush = (SolidBrush)SystemBrushes.HighlightText;
					solidBrush2 = (SolidBrush)SystemBrushes.Highlight;
					comboBoxState = ComboBoxState.Hot;
				}
				else
				{
					solidBrush = (SolidBrush)SystemBrushes.WindowText;
					solidBrush2 = (SolidBrush)SystemBrushes.Window;
					comboBoxState = ComboBoxState.Normal;
				}
				ComboBoxRenderer.DrawTextBox(p.Graphics, rectangle, string.Empty, this.Font, comboBoxState);
				Graphics graphics = p.Graphics;
				rectangle.Inflate(-2, -2);
				ControlPaint.DrawBorder(graphics, rectangle, solidBrush2.Color, ButtonBorderStyle.None);
				rectangle.Inflate(-1, -1);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle.X += this.button.Width;
				}
				rectangle.Width -= this.button.Width;
				graphics.FillRectangle(solidBrush2, rectangle);
				TextFormatFlags textFormatFlags = TextFormatFlags.VerticalCenter;
				if (this.RightToLeft == RightToLeft.No)
				{
					textFormatFlags = textFormatFlags;
				}
				else
				{
					textFormatFlags |= TextFormatFlags.Right;
				}
				if (base.ContainsFocus)
				{
					ControlPaint.DrawFocusRectangle(graphics, rectangle, Color.Empty, solidBrush2.Color);
				}
				TextRenderer.DrawText(graphics, text, this.Font, rectangle, solidBrush.Color, textFormatFlags);
				return;
			}
			if (!string.IsNullOrEmpty(text))
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Near;
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle rectangle2 = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle2.X += this.button.Width;
				}
				rectangle2.Width -= this.button.Width;
				TextFormatFlags textFormatFlags2 = TextFormatFlags.VerticalCenter;
				if (this.RightToLeft == RightToLeft.No)
				{
					textFormatFlags2 = textFormatFlags2;
				}
				else
				{
					textFormatFlags2 |= TextFormatFlags.Right;
				}
				TextRenderer.DrawText(p.Graphics, text, this.Font, rectangle2, this.ForeColor, textFormatFlags2);
				stringFormat.Dispose();
			}
		}

		protected void OnPropertyValueChanged(EventArgs e)
		{
			if (this.propertyValueChanged != null)
			{
				this.propertyValueChanged(this, e);
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys modifierKeys = Control.ModifierKeys;
			if ((modifierKeys & Keys.Alt) == Keys.Alt && (keyData & Keys.KeyCode) == Keys.Down)
			{
				this.DropDownPicker();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		private ITypeDescriptorContext context;

		private DropDownHolder dropDownHolder;

		private BindingFormattingWindowsFormsEditorService.DropDownButton button;

		private EventHandler propertyValueChanged;

		private Binding binding;

		private IComponent ownerComponent;

		private DataSourceUpdateMode defaultDataSourceUpdateMode;

		private DesignBindingPicker designBindingPicker;

		private string propertyName = string.Empty;

		private class BindingFormattingWindowFormsEditorAccessibleObject : Control.ControlAccessibleObject
		{
			public BindingFormattingWindowFormsEditorAccessibleObject(BindingFormattingWindowsFormsEditorService owner)
				: base(owner)
			{
				this.owner = owner;
			}

			public override string Name
			{
				get
				{
					return SR.GetString("BindingFormattingDialogBindingPickerAccName");
				}
			}

			public override string Value
			{
				get
				{
					return this.owner.Text;
				}
			}

			public override void DoDefaultAction()
			{
				this.owner.DropDownPicker();
			}

			private BindingFormattingWindowsFormsEditorService owner;
		}

		private class DropDownButton : Button
		{
			public DropDownButton(BindingFormattingWindowsFormsEditorService owner)
			{
				this.owner = owner;
				base.TabStop = false;
			}

			protected override Size DefaultSize
			{
				get
				{
					return new Size(17, 19);
				}
			}

			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				height = Math.Min(height, this.owner.Height - 2);
				width = SystemInformation.HorizontalScrollBarThumbWidth;
				y = 1;
				if (base.Parent != null)
				{
					if (base.Parent.RightToLeft == RightToLeft.No)
					{
						x = base.Parent.Width - width - 1;
					}
					else
					{
						x = 1;
					}
				}
				base.SetBoundsCore(x, y, width, height, specified);
			}

			protected override void OnEnabledChanged(EventArgs e)
			{
				base.OnEnabledChanged(e);
				if (!base.Enabled)
				{
					this.mouseIsDown = false;
					this.mouseIsOver = false;
				}
			}

			protected override void OnKeyDown(KeyEventArgs kevent)
			{
				base.OnKeyDown(kevent);
				if (kevent.KeyData == Keys.Space)
				{
					this.mouseIsDown = true;
					base.Invalidate();
				}
			}

			protected override void OnKeyUp(KeyEventArgs kevent)
			{
				base.OnKeyUp(kevent);
				if (this.mouseIsDown)
				{
					this.mouseIsDown = false;
					base.Invalidate();
				}
			}

			protected override void OnLostFocus(EventArgs e)
			{
				base.OnLostFocus(e);
				this.mouseIsDown = false;
				base.Invalidate();
			}

			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				if (!this.mouseIsOver)
				{
					this.mouseIsOver = true;
					base.Invalidate();
				}
			}

			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				if (this.mouseIsOver || this.mouseIsDown)
				{
					this.mouseIsOver = false;
					this.mouseIsDown = false;
					base.Invalidate();
				}
			}

			protected override void OnMouseDown(MouseEventArgs mevent)
			{
				base.OnMouseDown(mevent);
				if (mevent.Button == MouseButtons.Left)
				{
					this.mouseIsDown = true;
					base.Invalidate();
				}
			}

			protected override void OnMouseMove(MouseEventArgs mevent)
			{
				base.OnMouseMove(mevent);
				if (mevent.Button != MouseButtons.None)
				{
					if (!base.ClientRectangle.Contains(mevent.X, mevent.Y))
					{
						if (this.mouseIsDown)
						{
							this.mouseIsDown = false;
							base.Invalidate();
							return;
						}
					}
					else if (!this.mouseIsDown)
					{
						this.mouseIsDown = true;
						base.Invalidate();
					}
				}
			}

			protected override void OnMouseUp(MouseEventArgs mevent)
			{
				base.OnMouseUp(mevent);
				if (this.mouseIsDown)
				{
					this.mouseIsDown = false;
					base.Invalidate();
				}
			}

			protected override void OnPaint(PaintEventArgs pevent)
			{
				base.OnPaint(pevent);
				if (VisualStyleRenderer.IsSupported)
				{
					ComboBoxState comboBoxState = ComboBoxState.Normal;
					if (!base.Enabled)
					{
						comboBoxState = ComboBoxState.Disabled;
					}
					if (this.mouseIsDown && this.mouseIsOver)
					{
						comboBoxState = ComboBoxState.Pressed;
					}
					else if (this.mouseIsOver)
					{
						comboBoxState = ComboBoxState.Hot;
					}
					ComboBoxRenderer.DrawDropDownButton(pevent.Graphics, pevent.ClipRectangle, comboBoxState);
				}
			}

			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 8 || msg == 31 || msg == 533)
				{
					this.mouseIsDown = false;
					base.Invalidate();
					base.WndProc(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			private const int WM_KILLFOCUS = 8;

			private const int WM_CANCELMODE = 31;

			private const int WM_CAPTURECHANGED = 533;

			private bool mouseIsDown;

			private bool mouseIsOver;

			private BindingFormattingWindowsFormsEditorService owner;
		}
	}
}

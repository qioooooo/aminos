using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	public sealed class AnchorEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.anchorUI == null)
					{
						this.anchorUI = new AnchorEditor.AnchorUI(this);
					}
					this.anchorUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.anchorUI);
					value = this.anchorUI.Value;
					this.anchorUI.End();
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private AnchorEditor.AnchorUI anchorUI;

		private class AnchorUI : Control
		{
			public AnchorUI(AnchorEditor editor)
			{
				this.editor = editor;
				this.left = new AnchorEditor.AnchorUI.SpringControl(this);
				this.right = new AnchorEditor.AnchorUI.SpringControl(this);
				this.top = new AnchorEditor.AnchorUI.SpringControl(this);
				this.bottom = new AnchorEditor.AnchorUI.SpringControl(this);
				this.tabOrder = new AnchorEditor.AnchorUI.SpringControl[] { this.left, this.top, this.right, this.bottom };
				this.InitializeComponent();
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

			public virtual AnchorStyles GetSelectedAnchor()
			{
				AnchorStyles anchorStyles = AnchorStyles.None;
				if (this.left.GetSolid())
				{
					anchorStyles |= AnchorStyles.Left;
				}
				if (this.top.GetSolid())
				{
					anchorStyles |= AnchorStyles.Top;
				}
				if (this.bottom.GetSolid())
				{
					anchorStyles |= AnchorStyles.Bottom;
				}
				if (this.right.GetSolid())
				{
					anchorStyles |= AnchorStyles.Right;
				}
				return anchorStyles;
			}

			internal virtual void InitializeComponent()
			{
				int width = SystemInformation.Border3DSize.Width;
				int height = SystemInformation.Border3DSize.Height;
				base.SetBounds(0, 0, 90, 90);
				base.AccessibleName = SR.GetString("AnchorEditorAccName");
				this.container.Location = new Point(0, 0);
				this.container.Size = new Size(90, 90);
				this.container.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this.control.Location = new Point(30, 30);
				this.control.Size = new Size(30, 30);
				this.control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this.right.Location = new Point(60, 40);
				this.right.Size = new Size(30 - width, 10);
				this.right.TabIndex = 2;
				this.right.TabStop = true;
				this.right.Anchor = AnchorStyles.Right;
				this.right.AccessibleName = SR.GetString("AnchorEditorRightAccName");
				this.left.Location = new Point(width, 40);
				this.left.Size = new Size(30 - width, 10);
				this.left.TabIndex = 0;
				this.left.TabStop = true;
				this.left.Anchor = AnchorStyles.Left;
				this.left.AccessibleName = SR.GetString("AnchorEditorLeftAccName");
				this.top.Location = new Point(40, height);
				this.top.Size = new Size(10, 30 - height);
				this.top.TabIndex = 1;
				this.top.TabStop = true;
				this.top.Anchor = AnchorStyles.Top;
				this.top.AccessibleName = SR.GetString("AnchorEditorTopAccName");
				this.bottom.Location = new Point(40, 60);
				this.bottom.Size = new Size(10, 30 - height);
				this.bottom.TabIndex = 3;
				this.bottom.TabStop = true;
				this.bottom.Anchor = AnchorStyles.Bottom;
				this.bottom.AccessibleName = SR.GetString("AnchorEditorBottomAccName");
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.container });
				this.container.Controls.Clear();
				this.container.Controls.AddRange(new Control[] { this.control, this.top, this.left, this.bottom, this.right });
			}

			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.top.Focus();
			}

			private void SetValue()
			{
				this.value = this.GetSelectedAnchor();
			}

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				if (value is AnchorStyles)
				{
					this.left.SetSolid(((AnchorStyles)value & AnchorStyles.Left) == AnchorStyles.Left);
					this.top.SetSolid(((AnchorStyles)value & AnchorStyles.Top) == AnchorStyles.Top);
					this.bottom.SetSolid(((AnchorStyles)value & AnchorStyles.Bottom) == AnchorStyles.Bottom);
					this.right.SetSolid(((AnchorStyles)value & AnchorStyles.Right) == AnchorStyles.Right);
					this.oldAnchor = (AnchorStyles)value;
					return;
				}
				this.oldAnchor = AnchorStyles.Top | AnchorStyles.Left;
			}

			private void Teardown(bool saveAnchor)
			{
				if (!saveAnchor)
				{
					this.value = this.oldAnchor;
				}
				this.edSvc.CloseDropDown();
			}

			private AnchorEditor.AnchorUI.ContainerPlaceholder container = new AnchorEditor.AnchorUI.ContainerPlaceholder();

			private AnchorEditor.AnchorUI.ControlPlaceholder control = new AnchorEditor.AnchorUI.ControlPlaceholder();

			private IWindowsFormsEditorService edSvc;

			private AnchorEditor.AnchorUI.SpringControl left;

			private AnchorEditor.AnchorUI.SpringControl right;

			private AnchorEditor.AnchorUI.SpringControl top;

			private AnchorEditor.AnchorUI.SpringControl bottom;

			private AnchorEditor.AnchorUI.SpringControl[] tabOrder;

			private AnchorEditor editor;

			private AnchorStyles oldAnchor;

			private object value;

			private class ContainerPlaceholder : Control
			{
				public ContainerPlaceholder()
				{
					this.BackColor = SystemColors.Window;
					this.ForeColor = SystemColors.WindowText;
					base.TabStop = false;
				}

				protected override void OnPaint(PaintEventArgs e)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					ControlPaint.DrawBorder3D(e.Graphics, clientRectangle, Border3DStyle.Sunken);
				}
			}

			private class ControlPlaceholder : Control
			{
				public ControlPlaceholder()
				{
					this.BackColor = SystemColors.Control;
					base.TabStop = false;
					base.SetStyle(ControlStyles.Selectable, false);
				}

				protected override void OnPaint(PaintEventArgs e)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					ControlPaint.DrawButton(e.Graphics, clientRectangle, ButtonState.Normal);
				}
			}

			private class SpringControl : Control
			{
				public SpringControl(AnchorEditor.AnchorUI picker)
				{
					if (picker == null)
					{
						throw new ArgumentException();
					}
					this.picker = picker;
					base.TabStop = true;
				}

				protected override AccessibleObject CreateAccessibilityInstance()
				{
					return new AnchorEditor.AnchorUI.SpringControl.SpringControlAccessibleObject(this);
				}

				public virtual bool GetSolid()
				{
					return this.solid;
				}

				protected override void OnGotFocus(EventArgs e)
				{
					if (!this.focused)
					{
						this.focused = true;
						base.Invalidate();
					}
					base.OnGotFocus(e);
				}

				protected override void OnLostFocus(EventArgs e)
				{
					if (this.focused)
					{
						this.focused = false;
						base.Invalidate();
					}
					base.OnLostFocus(e);
				}

				protected override void OnMouseDown(MouseEventArgs e)
				{
					this.SetSolid(!this.solid);
					base.Focus();
				}

				protected override void OnPaint(PaintEventArgs e)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					if (this.solid)
					{
						e.Graphics.FillRectangle(SystemBrushes.ControlDark, clientRectangle);
						e.Graphics.DrawRectangle(SystemPens.WindowFrame, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
					}
					else
					{
						ControlPaint.DrawFocusRectangle(e.Graphics, clientRectangle);
					}
					if (this.focused)
					{
						clientRectangle.Inflate(-2, -2);
						ControlPaint.DrawFocusRectangle(e.Graphics, clientRectangle);
					}
				}

				protected override bool ProcessDialogChar(char charCode)
				{
					if (charCode == ' ')
					{
						this.SetSolid(!this.solid);
						return true;
					}
					return base.ProcessDialogChar(charCode);
				}

				protected override bool ProcessDialogKey(Keys keyData)
				{
					if ((keyData & Keys.KeyCode) == Keys.Return && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
					{
						this.picker.Teardown(true);
						return true;
					}
					if ((keyData & Keys.KeyCode) == Keys.Escape && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
					{
						this.picker.Teardown(false);
						return true;
					}
					if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & (Keys.Control | Keys.Alt)) == Keys.None)
					{
						for (int i = 0; i < this.picker.tabOrder.Length; i++)
						{
							if (this.picker.tabOrder[i] == this)
							{
								i += (((keyData & Keys.Shift) == Keys.None) ? 1 : (-1));
								i = ((i < 0) ? (i + this.picker.tabOrder.Length) : (i % this.picker.tabOrder.Length));
								this.picker.tabOrder[i].Focus();
								break;
							}
						}
						return true;
					}
					return base.ProcessDialogKey(keyData);
				}

				public virtual void SetSolid(bool value)
				{
					if (this.solid != value)
					{
						this.solid = value;
						this.picker.SetValue();
						base.Invalidate();
					}
				}

				internal bool solid;

				internal bool focused;

				private AnchorEditor.AnchorUI picker;

				private class SpringControlAccessibleObject : Control.ControlAccessibleObject
				{
					public SpringControlAccessibleObject(AnchorEditor.AnchorUI.SpringControl owner)
						: base(owner)
					{
					}

					public override AccessibleStates State
					{
						get
						{
							AccessibleStates accessibleStates = base.State;
							if (((AnchorEditor.AnchorUI.SpringControl)base.Owner).GetSolid())
							{
								accessibleStates |= AccessibleStates.Selected;
							}
							return accessibleStates;
						}
					}
				}
			}
		}
	}
}

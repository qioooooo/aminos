using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000176 RID: 374
	public sealed class AnchorEditor : UITypeEditor
	{
		// Token: 0x06000DAC RID: 3500 RVA: 0x00037D14 File Offset: 0x00036D14
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

		// Token: 0x06000DAD RID: 3501 RVA: 0x00037D83 File Offset: 0x00036D83
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x04000F27 RID: 3879
		private AnchorEditor.AnchorUI anchorUI;

		// Token: 0x02000177 RID: 375
		private class AnchorUI : Control
		{
			// Token: 0x06000DAF RID: 3503 RVA: 0x00037D90 File Offset: 0x00036D90
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

			// Token: 0x17000225 RID: 549
			// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00037E28 File Offset: 0x00036E28
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06000DB1 RID: 3505 RVA: 0x00037E30 File Offset: 0x00036E30
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x06000DB2 RID: 3506 RVA: 0x00037E40 File Offset: 0x00036E40
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

			// Token: 0x06000DB3 RID: 3507 RVA: 0x00037E94 File Offset: 0x00036E94
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

			// Token: 0x06000DB4 RID: 3508 RVA: 0x00038151 File Offset: 0x00037151
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.top.Focus();
			}

			// Token: 0x06000DB5 RID: 3509 RVA: 0x00038166 File Offset: 0x00037166
			private void SetValue()
			{
				this.value = this.GetSelectedAnchor();
			}

			// Token: 0x06000DB6 RID: 3510 RVA: 0x0003817C File Offset: 0x0003717C
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

			// Token: 0x06000DB7 RID: 3511 RVA: 0x0003820B File Offset: 0x0003720B
			private void Teardown(bool saveAnchor)
			{
				if (!saveAnchor)
				{
					this.value = this.oldAnchor;
				}
				this.edSvc.CloseDropDown();
			}

			// Token: 0x04000F28 RID: 3880
			private AnchorEditor.AnchorUI.ContainerPlaceholder container = new AnchorEditor.AnchorUI.ContainerPlaceholder();

			// Token: 0x04000F29 RID: 3881
			private AnchorEditor.AnchorUI.ControlPlaceholder control = new AnchorEditor.AnchorUI.ControlPlaceholder();

			// Token: 0x04000F2A RID: 3882
			private IWindowsFormsEditorService edSvc;

			// Token: 0x04000F2B RID: 3883
			private AnchorEditor.AnchorUI.SpringControl left;

			// Token: 0x04000F2C RID: 3884
			private AnchorEditor.AnchorUI.SpringControl right;

			// Token: 0x04000F2D RID: 3885
			private AnchorEditor.AnchorUI.SpringControl top;

			// Token: 0x04000F2E RID: 3886
			private AnchorEditor.AnchorUI.SpringControl bottom;

			// Token: 0x04000F2F RID: 3887
			private AnchorEditor.AnchorUI.SpringControl[] tabOrder;

			// Token: 0x04000F30 RID: 3888
			private AnchorEditor editor;

			// Token: 0x04000F31 RID: 3889
			private AnchorStyles oldAnchor;

			// Token: 0x04000F32 RID: 3890
			private object value;

			// Token: 0x02000178 RID: 376
			private class ContainerPlaceholder : Control
			{
				// Token: 0x06000DB8 RID: 3512 RVA: 0x0003822C File Offset: 0x0003722C
				public ContainerPlaceholder()
				{
					this.BackColor = SystemColors.Window;
					this.ForeColor = SystemColors.WindowText;
					base.TabStop = false;
				}

				// Token: 0x06000DB9 RID: 3513 RVA: 0x00038254 File Offset: 0x00037254
				protected override void OnPaint(PaintEventArgs e)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					ControlPaint.DrawBorder3D(e.Graphics, clientRectangle, Border3DStyle.Sunken);
				}
			}

			// Token: 0x02000179 RID: 377
			private class ControlPlaceholder : Control
			{
				// Token: 0x06000DBA RID: 3514 RVA: 0x00038276 File Offset: 0x00037276
				public ControlPlaceholder()
				{
					this.BackColor = SystemColors.Control;
					base.TabStop = false;
					base.SetStyle(ControlStyles.Selectable, false);
				}

				// Token: 0x06000DBB RID: 3515 RVA: 0x0003829C File Offset: 0x0003729C
				protected override void OnPaint(PaintEventArgs e)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					ControlPaint.DrawButton(e.Graphics, clientRectangle, ButtonState.Normal);
				}
			}

			// Token: 0x0200017A RID: 378
			private class SpringControl : Control
			{
				// Token: 0x06000DBC RID: 3516 RVA: 0x000382BD File Offset: 0x000372BD
				public SpringControl(AnchorEditor.AnchorUI picker)
				{
					if (picker == null)
					{
						throw new ArgumentException();
					}
					this.picker = picker;
					base.TabStop = true;
				}

				// Token: 0x06000DBD RID: 3517 RVA: 0x000382DC File Offset: 0x000372DC
				protected override AccessibleObject CreateAccessibilityInstance()
				{
					return new AnchorEditor.AnchorUI.SpringControl.SpringControlAccessibleObject(this);
				}

				// Token: 0x06000DBE RID: 3518 RVA: 0x000382E4 File Offset: 0x000372E4
				public virtual bool GetSolid()
				{
					return this.solid;
				}

				// Token: 0x06000DBF RID: 3519 RVA: 0x000382EC File Offset: 0x000372EC
				protected override void OnGotFocus(EventArgs e)
				{
					if (!this.focused)
					{
						this.focused = true;
						base.Invalidate();
					}
					base.OnGotFocus(e);
				}

				// Token: 0x06000DC0 RID: 3520 RVA: 0x0003830A File Offset: 0x0003730A
				protected override void OnLostFocus(EventArgs e)
				{
					if (this.focused)
					{
						this.focused = false;
						base.Invalidate();
					}
					base.OnLostFocus(e);
				}

				// Token: 0x06000DC1 RID: 3521 RVA: 0x00038328 File Offset: 0x00037328
				protected override void OnMouseDown(MouseEventArgs e)
				{
					this.SetSolid(!this.solid);
					base.Focus();
				}

				// Token: 0x06000DC2 RID: 3522 RVA: 0x00038340 File Offset: 0x00037340
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

				// Token: 0x06000DC3 RID: 3523 RVA: 0x000383CA File Offset: 0x000373CA
				protected override bool ProcessDialogChar(char charCode)
				{
					if (charCode == ' ')
					{
						this.SetSolid(!this.solid);
						return true;
					}
					return base.ProcessDialogChar(charCode);
				}

				// Token: 0x06000DC4 RID: 3524 RVA: 0x000383EC File Offset: 0x000373EC
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

				// Token: 0x06000DC5 RID: 3525 RVA: 0x000384CC File Offset: 0x000374CC
				public virtual void SetSolid(bool value)
				{
					if (this.solid != value)
					{
						this.solid = value;
						this.picker.SetValue();
						base.Invalidate();
					}
				}

				// Token: 0x04000F33 RID: 3891
				internal bool solid;

				// Token: 0x04000F34 RID: 3892
				internal bool focused;

				// Token: 0x04000F35 RID: 3893
				private AnchorEditor.AnchorUI picker;

				// Token: 0x0200017B RID: 379
				private class SpringControlAccessibleObject : Control.ControlAccessibleObject
				{
					// Token: 0x06000DC6 RID: 3526 RVA: 0x000384EF File Offset: 0x000374EF
					public SpringControlAccessibleObject(AnchorEditor.AnchorUI.SpringControl owner)
						: base(owner)
					{
					}

					// Token: 0x17000226 RID: 550
					// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x000384F8 File Offset: 0x000374F8
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

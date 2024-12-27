using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200025A RID: 602
	[Designer("System.Windows.Forms.Design.ButtonBaseDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionButton")]
	public class Button : ButtonBase, IButtonControl
	{
		// Token: 0x06001F9E RID: 8094 RVA: 0x00042AE1 File Offset: 0x00041AE1
		public Button()
		{
			base.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x00042B0A File Offset: 0x00041B0A
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x00042B14 File Offset: 0x00041B14
		[SRDescription("ControlAutoSizeModeDescr")]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[Localizable(true)]
		public AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					if (this.ParentInternal != null)
					{
						if (this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x00042B95 File Offset: 0x00041B95
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new ButtonFlatAdapter(this);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x00042B9D File Offset: 0x00041B9D
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new ButtonPopupAdapter(this);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x00042BA5 File Offset: 0x00041BA5
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new ButtonStandardAdapter(this);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x00042BB0 File Offset: 0x00041BB0
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (base.FlatStyle != FlatStyle.System)
			{
				Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
				if (this.AutoSizeMode != AutoSizeMode.GrowAndShrink)
				{
					return LayoutUtils.UnionSizes(preferredSizeCore, base.Size);
				}
				return preferredSizeCore;
			}
			else
			{
				if (this.systemSize.Width == -2147483648)
				{
					Size size = TextRenderer.MeasureText(this.Text, this.Font);
					size = this.SizeFromClientSize(size);
					size.Width += 14;
					size.Height += 9;
					this.systemSize = size;
				}
				Size size2 = this.systemSize + base.Padding.Size;
				if (this.AutoSizeMode != AutoSizeMode.GrowAndShrink)
				{
					return LayoutUtils.UnionSizes(size2, base.Size);
				}
				return size2;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x00042C68 File Offset: 0x00041C68
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.GetStyle(ControlStyles.UserPaint))
				{
					createParams.Style |= 11;
				}
				else
				{
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
					if (base.IsDefault)
					{
						createParams.Style |= 1;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x00042CC4 File Offset: 0x00041CC4
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x00042CCC File Offset: 0x00041CCC
		[SRDescription("ButtonDialogResultDescr")]
		[DefaultValue(DialogResult.None)]
		[SRCategory("CatBehavior")]
		public virtual DialogResult DialogResult
		{
			get
			{
				return this.dialogResult;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DialogResult));
				}
				this.dialogResult = value;
			}
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x00042CFB File Offset: 0x00041CFB
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x00042D04 File Offset: 0x00041D04
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06001FAA RID: 8106 RVA: 0x00042D0D File Offset: 0x00041D0D
		// (remove) Token: 0x06001FAB RID: 8107 RVA: 0x00042D16 File Offset: 0x00041D16
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06001FAC RID: 8108 RVA: 0x00042D1F File Offset: 0x00041D1F
		// (remove) Token: 0x06001FAD RID: 8109 RVA: 0x00042D28 File Offset: 0x00041D28
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00042D31 File Offset: 0x00041D31
		public virtual void NotifyDefault(bool value)
		{
			if (base.IsDefault != value)
			{
				base.IsDefault = value;
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00042D44 File Offset: 0x00041D44
		protected override void OnClick(EventArgs e)
		{
			Form form = base.FindFormInternal();
			if (form != null)
			{
				form.DialogResult = this.dialogResult;
			}
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			base.OnClick(e);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x00042D86 File Offset: 0x00041D86
		protected override void OnFontChanged(EventArgs e)
		{
			this.systemSize = new Size(int.MinValue, int.MinValue);
			base.OnFontChanged(e);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x00042DA4 File Offset: 0x00041DA4
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.MouseIsPressed)
			{
				bool mouseIsDown = base.MouseIsDown;
				if (base.GetStyle(ControlStyles.UserPaint))
				{
					base.ResetFlagsandPaint();
				}
				if (mouseIsDown)
				{
					Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
					if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle && !base.ValidationCancelled)
					{
						if (base.GetStyle(ControlStyles.UserPaint))
						{
							this.OnClick(mevent);
						}
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x00042E3D File Offset: 0x00041E3D
		protected override void OnTextChanged(EventArgs e)
		{
			this.systemSize = new Size(int.MinValue, int.MinValue);
			base.OnTextChanged(e);
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x00042E5C File Offset: 0x00041E5C
		public void PerformClick()
		{
			if (base.CanSelect)
			{
				bool flag2;
				bool flag = base.ValidateActiveControl(out flag2);
				if (!base.ValidationCancelled && (flag || flag2))
				{
					base.ResetFlagsandPaint();
					this.OnClick(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x00042E99 File Offset: 0x00041E99
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && this.CanProcessMnemonic() && Control.IsMnemonic(charCode, this.Text))
			{
				this.PerformClick();
				return true;
			}
			return base.ProcessMnemonic(charCode);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00042EC8 File Offset: 0x00041EC8
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", Text: " + this.Text;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x00042EF0 File Offset: 0x00041EF0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 20)
			{
				if (msg == 8465)
				{
					if (NativeMethods.Util.HIWORD(m.WParam) == 0 && !base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
				}
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x04001453 RID: 5203
		private DialogResult dialogResult;

		// Token: 0x04001454 RID: 5204
		private Size systemSize = new Size(int.MinValue, int.MinValue);
	}
}

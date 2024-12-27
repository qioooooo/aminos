using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000262 RID: 610
	[DefaultProperty("Checked")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultBindingProperty("CheckState")]
	[ComVisible(true)]
	[SRDescription("DescriptionCheckBox")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("CheckedChanged")]
	public class CheckBox : ButtonBase
	{
		// Token: 0x06001FCE RID: 8142 RVA: 0x00043295 File Offset: 0x00042295
		public CheckBox()
		{
			base.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.autoCheck = true;
			this.TextAlign = ContentAlignment.MiddleLeft;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x000432C7 File Offset: 0x000422C7
		// (set) Token: 0x06001FD0 RID: 8144 RVA: 0x000432CF File Offset: 0x000422CF
		private bool AccObjDoDefaultAction
		{
			get
			{
				return this.accObjDoDefaultAction;
			}
			set
			{
				this.accObjDoDefaultAction = value;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x000432D8 File Offset: 0x000422D8
		// (set) Token: 0x06001FD2 RID: 8146 RVA: 0x000432E0 File Offset: 0x000422E0
		[Localizable(true)]
		[DefaultValue(Appearance.Normal)]
		[SRDescription("CheckBoxAppearanceDescr")]
		[SRCategory("CatAppearance")]
		public Appearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Appearance));
				}
				if (this.appearance != value)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Appearance))
					{
						this.appearance = value;
						if (base.OwnerDraw)
						{
							this.Refresh();
						}
						else
						{
							base.UpdateStyles();
						}
						this.OnAppearanceChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06001FD3 RID: 8147 RVA: 0x00043378 File Offset: 0x00042378
		// (remove) Token: 0x06001FD4 RID: 8148 RVA: 0x0004338B File Offset: 0x0004238B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("CheckBoxOnAppearanceChangedDescr")]
		public event EventHandler AppearanceChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_APPEARANCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_APPEARANCECHANGED, value);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x0004339E File Offset: 0x0004239E
		// (set) Token: 0x06001FD6 RID: 8150 RVA: 0x000433A6 File Offset: 0x000423A6
		[SRDescription("CheckBoxAutoCheckDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AutoCheck
		{
			get
			{
				return this.autoCheck;
			}
			set
			{
				this.autoCheck = value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000433AF File Offset: 0x000423AF
		// (set) Token: 0x06001FD8 RID: 8152 RVA: 0x000433B8 File Offset: 0x000423B8
		[Localizable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[SRDescription("CheckBoxCheckAlignDescr")]
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		public ContentAlignment CheckAlign
		{
			get
			{
				return this.checkAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.checkAlign != value)
				{
					this.checkAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.CheckAlign);
					if (base.OwnerDraw)
					{
						base.Invalidate();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x0004341F File Offset: 0x0004241F
		// (set) Token: 0x06001FDA RID: 8154 RVA: 0x0004342D File Offset: 0x0004242D
		[SRDescription("CheckBoxCheckedDescr")]
		[SRCategory("CatAppearance")]
		[Bindable(true)]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.All)]
		[SettingsBindable(true)]
		public bool Checked
		{
			get
			{
				return this.checkState != CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
				}
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x00043445 File Offset: 0x00042445
		// (set) Token: 0x06001FDC RID: 8156 RVA: 0x00043450 File Offset: 0x00042450
		[SRDescription("CheckBoxCheckStateDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(CheckState.Unchecked)]
		public CheckState CheckState
		{
			get
			{
				return this.checkState;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (this.checkState != value)
				{
					bool @checked = this.Checked;
					this.checkState = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(241, (int)this.checkState, 0);
					}
					if (@checked != this.Checked)
					{
						this.OnCheckedChanged(EventArgs.Empty);
					}
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06001FDD RID: 8157 RVA: 0x000434D4 File Offset: 0x000424D4
		// (remove) Token: 0x06001FDE RID: 8158 RVA: 0x000434DD File Offset: 0x000424DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06001FDF RID: 8159 RVA: 0x000434E6 File Offset: 0x000424E6
		// (remove) Token: 0x06001FE0 RID: 8160 RVA: 0x000434EF File Offset: 0x000424EF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x000434F8 File Offset: 0x000424F8
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.OwnerDraw)
				{
					createParams.Style |= 11;
				}
				else
				{
					createParams.Style |= 5;
					if (this.Appearance == Appearance.Button)
					{
						createParams.Style |= 4096;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.CheckAlign);
					if ((contentAlignment & CheckBox.anyRight) != (ContentAlignment)0)
					{
						createParams.Style |= 32;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x0004357F File Offset: 0x0004257F
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, 24);
			}
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0004358C File Offset: 0x0004258C
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (this.Appearance == Appearance.Button)
			{
				ButtonStandardAdapter buttonStandardAdapter = new ButtonStandardAdapter(this);
				return buttonStandardAdapter.GetPreferredSizeCore(proposedConstraints);
			}
			if (base.FlatStyle != FlatStyle.System)
			{
				return base.GetPreferredSizeCore(proposedConstraints);
			}
			Size size = TextRenderer.MeasureText(this.Text, this.Font);
			Size size2 = this.SizeFromClientSize(size);
			size2.Width += 25;
			size2.Height += 5;
			return size2 + base.Padding.Size;
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x0004360F File Offset: 0x0004260F
		internal override Rectangle OverChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button)
				{
					return base.OverChangeRectangle;
				}
				if (base.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x00043649 File Offset: 0x00042649
		internal override Rectangle DownChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button || base.FlatStyle == FlatStyle.System)
				{
					return base.DownChangeRectangle;
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x00043679 File Offset: 0x00042679
		// (set) Token: 0x06001FE7 RID: 8167 RVA: 0x00043681 File Offset: 0x00042681
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[Localizable(true)]
		public override ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x0004368A File Offset: 0x0004268A
		// (set) Token: 0x06001FE9 RID: 8169 RVA: 0x00043692 File Offset: 0x00042692
		[SRDescription("CheckBoxThreeStateDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool ThreeState
		{
			get
			{
				return this.threeState;
			}
			set
			{
				this.threeState = value;
			}
		}

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06001FEA RID: 8170 RVA: 0x0004369B File Offset: 0x0004269B
		// (remove) Token: 0x06001FEB RID: 8171 RVA: 0x000436AE File Offset: 0x000426AE
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_CHECKEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_CHECKEDCHANGED, value);
			}
		}

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06001FEC RID: 8172 RVA: 0x000436C1 File Offset: 0x000426C1
		// (remove) Token: 0x06001FED RID: 8173 RVA: 0x000436D4 File Offset: 0x000426D4
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_CHECKSTATECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_CHECKSTATECHANGED, value);
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000436E7 File Offset: 0x000426E7
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new CheckBox.CheckBoxAccessibleObject(this);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000436F0 File Offset: 0x000426F0
		protected virtual void OnAppearanceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[CheckBox.EVENT_APPEARANCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00043720 File Offset: 0x00042720
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (base.FlatStyle == FlatStyle.System)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemCaptureStart, -1);
			}
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			if (base.FlatStyle == FlatStyle.System)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemCaptureEnd, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[CheckBox.EVENT_CHECKEDCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0004378C File Offset: 0x0004278C
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			if (base.OwnerDraw)
			{
				this.Refresh();
			}
			EventHandler eventHandler = (EventHandler)base.Events[CheckBox.EVENT_CHECKSTATECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000437C8 File Offset: 0x000427C8
		protected override void OnClick(EventArgs e)
		{
			if (this.autoCheck)
			{
				switch (this.CheckState)
				{
				case CheckState.Unchecked:
					this.CheckState = CheckState.Checked;
					break;
				case CheckState.Checked:
					if (this.threeState)
					{
						this.CheckState = CheckState.Indeterminate;
						if (this.AccObjDoDefaultAction)
						{
							base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
						}
					}
					else
					{
						this.CheckState = CheckState.Unchecked;
					}
					break;
				default:
					this.CheckState = CheckState.Unchecked;
					break;
				}
			}
			base.OnClick(e);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00043839 File Offset: 0x00042839
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (base.IsHandleCreated)
			{
				base.SendMessage(241, (int)this.checkState, 0);
			}
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0004385D File Offset: 0x0004285D
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00043868 File Offset: 0x00042868
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.MouseIsPressed && base.MouseIsDown)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						if (base.Capture)
						{
							this.OnClick(mevent);
						}
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000438F5 File Offset: 0x000428F5
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new CheckBoxFlatAdapter(this);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000438FD File Offset: 0x000428FD
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new CheckBoxPopupAdapter(this);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00043905 File Offset: 0x00042905
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new CheckBoxStandardAdapter(this);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00043910 File Offset: 0x00042910
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && Control.IsMnemonic(charCode, this.Text) && base.CanSelect)
			{
				if (this.FocusInternal())
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00043960 File Offset: 0x00042960
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", CheckState: " + ((int)this.CheckState).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x04001472 RID: 5234
		private static readonly object EVENT_CHECKEDCHANGED = new object();

		// Token: 0x04001473 RID: 5235
		private static readonly object EVENT_CHECKSTATECHANGED = new object();

		// Token: 0x04001474 RID: 5236
		private static readonly object EVENT_APPEARANCECHANGED = new object();

		// Token: 0x04001475 RID: 5237
		private static readonly ContentAlignment anyRight = (ContentAlignment)1092;

		// Token: 0x04001476 RID: 5238
		private bool autoCheck;

		// Token: 0x04001477 RID: 5239
		private bool threeState;

		// Token: 0x04001478 RID: 5240
		private bool accObjDoDefaultAction;

		// Token: 0x04001479 RID: 5241
		private ContentAlignment checkAlign = ContentAlignment.MiddleLeft;

		// Token: 0x0400147A RID: 5242
		private CheckState checkState;

		// Token: 0x0400147B RID: 5243
		private Appearance appearance;

		// Token: 0x02000263 RID: 611
		[ComVisible(true)]
		public class CheckBoxAccessibleObject : ButtonBase.ButtonBaseAccessibleObject
		{
			// Token: 0x06001FFC RID: 8188 RVA: 0x000439BC File Offset: 0x000429BC
			public CheckBoxAccessibleObject(Control owner)
				: base(owner)
			{
			}

			// Token: 0x17000496 RID: 1174
			// (get) Token: 0x06001FFD RID: 8189 RVA: 0x000439C8 File Offset: 0x000429C8
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = base.Owner.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					if (((CheckBox)base.Owner).Checked)
					{
						return SR.GetString("AccessibleActionUncheck");
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			// Token: 0x17000497 RID: 1175
			// (get) Token: 0x06001FFE RID: 8190 RVA: 0x00043A10 File Offset: 0x00042A10
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.CheckButton;
				}
			}

			// Token: 0x17000498 RID: 1176
			// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00043A34 File Offset: 0x00042A34
			public override AccessibleStates State
			{
				get
				{
					switch (((CheckBox)base.Owner).CheckState)
					{
					case CheckState.Checked:
						return AccessibleStates.Checked | base.State;
					case CheckState.Indeterminate:
						return AccessibleStates.Mixed | base.State;
					default:
						return base.State;
					}
				}
			}

			// Token: 0x06002000 RID: 8192 RVA: 0x00043A80 File Offset: 0x00042A80
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				CheckBox checkBox = base.Owner as CheckBox;
				if (checkBox != null)
				{
					checkBox.AccObjDoDefaultAction = true;
				}
				try
				{
					base.DoDefaultAction();
				}
				finally
				{
					if (checkBox != null)
					{
						checkBox.AccObjDoDefaultAction = false;
					}
				}
			}
		}
	}
}

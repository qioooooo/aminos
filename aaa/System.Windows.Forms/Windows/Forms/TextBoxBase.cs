using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020002E4 RID: 740
	[DefaultEvent("TextChanged")]
	[DefaultBindingProperty("Text")]
	[Designer("System.Windows.Forms.Design.TextBoxBaseDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public abstract class TextBoxBase : Control
	{
		// Token: 0x06002B92 RID: 11154 RVA: 0x00074FE8 File Offset: 0x00073FE8
		internal TextBoxBase()
		{
			base.SetState2(2048, true);
			this.textBoxFlags[TextBoxBase.autoSize | TextBoxBase.hideSelection | TextBoxBase.wordWrap | TextBoxBase.shortcutsEnabled] = true;
			base.SetStyle(ControlStyles.FixedHeight, this.textBoxFlags[TextBoxBase.autoSize]);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.StandardDoubleClick | ControlStyles.UseTextForAccessibility, false);
			this.requestedHeight = base.Height;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002B93 RID: 11155 RVA: 0x00075078 File Offset: 0x00074078
		// (set) Token: 0x06002B94 RID: 11156 RVA: 0x0007508A File Offset: 0x0007408A
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsTabDescr")]
		[SRCategory("CatBehavior")]
		public bool AcceptsTab
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.acceptsTab];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.acceptsTab] != value)
				{
					this.textBoxFlags[TextBoxBase.acceptsTab] = value;
					this.OnAcceptsTabChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x06002B95 RID: 11157 RVA: 0x000750BB File Offset: 0x000740BB
		// (remove) Token: 0x06002B96 RID: 11158 RVA: 0x000750CE File Offset: 0x000740CE
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnAcceptsTabChangedDescr")]
		public event EventHandler AcceptsTabChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_ACCEPTSTABCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_ACCEPTSTABCHANGED, value);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002B97 RID: 11159 RVA: 0x000750E1 File Offset: 0x000740E1
		// (set) Token: 0x06002B98 RID: 11160 RVA: 0x00075130 File Offset: 0x00074130
		[DefaultValue(true)]
		[SRDescription("TextBoxShortcutsEnabledDescr")]
		[SRCategory("CatBehavior")]
		public virtual bool ShortcutsEnabled
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.shortcutsEnabled];
			}
			set
			{
				if (TextBoxBase.shortcutsToDisable == null)
				{
					TextBoxBase.shortcutsToDisable = new int[]
					{
						131162, 131139, 131160, 131158, 131137, 131148, 131154, 131141, 131161, 131080,
						131118, 65582, 65581, 131146
					};
				}
				this.textBoxFlags[TextBoxBase.shortcutsEnabled] = value;
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00075164 File Offset: 0x00074164
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (!this.ShortcutsEnabled)
			{
				foreach (int num in TextBoxBase.shortcutsToDisable)
				{
					if (keyData == (Keys)num || keyData == (Keys)(num | 65536))
					{
						return true;
					}
				}
			}
			return (this.textBoxFlags[TextBoxBase.readOnly] && (keyData == (Keys)131148 || keyData == (Keys)131154 || keyData == (Keys)131141 || keyData == (Keys)131146)) || base.ProcessCmdKey(ref msg, keyData);
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000751E8 File Offset: 0x000741E8
		// (set) Token: 0x06002B9B RID: 11163 RVA: 0x000751FC File Offset: 0x000741FC
		[SRDescription("TextBoxAutoSizeDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(true)]
		[Browsable(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public override bool AutoSize
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.autoSize];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.autoSize] != value)
				{
					this.textBoxFlags[TextBoxBase.autoSize] = value;
					if (!this.Multiline)
					{
						base.SetStyle(ControlStyles.FixedHeight, value);
						this.AdjustHeight(false);
					}
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002B9C RID: 11164 RVA: 0x00075250 File Offset: 0x00074250
		// (set) Token: 0x06002B9D RID: 11165 RVA: 0x00075274 File Offset: 0x00074274
		[SRCategory("CatAppearance")]
		[DispId(-501)]
		[SRDescription("ControlBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				if (this.ReadOnly)
				{
					return SystemColors.Control;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x0007527D File Offset: 0x0007427D
		// (set) Token: 0x06002B9F RID: 11167 RVA: 0x00075285 File Offset: 0x00074285
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x06002BA0 RID: 11168 RVA: 0x0007528E File Offset: 0x0007428E
		// (remove) Token: 0x06002BA1 RID: 11169 RVA: 0x00075297 File Offset: 0x00074297
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x06002BA2 RID: 11170 RVA: 0x000752A0 File Offset: 0x000742A0
		// (remove) Token: 0x06002BA3 RID: 11171 RVA: 0x000752A9 File Offset: 0x000742A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x000752B2 File Offset: 0x000742B2
		// (set) Token: 0x06002BA5 RID: 11173 RVA: 0x000752BA File Offset: 0x000742BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x06002BA6 RID: 11174 RVA: 0x000752C3 File Offset: 0x000742C3
		// (remove) Token: 0x06002BA7 RID: 11175 RVA: 0x000752CC File Offset: 0x000742CC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000752D5 File Offset: 0x000742D5
		// (set) Token: 0x06002BA9 RID: 11177 RVA: 0x000752E0 File Offset: 0x000742E0
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("TextBoxBorderDescr")]
		[SRCategory("CatAppearance")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
					base.RecreateHandle();
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle))
					{
						this.OnBorderStyleChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x06002BAA RID: 11178 RVA: 0x00075370 File Offset: 0x00074370
		// (remove) Token: 0x06002BAB RID: 11179 RVA: 0x00075383 File Offset: 0x00074383
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_BORDERSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_BORDERSTYLECHANGED, value);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x00075396 File Offset: 0x00074396
		internal virtual bool CanRaiseTextChangedEvent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002BAD RID: 11181 RVA: 0x0007539C File Offset: 0x0007439C
		protected override bool CanEnableIme
		{
			get
			{
				return !this.ReadOnly && !this.PasswordProtect && base.CanEnableIme;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000753C4 File Offset: 0x000743C4
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[SRDescription("TextBoxCanUndoDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanUndo
		{
			get
			{
				return base.IsHandleCreated && (int)base.SendMessage(198, 0, 0) != 0;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002BAF RID: 11183 RVA: 0x000753F8 File Offset: 0x000743F8
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "EDIT";
				createParams.Style |= 192;
				if (!this.textBoxFlags[TextBoxBase.hideSelection])
				{
					createParams.Style |= 256;
				}
				if (this.textBoxFlags[TextBoxBase.readOnly])
				{
					createParams.Style |= 2048;
				}
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				if (this.textBoxFlags[TextBoxBase.multiline])
				{
					createParams.Style |= 4;
					if (this.textBoxFlags[TextBoxBase.wordWrap])
					{
						createParams.Style &= -129;
					}
				}
				return createParams;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x00075519 File Offset: 0x00074519
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x00075521 File Offset: 0x00074521
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		// Token: 0x1400014A RID: 330
		// (add) Token: 0x06002BB2 RID: 11186 RVA: 0x0007552A File Offset: 0x0007452A
		// (remove) Token: 0x06002BB3 RID: 11187 RVA: 0x00075533 File Offset: 0x00074533
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		// Token: 0x1400014B RID: 331
		// (add) Token: 0x06002BB4 RID: 11188 RVA: 0x0007553C File Offset: 0x0007453C
		// (remove) Token: 0x06002BB5 RID: 11189 RVA: 0x00075545 File Offset: 0x00074545
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x0007554E File Offset: 0x0007454E
		protected override Cursor DefaultCursor
		{
			get
			{
				return Cursors.IBeam;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x00075555 File Offset: 0x00074555
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, this.PreferredHeight);
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x00075564 File Offset: 0x00074564
		// (set) Token: 0x06002BB9 RID: 11193 RVA: 0x0007557A File Offset: 0x0007457A
		[DispId(-513)]
		[SRDescription("ControlForeColorDescr")]
		[SRCategory("CatAppearance")]
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002BBA RID: 11194 RVA: 0x00075583 File Offset: 0x00074583
		// (set) Token: 0x06002BBB RID: 11195 RVA: 0x00075595 File Offset: 0x00074595
		[SRCategory("CatBehavior")]
		[SRDescription("TextBoxHideSelectionDescr")]
		[DefaultValue(true)]
		public bool HideSelection
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.hideSelection];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.hideSelection] != value)
				{
					this.textBoxFlags[TextBoxBase.hideSelection] = value;
					base.RecreateHandle();
					this.OnHideSelectionChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400014C RID: 332
		// (add) Token: 0x06002BBC RID: 11196 RVA: 0x000755CC File Offset: 0x000745CC
		// (remove) Token: 0x06002BBD RID: 11197 RVA: 0x000755DF File Offset: 0x000745DF
		[SRDescription("TextBoxBaseOnHideSelectionChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler HideSelectionChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_HIDESELECTIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_HIDESELECTIONCHANGED, value);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000755F4 File Offset: 0x000745F4
		// (set) Token: 0x06002BBF RID: 11199 RVA: 0x00075623 File Offset: 0x00074623
		protected override ImeMode ImeModeBase
		{
			get
			{
				if (base.DesignMode)
				{
					return base.ImeModeBase;
				}
				return this.CanEnableIme ? base.ImeModeBase : ImeMode.Disable;
			}
			set
			{
				base.ImeModeBase = value;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x0007562C File Offset: 0x0007462C
		// (set) Token: 0x06002BC1 RID: 11201 RVA: 0x00075710 File Offset: 0x00074710
		[Localizable(true)]
		[SRDescription("TextBoxLinesDescr")]
		[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[SRCategory("CatAppearance")]
		public string[] Lines
		{
			get
			{
				string text = this.Text;
				ArrayList arrayList = new ArrayList();
				int j;
				for (int i = 0; i < text.Length; i = j)
				{
					for (j = i; j < text.Length; j++)
					{
						char c = text[j];
						if (c == '\r' || c == '\n')
						{
							break;
						}
					}
					string text2 = text.Substring(i, j - i);
					arrayList.Add(text2);
					if (j < text.Length && text[j] == '\r')
					{
						j++;
					}
					if (j < text.Length && text[j] == '\n')
					{
						j++;
					}
				}
				if (text.Length > 0 && (text[text.Length - 1] == '\r' || text[text.Length - 1] == '\n'))
				{
					arrayList.Add("");
				}
				return (string[])arrayList.ToArray(typeof(string));
			}
			set
			{
				if (value != null && value.Length > 0)
				{
					StringBuilder stringBuilder = new StringBuilder(value[0]);
					for (int i = 1; i < value.Length; i++)
					{
						stringBuilder.Append("\r\n");
						stringBuilder.Append(value[i]);
					}
					this.Text = stringBuilder.ToString();
					return;
				}
				this.Text = "";
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x0007576B File Offset: 0x0007476B
		// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x00075774 File Offset: 0x00074774
		[DefaultValue(32767)]
		[SRDescription("TextBoxMaxLengthDescr")]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public virtual int MaxLength
		{
			get
			{
				return this.maxLength;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaxLength", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxLength",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.maxLength != value)
				{
					this.maxLength = value;
					this.UpdateMaxLength();
				}
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x000757E0 File Offset: 0x000747E0
		// (set) Token: 0x06002BC5 RID: 11205 RVA: 0x00075850 File Offset: 0x00074850
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("TextBoxModifiedDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Modified
		{
			get
			{
				if (base.IsHandleCreated)
				{
					bool flag = 0 != (int)base.SendMessage(184, 0, 0);
					if (this.textBoxFlags[TextBoxBase.modified] != flag)
					{
						this.textBoxFlags[TextBoxBase.modified] = flag;
						this.OnModifiedChanged(EventArgs.Empty);
					}
					return flag;
				}
				return this.textBoxFlags[TextBoxBase.modified];
			}
			set
			{
				if (this.Modified != value)
				{
					if (base.IsHandleCreated)
					{
						base.SendMessage(185, value ? 1 : 0, 0);
					}
					this.textBoxFlags[TextBoxBase.modified] = value;
					this.OnModifiedChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400014D RID: 333
		// (add) Token: 0x06002BC6 RID: 11206 RVA: 0x0007589E File Offset: 0x0007489E
		// (remove) Token: 0x06002BC7 RID: 11207 RVA: 0x000758B1 File Offset: 0x000748B1
		[SRDescription("TextBoxBaseOnModifiedChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ModifiedChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_MODIFIEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_MODIFIEDCHANGED, value);
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000758C4 File Offset: 0x000748C4
		// (set) Token: 0x06002BC9 RID: 11209 RVA: 0x000758D8 File Offset: 0x000748D8
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("TextBoxMultilineDescr")]
		public virtual bool Multiline
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.multiline];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.multiline] != value)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Multiline))
					{
						this.textBoxFlags[TextBoxBase.multiline] = value;
						if (value)
						{
							base.SetStyle(ControlStyles.FixedHeight, false);
						}
						else
						{
							base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						}
						base.RecreateHandle();
						this.AdjustHeight(false);
						this.OnMultilineChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1400014E RID: 334
		// (add) Token: 0x06002BCA RID: 11210 RVA: 0x00075974 File Offset: 0x00074974
		// (remove) Token: 0x06002BCB RID: 11211 RVA: 0x00075987 File Offset: 0x00074987
		[SRDescription("TextBoxBaseOnMultilineChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler MultilineChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_MULTILINECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_MULTILINECHANGED, value);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x0007599A File Offset: 0x0007499A
		// (set) Token: 0x06002BCD RID: 11213 RVA: 0x000759A2 File Offset: 0x000749A2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x1400014F RID: 335
		// (add) Token: 0x06002BCE RID: 11214 RVA: 0x000759AB File Offset: 0x000749AB
		// (remove) Token: 0x06002BCF RID: 11215 RVA: 0x000759B4 File Offset: 0x000749B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnPaddingChangedDescr")]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000759BD File Offset: 0x000749BD
		internal virtual bool PasswordProtect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x000759C0 File Offset: 0x000749C0
		[SRDescription("TextBoxPreferredHeightDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int PreferredHeight
		{
			get
			{
				int num = base.FontHeight;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				return num;
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000759F4 File Offset: 0x000749F4
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size size = this.SizeFromClientSize(Size.Empty) + this.Padding.Size;
			if (this.BorderStyle != BorderStyle.None)
			{
				size += new Size(0, 3);
			}
			if (this.BorderStyle == BorderStyle.FixedSingle)
			{
				size.Width += 2;
				size.Height += 2;
			}
			proposedConstraints -= size;
			TextFormatFlags textFormatFlags = TextFormatFlags.Default;
			if (!this.Multiline)
			{
				textFormatFlags = TextFormatFlags.SingleLine;
			}
			else if (this.WordWrap)
			{
				textFormatFlags = TextFormatFlags.WordBreak;
			}
			Size size2 = TextRenderer.MeasureText(this.Text, this.Font, proposedConstraints, textFormatFlags);
			size2.Height = Math.Max(size2.Height, base.FontHeight);
			return size2 + size;
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x00075AB8 File Offset: 0x00074AB8
		internal void GetSelectionStartAndLength(out int start, out int length)
		{
			int num = 0;
			if (!base.IsHandleCreated)
			{
				this.AdjustSelectionStartAndEnd(this.selectionStart, this.selectionLength, out start, out num, -1);
				length = num - start;
				return;
			}
			start = 0;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 176, ref start, ref num);
			start = Math.Max(0, start);
			num = Math.Max(0, num);
			if (this.SelectionUsesDbcsOffsetsInWin9x && Marshal.SystemDefaultCharSize == 1)
			{
				TextBoxBase.ToUnicodeOffsets(this.WindowText, ref start, ref num);
			}
			length = num - start;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00075B3F File Offset: 0x00074B3F
		// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x00075B54 File Offset: 0x00074B54
		[DefaultValue(false)]
		[SRDescription("TextBoxReadOnlyDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		public bool ReadOnly
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.readOnly];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.readOnly] != value)
				{
					this.textBoxFlags[TextBoxBase.readOnly] = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(207, value ? (-1) : 0, 0);
					}
					this.OnReadOnlyChanged(EventArgs.Empty);
					base.VerifyImeRestrictedModeChanged();
				}
			}
		}

		// Token: 0x14000150 RID: 336
		// (add) Token: 0x06002BD6 RID: 11222 RVA: 0x00075BB2 File Offset: 0x00074BB2
		// (remove) Token: 0x06002BD7 RID: 11223 RVA: 0x00075BC5 File Offset: 0x00074BC5
		[SRDescription("TextBoxBaseOnReadOnlyChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_READONLYCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_READONLYCHANGED, value);
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x00075BD8 File Offset: 0x00074BD8
		// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x00075BFC File Offset: 0x00074BFC
		[SRDescription("TextBoxSelectedTextDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		public virtual string SelectedText
		{
			get
			{
				int num;
				int num2;
				this.GetSelectionStartAndLength(out num, out num2);
				return this.Text.Substring(num, num2);
			}
			set
			{
				this.SetSelectedTextInternal(value, true);
			}
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00075C08 File Offset: 0x00074C08
		internal virtual void SetSelectedTextInternal(string text, bool clearUndo)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			if (text == null)
			{
				text = "";
			}
			base.SendMessage(197, 0, 0);
			if (clearUndo)
			{
				base.SendMessage(194, 0, text);
				base.SendMessage(185, 0, 0);
				this.ClearUndo();
			}
			else
			{
				base.SendMessage(194, -1, text);
			}
			base.SendMessage(197, this.maxLength, 0);
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00075C84 File Offset: 0x00074C84
		// (set) Token: 0x06002BDC RID: 11228 RVA: 0x00075C9C File Offset: 0x00074C9C
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionLengthDescr")]
		[Browsable(false)]
		public virtual int SelectionLength
		{
			get
			{
				int num;
				int num2;
				this.GetSelectionStartAndLength(out num, out num2);
				return num2;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionLength", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionLength",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int num;
				int num2;
				this.GetSelectionStartAndLength(out num, out num2);
				if (value != num2)
				{
					this.Select(num, value);
				}
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x00075CF8 File Offset: 0x00074CF8
		// (set) Token: 0x06002BDE RID: 11230 RVA: 0x00075D10 File Offset: 0x00074D10
		[Browsable(false)]
		[SRDescription("TextBoxSelectionStartDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public int SelectionStart
		{
			get
			{
				int num;
				int num2;
				this.GetSelectionStartAndLength(out num, out num2);
				return num;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionStart",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.Select(value, this.SelectionLength);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x00075D62 File Offset: 0x00074D62
		internal virtual bool SetSelectionInCreateHandle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x00075D65 File Offset: 0x00074D65
		// (set) Token: 0x06002BE1 RID: 11233 RVA: 0x00075D6D File Offset: 0x00074D6D
		[Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (value != base.Text)
				{
					base.Text = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(185, 0, 0);
					}
				}
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x00075D9A File Offset: 0x00074D9A
		[Browsable(false)]
		public virtual int TextLength
		{
			get
			{
				if (base.IsHandleCreated && Marshal.SystemDefaultCharSize == 2)
				{
					return SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
				}
				return this.Text.Length;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x00075DC9 File Offset: 0x00074DC9
		internal virtual bool SelectionUsesDbcsOffsetsInWin9x
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x00075DCC File Offset: 0x00074DCC
		// (set) Token: 0x06002BE5 RID: 11237 RVA: 0x00075DD4 File Offset: 0x00074DD4
		internal override string WindowText
		{
			get
			{
				return base.WindowText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.WindowText.Equals(value))
				{
					this.textBoxFlags[TextBoxBase.codeUpdateText] = true;
					try
					{
						base.WindowText = value;
					}
					finally
					{
						this.textBoxFlags[TextBoxBase.codeUpdateText] = false;
					}
				}
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00075E38 File Offset: 0x00074E38
		internal void ForceWindowText(string value)
		{
			if (value == null)
			{
				value = "";
			}
			this.textBoxFlags[TextBoxBase.codeUpdateText] = true;
			try
			{
				if (base.IsHandleCreated)
				{
					UnsafeNativeMethods.SetWindowText(new HandleRef(this, base.Handle), value);
				}
				else if (value.Length == 0)
				{
					this.Text = null;
				}
				else
				{
					this.Text = value;
				}
			}
			finally
			{
				this.textBoxFlags[TextBoxBase.codeUpdateText] = false;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x00075EBC File Offset: 0x00074EBC
		// (set) Token: 0x06002BE8 RID: 11240 RVA: 0x00075ED0 File Offset: 0x00074ED0
		[DefaultValue(true)]
		[SRDescription("TextBoxWordWrapDescr")]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public bool WordWrap
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.wordWrap];
			}
			set
			{
				using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.WordWrap))
				{
					if (this.textBoxFlags[TextBoxBase.wordWrap] != value)
					{
						this.textBoxFlags[TextBoxBase.wordWrap] = value;
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00075F3C File Offset: 0x00074F3C
		private void AdjustHeight(bool returnIfAnchored)
		{
			if (returnIfAnchored && (this.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
			{
				return;
			}
			int num = this.requestedHeight;
			try
			{
				if (this.textBoxFlags[TextBoxBase.autoSize] && !this.textBoxFlags[TextBoxBase.multiline])
				{
					base.Height = this.PreferredHeight;
				}
				else
				{
					int height = base.Height;
					if (this.textBoxFlags[TextBoxBase.multiline])
					{
						base.Height = Math.Max(num, this.PreferredHeight + 2);
					}
					this.integralHeightAdjust = true;
					try
					{
						base.Height = num;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
			finally
			{
				this.requestedHeight = num;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00075FFC File Offset: 0x00074FFC
		public void AppendText(string text)
		{
			if (text.Length > 0)
			{
				int num;
				int num2;
				this.GetSelectionStartAndLength(out num, out num2);
				try
				{
					int endPosition = this.GetEndPosition();
					this.SelectInternal(endPosition, endPosition, endPosition);
					this.SelectedText = text;
				}
				finally
				{
					if (base.Width == 0 || base.Height == 0)
					{
						this.Select(num, num2);
					}
				}
			}
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x00076060 File Offset: 0x00075060
		public void Clear()
		{
			this.Text = null;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00076069 File Offset: 0x00075069
		public void ClearUndo()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(205, 0, 0);
			}
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x00076081 File Offset: 0x00075081
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public void Copy()
		{
			base.SendMessage(769, 0, 0);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x00076094 File Offset: 0x00075094
		protected override void CreateHandle()
		{
			this.textBoxFlags[TextBoxBase.creatingHandle] = true;
			try
			{
				base.CreateHandle();
				if (this.SetSelectionInCreateHandle)
				{
					this.SetSelectionOnHandle();
				}
			}
			finally
			{
				this.textBoxFlags[TextBoxBase.creatingHandle] = false;
			}
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000760EC File Offset: 0x000750EC
		public void Cut()
		{
			base.SendMessage(768, 0, 0);
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000760FC File Offset: 0x000750FC
		internal virtual int GetEndPosition()
		{
			if (!base.IsHandleCreated)
			{
				return this.TextLength;
			}
			return this.TextLength + 1;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00076118 File Offset: 0x00075118
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) != Keys.Alt)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Tab)
				{
					return this.Multiline && this.textBoxFlags[TextBoxBase.acceptsTab] && (keyData & Keys.Control) == Keys.None;
				}
				if (keys != Keys.Escape)
				{
					switch (keys)
					{
					case Keys.Prior:
					case Keys.Next:
					case Keys.End:
					case Keys.Home:
						return true;
					}
				}
				else if (this.Multiline)
				{
					return false;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x0007619C File Offset: 0x0007519C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			CommonProperties.xClearPreferredSizeCache(this);
			this.AdjustHeight(true);
			this.UpdateMaxLength();
			if (this.textBoxFlags[TextBoxBase.modified])
			{
				base.SendMessage(185, 1, 0);
			}
			if (this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated])
			{
				this.ScrollToCaret();
				this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated] = false;
			}
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x0007620C File Offset: 0x0007520C
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.textBoxFlags[TextBoxBase.modified] = this.Modified;
			this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = true;
			this.GetSelectionStartAndLength(out this.selectionStart, out this.selectionLength);
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00076259 File Offset: 0x00075259
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public void Paste()
		{
			IntSecurity.ClipboardRead.Demand();
			base.SendMessage(770, 0, 0);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00076274 File Offset: 0x00075274
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys == Keys.Tab && this.AcceptsTab && (keyData & Keys.Control) != Keys.None)
			{
				keyData &= ~Keys.Control;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x14000151 RID: 337
		// (add) Token: 0x06002BF6 RID: 11254 RVA: 0x000762AF File Offset: 0x000752AF
		// (remove) Token: 0x06002BF7 RID: 11255 RVA: 0x000762B8 File Offset: 0x000752B8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000762C4 File Offset: 0x000752C4
		protected virtual void OnAcceptsTabChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_ACCEPTSTABCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000762F4 File Offset: 0x000752F4
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_BORDERSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x00076322 File Offset: 0x00075322
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustHeight(false);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00076334 File Offset: 0x00075334
		protected virtual void OnHideSelectionChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_HIDESELECTIONCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00076364 File Offset: 0x00075364
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_MODIFIEDCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00076394 File Offset: 0x00075394
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			Point point = base.PointToScreen(mevent.Location);
			if (mevent.Button == MouseButtons.Left)
			{
				if (!base.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					if (!this.doubleClickFired)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
					else
					{
						this.doubleClickFired = false;
						this.OnDoubleClick(mevent);
						this.OnMouseDoubleClick(mevent);
					}
				}
				this.doubleClickFired = false;
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x00076420 File Offset: 0x00075420
		protected virtual void OnMultilineChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_MULTILINECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0007644E File Offset: 0x0007544E
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.AdjustHeight(false);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00076460 File Offset: 0x00075460
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_READONLYCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x0007648E File Offset: 0x0007548E
		protected override void OnTextChanged(EventArgs e)
		{
			CommonProperties.xClearPreferredSizeCache(this);
			base.OnTextChanged(e);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000764A0 File Offset: 0x000754A0
		public virtual char GetCharFromPosition(Point pt)
		{
			string text = this.Text;
			int charIndexFromPosition = this.GetCharIndexFromPosition(pt);
			if (charIndexFromPosition >= 0 && charIndexFromPosition < text.Length)
			{
				return text[charIndexFromPosition];
			}
			return '\0';
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000764D4 File Offset: 0x000754D4
		public virtual int GetCharIndexFromPosition(Point pt)
		{
			int num = NativeMethods.Util.MAKELONG(pt.X, pt.Y);
			int num2 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 215, 0, num);
			num2 = NativeMethods.Util.LOWORD(num2);
			if (num2 < 0)
			{
				num2 = 0;
			}
			else
			{
				string text = this.Text;
				if (num2 >= text.Length)
				{
					num2 = Math.Max(text.Length - 1, 0);
				}
			}
			return num2;
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x00076542 File Offset: 0x00075542
		public virtual int GetLineFromCharIndex(int index)
		{
			return (int)base.SendMessage(201, index, 0);
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x00076558 File Offset: 0x00075558
		public virtual Point GetPositionFromCharIndex(int index)
		{
			if (index < 0 || index >= this.Text.Length)
			{
				return Point.Empty;
			}
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 214, index, 0);
			return new Point(NativeMethods.Util.LOWORD(num), NativeMethods.Util.HIWORD(num));
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000765AC File Offset: 0x000755AC
		public int GetFirstCharIndexFromLine(int lineNumber)
		{
			if (lineNumber < 0)
			{
				throw new ArgumentOutOfRangeException("lineNumber", SR.GetString("InvalidArgument", new object[]
				{
					"lineNumber",
					lineNumber.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return (int)base.SendMessage(187, lineNumber, 0);
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00076603 File Offset: 0x00075603
		public int GetFirstCharIndexOfCurrentLine()
		{
			return (int)base.SendMessage(187, -1, 0);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x00076618 File Offset: 0x00075618
		public void ScrollToCaret()
		{
			if (base.IsHandleCreated)
			{
				if (string.IsNullOrEmpty(this.WindowText))
				{
					return;
				}
				bool flag = false;
				object obj = null;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					if (UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1084, 0, out obj) != 0)
					{
						intPtr = Marshal.GetIUnknownForObject(obj);
						if (intPtr != IntPtr.Zero)
						{
							IntPtr zero = IntPtr.Zero;
							Guid guid = typeof(UnsafeNativeMethods.ITextDocument).GUID;
							try
							{
								Marshal.QueryInterface(intPtr, ref guid, out zero);
								UnsafeNativeMethods.ITextDocument textDocument = Marshal.GetObjectForIUnknown(zero) as UnsafeNativeMethods.ITextDocument;
								if (textDocument != null)
								{
									int num;
									int num2;
									this.GetSelectionStartAndLength(out num, out num2);
									int lineFromCharIndex = this.GetLineFromCharIndex(num);
									UnsafeNativeMethods.ITextRange textRange = textDocument.Range(this.WindowText.Length - 1, this.WindowText.Length - 1);
									textRange.ScrollIntoView(0);
									int num3 = (int)base.SendMessage(206, 0, 0);
									if (num3 > lineFromCharIndex)
									{
										textRange = textDocument.Range(num, num + num2);
										textRange.ScrollIntoView(32);
									}
									flag = true;
								}
							}
							finally
							{
								if (zero != IntPtr.Zero)
								{
									Marshal.Release(zero);
								}
							}
						}
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.Release(intPtr);
					}
				}
				if (!flag)
				{
					base.SendMessage(183, 0, 0);
					return;
				}
			}
			else
			{
				this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated] = true;
			}
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00076794 File Offset: 0x00075794
		public void DeselectAll()
		{
			this.SelectionLength = 0;
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000767A0 File Offset: 0x000757A0
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidArgument", new object[]
				{
					"start",
					start.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int textLength = this.TextLength;
			if (start > textLength)
			{
				long num = Math.Min(0L, (long)length + (long)start - (long)textLength);
				if (num < -2147483648L)
				{
					length = int.MinValue;
				}
				else
				{
					length = (int)num;
				}
				start = textLength;
			}
			this.SelectInternal(start, length, textLength);
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00076824 File Offset: 0x00075824
		internal virtual void SelectInternal(int start, int length, int textLen)
		{
			if (base.IsHandleCreated)
			{
				int num;
				int num2;
				this.AdjustSelectionStartAndEnd(start, length, out num, out num2, textLen);
				base.SendMessage(177, num, num2);
				return;
			}
			this.selectionStart = start;
			this.selectionLength = length;
			this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = true;
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x00076874 File Offset: 0x00075874
		public void SelectAll()
		{
			int textLength = this.TextLength;
			this.SelectInternal(0, textLength, textLength);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00076894 File Offset: 0x00075894
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (!this.integralHeightAdjust && height != base.Height)
			{
				this.requestedHeight = height;
			}
			if (this.textBoxFlags[TextBoxBase.autoSize] && !this.textBoxFlags[TextBoxBase.multiline])
			{
				height = this.PreferredHeight;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000768F4 File Offset: 0x000758F4
		private static void Swap(ref int n1, ref int n2)
		{
			int num = n2;
			n2 = n1;
			n1 = num;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x0007690C File Offset: 0x0007590C
		internal void AdjustSelectionStartAndEnd(int selStart, int selLength, out int start, out int end, int textLen)
		{
			start = selStart;
			end = 0;
			if (start <= -1)
			{
				start = -1;
				return;
			}
			int num;
			if (textLen >= 0)
			{
				num = textLen;
			}
			else
			{
				num = this.TextLength;
			}
			if (start > num)
			{
				start = num;
			}
			try
			{
				end = checked(start + selLength);
			}
			catch (OverflowException)
			{
				end = ((start > 0) ? int.MaxValue : int.MinValue);
			}
			if (end < 0)
			{
				end = 0;
			}
			else if (end > num)
			{
				end = num;
			}
			if (this.SelectionUsesDbcsOffsetsInWin9x && Marshal.SystemDefaultCharSize == 1)
			{
				TextBoxBase.ToDbcsOffsets(this.WindowText, ref start, ref end);
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000769A8 File Offset: 0x000759A8
		internal void SetSelectionOnHandle()
		{
			if (this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated])
			{
				this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = false;
				int num;
				int num2;
				this.AdjustSelectionStartAndEnd(this.selectionStart, this.selectionLength, out num, out num2, -1);
				base.SendMessage(177, num, num2);
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x00076A00 File Offset: 0x00075A00
		private static void ToUnicodeOffsets(string str, ref int start, ref int end)
		{
			Encoding @default = Encoding.Default;
			byte[] bytes = @default.GetBytes(str);
			bool flag = start > end;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
			if (start < 0)
			{
				start = 0;
			}
			if (start > bytes.Length)
			{
				start = bytes.Length;
			}
			if (end > bytes.Length)
			{
				end = bytes.Length;
			}
			int num = ((start == 0) ? 0 : @default.GetCharCount(bytes, 0, start));
			end = num + @default.GetCharCount(bytes, start, end - start);
			start = num;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x00076A80 File Offset: 0x00075A80
		internal static void ToDbcsOffsets(string str, ref int start, ref int end)
		{
			Encoding @default = Encoding.Default;
			bool flag = start > end;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
			if (start < 0)
			{
				start = 0;
			}
			if (start > str.Length)
			{
				start = str.Length;
			}
			if (end < start)
			{
				end = start;
			}
			if (end > str.Length)
			{
				end = str.Length;
			}
			int num = ((start == 0) ? 0 : @default.GetByteCount(str.Substring(0, start)));
			end = num + @default.GetByteCount(str.Substring(start, end - start));
			start = num;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x00076B18 File Offset: 0x00075B18
		public override string ToString()
		{
			string text = base.ToString();
			string text2 = this.Text;
			if (text2.Length > 40)
			{
				text2 = text2.Substring(0, 40) + "...";
			}
			return text + ", Text: " + text2.ToString();
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x00076B62 File Offset: 0x00075B62
		public void Undo()
		{
			base.SendMessage(199, 0, 0);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x00076B72 File Offset: 0x00075B72
		internal virtual void UpdateMaxLength()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(197, this.maxLength, 0);
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x00076B8F File Offset: 0x00075B8F
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (msg == 312 && !this.ShouldSerializeBackColor())
			{
				return IntPtr.Zero;
			}
			return base.InitializeDCForWmCtlColor(dc, msg);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00076BB0 File Offset: 0x00075BB0
		private void WmReflectCommand(ref Message m)
		{
			if (!this.textBoxFlags[TextBoxBase.codeUpdateText] && !this.textBoxFlags[TextBoxBase.creatingHandle])
			{
				if (NativeMethods.Util.HIWORD(m.WParam) == 768 && this.CanRaiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
					return;
				}
				if (NativeMethods.Util.HIWORD(m.WParam) == 1024)
				{
					bool flag = this.Modified;
				}
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00076C20 File Offset: 0x00075C20
		private void WmSetFont(ref Message m)
		{
			base.WndProc(ref m);
			if (!this.textBoxFlags[TextBoxBase.multiline])
			{
				base.SendMessage(211, 3, 0);
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00076C4C File Offset: 0x00075C4C
		private void WmGetDlgCode(ref Message m)
		{
			base.WndProc(ref m);
			if (this.AcceptsTab)
			{
				m.Result = (IntPtr)((int)m.Result | 2);
				return;
			}
			m.Result = (IntPtr)((int)m.Result & -7);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x00076C9C File Offset: 0x00075C9C
		private void WmTextBoxContextMenu(ref Message m)
		{
			if (this.ContextMenu != null || this.ContextMenuStrip != null)
			{
				int num = NativeMethods.Util.SignedLOWORD(m.LParam);
				int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
				bool flag = false;
				Point point;
				if ((int)(long)m.LParam == -1)
				{
					flag = true;
					point = new Point(base.Width / 2, base.Height / 2);
				}
				else
				{
					point = base.PointToClientInternal(new Point(num, num2));
				}
				if (base.ClientRectangle.Contains(point))
				{
					if (this.ContextMenu != null)
					{
						this.ContextMenu.Show(this, point);
						return;
					}
					if (this.ContextMenuStrip != null)
					{
						this.ContextMenuStrip.ShowInternal(this, point, flag);
						return;
					}
					this.DefWndProc(ref m);
				}
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00076D54 File Offset: 0x00075D54
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 123)
			{
				if (msg == 48)
				{
					this.WmSetFont(ref m);
					return;
				}
				if (msg == 123)
				{
					if (this.ShortcutsEnabled)
					{
						base.WndProc(ref m);
						return;
					}
					this.WmTextBoxContextMenu(ref m);
					return;
				}
			}
			else
			{
				if (msg == 135)
				{
					this.WmGetDlgCode(ref m);
					return;
				}
				if (msg == 515)
				{
					this.doubleClickFired = true;
					base.WndProc(ref m);
					return;
				}
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x0400180A RID: 6154
		private static readonly int autoSize = BitVector32.CreateMask();

		// Token: 0x0400180B RID: 6155
		private static readonly int hideSelection = BitVector32.CreateMask(TextBoxBase.autoSize);

		// Token: 0x0400180C RID: 6156
		private static readonly int multiline = BitVector32.CreateMask(TextBoxBase.hideSelection);

		// Token: 0x0400180D RID: 6157
		private static readonly int modified = BitVector32.CreateMask(TextBoxBase.multiline);

		// Token: 0x0400180E RID: 6158
		private static readonly int readOnly = BitVector32.CreateMask(TextBoxBase.modified);

		// Token: 0x0400180F RID: 6159
		private static readonly int acceptsTab = BitVector32.CreateMask(TextBoxBase.readOnly);

		// Token: 0x04001810 RID: 6160
		private static readonly int wordWrap = BitVector32.CreateMask(TextBoxBase.acceptsTab);

		// Token: 0x04001811 RID: 6161
		private static readonly int creatingHandle = BitVector32.CreateMask(TextBoxBase.wordWrap);

		// Token: 0x04001812 RID: 6162
		private static readonly int codeUpdateText = BitVector32.CreateMask(TextBoxBase.creatingHandle);

		// Token: 0x04001813 RID: 6163
		private static readonly int shortcutsEnabled = BitVector32.CreateMask(TextBoxBase.codeUpdateText);

		// Token: 0x04001814 RID: 6164
		private static readonly int scrollToCaretOnHandleCreated = BitVector32.CreateMask(TextBoxBase.shortcutsEnabled);

		// Token: 0x04001815 RID: 6165
		private static readonly int setSelectionOnHandleCreated = BitVector32.CreateMask(TextBoxBase.scrollToCaretOnHandleCreated);

		// Token: 0x04001816 RID: 6166
		private static readonly object EVENT_ACCEPTSTABCHANGED = new object();

		// Token: 0x04001817 RID: 6167
		private static readonly object EVENT_BORDERSTYLECHANGED = new object();

		// Token: 0x04001818 RID: 6168
		private static readonly object EVENT_HIDESELECTIONCHANGED = new object();

		// Token: 0x04001819 RID: 6169
		private static readonly object EVENT_MODIFIEDCHANGED = new object();

		// Token: 0x0400181A RID: 6170
		private static readonly object EVENT_MULTILINECHANGED = new object();

		// Token: 0x0400181B RID: 6171
		private static readonly object EVENT_READONLYCHANGED = new object();

		// Token: 0x0400181C RID: 6172
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x0400181D RID: 6173
		private int maxLength = 32767;

		// Token: 0x0400181E RID: 6174
		private int requestedHeight;

		// Token: 0x0400181F RID: 6175
		private bool integralHeightAdjust;

		// Token: 0x04001820 RID: 6176
		private int selectionStart;

		// Token: 0x04001821 RID: 6177
		private int selectionLength;

		// Token: 0x04001822 RID: 6178
		private bool doubleClickFired;

		// Token: 0x04001823 RID: 6179
		private static int[] shortcutsToDisable;

		// Token: 0x04001824 RID: 6180
		private BitVector32 textBoxFlags = default(BitVector32);
	}
}

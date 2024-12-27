using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020002E5 RID: 741
	[Designer("System.Windows.Forms.Design.TextBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTextBox")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class TextBox : TextBoxBase
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x00076EE7 File Offset: 0x00075EE7
		// (set) Token: 0x06002C1F RID: 11295 RVA: 0x00076EEF File Offset: 0x00075EEF
		[SRDescription("TextBoxAcceptsReturnDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool AcceptsReturn
		{
			get
			{
				return this.acceptsReturn;
			}
			set
			{
				this.acceptsReturn = value;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002C20 RID: 11296 RVA: 0x00076EF8 File Offset: 0x00075EF8
		// (set) Token: 0x06002C21 RID: 11297 RVA: 0x00076F00 File Offset: 0x00075F00
		[SRDescription("TextBoxAutoCompleteModeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(AutoCompleteMode.None)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.autoCompleteMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteMode));
				}
				bool flag = false;
				if (this.autoCompleteMode != AutoCompleteMode.None && value == AutoCompleteMode.None)
				{
					flag = true;
				}
				this.autoCompleteMode = value;
				this.SetAutoComplete(flag);
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x00076F50 File Offset: 0x00075F50
		// (set) Token: 0x06002C23 RID: 11299 RVA: 0x00076F58 File Offset: 0x00075F58
		[Browsable(true)]
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("TextBoxAutoCompleteSourceDescr")]
		[TypeConverter(typeof(TextBoxAutoCompleteSourceConverter))]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.autoCompleteSource;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[] { 128, 7, 6, 64, 1, 32, 2, 256, 4 }))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteSource));
				}
				if (value == AutoCompleteSource.ListItems)
				{
					throw new NotSupportedException(SR.GetString("TextBoxAutoCompleteSourceNoItems"));
				}
				if (value != AutoCompleteSource.None && value != AutoCompleteSource.CustomSource)
				{
					new FileIOPermission(PermissionState.Unrestricted)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Demand();
				}
				this.autoCompleteSource = value;
				this.SetAutoComplete(false);
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x00077007 File Offset: 0x00076007
		// (set) Token: 0x06002C25 RID: 11301 RVA: 0x0007703C File Offset: 0x0007603C
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[SRDescription("TextBoxAutoCompleteCustomSourceDescr")]
		[Localizable(true)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				if (this.autoCompleteCustomSource == null)
				{
					this.autoCompleteCustomSource = new AutoCompleteStringCollection();
					this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
				}
				return this.autoCompleteCustomSource;
			}
			set
			{
				if (this.autoCompleteCustomSource != value)
				{
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
					}
					this.autoCompleteCustomSource = value;
					if (value != null)
					{
						this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
					}
					this.SetAutoComplete(false);
				}
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x00077099 File Offset: 0x00076099
		// (set) Token: 0x06002C27 RID: 11303 RVA: 0x000770A1 File Offset: 0x000760A1
		[SRDescription("TextBoxCharacterCasingDescr")]
		[DefaultValue(CharacterCasing.Normal)]
		[SRCategory("CatBehavior")]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return this.characterCasing;
			}
			set
			{
				if (this.characterCasing != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(CharacterCasing));
					}
					this.characterCasing = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x000770DF File Offset: 0x000760DF
		// (set) Token: 0x06002C29 RID: 11305 RVA: 0x000770E7 File Offset: 0x000760E7
		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				if (this.Multiline != value)
				{
					base.Multiline = value;
					if (value && this.AutoCompleteMode != AutoCompleteMode.None)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x0007710A File Offset: 0x0007610A
		internal override bool PasswordProtect
		{
			get
			{
				return this.PasswordChar != '\0';
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x00077118 File Offset: 0x00076118
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				switch (this.characterCasing)
				{
				case CharacterCasing.Upper:
					createParams.Style |= 8;
					break;
				case CharacterCasing.Lower:
					createParams.Style |= 16;
					break;
				}
				HorizontalAlignment horizontalAlignment = base.RtlTranslateHorizontal(this.textAlign);
				createParams.ExStyle &= -4097;
				switch (horizontalAlignment)
				{
				case HorizontalAlignment.Left:
				{
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
					break;
				}
				case HorizontalAlignment.Right:
					createParams.Style |= 2;
					break;
				case HorizontalAlignment.Center:
					createParams.Style |= 1;
					break;
				}
				if (this.Multiline)
				{
					if ((this.scrollBars & ScrollBars.Horizontal) == ScrollBars.Horizontal && this.textAlign == HorizontalAlignment.Left && !base.WordWrap)
					{
						createParams.Style |= 1048576;
					}
					if ((this.scrollBars & ScrollBars.Vertical) == ScrollBars.Vertical)
					{
						createParams.Style |= 2097152;
					}
				}
				if (this.useSystemPasswordChar)
				{
					createParams.Style |= 32;
				}
				return createParams;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x0007722F File Offset: 0x0007622F
		// (set) Token: 0x06002C2D RID: 11309 RVA: 0x00077254 File Offset: 0x00076254
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TextBoxPasswordCharDescr")]
		[DefaultValue('\0')]
		[Localizable(true)]
		public char PasswordChar
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				return (char)(int)base.SendMessage(210, 0, 0);
			}
			set
			{
				this.passwordChar = value;
				if (!this.useSystemPasswordChar && base.IsHandleCreated && this.PasswordChar != value)
				{
					base.SendMessage(204, (int)value, 0);
					base.VerifyImeRestrictedModeChanged();
					this.ResetAutoComplete(false);
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000772A2 File Offset: 0x000762A2
		// (set) Token: 0x06002C2F RID: 11311 RVA: 0x000772AA File Offset: 0x000762AA
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("TextBoxScrollBarsDescr")]
		[DefaultValue(ScrollBars.None)]
		public ScrollBars ScrollBars
		{
			get
			{
				return this.scrollBars;
			}
			set
			{
				if (this.scrollBars != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ScrollBars));
					}
					this.scrollBars = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000772E8 File Offset: 0x000762E8
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size empty = Size.Empty;
			if (this.Multiline && !base.WordWrap && (this.ScrollBars & ScrollBars.Horizontal) != ScrollBars.None)
			{
				empty.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			if (this.Multiline && (this.ScrollBars & ScrollBars.Vertical) != ScrollBars.None)
			{
				empty.Width += SystemInformation.VerticalScrollBarWidth;
			}
			proposedConstraints -= empty;
			Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			return preferredSizeCore + empty;
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x00077365 File Offset: 0x00076365
		// (set) Token: 0x06002C32 RID: 11314 RVA: 0x0007736D File Offset: 0x0007636D
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.selectionSet = false;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x0007737D File Offset: 0x0007637D
		// (set) Token: 0x06002C34 RID: 11316 RVA: 0x00077388 File Offset: 0x00076388
		[DefaultValue(HorizontalAlignment.Left)]
		[SRDescription("TextBoxTextAlignDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (this.textAlign != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
					}
					this.textAlign = value;
					base.RecreateHandle();
					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x000773DC File Offset: 0x000763DC
		// (set) Token: 0x06002C36 RID: 11318 RVA: 0x000773E4 File Offset: 0x000763E4
		[SRDescription("TextBoxUseSystemPasswordCharDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return this.useSystemPasswordChar;
			}
			set
			{
				if (value != this.useSystemPasswordChar)
				{
					this.useSystemPasswordChar = value;
					base.RecreateHandle();
					if (value)
					{
						this.ResetAutoComplete(false);
					}
				}
			}
		}

		// Token: 0x14000152 RID: 338
		// (add) Token: 0x06002C37 RID: 11319 RVA: 0x00077406 File Offset: 0x00076406
		// (remove) Token: 0x06002C38 RID: 11320 RVA: 0x00077419 File Offset: 0x00076419
		[SRDescription("RadioButtonOnTextAlignChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(TextBox.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBox.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x0007742C File Offset: 0x0007642C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ResetAutoComplete(true);
				if (this.autoCompleteCustomSource != null)
				{
					this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
				}
				if (this.stringSource != null)
				{
					this.stringSource.ReleaseAutoComplete();
					this.stringSource = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x00077484 File Offset: 0x00076484
		protected override bool IsInputKey(Keys keyData)
		{
			if (this.Multiline && (keyData & Keys.Alt) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
				{
					return this.acceptsReturn;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000774BD File Offset: 0x000764BD
		private void OnAutoCompleteCustomSourceChanged(object sender, CollectionChangeEventArgs e)
		{
			if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
			{
				this.SetAutoComplete(true);
			}
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000774D0 File Offset: 0x000764D0
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (Application.RenderWithVisualStyles && base.IsHandleCreated && base.BorderStyle == BorderStyle.Fixed3D)
			{
				SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
			}
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0007750E File Offset: 0x0007650E
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00077525 File Offset: 0x00076525
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (!this.selectionSet)
			{
				this.selectionSet = true;
				if (this.SelectionLength == 0 && Control.MouseButtons == MouseButtons.None)
				{
					base.SelectAll();
				}
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x00077554 File Offset: 0x00076554
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SetSelectionOnHandle();
			if (this.passwordChar != '\0' && !this.useSystemPasswordChar)
			{
				base.SendMessage(204, (int)this.passwordChar, 0);
			}
			base.VerifyImeRestrictedModeChanged();
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				try
				{
					this.fromHandleCreate = true;
					this.SetAutoComplete(false);
				}
				finally
				{
					this.fromHandleCreate = false;
				}
			}
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000775C8 File Offset: 0x000765C8
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (this.stringSource != null)
			{
				this.stringSource.ReleaseAutoComplete();
				this.stringSource = null;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000775EC File Offset: 0x000765EC
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBox.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x0007761A File Offset: 0x0007661A
		public void Paste(string text)
		{
			base.SetSelectedTextInternal(text, false);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x00077624 File Offset: 0x00076624
		internal override void SelectInternal(int start, int length, int textLen)
		{
			this.selectionSet = true;
			base.SelectInternal(start, length, textLen);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00077638 File Offset: 0x00076638
		private string[] GetStringsForAutoComplete()
		{
			string[] array = new string[this.AutoCompleteCustomSource.Count];
			for (int i = 0; i < this.AutoCompleteCustomSource.Count; i++)
			{
				array[i] = this.AutoCompleteCustomSource[i];
			}
			return array;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0007767C File Offset: 0x0007667C
		internal void SetAutoComplete(bool reset)
		{
			if (this.Multiline || this.passwordChar != '\0' || this.useSystemPasswordChar || this.AutoCompleteSource == AutoCompleteSource.None)
			{
				return;
			}
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				if (!this.fromHandleCreate)
				{
					AutoCompleteMode autoCompleteMode = this.AutoCompleteMode;
					this.autoCompleteMode = AutoCompleteMode.None;
					base.RecreateHandle();
					this.autoCompleteMode = autoCompleteMode;
				}
				if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
				{
					if (!base.IsHandleCreated || this.AutoCompleteCustomSource == null)
					{
						return;
					}
					if (this.AutoCompleteCustomSource.Count == 0)
					{
						this.ResetAutoComplete(true);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete());
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete());
					if (!this.stringSource.Bind(new HandleRef(this, base.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailure"));
					}
					return;
				}
				else
				{
					try
					{
						if (base.IsHandleCreated)
						{
							int num = 0;
							if (this.AutoCompleteMode == AutoCompleteMode.Suggest)
							{
								num |= -1879048192;
							}
							if (this.AutoCompleteMode == AutoCompleteMode.Append)
							{
								num |= 1610612736;
							}
							if (this.AutoCompleteMode == AutoCompleteMode.SuggestAppend)
							{
								num |= 268435456;
								num |= 1073741824;
							}
							SafeNativeMethods.SHAutoComplete(new HandleRef(this, base.Handle), (int)(this.AutoCompleteSource | (AutoCompleteSource)num));
						}
						return;
					}
					catch (SecurityException)
					{
						return;
					}
				}
			}
			if (reset)
			{
				this.ResetAutoComplete(true);
			}
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000777F0 File Offset: 0x000767F0
		private void ResetAutoComplete(bool force)
		{
			if ((this.AutoCompleteMode != AutoCompleteMode.None || force) && base.IsHandleCreated)
			{
				int num = -1610612729;
				SafeNativeMethods.SHAutoComplete(new HandleRef(this, base.Handle), num);
			}
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00077829 File Offset: 0x00076829
		private void ResetAutoCompleteCustomSource()
		{
			this.AutoCompleteCustomSource = null;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00077834 File Offset: 0x00076834
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && base.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rectangle = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rectangle);
						rectangle.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rectangle);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x00077904 File Offset: 0x00076904
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			switch (msg)
			{
			case 513:
			{
				MouseButtons mouseButtons = Control.MouseButtons;
				bool validationCancelled = base.ValidationCancelled;
				this.FocusInternal();
				if (mouseButtons == Control.MouseButtons && (!base.ValidationCancelled || validationCancelled))
				{
					base.WndProc(ref m);
					return;
				}
				break;
			}
			case 514:
				base.WndProc(ref m);
				return;
			default:
				if (msg == 791)
				{
					this.WmPrint(ref m);
					return;
				}
				base.WndProc(ref m);
				break;
			}
		}

		// Token: 0x04001825 RID: 6181
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x04001826 RID: 6182
		private bool acceptsReturn;

		// Token: 0x04001827 RID: 6183
		private char passwordChar;

		// Token: 0x04001828 RID: 6184
		private bool useSystemPasswordChar;

		// Token: 0x04001829 RID: 6185
		private CharacterCasing characterCasing;

		// Token: 0x0400182A RID: 6186
		private ScrollBars scrollBars;

		// Token: 0x0400182B RID: 6187
		private HorizontalAlignment textAlign;

		// Token: 0x0400182C RID: 6188
		private bool selectionSet;

		// Token: 0x0400182D RID: 6189
		private AutoCompleteMode autoCompleteMode;

		// Token: 0x0400182E RID: 6190
		private AutoCompleteSource autoCompleteSource = AutoCompleteSource.None;

		// Token: 0x0400182F RID: 6191
		private AutoCompleteStringCollection autoCompleteCustomSource;

		// Token: 0x04001830 RID: 6192
		private bool fromHandleCreate;

		// Token: 0x04001831 RID: 6193
		private StringSource stringSource;
	}
}

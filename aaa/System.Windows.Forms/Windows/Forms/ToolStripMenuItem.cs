using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020004A9 RID: 1193
	[DesignerSerializer("System.Windows.Forms.Design.ToolStripMenuItemCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripMenuItem : ToolStripDropDownItem
	{
		// Token: 0x060047A2 RID: 18338 RVA: 0x00103F89 File Offset: 0x00102F89
		public ToolStripMenuItem()
		{
			this.Initialize();
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x00103FC6 File Offset: 0x00102FC6
		public ToolStripMenuItem(string text)
			: base(text, null, null)
		{
			this.Initialize();
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x00104006 File Offset: 0x00103006
		public ToolStripMenuItem(Image image)
			: base(null, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x00104046 File Offset: 0x00103046
		public ToolStripMenuItem(string text, Image image)
			: base(text, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x00104086 File Offset: 0x00103086
		public ToolStripMenuItem(string text, Image image, EventHandler onClick)
			: base(text, image, onClick)
		{
			this.Initialize();
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x001040C8 File Offset: 0x001030C8
		public ToolStripMenuItem(string text, Image image, EventHandler onClick, string name)
			: base(text, image, onClick, name)
		{
			this.Initialize();
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x00104115 File Offset: 0x00103115
		public ToolStripMenuItem(string text, Image image, params ToolStripItem[] dropDownItems)
			: base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x00104158 File Offset: 0x00103158
		public ToolStripMenuItem(string text, Image image, EventHandler onClick, Keys shortcutKeys)
			: base(text, image, onClick)
		{
			this.Initialize();
			this.ShortcutKeys = shortcutKeys;
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x001041AC File Offset: 0x001031AC
		internal ToolStripMenuItem(Form mdiForm)
		{
			this.Initialize();
			base.Properties.SetObject(ToolStripMenuItem.PropMdiForm, mdiForm);
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x00104208 File Offset: 0x00103208
		internal ToolStripMenuItem(IntPtr hMenu, int nativeMenuCommandId, IWin32Window targetWindow)
		{
			this.Initialize();
			this.Overflow = ToolStripItemOverflow.Never;
			this.nativeMenuCommandID = nativeMenuCommandId;
			this.targetWindowHandle = Control.GetSafeHandle(targetWindow);
			this.nativeMenuHandle = hMenu;
			this.Image = this.GetNativeMenuItemImage();
			base.ImageScaling = ToolStripItemImageScaling.None;
			string nativeMenuItemTextAndShortcut = this.GetNativeMenuItemTextAndShortcut();
			if (nativeMenuItemTextAndShortcut != null)
			{
				string[] array = nativeMenuItemTextAndShortcut.Split(new char[] { '\t' });
				if (array.Length >= 1)
				{
					this.Text = array[0];
				}
				if (array.Length >= 2)
				{
					this.ShowShortcutKeys = true;
					this.ShortcutKeyDisplayString = array[1];
				}
			}
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x001042C7 File Offset: 0x001032C7
		internal override void AutoHide(ToolStripItem otherItemBeingSelected)
		{
			/*
An exception occurred when decompiling this method (060047AC)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.ToolStripMenuItem::AutoHide(System.Windows.Forms.ToolStripItem)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 283
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x001042EA File Offset: 0x001032EA
		private void ClearShortcutCache()
		{
			this.cachedShortcutSize = Size.Empty;
			this.cachedShortcutText = null;
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x001042FE File Offset: 0x001032FE
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x00104307 File Offset: 0x00103307
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripMenuItemInternalLayout(this);
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0010430F File Offset: 0x0010330F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripMenuItem.ToolStripMenuItemAccessibleObject(this);
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x00104317 File Offset: 0x00103317
		private void Initialize()
		{
			this.Overflow = ToolStripItemOverflow.Never;
			base.MouseDownAndUpMustBeInSameItem = false;
			base.SupportsDisabledHotTracking = true;
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x0010432E File Offset: 0x0010332E
		protected override Size DefaultSize
		{
			get
			{
				return new Size(32, 19);
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060047B3 RID: 18355 RVA: 0x00104339 File Offset: 0x00103339
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x00104340 File Offset: 0x00103340
		protected override Padding DefaultPadding
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return new Padding(0, 1, 0, 1);
				}
				return new Padding(4, 0, 4, 0);
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x00104360 File Offset: 0x00103360
		// (set) Token: 0x060047B6 RID: 18358 RVA: 0x001043B1 File Offset: 0x001033B1
		public override bool Enabled
		{
			get
			{
				if (this.nativeMenuCommandID != -1)
				{
					return base.Enabled && this.nativeMenuHandle != IntPtr.Zero && this.targetWindowHandle != IntPtr.Zero && this.GetNativeMenuItemEnabled();
				}
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x001043BA File Offset: 0x001033BA
		// (set) Token: 0x060047B8 RID: 18360 RVA: 0x001043C8 File Offset: 0x001033C8
		[DefaultValue(false)]
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("CheckBoxCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.CheckState != CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
					base.InvokePaint();
				}
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x001043E8 File Offset: 0x001033E8
		internal Image CheckedImage
		{
			get
			{
				/*
An exception occurred when decompiling this method (060047B9)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Drawing.Image System.Windows.Forms.ToolStripMenuItem::get_CheckedImage()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 439
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x00104477 File Offset: 0x00103477
		// (set) Token: 0x060047BB RID: 18363 RVA: 0x0010447F File Offset: 0x0010347F
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripButtonCheckOnClickDescr")]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x00104488 File Offset: 0x00103488
		// (set) Token: 0x060047BD RID: 18365 RVA: 0x001044BC File Offset: 0x001034BC
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(CheckState.Unchecked)]
		[SRDescription("CheckBoxCheckStateDescr")]
		[SRCategory("CatAppearance")]
		[Bindable(true)]
		public CheckState CheckState
		{
			get
			{
				bool flag = false;
				object obj = base.Properties.GetInteger(ToolStripMenuItem.PropCheckState, out flag);
				if (!flag)
				{
					return CheckState.Unchecked;
				}
				return (CheckState)obj;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (value != this.CheckState)
				{
					base.Properties.SetInteger(ToolStripMenuItem.PropCheckState, (int)value);
					this.OnCheckedChanged(EventArgs.Empty);
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000287 RID: 647
		// (add) Token: 0x060047BE RID: 18366 RVA: 0x0010451F File Offset: 0x0010351F
		// (remove) Token: 0x060047BF RID: 18367 RVA: 0x00104532 File Offset: 0x00103532
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripMenuItem.EventCheckedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripMenuItem.EventCheckedChanged, value);
			}
		}

		// Token: 0x14000288 RID: 648
		// (add) Token: 0x060047C0 RID: 18368 RVA: 0x00104545 File Offset: 0x00103545
		// (remove) Token: 0x060047C1 RID: 18369 RVA: 0x00104558 File Offset: 0x00103558
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripMenuItem.EventCheckStateChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripMenuItem.EventCheckStateChanged, value);
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x060047C2 RID: 18370 RVA: 0x0010456B File Offset: 0x0010356B
		// (set) Token: 0x060047C3 RID: 18371 RVA: 0x00104573 File Offset: 0x00103573
		[SRCategory("CatLayout")]
		[DefaultValue(ToolStripItemOverflow.Never)]
		[SRDescription("ToolStripItemOverflowDescr")]
		public new ToolStripItemOverflow Overflow
		{
			get
			{
				return base.Overflow;
			}
			set
			{
				base.Overflow = value;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x060047C4 RID: 18372 RVA: 0x0010457C File Offset: 0x0010357C
		// (set) Token: 0x060047C5 RID: 18373 RVA: 0x001045B0 File Offset: 0x001035B0
		[DefaultValue(Keys.None)]
		[SRDescription("MenuItemShortCutDescr")]
		[Localizable(true)]
		public Keys ShortcutKeys
		{
			get
			{
				bool flag = false;
				object obj = base.Properties.GetInteger(ToolStripMenuItem.PropShortcutKeys, out flag);
				if (!flag)
				{
					return Keys.None;
				}
				return (Keys)obj;
			}
			set
			{
				if (value != Keys.None && !ToolStripManager.IsValidShortcut(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Keys));
				}
				Keys shortcutKeys = this.ShortcutKeys;
				if (shortcutKeys != value)
				{
					this.ClearShortcutCache();
					ToolStrip owner = base.Owner;
					if (owner != null)
					{
						if (shortcutKeys != Keys.None)
						{
							owner.Shortcuts.Remove(shortcutKeys);
						}
						if (owner.Shortcuts.Contains(value))
						{
							owner.Shortcuts[value] = this;
						}
						else
						{
							owner.Shortcuts.Add(value, this);
						}
					}
					base.Properties.SetInteger(ToolStripMenuItem.PropShortcutKeys, (int)value);
					if (this.ShowShortcutKeys && base.IsOnDropDown)
					{
						ToolStripDropDownMenu toolStripDropDownMenu = base.GetCurrentParentDropDown() as ToolStripDropDownMenu;
						if (toolStripDropDownMenu != null)
						{
							LayoutTransaction.DoLayout(base.ParentInternal, this, "ShortcutKeys");
							toolStripDropDownMenu.AdjustSize();
						}
					}
				}
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x060047C6 RID: 18374 RVA: 0x00104690 File Offset: 0x00103690
		// (set) Token: 0x060047C7 RID: 18375 RVA: 0x00104698 File Offset: 0x00103698
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripMenuItemShortcutKeyDisplayStringDescr")]
		[Localizable(true)]
		public string ShortcutKeyDisplayString
		{
			get
			{
				return this.shortcutKeyDisplayString;
			}
			set
			{
				if (this.shortcutKeyDisplayString != value)
				{
					this.shortcutKeyDisplayString = value;
					this.ClearShortcutCache();
					if (this.ShowShortcutKeys)
					{
						ToolStripDropDown toolStripDropDown = base.ParentInternal as ToolStripDropDown;
						if (toolStripDropDown != null)
						{
							LayoutTransaction.DoLayout(toolStripDropDown, this, "ShortcutKeyDisplayString");
							toolStripDropDown.AdjustSize();
						}
					}
				}
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x001046E9 File Offset: 0x001036E9
		// (set) Token: 0x060047C9 RID: 18377 RVA: 0x001046F4 File Offset: 0x001036F4
		[DefaultValue(true)]
		[SRDescription("MenuItemShowShortCutDescr")]
		[Localizable(true)]
		public bool ShowShortcutKeys
		{
			get
			{
				return this.showShortcutKeys;
			}
			set
			{
				if (value != this.showShortcutKeys)
				{
					this.ClearShortcutCache();
					this.showShortcutKeys = value;
					ToolStripDropDown toolStripDropDown = base.ParentInternal as ToolStripDropDown;
					if (toolStripDropDown != null)
					{
						LayoutTransaction.DoLayout(toolStripDropDown, this, "ShortcutKeys");
						toolStripDropDown.AdjustSize();
					}
				}
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x060047CA RID: 18378 RVA: 0x00104738 File Offset: 0x00103738
		internal bool IsTopLevel
		{
			get
			{
				return !(base.ParentInternal is ToolStripDropDown);
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x060047CB RID: 18379 RVA: 0x00104748 File Offset: 0x00103748
		[Browsable(false)]
		public bool IsMdiWindowListEntry
		{
			get
			{
				return this.MdiForm != null;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x00104756 File Offset: 0x00103756
		internal static MenuTimer MenuTimer
		{
			get
			{
				return ToolStripMenuItem.menuTimer;
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x0010475D File Offset: 0x0010375D
		internal Form MdiForm
		{
			get
			{
				if (base.Properties.ContainsObject(ToolStripMenuItem.PropMdiForm))
				{
					return base.Properties.GetObject(ToolStripMenuItem.PropMdiForm) as Form;
				}
				return null;
			}
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x00104788 File Offset: 0x00103788
		internal ToolStripMenuItem Clone()
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
			toolStripMenuItem.Events.AddHandlers(base.Events);
			toolStripMenuItem.AccessibleName = base.AccessibleName;
			toolStripMenuItem.AccessibleRole = base.AccessibleRole;
			toolStripMenuItem.Alignment = base.Alignment;
			toolStripMenuItem.AllowDrop = this.AllowDrop;
			toolStripMenuItem.Anchor = base.Anchor;
			toolStripMenuItem.AutoSize = base.AutoSize;
			toolStripMenuItem.AutoToolTip = base.AutoToolTip;
			toolStripMenuItem.BackColor = this.BackColor;
			toolStripMenuItem.BackgroundImage = this.BackgroundImage;
			toolStripMenuItem.BackgroundImageLayout = this.BackgroundImageLayout;
			toolStripMenuItem.Checked = this.Checked;
			toolStripMenuItem.CheckOnClick = this.CheckOnClick;
			toolStripMenuItem.CheckState = this.CheckState;
			toolStripMenuItem.DisplayStyle = this.DisplayStyle;
			toolStripMenuItem.Dock = base.Dock;
			toolStripMenuItem.DoubleClickEnabled = base.DoubleClickEnabled;
			toolStripMenuItem.Enabled = this.Enabled;
			toolStripMenuItem.Font = this.Font;
			toolStripMenuItem.ForeColor = this.ForeColor;
			toolStripMenuItem.Image = this.Image;
			toolStripMenuItem.ImageAlign = base.ImageAlign;
			toolStripMenuItem.ImageScaling = base.ImageScaling;
			toolStripMenuItem.ImageTransparentColor = base.ImageTransparentColor;
			toolStripMenuItem.Margin = base.Margin;
			toolStripMenuItem.MergeAction = base.MergeAction;
			toolStripMenuItem.MergeIndex = base.MergeIndex;
			toolStripMenuItem.Name = base.Name;
			toolStripMenuItem.Overflow = this.Overflow;
			toolStripMenuItem.Padding = this.Padding;
			toolStripMenuItem.RightToLeft = this.RightToLeft;
			toolStripMenuItem.ShortcutKeys = this.ShortcutKeys;
			toolStripMenuItem.ShowShortcutKeys = this.ShowShortcutKeys;
			toolStripMenuItem.Tag = base.Tag;
			toolStripMenuItem.Text = this.Text;
			toolStripMenuItem.TextAlign = this.TextAlign;
			toolStripMenuItem.TextDirection = this.TextDirection;
			toolStripMenuItem.TextImageRelation = base.TextImageRelation;
			toolStripMenuItem.ToolTipText = base.ToolTipText;
			toolStripMenuItem.Visible = ((IArrangedElement)this).ParticipatesInLayout;
			if (!base.AutoSize)
			{
				toolStripMenuItem.Size = this.Size;
			}
			return toolStripMenuItem;
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x00104998 File Offset: 0x00103998
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.lastOwner != null)
			{
				Keys shortcutKeys = this.ShortcutKeys;
				if (shortcutKeys != Keys.None && this.lastOwner.Shortcuts.ContainsKey(shortcutKeys))
				{
					this.lastOwner.Shortcuts.Remove(shortcutKeys);
				}
				this.lastOwner = null;
				if (this.MdiForm != null)
				{
					base.Properties.SetObject(ToolStripMenuItem.PropMdiForm, null);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x00104A10 File Offset: 0x00103A10
		private bool GetNativeMenuItemEnabled()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return false;
			}
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));
			menuiteminfo_T_RW.fMask = 1;
			menuiteminfo_T_RW.fType = 1;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			return (menuiteminfo_T_RW.fState & 3) == 0;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x00104A94 File Offset: 0x00103A94
		private string GetNativeMenuItemTextAndShortcut()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return null;
			}
			string text = null;
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.fMask = 64;
			menuiteminfo_T_RW.fType = 64;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			menuiteminfo_T_RW.dwTypeData = IntPtr.Zero;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			if (menuiteminfo_T_RW.cch > 0)
			{
				menuiteminfo_T_RW.cch++;
				menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
				IntPtr intPtr = Marshal.AllocCoTaskMem(menuiteminfo_T_RW.cch * Marshal.SystemDefaultCharSize);
				menuiteminfo_T_RW.dwTypeData = intPtr;
				try
				{
					UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
					if (menuiteminfo_T_RW.dwTypeData != IntPtr.Zero)
					{
						text = Marshal.PtrToStringAuto(menuiteminfo_T_RW.dwTypeData, menuiteminfo_T_RW.cch);
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
			}
			return text;
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x00104BAC File Offset: 0x00103BAC
		private Image GetNativeMenuItemImage()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return null;
			}
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.fMask = 128;
			menuiteminfo_T_RW.fType = 128;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			if (menuiteminfo_T_RW.hbmpItem != IntPtr.Zero && menuiteminfo_T_RW.hbmpItem.ToInt32() > 11)
			{
				return Image.FromHbitmap(menuiteminfo_T_RW.hbmpItem);
			}
			int num = -1;
			switch (menuiteminfo_T_RW.hbmpItem.ToInt32())
			{
			case 2:
			case 9:
				num = 3;
				break;
			case 3:
			case 7:
			case 11:
				num = 1;
				break;
			case 5:
			case 6:
			case 8:
				num = 0;
				break;
			case 10:
				num = 2;
				break;
			}
			if (num > -1)
			{
				Bitmap bitmap = new Bitmap(16, 16);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					ControlPaint.DrawCaptionButton(graphics, new Rectangle(Point.Empty, bitmap.Size), (CaptionButton)num, ButtonState.Flat);
					graphics.DrawRectangle(SystemPens.Control, 0, 0, bitmap.Width - 1, bitmap.Height - 1);
				}
				bitmap.MakeTransparent(SystemColors.Control);
				return bitmap;
			}
			return null;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00104D1C File Offset: 0x00103D1C
		internal Size GetShortcutTextSize()
		{
			if (!this.ShowShortcutKeys)
			{
				return Size.Empty;
			}
			string shortcutText = this.GetShortcutText();
			if (string.IsNullOrEmpty(shortcutText))
			{
				return Size.Empty;
			}
			if (this.cachedShortcutSize == Size.Empty)
			{
				this.cachedShortcutSize = TextRenderer.MeasureText(shortcutText, this.Font);
			}
			return this.cachedShortcutSize;
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x00104D76 File Offset: 0x00103D76
		private string GetShortcutText()
		{
			if (this.cachedShortcutText == null)
			{
				this.cachedShortcutText = ToolStripMenuItem.ShortcutToText(this.ShortcutKeys, this.ShortcutKeyDisplayString);
			}
			return this.cachedShortcutText;
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00104D9D File Offset: 0x00103D9D
		internal void HandleAutoExpansion()
		{
			if (this.Enabled && base.ParentInternal != null && base.ParentInternal.MenuAutoExpand && this.HasDropDownItems)
			{
				base.ShowDropDown();
				base.DropDown.SelectNextToolStripItem(null, true);
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x00104DD8 File Offset: 0x00103DD8
		protected override void OnClick(EventArgs e)
		{
			if (this.checkOnClick)
			{
				this.Checked = !this.Checked;
			}
			base.OnClick(e);
			if (this.nativeMenuCommandID != -1)
			{
				if ((this.nativeMenuCommandID & 61440) != 0)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, this.targetWindowHandle), 274, this.nativeMenuCommandID, 0);
				}
				else
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, this.targetWindowHandle), 273, this.nativeMenuCommandID, 0);
				}
				base.Invalidate();
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00104E60 File Offset: 0x00103E60
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripMenuItem.EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00104E90 File Offset: 0x00103E90
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripMenuItem.EventCheckStateChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00104EC9 File Offset: 0x00103EC9
		protected override void OnDropDownHide(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			base.OnDropDownHide(e);
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x00104EDD File Offset: 0x00103EDD
		protected override void OnDropDownShow(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			if (base.ParentInternal != null)
			{
				base.ParentInternal.MenuAutoExpand = true;
			}
			base.OnDropDownShow(e);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x00104F05 File Offset: 0x00103F05
		protected override void OnFontChanged(EventArgs e)
		{
			this.ClearShortcutCache();
			base.OnFontChanged(e);
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x00104F14 File Offset: 0x00103F14
		internal void OnMenuAutoExpand()
		{
			base.ShowDropDown();
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00104F1C File Offset: 0x00103F1C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			this.OnMouseButtonStateChange(e, true);
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00104F31 File Offset: 0x00103F31
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.OnMouseButtonStateChange(e, false);
			base.OnMouseUp(e);
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00104F44 File Offset: 0x00103F44
		private void OnMouseButtonStateChange(MouseEventArgs e, bool isMouseDown)
		{
			bool flag = true;
			if (base.IsOnDropDown)
			{
				ToolStripDropDown currentParentDropDown = base.GetCurrentParentDropDown();
				base.SupportsRightClick = currentParentDropDown.GetFirstDropDown() is ContextMenuStrip;
			}
			else
			{
				flag = !base.DropDown.Visible;
				base.SupportsRightClick = false;
			}
			if (e.Button == MouseButtons.Left || (e.Button == MouseButtons.Right && base.SupportsRightClick))
			{
				if (isMouseDown && flag)
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
					return;
				}
				if (!isMouseDown && !flag)
				{
					byte b = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					int num = (int)this.openMouseId;
					if ((int)b != num)
					{
						this.openMouseId = 0;
						ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
						base.Select();
					}
				}
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0010501B File Offset: 0x0010401B
		protected override void OnMouseEnter(EventArgs e)
		{
			if (base.ParentInternal != null && base.ParentInternal.MenuAutoExpand && this.Selected)
			{
				ToolStripMenuItem.MenuTimer.Cancel(this);
				ToolStripMenuItem.MenuTimer.Start(this);
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00105057 File Offset: 0x00104057
		protected override void OnMouseLeave(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			base.OnMouseLeave(e);
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0010506C File Offset: 0x0010406C
		protected override void OnOwnerChanged(EventArgs e)
		{
			Keys shortcutKeys = this.ShortcutKeys;
			if (shortcutKeys != Keys.None)
			{
				if (this.lastOwner != null)
				{
					this.lastOwner.Shortcuts.Remove(shortcutKeys);
				}
				if (base.Owner != null)
				{
					if (base.Owner.Shortcuts.Contains(shortcutKeys))
					{
						base.Owner.Shortcuts[shortcutKeys] = this;
					}
					else
					{
						base.Owner.Shortcuts.Add(shortcutKeys, this);
					}
					this.lastOwner = base.Owner;
				}
			}
			base.OnOwnerChanged(e);
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x00105104 File Offset: 0x00104104
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				Graphics graphics = e.Graphics;
				renderer.DrawMenuItemBackground(new ToolStripItemRenderEventArgs(graphics, this));
				Color color = SystemColors.MenuText;
				if (base.IsForeColorSet)
				{
					color = this.ForeColor;
				}
				else if (!this.IsTopLevel || ToolStripManager.VisualStylesEnabled)
				{
					if (this.Selected || this.Pressed)
					{
						color = SystemColors.HighlightText;
					}
					else
					{
						color = SystemColors.MenuText;
					}
				}
				bool flag = this.RightToLeft == RightToLeft.Yes;
				ToolStripMenuItemInternalLayout toolStripMenuItemInternalLayout = base.InternalLayout as ToolStripMenuItemInternalLayout;
				if (toolStripMenuItemInternalLayout != null && toolStripMenuItemInternalLayout.UseMenuLayout)
				{
					if (this.CheckState != CheckState.Unchecked && toolStripMenuItemInternalLayout.PaintCheck)
					{
						Rectangle rectangle = toolStripMenuItemInternalLayout.CheckRectangle;
						if (!toolStripMenuItemInternalLayout.ShowCheckMargin)
						{
							rectangle = toolStripMenuItemInternalLayout.ImageRectangle;
						}
						if (rectangle.Width != 0)
						{
							renderer.DrawItemCheck(new ToolStripItemImageRenderEventArgs(graphics, this, this.CheckedImage, rectangle));
						}
					}
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, color, this.Font, flag ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft));
						bool flag2 = this.ShowShortcutKeys;
						if (!base.DesignMode)
						{
							flag2 = flag2 && !this.HasDropDownItems;
						}
						if (flag2)
						{
							renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.GetShortcutText(), base.InternalLayout.TextRectangle, color, this.Font, flag ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight));
						}
					}
					if (this.HasDropDownItems)
					{
						ArrowDirection arrowDirection = (flag ? ArrowDirection.Left : ArrowDirection.Right);
						Color color2 = ((this.Selected || this.Pressed) ? SystemColors.HighlightText : SystemColors.MenuText);
						color2 = (this.Enabled ? color2 : SystemColors.ControlDark);
						renderer.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, this, toolStripMenuItemInternalLayout.ArrowRectangle, color2, arrowDirection));
					}
					if (toolStripMenuItemInternalLayout.PaintImage && (this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image && this.Image != null)
					{
						renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
						return;
					}
				}
				else
				{
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, color, this.Font, base.InternalLayout.TextFormat));
					}
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image && this.Image != null)
					{
						renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
					}
				}
			}
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0010537B File Offset: 0x0010437B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (this.Enabled && this.ShortcutKeys == keyData && !this.HasDropDownItems)
			{
				base.FireEvent(ToolStripItemEventType.Click);
				return true;
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x001053A7 File Offset: 0x001043A7
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.HasDropDownItems)
			{
				base.Select();
				base.ShowDropDown();
				base.DropDown.SelectNextToolStripItem(null, true);
				return true;
			}
			return base.ProcessMnemonic(charCode);
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x001053D4 File Offset: 0x001043D4
		protected internal override void SetBounds(Rectangle rect)
		{
			ToolStripMenuItemInternalLayout toolStripMenuItemInternalLayout = base.InternalLayout as ToolStripMenuItemInternalLayout;
			if (toolStripMenuItemInternalLayout != null && toolStripMenuItemInternalLayout.UseMenuLayout)
			{
				ToolStripDropDownMenu toolStripDropDownMenu = base.Owner as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					rect.X -= toolStripDropDownMenu.Padding.Left;
					rect.X = Math.Max(rect.X, 0);
				}
			}
			base.SetBounds(rect);
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0010543E File Offset: 0x0010443E
		internal void SetNativeTargetWindow(IWin32Window window)
		{
			this.targetWindowHandle = Control.GetSafeHandle(window);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0010544C File Offset: 0x0010444C
		internal void SetNativeTargetMenu(IntPtr hMenu)
		{
			this.nativeMenuHandle = hMenu;
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x00105455 File Offset: 0x00104455
		internal static string ShortcutToText(Keys shortcutKeys, string shortcutKeyDisplayString)
		{
			if (!string.IsNullOrEmpty(shortcutKeyDisplayString))
			{
				return shortcutKeyDisplayString;
			}
			if (shortcutKeys == Keys.None)
			{
				return string.Empty;
			}
			return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(shortcutKeys);
		}

		// Token: 0x040021E7 RID: 8679
		private static MenuTimer menuTimer = new MenuTimer();

		// Token: 0x040021E8 RID: 8680
		private static readonly int PropShortcutKeys = PropertyStore.CreateKey();

		// Token: 0x040021E9 RID: 8681
		private static readonly int PropCheckState = PropertyStore.CreateKey();

		// Token: 0x040021EA RID: 8682
		private static readonly int PropMdiForm = PropertyStore.CreateKey();

		// Token: 0x040021EB RID: 8683
		private bool checkOnClick;

		// Token: 0x040021EC RID: 8684
		private bool showShortcutKeys = true;

		// Token: 0x040021ED RID: 8685
		private ToolStrip lastOwner;

		// Token: 0x040021EE RID: 8686
		private int nativeMenuCommandID = -1;

		// Token: 0x040021EF RID: 8687
		private IntPtr targetWindowHandle = IntPtr.Zero;

		// Token: 0x040021F0 RID: 8688
		private IntPtr nativeMenuHandle = IntPtr.Zero;

		// Token: 0x040021F1 RID: 8689
		[ThreadStatic]
		private static Image indeterminateCheckedImage;

		// Token: 0x040021F2 RID: 8690
		[ThreadStatic]
		private static Image checkedImage;

		// Token: 0x040021F3 RID: 8691
		private string shortcutKeyDisplayString;

		// Token: 0x040021F4 RID: 8692
		private string cachedShortcutText;

		// Token: 0x040021F5 RID: 8693
		private Size cachedShortcutSize = Size.Empty;

		// Token: 0x040021F6 RID: 8694
		private byte openMouseId;

		// Token: 0x040021F7 RID: 8695
		private static readonly object EventCheckedChanged = new object();

		// Token: 0x040021F8 RID: 8696
		private static readonly object EventCheckStateChanged = new object();

		// Token: 0x020004AB RID: 1195
		[ComVisible(true)]
		internal class ToolStripMenuItemAccessibleObject : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x060047F0 RID: 18416 RVA: 0x001055C3 File Offset: 0x001045C3
			public ToolStripMenuItemAccessibleObject(ToolStripMenuItem ownerItem)
				: base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x17000E57 RID: 3671
			// (get) Token: 0x060047F1 RID: 18417 RVA: 0x001055D4 File Offset: 0x001045D4
			public override AccessibleStates State
			{
				get
				{
					if (this.ownerItem.Enabled)
					{
						AccessibleStates accessibleStates = base.State;
						if ((accessibleStates & AccessibleStates.Pressed) == AccessibleStates.Pressed)
						{
							accessibleStates &= ~AccessibleStates.Pressed;
						}
						if (this.ownerItem.Checked)
						{
							accessibleStates |= AccessibleStates.Checked;
						}
						return accessibleStates;
					}
					return base.State;
				}
			}

			// Token: 0x040021FA RID: 8698
			private ToolStripMenuItem ownerItem;
		}
	}
}

﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020004A8 RID: 1192
	[Designer("System.Windows.Forms.Design.ToolStripMenuItemDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("DropDownItems")]
	public abstract class ToolStripDropDownItem : ToolStripItem
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x00103393 File Offset: 0x00102393
		protected ToolStripDropDownItem()
		{
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x001033A2 File Offset: 0x001023A2
		protected ToolStripDropDownItem(string text, Image image, EventHandler onClick)
			: base(text, image, onClick)
		{
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x001033B4 File Offset: 0x001023B4
		protected ToolStripDropDownItem(string text, Image image, EventHandler onClick, string name)
			: base(text, image, onClick, name)
		{
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x001033C8 File Offset: 0x001023C8
		protected ToolStripDropDownItem(string text, Image image, params ToolStripItem[] dropDownItems)
			: this(text, image, null)
		{
			if (dropDownItems != null)
			{
				this.DropDownItems.AddRange(dropDownItems);
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06004772 RID: 18290 RVA: 0x001033E2 File Offset: 0x001023E2
		// (set) Token: 0x06004773 RID: 18291 RVA: 0x00103414 File Offset: 0x00102414
		[SRDescription("ToolStripDropDownDescr")]
		[SRCategory("CatData")]
		[TypeConverter(typeof(ReferenceConverter))]
		public ToolStripDropDown DropDown
		{
			get
			{
				if (this.dropDown == null)
				{
					this.DropDown = this.CreateDefaultDropDown();
					if (!(this is ToolStripOverflowButton))
					{
						this.dropDown.SetAutoGeneratedInternal(true);
					}
				}
				return this.dropDown;
			}
			set
			{
				if (this.dropDown != value)
				{
					if (this.dropDown != null)
					{
						this.dropDown.Opened -= this.DropDown_Opened;
						this.dropDown.Closed -= this.DropDown_Closed;
						this.dropDown.ItemClicked -= this.DropDown_ItemClicked;
						this.dropDown.UnassignDropDownItem();
					}
					this.dropDown = value;
					if (this.dropDown != null)
					{
						this.dropDown.Opened += this.DropDown_Opened;
						this.dropDown.Closed += this.DropDown_Closed;
						this.dropDown.ItemClicked += this.DropDown_ItemClicked;
						this.dropDown.AssignToDropDownItem();
					}
				}
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x001034E4 File Offset: 0x001024E4
		internal virtual Rectangle DropDownButtonArea
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06004775 RID: 18293 RVA: 0x001034EC File Offset: 0x001024EC
		// (set) Token: 0x06004776 RID: 18294 RVA: 0x001035CC File Offset: 0x001025CC
		[SRDescription("ToolStripDropDownItemDropDownDirectionDescr")]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		public ToolStripDropDownDirection DropDownDirection
		{
			get
			{
				if (this.toolStripDropDownDirection == ToolStripDropDownDirection.Default)
				{
					ToolStrip parentInternal = base.ParentInternal;
					if (parentInternal != null)
					{
						ToolStripDropDownDirection toolStripDropDownDirection = parentInternal.DefaultDropDownDirection;
						if (this.OppositeDropDownAlign || (this.RightToLeft != parentInternal.RightToLeft && this.RightToLeft != RightToLeft.Inherit))
						{
							toolStripDropDownDirection = this.RTLTranslateDropDownDirection(toolStripDropDownDirection, this.RightToLeft);
						}
						if (base.IsOnDropDown)
						{
							Rectangle dropDownBounds = this.GetDropDownBounds(toolStripDropDownDirection);
							Rectangle rectangle = new Rectangle(base.TranslatePoint(Point.Empty, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords), this.Size);
							Rectangle rectangle2 = Rectangle.Intersect(dropDownBounds, rectangle);
							if (rectangle2.Width >= 2)
							{
								RightToLeft rightToLeft = ((this.RightToLeft == RightToLeft.Yes) ? RightToLeft.No : RightToLeft.Yes);
								ToolStripDropDownDirection toolStripDropDownDirection2 = this.RTLTranslateDropDownDirection(toolStripDropDownDirection, rightToLeft);
								int width = Rectangle.Intersect(this.GetDropDownBounds(toolStripDropDownDirection2), rectangle).Width;
								if (width < rectangle2.Width)
								{
									toolStripDropDownDirection = toolStripDropDownDirection2;
								}
							}
						}
						return toolStripDropDownDirection;
					}
				}
				return this.toolStripDropDownDirection;
			}
			set
			{
				switch (value)
				{
				case ToolStripDropDownDirection.AboveLeft:
				case ToolStripDropDownDirection.AboveRight:
				case ToolStripDropDownDirection.BelowLeft:
				case ToolStripDropDownDirection.BelowRight:
				case ToolStripDropDownDirection.Left:
				case ToolStripDropDownDirection.Right:
				case ToolStripDropDownDirection.Default:
					if (this.toolStripDropDownDirection != value)
					{
						this.toolStripDropDownDirection = value;
						if (this.HasDropDownItems && this.DropDown.Visible)
						{
							this.DropDown.Location = this.DropDownLocation;
						}
					}
					return;
				default:
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripDropDownDirection));
				}
			}
		}

		// Token: 0x14000283 RID: 643
		// (add) Token: 0x06004777 RID: 18295 RVA: 0x0010364D File Offset: 0x0010264D
		// (remove) Token: 0x06004778 RID: 18296 RVA: 0x00103660 File Offset: 0x00102660
		[SRDescription("ToolStripDropDownClosedDecr")]
		[SRCategory("CatAction")]
		public event EventHandler DropDownClosed
		{
			add
			{
				base.Events.AddHandler(ToolStripDropDownItem.EventDropDownClosed, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripDropDownItem.EventDropDownClosed, value);
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06004779 RID: 18297 RVA: 0x00103674 File Offset: 0x00102674
		protected internal virtual Point DropDownLocation
		{
			get
			{
				if (base.ParentInternal == null || !this.HasDropDownItems)
				{
					return Point.Empty;
				}
				ToolStripDropDownDirection dropDownDirection = this.DropDownDirection;
				return this.GetDropDownBounds(dropDownDirection).Location;
			}
		}

		// Token: 0x14000284 RID: 644
		// (add) Token: 0x0600477A RID: 18298 RVA: 0x001036AD File Offset: 0x001026AD
		// (remove) Token: 0x0600477B RID: 18299 RVA: 0x001036C0 File Offset: 0x001026C0
		[SRCategory("CatAction")]
		[SRDescription("ToolStripDropDownOpeningDescr")]
		public event EventHandler DropDownOpening
		{
			add
			{
				base.Events.AddHandler(ToolStripDropDownItem.EventDropDownShow, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripDropDownItem.EventDropDownShow, value);
			}
		}

		// Token: 0x14000285 RID: 645
		// (add) Token: 0x0600477C RID: 18300 RVA: 0x001036D3 File Offset: 0x001026D3
		// (remove) Token: 0x0600477D RID: 18301 RVA: 0x001036E6 File Offset: 0x001026E6
		[SRCategory("CatAction")]
		[SRDescription("ToolStripDropDownOpenedDescr")]
		public event EventHandler DropDownOpened
		{
			add
			{
				base.Events.AddHandler(ToolStripDropDownItem.EventDropDownOpened, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripDropDownItem.EventDropDownOpened, value);
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x0600477E RID: 18302 RVA: 0x001036F9 File Offset: 0x001026F9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatData")]
		[SRDescription("ToolStripDropDownItemsDescr")]
		public ToolStripItemCollection DropDownItems
		{
			get
			{
				return this.DropDown.Items;
			}
		}

		// Token: 0x14000286 RID: 646
		// (add) Token: 0x0600477F RID: 18303 RVA: 0x00103706 File Offset: 0x00102706
		// (remove) Token: 0x06004780 RID: 18304 RVA: 0x00103719 File Offset: 0x00102719
		[SRCategory("CatAction")]
		public event ToolStripItemClickedEventHandler DropDownItemClicked
		{
			add
			{
				base.Events.AddHandler(ToolStripDropDownItem.EventDropDownItemClicked, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripDropDownItem.EventDropDownItemClicked, value);
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x0010372C File Offset: 0x0010272C
		[Browsable(false)]
		public virtual bool HasDropDownItems
		{
			get
			{
				return this.dropDown != null && this.dropDown.HasVisibleItems;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x00103743 File Offset: 0x00102743
		internal bool HasDropDown
		{
			get
			{
				return this.dropDown != null;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06004783 RID: 18307 RVA: 0x00103754 File Offset: 0x00102754
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override bool Pressed
		{
			get
			{
				if (this.dropDown != null && (this.DropDown.AutoClose || !base.IsInDesignMode || (base.IsInDesignMode && !base.IsOnDropDown)))
				{
					return this.DropDown.OwnerItem == this && this.DropDown.Visible;
				}
				return base.Pressed;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06004784 RID: 18308 RVA: 0x001037B0 File Offset: 0x001027B0
		internal virtual bool OppositeDropDownAlign
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x001037B3 File Offset: 0x001027B3
		internal virtual void AutoHide(ToolStripItem otherItemBeingSelected)
		{
			this.HideDropDown();
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x001037BB File Offset: 0x001027BB
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripDropDownItemAccessibleObject(this);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x001037C3 File Offset: 0x001027C3
		protected virtual ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDown(this, true);
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x001037CC File Offset: 0x001027CC
		private Rectangle DropDownDirectionToDropDownBounds(ToolStripDropDownDirection dropDownDirection, Rectangle dropDownBounds)
		{
			Point empty = Point.Empty;
			switch (dropDownDirection)
			{
			case ToolStripDropDownDirection.AboveLeft:
				empty.X = -dropDownBounds.Width + base.Width;
				empty.Y = -dropDownBounds.Height + 1;
				break;
			case ToolStripDropDownDirection.AboveRight:
				empty.Y = -dropDownBounds.Height + 1;
				break;
			case ToolStripDropDownDirection.BelowLeft:
				empty.X = -dropDownBounds.Width + base.Width;
				empty.Y = base.Height - 1;
				break;
			case ToolStripDropDownDirection.BelowRight:
				empty.Y = base.Height - 1;
				break;
			case ToolStripDropDownDirection.Left:
				empty.X = -dropDownBounds.Width;
				break;
			case ToolStripDropDownDirection.Right:
				empty.X = base.Width;
				if (!base.IsOnDropDown)
				{
					empty.X--;
				}
				break;
			}
			Point point = base.TranslatePoint(Point.Empty, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords);
			dropDownBounds.Location = new Point(point.X + empty.X, point.Y + empty.Y);
			dropDownBounds = WindowsFormsUtils.ConstrainToScreenWorkingAreaBounds(dropDownBounds);
			return dropDownBounds;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x001038F1 File Offset: 0x001028F1
		private void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this.OnDropDownClosed(EventArgs.Empty);
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x001038FE File Offset: 0x001028FE
		private void DropDown_Opened(object sender, EventArgs e)
		{
			this.OnDropDownOpened(EventArgs.Empty);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0010390B File Offset: 0x0010290B
		private void DropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.OnDropDownItemClicked(e);
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x00103914 File Offset: 0x00102914
		protected override void Dispose(bool disposing)
		{
			if (this.dropDown != null)
			{
				this.dropDown.Opened -= this.DropDown_Opened;
				this.dropDown.Closed -= this.DropDown_Closed;
				this.dropDown.ItemClicked -= this.DropDown_ItemClicked;
				if (disposing && this.dropDown.IsAutoGenerated)
				{
					this.dropDown.Dispose();
					this.dropDown = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x00103998 File Offset: 0x00102998
		private Rectangle GetDropDownBounds(ToolStripDropDownDirection dropDownDirection)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, this.DropDown.GetSuggestedSize());
			rectangle = this.DropDownDirectionToDropDownBounds(dropDownDirection, rectangle);
			Rectangle rectangle2 = new Rectangle(base.TranslatePoint(Point.Empty, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords), this.Size);
			if (Rectangle.Intersect(rectangle, rectangle2).Height > 1)
			{
				bool flag = this.RightToLeft == RightToLeft.Yes;
				if (Rectangle.Intersect(rectangle, rectangle2).Width > 1)
				{
					rectangle = this.DropDownDirectionToDropDownBounds((!flag) ? ToolStripDropDownDirection.Right : ToolStripDropDownDirection.Left, rectangle);
				}
				if (Rectangle.Intersect(rectangle, rectangle2).Width > 1)
				{
					rectangle = this.DropDownDirectionToDropDownBounds((!flag) ? ToolStripDropDownDirection.Left : ToolStripDropDownDirection.Right, rectangle);
				}
			}
			return rectangle;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x00103A40 File Offset: 0x00102A40
		public void HideDropDown()
		{
			this.OnDropDownHide(EventArgs.Empty);
			if (this.dropDown != null && this.dropDown.Visible)
			{
				this.DropDown.Visible = false;
			}
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x00103A6E File Offset: 0x00102A6E
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (this.dropDown != null)
			{
				this.dropDown.OnOwnerItemFontChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x00103A8F File Offset: 0x00102A8F
		protected override void OnBoundsChanged()
		{
			base.OnBoundsChanged();
			if (this.dropDown != null && this.dropDown.Visible)
			{
				this.dropDown.Bounds = this.GetDropDownBounds(this.DropDownDirection);
			}
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x00103AC4 File Offset: 0x00102AC4
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			if (this.HasDropDownItems)
			{
				if (this.DropDown.Visible)
				{
					LayoutTransaction.DoLayout(this.DropDown, this, PropertyNames.RightToLeft);
					return;
				}
				CommonProperties.xClearPreferredSizeCache(this.DropDown);
				this.DropDown.LayoutRequired = true;
			}
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x00103B16 File Offset: 0x00102B16
		internal override void OnImageScalingSizeChanged(EventArgs e)
		{
			base.OnImageScalingSizeChanged(e);
			if (this.HasDropDown && this.DropDown.IsAutoGenerated)
			{
				this.DropDown.DoLayoutIfHandleCreated(new ToolStripItemEventArgs(this));
			}
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x00103B48 File Offset: 0x00102B48
		protected virtual void OnDropDownHide(EventArgs e)
		{
			base.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripDropDownItem.EventDropDownHide];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x00103B7C File Offset: 0x00102B7C
		protected virtual void OnDropDownShow(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripDropDownItem.EventDropDownShow];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x00103BAC File Offset: 0x00102BAC
		protected internal virtual void OnDropDownOpened(EventArgs e)
		{
			if (this.DropDown.OwnerItem == this)
			{
				EventHandler eventHandler = (EventHandler)base.Events[ToolStripDropDownItem.EventDropDownOpened];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x00103BE8 File Offset: 0x00102BE8
		protected internal virtual void OnDropDownClosed(EventArgs e)
		{
			/*
An exception occurred when decompiling this method (06004796)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.ToolStripDropDownItem::OnDropDownClosed(System.EventArgs)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.FlattenBasicBlocks(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1860
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.FlattenBasicBlocks(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1878
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.FlattenBasicBlocks(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1878
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.FlattenBasicBlocks(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1846
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 355
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x00103C44 File Offset: 0x00102C44
		protected internal virtual void OnDropDownItemClicked(ToolStripItemClickedEventArgs e)
		{
			if (this.DropDown.OwnerItem == this)
			{
				ToolStripItemClickedEventHandler toolStripItemClickedEventHandler = (ToolStripItemClickedEventHandler)base.Events[ToolStripDropDownItem.EventDropDownItemClicked];
				if (toolStripItemClickedEventHandler != null)
				{
					toolStripItemClickedEventHandler(this, e);
				}
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x00103C80 File Offset: 0x00102C80
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (this.HasDropDownItems)
			{
				return this.DropDown.ProcessCmdKeyInternal(ref m, keyData);
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x00103CA0 File Offset: 0x00102CA0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (this.HasDropDownItems)
			{
				bool flag = !base.IsOnDropDown || base.IsOnOverflow;
				if (flag && (keys == Keys.Down || keys == Keys.Up || keys == Keys.Return || (base.SupportsSpaceKey && keys == Keys.Space)))
				{
					if (this.Enabled || base.DesignMode)
					{
						this.ShowDropDown();
						this.DropDown.SelectNextToolStripItem(null, true);
					}
					return true;
				}
				if (!flag)
				{
					bool flag2 = (this.DropDownDirection & ToolStripDropDownDirection.AboveRight) == ToolStripDropDownDirection.AboveLeft;
					bool flag3 = keys == Keys.Return || (base.SupportsSpaceKey && keys == Keys.Space) || (flag2 && keys == Keys.Left) || (!flag2 && keys == Keys.Right);
					if (flag3)
					{
						if (this.Enabled || base.DesignMode)
						{
							this.ShowDropDown();
							this.DropDown.SelectNextToolStripItem(null, true);
						}
						return true;
					}
				}
			}
			if (base.IsOnDropDown)
			{
				bool flag4 = (this.DropDownDirection & ToolStripDropDownDirection.AboveRight) == ToolStripDropDownDirection.AboveLeft;
				bool flag5 = (flag4 && keys == Keys.Right) || (!flag4 && keys == Keys.Left);
				if (flag5)
				{
					ToolStripDropDown currentParentDropDown = base.GetCurrentParentDropDown();
					if (currentParentDropDown != null && !currentParentDropDown.IsFirstDropDown)
					{
						currentParentDropDown.SelectPreviousToolStrip();
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x00103DDC File Offset: 0x00102DDC
		private ToolStripDropDownDirection RTLTranslateDropDownDirection(ToolStripDropDownDirection dropDownDirection, RightToLeft rightToLeft)
		{
			switch (dropDownDirection)
			{
			case ToolStripDropDownDirection.AboveLeft:
				return ToolStripDropDownDirection.AboveRight;
			case ToolStripDropDownDirection.AboveRight:
				return ToolStripDropDownDirection.AboveLeft;
			case ToolStripDropDownDirection.BelowLeft:
				return ToolStripDropDownDirection.BelowRight;
			case ToolStripDropDownDirection.BelowRight:
				return ToolStripDropDownDirection.BelowLeft;
			case ToolStripDropDownDirection.Left:
				return ToolStripDropDownDirection.Right;
			case ToolStripDropDownDirection.Right:
				return ToolStripDropDownDirection.Left;
			default:
				if (base.IsOnDropDown)
				{
					if (rightToLeft != RightToLeft.Yes)
					{
						return ToolStripDropDownDirection.Right;
					}
					return ToolStripDropDownDirection.Left;
				}
				else
				{
					if (rightToLeft != RightToLeft.Yes)
					{
						return ToolStripDropDownDirection.BelowRight;
					}
					return ToolStripDropDownDirection.BelowLeft;
				}
				break;
			}
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x00103E2E File Offset: 0x00102E2E
		public void ShowDropDown()
		{
			this.ShowDropDown(false);
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x00103E38 File Offset: 0x00102E38
		internal void ShowDropDown(bool mousePush)
		{
			this.ShowDropDownInternal();
			ToolStripDropDownMenu toolStripDropDownMenu = this.dropDown as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				if (!mousePush)
				{
					toolStripDropDownMenu.ResetScrollPosition();
				}
				toolStripDropDownMenu.RestoreScrollPosition();
			}
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x00103E6C File Offset: 0x00102E6C
		private void ShowDropDownInternal()
		{
			/*
An exception occurred when decompiling this method (0600479D)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.ToolStripDropDownItem::ShowDropDownInternal()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 486
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x00103F16 File Offset: 0x00102F16
		private bool ShouldSerializeDropDown()
		{
			return this.dropDown != null && !this.dropDown.IsAutoGenerated;
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x00103F30 File Offset: 0x00102F30
		private bool ShouldSerializeDropDownDirection()
		{
			return this.toolStripDropDownDirection != ToolStripDropDownDirection.Default;
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x00103F3E File Offset: 0x00102F3E
		private bool ShouldSerializeDropDownItems()
		{
			return this.dropDown != null && this.dropDown.IsAutoGenerated;
		}

		// Token: 0x040021E0 RID: 8672
		private ToolStripDropDown dropDown;

		// Token: 0x040021E1 RID: 8673
		private ToolStripDropDownDirection toolStripDropDownDirection = ToolStripDropDownDirection.Default;

		// Token: 0x040021E2 RID: 8674
		private static readonly object EventDropDownShow = new object();

		// Token: 0x040021E3 RID: 8675
		private static readonly object EventDropDownHide = new object();

		// Token: 0x040021E4 RID: 8676
		private static readonly object EventDropDownOpened = new object();

		// Token: 0x040021E5 RID: 8677
		private static readonly object EventDropDownClosed = new object();

		// Token: 0x040021E6 RID: 8678
		private static readonly object EventDropDownItemClicked = new object();
	}
}

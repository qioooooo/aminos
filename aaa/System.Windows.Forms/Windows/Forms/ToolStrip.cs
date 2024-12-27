using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x02000245 RID: 581
	[DefaultEvent("ItemClicked")]
	[SRDescription("DescriptionToolStrip")]
	[ComVisible(true)]
	[DesignerSerializer("System.Windows.Forms.Design.ToolStripCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Items")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ToolStrip : ScrollableControl, IArrangedElement, IComponent, IDisposable, ISupportToolStripPanel
	{
		// Token: 0x06001BBC RID: 7100 RVA: 0x00035F40 File Offset: 0x00034F40
		public ToolStrip()
		{
			base.SuspendLayout();
			this.CanOverflow = true;
			this.TabStop = false;
			this.MenuAutoExpand = false;
			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this.SetToolStripState(192, true);
			base.SetState2(2064, true);
			ToolStripManager.ToolStrips.Add(this);
			this.layoutEngine = new ToolStripSplitStackLayout(this);
			this.Dock = this.DefaultDock;
			this.AutoSize = true;
			this.CausesValidation = false;
			Size defaultSize = this.DefaultSize;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.ShowItemToolTips = this.DefaultShowItemToolTips;
			base.ResumeLayout(true);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x00036056 File Offset: 0x00035056
		public ToolStrip(params ToolStripItem[] items)
			: this()
		{
			this.Items.AddRange(items);
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x0003606A File Offset: 0x0003506A
		internal ArrayList ActiveDropDowns
		{
			get
			{
				return this.activeDropDowns;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x00036072 File Offset: 0x00035072
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x0003607F File Offset: 0x0003507F
		internal virtual bool KeyboardActive
		{
			get
			{
				return this.GetToolStripState(32768);
			}
			set
			{
				this.SetToolStripState(32768, value);
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x0003608D File Offset: 0x0003508D
		// (set) Token: 0x06001BC2 RID: 7106 RVA: 0x00036090 File Offset: 0x00035090
		internal virtual bool AllItemsVisible
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x00036092 File Offset: 0x00035092
		// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x0003609C File Offset: 0x0003509C
		[DefaultValue(true)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				if (this.IsInToolStripPanel && base.AutoSize && !value)
				{
					Rectangle specifiedBounds = CommonProperties.GetSpecifiedBounds(this);
					specifiedBounds.Location = base.Location;
					CommonProperties.UpdateSpecifiedBounds(this, specifiedBounds.X, specifiedBounds.Y, specifiedBounds.Width, specifiedBounds.Height, BoundsSpecified.Location);
				}
				base.AutoSize = value;
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06001BC5 RID: 7109 RVA: 0x000360FA File Offset: 0x000350FA
		// (remove) Token: 0x06001BC6 RID: 7110 RVA: 0x00036103 File Offset: 0x00035103
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0003610C File Offset: 0x0003510C
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x00036114 File Offset: 0x00035114
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ToolStripDoesntSupportAutoScroll"));
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x00036125 File Offset: 0x00035125
		// (set) Token: 0x06001BCA RID: 7114 RVA: 0x0003612D File Offset: 0x0003512D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x00036136 File Offset: 0x00035136
		// (set) Token: 0x06001BCC RID: 7116 RVA: 0x0003613E File Offset: 0x0003513E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x00036147 File Offset: 0x00035147
		// (set) Token: 0x06001BCE RID: 7118 RVA: 0x0003614F File Offset: 0x0003514F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Point AutoScrollPosition
		{
			get
			{
				return base.AutoScrollPosition;
			}
			set
			{
				base.AutoScrollPosition = value;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x00036158 File Offset: 0x00035158
		// (set) Token: 0x06001BD0 RID: 7120 RVA: 0x00036160 File Offset: 0x00035160
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				if (value && this.AllowItemReorder)
				{
					throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
				}
				base.AllowDrop = value;
				if (value)
				{
					this.DropTargetManager.EnsureRegistered(this);
					return;
				}
				this.DropTargetManager.EnsureUnRegistered(this);
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000361A0 File Offset: 0x000351A0
		// (set) Token: 0x06001BD2 RID: 7122 RVA: 0x000361AC File Offset: 0x000351AC
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ToolStripAllowItemReorderDescr")]
		public bool AllowItemReorder
		{
			get
			{
				return this.GetToolStripState(2);
			}
			set
			{
				if (this.GetToolStripState(2) != value)
				{
					if (this.AllowDrop && value)
					{
						throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
					}
					this.SetToolStripState(2, value);
					if (value)
					{
						ToolStripSplitStackDragDropHandler toolStripSplitStackDragDropHandler = new ToolStripSplitStackDragDropHandler(this);
						this.ItemReorderDropSource = toolStripSplitStackDragDropHandler;
						this.ItemReorderDropTarget = toolStripSplitStackDragDropHandler;
						this.DropTargetManager.EnsureRegistered(this);
						return;
					}
					this.DropTargetManager.EnsureUnRegistered(this);
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x00036217 File Offset: 0x00035217
		// (set) Token: 0x06001BD4 RID: 7124 RVA: 0x00036224 File Offset: 0x00035224
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripAllowMergeDescr")]
		[DefaultValue(true)]
		public bool AllowMerge
		{
			get
			{
				return this.GetToolStripState(128);
			}
			set
			{
				if (this.GetToolStripState(128) != value)
				{
					this.SetToolStripState(128, value);
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x00036240 File Offset: 0x00035240
		// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x00036248 File Offset: 0x00035248
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				using (new LayoutTransaction(this, this, PropertyNames.Anchor))
				{
					base.Anchor = value;
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x00036288 File Offset: 0x00035288
		// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x00036290 File Offset: 0x00035290
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripBackColorDescr")]
		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06001BD9 RID: 7129 RVA: 0x00036299 File Offset: 0x00035299
		// (remove) Token: 0x06001BDA RID: 7130 RVA: 0x000362AC File Offset: 0x000352AC
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripOnBeginDrag")]
		public event EventHandler BeginDrag
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventBeginDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventBeginDrag, value);
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x000362C0 File Offset: 0x000352C0
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x00036302 File Offset: 0x00035302
		public override BindingContext BindingContext
		{
			get
			{
				BindingContext bindingContext = (BindingContext)base.Properties.GetObject(ToolStrip.PropBindingContext);
				if (bindingContext != null)
				{
					return bindingContext;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.BindingContext;
				}
				return null;
			}
			set
			{
				if (base.Properties.GetObject(ToolStrip.PropBindingContext) != value)
				{
					base.Properties.SetObject(ToolStrip.PropBindingContext, value);
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00036333 File Offset: 0x00035333
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x0003633C File Offset: 0x0003533C
		[DefaultValue(true)]
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		public bool CanOverflow
		{
			get
			{
				return this.GetToolStripState(1);
			}
			set
			{
				if (this.GetToolStripState(1) != value)
				{
					this.SetToolStripState(1, value);
					this.InvalidateLayout();
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x00036356 File Offset: 0x00035356
		internal bool CanHotTrack
		{
			get
			{
				return this.Focused || !base.ContainsFocus;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0003636B File Offset: 0x0003536B
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x00036373 File Offset: 0x00035373
		[DefaultValue(false)]
		[Browsable(false)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06001BE2 RID: 7138 RVA: 0x0003637C File Offset: 0x0003537C
		// (remove) Token: 0x06001BE3 RID: 7139 RVA: 0x00036385 File Offset: 0x00035385
		[Browsable(false)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0003638E File Offset: 0x0003538E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001BE5 RID: 7141 RVA: 0x00036396 File Offset: 0x00035396
		// (remove) Token: 0x06001BE6 RID: 7142 RVA: 0x0003639F File Offset: 0x0003539F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event ControlEventHandler ControlAdded
		{
			add
			{
				base.ControlAdded += value;
			}
			remove
			{
				base.ControlAdded -= value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x000363A8 File Offset: 0x000353A8
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x000363B0 File Offset: 0x000353B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06001BE9 RID: 7145 RVA: 0x000363B9 File Offset: 0x000353B9
		// (remove) Token: 0x06001BEA RID: 7146 RVA: 0x000363C2 File Offset: 0x000353C2
		[Browsable(false)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06001BEB RID: 7147 RVA: 0x000363CB File Offset: 0x000353CB
		// (remove) Token: 0x06001BEC RID: 7148 RVA: 0x000363D4 File Offset: 0x000353D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				base.ControlRemoved += value;
			}
			remove
			{
				base.ControlRemoved -= value;
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06001BED RID: 7149 RVA: 0x000363DD File Offset: 0x000353DD
		// (remove) Token: 0x06001BEE RID: 7150 RVA: 0x000363F0 File Offset: 0x000353F0
		[SRDescription("ToolStripOnEndDrag")]
		[SRCategory("CatBehavior")]
		public event EventHandler EndDrag
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventEndDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventEndDrag, value);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x00036403 File Offset: 0x00035403
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x0003642D File Offset: 0x0003542D
		public override Font Font
		{
			get
			{
				if (base.IsFontSet())
				{
					return base.Font;
				}
				if (this.defaultFont == null)
				{
					this.defaultFont = ToolStripManager.DefaultFont;
				}
				return this.defaultFont;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x00036436 File Offset: 0x00035436
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 25);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x00036441 File Offset: 0x00035441
		protected override Padding DefaultPadding
		{
			get
			{
				return new Padding(0, 0, 1, 0);
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x0003644C File Offset: 0x0003544C
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001BF4 RID: 7156 RVA: 0x00036453 File Offset: 0x00035453
		protected virtual DockStyle DefaultDock
		{
			get
			{
				return DockStyle.Top;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x00036456 File Offset: 0x00035456
		protected virtual Padding DefaultGripMargin
		{
			get
			{
				if (this.toolStripGrip != null)
				{
					return this.toolStripGrip.DefaultMargin;
				}
				return new Padding(2);
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x00036472 File Offset: 0x00035472
		protected virtual bool DefaultShowItemToolTips
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x00036478 File Offset: 0x00035478
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x00036548 File Offset: 0x00035548
		[SRDescription("ToolStripDefaultDropDownDirectionDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		public virtual ToolStripDropDownDirection DefaultDropDownDirection
		{
			get
			{
				ToolStripDropDownDirection toolStripDropDownDirection = this.toolStripDropDownDirection;
				if (toolStripDropDownDirection == ToolStripDropDownDirection.Default)
				{
					if (this.Orientation == Orientation.Vertical)
					{
						if (this.IsInToolStripPanel)
						{
							DockStyle dockStyle = ((this.ParentInternal != null) ? this.ParentInternal.Dock : DockStyle.Left);
							toolStripDropDownDirection = ((dockStyle == DockStyle.Right) ? ToolStripDropDownDirection.Left : ToolStripDropDownDirection.Right);
							if (base.DesignMode && dockStyle == DockStyle.Left)
							{
								toolStripDropDownDirection = ToolStripDropDownDirection.Right;
							}
						}
						else
						{
							toolStripDropDownDirection = ((this.Dock == DockStyle.Right && this.RightToLeft == RightToLeft.No) ? ToolStripDropDownDirection.Left : ToolStripDropDownDirection.Right);
							if (base.DesignMode && this.Dock == DockStyle.Left)
							{
								toolStripDropDownDirection = ToolStripDropDownDirection.Right;
							}
						}
					}
					else
					{
						DockStyle dockStyle2 = this.Dock;
						if (this.IsInToolStripPanel && this.ParentInternal != null)
						{
							dockStyle2 = this.ParentInternal.Dock;
						}
						if (dockStyle2 == DockStyle.Bottom)
						{
							toolStripDropDownDirection = ((this.RightToLeft == RightToLeft.Yes) ? ToolStripDropDownDirection.AboveLeft : ToolStripDropDownDirection.AboveRight);
						}
						else
						{
							toolStripDropDownDirection = ((this.RightToLeft == RightToLeft.Yes) ? ToolStripDropDownDirection.BelowLeft : ToolStripDropDownDirection.BelowRight);
						}
					}
				}
				return toolStripDropDownDirection;
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
					this.toolStripDropDownDirection = value;
					return;
				default:
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripDropDownDirection));
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x0003659A File Offset: 0x0003559A
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x000365A4 File Offset: 0x000355A4
		[DefaultValue(DockStyle.Top)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value != this.Dock)
				{
					using (new LayoutTransaction(this, this, PropertyNames.Dock))
					{
						using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Dock))
						{
							DefaultLayout.SetDock(this, value);
							this.UpdateLayoutStyle(this.Dock);
						}
					}
					this.OnDockChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x0003662C File Offset: 0x0003562C
		internal virtual NativeWindow DropDownOwnerWindow
		{
			get
			{
				if (this.dropDownOwnerWindow == null)
				{
					this.dropDownOwnerWindow = new NativeWindow();
				}
				if (this.dropDownOwnerWindow.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.ExStyle = 128;
					this.dropDownOwnerWindow.CreateHandle(createParams);
				}
				return this.dropDownOwnerWindow;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001BFC RID: 7164 RVA: 0x00036686 File Offset: 0x00035686
		// (set) Token: 0x06001BFD RID: 7165 RVA: 0x000366A2 File Offset: 0x000356A2
		internal ToolStripDropTargetManager DropTargetManager
		{
			get
			{
				if (this.dropTargetManager == null)
				{
					this.dropTargetManager = new ToolStripDropTargetManager(this);
				}
				return this.dropTargetManager;
			}
			set
			{
				this.dropTargetManager = value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001BFE RID: 7166 RVA: 0x000366AB File Offset: 0x000356AB
		protected internal virtual ToolStripItemCollection DisplayedItems
		{
			get
			{
				if (this.displayedItems == null)
				{
					this.displayedItems = new ToolStripItemCollection(this, false);
				}
				return this.displayedItems;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x000366C8 File Offset: 0x000356C8
		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = base.DisplayRectangle;
				if (this.LayoutEngine is ToolStripSplitStackLayout && this.GripStyle == ToolStripGripStyle.Visible)
				{
					if (this.Orientation == Orientation.Horizontal)
					{
						int num = this.Grip.GripThickness + this.Grip.Margin.Horizontal;
						displayRectangle.Width -= num;
						displayRectangle.X += ((this.RightToLeft == RightToLeft.No) ? num : 0);
					}
					else
					{
						int num2 = this.Grip.GripThickness + this.Grip.Margin.Vertical;
						displayRectangle.Y += num2;
						displayRectangle.Height -= num2;
					}
				}
				return displayRectangle;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x0003678B File Offset: 0x0003578B
		// (set) Token: 0x06001C01 RID: 7169 RVA: 0x00036793 File Offset: 0x00035793
		[Browsable(false)]
		public new Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06001C02 RID: 7170 RVA: 0x0003679C File Offset: 0x0003579C
		// (remove) Token: 0x06001C03 RID: 7171 RVA: 0x000367A5 File Offset: 0x000357A5
		[Browsable(false)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x000367AE File Offset: 0x000357AE
		private bool HasKeyboardInput
		{
			get
			{
				return base.ContainsFocus || (ToolStripManager.ModalMenuFilter.InMenuMode && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this);
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x000367CC File Offset: 0x000357CC
		internal ToolStripGrip Grip
		{
			get
			{
				if (this.toolStripGrip == null)
				{
					this.toolStripGrip = new ToolStripGrip();
					this.toolStripGrip.Overflow = ToolStripItemOverflow.Never;
					this.toolStripGrip.Visible = this.toolStripGripStyle == ToolStripGripStyle.Visible;
					this.toolStripGrip.AutoSize = false;
					this.toolStripGrip.ParentInternal = this;
					this.toolStripGrip.Margin = this.DefaultGripMargin;
				}
				return this.toolStripGrip;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001C06 RID: 7174 RVA: 0x0003683B File Offset: 0x0003583B
		// (set) Token: 0x06001C07 RID: 7175 RVA: 0x00036844 File Offset: 0x00035844
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripGripStyleDescr")]
		[DefaultValue(ToolStripGripStyle.Visible)]
		public ToolStripGripStyle GripStyle
		{
			get
			{
				return this.toolStripGripStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripGripStyle));
				}
				if (this.toolStripGripStyle != value)
				{
					this.toolStripGripStyle = value;
					this.Grip.Visible = this.toolStripGripStyle == ToolStripGripStyle.Visible;
					LayoutTransaction.DoLayout(this, this, PropertyNames.GripStyle);
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x000368A7 File Offset: 0x000358A7
		[Browsable(false)]
		public ToolStripGripDisplayStyle GripDisplayStyle
		{
			get
			{
				if (this.LayoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow)
				{
					return ToolStripGripDisplayStyle.Horizontal;
				}
				return ToolStripGripDisplayStyle.Vertical;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x000368B5 File Offset: 0x000358B5
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x000368C2 File Offset: 0x000358C2
		[SRDescription("ToolStripGripDisplayStyleDescr")]
		[SRCategory("CatLayout")]
		public Padding GripMargin
		{
			get
			{
				return this.Grip.Margin;
			}
			set
			{
				this.Grip.Margin = value;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x000368D0 File Offset: 0x000358D0
		[Browsable(false)]
		public Rectangle GripRectangle
		{
			get
			{
				if (this.GripStyle != ToolStripGripStyle.Visible)
				{
					return Rectangle.Empty;
				}
				return this.Grip.Bounds;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x000368EC File Offset: 0x000358EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool HasChildren
		{
			get
			{
				return base.HasChildren;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000368F4 File Offset: 0x000358F4
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x00036984 File Offset: 0x00035984
		internal bool HasVisibleItems
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
						{
							this.SetToolStripState(4096, true);
							return true;
						}
					}
					this.SetToolStripState(4096, false);
					return false;
				}
				return this.GetToolStripState(4096);
			}
			set
			{
				this.SetToolStripState(4096, value);
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x00036992 File Offset: 0x00035992
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new HScrollProperties HorizontalScroll
		{
			get
			{
				return base.HorizontalScroll;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0003699A File Offset: 0x0003599A
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x000369A2 File Offset: 0x000359A2
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripImageScalingSizeDescr")]
		[DefaultValue(typeof(Size), "16,16")]
		public Size ImageScalingSize
		{
			get
			{
				return this.ImageScalingSizeInternal;
			}
			set
			{
				this.ImageScalingSizeInternal = value;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x000369AB File Offset: 0x000359AB
		// (set) Token: 0x06001C13 RID: 7187 RVA: 0x000369B4 File Offset: 0x000359B4
		internal virtual Size ImageScalingSizeInternal
		{
			get
			{
				return this.imageScalingSize;
			}
			set
			{
				if (this.imageScalingSize != value)
				{
					this.imageScalingSize = value;
					LayoutTransaction.DoLayoutIf(this.Items.Count > 0, this, this, PropertyNames.ImageScalingSize);
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						toolStripItem.OnImageScalingSizeChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x00036A40 File Offset: 0x00035A40
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x00036A48 File Offset: 0x00035A48
		[SRDescription("ToolStripImageListDescr")]
		[Browsable(false)]
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (this.imageList != value)
				{
					EventHandler eventHandler = new EventHandler(this.ImageListRecreateHandle);
					if (this.imageList != null)
					{
						this.imageList.RecreateHandle -= eventHandler;
					}
					this.imageList = value;
					if (value != null)
					{
						value.RecreateHandle += eventHandler;
					}
					foreach (object obj in this.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						toolStripItem.InvalidateImageListImage();
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x00036AE4 File Offset: 0x00035AE4
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x00036AE7 File Offset: 0x00035AE7
		internal bool IsInToolStripPanel
		{
			get
			{
				return this.ToolStripPanelRow != null;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x00036AF5 File Offset: 0x00035AF5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public bool IsCurrentlyDragging
		{
			get
			{
				return this.GetToolStripState(2048);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x00036B02 File Offset: 0x00035B02
		private bool IsLocationChanging
		{
			get
			{
				return this.GetToolStripState(1024);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x00036B0F File Offset: 0x00035B0F
		[SRDescription("ToolStripItemsDescr")]
		[MergableProperty(false)]
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public virtual ToolStripItemCollection Items
		{
			get
			{
				if (this.toolStripItemCollection == null)
				{
					this.toolStripItemCollection = new ToolStripItemCollection(this, true);
				}
				return this.toolStripItemCollection;
			}
		}

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06001C1B RID: 7195 RVA: 0x00036B2C File Offset: 0x00035B2C
		// (remove) Token: 0x06001C1C RID: 7196 RVA: 0x00036B3F File Offset: 0x00035B3F
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemAddedDescr")]
		public event ToolStripItemEventHandler ItemAdded
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemAdded, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemAdded, value);
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06001C1D RID: 7197 RVA: 0x00036B52 File Offset: 0x00035B52
		// (remove) Token: 0x06001C1E RID: 7198 RVA: 0x00036B65 File Offset: 0x00035B65
		[SRCategory("CatAction")]
		[SRDescription("ToolStripItemOnClickDescr")]
		public event ToolStripItemClickedEventHandler ItemClicked
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemClicked, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemClicked, value);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x00036B78 File Offset: 0x00035B78
		private CachedItemHdcInfo ItemHdcInfo
		{
			get
			{
				if (this.cachedItemHdcInfo == null)
				{
					this.cachedItemHdcInfo = new CachedItemHdcInfo();
				}
				return this.cachedItemHdcInfo;
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06001C20 RID: 7200 RVA: 0x00036B93 File Offset: 0x00035B93
		// (remove) Token: 0x06001C21 RID: 7201 RVA: 0x00036BA6 File Offset: 0x00035BA6
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemRemovedDescr")]
		public event ToolStripItemEventHandler ItemRemoved
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventItemRemoved, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventItemRemoved, value);
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x00036BB9 File Offset: 0x00035BB9
		[Browsable(false)]
		public bool IsDropDown
		{
			get
			{
				return this is ToolStripDropDown;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x00036BC4 File Offset: 0x00035BC4
		internal bool IsDisposingItems
		{
			get
			{
				return this.GetToolStripState(4);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00036BCD File Offset: 0x00035BCD
		// (set) Token: 0x06001C25 RID: 7205 RVA: 0x00036BD5 File Offset: 0x00035BD5
		internal IDropTarget ItemReorderDropTarget
		{
			get
			{
				return this.itemReorderDropTarget;
			}
			set
			{
				this.itemReorderDropTarget = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x00036BDE File Offset: 0x00035BDE
		// (set) Token: 0x06001C27 RID: 7207 RVA: 0x00036BE6 File Offset: 0x00035BE6
		internal ISupportOleDropSource ItemReorderDropSource
		{
			get
			{
				return this.itemReorderDropSource;
			}
			set
			{
				this.itemReorderDropSource = value;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00036BEF File Offset: 0x00035BEF
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x00036BF7 File Offset: 0x00035BF7
		internal bool IsSelectionSuspended
		{
			get
			{
				return this.GetToolStripState(16384);
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00036C04 File Offset: 0x00035C04
		internal ToolStripItem LastMouseDownedItem
		{
			get
			{
				if (this.lastMouseDownedItem != null && (this.lastMouseDownedItem.IsDisposed || this.lastMouseDownedItem.ParentInternal != this))
				{
					this.lastMouseDownedItem = null;
				}
				return this.lastMouseDownedItem;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x00036C36 File Offset: 0x00035C36
		// (set) Token: 0x06001C2C RID: 7212 RVA: 0x00036C3E File Offset: 0x00035C3E
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(null)]
		public LayoutSettings LayoutSettings
		{
			get
			{
				return this.layoutSettings;
			}
			set
			{
				this.layoutSettings = value;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001C2D RID: 7213 RVA: 0x00036C48 File Offset: 0x00035C48
		// (set) Token: 0x06001C2E RID: 7214 RVA: 0x00036C80 File Offset: 0x00035C80
		[SRDescription("ToolStripLayoutStyle")]
		[AmbientValue(ToolStripLayoutStyle.StackWithOverflow)]
		[SRCategory("CatLayout")]
		public ToolStripLayoutStyle LayoutStyle
		{
			get
			{
				if (this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
				{
					switch (this.Orientation)
					{
					case Orientation.Horizontal:
						return ToolStripLayoutStyle.HorizontalStackWithOverflow;
					case Orientation.Vertical:
						return ToolStripLayoutStyle.VerticalStackWithOverflow;
					}
				}
				return this.layoutStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripLayoutStyle));
				}
				if (this.layoutStyle != value)
				{
					this.layoutStyle = value;
					switch (value)
					{
					case ToolStripLayoutStyle.Flow:
						if (!(this.layoutEngine is FlowLayout))
						{
							this.layoutEngine = FlowLayout.Instance;
						}
						this.UpdateOrientation(Orientation.Horizontal);
						goto IL_00EC;
					case ToolStripLayoutStyle.Table:
						if (!(this.layoutEngine is TableLayout))
						{
							this.layoutEngine = TableLayout.Instance;
						}
						this.UpdateOrientation(Orientation.Horizontal);
						goto IL_00EC;
					}
					if (value != ToolStripLayoutStyle.StackWithOverflow)
					{
						this.UpdateOrientation((value == ToolStripLayoutStyle.VerticalStackWithOverflow) ? Orientation.Vertical : Orientation.Horizontal);
					}
					else if (this.IsInToolStripPanel)
					{
						this.UpdateLayoutStyle(this.ToolStripPanelRow.Orientation);
					}
					else
					{
						this.UpdateLayoutStyle(this.Dock);
					}
					if (!(this.layoutEngine is ToolStripSplitStackLayout))
					{
						this.layoutEngine = new ToolStripSplitStackLayout(this);
					}
					IL_00EC:
					using (LayoutTransaction.CreateTransactionIf(base.IsHandleCreated, this, this, PropertyNames.LayoutStyle))
					{
						this.LayoutSettings = this.CreateLayoutSettings(this.layoutStyle);
					}
					this.OnLayoutStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06001C2F RID: 7215 RVA: 0x00036DC8 File Offset: 0x00035DC8
		// (remove) Token: 0x06001C30 RID: 7216 RVA: 0x00036DDB File Offset: 0x00035DDB
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLayoutCompleteDescr")]
		public event EventHandler LayoutCompleted
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLayoutCompleted, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLayoutCompleted, value);
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001C31 RID: 7217 RVA: 0x00036DEE File Offset: 0x00035DEE
		// (set) Token: 0x06001C32 RID: 7218 RVA: 0x00036DF6 File Offset: 0x00035DF6
		internal bool LayoutRequired
		{
			get
			{
				return this.layoutRequired;
			}
			set
			{
				this.layoutRequired = value;
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06001C33 RID: 7219 RVA: 0x00036DFF File Offset: 0x00035DFF
		// (remove) Token: 0x06001C34 RID: 7220 RVA: 0x00036E12 File Offset: 0x00035E12
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLayoutStyleChangedDescr")]
		public event EventHandler LayoutStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLayoutStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLayoutStyleChanged, value);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x00036E25 File Offset: 0x00035E25
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return this.layoutEngine;
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06001C36 RID: 7222 RVA: 0x00036E2D File Offset: 0x00035E2D
		// (remove) Token: 0x06001C37 RID: 7223 RVA: 0x00036E40 File Offset: 0x00035E40
		internal event ToolStripLocationCancelEventHandler LocationChanging
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventLocationChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventLocationChanging, value);
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x00036E54 File Offset: 0x00035E54
		protected internal virtual Size MaxItemSize
		{
			get
			{
				return this.DisplayRectangle.Size;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x00036E6F File Offset: 0x00035E6F
		// (set) Token: 0x06001C3A RID: 7226 RVA: 0x00036E9E File Offset: 0x00035E9E
		internal bool MenuAutoExpand
		{
			get
			{
				if (base.DesignMode || !this.GetToolStripState(8))
				{
					return false;
				}
				if (!this.IsDropDown && !ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					this.SetToolStripState(8, false);
					return false;
				}
				return true;
			}
			set
			{
				if (!base.DesignMode)
				{
					this.SetToolStripState(8, value);
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x00036EB0 File Offset: 0x00035EB0
		internal Stack<MergeHistory> MergeHistoryStack
		{
			get
			{
				if (this.mergeHistoryStack == null)
				{
					this.mergeHistoryStack = new Stack<MergeHistory>();
				}
				return this.mergeHistoryStack;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x00036ECB File Offset: 0x00035ECB
		private MouseHoverTimer MouseHoverTimer
		{
			get
			{
				if (this.mouseHoverTimer == null)
				{
					this.mouseHoverTimer = new MouseHoverTimer();
				}
				return this.mouseHoverTimer;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00036EE8 File Offset: 0x00035EE8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public ToolStripOverflowButton OverflowButton
		{
			get
			{
				if (this.toolStripOverflowButton == null)
				{
					this.toolStripOverflowButton = new ToolStripOverflowButton(this);
					this.toolStripOverflowButton.Overflow = ToolStripItemOverflow.Never;
					this.toolStripOverflowButton.ParentInternal = this;
					this.toolStripOverflowButton.Alignment = ToolStripItemAlignment.Right;
					this.toolStripOverflowButton.Size = this.toolStripOverflowButton.GetPreferredSize(this.DisplayRectangle.Size - base.Padding.Size);
				}
				return this.toolStripOverflowButton;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x00036F6A File Offset: 0x00035F6A
		internal ToolStripItemCollection OverflowItems
		{
			get
			{
				if (this.overflowItems == null)
				{
					this.overflowItems = new ToolStripItemCollection(this, false);
				}
				return this.overflowItems;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x00036F87 File Offset: 0x00035F87
		[Browsable(false)]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
		}

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06001C40 RID: 7232 RVA: 0x00036F8F File Offset: 0x00035F8F
		// (remove) Token: 0x06001C41 RID: 7233 RVA: 0x00036FA2 File Offset: 0x00035FA2
		[SRDescription("ToolStripPaintGripDescr")]
		[SRCategory("CatAppearance")]
		public event PaintEventHandler PaintGrip
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventPaintGrip, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventPaintGrip, value);
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x00036FB5 File Offset: 0x00035FB5
		internal ToolStrip.RestoreFocusMessageFilter RestoreFocusFilter
		{
			get
			{
				if (this.restoreFocusFilter == null)
				{
					this.restoreFocusFilter = new ToolStrip.RestoreFocusMessageFilter(this);
				}
				return this.restoreFocusFilter;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x00036FD1 File Offset: 0x00035FD1
		internal ToolStripPanelCell ToolStripPanelCell
		{
			get
			{
				return ((ISupportToolStripPanel)this).ToolStripPanelCell;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x00036FD9 File Offset: 0x00035FD9
		internal ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				return ((ISupportToolStripPanel)this).ToolStripPanelRow;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00036FE4 File Offset: 0x00035FE4
		ToolStripPanelCell ISupportToolStripPanel.ToolStripPanelCell
		{
			get
			{
				ToolStripPanelCell toolStripPanelCell = null;
				if (!this.IsDropDown && !base.IsDisposed)
				{
					if (base.Properties.ContainsObject(ToolStrip.PropToolStripPanelCell))
					{
						toolStripPanelCell = (ToolStripPanelCell)base.Properties.GetObject(ToolStrip.PropToolStripPanelCell);
					}
					else
					{
						toolStripPanelCell = new ToolStripPanelCell(this);
						base.Properties.SetObject(ToolStrip.PropToolStripPanelCell, toolStripPanelCell);
					}
				}
				return toolStripPanelCell;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x00037048 File Offset: 0x00036048
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x0003706C File Offset: 0x0003606C
		ToolStripPanelRow ISupportToolStripPanel.ToolStripPanelRow
		{
			get
			{
				if (this.ToolStripPanelCell == null)
				{
					return null;
				}
				return this.ToolStripPanelCell.ToolStripPanelRow;
			}
			set
			{
				ToolStripPanelRow toolStripPanelRow = this.ToolStripPanelRow;
				if (toolStripPanelRow != value)
				{
					ToolStripPanelCell toolStripPanelCell = this.ToolStripPanelCell;
					if (toolStripPanelCell == null)
					{
						return;
					}
					toolStripPanelCell.ToolStripPanelRow = value;
					if (value != null)
					{
						if (toolStripPanelRow == null || toolStripPanelRow.Orientation != value.Orientation)
						{
							if (this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
							{
								this.UpdateLayoutStyle(value.Orientation);
								return;
							}
							this.UpdateOrientation(value.Orientation);
							return;
						}
					}
					else
					{
						if (toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Contains(this))
						{
							toolStripPanelRow.ControlsInternal.Remove(this);
						}
						this.UpdateLayoutStyle(this.Dock);
					}
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000370F5 File Offset: 0x000360F5
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x00037102 File Offset: 0x00036102
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[SRDescription("ToolStripStretchDescr")]
		public bool Stretch
		{
			get
			{
				return this.GetToolStripState(512);
			}
			set
			{
				if (this.Stretch != value)
				{
					this.SetToolStripState(512, value);
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x0003711C File Offset: 0x0003611C
		// (set) Token: 0x06001C4B RID: 7243 RVA: 0x00037194 File Offset: 0x00036194
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripRenderer Renderer
		{
			get
			{
				if (this.IsDropDown)
				{
					ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
					if ((toolStripDropDown is ToolStripOverflow || toolStripDropDown.IsAutoGenerated) && toolStripDropDown.OwnerToolStrip != null)
					{
						return toolStripDropDown.OwnerToolStrip.Renderer;
					}
				}
				if (this.RenderMode == ToolStripRenderMode.ManagerRenderMode)
				{
					return ToolStripManager.Renderer;
				}
				this.SetToolStripState(64, false);
				if (this.renderer == null)
				{
					this.Renderer = ToolStripManager.CreateRenderer(this.RenderMode);
				}
				return this.renderer;
			}
			set
			{
				if (this.renderer != value)
				{
					this.SetToolStripState(64, value == null);
					this.renderer = value;
					this.currentRendererType = ((this.renderer != null) ? this.renderer.GetType() : typeof(Type));
					this.OnRendererChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06001C4C RID: 7244 RVA: 0x000371ED File Offset: 0x000361ED
		// (remove) Token: 0x06001C4D RID: 7245 RVA: 0x00037200 File Offset: 0x00036200
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStrip.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStrip.EventRendererChanged, value);
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x00037214 File Offset: 0x00036214
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x00037264 File Offset: 0x00036264
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripRenderModeDescr")]
		public ToolStripRenderMode RenderMode
		{
			get
			{
				if (this.GetToolStripState(64))
				{
					return ToolStripRenderMode.ManagerRenderMode;
				}
				if (this.renderer != null && !this.renderer.IsAutoGenerated)
				{
					return ToolStripRenderMode.Custom;
				}
				if (this.currentRendererType == ToolStripManager.ProfessionalRendererType)
				{
					return ToolStripRenderMode.Professional;
				}
				if (this.currentRendererType == ToolStripManager.SystemRendererType)
				{
					return ToolStripRenderMode.System;
				}
				return ToolStripRenderMode.Custom;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripRenderMode));
				}
				if (value == ToolStripRenderMode.Custom)
				{
					throw new NotSupportedException(SR.GetString("ToolStripRenderModeUseRendererPropertyInstead"));
				}
				if (value == ToolStripRenderMode.ManagerRenderMode)
				{
					if (!this.GetToolStripState(64))
					{
						this.SetToolStripState(64, true);
						this.OnRendererChanged(EventArgs.Empty);
						return;
					}
				}
				else
				{
					this.SetToolStripState(64, false);
					this.Renderer = ToolStripManager.CreateRenderer(value);
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x000372E2 File Offset: 0x000362E2
		internal bool ShowKeyboardCuesInternal
		{
			get
			{
				return this.ShowKeyboardCues;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000372EA File Offset: 0x000362EA
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x000372F2 File Offset: 0x000362F2
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[DefaultValue(true)]
		public bool ShowItemToolTips
		{
			get
			{
				return this.showItemToolTips;
			}
			set
			{
				if (this.showItemToolTips != value)
				{
					this.showItemToolTips = value;
					if (!this.showItemToolTips)
					{
						this.UpdateToolTip(null);
					}
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x00037313 File Offset: 0x00036313
		internal Hashtable Shortcuts
		{
			get
			{
				if (this.shortcuts == null)
				{
					this.shortcuts = new Hashtable(1);
				}
				return this.shortcuts;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x0003732F File Offset: 0x0003632F
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x00037337 File Offset: 0x00036337
		[DispId(-516)]
		[SRDescription("ControlTabStopDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x00037340 File Offset: 0x00036340
		internal ToolTip ToolTip
		{
			get
			{
				ToolTip toolTip;
				if (!base.Properties.ContainsObject(ToolStrip.PropToolTip))
				{
					toolTip = new ToolTip();
					base.Properties.SetObject(ToolStrip.PropToolTip, toolTip);
				}
				else
				{
					toolTip = (ToolTip)base.Properties.GetObject(ToolStrip.PropToolTip);
				}
				return toolTip;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x00037390 File Offset: 0x00036390
		// (set) Token: 0x06001C58 RID: 7256 RVA: 0x000373D0 File Offset: 0x000363D0
		[SRDescription("ToolStripTextDirectionDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		public virtual ToolStripTextDirection TextDirection
		{
			get
			{
				ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
				if (base.Properties.ContainsObject(ToolStrip.PropTextDirection))
				{
					toolStripTextDirection = (ToolStripTextDirection)base.Properties.GetObject(ToolStrip.PropTextDirection);
				}
				if (toolStripTextDirection == ToolStripTextDirection.Inherit)
				{
					toolStripTextDirection = ToolStripTextDirection.Horizontal;
				}
				return toolStripTextDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripTextDirection));
				}
				base.Properties.SetObject(ToolStrip.PropTextDirection, value);
				using (new LayoutTransaction(this, this, "TextDirection"))
				{
					for (int i = 0; i < this.Items.Count; i++)
					{
						this.Items[i].OnOwnerTextDirectionChanged();
					}
				}
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x0003746C File Offset: 0x0003646C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new VScrollProperties VerticalScroll
		{
			get
			{
				return base.VerticalScroll;
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00037474 File Offset: 0x00036474
		void ISupportToolStripPanel.BeginDrag()
		{
			this.OnBeginDrag(EventArgs.Empty);
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00037484 File Offset: 0x00036484
		internal virtual void ChangeSelection(ToolStripItem nextItem)
		{
			if (nextItem != null)
			{
				ToolStripControlHost toolStripControlHost = nextItem as ToolStripControlHost;
				if (base.ContainsFocus && !this.Focused)
				{
					this.FocusInternal();
					if (toolStripControlHost == null)
					{
						this.KeyboardActive = true;
					}
				}
				if (toolStripControlHost != null)
				{
					if (this.hwndThatLostFocus == IntPtr.Zero)
					{
						this.SnapFocus(UnsafeNativeMethods.GetFocus());
					}
					toolStripControlHost.Control.Select();
					toolStripControlHost.Control.FocusInternal();
				}
				nextItem.Select();
				ToolStripMenuItem toolStripMenuItem = nextItem as ToolStripMenuItem;
				if (toolStripMenuItem != null && !this.IsDropDown)
				{
					toolStripMenuItem.HandleAutoExpansion();
				}
			}
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00037514 File Offset: 0x00036514
		protected virtual LayoutSettings CreateLayoutSettings(ToolStripLayoutStyle layoutStyle)
		{
			switch (layoutStyle)
			{
			case ToolStripLayoutStyle.Flow:
				return new FlowLayoutSettings(this);
			case ToolStripLayoutStyle.Table:
				return new TableLayoutSettings(this);
			default:
				return null;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00037544 File Offset: 0x00036544
		protected internal virtual ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			if (text == "-")
			{
				return new ToolStripSeparator();
			}
			return new ToolStripButton(text, image, onClick);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00037561 File Offset: 0x00036561
		private void ClearAllSelections()
		{
			this.ClearAllSelectionsExcept(null);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0003756C File Offset: 0x0003656C
		private void ClearAllSelectionsExcept(ToolStripItem item)
		{
			Rectangle rectangle = ((item == null) ? Rectangle.Empty : item.Bounds);
			Region region = null;
			try
			{
				for (int i = 0; i < this.DisplayedItems.Count; i++)
				{
					if (this.DisplayedItems[i] != item)
					{
						if (item != null && this.DisplayedItems[i].Pressed)
						{
							ToolStripDropDownItem toolStripDropDownItem = this.DisplayedItems[i] as ToolStripDropDownItem;
							if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
							{
								toolStripDropDownItem.AutoHide(item);
							}
						}
						bool flag = false;
						if (this.DisplayedItems[i].Selected)
						{
							this.DisplayedItems[i].Unselect();
							flag = true;
						}
						if (flag)
						{
							if (region == null)
							{
								region = new Region(rectangle);
							}
							region.Union(this.DisplayedItems[i].Bounds);
						}
					}
				}
				if (region != null)
				{
					base.Invalidate(region, true);
					base.Update();
				}
				else if (rectangle != Rectangle.Empty)
				{
					base.Invalidate(rectangle, true);
					base.Update();
				}
			}
			finally
			{
				if (region != null)
				{
					region.Dispose();
				}
			}
			if (base.IsHandleCreated && item != null)
			{
				int num = this.DisplayedItems.IndexOf(item);
				base.AccessibilityNotifyClients(AccessibleEvents.Focus, num);
			}
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000376B4 File Offset: 0x000366B4
		internal void ClearInsertionMark()
		{
			if (this.lastInsertionMarkRect != Rectangle.Empty)
			{
				Rectangle rectangle = this.lastInsertionMarkRect;
				this.lastInsertionMarkRect = Rectangle.Empty;
				base.Invalidate(rectangle);
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000376EC File Offset: 0x000366EC
		private void ClearLastMouseDownedItem()
		{
			ToolStripItem toolStripItem = this.lastMouseDownedItem;
			this.lastMouseDownedItem = null;
			if (this.IsSelectionSuspended)
			{
				this.SetToolStripState(16384, false);
				if (toolStripItem != null)
				{
					toolStripItem.Invalidate();
				}
			}
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00037724 File Offset: 0x00036724
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToolStripOverflow overflow = this.GetOverflow();
				try
				{
					base.SuspendLayout();
					if (overflow != null)
					{
						overflow.SuspendLayout();
					}
					this.SetToolStripState(4, true);
					this.lastMouseDownedItem = null;
					this.HookStaticEvents(false);
					ToolStripPanelCell toolStripPanelCell = base.Properties.GetObject(ToolStrip.PropToolStripPanelCell) as ToolStripPanelCell;
					if (toolStripPanelCell != null)
					{
						toolStripPanelCell.Dispose();
					}
					if (this.cachedItemHdcInfo != null)
					{
						this.cachedItemHdcInfo.Dispose();
					}
					if (this.mouseHoverTimer != null)
					{
						this.mouseHoverTimer.Dispose();
					}
					ToolTip toolTip = (ToolTip)base.Properties.GetObject(ToolStrip.PropToolTip);
					if (toolTip != null)
					{
						toolTip.Dispose();
					}
					if (!this.Items.IsReadOnly)
					{
						for (int i = this.Items.Count - 1; i >= 0; i--)
						{
							this.Items[i].Dispose();
						}
						this.Items.Clear();
					}
					if (this.toolStripGrip != null)
					{
						this.toolStripGrip.Dispose();
					}
					if (this.toolStripOverflowButton != null)
					{
						this.toolStripOverflowButton.Dispose();
					}
					if (this.restoreFocusFilter != null)
					{
						Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.restoreFocusFilter);
						this.restoreFocusFilter = null;
					}
					bool flag = false;
					if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this)
					{
						flag = true;
					}
					ToolStripManager.ModalMenuFilter.RemoveActiveToolStrip(this);
					if (flag && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == null)
					{
						ToolStripManager.ModalMenuFilter.ExitMenuMode();
					}
					ToolStripManager.ToolStrips.Remove(this);
				}
				finally
				{
					base.ResumeLayout(false);
					if (overflow != null)
					{
						overflow.ResumeLayout(false);
					}
					this.SetToolStripState(4, false);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000378BC File Offset: 0x000368BC
		internal void DoLayoutIfHandleCreated(ToolStripItemEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				LayoutTransaction.DoLayout(this, e.Item, PropertyNames.Items);
				base.Invalidate();
				if (this.CanOverflow && this.OverflowButton.HasDropDown)
				{
					if (this.DeferOverflowDropDownLayout())
					{
						CommonProperties.xClearPreferredSizeCache(this.OverflowButton.DropDown);
						this.OverflowButton.DropDown.LayoutRequired = true;
						return;
					}
					LayoutTransaction.DoLayout(this.OverflowButton.DropDown, e.Item, PropertyNames.Items);
					this.OverflowButton.DropDown.Invalidate();
					return;
				}
			}
			else
			{
				CommonProperties.xClearPreferredSizeCache(this);
				this.LayoutRequired = true;
				if (this.CanOverflow && this.OverflowButton.HasDropDown)
				{
					this.OverflowButton.DropDown.LayoutRequired = true;
				}
			}
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0003798F File Offset: 0x0003698F
		private bool DeferOverflowDropDownLayout()
		{
			return base.IsLayoutSuspended || !this.OverflowButton.DropDown.Visible || !this.OverflowButton.DropDown.IsHandleCreated;
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000379C0 File Offset: 0x000369C0
		void ISupportToolStripPanel.EndDrag()
		{
			ToolStripPanel.ClearDragFeedback();
			this.OnEndDrag(EventArgs.Empty);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000379D2 File Offset: 0x000369D2
		internal ToolStripOverflow GetOverflow()
		{
			if (this.toolStripOverflowButton != null && this.toolStripOverflowButton.HasDropDown)
			{
				return this.toolStripOverflowButton.DropDown as ToolStripOverflow;
			}
			return null;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000379FB File Offset: 0x000369FB
		internal byte GetMouseId()
		{
			if (this.mouseDownID == 0)
			{
				this.mouseDownID += 1;
			}
			return this.mouseDownID;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00037A1A File Offset: 0x00036A1A
		internal virtual ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction, bool rtlAware)
		{
			if (rtlAware && this.RightToLeft == RightToLeft.Yes)
			{
				if (direction == ArrowDirection.Right)
				{
					direction = ArrowDirection.Left;
				}
				else if (direction == ArrowDirection.Left)
				{
					direction = ArrowDirection.Right;
				}
			}
			return this.GetNextItem(start, direction);
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00037A44 File Offset: 0x00036A44
		public virtual ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction)
		{
			if (!WindowsFormsUtils.EnumValidator.IsValidArrowDirection(direction))
			{
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(ArrowDirection));
			}
			switch (direction)
			{
			case ArrowDirection.Left:
				return this.GetNextItemHorizontal(start, false);
			case ArrowDirection.Up:
				return this.GetNextItemVertical(start, false);
			default:
				switch (direction)
				{
				case ArrowDirection.Right:
					return this.GetNextItemHorizontal(start, true);
				case ArrowDirection.Down:
					return this.GetNextItemVertical(start, true);
				default:
					return null;
				}
				break;
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00037AB8 File Offset: 0x00036AB8
		private ToolStripItem GetNextItemHorizontal(ToolStripItem start, bool forward)
		{
			if (this.DisplayedItems.Count <= 0)
			{
				return null;
			}
			if (start == null)
			{
				start = (forward ? this.DisplayedItems[this.DisplayedItems.Count - 1] : this.DisplayedItems[0]);
			}
			int num = this.DisplayedItems.IndexOf(start);
			if (num == -1)
			{
				return null;
			}
			int count = this.DisplayedItems.Count;
			for (;;)
			{
				if (forward)
				{
					num = (num + 1) % count;
				}
				else
				{
					num = ((--num < 0) ? (count + num) : num);
				}
				ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
				if (toolStripDropDown != null && toolStripDropDown.OwnerItem != null && toolStripDropDown.OwnerItem.IsInDesignMode)
				{
					break;
				}
				if (this.DisplayedItems[num].CanKeyboardSelect)
				{
					goto Block_10;
				}
				if (this.DisplayedItems[num] == start)
				{
					goto Block_11;
				}
			}
			return this.DisplayedItems[num];
			Block_10:
			return this.DisplayedItems[num];
			Block_11:
			return null;
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00037B9C File Offset: 0x00036B9C
		private ToolStripItem GetNextItemVertical(ToolStripItem selectedItem, bool down)
		{
			ToolStripItem toolStripItem = null;
			ToolStripItem toolStripItem2 = null;
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			double num3 = double.MaxValue;
			if (selectedItem == null)
			{
				return this.GetNextItemHorizontal(selectedItem, down);
			}
			ToolStripDropDown toolStripDropDown = this as ToolStripDropDown;
			if (toolStripDropDown != null && toolStripDropDown.OwnerItem != null && (toolStripDropDown.OwnerItem.IsInDesignMode || (toolStripDropDown.OwnerItem.Owner != null && toolStripDropDown.OwnerItem.Owner.IsInDesignMode)))
			{
				return this.GetNextItemHorizontal(selectedItem, down);
			}
			Point point = new Point(selectedItem.Bounds.X + selectedItem.Width / 2, selectedItem.Bounds.Y + selectedItem.Height / 2);
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem3 = this.DisplayedItems[i];
				if (toolStripItem3 != selectedItem && toolStripItem3.CanKeyboardSelect && (down || toolStripItem3.Bounds.Bottom <= selectedItem.Bounds.Top) && (!down || toolStripItem3.Bounds.Top >= selectedItem.Bounds.Bottom))
				{
					Point point2 = new Point(toolStripItem3.Bounds.X + toolStripItem3.Width / 2, down ? toolStripItem3.Bounds.Top : toolStripItem3.Bounds.Bottom);
					int num4 = point2.X - point.X;
					int num5 = point2.Y - point.Y;
					double num6 = Math.Sqrt((double)(num5 * num5 + num4 * num4));
					if (num5 != 0)
					{
						double num7 = Math.Abs(Math.Atan((double)(num4 / num5)));
						num2 = Math.Min(num2, num7);
						num = Math.Min(num, num6);
						if (num2 == num7 && num2 != double.NaN)
						{
							toolStripItem = toolStripItem3;
						}
						if (num == num6)
						{
							toolStripItem2 = toolStripItem3;
							num3 = num7;
						}
					}
				}
			}
			if (toolStripItem == null || toolStripItem2 == null)
			{
				return this.GetNextItemHorizontal(null, down);
			}
			if (num3 == num2)
			{
				return toolStripItem2;
			}
			if ((!down && toolStripItem.Bounds.Bottom <= toolStripItem2.Bounds.Top) || (down && toolStripItem.Bounds.Top > toolStripItem2.Bounds.Bottom))
			{
				return toolStripItem2;
			}
			return toolStripItem;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00037E18 File Offset: 0x00036E18
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = int.MaxValue;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = int.MaxValue;
			}
			Padding padding = base.Padding;
			Size preferredSize = this.LayoutEngine.GetPreferredSize(this, proposedSize - padding.Size);
			Padding padding2 = base.Padding;
			if (padding != padding2)
			{
				CommonProperties.xClearPreferredSizeCache(this);
			}
			return preferredSize + padding2.Size;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00037E98 File Offset: 0x00036E98
		internal static Size GetPreferredSizeHorizontal(IArrangedElement container, Size proposedConstraints)
		{
			Size size = Size.Empty;
			ToolStrip toolStrip = container as ToolStrip;
			Size size2 = toolStrip.DefaultSize - toolStrip.Padding.Size;
			size.Height = Math.Max(0, size2.Height);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					flag2 = true;
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						Padding margin = toolStripItem.Margin;
						Size preferredItemSize = ToolStrip.GetPreferredItemSize(toolStripItem);
						size.Width += margin.Horizontal + preferredItemSize.Width;
						size.Height = Math.Max(size.Height, margin.Vertical + preferredItemSize.Height);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (toolStrip.Items.Count == 0 || !flag2)
			{
				size = size2;
			}
			if (flag)
			{
				ToolStripOverflowButton overflowButton = toolStrip.OverflowButton;
				Padding margin2 = overflowButton.Margin;
				size.Width += margin2.Horizontal + overflowButton.Bounds.Width;
			}
			else
			{
				size.Width += 2;
			}
			if (toolStrip.GripStyle == ToolStripGripStyle.Visible)
			{
				Padding gripMargin = toolStrip.GripMargin;
				size.Width += gripMargin.Horizontal + toolStrip.Grip.GripThickness;
			}
			size = LayoutUtils.IntersectSizes(size, proposedConstraints);
			return size;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0003801C File Offset: 0x0003701C
		internal static Size GetPreferredSizeVertical(IArrangedElement container, Size proposedConstraints)
		{
			Size size = Size.Empty;
			bool flag = false;
			ToolStrip toolStrip = container as ToolStrip;
			bool flag2 = false;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					flag2 = true;
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						Size preferredItemSize = ToolStrip.GetPreferredItemSize(toolStripItem);
						Padding margin = toolStripItem.Margin;
						size.Height += margin.Vertical + preferredItemSize.Height;
						size.Width = Math.Max(size.Width, margin.Horizontal + preferredItemSize.Width);
					}
					else
					{
						flag = true;
					}
				}
			}
			if (toolStrip.Items.Count == 0 || !flag2)
			{
				size = LayoutUtils.FlipSize(toolStrip.DefaultSize);
			}
			if (flag)
			{
				ToolStripOverflowButton overflowButton = toolStrip.OverflowButton;
				Padding margin2 = overflowButton.Margin;
				size.Height += margin2.Vertical + overflowButton.Bounds.Height;
			}
			else
			{
				size.Height += 2;
			}
			if (toolStrip.GripStyle == ToolStripGripStyle.Visible)
			{
				Padding gripMargin = toolStrip.GripMargin;
				size.Height += gripMargin.Vertical + toolStrip.Grip.GripThickness;
			}
			if (toolStrip.Size != size)
			{
				CommonProperties.xClearPreferredSizeCache(toolStrip);
			}
			return size;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x0003817E File Offset: 0x0003717E
		private static Size GetPreferredItemSize(ToolStripItem item)
		{
			if (!item.AutoSize)
			{
				return item.Size;
			}
			return item.GetPreferredSize(Size.Empty);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x0003819A File Offset: 0x0003719A
		internal static Graphics GetMeasurementGraphics()
		{
			return WindowsFormsUtils.CreateMeasurementGraphics();
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000381A4 File Offset: 0x000371A4
		internal ToolStripItem GetSelectedItem()
		{
			ToolStripItem toolStripItem = null;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				if (this.DisplayedItems[i].Selected)
				{
					toolStripItem = this.DisplayedItems[i];
				}
			}
			return toolStripItem;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000381EA File Offset: 0x000371EA
		internal bool GetToolStripState(int flag)
		{
			return (this.toolStripState & flag) != 0;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000381FA File Offset: 0x000371FA
		internal virtual ToolStrip GetToplevelOwnerToolStrip()
		{
			return this;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000381FD File Offset: 0x000371FD
		internal virtual Control GetOwnerControl()
		{
			return this;
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00038200 File Offset: 0x00037200
		private void HandleMouseLeave()
		{
			if (this.lastMouseActiveItem != null)
			{
				if (!base.DesignMode)
				{
					this.MouseHoverTimer.Cancel(this.lastMouseActiveItem);
				}
				try
				{
					this.lastMouseActiveItem.FireEvent(EventArgs.Empty, ToolStripItemEventType.MouseLeave);
				}
				finally
				{
					this.lastMouseActiveItem = null;
				}
			}
			ToolStripMenuItem.MenuTimer.HandleToolStripMouseLeave(this);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00038264 File Offset: 0x00037264
		internal void HandleItemClick(ToolStripItem dismissingItem)
		{
			ToolStripItemClickedEventArgs toolStripItemClickedEventArgs = new ToolStripItemClickedEventArgs(dismissingItem);
			this.OnItemClicked(toolStripItemClickedEventArgs);
			if (!this.IsDropDown && dismissingItem.IsOnOverflow)
			{
				this.OverflowButton.DropDown.HandleItemClick(dismissingItem);
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000382A0 File Offset: 0x000372A0
		internal virtual void HandleItemClicked(ToolStripItem dismissingItem)
		{
			ToolStripDropDownItem toolStripDropDownItem = dismissingItem as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && !toolStripDropDownItem.HasDropDownItems)
			{
				this.KeyboardActive = false;
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000382C8 File Offset: 0x000372C8
		private void HookStaticEvents(bool hook)
		{
			if (hook)
			{
				if (this.alreadyHooked)
				{
					return;
				}
				try
				{
					ToolStripManager.RendererChanged += this.OnDefaultRendererChanged;
					SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
					return;
				}
				finally
				{
					this.alreadyHooked = true;
				}
			}
			if (this.alreadyHooked)
			{
				try
				{
					ToolStripManager.RendererChanged -= this.OnDefaultRendererChanged;
					SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
				}
				finally
				{
					this.alreadyHooked = false;
				}
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0003835C File Offset: 0x0003735C
		private void InitializeRenderer(ToolStripRenderer renderer)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this, this, PropertyNames.Renderer))
			{
				renderer.Initialize(this);
				for (int i = 0; i < this.Items.Count; i++)
				{
					renderer.InitializeItem(this.Items[i]);
				}
			}
			base.Invalidate(this.Controls.Count > 0);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000383DC File Offset: 0x000373DC
		private void InvalidateLayout()
		{
			if (base.IsHandleCreated)
			{
				LayoutTransaction.DoLayout(this, this, null);
			}
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000383F0 File Offset: 0x000373F0
		internal void InvalidateTextItems()
		{
			using (new LayoutTransaction(this, this, "ShowKeyboardFocusCues", base.Visible))
			{
				for (int i = 0; i < this.DisplayedItems.Count; i++)
				{
					if ((this.DisplayedItems[i].DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						this.DisplayedItems[i].InvalidateItemLayout("ShowKeyboardFocusCues");
					}
				}
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00038470 File Offset: 0x00037470
		protected override bool IsInputKey(Keys keyData)
		{
			ToolStripItem selectedItem = this.GetSelectedItem();
			return (selectedItem != null && selectedItem.IsInputKey(keyData)) || base.IsInputKey(keyData);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0003849C File Offset: 0x0003749C
		protected override bool IsInputChar(char charCode)
		{
			ToolStripItem selectedItem = this.GetSelectedItem();
			return (selectedItem != null && selectedItem.IsInputChar(charCode)) || base.IsInputChar(charCode);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000384C8 File Offset: 0x000374C8
		private static bool IsPseudoMnemonic(char charCode, string text)
		{
			if (!string.IsNullOrEmpty(text) && !WindowsFormsUtils.ContainsMnemonic(text))
			{
				char c = char.ToUpper(charCode, CultureInfo.CurrentCulture);
				char c2 = char.ToUpper(text[0], CultureInfo.CurrentCulture);
				if (c2 == c || char.ToLower(charCode, CultureInfo.CurrentCulture) == char.ToLower(text[0], CultureInfo.CurrentCulture))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00038528 File Offset: 0x00037528
		internal void InvokePaintItem(ToolStripItem item)
		{
			base.Invalidate(item.Bounds);
			base.Update();
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x0003853C File Offset: 0x0003753C
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			base.Invalidate();
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00038544 File Offset: 0x00037544
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Point location = base.Location;
			if (!this.IsCurrentlyDragging && !this.IsLocationChanging && this.IsInToolStripPanel)
			{
				ToolStripLocationCancelEventArgs toolStripLocationCancelEventArgs = new ToolStripLocationCancelEventArgs(new Point(x, y), false);
				try
				{
					if (location.X != x || location.Y != y)
					{
						this.SetToolStripState(1024, true);
						this.OnLocationChanging(toolStripLocationCancelEventArgs);
					}
					if (!toolStripLocationCancelEventArgs.Cancel)
					{
						base.SetBoundsCore(x, y, width, height, specified);
					}
					return;
				}
				finally
				{
					this.SetToolStripState(1024, false);
				}
			}
			if (this.IsCurrentlyDragging)
			{
				Region transparentRegion = this.Renderer.GetTransparentRegion(this);
				if (transparentRegion != null)
				{
					if (location.X == x)
					{
						if (location.Y == y)
						{
							goto IL_00BA;
						}
					}
					try
					{
						base.Invalidate(transparentRegion);
						base.Update();
					}
					finally
					{
						transparentRegion.Dispose();
					}
				}
			}
			IL_00BA:
			this.SetToolStripState(1024, false);
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00038640 File Offset: 0x00037640
		internal void PaintParentRegion(Graphics g, Region region)
		{
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00038642 File Offset: 0x00037642
		internal bool ProcessCmdKeyInternal(ref Message m, Keys keyData)
		{
			return this.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x0003864C File Offset: 0x0003764C
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				IntPtr hdc = graphics.GetHdc();
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 791, hdc, (IntPtr)30);
				IntPtr handle = hDC.Handle;
				SafeNativeMethods.BitBlt(new HandleRef(this, handle), bounds.X, bounds.Y, bounds.Width, bounds.Height, new HandleRef(graphics, hdc), 0, 0, 13369376);
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000386FC File Offset: 0x000376FC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.IsMenuKey(keyData) && !this.IsDropDown && ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.ClearAllSelections();
				ToolStripManager.ModalMenuFilter.MenuKeyToggle = true;
				ToolStripManager.ModalMenuFilter.ExitMenuMode();
			}
			ToolStripItem selectedItem = this.GetSelectedItem();
			if (selectedItem != null && selectedItem.ProcessCmdKey(ref m, keyData))
			{
				return true;
			}
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (toolStripItem != selectedItem && toolStripItem.ProcessCmdKey(ref m, keyData))
				{
					return true;
				}
			}
			if (!this.IsDropDown)
			{
				bool flag = (keyData & (Keys.LButton | Keys.Back | Keys.Control)) == (Keys.LButton | Keys.Back | Keys.Control);
				if (flag && !this.TabStop && this.HasKeyboardInput)
				{
					bool flag2;
					if ((keyData & Keys.Shift) == Keys.None)
					{
						flag2 = ToolStripManager.SelectNextToolStrip(this, true);
					}
					else
					{
						flag2 = ToolStripManager.SelectNextToolStrip(this, false);
					}
					if (flag2)
					{
						return true;
					}
				}
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00038800 File Offset: 0x00037800
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			bool flag = false;
			ToolStripItem selectedItem = this.GetSelectedItem();
			if (selectedItem != null && selectedItem.ProcessDialogKey(keyData))
			{
				return true;
			}
			bool flag2 = (keyData & (Keys.Control | Keys.Alt)) != Keys.None;
			Keys keys = keyData & Keys.KeyCode;
			Keys keys2 = keys;
			switch (keys2)
			{
			case Keys.Back:
				if (!base.ContainsFocus)
				{
					flag = this.ProcessTabKey(false);
				}
				break;
			case Keys.Tab:
				if (!flag2)
				{
					flag = this.ProcessTabKey((keyData & Keys.Shift) == Keys.None);
				}
				break;
			default:
				if (keys2 != Keys.Escape)
				{
					switch (keys2)
					{
					case Keys.End:
						this.SelectNextToolStripItem(null, false);
						flag = true;
						break;
					case Keys.Home:
						this.SelectNextToolStripItem(null, true);
						flag = true;
						break;
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						flag = this.ProcessArrowKey(keys);
						break;
					}
				}
				else if (!flag2 && !this.TabStop)
				{
					this.RestoreFocusInternal();
					flag = true;
				}
				break;
			}
			if (flag)
			{
				return flag;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000388E3 File Offset: 0x000378E3
		internal virtual void ProcessDuplicateMnemonic(ToolStripItem item, char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return;
			}
			if (item != null)
			{
				this.SetFocusUnsafe();
				item.Select();
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00038900 File Offset: 0x00037900
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			if (this.Focused || base.ContainsFocus)
			{
				return this.ProcessMnemonicInternal(charCode);
			}
			bool inMenuMode = ToolStripManager.ModalMenuFilter.InMenuMode;
			if (!inMenuMode && Control.ModifierKeys == Keys.Alt)
			{
				return this.ProcessMnemonicInternal(charCode);
			}
			return inMenuMode && ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == this && this.ProcessMnemonicInternal(charCode);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00038960 File Offset: 0x00037960
		private bool ProcessMnemonicInternal(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			ToolStripItem selectedItem = this.GetSelectedItem();
			int num = 0;
			if (selectedItem != null)
			{
				num = this.DisplayedItems.IndexOf(selectedItem);
			}
			num = Math.Max(0, num);
			ToolStripItem toolStripItem = null;
			bool flag = false;
			int num2 = num;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem2 = this.DisplayedItems[num2];
				num2 = (num2 + 1) % this.DisplayedItems.Count;
				if (!string.IsNullOrEmpty(toolStripItem2.Text) && toolStripItem2.Enabled && (toolStripItem2.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					flag = flag || toolStripItem2 is ToolStripMenuItem;
					if (Control.IsMnemonic(charCode, toolStripItem2.Text))
					{
						if (toolStripItem != null)
						{
							if (toolStripItem == selectedItem)
							{
								this.ProcessDuplicateMnemonic(toolStripItem2, charCode);
							}
							else
							{
								this.ProcessDuplicateMnemonic(toolStripItem, charCode);
							}
							return true;
						}
						toolStripItem = toolStripItem2;
					}
				}
			}
			if (toolStripItem != null)
			{
				return toolStripItem.ProcessMnemonic(charCode);
			}
			if (!flag)
			{
				return false;
			}
			num2 = num;
			for (int j = 0; j < this.DisplayedItems.Count; j++)
			{
				ToolStripItem toolStripItem3 = this.DisplayedItems[num2];
				num2 = (num2 + 1) % this.DisplayedItems.Count;
				if (toolStripItem3 is ToolStripMenuItem && !string.IsNullOrEmpty(toolStripItem3.Text) && toolStripItem3.Enabled && (toolStripItem3.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text && ToolStrip.IsPseudoMnemonic(charCode, toolStripItem3.Text))
				{
					if (toolStripItem != null)
					{
						if (toolStripItem == selectedItem)
						{
							this.ProcessDuplicateMnemonic(toolStripItem3, charCode);
						}
						else
						{
							this.ProcessDuplicateMnemonic(toolStripItem, charCode);
						}
						return true;
					}
					toolStripItem = toolStripItem3;
				}
			}
			return toolStripItem != null && toolStripItem.ProcessMnemonic(charCode);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00038B00 File Offset: 0x00037B00
		private bool ProcessTabKey(bool forward)
		{
			if (this.TabStop)
			{
				return false;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				forward = !forward;
			}
			this.SelectNextToolStripItem(this.GetSelectedItem(), forward);
			return true;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00038B2C File Offset: 0x00037B2C
		internal virtual bool ProcessArrowKey(Keys keyCode)
		{
			bool flag = false;
			ToolStripMenuItem.MenuTimer.Cancel();
			switch (keyCode)
			{
			case Keys.Left:
			case Keys.Right:
				flag = this.ProcessLeftRightArrowKey(keyCode == Keys.Right);
				break;
			case Keys.Up:
			case Keys.Down:
				if (this.IsDropDown || this.Orientation != Orientation.Horizontal)
				{
					ToolStripItem selectedItem = this.GetSelectedItem();
					if (keyCode == Keys.Down)
					{
						ToolStripItem nextItem = this.GetNextItem(selectedItem, ArrowDirection.Down);
						if (nextItem != null)
						{
							this.ChangeSelection(nextItem);
							flag = true;
						}
					}
					else
					{
						ToolStripItem nextItem2 = this.GetNextItem(selectedItem, ArrowDirection.Up);
						if (nextItem2 != null)
						{
							this.ChangeSelection(nextItem2);
							flag = true;
						}
					}
				}
				break;
			}
			return flag;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00038BBC File Offset: 0x00037BBC
		private bool ProcessLeftRightArrowKey(bool right)
		{
			this.GetSelectedItem();
			this.SelectNextToolStripItem(this.GetSelectedItem(), right);
			return true;
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00038BD4 File Offset: 0x00037BD4
		internal void NotifySelectionChange(ToolStripItem item)
		{
			if (item == null)
			{
				this.ClearAllSelections();
				return;
			}
			if (item.Selected)
			{
				this.ClearAllSelectionsExcept(item);
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00038BEF File Offset: 0x00037BEF
		private void OnDefaultRendererChanged(object sender, EventArgs e)
		{
			if (this.GetToolStripState(64))
			{
				this.OnRendererChanged(e);
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00038C04 File Offset: 0x00037C04
		protected virtual void OnBeginDrag(EventArgs e)
		{
			this.SetToolStripState(2048, true);
			this.ClearAllSelections();
			this.UpdateToolTip(null);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventBeginDrag];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00038C4C File Offset: 0x00037C4C
		protected virtual void OnEndDrag(EventArgs e)
		{
			this.SetToolStripState(2048, false);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventEndDrag];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00038C86 File Offset: 0x00037C86
		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00038C90 File Offset: 0x00037C90
		protected virtual void OnRendererChanged(EventArgs e)
		{
			this.InitializeRenderer(this.Renderer);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00038CCC File Offset: 0x00037CCC
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i] != null && this.Items[i].ParentInternal == this)
				{
					this.Items[i].OnParentEnabledChanged(e);
				}
			}
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00038D2A File Offset: 0x00037D2A
		internal void OnDefaultFontChanged()
		{
			this.defaultFont = null;
			if (!base.IsFontSet())
			{
				this.OnFontChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x00038D48 File Offset: 0x00037D48
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].OnOwnerFontChanged(e);
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x00038D84 File Offset: 0x00037D84
		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x00038D8D File Offset: 0x00037D8D
		protected override void OnHandleCreated(EventArgs e)
		{
			if ((this.AllowDrop || this.AllowItemReorder) && this.DropTargetManager != null)
			{
				this.DropTargetManager.EnsureRegistered(this);
			}
			base.OnHandleCreated(e);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00038DBA File Offset: 0x00037DBA
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (this.DropTargetManager != null)
			{
				this.DropTargetManager.EnsureUnRegistered(this);
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x00038DD8 File Offset: 0x00037DD8
		protected internal virtual void OnItemAdded(ToolStripItemEventArgs e)
		{
			this.DoLayoutIfHandleCreated(e);
			if (!this.HasVisibleItems && e.Item != null && ((IArrangedElement)e.Item).ParticipatesInLayout)
			{
				this.HasVisibleItems = true;
			}
			ToolStripItemEventHandler toolStripItemEventHandler = (ToolStripItemEventHandler)base.Events[ToolStrip.EventItemAdded];
			if (toolStripItemEventHandler != null)
			{
				toolStripItemEventHandler(this, e);
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x00038E34 File Offset: 0x00037E34
		protected virtual void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			ToolStripItemClickedEventHandler toolStripItemClickedEventHandler = (ToolStripItemClickedEventHandler)base.Events[ToolStrip.EventItemClicked];
			if (toolStripItemClickedEventHandler != null)
			{
				toolStripItemClickedEventHandler(this, e);
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x00038E64 File Offset: 0x00037E64
		protected internal virtual void OnItemRemoved(ToolStripItemEventArgs e)
		{
			this.OnItemVisibleChanged(e, true);
			ToolStripItemEventHandler toolStripItemEventHandler = (ToolStripItemEventHandler)base.Events[ToolStrip.EventItemRemoved];
			if (toolStripItemEventHandler != null)
			{
				toolStripItemEventHandler(this, e);
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x00038E9C File Offset: 0x00037E9C
		internal void OnItemVisibleChanged(ToolStripItemEventArgs e, bool performLayout)
		{
			if (e.Item == this.lastMouseActiveItem)
			{
				this.lastMouseActiveItem = null;
			}
			if (e.Item == this.LastMouseDownedItem)
			{
				this.lastMouseDownedItem = null;
			}
			if (e.Item == this.currentlyActiveTooltipItem)
			{
				this.UpdateToolTip(null);
			}
			if (performLayout)
			{
				this.DoLayoutIfHandleCreated(e);
			}
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x00038EF4 File Offset: 0x00037EF4
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.LayoutRequired = false;
			ToolStripOverflow overflow = this.GetOverflow();
			if (overflow != null)
			{
				overflow.SuspendLayout();
				this.toolStripOverflowButton.Size = this.toolStripOverflowButton.GetPreferredSize(this.DisplayRectangle.Size - base.Padding.Size);
			}
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].OnLayout(e);
			}
			base.OnLayout(e);
			this.SetDisplayedItems();
			this.OnLayoutCompleted(EventArgs.Empty);
			base.Invalidate();
			if (overflow != null)
			{
				overflow.ResumeLayout();
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x00038FA0 File Offset: 0x00037FA0
		protected virtual void OnLayoutCompleted(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventLayoutCompleted];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00038FD0 File Offset: 0x00037FD0
		protected virtual void OnLayoutStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStrip.EventLayoutStyleChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00038FFE File Offset: 0x00037FFE
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.ClearAllSelections();
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0003900D File Offset: 0x0003800D
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if (!this.IsDropDown)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.RestoreFocusFilter);
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00039030 File Offset: 0x00038030
		internal virtual void OnLocationChanging(ToolStripLocationCancelEventArgs e)
		{
			ToolStripLocationCancelEventHandler toolStripLocationCancelEventHandler = (ToolStripLocationCancelEventHandler)base.Events[ToolStrip.EventLocationChanging];
			if (toolStripLocationCancelEventHandler != null)
			{
				toolStripLocationCancelEventHandler(this, e);
			}
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x00039060 File Offset: 0x00038060
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			this.mouseDownID += 1;
			ToolStripItem itemAt = this.GetItemAt(mea.X, mea.Y);
			if (itemAt != null)
			{
				if (!this.IsDropDown && !(itemAt is ToolStripDropDownItem))
				{
					this.SetToolStripState(16384, true);
					base.CaptureInternal = true;
				}
				this.MenuAutoExpand = true;
				if (mea != null)
				{
					Point point = itemAt.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
					mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				}
				this.lastMouseDownedItem = itemAt;
				itemAt.FireEvent(mea, ToolStripItemEventType.MouseDown);
				return;
			}
			base.OnMouseDown(mea);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00039118 File Offset: 0x00038118
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			ToolStripItem toolStripItem = this.GetItemAt(mea.X, mea.Y);
			if (!this.Grip.MovingToolStrip)
			{
				if (toolStripItem != this.lastMouseActiveItem)
				{
					this.HandleMouseLeave();
					this.lastMouseActiveItem = ((toolStripItem is ToolStripControlHost) ? null : toolStripItem);
					if (this.lastMouseActiveItem != null)
					{
						toolStripItem.FireEvent(new EventArgs(), ToolStripItemEventType.MouseEnter);
					}
					if (!base.DesignMode)
					{
						this.MouseHoverTimer.Start(this.lastMouseActiveItem);
					}
				}
			}
			else
			{
				toolStripItem = this.Grip;
			}
			if (toolStripItem != null)
			{
				Point point = toolStripItem.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
				mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				toolStripItem.FireEvent(mea, ToolStripItemEventType.MouseMove);
				return;
			}
			base.OnMouseMove(mea);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000391F0 File Offset: 0x000381F0
		protected override void OnMouseLeave(EventArgs e)
		{
			this.HandleMouseLeave();
			base.OnMouseLeave(e);
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000391FF File Offset: 0x000381FF
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			if (!this.GetToolStripState(8192))
			{
				this.Grip.MovingToolStrip = false;
			}
			this.ClearLastMouseDownedItem();
			base.OnMouseCaptureChanged(e);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00039228 File Offset: 0x00038228
		protected override void OnMouseUp(MouseEventArgs mea)
		{
			ToolStripItem toolStripItem = (this.Grip.MovingToolStrip ? this.Grip : this.GetItemAt(mea.X, mea.Y));
			if (toolStripItem != null)
			{
				if (mea != null)
				{
					Point point = toolStripItem.TranslatePoint(new Point(mea.X, mea.Y), ToolStripPointType.ToolStripCoords, ToolStripPointType.ToolStripItemCoords);
					mea = new MouseEventArgs(mea.Button, mea.Clicks, point.X, point.Y, mea.Delta);
				}
				toolStripItem.FireEvent(mea, ToolStripItemEventType.MouseUp);
			}
			else
			{
				base.OnMouseUp(mea);
			}
			this.ClearLastMouseDownedItem();
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000392BC File Offset: 0x000382BC
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			Size size = this.largestDisplayedItemSize;
			bool flag = false;
			Rectangle displayRectangle = this.DisplayRectangle;
			using (Region transparentRegion = this.Renderer.GetTransparentRegion(this))
			{
				if (!LayoutUtils.IsZeroWidthOrHeight(size))
				{
					if (transparentRegion != null)
					{
						transparentRegion.Intersect(graphics.Clip);
						graphics.ExcludeClip(transparentRegion);
						flag = true;
					}
					using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping))
					{
						HandleRef handleRef = new HandleRef(this, windowsGraphics.GetHdc());
						HandleRef handleRef2 = this.ItemHdcInfo.GetCachedItemDC(handleRef, size);
						Graphics graphics2 = Graphics.FromHdcInternal(handleRef2.Handle);
						try
						{
							for (int i = 0; i < this.DisplayedItems.Count; i++)
							{
								ToolStripItem toolStripItem = this.DisplayedItems[i];
								if (toolStripItem != null)
								{
									Rectangle clipRectangle = e.ClipRectangle;
									Rectangle bounds = toolStripItem.Bounds;
									if (!this.IsDropDown && toolStripItem.Owner == this)
									{
										clipRectangle.Intersect(displayRectangle);
									}
									clipRectangle.Intersect(bounds);
									if (!LayoutUtils.IsZeroWidthOrHeight(clipRectangle))
									{
										Size size2 = toolStripItem.Size;
										if (!LayoutUtils.AreWidthAndHeightLarger(size, size2))
										{
											this.largestDisplayedItemSize = size2;
											size = size2;
											graphics2.Dispose();
											handleRef2 = this.ItemHdcInfo.GetCachedItemDC(handleRef, size);
											graphics2 = Graphics.FromHdcInternal(handleRef2.Handle);
										}
										clipRectangle.Offset(-bounds.X, -bounds.Y);
										SafeNativeMethods.BitBlt(handleRef2, 0, 0, toolStripItem.Size.Width, toolStripItem.Size.Height, handleRef, toolStripItem.Bounds.X, toolStripItem.Bounds.Y, 13369376);
										using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics2, clipRectangle))
										{
											toolStripItem.FireEvent(paintEventArgs, ToolStripItemEventType.Paint);
										}
										SafeNativeMethods.BitBlt(handleRef, toolStripItem.Bounds.X, toolStripItem.Bounds.Y, toolStripItem.Size.Width, toolStripItem.Size.Height, handleRef2, 0, 0, 13369376);
									}
								}
							}
						}
						finally
						{
							if (graphics2 != null)
							{
								graphics2.Dispose();
							}
						}
					}
				}
				this.Renderer.DrawToolStripBorder(new ToolStripRenderEventArgs(graphics, this));
				if (flag)
				{
					graphics.SetClip(transparentRegion, CombineMode.Union);
				}
				this.PaintInsertionMark(graphics);
			}
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0003959C File Offset: 0x0003859C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			using (new LayoutTransaction(this, this, PropertyNames.RightToLeft))
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].OnParentRightToLeftChanged(e);
				}
				if (this.toolStripOverflowButton != null)
				{
					this.toolStripOverflowButton.OnParentRightToLeftChanged(e);
				}
				if (this.toolStripGrip != null)
				{
					this.toolStripGrip.OnParentRightToLeftChanged(e);
				}
			}
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0003962C File Offset: 0x0003862C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			Graphics graphics = e.Graphics;
			GraphicsState graphicsState = graphics.Save();
			try
			{
				using (Region transparentRegion = this.Renderer.GetTransparentRegion(this))
				{
					if (transparentRegion != null)
					{
						this.EraseCorners(e, transparentRegion);
						graphics.ExcludeClip(transparentRegion);
					}
				}
				this.Renderer.DrawToolStripBackground(new ToolStripRenderEventArgs(graphics, this));
			}
			finally
			{
				if (graphicsState != null)
				{
					graphics.Restore(graphicsState);
				}
			}
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000396B4 File Offset: 0x000386B4
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (!base.Disposing && !base.IsDisposed)
			{
				this.HookStaticEvents(base.Visible);
			}
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000396D9 File Offset: 0x000386D9
		private void EraseCorners(PaintEventArgs e, Region transparentRegion)
		{
			if (transparentRegion != null)
			{
				base.PaintTransparentBackground(e, base.ClientRectangle, transparentRegion);
			}
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000396EC File Offset: 0x000386EC
		protected internal virtual void OnPaintGrip(PaintEventArgs e)
		{
			this.Renderer.DrawGrip(new ToolStripGripRenderEventArgs(e.Graphics, this));
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[ToolStrip.EventPaintGrip];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x00039731 File Offset: 0x00038731
		protected override void OnScroll(ScrollEventArgs se)
		{
			if (se.Type != ScrollEventType.ThumbTrack && se.NewValue != se.OldValue)
			{
				this.ScrollInternal(se.OldValue - se.NewValue);
			}
			base.OnScroll(se);
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x00039764 File Offset: 0x00038764
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			UserPreferenceCategory category = e.Category;
			if (category == UserPreferenceCategory.General)
			{
				this.InvalidateTextItems();
				return;
			}
			if (category != UserPreferenceCategory.Window)
			{
				return;
			}
			this.OnDefaultFontChanged();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0003978F File Offset: 0x0003878F
		protected override void OnTabStopChanged(EventArgs e)
		{
			base.SetStyle(ControlStyles.Selectable, this.TabStop);
			base.OnTabStopChanged(e);
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000397AC File Offset: 0x000387AC
		internal void PaintInsertionMark(Graphics g)
		{
			if (this.lastInsertionMarkRect != Rectangle.Empty)
			{
				int num = 6;
				if (this.Orientation == Orientation.Horizontal)
				{
					int x = this.lastInsertionMarkRect.X;
					int num2 = x + 2;
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(num2, this.lastInsertionMarkRect.Y),
						new Point(num2, this.lastInsertionMarkRect.Bottom - 1),
						new Point(num2 + 1, this.lastInsertionMarkRect.Y),
						new Point(num2 + 1, this.lastInsertionMarkRect.Bottom - 1)
					});
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(x, this.lastInsertionMarkRect.Bottom - 1),
						new Point(x + num - 1, this.lastInsertionMarkRect.Bottom - 1),
						new Point(x + 1, this.lastInsertionMarkRect.Bottom - 2),
						new Point(x + num - 2, this.lastInsertionMarkRect.Bottom - 2)
					});
					g.DrawLines(SystemPens.ControlText, new Point[]
					{
						new Point(x, this.lastInsertionMarkRect.Y),
						new Point(x + num - 1, this.lastInsertionMarkRect.Y),
						new Point(x + 1, this.lastInsertionMarkRect.Y + 1),
						new Point(x + num - 2, this.lastInsertionMarkRect.Y + 1)
					});
					return;
				}
				num = 6;
				int y = this.lastInsertionMarkRect.Y;
				int num3 = y + 2;
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.X, num3),
					new Point(this.lastInsertionMarkRect.Right - 1, num3),
					new Point(this.lastInsertionMarkRect.X, num3 + 1),
					new Point(this.lastInsertionMarkRect.Right - 1, num3 + 1)
				});
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.X, y),
					new Point(this.lastInsertionMarkRect.X, y + num - 1),
					new Point(this.lastInsertionMarkRect.X + 1, y + 1),
					new Point(this.lastInsertionMarkRect.X + 1, y + num - 2)
				});
				g.DrawLines(SystemPens.ControlText, new Point[]
				{
					new Point(this.lastInsertionMarkRect.Right - 1, y),
					new Point(this.lastInsertionMarkRect.Right - 1, y + num - 1),
					new Point(this.lastInsertionMarkRect.Right - 2, y + 1),
					new Point(this.lastInsertionMarkRect.Right - 2, y + num - 2)
				});
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00039BA1 File Offset: 0x00038BA1
		internal void PaintInsertionMark(Rectangle insertionRect)
		{
			if (this.lastInsertionMarkRect != insertionRect)
			{
				this.ClearInsertionMark();
				this.lastInsertionMarkRect = insertionRect;
				base.Invalidate(insertionRect);
			}
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00039BC5 File Offset: 0x00038BC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control GetChildAtPoint(Point point)
		{
			return base.GetChildAtPoint(point);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x00039BCE File Offset: 0x00038BCE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
		{
			return base.GetChildAtPoint(pt, skipValue);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x00039BD8 File Offset: 0x00038BD8
		internal override Control GetFirstChildControlInTabOrder(bool forward)
		{
			return null;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x00039BDB File Offset: 0x00038BDB
		public ToolStripItem GetItemAt(int x, int y)
		{
			return this.GetItemAt(new Point(x, y));
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x00039BEC File Offset: 0x00038BEC
		public ToolStripItem GetItemAt(Point point)
		{
			Rectangle rectangle = new Rectangle(point, ToolStrip.onePixel);
			if (this.lastMouseActiveItem != null && this.lastMouseActiveItem.Bounds.IntersectsWith(rectangle) && this.lastMouseActiveItem.ParentInternal == this)
			{
				return this.lastMouseActiveItem;
			}
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				if (this.DisplayedItems[i] != null && this.DisplayedItems[i].ParentInternal == this)
				{
					Rectangle rectangle2 = this.DisplayedItems[i].Bounds;
					if (this.toolStripGrip != null && this.DisplayedItems[i] == this.toolStripGrip)
					{
						rectangle2 = LayoutUtils.InflateRect(rectangle2, this.GripMargin);
					}
					if (rectangle2.IntersectsWith(rectangle))
					{
						return this.DisplayedItems[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00039CC7 File Offset: 0x00038CC7
		private void RestoreFocusInternal(bool wasInMenuMode)
		{
			if (wasInMenuMode == ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.RestoreFocusInternal();
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00039CD8 File Offset: 0x00038CD8
		internal void RestoreFocusInternal()
		{
			ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
			this.ClearAllSelections();
			this.lastMouseDownedItem = null;
			ToolStripManager.ModalMenuFilter.ExitMenuMode();
			if (!this.IsDropDown)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this.RestoreFocusFilter);
				this.MenuAutoExpand = false;
				if (!base.DesignMode && !this.TabStop && (this.Focused || base.ContainsFocus))
				{
					this.RestoreFocus();
				}
			}
			if (this.KeyboardActive && !this.Focused && !base.ContainsFocus)
			{
				this.KeyboardActive = false;
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x00039D64 File Offset: 0x00038D64
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RestoreFocus()
		{
			bool flag = false;
			if (this.hwndThatLostFocus != IntPtr.Zero && this.hwndThatLostFocus != base.Handle)
			{
				Control control = Control.FromHandleInternal(this.hwndThatLostFocus);
				this.hwndThatLostFocus = IntPtr.Zero;
				if (control != null && control.Visible)
				{
					flag = control.FocusInternal();
				}
			}
			this.hwndThatLostFocus = IntPtr.Zero;
			if (!flag)
			{
				UnsafeNativeMethods.SetFocus(NativeMethods.NullHandleRef);
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x00039DDA File Offset: 0x00038DDA
		internal virtual void ResetRenderMode()
		{
			this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00039DE3 File Offset: 0x00038DE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetMinimumSize()
		{
			CommonProperties.SetMinimumSize(this, new Size(-1, -1));
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x00039DF2 File Offset: 0x00038DF2
		private void ResetGripMargin()
		{
			this.GripMargin = this.Grip.DefaultMargin;
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x00039E05 File Offset: 0x00038E05
		internal void ResumeCaputureMode()
		{
			this.SetToolStripState(8192, false);
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x00039E13 File Offset: 0x00038E13
		internal void SuspendCaputureMode()
		{
			this.SetToolStripState(8192, true);
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00039E24 File Offset: 0x00038E24
		internal virtual void ScrollInternal(int delta)
		{
			base.SuspendLayout();
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				Point location = toolStripItem.Bounds.Location;
				location.Y -= delta;
				this.SetItemLocation(toolStripItem, location);
			}
			base.ResumeLayout(false);
			base.Invalidate();
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x00039EB4 File Offset: 0x00038EB4
		protected internal void SetItemLocation(ToolStripItem item, Point location)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Owner != this)
			{
				throw new NotSupportedException(SR.GetString("ToolStripCanOnlyPositionItsOwnItems"));
			}
			item.SetBounds(new Rectangle(location, item.Size));
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x00039EEF File Offset: 0x00038EEF
		protected static void SetItemParent(ToolStripItem item, ToolStrip parent)
		{
			item.Parent = parent;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x00039EF8 File Offset: 0x00038EF8
		protected override void SetVisibleCore(bool visible)
		{
			if (visible)
			{
				this.SnapMouseLocation();
			}
			else
			{
				if (!base.Disposing && !base.IsDisposed)
				{
					this.ClearAllSelections();
				}
				CachedItemHdcInfo cachedItemHdcInfo = this.cachedItemHdcInfo;
				this.cachedItemHdcInfo = null;
				this.lastMouseDownedItem = null;
				if (cachedItemHdcInfo != null)
				{
					cachedItemHdcInfo.Dispose();
				}
			}
			base.SetVisibleCore(visible);
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x00039F4C File Offset: 0x00038F4C
		internal bool ShouldSelectItem()
		{
			if (this.mouseEnterWhenShown == ToolStrip.InvalidMouseEnter)
			{
				return true;
			}
			Point lastCursorPoint = WindowsFormsUtils.LastCursorPoint;
			if (this.mouseEnterWhenShown != lastCursorPoint)
			{
				this.mouseEnterWhenShown = ToolStrip.InvalidMouseEnter;
				return true;
			}
			return false;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x00039F90 File Offset: 0x00038F90
		protected override void Select(bool directed, bool forward)
		{
			bool flag = true;
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					containerControlInternal.ActiveControl = this;
					flag = containerControlInternal.ActiveControl == this;
				}
			}
			if (directed && flag)
			{
				this.SelectNextToolStripItem(null, forward);
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x00039FD8 File Offset: 0x00038FD8
		internal ToolStripItem SelectNextToolStripItem(ToolStripItem start, bool forward)
		{
			ToolStripItem nextItem = this.GetNextItem(start, forward ? ArrowDirection.Right : ArrowDirection.Left, true);
			this.ChangeSelection(nextItem);
			return nextItem;
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x00039FFE File Offset: 0x00038FFE
		internal void SetFocusUnsafe()
		{
			if (this.TabStop)
			{
				this.FocusInternal();
				return;
			}
			ToolStripManager.ModalMenuFilter.SetActiveToolStrip(this, false);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0003A018 File Offset: 0x00039018
		private void SetupGrip()
		{
			Rectangle empty = Rectangle.Empty;
			Rectangle displayRectangle = this.DisplayRectangle;
			if (this.Orientation == Orientation.Horizontal)
			{
				empty.X = Math.Max(0, displayRectangle.X - this.Grip.GripThickness);
				empty.Y = Math.Max(0, displayRectangle.Top - this.Grip.Margin.Top);
				empty.Width = this.Grip.GripThickness;
				empty.Height = displayRectangle.Height;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					empty.X = base.ClientRectangle.Right - empty.Width - this.Grip.Margin.Horizontal;
					empty.X += this.Grip.Margin.Left;
				}
				else
				{
					empty.X -= this.Grip.Margin.Right;
				}
			}
			else
			{
				empty.X = displayRectangle.Left;
				empty.Y = displayRectangle.Top - (this.Grip.GripThickness + this.Grip.Margin.Bottom);
				empty.Width = displayRectangle.Width;
				empty.Height = this.Grip.GripThickness;
			}
			if (this.Grip.Bounds != empty)
			{
				this.Grip.SetBounds(empty);
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0003A1A7 File Offset: 0x000391A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void SetAutoScrollMargin(int x, int y)
		{
			base.SetAutoScrollMargin(x, y);
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0003A1B4 File Offset: 0x000391B4
		internal void SetLargestItemSize(Size size)
		{
			if (this.toolStripOverflowButton != null && this.toolStripOverflowButton.Visible)
			{
				size = LayoutUtils.UnionSizes(size, this.toolStripOverflowButton.Bounds.Size);
			}
			if (this.toolStripGrip != null && this.toolStripGrip.Visible)
			{
				size = LayoutUtils.UnionSizes(size, this.toolStripGrip.Bounds.Size);
			}
			this.largestDisplayedItemSize = size;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0003A228 File Offset: 0x00039228
		protected virtual void SetDisplayedItems()
		{
			this.DisplayedItems.Clear();
			this.OverflowItems.Clear();
			this.HasVisibleItems = false;
			Size size = Size.Empty;
			if (this.LayoutEngine is ToolStripSplitStackLayout)
			{
				if (ToolStripGripStyle.Visible == this.GripStyle)
				{
					this.DisplayedItems.Add(this.Grip);
					this.SetupGrip();
				}
				Rectangle displayRectangle = this.DisplayRectangle;
				int num = -1;
				for (int i = 0; i < 2; i++)
				{
					int num2 = 0;
					if (i == 1)
					{
						num2 = num;
					}
					while (num2 >= 0 && num2 < this.Items.Count)
					{
						ToolStripItem toolStripItem = this.Items[num2];
						ToolStripItemPlacement placement = toolStripItem.Placement;
						if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
						{
							if (placement == ToolStripItemPlacement.Main)
							{
								bool flag = false;
								if (i == 0)
								{
									flag = toolStripItem.Alignment == ToolStripItemAlignment.Left;
									if (!flag)
									{
										num = num2;
									}
								}
								else if (i == 1)
								{
									flag = toolStripItem.Alignment == ToolStripItemAlignment.Right;
								}
								if (flag)
								{
									this.HasVisibleItems = true;
									size = LayoutUtils.UnionSizes(size, toolStripItem.Bounds.Size);
									this.DisplayedItems.Add(toolStripItem);
								}
							}
							else if (placement == ToolStripItemPlacement.Overflow && !(toolStripItem is ToolStripSeparator))
							{
								if (toolStripItem is ToolStripControlHost && this.OverflowButton.DropDown.IsRestrictedWindow)
								{
									toolStripItem.SetPlacement(ToolStripItemPlacement.None);
								}
								else
								{
									this.OverflowItems.Add(toolStripItem);
								}
							}
						}
						else
						{
							toolStripItem.SetPlacement(ToolStripItemPlacement.None);
						}
						num2 = ((i == 0) ? (num2 + 1) : (num2 - 1));
					}
				}
				ToolStripOverflow overflow = this.GetOverflow();
				if (overflow != null)
				{
					overflow.LayoutRequired = true;
				}
				if (this.OverflowItems.Count == 0)
				{
					this.OverflowButton.Visible = false;
				}
				else if (this.CanOverflow)
				{
					this.DisplayedItems.Add(this.OverflowButton);
				}
			}
			else
			{
				Rectangle clientRectangle = base.ClientRectangle;
				bool flag2 = true;
				for (int j = 0; j < this.Items.Count; j++)
				{
					ToolStripItem toolStripItem2 = this.Items[j];
					if (((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						toolStripItem2.ParentInternal = this;
						bool flag3 = !this.IsDropDown;
						bool flag4 = toolStripItem2.Bounds.IntersectsWith(clientRectangle);
						if (!clientRectangle.Contains(clientRectangle.X, toolStripItem2.Bounds.Top) || !clientRectangle.Contains(clientRectangle.X, toolStripItem2.Bounds.Bottom))
						{
							flag2 = false;
						}
						if (!flag3 || flag4)
						{
							this.HasVisibleItems = true;
							size = LayoutUtils.UnionSizes(size, toolStripItem2.Bounds.Size);
							this.DisplayedItems.Add(toolStripItem2);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.Main);
						}
					}
					else
					{
						toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
					}
				}
				this.AllItemsVisible = flag2;
			}
			this.SetLargestItemSize(size);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0003A503 File Offset: 0x00039503
		internal void SetToolStripState(int flag, bool value)
		{
			this.toolStripState = (value ? (this.toolStripState | flag) : (this.toolStripState & ~flag));
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0003A521 File Offset: 0x00039521
		internal void SnapMouseLocation()
		{
			this.mouseEnterWhenShown = WindowsFormsUtils.LastCursorPoint;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0003A530 File Offset: 0x00039530
		private void SnapFocus(IntPtr otherHwnd)
		{
			if (!this.TabStop && !this.IsDropDown)
			{
				bool flag = false;
				if (this.Focused && otherHwnd != base.Handle)
				{
					flag = true;
				}
				else if (!base.ContainsFocus && !this.Focused)
				{
					flag = true;
				}
				if (flag)
				{
					this.SnapMouseLocation();
					HandleRef handleRef = new HandleRef(this, base.Handle);
					HandleRef handleRef2 = new HandleRef(null, otherHwnd);
					if (handleRef.Handle != handleRef2.Handle && !UnsafeNativeMethods.IsChild(handleRef, handleRef2))
					{
						HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
						HandleRef rootHWnd2 = WindowsFormsUtils.GetRootHWnd(handleRef2);
						if (rootHWnd.Handle == rootHWnd2.Handle && rootHWnd.Handle != IntPtr.Zero)
						{
							this.hwndThatLostFocus = handleRef2.Handle;
						}
					}
				}
			}
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0003A603 File Offset: 0x00039603
		internal void SnapFocusChange(ToolStrip otherToolStrip)
		{
			otherToolStrip.hwndThatLostFocus = this.hwndThatLostFocus;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0003A611 File Offset: 0x00039611
		private bool ShouldSerializeDefaultDropDownDirection()
		{
			return this.toolStripDropDownDirection != ToolStripDropDownDirection.Default;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0003A61F File Offset: 0x0003961F
		internal virtual bool ShouldSerializeLayoutStyle()
		{
			return this.layoutStyle != ToolStripLayoutStyle.StackWithOverflow;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0003A630 File Offset: 0x00039630
		internal override bool ShouldSerializeMinimumSize()
		{
			Size size = new Size(-1, -1);
			return CommonProperties.GetMinimumSize(this, size) != size;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0003A653 File Offset: 0x00039653
		private bool ShouldSerializeGripMargin()
		{
			return this.GripMargin != this.DefaultGripMargin;
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0003A666 File Offset: 0x00039666
		internal virtual bool ShouldSerializeRenderMode()
		{
			return this.RenderMode != ToolStripRenderMode.ManagerRenderMode && this.RenderMode != ToolStripRenderMode.Custom;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0003A680 File Offset: 0x00039680
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			stringBuilder.Append(", Name: ");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Items: ").Append(this.Items.Count);
			return stringBuilder.ToString();
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0003A6D4 File Offset: 0x000396D4
		internal void UpdateToolTip(ToolStripItem item)
		{
			if (this.ShowItemToolTips && item != this.currentlyActiveTooltipItem && this.ToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.ToolTip.Hide(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.ToolTip.Active = false;
				this.currentlyActiveTooltipItem = item;
				if (this.currentlyActiveTooltipItem != null && !this.GetToolStripState(2048))
				{
					Cursor currentInternal = Cursor.CurrentInternal;
					if (currentInternal != null)
					{
						this.ToolTip.Active = true;
						Point point = Cursor.Position;
						point.Y += this.Cursor.Size.Height - currentInternal.HotSpot.Y;
						point = WindowsFormsUtils.ConstrainToScreenBounds(new Rectangle(point, ToolStrip.onePixel)).Location;
						IntSecurity.AllWindows.Assert();
						try
						{
							this.ToolTip.Show(this.currentlyActiveTooltipItem.ToolTipText, this, base.PointToClient(point), this.ToolTip.AutoPopDelay);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0003A814 File Offset: 0x00039814
		private void UpdateLayoutStyle(DockStyle newDock)
		{
			if (!this.IsInToolStripPanel && this.layoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow && this.layoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				using (new LayoutTransaction(this, this, PropertyNames.Orientation))
				{
					if (newDock == DockStyle.Left || newDock == DockStyle.Right)
					{
						this.UpdateOrientation(Orientation.Vertical);
					}
					else
					{
						this.UpdateOrientation(Orientation.Horizontal);
					}
				}
				this.OnLayoutStyleChanged(EventArgs.Empty);
				if (this.ParentInternal != null)
				{
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Orientation);
				}
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0003A8A0 File Offset: 0x000398A0
		private void UpdateLayoutStyle(Orientation newRaftingRowOrientation)
		{
			if (this.layoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow && this.layoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				using (new LayoutTransaction(this, this, PropertyNames.Orientation))
				{
					this.UpdateOrientation(newRaftingRowOrientation);
					if (this.LayoutEngine is ToolStripSplitStackLayout && this.layoutStyle == ToolStripLayoutStyle.StackWithOverflow)
					{
						this.OnLayoutStyleChanged(EventArgs.Empty);
					}
					return;
				}
			}
			this.UpdateOrientation(newRaftingRowOrientation);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0003A918 File Offset: 0x00039918
		private void UpdateOrientation(Orientation newOrientation)
		{
			if (newOrientation != this.orientation)
			{
				Size size = CommonProperties.GetSpecifiedBounds(this).Size;
				this.orientation = newOrientation;
				this.SetupGrip();
			}
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x0003A94C File Offset: 0x0003994C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 7)
			{
				this.SnapFocus(m.WParam);
			}
			if (m.Msg == 33)
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				IntPtr intPtr = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, base.Handle), point.X, point.Y, 7);
				if (intPtr == base.Handle)
				{
					this.lastMouseDownedItem = null;
					m.Result = (IntPtr)3;
					if (!this.IsDropDown)
					{
						HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
						if (rootHWnd.Handle != IntPtr.Zero)
						{
							IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
							if (activeWindow != rootHWnd.Handle)
							{
								m.Result = (IntPtr)2;
							}
						}
					}
					return;
				}
				this.SnapFocus(UnsafeNativeMethods.GetFocus());
				if (!this.IsDropDown && !this.TabStop)
				{
					Application.ThreadContext.FromCurrent().AddMessageFilter(this.RestoreFocusFilter);
				}
			}
			base.WndProc(ref m);
			if (m.Msg == 130 && this.dropDownOwnerWindow != null)
			{
				this.dropDownOwnerWindow.DestroyHandle();
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x0003AA5F File Offset: 0x00039A5F
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.Items;
			}
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x0003AA67 File Offset: 0x00039A67
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x0003AA8C File Offset: 0x00039A8C
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return base.GetState(2);
			}
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0003AA95 File Offset: 0x00039A95
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStrip.ToolStripAccessibleObject(this);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0003AA9D File Offset: 0x00039A9D
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new WindowsFormsUtils.ReadOnlyControlCollection(this, !base.DesignMode);
		}

		// Token: 0x04001329 RID: 4905
		internal const int INSERTION_BEAM_WIDTH = 6;

		// Token: 0x0400132A RID: 4906
		internal const int STATE_CANOVERFLOW = 1;

		// Token: 0x0400132B RID: 4907
		internal const int STATE_ALLOWITEMREORDER = 2;

		// Token: 0x0400132C RID: 4908
		internal const int STATE_DISPOSINGITEMS = 4;

		// Token: 0x0400132D RID: 4909
		internal const int STATE_MENUAUTOEXPAND = 8;

		// Token: 0x0400132E RID: 4910
		internal const int STATE_MENUAUTOEXPANDDEFAULT = 16;

		// Token: 0x0400132F RID: 4911
		internal const int STATE_SCROLLBUTTONS = 32;

		// Token: 0x04001330 RID: 4912
		internal const int STATE_USEDEFAULTRENDERER = 64;

		// Token: 0x04001331 RID: 4913
		internal const int STATE_ALLOWMERGE = 128;

		// Token: 0x04001332 RID: 4914
		internal const int STATE_RAFTING = 256;

		// Token: 0x04001333 RID: 4915
		internal const int STATE_STRETCH = 512;

		// Token: 0x04001334 RID: 4916
		internal const int STATE_LOCATIONCHANGING = 1024;

		// Token: 0x04001335 RID: 4917
		internal const int STATE_DRAGGING = 2048;

		// Token: 0x04001336 RID: 4918
		internal const int STATE_HASVISIBLEITEMS = 4096;

		// Token: 0x04001337 RID: 4919
		internal const int STATE_SUSPENDCAPTURE = 8192;

		// Token: 0x04001338 RID: 4920
		internal const int STATE_LASTMOUSEDOWNEDITEMCAPTURE = 16384;

		// Token: 0x04001339 RID: 4921
		internal const int STATE_MENUACTIVE = 32768;

		// Token: 0x0400133A RID: 4922
		private static Size onePixel = new Size(1, 1);

		// Token: 0x0400133B RID: 4923
		internal static Point InvalidMouseEnter = new Point(int.MaxValue, int.MaxValue);

		// Token: 0x0400133C RID: 4924
		private ToolStripItemCollection toolStripItemCollection;

		// Token: 0x0400133D RID: 4925
		private ToolStripOverflowButton toolStripOverflowButton;

		// Token: 0x0400133E RID: 4926
		private ToolStripGrip toolStripGrip;

		// Token: 0x0400133F RID: 4927
		private ToolStripItemCollection displayedItems;

		// Token: 0x04001340 RID: 4928
		private ToolStripItemCollection overflowItems;

		// Token: 0x04001341 RID: 4929
		private ToolStripDropTargetManager dropTargetManager;

		// Token: 0x04001342 RID: 4930
		private IntPtr hwndThatLostFocus = IntPtr.Zero;

		// Token: 0x04001343 RID: 4931
		private ToolStripItem lastMouseActiveItem;

		// Token: 0x04001344 RID: 4932
		private ToolStripItem lastMouseDownedItem;

		// Token: 0x04001345 RID: 4933
		private LayoutEngine layoutEngine;

		// Token: 0x04001346 RID: 4934
		private ToolStripLayoutStyle layoutStyle;

		// Token: 0x04001347 RID: 4935
		private LayoutSettings layoutSettings;

		// Token: 0x04001348 RID: 4936
		private Rectangle lastInsertionMarkRect = Rectangle.Empty;

		// Token: 0x04001349 RID: 4937
		private ImageList imageList;

		// Token: 0x0400134A RID: 4938
		private ToolStripGripStyle toolStripGripStyle = ToolStripGripStyle.Visible;

		// Token: 0x0400134B RID: 4939
		private ISupportOleDropSource itemReorderDropSource;

		// Token: 0x0400134C RID: 4940
		private IDropTarget itemReorderDropTarget;

		// Token: 0x0400134D RID: 4941
		private int toolStripState;

		// Token: 0x0400134E RID: 4942
		private bool showItemToolTips;

		// Token: 0x0400134F RID: 4943
		private MouseHoverTimer mouseHoverTimer;

		// Token: 0x04001350 RID: 4944
		private ToolStripItem currentlyActiveTooltipItem;

		// Token: 0x04001351 RID: 4945
		private NativeWindow dropDownOwnerWindow;

		// Token: 0x04001352 RID: 4946
		private byte mouseDownID;

		// Token: 0x04001353 RID: 4947
		private Orientation orientation;

		// Token: 0x04001354 RID: 4948
		private ArrayList activeDropDowns = new ArrayList(1);

		// Token: 0x04001355 RID: 4949
		private ToolStripRenderer renderer;

		// Token: 0x04001356 RID: 4950
		private Type currentRendererType = typeof(Type);

		// Token: 0x04001357 RID: 4951
		private Hashtable shortcuts;

		// Token: 0x04001358 RID: 4952
		private Stack<MergeHistory> mergeHistoryStack;

		// Token: 0x04001359 RID: 4953
		private ToolStripDropDownDirection toolStripDropDownDirection = ToolStripDropDownDirection.Default;

		// Token: 0x0400135A RID: 4954
		private Size largestDisplayedItemSize = Size.Empty;

		// Token: 0x0400135B RID: 4955
		private CachedItemHdcInfo cachedItemHdcInfo;

		// Token: 0x0400135C RID: 4956
		private bool alreadyHooked;

		// Token: 0x0400135D RID: 4957
		private Size imageScalingSize = new Size(16, 16);

		// Token: 0x0400135E RID: 4958
		private Font defaultFont;

		// Token: 0x0400135F RID: 4959
		private ToolStrip.RestoreFocusMessageFilter restoreFocusFilter;

		// Token: 0x04001360 RID: 4960
		private bool layoutRequired;

		// Token: 0x04001361 RID: 4961
		private Point mouseEnterWhenShown = ToolStrip.InvalidMouseEnter;

		// Token: 0x04001362 RID: 4962
		private static readonly object EventPaintGrip = new object();

		// Token: 0x04001363 RID: 4963
		private static readonly object EventLayoutCompleted = new object();

		// Token: 0x04001364 RID: 4964
		private static readonly object EventItemAdded = new object();

		// Token: 0x04001365 RID: 4965
		private static readonly object EventItemRemoved = new object();

		// Token: 0x04001366 RID: 4966
		private static readonly object EventLayoutStyleChanged = new object();

		// Token: 0x04001367 RID: 4967
		private static readonly object EventRendererChanged = new object();

		// Token: 0x04001368 RID: 4968
		private static readonly object EventItemClicked = new object();

		// Token: 0x04001369 RID: 4969
		private static readonly object EventLocationChanging = new object();

		// Token: 0x0400136A RID: 4970
		private static readonly object EventBeginDrag = new object();

		// Token: 0x0400136B RID: 4971
		private static readonly object EventEndDrag = new object();

		// Token: 0x0400136C RID: 4972
		private static readonly int PropBindingContext = PropertyStore.CreateKey();

		// Token: 0x0400136D RID: 4973
		private static readonly int PropTextDirection = PropertyStore.CreateKey();

		// Token: 0x0400136E RID: 4974
		private static readonly int PropToolTip = PropertyStore.CreateKey();

		// Token: 0x0400136F RID: 4975
		private static readonly int PropToolStripPanelCell = PropertyStore.CreateKey();

		// Token: 0x04001370 RID: 4976
		internal static readonly TraceSwitch SelectionDebug;

		// Token: 0x04001371 RID: 4977
		internal static readonly TraceSwitch DropTargetDebug;

		// Token: 0x04001372 RID: 4978
		internal static readonly TraceSwitch LayoutDebugSwitch;

		// Token: 0x04001373 RID: 4979
		internal static readonly TraceSwitch MouseActivateDebug;

		// Token: 0x04001374 RID: 4980
		internal static readonly TraceSwitch MergeDebug;

		// Token: 0x04001375 RID: 4981
		internal static readonly TraceSwitch SnapFocusDebug;

		// Token: 0x04001376 RID: 4982
		internal static readonly TraceSwitch FlickerDebug;

		// Token: 0x04001377 RID: 4983
		internal static readonly TraceSwitch ItemReorderDebug;

		// Token: 0x04001378 RID: 4984
		internal static readonly TraceSwitch MDIMergeDebug;

		// Token: 0x04001379 RID: 4985
		internal static readonly TraceSwitch MenuAutoExpandDebug;

		// Token: 0x0400137A RID: 4986
		internal static readonly TraceSwitch ControlTabDebug;

		// Token: 0x02000246 RID: 582
		// (Invoke) Token: 0x06001CE2 RID: 7394
		private delegate void BooleanMethodInvoker(bool arg);

		// Token: 0x02000247 RID: 583
		[ComVisible(true)]
		public class ToolStripAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06001CE5 RID: 7397 RVA: 0x0003AB69 File Offset: 0x00039B69
			public ToolStripAccessibleObject(ToolStrip owner)
				: base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06001CE6 RID: 7398 RVA: 0x0003AB7C File Offset: 0x00039B7C
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = this.owner.PointToClient(new Point(x, y));
				ToolStripItem itemAt = this.owner.GetItemAt(point);
				if (itemAt == null || itemAt.AccessibilityObject == null)
				{
					return base.HitTest(x, y);
				}
				return itemAt.AccessibilityObject;
			}

			// Token: 0x06001CE7 RID: 7399 RVA: 0x0003ABC4 File Offset: 0x00039BC4
			public override AccessibleObject GetChild(int index)
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return null;
				}
				if (index == 0 && this.owner.Grip.Visible)
				{
					return this.owner.Grip.AccessibilityObject;
				}
				if (this.owner.Grip.Visible && index > 0)
				{
					index--;
				}
				if (index < this.owner.Items.Count)
				{
					ToolStripItem toolStripItem = null;
					int num = 0;
					for (int i = 0; i < this.owner.Items.Count; i++)
					{
						if (this.owner.Items[i].Available && this.owner.Items[i].Alignment == ToolStripItemAlignment.Left)
						{
							if (num == index)
							{
								toolStripItem = this.owner.Items[i];
								break;
							}
							num++;
						}
					}
					if (toolStripItem == null)
					{
						for (int j = 0; j < this.owner.Items.Count; j++)
						{
							if (this.owner.Items[j].Available && this.owner.Items[j].Alignment == ToolStripItemAlignment.Right)
							{
								if (num == index)
								{
									toolStripItem = this.owner.Items[j];
									break;
								}
								num++;
							}
						}
					}
					if (toolStripItem == null)
					{
						return null;
					}
					if (toolStripItem.Placement == ToolStripItemPlacement.Overflow)
					{
						return new ToolStrip.ToolStripAccessibleObjectWrapperForItemsOnOverflow(toolStripItem);
					}
					return toolStripItem.AccessibilityObject;
				}
				else
				{
					if (this.owner.CanOverflow && this.owner.OverflowButton.Visible && index == this.owner.Items.Count)
					{
						return this.owner.OverflowButton.AccessibilityObject;
					}
					return null;
				}
			}

			// Token: 0x06001CE8 RID: 7400 RVA: 0x0003AD78 File Offset: 0x00039D78
			public override int GetChildCount()
			{
				if (this.owner == null || this.owner.Items == null)
				{
					return -1;
				}
				int num = 0;
				for (int i = 0; i < this.owner.Items.Count; i++)
				{
					if (this.owner.Items[i].Available)
					{
						num++;
					}
				}
				if (this.owner.Grip.Visible)
				{
					num++;
				}
				if (this.owner.CanOverflow && this.owner.OverflowButton.Visible)
				{
					num++;
				}
				return num;
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x0003AE10 File Offset: 0x00039E10
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ToolBar;
				}
			}

			// Token: 0x0400137B RID: 4987
			private ToolStrip owner;
		}

		// Token: 0x0200024A RID: 586
		private class ToolStripAccessibleObjectWrapperForItemsOnOverflow : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x06001E44 RID: 7748 RVA: 0x0003E7D5 File Offset: 0x0003D7D5
			public ToolStripAccessibleObjectWrapperForItemsOnOverflow(ToolStripItem item)
				: base(item)
			{
			}

			// Token: 0x1700042D RID: 1069
			// (get) Token: 0x06001E45 RID: 7749 RVA: 0x0003E7E0 File Offset: 0x0003D7E0
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					accessibleStates |= AccessibleStates.Offscreen;
					return accessibleStates | AccessibleStates.Invisible;
				}
			}
		}

		// Token: 0x0200024C RID: 588
		internal class RestoreFocusMessageFilter : IMessageFilter
		{
			// Token: 0x06001E47 RID: 7751 RVA: 0x0003E805 File Offset: 0x0003D805
			public RestoreFocusMessageFilter(ToolStrip ownerToolStrip)
			{
				this.ownerToolStrip = ownerToolStrip;
			}

			// Token: 0x06001E48 RID: 7752 RVA: 0x0003E814 File Offset: 0x0003D814
			public bool PreFilterMessage(ref Message m)
			{
				if (this.ownerToolStrip.Disposing || this.ownerToolStrip.IsDisposed || this.ownerToolStrip.IsDropDown)
				{
					return false;
				}
				int msg = m.Msg;
				if (msg <= 167)
				{
					if (msg != 161 && msg != 164 && msg != 167)
					{
						return false;
					}
				}
				else if (msg != 513 && msg != 516 && msg != 519)
				{
					return false;
				}
				if (this.ownerToolStrip.ContainsFocus && !UnsafeNativeMethods.IsChild(new HandleRef(this, this.ownerToolStrip.Handle), new HandleRef(this, m.HWnd)))
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this.ownerToolStrip);
					if (rootHWnd.Handle == m.HWnd || UnsafeNativeMethods.IsChild(rootHWnd, new HandleRef(this, m.HWnd)))
					{
						this.RestoreFocusInternal();
					}
				}
				return false;
			}

			// Token: 0x06001E49 RID: 7753 RVA: 0x0003E8FC File Offset: 0x0003D8FC
			private void RestoreFocusInternal()
			{
				this.ownerToolStrip.BeginInvoke(new ToolStrip.BooleanMethodInvoker(this.ownerToolStrip.RestoreFocusInternal), new object[] { ToolStripManager.ModalMenuFilter.InMenuMode });
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(this);
			}

			// Token: 0x040013DC RID: 5084
			private ToolStrip ownerToolStrip;
		}
	}
}

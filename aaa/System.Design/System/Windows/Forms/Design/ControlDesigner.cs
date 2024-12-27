using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;
using Accessibility;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200017D RID: 381
	public class ControlDesigner : ComponentDesigner
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000DCC RID: 3532 RVA: 0x0003857D File Offset: 0x0003757D
		// (remove) Token: 0x06000DCD RID: 3533 RVA: 0x00038596 File Offset: 0x00037596
		private event EventHandler disposingHandler;

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000385AF File Offset: 0x000375AF
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x000385C6 File Offset: 0x000375C6
		private bool AllowDrop
		{
			get
			{
				return (bool)base.ShadowProperties["AllowDrop"];
			}
			set
			{
				base.ShadowProperties["AllowDrop"] = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000385DE File Offset: 0x000375DE
		protected BehaviorService BehaviorService
		{
			get
			{
				if (this.behaviorService == null)
				{
					this.behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				}
				return this.behaviorService;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x00038609 File Offset: 0x00037609
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x00038611 File Offset: 0x00037611
		internal bool ForceVisible
		{
			get
			{
				return this.forceVisible;
			}
			set
			{
				this.forceVisible = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0003861A File Offset: 0x0003761A
		private Dictionary<IntPtr, bool> SubclassedChildWindows
		{
			get
			{
				if (this.subclassedChildren == null)
				{
					this.subclassedChildren = new Dictionary<IntPtr, bool>();
				}
				return this.subclassedChildren;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00038635 File Offset: 0x00037635
		private IOverlayService OverlayService
		{
			get
			{
				if (this.overlayService == null)
				{
					this.overlayService = (IOverlayService)this.GetService(typeof(IOverlayService));
				}
				return this.overlayService;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00038660 File Offset: 0x00037660
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		private ControlDesigner.DesignerControlCollection Controls
		{
			get
			{
				if (this.controls == null)
				{
					this.controls = new ControlDesigner.DesignerControlCollection(this.Control);
				}
				return this.controls;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00038684 File Offset: 0x00037684
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x000386D0 File Offset: 0x000376D0
		private Point Location
		{
			get
			{
				Point location = this.Control.Location;
				ScrollableControl scrollableControl = this.Control.Parent as ScrollableControl;
				if (scrollableControl != null)
				{
					Point autoScrollPosition = scrollableControl.AutoScrollPosition;
					location.Offset(-autoScrollPosition.X, -autoScrollPosition.Y);
				}
				return location;
			}
			set
			{
				ScrollableControl scrollableControl = this.Control.Parent as ScrollableControl;
				if (scrollableControl != null)
				{
					Point autoScrollPosition = scrollableControl.AutoScrollPosition;
					value.Offset(autoScrollPosition.X, autoScrollPosition.Y);
				}
				this.Control.Location = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0003871C File Offset: 0x0003771C
		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Control.Controls)
				{
					Control control = (Control)obj;
					if (control.Site != null)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(control);
					}
				}
				if (arrayList != null)
				{
					return arrayList;
				}
				return base.AssociatedComponents;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0003879C File Offset: 0x0003779C
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x000387B4 File Offset: 0x000377B4
		private ContextMenu ContextMenu
		{
			get
			{
				return (ContextMenu)base.ShadowProperties["ContextMenu"];
			}
			set
			{
				ContextMenu contextMenu = (ContextMenu)base.ShadowProperties["ContextMenu"];
				if (contextMenu != value)
				{
					EventHandler eventHandler = new EventHandler(this.DetachContextMenu);
					if (contextMenu != null)
					{
						contextMenu.Disposed -= eventHandler;
					}
					base.ShadowProperties["ContextMenu"] = value;
					if (value != null)
					{
						value.Disposed += eventHandler;
					}
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0003880D File Offset: 0x0003780D
		public virtual AccessibleObject AccessibilityObject
		{
			get
			{
				if (this.accessibilityObj == null)
				{
					this.accessibilityObj = new ControlDesigner.ControlDesignerAccessibleObject(this, this.Control);
				}
				return this.accessibilityObj;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0003882F File Offset: 0x0003782F
		public virtual Control Control
		{
			get
			{
				return (Control)base.Component;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0003883C File Offset: 0x0003783C
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00038844 File Offset: 0x00037844
		private ControlDesigner.IDesignerTarget DesignerTarget
		{
			get
			{
				return this.designerTarget;
			}
			set
			{
				this.designerTarget = value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x0003884D File Offset: 0x0003784D
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00038864 File Offset: 0x00037864
		private bool Enabled
		{
			get
			{
				return (bool)base.ShadowProperties["Enabled"];
			}
			set
			{
				base.ShadowProperties["Enabled"] = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0003887C File Offset: 0x0003787C
		protected virtual bool EnableDragRect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0003887F File Offset: 0x0003787F
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x00038887 File Offset: 0x00037887
		private bool Locked
		{
			get
			{
				return this.locked;
			}
			set
			{
				if (this.locked != value)
				{
					this.locked = value;
				}
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x00038899 File Offset: 0x00037899
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x000388AC File Offset: 0x000378AC
		private string Name
		{
			get
			{
				return base.Component.Site.Name;
			}
			set
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost == null || (designerHost != null && !designerHost.Loading))
				{
					base.Component.Site.Name = value;
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x000388F0 File Offset: 0x000378F0
		protected override IComponent ParentComponent
		{
			get
			{
				Control control = base.Component as Control;
				if (control != null && control.Parent != null)
				{
					return control.Parent;
				}
				return base.ParentComponent;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00038921 File Offset: 0x00037921
		public virtual bool ParticipatesWithSnapLines
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00038924 File Offset: 0x00037924
		public virtual int NumberOfInternalControlDesigners()
		{
			return 0;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00038927 File Offset: 0x00037927
		public virtual ControlDesigner InternalControlDesigner(int internalControlIndex)
		{
			return null;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0003892C File Offset: 0x0003792C
		private bool IsResizableConsiderAutoSize(PropertyDescriptor autoSizeProp, PropertyDescriptor autoSizeModeProp)
		{
			object component = base.Component;
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			if (autoSizeProp != null && !autoSizeProp.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden) && !autoSizeProp.Attributes.Contains(BrowsableAttribute.No))
			{
				flag2 = (bool)autoSizeProp.GetValue(component);
			}
			if (autoSizeModeProp != null)
			{
				AutoSizeMode autoSizeMode = (AutoSizeMode)autoSizeModeProp.GetValue(component);
				flag3 = autoSizeMode == AutoSizeMode.GrowOnly;
			}
			if (flag2)
			{
				flag = flag3;
			}
			return flag;
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00038997 File Offset: 0x00037997
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x0003899F File Offset: 0x0003799F
		public bool AutoResizeHandles
		{
			get
			{
				return this.autoResizeHandles;
			}
			set
			{
				this.autoResizeHandles = value;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x000389A8 File Offset: 0x000379A8
		public virtual SelectionRules SelectionRules
		{
			get
			{
				object component = base.Component;
				SelectionRules selectionRules = SelectionRules.Visible;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
				PropertyDescriptor propertyDescriptor = properties["AutoSize"];
				PropertyDescriptor propertyDescriptor2 = properties["AutoSizeMode"];
				PropertyDescriptor propertyDescriptor3;
				if ((propertyDescriptor3 = properties["Location"]) != null && !propertyDescriptor3.IsReadOnly)
				{
					selectionRules |= SelectionRules.Moveable;
				}
				if ((propertyDescriptor3 = properties["Size"]) != null && !propertyDescriptor3.IsReadOnly)
				{
					if (this.AutoResizeHandles && base.Component != this.host.RootComponent)
					{
						selectionRules = (this.IsResizableConsiderAutoSize(propertyDescriptor, propertyDescriptor2) ? (selectionRules | SelectionRules.AllSizeable) : selectionRules);
					}
					else
					{
						selectionRules |= SelectionRules.AllSizeable;
					}
				}
				PropertyDescriptor propertyDescriptor4 = properties["Dock"];
				if (propertyDescriptor4 != null)
				{
					DockStyle dockStyle = (DockStyle)((int)propertyDescriptor4.GetValue(component));
					if (this.Control.Parent != null && this.Control.Parent.IsMirrored)
					{
						if (dockStyle == DockStyle.Left)
						{
							dockStyle = DockStyle.Right;
						}
						else if (dockStyle == DockStyle.Right)
						{
							dockStyle = DockStyle.Left;
						}
					}
					switch (dockStyle)
					{
					case DockStyle.Top:
						selectionRules &= ~(SelectionRules.Moveable | SelectionRules.TopSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
						break;
					case DockStyle.Bottom:
						selectionRules &= ~(SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
						break;
					case DockStyle.Left:
						selectionRules &= ~(SelectionRules.Moveable | SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable);
						break;
					case DockStyle.Right:
						selectionRules &= ~(SelectionRules.Moveable | SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.RightSizeable);
						break;
					case DockStyle.Fill:
						selectionRules &= ~(SelectionRules.Moveable | SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
						break;
					}
				}
				PropertyDescriptor propertyDescriptor5 = properties["Locked"];
				if (propertyDescriptor5 != null)
				{
					object value = propertyDescriptor5.GetValue(component);
					if (value is bool && (bool)value)
					{
						selectionRules = SelectionRules.Visible | SelectionRules.Locked;
					}
				}
				return selectionRules;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x00038B30 File Offset: 0x00037B30
		internal virtual bool ControlSupportsSnaplines
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00038B34 File Offset: 0x00037B34
		internal Point GetOffsetToClientArea()
		{
			NativeMethods.POINT point = new NativeMethods.POINT(0, 0);
			NativeMethods.MapWindowPoints(this.Control.Handle, this.Control.Parent.Handle, point, 1);
			Point location = this.Control.Location;
			if (this.Control.IsMirrored != this.Control.Parent.IsMirrored)
			{
				location.Offset(this.Control.Width, 0);
			}
			return new Point(Math.Abs(point.x - location.X), point.y - location.Y);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00038BCE File Offset: 0x00037BCE
		internal IList SnapLinesInternal()
		{
			return this.SnapLinesInternal(this.Control.Margin);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00038BE4 File Offset: 0x00037BE4
		internal IList SnapLinesInternal(Padding margin)
		{
			ArrayList arrayList = new ArrayList(4);
			int width = this.Control.Width;
			int height = this.Control.Height;
			arrayList.Add(new SnapLine(SnapLineType.Top, 0, SnapLinePriority.Low));
			arrayList.Add(new SnapLine(SnapLineType.Bottom, height - 1, SnapLinePriority.Low));
			arrayList.Add(new SnapLine(SnapLineType.Left, 0, SnapLinePriority.Low));
			arrayList.Add(new SnapLine(SnapLineType.Right, width - 1, SnapLinePriority.Low));
			arrayList.Add(new SnapLine(SnapLineType.Horizontal, -margin.Top, "Margin.Top", SnapLinePriority.Always));
			arrayList.Add(new SnapLine(SnapLineType.Horizontal, margin.Bottom + height, "Margin.Bottom", SnapLinePriority.Always));
			arrayList.Add(new SnapLine(SnapLineType.Vertical, -margin.Left, "Margin.Left", SnapLinePriority.Always));
			arrayList.Add(new SnapLine(SnapLineType.Vertical, margin.Right + width, "Margin.Right", SnapLinePriority.Always));
			return arrayList;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00038CBF File Offset: 0x00037CBF
		public virtual IList SnapLines
		{
			get
			{
				return this.SnapLinesInternal();
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00038CC7 File Offset: 0x00037CC7
		internal virtual Behavior StandardBehavior
		{
			get
			{
				if (this.resizeBehavior == null)
				{
					this.resizeBehavior = new ResizeBehavior(base.Component.Site);
				}
				return this.resizeBehavior;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00038CED File Offset: 0x00037CED
		internal virtual bool SerializePerformLayout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x00038CF0 File Offset: 0x00037CF0
		internal Behavior MoveBehavior
		{
			get
			{
				if (this.moveBehavior == null)
				{
					this.moveBehavior = new ContainerSelectorBehavior(this.Control, base.Component.Site);
				}
				return this.moveBehavior;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x00038D1C File Offset: 0x00037D1C
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x00038D33 File Offset: 0x00037D33
		private bool Visible
		{
			get
			{
				return (bool)base.ShadowProperties["Visible"];
			}
			set
			{
				base.ShadowProperties["Visible"] = value;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00038D4B File Offset: 0x00037D4B
		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.IsRootDesigner)
				{
					return InheritanceAttribute.Inherited;
				}
				return base.InheritanceAttribute;
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00038D61 File Offset: 0x00037D61
		protected void BaseWndProc(ref Message m)
		{
			m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00038D86 File Offset: 0x00037D86
		internal override bool CanBeAssociatedWith(IDesigner parentDesigner)
		{
			return this.CanBeParentedTo(parentDesigner);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00038D90 File Offset: 0x00037D90
		public virtual bool CanBeParentedTo(IDesigner parentDesigner)
		{
			ParentControlDesigner parentControlDesigner = parentDesigner as ParentControlDesigner;
			return parentControlDesigner != null && !this.Control.Contains(parentControlDesigner.Control);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00038DC0 File Offset: 0x00037DC0
		private void DataBindingsCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			Control control = base.Component as Control;
			if (control != null)
			{
				if (control.DataBindings.Count == 0 && this.removalNotificationHooked)
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoved -= this.DataSource_ComponentRemoved;
					}
					this.removalNotificationHooked = false;
					return;
				}
				if (control.DataBindings.Count > 0 && !this.removalNotificationHooked)
				{
					IComponentChangeService componentChangeService2 = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService2 != null)
					{
						componentChangeService2.ComponentRemoved += this.DataSource_ComponentRemoved;
					}
					this.removalNotificationHooked = true;
				}
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00038E70 File Offset: 0x00037E70
		private void DataSource_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			Control control = base.Component as Control;
			if (control != null)
			{
				control.DataBindings.CollectionChanged -= this.dataBindingsCollectionChanged;
				for (int i = 0; i < control.DataBindings.Count; i++)
				{
					Binding binding = control.DataBindings[i];
					if (binding.DataSource == e.Component)
					{
						control.DataBindings.Remove(binding);
					}
				}
				if (control.DataBindings.Count == 0)
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoved -= this.DataSource_ComponentRemoved;
					}
					this.removalNotificationHooked = false;
				}
				control.DataBindings.CollectionChanged += this.dataBindingsCollectionChanged;
			}
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00038F2D File Offset: 0x00037F2D
		protected void DefWndProc(ref Message m)
		{
			this.designerTarget.DefWndProc(ref m);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00038F3B File Offset: 0x00037F3B
		private void DetachContextMenu(object sender, EventArgs e)
		{
			this.ContextMenu = null;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00038F44 File Offset: 0x00037F44
		protected void DisplayError(Exception e)
		{
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				iuiservice.ShowError(e);
				return;
			}
			string text = e.Message;
			if (text == null || text.Length == 0)
			{
				text = e.ToString();
			}
			RTLAwareMessageBox.Show(this.Control, text, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00038FA0 File Offset: 0x00037FA0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.Control != null)
				{
					if (this.dataBindingsCollectionChanged != null)
					{
						this.Control.DataBindings.CollectionChanged -= this.dataBindingsCollectionChanged;
					}
					if (base.Inherited && this.inheritanceUI != null)
					{
						this.inheritanceUI.RemoveInheritedControl(this.Control);
					}
					if (this.removalNotificationHooked)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							componentChangeService.ComponentRemoved -= this.DataSource_ComponentRemoved;
						}
						this.removalNotificationHooked = false;
					}
					if (this.disposingHandler != null)
					{
						this.disposingHandler(this, EventArgs.Empty);
					}
					this.UnhookChildControls(this.Control);
				}
				if (this.ContextMenu != null)
				{
					this.ContextMenu.Disposed -= this.DetachContextMenu;
				}
				if (this.designerTarget != null)
				{
					this.designerTarget.Dispose();
				}
				this.downPos = Point.Empty;
				this.Control.ControlAdded -= this.OnControlAdded;
				this.Control.ControlRemoved -= this.OnControlRemoved;
				this.Control.ParentChanged -= this.OnParentChanged;
				this.Control.SizeChanged -= this.OnSizeChanged;
				this.Control.LocationChanged -= this.OnLocationChanged;
				this.Control.EnabledChanged -= this.OnEnabledChanged;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003912C File Offset: 0x0003812C
		protected bool EnableDesignMode(Control child, string name)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			INestedContainer nestedContainer = this.GetService(typeof(INestedContainer)) as INestedContainer;
			if (nestedContainer == null)
			{
				return false;
			}
			for (int i = 0; i < nestedContainer.Components.Count; i++)
			{
				if (nestedContainer.Components[i].Equals(child))
				{
					return true;
				}
			}
			nestedContainer.Add(child, name);
			return true;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000391A8 File Offset: 0x000381A8
		protected void EnableDragDrop(bool value)
		{
			Control control = this.Control;
			if (control == null)
			{
				return;
			}
			if (value)
			{
				control.DragDrop += this.OnDragDrop;
				control.DragOver += this.OnDragOver;
				control.DragEnter += this.OnDragEnter;
				control.DragLeave += this.OnDragLeave;
				control.GiveFeedback += this.OnGiveFeedback;
				this.hadDragDrop = control.AllowDrop;
				if (!this.hadDragDrop)
				{
					control.AllowDrop = true;
				}
				this.revokeDragDrop = false;
				return;
			}
			control.DragDrop -= this.OnDragDrop;
			control.DragOver -= this.OnDragOver;
			control.DragEnter -= this.OnDragEnter;
			control.DragLeave -= this.OnDragLeave;
			control.GiveFeedback -= this.OnGiveFeedback;
			if (!this.hadDragDrop)
			{
				control.AllowDrop = false;
			}
			this.revokeDragDrop = true;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000392B0 File Offset: 0x000382B0
		protected virtual ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			this.OnSetCursor();
			Cursor cursor = Cursor.Current;
			Rectangle rectangle = this.BehaviorService.ControlRectInAdornerWindow(this.Control);
			ControlBodyGlyph controlBodyGlyph = null;
			Control parent = this.Control.Parent;
			if (parent != null && this.host != null && this.host.RootComponent != base.Component)
			{
				Rectangle rectangle2 = parent.RectangleToScreen(parent.ClientRectangle);
				Rectangle rectangle3 = this.Control.RectangleToScreen(this.Control.ClientRectangle);
				if (!rectangle2.Contains(rectangle3) && !rectangle2.IntersectsWith(rectangle3))
				{
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					if (selectionService != null && selectionService.GetComponentSelected(this.Control))
					{
						controlBodyGlyph = new ControlBodyGlyph(rectangle, cursor, this.Control, this.MoveBehavior);
					}
					else if (cursor == Cursors.SizeAll)
					{
						cursor = Cursors.Default;
					}
				}
			}
			if (controlBodyGlyph == null)
			{
				controlBodyGlyph = new ControlBodyGlyph(rectangle, cursor, this.Control, this);
			}
			return controlBodyGlyph;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000393B2 File Offset: 0x000383B2
		internal ControlBodyGlyph GetControlGlyphInternal(GlyphSelectionType selectionType)
		{
			return this.GetControlGlyph(selectionType);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000393BC File Offset: 0x000383BC
		public virtual GlyphCollection GetGlyphs(GlyphSelectionType selectionType)
		{
			GlyphCollection glyphCollection = new GlyphCollection();
			if (selectionType != GlyphSelectionType.NotSelected)
			{
				Rectangle rectangle = this.BehaviorService.ControlRectInAdornerWindow(this.Control);
				bool flag = selectionType == GlyphSelectionType.SelectedPrimary;
				SelectionRules selectionRules = this.SelectionRules;
				if (this.Locked || this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
				{
					glyphCollection.Add(new LockedHandleGlyph(rectangle, flag));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Top));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Bottom));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Left));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Right));
				}
				else if ((selectionRules & SelectionRules.AllSizeable) == SelectionRules.None)
				{
					glyphCollection.Add(new NoResizeHandleGlyph(rectangle, selectionRules, flag, this.MoveBehavior));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Top, this.MoveBehavior));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Bottom, this.MoveBehavior));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Left, this.MoveBehavior));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Right, this.MoveBehavior));
					if (TypeDescriptor.GetAttributes(base.Component).Contains(DesignTimeVisibleAttribute.Yes) && this.behaviorService.DesignerActionUI != null)
					{
						Glyph designerActionGlyph = this.behaviorService.DesignerActionUI.GetDesignerActionGlyph(base.Component);
						if (designerActionGlyph != null)
						{
							glyphCollection.Insert(0, designerActionGlyph);
						}
					}
				}
				else
				{
					if ((selectionRules & SelectionRules.TopSizeable) != SelectionRules.None)
					{
						glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleTop, this.StandardBehavior, flag));
						if ((selectionRules & SelectionRules.LeftSizeable) != SelectionRules.None)
						{
							glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.UpperLeft, this.StandardBehavior, flag));
						}
						if ((selectionRules & SelectionRules.RightSizeable) != SelectionRules.None)
						{
							glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.UpperRight, this.StandardBehavior, flag));
						}
					}
					if ((selectionRules & SelectionRules.BottomSizeable) != SelectionRules.None)
					{
						glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleBottom, this.StandardBehavior, flag));
						if ((selectionRules & SelectionRules.LeftSizeable) != SelectionRules.None)
						{
							glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.LowerLeft, this.StandardBehavior, flag));
						}
						if ((selectionRules & SelectionRules.RightSizeable) != SelectionRules.None)
						{
							glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.LowerRight, this.StandardBehavior, flag));
						}
					}
					if ((selectionRules & SelectionRules.LeftSizeable) != SelectionRules.None)
					{
						glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleLeft, this.StandardBehavior, flag));
					}
					if ((selectionRules & SelectionRules.RightSizeable) != SelectionRules.None)
					{
						glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleRight, this.StandardBehavior, flag));
					}
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Top, this.StandardBehavior));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Bottom, this.StandardBehavior));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Left, this.StandardBehavior));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Right, this.StandardBehavior));
					if (TypeDescriptor.GetAttributes(base.Component).Contains(DesignTimeVisibleAttribute.Yes) && this.behaviorService.DesignerActionUI != null)
					{
						Glyph designerActionGlyph2 = this.behaviorService.DesignerActionUI.GetDesignerActionGlyph(base.Component);
						if (designerActionGlyph2 != null)
						{
							glyphCollection.Insert(0, designerActionGlyph2);
						}
					}
				}
			}
			return glyphCollection;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003968A File Offset: 0x0003868A
		protected virtual bool GetHitTest(Point point)
		{
			return false;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00039690 File Offset: 0x00038690
		private int GetParentPointFromLparam(IntPtr lParam)
		{
			Point point = new Point(NativeMethods.Util.SignedLOWORD((int)lParam), NativeMethods.Util.SignedHIWORD((int)lParam));
			point = this.Control.PointToScreen(point);
			point = this.Control.Parent.PointToClient(point);
			return NativeMethods.Util.MAKELONG(point.X, point.Y);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000396EC File Offset: 0x000386EC
		protected void HookChildControls(Control firstChild)
		{
			foreach (object obj in firstChild.Controls)
			{
				Control control = (Control)obj;
				if (control != null && this.host != null && !(this.host.GetDesigner(control) is ControlDesigner))
				{
					IWindowTarget windowTarget = control.WindowTarget;
					if (!(windowTarget is ControlDesigner.ChildWindowTarget))
					{
						control.WindowTarget = new ControlDesigner.ChildWindowTarget(this, control, windowTarget);
					}
					if (control.IsHandleCreated)
					{
						Application.OleRequired();
						NativeMethods.RevokeDragDrop(control.Handle);
						this.HookChildHandles(control.Handle);
					}
					else
					{
						control.HandleCreated += this.OnChildHandleCreated;
					}
					this.HookChildControls(control);
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x000397C0 File Offset: 0x000387C0
		private int CurrentProcessId
		{
			get
			{
				if (ControlDesigner.currentProcessId == 0)
				{
					ControlDesigner.currentProcessId = SafeNativeMethods.GetCurrentProcessId();
				}
				return ControlDesigner.currentProcessId;
			}
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x000397D8 File Offset: 0x000387D8
		private bool IsWindowInCurrentProcess(IntPtr hwnd)
		{
			int num;
			UnsafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, hwnd), out num);
			return num == this.CurrentProcessId;
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00039800 File Offset: 0x00038800
		private void OnChildHandleCreated(object sender, EventArgs e)
		{
			Control control = sender as Control;
			if (control != null)
			{
				this.HookChildHandles(control.Handle);
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00039824 File Offset: 0x00038824
		internal void HookChildHandles(IntPtr firstChild)
		{
			IntPtr intPtr = firstChild;
			while (intPtr != IntPtr.Zero)
			{
				if (!this.IsWindowInCurrentProcess(intPtr))
				{
					return;
				}
				Control control = Control.FromHandle(intPtr);
				if (control == null && !this.SubclassedChildWindows.ContainsKey(intPtr))
				{
					NativeMethods.RevokeDragDrop(intPtr);
					new ControlDesigner.ChildSubClass(this, intPtr);
					this.SubclassedChildWindows[intPtr] = true;
				}
				if (control == null || this.Control is UserControl)
				{
					this.HookChildHandles(NativeMethods.GetWindow(intPtr, 5));
				}
				intPtr = NativeMethods.GetWindow(intPtr, 2);
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000398A5 File Offset: 0x000388A5
		internal void RemoveSubclassedWindow(IntPtr hwnd)
		{
			if (this.SubclassedChildWindows.ContainsKey(hwnd))
			{
				this.SubclassedChildWindows.Remove(hwnd);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000398C4 File Offset: 0x000388C4
		public override void Initialize(IComponent component)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component.GetType());
			PropertyDescriptor propertyDescriptor = properties["Visible"];
			if (propertyDescriptor == null || propertyDescriptor.PropertyType != typeof(bool) || !propertyDescriptor.ShouldSerializeValue(component))
			{
				this.Visible = true;
			}
			else
			{
				this.Visible = (bool)propertyDescriptor.GetValue(component);
			}
			PropertyDescriptor propertyDescriptor2 = properties["Enabled"];
			if (propertyDescriptor2 == null || propertyDescriptor2.PropertyType != typeof(bool) || !propertyDescriptor2.ShouldSerializeValue(component))
			{
				this.Enabled = true;
			}
			else
			{
				this.Enabled = (bool)propertyDescriptor2.GetValue(component);
			}
			this.initializing = true;
			base.Initialize(component);
			this.initializing = false;
			this.host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			AttributeCollection attributes = TypeDescriptor.GetAttributes(base.Component);
			DockingAttribute dockingAttribute = (DockingAttribute)attributes[typeof(DockingAttribute)];
			if (dockingAttribute != null && dockingAttribute.DockingBehavior != DockingBehavior.Never)
			{
				this.dockingAction = new ControlDesigner.DockingActionList(this);
				DesignerActionService designerActionService = this.GetService(typeof(DesignerActionService)) as DesignerActionService;
				if (designerActionService != null)
				{
					designerActionService.Add(base.Component, this.dockingAction);
				}
			}
			this.dataBindingsCollectionChanged = new CollectionChangeEventHandler(this.DataBindingsCollectionChanged);
			this.Control.DataBindings.CollectionChanged += this.dataBindingsCollectionChanged;
			this.Control.ControlAdded += this.OnControlAdded;
			this.Control.ControlRemoved += this.OnControlRemoved;
			this.Control.ParentChanged += this.OnParentChanged;
			this.Control.SizeChanged += this.OnSizeChanged;
			this.Control.LocationChanged += this.OnLocationChanged;
			this.DesignerTarget = new ControlDesigner.DesignerWindowTarget(this);
			if (this.Control.IsHandleCreated)
			{
				this.OnCreateHandle();
			}
			if (base.Inherited && this.host != null && this.host.RootComponent != component)
			{
				this.inheritanceUI = (InheritanceUI)this.GetService(typeof(InheritanceUI));
				if (this.inheritanceUI != null)
				{
					this.inheritanceUI.AddInheritedControl(this.Control, this.InheritanceAttribute.InheritanceLevel);
				}
			}
			if ((this.host == null || this.host.RootComponent != component) && this.ForceVisible)
			{
				this.Control.Visible = true;
			}
			this.Control.Enabled = true;
			this.Control.EnabledChanged += this.OnEnabledChanged;
			this.AllowDrop = this.Control.AllowDrop;
			this.statusCommandUI = new StatusCommandUI(component.Site);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00039B88 File Offset: 0x00038B88
		public override void InitializeExistingComponent(IDictionary defaultValues)
		{
			base.InitializeExistingComponent(defaultValues);
			foreach (object obj in this.Control.Controls)
			{
				Control control = (Control)obj;
				if (control != null)
				{
					ISite site = control.Site;
					ControlDesigner.ChildWindowTarget childWindowTarget = control.WindowTarget as ControlDesigner.ChildWindowTarget;
					if (site != null && childWindowTarget != null)
					{
						control.WindowTarget = childWindowTarget.OldWindowTarget;
					}
				}
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00039C14 File Offset: 0x00038C14
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			ISite site = base.Component.Site;
			if (site != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
				{
					propertyDescriptor.SetValue(base.Component, site.Name);
				}
			}
			if (defaultValues != null)
			{
				IComponent component = defaultValues["Parent"] as IComponent;
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (component != null && designerHost != null)
				{
					ParentControlDesigner parentControlDesigner = designerHost.GetDesigner(component) as ParentControlDesigner;
					if (parentControlDesigner != null)
					{
						parentControlDesigner.AddControl(this.Control, defaultValues);
					}
					Control control = component as Control;
					if (control != null)
					{
						AttributeCollection attributes = TypeDescriptor.GetAttributes(base.Component);
						DockingAttribute dockingAttribute = (DockingAttribute)attributes[typeof(DockingAttribute)];
						if (dockingAttribute != null && dockingAttribute.DockingBehavior != DockingBehavior.Never && dockingAttribute.DockingBehavior == DockingBehavior.AutoDock)
						{
							bool flag = true;
							foreach (object obj in control.Controls)
							{
								Control control2 = (Control)obj;
								if (control2 != this.Control && control2.Dock == DockStyle.None)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(base.Component)["Dock"];
								if (propertyDescriptor2 != null && propertyDescriptor2.IsBrowsable)
								{
									propertyDescriptor2.SetValue(base.Component, DockStyle.Fill);
								}
							}
						}
					}
				}
			}
			base.InitializeNewComponent(defaultValues);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00039DD8 File Offset: 0x00038DD8
		[Obsolete("This method has been deprecated. Use InitializeNewComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public override void OnSetComponentDefaults()
		{
			ISite site = base.Component.Site;
			if (site != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
				if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
				{
					propertyDescriptor.SetValue(base.Component, site.Name);
				}
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00039E28 File Offset: 0x00038E28
		private bool IsDoubleClick(int x, int y)
		{
			bool flag = false;
			int doubleClickTime = SystemInformation.DoubleClickTime;
			int num = SafeNativeMethods.GetTickCount() - this.lastClickMessageTime;
			if (num <= doubleClickTime)
			{
				Size doubleClickSize = SystemInformation.DoubleClickSize;
				if (x >= this.lastClickMessagePositionX - doubleClickSize.Width && x <= this.lastClickMessagePositionX + doubleClickSize.Width && y >= this.lastClickMessagePositionY - doubleClickSize.Height && y <= this.lastClickMessagePositionY + doubleClickSize.Height)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.lastClickMessagePositionX = x;
				this.lastClickMessagePositionY = y;
				this.lastClickMessageTime = SafeNativeMethods.GetTickCount();
			}
			else
			{
				this.lastClickMessagePositionX = (this.lastClickMessagePositionY = 0);
				this.lastClickMessageTime = 0;
			}
			return flag;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00039ED4 File Offset: 0x00038ED4
		private bool IsMouseMessage(int msg)
		{
			if (msg >= 512 && msg <= 522)
			{
				return true;
			}
			switch (msg)
			{
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 171:
			case 172:
			case 173:
				break;
			case 170:
				return false;
			default:
				switch (msg)
				{
				case 672:
				case 673:
				case 674:
				case 675:
					break;
				default:
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00039F5A File Offset: 0x00038F5A
		protected virtual void OnContextMenu(int x, int y)
		{
			this.ShowContextMenu(x, y);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00039F64 File Offset: 0x00038F64
		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control != null && this.host != null && !(this.host.GetDesigner(e.Control) is ControlDesigner))
			{
				IWindowTarget windowTarget = e.Control.WindowTarget;
				if (!(windowTarget is ControlDesigner.ChildWindowTarget))
				{
					e.Control.WindowTarget = new ControlDesigner.ChildWindowTarget(this, e.Control, windowTarget);
				}
				if (e.Control.IsHandleCreated)
				{
					Application.OleRequired();
					NativeMethods.RevokeDragDrop(e.Control.Handle);
					this.HookChildControls(e.Control);
				}
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00039FF8 File Offset: 0x00038FF8
		private void OnControlRemoved(object sender, ControlEventArgs e)
		{
			if (e.Control != null)
			{
				ControlDesigner.ChildWindowTarget childWindowTarget = e.Control.WindowTarget as ControlDesigner.ChildWindowTarget;
				if (childWindowTarget != null)
				{
					e.Control.WindowTarget = childWindowTarget.OldWindowTarget;
				}
				this.UnhookChildControls(e.Control);
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0003A03E File Offset: 0x0003903E
		protected virtual void OnCreateHandle()
		{
			this.OnHandleChange();
			if (this.revokeDragDrop)
			{
				NativeMethods.RevokeDragDrop(this.Control.Handle);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0003A05F File Offset: 0x0003905F
		private void OnDragEnter(object s, DragEventArgs e)
		{
			if (this.BehaviorService != null)
			{
				this.BehaviorService.StartDragNotification();
			}
			this.OnDragEnter(e);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0003A07C File Offset: 0x0003907C
		protected virtual void OnDragEnter(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragEnter);
			control.DragEnter -= dragEventHandler;
			((IDropTarget)this.Control).OnDragEnter(de);
			control.DragEnter += dragEventHandler;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0003A0B7 File Offset: 0x000390B7
		private void OnDragDrop(object s, DragEventArgs e)
		{
			if (this.BehaviorService != null)
			{
				this.BehaviorService.EndDragNotification();
			}
			this.OnDragDrop(e);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003A0D3 File Offset: 0x000390D3
		protected virtual void OnDragComplete(DragEventArgs de)
		{
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003A0D8 File Offset: 0x000390D8
		protected virtual void OnDragDrop(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragDrop);
			control.DragDrop -= dragEventHandler;
			((IDropTarget)this.Control).OnDragDrop(de);
			control.DragDrop += dragEventHandler;
			this.OnDragComplete(de);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003A11A File Offset: 0x0003911A
		private void OnDragLeave(object s, EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0003A124 File Offset: 0x00039124
		protected virtual void OnDragLeave(EventArgs e)
		{
			Control control = this.Control;
			EventHandler eventHandler = new EventHandler(this.OnDragLeave);
			control.DragLeave -= eventHandler;
			((IDropTarget)this.Control).OnDragLeave(e);
			control.DragLeave += eventHandler;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0003A15F File Offset: 0x0003915F
		private void OnDragOver(object s, DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0003A168 File Offset: 0x00039168
		protected virtual void OnDragOver(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragOver);
			control.DragOver -= dragEventHandler;
			((IDropTarget)this.Control).OnDragOver(de);
			control.DragOver += dragEventHandler;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0003A1A3 File Offset: 0x000391A3
		private void OnGiveFeedback(object s, GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0003A1AC File Offset: 0x000391AC
		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0003A1AE File Offset: 0x000391AE
		private void OnHandleChange()
		{
			this.HookChildHandles(NativeMethods.GetWindow(this.Control.Handle, 5));
			this.HookChildControls(this.Control);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0003A1D4 File Offset: 0x000391D4
		private void OnMouseDoubleClick()
		{
			try
			{
				this.DoDefaultAction();
			}
			catch (Exception ex)
			{
				this.DisplayError(ex);
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0003A21C File Offset: 0x0003921C
		protected virtual void OnMouseDragBegin(int x, int y)
		{
			if (this.BehaviorService == null && this.mouseDragLast != ControlDesigner.InvalidPoint)
			{
				return;
			}
			this.mouseDragLast = new Point(x, y);
			this.ctrlSelect = (Control.ModifierKeys & Keys.Control) != Keys.None;
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (!this.ctrlSelect && selectionService != null)
			{
				selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Click);
			}
			this.Control.Capture = true;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0003A2B0 File Offset: 0x000392B0
		protected virtual void OnMouseDragEnd(bool cancel)
		{
			this.mouseDragLast = ControlDesigner.InvalidPoint;
			this.Control.Capture = false;
			if (!this.mouseDragMoved)
			{
				if (!cancel)
				{
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					if ((Control.ModifierKeys & Keys.Shift) == Keys.None && (this.ctrlSelect || (selectionService != null && !selectionService.GetComponentSelected(base.Component))))
					{
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Click);
						}
						this.ctrlSelect = false;
					}
				}
				return;
			}
			this.mouseDragMoved = false;
			this.ctrlSelect = false;
			if (this.BehaviorService != null && this.BehaviorService.Dragging && cancel)
			{
				this.BehaviorService.CancelDrag = true;
			}
			if (this.selectionUISvc == null)
			{
				this.selectionUISvc = (ISelectionUIService)this.GetService(typeof(ISelectionUIService));
			}
			if (this.selectionUISvc == null)
			{
				return;
			}
			if (this.selectionUISvc.Dragging)
			{
				this.selectionUISvc.EndDrag(cancel);
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0003A3C0 File Offset: 0x000393C0
		protected virtual void OnMouseDragMove(int x, int y)
		{
			if (!this.mouseDragMoved)
			{
				Size dragSize = SystemInformation.DragSize;
				Size doubleClickSize = SystemInformation.DoubleClickSize;
				dragSize.Width = Math.Max(dragSize.Width, doubleClickSize.Width);
				dragSize.Height = Math.Max(dragSize.Height, doubleClickSize.Height);
				if (this.mouseDragLast == ControlDesigner.InvalidPoint || (Math.Abs(this.mouseDragLast.X - x) < dragSize.Width && Math.Abs(this.mouseDragLast.Y - y) < dragSize.Height))
				{
					return;
				}
				this.mouseDragMoved = true;
				this.ctrlSelect = false;
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null && !base.Component.Equals(selectionService.PrimarySelection))
			{
				selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Click | SelectionTypes.Toggle);
			}
			if (this.BehaviorService != null && selectionService != null)
			{
				ArrayList arrayList = new ArrayList();
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				Control control = null;
				foreach (object obj in selectedComponents)
				{
					IComponent component = (IComponent)obj;
					Control control2 = component as Control;
					if (control2 != null)
					{
						if (control == null)
						{
							control = control2.Parent;
						}
						else if (!control.Equals(control2.Parent))
						{
							continue;
						}
						ControlDesigner controlDesigner = this.host.GetDesigner(component) as ControlDesigner;
						if (controlDesigner != null && (controlDesigner.SelectionRules & SelectionRules.Moveable) != SelectionRules.None)
						{
							arrayList.Add(component);
						}
					}
				}
				if (arrayList.Count > 0)
				{
					using (this.BehaviorService.AdornerWindowGraphics)
					{
						DropSourceBehavior dropSourceBehavior = new DropSourceBehavior(arrayList, this.Control.Parent, this.mouseDragLast);
						this.BehaviorService.DoDragDrop(dropSourceBehavior);
					}
				}
			}
			this.mouseDragLast = ControlDesigner.InvalidPoint;
			this.mouseDragMoved = false;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0003A5E8 File Offset: 0x000395E8
		protected virtual void OnMouseEnter()
		{
			Control control = this.Control;
			Control control2 = control;
			object obj = null;
			while (obj == null && control2 != null)
			{
				control2 = control2.Parent;
				if (control2 != null)
				{
					object designer = this.host.GetDesigner(control2);
					if (designer != this)
					{
						obj = designer;
					}
				}
			}
			ControlDesigner controlDesigner = obj as ControlDesigner;
			if (controlDesigner != null)
			{
				controlDesigner.OnMouseEnter();
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0003A638 File Offset: 0x00039638
		protected virtual void OnMouseHover()
		{
			Control control = this.Control;
			Control control2 = control;
			object obj = null;
			while (obj == null && control2 != null)
			{
				control2 = control2.Parent;
				if (control2 != null)
				{
					object designer = this.host.GetDesigner(control2);
					if (designer != this)
					{
						obj = designer;
					}
				}
			}
			ControlDesigner controlDesigner = obj as ControlDesigner;
			if (controlDesigner != null)
			{
				controlDesigner.OnMouseHover();
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0003A688 File Offset: 0x00039688
		protected virtual void OnMouseLeave()
		{
			Control control = this.Control;
			Control control2 = control;
			object obj = null;
			while (obj == null && control2 != null)
			{
				control2 = control2.Parent;
				if (control2 != null)
				{
					object designer = this.host.GetDesigner(control2);
					if (designer != this)
					{
						obj = designer;
					}
				}
			}
			ControlDesigner controlDesigner = obj as ControlDesigner;
			if (controlDesigner != null)
			{
				controlDesigner.OnMouseLeave();
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0003A6D8 File Offset: 0x000396D8
		protected virtual void OnPaintAdornments(PaintEventArgs pe)
		{
			if (this.inheritanceUI != null && pe.ClipRectangle.IntersectsWith(this.inheritanceUI.InheritanceGlyphRectangle))
			{
				pe.Graphics.DrawImage(this.inheritanceUI.InheritanceGlyph, 0, 0);
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0003A720 File Offset: 0x00039720
		private void OnParentChanged(object sender, EventArgs e)
		{
			if (this.Control.IsHandleCreated)
			{
				this.OnHandleChange();
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0003A738 File Offset: 0x00039738
		private void OnSizeChanged(object sender, EventArgs e)
		{
			ComponentCache componentCache = (ComponentCache)this.GetService(typeof(ComponentCache));
			object component = base.Component;
			if (componentCache != null && component != null)
			{
				componentCache.RemoveEntry(component);
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0003A770 File Offset: 0x00039770
		private void OnLocationChanged(object sender, EventArgs e)
		{
			ComponentCache componentCache = (ComponentCache)this.GetService(typeof(ComponentCache));
			object component = base.Component;
			if (componentCache != null && component != null)
			{
				componentCache.RemoveEntry(component);
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003A7A8 File Offset: 0x000397A8
		private void OnEnabledChanged(object sender, EventArgs e)
		{
			if (!this.enabledchangerecursionguard)
			{
				this.enabledchangerecursionguard = true;
				try
				{
					this.Control.Enabled = true;
				}
				finally
				{
					this.enabledchangerecursionguard = false;
				}
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0003A7EC File Offset: 0x000397EC
		protected virtual void OnSetCursor()
		{
			if (this.Control.Dock != DockStyle.None)
			{
				Cursor.Current = Cursors.Default;
				return;
			}
			if (this.toolboxSvc == null)
			{
				this.toolboxSvc = (IToolboxService)this.GetService(typeof(IToolboxService));
			}
			if (this.toolboxSvc != null && this.toolboxSvc.SetCursor())
			{
				return;
			}
			if (!this.locationChecked)
			{
				this.locationChecked = true;
				try
				{
					this.hasLocation = TypeDescriptor.GetProperties(base.Component)["Location"] != null;
				}
				catch
				{
				}
			}
			if (!this.hasLocation)
			{
				Cursor.Current = Cursors.Default;
				return;
			}
			if (this.Locked)
			{
				Cursor.Current = Cursors.Default;
				return;
			}
			Cursor.Current = Cursors.SizeAll;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0003A8C0 File Offset: 0x000398C0
		private void PaintException(PaintEventArgs e, Exception ex)
		{
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Near;
			stringFormat.LineAlignment = StringAlignment.Near;
			string text = ex.ToString();
			stringFormat.SetMeasurableCharacterRanges(new CharacterRange[]
			{
				new CharacterRange(0, text.Length)
			});
			int num = 2;
			Size iconSize = SystemInformation.IconSize;
			int num2 = num * 2;
			int num3 = num * 2;
			Rectangle clientRectangle = this.Control.ClientRectangle;
			Rectangle rectangle = clientRectangle;
			rectangle.X++;
			rectangle.Y++;
			rectangle.Width -= 2;
			rectangle.Height -= 2;
			Rectangle rectangle2 = new Rectangle(num2, num3, iconSize.Width, iconSize.Height);
			Rectangle rectangle3 = clientRectangle;
			rectangle3.X = rectangle2.X + rectangle2.Width + 2 * num2;
			rectangle3.Y = rectangle2.Y;
			rectangle3.Width -= rectangle3.X + num2 + num;
			rectangle3.Height -= rectangle3.Y + num3 + num;
			using (Font font = new Font(this.Control.Font.FontFamily, (float)Math.Max(SystemInformation.ToolWindowCaptionHeight - SystemInformation.BorderSize.Height - 2, this.Control.Font.Height), GraphicsUnit.Pixel))
			{
				using (Region region = e.Graphics.MeasureCharacterRanges(text, font, rectangle3, stringFormat)[0])
				{
					Region clip = e.Graphics.Clip;
					e.Graphics.ExcludeClip(region);
					e.Graphics.ExcludeClip(rectangle2);
					try
					{
						e.Graphics.FillRectangle(Brushes.White, clientRectangle);
					}
					finally
					{
						e.Graphics.Clip = clip;
					}
					using (Pen pen = new Pen(Color.Red, (float)num))
					{
						e.Graphics.DrawRectangle(pen, rectangle);
					}
					Icon error = SystemIcons.Error;
					e.Graphics.FillRectangle(Brushes.White, rectangle2);
					e.Graphics.DrawIcon(error, rectangle2.X, rectangle2.Y);
					rectangle3.X++;
					e.Graphics.IntersectClip(region);
					try
					{
						e.Graphics.FillRectangle(Brushes.White, rectangle3);
						e.Graphics.DrawString(text, font, new SolidBrush(this.Control.ForeColor), rectangle3, stringFormat);
					}
					finally
					{
						e.Graphics.Clip = clip;
					}
				}
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003ABEC File Offset: 0x00039BEC
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Visible", "Enabled", "ContextMenu", "AllowDrop", "Location", "Name" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ControlDesigner), propertyDescriptor, array2);
				}
			}
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties["Controls"];
			if (propertyDescriptor2 != null)
			{
				Attribute[] array3 = new Attribute[propertyDescriptor2.Attributes.Count];
				propertyDescriptor2.Attributes.CopyTo(array3, 0);
				properties["Controls"] = TypeDescriptor.CreateProperty(typeof(ControlDesigner), "Controls", typeof(ControlDesigner.DesignerControlCollection), array3);
			}
			PropertyDescriptor propertyDescriptor3 = (PropertyDescriptor)properties["Size"];
			if (propertyDescriptor3 != null)
			{
				properties["Size"] = new ControlDesigner.CanResetSizePropertyDescriptor(propertyDescriptor3);
			}
			properties["Locked"] = TypeDescriptor.CreateProperty(typeof(ControlDesigner), "Locked", typeof(bool), new Attribute[]
			{
				new DefaultValueAttribute(false),
				BrowsableAttribute.Yes,
				CategoryAttribute.Design,
				DesignOnlyAttribute.Yes,
				new SRDescriptionAttribute("lockedDescr")
			});
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0003AD73 File Offset: 0x00039D73
		private void ResetVisible()
		{
			this.Visible = true;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0003AD7C File Offset: 0x00039D7C
		private void ResetEnabled()
		{
			this.Enabled = true;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0003AD88 File Offset: 0x00039D88
		internal void SetUnhandledException(Control owner, Exception exception)
		{
			if (this.thrownException == null)
			{
				this.thrownException = exception;
				if (owner == null)
				{
					owner = this.Control;
				}
				string text = string.Empty;
				string[] array = exception.StackTrace.Split(new char[] { '\r', '\n' });
				string fullName = owner.GetType().FullName;
				foreach (string text2 in array)
				{
					if (text2.IndexOf(fullName) != -1)
					{
						text = string.Format(CultureInfo.CurrentCulture, "{0}\r\n{1}", new object[] { text, text2 });
					}
				}
				Exception ex = new Exception(SR.GetString("ControlDesigner_WndProcException", new object[] { fullName, exception.Message, text }), exception);
				this.DisplayError(ex);
				foreach (object obj in this.Control.Controls)
				{
					Control control = (Control)obj;
					control.Visible = false;
				}
				this.Control.Invalidate(true);
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0003AED0 File Offset: 0x00039ED0
		private bool ShouldSerializeAllowDrop()
		{
			return this.AllowDrop != this.hadDragDrop;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0003AEE3 File Offset: 0x00039EE3
		private bool ShouldSerializeEnabled()
		{
			return base.ShadowProperties.ShouldSerializeValue("Enabled", true);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0003AEFB File Offset: 0x00039EFB
		private bool ShouldSerializeVisible()
		{
			return base.ShadowProperties.ShouldSerializeValue("Visible", true);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0003AF14 File Offset: 0x00039F14
		private bool ShouldSerializeName()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!this.initializing)
			{
				return base.ShadowProperties.ShouldSerializeValue("Name", null);
			}
			return base.Component != designerHost.RootComponent;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0003AF64 File Offset: 0x00039F64
		protected void UnhookChildControls(Control firstChild)
		{
			if (this.host == null)
			{
				this.host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			}
			foreach (object obj in firstChild.Controls)
			{
				Control control = (Control)obj;
				IWindowTarget windowTarget = null;
				if (control != null)
				{
					windowTarget = control.WindowTarget;
					ControlDesigner.ChildWindowTarget childWindowTarget = windowTarget as ControlDesigner.ChildWindowTarget;
					if (childWindowTarget != null)
					{
						control.WindowTarget = childWindowTarget.OldWindowTarget;
					}
				}
				if (!(windowTarget is ControlDesigner.DesignerWindowTarget))
				{
					this.UnhookChildControls(control);
				}
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0003B010 File Offset: 0x0003A010
		protected virtual void WndProc(ref Message m)
		{
			IMouseHandler mouseHandler = null;
			if (m.Msg == 132 && !this.inHitTest)
			{
				this.inHitTest = true;
				Point point = new Point((int)((short)NativeMethods.Util.LOWORD((int)m.LParam)), (int)((short)NativeMethods.Util.HIWORD((int)m.LParam)));
				try
				{
					this.liveRegion = this.GetHitTest(point);
				}
				catch (Exception ex)
				{
					this.liveRegion = false;
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				catch
				{
					this.liveRegion = false;
				}
				this.inHitTest = false;
			}
			bool flag = m.Msg == 123;
			if (this.liveRegion && (this.IsMouseMessage(m.Msg) || flag))
			{
				if (m.Msg == 123)
				{
					ControlDesigner.inContextMenu = true;
				}
				try
				{
					this.DefWndProc(ref m);
				}
				finally
				{
					if (m.Msg == 123)
					{
						ControlDesigner.inContextMenu = false;
					}
					if (m.Msg == 514)
					{
						this.OnMouseDragEnd(true);
					}
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			if ((m.Msg >= 512 && m.Msg <= 522) || (m.Msg >= 160 && m.Msg <= 169) || m.Msg == 32)
			{
				if (this.eventSvc == null)
				{
					this.eventSvc = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
				}
				if (this.eventSvc != null)
				{
					mouseHandler = (IMouseHandler)this.eventSvc.GetHandler(typeof(IMouseHandler));
				}
			}
			if (m.Msg >= 512 && m.Msg <= 522)
			{
				NativeMethods.POINT point2 = new NativeMethods.POINT();
				point2.x = NativeMethods.Util.SignedLOWORD((int)m.LParam);
				point2.y = NativeMethods.Util.SignedHIWORD((int)m.LParam);
				NativeMethods.MapWindowPoints(m.HWnd, IntPtr.Zero, point2, 1);
				num = point2.x;
				num2 = point2.y;
			}
			else if (m.Msg >= 160 && m.Msg <= 169)
			{
				num = NativeMethods.Util.SignedLOWORD((int)m.LParam);
				num2 = NativeMethods.Util.SignedHIWORD((int)m.LParam);
			}
			MouseButtons mouseButtons = MouseButtons.None;
			int msg = m.Msg;
			if (msg > 61)
			{
				if (msg <= 169)
				{
					if (msg != 123)
					{
						switch (msg)
						{
						case 133:
						case 134:
							if (m.Msg == 134)
							{
								this.DefWndProc(ref m);
							}
							else if (this.thrownException == null)
							{
								this.DefWndProc(ref m);
							}
							if (this.OverlayService != null && this.Control != null && this.Control.Size != this.Control.ClientSize && this.Control.Parent != null)
							{
								Rectangle rectangle = new Rectangle(this.Control.Parent.PointToScreen(this.Control.Location), this.Control.Size);
								Rectangle rectangle2 = new Rectangle(this.Control.PointToScreen(Point.Empty), this.Control.ClientSize);
								using (Region region = new Region(rectangle))
								{
									region.Exclude(rectangle2);
									this.OverlayService.InvalidateOverlays(region);
									return;
								}
								goto IL_0A86;
							}
							return;
						default:
							switch (msg)
							{
							case 160:
								goto IL_05F2;
							case 161:
							case 164:
								goto IL_0459;
							case 162:
							case 165:
								goto IL_06D0;
							case 163:
							case 166:
								break;
							case 167:
							case 168:
							case 169:
								return;
							default:
								goto IL_0BD4;
							}
							break;
						}
					}
					else
					{
						if (ControlDesigner.inContextMenu)
						{
							return;
						}
						num = NativeMethods.Util.SignedLOWORD((int)m.LParam);
						num2 = NativeMethods.Util.SignedHIWORD((int)m.LParam);
						ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
						bool flag2 = false;
						if (toolStripKeyboardHandlingService != null)
						{
							flag2 = toolStripKeyboardHandlingService.OnContextMenu(num, num2);
						}
						if (!flag2)
						{
							if (num == -1 && num2 == -1)
							{
								Point position = Cursor.Position;
								num = position.X;
								num2 = position.Y;
							}
							this.OnContextMenu(num, num2);
							return;
						}
						return;
					}
				}
				else
				{
					switch (msg)
					{
					case 512:
						goto IL_05F2;
					case 513:
					case 516:
						goto IL_0459;
					case 514:
					case 517:
						goto IL_06D0;
					case 515:
					case 518:
						break;
					case 519:
					case 520:
					case 521:
					case 522:
						return;
					default:
						switch (msg)
						{
						case 672:
						case 674:
							return;
						case 673:
							if (mouseHandler != null)
							{
								mouseHandler.OnMouseHover(base.Component);
								return;
							}
							this.OnMouseHover();
							return;
						case 675:
							this.OnMouseLeave();
							this.BaseWndProc(ref m);
							return;
						default:
						{
							if (msg != 792)
							{
								goto IL_0BD4;
							}
							using (Graphics graphics = Graphics.FromHdc(m.WParam))
							{
								using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, this.Control.ClientRectangle))
								{
									this.DefWndProc(ref m);
									this.OnPaintAdornments(paintEventArgs);
								}
								return;
							}
							goto IL_07BE;
						}
						}
						break;
					}
				}
				if (m.Msg == 166 || m.Msg == 518)
				{
					mouseButtons = MouseButtons.Right;
				}
				else
				{
					mouseButtons = MouseButtons.Left;
				}
				if (mouseButtons != MouseButtons.Left)
				{
					return;
				}
				if (mouseHandler != null)
				{
					mouseHandler.OnMouseDoubleClick(base.Component);
					return;
				}
				this.OnMouseDoubleClick();
				return;
				IL_0459:
				if (m.Msg == 164 || m.Msg == 516)
				{
					mouseButtons = MouseButtons.Right;
				}
				else
				{
					mouseButtons = MouseButtons.Left;
				}
				NativeMethods.SendMessage(this.Control.Handle, 7, 0, 0);
				if (mouseButtons == MouseButtons.Left && this.IsDoubleClick(num, num2))
				{
					if (mouseHandler != null)
					{
						mouseHandler.OnMouseDoubleClick(base.Component);
						return;
					}
					this.OnMouseDoubleClick();
					return;
				}
				else
				{
					this.toolPassThrough = false;
					if (!this.EnableDragRect && mouseButtons == MouseButtons.Left)
					{
						if (this.toolboxSvc == null)
						{
							this.toolboxSvc = (IToolboxService)this.GetService(typeof(IToolboxService));
						}
						if (this.toolboxSvc != null && this.toolboxSvc.GetSelectedToolboxItem((IDesignerHost)this.GetService(typeof(IDesignerHost))) != null)
						{
							this.toolPassThrough = true;
						}
					}
					else
					{
						this.toolPassThrough = false;
					}
					if (this.toolPassThrough)
					{
						NativeMethods.SendMessage(this.Control.Parent.Handle, m.Msg, m.WParam, (IntPtr)this.GetParentPointFromLparam(m.LParam));
						return;
					}
					if (mouseHandler != null)
					{
						mouseHandler.OnMouseDown(base.Component, mouseButtons, num, num2);
					}
					else if (mouseButtons == MouseButtons.Left)
					{
						this.OnMouseDragBegin(num, num2);
					}
					else if (mouseButtons == MouseButtons.Right)
					{
						ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Click);
						}
					}
					this.lastMoveScreenX = num;
					this.lastMoveScreenY = num2;
					return;
				}
				IL_05F2:
				if (((int)m.WParam & 1) != 0)
				{
					mouseButtons = MouseButtons.Left;
				}
				else if (((int)m.WParam & 2) != 0)
				{
					mouseButtons = MouseButtons.Right;
					this.toolPassThrough = false;
				}
				else
				{
					this.toolPassThrough = false;
				}
				if (this.lastMoveScreenX != num || this.lastMoveScreenY != num2)
				{
					if (this.toolPassThrough)
					{
						NativeMethods.SendMessage(this.Control.Parent.Handle, m.Msg, m.WParam, (IntPtr)this.GetParentPointFromLparam(m.LParam));
						return;
					}
					if (mouseHandler != null)
					{
						mouseHandler.OnMouseMove(base.Component, num, num2);
					}
					else if (mouseButtons == MouseButtons.Left)
					{
						this.OnMouseDragMove(num, num2);
					}
				}
				this.lastMoveScreenX = num;
				this.lastMoveScreenY = num2;
				if (m.Msg == 512)
				{
					this.BaseWndProc(ref m);
					return;
				}
				return;
				IL_06D0:
				if (m.Msg == 165 || m.Msg == 517)
				{
					mouseButtons = MouseButtons.Right;
				}
				else
				{
					mouseButtons = MouseButtons.Left;
				}
				if (mouseHandler != null)
				{
					mouseHandler.OnMouseUp(base.Component, mouseButtons);
				}
				else
				{
					if (this.toolPassThrough)
					{
						NativeMethods.SendMessage(this.Control.Parent.Handle, m.Msg, m.WParam, (IntPtr)this.GetParentPointFromLparam(m.LParam));
						this.toolPassThrough = false;
						return;
					}
					if (mouseButtons == MouseButtons.Left)
					{
						this.OnMouseDragEnd(false);
					}
				}
				this.toolPassThrough = false;
				this.BaseWndProc(ref m);
				return;
			}
			if (msg <= 7)
			{
				if (msg != 1)
				{
					switch (msg)
					{
					case 5:
						if (this.thrownException != null)
						{
							this.Control.Invalidate();
						}
						this.DefWndProc(ref m);
						return;
					case 6:
						goto IL_0BD4;
					case 7:
					{
						if (this.host == null || this.host.RootComponent == null)
						{
							return;
						}
						IRootDesigner rootDesigner = this.host.GetDesigner(this.host.RootComponent) as IRootDesigner;
						if (rootDesigner == null)
						{
							return;
						}
						ViewTechnology[] supportedTechnologies = rootDesigner.SupportedTechnologies;
						if (supportedTechnologies.Length <= 0)
						{
							return;
						}
						Control control = rootDesigner.GetView(supportedTechnologies[0]) as Control;
						if (control != null)
						{
							control.Focus();
							return;
						}
						return;
					}
					default:
						goto IL_0BD4;
					}
				}
				else
				{
					this.DefWndProc(ref m);
					if (m.HWnd == this.Control.Handle)
					{
						this.OnCreateHandle();
						return;
					}
					return;
				}
			}
			else if (msg != 15)
			{
				switch (msg)
				{
				case 31:
					this.OnMouseDragEnd(true);
					this.DefWndProc(ref m);
					return;
				case 32:
					goto IL_0A86;
				default:
					if (msg != 61)
					{
						goto IL_0BD4;
					}
					if (-4 == (int)m.LParam)
					{
						Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
						try
						{
							IAccessible accessibilityObject = this.AccessibilityObject;
							if (accessibilityObject == null)
							{
								m.Result = (IntPtr)0;
							}
							else
							{
								IntPtr iunknownForObject = Marshal.GetIUnknownForObject(accessibilityObject);
								try
								{
									m.Result = UnsafeNativeMethods.LresultFromObject(ref guid, m.WParam, iunknownForObject);
								}
								finally
								{
									Marshal.Release(iunknownForObject);
								}
							}
							return;
						}
						catch (Exception ex2)
						{
							throw ex2;
						}
						catch
						{
							throw;
						}
					}
					this.DefWndProc(ref m);
					return;
				}
			}
			IL_07BE:
			if (OleDragDropHandler.FreezePainting)
			{
				NativeMethods.ValidateRect(m.HWnd, IntPtr.Zero);
				return;
			}
			if (this.Control == null)
			{
				return;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			IntPtr intPtr = NativeMethods.CreateRectRgn(0, 0, 0, 0);
			NativeMethods.GetUpdateRgn(m.HWnd, intPtr, false);
			NativeMethods.GetUpdateRect(m.HWnd, ref rect, false);
			Region region2 = Region.FromHrgn(intPtr);
			Rectangle rectangle3 = Rectangle.Empty;
			try
			{
				if (this.thrownException == null)
				{
					this.DefWndProc(ref m);
				}
				Graphics graphics2 = Graphics.FromHwnd(m.HWnd);
				try
				{
					if (m.HWnd != this.Control.Handle)
					{
						NativeMethods.POINT point3 = new NativeMethods.POINT();
						point3.x = 0;
						point3.y = 0;
						NativeMethods.MapWindowPoints(m.HWnd, this.Control.Handle, point3, 1);
						graphics2.TranslateTransform((float)(-(float)point3.x), (float)(-(float)point3.y));
						NativeMethods.MapWindowPoints(m.HWnd, this.Control.Handle, ref rect, 2);
					}
					rectangle3 = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					PaintEventArgs paintEventArgs2 = new PaintEventArgs(graphics2, rectangle3);
					try
					{
						graphics2.Clip = region2;
						if (this.thrownException == null)
						{
							this.OnPaintAdornments(paintEventArgs2);
						}
						else
						{
							UnsafeNativeMethods.PAINTSTRUCT paintstruct = default(UnsafeNativeMethods.PAINTSTRUCT);
							UnsafeNativeMethods.BeginPaint(m.HWnd, ref paintstruct);
							this.PaintException(paintEventArgs2, this.thrownException);
							UnsafeNativeMethods.EndPaint(m.HWnd, ref paintstruct);
						}
					}
					finally
					{
						paintEventArgs2.Dispose();
					}
				}
				finally
				{
					graphics2.Dispose();
				}
			}
			finally
			{
				region2.Dispose();
				NativeMethods.DeleteObject(intPtr);
			}
			if (this.OverlayService != null)
			{
				rectangle3.Location = this.Control.PointToScreen(rectangle3.Location);
				this.OverlayService.InvalidateOverlays(rectangle3);
				return;
			}
			return;
			IL_0A86:
			if (this.liveRegion)
			{
				this.DefWndProc(ref m);
				return;
			}
			if (mouseHandler != null)
			{
				mouseHandler.OnSetCursor(base.Component);
				return;
			}
			this.OnSetCursor();
			return;
			IL_0BD4:
			if (m.Msg == NativeMethods.WM_MOUSEENTER)
			{
				this.OnMouseEnter();
				this.BaseWndProc(ref m);
				return;
			}
			if (m.Msg < 256 || m.Msg > 264)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x04000F36 RID: 3894
		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x04000F37 RID: 3895
		private static int currentProcessId;

		// Token: 0x04000F38 RID: 3896
		private IDesignerHost host;

		// Token: 0x04000F39 RID: 3897
		private ControlDesigner.IDesignerTarget designerTarget;

		// Token: 0x04000F3A RID: 3898
		private bool liveRegion;

		// Token: 0x04000F3B RID: 3899
		private bool inHitTest;

		// Token: 0x04000F3C RID: 3900
		private bool hasLocation;

		// Token: 0x04000F3D RID: 3901
		private bool locationChecked;

		// Token: 0x04000F3E RID: 3902
		private bool locked;

		// Token: 0x04000F3F RID: 3903
		private bool initializing;

		// Token: 0x04000F40 RID: 3904
		private bool enabledchangerecursionguard;

		// Token: 0x04000F41 RID: 3905
		private BehaviorService behaviorService;

		// Token: 0x04000F42 RID: 3906
		private ResizeBehavior resizeBehavior;

		// Token: 0x04000F43 RID: 3907
		private ContainerSelectorBehavior moveBehavior;

		// Token: 0x04000F44 RID: 3908
		private ISelectionUIService selectionUISvc;

		// Token: 0x04000F45 RID: 3909
		private IEventHandlerService eventSvc;

		// Token: 0x04000F46 RID: 3910
		private IToolboxService toolboxSvc;

		// Token: 0x04000F47 RID: 3911
		private InheritanceUI inheritanceUI;

		// Token: 0x04000F48 RID: 3912
		private IOverlayService overlayService;

		// Token: 0x04000F49 RID: 3913
		private Point mouseDragLast = ControlDesigner.InvalidPoint;

		// Token: 0x04000F4A RID: 3914
		private bool mouseDragMoved;

		// Token: 0x04000F4B RID: 3915
		private int lastMoveScreenX;

		// Token: 0x04000F4C RID: 3916
		private int lastMoveScreenY;

		// Token: 0x04000F4D RID: 3917
		private int lastClickMessageTime;

		// Token: 0x04000F4E RID: 3918
		private int lastClickMessagePositionX;

		// Token: 0x04000F4F RID: 3919
		private int lastClickMessagePositionY;

		// Token: 0x04000F50 RID: 3920
		private Point downPos = Point.Empty;

		// Token: 0x04000F52 RID: 3922
		private CollectionChangeEventHandler dataBindingsCollectionChanged;

		// Token: 0x04000F53 RID: 3923
		private Exception thrownException;

		// Token: 0x04000F54 RID: 3924
		private bool ctrlSelect;

		// Token: 0x04000F55 RID: 3925
		private bool toolPassThrough;

		// Token: 0x04000F56 RID: 3926
		private bool removalNotificationHooked;

		// Token: 0x04000F57 RID: 3927
		private bool revokeDragDrop = true;

		// Token: 0x04000F58 RID: 3928
		private bool hadDragDrop;

		// Token: 0x04000F59 RID: 3929
		private ControlDesigner.DesignerControlCollection controls;

		// Token: 0x04000F5A RID: 3930
		private static bool inContextMenu = false;

		// Token: 0x04000F5B RID: 3931
		private ControlDesigner.DockingActionList dockingAction;

		// Token: 0x04000F5C RID: 3932
		private StatusCommandUI statusCommandUI;

		// Token: 0x04000F5D RID: 3933
		private bool forceVisible = true;

		// Token: 0x04000F5E RID: 3934
		private bool autoResizeHandles;

		// Token: 0x04000F5F RID: 3935
		private Dictionary<IntPtr, bool> subclassedChildren;

		// Token: 0x04000F60 RID: 3936
		protected AccessibleObject accessibilityObj;

		// Token: 0x0200017E RID: 382
		private interface IDesignerTarget : IDisposable
		{
			// Token: 0x06000E3F RID: 3647
			void DefWndProc(ref Message m);
		}

		// Token: 0x0200017F RID: 383
		private class ChildSubClass : NativeWindow, ControlDesigner.IDesignerTarget, IDisposable
		{
			// Token: 0x06000E40 RID: 3648 RVA: 0x0003BD9C File Offset: 0x0003AD9C
			public ChildSubClass(ControlDesigner designer, IntPtr hwnd)
			{
				this.designer = designer;
				if (designer != null)
				{
					designer.disposingHandler = (EventHandler)Delegate.Combine(designer.disposingHandler, new EventHandler(this.OnDesignerDisposing));
				}
				base.AssignHandle(hwnd);
			}

			// Token: 0x06000E41 RID: 3649 RVA: 0x0003BDD7 File Offset: 0x0003ADD7
			void ControlDesigner.IDesignerTarget.DefWndProc(ref Message m)
			{
				base.DefWndProc(ref m);
			}

			// Token: 0x06000E42 RID: 3650 RVA: 0x0003BDE0 File Offset: 0x0003ADE0
			public void Dispose()
			{
				this.designer = null;
			}

			// Token: 0x06000E43 RID: 3651 RVA: 0x0003BDE9 File Offset: 0x0003ADE9
			private void OnDesignerDisposing(object sender, EventArgs e)
			{
				this.Dispose();
			}

			// Token: 0x06000E44 RID: 3652 RVA: 0x0003BDF4 File Offset: 0x0003ADF4
			protected override void WndProc(ref Message m)
			{
				if (this.designer == null)
				{
					base.DefWndProc(ref m);
					return;
				}
				if (m.Msg == 2)
				{
					this.designer.RemoveSubclassedWindow(m.HWnd);
				}
				if (m.Msg == 528 && NativeMethods.Util.LOWORD((int)m.WParam) == 1)
				{
					this.designer.HookChildHandles(m.LParam);
				}
				ControlDesigner.IDesignerTarget designerTarget = this.designer.DesignerTarget;
				this.designer.DesignerTarget = this;
				try
				{
					this.designer.WndProc(ref m);
				}
				catch (Exception ex)
				{
					this.designer.SetUnhandledException(Control.FromChildHandle(m.HWnd), ex);
				}
				catch
				{
				}
				finally
				{
					if (this.designer != null && this.designer.Component != null)
					{
						this.designer.DesignerTarget = designerTarget;
					}
				}
			}

			// Token: 0x04000F61 RID: 3937
			private ControlDesigner designer;
		}

		// Token: 0x02000180 RID: 384
		private class ChildWindowTarget : IWindowTarget, ControlDesigner.IDesignerTarget, IDisposable
		{
			// Token: 0x06000E45 RID: 3653 RVA: 0x0003BEEC File Offset: 0x0003AEEC
			public ChildWindowTarget(ControlDesigner designer, Control childControl, IWindowTarget oldWindowTarget)
			{
				this.designer = designer;
				this.childControl = childControl;
				this.oldWindowTarget = oldWindowTarget;
			}

			// Token: 0x17000243 RID: 579
			// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0003BF14 File Offset: 0x0003AF14
			public IWindowTarget OldWindowTarget
			{
				get
				{
					return this.oldWindowTarget;
				}
			}

			// Token: 0x06000E47 RID: 3655 RVA: 0x0003BF1C File Offset: 0x0003AF1C
			public void DefWndProc(ref Message m)
			{
				this.oldWindowTarget.OnMessage(ref m);
			}

			// Token: 0x06000E48 RID: 3656 RVA: 0x0003BF2A File Offset: 0x0003AF2A
			public void Dispose()
			{
			}

			// Token: 0x06000E49 RID: 3657 RVA: 0x0003BF2C File Offset: 0x0003AF2C
			public void OnHandleChange(IntPtr newHandle)
			{
				this.handle = newHandle;
				this.oldWindowTarget.OnHandleChange(newHandle);
			}

			// Token: 0x06000E4A RID: 3658 RVA: 0x0003BF44 File Offset: 0x0003AF44
			public void OnMessage(ref Message m)
			{
				if (this.designer.Component == null)
				{
					this.oldWindowTarget.OnMessage(ref m);
					return;
				}
				ControlDesigner.IDesignerTarget designerTarget = this.designer.DesignerTarget;
				this.designer.DesignerTarget = this;
				try
				{
					this.designer.WndProc(ref m);
				}
				catch (Exception ex)
				{
					this.designer.SetUnhandledException(this.childControl, ex);
				}
				catch
				{
				}
				finally
				{
					if (this.designer.DesignerTarget == null)
					{
						designerTarget.Dispose();
					}
					else
					{
						this.designer.DesignerTarget = designerTarget;
					}
					if (m.Msg == 1)
					{
						NativeMethods.RevokeDragDrop(this.handle);
					}
				}
			}

			// Token: 0x04000F62 RID: 3938
			private ControlDesigner designer;

			// Token: 0x04000F63 RID: 3939
			private Control childControl;

			// Token: 0x04000F64 RID: 3940
			private IWindowTarget oldWindowTarget;

			// Token: 0x04000F65 RID: 3941
			private IntPtr handle = IntPtr.Zero;
		}

		// Token: 0x02000181 RID: 385
		private class DesignerWindowTarget : IWindowTarget, ControlDesigner.IDesignerTarget, IDisposable
		{
			// Token: 0x06000E4B RID: 3659 RVA: 0x0003C00C File Offset: 0x0003B00C
			public DesignerWindowTarget(ControlDesigner designer)
			{
				Control control = designer.Control;
				this.designer = designer;
				this.oldTarget = control.WindowTarget;
				control.WindowTarget = this;
			}

			// Token: 0x06000E4C RID: 3660 RVA: 0x0003C040 File Offset: 0x0003B040
			public void DefWndProc(ref Message m)
			{
				this.oldTarget.OnMessage(ref m);
			}

			// Token: 0x06000E4D RID: 3661 RVA: 0x0003C04E File Offset: 0x0003B04E
			public void Dispose()
			{
				if (this.designer != null)
				{
					this.designer.Control.WindowTarget = this.oldTarget;
					this.designer = null;
				}
			}

			// Token: 0x06000E4E RID: 3662 RVA: 0x0003C075 File Offset: 0x0003B075
			public void OnHandleChange(IntPtr newHandle)
			{
				this.oldTarget.OnHandleChange(newHandle);
				if (newHandle != IntPtr.Zero)
				{
					this.designer.OnHandleChange();
				}
			}

			// Token: 0x06000E4F RID: 3663 RVA: 0x0003C09C File Offset: 0x0003B09C
			public void OnMessage(ref Message m)
			{
				ControlDesigner controlDesigner = this.designer;
				if (controlDesigner != null)
				{
					ControlDesigner.IDesignerTarget designerTarget = controlDesigner.DesignerTarget;
					controlDesigner.DesignerTarget = this;
					try
					{
						try
						{
							controlDesigner.WndProc(ref m);
						}
						catch (Exception ex)
						{
							this.designer.SetUnhandledException(this.designer.Control, ex);
						}
						catch
						{
						}
						return;
					}
					finally
					{
						if (controlDesigner != null)
						{
							controlDesigner.DesignerTarget = designerTarget;
						}
					}
				}
				this.DefWndProc(ref m);
			}

			// Token: 0x04000F66 RID: 3942
			internal ControlDesigner designer;

			// Token: 0x04000F67 RID: 3943
			internal IWindowTarget oldTarget;
		}

		// Token: 0x02000182 RID: 386
		[ComVisible(true)]
		public class ControlDesignerAccessibleObject : AccessibleObject
		{
			// Token: 0x06000E50 RID: 3664 RVA: 0x0003C124 File Offset: 0x0003B124
			public ControlDesignerAccessibleObject(ControlDesigner designer, Control control)
			{
				this.designer = designer;
				this.control = control;
			}

			// Token: 0x17000244 RID: 580
			// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0003C13A File Offset: 0x0003B13A
			public override Rectangle Bounds
			{
				get
				{
					return this.control.AccessibilityObject.Bounds;
				}
			}

			// Token: 0x17000245 RID: 581
			// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0003C14C File Offset: 0x0003B14C
			public override string Description
			{
				get
				{
					return this.control.AccessibilityObject.Description;
				}
			}

			// Token: 0x17000246 RID: 582
			// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0003C15E File Offset: 0x0003B15E
			private IDesignerHost DesignerHost
			{
				get
				{
					if (this.host == null)
					{
						this.host = (IDesignerHost)this.designer.GetService(typeof(IDesignerHost));
					}
					return this.host;
				}
			}

			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0003C18E File Offset: 0x0003B18E
			public override string DefaultAction
			{
				get
				{
					return "";
				}
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0003C195 File Offset: 0x0003B195
			public override string Name
			{
				get
				{
					return this.control.Name;
				}
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0003C1A2 File Offset: 0x0003B1A2
			public override AccessibleObject Parent
			{
				get
				{
					return this.control.AccessibilityObject.Parent;
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0003C1B4 File Offset: 0x0003B1B4
			public override AccessibleRole Role
			{
				get
				{
					return this.control.AccessibilityObject.Role;
				}
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0003C1C6 File Offset: 0x0003B1C6
			private ISelectionService SelectionService
			{
				get
				{
					if (this.selSvc == null)
					{
						this.selSvc = (ISelectionService)this.designer.GetService(typeof(ISelectionService));
					}
					return this.selSvc;
				}
			}

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0003C1F8 File Offset: 0x0003B1F8
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = this.control.AccessibilityObject.State;
					ISelectionService selectionService = this.SelectionService;
					if (selectionService != null)
					{
						if (selectionService.GetComponentSelected(this.control))
						{
							accessibleStates |= AccessibleStates.Selected;
						}
						if (selectionService.PrimarySelection == this.control)
						{
							accessibleStates |= AccessibleStates.Focused;
						}
					}
					return accessibleStates;
				}
			}

			// Token: 0x1700024D RID: 589
			// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0003C245 File Offset: 0x0003B245
			public override string Value
			{
				get
				{
					return this.control.AccessibilityObject.Value;
				}
			}

			// Token: 0x06000E5B RID: 3675 RVA: 0x0003C258 File Offset: 0x0003B258
			public override AccessibleObject GetChild(int index)
			{
				Control.ControlAccessibleObject controlAccessibleObject = this.control.AccessibilityObject.GetChild(index) as Control.ControlAccessibleObject;
				if (controlAccessibleObject != null)
				{
					AccessibleObject designerAccessibleObject = this.GetDesignerAccessibleObject(controlAccessibleObject);
					if (designerAccessibleObject != null)
					{
						return designerAccessibleObject;
					}
				}
				return this.control.AccessibilityObject.GetChild(index);
			}

			// Token: 0x06000E5C RID: 3676 RVA: 0x0003C29D File Offset: 0x0003B29D
			public override int GetChildCount()
			{
				return this.control.AccessibilityObject.GetChildCount();
			}

			// Token: 0x06000E5D RID: 3677 RVA: 0x0003C2B0 File Offset: 0x0003B2B0
			private AccessibleObject GetDesignerAccessibleObject(Control.ControlAccessibleObject cao)
			{
				if (cao == null)
				{
					return null;
				}
				ControlDesigner controlDesigner = this.DesignerHost.GetDesigner(cao.Owner) as ControlDesigner;
				if (controlDesigner != null)
				{
					return controlDesigner.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06000E5E RID: 3678 RVA: 0x0003C2E4 File Offset: 0x0003B2E4
			public override AccessibleObject GetFocused()
			{
				if ((this.State & AccessibleStates.Focused) != AccessibleStates.None)
				{
					return this;
				}
				return base.GetFocused();
			}

			// Token: 0x06000E5F RID: 3679 RVA: 0x0003C2F8 File Offset: 0x0003B2F8
			public override AccessibleObject GetSelected()
			{
				if ((this.State & AccessibleStates.Selected) != AccessibleStates.None)
				{
					return this;
				}
				return base.GetFocused();
			}

			// Token: 0x06000E60 RID: 3680 RVA: 0x0003C30C File Offset: 0x0003B30C
			public override AccessibleObject HitTest(int x, int y)
			{
				return this.control.AccessibilityObject.HitTest(x, y);
			}

			// Token: 0x04000F68 RID: 3944
			private ControlDesigner designer;

			// Token: 0x04000F69 RID: 3945
			private Control control;

			// Token: 0x04000F6A RID: 3946
			private IDesignerHost host;

			// Token: 0x04000F6B RID: 3947
			private ISelectionService selSvc;
		}

		// Token: 0x02000183 RID: 387
		[ListBindable(false)]
		[DesignerSerializer(typeof(ControlDesigner.DesignerControlCollectionCodeDomSerializer), typeof(CodeDomSerializer))]
		internal class DesignerControlCollection : Control.ControlCollection, IList, ICollection, IEnumerable
		{
			// Token: 0x06000E61 RID: 3681 RVA: 0x0003C320 File Offset: 0x0003B320
			public DesignerControlCollection(Control owner)
				: base(owner)
			{
				this.realCollection = owner.Controls;
			}

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0003C335 File Offset: 0x0003B335
			public override int Count
			{
				get
				{
					return this.realCollection.Count;
				}
			}

			// Token: 0x1700024F RID: 591
			// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0003C342 File Offset: 0x0003B342
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000250 RID: 592
			// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0003C345 File Offset: 0x0003B345
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000251 RID: 593
			// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0003C348 File Offset: 0x0003B348
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000252 RID: 594
			// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0003C34B File Offset: 0x0003B34B
			public new bool IsReadOnly
			{
				get
				{
					return this.realCollection.IsReadOnly;
				}
			}

			// Token: 0x06000E67 RID: 3687 RVA: 0x0003C358 File Offset: 0x0003B358
			int IList.Add(object control)
			{
				return ((IList)this.realCollection).Add(control);
			}

			// Token: 0x06000E68 RID: 3688 RVA: 0x0003C366 File Offset: 0x0003B366
			public override void Add(Control c)
			{
				this.realCollection.Add(c);
			}

			// Token: 0x06000E69 RID: 3689 RVA: 0x0003C374 File Offset: 0x0003B374
			public override void AddRange(Control[] controls)
			{
				this.realCollection.AddRange(controls);
			}

			// Token: 0x06000E6A RID: 3690 RVA: 0x0003C382 File Offset: 0x0003B382
			bool IList.Contains(object control)
			{
				return ((IList)this.realCollection).Contains(control);
			}

			// Token: 0x06000E6B RID: 3691 RVA: 0x0003C390 File Offset: 0x0003B390
			public new void CopyTo(Array dest, int index)
			{
				this.realCollection.CopyTo(dest, index);
			}

			// Token: 0x06000E6C RID: 3692 RVA: 0x0003C39F File Offset: 0x0003B39F
			public override bool Equals(object other)
			{
				return this.realCollection.Equals(other);
			}

			// Token: 0x06000E6D RID: 3693 RVA: 0x0003C3AD File Offset: 0x0003B3AD
			public new IEnumerator GetEnumerator()
			{
				return this.realCollection.GetEnumerator();
			}

			// Token: 0x06000E6E RID: 3694 RVA: 0x0003C3BA File Offset: 0x0003B3BA
			public override int GetHashCode()
			{
				return this.realCollection.GetHashCode();
			}

			// Token: 0x06000E6F RID: 3695 RVA: 0x0003C3C7 File Offset: 0x0003B3C7
			int IList.IndexOf(object control)
			{
				return ((IList)this.realCollection).IndexOf(control);
			}

			// Token: 0x06000E70 RID: 3696 RVA: 0x0003C3D5 File Offset: 0x0003B3D5
			void IList.Insert(int index, object value)
			{
				((IList)this.realCollection).Insert(index, value);
			}

			// Token: 0x06000E71 RID: 3697 RVA: 0x0003C3E4 File Offset: 0x0003B3E4
			void IList.Remove(object control)
			{
				((IList)this.realCollection).Remove(control);
			}

			// Token: 0x06000E72 RID: 3698 RVA: 0x0003C3F2 File Offset: 0x0003B3F2
			void IList.RemoveAt(int index)
			{
				((IList)this.realCollection).RemoveAt(index);
			}

			// Token: 0x17000253 RID: 595
			object IList.this[int index]
			{
				get
				{
					return ((IList)this.realCollection)[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x06000E75 RID: 3701 RVA: 0x0003C415 File Offset: 0x0003B415
			public override int GetChildIndex(Control child, bool throwException)
			{
				return this.realCollection.GetChildIndex(child, throwException);
			}

			// Token: 0x06000E76 RID: 3702 RVA: 0x0003C424 File Offset: 0x0003B424
			public override void SetChildIndex(Control child, int newIndex)
			{
				this.realCollection.SetChildIndex(child, newIndex);
			}

			// Token: 0x06000E77 RID: 3703 RVA: 0x0003C434 File Offset: 0x0003B434
			public override void Clear()
			{
				for (int i = this.realCollection.Count - 1; i >= 0; i--)
				{
					if (this.realCollection[i] != null && this.realCollection[i].Site != null && TypeDescriptor.GetAttributes(this.realCollection[i]).Contains(InheritanceAttribute.NotInherited))
					{
						this.realCollection.RemoveAt(i);
					}
				}
			}

			// Token: 0x04000F6C RID: 3948
			private Control.ControlCollection realCollection;
		}

		// Token: 0x02000184 RID: 388
		internal class DesignerControlCollectionCodeDomSerializer : CollectionCodeDomSerializer
		{
			// Token: 0x06000E78 RID: 3704 RVA: 0x0003C4A4 File Offset: 0x0003B4A4
			protected override object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
			{
				ArrayList arrayList = new ArrayList();
				if (valuesToSerialize != null && valuesToSerialize.Count > 0)
				{
					foreach (object obj in valuesToSerialize)
					{
						IComponent component = obj as IComponent;
						if (component != null && component.Site != null && !(component.Site is INestedSite))
						{
							arrayList.Add(component);
						}
					}
				}
				return base.SerializeCollection(manager, targetExpression, targetType, originalCollection, arrayList);
			}
		}

		// Token: 0x02000185 RID: 389
		private class DockingActionList : DesignerActionList
		{
			// Token: 0x06000E7A RID: 3706 RVA: 0x0003C540 File Offset: 0x0003B540
			public DockingActionList(ControlDesigner owner)
				: base(owner.Component)
			{
				this._designer = owner;
				this._host = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
			}

			// Token: 0x06000E7B RID: 3707 RVA: 0x0003C570 File Offset: 0x0003B570
			private string GetActionName()
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Dock"];
				if (propertyDescriptor == null)
				{
					return null;
				}
				DockStyle dockStyle = (DockStyle)propertyDescriptor.GetValue(base.Component);
				if (dockStyle == DockStyle.Fill)
				{
					return SR.GetString("DesignerShortcutUndockInParent");
				}
				return SR.GetString("DesignerShortcutDockInParent");
			}

			// Token: 0x06000E7C RID: 3708 RVA: 0x0003C5C4 File Offset: 0x0003B5C4
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				string actionName = this.GetActionName();
				if (actionName != null)
				{
					designerActionItemCollection.Add(new DesignerActionVerbItem(new DesignerVerb(this.GetActionName(), new EventHandler(this.OnDockActionClick))));
				}
				return designerActionItemCollection;
			}

			// Token: 0x06000E7D RID: 3709 RVA: 0x0003C608 File Offset: 0x0003B608
			private void OnDockActionClick(object sender, EventArgs e)
			{
				DesignerVerb designerVerb = sender as DesignerVerb;
				if (designerVerb != null && this._host != null)
				{
					using (DesignerTransaction designerTransaction = this._host.CreateTransaction(designerVerb.Text))
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Dock"];
						DockStyle dockStyle = (DockStyle)propertyDescriptor.GetValue(base.Component);
						if (dockStyle == DockStyle.Fill)
						{
							propertyDescriptor.SetValue(base.Component, DockStyle.None);
						}
						else
						{
							propertyDescriptor.SetValue(base.Component, DockStyle.Fill);
						}
						designerTransaction.Commit();
					}
				}
			}

			// Token: 0x04000F6D RID: 3949
			private ControlDesigner _designer;

			// Token: 0x04000F6E RID: 3950
			private IDesignerHost _host;
		}

		// Token: 0x02000187 RID: 391
		internal class TransparentBehavior : Behavior
		{
			// Token: 0x06000E93 RID: 3731 RVA: 0x0003CAB4 File Offset: 0x0003BAB4
			internal TransparentBehavior(ControlDesigner designer)
			{
				this.designer = designer;
			}

			// Token: 0x06000E94 RID: 3732 RVA: 0x0003CACE File Offset: 0x0003BACE
			internal bool IsTransparent(Point p)
			{
				return this.designer.GetHitTest(p);
			}

			// Token: 0x06000E95 RID: 3733 RVA: 0x0003CADC File Offset: 0x0003BADC
			public override void OnDragDrop(Glyph g, DragEventArgs e)
			{
				this.controlRect = Rectangle.Empty;
				this.designer.OnDragDrop(e);
			}

			// Token: 0x06000E96 RID: 3734 RVA: 0x0003CAF8 File Offset: 0x0003BAF8
			public override void OnDragEnter(Glyph g, DragEventArgs e)
			{
				if (this.designer != null && this.designer.Control != null)
				{
					this.controlRect = this.designer.Control.RectangleToScreen(this.designer.Control.ClientRectangle);
				}
				this.designer.OnDragEnter(e);
			}

			// Token: 0x06000E97 RID: 3735 RVA: 0x0003CB4C File Offset: 0x0003BB4C
			public override void OnDragLeave(Glyph g, EventArgs e)
			{
				this.controlRect = Rectangle.Empty;
				this.designer.OnDragLeave(e);
			}

			// Token: 0x06000E98 RID: 3736 RVA: 0x0003CB68 File Offset: 0x0003BB68
			public override void OnDragOver(Glyph g, DragEventArgs e)
			{
				if (e != null && this.controlRect != Rectangle.Empty && !this.controlRect.Contains(new Point(e.X, e.Y)))
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				this.designer.OnDragOver(e);
			}

			// Token: 0x06000E99 RID: 3737 RVA: 0x0003CBBC File Offset: 0x0003BBBC
			public override void OnGiveFeedback(Glyph g, GiveFeedbackEventArgs e)
			{
				this.designer.OnGiveFeedback(e);
			}

			// Token: 0x04000F71 RID: 3953
			private ControlDesigner designer;

			// Token: 0x04000F72 RID: 3954
			private Rectangle controlRect = Rectangle.Empty;
		}

		// Token: 0x02000188 RID: 392
		private class CanResetSizePropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x06000E9A RID: 3738 RVA: 0x0003CBCA File Offset: 0x0003BBCA
			public CanResetSizePropertyDescriptor(PropertyDescriptor pd)
				: base(pd)
			{
				this._basePropDesc = pd;
			}

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0003CBDA File Offset: 0x0003BBDA
			public override Type ComponentType
			{
				get
				{
					return this._basePropDesc.ComponentType;
				}
			}

			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0003CBE7 File Offset: 0x0003BBE7
			public override string DisplayName
			{
				get
				{
					return this._basePropDesc.DisplayName;
				}
			}

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0003CBF4 File Offset: 0x0003BBF4
			public override bool IsReadOnly
			{
				get
				{
					return this._basePropDesc.IsReadOnly;
				}
			}

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0003CC01 File Offset: 0x0003BC01
			public override Type PropertyType
			{
				get
				{
					return this._basePropDesc.PropertyType;
				}
			}

			// Token: 0x06000E9F RID: 3743 RVA: 0x0003CC0E File Offset: 0x0003BC0E
			public override bool CanResetValue(object component)
			{
				return this._basePropDesc.ShouldSerializeValue(component);
			}

			// Token: 0x06000EA0 RID: 3744 RVA: 0x0003CC1C File Offset: 0x0003BC1C
			public override object GetValue(object component)
			{
				return this._basePropDesc.GetValue(component);
			}

			// Token: 0x06000EA1 RID: 3745 RVA: 0x0003CC2A File Offset: 0x0003BC2A
			public override void ResetValue(object component)
			{
				this._basePropDesc.ResetValue(component);
			}

			// Token: 0x06000EA2 RID: 3746 RVA: 0x0003CC38 File Offset: 0x0003BC38
			public override void SetValue(object component, object value)
			{
				this._basePropDesc.SetValue(component, value);
			}

			// Token: 0x06000EA3 RID: 3747 RVA: 0x0003CC47 File Offset: 0x0003BC47
			public override bool ShouldSerializeValue(object component)
			{
				return true;
			}

			// Token: 0x04000F73 RID: 3955
			private PropertyDescriptor _basePropDesc;
		}
	}
}

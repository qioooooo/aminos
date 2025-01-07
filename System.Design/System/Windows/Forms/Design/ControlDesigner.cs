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
	public class ControlDesigner : ComponentDesigner
	{
		private event EventHandler disposingHandler;

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

		public virtual Control Control
		{
			get
			{
				return (Control)base.Component;
			}
		}

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

		protected virtual bool EnableDragRect
		{
			get
			{
				return false;
			}
		}

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

		public virtual bool ParticipatesWithSnapLines
		{
			get
			{
				return true;
			}
		}

		public virtual int NumberOfInternalControlDesigners()
		{
			return 0;
		}

		public virtual ControlDesigner InternalControlDesigner(int internalControlIndex)
		{
			return null;
		}

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

		internal virtual bool ControlSupportsSnaplines
		{
			get
			{
				return true;
			}
		}

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

		internal IList SnapLinesInternal()
		{
			return this.SnapLinesInternal(this.Control.Margin);
		}

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

		public virtual IList SnapLines
		{
			get
			{
				return this.SnapLinesInternal();
			}
		}

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

		internal virtual bool SerializePerformLayout
		{
			get
			{
				return false;
			}
		}

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

		protected void BaseWndProc(ref Message m)
		{
			m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
		}

		internal override bool CanBeAssociatedWith(IDesigner parentDesigner)
		{
			return this.CanBeParentedTo(parentDesigner);
		}

		public virtual bool CanBeParentedTo(IDesigner parentDesigner)
		{
			ParentControlDesigner parentControlDesigner = parentDesigner as ParentControlDesigner;
			return parentControlDesigner != null && !this.Control.Contains(parentControlDesigner.Control);
		}

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

		protected void DefWndProc(ref Message m)
		{
			this.designerTarget.DefWndProc(ref m);
		}

		private void DetachContextMenu(object sender, EventArgs e)
		{
			this.ContextMenu = null;
		}

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

		internal ControlBodyGlyph GetControlGlyphInternal(GlyphSelectionType selectionType)
		{
			return this.GetControlGlyph(selectionType);
		}

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

		protected virtual bool GetHitTest(Point point)
		{
			return false;
		}

		private int GetParentPointFromLparam(IntPtr lParam)
		{
			Point point = new Point(NativeMethods.Util.SignedLOWORD((int)lParam), NativeMethods.Util.SignedHIWORD((int)lParam));
			point = this.Control.PointToScreen(point);
			point = this.Control.Parent.PointToClient(point);
			return NativeMethods.Util.MAKELONG(point.X, point.Y);
		}

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

		private bool IsWindowInCurrentProcess(IntPtr hwnd)
		{
			int num;
			UnsafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, hwnd), out num);
			return num == this.CurrentProcessId;
		}

		private void OnChildHandleCreated(object sender, EventArgs e)
		{
			Control control = sender as Control;
			if (control != null)
			{
				this.HookChildHandles(control.Handle);
			}
		}

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

		internal void RemoveSubclassedWindow(IntPtr hwnd)
		{
			if (this.SubclassedChildWindows.ContainsKey(hwnd))
			{
				this.SubclassedChildWindows.Remove(hwnd);
			}
		}

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

		protected virtual void OnContextMenu(int x, int y)
		{
			this.ShowContextMenu(x, y);
		}

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

		protected virtual void OnCreateHandle()
		{
			this.OnHandleChange();
			if (this.revokeDragDrop)
			{
				NativeMethods.RevokeDragDrop(this.Control.Handle);
			}
		}

		private void OnDragEnter(object s, DragEventArgs e)
		{
			if (this.BehaviorService != null)
			{
				this.BehaviorService.StartDragNotification();
			}
			this.OnDragEnter(e);
		}

		protected virtual void OnDragEnter(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragEnter);
			control.DragEnter -= dragEventHandler;
			((IDropTarget)this.Control).OnDragEnter(de);
			control.DragEnter += dragEventHandler;
		}

		private void OnDragDrop(object s, DragEventArgs e)
		{
			if (this.BehaviorService != null)
			{
				this.BehaviorService.EndDragNotification();
			}
			this.OnDragDrop(e);
		}

		protected virtual void OnDragComplete(DragEventArgs de)
		{
		}

		protected virtual void OnDragDrop(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragDrop);
			control.DragDrop -= dragEventHandler;
			((IDropTarget)this.Control).OnDragDrop(de);
			control.DragDrop += dragEventHandler;
			this.OnDragComplete(de);
		}

		private void OnDragLeave(object s, EventArgs e)
		{
			this.OnDragLeave(e);
		}

		protected virtual void OnDragLeave(EventArgs e)
		{
			Control control = this.Control;
			EventHandler eventHandler = new EventHandler(this.OnDragLeave);
			control.DragLeave -= eventHandler;
			((IDropTarget)this.Control).OnDragLeave(e);
			control.DragLeave += eventHandler;
		}

		private void OnDragOver(object s, DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		protected virtual void OnDragOver(DragEventArgs de)
		{
			Control control = this.Control;
			DragEventHandler dragEventHandler = new DragEventHandler(this.OnDragOver);
			control.DragOver -= dragEventHandler;
			((IDropTarget)this.Control).OnDragOver(de);
			control.DragOver += dragEventHandler;
		}

		private void OnGiveFeedback(object s, GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
		}

		private void OnHandleChange()
		{
			this.HookChildHandles(NativeMethods.GetWindow(this.Control.Handle, 5));
			this.HookChildControls(this.Control);
		}

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

		protected virtual void OnPaintAdornments(PaintEventArgs pe)
		{
			if (this.inheritanceUI != null && pe.ClipRectangle.IntersectsWith(this.inheritanceUI.InheritanceGlyphRectangle))
			{
				pe.Graphics.DrawImage(this.inheritanceUI.InheritanceGlyph, 0, 0);
			}
		}

		private void OnParentChanged(object sender, EventArgs e)
		{
			if (this.Control.IsHandleCreated)
			{
				this.OnHandleChange();
			}
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			ComponentCache componentCache = (ComponentCache)this.GetService(typeof(ComponentCache));
			object component = base.Component;
			if (componentCache != null && component != null)
			{
				componentCache.RemoveEntry(component);
			}
		}

		private void OnLocationChanged(object sender, EventArgs e)
		{
			ComponentCache componentCache = (ComponentCache)this.GetService(typeof(ComponentCache));
			object component = base.Component;
			if (componentCache != null && component != null)
			{
				componentCache.RemoveEntry(component);
			}
		}

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

		private void ResetVisible()
		{
			this.Visible = true;
		}

		private void ResetEnabled()
		{
			this.Enabled = true;
		}

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

		private bool ShouldSerializeAllowDrop()
		{
			return this.AllowDrop != this.hadDragDrop;
		}

		private bool ShouldSerializeEnabled()
		{
			return base.ShadowProperties.ShouldSerializeValue("Enabled", true);
		}

		private bool ShouldSerializeVisible()
		{
			return base.ShadowProperties.ShouldSerializeValue("Visible", true);
		}

		private bool ShouldSerializeName()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!this.initializing)
			{
				return base.ShadowProperties.ShouldSerializeValue("Name", null);
			}
			return base.Component != designerHost.RootComponent;
		}

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

		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		private static int currentProcessId;

		private IDesignerHost host;

		private ControlDesigner.IDesignerTarget designerTarget;

		private bool liveRegion;

		private bool inHitTest;

		private bool hasLocation;

		private bool locationChecked;

		private bool locked;

		private bool initializing;

		private bool enabledchangerecursionguard;

		private BehaviorService behaviorService;

		private ResizeBehavior resizeBehavior;

		private ContainerSelectorBehavior moveBehavior;

		private ISelectionUIService selectionUISvc;

		private IEventHandlerService eventSvc;

		private IToolboxService toolboxSvc;

		private InheritanceUI inheritanceUI;

		private IOverlayService overlayService;

		private Point mouseDragLast = ControlDesigner.InvalidPoint;

		private bool mouseDragMoved;

		private int lastMoveScreenX;

		private int lastMoveScreenY;

		private int lastClickMessageTime;

		private int lastClickMessagePositionX;

		private int lastClickMessagePositionY;

		private Point downPos = Point.Empty;

		private CollectionChangeEventHandler dataBindingsCollectionChanged;

		private Exception thrownException;

		private bool ctrlSelect;

		private bool toolPassThrough;

		private bool removalNotificationHooked;

		private bool revokeDragDrop = true;

		private bool hadDragDrop;

		private ControlDesigner.DesignerControlCollection controls;

		private static bool inContextMenu = false;

		private ControlDesigner.DockingActionList dockingAction;

		private StatusCommandUI statusCommandUI;

		private bool forceVisible = true;

		private bool autoResizeHandles;

		private Dictionary<IntPtr, bool> subclassedChildren;

		protected AccessibleObject accessibilityObj;

		private interface IDesignerTarget : IDisposable
		{
			void DefWndProc(ref Message m);
		}

		private class ChildSubClass : NativeWindow, ControlDesigner.IDesignerTarget, IDisposable
		{
			public ChildSubClass(ControlDesigner designer, IntPtr hwnd)
			{
				this.designer = designer;
				if (designer != null)
				{
					designer.disposingHandler = (EventHandler)Delegate.Combine(designer.disposingHandler, new EventHandler(this.OnDesignerDisposing));
				}
				base.AssignHandle(hwnd);
			}

			void ControlDesigner.IDesignerTarget.DefWndProc(ref Message m)
			{
				base.DefWndProc(ref m);
			}

			public void Dispose()
			{
				this.designer = null;
			}

			private void OnDesignerDisposing(object sender, EventArgs e)
			{
				this.Dispose();
			}

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

			private ControlDesigner designer;
		}

		private class ChildWindowTarget : IWindowTarget, ControlDesigner.IDesignerTarget, IDisposable
		{
			public ChildWindowTarget(ControlDesigner designer, Control childControl, IWindowTarget oldWindowTarget)
			{
				this.designer = designer;
				this.childControl = childControl;
				this.oldWindowTarget = oldWindowTarget;
			}

			public IWindowTarget OldWindowTarget
			{
				get
				{
					return this.oldWindowTarget;
				}
			}

			public void DefWndProc(ref Message m)
			{
				this.oldWindowTarget.OnMessage(ref m);
			}

			public void Dispose()
			{
			}

			public void OnHandleChange(IntPtr newHandle)
			{
				this.handle = newHandle;
				this.oldWindowTarget.OnHandleChange(newHandle);
			}

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

			private ControlDesigner designer;

			private Control childControl;

			private IWindowTarget oldWindowTarget;

			private IntPtr handle = IntPtr.Zero;
		}

		private class DesignerWindowTarget : IWindowTarget, ControlDesigner.IDesignerTarget, IDisposable
		{
			public DesignerWindowTarget(ControlDesigner designer)
			{
				Control control = designer.Control;
				this.designer = designer;
				this.oldTarget = control.WindowTarget;
				control.WindowTarget = this;
			}

			public void DefWndProc(ref Message m)
			{
				this.oldTarget.OnMessage(ref m);
			}

			public void Dispose()
			{
				if (this.designer != null)
				{
					this.designer.Control.WindowTarget = this.oldTarget;
					this.designer = null;
				}
			}

			public void OnHandleChange(IntPtr newHandle)
			{
				this.oldTarget.OnHandleChange(newHandle);
				if (newHandle != IntPtr.Zero)
				{
					this.designer.OnHandleChange();
				}
			}

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

			internal ControlDesigner designer;

			internal IWindowTarget oldTarget;
		}

		[ComVisible(true)]
		public class ControlDesignerAccessibleObject : AccessibleObject
		{
			public ControlDesignerAccessibleObject(ControlDesigner designer, Control control)
			{
				this.designer = designer;
				this.control = control;
			}

			public override Rectangle Bounds
			{
				get
				{
					return this.control.AccessibilityObject.Bounds;
				}
			}

			public override string Description
			{
				get
				{
					return this.control.AccessibilityObject.Description;
				}
			}

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

			public override string DefaultAction
			{
				get
				{
					return "";
				}
			}

			public override string Name
			{
				get
				{
					return this.control.Name;
				}
			}

			public override AccessibleObject Parent
			{
				get
				{
					return this.control.AccessibilityObject.Parent;
				}
			}

			public override AccessibleRole Role
			{
				get
				{
					return this.control.AccessibilityObject.Role;
				}
			}

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

			public override string Value
			{
				get
				{
					return this.control.AccessibilityObject.Value;
				}
			}

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

			public override int GetChildCount()
			{
				return this.control.AccessibilityObject.GetChildCount();
			}

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

			public override AccessibleObject GetFocused()
			{
				if ((this.State & AccessibleStates.Focused) != AccessibleStates.None)
				{
					return this;
				}
				return base.GetFocused();
			}

			public override AccessibleObject GetSelected()
			{
				if ((this.State & AccessibleStates.Selected) != AccessibleStates.None)
				{
					return this;
				}
				return base.GetFocused();
			}

			public override AccessibleObject HitTest(int x, int y)
			{
				return this.control.AccessibilityObject.HitTest(x, y);
			}

			private ControlDesigner designer;

			private Control control;

			private IDesignerHost host;

			private ISelectionService selSvc;
		}

		[ListBindable(false)]
		[DesignerSerializer(typeof(ControlDesigner.DesignerControlCollectionCodeDomSerializer), typeof(CodeDomSerializer))]
		internal class DesignerControlCollection : Control.ControlCollection, IList, ICollection, IEnumerable
		{
			public DesignerControlCollection(Control owner)
				: base(owner)
			{
				this.realCollection = owner.Controls;
			}

			public override int Count
			{
				get
				{
					return this.realCollection.Count;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			public new bool IsReadOnly
			{
				get
				{
					return this.realCollection.IsReadOnly;
				}
			}

			int IList.Add(object control)
			{
				return ((IList)this.realCollection).Add(control);
			}

			public override void Add(Control c)
			{
				this.realCollection.Add(c);
			}

			public override void AddRange(Control[] controls)
			{
				this.realCollection.AddRange(controls);
			}

			bool IList.Contains(object control)
			{
				return ((IList)this.realCollection).Contains(control);
			}

			public new void CopyTo(Array dest, int index)
			{
				this.realCollection.CopyTo(dest, index);
			}

			public override bool Equals(object other)
			{
				return this.realCollection.Equals(other);
			}

			public new IEnumerator GetEnumerator()
			{
				return this.realCollection.GetEnumerator();
			}

			public override int GetHashCode()
			{
				return this.realCollection.GetHashCode();
			}

			int IList.IndexOf(object control)
			{
				return ((IList)this.realCollection).IndexOf(control);
			}

			void IList.Insert(int index, object value)
			{
				((IList)this.realCollection).Insert(index, value);
			}

			void IList.Remove(object control)
			{
				((IList)this.realCollection).Remove(control);
			}

			void IList.RemoveAt(int index)
			{
				((IList)this.realCollection).RemoveAt(index);
			}

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

			public override int GetChildIndex(Control child, bool throwException)
			{
				return this.realCollection.GetChildIndex(child, throwException);
			}

			public override void SetChildIndex(Control child, int newIndex)
			{
				this.realCollection.SetChildIndex(child, newIndex);
			}

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

			private Control.ControlCollection realCollection;
		}

		internal class DesignerControlCollectionCodeDomSerializer : CollectionCodeDomSerializer
		{
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

		private class DockingActionList : DesignerActionList
		{
			public DockingActionList(ControlDesigner owner)
				: base(owner.Component)
			{
				this._designer = owner;
				this._host = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
			}

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

			private ControlDesigner _designer;

			private IDesignerHost _host;
		}

		internal class TransparentBehavior : Behavior
		{
			internal TransparentBehavior(ControlDesigner designer)
			{
				this.designer = designer;
			}

			internal bool IsTransparent(Point p)
			{
				return this.designer.GetHitTest(p);
			}

			public override void OnDragDrop(Glyph g, DragEventArgs e)
			{
				this.controlRect = Rectangle.Empty;
				this.designer.OnDragDrop(e);
			}

			public override void OnDragEnter(Glyph g, DragEventArgs e)
			{
				if (this.designer != null && this.designer.Control != null)
				{
					this.controlRect = this.designer.Control.RectangleToScreen(this.designer.Control.ClientRectangle);
				}
				this.designer.OnDragEnter(e);
			}

			public override void OnDragLeave(Glyph g, EventArgs e)
			{
				this.controlRect = Rectangle.Empty;
				this.designer.OnDragLeave(e);
			}

			public override void OnDragOver(Glyph g, DragEventArgs e)
			{
				if (e != null && this.controlRect != Rectangle.Empty && !this.controlRect.Contains(new Point(e.X, e.Y)))
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				this.designer.OnDragOver(e);
			}

			public override void OnGiveFeedback(Glyph g, GiveFeedbackEventArgs e)
			{
				this.designer.OnGiveFeedback(e);
			}

			private ControlDesigner designer;

			private Rectangle controlRect = Rectangle.Empty;
		}

		private class CanResetSizePropertyDescriptor : PropertyDescriptor
		{
			public CanResetSizePropertyDescriptor(PropertyDescriptor pd)
				: base(pd)
			{
				this._basePropDesc = pd;
			}

			public override Type ComponentType
			{
				get
				{
					return this._basePropDesc.ComponentType;
				}
			}

			public override string DisplayName
			{
				get
				{
					return this._basePropDesc.DisplayName;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this._basePropDesc.IsReadOnly;
				}
			}

			public override Type PropertyType
			{
				get
				{
					return this._basePropDesc.PropertyType;
				}
			}

			public override bool CanResetValue(object component)
			{
				return this._basePropDesc.ShouldSerializeValue(component);
			}

			public override object GetValue(object component)
			{
				return this._basePropDesc.GetValue(component);
			}

			public override void ResetValue(object component)
			{
				this._basePropDesc.ResetValue(component);
			}

			public override void SetValue(object component, object value)
			{
				this._basePropDesc.SetValue(component, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return true;
			}

			private PropertyDescriptor _basePropDesc;
		}
	}
}

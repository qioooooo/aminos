using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	public class ParentControlDesigner : ControlDesigner, IOleDragClient
	{
		protected virtual bool AllowControlLasso
		{
			get
			{
				return true;
			}
		}

		protected virtual bool AllowGenericDragBox
		{
			get
			{
				return true;
			}
		}

		protected internal virtual bool AllowSetChildIndexOnDrop
		{
			get
			{
				return true;
			}
		}

		private Size CurrentGridSize
		{
			get
			{
				return this.GridSize;
			}
		}

		protected virtual Point DefaultControlLocation
		{
			get
			{
				return new Point(0, 0);
			}
		}

		private bool DefaultUseSnapLines
		{
			get
			{
				if (this.checkSnapLineSetting)
				{
					this.checkSnapLineSetting = false;
					this.defaultUseSnapLines = DesignerUtils.UseSnapLines(base.Component.Site);
				}
				return this.defaultUseSnapLines;
			}
		}

		protected virtual bool DrawGrid
		{
			get
			{
				if (this.DefaultUseSnapLines)
				{
					return false;
				}
				if (this.getDefaultDrawGrid)
				{
					this.drawGrid = true;
					ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
					if (parentControlDesignerOfParent != null)
					{
						this.drawGrid = parentControlDesignerOfParent.DrawGrid;
					}
					else
					{
						object optionValue = DesignerUtils.GetOptionValue(this.ServiceProvider, "ShowGrid");
						if (optionValue is bool)
						{
							this.drawGrid = (bool)optionValue;
						}
					}
				}
				return this.drawGrid;
			}
			set
			{
				if (value != this.drawGrid)
				{
					if (this.parentCanSetDrawGrid)
					{
						this.parentCanSetDrawGrid = false;
					}
					if (this.getDefaultDrawGrid)
					{
						this.getDefaultDrawGrid = false;
					}
					this.drawGrid = value;
					Control control = this.Control;
					if (control != null)
					{
						control.Invalidate(true);
					}
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						foreach (object obj in this.Control.Controls)
						{
							Control control2 = (Control)obj;
							ParentControlDesigner parentControlDesigner = designerHost.GetDesigner(control2) as ParentControlDesigner;
							if (parentControlDesigner != null)
							{
								parentControlDesigner.DrawGridOfParentChanged(this.drawGrid);
							}
						}
					}
				}
			}
		}

		protected override bool EnableDragRect
		{
			get
			{
				return true;
			}
		}

		internal Size ParentGridSize
		{
			get
			{
				return this.GridSize;
			}
		}

		protected Size GridSize
		{
			get
			{
				if (this.getDefaultGridSize)
				{
					this.gridSize = new Size(8, 8);
					ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
					if (parentControlDesignerOfParent != null)
					{
						this.gridSize = parentControlDesignerOfParent.GridSize;
					}
					else
					{
						object optionValue = DesignerUtils.GetOptionValue(this.ServiceProvider, "GridSize");
						if (optionValue is Size)
						{
							this.gridSize = (Size)optionValue;
						}
					}
				}
				return this.gridSize;
			}
			set
			{
				if (this.parentCanSetGridSize)
				{
					this.parentCanSetGridSize = false;
				}
				if (this.getDefaultGridSize)
				{
					this.getDefaultGridSize = false;
				}
				if (value.Width < 2 || value.Height < 2 || value.Width > 200 || value.Height > 200)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"GridSize",
						value.ToString()
					}));
				}
				this.gridSize = value;
				Control control = this.Control;
				if (control != null)
				{
					control.Invalidate(true);
				}
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					foreach (object obj in this.Control.Controls)
					{
						Control control2 = (Control)obj;
						ParentControlDesigner parentControlDesigner = designerHost.GetDesigner(control2) as ParentControlDesigner;
						if (parentControlDesigner != null)
						{
							parentControlDesigner.GridSizeOfParentChanged(this.gridSize);
						}
					}
				}
			}
		}

		protected ToolboxItem MouseDragTool
		{
			get
			{
				return this.mouseDragTool;
			}
		}

		protected virtual Control GetParentForComponent(IComponent component)
		{
			return this.Control;
		}

		protected void AddPaddingSnapLines(ref ArrayList snapLines)
		{
			if (snapLines == null)
			{
				snapLines = new ArrayList(4);
			}
			Point offsetToClientArea = base.GetOffsetToClientArea();
			Rectangle displayRectangle = this.Control.DisplayRectangle;
			displayRectangle.X += offsetToClientArea.X;
			displayRectangle.Y += offsetToClientArea.Y;
			snapLines.Add(new SnapLine(SnapLineType.Vertical, displayRectangle.Left, "Padding.Left", SnapLinePriority.Always));
			snapLines.Add(new SnapLine(SnapLineType.Vertical, displayRectangle.Right, "Padding.Right", SnapLinePriority.Always));
			snapLines.Add(new SnapLine(SnapLineType.Horizontal, displayRectangle.Top, "Padding.Top", SnapLinePriority.Always));
			snapLines.Add(new SnapLine(SnapLineType.Horizontal, displayRectangle.Bottom, "Padding.Bottom", SnapLinePriority.Always));
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				if (arrayList == null)
				{
					arrayList = new ArrayList(4);
				}
				this.AddPaddingSnapLines(ref arrayList);
				return arrayList;
			}
		}

		private IServiceProvider ServiceProvider
		{
			get
			{
				if (base.Component != null)
				{
					return base.Component.Site;
				}
				return null;
			}
		}

		private bool SnapToGrid
		{
			get
			{
				if (this.DefaultUseSnapLines)
				{
					return false;
				}
				if (this.getDefaultGridSnap)
				{
					this.gridSnap = true;
					ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
					if (parentControlDesignerOfParent != null)
					{
						this.gridSnap = parentControlDesignerOfParent.SnapToGrid;
					}
					else
					{
						object optionValue = DesignerUtils.GetOptionValue(this.ServiceProvider, "SnapToGrid");
						if (optionValue != null && optionValue is bool)
						{
							this.gridSnap = (bool)optionValue;
						}
					}
				}
				return this.gridSnap;
			}
			set
			{
				if (this.gridSnap != value)
				{
					if (this.parentCanSetGridSnap)
					{
						this.parentCanSetGridSnap = false;
					}
					if (this.getDefaultGridSnap)
					{
						this.getDefaultGridSnap = false;
					}
					this.gridSnap = value;
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						foreach (object obj in this.Control.Controls)
						{
							Control control = (Control)obj;
							ParentControlDesigner parentControlDesigner = designerHost.GetDesigner(control) as ParentControlDesigner;
							if (parentControlDesigner != null)
							{
								parentControlDesigner.GridSnapOfParentChanged(this.gridSnap);
							}
						}
					}
				}
			}
		}

		internal static int DetermineTopChildIndex(Control parent)
		{
			int i;
			for (i = 0; i < parent.Controls.Count - 1; i++)
			{
				Control control = parent.Controls[i];
				if (control.Site != null)
				{
					InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control)[typeof(InheritanceAttribute)];
					InheritanceLevel inheritanceLevel = InheritanceLevel.NotInherited;
					if (inheritanceAttribute != null)
					{
						inheritanceLevel = inheritanceAttribute.InheritanceLevel;
					}
					if (inheritanceLevel == InheritanceLevel.NotInherited)
					{
						break;
					}
				}
			}
			return i;
		}

		internal virtual void AddChildControl(Control newChild)
		{
			if (newChild.Left == 0 && newChild.Top == 0 && newChild.Width >= this.Control.Width && newChild.Height >= this.Control.Height)
			{
				Point location = newChild.Location;
				location.Offset(this.GridSize.Width, this.GridSize.Height);
				newChild.Location = location;
			}
			this.Control.Controls.Add(newChild);
			int num = ParentControlDesigner.DetermineTopChildIndex(this.Control);
			this.Control.Controls.SetChildIndex(newChild, num);
		}

		internal void AddControl(Control newChild, IDictionary defaultValues)
		{
			Point point = Point.Empty;
			Size size = Size.Empty;
			Size size2 = new Size(0, 0);
			bool flag = defaultValues != null && defaultValues.Contains("Location");
			bool flag2 = defaultValues != null && defaultValues.Contains("Size");
			if (flag)
			{
				point = (Point)defaultValues["Location"];
			}
			if (flag2)
			{
				size = (Size)defaultValues["Size"];
			}
			if (defaultValues != null && defaultValues.Contains("Offset"))
			{
				size2 = (Size)defaultValues["Offset"];
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null && newChild != null && !this.Control.Contains(newChild) && designerHost.GetDesigner(newChild) is ControlDesigner && (!(newChild is Form) || !((Form)newChild).TopLevel))
			{
				Rectangle rectangle = default(Rectangle);
				if (flag)
				{
					point = this.Control.PointToClient(point);
					rectangle.X = point.X;
					rectangle.Y = point.Y;
				}
				else
				{
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					object primarySelection = selectionService.PrimarySelection;
					Control control = null;
					if (primarySelection != null)
					{
						control = ((IOleDragClient)this).GetControlForComponent(primarySelection);
					}
					if (control != null && control.Site == null)
					{
						control = null;
					}
					if (primarySelection == base.Component || control == null)
					{
						rectangle.X = this.DefaultControlLocation.X;
						rectangle.Y = this.DefaultControlLocation.Y;
					}
					else
					{
						rectangle.X = control.Location.X + this.GridSize.Width;
						rectangle.Y = control.Location.Y + this.GridSize.Height;
					}
				}
				if (flag2)
				{
					rectangle.Width = size.Width;
					rectangle.Height = size.Height;
				}
				else
				{
					rectangle.Size = this.GetDefaultSize(newChild);
				}
				if (!flag2 && !flag)
				{
					Rectangle rectangle2 = this.GetAdjustedSnapLocation(Rectangle.Empty, rectangle);
					rectangle2 = this.GetControlStackLocation(rectangle2);
					rectangle = rectangle2;
				}
				else
				{
					rectangle = this.GetAdjustedSnapLocation(Rectangle.Empty, rectangle);
				}
				rectangle.X += size2.Width;
				rectangle.Y += size2.Height;
				if (defaultValues != null && defaultValues.Contains("ToolboxSnapDragDropEventArgs"))
				{
					ToolboxSnapDragDropEventArgs toolboxSnapDragDropEventArgs = defaultValues["ToolboxSnapDragDropEventArgs"] as ToolboxSnapDragDropEventArgs;
					Rectangle boundsFromToolboxSnapDragDropInfo = DesignerUtils.GetBoundsFromToolboxSnapDragDropInfo(toolboxSnapDragDropEventArgs, rectangle, this.Control.IsMirrored);
					Control control2 = designerHost.RootComponent as Control;
					if (control2 != null && boundsFromToolboxSnapDragDropInfo.IntersectsWith(control2.ClientRectangle))
					{
						rectangle = boundsFromToolboxSnapDragDropInfo;
					}
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Control)["Controls"];
				if (this.componentChangeSvc != null)
				{
					this.componentChangeSvc.OnComponentChanging(this.Control, propertyDescriptor);
				}
				this.AddChildControl(newChild);
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(newChild);
				if (properties != null)
				{
					PropertyDescriptor propertyDescriptor2 = properties["Size"];
					if (propertyDescriptor2 != null)
					{
						propertyDescriptor2.SetValue(newChild, new Size(rectangle.Width, rectangle.Height));
					}
					Point point2 = new Point(rectangle.X, rectangle.Y);
					ScrollableControl scrollableControl = newChild.Parent as ScrollableControl;
					if (scrollableControl != null)
					{
						Point autoScrollPosition = scrollableControl.AutoScrollPosition;
						point2.Offset(-autoScrollPosition.X, -autoScrollPosition.Y);
					}
					propertyDescriptor2 = properties["Location"];
					if (propertyDescriptor2 != null)
					{
						propertyDescriptor2.SetValue(newChild, point2);
					}
				}
				if (this.componentChangeSvc != null)
				{
					this.componentChangeSvc.OnComponentChanged(this.Control, propertyDescriptor, this.Control.Controls, this.Control.Controls);
				}
				newChild.Update();
			}
		}

		private void AddChildComponents(IComponent component, IContainer container, IDesignerHost host)
		{
			Control control = this.GetControl(component);
			if (control != null)
			{
				Control control2 = control;
				Control[] array = new Control[control2.Controls.Count];
				control2.Controls.CopyTo(array, 0);
				host.GetDesigner(component);
				for (int i = 0; i < array.Length; i++)
				{
					ISite site = ((IComponent)array[i]).Site;
					if (site != null)
					{
						string text = site.Name;
						if (container.Components[text] != null)
						{
							text = null;
						}
						IContainer container2 = site.Container;
						if (container2 != null)
						{
							container2.Remove(array[i]);
						}
						if (text != null)
						{
							container.Add(array[i], text);
						}
						else
						{
							container.Add(array[i]);
						}
						if (array[i].Parent != control2)
						{
							control2.Controls.Add(array[i]);
						}
						else
						{
							int childIndex = control2.Controls.GetChildIndex(array[i]);
							control2.Controls.Remove(array[i]);
							control2.Controls.Add(array[i]);
							control2.Controls.SetChildIndex(array[i], childIndex);
						}
						IComponentInitializer componentInitializer = host.GetDesigner(component) as IComponentInitializer;
						if (componentInitializer != null)
						{
							componentInitializer.InitializeExistingComponent(null);
						}
						this.AddChildComponents(array[i], container, host);
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.OnMouseDragEnd(false);
				base.EnableDragDrop(false);
				if (this.Control is ScrollableControl)
				{
					((ScrollableControl)this.Control).Scroll -= this.OnScroll;
				}
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					this.componentChangeSvc.ComponentRemoving -= this.OnComponentRemoving;
					this.componentChangeSvc.ComponentRemoved -= this.OnComponentRemoved;
					this.componentChangeSvc = null;
				}
			}
			base.Dispose(disposing);
		}

		private void DrawGridOfParentChanged(bool drawGrid)
		{
			if (this.parentCanSetDrawGrid)
			{
				bool flag = this.getDefaultDrawGrid;
				this.DrawGrid = drawGrid;
				this.parentCanSetDrawGrid = true;
				this.getDefaultDrawGrid = flag;
			}
		}

		private void GridSizeOfParentChanged(Size gridSize)
		{
			if (this.parentCanSetGridSize)
			{
				bool flag = this.getDefaultGridSize;
				this.GridSize = gridSize;
				this.parentCanSetGridSize = true;
				this.getDefaultGridSize = flag;
			}
		}

		private void GridSnapOfParentChanged(bool gridSnap)
		{
			if (this.parentCanSetGridSnap)
			{
				bool flag = this.getDefaultGridSnap;
				this.SnapToGrid = gridSnap;
				this.parentCanSetGridSnap = true;
				this.getDefaultGridSnap = flag;
			}
		}

		protected static void InvokeCreateTool(ParentControlDesigner toInvoke, ToolboxItem tool)
		{
			toInvoke.CreateTool(tool);
		}

		public virtual bool CanParent(ControlDesigner controlDesigner)
		{
			return this.CanParent(controlDesigner.Control);
		}

		public virtual bool CanParent(Control control)
		{
			return !control.Contains(this.Control);
		}

		protected void CreateTool(ToolboxItem tool)
		{
			this.CreateToolCore(tool, 0, 0, 0, 0, false, false);
		}

		protected void CreateTool(ToolboxItem tool, Point location)
		{
			this.CreateToolCore(tool, location.X, location.Y, 0, 0, true, false);
		}

		protected void CreateTool(ToolboxItem tool, Rectangle bounds)
		{
			this.CreateToolCore(tool, bounds.X, bounds.Y, bounds.Width, bounds.Height, true, true);
		}

		protected virtual IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			IComponent[] array = null;
			try
			{
				array = this.GetOleDragHandler().CreateTool(tool, this.Control, x, y, width, height, hasLocation, hasSize, this.toolboxSnapDragDropEventArgs);
			}
			finally
			{
				this.toolboxSnapDragDropEventArgs = null;
			}
			return array;
		}

		private SnapLine[] GenerateNewToolSnapLines(Rectangle r)
		{
			return new SnapLine[]
			{
				new SnapLine(SnapLineType.Left, r.Right),
				new SnapLine(SnapLineType.Right, r.Right),
				new SnapLine(SnapLineType.Bottom, r.Bottom),
				new SnapLine(SnapLineType.Top, r.Bottom)
			};
		}

		internal object[] GetComponentsInRect(Rectangle value, bool screenCoords, bool containRect)
		{
			ArrayList arrayList = new ArrayList();
			Rectangle rectangle = (screenCoords ? this.Control.RectangleToClient(value) : value);
			IContainer container = base.Component.Site.Container;
			Control control = this.Control;
			int count = control.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				Control control2 = control.Controls[i];
				Rectangle bounds = control2.Bounds;
				container = DesignerUtils.CheckForNestedContainer(container);
				if (control2.Visible && ((containRect && rectangle.Contains(bounds)) || (!containRect && bounds.IntersectsWith(rectangle))) && control2.Site != null && control2.Site.Container == container)
				{
					arrayList.Add(control2);
				}
			}
			return arrayList.ToArray();
		}

		protected Control GetControl(object component)
		{
			IComponent component2 = component as IComponent;
			if (component2 != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(component2) as ControlDesigner;
					if (controlDesigner != null)
					{
						return controlDesigner.Control;
					}
				}
			}
			return null;
		}

		private Rectangle GetControlStackLocation(Rectangle centeredLocation)
		{
			Control control = this.Control;
			int height = control.ClientSize.Height;
			int width = control.ClientSize.Width;
			if (centeredLocation.Bottom >= height || centeredLocation.Right >= width)
			{
				centeredLocation.X = this.DefaultControlLocation.X;
				centeredLocation.Y = this.DefaultControlLocation.Y;
			}
			return centeredLocation;
		}

		private Size GetDefaultSize(IComponent component)
		{
			Size size = Size.Empty;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["AutoSize"];
			if (propertyDescriptor != null && !propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden) && !propertyDescriptor.Attributes.Contains(BrowsableAttribute.No))
			{
				bool flag = (bool)propertyDescriptor.GetValue(component);
				if (flag)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(component)["PreferredSize"];
					if (propertyDescriptor != null)
					{
						size = (Size)propertyDescriptor.GetValue(component);
						if (size != Size.Empty)
						{
							return size;
						}
					}
				}
			}
			propertyDescriptor = TypeDescriptor.GetProperties(component)["Size"];
			if (propertyDescriptor != null)
			{
				size = (Size)propertyDescriptor.GetValue(component);
				if (size.Width > 0 && size.Height > 0)
				{
					return size;
				}
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)propertyDescriptor.Attributes[typeof(DefaultValueAttribute)];
				if (defaultValueAttribute != null)
				{
					return (Size)defaultValueAttribute.Value;
				}
			}
			return new Size(75, 23);
		}

		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			this.OnSetCursor();
			Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(this.Control);
			Control parent = this.Control.Parent;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (parent != null && designerHost != null && designerHost.RootComponent != base.Component)
			{
				Rectangle rectangle2 = base.BehaviorService.ControlRectInAdornerWindow(parent);
				Rectangle rectangle3 = Rectangle.Intersect(rectangle2, rectangle);
				if (selectionType == GlyphSelectionType.NotSelected)
				{
					if (!rectangle3.IsEmpty && !rectangle2.Contains(rectangle))
					{
						return new ControlBodyGlyph(rectangle3, Cursor.Current, this.Control, this);
					}
					if (rectangle3.IsEmpty)
					{
						return null;
					}
				}
			}
			return new ControlBodyGlyph(rectangle, Cursor.Current, this.Control, this);
		}

		public override GlyphCollection GetGlyphs(GlyphSelectionType selectionType)
		{
			GlyphCollection glyphs = base.GetGlyphs(selectionType);
			if ((this.SelectionRules & SelectionRules.Moveable) != SelectionRules.None && this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly && selectionType != GlyphSelectionType.NotSelected)
			{
				Point point = base.BehaviorService.ControlToAdornerWindow((Control)base.Component);
				Rectangle rectangle = new Rectangle(point, ((Control)base.Component).Size);
				int num = (int)((double)DesignerUtils.CONTAINERGRABHANDLESIZE * 0.5);
				if (rectangle.Width < 2 * DesignerUtils.CONTAINERGRABHANDLESIZE)
				{
					num = -1 * num;
				}
				ContainerSelectorBehavior containerSelectorBehavior = new ContainerSelectorBehavior((Control)base.Component, base.Component.Site, true);
				ContainerSelectorGlyph containerSelectorGlyph = new ContainerSelectorGlyph(rectangle, DesignerUtils.CONTAINERGRABHANDLESIZE, num, containerSelectorBehavior);
				glyphs.Insert(0, containerSelectorGlyph);
			}
			return glyphs;
		}

		internal OleDragDropHandler GetOleDragHandler()
		{
			if (this.oleDragDropHandler == null)
			{
				this.oleDragDropHandler = new OleDragDropHandler(null, (IServiceProvider)this.GetService(typeof(IDesignerHost)), this);
			}
			return this.oleDragDropHandler;
		}

		private ParentControlDesigner GetParentControlDesignerOfParent()
		{
			Control parent = this.Control.Parent;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (parent != null && designerHost != null)
			{
				return designerHost.GetDesigner(parent) as ParentControlDesigner;
			}
			return null;
		}

		private Rectangle GetAdjustedSnapLocation(Rectangle originalRect, Rectangle dragRect)
		{
			Rectangle updatedRect = this.GetUpdatedRect(originalRect, dragRect, true);
			updatedRect.Width = dragRect.Width;
			updatedRect.Height = dragRect.Height;
			Point defaultControlLocation = this.DefaultControlLocation;
			if (updatedRect.X < defaultControlLocation.X)
			{
				updatedRect.X = defaultControlLocation.X;
			}
			if (updatedRect.Y < defaultControlLocation.Y)
			{
				updatedRect.Y = defaultControlLocation.Y;
			}
			return updatedRect;
		}

		internal Point GetSnappedPoint(Point pt)
		{
			Rectangle updatedRect = this.GetUpdatedRect(Rectangle.Empty, new Rectangle(pt.X, pt.Y, 0, 0), false);
			return new Point(updatedRect.X, updatedRect.Y);
		}

		internal Rectangle GetSnappedRect(Rectangle originalRect, Rectangle dragRect, bool updateSize)
		{
			return this.GetUpdatedRect(originalRect, dragRect, updateSize);
		}

		protected Rectangle GetUpdatedRect(Rectangle originalRect, Rectangle dragRect, bool updateSize)
		{
			Rectangle rectangle = Rectangle.Empty;
			if (this.SnapToGrid)
			{
				Size size = this.GridSize;
				Point point = new Point(size.Width / 2, size.Height / 2);
				rectangle = dragRect;
				int y = dragRect.Y;
				int height = dragRect.Height;
				int x = dragRect.X;
				int width = dragRect.Width;
				rectangle.X = originalRect.X;
				rectangle.Y = originalRect.Y;
				if (dragRect.X != originalRect.X)
				{
					rectangle.X = dragRect.X / size.Width * size.Width;
					if (dragRect.X - rectangle.X > point.X)
					{
						rectangle.X += size.Width;
					}
				}
				if (dragRect.Y != originalRect.Y)
				{
					rectangle.Y = dragRect.Y / size.Height * size.Height;
					if (dragRect.Y - rectangle.Y > point.Y)
					{
						rectangle.Y += size.Height;
					}
				}
				if (updateSize)
				{
					rectangle.Width = (dragRect.X + dragRect.Width) / size.Width * size.Width - rectangle.X;
					rectangle.Height = (dragRect.Y + dragRect.Height) / size.Height * size.Height - rectangle.Y;
					if (rectangle.Width < size.Width)
					{
						rectangle.Width = size.Width;
					}
					if (rectangle.Height < size.Height)
					{
						rectangle.Height = size.Height;
					}
				}
			}
			else
			{
				rectangle = dragRect;
			}
			return rectangle;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (this.Control is ScrollableControl)
			{
				((ScrollableControl)this.Control).Scroll += this.OnScroll;
			}
			base.EnableDragDrop(true);
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				this.componentChangeSvc = (IComponentChangeService)designerHost.GetService(typeof(IComponentChangeService));
				if (this.componentChangeSvc != null)
				{
					this.componentChangeSvc.ComponentRemoving += this.OnComponentRemoving;
					this.componentChangeSvc.ComponentRemoved += this.OnComponentRemoved;
				}
			}
			this.statusCommandUI = new StatusCommandUI(component.Site);
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			if (!this.AllowControlLasso)
			{
				return;
			}
			if (defaultValues != null && defaultValues["Size"] != null && defaultValues["Location"] != null && defaultValues["Parent"] != null)
			{
				Rectangle rectangle = new Rectangle((Point)defaultValues["Location"], (Size)defaultValues["Size"]);
				IComponent component = defaultValues["Parent"] as IComponent;
				if (component == null)
				{
					return;
				}
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost == null)
				{
					return;
				}
				ParentControlDesigner parentControlDesigner = designerHost.GetDesigner(component) as ParentControlDesigner;
				if (parentControlDesigner == null)
				{
					return;
				}
				object[] componentsInRect = parentControlDesigner.GetComponentsInRect(rectangle, true, true);
				if (componentsInRect == null || componentsInRect.Length == 0)
				{
					return;
				}
				ArrayList arrayList = new ArrayList(componentsInRect);
				if (arrayList.Contains(this.Control))
				{
					arrayList.Remove(this.Control);
				}
				this.ReParentControls(this.Control, arrayList, SR.GetString("ParentControlDesignerLassoShortcutRedo", new object[] { this.Control.Site.Name }), designerHost);
			}
		}

		private bool IsOptionDefault(string optionName, object value)
		{
			IDesignerOptionService designerOptionService = (IDesignerOptionService)this.GetService(typeof(IDesignerOptionService));
			object obj = null;
			if (designerOptionService == null)
			{
				if (optionName.Equals("ShowGrid"))
				{
					obj = true;
				}
				else if (optionName.Equals("SnapToGrid"))
				{
					obj = true;
				}
				else if (optionName.Equals("GridSize"))
				{
					obj = new Size(8, 8);
				}
			}
			else
			{
				obj = DesignerUtils.GetOptionValue(this.ServiceProvider, optionName);
			}
			if (obj != null)
			{
				return obj.Equals(value);
			}
			return value == null;
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.Parent != null && control.Parent == this.Control)
			{
				this.pendingRemoveControl = control;
				if (this.suspendChanging == 0)
				{
					this.componentChangeSvc.OnComponentChanging(this.Control, TypeDescriptor.GetProperties(this.Control)["Controls"]);
				}
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component == this.pendingRemoveControl)
			{
				this.pendingRemoveControl = null;
				this.componentChangeSvc.OnComponentChanged(this.Control, TypeDescriptor.GetProperties(this.Control)["Controls"], null, null);
			}
		}

		internal void SuspendChangingEvents()
		{
			this.suspendChanging++;
		}

		internal void ResumeChangingEvents()
		{
			this.suspendChanging--;
		}

		internal void ForceComponentChanging()
		{
			this.componentChangeSvc.OnComponentChanging(this.Control, TypeDescriptor.GetProperties(this.Control)["Controls"]);
		}

		protected override void OnDragComplete(DragEventArgs de)
		{
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				behaviorDataObject.CleanupDrag();
			}
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			if (de is ToolboxSnapDragDropEventArgs)
			{
				this.toolboxSnapDragDropEventArgs = de as ToolboxSnapDragDropEventArgs;
			}
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				behaviorDataObject.Target = base.Component;
				behaviorDataObject.EndDragDrop(this.AllowSetChildIndexOnDrop);
				this.OnDragComplete(de);
			}
			else if (this.mouseDragTool == null && behaviorDataObject == null)
			{
				OleDragDropHandler oleDragHandler = this.GetOleDragHandler();
				if (oleDragHandler != null)
				{
					IOleDragClient destination = oleDragHandler.Destination;
					if (destination != null && destination.Component != null && destination.Component.Site != null)
					{
						IContainer container = destination.Component.Site.Container;
						if (container != null)
						{
							object[] draggingObjects = oleDragHandler.GetDraggingObjects(de);
							for (int i = 0; i < draggingObjects.Length; i++)
							{
								IComponent component = draggingObjects[i] as IComponent;
								container.Add(component);
							}
						}
					}
				}
			}
			if (this.mouseDragTool != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.Activate();
				}
				try
				{
					if (base.BehaviorService != null)
					{
						base.BehaviorService.EndDragNotification();
					}
					this.CreateTool(this.mouseDragTool, new Point(de.X, de.Y));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					base.DisplayError(ex);
				}
				this.mouseDragTool = null;
			}
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			bool flag = false;
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = null;
			DropSourceBehavior.BehaviorDataObject behaviorDataObject2 = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject2 != null)
			{
				behaviorDataObject = behaviorDataObject2;
				behaviorDataObject.Target = base.Component;
				de.Effect = ((Control.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
				flag = !behaviorDataObject2.Source.Equals(base.Component);
			}
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				MenuCommand menuCommand = menuCommandService.FindCommand(StandardCommands.TabOrder);
				if (menuCommand != null && menuCommand.Checked)
				{
					de.Effect = DragDropEffects.None;
					return;
				}
			}
			object[] array;
			if (behaviorDataObject != null && behaviorDataObject.DragComponents != null)
			{
				array = new object[behaviorDataObject.DragComponents.Count];
				behaviorDataObject.DragComponents.CopyTo(array, 0);
			}
			else
			{
				OleDragDropHandler oleDragHandler = this.GetOleDragHandler();
				array = oleDragHandler.GetDraggingObjects(de);
			}
			Control control = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				DocumentDesigner documentDesigner = designerHost.GetDesigner(designerHost.RootComponent) as DocumentDesigner;
				if (documentDesigner != null && !documentDesigner.CanDropComponents(de))
				{
					de.Effect = DragDropEffects.None;
					return;
				}
			}
			if (array != null)
			{
				if (behaviorDataObject2 == null)
				{
					flag = true;
				}
				for (int i = 0; i < array.Length; i++)
				{
					IComponent component = array[i] as IComponent;
					if (designerHost != null && component != null)
					{
						if (flag)
						{
							InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(component)[typeof(InheritanceAttribute)];
							if (inheritanceAttribute != null && !inheritanceAttribute.Equals(InheritanceAttribute.NotInherited) && !inheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
							{
								de.Effect = DragDropEffects.None;
								return;
							}
						}
						object designer = designerHost.GetDesigner(component);
						if (designer is IOleDragClient)
						{
							control = ((IOleDragClient)this).GetControlForComponent(array[i]);
						}
						Control control2 = array[i] as Control;
						if (control == null && control2 != null)
						{
							control = control2;
						}
						if (control != null)
						{
							if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly && control.Parent != this.Control)
							{
								de.Effect = DragDropEffects.None;
								return;
							}
							if (!((IOleDragClient)this).IsDropOk(component))
							{
								de.Effect = DragDropEffects.None;
								return;
							}
						}
					}
				}
				if (behaviorDataObject2 == null)
				{
					this.PerformDragEnter(de, designerHost);
				}
			}
			if (this.toolboxService == null)
			{
				this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			}
			if (this.toolboxService != null && array == null)
			{
				this.mouseDragTool = this.toolboxService.DeserializeToolboxItem(de.Data, designerHost);
				if (this.mouseDragTool != null && base.BehaviorService != null && base.BehaviorService.UseSnapLines)
				{
					if (this.toolboxItemSnapLineBehavior == null)
					{
						this.toolboxItemSnapLineBehavior = new ToolboxItemSnapLineBehavior(base.Component.Site, base.BehaviorService, this, this.AllowGenericDragBox);
					}
					if (!this.toolboxItemSnapLineBehavior.IsPushed)
					{
						base.BehaviorService.PushBehavior(this.toolboxItemSnapLineBehavior);
						this.toolboxItemSnapLineBehavior.IsPushed = true;
					}
				}
				if (this.mouseDragTool != null)
				{
					this.PerformDragEnter(de, designerHost);
				}
				if (this.toolboxItemSnapLineBehavior != null)
				{
					this.toolboxItemSnapLineBehavior.OnBeginDrag();
				}
			}
		}

		private void PerformDragEnter(DragEventArgs de, IDesignerHost host)
		{
			if (host != null)
			{
				host.Activate();
			}
			if ((de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None)
			{
				de.Effect = DragDropEffects.Move;
			}
			else
			{
				de.Effect = DragDropEffects.Copy;
			}
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Replace);
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if (this.toolboxItemSnapLineBehavior != null && this.toolboxItemSnapLineBehavior.IsPushed)
			{
				base.BehaviorService.PopBehavior(this.toolboxItemSnapLineBehavior);
				this.toolboxItemSnapLineBehavior.IsPushed = false;
			}
			this.mouseDragTool = null;
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				behaviorDataObject.Target = base.Component;
				de.Effect = ((Control.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
			}
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				MenuCommand menuCommand = menuCommandService.FindCommand(StandardCommands.TabOrder);
				if (menuCommand != null && menuCommand.Checked)
				{
					de.Effect = DragDropEffects.None;
					return;
				}
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				DocumentDesigner documentDesigner = designerHost.GetDesigner(designerHost.RootComponent) as DocumentDesigner;
				if (documentDesigner != null && !documentDesigner.CanDropComponents(de))
				{
					de.Effect = DragDropEffects.None;
					return;
				}
			}
			if (this.mouseDragTool != null)
			{
				de.Effect = DragDropEffects.Copy;
			}
		}

		private static int FrameWidth(FrameStyle style)
		{
			if (style != FrameStyle.Dashed)
			{
				return 2;
			}
			return 1;
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			Control control = this.Control;
			if (!this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
			{
				if (this.toolboxService == null)
				{
					this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
				}
				if (this.toolboxService != null)
				{
					this.mouseDragTool = this.toolboxService.GetSelectedToolboxItem((IDesignerHost)this.GetService(typeof(IDesignerHost)));
				}
			}
			control.Capture = true;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			NativeMethods.GetWindowRect(control.Handle, ref rect);
			Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			this.mouseDragFrame = ((this.mouseDragTool == null) ? FrameStyle.Dashed : FrameStyle.Thick);
			this.mouseDragBase = new Point(x, y);
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Click);
			}
			IEventHandlerService eventHandlerService = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
			if (eventHandlerService != null && this.escapeHandler == null)
			{
				this.escapeHandler = new ParentControlDesigner.EscapeHandler(this);
				eventHandlerService.PushHandler(this.escapeHandler);
			}
			this.adornerWindowToScreenOffset = base.BehaviorService.AdornerWindowToScreen();
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			if (this.mouseDragBase == ControlDesigner.InvalidPoint)
			{
				base.OnMouseDragEnd(cancel);
				return;
			}
			Rectangle rectangle = this.mouseDragOffset;
			ToolboxItem toolboxItem = this.mouseDragTool;
			Point point = this.mouseDragBase;
			this.mouseDragOffset = Rectangle.Empty;
			this.mouseDragBase = ControlDesigner.InvalidPoint;
			this.mouseDragTool = null;
			this.Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			if (!rectangle.IsEmpty && this.graphics != null)
			{
				Rectangle rectangle2 = new Rectangle(rectangle.X - this.adornerWindowToScreenOffset.X, rectangle.Y - this.adornerWindowToScreenOffset.Y, rectangle.Width, rectangle.Height);
				int num = ParentControlDesigner.FrameWidth(this.mouseDragFrame);
				this.graphics.SetClip(rectangle2);
				using (Region region = new Region(rectangle2))
				{
					region.Exclude(Rectangle.Inflate(rectangle2, -num, -num));
					base.BehaviorService.Invalidate(region);
				}
				this.graphics.ResetClip();
			}
			if (this.graphics != null)
			{
				this.graphics.Dispose();
				this.graphics = null;
			}
			if (this.dragManager != null)
			{
				this.dragManager.OnMouseUp();
				this.dragManager = null;
			}
			IEventHandlerService eventHandlerService = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
			if (eventHandlerService != null && this.escapeHandler != null)
			{
				eventHandlerService.PopHandler(this.escapeHandler);
				this.escapeHandler = null;
			}
			if (this.statusCommandUI != null && !rectangle.IsEmpty)
			{
				NativeMethods.POINT point2 = new NativeMethods.POINT(point.X, point.Y);
				NativeMethods.MapWindowPoints(IntPtr.Zero, this.Control.Handle, point2, 1);
				if (this.statusCommandUI != null)
				{
					this.statusCommandUI.SetStatusInformation(new Rectangle(point2.x, point2.y, rectangle.Width, rectangle.Height));
				}
			}
			if (rectangle.IsEmpty && !cancel)
			{
				if (toolboxItem != null)
				{
					try
					{
						this.CreateTool(toolboxItem, point);
						if (this.toolboxService != null)
						{
							this.toolboxService.SelectedToolboxItemUsed();
						}
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
						base.DisplayError(ex);
					}
				}
				return;
			}
			if (cancel)
			{
				return;
			}
			if (toolboxItem != null)
			{
				try
				{
					Size size = new Size(DesignerUtils.MinDragSize.Width * 2, DesignerUtils.MinDragSize.Height * 2);
					if (rectangle.Width < size.Width)
					{
						rectangle.Width = size.Width;
					}
					if (rectangle.Height < size.Height)
					{
						rectangle.Height = size.Height;
					}
					this.CreateTool(toolboxItem, rectangle);
					if (this.toolboxService != null)
					{
						this.toolboxService.SelectedToolboxItemUsed();
					}
					return;
				}
				catch (Exception ex2)
				{
					if (ClientUtils.IsCriticalException(ex2))
					{
						throw;
					}
					base.DisplayError(ex2);
					return;
				}
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				object[] componentsInRect = this.GetComponentsInRect(rectangle, true, false);
				if (componentsInRect.Length > 0)
				{
					selectionService.SetSelectedComponents(componentsInRect);
				}
			}
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			if (this.toolboxItemSnapLineBehavior != null && this.toolboxItemSnapLineBehavior.IsPushed)
			{
				base.BehaviorService.PopBehavior(this.toolboxItemSnapLineBehavior);
				this.toolboxItemSnapLineBehavior.IsPushed = false;
			}
			if (this.GetOleDragHandler().Dragging || this.mouseDragBase == ControlDesigner.InvalidPoint)
			{
				return;
			}
			Rectangle rectangle = this.mouseDragOffset;
			this.mouseDragOffset.X = this.mouseDragBase.X;
			this.mouseDragOffset.Y = this.mouseDragBase.Y;
			this.mouseDragOffset.Width = x - this.mouseDragBase.X;
			this.mouseDragOffset.Height = y - this.mouseDragBase.Y;
			if (this.dragManager == null && this.ParticipatesWithSnapLines && this.mouseDragTool != null && base.BehaviorService.UseSnapLines)
			{
				this.dragManager = new DragAssistanceManager(base.Component.Site);
			}
			if (this.dragManager != null)
			{
				Rectangle rectangle2 = new Rectangle(this.mouseDragBase.X - this.adornerWindowToScreenOffset.X, this.mouseDragBase.Y - this.adornerWindowToScreenOffset.Y, x - this.mouseDragBase.X, y - this.mouseDragBase.Y);
				Point point = this.dragManager.OnMouseMove(rectangle2, this.GenerateNewToolSnapLines(rectangle2));
				this.mouseDragOffset.Width = this.mouseDragOffset.Width + point.X;
				this.mouseDragOffset.Height = this.mouseDragOffset.Height + point.Y;
				this.dragManager.RenderSnapLinesInternal();
			}
			if (this.mouseDragOffset.Width < 0)
			{
				this.mouseDragOffset.X = this.mouseDragOffset.X + this.mouseDragOffset.Width;
				this.mouseDragOffset.Width = -this.mouseDragOffset.Width;
			}
			if (this.mouseDragOffset.Height < 0)
			{
				this.mouseDragOffset.Y = this.mouseDragOffset.Y + this.mouseDragOffset.Height;
				this.mouseDragOffset.Height = -this.mouseDragOffset.Height;
			}
			if (this.mouseDragTool != null)
			{
				this.mouseDragOffset = this.Control.RectangleToClient(this.mouseDragOffset);
				this.mouseDragOffset = this.GetUpdatedRect(Rectangle.Empty, this.mouseDragOffset, true);
				this.mouseDragOffset = this.Control.RectangleToScreen(this.mouseDragOffset);
			}
			if (this.graphics == null)
			{
				this.graphics = base.BehaviorService.AdornerWindowGraphics;
			}
			if (!this.mouseDragOffset.IsEmpty && this.graphics != null)
			{
				Rectangle rectangle3 = new Rectangle(this.mouseDragOffset.X - this.adornerWindowToScreenOffset.X, this.mouseDragOffset.Y - this.adornerWindowToScreenOffset.Y, this.mouseDragOffset.Width, this.mouseDragOffset.Height);
				using (Region region = new Region(rectangle3))
				{
					int num = ParentControlDesigner.FrameWidth(this.mouseDragFrame);
					region.Exclude(Rectangle.Inflate(rectangle3, -num, -num));
					if (!rectangle.IsEmpty)
					{
						rectangle.X -= this.adornerWindowToScreenOffset.X;
						rectangle.Y -= this.adornerWindowToScreenOffset.Y;
						using (Region region2 = new Region(rectangle))
						{
							region2.Exclude(Rectangle.Inflate(rectangle, -num, -num));
							base.BehaviorService.Invalidate(region2);
						}
					}
					DesignerUtils.DrawFrame(this.graphics, region, this.mouseDragFrame, this.Control.BackColor);
				}
			}
			if (this.statusCommandUI != null)
			{
				NativeMethods.POINT point2 = new NativeMethods.POINT(this.mouseDragOffset.X, this.mouseDragOffset.Y);
				NativeMethods.MapWindowPoints(IntPtr.Zero, this.Control.Handle, point2, 1);
				if (this.statusCommandUI != null)
				{
					this.statusCommandUI.SetStatusInformation(new Rectangle(point2.x, point2.y, this.mouseDragOffset.Width, this.mouseDragOffset.Height));
				}
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (this.DrawGrid)
			{
				Control control = this.Control;
				Rectangle displayRectangle = this.Control.DisplayRectangle;
				Rectangle clientRectangle = this.Control.ClientRectangle;
				Rectangle rectangle = new Rectangle(Math.Min(displayRectangle.X, clientRectangle.X), Math.Min(displayRectangle.Y, clientRectangle.Y), Math.Max(displayRectangle.Width, clientRectangle.Width), Math.Max(displayRectangle.Height, clientRectangle.Height));
				float num = (float)rectangle.X;
				float num2 = (float)rectangle.Y;
				pe.Graphics.TranslateTransform(num, num2);
				rectangle.X = (rectangle.Y = 0);
				rectangle.Width++;
				rectangle.Height++;
				ControlPaint.DrawGrid(pe.Graphics, rectangle, this.GridSize, control.BackColor);
				pe.Graphics.TranslateTransform(-num, -num2);
			}
			base.OnPaintAdornments(pe);
		}

		private void OnScroll(object sender, ScrollEventArgs se)
		{
			base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.Control));
		}

		protected override void OnSetCursor()
		{
			if (this.toolboxService == null)
			{
				this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			}
			try
			{
				if (this.toolboxService == null || !this.toolboxService.SetCursor() || this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
				{
					Cursor.Current = Cursors.Default;
				}
			}
			catch
			{
				Cursor.Current = Cursors.Default;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (!this.DefaultUseSnapLines)
			{
				properties["DrawGrid"] = TypeDescriptor.CreateProperty(typeof(ParentControlDesigner), "DrawGrid", typeof(bool), new Attribute[]
				{
					BrowsableAttribute.Yes,
					DesignOnlyAttribute.Yes,
					new SRDescriptionAttribute("ParentControlDesignerDrawGridDescr"),
					CategoryAttribute.Design
				});
				properties["SnapToGrid"] = TypeDescriptor.CreateProperty(typeof(ParentControlDesigner), "SnapToGrid", typeof(bool), new Attribute[]
				{
					BrowsableAttribute.Yes,
					DesignOnlyAttribute.Yes,
					new SRDescriptionAttribute("ParentControlDesignerSnapToGridDescr"),
					CategoryAttribute.Design
				});
				properties["GridSize"] = TypeDescriptor.CreateProperty(typeof(ParentControlDesigner), "GridSize", typeof(Size), new Attribute[]
				{
					BrowsableAttribute.Yes,
					new SRDescriptionAttribute("ParentControlDesignerGridSizeDescr"),
					DesignOnlyAttribute.Yes,
					CategoryAttribute.Design
				});
			}
			properties["CurrentGridSize"] = TypeDescriptor.CreateProperty(typeof(ParentControlDesigner), "CurrentGridSize", typeof(Size), new Attribute[]
			{
				BrowsableAttribute.No,
				DesignerSerializationVisibilityAttribute.Hidden
			});
		}

		private void ReParentControls(Control newParent, ArrayList controls, string transactionName, IDesignerHost host)
		{
			using (DesignerTransaction designerTransaction = host.CreateTransaction(transactionName))
			{
				IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(newParent)["Controls"];
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(newParent)["Location"];
				Point point = Point.Empty;
				if (propertyDescriptor2 != null)
				{
					point = (Point)propertyDescriptor2.GetValue(newParent);
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanging(newParent, propertyDescriptor);
				}
				foreach (object obj in controls)
				{
					Control control = obj as Control;
					Control parent = control.Parent;
					Point point2 = Point.Empty;
					InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control)[typeof(InheritanceAttribute)];
					if (inheritanceAttribute == null || inheritanceAttribute != InheritanceAttribute.InheritedReadOnly)
					{
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(control)["Location"];
						if (propertyDescriptor3 != null)
						{
							point2 = (Point)propertyDescriptor3.GetValue(control);
						}
						if (parent != null)
						{
							if (componentChangeService != null)
							{
								componentChangeService.OnComponentChanging(parent, propertyDescriptor);
							}
							parent.Controls.Remove(control);
						}
						newParent.Controls.Add(control);
						Point empty = Point.Empty;
						if (parent != null)
						{
							if (parent.Controls.Contains(newParent))
							{
								empty = new Point(point2.X - point.X, point2.Y - point.Y);
							}
							else
							{
								Point point3 = (Point)propertyDescriptor3.GetValue(parent);
								empty = new Point(point2.X + point3.X, point2.Y + point3.Y);
							}
						}
						propertyDescriptor3.SetValue(control, empty);
						if (componentChangeService != null && parent != null)
						{
							componentChangeService.OnComponentChanged(parent, propertyDescriptor, null, null);
						}
					}
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(newParent, propertyDescriptor, null, null);
				}
				designerTransaction.Commit();
			}
		}

		private bool ShouldSerializeDrawGrid()
		{
			ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
			if (parentControlDesignerOfParent != null)
			{
				return this.DrawGrid != parentControlDesignerOfParent.DrawGrid;
			}
			return !this.IsOptionDefault("ShowGrid", this.DrawGrid);
		}

		private bool ShouldSerializeSnapToGrid()
		{
			ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
			if (parentControlDesignerOfParent != null)
			{
				return this.SnapToGrid != parentControlDesignerOfParent.SnapToGrid;
			}
			return !this.IsOptionDefault("SnapToGrid", this.SnapToGrid);
		}

		private bool ShouldSerializeGridSize()
		{
			ParentControlDesigner parentControlDesignerOfParent = this.GetParentControlDesignerOfParent();
			if (parentControlDesignerOfParent != null)
			{
				return !this.GridSize.Equals(parentControlDesignerOfParent.GridSize);
			}
			return !this.IsOptionDefault("GridSize", this.GridSize);
		}

		private void ResetGridSize()
		{
			this.getDefaultGridSize = true;
			this.parentCanSetGridSize = true;
			Control control = this.Control;
			if (control != null)
			{
				control.Invalidate(true);
			}
		}

		private void ResetDrawGrid()
		{
			this.getDefaultDrawGrid = true;
			this.parentCanSetDrawGrid = true;
			Control control = this.Control;
			if (control != null)
			{
				control.Invalidate(true);
			}
		}

		private void ResetSnapToGrid()
		{
			this.getDefaultGridSnap = true;
			this.parentCanSetGridSnap = true;
		}

		IComponent IOleDragClient.Component
		{
			get
			{
				return base.Component;
			}
		}

		bool IOleDragClient.AddComponent(IComponent component, string name, bool firstAdd)
		{
			IContainer container = DesignerUtils.CheckForNestedContainer(base.Component.Site.Container);
			bool flag = true;
			IContainer container2 = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!firstAdd)
			{
				if (component.Site != null)
				{
					container2 = component.Site.Container;
					IDesignerHost designerHost2 = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
					flag = container != container2;
					if (flag)
					{
						container2.Remove(component);
					}
				}
				if (flag)
				{
					if (name != null && container.Components[name] != null)
					{
						name = null;
					}
					if (name != null)
					{
						container.Add(component, name);
					}
					else
					{
						container.Add(component);
					}
				}
			}
			if (!((IOleDragClient)this).IsDropOk(component))
			{
				try
				{
					IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
					string @string = SR.GetString("DesignerCantParentType", new object[]
					{
						component.GetType().Name,
						base.Component.GetType().Name
					});
					if (iuiservice != null)
					{
						iuiservice.ShowError(@string);
					}
					else
					{
						RTLAwareMessageBox.Show(null, @string, null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
					return false;
				}
				finally
				{
					if (flag)
					{
						container.Remove(component);
						if (container2 != null)
						{
							container2.Add(component);
						}
					}
					else
					{
						container.Remove(component);
					}
				}
			}
			Control control = this.GetControl(component);
			if (control != null)
			{
				Control parentForComponent = this.GetParentForComponent(component);
				Form form = control as Form;
				if (form == null || !form.TopLevel)
				{
					if (control.Parent != parentForComponent)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parentForComponent)["Controls"];
						if (control.Parent != null)
						{
							Control parent = control.Parent;
							if (this.componentChangeSvc != null)
							{
								this.componentChangeSvc.OnComponentChanging(parent, propertyDescriptor);
							}
							parent.Controls.Remove(control);
							if (this.componentChangeSvc != null)
							{
								this.componentChangeSvc.OnComponentChanged(parent, propertyDescriptor, parent.Controls, parent.Controls);
							}
						}
						if (this.suspendChanging == 0 && this.componentChangeSvc != null)
						{
							this.componentChangeSvc.OnComponentChanging(parentForComponent, propertyDescriptor);
						}
						parentForComponent.Controls.Add(control);
						if (this.componentChangeSvc != null)
						{
							this.componentChangeSvc.OnComponentChanged(parentForComponent, propertyDescriptor, parentForComponent.Controls, parentForComponent.Controls);
						}
					}
					else
					{
						int childIndex = parentForComponent.Controls.GetChildIndex(control);
						parentForComponent.Controls.Remove(control);
						parentForComponent.Controls.Add(control);
						parentForComponent.Controls.SetChildIndex(control, childIndex);
					}
				}
				control.Invalidate(true);
			}
			if (designerHost != null && flag)
			{
				IComponentInitializer componentInitializer = designerHost.GetDesigner(component) as IComponentInitializer;
				if (componentInitializer != null)
				{
					componentInitializer.InitializeExistingComponent(null);
				}
				this.AddChildComponents(component, container, designerHost);
			}
			return true;
		}

		bool IOleDragClient.CanModifyComponents
		{
			get
			{
				return !this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly);
			}
		}

		bool IOleDragClient.IsDropOk(IComponent component)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IDesigner designer = designerHost.GetDesigner(component);
				bool flag = false;
				if (designer == null)
				{
					designer = TypeDescriptor.CreateDesigner(component, typeof(IDesigner));
					ControlDesigner controlDesigner = designer as ControlDesigner;
					if (controlDesigner != null)
					{
						controlDesigner.ForceVisible = false;
					}
					designer.Initialize(component);
					flag = true;
				}
				try
				{
					ComponentDesigner componentDesigner = designer as ComponentDesigner;
					if (componentDesigner != null)
					{
						if (!componentDesigner.CanBeAssociatedWith(this))
						{
							return false;
						}
						ControlDesigner controlDesigner2 = componentDesigner as ControlDesigner;
						if (controlDesigner2 != null)
						{
							return this.CanParent(controlDesigner2);
						}
					}
				}
				finally
				{
					if (flag)
					{
						designer.Dispose();
					}
				}
				return true;
			}
			return true;
		}

		Control IOleDragClient.GetDesignerControl()
		{
			return this.Control;
		}

		Control IOleDragClient.GetControlForComponent(object component)
		{
			return this.GetControl(component);
		}

		private const int minGridSize = 2;

		private const int maxGridSize = 200;

		private static BooleanSwitch StepControls = new BooleanSwitch("StepControls", "ParentControlDesigner: step added controls");

		private Point mouseDragBase = ControlDesigner.InvalidPoint;

		private Rectangle mouseDragOffset = Rectangle.Empty;

		private ToolboxItem mouseDragTool;

		private FrameStyle mouseDragFrame;

		private OleDragDropHandler oleDragDropHandler;

		private ParentControlDesigner.EscapeHandler escapeHandler;

		private Control pendingRemoveControl;

		private IComponentChangeService componentChangeSvc;

		private DragAssistanceManager dragManager;

		private ToolboxSnapDragDropEventArgs toolboxSnapDragDropEventArgs;

		private ToolboxItemSnapLineBehavior toolboxItemSnapLineBehavior;

		private Graphics graphics;

		private IToolboxService toolboxService;

		private Point adornerWindowToScreenOffset;

		private bool checkSnapLineSetting = true;

		private bool defaultUseSnapLines;

		private bool gridSnap = true;

		private Size gridSize = Size.Empty;

		private bool drawGrid = true;

		private bool parentCanSetDrawGrid = true;

		private bool parentCanSetGridSize = true;

		private bool parentCanSetGridSnap = true;

		private bool getDefaultDrawGrid = true;

		private bool getDefaultGridSize = true;

		private bool getDefaultGridSnap = true;

		private StatusCommandUI statusCommandUI;

		private int suspendChanging;

		private class EscapeHandler : IMenuStatusHandler
		{
			public EscapeHandler(ParentControlDesigner designer)
			{
				this.designer = designer;
			}

			public bool OverrideInvoke(MenuCommand cmd)
			{
				if (cmd.CommandID.Equals(MenuCommands.KeyCancel))
				{
					this.designer.OnMouseDragEnd(true);
					return true;
				}
				return false;
			}

			public bool OverrideStatus(MenuCommand cmd)
			{
				if (cmd.CommandID.Equals(MenuCommands.KeyCancel))
				{
					cmd.Enabled = true;
				}
				else
				{
					cmd.Enabled = false;
				}
				return true;
			}

			private ParentControlDesigner designer;
		}
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class SplitContainerDesigner : ParentControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				SplitContainerDesigner.OrientationActionList orientationActionList = new SplitContainerDesigner.OrientationActionList(this);
				designerActionListCollection.Add(orientationActionList);
				return designerActionListCollection;
			}
		}

		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		protected override bool DrawGrid
		{
			get
			{
				return !this.disableDrawGrid && base.DrawGrid;
			}
		}

		protected override Control GetParentForComponent(IComponent component)
		{
			return this.splitterPanel1;
		}

		public override IList SnapLines
		{
			get
			{
				return base.SnapLinesInternal() as ArrayList;
			}
		}

		public override int NumberOfInternalControlDesigners()
		{
			return SplitContainerDesigner.numberOfSplitterPanels;
		}

		public override ControlDesigner InternalControlDesigner(int internalControlIndex)
		{
			SplitterPanel splitterPanel;
			switch (internalControlIndex)
			{
			case 0:
				splitterPanel = this.splitterPanel1;
				break;
			case 1:
				splitterPanel = this.splitterPanel2;
				break;
			default:
				return null;
			}
			return this.designerHost.GetDesigner(splitterPanel) as ControlDesigner;
		}

		internal SplitterPanel Selected
		{
			get
			{
				return this.selectedPanel;
			}
			set
			{
				if (this.selectedPanel != null)
				{
					SplitterPanelDesigner splitterPanelDesigner = (SplitterPanelDesigner)this.designerHost.GetDesigner(this.selectedPanel);
					splitterPanelDesigner.Selected = false;
				}
				if (value != null)
				{
					SplitterPanelDesigner splitterPanelDesigner2 = (SplitterPanelDesigner)this.designerHost.GetDesigner(value);
					this.selectedPanel = value;
					splitterPanelDesigner2.Selected = true;
					return;
				}
				if (this.selectedPanel != null)
				{
					SplitterPanelDesigner splitterPanelDesigner3 = (SplitterPanelDesigner)this.designerHost.GetDesigner(this.selectedPanel);
					this.selectedPanel = null;
					splitterPanelDesigner3.Selected = false;
				}
			}
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this.splitContainer.Controls)
				{
					SplitterPanel splitterPanel = (SplitterPanel)obj;
					foreach (object obj2 in splitterPanel.Controls)
					{
						Control control = (Control)obj2;
						arrayList.Add(control);
					}
				}
				return arrayList;
			}
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			de.Effect = DragDropEffects.None;
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			if (this.Selected == null)
			{
				this.Selected = this.splitterPanel1;
			}
			SplitterPanelDesigner splitterPanelDesigner = (SplitterPanelDesigner)this.designerHost.GetDesigner(this.Selected);
			ParentControlDesigner.InvokeCreateTool(splitterPanelDesigner, tool);
			return null;
		}

		protected override void Dispose(bool disposing)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged -= this.OnSelectionChanged;
			}
			this.splitContainer.MouseDown -= this.OnSplitContainer;
			this.splitContainer.SplitterMoved -= this.OnSplitterMoved;
			this.splitContainer.SplitterMoving -= this.OnSplitterMoving;
			this.splitContainer.DoubleClick -= this.OnSplitContainerDoubleClick;
			base.Dispose(disposing);
		}

		protected override bool GetHitTest(Point point)
		{
			return this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly && this.splitContainerSelected;
		}

		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(this.splitterPanel1);
				SplitterPanelDesigner splitterPanelDesigner = this.designerHost.GetDesigner(this.splitterPanel1) as SplitterPanelDesigner;
				this.OnSetCursor();
				if (splitterPanelDesigner != null)
				{
					ControlBodyGlyph controlBodyGlyph = new ControlBodyGlyph(rectangle, Cursor.Current, this.splitterPanel1, splitterPanelDesigner);
					selectionManager.BodyGlyphAdorner.Glyphs.Add(controlBodyGlyph);
				}
				rectangle = base.BehaviorService.ControlRectInAdornerWindow(this.splitterPanel2);
				splitterPanelDesigner = this.designerHost.GetDesigner(this.splitterPanel2) as SplitterPanelDesigner;
				if (splitterPanelDesigner != null)
				{
					ControlBodyGlyph controlBodyGlyph = new ControlBodyGlyph(rectangle, Cursor.Current, this.splitterPanel2, splitterPanelDesigner);
					selectionManager.BodyGlyphAdorner.Glyphs.Add(controlBodyGlyph);
				}
			}
			return base.GetControlGlyph(selectionType);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.splitContainer = component as SplitContainer;
			this.splitterPanel1 = this.splitContainer.Panel1;
			this.splitterPanel2 = this.splitContainer.Panel2;
			base.EnableDesignMode(this.splitContainer.Panel1, "Panel1");
			base.EnableDesignMode(this.splitContainer.Panel2, "Panel2");
			this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			if (this.selectedPanel == null)
			{
				this.Selected = this.splitterPanel1;
			}
			this.splitContainer.MouseDown += this.OnSplitContainer;
			this.splitContainer.SplitterMoved += this.OnSplitterMoved;
			this.splitContainer.SplitterMoving += this.OnSplitterMoving;
			this.splitContainer.DoubleClick += this.OnSplitContainerDoubleClick;
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += this.OnSelectionChanged;
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			try
			{
				this.disableDrawGrid = true;
				base.OnPaintAdornments(pe);
			}
			finally
			{
				this.disableDrawGrid = false;
			}
		}

		public override bool CanParent(Control control)
		{
			return false;
		}

		private void OnSplitContainer(object sender, MouseEventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			selectionService.SetSelectedComponents(new object[] { this.Control });
		}

		private void OnSplitContainerDoubleClick(object sender, EventArgs e)
		{
			if (this.splitContainerSelected)
			{
				try
				{
					this.DoDefaultAction();
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
		}

		private void OnSplitterMoved(object sender, SplitterEventArgs e)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly || this.splitterDistanceException)
			{
				return;
			}
			try
			{
				base.RaiseComponentChanging(TypeDescriptor.GetProperties(this.splitContainer)["SplitterDistance"]);
				base.RaiseComponentChanged(TypeDescriptor.GetProperties(this.splitContainer)["SplitterDistance"], null, null);
				if (this.disabledGlyphs)
				{
					foreach (Adorner adorner in base.BehaviorService.Adorners)
					{
						adorner.Enabled = true;
					}
					SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
					if (selectionManager != null)
					{
						selectionManager.Refresh();
					}
					this.disabledGlyphs = false;
				}
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = (IUIService)base.Component.Site.GetService(typeof(IUIService));
				iuiservice.ShowError(ex.Message);
			}
			catch (CheckoutException ex2)
			{
				if (ex2 == CheckoutException.Canceled)
				{
					try
					{
						this.splitterDistanceException = true;
						this.splitContainer.SplitterDistance = this.initialSplitterDist;
						goto IL_011F;
					}
					finally
					{
						this.splitterDistanceException = false;
					}
					goto IL_011D;
					IL_011F:
					return;
				}
				IL_011D:
				throw;
			}
		}

		private void OnSplitterMoving(object sender, SplitterCancelEventArgs e)
		{
			this.initialSplitterDist = this.splitContainer.SplitterDistance;
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				return;
			}
			this.disabledGlyphs = true;
			Adorner adorner = null;
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				adorner = selectionManager.BodyGlyphAdorner;
			}
			foreach (Adorner adorner2 in base.BehaviorService.Adorners)
			{
				if (adorner == null || !adorner2.Equals(adorner))
				{
					adorner2.Enabled = false;
				}
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in adorner.Glyphs)
			{
				ControlBodyGlyph controlBodyGlyph = (ControlBodyGlyph)obj;
				if (!(controlBodyGlyph.RelatedComponent is SplitterPanel))
				{
					arrayList.Add(controlBodyGlyph);
				}
			}
			foreach (object obj2 in arrayList)
			{
				Glyph glyph = (Glyph)obj2;
				adorner.Glyphs.Remove(glyph);
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			this.splitContainerSelected = false;
			if (selectionService != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				foreach (object obj in selectedComponents)
				{
					SplitterPanel splitterPanel = SplitContainerDesigner.CheckIfPanelSelected(obj);
					if (splitterPanel != null && splitterPanel.Parent == this.splitContainer)
					{
						this.splitContainerSelected = false;
						this.Selected = splitterPanel;
						break;
					}
					this.Selected = null;
					if (obj == this.splitContainer)
					{
						this.splitContainerSelected = true;
						break;
					}
				}
			}
		}

		private static SplitterPanel CheckIfPanelSelected(object comp)
		{
			return comp as SplitterPanel;
		}

		internal void SplitterPanelHover()
		{
			this.OnMouseHover();
		}

		private const string panel1Name = "Panel1";

		private const string panel2Name = "Panel2";

		private IDesignerHost designerHost;

		private SplitContainer splitContainer;

		private SplitterPanel selectedPanel;

		private static int numberOfSplitterPanels = 2;

		private SplitterPanel splitterPanel1;

		private SplitterPanel splitterPanel2;

		private bool disableDrawGrid;

		private bool disabledGlyphs;

		private bool splitContainerSelected;

		private int initialSplitterDist;

		private bool splitterDistanceException;

		private class OrientationActionList : DesignerActionList
		{
			public OrientationActionList(SplitContainerDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
				this.ownerComponent = owner.Component as Component;
				if (this.ownerComponent != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.ownerComponent)["Orientation"];
					if (propertyDescriptor != null)
					{
						this.actionName = (((Orientation)propertyDescriptor.GetValue(this.ownerComponent) == Orientation.Horizontal) ? SR.GetString("DesignerShortcutVerticalOrientation") : SR.GetString("DesignerShortcutHorizontalOrientation"));
					}
				}
			}

			private void OnOrientationActionClick(object sender, EventArgs e)
			{
				DesignerVerb designerVerb = sender as DesignerVerb;
				if (designerVerb != null)
				{
					Orientation orientation = (designerVerb.Text.Equals(SR.GetString("DesignerShortcutHorizontalOrientation")) ? Orientation.Horizontal : Orientation.Vertical);
					this.actionName = ((orientation == Orientation.Horizontal) ? SR.GetString("DesignerShortcutVerticalOrientation") : SR.GetString("DesignerShortcutHorizontalOrientation"));
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.ownerComponent)["Orientation"];
					if (propertyDescriptor != null && (Orientation)propertyDescriptor.GetValue(this.ownerComponent) != orientation)
					{
						propertyDescriptor.SetValue(this.ownerComponent, orientation);
					}
					DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.owner.GetService(typeof(DesignerActionUIService));
					if (designerActionUIService != null)
					{
						designerActionUIService.Refresh(this.ownerComponent);
					}
				}
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionVerbItem(new DesignerVerb(this.actionName, new EventHandler(this.OnOrientationActionClick)))
				};
			}

			private string actionName;

			private SplitContainerDesigner owner;

			private Component ownerComponent;
		}
	}
}

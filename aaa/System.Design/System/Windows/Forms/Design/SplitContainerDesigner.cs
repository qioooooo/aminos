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
	// Token: 0x0200028D RID: 653
	internal class SplitContainerDesigner : ParentControlDesigner
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x0007F580 File Offset: 0x0007E580
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

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x0007F5A3 File Offset: 0x0007E5A3
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x0007F5A6 File Offset: 0x0007E5A6
		protected override bool DrawGrid
		{
			get
			{
				return !this.disableDrawGrid && base.DrawGrid;
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0007F5B8 File Offset: 0x0007E5B8
		protected override Control GetParentForComponent(IComponent component)
		{
			return this.splitterPanel1;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x0007F5C0 File Offset: 0x0007E5C0
		public override IList SnapLines
		{
			get
			{
				return base.SnapLinesInternal() as ArrayList;
			}
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0007F5DA File Offset: 0x0007E5DA
		public override int NumberOfInternalControlDesigners()
		{
			return SplitContainerDesigner.numberOfSplitterPanels;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0007F5E4 File Offset: 0x0007E5E4
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

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001845 RID: 6213 RVA: 0x0007F628 File Offset: 0x0007E628
		// (set) Token: 0x06001846 RID: 6214 RVA: 0x0007F630 File Offset: 0x0007E630
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

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001847 RID: 6215 RVA: 0x0007F6B4 File Offset: 0x0007E6B4
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

		// Token: 0x06001848 RID: 6216 RVA: 0x0007F768 File Offset: 0x0007E768
		protected override void OnDragEnter(DragEventArgs de)
		{
			de.Effect = DragDropEffects.None;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0007F774 File Offset: 0x0007E774
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

		// Token: 0x0600184A RID: 6218 RVA: 0x0007F7B4 File Offset: 0x0007E7B4
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

		// Token: 0x0600184B RID: 6219 RVA: 0x0007F84F File Offset: 0x0007E84F
		protected override bool GetHitTest(Point point)
		{
			return this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly && this.splitContainerSelected;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0007F868 File Offset: 0x0007E868
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

		// Token: 0x0600184D RID: 6221 RVA: 0x0007F944 File Offset: 0x0007E944
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

		// Token: 0x0600184E RID: 6222 RVA: 0x0007FA78 File Offset: 0x0007EA78
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

		// Token: 0x0600184F RID: 6223 RVA: 0x0007FAB0 File Offset: 0x0007EAB0
		public override bool CanParent(Control control)
		{
			return false;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0007FAB4 File Offset: 0x0007EAB4
		private void OnSplitContainer(object sender, MouseEventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			selectionService.SetSelectedComponents(new object[] { this.Control });
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0007FAF0 File Offset: 0x0007EAF0
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

		// Token: 0x06001852 RID: 6226 RVA: 0x0007FB34 File Offset: 0x0007EB34
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

		// Token: 0x06001853 RID: 6227 RVA: 0x0007FC98 File Offset: 0x0007EC98
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

		// Token: 0x06001854 RID: 6228 RVA: 0x0007FE04 File Offset: 0x0007EE04
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

		// Token: 0x06001855 RID: 6229 RVA: 0x0007FEBC File Offset: 0x0007EEBC
		private static SplitterPanel CheckIfPanelSelected(object comp)
		{
			return comp as SplitterPanel;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0007FEC4 File Offset: 0x0007EEC4
		internal void SplitterPanelHover()
		{
			this.OnMouseHover();
		}

		// Token: 0x04001414 RID: 5140
		private const string panel1Name = "Panel1";

		// Token: 0x04001415 RID: 5141
		private const string panel2Name = "Panel2";

		// Token: 0x04001416 RID: 5142
		private IDesignerHost designerHost;

		// Token: 0x04001417 RID: 5143
		private SplitContainer splitContainer;

		// Token: 0x04001418 RID: 5144
		private SplitterPanel selectedPanel;

		// Token: 0x04001419 RID: 5145
		private static int numberOfSplitterPanels = 2;

		// Token: 0x0400141A RID: 5146
		private SplitterPanel splitterPanel1;

		// Token: 0x0400141B RID: 5147
		private SplitterPanel splitterPanel2;

		// Token: 0x0400141C RID: 5148
		private bool disableDrawGrid;

		// Token: 0x0400141D RID: 5149
		private bool disabledGlyphs;

		// Token: 0x0400141E RID: 5150
		private bool splitContainerSelected;

		// Token: 0x0400141F RID: 5151
		private int initialSplitterDist;

		// Token: 0x04001420 RID: 5152
		private bool splitterDistanceException;

		// Token: 0x0200028E RID: 654
		private class OrientationActionList : DesignerActionList
		{
			// Token: 0x06001859 RID: 6233 RVA: 0x0007FEDC File Offset: 0x0007EEDC
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

			// Token: 0x0600185A RID: 6234 RVA: 0x0007FF64 File Offset: 0x0007EF64
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

			// Token: 0x0600185B RID: 6235 RVA: 0x00080024 File Offset: 0x0007F024
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionVerbItem(new DesignerVerb(this.actionName, new EventHandler(this.OnOrientationActionClick)))
				};
			}

			// Token: 0x04001421 RID: 5153
			private string actionName;

			// Token: 0x04001422 RID: 5154
			private SplitContainerDesigner owner;

			// Token: 0x04001423 RID: 5155
			private Component ownerComponent;
		}
	}
}

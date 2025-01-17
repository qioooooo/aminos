﻿using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000305 RID: 773
	internal sealed class SelectionManager : IDisposable
	{
		// Token: 0x06001D9C RID: 7580 RVA: 0x000A8220 File Offset: 0x000A7220
		public SelectionManager(IServiceProvider serviceProvider, BehaviorService behaviorService)
		{
			this.prevSelectionBounds = null;
			this.prevPrimarySelection = null;
			this.behaviorService = behaviorService;
			this.serviceProvider = serviceProvider;
			this.selSvc = (ISelectionService)serviceProvider.GetService(typeof(ISelectionService));
			this.designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
			if (this.designerHost != null)
			{
				ISelectionService selectionService = this.selSvc;
			}
			behaviorService.BeginDrag += this.OnBeginDrag;
			behaviorService.Synchronize += this.OnSynchronize;
			this.selSvc.SelectionChanged += this.OnSelectionChanged;
			this.rootComponent = (Control)this.designerHost.RootComponent;
			this.selectionAdorner = new Adorner();
			this.bodyAdorner = new Adorner();
			behaviorService.Adorners.Add(this.bodyAdorner);
			behaviorService.Adorners.Add(this.selectionAdorner);
			this.componentToDesigner = new Hashtable();
			IComponentChangeService componentChangeService = (IComponentChangeService)serviceProvider.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded += this.OnComponentAdded;
				componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
			this.designerHost.TransactionClosed += this.OnTransactionClosed;
			DesignerOptionService designerOptionService = this.designerHost.GetService(typeof(DesignerOptionService)) as DesignerOptionService;
			if (designerOptionService != null)
			{
				PropertyDescriptor propertyDescriptor = designerOptionService.Options.Properties["UseSmartTags"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && (bool)propertyDescriptor.GetValue(null))
				{
					this.designerActionUI = new DesignerActionUI(serviceProvider, this.selectionAdorner);
					behaviorService.DesignerActionUI = this.designerActionUI;
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000A8405 File Offset: 0x000A7405
		internal Adorner BodyGlyphAdorner
		{
			get
			{
				return this.bodyAdorner;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001D9E RID: 7582 RVA: 0x000A840D File Offset: 0x000A740D
		// (set) Token: 0x06001D9F RID: 7583 RVA: 0x000A8415 File Offset: 0x000A7415
		internal bool NeedRefresh
		{
			get
			{
				return this.needRefresh;
			}
			set
			{
				this.needRefresh = value;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x000A841E File Offset: 0x000A741E
		internal Adorner SelectionGlyphAdorner
		{
			get
			{
				return this.selectionAdorner;
			}
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000A8428 File Offset: 0x000A7428
		private void AddAllControlGlyphs(Control parent, ArrayList selComps, object primarySelection)
		{
			foreach (object obj in parent.Controls)
			{
				Control control = (Control)obj;
				this.AddAllControlGlyphs(control, selComps, primarySelection);
			}
			GlyphSelectionType glyphSelectionType = GlyphSelectionType.NotSelected;
			if (selComps.Contains(parent))
			{
				if (parent.Equals(primarySelection))
				{
					glyphSelectionType = GlyphSelectionType.SelectedPrimary;
				}
				else
				{
					glyphSelectionType = GlyphSelectionType.Selected;
				}
			}
			this.AddControlGlyphs(parent, glyphSelectionType);
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000A84A8 File Offset: 0x000A74A8
		private void AddControlGlyphs(Control c, GlyphSelectionType selType)
		{
			ControlDesigner controlDesigner = (ControlDesigner)this.componentToDesigner[c];
			if (controlDesigner != null)
			{
				ControlBodyGlyph controlGlyphInternal = controlDesigner.GetControlGlyphInternal(selType);
				if (controlGlyphInternal != null)
				{
					this.bodyAdorner.Glyphs.Add(controlGlyphInternal);
					if (selType == GlyphSelectionType.SelectedPrimary || selType == GlyphSelectionType.Selected)
					{
						if (this.curSelectionBounds[this.curCompIndex] == Rectangle.Empty)
						{
							this.curSelectionBounds[this.curCompIndex] = controlGlyphInternal.Bounds;
						}
						else
						{
							this.curSelectionBounds[this.curCompIndex] = Rectangle.Union(this.curSelectionBounds[this.curCompIndex], controlGlyphInternal.Bounds);
						}
					}
				}
				GlyphCollection glyphs = controlDesigner.GetGlyphs(selType);
				if (glyphs != null)
				{
					this.selectionAdorner.Glyphs.AddRange(glyphs);
					if (selType == GlyphSelectionType.SelectedPrimary || selType == GlyphSelectionType.Selected)
					{
						foreach (object obj in glyphs)
						{
							Glyph glyph = (Glyph)obj;
							this.curSelectionBounds[this.curCompIndex] = Rectangle.Union(this.curSelectionBounds[this.curCompIndex], glyph.Bounds);
						}
					}
				}
			}
			if (selType == GlyphSelectionType.SelectedPrimary || selType == GlyphSelectionType.Selected)
			{
				this.curCompIndex++;
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000A8628 File Offset: 0x000A7628
		public void Dispose()
		{
			if (this.designerHost != null)
			{
				this.designerHost.TransactionClosed -= this.OnTransactionClosed;
				this.designerHost = null;
			}
			if (this.serviceProvider != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded -= this.OnComponentAdded;
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
					componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				}
				if (this.selSvc != null)
				{
					this.selSvc.SelectionChanged -= this.OnSelectionChanged;
					this.selSvc = null;
				}
				this.serviceProvider = null;
			}
			if (this.behaviorService != null)
			{
				this.behaviorService.Adorners.Remove(this.bodyAdorner);
				this.behaviorService.Adorners.Remove(this.selectionAdorner);
				this.behaviorService.BeginDrag -= this.OnBeginDrag;
				this.behaviorService.Synchronize -= this.OnSynchronize;
				this.behaviorService = null;
			}
			if (this.selectionAdorner != null)
			{
				this.selectionAdorner.Glyphs.Clear();
				this.selectionAdorner = null;
			}
			if (this.bodyAdorner != null)
			{
				this.bodyAdorner.Glyphs.Clear();
				this.bodyAdorner = null;
			}
			if (this.designerActionUI != null)
			{
				this.designerActionUI.Dispose();
				this.designerActionUI = null;
			}
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000A87A8 File Offset: 0x000A77A8
		public void Refresh()
		{
			this.NeedRefresh = false;
			this.OnSelectionChanged(this, null);
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000A87BC File Offset: 0x000A77BC
		private void OnComponentAdded(object source, ComponentEventArgs ce)
		{
			IComponent component = ce.Component;
			IDesigner designer = this.designerHost.GetDesigner(component);
			if (designer is ControlDesigner)
			{
				this.componentToDesigner.Add(component, designer);
			}
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000A87F4 File Offset: 0x000A77F4
		private void OnBeginDrag(object source, BehaviorDragDropEventArgs e)
		{
			ArrayList arrayList = new ArrayList(e.DragComponents);
			ArrayList arrayList2 = new ArrayList();
			foreach (object obj in this.bodyAdorner.Glyphs)
			{
				ControlBodyGlyph controlBodyGlyph = (ControlBodyGlyph)obj;
				if (controlBodyGlyph.RelatedComponent is Control && (arrayList.Contains(controlBodyGlyph.RelatedComponent) || !((Control)controlBodyGlyph.RelatedComponent).AllowDrop))
				{
					arrayList2.Add(controlBodyGlyph);
				}
			}
			foreach (object obj2 in arrayList2)
			{
				Glyph glyph = (Glyph)obj2;
				this.bodyAdorner.Glyphs.Remove(glyph);
			}
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000A88F0 File Offset: 0x000A78F0
		internal void OnBeginDrag(BehaviorDragDropEventArgs e)
		{
			this.OnBeginDrag(null, e);
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000A88FA File Offset: 0x000A78FA
		private void OnComponentChanged(object source, ComponentChangedEventArgs ce)
		{
			if (this.selSvc.GetComponentSelected(ce.Component))
			{
				if (!this.designerHost.InTransaction)
				{
					this.Refresh();
					return;
				}
				this.NeedRefresh = true;
			}
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000A892A File Offset: 0x000A792A
		private void OnComponentRemoved(object source, ComponentEventArgs ce)
		{
			if (this.componentToDesigner.Contains(ce.Component))
			{
				this.componentToDesigner.Remove(ce.Component);
			}
			if (this.designerActionUI != null)
			{
				this.designerActionUI.RemoveActionGlyph(ce.Component);
			}
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x000A896C File Offset: 0x000A796C
		private Region DetermineRegionToRefresh(object primarySelection)
		{
			Region region = new Region(Rectangle.Empty);
			Rectangle[] array;
			Rectangle[] array2;
			if (this.curSelectionBounds.Length >= this.prevSelectionBounds.Length)
			{
				array = this.curSelectionBounds;
				array2 = this.prevSelectionBounds;
			}
			else
			{
				array = this.prevSelectionBounds;
				array2 = this.curSelectionBounds;
			}
			bool[] array3 = new bool[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array3[i] = false;
			}
			for (int j = 0; j < array.Length; j++)
			{
				bool flag = false;
				Rectangle rectangle = array[j];
				for (int k = 0; k < array2.Length; k++)
				{
					if (rectangle.IntersectsWith(array2[k]))
					{
						Rectangle rectangle2 = array2[k];
						flag = true;
						if (rectangle != rectangle2)
						{
							region.Union(rectangle);
							region.Union(rectangle2);
						}
						array3[k] = true;
						break;
					}
				}
				if (!flag)
				{
					region.Union(rectangle);
				}
			}
			for (int l = 0; l < array3.Length; l++)
			{
				if (!array3[l])
				{
					region.Union(array2[l]);
				}
			}
			using (Graphics adornerWindowGraphics = this.behaviorService.AdornerWindowGraphics)
			{
				if (region.IsEmpty(adornerWindowGraphics) && primarySelection != null && !primarySelection.Equals(this.prevPrimarySelection))
				{
					for (int m = 0; m < this.curSelectionBounds.Length; m++)
					{
						region.Union(this.curSelectionBounds[m]);
					}
				}
			}
			return region;
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x000A8B08 File Offset: 0x000A7B08
		private void OnSynchronize(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000A8B10 File Offset: 0x000A7B10
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (!this.selectionChanging)
			{
				this.selectionChanging = true;
				this.selectionAdorner.Glyphs.Clear();
				this.bodyAdorner.Glyphs.Clear();
				ArrayList arrayList = new ArrayList(this.selSvc.GetSelectedComponents());
				object primarySelection = this.selSvc.PrimarySelection;
				this.curCompIndex = 0;
				this.curSelectionBounds = new Rectangle[arrayList.Count];
				this.AddAllControlGlyphs(this.rootComponent, arrayList, primarySelection);
				if (this.prevSelectionBounds != null)
				{
					Region region = this.DetermineRegionToRefresh(primarySelection);
					using (Graphics adornerWindowGraphics = this.behaviorService.AdornerWindowGraphics)
					{
						if (!region.IsEmpty(adornerWindowGraphics))
						{
							this.selectionAdorner.Invalidate(region);
						}
						goto IL_012D;
					}
				}
				if (this.curSelectionBounds.Length > 0)
				{
					Rectangle rectangle = this.curSelectionBounds[0];
					for (int i = 1; i < this.curSelectionBounds.Length; i++)
					{
						rectangle = Rectangle.Union(rectangle, this.curSelectionBounds[i]);
					}
					if (rectangle != Rectangle.Empty)
					{
						this.selectionAdorner.Invalidate(rectangle);
					}
				}
				else
				{
					this.selectionAdorner.Invalidate();
				}
				IL_012D:
				this.prevPrimarySelection = primarySelection;
				if (this.curSelectionBounds.Length > 0)
				{
					this.prevSelectionBounds = new Rectangle[this.curSelectionBounds.Length];
					Array.Copy(this.curSelectionBounds, this.prevSelectionBounds, this.curSelectionBounds.Length);
				}
				else
				{
					this.prevSelectionBounds = null;
				}
				this.selectionChanging = false;
			}
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000A8CA8 File Offset: 0x000A7CA8
		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction && this.NeedRefresh)
			{
				this.Refresh();
			}
		}

		// Token: 0x040016BD RID: 5821
		private Adorner selectionAdorner;

		// Token: 0x040016BE RID: 5822
		private Adorner bodyAdorner;

		// Token: 0x040016BF RID: 5823
		private BehaviorService behaviorService;

		// Token: 0x040016C0 RID: 5824
		private IServiceProvider serviceProvider;

		// Token: 0x040016C1 RID: 5825
		private Hashtable componentToDesigner;

		// Token: 0x040016C2 RID: 5826
		private Control rootComponent;

		// Token: 0x040016C3 RID: 5827
		private ISelectionService selSvc;

		// Token: 0x040016C4 RID: 5828
		private IDesignerHost designerHost;

		// Token: 0x040016C5 RID: 5829
		private bool needRefresh;

		// Token: 0x040016C6 RID: 5830
		private Rectangle[] prevSelectionBounds;

		// Token: 0x040016C7 RID: 5831
		private object prevPrimarySelection;

		// Token: 0x040016C8 RID: 5832
		private Rectangle[] curSelectionBounds;

		// Token: 0x040016C9 RID: 5833
		private int curCompIndex;

		// Token: 0x040016CA RID: 5834
		private DesignerActionUI designerActionUI;

		// Token: 0x040016CB RID: 5835
		private bool selectionChanging;
	}
}

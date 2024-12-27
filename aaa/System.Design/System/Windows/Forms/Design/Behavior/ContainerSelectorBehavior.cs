using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002EB RID: 747
	internal sealed class ContainerSelectorBehavior : Behavior
	{
		// Token: 0x06001CF0 RID: 7408 RVA: 0x000A18E1 File Offset: 0x000A08E1
		internal ContainerSelectorBehavior(Control containerControl, IServiceProvider serviceProvider)
		{
			this.Init(containerControl, serviceProvider);
			this.setInitialDragPoint = false;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000A18F8 File Offset: 0x000A08F8
		internal ContainerSelectorBehavior(Control containerControl, IServiceProvider serviceProvider, bool setInitialDragPoint)
		{
			this.Init(containerControl, serviceProvider);
			this.setInitialDragPoint = setInitialDragPoint;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000A1910 File Offset: 0x000A0910
		private void Init(Control containerControl, IServiceProvider serviceProvider)
		{
			this.behaviorService = (BehaviorService)serviceProvider.GetService(typeof(BehaviorService));
			if (this.behaviorService == null)
			{
				return;
			}
			this.containerControl = containerControl;
			this.serviceProvider = serviceProvider;
			this.initialDragPoint = Point.Empty;
			this.okToMove = false;
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x000A1961 File Offset: 0x000A0961
		public Control ContainerControl
		{
			get
			{
				return this.containerControl;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000A1969 File Offset: 0x000A0969
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x000A1971 File Offset: 0x000A0971
		public bool OkToMove
		{
			get
			{
				return this.okToMove;
			}
			set
			{
				this.okToMove = value;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000A197A File Offset: 0x000A097A
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x000A1982 File Offset: 0x000A0982
		public Point InitialDragPoint
		{
			get
			{
				return this.initialDragPoint;
			}
			set
			{
				this.initialDragPoint = value;
			}
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000A198C File Offset: 0x000A098C
		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (button == MouseButtons.Left)
			{
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService != null && !this.containerControl.Equals(selectionService.PrimarySelection as Control))
				{
					selectionService.SetSelectedComponents(new object[] { this.containerControl }, SelectionTypes.Click | SelectionTypes.Toggle);
					ContainerSelectorGlyph containerSelectorGlyph = g as ContainerSelectorGlyph;
					if (containerSelectorGlyph == null)
					{
						return false;
					}
					using (BehaviorServiceAdornerCollectionEnumerator enumerator = this.behaviorService.Adorners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Adorner adorner = enumerator.Current;
							foreach (object obj in adorner.Glyphs)
							{
								Glyph glyph = (Glyph)obj;
								ContainerSelectorGlyph containerSelectorGlyph2 = glyph as ContainerSelectorGlyph;
								if (containerSelectorGlyph2 != null && !containerSelectorGlyph2.Equals(containerSelectorGlyph))
								{
									ContainerSelectorBehavior containerSelectorBehavior = containerSelectorGlyph2.RelatedBehavior as ContainerSelectorBehavior;
									ContainerSelectorBehavior containerSelectorBehavior2 = containerSelectorGlyph.RelatedBehavior as ContainerSelectorBehavior;
									if (containerSelectorBehavior != null && containerSelectorBehavior2 != null && containerSelectorBehavior2.ContainerControl.Equals(containerSelectorBehavior.ContainerControl))
									{
										containerSelectorBehavior.OkToMove = true;
										containerSelectorBehavior.InitialDragPoint = this.DetermineInitialDragPoint(mouseLoc);
										break;
									}
								}
							}
						}
						return false;
					}
				}
				this.InitialDragPoint = this.DetermineInitialDragPoint(mouseLoc);
				this.OkToMove = true;
			}
			return false;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000A1B20 File Offset: 0x000A0B20
		private Point DetermineInitialDragPoint(Point mouseLoc)
		{
			if (this.setInitialDragPoint)
			{
				Point point = this.behaviorService.ControlToAdornerWindow(this.containerControl);
				point = this.behaviorService.AdornerWindowPointToScreen(point);
				Cursor.Position = point;
				return point;
			}
			return mouseLoc;
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000A1B60 File Offset: 0x000A0B60
		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (button == MouseButtons.Left && this.OkToMove)
			{
				if (this.InitialDragPoint == Point.Empty)
				{
					this.InitialDragPoint = this.DetermineInitialDragPoint(mouseLoc);
				}
				Size size = new Size(Math.Abs(mouseLoc.X - this.InitialDragPoint.X), Math.Abs(mouseLoc.Y - this.InitialDragPoint.Y));
				if (size.Width >= DesignerUtils.MinDragSize.Width / 2 || size.Height >= DesignerUtils.MinDragSize.Height / 2)
				{
					Point point = this.behaviorService.AdornerWindowToScreen();
					point.Offset(mouseLoc.X, mouseLoc.Y);
					this.StartDragOperation(point);
				}
			}
			return false;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000A1C3A File Offset: 0x000A0C3A
		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			this.InitialDragPoint = Point.Empty;
			this.OkToMove = false;
			return false;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000A1C50 File Offset: 0x000A0C50
		private void StartDragOperation(Point initialMouseLocation)
		{
			ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			if (selectionService == null || designerHost == null)
			{
				return;
			}
			Control parent = this.containerControl.Parent;
			ArrayList arrayList = new ArrayList();
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			foreach (object obj in selectedComponents)
			{
				IComponent component = (IComponent)obj;
				Control control = component as Control;
				if (control != null && control.Parent.Equals(parent))
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
					if (controlDesigner != null && (controlDesigner.SelectionRules & SelectionRules.Moveable) != SelectionRules.None)
					{
						arrayList.Add(control);
					}
				}
			}
			if (arrayList.Count > 0)
			{
				Point point;
				if (this.setInitialDragPoint)
				{
					point = this.behaviorService.ControlToAdornerWindow(this.containerControl);
					point = this.behaviorService.AdornerWindowPointToScreen(point);
				}
				else
				{
					point = initialMouseLocation;
				}
				DropSourceBehavior dropSourceBehavior = new DropSourceBehavior(arrayList, this.containerControl.Parent, point);
				try
				{
					this.behaviorService.DoDragDrop(dropSourceBehavior);
				}
				finally
				{
					this.OkToMove = false;
					this.InitialDragPoint = Point.Empty;
				}
			}
		}

		// Token: 0x04001611 RID: 5649
		private Control containerControl;

		// Token: 0x04001612 RID: 5650
		private IServiceProvider serviceProvider;

		// Token: 0x04001613 RID: 5651
		private BehaviorService behaviorService;

		// Token: 0x04001614 RID: 5652
		private bool okToMove;

		// Token: 0x04001615 RID: 5653
		private Point initialDragPoint;

		// Token: 0x04001616 RID: 5654
		private bool setInitialDragPoint;
	}
}

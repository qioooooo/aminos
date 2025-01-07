using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class ContainerSelectorBehavior : Behavior
	{
		internal ContainerSelectorBehavior(Control containerControl, IServiceProvider serviceProvider)
		{
			this.Init(containerControl, serviceProvider);
			this.setInitialDragPoint = false;
		}

		internal ContainerSelectorBehavior(Control containerControl, IServiceProvider serviceProvider, bool setInitialDragPoint)
		{
			this.Init(containerControl, serviceProvider);
			this.setInitialDragPoint = setInitialDragPoint;
		}

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

		public Control ContainerControl
		{
			get
			{
				return this.containerControl;
			}
		}

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

		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			this.InitialDragPoint = Point.Empty;
			this.OkToMove = false;
			return false;
		}

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

		private Control containerControl;

		private IServiceProvider serviceProvider;

		private BehaviorService behaviorService;

		private bool okToMove;

		private Point initialDragPoint;

		private bool setInitialDragPoint;
	}
}

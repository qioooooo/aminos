using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	internal class SplitterPanelDesigner : PanelDesigner
	{
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return parentDesigner is SplitContainerDesigner;
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (this.splitterPanel != null && this.splitterPanel.Parent != null)
				{
					return (InheritanceAttribute)TypeDescriptor.GetAttributes(this.splitterPanel.Parent)[typeof(InheritanceAttribute)];
				}
				return base.InheritanceAttribute;
			}
		}

		internal bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
				if (this.selected)
				{
					this.DrawSelectedBorder();
					return;
				}
				this.EraseBorder();
			}
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragEnter(de);
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragOver(de);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				return;
			}
			base.OnDragLeave(e);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragDrop(de);
		}

		protected override void OnMouseHover()
		{
			if (this.splitContainerDesigner != null)
			{
				this.splitContainerDesigner.SplitterPanelHover();
			}
		}

		protected override void Dispose(bool disposing)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged -= this.OnComponentChanged;
			}
			base.Dispose(disposing);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.splitterPanel = (SplitterPanel)component;
			this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			this.splitContainerDesigner = (SplitContainerDesigner)this.designerHost.GetDesigner(this.splitterPanel.Parent);
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Locked"];
			if (propertyDescriptor != null && this.splitterPanel.Parent is SplitContainer)
			{
				propertyDescriptor.SetValue(component, true);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (this.splitterPanel.Parent == null)
			{
				return;
			}
			if (this.splitterPanel.Controls.Count == 0)
			{
				Graphics graphics = this.splitterPanel.CreateGraphics();
				this.DrawWaterMark(graphics);
				graphics.Dispose();
				return;
			}
			this.splitterPanel.Invalidate();
		}

		internal void DrawSelectedBorder()
		{
			Control control = this.Control;
			Rectangle clientRectangle = control.ClientRectangle;
			using (Graphics graphics = control.CreateGraphics())
			{
				Color color;
				if ((double)control.BackColor.GetBrightness() < 0.5)
				{
					color = ControlPaint.Light(control.BackColor);
				}
				else
				{
					color = ControlPaint.Dark(control.BackColor);
				}
				using (Pen pen = new Pen(color))
				{
					pen.DashStyle = DashStyle.Dash;
					clientRectangle.Inflate(-4, -4);
					graphics.DrawRectangle(pen, clientRectangle);
				}
			}
		}

		internal void EraseBorder()
		{
			Control control = this.Control;
			Rectangle clientRectangle = control.ClientRectangle;
			Graphics graphics = control.CreateGraphics();
			Color backColor = control.BackColor;
			Pen pen = new Pen(backColor);
			pen.DashStyle = DashStyle.Dash;
			clientRectangle.Inflate(-4, -4);
			graphics.DrawRectangle(pen, clientRectangle);
			pen.Dispose();
			graphics.Dispose();
			control.Invalidate();
		}

		internal void DrawWaterMark(Graphics g)
		{
			Control control = this.Control;
			Rectangle clientRectangle = control.ClientRectangle;
			string name = control.Name;
			using (Font font = new Font("Arial", 8f))
			{
				int num = clientRectangle.Width / 2 - (int)g.MeasureString(name, font).Width / 2;
				int num2 = clientRectangle.Height / 2;
				TextRenderer.DrawText(g, name, font, new Point(num, num2), Color.Black, TextFormatFlags.Default);
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			if (this.splitterPanel.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
			if (this.Selected)
			{
				this.DrawSelectedBorder();
			}
			if (this.splitterPanel.Controls.Count == 0)
			{
				this.DrawWaterMark(pe.Graphics);
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties.Remove("Modifiers");
			properties.Remove("Locked");
			properties.Remove("GenerateMember");
			foreach (object obj in properties)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)dictionaryEntry.Value;
				if (propertyDescriptor.Name.Equals("Name") && propertyDescriptor.DesignTimeOnly)
				{
					properties[dictionaryEntry.Key] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
					{
						BrowsableAttribute.No,
						DesignerSerializationVisibilityAttribute.Hidden
					});
					break;
				}
			}
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = null;
				base.AddPaddingSnapLines(ref arrayList);
				return arrayList;
			}
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = SelectionRules.None;
				Control control = this.Control;
				if (control.Parent is SplitContainer)
				{
					selectionRules = SelectionRules.Locked;
				}
				return selectionRules;
			}
		}

		private IDesignerHost designerHost;

		private SplitContainerDesigner splitContainerDesigner;

		private SplitterPanel splitterPanel;

		private bool selected;
	}
}

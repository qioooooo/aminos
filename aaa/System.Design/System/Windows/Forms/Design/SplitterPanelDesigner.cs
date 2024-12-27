using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000290 RID: 656
	internal class SplitterPanelDesigner : PanelDesigner
	{
		// Token: 0x06001860 RID: 6240 RVA: 0x00080161 File Offset: 0x0007F161
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return parentDesigner is SplitContainerDesigner;
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0008016C File Offset: 0x0007F16C
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

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x000801B9 File Offset: 0x0007F1B9
		// (set) Token: 0x06001863 RID: 6243 RVA: 0x000801C1 File Offset: 0x0007F1C1
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

		// Token: 0x06001864 RID: 6244 RVA: 0x000801DF File Offset: 0x0007F1DF
		protected override void OnDragEnter(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragEnter(de);
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x000801FD File Offset: 0x0007F1FD
		protected override void OnDragOver(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragOver(de);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0008021B File Offset: 0x0007F21B
		protected override void OnDragLeave(EventArgs e)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				return;
			}
			base.OnDragLeave(e);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00080232 File Offset: 0x0007F232
		protected override void OnDragDrop(DragEventArgs de)
		{
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragDrop(de);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00080250 File Offset: 0x0007F250
		protected override void OnMouseHover()
		{
			if (this.splitContainerDesigner != null)
			{
				this.splitContainerDesigner.SplitterPanelHover();
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00080268 File Offset: 0x0007F268
		protected override void Dispose(bool disposing)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged -= this.OnComponentChanged;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000802A8 File Offset: 0x0007F2A8
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

		// Token: 0x0600186B RID: 6251 RVA: 0x00080368 File Offset: 0x0007F368
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

		// Token: 0x0600186C RID: 6252 RVA: 0x000803BC File Offset: 0x0007F3BC
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

		// Token: 0x0600186D RID: 6253 RVA: 0x00080470 File Offset: 0x0007F470
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

		// Token: 0x0600186E RID: 6254 RVA: 0x000804D0 File Offset: 0x0007F4D0
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

		// Token: 0x0600186F RID: 6255 RVA: 0x00080560 File Offset: 0x0007F560
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

		// Token: 0x06001870 RID: 6256 RVA: 0x000805BC File Offset: 0x0007F5BC
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

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x00080690 File Offset: 0x0007F690
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = null;
				base.AddPaddingSnapLines(ref arrayList);
				return arrayList;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x000806A8 File Offset: 0x0007F6A8
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

		// Token: 0x04001424 RID: 5156
		private IDesignerHost designerHost;

		// Token: 0x04001425 RID: 5157
		private SplitContainerDesigner splitContainerDesigner;

		// Token: 0x04001426 RID: 5158
		private SplitterPanel splitterPanel;

		// Token: 0x04001427 RID: 5159
		private bool selected;
	}
}

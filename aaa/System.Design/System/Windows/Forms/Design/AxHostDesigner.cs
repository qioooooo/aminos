using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200018A RID: 394
	internal class AxHostDesigner : ControlDesigner
	{
		// Token: 0x06000EAC RID: 3756 RVA: 0x0003CDEF File Offset: 0x0003BDEF
		public AxHostDesigner()
		{
			this.handler = new EventHandler(this.OnVerb);
			base.AutoResizeHandles = true;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0003CE1C File Offset: 0x0003BE1C
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x0003CE1F File Offset: 0x0003BE1F
		private int SelectionStyle
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0003CE24 File Offset: 0x0003BE24
		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection designerVerbCollection = new DesignerVerbCollection();
				this.GetOleVerbs(designerVerbCollection);
				if (!this.foundAbout && this.axHost.HasAboutBox)
				{
					designerVerbCollection.Add(new AxHostDesigner.HostVerb(AxHostDesigner.AboutVerbData, this.handler));
				}
				return designerVerbCollection;
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003CE6C File Offset: 0x0003BE6C
		private static Size GetDefaultSize(IComponent component)
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

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003CF64 File Offset: 0x0003BF64
		public virtual void GetOleVerbs(DesignerVerbCollection rval)
		{
			NativeMethods.IEnumOLEVERB enumOLEVERB = null;
			NativeMethods.IOleObject oleObject = this.axHost.GetOcx() as NativeMethods.IOleObject;
			if (oleObject == null || NativeMethods.Failed(oleObject.EnumVerbs(out enumOLEVERB)))
			{
				return;
			}
			if (enumOLEVERB == null)
			{
				return;
			}
			int[] array = new int[1];
			NativeMethods.tagOLEVERB tagOLEVERB = new NativeMethods.tagOLEVERB();
			this.foundEdit = false;
			this.foundAbout = false;
			this.foundProperties = false;
			for (;;)
			{
				array[0] = 0;
				tagOLEVERB.lpszVerbName = null;
				int num = enumOLEVERB.Next(1, tagOLEVERB, array);
				if (num == 1)
				{
					break;
				}
				if (NativeMethods.Failed(num))
				{
					return;
				}
				if ((tagOLEVERB.grfAttribs & 2) != 0)
				{
					this.foundEdit = this.foundEdit || tagOLEVERB.lVerb == -4;
					this.foundAbout = this.foundAbout || tagOLEVERB.lVerb == 2;
					this.foundProperties = this.foundProperties || tagOLEVERB.lVerb == 1;
					rval.Add(new AxHostDesigner.HostVerb(new AxHostDesigner.OleVerbData(tagOLEVERB), this.handler));
				}
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0003D058 File Offset: 0x0003C058
		protected override bool GetHitTest(Point p)
		{
			return this.axHost.EditMode;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0003D068 File Offset: 0x0003C068
		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selType)
		{
			Cursor cursor = Cursors.Default;
			if (selType != GlyphSelectionType.NotSelected && (this.SelectionRules & SelectionRules.Moveable) != SelectionRules.None)
			{
				cursor = Cursors.SizeAll;
			}
			Point point = base.BehaviorService.ControlToAdornerWindow((Control)base.Component);
			Rectangle rectangle = new Rectangle(point, ((Control)base.Component).Size);
			return new ControlBodyGlyph(rectangle, cursor, this.Control, this);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0003D0D1 File Offset: 0x0003C0D1
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.axHost = (AxHost)component;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0003D0E6 File Offset: 0x0003C0E6
		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control == this.axHost)
			{
				this.defaultSize = AxHostDesigner.GetDefaultSize(this.axHost);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0003D107 File Offset: 0x0003C107
		protected override void OnCreateHandle()
		{
			base.OnCreateHandle();
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0003D110 File Offset: 0x0003C110
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			try
			{
				Control control = defaultValues["Parent"] as Control;
				if (control != null)
				{
					control.ControlAdded += this.OnControlAdded;
				}
				base.InitializeNewComponent(defaultValues);
				if (control != null)
				{
					control.ControlAdded -= this.OnControlAdded;
				}
				if ((defaultValues == null || !defaultValues.Contains("Size")) && this.defaultSize != Size.Empty)
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.axHost);
					if (properties != null)
					{
						PropertyDescriptor propertyDescriptor = properties["Size"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(this.axHost, new Size(this.defaultSize.Width, this.defaultSize.Height));
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0003D1E8 File Offset: 0x0003C1E8
		public virtual void OnVerb(object sender, EventArgs evevent)
		{
			if (sender != null && sender is AxHostDesigner.HostVerb)
			{
				AxHostDesigner.HostVerb hostVerb = (AxHostDesigner.HostVerb)sender;
				hostVerb.Invoke(this.axHost);
			}
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0003D214 File Offset: 0x0003C214
		protected override void PreFilterProperties(IDictionary properties)
		{
			object obj = properties["Enabled"];
			base.PreFilterProperties(properties);
			if (obj != null)
			{
				properties["Enabled"] = obj;
			}
			properties["SelectionStyle"] = TypeDescriptor.CreateProperty(typeof(AxHostDesigner), "SelectionStyle", typeof(int), new Attribute[]
			{
				BrowsableAttribute.No,
				DesignerSerializationVisibilityAttribute.Hidden,
				DesignOnlyAttribute.Yes
			});
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0003D28C File Offset: 0x0003C28C
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 132)
			{
				if (msg == 528)
				{
					if ((int)m.WParam == 1)
					{
						base.HookChildHandles(m.LParam);
					}
					base.WndProc(ref m);
					return;
				}
				base.WndProc(ref m);
			}
			else
			{
				if (!this.dragdropRevoked)
				{
					int num = NativeMethods.RevokeDragDrop(this.Control.Handle);
					this.dragdropRevoked = num == 0;
				}
				base.WndProc(ref m);
				if ((int)m.Result == -1 || (int)m.Result > 1)
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
		}

		// Token: 0x04000F77 RID: 3959
		private const int OLEIVERB_UIACTIVATE = -4;

		// Token: 0x04000F78 RID: 3960
		private const int HOSTVERB_ABOUT = 2;

		// Token: 0x04000F79 RID: 3961
		private const int HOSTVERB_PROPERTIES = 1;

		// Token: 0x04000F7A RID: 3962
		private const int HOSTVERB_EDIT = 3;

		// Token: 0x04000F7B RID: 3963
		private AxHost axHost;

		// Token: 0x04000F7C RID: 3964
		private EventHandler handler;

		// Token: 0x04000F7D RID: 3965
		private bool foundEdit;

		// Token: 0x04000F7E RID: 3966
		private bool foundAbout;

		// Token: 0x04000F7F RID: 3967
		private bool foundProperties;

		// Token: 0x04000F80 RID: 3968
		private bool dragdropRevoked;

		// Token: 0x04000F81 RID: 3969
		private Size defaultSize = Size.Empty;

		// Token: 0x04000F82 RID: 3970
		private static readonly AxHostDesigner.HostVerbData EditVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXEdit"), 3);

		// Token: 0x04000F83 RID: 3971
		private static readonly AxHostDesigner.HostVerbData PropertiesVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXProperties"), 1);

		// Token: 0x04000F84 RID: 3972
		private static readonly AxHostDesigner.HostVerbData AboutVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXAbout"), 2);

		// Token: 0x04000F85 RID: 3973
		private static TraceSwitch AxHostDesignerSwitch = new TraceSwitch("AxHostDesigner", "ActiveX Designer Trace");

		// Token: 0x0200018B RID: 395
		private class HostVerb : DesignerVerb
		{
			// Token: 0x06000EBC RID: 3772 RVA: 0x0003D38C File Offset: 0x0003C38C
			public HostVerb(AxHostDesigner.HostVerbData data, EventHandler handler)
				: base(data.ToString(), handler)
			{
				this.data = data;
			}

			// Token: 0x06000EBD RID: 3773 RVA: 0x0003D3A2 File Offset: 0x0003C3A2
			public void Invoke(AxHost host)
			{
				this.data.Execute(host);
			}

			// Token: 0x04000F86 RID: 3974
			private AxHostDesigner.HostVerbData data;
		}

		// Token: 0x0200018C RID: 396
		private class HostVerbData
		{
			// Token: 0x06000EBE RID: 3774 RVA: 0x0003D3B0 File Offset: 0x0003C3B0
			internal HostVerbData(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			// Token: 0x06000EBF RID: 3775 RVA: 0x0003D3C6 File Offset: 0x0003C3C6
			public override string ToString()
			{
				return this.name;
			}

			// Token: 0x06000EC0 RID: 3776 RVA: 0x0003D3D0 File Offset: 0x0003C3D0
			internal virtual void Execute(AxHost ctl)
			{
				switch (this.id)
				{
				case 1:
					ctl.ShowPropertyPages();
					return;
				case 2:
					ctl.ShowAboutBox();
					return;
				case 3:
					ctl.InvokeEditMode();
					return;
				default:
					return;
				}
			}

			// Token: 0x04000F87 RID: 3975
			internal readonly string name;

			// Token: 0x04000F88 RID: 3976
			internal readonly int id;
		}

		// Token: 0x0200018D RID: 397
		private class OleVerbData : AxHostDesigner.HostVerbData
		{
			// Token: 0x06000EC1 RID: 3777 RVA: 0x0003D40D File Offset: 0x0003C40D
			internal OleVerbData(NativeMethods.tagOLEVERB oleVerb)
				: base(SR.GetString("AXVerbPrefix") + oleVerb.lpszVerbName, oleVerb.lVerb)
			{
				this.dirties = (oleVerb.grfAttribs & 1) == 0;
			}

			// Token: 0x06000EC2 RID: 3778 RVA: 0x0003D441 File Offset: 0x0003C441
			internal override void Execute(AxHost ctl)
			{
				if (this.dirties)
				{
					ctl.MakeDirty();
				}
				ctl.DoVerb(this.id);
			}

			// Token: 0x04000F89 RID: 3977
			private readonly bool dirties;
		}

		// Token: 0x0200018E RID: 398
		internal class AxHostDesignerBehavior : Behavior
		{
			// Token: 0x06000EC3 RID: 3779 RVA: 0x0003D45D File Offset: 0x0003C45D
			internal AxHostDesignerBehavior(AxHostDesigner designer)
			{
				this.designer = designer;
			}

			// Token: 0x06000EC4 RID: 3780 RVA: 0x0003D46C File Offset: 0x0003C46C
			internal bool IsTransparent(Point p)
			{
				return this.designer.GetHitTest(p);
			}

			// Token: 0x06000EC5 RID: 3781 RVA: 0x0003D47C File Offset: 0x0003C47C
			private Point AdornerToControl(Point ptAdorner)
			{
				if (this.bs == null)
				{
					this.bs = (BehaviorService)this.designer.GetService(typeof(BehaviorService));
				}
				if (this.bs != null)
				{
					Point point = this.bs.AdornerWindowToScreen();
					point.X += ptAdorner.X;
					point.Y += ptAdorner.Y;
					point = this.designer.Control.PointToClient(point);
					return point;
				}
				return ptAdorner;
			}

			// Token: 0x06000EC6 RID: 3782 RVA: 0x0003D504 File Offset: 0x0003C504
			public override void OnDragDrop(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragDrop(e);
			}

			// Token: 0x06000EC7 RID: 3783 RVA: 0x0003D512 File Offset: 0x0003C512
			public override void OnDragEnter(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragEnter(e);
			}

			// Token: 0x06000EC8 RID: 3784 RVA: 0x0003D520 File Offset: 0x0003C520
			public override void OnDragLeave(Glyph g, EventArgs e)
			{
				this.designer.OnDragLeave(e);
			}

			// Token: 0x06000EC9 RID: 3785 RVA: 0x0003D52E File Offset: 0x0003C52E
			public override void OnDragOver(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragOver(e);
			}

			// Token: 0x06000ECA RID: 3786 RVA: 0x0003D53C File Offset: 0x0003C53C
			public override void OnGiveFeedback(Glyph g, GiveFeedbackEventArgs e)
			{
				this.designer.OnGiveFeedback(e);
			}

			// Token: 0x06000ECB RID: 3787 RVA: 0x0003D54C File Offset: 0x0003C54C
			public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
			{
				int num = 0;
				if (button == MouseButtons.Left)
				{
					num = 513;
				}
				else if (button == MouseButtons.Right)
				{
					num = 516;
				}
				if (num != 0)
				{
					Point point = this.AdornerToControl(mouseLoc);
					Message message = default(Message);
					message.HWnd = this.designer.Control.Handle;
					message.Msg = num;
					message.WParam = IntPtr.Zero;
					message.LParam = (IntPtr)((point.Y << 16) | point.X);
					this.designer.WndProc(ref message);
					return true;
				}
				return false;
			}

			// Token: 0x06000ECC RID: 3788 RVA: 0x0003D5E8 File Offset: 0x0003C5E8
			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				int num = 0;
				if (button == MouseButtons.Left)
				{
					num = 514;
				}
				else if (button == MouseButtons.Right)
				{
					num = 517;
				}
				if (num != 0)
				{
					Point point = this.designer.Control.PointToClient(Control.MousePosition);
					Message message = default(Message);
					message.HWnd = this.designer.Control.Handle;
					message.Msg = num;
					message.WParam = IntPtr.Zero;
					message.LParam = (IntPtr)((point.Y << 16) | point.X);
					this.designer.WndProc(ref message);
					return true;
				}
				return false;
			}

			// Token: 0x04000F8A RID: 3978
			private AxHostDesigner designer;

			// Token: 0x04000F8B RID: 3979
			private BehaviorService bs;
		}
	}
}

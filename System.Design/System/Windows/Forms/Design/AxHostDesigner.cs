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
	internal class AxHostDesigner : ControlDesigner
	{
		public AxHostDesigner()
		{
			this.handler = new EventHandler(this.OnVerb);
			base.AutoResizeHandles = true;
		}

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

		protected override bool GetHitTest(Point p)
		{
			return this.axHost.EditMode;
		}

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

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.axHost = (AxHost)component;
		}

		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control == this.axHost)
			{
				this.defaultSize = AxHostDesigner.GetDefaultSize(this.axHost);
			}
		}

		protected override void OnCreateHandle()
		{
			base.OnCreateHandle();
		}

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

		public virtual void OnVerb(object sender, EventArgs evevent)
		{
			if (sender != null && sender is AxHostDesigner.HostVerb)
			{
				AxHostDesigner.HostVerb hostVerb = (AxHostDesigner.HostVerb)sender;
				hostVerb.Invoke(this.axHost);
			}
		}

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

		private const int OLEIVERB_UIACTIVATE = -4;

		private const int HOSTVERB_ABOUT = 2;

		private const int HOSTVERB_PROPERTIES = 1;

		private const int HOSTVERB_EDIT = 3;

		private AxHost axHost;

		private EventHandler handler;

		private bool foundEdit;

		private bool foundAbout;

		private bool foundProperties;

		private bool dragdropRevoked;

		private Size defaultSize = Size.Empty;

		private static readonly AxHostDesigner.HostVerbData EditVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXEdit"), 3);

		private static readonly AxHostDesigner.HostVerbData PropertiesVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXProperties"), 1);

		private static readonly AxHostDesigner.HostVerbData AboutVerbData = new AxHostDesigner.HostVerbData(SR.GetString("AXAbout"), 2);

		private static TraceSwitch AxHostDesignerSwitch = new TraceSwitch("AxHostDesigner", "ActiveX Designer Trace");

		private class HostVerb : DesignerVerb
		{
			public HostVerb(AxHostDesigner.HostVerbData data, EventHandler handler)
				: base(data.ToString(), handler)
			{
				this.data = data;
			}

			public void Invoke(AxHost host)
			{
				this.data.Execute(host);
			}

			private AxHostDesigner.HostVerbData data;
		}

		private class HostVerbData
		{
			internal HostVerbData(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			public override string ToString()
			{
				return this.name;
			}

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

			internal readonly string name;

			internal readonly int id;
		}

		private class OleVerbData : AxHostDesigner.HostVerbData
		{
			internal OleVerbData(NativeMethods.tagOLEVERB oleVerb)
				: base(SR.GetString("AXVerbPrefix") + oleVerb.lpszVerbName, oleVerb.lVerb)
			{
				this.dirties = (oleVerb.grfAttribs & 1) == 0;
			}

			internal override void Execute(AxHost ctl)
			{
				if (this.dirties)
				{
					ctl.MakeDirty();
				}
				ctl.DoVerb(this.id);
			}

			private readonly bool dirties;
		}

		internal class AxHostDesignerBehavior : Behavior
		{
			internal AxHostDesignerBehavior(AxHostDesigner designer)
			{
				this.designer = designer;
			}

			internal bool IsTransparent(Point p)
			{
				return this.designer.GetHitTest(p);
			}

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

			public override void OnDragDrop(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragDrop(e);
			}

			public override void OnDragEnter(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragEnter(e);
			}

			public override void OnDragLeave(Glyph g, EventArgs e)
			{
				this.designer.OnDragLeave(e);
			}

			public override void OnDragOver(Glyph g, DragEventArgs e)
			{
				this.designer.OnDragOver(e);
			}

			public override void OnGiveFeedback(Glyph g, GiveFeedbackEventArgs e)
			{
				this.designer.OnGiveFeedback(e);
			}

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

			private AxHostDesigner designer;

			private BehaviorService bs;
		}
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Diagnostics;

namespace System.Windows.Forms.Design
{
	internal class AxDesigner : ControlDesigner
	{
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

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.webBrowserBase = (WebBrowserBase)component;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			try
			{
				base.InitializeNewComponent(defaultValues);
			}
			catch
			{
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
			properties["SelectionStyle"] = TypeDescriptor.CreateProperty(typeof(AxDesigner), "SelectionStyle", typeof(int), new Attribute[]
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
					IntPtr intPtr = this.Control.Handle;
					this.dragdropRevoked = true;
					while (intPtr != IntPtr.Zero && this.dragdropRevoked)
					{
						NativeMethods.RevokeDragDrop(intPtr);
						intPtr = NativeMethods.GetWindow(intPtr, 5);
					}
				}
				base.WndProc(ref m);
				if ((int)m.Result == -1 || (int)m.Result > 1)
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
		}

		private WebBrowserBase webBrowserBase;

		private bool dragdropRevoked;

		private static TraceSwitch AxDesignerSwitch = new TraceSwitch("AxDesigner", "ActiveX Designer Trace");
	}
}

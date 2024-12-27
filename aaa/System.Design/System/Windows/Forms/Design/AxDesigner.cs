using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Diagnostics;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000189 RID: 393
	internal class AxDesigner : ControlDesigner
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0003CC4A File Offset: 0x0003BC4A
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x0003CC4D File Offset: 0x0003BC4D
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

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0003CC4F File Offset: 0x0003BC4F
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.webBrowserBase = (WebBrowserBase)component;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0003CC6C File Offset: 0x0003BC6C
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

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0003CC98 File Offset: 0x0003BC98
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

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003CD10 File Offset: 0x0003BD10
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

		// Token: 0x04000F74 RID: 3956
		private WebBrowserBase webBrowserBase;

		// Token: 0x04000F75 RID: 3957
		private bool dragdropRevoked;

		// Token: 0x04000F76 RID: 3958
		private static TraceSwitch AxDesignerSwitch = new TraceSwitch("AxDesigner", "ActiveX Designer Trace");
	}
}

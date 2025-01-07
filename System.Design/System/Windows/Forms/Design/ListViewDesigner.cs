using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	internal class ListViewDesigner : ControlDesigner
	{
		public override ICollection AssociatedComponents
		{
			get
			{
				ListView listView = this.Control as ListView;
				if (listView != null)
				{
					return listView.Columns;
				}
				return base.AssociatedComponents;
			}
		}

		private bool OwnerDraw
		{
			get
			{
				return (bool)base.ShadowProperties["OwnerDraw"];
			}
			set
			{
				base.ShadowProperties["OwnerDraw"] = value;
			}
		}

		private View View
		{
			get
			{
				return ((ListView)base.Component).View;
			}
			set
			{
				((ListView)base.Component).View = value;
				if (value == View.Details)
				{
					base.HookChildHandles(this.Control.Handle);
				}
			}
		}

		protected override bool GetHitTest(Point point)
		{
			ListView listView = (ListView)base.Component;
			if (listView.View == View.Details)
			{
				Point point2 = this.Control.PointToClient(point);
				IntPtr handle = listView.Handle;
				IntPtr intPtr = NativeMethods.ChildWindowFromPointEx(handle, point2.X, point2.Y, 1);
				if (intPtr != IntPtr.Zero && intPtr != handle)
				{
					IntPtr intPtr2 = NativeMethods.SendMessage(handle, 4127, IntPtr.Zero, IntPtr.Zero);
					if (intPtr == intPtr2)
					{
						NativeMethods.POINT point3 = new NativeMethods.POINT();
						point3.x = point.X;
						point3.y = point.Y;
						NativeMethods.MapWindowPoints(IntPtr.Zero, intPtr2, point3, 1);
						this.hdrhit.pt_x = point3.x;
						this.hdrhit.pt_y = point3.y;
						NativeMethods.SendMessage(intPtr2, 4614, IntPtr.Zero, this.hdrhit);
						if (this.hdrhit.flags == 4)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public override void Initialize(IComponent component)
		{
			ListView listView = (ListView)component;
			this.OwnerDraw = listView.OwnerDraw;
			listView.OwnerDraw = false;
			listView.UseCompatibleStateImageBehavior = false;
			base.AutoResizeHandles = true;
			base.Initialize(component);
			if (listView.View == View.Details)
			{
				base.HookChildHandles(this.Control.Handle);
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["OwnerDraw"];
			if (propertyDescriptor != null)
			{
				properties["OwnerDraw"] = TypeDescriptor.CreateProperty(typeof(ListViewDesigner), propertyDescriptor, new Attribute[0]);
			}
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties["View"];
			if (propertyDescriptor2 != null)
			{
				properties["View"] = TypeDescriptor.CreateProperty(typeof(ListViewDesigner), propertyDescriptor2, new Attribute[0]);
			}
			base.PreFilterProperties(properties);
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 78 || msg == 8270)
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.NMHDR));
				if (nmhdr.code == NativeMethods.HDN_ENDTRACK)
				{
					try
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
						componentChangeService.OnComponentChanged(base.Component, null, null, null);
					}
					catch (InvalidOperationException ex)
					{
						if (this.inShowErrorDialog)
						{
							return;
						}
						IUIService iuiservice = (IUIService)base.Component.Site.GetService(typeof(IUIService));
						this.inShowErrorDialog = true;
						try
						{
							DataGridViewDesigner.ShowErrorDialog(iuiservice, ex, (ListView)base.Component);
						}
						finally
						{
							this.inShowErrorDialog = false;
						}
						return;
					}
				}
			}
			base.WndProc(ref m);
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new ListViewActionList(this));
				}
				return this._actionLists;
			}
		}

		private DesignerActionListCollection _actionLists;

		private NativeMethods.HDHITTESTINFO hdrhit = new NativeMethods.HDHITTESTINFO();

		private bool inShowErrorDialog;
	}
}

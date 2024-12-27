using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000264 RID: 612
	internal class ListViewDesigner : ControlDesigner
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x0007780C File Offset: 0x0007680C
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

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x00077835 File Offset: 0x00076835
		// (set) Token: 0x06001728 RID: 5928 RVA: 0x0007784C File Offset: 0x0007684C
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

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x00077864 File Offset: 0x00076864
		// (set) Token: 0x0600172A RID: 5930 RVA: 0x00077876 File Offset: 0x00076876
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

		// Token: 0x0600172B RID: 5931 RVA: 0x000778A0 File Offset: 0x000768A0
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

		// Token: 0x0600172C RID: 5932 RVA: 0x000779AC File Offset: 0x000769AC
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

		// Token: 0x0600172D RID: 5933 RVA: 0x00077A04 File Offset: 0x00076A04
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

		// Token: 0x0600172E RID: 5934 RVA: 0x00077A84 File Offset: 0x00076A84
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

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00077B70 File Offset: 0x00076B70
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

		// Token: 0x0400131E RID: 4894
		private DesignerActionListCollection _actionLists;

		// Token: 0x0400131F RID: 4895
		private NativeMethods.HDHITTESTINFO hdrhit = new NativeMethods.HDHITTESTINFO();

		// Token: 0x04001320 RID: 4896
		private bool inShowErrorDialog;
	}
}

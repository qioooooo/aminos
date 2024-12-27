using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000491 RID: 1169
	public sealed class ListViewInsertionMark
	{
		// Token: 0x060045C1 RID: 17857 RVA: 0x000FD6CF File Offset: 0x000FC6CF
		internal ListViewInsertionMark(ListView listView)
		{
			this.listView = listView;
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x060045C2 RID: 17858 RVA: 0x000FD6E9 File Offset: 0x000FC6E9
		// (set) Token: 0x060045C3 RID: 17859 RVA: 0x000FD6F1 File Offset: 0x000FC6F1
		public bool AppearsAfterItem
		{
			get
			{
				return this.appearsAfterItem;
			}
			set
			{
				if (this.appearsAfterItem != value)
				{
					this.appearsAfterItem = value;
					if (this.listView.IsHandleCreated)
					{
						this.UpdateListView();
					}
				}
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x000FD718 File Offset: 0x000FC718
		public Rectangle Bounds
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				this.listView.SendMessage(4265, 0, ref rect);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x000FD762 File Offset: 0x000FC762
		// (set) Token: 0x060045C6 RID: 17862 RVA: 0x000FD79C File Offset: 0x000FC79C
		public Color Color
		{
			get
			{
				if (this.color.IsEmpty)
				{
					this.color = SafeNativeMethods.ColorFromCOLORREF((int)this.listView.SendMessage(4267, 0, 0));
				}
				return this.color;
			}
			set
			{
				if (this.color != value)
				{
					this.color = value;
					if (this.listView.IsHandleCreated)
					{
						this.listView.SendMessage(4266, 0, SafeNativeMethods.ColorToCOLORREF(this.color));
					}
				}
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x000FD7E8 File Offset: 0x000FC7E8
		// (set) Token: 0x060045C8 RID: 17864 RVA: 0x000FD7F0 File Offset: 0x000FC7F0
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				if (this.index != value)
				{
					this.index = value;
					if (this.listView.IsHandleCreated)
					{
						this.UpdateListView();
					}
				}
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x000FD818 File Offset: 0x000FC818
		public int NearestIndex(Point pt)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = pt.X;
			point.y = pt.Y;
			NativeMethods.LVINSERTMARK lvinsertmark = new NativeMethods.LVINSERTMARK();
			UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), 4264, point, lvinsertmark);
			return lvinsertmark.iItem;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x000FD874 File Offset: 0x000FC874
		internal void UpdateListView()
		{
			NativeMethods.LVINSERTMARK lvinsertmark = new NativeMethods.LVINSERTMARK();
			lvinsertmark.dwFlags = (this.appearsAfterItem ? 1 : 0);
			lvinsertmark.iItem = this.index;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), 4262, 0, lvinsertmark);
			if (!this.color.IsEmpty)
			{
				this.listView.SendMessage(4266, 0, SafeNativeMethods.ColorToCOLORREF(this.color));
			}
		}

		// Token: 0x04002176 RID: 8566
		private ListView listView;

		// Token: 0x04002177 RID: 8567
		private int index;

		// Token: 0x04002178 RID: 8568
		private Color color = Color.Empty;

		// Token: 0x04002179 RID: 8569
		private bool appearsAfterItem;
	}
}

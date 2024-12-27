using System;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020002BA RID: 698
	public class CreateParams
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x0005D62D File Offset: 0x0005C62D
		// (set) Token: 0x06002658 RID: 9816 RVA: 0x0005D635 File Offset: 0x0005C635
		public string ClassName
		{
			get
			{
				return this.className;
			}
			set
			{
				this.className = value;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x0005D63E File Offset: 0x0005C63E
		// (set) Token: 0x0600265A RID: 9818 RVA: 0x0005D646 File Offset: 0x0005C646
		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption = value;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x0005D64F File Offset: 0x0005C64F
		// (set) Token: 0x0600265C RID: 9820 RVA: 0x0005D657 File Offset: 0x0005C657
		public int Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x0005D660 File Offset: 0x0005C660
		// (set) Token: 0x0600265E RID: 9822 RVA: 0x0005D668 File Offset: 0x0005C668
		public int ExStyle
		{
			get
			{
				return this.exStyle;
			}
			set
			{
				this.exStyle = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x0005D671 File Offset: 0x0005C671
		// (set) Token: 0x06002660 RID: 9824 RVA: 0x0005D679 File Offset: 0x0005C679
		public int ClassStyle
		{
			get
			{
				return this.classStyle;
			}
			set
			{
				this.classStyle = value;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x0005D682 File Offset: 0x0005C682
		// (set) Token: 0x06002662 RID: 9826 RVA: 0x0005D68A File Offset: 0x0005C68A
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x0005D693 File Offset: 0x0005C693
		// (set) Token: 0x06002664 RID: 9828 RVA: 0x0005D69B File Offset: 0x0005C69B
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x0005D6A4 File Offset: 0x0005C6A4
		// (set) Token: 0x06002666 RID: 9830 RVA: 0x0005D6AC File Offset: 0x0005C6AC
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x0005D6B5 File Offset: 0x0005C6B5
		// (set) Token: 0x06002668 RID: 9832 RVA: 0x0005D6BD File Offset: 0x0005C6BD
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x0005D6C6 File Offset: 0x0005C6C6
		// (set) Token: 0x0600266A RID: 9834 RVA: 0x0005D6CE File Offset: 0x0005C6CE
		public IntPtr Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x0005D6D7 File Offset: 0x0005C6D7
		// (set) Token: 0x0600266C RID: 9836 RVA: 0x0005D6DF File Offset: 0x0005C6DF
		public object Param
		{
			get
			{
				return this.param;
			}
			set
			{
				this.param = value;
			}
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0005D6E8 File Offset: 0x0005C6E8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("CreateParams {'");
			stringBuilder.Append(this.className);
			stringBuilder.Append("', '");
			stringBuilder.Append(this.caption);
			stringBuilder.Append("', 0x");
			stringBuilder.Append(Convert.ToString(this.style, 16));
			stringBuilder.Append(", 0x");
			stringBuilder.Append(Convert.ToString(this.exStyle, 16));
			stringBuilder.Append(", {");
			stringBuilder.Append(this.x);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.y);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.width);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.height);
			stringBuilder.Append("}");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x0400163F RID: 5695
		private string className;

		// Token: 0x04001640 RID: 5696
		private string caption;

		// Token: 0x04001641 RID: 5697
		private int style;

		// Token: 0x04001642 RID: 5698
		private int exStyle;

		// Token: 0x04001643 RID: 5699
		private int classStyle;

		// Token: 0x04001644 RID: 5700
		private int x;

		// Token: 0x04001645 RID: 5701
		private int y;

		// Token: 0x04001646 RID: 5702
		private int width;

		// Token: 0x04001647 RID: 5703
		private int height;

		// Token: 0x04001648 RID: 5704
		private IntPtr parent;

		// Token: 0x04001649 RID: 5705
		private object param;
	}
}

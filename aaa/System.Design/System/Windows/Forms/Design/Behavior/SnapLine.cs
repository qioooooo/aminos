using System;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000306 RID: 774
	public sealed class SnapLine
	{
		// Token: 0x06001DAE RID: 7598 RVA: 0x000A8CC0 File Offset: 0x000A7CC0
		public SnapLine(SnapLineType type, int offset)
			: this(type, offset, null, SnapLinePriority.Low)
		{
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000A8CCC File Offset: 0x000A7CCC
		public SnapLine(SnapLineType type, int offset, string filter)
			: this(type, offset, filter, SnapLinePriority.Low)
		{
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000A8CD8 File Offset: 0x000A7CD8
		public SnapLine(SnapLineType type, int offset, SnapLinePriority priority)
			: this(type, offset, null, priority)
		{
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000A8CE4 File Offset: 0x000A7CE4
		public SnapLine(SnapLineType type, int offset, string filter, SnapLinePriority priority)
		{
			this.type = type;
			this.offset = offset;
			this.filter = filter;
			this.priority = priority;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x000A8D09 File Offset: 0x000A7D09
		public string Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x000A8D11 File Offset: 0x000A7D11
		public bool IsHorizontal
		{
			get
			{
				return this.type == SnapLineType.Top || this.type == SnapLineType.Bottom || this.type == SnapLineType.Horizontal || this.type == SnapLineType.Baseline;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x000A8D38 File Offset: 0x000A7D38
		public bool IsVertical
		{
			get
			{
				return this.type == SnapLineType.Left || this.type == SnapLineType.Right || this.type == SnapLineType.Vertical;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x000A8D57 File Offset: 0x000A7D57
		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000A8D5F File Offset: 0x000A7D5F
		public SnapLinePriority Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x000A8D67 File Offset: 0x000A7D67
		public SnapLineType SnapLineType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000A8D6F File Offset: 0x000A7D6F
		public void AdjustOffset(int adjustment)
		{
			this.offset += adjustment;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000A8D80 File Offset: 0x000A7D80
		public static bool ShouldSnap(SnapLine line1, SnapLine line2)
		{
			if (line1.SnapLineType != line2.SnapLineType)
			{
				return false;
			}
			if (line1.Filter == null && line2.Filter == null)
			{
				return true;
			}
			if (line1.Filter == null || line2.Filter == null)
			{
				return false;
			}
			if (line1.Filter.Contains("Margin"))
			{
				return (line1.Filter.Equals("Margin.Right") && (line2.Filter.Equals("Margin.Left") || line2.Filter.Equals("Padding.Right"))) || (line1.Filter.Equals("Margin.Left") && (line2.Filter.Equals("Margin.Right") || line2.Filter.Equals("Padding.Left"))) || (line1.Filter.Equals("Margin.Top") && (line2.Filter.Equals("Margin.Bottom") || line2.Filter.Equals("Padding.Top"))) || (line1.Filter.Equals("Margin.Bottom") && line2.Filter.Equals("Margin.Top")) || line2.Filter.Equals("Padding.Bottom");
			}
			if (line1.Filter.Contains("Padding"))
			{
				return (line1.Filter.Equals("Padding.Left") && line2.Filter.Equals("Margin.Left")) || (line1.Filter.Equals("Padding.Right") && line2.Filter.Equals("Margin.Right")) || (line1.Filter.Equals("Padding.Top") && line2.Filter.Equals("Margin.Top")) || (line1.Filter.Equals("Padding.Bottom") && line2.Filter.Equals("Margin.Bottom"));
			}
			return line1.Filter.Equals(line2.Filter);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000A8F78 File Offset: 0x000A7F78
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"SnapLine: {type = ",
				this.type,
				", offset  = ",
				this.offset,
				", priority = ",
				this.priority,
				", filter = ",
				(this.filter == null) ? "<null>" : this.filter,
				"}"
			});
		}

		// Token: 0x040016CC RID: 5836
		internal const string Margin = "Margin";

		// Token: 0x040016CD RID: 5837
		internal const string MarginRight = "Margin.Right";

		// Token: 0x040016CE RID: 5838
		internal const string MarginLeft = "Margin.Left";

		// Token: 0x040016CF RID: 5839
		internal const string MarginBottom = "Margin.Bottom";

		// Token: 0x040016D0 RID: 5840
		internal const string MarginTop = "Margin.Top";

		// Token: 0x040016D1 RID: 5841
		internal const string Padding = "Padding";

		// Token: 0x040016D2 RID: 5842
		internal const string PaddingRight = "Padding.Right";

		// Token: 0x040016D3 RID: 5843
		internal const string PaddingLeft = "Padding.Left";

		// Token: 0x040016D4 RID: 5844
		internal const string PaddingBottom = "Padding.Bottom";

		// Token: 0x040016D5 RID: 5845
		internal const string PaddingTop = "Padding.Top";

		// Token: 0x040016D6 RID: 5846
		private SnapLineType type;

		// Token: 0x040016D7 RID: 5847
		private SnapLinePriority priority;

		// Token: 0x040016D8 RID: 5848
		private int offset;

		// Token: 0x040016D9 RID: 5849
		private string filter;
	}
}

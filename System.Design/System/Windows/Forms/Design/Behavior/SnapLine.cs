using System;

namespace System.Windows.Forms.Design.Behavior
{
	public sealed class SnapLine
	{
		public SnapLine(SnapLineType type, int offset)
			: this(type, offset, null, SnapLinePriority.Low)
		{
		}

		public SnapLine(SnapLineType type, int offset, string filter)
			: this(type, offset, filter, SnapLinePriority.Low)
		{
		}

		public SnapLine(SnapLineType type, int offset, SnapLinePriority priority)
			: this(type, offset, null, priority)
		{
		}

		public SnapLine(SnapLineType type, int offset, string filter, SnapLinePriority priority)
		{
			this.type = type;
			this.offset = offset;
			this.filter = filter;
			this.priority = priority;
		}

		public string Filter
		{
			get
			{
				return this.filter;
			}
		}

		public bool IsHorizontal
		{
			get
			{
				return this.type == SnapLineType.Top || this.type == SnapLineType.Bottom || this.type == SnapLineType.Horizontal || this.type == SnapLineType.Baseline;
			}
		}

		public bool IsVertical
		{
			get
			{
				return this.type == SnapLineType.Left || this.type == SnapLineType.Right || this.type == SnapLineType.Vertical;
			}
		}

		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		public SnapLinePriority Priority
		{
			get
			{
				return this.priority;
			}
		}

		public SnapLineType SnapLineType
		{
			get
			{
				return this.type;
			}
		}

		public void AdjustOffset(int adjustment)
		{
			this.offset += adjustment;
		}

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

		internal const string Margin = "Margin";

		internal const string MarginRight = "Margin.Right";

		internal const string MarginLeft = "Margin.Left";

		internal const string MarginBottom = "Margin.Bottom";

		internal const string MarginTop = "Margin.Top";

		internal const string Padding = "Padding";

		internal const string PaddingRight = "Padding.Right";

		internal const string PaddingLeft = "Padding.Left";

		internal const string PaddingBottom = "Padding.Bottom";

		internal const string PaddingTop = "Padding.Top";

		private SnapLineType type;

		private SnapLinePriority priority;

		private int offset;

		private string filter;
	}
}

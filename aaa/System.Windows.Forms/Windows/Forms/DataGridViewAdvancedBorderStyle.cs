using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020002F7 RID: 759
	public sealed class DataGridViewAdvancedBorderStyle : ICloneable
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000A9114 File Offset: 0x000A8114
		public DataGridViewAdvancedBorderStyle()
			: this(null, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet)
		{
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000A9120 File Offset: 0x000A8120
		internal DataGridViewAdvancedBorderStyle(DataGridView owner)
			: this(owner, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet, DataGridViewAdvancedCellBorderStyle.NotSet)
		{
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000A912C File Offset: 0x000A812C
		internal DataGridViewAdvancedBorderStyle(DataGridView owner, DataGridViewAdvancedCellBorderStyle banned1, DataGridViewAdvancedCellBorderStyle banned2, DataGridViewAdvancedCellBorderStyle banned3)
		{
			this.owner = owner;
			this.banned1 = banned1;
			this.banned2 = banned2;
			this.banned3 = banned3;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000A917F File Offset: 0x000A817F
		// (set) Token: 0x0600312F RID: 12591 RVA: 0x000A9194 File Offset: 0x000A8194
		public DataGridViewAdvancedCellBorderStyle All
		{
			get
			{
				if (!this.all)
				{
					return DataGridViewAdvancedCellBorderStyle.NotSet;
				}
				return this.top;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet || value == this.banned1 || value == this.banned2 || value == this.banned3)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "All" }));
				}
				if (!this.all || this.top != value)
				{
					this.all = true;
					this.bottom = value;
					this.right = value;
					this.left = value;
					this.top = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x000A9253 File Offset: 0x000A8253
		// (set) Token: 0x06003131 RID: 12593 RVA: 0x000A926C File Offset: 0x000A826C
		public DataGridViewAdvancedCellBorderStyle Bottom
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.bottom;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Bottom" }));
				}
				this.BottomInternal = value;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (set) Token: 0x06003132 RID: 12594 RVA: 0x000A92CC File Offset: 0x000A82CC
		internal DataGridViewAdvancedCellBorderStyle BottomInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.bottom != value))
				{
					if (this.all && this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
					{
						this.right = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					this.all = false;
					this.bottom = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x000A9335 File Offset: 0x000A8335
		// (set) Token: 0x06003134 RID: 12596 RVA: 0x000A934C File Offset: 0x000A834C
		public DataGridViewAdvancedCellBorderStyle Left
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.left;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Left" }));
				}
				this.LeftInternal = value;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (set) Token: 0x06003135 RID: 12597 RVA: 0x000A93AC File Offset: 0x000A83AC
		internal DataGridViewAdvancedCellBorderStyle LeftInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.left != value))
				{
					if (this.owner != null && this.owner.RightToLeftInternal && (value == DataGridViewAdvancedCellBorderStyle.InsetDouble || value == DataGridViewAdvancedCellBorderStyle.OutsetDouble))
					{
						throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Left" }));
					}
					if (this.all)
					{
						if (this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.right = DataGridViewAdvancedCellBorderStyle.Outset;
						}
						if (this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
						}
					}
					this.all = false;
					this.left = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003136 RID: 12598 RVA: 0x000A9468 File Offset: 0x000A8468
		// (set) Token: 0x06003137 RID: 12599 RVA: 0x000A9480 File Offset: 0x000A8480
		public DataGridViewAdvancedCellBorderStyle Right
		{
			get
			{
				if (this.all)
				{
					return this.top;
				}
				return this.right;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Right" }));
				}
				this.RightInternal = value;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (set) Token: 0x06003138 RID: 12600 RVA: 0x000A94E0 File Offset: 0x000A84E0
		internal DataGridViewAdvancedCellBorderStyle RightInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.right != value))
				{
					if (this.owner != null && !this.owner.RightToLeftInternal && (value == DataGridViewAdvancedCellBorderStyle.InsetDouble || value == DataGridViewAdvancedCellBorderStyle.OutsetDouble))
					{
						throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Right" }));
					}
					if (this.all && this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
					{
						this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					this.all = false;
					this.right = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000A9589 File Offset: 0x000A8589
		// (set) Token: 0x0600313A RID: 12602 RVA: 0x000A9594 File Offset: 0x000A8594
		public DataGridViewAdvancedCellBorderStyle Top
		{
			get
			{
				return this.top;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAdvancedCellBorderStyle));
				}
				if (value == DataGridViewAdvancedCellBorderStyle.NotSet)
				{
					throw new ArgumentException(SR.GetString("DataGridView_AdvancedCellBorderStyleInvalid", new object[] { "Top" }));
				}
				this.TopInternal = value;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (set) Token: 0x0600313B RID: 12603 RVA: 0x000A95F4 File Offset: 0x000A85F4
		internal DataGridViewAdvancedCellBorderStyle TopInternal
		{
			set
			{
				if ((this.all && this.top != value) || (!this.all && this.top != value))
				{
					if (this.all)
					{
						if (this.right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.right = DataGridViewAdvancedCellBorderStyle.Outset;
						}
						if (this.bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
						{
							this.bottom = DataGridViewAdvancedCellBorderStyle.Outset;
						}
					}
					this.all = false;
					this.top = value;
					if (this.owner != null)
					{
						this.owner.OnAdvancedBorderStyleChanged(this);
					}
				}
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000A9670 File Offset: 0x000A8670
		public override bool Equals(object other)
		{
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = other as DataGridViewAdvancedBorderStyle;
			return dataGridViewAdvancedBorderStyle != null && (dataGridViewAdvancedBorderStyle.all == this.all && dataGridViewAdvancedBorderStyle.top == this.top && dataGridViewAdvancedBorderStyle.left == this.left && dataGridViewAdvancedBorderStyle.bottom == this.bottom) && dataGridViewAdvancedBorderStyle.right == this.right;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000A96D4 File Offset: 0x000A86D4
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				(int)this.top,
				(int)this.left,
				(int)this.bottom,
				(int)this.right
			});
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000A9714 File Offset: 0x000A8714
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewAdvancedBorderStyle { All=",
				this.All.ToString(),
				", Left=",
				this.Left.ToString(),
				", Right=",
				this.Right.ToString(),
				", Top=",
				this.Top.ToString(),
				", Bottom=",
				this.Bottom.ToString(),
				" }"
			});
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000A97C0 File Offset: 0x000A87C0
		object ICloneable.Clone()
		{
			return new DataGridViewAdvancedBorderStyle(this.owner, this.banned1, this.banned2, this.banned3)
			{
				all = this.all,
				top = this.top,
				right = this.right,
				bottom = this.bottom,
				left = this.left
			};
		}

		// Token: 0x040019F4 RID: 6644
		private DataGridView owner;

		// Token: 0x040019F5 RID: 6645
		private bool all = true;

		// Token: 0x040019F6 RID: 6646
		private DataGridViewAdvancedCellBorderStyle banned1;

		// Token: 0x040019F7 RID: 6647
		private DataGridViewAdvancedCellBorderStyle banned2;

		// Token: 0x040019F8 RID: 6648
		private DataGridViewAdvancedCellBorderStyle banned3;

		// Token: 0x040019F9 RID: 6649
		private DataGridViewAdvancedCellBorderStyle top = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x040019FA RID: 6650
		private DataGridViewAdvancedCellBorderStyle left = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x040019FB RID: 6651
		private DataGridViewAdvancedCellBorderStyle right = DataGridViewAdvancedCellBorderStyle.None;

		// Token: 0x040019FC RID: 6652
		private DataGridViewAdvancedCellBorderStyle bottom = DataGridViewAdvancedCellBorderStyle.None;
	}
}

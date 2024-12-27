using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200038E RID: 910
	public class DataGridViewRowHeightInfoNeededEventArgs : EventArgs
	{
		// Token: 0x060037BB RID: 14267 RVA: 0x000CBF0A File Offset: 0x000CAF0A
		internal DataGridViewRowHeightInfoNeededEventArgs()
		{
			this.rowIndex = -1;
			this.height = -1;
			this.minimumHeight = -1;
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x000CBF27 File Offset: 0x000CAF27
		// (set) Token: 0x060037BD RID: 14269 RVA: 0x000CBF30 File Offset: 0x000CAF30
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (value < this.minimumHeight)
				{
					value = this.minimumHeight;
				}
				if (value > 65536)
				{
					throw new ArgumentOutOfRangeException("Height", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"Height",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.height = value;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x000CBFA6 File Offset: 0x000CAFA6
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x000CBFB0 File Offset: 0x000CAFB0
		public int MinimumHeight
		{
			get
			{
				return this.minimumHeight;
			}
			set
			{
				if (value < 2)
				{
					throw new ArgumentOutOfRangeException("MinimumHeight", value, SR.GetString("DataGridViewBand_MinimumHeightSmallerThanOne", new object[] { 2.ToString(CultureInfo.CurrentCulture) }));
				}
				if (this.height < value)
				{
					this.height = value;
				}
				this.minimumHeight = value;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x000CC00C File Offset: 0x000CB00C
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000CC014 File Offset: 0x000CB014
		internal void SetProperties(int rowIndex, int height, int minimumHeight)
		{
			this.rowIndex = rowIndex;
			this.height = height;
			this.minimumHeight = minimumHeight;
		}

		// Token: 0x04001C31 RID: 7217
		private int rowIndex;

		// Token: 0x04001C32 RID: 7218
		private int height;

		// Token: 0x04001C33 RID: 7219
		private int minimumHeight;
	}
}

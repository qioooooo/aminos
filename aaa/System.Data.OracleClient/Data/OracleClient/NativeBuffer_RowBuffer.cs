using System;
using System.Data.Common;

namespace System.Data.OracleClient
{
	// Token: 0x02000021 RID: 33
	internal sealed class NativeBuffer_RowBuffer : NativeBuffer
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0005A458 File Offset: 0x00059858
		internal NativeBuffer_RowBuffer(int initialSize, int numberOfRows)
			: base(initialSize * numberOfRows, false)
		{
			this._rowLength = initialSize;
			this._numberOfRows = numberOfRows;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0005A480 File Offset: 0x00059880
		internal bool CurrentPositionIsValid
		{
			get
			{
				return base.BaseOffset >= 0 && base.BaseOffset < this.NumberOfRows * this.RowLength;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0005A4B0 File Offset: 0x000598B0
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0005A4C4 File Offset: 0x000598C4
		internal int NumberOfRows
		{
			get
			{
				return this._numberOfRows;
			}
			set
			{
				if (value < 0 || base.Length < value * this.RowLength)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidNumberOfRows);
				}
				this._numberOfRows = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0005A4F4 File Offset: 0x000598F4
		internal int RowLength
		{
			get
			{
				return this._rowLength;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0005A508 File Offset: 0x00059908
		internal void MoveFirst()
		{
			base.BaseOffset = 0;
			this._ready = true;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0005A524 File Offset: 0x00059924
		internal bool MoveNext()
		{
			if (!this._ready)
			{
				return false;
			}
			base.BaseOffset += this.RowLength;
			return this.CurrentPositionIsValid;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0005A554 File Offset: 0x00059954
		internal bool MovePrevious()
		{
			if (!this._ready)
			{
				return false;
			}
			if (base.BaseOffset <= -this.RowLength)
			{
				return false;
			}
			base.BaseOffset -= this.RowLength;
			return true;
		}

		// Token: 0x040001BB RID: 443
		private int _numberOfRows;

		// Token: 0x040001BC RID: 444
		private int _rowLength;

		// Token: 0x040001BD RID: 445
		private bool _ready;
	}
}

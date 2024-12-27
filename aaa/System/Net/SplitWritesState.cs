using System;

namespace System.Net
{
	// Token: 0x02000422 RID: 1058
	internal class SplitWritesState
	{
		// Token: 0x060020E7 RID: 8423 RVA: 0x0008162E File Offset: 0x0008062E
		internal SplitWritesState(BufferOffsetSize[] buffers)
		{
			this._UserBuffers = buffers;
			this._LastBufferConsumed = 0;
			this._Index = 0;
			this._RealBuffers = null;
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x00081654 File Offset: 0x00080654
		internal bool IsDone
		{
			get
			{
				if (this._LastBufferConsumed != 0)
				{
					return false;
				}
				for (int i = this._Index; i < this._UserBuffers.Length; i++)
				{
					if (this._UserBuffers[i].Size != 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00081698 File Offset: 0x00080698
		internal BufferOffsetSize[] GetNextBuffers()
		{
			int i = this._Index;
			int num = 0;
			int num2 = 0;
			int num3 = this._LastBufferConsumed;
			while (this._Index < this._UserBuffers.Length)
			{
				num2 = this._UserBuffers[this._Index].Size - this._LastBufferConsumed;
				num += num2;
				if (num > 65536)
				{
					num2 -= num - 65536;
					num = 65536;
					break;
				}
				num2 = 0;
				this._LastBufferConsumed = 0;
				this._Index++;
			}
			if (num == 0)
			{
				return null;
			}
			if (num3 == 0 && i == 0 && this._Index == this._UserBuffers.Length)
			{
				return this._UserBuffers;
			}
			int num4 = ((num2 == 0) ? (this._Index - i) : (this._Index - i + 1));
			if (this._RealBuffers == null || this._RealBuffers.Length != num4)
			{
				this._RealBuffers = new BufferOffsetSize[num4];
			}
			int num5 = 0;
			while (i < this._Index)
			{
				this._RealBuffers[num5++] = new BufferOffsetSize(this._UserBuffers[i].Buffer, this._UserBuffers[i].Offset + num3, this._UserBuffers[i].Size - num3, false);
				num3 = 0;
				i++;
			}
			if (num2 != 0)
			{
				this._RealBuffers[num5] = new BufferOffsetSize(this._UserBuffers[i].Buffer, this._UserBuffers[i].Offset + this._LastBufferConsumed, num2, false);
				if ((this._LastBufferConsumed += num2) == this._UserBuffers[this._Index].Size)
				{
					this._Index++;
					this._LastBufferConsumed = 0;
				}
			}
			return this._RealBuffers;
		}

		// Token: 0x0400213D RID: 8509
		private const int c_SplitEncryptedBuffersSize = 65536;

		// Token: 0x0400213E RID: 8510
		private BufferOffsetSize[] _UserBuffers;

		// Token: 0x0400213F RID: 8511
		private int _Index;

		// Token: 0x04002140 RID: 8512
		private int _LastBufferConsumed;

		// Token: 0x04002141 RID: 8513
		private BufferOffsetSize[] _RealBuffers;
	}
}

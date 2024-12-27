using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000243 RID: 579
	[ComVisible(true)]
	[Serializable]
	public sealed class BitArray : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x06001723 RID: 5923 RVA: 0x0003BC37 File Offset: 0x0003AC37
		private BitArray()
		{
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0003BC3F File Offset: 0x0003AC3F
		public BitArray(int length)
			: this(length, false)
		{
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0003BC4C File Offset: 0x0003AC4C
		public BitArray(int length, bool defaultValue)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_array = new int[(length + 31) / 32];
			this.m_length = length;
			int num = (defaultValue ? (-1) : 0);
			for (int i = 0; i < this.m_array.Length; i++)
			{
				this.m_array[i] = num;
			}
			this._version = 0;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0003BCBC File Offset: 0x0003ACBC
		public BitArray(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.m_array = new int[(bytes.Length + 3) / 4];
			this.m_length = bytes.Length * 8;
			int num = 0;
			int num2 = 0;
			while (bytes.Length - num2 >= 4)
			{
				this.m_array[num++] = (int)(bytes[num2] & byte.MaxValue) | ((int)(bytes[num2 + 1] & byte.MaxValue) << 8) | ((int)(bytes[num2 + 2] & byte.MaxValue) << 16) | ((int)(bytes[num2 + 3] & byte.MaxValue) << 24);
				num2 += 4;
			}
			switch (bytes.Length - num2)
			{
			case 1:
				goto IL_00DB;
			case 2:
				break;
			case 3:
				this.m_array[num] = (int)(bytes[num2 + 2] & byte.MaxValue) << 16;
				break;
			default:
				goto IL_00FC;
			}
			this.m_array[num] |= (int)(bytes[num2 + 1] & byte.MaxValue) << 8;
			IL_00DB:
			this.m_array[num] |= (int)(bytes[num2] & byte.MaxValue);
			IL_00FC:
			this._version = 0;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0003BDCC File Offset: 0x0003ADCC
		public BitArray(bool[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.m_array = new int[(values.Length + 31) / 32];
			this.m_length = values.Length;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i])
				{
					this.m_array[i / 32] |= 1 << i % 32;
				}
			}
			this._version = 0;
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0003BE48 File Offset: 0x0003AE48
		public BitArray(int[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.m_array = new int[values.Length];
			this.m_length = values.Length * 32;
			Array.Copy(values, this.m_array, values.Length);
			this._version = 0;
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0003BE9C File Offset: 0x0003AE9C
		public BitArray(BitArray bits)
		{
			if (bits == null)
			{
				throw new ArgumentNullException("bits");
			}
			this.m_array = new int[(bits.m_length + 31) / 32];
			this.m_length = bits.m_length;
			Array.Copy(bits.m_array, this.m_array, (bits.m_length + 31) / 32);
			this._version = bits._version;
		}

		// Token: 0x17000331 RID: 817
		public bool this[int index]
		{
			get
			{
				return this.Get(index);
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0003BF1C File Offset: 0x0003AF1C
		public bool Get(int index)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (this.m_array[index / 32] & (1 << index % 32)) != 0;
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0003BF5C File Offset: 0x0003AF5C
		public void Set(int index, bool value)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (value)
			{
				this.m_array[index / 32] |= 1 << index % 32;
			}
			else
			{
				this.m_array[index / 32] &= ~(1 << index % 32);
			}
			this._version++;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0003BFE8 File Offset: 0x0003AFE8
		public void SetAll(bool value)
		{
			int num = (value ? (-1) : 0);
			int num2 = (this.m_length + 31) / 32;
			for (int i = 0; i < num2; i++)
			{
				this.m_array[i] = num;
			}
			this._version++;
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0003C030 File Offset: 0x0003B030
		public BitArray And(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] &= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0003C0B4 File Offset: 0x0003B0B4
		public BitArray Or(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] |= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0003C138 File Offset: 0x0003B138
		public BitArray Xor(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] ^= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0003C1BC File Offset: 0x0003B1BC
		public BitArray Not()
		{
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] = ~this.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x0003C202 File Offset: 0x0003B202
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x0003C20C File Offset: 0x0003B20C
		public int Length
		{
			get
			{
				return this.m_length;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				int num = (value + 31) / 32;
				if (num > this.m_array.Length || num + 256 < this.m_array.Length)
				{
					int[] array = new int[num];
					Array.Copy(this.m_array, array, (num > this.m_array.Length) ? this.m_array.Length : num);
					this.m_array = array;
				}
				if (value > this.m_length)
				{
					int num2 = (this.m_length + 31) / 32 - 1;
					int num3 = this.m_length % 32;
					if (num3 > 0)
					{
						this.m_array[num2] &= (1 << num3) - 1;
					}
					Array.Clear(this.m_array, num2 + 1, num - num2 - 1);
				}
				this.m_length = value;
				this._version++;
			}
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x0003C2F4 File Offset: 0x0003B2F4
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (array is int[])
			{
				Array.Copy(this.m_array, 0, array, index, (this.m_length + 31) / 32);
				return;
			}
			if (array is byte[])
			{
				if (array.Length - index < (this.m_length + 7) / 8)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				byte[] array2 = (byte[])array;
				for (int i = 0; i < (this.m_length + 7) / 8; i++)
				{
					array2[index + i] = (byte)((this.m_array[i / 4] >> i % 4 * 8) & 255);
				}
				return;
			}
			else
			{
				if (!(array is bool[]))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_BitArrayTypeUnsupported"));
				}
				if (array.Length - index < this.m_length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				bool[] array3 = (bool[])array;
				for (int j = 0; j < this.m_length; j++)
				{
					array3[index + j] = ((this.m_array[j / 32] >> j % 32) & 1) != 0;
				}
				return;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x0003C43C File Offset: 0x0003B43C
		public int Count
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0003C444 File Offset: 0x0003B444
		public object Clone()
		{
			return new BitArray(this.m_array)
			{
				_version = this._version,
				m_length = this.m_length
			};
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x0003C476 File Offset: 0x0003B476
		public object SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x0003C498 File Offset: 0x0003B498
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x0003C49B File Offset: 0x0003B49B
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0003C49E File Offset: 0x0003B49E
		public IEnumerator GetEnumerator()
		{
			return new BitArray.BitArrayEnumeratorSimple(this);
		}

		// Token: 0x0400092B RID: 2347
		private const int _ShrinkThreshold = 256;

		// Token: 0x0400092C RID: 2348
		private int[] m_array;

		// Token: 0x0400092D RID: 2349
		private int m_length;

		// Token: 0x0400092E RID: 2350
		private int _version;

		// Token: 0x0400092F RID: 2351
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000244 RID: 580
		[Serializable]
		private class BitArrayEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x0600173C RID: 5948 RVA: 0x0003C4A6 File Offset: 0x0003B4A6
			internal BitArrayEnumeratorSimple(BitArray bitarray)
			{
				this.bitarray = bitarray;
				this.index = -1;
				this.version = bitarray._version;
			}

			// Token: 0x0600173D RID: 5949 RVA: 0x0003C4C8 File Offset: 0x0003B4C8
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x0600173E RID: 5950 RVA: 0x0003C4D0 File Offset: 0x0003B4D0
			public virtual bool MoveNext()
			{
				if (this.version != this.bitarray._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.bitarray.Count - 1)
				{
					this.index++;
					this.currentElement = this.bitarray.Get(this.index);
					return true;
				}
				this.index = this.bitarray.Count;
				return false;
			}

			// Token: 0x17000337 RID: 823
			// (get) Token: 0x0600173F RID: 5951 RVA: 0x0003C550 File Offset: 0x0003B550
			public virtual object Current
			{
				get
				{
					if (this.index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this.index >= this.bitarray.Count)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001740 RID: 5952 RVA: 0x0003C5A4 File Offset: 0x0003B5A4
			public void Reset()
			{
				if (this.version != this.bitarray._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = -1;
			}

			// Token: 0x04000930 RID: 2352
			private BitArray bitarray;

			// Token: 0x04000931 RID: 2353
			private int index;

			// Token: 0x04000932 RID: 2354
			private int version;

			// Token: 0x04000933 RID: 2355
			private bool currentElement;
		}
	}
}

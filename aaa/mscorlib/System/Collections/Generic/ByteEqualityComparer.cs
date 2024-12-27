using System;

namespace System.Collections.Generic
{
	// Token: 0x02000281 RID: 641
	[Serializable]
	internal class ByteEqualityComparer : EqualityComparer<byte>
	{
		// Token: 0x060019C3 RID: 6595 RVA: 0x00043EA1 File Offset: 0x00042EA1
		public override bool Equals(byte x, byte y)
		{
			return x == y;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00043EA7 File Offset: 0x00042EA7
		public override int GetHashCode(byte b)
		{
			return b.GetHashCode();
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00043EB0 File Offset: 0x00042EB0
		internal unsafe override int IndexOf(byte[] array, byte value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (count > array.Length - startIndex)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (count == 0)
			{
				return -1;
			}
			fixed (byte* ptr = array)
			{
				return Buffer.IndexOfByte(ptr, value, startIndex, count);
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00043F44 File Offset: 0x00042F44
		internal override int LastIndexOf(byte[] array, byte value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; i--)
			{
				if (array[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00043F70 File Offset: 0x00042F70
		public override bool Equals(object obj)
		{
			ByteEqualityComparer byteEqualityComparer = obj as ByteEqualityComparer;
			return byteEqualityComparer != null;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00043F8B File Offset: 0x00042F8B
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

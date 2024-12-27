using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x020002C8 RID: 712
	internal sealed class BigInt
	{
		// Token: 0x06001855 RID: 6229 RVA: 0x00053A23 File Offset: 0x00052A23
		internal BigInt()
		{
			this.m_elements = new byte[128];
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00053A3B File Offset: 0x00052A3B
		internal BigInt(byte b)
		{
			this.m_elements = new byte[128];
			this.SetDigit(0, b);
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001857 RID: 6231 RVA: 0x00053A5B File Offset: 0x00052A5B
		// (set) Token: 0x06001858 RID: 6232 RVA: 0x00053A63 File Offset: 0x00052A63
		internal int Size
		{
			get
			{
				return this.m_size;
			}
			set
			{
				if (value > 128)
				{
					this.m_size = 128;
				}
				if (value < 0)
				{
					this.m_size = 0;
				}
				this.m_size = value;
			}
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00053A8A File Offset: 0x00052A8A
		internal byte GetDigit(int index)
		{
			if (index < 0 || index >= this.m_size)
			{
				return 0;
			}
			return this.m_elements[index];
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00053AA4 File Offset: 0x00052AA4
		internal void SetDigit(int index, byte digit)
		{
			if (index >= 0 && index < 128)
			{
				this.m_elements[index] = digit;
				if (index >= this.m_size && digit != 0)
				{
					this.m_size = index + 1;
				}
				if (index == this.m_size - 1 && digit == 0)
				{
					this.m_size--;
				}
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00053AF7 File Offset: 0x00052AF7
		internal void SetDigit(int index, byte digit, ref int size)
		{
			if (index >= 0 && index < 128)
			{
				this.m_elements[index] = digit;
				if (index >= size && digit != 0)
				{
					size = index + 1;
				}
				if (index == size - 1 && digit == 0)
				{
					size--;
				}
			}
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00053B2C File Offset: 0x00052B2C
		public static bool operator <(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return true;
			}
			if (value2 == null)
			{
				return false;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return size < size2;
			}
			while (size-- > 0)
			{
				if (value1.m_elements[size] != value2.m_elements[size])
				{
					return value1.m_elements[size] < value2.m_elements[size];
				}
			}
			return false;
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00053B94 File Offset: 0x00052B94
		public static bool operator >(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return false;
			}
			if (value2 == null)
			{
				return true;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return size > size2;
			}
			while (size-- > 0)
			{
				if (value1.m_elements[size] != value2.m_elements[size])
				{
					return value1.m_elements[size] > value2.m_elements[size];
				}
			}
			return false;
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00053BFC File Offset: 0x00052BFC
		public static bool operator ==(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return value2 == null;
			}
			if (value2 == null)
			{
				return value1 == null;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return false;
			}
			for (int i = 0; i < size; i++)
			{
				if (value1.m_elements[i] != value2.m_elements[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00053C4E File Offset: 0x00052C4E
		public static bool operator !=(BigInt value1, BigInt value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00053C5A File Offset: 0x00052C5A
		public override bool Equals(object obj)
		{
			return obj is BigInt && this == (BigInt)obj;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00053C74 File Offset: 0x00052C74
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.m_size; i++)
			{
				num += (int)this.GetDigit(i);
			}
			return num;
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00053CA0 File Offset: 0x00052CA0
		internal static void Add(BigInt a, byte b, ref BigInt c)
		{
			byte b2 = b;
			int size = a.Size;
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				int num2 = (int)(a.GetDigit(i) + b2);
				c.SetDigit(i, (byte)(num2 & 255), ref num);
				b2 = (byte)((num2 >> 8) & 255);
			}
			if (b2 != 0)
			{
				c.SetDigit(a.Size, b2, ref num);
			}
			c.Size = num;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00053D10 File Offset: 0x00052D10
		internal static void Negate(ref BigInt a)
		{
			int num = 0;
			for (int i = 0; i < 128; i++)
			{
				a.SetDigit(i, ~a.GetDigit(i) & byte.MaxValue, ref num);
			}
			for (int j = 0; j < 128; j++)
			{
				a.SetDigit(j, a.GetDigit(j) + 1, ref num);
				if ((a.GetDigit(j) & 255) != 0)
				{
					break;
				}
				a.SetDigit(j, a.GetDigit(j) & byte.MaxValue, ref num);
			}
			a.Size = num;
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00053DA0 File Offset: 0x00052DA0
		internal static void Subtract(BigInt a, BigInt b, ref BigInt c)
		{
			byte b2 = 0;
			if (a < b)
			{
				BigInt.Subtract(b, a, ref c);
				BigInt.Negate(ref c);
				return;
			}
			int size = a.Size;
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				int num2 = (int)(a.GetDigit(i) - b.GetDigit(i) - b2);
				b2 = 0;
				if (num2 < 0)
				{
					num2 += 256;
					b2 = 1;
				}
				c.SetDigit(i, (byte)(num2 & 255), ref num);
			}
			c.Size = num;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00053E20 File Offset: 0x00052E20
		private void Multiply(int b)
		{
			if (b == 0)
			{
				this.Clear();
				return;
			}
			int num = 0;
			int size = this.Size;
			int num2 = 0;
			for (int i = 0; i < size; i++)
			{
				int num3 = b * (int)this.GetDigit(i) + num;
				num = num3 / 256;
				this.SetDigit(i, (byte)(num3 % 256), ref num2);
			}
			if (num != 0)
			{
				byte[] bytes = BitConverter.GetBytes(num);
				for (int j = 0; j < bytes.Length; j++)
				{
					this.SetDigit(size + j, bytes[j], ref num2);
				}
			}
			this.Size = num2;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00053EB4 File Offset: 0x00052EB4
		private static void Multiply(BigInt a, int b, ref BigInt c)
		{
			if (b == 0)
			{
				c.Clear();
				return;
			}
			int num = 0;
			int size = a.Size;
			int num2 = 0;
			for (int i = 0; i < size; i++)
			{
				int num3 = b * (int)a.GetDigit(i) + num;
				num = num3 / 256;
				c.SetDigit(i, (byte)(num3 % 256), ref num2);
			}
			if (num != 0)
			{
				byte[] bytes = BitConverter.GetBytes(num);
				for (int j = 0; j < bytes.Length; j++)
				{
					c.SetDigit(size + j, bytes[j], ref num2);
				}
			}
			c.Size = num2;
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00053F4C File Offset: 0x00052F4C
		private void Divide(int b)
		{
			int num = 0;
			int size = this.Size;
			int num2 = 0;
			while (size-- > 0)
			{
				int num3 = 256 * num + (int)this.GetDigit(size);
				num = num3 % b;
				this.SetDigit(size, (byte)(num3 / b), ref num2);
			}
			this.Size = num2;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00053F98 File Offset: 0x00052F98
		internal static void Divide(BigInt numerator, BigInt denominator, ref BigInt quotient, ref BigInt remainder)
		{
			if (numerator < denominator)
			{
				quotient.Clear();
				remainder.CopyFrom(numerator);
				return;
			}
			if (numerator == denominator)
			{
				quotient.Clear();
				quotient.SetDigit(0, 1);
				remainder.Clear();
				return;
			}
			BigInt bigInt = new BigInt();
			bigInt.CopyFrom(numerator);
			BigInt bigInt2 = new BigInt();
			bigInt2.CopyFrom(denominator);
			uint num = 0U;
			while (bigInt2.Size < bigInt.Size)
			{
				bigInt2.Multiply(256);
				num += 1U;
			}
			if (bigInt2 > bigInt)
			{
				bigInt2.Divide(256);
				num -= 1U;
			}
			BigInt bigInt3 = new BigInt();
			quotient.Clear();
			int num2 = 0;
			while ((long)num2 <= (long)((ulong)num))
			{
				int num3 = ((bigInt.Size == bigInt2.Size) ? ((int)bigInt.GetDigit(bigInt.Size - 1)) : (256 * (int)bigInt.GetDigit(bigInt.Size - 1) + (int)bigInt.GetDigit(bigInt.Size - 2)));
				int digit = (int)bigInt2.GetDigit(bigInt2.Size - 1);
				int num4 = num3 / digit;
				if (num4 >= 256)
				{
					num4 = 255;
				}
				BigInt.Multiply(bigInt2, num4, ref bigInt3);
				while (bigInt3 > bigInt)
				{
					num4--;
					BigInt.Multiply(bigInt2, num4, ref bigInt3);
				}
				quotient.Multiply(256);
				BigInt.Add(quotient, (byte)num4, ref quotient);
				BigInt.Subtract(bigInt, bigInt3, ref bigInt);
				bigInt2.Divide(256);
				num2++;
			}
			remainder.CopyFrom(bigInt);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00054122 File Offset: 0x00053122
		internal void CopyFrom(BigInt a)
		{
			Array.Copy(a.m_elements, this.m_elements, 128);
			this.m_size = a.m_size;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00054148 File Offset: 0x00053148
		internal bool IsZero()
		{
			for (int i = 0; i < this.m_size; i++)
			{
				if (this.m_elements[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00054174 File Offset: 0x00053174
		internal byte[] ToByteArray()
		{
			byte[] array = new byte[this.Size];
			Array.Copy(this.m_elements, array, this.Size);
			return array;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000541A0 File Offset: 0x000531A0
		internal void Clear()
		{
			this.m_size = 0;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000541AC File Offset: 0x000531AC
		internal void FromHexadecimal(string hexNum)
		{
			byte[] array = X509Utils.DecodeHexString(hexNum);
			Array.Reverse(array);
			int hexArraySize = X509Utils.GetHexArraySize(array);
			Array.Copy(array, this.m_elements, hexArraySize);
			this.Size = hexArraySize;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000541E4 File Offset: 0x000531E4
		internal void FromDecimal(string decNum)
		{
			BigInt bigInt = new BigInt();
			BigInt bigInt2 = new BigInt();
			int length = decNum.Length;
			for (int i = 0; i < length; i++)
			{
				if (decNum[i] <= '9' && decNum[i] >= '0')
				{
					BigInt.Multiply(bigInt, 10, ref bigInt2);
					BigInt.Add(bigInt2, (byte)(decNum[i] - '0'), ref bigInt);
				}
			}
			this.CopyFrom(bigInt);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0005424C File Offset: 0x0005324C
		internal string ToDecimal()
		{
			if (this.IsZero())
			{
				return "0";
			}
			BigInt bigInt = new BigInt(10);
			BigInt bigInt2 = new BigInt();
			BigInt bigInt3 = new BigInt();
			BigInt bigInt4 = new BigInt();
			bigInt2.CopyFrom(this);
			char[] array = new char[(int)Math.Ceiling((double)(this.m_size * 2) * 1.21)];
			int num = 0;
			do
			{
				BigInt.Divide(bigInt2, bigInt, ref bigInt3, ref bigInt4);
				array[num++] = BigInt.decValues[(int)(bigInt4.IsZero() ? 0 : bigInt4.m_elements[0])];
				bigInt2.CopyFrom(bigInt3);
			}
			while (!bigInt3.IsZero());
			Array.Reverse(array, 0, num);
			return new string(array, 0, num);
		}

		// Token: 0x04001633 RID: 5683
		private const int m_maxbytes = 128;

		// Token: 0x04001634 RID: 5684
		private const int m_base = 256;

		// Token: 0x04001635 RID: 5685
		private byte[] m_elements;

		// Token: 0x04001636 RID: 5686
		private int m_size;

		// Token: 0x04001637 RID: 5687
		private static readonly char[] decValues = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
	}
}

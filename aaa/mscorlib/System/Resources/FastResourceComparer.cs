using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Resources
{
	// Token: 0x02000419 RID: 1049
	internal sealed class FastResourceComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
	{
		// Token: 0x06002B6A RID: 11114 RVA: 0x000923E8 File Offset: 0x000913E8
		public int GetHashCode(object key)
		{
			string text = (string)key;
			return FastResourceComparer.HashFunction(text);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00092402 File Offset: 0x00091402
		public int GetHashCode(string key)
		{
			return FastResourceComparer.HashFunction(key);
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x0009240C File Offset: 0x0009140C
		internal static int HashFunction(string key)
		{
			uint num = 5381U;
			for (int i = 0; i < key.Length; i++)
			{
				num = ((num << 5) + num) ^ (uint)key[i];
			}
			return (int)num;
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x00092440 File Offset: 0x00091440
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			string text = (string)a;
			string text2 = (string)b;
			return string.CompareOrdinal(text, text2);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x00092468 File Offset: 0x00091468
		public int Compare(string a, string b)
		{
			return string.CompareOrdinal(a, b);
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x00092471 File Offset: 0x00091471
		public bool Equals(string a, string b)
		{
			return string.Equals(a, b);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x0009247C File Offset: 0x0009147C
		public bool Equals(object a, object b)
		{
			if (a == b)
			{
				return true;
			}
			string text = (string)a;
			string text2 = (string)b;
			return string.Equals(text, text2);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000924A4 File Offset: 0x000914A4
		public unsafe static int CompareOrdinal(string a, byte[] bytes, int bCharLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = a.Length;
			if (num3 > bCharLength)
			{
				num3 = bCharLength;
			}
			if (bCharLength == 0)
			{
				if (a.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				fixed (byte* ptr = bytes)
				{
					byte* ptr2 = ptr;
					while (num < num3 && num2 == 0)
					{
						int num4 = (int)(*ptr2) | ((int)ptr2[1] << 8);
						num2 = (int)a[num++] - num4;
						ptr2 += 2;
					}
				}
				if (num2 != 0)
				{
					return num2;
				}
				return a.Length - bCharLength;
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0009252C File Offset: 0x0009152C
		public static int CompareOrdinal(byte[] bytes, int aCharLength, string b)
		{
			return -FastResourceComparer.CompareOrdinal(b, bytes, aCharLength);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x00092538 File Offset: 0x00091538
		internal unsafe static int CompareOrdinal(byte* a, int byteLen, string b)
		{
			int num = 0;
			int num2 = 0;
			int num3 = byteLen >> 1;
			if (num3 > b.Length)
			{
				num3 = b.Length;
			}
			while (num2 < num3 && num == 0)
			{
				char c = (char)((int)(*(a++)) | ((int)(*(a++)) << 8));
				num = (int)(c - b[num2++]);
			}
			if (num != 0)
			{
				return num;
			}
			return byteLen - b.Length * 2;
		}

		// Token: 0x04001508 RID: 5384
		internal static readonly FastResourceComparer Default = new FastResourceComparer();
	}
}

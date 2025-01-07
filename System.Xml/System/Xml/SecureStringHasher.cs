using System;
using System.Collections.Generic;

namespace System.Xml
{
	internal class SecureStringHasher : IEqualityComparer<string>
	{
		public SecureStringHasher()
		{
			this.hashCodeRandomizer = Environment.TickCount;
		}

		public SecureStringHasher(int hashCodeRandomizer)
		{
			this.hashCodeRandomizer = hashCodeRandomizer;
		}

		public int Compare(string x, string y)
		{
			return string.Compare(x, y, StringComparison.Ordinal);
		}

		public bool Equals(string x, string y)
		{
			return string.Equals(x, y, StringComparison.Ordinal);
		}

		public int GetHashCode(string key)
		{
			int num = this.hashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7) ^ (int)key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			return num - (num >> 5);
		}

		private int hashCodeRandomizer;
	}
}

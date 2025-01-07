using System;

namespace System.Xml
{
	public class NameTable : XmlNameTable
	{
		public NameTable()
		{
			this.mask = 31;
			this.entries = new NameTable.Entry[this.mask + 1];
			this.marvinHashSeed = MarvinHash.DefaultSeed;
		}

		public override string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key.Length == 0)
			{
				return string.Empty;
			}
			int num = this.ComputeHash32(key);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && entry.str.Equals(key))
				{
					return entry.str;
				}
			}
			return this.AddEntry(key, num);
		}

		public override string Add(char[] key, int start, int len)
		{
			if (len == 0)
			{
				return string.Empty;
			}
			if (start >= key.Length || start < 0 || (long)start + (long)len > (long)key.Length)
			{
				throw new IndexOutOfRangeException();
			}
			if (len < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = this.ComputeHash32(key, start, len);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && NameTable.TextEquals(entry.str, key, start, len))
				{
					return entry.str;
				}
			}
			return this.AddEntry(new string(key, start, len), num);
		}

		public override string Get(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				return string.Empty;
			}
			int num = this.ComputeHash32(value);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && entry.str.Equals(value))
				{
					return entry.str;
				}
			}
			return null;
		}

		public override string Get(char[] key, int start, int len)
		{
			if (len == 0)
			{
				return string.Empty;
			}
			if (start >= key.Length || start < 0 || (long)start + (long)len > (long)key.Length)
			{
				throw new IndexOutOfRangeException();
			}
			if (len < 0)
			{
				return null;
			}
			int num = this.ComputeHash32(key, start, len);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && NameTable.TextEquals(entry.str, key, start, len))
				{
					return entry.str;
				}
			}
			return null;
		}

		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this.mask;
			NameTable.Entry entry = new NameTable.Entry(str, hashCode, this.entries[num]);
			this.entries[num] = entry;
			if (this.count++ == this.mask)
			{
				this.Grow();
			}
			return entry.str;
		}

		private void Grow()
		{
			int num = this.mask * 2 + 1;
			NameTable.Entry[] array = this.entries;
			NameTable.Entry[] array2 = new NameTable.Entry[num + 1];
			foreach (NameTable.Entry entry in array)
			{
				while (entry != null)
				{
					int num2 = entry.hashCode & num;
					NameTable.Entry next = entry.next;
					entry.next = array2[num2];
					array2[num2] = entry;
					entry = next;
				}
			}
			this.entries = array2;
			this.mask = num;
		}

		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}

		private int ComputeHash32(string key)
		{
			return MarvinHash.ComputeHash32(key, this.marvinHashSeed);
		}

		private int ComputeHash32(char[] key, int start, int len)
		{
			return MarvinHash.ComputeHash32(key, start, len, this.marvinHashSeed);
		}

		private NameTable.Entry[] entries;

		private int count;

		private int mask;

		private int hashCodeRandomizer;

		private ulong marvinHashSeed;

		private class Entry
		{
			internal Entry(string str, int hashCode, NameTable.Entry next)
			{
				this.str = str;
				this.hashCode = hashCode;
				this.next = next;
			}

			internal string str;

			internal int hashCode;

			internal NameTable.Entry next;
		}
	}
}

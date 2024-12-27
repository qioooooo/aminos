using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class MatchCollection : ICollection, IEnumerable
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x0000D89C File Offset: 0x0000C89C
		internal MatchCollection(Regex regex, string input, int beginning, int length, int startat)
		{
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("startat", SR.GetString("BeginIndexNotNegative"));
			}
			this._regex = regex;
			this._input = input;
			this._beginning = beginning;
			this._length = length;
			this._startat = startat;
			this._prevlen = -1;
			this._matches = new ArrayList();
			this._done = false;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000D914 File Offset: 0x0000C914
		internal Match GetMatch(int i)
		{
			if (i < 0)
			{
				return null;
			}
			if (this._matches.Count > i)
			{
				return (Match)this._matches[i];
			}
			if (this._done)
			{
				return null;
			}
			for (;;)
			{
				Match match = this._regex.Run(false, this._prevlen, this._input, this._beginning, this._length, this._startat);
				if (!match.Success)
				{
					break;
				}
				this._matches.Add(match);
				this._prevlen = match._length;
				this._startat = match._textpos;
				if (this._matches.Count > i)
				{
					return match;
				}
			}
			this._done = true;
			return null;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000D9C1 File Offset: 0x0000C9C1
		public int Count
		{
			get
			{
				if (this._done)
				{
					return this._matches.Count;
				}
				this.GetMatch(MatchCollection.infinite);
				return this._matches.Count;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000D9EE File Offset: 0x0000C9EE
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000D9F1 File Offset: 0x0000C9F1
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000D9F4 File Offset: 0x0000C9F4
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003E RID: 62
		public virtual Match this[int i]
		{
			get
			{
				Match match = this.GetMatch(i);
				if (match == null)
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return match;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000DA1C File Offset: 0x0000CA1C
		public void CopyTo(Array array, int arrayIndex)
		{
			int count = this.Count;
			this._matches.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000DA32 File Offset: 0x0000CA32
		public IEnumerator GetEnumerator()
		{
			return new MatchEnumerator(this);
		}

		// Token: 0x04000756 RID: 1878
		internal Regex _regex;

		// Token: 0x04000757 RID: 1879
		internal ArrayList _matches;

		// Token: 0x04000758 RID: 1880
		internal bool _done;

		// Token: 0x04000759 RID: 1881
		internal string _input;

		// Token: 0x0400075A RID: 1882
		internal int _beginning;

		// Token: 0x0400075B RID: 1883
		internal int _length;

		// Token: 0x0400075C RID: 1884
		internal int _startat;

		// Token: 0x0400075D RID: 1885
		internal int _prevlen;

		// Token: 0x0400075E RID: 1886
		private static int infinite = int.MaxValue;
	}
}

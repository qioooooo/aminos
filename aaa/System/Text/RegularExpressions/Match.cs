using System;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public class Match : Group
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000D335 File Offset: 0x0000C335
		public static Match Empty
		{
			get
			{
				return Match._empty;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000D33C File Offset: 0x0000C33C
		internal Match(Regex regex, int capcount, string text, int begpos, int len, int startpos)
			: base(text, new int[2], 0)
		{
			this._regex = regex;
			this._matchcount = new int[capcount];
			this._matches = new int[capcount][];
			this._matches[0] = this._caps;
			this._textbeg = begpos;
			this._textend = begpos + len;
			this._textstart = startpos;
			this._balancing = false;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000D3A8 File Offset: 0x0000C3A8
		internal virtual void Reset(Regex regex, string text, int textbeg, int textend, int textstart)
		{
			this._regex = regex;
			this._text = text;
			this._textbeg = textbeg;
			this._textend = textend;
			this._textstart = textstart;
			for (int i = 0; i < this._matchcount.Length; i++)
			{
				this._matchcount[i] = 0;
			}
			this._balancing = false;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000D3FD File Offset: 0x0000C3FD
		public virtual GroupCollection Groups
		{
			get
			{
				if (this._groupcoll == null)
				{
					this._groupcoll = new GroupCollection(this, null);
				}
				return this._groupcoll;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000D41A File Offset: 0x0000C41A
		public Match NextMatch()
		{
			if (this._regex == null)
			{
				return this;
			}
			return this._regex.Run(false, this._length, this._text, this._textbeg, this._textend - this._textbeg, this._textpos);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000D458 File Offset: 0x0000C458
		public virtual string Result(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			if (this._regex == null)
			{
				throw new NotSupportedException(SR.GetString("NoResultOnFailed"));
			}
			RegexReplacement regexReplacement = (RegexReplacement)this._regex.replref.Get();
			if (regexReplacement == null || !regexReplacement.Pattern.Equals(replacement))
			{
				regexReplacement = RegexParser.ParseReplacement(replacement, this._regex.caps, this._regex.capsize, this._regex.capnames, this._regex.roptions);
				this._regex.replref.Cache(regexReplacement);
			}
			return regexReplacement.Replacement(this);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000D500 File Offset: 0x0000C500
		internal virtual string GroupToStringImpl(int groupnum)
		{
			int num = this._matchcount[groupnum];
			if (num == 0)
			{
				return string.Empty;
			}
			int[] array = this._matches[groupnum];
			return this._text.Substring(array[(num - 1) * 2], array[num * 2 - 1]);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000D541 File Offset: 0x0000C541
		internal string LastGroupToStringImpl()
		{
			return this.GroupToStringImpl(this._matchcount.Length - 1);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000D554 File Offset: 0x0000C554
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Match Synchronized(Match inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			int num = inner._matchcount.Length;
			for (int i = 0; i < num; i++)
			{
				Group group = inner.Groups[i];
				Group.Synchronized(group);
			}
			return inner;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000D59C File Offset: 0x0000C59C
		internal virtual void AddMatch(int cap, int start, int len)
		{
			if (this._matches[cap] == null)
			{
				this._matches[cap] = new int[2];
			}
			int num = this._matchcount[cap];
			if (num * 2 + 2 > this._matches[cap].Length)
			{
				int[] array = this._matches[cap];
				int[] array2 = new int[num * 8];
				for (int i = 0; i < num * 2; i++)
				{
					array2[i] = array[i];
				}
				this._matches[cap] = array2;
			}
			this._matches[cap][num * 2] = start;
			this._matches[cap][num * 2 + 1] = len;
			this._matchcount[cap] = num + 1;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000D634 File Offset: 0x0000C634
		internal virtual void BalanceMatch(int cap)
		{
			this._balancing = true;
			int num = this._matchcount[cap];
			int num2 = num * 2 - 2;
			if (this._matches[cap][num2] < 0)
			{
				num2 = -3 - this._matches[cap][num2];
			}
			num2 -= 2;
			if (num2 >= 0 && this._matches[cap][num2] < 0)
			{
				this.AddMatch(cap, this._matches[cap][num2], this._matches[cap][num2 + 1]);
				return;
			}
			this.AddMatch(cap, -3 - num2, -4 - num2);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000D6B4 File Offset: 0x0000C6B4
		internal virtual void RemoveMatch(int cap)
		{
			this._matchcount[cap]--;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000D6CF File Offset: 0x0000C6CF
		internal virtual bool IsMatched(int cap)
		{
			return cap < this._matchcount.Length && this._matchcount[cap] > 0 && this._matches[cap][this._matchcount[cap] * 2 - 1] != -2;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000D708 File Offset: 0x0000C708
		internal virtual int MatchIndex(int cap)
		{
			int num = this._matches[cap][this._matchcount[cap] * 2 - 2];
			if (num >= 0)
			{
				return num;
			}
			return this._matches[cap][-3 - num];
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000D740 File Offset: 0x0000C740
		internal virtual int MatchLength(int cap)
		{
			int num = this._matches[cap][this._matchcount[cap] * 2 - 1];
			if (num >= 0)
			{
				return num;
			}
			return this._matches[cap][-3 - num];
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000D778 File Offset: 0x0000C778
		internal virtual void Tidy(int textpos)
		{
			int[] array = this._matches[0];
			this._index = array[0];
			this._length = array[1];
			this._textpos = textpos;
			this._capcount = this._matchcount[0];
			if (this._balancing)
			{
				for (int i = 0; i < this._matchcount.Length; i++)
				{
					int num = this._matchcount[i] * 2;
					int[] array2 = this._matches[i];
					int j = 0;
					while (j < num && array2[j] >= 0)
					{
						j++;
					}
					int num2 = j;
					while (j < num)
					{
						if (array2[j] < 0)
						{
							num2--;
						}
						else
						{
							if (j != num2)
							{
								array2[num2] = array2[j];
							}
							num2++;
						}
						j++;
					}
					this._matchcount[i] = num2 / 2;
				}
				this._balancing = false;
			}
		}

		// Token: 0x0400074B RID: 1867
		internal static Match _empty = new Match(null, 1, string.Empty, 0, 0, 0);

		// Token: 0x0400074C RID: 1868
		internal GroupCollection _groupcoll;

		// Token: 0x0400074D RID: 1869
		internal Regex _regex;

		// Token: 0x0400074E RID: 1870
		internal int _textbeg;

		// Token: 0x0400074F RID: 1871
		internal int _textpos;

		// Token: 0x04000750 RID: 1872
		internal int _textend;

		// Token: 0x04000751 RID: 1873
		internal int _textstart;

		// Token: 0x04000752 RID: 1874
		internal int[][] _matches;

		// Token: 0x04000753 RID: 1875
		internal int[] _matchcount;

		// Token: 0x04000754 RID: 1876
		internal bool _balancing;
	}
}

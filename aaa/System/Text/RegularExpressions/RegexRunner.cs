using System;
using System.ComponentModel;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000023 RID: 35
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class RegexRunner
	{
		// Token: 0x06000164 RID: 356 RVA: 0x0000B52B File Offset: 0x0000A52B
		protected internal RegexRunner()
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B534 File Offset: 0x0000A534
		protected internal Match Scan(Regex regex, string text, int textbeg, int textend, int textstart, int prevlen, bool quick)
		{
			bool flag = false;
			this.runregex = regex;
			this.runtext = text;
			this.runtextbeg = textbeg;
			this.runtextend = textend;
			this.runtextstart = textstart;
			int num = (this.runregex.RightToLeft ? (-1) : 1);
			int num2 = (this.runregex.RightToLeft ? this.runtextbeg : this.runtextend);
			this.runtextpos = textstart;
			if (prevlen == 0)
			{
				if (this.runtextpos == num2)
				{
					return Match.Empty;
				}
				this.runtextpos += num;
			}
			for (;;)
			{
				if (this.FindFirstChar())
				{
					if (!flag)
					{
						this.InitMatch();
						flag = true;
					}
					this.Go();
					if (this.runmatch._matchcount[0] > 0)
					{
						break;
					}
					this.runtrackpos = this.runtrack.Length;
					this.runstackpos = this.runstack.Length;
					this.runcrawlpos = this.runcrawl.Length;
				}
				if (this.runtextpos == num2)
				{
					goto Block_8;
				}
				this.runtextpos += num;
			}
			return this.TidyMatch(quick);
			Block_8:
			this.TidyMatch(true);
			return Match.Empty;
		}

		// Token: 0x06000166 RID: 358
		protected abstract void Go();

		// Token: 0x06000167 RID: 359
		protected abstract bool FindFirstChar();

		// Token: 0x06000168 RID: 360
		protected abstract void InitTrackCount();

		// Token: 0x06000169 RID: 361 RVA: 0x0000B644 File Offset: 0x0000A644
		private void InitMatch()
		{
			if (this.runmatch == null)
			{
				if (this.runregex.caps != null)
				{
					this.runmatch = new MatchSparse(this.runregex, this.runregex.caps, this.runregex.capsize, this.runtext, this.runtextbeg, this.runtextend - this.runtextbeg, this.runtextstart);
				}
				else
				{
					this.runmatch = new Match(this.runregex, this.runregex.capsize, this.runtext, this.runtextbeg, this.runtextend - this.runtextbeg, this.runtextstart);
				}
			}
			else
			{
				this.runmatch.Reset(this.runregex, this.runtext, this.runtextbeg, this.runtextend, this.runtextstart);
			}
			if (this.runcrawl != null)
			{
				this.runtrackpos = this.runtrack.Length;
				this.runstackpos = this.runstack.Length;
				this.runcrawlpos = this.runcrawl.Length;
				return;
			}
			this.InitTrackCount();
			int num = this.runtrackcount * 8;
			int num2 = this.runtrackcount * 8;
			if (num < 32)
			{
				num = 32;
			}
			if (num2 < 16)
			{
				num2 = 16;
			}
			this.runtrack = new int[num];
			this.runtrackpos = num;
			this.runstack = new int[num2];
			this.runstackpos = num2;
			this.runcrawl = new int[32];
			this.runcrawlpos = 32;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B7B0 File Offset: 0x0000A7B0
		private Match TidyMatch(bool quick)
		{
			if (!quick)
			{
				Match match = this.runmatch;
				this.runmatch = null;
				match.Tidy(this.runtextpos);
				return match;
			}
			return null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000B7DD File Offset: 0x0000A7DD
		protected void EnsureStorage()
		{
			if (this.runstackpos < this.runtrackcount * 4)
			{
				this.DoubleStack();
			}
			if (this.runtrackpos < this.runtrackcount * 4)
			{
				this.DoubleTrack();
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B80B File Offset: 0x0000A80B
		protected bool IsBoundary(int index, int startpos, int endpos)
		{
			return (index > startpos && RegexCharClass.IsWordChar(this.runtext[index - 1])) != (index < endpos && RegexCharClass.IsWordChar(this.runtext[index]));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B844 File Offset: 0x0000A844
		protected bool IsECMABoundary(int index, int startpos, int endpos)
		{
			return (index > startpos && RegexCharClass.IsECMAWordChar(this.runtext[index - 1])) != (index < endpos && RegexCharClass.IsECMAWordChar(this.runtext[index]));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B880 File Offset: 0x0000A880
		protected static bool CharInSet(char ch, string set, string category)
		{
			string text = RegexCharClass.ConvertOldStringsToClass(set, category);
			return RegexCharClass.CharInClass(ch, text);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000B89C File Offset: 0x0000A89C
		protected static bool CharInClass(char ch, string charClass)
		{
			return RegexCharClass.CharInClass(ch, charClass);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000B8A8 File Offset: 0x0000A8A8
		protected void DoubleTrack()
		{
			int[] array = new int[this.runtrack.Length * 2];
			Array.Copy(this.runtrack, 0, array, this.runtrack.Length, this.runtrack.Length);
			this.runtrackpos += this.runtrack.Length;
			this.runtrack = array;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B900 File Offset: 0x0000A900
		protected void DoubleStack()
		{
			int[] array = new int[this.runstack.Length * 2];
			Array.Copy(this.runstack, 0, array, this.runstack.Length, this.runstack.Length);
			this.runstackpos += this.runstack.Length;
			this.runstack = array;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B958 File Offset: 0x0000A958
		protected void DoubleCrawl()
		{
			int[] array = new int[this.runcrawl.Length * 2];
			Array.Copy(this.runcrawl, 0, array, this.runcrawl.Length, this.runcrawl.Length);
			this.runcrawlpos += this.runcrawl.Length;
			this.runcrawl = array;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B9B0 File Offset: 0x0000A9B0
		protected void Crawl(int i)
		{
			if (this.runcrawlpos == 0)
			{
				this.DoubleCrawl();
			}
			this.runcrawl[--this.runcrawlpos] = i;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B9E4 File Offset: 0x0000A9E4
		protected int Popcrawl()
		{
			return this.runcrawl[this.runcrawlpos++];
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000BA09 File Offset: 0x0000AA09
		protected int Crawlpos()
		{
			return this.runcrawl.Length - this.runcrawlpos;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000BA1C File Offset: 0x0000AA1C
		protected void Capture(int capnum, int start, int end)
		{
			if (end < start)
			{
				int num = end;
				end = start;
				start = num;
			}
			this.Crawl(capnum);
			this.runmatch.AddMatch(capnum, start, end - start);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000BA4C File Offset: 0x0000AA4C
		protected void TransferCapture(int capnum, int uncapnum, int start, int end)
		{
			if (end < start)
			{
				int num = end;
				end = start;
				start = num;
			}
			int num2 = this.MatchIndex(uncapnum);
			int num3 = num2 + this.MatchLength(uncapnum);
			if (start >= num3)
			{
				end = start;
				start = num3;
			}
			else if (end <= num2)
			{
				start = num2;
			}
			else
			{
				if (end > num3)
				{
					end = num3;
				}
				if (num2 > start)
				{
					start = num2;
				}
			}
			this.Crawl(uncapnum);
			this.runmatch.BalanceMatch(uncapnum);
			if (capnum != -1)
			{
				this.Crawl(capnum);
				this.runmatch.AddMatch(capnum, start, end - start);
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000BAD0 File Offset: 0x0000AAD0
		protected void Uncapture()
		{
			int num = this.Popcrawl();
			this.runmatch.RemoveMatch(num);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000BAF0 File Offset: 0x0000AAF0
		protected bool IsMatched(int cap)
		{
			return this.runmatch.IsMatched(cap);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000BAFE File Offset: 0x0000AAFE
		protected int MatchIndex(int cap)
		{
			return this.runmatch.MatchIndex(cap);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000BB0C File Offset: 0x0000AB0C
		protected int MatchLength(int cap)
		{
			return this.runmatch.MatchLength(cap);
		}

		// Token: 0x04000732 RID: 1842
		protected internal int runtextbeg;

		// Token: 0x04000733 RID: 1843
		protected internal int runtextend;

		// Token: 0x04000734 RID: 1844
		protected internal int runtextstart;

		// Token: 0x04000735 RID: 1845
		protected internal string runtext;

		// Token: 0x04000736 RID: 1846
		protected internal int runtextpos;

		// Token: 0x04000737 RID: 1847
		protected internal int[] runtrack;

		// Token: 0x04000738 RID: 1848
		protected internal int runtrackpos;

		// Token: 0x04000739 RID: 1849
		protected internal int[] runstack;

		// Token: 0x0400073A RID: 1850
		protected internal int runstackpos;

		// Token: 0x0400073B RID: 1851
		protected internal int[] runcrawl;

		// Token: 0x0400073C RID: 1852
		protected internal int runcrawlpos;

		// Token: 0x0400073D RID: 1853
		protected internal int runtrackcount;

		// Token: 0x0400073E RID: 1854
		protected internal Match runmatch;

		// Token: 0x0400073F RID: 1855
		protected internal Regex runregex;
	}
}

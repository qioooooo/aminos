using System;
using System.Collections;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000799 RID: 1945
	internal sealed class TwoLevelFileEnumerator : IEnumerator
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x000EF98C File Offset: 0x000EE98C
		public TwoLevelFileEnumerator(string root)
		{
			this.m_Root = root;
			this.Reset();
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x000EF9A4 File Offset: 0x000EE9A4
		public bool MoveNext()
		{
			lock (this)
			{
				if (this.m_fReset)
				{
					this.m_fReset = false;
					return this.AdvanceRootDir();
				}
				if (this.m_RootDir.Length == 0)
				{
					return false;
				}
				this.m_nSubDir++;
				if (this.m_nSubDir >= this.m_SubDir.Length)
				{
					this.m_nSubDir = this.m_SubDir.Length;
					return this.AdvanceRootDir();
				}
				this.UpdateCurrent();
			}
			return true;
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x000EFA38 File Offset: 0x000EEA38
		private bool AdvanceRootDir()
		{
			this.m_nRootDir++;
			if (this.m_nRootDir >= this.m_RootDir.Length)
			{
				this.m_nRootDir = this.m_RootDir.Length;
				return false;
			}
			this.m_SubDir = Directory.GetDirectories(this.m_RootDir[this.m_nRootDir]);
			if (this.m_SubDir.Length == 0)
			{
				return this.AdvanceRootDir();
			}
			this.m_nSubDir = 0;
			this.UpdateCurrent();
			return true;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x000EFAAA File Offset: 0x000EEAAA
		private void UpdateCurrent()
		{
			this.m_Current.Path1 = Path.GetFileName(this.m_RootDir[this.m_nRootDir]);
			this.m_Current.Path2 = Path.GetFileName(this.m_SubDir[this.m_nSubDir]);
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x000EFAE6 File Offset: 0x000EEAE6
		public object Current
		{
			get
			{
				if (this.m_fReset)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (this.m_nRootDir >= this.m_RootDir.Length)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
				return this.m_Current;
			}
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x000EFB28 File Offset: 0x000EEB28
		public void Reset()
		{
			this.m_RootDir = null;
			this.m_nRootDir = -1;
			this.m_SubDir = null;
			this.m_nSubDir = -1;
			this.m_Current = new TwoPaths();
			this.m_fReset = true;
			this.m_RootDir = Directory.GetDirectories(this.m_Root);
		}

		// Token: 0x040022A5 RID: 8869
		private string m_Root;

		// Token: 0x040022A6 RID: 8870
		private TwoPaths m_Current;

		// Token: 0x040022A7 RID: 8871
		private bool m_fReset;

		// Token: 0x040022A8 RID: 8872
		private string[] m_RootDir;

		// Token: 0x040022A9 RID: 8873
		private int m_nRootDir;

		// Token: 0x040022AA RID: 8874
		private string[] m_SubDir;

		// Token: 0x040022AB RID: 8875
		private int m_nSubDir;
	}
}

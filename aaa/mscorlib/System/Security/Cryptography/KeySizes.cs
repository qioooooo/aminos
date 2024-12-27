using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200084C RID: 2124
	[ComVisible(true)]
	public sealed class KeySizes
	{
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0010FAAF File Offset: 0x0010EAAF
		public int MinSize
		{
			get
			{
				return this.m_minSize;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06004E14 RID: 19988 RVA: 0x0010FAB7 File Offset: 0x0010EAB7
		public int MaxSize
		{
			get
			{
				return this.m_maxSize;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0010FABF File Offset: 0x0010EABF
		public int SkipSize
		{
			get
			{
				return this.m_skipSize;
			}
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x0010FAC7 File Offset: 0x0010EAC7
		public KeySizes(int minSize, int maxSize, int skipSize)
		{
			this.m_minSize = minSize;
			this.m_maxSize = maxSize;
			this.m_skipSize = skipSize;
		}

		// Token: 0x0400283F RID: 10303
		private int m_minSize;

		// Token: 0x04002840 RID: 10304
		private int m_maxSize;

		// Token: 0x04002841 RID: 10305
		private int m_skipSize;
	}
}

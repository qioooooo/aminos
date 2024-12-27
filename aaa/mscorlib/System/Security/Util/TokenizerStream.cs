using System;

namespace System.Security.Util
{
	// Token: 0x0200060A RID: 1546
	internal sealed class TokenizerStream
	{
		// Token: 0x06003839 RID: 14393 RVA: 0x000BE7BE File Offset: 0x000BD7BE
		internal TokenizerStream()
		{
			this.m_countTokens = 0;
			this.m_headTokens = new TokenizerShortBlock();
			this.m_headStrings = new TokenizerStringBlock();
			this.Reset();
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x000BE7EC File Offset: 0x000BD7EC
		internal void AddToken(short token)
		{
			if (this.m_currentTokens.m_block.Length <= this.m_indexTokens)
			{
				this.m_currentTokens.m_next = new TokenizerShortBlock();
				this.m_currentTokens = this.m_currentTokens.m_next;
				this.m_indexTokens = 0;
			}
			this.m_countTokens++;
			this.m_currentTokens.m_block[this.m_indexTokens++] = token;
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000BE864 File Offset: 0x000BD864
		internal void AddString(string str)
		{
			if (this.m_currentStrings.m_block.Length <= this.m_indexStrings)
			{
				this.m_currentStrings.m_next = new TokenizerStringBlock();
				this.m_currentStrings = this.m_currentStrings.m_next;
				this.m_indexStrings = 0;
			}
			this.m_currentStrings.m_block[this.m_indexStrings++] = str;
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000BE8CC File Offset: 0x000BD8CC
		internal void Reset()
		{
			this.m_lastTokens = null;
			this.m_currentTokens = this.m_headTokens;
			this.m_currentStrings = this.m_headStrings;
			this.m_indexTokens = 0;
			this.m_indexStrings = 0;
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000BE8FC File Offset: 0x000BD8FC
		internal short GetNextFullToken()
		{
			if (this.m_currentTokens.m_block.Length <= this.m_indexTokens)
			{
				this.m_lastTokens = this.m_currentTokens;
				this.m_currentTokens = this.m_currentTokens.m_next;
				this.m_indexTokens = 0;
			}
			return this.m_currentTokens.m_block[this.m_indexTokens++];
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000BE960 File Offset: 0x000BD960
		internal short GetNextToken()
		{
			return this.GetNextFullToken() & 255;
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000BE97C File Offset: 0x000BD97C
		internal string GetNextString()
		{
			if (this.m_currentStrings.m_block.Length <= this.m_indexStrings)
			{
				this.m_currentStrings = this.m_currentStrings.m_next;
				this.m_indexStrings = 0;
			}
			return this.m_currentStrings.m_block[this.m_indexStrings++];
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000BE9D3 File Offset: 0x000BD9D3
		internal void ThrowAwayNextString()
		{
			this.GetNextString();
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000BE9DC File Offset: 0x000BD9DC
		internal void TagLastToken(short tag)
		{
			if (this.m_indexTokens == 0)
			{
				this.m_lastTokens.m_block[this.m_lastTokens.m_block.Length - 1] = (short)((ushort)this.m_lastTokens.m_block[this.m_lastTokens.m_block.Length - 1] | (ushort)tag);
				return;
			}
			this.m_currentTokens.m_block[this.m_indexTokens - 1] = (short)((ushort)this.m_currentTokens.m_block[this.m_indexTokens - 1] | (ushort)tag);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000BEA5A File Offset: 0x000BDA5A
		internal int GetTokenCount()
		{
			return this.m_countTokens;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000BEA64 File Offset: 0x000BDA64
		internal void GoToPosition(int position)
		{
			this.Reset();
			for (int i = 0; i < position; i++)
			{
				if (this.GetNextToken() == 3)
				{
					this.ThrowAwayNextString();
				}
			}
		}

		// Token: 0x04001D25 RID: 7461
		private int m_countTokens;

		// Token: 0x04001D26 RID: 7462
		private TokenizerShortBlock m_headTokens;

		// Token: 0x04001D27 RID: 7463
		private TokenizerShortBlock m_lastTokens;

		// Token: 0x04001D28 RID: 7464
		private TokenizerShortBlock m_currentTokens;

		// Token: 0x04001D29 RID: 7465
		private int m_indexTokens;

		// Token: 0x04001D2A RID: 7466
		private TokenizerStringBlock m_headStrings;

		// Token: 0x04001D2B RID: 7467
		private TokenizerStringBlock m_currentStrings;

		// Token: 0x04001D2C RID: 7468
		private int m_indexStrings;
	}
}

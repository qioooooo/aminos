using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	// Token: 0x02000815 RID: 2069
	internal class LineNumberInfo
	{
		// Token: 0x06004A20 RID: 18976 RVA: 0x001024D7 File Offset: 0x001014D7
		internal LineNumberInfo()
		{
			this.m_DocumentCount = 0;
			this.m_iLastFound = 0;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x001024F0 File Offset: 0x001014F0
		internal void AddLineNumberInfo(ISymbolDocumentWriter document, int iOffset, int iStartLine, int iStartColumn, int iEndLine, int iEndColumn)
		{
			int num = this.FindDocument(document);
			this.m_Documents[num].AddLineNumberInfo(document, iOffset, iStartLine, iStartColumn, iEndLine, iEndColumn);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0010251C File Offset: 0x0010151C
		internal int FindDocument(ISymbolDocumentWriter document)
		{
			if (this.m_iLastFound < this.m_DocumentCount && this.m_Documents[this.m_iLastFound] == document)
			{
				return this.m_iLastFound;
			}
			for (int i = 0; i < this.m_DocumentCount; i++)
			{
				if (this.m_Documents[i].m_document == document)
				{
					this.m_iLastFound = i;
					return this.m_iLastFound;
				}
			}
			this.EnsureCapacity();
			this.m_iLastFound = this.m_DocumentCount;
			this.m_Documents[this.m_DocumentCount++] = new REDocument(document);
			return this.m_iLastFound;
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x001025B4 File Offset: 0x001015B4
		internal void EnsureCapacity()
		{
			if (this.m_DocumentCount == 0)
			{
				this.m_Documents = new REDocument[16];
				return;
			}
			if (this.m_DocumentCount == this.m_Documents.Length)
			{
				REDocument[] array = new REDocument[this.m_DocumentCount * 2];
				Array.Copy(this.m_Documents, array, this.m_DocumentCount);
				this.m_Documents = array;
			}
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00102610 File Offset: 0x00101610
		internal void EmitLineNumberInfo(ISymbolWriter symWriter)
		{
			for (int i = 0; i < this.m_DocumentCount; i++)
			{
				this.m_Documents[i].EmitLineNumberInfo(symWriter);
			}
		}

		// Token: 0x040025C3 RID: 9667
		internal const int InitialSize = 16;

		// Token: 0x040025C4 RID: 9668
		internal int m_DocumentCount;

		// Token: 0x040025C5 RID: 9669
		internal REDocument[] m_Documents;

		// Token: 0x040025C6 RID: 9670
		private int m_iLastFound;
	}
}

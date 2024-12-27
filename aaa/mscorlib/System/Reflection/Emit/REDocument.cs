using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	// Token: 0x02000816 RID: 2070
	internal class REDocument
	{
		// Token: 0x06004A25 RID: 18981 RVA: 0x0010263C File Offset: 0x0010163C
		internal REDocument(ISymbolDocumentWriter document)
		{
			this.m_iLineNumberCount = 0;
			this.m_document = document;
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x00102654 File Offset: 0x00101654
		internal void AddLineNumberInfo(ISymbolDocumentWriter document, int iOffset, int iStartLine, int iStartColumn, int iEndLine, int iEndColumn)
		{
			this.EnsureCapacity();
			this.m_iOffsets[this.m_iLineNumberCount] = iOffset;
			this.m_iLines[this.m_iLineNumberCount] = iStartLine;
			this.m_iColumns[this.m_iLineNumberCount] = iStartColumn;
			this.m_iEndLines[this.m_iLineNumberCount] = iEndLine;
			this.m_iEndColumns[this.m_iLineNumberCount] = iEndColumn;
			this.m_iLineNumberCount++;
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x001026C0 File Offset: 0x001016C0
		internal void EnsureCapacity()
		{
			if (this.m_iLineNumberCount == 0)
			{
				this.m_iOffsets = new int[16];
				this.m_iLines = new int[16];
				this.m_iColumns = new int[16];
				this.m_iEndLines = new int[16];
				this.m_iEndColumns = new int[16];
				return;
			}
			if (this.m_iLineNumberCount == this.m_iOffsets.Length)
			{
				int[] array = new int[this.m_iLineNumberCount * 2];
				Array.Copy(this.m_iOffsets, array, this.m_iLineNumberCount);
				this.m_iOffsets = array;
				array = new int[this.m_iLineNumberCount * 2];
				Array.Copy(this.m_iLines, array, this.m_iLineNumberCount);
				this.m_iLines = array;
				array = new int[this.m_iLineNumberCount * 2];
				Array.Copy(this.m_iColumns, array, this.m_iLineNumberCount);
				this.m_iColumns = array;
				array = new int[this.m_iLineNumberCount * 2];
				Array.Copy(this.m_iEndLines, array, this.m_iLineNumberCount);
				this.m_iEndLines = array;
				array = new int[this.m_iLineNumberCount * 2];
				Array.Copy(this.m_iEndColumns, array, this.m_iLineNumberCount);
				this.m_iEndColumns = array;
			}
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x001027F0 File Offset: 0x001017F0
		internal void EmitLineNumberInfo(ISymbolWriter symWriter)
		{
			if (this.m_iLineNumberCount == 0)
			{
				return;
			}
			int[] array = new int[this.m_iLineNumberCount];
			Array.Copy(this.m_iOffsets, array, this.m_iLineNumberCount);
			int[] array2 = new int[this.m_iLineNumberCount];
			Array.Copy(this.m_iLines, array2, this.m_iLineNumberCount);
			int[] array3 = new int[this.m_iLineNumberCount];
			Array.Copy(this.m_iColumns, array3, this.m_iLineNumberCount);
			int[] array4 = new int[this.m_iLineNumberCount];
			Array.Copy(this.m_iEndLines, array4, this.m_iLineNumberCount);
			int[] array5 = new int[this.m_iLineNumberCount];
			Array.Copy(this.m_iEndColumns, array5, this.m_iLineNumberCount);
			symWriter.DefineSequencePoints(this.m_document, array, array2, array3, array4, array5);
		}

		// Token: 0x040025C7 RID: 9671
		internal const int InitialSize = 16;

		// Token: 0x040025C8 RID: 9672
		internal int[] m_iOffsets;

		// Token: 0x040025C9 RID: 9673
		internal int[] m_iLines;

		// Token: 0x040025CA RID: 9674
		internal int[] m_iColumns;

		// Token: 0x040025CB RID: 9675
		internal int[] m_iEndLines;

		// Token: 0x040025CC RID: 9676
		internal int[] m_iEndColumns;

		// Token: 0x040025CD RID: 9677
		internal ISymbolDocumentWriter m_document;

		// Token: 0x040025CE RID: 9678
		internal int m_iLineNumberCount;
	}
}

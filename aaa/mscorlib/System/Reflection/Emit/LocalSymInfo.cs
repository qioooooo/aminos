using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	// Token: 0x0200081B RID: 2075
	internal class LocalSymInfo
	{
		// Token: 0x06004A88 RID: 19080 RVA: 0x00103B04 File Offset: 0x00102B04
		internal LocalSymInfo()
		{
			this.m_iLocalSymCount = 0;
			this.m_iNameSpaceCount = 0;
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x00103B1C File Offset: 0x00102B1C
		private void EnsureCapacityNamespace()
		{
			if (this.m_iNameSpaceCount == 0)
			{
				this.m_namespace = new string[16];
				return;
			}
			if (this.m_iNameSpaceCount == this.m_namespace.Length)
			{
				string[] array = new string[this.m_iNameSpaceCount * 2];
				Array.Copy(this.m_namespace, array, this.m_iNameSpaceCount);
				this.m_namespace = array;
			}
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x00103B78 File Offset: 0x00102B78
		private void EnsureCapacity()
		{
			if (this.m_iLocalSymCount == 0)
			{
				this.m_strName = new string[16];
				this.m_ubSignature = new byte[16][];
				this.m_iLocalSlot = new int[16];
				this.m_iStartOffset = new int[16];
				this.m_iEndOffset = new int[16];
				return;
			}
			if (this.m_iLocalSymCount == this.m_strName.Length)
			{
				int[] array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iLocalSlot, array, this.m_iLocalSymCount);
				this.m_iLocalSlot = array;
				array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iStartOffset, array, this.m_iLocalSymCount);
				this.m_iStartOffset = array;
				array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iEndOffset, array, this.m_iLocalSymCount);
				this.m_iEndOffset = array;
				string[] array2 = new string[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_strName, array2, this.m_iLocalSymCount);
				this.m_strName = array2;
				byte[][] array3 = new byte[this.m_iLocalSymCount * 2][];
				Array.Copy(this.m_ubSignature, array3, this.m_iLocalSymCount);
				this.m_ubSignature = array3;
			}
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x00103CA8 File Offset: 0x00102CA8
		internal void AddLocalSymInfo(string strName, byte[] signature, int slot, int startOffset, int endOffset)
		{
			this.EnsureCapacity();
			this.m_iStartOffset[this.m_iLocalSymCount] = startOffset;
			this.m_iEndOffset[this.m_iLocalSymCount] = endOffset;
			this.m_iLocalSlot[this.m_iLocalSymCount] = slot;
			this.m_strName[this.m_iLocalSymCount] = strName;
			this.m_ubSignature[this.m_iLocalSymCount] = signature;
			this.m_iLocalSymCount++;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x00103D14 File Offset: 0x00102D14
		internal void AddUsingNamespace(string strNamespace)
		{
			this.EnsureCapacityNamespace();
			this.m_namespace[this.m_iNameSpaceCount++] = strNamespace;
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x00103D40 File Offset: 0x00102D40
		internal virtual void EmitLocalSymInfo(ISymbolWriter symWriter)
		{
			for (int i = 0; i < this.m_iLocalSymCount; i++)
			{
				symWriter.DefineLocalVariable(this.m_strName[i], FieldAttributes.PrivateScope, this.m_ubSignature[i], SymAddressKind.ILOffset, this.m_iLocalSlot[i], 0, 0, this.m_iStartOffset[i], this.m_iEndOffset[i]);
			}
			for (int i = 0; i < this.m_iNameSpaceCount; i++)
			{
				symWriter.UsingNamespace(this.m_namespace[i]);
			}
		}

		// Token: 0x040025F6 RID: 9718
		internal const int InitialSize = 16;

		// Token: 0x040025F7 RID: 9719
		internal string[] m_strName;

		// Token: 0x040025F8 RID: 9720
		internal byte[][] m_ubSignature;

		// Token: 0x040025F9 RID: 9721
		internal int[] m_iLocalSlot;

		// Token: 0x040025FA RID: 9722
		internal int[] m_iStartOffset;

		// Token: 0x040025FB RID: 9723
		internal int[] m_iEndOffset;

		// Token: 0x040025FC RID: 9724
		internal int m_iLocalSymCount;

		// Token: 0x040025FD RID: 9725
		internal string[] m_namespace;

		// Token: 0x040025FE RID: 9726
		internal int m_iNameSpaceCount;
	}
}

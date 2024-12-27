using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x02000101 RID: 257
	internal struct Utf8String
	{
		// Token: 0x06000EB5 RID: 3765
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool EqualsCaseSensitive(void* szLhs, void* szRhs, int cSz);

		// Token: 0x06000EB6 RID: 3766
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool EqualsCaseInsensitive(void* szLhs, void* szRhs, int cSz);

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0002C160 File Offset: 0x0002B160
		private unsafe static int GetUtf8StringByteLength(void* pUtf8String)
		{
			int num = 0;
			byte* ptr = (byte*)pUtf8String;
			while (*ptr != 0)
			{
				num++;
				ptr++;
			}
			return num;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0002C181 File Offset: 0x0002B181
		internal unsafe Utf8String(void* pStringHeap)
		{
			this.m_pStringHeap = pStringHeap;
			if (pStringHeap != null)
			{
				this.m_StringHeapByteLength = Utf8String.GetUtf8StringByteLength(pStringHeap);
				return;
			}
			this.m_StringHeapByteLength = 0;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0002C1A3 File Offset: 0x0002B1A3
		internal unsafe Utf8String(void* pUtf8String, int cUtf8String)
		{
			this.m_pStringHeap = pUtf8String;
			this.m_StringHeapByteLength = cUtf8String;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0002C1B4 File Offset: 0x0002B1B4
		internal bool Equals(Utf8String s)
		{
			if (this.m_pStringHeap == null)
			{
				return s.m_StringHeapByteLength == 0;
			}
			return s.m_StringHeapByteLength == this.m_StringHeapByteLength && this.m_StringHeapByteLength != 0 && Utf8String.EqualsCaseSensitive(s.m_pStringHeap, this.m_pStringHeap, this.m_StringHeapByteLength);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0002C208 File Offset: 0x0002B208
		internal bool EqualsCaseInsensitive(Utf8String s)
		{
			if (this.m_pStringHeap == null)
			{
				return s.m_StringHeapByteLength == 0;
			}
			return s.m_StringHeapByteLength == this.m_StringHeapByteLength && this.m_StringHeapByteLength != 0 && Utf8String.EqualsCaseInsensitive(s.m_pStringHeap, this.m_pStringHeap, this.m_StringHeapByteLength);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0002C25C File Offset: 0x0002B25C
		public unsafe override string ToString()
		{
			byte* ptr = stackalloc byte[1 * this.m_StringHeapByteLength];
			byte* ptr2 = (byte*)this.m_pStringHeap;
			for (int i = 0; i < this.m_StringHeapByteLength; i++)
			{
				ptr[i] = *ptr2;
				ptr2++;
			}
			if (this.m_StringHeapByteLength == 0)
			{
				return "";
			}
			int charCount = Encoding.UTF8.GetCharCount(ptr, this.m_StringHeapByteLength);
			char* ptr3 = stackalloc char[2 * charCount];
			Encoding.UTF8.GetChars(ptr, this.m_StringHeapByteLength, ptr3, charCount);
			return new string(ptr3, 0, charCount);
		}

		// Token: 0x040004F0 RID: 1264
		private unsafe void* m_pStringHeap;

		// Token: 0x040004F1 RID: 1265
		private int m_StringHeapByteLength;
	}
}

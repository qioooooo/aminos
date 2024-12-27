using System;

namespace System
{
	// Token: 0x02000123 RID: 291
	internal struct UnSafeCharBuffer
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x0002FAA0 File Offset: 0x0002EAA0
		public unsafe UnSafeCharBuffer(char* buffer, int bufferSize)
		{
			this.m_buffer = buffer;
			this.m_totalSize = bufferSize;
			this.m_length = 0;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0002FAB8 File Offset: 0x0002EAB8
		public unsafe void AppendString(string stringToAppend)
		{
			if (string.IsNullOrEmpty(stringToAppend))
			{
				return;
			}
			if (this.m_totalSize - this.m_length < stringToAppend.Length)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* ptr = stringToAppend)
			{
				Buffer.memcpyimpl((byte*)ptr, (byte*)(this.m_buffer + this.m_length), stringToAppend.Length * 2);
			}
			this.m_length += stringToAppend.Length;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0002FB2A File Offset: 0x0002EB2A
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x0400058A RID: 1418
		private unsafe char* m_buffer;

		// Token: 0x0400058B RID: 1419
		private int m_totalSize;

		// Token: 0x0400058C RID: 1420
		private int m_length;
	}
}

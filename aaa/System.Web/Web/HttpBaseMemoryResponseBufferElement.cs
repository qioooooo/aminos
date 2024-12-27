using System;
using System.Text;

namespace System.Web
{
	// Token: 0x02000097 RID: 151
	internal abstract class HttpBaseMemoryResponseBufferElement
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00022D1A File Offset: 0x00021D1A
		internal int FreeBytes
		{
			get
			{
				return this._free;
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00022D22 File Offset: 0x00021D22
		internal void DisableRecycling()
		{
			this._recycle = false;
		}

		// Token: 0x060007CA RID: 1994
		internal abstract void Recycle();

		// Token: 0x060007CB RID: 1995
		internal abstract HttpResponseBufferElement Clone();

		// Token: 0x060007CC RID: 1996
		internal abstract int Append(byte[] data, int offset, int size);

		// Token: 0x060007CD RID: 1997
		internal abstract int Append(IntPtr data, int offset, int size);

		// Token: 0x060007CE RID: 1998
		internal abstract void AppendEncodedChars(char[] data, int offset, int size, Encoder encoder, bool flushEncoder);

		// Token: 0x0400116E RID: 4462
		protected int _size;

		// Token: 0x0400116F RID: 4463
		protected int _free;

		// Token: 0x04001170 RID: 4464
		protected bool _recycle;
	}
}

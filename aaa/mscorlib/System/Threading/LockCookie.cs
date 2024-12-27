using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000145 RID: 325
	[ComVisible(true)]
	public struct LockCookie
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x000331EE File Offset: 0x000321EE
		public override int GetHashCode()
		{
			return this._dwFlags + this._dwWriterSeqNum + this._wReaderAndWriterLevel + this._dwThreadID;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0003320B File Offset: 0x0003220B
		public override bool Equals(object obj)
		{
			return obj is LockCookie && this.Equals((LockCookie)obj);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00033223 File Offset: 0x00032223
		public bool Equals(LockCookie obj)
		{
			return obj._dwFlags == this._dwFlags && obj._dwWriterSeqNum == this._dwWriterSeqNum && obj._wReaderAndWriterLevel == this._wReaderAndWriterLevel && obj._dwThreadID == this._dwThreadID;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00033263 File Offset: 0x00032263
		public static bool operator ==(LockCookie a, LockCookie b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0003326D File Offset: 0x0003226D
		public static bool operator !=(LockCookie a, LockCookie b)
		{
			return !(a == b);
		}

		// Token: 0x0400060D RID: 1549
		private int _dwFlags;

		// Token: 0x0400060E RID: 1550
		private int _dwWriterSeqNum;

		// Token: 0x0400060F RID: 1551
		private int _wReaderAndWriterLevel;

		// Token: 0x04000610 RID: 1552
		private int _dwThreadID;
	}
}

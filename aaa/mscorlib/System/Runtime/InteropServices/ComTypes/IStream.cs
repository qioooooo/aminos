using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000566 RID: 1382
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IStream
	{
		// Token: 0x060033C6 RID: 13254
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x060033C7 RID: 13255
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x060033C8 RID: 13256
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x060033C9 RID: 13257
		void SetSize(long libNewSize);

		// Token: 0x060033CA RID: 13258
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x060033CB RID: 13259
		void Commit(int grfCommitFlags);

		// Token: 0x060033CC RID: 13260
		void Revert();

		// Token: 0x060033CD RID: 13261
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060033CE RID: 13262
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x060033CF RID: 13263
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x060033D0 RID: 13264
		void Clone(out IStream ppstm);
	}
}

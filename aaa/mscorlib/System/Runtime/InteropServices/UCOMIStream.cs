using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000533 RID: 1331
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IStream instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIStream
	{
		// Token: 0x0600333E RID: 13118
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x0600333F RID: 13119
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x06003340 RID: 13120
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x06003341 RID: 13121
		void SetSize(long libNewSize);

		// Token: 0x06003342 RID: 13122
		void CopyTo(UCOMIStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x06003343 RID: 13123
		void Commit(int grfCommitFlags);

		// Token: 0x06003344 RID: 13124
		void Revert();

		// Token: 0x06003345 RID: 13125
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06003346 RID: 13126
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06003347 RID: 13127
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x06003348 RID: 13128
		void Clone(out UCOMIStream ppstm);
	}
}

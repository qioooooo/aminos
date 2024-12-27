using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x02000081 RID: 129
	internal class CrmMonitorLogRecords
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00004048 File Offset: 0x00003448
		public unsafe CrmMonitorLogRecords(IntPtr mon)
		{
			IUnknown* ptr = mon.ToInt32();
			if (ptr == null)
			{
				throw new NullReferenceException();
			}
			ICrmMonitorLogRecords* ptr2;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_ICrmMonitorLogRecords, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			this._pMon = ptr2;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000040BC File Offset: 0x000034BC
		public unsafe int GetCount()
		{
			ICrmMonitorLogRecords* pMon = this._pMon;
			int num2;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)*), pMon, (int*)(&num2), (IntPtr)(*(*(int*)pMon + 12)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return num2;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000040EC File Offset: 0x000034EC
		public unsafe int GetTransactionState()
		{
			ICrmMonitorLogRecords* pMon = this._pMon;
			int num2;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32*), pMon, &num2, (IntPtr)(*(*(int*)pMon + 16)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return num2;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000411C File Offset: 0x0000351C
		public unsafe _LogRecord GetLogRecord(int index)
		{
			ICrmMonitorLogRecords* pMon = this._pMon;
			tagCrmLogRecordRead tagCrmLogRecordRead;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.EnterpriseServices.Thunk.tagCrmLogRecordRead*), pMon, index, &tagCrmLogRecordRead, (IntPtr)(*(*(int*)pMon + 24)));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			_LogRecord logRecord = default(_LogRecord);
			logRecord.dwCrmFlags = tagCrmLogRecordRead;
			logRecord.dwSequenceNumber = *((ref tagCrmLogRecordRead) + 4);
			logRecord.blobUserData.cbSize = *((ref tagCrmLogRecordRead) + 8);
			IntPtr intPtr = new IntPtr(*((ref tagCrmLogRecordRead) + 12));
			logRecord.blobUserData.pBlobData = intPtr;
			return logRecord;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004090 File Offset: 0x00003490
		public unsafe void Dispose()
		{
			ICrmMonitorLogRecords* pMon = this._pMon;
			if (pMon != null)
			{
				ICrmMonitorLogRecords* ptr = pMon;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
				this._pMon = null;
			}
		}

		// Token: 0x040000B2 RID: 178
		private unsafe ICrmMonitorLogRecords* _pMon;
	}
}

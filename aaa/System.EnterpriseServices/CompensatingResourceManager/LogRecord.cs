using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AD RID: 173
	public sealed class LogRecord
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x0000D092 File Offset: 0x0000C092
		internal LogRecord()
		{
			this._flags = (LogRecordFlags)0;
			this._seq = 0;
			this._data = null;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000D0AF File Offset: 0x0000C0AF
		internal LogRecord(_LogRecord r)
		{
			this._flags = (LogRecordFlags)r.dwCrmFlags;
			this._seq = r.dwSequenceNumber;
			this._data = Packager.Deserialize(new BlobPackage(r.blobUserData));
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000D0E8 File Offset: 0x0000C0E8
		public LogRecordFlags Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000D0F0 File Offset: 0x0000C0F0
		public int Sequence
		{
			get
			{
				return this._seq;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000D0F8 File Offset: 0x0000C0F8
		public object Record
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x040001DE RID: 478
		internal LogRecordFlags _flags;

		// Token: 0x040001DF RID: 479
		internal int _seq;

		// Token: 0x040001E0 RID: 480
		internal object _data;
	}
}

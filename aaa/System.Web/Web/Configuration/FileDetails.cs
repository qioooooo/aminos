using System;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001DB RID: 475
	internal class FileDetails
	{
		// Token: 0x06001A89 RID: 6793 RVA: 0x0007B8A7 File Offset: 0x0007A8A7
		internal FileDetails(bool exists, long fileSize, DateTime utcCreationTime, DateTime utcLastWriteTime)
		{
			this._exists = exists;
			this._fileSize = fileSize;
			this._utcCreationTime = utcCreationTime;
			this._utcLastWriteTime = utcLastWriteTime;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0007B8CC File Offset: 0x0007A8CC
		public override bool Equals(object obj)
		{
			FileDetails fileDetails = obj as FileDetails;
			return fileDetails != null && this._exists == fileDetails._exists && this._fileSize == fileDetails._fileSize && this._utcCreationTime == fileDetails._utcCreationTime && this._utcLastWriteTime == fileDetails._utcLastWriteTime;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0007B925 File Offset: 0x0007A925
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this._exists.GetHashCode(), this._fileSize.GetHashCode(), this._utcCreationTime.GetHashCode(), this._utcLastWriteTime.GetHashCode());
		}

		// Token: 0x040017DE RID: 6110
		private bool _exists;

		// Token: 0x040017DF RID: 6111
		private long _fileSize;

		// Token: 0x040017E0 RID: 6112
		private DateTime _utcCreationTime;

		// Token: 0x040017E1 RID: 6113
		private DateTime _utcLastWriteTime;
	}
}

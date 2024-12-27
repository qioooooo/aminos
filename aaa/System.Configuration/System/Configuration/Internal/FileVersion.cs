using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000BA RID: 186
	internal class FileVersion
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x0001F851 File Offset: 0x0001E851
		internal FileVersion(bool exists, long fileSize, DateTime utcCreationTime, DateTime utcLastWriteTime)
		{
			this._exists = exists;
			this._fileSize = fileSize;
			this._utcCreationTime = utcCreationTime;
			this._utcLastWriteTime = utcLastWriteTime;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001F878 File Offset: 0x0001E878
		public override bool Equals(object obj)
		{
			FileVersion fileVersion = obj as FileVersion;
			return fileVersion != null && this._exists == fileVersion._exists && this._fileSize == fileVersion._fileSize && this._utcCreationTime == fileVersion._utcCreationTime && this._utcLastWriteTime == fileVersion._utcLastWriteTime;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001F8D1 File Offset: 0x0001E8D1
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400041C RID: 1052
		private bool _exists;

		// Token: 0x0400041D RID: 1053
		private long _fileSize;

		// Token: 0x0400041E RID: 1054
		private DateTime _utcCreationTime;

		// Token: 0x0400041F RID: 1055
		private DateTime _utcLastWriteTime;
	}
}

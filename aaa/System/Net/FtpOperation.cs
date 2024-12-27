using System;

namespace System.Net
{
	// Token: 0x020003BD RID: 957
	internal enum FtpOperation
	{
		// Token: 0x04001DEA RID: 7658
		DownloadFile,
		// Token: 0x04001DEB RID: 7659
		ListDirectory,
		// Token: 0x04001DEC RID: 7660
		ListDirectoryDetails,
		// Token: 0x04001DED RID: 7661
		UploadFile,
		// Token: 0x04001DEE RID: 7662
		UploadFileUnique,
		// Token: 0x04001DEF RID: 7663
		AppendFile,
		// Token: 0x04001DF0 RID: 7664
		DeleteFile,
		// Token: 0x04001DF1 RID: 7665
		GetDateTimestamp,
		// Token: 0x04001DF2 RID: 7666
		GetFileSize,
		// Token: 0x04001DF3 RID: 7667
		Rename,
		// Token: 0x04001DF4 RID: 7668
		MakeDirectory,
		// Token: 0x04001DF5 RID: 7669
		RemoveDirectory,
		// Token: 0x04001DF6 RID: 7670
		PrintWorkingDirectory,
		// Token: 0x04001DF7 RID: 7671
		Other
	}
}

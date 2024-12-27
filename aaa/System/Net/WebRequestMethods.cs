using System;

namespace System.Net
{
	// Token: 0x020003B9 RID: 953
	public static class WebRequestMethods
	{
		// Token: 0x020003BA RID: 954
		public static class Ftp
		{
			// Token: 0x04001DD4 RID: 7636
			public const string DownloadFile = "RETR";

			// Token: 0x04001DD5 RID: 7637
			public const string ListDirectory = "NLST";

			// Token: 0x04001DD6 RID: 7638
			public const string UploadFile = "STOR";

			// Token: 0x04001DD7 RID: 7639
			public const string DeleteFile = "DELE";

			// Token: 0x04001DD8 RID: 7640
			public const string AppendFile = "APPE";

			// Token: 0x04001DD9 RID: 7641
			public const string GetFileSize = "SIZE";

			// Token: 0x04001DDA RID: 7642
			public const string UploadFileWithUniqueName = "STOU";

			// Token: 0x04001DDB RID: 7643
			public const string MakeDirectory = "MKD";

			// Token: 0x04001DDC RID: 7644
			public const string RemoveDirectory = "RMD";

			// Token: 0x04001DDD RID: 7645
			public const string ListDirectoryDetails = "LIST";

			// Token: 0x04001DDE RID: 7646
			public const string GetDateTimestamp = "MDTM";

			// Token: 0x04001DDF RID: 7647
			public const string PrintWorkingDirectory = "PWD";

			// Token: 0x04001DE0 RID: 7648
			public const string Rename = "RENAME";
		}

		// Token: 0x020003BB RID: 955
		public static class Http
		{
			// Token: 0x04001DE1 RID: 7649
			public const string Get = "GET";

			// Token: 0x04001DE2 RID: 7650
			public const string Connect = "CONNECT";

			// Token: 0x04001DE3 RID: 7651
			public const string Head = "HEAD";

			// Token: 0x04001DE4 RID: 7652
			public const string Put = "PUT";

			// Token: 0x04001DE5 RID: 7653
			public const string Post = "POST";

			// Token: 0x04001DE6 RID: 7654
			public const string MkCol = "MKCOL";
		}

		// Token: 0x020003BC RID: 956
		public static class File
		{
			// Token: 0x04001DE7 RID: 7655
			public const string DownloadFile = "GET";

			// Token: 0x04001DE8 RID: 7656
			public const string UploadFile = "PUT";
		}
	}
}

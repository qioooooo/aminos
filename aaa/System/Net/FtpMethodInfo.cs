using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020003BF RID: 959
	internal class FtpMethodInfo
	{
		// Token: 0x06001DF0 RID: 7664 RVA: 0x00071757 File Offset: 0x00070757
		internal FtpMethodInfo(string method, FtpOperation operation, FtpMethodFlags flags, string httpCommand)
		{
			this.Method = method;
			this.Operation = operation;
			this.Flags = flags;
			this.HttpCommand = httpCommand;
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0007177C File Offset: 0x0007077C
		internal bool HasFlag(FtpMethodFlags flags)
		{
			return (this.Flags & flags) != FtpMethodFlags.None;
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0007178C File Offset: 0x0007078C
		internal bool IsCommandOnly
		{
			get
			{
				return (this.Flags & (FtpMethodFlags.IsDownload | FtpMethodFlags.IsUpload)) == FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x00071799 File Offset: 0x00070799
		internal bool IsUpload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsUpload) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x000717A9 File Offset: 0x000707A9
		internal bool IsDownload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsDownload) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x000717B9 File Offset: 0x000707B9
		internal bool HasHttpCommand
		{
			get
			{
				return (this.Flags & FtpMethodFlags.HasHttpCommand) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x000717CD File Offset: 0x000707CD
		internal bool ShouldParseForResponseUri
		{
			get
			{
				return (this.Flags & FtpMethodFlags.ShouldParseForResponseUri) != FtpMethodFlags.None;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x000717DE File Offset: 0x000707DE
		internal bool IsUnknownMethod
		{
			get
			{
				return this.Operation == FtpOperation.Other;
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000717EC File Offset: 0x000707EC
		internal static FtpMethodInfo GetMethodInfo(string method)
		{
			method = method.ToUpper(CultureInfo.InvariantCulture);
			foreach (FtpMethodInfo ftpMethodInfo in FtpMethodInfo.KnownMethodInfo)
			{
				if (method == ftpMethodInfo.Method)
				{
					return ftpMethodInfo;
				}
			}
			throw new ArgumentException(SR.GetString("net_ftp_unsupported_method"), "method");
		}

		// Token: 0x04001E02 RID: 7682
		internal string Method;

		// Token: 0x04001E03 RID: 7683
		internal FtpOperation Operation;

		// Token: 0x04001E04 RID: 7684
		internal FtpMethodFlags Flags;

		// Token: 0x04001E05 RID: 7685
		internal string HttpCommand;

		// Token: 0x04001E06 RID: 7686
		private static readonly FtpMethodInfo[] KnownMethodInfo = new FtpMethodInfo[]
		{
			new FtpMethodInfo("RETR", FtpOperation.DownloadFile, FtpMethodFlags.IsDownload | FtpMethodFlags.TakesParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("NLST", FtpOperation.ListDirectory, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("LIST", FtpOperation.ListDirectoryDetails, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("STOR", FtpOperation.UploadFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("STOU", FtpOperation.UploadFileUnique, FtpMethodFlags.IsUpload | FtpMethodFlags.DoesNotTakeParameter | FtpMethodFlags.ShouldParseForResponseUri, null),
			new FtpMethodInfo("APPE", FtpOperation.AppendFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("DELE", FtpOperation.DeleteFile, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MDTM", FtpOperation.GetDateTimestamp, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("SIZE", FtpOperation.GetFileSize, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("RENAME", FtpOperation.Rename, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MKD", FtpOperation.MakeDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("RMD", FtpOperation.RemoveDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("PWD", FtpOperation.PrintWorkingDirectory, FtpMethodFlags.DoesNotTakeParameter, null)
		};
	}
}

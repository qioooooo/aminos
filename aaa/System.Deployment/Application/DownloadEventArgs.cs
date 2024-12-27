using System;

namespace System.Deployment.Application
{
	// Token: 0x02000055 RID: 85
	internal class DownloadEventArgs : EventArgs
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000FA84 File Offset: 0x0000EA84
		public int Progress
		{
			get
			{
				return this._progress;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000FA8C File Offset: 0x0000EA8C
		public long BytesCompleted
		{
			get
			{
				return this._bytesCompleted;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000FA94 File Offset: 0x0000EA94
		public long BytesTotal
		{
			get
			{
				return this._bytesTotal;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000FA9C File Offset: 0x0000EA9C
		public Uri FileSourceUri
		{
			get
			{
				return this._fileSourceUri;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000FAA4 File Offset: 0x0000EAA4
		public Uri FileResponseUri
		{
			get
			{
				return this._fileResponseUri;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000FAAC File Offset: 0x0000EAAC
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000FAB4 File Offset: 0x0000EAB4
		internal string FileLocalPath
		{
			get
			{
				return this._fileLocalPath;
			}
			set
			{
				this._fileLocalPath = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000FABD File Offset: 0x0000EABD
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000FAC5 File Offset: 0x0000EAC5
		internal object Cookie
		{
			get
			{
				return this._cookie;
			}
			set
			{
				this._cookie = value;
			}
		}

		// Token: 0x04000207 RID: 519
		internal int _progress;

		// Token: 0x04000208 RID: 520
		internal int _filesCompleted;

		// Token: 0x04000209 RID: 521
		internal int _filesTotal;

		// Token: 0x0400020A RID: 522
		internal long _bytesCompleted;

		// Token: 0x0400020B RID: 523
		internal long _bytesTotal;

		// Token: 0x0400020C RID: 524
		internal Uri _fileSourceUri;

		// Token: 0x0400020D RID: 525
		internal Uri _fileResponseUri;

		// Token: 0x0400020E RID: 526
		internal string _fileLocalPath;

		// Token: 0x0400020F RID: 527
		internal object _cookie;
	}
}

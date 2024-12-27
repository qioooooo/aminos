using System;
using System.IO;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000CE RID: 206
	internal class TempFile : DisposableBase
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x0001C848 File Offset: 0x0001B848
		public TempFile()
			: this(global::System.IO.Path.GetTempPath())
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001C855 File Offset: 0x0001B855
		public TempFile(string basePath)
			: this(basePath, string.Empty)
		{
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001C864 File Offset: 0x0001B864
		public TempFile(string basePath, string suffix)
		{
			do
			{
				this._thePath = global::System.IO.Path.Combine(basePath, PathHelper.GenerateRandomPath(2U) + suffix);
			}
			while (File.Exists(this._thePath) || Directory.Exists(this._thePath));
			string directoryName = global::System.IO.Path.GetDirectoryName(this._thePath);
			Directory.CreateDirectory(directoryName);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001C8BC File Offset: 0x0001B8BC
		protected override void DisposeUnmanagedResources()
		{
			string rootSegmentPath = PathHelper.GetRootSegmentPath(this._thePath, 2U);
			if (Directory.Exists(rootSegmentPath))
			{
				try
				{
					Directory.Delete(rootSegmentPath, true);
				}
				catch (IOException)
				{
					Thread.Sleep(10);
					try
					{
						Directory.Delete(rootSegmentPath, true);
					}
					catch (IOException)
					{
					}
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001C91C File Offset: 0x0001B91C
		public string Path
		{
			get
			{
				return this._thePath;
			}
		}

		// Token: 0x04000478 RID: 1144
		private const uint _filePathSegmentCount = 2U;

		// Token: 0x04000479 RID: 1145
		private string _thePath;
	}
}

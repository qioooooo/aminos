using System;
using System.IO;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000CD RID: 205
	internal class TempDirectory : DisposableBase
	{
		// Token: 0x06000560 RID: 1376 RVA: 0x0001C790 File Offset: 0x0001B790
		public TempDirectory()
			: this(global::System.IO.Path.GetTempPath())
		{
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001C79D File Offset: 0x0001B79D
		public TempDirectory(string basePath)
		{
			do
			{
				this._thePath = global::System.IO.Path.Combine(basePath, PathHelper.GenerateRandomPath(2U));
			}
			while (Directory.Exists(this._thePath) || File.Exists(this._thePath));
			Directory.CreateDirectory(this._thePath);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001C7E0 File Offset: 0x0001B7E0
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001C840 File Offset: 0x0001B840
		public string Path
		{
			get
			{
				return this._thePath;
			}
		}

		// Token: 0x04000476 RID: 1142
		private const uint _directorySegmentCount = 2U;

		// Token: 0x04000477 RID: 1143
		private string _thePath;
	}
}

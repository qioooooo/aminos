using System;

namespace System.Web.Util
{
	// Token: 0x02000765 RID: 1893
	internal abstract class FileData
	{
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x00171414 File Offset: 0x00170414
		internal string Name
		{
			get
			{
				return this._wfd.cFileName;
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x00171421 File Offset: 0x00170421
		internal string FullName
		{
			get
			{
				return this._path + "\\" + this._wfd.cFileName;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x0017143E File Offset: 0x0017043E
		internal bool IsDirectory
		{
			get
			{
				return (this._wfd.dwFileAttributes & 16U) != 0U;
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x00171454 File Offset: 0x00170454
		internal bool IsHidden
		{
			get
			{
				return (this._wfd.dwFileAttributes & 2U) != 0U;
			}
		}

		// Token: 0x06005BFC RID: 23548 RVA: 0x00171469 File Offset: 0x00170469
		internal FindFileData GetFindFileData()
		{
			return new FindFileData(ref this._wfd);
		}

		// Token: 0x04003138 RID: 12600
		protected string _path;

		// Token: 0x04003139 RID: 12601
		protected UnsafeNativeMethods.WIN32_FIND_DATA _wfd;
	}
}

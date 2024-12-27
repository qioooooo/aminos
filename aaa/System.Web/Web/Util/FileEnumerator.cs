using System;
using System.Collections;
using System.IO;

namespace System.Web.Util
{
	// Token: 0x02000766 RID: 1894
	internal class FileEnumerator : FileData, IEnumerable, IEnumerator, IDisposable
	{
		// Token: 0x06005BFE RID: 23550 RVA: 0x0017147E File Offset: 0x0017047E
		internal static FileEnumerator Create(string path)
		{
			return new FileEnumerator(path);
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x00171486 File Offset: 0x00170486
		private FileEnumerator(string path)
		{
			this._path = Path.GetFullPath(path);
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x001714A8 File Offset: 0x001704A8
		~FileEnumerator()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x001714D4 File Offset: 0x001704D4
		private bool SkipCurrent()
		{
			return this._wfd.cFileName == "." || this._wfd.cFileName == "..";
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x00171507 File Offset: 0x00170507
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x0017150C File Offset: 0x0017050C
		bool IEnumerator.MoveNext()
		{
			for (;;)
			{
				if (this._hFindFile == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
				{
					this._hFindFile = UnsafeNativeMethods.FindFirstFile(this._path + "\\*.*", out this._wfd);
					if (this._hFindFile == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
					{
						break;
					}
				}
				else if (!UnsafeNativeMethods.FindNextFile(this._hFindFile, out this._wfd))
				{
					return false;
				}
				if (!this.SkipCurrent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x06005C04 RID: 23556 RVA: 0x00171580 File Offset: 0x00170580
		object IEnumerator.Current
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x00171583 File Offset: 0x00170583
		void IEnumerator.Reset()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x0017158A File Offset: 0x0017058A
		void IDisposable.Dispose()
		{
			if (this._hFindFile != UnsafeNativeMethods.INVALID_HANDLE_VALUE)
			{
				UnsafeNativeMethods.FindClose(this._hFindFile);
				this._hFindFile = UnsafeNativeMethods.INVALID_HANDLE_VALUE;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400313A RID: 12602
		private IntPtr _hFindFile = UnsafeNativeMethods.INVALID_HANDLE_VALUE;
	}
}

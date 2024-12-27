using System;
using System.Web.Caching;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000332 RID: 818
	internal class FileSecurityDescriptorWrapper : IDisposable
	{
		// Token: 0x06002808 RID: 10248 RVA: 0x000AFBAC File Offset: 0x000AEBAC
		~FileSecurityDescriptorWrapper()
		{
			this.FreeSecurityDescriptor();
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000AFBD8 File Offset: 0x000AEBD8
		internal FileSecurityDescriptorWrapper(string strFile)
		{
			if (strFile.EndsWith("\\", StringComparison.Ordinal))
			{
				this._FileName = strFile.Substring(0, strFile.Length - 1);
			}
			else
			{
				this._FileName = strFile;
			}
			this._securityDescriptor = UnsafeNativeMethods.GetFileSecurityDescriptor(this._FileName);
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000AFC34 File Offset: 0x000AEC34
		internal bool IsAccessAllowed(IntPtr iToken, int iAccess)
		{
			if (iToken == IntPtr.Zero)
			{
				return true;
			}
			if (this._SecurityDescriptorBeingFreed)
			{
				return this.IsAccessAllowedUsingNewSecurityDescriptor(iToken, iAccess);
			}
			this._Lock.AcquireReaderLock();
			try
			{
				try
				{
					if (!this._SecurityDescriptorBeingFreed)
					{
						if (this._securityDescriptor == IntPtr.Zero)
						{
							return true;
						}
						if (this._securityDescriptor == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
						{
							return false;
						}
						return UnsafeNativeMethods.IsAccessToFileAllowed(this._securityDescriptor, iToken, iAccess) != 0;
					}
				}
				finally
				{
					this._Lock.ReleaseReaderLock();
				}
			}
			catch
			{
				throw;
			}
			return this.IsAccessAllowedUsingNewSecurityDescriptor(iToken, iAccess);
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000AFCF0 File Offset: 0x000AECF0
		private bool IsAccessAllowedUsingNewSecurityDescriptor(IntPtr iToken, int iAccess)
		{
			if (iToken == IntPtr.Zero)
			{
				return true;
			}
			IntPtr fileSecurityDescriptor = UnsafeNativeMethods.GetFileSecurityDescriptor(this._FileName);
			if (fileSecurityDescriptor == IntPtr.Zero)
			{
				return true;
			}
			if (fileSecurityDescriptor == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
			{
				return false;
			}
			bool flag;
			try
			{
				try
				{
					flag = UnsafeNativeMethods.IsAccessToFileAllowed(fileSecurityDescriptor, iToken, iAccess) != 0;
				}
				finally
				{
					UnsafeNativeMethods.FreeFileSecurityDescriptor(fileSecurityDescriptor);
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000AFD70 File Offset: 0x000AED70
		internal void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			this.FreeSecurityDescriptor();
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000AFD78 File Offset: 0x000AED78
		internal void FreeSecurityDescriptor()
		{
			if (!this.IsSecurityDescriptorValid())
			{
				return;
			}
			this._SecurityDescriptorBeingFreed = true;
			this._Lock.AcquireWriterLock();
			try
			{
				try
				{
					if (this.IsSecurityDescriptorValid())
					{
						IntPtr securityDescriptor = this._securityDescriptor;
						this._securityDescriptor = UnsafeNativeMethods.INVALID_HANDLE_VALUE;
						UnsafeNativeMethods.FreeFileSecurityDescriptor(securityDescriptor);
					}
				}
				finally
				{
					this._Lock.ReleaseWriterLock();
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000AFDF4 File Offset: 0x000AEDF4
		internal bool IsSecurityDescriptorValid()
		{
			return this._securityDescriptor != UnsafeNativeMethods.INVALID_HANDLE_VALUE && this._securityDescriptor != IntPtr.Zero;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000AFE1A File Offset: 0x000AEE1A
		void IDisposable.Dispose()
		{
			this.FreeSecurityDescriptor();
			GC.SuppressFinalize(this);
		}

		// Token: 0x04001E84 RID: 7812
		private IntPtr _securityDescriptor;

		// Token: 0x04001E85 RID: 7813
		internal bool _AnonymousAccessChecked;

		// Token: 0x04001E86 RID: 7814
		internal bool _AnonymousAccess;

		// Token: 0x04001E87 RID: 7815
		private bool _SecurityDescriptorBeingFreed;

		// Token: 0x04001E88 RID: 7816
		private string _FileName;

		// Token: 0x04001E89 RID: 7817
		private ReadWriteSpinLock _Lock = default(ReadWriteSpinLock);
	}
}

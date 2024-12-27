using System;
using System.Runtime.InteropServices;

namespace System.Web
{
	// Token: 0x020000A5 RID: 165
	internal class ImpersonationContext : IDisposable
	{
		// Token: 0x06000855 RID: 2133 RVA: 0x00024E7C File Offset: 0x00023E7C
		internal ImpersonationContext()
		{
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00024E84 File Offset: 0x00023E84
		internal ImpersonationContext(IntPtr token)
		{
			this.ImpersonateToken(new HandleRef(this, token));
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00024E9C File Offset: 0x00023E9C
		~ImpersonationContext()
		{
			this.Dispose(false);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00024ECC File Offset: 0x00023ECC
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00024ED4 File Offset: 0x00023ED4
		private void Dispose(bool disposing)
		{
			if (this._savedToken.Handle != IntPtr.Zero)
			{
				try
				{
				}
				finally
				{
					UnsafeNativeMethods.CloseHandle(this._savedToken.Handle);
					this._savedToken = new HandleRef(this, IntPtr.Zero);
				}
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00024F30 File Offset: 0x00023F30
		protected void ImpersonateToken(HandleRef token)
		{
			try
			{
				this._savedToken = new HandleRef(this, ImpersonationContext.GetCurrentToken());
				if (this._savedToken.Handle != IntPtr.Zero && UnsafeNativeMethods.RevertToSelf() != 0)
				{
					this._reverted = true;
				}
				if (token.Handle != IntPtr.Zero)
				{
					if (UnsafeNativeMethods.SetThreadToken(IntPtr.Zero, token.Handle) == 0)
					{
						throw new HttpException(SR.GetString("Cannot_impersonate"));
					}
					this._impersonating = true;
				}
			}
			catch
			{
				this.RestoreImpersonation();
				throw;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00024FCC File Offset: 0x00023FCC
		private void RestoreImpersonation()
		{
			if (this._impersonating)
			{
				UnsafeNativeMethods.RevertToSelf();
				this._impersonating = false;
			}
			if (this._savedToken.Handle != IntPtr.Zero)
			{
				if (this._reverted && UnsafeNativeMethods.SetThreadToken(IntPtr.Zero, this._savedToken.Handle) == 0)
				{
					throw new HttpException(SR.GetString("Cannot_impersonate"));
				}
				this._reverted = false;
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002503B File Offset: 0x0002403B
		internal void Undo()
		{
			this.RestoreImpersonation();
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00025050 File Offset: 0x00024050
		private static IntPtr GetCurrentToken()
		{
			IntPtr zero = IntPtr.Zero;
			if (UnsafeNativeMethods.OpenThreadToken(UnsafeNativeMethods.GetCurrentThread(), 131084, true, ref zero) == 0 && Marshal.GetLastWin32Error() != 1008)
			{
				throw new HttpException(SR.GetString("Cannot_impersonate"));
			}
			return zero;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00025094 File Offset: 0x00024094
		internal static bool CurrentThreadTokenExists
		{
			get
			{
				bool flag = false;
				try
				{
				}
				finally
				{
					IntPtr currentToken = ImpersonationContext.GetCurrentToken();
					if (currentToken != IntPtr.Zero)
					{
						flag = true;
						UnsafeNativeMethods.CloseHandle(currentToken);
					}
				}
				return flag;
			}
		}

		// Token: 0x0400119C RID: 4508
		private HandleRef _savedToken;

		// Token: 0x0400119D RID: 4509
		private bool _reverted;

		// Token: 0x0400119E RID: 4510
		private bool _impersonating;
	}
}

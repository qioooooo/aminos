using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Text
{
	// Token: 0x020000DC RID: 220
	public sealed class PrivateFontCollection : FontCollection
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x00026460 File Offset: 0x00025460
		public PrivateFontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipNewPrivateFontCollection(out this.nativeFontCollection);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00026494 File Offset: 0x00025494
		protected override void Dispose(bool disposing)
		{
			if (this.nativeFontCollection != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePrivateFontCollection(out this.nativeFontCollection);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeFontCollection = IntPtr.Zero;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00026500 File Offset: 0x00025500
		public void AddFontFile(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			int num = SafeNativeMethods.Gdip.GdipPrivateAddFontFile(new HandleRef(this, this.nativeFontCollection), filename);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			SafeNativeMethods.AddFontFile(filename);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00026538 File Offset: 0x00025538
		public void AddMemoryFont(IntPtr memory, int length)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			int num = SafeNativeMethods.Gdip.GdipPrivateAddMemoryFont(new HandleRef(this, this.nativeFontCollection), new HandleRef(null, memory), length);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}
	}
}

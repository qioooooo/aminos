using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x020008C9 RID: 2249
	internal sealed class WindowsGraphicsWrapper : IDisposable
	{
		// Token: 0x06006D3C RID: 27964 RVA: 0x001911AC File Offset: 0x001901AC
		public WindowsGraphicsWrapper(IDeviceContext idc, TextFormatFlags flags)
		{
			if (idc is Graphics)
			{
				ApplyGraphicsProperties applyGraphicsProperties = ApplyGraphicsProperties.None;
				if ((flags & TextFormatFlags.PreserveGraphicsClipping) != TextFormatFlags.Default)
				{
					applyGraphicsProperties |= ApplyGraphicsProperties.Clipping;
				}
				if ((flags & TextFormatFlags.PreserveGraphicsTranslateTransform) != TextFormatFlags.Default)
				{
					applyGraphicsProperties |= ApplyGraphicsProperties.TranslateTransform;
				}
				if (applyGraphicsProperties != ApplyGraphicsProperties.None)
				{
					this.wg = WindowsGraphics.FromGraphics(idc as Graphics, applyGraphicsProperties);
				}
			}
			else
			{
				this.wg = idc as WindowsGraphics;
				if (this.wg != null)
				{
					this.idc = idc;
				}
			}
			if (this.wg == null)
			{
				this.idc = idc;
				this.wg = WindowsGraphics.FromHdc(idc.GetHdc());
			}
			if ((flags & TextFormatFlags.LeftAndRightPadding) != TextFormatFlags.Default)
			{
				this.wg.TextPadding = TextPaddingOptions.LeftAndRightPadding;
				return;
			}
			if ((flags & TextFormatFlags.NoPadding) != TextFormatFlags.Default)
			{
				this.wg.TextPadding = TextPaddingOptions.NoPadding;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06006D3D RID: 27965 RVA: 0x00191260 File Offset: 0x00190260
		public WindowsGraphics WindowsGraphics
		{
			get
			{
				return this.wg;
			}
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x00191268 File Offset: 0x00190268
		~WindowsGraphicsWrapper()
		{
			this.Dispose(false);
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x00191298 File Offset: 0x00190298
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x001912A8 File Offset: 0x001902A8
		public void Dispose(bool disposing)
		{
			if (this.wg != null)
			{
				if (this.wg != this.idc)
				{
					this.wg.Dispose();
					if (this.idc != null)
					{
						this.idc.ReleaseHdc();
					}
				}
				this.idc = null;
				this.wg = null;
			}
		}

		// Token: 0x0400424C RID: 16972
		private IDeviceContext idc;

		// Token: 0x0400424D RID: 16973
		private WindowsGraphics wg;
	}
}

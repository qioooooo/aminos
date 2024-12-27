using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000034 RID: 52
	public abstract class Brush : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x0600019D RID: 413
		public abstract object Clone();

		// Token: 0x0600019E RID: 414 RVA: 0x000069EB File Offset: 0x000059EB
		protected internal void SetNativeBrush(IntPtr brush)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.SetNativeBrushInternal(brush);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000069FE File Offset: 0x000059FE
		internal void SetNativeBrushInternal(IntPtr brush)
		{
			this.nativeBrush = brush;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006A07 File Offset: 0x00005A07
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		internal IntPtr NativeBrush
		{
			get
			{
				return this.nativeBrush;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006A0F File Offset: 0x00005A0F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006A20 File Offset: 0x00005A20
		protected virtual void Dispose(bool disposing)
		{
			if (this.nativeBrush != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteBrush(new HandleRef(this, this.nativeBrush));
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
					this.nativeBrush = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006A8C File Offset: 0x00005A8C
		~Brush()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C5 RID: 453
		private IntPtr nativeBrush;
	}
}

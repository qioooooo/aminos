using System;
using System.Drawing;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000026 RID: 38
	internal abstract class WindowsBrush : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060000DA RID: 218
		public abstract object Clone();

		// Token: 0x060000DB RID: 219
		protected abstract void CreateBrush();

		// Token: 0x060000DC RID: 220 RVA: 0x00003BB6 File Offset: 0x00002BB6
		public WindowsBrush(DeviceContext dc)
		{
			this.dc = dc;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003BD0 File Offset: 0x00002BD0
		public WindowsBrush(DeviceContext dc, Color color)
		{
			this.dc = dc;
			this.color = color;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003BF4 File Offset: 0x00002BF4
		~WindowsBrush()
		{
			this.Dispose(false);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003C24 File Offset: 0x00002C24
		protected DeviceContext DC
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003C2C File Offset: 0x00002C2C
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003C38 File Offset: 0x00002C38
		protected virtual void Dispose(bool disposing)
		{
			if (this.dc != null && this.nativeHandle != IntPtr.Zero)
			{
				this.dc.DeleteObject(this.nativeHandle, GdiObjectType.Brush);
				this.nativeHandle = IntPtr.Zero;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003C85 File Offset: 0x00002C85
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003C8D File Offset: 0x00002C8D
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003CAD File Offset: 0x00002CAD
		protected IntPtr NativeHandle
		{
			get
			{
				if (this.nativeHandle == IntPtr.Zero)
				{
					this.CreateBrush();
				}
				return this.nativeHandle;
			}
			set
			{
				this.nativeHandle = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003CB6 File Offset: 0x00002CB6
		public IntPtr HBrush
		{
			get
			{
				return this.NativeHandle;
			}
		}

		// Token: 0x04000AB9 RID: 2745
		private DeviceContext dc;

		// Token: 0x04000ABA RID: 2746
		private IntPtr nativeHandle;

		// Token: 0x04000ABB RID: 2747
		private Color color = Color.White;
	}
}

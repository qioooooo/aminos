using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000CD RID: 205
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MetafileHeader
	{
		// Token: 0x06000C57 RID: 3159 RVA: 0x00025307 File Offset: 0x00024307
		internal MetafileHeader()
		{
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0002530F File Offset: 0x0002430F
		public MetafileType Type
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.type;
				}
				return this.wmf.type;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00025330 File Offset: 0x00024330
		public int MetafileSize
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.size;
				}
				return this.wmf.size;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00025351 File Offset: 0x00024351
		public int Version
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.version;
				}
				return this.wmf.version;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00025372 File Offset: 0x00024372
		public float DpiX
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.dpiX;
				}
				return this.wmf.dpiX;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00025393 File Offset: 0x00024393
		public float DpiY
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.dpiY;
				}
				return this.wmf.dpiY;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x000253B4 File Offset: 0x000243B4
		public Rectangle Bounds
		{
			get
			{
				if (!this.IsWmf())
				{
					return new Rectangle(this.emf.X, this.emf.Y, this.emf.Width, this.emf.Height);
				}
				return new Rectangle(this.wmf.X, this.wmf.Y, this.wmf.Width, this.wmf.Height);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002542C File Offset: 0x0002442C
		public bool IsWmf()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.wmf != null && (this.wmf.type == MetafileType.Wmf || this.wmf.type == MetafileType.WmfPlaceable);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002546C File Offset: 0x0002446C
		public bool IsWmfPlaceable()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.wmf != null && this.wmf.type == MetafileType.WmfPlaceable;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0002549D File Offset: 0x0002449D
		public bool IsEmf()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.Emf;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000254CE File Offset: 0x000244CE
		public bool IsEmfOrEmfPlus()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type >= MetafileType.Emf;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00025502 File Offset: 0x00024502
		public bool IsEmfPlus()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type >= MetafileType.EmfPlusOnly;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00025536 File Offset: 0x00024536
		public bool IsEmfPlusDual()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.EmfPlusDual;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00025567 File Offset: 0x00024567
		public bool IsEmfPlusOnly()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.EmfPlusOnly;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00025598 File Offset: 0x00024598
		public bool IsDisplay()
		{
			return this.IsEmfPlus() && (this.emf.emfPlusFlags & EmfPlusFlags.Display) != (EmfPlusFlags)0;
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x000255B7 File Offset: 0x000245B7
		public MetaHeader WmfHeader
		{
			get
			{
				if (this.wmf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				return this.wmf.WmfHeader;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x000255D3 File Offset: 0x000245D3
		public int EmfPlusHeaderSize
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.EmfPlusHeaderSize;
				}
				return this.wmf.EmfPlusHeaderSize;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0002560B File Offset: 0x0002460B
		public int LogicalDpiX
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.LogicalDpiX;
				}
				return this.wmf.LogicalDpiX;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00025643 File Offset: 0x00024643
		public int LogicalDpiY
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.LogicalDpiX;
				}
				return this.wmf.LogicalDpiY;
			}
		}

		// Token: 0x04000A92 RID: 2706
		internal MetafileHeaderWmf wmf;

		// Token: 0x04000A93 RID: 2707
		internal MetafileHeaderEmf emf;
	}
}

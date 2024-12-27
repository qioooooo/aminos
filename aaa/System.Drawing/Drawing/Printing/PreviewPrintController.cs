using System;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Printing
{
	// Token: 0x02000114 RID: 276
	public class PreviewPrintController : PrintController
	{
		// Token: 0x06000E83 RID: 3715 RVA: 0x0002B14E File Offset: 0x0002A14E
		private void CheckSecurity()
		{
			IntSecurity.SafePrinting.Demand();
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0002B15A File Offset: 0x0002A15A
		public override bool IsPreview
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0002B160 File Offset: 0x0002A160
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity();
			base.OnStartPrint(document, e);
			try
			{
				if (!document.PrinterSettings.IsValid)
				{
					throw new InvalidPrinterException(document.PrinterSettings);
				}
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				this.dc = document.PrinterSettings.CreateInformationContext(this.modeHandle);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0002B1D4 File Offset: 0x0002A1D4
		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity();
			base.OnStartPage(document, e);
			try
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				e.PageSettings.CopyToHdevmode(this.modeHandle);
				Size size = e.PageBounds.Size;
				Size size2 = PrinterUnitConvert.Convert(size, PrinterUnit.Display, PrinterUnit.HundredthsOfAMillimeter);
				Metafile metafile = new Metafile(this.dc.Hdc, new Rectangle(0, 0, size2.Width, size2.Height), MetafileFrameUnit.GdiCompatible, EmfType.EmfPlusOnly);
				PreviewPageInfo previewPageInfo = new PreviewPageInfo(metafile, size);
				this.list.Add(previewPageInfo);
				PrintPreviewGraphics printPreviewGraphics = new PrintPreviewGraphics(document, e);
				this.graphics = Graphics.FromImage(metafile);
				if (this.graphics != null && document.OriginAtMargins)
				{
					int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 88);
					int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 90);
					int deviceCaps3 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 112);
					int deviceCaps4 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 113);
					float num = (float)(deviceCaps3 * 100 / deviceCaps);
					float num2 = (float)(deviceCaps4 * 100 / deviceCaps2);
					this.graphics.TranslateTransform(-num, -num2);
					this.graphics.TranslateTransform((float)document.DefaultPageSettings.Margins.Left, (float)document.DefaultPageSettings.Margins.Top);
				}
				this.graphics.PrintingHelper = printPreviewGraphics;
				if (this.antiAlias)
				{
					this.graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
					this.graphics.SmoothingMode = SmoothingMode.AntiAlias;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return this.graphics;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0002B3B8 File Offset: 0x0002A3B8
		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity();
			this.graphics.Dispose();
			this.graphics = null;
			base.OnEndPage(document, e);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0002B3DA File Offset: 0x0002A3DA
		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity();
			this.dc.Dispose();
			this.dc = null;
			base.OnEndPrint(document, e);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0002B3FC File Offset: 0x0002A3FC
		public PreviewPageInfo[] GetPreviewPageInfo()
		{
			this.CheckSecurity();
			PreviewPageInfo[] array = new PreviewPageInfo[this.list.Count];
			this.list.CopyTo(array, 0);
			return array;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0002B42E File Offset: 0x0002A42E
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x0002B436 File Offset: 0x0002A436
		public virtual bool UseAntiAlias
		{
			get
			{
				return this.antiAlias;
			}
			set
			{
				this.antiAlias = value;
			}
		}

		// Token: 0x04000C2A RID: 3114
		private IList list = new ArrayList();

		// Token: 0x04000C2B RID: 3115
		private Graphics graphics;

		// Token: 0x04000C2C RID: 3116
		private DeviceContext dc;

		// Token: 0x04000C2D RID: 3117
		private bool antiAlias;
	}
}

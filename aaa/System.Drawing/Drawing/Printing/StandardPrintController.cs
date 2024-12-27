using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Printing
{
	// Token: 0x02000108 RID: 264
	public class StandardPrintController : PrintController
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x00029775 File Offset: 0x00028775
		private void CheckSecurity(PrintDocument document)
		{
			if (document.PrinterSettings.PrintDialogDisplayed)
			{
				IntSecurity.SafePrinting.Demand();
				return;
			}
			if (document.PrinterSettings.IsDefaultPrinter)
			{
				IntSecurity.DefaultPrinting.Demand();
				return;
			}
			IntSecurity.AllPrinting.Demand();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000297B4 File Offset: 0x000287B4
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity(document);
			base.OnStartPrint(document, e);
			if (!document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(document.PrinterSettings);
			}
			this.dc = document.PrinterSettings.CreateDeviceContext(this.modeHandle);
			SafeNativeMethods.DOCINFO docinfo = new SafeNativeMethods.DOCINFO();
			docinfo.lpszDocName = document.DocumentName;
			if (document.PrinterSettings.PrintToFile)
			{
				docinfo.lpszOutput = document.PrinterSettings.OutputPort;
			}
			else
			{
				docinfo.lpszOutput = null;
			}
			docinfo.lpszDatatype = null;
			docinfo.fwType = 0;
			int num = SafeNativeMethods.StartDoc(new HandleRef(this.dc, this.dc.Hdc), docinfo);
			if (num > 0)
			{
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 1223)
			{
				e.Cancel = true;
				return;
			}
			throw new Win32Exception(lastWin32Error);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0002988C File Offset: 0x0002888C
		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity(document);
			base.OnStartPage(document, e);
			try
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				e.PageSettings.CopyToHdevmode(this.modeHandle);
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(this, this.modeHandle));
				try
				{
					SafeNativeMethods.ResetDC(new HandleRef(this.dc, this.dc.Hdc), new HandleRef(null, intPtr));
				}
				finally
				{
					SafeNativeMethods.GlobalUnlock(new HandleRef(this, this.modeHandle));
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.graphics = Graphics.FromHdcInternal(this.dc.Hdc);
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
			int num3 = SafeNativeMethods.StartPage(new HandleRef(this.dc, this.dc.Hdc));
			if (num3 <= 0)
			{
				throw new Win32Exception();
			}
			return this.graphics;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00029A68 File Offset: 0x00028A68
		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity(document);
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				int num = SafeNativeMethods.EndPage(new HandleRef(this.dc, this.dc.Hdc));
				if (num <= 0)
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				this.graphics.Dispose();
				this.graphics = null;
			}
			base.OnEndPage(document, e);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00029AE0 File Offset: 0x00028AE0
		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity(document);
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				if (this.dc != null)
				{
					try
					{
						int num = (e.Cancel ? SafeNativeMethods.AbortDoc(new HandleRef(this.dc, this.dc.Hdc)) : SafeNativeMethods.EndDoc(new HandleRef(this.dc, this.dc.Hdc)));
						if (num <= 0)
						{
							throw new Win32Exception();
						}
					}
					finally
					{
						this.dc.Dispose();
						this.dc = null;
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.OnEndPrint(document, e);
		}

		// Token: 0x04000B7C RID: 2940
		private DeviceContext dc;

		// Token: 0x04000B7D RID: 2941
		private Graphics graphics;
	}
}

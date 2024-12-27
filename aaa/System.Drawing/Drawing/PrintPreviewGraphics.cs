using System;
using System.Drawing.Internal;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000128 RID: 296
	internal class PrintPreviewGraphics
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x0002DCAC File Offset: 0x0002CCAC
		public PrintPreviewGraphics(PrintDocument document, PrintPageEventArgs e)
		{
			this.printPageEventArgs = e;
			this.printDocument = document;
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002DCC4 File Offset: 0x0002CCC4
		public RectangleF VisibleClipBounds
		{
			get
			{
				IntPtr hdevmodeInternal = this.printPageEventArgs.PageSettings.PrinterSettings.GetHdevmodeInternal();
				RectangleF visibleClipBounds;
				using (DeviceContext deviceContext = this.printPageEventArgs.PageSettings.PrinterSettings.CreateDeviceContext(hdevmodeInternal))
				{
					using (Graphics graphics = Graphics.FromHdcInternal(deviceContext.Hdc))
					{
						if (this.printDocument.OriginAtMargins)
						{
							int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 88);
							int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 90);
							int deviceCaps3 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 112);
							int deviceCaps4 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 113);
							float num = (float)(deviceCaps3 * 100 / deviceCaps);
							float num2 = (float)(deviceCaps4 * 100 / deviceCaps2);
							graphics.TranslateTransform(-num, -num2);
							graphics.TranslateTransform((float)this.printDocument.DefaultPageSettings.Margins.Left, (float)this.printDocument.DefaultPageSettings.Margins.Top);
						}
						visibleClipBounds = graphics.VisibleClipBounds;
					}
				}
				return visibleClipBounds;
			}
		}

		// Token: 0x04000C74 RID: 3188
		private PrintPageEventArgs printPageEventArgs;

		// Token: 0x04000C75 RID: 3189
		private PrintDocument printDocument;
	}
}

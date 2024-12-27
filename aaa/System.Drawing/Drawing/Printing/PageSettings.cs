using System;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Printing
{
	// Token: 0x0200010E RID: 270
	[Serializable]
	public class PageSettings : ICloneable
	{
		// Token: 0x06000E4C RID: 3660 RVA: 0x0002A303 File Offset: 0x00029303
		public PageSettings()
			: this(new PrinterSettings())
		{
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002A310 File Offset: 0x00029310
		public PageSettings(PrinterSettings printerSettings)
		{
			this.printerSettings = printerSettings;
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0002A340 File Offset: 0x00029340
		public Rectangle Bounds
		{
			get
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				IntPtr hdevmode = this.printerSettings.GetHdevmode();
				Rectangle bounds = this.GetBounds(hdevmode);
				SafeNativeMethods.GlobalFree(new HandleRef(this, hdevmode));
				return bounds;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0002A379 File Offset: 0x00029379
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x0002A3A4 File Offset: 0x000293A4
		public bool Color
		{
			get
			{
				if (this.color.IsDefault)
				{
					return this.printerSettings.GetModeField(ModeField.Color, 1) == 2;
				}
				return (bool)this.color;
			}
			set
			{
				this.color = value;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0002A3B4 File Offset: 0x000293B4
		public float HardMarginX
		{
			get
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				float num = 0f;
				DeviceContext deviceContext = this.printerSettings.CreateDeviceContext(this);
				try
				{
					int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 88);
					int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 112);
					num = (float)(deviceCaps2 * 100 / deviceCaps);
				}
				finally
				{
					deviceContext.Dispose();
				}
				return num;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0002A428 File Offset: 0x00029428
		public float HardMarginY
		{
			get
			{
				float num = 0f;
				DeviceContext deviceContext = this.printerSettings.CreateDeviceContext(this);
				try
				{
					int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 90);
					int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), 113);
					num = (float)(deviceCaps2 * 100 / deviceCaps);
				}
				finally
				{
					deviceContext.Dispose();
				}
				return num;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0002A494 File Offset: 0x00029494
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x0002A4BF File Offset: 0x000294BF
		public bool Landscape
		{
			get
			{
				if (this.landscape.IsDefault)
				{
					return this.printerSettings.GetModeField(ModeField.Orientation, 1) == 2;
				}
				return (bool)this.landscape;
			}
			set
			{
				this.landscape = value;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0002A4CD File Offset: 0x000294CD
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x0002A4D5 File Offset: 0x000294D5
		public Margins Margins
		{
			get
			{
				return this.margins;
			}
			set
			{
				this.margins = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002A4DE File Offset: 0x000294DE
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0002A4F5 File Offset: 0x000294F5
		public PaperSize PaperSize
		{
			get
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				return this.GetPaperSize(IntPtr.Zero);
			}
			set
			{
				this.paperSize = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002A500 File Offset: 0x00029500
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0002A578 File Offset: 0x00029578
		public PaperSource PaperSource
		{
			get
			{
				if (this.paperSource == null)
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					IntPtr hdevmode = this.printerSettings.GetHdevmode();
					IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(this, hdevmode));
					SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
					PaperSource paperSource = this.PaperSourceFromMode(devmode);
					SafeNativeMethods.GlobalUnlock(new HandleRef(this, hdevmode));
					SafeNativeMethods.GlobalFree(new HandleRef(this, hdevmode));
					return paperSource;
				}
				return this.paperSource;
			}
			set
			{
				this.paperSource = value;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002A584 File Offset: 0x00029584
		public RectangleF PrintableArea
		{
			get
			{
				RectangleF rectangleF = default(RectangleF);
				DeviceContext deviceContext = this.printerSettings.CreateInformationContext(this);
				HandleRef handleRef = new HandleRef(deviceContext, deviceContext.Hdc);
				try
				{
					int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(handleRef, 88);
					int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(handleRef, 90);
					if (!this.Landscape)
					{
						rectangleF.X = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 112) * 100f / (float)deviceCaps;
						rectangleF.Y = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 113) * 100f / (float)deviceCaps2;
						rectangleF.Width = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 8) * 100f / (float)deviceCaps;
						rectangleF.Height = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 10) * 100f / (float)deviceCaps2;
					}
					else
					{
						rectangleF.Y = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 112) * 100f / (float)deviceCaps;
						rectangleF.X = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 113) * 100f / (float)deviceCaps2;
						rectangleF.Height = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 8) * 100f / (float)deviceCaps;
						rectangleF.Width = (float)UnsafeNativeMethods.GetDeviceCaps(handleRef, 10) * 100f / (float)deviceCaps2;
					}
				}
				finally
				{
					deviceContext.Dispose();
				}
				return rectangleF;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0002A6B8 File Offset: 0x000296B8
		// (set) Token: 0x06000E5D RID: 3677 RVA: 0x0002A730 File Offset: 0x00029730
		public PrinterResolution PrinterResolution
		{
			get
			{
				if (this.printerResolution == null)
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					IntPtr hdevmode = this.printerSettings.GetHdevmode();
					IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(this, hdevmode));
					SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
					PrinterResolution printerResolution = this.PrinterResolutionFromMode(devmode);
					SafeNativeMethods.GlobalUnlock(new HandleRef(this, hdevmode));
					SafeNativeMethods.GlobalFree(new HandleRef(this, hdevmode));
					return printerResolution;
				}
				return this.printerResolution;
			}
			set
			{
				this.printerResolution = value;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0002A739 File Offset: 0x00029739
		// (set) Token: 0x06000E5F RID: 3679 RVA: 0x0002A741 File Offset: 0x00029741
		public PrinterSettings PrinterSettings
		{
			get
			{
				return this.printerSettings;
			}
			set
			{
				if (value == null)
				{
					value = new PrinterSettings();
				}
				this.printerSettings = value;
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0002A754 File Offset: 0x00029754
		public object Clone()
		{
			PageSettings pageSettings = (PageSettings)base.MemberwiseClone();
			pageSettings.margins = (Margins)this.margins.Clone();
			return pageSettings;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0002A784 File Offset: 0x00029784
		public void CopyToHdevmode(IntPtr hdevmode)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevmode));
			SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
			if (this.color.IsNotDefault && (devmode.dmFields & 2048) == 2048)
			{
				devmode.dmColor = (((bool)this.color) ? 2 : 1);
			}
			if (this.landscape.IsNotDefault && (devmode.dmFields & 1) == 1)
			{
				devmode.dmOrientation = (((bool)this.landscape) ? 2 : 1);
			}
			if (this.paperSize != null)
			{
				if ((devmode.dmFields & 2) == 2)
				{
					devmode.dmPaperSize = (short)this.paperSize.RawKind;
				}
				bool flag = false;
				bool flag2 = false;
				if ((devmode.dmFields & 4) == 4)
				{
					devmode.dmPaperLength = (short)PrinterUnitConvert.Convert(this.paperSize.Height, PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
					flag2 = true;
				}
				if ((devmode.dmFields & 8) == 8)
				{
					devmode.dmPaperWidth = (short)PrinterUnitConvert.Convert(this.paperSize.Width, PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
					flag = true;
				}
				if (this.paperSize.Kind == PaperKind.Custom)
				{
					if (!flag2)
					{
						devmode.dmFields |= 4;
						devmode.dmPaperLength = (short)PrinterUnitConvert.Convert(this.paperSize.Height, PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
					}
					if (!flag)
					{
						devmode.dmFields |= 8;
						devmode.dmPaperWidth = (short)PrinterUnitConvert.Convert(this.paperSize.Width, PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
					}
				}
			}
			if (this.paperSource != null && (devmode.dmFields & 512) == 512)
			{
				devmode.dmDefaultSource = (short)this.paperSource.RawKind;
			}
			if (this.printerResolution != null)
			{
				if (this.printerResolution.Kind == PrinterResolutionKind.Custom)
				{
					if ((devmode.dmFields & 1024) == 1024)
					{
						devmode.dmPrintQuality = (short)this.printerResolution.X;
					}
					if ((devmode.dmFields & 8192) == 8192)
					{
						devmode.dmYResolution = (short)this.printerResolution.Y;
					}
				}
				else if ((devmode.dmFields & 1024) == 1024)
				{
					devmode.dmPrintQuality = (short)this.printerResolution.Kind;
				}
			}
			Marshal.StructureToPtr(devmode, intPtr, false);
			if (devmode.dmDriverExtra >= this.ExtraBytes)
			{
				int num = SafeNativeMethods.DocumentProperties(NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, this.printerSettings.PrinterName, intPtr, intPtr, 10);
				if (num < 0)
				{
					SafeNativeMethods.GlobalFree(new HandleRef(null, intPtr));
				}
			}
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevmode));
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0002AA08 File Offset: 0x00029A08
		private short ExtraBytes
		{
			get
			{
				IntPtr hdevmodeInternal = this.printerSettings.GetHdevmodeInternal();
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(this, hdevmodeInternal));
				SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
				short dmDriverExtra = devmode.dmDriverExtra;
				SafeNativeMethods.GlobalUnlock(new HandleRef(this, hdevmodeInternal));
				SafeNativeMethods.GlobalFree(new HandleRef(this, hdevmodeInternal));
				return dmDriverExtra;
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0002AA68 File Offset: 0x00029A68
		internal Rectangle GetBounds(IntPtr modeHandle)
		{
			PaperSize paperSize = this.GetPaperSize(modeHandle);
			Rectangle rectangle;
			if (this.GetLandscape(modeHandle))
			{
				rectangle = new Rectangle(0, 0, paperSize.Height, paperSize.Width);
			}
			else
			{
				rectangle = new Rectangle(0, 0, paperSize.Width, paperSize.Height);
			}
			return rectangle;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0002AAB3 File Offset: 0x00029AB3
		private bool GetLandscape(IntPtr modeHandle)
		{
			if (this.landscape.IsDefault)
			{
				return this.printerSettings.GetModeField(ModeField.Orientation, 1, modeHandle) == 2;
			}
			return (bool)this.landscape;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0002AAE0 File Offset: 0x00029AE0
		private PaperSize GetPaperSize(IntPtr modeHandle)
		{
			if (this.paperSize == null)
			{
				bool flag = false;
				if (modeHandle == IntPtr.Zero)
				{
					modeHandle = this.printerSettings.GetHdevmode();
					flag = true;
				}
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, modeHandle));
				SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
				PaperSize paperSize = this.PaperSizeFromMode(devmode);
				SafeNativeMethods.GlobalUnlock(new HandleRef(null, modeHandle));
				if (flag)
				{
					SafeNativeMethods.GlobalFree(new HandleRef(null, modeHandle));
				}
				return paperSize;
			}
			return this.paperSize;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002AB64 File Offset: 0x00029B64
		private PaperSize PaperSizeFromMode(SafeNativeMethods.DEVMODE mode)
		{
			PaperSize[] paperSizes = this.printerSettings.Get_PaperSizes();
			if ((mode.dmFields & 2) == 2)
			{
				for (int i = 0; i < paperSizes.Length; i++)
				{
					if (paperSizes[i].RawKind == (int)mode.dmPaperSize)
					{
						return paperSizes[i];
					}
				}
			}
			return new PaperSize(PaperKind.Custom, "custom", PrinterUnitConvert.Convert((int)mode.dmPaperWidth, PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display), PrinterUnitConvert.Convert((int)mode.dmPaperLength, PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display));
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0002ABD0 File Offset: 0x00029BD0
		private PaperSource PaperSourceFromMode(SafeNativeMethods.DEVMODE mode)
		{
			PaperSource[] paperSources = this.printerSettings.Get_PaperSources();
			if ((mode.dmFields & 512) == 512)
			{
				for (int i = 0; i < paperSources.Length; i++)
				{
					if ((short)paperSources[i].RawKind == mode.dmDefaultSource)
					{
						return paperSources[i];
					}
				}
			}
			return new PaperSource((PaperSourceKind)mode.dmDefaultSource, "unknown");
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0002AC30 File Offset: 0x00029C30
		private PrinterResolution PrinterResolutionFromMode(SafeNativeMethods.DEVMODE mode)
		{
			PrinterResolution[] printerResolutions = this.printerSettings.Get_PrinterResolutions();
			for (int i = 0; i < printerResolutions.Length; i++)
			{
				if (mode.dmPrintQuality >= 0 && (mode.dmFields & 1024) == 1024 && (mode.dmFields & 8192) == 8192)
				{
					if (printerResolutions[i].X == (int)mode.dmPrintQuality && printerResolutions[i].Y == (int)mode.dmYResolution)
					{
						return printerResolutions[i];
					}
				}
				else if ((mode.dmFields & 1024) == 1024 && printerResolutions[i].Kind == (PrinterResolutionKind)mode.dmPrintQuality)
				{
					return printerResolutions[i];
				}
			}
			return new PrinterResolution(PrinterResolutionKind.Custom, (int)mode.dmPrintQuality, (int)mode.dmYResolution);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0002ACE8 File Offset: 0x00029CE8
		public void SetHdevmode(IntPtr hdevmode)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			if (hdevmode == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidPrinterHandle", new object[] { hdevmode }));
			}
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevmode));
			SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
			if ((devmode.dmFields & 2048) == 2048)
			{
				this.color = devmode.dmColor == 2;
			}
			if ((devmode.dmFields & 1) == 1)
			{
				this.landscape = devmode.dmOrientation == 2;
			}
			this.paperSize = this.PaperSizeFromMode(devmode);
			this.paperSource = this.PaperSourceFromMode(devmode);
			this.printerResolution = this.PrinterResolutionFromMode(devmode);
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevmode));
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0002ADCC File Offset: 0x00029DCC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[PageSettings: Color=",
				this.Color.ToString(),
				", Landscape=",
				this.Landscape.ToString(),
				", Margins=",
				this.Margins.ToString(),
				", PaperSize=",
				this.PaperSize.ToString(),
				", PaperSource=",
				this.PaperSource.ToString(),
				", PrinterResolution=",
				this.PrinterResolution.ToString(),
				"]"
			});
		}

		// Token: 0x04000B95 RID: 2965
		internal PrinterSettings printerSettings;

		// Token: 0x04000B96 RID: 2966
		private TriState color = TriState.Default;

		// Token: 0x04000B97 RID: 2967
		private PaperSize paperSize;

		// Token: 0x04000B98 RID: 2968
		private PaperSource paperSource;

		// Token: 0x04000B99 RID: 2969
		private PrinterResolution printerResolution;

		// Token: 0x04000B9A RID: 2970
		private TriState landscape = TriState.Default;

		// Token: 0x04000B9B RID: 2971
		private Margins margins = new Margins();
	}
}

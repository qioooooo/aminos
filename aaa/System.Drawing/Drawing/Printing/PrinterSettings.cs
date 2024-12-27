using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Drawing.Printing
{
	// Token: 0x02000119 RID: 281
	[Serializable]
	public class PrinterSettings : ICloneable
	{
		// Token: 0x06000EB3 RID: 3763 RVA: 0x0002B8B8 File Offset: 0x0002A8B8
		public PrinterSettings()
		{
			this.defaultPageSettings = new PageSettings(this);
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0002B911 File Offset: 0x0002A911
		public bool CanDuplex
		{
			get
			{
				return this.DeviceCapabilities(7, IntPtr.Zero, 0) == 1;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002B923 File Offset: 0x0002A923
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0002B940 File Offset: 0x0002A940
		public short Copies
		{
			get
			{
				if (this.copies != -1)
				{
					return this.copies;
				}
				return this.GetModeField(ModeField.Copies, 1);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				IntSecurity.SafePrinting.Demand();
				this.copies = value;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002B9A2 File Offset: 0x0002A9A2
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0002B9C9 File Offset: 0x0002A9C9
		public bool Collate
		{
			get
			{
				if (!this.collate.IsDefault)
				{
					return (bool)this.collate;
				}
				return this.GetModeField(ModeField.Collate, 0) == 1;
			}
			set
			{
				this.collate = value;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0002B9D7 File Offset: 0x0002A9D7
		public PageSettings DefaultPageSettings
		{
			get
			{
				return this.defaultPageSettings;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0002B9DF File Offset: 0x0002A9DF
		internal string DriverName
		{
			get
			{
				return this.driverName;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0002B9E7 File Offset: 0x0002A9E7
		// (set) Token: 0x06000EBC RID: 3772 RVA: 0x0002BA01 File Offset: 0x0002AA01
		public Duplex Duplex
		{
			get
			{
				if (this.duplex != Duplex.Default)
				{
					return this.duplex;
				}
				return (Duplex)this.GetModeField(ModeField.Duplex, 1);
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Duplex));
				}
				this.duplex = value;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002BA30 File Offset: 0x0002AA30
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x0002BA38 File Offset: 0x0002AA38
		public int FromPage
		{
			get
			{
				return this.fromPage;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.fromPage = value;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002BA90 File Offset: 0x0002AA90
		public static PrinterSettings.StringCollection InstalledPrinters
		{
			get
			{
				IntSecurity.AllPrinting.Demand();
				int num;
				int num2;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					num = 4;
					if (IntPtr.Size == 8)
					{
						num2 = IntPtr.Size * 2 + Marshal.SizeOf(typeof(int)) + 4;
					}
					else
					{
						num2 = IntPtr.Size * 2 + Marshal.SizeOf(typeof(int));
					}
				}
				else
				{
					num = 5;
					num2 = IntPtr.Size * 2 + Marshal.SizeOf(typeof(int)) * 3;
				}
				IntSecurity.UnmanagedCode.Assert();
				string[] array;
				try
				{
					int num3;
					int num4;
					SafeNativeMethods.EnumPrinters(6, null, num, IntPtr.Zero, 0, out num3, out num4);
					IntPtr intPtr = Marshal.AllocCoTaskMem(num3);
					int num5 = SafeNativeMethods.EnumPrinters(6, null, num, intPtr, num3, out num3, out num4);
					array = new string[num4];
					if (num5 == 0)
					{
						Marshal.FreeCoTaskMem(intPtr);
						throw new Win32Exception();
					}
					for (int i = 0; i < num4; i++)
					{
						checked
						{
							IntPtr intPtr2 = Marshal.ReadIntPtr((IntPtr)((long)intPtr + unchecked((long)(checked(i * num2)))));
							array[i] = Marshal.PtrToStringAuto(intPtr2);
						}
					}
					Marshal.FreeCoTaskMem(intPtr);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return new PrinterSettings.StringCollection(array);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0002BBBC File Offset: 0x0002ABBC
		public bool IsDefaultPrinter
		{
			get
			{
				return this.printerName == null || this.printerName == PrinterSettings.GetDefaultPrinterName();
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0002BBD8 File Offset: 0x0002ABD8
		public bool IsPlotter
		{
			get
			{
				return this.GetDeviceCaps(2, 2) == 0;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0002BBE5 File Offset: 0x0002ABE5
		public bool IsValid
		{
			get
			{
				return this.DeviceCapabilities(18, IntPtr.Zero, -1) != -1;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0002BBFB File Offset: 0x0002ABFB
		public int LandscapeAngle
		{
			get
			{
				return this.DeviceCapabilities(17, IntPtr.Zero, 0);
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0002BC0B File Offset: 0x0002AC0B
		public int MaximumCopies
		{
			get
			{
				return this.DeviceCapabilities(18, IntPtr.Zero, 1);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0002BC1B File Offset: 0x0002AC1B
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0002BC24 File Offset: 0x0002AC24
		public int MaximumPage
		{
			get
			{
				return this.maxPage;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.maxPage = value;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002BC7C File Offset: 0x0002AC7C
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0002BC84 File Offset: 0x0002AC84
		public int MinimumPage
		{
			get
			{
				return this.minPage;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.minPage = value;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002BCDC File Offset: 0x0002ACDC
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0002BCE4 File Offset: 0x0002ACE4
		internal string OutputPort
		{
			get
			{
				return this.outputPort;
			}
			set
			{
				this.outputPort = value;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002BCF0 File Offset: 0x0002ACF0
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0002BD13 File Offset: 0x0002AD13
		public string PrintFileName
		{
			get
			{
				string text = this.OutputPort;
				if (!string.IsNullOrEmpty(text))
				{
					IntSecurity.DemandReadFileIO(text);
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(value);
				}
				IntSecurity.DemandWriteFileIO(value);
				this.OutputPort = value;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002BD31 File Offset: 0x0002AD31
		public PrinterSettings.PaperSizeCollection PaperSizes
		{
			get
			{
				return new PrinterSettings.PaperSizeCollection(this.Get_PaperSizes());
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0002BD3E File Offset: 0x0002AD3E
		public PrinterSettings.PaperSourceCollection PaperSources
		{
			get
			{
				return new PrinterSettings.PaperSourceCollection(this.Get_PaperSources());
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0002BD4B File Offset: 0x0002AD4B
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0002BD53 File Offset: 0x0002AD53
		internal bool PrintDialogDisplayed
		{
			get
			{
				return this.printDialogDisplayed;
			}
			set
			{
				IntSecurity.AllPrinting.Demand();
				this.printDialogDisplayed = value;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0002BD66 File Offset: 0x0002AD66
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x0002BD6E File Offset: 0x0002AD6E
		public PrintRange PrintRange
		{
			get
			{
				return this.printRange;
			}
			set
			{
				if (!Enum.IsDefined(typeof(PrintRange), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PrintRange));
				}
				this.printRange = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0002BDA4 File Offset: 0x0002ADA4
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x0002BDAC File Offset: 0x0002ADAC
		public bool PrintToFile
		{
			get
			{
				return this.printToFile;
			}
			set
			{
				this.printToFile = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0002BDB5 File Offset: 0x0002ADB5
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0002BDC7 File Offset: 0x0002ADC7
		public string PrinterName
		{
			get
			{
				IntSecurity.AllPrinting.Demand();
				return this.PrinterNameInternal;
			}
			set
			{
				IntSecurity.AllPrinting.Demand();
				this.PrinterNameInternal = value;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0002BDDA File Offset: 0x0002ADDA
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0002BDF0 File Offset: 0x0002ADF0
		private string PrinterNameInternal
		{
			get
			{
				if (this.printerName == null)
				{
					return PrinterSettings.GetDefaultPrinterName();
				}
				return this.printerName;
			}
			set
			{
				this.cachedDevmode = null;
				this.extrainfo = null;
				this.printerName = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0002BE07 File Offset: 0x0002AE07
		public PrinterSettings.PrinterResolutionCollection PrinterResolutions
		{
			get
			{
				return new PrinterSettings.PrinterResolutionCollection(this.Get_PrinterResolutions());
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002BE14 File Offset: 0x0002AE14
		public bool IsDirectPrintingSupported(ImageFormat imageFormat)
		{
			bool flag = false;
			if (imageFormat.Equals(ImageFormat.Jpeg) || imageFormat.Equals(ImageFormat.Png))
			{
				int num = (imageFormat.Equals(ImageFormat.Jpeg) ? 4119 : 4120);
				int num2 = 0;
				DeviceContext deviceContext = this.CreateInformationContext(this.DefaultPageSettings);
				HandleRef handleRef = new HandleRef(deviceContext, deviceContext.Hdc);
				try
				{
					flag = SafeNativeMethods.ExtEscape(handleRef, 8, Marshal.SizeOf(typeof(int)), ref num, 0, out num2) > 0;
				}
				finally
				{
					deviceContext.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002BEB0 File Offset: 0x0002AEB0
		public bool IsDirectPrintingSupported(Image image)
		{
			bool flag = false;
			if (image.RawFormat.Equals(ImageFormat.Jpeg) || image.RawFormat.Equals(ImageFormat.Png))
			{
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					image.Save(memoryStream, image.RawFormat);
					memoryStream.Position = 0L;
					using (BufferedStream bufferedStream = new BufferedStream(memoryStream))
					{
						int num = (int)bufferedStream.Length;
						byte[] array = new byte[num];
						bufferedStream.Read(array, 0, num);
						int num2 = (image.RawFormat.Equals(ImageFormat.Jpeg) ? 4119 : 4120);
						int num3 = 0;
						DeviceContext deviceContext = this.CreateInformationContext(this.DefaultPageSettings);
						HandleRef handleRef = new HandleRef(deviceContext, deviceContext.Hdc);
						try
						{
							bool flag2 = SafeNativeMethods.ExtEscape(handleRef, 8, Marshal.SizeOf(typeof(int)), ref num2, 0, out num3) > 0;
							if (flag2)
							{
								flag = SafeNativeMethods.ExtEscape(handleRef, num2, num, array, Marshal.SizeOf(typeof(int)), out num3) > 0 && num3 == 1;
							}
						}
						finally
						{
							deviceContext.Dispose();
						}
					}
				}
				finally
				{
					memoryStream.Close();
				}
			}
			return flag;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x0002BFFC File Offset: 0x0002AFFC
		public bool SupportsColor
		{
			get
			{
				return this.GetDeviceCaps(12, 1) > 1;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x0002C00A File Offset: 0x0002B00A
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x0002C014 File Offset: 0x0002B014
		public int ToPage
		{
			get
			{
				return this.toPage;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.toPage = value;
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0002C06C File Offset: 0x0002B06C
		public object Clone()
		{
			PrinterSettings printerSettings = (PrinterSettings)base.MemberwiseClone();
			printerSettings.printDialogDisplayed = false;
			return printerSettings;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002C090 File Offset: 0x0002B090
		internal DeviceContext CreateDeviceContext(PageSettings pageSettings)
		{
			IntPtr hdevmodeInternal = this.GetHdevmodeInternal();
			DeviceContext deviceContext = null;
			try
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				try
				{
					pageSettings.CopyToHdevmode(hdevmodeInternal);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				deviceContext = this.CreateDeviceContext(hdevmodeInternal);
			}
			finally
			{
				SafeNativeMethods.GlobalFree(new HandleRef(null, hdevmodeInternal));
			}
			return deviceContext;
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0002C0F4 File Offset: 0x0002B0F4
		internal DeviceContext CreateDeviceContext(IntPtr hdevmode)
		{
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevmode));
			DeviceContext deviceContext = DeviceContext.CreateDC(this.DriverName, this.PrinterNameInternal, null, new HandleRef(null, intPtr));
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevmode));
			return deviceContext;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002C138 File Offset: 0x0002B138
		internal DeviceContext CreateInformationContext(PageSettings pageSettings)
		{
			IntPtr hdevmodeInternal = this.GetHdevmodeInternal();
			DeviceContext deviceContext;
			try
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				try
				{
					pageSettings.CopyToHdevmode(hdevmodeInternal);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				deviceContext = this.CreateInformationContext(hdevmodeInternal);
			}
			finally
			{
				SafeNativeMethods.GlobalFree(new HandleRef(null, hdevmodeInternal));
			}
			return deviceContext;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0002C19C File Offset: 0x0002B19C
		internal DeviceContext CreateInformationContext(IntPtr hdevmode)
		{
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevmode));
			DeviceContext deviceContext = DeviceContext.CreateIC(this.DriverName, this.PrinterNameInternal, null, new HandleRef(null, intPtr));
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevmode));
			return deviceContext;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0002C1DE File Offset: 0x0002B1DE
		public Graphics CreateMeasurementGraphics()
		{
			return this.CreateMeasurementGraphics(this.DefaultPageSettings);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0002C1EC File Offset: 0x0002B1EC
		public Graphics CreateMeasurementGraphics(bool honorOriginAtMargins)
		{
			Graphics graphics = this.CreateMeasurementGraphics();
			if (graphics != null && honorOriginAtMargins)
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				try
				{
					graphics.TranslateTransform(-this.defaultPageSettings.HardMarginX, -this.defaultPageSettings.HardMarginY);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				graphics.TranslateTransform((float)this.defaultPageSettings.Margins.Left, (float)this.defaultPageSettings.Margins.Top);
			}
			return graphics;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0002C270 File Offset: 0x0002B270
		public Graphics CreateMeasurementGraphics(PageSettings pageSettings)
		{
			DeviceContext deviceContext = this.CreateDeviceContext(pageSettings);
			Graphics graphics = Graphics.FromHdcInternal(deviceContext.Hdc);
			graphics.PrintingHelper = deviceContext;
			return graphics;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002C29C File Offset: 0x0002B29C
		public Graphics CreateMeasurementGraphics(PageSettings pageSettings, bool honorOriginAtMargins)
		{
			Graphics graphics = this.CreateMeasurementGraphics();
			if (graphics != null && honorOriginAtMargins)
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				try
				{
					graphics.TranslateTransform(-pageSettings.HardMarginX, -pageSettings.HardMarginY);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				graphics.TranslateTransform((float)pageSettings.Margins.Left, (float)pageSettings.Margins.Top);
			}
			return graphics;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0002C30C File Offset: 0x0002B30C
		private static SafeNativeMethods.PRINTDLGX86 CreatePRINTDLGX86()
		{
			return new SafeNativeMethods.PRINTDLGX86
			{
				lStructSize = Marshal.SizeOf(typeof(SafeNativeMethods.PRINTDLGX86)),
				hwndOwner = IntPtr.Zero,
				hDevMode = IntPtr.Zero,
				hDevNames = IntPtr.Zero,
				Flags = 0,
				hwndOwner = IntPtr.Zero,
				hDC = IntPtr.Zero,
				nFromPage = 1,
				nToPage = 1,
				nMinPage = 0,
				nMaxPage = 9999,
				nCopies = 1,
				hInstance = IntPtr.Zero,
				lCustData = IntPtr.Zero,
				lpfnPrintHook = IntPtr.Zero,
				lpfnSetupHook = IntPtr.Zero,
				lpPrintTemplateName = null,
				lpSetupTemplateName = null,
				hPrintTemplate = IntPtr.Zero,
				hSetupTemplate = IntPtr.Zero
			};
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0002C3EC File Offset: 0x0002B3EC
		private static SafeNativeMethods.PRINTDLG CreatePRINTDLG()
		{
			return new SafeNativeMethods.PRINTDLG
			{
				lStructSize = Marshal.SizeOf(typeof(SafeNativeMethods.PRINTDLG)),
				hwndOwner = IntPtr.Zero,
				hDevMode = IntPtr.Zero,
				hDevNames = IntPtr.Zero,
				Flags = 0,
				hwndOwner = IntPtr.Zero,
				hDC = IntPtr.Zero,
				nFromPage = 1,
				nToPage = 1,
				nMinPage = 0,
				nMaxPage = 9999,
				nCopies = 1,
				hInstance = IntPtr.Zero,
				lCustData = IntPtr.Zero,
				lpfnPrintHook = IntPtr.Zero,
				lpfnSetupHook = IntPtr.Zero,
				lpPrintTemplateName = null,
				lpSetupTemplateName = null,
				hPrintTemplate = IntPtr.Zero,
				hSetupTemplate = IntPtr.Zero
			};
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0002C4CC File Offset: 0x0002B4CC
		private int DeviceCapabilities(short capability, IntPtr pointerToBuffer, int defaultValue)
		{
			IntSecurity.AllPrinting.Assert();
			string text = this.PrinterName;
			CodeAccessPermission.RevertAssert();
			IntSecurity.UnmanagedCode.Assert();
			return PrinterSettings.FastDeviceCapabilities(capability, pointerToBuffer, defaultValue, text);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0002C504 File Offset: 0x0002B504
		private static int FastDeviceCapabilities(short capability, IntPtr pointerToBuffer, int defaultValue, string printerName)
		{
			int num = SafeNativeMethods.DeviceCapabilities(printerName, PrinterSettings.GetOutputPort(), capability, pointerToBuffer, IntPtr.Zero);
			if (num == -1)
			{
				return defaultValue;
			}
			return num;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0002C52C File Offset: 0x0002B52C
		private static string GetDefaultPrinterName()
		{
			IntSecurity.UnmanagedCode.Assert();
			if (IntPtr.Size == 8)
			{
				SafeNativeMethods.PRINTDLG printdlg = PrinterSettings.CreatePRINTDLG();
				printdlg.Flags = 1024;
				if (!SafeNativeMethods.PrintDlg(printdlg))
				{
					return SR.GetString("NoDefaultPrinter");
				}
				IntPtr hDevNames = printdlg.hDevNames;
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(printdlg, hDevNames));
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string text = PrinterSettings.ReadOneDEVNAME(intPtr, 1);
				SafeNativeMethods.GlobalUnlock(new HandleRef(printdlg, hDevNames));
				intPtr = IntPtr.Zero;
				SafeNativeMethods.GlobalFree(new HandleRef(printdlg, printdlg.hDevNames));
				SafeNativeMethods.GlobalFree(new HandleRef(printdlg, printdlg.hDevMode));
				return text;
			}
			else
			{
				SafeNativeMethods.PRINTDLGX86 printdlgx = PrinterSettings.CreatePRINTDLGX86();
				printdlgx.Flags = 1024;
				if (!SafeNativeMethods.PrintDlg(printdlgx))
				{
					return SR.GetString("NoDefaultPrinter");
				}
				IntPtr hDevNames2 = printdlgx.hDevNames;
				IntPtr intPtr2 = SafeNativeMethods.GlobalLock(new HandleRef(printdlgx, hDevNames2));
				if (intPtr2 == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string text2 = PrinterSettings.ReadOneDEVNAME(intPtr2, 1);
				SafeNativeMethods.GlobalUnlock(new HandleRef(printdlgx, hDevNames2));
				intPtr2 = IntPtr.Zero;
				SafeNativeMethods.GlobalFree(new HandleRef(printdlgx, printdlgx.hDevNames));
				SafeNativeMethods.GlobalFree(new HandleRef(printdlgx, printdlgx.hDevMode));
				return text2;
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0002C680 File Offset: 0x0002B680
		private static string GetOutputPort()
		{
			IntSecurity.UnmanagedCode.Assert();
			if (IntPtr.Size == 8)
			{
				SafeNativeMethods.PRINTDLG printdlg = PrinterSettings.CreatePRINTDLG();
				printdlg.Flags = 1024;
				if (!SafeNativeMethods.PrintDlg(printdlg))
				{
					return SR.GetString("NoDefaultPrinter");
				}
				IntPtr hDevNames = printdlg.hDevNames;
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(printdlg, hDevNames));
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string text = PrinterSettings.ReadOneDEVNAME(intPtr, 2);
				SafeNativeMethods.GlobalUnlock(new HandleRef(printdlg, hDevNames));
				intPtr = IntPtr.Zero;
				SafeNativeMethods.GlobalFree(new HandleRef(printdlg, printdlg.hDevNames));
				SafeNativeMethods.GlobalFree(new HandleRef(printdlg, printdlg.hDevMode));
				return text;
			}
			else
			{
				SafeNativeMethods.PRINTDLGX86 printdlgx = PrinterSettings.CreatePRINTDLGX86();
				printdlgx.Flags = 1024;
				if (!SafeNativeMethods.PrintDlg(printdlgx))
				{
					return SR.GetString("NoDefaultPrinter");
				}
				IntPtr hDevNames2 = printdlgx.hDevNames;
				IntPtr intPtr2 = SafeNativeMethods.GlobalLock(new HandleRef(printdlgx, hDevNames2));
				if (intPtr2 == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string text2 = PrinterSettings.ReadOneDEVNAME(intPtr2, 2);
				SafeNativeMethods.GlobalUnlock(new HandleRef(printdlgx, hDevNames2));
				intPtr2 = IntPtr.Zero;
				SafeNativeMethods.GlobalFree(new HandleRef(printdlgx, printdlgx.hDevNames));
				SafeNativeMethods.GlobalFree(new HandleRef(printdlgx, printdlgx.hDevMode));
				return text2;
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0002C7D4 File Offset: 0x0002B7D4
		private int GetDeviceCaps(int capability, int defaultValue)
		{
			DeviceContext deviceContext = this.CreateInformationContext(this.DefaultPageSettings);
			int num = defaultValue;
			try
			{
				num = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(deviceContext, deviceContext.Hdc), capability);
			}
			catch (InvalidPrinterException)
			{
			}
			finally
			{
				deviceContext.Dispose();
			}
			return num;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0002C830 File Offset: 0x0002B830
		public IntPtr GetHdevmode()
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			IntPtr hdevmodeInternal = this.GetHdevmodeInternal();
			this.defaultPageSettings.CopyToHdevmode(hdevmodeInternal);
			return hdevmodeInternal;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0002C85B File Offset: 0x0002B85B
		internal IntPtr GetHdevmodeInternal()
		{
			return this.GetHdevmodeInternal(this.PrinterNameInternal);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0002C86C File Offset: 0x0002B86C
		private IntPtr GetHdevmodeInternal(string printer)
		{
			int num = SafeNativeMethods.DocumentProperties(NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, printer, IntPtr.Zero, NativeMethods.NullHandleRef, 0);
			if (num < 1)
			{
				throw new InvalidPrinterException(this);
			}
			IntPtr intPtr = SafeNativeMethods.GlobalAlloc(2, (uint)num);
			IntPtr intPtr2 = SafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
			if (this.cachedDevmode != null)
			{
				Marshal.Copy(this.cachedDevmode, 0, intPtr2, (int)this.devmodebytes);
			}
			else
			{
				int num2 = SafeNativeMethods.DocumentProperties(NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, printer, intPtr2, NativeMethods.NullHandleRef, 2);
				if (num2 < 0)
				{
					throw new Win32Exception();
				}
			}
			SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr2, typeof(SafeNativeMethods.DEVMODE));
			checked
			{
				if (this.extrainfo != null && this.extrabytes <= devmode.dmDriverExtra)
				{
					IntPtr intPtr3 = (IntPtr)((long)intPtr2 + unchecked((long)devmode.dmSize));
					Marshal.Copy(this.extrainfo, 0, intPtr3, (int)this.extrabytes);
				}
				if ((devmode.dmFields & 256) == 256 && this.copies != -1)
				{
					devmode.dmCopies = this.copies;
				}
			}
			if ((devmode.dmFields & 4096) == 4096 && this.duplex != Duplex.Default)
			{
				devmode.dmDuplex = (short)this.duplex;
			}
			if ((devmode.dmFields & 32768) == 32768 && this.collate.IsNotDefault)
			{
				devmode.dmCollate = (((bool)this.collate) ? 1 : 0);
			}
			Marshal.StructureToPtr(devmode, intPtr2, false);
			int num3 = SafeNativeMethods.DocumentProperties(NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, printer, intPtr2, intPtr2, 10);
			if (num3 < 0)
			{
				SafeNativeMethods.GlobalFree(new HandleRef(null, intPtr));
				SafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
				return IntPtr.Zero;
			}
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
			return intPtr;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002CA30 File Offset: 0x0002BA30
		public IntPtr GetHdevmode(PageSettings pageSettings)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			IntPtr hdevmodeInternal = this.GetHdevmodeInternal();
			pageSettings.CopyToHdevmode(hdevmodeInternal);
			return hdevmodeInternal;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0002CA58 File Offset: 0x0002BA58
		public IntPtr GetHdevnames()
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			string text = this.PrinterName;
			string text2 = this.DriverName;
			string text3 = this.OutputPort;
			int num = checked(4 + text.Length + text2.Length + text3.Length);
			short num2 = (short)(8 / Marshal.SystemDefaultCharSize);
			uint num3 = (uint)(checked(Marshal.SystemDefaultCharSize * ((int)num2 + num)));
			IntPtr intPtr = SafeNativeMethods.GlobalAlloc(66, num3);
			IntPtr intPtr2 = SafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
			Marshal.WriteInt16(intPtr2, num2);
			num2 += this.WriteOneDEVNAME(text2, intPtr2, (int)num2);
			Marshal.WriteInt16((IntPtr)(checked((long)intPtr2 + 2L)), num2);
			num2 += this.WriteOneDEVNAME(text, intPtr2, (int)num2);
			Marshal.WriteInt16((IntPtr)(checked((long)intPtr2 + 4L)), num2);
			num2 += this.WriteOneDEVNAME(text3, intPtr2, (int)num2);
			Marshal.WriteInt16((IntPtr)(checked((long)intPtr2 + 6L)), num2);
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
			return intPtr;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0002CB59 File Offset: 0x0002BB59
		internal short GetModeField(ModeField field, short defaultValue)
		{
			return this.GetModeField(field, defaultValue, IntPtr.Zero);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0002CB68 File Offset: 0x0002BB68
		internal short GetModeField(ModeField field, short defaultValue, IntPtr modeHandle)
		{
			bool flag = false;
			short num;
			try
			{
				if (modeHandle == IntPtr.Zero)
				{
					try
					{
						modeHandle = this.GetHdevmodeInternal();
						flag = true;
					}
					catch (InvalidPrinterException)
					{
						return defaultValue;
					}
				}
				IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(this, modeHandle));
				SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
				switch (field)
				{
				case ModeField.Orientation:
					num = devmode.dmOrientation;
					break;
				case ModeField.PaperSize:
					num = devmode.dmPaperSize;
					break;
				case ModeField.PaperLength:
					num = devmode.dmPaperLength;
					break;
				case ModeField.PaperWidth:
					num = devmode.dmPaperWidth;
					break;
				case ModeField.Copies:
					num = devmode.dmCopies;
					break;
				case ModeField.DefaultSource:
					num = devmode.dmDefaultSource;
					break;
				case ModeField.PrintQuality:
					num = devmode.dmPrintQuality;
					break;
				case ModeField.Color:
					num = devmode.dmColor;
					break;
				case ModeField.Duplex:
					num = devmode.dmDuplex;
					break;
				case ModeField.YResolution:
					num = devmode.dmYResolution;
					break;
				case ModeField.TTOption:
					num = devmode.dmTTOption;
					break;
				case ModeField.Collate:
					num = devmode.dmCollate;
					break;
				default:
					num = defaultValue;
					break;
				}
				SafeNativeMethods.GlobalUnlock(new HandleRef(this, modeHandle));
			}
			finally
			{
				if (flag)
				{
					SafeNativeMethods.GlobalFree(new HandleRef(this, modeHandle));
				}
			}
			return num;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0002CCA8 File Offset: 0x0002BCA8
		internal PaperSize[] Get_PaperSizes()
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			string text = this.PrinterName;
			int num = PrinterSettings.FastDeviceCapabilities(16, IntPtr.Zero, -1, text);
			if (num == -1)
			{
				return new PaperSize[0];
			}
			int num2 = Marshal.SystemDefaultCharSize * 64;
			IntPtr intPtr = Marshal.AllocCoTaskMem(checked(num2 * num));
			PrinterSettings.FastDeviceCapabilities(16, intPtr, -1, text);
			IntPtr intPtr2 = Marshal.AllocCoTaskMem(2 * num);
			PrinterSettings.FastDeviceCapabilities(2, intPtr2, -1, text);
			IntPtr intPtr3 = Marshal.AllocCoTaskMem(8 * num);
			PrinterSettings.FastDeviceCapabilities(3, intPtr3, -1, text);
			PaperSize[] array = new PaperSize[num];
			for (int i = 0; i < num; i++)
			{
				checked
				{
					string text2 = Marshal.PtrToStringAuto((IntPtr)((long)intPtr + unchecked((long)(checked(num2 * i)))), 64);
					int num3 = text2.IndexOf('\0');
					if (num3 > -1)
					{
						text2 = text2.Substring(0, num3);
					}
					short num4 = Marshal.ReadInt16((IntPtr)((long)intPtr2 + unchecked((long)(checked(i * 2)))));
					int num5 = Marshal.ReadInt32((IntPtr)((long)intPtr3 + unchecked((long)(checked(i * 8)))));
					int num6 = Marshal.ReadInt32((IntPtr)((long)intPtr3 + unchecked((long)(checked(i * 8))) + 4L));
					array[i] = new PaperSize((PaperKind)num4, text2, PrinterUnitConvert.Convert(num5, PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display), PrinterUnitConvert.Convert(num6, PrinterUnit.TenthsOfAMillimeter, PrinterUnit.Display));
				}
			}
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			Marshal.FreeCoTaskMem(intPtr3);
			return array;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0002CE00 File Offset: 0x0002BE00
		internal PaperSource[] Get_PaperSources()
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			string text = this.PrinterName;
			int num = PrinterSettings.FastDeviceCapabilities(12, IntPtr.Zero, -1, text);
			if (num == -1)
			{
				return new PaperSource[0];
			}
			int num2 = Marshal.SystemDefaultCharSize * 24;
			IntPtr intPtr = Marshal.AllocCoTaskMem(checked(num2 * num));
			PrinterSettings.FastDeviceCapabilities(12, intPtr, -1, text);
			IntPtr intPtr2 = Marshal.AllocCoTaskMem(2 * num);
			PrinterSettings.FastDeviceCapabilities(6, intPtr2, -1, text);
			PaperSource[] array = new PaperSource[num];
			for (int i = 0; i < num; i++)
			{
				checked
				{
					string text2 = Marshal.PtrToStringAuto((IntPtr)((long)intPtr + unchecked((long)(checked(num2 * i)))));
					short num3 = Marshal.ReadInt16((IntPtr)((long)intPtr2 + unchecked((long)(checked(2 * i)))));
					array[i] = new PaperSource((PaperSourceKind)num3, text2);
				}
			}
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return array;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0002CED0 File Offset: 0x0002BED0
		internal PrinterResolution[] Get_PrinterResolutions()
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			string text = this.PrinterName;
			int num = PrinterSettings.FastDeviceCapabilities(13, IntPtr.Zero, -1, text);
			if (num == -1)
			{
				return new PrinterResolution[]
				{
					new PrinterResolution(PrinterResolutionKind.High, -4, -1),
					new PrinterResolution(PrinterResolutionKind.Medium, -3, -1),
					new PrinterResolution(PrinterResolutionKind.Low, -2, -1),
					new PrinterResolution(PrinterResolutionKind.Draft, -1, -1)
				};
			}
			PrinterResolution[] array = new PrinterResolution[num + 4];
			array[0] = new PrinterResolution(PrinterResolutionKind.High, -4, -1);
			array[1] = new PrinterResolution(PrinterResolutionKind.Medium, -3, -1);
			array[2] = new PrinterResolution(PrinterResolutionKind.Low, -2, -1);
			array[3] = new PrinterResolution(PrinterResolutionKind.Draft, -1, -1);
			IntPtr intPtr = Marshal.AllocCoTaskMem(checked(8 * num));
			PrinterSettings.FastDeviceCapabilities(13, intPtr, -1, text);
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				checked
				{
					num2 = Marshal.ReadInt32((IntPtr)((long)intPtr + unchecked((long)(checked(i * 8)))));
					num3 = Marshal.ReadInt32((IntPtr)((long)intPtr + unchecked((long)(checked(i * 8))) + 4L));
				}
				array[i + 4] = new PrinterResolution(PrinterResolutionKind.Custom, num2, num3);
			}
			Marshal.FreeCoTaskMem(intPtr);
			return array;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0002CFE8 File Offset: 0x0002BFE8
		private static string ReadOneDEVNAME(IntPtr pDevnames, int slot)
		{
			checked
			{
				int num = Marshal.SystemDefaultCharSize * (int)Marshal.ReadInt16((IntPtr)((long)pDevnames + unchecked((long)(checked(slot * 2)))));
				return Marshal.PtrToStringAuto((IntPtr)((long)pDevnames + unchecked((long)num)));
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0002D028 File Offset: 0x0002C028
		public void SetHdevmode(IntPtr hdevmode)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			if (hdevmode == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidPrinterHandle", new object[] { hdevmode }));
			}
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevmode));
			SafeNativeMethods.DEVMODE devmode = (SafeNativeMethods.DEVMODE)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(SafeNativeMethods.DEVMODE));
			this.devmodebytes = devmode.dmSize;
			if (this.devmodebytes > 0)
			{
				this.cachedDevmode = new byte[(int)this.devmodebytes];
				Marshal.Copy(intPtr, this.cachedDevmode, 0, (int)this.devmodebytes);
			}
			this.extrabytes = devmode.dmDriverExtra;
			checked
			{
				if (this.extrabytes > 0)
				{
					this.extrainfo = new byte[(int)this.extrabytes];
					Marshal.Copy((IntPtr)((long)intPtr + unchecked((long)devmode.dmSize)), this.extrainfo, 0, (int)this.extrabytes);
				}
				if ((devmode.dmFields & 256) == 256)
				{
					this.copies = devmode.dmCopies;
				}
				if ((devmode.dmFields & 4096) == 4096)
				{
					this.duplex = (Duplex)devmode.dmDuplex;
				}
				if ((devmode.dmFields & 32768) == 32768)
				{
					this.collate = devmode.dmCollate == 1;
				}
				SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevmode));
			}
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0002D188 File Offset: 0x0002C188
		public void SetHdevnames(IntPtr hdevnames)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Demand();
			if (hdevnames == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidPrinterHandle", new object[] { hdevnames }));
			}
			IntPtr intPtr = SafeNativeMethods.GlobalLock(new HandleRef(null, hdevnames));
			this.driverName = PrinterSettings.ReadOneDEVNAME(intPtr, 0);
			this.printerName = PrinterSettings.ReadOneDEVNAME(intPtr, 1);
			this.outputPort = PrinterSettings.ReadOneDEVNAME(intPtr, 2);
			this.PrintDialogDisplayed = true;
			SafeNativeMethods.GlobalUnlock(new HandleRef(null, hdevnames));
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0002D218 File Offset: 0x0002C218
		public override string ToString()
		{
			string text = (IntSecurity.HasPermission(IntSecurity.AllPrinting) ? this.PrinterName : "<printer name unavailable>");
			return string.Concat(new string[]
			{
				"[PrinterSettings ",
				text,
				" Copies=",
				this.Copies.ToString(CultureInfo.InvariantCulture),
				" Collate=",
				this.Collate.ToString(CultureInfo.InvariantCulture),
				" Duplex=",
				TypeDescriptor.GetConverter(typeof(Duplex)).ConvertToString((int)this.Duplex),
				" FromPage=",
				this.FromPage.ToString(CultureInfo.InvariantCulture),
				" LandscapeAngle=",
				this.LandscapeAngle.ToString(CultureInfo.InvariantCulture),
				" MaximumCopies=",
				this.MaximumCopies.ToString(CultureInfo.InvariantCulture),
				" OutputPort=",
				this.OutputPort.ToString(CultureInfo.InvariantCulture),
				" ToPage=",
				this.ToPage.ToString(CultureInfo.InvariantCulture),
				"]"
			});
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0002D368 File Offset: 0x0002C368
		private short WriteOneDEVNAME(string str, IntPtr bufferStart, int index)
		{
			if (str == null)
			{
				str = "";
			}
			checked
			{
				IntPtr intPtr = (IntPtr)((long)bufferStart + unchecked((long)(checked(index * Marshal.SystemDefaultCharSize))));
				if (Marshal.SystemDefaultCharSize == 1)
				{
					byte[] bytes = Encoding.Default.GetBytes(str);
					Marshal.Copy(bytes, 0, intPtr, bytes.Length);
					Marshal.WriteByte((IntPtr)((long)intPtr + unchecked((long)bytes.Length)), 0);
				}
				else
				{
					char[] array = str.ToCharArray();
					Marshal.Copy(array, 0, intPtr, array.Length);
					Marshal.WriteInt16((IntPtr)((long)intPtr + unchecked((long)(checked(array.Length * 2)))), 0);
				}
				return (short)(str.Length + 1);
			}
		}

		// Token: 0x04000C45 RID: 3141
		private const int PADDING_IA64 = 4;

		// Token: 0x04000C46 RID: 3142
		private string printerName;

		// Token: 0x04000C47 RID: 3143
		private string driverName = "";

		// Token: 0x04000C48 RID: 3144
		private string outputPort = "";

		// Token: 0x04000C49 RID: 3145
		private bool printToFile;

		// Token: 0x04000C4A RID: 3146
		private bool printDialogDisplayed;

		// Token: 0x04000C4B RID: 3147
		private short extrabytes;

		// Token: 0x04000C4C RID: 3148
		private byte[] extrainfo;

		// Token: 0x04000C4D RID: 3149
		private short copies = -1;

		// Token: 0x04000C4E RID: 3150
		private Duplex duplex = Duplex.Default;

		// Token: 0x04000C4F RID: 3151
		private TriState collate = TriState.Default;

		// Token: 0x04000C50 RID: 3152
		private PageSettings defaultPageSettings;

		// Token: 0x04000C51 RID: 3153
		private int fromPage;

		// Token: 0x04000C52 RID: 3154
		private int toPage;

		// Token: 0x04000C53 RID: 3155
		private int maxPage = 9999;

		// Token: 0x04000C54 RID: 3156
		private int minPage;

		// Token: 0x04000C55 RID: 3157
		private PrintRange printRange;

		// Token: 0x04000C56 RID: 3158
		private short devmodebytes;

		// Token: 0x04000C57 RID: 3159
		private byte[] cachedDevmode;

		// Token: 0x0200011A RID: 282
		public class PaperSizeCollection : ICollection, IEnumerable
		{
			// Token: 0x06000EFE RID: 3838 RVA: 0x0002D3FE File Offset: 0x0002C3FE
			public PaperSizeCollection(PaperSize[] array)
			{
				this.array = array;
			}

			// Token: 0x170003D0 RID: 976
			// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002D40D File Offset: 0x0002C40D
			public int Count
			{
				get
				{
					return this.array.Length;
				}
			}

			// Token: 0x170003D1 RID: 977
			public virtual PaperSize this[int index]
			{
				get
				{
					return this.array[index];
				}
			}

			// Token: 0x06000F01 RID: 3841 RVA: 0x0002D421 File Offset: 0x0002C421
			public IEnumerator GetEnumerator()
			{
				return new PrinterSettings.ArrayEnumerator(this.array, 0, this.Count);
			}

			// Token: 0x170003D2 RID: 978
			// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0002D435 File Offset: 0x0002C435
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002D43D File Offset: 0x0002C43D
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0002D440 File Offset: 0x0002C440
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x06000F05 RID: 3845 RVA: 0x0002D443 File Offset: 0x0002C443
			void ICollection.CopyTo(Array array, int index)
			{
				Array.Copy(this.array, index, array, 0, this.array.Length);
			}

			// Token: 0x06000F06 RID: 3846 RVA: 0x0002D45B File Offset: 0x0002C45B
			public void CopyTo(PaperSize[] paperSizes, int index)
			{
				Array.Copy(this.array, index, paperSizes, 0, this.array.Length);
			}

			// Token: 0x06000F07 RID: 3847 RVA: 0x0002D473 File Offset: 0x0002C473
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F08 RID: 3848 RVA: 0x0002D47C File Offset: 0x0002C47C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(PaperSize paperSize)
			{
				PaperSize[] array = new PaperSize[this.Count + 1];
				((ICollection)this).CopyTo(array, 0);
				array[this.Count] = paperSize;
				this.array = array;
				return this.Count;
			}

			// Token: 0x04000C58 RID: 3160
			private PaperSize[] array;
		}

		// Token: 0x0200011B RID: 283
		public class PaperSourceCollection : ICollection, IEnumerable
		{
			// Token: 0x06000F09 RID: 3849 RVA: 0x0002D4B5 File Offset: 0x0002C4B5
			public PaperSourceCollection(PaperSource[] array)
			{
				this.array = array;
			}

			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0002D4C4 File Offset: 0x0002C4C4
			public int Count
			{
				get
				{
					return this.array.Length;
				}
			}

			// Token: 0x170003D6 RID: 982
			public virtual PaperSource this[int index]
			{
				get
				{
					return this.array[index];
				}
			}

			// Token: 0x06000F0C RID: 3852 RVA: 0x0002D4D8 File Offset: 0x0002C4D8
			public IEnumerator GetEnumerator()
			{
				return new PrinterSettings.ArrayEnumerator(this.array, 0, this.Count);
			}

			// Token: 0x170003D7 RID: 983
			// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0002D4EC File Offset: 0x0002C4EC
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x170003D8 RID: 984
			// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002D4F4 File Offset: 0x0002C4F4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003D9 RID: 985
			// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002D4F7 File Offset: 0x0002C4F7
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x06000F10 RID: 3856 RVA: 0x0002D4FA File Offset: 0x0002C4FA
			void ICollection.CopyTo(Array array, int index)
			{
				Array.Copy(this.array, index, array, 0, this.array.Length);
			}

			// Token: 0x06000F11 RID: 3857 RVA: 0x0002D512 File Offset: 0x0002C512
			public void CopyTo(PaperSource[] paperSources, int index)
			{
				Array.Copy(this.array, index, paperSources, 0, this.array.Length);
			}

			// Token: 0x06000F12 RID: 3858 RVA: 0x0002D52A File Offset: 0x0002C52A
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F13 RID: 3859 RVA: 0x0002D534 File Offset: 0x0002C534
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(PaperSource paperSource)
			{
				PaperSource[] array = new PaperSource[this.Count + 1];
				((ICollection)this).CopyTo(array, 0);
				array[this.Count] = paperSource;
				this.array = array;
				return this.Count;
			}

			// Token: 0x04000C59 RID: 3161
			private PaperSource[] array;
		}

		// Token: 0x0200011C RID: 284
		public class PrinterResolutionCollection : ICollection, IEnumerable
		{
			// Token: 0x06000F14 RID: 3860 RVA: 0x0002D56D File Offset: 0x0002C56D
			public PrinterResolutionCollection(PrinterResolution[] array)
			{
				this.array = array;
			}

			// Token: 0x170003DA RID: 986
			// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0002D57C File Offset: 0x0002C57C
			public int Count
			{
				get
				{
					return this.array.Length;
				}
			}

			// Token: 0x170003DB RID: 987
			public virtual PrinterResolution this[int index]
			{
				get
				{
					return this.array[index];
				}
			}

			// Token: 0x06000F17 RID: 3863 RVA: 0x0002D590 File Offset: 0x0002C590
			public IEnumerator GetEnumerator()
			{
				return new PrinterSettings.ArrayEnumerator(this.array, 0, this.Count);
			}

			// Token: 0x170003DC RID: 988
			// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0002D5A4 File Offset: 0x0002C5A4
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x170003DD RID: 989
			// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0002D5AC File Offset: 0x0002C5AC
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003DE RID: 990
			// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0002D5AF File Offset: 0x0002C5AF
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x06000F1B RID: 3867 RVA: 0x0002D5B2 File Offset: 0x0002C5B2
			void ICollection.CopyTo(Array array, int index)
			{
				Array.Copy(this.array, index, array, 0, this.array.Length);
			}

			// Token: 0x06000F1C RID: 3868 RVA: 0x0002D5CA File Offset: 0x0002C5CA
			public void CopyTo(PrinterResolution[] printerResolutions, int index)
			{
				Array.Copy(this.array, index, printerResolutions, 0, this.array.Length);
			}

			// Token: 0x06000F1D RID: 3869 RVA: 0x0002D5E2 File Offset: 0x0002C5E2
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F1E RID: 3870 RVA: 0x0002D5EC File Offset: 0x0002C5EC
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(PrinterResolution printerResolution)
			{
				PrinterResolution[] array = new PrinterResolution[this.Count + 1];
				((ICollection)this).CopyTo(array, 0);
				array[this.Count] = printerResolution;
				this.array = array;
				return this.Count;
			}

			// Token: 0x04000C5A RID: 3162
			private PrinterResolution[] array;
		}

		// Token: 0x0200011D RID: 285
		public class StringCollection : ICollection, IEnumerable
		{
			// Token: 0x06000F1F RID: 3871 RVA: 0x0002D625 File Offset: 0x0002C625
			public StringCollection(string[] array)
			{
				this.array = array;
			}

			// Token: 0x170003DF RID: 991
			// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0002D634 File Offset: 0x0002C634
			public int Count
			{
				get
				{
					return this.array.Length;
				}
			}

			// Token: 0x170003E0 RID: 992
			public virtual string this[int index]
			{
				get
				{
					return this.array[index];
				}
			}

			// Token: 0x06000F22 RID: 3874 RVA: 0x0002D648 File Offset: 0x0002C648
			public IEnumerator GetEnumerator()
			{
				return new PrinterSettings.ArrayEnumerator(this.array, 0, this.Count);
			}

			// Token: 0x170003E1 RID: 993
			// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0002D65C File Offset: 0x0002C65C
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0002D664 File Offset: 0x0002C664
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0002D667 File Offset: 0x0002C667
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x06000F26 RID: 3878 RVA: 0x0002D66A File Offset: 0x0002C66A
			void ICollection.CopyTo(Array array, int index)
			{
				Array.Copy(this.array, index, array, 0, this.array.Length);
			}

			// Token: 0x06000F27 RID: 3879 RVA: 0x0002D682 File Offset: 0x0002C682
			public void CopyTo(string[] strings, int index)
			{
				Array.Copy(this.array, index, strings, 0, this.array.Length);
			}

			// Token: 0x06000F28 RID: 3880 RVA: 0x0002D69A File Offset: 0x0002C69A
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F29 RID: 3881 RVA: 0x0002D6A4 File Offset: 0x0002C6A4
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(string value)
			{
				string[] array = new string[this.Count + 1];
				((ICollection)this).CopyTo(array, 0);
				array[this.Count] = value;
				this.array = array;
				return this.Count;
			}

			// Token: 0x04000C5B RID: 3163
			private string[] array;
		}

		// Token: 0x0200011E RID: 286
		private class ArrayEnumerator : IEnumerator
		{
			// Token: 0x06000F2A RID: 3882 RVA: 0x0002D6DD File Offset: 0x0002C6DD
			public ArrayEnumerator(object[] array, int startIndex, int count)
			{
				this.array = array;
				this.startIndex = startIndex;
				this.endIndex = this.index + count;
				this.index = this.startIndex;
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0002D70D File Offset: 0x0002C70D
			public object Current
			{
				get
				{
					return this.item;
				}
			}

			// Token: 0x06000F2C RID: 3884 RVA: 0x0002D718 File Offset: 0x0002C718
			public bool MoveNext()
			{
				if (this.index >= this.endIndex)
				{
					return false;
				}
				this.item = this.array[this.index++];
				return true;
			}

			// Token: 0x06000F2D RID: 3885 RVA: 0x0002D754 File Offset: 0x0002C754
			public void Reset()
			{
				this.index = this.startIndex;
				this.item = null;
			}

			// Token: 0x04000C5C RID: 3164
			private object[] array;

			// Token: 0x04000C5D RID: 3165
			private object item;

			// Token: 0x04000C5E RID: 3166
			private int index;

			// Token: 0x04000C5F RID: 3167
			private int startIndex;

			// Token: 0x04000C60 RID: 3168
			private int endIndex;
		}
	}
}

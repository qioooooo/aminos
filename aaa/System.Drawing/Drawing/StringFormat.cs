using System;
using System.ComponentModel;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x020000E8 RID: 232
	public sealed class StringFormat : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x06000D2C RID: 3372 RVA: 0x000272F0 File Offset: 0x000262F0
		private StringFormat(IntPtr format)
		{
			this.nativeFormat = format;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000272FF File Offset: 0x000262FF
		public StringFormat()
			: this((StringFormatFlags)0, 0)
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00027309 File Offset: 0x00026309
		public StringFormat(StringFormatFlags options)
			: this(options, 0)
		{
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00027314 File Offset: 0x00026314
		public StringFormat(StringFormatFlags options, int language)
		{
			int num = SafeNativeMethods.Gdip.GdipCreateStringFormat(options, language, out this.nativeFormat);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00027340 File Offset: 0x00026340
		public StringFormat(StringFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			int num = SafeNativeMethods.Gdip.GdipCloneStringFormat(new HandleRef(format, format.nativeFormat), out this.nativeFormat);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00027383 File Offset: 0x00026383
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00027394 File Offset: 0x00026394
		private void Dispose(bool disposing)
		{
			if (this.nativeFormat != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteStringFormat(new HandleRef(this, this.nativeFormat));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeFormat = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00027400 File Offset: 0x00026400
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneStringFormat(new HandleRef(this, this.nativeFormat), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new StringFormat(zero);
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0002743C File Offset: 0x0002643C
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x00027468 File Offset: 0x00026468
		public StringFormatFlags FormatFlags
		{
			get
			{
				StringFormatFlags stringFormatFlags;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatFlags(new HandleRef(this, this.nativeFormat), out stringFormatFlags);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return stringFormatFlags;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatFlags(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00027494 File Offset: 0x00026494
		public void SetMeasurableCharacterRanges(CharacterRange[] ranges)
		{
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatMeasurableCharacterRanges(new HandleRef(this, this.nativeFormat), ranges.Length, ranges);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x000274C4 File Offset: 0x000264C4
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x000274F4 File Offset: 0x000264F4
		public StringAlignment Alignment
		{
			get
			{
				StringAlignment stringAlignment = StringAlignment.Near;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatAlign(new HandleRef(this, this.nativeFormat), out stringAlignment);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return stringAlignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringAlignment));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatAlign(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00027544 File Offset: 0x00026544
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x00027574 File Offset: 0x00026574
		public StringAlignment LineAlignment
		{
			get
			{
				StringAlignment stringAlignment = StringAlignment.Near;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatLineAlign(new HandleRef(this, this.nativeFormat), out stringAlignment);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return stringAlignment;
			}
			set
			{
				if (value < StringAlignment.Near || value > StringAlignment.Far)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringAlignment));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatLineAlign(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000275BC File Offset: 0x000265BC
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x000275E8 File Offset: 0x000265E8
		public HotkeyPrefix HotkeyPrefix
		{
			get
			{
				HotkeyPrefix hotkeyPrefix;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatHotkeyPrefix(new HandleRef(this, this.nativeFormat), out hotkeyPrefix);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return hotkeyPrefix;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HotkeyPrefix));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatHotkeyPrefix(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00027638 File Offset: 0x00026638
		public void SetTabStops(float firstTabOffset, float[] tabStops)
		{
			if (firstTabOffset < 0f)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "firstTabOffset", firstTabOffset }));
			}
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatTabStops(new HandleRef(this, this.nativeFormat), firstTabOffset, tabStops.Length, tabStops);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00027698 File Offset: 0x00026698
		public float[] GetTabStops(out float firstTabOffset)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetStringFormatTabStopCount(new HandleRef(this, this.nativeFormat), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			float[] array = new float[num];
			num2 = SafeNativeMethods.Gdip.GdipGetStringFormatTabStops(new HandleRef(this, this.nativeFormat), num, out firstTabOffset, array);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return array;
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x000276EC File Offset: 0x000266EC
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00027718 File Offset: 0x00026718
		public StringTrimming Trimming
		{
			get
			{
				StringTrimming stringTrimming;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatTrimming(new HandleRef(this, this.nativeFormat), out stringTrimming);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return stringTrimming;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringTrimming));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatTrimming(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00027768 File Offset: 0x00026768
		public static StringFormat GenericDefault
		{
			get
			{
				IntPtr intPtr;
				int num = SafeNativeMethods.Gdip.GdipStringFormatGetGenericDefault(out intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new StringFormat(intPtr);
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x00027790 File Offset: 0x00026790
		public static StringFormat GenericTypographic
		{
			get
			{
				IntPtr intPtr;
				int num = SafeNativeMethods.Gdip.GdipStringFormatGetGenericTypographic(out intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new StringFormat(intPtr);
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000277B8 File Offset: 0x000267B8
		public void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
		{
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), language, substitute);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x000277E4 File Offset: 0x000267E4
		public StringDigitSubstitute DigitSubstitutionMethod
		{
			get
			{
				int num = 0;
				StringDigitSubstitute stringDigitSubstitute;
				int num2 = SafeNativeMethods.Gdip.GdipGetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), out num, out stringDigitSubstitute);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return stringDigitSubstitute;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00027814 File Offset: 0x00026814
		public int DigitSubstitutionLanguage
		{
			get
			{
				int num = 0;
				StringDigitSubstitute stringDigitSubstitute;
				int num2 = SafeNativeMethods.Gdip.GdipGetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), out num, out stringDigitSubstitute);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return num;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00027844 File Offset: 0x00026844
		~StringFormat()
		{
			this.Dispose(false);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00027874 File Offset: 0x00026874
		public override string ToString()
		{
			return "[StringFormat, FormatFlags=" + this.FormatFlags.ToString() + "]";
		}

		// Token: 0x04000B23 RID: 2851
		internal IntPtr nativeFormat;
	}
}

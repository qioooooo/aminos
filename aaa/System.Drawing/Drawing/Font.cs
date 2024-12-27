using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Internal;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x0200008B RID: 139
	[ComVisible(true)]
	[Editor("System.Drawing.Design.FontEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter(typeof(FontConverter))]
	[Serializable]
	public sealed class Font : MarshalByRefObject, ICloneable, ISerializable, IDisposable
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x0001DA1C File Offset: 0x0001CA1C
		private void CreateNativeFont()
		{
			int num = SafeNativeMethods.Gdip.GdipCreateFont(new HandleRef(this, this.fontFamily.NativeFamily), this.fontSize, this.fontStyle, this.fontUnit, out this.nativeFont);
			if (num == 15)
			{
				throw new ArgumentException(SR.GetString("GdiplusFontStyleNotFound", new object[]
				{
					this.fontFamily.Name,
					this.fontStyle.ToString()
				}));
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001DAA0 File Offset: 0x0001CAA0
		private Font(SerializationInfo info, StreamingContext context)
		{
			string text = null;
			float num = -1f;
			FontStyle fontStyle = FontStyle.Regular;
			GraphicsUnit graphicsUnit = GraphicsUnit.Point;
			SingleConverter singleConverter = new SingleConverter();
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "Name", StringComparison.OrdinalIgnoreCase))
				{
					text = (string)enumerator.Value;
				}
				else if (string.Equals(enumerator.Name, "Size", StringComparison.OrdinalIgnoreCase))
				{
					if (enumerator.Value is string)
					{
						num = (float)singleConverter.ConvertFrom(enumerator.Value);
					}
					else
					{
						num = (float)enumerator.Value;
					}
				}
				else if (string.Compare(enumerator.Name, "Style", true, CultureInfo.InvariantCulture) == 0)
				{
					fontStyle = (FontStyle)enumerator.Value;
				}
				else if (string.Compare(enumerator.Name, "Unit", true, CultureInfo.InvariantCulture) == 0)
				{
					graphicsUnit = (GraphicsUnit)enumerator.Value;
				}
			}
			this.Initialize(text, num, fontStyle, graphicsUnit, 1, Font.IsVerticalName(text));
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001DBC0 File Offset: 0x0001CBC0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("Name", string.IsNullOrEmpty(this.OriginalFontName) ? this.Name : this.OriginalFontName);
			si.AddValue("Size", this.Size);
			si.AddValue("Style", this.Style);
			si.AddValue("Unit", this.Unit);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001DC30 File Offset: 0x0001CC30
		public Font(Font prototype, FontStyle newStyle)
		{
			this.originalFontName = prototype.OriginalFontName;
			this.Initialize(prototype.FontFamily, prototype.Size, newStyle, prototype.Unit, 1, false);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001DC7C File Offset: 0x0001CC7C
		public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
		{
			this.Initialize(family, emSize, style, unit, 1, false);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001DCA3 File Offset: 0x0001CCA3
		public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
		{
			this.Initialize(family, emSize, style, unit, gdiCharSet, false);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001DCCB File Offset: 0x0001CCCB
		public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			this.Initialize(family, emSize, style, unit, gdiCharSet, gdiVerticalFont);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001DCF4 File Offset: 0x0001CCF4
		public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
		{
			this.Initialize(familyName, emSize, style, unit, gdiCharSet, Font.IsVerticalName(familyName));
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001DD24 File Offset: 0x0001CD24
		public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			if (float.IsNaN(emSize) || float.IsInfinity(emSize) || emSize <= 0f)
			{
				throw new ArgumentException(SR.GetString("InvalidBoundArgument", new object[] { "emSize", emSize, 0, "System.Single.MaxValue" }), "emSize");
			}
			this.Initialize(familyName, emSize, style, unit, gdiCharSet, gdiVerticalFont);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001DDAF File Offset: 0x0001CDAF
		public Font(FontFamily family, float emSize, FontStyle style)
		{
			this.Initialize(family, emSize, style, GraphicsUnit.Point, 1, false);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001DDD5 File Offset: 0x0001CDD5
		public Font(FontFamily family, float emSize, GraphicsUnit unit)
		{
			this.Initialize(family, emSize, FontStyle.Regular, unit, 1, false);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001DDFB File Offset: 0x0001CDFB
		public Font(FontFamily family, float emSize)
		{
			this.Initialize(family, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001DE21 File Offset: 0x0001CE21
		public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit)
		{
			this.Initialize(familyName, emSize, style, unit, 1, Font.IsVerticalName(familyName));
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001DE4D File Offset: 0x0001CE4D
		public Font(string familyName, float emSize, FontStyle style)
		{
			this.Initialize(familyName, emSize, style, GraphicsUnit.Point, 1, Font.IsVerticalName(familyName));
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001DE78 File Offset: 0x0001CE78
		public Font(string familyName, float emSize, GraphicsUnit unit)
		{
			this.Initialize(familyName, emSize, FontStyle.Regular, unit, 1, Font.IsVerticalName(familyName));
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001DEA3 File Offset: 0x0001CEA3
		public Font(string familyName, float emSize)
		{
			this.Initialize(familyName, emSize, FontStyle.Regular, GraphicsUnit.Point, 1, Font.IsVerticalName(familyName));
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001DED0 File Offset: 0x0001CED0
		private Font(IntPtr nativeFont, byte gdiCharSet, bool gdiVerticalFont)
		{
			float num = 0f;
			GraphicsUnit graphicsUnit = GraphicsUnit.Point;
			FontStyle fontStyle = FontStyle.Regular;
			IntPtr zero = IntPtr.Zero;
			this.nativeFont = nativeFont;
			int num2 = SafeNativeMethods.Gdip.GdipGetFontUnit(new HandleRef(this, nativeFont), out graphicsUnit);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			num2 = SafeNativeMethods.Gdip.GdipGetFontSize(new HandleRef(this, nativeFont), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			num2 = SafeNativeMethods.Gdip.GdipGetFontStyle(new HandleRef(this, nativeFont), out fontStyle);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			num2 = SafeNativeMethods.Gdip.GdipGetFamily(new HandleRef(this, nativeFont), out zero);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			this.SetFontFamily(new FontFamily(zero));
			this.Initialize(this.fontFamily, num, fontStyle, graphicsUnit, gdiCharSet, gdiVerticalFont);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001DF91 File Offset: 0x0001CF91
		private void Initialize(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			this.originalFontName = familyName;
			this.SetFontFamily(new FontFamily(Font.StripVerticalName(familyName), true));
			this.Initialize(this.fontFamily, emSize, style, unit, gdiCharSet, gdiVerticalFont);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001DFC0 File Offset: 0x0001CFC0
		private void Initialize(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		{
			if (family == null)
			{
				throw new ArgumentNullException("family");
			}
			if (float.IsNaN(emSize) || float.IsInfinity(emSize) || emSize <= 0f)
			{
				throw new ArgumentException(SR.GetString("InvalidBoundArgument", new object[] { "emSize", emSize, 0, "System.Single.MaxValue" }), "emSize");
			}
			this.fontSize = emSize;
			this.fontStyle = style;
			this.fontUnit = unit;
			this.gdiCharSet = gdiCharSet;
			this.gdiVerticalFont = gdiVerticalFont;
			if (this.fontFamily == null)
			{
				this.SetFontFamily(new FontFamily(family.NativeFamily));
			}
			if (this.nativeFont == IntPtr.Zero)
			{
				this.CreateNativeFont();
			}
			int num = SafeNativeMethods.Gdip.GdipGetFontSize(new HandleRef(this, this.nativeFont), out this.fontSize);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001E0AC File Offset: 0x0001D0AC
		public static Font FromHfont(IntPtr hfont)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			SafeNativeMethods.LOGFONT logfont = new SafeNativeMethods.LOGFONT();
			SafeNativeMethods.GetObject(new HandleRef(null, hfont), logfont);
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			Font font;
			try
			{
				font = Font.FromLogFont(logfont, dc);
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
			return font;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001E110 File Offset: 0x0001D110
		public static Font FromLogFont(object lf)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			Font font;
			try
			{
				font = Font.FromLogFont(lf, dc);
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
			return font;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001E158 File Offset: 0x0001D158
		public static Font FromLogFont(object lf, IntPtr hdc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				num = SafeNativeMethods.Gdip.GdipCreateFontFromLogfontA(new HandleRef(null, hdc), lf, out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipCreateFontFromLogfontW(new HandleRef(null, hdc), lf, out zero);
			}
			if (num == 16)
			{
				throw new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont_NoName"));
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (zero == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont", new object[] { lf.ToString() }));
			}
			bool flag;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				flag = Marshal.ReadByte(lf, 28) == 64;
			}
			else
			{
				flag = Marshal.ReadInt16(lf, 28) == 64;
			}
			return new Font(zero, Marshal.ReadByte(lf, 23), flag);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001E220 File Offset: 0x0001D220
		public static Font FromHdc(IntPtr hdc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFontFromDC(new HandleRef(null, hdc), ref zero);
			if (num == 16)
			{
				throw new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont_NoName"));
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Font(zero, 0, false);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001E274 File Offset: 0x0001D274
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneFont(new HandleRef(this, this.nativeFont), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Font(zero, this.gdiCharSet, this.gdiVerticalFont);
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0001E2B9 File Offset: 0x0001D2B9
		internal IntPtr NativeFont
		{
			get
			{
				return this.nativeFont;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0001E2C1 File Offset: 0x0001D2C1
		[Browsable(false)]
		public FontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001E2C9 File Offset: 0x0001D2C9
		private void SetFontFamily(FontFamily family)
		{
			this.fontFamily = family;
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			GC.SuppressFinalize(this.fontFamily);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001E2E8 File Offset: 0x0001D2E8
		~Font()
		{
			this.Dispose(false);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001E318 File Offset: 0x0001D318
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001E328 File Offset: 0x0001D328
		private void Dispose(bool disposing)
		{
			if (this.nativeFont != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteFont(new HandleRef(this, this.nativeFont));
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
					this.nativeFont = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001E394 File Offset: 0x0001D394
		private static bool IsVerticalName(string familyName)
		{
			return familyName != null && familyName.Length > 0 && familyName[0] == '@';
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0001E3AF File Offset: 0x0001D3AF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Bold
		{
			get
			{
				return (this.Style & FontStyle.Bold) != FontStyle.Regular;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001E3BF File Offset: 0x0001D3BF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public byte GdiCharSet
		{
			get
			{
				return this.gdiCharSet;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001E3C7 File Offset: 0x0001D3C7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool GdiVerticalFont
		{
			get
			{
				return this.gdiVerticalFont;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001E3CF File Offset: 0x0001D3CF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Italic
		{
			get
			{
				return (this.Style & FontStyle.Italic) != FontStyle.Regular;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0001E3DF File Offset: 0x0001D3DF
		[TypeConverter(typeof(FontConverter.FontNameConverter))]
		[Editor("System.Drawing.Design.FontNameEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Name
		{
			get
			{
				return this.FontFamily.Name;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001E3EC File Offset: 0x0001D3EC
		[Browsable(false)]
		public string OriginalFontName
		{
			get
			{
				return this.originalFontName;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0001E3F4 File Offset: 0x0001D3F4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Strikeout
		{
			get
			{
				return (this.Style & FontStyle.Strikeout) != FontStyle.Regular;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001E404 File Offset: 0x0001D404
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Underline
		{
			get
			{
				return (this.Style & FontStyle.Underline) != FontStyle.Regular;
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001E414 File Offset: 0x0001D414
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			Font font = obj as Font;
			return font != null && (font.FontFamily.Equals(this.FontFamily) && font.GdiVerticalFont == this.GdiVerticalFont && font.GdiCharSet == this.GdiCharSet && font.Style == this.Style && font.Size == this.Size) && font.Unit == this.Unit;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001E48E File Offset: 0x0001D48E
		public override int GetHashCode()
		{
			return (int)((((uint)this.fontStyle << 13) | (this.fontStyle >> 19)) ^ (FontStyle)(((uint)this.fontUnit << 26) | (this.fontUnit >> 6)) ^ (FontStyle)(((uint)this.fontSize << 7) | ((uint)this.fontSize >> 25)));
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001E4CB File Offset: 0x0001D4CB
		private static string StripVerticalName(string familyName)
		{
			if (familyName != null && familyName.Length > 1 && familyName[0] == '@')
			{
				return familyName.Substring(1);
			}
			return familyName;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001E4F0 File Offset: 0x0001D4F0
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "[{0}: Name={1}, Size={2}, Units={3}, GdiCharSet={4}, GdiVerticalFont={5}]", new object[]
			{
				base.GetType().Name,
				this.FontFamily.Name,
				this.fontSize,
				(int)this.fontUnit,
				this.gdiCharSet,
				this.gdiVerticalFont
			});
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001E568 File Offset: 0x0001D568
		public void ToLogFont(object logFont)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(dc);
				try
				{
					this.ToLogFont(logFont, graphics);
				}
				finally
				{
					graphics.Dispose();
				}
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001E5CC File Offset: 0x0001D5CC
		public void ToLogFont(object logFont, Graphics graphics)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			int num;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				num = SafeNativeMethods.Gdip.GdipGetLogFontA(new HandleRef(this, this.NativeFont), new HandleRef(graphics, graphics.NativeGraphics), logFont);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipGetLogFontW(new HandleRef(this, this.NativeFont), new HandleRef(graphics, graphics.NativeGraphics), logFont);
			}
			if (this.gdiVerticalFont)
			{
				if (Marshal.SystemDefaultCharSize == 1)
				{
					for (int i = 30; i >= 0; i--)
					{
						Marshal.WriteByte(logFont, 28 + i + 1, Marshal.ReadByte(logFont, 28 + i));
					}
					Marshal.WriteByte(logFont, 28, 64);
				}
				else
				{
					for (int j = 60; j >= 0; j -= 2)
					{
						Marshal.WriteInt16(logFont, 28 + j + 2, Marshal.ReadInt16(logFont, 28 + j));
					}
					Marshal.WriteInt16(logFont, 28, 64);
				}
			}
			if (Marshal.ReadByte(logFont, 23) == 0)
			{
				Marshal.WriteByte(logFont, 23, this.gdiCharSet);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001E6C8 File Offset: 0x0001D6C8
		public IntPtr ToHfont()
		{
			SafeNativeMethods.LOGFONT logfont = new SafeNativeMethods.LOGFONT();
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				this.ToLogFont(logfont);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			IntPtr intPtr = IntUnsafeNativeMethods.IntCreateFontIndirect(logfont);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001E720 File Offset: 0x0001D720
		public float GetHeight(Graphics graphics)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetFontHeight(new HandleRef(this, this.NativeFont), new HandleRef(graphics, graphics.NativeGraphics), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001E768 File Offset: 0x0001D768
		public float GetHeight()
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			float num = 0f;
			try
			{
				using (Graphics graphics = Graphics.FromHdcInternal(dc))
				{
					num = this.GetHeight(graphics);
				}
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
			return num;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001E7D4 File Offset: 0x0001D7D4
		public float GetHeight(float dpi)
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetFontHeightGivenDPI(new HandleRef(this, this.NativeFont), dpi, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001E801 File Offset: 0x0001D801
		[Browsable(false)]
		public FontStyle Style
		{
			get
			{
				return this.fontStyle;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001E809 File Offset: 0x0001D809
		public float Size
		{
			get
			{
				return this.fontSize;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0001E814 File Offset: 0x0001D814
		[Browsable(false)]
		public float SizeInPoints
		{
			get
			{
				if (this.Unit == GraphicsUnit.Point)
				{
					return this.Size;
				}
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				float num3;
				try
				{
					using (Graphics graphics = Graphics.FromHdcInternal(dc))
					{
						float num = (float)((double)graphics.DpiY / 72.0);
						float height = this.GetHeight(graphics);
						float num2 = height * (float)this.FontFamily.GetEmHeight(this.Style) / (float)this.FontFamily.GetLineSpacing(this.Style);
						num3 = num2 / num;
					}
				}
				finally
				{
					UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
				}
				return num3;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001E8CC File Offset: 0x0001D8CC
		[TypeConverter(typeof(FontConverter.FontUnitConverter))]
		public GraphicsUnit Unit
		{
			get
			{
				return this.fontUnit;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001E8D4 File Offset: 0x0001D8D4
		[Browsable(false)]
		public int Height
		{
			get
			{
				return (int)Math.Ceiling((double)this.GetHeight());
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0001E8E3 File Offset: 0x0001D8E3
		[Browsable(false)]
		public bool IsSystemFont
		{
			get
			{
				return !string.IsNullOrEmpty(this.systemFontName);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0001E8F3 File Offset: 0x0001D8F3
		[Browsable(false)]
		public string SystemFontName
		{
			get
			{
				return this.systemFontName;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001E8FB File Offset: 0x0001D8FB
		internal void SetSystemFontName(string systemFontName)
		{
			this.systemFontName = systemFontName;
		}

		// Token: 0x0400063B RID: 1595
		private const int LogFontCharSetOffset = 23;

		// Token: 0x0400063C RID: 1596
		private const int LogFontNameOffset = 28;

		// Token: 0x0400063D RID: 1597
		private IntPtr nativeFont;

		// Token: 0x0400063E RID: 1598
		private float fontSize;

		// Token: 0x0400063F RID: 1599
		private FontStyle fontStyle;

		// Token: 0x04000640 RID: 1600
		private FontFamily fontFamily;

		// Token: 0x04000641 RID: 1601
		private GraphicsUnit fontUnit;

		// Token: 0x04000642 RID: 1602
		private byte gdiCharSet = 1;

		// Token: 0x04000643 RID: 1603
		private bool gdiVerticalFont;

		// Token: 0x04000644 RID: 1604
		private string systemFontName = "";

		// Token: 0x04000645 RID: 1605
		private string originalFontName;
	}
}

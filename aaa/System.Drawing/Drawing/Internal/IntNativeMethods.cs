using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x02000023 RID: 35
	internal class IntNativeMethods
	{
		// Token: 0x0400011C RID: 284
		public const int MaxTextLengthInWin9x = 8192;

		// Token: 0x0400011D RID: 285
		public const int DT_TOP = 0;

		// Token: 0x0400011E RID: 286
		public const int DT_LEFT = 0;

		// Token: 0x0400011F RID: 287
		public const int DT_CENTER = 1;

		// Token: 0x04000120 RID: 288
		public const int DT_RIGHT = 2;

		// Token: 0x04000121 RID: 289
		public const int DT_VCENTER = 4;

		// Token: 0x04000122 RID: 290
		public const int DT_BOTTOM = 8;

		// Token: 0x04000123 RID: 291
		public const int DT_WORDBREAK = 16;

		// Token: 0x04000124 RID: 292
		public const int DT_SINGLELINE = 32;

		// Token: 0x04000125 RID: 293
		public const int DT_EXPANDTABS = 64;

		// Token: 0x04000126 RID: 294
		public const int DT_TABSTOP = 128;

		// Token: 0x04000127 RID: 295
		public const int DT_NOCLIP = 256;

		// Token: 0x04000128 RID: 296
		public const int DT_EXTERNALLEADING = 512;

		// Token: 0x04000129 RID: 297
		public const int DT_CALCRECT = 1024;

		// Token: 0x0400012A RID: 298
		public const int DT_NOPREFIX = 2048;

		// Token: 0x0400012B RID: 299
		public const int DT_INTERNAL = 4096;

		// Token: 0x0400012C RID: 300
		public const int DT_EDITCONTROL = 8192;

		// Token: 0x0400012D RID: 301
		public const int DT_PATH_ELLIPSIS = 16384;

		// Token: 0x0400012E RID: 302
		public const int DT_END_ELLIPSIS = 32768;

		// Token: 0x0400012F RID: 303
		public const int DT_MODIFYSTRING = 65536;

		// Token: 0x04000130 RID: 304
		public const int DT_RTLREADING = 131072;

		// Token: 0x04000131 RID: 305
		public const int DT_WORD_ELLIPSIS = 262144;

		// Token: 0x04000132 RID: 306
		public const int DT_NOFULLWIDTHCHARBREAK = 524288;

		// Token: 0x04000133 RID: 307
		public const int DT_HIDEPREFIX = 1048576;

		// Token: 0x04000134 RID: 308
		public const int DT_PREFIXONLY = 2097152;

		// Token: 0x04000135 RID: 309
		public const int DIB_RGB_COLORS = 0;

		// Token: 0x04000136 RID: 310
		public const int BI_BITFIELDS = 3;

		// Token: 0x04000137 RID: 311
		public const int BI_RGB = 0;

		// Token: 0x04000138 RID: 312
		public const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04000139 RID: 313
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x0400013A RID: 314
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x0400013B RID: 315
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x0400013C RID: 316
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x0400013D RID: 317
		public const int BITSPIXEL = 12;

		// Token: 0x0400013E RID: 318
		public const int ALTERNATE = 1;

		// Token: 0x0400013F RID: 319
		public const int WINDING = 2;

		// Token: 0x04000140 RID: 320
		public const int SRCCOPY = 13369376;

		// Token: 0x04000141 RID: 321
		public const int SRCPAINT = 15597702;

		// Token: 0x04000142 RID: 322
		public const int SRCAND = 8913094;

		// Token: 0x04000143 RID: 323
		public const int SRCINVERT = 6684742;

		// Token: 0x04000144 RID: 324
		public const int SRCERASE = 4457256;

		// Token: 0x04000145 RID: 325
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x04000146 RID: 326
		public const int NOTSRCERASE = 1114278;

		// Token: 0x04000147 RID: 327
		public const int MERGECOPY = 12583114;

		// Token: 0x04000148 RID: 328
		public const int MERGEPAINT = 12255782;

		// Token: 0x04000149 RID: 329
		public const int PATCOPY = 15728673;

		// Token: 0x0400014A RID: 330
		public const int PATPAINT = 16452105;

		// Token: 0x0400014B RID: 331
		public const int PATINVERT = 5898313;

		// Token: 0x0400014C RID: 332
		public const int DSTINVERT = 5570569;

		// Token: 0x0400014D RID: 333
		public const int BLACKNESS = 66;

		// Token: 0x0400014E RID: 334
		public const int WHITENESS = 16711778;

		// Token: 0x0400014F RID: 335
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x04000150 RID: 336
		public const int FW_DONTCARE = 0;

		// Token: 0x04000151 RID: 337
		public const int FW_NORMAL = 400;

		// Token: 0x04000152 RID: 338
		public const int FW_BOLD = 700;

		// Token: 0x04000153 RID: 339
		public const int ANSI_CHARSET = 0;

		// Token: 0x04000154 RID: 340
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x04000155 RID: 341
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x04000156 RID: 342
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x04000157 RID: 343
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x04000158 RID: 344
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x04000159 RID: 345
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x0400015A RID: 346
		public const int DRAFT_QUALITY = 1;

		// Token: 0x0400015B RID: 347
		public const int PROOF_QUALITY = 2;

		// Token: 0x0400015C RID: 348
		public const int NONANTIALIASED_QUALITY = 3;

		// Token: 0x0400015D RID: 349
		public const int ANTIALIASED_QUALITY = 4;

		// Token: 0x0400015E RID: 350
		public const int CLEARTYPE_QUALITY = 5;

		// Token: 0x0400015F RID: 351
		public const int CLEARTYPE_NATURAL_QUALITY = 6;

		// Token: 0x04000160 RID: 352
		public const int OBJ_PEN = 1;

		// Token: 0x04000161 RID: 353
		public const int OBJ_BRUSH = 2;

		// Token: 0x04000162 RID: 354
		public const int OBJ_DC = 3;

		// Token: 0x04000163 RID: 355
		public const int OBJ_METADC = 4;

		// Token: 0x04000164 RID: 356
		public const int OBJ_FONT = 6;

		// Token: 0x04000165 RID: 357
		public const int OBJ_BITMAP = 7;

		// Token: 0x04000166 RID: 358
		public const int OBJ_MEMDC = 10;

		// Token: 0x04000167 RID: 359
		public const int OBJ_EXTPEN = 11;

		// Token: 0x04000168 RID: 360
		public const int OBJ_ENHMETADC = 12;

		// Token: 0x04000169 RID: 361
		public const int BS_SOLID = 0;

		// Token: 0x0400016A RID: 362
		public const int BS_HATCHED = 2;

		// Token: 0x0400016B RID: 363
		public const int CP_ACP = 0;

		// Token: 0x0400016C RID: 364
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x0400016D RID: 365
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x0400016E RID: 366
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x0400016F RID: 367
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x02000024 RID: 36
		public enum RegionFlags
		{
			// Token: 0x04000171 RID: 369
			ERROR,
			// Token: 0x04000172 RID: 370
			NULLREGION,
			// Token: 0x04000173 RID: 371
			SIMPLEREGION,
			// Token: 0x04000174 RID: 372
			COMPLEXREGION
		}

		// Token: 0x02000025 RID: 37
		public struct RECT
		{
			// Token: 0x060000B9 RID: 185 RVA: 0x000043AA File Offset: 0x000033AA
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x060000BA RID: 186 RVA: 0x000043C9 File Offset: 0x000033C9
			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x060000BB RID: 187 RVA: 0x000043FF File Offset: 0x000033FF
			public static IntNativeMethods.RECT FromXYWH(int x, int y, int width, int height)
			{
				return new IntNativeMethods.RECT(x, y, x + width, y + height);
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060000BC RID: 188 RVA: 0x0000440E File Offset: 0x0000340E
			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			// Token: 0x060000BD RID: 189 RVA: 0x0000442F File Offset: 0x0000342F
			public Rectangle ToRectangle()
			{
				return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
			}

			// Token: 0x04000175 RID: 373
			public int left;

			// Token: 0x04000176 RID: 374
			public int top;

			// Token: 0x04000177 RID: 375
			public int right;

			// Token: 0x04000178 RID: 376
			public int bottom;
		}

		// Token: 0x02000026 RID: 38
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x060000BE RID: 190 RVA: 0x0000445C File Offset: 0x0000345C
			public POINT()
			{
			}

			// Token: 0x060000BF RID: 191 RVA: 0x00004464 File Offset: 0x00003464
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x0000447A File Offset: 0x0000347A
			public Point ToPoint()
			{
				return new Point(this.x, this.y);
			}

			// Token: 0x04000179 RID: 377
			public int x;

			// Token: 0x0400017A RID: 378
			public int y;
		}

		// Token: 0x02000027 RID: 39
		[StructLayout(LayoutKind.Sequential)]
		public class DRAWTEXTPARAMS
		{
			// Token: 0x060000C1 RID: 193 RVA: 0x0000448D File Offset: 0x0000348D
			public DRAWTEXTPARAMS()
			{
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x000044AC File Offset: 0x000034AC
			public DRAWTEXTPARAMS(IntNativeMethods.DRAWTEXTPARAMS original)
			{
				this.iLeftMargin = original.iLeftMargin;
				this.iRightMargin = original.iRightMargin;
				this.iTabLength = original.iTabLength;
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x000044F8 File Offset: 0x000034F8
			public DRAWTEXTPARAMS(int leftMargin, int rightMargin)
			{
				this.iLeftMargin = leftMargin;
				this.iRightMargin = rightMargin;
			}

			// Token: 0x0400017B RID: 379
			private int cbSize = Marshal.SizeOf(typeof(IntNativeMethods.DRAWTEXTPARAMS));

			// Token: 0x0400017C RID: 380
			public int iTabLength;

			// Token: 0x0400017D RID: 381
			public int iLeftMargin;

			// Token: 0x0400017E RID: 382
			public int iRightMargin;

			// Token: 0x0400017F RID: 383
			public int uiLengthDrawn;
		}

		// Token: 0x02000028 RID: 40
		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			// Token: 0x04000180 RID: 384
			public int lbStyle;

			// Token: 0x04000181 RID: 385
			public int lbColor;

			// Token: 0x04000182 RID: 386
			public int lbHatch;
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x060000C5 RID: 197 RVA: 0x0000452B File Offset: 0x0000352B
			public LOGFONT()
			{
			}

			// Token: 0x060000C6 RID: 198 RVA: 0x00004534 File Offset: 0x00003534
			public LOGFONT(IntNativeMethods.LOGFONT lf)
			{
				this.lfHeight = lf.lfHeight;
				this.lfWidth = lf.lfWidth;
				this.lfEscapement = lf.lfEscapement;
				this.lfOrientation = lf.lfOrientation;
				this.lfWeight = lf.lfWeight;
				this.lfItalic = lf.lfItalic;
				this.lfUnderline = lf.lfUnderline;
				this.lfStrikeOut = lf.lfStrikeOut;
				this.lfCharSet = lf.lfCharSet;
				this.lfOutPrecision = lf.lfOutPrecision;
				this.lfClipPrecision = lf.lfClipPrecision;
				this.lfQuality = lf.lfQuality;
				this.lfPitchAndFamily = lf.lfPitchAndFamily;
				this.lfFaceName = lf.lfFaceName;
			}

			// Token: 0x04000183 RID: 387
			public int lfHeight;

			// Token: 0x04000184 RID: 388
			public int lfWidth;

			// Token: 0x04000185 RID: 389
			public int lfEscapement;

			// Token: 0x04000186 RID: 390
			public int lfOrientation;

			// Token: 0x04000187 RID: 391
			public int lfWeight;

			// Token: 0x04000188 RID: 392
			public byte lfItalic;

			// Token: 0x04000189 RID: 393
			public byte lfUnderline;

			// Token: 0x0400018A RID: 394
			public byte lfStrikeOut;

			// Token: 0x0400018B RID: 395
			public byte lfCharSet;

			// Token: 0x0400018C RID: 396
			public byte lfOutPrecision;

			// Token: 0x0400018D RID: 397
			public byte lfClipPrecision;

			// Token: 0x0400018E RID: 398
			public byte lfQuality;

			// Token: 0x0400018F RID: 399
			public byte lfPitchAndFamily;

			// Token: 0x04000190 RID: 400
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x0200002A RID: 42
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04000191 RID: 401
			public int tmHeight;

			// Token: 0x04000192 RID: 402
			public int tmAscent;

			// Token: 0x04000193 RID: 403
			public int tmDescent;

			// Token: 0x04000194 RID: 404
			public int tmInternalLeading;

			// Token: 0x04000195 RID: 405
			public int tmExternalLeading;

			// Token: 0x04000196 RID: 406
			public int tmAveCharWidth;

			// Token: 0x04000197 RID: 407
			public int tmMaxCharWidth;

			// Token: 0x04000198 RID: 408
			public int tmWeight;

			// Token: 0x04000199 RID: 409
			public int tmOverhang;

			// Token: 0x0400019A RID: 410
			public int tmDigitizedAspectX;

			// Token: 0x0400019B RID: 411
			public int tmDigitizedAspectY;

			// Token: 0x0400019C RID: 412
			public char tmFirstChar;

			// Token: 0x0400019D RID: 413
			public char tmLastChar;

			// Token: 0x0400019E RID: 414
			public char tmDefaultChar;

			// Token: 0x0400019F RID: 415
			public char tmBreakChar;

			// Token: 0x040001A0 RID: 416
			public byte tmItalic;

			// Token: 0x040001A1 RID: 417
			public byte tmUnderlined;

			// Token: 0x040001A2 RID: 418
			public byte tmStruckOut;

			// Token: 0x040001A3 RID: 419
			public byte tmPitchAndFamily;

			// Token: 0x040001A4 RID: 420
			public byte tmCharSet;
		}

		// Token: 0x0200002B RID: 43
		public struct TEXTMETRICA
		{
			// Token: 0x040001A5 RID: 421
			public int tmHeight;

			// Token: 0x040001A6 RID: 422
			public int tmAscent;

			// Token: 0x040001A7 RID: 423
			public int tmDescent;

			// Token: 0x040001A8 RID: 424
			public int tmInternalLeading;

			// Token: 0x040001A9 RID: 425
			public int tmExternalLeading;

			// Token: 0x040001AA RID: 426
			public int tmAveCharWidth;

			// Token: 0x040001AB RID: 427
			public int tmMaxCharWidth;

			// Token: 0x040001AC RID: 428
			public int tmWeight;

			// Token: 0x040001AD RID: 429
			public int tmOverhang;

			// Token: 0x040001AE RID: 430
			public int tmDigitizedAspectX;

			// Token: 0x040001AF RID: 431
			public int tmDigitizedAspectY;

			// Token: 0x040001B0 RID: 432
			public byte tmFirstChar;

			// Token: 0x040001B1 RID: 433
			public byte tmLastChar;

			// Token: 0x040001B2 RID: 434
			public byte tmDefaultChar;

			// Token: 0x040001B3 RID: 435
			public byte tmBreakChar;

			// Token: 0x040001B4 RID: 436
			public byte tmItalic;

			// Token: 0x040001B5 RID: 437
			public byte tmUnderlined;

			// Token: 0x040001B6 RID: 438
			public byte tmStruckOut;

			// Token: 0x040001B7 RID: 439
			public byte tmPitchAndFamily;

			// Token: 0x040001B8 RID: 440
			public byte tmCharSet;
		}

		// Token: 0x0200002C RID: 44
		[StructLayout(LayoutKind.Sequential)]
		public class SIZE
		{
			// Token: 0x060000C7 RID: 199 RVA: 0x000045EF File Offset: 0x000035EF
			public SIZE()
			{
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x000045F7 File Offset: 0x000035F7
			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x0000460D File Offset: 0x0000360D
			public Size ToSize()
			{
				return new Size(this.cx, this.cy);
			}

			// Token: 0x040001B9 RID: 441
			public int cx;

			// Token: 0x040001BA RID: 442
			public int cy;
		}
	}
}

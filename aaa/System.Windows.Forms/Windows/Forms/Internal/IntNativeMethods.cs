using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000015 RID: 21
	internal class IntNativeMethods
	{
		// Token: 0x040009F5 RID: 2549
		public const int MaxTextLengthInWin9x = 8192;

		// Token: 0x040009F6 RID: 2550
		public const int DT_TOP = 0;

		// Token: 0x040009F7 RID: 2551
		public const int DT_LEFT = 0;

		// Token: 0x040009F8 RID: 2552
		public const int DT_CENTER = 1;

		// Token: 0x040009F9 RID: 2553
		public const int DT_RIGHT = 2;

		// Token: 0x040009FA RID: 2554
		public const int DT_VCENTER = 4;

		// Token: 0x040009FB RID: 2555
		public const int DT_BOTTOM = 8;

		// Token: 0x040009FC RID: 2556
		public const int DT_WORDBREAK = 16;

		// Token: 0x040009FD RID: 2557
		public const int DT_SINGLELINE = 32;

		// Token: 0x040009FE RID: 2558
		public const int DT_EXPANDTABS = 64;

		// Token: 0x040009FF RID: 2559
		public const int DT_TABSTOP = 128;

		// Token: 0x04000A00 RID: 2560
		public const int DT_NOCLIP = 256;

		// Token: 0x04000A01 RID: 2561
		public const int DT_EXTERNALLEADING = 512;

		// Token: 0x04000A02 RID: 2562
		public const int DT_CALCRECT = 1024;

		// Token: 0x04000A03 RID: 2563
		public const int DT_NOPREFIX = 2048;

		// Token: 0x04000A04 RID: 2564
		public const int DT_INTERNAL = 4096;

		// Token: 0x04000A05 RID: 2565
		public const int DT_EDITCONTROL = 8192;

		// Token: 0x04000A06 RID: 2566
		public const int DT_PATH_ELLIPSIS = 16384;

		// Token: 0x04000A07 RID: 2567
		public const int DT_END_ELLIPSIS = 32768;

		// Token: 0x04000A08 RID: 2568
		public const int DT_MODIFYSTRING = 65536;

		// Token: 0x04000A09 RID: 2569
		public const int DT_RTLREADING = 131072;

		// Token: 0x04000A0A RID: 2570
		public const int DT_WORD_ELLIPSIS = 262144;

		// Token: 0x04000A0B RID: 2571
		public const int DT_NOFULLWIDTHCHARBREAK = 524288;

		// Token: 0x04000A0C RID: 2572
		public const int DT_HIDEPREFIX = 1048576;

		// Token: 0x04000A0D RID: 2573
		public const int DT_PREFIXONLY = 2097152;

		// Token: 0x04000A0E RID: 2574
		public const int DIB_RGB_COLORS = 0;

		// Token: 0x04000A0F RID: 2575
		public const int BI_BITFIELDS = 3;

		// Token: 0x04000A10 RID: 2576
		public const int BI_RGB = 0;

		// Token: 0x04000A11 RID: 2577
		public const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04000A12 RID: 2578
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x04000A13 RID: 2579
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x04000A14 RID: 2580
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x04000A15 RID: 2581
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x04000A16 RID: 2582
		public const int BITSPIXEL = 12;

		// Token: 0x04000A17 RID: 2583
		public const int ALTERNATE = 1;

		// Token: 0x04000A18 RID: 2584
		public const int WINDING = 2;

		// Token: 0x04000A19 RID: 2585
		public const int SRCCOPY = 13369376;

		// Token: 0x04000A1A RID: 2586
		public const int SRCPAINT = 15597702;

		// Token: 0x04000A1B RID: 2587
		public const int SRCAND = 8913094;

		// Token: 0x04000A1C RID: 2588
		public const int SRCINVERT = 6684742;

		// Token: 0x04000A1D RID: 2589
		public const int SRCERASE = 4457256;

		// Token: 0x04000A1E RID: 2590
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x04000A1F RID: 2591
		public const int NOTSRCERASE = 1114278;

		// Token: 0x04000A20 RID: 2592
		public const int MERGECOPY = 12583114;

		// Token: 0x04000A21 RID: 2593
		public const int MERGEPAINT = 12255782;

		// Token: 0x04000A22 RID: 2594
		public const int PATCOPY = 15728673;

		// Token: 0x04000A23 RID: 2595
		public const int PATPAINT = 16452105;

		// Token: 0x04000A24 RID: 2596
		public const int PATINVERT = 5898313;

		// Token: 0x04000A25 RID: 2597
		public const int DSTINVERT = 5570569;

		// Token: 0x04000A26 RID: 2598
		public const int BLACKNESS = 66;

		// Token: 0x04000A27 RID: 2599
		public const int WHITENESS = 16711778;

		// Token: 0x04000A28 RID: 2600
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x04000A29 RID: 2601
		public const int FW_DONTCARE = 0;

		// Token: 0x04000A2A RID: 2602
		public const int FW_NORMAL = 400;

		// Token: 0x04000A2B RID: 2603
		public const int FW_BOLD = 700;

		// Token: 0x04000A2C RID: 2604
		public const int ANSI_CHARSET = 0;

		// Token: 0x04000A2D RID: 2605
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x04000A2E RID: 2606
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x04000A2F RID: 2607
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x04000A30 RID: 2608
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x04000A31 RID: 2609
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x04000A32 RID: 2610
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x04000A33 RID: 2611
		public const int DRAFT_QUALITY = 1;

		// Token: 0x04000A34 RID: 2612
		public const int PROOF_QUALITY = 2;

		// Token: 0x04000A35 RID: 2613
		public const int NONANTIALIASED_QUALITY = 3;

		// Token: 0x04000A36 RID: 2614
		public const int ANTIALIASED_QUALITY = 4;

		// Token: 0x04000A37 RID: 2615
		public const int CLEARTYPE_QUALITY = 5;

		// Token: 0x04000A38 RID: 2616
		public const int CLEARTYPE_NATURAL_QUALITY = 6;

		// Token: 0x04000A39 RID: 2617
		public const int OBJ_PEN = 1;

		// Token: 0x04000A3A RID: 2618
		public const int OBJ_BRUSH = 2;

		// Token: 0x04000A3B RID: 2619
		public const int OBJ_DC = 3;

		// Token: 0x04000A3C RID: 2620
		public const int OBJ_METADC = 4;

		// Token: 0x04000A3D RID: 2621
		public const int OBJ_FONT = 6;

		// Token: 0x04000A3E RID: 2622
		public const int OBJ_BITMAP = 7;

		// Token: 0x04000A3F RID: 2623
		public const int OBJ_MEMDC = 10;

		// Token: 0x04000A40 RID: 2624
		public const int OBJ_EXTPEN = 11;

		// Token: 0x04000A41 RID: 2625
		public const int OBJ_ENHMETADC = 12;

		// Token: 0x04000A42 RID: 2626
		public const int BS_SOLID = 0;

		// Token: 0x04000A43 RID: 2627
		public const int BS_HATCHED = 2;

		// Token: 0x04000A44 RID: 2628
		public const int CP_ACP = 0;

		// Token: 0x04000A45 RID: 2629
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000A46 RID: 2630
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000A47 RID: 2631
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000A48 RID: 2632
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x02000016 RID: 22
		public enum RegionFlags
		{
			// Token: 0x04000A4A RID: 2634
			ERROR,
			// Token: 0x04000A4B RID: 2635
			NULLREGION,
			// Token: 0x04000A4C RID: 2636
			SIMPLEREGION,
			// Token: 0x04000A4D RID: 2637
			COMPLEXREGION
		}

		// Token: 0x02000017 RID: 23
		public struct RECT
		{
			// Token: 0x0600004C RID: 76 RVA: 0x00002F20 File Offset: 0x00001F20
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x0600004D RID: 77 RVA: 0x00002F3F File Offset: 0x00001F3F
			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x0600004E RID: 78 RVA: 0x00002F75 File Offset: 0x00001F75
			public static IntNativeMethods.RECT FromXYWH(int x, int y, int width, int height)
			{
				return new IntNativeMethods.RECT(x, y, x + width, y + height);
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x0600004F RID: 79 RVA: 0x00002F84 File Offset: 0x00001F84
			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			// Token: 0x06000050 RID: 80 RVA: 0x00002FA5 File Offset: 0x00001FA5
			public Rectangle ToRectangle()
			{
				return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
			}

			// Token: 0x04000A4E RID: 2638
			public int left;

			// Token: 0x04000A4F RID: 2639
			public int top;

			// Token: 0x04000A50 RID: 2640
			public int right;

			// Token: 0x04000A51 RID: 2641
			public int bottom;
		}

		// Token: 0x02000018 RID: 24
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x06000051 RID: 81 RVA: 0x00002FD2 File Offset: 0x00001FD2
			public POINT()
			{
			}

			// Token: 0x06000052 RID: 82 RVA: 0x00002FDA File Offset: 0x00001FDA
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x06000053 RID: 83 RVA: 0x00002FF0 File Offset: 0x00001FF0
			public Point ToPoint()
			{
				return new Point(this.x, this.y);
			}

			// Token: 0x04000A52 RID: 2642
			public int x;

			// Token: 0x04000A53 RID: 2643
			public int y;
		}

		// Token: 0x02000019 RID: 25
		[StructLayout(LayoutKind.Sequential)]
		public class DRAWTEXTPARAMS
		{
			// Token: 0x06000054 RID: 84 RVA: 0x00003003 File Offset: 0x00002003
			public DRAWTEXTPARAMS()
			{
			}

			// Token: 0x06000055 RID: 85 RVA: 0x00003020 File Offset: 0x00002020
			public DRAWTEXTPARAMS(IntNativeMethods.DRAWTEXTPARAMS original)
			{
				this.iLeftMargin = original.iLeftMargin;
				this.iRightMargin = original.iRightMargin;
				this.iTabLength = original.iTabLength;
			}

			// Token: 0x06000056 RID: 86 RVA: 0x0000306C File Offset: 0x0000206C
			public DRAWTEXTPARAMS(int leftMargin, int rightMargin)
			{
				this.iLeftMargin = leftMargin;
				this.iRightMargin = rightMargin;
			}

			// Token: 0x04000A54 RID: 2644
			private int cbSize = Marshal.SizeOf(typeof(IntNativeMethods.DRAWTEXTPARAMS));

			// Token: 0x04000A55 RID: 2645
			public int iTabLength;

			// Token: 0x04000A56 RID: 2646
			public int iLeftMargin;

			// Token: 0x04000A57 RID: 2647
			public int iRightMargin;

			// Token: 0x04000A58 RID: 2648
			public int uiLengthDrawn;
		}

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			// Token: 0x04000A59 RID: 2649
			public int lbStyle;

			// Token: 0x04000A5A RID: 2650
			public int lbColor;

			// Token: 0x04000A5B RID: 2651
			public int lbHatch;
		}

		// Token: 0x0200001B RID: 27
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x06000058 RID: 88 RVA: 0x0000309F File Offset: 0x0000209F
			public LOGFONT()
			{
			}

			// Token: 0x06000059 RID: 89 RVA: 0x000030A8 File Offset: 0x000020A8
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

			// Token: 0x04000A5C RID: 2652
			public int lfHeight;

			// Token: 0x04000A5D RID: 2653
			public int lfWidth;

			// Token: 0x04000A5E RID: 2654
			public int lfEscapement;

			// Token: 0x04000A5F RID: 2655
			public int lfOrientation;

			// Token: 0x04000A60 RID: 2656
			public int lfWeight;

			// Token: 0x04000A61 RID: 2657
			public byte lfItalic;

			// Token: 0x04000A62 RID: 2658
			public byte lfUnderline;

			// Token: 0x04000A63 RID: 2659
			public byte lfStrikeOut;

			// Token: 0x04000A64 RID: 2660
			public byte lfCharSet;

			// Token: 0x04000A65 RID: 2661
			public byte lfOutPrecision;

			// Token: 0x04000A66 RID: 2662
			public byte lfClipPrecision;

			// Token: 0x04000A67 RID: 2663
			public byte lfQuality;

			// Token: 0x04000A68 RID: 2664
			public byte lfPitchAndFamily;

			// Token: 0x04000A69 RID: 2665
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x0200001C RID: 28
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04000A6A RID: 2666
			public int tmHeight;

			// Token: 0x04000A6B RID: 2667
			public int tmAscent;

			// Token: 0x04000A6C RID: 2668
			public int tmDescent;

			// Token: 0x04000A6D RID: 2669
			public int tmInternalLeading;

			// Token: 0x04000A6E RID: 2670
			public int tmExternalLeading;

			// Token: 0x04000A6F RID: 2671
			public int tmAveCharWidth;

			// Token: 0x04000A70 RID: 2672
			public int tmMaxCharWidth;

			// Token: 0x04000A71 RID: 2673
			public int tmWeight;

			// Token: 0x04000A72 RID: 2674
			public int tmOverhang;

			// Token: 0x04000A73 RID: 2675
			public int tmDigitizedAspectX;

			// Token: 0x04000A74 RID: 2676
			public int tmDigitizedAspectY;

			// Token: 0x04000A75 RID: 2677
			public char tmFirstChar;

			// Token: 0x04000A76 RID: 2678
			public char tmLastChar;

			// Token: 0x04000A77 RID: 2679
			public char tmDefaultChar;

			// Token: 0x04000A78 RID: 2680
			public char tmBreakChar;

			// Token: 0x04000A79 RID: 2681
			public byte tmItalic;

			// Token: 0x04000A7A RID: 2682
			public byte tmUnderlined;

			// Token: 0x04000A7B RID: 2683
			public byte tmStruckOut;

			// Token: 0x04000A7C RID: 2684
			public byte tmPitchAndFamily;

			// Token: 0x04000A7D RID: 2685
			public byte tmCharSet;
		}

		// Token: 0x0200001D RID: 29
		public struct TEXTMETRICA
		{
			// Token: 0x04000A7E RID: 2686
			public int tmHeight;

			// Token: 0x04000A7F RID: 2687
			public int tmAscent;

			// Token: 0x04000A80 RID: 2688
			public int tmDescent;

			// Token: 0x04000A81 RID: 2689
			public int tmInternalLeading;

			// Token: 0x04000A82 RID: 2690
			public int tmExternalLeading;

			// Token: 0x04000A83 RID: 2691
			public int tmAveCharWidth;

			// Token: 0x04000A84 RID: 2692
			public int tmMaxCharWidth;

			// Token: 0x04000A85 RID: 2693
			public int tmWeight;

			// Token: 0x04000A86 RID: 2694
			public int tmOverhang;

			// Token: 0x04000A87 RID: 2695
			public int tmDigitizedAspectX;

			// Token: 0x04000A88 RID: 2696
			public int tmDigitizedAspectY;

			// Token: 0x04000A89 RID: 2697
			public byte tmFirstChar;

			// Token: 0x04000A8A RID: 2698
			public byte tmLastChar;

			// Token: 0x04000A8B RID: 2699
			public byte tmDefaultChar;

			// Token: 0x04000A8C RID: 2700
			public byte tmBreakChar;

			// Token: 0x04000A8D RID: 2701
			public byte tmItalic;

			// Token: 0x04000A8E RID: 2702
			public byte tmUnderlined;

			// Token: 0x04000A8F RID: 2703
			public byte tmStruckOut;

			// Token: 0x04000A90 RID: 2704
			public byte tmPitchAndFamily;

			// Token: 0x04000A91 RID: 2705
			public byte tmCharSet;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Sequential)]
		public class SIZE
		{
			// Token: 0x0600005A RID: 90 RVA: 0x00003163 File Offset: 0x00002163
			public SIZE()
			{
			}

			// Token: 0x0600005B RID: 91 RVA: 0x0000316B File Offset: 0x0000216B
			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00003181 File Offset: 0x00002181
			public Size ToSize()
			{
				return new Size(this.cx, this.cy);
			}

			// Token: 0x04000A92 RID: 2706
			public int cx;

			// Token: 0x04000A93 RID: 2707
			public int cy;
		}
	}
}

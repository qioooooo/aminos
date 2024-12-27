using System;

namespace System.Drawing
{
	// Token: 0x02000035 RID: 53
	public sealed class Brushes
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00006AC4 File Offset: 0x00005AC4
		private Brushes()
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006ACC File Offset: 0x00005ACC
		public static Brush Transparent
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TransparentKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Transparent);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TransparentKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006B10 File Offset: 0x00005B10
		public static Brush AliceBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AliceBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.AliceBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AliceBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006B54 File Offset: 0x00005B54
		public static Brush AntiqueWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AntiqueWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.AntiqueWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AntiqueWhiteKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00006B98 File Offset: 0x00005B98
		public static Brush Aqua
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AquaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Aqua);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AquaKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006BDC File Offset: 0x00005BDC
		public static Brush Aquamarine
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AquamarineKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Aquamarine);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AquamarineKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00006C20 File Offset: 0x00005C20
		public static Brush Azure
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AzureKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Azure);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AzureKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006C64 File Offset: 0x00005C64
		public static Brush Beige
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BeigeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Beige);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BeigeKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00006CA8 File Offset: 0x00005CA8
		public static Brush Bisque
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BisqueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Bisque);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BisqueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00006CEC File Offset: 0x00005CEC
		public static Brush Black
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlackKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Black);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlackKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006D30 File Offset: 0x00005D30
		public static Brush BlanchedAlmond
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlanchedAlmondKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BlanchedAlmond);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlanchedAlmondKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00006D74 File Offset: 0x00005D74
		public static Brush Blue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Blue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006DB8 File Offset: 0x00005DB8
		public static Brush BlueViolet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlueVioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BlueViolet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlueVioletKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00006DFC File Offset: 0x00005DFC
		public static Brush Brown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Brown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BrownKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006E40 File Offset: 0x00005E40
		public static Brush BurlyWood
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BurlyWoodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BurlyWood);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BurlyWoodKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006E84 File Offset: 0x00005E84
		public static Brush CadetBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CadetBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.CadetBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CadetBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006EC8 File Offset: 0x00005EC8
		public static Brush Chartreuse
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChartreuseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Chartreuse);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChartreuseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006F0C File Offset: 0x00005F0C
		public static Brush Chocolate
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChocolateKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Chocolate);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChocolateKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00006F50 File Offset: 0x00005F50
		public static Brush Coral
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChoralKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Coral);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChoralKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00006F94 File Offset: 0x00005F94
		public static Brush CornflowerBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CornflowerBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.CornflowerBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CornflowerBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00006FD8 File Offset: 0x00005FD8
		public static Brush Cornsilk
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CornsilkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Cornsilk);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CornsilkKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000701C File Offset: 0x0000601C
		public static Brush Crimson
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CrimsonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Crimson);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CrimsonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007060 File Offset: 0x00006060
		public static Brush Cyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Cyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CyanKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000070A4 File Offset: 0x000060A4
		public static Brush DarkBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000070E8 File Offset: 0x000060E8
		public static Brush DarkCyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkCyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkCyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkCyanKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000712C File Offset: 0x0000612C
		public static Brush DarkGoldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGoldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGoldenrodKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007170 File Offset: 0x00006170
		public static Brush DarkGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000071B4 File Offset: 0x000061B4
		public static Brush DarkGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000071F8 File Offset: 0x000061F8
		public static Brush DarkKhaki
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkKhakiKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkKhaki);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkKhakiKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000723C File Offset: 0x0000623C
		public static Brush DarkMagenta
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkMagentaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkMagenta);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkMagentaKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007280 File Offset: 0x00006280
		public static Brush DarkOliveGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOliveGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOliveGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOliveGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000072C4 File Offset: 0x000062C4
		public static Brush DarkOrange
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrangeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOrange);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrangeKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007308 File Offset: 0x00006308
		public static Brush DarkOrchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOrchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrchidKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000734C File Offset: 0x0000634C
		public static Brush DarkRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkRedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007390 File Offset: 0x00006390
		public static Brush DarkSalmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSalmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSalmonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x000073D4 File Offset: 0x000063D4
		public static Brush DarkSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007418 File Offset: 0x00006418
		public static Brush DarkSlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000745C File Offset: 0x0000645C
		public static Brush DarkSlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000074A0 File Offset: 0x000064A0
		public static Brush DarkTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000074E4 File Offset: 0x000064E4
		public static Brush DarkViolet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkVioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkViolet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkVioletKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007528 File Offset: 0x00006528
		public static Brush DeepPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DeepPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DeepPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DeepPinkKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000756C File Offset: 0x0000656C
		public static Brush DeepSkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DeepSkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DeepSkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DeepSkyBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000075B0 File Offset: 0x000065B0
		public static Brush DimGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DimGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DimGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DimGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x000075F4 File Offset: 0x000065F4
		public static Brush DodgerBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DodgerBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DodgerBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DodgerBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007638 File Offset: 0x00006638
		public static Brush Firebrick
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FirebrickKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Firebrick);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FirebrickKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000767C File Offset: 0x0000667C
		public static Brush FloralWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FloralWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.FloralWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FloralWhiteKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000076C0 File Offset: 0x000066C0
		public static Brush ForestGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ForestGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.ForestGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ForestGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00007704 File Offset: 0x00006704
		public static Brush Fuchsia
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FuchiaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Fuchsia);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FuchiaKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007748 File Offset: 0x00006748
		public static Brush Gainsboro
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GainsboroKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gainsboro);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GainsboroKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000778C File Offset: 0x0000678C
		public static Brush GhostWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GhostWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.GhostWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GhostWhiteKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000077D0 File Offset: 0x000067D0
		public static Brush Gold
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GoldKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gold);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GoldKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007814 File Offset: 0x00006814
		public static Brush Goldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Goldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GoldenrodKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007858 File Offset: 0x00006858
		public static Brush Gray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000789C File Offset: 0x0000689C
		public static Brush Green
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Green);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000078E0 File Offset: 0x000068E0
		public static Brush GreenYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GreenYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.GreenYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GreenYellowKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00007924 File Offset: 0x00006924
		public static Brush Honeydew
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.HoneydewKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Honeydew);
					SafeNativeMethods.Gdip.ThreadData[Brushes.HoneydewKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00007968 File Offset: 0x00006968
		public static Brush HotPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.HotPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.HotPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.HotPinkKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000079AC File Offset: 0x000069AC
		public static Brush IndianRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IndianRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.IndianRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IndianRedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000079F0 File Offset: 0x000069F0
		public static Brush Indigo
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IndigoKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Indigo);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IndigoKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00007A34 File Offset: 0x00006A34
		public static Brush Ivory
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IvoryKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Ivory);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IvoryKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00007A78 File Offset: 0x00006A78
		public static Brush Khaki
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.KhakiKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Khaki);
					SafeNativeMethods.Gdip.ThreadData[Brushes.KhakiKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00007ABC File Offset: 0x00006ABC
		public static Brush Lavender
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Lavender);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007B00 File Offset: 0x00006B00
		public static Brush LavenderBlush
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderBlushKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LavenderBlush);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderBlushKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00007B44 File Offset: 0x00006B44
		public static Brush LawnGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LawnGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LawnGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LawnGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007B88 File Offset: 0x00006B88
		public static Brush LemonChiffon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LemonChiffonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LemonChiffon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LemonChiffonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007BCC File Offset: 0x00006BCC
		public static Brush LightBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007C10 File Offset: 0x00006C10
		public static Brush LightCoral
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightCoralKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightCoral);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightCoralKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007C54 File Offset: 0x00006C54
		public static Brush LightCyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightCyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightCyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightCyanKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00007C98 File Offset: 0x00006C98
		public static Brush LightGoldenrodYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGoldenrodYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGoldenrodYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGoldenrodYellowKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00007CDC File Offset: 0x00006CDC
		public static Brush LightGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00007D20 File Offset: 0x00006D20
		public static Brush LightGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00007D64 File Offset: 0x00006D64
		public static Brush LightPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightPinkKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007DA8 File Offset: 0x00006DA8
		public static Brush LightSalmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSalmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSalmonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00007DEC File Offset: 0x00006DEC
		public static Brush LightSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007E30 File Offset: 0x00006E30
		public static Brush LightSkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSkyBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007E74 File Offset: 0x00006E74
		public static Brush LightSlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSlateGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00007EB8 File Offset: 0x00006EB8
		public static Brush LightSteelBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSteelBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSteelBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSteelBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007EFC File Offset: 0x00006EFC
		public static Brush LightYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightYellowKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007F40 File Offset: 0x00006F40
		public static Brush Lime
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LimeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Lime);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LimeKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007F84 File Offset: 0x00006F84
		public static Brush LimeGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LimeGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LimeGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LimeGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00007FC8 File Offset: 0x00006FC8
		public static Brush Linen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LinenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Linen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LinenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000800C File Offset: 0x0000700C
		public static Brush Magenta
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MagentaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Magenta);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MagentaKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00008050 File Offset: 0x00007050
		public static Brush Maroon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MaroonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Maroon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MaroonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00008094 File Offset: 0x00007094
		public static Brush MediumAquamarine
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumAquamarineKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumAquamarine);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumAquamarineKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000080D8 File Offset: 0x000070D8
		public static Brush MediumBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000811C File Offset: 0x0000711C
		public static Brush MediumOrchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumOrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumOrchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumOrchidKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00008160 File Offset: 0x00007160
		public static Brush MediumPurple
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumPurpleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumPurple);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumPurpleKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000081A4 File Offset: 0x000071A4
		public static Brush MediumSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000081E8 File Offset: 0x000071E8
		public static Brush MediumSlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSlateBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000822C File Offset: 0x0000722C
		public static Brush MediumSpringGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSpringGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSpringGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSpringGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00008270 File Offset: 0x00007270
		public static Brush MediumTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000082B4 File Offset: 0x000072B4
		public static Brush MediumVioletRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumVioletRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumVioletRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumVioletRedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000082F8 File Offset: 0x000072F8
		public static Brush MidnightBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MidnightBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MidnightBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MidnightBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000833C File Offset: 0x0000733C
		public static Brush MintCream
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MintCreamKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MintCream);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MintCreamKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008380 File Offset: 0x00007380
		public static Brush MistyRose
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MistyRoseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MistyRose);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MistyRoseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000083C4 File Offset: 0x000073C4
		public static Brush Moccasin
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MoccasinKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Moccasin);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MoccasinKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00008408 File Offset: 0x00007408
		public static Brush NavajoWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.NavajoWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.NavajoWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.NavajoWhiteKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000844C File Offset: 0x0000744C
		public static Brush Navy
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.NavyKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Navy);
					SafeNativeMethods.Gdip.ThreadData[Brushes.NavyKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008490 File Offset: 0x00007490
		public static Brush OldLace
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OldLaceKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OldLace);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OldLaceKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000084D4 File Offset: 0x000074D4
		public static Brush Olive
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OliveKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Olive);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OliveKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008518 File Offset: 0x00007518
		public static Brush OliveDrab
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OliveDrabKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OliveDrab);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OliveDrabKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000855C File Offset: 0x0000755C
		public static Brush Orange
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Orange);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600020B RID: 523 RVA: 0x000085A0 File Offset: 0x000075A0
		public static Brush OrangeRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OrangeRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeRedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000085E4 File Offset: 0x000075E4
		public static Brush Orchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Orchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrchidKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008628 File Offset: 0x00007628
		public static Brush PaleGoldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleGoldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGoldenrodKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000866C File Offset: 0x0000766C
		public static Brush PaleGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000086B0 File Offset: 0x000076B0
		public static Brush PaleTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000210 RID: 528 RVA: 0x000086F4 File Offset: 0x000076F4
		public static Brush PaleVioletRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleVioletRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleVioletRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleVioletRedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008738 File Offset: 0x00007738
		public static Brush PapayaWhip
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PapayaWhipKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PapayaWhip);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PapayaWhipKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000877C File Offset: 0x0000777C
		public static Brush PeachPuff
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PeachPuffKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PeachPuff);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PeachPuffKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000087C0 File Offset: 0x000077C0
		public static Brush Peru
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PeruKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Peru);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PeruKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00008804 File Offset: 0x00007804
		public static Brush Pink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Pink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PinkKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008848 File Offset: 0x00007848
		public static Brush Plum
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PlumKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Plum);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PlumKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000888C File Offset: 0x0000788C
		public static Brush PowderBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PowderBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PowderBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PowderBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000088D0 File Offset: 0x000078D0
		public static Brush Purple
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PurpleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Purple);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PurpleKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00008914 File Offset: 0x00007914
		public static Brush Red
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Red);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RedKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00008958 File Offset: 0x00007958
		public static Brush RosyBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RosyBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.RosyBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RosyBrownKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000899C File Offset: 0x0000799C
		public static Brush RoyalBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RoyalBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.RoyalBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RoyalBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000089E0 File Offset: 0x000079E0
		public static Brush SaddleBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SaddleBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SaddleBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SaddleBrownKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00008A24 File Offset: 0x00007A24
		public static Brush Salmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Salmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SalmonKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00008A68 File Offset: 0x00007A68
		public static Brush SandyBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SandyBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SandyBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SandyBrownKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00008AAC File Offset: 0x00007AAC
		public static Brush SeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SeaGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008AF0 File Offset: 0x00007AF0
		public static Brush SeaShell
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SeaShellKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SeaShell);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SeaShellKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00008B34 File Offset: 0x00007B34
		public static Brush Sienna
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SiennaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Sienna);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SiennaKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00008B78 File Offset: 0x00007B78
		public static Brush Silver
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SilverKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Silver);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SilverKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00008BBC File Offset: 0x00007BBC
		public static Brush SkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SkyBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00008C00 File Offset: 0x00007C00
		public static Brush SlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SlateBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008C44 File Offset: 0x00007C44
		public static Brush SlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SlateGrayKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00008C88 File Offset: 0x00007C88
		public static Brush Snow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SnowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Snow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SnowKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00008CCC File Offset: 0x00007CCC
		public static Brush SpringGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SpringGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SpringGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SpringGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00008D10 File Offset: 0x00007D10
		public static Brush SteelBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SteelBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SteelBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SteelBlueKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00008D54 File Offset: 0x00007D54
		public static Brush Tan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Tan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TanKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00008D98 File Offset: 0x00007D98
		public static Brush Teal
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TealKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Teal);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TealKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00008DDC File Offset: 0x00007DDC
		public static Brush Thistle
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ThistleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Thistle);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ThistleKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00008E20 File Offset: 0x00007E20
		public static Brush Tomato
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TomatoKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Tomato);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TomatoKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00008E64 File Offset: 0x00007E64
		public static Brush Turquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Turquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TurquoiseKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00008EA8 File Offset: 0x00007EA8
		public static Brush Violet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.VioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Violet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.VioletKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00008EEC File Offset: 0x00007EEC
		public static Brush Wheat
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WheatKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Wheat);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WheatKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00008F30 File Offset: 0x00007F30
		public static Brush White
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.White);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00008F74 File Offset: 0x00007F74
		public static Brush WhiteSmoke
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteSmokeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.WhiteSmoke);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteSmokeKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00008FB8 File Offset: 0x00007FB8
		public static Brush Yellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.YellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Yellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.YellowKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00008FFC File Offset: 0x00007FFC
		public static Brush YellowGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.YellowGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.YellowGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.YellowGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x040001C6 RID: 454
		private static readonly object TransparentKey = new object();

		// Token: 0x040001C7 RID: 455
		private static readonly object AliceBlueKey = new object();

		// Token: 0x040001C8 RID: 456
		private static readonly object AntiqueWhiteKey = new object();

		// Token: 0x040001C9 RID: 457
		private static readonly object AquaKey = new object();

		// Token: 0x040001CA RID: 458
		private static readonly object AquamarineKey = new object();

		// Token: 0x040001CB RID: 459
		private static readonly object AzureKey = new object();

		// Token: 0x040001CC RID: 460
		private static readonly object BeigeKey = new object();

		// Token: 0x040001CD RID: 461
		private static readonly object BisqueKey = new object();

		// Token: 0x040001CE RID: 462
		private static readonly object BlackKey = new object();

		// Token: 0x040001CF RID: 463
		private static readonly object BlanchedAlmondKey = new object();

		// Token: 0x040001D0 RID: 464
		private static readonly object BlueKey = new object();

		// Token: 0x040001D1 RID: 465
		private static readonly object BlueVioletKey = new object();

		// Token: 0x040001D2 RID: 466
		private static readonly object BrownKey = new object();

		// Token: 0x040001D3 RID: 467
		private static readonly object BurlyWoodKey = new object();

		// Token: 0x040001D4 RID: 468
		private static readonly object CadetBlueKey = new object();

		// Token: 0x040001D5 RID: 469
		private static readonly object ChartreuseKey = new object();

		// Token: 0x040001D6 RID: 470
		private static readonly object ChocolateKey = new object();

		// Token: 0x040001D7 RID: 471
		private static readonly object ChoralKey = new object();

		// Token: 0x040001D8 RID: 472
		private static readonly object CornflowerBlueKey = new object();

		// Token: 0x040001D9 RID: 473
		private static readonly object CornsilkKey = new object();

		// Token: 0x040001DA RID: 474
		private static readonly object CrimsonKey = new object();

		// Token: 0x040001DB RID: 475
		private static readonly object CyanKey = new object();

		// Token: 0x040001DC RID: 476
		private static readonly object DarkBlueKey = new object();

		// Token: 0x040001DD RID: 477
		private static readonly object DarkCyanKey = new object();

		// Token: 0x040001DE RID: 478
		private static readonly object DarkGoldenrodKey = new object();

		// Token: 0x040001DF RID: 479
		private static readonly object DarkGrayKey = new object();

		// Token: 0x040001E0 RID: 480
		private static readonly object DarkGreenKey = new object();

		// Token: 0x040001E1 RID: 481
		private static readonly object DarkKhakiKey = new object();

		// Token: 0x040001E2 RID: 482
		private static readonly object DarkMagentaKey = new object();

		// Token: 0x040001E3 RID: 483
		private static readonly object DarkOliveGreenKey = new object();

		// Token: 0x040001E4 RID: 484
		private static readonly object DarkOrangeKey = new object();

		// Token: 0x040001E5 RID: 485
		private static readonly object DarkOrchidKey = new object();

		// Token: 0x040001E6 RID: 486
		private static readonly object DarkRedKey = new object();

		// Token: 0x040001E7 RID: 487
		private static readonly object DarkSalmonKey = new object();

		// Token: 0x040001E8 RID: 488
		private static readonly object DarkSeaGreenKey = new object();

		// Token: 0x040001E9 RID: 489
		private static readonly object DarkSlateBlueKey = new object();

		// Token: 0x040001EA RID: 490
		private static readonly object DarkSlateGrayKey = new object();

		// Token: 0x040001EB RID: 491
		private static readonly object DarkTurquoiseKey = new object();

		// Token: 0x040001EC RID: 492
		private static readonly object DarkVioletKey = new object();

		// Token: 0x040001ED RID: 493
		private static readonly object DeepPinkKey = new object();

		// Token: 0x040001EE RID: 494
		private static readonly object DeepSkyBlueKey = new object();

		// Token: 0x040001EF RID: 495
		private static readonly object DimGrayKey = new object();

		// Token: 0x040001F0 RID: 496
		private static readonly object DodgerBlueKey = new object();

		// Token: 0x040001F1 RID: 497
		private static readonly object FirebrickKey = new object();

		// Token: 0x040001F2 RID: 498
		private static readonly object FloralWhiteKey = new object();

		// Token: 0x040001F3 RID: 499
		private static readonly object ForestGreenKey = new object();

		// Token: 0x040001F4 RID: 500
		private static readonly object FuchiaKey = new object();

		// Token: 0x040001F5 RID: 501
		private static readonly object GainsboroKey = new object();

		// Token: 0x040001F6 RID: 502
		private static readonly object GhostWhiteKey = new object();

		// Token: 0x040001F7 RID: 503
		private static readonly object GoldKey = new object();

		// Token: 0x040001F8 RID: 504
		private static readonly object GoldenrodKey = new object();

		// Token: 0x040001F9 RID: 505
		private static readonly object GrayKey = new object();

		// Token: 0x040001FA RID: 506
		private static readonly object GreenKey = new object();

		// Token: 0x040001FB RID: 507
		private static readonly object GreenYellowKey = new object();

		// Token: 0x040001FC RID: 508
		private static readonly object HoneydewKey = new object();

		// Token: 0x040001FD RID: 509
		private static readonly object HotPinkKey = new object();

		// Token: 0x040001FE RID: 510
		private static readonly object IndianRedKey = new object();

		// Token: 0x040001FF RID: 511
		private static readonly object IndigoKey = new object();

		// Token: 0x04000200 RID: 512
		private static readonly object IvoryKey = new object();

		// Token: 0x04000201 RID: 513
		private static readonly object KhakiKey = new object();

		// Token: 0x04000202 RID: 514
		private static readonly object LavenderKey = new object();

		// Token: 0x04000203 RID: 515
		private static readonly object LavenderBlushKey = new object();

		// Token: 0x04000204 RID: 516
		private static readonly object LawnGreenKey = new object();

		// Token: 0x04000205 RID: 517
		private static readonly object LemonChiffonKey = new object();

		// Token: 0x04000206 RID: 518
		private static readonly object LightBlueKey = new object();

		// Token: 0x04000207 RID: 519
		private static readonly object LightCoralKey = new object();

		// Token: 0x04000208 RID: 520
		private static readonly object LightCyanKey = new object();

		// Token: 0x04000209 RID: 521
		private static readonly object LightGoldenrodYellowKey = new object();

		// Token: 0x0400020A RID: 522
		private static readonly object LightGreenKey = new object();

		// Token: 0x0400020B RID: 523
		private static readonly object LightGrayKey = new object();

		// Token: 0x0400020C RID: 524
		private static readonly object LightPinkKey = new object();

		// Token: 0x0400020D RID: 525
		private static readonly object LightSalmonKey = new object();

		// Token: 0x0400020E RID: 526
		private static readonly object LightSeaGreenKey = new object();

		// Token: 0x0400020F RID: 527
		private static readonly object LightSkyBlueKey = new object();

		// Token: 0x04000210 RID: 528
		private static readonly object LightSlateGrayKey = new object();

		// Token: 0x04000211 RID: 529
		private static readonly object LightSteelBlueKey = new object();

		// Token: 0x04000212 RID: 530
		private static readonly object LightYellowKey = new object();

		// Token: 0x04000213 RID: 531
		private static readonly object LimeKey = new object();

		// Token: 0x04000214 RID: 532
		private static readonly object LimeGreenKey = new object();

		// Token: 0x04000215 RID: 533
		private static readonly object LinenKey = new object();

		// Token: 0x04000216 RID: 534
		private static readonly object MagentaKey = new object();

		// Token: 0x04000217 RID: 535
		private static readonly object MaroonKey = new object();

		// Token: 0x04000218 RID: 536
		private static readonly object MediumAquamarineKey = new object();

		// Token: 0x04000219 RID: 537
		private static readonly object MediumBlueKey = new object();

		// Token: 0x0400021A RID: 538
		private static readonly object MediumOrchidKey = new object();

		// Token: 0x0400021B RID: 539
		private static readonly object MediumPurpleKey = new object();

		// Token: 0x0400021C RID: 540
		private static readonly object MediumSeaGreenKey = new object();

		// Token: 0x0400021D RID: 541
		private static readonly object MediumSlateBlueKey = new object();

		// Token: 0x0400021E RID: 542
		private static readonly object MediumSpringGreenKey = new object();

		// Token: 0x0400021F RID: 543
		private static readonly object MediumTurquoiseKey = new object();

		// Token: 0x04000220 RID: 544
		private static readonly object MediumVioletRedKey = new object();

		// Token: 0x04000221 RID: 545
		private static readonly object MidnightBlueKey = new object();

		// Token: 0x04000222 RID: 546
		private static readonly object MintCreamKey = new object();

		// Token: 0x04000223 RID: 547
		private static readonly object MistyRoseKey = new object();

		// Token: 0x04000224 RID: 548
		private static readonly object MoccasinKey = new object();

		// Token: 0x04000225 RID: 549
		private static readonly object NavajoWhiteKey = new object();

		// Token: 0x04000226 RID: 550
		private static readonly object NavyKey = new object();

		// Token: 0x04000227 RID: 551
		private static readonly object OldLaceKey = new object();

		// Token: 0x04000228 RID: 552
		private static readonly object OliveKey = new object();

		// Token: 0x04000229 RID: 553
		private static readonly object OliveDrabKey = new object();

		// Token: 0x0400022A RID: 554
		private static readonly object OrangeKey = new object();

		// Token: 0x0400022B RID: 555
		private static readonly object OrangeRedKey = new object();

		// Token: 0x0400022C RID: 556
		private static readonly object OrchidKey = new object();

		// Token: 0x0400022D RID: 557
		private static readonly object PaleGoldenrodKey = new object();

		// Token: 0x0400022E RID: 558
		private static readonly object PaleGreenKey = new object();

		// Token: 0x0400022F RID: 559
		private static readonly object PaleTurquoiseKey = new object();

		// Token: 0x04000230 RID: 560
		private static readonly object PaleVioletRedKey = new object();

		// Token: 0x04000231 RID: 561
		private static readonly object PapayaWhipKey = new object();

		// Token: 0x04000232 RID: 562
		private static readonly object PeachPuffKey = new object();

		// Token: 0x04000233 RID: 563
		private static readonly object PeruKey = new object();

		// Token: 0x04000234 RID: 564
		private static readonly object PinkKey = new object();

		// Token: 0x04000235 RID: 565
		private static readonly object PlumKey = new object();

		// Token: 0x04000236 RID: 566
		private static readonly object PowderBlueKey = new object();

		// Token: 0x04000237 RID: 567
		private static readonly object PurpleKey = new object();

		// Token: 0x04000238 RID: 568
		private static readonly object RedKey = new object();

		// Token: 0x04000239 RID: 569
		private static readonly object RosyBrownKey = new object();

		// Token: 0x0400023A RID: 570
		private static readonly object RoyalBlueKey = new object();

		// Token: 0x0400023B RID: 571
		private static readonly object SaddleBrownKey = new object();

		// Token: 0x0400023C RID: 572
		private static readonly object SalmonKey = new object();

		// Token: 0x0400023D RID: 573
		private static readonly object SandyBrownKey = new object();

		// Token: 0x0400023E RID: 574
		private static readonly object SeaGreenKey = new object();

		// Token: 0x0400023F RID: 575
		private static readonly object SeaShellKey = new object();

		// Token: 0x04000240 RID: 576
		private static readonly object SiennaKey = new object();

		// Token: 0x04000241 RID: 577
		private static readonly object SilverKey = new object();

		// Token: 0x04000242 RID: 578
		private static readonly object SkyBlueKey = new object();

		// Token: 0x04000243 RID: 579
		private static readonly object SlateBlueKey = new object();

		// Token: 0x04000244 RID: 580
		private static readonly object SlateGrayKey = new object();

		// Token: 0x04000245 RID: 581
		private static readonly object SnowKey = new object();

		// Token: 0x04000246 RID: 582
		private static readonly object SpringGreenKey = new object();

		// Token: 0x04000247 RID: 583
		private static readonly object SteelBlueKey = new object();

		// Token: 0x04000248 RID: 584
		private static readonly object TanKey = new object();

		// Token: 0x04000249 RID: 585
		private static readonly object TealKey = new object();

		// Token: 0x0400024A RID: 586
		private static readonly object ThistleKey = new object();

		// Token: 0x0400024B RID: 587
		private static readonly object TomatoKey = new object();

		// Token: 0x0400024C RID: 588
		private static readonly object TurquoiseKey = new object();

		// Token: 0x0400024D RID: 589
		private static readonly object VioletKey = new object();

		// Token: 0x0400024E RID: 590
		private static readonly object WheatKey = new object();

		// Token: 0x0400024F RID: 591
		private static readonly object WhiteKey = new object();

		// Token: 0x04000250 RID: 592
		private static readonly object WhiteSmokeKey = new object();

		// Token: 0x04000251 RID: 593
		private static readonly object YellowKey = new object();

		// Token: 0x04000252 RID: 594
		private static readonly object YellowGreenKey = new object();
	}
}

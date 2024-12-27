using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Drawing
{
	// Token: 0x02000039 RID: 57
	[TypeConverter(typeof(ColorConverter))]
	[DebuggerDisplay("{NameAndARGBValue}")]
	[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public struct Color
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000A166 File Offset: 0x00009166
		public static Color Transparent
		{
			get
			{
				return new Color(KnownColor.Transparent);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000A16F File Offset: 0x0000916F
		public static Color AliceBlue
		{
			get
			{
				return new Color(KnownColor.AliceBlue);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000A178 File Offset: 0x00009178
		public static Color AntiqueWhite
		{
			get
			{
				return new Color(KnownColor.AntiqueWhite);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000A181 File Offset: 0x00009181
		public static Color Aqua
		{
			get
			{
				return new Color(KnownColor.Aqua);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000A18A File Offset: 0x0000918A
		public static Color Aquamarine
		{
			get
			{
				return new Color(KnownColor.Aquamarine);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000A193 File Offset: 0x00009193
		public static Color Azure
		{
			get
			{
				return new Color(KnownColor.Azure);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000A19C File Offset: 0x0000919C
		public static Color Beige
		{
			get
			{
				return new Color(KnownColor.Beige);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000A1A5 File Offset: 0x000091A5
		public static Color Bisque
		{
			get
			{
				return new Color(KnownColor.Bisque);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000A1AE File Offset: 0x000091AE
		public static Color Black
		{
			get
			{
				return new Color(KnownColor.Black);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000A1B7 File Offset: 0x000091B7
		public static Color BlanchedAlmond
		{
			get
			{
				return new Color(KnownColor.BlanchedAlmond);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000A1C0 File Offset: 0x000091C0
		public static Color Blue
		{
			get
			{
				return new Color(KnownColor.Blue);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000A1C9 File Offset: 0x000091C9
		public static Color BlueViolet
		{
			get
			{
				return new Color(KnownColor.BlueViolet);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000A1D2 File Offset: 0x000091D2
		public static Color Brown
		{
			get
			{
				return new Color(KnownColor.Brown);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000A1DB File Offset: 0x000091DB
		public static Color BurlyWood
		{
			get
			{
				return new Color(KnownColor.BurlyWood);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000A1E4 File Offset: 0x000091E4
		public static Color CadetBlue
		{
			get
			{
				return new Color(KnownColor.CadetBlue);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000A1ED File Offset: 0x000091ED
		public static Color Chartreuse
		{
			get
			{
				return new Color(KnownColor.Chartreuse);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000A1F6 File Offset: 0x000091F6
		public static Color Chocolate
		{
			get
			{
				return new Color(KnownColor.Chocolate);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000A1FF File Offset: 0x000091FF
		public static Color Coral
		{
			get
			{
				return new Color(KnownColor.Coral);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000A208 File Offset: 0x00009208
		public static Color CornflowerBlue
		{
			get
			{
				return new Color(KnownColor.CornflowerBlue);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000A211 File Offset: 0x00009211
		public static Color Cornsilk
		{
			get
			{
				return new Color(KnownColor.Cornsilk);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000A21A File Offset: 0x0000921A
		public static Color Crimson
		{
			get
			{
				return new Color(KnownColor.Crimson);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000A223 File Offset: 0x00009223
		public static Color Cyan
		{
			get
			{
				return new Color(KnownColor.Cyan);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000A22C File Offset: 0x0000922C
		public static Color DarkBlue
		{
			get
			{
				return new Color(KnownColor.DarkBlue);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000A235 File Offset: 0x00009235
		public static Color DarkCyan
		{
			get
			{
				return new Color(KnownColor.DarkCyan);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000A23E File Offset: 0x0000923E
		public static Color DarkGoldenrod
		{
			get
			{
				return new Color(KnownColor.DarkGoldenrod);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000A247 File Offset: 0x00009247
		public static Color DarkGray
		{
			get
			{
				return new Color(KnownColor.DarkGray);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000A250 File Offset: 0x00009250
		public static Color DarkGreen
		{
			get
			{
				return new Color(KnownColor.DarkGreen);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000A259 File Offset: 0x00009259
		public static Color DarkKhaki
		{
			get
			{
				return new Color(KnownColor.DarkKhaki);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000A262 File Offset: 0x00009262
		public static Color DarkMagenta
		{
			get
			{
				return new Color(KnownColor.DarkMagenta);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000A26B File Offset: 0x0000926B
		public static Color DarkOliveGreen
		{
			get
			{
				return new Color(KnownColor.DarkOliveGreen);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000A274 File Offset: 0x00009274
		public static Color DarkOrange
		{
			get
			{
				return new Color(KnownColor.DarkOrange);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000A27D File Offset: 0x0000927D
		public static Color DarkOrchid
		{
			get
			{
				return new Color(KnownColor.DarkOrchid);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000A286 File Offset: 0x00009286
		public static Color DarkRed
		{
			get
			{
				return new Color(KnownColor.DarkRed);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000A28F File Offset: 0x0000928F
		public static Color DarkSalmon
		{
			get
			{
				return new Color(KnownColor.DarkSalmon);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000A298 File Offset: 0x00009298
		public static Color DarkSeaGreen
		{
			get
			{
				return new Color(KnownColor.DarkSeaGreen);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000A2A1 File Offset: 0x000092A1
		public static Color DarkSlateBlue
		{
			get
			{
				return new Color(KnownColor.DarkSlateBlue);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000A2AA File Offset: 0x000092AA
		public static Color DarkSlateGray
		{
			get
			{
				return new Color(KnownColor.DarkSlateGray);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000A2B3 File Offset: 0x000092B3
		public static Color DarkTurquoise
		{
			get
			{
				return new Color(KnownColor.DarkTurquoise);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000A2BC File Offset: 0x000092BC
		public static Color DarkViolet
		{
			get
			{
				return new Color(KnownColor.DarkViolet);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000A2C5 File Offset: 0x000092C5
		public static Color DeepPink
		{
			get
			{
				return new Color(KnownColor.DeepPink);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000A2CE File Offset: 0x000092CE
		public static Color DeepSkyBlue
		{
			get
			{
				return new Color(KnownColor.DeepSkyBlue);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000A2D7 File Offset: 0x000092D7
		public static Color DimGray
		{
			get
			{
				return new Color(KnownColor.DimGray);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000A2E0 File Offset: 0x000092E0
		public static Color DodgerBlue
		{
			get
			{
				return new Color(KnownColor.DodgerBlue);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000A2E9 File Offset: 0x000092E9
		public static Color Firebrick
		{
			get
			{
				return new Color(KnownColor.Firebrick);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000A2F2 File Offset: 0x000092F2
		public static Color FloralWhite
		{
			get
			{
				return new Color(KnownColor.FloralWhite);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000A2FB File Offset: 0x000092FB
		public static Color ForestGreen
		{
			get
			{
				return new Color(KnownColor.ForestGreen);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A304 File Offset: 0x00009304
		public static Color Fuchsia
		{
			get
			{
				return new Color(KnownColor.Fuchsia);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000A30D File Offset: 0x0000930D
		public static Color Gainsboro
		{
			get
			{
				return new Color(KnownColor.Gainsboro);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000A316 File Offset: 0x00009316
		public static Color GhostWhite
		{
			get
			{
				return new Color(KnownColor.GhostWhite);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000A31F File Offset: 0x0000931F
		public static Color Gold
		{
			get
			{
				return new Color(KnownColor.Gold);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000A328 File Offset: 0x00009328
		public static Color Goldenrod
		{
			get
			{
				return new Color(KnownColor.Goldenrod);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000A331 File Offset: 0x00009331
		public static Color Gray
		{
			get
			{
				return new Color(KnownColor.Gray);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000A33A File Offset: 0x0000933A
		public static Color Green
		{
			get
			{
				return new Color(KnownColor.Green);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000A343 File Offset: 0x00009343
		public static Color GreenYellow
		{
			get
			{
				return new Color(KnownColor.GreenYellow);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000A34C File Offset: 0x0000934C
		public static Color Honeydew
		{
			get
			{
				return new Color(KnownColor.Honeydew);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000A355 File Offset: 0x00009355
		public static Color HotPink
		{
			get
			{
				return new Color(KnownColor.HotPink);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000A35E File Offset: 0x0000935E
		public static Color IndianRed
		{
			get
			{
				return new Color(KnownColor.IndianRed);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000A367 File Offset: 0x00009367
		public static Color Indigo
		{
			get
			{
				return new Color(KnownColor.Indigo);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A370 File Offset: 0x00009370
		public static Color Ivory
		{
			get
			{
				return new Color(KnownColor.Ivory);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000A379 File Offset: 0x00009379
		public static Color Khaki
		{
			get
			{
				return new Color(KnownColor.Khaki);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000A382 File Offset: 0x00009382
		public static Color Lavender
		{
			get
			{
				return new Color(KnownColor.Lavender);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000A38B File Offset: 0x0000938B
		public static Color LavenderBlush
		{
			get
			{
				return new Color(KnownColor.LavenderBlush);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000A394 File Offset: 0x00009394
		public static Color LawnGreen
		{
			get
			{
				return new Color(KnownColor.LawnGreen);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000A39D File Offset: 0x0000939D
		public static Color LemonChiffon
		{
			get
			{
				return new Color(KnownColor.LemonChiffon);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000A3A6 File Offset: 0x000093A6
		public static Color LightBlue
		{
			get
			{
				return new Color(KnownColor.LightBlue);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A3AF File Offset: 0x000093AF
		public static Color LightCoral
		{
			get
			{
				return new Color(KnownColor.LightCoral);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000A3B8 File Offset: 0x000093B8
		public static Color LightCyan
		{
			get
			{
				return new Color(KnownColor.LightCyan);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000A3C1 File Offset: 0x000093C1
		public static Color LightGoldenrodYellow
		{
			get
			{
				return new Color(KnownColor.LightGoldenrodYellow);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000A3CA File Offset: 0x000093CA
		public static Color LightGreen
		{
			get
			{
				return new Color(KnownColor.LightGreen);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000A3D3 File Offset: 0x000093D3
		public static Color LightGray
		{
			get
			{
				return new Color(KnownColor.LightGray);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000A3DC File Offset: 0x000093DC
		public static Color LightPink
		{
			get
			{
				return new Color(KnownColor.LightPink);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000A3E5 File Offset: 0x000093E5
		public static Color LightSalmon
		{
			get
			{
				return new Color(KnownColor.LightSalmon);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000A3EE File Offset: 0x000093EE
		public static Color LightSeaGreen
		{
			get
			{
				return new Color(KnownColor.LightSeaGreen);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000A3F7 File Offset: 0x000093F7
		public static Color LightSkyBlue
		{
			get
			{
				return new Color(KnownColor.LightSkyBlue);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000A400 File Offset: 0x00009400
		public static Color LightSlateGray
		{
			get
			{
				return new Color(KnownColor.LightSlateGray);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000A409 File Offset: 0x00009409
		public static Color LightSteelBlue
		{
			get
			{
				return new Color(KnownColor.LightSteelBlue);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000A412 File Offset: 0x00009412
		public static Color LightYellow
		{
			get
			{
				return new Color(KnownColor.LightYellow);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000A41B File Offset: 0x0000941B
		public static Color Lime
		{
			get
			{
				return new Color(KnownColor.Lime);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000A424 File Offset: 0x00009424
		public static Color LimeGreen
		{
			get
			{
				return new Color(KnownColor.LimeGreen);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000A42D File Offset: 0x0000942D
		public static Color Linen
		{
			get
			{
				return new Color(KnownColor.Linen);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000A436 File Offset: 0x00009436
		public static Color Magenta
		{
			get
			{
				return new Color(KnownColor.Magenta);
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000A43F File Offset: 0x0000943F
		public static Color Maroon
		{
			get
			{
				return new Color(KnownColor.Maroon);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000A448 File Offset: 0x00009448
		public static Color MediumAquamarine
		{
			get
			{
				return new Color(KnownColor.MediumAquamarine);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000A451 File Offset: 0x00009451
		public static Color MediumBlue
		{
			get
			{
				return new Color(KnownColor.MediumBlue);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000A45A File Offset: 0x0000945A
		public static Color MediumOrchid
		{
			get
			{
				return new Color(KnownColor.MediumOrchid);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000A463 File Offset: 0x00009463
		public static Color MediumPurple
		{
			get
			{
				return new Color(KnownColor.MediumPurple);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000A46C File Offset: 0x0000946C
		public static Color MediumSeaGreen
		{
			get
			{
				return new Color(KnownColor.MediumSeaGreen);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000A475 File Offset: 0x00009475
		public static Color MediumSlateBlue
		{
			get
			{
				return new Color(KnownColor.MediumSlateBlue);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000A47E File Offset: 0x0000947E
		public static Color MediumSpringGreen
		{
			get
			{
				return new Color(KnownColor.MediumSpringGreen);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000A487 File Offset: 0x00009487
		public static Color MediumTurquoise
		{
			get
			{
				return new Color(KnownColor.MediumTurquoise);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000A490 File Offset: 0x00009490
		public static Color MediumVioletRed
		{
			get
			{
				return new Color(KnownColor.MediumVioletRed);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000A499 File Offset: 0x00009499
		public static Color MidnightBlue
		{
			get
			{
				return new Color(KnownColor.MidnightBlue);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000A4A2 File Offset: 0x000094A2
		public static Color MintCream
		{
			get
			{
				return new Color(KnownColor.MintCream);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000A4AB File Offset: 0x000094AB
		public static Color MistyRose
		{
			get
			{
				return new Color(KnownColor.MistyRose);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000A4B4 File Offset: 0x000094B4
		public static Color Moccasin
		{
			get
			{
				return new Color(KnownColor.Moccasin);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000A4BD File Offset: 0x000094BD
		public static Color NavajoWhite
		{
			get
			{
				return new Color(KnownColor.NavajoWhite);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000A4C6 File Offset: 0x000094C6
		public static Color Navy
		{
			get
			{
				return new Color(KnownColor.Navy);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000A4CF File Offset: 0x000094CF
		public static Color OldLace
		{
			get
			{
				return new Color(KnownColor.OldLace);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000A4D8 File Offset: 0x000094D8
		public static Color Olive
		{
			get
			{
				return new Color(KnownColor.Olive);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000A4E1 File Offset: 0x000094E1
		public static Color OliveDrab
		{
			get
			{
				return new Color(KnownColor.OliveDrab);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000A4EA File Offset: 0x000094EA
		public static Color Orange
		{
			get
			{
				return new Color(KnownColor.Orange);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000A4F3 File Offset: 0x000094F3
		public static Color OrangeRed
		{
			get
			{
				return new Color(KnownColor.OrangeRed);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000A4FF File Offset: 0x000094FF
		public static Color Orchid
		{
			get
			{
				return new Color(KnownColor.Orchid);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000A50B File Offset: 0x0000950B
		public static Color PaleGoldenrod
		{
			get
			{
				return new Color(KnownColor.PaleGoldenrod);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000A517 File Offset: 0x00009517
		public static Color PaleGreen
		{
			get
			{
				return new Color(KnownColor.PaleGreen);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000A523 File Offset: 0x00009523
		public static Color PaleTurquoise
		{
			get
			{
				return new Color(KnownColor.PaleTurquoise);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000A52F File Offset: 0x0000952F
		public static Color PaleVioletRed
		{
			get
			{
				return new Color(KnownColor.PaleVioletRed);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000A53B File Offset: 0x0000953B
		public static Color PapayaWhip
		{
			get
			{
				return new Color(KnownColor.PapayaWhip);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000A547 File Offset: 0x00009547
		public static Color PeachPuff
		{
			get
			{
				return new Color(KnownColor.PeachPuff);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000A553 File Offset: 0x00009553
		public static Color Peru
		{
			get
			{
				return new Color(KnownColor.Peru);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000A55F File Offset: 0x0000955F
		public static Color Pink
		{
			get
			{
				return new Color(KnownColor.Pink);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000A56B File Offset: 0x0000956B
		public static Color Plum
		{
			get
			{
				return new Color(KnownColor.Plum);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000A577 File Offset: 0x00009577
		public static Color PowderBlue
		{
			get
			{
				return new Color(KnownColor.PowderBlue);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000A583 File Offset: 0x00009583
		public static Color Purple
		{
			get
			{
				return new Color(KnownColor.Purple);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000A58F File Offset: 0x0000958F
		public static Color Red
		{
			get
			{
				return new Color(KnownColor.Red);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000A59B File Offset: 0x0000959B
		public static Color RosyBrown
		{
			get
			{
				return new Color(KnownColor.RosyBrown);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000A5A7 File Offset: 0x000095A7
		public static Color RoyalBlue
		{
			get
			{
				return new Color(KnownColor.RoyalBlue);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000A5B3 File Offset: 0x000095B3
		public static Color SaddleBrown
		{
			get
			{
				return new Color(KnownColor.SaddleBrown);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000A5BF File Offset: 0x000095BF
		public static Color Salmon
		{
			get
			{
				return new Color(KnownColor.Salmon);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000A5CB File Offset: 0x000095CB
		public static Color SandyBrown
		{
			get
			{
				return new Color(KnownColor.SandyBrown);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000A5D7 File Offset: 0x000095D7
		public static Color SeaGreen
		{
			get
			{
				return new Color(KnownColor.SeaGreen);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000A5E3 File Offset: 0x000095E3
		public static Color SeaShell
		{
			get
			{
				return new Color(KnownColor.SeaShell);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000A5EF File Offset: 0x000095EF
		public static Color Sienna
		{
			get
			{
				return new Color(KnownColor.Sienna);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000A5FB File Offset: 0x000095FB
		public static Color Silver
		{
			get
			{
				return new Color(KnownColor.Silver);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000A607 File Offset: 0x00009607
		public static Color SkyBlue
		{
			get
			{
				return new Color(KnownColor.SkyBlue);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000A613 File Offset: 0x00009613
		public static Color SlateBlue
		{
			get
			{
				return new Color(KnownColor.SlateBlue);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000A61F File Offset: 0x0000961F
		public static Color SlateGray
		{
			get
			{
				return new Color(KnownColor.SlateGray);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000A62B File Offset: 0x0000962B
		public static Color Snow
		{
			get
			{
				return new Color(KnownColor.Snow);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000A637 File Offset: 0x00009637
		public static Color SpringGreen
		{
			get
			{
				return new Color(KnownColor.SpringGreen);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000A643 File Offset: 0x00009643
		public static Color SteelBlue
		{
			get
			{
				return new Color(KnownColor.SteelBlue);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000A64F File Offset: 0x0000964F
		public static Color Tan
		{
			get
			{
				return new Color(KnownColor.Tan);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000A65B File Offset: 0x0000965B
		public static Color Teal
		{
			get
			{
				return new Color(KnownColor.Teal);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000A667 File Offset: 0x00009667
		public static Color Thistle
		{
			get
			{
				return new Color(KnownColor.Thistle);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000A673 File Offset: 0x00009673
		public static Color Tomato
		{
			get
			{
				return new Color(KnownColor.Tomato);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000A67F File Offset: 0x0000967F
		public static Color Turquoise
		{
			get
			{
				return new Color(KnownColor.Turquoise);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000A68B File Offset: 0x0000968B
		public static Color Violet
		{
			get
			{
				return new Color(KnownColor.Violet);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000A697 File Offset: 0x00009697
		public static Color Wheat
		{
			get
			{
				return new Color(KnownColor.Wheat);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000A6A3 File Offset: 0x000096A3
		public static Color White
		{
			get
			{
				return new Color(KnownColor.White);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000A6AF File Offset: 0x000096AF
		public static Color WhiteSmoke
		{
			get
			{
				return new Color(KnownColor.WhiteSmoke);
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000A6BB File Offset: 0x000096BB
		public static Color Yellow
		{
			get
			{
				return new Color(KnownColor.Yellow);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000A6C7 File Offset: 0x000096C7
		public static Color YellowGreen
		{
			get
			{
				return new Color(KnownColor.YellowGreen);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000A6D3 File Offset: 0x000096D3
		internal Color(KnownColor knownColor)
		{
			this.value = 0L;
			this.state = Color.StateKnownColorValid;
			this.name = null;
			this.knownColor = (short)knownColor;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000A6F7 File Offset: 0x000096F7
		private Color(long value, short state, string name, KnownColor knownColor)
		{
			this.value = value;
			this.state = state;
			this.name = name;
			this.knownColor = (short)knownColor;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000A717 File Offset: 0x00009717
		public byte R
		{
			get
			{
				return (byte)((this.Value >> 16) & 255L);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000A72A File Offset: 0x0000972A
		public byte G
		{
			get
			{
				return (byte)((this.Value >> 8) & 255L);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000A73C File Offset: 0x0000973C
		public byte B
		{
			get
			{
				return (byte)(this.Value & 255L);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000A74C File Offset: 0x0000974C
		public byte A
		{
			get
			{
				return (byte)((this.Value >> 24) & 255L);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000A75F File Offset: 0x0000975F
		public bool IsKnownColor
		{
			get
			{
				return (this.state & Color.StateKnownColorValid) != 0;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000A773 File Offset: 0x00009773
		public bool IsEmpty
		{
			get
			{
				return this.state == 0;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000A77E File Offset: 0x0000977E
		public bool IsNamedColor
		{
			get
			{
				return (this.state & Color.StateNameValid) != 0 || this.IsKnownColor;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000A796 File Offset: 0x00009796
		public bool IsSystemColor
		{
			get
			{
				return this.IsKnownColor && (this.knownColor <= 26 || this.knownColor > 167);
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000A7BC File Offset: 0x000097BC
		private string NameAndARGBValue
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "{{Name={0}, ARGB=({1}, {2}, {3}, {4})}}", new object[] { this.Name, this.A, this.R, this.G, this.B });
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000A824 File Offset: 0x00009824
		public string Name
		{
			get
			{
				if ((this.state & Color.StateNameValid) != 0)
				{
					return this.name;
				}
				if (!this.IsKnownColor)
				{
					return Convert.ToString(this.value, 16);
				}
				string text = KnownColorTable.KnownColorToName((KnownColor)this.knownColor);
				if (text != null)
				{
					return text;
				}
				return ((KnownColor)this.knownColor).ToString();
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000A87D File Offset: 0x0000987D
		private long Value
		{
			get
			{
				if ((this.state & Color.StateValueMask) != 0)
				{
					return this.value;
				}
				if (this.IsKnownColor)
				{
					return (long)KnownColorTable.KnownColorToArgb((KnownColor)this.knownColor);
				}
				return Color.NotDefinedValue;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000A8B0 File Offset: 0x000098B0
		private static void CheckByte(int value, string name)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentException(SR.GetString("InvalidEx2BoundArgument", new object[] { name, value, 0, 255 }));
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000A904 File Offset: 0x00009904
		private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
		{
			return (long)((ulong)(((int)red << 16) | ((int)green << 8) | (int)blue | ((int)alpha << 24)) & (ulong)(-1));
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A919 File Offset: 0x00009919
		public static Color FromArgb(int argb)
		{
			return new Color((long)argb & (long)((ulong)(-1)), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A92C File Offset: 0x0000992C
		public static Color FromArgb(int alpha, int red, int green, int blue)
		{
			Color.CheckByte(alpha, "alpha");
			Color.CheckByte(red, "red");
			Color.CheckByte(green, "green");
			Color.CheckByte(blue, "blue");
			return new Color(Color.MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A97E File Offset: 0x0000997E
		public static Color FromArgb(int alpha, Color baseColor)
		{
			Color.CheckByte(alpha, "alpha");
			return new Color(Color.MakeArgb((byte)alpha, baseColor.R, baseColor.G, baseColor.B), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A9B3 File Offset: 0x000099B3
		public static Color FromArgb(int red, int green, int blue)
		{
			return Color.FromArgb(255, red, green, blue);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000A9C2 File Offset: 0x000099C2
		public static Color FromKnownColor(KnownColor color)
		{
			if (!ClientUtils.IsEnumValid(color, (int)color, 1, 174))
			{
				return Color.FromName(color.ToString());
			}
			return new Color(color);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000A9F0 File Offset: 0x000099F0
		public static Color FromName(string name)
		{
			object namedColor = ColorConverter.GetNamedColor(name);
			if (namedColor != null)
			{
				return (Color)namedColor;
			}
			return new Color(Color.NotDefinedValue, Color.StateNameValid, name, (KnownColor)0);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000AA20 File Offset: 0x00009A20
		public float GetBrightness()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = num;
			float num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			return (num4 + num5) / 2f;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000AA84 File Offset: 0x00009A84
		public float GetHue()
		{
			if (this.R == this.G && this.G == this.B)
			{
				return 0f;
			}
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = 0f;
			float num5 = num;
			float num6 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num2 < num6)
			{
				num6 = num2;
			}
			if (num3 < num6)
			{
				num6 = num3;
			}
			float num7 = num5 - num6;
			if (num == num5)
			{
				num4 = (num2 - num3) / num7;
			}
			else if (num2 == num5)
			{
				num4 = 2f + (num3 - num) / num7;
			}
			else if (num3 == num5)
			{
				num4 = 4f + (num - num2) / num7;
			}
			num4 *= 60f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			return num4;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000AB60 File Offset: 0x00009B60
		public float GetSaturation()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = 0f;
			float num5 = num;
			float num6 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num2 < num6)
			{
				num6 = num2;
			}
			if (num3 < num6)
			{
				num6 = num3;
			}
			if (num5 != num6)
			{
				float num7 = (num5 + num6) / 2f;
				if ((double)num7 <= 0.5)
				{
					num4 = (num5 - num6) / (num5 + num6);
				}
				else
				{
					num4 = (num5 - num6) / (2f - num5 - num6);
				}
			}
			return num4;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000ABFE File Offset: 0x00009BFE
		public int ToArgb()
		{
			return (int)this.Value;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000AC07 File Offset: 0x00009C07
		public KnownColor ToKnownColor()
		{
			return (KnownColor)this.knownColor;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000AC10 File Offset: 0x00009C10
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(" [");
			if ((this.state & Color.StateNameValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateKnownColorValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateValueMask) != 0)
			{
				stringBuilder.Append("A=");
				stringBuilder.Append(this.A);
				stringBuilder.Append(", R=");
				stringBuilder.Append(this.R);
				stringBuilder.Append(", G=");
				stringBuilder.Append(this.G);
				stringBuilder.Append(", B=");
				stringBuilder.Append(this.B);
			}
			else
			{
				stringBuilder.Append("Empty");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000AD20 File Offset: 0x00009D20
		public static bool operator ==(Color left, Color right)
		{
			return left.value == right.value && left.state == right.state && left.knownColor == right.knownColor && (left.name == right.name || (left.name != null && right.name != null && left.name.Equals(right.name)));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000AD9D File Offset: 0x00009D9D
		public static bool operator !=(Color left, Color right)
		{
			return !(left == right);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000ADAC File Offset: 0x00009DAC
		public override bool Equals(object obj)
		{
			if (obj is Color)
			{
				Color color = (Color)obj;
				if (this.value == color.value && this.state == color.state && this.knownColor == color.knownColor)
				{
					return this.name == color.name || (this.name != null && color.name != null && this.name.Equals(this.name));
				}
			}
			return false;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000AE34 File Offset: 0x00009E34
		public override int GetHashCode()
		{
			return this.value.GetHashCode() ^ this.state.GetHashCode() ^ this.knownColor.GetHashCode();
		}

		// Token: 0x0400026B RID: 619
		private const int ARGBAlphaShift = 24;

		// Token: 0x0400026C RID: 620
		private const int ARGBRedShift = 16;

		// Token: 0x0400026D RID: 621
		private const int ARGBGreenShift = 8;

		// Token: 0x0400026E RID: 622
		private const int ARGBBlueShift = 0;

		// Token: 0x0400026F RID: 623
		public static readonly Color Empty = default(Color);

		// Token: 0x04000270 RID: 624
		private static short StateKnownColorValid = 1;

		// Token: 0x04000271 RID: 625
		private static short StateARGBValueValid = 2;

		// Token: 0x04000272 RID: 626
		private static short StateValueMask = Color.StateARGBValueValid;

		// Token: 0x04000273 RID: 627
		private static short StateNameValid = 8;

		// Token: 0x04000274 RID: 628
		private static long NotDefinedValue = 0L;

		// Token: 0x04000275 RID: 629
		private readonly string name;

		// Token: 0x04000276 RID: 630
		private readonly long value;

		// Token: 0x04000277 RID: 631
		private readonly short knownColor;

		// Token: 0x04000278 RID: 632
		private readonly short state;
	}
}

using System;

namespace System.Drawing
{
	// Token: 0x02000058 RID: 88
	public sealed class Pens
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x00014995 File Offset: 0x00013995
		private Pens()
		{
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x000149A0 File Offset: 0x000139A0
		public static Pen Transparent
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.TransparentKey];
				if (pen == null)
				{
					pen = new Pen(Color.Transparent, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.TransparentKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000149E4 File Offset: 0x000139E4
		public static Pen AliceBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.AliceBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.AliceBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.AliceBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00014A28 File Offset: 0x00013A28
		public static Pen AntiqueWhite
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.AntiqueWhiteKey];
				if (pen == null)
				{
					pen = new Pen(Color.AntiqueWhite, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.AntiqueWhiteKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00014A6C File Offset: 0x00013A6C
		public static Pen Aqua
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.AquaKey];
				if (pen == null)
				{
					pen = new Pen(Color.Aqua, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.AquaKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00014AB0 File Offset: 0x00013AB0
		public static Pen Aquamarine
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.AquamarineKey];
				if (pen == null)
				{
					pen = new Pen(Color.Aquamarine, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.AquamarineKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00014AF4 File Offset: 0x00013AF4
		public static Pen Azure
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.AzureKey];
				if (pen == null)
				{
					pen = new Pen(Color.Azure, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.AzureKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00014B38 File Offset: 0x00013B38
		public static Pen Beige
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BeigeKey];
				if (pen == null)
				{
					pen = new Pen(Color.Beige, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BeigeKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00014B7C File Offset: 0x00013B7C
		public static Pen Bisque
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BisqueKey];
				if (pen == null)
				{
					pen = new Pen(Color.Bisque, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BisqueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00014BC0 File Offset: 0x00013BC0
		public static Pen Black
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BlackKey];
				if (pen == null)
				{
					pen = new Pen(Color.Black, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BlackKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00014C04 File Offset: 0x00013C04
		public static Pen BlanchedAlmond
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BlanchedAlmondKey];
				if (pen == null)
				{
					pen = new Pen(Color.BlanchedAlmond, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BlanchedAlmondKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00014C48 File Offset: 0x00013C48
		public static Pen Blue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.Blue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00014C8C File Offset: 0x00013C8C
		public static Pen BlueViolet
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BlueVioletKey];
				if (pen == null)
				{
					pen = new Pen(Color.BlueViolet, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BlueVioletKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00014CD0 File Offset: 0x00013CD0
		public static Pen Brown
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BrownKey];
				if (pen == null)
				{
					pen = new Pen(Color.Brown, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BrownKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00014D14 File Offset: 0x00013D14
		public static Pen BurlyWood
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.BurlyWoodKey];
				if (pen == null)
				{
					pen = new Pen(Color.BurlyWood, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.BurlyWoodKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00014D58 File Offset: 0x00013D58
		public static Pen CadetBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.CadetBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.CadetBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.CadetBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00014D9C File Offset: 0x00013D9C
		public static Pen Chartreuse
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.ChartreuseKey];
				if (pen == null)
				{
					pen = new Pen(Color.Chartreuse, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.ChartreuseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00014DE0 File Offset: 0x00013DE0
		public static Pen Chocolate
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.ChocolateKey];
				if (pen == null)
				{
					pen = new Pen(Color.Chocolate, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.ChocolateKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00014E24 File Offset: 0x00013E24
		public static Pen Coral
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.ChoralKey];
				if (pen == null)
				{
					pen = new Pen(Color.Coral, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.ChoralKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00014E68 File Offset: 0x00013E68
		public static Pen CornflowerBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.CornflowerBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.CornflowerBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.CornflowerBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00014EAC File Offset: 0x00013EAC
		public static Pen Cornsilk
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.CornsilkKey];
				if (pen == null)
				{
					pen = new Pen(Color.Cornsilk, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.CornsilkKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00014EF0 File Offset: 0x00013EF0
		public static Pen Crimson
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.CrimsonKey];
				if (pen == null)
				{
					pen = new Pen(Color.Crimson, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.CrimsonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00014F34 File Offset: 0x00013F34
		public static Pen Cyan
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.CyanKey];
				if (pen == null)
				{
					pen = new Pen(Color.Cyan, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.CyanKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00014F78 File Offset: 0x00013F78
		public static Pen DarkBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00014FBC File Offset: 0x00013FBC
		public static Pen DarkCyan
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkCyanKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkCyan, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkCyanKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00015000 File Offset: 0x00014000
		public static Pen DarkGoldenrod
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkGoldenrodKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkGoldenrod, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkGoldenrodKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00015044 File Offset: 0x00014044
		public static Pen DarkGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00015088 File Offset: 0x00014088
		public static Pen DarkGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000150CC File Offset: 0x000140CC
		public static Pen DarkKhaki
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkKhakiKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkKhaki, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkKhakiKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00015110 File Offset: 0x00014110
		public static Pen DarkMagenta
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkMagentaKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkMagenta, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkMagentaKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00015154 File Offset: 0x00014154
		public static Pen DarkOliveGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkOliveGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkOliveGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkOliveGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00015198 File Offset: 0x00014198
		public static Pen DarkOrange
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkOrangeKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkOrange, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkOrangeKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x000151DC File Offset: 0x000141DC
		public static Pen DarkOrchid
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkOrchidKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkOrchid, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkOrchidKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00015220 File Offset: 0x00014220
		public static Pen DarkRed
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkRedKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkRed, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkRedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00015264 File Offset: 0x00014264
		public static Pen DarkSalmon
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkSalmonKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkSalmon, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkSalmonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x000152A8 File Offset: 0x000142A8
		public static Pen DarkSeaGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkSeaGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkSeaGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkSeaGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x000152EC File Offset: 0x000142EC
		public static Pen DarkSlateBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkSlateBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkSlateBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkSlateBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00015330 File Offset: 0x00014330
		public static Pen DarkSlateGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkSlateGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkSlateGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkSlateGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00015374 File Offset: 0x00014374
		public static Pen DarkTurquoise
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkTurquoiseKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkTurquoise, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkTurquoiseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x000153B8 File Offset: 0x000143B8
		public static Pen DarkViolet
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DarkVioletKey];
				if (pen == null)
				{
					pen = new Pen(Color.DarkViolet, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DarkVioletKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x000153FC File Offset: 0x000143FC
		public static Pen DeepPink
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DeepPinkKey];
				if (pen == null)
				{
					pen = new Pen(Color.DeepPink, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DeepPinkKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00015440 File Offset: 0x00014440
		public static Pen DeepSkyBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DeepSkyBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.DeepSkyBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DeepSkyBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00015484 File Offset: 0x00014484
		public static Pen DimGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DimGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.DimGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DimGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x000154C8 File Offset: 0x000144C8
		public static Pen DodgerBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.DodgerBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.DodgerBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.DodgerBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001550C File Offset: 0x0001450C
		public static Pen Firebrick
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.FirebrickKey];
				if (pen == null)
				{
					pen = new Pen(Color.Firebrick, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.FirebrickKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00015550 File Offset: 0x00014550
		public static Pen FloralWhite
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.FloralWhiteKey];
				if (pen == null)
				{
					pen = new Pen(Color.FloralWhite, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.FloralWhiteKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00015594 File Offset: 0x00014594
		public static Pen ForestGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.ForestGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.ForestGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.ForestGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000155D8 File Offset: 0x000145D8
		public static Pen Fuchsia
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.FuchiaKey];
				if (pen == null)
				{
					pen = new Pen(Color.Fuchsia, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.FuchiaKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001561C File Offset: 0x0001461C
		public static Pen Gainsboro
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GainsboroKey];
				if (pen == null)
				{
					pen = new Pen(Color.Gainsboro, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GainsboroKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00015660 File Offset: 0x00014660
		public static Pen GhostWhite
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GhostWhiteKey];
				if (pen == null)
				{
					pen = new Pen(Color.GhostWhite, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GhostWhiteKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000156A4 File Offset: 0x000146A4
		public static Pen Gold
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GoldKey];
				if (pen == null)
				{
					pen = new Pen(Color.Gold, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GoldKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000156E8 File Offset: 0x000146E8
		public static Pen Goldenrod
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GoldenrodKey];
				if (pen == null)
				{
					pen = new Pen(Color.Goldenrod, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GoldenrodKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001572C File Offset: 0x0001472C
		public static Pen Gray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.Gray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00015770 File Offset: 0x00014770
		public static Pen Green
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.Green, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x000157B4 File Offset: 0x000147B4
		public static Pen GreenYellow
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.GreenYellowKey];
				if (pen == null)
				{
					pen = new Pen(Color.GreenYellow, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.GreenYellowKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000157F8 File Offset: 0x000147F8
		public static Pen Honeydew
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.HoneydewKey];
				if (pen == null)
				{
					pen = new Pen(Color.Honeydew, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.HoneydewKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001583C File Offset: 0x0001483C
		public static Pen HotPink
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.HotPinkKey];
				if (pen == null)
				{
					pen = new Pen(Color.HotPink, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.HotPinkKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00015880 File Offset: 0x00014880
		public static Pen IndianRed
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.IndianRedKey];
				if (pen == null)
				{
					pen = new Pen(Color.IndianRed, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.IndianRedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x000158C4 File Offset: 0x000148C4
		public static Pen Indigo
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.IndigoKey];
				if (pen == null)
				{
					pen = new Pen(Color.Indigo, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.IndigoKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00015908 File Offset: 0x00014908
		public static Pen Ivory
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.IvoryKey];
				if (pen == null)
				{
					pen = new Pen(Color.Ivory, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.IvoryKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001594C File Offset: 0x0001494C
		public static Pen Khaki
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.KhakiKey];
				if (pen == null)
				{
					pen = new Pen(Color.Khaki, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.KhakiKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00015990 File Offset: 0x00014990
		public static Pen Lavender
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LavenderKey];
				if (pen == null)
				{
					pen = new Pen(Color.Lavender, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LavenderKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x000159D4 File Offset: 0x000149D4
		public static Pen LavenderBlush
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LavenderBlushKey];
				if (pen == null)
				{
					pen = new Pen(Color.LavenderBlush, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LavenderBlushKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00015A18 File Offset: 0x00014A18
		public static Pen LawnGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LawnGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.LawnGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LawnGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00015A5C File Offset: 0x00014A5C
		public static Pen LemonChiffon
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LemonChiffonKey];
				if (pen == null)
				{
					pen = new Pen(Color.LemonChiffon, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LemonChiffonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00015AA0 File Offset: 0x00014AA0
		public static Pen LightBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00015AE4 File Offset: 0x00014AE4
		public static Pen LightCoral
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightCoralKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightCoral, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightCoralKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00015B28 File Offset: 0x00014B28
		public static Pen LightCyan
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightCyanKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightCyan, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightCyanKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00015B6C File Offset: 0x00014B6C
		public static Pen LightGoldenrodYellow
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightGoldenrodYellowKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightGoldenrodYellow, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightGoldenrodYellowKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00015BB0 File Offset: 0x00014BB0
		public static Pen LightGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00015BF4 File Offset: 0x00014BF4
		public static Pen LightGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00015C38 File Offset: 0x00014C38
		public static Pen LightPink
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightPinkKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightPink, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightPinkKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00015C7C File Offset: 0x00014C7C
		public static Pen LightSalmon
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightSalmonKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightSalmon, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightSalmonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00015CC0 File Offset: 0x00014CC0
		public static Pen LightSeaGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightSeaGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightSeaGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightSeaGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00015D04 File Offset: 0x00014D04
		public static Pen LightSkyBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightSkyBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightSkyBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightSkyBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00015D48 File Offset: 0x00014D48
		public static Pen LightSlateGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightSlateGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightSlateGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightSlateGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00015D8C File Offset: 0x00014D8C
		public static Pen LightSteelBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightSteelBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightSteelBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightSteelBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00015DD0 File Offset: 0x00014DD0
		public static Pen LightYellow
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LightYellowKey];
				if (pen == null)
				{
					pen = new Pen(Color.LightYellow, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LightYellowKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00015E14 File Offset: 0x00014E14
		public static Pen Lime
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LimeKey];
				if (pen == null)
				{
					pen = new Pen(Color.Lime, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LimeKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00015E58 File Offset: 0x00014E58
		public static Pen LimeGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LimeGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.LimeGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LimeGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00015E9C File Offset: 0x00014E9C
		public static Pen Linen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.LinenKey];
				if (pen == null)
				{
					pen = new Pen(Color.Linen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.LinenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00015EE0 File Offset: 0x00014EE0
		public static Pen Magenta
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MagentaKey];
				if (pen == null)
				{
					pen = new Pen(Color.Magenta, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MagentaKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00015F24 File Offset: 0x00014F24
		public static Pen Maroon
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MaroonKey];
				if (pen == null)
				{
					pen = new Pen(Color.Maroon, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MaroonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00015F68 File Offset: 0x00014F68
		public static Pen MediumAquamarine
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumAquamarineKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumAquamarine, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumAquamarineKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00015FAC File Offset: 0x00014FAC
		public static Pen MediumBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00015FF0 File Offset: 0x00014FF0
		public static Pen MediumOrchid
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumOrchidKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumOrchid, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumOrchidKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00016034 File Offset: 0x00015034
		public static Pen MediumPurple
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumPurpleKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumPurple, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumPurpleKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00016078 File Offset: 0x00015078
		public static Pen MediumSeaGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumSeaGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumSeaGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumSeaGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x000160BC File Offset: 0x000150BC
		public static Pen MediumSlateBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumSlateBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumSlateBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumSlateBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00016100 File Offset: 0x00015100
		public static Pen MediumSpringGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumSpringGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumSpringGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumSpringGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00016144 File Offset: 0x00015144
		public static Pen MediumTurquoise
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumTurquoiseKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumTurquoise, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumTurquoiseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00016188 File Offset: 0x00015188
		public static Pen MediumVioletRed
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MediumVioletRedKey];
				if (pen == null)
				{
					pen = new Pen(Color.MediumVioletRed, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MediumVioletRedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x000161CC File Offset: 0x000151CC
		public static Pen MidnightBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MidnightBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.MidnightBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MidnightBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00016210 File Offset: 0x00015210
		public static Pen MintCream
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MintCreamKey];
				if (pen == null)
				{
					pen = new Pen(Color.MintCream, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MintCreamKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00016254 File Offset: 0x00015254
		public static Pen MistyRose
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MistyRoseKey];
				if (pen == null)
				{
					pen = new Pen(Color.MistyRose, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MistyRoseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00016298 File Offset: 0x00015298
		public static Pen Moccasin
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.MoccasinKey];
				if (pen == null)
				{
					pen = new Pen(Color.Moccasin, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.MoccasinKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000162DC File Offset: 0x000152DC
		public static Pen NavajoWhite
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.NavajoWhiteKey];
				if (pen == null)
				{
					pen = new Pen(Color.NavajoWhite, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.NavajoWhiteKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00016320 File Offset: 0x00015320
		public static Pen Navy
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.NavyKey];
				if (pen == null)
				{
					pen = new Pen(Color.Navy, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.NavyKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00016364 File Offset: 0x00015364
		public static Pen OldLace
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OldLaceKey];
				if (pen == null)
				{
					pen = new Pen(Color.OldLace, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OldLaceKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000163A8 File Offset: 0x000153A8
		public static Pen Olive
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OliveKey];
				if (pen == null)
				{
					pen = new Pen(Color.Olive, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OliveKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000163EC File Offset: 0x000153EC
		public static Pen OliveDrab
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OliveDrabKey];
				if (pen == null)
				{
					pen = new Pen(Color.OliveDrab, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OliveDrabKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00016430 File Offset: 0x00015430
		public static Pen Orange
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OrangeKey];
				if (pen == null)
				{
					pen = new Pen(Color.Orange, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OrangeKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00016474 File Offset: 0x00015474
		public static Pen OrangeRed
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OrangeRedKey];
				if (pen == null)
				{
					pen = new Pen(Color.OrangeRed, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OrangeRedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000164B8 File Offset: 0x000154B8
		public static Pen Orchid
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.OrchidKey];
				if (pen == null)
				{
					pen = new Pen(Color.Orchid, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.OrchidKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x000164FC File Offset: 0x000154FC
		public static Pen PaleGoldenrod
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PaleGoldenrodKey];
				if (pen == null)
				{
					pen = new Pen(Color.PaleGoldenrod, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PaleGoldenrodKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00016540 File Offset: 0x00015540
		public static Pen PaleGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PaleGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.PaleGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PaleGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00016584 File Offset: 0x00015584
		public static Pen PaleTurquoise
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PaleTurquoiseKey];
				if (pen == null)
				{
					pen = new Pen(Color.PaleTurquoise, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PaleTurquoiseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x000165C8 File Offset: 0x000155C8
		public static Pen PaleVioletRed
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PaleVioletRedKey];
				if (pen == null)
				{
					pen = new Pen(Color.PaleVioletRed, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PaleVioletRedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001660C File Offset: 0x0001560C
		public static Pen PapayaWhip
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PapayaWhipKey];
				if (pen == null)
				{
					pen = new Pen(Color.PapayaWhip, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PapayaWhipKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00016650 File Offset: 0x00015650
		public static Pen PeachPuff
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PeachPuffKey];
				if (pen == null)
				{
					pen = new Pen(Color.PeachPuff, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PeachPuffKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00016694 File Offset: 0x00015694
		public static Pen Peru
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PeruKey];
				if (pen == null)
				{
					pen = new Pen(Color.Peru, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PeruKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000166D8 File Offset: 0x000156D8
		public static Pen Pink
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PinkKey];
				if (pen == null)
				{
					pen = new Pen(Color.Pink, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PinkKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001671C File Offset: 0x0001571C
		public static Pen Plum
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PlumKey];
				if (pen == null)
				{
					pen = new Pen(Color.Plum, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PlumKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00016760 File Offset: 0x00015760
		public static Pen PowderBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PowderBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.PowderBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PowderBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000167A4 File Offset: 0x000157A4
		public static Pen Purple
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.PurpleKey];
				if (pen == null)
				{
					pen = new Pen(Color.Purple, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.PurpleKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000167E8 File Offset: 0x000157E8
		public static Pen Red
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.RedKey];
				if (pen == null)
				{
					pen = new Pen(Color.Red, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.RedKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001682C File Offset: 0x0001582C
		public static Pen RosyBrown
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.RosyBrownKey];
				if (pen == null)
				{
					pen = new Pen(Color.RosyBrown, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.RosyBrownKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00016870 File Offset: 0x00015870
		public static Pen RoyalBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.RoyalBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.RoyalBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.RoyalBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000168B4 File Offset: 0x000158B4
		public static Pen SaddleBrown
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SaddleBrownKey];
				if (pen == null)
				{
					pen = new Pen(Color.SaddleBrown, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SaddleBrownKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000168F8 File Offset: 0x000158F8
		public static Pen Salmon
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SalmonKey];
				if (pen == null)
				{
					pen = new Pen(Color.Salmon, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SalmonKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001693C File Offset: 0x0001593C
		public static Pen SandyBrown
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SandyBrownKey];
				if (pen == null)
				{
					pen = new Pen(Color.SandyBrown, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SandyBrownKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00016980 File Offset: 0x00015980
		public static Pen SeaGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SeaGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.SeaGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SeaGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x000169C4 File Offset: 0x000159C4
		public static Pen SeaShell
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SeaShellKey];
				if (pen == null)
				{
					pen = new Pen(Color.SeaShell, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SeaShellKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00016A08 File Offset: 0x00015A08
		public static Pen Sienna
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SiennaKey];
				if (pen == null)
				{
					pen = new Pen(Color.Sienna, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SiennaKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00016A4C File Offset: 0x00015A4C
		public static Pen Silver
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SilverKey];
				if (pen == null)
				{
					pen = new Pen(Color.Silver, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SilverKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00016A90 File Offset: 0x00015A90
		public static Pen SkyBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SkyBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.SkyBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SkyBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00016AD4 File Offset: 0x00015AD4
		public static Pen SlateBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SlateBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.SlateBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SlateBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00016B18 File Offset: 0x00015B18
		public static Pen SlateGray
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SlateGrayKey];
				if (pen == null)
				{
					pen = new Pen(Color.SlateGray, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SlateGrayKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00016B5C File Offset: 0x00015B5C
		public static Pen Snow
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SnowKey];
				if (pen == null)
				{
					pen = new Pen(Color.Snow, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SnowKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00016BA0 File Offset: 0x00015BA0
		public static Pen SpringGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SpringGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.SpringGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SpringGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00016BE4 File Offset: 0x00015BE4
		public static Pen SteelBlue
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.SteelBlueKey];
				if (pen == null)
				{
					pen = new Pen(Color.SteelBlue, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.SteelBlueKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00016C28 File Offset: 0x00015C28
		public static Pen Tan
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.TanKey];
				if (pen == null)
				{
					pen = new Pen(Color.Tan, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.TanKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00016C6C File Offset: 0x00015C6C
		public static Pen Teal
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.TealKey];
				if (pen == null)
				{
					pen = new Pen(Color.Teal, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.TealKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x00016CB0 File Offset: 0x00015CB0
		public static Pen Thistle
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.ThistleKey];
				if (pen == null)
				{
					pen = new Pen(Color.Thistle, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.ThistleKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00016CF4 File Offset: 0x00015CF4
		public static Pen Tomato
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.TomatoKey];
				if (pen == null)
				{
					pen = new Pen(Color.Tomato, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.TomatoKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00016D38 File Offset: 0x00015D38
		public static Pen Turquoise
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.TurquoiseKey];
				if (pen == null)
				{
					pen = new Pen(Color.Turquoise, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.TurquoiseKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00016D7C File Offset: 0x00015D7C
		public static Pen Violet
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.VioletKey];
				if (pen == null)
				{
					pen = new Pen(Color.Violet, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.VioletKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00016DC0 File Offset: 0x00015DC0
		public static Pen Wheat
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.WheatKey];
				if (pen == null)
				{
					pen = new Pen(Color.Wheat, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.WheatKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00016E04 File Offset: 0x00015E04
		public static Pen White
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.WhiteKey];
				if (pen == null)
				{
					pen = new Pen(Color.White, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.WhiteKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00016E48 File Offset: 0x00015E48
		public static Pen WhiteSmoke
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.WhiteSmokeKey];
				if (pen == null)
				{
					pen = new Pen(Color.WhiteSmoke, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.WhiteSmokeKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00016E8C File Offset: 0x00015E8C
		public static Pen Yellow
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.YellowKey];
				if (pen == null)
				{
					pen = new Pen(Color.Yellow, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.YellowKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00016ED0 File Offset: 0x00015ED0
		public static Pen YellowGreen
		{
			get
			{
				Pen pen = (Pen)SafeNativeMethods.Gdip.ThreadData[Pens.YellowGreenKey];
				if (pen == null)
				{
					pen = new Pen(Color.YellowGreen, true);
					SafeNativeMethods.Gdip.ThreadData[Pens.YellowGreenKey] = pen;
				}
				return pen;
			}
		}

		// Token: 0x040003CF RID: 975
		private static readonly object TransparentKey = new object();

		// Token: 0x040003D0 RID: 976
		private static readonly object AliceBlueKey = new object();

		// Token: 0x040003D1 RID: 977
		private static readonly object AntiqueWhiteKey = new object();

		// Token: 0x040003D2 RID: 978
		private static readonly object AquaKey = new object();

		// Token: 0x040003D3 RID: 979
		private static readonly object AquamarineKey = new object();

		// Token: 0x040003D4 RID: 980
		private static readonly object AzureKey = new object();

		// Token: 0x040003D5 RID: 981
		private static readonly object BeigeKey = new object();

		// Token: 0x040003D6 RID: 982
		private static readonly object BisqueKey = new object();

		// Token: 0x040003D7 RID: 983
		private static readonly object BlackKey = new object();

		// Token: 0x040003D8 RID: 984
		private static readonly object BlanchedAlmondKey = new object();

		// Token: 0x040003D9 RID: 985
		private static readonly object BlueKey = new object();

		// Token: 0x040003DA RID: 986
		private static readonly object BlueVioletKey = new object();

		// Token: 0x040003DB RID: 987
		private static readonly object BrownKey = new object();

		// Token: 0x040003DC RID: 988
		private static readonly object BurlyWoodKey = new object();

		// Token: 0x040003DD RID: 989
		private static readonly object CadetBlueKey = new object();

		// Token: 0x040003DE RID: 990
		private static readonly object ChartreuseKey = new object();

		// Token: 0x040003DF RID: 991
		private static readonly object ChocolateKey = new object();

		// Token: 0x040003E0 RID: 992
		private static readonly object ChoralKey = new object();

		// Token: 0x040003E1 RID: 993
		private static readonly object CornflowerBlueKey = new object();

		// Token: 0x040003E2 RID: 994
		private static readonly object CornsilkKey = new object();

		// Token: 0x040003E3 RID: 995
		private static readonly object CrimsonKey = new object();

		// Token: 0x040003E4 RID: 996
		private static readonly object CyanKey = new object();

		// Token: 0x040003E5 RID: 997
		private static readonly object DarkBlueKey = new object();

		// Token: 0x040003E6 RID: 998
		private static readonly object DarkCyanKey = new object();

		// Token: 0x040003E7 RID: 999
		private static readonly object DarkGoldenrodKey = new object();

		// Token: 0x040003E8 RID: 1000
		private static readonly object DarkGrayKey = new object();

		// Token: 0x040003E9 RID: 1001
		private static readonly object DarkGreenKey = new object();

		// Token: 0x040003EA RID: 1002
		private static readonly object DarkKhakiKey = new object();

		// Token: 0x040003EB RID: 1003
		private static readonly object DarkMagentaKey = new object();

		// Token: 0x040003EC RID: 1004
		private static readonly object DarkOliveGreenKey = new object();

		// Token: 0x040003ED RID: 1005
		private static readonly object DarkOrangeKey = new object();

		// Token: 0x040003EE RID: 1006
		private static readonly object DarkOrchidKey = new object();

		// Token: 0x040003EF RID: 1007
		private static readonly object DarkRedKey = new object();

		// Token: 0x040003F0 RID: 1008
		private static readonly object DarkSalmonKey = new object();

		// Token: 0x040003F1 RID: 1009
		private static readonly object DarkSeaGreenKey = new object();

		// Token: 0x040003F2 RID: 1010
		private static readonly object DarkSlateBlueKey = new object();

		// Token: 0x040003F3 RID: 1011
		private static readonly object DarkSlateGrayKey = new object();

		// Token: 0x040003F4 RID: 1012
		private static readonly object DarkTurquoiseKey = new object();

		// Token: 0x040003F5 RID: 1013
		private static readonly object DarkVioletKey = new object();

		// Token: 0x040003F6 RID: 1014
		private static readonly object DeepPinkKey = new object();

		// Token: 0x040003F7 RID: 1015
		private static readonly object DeepSkyBlueKey = new object();

		// Token: 0x040003F8 RID: 1016
		private static readonly object DimGrayKey = new object();

		// Token: 0x040003F9 RID: 1017
		private static readonly object DodgerBlueKey = new object();

		// Token: 0x040003FA RID: 1018
		private static readonly object FirebrickKey = new object();

		// Token: 0x040003FB RID: 1019
		private static readonly object FloralWhiteKey = new object();

		// Token: 0x040003FC RID: 1020
		private static readonly object ForestGreenKey = new object();

		// Token: 0x040003FD RID: 1021
		private static readonly object FuchiaKey = new object();

		// Token: 0x040003FE RID: 1022
		private static readonly object GainsboroKey = new object();

		// Token: 0x040003FF RID: 1023
		private static readonly object GhostWhiteKey = new object();

		// Token: 0x04000400 RID: 1024
		private static readonly object GoldKey = new object();

		// Token: 0x04000401 RID: 1025
		private static readonly object GoldenrodKey = new object();

		// Token: 0x04000402 RID: 1026
		private static readonly object GrayKey = new object();

		// Token: 0x04000403 RID: 1027
		private static readonly object GreenKey = new object();

		// Token: 0x04000404 RID: 1028
		private static readonly object GreenYellowKey = new object();

		// Token: 0x04000405 RID: 1029
		private static readonly object HoneydewKey = new object();

		// Token: 0x04000406 RID: 1030
		private static readonly object HotPinkKey = new object();

		// Token: 0x04000407 RID: 1031
		private static readonly object IndianRedKey = new object();

		// Token: 0x04000408 RID: 1032
		private static readonly object IndigoKey = new object();

		// Token: 0x04000409 RID: 1033
		private static readonly object IvoryKey = new object();

		// Token: 0x0400040A RID: 1034
		private static readonly object KhakiKey = new object();

		// Token: 0x0400040B RID: 1035
		private static readonly object LavenderKey = new object();

		// Token: 0x0400040C RID: 1036
		private static readonly object LavenderBlushKey = new object();

		// Token: 0x0400040D RID: 1037
		private static readonly object LawnGreenKey = new object();

		// Token: 0x0400040E RID: 1038
		private static readonly object LemonChiffonKey = new object();

		// Token: 0x0400040F RID: 1039
		private static readonly object LightBlueKey = new object();

		// Token: 0x04000410 RID: 1040
		private static readonly object LightCoralKey = new object();

		// Token: 0x04000411 RID: 1041
		private static readonly object LightCyanKey = new object();

		// Token: 0x04000412 RID: 1042
		private static readonly object LightGoldenrodYellowKey = new object();

		// Token: 0x04000413 RID: 1043
		private static readonly object LightGreenKey = new object();

		// Token: 0x04000414 RID: 1044
		private static readonly object LightGrayKey = new object();

		// Token: 0x04000415 RID: 1045
		private static readonly object LightPinkKey = new object();

		// Token: 0x04000416 RID: 1046
		private static readonly object LightSalmonKey = new object();

		// Token: 0x04000417 RID: 1047
		private static readonly object LightSeaGreenKey = new object();

		// Token: 0x04000418 RID: 1048
		private static readonly object LightSkyBlueKey = new object();

		// Token: 0x04000419 RID: 1049
		private static readonly object LightSlateGrayKey = new object();

		// Token: 0x0400041A RID: 1050
		private static readonly object LightSteelBlueKey = new object();

		// Token: 0x0400041B RID: 1051
		private static readonly object LightYellowKey = new object();

		// Token: 0x0400041C RID: 1052
		private static readonly object LimeKey = new object();

		// Token: 0x0400041D RID: 1053
		private static readonly object LimeGreenKey = new object();

		// Token: 0x0400041E RID: 1054
		private static readonly object LinenKey = new object();

		// Token: 0x0400041F RID: 1055
		private static readonly object MagentaKey = new object();

		// Token: 0x04000420 RID: 1056
		private static readonly object MaroonKey = new object();

		// Token: 0x04000421 RID: 1057
		private static readonly object MediumAquamarineKey = new object();

		// Token: 0x04000422 RID: 1058
		private static readonly object MediumBlueKey = new object();

		// Token: 0x04000423 RID: 1059
		private static readonly object MediumOrchidKey = new object();

		// Token: 0x04000424 RID: 1060
		private static readonly object MediumPurpleKey = new object();

		// Token: 0x04000425 RID: 1061
		private static readonly object MediumSeaGreenKey = new object();

		// Token: 0x04000426 RID: 1062
		private static readonly object MediumSlateBlueKey = new object();

		// Token: 0x04000427 RID: 1063
		private static readonly object MediumSpringGreenKey = new object();

		// Token: 0x04000428 RID: 1064
		private static readonly object MediumTurquoiseKey = new object();

		// Token: 0x04000429 RID: 1065
		private static readonly object MediumVioletRedKey = new object();

		// Token: 0x0400042A RID: 1066
		private static readonly object MidnightBlueKey = new object();

		// Token: 0x0400042B RID: 1067
		private static readonly object MintCreamKey = new object();

		// Token: 0x0400042C RID: 1068
		private static readonly object MistyRoseKey = new object();

		// Token: 0x0400042D RID: 1069
		private static readonly object MoccasinKey = new object();

		// Token: 0x0400042E RID: 1070
		private static readonly object NavajoWhiteKey = new object();

		// Token: 0x0400042F RID: 1071
		private static readonly object NavyKey = new object();

		// Token: 0x04000430 RID: 1072
		private static readonly object OldLaceKey = new object();

		// Token: 0x04000431 RID: 1073
		private static readonly object OliveKey = new object();

		// Token: 0x04000432 RID: 1074
		private static readonly object OliveDrabKey = new object();

		// Token: 0x04000433 RID: 1075
		private static readonly object OrangeKey = new object();

		// Token: 0x04000434 RID: 1076
		private static readonly object OrangeRedKey = new object();

		// Token: 0x04000435 RID: 1077
		private static readonly object OrchidKey = new object();

		// Token: 0x04000436 RID: 1078
		private static readonly object PaleGoldenrodKey = new object();

		// Token: 0x04000437 RID: 1079
		private static readonly object PaleGreenKey = new object();

		// Token: 0x04000438 RID: 1080
		private static readonly object PaleTurquoiseKey = new object();

		// Token: 0x04000439 RID: 1081
		private static readonly object PaleVioletRedKey = new object();

		// Token: 0x0400043A RID: 1082
		private static readonly object PapayaWhipKey = new object();

		// Token: 0x0400043B RID: 1083
		private static readonly object PeachPuffKey = new object();

		// Token: 0x0400043C RID: 1084
		private static readonly object PeruKey = new object();

		// Token: 0x0400043D RID: 1085
		private static readonly object PinkKey = new object();

		// Token: 0x0400043E RID: 1086
		private static readonly object PlumKey = new object();

		// Token: 0x0400043F RID: 1087
		private static readonly object PowderBlueKey = new object();

		// Token: 0x04000440 RID: 1088
		private static readonly object PurpleKey = new object();

		// Token: 0x04000441 RID: 1089
		private static readonly object RedKey = new object();

		// Token: 0x04000442 RID: 1090
		private static readonly object RosyBrownKey = new object();

		// Token: 0x04000443 RID: 1091
		private static readonly object RoyalBlueKey = new object();

		// Token: 0x04000444 RID: 1092
		private static readonly object SaddleBrownKey = new object();

		// Token: 0x04000445 RID: 1093
		private static readonly object SalmonKey = new object();

		// Token: 0x04000446 RID: 1094
		private static readonly object SandyBrownKey = new object();

		// Token: 0x04000447 RID: 1095
		private static readonly object SeaGreenKey = new object();

		// Token: 0x04000448 RID: 1096
		private static readonly object SeaShellKey = new object();

		// Token: 0x04000449 RID: 1097
		private static readonly object SiennaKey = new object();

		// Token: 0x0400044A RID: 1098
		private static readonly object SilverKey = new object();

		// Token: 0x0400044B RID: 1099
		private static readonly object SkyBlueKey = new object();

		// Token: 0x0400044C RID: 1100
		private static readonly object SlateBlueKey = new object();

		// Token: 0x0400044D RID: 1101
		private static readonly object SlateGrayKey = new object();

		// Token: 0x0400044E RID: 1102
		private static readonly object SnowKey = new object();

		// Token: 0x0400044F RID: 1103
		private static readonly object SpringGreenKey = new object();

		// Token: 0x04000450 RID: 1104
		private static readonly object SteelBlueKey = new object();

		// Token: 0x04000451 RID: 1105
		private static readonly object TanKey = new object();

		// Token: 0x04000452 RID: 1106
		private static readonly object TealKey = new object();

		// Token: 0x04000453 RID: 1107
		private static readonly object ThistleKey = new object();

		// Token: 0x04000454 RID: 1108
		private static readonly object TomatoKey = new object();

		// Token: 0x04000455 RID: 1109
		private static readonly object TurquoiseKey = new object();

		// Token: 0x04000456 RID: 1110
		private static readonly object VioletKey = new object();

		// Token: 0x04000457 RID: 1111
		private static readonly object WheatKey = new object();

		// Token: 0x04000458 RID: 1112
		private static readonly object WhiteKey = new object();

		// Token: 0x04000459 RID: 1113
		private static readonly object WhiteSmokeKey = new object();

		// Token: 0x0400045A RID: 1114
		private static readonly object YellowKey = new object();

		// Token: 0x0400045B RID: 1115
		private static readonly object YellowGreenKey = new object();
	}
}

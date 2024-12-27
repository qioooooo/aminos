﻿using System;

namespace System.Xml
{
	// Token: 0x020000F3 RID: 243
	internal enum BinXmlToken
	{
		// Token: 0x040009B0 RID: 2480
		Error,
		// Token: 0x040009B1 RID: 2481
		NotImpl = -2,
		// Token: 0x040009B2 RID: 2482
		EOF,
		// Token: 0x040009B3 RID: 2483
		XmlDecl = 254,
		// Token: 0x040009B4 RID: 2484
		Encoding = 253,
		// Token: 0x040009B5 RID: 2485
		DocType = 252,
		// Token: 0x040009B6 RID: 2486
		System = 251,
		// Token: 0x040009B7 RID: 2487
		Public = 250,
		// Token: 0x040009B8 RID: 2488
		Subset = 249,
		// Token: 0x040009B9 RID: 2489
		Element = 248,
		// Token: 0x040009BA RID: 2490
		EndElem = 247,
		// Token: 0x040009BB RID: 2491
		Attr = 246,
		// Token: 0x040009BC RID: 2492
		EndAttrs = 245,
		// Token: 0x040009BD RID: 2493
		PI = 244,
		// Token: 0x040009BE RID: 2494
		Comment = 243,
		// Token: 0x040009BF RID: 2495
		CData = 242,
		// Token: 0x040009C0 RID: 2496
		EndCData = 241,
		// Token: 0x040009C1 RID: 2497
		Name = 240,
		// Token: 0x040009C2 RID: 2498
		QName = 239,
		// Token: 0x040009C3 RID: 2499
		XmlText = 237,
		// Token: 0x040009C4 RID: 2500
		Nest = 236,
		// Token: 0x040009C5 RID: 2501
		EndNest = 235,
		// Token: 0x040009C6 RID: 2502
		Extn = 234,
		// Token: 0x040009C7 RID: 2503
		NmFlush = 233,
		// Token: 0x040009C8 RID: 2504
		SQL_BIT = 6,
		// Token: 0x040009C9 RID: 2505
		SQL_TINYINT,
		// Token: 0x040009CA RID: 2506
		SQL_SMALLINT = 1,
		// Token: 0x040009CB RID: 2507
		SQL_INT,
		// Token: 0x040009CC RID: 2508
		SQL_BIGINT = 8,
		// Token: 0x040009CD RID: 2509
		SQL_REAL = 3,
		// Token: 0x040009CE RID: 2510
		SQL_FLOAT,
		// Token: 0x040009CF RID: 2511
		SQL_MONEY,
		// Token: 0x040009D0 RID: 2512
		SQL_SMALLMONEY = 20,
		// Token: 0x040009D1 RID: 2513
		SQL_DATETIME = 18,
		// Token: 0x040009D2 RID: 2514
		SQL_SMALLDATETIME,
		// Token: 0x040009D3 RID: 2515
		SQL_DECIMAL = 10,
		// Token: 0x040009D4 RID: 2516
		SQL_NUMERIC,
		// Token: 0x040009D5 RID: 2517
		SQL_UUID = 9,
		// Token: 0x040009D6 RID: 2518
		SQL_VARBINARY = 15,
		// Token: 0x040009D7 RID: 2519
		SQL_BINARY = 12,
		// Token: 0x040009D8 RID: 2520
		SQL_IMAGE = 23,
		// Token: 0x040009D9 RID: 2521
		SQL_CHAR = 13,
		// Token: 0x040009DA RID: 2522
		SQL_VARCHAR = 16,
		// Token: 0x040009DB RID: 2523
		SQL_TEXT = 22,
		// Token: 0x040009DC RID: 2524
		SQL_NVARCHAR = 17,
		// Token: 0x040009DD RID: 2525
		SQL_NCHAR = 14,
		// Token: 0x040009DE RID: 2526
		SQL_NTEXT = 24,
		// Token: 0x040009DF RID: 2527
		SQL_UDT = 27,
		// Token: 0x040009E0 RID: 2528
		XSD_KATMAI_DATE = 127,
		// Token: 0x040009E1 RID: 2529
		XSD_KATMAI_DATETIME = 126,
		// Token: 0x040009E2 RID: 2530
		XSD_KATMAI_TIME = 125,
		// Token: 0x040009E3 RID: 2531
		XSD_KATMAI_DATEOFFSET = 124,
		// Token: 0x040009E4 RID: 2532
		XSD_KATMAI_DATETIMEOFFSET = 123,
		// Token: 0x040009E5 RID: 2533
		XSD_KATMAI_TIMEOFFSET = 122,
		// Token: 0x040009E6 RID: 2534
		XSD_BOOLEAN = 134,
		// Token: 0x040009E7 RID: 2535
		XSD_TIME = 129,
		// Token: 0x040009E8 RID: 2536
		XSD_DATETIME,
		// Token: 0x040009E9 RID: 2537
		XSD_DATE,
		// Token: 0x040009EA RID: 2538
		XSD_BINHEX,
		// Token: 0x040009EB RID: 2539
		XSD_BASE64,
		// Token: 0x040009EC RID: 2540
		XSD_DECIMAL = 135,
		// Token: 0x040009ED RID: 2541
		XSD_BYTE,
		// Token: 0x040009EE RID: 2542
		XSD_UNSIGNEDSHORT,
		// Token: 0x040009EF RID: 2543
		XSD_UNSIGNEDINT,
		// Token: 0x040009F0 RID: 2544
		XSD_UNSIGNEDLONG,
		// Token: 0x040009F1 RID: 2545
		XSD_QNAME
	}
}

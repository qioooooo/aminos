using System;

namespace System.Drawing.Imaging
{
	// Token: 0x02000082 RID: 130
	public enum EmfPlusRecordType
	{
		// Token: 0x04000502 RID: 1282
		WmfRecordBase = 65536,
		// Token: 0x04000503 RID: 1283
		WmfSetBkColor = 66049,
		// Token: 0x04000504 RID: 1284
		WmfSetBkMode = 65794,
		// Token: 0x04000505 RID: 1285
		WmfSetMapMode,
		// Token: 0x04000506 RID: 1286
		WmfSetROP2,
		// Token: 0x04000507 RID: 1287
		WmfSetRelAbs,
		// Token: 0x04000508 RID: 1288
		WmfSetPolyFillMode,
		// Token: 0x04000509 RID: 1289
		WmfSetStretchBltMode,
		// Token: 0x0400050A RID: 1290
		WmfSetTextCharExtra,
		// Token: 0x0400050B RID: 1291
		WmfSetTextColor = 66057,
		// Token: 0x0400050C RID: 1292
		WmfSetTextJustification,
		// Token: 0x0400050D RID: 1293
		WmfSetWindowOrg,
		// Token: 0x0400050E RID: 1294
		WmfSetWindowExt,
		// Token: 0x0400050F RID: 1295
		WmfSetViewportOrg,
		// Token: 0x04000510 RID: 1296
		WmfSetViewportExt,
		// Token: 0x04000511 RID: 1297
		WmfOffsetWindowOrg,
		// Token: 0x04000512 RID: 1298
		WmfScaleWindowExt = 66576,
		// Token: 0x04000513 RID: 1299
		WmfOffsetViewportOrg = 66065,
		// Token: 0x04000514 RID: 1300
		WmfScaleViewportExt = 66578,
		// Token: 0x04000515 RID: 1301
		WmfLineTo = 66067,
		// Token: 0x04000516 RID: 1302
		WmfMoveTo,
		// Token: 0x04000517 RID: 1303
		WmfExcludeClipRect = 66581,
		// Token: 0x04000518 RID: 1304
		WmfIntersectClipRect,
		// Token: 0x04000519 RID: 1305
		WmfArc = 67607,
		// Token: 0x0400051A RID: 1306
		WmfEllipse = 66584,
		// Token: 0x0400051B RID: 1307
		WmfFloodFill,
		// Token: 0x0400051C RID: 1308
		WmfPie = 67610,
		// Token: 0x0400051D RID: 1309
		WmfRectangle = 66587,
		// Token: 0x0400051E RID: 1310
		WmfRoundRect = 67100,
		// Token: 0x0400051F RID: 1311
		WmfPatBlt,
		// Token: 0x04000520 RID: 1312
		WmfSaveDC = 65566,
		// Token: 0x04000521 RID: 1313
		WmfSetPixel = 66591,
		// Token: 0x04000522 RID: 1314
		WmfOffsetCilpRgn = 66080,
		// Token: 0x04000523 RID: 1315
		WmfTextOut = 66849,
		// Token: 0x04000524 RID: 1316
		WmfBitBlt = 67874,
		// Token: 0x04000525 RID: 1317
		WmfStretchBlt = 68387,
		// Token: 0x04000526 RID: 1318
		WmfPolygon = 66340,
		// Token: 0x04000527 RID: 1319
		WmfPolyline,
		// Token: 0x04000528 RID: 1320
		WmfEscape = 67110,
		// Token: 0x04000529 RID: 1321
		WmfRestoreDC = 65831,
		// Token: 0x0400052A RID: 1322
		WmfFillRegion = 66088,
		// Token: 0x0400052B RID: 1323
		WmfFrameRegion = 66601,
		// Token: 0x0400052C RID: 1324
		WmfInvertRegion = 65834,
		// Token: 0x0400052D RID: 1325
		WmfPaintRegion,
		// Token: 0x0400052E RID: 1326
		WmfSelectClipRegion,
		// Token: 0x0400052F RID: 1327
		WmfSelectObject,
		// Token: 0x04000530 RID: 1328
		WmfSetTextAlign,
		// Token: 0x04000531 RID: 1329
		WmfChord = 67632,
		// Token: 0x04000532 RID: 1330
		WmfSetMapperFlags = 66097,
		// Token: 0x04000533 RID: 1331
		WmfExtTextOut = 68146,
		// Token: 0x04000534 RID: 1332
		WmfSetDibToDev = 68915,
		// Token: 0x04000535 RID: 1333
		WmfSelectPalette = 66100,
		// Token: 0x04000536 RID: 1334
		WmfRealizePalette = 65589,
		// Token: 0x04000537 RID: 1335
		WmfAnimatePalette = 66614,
		// Token: 0x04000538 RID: 1336
		WmfSetPalEntries = 65591,
		// Token: 0x04000539 RID: 1337
		WmfPolyPolygon = 66872,
		// Token: 0x0400053A RID: 1338
		WmfResizePalette = 65849,
		// Token: 0x0400053B RID: 1339
		WmfDibBitBlt = 67904,
		// Token: 0x0400053C RID: 1340
		WmfDibStretchBlt = 68417,
		// Token: 0x0400053D RID: 1341
		WmfDibCreatePatternBrush = 65858,
		// Token: 0x0400053E RID: 1342
		WmfStretchDib = 69443,
		// Token: 0x0400053F RID: 1343
		WmfExtFloodFill = 66888,
		// Token: 0x04000540 RID: 1344
		WmfSetLayout = 65865,
		// Token: 0x04000541 RID: 1345
		WmfDeleteObject = 66032,
		// Token: 0x04000542 RID: 1346
		WmfCreatePalette = 65783,
		// Token: 0x04000543 RID: 1347
		WmfCreatePatternBrush = 66041,
		// Token: 0x04000544 RID: 1348
		WmfCreatePenIndirect = 66298,
		// Token: 0x04000545 RID: 1349
		WmfCreateFontIndirect,
		// Token: 0x04000546 RID: 1350
		WmfCreateBrushIndirect,
		// Token: 0x04000547 RID: 1351
		WmfCreateRegion = 67327,
		// Token: 0x04000548 RID: 1352
		EmfHeader = 1,
		// Token: 0x04000549 RID: 1353
		EmfPolyBezier,
		// Token: 0x0400054A RID: 1354
		EmfPolygon,
		// Token: 0x0400054B RID: 1355
		EmfPolyline,
		// Token: 0x0400054C RID: 1356
		EmfPolyBezierTo,
		// Token: 0x0400054D RID: 1357
		EmfPolyLineTo,
		// Token: 0x0400054E RID: 1358
		EmfPolyPolyline,
		// Token: 0x0400054F RID: 1359
		EmfPolyPolygon,
		// Token: 0x04000550 RID: 1360
		EmfSetWindowExtEx,
		// Token: 0x04000551 RID: 1361
		EmfSetWindowOrgEx,
		// Token: 0x04000552 RID: 1362
		EmfSetViewportExtEx,
		// Token: 0x04000553 RID: 1363
		EmfSetViewportOrgEx,
		// Token: 0x04000554 RID: 1364
		EmfSetBrushOrgEx,
		// Token: 0x04000555 RID: 1365
		EmfEof,
		// Token: 0x04000556 RID: 1366
		EmfSetPixelV,
		// Token: 0x04000557 RID: 1367
		EmfSetMapperFlags,
		// Token: 0x04000558 RID: 1368
		EmfSetMapMode,
		// Token: 0x04000559 RID: 1369
		EmfSetBkMode,
		// Token: 0x0400055A RID: 1370
		EmfSetPolyFillMode,
		// Token: 0x0400055B RID: 1371
		EmfSetROP2,
		// Token: 0x0400055C RID: 1372
		EmfSetStretchBltMode,
		// Token: 0x0400055D RID: 1373
		EmfSetTextAlign,
		// Token: 0x0400055E RID: 1374
		EmfSetColorAdjustment,
		// Token: 0x0400055F RID: 1375
		EmfSetTextColor,
		// Token: 0x04000560 RID: 1376
		EmfSetBkColor,
		// Token: 0x04000561 RID: 1377
		EmfOffsetClipRgn,
		// Token: 0x04000562 RID: 1378
		EmfMoveToEx,
		// Token: 0x04000563 RID: 1379
		EmfSetMetaRgn,
		// Token: 0x04000564 RID: 1380
		EmfExcludeClipRect,
		// Token: 0x04000565 RID: 1381
		EmfIntersectClipRect,
		// Token: 0x04000566 RID: 1382
		EmfScaleViewportExtEx,
		// Token: 0x04000567 RID: 1383
		EmfScaleWindowExtEx,
		// Token: 0x04000568 RID: 1384
		EmfSaveDC,
		// Token: 0x04000569 RID: 1385
		EmfRestoreDC,
		// Token: 0x0400056A RID: 1386
		EmfSetWorldTransform,
		// Token: 0x0400056B RID: 1387
		EmfModifyWorldTransform,
		// Token: 0x0400056C RID: 1388
		EmfSelectObject,
		// Token: 0x0400056D RID: 1389
		EmfCreatePen,
		// Token: 0x0400056E RID: 1390
		EmfCreateBrushIndirect,
		// Token: 0x0400056F RID: 1391
		EmfDeleteObject,
		// Token: 0x04000570 RID: 1392
		EmfAngleArc,
		// Token: 0x04000571 RID: 1393
		EmfEllipse,
		// Token: 0x04000572 RID: 1394
		EmfRectangle,
		// Token: 0x04000573 RID: 1395
		EmfRoundRect,
		// Token: 0x04000574 RID: 1396
		EmfRoundArc,
		// Token: 0x04000575 RID: 1397
		EmfChord,
		// Token: 0x04000576 RID: 1398
		EmfPie,
		// Token: 0x04000577 RID: 1399
		EmfSelectPalette,
		// Token: 0x04000578 RID: 1400
		EmfCreatePalette,
		// Token: 0x04000579 RID: 1401
		EmfSetPaletteEntries,
		// Token: 0x0400057A RID: 1402
		EmfResizePalette,
		// Token: 0x0400057B RID: 1403
		EmfRealizePalette,
		// Token: 0x0400057C RID: 1404
		EmfExtFloodFill,
		// Token: 0x0400057D RID: 1405
		EmfLineTo,
		// Token: 0x0400057E RID: 1406
		EmfArcTo,
		// Token: 0x0400057F RID: 1407
		EmfPolyDraw,
		// Token: 0x04000580 RID: 1408
		EmfSetArcDirection,
		// Token: 0x04000581 RID: 1409
		EmfSetMiterLimit,
		// Token: 0x04000582 RID: 1410
		EmfBeginPath,
		// Token: 0x04000583 RID: 1411
		EmfEndPath,
		// Token: 0x04000584 RID: 1412
		EmfCloseFigure,
		// Token: 0x04000585 RID: 1413
		EmfFillPath,
		// Token: 0x04000586 RID: 1414
		EmfStrokeAndFillPath,
		// Token: 0x04000587 RID: 1415
		EmfStrokePath,
		// Token: 0x04000588 RID: 1416
		EmfFlattenPath,
		// Token: 0x04000589 RID: 1417
		EmfWidenPath,
		// Token: 0x0400058A RID: 1418
		EmfSelectClipPath,
		// Token: 0x0400058B RID: 1419
		EmfAbortPath,
		// Token: 0x0400058C RID: 1420
		EmfReserved069,
		// Token: 0x0400058D RID: 1421
		EmfGdiComment,
		// Token: 0x0400058E RID: 1422
		EmfFillRgn,
		// Token: 0x0400058F RID: 1423
		EmfFrameRgn,
		// Token: 0x04000590 RID: 1424
		EmfInvertRgn,
		// Token: 0x04000591 RID: 1425
		EmfPaintRgn,
		// Token: 0x04000592 RID: 1426
		EmfExtSelectClipRgn,
		// Token: 0x04000593 RID: 1427
		EmfBitBlt,
		// Token: 0x04000594 RID: 1428
		EmfStretchBlt,
		// Token: 0x04000595 RID: 1429
		EmfMaskBlt,
		// Token: 0x04000596 RID: 1430
		EmfPlgBlt,
		// Token: 0x04000597 RID: 1431
		EmfSetDIBitsToDevice,
		// Token: 0x04000598 RID: 1432
		EmfStretchDIBits,
		// Token: 0x04000599 RID: 1433
		EmfExtCreateFontIndirect,
		// Token: 0x0400059A RID: 1434
		EmfExtTextOutA,
		// Token: 0x0400059B RID: 1435
		EmfExtTextOutW,
		// Token: 0x0400059C RID: 1436
		EmfPolyBezier16,
		// Token: 0x0400059D RID: 1437
		EmfPolygon16,
		// Token: 0x0400059E RID: 1438
		EmfPolyline16,
		// Token: 0x0400059F RID: 1439
		EmfPolyBezierTo16,
		// Token: 0x040005A0 RID: 1440
		EmfPolylineTo16,
		// Token: 0x040005A1 RID: 1441
		EmfPolyPolyline16,
		// Token: 0x040005A2 RID: 1442
		EmfPolyPolygon16,
		// Token: 0x040005A3 RID: 1443
		EmfPolyDraw16,
		// Token: 0x040005A4 RID: 1444
		EmfCreateMonoBrush,
		// Token: 0x040005A5 RID: 1445
		EmfCreateDibPatternBrushPt,
		// Token: 0x040005A6 RID: 1446
		EmfExtCreatePen,
		// Token: 0x040005A7 RID: 1447
		EmfPolyTextOutA,
		// Token: 0x040005A8 RID: 1448
		EmfPolyTextOutW,
		// Token: 0x040005A9 RID: 1449
		EmfSetIcmMode,
		// Token: 0x040005AA RID: 1450
		EmfCreateColorSpace,
		// Token: 0x040005AB RID: 1451
		EmfSetColorSpace,
		// Token: 0x040005AC RID: 1452
		EmfDeleteColorSpace,
		// Token: 0x040005AD RID: 1453
		EmfGlsRecord,
		// Token: 0x040005AE RID: 1454
		EmfGlsBoundedRecord,
		// Token: 0x040005AF RID: 1455
		EmfPixelFormat,
		// Token: 0x040005B0 RID: 1456
		EmfDrawEscape,
		// Token: 0x040005B1 RID: 1457
		EmfExtEscape,
		// Token: 0x040005B2 RID: 1458
		EmfStartDoc,
		// Token: 0x040005B3 RID: 1459
		EmfSmallTextOut,
		// Token: 0x040005B4 RID: 1460
		EmfForceUfiMapping,
		// Token: 0x040005B5 RID: 1461
		EmfNamedEscpae,
		// Token: 0x040005B6 RID: 1462
		EmfColorCorrectPalette,
		// Token: 0x040005B7 RID: 1463
		EmfSetIcmProfileA,
		// Token: 0x040005B8 RID: 1464
		EmfSetIcmProfileW,
		// Token: 0x040005B9 RID: 1465
		EmfAlphaBlend,
		// Token: 0x040005BA RID: 1466
		EmfSetLayout,
		// Token: 0x040005BB RID: 1467
		EmfTransparentBlt,
		// Token: 0x040005BC RID: 1468
		EmfReserved117,
		// Token: 0x040005BD RID: 1469
		EmfGradientFill,
		// Token: 0x040005BE RID: 1470
		EmfSetLinkedUfis,
		// Token: 0x040005BF RID: 1471
		EmfSetTextJustification,
		// Token: 0x040005C0 RID: 1472
		EmfColorMatchToTargetW,
		// Token: 0x040005C1 RID: 1473
		EmfCreateColorSpaceW,
		// Token: 0x040005C2 RID: 1474
		EmfMax = 122,
		// Token: 0x040005C3 RID: 1475
		EmfMin = 1,
		// Token: 0x040005C4 RID: 1476
		EmfPlusRecordBase = 16384,
		// Token: 0x040005C5 RID: 1477
		Invalid = 16384,
		// Token: 0x040005C6 RID: 1478
		Header,
		// Token: 0x040005C7 RID: 1479
		EndOfFile,
		// Token: 0x040005C8 RID: 1480
		Comment,
		// Token: 0x040005C9 RID: 1481
		GetDC,
		// Token: 0x040005CA RID: 1482
		MultiFormatStart,
		// Token: 0x040005CB RID: 1483
		MultiFormatSection,
		// Token: 0x040005CC RID: 1484
		MultiFormatEnd,
		// Token: 0x040005CD RID: 1485
		Object,
		// Token: 0x040005CE RID: 1486
		Clear,
		// Token: 0x040005CF RID: 1487
		FillRects,
		// Token: 0x040005D0 RID: 1488
		DrawRects,
		// Token: 0x040005D1 RID: 1489
		FillPolygon,
		// Token: 0x040005D2 RID: 1490
		DrawLines,
		// Token: 0x040005D3 RID: 1491
		FillEllipse,
		// Token: 0x040005D4 RID: 1492
		DrawEllipse,
		// Token: 0x040005D5 RID: 1493
		FillPie,
		// Token: 0x040005D6 RID: 1494
		DrawPie,
		// Token: 0x040005D7 RID: 1495
		DrawArc,
		// Token: 0x040005D8 RID: 1496
		FillRegion,
		// Token: 0x040005D9 RID: 1497
		FillPath,
		// Token: 0x040005DA RID: 1498
		DrawPath,
		// Token: 0x040005DB RID: 1499
		FillClosedCurve,
		// Token: 0x040005DC RID: 1500
		DrawClosedCurve,
		// Token: 0x040005DD RID: 1501
		DrawCurve,
		// Token: 0x040005DE RID: 1502
		DrawBeziers,
		// Token: 0x040005DF RID: 1503
		DrawImage,
		// Token: 0x040005E0 RID: 1504
		DrawImagePoints,
		// Token: 0x040005E1 RID: 1505
		DrawString,
		// Token: 0x040005E2 RID: 1506
		SetRenderingOrigin,
		// Token: 0x040005E3 RID: 1507
		SetAntiAliasMode,
		// Token: 0x040005E4 RID: 1508
		SetTextRenderingHint,
		// Token: 0x040005E5 RID: 1509
		SetTextContrast,
		// Token: 0x040005E6 RID: 1510
		SetInterpolationMode,
		// Token: 0x040005E7 RID: 1511
		SetPixelOffsetMode,
		// Token: 0x040005E8 RID: 1512
		SetCompositingMode,
		// Token: 0x040005E9 RID: 1513
		SetCompositingQuality,
		// Token: 0x040005EA RID: 1514
		Save,
		// Token: 0x040005EB RID: 1515
		Restore,
		// Token: 0x040005EC RID: 1516
		BeginContainer,
		// Token: 0x040005ED RID: 1517
		BeginContainerNoParams,
		// Token: 0x040005EE RID: 1518
		EndContainer,
		// Token: 0x040005EF RID: 1519
		SetWorldTransform,
		// Token: 0x040005F0 RID: 1520
		ResetWorldTransform,
		// Token: 0x040005F1 RID: 1521
		MultiplyWorldTransform,
		// Token: 0x040005F2 RID: 1522
		TranslateWorldTransform,
		// Token: 0x040005F3 RID: 1523
		ScaleWorldTransform,
		// Token: 0x040005F4 RID: 1524
		RotateWorldTransform,
		// Token: 0x040005F5 RID: 1525
		SetPageTransform,
		// Token: 0x040005F6 RID: 1526
		ResetClip,
		// Token: 0x040005F7 RID: 1527
		SetClipRect,
		// Token: 0x040005F8 RID: 1528
		SetClipPath,
		// Token: 0x040005F9 RID: 1529
		SetClipRegion,
		// Token: 0x040005FA RID: 1530
		OffsetClip,
		// Token: 0x040005FB RID: 1531
		DrawDriverString,
		// Token: 0x040005FC RID: 1532
		Total,
		// Token: 0x040005FD RID: 1533
		Max = 16438,
		// Token: 0x040005FE RID: 1534
		Min = 16385
	}
}

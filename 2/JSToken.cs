using System;

namespace Microsoft.JScript
{
	// Token: 0x020000C5 RID: 197
	public enum JSToken
	{
		// Token: 0x040004CB RID: 1227
		None = -1,
		// Token: 0x040004CC RID: 1228
		EndOfFile,
		// Token: 0x040004CD RID: 1229
		If,
		// Token: 0x040004CE RID: 1230
		For,
		// Token: 0x040004CF RID: 1231
		Do,
		// Token: 0x040004D0 RID: 1232
		While,
		// Token: 0x040004D1 RID: 1233
		Continue,
		// Token: 0x040004D2 RID: 1234
		Break,
		// Token: 0x040004D3 RID: 1235
		Return,
		// Token: 0x040004D4 RID: 1236
		Import,
		// Token: 0x040004D5 RID: 1237
		With,
		// Token: 0x040004D6 RID: 1238
		Switch,
		// Token: 0x040004D7 RID: 1239
		Throw,
		// Token: 0x040004D8 RID: 1240
		Try,
		// Token: 0x040004D9 RID: 1241
		Package,
		// Token: 0x040004DA RID: 1242
		Internal,
		// Token: 0x040004DB RID: 1243
		Abstract,
		// Token: 0x040004DC RID: 1244
		Public,
		// Token: 0x040004DD RID: 1245
		Static,
		// Token: 0x040004DE RID: 1246
		Private,
		// Token: 0x040004DF RID: 1247
		Protected,
		// Token: 0x040004E0 RID: 1248
		Final,
		// Token: 0x040004E1 RID: 1249
		Event,
		// Token: 0x040004E2 RID: 1250
		Var,
		// Token: 0x040004E3 RID: 1251
		Const,
		// Token: 0x040004E4 RID: 1252
		Class,
		// Token: 0x040004E5 RID: 1253
		Function,
		// Token: 0x040004E6 RID: 1254
		LeftCurly,
		// Token: 0x040004E7 RID: 1255
		Semicolon,
		// Token: 0x040004E8 RID: 1256
		Null,
		// Token: 0x040004E9 RID: 1257
		True,
		// Token: 0x040004EA RID: 1258
		False,
		// Token: 0x040004EB RID: 1259
		This,
		// Token: 0x040004EC RID: 1260
		Identifier,
		// Token: 0x040004ED RID: 1261
		StringLiteral,
		// Token: 0x040004EE RID: 1262
		IntegerLiteral,
		// Token: 0x040004EF RID: 1263
		NumericLiteral,
		// Token: 0x040004F0 RID: 1264
		LeftParen,
		// Token: 0x040004F1 RID: 1265
		LeftBracket,
		// Token: 0x040004F2 RID: 1266
		AccessField,
		// Token: 0x040004F3 RID: 1267
		FirstOp,
		// Token: 0x040004F4 RID: 1268
		LogicalNot = 39,
		// Token: 0x040004F5 RID: 1269
		BitwiseNot,
		// Token: 0x040004F6 RID: 1270
		Delete,
		// Token: 0x040004F7 RID: 1271
		Void,
		// Token: 0x040004F8 RID: 1272
		Typeof,
		// Token: 0x040004F9 RID: 1273
		Increment,
		// Token: 0x040004FA RID: 1274
		Decrement,
		// Token: 0x040004FB RID: 1275
		FirstBinaryOp,
		// Token: 0x040004FC RID: 1276
		Plus = 46,
		// Token: 0x040004FD RID: 1277
		Minus,
		// Token: 0x040004FE RID: 1278
		LogicalOr,
		// Token: 0x040004FF RID: 1279
		LogicalAnd,
		// Token: 0x04000500 RID: 1280
		BitwiseOr,
		// Token: 0x04000501 RID: 1281
		BitwiseXor,
		// Token: 0x04000502 RID: 1282
		BitwiseAnd,
		// Token: 0x04000503 RID: 1283
		Equal,
		// Token: 0x04000504 RID: 1284
		NotEqual,
		// Token: 0x04000505 RID: 1285
		StrictEqual,
		// Token: 0x04000506 RID: 1286
		StrictNotEqual,
		// Token: 0x04000507 RID: 1287
		GreaterThan,
		// Token: 0x04000508 RID: 1288
		LessThan,
		// Token: 0x04000509 RID: 1289
		LessThanEqual,
		// Token: 0x0400050A RID: 1290
		GreaterThanEqual,
		// Token: 0x0400050B RID: 1291
		LeftShift,
		// Token: 0x0400050C RID: 1292
		RightShift,
		// Token: 0x0400050D RID: 1293
		UnsignedRightShift,
		// Token: 0x0400050E RID: 1294
		Multiply,
		// Token: 0x0400050F RID: 1295
		Divide,
		// Token: 0x04000510 RID: 1296
		Modulo,
		// Token: 0x04000511 RID: 1297
		LastPPOperator = 66,
		// Token: 0x04000512 RID: 1298
		Instanceof,
		// Token: 0x04000513 RID: 1299
		In,
		// Token: 0x04000514 RID: 1300
		Assign,
		// Token: 0x04000515 RID: 1301
		PlusAssign,
		// Token: 0x04000516 RID: 1302
		MinusAssign,
		// Token: 0x04000517 RID: 1303
		MultiplyAssign,
		// Token: 0x04000518 RID: 1304
		DivideAssign,
		// Token: 0x04000519 RID: 1305
		BitwiseAndAssign,
		// Token: 0x0400051A RID: 1306
		BitwiseOrAssign,
		// Token: 0x0400051B RID: 1307
		BitwiseXorAssign,
		// Token: 0x0400051C RID: 1308
		ModuloAssign,
		// Token: 0x0400051D RID: 1309
		LeftShiftAssign,
		// Token: 0x0400051E RID: 1310
		RightShiftAssign,
		// Token: 0x0400051F RID: 1311
		UnsignedRightShiftAssign,
		// Token: 0x04000520 RID: 1312
		LastAssign = 80,
		// Token: 0x04000521 RID: 1313
		LastBinaryOp = 80,
		// Token: 0x04000522 RID: 1314
		ConditionalIf,
		// Token: 0x04000523 RID: 1315
		Colon,
		// Token: 0x04000524 RID: 1316
		Comma,
		// Token: 0x04000525 RID: 1317
		LastOp = 83,
		// Token: 0x04000526 RID: 1318
		Case,
		// Token: 0x04000527 RID: 1319
		Catch,
		// Token: 0x04000528 RID: 1320
		Debugger,
		// Token: 0x04000529 RID: 1321
		Default,
		// Token: 0x0400052A RID: 1322
		Else,
		// Token: 0x0400052B RID: 1323
		Export,
		// Token: 0x0400052C RID: 1324
		Extends,
		// Token: 0x0400052D RID: 1325
		Finally,
		// Token: 0x0400052E RID: 1326
		Get,
		// Token: 0x0400052F RID: 1327
		Implements,
		// Token: 0x04000530 RID: 1328
		Interface,
		// Token: 0x04000531 RID: 1329
		New,
		// Token: 0x04000532 RID: 1330
		Set,
		// Token: 0x04000533 RID: 1331
		Super,
		// Token: 0x04000534 RID: 1332
		RightParen,
		// Token: 0x04000535 RID: 1333
		RightCurly,
		// Token: 0x04000536 RID: 1334
		RightBracket,
		// Token: 0x04000537 RID: 1335
		PreProcessorConstant,
		// Token: 0x04000538 RID: 1336
		Comment,
		// Token: 0x04000539 RID: 1337
		UnterminatedComment,
		// Token: 0x0400053A RID: 1338
		Assert,
		// Token: 0x0400053B RID: 1339
		Boolean,
		// Token: 0x0400053C RID: 1340
		Byte,
		// Token: 0x0400053D RID: 1341
		Char,
		// Token: 0x0400053E RID: 1342
		Decimal,
		// Token: 0x0400053F RID: 1343
		Double,
		// Token: 0x04000540 RID: 1344
		DoubleColon,
		// Token: 0x04000541 RID: 1345
		Enum,
		// Token: 0x04000542 RID: 1346
		Ensure,
		// Token: 0x04000543 RID: 1347
		Float,
		// Token: 0x04000544 RID: 1348
		Goto,
		// Token: 0x04000545 RID: 1349
		Int,
		// Token: 0x04000546 RID: 1350
		Invariant,
		// Token: 0x04000547 RID: 1351
		Long,
		// Token: 0x04000548 RID: 1352
		Namespace,
		// Token: 0x04000549 RID: 1353
		Native,
		// Token: 0x0400054A RID: 1354
		Require,
		// Token: 0x0400054B RID: 1355
		Sbyte,
		// Token: 0x0400054C RID: 1356
		Short,
		// Token: 0x0400054D RID: 1357
		Synchronized,
		// Token: 0x0400054E RID: 1358
		Transient,
		// Token: 0x0400054F RID: 1359
		Throws,
		// Token: 0x04000550 RID: 1360
		ParamArray,
		// Token: 0x04000551 RID: 1361
		Volatile,
		// Token: 0x04000552 RID: 1362
		Ushort,
		// Token: 0x04000553 RID: 1363
		Uint,
		// Token: 0x04000554 RID: 1364
		Ulong,
		// Token: 0x04000555 RID: 1365
		Use,
		// Token: 0x04000556 RID: 1366
		EndOfLine,
		// Token: 0x04000557 RID: 1367
		PreProcessDirective
	}
}

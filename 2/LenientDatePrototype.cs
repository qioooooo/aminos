using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CD RID: 205
	public sealed class LenientDatePrototype : DatePrototype
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x000488A8 File Offset: 0x000478A8
		internal LenientDatePrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(DatePrototype);
			this.getTime = new BuiltinFunction("getTime", this, typeFromHandle.GetMethod("getTime"), funcprot);
			this.getYear = new BuiltinFunction("getYear", this, typeFromHandle.GetMethod("getYear"), funcprot);
			this.getFullYear = new BuiltinFunction("getFullYear", this, typeFromHandle.GetMethod("getFullYear"), funcprot);
			this.getUTCFullYear = new BuiltinFunction("getUTCFullYear", this, typeFromHandle.GetMethod("getUTCFullYear"), funcprot);
			this.getMonth = new BuiltinFunction("getMonth", this, typeFromHandle.GetMethod("getMonth"), funcprot);
			this.getUTCMonth = new BuiltinFunction("getUTCMonth", this, typeFromHandle.GetMethod("getUTCMonth"), funcprot);
			this.getDate = new BuiltinFunction("getDate", this, typeFromHandle.GetMethod("getDate"), funcprot);
			this.getUTCDate = new BuiltinFunction("getUTCDate", this, typeFromHandle.GetMethod("getUTCDate"), funcprot);
			this.getDay = new BuiltinFunction("getDay", this, typeFromHandle.GetMethod("getDay"), funcprot);
			this.getUTCDay = new BuiltinFunction("getUTCDay", this, typeFromHandle.GetMethod("getUTCDay"), funcprot);
			this.getHours = new BuiltinFunction("getHours", this, typeFromHandle.GetMethod("getHours"), funcprot);
			this.getUTCHours = new BuiltinFunction("getUTCHours", this, typeFromHandle.GetMethod("getUTCHours"), funcprot);
			this.getMinutes = new BuiltinFunction("getMinutes", this, typeFromHandle.GetMethod("getMinutes"), funcprot);
			this.getUTCMinutes = new BuiltinFunction("getUTCMinutes", this, typeFromHandle.GetMethod("getUTCMinutes"), funcprot);
			this.getSeconds = new BuiltinFunction("getSeconds", this, typeFromHandle.GetMethod("getSeconds"), funcprot);
			this.getUTCSeconds = new BuiltinFunction("getUTCSeconds", this, typeFromHandle.GetMethod("getUTCSeconds"), funcprot);
			this.getMilliseconds = new BuiltinFunction("getMilliseconds", this, typeFromHandle.GetMethod("getMilliseconds"), funcprot);
			this.getUTCMilliseconds = new BuiltinFunction("getUTCMilliseconds", this, typeFromHandle.GetMethod("getUTCMilliseconds"), funcprot);
			this.getVarDate = new BuiltinFunction("getVarDate", this, typeFromHandle.GetMethod("getVarDate"), funcprot);
			this.getTimezoneOffset = new BuiltinFunction("getTimezoneOffset", this, typeFromHandle.GetMethod("getTimezoneOffset"), funcprot);
			this.setTime = new BuiltinFunction("setTime", this, typeFromHandle.GetMethod("setTime"), funcprot);
			this.setMilliseconds = new BuiltinFunction("setMilliseconds", this, typeFromHandle.GetMethod("setMilliseconds"), funcprot);
			this.setUTCMilliseconds = new BuiltinFunction("setUTCMilliseconds", this, typeFromHandle.GetMethod("setUTCMilliseconds"), funcprot);
			this.setSeconds = new BuiltinFunction("setSeconds", this, typeFromHandle.GetMethod("setSeconds"), funcprot);
			this.setUTCSeconds = new BuiltinFunction("setUTCSeconds", this, typeFromHandle.GetMethod("setUTCSeconds"), funcprot);
			this.setMinutes = new BuiltinFunction("setMinutes", this, typeFromHandle.GetMethod("setMinutes"), funcprot);
			this.setUTCMinutes = new BuiltinFunction("setUTCMinutes", this, typeFromHandle.GetMethod("setUTCMinutes"), funcprot);
			this.setHours = new BuiltinFunction("setHours", this, typeFromHandle.GetMethod("setHours"), funcprot);
			this.setUTCHours = new BuiltinFunction("setUTCHours", this, typeFromHandle.GetMethod("setUTCHours"), funcprot);
			this.setDate = new BuiltinFunction("setDate", this, typeFromHandle.GetMethod("setDate"), funcprot);
			this.setUTCDate = new BuiltinFunction("setUTCDate", this, typeFromHandle.GetMethod("setUTCDate"), funcprot);
			this.setMonth = new BuiltinFunction("setMonth", this, typeFromHandle.GetMethod("setMonth"), funcprot);
			this.setUTCMonth = new BuiltinFunction("setUTCMonth", this, typeFromHandle.GetMethod("setUTCMonth"), funcprot);
			this.setFullYear = new BuiltinFunction("setFullYear", this, typeFromHandle.GetMethod("setFullYear"), funcprot);
			this.setUTCFullYear = new BuiltinFunction("setUTCFullYear", this, typeFromHandle.GetMethod("setUTCFullYear"), funcprot);
			this.setYear = new BuiltinFunction("setYear", this, typeFromHandle.GetMethod("setYear"), funcprot);
			this.toDateString = new BuiltinFunction("toDateString", this, typeFromHandle.GetMethod("toDateString"), funcprot);
			this.toLocaleDateString = new BuiltinFunction("toLocaleDateString", this, typeFromHandle.GetMethod("toLocaleDateString"), funcprot);
			this.toLocaleString = new BuiltinFunction("toLocaleString", this, typeFromHandle.GetMethod("toLocaleString"), funcprot);
			this.toLocaleTimeString = new BuiltinFunction("toLocaleTimeString", this, typeFromHandle.GetMethod("toLocaleTimeString"), funcprot);
			this.toGMTString = new BuiltinFunction("toUTCString", this, typeFromHandle.GetMethod("toUTCString"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
			this.toTimeString = new BuiltinFunction("toTimeString", this, typeFromHandle.GetMethod("toTimeString"), funcprot);
			this.toUTCString = new BuiltinFunction("toUTCString", this, typeFromHandle.GetMethod("toUTCString"), funcprot);
			this.valueOf = new BuiltinFunction("valueOf", this, typeFromHandle.GetMethod("valueOf"), funcprot);
		}

		// Token: 0x04000577 RID: 1399
		public new object constructor;

		// Token: 0x04000578 RID: 1400
		public new object getTime;

		// Token: 0x04000579 RID: 1401
		[NotRecommended("getYear")]
		public new object getYear;

		// Token: 0x0400057A RID: 1402
		public new object getFullYear;

		// Token: 0x0400057B RID: 1403
		public new object getUTCFullYear;

		// Token: 0x0400057C RID: 1404
		public new object getMonth;

		// Token: 0x0400057D RID: 1405
		public new object getUTCMonth;

		// Token: 0x0400057E RID: 1406
		public new object getDate;

		// Token: 0x0400057F RID: 1407
		public new object getUTCDate;

		// Token: 0x04000580 RID: 1408
		public new object getDay;

		// Token: 0x04000581 RID: 1409
		public new object getUTCDay;

		// Token: 0x04000582 RID: 1410
		public new object getHours;

		// Token: 0x04000583 RID: 1411
		public new object getUTCHours;

		// Token: 0x04000584 RID: 1412
		public new object getMinutes;

		// Token: 0x04000585 RID: 1413
		public new object getUTCMinutes;

		// Token: 0x04000586 RID: 1414
		public new object getSeconds;

		// Token: 0x04000587 RID: 1415
		public new object getUTCSeconds;

		// Token: 0x04000588 RID: 1416
		public new object getMilliseconds;

		// Token: 0x04000589 RID: 1417
		public new object getUTCMilliseconds;

		// Token: 0x0400058A RID: 1418
		public new object getVarDate;

		// Token: 0x0400058B RID: 1419
		public new object getTimezoneOffset;

		// Token: 0x0400058C RID: 1420
		public new object setTime;

		// Token: 0x0400058D RID: 1421
		public new object setMilliseconds;

		// Token: 0x0400058E RID: 1422
		public new object setUTCMilliseconds;

		// Token: 0x0400058F RID: 1423
		public new object setSeconds;

		// Token: 0x04000590 RID: 1424
		public new object setUTCSeconds;

		// Token: 0x04000591 RID: 1425
		public new object setMinutes;

		// Token: 0x04000592 RID: 1426
		public new object setUTCMinutes;

		// Token: 0x04000593 RID: 1427
		public new object setHours;

		// Token: 0x04000594 RID: 1428
		public new object setUTCHours;

		// Token: 0x04000595 RID: 1429
		public new object setDate;

		// Token: 0x04000596 RID: 1430
		public new object setUTCDate;

		// Token: 0x04000597 RID: 1431
		public new object setMonth;

		// Token: 0x04000598 RID: 1432
		public new object setUTCMonth;

		// Token: 0x04000599 RID: 1433
		public new object setFullYear;

		// Token: 0x0400059A RID: 1434
		public new object setUTCFullYear;

		// Token: 0x0400059B RID: 1435
		[NotRecommended("setYear")]
		public new object setYear;

		// Token: 0x0400059C RID: 1436
		[NotRecommended("toGMTString")]
		public new object toGMTString;

		// Token: 0x0400059D RID: 1437
		public new object toDateString;

		// Token: 0x0400059E RID: 1438
		public new object toLocaleDateString;

		// Token: 0x0400059F RID: 1439
		public new object toLocaleString;

		// Token: 0x040005A0 RID: 1440
		public new object toLocaleTimeString;

		// Token: 0x040005A1 RID: 1441
		public new object toString;

		// Token: 0x040005A2 RID: 1442
		public new object toTimeString;

		// Token: 0x040005A3 RID: 1443
		public new object toUTCString;

		// Token: 0x040005A4 RID: 1444
		public new object valueOf;
	}
}

using System;

namespace Microsoft.JScript
{
	// Token: 0x020000DF RID: 223
	public sealed class LenientStringPrototype : StringPrototype
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0004B990 File Offset: 0x0004A990
		internal LenientStringPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(funcprot, parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(StringPrototype);
			this.anchor = new BuiltinFunction("anchor", this, typeFromHandle.GetMethod("anchor"), funcprot);
			this.big = new BuiltinFunction("big", this, typeFromHandle.GetMethod("big"), funcprot);
			this.blink = new BuiltinFunction("blink", this, typeFromHandle.GetMethod("blink"), funcprot);
			this.bold = new BuiltinFunction("bold", this, typeFromHandle.GetMethod("bold"), funcprot);
			this.charAt = new BuiltinFunction("charAt", this, typeFromHandle.GetMethod("charAt"), funcprot);
			this.charCodeAt = new BuiltinFunction("charCodeAt", this, typeFromHandle.GetMethod("charCodeAt"), funcprot);
			this.concat = new BuiltinFunction("concat", this, typeFromHandle.GetMethod("concat"), funcprot);
			this.@fixed = new BuiltinFunction("fixed", this, typeFromHandle.GetMethod("fixed"), funcprot);
			this.fontcolor = new BuiltinFunction("fontcolor", this, typeFromHandle.GetMethod("fontcolor"), funcprot);
			this.fontsize = new BuiltinFunction("fontsize", this, typeFromHandle.GetMethod("fontsize"), funcprot);
			this.indexOf = new BuiltinFunction("indexOf", this, typeFromHandle.GetMethod("indexOf"), funcprot);
			this.italics = new BuiltinFunction("italics", this, typeFromHandle.GetMethod("italics"), funcprot);
			this.lastIndexOf = new BuiltinFunction("lastIndexOf", this, typeFromHandle.GetMethod("lastIndexOf"), funcprot);
			this.link = new BuiltinFunction("link", this, typeFromHandle.GetMethod("link"), funcprot);
			this.localeCompare = new BuiltinFunction("localeCompare", this, typeFromHandle.GetMethod("localeCompare"), funcprot);
			this.match = new BuiltinFunction("match", this, typeFromHandle.GetMethod("match"), funcprot);
			this.replace = new BuiltinFunction("replace", this, typeFromHandle.GetMethod("replace"), funcprot);
			this.search = new BuiltinFunction("search", this, typeFromHandle.GetMethod("search"), funcprot);
			this.slice = new BuiltinFunction("slice", this, typeFromHandle.GetMethod("slice"), funcprot);
			this.small = new BuiltinFunction("small", this, typeFromHandle.GetMethod("small"), funcprot);
			this.split = new BuiltinFunction("split", this, typeFromHandle.GetMethod("split"), funcprot);
			this.strike = new BuiltinFunction("strike", this, typeFromHandle.GetMethod("strike"), funcprot);
			this.sub = new BuiltinFunction("sub", this, typeFromHandle.GetMethod("sub"), funcprot);
			this.substr = new BuiltinFunction("substr", this, typeFromHandle.GetMethod("substr"), funcprot);
			this.substring = new BuiltinFunction("substring", this, typeFromHandle.GetMethod("substring"), funcprot);
			this.sup = new BuiltinFunction("sup", this, typeFromHandle.GetMethod("sup"), funcprot);
			this.toLocaleLowerCase = new BuiltinFunction("toLocaleLowerCase", this, typeFromHandle.GetMethod("toLocaleLowerCase"), funcprot);
			this.toLocaleUpperCase = new BuiltinFunction("toLocaleUpperCase", this, typeFromHandle.GetMethod("toLocaleUpperCase"), funcprot);
			this.toLowerCase = new BuiltinFunction("toLowerCase", this, typeFromHandle.GetMethod("toLowerCase"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
			this.toUpperCase = new BuiltinFunction("toUpperCase", this, typeFromHandle.GetMethod("toUpperCase"), funcprot);
			this.valueOf = new BuiltinFunction("valueOf", this, typeFromHandle.GetMethod("valueOf"), funcprot);
		}

		// Token: 0x04000630 RID: 1584
		public new object constructor;

		// Token: 0x04000631 RID: 1585
		public new object anchor;

		// Token: 0x04000632 RID: 1586
		public new object big;

		// Token: 0x04000633 RID: 1587
		public new object blink;

		// Token: 0x04000634 RID: 1588
		public new object bold;

		// Token: 0x04000635 RID: 1589
		public new object charAt;

		// Token: 0x04000636 RID: 1590
		public new object charCodeAt;

		// Token: 0x04000637 RID: 1591
		public new object concat;

		// Token: 0x04000638 RID: 1592
		public new object @fixed;

		// Token: 0x04000639 RID: 1593
		public new object fontcolor;

		// Token: 0x0400063A RID: 1594
		public new object fontsize;

		// Token: 0x0400063B RID: 1595
		public new object indexOf;

		// Token: 0x0400063C RID: 1596
		public new object italics;

		// Token: 0x0400063D RID: 1597
		public new object lastIndexOf;

		// Token: 0x0400063E RID: 1598
		public new object link;

		// Token: 0x0400063F RID: 1599
		public new object localeCompare;

		// Token: 0x04000640 RID: 1600
		public new object match;

		// Token: 0x04000641 RID: 1601
		public new object replace;

		// Token: 0x04000642 RID: 1602
		public new object search;

		// Token: 0x04000643 RID: 1603
		public new object slice;

		// Token: 0x04000644 RID: 1604
		public new object small;

		// Token: 0x04000645 RID: 1605
		public new object split;

		// Token: 0x04000646 RID: 1606
		public new object strike;

		// Token: 0x04000647 RID: 1607
		public new object sub;

		// Token: 0x04000648 RID: 1608
		[NotRecommended("substr")]
		public new object substr;

		// Token: 0x04000649 RID: 1609
		public new object substring;

		// Token: 0x0400064A RID: 1610
		public new object sup;

		// Token: 0x0400064B RID: 1611
		public new object toLocaleLowerCase;

		// Token: 0x0400064C RID: 1612
		public new object toLocaleUpperCase;

		// Token: 0x0400064D RID: 1613
		public new object toLowerCase;

		// Token: 0x0400064E RID: 1614
		public new object toString;

		// Token: 0x0400064F RID: 1615
		public new object toUpperCase;

		// Token: 0x04000650 RID: 1616
		public new object valueOf;
	}
}

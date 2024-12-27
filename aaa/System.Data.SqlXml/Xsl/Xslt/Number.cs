using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000116 RID: 278
	internal class Number : XslNode
	{
		// Token: 0x06000BEE RID: 3054 RVA: 0x0003D5C0 File Offset: 0x0003C5C0
		public Number(NumberLevel level, string count, string from, string value, string format, string lang, string letterValue, string groupingSeparator, string groupingSize, XslVersion xslVer)
			: base(XslNodeType.Number, null, null, xslVer)
		{
			this.Level = level;
			this.Count = count;
			this.From = from;
			this.Value = value;
			this.Format = format;
			this.Lang = lang;
			this.LetterValue = letterValue;
			this.GroupingSeparator = groupingSeparator;
			this.GroupingSize = groupingSize;
		}

		// Token: 0x0400086D RID: 2157
		public readonly NumberLevel Level;

		// Token: 0x0400086E RID: 2158
		public readonly string Count;

		// Token: 0x0400086F RID: 2159
		public readonly string From;

		// Token: 0x04000870 RID: 2160
		public readonly string Value;

		// Token: 0x04000871 RID: 2161
		public readonly string Format;

		// Token: 0x04000872 RID: 2162
		public readonly string Lang;

		// Token: 0x04000873 RID: 2163
		public readonly string LetterValue;

		// Token: 0x04000874 RID: 2164
		public readonly string GroupingSeparator;

		// Token: 0x04000875 RID: 2165
		public readonly string GroupingSize;
	}
}

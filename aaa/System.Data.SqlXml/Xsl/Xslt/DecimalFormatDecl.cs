using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E5 RID: 229
	internal class DecimalFormatDecl
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x00033319 File Offset: 0x00032319
		public DecimalFormatDecl(XmlQualifiedName name, string infinitySymbol, string nanSymbol, string characters)
		{
			this.Name = name;
			this.InfinitySymbol = infinitySymbol;
			this.NanSymbol = nanSymbol;
			this.Characters = characters.ToCharArray();
		}

		// Token: 0x0400070F RID: 1807
		public readonly XmlQualifiedName Name;

		// Token: 0x04000710 RID: 1808
		public readonly string InfinitySymbol;

		// Token: 0x04000711 RID: 1809
		public readonly string NanSymbol;

		// Token: 0x04000712 RID: 1810
		public readonly char[] Characters;

		// Token: 0x04000713 RID: 1811
		public static DecimalFormatDecl Default = new DecimalFormatDecl(new XmlQualifiedName(), "Infinity", "NaN", ".,%‰0#;-");
	}
}

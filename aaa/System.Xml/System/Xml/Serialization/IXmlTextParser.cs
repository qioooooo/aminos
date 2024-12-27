using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002BD RID: 701
	public interface IXmlTextParser
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x0600216E RID: 8558
		// (set) Token: 0x0600216F RID: 8559
		bool Normalized { get; set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002170 RID: 8560
		// (set) Token: 0x06002171 RID: 8561
		WhitespaceHandling WhitespaceHandling { get; set; }
	}
}

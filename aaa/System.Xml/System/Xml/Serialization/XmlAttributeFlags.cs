using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000304 RID: 772
	internal enum XmlAttributeFlags
	{
		// Token: 0x04001554 RID: 5460
		Enum = 1,
		// Token: 0x04001555 RID: 5461
		Array,
		// Token: 0x04001556 RID: 5462
		Text = 4,
		// Token: 0x04001557 RID: 5463
		ArrayItems = 8,
		// Token: 0x04001558 RID: 5464
		Elements = 16,
		// Token: 0x04001559 RID: 5465
		Attribute = 32,
		// Token: 0x0400155A RID: 5466
		Root = 64,
		// Token: 0x0400155B RID: 5467
		Type = 128,
		// Token: 0x0400155C RID: 5468
		AnyElements = 256,
		// Token: 0x0400155D RID: 5469
		AnyAttribute = 512,
		// Token: 0x0400155E RID: 5470
		ChoiceIdentifier = 1024,
		// Token: 0x0400155F RID: 5471
		XmlnsDeclarations = 2048
	}
}

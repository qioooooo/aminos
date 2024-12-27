using System;

namespace System.Xml.Xsl
{
	// Token: 0x0200000E RID: 14
	[Flags]
	internal enum XmlNodeKindFlags
	{
		// Token: 0x040000C6 RID: 198
		None = 0,
		// Token: 0x040000C7 RID: 199
		Document = 1,
		// Token: 0x040000C8 RID: 200
		Element = 2,
		// Token: 0x040000C9 RID: 201
		Attribute = 4,
		// Token: 0x040000CA RID: 202
		Text = 8,
		// Token: 0x040000CB RID: 203
		Comment = 16,
		// Token: 0x040000CC RID: 204
		PI = 32,
		// Token: 0x040000CD RID: 205
		Namespace = 64,
		// Token: 0x040000CE RID: 206
		Content = 58,
		// Token: 0x040000CF RID: 207
		Any = 127
	}
}

using System;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	internal enum SoapAttributeType
	{
		// Token: 0x0400012D RID: 301
		None,
		// Token: 0x0400012E RID: 302
		Embedded,
		// Token: 0x0400012F RID: 303
		XmlElement,
		// Token: 0x04000130 RID: 304
		XmlAttribute = 4,
		// Token: 0x04000131 RID: 305
		XmlType = 8
	}
}

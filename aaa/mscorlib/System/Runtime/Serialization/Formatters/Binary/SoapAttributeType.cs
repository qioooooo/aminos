using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007B8 RID: 1976
	[Serializable]
	internal enum SoapAttributeType
	{
		// Token: 0x0400235D RID: 9053
		None,
		// Token: 0x0400235E RID: 9054
		SchemaType,
		// Token: 0x0400235F RID: 9055
		Embedded,
		// Token: 0x04002360 RID: 9056
		XmlElement = 4,
		// Token: 0x04002361 RID: 9057
		XmlAttribute = 8
	}
}

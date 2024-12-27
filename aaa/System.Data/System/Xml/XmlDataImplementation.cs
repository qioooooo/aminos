using System;

namespace System.Xml
{
	// Token: 0x0200038D RID: 909
	internal sealed class XmlDataImplementation : XmlImplementation
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x002B6940 File Offset: 0x002B5D40
		public override XmlDocument CreateDocument()
		{
			return new XmlDataDocument(this);
		}
	}
}

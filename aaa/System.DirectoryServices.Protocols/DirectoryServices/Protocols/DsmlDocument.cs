using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005D RID: 93
	public abstract class DsmlDocument
	{
		// Token: 0x060001B4 RID: 436
		public abstract XmlDocument ToXml();

		// Token: 0x040001DF RID: 479
		internal string dsmlRequestID;
	}
}

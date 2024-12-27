using System;
using System.Configuration.Provider;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200005E RID: 94
	public abstract class ProtectedConfigurationProvider : ProviderBase
	{
		// Token: 0x0600039C RID: 924
		public abstract XmlNode Encrypt(XmlNode node);

		// Token: 0x0600039D RID: 925
		public abstract XmlNode Decrypt(XmlNode encryptedNode);
	}
}

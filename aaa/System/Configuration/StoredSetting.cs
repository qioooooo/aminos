using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EA RID: 1770
	internal struct StoredSetting
	{
		// Token: 0x060036B1 RID: 14001 RVA: 0x000E9659 File Offset: 0x000E8659
		internal StoredSetting(SettingsSerializeAs serializeAs, XmlNode value)
		{
			this.SerializeAs = serializeAs;
			this.Value = value;
		}

		// Token: 0x04003191 RID: 12689
		internal SettingsSerializeAs SerializeAs;

		// Token: 0x04003192 RID: 12690
		internal XmlNode Value;
	}
}

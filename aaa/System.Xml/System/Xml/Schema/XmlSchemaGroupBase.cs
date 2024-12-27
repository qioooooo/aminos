using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000232 RID: 562
	public abstract class XmlSchemaGroupBase : XmlSchemaParticle
	{
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001ADE RID: 6878
		[XmlIgnore]
		public abstract XmlSchemaObjectCollection Items { get; }

		// Token: 0x06001ADF RID: 6879
		internal abstract void SetItems(XmlSchemaObjectCollection newItems);
	}
}

using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B1 RID: 433
	internal class Datatype_anyAtomicType : Datatype_anySimpleType
	{
		// Token: 0x0600163E RID: 5694 RVA: 0x00062E20 File Offset: 0x00061E20
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlAnyConverter.AnyAtomic;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x00062E27 File Offset: 0x00061E27
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Preserve;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00062E2A File Offset: 0x00061E2A
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyAtomicType;
			}
		}
	}
}

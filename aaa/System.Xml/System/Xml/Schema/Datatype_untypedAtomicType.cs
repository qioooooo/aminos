using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B2 RID: 434
	internal class Datatype_untypedAtomicType : Datatype_anyAtomicType
	{
		// Token: 0x06001642 RID: 5698 RVA: 0x00062E36 File Offset: 0x00061E36
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlUntypedConverter.Untyped;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00062E3D File Offset: 0x00061E3D
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Preserve;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x00062E40 File Offset: 0x00061E40
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UntypedAtomic;
			}
		}
	}
}

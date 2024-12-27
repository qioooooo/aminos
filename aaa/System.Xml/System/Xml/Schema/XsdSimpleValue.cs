using System;

namespace System.Xml.Schema
{
	// Token: 0x020001A7 RID: 423
	internal class XsdSimpleValue
	{
		// Token: 0x060015BA RID: 5562 RVA: 0x00060AF8 File Offset: 0x0005FAF8
		public XsdSimpleValue(XmlSchemaSimpleType st, object value)
		{
			this.xmlType = st;
			this.typedValue = value;
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x00060B0E File Offset: 0x0005FB0E
		public XmlSchemaSimpleType XmlType
		{
			get
			{
				return this.xmlType;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00060B16 File Offset: 0x0005FB16
		public object TypedValue
		{
			get
			{
				return this.typedValue;
			}
		}

		// Token: 0x04000CF2 RID: 3314
		private XmlSchemaSimpleType xmlType;

		// Token: 0x04000CF3 RID: 3315
		private object typedValue;
	}
}

using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E7 RID: 487
	internal class Datatype_QNameXdr : Datatype_anySimpleType
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x00064364 File Offset: 0x00063364
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.QName;
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00064368 File Offset: 0x00063368
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			if (s == null || s.Length == 0)
			{
				throw new XmlSchemaException("Sch_EmptyAttributeValue", string.Empty);
			}
			if (nsmgr == null)
			{
				throw new ArgumentNullException("nsmgr");
			}
			object obj;
			try
			{
				string text;
				obj = XmlQualifiedName.Parse(s.Trim(), nsmgr, out text);
			}
			catch (XmlSchemaException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new XmlSchemaException(Res.GetString("Sch_InvalidValue", new object[] { s }), ex2);
			}
			return obj;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x000643F0 File Offset: 0x000633F0
		public override Type ValueType
		{
			get
			{
				return Datatype_QNameXdr.atomicValueType;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x000643F7 File Offset: 0x000633F7
		internal override Type ListValueType
		{
			get
			{
				return Datatype_QNameXdr.listValueType;
			}
		}

		// Token: 0x04000DAD RID: 3501
		private static readonly Type atomicValueType = typeof(XmlQualifiedName);

		// Token: 0x04000DAE RID: 3502
		private static readonly Type listValueType = typeof(XmlQualifiedName[]);
	}
}

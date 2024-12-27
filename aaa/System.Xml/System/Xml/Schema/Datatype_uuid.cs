using System;

namespace System.Xml.Schema
{
	// Token: 0x020001EB RID: 491
	internal class Datatype_uuid : Datatype_anySimpleType
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x000645E1 File Offset: 0x000635E1
		public override Type ValueType
		{
			get
			{
				return Datatype_uuid.atomicValueType;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x000645E8 File Offset: 0x000635E8
		internal override Type ListValueType
		{
			get
			{
				return Datatype_uuid.listValueType;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x000645EF File Offset: 0x000635EF
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return (RestrictionFlags)0;
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000645F4 File Offset: 0x000635F4
		internal override int Compare(object value1, object value2)
		{
			if (!((Guid)value1).Equals(value2))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0006461C File Offset: 0x0006361C
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			object obj;
			try
			{
				obj = XmlConvert.ToGuid(s);
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

		// Token: 0x06001774 RID: 6004 RVA: 0x00064678 File Offset: 0x00063678
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Guid guid;
			Exception ex = XmlConvert.TryToGuid(s, out guid);
			if (ex == null)
			{
				typedValue = guid;
				return null;
			}
			return ex;
		}

		// Token: 0x04000DB1 RID: 3505
		private static readonly Type atomicValueType = typeof(Guid);

		// Token: 0x04000DB2 RID: 3506
		private static readonly Type listValueType = typeof(Guid[]);
	}
}

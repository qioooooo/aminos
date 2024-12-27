using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E9 RID: 489
	internal class Datatype_char : Datatype_anySimpleType
	{
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00064432 File Offset: 0x00063432
		public override Type ValueType
		{
			get
			{
				return Datatype_char.atomicValueType;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x00064439 File Offset: 0x00063439
		internal override Type ListValueType
		{
			get
			{
				return Datatype_char.listValueType;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00064440 File Offset: 0x00063440
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return (RestrictionFlags)0;
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00064444 File Offset: 0x00063444
		internal override int Compare(object value1, object value2)
		{
			return ((char)value1).CompareTo(value2);
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00064460 File Offset: 0x00063460
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			object obj;
			try
			{
				obj = XmlConvert.ToChar(s);
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

		// Token: 0x06001769 RID: 5993 RVA: 0x000644BC File Offset: 0x000634BC
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			char c;
			Exception ex = XmlConvert.TryToChar(s, out c);
			if (ex == null)
			{
				typedValue = c;
				return null;
			}
			return ex;
		}

		// Token: 0x04000DAF RID: 3503
		private static readonly Type atomicValueType = typeof(char);

		// Token: 0x04000DB0 RID: 3504
		private static readonly Type listValueType = typeof(char[]);
	}
}

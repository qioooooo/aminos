using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E2 RID: 482
	internal class Datatype_unsignedShort : Datatype_unsignedInt
	{
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x000640CA File Offset: 0x000630CA
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_unsignedShort.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x000640D1 File Offset: 0x000630D1
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedShort;
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000640D8 File Offset: 0x000630D8
		internal override int Compare(object value1, object value2)
		{
			return ((ushort)value1).CompareTo(value2);
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x000640F4 File Offset: 0x000630F4
		public override Type ValueType
		{
			get
			{
				return Datatype_unsignedShort.atomicValueType;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x000640FB File Offset: 0x000630FB
		internal override Type ListValueType
		{
			get
			{
				return Datatype_unsignedShort.listValueType;
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00064104 File Offset: 0x00063104
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_unsignedShort.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				ushort num;
				ex = XmlConvert.TryToUInt16(s, out num);
				if (ex == null)
				{
					ex = Datatype_unsignedShort.numeric10FacetsChecker.CheckValueFacets((int)num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000DA6 RID: 3494
		private static readonly Type atomicValueType = typeof(ushort);

		// Token: 0x04000DA7 RID: 3495
		private static readonly Type listValueType = typeof(ushort[]);

		// Token: 0x04000DA8 RID: 3496
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 65535m);
	}
}

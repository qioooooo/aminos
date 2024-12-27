using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E1 RID: 481
	internal class Datatype_unsignedInt : Datatype_unsignedLong
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00064009 File Offset: 0x00063009
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_unsignedInt.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00064010 File Offset: 0x00063010
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedInt;
			}
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00064014 File Offset: 0x00063014
		internal override int Compare(object value1, object value2)
		{
			return ((uint)value1).CompareTo(value2);
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00064030 File Offset: 0x00063030
		public override Type ValueType
		{
			get
			{
				return Datatype_unsignedInt.atomicValueType;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00064037 File Offset: 0x00063037
		internal override Type ListValueType
		{
			get
			{
				return Datatype_unsignedInt.listValueType;
			}
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00064040 File Offset: 0x00063040
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_unsignedInt.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				uint num;
				ex = XmlConvert.TryToUInt32(s, out num);
				if (ex == null)
				{
					ex = Datatype_unsignedInt.numeric10FacetsChecker.CheckValueFacets((long)((ulong)num), this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000DA3 RID: 3491
		private static readonly Type atomicValueType = typeof(uint);

		// Token: 0x04000DA4 RID: 3492
		private static readonly Type listValueType = typeof(uint[]);

		// Token: 0x04000DA5 RID: 3493
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 4294967295m);
	}
}

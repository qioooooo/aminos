using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DD RID: 477
	internal class Datatype_short : Datatype_int
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00063D84 File Offset: 0x00062D84
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_short.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x00063D8B File Offset: 0x00062D8B
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Short;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00063D90 File Offset: 0x00062D90
		internal override int Compare(object value1, object value2)
		{
			return ((short)value1).CompareTo(value2);
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00063DAC File Offset: 0x00062DAC
		public override Type ValueType
		{
			get
			{
				return Datatype_short.atomicValueType;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x00063DB3 File Offset: 0x00062DB3
		internal override Type ListValueType
		{
			get
			{
				return Datatype_short.listValueType;
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00063DBC File Offset: 0x00062DBC
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_short.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				short num;
				ex = XmlConvert.TryToInt16(s, out num);
				if (ex == null)
				{
					ex = Datatype_short.numeric10FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D99 RID: 3481
		private static readonly Type atomicValueType = typeof(short);

		// Token: 0x04000D9A RID: 3482
		private static readonly Type listValueType = typeof(short[]);

		// Token: 0x04000D9B RID: 3483
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-32768m, 32767m);
	}
}

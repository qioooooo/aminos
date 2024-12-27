using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DB RID: 475
	internal class Datatype_long : Datatype_integer
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00063BDD File Offset: 0x00062BDD
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_long.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00063BE4 File Offset: 0x00062BE4
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x00063BE7 File Offset: 0x00062BE7
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Long;
			}
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00063BEC File Offset: 0x00062BEC
		internal override int Compare(object value1, object value2)
		{
			return ((long)value1).CompareTo(value2);
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00063C08 File Offset: 0x00062C08
		public override Type ValueType
		{
			get
			{
				return Datatype_long.atomicValueType;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00063C0F File Offset: 0x00062C0F
		internal override Type ListValueType
		{
			get
			{
				return Datatype_long.listValueType;
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00063C18 File Offset: 0x00062C18
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_long.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				long num;
				ex = XmlConvert.TryToInt64(s, out num);
				if (ex == null)
				{
					ex = Datatype_long.numeric10FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D93 RID: 3475
		private static readonly Type atomicValueType = typeof(long);

		// Token: 0x04000D94 RID: 3476
		private static readonly Type listValueType = typeof(long[]);

		// Token: 0x04000D95 RID: 3477
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-9223372036854775808m, 9223372036854775807m);
	}
}

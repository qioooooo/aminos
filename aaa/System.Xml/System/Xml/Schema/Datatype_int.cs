using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DC RID: 476
	internal class Datatype_int : Datatype_long
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x00063CBD File Offset: 0x00062CBD
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_int.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x00063CC4 File Offset: 0x00062CC4
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Int;
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x00063CC8 File Offset: 0x00062CC8
		internal override int Compare(object value1, object value2)
		{
			return ((int)value1).CompareTo(value2);
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x00063CE4 File Offset: 0x00062CE4
		public override Type ValueType
		{
			get
			{
				return Datatype_int.atomicValueType;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00063CEB File Offset: 0x00062CEB
		internal override Type ListValueType
		{
			get
			{
				return Datatype_int.listValueType;
			}
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00063CF4 File Offset: 0x00062CF4
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_int.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				int num;
				ex = XmlConvert.TryToInt32(s, out num);
				if (ex == null)
				{
					ex = Datatype_int.numeric10FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D96 RID: 3478
		private static readonly Type atomicValueType = typeof(int);

		// Token: 0x04000D97 RID: 3479
		private static readonly Type listValueType = typeof(int[]);

		// Token: 0x04000D98 RID: 3480
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-2147483648m, 2147483647m);
	}
}

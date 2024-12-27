using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E3 RID: 483
	internal class Datatype_unsignedByte : Datatype_unsignedShort
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00064190 File Offset: 0x00063190
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_unsignedByte.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00064197 File Offset: 0x00063197
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedByte;
			}
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0006419C File Offset: 0x0006319C
		internal override int Compare(object value1, object value2)
		{
			return ((byte)value1).CompareTo(value2);
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x000641B8 File Offset: 0x000631B8
		public override Type ValueType
		{
			get
			{
				return Datatype_unsignedByte.atomicValueType;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000641BF File Offset: 0x000631BF
		internal override Type ListValueType
		{
			get
			{
				return Datatype_unsignedByte.listValueType;
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x000641C8 File Offset: 0x000631C8
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_unsignedByte.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				byte b;
				ex = XmlConvert.TryToByte(s, out b);
				if (ex == null)
				{
					ex = Datatype_unsignedByte.numeric10FacetsChecker.CheckValueFacets((short)b, this);
					if (ex == null)
					{
						typedValue = b;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000DA9 RID: 3497
		private static readonly Type atomicValueType = typeof(byte);

		// Token: 0x04000DAA RID: 3498
		private static readonly Type listValueType = typeof(byte[]);

		// Token: 0x04000DAB RID: 3499
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 255m);
	}
}

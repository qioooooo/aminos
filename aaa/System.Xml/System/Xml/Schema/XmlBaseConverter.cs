using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x0200028B RID: 651
	internal abstract class XmlBaseConverter : XmlValueConverter
	{
		// Token: 0x06001E4E RID: 7758 RVA: 0x0008A2A4 File Offset: 0x000892A4
		protected XmlBaseConverter(XmlSchemaType schemaType)
		{
			XmlSchemaDatatype datatype = schemaType.Datatype;
			while (schemaType != null && !(schemaType is XmlSchemaSimpleType))
			{
				schemaType = schemaType.BaseXmlSchemaType;
			}
			if (schemaType == null)
			{
				schemaType = XmlSchemaType.GetBuiltInSimpleType(datatype.TypeCode);
			}
			this.schemaType = schemaType;
			this.typeCode = schemaType.TypeCode;
			this.clrTypeDefault = schemaType.Datatype.ValueType;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0008A308 File Offset: 0x00089308
		protected XmlBaseConverter(XmlTypeCode typeCode)
		{
			switch (typeCode)
			{
			case XmlTypeCode.Item:
				this.clrTypeDefault = XmlBaseConverter.XPathItemType;
				break;
			case XmlTypeCode.Node:
				this.clrTypeDefault = XmlBaseConverter.XPathNavigatorType;
				break;
			default:
				if (typeCode == XmlTypeCode.AnyAtomicType)
				{
					this.clrTypeDefault = XmlBaseConverter.XmlAtomicValueType;
				}
				break;
			}
			this.typeCode = typeCode;
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0008A360 File Offset: 0x00089360
		protected XmlBaseConverter(XmlBaseConverter converterAtomic)
		{
			this.schemaType = converterAtomic.schemaType;
			this.typeCode = converterAtomic.typeCode;
			this.clrTypeDefault = Array.CreateInstance(converterAtomic.DefaultClrType, 0).GetType();
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x0008A397 File Offset: 0x00089397
		protected XmlBaseConverter(XmlBaseConverter converterAtomic, Type clrTypeDefault)
		{
			this.schemaType = converterAtomic.schemaType;
			this.typeCode = converterAtomic.typeCode;
			this.clrTypeDefault = clrTypeDefault;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x0008A3BE File Offset: 0x000893BE
		public override bool ToBoolean(bool value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0008A3D7 File Offset: 0x000893D7
		public override bool ToBoolean(DateTime value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0008A3F0 File Offset: 0x000893F0
		public override bool ToBoolean(DateTimeOffset value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0008A409 File Offset: 0x00089409
		public override bool ToBoolean(decimal value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0008A422 File Offset: 0x00089422
		public override bool ToBoolean(double value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x0008A43B File Offset: 0x0008943B
		public override bool ToBoolean(int value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0008A454 File Offset: 0x00089454
		public override bool ToBoolean(long value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0008A46D File Offset: 0x0008946D
		public override bool ToBoolean(float value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0008A486 File Offset: 0x00089486
		public override bool ToBoolean(string value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0008A49A File Offset: 0x0008949A
		public override bool ToBoolean(object value)
		{
			return (bool)this.ChangeType(value, XmlBaseConverter.BooleanType, null);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0008A4AE File Offset: 0x000894AE
		public override DateTime ToDateTime(bool value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0008A4C7 File Offset: 0x000894C7
		public override DateTime ToDateTime(DateTime value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0008A4E0 File Offset: 0x000894E0
		public override DateTime ToDateTime(DateTimeOffset value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0008A4F9 File Offset: 0x000894F9
		public override DateTime ToDateTime(decimal value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0008A512 File Offset: 0x00089512
		public override DateTime ToDateTime(double value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0008A52B File Offset: 0x0008952B
		public override DateTime ToDateTime(int value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0008A544 File Offset: 0x00089544
		public override DateTime ToDateTime(long value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0008A55D File Offset: 0x0008955D
		public override DateTime ToDateTime(float value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0008A576 File Offset: 0x00089576
		public override DateTime ToDateTime(string value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0008A58A File Offset: 0x0008958A
		public override DateTime ToDateTime(object value)
		{
			return (DateTime)this.ChangeType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0008A59E File Offset: 0x0008959E
		public override DateTimeOffset ToDateTimeOffset(bool value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0008A5B7 File Offset: 0x000895B7
		public override DateTimeOffset ToDateTimeOffset(DateTime value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0008A5D0 File Offset: 0x000895D0
		public override DateTimeOffset ToDateTimeOffset(DateTimeOffset value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0008A5E9 File Offset: 0x000895E9
		public override DateTimeOffset ToDateTimeOffset(decimal value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0008A602 File Offset: 0x00089602
		public override DateTimeOffset ToDateTimeOffset(double value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0008A61B File Offset: 0x0008961B
		public override DateTimeOffset ToDateTimeOffset(int value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0008A634 File Offset: 0x00089634
		public override DateTimeOffset ToDateTimeOffset(long value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0008A64D File Offset: 0x0008964D
		public override DateTimeOffset ToDateTimeOffset(float value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0008A666 File Offset: 0x00089666
		public override DateTimeOffset ToDateTimeOffset(string value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0008A67A File Offset: 0x0008967A
		public override DateTimeOffset ToDateTimeOffset(object value)
		{
			return (DateTimeOffset)this.ChangeType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0008A68E File Offset: 0x0008968E
		public override decimal ToDecimal(bool value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0008A6A7 File Offset: 0x000896A7
		public override decimal ToDecimal(DateTime value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0008A6C0 File Offset: 0x000896C0
		public override decimal ToDecimal(DateTimeOffset value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0008A6D9 File Offset: 0x000896D9
		public override decimal ToDecimal(decimal value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0008A6F2 File Offset: 0x000896F2
		public override decimal ToDecimal(double value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0008A70B File Offset: 0x0008970B
		public override decimal ToDecimal(int value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0008A724 File Offset: 0x00089724
		public override decimal ToDecimal(long value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0008A73D File Offset: 0x0008973D
		public override decimal ToDecimal(float value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0008A756 File Offset: 0x00089756
		public override decimal ToDecimal(string value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0008A76A File Offset: 0x0008976A
		public override decimal ToDecimal(object value)
		{
			return (decimal)this.ChangeType(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0008A77E File Offset: 0x0008977E
		public override double ToDouble(bool value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0008A797 File Offset: 0x00089797
		public override double ToDouble(DateTime value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0008A7B0 File Offset: 0x000897B0
		public override double ToDouble(DateTimeOffset value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0008A7C9 File Offset: 0x000897C9
		public override double ToDouble(decimal value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0008A7E2 File Offset: 0x000897E2
		public override double ToDouble(double value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0008A7FB File Offset: 0x000897FB
		public override double ToDouble(int value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0008A814 File Offset: 0x00089814
		public override double ToDouble(long value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0008A82D File Offset: 0x0008982D
		public override double ToDouble(float value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0008A846 File Offset: 0x00089846
		public override double ToDouble(string value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0008A85A File Offset: 0x0008985A
		public override double ToDouble(object value)
		{
			return (double)this.ChangeType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0008A86E File Offset: 0x0008986E
		public override int ToInt32(bool value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0008A887 File Offset: 0x00089887
		public override int ToInt32(DateTime value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0008A8A0 File Offset: 0x000898A0
		public override int ToInt32(DateTimeOffset value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0008A8B9 File Offset: 0x000898B9
		public override int ToInt32(decimal value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0008A8D2 File Offset: 0x000898D2
		public override int ToInt32(double value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0008A8EB File Offset: 0x000898EB
		public override int ToInt32(int value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0008A904 File Offset: 0x00089904
		public override int ToInt32(long value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0008A91D File Offset: 0x0008991D
		public override int ToInt32(float value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0008A936 File Offset: 0x00089936
		public override int ToInt32(string value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0008A94A File Offset: 0x0008994A
		public override int ToInt32(object value)
		{
			return (int)this.ChangeType(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0008A95E File Offset: 0x0008995E
		public override long ToInt64(bool value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0008A977 File Offset: 0x00089977
		public override long ToInt64(DateTime value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x0008A990 File Offset: 0x00089990
		public override long ToInt64(DateTimeOffset value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0008A9A9 File Offset: 0x000899A9
		public override long ToInt64(decimal value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x0008A9C2 File Offset: 0x000899C2
		public override long ToInt64(double value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0008A9DB File Offset: 0x000899DB
		public override long ToInt64(int value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0008A9F4 File Offset: 0x000899F4
		public override long ToInt64(long value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0008AA0D File Offset: 0x00089A0D
		public override long ToInt64(float value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0008AA26 File Offset: 0x00089A26
		public override long ToInt64(string value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0008AA3A File Offset: 0x00089A3A
		public override long ToInt64(object value)
		{
			return (long)this.ChangeType(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0008AA4E File Offset: 0x00089A4E
		public override float ToSingle(bool value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0008AA67 File Offset: 0x00089A67
		public override float ToSingle(DateTime value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0008AA80 File Offset: 0x00089A80
		public override float ToSingle(DateTimeOffset value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0008AA99 File Offset: 0x00089A99
		public override float ToSingle(decimal value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0008AAB2 File Offset: 0x00089AB2
		public override float ToSingle(double value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0008AACB File Offset: 0x00089ACB
		public override float ToSingle(int value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0008AAE4 File Offset: 0x00089AE4
		public override float ToSingle(long value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0008AAFD File Offset: 0x00089AFD
		public override float ToSingle(float value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0008AB16 File Offset: 0x00089B16
		public override float ToSingle(string value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0008AB2A File Offset: 0x00089B2A
		public override float ToSingle(object value)
		{
			return (float)this.ChangeType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0008AB3E File Offset: 0x00089B3E
		public override string ToString(bool value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0008AB57 File Offset: 0x00089B57
		public override string ToString(DateTime value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0008AB70 File Offset: 0x00089B70
		public override string ToString(DateTimeOffset value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0008AB89 File Offset: 0x00089B89
		public override string ToString(decimal value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0008ABA2 File Offset: 0x00089BA2
		public override string ToString(double value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0008ABBB File Offset: 0x00089BBB
		public override string ToString(int value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0008ABD4 File Offset: 0x00089BD4
		public override string ToString(long value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0008ABED File Offset: 0x00089BED
		public override string ToString(float value)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, null);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0008AC06 File Offset: 0x00089C06
		public override string ToString(string value, IXmlNamespaceResolver nsResolver)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, nsResolver);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x0008AC1A File Offset: 0x00089C1A
		public override string ToString(object value, IXmlNamespaceResolver nsResolver)
		{
			return (string)this.ChangeType(value, XmlBaseConverter.StringType, nsResolver);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0008AC2E File Offset: 0x00089C2E
		public override string ToString(string value)
		{
			return this.ToString(value, null);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0008AC38 File Offset: 0x00089C38
		public override string ToString(object value)
		{
			return this.ToString(value, null);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0008AC42 File Offset: 0x00089C42
		public override object ChangeType(bool value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0008AC52 File Offset: 0x00089C52
		public override object ChangeType(DateTime value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0008AC62 File Offset: 0x00089C62
		public override object ChangeType(DateTimeOffset value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0008AC72 File Offset: 0x00089C72
		public override object ChangeType(decimal value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x0008AC82 File Offset: 0x00089C82
		public override object ChangeType(double value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0008AC92 File Offset: 0x00089C92
		public override object ChangeType(int value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0008ACA2 File Offset: 0x00089CA2
		public override object ChangeType(long value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0008ACB2 File Offset: 0x00089CB2
		public override object ChangeType(float value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0008ACC2 File Offset: 0x00089CC2
		public override object ChangeType(string value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			return this.ChangeType(value, destinationType, nsResolver);
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0008ACCD File Offset: 0x00089CCD
		public override object ChangeType(string value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0008ACD8 File Offset: 0x00089CD8
		public override object ChangeType(object value, Type destinationType)
		{
			return this.ChangeType(value, destinationType, null);
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x0008ACE3 File Offset: 0x00089CE3
		protected XmlSchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0008ACEB File Offset: 0x00089CEB
		protected XmlTypeCode TypeCode
		{
			get
			{
				return this.typeCode;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0008ACF4 File Offset: 0x00089CF4
		protected string XmlTypeName
		{
			get
			{
				XmlSchemaType baseXmlSchemaType = this.schemaType;
				if (baseXmlSchemaType != null)
				{
					while (baseXmlSchemaType.QualifiedName.IsEmpty)
					{
						baseXmlSchemaType = baseXmlSchemaType.BaseXmlSchemaType;
					}
					return XmlBaseConverter.QNameToString(baseXmlSchemaType.QualifiedName);
				}
				if (this.typeCode == XmlTypeCode.Node)
				{
					return "node";
				}
				if (this.typeCode == XmlTypeCode.AnyAtomicType)
				{
					return "xdt:anyAtomicType";
				}
				return "item";
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0008AD51 File Offset: 0x00089D51
		protected Type DefaultClrType
		{
			get
			{
				return this.clrTypeDefault;
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0008AD59 File Offset: 0x00089D59
		protected static bool IsDerivedFrom(Type derivedType, Type baseType)
		{
			while (derivedType != null)
			{
				if (derivedType == baseType)
				{
					return true;
				}
				derivedType = derivedType.BaseType;
			}
			return false;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0008AD70 File Offset: 0x00089D70
		protected Exception CreateInvalidClrMappingException(Type sourceType, Type destinationType)
		{
			if (sourceType == destinationType)
			{
				return new InvalidCastException(Res.GetString("XmlConvert_TypeBadMapping", new object[] { this.XmlTypeName, sourceType.Name }));
			}
			return new InvalidCastException(Res.GetString("XmlConvert_TypeBadMapping2", new object[] { this.XmlTypeName, sourceType.Name, destinationType.Name }));
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0008ADE0 File Offset: 0x00089DE0
		protected static string QNameToString(XmlQualifiedName name)
		{
			if (name.Namespace.Length == 0)
			{
				return name.Name;
			}
			if (name.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return "xs:" + name.Name;
			}
			if (name.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
			{
				return "xdt:" + name.Name;
			}
			return "{" + name.Namespace + "}" + name.Name;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0008AE62 File Offset: 0x00089E62
		protected virtual object ChangeListType(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			throw this.CreateInvalidClrMappingException(value.GetType(), destinationType);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0008AE71 File Offset: 0x00089E71
		protected static byte[] StringToBase64Binary(string value)
		{
			return Convert.FromBase64String(XmlConvert.TrimString(value));
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0008AE7E File Offset: 0x00089E7E
		protected static DateTime StringToDate(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Date);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0008AE8C File Offset: 0x00089E8C
		protected static DateTime StringToDateTime(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.DateTime);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0008AE9C File Offset: 0x00089E9C
		protected static TimeSpan StringToDayTimeDuration(string value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.DayTimeDuration).ToTimeSpan(XsdDuration.DurationType.DayTimeDuration);
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0008AEBC File Offset: 0x00089EBC
		protected static TimeSpan StringToDuration(string value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.Duration).ToTimeSpan(XsdDuration.DurationType.Duration);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0008AED9 File Offset: 0x00089ED9
		protected static DateTime StringToGDay(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GDay);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0008AEE8 File Offset: 0x00089EE8
		protected static DateTime StringToGMonth(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonth);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0008AEFA File Offset: 0x00089EFA
		protected static DateTime StringToGMonthDay(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonthDay);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x0008AF09 File Offset: 0x00089F09
		protected static DateTime StringToGYear(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYear);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0008AF18 File Offset: 0x00089F18
		protected static DateTime StringToGYearMonth(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYearMonth);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0008AF26 File Offset: 0x00089F26
		protected static DateTimeOffset StringToDateOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Date);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0008AF34 File Offset: 0x00089F34
		protected static DateTimeOffset StringToDateTimeOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.DateTime);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0008AF42 File Offset: 0x00089F42
		protected static DateTimeOffset StringToGDayOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GDay);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0008AF51 File Offset: 0x00089F51
		protected static DateTimeOffset StringToGMonthOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonth);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0008AF63 File Offset: 0x00089F63
		protected static DateTimeOffset StringToGMonthDayOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonthDay);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0008AF72 File Offset: 0x00089F72
		protected static DateTimeOffset StringToGYearOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYear);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0008AF81 File Offset: 0x00089F81
		protected static DateTimeOffset StringToGYearMonthOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYearMonth);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0008AF90 File Offset: 0x00089F90
		protected static byte[] StringToHexBinary(string value)
		{
			byte[] array;
			try
			{
				array = XmlConvert.FromBinHexString(XmlConvert.TrimString(value), false);
			}
			catch (XmlException ex)
			{
				throw new FormatException(ex.Message);
			}
			return array;
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0008AFCC File Offset: 0x00089FCC
		protected static XmlQualifiedName StringToQName(string value, IXmlNamespaceResolver nsResolver)
		{
			value = value.Trim();
			string text;
			string text2;
			try
			{
				ValidateNames.ParseQNameThrow(value, out text, out text2);
			}
			catch (XmlException ex)
			{
				throw new FormatException(ex.Message);
			}
			if (nsResolver == null)
			{
				throw new InvalidCastException(Res.GetString("XmlConvert_TypeNoNamespace", new object[] { value, text }));
			}
			string text3 = nsResolver.LookupNamespace(text);
			if (text3 == null)
			{
				throw new InvalidCastException(Res.GetString("XmlConvert_TypeNoNamespace", new object[] { value, text }));
			}
			return new XmlQualifiedName(text2, text3);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0008B068 File Offset: 0x0008A068
		protected static DateTime StringToTime(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Time);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0008B076 File Offset: 0x0008A076
		protected static DateTimeOffset StringToTimeOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Time);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0008B084 File Offset: 0x0008A084
		protected static TimeSpan StringToYearMonthDuration(string value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.YearMonthDuration).ToTimeSpan(XsdDuration.DurationType.YearMonthDuration);
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0008B0A1 File Offset: 0x0008A0A1
		protected static string AnyUriToString(Uri value)
		{
			return value.OriginalString;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0008B0A9 File Offset: 0x0008A0A9
		protected static string Base64BinaryToString(byte[] value)
		{
			return Convert.ToBase64String(value);
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0008B0B4 File Offset: 0x0008A0B4
		protected static string DateToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Date).ToString();
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x0008B0D8 File Offset: 0x0008A0D8
		protected static string DateTimeToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.DateTime).ToString();
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0008B0FC File Offset: 0x0008A0FC
		protected static string DayTimeDurationToString(TimeSpan value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.DayTimeDuration).ToString(XsdDuration.DurationType.DayTimeDuration);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0008B11C File Offset: 0x0008A11C
		protected static string DurationToString(TimeSpan value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.Duration).ToString(XsdDuration.DurationType.Duration);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0008B13C File Offset: 0x0008A13C
		protected static string GDayToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GDay).ToString();
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0008B160 File Offset: 0x0008A160
		protected static string GMonthToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonth).ToString();
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0008B188 File Offset: 0x0008A188
		protected static string GMonthDayToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonthDay).ToString();
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0008B1AC File Offset: 0x0008A1AC
		protected static string GYearToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYear).ToString();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0008B1D0 File Offset: 0x0008A1D0
		protected static string GYearMonthToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYearMonth).ToString();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0008B1F4 File Offset: 0x0008A1F4
		protected static string DateOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Date).ToString();
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0008B218 File Offset: 0x0008A218
		protected static string DateTimeOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.DateTime).ToString();
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0008B23C File Offset: 0x0008A23C
		protected static string GDayOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GDay).ToString();
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0008B260 File Offset: 0x0008A260
		protected static string GMonthOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonth).ToString();
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0008B288 File Offset: 0x0008A288
		protected static string GMonthDayOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GMonthDay).ToString();
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0008B2AC File Offset: 0x0008A2AC
		protected static string GYearOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYear).ToString();
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0008B2D0 File Offset: 0x0008A2D0
		protected static string GYearMonthOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.GYearMonth).ToString();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0008B2F4 File Offset: 0x0008A2F4
		protected static string QNameToString(XmlQualifiedName qname, IXmlNamespaceResolver nsResolver)
		{
			if (nsResolver == null)
			{
				return "{" + qname.Namespace + "}" + qname.Name;
			}
			string text = nsResolver.LookupPrefix(qname.Namespace);
			if (text == null)
			{
				throw new InvalidCastException(Res.GetString("XmlConvert_TypeNoPrefix", new object[]
				{
					qname.ToString(),
					qname.Namespace
				}));
			}
			if (text.Length == 0)
			{
				return qname.Name;
			}
			return text + ":" + qname.Name;
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0008B37C File Offset: 0x0008A37C
		protected static string TimeToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Time).ToString();
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0008B3A0 File Offset: 0x0008A3A0
		protected static string TimeOffsetToString(DateTimeOffset value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.Time).ToString();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0008B3C4 File Offset: 0x0008A3C4
		protected static string YearMonthDurationToString(TimeSpan value)
		{
			return new XsdDuration(value, XsdDuration.DurationType.YearMonthDuration).ToString(XsdDuration.DurationType.YearMonthDuration);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x0008B3E1 File Offset: 0x0008A3E1
		internal static DateTime DateTimeOffsetToDateTime(DateTimeOffset value)
		{
			return value.LocalDateTime;
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x0008B3EC File Offset: 0x0008A3EC
		internal static int DecimalToInt32(decimal value)
		{
			if (value < -2147483648m || value > 2147483647m)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"Int32"
				}));
			}
			return (int)value;
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0008B44C File Offset: 0x0008A44C
		protected static long DecimalToInt64(decimal value)
		{
			if (value < -9223372036854775808m || value > 9223372036854775807m)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"Int64"
				}));
			}
			return (long)value;
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0008B4B4 File Offset: 0x0008A4B4
		protected static ulong DecimalToUInt64(decimal value)
		{
			if (value < 0m || value > 18446744073709551615m)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"UInt64"
				}));
			}
			return (ulong)value;
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0008B510 File Offset: 0x0008A510
		protected static byte Int32ToByte(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"Byte"
				}));
			}
			return (byte)value;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0008B554 File Offset: 0x0008A554
		protected static short Int32ToInt16(int value)
		{
			if (value < -32768 || value > 32767)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"Int16"
				}));
			}
			return (short)value;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0008B59C File Offset: 0x0008A59C
		protected static sbyte Int32ToSByte(int value)
		{
			if (value < -128 || value > 127)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"SByte"
				}));
			}
			return (sbyte)value;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0008B5E0 File Offset: 0x0008A5E0
		protected static ushort Int32ToUInt16(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"UInt16"
				}));
			}
			return (ushort)value;
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0008B624 File Offset: 0x0008A624
		protected static int Int64ToInt32(long value)
		{
			if (value < -2147483648L || value > 2147483647L)
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"Int32"
				}));
			}
			return (int)value;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0008B670 File Offset: 0x0008A670
		protected static uint Int64ToUInt32(long value)
		{
			if (value < 0L || value > (long)((ulong)(-1)))
			{
				throw new OverflowException(Res.GetString("XmlConvert_Overflow", new string[]
				{
					XmlConvert.ToString(value),
					"UInt32"
				}));
			}
			return (uint)value;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0008B6B2 File Offset: 0x0008A6B2
		protected static DateTime UntypedAtomicToDateTime(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.AllXsd);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0008B6C4 File Offset: 0x0008A6C4
		protected static DateTimeOffset UntypedAtomicToDateTimeOffset(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.AllXsd);
		}

		// Token: 0x04001284 RID: 4740
		private XmlSchemaType schemaType;

		// Token: 0x04001285 RID: 4741
		private XmlTypeCode typeCode;

		// Token: 0x04001286 RID: 4742
		private Type clrTypeDefault;

		// Token: 0x04001287 RID: 4743
		protected static readonly Type ICollectionType = typeof(ICollection);

		// Token: 0x04001288 RID: 4744
		protected static readonly Type IEnumerableType = typeof(IEnumerable);

		// Token: 0x04001289 RID: 4745
		protected static readonly Type IListType = typeof(IList);

		// Token: 0x0400128A RID: 4746
		protected static readonly Type ObjectArrayType = typeof(object[]);

		// Token: 0x0400128B RID: 4747
		protected static readonly Type StringArrayType = typeof(string[]);

		// Token: 0x0400128C RID: 4748
		protected static readonly Type XmlAtomicValueArrayType = typeof(XmlAtomicValue[]);

		// Token: 0x0400128D RID: 4749
		protected static readonly Type DecimalType = typeof(decimal);

		// Token: 0x0400128E RID: 4750
		protected static readonly Type Int32Type = typeof(int);

		// Token: 0x0400128F RID: 4751
		protected static readonly Type Int64Type = typeof(long);

		// Token: 0x04001290 RID: 4752
		protected static readonly Type StringType = typeof(string);

		// Token: 0x04001291 RID: 4753
		protected static readonly Type XmlAtomicValueType = typeof(XmlAtomicValue);

		// Token: 0x04001292 RID: 4754
		protected static readonly Type ObjectType = typeof(object);

		// Token: 0x04001293 RID: 4755
		protected static readonly Type ByteType = typeof(byte);

		// Token: 0x04001294 RID: 4756
		protected static readonly Type Int16Type = typeof(short);

		// Token: 0x04001295 RID: 4757
		protected static readonly Type SByteType = typeof(sbyte);

		// Token: 0x04001296 RID: 4758
		protected static readonly Type UInt16Type = typeof(ushort);

		// Token: 0x04001297 RID: 4759
		protected static readonly Type UInt32Type = typeof(uint);

		// Token: 0x04001298 RID: 4760
		protected static readonly Type UInt64Type = typeof(ulong);

		// Token: 0x04001299 RID: 4761
		protected static readonly Type XPathItemType = typeof(XPathItem);

		// Token: 0x0400129A RID: 4762
		protected static readonly Type DoubleType = typeof(double);

		// Token: 0x0400129B RID: 4763
		protected static readonly Type SingleType = typeof(float);

		// Token: 0x0400129C RID: 4764
		protected static readonly Type DateTimeType = typeof(DateTime);

		// Token: 0x0400129D RID: 4765
		protected static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);

		// Token: 0x0400129E RID: 4766
		protected static readonly Type BooleanType = typeof(bool);

		// Token: 0x0400129F RID: 4767
		protected static readonly Type ByteArrayType = typeof(byte[]);

		// Token: 0x040012A0 RID: 4768
		protected static readonly Type XmlQualifiedNameType = typeof(XmlQualifiedName);

		// Token: 0x040012A1 RID: 4769
		protected static readonly Type UriType = typeof(Uri);

		// Token: 0x040012A2 RID: 4770
		protected static readonly Type TimeSpanType = typeof(TimeSpan);

		// Token: 0x040012A3 RID: 4771
		protected static readonly Type XPathNavigatorType = typeof(XPathNavigator);
	}
}

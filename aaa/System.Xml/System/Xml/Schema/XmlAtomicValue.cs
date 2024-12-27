using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x02000229 RID: 553
	public sealed class XmlAtomicValue : XPathItem, ICloneable
	{
		// Token: 0x06001A4C RID: 6732 RVA: 0x0007F3CD File Offset: 0x0007E3CD
		internal XmlAtomicValue(XmlSchemaType xmlType, bool value)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.clrType = TypeCode.Boolean;
			this.unionVal.boolVal = value;
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x0007F3FD File Offset: 0x0007E3FD
		internal XmlAtomicValue(XmlSchemaType xmlType, DateTime value)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.clrType = TypeCode.DateTime;
			this.unionVal.dtVal = value;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0007F42E File Offset: 0x0007E42E
		internal XmlAtomicValue(XmlSchemaType xmlType, double value)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.clrType = TypeCode.Double;
			this.unionVal.dblVal = value;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0007F45F File Offset: 0x0007E45F
		internal XmlAtomicValue(XmlSchemaType xmlType, int value)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.clrType = TypeCode.Int32;
			this.unionVal.i32Val = value;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0007F490 File Offset: 0x0007E490
		internal XmlAtomicValue(XmlSchemaType xmlType, long value)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.clrType = TypeCode.Int64;
			this.unionVal.i64Val = value;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0007F4C1 File Offset: 0x0007E4C1
		internal XmlAtomicValue(XmlSchemaType xmlType, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.objVal = value;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0007F4F4 File Offset: 0x0007E4F4
		internal XmlAtomicValue(XmlSchemaType xmlType, string value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.objVal = value;
			if (nsResolver != null && (this.xmlType.TypeCode == XmlTypeCode.QName || this.xmlType.TypeCode == XmlTypeCode.Notation))
			{
				string prefixFromQName = this.GetPrefixFromQName(value);
				this.nsPrefix = new XmlAtomicValue.NamespacePrefixForQName(prefixFromQName, nsResolver.LookupNamespace(prefixFromQName));
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0007F56D File Offset: 0x0007E56D
		internal XmlAtomicValue(XmlSchemaType xmlType, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.objVal = value;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0007F5A0 File Offset: 0x0007E5A0
		internal XmlAtomicValue(XmlSchemaType xmlType, object value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlType = xmlType;
			this.objVal = value;
			if (nsResolver != null && (this.xmlType.TypeCode == XmlTypeCode.QName || this.xmlType.TypeCode == XmlTypeCode.Notation))
			{
				XmlQualifiedName xmlQualifiedName = this.objVal as XmlQualifiedName;
				string @namespace = xmlQualifiedName.Namespace;
				this.nsPrefix = new XmlAtomicValue.NamespacePrefixForQName(nsResolver.LookupPrefix(@namespace), @namespace);
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0007F624 File Offset: 0x0007E624
		public XmlAtomicValue Clone()
		{
			return this;
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0007F627 File Offset: 0x0007E627
		object ICloneable.Clone()
		{
			return this;
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001A57 RID: 6743 RVA: 0x0007F62A File Offset: 0x0007E62A
		public override bool IsNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0007F62D File Offset: 0x0007E62D
		public override XmlSchemaType XmlType
		{
			get
			{
				return this.xmlType;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0007F635 File Offset: 0x0007E635
		public override Type ValueType
		{
			get
			{
				return this.xmlType.Datatype.ValueType;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0007F648 File Offset: 0x0007E648
		public override object TypedValue
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ChangeType(this.unionVal.boolVal, this.ValueType);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ChangeType(this.unionVal.i32Val, this.ValueType);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ChangeType(this.unionVal.i64Val, this.ValueType);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ChangeType(this.unionVal.dblVal, this.ValueType);
						case TypeCode.DateTime:
							return valueConverter.ChangeType(this.unionVal.dtVal, this.ValueType);
						}
						break;
					}
				}
				return valueConverter.ChangeType(this.objVal, this.ValueType, this.nsPrefix);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0007F734 File Offset: 0x0007E734
		public override bool ValueAsBoolean
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return this.unionVal.boolVal;
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ToBoolean(this.unionVal.i32Val);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ToBoolean(this.unionVal.i64Val);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ToBoolean(this.unionVal.dblVal);
						case TypeCode.DateTime:
							return valueConverter.ToBoolean(this.unionVal.dtVal);
						}
						break;
					}
				}
				return valueConverter.ToBoolean(this.objVal);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0007F7F0 File Offset: 0x0007E7F0
		public override DateTime ValueAsDateTime
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ToDateTime(this.unionVal.boolVal);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ToDateTime(this.unionVal.i32Val);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ToDateTime(this.unionVal.i64Val);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ToDateTime(this.unionVal.dblVal);
						case TypeCode.DateTime:
							return this.unionVal.dtVal;
						}
						break;
					}
				}
				return valueConverter.ToDateTime(this.objVal);
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0007F8AC File Offset: 0x0007E8AC
		public override double ValueAsDouble
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ToDouble(this.unionVal.boolVal);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ToDouble(this.unionVal.i32Val);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ToDouble(this.unionVal.i64Val);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return this.unionVal.dblVal;
						case TypeCode.DateTime:
							return valueConverter.ToDouble(this.unionVal.dtVal);
						}
						break;
					}
				}
				return valueConverter.ToDouble(this.objVal);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x0007F968 File Offset: 0x0007E968
		public override int ValueAsInt
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ToInt32(this.unionVal.boolVal);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return this.unionVal.i32Val;
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ToInt32(this.unionVal.i64Val);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ToInt32(this.unionVal.dblVal);
						case TypeCode.DateTime:
							return valueConverter.ToInt32(this.unionVal.dtVal);
						}
						break;
					}
				}
				return valueConverter.ToInt32(this.objVal);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x0007FA24 File Offset: 0x0007EA24
		public override long ValueAsLong
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ToInt64(this.unionVal.boolVal);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ToInt64(this.unionVal.i32Val);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return this.unionVal.i64Val;
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ToInt64(this.unionVal.dblVal);
						case TypeCode.DateTime:
							return valueConverter.ToInt64(this.unionVal.dtVal);
						}
						break;
					}
				}
				return valueConverter.ToInt64(this.objVal);
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0007FAE0 File Offset: 0x0007EAE0
		public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver)
		{
			XmlValueConverter valueConverter = this.xmlType.ValueConverter;
			if (type == typeof(XPathItem) || type == typeof(XmlAtomicValue))
			{
				return this;
			}
			if (this.objVal == null)
			{
				TypeCode typeCode = this.clrType;
				if (typeCode == TypeCode.Boolean)
				{
					return valueConverter.ChangeType(this.unionVal.boolVal, type);
				}
				switch (typeCode)
				{
				case TypeCode.Int32:
					return valueConverter.ChangeType(this.unionVal.i32Val, type);
				case TypeCode.UInt32:
					break;
				case TypeCode.Int64:
					return valueConverter.ChangeType(this.unionVal.i64Val, type);
				default:
					switch (typeCode)
					{
					case TypeCode.Double:
						return valueConverter.ChangeType(this.unionVal.dblVal, type);
					case TypeCode.DateTime:
						return valueConverter.ChangeType(this.unionVal.dtVal, type);
					}
					break;
				}
			}
			return valueConverter.ChangeType(this.objVal, type, nsResolver);
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0007FBC4 File Offset: 0x0007EBC4
		public override string Value
		{
			get
			{
				XmlValueConverter valueConverter = this.xmlType.ValueConverter;
				if (this.objVal == null)
				{
					TypeCode typeCode = this.clrType;
					if (typeCode == TypeCode.Boolean)
					{
						return valueConverter.ToString(this.unionVal.boolVal);
					}
					switch (typeCode)
					{
					case TypeCode.Int32:
						return valueConverter.ToString(this.unionVal.i32Val);
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						return valueConverter.ToString(this.unionVal.i64Val);
					default:
						switch (typeCode)
						{
						case TypeCode.Double:
							return valueConverter.ToString(this.unionVal.dblVal);
						case TypeCode.DateTime:
							return valueConverter.ToString(this.unionVal.dtVal);
						}
						break;
					}
				}
				return valueConverter.ToString(this.objVal, this.nsPrefix);
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0007FC8B File Offset: 0x0007EC8B
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0007FC94 File Offset: 0x0007EC94
		private string GetPrefixFromQName(string value)
		{
			int num2;
			int num = ValidateNames.ParseQName(value, 0, out num2);
			if (num == 0 || num != value.Length)
			{
				return null;
			}
			if (num2 != 0)
			{
				return value.Substring(0, num2);
			}
			return string.Empty;
		}

		// Token: 0x040010A1 RID: 4257
		private XmlSchemaType xmlType;

		// Token: 0x040010A2 RID: 4258
		private object objVal;

		// Token: 0x040010A3 RID: 4259
		private TypeCode clrType;

		// Token: 0x040010A4 RID: 4260
		private XmlAtomicValue.Union unionVal;

		// Token: 0x040010A5 RID: 4261
		private XmlAtomicValue.NamespacePrefixForQName nsPrefix;

		// Token: 0x0200022A RID: 554
		[StructLayout(LayoutKind.Explicit, Size = 8)]
		private struct Union
		{
			// Token: 0x040010A6 RID: 4262
			[FieldOffset(0)]
			public bool boolVal;

			// Token: 0x040010A7 RID: 4263
			[FieldOffset(0)]
			public double dblVal;

			// Token: 0x040010A8 RID: 4264
			[FieldOffset(0)]
			public long i64Val;

			// Token: 0x040010A9 RID: 4265
			[FieldOffset(0)]
			public int i32Val;

			// Token: 0x040010AA RID: 4266
			[FieldOffset(0)]
			public DateTime dtVal;
		}

		// Token: 0x0200022B RID: 555
		private class NamespacePrefixForQName : IXmlNamespaceResolver
		{
			// Token: 0x06001A64 RID: 6756 RVA: 0x0007FCCA File Offset: 0x0007ECCA
			public NamespacePrefixForQName(string prefix, string ns)
			{
				this.ns = ns;
				this.prefix = prefix;
			}

			// Token: 0x06001A65 RID: 6757 RVA: 0x0007FCE0 File Offset: 0x0007ECE0
			public string LookupNamespace(string prefix)
			{
				if (prefix == this.prefix)
				{
					return this.ns;
				}
				return null;
			}

			// Token: 0x06001A66 RID: 6758 RVA: 0x0007FCF8 File Offset: 0x0007ECF8
			public string LookupPrefix(string namespaceName)
			{
				if (this.ns == namespaceName)
				{
					return this.prefix;
				}
				return null;
			}

			// Token: 0x06001A67 RID: 6759 RVA: 0x0007FD10 File Offset: 0x0007ED10
			public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>(1);
				dictionary[this.prefix] = this.ns;
				return dictionary;
			}

			// Token: 0x040010AB RID: 4267
			public string prefix;

			// Token: 0x040010AC RID: 4268
			public string ns;
		}
	}
}

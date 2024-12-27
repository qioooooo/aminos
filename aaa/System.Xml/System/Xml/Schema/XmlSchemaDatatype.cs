using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.Schema
{
	// Token: 0x020001AB RID: 427
	public abstract class XmlSchemaDatatype
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060015BE RID: 5566
		public abstract Type ValueType { get; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060015BF RID: 5567
		public abstract XmlTokenizedType TokenizedType { get; }

		// Token: 0x060015C0 RID: 5568
		public abstract object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr);

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00060B26 File Offset: 0x0005FB26
		public virtual XmlSchemaDatatypeVariety Variety
		{
			get
			{
				return XmlSchemaDatatypeVariety.Atomic;
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00060B29 File Offset: 0x0005FB29
		public virtual object ChangeType(object value, Type targetType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			return this.ValueConverter.ChangeType(value, targetType);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00060B54 File Offset: 0x0005FB54
		public virtual object ChangeType(object value, Type targetType, IXmlNamespaceResolver namespaceResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (namespaceResolver == null)
			{
				throw new ArgumentNullException("namespaceResolver");
			}
			return this.ValueConverter.ChangeType(value, targetType, namespaceResolver);
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00060B8E File Offset: 0x0005FB8E
		public virtual XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.None;
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00060B91 File Offset: 0x0005FB91
		public virtual bool IsDerivedFrom(XmlSchemaDatatype datatype)
		{
			return false;
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060015C6 RID: 5574
		internal abstract bool HasLexicalFacets { get; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060015C7 RID: 5575
		internal abstract bool HasValueFacets { get; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060015C8 RID: 5576
		internal abstract XmlValueConverter ValueConverter { get; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060015C9 RID: 5577
		// (set) Token: 0x060015CA RID: 5578
		internal abstract RestrictionFacets Restriction { get; set; }

		// Token: 0x060015CB RID: 5579
		internal abstract int Compare(object value1, object value2);

		// Token: 0x060015CC RID: 5580
		internal abstract object ParseValue(string s, Type typDest, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr);

		// Token: 0x060015CD RID: 5581
		internal abstract object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, bool createAtomicValue);

		// Token: 0x060015CE RID: 5582
		internal abstract Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue);

		// Token: 0x060015CF RID: 5583
		internal abstract Exception TryParseValue(object value, XmlNameTable nameTable, IXmlNamespaceResolver namespaceResolver, out object typedValue);

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060015D0 RID: 5584
		internal abstract FacetsChecker FacetsChecker { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060015D1 RID: 5585
		internal abstract XmlSchemaWhiteSpace BuiltInWhitespaceFacet { get; }

		// Token: 0x060015D2 RID: 5586
		internal abstract XmlSchemaDatatype DeriveByRestriction(XmlSchemaObjectCollection facets, XmlNameTable nameTable, XmlSchemaType schemaType);

		// Token: 0x060015D3 RID: 5587
		internal abstract XmlSchemaDatatype DeriveByList(XmlSchemaType schemaType);

		// Token: 0x060015D4 RID: 5588
		internal abstract void VerifySchemaValid(XmlSchemaObjectTable notations, XmlSchemaObject caller);

		// Token: 0x060015D5 RID: 5589
		internal abstract bool IsEqual(object o1, object o2);

		// Token: 0x060015D6 RID: 5590
		internal abstract bool IsComparable(XmlSchemaDatatype dtype);

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00060B94 File Offset: 0x0005FB94
		internal string TypeCodeString
		{
			get
			{
				string text = string.Empty;
				XmlTypeCode typeCode = this.TypeCode;
				switch (this.Variety)
				{
				case XmlSchemaDatatypeVariety.Atomic:
					if (typeCode == XmlTypeCode.AnyAtomicType)
					{
						text = "anySimpleType";
					}
					else
					{
						text = this.TypeCodeToString(typeCode);
					}
					break;
				case XmlSchemaDatatypeVariety.List:
					if (typeCode == XmlTypeCode.AnyAtomicType)
					{
						text = "List of Union";
					}
					else
					{
						text = "List of " + this.TypeCodeToString(typeCode);
					}
					break;
				case XmlSchemaDatatypeVariety.Union:
					text = "Union";
					break;
				}
				return text;
			}
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00060C08 File Offset: 0x0005FC08
		internal string TypeCodeToString(XmlTypeCode typeCode)
		{
			switch (typeCode)
			{
			case XmlTypeCode.None:
				return "None";
			case XmlTypeCode.Item:
				return "AnyType";
			case XmlTypeCode.AnyAtomicType:
				return "AnyAtomicType";
			case XmlTypeCode.String:
				return "String";
			case XmlTypeCode.Boolean:
				return "Boolean";
			case XmlTypeCode.Decimal:
				return "Decimal";
			case XmlTypeCode.Float:
				return "Float";
			case XmlTypeCode.Double:
				return "Double";
			case XmlTypeCode.Duration:
				return "Duration";
			case XmlTypeCode.DateTime:
				return "DateTime";
			case XmlTypeCode.Time:
				return "Time";
			case XmlTypeCode.Date:
				return "Date";
			case XmlTypeCode.GYearMonth:
				return "GYearMonth";
			case XmlTypeCode.GYear:
				return "GYear";
			case XmlTypeCode.GMonthDay:
				return "GMonthDay";
			case XmlTypeCode.GDay:
				return "GDay";
			case XmlTypeCode.GMonth:
				return "GMonth";
			case XmlTypeCode.HexBinary:
				return "HexBinary";
			case XmlTypeCode.Base64Binary:
				return "Base64Binary";
			case XmlTypeCode.AnyUri:
				return "AnyUri";
			case XmlTypeCode.QName:
				return "QName";
			case XmlTypeCode.Notation:
				return "Notation";
			case XmlTypeCode.NormalizedString:
				return "NormalizedString";
			case XmlTypeCode.Token:
				return "Token";
			case XmlTypeCode.Language:
				return "Language";
			case XmlTypeCode.NmToken:
				return "NmToken";
			case XmlTypeCode.Name:
				return "Name";
			case XmlTypeCode.NCName:
				return "NCName";
			case XmlTypeCode.Id:
				return "Id";
			case XmlTypeCode.Idref:
				return "Idref";
			case XmlTypeCode.Entity:
				return "Entity";
			case XmlTypeCode.Integer:
				return "Integer";
			case XmlTypeCode.NonPositiveInteger:
				return "NonPositiveInteger";
			case XmlTypeCode.NegativeInteger:
				return "NegativeInteger";
			case XmlTypeCode.Long:
				return "Long";
			case XmlTypeCode.Int:
				return "Int";
			case XmlTypeCode.Short:
				return "Short";
			case XmlTypeCode.Byte:
				return "Byte";
			case XmlTypeCode.NonNegativeInteger:
				return "NonNegativeInteger";
			case XmlTypeCode.UnsignedLong:
				return "UnsignedLong";
			case XmlTypeCode.UnsignedInt:
				return "UnsignedInt";
			case XmlTypeCode.UnsignedShort:
				return "UnsignedShort";
			case XmlTypeCode.UnsignedByte:
				return "UnsignedByte";
			case XmlTypeCode.PositiveInteger:
				return "PositiveInteger";
			}
			return typeCode.ToString();
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00060E0C File Offset: 0x0005FE0C
		internal static string ConcatenatedToString(object value)
		{
			Type type = value.GetType();
			string text = string.Empty;
			if (type == typeof(IEnumerable) && type != typeof(string))
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerator enumerator = (value as IEnumerable).GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("{");
					object obj = enumerator.Current;
					if (obj is IFormattable)
					{
						stringBuilder.Append(((IFormattable)obj).ToString("", CultureInfo.InvariantCulture));
					}
					else
					{
						stringBuilder.Append(obj.ToString());
					}
					while (enumerator.MoveNext())
					{
						stringBuilder.Append(" , ");
						obj = enumerator.Current;
						if (obj is IFormattable)
						{
							stringBuilder.Append(((IFormattable)obj).ToString("", CultureInfo.InvariantCulture));
						}
						else
						{
							stringBuilder.Append(obj.ToString());
						}
					}
					stringBuilder.Append("}");
					text = stringBuilder.ToString();
				}
			}
			else if (value is IFormattable)
			{
				text = ((IFormattable)value).ToString("", CultureInfo.InvariantCulture);
			}
			else
			{
				text = value.ToString();
			}
			return text;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00060F3E File Offset: 0x0005FF3E
		internal static XmlSchemaDatatype FromXmlTokenizedType(XmlTokenizedType token)
		{
			return DatatypeImplementation.FromXmlTokenizedType(token);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00060F46 File Offset: 0x0005FF46
		internal static XmlSchemaDatatype FromXmlTokenizedTypeXsd(XmlTokenizedType token)
		{
			return DatatypeImplementation.FromXmlTokenizedTypeXsd(token);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00060F4E File Offset: 0x0005FF4E
		internal static XmlSchemaDatatype FromXdrName(string name)
		{
			return DatatypeImplementation.FromXdrName(name);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00060F56 File Offset: 0x0005FF56
		internal static XmlSchemaDatatype DeriveByUnion(XmlSchemaSimpleType[] types, XmlSchemaType schemaType)
		{
			return DatatypeImplementation.DeriveByUnion(types, schemaType);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00060F60 File Offset: 0x0005FF60
		internal static string XdrCanonizeUri(string uri, XmlNameTable nameTable, SchemaNames schemaNames)
		{
			int num = 5;
			bool flag = false;
			if (uri.Length > 5 && uri.StartsWith("uuid:", StringComparison.Ordinal))
			{
				flag = true;
			}
			else if (uri.Length > 9 && uri.StartsWith("urn:uuid:", StringComparison.Ordinal))
			{
				flag = true;
				num = 9;
			}
			string text;
			if (flag)
			{
				text = nameTable.Add(uri.Substring(0, num) + uri.Substring(num, uri.Length - num).ToUpper(CultureInfo.InvariantCulture));
			}
			else
			{
				text = uri;
			}
			if (Ref.Equal(schemaNames.NsDataTypeAlias, text) || Ref.Equal(schemaNames.NsDataTypeOld, text))
			{
				text = schemaNames.NsDataType;
			}
			else if (Ref.Equal(schemaNames.NsXdrAlias, text))
			{
				text = schemaNames.NsXdr;
			}
			return text;
		}
	}
}

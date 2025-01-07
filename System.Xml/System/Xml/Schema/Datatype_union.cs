﻿using System;

namespace System.Xml.Schema
{
	internal class Datatype_union : Datatype_anySimpleType
	{
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlUnionConverter.Create(schemaType);
		}

		internal Datatype_union(XmlSchemaSimpleType[] types)
		{
			this.types = types;
		}

		internal override int Compare(object value1, object value2)
		{
			XsdSimpleValue xsdSimpleValue = value1 as XsdSimpleValue;
			XsdSimpleValue xsdSimpleValue2 = value2 as XsdSimpleValue;
			if (xsdSimpleValue == null || xsdSimpleValue2 == null)
			{
				return -1;
			}
			XmlSchemaType xmlType = xsdSimpleValue.XmlType;
			XmlSchemaType xmlType2 = xsdSimpleValue2.XmlType;
			if (xmlType == xmlType2)
			{
				XmlSchemaDatatype datatype = xmlType.Datatype;
				return datatype.Compare(xsdSimpleValue.TypedValue, xsdSimpleValue2.TypedValue);
			}
			return -1;
		}

		public override Type ValueType
		{
			get
			{
				return Datatype_union.atomicValueType;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyAtomicType;
			}
		}

		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.unionFacetsChecker;
			}
		}

		internal override Type ListValueType
		{
			get
			{
				return Datatype_union.listValueType;
			}
		}

		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration;
			}
		}

		internal XmlSchemaSimpleType[] BaseMemberTypes
		{
			get
			{
				return this.types;
			}
		}

		internal bool HasAtomicMembers()
		{
			foreach (XmlSchemaSimpleType xmlSchemaSimpleType in this.types)
			{
				if (xmlSchemaSimpleType.Datatype.Variety == XmlSchemaDatatypeVariety.List)
				{
					return false;
				}
			}
			return true;
		}

		internal bool IsUnionBaseOf(DatatypeImplementation derivedType)
		{
			foreach (XmlSchemaSimpleType xmlSchemaSimpleType in this.types)
			{
				if (derivedType.IsDerivedFrom(xmlSchemaSimpleType.Datatype))
				{
					return true;
				}
			}
			return false;
		}

		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = null;
			typedValue = null;
			Exception ex = DatatypeImplementation.unionFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				foreach (XmlSchemaSimpleType xmlSchemaSimpleType2 in this.types)
				{
					if (xmlSchemaSimpleType2.Datatype.TryParseValue(s, nameTable, nsmgr, out typedValue) == null)
					{
						xmlSchemaSimpleType = xmlSchemaSimpleType2;
						break;
					}
				}
				if (xmlSchemaSimpleType == null)
				{
					ex = new XmlSchemaException("Sch_UnionFailedEx", s);
				}
				else
				{
					typedValue = new XsdSimpleValue(xmlSchemaSimpleType, typedValue);
					ex = DatatypeImplementation.unionFacetsChecker.CheckValueFacets(typedValue, this);
					if (ex == null)
					{
						return null;
					}
				}
			}
			return ex;
		}

		internal override Exception TryParseValue(object value, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			typedValue = null;
			string text = value as string;
			if (text != null)
			{
				return this.TryParseValue(text, nameTable, nsmgr, out typedValue);
			}
			object obj = null;
			XmlSchemaSimpleType xmlSchemaSimpleType = null;
			foreach (XmlSchemaSimpleType xmlSchemaSimpleType2 in this.types)
			{
				if (xmlSchemaSimpleType2.Datatype.TryParseValue(value, nameTable, nsmgr, out obj) == null)
				{
					xmlSchemaSimpleType = xmlSchemaSimpleType2;
					break;
				}
			}
			Exception ex;
			if (obj != null)
			{
				try
				{
					if (this.HasLexicalFacets)
					{
						string text2 = (string)this.ValueConverter.ChangeType(obj, typeof(string), nsmgr);
						ex = DatatypeImplementation.unionFacetsChecker.CheckLexicalFacets(ref text2, this);
						if (ex != null)
						{
							return ex;
						}
					}
					typedValue = new XsdSimpleValue(xmlSchemaSimpleType, obj);
					if (this.HasValueFacets)
					{
						ex = DatatypeImplementation.unionFacetsChecker.CheckValueFacets(typedValue, this);
						if (ex != null)
						{
							return ex;
						}
					}
					return null;
				}
				catch (FormatException ex2)
				{
					ex = ex2;
				}
				catch (InvalidCastException ex3)
				{
					ex = ex3;
				}
				catch (OverflowException ex4)
				{
					ex = ex4;
				}
				catch (ArgumentException ex5)
				{
					ex = ex5;
				}
				return ex;
			}
			ex = new XmlSchemaException("Sch_UnionFailedEx", value.ToString());
			return ex;
		}

		private static readonly Type atomicValueType = typeof(object);

		private static readonly Type listValueType = typeof(object[]);

		private XmlSchemaSimpleType[] types;
	}
}

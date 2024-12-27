using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	// Token: 0x02000014 RID: 20
	internal static class XmlQueryTypeFactory
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004E8E File Offset: 0x00003E8E
		public static XmlQueryType Type(XmlTypeCode code, bool isStrict)
		{
			return XmlQueryTypeFactory.ItemType.Create(code, isStrict);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004E98 File Offset: 0x00003E98
		public static XmlQueryType Type(XmlSchemaSimpleType schemaType, bool isStrict)
		{
			if (schemaType.Datatype.Variety == XmlSchemaDatatypeVariety.Atomic)
			{
				if (schemaType == DatatypeImplementation.AnySimpleType)
				{
					return XmlQueryTypeFactory.AnyAtomicTypeS;
				}
				return XmlQueryTypeFactory.ItemType.Create(schemaType, isStrict);
			}
			else
			{
				while (schemaType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
				{
					schemaType = (XmlSchemaSimpleType)schemaType.BaseXmlSchemaType;
				}
				if (schemaType.DerivedBy == XmlSchemaDerivationMethod.List)
				{
					return XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Type(((XmlSchemaSimpleTypeList)schemaType.Content).BaseItemType, isStrict), XmlQueryCardinality.ZeroOrMore);
				}
				XmlSchemaSimpleType[] baseMemberTypes = ((XmlSchemaSimpleTypeUnion)schemaType.Content).BaseMemberTypes;
				XmlQueryType[] array = new XmlQueryType[baseMemberTypes.Length];
				for (int i = 0; i < baseMemberTypes.Length; i++)
				{
					array[i] = XmlQueryTypeFactory.Type(baseMemberTypes[i], isStrict);
				}
				return XmlQueryTypeFactory.Choice(array);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004F42 File Offset: 0x00003F42
		public static XmlQueryType Choice(XmlQueryType left, XmlQueryType right)
		{
			return XmlQueryTypeFactory.SequenceType.Create(XmlQueryTypeFactory.ChoiceType.Create(XmlQueryTypeFactory.PrimeChoice(new List<XmlQueryType>(left), right)), left.Cardinality | right.Cardinality);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004F6C File Offset: 0x00003F6C
		public static XmlQueryType Choice(params XmlQueryType[] types)
		{
			if (types.Length == 0)
			{
				return XmlQueryTypeFactory.None;
			}
			if (types.Length == 1)
			{
				return types[0];
			}
			List<XmlQueryType> list = new List<XmlQueryType>(types[0]);
			XmlQueryCardinality xmlQueryCardinality = types[0].Cardinality;
			for (int i = 1; i < types.Length; i++)
			{
				XmlQueryTypeFactory.PrimeChoice(list, types[i]);
				xmlQueryCardinality |= types[i].Cardinality;
			}
			return XmlQueryTypeFactory.SequenceType.Create(XmlQueryTypeFactory.ChoiceType.Create(list), xmlQueryCardinality);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004FD3 File Offset: 0x00003FD3
		public static XmlQueryType NodeChoice(XmlNodeKindFlags kinds)
		{
			return XmlQueryTypeFactory.ChoiceType.Create(kinds);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004FDB File Offset: 0x00003FDB
		public static XmlQueryType Sequence(XmlQueryType left, XmlQueryType right)
		{
			return XmlQueryTypeFactory.SequenceType.Create(XmlQueryTypeFactory.ChoiceType.Create(XmlQueryTypeFactory.PrimeChoice(new List<XmlQueryType>(left), right)), left.Cardinality + right.Cardinality);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005004 File Offset: 0x00004004
		public static XmlQueryType PrimeProduct(XmlQueryType t, XmlQueryCardinality c)
		{
			if (t.Cardinality == c && !t.IsDod)
			{
				return t;
			}
			return XmlQueryTypeFactory.SequenceType.Create(t.Prime, c);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000502A File Offset: 0x0000402A
		public static XmlQueryType Product(XmlQueryType t, XmlQueryCardinality c)
		{
			return XmlQueryTypeFactory.PrimeProduct(t, t.Cardinality * c);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000503E File Offset: 0x0000403E
		public static XmlQueryType AtMost(XmlQueryType t, XmlQueryCardinality c)
		{
			return XmlQueryTypeFactory.PrimeProduct(t, c.AtMost());
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005050 File Offset: 0x00004050
		private static List<XmlQueryType> PrimeChoice(List<XmlQueryType> accumulator, IList<XmlQueryType> types)
		{
			foreach (XmlQueryType xmlQueryType in types)
			{
				XmlQueryTypeFactory.AddItemToChoice(accumulator, xmlQueryType);
			}
			return accumulator;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000509C File Offset: 0x0000409C
		private static void AddItemToChoice(List<XmlQueryType> accumulator, XmlQueryType itemType)
		{
			bool flag = true;
			for (int i = 0; i < accumulator.Count; i++)
			{
				if (itemType.IsSubtypeOf(accumulator[i]))
				{
					return;
				}
				if (accumulator[i].IsSubtypeOf(itemType))
				{
					if (flag)
					{
						flag = false;
						accumulator[i] = itemType;
					}
					else
					{
						accumulator.RemoveAt(i);
						i--;
					}
				}
			}
			if (flag)
			{
				accumulator.Add(itemType);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000050FF File Offset: 0x000040FF
		public static XmlQueryType Type(XPathNodeType kind, XmlQualifiedNameTest nameTest, XmlSchemaType contentType, bool isNillable)
		{
			return XmlQueryTypeFactory.ItemType.Create(XmlQueryTypeFactory.NodeKindToTypeCode[(int)kind], nameTest, contentType, isNillable);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005110 File Offset: 0x00004110
		[Conditional("DEBUG")]
		public static void CheckSerializability(XmlQueryType type)
		{
			type.GetObjectData(new BinaryWriter(Stream.Null));
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005124 File Offset: 0x00004124
		public static void Serialize(BinaryWriter writer, XmlQueryType type)
		{
			sbyte b;
			if (type.GetType() == typeof(XmlQueryTypeFactory.ItemType))
			{
				b = 0;
			}
			else if (type.GetType() == typeof(XmlQueryTypeFactory.ChoiceType))
			{
				b = 1;
			}
			else if (type.GetType() == typeof(XmlQueryTypeFactory.SequenceType))
			{
				b = 2;
			}
			else
			{
				b = -1;
			}
			writer.Write(b);
			type.GetObjectData(writer);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005184 File Offset: 0x00004184
		public static XmlQueryType Deserialize(BinaryReader reader)
		{
			switch (reader.ReadByte())
			{
			case 0:
				return XmlQueryTypeFactory.ItemType.Create(reader);
			case 1:
				return XmlQueryTypeFactory.ChoiceType.Create(reader);
			case 2:
				return XmlQueryTypeFactory.SequenceType.Create(reader);
			default:
				return null;
			}
		}

		// Token: 0x040000E3 RID: 227
		public static readonly XmlQueryType None = XmlQueryTypeFactory.ChoiceType.None;

		// Token: 0x040000E4 RID: 228
		public static readonly XmlQueryType Empty = XmlQueryTypeFactory.SequenceType.Zero;

		// Token: 0x040000E5 RID: 229
		public static readonly XmlQueryType Item = XmlQueryTypeFactory.Type(XmlTypeCode.Item, false);

		// Token: 0x040000E6 RID: 230
		public static readonly XmlQueryType ItemS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Item, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000E7 RID: 231
		public static readonly XmlQueryType Node = XmlQueryTypeFactory.Type(XmlTypeCode.Node, false);

		// Token: 0x040000E8 RID: 232
		public static readonly XmlQueryType NodeS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Node, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000E9 RID: 233
		public static readonly XmlQueryType Element = XmlQueryTypeFactory.Type(XmlTypeCode.Element, false);

		// Token: 0x040000EA RID: 234
		public static readonly XmlQueryType ElementS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Element, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000EB RID: 235
		public static readonly XmlQueryType Document = XmlQueryTypeFactory.Type(XmlTypeCode.Document, false);

		// Token: 0x040000EC RID: 236
		public static readonly XmlQueryType DocumentS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Document, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000ED RID: 237
		public static readonly XmlQueryType Attribute = XmlQueryTypeFactory.Type(XmlTypeCode.Attribute, false);

		// Token: 0x040000EE RID: 238
		public static readonly XmlQueryType AttributeQ = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Attribute, XmlQueryCardinality.ZeroOrOne);

		// Token: 0x040000EF RID: 239
		public static readonly XmlQueryType AttributeS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Attribute, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000F0 RID: 240
		public static readonly XmlQueryType Namespace = XmlQueryTypeFactory.Type(XmlTypeCode.Namespace, false);

		// Token: 0x040000F1 RID: 241
		public static readonly XmlQueryType NamespaceS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Namespace, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000F2 RID: 242
		public static readonly XmlQueryType Text = XmlQueryTypeFactory.Type(XmlTypeCode.Text, false);

		// Token: 0x040000F3 RID: 243
		public static readonly XmlQueryType TextS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Text, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000F4 RID: 244
		public static readonly XmlQueryType Comment = XmlQueryTypeFactory.Type(XmlTypeCode.Comment, false);

		// Token: 0x040000F5 RID: 245
		public static readonly XmlQueryType CommentS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Comment, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000F6 RID: 246
		public static readonly XmlQueryType PI = XmlQueryTypeFactory.Type(XmlTypeCode.ProcessingInstruction, false);

		// Token: 0x040000F7 RID: 247
		public static readonly XmlQueryType PIS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.PI, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000F8 RID: 248
		public static readonly XmlQueryType DocumentOrElement = XmlQueryTypeFactory.Choice(XmlQueryTypeFactory.Document, XmlQueryTypeFactory.Element);

		// Token: 0x040000F9 RID: 249
		public static readonly XmlQueryType DocumentOrElementQ = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.DocumentOrElement, XmlQueryCardinality.ZeroOrOne);

		// Token: 0x040000FA RID: 250
		public static readonly XmlQueryType DocumentOrElementS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.DocumentOrElement, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000FB RID: 251
		public static readonly XmlQueryType Content = XmlQueryTypeFactory.Choice(new XmlQueryType[]
		{
			XmlQueryTypeFactory.Element,
			XmlQueryTypeFactory.Comment,
			XmlQueryTypeFactory.PI,
			XmlQueryTypeFactory.Text
		});

		// Token: 0x040000FC RID: 252
		public static readonly XmlQueryType ContentS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.Content, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000FD RID: 253
		public static readonly XmlQueryType DocumentOrContent = XmlQueryTypeFactory.Choice(XmlQueryTypeFactory.Document, XmlQueryTypeFactory.Content);

		// Token: 0x040000FE RID: 254
		public static readonly XmlQueryType DocumentOrContentS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.DocumentOrContent, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x040000FF RID: 255
		public static readonly XmlQueryType AttributeOrContent = XmlQueryTypeFactory.Choice(XmlQueryTypeFactory.Attribute, XmlQueryTypeFactory.Content);

		// Token: 0x04000100 RID: 256
		public static readonly XmlQueryType AttributeOrContentS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.AttributeOrContent, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x04000101 RID: 257
		public static readonly XmlQueryType AnyAtomicType = XmlQueryTypeFactory.Type(XmlTypeCode.AnyAtomicType, false);

		// Token: 0x04000102 RID: 258
		public static readonly XmlQueryType AnyAtomicTypeS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.AnyAtomicType, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x04000103 RID: 259
		public static readonly XmlQueryType String = XmlQueryTypeFactory.Type(XmlTypeCode.String, false);

		// Token: 0x04000104 RID: 260
		public static readonly XmlQueryType StringX = XmlQueryTypeFactory.Type(XmlTypeCode.String, true);

		// Token: 0x04000105 RID: 261
		public static readonly XmlQueryType StringXS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.StringX, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x04000106 RID: 262
		public static readonly XmlQueryType Boolean = XmlQueryTypeFactory.Type(XmlTypeCode.Boolean, false);

		// Token: 0x04000107 RID: 263
		public static readonly XmlQueryType BooleanX = XmlQueryTypeFactory.Type(XmlTypeCode.Boolean, true);

		// Token: 0x04000108 RID: 264
		public static readonly XmlQueryType Int = XmlQueryTypeFactory.Type(XmlTypeCode.Int, false);

		// Token: 0x04000109 RID: 265
		public static readonly XmlQueryType IntX = XmlQueryTypeFactory.Type(XmlTypeCode.Int, true);

		// Token: 0x0400010A RID: 266
		public static readonly XmlQueryType IntXS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.IntX, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x0400010B RID: 267
		public static readonly XmlQueryType IntegerX = XmlQueryTypeFactory.Type(XmlTypeCode.Integer, true);

		// Token: 0x0400010C RID: 268
		public static readonly XmlQueryType LongX = XmlQueryTypeFactory.Type(XmlTypeCode.Long, true);

		// Token: 0x0400010D RID: 269
		public static readonly XmlQueryType DecimalX = XmlQueryTypeFactory.Type(XmlTypeCode.Decimal, true);

		// Token: 0x0400010E RID: 270
		public static readonly XmlQueryType FloatX = XmlQueryTypeFactory.Type(XmlTypeCode.Float, true);

		// Token: 0x0400010F RID: 271
		public static readonly XmlQueryType Double = XmlQueryTypeFactory.Type(XmlTypeCode.Double, false);

		// Token: 0x04000110 RID: 272
		public static readonly XmlQueryType DoubleX = XmlQueryTypeFactory.Type(XmlTypeCode.Double, true);

		// Token: 0x04000111 RID: 273
		public static readonly XmlQueryType DateTimeX = XmlQueryTypeFactory.Type(XmlTypeCode.DateTime, true);

		// Token: 0x04000112 RID: 274
		public static readonly XmlQueryType QNameX = XmlQueryTypeFactory.Type(XmlTypeCode.QName, true);

		// Token: 0x04000113 RID: 275
		public static readonly XmlQueryType UntypedDocument = XmlQueryTypeFactory.ItemType.UntypedDocument;

		// Token: 0x04000114 RID: 276
		public static readonly XmlQueryType UntypedElement = XmlQueryTypeFactory.ItemType.UntypedElement;

		// Token: 0x04000115 RID: 277
		public static readonly XmlQueryType UntypedAttribute = XmlQueryTypeFactory.ItemType.UntypedAttribute;

		// Token: 0x04000116 RID: 278
		public static readonly XmlQueryType UntypedNode = XmlQueryTypeFactory.Choice(new XmlQueryType[]
		{
			XmlQueryTypeFactory.UntypedDocument,
			XmlQueryTypeFactory.UntypedElement,
			XmlQueryTypeFactory.UntypedAttribute,
			XmlQueryTypeFactory.Namespace,
			XmlQueryTypeFactory.Text,
			XmlQueryTypeFactory.Comment,
			XmlQueryTypeFactory.PI
		});

		// Token: 0x04000117 RID: 279
		public static readonly XmlQueryType UntypedNodeS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.UntypedNode, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x04000118 RID: 280
		public static readonly XmlQueryType NodeNotRtf = XmlQueryTypeFactory.ItemType.NodeNotRtf;

		// Token: 0x04000119 RID: 281
		public static readonly XmlQueryType NodeNotRtfQ = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtf, XmlQueryCardinality.ZeroOrOne);

		// Token: 0x0400011A RID: 282
		public static readonly XmlQueryType NodeNotRtfS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtf, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x0400011B RID: 283
		public static readonly XmlQueryType NodeDodS = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtf, XmlQueryCardinality.ZeroOrMore);

		// Token: 0x0400011C RID: 284
		private static readonly XmlTypeCode[] NodeKindToTypeCode = new XmlTypeCode[]
		{
			XmlTypeCode.Document,
			XmlTypeCode.Element,
			XmlTypeCode.Attribute,
			XmlTypeCode.Namespace,
			XmlTypeCode.Text,
			XmlTypeCode.Text,
			XmlTypeCode.Text,
			XmlTypeCode.ProcessingInstruction,
			XmlTypeCode.Comment,
			XmlTypeCode.Node
		};

		// Token: 0x02000015 RID: 21
		private sealed class ItemType : XmlQueryType
		{
			// Token: 0x060000AB RID: 171 RVA: 0x000055E8 File Offset: 0x000045E8
			static ItemType()
			{
				int num = 55;
				XmlQueryTypeFactory.ItemType.BuiltInItemTypes = new XmlQueryType[num];
				XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict = new XmlQueryType[num];
				for (int i = 0; i < num; i++)
				{
					XmlTypeCode xmlTypeCode = (XmlTypeCode)i;
					switch (i)
					{
					case 0:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = XmlQueryTypeFactory.ChoiceType.None;
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ChoiceType.None;
						break;
					case 1:
					case 2:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, false, false, false);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i];
						break;
					case 3:
					case 4:
					case 6:
					case 7:
					case 8:
					case 9:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, false, false, true);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i];
						break;
					case 5:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.AnySimpleType, false, false, true);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i];
						break;
					case 10:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.AnyAtomicType, false, false, true);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i];
						break;
					case 11:
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.UntypedAtomicType, false, true, true);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i];
						break;
					default:
					{
						XmlSchemaType builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(xmlTypeCode);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypes[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, builtInSimpleType, false, false, true);
						XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[i] = new XmlQueryTypeFactory.ItemType(xmlTypeCode, XmlQualifiedNameTest.Wildcard, builtInSimpleType, false, true, true);
						break;
					}
					}
				}
				XmlQueryTypeFactory.ItemType.UntypedDocument = new XmlQueryTypeFactory.ItemType(XmlTypeCode.Document, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.UntypedAnyType, false, false, true);
				XmlQueryTypeFactory.ItemType.UntypedElement = new XmlQueryTypeFactory.ItemType(XmlTypeCode.Element, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.UntypedAnyType, false, false, true);
				XmlQueryTypeFactory.ItemType.UntypedAttribute = new XmlQueryTypeFactory.ItemType(XmlTypeCode.Attribute, XmlQualifiedNameTest.Wildcard, DatatypeImplementation.UntypedAtomicType, false, false, true);
				XmlQueryTypeFactory.ItemType.NodeNotRtf = new XmlQueryTypeFactory.ItemType(XmlTypeCode.Node, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, false, false, true);
				XmlQueryTypeFactory.ItemType.NodeDod = new XmlQueryTypeFactory.ItemType(XmlTypeCode.Node, XmlQualifiedNameTest.Wildcard, XmlSchemaComplexType.AnyType, false, false, true);
				XmlQueryTypeFactory.ItemType.SpecialBuiltInItemTypes = new XmlQueryType[]
				{
					XmlQueryTypeFactory.ItemType.UntypedDocument,
					XmlQueryTypeFactory.ItemType.UntypedElement,
					XmlQueryTypeFactory.ItemType.UntypedAttribute,
					XmlQueryTypeFactory.ItemType.NodeNotRtf
				};
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00005837 File Offset: 0x00004837
			public static XmlQueryType Create(XmlTypeCode code, bool isStrict)
			{
				if (isStrict)
				{
					return XmlQueryTypeFactory.ItemType.BuiltInItemTypesStrict[(int)code];
				}
				return XmlQueryTypeFactory.ItemType.BuiltInItemTypes[(int)code];
			}

			// Token: 0x060000AD RID: 173 RVA: 0x0000584C File Offset: 0x0000484C
			public static XmlQueryType Create(XmlSchemaSimpleType schemaType, bool isStrict)
			{
				XmlTypeCode typeCode = schemaType.Datatype.TypeCode;
				if (schemaType == XmlSchemaType.GetBuiltInSimpleType(typeCode))
				{
					return XmlQueryTypeFactory.ItemType.Create(typeCode, isStrict);
				}
				return new XmlQueryTypeFactory.ItemType(typeCode, XmlQualifiedNameTest.Wildcard, schemaType, false, isStrict, true);
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00005888 File Offset: 0x00004888
			public static XmlQueryType Create(XmlTypeCode code, XmlQualifiedNameTest nameTest, XmlSchemaType contentType, bool isNillable)
			{
				switch (code)
				{
				case XmlTypeCode.Document:
				case XmlTypeCode.Element:
					if (nameTest.IsWildcard)
					{
						if (contentType == XmlSchemaComplexType.AnyType)
						{
							return XmlQueryTypeFactory.ItemType.Create(code, false);
						}
						if (contentType == XmlSchemaComplexType.UntypedAnyType)
						{
							if (code == XmlTypeCode.Element)
							{
								return XmlQueryTypeFactory.ItemType.UntypedElement;
							}
							if (code == XmlTypeCode.Document)
							{
								return XmlQueryTypeFactory.ItemType.UntypedDocument;
							}
						}
					}
					return new XmlQueryTypeFactory.ItemType(code, nameTest, contentType, isNillable, false, true);
				case XmlTypeCode.Attribute:
					if (nameTest.IsWildcard)
					{
						if (contentType == DatatypeImplementation.AnySimpleType)
						{
							return XmlQueryTypeFactory.ItemType.Create(code, false);
						}
						if (contentType == DatatypeImplementation.UntypedAtomicType)
						{
							return XmlQueryTypeFactory.ItemType.UntypedAttribute;
						}
					}
					return new XmlQueryTypeFactory.ItemType(code, nameTest, contentType, isNillable, false, true);
				default:
					return XmlQueryTypeFactory.ItemType.Create(code, false);
				}
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00005928 File Offset: 0x00004928
			private ItemType(XmlTypeCode code, XmlQualifiedNameTest nameTest, XmlSchemaType schemaType, bool isNillable, bool isStrict, bool isNotRtf)
			{
				this.code = code;
				this.nameTest = nameTest;
				this.schemaType = schemaType;
				this.isNillable = isNillable;
				this.isStrict = isStrict;
				this.isNotRtf = isNotRtf;
				switch (code)
				{
				case XmlTypeCode.Item:
					this.nodeKinds = XmlNodeKindFlags.Any;
					return;
				case XmlTypeCode.Node:
					this.nodeKinds = XmlNodeKindFlags.Any;
					return;
				case XmlTypeCode.Document:
					this.nodeKinds = XmlNodeKindFlags.Document;
					return;
				case XmlTypeCode.Element:
					this.nodeKinds = XmlNodeKindFlags.Element;
					return;
				case XmlTypeCode.Attribute:
					this.nodeKinds = XmlNodeKindFlags.Attribute;
					return;
				case XmlTypeCode.Namespace:
					this.nodeKinds = XmlNodeKindFlags.Namespace;
					return;
				case XmlTypeCode.ProcessingInstruction:
					this.nodeKinds = XmlNodeKindFlags.PI;
					return;
				case XmlTypeCode.Comment:
					this.nodeKinds = XmlNodeKindFlags.Comment;
					return;
				case XmlTypeCode.Text:
					this.nodeKinds = XmlNodeKindFlags.Text;
					return;
				default:
					this.nodeKinds = XmlNodeKindFlags.None;
					return;
				}
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x000059EC File Offset: 0x000049EC
			public override void GetObjectData(BinaryWriter writer)
			{
				sbyte b = (sbyte)this.code;
				for (int i = 0; i < XmlQueryTypeFactory.ItemType.SpecialBuiltInItemTypes.Length; i++)
				{
					if (this == XmlQueryTypeFactory.ItemType.SpecialBuiltInItemTypes[i])
					{
						b = (sbyte)(~(sbyte)i);
						break;
					}
				}
				writer.Write(b);
				if (0 <= b)
				{
					writer.Write(this.isStrict);
				}
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00005A3C File Offset: 0x00004A3C
			public static XmlQueryType Create(BinaryReader reader)
			{
				sbyte b = reader.ReadSByte();
				if (0 <= b)
				{
					return XmlQueryTypeFactory.ItemType.Create((XmlTypeCode)b, reader.ReadBoolean());
				}
				return XmlQueryTypeFactory.ItemType.SpecialBuiltInItemTypes[(int)(~(int)b)];
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005A69 File Offset: 0x00004A69
			public override XmlTypeCode TypeCode
			{
				get
				{
					return this.code;
				}
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005A71 File Offset: 0x00004A71
			public override XmlQualifiedNameTest NameTest
			{
				get
				{
					return this.nameTest;
				}
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005A79 File Offset: 0x00004A79
			public override XmlSchemaType SchemaType
			{
				get
				{
					return this.schemaType;
				}
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x00005A81 File Offset: 0x00004A81
			public override bool IsNillable
			{
				get
				{
					return this.isNillable;
				}
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005A89 File Offset: 0x00004A89
			public override XmlNodeKindFlags NodeKinds
			{
				get
				{
					return this.nodeKinds;
				}
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060000B7 RID: 183 RVA: 0x00005A91 File Offset: 0x00004A91
			public override bool IsStrict
			{
				get
				{
					return this.isStrict;
				}
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060000B8 RID: 184 RVA: 0x00005A99 File Offset: 0x00004A99
			public override bool IsNotRtf
			{
				get
				{
					return this.isNotRtf;
				}
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005AA1 File Offset: 0x00004AA1
			public override bool IsDod
			{
				get
				{
					return this == XmlQueryTypeFactory.ItemType.NodeDod;
				}
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060000BA RID: 186 RVA: 0x00005AAB File Offset: 0x00004AAB
			public override XmlQueryCardinality Cardinality
			{
				get
				{
					return XmlQueryCardinality.One;
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060000BB RID: 187 RVA: 0x00005AB2 File Offset: 0x00004AB2
			public override XmlQueryType Prime
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060000BC RID: 188 RVA: 0x00005AB5 File Offset: 0x00004AB5
			public override XmlValueConverter ClrMapping
			{
				get
				{
					if (base.IsAtomicValue)
					{
						return this.SchemaType.ValueConverter;
					}
					if (base.IsNode)
					{
						return XmlNodeConverter.Node;
					}
					return XmlAnyConverter.Item;
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060000BD RID: 189 RVA: 0x00005ADE File Offset: 0x00004ADE
			public override int Count
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x17000044 RID: 68
			public override XmlQueryType this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this;
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x0400011D RID: 285
			public static readonly XmlQueryType UntypedDocument;

			// Token: 0x0400011E RID: 286
			public static readonly XmlQueryType UntypedElement;

			// Token: 0x0400011F RID: 287
			public static readonly XmlQueryType UntypedAttribute;

			// Token: 0x04000120 RID: 288
			public static readonly XmlQueryType NodeNotRtf;

			// Token: 0x04000121 RID: 289
			public static readonly XmlQueryType NodeDod;

			// Token: 0x04000122 RID: 290
			private static XmlQueryType[] BuiltInItemTypes;

			// Token: 0x04000123 RID: 291
			private static XmlQueryType[] BuiltInItemTypesStrict;

			// Token: 0x04000124 RID: 292
			private static XmlQueryType[] SpecialBuiltInItemTypes;

			// Token: 0x04000125 RID: 293
			private XmlTypeCode code;

			// Token: 0x04000126 RID: 294
			private XmlQualifiedNameTest nameTest;

			// Token: 0x04000127 RID: 295
			private XmlSchemaType schemaType;

			// Token: 0x04000128 RID: 296
			private bool isNillable;

			// Token: 0x04000129 RID: 297
			private XmlNodeKindFlags nodeKinds;

			// Token: 0x0400012A RID: 298
			private bool isStrict;

			// Token: 0x0400012B RID: 299
			private bool isNotRtf;
		}

		// Token: 0x02000016 RID: 22
		private sealed class ChoiceType : XmlQueryType
		{
			// Token: 0x060000C0 RID: 192 RVA: 0x00005AF4 File Offset: 0x00004AF4
			public static XmlQueryType Create(XmlNodeKindFlags nodeKinds)
			{
				if (Bits.ExactlyOne((uint)nodeKinds))
				{
					return XmlQueryTypeFactory.ItemType.Create(XmlQueryTypeFactory.ChoiceType.NodeKindToTypeCode[Bits.LeastPosition((uint)nodeKinds)], false);
				}
				List<XmlQueryType> list = new List<XmlQueryType>();
				while (nodeKinds != XmlNodeKindFlags.None)
				{
					list.Add(XmlQueryTypeFactory.ItemType.Create(XmlQueryTypeFactory.ChoiceType.NodeKindToTypeCode[Bits.LeastPosition((uint)nodeKinds)], false));
					nodeKinds = (XmlNodeKindFlags)Bits.ClearLeast((uint)nodeKinds);
				}
				return XmlQueryTypeFactory.ChoiceType.Create(list);
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00005B4D File Offset: 0x00004B4D
			public static XmlQueryType Create(List<XmlQueryType> members)
			{
				if (members.Count == 0)
				{
					return XmlQueryTypeFactory.ChoiceType.None;
				}
				if (members.Count == 1)
				{
					return members[0];
				}
				return new XmlQueryTypeFactory.ChoiceType(members);
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00005B74 File Offset: 0x00004B74
			private ChoiceType(List<XmlQueryType> members)
			{
				this.members = members;
				for (int i = 0; i < members.Count; i++)
				{
					XmlQueryType xmlQueryType = members[i];
					if (this.code == XmlTypeCode.None)
					{
						this.code = xmlQueryType.TypeCode;
						this.schemaType = xmlQueryType.SchemaType;
					}
					else if (base.IsNode && xmlQueryType.IsNode)
					{
						if (this.code == xmlQueryType.TypeCode)
						{
							if (this.code == XmlTypeCode.Element)
							{
								this.schemaType = XmlSchemaComplexType.AnyType;
							}
							else if (this.code == XmlTypeCode.Attribute)
							{
								this.schemaType = DatatypeImplementation.AnySimpleType;
							}
						}
						else
						{
							this.code = XmlTypeCode.Node;
							this.schemaType = null;
						}
					}
					else if (base.IsAtomicValue && xmlQueryType.IsAtomicValue)
					{
						this.code = XmlTypeCode.AnyAtomicType;
						this.schemaType = DatatypeImplementation.AnyAtomicType;
					}
					else
					{
						this.code = XmlTypeCode.Item;
						this.schemaType = null;
					}
					this.nodeKinds |= xmlQueryType.NodeKinds;
				}
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00005C74 File Offset: 0x00004C74
			public override void GetObjectData(BinaryWriter writer)
			{
				writer.Write(this.members.Count);
				for (int i = 0; i < this.members.Count; i++)
				{
					XmlQueryTypeFactory.Serialize(writer, this.members[i]);
				}
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00005CBC File Offset: 0x00004CBC
			public static XmlQueryType Create(BinaryReader reader)
			{
				int num = reader.ReadInt32();
				List<XmlQueryType> list = new List<XmlQueryType>(num);
				for (int i = 0; i < num; i++)
				{
					list.Add(XmlQueryTypeFactory.Deserialize(reader));
				}
				return XmlQueryTypeFactory.ChoiceType.Create(list);
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005CF5 File Offset: 0x00004CF5
			public override XmlTypeCode TypeCode
			{
				get
				{
					return this.code;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x060000C6 RID: 198 RVA: 0x00005CFD File Offset: 0x00004CFD
			public override XmlQualifiedNameTest NameTest
			{
				get
				{
					return XmlQualifiedNameTest.Wildcard;
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005D04 File Offset: 0x00004D04
			public override XmlSchemaType SchemaType
			{
				get
				{
					return this.schemaType;
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005D0C File Offset: 0x00004D0C
			public override bool IsNillable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005D0F File Offset: 0x00004D0F
			public override XmlNodeKindFlags NodeKinds
			{
				get
				{
					return this.nodeKinds;
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060000CA RID: 202 RVA: 0x00005D17 File Offset: 0x00004D17
			public override bool IsStrict
			{
				get
				{
					return this.members.Count == 0;
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060000CB RID: 203 RVA: 0x00005D28 File Offset: 0x00004D28
			public override bool IsNotRtf
			{
				get
				{
					for (int i = 0; i < this.members.Count; i++)
					{
						if (!this.members[i].IsNotRtf)
						{
							return false;
						}
					}
					return true;
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060000CC RID: 204 RVA: 0x00005D64 File Offset: 0x00004D64
			public override bool IsDod
			{
				get
				{
					for (int i = 0; i < this.members.Count; i++)
					{
						if (!this.members[i].IsDod)
						{
							return false;
						}
					}
					return true;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060000CD RID: 205 RVA: 0x00005D9D File Offset: 0x00004D9D
			public override XmlQueryCardinality Cardinality
			{
				get
				{
					if (this.TypeCode != XmlTypeCode.None)
					{
						return XmlQueryCardinality.One;
					}
					return XmlQueryCardinality.None;
				}
			}

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060000CE RID: 206 RVA: 0x00005DB2 File Offset: 0x00004DB2
			public override XmlQueryType Prime
			{
				get
				{
					return this;
				}
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060000CF RID: 207 RVA: 0x00005DB5 File Offset: 0x00004DB5
			public override XmlValueConverter ClrMapping
			{
				get
				{
					if (this.code == XmlTypeCode.None || this.code == XmlTypeCode.Item)
					{
						return XmlAnyConverter.Item;
					}
					if (base.IsAtomicValue)
					{
						return this.SchemaType.ValueConverter;
					}
					return XmlNodeConverter.Node;
				}
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005DE7 File Offset: 0x00004DE7
			public override int Count
			{
				get
				{
					return this.members.Count;
				}
			}

			// Token: 0x17000051 RID: 81
			public override XmlQueryType this[int index]
			{
				get
				{
					return this.members[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x0400012C RID: 300
			public static readonly XmlQueryType None = new XmlQueryTypeFactory.ChoiceType(new List<XmlQueryType>());

			// Token: 0x0400012D RID: 301
			private XmlTypeCode code;

			// Token: 0x0400012E RID: 302
			private XmlSchemaType schemaType;

			// Token: 0x0400012F RID: 303
			private XmlNodeKindFlags nodeKinds;

			// Token: 0x04000130 RID: 304
			private List<XmlQueryType> members;

			// Token: 0x04000131 RID: 305
			private static readonly XmlTypeCode[] NodeKindToTypeCode = new XmlTypeCode[]
			{
				XmlTypeCode.None,
				XmlTypeCode.Document,
				XmlTypeCode.Element,
				XmlTypeCode.Attribute,
				XmlTypeCode.Text,
				XmlTypeCode.Comment,
				XmlTypeCode.ProcessingInstruction,
				XmlTypeCode.Namespace
			};
		}

		// Token: 0x02000017 RID: 23
		private sealed class SequenceType : XmlQueryType
		{
			// Token: 0x060000D4 RID: 212 RVA: 0x00005E54 File Offset: 0x00004E54
			public static XmlQueryType Create(XmlQueryType prime, XmlQueryCardinality card)
			{
				if (prime.TypeCode == XmlTypeCode.None)
				{
					if (!(XmlQueryCardinality.Zero <= card))
					{
						return XmlQueryTypeFactory.None;
					}
					return XmlQueryTypeFactory.SequenceType.Zero;
				}
				else
				{
					if (card == XmlQueryCardinality.None)
					{
						return XmlQueryTypeFactory.None;
					}
					if (card == XmlQueryCardinality.Zero)
					{
						return XmlQueryTypeFactory.SequenceType.Zero;
					}
					if (card == XmlQueryCardinality.One)
					{
						return prime;
					}
					return new XmlQueryTypeFactory.SequenceType(prime, card);
				}
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00005EBE File Offset: 0x00004EBE
			private SequenceType(XmlQueryType prime, XmlQueryCardinality card)
			{
				this.prime = prime;
				this.card = card;
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x00005ED4 File Offset: 0x00004ED4
			public override void GetObjectData(BinaryWriter writer)
			{
				writer.Write(this.IsDod);
				if (this.IsDod)
				{
					return;
				}
				XmlQueryTypeFactory.Serialize(writer, this.prime);
				this.card.GetObjectData(writer);
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00005F04 File Offset: 0x00004F04
			public static XmlQueryType Create(BinaryReader reader)
			{
				if (reader.ReadBoolean())
				{
					return XmlQueryTypeFactory.NodeDodS;
				}
				XmlQueryType xmlQueryType = XmlQueryTypeFactory.Deserialize(reader);
				XmlQueryCardinality xmlQueryCardinality = new XmlQueryCardinality(reader);
				return XmlQueryTypeFactory.SequenceType.Create(xmlQueryType, xmlQueryCardinality);
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005F35 File Offset: 0x00004F35
			public override XmlTypeCode TypeCode
			{
				get
				{
					return this.prime.TypeCode;
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005F42 File Offset: 0x00004F42
			public override XmlQualifiedNameTest NameTest
			{
				get
				{
					return this.prime.NameTest;
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060000DA RID: 218 RVA: 0x00005F4F File Offset: 0x00004F4F
			public override XmlSchemaType SchemaType
			{
				get
				{
					return this.prime.SchemaType;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060000DB RID: 219 RVA: 0x00005F5C File Offset: 0x00004F5C
			public override bool IsNillable
			{
				get
				{
					return this.prime.IsNillable;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060000DC RID: 220 RVA: 0x00005F69 File Offset: 0x00004F69
			public override XmlNodeKindFlags NodeKinds
			{
				get
				{
					return this.prime.NodeKinds;
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060000DD RID: 221 RVA: 0x00005F76 File Offset: 0x00004F76
			public override bool IsStrict
			{
				get
				{
					return this.prime.IsStrict;
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060000DE RID: 222 RVA: 0x00005F83 File Offset: 0x00004F83
			public override bool IsNotRtf
			{
				get
				{
					return this.prime.IsNotRtf;
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060000DF RID: 223 RVA: 0x00005F90 File Offset: 0x00004F90
			public override bool IsDod
			{
				get
				{
					return this == XmlQueryTypeFactory.NodeDodS;
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005F9A File Offset: 0x00004F9A
			public override XmlQueryCardinality Cardinality
			{
				get
				{
					return this.card;
				}
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005FA2 File Offset: 0x00004FA2
			public override XmlQueryType Prime
			{
				get
				{
					return this.prime;
				}
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005FAA File Offset: 0x00004FAA
			public override XmlValueConverter ClrMapping
			{
				get
				{
					if (this.converter == null)
					{
						this.converter = XmlListConverter.Create(this.prime.ClrMapping);
					}
					return this.converter;
				}
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005FD0 File Offset: 0x00004FD0
			public override int Count
			{
				get
				{
					return this.prime.Count;
				}
			}

			// Token: 0x1700005E RID: 94
			public override XmlQueryType this[int index]
			{
				get
				{
					return this.prime[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x04000132 RID: 306
			public static readonly XmlQueryType Zero = new XmlQueryTypeFactory.SequenceType(XmlQueryTypeFactory.ChoiceType.None, XmlQueryCardinality.Zero);

			// Token: 0x04000133 RID: 307
			private XmlQueryType prime;

			// Token: 0x04000134 RID: 308
			private XmlQueryCardinality card;

			// Token: 0x04000135 RID: 309
			private XmlValueConverter converter;
		}
	}
}

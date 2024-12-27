using System;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Xsl
{
	// Token: 0x02000011 RID: 17
	internal abstract class XmlQueryType : ListBase<XmlQueryType>
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00003EC8 File Offset: 0x00002EC8
		static XmlQueryType()
		{
			for (int i = 0; i < XmlQueryType.BaseTypeCodes.Length; i++)
			{
				int num = i;
				for (;;)
				{
					XmlQueryType.TypeCodeDerivation[i, num] = true;
					if (XmlQueryType.BaseTypeCodes[num] == (XmlTypeCode)num)
					{
						break;
					}
					num = (int)XmlQueryType.BaseTypeCodes[num];
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000075 RID: 117
		public abstract XmlTypeCode TypeCode { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000076 RID: 118
		public abstract XmlQualifiedNameTest NameTest { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000077 RID: 119
		public abstract XmlSchemaType SchemaType { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000078 RID: 120
		public abstract bool IsNillable { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000079 RID: 121
		public abstract XmlNodeKindFlags NodeKinds { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007A RID: 122
		public abstract bool IsStrict { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600007B RID: 123
		public abstract XmlQueryCardinality Cardinality { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007C RID: 124
		public abstract XmlQueryType Prime { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007D RID: 125
		public abstract bool IsNotRtf { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007E RID: 126
		public abstract bool IsDod { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007F RID: 127
		public abstract XmlValueConverter ClrMapping { get; }

		// Token: 0x06000080 RID: 128 RVA: 0x0000439C File Offset: 0x0000339C
		public bool IsSubtypeOf(XmlQueryType baseType)
		{
			if (!(this.Cardinality <= baseType.Cardinality) || (!this.IsDod && baseType.IsDod))
			{
				return false;
			}
			XmlQueryType prime = this.Prime;
			XmlQueryType prime2 = baseType.Prime;
			if (prime == prime2)
			{
				return true;
			}
			if (prime.Count == 1 && prime2.Count == 1)
			{
				return prime.IsSubtypeOfItemType(prime2);
			}
			foreach (XmlQueryType xmlQueryType in prime)
			{
				bool flag = false;
				foreach (XmlQueryType xmlQueryType2 in prime2)
				{
					if (xmlQueryType.IsSubtypeOfItemType(xmlQueryType2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000448C File Offset: 0x0000348C
		public bool NeverSubtypeOf(XmlQueryType baseType)
		{
			if (this.Cardinality.NeverSubset(baseType.Cardinality))
			{
				return true;
			}
			if (this.MaybeEmpty && baseType.MaybeEmpty)
			{
				return false;
			}
			if (this.Count == 0)
			{
				return false;
			}
			foreach (XmlQueryType xmlQueryType in this)
			{
				foreach (XmlQueryType xmlQueryType2 in baseType)
				{
					if (xmlQueryType.HasIntersectionItemType(xmlQueryType2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004550 File Offset: 0x00003550
		public bool Equals(XmlQueryType that)
		{
			if (that == null)
			{
				return false;
			}
			if (this.Cardinality != that.Cardinality || this.IsDod != that.IsDod)
			{
				return false;
			}
			XmlQueryType prime = this.Prime;
			XmlQueryType prime2 = that.Prime;
			if (prime == prime2)
			{
				return true;
			}
			if (prime.Count != prime2.Count)
			{
				return false;
			}
			if (prime.Count == 1)
			{
				return prime.TypeCode == prime2.TypeCode && prime.NameTest == prime2.NameTest && prime.SchemaType == prime2.SchemaType && prime.IsStrict == prime2.IsStrict && prime.IsNotRtf == prime2.IsNotRtf;
			}
			foreach (XmlQueryType xmlQueryType in this)
			{
				bool flag = false;
				foreach (XmlQueryType xmlQueryType2 in that)
				{
					if (xmlQueryType.TypeCode == xmlQueryType2.TypeCode && xmlQueryType.NameTest == xmlQueryType2.NameTest && xmlQueryType.SchemaType == xmlQueryType2.SchemaType && xmlQueryType.IsStrict == xmlQueryType2.IsStrict && xmlQueryType.IsNotRtf == xmlQueryType2.IsNotRtf)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000046E4 File Offset: 0x000036E4
		public static bool operator ==(XmlQueryType left, XmlQueryType right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000046F5 File Offset: 0x000036F5
		public static bool operator !=(XmlQueryType left, XmlQueryType right)
		{
			if (left == null)
			{
				return right != null;
			}
			return !left.Equals(right);
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000470C File Offset: 0x0000370C
		public bool IsEmpty
		{
			get
			{
				return this.Cardinality <= XmlQueryCardinality.Zero;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000471E File Offset: 0x0000371E
		public bool IsSingleton
		{
			get
			{
				return this.Cardinality <= XmlQueryCardinality.One;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004730 File Offset: 0x00003730
		public bool MaybeEmpty
		{
			get
			{
				return XmlQueryCardinality.Zero <= this.Cardinality;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004742 File Offset: 0x00003742
		public bool MaybeMany
		{
			get
			{
				return XmlQueryCardinality.More <= this.Cardinality;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004754 File Offset: 0x00003754
		public bool IsNode
		{
			get
			{
				return (XmlQueryType.TypeCodeToFlags[(int)this.TypeCode] & XmlQueryType.TypeFlags.IsNode) != XmlQueryType.TypeFlags.None;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000476A File Offset: 0x0000376A
		public bool IsAtomicValue
		{
			get
			{
				return (XmlQueryType.TypeCodeToFlags[(int)this.TypeCode] & XmlQueryType.TypeFlags.IsAtomicValue) != XmlQueryType.TypeFlags.None;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004780 File Offset: 0x00003780
		public bool IsNumeric
		{
			get
			{
				return (XmlQueryType.TypeCodeToFlags[(int)this.TypeCode] & XmlQueryType.TypeFlags.IsNumeric) != XmlQueryType.TypeFlags.None;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004798 File Offset: 0x00003798
		public override bool Equals(object obj)
		{
			XmlQueryType xmlQueryType = obj as XmlQueryType;
			return !(xmlQueryType == null) && this.Equals(xmlQueryType);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000047C0 File Offset: 0x000037C0
		public override int GetHashCode()
		{
			if (this.hashCode == 0)
			{
				int num = (int)this.TypeCode;
				XmlSchemaType schemaType = this.SchemaType;
				if (schemaType != null)
				{
					num += (num << 7) ^ schemaType.GetHashCode();
				}
				num += (num << 7) ^ (int)this.NodeKinds;
				num += (num << 7) ^ this.Cardinality.GetHashCode();
				num += (num << 7) ^ (this.IsStrict ? 1 : 0);
				num -= num >> 17;
				num -= num >> 11;
				num -= num >> 5;
				this.hashCode = ((num == 0) ? 1 : num);
			}
			return this.hashCode;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004855 File Offset: 0x00003855
		public override string ToString()
		{
			return this.ToString("G");
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004864 File Offset: 0x00003864
		public string ToString(string format)
		{
			if (format == "S")
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.Cardinality.ToString(format));
				stringBuilder.Append(';');
				for (int i = 0; i < this.Count; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append("|");
					}
					stringBuilder.Append(this[i].TypeCode.ToString());
				}
				stringBuilder.Append(';');
				stringBuilder.Append(this.IsStrict);
				return stringBuilder.ToString();
			}
			bool flag = format == "X";
			if (this.Cardinality == XmlQueryCardinality.None)
			{
				return "none";
			}
			if (this.Cardinality == XmlQueryCardinality.Zero)
			{
				return "empty";
			}
			switch (this.Count)
			{
			case 0:
				return "none" + this.Cardinality.ToString();
			case 1:
				return this[0].ItemTypeToString(flag) + this.Cardinality.ToString();
			default:
			{
				string[] array = new string[this.Count];
				for (int j = 0; j < this.Count; j++)
				{
					array[j] = this[j].ItemTypeToString(flag);
				}
				Array.Sort<string>(array);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				stringBuilder.Append(array[0]);
				for (int k = 1; k < array.Length; k++)
				{
					stringBuilder.Append(" | ");
					stringBuilder.Append(array[k]);
				}
				stringBuilder.Append(")");
				stringBuilder.Append(this.Cardinality.ToString());
				if (!flag && this.IsDod)
				{
					stringBuilder.Append('#');
				}
				return stringBuilder.ToString();
			}
			}
		}

		// Token: 0x06000090 RID: 144
		public abstract void GetObjectData(BinaryWriter writer);

		// Token: 0x06000091 RID: 145 RVA: 0x00004A60 File Offset: 0x00003A60
		private bool IsSubtypeOfItemType(XmlQueryType baseType)
		{
			XmlSchemaType schemaType = baseType.SchemaType;
			if (this.TypeCode != baseType.TypeCode)
			{
				if (baseType.IsStrict)
				{
					return false;
				}
				XmlSchemaType builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(baseType.TypeCode);
				return (builtInSimpleType == null || schemaType == builtInSimpleType) && XmlQueryType.TypeCodeDerivation[this.TypeCode, baseType.TypeCode];
			}
			else
			{
				if (baseType.IsStrict)
				{
					return this.IsStrict && this.SchemaType == schemaType;
				}
				return (this.IsNotRtf || !baseType.IsNotRtf) && (this.IsDod || !baseType.IsDod) && this.NameTest.IsSubsetOf(baseType.NameTest) && (schemaType == XmlSchemaComplexType.AnyType || XmlSchemaType.IsDerivedFrom(this.SchemaType, schemaType, XmlSchemaDerivationMethod.Empty)) && (!this.IsNillable || baseType.IsNillable);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004B30 File Offset: 0x00003B30
		private bool HasIntersectionItemType(XmlQueryType other)
		{
			if (this.TypeCode == other.TypeCode && (this.NodeKinds & (XmlNodeKindFlags.Document | XmlNodeKindFlags.Element | XmlNodeKindFlags.Attribute)) != XmlNodeKindFlags.None)
			{
				return this.TypeCode == XmlTypeCode.Node || (this.NameTest.HasIntersection(other.NameTest) && (XmlSchemaType.IsDerivedFrom(this.SchemaType, other.SchemaType, XmlSchemaDerivationMethod.Empty) || XmlSchemaType.IsDerivedFrom(other.SchemaType, this.SchemaType, XmlSchemaDerivationMethod.Empty)));
			}
			return this.IsSubtypeOf(other) || other.IsSubtypeOf(this);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004BB8 File Offset: 0x00003BB8
		private string ItemTypeToString(bool isXQ)
		{
			string text;
			if (this.IsNode)
			{
				text = XmlQueryType.TypeNames[(int)this.TypeCode];
				switch (this.TypeCode)
				{
				case XmlTypeCode.Document:
					if (isXQ)
					{
						text = text + "{(element" + this.NameAndType(true) + "?&text?&comment?&processing-instruction?)*}";
						goto IL_00BA;
					}
					break;
				case XmlTypeCode.Element:
				case XmlTypeCode.Attribute:
					break;
				default:
					goto IL_00BA;
				}
				text += this.NameAndType(isXQ);
			}
			else if (this.SchemaType != XmlSchemaComplexType.AnyType)
			{
				if (this.SchemaType.QualifiedName.IsEmpty)
				{
					text = "<:" + XmlQueryType.TypeNames[(int)this.TypeCode];
				}
				else
				{
					text = XmlQueryType.QNameToString(this.SchemaType.QualifiedName);
				}
			}
			else
			{
				text = XmlQueryType.TypeNames[(int)this.TypeCode];
			}
			IL_00BA:
			if (!isXQ && this.IsStrict)
			{
				text += "=";
			}
			return text;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004C98 File Offset: 0x00003C98
		private string NameAndType(bool isXQ)
		{
			string text = this.NameTest.ToString();
			string text2 = "*";
			if (this.SchemaType.QualifiedName.IsEmpty)
			{
				text2 = "typeof(" + text + ")";
			}
			else if (isXQ || (this.SchemaType != XmlSchemaComplexType.AnyType && this.SchemaType != DatatypeImplementation.AnySimpleType))
			{
				text2 = XmlQueryType.QNameToString(this.SchemaType.QualifiedName);
			}
			if (this.IsNillable)
			{
				text2 += " nillable";
			}
			if (text == "*" && text2 == "*")
			{
				return "";
			}
			return string.Concat(new string[] { "(", text, ", ", text2, ")" });
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004D6C File Offset: 0x00003D6C
		private static string QNameToString(XmlQualifiedName name)
		{
			if (name.IsEmpty)
			{
				return "*";
			}
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

		// Token: 0x040000D8 RID: 216
		private static readonly XmlQueryType.BitMatrix TypeCodeDerivation = new XmlQueryType.BitMatrix(XmlQueryType.BaseTypeCodes.Length);

		// Token: 0x040000D9 RID: 217
		private int hashCode;

		// Token: 0x040000DA RID: 218
		private static readonly XmlQueryType.TypeFlags[] TypeCodeToFlags = new XmlQueryType.TypeFlags[]
		{
			(XmlQueryType.TypeFlags)7,
			XmlQueryType.TypeFlags.None,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsNode,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			(XmlQueryType.TypeFlags)6,
			XmlQueryType.TypeFlags.IsAtomicValue,
			XmlQueryType.TypeFlags.IsAtomicValue
		};

		// Token: 0x040000DB RID: 219
		private static readonly XmlTypeCode[] BaseTypeCodes = new XmlTypeCode[]
		{
			XmlTypeCode.None,
			XmlTypeCode.Item,
			XmlTypeCode.Item,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Node,
			XmlTypeCode.Item,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.AnyAtomicType,
			XmlTypeCode.String,
			XmlTypeCode.NormalizedString,
			XmlTypeCode.Token,
			XmlTypeCode.Token,
			XmlTypeCode.Token,
			XmlTypeCode.Name,
			XmlTypeCode.NCName,
			XmlTypeCode.NCName,
			XmlTypeCode.NCName,
			XmlTypeCode.Decimal,
			XmlTypeCode.Integer,
			XmlTypeCode.NonPositiveInteger,
			XmlTypeCode.Integer,
			XmlTypeCode.Long,
			XmlTypeCode.Int,
			XmlTypeCode.Short,
			XmlTypeCode.Integer,
			XmlTypeCode.NonNegativeInteger,
			XmlTypeCode.UnsignedLong,
			XmlTypeCode.UnsignedInt,
			XmlTypeCode.UnsignedShort,
			XmlTypeCode.NonNegativeInteger,
			XmlTypeCode.Duration,
			XmlTypeCode.Duration
		};

		// Token: 0x040000DC RID: 220
		private static readonly string[] TypeNames = new string[]
		{
			"none", "item", "node", "document", "element", "attribute", "namespace", "processing-instruction", "comment", "text",
			"xdt:anyAtomicType", "xdt:untypedAtomic", "xs:string", "xs:boolean", "xs:decimal", "xs:float", "xs:double", "xs:duration", "xs:dateTime", "xs:time",
			"xs:date", "xs:gYearMonth", "xs:gYear", "xs:gMonthDay", "xs:gDay", "xs:gMonth", "xs:hexBinary", "xs:base64Binary", "xs:anyUri", "xs:QName",
			"xs:NOTATION", "xs:normalizedString", "xs:token", "xs:language", "xs:NMTOKEN", "xs:Name", "xs:NCName", "xs:ID", "xs:IDREF", "xs:ENTITY",
			"xs:integer", "xs:nonPositiveInteger", "xs:negativeInteger", "xs:long", "xs:int", "xs:short", "xs:byte", "xs:nonNegativeInteger", "xs:unsignedLong", "xs:unsignedInt",
			"xs:unsignedShort", "xs:unsignedByte", "xs:positiveInteger", "xdt:yearMonthDuration", "xdt:dayTimeDuration"
		};

		// Token: 0x02000012 RID: 18
		private enum TypeFlags
		{
			// Token: 0x040000DE RID: 222
			None,
			// Token: 0x040000DF RID: 223
			IsNode,
			// Token: 0x040000E0 RID: 224
			IsAtomicValue,
			// Token: 0x040000E1 RID: 225
			IsNumeric = 4
		}

		// Token: 0x02000013 RID: 19
		private sealed class BitMatrix
		{
			// Token: 0x06000097 RID: 151 RVA: 0x00004E04 File Offset: 0x00003E04
			public BitMatrix(int count)
			{
				this.bits = new ulong[count];
			}

			// Token: 0x17000036 RID: 54
			public bool this[int index1, int index2]
			{
				get
				{
					return (this.bits[index1] & (1UL << index2)) != 0UL;
				}
				set
				{
					if (value)
					{
						this.bits[index1] |= 1UL << index2;
						return;
					}
					this.bits[index1] &= ~(1UL << index2);
				}
			}

			// Token: 0x17000037 RID: 55
			public bool this[XmlTypeCode index1, XmlTypeCode index2]
			{
				get
				{
					return this[(int)index1, (int)index2];
				}
			}

			// Token: 0x040000E2 RID: 226
			private ulong[] bits;
		}
	}
}

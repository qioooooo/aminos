using System;
using System.IO;

namespace System.Xml.Xsl
{
	// Token: 0x02000010 RID: 16
	internal struct XmlQueryCardinality
	{
		// Token: 0x06000058 RID: 88 RVA: 0x000032FE File Offset: 0x000022FE
		private XmlQueryCardinality(int value)
		{
			this.value = value;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003307 File Offset: 0x00002307
		public static XmlQueryCardinality None
		{
			get
			{
				return new XmlQueryCardinality(0);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000330F File Offset: 0x0000230F
		public static XmlQueryCardinality Zero
		{
			get
			{
				return new XmlQueryCardinality(1);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003317 File Offset: 0x00002317
		public static XmlQueryCardinality One
		{
			get
			{
				return new XmlQueryCardinality(2);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000331F File Offset: 0x0000231F
		public static XmlQueryCardinality ZeroOrOne
		{
			get
			{
				return new XmlQueryCardinality(3);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003327 File Offset: 0x00002327
		public static XmlQueryCardinality More
		{
			get
			{
				return new XmlQueryCardinality(4);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000332F File Offset: 0x0000232F
		public static XmlQueryCardinality NotOne
		{
			get
			{
				return new XmlQueryCardinality(5);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003337 File Offset: 0x00002337
		public static XmlQueryCardinality OneOrMore
		{
			get
			{
				return new XmlQueryCardinality(6);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000333F File Offset: 0x0000233F
		public static XmlQueryCardinality ZeroOrMore
		{
			get
			{
				return new XmlQueryCardinality(7);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003347 File Offset: 0x00002347
		public bool Equals(XmlQueryCardinality other)
		{
			return this.value == other.value;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003358 File Offset: 0x00002358
		public static bool operator ==(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return left.value == right.value;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000336A File Offset: 0x0000236A
		public static bool operator !=(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return left.value != right.value;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000337F File Offset: 0x0000237F
		public override bool Equals(object other)
		{
			return other is XmlQueryCardinality && this.Equals((XmlQueryCardinality)other);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003397 File Offset: 0x00002397
		public override int GetHashCode()
		{
			return this.value;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000339F File Offset: 0x0000239F
		public static XmlQueryCardinality operator |(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return new XmlQueryCardinality(left.value | right.value);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000033B5 File Offset: 0x000023B5
		public static XmlQueryCardinality operator &(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return new XmlQueryCardinality(left.value & right.value);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033CB File Offset: 0x000023CB
		public static XmlQueryCardinality operator *(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return XmlQueryCardinality.cardinalityProduct[left.value, right.value];
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000033EA File Offset: 0x000023EA
		public static XmlQueryCardinality operator +(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return XmlQueryCardinality.cardinalitySum[left.value, right.value];
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003409 File Offset: 0x00002409
		public static bool operator <=(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return (left.value & ~right.value) == 0;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000341E File Offset: 0x0000241E
		public static bool operator >=(XmlQueryCardinality left, XmlQueryCardinality right)
		{
			return (right.value & ~left.value) == 0;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003433 File Offset: 0x00002433
		public XmlQueryCardinality AtMost()
		{
			return new XmlQueryCardinality(this.value | (this.value >> 1) | (this.value >> 2));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003452 File Offset: 0x00002452
		public bool NeverSubset(XmlQueryCardinality other)
		{
			return this.value != 0 && (this.value & other.value) == 0;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000346F File Offset: 0x0000246F
		public string ToString(string format)
		{
			if (format == "S")
			{
				return XmlQueryCardinality.serialized[this.value];
			}
			return this.ToString();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003497 File Offset: 0x00002497
		public override string ToString()
		{
			return XmlQueryCardinality.toString[this.value];
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034A8 File Offset: 0x000024A8
		public XmlQueryCardinality(string s)
		{
			this.value = 0;
			for (int i = 0; i < XmlQueryCardinality.serialized.Length; i++)
			{
				if (s == XmlQueryCardinality.serialized[i])
				{
					this.value = i;
					return;
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034E5 File Offset: 0x000024E5
		public void GetObjectData(BinaryWriter writer)
		{
			writer.Write((byte)this.value);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000034F4 File Offset: 0x000024F4
		public XmlQueryCardinality(BinaryReader reader)
		{
			this = new XmlQueryCardinality((int)reader.ReadByte());
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003504 File Offset: 0x00002504
		// Note: this type is marked as 'beforefieldinit'.
		static XmlQueryCardinality()
		{
			XmlQueryCardinality[,] array = new XmlQueryCardinality[8, 8];
			array[0, 0] = XmlQueryCardinality.None;
			array[0, 1] = XmlQueryCardinality.Zero;
			array[0, 2] = XmlQueryCardinality.None;
			array[0, 3] = XmlQueryCardinality.Zero;
			array[0, 4] = XmlQueryCardinality.None;
			array[0, 5] = XmlQueryCardinality.Zero;
			array[0, 6] = XmlQueryCardinality.None;
			array[0, 7] = XmlQueryCardinality.Zero;
			array[1, 0] = XmlQueryCardinality.Zero;
			array[1, 1] = XmlQueryCardinality.Zero;
			array[1, 2] = XmlQueryCardinality.Zero;
			array[1, 3] = XmlQueryCardinality.Zero;
			array[1, 4] = XmlQueryCardinality.Zero;
			array[1, 5] = XmlQueryCardinality.Zero;
			array[1, 6] = XmlQueryCardinality.Zero;
			array[1, 7] = XmlQueryCardinality.Zero;
			array[2, 0] = XmlQueryCardinality.None;
			array[2, 1] = XmlQueryCardinality.Zero;
			array[2, 2] = XmlQueryCardinality.One;
			array[2, 3] = XmlQueryCardinality.ZeroOrOne;
			array[2, 4] = XmlQueryCardinality.More;
			array[2, 5] = XmlQueryCardinality.NotOne;
			array[2, 6] = XmlQueryCardinality.OneOrMore;
			array[2, 7] = XmlQueryCardinality.ZeroOrMore;
			array[3, 0] = XmlQueryCardinality.Zero;
			array[3, 1] = XmlQueryCardinality.Zero;
			array[3, 2] = XmlQueryCardinality.ZeroOrOne;
			array[3, 3] = XmlQueryCardinality.ZeroOrOne;
			array[3, 4] = XmlQueryCardinality.NotOne;
			array[3, 5] = XmlQueryCardinality.NotOne;
			array[3, 6] = XmlQueryCardinality.ZeroOrMore;
			array[3, 7] = XmlQueryCardinality.ZeroOrMore;
			array[4, 0] = XmlQueryCardinality.None;
			array[4, 1] = XmlQueryCardinality.Zero;
			array[4, 2] = XmlQueryCardinality.More;
			array[4, 3] = XmlQueryCardinality.NotOne;
			array[4, 4] = XmlQueryCardinality.More;
			array[4, 5] = XmlQueryCardinality.NotOne;
			array[4, 6] = XmlQueryCardinality.More;
			array[4, 7] = XmlQueryCardinality.NotOne;
			array[5, 0] = XmlQueryCardinality.Zero;
			array[5, 1] = XmlQueryCardinality.Zero;
			array[5, 2] = XmlQueryCardinality.NotOne;
			array[5, 3] = XmlQueryCardinality.NotOne;
			array[5, 4] = XmlQueryCardinality.NotOne;
			array[5, 5] = XmlQueryCardinality.NotOne;
			array[5, 6] = XmlQueryCardinality.NotOne;
			array[5, 7] = XmlQueryCardinality.NotOne;
			array[6, 0] = XmlQueryCardinality.None;
			array[6, 1] = XmlQueryCardinality.Zero;
			array[6, 2] = XmlQueryCardinality.OneOrMore;
			array[6, 3] = XmlQueryCardinality.ZeroOrMore;
			array[6, 4] = XmlQueryCardinality.More;
			array[6, 5] = XmlQueryCardinality.NotOne;
			array[6, 6] = XmlQueryCardinality.OneOrMore;
			array[6, 7] = XmlQueryCardinality.ZeroOrMore;
			array[7, 0] = XmlQueryCardinality.Zero;
			array[7, 1] = XmlQueryCardinality.Zero;
			array[7, 2] = XmlQueryCardinality.ZeroOrMore;
			array[7, 3] = XmlQueryCardinality.ZeroOrMore;
			array[7, 4] = XmlQueryCardinality.NotOne;
			array[7, 5] = XmlQueryCardinality.NotOne;
			array[7, 6] = XmlQueryCardinality.ZeroOrMore;
			array[7, 7] = XmlQueryCardinality.ZeroOrMore;
			XmlQueryCardinality.cardinalityProduct = array;
			XmlQueryCardinality[,] array2 = new XmlQueryCardinality[8, 8];
			array2[0, 0] = XmlQueryCardinality.None;
			array2[0, 1] = XmlQueryCardinality.Zero;
			array2[0, 2] = XmlQueryCardinality.One;
			array2[0, 3] = XmlQueryCardinality.ZeroOrOne;
			array2[0, 4] = XmlQueryCardinality.More;
			array2[0, 5] = XmlQueryCardinality.NotOne;
			array2[0, 6] = XmlQueryCardinality.OneOrMore;
			array2[0, 7] = XmlQueryCardinality.ZeroOrMore;
			array2[1, 0] = XmlQueryCardinality.Zero;
			array2[1, 1] = XmlQueryCardinality.Zero;
			array2[1, 2] = XmlQueryCardinality.One;
			array2[1, 3] = XmlQueryCardinality.ZeroOrOne;
			array2[1, 4] = XmlQueryCardinality.More;
			array2[1, 5] = XmlQueryCardinality.NotOne;
			array2[1, 6] = XmlQueryCardinality.OneOrMore;
			array2[1, 7] = XmlQueryCardinality.ZeroOrMore;
			array2[2, 0] = XmlQueryCardinality.One;
			array2[2, 1] = XmlQueryCardinality.One;
			array2[2, 2] = XmlQueryCardinality.More;
			array2[2, 3] = XmlQueryCardinality.OneOrMore;
			array2[2, 4] = XmlQueryCardinality.More;
			array2[2, 5] = XmlQueryCardinality.OneOrMore;
			array2[2, 6] = XmlQueryCardinality.More;
			array2[2, 7] = XmlQueryCardinality.OneOrMore;
			array2[3, 0] = XmlQueryCardinality.ZeroOrOne;
			array2[3, 1] = XmlQueryCardinality.ZeroOrOne;
			array2[3, 2] = XmlQueryCardinality.OneOrMore;
			array2[3, 3] = XmlQueryCardinality.ZeroOrMore;
			array2[3, 4] = XmlQueryCardinality.More;
			array2[3, 5] = XmlQueryCardinality.ZeroOrMore;
			array2[3, 6] = XmlQueryCardinality.OneOrMore;
			array2[3, 7] = XmlQueryCardinality.ZeroOrMore;
			array2[4, 0] = XmlQueryCardinality.More;
			array2[4, 1] = XmlQueryCardinality.More;
			array2[4, 2] = XmlQueryCardinality.More;
			array2[4, 3] = XmlQueryCardinality.More;
			array2[4, 4] = XmlQueryCardinality.More;
			array2[4, 5] = XmlQueryCardinality.More;
			array2[4, 6] = XmlQueryCardinality.More;
			array2[4, 7] = XmlQueryCardinality.More;
			array2[5, 0] = XmlQueryCardinality.NotOne;
			array2[5, 1] = XmlQueryCardinality.NotOne;
			array2[5, 2] = XmlQueryCardinality.OneOrMore;
			array2[5, 3] = XmlQueryCardinality.ZeroOrMore;
			array2[5, 4] = XmlQueryCardinality.More;
			array2[5, 5] = XmlQueryCardinality.NotOne;
			array2[5, 6] = XmlQueryCardinality.OneOrMore;
			array2[5, 7] = XmlQueryCardinality.ZeroOrMore;
			array2[6, 0] = XmlQueryCardinality.OneOrMore;
			array2[6, 1] = XmlQueryCardinality.OneOrMore;
			array2[6, 2] = XmlQueryCardinality.More;
			array2[6, 3] = XmlQueryCardinality.OneOrMore;
			array2[6, 4] = XmlQueryCardinality.More;
			array2[6, 5] = XmlQueryCardinality.OneOrMore;
			array2[6, 6] = XmlQueryCardinality.More;
			array2[6, 7] = XmlQueryCardinality.OneOrMore;
			array2[7, 0] = XmlQueryCardinality.ZeroOrMore;
			array2[7, 1] = XmlQueryCardinality.ZeroOrMore;
			array2[7, 2] = XmlQueryCardinality.OneOrMore;
			array2[7, 3] = XmlQueryCardinality.ZeroOrMore;
			array2[7, 4] = XmlQueryCardinality.More;
			array2[7, 5] = XmlQueryCardinality.ZeroOrMore;
			array2[7, 6] = XmlQueryCardinality.OneOrMore;
			array2[7, 7] = XmlQueryCardinality.ZeroOrMore;
			XmlQueryCardinality.cardinalitySum = array2;
			XmlQueryCardinality.toString = new string[] { "", "?", "", "?", "+", "*", "+", "*" };
			XmlQueryCardinality.serialized = new string[] { "None", "Zero", "One", "ZeroOrOne", "More", "NotOne", "OneOrMore", "ZeroOrMore" };
		}

		// Token: 0x040000D3 RID: 211
		private int value;

		// Token: 0x040000D4 RID: 212
		private static readonly XmlQueryCardinality[,] cardinalityProduct;

		// Token: 0x040000D5 RID: 213
		private static readonly XmlQueryCardinality[,] cardinalitySum;

		// Token: 0x040000D6 RID: 214
		private static readonly string[] toString;

		// Token: 0x040000D7 RID: 215
		private static readonly string[] serialized;
	}
}

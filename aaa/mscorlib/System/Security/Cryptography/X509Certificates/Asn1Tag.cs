using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008AD RID: 2221
	internal struct Asn1Tag : IEquatable<Asn1Tag>
	{
		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x0600516E RID: 20846 RVA: 0x00124AF1 File Offset: 0x00123AF1
		public TagClass TagClass
		{
			get
			{
				return (TagClass)(this._controlFlags & 192);
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x00124AFF File Offset: 0x00123AFF
		public bool IsConstructed
		{
			get
			{
				return (this._controlFlags & 32) != 0;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x00124B10 File Offset: 0x00123B10
		public int TagValue
		{
			get
			{
				return this._tagValue;
			}
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x00124B18 File Offset: 0x00123B18
		private Asn1Tag(byte controlFlags, int tagValue)
		{
			this._controlFlags = controlFlags & 224;
			this._tagValue = tagValue;
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x00124B2F File Offset: 0x00123B2F
		public Asn1Tag(UniversalTagNumber universalTagNumber, bool isConstructed)
		{
			this = new Asn1Tag(isConstructed ? 32 : 0, (int)universalTagNumber);
			if (universalTagNumber < UniversalTagNumber.EndOfContents || universalTagNumber > UniversalTagNumber.RelativeObjectIdentifierIRI || universalTagNumber == (UniversalTagNumber)15)
			{
				throw new ArgumentOutOfRangeException("universalTagNumber");
			}
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x00124B5C File Offset: 0x00123B5C
		public Asn1Tag(TagClass tagClass, int tagValue, bool isConstructed)
		{
			this = new Asn1Tag((byte)tagClass | (isConstructed ? 32 : 0), tagValue);
			if (tagClass <= TagClass.Application)
			{
				if (tagClass == TagClass.Universal || tagClass == TagClass.Application)
				{
					goto IL_0040;
				}
			}
			else if (tagClass == TagClass.ContextSpecific || tagClass == TagClass.Private)
			{
				goto IL_0040;
			}
			throw new ArgumentOutOfRangeException("tagClass");
			IL_0040:
			if (tagValue < 0)
			{
				throw new ArgumentOutOfRangeException("tagValue");
			}
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x00124BB8 File Offset: 0x00123BB8
		public Asn1Tag(TagClass tagClass, int tagValue)
		{
			this = new Asn1Tag(tagClass, tagValue, false);
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x00124BC3 File Offset: 0x00123BC3
		public Asn1Tag AsConstructed()
		{
			return new Asn1Tag(this._controlFlags | 32, this.TagValue);
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x00124BDC File Offset: 0x00123BDC
		public static bool TryDecode(ReadOnlySpan<byte> source, out Asn1Tag tag, out int bytesConsumed)
		{
			tag = default(Asn1Tag);
			bytesConsumed = 0;
			if (source.IsEmpty)
			{
				return false;
			}
			byte b = source[bytesConsumed];
			bytesConsumed++;
			uint num = (uint)(b & 31);
			if (num == 31U)
			{
				num = 0U;
				while (source.Length > bytesConsumed)
				{
					byte b2 = source[bytesConsumed];
					byte b3 = b2 & 127;
					bytesConsumed++;
					if (num >= 33554432U)
					{
						bytesConsumed = 0;
						return false;
					}
					num <<= 7;
					num |= (uint)b3;
					if (num == 0U)
					{
						bytesConsumed = 0;
						return false;
					}
					if ((b2 & 128) != 128)
					{
						if (num <= 30U)
						{
							bytesConsumed = 0;
							return false;
						}
						if (num > 2147483647U)
						{
							bytesConsumed = 0;
							return false;
						}
						goto IL_0099;
					}
				}
				bytesConsumed = 0;
				return false;
			}
			IL_0099:
			tag = new Asn1Tag(b, (int)num);
			return true;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x00124C90 File Offset: 0x00123C90
		public static Asn1Tag Decode(ReadOnlySpan<byte> source, out int bytesConsumed)
		{
			Asn1Tag asn1Tag;
			if (Asn1Tag.TryDecode(source, out asn1Tag, out bytesConsumed))
			{
				return asn1Tag;
			}
			throw new InvalidOperationException("The provided data does not represent a valid tag.");
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x00124CB4 File Offset: 0x00123CB4
		public bool Equals(Asn1Tag other)
		{
			return this._controlFlags == other._controlFlags && this.TagValue == other.TagValue;
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x00124CD6 File Offset: 0x00123CD6
		public override bool Equals(object obj)
		{
			return obj is Asn1Tag && this.Equals((Asn1Tag)obj);
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x00124CEE File Offset: 0x00123CEE
		public override int GetHashCode()
		{
			return ((int)this._controlFlags << 24) ^ this.TagValue;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x00124D00 File Offset: 0x00123D00
		public static bool operator ==(Asn1Tag left, Asn1Tag right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x00124D0A File Offset: 0x00123D0A
		public static bool operator !=(Asn1Tag left, Asn1Tag right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x00124D17 File Offset: 0x00123D17
		public bool HasSameClassAndValue(Asn1Tag other)
		{
			return this.TagValue == other.TagValue && this.TagClass == other.TagClass;
		}

		// Token: 0x040029D7 RID: 10711
		private const byte ClassMask = 192;

		// Token: 0x040029D8 RID: 10712
		private const byte ConstructedMask = 32;

		// Token: 0x040029D9 RID: 10713
		private const byte ControlMask = 224;

		// Token: 0x040029DA RID: 10714
		private const byte TagNumberMask = 31;

		// Token: 0x040029DB RID: 10715
		internal static readonly Asn1Tag EndOfContents = new Asn1Tag(0, 0);

		// Token: 0x040029DC RID: 10716
		public static readonly Asn1Tag Integer = new Asn1Tag(0, 2);

		// Token: 0x040029DD RID: 10717
		public static readonly Asn1Tag PrimitiveBitString = new Asn1Tag(0, 3);

		// Token: 0x040029DE RID: 10718
		public static readonly Asn1Tag ConstructedBitString = new Asn1Tag(32, 3);

		// Token: 0x040029DF RID: 10719
		public static readonly Asn1Tag PrimitiveOctetString = new Asn1Tag(0, 4);

		// Token: 0x040029E0 RID: 10720
		public static readonly Asn1Tag ConstructedOctetString = new Asn1Tag(32, 4);

		// Token: 0x040029E1 RID: 10721
		public static readonly Asn1Tag Null = new Asn1Tag(0, 5);

		// Token: 0x040029E2 RID: 10722
		public static readonly Asn1Tag ObjectIdentifier = new Asn1Tag(0, 6);

		// Token: 0x040029E3 RID: 10723
		public static readonly Asn1Tag Sequence = new Asn1Tag(32, 16);

		// Token: 0x040029E4 RID: 10724
		public static readonly Asn1Tag SetOf = new Asn1Tag(32, 17);

		// Token: 0x040029E5 RID: 10725
		private readonly byte _controlFlags;

		// Token: 0x040029E6 RID: 10726
		private int _tagValue;
	}
}

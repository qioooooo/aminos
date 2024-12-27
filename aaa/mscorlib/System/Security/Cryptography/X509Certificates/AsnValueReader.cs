using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008B6 RID: 2230
	internal struct AsnValueReader
	{
		// Token: 0x060051D8 RID: 20952 RVA: 0x00126C2B File Offset: 0x00125C2B
		internal AsnValueReader(ReadOnlySpan<byte> span, AsnEncodingRules ruleSet)
		{
			this._span = span;
			this._ruleSet = ruleSet;
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x00126C3B File Offset: 0x00125C3B
		internal bool HasData
		{
			get
			{
				return !this._span.IsEmpty;
			}
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x00126C4B File Offset: 0x00125C4B
		internal void ThrowIfNotEmpty()
		{
			if (!this._span.IsEmpty)
			{
				new AsnReader(AsnValueReader.s_singleByte, this._ruleSet).ThrowIfNotEmpty();
			}
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00126C74 File Offset: 0x00125C74
		internal Asn1Tag PeekTag()
		{
			int num;
			return Asn1Tag.Decode(this._span, out num);
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x00126C90 File Offset: 0x00125C90
		internal ReadOnlySpan<byte> PeekContentBytes()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._span, this._ruleSet, out num, out num2, out num3);
			return this._span.Slice(num, num2);
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x00126CC4 File Offset: 0x00125CC4
		internal ReadOnlySpan<byte> PeekEncodedValue()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._span, this._ruleSet, out num, out num2, out num3);
			return this._span.Slice(0, num3);
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x00126CF8 File Offset: 0x00125CF8
		internal ReadOnlySpan<byte> ReadEncodedValue()
		{
			ReadOnlySpan<byte> readOnlySpan = this.PeekEncodedValue();
			this._span = this._span.Slice(readOnlySpan.Length);
			return readOnlySpan;
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x00126D28 File Offset: 0x00125D28
		internal bool TryReadInt32(out int value)
		{
			return this.TryReadInt32(out value, null);
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x00126D48 File Offset: 0x00125D48
		internal bool TryReadInt32(out int value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadInt32(this._span, this._ruleSet, out value, out num, expectedTag);
			this._span = this._span.Slice(num);
			return flag;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00126D80 File Offset: 0x00125D80
		internal ReadOnlySpan<byte> ReadIntegerBytes()
		{
			return this.ReadIntegerBytes(null);
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00126D9C File Offset: 0x00125D9C
		internal ReadOnlySpan<byte> ReadIntegerBytes(Asn1Tag? expectedTag)
		{
			int num;
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.ReadIntegerBytes(this._span, this._ruleSet, out num, expectedTag);
			this._span = this._span.Slice(num);
			return readOnlySpan;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x00126DD4 File Offset: 0x00125DD4
		internal bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlySpan<byte> value)
		{
			return this.TryReadPrimitiveBitString(out unusedBitCount, out value, null);
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x00126DF4 File Offset: 0x00125DF4
		internal bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlySpan<byte> value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveBitString(this._span, this._ruleSet, out unusedBitCount, out value, out num, expectedTag);
			this._span = this._span.Slice(num);
			return flag;
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x00126E2C File Offset: 0x00125E2C
		internal byte[] ReadBitString(out int unusedBitCount)
		{
			return this.ReadBitString(out unusedBitCount, null);
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x00126E4C File Offset: 0x00125E4C
		internal byte[] ReadBitString(out int unusedBitCount, Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadBitString(this._span, this._ruleSet, out unusedBitCount, out num, expectedTag);
			this._span = this._span.Slice(num);
			return array;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00126E84 File Offset: 0x00125E84
		internal bool TryReadPrimitiveOctetString(out ReadOnlySpan<byte> value)
		{
			return this.TryReadPrimitiveOctetString(out value, null);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00126EA4 File Offset: 0x00125EA4
		internal bool TryReadPrimitiveOctetString(out ReadOnlySpan<byte> value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveOctetString(this._span, this._ruleSet, out value, out num, expectedTag);
			this._span = this._span.Slice(num);
			return flag;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x00126EDC File Offset: 0x00125EDC
		internal byte[] ReadOctetString()
		{
			return this.ReadOctetString(null);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x00126EF8 File Offset: 0x00125EF8
		internal byte[] ReadOctetString(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadOctetString(this._span, this._ruleSet, out num, expectedTag);
			this._span = this._span.Slice(num);
			return array;
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x00126F30 File Offset: 0x00125F30
		internal byte[] ReadObjectIdentifier()
		{
			return this.ReadObjectIdentifier(null);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x00126F4C File Offset: 0x00125F4C
		internal byte[] ReadObjectIdentifier(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadObjectIdentifier(this._span, this._ruleSet, out num, expectedTag);
			this._span = this._span.Slice(num);
			return array;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00126F84 File Offset: 0x00125F84
		internal AsnValueReader ReadSequence()
		{
			return this.ReadSequence(null);
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x00126FA0 File Offset: 0x00125FA0
		internal AsnValueReader ReadSequence(Asn1Tag? expectedTag)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSequence(this._span, this._ruleSet, out num, out num2, out num3, expectedTag);
			ReadOnlySpan<byte> readOnlySpan = this._span.Slice(num, num2);
			this._span = this._span.Slice(num3);
			return new AsnValueReader(readOnlySpan, this._ruleSet);
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x00126FF4 File Offset: 0x00125FF4
		internal AsnValueReader ReadSetOf()
		{
			return this.ReadSetOf(null, false);
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x00127011 File Offset: 0x00126011
		internal AsnValueReader ReadSetOf(Asn1Tag? expectedTag)
		{
			return this.ReadSetOf(expectedTag, false);
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x0012701C File Offset: 0x0012601C
		internal AsnValueReader ReadSetOf(Asn1Tag? expectedTag, bool skipSortOrderValidation)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSetOf(this._span, this._ruleSet, out num, out num2, out num3, skipSortOrderValidation, expectedTag);
			ReadOnlySpan<byte> readOnlySpan = this._span.Slice(num, num2);
			this._span = this._span.Slice(num3);
			return new AsnValueReader(readOnlySpan, this._ruleSet);
		}

		// Token: 0x04002A04 RID: 10756
		private static readonly byte[] s_singleByte = new byte[1];

		// Token: 0x04002A05 RID: 10757
		private ReadOnlySpan<byte> _span;

		// Token: 0x04002A06 RID: 10758
		private readonly AsnEncodingRules _ruleSet;
	}
}

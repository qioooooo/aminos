using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008B3 RID: 2227
	internal class AsnReader
	{
		// Token: 0x060051BA RID: 20922 RVA: 0x00126680 File Offset: 0x00125680
		public bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlyMemory<byte> value, Asn1Tag? expectedTag)
		{
			ReadOnlySpan<byte> readOnlySpan;
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveBitString(this._data.Span, this.RuleSet, out unusedBitCount, out readOnlySpan, out num, expectedTag);
			if (flag)
			{
				value = AsnDecoder.Slice(this._data, readOnlySpan);
				this._data = this._data.Slice(num);
			}
			else
			{
				value = default(ReadOnlyMemory<byte>);
			}
			return flag;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001266DC File Offset: 0x001256DC
		public bool TryReadBitString(Span<byte> destination, out int unusedBitCount, out int bytesWritten, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadBitString(this._data.Span, destination, this.RuleSet, out unusedBitCount, out num, out bytesWritten, expectedTag);
			if (flag)
			{
				this._data = this._data.Slice(num);
			}
			return flag;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x00126720 File Offset: 0x00125720
		public byte[] ReadBitString(out int unusedBitCount, Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadBitString(this._data.Span, this.RuleSet, out unusedBitCount, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x060051BD RID: 20925 RVA: 0x0012675B File Offset: 0x0012575B
		public AsnEncodingRules RuleSet
		{
			get
			{
				return this._ruleSet;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x060051BE RID: 20926 RVA: 0x00126763 File Offset: 0x00125763
		public bool HasData
		{
			get
			{
				return !this._data.IsEmpty;
			}
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x00126773 File Offset: 0x00125773
		public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet, AsnReaderOptions options)
		{
			AsnDecoder.CheckEncodingRules(ruleSet);
			this._data = data;
			this._ruleSet = ruleSet;
			this._options = options;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00126798 File Offset: 0x00125798
		public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet)
			: this(data, ruleSet, default(AsnReaderOptions))
		{
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x001267B6 File Offset: 0x001257B6
		public void ThrowIfNotEmpty()
		{
			if (this.HasData)
			{
				throw new InvalidOperationException("The last expected value has been read, but the reader still has pending data. This value may be from a newer schema, or is corrupt.");
			}
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x001267CC File Offset: 0x001257CC
		public Asn1Tag PeekTag()
		{
			int num;
			return Asn1Tag.Decode(this._data.Span, out num);
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x001267EC File Offset: 0x001257EC
		public ReadOnlyMemory<byte> PeekEncodedValue()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._data.Span, this.RuleSet, out num, out num2, out num3);
			return this._data.Slice(0, num3);
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x00126824 File Offset: 0x00125824
		public ReadOnlyMemory<byte> PeekContentBytes()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._data.Span, this.RuleSet, out num, out num2, out num3);
			return this._data.Slice(num, num2);
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x0012685C File Offset: 0x0012585C
		public ReadOnlyMemory<byte> ReadEncodedValue()
		{
			ReadOnlyMemory<byte> readOnlyMemory = this.PeekEncodedValue();
			this._data = this._data.Slice(readOnlyMemory.Length);
			return readOnlyMemory;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00126889 File Offset: 0x00125889
		private AsnReader CloneAtSlice(int start, int length)
		{
			return new AsnReader(this._data.Slice(start, length), this.RuleSet, this._options);
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x001268AC File Offset: 0x001258AC
		public ReadOnlyMemory<byte> ReadIntegerBytes(Asn1Tag? expectedTag)
		{
			int num;
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.ReadIntegerBytes(this._data.Span, this.RuleSet, out num, expectedTag);
			ReadOnlyMemory<byte> readOnlyMemory = AsnDecoder.Slice(this._data, readOnlySpan);
			this._data = this._data.Slice(num);
			return readOnlyMemory;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001268F4 File Offset: 0x001258F4
		public bool TryReadInt32(out int value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadInt32(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x00126930 File Offset: 0x00125930
		public bool TryReadUInt32(out uint value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadUInt32(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x0012696C File Offset: 0x0012596C
		public bool TryReadInt64(out long value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadInt64(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001269A8 File Offset: 0x001259A8
		public bool TryReadUInt64(out ulong value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadUInt64(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x001269E4 File Offset: 0x001259E4
		public void ReadNull(Asn1Tag? expectedTag)
		{
			int num;
			AsnDecoder.ReadNull(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00126A1C File Offset: 0x00125A1C
		public bool TryReadOctetString(Span<byte> destination, out int bytesWritten, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadOctetString(this._data.Span, destination, this.RuleSet, out num, out bytesWritten, expectedTag);
			if (flag)
			{
				this._data = this._data.Slice(num);
			}
			return flag;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x00126A5C File Offset: 0x00125A5C
		public byte[] ReadOctetString(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadOctetString(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00126A98 File Offset: 0x00125A98
		public bool TryReadPrimitiveOctetString(out ReadOnlyMemory<byte> contents, Asn1Tag? expectedTag)
		{
			ReadOnlySpan<byte> readOnlySpan;
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveOctetString(this._data.Span, this.RuleSet, out readOnlySpan, out num, expectedTag);
			if (flag)
			{
				contents = AsnDecoder.Slice(this._data, readOnlySpan);
				this._data = this._data.Slice(num);
			}
			else
			{
				contents = default(ReadOnlyMemory<byte>);
			}
			return flag;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x00126AF4 File Offset: 0x00125AF4
		public byte[] ReadObjectIdentifier(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadObjectIdentifier(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00126B30 File Offset: 0x00125B30
		public AsnReader ReadSequence(Asn1Tag? expectedTag)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSequence(this._data.Span, this.RuleSet, out num, out num2, out num3, expectedTag);
			AsnReader asnReader = this.CloneAtSlice(num, num2);
			this._data = this._data.Slice(num3);
			return asnReader;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00126B78 File Offset: 0x00125B78
		public AsnReader ReadSetOf(Asn1Tag? expectedTag)
		{
			return this.ReadSetOf(this._options.SkipSetSortOrderVerification, expectedTag);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00126B9C File Offset: 0x00125B9C
		public AsnReader ReadSetOf(bool skipSortOrderValidation, Asn1Tag? expectedTag)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSetOf(this._data.Span, this.RuleSet, out num, out num2, out num3, skipSortOrderValidation, expectedTag);
			AsnReader asnReader = this.CloneAtSlice(num, num2);
			this._data = this._data.Slice(num3);
			return asnReader;
		}

		// Token: 0x040029F9 RID: 10745
		internal const int MaxCERSegmentSize = 1000;

		// Token: 0x040029FA RID: 10746
		private ReadOnlyMemory<byte> _data;

		// Token: 0x040029FB RID: 10747
		private readonly AsnReaderOptions _options;

		// Token: 0x040029FC RID: 10748
		private AsnEncodingRules _ruleSet;
	}
}

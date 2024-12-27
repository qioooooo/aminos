using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C2 RID: 2242
	internal struct AlgorithmIdentifierAsn
	{
		// Token: 0x06005225 RID: 21029 RVA: 0x0012879A File Offset: 0x0012779A
		internal static AlgorithmIdentifierAsn Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return AlgorithmIdentifierAsn.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001287A8 File Offset: 0x001277A8
		internal static AlgorithmIdentifierAsn Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			AlgorithmIdentifierAsn algorithmIdentifierAsn2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				AlgorithmIdentifierAsn algorithmIdentifierAsn;
				AlgorithmIdentifierAsn.DecodeCore(ref asnValueReader, expectedTag, encoded, out algorithmIdentifierAsn);
				asnValueReader.ThrowIfNotEmpty();
				algorithmIdentifierAsn2 = algorithmIdentifierAsn;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return algorithmIdentifierAsn2;
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00128800 File Offset: 0x00127800
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out AlgorithmIdentifierAsn decoded)
		{
			AlgorithmIdentifierAsn.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x00128810 File Offset: 0x00127810
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out AlgorithmIdentifierAsn decoded)
		{
			try
			{
				AlgorithmIdentifierAsn.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00128848 File Offset: 0x00127848
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out AlgorithmIdentifierAsn decoded)
		{
			decoded = default(AlgorithmIdentifierAsn);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			ReadOnlySpan<byte> span = rebind.Span;
			decoded.Algorithm = asnValueReader.ReadObjectIdentifier();
			if (asnValueReader.HasData)
			{
				ReadOnlySpan<byte> readOnlySpan = asnValueReader.ReadEncodedValue();
				int num;
				decoded.Parameters = new ReadOnlyMemory<byte>?(span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001288CA File Offset: 0x001278CA
		internal bool HasNullEquivalentParameters()
		{
			return AlgorithmIdentifierAsn.RepresentsNull(this.Parameters);
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x001288D8 File Offset: 0x001278D8
		internal static bool RepresentsNull(ReadOnlyMemory<byte>? parameters)
		{
			if (parameters == null)
			{
				return true;
			}
			ReadOnlySpan<byte> span = parameters.Value.Span;
			return span.Length == 2 && span[0] == 5 && span[1] == 0;
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x00128924 File Offset: 0x00127924
		// Note: this type is marked as 'beforefieldinit'.
		static AlgorithmIdentifierAsn()
		{
			byte[] array = new byte[2];
			array[0] = 5;
			AlgorithmIdentifierAsn.ExplicitDerNull = array;
		}

		// Token: 0x04002A30 RID: 10800
		internal byte[] Algorithm;

		// Token: 0x04002A31 RID: 10801
		internal ReadOnlyMemory<byte>? Parameters;

		// Token: 0x04002A32 RID: 10802
		internal static readonly ReadOnlyMemory<byte> ExplicitDerNull;
	}
}

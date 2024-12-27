using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008CC RID: 2252
	internal struct Pbkdf2Params
	{
		// Token: 0x0600525B RID: 21083 RVA: 0x0012954C File Offset: 0x0012854C
		internal static Pbkdf2Params Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return Pbkdf2Params.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x0012955C File Offset: 0x0012855C
		internal static Pbkdf2Params Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			Pbkdf2Params pbkdf2Params2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				Pbkdf2Params pbkdf2Params;
				Pbkdf2Params.DecodeCore(ref asnValueReader, expectedTag, encoded, out pbkdf2Params);
				asnValueReader.ThrowIfNotEmpty();
				pbkdf2Params2 = pbkdf2Params;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return pbkdf2Params2;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x001295B4 File Offset: 0x001285B4
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out Pbkdf2Params decoded)
		{
			Pbkdf2Params.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001295C4 File Offset: 0x001285C4
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out Pbkdf2Params decoded)
		{
			try
			{
				Pbkdf2Params.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x001295FC File Offset: 0x001285FC
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out Pbkdf2Params decoded)
		{
			decoded = default(Pbkdf2Params);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			Pbkdf2SaltChoice.Decode(ref asnValueReader, rebind, out decoded.Salt);
			if (!asnValueReader.TryReadInt32(out decoded.IterationCount))
			{
				asnValueReader.ThrowIfNotEmpty();
			}
			if (asnValueReader.HasData && asnValueReader.PeekTag().HasSameClassAndValue(Asn1Tag.Integer))
			{
				int num;
				if (asnValueReader.TryReadInt32(out num))
				{
					decoded.KeyLength = new int?(num);
				}
				else
				{
					asnValueReader.ThrowIfNotEmpty();
				}
			}
			if (asnValueReader.HasData && asnValueReader.PeekTag().HasSameClassAndValue(Asn1Tag.Sequence))
			{
				AlgorithmIdentifierAsn.Decode(ref asnValueReader, rebind, out decoded.Prf);
			}
			else
			{
				AsnValueReader asnValueReader2 = new AsnValueReader(Pbkdf2Params.s_DefaultPrf, AsnEncodingRules.DER);
				AlgorithmIdentifierAsn.Decode(ref asnValueReader2, rebind, out decoded.Prf);
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x04002A49 RID: 10825
		private static readonly byte[] s_DefaultPrf = new byte[]
		{
			48, 12, 6, 8, 42, 134, 72, 134, 247, 13,
			2, 7, 5, 0
		};

		// Token: 0x04002A4A RID: 10826
		internal Pbkdf2SaltChoice Salt;

		// Token: 0x04002A4B RID: 10827
		internal int IterationCount;

		// Token: 0x04002A4C RID: 10828
		internal int? KeyLength;

		// Token: 0x04002A4D RID: 10829
		internal AlgorithmIdentifierAsn Prf;
	}
}

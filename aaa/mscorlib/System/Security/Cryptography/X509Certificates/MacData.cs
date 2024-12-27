using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C9 RID: 2249
	internal struct MacData
	{
		// Token: 0x0600524B RID: 21067 RVA: 0x00129158 File Offset: 0x00128158
		internal static MacData Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return MacData.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x00129168 File Offset: 0x00128168
		internal static MacData Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			MacData macData2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				MacData macData;
				MacData.DecodeCore(ref asnValueReader, expectedTag, encoded, out macData);
				asnValueReader.ThrowIfNotEmpty();
				macData2 = macData;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return macData2;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x001291C0 File Offset: 0x001281C0
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out MacData decoded)
		{
			MacData.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001291D0 File Offset: 0x001281D0
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out MacData decoded)
		{
			try
			{
				MacData.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00129208 File Offset: 0x00128208
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out MacData decoded)
		{
			decoded = default(MacData);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			ReadOnlySpan<byte> span = rebind.Span;
			DigestInfoAsn.Decode(ref asnValueReader, rebind, out decoded.Mac);
			ReadOnlySpan<byte> readOnlySpan;
			if (asnValueReader.TryReadPrimitiveOctetString(out readOnlySpan))
			{
				int num;
				decoded.MacSalt = (span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
			}
			else
			{
				decoded.MacSalt = asnValueReader.ReadOctetString();
			}
			if (asnValueReader.HasData && asnValueReader.PeekTag().HasSameClassAndValue(Asn1Tag.Integer))
			{
				if (!asnValueReader.TryReadInt32(out decoded.IterationCount))
				{
					asnValueReader.ThrowIfNotEmpty();
				}
			}
			else
			{
				AsnValueReader asnValueReader2 = new AsnValueReader(MacData.s_DefaultIterationCount, AsnEncodingRules.DER);
				if (!asnValueReader2.TryReadInt32(out decoded.IterationCount))
				{
					asnValueReader2.ThrowIfNotEmpty();
				}
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x04002A41 RID: 10817
		private static readonly byte[] s_DefaultIterationCount = new byte[] { 2, 1, 1 };

		// Token: 0x04002A42 RID: 10818
		internal DigestInfoAsn Mac;

		// Token: 0x04002A43 RID: 10819
		internal ReadOnlyMemory<byte> MacSalt;

		// Token: 0x04002A44 RID: 10820
		internal int IterationCount;
	}
}

using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C8 RID: 2248
	internal struct EncryptedPrivateKeyInfoAsn
	{
		// Token: 0x06005246 RID: 21062 RVA: 0x0012901C File Offset: 0x0012801C
		internal static EncryptedPrivateKeyInfoAsn Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return EncryptedPrivateKeyInfoAsn.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x0012902C File Offset: 0x0012802C
		internal static EncryptedPrivateKeyInfoAsn Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			EncryptedPrivateKeyInfoAsn encryptedPrivateKeyInfoAsn2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				EncryptedPrivateKeyInfoAsn encryptedPrivateKeyInfoAsn;
				EncryptedPrivateKeyInfoAsn.DecodeCore(ref asnValueReader, expectedTag, encoded, out encryptedPrivateKeyInfoAsn);
				asnValueReader.ThrowIfNotEmpty();
				encryptedPrivateKeyInfoAsn2 = encryptedPrivateKeyInfoAsn;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return encryptedPrivateKeyInfoAsn2;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x00129084 File Offset: 0x00128084
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out EncryptedPrivateKeyInfoAsn decoded)
		{
			EncryptedPrivateKeyInfoAsn.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x00129094 File Offset: 0x00128094
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out EncryptedPrivateKeyInfoAsn decoded)
		{
			try
			{
				EncryptedPrivateKeyInfoAsn.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001290CC File Offset: 0x001280CC
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out EncryptedPrivateKeyInfoAsn decoded)
		{
			decoded = default(EncryptedPrivateKeyInfoAsn);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			ReadOnlySpan<byte> span = rebind.Span;
			AlgorithmIdentifierAsn.Decode(ref asnValueReader, rebind, out decoded.EncryptionAlgorithm);
			ReadOnlySpan<byte> readOnlySpan;
			if (asnValueReader.TryReadPrimitiveOctetString(out readOnlySpan))
			{
				int num;
				decoded.EncryptedData = (span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
			}
			else
			{
				decoded.EncryptedData = asnValueReader.ReadOctetString();
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x04002A3F RID: 10815
		internal AlgorithmIdentifierAsn EncryptionAlgorithm;

		// Token: 0x04002A40 RID: 10816
		internal ReadOnlyMemory<byte> EncryptedData;
	}
}

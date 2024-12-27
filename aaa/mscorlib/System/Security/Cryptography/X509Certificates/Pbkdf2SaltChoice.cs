using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008CD RID: 2253
	internal struct Pbkdf2SaltChoice
	{
		// Token: 0x06005261 RID: 21089 RVA: 0x00129708 File Offset: 0x00128708
		internal static Pbkdf2SaltChoice Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			Pbkdf2SaltChoice pbkdf2SaltChoice2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				Pbkdf2SaltChoice pbkdf2SaltChoice;
				Pbkdf2SaltChoice.DecodeCore(ref asnValueReader, encoded, out pbkdf2SaltChoice);
				asnValueReader.ThrowIfNotEmpty();
				pbkdf2SaltChoice2 = pbkdf2SaltChoice;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return pbkdf2SaltChoice2;
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x0012975C File Offset: 0x0012875C
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out Pbkdf2SaltChoice decoded)
		{
			try
			{
				Pbkdf2SaltChoice.DecodeCore(ref reader, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x00129790 File Offset: 0x00128790
		private static void DecodeCore(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out Pbkdf2SaltChoice decoded)
		{
			decoded = default(Pbkdf2SaltChoice);
			Asn1Tag asn1Tag = reader.PeekTag();
			ReadOnlySpan<byte> span = rebind.Span;
			if (asn1Tag.HasSameClassAndValue(Asn1Tag.PrimitiveOctetString))
			{
				ReadOnlySpan<byte> readOnlySpan;
				if (reader.TryReadPrimitiveOctetString(out readOnlySpan))
				{
					int num;
					decoded.Specified = new ReadOnlyMemory<byte>?(span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
					return;
				}
				decoded.Specified = new ReadOnlyMemory<byte>?(reader.ReadOctetString());
				return;
			}
			else
			{
				if (asn1Tag.HasSameClassAndValue(Asn1Tag.Sequence))
				{
					AlgorithmIdentifierAsn algorithmIdentifierAsn;
					AlgorithmIdentifierAsn.Decode(ref reader, rebind, out algorithmIdentifierAsn);
					decoded.OtherSource = new AlgorithmIdentifierAsn?(algorithmIdentifierAsn);
					return;
				}
				throw new CryptographicException();
			}
		}

		// Token: 0x04002A4E RID: 10830
		internal ReadOnlyMemory<byte>? Specified;

		// Token: 0x04002A4F RID: 10831
		internal AlgorithmIdentifierAsn? OtherSource;
	}
}

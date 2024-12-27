using System;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C7 RID: 2247
	internal struct EncryptedDataAsn
	{
		// Token: 0x06005241 RID: 21057 RVA: 0x00128EB4 File Offset: 0x00127EB4
		internal static EncryptedDataAsn Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return EncryptedDataAsn.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x00128EC4 File Offset: 0x00127EC4
		internal static EncryptedDataAsn Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			EncryptedDataAsn encryptedDataAsn2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				EncryptedDataAsn encryptedDataAsn;
				EncryptedDataAsn.DecodeCore(ref asnValueReader, expectedTag, encoded, out encryptedDataAsn);
				asnValueReader.ThrowIfNotEmpty();
				encryptedDataAsn2 = encryptedDataAsn;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return encryptedDataAsn2;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00128F1C File Offset: 0x00127F1C
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out EncryptedDataAsn decoded)
		{
			EncryptedDataAsn.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x00128F2C File Offset: 0x00127F2C
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out EncryptedDataAsn decoded)
		{
			try
			{
				EncryptedDataAsn.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x00128F64 File Offset: 0x00127F64
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out EncryptedDataAsn decoded)
		{
			decoded = default(EncryptedDataAsn);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			if (!asnValueReader.TryReadInt32(out decoded.Version))
			{
				asnValueReader.ThrowIfNotEmpty();
			}
			EncryptedContentInfoAsn.Decode(ref asnValueReader, rebind, out decoded.EncryptedContentInfo);
			if (asnValueReader.HasData && asnValueReader.PeekTag().HasSameClassAndValue(new Asn1Tag(TagClass.ContextSpecific, 1)))
			{
				AsnValueReader asnValueReader2 = asnValueReader.ReadSetOf(new Asn1Tag?(new Asn1Tag(TagClass.ContextSpecific, 1)));
				List<AttributeAsn> list = new List<AttributeAsn>();
				while (asnValueReader2.HasData)
				{
					AttributeAsn attributeAsn;
					AttributeAsn.Decode(ref asnValueReader2, rebind, out attributeAsn);
					list.Add(attributeAsn);
				}
				decoded.UnprotectedAttributes = list.ToArray();
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x04002A3C RID: 10812
		internal int Version;

		// Token: 0x04002A3D RID: 10813
		internal EncryptedContentInfoAsn EncryptedContentInfo;

		// Token: 0x04002A3E RID: 10814
		internal AttributeAsn[] UnprotectedAttributes;
	}
}

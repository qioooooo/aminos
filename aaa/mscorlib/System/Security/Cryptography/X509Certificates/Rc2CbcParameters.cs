﻿using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008CF RID: 2255
	internal struct Rc2CbcParameters
	{
		// Token: 0x0600526A RID: 21098 RVA: 0x00129E93 File Offset: 0x00128E93
		internal static Rc2CbcParameters Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return Rc2CbcParameters.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x00129EA4 File Offset: 0x00128EA4
		internal static Rc2CbcParameters Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			Rc2CbcParameters rc2CbcParameters2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				Rc2CbcParameters rc2CbcParameters;
				Rc2CbcParameters.DecodeCore(ref asnValueReader, expectedTag, encoded, out rc2CbcParameters);
				asnValueReader.ThrowIfNotEmpty();
				rc2CbcParameters2 = rc2CbcParameters;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return rc2CbcParameters2;
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x00129EFC File Offset: 0x00128EFC
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out Rc2CbcParameters decoded)
		{
			Rc2CbcParameters.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x00129F0C File Offset: 0x00128F0C
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out Rc2CbcParameters decoded)
		{
			try
			{
				Rc2CbcParameters.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00129F44 File Offset: 0x00128F44
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out Rc2CbcParameters decoded)
		{
			decoded = default(Rc2CbcParameters);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			ReadOnlySpan<byte> span = rebind.Span;
			if (!asnValueReader.TryReadInt32(out decoded.Rc2Version))
			{
				asnValueReader.ThrowIfNotEmpty();
			}
			ReadOnlySpan<byte> readOnlySpan;
			if (asnValueReader.TryReadPrimitiveOctetString(out readOnlySpan))
			{
				int num;
				decoded.Iv = (span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
			}
			else
			{
				decoded.Iv = asnValueReader.ReadOctetString();
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00129FD8 File Offset: 0x00128FD8
		internal Rc2CbcParameters(ReadOnlyMemory<byte> iv, int keySize)
		{
			this.Rc2Version = ((keySize > 255) ? keySize : ((int)Rc2CbcParameters.s_Rc2EkbEncoding[keySize]));
			this.Iv = iv;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x00129FF9 File Offset: 0x00128FF9
		internal int GetEffectiveKeyBits()
		{
			if (this.Rc2Version <= 255)
			{
				return Array.IndexOf<byte>(Rc2CbcParameters.s_Rc2EkbEncoding, (byte)this.Rc2Version);
			}
			return this.Rc2Version;
		}

		// Token: 0x04002A54 RID: 10836
		internal int Rc2Version;

		// Token: 0x04002A55 RID: 10837
		internal ReadOnlyMemory<byte> Iv;

		// Token: 0x04002A56 RID: 10838
		private static readonly byte[] s_Rc2EkbEncoding = new byte[]
		{
			189, 86, 234, 242, 162, 241, 172, 42, 176, 147,
			209, 156, 27, 51, 253, 208, 48, 4, 182, 220,
			125, 223, 50, 75, 247, 203, 69, 155, 49, 187,
			33, 90, 65, 159, 225, 217, 74, 77, 158, 218,
			160, 104, 44, 195, 39, 95, 128, 54, 62, 238,
			251, 149, 26, 254, 206, 168, 52, 169, 19, 240,
			166, 63, 216, 12, 120, 36, 175, 35, 82, 193,
			103, 23, 245, 102, 144, 231, 232, 7, 184, 96,
			72, 230, 30, 83, 243, 146, 164, 114, 140, 8,
			21, 110, 134, 0, 132, 250, 244, 127, 138, 66,
			25, 246, 219, 205, 20, 141, 80, 18, 186, 60,
			6, 78, 236, 179, 53, 17, 161, 136, 142, 43,
			148, 153, 183, 113, 116, 211, 228, 191, 58, 222,
			150, 14, 188, 10, 237, 119, 252, 55, 107, 3,
			121, 137, 98, 198, 215, 192, 210, 124, 106, 139,
			34, 163, 91, 5, 93, 2, 117, 213, 97, 227,
			24, 143, 85, 81, 173, 31, 11, 94, 133, 229,
			194, 87, 99, 202, 61, 108, 180, 197, 204, 112,
			178, 145, 89, 13, 71, 32, 200, 79, 88, 224,
			1, 226, 22, 56, 196, 111, 59, 15, 101, 70,
			190, 126, 45, 123, 130, 249, 64, 181, 29, 115,
			248, 235, 38, 199, 135, 151, 37, 84, 177, 40,
			170, 152, 157, 165, 100, 109, 122, 212, 16, 129,
			68, 239, 73, 214, 174, 46, 221, 118, 92, 47,
			167, 28, 201, 9, 105, 154, 131, 207, 41, 57,
			185, 233, 76, byte.MaxValue, 67, 171
		};
	}
}

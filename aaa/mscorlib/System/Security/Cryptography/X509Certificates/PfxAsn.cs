using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008CE RID: 2254
	internal struct PfxAsn
	{
		// Token: 0x06005264 RID: 21092 RVA: 0x00129841 File Offset: 0x00128841
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out PfxAsn decoded)
		{
			PfxAsn.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x00129850 File Offset: 0x00128850
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out PfxAsn decoded)
		{
			try
			{
				PfxAsn.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x00129888 File Offset: 0x00128888
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out PfxAsn decoded)
		{
			decoded = default(PfxAsn);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			if (!asnValueReader.TryReadInt32(out decoded.Version))
			{
				asnValueReader.ThrowIfNotEmpty();
			}
			ContentInfoAsn.Decode(ref asnValueReader, rebind, out decoded.AuthSafe);
			if (asnValueReader.HasData && asnValueReader.PeekTag().HasSameClassAndValue(Asn1Tag.Sequence))
			{
				MacData macData;
				global::System.Security.Cryptography.X509Certificates.MacData.Decode(ref asnValueReader, rebind, out macData);
				decoded.MacData = new MacData?(macData);
			}
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x0012990C File Offset: 0x0012890C
		internal ulong CountTotalIterations()
		{
			ulong num = 0UL;
			if (!Helpers.SequenceEqual(this.AuthSafe.ContentType, Oids.Pkcs7Data))
			{
				throw new CryptographicException("ASN1 corrupted data.");
			}
			ReadOnlyMemory<byte> readOnlyMemory = Helpers.DecodeOctetStringAsMemory(this.AuthSafe.Content);
			AsnValueReader asnValueReader = new AsnValueReader(readOnlyMemory.Span, AsnEncodingRules.BER);
			AsnValueReader asnValueReader2 = asnValueReader.ReadSequence();
			asnValueReader.ThrowIfNotEmpty();
			bool flag = false;
			checked
			{
				while (asnValueReader2.HasData)
				{
					ContentInfoAsn contentInfoAsn;
					ContentInfoAsn.Decode(ref asnValueReader2, readOnlyMemory, out contentInfoAsn);
					ArraySegment<byte>? arraySegment = null;
					try
					{
						ReadOnlyMemory<byte> readOnlyMemory2;
						if (!Helpers.SequenceEqual(contentInfoAsn.ContentType, Oids.Pkcs7Data))
						{
							if (!Helpers.SequenceEqual(contentInfoAsn.ContentType, Oids.Pkcs7Encrypted))
							{
								throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_PfxWithoutPassword"));
							}
							if (flag)
							{
								throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_PfxWithoutPassword"));
							}
							uint num2;
							ArraySegment<byte> arraySegment2 = PfxAsn.DecryptContentInfo(contentInfoAsn, out num2);
							readOnlyMemory2 = arraySegment2;
							arraySegment = new ArraySegment<byte>?(arraySegment2);
							flag = true;
							num += unchecked((ulong)num2);
						}
						else
						{
							readOnlyMemory2 = Helpers.DecodeOctetStringAsMemory(contentInfoAsn.Content);
						}
						AsnValueReader asnValueReader3 = new AsnValueReader(readOnlyMemory2.Span, AsnEncodingRules.BER);
						AsnValueReader asnValueReader4 = asnValueReader3.ReadSequence();
						asnValueReader3.ThrowIfNotEmpty();
						while (asnValueReader4.HasData)
						{
							SafeBagAsn safeBagAsn;
							SafeBagAsn.Decode(ref asnValueReader4, readOnlyMemory2, out safeBagAsn);
							if (Helpers.SequenceEqual(safeBagAsn.BagId, Oids.Pkcs12ShroudedKeyBag))
							{
								AsnValueReader asnValueReader5 = new AsnValueReader(safeBagAsn.BagValue.Span, AsnEncodingRules.BER);
								EncryptedPrivateKeyInfoAsn encryptedPrivateKeyInfoAsn;
								EncryptedPrivateKeyInfoAsn.Decode(ref asnValueReader5, safeBagAsn.BagValue, out encryptedPrivateKeyInfoAsn);
								num += unchecked((ulong)PfxAsn.IterationsFromParameters(ref encryptedPrivateKeyInfoAsn.EncryptionAlgorithm));
							}
						}
					}
					finally
					{
						if (arraySegment != null)
						{
							CryptoPool.Return(arraySegment.Value);
						}
					}
				}
				if (this.MacData != null)
				{
					if (this.MacData.Value.IterationCount < 0)
					{
						throw new CryptographicException("ASN1 corrupted data.");
					}
					num += unchecked((ulong)(checked((uint)this.MacData.Value.IterationCount)));
				}
				return num;
			}
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x00129B0C File Offset: 0x00128B0C
		private static ArraySegment<byte> DecryptContentInfo(ContentInfoAsn contentInfo, out uint iterations)
		{
			char[] array = new char[0];
			byte[] array2 = new byte[0];
			char[] array3 = null;
			byte[] array4 = null;
			EncryptedDataAsn encryptedDataAsn = EncryptedDataAsn.Decode(contentInfo.Content, AsnEncodingRules.BER);
			if (encryptedDataAsn.Version != 0 && encryptedDataAsn.Version != 2)
			{
				throw new CryptographicException("ASN1 corrupted data.");
			}
			if (!Helpers.SequenceEqual(encryptedDataAsn.EncryptedContentInfo.ContentType, Oids.Pkcs7Data))
			{
				throw new CryptographicException("ASN1 corrupted data.");
			}
			if (encryptedDataAsn.EncryptedContentInfo.EncryptedContent == null)
			{
				throw new CryptographicException("ASN1 corrupted data.");
			}
			iterations = PfxAsn.IterationsFromParameters(ref encryptedDataAsn.EncryptedContentInfo.ContentEncryptionAlgorithm);
			if (iterations > 600000U)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_PfxWithoutPassword"));
			}
			int length = encryptedDataAsn.EncryptedContentInfo.EncryptedContent.Value.Length;
			byte[] array5 = new byte[length];
			int num = 0;
			try
			{
				num = PasswordBasedEncryption.Decrypt(ref encryptedDataAsn.EncryptedContentInfo.ContentEncryptionAlgorithm, array, array2, encryptedDataAsn.EncryptedContentInfo.EncryptedContent.Value.Span, array5);
				AsnValueReader asnValueReader = new AsnValueReader(new ReadOnlySpan<byte>(array5, 0, num), AsnEncodingRules.BER);
				asnValueReader.ReadSequence();
				asnValueReader.ThrowIfNotEmpty();
			}
			catch
			{
				num = PasswordBasedEncryption.Decrypt(ref encryptedDataAsn.EncryptedContentInfo.ContentEncryptionAlgorithm, array3, array4, encryptedDataAsn.EncryptedContentInfo.EncryptedContent.Value.Span, array5);
				AsnValueReader asnValueReader2 = new AsnValueReader(new ReadOnlySpan<byte>(array5, 0, num), AsnEncodingRules.BER);
				asnValueReader2.ReadSequence();
				asnValueReader2.ThrowIfNotEmpty();
			}
			finally
			{
				if (num == 0)
				{
					CryptographicOperations.ZeroMemory(array5);
				}
			}
			return new ArraySegment<byte>(array5, 0, num);
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x00129CF0 File Offset: 0x00128CF0
		private static uint IterationsFromParameters(ref AlgorithmIdentifierAsn algorithmIdentifier)
		{
			if (Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.PasswordBasedEncryptionScheme2))
			{
				if (algorithmIdentifier.Parameters == null)
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				PBES2Params pbes2Params = PBES2Params.Decode(algorithmIdentifier.Parameters.Value, AsnEncodingRules.BER);
				if (!Helpers.SequenceEqual(pbes2Params.KeyDerivationFunc.Algorithm, Oids.Pbkdf2))
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				if (pbes2Params.KeyDerivationFunc.Parameters == null)
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				Pbkdf2Params pbkdf2Params = Pbkdf2Params.Decode(pbes2Params.KeyDerivationFunc.Parameters.Value, AsnEncodingRules.BER);
				if (pbkdf2Params.IterationCount < 0)
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				return (uint)pbkdf2Params.IterationCount;
			}
			else
			{
				if (!Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.PbeWithMD5AndDESCBC) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.PbeWithMD5AndRC2CBC) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.PbeWithSha1AndDESCBC) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.PbeWithSha1AndRC2CBC) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.Pkcs12PbeWithShaAnd3Key3Des) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.Pkcs12PbeWithShaAnd2Key3Des) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.Pkcs12PbeWithShaAnd128BitRC2) && !Helpers.SequenceEqual(algorithmIdentifier.Algorithm, Oids.Pkcs12PbeWithShaAnd40BitRC2))
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				if (algorithmIdentifier.Parameters == null)
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				PBEParameter pbeparameter = PBEParameter.Decode(algorithmIdentifier.Parameters.Value, AsnEncodingRules.BER);
				if (pbeparameter.IterationCount < 0)
				{
					throw new CryptographicException("ASN1 corrupted data.");
				}
				return (uint)pbeparameter.IterationCount;
			}
		}

		// Token: 0x04002A50 RID: 10832
		private const uint MaxIterationWork = 600000U;

		// Token: 0x04002A51 RID: 10833
		internal int Version;

		// Token: 0x04002A52 RID: 10834
		internal ContentInfoAsn AuthSafe;

		// Token: 0x04002A53 RID: 10835
		internal MacData? MacData;
	}
}

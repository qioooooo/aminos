using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008AE RID: 2222
	internal static class AsnDecoder
	{
		// Token: 0x0600517F RID: 20863 RVA: 0x00124DC8 File Offset: 0x00123DC8
		public static bool TryReadPrimitiveBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int? num;
			int num2;
			int num3;
			ReadOnlySpan<byte> readOnlySpan;
			int num4;
			byte b;
			if (AsnDecoder.TryReadPrimitiveBitStringCore(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveBitString, out num, out num2, out num3, out readOnlySpan, out num4, out b) && (readOnlySpan.Length == 0 || b == readOnlySpan[readOnlySpan.Length - 1]))
			{
				unusedBitCount = num3;
				value = readOnlySpan;
				bytesConsumed = num4;
				return true;
			}
			unusedBitCount = 0;
			value = default(ReadOnlySpan<byte>);
			bytesConsumed = 0;
			return false;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x00124E44 File Offset: 0x00123E44
		public static bool TryReadBitString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag)
		{
			if (source.Overlaps(destination))
			{
				throw new ArgumentException("The destination buffer overlaps the source buffer.", "destination");
			}
			int? num;
			int num2;
			int num3;
			ReadOnlySpan<byte> readOnlySpan;
			int num4;
			byte b;
			if (AsnDecoder.TryReadPrimitiveBitStringCore(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveBitString, out num, out num2, out num3, out readOnlySpan, out num4, out b))
			{
				if (readOnlySpan.Length > destination.Length)
				{
					bytesConsumed = 0;
					bytesWritten = 0;
					unusedBitCount = 0;
					return false;
				}
				AsnDecoder.CopyBitStringValue(readOnlySpan, b, destination);
				bytesWritten = readOnlySpan.Length;
				bytesConsumed = num4;
				unusedBitCount = num3;
				return true;
			}
			else
			{
				int num5;
				int num6;
				if (AsnDecoder.TryCopyConstructedBitStringValue(AsnDecoder.Slice(source, num2, num), ruleSet, destination, num == null, out num3, out num5, out num6))
				{
					unusedBitCount = num3;
					bytesConsumed = num2 + num5;
					bytesWritten = num6;
					return true;
				}
				bytesWritten = (bytesConsumed = (unusedBitCount = 0));
				return false;
			}
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x00124F24 File Offset: 0x00123F24
		public static byte[] ReadBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int? num;
			int num2;
			int num3;
			ReadOnlySpan<byte> readOnlySpan;
			int num4;
			byte b;
			if (AsnDecoder.TryReadPrimitiveBitStringCore(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveBitString, out num, out num2, out num3, out readOnlySpan, out num4, out b))
			{
				byte[] array = readOnlySpan.ToArray();
				if (readOnlySpan.Length > 0)
				{
					array[array.Length - 1] = b;
				}
				unusedBitCount = num3;
				bytesConsumed = num4;
				return array;
			}
			int num5 = num ?? AsnDecoder.SeekEndOfContents(source.Slice(num2), ruleSet);
			byte[] array2 = CryptoPool.Rent(num5);
			int num6;
			int num7;
			if (AsnDecoder.TryCopyConstructedBitStringValue(AsnDecoder.Slice(source, num2, num), ruleSet, array2, num == null, out num3, out num6, out num7))
			{
				byte[] array3 = Utility.GetSpanForArray<byte>(array2, 0, num7).ToArray();
				CryptoPool.Return(array2, num7);
				unusedBitCount = num3;
				bytesConsumed = num2 + num6;
				return array3;
			}
			throw new InvalidOperationException("TryCopyConstructedBitStringValue failed with a pre-allocated buffer");
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x00125014 File Offset: 0x00124014
		private static void ParsePrimitiveBitStringContents(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out ReadOnlySpan<byte> value, out byte normalizedLastByte)
		{
			if (ruleSet == AsnEncodingRules.CER && source.Length > 1000)
			{
				throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER or DER encoding.");
			}
			if (source.Length == 0)
			{
				throw new InvalidOperationException();
			}
			unusedBitCount = (int)source[0];
			if (unusedBitCount > 7)
			{
				throw new InvalidOperationException();
			}
			if (source.Length == 1)
			{
				if (unusedBitCount > 0)
				{
					throw new InvalidOperationException();
				}
				value = ReadOnlySpan<byte>.Empty;
				normalizedLastByte = 0;
				return;
			}
			else
			{
				int num = -1 << unusedBitCount;
				byte b = source[source.Length - 1];
				byte b2 = (byte)((int)b & num);
				if (b2 != b && (ruleSet == AsnEncodingRules.DER || ruleSet == AsnEncodingRules.CER))
				{
					throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
				}
				normalizedLastByte = b2;
				value = source.Slice(1);
				return;
			}
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001250D3 File Offset: 0x001240D3
		private static void CopyBitStringValue(ReadOnlySpan<byte> value, byte normalizedLastByte, Span<byte> destination)
		{
			if (value.Length == 0)
			{
				return;
			}
			value.CopyTo(destination);
			destination[value.Length - 1] = normalizedLastByte;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001250F8 File Offset: 0x001240F8
		private static int CountConstructedBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, bool isIndefinite)
		{
			Span<byte> empty = Span<byte>.Empty;
			int num;
			int num2;
			return AsnDecoder.ProcessConstructedBitString(source, ruleSet, empty, null, isIndefinite, out num, out num2);
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x0012511C File Offset: 0x0012411C
		private static void CopyConstructedBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> destination, bool isIndefinite, out int unusedBitCount, out int bytesRead, out int bytesWritten)
		{
			bytesWritten = AsnDecoder.ProcessConstructedBitString(source, ruleSet, destination, new AsnDecoder.BitStringCopyAction(AsnDecoder.CopyBitStringValue), isIndefinite, out unusedBitCount, out bytesRead);
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x00125148 File Offset: 0x00124148
		private static int ProcessConstructedBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> destination, AsnDecoder.BitStringCopyAction copyAction, bool isIndefinite, out int lastUnusedBitCount, out int bytesRead)
		{
			lastUnusedBitCount = 0;
			bytesRead = 0;
			int num = 1000;
			ReadOnlySpan<byte> readOnlySpan = source;
			Stack stack = null;
			int num2 = 0;
			Asn1Tag asn1Tag = Asn1Tag.ConstructedBitString;
			Span<byte> span = destination;
			for (;;)
			{
				if (!readOnlySpan.IsEmpty)
				{
					int? num3;
					int num4;
					asn1Tag = AsnDecoder.ReadTagAndLength(readOnlySpan, ruleSet, out num3, out num4);
					if (asn1Tag == Asn1Tag.PrimitiveBitString)
					{
						if (lastUnusedBitCount != 0)
						{
							break;
						}
						if (ruleSet == AsnEncodingRules.CER && num != 1000)
						{
							goto Block_4;
						}
						ReadOnlySpan<byte> readOnlySpan2 = AsnDecoder.Slice(readOnlySpan, num4, num3.Value);
						ReadOnlySpan<byte> readOnlySpan3;
						byte b;
						AsnDecoder.ParsePrimitiveBitStringContents(readOnlySpan2, ruleSet, out lastUnusedBitCount, out readOnlySpan3, out b);
						int num5 = num4 + readOnlySpan2.Length;
						readOnlySpan = readOnlySpan.Slice(num5);
						bytesRead += num5;
						num2 += readOnlySpan3.Length;
						num = readOnlySpan2.Length;
						if (copyAction != null)
						{
							copyAction(readOnlySpan3, b, span);
							span = span.Slice(readOnlySpan3.Length);
							continue;
						}
						continue;
					}
					else if (asn1Tag == Asn1Tag.EndOfContents && isIndefinite)
					{
						AsnDecoder.ValidateEndOfContents(asn1Tag, num3, num4);
						bytesRead += num4;
						if (stack != null && stack.Count > 0)
						{
							AsnDecoder.ParseFrame parseFrame = (AsnDecoder.ParseFrame)stack.Pop();
							readOnlySpan = source.Slice(parseFrame.Offset, parseFrame.Length).Slice(bytesRead);
							bytesRead += parseFrame.BytesRead;
							isIndefinite = parseFrame.Indefinite;
							continue;
						}
					}
					else
					{
						if (!(asn1Tag == Asn1Tag.ConstructedBitString))
						{
							goto IL_01CB;
						}
						if (ruleSet == AsnEncodingRules.CER)
						{
							goto Block_11;
						}
						if (stack == null)
						{
							stack = new Stack();
						}
						int num6;
						if (!source.Overlaps(readOnlySpan, out num6))
						{
							goto Block_13;
						}
						stack.Push(new AsnDecoder.ParseFrame(num6, readOnlySpan.Length, isIndefinite, bytesRead));
						readOnlySpan = AsnDecoder.Slice(readOnlySpan, num4, num3);
						bytesRead = num4;
						isIndefinite = num3 == null;
						continue;
					}
				}
				if (isIndefinite && asn1Tag != Asn1Tag.EndOfContents)
				{
					goto Block_15;
				}
				if (stack == null || stack.Count <= 0)
				{
					return num2;
				}
				AsnDecoder.ParseFrame parseFrame2 = (AsnDecoder.ParseFrame)stack.Pop();
				readOnlySpan = source.Slice(parseFrame2.Offset, parseFrame2.Length).Slice(bytesRead);
				isIndefinite = parseFrame2.Indefinite;
				bytesRead += parseFrame2.BytesRead;
			}
			throw new InvalidOperationException();
			Block_4:
			throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER or DER encoding.");
			Block_11:
			throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			Block_13:
			throw new InvalidOperationException();
			IL_01CB:
			throw new InvalidOperationException();
			Block_15:
			throw new InvalidOperationException();
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x001253A0 File Offset: 0x001243A0
		private static bool TryCopyConstructedBitStringValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> dest, bool isIndefinite, out int unusedBitCount, out int bytesRead, out int bytesWritten)
		{
			int num = AsnDecoder.CountConstructedBitString(source, ruleSet, isIndefinite);
			if (ruleSet == AsnEncodingRules.CER && num < 1000)
			{
				throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			}
			if (dest.Length < num)
			{
				unusedBitCount = 0;
				bytesRead = 0;
				bytesWritten = 0;
				return false;
			}
			AsnDecoder.CopyConstructedBitString(source, ruleSet, dest, isIndefinite, out unusedBitCount, out bytesRead, out bytesWritten);
			return true;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x001253F8 File Offset: 0x001243F8
		private static bool TryReadPrimitiveBitStringCore(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, out int? contentsLength, out int headerLength, out int unusedBitCount, out ReadOnlySpan<byte> value, out int bytesConsumed, out byte normalizedLastByte)
		{
			Asn1Tag asn1Tag = AsnDecoder.ReadTagAndLength(source, ruleSet, out contentsLength, out headerLength);
			AsnDecoder.CheckExpectedTag(asn1Tag, expectedTag, UniversalTagNumber.BitString);
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.Slice(source, headerLength, contentsLength);
			if (!asn1Tag.IsConstructed)
			{
				AsnDecoder.ParsePrimitiveBitStringContents(readOnlySpan, ruleSet, out unusedBitCount, out value, out normalizedLastByte);
				bytesConsumed = headerLength + readOnlySpan.Length;
				return true;
			}
			if (ruleSet == AsnEncodingRules.DER)
			{
				throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER or CER encoding.");
			}
			unusedBitCount = 0;
			value = default(ReadOnlySpan<byte>);
			normalizedLastByte = 0;
			bytesConsumed = 0;
			return false;
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x00125474 File Offset: 0x00124474
		public static bool TryReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out Asn1Tag tag, out int contentOffset, out int contentLength, out int bytesConsumed)
		{
			AsnDecoder.CheckEncodingRules(ruleSet);
			Asn1Tag asn1Tag;
			int num;
			int? num2;
			int num3;
			if (Asn1Tag.TryDecode(source, out asn1Tag, out num) && AsnDecoder.TryReadLength(source.Slice(num), ruleSet, out num2, out num3))
			{
				int num4 = num + num3;
				int num5;
				int num6;
				AsnDecoder.LengthValidity lengthValidity = AsnDecoder.ValidateLength(source.Slice(num4), ruleSet, asn1Tag, num2, out num5, out num6);
				if (lengthValidity == AsnDecoder.LengthValidity.Valid)
				{
					tag = asn1Tag;
					contentOffset = num4;
					contentLength = num5;
					bytesConsumed = num4 + num6;
					return true;
				}
			}
			tag = default(Asn1Tag);
			contentOffset = (contentLength = (bytesConsumed = 0));
			return false;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x00125500 File Offset: 0x00124500
		public static Asn1Tag ReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed)
		{
			AsnDecoder.CheckEncodingRules(ruleSet);
			int num;
			Asn1Tag asn1Tag = Asn1Tag.Decode(source, out num);
			int num3;
			int? num2 = AsnDecoder.ReadLength(source.Slice(num), ruleSet, out num3);
			int num4 = num + num3;
			int num5;
			int num6;
			AsnDecoder.LengthValidity lengthValidity = AsnDecoder.ValidateLength(source.Slice(num4), ruleSet, asn1Tag, num2, out num5, out num6);
			if (lengthValidity == AsnDecoder.LengthValidity.Valid)
			{
				contentOffset = num4;
				contentLength = num5;
				bytesConsumed = num4 + num6;
				return asn1Tag;
			}
			throw AsnDecoder.GetValidityException(lengthValidity);
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x00125568 File Offset: 0x00124568
		private static ReadOnlySpan<byte> GetPrimitiveContentSpan(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, UniversalTagNumber tagNumber, out int bytesConsumed)
		{
			AsnDecoder.CheckEncodingRules(ruleSet);
			int num;
			Asn1Tag asn1Tag = Asn1Tag.Decode(source, out num);
			int num3;
			int? num2 = AsnDecoder.ReadLength(source.Slice(num), ruleSet, out num3);
			int num4 = num + num3;
			AsnDecoder.CheckExpectedTag(asn1Tag, expectedTag, tagNumber);
			if (asn1Tag.IsConstructed)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The encoded value uses a constructed encoding, which is invalid for '{0}' values.", new object[] { tagNumber }));
			}
			if (num2 == null)
			{
				throw new InvalidOperationException();
			}
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.Slice(source, num4, num2.Value);
			bytesConsumed = num4 + readOnlySpan.Length;
			return readOnlySpan;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x00125603 File Offset: 0x00124603
		private static bool TryReadLength(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int? length, out int bytesRead)
		{
			return AsnDecoder.DecodeLength(source, ruleSet, out length, out bytesRead) == AsnDecoder.LengthDecodeStatus.Success;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x00125614 File Offset: 0x00124614
		private static int? ReadLength(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed)
		{
			int? num;
			switch (AsnDecoder.DecodeLength(source, ruleSet, out num, out bytesConsumed))
			{
			case AsnDecoder.LengthDecodeStatus.DerIndefinite:
			case AsnDecoder.LengthDecodeStatus.LaxEncodingProhibited:
				throw new InvalidOperationException("The encoded length is not valid under the requested encoding rules, the value may be valid under the BER encoding.");
			case AsnDecoder.LengthDecodeStatus.LengthTooBig:
				throw new InvalidOperationException("The encoded length exceeds the maximum supported by this library (Int32.MaxValue).");
			case AsnDecoder.LengthDecodeStatus.Success:
				return num;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x0012566C File Offset: 0x0012466C
		private static AsnDecoder.LengthDecodeStatus DecodeLength(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int? length, out int bytesRead)
		{
			length = null;
			bytesRead = 0;
			if (source.IsEmpty)
			{
				return AsnDecoder.LengthDecodeStatus.NeedMoreData;
			}
			byte b = source[bytesRead];
			bytesRead++;
			if (b == 128)
			{
				if (ruleSet == AsnEncodingRules.DER)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.DerIndefinite;
				}
				return AsnDecoder.LengthDecodeStatus.Success;
			}
			else
			{
				if (b < 128)
				{
					length = new int?((int)b);
					return AsnDecoder.LengthDecodeStatus.Success;
				}
				if (b == 255)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.ReservedValue;
				}
				byte b2 = (byte)((int)b & -129);
				if ((int)(b2 + 1) > source.Length)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.NeedMoreData;
				}
				bool flag = ruleSet == AsnEncodingRules.DER || ruleSet == AsnEncodingRules.CER;
				if (flag && b2 > 4)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.LengthTooBig;
				}
				uint num = 0U;
				for (int i = 0; i < (int)b2; i++)
				{
					byte b3 = source[bytesRead];
					bytesRead++;
					if (num == 0U)
					{
						if (flag && b3 == 0)
						{
							bytesRead = 0;
							return AsnDecoder.LengthDecodeStatus.LaxEncodingProhibited;
						}
						if (!flag && b3 != 0 && (int)b2 - i > 4)
						{
							bytesRead = 0;
							return AsnDecoder.LengthDecodeStatus.LengthTooBig;
						}
					}
					num <<= 8;
					num |= (uint)b3;
				}
				if (num > 2147483647U)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.LengthTooBig;
				}
				if (flag && num < 128U)
				{
					bytesRead = 0;
					return AsnDecoder.LengthDecodeStatus.LaxEncodingProhibited;
				}
				length = new int?((int)num);
				return AsnDecoder.LengthDecodeStatus.Success;
			}
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x00125780 File Offset: 0x00124780
		private static Asn1Tag ReadTagAndLength(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int? contentsLength, out int bytesRead)
		{
			int num;
			Asn1Tag asn1Tag = Asn1Tag.Decode(source, out num);
			int num3;
			int? num2 = AsnDecoder.ReadLength(source.Slice(num), ruleSet, out num3);
			int num4 = num + num3;
			if (asn1Tag.IsConstructed)
			{
				if (ruleSet == AsnEncodingRules.CER && num2 != null)
				{
					throw AsnDecoder.GetValidityException(AsnDecoder.LengthValidity.CerRequiresIndefinite);
				}
			}
			else if (num2 == null)
			{
				throw AsnDecoder.GetValidityException(AsnDecoder.LengthValidity.PrimitiveEncodingRequiresDefinite);
			}
			bytesRead = num4;
			contentsLength = num2;
			return asn1Tag;
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001257E8 File Offset: 0x001247E8
		private static void ValidateEndOfContents(Asn1Tag tag, int? length, int headerLength)
		{
			if (tag.IsConstructed || length != 0 || headerLength != 2)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x00125824 File Offset: 0x00124824
		private static AsnDecoder.LengthValidity ValidateLength(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag localTag, int? encodedLength, out int actualLength, out int bytesConsumed)
		{
			if (localTag.IsConstructed)
			{
				if (ruleSet == AsnEncodingRules.CER && encodedLength != null)
				{
					actualLength = (bytesConsumed = 0);
					return AsnDecoder.LengthValidity.CerRequiresIndefinite;
				}
			}
			else if (encodedLength == null)
			{
				actualLength = (bytesConsumed = 0);
				return AsnDecoder.LengthValidity.PrimitiveEncodingRequiresDefinite;
			}
			if (encodedLength == null)
			{
				actualLength = AsnDecoder.SeekEndOfContents(source, ruleSet);
				bytesConsumed = actualLength + 2;
				return AsnDecoder.LengthValidity.Valid;
			}
			int value = encodedLength.Value;
			int num = value;
			if (num > source.Length)
			{
				actualLength = (bytesConsumed = 0);
				return AsnDecoder.LengthValidity.LengthExceedsInput;
			}
			actualLength = value;
			bytesConsumed = value;
			return AsnDecoder.LengthValidity.Valid;
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001258B0 File Offset: 0x001248B0
		private static InvalidOperationException GetValidityException(AsnDecoder.LengthValidity validity)
		{
			switch (validity)
			{
			case AsnDecoder.LengthValidity.CerRequiresIndefinite:
				return new InvalidOperationException("A constructed tag used a definite length encoding, which is invalid for CER data. The input may be encoded with BER or DER.");
			case AsnDecoder.LengthValidity.LengthExceedsInput:
				return new InvalidOperationException("The encoded length exceeds the number of bytes remaining in the input buffer.");
			}
			return new InvalidOperationException();
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001258F0 File Offset: 0x001248F0
		private static int GetPrimitiveIntegerSize(Type primitiveType)
		{
			if (primitiveType == typeof(byte) || primitiveType == typeof(sbyte))
			{
				return 1;
			}
			if (primitiveType == typeof(short) || primitiveType == typeof(ushort))
			{
				return 2;
			}
			if (primitiveType == typeof(int) || primitiveType == typeof(uint))
			{
				return 4;
			}
			if (primitiveType == typeof(long) || primitiveType == typeof(ulong))
			{
				return 8;
			}
			return 0;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x00125970 File Offset: 0x00124970
		private static int SeekEndOfContents(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet)
		{
			ReadOnlySpan<byte> readOnlySpan = source;
			int num = 0;
			int num2 = 1;
			while (!readOnlySpan.IsEmpty)
			{
				int? num3;
				int num4;
				Asn1Tag asn1Tag = AsnDecoder.ReadTagAndLength(readOnlySpan, ruleSet, out num3, out num4);
				if (asn1Tag == Asn1Tag.EndOfContents)
				{
					AsnDecoder.ValidateEndOfContents(asn1Tag, num3, num4);
					num2--;
					if (num2 == 0)
					{
						return num;
					}
				}
				if (num3 == null)
				{
					num2++;
					readOnlySpan = readOnlySpan.Slice(num4);
					num += num4;
				}
				else
				{
					ReadOnlySpan<byte> readOnlySpan2 = AsnDecoder.Slice(readOnlySpan, 0, num4 + num3.Value);
					readOnlySpan = readOnlySpan.Slice(readOnlySpan2.Length);
					num += readOnlySpan2.Length;
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x00125A10 File Offset: 0x00124A10
		private static ReadOnlySpan<byte> SliceAtMost(ReadOnlySpan<byte> source, int longestPermitted)
		{
			int num = Math.Min(longestPermitted, source.Length);
			return source.Slice(0, num);
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x00125A34 File Offset: 0x00124A34
		private static ReadOnlySpan<byte> Slice(ReadOnlySpan<byte> source, int offset, int length)
		{
			if (length < 0 || source.Length - offset < length)
			{
				throw new InvalidOperationException("The encoded length exceeds the number of bytes remaining in the input buffer.");
			}
			return source.Slice(offset, length);
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x00125A5C File Offset: 0x00124A5C
		private static ReadOnlySpan<byte> Slice(ReadOnlySpan<byte> source, int offset, int? length)
		{
			if (length == null)
			{
				return source.Slice(offset);
			}
			int value = length.Value;
			if (value < 0 || source.Length - offset < value)
			{
				throw new InvalidOperationException("The encoded length exceeds the number of bytes remaining in the input buffer.");
			}
			return source.Slice(offset, value);
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x00125AA8 File Offset: 0x00124AA8
		internal static ReadOnlyMemory<byte> Slice(ReadOnlyMemory<byte> bigger, ReadOnlySpan<byte> smaller)
		{
			if (smaller.IsEmpty)
			{
				return default(ReadOnlyMemory<byte>);
			}
			int num;
			if (bigger.Span.Overlaps(smaller, out num))
			{
				return bigger.Slice(num, smaller.Length);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x00125AF1 File Offset: 0x00124AF1
		[Conditional("DEBUG")]
		private static void AssertEncodingRules(AsnEncodingRules ruleSet)
		{
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x00125AF3 File Offset: 0x00124AF3
		internal static void CheckEncodingRules(AsnEncodingRules ruleSet)
		{
			if (ruleSet != AsnEncodingRules.BER && ruleSet != AsnEncodingRules.CER && ruleSet != AsnEncodingRules.DER)
			{
				throw new ArgumentOutOfRangeException("ruleSet");
			}
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x00125B0C File Offset: 0x00124B0C
		private static void CheckExpectedTag(Asn1Tag tag, Asn1Tag expectedTag, UniversalTagNumber tagNumber)
		{
			if (expectedTag.TagClass == TagClass.Universal && expectedTag.TagValue != (int)tagNumber)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Tags with TagClass Universal must have the appropriate TagValue value for the data type being read or written.", new object[] { expectedTag }));
			}
			if (expectedTag.TagClass != tag.TagClass || expectedTag.TagValue != tag.TagValue)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The provided data is tagged with '{0}' class value '{1}', but it should have been '{2}' class value '{3}'.", new object[] { tag.TagClass, tag.TagValue, expectedTag.TagClass, expectedTag.TagValue }));
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00125BCC File Offset: 0x00124BCC
		public static ReadOnlySpan<byte> ReadIntegerBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			return AsnDecoder.GetIntegerContents(source, ruleSet, expectedTag ?? Asn1Tag.Integer, UniversalTagNumber.Integer, out bytesConsumed);
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x00125BFC File Offset: 0x00124BFC
		public static bool TryReadInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			long num;
			if (AsnDecoder.TryReadSignedInteger(source, ruleSet, 4, expectedTag ?? Asn1Tag.Integer, UniversalTagNumber.Integer, out num, out bytesConsumed))
			{
				value = (int)num;
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x00125C3C File Offset: 0x00124C3C
		public static bool TryReadUInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out uint value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			ulong num;
			if (AsnDecoder.TryReadUnsignedInteger(source, ruleSet, 4, expectedTag ?? Asn1Tag.Integer, UniversalTagNumber.Integer, out num, out bytesConsumed))
			{
				value = (uint)num;
				return true;
			}
			value = 0U;
			return false;
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x00125C7C File Offset: 0x00124C7C
		public static bool TryReadInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out long value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			return AsnDecoder.TryReadSignedInteger(source, ruleSet, 8, expectedTag ?? Asn1Tag.Integer, UniversalTagNumber.Integer, out value, out bytesConsumed);
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x00125CB0 File Offset: 0x00124CB0
		public static bool TryReadUInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ulong value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			return AsnDecoder.TryReadUnsignedInteger(source, ruleSet, 8, expectedTag ?? Asn1Tag.Integer, UniversalTagNumber.Integer, out value, out bytesConsumed);
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00125CE4 File Offset: 0x00124CE4
		private static ReadOnlySpan<byte> GetIntegerContents(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, UniversalTagNumber tagNumber, out int bytesConsumed)
		{
			int num;
			ReadOnlySpan<byte> primitiveContentSpan = AsnDecoder.GetPrimitiveContentSpan(source, ruleSet, expectedTag, tagNumber, out num);
			if (primitiveContentSpan.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			ushort num2;
			if (BinaryPrimitives.TryReadUInt16BigEndian(primitiveContentSpan, out num2))
			{
				ushort num3 = num2 & 65408;
				if (num3 == 0 || num3 == 65408)
				{
					throw new InvalidOperationException();
				}
			}
			bytesConsumed = num;
			return primitiveContentSpan;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x00125D38 File Offset: 0x00124D38
		private static bool TryReadSignedInteger(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, int sizeLimit, Asn1Tag expectedTag, UniversalTagNumber tagNumber, out long value, out int bytesConsumed)
		{
			int num;
			ReadOnlySpan<byte> integerContents = AsnDecoder.GetIntegerContents(source, ruleSet, expectedTag, tagNumber, out num);
			if (integerContents.Length > sizeLimit)
			{
				value = 0L;
				bytesConsumed = 0;
				return false;
			}
			long num2 = (((integerContents[0] & 128) != 0) ? (-1L) : 0L);
			for (int i = 0; i < integerContents.Length; i++)
			{
				num2 <<= 8;
				num2 |= (long)((ulong)integerContents[i]);
			}
			bytesConsumed = num;
			value = num2;
			return true;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x00125DB8 File Offset: 0x00124DB8
		private static bool TryReadUnsignedInteger(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, int sizeLimit, Asn1Tag expectedTag, UniversalTagNumber tagNumber, out ulong value, out int bytesConsumed)
		{
			int num;
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.GetIntegerContents(source, ruleSet, expectedTag, tagNumber, out num);
			bool flag = (readOnlySpan[0] & 128) != 0;
			if (flag)
			{
				bytesConsumed = 0;
				value = 0UL;
				return false;
			}
			if (readOnlySpan.Length > 1 && readOnlySpan[0] == 0)
			{
				readOnlySpan = readOnlySpan.Slice(1);
			}
			if (readOnlySpan.Length > sizeLimit)
			{
				bytesConsumed = 0;
				value = 0UL;
				return false;
			}
			ulong num2 = 0UL;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				num2 <<= 8;
				num2 |= (ulong)readOnlySpan[i];
			}
			bytesConsumed = num;
			value = num2;
			return true;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x00125E5C File Offset: 0x00124E5C
		public static void ReadNull(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int num;
			if (AsnDecoder.GetPrimitiveContentSpan(source, ruleSet, expectedTag ?? Asn1Tag.Null, UniversalTagNumber.Null, out num).Length != 0)
			{
				throw new InvalidOperationException();
			}
			bytesConsumed = num;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00125EA0 File Offset: 0x00124EA0
		public static bool TryReadOctetString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag)
		{
			if (source.Overlaps(destination))
			{
				throw new ArgumentException("The destination buffer overlaps the source buffer.", "destination");
			}
			int? num;
			int num2;
			ReadOnlySpan<byte> readOnlySpan;
			int num3;
			if (!AsnDecoder.TryReadPrimitiveOctetStringCore(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveOctetString, UniversalTagNumber.OctetString, out num, out num2, out readOnlySpan, out num3))
			{
				int num4;
				bool flag = AsnDecoder.TryCopyConstructedOctetStringContents(AsnDecoder.Slice(source, num2, num), ruleSet, destination, num == null, out num4, out bytesWritten);
				if (flag)
				{
					bytesConsumed = num2 + num4;
				}
				else
				{
					bytesConsumed = 0;
				}
				return flag;
			}
			if (readOnlySpan.Length > destination.Length)
			{
				bytesWritten = 0;
				bytesConsumed = 0;
				return false;
			}
			readOnlySpan.CopyTo(destination);
			bytesWritten = readOnlySpan.Length;
			bytesConsumed = num3;
			return true;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00125F5C File Offset: 0x00124F5C
		public static byte[] ReadOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			byte[] array = null;
			int num;
			ReadOnlySpan<byte> octetStringContents = AsnDecoder.GetOctetStringContents(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveOctetString, UniversalTagNumber.OctetString, out num, ref array, default(Span<byte>));
			byte[] array2 = octetStringContents.ToArray();
			if (array != null)
			{
				CryptoPool.Return(array, octetStringContents.Length);
			}
			bytesConsumed = num;
			return array2;
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00125FB8 File Offset: 0x00124FB8
		private static bool TryReadPrimitiveOctetStringCore(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, UniversalTagNumber universalTagNumber, out int? contentLength, out int headerLength, out ReadOnlySpan<byte> contents, out int bytesConsumed)
		{
			Asn1Tag asn1Tag = AsnDecoder.ReadTagAndLength(source, ruleSet, out contentLength, out headerLength);
			AsnDecoder.CheckExpectedTag(asn1Tag, expectedTag, universalTagNumber);
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.Slice(source, headerLength, contentLength);
			if (asn1Tag.IsConstructed)
			{
				if (ruleSet == AsnEncodingRules.DER)
				{
					throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER or CER encoding.");
				}
				contents = default(ReadOnlySpan<byte>);
				bytesConsumed = 0;
				return false;
			}
			else
			{
				if (ruleSet == AsnEncodingRules.CER && readOnlySpan.Length > 1000)
				{
					throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER or DER encoding.");
				}
				contents = readOnlySpan;
				bytesConsumed = headerLength + readOnlySpan.Length;
				return true;
			}
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x00126044 File Offset: 0x00125044
		public static bool TryReadPrimitiveOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int? num;
			int num2;
			return AsnDecoder.TryReadPrimitiveOctetStringCore(source, ruleSet, expectedTag ?? Asn1Tag.PrimitiveOctetString, UniversalTagNumber.OctetString, out num, out num2, out value, out bytesConsumed);
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x0012607C File Offset: 0x0012507C
		private static int CountConstructedOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, bool isIndefinite)
		{
			int num2;
			int num = AsnDecoder.CopyConstructedOctetString(source, ruleSet, Span<byte>.Empty, false, isIndefinite, out num2);
			if (ruleSet == AsnEncodingRules.CER && num <= 1000)
			{
				throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			}
			return num;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001260B2 File Offset: 0x001250B2
		private static void CopyConstructedOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> destination, bool isIndefinite, out int bytesRead, out int bytesWritten)
		{
			bytesWritten = AsnDecoder.CopyConstructedOctetString(source, ruleSet, destination, true, isIndefinite, out bytesRead);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001260C4 File Offset: 0x001250C4
		private static int CopyConstructedOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> destination, bool write, bool isIndefinite, out int bytesRead)
		{
			bytesRead = 0;
			int num = 1000;
			ReadOnlySpan<byte> readOnlySpan = source;
			Stack stack = null;
			int num2 = 0;
			Asn1Tag asn1Tag = Asn1Tag.ConstructedBitString;
			Span<byte> span = destination;
			for (;;)
			{
				if (!readOnlySpan.IsEmpty)
				{
					int? num3;
					int num4;
					asn1Tag = AsnDecoder.ReadTagAndLength(readOnlySpan, ruleSet, out num3, out num4);
					if (asn1Tag == Asn1Tag.PrimitiveOctetString)
					{
						if (ruleSet == AsnEncodingRules.CER && num != 1000)
						{
							break;
						}
						ReadOnlySpan<byte> readOnlySpan2 = AsnDecoder.Slice(readOnlySpan, num4, num3.Value);
						int num5 = num4 + readOnlySpan2.Length;
						readOnlySpan = readOnlySpan.Slice(num5);
						bytesRead += num5;
						num2 += readOnlySpan2.Length;
						num = readOnlySpan2.Length;
						if (ruleSet == AsnEncodingRules.CER && num > 1000)
						{
							goto Block_5;
						}
						if (write)
						{
							readOnlySpan2.CopyTo(span);
							span = span.Slice(readOnlySpan2.Length);
							continue;
						}
						continue;
					}
					else if (asn1Tag == Asn1Tag.EndOfContents && isIndefinite)
					{
						AsnDecoder.ValidateEndOfContents(asn1Tag, num3, num4);
						bytesRead += num4;
						if (stack != null && stack.Count > 0)
						{
							AsnDecoder.ParseFrame parseFrame = (AsnDecoder.ParseFrame)stack.Pop();
							readOnlySpan = source.Slice(parseFrame.Offset, parseFrame.Length).Slice(bytesRead);
							bytesRead += parseFrame.BytesRead;
							isIndefinite = parseFrame.Indefinite;
							continue;
						}
					}
					else
					{
						if (!(asn1Tag == Asn1Tag.ConstructedOctetString))
						{
							goto IL_01C2;
						}
						if (ruleSet == AsnEncodingRules.CER)
						{
							goto Block_12;
						}
						if (stack == null)
						{
							stack = new Stack();
						}
						int num6;
						if (!source.Overlaps(readOnlySpan, out num6))
						{
							goto Block_14;
						}
						stack.Push(new AsnDecoder.ParseFrame(num6, readOnlySpan.Length, isIndefinite, bytesRead));
						readOnlySpan = AsnDecoder.Slice(readOnlySpan, num4, num3);
						bytesRead = num4;
						isIndefinite = num3 == null;
						continue;
					}
				}
				if (isIndefinite && asn1Tag != Asn1Tag.EndOfContents)
				{
					goto Block_16;
				}
				if (stack == null || stack.Count <= 0)
				{
					return num2;
				}
				AsnDecoder.ParseFrame parseFrame2 = (AsnDecoder.ParseFrame)stack.Pop();
				readOnlySpan = source.Slice(parseFrame2.Offset, parseFrame2.Length).Slice(bytesRead);
				isIndefinite = parseFrame2.Indefinite;
				bytesRead += parseFrame2.BytesRead;
			}
			throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			Block_5:
			throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			Block_12:
			throw new InvalidOperationException("The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.");
			Block_14:
			throw new InvalidOperationException();
			IL_01C2:
			throw new InvalidOperationException();
			Block_16:
			throw new InvalidOperationException();
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x00126314 File Offset: 0x00125314
		private static bool TryCopyConstructedOctetStringContents(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Span<byte> dest, bool isIndefinite, out int bytesRead, out int bytesWritten)
		{
			bytesRead = 0;
			int num = AsnDecoder.CountConstructedOctetString(source, ruleSet, isIndefinite);
			if (dest.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			AsnDecoder.CopyConstructedOctetString(source, ruleSet, dest, isIndefinite, out bytesRead, out bytesWritten);
			return true;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x0012634C File Offset: 0x0012534C
		private static ReadOnlySpan<byte> GetOctetStringContents(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, UniversalTagNumber universalTagNumber, out int bytesConsumed, ref byte[] rented, Span<byte> tmpSpace)
		{
			int? num;
			int num2;
			ReadOnlySpan<byte> readOnlySpan;
			if (AsnDecoder.TryReadPrimitiveOctetStringCore(source, ruleSet, expectedTag, universalTagNumber, out num, out num2, out readOnlySpan, out bytesConsumed))
			{
				return readOnlySpan;
			}
			readOnlySpan = source.Slice(num2);
			int num3 = num ?? AsnDecoder.SeekEndOfContents(readOnlySpan, ruleSet);
			if (tmpSpace.Length > 0 && num3 > tmpSpace.Length)
			{
				bool flag = num == null;
				num3 = AsnDecoder.CountConstructedOctetString(readOnlySpan, ruleSet, flag);
			}
			if (num3 > tmpSpace.Length)
			{
				rented = CryptoPool.Rent(num3);
				tmpSpace = rented;
			}
			int num4;
			int num5;
			if (AsnDecoder.TryCopyConstructedOctetStringContents(AsnDecoder.Slice(source, num2, num), ruleSet, tmpSpace, num == null, out num4, out num5))
			{
				bytesConsumed = num2 + num4;
				return tmpSpace.Slice(0, num5);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x00126418 File Offset: 0x00125418
		public static byte[] ReadObjectIdentifier(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int num;
			ReadOnlySpan<byte> primitiveContentSpan = AsnDecoder.GetPrimitiveContentSpan(source, ruleSet, expectedTag ?? Asn1Tag.ObjectIdentifier, UniversalTagNumber.ObjectIdentifier, out num);
			bytesConsumed = num;
			return primitiveContentSpan.ToArray();
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00126454 File Offset: 0x00125454
		public static void ReadSequence(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, Asn1Tag? expectedTag)
		{
			int? num;
			int num2;
			Asn1Tag asn1Tag = AsnDecoder.ReadTagAndLength(source, ruleSet, out num, out num2);
			AsnDecoder.CheckExpectedTag(asn1Tag, expectedTag ?? Asn1Tag.Sequence, UniversalTagNumber.Sequence);
			if (!asn1Tag.IsConstructed)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The encoded value uses a primitive encoding, which is invalid for '{0}' values.", new object[] { UniversalTagNumber.Sequence }));
			}
			if (num == null)
			{
				int num3 = AsnDecoder.SeekEndOfContents(source.Slice(num2), ruleSet);
				contentLength = num3;
				contentOffset = num2;
				bytesConsumed = num3 + num2 + 2;
				return;
			}
			if (num.Value + num2 > source.Length)
			{
				throw AsnDecoder.GetValidityException(AsnDecoder.LengthValidity.LengthExceedsInput);
			}
			contentLength = num.Value;
			contentOffset = num2;
			bytesConsumed = contentLength + num2;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00126518 File Offset: 0x00125518
		public static void ReadSetOf(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, bool skipSortOrderValidation, Asn1Tag? expectedTag)
		{
			int? num;
			int num2;
			Asn1Tag asn1Tag = AsnDecoder.ReadTagAndLength(source, ruleSet, out num, out num2);
			AsnDecoder.CheckExpectedTag(asn1Tag, expectedTag ?? Asn1Tag.SetOf, UniversalTagNumber.Set);
			if (!asn1Tag.IsConstructed)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The encoded value uses a primitive encoding, which is invalid for '{0}' values.", new object[] { UniversalTagNumber.Set }));
			}
			int num3;
			ReadOnlySpan<byte> readOnlySpan;
			if (num != null)
			{
				num3 = 0;
				readOnlySpan = AsnDecoder.Slice(source, num2, num.Value);
			}
			else
			{
				int num4 = AsnDecoder.SeekEndOfContents(source.Slice(num2), ruleSet);
				readOnlySpan = AsnDecoder.Slice(source, num2, num4);
				num3 = 2;
			}
			if (!skipSortOrderValidation && (ruleSet == AsnEncodingRules.DER || ruleSet == AsnEncodingRules.CER))
			{
				ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan;
				ReadOnlySpan<byte> readOnlySpan3 = default(ReadOnlySpan<byte>);
				while (!readOnlySpan2.IsEmpty)
				{
					int num5;
					int num6;
					int num7;
					AsnDecoder.ReadEncodedValue(readOnlySpan2, ruleSet, out num5, out num6, out num7);
					ReadOnlySpan<byte> readOnlySpan4 = readOnlySpan2.Slice(0, num7);
					readOnlySpan2 = readOnlySpan2.Slice(num7);
					if (SetOfValueComparer.Compare(readOnlySpan4, readOnlySpan3) < 0)
					{
						throw new InvalidOperationException("The encoded set is not sorted as required by the current encoding rules. The value may be valid under the BER encoding, or you can ignore the sort validation by specifying skipSortValidation=true.");
					}
					readOnlySpan3 = readOnlySpan4;
				}
			}
			contentOffset = num2;
			contentLength = readOnlySpan.Length;
			bytesConsumed = num2 + readOnlySpan.Length + num3;
		}

		// Token: 0x040029E7 RID: 10727
		internal const int MaxCERSegmentSize = 1000;

		// Token: 0x040029E8 RID: 10728
		internal const int EndOfContentsEncodedLength = 2;

		// Token: 0x020008AF RID: 2223
		// (Invoke) Token: 0x060051B2 RID: 20914
		private delegate void BitStringCopyAction(ReadOnlySpan<byte> value, byte normalizedLastByte, Span<byte> destination);

		// Token: 0x020008B0 RID: 2224
		private struct ParseFrame
		{
			// Token: 0x060051B5 RID: 20917 RVA: 0x0012663F File Offset: 0x0012563F
			public ParseFrame(int offset, int length, bool indefinite, int bytesRead)
			{
				this._offset = offset;
				this._length = length;
				this._indefinite = indefinite;
				this._bytesRead = bytesRead;
			}

			// Token: 0x17000E2E RID: 3630
			// (get) Token: 0x060051B6 RID: 20918 RVA: 0x0012665E File Offset: 0x0012565E
			public int Offset
			{
				get
				{
					return this._offset;
				}
			}

			// Token: 0x17000E2F RID: 3631
			// (get) Token: 0x060051B7 RID: 20919 RVA: 0x00126666 File Offset: 0x00125666
			public int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17000E30 RID: 3632
			// (get) Token: 0x060051B8 RID: 20920 RVA: 0x0012666E File Offset: 0x0012566E
			public bool Indefinite
			{
				get
				{
					return this._indefinite;
				}
			}

			// Token: 0x17000E31 RID: 3633
			// (get) Token: 0x060051B9 RID: 20921 RVA: 0x00126676 File Offset: 0x00125676
			public int BytesRead
			{
				get
				{
					return this._bytesRead;
				}
			}

			// Token: 0x040029E9 RID: 10729
			private int _offset;

			// Token: 0x040029EA RID: 10730
			private int _length;

			// Token: 0x040029EB RID: 10731
			private bool _indefinite;

			// Token: 0x040029EC RID: 10732
			private int _bytesRead;
		}

		// Token: 0x020008B1 RID: 2225
		private enum LengthDecodeStatus
		{
			// Token: 0x040029EE RID: 10734
			NeedMoreData,
			// Token: 0x040029EF RID: 10735
			DerIndefinite,
			// Token: 0x040029F0 RID: 10736
			ReservedValue,
			// Token: 0x040029F1 RID: 10737
			LengthTooBig,
			// Token: 0x040029F2 RID: 10738
			LaxEncodingProhibited,
			// Token: 0x040029F3 RID: 10739
			Success
		}

		// Token: 0x020008B2 RID: 2226
		private enum LengthValidity
		{
			// Token: 0x040029F5 RID: 10741
			CerRequiresIndefinite,
			// Token: 0x040029F6 RID: 10742
			PrimitiveEncodingRequiresDefinite,
			// Token: 0x040029F7 RID: 10743
			LengthExceedsInput,
			// Token: 0x040029F8 RID: 10744
			Valid
		}
	}
}

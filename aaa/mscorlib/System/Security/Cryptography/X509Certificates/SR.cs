using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D7 RID: 2263
	internal static class SR
	{
		// Token: 0x04002A67 RID: 10855
		internal const string Argument_InvalidValue = "Value was invalid.";

		// Token: 0x04002A68 RID: 10856
		internal const string Argument_SourceOverlapsDestination = "The destination buffer overlaps the source buffer.";

		// Token: 0x04002A69 RID: 10857
		internal const string Argument_UniversalValueIsFixed = "Tags with TagClass Universal must have the appropriate TagValue value for the data type being read or written.";

		// Token: 0x04002A6A RID: 10858
		internal const string BCryptAlgorithmHandle_ProviderNotFound = "A provider could not be found for algorithm '{0}'.";

		// Token: 0x04002A6B RID: 10859
		internal const string BCryptDeriveKeyPBKDF2_Failed = "A call to BCryptDeriveKeyPBKDF2 failed with code '{0}'.";

		// Token: 0x04002A6C RID: 10860
		internal const string ContentException_CerRequiresIndefiniteLength = "A constructed tag used a definite length encoding, which is invalid for CER data. The input may be encoded with BER or DER.";

		// Token: 0x04002A6D RID: 10861
		internal const string ContentException_ConstructedEncodingRequired = "The encoded value uses a primitive encoding, which is invalid for '{0}' values.";

		// Token: 0x04002A6E RID: 10862
		internal const string ContentException_DefaultMessage = "The ASN.1 value is invalid.";

		// Token: 0x04002A6F RID: 10863
		internal const string ContentException_InvalidTag = "The provided data does not represent a valid tag.";

		// Token: 0x04002A70 RID: 10864
		internal const string ContentException_InvalidUnderCerOrDer_TryBer = "The encoded value is not valid under the selected encoding, but it may be valid under the BER encoding.";

		// Token: 0x04002A71 RID: 10865
		internal const string ContentException_InvalidUnderCer_TryBerOrDer = "The encoded value is not valid under the selected encoding, but it may be valid under the BER or DER encoding.";

		// Token: 0x04002A72 RID: 10866
		internal const string ContentException_InvalidUnderDer_TryBerOrCer = "The encoded value is not valid under the selected encoding, but it may be valid under the BER or CER encoding.";

		// Token: 0x04002A73 RID: 10867
		internal const string ContentException_LengthExceedsPayload = "The encoded length exceeds the number of bytes remaining in the input buffer.";

		// Token: 0x04002A74 RID: 10868
		internal const string ContentException_LengthRuleSetConstraint = "The encoded length is not valid under the requested encoding rules, the value may be valid under the BER encoding.";

		// Token: 0x04002A75 RID: 10869
		internal const string ContentException_LengthTooBig = "The encoded length exceeds the maximum supported by this library (Int32.MaxValue).";

		// Token: 0x04002A76 RID: 10870
		internal const string ContentException_PrimitiveEncodingRequired = "The encoded value uses a constructed encoding, which is invalid for '{0}' values.";

		// Token: 0x04002A77 RID: 10871
		internal const string ContentException_SetOfNotSorted = "The encoded set is not sorted as required by the current encoding rules. The value may be valid under the BER encoding, or you can ignore the sort validation by specifying skipSortValidation=true.";

		// Token: 0x04002A78 RID: 10872
		internal const string ContentException_TooMuchData = "The last expected value has been read, but the reader still has pending data. This value may be from a newer schema, or is corrupt.";

		// Token: 0x04002A79 RID: 10873
		internal const string ContentException_WrongTag = "The provided data is tagged with '{0}' class value '{1}', but it should have been '{2}' class value '{3}'.";

		// Token: 0x04002A7A RID: 10874
		internal const string Cryptography_AlgKdfRequiresChars = "The KDF requires a char-based password input.";

		// Token: 0x04002A7B RID: 10875
		internal const string Cryptography_Der_Invalid_Encoding = "ASN1 corrupted data.";

		// Token: 0x04002A7C RID: 10876
		internal const string Cryptography_UnknownAlgorithmIdentifier = "The algorithm is unknown, not valid for the requested usage, or was not handled.";

		// Token: 0x04002A7D RID: 10877
		internal const string Cryptography_UnknownHashAlgorithm = "'{0}' is not a known hash algorithm.";
	}
}

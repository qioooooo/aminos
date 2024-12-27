using System;

namespace System.Drawing.Printing
{
	// Token: 0x0200010F RID: 271
	[Serializable]
	public enum PaperKind
	{
		// Token: 0x04000B9D RID: 2973
		Custom,
		// Token: 0x04000B9E RID: 2974
		Letter,
		// Token: 0x04000B9F RID: 2975
		Legal = 5,
		// Token: 0x04000BA0 RID: 2976
		A4 = 9,
		// Token: 0x04000BA1 RID: 2977
		CSheet = 24,
		// Token: 0x04000BA2 RID: 2978
		DSheet,
		// Token: 0x04000BA3 RID: 2979
		ESheet,
		// Token: 0x04000BA4 RID: 2980
		LetterSmall = 2,
		// Token: 0x04000BA5 RID: 2981
		Tabloid,
		// Token: 0x04000BA6 RID: 2982
		Ledger,
		// Token: 0x04000BA7 RID: 2983
		Statement = 6,
		// Token: 0x04000BA8 RID: 2984
		Executive,
		// Token: 0x04000BA9 RID: 2985
		A3,
		// Token: 0x04000BAA RID: 2986
		A4Small = 10,
		// Token: 0x04000BAB RID: 2987
		A5,
		// Token: 0x04000BAC RID: 2988
		B4,
		// Token: 0x04000BAD RID: 2989
		B5,
		// Token: 0x04000BAE RID: 2990
		Folio,
		// Token: 0x04000BAF RID: 2991
		Quarto,
		// Token: 0x04000BB0 RID: 2992
		Standard10x14,
		// Token: 0x04000BB1 RID: 2993
		Standard11x17,
		// Token: 0x04000BB2 RID: 2994
		Note,
		// Token: 0x04000BB3 RID: 2995
		Number9Envelope,
		// Token: 0x04000BB4 RID: 2996
		Number10Envelope,
		// Token: 0x04000BB5 RID: 2997
		Number11Envelope,
		// Token: 0x04000BB6 RID: 2998
		Number12Envelope,
		// Token: 0x04000BB7 RID: 2999
		Number14Envelope,
		// Token: 0x04000BB8 RID: 3000
		DLEnvelope = 27,
		// Token: 0x04000BB9 RID: 3001
		C5Envelope,
		// Token: 0x04000BBA RID: 3002
		C3Envelope,
		// Token: 0x04000BBB RID: 3003
		C4Envelope,
		// Token: 0x04000BBC RID: 3004
		C6Envelope,
		// Token: 0x04000BBD RID: 3005
		C65Envelope,
		// Token: 0x04000BBE RID: 3006
		B4Envelope,
		// Token: 0x04000BBF RID: 3007
		B5Envelope,
		// Token: 0x04000BC0 RID: 3008
		B6Envelope,
		// Token: 0x04000BC1 RID: 3009
		ItalyEnvelope,
		// Token: 0x04000BC2 RID: 3010
		MonarchEnvelope,
		// Token: 0x04000BC3 RID: 3011
		PersonalEnvelope,
		// Token: 0x04000BC4 RID: 3012
		USStandardFanfold,
		// Token: 0x04000BC5 RID: 3013
		GermanStandardFanfold,
		// Token: 0x04000BC6 RID: 3014
		GermanLegalFanfold,
		// Token: 0x04000BC7 RID: 3015
		IsoB4,
		// Token: 0x04000BC8 RID: 3016
		JapanesePostcard,
		// Token: 0x04000BC9 RID: 3017
		Standard9x11,
		// Token: 0x04000BCA RID: 3018
		Standard10x11,
		// Token: 0x04000BCB RID: 3019
		Standard15x11,
		// Token: 0x04000BCC RID: 3020
		InviteEnvelope,
		// Token: 0x04000BCD RID: 3021
		LetterExtra = 50,
		// Token: 0x04000BCE RID: 3022
		LegalExtra,
		// Token: 0x04000BCF RID: 3023
		TabloidExtra,
		// Token: 0x04000BD0 RID: 3024
		A4Extra,
		// Token: 0x04000BD1 RID: 3025
		LetterTransverse,
		// Token: 0x04000BD2 RID: 3026
		A4Transverse,
		// Token: 0x04000BD3 RID: 3027
		LetterExtraTransverse,
		// Token: 0x04000BD4 RID: 3028
		APlus,
		// Token: 0x04000BD5 RID: 3029
		BPlus,
		// Token: 0x04000BD6 RID: 3030
		LetterPlus,
		// Token: 0x04000BD7 RID: 3031
		A4Plus,
		// Token: 0x04000BD8 RID: 3032
		A5Transverse,
		// Token: 0x04000BD9 RID: 3033
		B5Transverse,
		// Token: 0x04000BDA RID: 3034
		A3Extra,
		// Token: 0x04000BDB RID: 3035
		A5Extra,
		// Token: 0x04000BDC RID: 3036
		B5Extra,
		// Token: 0x04000BDD RID: 3037
		A2,
		// Token: 0x04000BDE RID: 3038
		A3Transverse,
		// Token: 0x04000BDF RID: 3039
		A3ExtraTransverse,
		// Token: 0x04000BE0 RID: 3040
		JapaneseDoublePostcard,
		// Token: 0x04000BE1 RID: 3041
		A6,
		// Token: 0x04000BE2 RID: 3042
		JapaneseEnvelopeKakuNumber2,
		// Token: 0x04000BE3 RID: 3043
		JapaneseEnvelopeKakuNumber3,
		// Token: 0x04000BE4 RID: 3044
		JapaneseEnvelopeChouNumber3,
		// Token: 0x04000BE5 RID: 3045
		JapaneseEnvelopeChouNumber4,
		// Token: 0x04000BE6 RID: 3046
		LetterRotated,
		// Token: 0x04000BE7 RID: 3047
		A3Rotated,
		// Token: 0x04000BE8 RID: 3048
		A4Rotated,
		// Token: 0x04000BE9 RID: 3049
		A5Rotated,
		// Token: 0x04000BEA RID: 3050
		B4JisRotated,
		// Token: 0x04000BEB RID: 3051
		B5JisRotated,
		// Token: 0x04000BEC RID: 3052
		JapanesePostcardRotated,
		// Token: 0x04000BED RID: 3053
		JapaneseDoublePostcardRotated,
		// Token: 0x04000BEE RID: 3054
		A6Rotated,
		// Token: 0x04000BEF RID: 3055
		JapaneseEnvelopeKakuNumber2Rotated,
		// Token: 0x04000BF0 RID: 3056
		JapaneseEnvelopeKakuNumber3Rotated,
		// Token: 0x04000BF1 RID: 3057
		JapaneseEnvelopeChouNumber3Rotated,
		// Token: 0x04000BF2 RID: 3058
		JapaneseEnvelopeChouNumber4Rotated,
		// Token: 0x04000BF3 RID: 3059
		B6Jis,
		// Token: 0x04000BF4 RID: 3060
		B6JisRotated,
		// Token: 0x04000BF5 RID: 3061
		Standard12x11,
		// Token: 0x04000BF6 RID: 3062
		JapaneseEnvelopeYouNumber4,
		// Token: 0x04000BF7 RID: 3063
		JapaneseEnvelopeYouNumber4Rotated,
		// Token: 0x04000BF8 RID: 3064
		Prc16K,
		// Token: 0x04000BF9 RID: 3065
		Prc32K,
		// Token: 0x04000BFA RID: 3066
		Prc32KBig,
		// Token: 0x04000BFB RID: 3067
		PrcEnvelopeNumber1,
		// Token: 0x04000BFC RID: 3068
		PrcEnvelopeNumber2,
		// Token: 0x04000BFD RID: 3069
		PrcEnvelopeNumber3,
		// Token: 0x04000BFE RID: 3070
		PrcEnvelopeNumber4,
		// Token: 0x04000BFF RID: 3071
		PrcEnvelopeNumber5,
		// Token: 0x04000C00 RID: 3072
		PrcEnvelopeNumber6,
		// Token: 0x04000C01 RID: 3073
		PrcEnvelopeNumber7,
		// Token: 0x04000C02 RID: 3074
		PrcEnvelopeNumber8,
		// Token: 0x04000C03 RID: 3075
		PrcEnvelopeNumber9,
		// Token: 0x04000C04 RID: 3076
		PrcEnvelopeNumber10,
		// Token: 0x04000C05 RID: 3077
		Prc16KRotated,
		// Token: 0x04000C06 RID: 3078
		Prc32KRotated,
		// Token: 0x04000C07 RID: 3079
		Prc32KBigRotated,
		// Token: 0x04000C08 RID: 3080
		PrcEnvelopeNumber1Rotated,
		// Token: 0x04000C09 RID: 3081
		PrcEnvelopeNumber2Rotated,
		// Token: 0x04000C0A RID: 3082
		PrcEnvelopeNumber3Rotated,
		// Token: 0x04000C0B RID: 3083
		PrcEnvelopeNumber4Rotated,
		// Token: 0x04000C0C RID: 3084
		PrcEnvelopeNumber5Rotated,
		// Token: 0x04000C0D RID: 3085
		PrcEnvelopeNumber6Rotated,
		// Token: 0x04000C0E RID: 3086
		PrcEnvelopeNumber7Rotated,
		// Token: 0x04000C0F RID: 3087
		PrcEnvelopeNumber8Rotated,
		// Token: 0x04000C10 RID: 3088
		PrcEnvelopeNumber9Rotated,
		// Token: 0x04000C11 RID: 3089
		PrcEnvelopeNumber10Rotated
	}
}

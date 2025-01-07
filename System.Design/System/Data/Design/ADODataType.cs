using System;

namespace System.Data.Design
{
	internal enum ADODataType
	{
		adEmpty,
		adTinyInt = 16,
		adSmallInt = 2,
		adInteger,
		adBigInt = 20,
		adUnsignedTinyInt = 17,
		adUnsignedSmallInt,
		adUnsignedInt,
		adUnsignedBigInt = 21,
		adSingle = 4,
		adDouble,
		adCurrency,
		adDecimal = 14,
		adNumeric = 131,
		adBoolean = 11,
		adError = 10,
		adUserDefined = 132,
		adVariant = 12,
		adIDispatch = 9,
		adIUnknown = 13,
		adGUID = 72,
		adDate = 7,
		adDBDate = 133,
		adDBTime,
		adDBTimeStamp,
		adBSTR = 8,
		adChar = 129,
		adVarChar = 200,
		adLongVarChar,
		adWChar = 130,
		adVarWChar = 202,
		adLongVarWChar,
		adBinary = 128,
		adVarBinary = 204,
		adLongVarBinary,
		adChapter = 136,
		adFileTime = 64,
		adPropVariant = 138,
		adVarNumeric,
		adArray = 8192
	}
}

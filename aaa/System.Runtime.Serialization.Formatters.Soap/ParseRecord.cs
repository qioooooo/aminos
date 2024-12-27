using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200000F RID: 15
	internal sealed class ParseRecord : ITrace
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00005238 File Offset: 0x00004238
		internal ParseRecord()
		{
			this.Counter();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005248 File Offset: 0x00004248
		private void Counter()
		{
			lock (typeof(ParseRecord))
			{
				this.PRparseRecordId = ParseRecord.parseRecordIdCount++;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005294 File Offset: 0x00004294
		public string Trace()
		{
			return string.Concat(new object[]
			{
				"ParseRecord",
				this.PRparseRecordId,
				" ParseType ",
				this.PRparseTypeEnum.ToString(),
				" name ",
				this.PRname,
				" keyDt ",
				Util.PString(this.PRkeyDt)
			});
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005308 File Offset: 0x00004308
		internal void Init()
		{
			this.PRparseTypeEnum = InternalParseTypeE.Empty;
			this.PRobjectTypeEnum = InternalObjectTypeE.Empty;
			this.PRarrayTypeEnum = InternalArrayTypeE.Empty;
			this.PRmemberTypeEnum = InternalMemberTypeE.Empty;
			this.PRmemberValueEnum = InternalMemberValueE.Empty;
			this.PRobjectPositionEnum = InternalObjectPositionE.Empty;
			this.PRname = null;
			this.PRnameXmlKey = null;
			this.PRxmlNameSpace = null;
			this.PRisParsed = false;
			this.PRisProcessAttributes = false;
			this.PRvalue = null;
			this.PRkeyDt = null;
			this.PRdtType = null;
			this.PRassemblyName = null;
			this.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.PRisEnum = false;
			this.PRobjectId = 0L;
			this.PRidRef = 0L;
			this.PRarrayElementTypeString = null;
			this.PRarrayElementType = null;
			this.PRisArrayVariant = false;
			this.PRarrayElementTypeCode = InternalPrimitiveTypeE.Invalid;
			this.PRprimitiveArrayTypeString = null;
			this.PRrank = 0;
			this.PRlengthA = null;
			this.PRpositionA = null;
			this.PRlowerBoundA = null;
			this.PRupperBoundA = null;
			this.PRindexMap = null;
			this.PRmemberIndex = 0;
			this.PRlinearlength = 0;
			this.PRrectangularMap = null;
			this.PRisLowerBound = false;
			this.PRtopId = 0L;
			this.PRheaderId = 0L;
			this.PRisHeaderRoot = false;
			this.PRisAttributesProcessed = false;
			this.PRisMustUnderstand = false;
			this.PRparseStateEnum = InternalParseStateE.Initial;
			this.PRisWaitingForNestedObject = false;
			this.PRisValueTypeFixup = false;
			this.PRnewObj = null;
			this.PRobjectA = null;
			this.PRprimitiveArray = null;
			this.PRobjectInfo = null;
			this.PRisRegistered = false;
			this.PRisXmlAttribute = false;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000546C File Offset: 0x0000446C
		internal ParseRecord Copy()
		{
			return new ParseRecord
			{
				PRparseTypeEnum = this.PRparseTypeEnum,
				PRobjectTypeEnum = this.PRobjectTypeEnum,
				PRarrayTypeEnum = this.PRarrayTypeEnum,
				PRmemberTypeEnum = this.PRmemberTypeEnum,
				PRmemberValueEnum = this.PRmemberValueEnum,
				PRobjectPositionEnum = this.PRobjectPositionEnum,
				PRname = this.PRname,
				PRisParsed = this.PRisParsed,
				PRisProcessAttributes = this.PRisProcessAttributes,
				PRnameXmlKey = this.PRnameXmlKey,
				PRxmlNameSpace = this.PRxmlNameSpace,
				PRvalue = this.PRvalue,
				PRkeyDt = this.PRkeyDt,
				PRdtType = this.PRdtType,
				PRassemblyName = this.PRassemblyName,
				PRdtTypeCode = this.PRdtTypeCode,
				PRisEnum = this.PRisEnum,
				PRobjectId = this.PRobjectId,
				PRidRef = this.PRidRef,
				PRarrayElementTypeString = this.PRarrayElementTypeString,
				PRarrayElementType = this.PRarrayElementType,
				PRisArrayVariant = this.PRisArrayVariant,
				PRarrayElementTypeCode = this.PRarrayElementTypeCode,
				PRprimitiveArrayTypeString = this.PRprimitiveArrayTypeString,
				PRrank = this.PRrank,
				PRlengthA = this.PRlengthA,
				PRpositionA = this.PRpositionA,
				PRlowerBoundA = this.PRlowerBoundA,
				PRupperBoundA = this.PRupperBoundA,
				PRindexMap = this.PRindexMap,
				PRmemberIndex = this.PRmemberIndex,
				PRlinearlength = this.PRlinearlength,
				PRrectangularMap = this.PRrectangularMap,
				PRisLowerBound = this.PRisLowerBound,
				PRtopId = this.PRtopId,
				PRheaderId = this.PRheaderId,
				PRisHeaderRoot = this.PRisHeaderRoot,
				PRisAttributesProcessed = this.PRisAttributesProcessed,
				PRisMustUnderstand = this.PRisMustUnderstand,
				PRparseStateEnum = this.PRparseStateEnum,
				PRisWaitingForNestedObject = this.PRisWaitingForNestedObject,
				PRisValueTypeFixup = this.PRisValueTypeFixup,
				PRnewObj = this.PRnewObj,
				PRobjectA = this.PRobjectA,
				PRprimitiveArray = this.PRprimitiveArray,
				PRobjectInfo = this.PRobjectInfo,
				PRisRegistered = this.PRisRegistered,
				PRisXmlAttribute = this.PRisXmlAttribute
			};
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000056C0 File Offset: 0x000046C0
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
		}

		// Token: 0x0400004D RID: 77
		internal static int parseRecordIdCount = 1;

		// Token: 0x0400004E RID: 78
		internal int PRparseRecordId;

		// Token: 0x0400004F RID: 79
		internal InternalParseTypeE PRparseTypeEnum;

		// Token: 0x04000050 RID: 80
		internal InternalObjectTypeE PRobjectTypeEnum;

		// Token: 0x04000051 RID: 81
		internal InternalArrayTypeE PRarrayTypeEnum;

		// Token: 0x04000052 RID: 82
		internal InternalMemberTypeE PRmemberTypeEnum;

		// Token: 0x04000053 RID: 83
		internal InternalMemberValueE PRmemberValueEnum;

		// Token: 0x04000054 RID: 84
		internal InternalObjectPositionE PRobjectPositionEnum;

		// Token: 0x04000055 RID: 85
		internal string PRname;

		// Token: 0x04000056 RID: 86
		internal string PRnameXmlKey;

		// Token: 0x04000057 RID: 87
		internal string PRxmlNameSpace;

		// Token: 0x04000058 RID: 88
		internal bool PRisParsed;

		// Token: 0x04000059 RID: 89
		internal bool PRisProcessAttributes;

		// Token: 0x0400005A RID: 90
		internal string PRvalue;

		// Token: 0x0400005B RID: 91
		internal object PRvarValue;

		// Token: 0x0400005C RID: 92
		internal string PRkeyDt;

		// Token: 0x0400005D RID: 93
		internal string PRtypeXmlKey;

		// Token: 0x0400005E RID: 94
		internal Type PRdtType;

		// Token: 0x0400005F RID: 95
		internal string PRassemblyName;

		// Token: 0x04000060 RID: 96
		internal InternalPrimitiveTypeE PRdtTypeCode;

		// Token: 0x04000061 RID: 97
		internal bool PRisVariant;

		// Token: 0x04000062 RID: 98
		internal bool PRisEnum;

		// Token: 0x04000063 RID: 99
		internal long PRobjectId;

		// Token: 0x04000064 RID: 100
		internal long PRidRef;

		// Token: 0x04000065 RID: 101
		internal string PRarrayElementTypeString;

		// Token: 0x04000066 RID: 102
		internal Type PRarrayElementType;

		// Token: 0x04000067 RID: 103
		internal bool PRisArrayVariant;

		// Token: 0x04000068 RID: 104
		internal InternalPrimitiveTypeE PRarrayElementTypeCode;

		// Token: 0x04000069 RID: 105
		internal string PRprimitiveArrayTypeString;

		// Token: 0x0400006A RID: 106
		internal int PRrank;

		// Token: 0x0400006B RID: 107
		internal int[] PRlengthA;

		// Token: 0x0400006C RID: 108
		internal int[] PRpositionA;

		// Token: 0x0400006D RID: 109
		internal int[] PRlowerBoundA;

		// Token: 0x0400006E RID: 110
		internal int[] PRupperBoundA;

		// Token: 0x0400006F RID: 111
		internal int[] PRindexMap;

		// Token: 0x04000070 RID: 112
		internal int PRmemberIndex;

		// Token: 0x04000071 RID: 113
		internal int PRlinearlength;

		// Token: 0x04000072 RID: 114
		internal int[] PRrectangularMap;

		// Token: 0x04000073 RID: 115
		internal bool PRisLowerBound;

		// Token: 0x04000074 RID: 116
		internal long PRtopId;

		// Token: 0x04000075 RID: 117
		internal long PRheaderId;

		// Token: 0x04000076 RID: 118
		internal bool PRisHeaderRoot;

		// Token: 0x04000077 RID: 119
		internal bool PRisAttributesProcessed;

		// Token: 0x04000078 RID: 120
		internal bool PRisMustUnderstand;

		// Token: 0x04000079 RID: 121
		internal InternalParseStateE PRparseStateEnum;

		// Token: 0x0400007A RID: 122
		internal bool PRisWaitingForNestedObject;

		// Token: 0x0400007B RID: 123
		internal ReadObjectInfo PRobjectInfo;

		// Token: 0x0400007C RID: 124
		internal bool PRisValueTypeFixup;

		// Token: 0x0400007D RID: 125
		internal object PRnewObj;

		// Token: 0x0400007E RID: 126
		internal object[] PRobjectA;

		// Token: 0x0400007F RID: 127
		internal PrimitiveArray PRprimitiveArray;

		// Token: 0x04000080 RID: 128
		internal bool PRisRegistered;

		// Token: 0x04000081 RID: 129
		internal bool PRisXmlAttribute;
	}
}

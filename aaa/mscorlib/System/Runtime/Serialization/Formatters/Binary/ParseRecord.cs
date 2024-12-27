using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D5 RID: 2005
	internal sealed class ParseRecord
	{
		// Token: 0x06004750 RID: 18256 RVA: 0x000F5548 File Offset: 0x000F4548
		internal ParseRecord()
		{
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x000F5550 File Offset: 0x000F4550
		internal void Init()
		{
			this.PRparseTypeEnum = InternalParseTypeE.Empty;
			this.PRobjectTypeEnum = InternalObjectTypeE.Empty;
			this.PRarrayTypeEnum = InternalArrayTypeE.Empty;
			this.PRmemberTypeEnum = InternalMemberTypeE.Empty;
			this.PRmemberValueEnum = InternalMemberValueE.Empty;
			this.PRobjectPositionEnum = InternalObjectPositionE.Empty;
			this.PRname = null;
			this.PRvalue = null;
			this.PRkeyDt = null;
			this.PRdtType = null;
			this.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.PRisEnum = false;
			this.PRobjectId = 0L;
			this.PRidRef = 0L;
			this.PRarrayElementTypeString = null;
			this.PRarrayElementType = null;
			this.PRisArrayVariant = false;
			this.PRarrayElementTypeCode = InternalPrimitiveTypeE.Invalid;
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
			this.PRisValueTypeFixup = false;
			this.PRnewObj = null;
			this.PRobjectA = null;
			this.PRprimitiveArray = null;
			this.PRobjectInfo = null;
			this.PRisRegistered = false;
			this.PRmemberData = null;
			this.PRsi = null;
			this.PRnullCount = 0;
		}

		// Token: 0x04002409 RID: 9225
		internal static int parseRecordIdCount = 1;

		// Token: 0x0400240A RID: 9226
		internal int PRparseRecordId;

		// Token: 0x0400240B RID: 9227
		internal InternalParseTypeE PRparseTypeEnum;

		// Token: 0x0400240C RID: 9228
		internal InternalObjectTypeE PRobjectTypeEnum;

		// Token: 0x0400240D RID: 9229
		internal InternalArrayTypeE PRarrayTypeEnum;

		// Token: 0x0400240E RID: 9230
		internal InternalMemberTypeE PRmemberTypeEnum;

		// Token: 0x0400240F RID: 9231
		internal InternalMemberValueE PRmemberValueEnum;

		// Token: 0x04002410 RID: 9232
		internal InternalObjectPositionE PRobjectPositionEnum;

		// Token: 0x04002411 RID: 9233
		internal string PRname;

		// Token: 0x04002412 RID: 9234
		internal string PRvalue;

		// Token: 0x04002413 RID: 9235
		internal object PRvarValue;

		// Token: 0x04002414 RID: 9236
		internal string PRkeyDt;

		// Token: 0x04002415 RID: 9237
		internal Type PRdtType;

		// Token: 0x04002416 RID: 9238
		internal InternalPrimitiveTypeE PRdtTypeCode;

		// Token: 0x04002417 RID: 9239
		internal bool PRisVariant;

		// Token: 0x04002418 RID: 9240
		internal bool PRisEnum;

		// Token: 0x04002419 RID: 9241
		internal long PRobjectId;

		// Token: 0x0400241A RID: 9242
		internal long PRidRef;

		// Token: 0x0400241B RID: 9243
		internal string PRarrayElementTypeString;

		// Token: 0x0400241C RID: 9244
		internal Type PRarrayElementType;

		// Token: 0x0400241D RID: 9245
		internal bool PRisArrayVariant;

		// Token: 0x0400241E RID: 9246
		internal InternalPrimitiveTypeE PRarrayElementTypeCode;

		// Token: 0x0400241F RID: 9247
		internal int PRrank;

		// Token: 0x04002420 RID: 9248
		internal int[] PRlengthA;

		// Token: 0x04002421 RID: 9249
		internal int[] PRpositionA;

		// Token: 0x04002422 RID: 9250
		internal int[] PRlowerBoundA;

		// Token: 0x04002423 RID: 9251
		internal int[] PRupperBoundA;

		// Token: 0x04002424 RID: 9252
		internal int[] PRindexMap;

		// Token: 0x04002425 RID: 9253
		internal int PRmemberIndex;

		// Token: 0x04002426 RID: 9254
		internal int PRlinearlength;

		// Token: 0x04002427 RID: 9255
		internal int[] PRrectangularMap;

		// Token: 0x04002428 RID: 9256
		internal bool PRisLowerBound;

		// Token: 0x04002429 RID: 9257
		internal long PRtopId;

		// Token: 0x0400242A RID: 9258
		internal long PRheaderId;

		// Token: 0x0400242B RID: 9259
		internal ReadObjectInfo PRobjectInfo;

		// Token: 0x0400242C RID: 9260
		internal bool PRisValueTypeFixup;

		// Token: 0x0400242D RID: 9261
		internal object PRnewObj;

		// Token: 0x0400242E RID: 9262
		internal object[] PRobjectA;

		// Token: 0x0400242F RID: 9263
		internal PrimitiveArray PRprimitiveArray;

		// Token: 0x04002430 RID: 9264
		internal bool PRisRegistered;

		// Token: 0x04002431 RID: 9265
		internal object[] PRmemberData;

		// Token: 0x04002432 RID: 9266
		internal SerializationInfo PRsi;

		// Token: 0x04002433 RID: 9267
		internal int PRnullCount;
	}
}

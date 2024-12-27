using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D4 RID: 2004
	internal sealed class ObjectProgress
	{
		// Token: 0x0600474A RID: 18250 RVA: 0x000F5354 File Offset: 0x000F4354
		internal ObjectProgress()
		{
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x000F5370 File Offset: 0x000F4370
		[Conditional("SER_LOGGING")]
		private void Counter()
		{
			lock (this)
			{
				this.opRecordId = ObjectProgress.opRecordIdCount++;
				if (ObjectProgress.opRecordIdCount > 1000)
				{
					ObjectProgress.opRecordIdCount = 1;
				}
			}
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x000F53C4 File Offset: 0x000F43C4
		internal void Init()
		{
			this.isInitial = false;
			this.count = 0;
			this.expectedType = BinaryTypeEnum.ObjectUrt;
			this.expectedTypeInformation = null;
			this.name = null;
			this.objectTypeEnum = InternalObjectTypeE.Empty;
			this.memberTypeEnum = InternalMemberTypeE.Empty;
			this.memberValueEnum = InternalMemberValueE.Empty;
			this.dtType = null;
			this.numItems = 0;
			this.nullCount = 0;
			this.typeInformation = null;
			this.memberLength = 0;
			this.binaryTypeEnumA = null;
			this.typeInformationA = null;
			this.memberNames = null;
			this.memberTypes = null;
			this.pr.Init();
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x000F5453 File Offset: 0x000F4453
		internal void ArrayCountIncrement(int value)
		{
			this.count += value;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x000F5464 File Offset: 0x000F4464
		internal bool GetNext(out BinaryTypeEnum outBinaryTypeEnum, out object outTypeInformation)
		{
			outBinaryTypeEnum = BinaryTypeEnum.Primitive;
			outTypeInformation = null;
			if (this.objectTypeEnum == InternalObjectTypeE.Array)
			{
				if (this.count == this.numItems)
				{
					return false;
				}
				outBinaryTypeEnum = this.binaryTypeEnum;
				outTypeInformation = this.typeInformation;
				if (this.count == 0)
				{
					this.isInitial = false;
				}
				this.count++;
				return true;
			}
			else
			{
				if (this.count == this.memberLength && !this.isInitial)
				{
					return false;
				}
				outBinaryTypeEnum = this.binaryTypeEnumA[this.count];
				outTypeInformation = this.typeInformationA[this.count];
				if (this.count == 0)
				{
					this.isInitial = false;
				}
				this.name = this.memberNames[this.count];
				Type[] array = this.memberTypes;
				this.dtType = this.memberTypes[this.count];
				this.count++;
				return true;
			}
		}

		// Token: 0x040023F4 RID: 9204
		internal static int opRecordIdCount = 1;

		// Token: 0x040023F5 RID: 9205
		internal int opRecordId;

		// Token: 0x040023F6 RID: 9206
		internal bool isInitial;

		// Token: 0x040023F7 RID: 9207
		internal int count;

		// Token: 0x040023F8 RID: 9208
		internal BinaryTypeEnum expectedType = BinaryTypeEnum.ObjectUrt;

		// Token: 0x040023F9 RID: 9209
		internal object expectedTypeInformation;

		// Token: 0x040023FA RID: 9210
		internal string name;

		// Token: 0x040023FB RID: 9211
		internal InternalObjectTypeE objectTypeEnum;

		// Token: 0x040023FC RID: 9212
		internal InternalMemberTypeE memberTypeEnum;

		// Token: 0x040023FD RID: 9213
		internal InternalMemberValueE memberValueEnum;

		// Token: 0x040023FE RID: 9214
		internal Type dtType;

		// Token: 0x040023FF RID: 9215
		internal int numItems;

		// Token: 0x04002400 RID: 9216
		internal BinaryTypeEnum binaryTypeEnum;

		// Token: 0x04002401 RID: 9217
		internal object typeInformation;

		// Token: 0x04002402 RID: 9218
		internal int nullCount;

		// Token: 0x04002403 RID: 9219
		internal int memberLength;

		// Token: 0x04002404 RID: 9220
		internal BinaryTypeEnum[] binaryTypeEnumA;

		// Token: 0x04002405 RID: 9221
		internal object[] typeInformationA;

		// Token: 0x04002406 RID: 9222
		internal string[] memberNames;

		// Token: 0x04002407 RID: 9223
		internal Type[] memberTypes;

		// Token: 0x04002408 RID: 9224
		internal ParseRecord pr = new ParseRecord();
	}
}

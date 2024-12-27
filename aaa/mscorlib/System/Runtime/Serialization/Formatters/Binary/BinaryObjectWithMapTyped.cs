using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CD RID: 1997
	internal sealed class BinaryObjectWithMapTyped : IStreamable
	{
		// Token: 0x06004721 RID: 18209 RVA: 0x000F48BB File Offset: 0x000F38BB
		internal BinaryObjectWithMapTyped()
		{
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x000F48C3 File Offset: 0x000F38C3
		internal BinaryObjectWithMapTyped(BinaryHeaderEnum binaryHeaderEnum)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x000F48D4 File Offset: 0x000F38D4
		internal void Set(int objectId, string name, int numMembers, string[] memberNames, BinaryTypeEnum[] binaryTypeEnumA, object[] typeInformationA, int[] memberAssemIds, int assemId)
		{
			this.objectId = objectId;
			this.assemId = assemId;
			this.name = name;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.binaryTypeEnumA = binaryTypeEnumA;
			this.typeInformationA = typeInformationA;
			this.memberAssemIds = memberAssemIds;
			this.assemId = assemId;
			if (assemId > 0)
			{
				this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapTypedAssemId;
				return;
			}
			this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapTyped;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x000F493C File Offset: 0x000F393C
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte((byte)this.binaryHeaderEnum);
			sout.WriteInt32(this.objectId);
			sout.WriteString(this.name);
			sout.WriteInt32(this.numMembers);
			for (int i = 0; i < this.numMembers; i++)
			{
				sout.WriteString(this.memberNames[i]);
			}
			for (int j = 0; j < this.numMembers; j++)
			{
				sout.WriteByte((byte)this.binaryTypeEnumA[j]);
			}
			for (int k = 0; k < this.numMembers; k++)
			{
				BinaryConverter.WriteTypeInfo(this.binaryTypeEnumA[k], this.typeInformationA[k], this.memberAssemIds[k], sout);
			}
			if (this.assemId > 0)
			{
				sout.WriteInt32(this.assemId);
			}
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x000F4A00 File Offset: 0x000F3A00
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.name = input.ReadString();
			this.numMembers = input.ReadInt32();
			this.memberNames = new string[this.numMembers];
			this.binaryTypeEnumA = new BinaryTypeEnum[this.numMembers];
			this.typeInformationA = new object[this.numMembers];
			this.memberAssemIds = new int[this.numMembers];
			for (int i = 0; i < this.numMembers; i++)
			{
				this.memberNames[i] = input.ReadString();
			}
			for (int j = 0; j < this.numMembers; j++)
			{
				this.binaryTypeEnumA[j] = (BinaryTypeEnum)input.ReadByte();
			}
			for (int k = 0; k < this.numMembers; k++)
			{
				if (this.binaryTypeEnumA[k] != BinaryTypeEnum.ObjectUrt && this.binaryTypeEnumA[k] != BinaryTypeEnum.ObjectUser)
				{
					this.typeInformationA[k] = BinaryConverter.ReadTypeInfo(this.binaryTypeEnumA[k], input, out this.memberAssemIds[k]);
				}
				else
				{
					BinaryConverter.ReadTypeInfo(this.binaryTypeEnumA[k], input, out this.memberAssemIds[k]);
				}
			}
			if (this.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTypedAssemId)
			{
				this.assemId = input.ReadInt32();
			}
		}

		// Token: 0x040023D3 RID: 9171
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x040023D4 RID: 9172
		internal int objectId;

		// Token: 0x040023D5 RID: 9173
		internal string name;

		// Token: 0x040023D6 RID: 9174
		internal int numMembers;

		// Token: 0x040023D7 RID: 9175
		internal string[] memberNames;

		// Token: 0x040023D8 RID: 9176
		internal BinaryTypeEnum[] binaryTypeEnumA;

		// Token: 0x040023D9 RID: 9177
		internal object[] typeInformationA;

		// Token: 0x040023DA RID: 9178
		internal int[] memberAssemIds;

		// Token: 0x040023DB RID: 9179
		internal int assemId;
	}
}

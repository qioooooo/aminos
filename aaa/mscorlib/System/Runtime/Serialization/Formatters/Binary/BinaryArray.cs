using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CE RID: 1998
	internal sealed class BinaryArray : IStreamable
	{
		// Token: 0x06004726 RID: 18214 RVA: 0x000F4B2E File Offset: 0x000F3B2E
		internal BinaryArray()
		{
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x000F4B36 File Offset: 0x000F3B36
		internal BinaryArray(BinaryHeaderEnum binaryHeaderEnum)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x000F4B48 File Offset: 0x000F3B48
		internal void Set(int objectId, int rank, int[] lengthA, int[] lowerBoundA, BinaryTypeEnum binaryTypeEnum, object typeInformation, BinaryArrayTypeEnum binaryArrayTypeEnum, int assemId)
		{
			this.objectId = objectId;
			this.binaryArrayTypeEnum = binaryArrayTypeEnum;
			this.rank = rank;
			this.lengthA = lengthA;
			this.lowerBoundA = lowerBoundA;
			this.binaryTypeEnum = binaryTypeEnum;
			this.typeInformation = typeInformation;
			this.assemId = assemId;
			this.binaryHeaderEnum = BinaryHeaderEnum.Array;
			if (binaryArrayTypeEnum == BinaryArrayTypeEnum.Single)
			{
				if (binaryTypeEnum == BinaryTypeEnum.Primitive)
				{
					this.binaryHeaderEnum = BinaryHeaderEnum.ArraySinglePrimitive;
					return;
				}
				if (binaryTypeEnum == BinaryTypeEnum.String)
				{
					this.binaryHeaderEnum = BinaryHeaderEnum.ArraySingleString;
					return;
				}
				if (binaryTypeEnum == BinaryTypeEnum.Object)
				{
					this.binaryHeaderEnum = BinaryHeaderEnum.ArraySingleObject;
				}
			}
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x000F4BC8 File Offset: 0x000F3BC8
		public void Write(__BinaryWriter sout)
		{
			switch (this.binaryHeaderEnum)
			{
			case BinaryHeaderEnum.ArraySinglePrimitive:
				sout.WriteByte((byte)this.binaryHeaderEnum);
				sout.WriteInt32(this.objectId);
				sout.WriteInt32(this.lengthA[0]);
				sout.WriteByte((byte)((InternalPrimitiveTypeE)this.typeInformation));
				return;
			case BinaryHeaderEnum.ArraySingleObject:
				sout.WriteByte((byte)this.binaryHeaderEnum);
				sout.WriteInt32(this.objectId);
				sout.WriteInt32(this.lengthA[0]);
				return;
			case BinaryHeaderEnum.ArraySingleString:
				sout.WriteByte((byte)this.binaryHeaderEnum);
				sout.WriteInt32(this.objectId);
				sout.WriteInt32(this.lengthA[0]);
				return;
			default:
			{
				sout.WriteByte((byte)this.binaryHeaderEnum);
				sout.WriteInt32(this.objectId);
				sout.WriteByte((byte)this.binaryArrayTypeEnum);
				sout.WriteInt32(this.rank);
				for (int i = 0; i < this.rank; i++)
				{
					sout.WriteInt32(this.lengthA[i]);
				}
				if (this.binaryArrayTypeEnum == BinaryArrayTypeEnum.SingleOffset || this.binaryArrayTypeEnum == BinaryArrayTypeEnum.JaggedOffset || this.binaryArrayTypeEnum == BinaryArrayTypeEnum.RectangularOffset)
				{
					for (int j = 0; j < this.rank; j++)
					{
						sout.WriteInt32(this.lowerBoundA[j]);
					}
				}
				sout.WriteByte((byte)this.binaryTypeEnum);
				BinaryConverter.WriteTypeInfo(this.binaryTypeEnum, this.typeInformation, this.assemId, sout);
				return;
			}
			}
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x000F4D30 File Offset: 0x000F3D30
		public void Read(__BinaryParser input)
		{
			switch (this.binaryHeaderEnum)
			{
			case BinaryHeaderEnum.ArraySinglePrimitive:
				this.objectId = input.ReadInt32();
				this.lengthA = new int[1];
				this.lengthA[0] = input.ReadInt32();
				this.binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
				this.rank = 1;
				this.lowerBoundA = new int[this.rank];
				this.binaryTypeEnum = BinaryTypeEnum.Primitive;
				this.typeInformation = (InternalPrimitiveTypeE)input.ReadByte();
				return;
			case BinaryHeaderEnum.ArraySingleObject:
				this.objectId = input.ReadInt32();
				this.lengthA = new int[1];
				this.lengthA[0] = input.ReadInt32();
				this.binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
				this.rank = 1;
				this.lowerBoundA = new int[this.rank];
				this.binaryTypeEnum = BinaryTypeEnum.Object;
				this.typeInformation = null;
				return;
			case BinaryHeaderEnum.ArraySingleString:
				this.objectId = input.ReadInt32();
				this.lengthA = new int[1];
				this.lengthA[0] = input.ReadInt32();
				this.binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
				this.rank = 1;
				this.lowerBoundA = new int[this.rank];
				this.binaryTypeEnum = BinaryTypeEnum.String;
				this.typeInformation = null;
				return;
			default:
			{
				this.objectId = input.ReadInt32();
				this.binaryArrayTypeEnum = (BinaryArrayTypeEnum)input.ReadByte();
				this.rank = input.ReadInt32();
				this.lengthA = new int[this.rank];
				this.lowerBoundA = new int[this.rank];
				for (int i = 0; i < this.rank; i++)
				{
					this.lengthA[i] = input.ReadInt32();
				}
				if (this.binaryArrayTypeEnum == BinaryArrayTypeEnum.SingleOffset || this.binaryArrayTypeEnum == BinaryArrayTypeEnum.JaggedOffset || this.binaryArrayTypeEnum == BinaryArrayTypeEnum.RectangularOffset)
				{
					for (int j = 0; j < this.rank; j++)
					{
						this.lowerBoundA[j] = input.ReadInt32();
					}
				}
				this.binaryTypeEnum = (BinaryTypeEnum)input.ReadByte();
				this.typeInformation = BinaryConverter.ReadTypeInfo(this.binaryTypeEnum, input, out this.assemId);
				return;
			}
			}
		}

		// Token: 0x040023DC RID: 9180
		internal int objectId;

		// Token: 0x040023DD RID: 9181
		internal int rank;

		// Token: 0x040023DE RID: 9182
		internal int[] lengthA;

		// Token: 0x040023DF RID: 9183
		internal int[] lowerBoundA;

		// Token: 0x040023E0 RID: 9184
		internal BinaryTypeEnum binaryTypeEnum;

		// Token: 0x040023E1 RID: 9185
		internal object typeInformation;

		// Token: 0x040023E2 RID: 9186
		internal int assemId;

		// Token: 0x040023E3 RID: 9187
		private BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x040023E4 RID: 9188
		internal BinaryArrayTypeEnum binaryArrayTypeEnum;
	}
}

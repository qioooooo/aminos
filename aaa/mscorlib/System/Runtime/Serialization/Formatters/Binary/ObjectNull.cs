using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D1 RID: 2001
	internal sealed class ObjectNull : IStreamable
	{
		// Token: 0x06004738 RID: 18232 RVA: 0x000F4FD0 File Offset: 0x000F3FD0
		internal ObjectNull()
		{
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x000F4FD8 File Offset: 0x000F3FD8
		internal void SetNullCount(int nullCount)
		{
			this.nullCount = nullCount;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x000F4FE4 File Offset: 0x000F3FE4
		public void Write(__BinaryWriter sout)
		{
			if (this.nullCount == 1)
			{
				sout.WriteByte(10);
				return;
			}
			if (this.nullCount < 256)
			{
				sout.WriteByte(13);
				sout.WriteByte((byte)this.nullCount);
				return;
			}
			sout.WriteByte(14);
			sout.WriteInt32(this.nullCount);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x000F503A File Offset: 0x000F403A
		public void Read(__BinaryParser input)
		{
			this.Read(input, BinaryHeaderEnum.ObjectNull);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x000F5048 File Offset: 0x000F4048
		public void Read(__BinaryParser input, BinaryHeaderEnum binaryHeaderEnum)
		{
			switch (binaryHeaderEnum)
			{
			case BinaryHeaderEnum.ObjectNull:
				this.nullCount = 1;
				return;
			case BinaryHeaderEnum.MessageEnd:
			case BinaryHeaderEnum.Assembly:
				break;
			case BinaryHeaderEnum.ObjectNullMultiple256:
				this.nullCount = (int)input.ReadByte();
				return;
			case BinaryHeaderEnum.ObjectNullMultiple:
				this.nullCount = input.ReadInt32();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x000F5096 File Offset: 0x000F4096
		public void Dump()
		{
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x000F5098 File Offset: 0x000F4098
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				if (this.nullCount == 1)
				{
					return;
				}
				if (this.nullCount < 256)
				{
				}
			}
		}

		// Token: 0x040023E8 RID: 9192
		internal int nullCount;
	}
}

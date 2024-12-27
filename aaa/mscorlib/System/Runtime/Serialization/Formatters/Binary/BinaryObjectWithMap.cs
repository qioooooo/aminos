using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CC RID: 1996
	internal sealed class BinaryObjectWithMap : IStreamable
	{
		// Token: 0x0600471A RID: 18202 RVA: 0x000F474A File Offset: 0x000F374A
		internal BinaryObjectWithMap()
		{
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x000F4752 File Offset: 0x000F3752
		internal BinaryObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x000F4761 File Offset: 0x000F3761
		internal void Set(int objectId, string name, int numMembers, string[] memberNames, int assemId)
		{
			this.objectId = objectId;
			this.name = name;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.assemId = assemId;
			if (assemId > 0)
			{
				this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapAssemId;
				return;
			}
			this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMap;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x000F479C File Offset: 0x000F379C
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
			if (this.assemId > 0)
			{
				sout.WriteInt32(this.assemId);
			}
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x000F4810 File Offset: 0x000F3810
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.name = input.ReadString();
			this.numMembers = input.ReadInt32();
			this.memberNames = new string[this.numMembers];
			for (int i = 0; i < this.numMembers; i++)
			{
				this.memberNames[i] = input.ReadString();
			}
			if (this.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapAssemId)
			{
				this.assemId = input.ReadInt32();
			}
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000F4886 File Offset: 0x000F3886
		public void Dump()
		{
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x000F4888 File Offset: 0x000F3888
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				for (int i = 0; i < this.numMembers; i++)
				{
				}
				BinaryHeaderEnum binaryHeaderEnum = this.binaryHeaderEnum;
			}
		}

		// Token: 0x040023CD RID: 9165
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x040023CE RID: 9166
		internal int objectId;

		// Token: 0x040023CF RID: 9167
		internal string name;

		// Token: 0x040023D0 RID: 9168
		internal int numMembers;

		// Token: 0x040023D1 RID: 9169
		internal string[] memberNames;

		// Token: 0x040023D2 RID: 9170
		internal int assemId;
	}
}

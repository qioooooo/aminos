using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007BC RID: 1980
	internal sealed class ObjectMapInfo
	{
		// Token: 0x060046C9 RID: 18121 RVA: 0x000F317B File Offset: 0x000F217B
		internal ObjectMapInfo(int objectId, int numMembers, string[] memberNames, Type[] memberTypes)
		{
			this.objectId = objectId;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.memberTypes = memberTypes;
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x000F31A0 File Offset: 0x000F21A0
		internal bool isCompatible(int numMembers, string[] memberNames, Type[] memberTypes)
		{
			bool flag = true;
			if (this.numMembers == numMembers)
			{
				for (int i = 0; i < numMembers; i++)
				{
					if (!this.memberNames[i].Equals(memberNames[i]))
					{
						flag = false;
						break;
					}
					if (memberTypes != null && this.memberTypes[i] != memberTypes[i])
					{
						flag = false;
						break;
					}
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0400239A RID: 9114
		internal int objectId;

		// Token: 0x0400239B RID: 9115
		private int numMembers;

		// Token: 0x0400239C RID: 9116
		private string[] memberNames;

		// Token: 0x0400239D RID: 9117
		private Type[] memberTypes;
	}
}

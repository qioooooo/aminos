using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000358 RID: 856
	[Serializable]
	internal class MemberHolder
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x00056E4C File Offset: 0x00055E4C
		internal MemberHolder(Type type, StreamingContext ctx)
		{
			this.memberType = type;
			this.context = ctx;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x00056E62 File Offset: 0x00055E62
		public override int GetHashCode()
		{
			return this.memberType.GetHashCode();
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00056E70 File Offset: 0x00055E70
		public override bool Equals(object obj)
		{
			if (!(obj is MemberHolder))
			{
				return false;
			}
			MemberHolder memberHolder = (MemberHolder)obj;
			return memberHolder.memberType == this.memberType && memberHolder.context.State == this.context.State;
		}

		// Token: 0x04000E3B RID: 3643
		internal MemberInfo[] members;

		// Token: 0x04000E3C RID: 3644
		internal Type memberType;

		// Token: 0x04000E3D RID: 3645
		internal StreamingContext context;
	}
}

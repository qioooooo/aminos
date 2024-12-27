using System;
using System.Reflection;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000288 RID: 648
	internal sealed class FieldInfoEx : IComparable
	{
		// Token: 0x06002226 RID: 8742 RVA: 0x0026CA78 File Offset: 0x0026BE78
		internal FieldInfoEx(FieldInfo fi, int offset, Normalizer normalizer)
		{
			this.fieldInfo = fi;
			this.offset = offset;
			this.normalizer = normalizer;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x0026CAA0 File Offset: 0x0026BEA0
		public int CompareTo(object other)
		{
			FieldInfoEx fieldInfoEx = other as FieldInfoEx;
			if (fieldInfoEx == null)
			{
				return -1;
			}
			return this.offset.CompareTo(fieldInfoEx.offset);
		}

		// Token: 0x04001647 RID: 5703
		internal readonly int offset;

		// Token: 0x04001648 RID: 5704
		internal readonly FieldInfo fieldInfo;

		// Token: 0x04001649 RID: 5705
		internal readonly Normalizer normalizer;
	}
}

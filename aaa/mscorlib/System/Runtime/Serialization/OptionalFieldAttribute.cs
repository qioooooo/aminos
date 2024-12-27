using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000359 RID: 857
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalFieldAttribute : Attribute
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x00056EC6 File Offset: 0x00055EC6
		// (set) Token: 0x0600223A RID: 8762 RVA: 0x00056ECE File Offset: 0x00055ECE
		public int VersionAdded
		{
			get
			{
				return this.versionAdded;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Serialization_OptionalFieldVersionValue"));
				}
				this.versionAdded = value;
			}
		}

		// Token: 0x04000E3E RID: 3646
		private int versionAdded = 1;
	}
}

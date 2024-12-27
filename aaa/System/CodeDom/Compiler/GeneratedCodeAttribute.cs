using System;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F5 RID: 501
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
	public sealed class GeneratedCodeAttribute : Attribute
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00037E94 File Offset: 0x00036E94
		public GeneratedCodeAttribute(string tool, string version)
		{
			this.tool = tool;
			this.version = version;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x00037EAA File Offset: 0x00036EAA
		public string Tool
		{
			get
			{
				return this.tool;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00037EB2 File Offset: 0x00036EB2
		public string Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x04000F83 RID: 3971
		private readonly string tool;

		// Token: 0x04000F84 RID: 3972
		private readonly string version;
	}
}

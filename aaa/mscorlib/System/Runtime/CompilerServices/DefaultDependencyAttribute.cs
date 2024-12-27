using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005DC RID: 1500
	[AttributeUsage(AttributeTargets.Assembly)]
	[Serializable]
	public sealed class DefaultDependencyAttribute : Attribute
	{
		// Token: 0x060037A6 RID: 14246 RVA: 0x000BB943 File Offset: 0x000BA943
		public DefaultDependencyAttribute(LoadHint loadHintArgument)
		{
			this.loadHint = loadHintArgument;
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x000BB952 File Offset: 0x000BA952
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04001CB3 RID: 7347
		private LoadHint loadHint;
	}
}

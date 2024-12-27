using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005DD RID: 1501
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[Serializable]
	public sealed class DependencyAttribute : Attribute
	{
		// Token: 0x060037A8 RID: 14248 RVA: 0x000BB95A File Offset: 0x000BA95A
		public DependencyAttribute(string dependentAssemblyArgument, LoadHint loadHintArgument)
		{
			this.dependentAssembly = dependentAssemblyArgument;
			this.loadHint = loadHintArgument;
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060037A9 RID: 14249 RVA: 0x000BB970 File Offset: 0x000BA970
		public string DependentAssembly
		{
			get
			{
				return this.dependentAssembly;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060037AA RID: 14250 RVA: 0x000BB978 File Offset: 0x000BA978
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04001CB4 RID: 7348
		private string dependentAssembly;

		// Token: 0x04001CB5 RID: 7349
		private LoadHint loadHint;
	}
}

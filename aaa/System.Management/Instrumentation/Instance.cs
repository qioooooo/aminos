using System;

namespace System.Management.Instrumentation
{
	// Token: 0x020000A9 RID: 169
	[InstrumentationClass(InstrumentationType.Instance)]
	public abstract class Instance : IInstance
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000238A2 File Offset: 0x000228A2
		private ProvisionFunction PublishFunction
		{
			get
			{
				if (this.publishFunction == null)
				{
					this.publishFunction = Instrumentation.GetPublishFunction(base.GetType());
				}
				return this.publishFunction;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000238C3 File Offset: 0x000228C3
		private ProvisionFunction RevokeFunction
		{
			get
			{
				if (this.revokeFunction == null)
				{
					this.revokeFunction = Instrumentation.GetRevokeFunction(base.GetType());
				}
				return this.revokeFunction;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000238E4 File Offset: 0x000228E4
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000238EC File Offset: 0x000228EC
		[IgnoreMember]
		public bool Published
		{
			get
			{
				return this.published;
			}
			set
			{
				if (this.published && !value)
				{
					this.RevokeFunction(this);
					this.published = false;
					return;
				}
				if (!this.published && value)
				{
					this.PublishFunction(this);
					this.published = true;
				}
			}
		}

		// Token: 0x0400029F RID: 671
		private ProvisionFunction publishFunction;

		// Token: 0x040002A0 RID: 672
		private ProvisionFunction revokeFunction;

		// Token: 0x040002A1 RID: 673
		private bool published;
	}
}

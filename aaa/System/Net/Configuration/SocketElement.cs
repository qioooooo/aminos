using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000668 RID: 1640
	public sealed class SocketElement : ConfigurationElement
	{
		// Token: 0x060032B9 RID: 12985 RVA: 0x000D726C File Offset: 0x000D626C
		public SocketElement()
		{
			this.properties.Add(this.alwaysUseCompletionPortsForAccept);
			this.properties.Add(this.alwaysUseCompletionPortsForConnect);
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000D72F0 File Offset: 0x000D62F0
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnrestrictedSocketPermission.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[] { "socket" }), ex);
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x000D734C File Offset: 0x000D634C
		// (set) Token: 0x060032BC RID: 12988 RVA: 0x000D735F File Offset: 0x000D635F
		[ConfigurationProperty("alwaysUseCompletionPortsForAccept", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForAccept];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForAccept] = value;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x000D7373 File Offset: 0x000D6373
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x000D7386 File Offset: 0x000D6386
		[ConfigurationProperty("alwaysUseCompletionPortsForConnect", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForConnect];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForConnect] = value;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000D739A File Offset: 0x000D639A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F64 RID: 12132
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F65 RID: 12133
		private readonly ConfigurationProperty alwaysUseCompletionPortsForConnect = new ConfigurationProperty("alwaysUseCompletionPortsForConnect", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F66 RID: 12134
		private readonly ConfigurationProperty alwaysUseCompletionPortsForAccept = new ConfigurationProperty("alwaysUseCompletionPortsForAccept", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

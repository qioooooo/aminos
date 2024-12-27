using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000493 RID: 1171
	[ComVisible(true)]
	public class TrustManagerContext
	{
		// Token: 0x06002EE7 RID: 12007 RVA: 0x0009FB09 File Offset: 0x0009EB09
		public TrustManagerContext()
			: this(TrustManagerUIContext.Run)
		{
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x0009FB12 File Offset: 0x0009EB12
		public TrustManagerContext(TrustManagerUIContext uiContext)
		{
			this.m_ignorePersistedDecision = false;
			this.m_uiContext = uiContext;
			this.m_keepAlive = false;
			this.m_persist = true;
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x0009FB36 File Offset: 0x0009EB36
		// (set) Token: 0x06002EEA RID: 12010 RVA: 0x0009FB3E File Offset: 0x0009EB3E
		public virtual TrustManagerUIContext UIContext
		{
			get
			{
				return this.m_uiContext;
			}
			set
			{
				this.m_uiContext = value;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002EEB RID: 12011 RVA: 0x0009FB47 File Offset: 0x0009EB47
		// (set) Token: 0x06002EEC RID: 12012 RVA: 0x0009FB4F File Offset: 0x0009EB4F
		public virtual bool NoPrompt
		{
			get
			{
				return this.m_noPrompt;
			}
			set
			{
				this.m_noPrompt = value;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x0009FB58 File Offset: 0x0009EB58
		// (set) Token: 0x06002EEE RID: 12014 RVA: 0x0009FB60 File Offset: 0x0009EB60
		public virtual bool IgnorePersistedDecision
		{
			get
			{
				return this.m_ignorePersistedDecision;
			}
			set
			{
				this.m_ignorePersistedDecision = value;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x0009FB69 File Offset: 0x0009EB69
		// (set) Token: 0x06002EF0 RID: 12016 RVA: 0x0009FB71 File Offset: 0x0009EB71
		public virtual bool KeepAlive
		{
			get
			{
				return this.m_keepAlive;
			}
			set
			{
				this.m_keepAlive = value;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x0009FB7A File Offset: 0x0009EB7A
		// (set) Token: 0x06002EF2 RID: 12018 RVA: 0x0009FB82 File Offset: 0x0009EB82
		public virtual bool Persist
		{
			get
			{
				return this.m_persist;
			}
			set
			{
				this.m_persist = value;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x0009FB8B File Offset: 0x0009EB8B
		// (set) Token: 0x06002EF4 RID: 12020 RVA: 0x0009FB93 File Offset: 0x0009EB93
		public virtual ApplicationIdentity PreviousApplicationIdentity
		{
			get
			{
				return this.m_appId;
			}
			set
			{
				this.m_appId = value;
			}
		}

		// Token: 0x040017B9 RID: 6073
		private bool m_ignorePersistedDecision;

		// Token: 0x040017BA RID: 6074
		private TrustManagerUIContext m_uiContext;

		// Token: 0x040017BB RID: 6075
		private bool m_noPrompt;

		// Token: 0x040017BC RID: 6076
		private bool m_keepAlive;

		// Token: 0x040017BD RID: 6077
		private bool m_persist;

		// Token: 0x040017BE RID: 6078
		private ApplicationIdentity m_appId;
	}
}

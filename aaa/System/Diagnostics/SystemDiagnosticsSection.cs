using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001D4 RID: 468
	internal class SystemDiagnosticsSection : ConfigurationSection
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x0002E03C File Offset: 0x0002D03C
		static SystemDiagnosticsSection()
		{
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propAssert);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propPerfCounters);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSources);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSharedListeners);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSwitches);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propTrace);
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0002E167 File Offset: 0x0002D167
		[ConfigurationProperty("assert")]
		public AssertSection Assert
		{
			get
			{
				return (AssertSection)base[SystemDiagnosticsSection._propAssert];
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0002E179 File Offset: 0x0002D179
		[ConfigurationProperty("performanceCounters")]
		public PerfCounterSection PerfCounters
		{
			get
			{
				return (PerfCounterSection)base[SystemDiagnosticsSection._propPerfCounters];
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0002E18B File Offset: 0x0002D18B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SystemDiagnosticsSection._properties;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002E192 File Offset: 0x0002D192
		[ConfigurationProperty("sources")]
		public SourceElementsCollection Sources
		{
			get
			{
				return (SourceElementsCollection)base[SystemDiagnosticsSection._propSources];
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0002E1A4 File Offset: 0x0002D1A4
		[ConfigurationProperty("sharedListeners")]
		public ListenerElementsCollection SharedListeners
		{
			get
			{
				return (ListenerElementsCollection)base[SystemDiagnosticsSection._propSharedListeners];
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002E1B6 File Offset: 0x0002D1B6
		[ConfigurationProperty("switches")]
		public SwitchElementsCollection Switches
		{
			get
			{
				return (SwitchElementsCollection)base[SystemDiagnosticsSection._propSwitches];
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0002E1C8 File Offset: 0x0002D1C8
		[ConfigurationProperty("trace")]
		public TraceSection Trace
		{
			get
			{
				return (TraceSection)base[SystemDiagnosticsSection._propTrace];
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0002E1DA File Offset: 0x0002D1DA
		protected override void InitializeDefault()
		{
			this.Trace.Listeners.InitializeDefaultInternal();
		}

		// Token: 0x04000F0C RID: 3852
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000F0D RID: 3853
		private static readonly ConfigurationProperty _propAssert = new ConfigurationProperty("assert", typeof(AssertSection), new AssertSection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F0E RID: 3854
		private static readonly ConfigurationProperty _propPerfCounters = new ConfigurationProperty("performanceCounters", typeof(PerfCounterSection), new PerfCounterSection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F0F RID: 3855
		private static readonly ConfigurationProperty _propSources = new ConfigurationProperty("sources", typeof(SourceElementsCollection), new SourceElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F10 RID: 3856
		private static readonly ConfigurationProperty _propSharedListeners = new ConfigurationProperty("sharedListeners", typeof(SharedListenerElementsCollection), new SharedListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F11 RID: 3857
		private static readonly ConfigurationProperty _propSwitches = new ConfigurationProperty("switches", typeof(SwitchElementsCollection), new SwitchElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F12 RID: 3858
		private static readonly ConfigurationProperty _propTrace = new ConfigurationProperty("trace", typeof(TraceSection), new TraceSection(), ConfigurationPropertyOptions.None);
	}
}

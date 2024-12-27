using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000257 RID: 599
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TraceSection : ConfigurationSection
	{
		// Token: 0x06001FA5 RID: 8101 RVA: 0x0008B960 File Offset: 0x0008A960
		static TraceSection()
		{
			TraceSection._properties.Add(TraceSection._propEnabled);
			TraceSection._properties.Add(TraceSection._propLocalOnly);
			TraceSection._properties.Add(TraceSection._propMostRecent);
			TraceSection._properties.Add(TraceSection._propPageOutput);
			TraceSection._properties.Add(TraceSection._propRequestLimit);
			TraceSection._properties.Add(TraceSection._propMode);
			TraceSection._properties.Add(TraceSection._writeToDiagnosticTrace);
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x0008BACF File Offset: 0x0008AACF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TraceSection._properties;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0008BAD6 File Offset: 0x0008AAD6
		// (set) Token: 0x06001FA9 RID: 8105 RVA: 0x0008BAE8 File Offset: 0x0008AAE8
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get
			{
				return (bool)base[TraceSection._propEnabled];
			}
			set
			{
				base[TraceSection._propEnabled] = value;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0008BAFB File Offset: 0x0008AAFB
		// (set) Token: 0x06001FAB RID: 8107 RVA: 0x0008BB0D File Offset: 0x0008AB0D
		[ConfigurationProperty("mostRecent", DefaultValue = false)]
		public bool MostRecent
		{
			get
			{
				return (bool)base[TraceSection._propMostRecent];
			}
			set
			{
				base[TraceSection._propMostRecent] = value;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x0008BB20 File Offset: 0x0008AB20
		// (set) Token: 0x06001FAD RID: 8109 RVA: 0x0008BB32 File Offset: 0x0008AB32
		[ConfigurationProperty("localOnly", DefaultValue = true)]
		public bool LocalOnly
		{
			get
			{
				return (bool)base[TraceSection._propLocalOnly];
			}
			set
			{
				base[TraceSection._propLocalOnly] = value;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x0008BB45 File Offset: 0x0008AB45
		// (set) Token: 0x06001FAF RID: 8111 RVA: 0x0008BB57 File Offset: 0x0008AB57
		[ConfigurationProperty("pageOutput", DefaultValue = false)]
		public bool PageOutput
		{
			get
			{
				return (bool)base[TraceSection._propPageOutput];
			}
			set
			{
				base[TraceSection._propPageOutput] = value;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0008BB6A File Offset: 0x0008AB6A
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x0008BB7C File Offset: 0x0008AB7C
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("requestLimit", DefaultValue = 10)]
		public int RequestLimit
		{
			get
			{
				return (int)base[TraceSection._propRequestLimit];
			}
			set
			{
				base[TraceSection._propRequestLimit] = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0008BB8F File Offset: 0x0008AB8F
		// (set) Token: 0x06001FB3 RID: 8115 RVA: 0x0008BBA1 File Offset: 0x0008ABA1
		[ConfigurationProperty("traceMode", DefaultValue = TraceDisplayMode.SortByTime)]
		public TraceDisplayMode TraceMode
		{
			get
			{
				return (TraceDisplayMode)base[TraceSection._propMode];
			}
			set
			{
				base[TraceSection._propMode] = value;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0008BBB4 File Offset: 0x0008ABB4
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x0008BBC6 File Offset: 0x0008ABC6
		[ConfigurationProperty("writeToDiagnosticsTrace", DefaultValue = false)]
		public bool WriteToDiagnosticsTrace
		{
			get
			{
				return (bool)base[TraceSection._writeToDiagnosticTrace];
			}
			set
			{
				base[TraceSection._writeToDiagnosticTrace] = value;
			}
		}

		// Token: 0x04001A62 RID: 6754
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A63 RID: 6755
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001A64 RID: 6756
		private static readonly ConfigurationProperty _propLocalOnly = new ConfigurationProperty("localOnly", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001A65 RID: 6757
		private static readonly ConfigurationProperty _propMostRecent = new ConfigurationProperty("mostRecent", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001A66 RID: 6758
		private static readonly ConfigurationProperty _propPageOutput = new ConfigurationProperty("pageOutput", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001A67 RID: 6759
		private static readonly ConfigurationProperty _propRequestLimit = new ConfigurationProperty("requestLimit", typeof(int), 10, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001A68 RID: 6760
		private static readonly ConfigurationProperty _propMode = new ConfigurationProperty("traceMode", typeof(TraceDisplayMode), TraceDisplayMode.SortByTime, ConfigurationPropertyOptions.None);

		// Token: 0x04001A69 RID: 6761
		private static readonly ConfigurationProperty _writeToDiagnosticTrace = new ConfigurationProperty("writeToDiagnosticsTrace", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

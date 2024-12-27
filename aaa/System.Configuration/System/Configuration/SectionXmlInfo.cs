using System;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x02000099 RID: 153
	internal sealed class SectionXmlInfo : IConfigErrorInfo
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x0001C74C File Offset: 0x0001B74C
		internal SectionXmlInfo(string configKey, string definitionConfigPath, string targetConfigPath, string subPath, string filename, int lineNumber, object streamVersion, string rawXml, string configSource, string configSourceStreamName, object configSourceStreamVersion, string protectionProviderName, OverrideModeSetting overrideMode, bool skipInChildApps)
		{
			this._configKey = configKey;
			this._definitionConfigPath = definitionConfigPath;
			this._targetConfigPath = targetConfigPath;
			this._subPath = subPath;
			this._filename = filename;
			this._lineNumber = lineNumber;
			this._streamVersion = streamVersion;
			this._rawXml = rawXml;
			this._configSource = configSource;
			this._configSourceStreamName = configSourceStreamName;
			this._configSourceStreamVersion = configSourceStreamVersion;
			this._protectionProviderName = protectionProviderName;
			this._overrideMode = overrideMode;
			this._skipInChildApps = skipInChildApps;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001C7CC File Offset: 0x0001B7CC
		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001C7D4 File Offset: 0x0001B7D4
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x0001C7DC File Offset: 0x0001B7DC
		public int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0001C7E5 File Offset: 0x0001B7E5
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x0001C7ED File Offset: 0x0001B7ED
		internal object StreamVersion
		{
			get
			{
				return this._streamVersion;
			}
			set
			{
				this._streamVersion = value;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0001C7F6 File Offset: 0x0001B7F6
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x0001C7FE File Offset: 0x0001B7FE
		internal string ConfigSource
		{
			get
			{
				return this._configSource;
			}
			set
			{
				this._configSource = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0001C807 File Offset: 0x0001B807
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0001C80F File Offset: 0x0001B80F
		internal string ConfigSourceStreamName
		{
			get
			{
				return this._configSourceStreamName;
			}
			set
			{
				this._configSourceStreamName = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x0001C818 File Offset: 0x0001B818
		internal object ConfigSourceStreamVersion
		{
			set
			{
				this._configSourceStreamVersion = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0001C821 File Offset: 0x0001B821
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0001C829 File Offset: 0x0001B829
		internal string DefinitionConfigPath
		{
			get
			{
				return this._definitionConfigPath;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001C831 File Offset: 0x0001B831
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001C839 File Offset: 0x0001B839
		internal string TargetConfigPath
		{
			get
			{
				return this._targetConfigPath;
			}
			set
			{
				this._targetConfigPath = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001C842 File Offset: 0x0001B842
		internal string SubPath
		{
			get
			{
				return this._subPath;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001C84A File Offset: 0x0001B84A
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0001C852 File Offset: 0x0001B852
		internal string RawXml
		{
			get
			{
				return this._rawXml;
			}
			set
			{
				this._rawXml = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001C85B File Offset: 0x0001B85B
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0001C863 File Offset: 0x0001B863
		internal string ProtectionProviderName
		{
			get
			{
				return this._protectionProviderName;
			}
			set
			{
				this._protectionProviderName = value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001C86C File Offset: 0x0001B86C
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0001C874 File Offset: 0x0001B874
		internal OverrideModeSetting OverrideModeSetting
		{
			get
			{
				return this._overrideMode;
			}
			set
			{
				this._overrideMode = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001C87D File Offset: 0x0001B87D
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0001C885 File Offset: 0x0001B885
		internal bool SkipInChildApps
		{
			get
			{
				return this._skipInChildApps;
			}
			set
			{
				this._skipInChildApps = value;
			}
		}

		// Token: 0x040003CD RID: 973
		private string _configKey;

		// Token: 0x040003CE RID: 974
		private string _definitionConfigPath;

		// Token: 0x040003CF RID: 975
		private string _targetConfigPath;

		// Token: 0x040003D0 RID: 976
		private string _subPath;

		// Token: 0x040003D1 RID: 977
		private string _filename;

		// Token: 0x040003D2 RID: 978
		private int _lineNumber;

		// Token: 0x040003D3 RID: 979
		private object _streamVersion;

		// Token: 0x040003D4 RID: 980
		private string _configSource;

		// Token: 0x040003D5 RID: 981
		private string _configSourceStreamName;

		// Token: 0x040003D6 RID: 982
		private object _configSourceStreamVersion;

		// Token: 0x040003D7 RID: 983
		private bool _skipInChildApps;

		// Token: 0x040003D8 RID: 984
		private string _rawXml;

		// Token: 0x040003D9 RID: 985
		private string _protectionProviderName;

		// Token: 0x040003DA RID: 986
		private OverrideModeSetting _overrideMode;
	}
}

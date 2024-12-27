using System;

namespace System.Configuration
{
	// Token: 0x0200009B RID: 155
	internal class StreamInfo
	{
		// Token: 0x06000618 RID: 1560 RVA: 0x0001C8D7 File Offset: 0x0001B8D7
		internal StreamInfo(string sectionName, string configSource, string streamName)
		{
			this._sectionName = sectionName;
			this._configSource = configSource;
			this._streamName = streamName;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001C8F4 File Offset: 0x0001B8F4
		private StreamInfo()
		{
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001C8FC File Offset: 0x0001B8FC
		internal StreamInfo Clone()
		{
			return new StreamInfo
			{
				_sectionName = this._sectionName,
				_configSource = this._configSource,
				_streamName = this._streamName,
				_isMonitored = this._isMonitored,
				_version = this._version
			};
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001C94C File Offset: 0x0001B94C
		internal string SectionName
		{
			get
			{
				return this._sectionName;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001C954 File Offset: 0x0001B954
		internal string ConfigSource
		{
			get
			{
				return this._configSource;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001C95C File Offset: 0x0001B95C
		internal string StreamName
		{
			get
			{
				return this._streamName;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001C964 File Offset: 0x0001B964
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0001C96C File Offset: 0x0001B96C
		internal bool IsMonitored
		{
			get
			{
				return this._isMonitored;
			}
			set
			{
				this._isMonitored = value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001C975 File Offset: 0x0001B975
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x0001C97D File Offset: 0x0001B97D
		internal object Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x040003DC RID: 988
		private string _sectionName;

		// Token: 0x040003DD RID: 989
		private string _configSource;

		// Token: 0x040003DE RID: 990
		private string _streamName;

		// Token: 0x040003DF RID: 991
		private bool _isMonitored;

		// Token: 0x040003E0 RID: 992
		private object _version;
	}
}

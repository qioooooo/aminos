using System;

namespace System.Deployment.Application
{
	// Token: 0x02000009 RID: 9
	public class UpdateCheckInfo
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00004C39 File Offset: 0x00003C39
		internal UpdateCheckInfo(bool updateAvailable, Version availableVersion, bool isUpdateRequired, Version minimumRequiredVersion, long updateSize)
		{
			this._updateAvailable = updateAvailable;
			this._availableVersion = availableVersion;
			this._isUpdateRequired = isUpdateRequired;
			this._minimumRequiredVersion = minimumRequiredVersion;
			this._updateSize = updateSize;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00004C66 File Offset: 0x00003C66
		public bool UpdateAvailable
		{
			get
			{
				return this._updateAvailable;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00004C6E File Offset: 0x00003C6E
		public Version AvailableVersion
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._availableVersion;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00004C7C File Offset: 0x00003C7C
		public bool IsUpdateRequired
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._isUpdateRequired;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00004C8A File Offset: 0x00003C8A
		public Version MinimumRequiredVersion
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._minimumRequiredVersion;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004C98 File Offset: 0x00003C98
		public long UpdateSizeBytes
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._updateSize;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004CA6 File Offset: 0x00003CA6
		private void RaiseExceptionIfUpdateNotAvailable()
		{
			if (!this.UpdateAvailable)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_UpdateNotAvailable"));
			}
		}

		// Token: 0x0400003D RID: 61
		private readonly bool _updateAvailable;

		// Token: 0x0400003E RID: 62
		private readonly Version _availableVersion;

		// Token: 0x0400003F RID: 63
		private readonly bool _isUpdateRequired;

		// Token: 0x04000040 RID: 64
		private readonly Version _minimumRequiredVersion;

		// Token: 0x04000041 RID: 65
		private readonly long _updateSize;
	}
}

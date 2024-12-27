using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x0200000B RID: 11
	public class CheckForUpdateCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00004CC0 File Offset: 0x00003CC0
		internal CheckForUpdateCompletedEventArgs(Exception error, bool cancelled, object userState, bool updateAvailable, Version availableVersion, bool isUpdateRequired, Version minimumRequiredVersion, long updateSize)
			: base(error, cancelled, userState)
		{
			this._updateAvailable = updateAvailable;
			this._availableVersion = availableVersion;
			this._isUpdateRequired = isUpdateRequired;
			this._minimumRequiredVersion = minimumRequiredVersion;
			this._updateSize = updateSize;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00004CF3 File Offset: 0x00003CF3
		public bool UpdateAvailable
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._updateAvailable;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004D01 File Offset: 0x00003D01
		public Version AvailableVersion
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._availableVersion;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004D0F File Offset: 0x00003D0F
		public bool IsUpdateRequired
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._isUpdateRequired;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00004D1D File Offset: 0x00003D1D
		public Version MinimumRequiredVersion
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._minimumRequiredVersion;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00004D2B File Offset: 0x00003D2B
		public long UpdateSizeBytes
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._updateSize;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004D39 File Offset: 0x00003D39
		private void RaiseExceptionIfUpdateNotAvailable()
		{
			if (!this.UpdateAvailable)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_UpdateNotAvailable"));
			}
		}

		// Token: 0x04000042 RID: 66
		private readonly bool _updateAvailable;

		// Token: 0x04000043 RID: 67
		private readonly Version _availableVersion;

		// Token: 0x04000044 RID: 68
		private readonly bool _isUpdateRequired;

		// Token: 0x04000045 RID: 69
		private readonly Version _minimumRequiredVersion;

		// Token: 0x04000046 RID: 70
		private readonly long _updateSize;
	}
}

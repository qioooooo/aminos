using System;
using System.Data.Common;

namespace System.Data.Sql
{
	// Token: 0x02000296 RID: 662
	public sealed class SqlNotificationRequest
	{
		// Token: 0x06002265 RID: 8805 RVA: 0x0026D7BC File Offset: 0x0026CBBC
		public SqlNotificationRequest()
			: this(null, null, 0)
		{
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0026D7D4 File Offset: 0x0026CBD4
		public SqlNotificationRequest(string userData, string options, int timeout)
		{
			this.UserData = userData;
			this.Timeout = timeout;
			this.Options = options;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x0026D7FC File Offset: 0x0026CBFC
		// (set) Token: 0x06002268 RID: 8808 RVA: 0x0026D810 File Offset: 0x0026CC10
		public string Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != null && 65535 < value.Length)
				{
					throw ADP.ArgumentOutOfRange(string.Empty, "Service");
				}
				this._options = value;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x0026D844 File Offset: 0x0026CC44
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x0026D858 File Offset: 0x0026CC58
		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				if (0 > value)
				{
					throw ADP.ArgumentOutOfRange(string.Empty, "Timeout");
				}
				this._timeout = value;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x0026D880 File Offset: 0x0026CC80
		// (set) Token: 0x0600226C RID: 8812 RVA: 0x0026D894 File Offset: 0x0026CC94
		public string UserData
		{
			get
			{
				return this._userData;
			}
			set
			{
				if (value != null && 65535 < value.Length)
				{
					throw ADP.ArgumentOutOfRange(string.Empty, "UserData");
				}
				this._userData = value;
			}
		}

		// Token: 0x04001650 RID: 5712
		private string _userData;

		// Token: 0x04001651 RID: 5713
		private string _options;

		// Token: 0x04001652 RID: 5714
		private int _timeout;
	}
}

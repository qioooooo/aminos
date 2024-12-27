using System;

namespace System.Net
{
	// Token: 0x0200037E RID: 894
	public class Authorization
	{
		// Token: 0x06001BE8 RID: 7144 RVA: 0x00069565 File Offset: 0x00068565
		public Authorization(string token)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = true;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00069580 File Offset: 0x00068580
		public Authorization(string token, bool finished)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = finished;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0006959B File Offset: 0x0006859B
		public Authorization(string token, bool finished, string connectionGroupId)
			: this(token, finished, connectionGroupId, false)
		{
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000695A7 File Offset: 0x000685A7
		internal Authorization(string token, bool finished, string connectionGroupId, bool mutualAuth)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_ConnectionGroupId = ValidationHelper.MakeStringNull(connectionGroupId);
			this.m_Complete = finished;
			this.m_MutualAuth = mutualAuth;
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x000695D6 File Offset: 0x000685D6
		public string Message
		{
			get
			{
				return this.m_Message;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x000695DE File Offset: 0x000685DE
		public string ConnectionGroupId
		{
			get
			{
				return this.m_ConnectionGroupId;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x000695E6 File Offset: 0x000685E6
		public bool Complete
		{
			get
			{
				return this.m_Complete;
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000695EE File Offset: 0x000685EE
		internal void SetComplete(bool complete)
		{
			this.m_Complete = complete;
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x000695F7 File Offset: 0x000685F7
		// (set) Token: 0x06001BF1 RID: 7153 RVA: 0x00069600 File Offset: 0x00068600
		public string[] ProtectionRealm
		{
			get
			{
				return this.m_ProtectionRealm;
			}
			set
			{
				string[] array = ValidationHelper.MakeEmptyArrayNull(value);
				this.m_ProtectionRealm = array;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x0006961B File Offset: 0x0006861B
		// (set) Token: 0x06001BF3 RID: 7155 RVA: 0x0006962D File Offset: 0x0006862D
		public bool MutuallyAuthenticated
		{
			get
			{
				return this.Complete && this.m_MutualAuth;
			}
			set
			{
				this.m_MutualAuth = value;
			}
		}

		// Token: 0x04001C8D RID: 7309
		private string m_Message;

		// Token: 0x04001C8E RID: 7310
		private bool m_Complete;

		// Token: 0x04001C8F RID: 7311
		private string[] m_ProtectionRealm;

		// Token: 0x04001C90 RID: 7312
		private string m_ConnectionGroupId;

		// Token: 0x04001C91 RID: 7313
		private bool m_MutualAuth;
	}
}

using System;

namespace System.Configuration
{
	// Token: 0x02000067 RID: 103
	public sealed class ExeContext
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x00013791 File Offset: 0x00012791
		internal ExeContext(ConfigurationUserLevel userContext, string exePath)
		{
			this._userContext = userContext;
			this._exePath = exePath;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000137A7 File Offset: 0x000127A7
		public ConfigurationUserLevel UserLevel
		{
			get
			{
				return this._userContext;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000137AF File Offset: 0x000127AF
		public string ExePath
		{
			get
			{
				return this._exePath;
			}
		}

		// Token: 0x040002FC RID: 764
		private ConfigurationUserLevel _userContext;

		// Token: 0x040002FD RID: 765
		private string _exePath;
	}
}

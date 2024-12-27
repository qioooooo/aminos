using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000B7 RID: 183
	internal class ConfigSystem : IConfigSystem
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0001F774 File Offset: 0x0001E774
		void IConfigSystem.Init(Type typeConfigHost, params object[] hostInitParams)
		{
			this._configRoot = new InternalConfigRoot();
			this._configHost = (IInternalConfigHost)TypeUtil.CreateInstanceWithReflectionPermission(typeConfigHost);
			this._configRoot.Init(this._configHost, false);
			this._configHost.Init(this._configRoot, hostInitParams);
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001F7C1 File Offset: 0x0001E7C1
		IInternalConfigHost IConfigSystem.Host
		{
			get
			{
				return this._configHost;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001F7C9 File Offset: 0x0001E7C9
		IInternalConfigRoot IConfigSystem.Root
		{
			get
			{
				return this._configRoot;
			}
		}

		// Token: 0x0400041A RID: 1050
		private IInternalConfigRoot _configRoot;

		// Token: 0x0400041B RID: 1051
		private IInternalConfigHost _configHost;
	}
}

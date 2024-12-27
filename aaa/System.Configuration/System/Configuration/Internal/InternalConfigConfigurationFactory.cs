using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000BF RID: 191
	internal sealed class InternalConfigConfigurationFactory : IInternalConfigConfigurationFactory
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x0001F8D9 File Offset: 0x0001E8D9
		private InternalConfigConfigurationFactory()
		{
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001F8E1 File Offset: 0x0001E8E1
		Configuration IInternalConfigConfigurationFactory.Create(Type typeConfigHost, params object[] hostInitConfigurationParams)
		{
			return new Configuration(null, typeConfigHost, hostInitConfigurationParams);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001F8EB File Offset: 0x0001E8EB
		string IInternalConfigConfigurationFactory.NormalizeLocationSubPath(string subPath, IConfigErrorInfo errorInfo)
		{
			return BaseConfigurationRecord.NormalizeLocationSubPath(subPath, errorInfo);
		}
	}
}

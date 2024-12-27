using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x020000BC RID: 188
	[ComVisible(false)]
	public interface IInternalConfigConfigurationFactory
	{
		// Token: 0x0600070C RID: 1804
		Configuration Create(Type typeConfigHost, params object[] hostInitConfigurationParams);

		// Token: 0x0600070D RID: 1805
		string NormalizeLocationSubPath(string subPath, IConfigErrorInfo errorInfo);
	}
}

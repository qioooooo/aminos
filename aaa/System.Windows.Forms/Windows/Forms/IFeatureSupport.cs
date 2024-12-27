using System;

namespace System.Windows.Forms
{
	// Token: 0x020003E0 RID: 992
	public interface IFeatureSupport
	{
		// Token: 0x06003B73 RID: 15219
		bool IsPresent(object feature);

		// Token: 0x06003B74 RID: 15220
		bool IsPresent(object feature, Version minimumVersion);

		// Token: 0x06003B75 RID: 15221
		Version GetVersionPresent(object feature);
	}
}

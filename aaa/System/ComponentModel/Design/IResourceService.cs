using System;
using System.Globalization;
using System.Resources;

namespace System.ComponentModel.Design
{
	// Token: 0x0200018C RID: 396
	public interface IResourceService
	{
		// Token: 0x06000C93 RID: 3219
		IResourceReader GetResourceReader(CultureInfo info);

		// Token: 0x06000C94 RID: 3220
		IResourceWriter GetResourceWriter(CultureInfo info);
	}
}

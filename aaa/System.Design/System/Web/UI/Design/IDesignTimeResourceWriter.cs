using System;
using System.Resources;

namespace System.Web.UI.Design
{
	// Token: 0x02000379 RID: 889
	public interface IDesignTimeResourceWriter : IResourceWriter, IDisposable
	{
		// Token: 0x06002120 RID: 8480
		string CreateResourceKey(string resourceName, object obj);
	}
}

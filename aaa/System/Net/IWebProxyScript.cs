using System;

namespace System.Net
{
	// Token: 0x020004B4 RID: 1204
	public interface IWebProxyScript
	{
		// Token: 0x0600253C RID: 9532
		bool Load(Uri scriptLocation, string script, Type helperType);

		// Token: 0x0600253D RID: 9533
		string Run(string url, string host);

		// Token: 0x0600253E RID: 9534
		void Close();
	}
}

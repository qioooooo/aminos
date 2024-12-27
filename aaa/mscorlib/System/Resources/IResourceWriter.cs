using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x0200041B RID: 1051
	[ComVisible(true)]
	public interface IResourceWriter : IDisposable
	{
		// Token: 0x06002B78 RID: 11128
		void AddResource(string name, string value);

		// Token: 0x06002B79 RID: 11129
		void AddResource(string name, object value);

		// Token: 0x06002B7A RID: 11130
		void AddResource(string name, byte[] value);

		// Token: 0x06002B7B RID: 11131
		void Close();

		// Token: 0x06002B7C RID: 11132
		void Generate();
	}
}

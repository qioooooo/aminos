using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000021 RID: 33
	[Guid("6619a740-8154-43be-a186-0319578e02db")]
	public interface IRemoteDispatch
	{
		// Token: 0x06000067 RID: 103
		[AutoComplete(true)]
		string RemoteDispatchAutoDone(string s);

		// Token: 0x06000068 RID: 104
		[AutoComplete(false)]
		string RemoteDispatchNotAutoDone(string s);
	}
}

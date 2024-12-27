using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x02000500 RID: 1280
	internal struct AddressInfo
	{
		// Token: 0x04002721 RID: 10017
		internal AddressInfoHints ai_flags;

		// Token: 0x04002722 RID: 10018
		internal AddressFamily ai_family;

		// Token: 0x04002723 RID: 10019
		internal SocketType ai_socktype;

		// Token: 0x04002724 RID: 10020
		internal ProtocolFamily ai_protocol;

		// Token: 0x04002725 RID: 10021
		internal int ai_addrlen;

		// Token: 0x04002726 RID: 10022
		internal unsafe sbyte* ai_canonname;

		// Token: 0x04002727 RID: 10023
		internal unsafe byte* ai_addr;

		// Token: 0x04002728 RID: 10024
		internal unsafe AddressInfo* ai_next;
	}
}

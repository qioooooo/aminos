using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x020000FF RID: 255
	[Guid("5B807FA1-00CD-46ee-A493-FD80AC944715")]
	[ComVisible(true)]
	public interface IRedirectOutput
	{
		// Token: 0x06000AED RID: 2797
		void SetOutputStream(IMessageReceiver output);
	}
}

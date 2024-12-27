using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000100 RID: 256
	[Guid("F062C7FB-53BF-4f0d-B0F6-D66C5948E63F")]
	[ComVisible(true)]
	public interface IMessageReceiver
	{
		// Token: 0x06000AEE RID: 2798
		void Message(string strValue);
	}
}

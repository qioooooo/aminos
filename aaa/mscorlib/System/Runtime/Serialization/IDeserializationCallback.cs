using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000035 RID: 53
	[ComVisible(true)]
	public interface IDeserializationCallback
	{
		// Token: 0x0600031F RID: 799
		void OnDeserialization(object sender);
	}
}

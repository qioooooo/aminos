using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035E RID: 862
	[ComVisible(true)]
	[Serializable]
	public abstract class SerializationBinder
	{
		// Token: 0x0600223F RID: 8767
		public abstract Type BindToType(string assemblyName, string typeName);
	}
}

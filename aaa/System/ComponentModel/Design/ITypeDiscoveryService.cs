using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x02000191 RID: 401
	public interface ITypeDiscoveryService
	{
		// Token: 0x06000CA6 RID: 3238
		ICollection GetTypes(Type baseType, bool excludeGlobalTypes);
	}
}

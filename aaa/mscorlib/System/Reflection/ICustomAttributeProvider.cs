using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020000EE RID: 238
	[ComVisible(true)]
	public interface ICustomAttributeProvider
	{
		// Token: 0x06000C9B RID: 3227
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000C9C RID: 3228
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000C9D RID: 3229
		bool IsDefined(Type attributeType, bool inherit);
	}
}

using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000006 RID: 6
	public interface IActivationObject
	{
		// Token: 0x06000032 RID: 50
		object GetDefaultThisObject();

		// Token: 0x06000033 RID: 51
		GlobalScope GetGlobalScope();

		// Token: 0x06000034 RID: 52
		FieldInfo GetLocalField(string name);

		// Token: 0x06000035 RID: 53
		object GetMemberValue(string name, int lexlevel);

		// Token: 0x06000036 RID: 54
		FieldInfo GetField(string name, int lexLevel);
	}
}

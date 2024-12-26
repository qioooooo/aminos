using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000066 RID: 102
	[Guid("8E93D770-6168-4b68-B896-A71B74C7076A")]
	[ComVisible(true)]
	public interface IDebuggerObject
	{
		// Token: 0x06000505 RID: 1285
		bool IsCOMObject();

		// Token: 0x06000506 RID: 1286
		bool IsEqual(IDebuggerObject o);

		// Token: 0x06000507 RID: 1287
		bool HasEnumerableMember(string name);

		// Token: 0x06000508 RID: 1288
		bool IsScriptFunction();

		// Token: 0x06000509 RID: 1289
		bool IsScriptObject();
	}
}

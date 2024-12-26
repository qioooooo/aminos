using System;

namespace Microsoft.JScript
{
	// Token: 0x020000C3 RID: 195
	public class JSPrototypeObject : JSObject
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x00041E32 File Offset: 0x00040E32
		internal JSPrototypeObject(ScriptObject parent, ScriptFunction constructor)
			: base(parent, typeof(JSPrototypeObject))
		{
			this.constructor = constructor;
			this.noExpando = false;
		}

		// Token: 0x040004B1 RID: 1201
		public object constructor;
	}
}

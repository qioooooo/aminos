using System;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000EA RID: 234
	public sealed class Namespace
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x0004F2AC File Offset: 0x0004E2AC
		private Namespace(string name, VsaEngine engine)
		{
			this.name = name;
			this.engine = engine;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0004F2C2 File Offset: 0x0004E2C2
		public static Namespace GetNamespace(string name, VsaEngine engine)
		{
			return new Namespace(name, engine);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0004F2CB File Offset: 0x0004E2CB
		internal Type GetType(string typeName)
		{
			return this.engine.GetType(typeName);
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0004F2D9 File Offset: 0x0004E2D9
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0400066E RID: 1646
		private string name;

		// Token: 0x0400066F RID: 1647
		internal VsaEngine engine;
	}
}

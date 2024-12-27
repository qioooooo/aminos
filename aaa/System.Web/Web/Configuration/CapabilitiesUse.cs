using System;

namespace System.Web.Configuration
{
	// Token: 0x020001B7 RID: 439
	internal class CapabilitiesUse : CapabilitiesRule
	{
		// Token: 0x06001939 RID: 6457 RVA: 0x00078805 File Offset: 0x00077805
		internal CapabilitiesUse(string var, string asParam)
		{
			this._var = var;
			this._as = asParam;
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0007881B File Offset: 0x0007781B
		internal override void Evaluate(CapabilitiesState state)
		{
			state.SetVariable(this._as, state.ResolveServerVariable(this._var));
			state.Exit = false;
		}

		// Token: 0x04001722 RID: 5922
		internal string _var;

		// Token: 0x04001723 RID: 5923
		internal string _as;
	}
}

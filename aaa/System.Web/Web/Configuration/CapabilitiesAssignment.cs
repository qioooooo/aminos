using System;

namespace System.Web.Configuration
{
	// Token: 0x020001B3 RID: 435
	internal class CapabilitiesAssignment : CapabilitiesRule
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0007824C File Offset: 0x0007724C
		internal CapabilitiesAssignment(string var, CapabilitiesPattern pat)
		{
			this._type = 1;
			this._var = var;
			this._pat = pat;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x00078269 File Offset: 0x00077269
		internal override void Evaluate(CapabilitiesState state)
		{
			state.SetVariable(this._var, this._pat.Expand(state));
			state.Exit = false;
		}

		// Token: 0x0400170D RID: 5901
		internal string _var;

		// Token: 0x0400170E RID: 5902
		internal CapabilitiesPattern _pat;
	}
}

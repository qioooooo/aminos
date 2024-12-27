using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x020001B5 RID: 437
	internal class CapabilitiesSection : CapabilitiesRule
	{
		// Token: 0x0600192A RID: 6442 RVA: 0x0007854B File Offset: 0x0007754B
		internal CapabilitiesSection(int type, DelayedRegex regex, CapabilitiesPattern expr, ArrayList rulelist)
		{
			this._type = type;
			this._regex = regex;
			this._expr = expr;
			this._rules = (CapabilitiesRule[])rulelist.ToArray(typeof(CapabilitiesRule));
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00078584 File Offset: 0x00077584
		internal override void Evaluate(CapabilitiesState state)
		{
			state.Exit = false;
			if (this._regex != null)
			{
				Match match = this._regex.Match(this._expr.Expand(state));
				if (!match.Success)
				{
					return;
				}
				state.AddMatch(this._regex, match);
			}
			for (int i = 0; i < this._rules.Length; i++)
			{
				this._rules[i].Evaluate(state);
				if (state.Exit)
				{
					break;
				}
			}
			if (this._regex != null)
			{
				state.PopMatch();
			}
			state.Exit = this.Type == 3;
		}

		// Token: 0x04001719 RID: 5913
		internal CapabilitiesPattern _expr;

		// Token: 0x0400171A RID: 5914
		internal DelayedRegex _regex;

		// Token: 0x0400171B RID: 5915
		internal CapabilitiesRule[] _rules;
	}
}

using System;
using System.Collections;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x020001B6 RID: 438
	internal class CapabilitiesState
	{
		// Token: 0x0600192C RID: 6444 RVA: 0x00078614 File Offset: 0x00077614
		internal CapabilitiesState(HttpRequest request, IDictionary values)
		{
			this._request = request;
			this._values = values;
			this._matchlist = new ArrayList();
			this._regexlist = new ArrayList();
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00078640 File Offset: 0x00077640
		// (set) Token: 0x0600192E RID: 6446 RVA: 0x00078648 File Offset: 0x00077648
		internal bool EvaluateOnlyUserAgent
		{
			get
			{
				return this._evaluateOnlyUserAgent;
			}
			set
			{
				this._evaluateOnlyUserAgent = value;
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00078651 File Offset: 0x00077651
		internal virtual void ClearMatch()
		{
			if (this._matchlist == null)
			{
				this._regexlist = new ArrayList();
				this._matchlist = new ArrayList();
				return;
			}
			this._regexlist.Clear();
			this._matchlist.Clear();
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00078688 File Offset: 0x00077688
		internal virtual void AddMatch(DelayedRegex regex, Match match)
		{
			this._regexlist.Add(regex);
			this._matchlist.Add(match);
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x000786A4 File Offset: 0x000776A4
		internal virtual void PopMatch()
		{
			this._regexlist.RemoveAt(this._regexlist.Count - 1);
			this._matchlist.RemoveAt(this._matchlist.Count - 1);
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x000786D8 File Offset: 0x000776D8
		internal virtual string ResolveReference(string refname)
		{
			if (this._matchlist == null)
			{
				return string.Empty;
			}
			int i = this._matchlist.Count;
			while (i > 0)
			{
				i--;
				int num = ((DelayedRegex)this._regexlist[i]).GroupNumberFromName(refname);
				if (num >= 0)
				{
					Group group = ((Match)this._matchlist[i]).Groups[num];
					if (group.Success)
					{
						return group.ToString();
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00078758 File Offset: 0x00077758
		[AspNetHostingPermission(SecurityAction.Assert, Level = AspNetHostingPermissionLevel.Low)]
		private string ResolveServerVariableWithAssert(string varname)
		{
			string text = this._request.ServerVariables[varname];
			if (text == null)
			{
				return string.Empty;
			}
			return text;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00078781 File Offset: 0x00077781
		internal virtual string ResolveServerVariable(string varname)
		{
			if (varname.Length == 0 || varname == "HTTP_USER_AGENT")
			{
				return HttpCapabilitiesEvaluator.GetUserAgent(this._request);
			}
			if (this.EvaluateOnlyUserAgent)
			{
				return string.Empty;
			}
			return this.ResolveServerVariableWithAssert(varname);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000787BC File Offset: 0x000777BC
		internal virtual string ResolveVariable(string varname)
		{
			string text = (string)this._values[varname];
			if (text == null)
			{
				return string.Empty;
			}
			return text;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000787E5 File Offset: 0x000777E5
		internal virtual void SetVariable(string varname, string value)
		{
			this._values[varname] = value;
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x000787F4 File Offset: 0x000777F4
		// (set) Token: 0x06001938 RID: 6456 RVA: 0x000787FC File Offset: 0x000777FC
		internal virtual bool Exit
		{
			get
			{
				return this._exit;
			}
			set
			{
				this._exit = value;
			}
		}

		// Token: 0x0400171C RID: 5916
		internal HttpRequest _request;

		// Token: 0x0400171D RID: 5917
		internal IDictionary _values;

		// Token: 0x0400171E RID: 5918
		internal ArrayList _matchlist;

		// Token: 0x0400171F RID: 5919
		internal ArrayList _regexlist;

		// Token: 0x04001720 RID: 5920
		internal bool _exit;

		// Token: 0x04001721 RID: 5921
		internal bool _evaluateOnlyUserAgent;
	}
}

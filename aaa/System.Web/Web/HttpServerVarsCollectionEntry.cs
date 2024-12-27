using System;

namespace System.Web
{
	// Token: 0x02000091 RID: 145
	internal class HttpServerVarsCollectionEntry
	{
		// Token: 0x060007A2 RID: 1954 RVA: 0x0002288F File Offset: 0x0002188F
		internal HttpServerVarsCollectionEntry(string name, string value)
		{
			this.Name = name;
			this.Value = value;
			this.IsDynamic = false;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000228AC File Offset: 0x000218AC
		internal HttpServerVarsCollectionEntry(string name, DynamicServerVariable var)
		{
			this.Name = name;
			this.Var = var;
			this.IsDynamic = true;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000228CC File Offset: 0x000218CC
		internal string GetValue(HttpRequest request)
		{
			string text = null;
			if (this.IsDynamic)
			{
				if (request != null)
				{
					text = request.CalcDynamicServerVariable(this.Var);
				}
			}
			else
			{
				text = this.Value;
			}
			return text;
		}

		// Token: 0x0400115C RID: 4444
		internal readonly string Name;

		// Token: 0x0400115D RID: 4445
		internal readonly bool IsDynamic;

		// Token: 0x0400115E RID: 4446
		internal readonly string Value;

		// Token: 0x0400115F RID: 4447
		internal readonly DynamicServerVariable Var;
	}
}

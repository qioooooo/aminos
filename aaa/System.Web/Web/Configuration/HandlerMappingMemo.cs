using System;

namespace System.Web.Configuration
{
	// Token: 0x020001E9 RID: 489
	internal class HandlerMappingMemo
	{
		// Token: 0x06001B0B RID: 6923 RVA: 0x0007D1BB File Offset: 0x0007C1BB
		internal HandlerMappingMemo(HttpHandlerAction mapping, string verb, VirtualPath path)
		{
			this._mapping = mapping;
			this._verb = verb;
			this._path = path;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0007D1D8 File Offset: 0x0007C1D8
		internal bool IsMatch(string verb, VirtualPath path)
		{
			return this._verb.Equals(verb) && this._path.Equals(path);
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0007D1F6 File Offset: 0x0007C1F6
		internal HttpHandlerAction Mapping
		{
			get
			{
				return this._mapping;
			}
		}

		// Token: 0x04001814 RID: 6164
		private HttpHandlerAction _mapping;

		// Token: 0x04001815 RID: 6165
		private string _verb;

		// Token: 0x04001816 RID: 6166
		private VirtualPath _path;
	}
}

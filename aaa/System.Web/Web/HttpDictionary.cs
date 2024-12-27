using System;
using System.Collections.Specialized;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200006A RID: 106
	internal class HttpDictionary : NameObjectCollectionBase
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x00013BFA File Offset: 0x00012BFA
		internal HttpDictionary()
			: base(Misc.CaseInsensitiveInvariantKeyComparer)
		{
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00013C07 File Offset: 0x00012C07
		internal int Size
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00013C0F File Offset: 0x00012C0F
		internal object GetValue(string key)
		{
			return base.BaseGet(key);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00013C18 File Offset: 0x00012C18
		internal void SetValue(string key, object value)
		{
			base.BaseSet(key, value);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00013C22 File Offset: 0x00012C22
		internal object GetValue(int index)
		{
			return base.BaseGet(index);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00013C2B File Offset: 0x00012C2B
		internal string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00013C34 File Offset: 0x00012C34
		internal string[] GetAllKeys()
		{
			return base.BaseGetAllKeys();
		}
	}
}

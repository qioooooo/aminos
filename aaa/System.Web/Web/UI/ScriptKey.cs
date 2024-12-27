using System;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003AC RID: 940
	internal class ScriptKey
	{
		// Token: 0x06002E26 RID: 11814 RVA: 0x000CF2D9 File Offset: 0x000CE2D9
		internal ScriptKey(Type type, string key)
			: this(type, key, false)
		{
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000CF2E4 File Offset: 0x000CE2E4
		internal ScriptKey(Type type, string key, bool isInclude)
		{
			this._type = type;
			if (string.IsNullOrEmpty(key))
			{
				key = null;
			}
			this._key = key;
			this._isInclude = isInclude;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000CF30C File Offset: 0x000CE30C
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this._type.GetHashCode(), this._key.GetHashCode(), this._isInclude.GetHashCode());
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000CF334 File Offset: 0x000CE334
		public override bool Equals(object o)
		{
			ScriptKey scriptKey = (ScriptKey)o;
			return scriptKey._type == this._type && scriptKey._key == this._key && scriptKey._isInclude == this._isInclude;
		}

		// Token: 0x0400216A RID: 8554
		private Type _type;

		// Token: 0x0400216B RID: 8555
		private string _key;

		// Token: 0x0400216C RID: 8556
		private bool _isInclude;
	}
}

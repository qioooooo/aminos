using System;

namespace System.Web.Caching
{
	// Token: 0x02000107 RID: 263
	internal class CacheKey
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x00031093 File Offset: 0x00030093
		internal CacheKey(string key, bool isPublic)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._key = key;
			if (isPublic)
			{
				this._bits = 32;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x000310BB File Offset: 0x000300BB
		internal string Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x000310C3 File Offset: 0x000300C3
		internal bool IsPublic
		{
			get
			{
				return (this._bits & 32) != 0;
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000310D4 File Offset: 0x000300D4
		public override int GetHashCode()
		{
			if (this._hashCode == 0)
			{
				this._hashCode = this._key.GetHashCode();
			}
			return this._hashCode;
		}

		// Token: 0x0400140D RID: 5133
		protected const byte BitPublic = 32;

		// Token: 0x0400140E RID: 5134
		protected string _key;

		// Token: 0x0400140F RID: 5135
		protected byte _bits;

		// Token: 0x04001410 RID: 5136
		private int _hashCode;
	}
}

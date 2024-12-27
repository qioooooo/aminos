using System;
using System.Web.Util;

namespace System.Web.Caching
{
	// Token: 0x020000B1 RID: 177
	internal class CachedVary
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x000264D1 File Offset: 0x000254D1
		internal CachedVary(string[] contentEncodings, string[] headers, string[] parameters, bool varyByAllParams, string varyByCustom)
		{
			this._contentEncodings = contentEncodings;
			this._headers = headers;
			this._params = parameters;
			this._varyByAllParams = varyByAllParams;
			this._varyByCustom = varyByCustom;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00026500 File Offset: 0x00025500
		public override bool Equals(object obj)
		{
			if (!(obj is CachedVary))
			{
				return false;
			}
			CachedVary cachedVary = (CachedVary)obj;
			return this._varyByAllParams == cachedVary._varyByAllParams && this._varyByCustom == cachedVary._varyByCustom && StringUtil.StringArrayEquals(this._contentEncodings, cachedVary._contentEncodings) && StringUtil.StringArrayEquals(this._headers, cachedVary._headers) && StringUtil.StringArrayEquals(this._params, cachedVary._params);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00026578 File Offset: 0x00025578
		public override int GetHashCode()
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddObject(this._varyByAllParams);
			hashCodeCombiner.AddObject(this._varyByCustom);
			hashCodeCombiner.AddArray(this._contentEncodings);
			hashCodeCombiner.AddArray(this._headers);
			hashCodeCombiner.AddArray(this._params);
			return hashCodeCombiner.CombinedHash32;
		}

		// Token: 0x040011CC RID: 4556
		internal readonly string[] _contentEncodings;

		// Token: 0x040011CD RID: 4557
		internal readonly string[] _headers;

		// Token: 0x040011CE RID: 4558
		internal readonly string[] _params;

		// Token: 0x040011CF RID: 4559
		internal readonly string _varyByCustom;

		// Token: 0x040011D0 RID: 4560
		internal readonly bool _varyByAllParams;
	}
}

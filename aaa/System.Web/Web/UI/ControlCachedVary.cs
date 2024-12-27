using System;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000456 RID: 1110
	internal class ControlCachedVary
	{
		// Token: 0x060034BF RID: 13503 RVA: 0x000E49C6 File Offset: 0x000E39C6
		internal ControlCachedVary(string[] varyByParams, string[] varyByControls, string varyByCustom)
		{
			this._varyByParams = varyByParams;
			this._varyByControls = varyByControls;
			this._varyByCustom = varyByCustom;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000E49E4 File Offset: 0x000E39E4
		public override bool Equals(object obj)
		{
			if (!(obj is ControlCachedVary))
			{
				return false;
			}
			ControlCachedVary controlCachedVary = (ControlCachedVary)obj;
			return this._varyByCustom == controlCachedVary._varyByCustom && StringUtil.StringArrayEquals(this._varyByParams, controlCachedVary._varyByParams) && StringUtil.StringArrayEquals(this._varyByControls, controlCachedVary._varyByControls);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000E4A3C File Offset: 0x000E3A3C
		public override int GetHashCode()
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddObject(this._varyByCustom);
			hashCodeCombiner.AddArray(this._varyByParams);
			hashCodeCombiner.AddArray(this._varyByControls);
			return hashCodeCombiner.CombinedHash32;
		}

		// Token: 0x040024E9 RID: 9449
		internal readonly string[] _varyByParams;

		// Token: 0x040024EA RID: 9450
		internal readonly string _varyByCustom;

		// Token: 0x040024EB RID: 9451
		internal readonly string[] _varyByControls;
	}
}

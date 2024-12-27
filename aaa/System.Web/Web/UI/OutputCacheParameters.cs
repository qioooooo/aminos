using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200043A RID: 1082
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class OutputCacheParameters
	{
		// Token: 0x060033AB RID: 13227 RVA: 0x000E0E7F File Offset: 0x000DFE7F
		internal bool IsParameterSet(OutputCacheParameter value)
		{
			return this._flags[(int)value];
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x000E0E8D File Offset: 0x000DFE8D
		// (set) Token: 0x060033AD RID: 13229 RVA: 0x000E0E95 File Offset: 0x000DFE95
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._flags[4] = true;
				this._enabled = value;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x000E0EAB File Offset: 0x000DFEAB
		// (set) Token: 0x060033AF RID: 13231 RVA: 0x000E0EB3 File Offset: 0x000DFEB3
		public int Duration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._flags[2] = true;
				this._duration = value;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x000E0EC9 File Offset: 0x000DFEC9
		// (set) Token: 0x060033B1 RID: 13233 RVA: 0x000E0ED1 File Offset: 0x000DFED1
		public OutputCacheLocation Location
		{
			get
			{
				return this._location;
			}
			set
			{
				this._flags[8] = true;
				this._location = value;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x000E0EE7 File Offset: 0x000DFEE7
		// (set) Token: 0x060033B3 RID: 13235 RVA: 0x000E0EEF File Offset: 0x000DFEEF
		public string VaryByCustom
		{
			get
			{
				return this._varyByCustom;
			}
			set
			{
				this._flags[128] = true;
				this._varyByCustom = value;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000E0F09 File Offset: 0x000DFF09
		// (set) Token: 0x060033B5 RID: 13237 RVA: 0x000E0F11 File Offset: 0x000DFF11
		public string VaryByParam
		{
			get
			{
				return this._varyByParam;
			}
			set
			{
				this._flags[512] = true;
				this._varyByParam = value;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060033B6 RID: 13238 RVA: 0x000E0F2B File Offset: 0x000DFF2B
		// (set) Token: 0x060033B7 RID: 13239 RVA: 0x000E0F33 File Offset: 0x000DFF33
		public string VaryByContentEncoding
		{
			get
			{
				return this._varyByContentEncoding;
			}
			set
			{
				this._flags[1024] = true;
				this._varyByContentEncoding = value;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000E0F4D File Offset: 0x000DFF4D
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x000E0F55 File Offset: 0x000DFF55
		public string VaryByHeader
		{
			get
			{
				return this._varyByHeader;
			}
			set
			{
				this._flags[256] = true;
				this._varyByHeader = value;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000E0F6F File Offset: 0x000DFF6F
		// (set) Token: 0x060033BB RID: 13243 RVA: 0x000E0F77 File Offset: 0x000DFF77
		public bool NoStore
		{
			get
			{
				return this._noStore;
			}
			set
			{
				this._flags[16] = true;
				this._noStore = value;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x000E0F8E File Offset: 0x000DFF8E
		// (set) Token: 0x060033BD RID: 13245 RVA: 0x000E0F96 File Offset: 0x000DFF96
		public string SqlDependency
		{
			get
			{
				return this._sqlDependency;
			}
			set
			{
				this._flags[32] = true;
				this._sqlDependency = value;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x000E0FAD File Offset: 0x000DFFAD
		// (set) Token: 0x060033BF RID: 13247 RVA: 0x000E0FB5 File Offset: 0x000DFFB5
		public string VaryByControl
		{
			get
			{
				return this._varyByControl;
			}
			set
			{
				this._flags[64] = true;
				this._varyByControl = value;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000E0FCC File Offset: 0x000DFFCC
		// (set) Token: 0x060033C1 RID: 13249 RVA: 0x000E0FD4 File Offset: 0x000DFFD4
		public string CacheProfile
		{
			get
			{
				return this._cacheProfile;
			}
			set
			{
				this._flags[1] = true;
				this._cacheProfile = value;
			}
		}

		// Token: 0x0400244F RID: 9295
		private SimpleBitVector32 _flags;

		// Token: 0x04002450 RID: 9296
		private bool _enabled = true;

		// Token: 0x04002451 RID: 9297
		private int _duration;

		// Token: 0x04002452 RID: 9298
		private OutputCacheLocation _location;

		// Token: 0x04002453 RID: 9299
		private string _varyByCustom;

		// Token: 0x04002454 RID: 9300
		private string _varyByParam;

		// Token: 0x04002455 RID: 9301
		private string _varyByContentEncoding;

		// Token: 0x04002456 RID: 9302
		private string _varyByHeader;

		// Token: 0x04002457 RID: 9303
		private bool _noStore;

		// Token: 0x04002458 RID: 9304
		private string _sqlDependency;

		// Token: 0x04002459 RID: 9305
		private string _varyByControl;

		// Token: 0x0400245A RID: 9306
		private string _cacheProfile;
	}
}

using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F9 RID: 1785
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class SharedPersonalizationStateInfo : PersonalizationStateInfo
	{
		// Token: 0x06005741 RID: 22337 RVA: 0x0015F710 File Offset: 0x0015E710
		public SharedPersonalizationStateInfo(string path, DateTime lastUpdatedDate, int size, int sizeOfPersonalizations, int countOfPersonalizations)
			: base(path, lastUpdatedDate, size)
		{
			PersonalizationProviderHelper.CheckNegativeInteger(sizeOfPersonalizations, "sizeOfPersonalizations");
			PersonalizationProviderHelper.CheckNegativeInteger(countOfPersonalizations, "countOfPersonalizations");
			this._sizeOfPersonalizations = sizeOfPersonalizations;
			this._countOfPersonalizations = countOfPersonalizations;
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x0015F743 File Offset: 0x0015E743
		public int SizeOfPersonalizations
		{
			get
			{
				return this._sizeOfPersonalizations;
			}
		}

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x0015F74B File Offset: 0x0015E74B
		public int CountOfPersonalizations
		{
			get
			{
				return this._countOfPersonalizations;
			}
		}

		// Token: 0x04002F91 RID: 12177
		private int _sizeOfPersonalizations;

		// Token: 0x04002F92 RID: 12178
		private int _countOfPersonalizations;
	}
}

using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E4 RID: 1764
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public abstract class PersonalizationStateInfo
	{
		// Token: 0x06005686 RID: 22150 RVA: 0x0015D45A File Offset: 0x0015C45A
		internal PersonalizationStateInfo(string path, DateTime lastUpdatedDate, int size)
		{
			this._path = StringUtil.CheckAndTrimString(path, "path");
			PersonalizationProviderHelper.CheckNegativeInteger(size, "size");
			this._lastUpdatedDate = lastUpdatedDate.ToUniversalTime();
			this._size = size;
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x0015D492 File Offset: 0x0015C492
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x06005688 RID: 22152 RVA: 0x0015D49A File Offset: 0x0015C49A
		public DateTime LastUpdatedDate
		{
			get
			{
				return this._lastUpdatedDate.ToLocalTime();
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06005689 RID: 22153 RVA: 0x0015D4A7 File Offset: 0x0015C4A7
		public int Size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x04002F61 RID: 12129
		private string _path;

		// Token: 0x04002F62 RID: 12130
		private DateTime _lastUpdatedDate;

		// Token: 0x04002F63 RID: 12131
		private int _size;
	}
}

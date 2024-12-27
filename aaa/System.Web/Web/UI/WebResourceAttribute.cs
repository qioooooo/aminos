using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200048D RID: 1165
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebResourceAttribute : Attribute
	{
		// Token: 0x060036A0 RID: 13984 RVA: 0x000EB94C File Offset: 0x000EA94C
		public WebResourceAttribute(string webResource, string contentType)
		{
			if (string.IsNullOrEmpty(webResource))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("webResource");
			}
			if (string.IsNullOrEmpty(contentType))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("contentType");
			}
			this._contentType = contentType;
			this._webResource = webResource;
			this._performSubstitution = false;
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x000EB99A File Offset: 0x000EA99A
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x000EB9A2 File Offset: 0x000EA9A2
		// (set) Token: 0x060036A3 RID: 13987 RVA: 0x000EB9AA File Offset: 0x000EA9AA
		public bool PerformSubstitution
		{
			get
			{
				return this._performSubstitution;
			}
			set
			{
				this._performSubstitution = value;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x000EB9B3 File Offset: 0x000EA9B3
		public string WebResource
		{
			get
			{
				return this._webResource;
			}
		}

		// Token: 0x040025A8 RID: 9640
		private string _contentType;

		// Token: 0x040025A9 RID: 9641
		private bool _performSubstitution;

		// Token: 0x040025AA RID: 9642
		private string _webResource;
	}
}

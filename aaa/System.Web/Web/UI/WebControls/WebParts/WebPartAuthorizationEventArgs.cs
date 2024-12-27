using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000708 RID: 1800
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartAuthorizationEventArgs : EventArgs
	{
		// Token: 0x060057B4 RID: 22452 RVA: 0x0016145E File Offset: 0x0016045E
		public WebPartAuthorizationEventArgs(Type type, string path, string authorizationFilter, bool isShared)
		{
			this._type = type;
			this._path = path;
			this._authorizationFilter = authorizationFilter;
			this._isShared = isShared;
			this._isAuthorized = true;
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x060057B5 RID: 22453 RVA: 0x0016148A File Offset: 0x0016048A
		public string AuthorizationFilter
		{
			get
			{
				return this._authorizationFilter;
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x060057B6 RID: 22454 RVA: 0x00161492 File Offset: 0x00160492
		// (set) Token: 0x060057B7 RID: 22455 RVA: 0x0016149A File Offset: 0x0016049A
		public bool IsAuthorized
		{
			get
			{
				return this._isAuthorized;
			}
			set
			{
				this._isAuthorized = value;
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x060057B8 RID: 22456 RVA: 0x001614A3 File Offset: 0x001604A3
		public bool IsShared
		{
			get
			{
				return this._isShared;
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x060057B9 RID: 22457 RVA: 0x001614AB File Offset: 0x001604AB
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x001614B3 File Offset: 0x001604B3
		public Type Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04002FB0 RID: 12208
		private Type _type;

		// Token: 0x04002FB1 RID: 12209
		private string _path;

		// Token: 0x04002FB2 RID: 12210
		private string _authorizationFilter;

		// Token: 0x04002FB3 RID: 12211
		private bool _isShared;

		// Token: 0x04002FB4 RID: 12212
		private bool _isAuthorized;
	}
}

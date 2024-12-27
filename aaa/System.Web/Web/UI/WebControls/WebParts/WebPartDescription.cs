using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200071D RID: 1821
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartDescription
	{
		// Token: 0x06005867 RID: 22631 RVA: 0x00163935 File Offset: 0x00162935
		private WebPartDescription()
		{
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x00163940 File Offset: 0x00162940
		public WebPartDescription(string id, string title, string description, string imageUrl)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id");
			}
			if (string.IsNullOrEmpty(title))
			{
				throw new ArgumentNullException("title");
			}
			this._id = id;
			this._title = title;
			this._description = ((description != null) ? description : string.Empty);
			this._imageUrl = ((imageUrl != null) ? imageUrl : string.Empty);
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x001639AC File Offset: 0x001629AC
		public WebPartDescription(WebPart part)
		{
			string id = part.ID;
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException(SR.GetString("WebPartManager_NoWebPartID"), "part");
			}
			this._id = id;
			string displayTitle = part.DisplayTitle;
			this._title = ((displayTitle != null) ? displayTitle : string.Empty);
			string description = part.Description;
			this._description = ((description != null) ? description : string.Empty);
			string catalogIconImageUrl = part.CatalogIconImageUrl;
			this._imageUrl = ((catalogIconImageUrl != null) ? catalogIconImageUrl : string.Empty);
			this._part = part;
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x0600586A RID: 22634 RVA: 0x00163A39 File Offset: 0x00162A39
		public string CatalogIconImageUrl
		{
			get
			{
				return this._imageUrl;
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x0600586B RID: 22635 RVA: 0x00163A41 File Offset: 0x00162A41
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x0600586C RID: 22636 RVA: 0x00163A49 File Offset: 0x00162A49
		public string ID
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x00163A51 File Offset: 0x00162A51
		public string Title
		{
			get
			{
				return this._title;
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x0600586E RID: 22638 RVA: 0x00163A59 File Offset: 0x00162A59
		internal WebPart WebPart
		{
			get
			{
				return this._part;
			}
		}

		// Token: 0x04002FE1 RID: 12257
		private string _id;

		// Token: 0x04002FE2 RID: 12258
		private string _title;

		// Token: 0x04002FE3 RID: 12259
		private string _description;

		// Token: 0x04002FE4 RID: 12260
		private string _imageUrl;

		// Token: 0x04002FE5 RID: 12261
		private WebPart _part;
	}
}

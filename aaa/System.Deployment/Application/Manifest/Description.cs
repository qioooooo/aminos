using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x02000015 RID: 21
	internal class Description
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00005604 File Offset: 0x00004604
		public Description(DescriptionMetadataEntry descriptionMetadataEntry)
		{
			this._publisher = descriptionMetadataEntry.Publisher;
			this._product = descriptionMetadataEntry.Product;
			this._suiteName = descriptionMetadataEntry.SuiteName;
			if (this._suiteName == null)
			{
				this._suiteName = "";
			}
			this._supportUri = AssemblyManifest.UriFromMetadataEntry(descriptionMetadataEntry.SupportUrl, "Ex_DescriptionSupportUrlNotValid");
			this._errorReportUri = AssemblyManifest.UriFromMetadataEntry(descriptionMetadataEntry.ErrorReportUrl, "Ex_DescriptionErrorReportUrlNotValid");
			this._iconFile = descriptionMetadataEntry.IconFile;
			if (this._iconFile != null)
			{
				this._iconFileFS = UriHelper.NormalizePathDirectorySeparators(this._iconFile);
			}
			this._filteredPublisher = PathTwiddler.FilterString(this._publisher, ' ', false);
			this._filteredProduct = PathTwiddler.FilterString(this._product, ' ', false);
			this._filteredSuiteName = PathTwiddler.FilterString(this._suiteName, ' ', false);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000056DB File Offset: 0x000046DB
		public string Publisher
		{
			get
			{
				return this._publisher;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000056E3 File Offset: 0x000046E3
		public string Product
		{
			get
			{
				return this._product;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000056EB File Offset: 0x000046EB
		public Uri SupportUri
		{
			get
			{
				return this._supportUri;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000056F3 File Offset: 0x000046F3
		public string SupportUrl
		{
			get
			{
				if (!(this._supportUri != null))
				{
					return null;
				}
				return this._supportUri.AbsoluteUri;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005710 File Offset: 0x00004710
		public string IconFile
		{
			get
			{
				return this._iconFile;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00005718 File Offset: 0x00004718
		public string IconFileFS
		{
			get
			{
				return this._iconFileFS;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00005720 File Offset: 0x00004720
		public Uri ErrorReportUri
		{
			get
			{
				return this._errorReportUri;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005728 File Offset: 0x00004728
		public string ErrorReportUrl
		{
			get
			{
				if (!(this._errorReportUri != null))
				{
					return null;
				}
				return this._errorReportUri.AbsoluteUri;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00005745 File Offset: 0x00004745
		public string FilteredPublisher
		{
			get
			{
				return this._filteredPublisher;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000574D File Offset: 0x0000474D
		public string FilteredProduct
		{
			get
			{
				return this._filteredProduct;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005755 File Offset: 0x00004755
		public string FilteredSuiteName
		{
			get
			{
				return this._filteredSuiteName;
			}
		}

		// Token: 0x0400005E RID: 94
		private readonly string _publisher;

		// Token: 0x0400005F RID: 95
		private readonly string _product;

		// Token: 0x04000060 RID: 96
		private readonly string _suiteName;

		// Token: 0x04000061 RID: 97
		private readonly Uri _supportUri;

		// Token: 0x04000062 RID: 98
		private readonly Uri _errorReportUri;

		// Token: 0x04000063 RID: 99
		private readonly string _iconFile;

		// Token: 0x04000064 RID: 100
		private readonly string _iconFileFS;

		// Token: 0x04000065 RID: 101
		private readonly string _filteredPublisher;

		// Token: 0x04000066 RID: 102
		private readonly string _filteredProduct;

		// Token: 0x04000067 RID: 103
		private readonly string _filteredSuiteName;
	}
}

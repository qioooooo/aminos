using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004B7 RID: 1207
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AdCreatedEventArgs : EventArgs
	{
		// Token: 0x0600392B RID: 14635 RVA: 0x000F28DC File Offset: 0x000F18DC
		public AdCreatedEventArgs(IDictionary adProperties)
			: this(adProperties, null, null, null)
		{
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000F28E8 File Offset: 0x000F18E8
		internal AdCreatedEventArgs(IDictionary adProperties, string imageUrlField, string navigateUrlField, string alternateTextField)
		{
			if (adProperties != null)
			{
				this.adProperties = adProperties;
				this.imageUrl = this.GetAdProperty("ImageUrl", imageUrlField);
				this.navigateUrl = this.GetAdProperty("NavigateUrl", navigateUrlField);
				this.alternateText = this.GetAdProperty("AlternateText", alternateTextField);
				this.hasWidth = this.GetUnitValue(adProperties, "Width", ref this.width);
				this.hasHeight = this.GetUnitValue(adProperties, "Height", ref this.height);
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x000F298D File Offset: 0x000F198D
		public IDictionary AdProperties
		{
			get
			{
				return this.adProperties;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x000F2995 File Offset: 0x000F1995
		// (set) Token: 0x0600392F RID: 14639 RVA: 0x000F299D File Offset: 0x000F199D
		public string AlternateText
		{
			get
			{
				return this.alternateText;
			}
			set
			{
				this.alternateText = value;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x000F29A6 File Offset: 0x000F19A6
		internal bool HasHeight
		{
			get
			{
				return this.hasHeight;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x000F29AE File Offset: 0x000F19AE
		internal bool HasWidth
		{
			get
			{
				return this.hasWidth;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003932 RID: 14642 RVA: 0x000F29B6 File Offset: 0x000F19B6
		internal Unit Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x000F29BE File Offset: 0x000F19BE
		// (set) Token: 0x06003934 RID: 14644 RVA: 0x000F29C6 File Offset: 0x000F19C6
		public string ImageUrl
		{
			get
			{
				return this.imageUrl;
			}
			set
			{
				this.imageUrl = value;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x000F29CF File Offset: 0x000F19CF
		// (set) Token: 0x06003936 RID: 14646 RVA: 0x000F29D7 File Offset: 0x000F19D7
		public string NavigateUrl
		{
			get
			{
				return this.navigateUrl;
			}
			set
			{
				this.navigateUrl = value;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x000F29E0 File Offset: 0x000F19E0
		internal Unit Width
		{
			get
			{
				return this.width;
			}
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x000F29E8 File Offset: 0x000F19E8
		private string GetAdProperty(string defaultIndex, string keyIndex)
		{
			string text = (string.IsNullOrEmpty(keyIndex) ? defaultIndex : keyIndex);
			string text2 = ((this.adProperties == null) ? null : ((string)this.adProperties[text]));
			if (text2 != null)
			{
				return text2;
			}
			return string.Empty;
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000F2A2C File Offset: 0x000F1A2C
		private bool GetUnitValue(IDictionary properties, string keyIndex, ref Unit unitValue)
		{
			string text = properties[keyIndex] as string;
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					unitValue = Unit.Parse(text, CultureInfo.InvariantCulture);
				}
				catch
				{
					throw new FormatException(SR.GetString("AdRotator_invalid_integer_format", new object[]
					{
						text,
						keyIndex,
						typeof(Unit).FullName
					}));
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400260E RID: 9742
		internal const string ImageUrlElement = "ImageUrl";

		// Token: 0x0400260F RID: 9743
		internal const string NavigateUrlElement = "NavigateUrl";

		// Token: 0x04002610 RID: 9744
		internal const string AlternateTextElement = "AlternateText";

		// Token: 0x04002611 RID: 9745
		private const string WidthElement = "Width";

		// Token: 0x04002612 RID: 9746
		private const string HeightElement = "Height";

		// Token: 0x04002613 RID: 9747
		private string imageUrl = string.Empty;

		// Token: 0x04002614 RID: 9748
		private string navigateUrl = string.Empty;

		// Token: 0x04002615 RID: 9749
		private string alternateText = string.Empty;

		// Token: 0x04002616 RID: 9750
		private IDictionary adProperties;

		// Token: 0x04002617 RID: 9751
		private bool hasHeight;

		// Token: 0x04002618 RID: 9752
		private bool hasWidth;

		// Token: 0x04002619 RID: 9753
		private Unit width;

		// Token: 0x0400261A RID: 9754
		private Unit height;
	}
}

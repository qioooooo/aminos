using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200021A RID: 538
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class OutputCacheSection : ConfigurationSection
	{
		// Token: 0x06001CDF RID: 7391 RVA: 0x000838D0 File Offset: 0x000828D0
		static OutputCacheSection()
		{
			OutputCacheSection._properties.Add(OutputCacheSection._propEnableOutputCache);
			OutputCacheSection._properties.Add(OutputCacheSection._propEnableFragmentCache);
			OutputCacheSection._properties.Add(OutputCacheSection._propSendCacheControlHeader);
			OutputCacheSection._properties.Add(OutputCacheSection._propOmitVaryStar);
			OutputCacheSection._properties.Add(OutputCacheSection._propEnableKernelCacheForVaryByStar);
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000839DA File Offset: 0x000829DA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return OutputCacheSection._properties;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000839E1 File Offset: 0x000829E1
		// (set) Token: 0x06001CE3 RID: 7395 RVA: 0x00083A0E File Offset: 0x00082A0E
		[ConfigurationProperty("enableOutputCache", DefaultValue = true)]
		public bool EnableOutputCache
		{
			get
			{
				if (!this.enableOutputCacheCached)
				{
					this.enableOutputCache = (bool)base[OutputCacheSection._propEnableOutputCache];
					this.enableOutputCacheCached = true;
				}
				return this.enableOutputCache;
			}
			set
			{
				base[OutputCacheSection._propEnableOutputCache] = value;
				this.enableOutputCache = value;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x00083A28 File Offset: 0x00082A28
		// (set) Token: 0x06001CE5 RID: 7397 RVA: 0x00083A3A File Offset: 0x00082A3A
		[ConfigurationProperty("enableFragmentCache", DefaultValue = true)]
		public bool EnableFragmentCache
		{
			get
			{
				return (bool)base[OutputCacheSection._propEnableFragmentCache];
			}
			set
			{
				base[OutputCacheSection._propEnableFragmentCache] = value;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x00083A4D File Offset: 0x00082A4D
		// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x00083A7A File Offset: 0x00082A7A
		[ConfigurationProperty("sendCacheControlHeader", DefaultValue = true)]
		public bool SendCacheControlHeader
		{
			get
			{
				if (!this.sendCacheControlHeaderCached)
				{
					this.sendCacheControlHeaderCache = (bool)base[OutputCacheSection._propSendCacheControlHeader];
					this.sendCacheControlHeaderCached = true;
				}
				return this.sendCacheControlHeaderCache;
			}
			set
			{
				base[OutputCacheSection._propSendCacheControlHeader] = value;
				this.sendCacheControlHeaderCache = value;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x00083A94 File Offset: 0x00082A94
		// (set) Token: 0x06001CE9 RID: 7401 RVA: 0x00083AC1 File Offset: 0x00082AC1
		[ConfigurationProperty("omitVaryStar", DefaultValue = false)]
		public bool OmitVaryStar
		{
			get
			{
				if (!this.omitVaryStarCached)
				{
					this.omitVaryStar = (bool)base[OutputCacheSection._propOmitVaryStar];
					this.omitVaryStarCached = true;
				}
				return this.omitVaryStar;
			}
			set
			{
				base[OutputCacheSection._propOmitVaryStar] = value;
				this.omitVaryStar = value;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x00083ADB File Offset: 0x00082ADB
		// (set) Token: 0x06001CEB RID: 7403 RVA: 0x00083B08 File Offset: 0x00082B08
		[ConfigurationProperty("enableKernelCacheForVaryByStar", DefaultValue = false)]
		public bool EnableKernelCacheForVaryByStar
		{
			get
			{
				if (!this.enableKernelCacheForVaryByStarCached)
				{
					this.enableKernelCacheForVaryByStar = (bool)base[OutputCacheSection._propEnableKernelCacheForVaryByStar];
					this.enableKernelCacheForVaryByStarCached = true;
				}
				return this.enableKernelCacheForVaryByStar;
			}
			set
			{
				base[OutputCacheSection._propEnableKernelCacheForVaryByStar] = value;
				this.enableKernelCacheForVaryByStar = value;
			}
		}

		// Token: 0x0400190C RID: 6412
		internal const bool DefaultOmitVaryStar = false;

		// Token: 0x0400190D RID: 6413
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400190E RID: 6414
		private static readonly ConfigurationProperty _propEnableOutputCache = new ConfigurationProperty("enableOutputCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x0400190F RID: 6415
		private static readonly ConfigurationProperty _propEnableFragmentCache = new ConfigurationProperty("enableFragmentCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001910 RID: 6416
		private static readonly ConfigurationProperty _propSendCacheControlHeader = new ConfigurationProperty("sendCacheControlHeader", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001911 RID: 6417
		private static readonly ConfigurationProperty _propOmitVaryStar = new ConfigurationProperty("omitVaryStar", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001912 RID: 6418
		private static readonly ConfigurationProperty _propEnableKernelCacheForVaryByStar = new ConfigurationProperty("enableKernelCacheForVaryByStar", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001913 RID: 6419
		private bool sendCacheControlHeaderCached;

		// Token: 0x04001914 RID: 6420
		private bool sendCacheControlHeaderCache;

		// Token: 0x04001915 RID: 6421
		private bool omitVaryStarCached;

		// Token: 0x04001916 RID: 6422
		private bool omitVaryStar;

		// Token: 0x04001917 RID: 6423
		private bool enableKernelCacheForVaryByStarCached;

		// Token: 0x04001918 RID: 6424
		private bool enableKernelCacheForVaryByStar;

		// Token: 0x04001919 RID: 6425
		private bool enableOutputCacheCached;

		// Token: 0x0400191A RID: 6426
		private bool enableOutputCache;
	}
}

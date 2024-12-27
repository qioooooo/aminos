using System;
using System.Configuration;
using System.Net.Cache;
using System.Threading;
using Microsoft.Win32;

namespace System.Net.Configuration
{
	// Token: 0x0200065D RID: 1629
	internal sealed class RequestCachingSectionInternal
	{
		// Token: 0x0600324B RID: 12875 RVA: 0x000D6184 File Offset: 0x000D5184
		private RequestCachingSectionInternal()
		{
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000D618C File Offset: 0x000D518C
		internal RequestCachingSectionInternal(RequestCachingSection section)
		{
			if (!section.DisableAllCaching)
			{
				this.defaultCachePolicy = new RequestCachePolicy(section.DefaultPolicyLevel);
				this.isPrivateCache = section.IsPrivateCache;
				this.unspecifiedMaximumAge = section.UnspecifiedMaximumAge;
			}
			else
			{
				this.disableAllCaching = true;
			}
			this.httpRequestCacheValidator = new HttpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.ftpRequestCacheValidator = new FtpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.defaultCache = new WinInetCache(this.IsPrivateCache, true, true);
			if (section.DisableAllCaching)
			{
				return;
			}
			HttpCachePolicyElement httpCachePolicyElement = section.DefaultHttpCachePolicy;
			if (httpCachePolicyElement.WasReadFromConfig)
			{
				if (httpCachePolicyElement.PolicyLevel == HttpRequestCacheLevel.Default)
				{
					HttpCacheAgeControl httpCacheAgeControl = ((httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? HttpCacheAgeControl.MaxAgeAndMinFresh : HttpCacheAgeControl.MaxAgeAndMaxStale);
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(httpCacheAgeControl, httpCachePolicyElement.MaximumAge, (httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? httpCachePolicyElement.MinimumFresh : httpCachePolicyElement.MaximumStale);
				}
				else
				{
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(httpCachePolicyElement.PolicyLevel);
				}
			}
			FtpCachePolicyElement ftpCachePolicyElement = section.DefaultFtpCachePolicy;
			if (ftpCachePolicyElement.WasReadFromConfig)
			{
				this.defaultFtpCachePolicy = new RequestCachePolicy(ftpCachePolicyElement.PolicyLevel);
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x000D62AC File Offset: 0x000D52AC
		internal static object ClassSyncObject
		{
			get
			{
				if (RequestCachingSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref RequestCachingSectionInternal.classSyncObject, obj, null);
				}
				return RequestCachingSectionInternal.classSyncObject;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x000D62D8 File Offset: 0x000D52D8
		internal bool DisableAllCaching
		{
			get
			{
				return this.disableAllCaching;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x000D62E0 File Offset: 0x000D52E0
		internal RequestCache DefaultCache
		{
			get
			{
				return this.defaultCache;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003250 RID: 12880 RVA: 0x000D62E8 File Offset: 0x000D52E8
		internal RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return this.defaultCachePolicy;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x000D62F0 File Offset: 0x000D52F0
		internal bool IsPrivateCache
		{
			get
			{
				return this.isPrivateCache;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x000D62F8 File Offset: 0x000D52F8
		internal TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return this.unspecifiedMaximumAge;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x000D6300 File Offset: 0x000D5300
		internal HttpRequestCachePolicy DefaultHttpCachePolicy
		{
			get
			{
				return this.defaultHttpCachePolicy;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003254 RID: 12884 RVA: 0x000D6308 File Offset: 0x000D5308
		internal RequestCachePolicy DefaultFtpCachePolicy
		{
			get
			{
				return this.defaultFtpCachePolicy;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x000D6310 File Offset: 0x000D5310
		internal HttpRequestCacheValidator DefaultHttpValidator
		{
			get
			{
				return this.httpRequestCacheValidator;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x000D6318 File Offset: 0x000D5318
		internal FtpRequestCacheValidator DefaultFtpValidator
		{
			get
			{
				return this.ftpRequestCacheValidator;
			}
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000D6320 File Offset: 0x000D5320
		internal static RequestCachingSectionInternal GetSection()
		{
			RequestCachingSectionInternal requestCachingSectionInternal;
			lock (RequestCachingSectionInternal.ClassSyncObject)
			{
				RequestCachingSection requestCachingSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.RequestCachingSectionPath) as RequestCachingSection;
				if (requestCachingSection == null)
				{
					requestCachingSectionInternal = null;
				}
				else
				{
					try
					{
						requestCachingSectionInternal = new RequestCachingSectionInternal(requestCachingSection);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_requestcaching"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_requestcaching"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}
			return requestCachingSectionInternal;
		}

		// Token: 0x04002F21 RID: 12065
		private static object classSyncObject;

		// Token: 0x04002F22 RID: 12066
		private RequestCache defaultCache;

		// Token: 0x04002F23 RID: 12067
		private HttpRequestCachePolicy defaultHttpCachePolicy;

		// Token: 0x04002F24 RID: 12068
		private RequestCachePolicy defaultFtpCachePolicy;

		// Token: 0x04002F25 RID: 12069
		private RequestCachePolicy defaultCachePolicy;

		// Token: 0x04002F26 RID: 12070
		private bool disableAllCaching;

		// Token: 0x04002F27 RID: 12071
		private HttpRequestCacheValidator httpRequestCacheValidator;

		// Token: 0x04002F28 RID: 12072
		private FtpRequestCacheValidator ftpRequestCacheValidator;

		// Token: 0x04002F29 RID: 12073
		private bool isPrivateCache;

		// Token: 0x04002F2A RID: 12074
		private TimeSpan unspecifiedMaximumAge;
	}
}

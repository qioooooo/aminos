using System;
using System.Threading;

namespace System.Configuration
{
	// Token: 0x02000672 RID: 1650
	internal sealed class UriSectionInternal
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x000D7B42 File Offset: 0x000D6B42
		internal UriSectionInternal(UriSection section)
		{
			this.idn = section.Idn.Enabled;
			this.iriParsing = section.IriParsing.Enabled;
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060032F4 RID: 13044 RVA: 0x000D7B6C File Offset: 0x000D6B6C
		internal UriIdnScope Idn
		{
			get
			{
				return this.idn;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060032F5 RID: 13045 RVA: 0x000D7B74 File Offset: 0x000D6B74
		internal bool IriParsing
		{
			get
			{
				return this.iriParsing;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060032F6 RID: 13046 RVA: 0x000D7B7C File Offset: 0x000D6B7C
		internal static object ClassSyncObject
		{
			get
			{
				if (UriSectionInternal.classSyncObject == null)
				{
					Interlocked.CompareExchange(ref UriSectionInternal.classSyncObject, new object(), null);
				}
				return UriSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x000D7B9C File Offset: 0x000D6B9C
		internal static UriSectionInternal GetSection()
		{
			UriSectionInternal uriSectionInternal;
			lock (UriSectionInternal.ClassSyncObject)
			{
				UriSection uriSection = PrivilegedConfigurationManager.GetSection(CommonConfigurationStrings.UriSectionPath) as UriSection;
				if (uriSection == null)
				{
					uriSectionInternal = null;
				}
				else
				{
					uriSectionInternal = new UriSectionInternal(uriSection);
				}
			}
			return uriSectionInternal;
		}

		// Token: 0x04002F76 RID: 12150
		private bool iriParsing;

		// Token: 0x04002F77 RID: 12151
		private UriIdnScope idn;

		// Token: 0x04002F78 RID: 12152
		private static object classSyncObject;
	}
}

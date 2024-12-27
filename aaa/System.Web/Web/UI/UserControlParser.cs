using System;
using System.Collections;
using System.Web.Caching;

namespace System.Web.UI
{
	// Token: 0x0200042C RID: 1068
	internal class UserControlParser : TemplateControlParser
	{
		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x000DEC6D File Offset: 0x000DDC6D
		internal bool FSharedPartialCaching
		{
			get
			{
				return this._fSharedPartialCaching;
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000DEC75 File Offset: 0x000DDC75
		internal override void ProcessConfigSettings()
		{
			base.ProcessConfigSettings();
			this.ApplyBaseType();
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000DEC83 File Offset: 0x000DDC83
		internal virtual void ApplyBaseType()
		{
			if (base.PagesConfig != null && base.PagesConfig.UserControlBaseTypeInternal != null)
			{
				base.BaseType = base.PagesConfig.UserControlBaseTypeInternal;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x000DECAB File Offset: 0x000DDCAB
		internal override Type DefaultBaseType
		{
			get
			{
				return typeof(UserControl);
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x000DECB7 File Offset: 0x000DDCB7
		internal override string DefaultDirectiveName
		{
			get
			{
				return "control";
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x000DECBE File Offset: 0x000DDCBE
		internal override Type DefaultFileLevelBuilderType
		{
			get
			{
				return typeof(FileLevelUserControlBuilder);
			}
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000DECCA File Offset: 0x000DDCCA
		internal override RootBuilder CreateDefaultFileLevelBuilder()
		{
			return new FileLevelUserControlBuilder();
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000DECD4 File Offset: 0x000DDCD4
		internal override void ProcessOutputCacheDirective(string directiveName, IDictionary directive)
		{
			Util.GetAndRemoveBooleanAttribute(directive, "shared", ref this._fSharedPartialCaching);
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "sqldependency");
			if (andRemoveNonEmptyAttribute != null)
			{
				SqlCacheDependency.ValidateOutputCacheDependencyString(andRemoveNonEmptyAttribute, false);
				base.OutputCacheParameters.SqlDependency = andRemoveNonEmptyAttribute;
			}
			base.ProcessOutputCacheDirective(directiveName, directive);
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x000DED1D File Offset: 0x000DDD1D
		internal override bool FVaryByParamsRequiredOnOutputCache
		{
			get
			{
				return base.OutputCacheParameters.VaryByControl == null;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x000DED2D File Offset: 0x000DDD2D
		internal override string UnknownOutputCacheAttributeError
		{
			get
			{
				return "Attr_not_supported_in_ucdirective";
			}
		}

		// Token: 0x040023F3 RID: 9203
		internal const string defaultDirectiveName = "control";

		// Token: 0x040023F4 RID: 9204
		private bool _fSharedPartialCaching;
	}
}

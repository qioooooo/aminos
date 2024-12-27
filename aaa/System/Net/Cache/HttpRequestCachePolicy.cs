using System;

namespace System.Net.Cache
{
	// Token: 0x0200056F RID: 1391
	public class HttpRequestCachePolicy : RequestCachePolicy
	{
		// Token: 0x06002A95 RID: 10901 RVA: 0x000B4F20 File Offset: 0x000B3F20
		public HttpRequestCachePolicy()
			: this(HttpRequestCacheLevel.Default)
		{
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000B4F2C File Offset: 0x000B3F2C
		public HttpRequestCachePolicy(HttpRequestCacheLevel level)
			: base(HttpRequestCachePolicy.MapLevel(level))
		{
			this.m_Level = level;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000B4F78 File Offset: 0x000B3F78
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale)
			: this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = ageOrFreshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "HttpCacheAgeControl" }), "cacheAgeControl");
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000B4FE4 File Offset: 0x000B3FE4
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale)
			: this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = maxAge;
				return;
			case HttpCacheAgeControl.MaxAgeAndMinFresh:
				this.m_MaxAge = maxAge;
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAgeAndMaxStale:
				this.m_MaxAge = maxAge;
				this.m_MaxStale = freshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "HttpCacheAgeControl" }), "cacheAgeControl");
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000B5076 File Offset: 0x000B4076
		public HttpRequestCachePolicy(DateTime cacheSyncDate)
			: this(HttpRequestCacheLevel.Default)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000B508C File Offset: 0x000B408C
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate)
			: this(cacheAgeControl, maxAge, freshOrStale)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000B50A4 File Offset: 0x000B40A4
		public new HttpRequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000B50AC File Offset: 0x000B40AC
		public DateTime CacheSyncDate
		{
			get
			{
				if (this.m_LastSyncDateUtc == DateTime.MinValue || this.m_LastSyncDateUtc == DateTime.MaxValue)
				{
					return this.m_LastSyncDateUtc;
				}
				return this.m_LastSyncDateUtc.ToLocalTime();
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x000B50E4 File Offset: 0x000B40E4
		internal DateTime InternalCacheSyncDateUtc
		{
			get
			{
				return this.m_LastSyncDateUtc;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x000B50EC File Offset: 0x000B40EC
		public TimeSpan MaxAge
		{
			get
			{
				return this.m_MaxAge;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000B50F4 File Offset: 0x000B40F4
		public TimeSpan MinFresh
		{
			get
			{
				return this.m_MinFresh;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000B50FC File Offset: 0x000B40FC
		public TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000B5104 File Offset: 0x000B4104
		public override string ToString()
		{
			/*
An exception occurred when decompiling this method (06002AA1)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Net.Cache.HttpRequestCachePolicy::ToString()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 217
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.Run(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 49
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 264
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000B5217 File Offset: 0x000B4217
		private static RequestCacheLevel MapLevel(HttpRequestCacheLevel level)
		{
			if (level <= HttpRequestCacheLevel.NoCacheNoStore)
			{
				return (RequestCacheLevel)level;
			}
			if (level == HttpRequestCacheLevel.CacheOrNextCacheOnly)
			{
				return RequestCacheLevel.CacheOnly;
			}
			if (level == HttpRequestCacheLevel.Refresh)
			{
				return RequestCacheLevel.Reload;
			}
			throw new ArgumentOutOfRangeException("level");
		}

		// Token: 0x04002926 RID: 10534
		internal static readonly HttpRequestCachePolicy BypassCache = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);

		// Token: 0x04002927 RID: 10535
		private HttpRequestCacheLevel m_Level;

		// Token: 0x04002928 RID: 10536
		private DateTime m_LastSyncDateUtc = DateTime.MinValue;

		// Token: 0x04002929 RID: 10537
		private TimeSpan m_MaxAge = TimeSpan.MaxValue;

		// Token: 0x0400292A RID: 10538
		private TimeSpan m_MinFresh = TimeSpan.MinValue;

		// Token: 0x0400292B RID: 10539
		private TimeSpan m_MaxStale = TimeSpan.MinValue;
	}
}

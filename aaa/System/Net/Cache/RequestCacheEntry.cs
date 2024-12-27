using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.Win32;

namespace System.Net.Cache
{
	// Token: 0x02000568 RID: 1384
	internal class RequestCacheEntry
	{
		// Token: 0x06002A6C RID: 10860 RVA: 0x000B476C File Offset: 0x000B376C
		internal RequestCacheEntry()
		{
			this.m_ExpiresUtc = (this.m_LastAccessedUtc = (this.m_LastModifiedUtc = (this.m_LastSynchronizedUtc = DateTime.MinValue)));
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000B47A8 File Offset: 0x000B37A8
		internal RequestCacheEntry(_WinInetCache.Entry entry, bool isPrivateEntry)
		{
			this.m_IsPrivateEntry = isPrivateEntry;
			this.m_StreamSize = ((long)entry.Info.SizeHigh << 32) | (long)entry.Info.SizeLow;
			this.m_ExpiresUtc = (entry.Info.ExpireTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.ExpireTime.ToLong()));
			this.m_HitCount = entry.Info.HitRate;
			this.m_LastAccessedUtc = (entry.Info.LastAccessTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastAccessTime.ToLong()));
			this.m_LastModifiedUtc = (entry.Info.LastModifiedTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastModifiedTime.ToLong()));
			this.m_LastSynchronizedUtc = (entry.Info.LastSyncTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastSyncTime.ToLong()));
			this.m_MaxStale = TimeSpan.FromSeconds((double)entry.Info.U.ExemptDelta);
			if (this.m_MaxStale == WinInetCache.s_MaxTimeSpanForInt32)
			{
				this.m_MaxStale = TimeSpan.MaxValue;
			}
			this.m_UsageCount = entry.Info.UseCount;
			this.m_IsPartialEntry = (entry.Info.EntryType & _WinInetCache.EntryType.Sparse) != (_WinInetCache.EntryType)0;
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x000B492C File Offset: 0x000B392C
		// (set) Token: 0x06002A6F RID: 10863 RVA: 0x000B4934 File Offset: 0x000B3934
		internal bool IsPrivateEntry
		{
			get
			{
				return this.m_IsPrivateEntry;
			}
			set
			{
				this.m_IsPrivateEntry = value;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000B493D File Offset: 0x000B393D
		// (set) Token: 0x06002A71 RID: 10865 RVA: 0x000B4945 File Offset: 0x000B3945
		internal long StreamSize
		{
			get
			{
				return this.m_StreamSize;
			}
			set
			{
				this.m_StreamSize = value;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x000B494E File Offset: 0x000B394E
		// (set) Token: 0x06002A73 RID: 10867 RVA: 0x000B4956 File Offset: 0x000B3956
		internal DateTime ExpiresUtc
		{
			get
			{
				/*
An exception occurred when decompiling this method (06002A72)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.DateTime System.Net.Cache.RequestCacheEntry::get_ExpiresUtc()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.YieldReturnDecompiler.Run(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, String& compilerName, List`1 list_ILNode, Func`2 getILInlining, List`1 listExpr, List`1 listBlock, Dictionary`2 labelRefCount) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\YieldReturnDecompiler.cs:line 71
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 248
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
			set
			{
				this.m_ExpiresUtc = value;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x000B495F File Offset: 0x000B395F
		// (set) Token: 0x06002A75 RID: 10869 RVA: 0x000B4967 File Offset: 0x000B3967
		internal DateTime LastAccessedUtc
		{
			get
			{
				return this.m_LastAccessedUtc;
			}
			set
			{
				this.m_LastAccessedUtc = value;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000B4970 File Offset: 0x000B3970
		// (set) Token: 0x06002A77 RID: 10871 RVA: 0x000B4978 File Offset: 0x000B3978
		internal DateTime LastModifiedUtc
		{
			get
			{
				return this.m_LastModifiedUtc;
			}
			set
			{
				this.m_LastModifiedUtc = value;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000B4981 File Offset: 0x000B3981
		// (set) Token: 0x06002A79 RID: 10873 RVA: 0x000B4989 File Offset: 0x000B3989
		internal DateTime LastSynchronizedUtc
		{
			get
			{
				return this.m_LastSynchronizedUtc;
			}
			set
			{
				/*
An exception occurred when decompiling this method (06002A79)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Net.Cache.RequestCacheEntry::set_LastSynchronizedUtc(System.DateTime)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression..ctor(ILCode code, Object operand) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 626
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1010
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000B4992 File Offset: 0x000B3992
		// (set) Token: 0x06002A7B RID: 10875 RVA: 0x000B499A File Offset: 0x000B399A
		internal TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
			set
			{
				this.m_MaxStale = value;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000B49A3 File Offset: 0x000B39A3
		// (set) Token: 0x06002A7D RID: 10877 RVA: 0x000B49AB File Offset: 0x000B39AB
		internal int HitCount
		{
			get
			{
				return this.m_HitCount;
			}
			set
			{
				this.m_HitCount = value;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000B49B4 File Offset: 0x000B39B4
		// (set) Token: 0x06002A7F RID: 10879 RVA: 0x000B49BC File Offset: 0x000B39BC
		internal int UsageCount
		{
			get
			{
				return this.m_UsageCount;
			}
			set
			{
				this.m_UsageCount = value;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000B49C5 File Offset: 0x000B39C5
		// (set) Token: 0x06002A81 RID: 10881 RVA: 0x000B49CD File Offset: 0x000B39CD
		internal bool IsPartialEntry
		{
			get
			{
				/*
An exception occurred when decompiling this method (06002A80)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Net.Cache.RequestCacheEntry::get_IsPartialEntry()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at ICSharpCode.Decompiler.FlowAnalysis.ControlFlowGraph.ComputeDominance(CancellationToken cancellationToken)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 66
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
			set
			{
				this.m_IsPartialEntry = value;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000B49D6 File Offset: 0x000B39D6
		// (set) Token: 0x06002A83 RID: 10883 RVA: 0x000B49DE File Offset: 0x000B39DE
		internal StringCollection EntryMetadata
		{
			get
			{
				return this.m_EntryMetadata;
			}
			set
			{
				this.m_EntryMetadata = value;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000B49E7 File Offset: 0x000B39E7
		// (set) Token: 0x06002A85 RID: 10885 RVA: 0x000B49EF File Offset: 0x000B39EF
		internal StringCollection SystemMetadata
		{
			get
			{
				return this.m_SystemMetadata;
			}
			set
			{
				this.m_SystemMetadata = value;
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000B49F8 File Offset: 0x000B39F8
		internal virtual string ToString(bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("\r\nIsPrivateEntry   = ").Append(this.IsPrivateEntry);
			stringBuilder.Append("\r\nIsPartialEntry   = ").Append(this.IsPartialEntry);
			stringBuilder.Append("\r\nStreamSize       = ").Append(this.StreamSize);
			stringBuilder.Append("\r\nExpires          = ").Append((this.ExpiresUtc == DateTime.MinValue) ? "" : this.ExpiresUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastAccessed     = ").Append((this.LastAccessedUtc == DateTime.MinValue) ? "" : this.LastAccessedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastModified     = ").Append((this.LastModifiedUtc == DateTime.MinValue) ? "" : this.LastModifiedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastSynchronized = ").Append((this.LastSynchronizedUtc == DateTime.MinValue) ? "" : this.LastSynchronizedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nMaxStale(sec)    = ").Append((this.MaxStale == TimeSpan.MinValue) ? "" : ((int)this.MaxStale.TotalSeconds).ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nHitCount         = ").Append(this.HitCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nUsageCount       = ").Append(this.UsageCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\n");
			if (verbose)
			{
				stringBuilder.Append("EntryMetadata:\r\n");
				if (this.m_EntryMetadata != null)
				{
					foreach (string text in this.m_EntryMetadata)
					{
						stringBuilder.Append(text).Append("\r\n");
					}
				}
				stringBuilder.Append("---\r\nSystemMetadata:\r\n");
				if (this.m_SystemMetadata != null)
				{
					foreach (string text2 in this.m_SystemMetadata)
					{
						stringBuilder.Append(text2).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040028F8 RID: 10488
		private bool m_IsPrivateEntry;

		// Token: 0x040028F9 RID: 10489
		private long m_StreamSize;

		// Token: 0x040028FA RID: 10490
		private DateTime m_ExpiresUtc;

		// Token: 0x040028FB RID: 10491
		private int m_HitCount;

		// Token: 0x040028FC RID: 10492
		private DateTime m_LastAccessedUtc;

		// Token: 0x040028FD RID: 10493
		private DateTime m_LastModifiedUtc;

		// Token: 0x040028FE RID: 10494
		private DateTime m_LastSynchronizedUtc;

		// Token: 0x040028FF RID: 10495
		private TimeSpan m_MaxStale;

		// Token: 0x04002900 RID: 10496
		private int m_UsageCount;

		// Token: 0x04002901 RID: 10497
		private bool m_IsPartialEntry;

		// Token: 0x04002902 RID: 10498
		private StringCollection m_EntryMetadata;

		// Token: 0x04002903 RID: 10499
		private StringCollection m_SystemMetadata;
	}
}

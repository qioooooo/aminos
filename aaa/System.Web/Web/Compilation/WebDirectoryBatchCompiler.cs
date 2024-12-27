using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Web.Configuration;
using System.Web.Hosting;

namespace System.Web.Compilation
{
	// Token: 0x02000140 RID: 320
	internal class WebDirectoryBatchCompiler
	{
		// Token: 0x06000EFE RID: 3838 RVA: 0x00043B94 File Offset: 0x00042B94
		internal WebDirectoryBatchCompiler(VirtualDirectory vdir)
		{
			this._vdir = vdir;
			this._utcStart = DateTime.UtcNow;
			this._compConfig = RuntimeConfig.GetConfig(this._vdir.VirtualPath).Compilation;
			this._referencedAssemblies = BuildManager.GetReferencedAssemblies(this._compConfig);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00043BF5 File Offset: 0x00042BF5
		internal void SetIgnoreErrors()
		{
			this._ignoreProvidersWithErrors = true;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00043C00 File Offset: 0x00042C00
		internal void Process()
		{
			this.AddBuildProviders(true);
			if (this._buildProviders.Count == 0)
			{
				return;
			}
			BuildManager.ReportDirectoryCompilationProgress(this._vdir.VirtualPathObject);
			this.GetBuildResultDependencies();
			this.ProcessDependencies();
			foreach (ArrayList collection in this._nonDependentBuckets)
			{
				if (!this.CompileNonDependentBuildProviders(collection))
				{
					break;
				}
			}
			if (this._parserErrors != null && this._parserErrors.Count > 0)
			{
				HttpParseException ex = new HttpParseException(this._firstException.Message, this._firstException, this._firstException.VirtualPath, this._firstException.Source, this._firstException.Line);
				for (int j = 1; j < this._parserErrors.Count; j++)
				{
					ex.ParserErrors.Add(this._parserErrors[j]);
				}
				throw ex;
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00043CE4 File Offset: 0x00042CE4
		private void AddBuildProviders(bool retryIfDeletionHappens)
		{
			DiskBuildResultCache.ResetAssemblyDeleted();
			foreach (object obj in this._vdir.Files)
			{
				VirtualFile virtualFile = (VirtualFile)obj;
				BuildResult buildResult = null;
				try
				{
					buildResult = BuildManager.GetVPathBuildResultFromCache(virtualFile.VirtualPathObject);
				}
				catch
				{
					if (!BuildManager.PerformingPrecompilation)
					{
						continue;
					}
				}
				if (buildResult == null)
				{
					BuildProvider buildProvider = BuildManager.CreateBuildProvider(virtualFile.VirtualPathObject, this._compConfig, this._referencedAssemblies, false);
					if (buildProvider != null)
					{
						this._buildProviders[virtualFile.VirtualPath] = buildProvider;
					}
				}
			}
			if (DiskBuildResultCache.InUseAssemblyWasDeleted && retryIfDeletionHappens && BuildManager.PerformingPrecompilation)
			{
				this.AddBuildProviders(false);
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00043DB4 File Offset: 0x00042DB4
		private void CacheAssemblyResults(AssemblyBuilder assemblyBuilder, CompilerResults results)
		{
			foreach (object obj in assemblyBuilder.BuildProviders)
			{
				BuildProvider buildProvider = (BuildProvider)obj;
				BuildResult buildResult = buildProvider.GetBuildResult(results);
				if (buildResult != null && !BuildManager.CacheVPathBuildResult(buildProvider.VirtualPathObject, buildResult, this._utcStart))
				{
					break;
				}
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00043E28 File Offset: 0x00042E28
		private void CacheCompileErrors(AssemblyBuilder assemblyBuilder, CompilerResults results)
		{
			BuildProvider buildProvider = null;
			foreach (object obj in results.Errors)
			{
				CompilerError compilerError = (CompilerError)obj;
				if (!compilerError.IsWarning)
				{
					BuildProvider buildProviderFromLinePragma = assemblyBuilder.GetBuildProviderFromLinePragma(compilerError.FileName);
					if (buildProviderFromLinePragma != null && buildProviderFromLinePragma is BaseTemplateBuildProvider && buildProviderFromLinePragma != buildProvider)
					{
						buildProvider = buildProviderFromLinePragma;
						CompilerResults compilerResults = new CompilerResults(null);
						foreach (string text in results.Output)
						{
							compilerResults.Output.Add(text);
						}
						compilerResults.PathToAssembly = results.PathToAssembly;
						compilerResults.NativeCompilerReturnValue = results.NativeCompilerReturnValue;
						compilerResults.Errors.Add(compilerError);
						HttpCompileException ex = new HttpCompileException(compilerResults, assemblyBuilder.GetGeneratedSourceFromBuildProvider(buildProviderFromLinePragma));
						BuildResult buildResult = new BuildResultCompileError(buildProviderFromLinePragma.VirtualPathObject, ex);
						buildProviderFromLinePragma.SetBuildResultDependencies(buildResult);
						BuildManager.CacheVPathBuildResult(buildProviderFromLinePragma.VirtualPathObject, buildResult, this._utcStart);
					}
				}
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00043F74 File Offset: 0x00042F74
		private void GetBuildResultDependencies()
		{
			foreach (object obj in this._buildProviders.Values)
			{
				BuildProvider buildProvider = (BuildProvider)obj;
				ICollection buildResultVirtualPathDependencies = buildProvider.GetBuildResultVirtualPathDependencies();
				if (buildResultVirtualPathDependencies != null)
				{
					foreach (object obj2 in buildResultVirtualPathDependencies)
					{
						string text = (string)obj2;
						BuildProvider buildProvider2 = (BuildProvider)this._buildProviders[text];
						if (buildProvider2 != null)
						{
							buildProvider.AddBuildProviderDependency(buildProvider2);
						}
					}
				}
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0004403C File Offset: 0x0004303C
		private void ProcessDependencies()
		{
			int num = 0;
			Hashtable hashtable = new Hashtable();
			Stack stack = new Stack();
			foreach (object obj in this._buildProviders.Values)
			{
				BuildProvider buildProvider = (BuildProvider)obj;
				stack.Push(buildProvider);
				while (stack.Count > 0)
				{
					BuildProvider buildProvider2 = (BuildProvider)stack.Peek();
					bool flag = false;
					int num2 = 0;
					if (buildProvider2.BuildProviderDependencies != null)
					{
						foreach (object obj2 in ((IEnumerable)buildProvider2.BuildProviderDependencies))
						{
							BuildProvider buildProvider3 = (BuildProvider)obj2;
							if (hashtable.ContainsKey(buildProvider3))
							{
								if (num2 <= (int)hashtable[buildProvider3])
								{
									num2 = (int)hashtable[buildProvider3] + 1;
								}
								else if ((int)hashtable[buildProvider3] == -1)
								{
									throw new HttpException(SR.GetString("File_Circular_Reference", new object[] { buildProvider3.VirtualPath }));
								}
							}
							else
							{
								flag = true;
								stack.Push(buildProvider3);
							}
						}
					}
					if (flag)
					{
						hashtable[buildProvider2] = -1;
					}
					else
					{
						stack.Pop();
						hashtable[buildProvider2] = num2;
						if (num <= num2)
						{
							num = num2 + 1;
						}
					}
				}
			}
			this._nonDependentBuckets = new ArrayList[num];
			IDictionaryEnumerator enumerator3 = hashtable.GetEnumerator();
			while (enumerator3.MoveNext())
			{
				int num3 = (int)enumerator3.Value;
				if (this._nonDependentBuckets[num3] == null)
				{
					this._nonDependentBuckets[num3] = new ArrayList();
				}
				this._nonDependentBuckets[num3].Add(enumerator3.Key);
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00044248 File Offset: 0x00043248
		private bool IsBuildProviderSkipable(BuildProvider buildProvider)
		{
			return !buildProvider.IsDependedOn && (buildProvider is SourceFileBuildProvider || buildProvider is ResXBuildProvider);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004426C File Offset: 0x0004326C
		private bool CompileNonDependentBuildProviders(ICollection buildProviders)
		{
			IDictionary dictionary = new Hashtable();
			ArrayList arrayList = null;
			AssemblyBuilder assemblyBuilder = null;
			bool flag = false;
			foreach (object obj in buildProviders)
			{
				BuildProvider buildProvider = (BuildProvider)obj;
				if (!this.IsBuildProviderSkipable(buildProvider))
				{
					if (!BuildManager.ThrowOnFirstParseError)
					{
						InternalBuildProvider internalBuildProvider = buildProvider as InternalBuildProvider;
						if (internalBuildProvider != null)
						{
							internalBuildProvider.ThrowOnFirstParseError = false;
						}
					}
					CompilerType compilerType = null;
					try
					{
						compilerType = BuildProvider.GetCompilerTypeFromBuildProvider(buildProvider);
					}
					catch (HttpParseException ex)
					{
						if (this._ignoreProvidersWithErrors)
						{
							continue;
						}
						flag = true;
						if (this._firstException == null)
						{
							this._firstException = ex;
						}
						if (this._parserErrors == null)
						{
							this._parserErrors = new ParserErrorCollection();
						}
						this._parserErrors.AddRange(ex.ParserErrors);
						continue;
					}
					catch
					{
						if (this._ignoreProvidersWithErrors)
						{
							continue;
						}
						throw;
					}
					AssemblyBuilder assemblyBuilder2 = assemblyBuilder;
					if (compilerType == null)
					{
						if (assemblyBuilder == null)
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							arrayList.Add(buildProvider);
							continue;
						}
					}
					else
					{
						assemblyBuilder2 = (AssemblyBuilder)dictionary[compilerType];
					}
					ICollection generatedTypeNames = buildProvider.GetGeneratedTypeNames();
					if (assemblyBuilder2 == null || assemblyBuilder2.IsBatchFull || assemblyBuilder2.ContainsTypeNames(generatedTypeNames))
					{
						if (assemblyBuilder2 != null)
						{
							this.CompileAssemblyBuilder(assemblyBuilder2);
						}
						AssemblyBuilder assemblyBuilder3 = compilerType.CreateAssemblyBuilder(this._compConfig, this._referencedAssemblies);
						dictionary[compilerType] = assemblyBuilder3;
						if (assemblyBuilder == null || assemblyBuilder == assemblyBuilder2)
						{
							assemblyBuilder = assemblyBuilder3;
						}
						assemblyBuilder2 = assemblyBuilder3;
					}
					assemblyBuilder2.AddTypeNames(generatedTypeNames);
					assemblyBuilder2.AddBuildProvider(buildProvider);
				}
			}
			if (flag)
			{
				return false;
			}
			if (arrayList != null)
			{
				bool flag2 = assemblyBuilder == null;
				foreach (object obj2 in arrayList)
				{
					BuildProvider buildProvider2 = (BuildProvider)obj2;
					ICollection generatedTypeNames2 = buildProvider2.GetGeneratedTypeNames();
					if (assemblyBuilder == null || assemblyBuilder.IsBatchFull || assemblyBuilder.ContainsTypeNames(generatedTypeNames2))
					{
						if (assemblyBuilder != null)
						{
							this.CompileAssemblyBuilder(assemblyBuilder);
						}
						assemblyBuilder = CompilerType.GetDefaultAssemblyBuilder(this._compConfig, this._referencedAssemblies, this._vdir.VirtualPathObject, null);
						flag2 = true;
					}
					assemblyBuilder.AddTypeNames(generatedTypeNames2);
					assemblyBuilder.AddBuildProvider(buildProvider2);
				}
				if (flag2)
				{
					this.CompileAssemblyBuilder(assemblyBuilder);
				}
			}
			foreach (object obj3 in dictionary.Values)
			{
				AssemblyBuilder assemblyBuilder4 = (AssemblyBuilder)obj3;
				this.CompileAssemblyBuilder(assemblyBuilder4);
			}
			return true;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00044560 File Offset: 0x00043560
		private void CompileAssemblyBuilder(AssemblyBuilder builder)
		{
			CompilerResults compilerResults;
			try
			{
				compilerResults = builder.Compile();
			}
			catch (HttpCompileException ex)
			{
				this.CacheCompileErrors(builder, ex.Results);
				throw;
			}
			this.CacheAssemblyResults(builder, compilerResults);
		}

		// Token: 0x040015BE RID: 5566
		private DateTime _utcStart;

		// Token: 0x040015BF RID: 5567
		private ICollection _referencedAssemblies;

		// Token: 0x040015C0 RID: 5568
		private CompilationSection _compConfig;

		// Token: 0x040015C1 RID: 5569
		private IDictionary _buildProviders = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040015C2 RID: 5570
		private VirtualDirectory _vdir;

		// Token: 0x040015C3 RID: 5571
		private ArrayList[] _nonDependentBuckets;

		// Token: 0x040015C4 RID: 5572
		private bool _ignoreProvidersWithErrors;

		// Token: 0x040015C5 RID: 5573
		private ParserErrorCollection _parserErrors;

		// Token: 0x040015C6 RID: 5574
		private HttpParseException _firstException;
	}
}

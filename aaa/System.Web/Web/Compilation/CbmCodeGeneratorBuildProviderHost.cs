using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200012B RID: 299
	internal class CbmCodeGeneratorBuildProviderHost : AssemblyBuilder
	{
		// Token: 0x06000DAE RID: 3502 RVA: 0x000392F4 File Offset: 0x000382F4
		internal CbmCodeGeneratorBuildProviderHost(CompilationSection compConfig, ICollection referencedAssemblies, CompilerType compilerType, string generatedFilesDir, string outputAssemblyName)
			: base(compConfig, referencedAssemblies, compilerType, outputAssemblyName)
		{
			if (Directory.Exists(generatedFilesDir))
			{
				foreach (object obj in ((IEnumerable)FileEnumerator.Create(generatedFilesDir)))
				{
					FileData fileData = (FileData)obj;
					if (!fileData.IsDirectory)
					{
						File.Delete(fileData.FullName);
					}
				}
			}
			Directory.CreateDirectory(generatedFilesDir);
			this._generatedFilesDir = generatedFilesDir;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00039380 File Offset: 0x00038380
		internal override TextWriter CreateCodeFile(BuildProvider buildProvider, out string filename)
		{
			string text = BuildManager.GetCacheKeyFromVirtualPath(buildProvider.VirtualPathObject);
			text = Path.Combine(this._generatedFilesDir, text);
			text = FileUtil.TruncatePathIfNeeded(text, 10);
			text = text + "." + this._codeProvider.FileExtension;
			filename = text;
			BuildManager.GenerateFileTable[buildProvider.VirtualPathObject.VirtualPathStringNoTrailingSlash] = text;
			Stream stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read);
			return new StreamWriter(stream, Encoding.UTF8);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000393F4 File Offset: 0x000383F4
		internal override void AddBuildProvider(BuildProvider buildProvider)
		{
			if (buildProvider is SourceFileBuildProvider)
			{
				return;
			}
			base.AddBuildProvider(buildProvider);
		}

		// Token: 0x0400151D RID: 5405
		private string _generatedFilesDir;
	}
}

using System;
using System.CodeDom;
using System.Collections;
using System.Data.Design;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Xml;

namespace System.Web.Compilation
{
	// Token: 0x0200019A RID: 410
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Code)]
	internal class XsdBuildProvider : BuildProvider
	{
		// Token: 0x06001138 RID: 4408 RVA: 0x0004D4B4 File Offset: 0x0004C4B4
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			string namespaceFromVirtualPath = Util.GetNamespaceFromVirtualPath(base.VirtualPathObject);
			XmlDocument xmlDocument = new XmlDocument();
			using (Stream stream = base.OpenStream())
			{
				xmlDocument.Load(stream);
			}
			string outerXml = xmlDocument.OuterXml;
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(namespaceFromVirtualPath);
			codeCompileUnit.Namespaces.Add(codeNamespace);
			bool flag = CompilationUtil.IsCompilerVersion35(assemblyBuilder.CodeDomProvider.GetType());
			if (flag)
			{
				TypedDataSetGenerator.GenerateOption generateOption = TypedDataSetGenerator.GenerateOption.None;
				generateOption |= TypedDataSetGenerator.GenerateOption.HierarchicalUpdate;
				generateOption |= TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets;
				Hashtable hashtable = null;
				TypedDataSetGenerator.Generate(outerXml, codeCompileUnit, codeNamespace, assemblyBuilder.CodeDomProvider, hashtable, generateOption);
			}
			else
			{
				TypedDataSetGenerator.Generate(outerXml, codeCompileUnit, codeNamespace, assemblyBuilder.CodeDomProvider);
			}
			if (TypedDataSetGenerator.ReferencedAssemblies != null)
			{
				foreach (Assembly assembly in TypedDataSetGenerator.ReferencedAssemblies)
				{
					assemblyBuilder.AddAssemblyReference(assembly);
				}
			}
			assemblyBuilder.AddCodeCompileUnit(this, codeCompileUnit);
		}
	}
}

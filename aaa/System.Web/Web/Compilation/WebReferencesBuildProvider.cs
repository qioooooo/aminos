using System;
using System.CodeDom;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Web.UI;
using System.Web.Util;
using System.Xml.Serialization;

namespace System.Web.Compilation
{
	// Token: 0x02000198 RID: 408
	internal class WebReferencesBuildProvider : BuildProvider
	{
		// Token: 0x06001134 RID: 4404 RVA: 0x0004D172 File Offset: 0x0004C172
		internal WebReferencesBuildProvider(VirtualDirectory vdir)
		{
			this._vdir = vdir;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0004D184 File Offset: 0x0004C184
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			if (!WebReferencesBuildProvider.s_triedToGetWebRefType)
			{
				WebReferencesBuildProvider.s_indigoWebRefProviderType = BuildManager.GetType("System.Web.Compilation.WCFBuildProvider", false);
				WebReferencesBuildProvider.s_triedToGetWebRefType = true;
			}
			if (WebReferencesBuildProvider.s_indigoWebRefProviderType != null)
			{
				BuildProvider buildProvider = (BuildProvider)HttpRuntime.CreateNonPublicInstance(WebReferencesBuildProvider.s_indigoWebRefProviderType);
				buildProvider.SetVirtualPath(base.VirtualPathObject);
				buildProvider.GenerateCode(assemblyBuilder);
			}
			VirtualPath webRefDirectoryVirtualPath = HttpRuntime.WebRefDirectoryVirtualPath;
			string text = this._vdir.VirtualPath;
			string text2;
			if (webRefDirectoryVirtualPath.VirtualPathString.Length == text.Length)
			{
				text2 = string.Empty;
			}
			else
			{
				text = UrlPath.RemoveSlashFromPathIfNeeded(text);
				text = text.Substring(webRefDirectoryVirtualPath.VirtualPathString.Length);
				string[] array = text.Split(new char[] { '/' });
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Util.MakeValidTypeNameFromString(array[i]);
				}
				text2 = string.Join(".", array);
			}
			CodeNamespace codeNamespace = new CodeNamespace(text2);
			WebReferenceCollection webReferenceCollection = new WebReferenceCollection();
			bool flag = false;
			foreach (object obj in this._vdir.Files)
			{
				VirtualFile virtualFile = (VirtualFile)obj;
				string text3 = UrlPath.GetExtension(virtualFile.VirtualPath);
				text3 = text3.ToLower(CultureInfo.InvariantCulture);
				if (text3 == ".discomap")
				{
					string text4 = HostingEnvironment.MapPath(virtualFile.VirtualPath);
					DiscoveryClientProtocol discoveryClientProtocol = new DiscoveryClientProtocol();
					discoveryClientProtocol.AllowAutoRedirect = true;
					discoveryClientProtocol.Credentials = CredentialCache.DefaultCredentials;
					discoveryClientProtocol.ReadAll(text4);
					WebReference webReference = new WebReference(discoveryClientProtocol.Documents, codeNamespace);
					string text5 = Path.ChangeExtension(UrlPath.GetFileName(virtualFile.VirtualPath), null);
					string text6 = text2 + "." + text5;
					WebReference webReference2 = new WebReference(discoveryClientProtocol.Documents, codeNamespace, webReference.ProtocolName, text6, null);
					webReferenceCollection.Add(webReference2);
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			codeCompileUnit.Namespaces.Add(codeNamespace);
			WebReferenceOptions webReferenceOptions = new WebReferenceOptions();
			webReferenceOptions.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync | CodeGenerationOptions.GenerateOldAsync;
			webReferenceOptions.Style = ServiceDescriptionImportStyle.Client;
			webReferenceOptions.Verbose = true;
			ServiceDescriptionImporter.GenerateWebReferences(webReferenceCollection, assemblyBuilder.CodeDomProvider, codeCompileUnit, webReferenceOptions);
			assemblyBuilder.AddCodeCompileUnit(this, codeCompileUnit);
		}

		// Token: 0x04001695 RID: 5781
		private const string IndigoWebRefProviderTypeName = "System.Web.Compilation.WCFBuildProvider";

		// Token: 0x04001696 RID: 5782
		private VirtualDirectory _vdir;

		// Token: 0x04001697 RID: 5783
		private static Type s_indigoWebRefProviderType;

		// Token: 0x04001698 RID: 5784
		private static bool s_triedToGetWebRefType;
	}
}

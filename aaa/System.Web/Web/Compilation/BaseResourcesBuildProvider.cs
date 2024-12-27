using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Resources.Tools;
using System.Web.UI;
using System.Web.Util;
using System.Xml;
using System.Xml.Schema;

namespace System.Web.Compilation
{
	// Token: 0x0200012C RID: 300
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Resources)]
	internal abstract class BaseResourcesBuildProvider : BuildProvider
	{
		// Token: 0x06000DB1 RID: 3505 RVA: 0x00039406 File Offset: 0x00038406
		internal void DontGenerateStronglyTypedClass()
		{
			this._dontGenerateStronglyTypedClass = true;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00039410 File Offset: 0x00038410
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			this._cultureName = base.GetCultureName();
			if (!this._dontGenerateStronglyTypedClass)
			{
				this._ns = Util.GetNamespaceAndTypeNameFromVirtualPath(base.VirtualPathObject, (this._cultureName == null) ? 1 : 2, out this._typeName);
				if (this._ns.Length == 0)
				{
					this._ns = "Resources";
				}
				else
				{
					this._ns = "Resources." + this._ns;
				}
			}
			using (Stream stream = base.OpenStream())
			{
				IResourceReader resourceReader = this.GetResourceReader(stream);
				try
				{
					this.GenerateResourceFile(assemblyBuilder, resourceReader);
				}
				catch (ArgumentException ex)
				{
					if (ex.InnerException != null && (ex.InnerException is XmlException || ex.InnerException is XmlSchemaException))
					{
						throw ex.InnerException;
					}
					throw;
				}
				if (this._cultureName == null && !this._dontGenerateStronglyTypedClass)
				{
					this.GenerateStronglyTypedClass(assemblyBuilder, resourceReader);
				}
			}
		}

		// Token: 0x06000DB3 RID: 3507
		protected abstract IResourceReader GetResourceReader(Stream inputStream);

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0003950C File Offset: 0x0003850C
		private void GenerateResourceFile(AssemblyBuilder assemblyBuilder, IResourceReader reader)
		{
			string text;
			if (this._ns == null)
			{
				text = UrlPath.GetFileNameWithoutExtension(base.VirtualPath) + ".resources";
			}
			else if (this._cultureName == null)
			{
				text = this._ns + "." + this._typeName + ".resources";
			}
			else
			{
				text = string.Concat(new string[] { this._ns, ".", this._typeName, ".", this._cultureName, ".resources" });
			}
			text = text.ToLower(CultureInfo.InvariantCulture);
			Stream stream;
			try
			{
				stream = assemblyBuilder.CreateEmbeddedResource(this, text);
			}
			catch (ArgumentException)
			{
				throw new HttpException(SR.GetString("Duplicate_Resource_File", new object[] { base.VirtualPath }));
			}
			using (stream)
			{
				using (IResourceWriter resourceWriter = new ResourceWriter(stream))
				{
					foreach (object obj in reader)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						resourceWriter.AddResource((string)dictionaryEntry.Key, dictionaryEntry.Value);
					}
				}
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0003968C File Offset: 0x0003868C
		private void GenerateStronglyTypedClass(AssemblyBuilder assemblyBuilder, IResourceReader reader)
		{
			IDictionary resourceList;
			try
			{
				resourceList = this.GetResourceList(reader);
			}
			finally
			{
				if (reader != null)
				{
					reader.Dispose();
				}
			}
			CodeDomProvider codeDomProvider = assemblyBuilder.CodeDomProvider;
			string[] array;
			CodeCompileUnit codeCompileUnit = StronglyTypedResourceBuilder.Create(resourceList, this._typeName, this._ns, codeDomProvider, false, out array);
			assemblyBuilder.AddCodeCompileUnit(this, codeCompileUnit);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000396E8 File Offset: 0x000386E8
		private IDictionary GetResourceList(IResourceReader reader)
		{
			IDictionary dictionary = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in reader)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				dictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
			return dictionary;
		}

		// Token: 0x0400151E RID: 5406
		internal const string DefaultResourcesNamespace = "Resources";

		// Token: 0x0400151F RID: 5407
		private string _ns;

		// Token: 0x04001520 RID: 5408
		private string _typeName;

		// Token: 0x04001521 RID: 5409
		private string _cultureName;

		// Token: 0x04001522 RID: 5410
		private bool _dontGenerateStronglyTypedClass;
	}
}

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.UI.Design;

namespace System.Web.UI
{
	// Token: 0x0200046B RID: 1131
	internal class NamespaceTagNameToTypeMapper : ITagNameToTypeMapper
	{
		// Token: 0x06003590 RID: 13712 RVA: 0x000E73FC File Offset: 0x000E63FC
		internal NamespaceTagNameToTypeMapper(TagNamespaceRegisterEntry nsRegisterEntry, Assembly assembly, TemplateParser parser)
		{
			this._nsRegisterEntry = nsRegisterEntry;
			this._assembly = assembly;
			this._parser = parser;
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003591 RID: 13713 RVA: 0x000E7419 File Offset: 0x000E6419
		public TagNamespaceRegisterEntry RegisterEntry
		{
			get
			{
				return this._nsRegisterEntry;
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000E7421 File Offset: 0x000E6421
		Type ITagNameToTypeMapper.GetControlType(string tagName, IDictionary attribs)
		{
			return this.GetControlType(tagName, attribs, false);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000E742C File Offset: 0x000E642C
		internal Type GetControlType(string tagName, IDictionary attribs, bool throwOnError)
		{
			string @namespace = this._nsRegisterEntry.Namespace;
			string text;
			if (string.IsNullOrEmpty(@namespace))
			{
				text = tagName;
			}
			else
			{
				text = @namespace + "." + tagName;
			}
			if (this._assembly != null)
			{
				Type type = null;
				if (throwOnError)
				{
					try
					{
						return this._assembly.GetType(text, true, true);
					}
					catch (FileNotFoundException)
					{
						throw;
					}
					catch (FileLoadException)
					{
						throw;
					}
					catch (BadImageFormatException)
					{
						throw;
					}
					catch
					{
						return type;
					}
				}
				type = this._assembly.GetType(text, false, true);
				return type;
			}
			if (this._parser.FInDesigner && this._parser.DesignerHost != null)
			{
				if (this._parser.DesignerHost.RootComponent != null)
				{
					WebFormsRootDesigner webFormsRootDesigner = this._parser.DesignerHost.GetDesigner(this._parser.DesignerHost.RootComponent) as WebFormsRootDesigner;
					if (webFormsRootDesigner != null)
					{
						WebFormsReferenceManager referenceManager = webFormsRootDesigner.ReferenceManager;
						if (referenceManager != null)
						{
							Type type2 = referenceManager.GetType(this._nsRegisterEntry.TagPrefix, tagName);
							if (type2 != null)
							{
								return type2;
							}
						}
					}
				}
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)this._parser.DesignerHost.GetService(typeof(ITypeResolutionService));
				if (typeResolutionService != null)
				{
					Type type3 = typeResolutionService.GetType(text, false, true);
					if (type3 != null)
					{
						return type3;
					}
				}
			}
			if (!HostingEnvironment.IsHosted)
			{
				return null;
			}
			return BuildManager.GetTypeFromCodeAssembly(text, true);
		}

		// Token: 0x04002534 RID: 9524
		private TagNamespaceRegisterEntry _nsRegisterEntry;

		// Token: 0x04002535 RID: 9525
		private Assembly _assembly;

		// Token: 0x04002536 RID: 9526
		private TemplateParser _parser;
	}
}

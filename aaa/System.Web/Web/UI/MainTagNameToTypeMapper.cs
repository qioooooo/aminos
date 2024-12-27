using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Configuration;

namespace System.Web.UI
{
	// Token: 0x0200046D RID: 1133
	internal class MainTagNameToTypeMapper
	{
		// Token: 0x06003597 RID: 13719 RVA: 0x000E7788 File Offset: 0x000E6788
		internal MainTagNameToTypeMapper(BaseTemplateParser parser)
		{
			this._parser = parser;
			if (parser != null)
			{
				PagesSection pagesConfig = parser.PagesConfig;
				if (pagesConfig != null)
				{
					this._tagNamespaceRegisterEntries = pagesConfig.TagNamespaceRegisterEntriesInternal;
					if (this._tagNamespaceRegisterEntries != null)
					{
						this._tagNamespaceRegisterEntries = (TagNamespaceRegisterEntryTable)this._tagNamespaceRegisterEntries.Clone();
					}
					this._userControlRegisterEntries = pagesConfig.UserControlRegisterEntriesInternal;
					if (this._userControlRegisterEntries != null)
					{
						this._userControlRegisterEntries = (Hashtable)this._userControlRegisterEntries.Clone();
					}
				}
				if (parser.FInDesigner && this._tagNamespaceRegisterEntries == null)
				{
					this._tagNamespaceRegisterEntries = new TagNamespaceRegisterEntryTable();
					foreach (object obj in PagesSection.DefaultTagNamespaceRegisterEntries)
					{
						TagNamespaceRegisterEntry tagNamespaceRegisterEntry = (TagNamespaceRegisterEntry)obj;
						this._tagNamespaceRegisterEntries[tagNamespaceRegisterEntry.TagPrefix] = new ArrayList(new object[] { tagNamespaceRegisterEntry });
					}
				}
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x000E7898 File Offset: 0x000E6898
		internal ICollection UserControlRegisterEntries
		{
			get
			{
				if (this._userControlRegisterEntries != null)
				{
					return this._userControlRegisterEntries.Values;
				}
				return null;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003599 RID: 13721 RVA: 0x000E78AF File Offset: 0x000E68AF
		internal List<TagNamespaceRegisterEntry> TagRegisterEntries
		{
			get
			{
				if (this._tagRegisterEntries == null)
				{
					this._tagRegisterEntries = new List<TagNamespaceRegisterEntry>();
				}
				return this._tagRegisterEntries;
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000E78CC File Offset: 0x000E68CC
		internal void ProcessTagNamespaceRegistration(TagNamespaceRegisterEntry nsRegisterEntry)
		{
			string tagPrefix = nsRegisterEntry.TagPrefix;
			ArrayList arrayList = null;
			if (this._tagNamespaceRegisterEntries != null)
			{
				arrayList = (ArrayList)this._tagNamespaceRegisterEntries[tagPrefix];
			}
			if (arrayList != null && (this._prefixedMappers == null || this._prefixedMappers[tagPrefix] == null))
			{
				this.ProcessTagNamespaceRegistration(arrayList);
			}
			this.ProcessTagNamespaceRegistrationCore(nsRegisterEntry);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000E7924 File Offset: 0x000E6924
		private void ProcessTagNamespaceRegistration(ArrayList nsRegisterEntries)
		{
			foreach (object obj in nsRegisterEntries)
			{
				TagNamespaceRegisterEntry tagNamespaceRegisterEntry = (TagNamespaceRegisterEntry)obj;
				try
				{
					this.ProcessTagNamespaceRegistrationCore(tagNamespaceRegisterEntry);
				}
				catch (Exception ex)
				{
					throw new HttpParseException(ex.Message, ex, tagNamespaceRegisterEntry.VirtualPath, null, tagNamespaceRegisterEntry.Line);
				}
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000E79A4 File Offset: 0x000E69A4
		private void ProcessTagNamespaceRegistrationCore(TagNamespaceRegisterEntry nsRegisterEntry)
		{
			Assembly assembly = null;
			if (!string.IsNullOrEmpty(nsRegisterEntry.AssemblyName))
			{
				assembly = this._parser.AddAssemblyDependency(nsRegisterEntry.AssemblyName);
			}
			if (!string.IsNullOrEmpty(nsRegisterEntry.Namespace))
			{
				this._parser.AddImportEntry(nsRegisterEntry.Namespace);
			}
			NamespaceTagNameToTypeMapper namespaceTagNameToTypeMapper = new NamespaceTagNameToTypeMapper(nsRegisterEntry, assembly, this._parser);
			if (this._prefixedMappers == null)
			{
				this._prefixedMappers = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			TagPrefixTagNameToTypeMapper tagPrefixTagNameToTypeMapper = (TagPrefixTagNameToTypeMapper)this._prefixedMappers[nsRegisterEntry.TagPrefix];
			if (tagPrefixTagNameToTypeMapper == null)
			{
				tagPrefixTagNameToTypeMapper = new TagPrefixTagNameToTypeMapper(nsRegisterEntry.TagPrefix);
				this._prefixedMappers[nsRegisterEntry.TagPrefix] = tagPrefixTagNameToTypeMapper;
			}
			tagPrefixTagNameToTypeMapper.AddNamespaceMapper(namespaceTagNameToTypeMapper);
			this.TagRegisterEntries.Add(nsRegisterEntry);
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000E7A64 File Offset: 0x000E6A64
		internal void ProcessUserControlRegistration(UserControlRegisterEntry ucRegisterEntry)
		{
			Type type;
			if (this._parser.FInDesigner)
			{
				type = this._parser.GetDesignTimeUserControlType(ucRegisterEntry.TagPrefix, ucRegisterEntry.TagName);
			}
			else
			{
				type = this._parser.GetUserControlType(ucRegisterEntry.UserControlSource.VirtualPathString);
			}
			if (type == null)
			{
				return;
			}
			if (this._userControlRegisterEntries == null)
			{
				this._userControlRegisterEntries = new Hashtable();
			}
			this._userControlRegisterEntries[ucRegisterEntry.TagPrefix + ":" + ucRegisterEntry.TagName] = ucRegisterEntry;
			this.RegisterTag(ucRegisterEntry.TagPrefix + ":" + ucRegisterEntry.TagName, type);
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000E7B08 File Offset: 0x000E6B08
		private bool TryUserControlRegisterDirectives(string tagName)
		{
			if (this._userControlRegisterEntries == null)
			{
				return false;
			}
			UserControlRegisterEntry userControlRegisterEntry = (UserControlRegisterEntry)this._userControlRegisterEntries[tagName];
			if (userControlRegisterEntry == null)
			{
				return false;
			}
			if (userControlRegisterEntry.ComesFromConfig)
			{
				VirtualPath parent = userControlRegisterEntry.UserControlSource.Parent;
				if (parent == this._parser.BaseVirtualDir)
				{
					throw new HttpException(SR.GetString("Invalid_use_of_config_uc", new object[]
					{
						this._parser.CurrentVirtualPath,
						userControlRegisterEntry.UserControlSource
					}));
				}
			}
			try
			{
				this.ProcessUserControlRegistration(userControlRegisterEntry);
			}
			catch (Exception ex)
			{
				throw new HttpParseException(ex.Message, ex, userControlRegisterEntry.VirtualPath, null, userControlRegisterEntry.Line);
			}
			return true;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000E7BC4 File Offset: 0x000E6BC4
		private bool TryNamespaceRegisterDirectives(string prefix)
		{
			if (this._tagNamespaceRegisterEntries == null)
			{
				return false;
			}
			ArrayList arrayList = (ArrayList)this._tagNamespaceRegisterEntries[prefix];
			if (arrayList == null)
			{
				return false;
			}
			this.ProcessTagNamespaceRegistration(arrayList);
			return true;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000E7BFC File Offset: 0x000E6BFC
		internal void RegisterTag(string tagName, Type type)
		{
			if (this._mappedTags == null)
			{
				this._mappedTags = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			try
			{
				this._mappedTags.Add(tagName, type);
			}
			catch (ArgumentException)
			{
				throw new HttpException(SR.GetString("Duplicate_registered_tag", new object[] { tagName }));
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000E7C60 File Offset: 0x000E6C60
		internal Type GetControlType(string tagName, IDictionary attribs, bool fAllowHtmlTags)
		{
			Type type = this.GetControlType2(tagName, attribs, fAllowHtmlTags);
			if (type != null && this._parser != null && !this._parser.FInDesigner)
			{
				Hashtable tagTypeMappingInternal = this._parser.PagesConfig.TagMapping.TagTypeMappingInternal;
				if (tagTypeMappingInternal != null)
				{
					Type type2 = (Type)tagTypeMappingInternal[type];
					if (type2 != null)
					{
						type = type2;
					}
				}
			}
			return type;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000E7CBC File Offset: 0x000E6CBC
		private Type GetControlType2(string tagName, IDictionary attribs, bool fAllowHtmlTags)
		{
			if (this._mappedTags != null)
			{
				Type type = (Type)this._mappedTags[tagName];
				if (type == null && this.TryUserControlRegisterDirectives(tagName))
				{
					type = (Type)this._mappedTags[tagName];
				}
				if (type != null)
				{
					if (this._parser != null && this._parser._pageParserFilter != null && this._parser._pageParserFilter.GetNoCompileUserControlType() == type)
					{
						UserControlRegisterEntry userControlRegisterEntry = (UserControlRegisterEntry)this._userControlRegisterEntries[tagName];
						attribs["virtualpath"] = userControlRegisterEntry.UserControlSource;
					}
					return type;
				}
			}
			int num = tagName.IndexOf(':');
			if (num >= 0)
			{
				if (num == tagName.Length - 1)
				{
					return null;
				}
				string text = tagName.Substring(0, num);
				tagName = tagName.Substring(num + 1);
				ITagNameToTypeMapper tagNameToTypeMapper = null;
				if (this._prefixedMappers != null)
				{
					tagNameToTypeMapper = (ITagNameToTypeMapper)this._prefixedMappers[text];
				}
				if (tagNameToTypeMapper == null && this.TryNamespaceRegisterDirectives(text) && this._prefixedMappers != null)
				{
					tagNameToTypeMapper = (ITagNameToTypeMapper)this._prefixedMappers[text];
				}
				if (tagNameToTypeMapper == null)
				{
					return null;
				}
				return tagNameToTypeMapper.GetControlType(tagName, attribs);
			}
			else
			{
				if (fAllowHtmlTags)
				{
					return this._htmlMapper.GetControlType(tagName, attribs);
				}
				return null;
			}
		}

		// Token: 0x04002539 RID: 9529
		private BaseTemplateParser _parser;

		// Token: 0x0400253A RID: 9530
		private IDictionary _prefixedMappers;

		// Token: 0x0400253B RID: 9531
		private IDictionary _mappedTags;

		// Token: 0x0400253C RID: 9532
		private ITagNameToTypeMapper _htmlMapper = new HtmlTagNameToTypeMapper();

		// Token: 0x0400253D RID: 9533
		private Hashtable _userControlRegisterEntries;

		// Token: 0x0400253E RID: 9534
		private List<TagNamespaceRegisterEntry> _tagRegisterEntries;

		// Token: 0x0400253F RID: 9535
		private TagNamespaceRegisterEntryTable _tagNamespaceRegisterEntries;
	}
}

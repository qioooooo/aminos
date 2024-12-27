using System;
using System.Collections;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000252 RID: 594
	[ConfigurationCollection(typeof(TagMapInfo))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TagMapCollection : ConfigurationElementCollection
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0008B168 File Offset: 0x0008A168
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TagMapCollection._properties;
			}
		}

		// Token: 0x170006AF RID: 1711
		public TagMapInfo this[int index]
		{
			get
			{
				/*
An exception occurred when decompiling this method (06001F72)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Web.Configuration.TagMapInfo System.Web.Configuration.TagMapCollection::get_Item(System.Int32)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertParameters(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 838
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 630
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0008B197 File Offset: 0x0008A197
		public void Add(TagMapInfo tagMapInformation)
		{
			this.BaseAdd(tagMapInformation);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0008B1A0 File Offset: 0x0008A1A0
		public void Remove(TagMapInfo tagMapInformation)
		{
			base.BaseRemove(this.GetElementKey(tagMapInformation));
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0008B1AF File Offset: 0x0008A1AF
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0008B1B7 File Offset: 0x0008A1B7
		protected override ConfigurationElement CreateNewElement()
		{
			return new TagMapInfo();
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0008B1BE File Offset: 0x0008A1BE
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TagMapInfo)element).TagType;
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x0008B1CC File Offset: 0x0008A1CC
		internal Hashtable TagTypeMappingInternal
		{
			get
			{
				if (this._tagMappings == null)
				{
					lock (this)
					{
						if (this._tagMappings == null)
						{
							Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
							foreach (object obj in this)
							{
								TagMapInfo tagMapInfo = (TagMapInfo)obj;
								Type type = ConfigUtil.GetType(tagMapInfo.TagType, "tagType", tagMapInfo);
								Type type2 = ConfigUtil.GetType(tagMapInfo.MappedTagType, "mappedTagType", tagMapInfo);
								if (!type.IsAssignableFrom(type2))
								{
									throw new ConfigurationErrorsException(SR.GetString("Mapped_type_must_inherit", new object[] { tagMapInfo.MappedTagType, tagMapInfo.TagType }), tagMapInfo.ElementInformation.Properties["mappedTagType"].Source, tagMapInfo.ElementInformation.Properties["mappedTagType"].LineNumber);
								}
								hashtable[type] = type2;
							}
							this._tagMappings = hashtable;
						}
					}
				}
				return this._tagMappings;
			}
		}

		// Token: 0x04001A52 RID: 6738
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A53 RID: 6739
		private Hashtable _tagMappings;
	}
}

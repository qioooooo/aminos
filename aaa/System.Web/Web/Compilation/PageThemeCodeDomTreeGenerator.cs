using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000186 RID: 390
	internal class PageThemeCodeDomTreeGenerator : BaseTemplateCodeDomTreeGenerator
	{
		// Token: 0x060010C8 RID: 4296 RVA: 0x0004AFF0 File Offset: 0x00049FF0
		internal PageThemeCodeDomTreeGenerator(PageThemeParser parser)
			: base(parser)
		{
			this._themeParser = parser;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0004B04C File Offset: 0x0004A04C
		private void AddMemberOverride(string name, Type type, CodeExpression expr)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Name = name;
			codeMemberProperty.Attributes = (MemberAttributes)12292;
			codeMemberProperty.Type = new CodeTypeReference(type.FullName);
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(expr);
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			this._sourceDataClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0004B0A8 File Offset: 0x0004A0A8
		private void BuildControlSkins(CodeStatementCollection statements)
		{
			foreach (object obj in this._controlSkinBuilderEntryList)
			{
				PageThemeCodeDomTreeGenerator.ControlSkinBuilderEntry controlSkinBuilderEntry = (PageThemeCodeDomTreeGenerator.ControlSkinBuilderEntry)obj;
				string skinID = controlSkinBuilderEntry.SkinID;
				ControlBuilder builder = controlSkinBuilderEntry.Builder;
				statements.Add(this.BuildControlSkinAssignmentStatement(builder, skinID));
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0004B11C File Offset: 0x0004A11C
		private CodeStatement BuildControlSkinAssignmentStatement(ControlBuilder builder, string skinID)
		{
			Type controlType = builder.ControlType;
			string text = base.GetMethodNameForBuilder(BaseTemplateCodeDomTreeGenerator.buildMethodPrefix, builder) + "_skinKey";
			CodeMemberField codeMemberField = new CodeMemberField(typeof(object), text);
			codeMemberField.Attributes = (MemberAttributes)20483;
			codeMemberField.InitExpression = new CodeMethodInvokeExpression
			{
				Method = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(PageTheme)), "CreateSkinKey"),
				Parameters = 
				{
					new CodeTypeOfExpression(controlType),
					new CodePrimitiveExpression(skinID)
				}
			};
			this._sourceDataClass.Members.Add(codeMemberField);
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "__controlSkins");
			CodeIndexerExpression codeIndexerExpression = new CodeIndexerExpression(codeFieldReferenceExpression, new CodeExpression[]
			{
				new CodeVariableReferenceExpression(text)
			});
			CodeDelegateCreateExpression codeDelegateCreateExpression = new CodeDelegateCreateExpression(this._controlSkinDelegateType, new CodeThisReferenceExpression(), base.GetMethodNameForBuilder(BaseTemplateCodeDomTreeGenerator.buildMethodPrefix, builder));
			return new CodeAssignStatement(codeIndexerExpression, new CodeObjectCreateExpression(this._controlSkinType, new CodeExpression[0])
			{
				Parameters = 
				{
					new CodeTypeOfExpression(controlType),
					codeDelegateCreateExpression
				}
			});
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0004B250 File Offset: 0x0004A250
		private void BuildControlSkinMember()
		{
			int count = this._controlSkinBuilderEntryList.Count;
			CodeMemberField codeMemberField = new CodeMemberField(typeof(HybridDictionary).FullName, "__controlSkins");
			codeMemberField.InitExpression = new CodeObjectCreateExpression(typeof(HybridDictionary), new CodeExpression[0])
			{
				Parameters = 
				{
					new CodePrimitiveExpression(count)
				}
			};
			this._sourceDataClass.Members.Add(codeMemberField);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0004B2CC File Offset: 0x0004A2CC
		private void BuildControlSkinProperty()
		{
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "__controlSkins");
			this.AddMemberOverride("ControlSkins", typeof(IDictionary), codeFieldReferenceExpression);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0004B300 File Offset: 0x0004A300
		private void BuildLinkedStyleSheetMember()
		{
			CodeMemberField codeMemberField = new CodeMemberField(typeof(string[]), "__linkedStyleSheets");
			if (this._themeParser.CssFileList != null && this._themeParser.CssFileList.Count > 0)
			{
				CodeExpression[] array = new CodeExpression[this._themeParser.CssFileList.Count];
				int num = 0;
				foreach (object obj in this._themeParser.CssFileList)
				{
					string text = (string)obj;
					array[num++] = new CodePrimitiveExpression(text);
				}
				CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression(typeof(string), array);
				codeMemberField.InitExpression = codeArrayCreateExpression;
			}
			else
			{
				codeMemberField.InitExpression = new CodePrimitiveExpression(null);
			}
			this._sourceDataClass.Members.Add(codeMemberField);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0004B3F8 File Offset: 0x0004A3F8
		private void BuildLinkedStyleSheetProperty()
		{
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "__linkedStyleSheets");
			this.AddMemberOverride("LinkedStyleSheets", typeof(string[]), codeFieldReferenceExpression);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0004B42B File Offset: 0x0004A42B
		protected override void BuildInitStatements(CodeStatementCollection trueStatements, CodeStatementCollection topLevelStatements)
		{
			base.BuildInitStatements(trueStatements, topLevelStatements);
			this.BuildControlSkins(topLevelStatements);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0004B43C File Offset: 0x0004A43C
		protected override void BuildMiscClassMembers()
		{
			base.BuildMiscClassMembers();
			this.AddMemberOverride(BaseTemplateCodeDomTreeGenerator.templateSourceDirectoryName, typeof(string), new CodePrimitiveExpression(this._themeParser.VirtualDirPath.VirtualPathString));
			this.BuildSourceDataTreeFromBuilder(this._themeParser.RootBuilder, false, false, null);
			this.BuildControlSkinMember();
			this.BuildControlSkinProperty();
			this.BuildLinkedStyleSheetMember();
			this.BuildLinkedStyleSheetProperty();
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0004B4A8 File Offset: 0x0004A4A8
		protected override void BuildSourceDataTreeFromBuilder(ControlBuilder builder, bool fInTemplate, bool topLevelControlInTemplate, PropertyEntry pse)
		{
			if (builder is CodeBlockBuilder)
			{
				return;
			}
			bool flag = builder is TemplateBuilder;
			bool flag2 = builder == this._themeParser.RootBuilder;
			bool flag3 = !fInTemplate && !flag && topLevelControlInTemplate;
			this._controlCount++;
			builder.ID = "__control" + this._controlCount.ToString(NumberFormatInfo.InvariantInfo);
			builder.IsGeneratedID = true;
			if (flag3 && !(builder is DataBoundLiteralControlBuilder))
			{
				Type controlType = builder.ControlType;
				string skinID = builder.SkinID;
				object obj = PageTheme.CreateSkinKey(builder.ControlType, skinID);
				if (this._controlSkinTypeNameCollection.Contains(obj))
				{
					if (string.IsNullOrEmpty(skinID))
					{
						throw new HttpParseException(SR.GetString("Page_theme_default_theme_already_defined", new object[] { builder.ControlType.FullName }), null, builder.VirtualPath, null, builder.Line);
					}
					throw new HttpParseException(SR.GetString("Page_theme_skinID_already_defined", new object[] { skinID }), null, builder.VirtualPath, null, builder.Line);
				}
				else
				{
					this._controlSkinTypeNameCollection.Add(obj, true);
					this._controlSkinBuilderEntryList.Add(new PageThemeCodeDomTreeGenerator.ControlSkinBuilderEntry(builder, skinID));
				}
			}
			if (builder.SubBuilders != null)
			{
				foreach (object obj2 in builder.SubBuilders)
				{
					if (obj2 is ControlBuilder)
					{
						bool flag4 = flag && typeof(Control).IsAssignableFrom(((ControlBuilder)obj2).ControlType);
						this.BuildSourceDataTreeFromBuilder((ControlBuilder)obj2, fInTemplate, flag4, null);
					}
				}
			}
			foreach (object obj3 in builder.TemplatePropertyEntries)
			{
				TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj3;
				this.BuildSourceDataTreeFromBuilder(templatePropertyEntry.Builder, true, false, templatePropertyEntry);
			}
			foreach (object obj4 in builder.ComplexPropertyEntries)
			{
				ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj4;
				if (!(complexPropertyEntry.Builder is StringPropertyBuilder))
				{
					this.BuildSourceDataTreeFromBuilder(complexPropertyEntry.Builder, fInTemplate, false, complexPropertyEntry);
				}
			}
			if (!flag2)
			{
				base.BuildBuildMethod(builder, flag, fInTemplate, topLevelControlInTemplate, pse, flag3);
			}
			if (!flag3 && builder.HasAspCode)
			{
				base.BuildRenderMethod(builder, flag);
			}
			base.BuildExtractMethod(builder);
			base.BuildPropertyBindingMethod(builder, flag3);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0004B768 File Offset: 0x0004A768
		internal override CodeExpression BuildStringPropertyExpression(PropertyEntry pse)
		{
			if (pse.PropertyInfo != null)
			{
				UrlPropertyAttribute urlPropertyAttribute = Attribute.GetCustomAttribute(pse.PropertyInfo, typeof(UrlPropertyAttribute)) as UrlPropertyAttribute;
				if (urlPropertyAttribute != null)
				{
					if (pse is SimplePropertyEntry)
					{
						SimplePropertyEntry simplePropertyEntry = (SimplePropertyEntry)pse;
						string text = (string)simplePropertyEntry.Value;
						if (UrlPath.IsRelativeUrl(text) && !UrlPath.IsAppRelativePath(text))
						{
							simplePropertyEntry.Value = UrlPath.MakeVirtualPathAppRelative(UrlPath.Combine(this._themeParser.VirtualDirPath.VirtualPathString, text));
						}
					}
					else
					{
						ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)pse;
						StringPropertyBuilder stringPropertyBuilder = (StringPropertyBuilder)complexPropertyEntry.Builder;
						string text2 = (string)stringPropertyBuilder.BuildObject();
						if (UrlPath.IsRelativeUrl(text2) && !UrlPath.IsAppRelativePath(text2))
						{
							complexPropertyEntry.Builder = new StringPropertyBuilder(UrlPath.MakeVirtualPathAppRelative(UrlPath.Combine(this._themeParser.VirtualDirPath.VirtualPathString, text2)));
						}
					}
				}
			}
			return base.BuildStringPropertyExpression(pse);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0004B854 File Offset: 0x0004A854
		protected override CodeAssignStatement BuildTemplatePropertyStatement(CodeExpression ctrlRefExpr)
		{
			return new CodeAssignStatement
			{
				Left = new CodePropertyReferenceExpression(ctrlRefExpr, BaseTemplateCodeDomTreeGenerator.templateSourceDirectoryName),
				Right = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.templateSourceDirectoryName)
			};
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0004B890 File Offset: 0x0004A890
		protected override string GetGeneratedClassName()
		{
			string fileName = this._themeParser.VirtualDirPath.FileName;
			return Util.MakeValidTypeNameFromString(fileName);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0004B8B6 File Offset: 0x0004A8B6
		protected override bool UseResourceLiteralString(string s)
		{
			return false;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0004B8B9 File Offset: 0x0004A8B9
		protected override bool NeedProfileProperty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001674 RID: 5748
		private const string _controlSkinsVarName = "__controlSkins";

		// Token: 0x04001675 RID: 5749
		private const string _controlSkinsPropertyName = "ControlSkins";

		// Token: 0x04001676 RID: 5750
		private const string _linkedStyleSheetsVarName = "__linkedStyleSheets";

		// Token: 0x04001677 RID: 5751
		private const string _linkedStyleSheetsPropertyName = "LinkedStyleSheets";

		// Token: 0x04001678 RID: 5752
		private Hashtable _controlSkinTypeNameCollection = new Hashtable();

		// Token: 0x04001679 RID: 5753
		private ArrayList _controlSkinBuilderEntryList = new ArrayList();

		// Token: 0x0400167A RID: 5754
		private int _controlCount;

		// Token: 0x0400167B RID: 5755
		private CodeTypeReference _controlSkinDelegateType = new CodeTypeReference(typeof(ControlSkinDelegate));

		// Token: 0x0400167C RID: 5756
		private CodeTypeReference _controlSkinType = new CodeTypeReference(typeof(ControlSkin));

		// Token: 0x0400167D RID: 5757
		private PageThemeParser _themeParser;

		// Token: 0x02000187 RID: 391
		private class ControlSkinBuilderEntry
		{
			// Token: 0x060010D8 RID: 4312 RVA: 0x0004B8BC File Offset: 0x0004A8BC
			public ControlSkinBuilderEntry(ControlBuilder builder, string skinID)
			{
				this._builder = builder;
				this._id = skinID;
			}

			// Token: 0x17000421 RID: 1057
			// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0004B8D2 File Offset: 0x0004A8D2
			public ControlBuilder Builder
			{
				get
				{
					return this._builder;
				}
			}

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x060010DA RID: 4314 RVA: 0x0004B8DA File Offset: 0x0004A8DA
			public string SkinID
			{
				get
				{
					if (this._id != null)
					{
						return this._id;
					}
					return string.Empty;
				}
			}

			// Token: 0x0400167E RID: 5758
			private ControlBuilder _builder;

			// Token: 0x0400167F RID: 5759
			private string _id;
		}
	}
}

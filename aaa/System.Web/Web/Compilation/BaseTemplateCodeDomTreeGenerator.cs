using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200012D RID: 301
	internal abstract class BaseTemplateCodeDomTreeGenerator : BaseCodeDomTreeGenerator
	{
		// Token: 0x06000DB8 RID: 3512 RVA: 0x00039760 File Offset: 0x00038760
		internal BaseTemplateCodeDomTreeGenerator(TemplateParser parser)
			: base(parser)
		{
			this._parser = parser;
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00039770 File Offset: 0x00038770
		private TemplateParser Parser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00039778 File Offset: 0x00038778
		private CodeStatement GetOutputWriteStatement(CodeExpression expr)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
			codeMethodInvokeExpression.Method.TargetObject = new CodeArgumentReferenceExpression("__w");
			codeMethodInvokeExpression.Method.MethodName = "Write";
			codeMethodInvokeExpression.Parameters.Add(expr);
			return codeExpressionStatement;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000397C8 File Offset: 0x000387C8
		private void AddOutputWriteStatement(CodeStatementCollection methodStatements, CodeExpression expr, CodeLinePragma linePragma)
		{
			CodeStatement outputWriteStatement = this.GetOutputWriteStatement(expr);
			if (linePragma != null)
			{
				outputWriteStatement.LinePragma = linePragma;
			}
			methodStatements.Add(outputWriteStatement);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000397F0 File Offset: 0x000387F0
		private void AddOutputWriteStringStatement(CodeStatementCollection methodStatements, string s)
		{
			if (!this.UseResourceLiteralString(s))
			{
				this.AddOutputWriteStatement(methodStatements, new CodePrimitiveExpression(s), null);
				return;
			}
			int num;
			int num2;
			bool flag;
			this._stringResourceBuilder.AddString(s, out num, out num2, out flag);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "WriteUTF8ResourceString";
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("__w"));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(num));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(num2));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(flag));
			methodStatements.Add(codeExpressionStatement);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000398BC File Offset: 0x000388BC
		private static void BuildAddParsedSubObjectStatement(CodeStatementCollection statements, CodeExpression ctrlToAdd, CodeLinePragma linePragma, CodeExpression ctrlRefExpr, ref bool gotParserVariable)
		{
			if (!gotParserVariable)
			{
				statements.Add(new CodeVariableDeclarationStatement
				{
					Name = "__parser",
					Type = new CodeTypeReference(typeof(IParserAccessor)),
					InitExpression = new CodeCastExpression(typeof(IParserAccessor), ctrlRefExpr)
				});
				gotParserVariable = true;
			}
			statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("__parser"), "AddParsedSubObject", new CodeExpression[0])
			{
				Parameters = { ctrlToAdd }
			})
			{
				LinePragma = linePragma
			});
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00039954 File Offset: 0x00038954
		internal virtual CodeExpression BuildPagePropertyReferenceExpression()
		{
			return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.pagePropertyName);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00039968 File Offset: 0x00038968
		protected CodeMemberMethod BuildBuildMethod(ControlBuilder builder, bool fTemplate, bool fInTemplate, bool topLevelControlInTemplate, PropertyEntry pse, bool fControlSkin)
		{
			ServiceContainer serviceContainer = new ServiceContainer();
			serviceContainer.AddService(typeof(IFilterResolutionService), HttpCapabilitiesBase.EmptyHttpCapabilitiesBase);
			try
			{
				builder.SetServiceProvider(serviceContainer);
				builder.EnsureEntriesSorted();
			}
			finally
			{
				builder.SetServiceProvider(null);
			}
			string methodNameForBuilder = this.GetMethodNameForBuilder(BaseTemplateCodeDomTreeGenerator.buildMethodPrefix, builder);
			Type ctrlTypeForBuilder = this.GetCtrlTypeForBuilder(builder, fTemplate);
			bool flag = false;
			bool flag2 = false;
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = methodNameForBuilder;
			codeMemberMethod.Attributes = (MemberAttributes)20482;
			this._sourceDataClass.Members.Add(codeMemberMethod);
			ComplexPropertyEntry complexPropertyEntry = pse as ComplexPropertyEntry;
			if (fTemplate || (complexPropertyEntry != null && complexPropertyEntry.ReadOnly))
			{
				if (builder is RootBuilder)
				{
					codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(this._sourceDataClass.Name, "__ctrl"));
				}
				else
				{
					codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(ctrlTypeForBuilder, "__ctrl"));
				}
			}
			else
			{
				if (typeof(Control).IsAssignableFrom(builder.ControlType))
				{
					flag = true;
				}
				if (builder.ControlType != null)
				{
					if (fControlSkin)
					{
						if (flag)
						{
							codeMemberMethod.ReturnType = new CodeTypeReference(typeof(Control));
						}
					}
					else
					{
						PartialCachingAttribute partialCachingAttribute = (PartialCachingAttribute)TypeDescriptor.GetAttributes(builder.ControlType)[typeof(PartialCachingAttribute)];
						if (partialCachingAttribute != null)
						{
							codeMemberMethod.ReturnType = new CodeTypeReference(typeof(Control));
						}
						else
						{
							codeMemberMethod.ReturnType = new CodeTypeReference(builder.ControlType, CodeTypeReferenceOptions.GlobalReference);
						}
					}
				}
				flag2 = true;
			}
			if (fControlSkin)
			{
				codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Control).FullName, "ctrl"));
			}
			this.BuildBuildMethodInternal(builder, builder.ControlType, fInTemplate, topLevelControlInTemplate, pse, codeMemberMethod.Statements, flag, flag2, null, fControlSkin);
			return codeMemberMethod;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00039B4C File Offset: 0x00038B4C
		private void BuildBuildMethodInternal(ControlBuilder builder, Type ctrlType, bool fInTemplate, bool topLevelControlInTemplate, PropertyEntry pse, CodeStatementCollection statements, bool fStandardControl, bool fControlFieldDeclared, string deviceFilter, bool fControlSkin)
		{
			CodeLinePragma codeLinePragma = base.CreateCodeLinePragma(builder);
			CodeExpression codeExpression;
			if (fControlSkin)
			{
				CodeCastExpression codeCastExpression = new CodeCastExpression(builder.ControlType.FullName, new CodeArgumentReferenceExpression("ctrl"));
				statements.Add(new CodeVariableDeclarationStatement(builder.ControlType.FullName, "__ctrl", codeCastExpression));
				codeExpression = new CodeVariableReferenceExpression("__ctrl");
			}
			else if (!fControlFieldDeclared)
			{
				codeExpression = new CodeArgumentReferenceExpression("__ctrl");
			}
			else
			{
				CodeTypeReference codeTypeReference = new CodeTypeReference(ctrlType, CodeTypeReferenceOptions.GlobalReference);
				CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(codeTypeReference, new CodeExpression[0]);
				ConstructorNeedsTagAttribute constructorNeedsTagAttribute = (ConstructorNeedsTagAttribute)TypeDescriptor.GetAttributes(ctrlType)[typeof(ConstructorNeedsTagAttribute)];
				if (constructorNeedsTagAttribute != null && constructorNeedsTagAttribute.NeedsTag)
				{
					codeObjectCreateExpression.Parameters.Add(new CodePrimitiveExpression(builder.TagName));
				}
				DataBoundLiteralControlBuilder dataBoundLiteralControlBuilder = builder as DataBoundLiteralControlBuilder;
				if (dataBoundLiteralControlBuilder != null)
				{
					codeObjectCreateExpression.Parameters.Add(new CodePrimitiveExpression(dataBoundLiteralControlBuilder.GetStaticLiteralsCount()));
					codeObjectCreateExpression.Parameters.Add(new CodePrimitiveExpression(dataBoundLiteralControlBuilder.GetDataBoundLiteralCount()));
				}
				statements.Add(new CodeVariableDeclarationStatement(codeTypeReference, "__ctrl"));
				codeExpression = new CodeVariableReferenceExpression("__ctrl");
				statements.Add(new CodeAssignStatement(codeExpression, codeObjectCreateExpression)
				{
					LinePragma = codeLinePragma
				});
				if (!builder.IsGeneratedID)
				{
					CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), builder.ID);
					CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeFieldReferenceExpression, codeExpression);
					statements.Add(codeAssignStatement);
				}
				if (topLevelControlInTemplate && !typeof(TemplateControl).IsAssignableFrom(ctrlType))
				{
					statements.Add(this.BuildTemplatePropertyStatement(codeExpression));
				}
				if (fStandardControl)
				{
					if (builder.SkinID != null)
					{
						statements.Add(new CodeAssignStatement
						{
							Left = new CodePropertyReferenceExpression(codeExpression, "SkinID"),
							Right = new CodePrimitiveExpression(builder.SkinID)
						});
					}
					if (ThemeableAttribute.IsTypeThemeable(ctrlType))
					{
						statements.Add(new CodeMethodInvokeExpression(codeExpression, BaseTemplateCodeDomTreeGenerator.applyStyleSheetMethodName, new CodeExpression[0])
						{
							Parameters = { this.BuildPagePropertyReferenceExpression() }
						});
					}
				}
			}
			if (builder.TemplatePropertyEntries.Count > 0)
			{
				CodeStatementCollection codeStatementCollection = statements;
				PropertyEntry propertyEntry = null;
				foreach (object obj in builder.TemplatePropertyEntries)
				{
					TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj;
					CodeStatementCollection codeStatementCollection2 = codeStatementCollection;
					this.HandleDeviceFilterConditional(ref propertyEntry, templatePropertyEntry, statements, ref codeStatementCollection2, out codeStatementCollection);
					string id = templatePropertyEntry.Builder.ID;
					CodeDelegateCreateExpression codeDelegateCreateExpression = new CodeDelegateCreateExpression();
					codeDelegateCreateExpression.DelegateType = new CodeTypeReference(typeof(BuildTemplateMethod));
					codeDelegateCreateExpression.TargetObject = new CodeThisReferenceExpression();
					codeDelegateCreateExpression.MethodName = BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + id;
					CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement();
					if (templatePropertyEntry.PropertyInfo != null)
					{
						codeAssignStatement2.Left = new CodePropertyReferenceExpression(codeExpression, templatePropertyEntry.Name);
					}
					else
					{
						codeAssignStatement2.Left = new CodeFieldReferenceExpression(codeExpression, templatePropertyEntry.Name);
					}
					CodeObjectCreateExpression codeObjectCreateExpression;
					if (templatePropertyEntry.BindableTemplate)
					{
						CodeExpression codeExpression2;
						if (templatePropertyEntry.Builder.HasTwoWayBoundProperties)
						{
							codeExpression2 = new CodeDelegateCreateExpression();
							((CodeDelegateCreateExpression)codeExpression2).DelegateType = new CodeTypeReference(typeof(ExtractTemplateValuesMethod));
							((CodeDelegateCreateExpression)codeExpression2).TargetObject = new CodeThisReferenceExpression();
							((CodeDelegateCreateExpression)codeExpression2).MethodName = BaseTemplateCodeDomTreeGenerator.extractTemplateValuesMethodPrefix + id;
						}
						else
						{
							codeExpression2 = new CodePrimitiveExpression(null);
						}
						codeObjectCreateExpression = new CodeObjectCreateExpression(typeof(CompiledBindableTemplateBuilder), new CodeExpression[0]);
						codeObjectCreateExpression.Parameters.Add(codeDelegateCreateExpression);
						codeObjectCreateExpression.Parameters.Add(codeExpression2);
					}
					else
					{
						codeObjectCreateExpression = new CodeObjectCreateExpression(typeof(CompiledTemplateBuilder), new CodeExpression[0]);
						codeObjectCreateExpression.Parameters.Add(codeDelegateCreateExpression);
					}
					codeAssignStatement2.Right = codeObjectCreateExpression;
					codeAssignStatement2.LinePragma = base.CreateCodeLinePragma(templatePropertyEntry.Builder);
					codeStatementCollection2.Add(codeAssignStatement2);
				}
			}
			if (typeof(UserControl).IsAssignableFrom(ctrlType) && fControlFieldDeclared && !fControlSkin)
			{
				statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(codeExpression, "InitializeAsUserControl", new CodeExpression[0])
				{
					Parameters = 
					{
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.pagePropertyName)
					}
				})
				{
					LinePragma = codeLinePragma
				});
			}
			if (builder.SimplePropertyEntries.Count > 0)
			{
				CodeStatementCollection codeStatementCollection3 = statements;
				PropertyEntry propertyEntry2 = null;
				foreach (object obj2 in builder.SimplePropertyEntries)
				{
					SimplePropertyEntry simplePropertyEntry = (SimplePropertyEntry)obj2;
					CodeStatementCollection codeStatementCollection4 = codeStatementCollection3;
					this.HandleDeviceFilterConditional(ref propertyEntry2, simplePropertyEntry, statements, ref codeStatementCollection4, out codeStatementCollection3);
					CodeStatement codeStatement = simplePropertyEntry.GetCodeStatement(this, codeExpression);
					codeStatement.LinePragma = codeLinePragma;
					codeStatementCollection4.Add(codeStatement);
				}
			}
			if (typeof(Page).IsAssignableFrom(ctrlType) && !fControlSkin)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InitializeCulture", new CodeExpression[0]);
				statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression)
				{
					LinePragma = codeLinePragma
				});
			}
			CodeStatementCollection codeStatementCollection5 = statements;
			if (builder is ContentPlaceHolderBuilder)
			{
				string name = ((ContentPlaceHolderBuilder)builder).Name;
				string text = MasterPageControlBuilder.AutoTemplatePrefix + name;
				string text2 = "__" + text;
				Type type = builder.BindingContainerType;
				if (!typeof(INamingContainer).IsAssignableFrom(type))
				{
					if (typeof(INamingContainer).IsAssignableFrom(this.Parser.BaseType))
					{
						type = this.Parser.BaseType;
					}
					else
					{
						type = typeof(Control);
					}
				}
				CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("TemplateContainer", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodeTypeOfExpression(type))
				});
				codeAttributeDeclarationCollection.Add(codeAttributeDeclaration);
				if (!fInTemplate)
				{
					CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration("TemplateInstanceAttribute", new CodeAttributeArgument[]
					{
						new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(TemplateInstance)), "Single"))
					});
					codeAttributeDeclarationCollection.Add(codeAttributeDeclaration2);
				}
				base.BuildFieldAndAccessorProperty(text, text2, typeof(ITemplate), false, codeAttributeDeclarationCollection);
				CodeExpression codeExpression3 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text2);
				if (builder is ContentPlaceHolderBuilder)
				{
					CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "ContentTemplates");
					CodeAssignStatement codeAssignStatement3 = new CodeAssignStatement();
					codeAssignStatement3.Left = codeExpression3;
					codeAssignStatement3.Right = new CodeCastExpression(typeof(ITemplate), new CodeIndexerExpression(codePropertyReferenceExpression, new CodeExpression[]
					{
						new CodePrimitiveExpression(name)
					}));
					CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
					CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(codePropertyReferenceExpression, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
					CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(codePropertyReferenceExpression, "Remove", new CodeExpression[0]);
					codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(name));
					codeConditionStatement.Condition = codeBinaryOperatorExpression;
					codeConditionStatement.TrueStatements.Add(codeAssignStatement3);
					statements.Add(codeConditionStatement);
				}
				CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression(codeExpression3, "InstantiateIn", new CodeExpression[0]);
				codeMethodInvokeExpression3.Parameters.Add(codeExpression);
				CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
				codeConditionStatement2.Condition = new CodeBinaryOperatorExpression(codeExpression3, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
				codeConditionStatement2.TrueStatements.Add(new CodeExpressionStatement(codeMethodInvokeExpression3));
				codeStatementCollection5 = codeConditionStatement2.FalseStatements;
				statements.Add(codeConditionStatement2);
			}
			if (builder is FileLevelPageControlBuilder)
			{
				ICollection contentBuilderEntries = ((FileLevelPageControlBuilder)builder).ContentBuilderEntries;
				if (contentBuilderEntries != null)
				{
					CodeStatementCollection codeStatementCollection6 = statements;
					PropertyEntry propertyEntry3 = null;
					foreach (object obj3 in contentBuilderEntries)
					{
						TemplatePropertyEntry templatePropertyEntry2 = (TemplatePropertyEntry)obj3;
						ContentBuilderInternal contentBuilderInternal = (ContentBuilderInternal)templatePropertyEntry2.Builder;
						CodeStatementCollection codeStatementCollection7 = codeStatementCollection6;
						this.HandleDeviceFilterConditional(ref propertyEntry3, templatePropertyEntry2, statements, ref codeStatementCollection7, out codeStatementCollection6);
						string id2 = contentBuilderInternal.ID;
						string contentPlaceHolder = contentBuilderInternal.ContentPlaceHolder;
						CodeDelegateCreateExpression codeDelegateCreateExpression2 = new CodeDelegateCreateExpression();
						codeDelegateCreateExpression2.DelegateType = new CodeTypeReference(typeof(BuildTemplateMethod));
						codeDelegateCreateExpression2.TargetObject = new CodeThisReferenceExpression();
						codeDelegateCreateExpression2.MethodName = BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + id2;
						CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression(typeof(CompiledTemplateBuilder), new CodeExpression[0]);
						codeObjectCreateExpression2.Parameters.Add(codeDelegateCreateExpression2);
						CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "AddContentTemplate", new CodeExpression[0])
						{
							Parameters = 
							{
								new CodePrimitiveExpression(contentPlaceHolder),
								codeObjectCreateExpression2
							}
						});
						codeExpressionStatement.LinePragma = base.CreateCodeLinePragma(contentBuilderInternal);
						codeStatementCollection7.Add(codeExpressionStatement);
					}
				}
			}
			if (builder is DataBoundLiteralControlBuilder)
			{
				int num = -1;
				using (IEnumerator enumerator = builder.SubBuilders.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj4 = enumerator.Current;
						num++;
						if (obj4 != null && num % 2 != 1)
						{
							string text3 = (string)obj4;
							statements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(codeExpression, "SetStaticString", new CodeExpression[0])
							{
								Parameters = 
								{
									new CodePrimitiveExpression(num / 2),
									new CodePrimitiveExpression(text3)
								}
							}));
						}
					}
					goto IL_0D82;
				}
			}
			if (builder.SubBuilders != null)
			{
				bool flag = false;
				int num2 = 1;
				foreach (object obj5 in builder.SubBuilders)
				{
					if (obj5 is ControlBuilder && !(obj5 is CodeBlockBuilder) && !(obj5 is ContentBuilderInternal))
					{
						ControlBuilder controlBuilder = (ControlBuilder)obj5;
						if (fControlSkin)
						{
							throw new HttpParseException(SR.GetString("ControlSkin_cannot_contain_controls"), null, builder.VirtualPath, null, builder.Line);
						}
						PartialCachingAttribute partialCachingAttribute = (PartialCachingAttribute)TypeDescriptor.GetAttributes(controlBuilder.ControlType)[typeof(PartialCachingAttribute)];
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + controlBuilder.ID, new CodeExpression[0]);
						CodeExpressionStatement codeExpressionStatement2 = new CodeExpressionStatement(codeMethodInvokeExpression);
						if (partialCachingAttribute == null)
						{
							string text4 = "__ctrl" + num2++.ToString(CultureInfo.InvariantCulture);
							CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression(text4);
							CodeTypeReference codeTypeReference2 = new CodeTypeReference(controlBuilder.ControlType, CodeTypeReferenceOptions.GlobalReference);
							codeStatementCollection5.Add(new CodeVariableDeclarationStatement(codeTypeReference2, text4));
							codeStatementCollection5.Add(new CodeAssignStatement(codeVariableReferenceExpression, codeMethodInvokeExpression)
							{
								LinePragma = codeLinePragma
							});
							BaseTemplateCodeDomTreeGenerator.BuildAddParsedSubObjectStatement(codeStatementCollection5, codeVariableReferenceExpression, codeLinePragma, codeExpression, ref flag);
						}
						else
						{
							CodeMethodInvokeExpression codeMethodInvokeExpression4 = new CodeMethodInvokeExpression();
							codeMethodInvokeExpression4.Method.TargetObject = new CodeTypeReferenceExpression(typeof(StaticPartialCachingControl));
							codeMethodInvokeExpression4.Method.MethodName = "BuildCachedControl";
							codeMethodInvokeExpression4.Parameters.Add(codeExpression);
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(controlBuilder.ID));
							if (partialCachingAttribute.Shared)
							{
								codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(controlBuilder.ControlType.GetHashCode().ToString(CultureInfo.InvariantCulture)));
							}
							else
							{
								codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(Guid.NewGuid().ToString()));
							}
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(partialCachingAttribute.Duration));
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(partialCachingAttribute.VaryByParams));
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(partialCachingAttribute.VaryByControls));
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(partialCachingAttribute.VaryByCustom));
							codeMethodInvokeExpression4.Parameters.Add(new CodePrimitiveExpression(partialCachingAttribute.SqlDependency));
							CodeDelegateCreateExpression codeDelegateCreateExpression3 = new CodeDelegateCreateExpression();
							codeDelegateCreateExpression3.DelegateType = new CodeTypeReference(typeof(BuildMethod));
							codeDelegateCreateExpression3.TargetObject = new CodeThisReferenceExpression();
							codeDelegateCreateExpression3.MethodName = BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + controlBuilder.ID;
							codeMethodInvokeExpression4.Parameters.Add(codeDelegateCreateExpression3);
							codeStatementCollection5.Add(new CodeExpressionStatement(codeMethodInvokeExpression4));
						}
					}
					else if (obj5 is string && !builder.HasAspCode && (!fControlSkin || !builder.AllowWhitespaceLiterals()))
					{
						string text5 = (string)obj5;
						CodeExpression codeExpression4;
						if (!this.UseResourceLiteralString(text5))
						{
							codeExpression4 = new CodeObjectCreateExpression(typeof(LiteralControl), new CodeExpression[0])
							{
								Parameters = 
								{
									new CodePrimitiveExpression(text5)
								}
							};
						}
						else
						{
							int num3;
							int num4;
							bool flag2;
							this._stringResourceBuilder.AddString(text5, out num3, out num4, out flag2);
							codeExpression4 = new CodeMethodInvokeExpression
							{
								Method = 
								{
									TargetObject = new CodeThisReferenceExpression(),
									MethodName = "CreateResourceBasedLiteralControl"
								},
								Parameters = 
								{
									new CodePrimitiveExpression(num3),
									new CodePrimitiveExpression(num4),
									new CodePrimitiveExpression(flag2)
								}
							};
						}
						BaseTemplateCodeDomTreeGenerator.BuildAddParsedSubObjectStatement(codeStatementCollection5, codeExpression4, codeLinePragma, codeExpression, ref flag);
					}
				}
			}
			IL_0D82:
			if (builder.ComplexPropertyEntries.Count > 0)
			{
				CodeStatementCollection codeStatementCollection8 = statements;
				PropertyEntry propertyEntry4 = null;
				int num5 = 1;
				foreach (object obj6 in builder.ComplexPropertyEntries)
				{
					ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj6;
					CodeStatementCollection codeStatementCollection9 = codeStatementCollection8;
					this.HandleDeviceFilterConditional(ref propertyEntry4, complexPropertyEntry, statements, ref codeStatementCollection9, out codeStatementCollection8);
					if (complexPropertyEntry.Builder is StringPropertyBuilder)
					{
						CodeExpression codeExpression5 = new CodePropertyReferenceExpression(codeExpression, complexPropertyEntry.Name);
						CodeExpression codeExpression6 = this.BuildStringPropertyExpression(complexPropertyEntry);
						CodeAssignStatement codeAssignStatement4 = new CodeAssignStatement(codeExpression5, codeExpression6);
						codeAssignStatement4.LinePragma = codeLinePragma;
						codeStatementCollection9.Add(codeAssignStatement4);
					}
					else if (complexPropertyEntry.ReadOnly)
					{
						if (fControlSkin && complexPropertyEntry.Builder != null && complexPropertyEntry.Builder is CollectionBuilder && complexPropertyEntry.Builder.ComplexPropertyEntries.Count > 0)
						{
							BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
							if (complexPropertyEntry.Type.GetMethod("Clear", bindingFlags) != null)
							{
								CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression();
								codeMethodReferenceExpression.MethodName = "Clear";
								codeMethodReferenceExpression.TargetObject = new CodePropertyReferenceExpression(codeExpression, complexPropertyEntry.Name);
								CodeMethodInvokeExpression codeMethodInvokeExpression5 = new CodeMethodInvokeExpression();
								codeMethodInvokeExpression5.Method = codeMethodReferenceExpression;
								codeStatementCollection9.Add(codeMethodInvokeExpression5);
							}
						}
						CodeExpressionStatement codeExpressionStatement2 = new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + complexPropertyEntry.Builder.ID, new CodeExpression[0])
						{
							Parameters = 
							{
								new CodePropertyReferenceExpression(codeExpression, complexPropertyEntry.Name)
							}
						});
						codeExpressionStatement2.LinePragma = codeLinePragma;
						codeStatementCollection9.Add(codeExpressionStatement2);
					}
					else
					{
						string text6 = "__ctrl" + num5++.ToString(CultureInfo.InvariantCulture);
						CodeTypeReference codeTypeReference3 = new CodeTypeReference(complexPropertyEntry.Builder.ControlType, CodeTypeReferenceOptions.GlobalReference);
						codeStatementCollection9.Add(new CodeVariableDeclarationStatement(codeTypeReference3, text6));
						CodeVariableReferenceExpression codeVariableReferenceExpression2 = new CodeVariableReferenceExpression(text6);
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), BaseTemplateCodeDomTreeGenerator.buildMethodPrefix + complexPropertyEntry.Builder.ID, new CodeExpression[0]);
						CodeExpressionStatement codeExpressionStatement2 = new CodeExpressionStatement(codeMethodInvokeExpression);
						CodeAssignStatement codeAssignStatement5 = new CodeAssignStatement(codeVariableReferenceExpression2, codeMethodInvokeExpression);
						codeAssignStatement5.LinePragma = codeLinePragma;
						codeStatementCollection9.Add(codeAssignStatement5);
						if (complexPropertyEntry.IsCollectionItem)
						{
							codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeExpression, "Add", new CodeExpression[0]);
							codeExpressionStatement2 = new CodeExpressionStatement(codeMethodInvokeExpression);
							codeExpressionStatement2.LinePragma = codeLinePragma;
							codeStatementCollection9.Add(codeExpressionStatement2);
							codeMethodInvokeExpression.Parameters.Add(codeVariableReferenceExpression2);
						}
						else
						{
							CodeAssignStatement codeAssignStatement6 = new CodeAssignStatement();
							codeAssignStatement6.Left = new CodePropertyReferenceExpression(codeExpression, complexPropertyEntry.Name);
							codeAssignStatement6.Right = codeVariableReferenceExpression2;
							codeAssignStatement6.LinePragma = codeLinePragma;
							codeStatementCollection9.Add(codeAssignStatement6);
						}
					}
				}
			}
			if (builder.BoundPropertyEntries.Count > 0)
			{
				bool flag3 = builder is BindableTemplateBuilder;
				bool flag4 = false;
				CodeStatementCollection codeStatementCollection10 = statements;
				PropertyEntry propertyEntry5 = null;
				bool flag5 = false;
				foreach (object obj7 in builder.BoundPropertyEntries)
				{
					BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj7;
					if (!boundPropertyEntry.TwoWayBound || (!flag3 && !boundPropertyEntry.ReadOnlyProperty))
					{
						if (boundPropertyEntry.IsDataBindingEntry)
						{
							flag4 = true;
						}
						else
						{
							CodeStatementCollection codeStatementCollection11 = codeStatementCollection10;
							this.HandleDeviceFilterConditional(ref propertyEntry5, boundPropertyEntry, statements, ref codeStatementCollection11, out codeStatementCollection10);
							ExpressionBuilder expressionBuilder = boundPropertyEntry.ExpressionBuilder;
							expressionBuilder.BuildExpression(boundPropertyEntry, builder, codeExpression, statements, codeStatementCollection11, null, ref flag5);
						}
					}
				}
				if (flag4)
				{
					EventInfo @event = DataBindingExpressionBuilder.Event;
					CodeDelegateCreateExpression codeDelegateCreateExpression4 = new CodeDelegateCreateExpression();
					CodeAttachEventStatement codeAttachEventStatement = new CodeAttachEventStatement(codeExpression, @event.Name, codeDelegateCreateExpression4);
					codeAttachEventStatement.LinePragma = codeLinePragma;
					codeDelegateCreateExpression4.DelegateType = new CodeTypeReference(typeof(EventHandler));
					codeDelegateCreateExpression4.TargetObject = new CodeThisReferenceExpression();
					codeDelegateCreateExpression4.MethodName = this.GetExpressionBuilderMethodName(@event.Name, builder);
					statements.Add(codeAttachEventStatement);
				}
			}
			if (builder is DataBoundLiteralControlBuilder)
			{
				CodeDelegateCreateExpression codeDelegateCreateExpression5 = new CodeDelegateCreateExpression();
				CodeAttachEventStatement codeAttachEventStatement2 = new CodeAttachEventStatement(codeExpression, "DataBinding", codeDelegateCreateExpression5);
				codeAttachEventStatement2.LinePragma = codeLinePragma;
				codeDelegateCreateExpression5.DelegateType = new CodeTypeReference(typeof(EventHandler));
				codeDelegateCreateExpression5.TargetObject = new CodeThisReferenceExpression();
				codeDelegateCreateExpression5.MethodName = this.BindingMethodName(builder);
				statements.Add(codeAttachEventStatement2);
			}
			if (builder.HasAspCode && !fControlSkin)
			{
				CodeDelegateCreateExpression codeDelegateCreateExpression6 = new CodeDelegateCreateExpression();
				codeDelegateCreateExpression6.DelegateType = new CodeTypeReference(typeof(RenderMethod));
				codeDelegateCreateExpression6.TargetObject = new CodeThisReferenceExpression();
				codeDelegateCreateExpression6.MethodName = "__Render" + builder.ID;
				CodeExpressionStatement codeExpressionStatement2 = new CodeExpressionStatement(new CodeMethodInvokeExpression(codeExpression, "SetRenderMethodDelegate", new CodeExpression[0])
				{
					Parameters = { codeDelegateCreateExpression6 }
				});
				if (builder is ContentPlaceHolderBuilder)
				{
					string name2 = ((ContentPlaceHolderBuilder)builder).Name;
					string text = MasterPageControlBuilder.AutoTemplatePrefix + name2;
					string text7 = "__" + text;
					CodeExpression codeExpression7 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text7);
					statements.Add(new CodeConditionStatement
					{
						Condition = new CodeBinaryOperatorExpression(codeExpression7, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
						TrueStatements = { codeExpressionStatement2 }
					});
				}
				else
				{
					statements.Add(codeExpressionStatement2);
				}
			}
			if (builder.EventEntries.Count > 0)
			{
				foreach (object obj8 in builder.EventEntries)
				{
					EventEntry eventEntry = (EventEntry)obj8;
					CodeDelegateCreateExpression codeDelegateCreateExpression7 = new CodeDelegateCreateExpression();
					codeDelegateCreateExpression7.DelegateType = new CodeTypeReference(eventEntry.HandlerType);
					codeDelegateCreateExpression7.TargetObject = new CodeThisReferenceExpression();
					codeDelegateCreateExpression7.MethodName = eventEntry.HandlerMethodName;
					if (this.Parser.HasCodeBehind)
					{
						statements.Add(new CodeRemoveEventStatement(codeExpression, eventEntry.Name, codeDelegateCreateExpression7)
						{
							LinePragma = codeLinePragma
						});
					}
					statements.Add(new CodeAttachEventStatement(codeExpression, eventEntry.Name, codeDelegateCreateExpression7)
					{
						LinePragma = codeLinePragma
					});
				}
			}
			if (fControlFieldDeclared)
			{
				statements.Add(new CodeMethodReturnStatement(codeExpression));
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0003AFE0 File Offset: 0x00039FE0
		protected void BuildExtractMethod(ControlBuilder builder)
		{
			BindableTemplateBuilder bindableTemplateBuilder = builder as BindableTemplateBuilder;
			if (bindableTemplateBuilder != null && bindableTemplateBuilder.HasTwoWayBoundProperties)
			{
				string text = this.ExtractMethodName(builder);
				CodeLinePragma codeLinePragma = base.CreateCodeLinePragma(builder);
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
				codeMemberMethod.Name = text;
				codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
				codeMemberMethod.Attributes |= MemberAttributes.Public;
				codeMemberMethod.ReturnType = new CodeTypeReference(typeof(IOrderedDictionary));
				this._sourceDataClass.Members.Add(codeMemberMethod);
				CodeStatementCollection statements = codeMemberMethod.Statements;
				CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
				codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Control), "__container"));
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(typeof(OrderedDictionary), "__table");
				statements.Add(codeVariableDeclarationStatement);
				CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(typeof(OrderedDictionary), new CodeExpression[0]);
				codeStatementCollection.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("__table"), codeObjectCreateExpression)
				{
					LinePragma = codeLinePragma
				});
				this.BuildExtractStatementsRecursive(bindableTemplateBuilder.SubBuilders, codeStatementCollection, statements, codeLinePragma, "__table", "__container");
				CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeVariableReferenceExpression("__table"));
				codeStatementCollection.Add(codeMethodReturnStatement);
				codeMemberMethod.Statements.AddRange(codeStatementCollection);
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0003B144 File Offset: 0x0003A144
		private void BuildExtractStatementsRecursive(ArrayList subBuilders, CodeStatementCollection statements, CodeStatementCollection topLevelStatements, CodeLinePragma linePragma, string tableVarName, string containerVarName)
		{
			foreach (object obj in subBuilders)
			{
				ControlBuilder controlBuilder = obj as ControlBuilder;
				if (controlBuilder != null)
				{
					CodeStatementCollection codeStatementCollection = null;
					CodeStatementCollection codeStatementCollection2 = statements;
					PropertyEntry propertyEntry = null;
					string text = null;
					foreach (object obj2 in controlBuilder.BoundPropertyEntries)
					{
						BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj2;
						if (boundPropertyEntry.TwoWayBound)
						{
							bool flag;
							if (string.Compare(text, boundPropertyEntry.ControlID, StringComparison.Ordinal) != 0)
							{
								propertyEntry = null;
								flag = true;
							}
							else
							{
								flag = false;
							}
							text = boundPropertyEntry.ControlID;
							codeStatementCollection = codeStatementCollection2;
							this.HandleDeviceFilterConditional(ref propertyEntry, boundPropertyEntry, statements, ref codeStatementCollection, out codeStatementCollection2);
							if (flag)
							{
								CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(boundPropertyEntry.ControlType, boundPropertyEntry.ControlID);
								topLevelStatements.Add(codeVariableDeclarationStatement);
								CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(containerVarName), "FindControl", new CodeExpression[0]);
								string controlID = boundPropertyEntry.ControlID;
								codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(controlID));
								CodeCastExpression codeCastExpression = new CodeCastExpression(boundPropertyEntry.ControlType, codeMethodInvokeExpression);
								topLevelStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(boundPropertyEntry.ControlID), codeCastExpression)
								{
									LinePragma = linePragma
								});
							}
							CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
							codeConditionStatement.Condition = new CodeBinaryOperatorExpression
							{
								Operator = CodeBinaryOperatorType.IdentityInequality,
								Left = new CodeVariableReferenceExpression(boundPropertyEntry.ControlID),
								Right = new CodePrimitiveExpression(null)
							};
							string fieldName = boundPropertyEntry.FieldName;
							CodeIndexerExpression codeIndexerExpression = new CodeIndexerExpression(new CodeVariableReferenceExpression(tableVarName), new CodeExpression[]
							{
								new CodePrimitiveExpression(fieldName)
							});
							CodeExpression codeExpression = CodeDomUtility.BuildPropertyReferenceExpression(new CodeVariableReferenceExpression(boundPropertyEntry.ControlID), boundPropertyEntry.Name);
							if (this._usingVJSCompiler)
							{
								codeExpression = CodeDomUtility.BuildJSharpCastExpression(boundPropertyEntry.Type, codeExpression);
							}
							CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeIndexerExpression, codeExpression);
							codeConditionStatement.TrueStatements.Add(codeAssignStatement);
							codeConditionStatement.LinePragma = linePragma;
							codeStatementCollection.Add(codeConditionStatement);
						}
					}
					if (controlBuilder.SubBuilders.Count > 0)
					{
						this.BuildExtractStatementsRecursive(controlBuilder.SubBuilders, statements, topLevelStatements, linePragma, tableVarName, containerVarName);
					}
				}
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0003B3DC File Offset: 0x0003A3DC
		private void BuildFieldDeclaration(ControlBuilder builder)
		{
			if (builder is ContentBuilderInternal)
			{
				return;
			}
			bool flag = false;
			if (this.Parser.BaseType != null)
			{
				Type type = Util.GetNonPrivateFieldType(this.Parser.BaseType, builder.ID);
				if (type == null)
				{
					type = Util.GetNonPrivatePropertyType(this.Parser.BaseType, builder.ID);
				}
				if (type != null)
				{
					if (type.IsAssignableFrom(builder.ControlType))
					{
						return;
					}
					if (typeof(Control).IsAssignableFrom(type))
					{
						throw new HttpParseException(SR.GetString("Base_class_field_with_type_different_from_type_of_control", new object[]
						{
							builder.ID,
							type.FullName,
							builder.ControlType.FullName
						}), null, builder.VirtualPath, null, builder.Line);
					}
					flag = true;
				}
			}
			CodeMemberField codeMemberField = new CodeMemberField(new CodeTypeReference(builder.DeclareType, CodeTypeReferenceOptions.GlobalReference), builder.ID);
			codeMemberField.Attributes &= (MemberAttributes)(-61441);
			if (flag)
			{
				codeMemberField.Attributes |= MemberAttributes.New;
			}
			codeMemberField.LinePragma = base.CreateCodeLinePragma(builder);
			codeMemberField.Attributes |= MemberAttributes.Family;
			if (typeof(Control).IsAssignableFrom(builder.DeclareType))
			{
				codeMemberField.UserData["WithEvents"] = true;
			}
			this._intermediateClass.Members.Add(codeMemberField);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0003B53D File Offset: 0x0003A53D
		private string GetExpressionBuilderMethodName(string eventName, ControlBuilder builder)
		{
			return "__" + eventName + builder.ID;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0003B550 File Offset: 0x0003A550
		private string BindingMethodName(ControlBuilder builder)
		{
			return "__DataBind" + builder.ID;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0003B564 File Offset: 0x0003A564
		protected CodeMemberMethod BuildPropertyBindingMethod(ControlBuilder builder, bool fControlSkin)
		{
			if (builder is DataBoundLiteralControlBuilder)
			{
				string text = this.BindingMethodName(builder);
				CodeLinePragma codeLinePragma = base.CreateCodeLinePragma(builder);
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = text;
				codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
				codeMemberMethod.Attributes |= MemberAttributes.Public;
				codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "sender"));
				codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(EventArgs), "e"));
				CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
				CodeStatementCollection codeStatementCollection2 = new CodeStatementCollection();
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(builder.ControlType, "target");
				Type bindingContainerType = builder.BindingContainerType;
				CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = new CodeVariableDeclarationStatement(bindingContainerType, "Container");
				codeStatementCollection.Add(codeVariableDeclarationStatement2);
				codeStatementCollection.Add(codeVariableDeclarationStatement);
				codeStatementCollection2.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(codeVariableDeclarationStatement.Name), new CodeCastExpression(builder.ControlType, new CodeArgumentReferenceExpression("sender")))
				{
					LinePragma = codeLinePragma
				});
				codeStatementCollection2.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(codeVariableDeclarationStatement2.Name), new CodeCastExpression(bindingContainerType, new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("target"), "BindingContainer")))
				{
					LinePragma = codeLinePragma
				});
				bool flag = false;
				int num = -1;
				foreach (object obj in builder.SubBuilders)
				{
					num++;
					if (obj != null && num % 2 != 0)
					{
						CodeBlockBuilder codeBlockBuilder = (CodeBlockBuilder)obj;
						if (this._designerMode)
						{
							if (!flag)
							{
								flag = true;
								codeStatementCollection.Add(new CodeVariableDeclarationStatement(typeof(object), "__o"));
							}
							codeStatementCollection2.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("__o"), new CodeSnippetExpression(codeBlockBuilder.Content))
							{
								LinePragma = base.CreateCodeLinePragma(codeBlockBuilder)
							});
						}
						else
						{
							CodeExpression codeExpression = CodeDomUtility.GenerateConvertToString(new CodeSnippetExpression(codeBlockBuilder.Content.Trim()));
							codeStatementCollection2.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("target"), "SetDataBoundString", new CodeExpression[0])
							{
								Parameters = 
								{
									new CodePrimitiveExpression(num / 2),
									codeExpression
								}
							})
							{
								LinePragma = base.CreateCodeLinePragma(codeBlockBuilder)
							});
						}
					}
				}
				foreach (object obj2 in codeStatementCollection)
				{
					CodeStatement codeStatement = (CodeStatement)obj2;
					codeMemberMethod.Statements.Add(codeStatement);
				}
				foreach (object obj3 in codeStatementCollection2)
				{
					CodeStatement codeStatement2 = (CodeStatement)obj3;
					codeMemberMethod.Statements.Add(codeStatement2);
				}
				this._sourceDataClass.Members.Add(codeMemberMethod);
				return codeMemberMethod;
			}
			EventInfo @event = DataBindingExpressionBuilder.Event;
			CodeLinePragma codeLinePragma2 = base.CreateCodeLinePragma(builder);
			CodeMemberMethod codeMemberMethod2 = null;
			CodeStatementCollection codeStatementCollection3 = null;
			CodeStatementCollection codeStatementCollection4 = null;
			CodeStatementCollection codeStatementCollection5 = null;
			PropertyEntry propertyEntry = null;
			bool flag2 = builder is BindableTemplateBuilder;
			bool flag3 = true;
			bool flag4 = false;
			foreach (object obj4 in builder.BoundPropertyEntries)
			{
				BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj4;
				if ((!boundPropertyEntry.TwoWayBound || (!flag2 && !boundPropertyEntry.ReadOnlyProperty)) && boundPropertyEntry.IsDataBindingEntry)
				{
					if (flag3)
					{
						flag3 = false;
						codeMemberMethod2 = new CodeMemberMethod();
						codeStatementCollection3 = new CodeStatementCollection();
						codeStatementCollection4 = new CodeStatementCollection();
						string expressionBuilderMethodName = this.GetExpressionBuilderMethodName(@event.Name, builder);
						codeMemberMethod2.Name = expressionBuilderMethodName;
						codeMemberMethod2.Attributes &= (MemberAttributes)(-61441);
						codeMemberMethod2.Attributes |= MemberAttributes.Public;
						if (this._designerMode)
						{
							base.ApplyEditorBrowsableCustomAttribute(codeMemberMethod2);
						}
						Type eventHandlerType = @event.EventHandlerType;
						MethodInfo method = eventHandlerType.GetMethod("Invoke");
						ParameterInfo[] parameters = method.GetParameters();
						foreach (ParameterInfo parameterInfo in parameters)
						{
							codeMemberMethod2.Parameters.Add(new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name));
						}
						codeStatementCollection5 = codeStatementCollection4;
						DataBindingExpressionBuilder.BuildExpressionSetup(builder, codeStatementCollection3, codeStatementCollection4);
						this._sourceDataClass.Members.Add(codeMemberMethod2);
					}
					CodeStatementCollection codeStatementCollection6 = codeStatementCollection5;
					this.HandleDeviceFilterConditional(ref propertyEntry, boundPropertyEntry, codeStatementCollection4, ref codeStatementCollection6, out codeStatementCollection5);
					if (boundPropertyEntry.TwoWayBound)
					{
						DataBindingExpressionBuilder.BuildEvalExpression(boundPropertyEntry.FieldName, boundPropertyEntry.FormatString, boundPropertyEntry.Name, boundPropertyEntry.Type, builder, codeStatementCollection3, codeStatementCollection6, codeLinePragma2, ref flag4);
					}
					else
					{
						DataBindingExpressionBuilder.BuildExpressionStatic(boundPropertyEntry, builder, null, codeStatementCollection3, codeStatementCollection6, codeLinePragma2, ref flag4);
					}
				}
			}
			if (codeStatementCollection3 != null)
			{
				foreach (object obj5 in codeStatementCollection3)
				{
					CodeStatement codeStatement3 = (CodeStatement)obj5;
					codeMemberMethod2.Statements.Add(codeStatement3);
				}
			}
			if (codeStatementCollection4 != null)
			{
				foreach (object obj6 in codeStatementCollection4)
				{
					CodeStatement codeStatement4 = (CodeStatement)obj6;
					codeMemberMethod2.Statements.Add(codeStatement4);
				}
			}
			return codeMemberMethod2;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003BBB8 File Offset: 0x0003ABB8
		internal void BuildRenderMethod(ControlBuilder builder, bool fTemplate)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Attributes = (MemberAttributes)20482;
			codeMemberMethod.Name = "__Render" + builder.ID;
			if (this._designerMode)
			{
				base.ApplyEditorBrowsableCustomAttribute(codeMemberMethod);
			}
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HtmlTextWriter), "__w"));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Control), "parameterContainer"));
			this._sourceDataClass.Members.Add(codeMemberMethod);
			bool flag = false;
			if (builder.SubBuilders != null)
			{
				IEnumerator enumerator = builder.SubBuilders.GetEnumerator();
				int num = 0;
				int num2 = 0;
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					CodeLinePragma codeLinePragma = null;
					if (obj is ControlBuilder)
					{
						codeLinePragma = base.CreateCodeLinePragma((ControlBuilder)obj);
					}
					if (obj is string)
					{
						if (!this._designerMode)
						{
							this.AddOutputWriteStringStatement(codeMemberMethod.Statements, (string)obj);
						}
					}
					else if (obj is CodeBlockBuilder)
					{
						CodeBlockBuilder codeBlockBuilder = (CodeBlockBuilder)obj;
						if (codeBlockBuilder.BlockType == CodeBlockType.Expression)
						{
							string content = codeBlockBuilder.Content;
							if (this._designerMode)
							{
								if (!flag)
								{
									flag = true;
									codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(typeof(object), "__o"));
								}
								CodeStatement codeStatement = new CodeAssignStatement(new CodeVariableReferenceExpression("__o"), new CodeSnippetExpression(content));
								codeStatement.LinePragma = codeLinePragma;
								codeMemberMethod.Statements.Add(codeStatement);
							}
							else
							{
								CodeStatement outputWriteStatement = this.GetOutputWriteStatement(new CodeSnippetExpression(content));
								TextWriter textWriter = new StringWriter(CultureInfo.InvariantCulture);
								this._codeDomProvider.GenerateCodeFromStatement(outputWriteStatement, textWriter, null);
								string text = textWriter.ToString();
								text = text.PadLeft(codeBlockBuilder.Column + content.Length + 3);
								CodeSnippetStatement codeSnippetStatement = new CodeSnippetStatement(text);
								codeSnippetStatement.LinePragma = codeLinePragma;
								codeMemberMethod.Statements.Add(codeSnippetStatement);
							}
						}
						else
						{
							string text2 = codeBlockBuilder.Content;
							text2 = text2.PadLeft(text2.Length + codeBlockBuilder.Column - 1);
							CodeSnippetStatement codeSnippetStatement2 = new CodeSnippetStatement(text2);
							codeSnippetStatement2.LinePragma = codeLinePragma;
							codeMemberMethod.Statements.Add(codeSnippetStatement2);
						}
					}
					else if (obj is ControlBuilder && !this._designerMode)
					{
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
						CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
						codeMethodInvokeExpression.Method.TargetObject = new CodeIndexerExpression(new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("parameterContainer"), "Controls"), new CodeExpression[]
						{
							new CodePrimitiveExpression(num++)
						});
						codeMethodInvokeExpression.Method.MethodName = "RenderControl";
						codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("__w"));
						codeMemberMethod.Statements.Add(codeExpressionStatement);
					}
					num2++;
				}
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0003BEB8 File Offset: 0x0003AEB8
		protected virtual void BuildSourceDataTreeFromBuilder(ControlBuilder builder, bool fInTemplate, bool topLevelControlInTemplate, PropertyEntry pse)
		{
			if (builder is CodeBlockBuilder)
			{
				return;
			}
			bool flag = builder is TemplateBuilder;
			if (builder.ID == null || fInTemplate)
			{
				this._controlCount++;
				builder.ID = "__control" + this._controlCount.ToString(NumberFormatInfo.InvariantInfo);
				builder.IsGeneratedID = true;
			}
			if (builder.SubBuilders != null)
			{
				foreach (object obj in builder.SubBuilders)
				{
					if (obj is ControlBuilder)
					{
						bool flag2 = flag && typeof(Control).IsAssignableFrom(((ControlBuilder)obj).ControlType) && !(builder is RootBuilder);
						this.BuildSourceDataTreeFromBuilder((ControlBuilder)obj, fInTemplate, flag2, null);
					}
				}
			}
			foreach (object obj2 in builder.TemplatePropertyEntries)
			{
				TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj2;
				bool flag3 = true;
				if (templatePropertyEntry.PropertyInfo != null)
				{
					TemplateInstanceAttribute templateInstanceAttribute = (TemplateInstanceAttribute)Attribute.GetCustomAttribute(templatePropertyEntry.PropertyInfo, typeof(TemplateInstanceAttribute), false);
					if (templateInstanceAttribute != null)
					{
						flag3 = templateInstanceAttribute.Instances == TemplateInstance.Multiple;
					}
				}
				this.BuildSourceDataTreeFromBuilder(templatePropertyEntry.Builder, flag3, false, templatePropertyEntry);
			}
			foreach (object obj3 in builder.ComplexPropertyEntries)
			{
				ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj3;
				if (!(complexPropertyEntry.Builder is StringPropertyBuilder))
				{
					this.BuildSourceDataTreeFromBuilder(complexPropertyEntry.Builder, fInTemplate, false, complexPropertyEntry);
				}
			}
			if (!builder.IsGeneratedID)
			{
				this.BuildFieldDeclaration(builder);
			}
			CodeMemberMethod codeMemberMethod = null;
			CodeMemberMethod codeMemberMethod2 = null;
			if (this._sourceDataClass != null)
			{
				if (!this._designerMode)
				{
					codeMemberMethod = this.BuildBuildMethod(builder, flag, fInTemplate, topLevelControlInTemplate, pse, false);
				}
				if (builder.HasAspCode)
				{
					this.BuildRenderMethod(builder, flag);
				}
				this.BuildExtractMethod(builder);
				codeMemberMethod2 = this.BuildPropertyBindingMethod(builder, false);
			}
			builder.ProcessGeneratedCode(this._codeCompileUnit, this._intermediateClass, this._sourceDataClass, codeMemberMethod, codeMemberMethod2);
			if (this.Parser.ControlBuilderInterceptor != null)
			{
				this.Parser.ControlBuilderInterceptor.OnProcessGeneratedCode(builder, this._codeCompileUnit, this._intermediateClass, this._sourceDataClass, codeMemberMethod, codeMemberMethod2, builder.AdditionalState);
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003C150 File Offset: 0x0003B150
		internal virtual CodeExpression BuildStringPropertyExpression(PropertyEntry pse)
		{
			string text = string.Empty;
			if (pse is SimplePropertyEntry)
			{
				text = (string)((SimplePropertyEntry)pse).Value;
			}
			else
			{
				ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)pse;
				text = (string)((StringPropertyBuilder)complexPropertyEntry.Builder).BuildObject();
			}
			return CodeDomUtility.GenerateExpressionForValue(pse.PropertyInfo, text, typeof(string));
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0003C1B4 File Offset: 0x0003B1B4
		protected virtual CodeAssignStatement BuildTemplatePropertyStatement(CodeExpression ctrlRefExpr)
		{
			return new CodeAssignStatement
			{
				Left = new CodePropertyReferenceExpression(ctrlRefExpr, "TemplateControl"),
				Right = new CodeThisReferenceExpression()
			};
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0003C1E4 File Offset: 0x0003B1E4
		private string ExtractMethodName(ControlBuilder builder)
		{
			return BaseTemplateCodeDomTreeGenerator.extractTemplateValuesMethodPrefix + builder.ID;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003C1F6 File Offset: 0x0003B1F6
		private Type GetCtrlTypeForBuilder(ControlBuilder builder, bool fTemplate)
		{
			if (builder is RootBuilder && builder.ControlType != null)
			{
				return builder.ControlType;
			}
			if (fTemplate)
			{
				return typeof(Control);
			}
			return builder.ControlType;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0003C223 File Offset: 0x0003B223
		protected string GetMethodNameForBuilder(string prefix, ControlBuilder builder)
		{
			if (builder is RootBuilder)
			{
				return prefix + "Tree";
			}
			return prefix + builder.ID;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0003C248 File Offset: 0x0003B248
		private void HandleDeviceFilterConditional(ref PropertyEntry previous, PropertyEntry current, CodeStatementCollection topStmts, ref CodeStatementCollection currentStmts, out CodeStatementCollection nextStmts)
		{
			bool flag = previous != null && StringUtil.EqualsIgnoreCase(previous.Name, current.Name);
			if (current.Filter.Length != 0)
			{
				if (!flag)
				{
					currentStmts = topStmts;
					previous = null;
				}
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "TestDeviceFilter", new CodeExpression[0])
				{
					Parameters = 
					{
						new CodePrimitiveExpression(current.Filter)
					}
				};
				currentStmts.Add(codeConditionStatement);
				currentStmts = codeConditionStatement.TrueStatements;
				nextStmts = codeConditionStatement.FalseStatements;
				previous = current;
				return;
			}
			if (!flag)
			{
				currentStmts = topStmts;
			}
			nextStmts = topStmts;
			previous = null;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0003C2EE File Offset: 0x0003B2EE
		protected virtual bool UseResourceLiteralString(string s)
		{
			return s.Length >= 256 && this._codeDomProvider.Supports(GeneratorSupport.Win32Resources);
		}

		// Token: 0x04001523 RID: 5411
		internal const string skinIDPropertyName = "SkinID";

		// Token: 0x04001524 RID: 5412
		private const string _localVariableRef = "__ctrl";

		// Token: 0x04001525 RID: 5413
		private const int minLongLiteralStringLength = 256;

		// Token: 0x04001526 RID: 5414
		private const string renderMethodParameterName = "__w";

		// Token: 0x04001527 RID: 5415
		internal const string tempObjectVariable = "__o";

		// Token: 0x04001528 RID: 5416
		protected static readonly string buildMethodPrefix = "__BuildControl";

		// Token: 0x04001529 RID: 5417
		protected static readonly string extractTemplateValuesMethodPrefix = "__ExtractValues";

		// Token: 0x0400152A RID: 5418
		protected static readonly string templateSourceDirectoryName = "AppRelativeTemplateSourceDirectory";

		// Token: 0x0400152B RID: 5419
		protected static readonly string applyStyleSheetMethodName = "ApplyStyleSheetSkin";

		// Token: 0x0400152C RID: 5420
		protected static readonly string pagePropertyName = "Page";

		// Token: 0x0400152D RID: 5421
		private TemplateParser _parser;

		// Token: 0x0400152E RID: 5422
		private int _controlCount;
	}
}

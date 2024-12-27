using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000126 RID: 294
	internal class WebCodeGenerator
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x000417FC File Offset: 0x000407FC
		private WebCodeGenerator()
		{
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00041804 File Offset: 0x00040804
		internal static CodeAttributeDeclaration GeneratedCodeAttribute
		{
			get
			{
				if (WebCodeGenerator.generatedCodeAttribute == null)
				{
					CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(GeneratedCodeAttribute).FullName);
					Assembly assembly = Assembly.GetEntryAssembly();
					if (assembly == null)
					{
						assembly = Assembly.GetExecutingAssembly();
						if (assembly == null)
						{
							assembly = typeof(WebCodeGenerator).Assembly;
						}
					}
					AssemblyName name = assembly.GetName();
					codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(name.Name)));
					string productVersion = WebCodeGenerator.GetProductVersion(assembly);
					codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression((productVersion == null) ? name.Version.ToString() : productVersion)));
					WebCodeGenerator.generatedCodeAttribute = codeAttributeDeclaration;
				}
				return WebCodeGenerator.generatedCodeAttribute;
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000418B0 File Offset: 0x000408B0
		private static string GetProductVersion(Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(true);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is AssemblyInformationalVersionAttribute)
				{
					AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = (AssemblyInformationalVersionAttribute)customAttributes[i];
					return assemblyInformationalVersionAttribute.InformationalVersion;
				}
			}
			return null;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x000418F0 File Offset: 0x000408F0
		internal static string[] GetNamespacesForTypes(Type[] types)
		{
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < types.Length; i++)
			{
				string fullName = types[i].FullName;
				int num = fullName.LastIndexOf('.');
				if (num > 0)
				{
					hashtable[fullName.Substring(0, num)] = types[i];
				}
			}
			string[] array = new string[hashtable.Keys.Count];
			hashtable.Keys.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0004195C File Offset: 0x0004095C
		internal static void AddImports(CodeNamespace codeNamespace, string[] namespaces)
		{
			foreach (string text in namespaces)
			{
				codeNamespace.Imports.Add(new CodeNamespaceImport(text));
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00041990 File Offset: 0x00040990
		private static CodeMemberProperty CreatePropertyDeclaration(CodeMemberField field, string name, string typeName)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Type = new CodeTypeReference(typeName);
			codeMemberProperty.Name = name;
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement();
			codeMethodReturnStatement.Expression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			CodeExpression codeExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
			CodeExpression codeExpression2 = new CodeArgumentReferenceExpression("value");
			codeMemberProperty.SetStatements.Add(new CodeAssignStatement(codeExpression, codeExpression2));
			return codeMemberProperty;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00041A10 File Offset: 0x00040A10
		internal static CodeTypeMember AddMember(CodeTypeDeclaration codeClass, string typeName, string memberName, CodeExpression initializer, CodeAttributeDeclarationCollection metadata, CodeFlags flags, CodeGenerationOptions options)
		{
			bool flag = (options & CodeGenerationOptions.GenerateProperties) != CodeGenerationOptions.None;
			string text = (flag ? WebCodeGenerator.MakeFieldName(memberName) : memberName);
			CodeMemberField codeMemberField = new CodeMemberField(typeName, text);
			codeMemberField.InitExpression = initializer;
			CodeTypeMember codeTypeMember;
			if (flag)
			{
				codeClass.Members.Add(codeMemberField);
				codeTypeMember = WebCodeGenerator.CreatePropertyDeclaration(codeMemberField, memberName, typeName);
			}
			else
			{
				codeTypeMember = codeMemberField;
			}
			codeTypeMember.CustomAttributes = metadata;
			if ((flags & CodeFlags.IsPublic) != (CodeFlags)0)
			{
				codeTypeMember.Attributes = (codeMemberField.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			}
			codeClass.Members.Add(codeTypeMember);
			return codeTypeMember;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00041A95 File Offset: 0x00040A95
		internal static string FullTypeName(XmlMemberMapping mapping, CodeDomProvider codeProvider)
		{
			return mapping.GenerateTypeName(codeProvider);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00041A9E File Offset: 0x00040A9E
		private static string MakeFieldName(string name)
		{
			return CodeIdentifier.MakeCamel(name) + "Field";
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00041AB0 File Offset: 0x00040AB0
		internal static CodeConstructor AddConstructor(CodeTypeDeclaration codeClass, string[] parameterTypeNames, string[] parameterNames, CodeAttributeDeclarationCollection metadata, CodeFlags flags)
		{
			CodeConstructor codeConstructor = new CodeConstructor();
			if ((flags & CodeFlags.IsPublic) != (CodeFlags)0)
			{
				codeConstructor.Attributes = (codeConstructor.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			}
			if ((flags & CodeFlags.IsAbstract) != (CodeFlags)0)
			{
				codeConstructor.Attributes |= MemberAttributes.Abstract;
			}
			codeConstructor.CustomAttributes = metadata;
			for (int i = 0; i < parameterTypeNames.Length; i++)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(parameterTypeNames[i], parameterNames[i]);
				codeConstructor.Parameters.Add(codeParameterDeclarationExpression);
			}
			codeClass.Members.Add(codeConstructor);
			return codeConstructor;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00041B34 File Offset: 0x00040B34
		internal static CodeMemberMethod AddMethod(CodeTypeDeclaration codeClass, string methodName, CodeFlags[] parameterFlags, string[] parameterTypeNames, string[] parameterNames, string returnTypeName, CodeAttributeDeclarationCollection metadata, CodeFlags flags)
		{
			return WebCodeGenerator.AddMethod(codeClass, methodName, parameterFlags, parameterTypeNames, parameterNames, new CodeAttributeDeclarationCollection[0], returnTypeName, metadata, flags);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00041B58 File Offset: 0x00040B58
		internal static CodeMemberMethod AddMethod(CodeTypeDeclaration codeClass, string methodName, CodeFlags[] parameterFlags, string[] parameterTypeNames, string[] parameterNames, CodeAttributeDeclarationCollection[] parameterAttributes, string returnTypeName, CodeAttributeDeclarationCollection metadata, CodeFlags flags)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = methodName;
			codeMemberMethod.ReturnType = new CodeTypeReference(returnTypeName);
			codeMemberMethod.CustomAttributes = metadata;
			if ((flags & CodeFlags.IsPublic) != (CodeFlags)0)
			{
				codeMemberMethod.Attributes = (codeMemberMethod.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			}
			if ((flags & CodeFlags.IsAbstract) != (CodeFlags)0)
			{
				codeMemberMethod.Attributes = (codeMemberMethod.Attributes & (MemberAttributes)(-16)) | MemberAttributes.Abstract;
			}
			if ((flags & CodeFlags.IsNew) != (CodeFlags)0)
			{
				codeMemberMethod.Attributes = (codeMemberMethod.Attributes & (MemberAttributes)(-241)) | MemberAttributes.New;
			}
			for (int i = 0; i < parameterNames.Length; i++)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(parameterTypeNames[i], parameterNames[i]);
				if ((parameterFlags[i] & CodeFlags.IsByRef) != (CodeFlags)0)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
				}
				else if ((parameterFlags[i] & CodeFlags.IsOut) != (CodeFlags)0)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.Out;
				}
				if (i < parameterAttributes.Length)
				{
					codeParameterDeclarationExpression.CustomAttributes = parameterAttributes[i];
				}
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			codeClass.Members.Add(codeMemberMethod);
			return codeMemberMethod;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00041C40 File Offset: 0x00040C40
		internal static CodeTypeDeclaration AddClass(CodeNamespace codeNamespace, string className, string baseClassName, string[] implementedInterfaceNames, CodeAttributeDeclarationCollection metadata, CodeFlags flags, bool isPartial)
		{
			CodeTypeDeclaration codeTypeDeclaration = WebCodeGenerator.CreateClass(className, baseClassName, implementedInterfaceNames, metadata, flags, isPartial);
			codeNamespace.Types.Add(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00041C6C File Offset: 0x00040C6C
		internal static CodeTypeDeclaration CreateClass(string className, string baseClassName, string[] implementedInterfaceNames, CodeAttributeDeclarationCollection metadata, CodeFlags flags, bool isPartial)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(className);
			if (baseClassName != null && baseClassName.Length > 0)
			{
				codeTypeDeclaration.BaseTypes.Add(baseClassName);
			}
			foreach (string text in implementedInterfaceNames)
			{
				codeTypeDeclaration.BaseTypes.Add(text);
			}
			codeTypeDeclaration.IsStruct = (flags & CodeFlags.IsStruct) != (CodeFlags)0;
			if ((flags & CodeFlags.IsPublic) != (CodeFlags)0)
			{
				codeTypeDeclaration.TypeAttributes |= TypeAttributes.Public;
			}
			else
			{
				codeTypeDeclaration.TypeAttributes &= ~TypeAttributes.Public;
			}
			if ((flags & CodeFlags.IsAbstract) != (CodeFlags)0)
			{
				codeTypeDeclaration.TypeAttributes |= TypeAttributes.Abstract;
			}
			else
			{
				codeTypeDeclaration.TypeAttributes &= ~TypeAttributes.Abstract;
			}
			if ((flags & CodeFlags.IsInterface) != (CodeFlags)0)
			{
				codeTypeDeclaration.IsInterface = true;
			}
			else
			{
				codeTypeDeclaration.IsPartial = isPartial;
			}
			codeTypeDeclaration.CustomAttributes = metadata;
			codeTypeDeclaration.CustomAttributes.Add(WebCodeGenerator.GeneratedCodeAttribute);
			return codeTypeDeclaration;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00041D4C File Offset: 0x00040D4C
		internal static CodeAttributeDeclarationCollection AddCustomAttribute(CodeAttributeDeclarationCollection metadata, Type type, CodeAttributeArgument[] arguments)
		{
			if (metadata == null)
			{
				metadata = new CodeAttributeDeclarationCollection();
			}
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(type.FullName, arguments);
			metadata.Add(codeAttributeDeclaration);
			return metadata;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00041D79 File Offset: 0x00040D79
		internal static CodeAttributeDeclarationCollection AddCustomAttribute(CodeAttributeDeclarationCollection metadata, Type type, CodeExpression[] arguments)
		{
			return WebCodeGenerator.AddCustomAttribute(metadata, type, arguments, new string[0], new CodeExpression[0]);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00041D90 File Offset: 0x00040D90
		internal static CodeAttributeDeclarationCollection AddCustomAttribute(CodeAttributeDeclarationCollection metadata, Type type, CodeExpression[] parameters, string[] propNames, CodeExpression[] propValues)
		{
			int num = ((parameters == null) ? 0 : parameters.Length) + ((propNames == null) ? 0 : propNames.Length);
			CodeAttributeArgument[] array = new CodeAttributeArgument[num];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = new CodeAttributeArgument(null, parameters[i]);
			}
			for (int j = 0; j < propNames.Length; j++)
			{
				array[parameters.Length + j] = new CodeAttributeArgument(propNames[j], propValues[j]);
			}
			return WebCodeGenerator.AddCustomAttribute(metadata, type, array);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00041DFC File Offset: 0x00040DFC
		internal static void AddEvent(CodeTypeMemberCollection members, string handlerType, string handlerName)
		{
			CodeMemberEvent codeMemberEvent = new CodeMemberEvent();
			codeMemberEvent.Type = new CodeTypeReference(handlerType);
			codeMemberEvent.Name = handlerName;
			codeMemberEvent.Attributes = (codeMemberEvent.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			codeMemberEvent.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			members.Add(codeMemberEvent);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00041E60 File Offset: 0x00040E60
		internal static void AddDelegate(CodeTypeDeclarationCollection codeClasses, string handlerType, string handlerArgs)
		{
			codeClasses.Add(new CodeTypeDelegate(handlerType)
			{
				CustomAttributes = { WebCodeGenerator.GeneratedCodeAttribute },
				Parameters = 
				{
					new CodeParameterDeclarationExpression(typeof(object), "sender"),
					new CodeParameterDeclarationExpression(handlerArgs, "e")
				},
				Comments = 
				{
					new CodeCommentStatement(Res.GetString("CodeRemarks"), true)
				}
			});
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00041EE0 File Offset: 0x00040EE0
		internal static void AddCallbackDeclaration(CodeTypeMemberCollection members, string callbackMember)
		{
			members.Add(new CodeMemberField
			{
				Type = new CodeTypeReference(typeof(SendOrPostCallback)),
				Name = callbackMember
			});
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00041F18 File Offset: 0x00040F18
		internal static void AddCallbackImplementation(CodeTypeDeclaration codeClass, string callbackName, string handlerName, string handlerArgs, bool methodHasOutParameters)
		{
			CodeFlags[] array = new CodeFlags[1];
			CodeMemberMethod codeMemberMethod = WebCodeGenerator.AddMethod(codeClass, callbackName, array, new string[] { typeof(object).FullName }, new string[] { "arg" }, typeof(void).FullName, null, (CodeFlags)0);
			CodeEventReferenceExpression codeEventReferenceExpression = new CodeEventReferenceExpression(new CodeThisReferenceExpression(), handlerName);
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(codeEventReferenceExpression, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			CodeStatement[] array2 = new CodeStatement[2];
			array2[0] = new CodeVariableDeclarationStatement(typeof(InvokeCompletedEventArgs), "invokeArgs", new CodeCastExpression(typeof(InvokeCompletedEventArgs), new CodeArgumentReferenceExpression("arg")));
			CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression("invokeArgs");
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			if (methodHasOutParameters)
			{
				codeObjectCreateExpression.CreateType = new CodeTypeReference(handlerArgs);
				codeObjectCreateExpression.Parameters.Add(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Results"));
			}
			else
			{
				codeObjectCreateExpression.CreateType = new CodeTypeReference(typeof(AsyncCompletedEventArgs));
			}
			codeObjectCreateExpression.Parameters.Add(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Error"));
			codeObjectCreateExpression.Parameters.Add(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Cancelled"));
			codeObjectCreateExpression.Parameters.Add(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "UserState"));
			array2[1] = new CodeExpressionStatement(new CodeDelegateInvokeExpression(new CodeEventReferenceExpression(new CodeThisReferenceExpression(), handlerName), new CodeExpression[]
			{
				new CodeThisReferenceExpression(),
				codeObjectCreateExpression
			}));
			codeMemberMethod.Statements.Add(new CodeConditionStatement(codeBinaryOperatorExpression, array2, new CodeStatement[0]));
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000420B4 File Offset: 0x000410B4
		internal static CodeMemberMethod AddAsyncMethod(CodeTypeDeclaration codeClass, string methodName, string[] parameterTypeNames, string[] parameterNames, string callbackMember, string callbackName, string userState)
		{
			CodeMemberMethod codeMemberMethod = WebCodeGenerator.AddMethod(codeClass, methodName, new CodeFlags[parameterNames.Length], parameterTypeNames, parameterNames, typeof(void).FullName, null, CodeFlags.IsPublic);
			codeMemberMethod.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodName, new CodeExpression[0]);
			for (int i = 0; i < parameterNames.Length; i++)
			{
				codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression(parameterNames[i]));
			}
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(null));
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
			codeMemberMethod = WebCodeGenerator.AddMethod(codeClass, methodName, new CodeFlags[parameterNames.Length], parameterTypeNames, parameterNames, typeof(void).FullName, null, CodeFlags.IsPublic);
			codeMemberMethod.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), userState));
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), callbackMember);
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(codeFieldReferenceExpression, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
			CodeStatement[] array = new CodeStatement[]
			{
				new CodeAssignStatement(codeFieldReferenceExpression, new CodeDelegateCreateExpression
				{
					DelegateType = new CodeTypeReference(typeof(SendOrPostCallback)),
					TargetObject = new CodeThisReferenceExpression(),
					MethodName = callbackName
				})
			};
			codeMemberMethod.Statements.Add(new CodeConditionStatement(codeBinaryOperatorExpression, array, new CodeStatement[0]));
			return codeMemberMethod;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00042234 File Offset: 0x00041234
		internal static CodeTypeDeclaration CreateArgsClass(string name, string[] paramTypes, string[] paramNames, bool isPartial)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name);
			codeTypeDeclaration.CustomAttributes.Add(WebCodeGenerator.GeneratedCodeAttribute);
			codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DebuggerStepThroughAttribute).FullName));
			codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DesignerCategoryAttribute).FullName, new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression("code"))
			}));
			codeTypeDeclaration.IsPartial = isPartial;
			codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(typeof(AsyncCompletedEventArgs)));
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			codeIdentifiers.AddUnique("Error", "Error");
			codeIdentifiers.AddUnique("Cancelled", "Cancelled");
			codeIdentifiers.AddUnique("UserState", "UserState");
			for (int i = 0; i < paramNames.Length; i++)
			{
				if (paramNames[i] != null)
				{
					codeIdentifiers.AddUnique(paramNames[i], paramNames[i]);
				}
			}
			string text = codeIdentifiers.AddUnique("results", "results");
			CodeMemberField codeMemberField = new CodeMemberField(typeof(object[]), text);
			codeTypeDeclaration.Members.Add(codeMemberField);
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes = (codeConstructor.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Assembly;
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(object[]), text);
			codeConstructor.Parameters.Add(codeParameterDeclarationExpression);
			codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Exception), "exception"));
			codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "cancelled"));
			codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "userState"));
			codeConstructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("exception"));
			codeConstructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("cancelled"));
			codeConstructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("userState"));
			codeConstructor.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), codeMemberField.Name), new CodeArgumentReferenceExpression(text)));
			codeTypeDeclaration.Members.Add(codeConstructor);
			int num = 0;
			for (int j = 0; j < paramNames.Length; j++)
			{
				if (paramNames[j] != null)
				{
					codeTypeDeclaration.Members.Add(WebCodeGenerator.CreatePropertyDeclaration(codeMemberField, paramNames[j], paramTypes[j], num++));
				}
			}
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			return codeTypeDeclaration;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000424E0 File Offset: 0x000414E0
		private static CodeMemberProperty CreatePropertyDeclaration(CodeMemberField field, string name, string typeName, int index)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Type = new CodeTypeReference(typeName);
			codeMemberProperty.Name = name;
			codeMemberProperty.Attributes = (codeMemberProperty.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			codeMemberProperty.GetStatements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "RaiseExceptionIfNecessary", new CodeExpression[0]));
			CodeArrayIndexerExpression codeArrayIndexerExpression = new CodeArrayIndexerExpression();
			codeArrayIndexerExpression.TargetObject = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
			codeArrayIndexerExpression.Indices.Add(new CodePrimitiveExpression(index));
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement();
			codeMethodReturnStatement.Expression = new CodeCastExpression(typeName, codeArrayIndexerExpression);
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			codeMemberProperty.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			return codeMemberProperty;
		}

		// Token: 0x040005DF RID: 1503
		private static CodeAttributeDeclaration generatedCodeAttribute;
	}
}

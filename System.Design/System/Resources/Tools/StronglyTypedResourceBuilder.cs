using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Resources.Tools
{
	public static class StronglyTypedResourceBuilder
	{
		public static CodeCompileUnit Create(IDictionary resourceList, string baseName, string generatedCodeNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
		{
			return StronglyTypedResourceBuilder.Create(resourceList, baseName, generatedCodeNamespace, null, codeProvider, internalClass, out unmatchable);
		}

		public static CodeCompileUnit Create(IDictionary resourceList, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
		{
			if (resourceList == null)
			{
				throw new ArgumentNullException("resourceList");
			}
			Dictionary<string, StronglyTypedResourceBuilder.ResourceData> dictionary = new Dictionary<string, StronglyTypedResourceBuilder.ResourceData>(StringComparer.InvariantCultureIgnoreCase);
			foreach (object obj in resourceList)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				ResXDataNode resXDataNode = dictionaryEntry.Value as ResXDataNode;
				StronglyTypedResourceBuilder.ResourceData resourceData;
				if (resXDataNode != null)
				{
					string text = (string)dictionaryEntry.Key;
					if (text != resXDataNode.Name)
					{
						throw new ArgumentException(SR.GetString("MismatchedResourceName", new object[] { text, resXDataNode.Name }));
					}
					string valueTypeName = resXDataNode.GetValueTypeName(null);
					Type type = Type.GetType(valueTypeName);
					string text2 = null;
					if (type == typeof(string))
					{
						text2 = (string)resXDataNode.GetValue(null);
					}
					resourceData = new StronglyTypedResourceBuilder.ResourceData(type, text2);
				}
				else
				{
					Type type2 = ((dictionaryEntry.Value == null) ? typeof(object) : dictionaryEntry.Value.GetType());
					resourceData = new StronglyTypedResourceBuilder.ResourceData(type2, dictionaryEntry.Value as string);
				}
				dictionary.Add((string)dictionaryEntry.Key, resourceData);
			}
			return StronglyTypedResourceBuilder.InternalCreate(dictionary, baseName, generatedCodeNamespace, resourcesNamespace, codeProvider, internalClass, out unmatchable);
		}

		private static CodeCompileUnit InternalCreate(Dictionary<string, StronglyTypedResourceBuilder.ResourceData> resourceList, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (codeProvider == null)
			{
				throw new ArgumentNullException("codeProvider");
			}
			ArrayList arrayList = new ArrayList(0);
			Hashtable hashtable;
			SortedList sortedList = StronglyTypedResourceBuilder.VerifyResourceNames(resourceList, codeProvider, arrayList, out hashtable);
			string text = baseName;
			if (!codeProvider.IsValidIdentifier(text))
			{
				string text2 = StronglyTypedResourceBuilder.VerifyResourceName(text, codeProvider);
				if (text2 != null)
				{
					text = text2;
				}
			}
			if (!codeProvider.IsValidIdentifier(text))
			{
				throw new ArgumentException(SR.GetString("InvalidIdentifier", new object[] { text }));
			}
			if (!string.IsNullOrEmpty(generatedCodeNamespace) && !codeProvider.IsValidIdentifier(generatedCodeNamespace))
			{
				string text3 = StronglyTypedResourceBuilder.VerifyResourceName(generatedCodeNamespace, codeProvider, true);
				if (text3 != null)
				{
					generatedCodeNamespace = text3;
				}
			}
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			codeCompileUnit.ReferencedAssemblies.Add("System.dll");
			codeCompileUnit.UserData.Add("AllowLateBound", false);
			codeCompileUnit.UserData.Add("RequireVariableDeclaration", true);
			CodeNamespace codeNamespace = new CodeNamespace(generatedCodeNamespace);
			codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
			codeCompileUnit.Namespaces.Add(codeNamespace);
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(text);
			codeNamespace.Types.Add(codeTypeDeclaration);
			StronglyTypedResourceBuilder.AddGeneratedCodeAttributeforMember(codeTypeDeclaration);
			TypeAttributes typeAttributes = (internalClass ? TypeAttributes.NotPublic : TypeAttributes.Public);
			codeTypeDeclaration.TypeAttributes = typeAttributes;
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement("<summary>", true));
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(SR.GetString("ClassDocComment"), true));
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement("</summary>", true));
			CodeTypeReference codeTypeReference = new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute));
			codeTypeReference.Options = CodeTypeReferenceOptions.GlobalReference;
			codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(codeTypeReference));
			CodeTypeReference codeTypeReference2 = new CodeTypeReference(typeof(CompilerGeneratedAttribute));
			codeTypeReference2.Options = CodeTypeReferenceOptions.GlobalReference;
			codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(codeTypeReference2));
			bool flag = internalClass || codeProvider.Supports(GeneratorSupport.PublicStaticMembers);
			bool flag2 = codeProvider.Supports(GeneratorSupport.TryCatchStatements);
			StronglyTypedResourceBuilder.EmitBasicClassMembers(codeTypeDeclaration, generatedCodeNamespace, baseName, resourcesNamespace, internalClass, flag, flag2);
			foreach (object obj in sortedList)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = (string)dictionaryEntry.Key;
				string text5 = (string)hashtable[text4];
				if (text5 == null)
				{
					text5 = text4;
				}
				if (!StronglyTypedResourceBuilder.DefineResourceFetchingProperty(text4, text5, (StronglyTypedResourceBuilder.ResourceData)dictionaryEntry.Value, codeTypeDeclaration, internalClass, flag))
				{
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			unmatchable = (string[])arrayList.ToArray(typeof(string));
			CodeGenerator.ValidateIdentifiers(codeCompileUnit);
			return codeCompileUnit;
		}

		public static CodeCompileUnit Create(string resxFile, string baseName, string generatedCodeNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
		{
			return StronglyTypedResourceBuilder.Create(resxFile, baseName, generatedCodeNamespace, null, codeProvider, internalClass, out unmatchable);
		}

		public static CodeCompileUnit Create(string resxFile, string baseName, string generatedCodeNamespace, string resourcesNamespace, CodeDomProvider codeProvider, bool internalClass, out string[] unmatchable)
		{
			if (resxFile == null)
			{
				throw new ArgumentNullException("resxFile");
			}
			Dictionary<string, StronglyTypedResourceBuilder.ResourceData> dictionary = new Dictionary<string, StronglyTypedResourceBuilder.ResourceData>(StringComparer.InvariantCultureIgnoreCase);
			using (ResXResourceReader resXResourceReader = new ResXResourceReader(resxFile))
			{
				resXResourceReader.UseResXDataNodes = true;
				foreach (object obj in resXResourceReader)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ResXDataNode resXDataNode = (ResXDataNode)dictionaryEntry.Value;
					string valueTypeName = resXDataNode.GetValueTypeName(null);
					Type type = Type.GetType(valueTypeName);
					string text = null;
					if (type == typeof(string))
					{
						text = (string)resXDataNode.GetValue(null);
					}
					StronglyTypedResourceBuilder.ResourceData resourceData = new StronglyTypedResourceBuilder.ResourceData(type, text);
					dictionary.Add((string)dictionaryEntry.Key, resourceData);
				}
			}
			return StronglyTypedResourceBuilder.InternalCreate(dictionary, baseName, generatedCodeNamespace, resourcesNamespace, codeProvider, internalClass, out unmatchable);
		}

		private static void AddGeneratedCodeAttributeforMember(CodeTypeMember typeMember)
		{
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedCodeAttribute)));
			codeAttributeDeclaration.AttributeType.Options = CodeTypeReferenceOptions.GlobalReference;
			CodeAttributeArgument codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(typeof(StronglyTypedResourceBuilder).FullName));
			CodeAttributeArgument codeAttributeArgument2 = new CodeAttributeArgument(new CodePrimitiveExpression("2.0.0.0"));
			codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
			codeAttributeDeclaration.Arguments.Add(codeAttributeArgument2);
			typeMember.CustomAttributes.Add(codeAttributeDeclaration);
		}

		private static void EmitBasicClassMembers(CodeTypeDeclaration srClass, string nameSpace, string baseName, string resourcesNamespace, bool internalClass, bool useStatic, bool supportsTryCatch)
		{
			string text;
			if (resourcesNamespace != null)
			{
				if (resourcesNamespace.Length > 0)
				{
					text = resourcesNamespace + '.' + baseName;
				}
				else
				{
					text = baseName;
				}
			}
			else if (nameSpace != null && nameSpace.Length > 0)
			{
				text = nameSpace + '.' + baseName;
			}
			else
			{
				text = baseName;
			}
			CodeCommentStatement codeCommentStatement = new CodeCommentStatement(SR.GetString("ClassComments1"));
			srClass.Comments.Add(codeCommentStatement);
			codeCommentStatement = new CodeCommentStatement(SR.GetString("ClassComments2"));
			srClass.Comments.Add(codeCommentStatement);
			codeCommentStatement = new CodeCommentStatement(SR.GetString("ClassComments3"));
			srClass.Comments.Add(codeCommentStatement);
			codeCommentStatement = new CodeCommentStatement(SR.GetString("ClassComments4"));
			srClass.Comments.Add(codeCommentStatement);
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(SuppressMessageAttribute)));
			codeAttributeDeclaration.AttributeType.Options = CodeTypeReferenceOptions.GlobalReference;
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression("Microsoft.Performance")));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression("CA1811:AvoidUncalledPrivateCode")));
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.CustomAttributes.Add(codeAttributeDeclaration);
			if (useStatic || internalClass)
			{
				codeConstructor.Attributes = MemberAttributes.FamilyAndAssembly;
			}
			else
			{
				codeConstructor.Attributes = MemberAttributes.Public;
			}
			srClass.Members.Add(codeConstructor);
			CodeTypeReference codeTypeReference = new CodeTypeReference(typeof(ResourceManager), CodeTypeReferenceOptions.GlobalReference);
			CodeMemberField codeMemberField = new CodeMemberField(codeTypeReference, "resourceMan");
			codeMemberField.Attributes = MemberAttributes.Private;
			if (useStatic)
			{
				codeMemberField.Attributes |= MemberAttributes.Static;
			}
			srClass.Members.Add(codeMemberField);
			CodeTypeReference codeTypeReference2 = new CodeTypeReference(typeof(CultureInfo), CodeTypeReferenceOptions.GlobalReference);
			codeMemberField = new CodeMemberField(codeTypeReference2, "resourceCulture");
			codeMemberField.Attributes = MemberAttributes.Private;
			if (useStatic)
			{
				codeMemberField.Attributes |= MemberAttributes.Static;
			}
			srClass.Members.Add(codeMemberField);
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			srClass.Members.Add(codeMemberProperty);
			codeMemberProperty.Name = "ResourceManager";
			codeMemberProperty.HasGet = true;
			codeMemberProperty.HasSet = false;
			codeMemberProperty.Type = codeTypeReference;
			if (internalClass)
			{
				codeMemberProperty.Attributes = MemberAttributes.Assembly;
			}
			else
			{
				codeMemberProperty.Attributes = MemberAttributes.Public;
			}
			if (useStatic)
			{
				codeMemberProperty.Attributes |= MemberAttributes.Static;
			}
			CodeAttributeArgument codeAttributeArgument = new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(new CodeTypeReference(typeof(EditorBrowsableState))
			{
				Options = CodeTypeReferenceOptions.GlobalReference
			}), "Advanced"));
			CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration("System.ComponentModel.EditorBrowsableAttribute", new CodeAttributeArgument[] { codeAttributeArgument });
			codeAttributeDeclaration2.AttributeType.Options = CodeTypeReferenceOptions.GlobalReference;
			codeMemberProperty.CustomAttributes.Add(codeAttributeDeclaration2);
			CodeMemberProperty codeMemberProperty2 = new CodeMemberProperty();
			srClass.Members.Add(codeMemberProperty2);
			codeMemberProperty2.Name = "Culture";
			codeMemberProperty2.HasGet = true;
			codeMemberProperty2.HasSet = true;
			codeMemberProperty2.Type = codeTypeReference2;
			if (internalClass)
			{
				codeMemberProperty2.Attributes = MemberAttributes.Assembly;
			}
			else
			{
				codeMemberProperty2.Attributes = MemberAttributes.Public;
			}
			if (useStatic)
			{
				codeMemberProperty2.Attributes |= MemberAttributes.Static;
			}
			codeMemberProperty2.CustomAttributes.Add(codeAttributeDeclaration2);
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(null, "resourceMan");
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(object)), "ReferenceEquals");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[]
			{
				codeFieldReferenceExpression,
				new CodePrimitiveExpression(null)
			});
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeTypeOfExpression(new CodeTypeReference(srClass.Name)), "Assembly");
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(codeTypeReference, new CodeExpression[]
			{
				new CodePrimitiveExpression(text),
				codePropertyReferenceExpression
			});
			CodeStatement[] array = new CodeStatement[]
			{
				new CodeVariableDeclarationStatement(codeTypeReference, "temp", codeObjectCreateExpression),
				new CodeAssignStatement(codeFieldReferenceExpression, new CodeVariableReferenceExpression("temp"))
			};
			codeMemberProperty.GetStatements.Add(new CodeConditionStatement(codeMethodInvokeExpression, array));
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(codeFieldReferenceExpression));
			codeMemberProperty.Comments.Add(new CodeCommentStatement("<summary>", true));
			codeMemberProperty.Comments.Add(new CodeCommentStatement(SR.GetString("ResMgrPropertyComment"), true));
			codeMemberProperty.Comments.Add(new CodeCommentStatement("</summary>", true));
			CodeFieldReferenceExpression codeFieldReferenceExpression2 = new CodeFieldReferenceExpression(null, "resourceCulture");
			codeMemberProperty2.GetStatements.Add(new CodeMethodReturnStatement(codeFieldReferenceExpression2));
			CodePropertySetValueReferenceExpression codePropertySetValueReferenceExpression = new CodePropertySetValueReferenceExpression();
			codeMemberProperty2.SetStatements.Add(new CodeAssignStatement(codeFieldReferenceExpression2, codePropertySetValueReferenceExpression));
			codeMemberProperty2.Comments.Add(new CodeCommentStatement("<summary>", true));
			codeMemberProperty2.Comments.Add(new CodeCommentStatement(SR.GetString("CulturePropertyComment1"), true));
			codeMemberProperty2.Comments.Add(new CodeCommentStatement(SR.GetString("CulturePropertyComment2"), true));
			codeMemberProperty2.Comments.Add(new CodeCommentStatement("</summary>", true));
		}

		private static bool DefineResourceFetchingProperty(string propertyName, string resourceName, StronglyTypedResourceBuilder.ResourceData data, CodeTypeDeclaration srClass, bool internalClass, bool useStatic)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Name = propertyName;
			codeMemberProperty.HasGet = true;
			codeMemberProperty.HasSet = false;
			Type type = data.Type;
			if (type == null)
			{
				return false;
			}
			if (type == typeof(MemoryStream))
			{
				type = typeof(UnmanagedMemoryStream);
			}
			while (!type.IsPublic)
			{
				type = type.BaseType;
			}
			CodeTypeReference codeTypeReference = new CodeTypeReference(type);
			codeMemberProperty.Type = codeTypeReference;
			if (internalClass)
			{
				codeMemberProperty.Attributes = MemberAttributes.Assembly;
			}
			else
			{
				codeMemberProperty.Attributes = MemberAttributes.Public;
			}
			if (useStatic)
			{
				codeMemberProperty.Attributes |= MemberAttributes.Static;
			}
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(null, "ResourceManager");
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(useStatic ? null : new CodeThisReferenceExpression(), "resourceCulture");
			bool flag = type == typeof(string);
			bool flag2 = type == typeof(UnmanagedMemoryStream) || type == typeof(MemoryStream);
			string text = "GetObject";
			if (flag)
			{
				text = "GetString";
				string text2 = data.ValueIfString;
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				if (text2.Length > 512)
				{
					text2 = SR.GetString("StringPropertyTruncatedComment", new object[] { text2.Substring(0, 512) });
				}
				text2 = SecurityElement.Escape(text2);
				string text3 = string.Format(CultureInfo.CurrentCulture, SR.GetString("StringPropertyComment"), new object[] { text2 });
				codeMemberProperty.Comments.Add(new CodeCommentStatement("<summary>", true));
				codeMemberProperty.Comments.Add(new CodeCommentStatement(text3, true));
				codeMemberProperty.Comments.Add(new CodeCommentStatement("</summary>", true));
			}
			else if (flag2)
			{
				text = "GetStream";
			}
			CodeExpression codeExpression = new CodeMethodInvokeExpression(codePropertyReferenceExpression, text, new CodeExpression[]
			{
				new CodePrimitiveExpression(resourceName),
				codeFieldReferenceExpression
			});
			CodeMethodReturnStatement codeMethodReturnStatement;
			if (flag || flag2)
			{
				codeMethodReturnStatement = new CodeMethodReturnStatement(codeExpression);
			}
			else
			{
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(typeof(object), "obj", codeExpression);
				codeMemberProperty.GetStatements.Add(codeVariableDeclarationStatement);
				codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeTypeReference, new CodeVariableReferenceExpression("obj")));
			}
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			srClass.Members.Add(codeMemberProperty);
			return true;
		}

		public static string VerifyResourceName(string key, CodeDomProvider provider)
		{
			return StronglyTypedResourceBuilder.VerifyResourceName(key, provider, false);
		}

		private static string VerifyResourceName(string key, CodeDomProvider provider, bool isNameSpace)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			foreach (char c in StronglyTypedResourceBuilder.CharsToReplace)
			{
				if (!isNameSpace || (c != '.' && c != ':'))
				{
					key = key.Replace(c, '_');
				}
			}
			if (provider.IsValidIdentifier(key))
			{
				return key;
			}
			key = provider.CreateValidIdentifier(key);
			if (provider.IsValidIdentifier(key))
			{
				return key;
			}
			key = "_" + key;
			if (provider.IsValidIdentifier(key))
			{
				return key;
			}
			return null;
		}

		private static SortedList VerifyResourceNames(Dictionary<string, StronglyTypedResourceBuilder.ResourceData> resourceList, CodeDomProvider codeProvider, ArrayList errors, out Hashtable reverseFixupTable)
		{
			reverseFixupTable = new Hashtable(0, StringComparer.InvariantCultureIgnoreCase);
			SortedList sortedList = new SortedList(StringComparer.InvariantCultureIgnoreCase, resourceList.Count);
			foreach (KeyValuePair<string, StronglyTypedResourceBuilder.ResourceData> keyValuePair in resourceList)
			{
				string text = keyValuePair.Key;
				if (string.Equals(text, "ResourceManager") || string.Equals(text, "Culture") || typeof(void) == keyValuePair.Value.Type)
				{
					errors.Add(text);
				}
				else if ((text.Length <= 0 || text[0] != '$') && (text.Length <= 1 || text[0] != '>' || text[1] != '>'))
				{
					if (!codeProvider.IsValidIdentifier(text))
					{
						string text2 = StronglyTypedResourceBuilder.VerifyResourceName(text, codeProvider, false);
						if (text2 == null)
						{
							errors.Add(text);
							continue;
						}
						string text3 = (string)reverseFixupTable[text2];
						if (text3 != null)
						{
							if (!errors.Contains(text3))
							{
								errors.Add(text3);
							}
							if (sortedList.Contains(text2))
							{
								sortedList.Remove(text2);
							}
							errors.Add(text);
							continue;
						}
						reverseFixupTable[text2] = text;
						text = text2;
					}
					StronglyTypedResourceBuilder.ResourceData value = keyValuePair.Value;
					if (!sortedList.Contains(text))
					{
						sortedList.Add(text, value);
					}
					else
					{
						string text4 = (string)reverseFixupTable[text];
						if (text4 != null)
						{
							if (!errors.Contains(text4))
							{
								errors.Add(text4);
							}
							reverseFixupTable.Remove(text);
						}
						errors.Add(keyValuePair.Key);
						sortedList.Remove(text);
					}
				}
			}
			return sortedList;
		}

		private const string ResMgrFieldName = "resourceMan";

		private const string ResMgrPropertyName = "ResourceManager";

		private const string CultureInfoFieldName = "resourceCulture";

		private const string CultureInfoPropertyName = "Culture";

		private const char ReplacementChar = '_';

		private const string DocCommentSummaryStart = "<summary>";

		private const string DocCommentSummaryEnd = "</summary>";

		private const int DocCommentLengthThreshold = 512;

		private static readonly char[] CharsToReplace = new char[]
		{
			' ', '\u00a0', '.', ',', ';', '|', '~', '@', '#', '%',
			'^', '&', '*', '+', '-', '/', '\\', '<', '>', '?',
			'[', ']', '(', ')', '{', '}', '"', '\'', ':', '!'
		};

		internal sealed class ResourceData
		{
			internal ResourceData(Type type, string valueIfItWasAString)
			{
				this._type = type;
				this._valueIfString = valueIfItWasAString;
			}

			internal Type Type
			{
				get
				{
					return this._type;
				}
			}

			internal string ValueIfString
			{
				get
				{
					return this._valueIfString;
				}
			}

			private Type _type;

			private string _valueIfString;
		}
	}
}

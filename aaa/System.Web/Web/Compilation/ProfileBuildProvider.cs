using System;
using System.CodeDom;
using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Profile;

namespace System.Web.Compilation
{
	// Token: 0x0200018A RID: 394
	internal class ProfileBuildProvider : BuildProvider
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x0004BD63 File Offset: 0x0004AD63
		private ProfileBuildProvider()
		{
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0004BD6C File Offset: 0x0004AD6C
		internal static ProfileBuildProvider Create()
		{
			ProfileBuildProvider profileBuildProvider = new ProfileBuildProvider();
			profileBuildProvider.SetVirtualPath(HttpRuntime.AppDomainAppVirtualPathObject.SimpleCombine("Profile"));
			return profileBuildProvider;
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x0004BD95 File Offset: 0x0004AD95
		internal static bool HasCompilableProfile
		{
			get
			{
				return ProfileManager.Enabled && (ProfileBase.GetPropertiesForCompilation().Count != 0 || ProfileBase.InheritsFromCustomType);
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0004BDB8 File Offset: 0x0004ADB8
		internal static Type GetProfileTypeFromAssembly(Assembly assembly, bool isPrecompiledApp)
		{
			if (!ProfileBuildProvider.HasCompilableProfile)
			{
				return null;
			}
			Type type = assembly.GetType("ProfileCommon");
			if (type == null && isPrecompiledApp)
			{
				throw new HttpException(SR.GetString("Profile_not_precomped"));
			}
			return type;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0004BDF4 File Offset: 0x0004ADF4
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			Hashtable propertiesForCompilation = ProfileBase.GetPropertiesForCompilation();
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			Hashtable hashtable = new Hashtable();
			Type type = Type.GetType(ProfileBase.InheritsFromTypeString, false);
			CodeNamespace codeNamespace = new CodeNamespace();
			codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web.Profile"));
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Name = "ProfileCommon";
			if (type != null)
			{
				codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(type));
				assemblyBuilder.AddAssemblyReference(type.Assembly, codeCompileUnit);
			}
			else
			{
				codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(ProfileBase.InheritsFromTypeString));
				ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
				if (profile != null)
				{
					PropertyInformation propertyInformation = profile.ElementInformation.Properties["inherits"];
					if (propertyInformation != null && propertyInformation.Source != null && propertyInformation.LineNumber > 0)
					{
						codeTypeDeclaration.LinePragma = new CodeLinePragma(HttpRuntime.GetSafePath(propertyInformation.Source), propertyInformation.LineNumber);
					}
				}
			}
			assemblyBuilder.GenerateTypeFactory("ProfileCommon");
			foreach (object obj in propertiesForCompilation)
			{
				ProfileNameTypeStruct profileNameTypeStruct = (ProfileNameTypeStruct)((DictionaryEntry)obj).Value;
				if (profileNameTypeStruct.PropertyType != null)
				{
					assemblyBuilder.AddAssemblyReference(profileNameTypeStruct.PropertyType.Assembly, codeCompileUnit);
				}
				int num = profileNameTypeStruct.Name.IndexOf('.');
				if (num < 0)
				{
					this.CreateCodeForProperty(assemblyBuilder, codeTypeDeclaration, profileNameTypeStruct);
				}
				else
				{
					string text = profileNameTypeStruct.Name.Substring(0, num);
					if (!assemblyBuilder.CodeDomProvider.IsValidIdentifier(text))
					{
						throw new ConfigurationErrorsException(SR.GetString("Profile_bad_group", new object[] { text }), profileNameTypeStruct.FileName, profileNameTypeStruct.LineNumber);
					}
					if (hashtable[text] == null)
					{
						hashtable.Add(text, profileNameTypeStruct.Name);
					}
					else
					{
						hashtable[text] = (string)hashtable[text] + ";" + profileNameTypeStruct.Name;
					}
				}
			}
			foreach (object obj2 in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				this.AddPropertyGroup(assemblyBuilder, (string)dictionaryEntry.Key, (string)dictionaryEntry.Value, propertiesForCompilation, codeTypeDeclaration, codeNamespace);
			}
			this.AddCodeForGetProfileForUser(codeTypeDeclaration);
			codeNamespace.Types.Add(codeTypeDeclaration);
			codeCompileUnit.Namespaces.Add(codeNamespace);
			assemblyBuilder.AddCodeCompileUnit(this, codeCompileUnit);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0004C0E4 File Offset: 0x0004B0E4
		private void CreateCodeForProperty(AssemblyBuilder assemblyBuilder, CodeTypeDeclaration type, ProfileNameTypeStruct property)
		{
			string text = property.Name;
			int num = text.IndexOf('.');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			if (!assemblyBuilder.CodeDomProvider.IsValidIdentifier(text))
			{
				throw new ConfigurationErrorsException(SR.GetString("Profile_bad_name"), property.FileName, property.LineNumber);
			}
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Name = text;
			codeMemberProperty.Attributes = MemberAttributes.Public;
			codeMemberProperty.HasGet = true;
			codeMemberProperty.Type = property.PropertyCodeRefType;
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetPropertyValue";
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(text));
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeMemberProperty.Type, codeMethodInvokeExpression));
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			if (!property.IsReadOnly)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression2.Method.TargetObject = new CodeThisReferenceExpression();
				codeMethodInvokeExpression2.Method.MethodName = "SetPropertyValue";
				codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(text));
				codeMethodInvokeExpression2.Parameters.Add(new CodePropertySetValueReferenceExpression());
				codeMemberProperty.HasSet = true;
				codeMemberProperty.SetStatements.Add(codeMethodInvokeExpression2);
			}
			type.Members.Add(codeMemberProperty);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0004C238 File Offset: 0x0004B238
		private void AddPropertyGroup(AssemblyBuilder assemblyBuilder, string groupName, string propertyNames, Hashtable properties, CodeTypeDeclaration type, CodeNamespace ns)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Name = groupName;
			codeMemberProperty.Attributes = MemberAttributes.Public;
			codeMemberProperty.HasGet = true;
			codeMemberProperty.Type = new CodeTypeReference("ProfileGroup" + groupName);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetProfileGroup";
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(codeMemberProperty.Name));
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeMemberProperty.Type, codeMethodInvokeExpression));
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			type.Members.Add(codeMemberProperty);
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Name = "ProfileGroup" + groupName;
			codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(typeof(ProfileGroupBase)));
			string[] array = propertyNames.Split(new char[] { ';' });
			foreach (string text in array)
			{
				this.CreateCodeForProperty(assemblyBuilder, codeTypeDeclaration, (ProfileNameTypeStruct)properties[text]);
			}
			ns.Types.Add(codeTypeDeclaration);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0004C370 File Offset: 0x0004B370
		private void AddCodeForGetProfileForUser(CodeTypeDeclaration type)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = "GetProfile";
			codeMemberMethod.Attributes = MemberAttributes.Public;
			codeMemberMethod.ReturnType = new CodeTypeReference("ProfileCommon");
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "username"));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeTypeReferenceExpression("ProfileBase");
			codeMethodInvokeExpression.Method.MethodName = "Create";
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("username"));
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeCastExpression(codeMemberMethod.ReturnType, codeMethodInvokeExpression));
			ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
			codeMemberMethod.Statements.Add(codeMethodReturnStatement);
			type.Members.Add(codeMemberMethod);
		}

		// Token: 0x0400168A RID: 5770
		private const string ProfileTypeName = "ProfileCommon";
	}
}

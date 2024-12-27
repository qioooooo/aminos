using System;
using System.CodeDom;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000181 RID: 385
	internal class ObjectFactoryCodeDomTreeGenerator
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00049B44 File Offset: 0x00048B44
		internal ObjectFactoryCodeDomTreeGenerator(string outputAssemblyName)
		{
			this._codeCompileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace("__ASP");
			this._codeCompileUnit.Namespaces.Add(codeNamespace);
			string text = "FastObjectFactory_" + Util.MakeValidTypeNameFromString(outputAssemblyName).ToLower(CultureInfo.InvariantCulture);
			this._factoryClass = new CodeTypeDeclaration(text);
			this._factoryClass.TypeAttributes &= ~TypeAttributes.Public;
			CodeSnippetTypeMember codeSnippetTypeMember = new CodeSnippetTypeMember(string.Empty);
			codeSnippetTypeMember.LinePragma = new CodeLinePragma("c:\\\\dummy.txt", 1);
			this._factoryClass.Members.Add(codeSnippetTypeMember);
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes |= MemberAttributes.Private;
			this._factoryClass.Members.Add(codeConstructor);
			codeNamespace.Types.Add(this._factoryClass);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00049C24 File Offset: 0x00048C24
		internal void AddFactoryMethod(string typeToCreate)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = ObjectFactoryCodeDomTreeGenerator.GetCreateMethodNameForType(typeToCreate);
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(object));
			codeMemberMethod.Attributes = MemberAttributes.Static;
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeObjectCreateExpression(typeToCreate, new CodeExpression[0]));
			codeMemberMethod.Statements.Add(codeMethodReturnStatement);
			this._factoryClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00049C90 File Offset: 0x00048C90
		private static string GetCreateMethodNameForType(string typeToCreate)
		{
			return "Create_" + Util.MakeValidTypeNameFromString(typeToCreate);
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00049CA2 File Offset: 0x00048CA2
		internal CodeCompileUnit CodeCompileUnit
		{
			get
			{
				return this._codeCompileUnit;
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00049CAC File Offset: 0x00048CAC
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal static InstantiateObject GetFastObjectCreationDelegate(Type t)
		{
			Assembly assembly = t.Assembly;
			string text = Util.GetAssemblyShortName(t.Assembly);
			text = text.ToLower(CultureInfo.InvariantCulture);
			Type type = assembly.GetType("__ASP.FastObjectFactory_" + Util.MakeValidTypeNameFromString(text));
			if (type == null)
			{
				return null;
			}
			string createMethodNameForType = ObjectFactoryCodeDomTreeGenerator.GetCreateMethodNameForType(t.FullName);
			InstantiateObject instantiateObject;
			try
			{
				instantiateObject = (InstantiateObject)Delegate.CreateDelegate(typeof(InstantiateObject), type, createMethodNameForType);
			}
			catch
			{
				instantiateObject = null;
			}
			return instantiateObject;
		}

		// Token: 0x04001663 RID: 5731
		private const string factoryClassNameBase = "FastObjectFactory_";

		// Token: 0x04001664 RID: 5732
		private const string factoryFullClassNameBase = "__ASP.FastObjectFactory_";

		// Token: 0x04001665 RID: 5733
		private CodeCompileUnit _codeCompileUnit;

		// Token: 0x04001666 RID: 5734
		private CodeTypeDeclaration _factoryClass;
	}
}

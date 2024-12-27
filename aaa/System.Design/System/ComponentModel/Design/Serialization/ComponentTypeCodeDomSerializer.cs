using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000159 RID: 345
	internal class ComponentTypeCodeDomSerializer : TypeCodeDomSerializer
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00033B44 File Offset: 0x00032B44
		internal new static ComponentTypeCodeDomSerializer Default
		{
			get
			{
				if (ComponentTypeCodeDomSerializer._default == null)
				{
					ComponentTypeCodeDomSerializer._default = new ComponentTypeCodeDomSerializer();
				}
				return ComponentTypeCodeDomSerializer._default;
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00033B5C File Offset: 0x00032B5C
		protected override CodeMemberMethod GetInitializeMethod(IDesignerSerializationManager manager, CodeTypeDeclaration typeDecl, object value)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (typeDecl == null)
			{
				throw new ArgumentNullException("typeDecl");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CodeMemberMethod codeMemberMethod = typeDecl.UserData[ComponentTypeCodeDomSerializer._initMethodKey] as CodeMemberMethod;
			if (codeMemberMethod == null)
			{
				codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "InitializeComponent";
				codeMemberMethod.Attributes = MemberAttributes.Private;
				typeDecl.UserData[ComponentTypeCodeDomSerializer._initMethodKey] = codeMemberMethod;
				CodeConstructor codeConstructor = new CodeConstructor();
				codeConstructor.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InitializeComponent", new CodeExpression[0]));
				typeDecl.Members.Add(codeConstructor);
			}
			return codeMemberMethod;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00033C10 File Offset: 0x00032C10
		protected override CodeMemberMethod[] GetInitializeMethods(IDesignerSerializationManager manager, CodeTypeDeclaration typeDecl)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (typeDecl == null)
			{
				throw new ArgumentNullException("typeDecl");
			}
			foreach (object obj in typeDecl.Members)
			{
				CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
				CodeMemberMethod codeMemberMethod = codeTypeMember as CodeMemberMethod;
				if (codeMemberMethod != null && codeMemberMethod.Name.Equals("InitializeComponent") && codeMemberMethod.Parameters.Count == 0)
				{
					return new CodeMemberMethod[] { codeMemberMethod };
				}
			}
			return new CodeMemberMethod[0];
		}

		// Token: 0x04000EE6 RID: 3814
		private const string _initMethodName = "InitializeComponent";

		// Token: 0x04000EE7 RID: 3815
		private static object _initMethodKey = new object();

		// Token: 0x04000EE8 RID: 3816
		private static ComponentTypeCodeDomSerializer _default;
	}
}

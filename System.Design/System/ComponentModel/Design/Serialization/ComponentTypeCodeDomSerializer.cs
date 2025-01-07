using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	internal class ComponentTypeCodeDomSerializer : TypeCodeDomSerializer
	{
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

		private const string _initMethodName = "InitializeComponent";

		private static object _initMethodKey = new object();

		private static ComponentTypeCodeDomSerializer _default;
	}
}

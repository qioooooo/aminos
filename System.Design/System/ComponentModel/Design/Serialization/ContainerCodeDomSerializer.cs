using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	internal class ContainerCodeDomSerializer : CodeDomSerializer
	{
		internal new static ContainerCodeDomSerializer Default
		{
			get
			{
				if (ContainerCodeDomSerializer._defaultSerializer == null)
				{
					ContainerCodeDomSerializer._defaultSerializer = new ContainerCodeDomSerializer();
				}
				return ContainerCodeDomSerializer._defaultSerializer;
			}
		}

		protected override object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer)
		{
			if (typeof(IContainer).IsAssignableFrom(type))
			{
				object service = manager.GetService(typeof(IContainer));
				if (service != null)
				{
					manager.SetName(service, name);
					return service;
				}
			}
			return base.DeserializeInstance(manager, type, parameters, name, addToContainer);
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			CodeTypeDeclaration codeTypeDeclaration = manager.Context[typeof(CodeTypeDeclaration)] as CodeTypeDeclaration;
			RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			CodeExpression codeExpression;
			if (codeTypeDeclaration != null && rootContext != null)
			{
				CodeMemberField codeMemberField = new CodeMemberField(typeof(IContainer), "components");
				codeMemberField.Attributes = MemberAttributes.Private;
				codeTypeDeclaration.Members.Add(codeMemberField);
				codeExpression = new CodeFieldReferenceExpression(rootContext.Expression, "components");
			}
			else
			{
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(typeof(IContainer), "components");
				codeStatementCollection.Add(codeVariableDeclarationStatement);
				codeExpression = new CodeVariableReferenceExpression("components");
			}
			base.SetExpression(manager, value, codeExpression);
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(typeof(Container), new CodeExpression[0]);
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression, codeObjectCreateExpression);
			codeAssignStatement.UserData["IContainer"] = "IContainer";
			codeStatementCollection.Add(codeAssignStatement);
			return codeStatementCollection;
		}

		private const string _containerName = "components";

		private static ContainerCodeDomSerializer _defaultSerializer;
	}
}

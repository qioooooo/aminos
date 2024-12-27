using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015A RID: 346
	internal class ContainerCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00033CD8 File Offset: 0x00032CD8
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

		// Token: 0x06000D0B RID: 3339 RVA: 0x00033CF0 File Offset: 0x00032CF0
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

		// Token: 0x06000D0C RID: 3340 RVA: 0x00033D3C File Offset: 0x00032D3C
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

		// Token: 0x04000EE9 RID: 3817
		private const string _containerName = "components";

		// Token: 0x04000EEA RID: 3818
		private static ContainerCodeDomSerializer _defaultSerializer;
	}
}

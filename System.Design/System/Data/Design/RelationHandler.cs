using System;
using System.CodeDom;

namespace System.Data.Design
{
	internal sealed class RelationHandler
	{
		internal RelationHandler(TypedDataSourceCodeGenerator codeGenerator, DesignRelationCollection relations)
		{
			this.codeGenerator = codeGenerator;
			this.relations = relations;
		}

		internal DesignRelationCollection Relations
		{
			get
			{
				return this.relations;
			}
		}

		internal void AddPrivateVars(CodeTypeDeclaration dataSourceClass)
		{
			if (dataSourceClass == null)
			{
				throw new InternalException("DataSource CodeTypeDeclaration should not be null.");
			}
			if (this.relations == null)
			{
				return;
			}
			foreach (object obj in this.relations)
			{
				DesignRelation designRelation = (DesignRelation)obj;
				if (designRelation.DataRelation != null)
				{
					string generatorRelationVarName = designRelation.GeneratorRelationVarName;
					dataSourceClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(DataRelation)), generatorRelationVarName));
				}
			}
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private DesignRelationCollection relations;
	}
}

using System;
using System.CodeDom;

namespace System.Data.Design
{
	// Token: 0x020000B4 RID: 180
	internal sealed class RelationHandler
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x00014E1D File Offset: 0x00013E1D
		internal RelationHandler(TypedDataSourceCodeGenerator codeGenerator, DesignRelationCollection relations)
		{
			this.codeGenerator = codeGenerator;
			this.relations = relations;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00014E33 File Offset: 0x00013E33
		internal DesignRelationCollection Relations
		{
			get
			{
				return this.relations;
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00014E3C File Offset: 0x00013E3C
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

		// Token: 0x04000BF6 RID: 3062
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000BF7 RID: 3063
		private DesignRelationCollection relations;
	}
}

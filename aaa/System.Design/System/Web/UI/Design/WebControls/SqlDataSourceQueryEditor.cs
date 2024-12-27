using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004D8 RID: 1240
	internal sealed class SqlDataSourceQueryEditor : UITypeEditor
	{
		// Token: 0x06002C8C RID: 11404 RVA: 0x000FABC0 File Offset: 0x000F9BC0
		private bool EditQueryChangeCallback(object context)
		{
			SqlDataSource sqlDataSource = (SqlDataSource)((Pair)context).First;
			DataSourceOperation dataSourceOperation = (DataSourceOperation)((Pair)context).Second;
			IServiceProvider site = sqlDataSource.Site;
			IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
			SqlDataSourceDesigner sqlDataSourceDesigner = (SqlDataSourceDesigner)designerHost.GetDesigner(sqlDataSource);
			ParameterCollection parameterCollection = null;
			string text = string.Empty;
			SqlDataSourceCommandType sqlDataSourceCommandType = SqlDataSourceCommandType.Text;
			switch (dataSourceOperation)
			{
			case DataSourceOperation.Delete:
				parameterCollection = sqlDataSource.DeleteParameters;
				text = sqlDataSource.DeleteCommand;
				sqlDataSourceCommandType = sqlDataSource.DeleteCommandType;
				break;
			case DataSourceOperation.Insert:
				parameterCollection = sqlDataSource.InsertParameters;
				text = sqlDataSource.InsertCommand;
				sqlDataSourceCommandType = sqlDataSource.InsertCommandType;
				break;
			case DataSourceOperation.Select:
				parameterCollection = sqlDataSource.SelectParameters;
				text = sqlDataSource.SelectCommand;
				sqlDataSourceCommandType = sqlDataSource.SelectCommandType;
				break;
			case DataSourceOperation.Update:
				parameterCollection = sqlDataSource.UpdateParameters;
				text = sqlDataSource.UpdateCommand;
				sqlDataSourceCommandType = sqlDataSource.UpdateCommandType;
				break;
			}
			SqlDataSourceQueryEditorForm sqlDataSourceQueryEditorForm = new SqlDataSourceQueryEditorForm(site, sqlDataSourceDesigner, sqlDataSource.ProviderName, sqlDataSourceDesigner.ConnectionString, dataSourceOperation, sqlDataSourceCommandType, text, parameterCollection);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(site, sqlDataSourceQueryEditorForm);
			if (dialogResult == DialogResult.OK)
			{
				PropertyDescriptor propertyDescriptor = null;
				switch (dataSourceOperation)
				{
				case DataSourceOperation.Delete:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["DeleteCommand"];
					break;
				case DataSourceOperation.Insert:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["InsertCommand"];
					break;
				case DataSourceOperation.Select:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["SelectCommand"];
					break;
				case DataSourceOperation.Update:
					propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["UpdateCommand"];
					break;
				}
				if (propertyDescriptor != null)
				{
					propertyDescriptor.ResetValue(sqlDataSource);
					propertyDescriptor.SetValue(sqlDataSource, sqlDataSourceQueryEditorForm.Command);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000FAD64 File Offset: 0x000F9D64
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			/*
An exception occurred when decompiling this method (06002C8D)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Object System.Web.UI.Design.WebControls.SqlDataSourceQueryEditor::EditValue(System.ComponentModel.ITypeDescriptorContext,System.IServiceProvider,System.Object)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression..ctor(ILCode code, Object operand) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 626
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1010
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000FAD99 File Offset: 0x000F9D99
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}

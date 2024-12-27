using System;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000504 RID: 1284
	internal sealed class WizardAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002DC2 RID: 11714 RVA: 0x00103754 File Offset: 0x00102754
		public WizardAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this.Load(schemeData);
			base.Style.Width = 350;
			base.Style.Height = 200;
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x001037AD File Offset: 0x001027AD
		public override void Apply(Control control)
		{
			if (control is Wizard)
			{
				this.Apply(control as Wizard);
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x001037C4 File Offset: 0x001027C4
		private void Apply(Wizard wizard)
		{
			wizard.Font.Name = this.FontName;
			wizard.Font.Size = this.FontSize;
			wizard.BackColor = this.BackColor;
			wizard.BorderColor = this.BorderColor;
			wizard.BorderWidth = this.BorderWidth;
			wizard.BorderStyle = this.BorderStyle;
			wizard.Font.ClearDefaults();
			wizard.NavigationButtonStyle.BorderWidth = this.NavigationButtonStyleBorderWidth;
			wizard.NavigationButtonStyle.Font.Name = this.NavigationButtonStyleFontName;
			wizard.NavigationButtonStyle.Font.Size = this.NavigationButtonStyleFontSize;
			wizard.NavigationButtonStyle.BorderStyle = this.NavigationButtonStyleBorderStyle;
			wizard.NavigationButtonStyle.BorderColor = this.NavigationButtonStyleBorderColor;
			wizard.NavigationButtonStyle.ForeColor = this.NavigationButtonStyleForeColor;
			wizard.NavigationButtonStyle.BackColor = this.NavigationButtonStyleBackColor;
			wizard.NavigationButtonStyle.Font.ClearDefaults();
			wizard.StepStyle.BorderWidth = this.StepStyleBorderWidth;
			wizard.StepStyle.BorderStyle = this.StepStyleBorderStyle;
			wizard.StepStyle.BorderColor = this.StepStyleBorderColor;
			wizard.StepStyle.ForeColor = this.StepStyleForeColor;
			wizard.StepStyle.BackColor = this.StepStyleBackColor;
			wizard.StepStyle.Font.Size = this.StepStyleFontSize;
			wizard.StepStyle.Font.ClearDefaults();
			wizard.SideBarButtonStyle.Font.Underline = this.SideBarButtonStyleFontUnderline;
			wizard.SideBarButtonStyle.Font.Name = this.SideBarButtonStyleFontName;
			wizard.SideBarButtonStyle.ForeColor = this.SideBarButtonStyleForeColor;
			wizard.SideBarButtonStyle.BorderWidth = this.SideBarButtonStyleBorderWidth;
			wizard.SideBarButtonStyle.BackColor = this.SideBarButtonStyleBackColor;
			wizard.SideBarButtonStyle.Font.ClearDefaults();
			wizard.HeaderStyle.ForeColor = this.HeaderStyleForeColor;
			wizard.HeaderStyle.BorderColor = this.HeaderStyleBorderColor;
			wizard.HeaderStyle.BackColor = this.HeaderStyleBackColor;
			wizard.HeaderStyle.Font.Size = this.HeaderStyleFontSize;
			wizard.HeaderStyle.Font.Bold = this.HeaderStyleFontBold;
			wizard.HeaderStyle.BorderWidth = this.HeaderStyleBorderWidth;
			wizard.HeaderStyle.HorizontalAlign = this.HeaderStyleHorizontalAlign;
			wizard.HeaderStyle.BorderStyle = this.HeaderStyleBorderStyle;
			wizard.HeaderStyle.Font.ClearDefaults();
			wizard.SideBarStyle.BackColor = this.SideBarStyleBackColor;
			wizard.SideBarStyle.VerticalAlign = this.SideBarStyleVerticalAlign;
			wizard.SideBarStyle.Font.Size = this.SideBarStyleFontSize;
			wizard.SideBarStyle.Font.Underline = this.SideBarStyleFontUnderline;
			wizard.SideBarStyle.Font.Strikeout = this.SideBarStyleFontStrikeout;
			wizard.SideBarStyle.BorderWidth = this.SideBarStyleBorderWidth;
			wizard.SideBarStyle.Font.ClearDefaults();
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x00103AD0 File Offset: 0x00102AD0
		private bool GetBooleanProperty(string propertyTag, DataRow schemeData)
		{
			/*
An exception occurred when decompiling this method (06002DC5)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Web.UI.Design.WebControls.WizardAutoFormat::GetBooleanProperty(System.String,System.Data.DataRow)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 347
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x00103B04 File Offset: 0x00102B04
		private int GetIntProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
			}
			return 0;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x00103B3C File Offset: 0x00102B3C
		private string GetStringProperty(string propertyTag, DataRow schemeData)
		{
			object obj = schemeData[propertyTag];
			if (obj != null && !obj.Equals(DBNull.Value))
			{
				return obj.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x00103B70 File Offset: 0x00102B70
		private void Load(DataRow schemeData)
		{
			/*
An exception occurred when decompiling this method (06002DC8)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.Design.WebControls.WizardAutoFormat::Load(System.Data.DataRow)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 486
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 268
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04001F16 RID: 7958
		private string FontName;

		// Token: 0x04001F17 RID: 7959
		private FontUnit FontSize;

		// Token: 0x04001F18 RID: 7960
		private Color BackColor;

		// Token: 0x04001F19 RID: 7961
		private Color BorderColor;

		// Token: 0x04001F1A RID: 7962
		private Unit BorderWidth;

		// Token: 0x04001F1B RID: 7963
		private BorderStyle BorderStyle;

		// Token: 0x04001F1C RID: 7964
		private Unit NavigationButtonStyleBorderWidth;

		// Token: 0x04001F1D RID: 7965
		private string NavigationButtonStyleFontName;

		// Token: 0x04001F1E RID: 7966
		private FontUnit NavigationButtonStyleFontSize;

		// Token: 0x04001F1F RID: 7967
		private BorderStyle NavigationButtonStyleBorderStyle;

		// Token: 0x04001F20 RID: 7968
		private Color NavigationButtonStyleBorderColor;

		// Token: 0x04001F21 RID: 7969
		private Color NavigationButtonStyleForeColor;

		// Token: 0x04001F22 RID: 7970
		private Color NavigationButtonStyleBackColor;

		// Token: 0x04001F23 RID: 7971
		private Unit StepStyleBorderWidth;

		// Token: 0x04001F24 RID: 7972
		private BorderStyle StepStyleBorderStyle;

		// Token: 0x04001F25 RID: 7973
		private Color StepStyleBorderColor;

		// Token: 0x04001F26 RID: 7974
		private Color StepStyleForeColor;

		// Token: 0x04001F27 RID: 7975
		private Color StepStyleBackColor;

		// Token: 0x04001F28 RID: 7976
		private FontUnit StepStyleFontSize;

		// Token: 0x04001F29 RID: 7977
		private bool SideBarButtonStyleFontUnderline;

		// Token: 0x04001F2A RID: 7978
		private string SideBarButtonStyleFontName;

		// Token: 0x04001F2B RID: 7979
		private Color SideBarButtonStyleForeColor;

		// Token: 0x04001F2C RID: 7980
		private Unit SideBarButtonStyleBorderWidth;

		// Token: 0x04001F2D RID: 7981
		private Color SideBarButtonStyleBackColor;

		// Token: 0x04001F2E RID: 7982
		private Color HeaderStyleForeColor;

		// Token: 0x04001F2F RID: 7983
		private Color HeaderStyleBorderColor;

		// Token: 0x04001F30 RID: 7984
		private Color HeaderStyleBackColor;

		// Token: 0x04001F31 RID: 7985
		private FontUnit HeaderStyleFontSize;

		// Token: 0x04001F32 RID: 7986
		private bool HeaderStyleFontBold;

		// Token: 0x04001F33 RID: 7987
		private Unit HeaderStyleBorderWidth;

		// Token: 0x04001F34 RID: 7988
		private HorizontalAlign HeaderStyleHorizontalAlign;

		// Token: 0x04001F35 RID: 7989
		private BorderStyle HeaderStyleBorderStyle;

		// Token: 0x04001F36 RID: 7990
		private Color SideBarStyleBackColor;

		// Token: 0x04001F37 RID: 7991
		private VerticalAlign SideBarStyleVerticalAlign;

		// Token: 0x04001F38 RID: 7992
		private FontUnit SideBarStyleFontSize;

		// Token: 0x04001F39 RID: 7993
		private bool SideBarStyleFontUnderline;

		// Token: 0x04001F3A RID: 7994
		private bool SideBarStyleFontStrikeout;

		// Token: 0x04001F3B RID: 7995
		private Unit SideBarStyleBorderWidth;
	}
}

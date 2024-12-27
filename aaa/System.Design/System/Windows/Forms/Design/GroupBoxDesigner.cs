using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000248 RID: 584
	internal class GroupBoxDesigner : ParentControlDesigner
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x00074600 File Offset: 0x00073600
		protected override Point DefaultControlLocation
		{
			get
			{
				GroupBox groupBox = (GroupBox)this.Control;
				return new Point(groupBox.DisplayRectangle.X, groupBox.DisplayRectangle.Y);
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0007463C File Offset: 0x0007363C
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			/*
An exception occurred when decompiling this method (0600164D)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.GroupBoxDesigner::OnPaintAdornments(System.Windows.Forms.PaintEventArgs)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression.GetBranchTargets() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 703
   at ICSharpCode.Decompiler.ILAst.SimpleControlFlow.Initialize(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\SimpleControlFlow.cs:line 50
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 286
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000746E8 File Offset: 0x000736E8
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 132)
			{
				base.WndProc(ref m);
				if ((int)m.Result == -1)
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x040012E8 RID: 4840
		private InheritanceUI inheritanceUI;
	}
}

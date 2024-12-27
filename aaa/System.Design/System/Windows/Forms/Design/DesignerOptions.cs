using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200021E RID: 542
	public class DesignerOptions
	{
		// Token: 0x06001454 RID: 5204 RVA: 0x00067418 File Offset: 0x00066418
		public DesignerOptions()
		{
			/*
An exception occurred when decompiling this method (06001454)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.DesignerOptions::.ctor()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at dnlib.DotNet.Emit.MethodBodyReaderBase.ReadOneInstruction()
   at dnlib.DotNet.Emit.MethodBodyReaderBase.ReadInstructionsNumBytes(UInt32 codeSize)
   at dnlib.DotNet.Emit.MethodBodyReader.Read()
   at dnlib.DotNet.MethodDef.InitializeMethodBody()
   at ICSharpCode.Decompiler.ILAst.AutoPropertyInfo.GetGetterBackingField(MethodDef getter) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\AutoPropertyInfo.cs:line 65
   at ICSharpCode.Decompiler.ILAst.AutoPropertyInfo.Initialize(TypeDef type) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\AutoPropertyInfo.cs:line 42
   at ICSharpCode.Decompiler.ILAst.AutoPropertyProvider.GetOrCreate(TypeDef type) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\AutoPropertyProvider.cs:line 60
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.ConvertFieldAccessesToPropertyMethodCalls(ILBlock method, AutoPropertyProvider autoPropertyProvider) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 772
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 242
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00067449 File Offset: 0x00066449
		// (set) Token: 0x06001456 RID: 5206 RVA: 0x00067454 File Offset: 0x00066454
		[SRDescription("DesignerOptions_GridSizeDesc")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual Size GridSize
		{
			get
			{
				return this.gridSize;
			}
			set
			{
				if (value.Width < 2)
				{
					value.Width = 2;
				}
				if (value.Height < 2)
				{
					value.Height = 2;
				}
				if (value.Width > 200)
				{
					value.Width = 200;
				}
				if (value.Height > 200)
				{
					value.Height = 200;
				}
				this.gridSize = value;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x000674C0 File Offset: 0x000664C0
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x000674C8 File Offset: 0x000664C8
		[SRCategory("DesignerOptions_LayoutSettings")]
		[SRDescription("DesignerOptions_ShowGridDesc")]
		public virtual bool ShowGrid
		{
			get
			{
				return this.showGrid;
			}
			set
			{
				this.showGrid = value;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x000674D1 File Offset: 0x000664D1
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x000674D9 File Offset: 0x000664D9
		[SRCategory("DesignerOptions_LayoutSettings")]
		[SRDescription("DesignerOptions_SnapToGridDesc")]
		public virtual bool SnapToGrid
		{
			get
			{
				return this.snapToGrid;
			}
			set
			{
				this.snapToGrid = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x000674E2 File Offset: 0x000664E2
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x000674EA File Offset: 0x000664EA
		[SRDescription("DesignerOptions_UseSnapLines")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual bool UseSnapLines
		{
			get
			{
				return this.useSnapLines;
			}
			set
			{
				this.useSnapLines = value;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x000674F3 File Offset: 0x000664F3
		// (set) Token: 0x0600145E RID: 5214 RVA: 0x000674FB File Offset: 0x000664FB
		[SRDescription("DesignerOptions_UseSmartTags")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual bool UseSmartTags
		{
			get
			{
				return this.useSmartTags;
			}
			set
			{
				this.useSmartTags = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x00067504 File Offset: 0x00066504
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x0006750C File Offset: 0x0006650C
		[SRDescription("DesignerOptions_ObjectBoundSmartTagAutoShow")]
		[SRCategory("DesignerOptions_ObjectBoundSmartTagSettings")]
		[SRDisplayName("DesignerOptions_ObjectBoundSmartTagAutoShowDisplayName")]
		public virtual bool ObjectBoundSmartTagAutoShow
		{
			get
			{
				return this.objectBoundSmartTagAutoShow;
			}
			set
			{
				this.objectBoundSmartTagAutoShow = value;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x00067515 File Offset: 0x00066515
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x0006751D File Offset: 0x0006651D
		[SRDisplayName("DesignerOptions_CodeGenDisplay")]
		[SRCategory("DesignerOptions_CodeGenSettings")]
		[SRDescription("DesignerOptions_OptimizedCodeGen")]
		public virtual bool UseOptimizedCodeGeneration
		{
			get
			{
				return this.enableComponentCache;
			}
			set
			{
				/*
An exception occurred when decompiling this method (06001462)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.DesignerOptions::set_UseOptimizedCodeGeneration(System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.NameVariables.AssignNamesToVariables(DecompilerContext context, IList`1 parameters, HashSet`1 variables, ILBlock methodBody, StringBuilder stringBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\NameVariables.cs:line 61
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 136
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x00067526 File Offset: 0x00066526
		// (set) Token: 0x06001464 RID: 5220 RVA: 0x0006752E File Offset: 0x0006652E
		[SRDisplayName("DesignerOptions_EnableInSituEditingDisplay")]
		[SRDescription("DesignerOptions_EnableInSituEditingDesc")]
		[Browsable(false)]
		[SRCategory("DesignerOptions_EnableInSituEditingCat")]
		public virtual bool EnableInSituEditing
		{
			get
			{
				return this.enableInSituEditing;
			}
			set
			{
				this.enableInSituEditing = value;
			}
		}

		// Token: 0x040011FC RID: 4604
		private const int minGridSize = 2;

		// Token: 0x040011FD RID: 4605
		private const int maxGridSize = 200;

		// Token: 0x040011FE RID: 4606
		private bool showGrid;

		// Token: 0x040011FF RID: 4607
		private bool snapToGrid;

		// Token: 0x04001200 RID: 4608
		private Size gridSize;

		// Token: 0x04001201 RID: 4609
		private bool useSnapLines;

		// Token: 0x04001202 RID: 4610
		private bool useSmartTags;

		// Token: 0x04001203 RID: 4611
		private bool objectBoundSmartTagAutoShow;

		// Token: 0x04001204 RID: 4612
		private bool enableComponentCache;

		// Token: 0x04001205 RID: 4613
		private bool enableInSituEditing;
	}
}

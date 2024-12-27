using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001DF RID: 479
	internal partial class DataGridViewCellStyleBuilder : Form
	{
		// Token: 0x06001270 RID: 4720 RVA: 0x0005CAB0 File Offset: 0x0005BAB0
		public DataGridViewCellStyleBuilder(IServiceProvider serviceProvider, IComponent comp)
		{
			this.InitializeComponent();
			this.InitializeGrids();
			this.listenerDataGridView = new DataGridView();
			this.serviceProvider = serviceProvider;
			this.comp = comp;
			if (this.serviceProvider != null)
			{
				this.helpService = (IHelpService)serviceProvider.GetService(typeof(IHelpService));
			}
			this.cellStyleProperties.Site = new DataGridViewComponentPropertyGridSite(serviceProvider, comp);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0005CB20 File Offset: 0x0005BB20
		private void InitializeGrids()
		{
			/*
An exception occurred when decompiling this method (06001271)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.DataGridViewCellStyleBuilder::InitializeGrids()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at dnlib.DotNet.AssemblyNameComparer.CompareTo(IAssembly a, IAssembly b)
   at dnSpy.Documents.DsDocumentService.FindAssembly(IAssembly assembly, FindAssemblyOptions options) in D:\a\dnSpy\dnSpy\dnSpy\dnSpy\Documents\DsDocumentService.cs:line 178
   at dnSpy.Documents.AssemblyResolver.ResolveNormal(IAssembly assembly, ModuleDef sourceModule) in D:\a\dnSpy\dnSpy\dnSpy\dnSpy\Documents\AssemblyResolver.cs:line 582
   at dnlib.DotNet.Resolver.Resolve(TypeRef typeRef, ModuleDef sourceModule)
   at dnlib.DotNet.Extensions.ToTypeSig(ITypeDefOrRef type, Boolean resolveToCheckValueType)
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 383
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 284
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 220
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.Run(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 49
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 264
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0005CCA2 File Offset: 0x0005BCA2
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0005CCAC File Offset: 0x0005BCAC
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				this.cellStyle = new DataGridViewCellStyle(value);
				this.cellStyleProperties.SelectedObject = this.cellStyle;
				this.ListenerDataGridViewDefaultCellStyleChanged(null, EventArgs.Empty);
				this.listenerDataGridView.DefaultCellStyle = this.cellStyle;
				this.listenerDataGridView.DefaultCellStyleChanged += this.ListenerDataGridViewDefaultCellStyleChanged;
			}
		}

		// Token: 0x170002ED RID: 749
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x0005CD0A File Offset: 0x0005BD0A
		public ITypeDescriptorContext Context
		{
			set
			{
				this.context = value;
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0005CD14 File Offset: 0x0005BD14
		private void ListenerDataGridViewDefaultCellStyleChanged(object sender, EventArgs e)
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle(this.cellStyle);
			this.sampleDataGridView.DefaultCellStyle = dataGridViewCellStyle;
			this.sampleDataGridViewSelected.DefaultCellStyle = dataGridViewCellStyle;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0005D595 File Offset: 0x0005C595
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) == Keys.None && (keyData & Keys.KeyCode) == Keys.Escape)
			{
				base.Close();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0005D5BA File Offset: 0x0005C5BA
		private void DataGridViewCellStyleBuilder_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.DataGridViewCellStyleBuilder_HelpRequestHandled();
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0005D5C9 File Offset: 0x0005C5C9
		private void DataGridViewCellStyleBuilder_HelpRequested(object sender, HelpEventArgs e)
		{
			e.Handled = true;
			this.DataGridViewCellStyleBuilder_HelpRequestHandled();
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0005D5D8 File Offset: 0x0005C5D8
		private void DataGridViewCellStyleBuilder_HelpRequestHandled()
		{
			IHelpService helpService = this.context.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.CellStyleDialog");
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0005D610 File Offset: 0x0005C610
		private void DataGridViewCellStyleBuilder_Load(object sender, EventArgs e)
		{
			this.sampleDataGridView.ClearSelection();
			this.sampleDataGridView.Rows[0].Height = this.sampleDataGridView.Height;
			this.sampleDataGridView.Columns[0].Width = this.sampleDataGridView.Width;
			this.sampleDataGridViewSelected.Rows[0].Height = this.sampleDataGridViewSelected.Height;
			this.sampleDataGridViewSelected.Columns[0].Width = this.sampleDataGridViewSelected.Width;
			this.sampleDataGridView.Layout += this.sampleDataGridView_Layout;
			this.sampleDataGridViewSelected.Layout += this.sampleDataGridView_Layout;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0005D6DA File Offset: 0x0005C6DA
		private void sampleDataGridView_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
		{
			if ((e.StateChanged & DataGridViewElementStates.Selected) != DataGridViewElementStates.None && (e.Cell.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None)
			{
				this.sampleDataGridView.ClearSelection();
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0005D704 File Offset: 0x0005C704
		private void sampleDataGridView_Layout(object sender, LayoutEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			dataGridView.Rows[0].Height = dataGridView.Height;
			dataGridView.Columns[0].Width = dataGridView.Width;
		}

		// Token: 0x0400113F RID: 4415
		private DataGridView listenerDataGridView;

		// Token: 0x04001149 RID: 4425
		private IHelpService helpService;

		// Token: 0x0400114A RID: 4426
		private IComponent comp;

		// Token: 0x0400114B RID: 4427
		private IServiceProvider serviceProvider;

		// Token: 0x0400114C RID: 4428
		private DataGridViewCellStyle cellStyle;

		// Token: 0x0400114D RID: 4429
		private ITypeDescriptorContext context;

		// Token: 0x020001E0 RID: 480
		private class DialogDataGridViewCell : DataGridViewTextBoxCell
		{
			// Token: 0x0600127F RID: 4735 RVA: 0x0005D746 File Offset: 0x0005C746
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (this.accObj == null)
				{
					this.accObj = new DataGridViewCellStyleBuilder.DialogDataGridViewCell.DialogDataGridViewCellAccessibleObject(this);
				}
				return this.accObj;
			}

			// Token: 0x0400114E RID: 4430
			private DataGridViewCellStyleBuilder.DialogDataGridViewCell.DialogDataGridViewCellAccessibleObject accObj;

			// Token: 0x020001E1 RID: 481
			private class DialogDataGridViewCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
			{
				// Token: 0x06001281 RID: 4737 RVA: 0x0005D76A File Offset: 0x0005C76A
				public DialogDataGridViewCellAccessibleObject(DataGridViewCell owner)
					: base(owner)
				{
				}

				// Token: 0x170002EE RID: 750
				// (get) Token: 0x06001282 RID: 4738 RVA: 0x0005D77E File Offset: 0x0005C77E
				// (set) Token: 0x06001283 RID: 4739 RVA: 0x0005D786 File Offset: 0x0005C786
				public override string Name
				{
					get
					{
						return this.name;
					}
					set
					{
						this.name = value;
					}
				}

				// Token: 0x0400114F RID: 4431
				private string name = "";
			}
		}
	}
}

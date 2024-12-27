using System;
using System.Collections;
using System.Xml.Xsl.XsltOld.Debugger;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200014B RID: 331
	internal class DbgCompiler : Compiler
	{
		// Token: 0x06000E75 RID: 3701 RVA: 0x00049B44 File Offset: 0x00048B44
		public DbgCompiler(IXsltDebugger debugger)
		{
			this.debugger = debugger;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00049B69 File Offset: 0x00048B69
		public override IXsltDebugger Debugger
		{
			get
			{
				return this.debugger;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00049B71 File Offset: 0x00048B71
		public virtual VariableAction[] GlobalVariables
		{
			get
			{
				if (this.globalVarsCache == null)
				{
					this.globalVarsCache = (VariableAction[])this.globalVars.ToArray(typeof(VariableAction));
				}
				return this.globalVarsCache;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00049BA1 File Offset: 0x00048BA1
		public virtual VariableAction[] LocalVariables
		{
			get
			{
				if (this.localVarsCache == null)
				{
					this.localVarsCache = (VariableAction[])this.localVars.ToArray(typeof(VariableAction));
				}
				return this.localVarsCache;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00049BD4 File Offset: 0x00048BD4
		private void DefineVariable(VariableAction variable)
		{
			if (variable.IsGlobal)
			{
				for (int i = 0; i < this.globalVars.Count; i++)
				{
					VariableAction variableAction = (VariableAction)this.globalVars[i];
					if (variableAction.Name == variable.Name)
					{
						if (variable.Stylesheetid < variableAction.Stylesheetid)
						{
							this.globalVars[i] = variable;
							this.globalVarsCache = null;
						}
						return;
					}
				}
				this.globalVars.Add(variable);
				this.globalVarsCache = null;
				return;
			}
			this.localVars.Add(variable);
			this.localVarsCache = null;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00049C70 File Offset: 0x00048C70
		private void UnDefineVariables(int count)
		{
			if (count != 0)
			{
				this.localVars.RemoveRange(this.localVars.Count - count, count);
				this.localVarsCache = null;
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00049C95 File Offset: 0x00048C95
		internal override void PopScope()
		{
			this.UnDefineVariables(base.ScopeManager.CurrentScope.GetVeriablesCount());
			base.PopScope();
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00049CB4 File Offset: 0x00048CB4
		public override ApplyImportsAction CreateApplyImportsAction()
		{
			ApplyImportsAction applyImportsAction = new DbgCompiler.ApplyImportsActionDbg();
			applyImportsAction.Compile(this);
			return applyImportsAction;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00049CD0 File Offset: 0x00048CD0
		public override ApplyTemplatesAction CreateApplyTemplatesAction()
		{
			ApplyTemplatesAction applyTemplatesAction = new DbgCompiler.ApplyTemplatesActionDbg();
			applyTemplatesAction.Compile(this);
			return applyTemplatesAction;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00049CEC File Offset: 0x00048CEC
		public override AttributeAction CreateAttributeAction()
		{
			AttributeAction attributeAction = new DbgCompiler.AttributeActionDbg();
			attributeAction.Compile(this);
			return attributeAction;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00049D08 File Offset: 0x00048D08
		public override AttributeSetAction CreateAttributeSetAction()
		{
			AttributeSetAction attributeSetAction = new DbgCompiler.AttributeSetActionDbg();
			attributeSetAction.Compile(this);
			return attributeSetAction;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00049D24 File Offset: 0x00048D24
		public override CallTemplateAction CreateCallTemplateAction()
		{
			CallTemplateAction callTemplateAction = new DbgCompiler.CallTemplateActionDbg();
			callTemplateAction.Compile(this);
			return callTemplateAction;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00049D40 File Offset: 0x00048D40
		public override ChooseAction CreateChooseAction()
		{
			ChooseAction chooseAction = new ChooseAction();
			chooseAction.Compile(this);
			return chooseAction;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00049D5C File Offset: 0x00048D5C
		public override CommentAction CreateCommentAction()
		{
			CommentAction commentAction = new DbgCompiler.CommentActionDbg();
			commentAction.Compile(this);
			return commentAction;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00049D78 File Offset: 0x00048D78
		public override CopyAction CreateCopyAction()
		{
			CopyAction copyAction = new DbgCompiler.CopyActionDbg();
			copyAction.Compile(this);
			return copyAction;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00049D94 File Offset: 0x00048D94
		public override CopyOfAction CreateCopyOfAction()
		{
			CopyOfAction copyOfAction = new DbgCompiler.CopyOfActionDbg();
			copyOfAction.Compile(this);
			return copyOfAction;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00049DB0 File Offset: 0x00048DB0
		public override ElementAction CreateElementAction()
		{
			ElementAction elementAction = new DbgCompiler.ElementActionDbg();
			elementAction.Compile(this);
			return elementAction;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00049DCC File Offset: 0x00048DCC
		public override ForEachAction CreateForEachAction()
		{
			ForEachAction forEachAction = new DbgCompiler.ForEachActionDbg();
			forEachAction.Compile(this);
			return forEachAction;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00049DE8 File Offset: 0x00048DE8
		public override IfAction CreateIfAction(IfAction.ConditionType type)
		{
			IfAction ifAction = new DbgCompiler.IfActionDbg(type);
			ifAction.Compile(this);
			return ifAction;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00049E04 File Offset: 0x00048E04
		public override MessageAction CreateMessageAction()
		{
			MessageAction messageAction = new DbgCompiler.MessageActionDbg();
			messageAction.Compile(this);
			return messageAction;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00049E20 File Offset: 0x00048E20
		public override NewInstructionAction CreateNewInstructionAction()
		{
			NewInstructionAction newInstructionAction = new DbgCompiler.NewInstructionActionDbg();
			newInstructionAction.Compile(this);
			return newInstructionAction;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00049E3C File Offset: 0x00048E3C
		public override NumberAction CreateNumberAction()
		{
			NumberAction numberAction = new DbgCompiler.NumberActionDbg();
			numberAction.Compile(this);
			return numberAction;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00049E58 File Offset: 0x00048E58
		public override ProcessingInstructionAction CreateProcessingInstructionAction()
		{
			ProcessingInstructionAction processingInstructionAction = new DbgCompiler.ProcessingInstructionActionDbg();
			processingInstructionAction.Compile(this);
			return processingInstructionAction;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00049E73 File Offset: 0x00048E73
		public override void CreateRootAction()
		{
			base.RootAction = new DbgCompiler.RootActionDbg();
			base.RootAction.Compile(this);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00049E8C File Offset: 0x00048E8C
		public override SortAction CreateSortAction()
		{
			SortAction sortAction = new DbgCompiler.SortActionDbg();
			sortAction.Compile(this);
			return sortAction;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00049EA8 File Offset: 0x00048EA8
		public override TemplateAction CreateTemplateAction()
		{
			TemplateAction templateAction = new DbgCompiler.TemplateActionDbg();
			templateAction.Compile(this);
			return templateAction;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00049EC4 File Offset: 0x00048EC4
		public override TemplateAction CreateSingleTemplateAction()
		{
			TemplateAction templateAction = new DbgCompiler.TemplateActionDbg();
			templateAction.CompileSingle(this);
			return templateAction;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00049EE0 File Offset: 0x00048EE0
		public override TextAction CreateTextAction()
		{
			TextAction textAction = new DbgCompiler.TextActionDbg();
			textAction.Compile(this);
			return textAction;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00049EFC File Offset: 0x00048EFC
		public override UseAttributeSetsAction CreateUseAttributeSetsAction()
		{
			UseAttributeSetsAction useAttributeSetsAction = new DbgCompiler.UseAttributeSetsActionDbg();
			useAttributeSetsAction.Compile(this);
			return useAttributeSetsAction;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00049F18 File Offset: 0x00048F18
		public override ValueOfAction CreateValueOfAction()
		{
			ValueOfAction valueOfAction = new DbgCompiler.ValueOfActionDbg();
			valueOfAction.Compile(this);
			return valueOfAction;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00049F34 File Offset: 0x00048F34
		public override VariableAction CreateVariableAction(VariableType type)
		{
			VariableAction variableAction = new DbgCompiler.VariableActionDbg(type);
			variableAction.Compile(this);
			return variableAction;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00049F50 File Offset: 0x00048F50
		public override WithParamAction CreateWithParamAction()
		{
			WithParamAction withParamAction = new DbgCompiler.WithParamActionDbg();
			withParamAction.Compile(this);
			return withParamAction;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00049F6B File Offset: 0x00048F6B
		public override BeginEvent CreateBeginEvent()
		{
			return new DbgCompiler.BeginEventDbg(this);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00049F73 File Offset: 0x00048F73
		public override TextEvent CreateTextEvent()
		{
			return new DbgCompiler.TextEventDbg(this);
		}

		// Token: 0x04000964 RID: 2404
		private IXsltDebugger debugger;

		// Token: 0x04000965 RID: 2405
		private ArrayList globalVars = new ArrayList();

		// Token: 0x04000966 RID: 2406
		private ArrayList localVars = new ArrayList();

		// Token: 0x04000967 RID: 2407
		private VariableAction[] globalVarsCache;

		// Token: 0x04000968 RID: 2408
		private VariableAction[] localVarsCache;

		// Token: 0x0200014C RID: 332
		private class ApplyImportsActionDbg : ApplyImportsAction
		{
			// Token: 0x06000E97 RID: 3735 RVA: 0x00049F7B File Offset: 0x00048F7B
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000E98 RID: 3736 RVA: 0x00049F83 File Offset: 0x00048F83
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000E99 RID: 3737 RVA: 0x00049F98 File Offset: 0x00048F98
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x04000969 RID: 2409
			private DbgData dbgData;
		}

		// Token: 0x0200014D RID: 333
		private class ApplyTemplatesActionDbg : ApplyTemplatesAction
		{
			// Token: 0x06000E9B RID: 3739 RVA: 0x00049FB8 File Offset: 0x00048FB8
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000E9C RID: 3740 RVA: 0x00049FC0 File Offset: 0x00048FC0
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000E9D RID: 3741 RVA: 0x00049FD5 File Offset: 0x00048FD5
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096A RID: 2410
			private DbgData dbgData;
		}

		// Token: 0x0200014E RID: 334
		private class AttributeActionDbg : AttributeAction
		{
			// Token: 0x06000E9F RID: 3743 RVA: 0x00049FF5 File Offset: 0x00048FF5
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EA0 RID: 3744 RVA: 0x00049FFD File Offset: 0x00048FFD
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EA1 RID: 3745 RVA: 0x0004A012 File Offset: 0x00049012
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096B RID: 2411
			private DbgData dbgData;
		}

		// Token: 0x0200014F RID: 335
		private class AttributeSetActionDbg : AttributeSetAction
		{
			// Token: 0x06000EA3 RID: 3747 RVA: 0x0004A032 File Offset: 0x00049032
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EA4 RID: 3748 RVA: 0x0004A03A File Offset: 0x0004903A
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EA5 RID: 3749 RVA: 0x0004A04F File Offset: 0x0004904F
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096C RID: 2412
			private DbgData dbgData;
		}

		// Token: 0x02000150 RID: 336
		private class CallTemplateActionDbg : CallTemplateAction
		{
			// Token: 0x06000EA7 RID: 3751 RVA: 0x0004A06F File Offset: 0x0004906F
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EA8 RID: 3752 RVA: 0x0004A077 File Offset: 0x00049077
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EA9 RID: 3753 RVA: 0x0004A08C File Offset: 0x0004908C
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096D RID: 2413
			private DbgData dbgData;
		}

		// Token: 0x02000151 RID: 337
		private class CommentActionDbg : CommentAction
		{
			// Token: 0x06000EAB RID: 3755 RVA: 0x0004A0AC File Offset: 0x000490AC
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EAC RID: 3756 RVA: 0x0004A0B4 File Offset: 0x000490B4
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EAD RID: 3757 RVA: 0x0004A0C9 File Offset: 0x000490C9
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096E RID: 2414
			private DbgData dbgData;
		}

		// Token: 0x02000152 RID: 338
		private class CopyActionDbg : CopyAction
		{
			// Token: 0x06000EAF RID: 3759 RVA: 0x0004A0E9 File Offset: 0x000490E9
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EB0 RID: 3760 RVA: 0x0004A0F1 File Offset: 0x000490F1
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EB1 RID: 3761 RVA: 0x0004A106 File Offset: 0x00049106
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400096F RID: 2415
			private DbgData dbgData;
		}

		// Token: 0x02000153 RID: 339
		private class CopyOfActionDbg : CopyOfAction
		{
			// Token: 0x06000EB3 RID: 3763 RVA: 0x0004A126 File Offset: 0x00049126
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EB4 RID: 3764 RVA: 0x0004A12E File Offset: 0x0004912E
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EB5 RID: 3765 RVA: 0x0004A143 File Offset: 0x00049143
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x04000970 RID: 2416
			private DbgData dbgData;
		}

		// Token: 0x02000155 RID: 341
		private class ElementActionDbg : ElementAction
		{
			// Token: 0x06000EBC RID: 3772 RVA: 0x0004A3FE File Offset: 0x000493FE
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EBD RID: 3773 RVA: 0x0004A406 File Offset: 0x00049406
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EBE RID: 3774 RVA: 0x0004A41B File Offset: 0x0004941B
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x04000979 RID: 2425
			private DbgData dbgData;
		}

		// Token: 0x02000157 RID: 343
		private class ForEachActionDbg : ForEachAction
		{
			// Token: 0x06000EC5 RID: 3781 RVA: 0x0004A656 File Offset: 0x00049656
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EC6 RID: 3782 RVA: 0x0004A65E File Offset: 0x0004965E
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EC7 RID: 3783 RVA: 0x0004A673 File Offset: 0x00049673
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.PushDebuggerStack();
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
				if (frame.State == -1)
				{
					processor.PopDebuggerStack();
				}
			}

			// Token: 0x04000980 RID: 2432
			private DbgData dbgData;
		}

		// Token: 0x0200015A RID: 346
		private class IfActionDbg : IfAction
		{
			// Token: 0x06000ECD RID: 3789 RVA: 0x0004A7D1 File Offset: 0x000497D1
			internal IfActionDbg(IfAction.ConditionType type)
				: base(type)
			{
			}

			// Token: 0x06000ECE RID: 3790 RVA: 0x0004A7DA File Offset: 0x000497DA
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000ECF RID: 3791 RVA: 0x0004A7E2 File Offset: 0x000497E2
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000ED0 RID: 3792 RVA: 0x0004A7F7 File Offset: 0x000497F7
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x04000987 RID: 2439
			private DbgData dbgData;
		}

		// Token: 0x0200015C RID: 348
		private class MessageActionDbg : MessageAction
		{
			// Token: 0x06000ED5 RID: 3797 RVA: 0x0004A919 File Offset: 0x00049919
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000ED6 RID: 3798 RVA: 0x0004A921 File Offset: 0x00049921
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000ED7 RID: 3799 RVA: 0x0004A936 File Offset: 0x00049936
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x04000989 RID: 2441
			private DbgData dbgData;
		}

		// Token: 0x0200015E RID: 350
		private class NewInstructionActionDbg : NewInstructionAction
		{
			// Token: 0x06000EDD RID: 3805 RVA: 0x0004AA92 File Offset: 0x00049A92
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EDE RID: 3806 RVA: 0x0004AA9A File Offset: 0x00049A9A
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EDF RID: 3807 RVA: 0x0004AAAF File Offset: 0x00049AAF
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x0400098D RID: 2445
			private DbgData dbgData;
		}

		// Token: 0x02000162 RID: 354
		private class NumberActionDbg : NumberAction
		{
			// Token: 0x06000EFA RID: 3834 RVA: 0x0004B98F File Offset: 0x0004A98F
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000EFB RID: 3835 RVA: 0x0004B997 File Offset: 0x0004A997
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000EFC RID: 3836 RVA: 0x0004B9AC File Offset: 0x0004A9AC
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009B0 RID: 2480
			private DbgData dbgData;
		}

		// Token: 0x02000164 RID: 356
		private class ProcessingInstructionActionDbg : ProcessingInstructionAction
		{
			// Token: 0x06000F03 RID: 3843 RVA: 0x0004BC38 File Offset: 0x0004AC38
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F04 RID: 3844 RVA: 0x0004BC40 File Offset: 0x0004AC40
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F05 RID: 3845 RVA: 0x0004BC55 File Offset: 0x0004AC55
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009BB RID: 2491
			private DbgData dbgData;
		}

		// Token: 0x02000167 RID: 359
		private class RootActionDbg : RootAction
		{
			// Token: 0x06000F17 RID: 3863 RVA: 0x0004C242 File Offset: 0x0004B242
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F18 RID: 3864 RVA: 0x0004C24C File Offset: 0x0004B24C
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
				string builtInTemplatesUri = compiler.Debugger.GetBuiltInTemplatesUri();
				if (builtInTemplatesUri != null && builtInTemplatesUri.Length != 0)
				{
					compiler.AllowBuiltInMode = true;
					this.builtInSheet = compiler.RootAction.CompileImport(compiler, compiler.ResolveUri(builtInTemplatesUri), int.MaxValue);
					compiler.AllowBuiltInMode = false;
				}
				this.dbgData.ReplaceVariables(((DbgCompiler)compiler).GlobalVariables);
			}

			// Token: 0x06000F19 RID: 3865 RVA: 0x0004C2C5 File Offset: 0x0004B2C5
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.PushDebuggerStack();
					processor.OnInstructionExecute();
					processor.PushDebuggerStack();
				}
				base.Execute(processor, frame);
				if (frame.State == -1)
				{
					processor.PopDebuggerStack();
					processor.PopDebuggerStack();
				}
			}

			// Token: 0x040009C6 RID: 2502
			private DbgData dbgData;
		}

		// Token: 0x02000169 RID: 361
		private class SortActionDbg : SortAction
		{
			// Token: 0x06000F23 RID: 3875 RVA: 0x0004C742 File Offset: 0x0004B742
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F24 RID: 3876 RVA: 0x0004C74A File Offset: 0x0004B74A
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F25 RID: 3877 RVA: 0x0004C75F File Offset: 0x0004B75F
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009D3 RID: 2515
			private DbgData dbgData;
		}

		// Token: 0x0200016B RID: 363
		private class TemplateActionDbg : TemplateAction
		{
			// Token: 0x06000F36 RID: 3894 RVA: 0x0004CBEC File Offset: 0x0004BBEC
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F37 RID: 3895 RVA: 0x0004CBF4 File Offset: 0x0004BBF4
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F38 RID: 3896 RVA: 0x0004CC09 File Offset: 0x0004BC09
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.PushDebuggerStack();
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
				if (frame.State == -1)
				{
					processor.PopDebuggerStack();
				}
			}

			// Token: 0x040009DA RID: 2522
			private DbgData dbgData;
		}

		// Token: 0x0200016D RID: 365
		private class TextActionDbg : TextAction
		{
			// Token: 0x06000F3F RID: 3903 RVA: 0x0004CD53 File Offset: 0x0004BD53
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F40 RID: 3904 RVA: 0x0004CD5B File Offset: 0x0004BD5B
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F41 RID: 3905 RVA: 0x0004CD70 File Offset: 0x0004BD70
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009DD RID: 2525
			private DbgData dbgData;
		}

		// Token: 0x0200016F RID: 367
		private class UseAttributeSetsActionDbg : UseAttributeSetsAction
		{
			// Token: 0x06000F47 RID: 3911 RVA: 0x0004CEB5 File Offset: 0x0004BEB5
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F48 RID: 3912 RVA: 0x0004CEBD File Offset: 0x0004BEBD
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F49 RID: 3913 RVA: 0x0004CED2 File Offset: 0x0004BED2
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009E1 RID: 2529
			private DbgData dbgData;
		}

		// Token: 0x02000171 RID: 369
		private class ValueOfActionDbg : ValueOfAction
		{
			// Token: 0x06000F51 RID: 3921 RVA: 0x0004D018 File Offset: 0x0004C018
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F52 RID: 3922 RVA: 0x0004D020 File Offset: 0x0004C020
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F53 RID: 3923 RVA: 0x0004D035 File Offset: 0x0004C035
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009E6 RID: 2534
			private DbgData dbgData;
		}

		// Token: 0x02000173 RID: 371
		private class VariableActionDbg : VariableAction
		{
			// Token: 0x06000F64 RID: 3940 RVA: 0x0004D326 File Offset: 0x0004C326
			internal VariableActionDbg(VariableType type)
				: base(type)
			{
			}

			// Token: 0x06000F65 RID: 3941 RVA: 0x0004D32F File Offset: 0x0004C32F
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F66 RID: 3942 RVA: 0x0004D337 File Offset: 0x0004C337
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
				((DbgCompiler)compiler).DefineVariable(this);
			}

			// Token: 0x06000F67 RID: 3943 RVA: 0x0004D358 File Offset: 0x0004C358
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009F0 RID: 2544
			private DbgData dbgData;
		}

		// Token: 0x02000175 RID: 373
		private class WithParamActionDbg : WithParamAction
		{
			// Token: 0x06000F6B RID: 3947 RVA: 0x0004D49C File Offset: 0x0004C49C
			internal override DbgData GetDbgData(ActionFrame frame)
			{
				return this.dbgData;
			}

			// Token: 0x06000F6C RID: 3948 RVA: 0x0004D4A4 File Offset: 0x0004C4A4
			internal override void Compile(Compiler compiler)
			{
				this.dbgData = new DbgData(compiler);
				base.Compile(compiler);
			}

			// Token: 0x06000F6D RID: 3949 RVA: 0x0004D4B9 File Offset: 0x0004C4B9
			internal override void Execute(Processor processor, ActionFrame frame)
			{
				if (frame.State == 0)
				{
					processor.OnInstructionExecute();
				}
				base.Execute(processor, frame);
			}

			// Token: 0x040009F1 RID: 2545
			private DbgData dbgData;
		}

		// Token: 0x02000176 RID: 374
		private class BeginEventDbg : BeginEvent
		{
			// Token: 0x170001E4 RID: 484
			// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0004D4D9 File Offset: 0x0004C4D9
			internal override DbgData DbgData
			{
				get
				{
					return this.dbgData;
				}
			}

			// Token: 0x06000F70 RID: 3952 RVA: 0x0004D4E1 File Offset: 0x0004C4E1
			public BeginEventDbg(Compiler compiler)
				: base(compiler)
			{
				this.dbgData = new DbgData(compiler);
			}

			// Token: 0x06000F71 RID: 3953 RVA: 0x0004D4F6 File Offset: 0x0004C4F6
			public override bool Output(Processor processor, ActionFrame frame)
			{
				base.OnInstructionExecute(processor);
				return base.Output(processor, frame);
			}

			// Token: 0x040009F2 RID: 2546
			private DbgData dbgData;
		}

		// Token: 0x02000177 RID: 375
		private class TextEventDbg : TextEvent
		{
			// Token: 0x170001E5 RID: 485
			// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0004D507 File Offset: 0x0004C507
			internal override DbgData DbgData
			{
				get
				{
					return this.dbgData;
				}
			}

			// Token: 0x06000F73 RID: 3955 RVA: 0x0004D50F File Offset: 0x0004C50F
			public TextEventDbg(Compiler compiler)
				: base(compiler)
			{
				this.dbgData = new DbgData(compiler);
			}

			// Token: 0x06000F74 RID: 3956 RVA: 0x0004D524 File Offset: 0x0004C524
			public override bool Output(Processor processor, ActionFrame frame)
			{
				base.OnInstructionExecute(processor);
				return base.Output(processor, frame);
			}

			// Token: 0x040009F3 RID: 2547
			private DbgData dbgData;
		}
	}
}

using System;
using System.Collections;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200002C RID: 44
	public sealed class Block : AST
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E48D File Offset: 0x0000D48D
		internal Block(Context context)
			: base(context)
		{
			this.completion = new Completion();
			this.list = new ArrayList();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000E4AC File Offset: 0x0000D4AC
		internal void Append(AST elem)
		{
			this.list.Add(elem);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000E4BC File Offset: 0x0000D4BC
		internal void ComplainAboutAnythingOtherThanClassOrPackage()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				object obj = this.list[i];
				if (!(obj is Class) && !(obj is Package) && !(obj is Import))
				{
					Block block = obj as Block;
					if (block == null || block.list.Count != 0)
					{
						Expression expression = obj as Expression;
						if (expression == null || !(expression.operand is AssemblyCustomAttributeList))
						{
							((AST)obj).context.HandleError(JSError.OnlyClassesAndPackagesAllowed);
							return;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E550 File Offset: 0x0000D550
		internal override object Evaluate()
		{
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				object obj;
				try
				{
					obj = ast.Evaluate();
				}
				catch (JScriptException ex)
				{
					if (ex.context == null)
					{
						ex.context = ast.context;
					}
					throw ex;
				}
				Completion completion = (Completion)obj;
				if (completion.value != null)
				{
					this.completion.value = completion.value;
				}
				if (completion.Continue > 1)
				{
					this.completion.Continue = completion.Continue - 1;
					break;
				}
				if (completion.Exit > 0)
				{
					this.completion.Exit = completion.Exit - 1;
					break;
				}
				if (completion.Return)
				{
					return completion;
				}
				i++;
			}
			return this.completion;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000E65C File Offset: 0x0000D65C
		internal void EvaluateStaticVariableInitializers()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				object obj = this.list[i];
				VariableDeclaration variableDeclaration = obj as VariableDeclaration;
				if (variableDeclaration != null && variableDeclaration.field.IsStatic && !variableDeclaration.field.IsLiteral)
				{
					variableDeclaration.Evaluate();
				}
				else
				{
					StaticInitializer staticInitializer = obj as StaticInitializer;
					if (staticInitializer != null)
					{
						staticInitializer.Evaluate();
					}
					else
					{
						Class @class = obj as Class;
						if (@class != null)
						{
							@class.Evaluate();
						}
						else
						{
							Constant constant = obj as Constant;
							if (constant != null && constant.field.IsStatic)
							{
								constant.Evaluate();
							}
							else
							{
								Block block = obj as Block;
								if (block != null)
								{
									block.EvaluateStaticVariableInitializers();
								}
							}
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E724 File Offset: 0x0000D724
		internal void EvaluateInstanceVariableInitializers()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				object obj = this.list[i];
				VariableDeclaration variableDeclaration = obj as VariableDeclaration;
				if (variableDeclaration != null && !variableDeclaration.field.IsStatic && !variableDeclaration.field.IsLiteral)
				{
					variableDeclaration.Evaluate();
				}
				else
				{
					Block block = obj as Block;
					if (block != null)
					{
						block.EvaluateInstanceVariableInitializers();
					}
				}
				i++;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E798 File Offset: 0x0000D798
		internal override bool HasReturn()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				if (ast.HasReturn())
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E7DC File Offset: 0x0000D7DC
		internal void ProcessAssemblyAttributeLists()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				Expression expression = this.list[i] as Expression;
				if (expression != null)
				{
					AssemblyCustomAttributeList assemblyCustomAttributeList = expression.operand as AssemblyCustomAttributeList;
					if (assemblyCustomAttributeList != null)
					{
						assemblyCustomAttributeList.Process();
					}
				}
				i++;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E82B File Offset: 0x0000D82B
		internal void MarkSuperOKIfIsFirstStatement()
		{
			if (this.list.Count > 0 && this.list[0] is ConstructorCall)
			{
				((ConstructorCall)this.list[0]).isOK = true;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000E868 File Offset: 0x0000D868
		internal override AST PartiallyEvaluate()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				this.list[i] = ast.PartiallyEvaluate();
				i++;
			}
			return this;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E8B2 File Offset: 0x0000D8B2
		internal Expression ToExpression()
		{
			if (this.list.Count == 1 && this.list[0] is Expression)
			{
				return (Expression)this.list[0];
			}
			return null;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000E8E8 File Offset: 0x0000D8E8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label);
			base.compilerGlobals.ContinueLabelStack.Push(label);
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				ast.TranslateToIL(il, Typeob.Void);
				i++;
			}
			il.MarkLabel(label);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000E988 File Offset: 0x0000D988
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				ast.TranslateToILInitializer(il);
				i++;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000E9C8 File Offset: 0x0000D9C8
		internal void TranslateToILInitOnlyInitializers(ILGenerator il)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				Constant constant = this.list[i] as Constant;
				if (constant != null)
				{
					constant.TranslateToILInitOnlyInitializers(il);
				}
				i++;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000EA0C File Offset: 0x0000DA0C
		internal void TranslateToILInstanceInitializers(ILGenerator il)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				if (ast is VariableDeclaration && !((VariableDeclaration)ast).field.IsStatic && !((VariableDeclaration)ast).field.IsLiteral)
				{
					ast.TranslateToILInitializer(il);
					ast.TranslateToIL(il, Typeob.Void);
				}
				else if (ast is FunctionDeclaration && !((FunctionDeclaration)ast).func.isStatic)
				{
					ast.TranslateToILInitializer(il);
				}
				else if (ast is Constant && !((Constant)ast).field.IsStatic)
				{
					ast.TranslateToIL(il, Typeob.Void);
				}
				else if (ast is Block)
				{
					((Block)ast).TranslateToILInstanceInitializers(il);
				}
				i++;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000EAEC File Offset: 0x0000DAEC
		internal void TranslateToILStaticInitializers(ILGenerator il)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				if ((ast is VariableDeclaration && ((VariableDeclaration)ast).field.IsStatic) || (ast is Constant && ((Constant)ast).field.IsStatic))
				{
					ast.TranslateToILInitializer(il);
					ast.TranslateToIL(il, Typeob.Void);
				}
				else if (ast is StaticInitializer)
				{
					ast.TranslateToIL(il, Typeob.Void);
				}
				else if (ast is FunctionDeclaration && ((FunctionDeclaration)ast).func.isStatic)
				{
					ast.TranslateToILInitializer(il);
				}
				else if (ast is Class)
				{
					ast.TranslateToIL(il, Typeob.Void);
				}
				else if (ast is Block)
				{
					((Block)ast).TranslateToILStaticInitializers(il);
				}
				i++;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000EBD8 File Offset: 0x0000DBD8
		internal override Context GetFirstExecutableContext()
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				AST ast = (AST)this.list[i];
				Context firstExecutableContext;
				if ((firstExecutableContext = ast.GetFirstExecutableContext()) != null)
				{
					return firstExecutableContext;
				}
				i++;
			}
			return null;
		}

		// Token: 0x04000090 RID: 144
		private Completion completion;

		// Token: 0x04000091 RID: 145
		private ArrayList list;
	}
}

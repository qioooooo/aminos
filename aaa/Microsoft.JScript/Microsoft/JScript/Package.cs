using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000F5 RID: 245
	public sealed class Package : AST
	{
		// Token: 0x06000AA2 RID: 2722 RVA: 0x00051830 File Offset: 0x00050830
		internal Package(string name, AST id, ASTList classList, Context context)
			: base(context)
		{
			this.name = name;
			this.classList = classList;
			this.scope = (PackageScope)base.Globals.ScopeStack.Peek();
			this.scope.owner = this;
			base.Engine.AddPackage(this.scope);
			Lookup lookup = id as Lookup;
			if (lookup != null)
			{
				lookup.EvaluateAsWrappedNamespace(true);
				return;
			}
			Member member = id as Member;
			if (member != null)
			{
				member.EvaluateAsWrappedNamespace(true);
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000518B0 File Offset: 0x000508B0
		internal override object Evaluate()
		{
			base.Globals.ScopeStack.Push(this.scope);
			object obj;
			try
			{
				int i = 0;
				int count = this.classList.count;
				while (i < count)
				{
					this.classList[i].Evaluate();
					i++;
				}
				obj = new Completion();
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			return obj;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00051928 File Offset: 0x00050928
		public static void JScriptPackage(string rootName, VsaEngine engine)
		{
			GlobalScope globalScope = ((IActivationObject)engine.ScriptObjectStackTop()).GetGlobalScope();
			if (globalScope.GetLocalField(rootName) == null)
			{
				FieldInfo fieldInfo = globalScope.AddNewField(rootName, Namespace.GetNamespace(rootName, engine), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00051964 File Offset: 0x00050964
		internal void MergeWith(Package p)
		{
			int i = 0;
			int count = p.classList.count;
			while (i < count)
			{
				this.classList.Append(p.classList[i]);
				i++;
			}
			this.scope.MergeWith(p.scope);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000519B4 File Offset: 0x000509B4
		internal override AST PartiallyEvaluate()
		{
			this.scope.AddOwnName();
			base.Globals.ScopeStack.Push(this.scope);
			try
			{
				int i = 0;
				int count = this.classList.count;
				while (i < count)
				{
					this.classList[i].PartiallyEvaluate();
					i++;
				}
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			return this;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00051A34 File Offset: 0x00050A34
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			base.Globals.ScopeStack.Push(this.scope);
			int i = 0;
			int count = this.classList.count;
			while (i < count)
			{
				this.classList[i].TranslateToIL(il, Typeob.Void);
				i++;
			}
			base.Globals.ScopeStack.Pop();
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00051A98 File Offset: 0x00050A98
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			string text = this.name;
			int num = text.IndexOf('.');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			il.Emit(OpCodes.Ldstr, text);
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptPackageMethod);
			base.Globals.ScopeStack.Push(this.scope);
			int i = 0;
			int count = this.classList.count;
			while (i < count)
			{
				this.classList[i].TranslateToILInitializer(il);
				i++;
			}
			base.Globals.ScopeStack.Pop();
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00051B36 File Offset: 0x00050B36
		internal override Context GetFirstExecutableContext()
		{
			return null;
		}

		// Token: 0x04000693 RID: 1683
		private string name;

		// Token: 0x04000694 RID: 1684
		private ASTList classList;

		// Token: 0x04000695 RID: 1685
		private PackageScope scope;
	}
}

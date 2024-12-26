using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200008F RID: 143
	public sealed class Import : AST
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x0002E7EC File Offset: 0x0002D7EC
		internal Import(Context context, AST name)
			: base(context)
		{
			if (name == null)
			{
				return;
			}
			WrappedNamespace wrappedNamespace = name.EvaluateAsWrappedNamespace(true);
			base.Engine.SetEnclosingContext(wrappedNamespace);
			this.name = wrappedNamespace.name;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002E824 File Offset: 0x0002D824
		internal override object Evaluate()
		{
			return new Completion();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002E82B File Offset: 0x0002D82B
		internal override AST PartiallyEvaluate()
		{
			return this;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002E830 File Offset: 0x0002D830
		public static void JScriptImport(string name, VsaEngine engine)
		{
			int num = name.IndexOf('.');
			string text = ((num > 0) ? name.Substring(0, num) : name);
			GlobalScope globalScope = ((IActivationObject)engine.ScriptObjectStackTop()).GetGlobalScope();
			if (globalScope.GetLocalField(text) == null)
			{
				FieldInfo fieldInfo = globalScope.AddNewField(text, Namespace.GetNamespace(text, engine), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
			}
			engine.SetEnclosingContext(new WrappedNamespace(name, engine, false));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002E891 File Offset: 0x0002D891
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002E893 File Offset: 0x0002D893
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			il.Emit(OpCodes.Ldstr, this.name);
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptImportMethod);
		}

		// Token: 0x04000304 RID: 772
		private string name;
	}
}

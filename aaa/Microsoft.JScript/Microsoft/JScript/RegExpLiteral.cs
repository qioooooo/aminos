using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000105 RID: 261
	internal sealed class RegExpLiteral : AST
	{
		// Token: 0x06000B1B RID: 2843 RVA: 0x00054E4C File Offset: 0x00053E4C
		internal RegExpLiteral(string source, string flags, Context context)
			: base(context)
		{
			this.source = source;
			this.ignoreCase = (this.global = (this.multiline = false));
			if (flags != null)
			{
				int i = 0;
				while (i < flags.Length)
				{
					char c = flags[i];
					switch (c)
					{
					case 'g':
						if (this.global)
						{
							throw new JScriptException(JSError.RegExpSyntax);
						}
						this.global = true;
						break;
					case 'h':
						goto IL_00AC;
					case 'i':
						if (this.ignoreCase)
						{
							throw new JScriptException(JSError.RegExpSyntax);
						}
						this.ignoreCase = true;
						break;
					default:
						if (c != 'm')
						{
							goto IL_00AC;
						}
						if (this.multiline)
						{
							throw new JScriptException(JSError.RegExpSyntax);
						}
						this.multiline = true;
						break;
					}
					i++;
					continue;
					IL_00AC:
					throw new JScriptException(JSError.RegExpSyntax);
				}
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00054F20 File Offset: 0x00053F20
		internal override object Evaluate()
		{
			if (VsaEngine.executeForJSEE)
			{
				throw new JScriptException(JSError.NonSupportedInDebugger);
			}
			RegExpObject regExpObject = (RegExpObject)base.Globals.RegExpTable[this];
			if (regExpObject == null)
			{
				regExpObject = (RegExpObject)base.Engine.GetOriginalRegExpConstructor().Construct(this.source, this.ignoreCase, this.global, this.multiline);
				base.Globals.RegExpTable[this] = regExpObject;
			}
			return regExpObject;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00054F9A File Offset: 0x00053F9A
		internal override IReflect InferType(JSField inferenceTarget)
		{
			return Typeob.RegExpObject;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00054FA4 File Offset: 0x00053FA4
		internal override AST PartiallyEvaluate()
		{
			string text = "regexp " + RegExpLiteral.counter++.ToString(CultureInfo.InvariantCulture);
			GlobalScope globalScope = (GlobalScope)base.Engine.GetGlobalScope().GetObject();
			JSGlobalField jsglobalField = (JSGlobalField)globalScope.AddNewField(text, null, FieldAttributes.Assembly);
			jsglobalField.type = new TypeExpression(new ConstantWrapper(Typeob.RegExpObject, this.context));
			this.regExpVar = jsglobalField;
			return this;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0005501E File Offset: 0x0005401E
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			il.Emit(OpCodes.Ldsfld, (FieldInfo)this.regExpVar.GetMetaData());
			Convert.Emit(this, il, Typeob.RegExpObject, rtype);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00055048 File Offset: 0x00054048
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			ScriptObject scriptObject = base.Engine.ScriptObjectStackTop();
			while (scriptObject != null && (scriptObject is WithObject || scriptObject is BlockScope))
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Pop);
			}
			il.Emit(OpCodes.Ldsfld, (FieldInfo)this.regExpVar.GetMetaData());
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Brtrue_S, label);
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.getOriginalRegExpConstructorMethod);
			il.Emit(OpCodes.Ldstr, this.source);
			if (this.ignoreCase)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (this.global)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (this.multiline)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			il.Emit(OpCodes.Call, CompilerGlobals.regExpConstructMethod);
			il.Emit(OpCodes.Castclass, Typeob.RegExpObject);
			il.Emit(OpCodes.Stsfld, (FieldInfo)this.regExpVar.GetMetaData());
			il.MarkLabel(label);
		}

		// Token: 0x040006B7 RID: 1719
		private string source;

		// Token: 0x040006B8 RID: 1720
		private bool ignoreCase;

		// Token: 0x040006B9 RID: 1721
		private bool global;

		// Token: 0x040006BA RID: 1722
		private bool multiline;

		// Token: 0x040006BB RID: 1723
		private JSGlobalField regExpVar;

		// Token: 0x040006BC RID: 1724
		private static int counter;
	}
}

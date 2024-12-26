using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000120 RID: 288
	public sealed class Throw : AST
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x000599BE File Offset: 0x000589BE
		internal Throw(Context context, AST operand)
			: base(context)
		{
			this.operand = operand;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000599D0 File Offset: 0x000589D0
		internal override object Evaluate()
		{
			if (this.operand == null)
			{
				ScriptObject scriptObject = base.Engine.ScriptObjectStackTop();
				while (scriptObject != null)
				{
					BlockScope blockScope = scriptObject as BlockScope;
					if (blockScope != null && blockScope.catchHanderScope)
					{
						throw (Exception)blockScope.GetFields(BindingFlags.Static | BindingFlags.Public)[0].GetValue(null);
					}
				}
			}
			throw Throw.JScriptThrow(this.operand.Evaluate());
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00059A2E File Offset: 0x00058A2E
		internal override bool HasReturn()
		{
			return true;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00059A34 File Offset: 0x00058A34
		public static Exception JScriptThrow(object value)
		{
			if (value is Exception)
			{
				return (Exception)value;
			}
			if (value is ErrorObject && ((ErrorObject)value).exception is Exception)
			{
				return (Exception)((ErrorObject)value).exception;
			}
			return new JScriptException(value, null);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00059A84 File Offset: 0x00058A84
		internal override AST PartiallyEvaluate()
		{
			if (this.operand == null)
			{
				BlockScope blockScope = null;
				for (ScriptObject scriptObject = base.Engine.ScriptObjectStackTop(); scriptObject != null; scriptObject = scriptObject.GetParent())
				{
					if (!(scriptObject is WithObject))
					{
						blockScope = scriptObject as BlockScope;
						if (blockScope == null || blockScope.catchHanderScope)
						{
							break;
						}
					}
				}
				if (blockScope == null)
				{
					this.context.HandleError(JSError.BadThrow);
					this.operand = new ConstantWrapper(null, this.context);
				}
			}
			else
			{
				this.operand = this.operand.PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00059B08 File Offset: 0x00058B08
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.context.EmitLineInfo(il);
			if (this.operand == null)
			{
				il.Emit(OpCodes.Rethrow);
				return;
			}
			IReflect reflect = this.operand.InferType(null);
			if (reflect is Type && Typeob.Exception.IsAssignableFrom((Type)reflect))
			{
				this.operand.TranslateToIL(il, (Type)reflect);
			}
			else
			{
				this.operand.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.jScriptThrowMethod);
			}
			il.Emit(OpCodes.Throw);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00059B9C File Offset: 0x00058B9C
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.operand != null)
			{
				this.operand.TranslateToILInitializer(il);
			}
		}

		// Token: 0x04000706 RID: 1798
		private AST operand;
	}
}

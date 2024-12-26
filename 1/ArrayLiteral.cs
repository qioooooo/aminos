using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000011 RID: 17
	public sealed class ArrayLiteral : AST
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00005001 File Offset: 0x00004001
		public ArrayLiteral(Context context, ASTList elements)
			: base(context)
		{
			this.elements = elements;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005014 File Offset: 0x00004014
		internal bool AssignmentCompatible(IReflect lhir, bool reportError)
		{
			if (lhir == Typeob.Object || lhir == Typeob.Array || lhir is ArrayObject)
			{
				return true;
			}
			IReflect reflect;
			if (lhir == Typeob.Array)
			{
				reflect = Typeob.Object;
			}
			else if (lhir is TypedArray)
			{
				TypedArray typedArray = (TypedArray)lhir;
				if (typedArray.rank != 1)
				{
					this.context.HandleError(JSError.TypeMismatch, reportError);
					return false;
				}
				reflect = typedArray.elementType;
			}
			else
			{
				if (!(lhir is Type) || !((Type)lhir).IsArray)
				{
					return false;
				}
				Type type = (Type)lhir;
				if (type.GetArrayRank() != 1)
				{
					this.context.HandleError(JSError.TypeMismatch, reportError);
					return false;
				}
				reflect = type.GetElementType();
			}
			int i = 0;
			int count = this.elements.count;
			while (i < count)
			{
				if (!Binding.AssignmentCompatible(reflect, this.elements[i], this.elements[i].InferType(null), reportError))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005100 File Offset: 0x00004100
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			int i = 0;
			int count = this.elements.count;
			while (i < count)
			{
				this.elements[i].CheckIfOKToUseInSuperConstructorCall();
				i++;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005138 File Offset: 0x00004138
		internal override object Evaluate()
		{
			if (VsaEngine.executeForJSEE)
			{
				throw new JScriptException(JSError.NonSupportedInDebugger);
			}
			int count = this.elements.count;
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.elements[i].Evaluate();
			}
			return base.Engine.GetOriginalArrayConstructor().ConstructArray(array);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000519C File Offset: 0x0000419C
		internal bool IsOkToUseInCustomAttribute()
		{
			int count = this.elements.count;
			for (int i = 0; i < count; i++)
			{
				object obj = this.elements[i];
				if (!(obj is ConstantWrapper))
				{
					return false;
				}
				if (CustomAttribute.TypeOfArgument(((ConstantWrapper)obj).Evaluate()) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000051F0 File Offset: 0x000041F0
		internal override AST PartiallyEvaluate()
		{
			int count = this.elements.count;
			for (int i = 0; i < count; i++)
			{
				this.elements[i] = this.elements[i].PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005233 File Offset: 0x00004233
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.ArrayObject;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000523C File Offset: 0x0000423C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (rtype == Typeob.Array)
			{
				this.TranslateToILArray(il, Typeob.Object);
				return;
			}
			if (rtype.IsArray && rtype.GetArrayRank() == 1)
			{
				this.TranslateToILArray(il, rtype.GetElementType());
				return;
			}
			int count = this.elements.count;
			MethodInfo methodInfo;
			if (base.Engine.Globals.globalObject is LenientGlobalObject)
			{
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Call, CompilerGlobals.getOriginalArrayConstructorMethod);
				methodInfo = CompilerGlobals.constructArrayMethod;
			}
			else
			{
				methodInfo = CompilerGlobals.fastConstructArrayLiteralMethod;
			}
			ConstantWrapper.TranslateToILInt(il, count);
			il.Emit(OpCodes.Newarr, Typeob.Object);
			for (int i = 0; i < count; i++)
			{
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				this.elements[i].TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Stelem_Ref);
			}
			il.Emit(OpCodes.Call, methodInfo);
			Convert.Emit(this, il, Typeob.ArrayObject, rtype);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005338 File Offset: 0x00004338
		private void TranslateToILArray(ILGenerator il, Type etype)
		{
			int count = this.elements.count;
			ConstantWrapper.TranslateToILInt(il, count);
			Type.GetTypeCode(etype);
			il.Emit(OpCodes.Newarr, etype);
			for (int i = 0; i < count; i++)
			{
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				if (etype.IsValueType && !etype.IsPrimitive)
				{
					il.Emit(OpCodes.Ldelema, etype);
				}
				this.elements[i].TranslateToIL(il, etype);
				Binding.TranslateToStelem(il, etype);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000053C0 File Offset: 0x000043C0
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			int i = 0;
			int count = this.elements.count;
			while (i < count)
			{
				this.elements[i].TranslateToILInitializer(il);
				i++;
			}
		}

		// Token: 0x04000031 RID: 49
		internal ASTList elements;
	}
}

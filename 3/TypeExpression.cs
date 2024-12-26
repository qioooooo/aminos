using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000124 RID: 292
	internal sealed class TypeExpression : AST
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x0005AA8C File Offset: 0x00059A8C
		internal TypeExpression(AST expression)
			: base(expression.context)
		{
			this.expression = expression;
			this.isArray = false;
			this.rank = 0;
			this.recursive = false;
			this.cachedIR = null;
			if (expression is Lookup)
			{
				string text = expression.ToString();
				object predefinedType = Globals.TypeRefs.GetPredefinedType(text);
				if (predefinedType != null)
				{
					this.expression = new ConstantWrapper(predefinedType, expression.context);
				}
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0005AAF8 File Offset: 0x00059AF8
		internal override object Evaluate()
		{
			return this.ToIReflect();
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0005AB00 File Offset: 0x00059B00
		internal override IReflect InferType(JSField inference_target)
		{
			return this.ToIReflect();
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0005AB08 File Offset: 0x00059B08
		internal bool IsCLSCompliant()
		{
			object obj = this.expression.Evaluate();
			return TypeExpression.TypeIsCLSCompliant(obj);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0005AB28 File Offset: 0x00059B28
		internal override AST PartiallyEvaluate()
		{
			if (this.recursive)
			{
				if (this.expression is ConstantWrapper)
				{
					return this;
				}
				this.expression = new ConstantWrapper(Typeob.Object, this.context);
				return this;
			}
			else
			{
				Member member = this.expression as Member;
				if (member != null)
				{
					object obj = member.EvaluateAsType();
					if (obj != null)
					{
						this.expression = new ConstantWrapper(obj, member.context);
						return this;
					}
				}
				this.recursive = true;
				this.expression = this.expression.PartiallyEvaluate();
				this.recursive = false;
				if (this.expression is TypeExpression)
				{
					return this;
				}
				Type type;
				if (this.expression is ConstantWrapper)
				{
					object obj2 = this.expression.Evaluate();
					if (obj2 == null)
					{
						this.expression.context.HandleError(JSError.NeedType);
						this.expression = new ConstantWrapper(Typeob.Object, this.context);
						return this;
					}
					type = Globals.TypeRefs.ToReferenceContext(obj2.GetType());
					Binding.WarnIfObsolete(obj2 as Type, this.expression.context);
				}
				else
				{
					if (!this.expression.OkToUseAsType())
					{
						this.expression.context.HandleError(JSError.NeedCompileTimeConstant);
						this.expression = new ConstantWrapper(Typeob.Object, this.expression.context);
						return this;
					}
					type = Globals.TypeRefs.ToReferenceContext(this.expression.Evaluate().GetType());
				}
				if (type == null || (type != Typeob.ClassScope && type != Typeob.TypedArray && !Typeob.Type.IsAssignableFrom(type)))
				{
					this.expression.context.HandleError(JSError.NeedType);
					this.expression = new ConstantWrapper(Typeob.Object, this.expression.context);
				}
				return this;
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0005ACE0 File Offset: 0x00059CE0
		internal IReflect ToIReflect()
		{
			if (!(this.expression is ConstantWrapper))
			{
				this.PartiallyEvaluate();
			}
			IReflect reflect = this.cachedIR;
			if (reflect != null)
			{
				return reflect;
			}
			object obj = this.expression.Evaluate();
			if (obj is ClassScope || obj is TypedArray || this.context == null)
			{
				reflect = (IReflect)obj;
			}
			else
			{
				reflect = Convert.ToIReflect((Type)obj, base.Engine);
			}
			if (this.isArray)
			{
				return this.cachedIR = new TypedArray(reflect, this.rank);
			}
			return this.cachedIR = reflect;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0005AD74 File Offset: 0x00059D74
		internal Type ToType()
		{
			if (!(this.expression is ConstantWrapper))
			{
				this.PartiallyEvaluate();
			}
			object obj = this.expression.Evaluate();
			Type type;
			if (obj is ClassScope)
			{
				type = ((ClassScope)obj).GetTypeBuilderOrEnumBuilder();
			}
			else if (obj is TypedArray)
			{
				type = Convert.ToType((TypedArray)obj);
			}
			else
			{
				type = Globals.TypeRefs.ToReferenceContext((Type)obj);
			}
			if (this.isArray)
			{
				return Convert.ToType(TypedArray.ToRankString(this.rank), type);
			}
			return type;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0005ADFB File Offset: 0x00059DFB
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.expression.TranslateToIL(il, rtype);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0005AE0A File Offset: 0x00059E0A
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.expression.TranslateToILInitializer(il);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0005AE18 File Offset: 0x00059E18
		internal static bool TypeIsCLSCompliant(object type)
		{
			if (type is ClassScope)
			{
				return ((ClassScope)type).IsCLSCompliant();
			}
			if (type is TypedArray)
			{
				object elementType = ((TypedArray)type).elementType;
				return !(elementType is TypedArray) && (!(elementType is Type) || !((Type)elementType).IsArray) && TypeExpression.TypeIsCLSCompliant(elementType);
			}
			Type type2 = (Type)type;
			if (type2.IsPrimitive)
			{
				return type2 == Typeob.Boolean || type2 == Typeob.Byte || type2 == Typeob.Char || type2 == Typeob.Double || type2 == Typeob.Int16 || type2 == Typeob.Int32 || type2 == Typeob.Int64 || type2 == Typeob.Single;
			}
			if (type2.IsArray)
			{
				Type elementType2 = type2.GetElementType();
				return !elementType2.IsArray && TypeExpression.TypeIsCLSCompliant(type2);
			}
			object[] array = CustomAttribute.GetCustomAttributes(type2, typeof(CLSCompliantAttribute), false);
			if (array.Length > 0)
			{
				return ((CLSCompliantAttribute)array[0]).IsCompliant;
			}
			Module module = type2.Module;
			array = CustomAttribute.GetCustomAttributes(module, typeof(CLSCompliantAttribute), false);
			if (array.Length > 0)
			{
				return ((CLSCompliantAttribute)array[0]).IsCompliant;
			}
			Assembly assembly = module.Assembly;
			array = CustomAttribute.GetCustomAttributes(assembly, typeof(CLSCompliantAttribute), false);
			return array.Length > 0 && ((CLSCompliantAttribute)array[0]).IsCompliant;
		}

		// Token: 0x04000712 RID: 1810
		internal AST expression;

		// Token: 0x04000713 RID: 1811
		internal bool isArray;

		// Token: 0x04000714 RID: 1812
		internal int rank;

		// Token: 0x04000715 RID: 1813
		private bool recursive;

		// Token: 0x04000716 RID: 1814
		private IReflect cachedIR;
	}
}

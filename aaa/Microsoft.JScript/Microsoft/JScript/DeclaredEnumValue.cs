using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000073 RID: 115
	internal sealed class DeclaredEnumValue : EnumWrapper
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x00026025 File Offset: 0x00025025
		internal DeclaredEnumValue(object value, string name, ClassScope classScope)
		{
			this._name = name;
			this._classScope = classScope;
			this._value = value;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00026042 File Offset: 0x00025042
		internal override object value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0002604A File Offset: 0x0002504A
		internal override Type type
		{
			get
			{
				return this._classScope.GetTypeBuilderOrEnumBuilder();
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00026057 File Offset: 0x00025057
		internal override string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0002605F File Offset: 0x0002505F
		internal override IReflect classScopeOrType
		{
			get
			{
				return this._classScope;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00026068 File Offset: 0x00025068
		internal void CoerceToBaseType(Type bt, Context errCtx)
		{
			object obj = 0;
			AST ast = ((AST)this.value).PartiallyEvaluate();
			if (ast is ConstantWrapper)
			{
				obj = ((ConstantWrapper)ast).Evaluate();
			}
			else
			{
				ast.context.HandleError(JSError.NotConst);
			}
			try
			{
				this._value = Convert.CoerceT(obj, bt);
			}
			catch
			{
				errCtx.HandleError(JSError.TypeMismatch);
				this._value = Convert.CoerceT(0, bt);
			}
		}

		// Token: 0x0400024F RID: 591
		private string _name;

		// Token: 0x04000250 RID: 592
		internal ClassScope _classScope;

		// Token: 0x04000251 RID: 593
		internal object _value;
	}
}

using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015E RID: 350
	public sealed class ExpressionContext
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x0003410C File Offset: 0x0003310C
		public ExpressionContext(CodeExpression expression, Type expressionType, object owner, object presetValue)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (expressionType == null)
			{
				throw new ArgumentNullException("expressionType");
			}
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._expression = expression;
			this._expressionType = expressionType;
			this._owner = owner;
			this._presetValue = presetValue;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00034166 File Offset: 0x00033166
		public ExpressionContext(CodeExpression expression, Type expressionType, object owner)
			: this(expression, expressionType, owner, null)
		{
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00034172 File Offset: 0x00033172
		public CodeExpression Expression
		{
			get
			{
				return this._expression;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0003417A File Offset: 0x0003317A
		public Type ExpressionType
		{
			get
			{
				return this._expressionType;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00034182 File Offset: 0x00033182
		public object Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0003418A File Offset: 0x0003318A
		public object PresetValue
		{
			get
			{
				return this._presetValue;
			}
		}

		// Token: 0x04000EEE RID: 3822
		private CodeExpression _expression;

		// Token: 0x04000EEF RID: 3823
		private Type _expressionType;

		// Token: 0x04000EF0 RID: 3824
		private object _owner;

		// Token: 0x04000EF1 RID: 3825
		private object _presetValue;
	}
}

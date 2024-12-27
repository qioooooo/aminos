using System;
using System.CodeDom;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015F RID: 351
	internal sealed class ExpressionTable
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00034192 File Offset: 0x00033192
		private Hashtable Expressions
		{
			get
			{
				if (this._expressions == null)
				{
					this._expressions = new Hashtable(new ExpressionTable.ReferenceComparer());
				}
				return this._expressions;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000341B2 File Offset: 0x000331B2
		internal void SetExpression(object value, CodeExpression expression, bool isPreset)
		{
			this.Expressions[value] = new ExpressionTable.ExpressionInfo(expression, isPreset);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000341C8 File Offset: 0x000331C8
		internal CodeExpression GetExpression(object value)
		{
			CodeExpression codeExpression = null;
			ExpressionTable.ExpressionInfo expressionInfo = this.Expressions[value] as ExpressionTable.ExpressionInfo;
			if (expressionInfo != null)
			{
				codeExpression = expressionInfo.Expression;
			}
			return codeExpression;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000341F4 File Offset: 0x000331F4
		internal bool ContainsPresetExpression(object value)
		{
			ExpressionTable.ExpressionInfo expressionInfo = this.Expressions[value] as ExpressionTable.ExpressionInfo;
			return expressionInfo != null && expressionInfo.IsPreset;
		}

		// Token: 0x04000EF2 RID: 3826
		private Hashtable _expressions;

		// Token: 0x02000160 RID: 352
		private class ExpressionInfo
		{
			// Token: 0x06000D24 RID: 3364 RVA: 0x00034226 File Offset: 0x00033226
			internal ExpressionInfo(CodeExpression expression, bool isPreset)
			{
				this._expression = expression;
				this._isPreset = isPreset;
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0003423C File Offset: 0x0003323C
			internal CodeExpression Expression
			{
				get
				{
					return this._expression;
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00034244 File Offset: 0x00033244
			internal bool IsPreset
			{
				get
				{
					return this._isPreset;
				}
			}

			// Token: 0x04000EF3 RID: 3827
			private CodeExpression _expression;

			// Token: 0x04000EF4 RID: 3828
			private bool _isPreset;
		}

		// Token: 0x02000161 RID: 353
		private class ReferenceComparer : IEqualityComparer
		{
			// Token: 0x06000D27 RID: 3367 RVA: 0x0003424C File Offset: 0x0003324C
			bool IEqualityComparer.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x06000D28 RID: 3368 RVA: 0x00034255 File Offset: 0x00033255
			int IEqualityComparer.GetHashCode(object x)
			{
				if (x != null)
				{
					return x.GetHashCode();
				}
				return 0;
			}
		}
	}
}

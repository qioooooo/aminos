using System;
using System.CodeDom;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	internal sealed class ExpressionTable
	{
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

		internal void SetExpression(object value, CodeExpression expression, bool isPreset)
		{
			this.Expressions[value] = new ExpressionTable.ExpressionInfo(expression, isPreset);
		}

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

		internal bool ContainsPresetExpression(object value)
		{
			ExpressionTable.ExpressionInfo expressionInfo = this.Expressions[value] as ExpressionTable.ExpressionInfo;
			return expressionInfo != null && expressionInfo.IsPreset;
		}

		private Hashtable _expressions;

		private class ExpressionInfo
		{
			internal ExpressionInfo(CodeExpression expression, bool isPreset)
			{
				this._expression = expression;
				this._isPreset = isPreset;
			}

			internal CodeExpression Expression
			{
				get
				{
					return this._expression;
				}
			}

			internal bool IsPreset
			{
				get
				{
					return this._isPreset;
				}
			}

			private CodeExpression _expression;

			private bool _isPreset;
		}

		private class ReferenceComparer : IEqualityComparer
		{
			bool IEqualityComparer.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

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

using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class ExpressionContext
	{
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

		public ExpressionContext(CodeExpression expression, Type expressionType, object owner)
			: this(expression, expressionType, owner, null)
		{
		}

		public CodeExpression Expression
		{
			get
			{
				return this._expression;
			}
		}

		public Type ExpressionType
		{
			get
			{
				return this._expressionType;
			}
		}

		public object Owner
		{
			get
			{
				return this._owner;
			}
		}

		public object PresetValue
		{
			get
			{
				return this._presetValue;
			}
		}

		private CodeExpression _expression;

		private Type _expressionType;

		private object _owner;

		private object _presetValue;
	}
}

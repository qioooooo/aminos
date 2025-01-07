using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class RootContext
	{
		public RootContext(CodeExpression expression, object value)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.expression = expression;
			this.value = value;
		}

		public CodeExpression Expression
		{
			get
			{
				return this.expression;
			}
		}

		public object Value
		{
			get
			{
				return this.value;
			}
		}

		private CodeExpression expression;

		private object value;
	}
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;

namespace System.Web.UI.Design
{
	internal sealed class DesignTimeDataBinding
	{
		public DesignTimeDataBinding(DataBinding runtimeDataBinding)
		{
			this._runtimeDataBinding = runtimeDataBinding;
		}

		public DesignTimeDataBinding(PropertyDescriptor propDesc, string expression)
		{
			this._expression = expression;
			this._runtimeDataBinding = new DataBinding(propDesc.Name, propDesc.PropertyType, expression);
		}

		public DesignTimeDataBinding(PropertyDescriptor propDesc, string field, string format, bool twoWayBinding)
		{
			this._field = field;
			this._format = format;
			if (twoWayBinding)
			{
				this._expression = DesignTimeDataBinding.CreateBindExpression(field, format);
			}
			else
			{
				this._expression = DesignTimeDataBinding.CreateEvalExpression(field, format);
			}
			this._parsed = true;
			this._twoWayBinding = twoWayBinding;
			this._runtimeDataBinding = new DataBinding(propDesc.Name, propDesc.PropertyType, this._expression);
		}

		public bool IsCustom
		{
			get
			{
				this.EnsureParsed();
				return this._field == null;
			}
		}

		public string Expression
		{
			get
			{
				this.EnsureParsed();
				return this._expression;
			}
		}

		public string Field
		{
			get
			{
				this.EnsureParsed();
				return this._field;
			}
		}

		public string Format
		{
			get
			{
				this.EnsureParsed();
				return this._format;
			}
		}

		public bool IsTwoWayBound
		{
			get
			{
				this.EnsureParsed();
				return this._twoWayBinding;
			}
		}

		public DataBinding RuntimeDataBinding
		{
			get
			{
				return this._runtimeDataBinding;
			}
		}

		public static string CreateBindExpression(string field, string format)
		{
			string text = field;
			bool flag = false;
			foreach (char c in field)
			{
				if (!char.IsLetterOrDigit(c) && c != '_' && !flag)
				{
					text = "[" + field + "]";
					flag = true;
				}
			}
			if (format != null && format.Length != 0)
			{
				return string.Format(CultureInfo.InvariantCulture, "Bind(\"{0}\", \"{1}\")", new object[] { text, format });
			}
			return string.Format(CultureInfo.InvariantCulture, "Bind(\"{0}\")", new object[] { text });
		}

		public static string CreateEvalExpression(string field, string format)
		{
			string text = field;
			bool flag = false;
			foreach (char c in field)
			{
				if (!char.IsLetterOrDigit(c) && c != '_' && !flag)
				{
					text = "[" + field + "]";
					flag = true;
				}
			}
			if (format != null && format.Length != 0)
			{
				return string.Format(CultureInfo.InvariantCulture, "Eval(\"{0}\", \"{1}\")", new object[] { text, format });
			}
			return string.Format(CultureInfo.InvariantCulture, "Eval(\"{0}\")", new object[] { text });
		}

		private void EnsureParsed()
		{
			if (!this._parsed)
			{
				this._expression = this._runtimeDataBinding.Expression.Trim();
				if (this._expression.Length != 0)
				{
					try
					{
						bool flag = false;
						Match match = DesignTimeDataBinding.EvalRegex.Match(this._expression);
						if (match.Success)
						{
							flag = true;
						}
						else
						{
							match = DesignTimeDataBinding.BindExpressionRegex.Match(this._expression);
						}
						if (match.Success)
						{
							string value = match.Groups["params"].Value;
							if ((match = DesignTimeDataBinding.BindParametersRegex.Match(value, 0)).Success)
							{
								this._field = match.Groups["fieldName"].Value;
								Group group = match.Groups["formatString"];
								if (group != null)
								{
									this._format = group.Value;
								}
								if (!flag)
								{
									this._twoWayBinding = true;
								}
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			this._parsed = true;
		}

		private static readonly Regex EvalRegex = new EvalExpressionRegex();

		private static readonly Regex BindExpressionRegex = new BindExpressionRegex();

		private static readonly Regex BindParametersRegex = new BindParametersRegex();

		private DataBinding _runtimeDataBinding;

		private bool _parsed;

		private bool _twoWayBinding;

		private string _field;

		private string _format;

		private string _expression;
	}
}

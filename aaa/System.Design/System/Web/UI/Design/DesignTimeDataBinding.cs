using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;

namespace System.Web.UI.Design
{
	// Token: 0x02000362 RID: 866
	internal sealed class DesignTimeDataBinding
	{
		// Token: 0x0600209A RID: 8346 RVA: 0x000B730E File Offset: 0x000B630E
		public DesignTimeDataBinding(DataBinding runtimeDataBinding)
		{
			this._runtimeDataBinding = runtimeDataBinding;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000B731D File Offset: 0x000B631D
		public DesignTimeDataBinding(PropertyDescriptor propDesc, string expression)
		{
			this._expression = expression;
			this._runtimeDataBinding = new DataBinding(propDesc.Name, propDesc.PropertyType, expression);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000B7344 File Offset: 0x000B6344
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

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x000B73B1 File Offset: 0x000B63B1
		public bool IsCustom
		{
			get
			{
				this.EnsureParsed();
				return this._field == null;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x000B73C2 File Offset: 0x000B63C2
		public string Expression
		{
			get
			{
				this.EnsureParsed();
				return this._expression;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x000B73D0 File Offset: 0x000B63D0
		public string Field
		{
			get
			{
				this.EnsureParsed();
				return this._field;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000B73DE File Offset: 0x000B63DE
		public string Format
		{
			get
			{
				this.EnsureParsed();
				return this._format;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x000B73EC File Offset: 0x000B63EC
		public bool IsTwoWayBound
		{
			get
			{
				this.EnsureParsed();
				return this._twoWayBinding;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000B73FA File Offset: 0x000B63FA
		public DataBinding RuntimeDataBinding
		{
			get
			{
				return this._runtimeDataBinding;
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000B7404 File Offset: 0x000B6404
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

		// Token: 0x060020A4 RID: 8356 RVA: 0x000B74A0 File Offset: 0x000B64A0
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

		// Token: 0x060020A5 RID: 8357 RVA: 0x000B753C File Offset: 0x000B653C
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

		// Token: 0x040017E1 RID: 6113
		private static readonly Regex EvalRegex = new EvalExpressionRegex();

		// Token: 0x040017E2 RID: 6114
		private static readonly Regex BindExpressionRegex = new BindExpressionRegex();

		// Token: 0x040017E3 RID: 6115
		private static readonly Regex BindParametersRegex = new BindParametersRegex();

		// Token: 0x040017E4 RID: 6116
		private DataBinding _runtimeDataBinding;

		// Token: 0x040017E5 RID: 6117
		private bool _parsed;

		// Token: 0x040017E6 RID: 6118
		private bool _twoWayBinding;

		// Token: 0x040017E7 RID: 6119
		private string _field;

		// Token: 0x040017E8 RID: 6120
		private string _format;

		// Token: 0x040017E9 RID: 6121
		private string _expression;
	}
}

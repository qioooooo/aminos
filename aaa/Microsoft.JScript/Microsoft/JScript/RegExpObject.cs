using System;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x02000107 RID: 263
	public sealed class RegExpObject : JSObject
	{
		// Token: 0x06000B2F RID: 2863 RVA: 0x000555C4 File Offset: 0x000545C4
		internal RegExpObject(RegExpPrototype parent, string source, bool ignoreCase, bool global, bool multiline, RegExpConstructor regExpConst)
			: base(parent)
		{
			this.regExpConst = regExpConst;
			this.sourceInt = source;
			this.ignoreCaseInt = ignoreCase;
			this.globalInt = global;
			this.multilineInt = multiline;
			RegexOptions regexOptions = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
			if (ignoreCase)
			{
				regexOptions |= RegexOptions.IgnoreCase;
			}
			if (multiline)
			{
				regexOptions |= RegexOptions.Multiline;
			}
			try
			{
				this.regex = new Regex(source, regexOptions);
			}
			catch (ArgumentException)
			{
				throw new JScriptException(JSError.RegExpSyntax);
			}
			this.lastIndexInt = 0;
			this.noExpando = false;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00055654 File Offset: 0x00054654
		internal RegExpObject(Regex regex)
			: base(null)
		{
			this.regExpConst = null;
			this.sourceInt = "";
			this.ignoreCaseInt = (regex.Options & RegexOptions.IgnoreCase) != RegexOptions.None;
			this.globalInt = false;
			this.multilineInt = (regex.Options & RegexOptions.Multiline) != RegexOptions.None;
			this.regex = regex;
			this.lastIndexInt = 0;
			this.noExpando = true;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000556C4 File Offset: 0x000546C4
		internal RegExpObject compile(string source, string flags)
		{
			this.sourceInt = source;
			this.ignoreCaseInt = (this.globalInt = (this.multilineInt = false));
			RegexOptions regexOptions = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
			int i = 0;
			while (i < flags.Length)
			{
				char c = flags[i];
				switch (c)
				{
				case 'g':
					if (this.globalInt)
					{
						throw new JScriptException(JSError.RegExpSyntax);
					}
					this.globalInt = true;
					break;
				case 'h':
					goto IL_00B0;
				case 'i':
					if (this.ignoreCaseInt)
					{
						throw new JScriptException(JSError.RegExpSyntax);
					}
					this.ignoreCaseInt = true;
					regexOptions |= RegexOptions.IgnoreCase;
					break;
				default:
					if (c != 'm')
					{
						goto IL_00B0;
					}
					if (this.multilineInt)
					{
						throw new JScriptException(JSError.RegExpSyntax);
					}
					this.multilineInt = true;
					regexOptions |= RegexOptions.Multiline;
					break;
				}
				i++;
				continue;
				IL_00B0:
				throw new JScriptException(JSError.RegExpSyntax);
			}
			try
			{
				this.regex = new Regex(source, regexOptions);
			}
			catch (ArgumentException)
			{
				throw new JScriptException(JSError.RegExpSyntax);
			}
			return this;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000557C8 File Offset: 0x000547C8
		internal object exec(string input)
		{
			Match match = null;
			if (!this.globalInt)
			{
				match = this.regex.Match(input);
			}
			else
			{
				int num = (int)Runtime.DoubleToInt64(Convert.ToInteger(this.lastIndexInt));
				if (num <= 0)
				{
					match = this.regex.Match(input);
				}
				else if (num <= input.Length)
				{
					match = this.regex.Match(input, num);
				}
			}
			if (match == null || !match.Success)
			{
				this.lastIndexInt = 0;
				return DBNull.Value;
			}
			this.lastIndexInt = this.regExpConst.UpdateConstructor(this.regex, match, input);
			return new RegExpMatch(this.regExpConst.arrayPrototype, this.regex, match, input);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0005587D File Offset: 0x0005487D
		internal override string GetClassName()
		{
			return "RegExp";
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00055884 File Offset: 0x00054884
		internal bool test(string input)
		{
			Match match = null;
			if (!this.globalInt)
			{
				match = this.regex.Match(input);
			}
			else
			{
				int num = (int)Runtime.DoubleToInt64(Convert.ToInteger(this.lastIndexInt));
				if (num <= 0)
				{
					match = this.regex.Match(input);
				}
				else if (num <= input.Length)
				{
					match = this.regex.Match(input, num);
				}
			}
			if (match == null || !match.Success)
			{
				this.lastIndexInt = 0;
				return false;
			}
			this.lastIndexInt = this.regExpConst.UpdateConstructor(this.regex, match, input);
			return true;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00055920 File Offset: 0x00054920
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"/",
				this.sourceInt,
				"/",
				this.ignoreCaseInt ? "i" : "",
				this.globalInt ? "g" : "",
				this.multilineInt ? "m" : ""
			});
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00055998 File Offset: 0x00054998
		public string source
		{
			get
			{
				return this.sourceInt;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x000559A0 File Offset: 0x000549A0
		public bool ignoreCase
		{
			get
			{
				return this.ignoreCaseInt;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x000559A8 File Offset: 0x000549A8
		public bool global
		{
			get
			{
				return this.globalInt;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x000559B0 File Offset: 0x000549B0
		public bool multiline
		{
			get
			{
				return this.multilineInt;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x000559B8 File Offset: 0x000549B8
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x000559C0 File Offset: 0x000549C0
		public object lastIndex
		{
			get
			{
				return this.lastIndexInt;
			}
			set
			{
				this.lastIndexInt = value;
			}
		}

		// Token: 0x040006C1 RID: 1729
		internal RegExpConstructor regExpConst;

		// Token: 0x040006C2 RID: 1730
		private string sourceInt;

		// Token: 0x040006C3 RID: 1731
		internal bool ignoreCaseInt;

		// Token: 0x040006C4 RID: 1732
		internal bool globalInt;

		// Token: 0x040006C5 RID: 1733
		internal bool multilineInt;

		// Token: 0x040006C6 RID: 1734
		internal Regex regex;

		// Token: 0x040006C7 RID: 1735
		internal object lastIndexInt;
	}
}

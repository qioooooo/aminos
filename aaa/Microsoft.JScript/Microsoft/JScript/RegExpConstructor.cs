using System;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x02000104 RID: 260
	public sealed class RegExpConstructor : ScriptFunction
	{
		// Token: 0x06000AFF RID: 2815 RVA: 0x00054810 File Offset: 0x00053810
		internal RegExpConstructor()
			: base(FunctionPrototype.ob, "RegExp", 2)
		{
			this.originalPrototype = RegExpPrototype.ob;
			RegExpPrototype._constructor = this;
			this.proto = RegExpPrototype.ob;
			this.arrayPrototype = ArrayPrototype.ob;
			this.regex = null;
			this.lastRegexMatch = null;
			this.inputString = "";
			this.lastInput = null;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00054878 File Offset: 0x00053878
		internal RegExpConstructor(LenientFunctionPrototype parent, LenientRegExpPrototype prototypeProp, LenientArrayPrototype arrayPrototype)
			: base(parent, "RegExp", 2)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.arrayPrototype = arrayPrototype;
			this.regex = null;
			this.lastRegexMatch = null;
			this.inputString = "";
			this.lastInput = null;
			this.noExpando = false;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000548D5 File Offset: 0x000538D5
		internal override object Call(object[] args, object thisob)
		{
			return this.Invoke(args);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000548DE File Offset: 0x000538DE
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000548E8 File Offset: 0x000538E8
		private RegExpObject ConstructNew(object[] args)
		{
			string text = ((args.Length > 0 && args[0] != null) ? Convert.ToString(args[0]) : "");
			if (args.Length > 0 && args[0] is Regex)
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (args.Length >= 2 && args[1] != null)
			{
				string text2 = Convert.ToString(args[1]);
				int i = 0;
				while (i < text2.Length)
				{
					char c = text2[i];
					switch (c)
					{
					case 'g':
						flag2 = true;
						break;
					case 'h':
						goto IL_0088;
					case 'i':
						flag = true;
						break;
					default:
						if (c != 'm')
						{
							goto IL_0088;
						}
						flag3 = true;
						break;
					}
					i++;
					continue;
					IL_0088:
					throw new JScriptException(JSError.RegExpSyntax);
				}
			}
			return new RegExpObject(this.originalPrototype, text, flag, flag2, flag3, this);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000549A9 File Offset: 0x000539A9
		public object Construct(string pattern, bool ignoreCase, bool global, bool multiline)
		{
			return new RegExpObject(this.originalPrototype, pattern, ignoreCase, global, multiline, this);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000549BC File Offset: 0x000539BC
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new RegExpObject CreateInstance(params object[] args)
		{
			RegExpObject regExpObject;
			if (args == null || args.Length <= 0 || (regExpObject = args[0] as RegExpObject) == null)
			{
				return this.ConstructNew(args);
			}
			if (args.Length > 1 && args[1] != null)
			{
				throw new JScriptException(JSError.RegExpSyntax);
			}
			return new RegExpObject(this.originalPrototype, regExpObject.source, regExpObject.ignoreCase, regExpObject.global, regExpObject.multiline, this);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00054A20 File Offset: 0x00053A20
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public RegExpObject Invoke(params object[] args)
		{
			RegExpObject regExpObject;
			if (args == null || args.Length <= 0 || (regExpObject = args[0] as RegExpObject) == null)
			{
				return this.ConstructNew(args);
			}
			if (args.Length > 1 && args[1] != null)
			{
				throw new JScriptException(JSError.RegExpSyntax);
			}
			return regExpObject;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00054A61 File Offset: 0x00053A61
		private object GetIndex()
		{
			return (this.lastRegexMatch == null) ? (-1) : this.lastRegexMatch.Index;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00054A7E File Offset: 0x00053A7E
		private object GetInput()
		{
			return this.inputString;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00054A88 File Offset: 0x00053A88
		private object GetLastIndex()
		{
			return (this.lastRegexMatch == null) ? (-1) : ((this.lastRegexMatch.Length == 0) ? (this.lastRegexMatch.Index + 1) : (this.lastRegexMatch.Index + this.lastRegexMatch.Length));
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00054AD8 File Offset: 0x00053AD8
		private object GetLastMatch()
		{
			if (this.lastRegexMatch != null)
			{
				return this.lastRegexMatch.ToString();
			}
			return "";
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00054AF4 File Offset: 0x00053AF4
		private object GetLastParen()
		{
			if (this.regex == null || this.lastRegexMatch == null)
			{
				return "";
			}
			string[] groupNames = this.regex.GetGroupNames();
			if (groupNames.Length <= 1)
			{
				return "";
			}
			int num = this.regex.GroupNumberFromName(groupNames[groupNames.Length - 1]);
			Group group = this.lastRegexMatch.Groups[num];
			if (!group.Success)
			{
				return "";
			}
			return group.ToString();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00054B68 File Offset: 0x00053B68
		private object GetLeftContext()
		{
			if (this.lastRegexMatch != null && this.lastInput != null)
			{
				return this.lastInput.Substring(0, this.lastRegexMatch.Index);
			}
			return "";
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00054B98 File Offset: 0x00053B98
		internal override object GetMemberValue(string name)
		{
			if (name.Length == 2 && name[0] == '$')
			{
				char c = name[1];
				char c2 = c;
				switch (c2)
				{
				case '&':
					return this.GetLastMatch();
				case '\'':
					return this.GetRightContext();
				case '(':
				case ')':
				case '*':
				case ',':
				case '-':
				case '.':
				case '/':
				case '0':
					break;
				case '+':
					return this.GetLastParen();
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				{
					if (this.lastRegexMatch == null)
					{
						return "";
					}
					Group group = this.lastRegexMatch.Groups[c.ToString()];
					if (!group.Success)
					{
						return "";
					}
					return group.ToString();
				}
				default:
					switch (c2)
					{
					case '_':
						return this.GetInput();
					case '`':
						return this.GetLeftContext();
					}
					break;
				}
			}
			return base.GetMemberValue(name);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00054C9A File Offset: 0x00053C9A
		private object GetRightContext()
		{
			if (this.lastRegexMatch != null && this.lastInput != null)
			{
				return this.lastInput.Substring(this.lastRegexMatch.Index + this.lastRegexMatch.Length);
			}
			return "";
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00054CD4 File Offset: 0x00053CD4
		private void SetInput(object value)
		{
			this.inputString = value;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00054CE0 File Offset: 0x00053CE0
		internal override void SetMemberValue(string name, object value)
		{
			if (this.noExpando)
			{
				throw new JScriptException(JSError.AssignmentToReadOnly);
			}
			if (name.Length == 2 && name[0] == '$')
			{
				char c = name[1];
				switch (c)
				{
				case '&':
				case '\'':
				case '+':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					break;
				case '(':
				case ')':
				case '*':
				case ',':
				case '-':
				case '.':
				case '/':
				case '0':
					goto IL_00A7;
				default:
					switch (c)
					{
					case '_':
						this.SetInput(value);
						return;
					case '`':
						break;
					default:
						goto IL_00A7;
					}
					break;
				}
				return;
			}
			IL_00A7:
			base.SetMemberValue(name, value);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00054D9C File Offset: 0x00053D9C
		internal int UpdateConstructor(Regex regex, Match match, string input)
		{
			if (!this.noExpando)
			{
				this.regex = regex;
				this.lastRegexMatch = match;
				this.inputString = input;
				this.lastInput = input;
			}
			if (match.Length != 0)
			{
				return match.Index + match.Length;
			}
			return match.Index + 1;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00054DEB File Offset: 0x00053DEB
		public object index
		{
			get
			{
				return this.GetIndex();
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00054DF3 File Offset: 0x00053DF3
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x00054DFB File Offset: 0x00053DFB
		public object input
		{
			get
			{
				return this.GetInput();
			}
			set
			{
				if (this.noExpando)
				{
					throw new JScriptException(JSError.AssignmentToReadOnly);
				}
				this.SetInput(value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00054E17 File Offset: 0x00053E17
		public object lastIndex
		{
			get
			{
				return this.GetLastIndex();
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00054E1F File Offset: 0x00053E1F
		public object lastMatch
		{
			get
			{
				return this.GetLastMatch();
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00054E27 File Offset: 0x00053E27
		public object lastParen
		{
			get
			{
				return this.GetLastParen();
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00054E2F File Offset: 0x00053E2F
		public object leftContext
		{
			get
			{
				return this.GetLeftContext();
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00054E37 File Offset: 0x00053E37
		public object rightContext
		{
			get
			{
				return this.GetRightContext();
			}
		}

		// Token: 0x040006B0 RID: 1712
		internal static readonly RegExpConstructor ob = new RegExpConstructor();

		// Token: 0x040006B1 RID: 1713
		private RegExpPrototype originalPrototype;

		// Token: 0x040006B2 RID: 1714
		internal ArrayPrototype arrayPrototype;

		// Token: 0x040006B3 RID: 1715
		private Regex regex;

		// Token: 0x040006B4 RID: 1716
		private Match lastRegexMatch;

		// Token: 0x040006B5 RID: 1717
		internal object inputString;

		// Token: 0x040006B6 RID: 1718
		private string lastInput;
	}
}

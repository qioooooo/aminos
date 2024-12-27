using System;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x02000109 RID: 265
	internal class ReplaceUsingFunction : RegExpReplace
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x000559D8 File Offset: 0x000549D8
		internal ReplaceUsingFunction(Regex regex, ScriptFunction function, string source)
		{
			this.function = function;
			this.cArgs = function.GetNumberOfFormalParameters();
			bool flag = function is Closure && ((Closure)function).func.hasArgumentsObject;
			this.groupNumbers = null;
			this.source = source;
			if (this.cArgs > 1 || flag)
			{
				string[] groupNames = regex.GetGroupNames();
				int num = groupNames.Length - 1;
				if (flag)
				{
					this.cArgs = num + 3;
				}
				if (num > 0)
				{
					if (num > this.cArgs - 1)
					{
						num = this.cArgs - 1;
					}
					this.groupNumbers = new int[num];
					for (int i = 0; i < num; i++)
					{
						this.groupNumbers[i] = regex.GroupNumberFromName(groupNames[i + 1]);
					}
				}
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00055A90 File Offset: 0x00054A90
		internal override string Evaluate(Match match)
		{
			this.lastMatch = match;
			object[] array = new object[this.cArgs];
			if (this.cArgs > 0)
			{
				array[0] = match.ToString();
				if (this.cArgs > 1)
				{
					int i = 1;
					if (this.groupNumbers != null)
					{
						while (i <= this.groupNumbers.Length)
						{
							Group group = match.Groups[this.groupNumbers[i - 1]];
							array[i] = (group.Success ? group.ToString() : null);
							i++;
						}
					}
					if (i < this.cArgs)
					{
						array[i++] = match.Index;
						if (i < this.cArgs)
						{
							array[i++] = this.source;
							while (i < this.cArgs)
							{
								array[i] = null;
								i++;
							}
						}
					}
				}
			}
			object obj = this.function.Call(array, null);
			return match.Result((obj is Empty) ? "" : Convert.ToString(obj));
		}

		// Token: 0x040006C9 RID: 1737
		private ScriptFunction function;

		// Token: 0x040006CA RID: 1738
		private int cArgs;

		// Token: 0x040006CB RID: 1739
		private int[] groupNumbers;

		// Token: 0x040006CC RID: 1740
		private string source;
	}
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x02000106 RID: 262
	public sealed class RegExpMatch : ArrayObject
	{
		// Token: 0x06000B21 RID: 2849 RVA: 0x00055190 File Offset: 0x00054190
		internal RegExpMatch(ArrayPrototype parent, Regex regex, Match match, string input)
			: base(parent, typeof(RegExpMatch))
		{
			this.hydrated = false;
			this.regex = regex;
			this.matches = null;
			this.match = match;
			base.SetMemberValue("input", input);
			base.SetMemberValue("index", match.Index);
			base.SetMemberValue("lastIndex", (match.Length == 0) ? (match.Index + 1) : (match.Index + match.Length));
			string[] groupNames = regex.GetGroupNames();
			int num = 0;
			for (int i = 1; i < groupNames.Length; i++)
			{
				string text = groupNames[i];
				int num2 = regex.GroupNumberFromName(text);
				if (text.Equals(num2.ToString(CultureInfo.InvariantCulture)))
				{
					if (num2 > num)
					{
						num = num2;
					}
				}
				else
				{
					Group group = match.Groups[text];
					base.SetMemberValue(text, group.Success ? group.ToString() : null);
				}
			}
			this.length = num + 1;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00055294 File Offset: 0x00054294
		internal RegExpMatch(ArrayPrototype parent, Regex regex, MatchCollection matches, string input)
			: base(parent)
		{
			this.hydrated = false;
			this.length = matches.Count;
			this.regex = regex;
			this.matches = matches;
			this.match = null;
			Match match = matches[matches.Count - 1];
			base.SetMemberValue("input", input);
			base.SetMemberValue("index", match.Index);
			base.SetMemberValue("lastIndex", (match.Length == 0) ? (match.Index + 1) : (match.Index + match.Length));
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00055336 File Offset: 0x00054336
		internal override void Concat(ArrayObject source)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.Concat(source);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0005534D File Offset: 0x0005434D
		internal override void Concat(object value)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.Concat(value);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00055364 File Offset: 0x00054364
		internal override bool DeleteValueAtIndex(uint index)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			return base.DeleteValueAtIndex(index);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0005537C File Offset: 0x0005437C
		internal override object GetValueAtIndex(uint index)
		{
			if (!this.hydrated)
			{
				if (this.matches != null)
				{
					if ((ulong)index < (ulong)((long)this.matches.Count))
					{
						return this.matches[(int)index].ToString();
					}
				}
				else if (this.match != null)
				{
					int num = this.regex.GroupNumberFromName(index.ToString(CultureInfo.InvariantCulture));
					if (num >= 0)
					{
						Group group = this.match.Groups[num];
						if (!group.Success)
						{
							return "";
						}
						return group.ToString();
					}
				}
			}
			return base.GetValueAtIndex(index);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00055410 File Offset: 0x00054410
		internal override object GetMemberValue(string name)
		{
			if (!this.hydrated)
			{
				long num = ArrayObject.Array_index_for(name);
				if (num >= 0L)
				{
					return this.GetValueAtIndex((uint)num);
				}
			}
			return base.GetMemberValue(name);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00055444 File Offset: 0x00054444
		private void Hydrate()
		{
			if (this.matches != null)
			{
				int i = 0;
				int count = this.matches.Count;
				while (i < count)
				{
					base.SetValueAtIndex((uint)i, this.matches[i].ToString());
					i++;
				}
			}
			else if (this.match != null)
			{
				string[] groupNames = this.regex.GetGroupNames();
				int j = 1;
				int num = groupNames.Length;
				while (j < num)
				{
					string text = groupNames[j];
					int num2 = this.regex.GroupNumberFromName(text);
					Group group = this.match.Groups[num2];
					object obj = (group.Success ? group.ToString() : "");
					if (text.Equals(num2.ToString(CultureInfo.InvariantCulture)))
					{
						base.SetValueAtIndex((uint)num2, obj);
					}
					j++;
				}
			}
			this.hydrated = true;
			this.regex = null;
			this.matches = null;
			this.match = null;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00055530 File Offset: 0x00054530
		internal override void SetValueAtIndex(uint index, object value)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.SetValueAtIndex(index, value);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00055548 File Offset: 0x00054548
		internal override object Shift()
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			return base.Shift();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0005555E File Offset: 0x0005455E
		internal override void Sort(ScriptFunction compareFn)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.Sort(compareFn);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00055575 File Offset: 0x00054575
		internal override void Splice(uint start, uint deleteItems, object[] args, ArrayObject outArray, uint oldLength, uint newLength)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.Splice(start, deleteItems, args, outArray, oldLength, newLength);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00055594 File Offset: 0x00054594
		internal override void SwapValues(uint pi, uint qi)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			base.SwapValues(pi, qi);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000555AC File Offset: 0x000545AC
		internal override ArrayObject Unshift(object[] args)
		{
			if (!this.hydrated)
			{
				this.Hydrate();
			}
			return base.Unshift(args);
		}

		// Token: 0x040006BD RID: 1725
		private bool hydrated;

		// Token: 0x040006BE RID: 1726
		private Regex regex;

		// Token: 0x040006BF RID: 1727
		private MatchCollection matches;

		// Token: 0x040006C0 RID: 1728
		private Match match;
	}
}

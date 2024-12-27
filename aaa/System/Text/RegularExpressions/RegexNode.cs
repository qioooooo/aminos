using System;
using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000029 RID: 41
	internal sealed class RegexNode
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x0000DADA File Offset: 0x0000CADA
		internal RegexNode(int type, RegexOptions options)
		{
			this._type = type;
			this._options = options;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000DAF0 File Offset: 0x0000CAF0
		internal RegexNode(int type, RegexOptions options, char ch)
		{
			this._type = type;
			this._options = options;
			this._ch = ch;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000DB0D File Offset: 0x0000CB0D
		internal RegexNode(int type, RegexOptions options, string str)
		{
			this._type = type;
			this._options = options;
			this._str = str;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000DB2A File Offset: 0x0000CB2A
		internal RegexNode(int type, RegexOptions options, int m)
		{
			this._type = type;
			this._options = options;
			this._m = m;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000DB47 File Offset: 0x0000CB47
		internal RegexNode(int type, RegexOptions options, int m, int n)
		{
			this._type = type;
			this._options = options;
			this._m = m;
			this._n = n;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000DB6C File Offset: 0x0000CB6C
		internal bool UseOptionR()
		{
			return (this._options & RegexOptions.RightToLeft) != RegexOptions.None;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000DB7D File Offset: 0x0000CB7D
		internal RegexNode ReverseLeft()
		{
			if (this.UseOptionR() && this._type == 25 && this._children != null)
			{
				this._children.Reverse(0, this._children.Count);
			}
			return this;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000DBB1 File Offset: 0x0000CBB1
		internal void MakeRep(int type, int min, int max)
		{
			this._type += type - 9;
			this._m = min;
			this._n = max;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000DBD4 File Offset: 0x0000CBD4
		internal RegexNode Reduce()
		{
			int num = this.Type();
			RegexNode regexNode;
			if (num != 5 && num != 11)
			{
				switch (num)
				{
				case 24:
					return this.ReduceAlternation();
				case 25:
					return this.ReduceConcatenation();
				case 26:
				case 27:
					return this.ReduceRep();
				case 29:
					return this.ReduceGroup();
				}
				regexNode = this;
			}
			else
			{
				regexNode = this.ReduceSet();
			}
			return regexNode;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000DC44 File Offset: 0x0000CC44
		internal RegexNode StripEnation(int emptyType)
		{
			switch (this.ChildCount())
			{
			case 0:
				return new RegexNode(emptyType, this._options);
			case 1:
				return this.Child(0);
			default:
				return this;
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000DC80 File Offset: 0x0000CC80
		internal RegexNode ReduceGroup()
		{
			RegexNode regexNode = this;
			while (regexNode.Type() == 29)
			{
				regexNode = regexNode.Child(0);
			}
			return regexNode;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000DCA4 File Offset: 0x0000CCA4
		internal RegexNode ReduceRep()
		{
			RegexNode regexNode = this;
			int num = this.Type();
			int num2 = this._m;
			int num3 = this._n;
			while (regexNode.ChildCount() != 0)
			{
				RegexNode regexNode2 = regexNode.Child(0);
				if (regexNode2.Type() != num)
				{
					int num4 = regexNode2.Type();
					if ((num4 < 3 || num4 > 5 || num != 26) && (num4 < 6 || num4 > 8 || num != 27))
					{
						break;
					}
				}
				if ((regexNode._m == 0 && regexNode2._m > 1) || regexNode2._n < regexNode2._m * 2)
				{
					break;
				}
				regexNode = regexNode2;
				if (regexNode._m > 0)
				{
					num2 = (regexNode._m = ((2147483646 / regexNode._m < num2) ? int.MaxValue : (regexNode._m * num2)));
				}
				if (regexNode._n > 0)
				{
					num3 = (regexNode._n = ((2147483646 / regexNode._n < num3) ? int.MaxValue : (regexNode._n * num3)));
				}
			}
			if (num2 != 2147483647)
			{
				return regexNode;
			}
			return new RegexNode(22, this._options);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000DDB8 File Offset: 0x0000CDB8
		internal RegexNode ReduceSet()
		{
			if (RegexCharClass.IsEmpty(this._str))
			{
				this._type = 22;
				this._str = null;
			}
			else if (RegexCharClass.IsSingleton(this._str))
			{
				this._ch = RegexCharClass.SingletonChar(this._str);
				this._str = null;
				this._type += -2;
			}
			else if (RegexCharClass.IsSingletonInverse(this._str))
			{
				this._ch = RegexCharClass.SingletonChar(this._str);
				this._str = null;
				this._type += -1;
			}
			return this;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000DE50 File Offset: 0x0000CE50
		internal RegexNode ReduceAlternation()
		{
			if (this._children == null)
			{
				return new RegexNode(22, this._options);
			}
			bool flag = false;
			bool flag2 = false;
			RegexOptions regexOptions = RegexOptions.None;
			int i = 0;
			int num = 0;
			while (i < this._children.Count)
			{
				RegexNode regexNode = (RegexNode)this._children[i];
				if (num < i)
				{
					this._children[num] = regexNode;
				}
				if (regexNode._type == 24)
				{
					for (int j = 0; j < regexNode._children.Count; j++)
					{
						((RegexNode)regexNode._children[j])._next = this;
					}
					this._children.InsertRange(i + 1, regexNode._children);
					num--;
				}
				else if (regexNode._type == 11 || regexNode._type == 9)
				{
					RegexOptions regexOptions2 = regexNode._options & (RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
					if (regexNode._type == 11)
					{
						if (!flag || regexOptions != regexOptions2 || flag2 || !RegexCharClass.IsMergeable(regexNode._str))
						{
							flag = true;
							flag2 = !RegexCharClass.IsMergeable(regexNode._str);
							regexOptions = regexOptions2;
							goto IL_01D1;
						}
					}
					else if (!flag || regexOptions != regexOptions2 || flag2)
					{
						flag = true;
						flag2 = false;
						regexOptions = regexOptions2;
						goto IL_01D1;
					}
					num--;
					RegexNode regexNode2 = (RegexNode)this._children[num];
					RegexCharClass regexCharClass;
					if (regexNode2._type == 9)
					{
						regexCharClass = new RegexCharClass();
						regexCharClass.AddChar(regexNode2._ch);
					}
					else
					{
						regexCharClass = RegexCharClass.Parse(regexNode2._str);
					}
					if (regexNode._type == 9)
					{
						regexCharClass.AddChar(regexNode._ch);
					}
					else
					{
						RegexCharClass regexCharClass2 = RegexCharClass.Parse(regexNode._str);
						regexCharClass.AddCharClass(regexCharClass2);
					}
					regexNode2._type = 11;
					regexNode2._str = regexCharClass.ToStringClass();
				}
				else if (regexNode._type == 22)
				{
					num--;
				}
				else
				{
					flag = false;
					flag2 = false;
				}
				IL_01D1:
				i++;
				num++;
			}
			if (num < i)
			{
				this._children.RemoveRange(num, i - num);
			}
			return this.StripEnation(22);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E06C File Offset: 0x0000D06C
		internal RegexNode ReduceConcatenation()
		{
			if (this._children == null)
			{
				return new RegexNode(23, this._options);
			}
			bool flag = false;
			RegexOptions regexOptions = RegexOptions.None;
			int i = 0;
			int num = 0;
			while (i < this._children.Count)
			{
				RegexNode regexNode = (RegexNode)this._children[i];
				if (num < i)
				{
					this._children[num] = regexNode;
				}
				if (regexNode._type == 25 && (regexNode._options & RegexOptions.RightToLeft) == (this._options & RegexOptions.RightToLeft))
				{
					for (int j = 0; j < regexNode._children.Count; j++)
					{
						((RegexNode)regexNode._children[j])._next = this;
					}
					this._children.InsertRange(i + 1, regexNode._children);
					num--;
				}
				else if (regexNode._type == 12 || regexNode._type == 9)
				{
					RegexOptions regexOptions2 = regexNode._options & (RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
					if (!flag || regexOptions != regexOptions2)
					{
						flag = true;
						regexOptions = regexOptions2;
					}
					else
					{
						RegexNode regexNode2 = (RegexNode)this._children[--num];
						if (regexNode2._type == 9)
						{
							regexNode2._type = 12;
							regexNode2._str = Convert.ToString(regexNode2._ch, CultureInfo.InvariantCulture);
						}
						if ((regexOptions2 & RegexOptions.RightToLeft) == RegexOptions.None)
						{
							if (regexNode._type == 9)
							{
								RegexNode regexNode3 = regexNode2;
								regexNode3._str += regexNode._ch.ToString();
							}
							else
							{
								RegexNode regexNode4 = regexNode2;
								regexNode4._str += regexNode._str;
							}
						}
						else if (regexNode._type == 9)
						{
							regexNode2._str = regexNode._ch.ToString() + regexNode2._str;
						}
						else
						{
							regexNode2._str = regexNode._str + regexNode2._str;
						}
					}
				}
				else if (regexNode._type == 23)
				{
					num--;
				}
				else
				{
					flag = false;
				}
				i++;
				num++;
			}
			if (num < i)
			{
				this._children.RemoveRange(num, i - num);
			}
			return this.StripEnation(23);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E294 File Offset: 0x0000D294
		internal RegexNode MakeQuantifier(bool lazy, int min, int max)
		{
			if (min == 0 && max == 0)
			{
				return new RegexNode(23, this._options);
			}
			if (min == 1 && max == 1)
			{
				return this;
			}
			switch (this._type)
			{
			case 9:
			case 10:
			case 11:
				this.MakeRep(lazy ? 6 : 3, min, max);
				return this;
			default:
			{
				RegexNode regexNode = new RegexNode(lazy ? 27 : 26, this._options, min, max);
				regexNode.AddChild(this);
				return regexNode;
			}
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E310 File Offset: 0x0000D310
		internal void AddChild(RegexNode newChild)
		{
			if (this._children == null)
			{
				this._children = new ArrayList(4);
			}
			RegexNode regexNode = newChild.Reduce();
			this._children.Add(regexNode);
			regexNode._next = this;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E34C File Offset: 0x0000D34C
		internal RegexNode Child(int i)
		{
			return (RegexNode)this._children[i];
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000E35F File Offset: 0x0000D35F
		internal int ChildCount()
		{
			if (this._children != null)
			{
				return this._children.Count;
			}
			return 0;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E376 File Offset: 0x0000D376
		internal int Type()
		{
			return this._type;
		}

		// Token: 0x04000763 RID: 1891
		internal const int Oneloop = 3;

		// Token: 0x04000764 RID: 1892
		internal const int Notoneloop = 4;

		// Token: 0x04000765 RID: 1893
		internal const int Setloop = 5;

		// Token: 0x04000766 RID: 1894
		internal const int Onelazy = 6;

		// Token: 0x04000767 RID: 1895
		internal const int Notonelazy = 7;

		// Token: 0x04000768 RID: 1896
		internal const int Setlazy = 8;

		// Token: 0x04000769 RID: 1897
		internal const int One = 9;

		// Token: 0x0400076A RID: 1898
		internal const int Notone = 10;

		// Token: 0x0400076B RID: 1899
		internal const int Set = 11;

		// Token: 0x0400076C RID: 1900
		internal const int Multi = 12;

		// Token: 0x0400076D RID: 1901
		internal const int Ref = 13;

		// Token: 0x0400076E RID: 1902
		internal const int Bol = 14;

		// Token: 0x0400076F RID: 1903
		internal const int Eol = 15;

		// Token: 0x04000770 RID: 1904
		internal const int Boundary = 16;

		// Token: 0x04000771 RID: 1905
		internal const int Nonboundary = 17;

		// Token: 0x04000772 RID: 1906
		internal const int ECMABoundary = 41;

		// Token: 0x04000773 RID: 1907
		internal const int NonECMABoundary = 42;

		// Token: 0x04000774 RID: 1908
		internal const int Beginning = 18;

		// Token: 0x04000775 RID: 1909
		internal const int Start = 19;

		// Token: 0x04000776 RID: 1910
		internal const int EndZ = 20;

		// Token: 0x04000777 RID: 1911
		internal const int End = 21;

		// Token: 0x04000778 RID: 1912
		internal const int Nothing = 22;

		// Token: 0x04000779 RID: 1913
		internal const int Empty = 23;

		// Token: 0x0400077A RID: 1914
		internal const int Alternate = 24;

		// Token: 0x0400077B RID: 1915
		internal const int Concatenate = 25;

		// Token: 0x0400077C RID: 1916
		internal const int Loop = 26;

		// Token: 0x0400077D RID: 1917
		internal const int Lazyloop = 27;

		// Token: 0x0400077E RID: 1918
		internal const int Capture = 28;

		// Token: 0x0400077F RID: 1919
		internal const int Group = 29;

		// Token: 0x04000780 RID: 1920
		internal const int Require = 30;

		// Token: 0x04000781 RID: 1921
		internal const int Prevent = 31;

		// Token: 0x04000782 RID: 1922
		internal const int Greedy = 32;

		// Token: 0x04000783 RID: 1923
		internal const int Testref = 33;

		// Token: 0x04000784 RID: 1924
		internal const int Testgroup = 34;

		// Token: 0x04000785 RID: 1925
		internal int _type;

		// Token: 0x04000786 RID: 1926
		internal ArrayList _children;

		// Token: 0x04000787 RID: 1927
		internal string _str;

		// Token: 0x04000788 RID: 1928
		internal char _ch;

		// Token: 0x04000789 RID: 1929
		internal int _m;

		// Token: 0x0400078A RID: 1930
		internal int _n;

		// Token: 0x0400078B RID: 1931
		internal RegexOptions _options;

		// Token: 0x0400078C RID: 1932
		internal RegexNode _next;
	}
}

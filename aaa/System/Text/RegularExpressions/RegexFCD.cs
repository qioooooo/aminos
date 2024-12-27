using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200001D RID: 29
	internal sealed class RegexFCD
	{
		// Token: 0x06000135 RID: 309 RVA: 0x0000A830 File Offset: 0x00009830
		internal static RegexPrefix FirstChars(RegexTree t)
		{
			RegexFCD regexFCD = new RegexFCD();
			RegexFC regexFC = regexFCD.RegexFCFromRegexTree(t);
			if (regexFC == null || regexFC._nullable)
			{
				return null;
			}
			CultureInfo cultureInfo = (((t._options & RegexOptions.CultureInvariant) != RegexOptions.None) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			return new RegexPrefix(regexFC.GetFirstChars(cultureInfo), regexFC.IsCaseInsensitive());
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000A888 File Offset: 0x00009888
		internal static RegexPrefix Prefix(RegexTree tree)
		{
			RegexNode regexNode = null;
			int num = 0;
			RegexNode regexNode2 = tree._root;
			for (;;)
			{
				int type = regexNode2._type;
				switch (type)
				{
				case 3:
				case 6:
					goto IL_00C6;
				case 4:
				case 5:
				case 7:
				case 8:
				case 10:
				case 11:
				case 13:
				case 17:
				case 22:
				case 24:
				case 26:
				case 27:
				case 29:
					goto IL_013F;
				case 9:
					goto IL_0101;
				case 12:
					goto IL_0125;
				case 14:
				case 15:
				case 16:
				case 18:
				case 19:
				case 20:
				case 21:
				case 23:
				case 30:
				case 31:
					break;
				case 25:
					if (regexNode2.ChildCount() > 0)
					{
						regexNode = regexNode2;
						num = 0;
					}
					break;
				case 28:
				case 32:
					regexNode2 = regexNode2.Child(0);
					regexNode = null;
					continue;
				default:
					if (type != 41)
					{
						goto Block_2;
					}
					break;
				}
				if (regexNode == null || num >= regexNode.ChildCount())
				{
					goto IL_0151;
				}
				regexNode2 = regexNode.Child(num++);
			}
			Block_2:
			goto IL_013F;
			IL_00C6:
			if (regexNode2._m > 0)
			{
				string text = string.Empty.PadRight(regexNode2._m, regexNode2._ch);
				return new RegexPrefix(text, RegexOptions.None != (regexNode2._options & RegexOptions.IgnoreCase));
			}
			return RegexPrefix.Empty;
			IL_0101:
			return new RegexPrefix(regexNode2._ch.ToString(CultureInfo.InvariantCulture), RegexOptions.None != (regexNode2._options & RegexOptions.IgnoreCase));
			IL_0125:
			return new RegexPrefix(regexNode2._str, RegexOptions.None != (regexNode2._options & RegexOptions.IgnoreCase));
			IL_013F:
			return RegexPrefix.Empty;
			IL_0151:
			return RegexPrefix.Empty;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000A9FC File Offset: 0x000099FC
		internal static int Anchors(RegexTree tree)
		{
			RegexNode regexNode = null;
			int num = 0;
			int num2 = 0;
			RegexNode regexNode2 = tree._root;
			int type;
			for (;;)
			{
				type = regexNode2._type;
				switch (type)
				{
				case 14:
				case 15:
				case 16:
				case 18:
				case 19:
				case 20:
				case 21:
					goto IL_0091;
				case 17:
				case 22:
				case 24:
				case 26:
				case 27:
				case 29:
					return num2;
				case 23:
				case 30:
				case 31:
					goto IL_00A1;
				case 25:
					if (regexNode2.ChildCount() > 0)
					{
						regexNode = regexNode2;
						num = 0;
						goto IL_00A1;
					}
					goto IL_00A1;
				case 28:
				case 32:
					regexNode2 = regexNode2.Child(0);
					regexNode = null;
					continue;
				}
				break;
				IL_00A1:
				if (regexNode == null || num >= regexNode.ChildCount())
				{
					return num2;
				}
				regexNode2 = regexNode.Child(num++);
			}
			if (type != 41)
			{
				return num2;
			}
			IL_0091:
			return num2 | RegexFCD.AnchorFromType(regexNode2._type);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000AAC8 File Offset: 0x00009AC8
		private static int AnchorFromType(int type)
		{
			switch (type)
			{
			case 14:
				return 2;
			case 15:
				return 8;
			case 16:
				return 64;
			case 17:
				break;
			case 18:
				return 1;
			case 19:
				return 4;
			case 20:
				return 16;
			case 21:
				return 32;
			default:
				if (type == 41)
				{
					return 128;
				}
				break;
			}
			return 0;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000AB1F File Offset: 0x00009B1F
		private RegexFCD()
		{
			this._fcStack = new RegexFC[32];
			this._intStack = new int[32];
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000AB44 File Offset: 0x00009B44
		private void PushInt(int I)
		{
			if (this._intDepth >= this._intStack.Length)
			{
				int[] array = new int[this._intDepth * 2];
				Array.Copy(this._intStack, 0, array, 0, this._intDepth);
				this._intStack = array;
			}
			this._intStack[this._intDepth++] = I;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000ABA3 File Offset: 0x00009BA3
		private bool IntIsEmpty()
		{
			return this._intDepth == 0;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000ABB0 File Offset: 0x00009BB0
		private int PopInt()
		{
			return this._intStack[--this._intDepth];
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000ABD8 File Offset: 0x00009BD8
		private void PushFC(RegexFC fc)
		{
			if (this._fcDepth >= this._fcStack.Length)
			{
				RegexFC[] array = new RegexFC[this._fcDepth * 2];
				Array.Copy(this._fcStack, 0, array, 0, this._fcDepth);
				this._fcStack = array;
			}
			this._fcStack[this._fcDepth++] = fc;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000AC37 File Offset: 0x00009C37
		private bool FCIsEmpty()
		{
			return this._fcDepth == 0;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000AC44 File Offset: 0x00009C44
		private RegexFC PopFC()
		{
			return this._fcStack[--this._fcDepth];
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000AC69 File Offset: 0x00009C69
		private RegexFC TopFC()
		{
			return this._fcStack[this._fcDepth - 1];
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000AC7C File Offset: 0x00009C7C
		private RegexFC RegexFCFromRegexTree(RegexTree tree)
		{
			RegexNode regexNode = tree._root;
			int num = 0;
			for (;;)
			{
				if (regexNode._children == null)
				{
					this.CalculateFC(regexNode._type, regexNode, 0);
				}
				else if (num < regexNode._children.Count && !this._skipAllChildren)
				{
					this.CalculateFC(regexNode._type | 64, regexNode, num);
					if (!this._skipchild)
					{
						regexNode = (RegexNode)regexNode._children[num];
						this.PushInt(num);
						num = 0;
						continue;
					}
					num++;
					this._skipchild = false;
					continue;
				}
				this._skipAllChildren = false;
				if (this.IntIsEmpty())
				{
					goto IL_00BE;
				}
				num = this.PopInt();
				regexNode = regexNode._next;
				this.CalculateFC(regexNode._type | 128, regexNode, num);
				if (this._failed)
				{
					break;
				}
				num++;
			}
			return null;
			IL_00BE:
			if (this.FCIsEmpty())
			{
				return null;
			}
			return this.PopFC();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000AD57 File Offset: 0x00009D57
		private void SkipChild()
		{
			this._skipchild = true;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000AD60 File Offset: 0x00009D60
		private void CalculateFC(int NodeType, RegexNode node, int CurIndex)
		{
			bool flag = false;
			bool flag2 = false;
			if (NodeType <= 13)
			{
				if ((node._options & RegexOptions.IgnoreCase) != RegexOptions.None)
				{
					flag = true;
				}
				if ((node._options & RegexOptions.RightToLeft) != RegexOptions.None)
				{
					flag2 = true;
				}
			}
			int num = NodeType;
			switch (num)
			{
			case 3:
			case 6:
				this.PushFC(new RegexFC(node._ch, false, node._m == 0, flag));
				return;
			case 4:
			case 7:
				this.PushFC(new RegexFC(node._ch, true, node._m == 0, flag));
				return;
			case 5:
			case 8:
				this.PushFC(new RegexFC(node._str, node._m == 0, flag));
				return;
			case 9:
			case 10:
				this.PushFC(new RegexFC(node._ch, NodeType == 10, false, flag));
				return;
			case 11:
				this.PushFC(new RegexFC(node._str, false, flag));
				return;
			case 12:
				if (node._str.Length == 0)
				{
					this.PushFC(new RegexFC(true));
					return;
				}
				if (!flag2)
				{
					this.PushFC(new RegexFC(node._str[0], false, false, flag));
					return;
				}
				this.PushFC(new RegexFC(node._str[node._str.Length - 1], false, false, flag));
				return;
			case 13:
				this.PushFC(new RegexFC("\0\u0001\0\0", true, false));
				return;
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 41:
			case 42:
				this.PushFC(new RegexFC(true));
				return;
			case 23:
				this.PushFC(new RegexFC(true));
				return;
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
				break;
			default:
				switch (num)
				{
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 96:
				case 97:
					break;
				case 94:
				case 95:
					this.SkipChild();
					this.PushFC(new RegexFC(true));
					return;
				case 98:
					if (CurIndex == 0)
					{
						this.SkipChild();
						return;
					}
					break;
				default:
					switch (num)
					{
					case 152:
					case 161:
						if (CurIndex != 0)
						{
							RegexFC regexFC = this.PopFC();
							RegexFC regexFC2 = this.TopFC();
							this._failed = !regexFC2.AddFC(regexFC, false);
							return;
						}
						break;
					case 153:
						if (CurIndex != 0)
						{
							RegexFC regexFC3 = this.PopFC();
							RegexFC regexFC4 = this.TopFC();
							this._failed = !regexFC4.AddFC(regexFC3, true);
						}
						if (!this.TopFC()._nullable)
						{
							this._skipAllChildren = true;
							return;
						}
						break;
					case 154:
					case 155:
						if (node._m == 0)
						{
							this.TopFC()._nullable = true;
							return;
						}
						break;
					case 156:
					case 157:
					case 158:
					case 159:
					case 160:
						break;
					case 162:
						if (CurIndex > 1)
						{
							RegexFC regexFC5 = this.PopFC();
							RegexFC regexFC6 = this.TopFC();
							this._failed = !regexFC6.AddFC(regexFC5, false);
							return;
						}
						break;
					default:
						goto IL_0318;
					}
					break;
				}
				return;
			}
			IL_0318:
			throw new ArgumentException(SR.GetString("UnexpectedOpcode", new object[] { NodeType.ToString(CultureInfo.CurrentCulture) }));
		}

		// Token: 0x04000712 RID: 1810
		private const int BeforeChild = 64;

		// Token: 0x04000713 RID: 1811
		private const int AfterChild = 128;

		// Token: 0x04000714 RID: 1812
		internal const int Beginning = 1;

		// Token: 0x04000715 RID: 1813
		internal const int Bol = 2;

		// Token: 0x04000716 RID: 1814
		internal const int Start = 4;

		// Token: 0x04000717 RID: 1815
		internal const int Eol = 8;

		// Token: 0x04000718 RID: 1816
		internal const int EndZ = 16;

		// Token: 0x04000719 RID: 1817
		internal const int End = 32;

		// Token: 0x0400071A RID: 1818
		internal const int Boundary = 64;

		// Token: 0x0400071B RID: 1819
		internal const int ECMABoundary = 128;

		// Token: 0x0400071C RID: 1820
		private int[] _intStack;

		// Token: 0x0400071D RID: 1821
		private int _intDepth;

		// Token: 0x0400071E RID: 1822
		private RegexFC[] _fcStack;

		// Token: 0x0400071F RID: 1823
		private int _fcDepth;

		// Token: 0x04000720 RID: 1824
		private bool _skipAllChildren;

		// Token: 0x04000721 RID: 1825
		private bool _skipchild;

		// Token: 0x04000722 RID: 1826
		private bool _failed;
	}
}

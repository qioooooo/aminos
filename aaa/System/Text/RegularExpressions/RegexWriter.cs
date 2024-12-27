using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002F RID: 47
	internal sealed class RegexWriter
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00011794 File Offset: 0x00010794
		internal static RegexCode Write(RegexTree t)
		{
			RegexWriter regexWriter = new RegexWriter();
			return regexWriter.RegexCodeFromRegexTree(t);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000117B0 File Offset: 0x000107B0
		private RegexWriter()
		{
			this._intStack = new int[32];
			this._emitted = new int[32];
			this._stringhash = new HybridDictionary();
			this._stringtable = new ArrayList();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000117E8 File Offset: 0x000107E8
		internal void PushInt(int I)
		{
			if (this._depth >= this._intStack.Length)
			{
				int[] array = new int[this._depth * 2];
				Array.Copy(this._intStack, 0, array, 0, this._depth);
				this._intStack = array;
			}
			this._intStack[this._depth++] = I;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00011847 File Offset: 0x00010847
		internal bool EmptyStack()
		{
			return this._depth == 0;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00011854 File Offset: 0x00010854
		internal int PopInt()
		{
			return this._intStack[--this._depth];
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00011879 File Offset: 0x00010879
		internal int CurPos()
		{
			return this._curpos;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00011881 File Offset: 0x00010881
		internal void PatchJump(int Offset, int jumpDest)
		{
			this._emitted[Offset + 1] = jumpDest;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00011890 File Offset: 0x00010890
		internal void Emit(int op)
		{
			if (this._counting)
			{
				this._count++;
				if (RegexCode.OpcodeBacktracks(op))
				{
					this._trackcount++;
				}
				return;
			}
			this._emitted[this._curpos++] = op;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000118E4 File Offset: 0x000108E4
		internal void Emit(int op, int opd1)
		{
			if (this._counting)
			{
				this._count += 2;
				if (RegexCode.OpcodeBacktracks(op))
				{
					this._trackcount++;
				}
				return;
			}
			this._emitted[this._curpos++] = op;
			this._emitted[this._curpos++] = opd1;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00011950 File Offset: 0x00010950
		internal void Emit(int op, int opd1, int opd2)
		{
			if (this._counting)
			{
				this._count += 3;
				if (RegexCode.OpcodeBacktracks(op))
				{
					this._trackcount++;
				}
				return;
			}
			this._emitted[this._curpos++] = op;
			this._emitted[this._curpos++] = opd1;
			this._emitted[this._curpos++] = opd2;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000119D8 File Offset: 0x000109D8
		internal int StringCode(string str)
		{
			if (this._counting)
			{
				return 0;
			}
			if (str == null)
			{
				str = string.Empty;
			}
			int num;
			if (this._stringhash.Contains(str))
			{
				num = (int)this._stringhash[str];
			}
			else
			{
				num = this._stringtable.Count;
				this._stringhash[str] = num;
				this._stringtable.Add(str);
			}
			return num;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00011A47 File Offset: 0x00010A47
		internal ArgumentException MakeException(string message)
		{
			return new ArgumentException(message);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00011A4F File Offset: 0x00010A4F
		internal int MapCapnum(int capnum)
		{
			if (capnum == -1)
			{
				return -1;
			}
			if (this._caps != null)
			{
				return (int)this._caps[capnum];
			}
			return capnum;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00011A78 File Offset: 0x00010A78
		internal RegexCode RegexCodeFromRegexTree(RegexTree tree)
		{
			int num;
			if (tree._capnumlist == null || tree._captop == tree._capnumlist.Length)
			{
				num = tree._captop;
				this._caps = null;
			}
			else
			{
				num = tree._capnumlist.Length;
				this._caps = tree._caps;
				for (int i = 0; i < tree._capnumlist.Length; i++)
				{
					this._caps[tree._capnumlist[i]] = i;
				}
			}
			this._counting = true;
			for (;;)
			{
				if (!this._counting)
				{
					this._emitted = new int[this._count];
				}
				RegexNode regexNode = tree._root;
				int num2 = 0;
				this.Emit(23, 0);
				for (;;)
				{
					if (regexNode._children == null)
					{
						this.EmitFragment(regexNode._type, regexNode, 0);
					}
					else if (num2 < regexNode._children.Count)
					{
						this.EmitFragment(regexNode._type | 64, regexNode, num2);
						regexNode = (RegexNode)regexNode._children[num2];
						this.PushInt(num2);
						num2 = 0;
						continue;
					}
					if (this.EmptyStack())
					{
						break;
					}
					num2 = this.PopInt();
					regexNode = regexNode._next;
					this.EmitFragment(regexNode._type | 128, regexNode, num2);
					num2++;
				}
				this.PatchJump(0, this.CurPos());
				this.Emit(40);
				if (!this._counting)
				{
					break;
				}
				this._counting = false;
			}
			RegexPrefix regexPrefix = RegexFCD.FirstChars(tree);
			RegexPrefix regexPrefix2 = RegexFCD.Prefix(tree);
			bool flag = (tree._options & RegexOptions.RightToLeft) != RegexOptions.None;
			CultureInfo cultureInfo = (((tree._options & RegexOptions.CultureInvariant) != RegexOptions.None) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			RegexBoyerMoore regexBoyerMoore;
			if (regexPrefix2 != null && regexPrefix2.Prefix.Length > 0)
			{
				regexBoyerMoore = new RegexBoyerMoore(regexPrefix2.Prefix, regexPrefix2.CaseInsensitive, flag, cultureInfo);
			}
			else
			{
				regexBoyerMoore = null;
			}
			int num3 = RegexFCD.Anchors(tree);
			return new RegexCode(this._emitted, this._stringtable, this._trackcount, this._caps, num, regexBoyerMoore, regexPrefix, num3, flag);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00011C70 File Offset: 0x00010C70
		internal void EmitFragment(int nodetype, RegexNode node, int CurIndex)
		{
			int num = 0;
			if (nodetype <= 13)
			{
				if (node.UseOptionR())
				{
					num |= 64;
				}
				if ((node._options & RegexOptions.IgnoreCase) != RegexOptions.None)
				{
					num |= 512;
				}
			}
			int num2 = nodetype;
			switch (num2)
			{
			case 3:
			case 4:
			case 6:
			case 7:
				if (node._m > 0)
				{
					this.Emit(((node._type == 3 || node._type == 6) ? 0 : 1) | num, (int)node._ch, node._m);
				}
				if (node._n > node._m)
				{
					this.Emit(node._type | num, (int)node._ch, (node._n == int.MaxValue) ? int.MaxValue : (node._n - node._m));
					return;
				}
				return;
			case 5:
			case 8:
				if (node._m > 0)
				{
					this.Emit(2 | num, this.StringCode(node._str), node._m);
				}
				if (node._n > node._m)
				{
					this.Emit(node._type | num, this.StringCode(node._str), (node._n == int.MaxValue) ? int.MaxValue : (node._n - node._m));
					return;
				}
				return;
			case 9:
			case 10:
				this.Emit(node._type | num, (int)node._ch);
				return;
			case 11:
				this.Emit(node._type | num, this.StringCode(node._str));
				return;
			case 12:
				this.Emit(node._type | num, this.StringCode(node._str));
				return;
			case 13:
				this.Emit(node._type | num, this.MapCapnum(node._m));
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
				this.Emit(node._type);
				return;
			case 23:
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
				switch (num2)
				{
				case 88:
					if (CurIndex < node._children.Count - 1)
					{
						this.PushInt(this.CurPos());
						this.Emit(23, 0);
						return;
					}
					return;
				case 89:
				case 93:
					return;
				case 90:
				case 91:
					if (node._n < 2147483647 || node._m > 1)
					{
						this.Emit((node._m == 0) ? 26 : 27, (node._m == 0) ? 0 : (1 - node._m));
					}
					else
					{
						this.Emit((node._m == 0) ? 30 : 31);
					}
					if (node._m == 0)
					{
						this.PushInt(this.CurPos());
						this.Emit(38, 0);
					}
					this.PushInt(this.CurPos());
					return;
				case 92:
					this.Emit(31);
					return;
				case 94:
					this.Emit(34);
					this.Emit(31);
					return;
				case 95:
					this.Emit(34);
					this.PushInt(this.CurPos());
					this.Emit(23, 0);
					return;
				case 96:
					this.Emit(34);
					return;
				case 97:
					if (CurIndex != 0)
					{
						return;
					}
					this.Emit(34);
					this.PushInt(this.CurPos());
					this.Emit(23, 0);
					this.Emit(37, this.MapCapnum(node._m));
					this.Emit(36);
					return;
				case 98:
					if (CurIndex != 0)
					{
						return;
					}
					this.Emit(34);
					this.Emit(31);
					this.PushInt(this.CurPos());
					this.Emit(23, 0);
					return;
				default:
					switch (num2)
					{
					case 152:
					{
						if (CurIndex < node._children.Count - 1)
						{
							int num3 = this.PopInt();
							this.PushInt(this.CurPos());
							this.Emit(38, 0);
							this.PatchJump(num3, this.CurPos());
							return;
						}
						for (int i = 0; i < CurIndex; i++)
						{
							this.PatchJump(this.PopInt(), this.CurPos());
						}
						return;
					}
					case 153:
					case 157:
						return;
					case 154:
					case 155:
					{
						int num4 = this.CurPos();
						int num5 = nodetype - 154;
						if (node._n < 2147483647 || node._m > 1)
						{
							this.Emit(28 + num5, this.PopInt(), (node._n == int.MaxValue) ? int.MaxValue : (node._n - node._m));
						}
						else
						{
							this.Emit(24 + num5, this.PopInt());
						}
						if (node._m == 0)
						{
							this.PatchJump(this.PopInt(), num4);
							return;
						}
						return;
					}
					case 156:
						this.Emit(32, this.MapCapnum(node._m), this.MapCapnum(node._n));
						return;
					case 158:
						this.Emit(33);
						this.Emit(36);
						return;
					case 159:
						this.Emit(35);
						this.PatchJump(this.PopInt(), this.CurPos());
						this.Emit(36);
						return;
					case 160:
						this.Emit(36);
						return;
					case 161:
						switch (CurIndex)
						{
						case 0:
						{
							int num6 = this.PopInt();
							this.PushInt(this.CurPos());
							this.Emit(38, 0);
							this.PatchJump(num6, this.CurPos());
							this.Emit(36);
							if (node._children.Count > 1)
							{
								return;
							}
							break;
						}
						case 1:
							break;
						default:
							return;
						}
						this.PatchJump(this.PopInt(), this.CurPos());
						return;
					case 162:
						switch (CurIndex)
						{
						case 0:
							this.Emit(33);
							this.Emit(36);
							return;
						case 1:
						{
							int num7 = this.PopInt();
							this.PushInt(this.CurPos());
							this.Emit(38, 0);
							this.PatchJump(num7, this.CurPos());
							this.Emit(33);
							this.Emit(36);
							if (node._children.Count > 2)
							{
								return;
							}
							break;
						}
						case 2:
							break;
						default:
							return;
						}
						this.PatchJump(this.PopInt(), this.CurPos());
						return;
					}
					break;
				}
				break;
			}
			throw this.MakeException(SR.GetString("UnexpectedOpcode", new object[] { nodetype.ToString(CultureInfo.CurrentCulture) }));
		}

		// Token: 0x040007C2 RID: 1986
		internal const int BeforeChild = 64;

		// Token: 0x040007C3 RID: 1987
		internal const int AfterChild = 128;

		// Token: 0x040007C4 RID: 1988
		internal int[] _intStack;

		// Token: 0x040007C5 RID: 1989
		internal int _depth;

		// Token: 0x040007C6 RID: 1990
		internal int[] _emitted;

		// Token: 0x040007C7 RID: 1991
		internal int _curpos;

		// Token: 0x040007C8 RID: 1992
		internal IDictionary _stringhash;

		// Token: 0x040007C9 RID: 1993
		internal ArrayList _stringtable;

		// Token: 0x040007CA RID: 1994
		internal bool _counting;

		// Token: 0x040007CB RID: 1995
		internal int _count;

		// Token: 0x040007CC RID: 1996
		internal int _trackcount;

		// Token: 0x040007CD RID: 1997
		internal Hashtable _caps;
	}
}

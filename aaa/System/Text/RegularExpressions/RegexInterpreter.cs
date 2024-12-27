using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000024 RID: 36
	internal sealed class RegexInterpreter : RegexRunner
	{
		// Token: 0x0600017C RID: 380 RVA: 0x0000BB1C File Offset: 0x0000AB1C
		internal RegexInterpreter(RegexCode code, CultureInfo culture)
		{
			this.runcode = code;
			this.runcodes = code._codes;
			this.runstrings = code._strings;
			this.runfcPrefix = code._fcPrefix;
			this.runbmPrefix = code._bmPrefix;
			this.runanchors = code._anchors;
			this.runculture = culture;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000BB79 File Offset: 0x0000AB79
		protected override void InitTrackCount()
		{
			this.runtrackcount = this.runcode._trackcount;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000BB8C File Offset: 0x0000AB8C
		private void Advance()
		{
			this.Advance(0);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000BB95 File Offset: 0x0000AB95
		private void Advance(int i)
		{
			this.runcodepos += i + 1;
			this.SetOperator(this.runcodes[this.runcodepos]);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000BBBA File Offset: 0x0000ABBA
		private void Goto(int newpos)
		{
			if (newpos < this.runcodepos)
			{
				base.EnsureStorage();
			}
			this.SetOperator(this.runcodes[newpos]);
			this.runcodepos = newpos;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000BBE0 File Offset: 0x0000ABE0
		private void Textto(int newpos)
		{
			this.runtextpos = newpos;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000BBE9 File Offset: 0x0000ABE9
		private void Trackto(int newpos)
		{
			this.runtrackpos = this.runtrack.Length - newpos;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000BBFB File Offset: 0x0000ABFB
		private int Textstart()
		{
			return this.runtextstart;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000BC03 File Offset: 0x0000AC03
		private int Textpos()
		{
			return this.runtextpos;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000BC0B File Offset: 0x0000AC0B
		private int Trackpos()
		{
			return this.runtrack.Length - this.runtrackpos;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000BC1C File Offset: 0x0000AC1C
		private void TrackPush()
		{
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BC48 File Offset: 0x0000AC48
		private void TrackPush(int I1)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BC8C File Offset: 0x0000AC8C
		private void TrackPush(int I1, int I2)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000BCEC File Offset: 0x0000ACEC
		private void TrackPush(int I1, int I2, int I3)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = I3;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000BD64 File Offset: 0x0000AD64
		private void TrackPush2(int I1)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = -this.runcodepos;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000BDAC File Offset: 0x0000ADAC
		private void TrackPush2(int I1, int I2)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = -this.runcodepos;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000BE0C File Offset: 0x0000AE0C
		private void Backtrack()
		{
			int num = this.runtrack[this.runtrackpos++];
			if (num < 0)
			{
				num = -num;
				this.SetOperator(this.runcodes[num] | 256);
			}
			else
			{
				this.SetOperator(this.runcodes[num] | 128);
			}
			if (num < this.runcodepos)
			{
				base.EnsureStorage();
			}
			this.runcodepos = num;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000BE79 File Offset: 0x0000AE79
		private void SetOperator(int op)
		{
			this.runci = 0 != (op & 512);
			this.runrtl = 0 != (op & 64);
			this.runoperator = op & -577;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000BEAB File Offset: 0x0000AEAB
		private void TrackPop()
		{
			this.runtrackpos++;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000BEBB File Offset: 0x0000AEBB
		private void TrackPop(int framesize)
		{
			this.runtrackpos += framesize;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000BECB File Offset: 0x0000AECB
		private int TrackPeek()
		{
			return this.runtrack[this.runtrackpos - 1];
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000BEDC File Offset: 0x0000AEDC
		private int TrackPeek(int i)
		{
			return this.runtrack[this.runtrackpos - i - 1];
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000BEF0 File Offset: 0x0000AEF0
		private void StackPush(int I1)
		{
			this.runstack[--this.runstackpos] = I1;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000BF18 File Offset: 0x0000AF18
		private void StackPush(int I1, int I2)
		{
			this.runstack[--this.runstackpos] = I1;
			this.runstack[--this.runstackpos] = I2;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000BF57 File Offset: 0x0000AF57
		private void StackPop()
		{
			this.runstackpos++;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000BF67 File Offset: 0x0000AF67
		private void StackPop(int framesize)
		{
			this.runstackpos += framesize;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000BF77 File Offset: 0x0000AF77
		private int StackPeek()
		{
			return this.runstack[this.runstackpos - 1];
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000BF88 File Offset: 0x0000AF88
		private int StackPeek(int i)
		{
			return this.runstack[this.runstackpos - i - 1];
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000BF9B File Offset: 0x0000AF9B
		private int Operator()
		{
			return this.runoperator;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000BFA3 File Offset: 0x0000AFA3
		private int Operand(int i)
		{
			return this.runcodes[this.runcodepos + i + 1];
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000BFB6 File Offset: 0x0000AFB6
		private int Leftchars()
		{
			return this.runtextpos - this.runtextbeg;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000BFC5 File Offset: 0x0000AFC5
		private int Rightchars()
		{
			return this.runtextend - this.runtextpos;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000BFD4 File Offset: 0x0000AFD4
		private int Bump()
		{
			if (!this.runrtl)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000BFE1 File Offset: 0x0000AFE1
		private int Forwardchars()
		{
			if (!this.runrtl)
			{
				return this.runtextend - this.runtextpos;
			}
			return this.runtextpos - this.runtextbeg;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000C008 File Offset: 0x0000B008
		private char Forwardcharnext()
		{
			char c = (this.runrtl ? this.runtext[--this.runtextpos] : this.runtext[this.runtextpos++]);
			if (!this.runci)
			{
				return c;
			}
			return char.ToLower(c, this.runculture);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000C070 File Offset: 0x0000B070
		private bool Stringmatch(string str)
		{
			int num;
			int num2;
			if (!this.runrtl)
			{
				if (this.runtextend - this.runtextpos < (num = str.Length))
				{
					return false;
				}
				num2 = this.runtextpos + num;
			}
			else
			{
				if (this.runtextpos - this.runtextbeg < (num = str.Length))
				{
					return false;
				}
				num2 = this.runtextpos;
			}
			if (!this.runci)
			{
				while (num != 0)
				{
					if (str[--num] != this.runtext[--num2])
					{
						return false;
					}
				}
			}
			else
			{
				while (num != 0)
				{
					if (str[--num] != char.ToLower(this.runtext[--num2], this.runculture))
					{
						return false;
					}
				}
			}
			if (!this.runrtl)
			{
				num2 += str.Length;
			}
			this.runtextpos = num2;
			return true;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C140 File Offset: 0x0000B140
		private bool Refmatch(int index, int len)
		{
			/*
An exception occurred when decompiling this method (060001A0)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Text.RegularExpressions.RegexInterpreter::Refmatch(System.Int32,System.Int32)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackSlot.ModifyStack(StackSlot[] stack, Int32 popCount, Int32 pushCount, ByteCode pushDefinition) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 54
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 403
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C21D File Offset: 0x0000B21D
		private void Backwardnext()
		{
			this.runtextpos += (this.runrtl ? 1 : (-1));
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C238 File Offset: 0x0000B238
		private char CharAt(int j)
		{
			return this.runtext[j];
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C248 File Offset: 0x0000B248
		protected override bool FindFirstChar()
		{
			if ((this.runanchors & 53) != 0)
			{
				if (!this.runcode._rightToLeft)
				{
					if (((this.runanchors & 1) != 0 && this.runtextpos > this.runtextbeg) || ((this.runanchors & 4) != 0 && this.runtextpos > this.runtextstart))
					{
						this.runtextpos = this.runtextend;
						return false;
					}
					if ((this.runanchors & 16) != 0 && this.runtextpos < this.runtextend - 1)
					{
						this.runtextpos = this.runtextend - 1;
					}
					else if ((this.runanchors & 32) != 0 && this.runtextpos < this.runtextend)
					{
						this.runtextpos = this.runtextend;
					}
				}
				else
				{
					if (((this.runanchors & 32) != 0 && this.runtextpos < this.runtextend) || ((this.runanchors & 16) != 0 && (this.runtextpos < this.runtextend - 1 || (this.runtextpos == this.runtextend - 1 && this.CharAt(this.runtextpos) != '\n'))) || ((this.runanchors & 4) != 0 && this.runtextpos < this.runtextstart))
					{
						this.runtextpos = this.runtextbeg;
						return false;
					}
					if ((this.runanchors & 1) != 0 && this.runtextpos > this.runtextbeg)
					{
						this.runtextpos = this.runtextbeg;
					}
				}
				if (this.runbmPrefix != null)
				{
					return this.runbmPrefix.IsMatch(this.runtext, this.runtextpos, this.runtextbeg, this.runtextend);
				}
			}
			else if (this.runbmPrefix != null)
			{
				this.runtextpos = this.runbmPrefix.Scan(this.runtext, this.runtextpos, this.runtextbeg, this.runtextend);
				if (this.runtextpos == -1)
				{
					this.runtextpos = (this.runcode._rightToLeft ? this.runtextbeg : this.runtextend);
					return false;
				}
				return true;
			}
			if (this.runfcPrefix == null)
			{
				return true;
			}
			this.runrtl = this.runcode._rightToLeft;
			this.runci = this.runfcPrefix.CaseInsensitive;
			string prefix = this.runfcPrefix.Prefix;
			if (RegexCharClass.IsSingleton(prefix))
			{
				char c = RegexCharClass.SingletonChar(prefix);
				for (int i = this.Forwardchars(); i > 0; i--)
				{
					if (c == this.Forwardcharnext())
					{
						this.Backwardnext();
						return true;
					}
				}
			}
			else
			{
				for (int i = this.Forwardchars(); i > 0; i--)
				{
					if (RegexCharClass.CharInClass(this.Forwardcharnext(), prefix))
					{
						this.Backwardnext();
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000C4D0 File Offset: 0x0000B4D0
		protected override void Go()
		{
			/*
An exception occurred when decompiling this method (060001A4)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Text.RegularExpressions.RegexInterpreter::Go()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Runtime.CompilerServices.RuntimeHelpers.AllocateUninitializedClone(Object obj)
   at System.Collections.Generic.HashSet`1.ConstructFrom(HashSet`1 source)
   at System.Collections.Generic.HashSet`1..ctor(IEnumerable`1 collection, IEqualityComparer`1 comparer)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 249
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 69
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04000740 RID: 1856
		internal int runoperator;

		// Token: 0x04000741 RID: 1857
		internal int[] runcodes;

		// Token: 0x04000742 RID: 1858
		internal int runcodepos;

		// Token: 0x04000743 RID: 1859
		internal string[] runstrings;

		// Token: 0x04000744 RID: 1860
		internal RegexCode runcode;

		// Token: 0x04000745 RID: 1861
		internal RegexPrefix runfcPrefix;

		// Token: 0x04000746 RID: 1862
		internal RegexBoyerMoore runbmPrefix;

		// Token: 0x04000747 RID: 1863
		internal int runanchors;

		// Token: 0x04000748 RID: 1864
		internal bool runrtl;

		// Token: 0x04000749 RID: 1865
		internal bool runci;

		// Token: 0x0400074A RID: 1866
		internal CultureInfo runculture;
	}
}

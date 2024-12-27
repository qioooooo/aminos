using System;
using System.Threading;

namespace System.IO
{
	// Token: 0x0200000A RID: 10
	internal class ByteBufferPool : IByteBufferPool
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002588 File Offset: 0x00001588
		public ByteBufferPool(int maxBuffers, int bufferSize)
		{
			this._max = maxBuffers;
			this._bufferPool = new byte[this._max][];
			this._bufferSize = bufferSize;
			this._current = -1;
			this._last = -1;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000025C8 File Offset: 0x000015C8
		public byte[] GetBuffer()
		{
			object obj = null;
			byte[] array;
			try
			{
				obj = Interlocked.Exchange(ref this._controlCookie, null);
				if (obj != null)
				{
					if (this._current == -1)
					{
						this._controlCookie = obj;
						array = new byte[this._bufferSize];
					}
					else
					{
						byte[] array2 = this._bufferPool[this._current];
						this._bufferPool[this._current] = null;
						if (this._current == this._last)
						{
							this._current = -1;
						}
						else
						{
							this._current = (this._current + 1) % this._max;
						}
						this._controlCookie = obj;
						array = array2;
					}
				}
				else
				{
					array = new byte[this._bufferSize];
				}
			}
			catch (ThreadAbortException)
			{
				if (obj != null)
				{
					this._current = -1;
					this._last = -1;
					this._controlCookie = obj;
				}
				throw;
			}
			return array;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002694 File Offset: 0x00001694
		public void ReturnBuffer(byte[] buffer)
		{
			/*
An exception occurred when decompiling this method (06000023)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.IO.ByteBufferPool::ReturnBuffer(System.Byte[])

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.HashSet`1.Resize(Int32 newSize, Boolean forceNewHashCodes)
   at System.Collections.Generic.HashSet`1.AddIfNotPresent(T value, Int32& location)
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 270
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 378
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 313
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 378
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 275
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 284
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.TrySimplifyGoto(ILExpression gotoExpr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 228
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveGotosCore(ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 102
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveGotos(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 56
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 364
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04000041 RID: 65
		private byte[][] _bufferPool;

		// Token: 0x04000042 RID: 66
		private int _current;

		// Token: 0x04000043 RID: 67
		private int _last;

		// Token: 0x04000044 RID: 68
		private int _max;

		// Token: 0x04000045 RID: 69
		private int _bufferSize;

		// Token: 0x04000046 RID: 70
		private object _controlCookie = "cookie object";
	}
}

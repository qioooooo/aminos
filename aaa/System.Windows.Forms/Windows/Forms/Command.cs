using System;

namespace System.Windows.Forms
{
	// Token: 0x0200029B RID: 667
	internal class Command : WeakReference
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x0005245C File Offset: 0x0005145C
		public Command(ICommandExecutor target)
			: base(target, false)
		{
			Command.AssignID(this);
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0005246C File Offset: 0x0005146C
		public virtual int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x00052474 File Offset: 0x00051474
		protected static void AssignID(Command cmd)
		{
			/*
An exception occurred when decompiling this method (060023F1)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Command::AssignID(System.Windows.Forms.Command)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.SZArrayHelper.GetEnumerator[T]()
   at System.Collections.Generic.HashSet`1.UnionWith(IEnumerable`1 other)
   at System.Collections.Generic.HashSet`1..ctor(IEnumerable`1 collection, IEqualityComparer`1 comparer)
   at System.Linq.Enumerable.IntersectIterator[TSource](IEnumerable`1 first, IEnumerable`1 second, IEqualityComparer`1 comparer)+MoveNext()
   at System.Linq.Enumerable.<Any>g__WithEnumerator|36_0[TSource](IEnumerable`1 source)
   at System.Linq.Enumerable.WhereListIterator`1.ToList()
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertLocalVariables(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 804
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 612
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000525C0 File Offset: 0x000515C0
		public static bool DispatchID(int id)
		{
			Command commandFromID = Command.GetCommandFromID(id);
			return commandFromID != null && commandFromID.Invoke();
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000525E0 File Offset: 0x000515E0
		protected static void Dispose(Command cmd)
		{
			lock (Command.internalSyncObject)
			{
				if (cmd.id >= 256)
				{
					cmd.Target = null;
					if (Command.cmds[cmd.id - 256] == cmd)
					{
						Command.cmds[cmd.id - 256] = null;
					}
					cmd.id = 0;
				}
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x00052658 File Offset: 0x00051658
		public virtual void Dispose()
		{
			/*
An exception occurred when decompiling this method (060023F4)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Command::Dispose()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILBlockBase..ctor() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 251
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 109
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00052670 File Offset: 0x00051670
		public static Command GetCommandFromID(int id)
		{
			Command command;
			lock (Command.internalSyncObject)
			{
				if (Command.cmds == null)
				{
					command = null;
				}
				else
				{
					int num = id - 256;
					if (num < 0 || num >= Command.cmds.Length)
					{
						command = null;
					}
					else
					{
						command = Command.cmds[num];
					}
				}
			}
			return command;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x000526D0 File Offset: 0x000516D0
		public virtual bool Invoke()
		{
			object target = this.Target;
			if (!(target is ICommandExecutor))
			{
				return false;
			}
			((ICommandExecutor)target).Execute();
			return true;
		}

		// Token: 0x0400158E RID: 5518
		private const int idMin = 256;

		// Token: 0x0400158F RID: 5519
		private const int idLim = 65536;

		// Token: 0x04001590 RID: 5520
		private static Command[] cmds;

		// Token: 0x04001591 RID: 5521
		private static int icmdTry;

		// Token: 0x04001592 RID: 5522
		private static object internalSyncObject = new object();

		// Token: 0x04001593 RID: 5523
		internal int id;
	}
}

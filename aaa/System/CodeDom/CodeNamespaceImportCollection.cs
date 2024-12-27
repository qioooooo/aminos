using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000069 RID: 105
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeNamespaceImportCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170000B3 RID: 179
		public CodeNamespaceImport this[int index]
		{
			get
			{
				return (CodeNamespaceImport)this.data[index];
			}
			set
			{
				this.data[index] = value;
				this.SyncKeys();
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00014037 File Offset: 0x00013037
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014044 File Offset: 0x00013044
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00014047 File Offset: 0x00013047
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001404A File Offset: 0x0001304A
		public void Add(CodeNamespaceImport value)
		{
			if (!this.keys.ContainsKey(value.Namespace))
			{
				this.keys[value.Namespace] = value;
				this.data.Add(value);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00014080 File Offset: 0x00013080
		public void AddRange(CodeNamespaceImport[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (CodeNamespaceImport codeNamespaceImport in value)
			{
				this.Add(codeNamespaceImport);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000140B6 File Offset: 0x000130B6
		public void Clear()
		{
			this.data.Clear();
			this.keys.Clear();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000140D0 File Offset: 0x000130D0
		private void SyncKeys()
		{
			/*
An exception occurred when decompiling this method (060003DC)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.CodeDom.CodeNamespaceImportCollection::SyncKeys()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 286
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00014140 File Offset: 0x00013140
		public IEnumerator GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x170000B7 RID: 183
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (CodeNamespaceImport)value;
				this.SyncKeys();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001416B File Offset: 0x0001316B
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00014173 File Offset: 0x00013173
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00014176 File Offset: 0x00013176
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00014179 File Offset: 0x00013179
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00014188 File Offset: 0x00013188
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014190 File Offset: 0x00013190
		int IList.Add(object value)
		{
			return this.data.Add((CodeNamespaceImport)value);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000141A3 File Offset: 0x000131A3
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000141AB File Offset: 0x000131AB
		bool IList.Contains(object value)
		{
			return this.data.Contains(value);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000141B9 File Offset: 0x000131B9
		int IList.IndexOf(object value)
		{
			return this.data.IndexOf((CodeNamespaceImport)value);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000141CC File Offset: 0x000131CC
		void IList.Insert(int index, object value)
		{
			this.data.Insert(index, (CodeNamespaceImport)value);
			this.SyncKeys();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000141E6 File Offset: 0x000131E6
		void IList.Remove(object value)
		{
			this.data.Remove((CodeNamespaceImport)value);
			this.SyncKeys();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000141FF File Offset: 0x000131FF
		void IList.RemoveAt(int index)
		{
			this.data.RemoveAt(index);
			this.SyncKeys();
		}

		// Token: 0x04000860 RID: 2144
		private ArrayList data = new ArrayList();

		// Token: 0x04000861 RID: 2145
		private Hashtable keys = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}

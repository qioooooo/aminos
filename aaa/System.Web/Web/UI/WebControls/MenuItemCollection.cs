using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005E6 RID: 1510
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MenuItemCollection : ICollection, IEnumerable, IStateManager
	{
		// Token: 0x06004AD2 RID: 19154 RVA: 0x001318E3 File Offset: 0x001308E3
		public MenuItemCollection()
			: this(null)
		{
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x001318EC File Offset: 0x001308EC
		public MenuItemCollection(MenuItem owner)
		{
			this._owner = owner;
			this._list = new List<MenuItem>();
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06004AD4 RID: 19156 RVA: 0x00131906 File Offset: 0x00130906
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06004AD5 RID: 19157 RVA: 0x00131913 File Offset: 0x00130913
		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)this._list).IsSynchronized;
			}
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06004AD6 RID: 19158 RVA: 0x00131920 File Offset: 0x00130920
		private List<MenuItemCollection.LogItem> Log
		{
			get
			{
				if (this._log == null)
				{
					this._log = new List<MenuItemCollection.LogItem>();
				}
				return this._log;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06004AD7 RID: 19159 RVA: 0x0013193B File Offset: 0x0013093B
		public object SyncRoot
		{
			get
			{
				return ((ICollection)this._list).SyncRoot;
			}
		}

		// Token: 0x170012C1 RID: 4801
		public MenuItem this[int index]
		{
			get
			{
				return this._list[index];
			}
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x00131956 File Offset: 0x00130956
		public void Add(MenuItem child)
		{
			this.AddAt(this._list.Count, child);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0013196C File Offset: 0x0013096C
		public void AddAt(int index, MenuItem child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (child.Owner != null && child.Parent == null)
			{
				child.Owner.Items.Remove(child);
			}
			if (child.Parent != null)
			{
				child.Parent.ChildItems.Remove(child);
			}
			if (this._owner != null)
			{
				child.SetParent(this._owner);
				child.SetOwner(this._owner.Owner);
			}
			this._list.Insert(index, child);
			this._version++;
			if (this._isTrackingViewState)
			{
				((IStateManager)child).TrackViewState();
				child.SetDirty();
			}
			this.Log.Add(new MenuItemCollection.LogItem(MenuItemCollection.LogItemType.Insert, index, this._isTrackingViewState));
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00131A30 File Offset: 0x00130A30
		public void Clear()
		{
			if (this.Count == 0)
			{
				return;
			}
			if (this._owner != null)
			{
				Menu owner = this._owner.Owner;
				if (owner != null)
				{
					for (MenuItem menuItem = owner.SelectedItem; menuItem != null; menuItem = menuItem.Parent)
					{
						if (this.Contains(menuItem))
						{
							owner.SetSelectedItem(null);
							break;
						}
					}
				}
			}
			foreach (MenuItem menuItem2 in this._list)
			{
				menuItem2.SetParent(null);
			}
			this._list.Clear();
			this._version++;
			if (this._isTrackingViewState)
			{
				this.Log.Clear();
			}
			this.Log.Add(new MenuItemCollection.LogItem(MenuItemCollection.LogItemType.Clear, 0, this._isTrackingViewState));
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00131B0C File Offset: 0x00130B0C
		public void CopyTo(Array array, int index)
		{
			if (!(array is MenuItem[]))
			{
				throw new ArgumentException(SR.GetString("MenuItemCollection_InvalidArrayType"), "array");
			}
			this._list.CopyTo((MenuItem[])array, index);
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00131B3D File Offset: 0x00130B3D
		public void CopyTo(MenuItem[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00131B4C File Offset: 0x00130B4C
		public bool Contains(MenuItem c)
		{
			return this._list.Contains(c);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00131B5C File Offset: 0x00130B5C
		internal MenuItem FindItem(string[] path, int pos)
		{
			if (pos == path.Length)
			{
				return this._owner;
			}
			string text = TreeView.UnEscape(path[pos]);
			for (int i = 0; i < this.Count; i++)
			{
				MenuItem menuItem = this._list[i];
				if (menuItem.Value == text)
				{
					return menuItem.ChildItems.FindItem(path, pos + 1);
				}
			}
			return null;
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x00131BBC File Offset: 0x00130BBC
		public IEnumerator GetEnumerator()
		{
			return new MenuItemCollection.MenuItemCollectionEnumerator(this);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00131BC4 File Offset: 0x00130BC4
		public int IndexOf(MenuItem value)
		{
			return this._list.IndexOf(value);
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00131BD4 File Offset: 0x00130BD4
		public void Remove(MenuItem value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int num = this._list.IndexOf(value);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00131C08 File Offset: 0x00130C08
		public void RemoveAt(int index)
		{
			MenuItem menuItem = this._list[index];
			Menu owner = menuItem.Owner;
			if (owner != null)
			{
				for (MenuItem menuItem2 = owner.SelectedItem; menuItem2 != null; menuItem2 = menuItem2.Parent)
				{
					if (menuItem2 == menuItem)
					{
						owner.SetSelectedItem(null);
						break;
					}
				}
			}
			menuItem.SetParent(null);
			this._list.RemoveAt(index);
			this._version++;
			this.Log.Add(new MenuItemCollection.LogItem(MenuItemCollection.LogItemType.Remove, index, this._isTrackingViewState));
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00131C88 File Offset: 0x00130C88
		internal void SetDirty()
		{
			/*
An exception occurred when decompiling this method (06004AE4)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.MenuItemCollection::SetDirty()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 893
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x00131CF8 File Offset: 0x00130CF8
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				/*
An exception occurred when decompiling this method (06004AE5)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Web.UI.WebControls.MenuItemCollection::System.Web.UI.IStateManager.get_IsTrackingViewState()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.NameVariables.AssignNamesToVariables(DecompilerContext context, IList`1 parameters, HashSet`1 variables, ILBlock methodBody, StringBuilder stringBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\NameVariables.cs:line 85
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 136
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x00131D00 File Offset: 0x00130D00
		void IStateManager.LoadViewState(object state)
		{
			/*
An exception occurred when decompiling this method (06004AE6)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.MenuItemCollection::System.Web.UI.IStateManager.LoadViewState(System.Object)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 486
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 282
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00131DF0 File Offset: 0x00130DF0
		object IStateManager.SaveViewState()
		{
			object[] array = new object[this.Count + 1];
			bool flag = false;
			if (this._log != null && this._log.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (int i = 0; i < this._log.Count; i++)
				{
					MenuItemCollection.LogItem logItem = this._log[i];
					if (logItem.Tracked)
					{
						stringBuilder.Append((int)logItem.Type);
						stringBuilder.Append(":");
						stringBuilder.Append(logItem.Index);
						if (i < this._log.Count - 1)
						{
							stringBuilder.Append(",");
						}
						num++;
					}
				}
				if (num > 0)
				{
					array[0] = stringBuilder.ToString();
					flag = true;
				}
			}
			for (int j = 0; j < this.Count; j++)
			{
				array[j + 1] = ((IStateManager)this[j]).SaveViewState();
				if (array[j + 1] != null)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return null;
			}
			return array;
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x00131EF4 File Offset: 0x00130EF4
		void IStateManager.TrackViewState()
		{
			this._isTrackingViewState = true;
			for (int i = 0; i < this.Count; i++)
			{
				((IStateManager)this[i]).TrackViewState();
			}
		}

		// Token: 0x04002B83 RID: 11139
		private List<MenuItem> _list;

		// Token: 0x04002B84 RID: 11140
		private MenuItem _owner;

		// Token: 0x04002B85 RID: 11141
		private int _version;

		// Token: 0x04002B86 RID: 11142
		private bool _isTrackingViewState;

		// Token: 0x04002B87 RID: 11143
		private List<MenuItemCollection.LogItem> _log;

		// Token: 0x020005E7 RID: 1511
		private class LogItem
		{
			// Token: 0x06004AE9 RID: 19177 RVA: 0x00131F25 File Offset: 0x00130F25
			public LogItem(MenuItemCollection.LogItemType type, int index, bool tracked)
			{
				this._type = type;
				this._index = index;
				this._tracked = tracked;
			}

			// Token: 0x170012C3 RID: 4803
			// (get) Token: 0x06004AEA RID: 19178 RVA: 0x00131F42 File Offset: 0x00130F42
			public int Index
			{
				get
				{
					return this._index;
				}
			}

			// Token: 0x170012C4 RID: 4804
			// (get) Token: 0x06004AEB RID: 19179 RVA: 0x00131F4A File Offset: 0x00130F4A
			// (set) Token: 0x06004AEC RID: 19180 RVA: 0x00131F52 File Offset: 0x00130F52
			public bool Tracked
			{
				get
				{
					return this._tracked;
				}
				set
				{
					this._tracked = value;
				}
			}

			// Token: 0x170012C5 RID: 4805
			// (get) Token: 0x06004AED RID: 19181 RVA: 0x00131F5B File Offset: 0x00130F5B
			public MenuItemCollection.LogItemType Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x04002B88 RID: 11144
			private MenuItemCollection.LogItemType _type;

			// Token: 0x04002B89 RID: 11145
			private int _index;

			// Token: 0x04002B8A RID: 11146
			private bool _tracked;
		}

		// Token: 0x020005E8 RID: 1512
		private enum LogItemType
		{
			// Token: 0x04002B8C RID: 11148
			Insert,
			// Token: 0x04002B8D RID: 11149
			Remove,
			// Token: 0x04002B8E RID: 11150
			Clear
		}

		// Token: 0x020005E9 RID: 1513
		private class MenuItemCollectionEnumerator : IEnumerator
		{
			// Token: 0x06004AEE RID: 19182 RVA: 0x00131F63 File Offset: 0x00130F63
			internal MenuItemCollectionEnumerator(MenuItemCollection list)
			{
				this.list = list;
				this.index = -1;
				this.version = list._version;
			}

			// Token: 0x06004AEF RID: 19183 RVA: 0x00131F88 File Offset: 0x00130F88
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
				}
				if (this.index < this.list.Count - 1)
				{
					this.index++;
					this.currentElement = this.list[this.index];
					return true;
				}
				this.index = this.list.Count;
				return false;
			}

			// Token: 0x170012C6 RID: 4806
			// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x00132006 File Offset: 0x00131006
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x170012C7 RID: 4807
			// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x00132010 File Offset: 0x00131010
			public MenuItem Current
			{
				get
				{
					if (this.index == -1)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
					}
					if (this.index >= this.list.Count)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06004AF2 RID: 19186 RVA: 0x0013205F File Offset: 0x0013105F
			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
				}
				this.currentElement = null;
				this.index = -1;
			}

			// Token: 0x04002B8F RID: 11151
			private MenuItemCollection list;

			// Token: 0x04002B90 RID: 11152
			private int index;

			// Token: 0x04002B91 RID: 11153
			private int version;

			// Token: 0x04002B92 RID: 11154
			private MenuItem currentElement;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl.XsltOld.Debugger;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200012E RID: 302
	internal class ActionFrame : IStackFrame
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00044D49 File Offset: 0x00043D49
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x00044D51 File Offset: 0x00043D51
		internal PrefixQName CalulatedName
		{
			get
			{
				return this.calulatedName;
			}
			set
			{
				this.calulatedName = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00044D5A File Offset: 0x00043D5A
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00044D62 File Offset: 0x00043D62
		internal string StoredOutput
		{
			get
			{
				return this.storedOutput;
			}
			set
			{
				this.storedOutput = value;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00044D6B File Offset: 0x00043D6B
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00044D73 File Offset: 0x00043D73
		internal int State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00044D7C File Offset: 0x00043D7C
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x00044D84 File Offset: 0x00043D84
		internal int Counter
		{
			get
			{
				return this.counter;
			}
			set
			{
				this.counter = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00044D8D File Offset: 0x00043D8D
		internal ActionFrame Container
		{
			get
			{
				return this.container;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00044D95 File Offset: 0x00043D95
		internal XPathNavigator Node
		{
			get
			{
				if (this.nodeSet != null)
				{
					return this.nodeSet.Current;
				}
				return null;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00044DAC File Offset: 0x00043DAC
		internal XPathNodeIterator NodeSet
		{
			get
			{
				return this.nodeSet;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00044DB4 File Offset: 0x00043DB4
		internal XPathNodeIterator NewNodeSet
		{
			get
			{
				return this.newNodeSet;
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00044DBC File Offset: 0x00043DBC
		internal int IncrementCounter()
		{
			return ++this.counter;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00044DDA File Offset: 0x00043DDA
		internal void AllocateVariables(int count)
		{
			if (0 < count)
			{
				this.variables = new object[count];
				return;
			}
			this.variables = null;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00044DF4 File Offset: 0x00043DF4
		internal object GetVariable(int index)
		{
			return this.variables[index];
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00044DFE File Offset: 0x00043DFE
		internal void SetVariable(int index, object value)
		{
			this.variables[index] = value;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00044E09 File Offset: 0x00043E09
		internal void SetParameter(XmlQualifiedName name, object value)
		{
			if (this.withParams == null)
			{
				this.withParams = new Hashtable();
			}
			this.withParams[name] = value;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00044E2B File Offset: 0x00043E2B
		internal void ResetParams()
		{
			if (this.withParams != null)
			{
				this.withParams.Clear();
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00044E40 File Offset: 0x00043E40
		internal object GetParameter(XmlQualifiedName name)
		{
			if (this.withParams != null)
			{
				return this.withParams[name];
			}
			return null;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00044E58 File Offset: 0x00043E58
		internal void InitNodeSet(XPathNodeIterator nodeSet)
		{
			this.nodeSet = nodeSet;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00044E61 File Offset: 0x00043E61
		internal void InitNewNodeSet(XPathNodeIterator nodeSet)
		{
			this.newNodeSet = nodeSet;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00044E6C File Offset: 0x00043E6C
		internal void SortNewNodeSet(Processor proc, ArrayList sortarray)
		{
			int count = sortarray.Count;
			XPathSortComparer xpathSortComparer = new XPathSortComparer(count);
			for (int i = 0; i < count; i++)
			{
				Sort sort = (Sort)sortarray[i];
				Query compiledQuery = proc.GetCompiledQuery(sort.select);
				xpathSortComparer.AddSort(compiledQuery, new XPathComparerHelper(sort.order, sort.caseOrder, sort.lang, sort.dataType));
			}
			List<SortKey> list = new List<SortKey>();
			while (this.NewNextNode(proc))
			{
				XPathNodeIterator xpathNodeIterator = this.nodeSet;
				this.nodeSet = this.newNodeSet;
				SortKey sortKey = new SortKey(count, list.Count, this.newNodeSet.Current.Clone());
				for (int j = 0; j < count; j++)
				{
					sortKey[j] = xpathSortComparer.Expression(j).Evaluate(this.newNodeSet);
				}
				list.Add(sortKey);
				this.nodeSet = xpathNodeIterator;
			}
			list.Sort(xpathSortComparer);
			this.newNodeSet = new ActionFrame.XPathSortArrayIterator(list);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00044F6A File Offset: 0x00043F6A
		internal void Finished()
		{
			this.State = -1;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00044F73 File Offset: 0x00043F73
		internal void Inherit(ActionFrame parent)
		{
			this.variables = parent.variables;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00044F81 File Offset: 0x00043F81
		private void Init(Action action, ActionFrame container, XPathNodeIterator nodeSet)
		{
			this.state = 0;
			this.action = action;
			this.container = container;
			this.currentAction = 0;
			this.nodeSet = nodeSet;
			this.newNodeSet = null;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00044FAD File Offset: 0x00043FAD
		internal void Init(Action action, XPathNodeIterator nodeSet)
		{
			this.Init(action, null, nodeSet);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00044FB8 File Offset: 0x00043FB8
		internal void Init(ActionFrame containerFrame, XPathNodeIterator nodeSet)
		{
			this.Init(containerFrame.GetAction(0), containerFrame, nodeSet);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00044FC9 File Offset: 0x00043FC9
		internal void SetAction(Action action)
		{
			this.SetAction(action, 0);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00044FD3 File Offset: 0x00043FD3
		internal void SetAction(Action action, int state)
		{
			this.action = action;
			this.state = state;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00044FE3 File Offset: 0x00043FE3
		private Action GetAction(int actionIndex)
		{
			return ((ContainerAction)this.action).GetAction(actionIndex);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00044FF6 File Offset: 0x00043FF6
		internal void Exit()
		{
			this.Finished();
			this.container = null;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00045008 File Offset: 0x00044008
		internal bool Execute(Processor processor)
		{
			if (this.action == null)
			{
				return true;
			}
			this.action.Execute(processor, this);
			if (this.State == -1)
			{
				if (this.container != null)
				{
					this.currentAction++;
					this.action = this.container.GetAction(this.currentAction);
					this.State = 0;
				}
				else
				{
					this.action = null;
				}
				return this.action == null;
			}
			return false;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00045080 File Offset: 0x00044080
		internal bool NextNode(Processor proc)
		{
			bool flag = this.nodeSet.MoveNext();
			if (flag && proc.Stylesheet.Whitespace)
			{
				XPathNodeType xpathNodeType = this.nodeSet.Current.NodeType;
				if (xpathNodeType == XPathNodeType.Whitespace)
				{
					XPathNavigator xpathNavigator = this.nodeSet.Current.Clone();
					bool flag2;
					do
					{
						xpathNavigator.MoveTo(this.nodeSet.Current);
						xpathNavigator.MoveToParent();
						flag2 = !proc.Stylesheet.PreserveWhiteSpace(proc, xpathNavigator) && (flag = this.nodeSet.MoveNext());
						xpathNodeType = this.nodeSet.Current.NodeType;
					}
					while (flag2 && xpathNodeType == XPathNodeType.Whitespace);
				}
			}
			return flag;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00045124 File Offset: 0x00044124
		internal bool NewNextNode(Processor proc)
		{
			bool flag = this.newNodeSet.MoveNext();
			if (flag && proc.Stylesheet.Whitespace)
			{
				XPathNodeType xpathNodeType = this.newNodeSet.Current.NodeType;
				if (xpathNodeType == XPathNodeType.Whitespace)
				{
					XPathNavigator xpathNavigator = this.newNodeSet.Current.Clone();
					bool flag2;
					do
					{
						xpathNavigator.MoveTo(this.newNodeSet.Current);
						xpathNavigator.MoveToParent();
						flag2 = !proc.Stylesheet.PreserveWhiteSpace(proc, xpathNavigator) && (flag = this.newNodeSet.MoveNext());
						xpathNodeType = this.newNodeSet.Current.NodeType;
					}
					while (flag2 && xpathNodeType == XPathNodeType.Whitespace);
				}
			}
			return flag;
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000451C8 File Offset: 0x000441C8
		XPathNavigator IStackFrame.Instruction
		{
			get
			{
				if (this.action == null)
				{
					return null;
				}
				return this.action.GetDbgData(this).StyleSheet;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x000451E5 File Offset: 0x000441E5
		XPathNodeIterator IStackFrame.NodeSet
		{
			get
			{
				return this.nodeSet.Clone();
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000451F2 File Offset: 0x000441F2
		int IStackFrame.GetVariablesCount()
		{
			if (this.action == null)
			{
				return 0;
			}
			return this.action.GetDbgData(this).Variables.Length;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00045211 File Offset: 0x00044211
		XPathNavigator IStackFrame.GetVariable(int varIndex)
		{
			return this.action.GetDbgData(this).Variables[varIndex].GetDbgData(null).StyleSheet;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00045231 File Offset: 0x00044231
		object IStackFrame.GetVariableValue(int varIndex)
		{
			return this.GetVariable(this.action.GetDbgData(this).Variables[varIndex].VarKey);
		}

		// Token: 0x040008E1 RID: 2273
		private int state;

		// Token: 0x040008E2 RID: 2274
		private int counter;

		// Token: 0x040008E3 RID: 2275
		private object[] variables;

		// Token: 0x040008E4 RID: 2276
		private Hashtable withParams;

		// Token: 0x040008E5 RID: 2277
		private Action action;

		// Token: 0x040008E6 RID: 2278
		private ActionFrame container;

		// Token: 0x040008E7 RID: 2279
		private int currentAction;

		// Token: 0x040008E8 RID: 2280
		private XPathNodeIterator nodeSet;

		// Token: 0x040008E9 RID: 2281
		private XPathNodeIterator newNodeSet;

		// Token: 0x040008EA RID: 2282
		private PrefixQName calulatedName;

		// Token: 0x040008EB RID: 2283
		private string storedOutput;

		// Token: 0x0200012F RID: 303
		private class XPathSortArrayIterator : XPathArrayIterator
		{
			// Token: 0x06000D69 RID: 3433 RVA: 0x00045259 File Offset: 0x00044259
			public XPathSortArrayIterator(List<SortKey> list)
				: base(list)
			{
			}

			// Token: 0x06000D6A RID: 3434 RVA: 0x00045262 File Offset: 0x00044262
			public XPathSortArrayIterator(ActionFrame.XPathSortArrayIterator it)
				: base(it)
			{
			}

			// Token: 0x06000D6B RID: 3435 RVA: 0x0004526B File Offset: 0x0004426B
			public override XPathNodeIterator Clone()
			{
				return new ActionFrame.XPathSortArrayIterator(this);
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00045273 File Offset: 0x00044273
			public override XPathNavigator Current
			{
				get
				{
					return ((SortKey)this.list[this.index - 1]).Node;
				}
			}
		}
	}
}

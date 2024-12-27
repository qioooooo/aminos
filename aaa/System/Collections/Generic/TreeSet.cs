using System;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000248 RID: 584
	[Serializable]
	internal class TreeSet<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x060013E1 RID: 5089 RVA: 0x00042393 File Offset: 0x00041393
		public TreeSet(IComparer<T> comparer)
		{
			if (comparer == null)
			{
				this.comparer = Comparer<T>.Default;
				return;
			}
			this.comparer = comparer;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000423B1 File Offset: 0x000413B1
		protected TreeSet(SerializationInfo info, StreamingContext context)
		{
			this.siInfo = info;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x000423C0 File Offset: 0x000413C0
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x000423C8 File Offset: 0x000413C8
		public IComparer<T> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x000423D0 File Offset: 0x000413D0
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x000423D3 File Offset: 0x000413D3
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x000423D6 File Offset: 0x000413D6
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x000423F8 File Offset: 0x000413F8
		public void Add(T item)
		{
			if (this.root == null)
			{
				this.root = new TreeSet<T>.Node(item, false);
				this.count = 1;
				return;
			}
			TreeSet<T>.Node node = this.root;
			TreeSet<T>.Node node2 = null;
			TreeSet<T>.Node node3 = null;
			TreeSet<T>.Node node4 = null;
			int num = 0;
			while (node != null)
			{
				num = this.comparer.Compare(item, node.Item);
				if (num == 0)
				{
					this.root.IsRed = false;
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
				}
				if (TreeSet<T>.Is4Node(node))
				{
					TreeSet<T>.Split4Node(node);
					if (TreeSet<T>.IsRed(node2))
					{
						this.InsertionBalance(node, ref node2, node3, node4);
					}
				}
				node4 = node3;
				node3 = node2;
				node2 = node;
				node = ((num < 0) ? node.Left : node.Right);
			}
			TreeSet<T>.Node node5 = new TreeSet<T>.Node(item);
			if (num > 0)
			{
				node2.Right = node5;
			}
			else
			{
				node2.Left = node5;
			}
			if (node2.IsRed)
			{
				this.InsertionBalance(node5, ref node2, node3, node4);
			}
			this.root.IsRed = false;
			this.count++;
			this.version++;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000424F7 File Offset: 0x000414F7
		public void Clear()
		{
			this.root = null;
			this.count = 0;
			this.version++;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00042515 File Offset: 0x00041515
		public bool Contains(T item)
		{
			return this.FindNode(item) != null;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00042524 File Offset: 0x00041524
		internal bool InOrderTreeWalk(TreeWalkAction<T> action)
		{
			if (this.root == null)
			{
				return true;
			}
			Stack<TreeSet<T>.Node> stack = new Stack<TreeSet<T>.Node>(2 * (int)Math.Log((double)(this.Count + 1)));
			for (TreeSet<T>.Node node = this.root; node != null; node = node.Left)
			{
				stack.Push(node);
			}
			while (stack.Count != 0)
			{
				TreeSet<T>.Node node = stack.Pop();
				if (!action(node))
				{
					return false;
				}
				for (TreeSet<T>.Node node2 = node.Right; node2 != null; node2 = node2.Left)
				{
					stack.Push(node2);
				}
			}
			return true;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000425DC File Offset: 0x000415DC
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			this.InOrderTreeWalk(delegate(TreeSet<T>.Node node)
			{
				array[index++] = node.Item;
				return true;
			});
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00042694 File Offset: 0x00041694
		void ICollection.CopyTo(Array array, int index)
		{
			TreeSet<T>.<>c__DisplayClass4 CS$<>8__locals1 = new TreeSet<T>.<>c__DisplayClass4();
			CS$<>8__locals1.index = index;
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (CS$<>8__locals1.index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - CS$<>8__locals1.index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.CopyTo(array2, CS$<>8__locals1.index);
				return;
			}
			object[] objects = array as object[];
			if (objects == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				this.InOrderTreeWalk(delegate(TreeSet<T>.Node node)
				{
					objects[CS$<>8__locals1.index++] = node.Item;
					return true;
				});
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00042778 File Offset: 0x00041778
		public TreeSet<T>.Enumerator GetEnumerator()
		{
			return new TreeSet<T>.Enumerator(this);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00042780 File Offset: 0x00041780
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new TreeSet<T>.Enumerator(this);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004278D File Offset: 0x0004178D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new TreeSet<T>.Enumerator(this);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004279C File Offset: 0x0004179C
		internal TreeSet<T>.Node FindNode(T item)
		{
			int num;
			for (TreeSet<T>.Node node = this.root; node != null; node = ((num < 0) ? node.Left : node.Right))
			{
				num = this.comparer.Compare(item, node.Item);
				if (num == 0)
				{
					return node;
				}
			}
			return null;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x000427E4 File Offset: 0x000417E4
		public bool Remove(T item)
		{
			if (this.root == null)
			{
				return false;
			}
			TreeSet<T>.Node node = this.root;
			TreeSet<T>.Node node2 = null;
			TreeSet<T>.Node node3 = null;
			TreeSet<T>.Node node4 = null;
			TreeSet<T>.Node node5 = null;
			bool flag = false;
			while (node != null)
			{
				if (TreeSet<T>.Is2Node(node))
				{
					if (node2 == null)
					{
						node.IsRed = true;
					}
					else
					{
						TreeSet<T>.Node node6 = TreeSet<T>.GetSibling(node, node2);
						if (node6.IsRed)
						{
							if (node2.Right == node6)
							{
								TreeSet<T>.RotateLeft(node2);
							}
							else
							{
								TreeSet<T>.RotateRight(node2);
							}
							node2.IsRed = true;
							node6.IsRed = false;
							this.ReplaceChildOfNodeOrRoot(node3, node2, node6);
							node3 = node6;
							if (node2 == node4)
							{
								node5 = node6;
							}
							node6 = ((node2.Left == node) ? node2.Right : node2.Left);
						}
						if (TreeSet<T>.Is2Node(node6))
						{
							TreeSet<T>.Merge2Nodes(node2, node, node6);
						}
						else
						{
							TreeRotation treeRotation = TreeSet<T>.RotationNeeded(node2, node, node6);
							TreeSet<T>.Node node7 = null;
							switch (treeRotation)
							{
							case TreeRotation.LeftRotation:
								node6.Right.IsRed = false;
								node7 = TreeSet<T>.RotateLeft(node2);
								break;
							case TreeRotation.RightRotation:
								node6.Left.IsRed = false;
								node7 = TreeSet<T>.RotateRight(node2);
								break;
							case TreeRotation.RightLeftRotation:
								node7 = TreeSet<T>.RotateRightLeft(node2);
								break;
							case TreeRotation.LeftRightRotation:
								node7 = TreeSet<T>.RotateLeftRight(node2);
								break;
							}
							node7.IsRed = node2.IsRed;
							node2.IsRed = false;
							node.IsRed = true;
							this.ReplaceChildOfNodeOrRoot(node3, node2, node7);
							if (node2 == node4)
							{
								node5 = node7;
							}
						}
					}
				}
				int num = (flag ? (-1) : this.comparer.Compare(item, node.Item));
				if (num == 0)
				{
					flag = true;
					node4 = node;
					node5 = node2;
				}
				node3 = node2;
				node2 = node;
				if (num < 0)
				{
					node = node.Left;
				}
				else
				{
					node = node.Right;
				}
			}
			if (node4 != null)
			{
				this.ReplaceNode(node4, node5, node2, node3);
				this.count--;
			}
			if (this.root != null)
			{
				this.root.IsRed = false;
			}
			this.version++;
			return flag;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000429D0 File Offset: 0x000419D0
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000429DC File Offset: 0x000419DC
		protected void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
			}
			info.AddValue("Count", this.count);
			info.AddValue("Comparer", this.comparer, typeof(IComparer<T>));
			info.AddValue("Version", this.version);
			if (this.root != null)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("Items", array, typeof(T[]));
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00042A61 File Offset: 0x00041A61
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(sender);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00042A6C File Offset: 0x00041A6C
		protected void OnDeserialization(object sender)
		{
			if (this.comparer != null)
			{
				return;
			}
			if (this.siInfo == null)
			{
				ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_InvalidOnDeser);
			}
			this.comparer = (IComparer<T>)this.siInfo.GetValue("Comparer", typeof(IComparer<T>));
			int @int = this.siInfo.GetInt32("Count");
			if (@int != 0)
			{
				T[] array = (T[])this.siInfo.GetValue("Items", typeof(T[]));
				if (array == null)
				{
					ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingValues);
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.Add(array[i]);
				}
			}
			this.version = this.siInfo.GetInt32("Version");
			if (this.count != @int)
			{
				ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MismatchedCount);
			}
			this.siInfo = null;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00042B39 File Offset: 0x00041B39
		private static TreeSet<T>.Node GetSibling(TreeSet<T>.Node node, TreeSet<T>.Node parent)
		{
			if (parent.Left == node)
			{
				return parent.Right;
			}
			return parent.Left;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00042B54 File Offset: 0x00041B54
		private void InsertionBalance(TreeSet<T>.Node current, ref TreeSet<T>.Node parent, TreeSet<T>.Node grandParent, TreeSet<T>.Node greatGrandParent)
		{
			bool flag = grandParent.Right == parent;
			bool flag2 = parent.Right == current;
			TreeSet<T>.Node node;
			if (flag == flag2)
			{
				node = (flag2 ? TreeSet<T>.RotateLeft(grandParent) : TreeSet<T>.RotateRight(grandParent));
			}
			else
			{
				node = (flag2 ? TreeSet<T>.RotateLeftRight(grandParent) : TreeSet<T>.RotateRightLeft(grandParent));
				parent = greatGrandParent;
			}
			grandParent.IsRed = true;
			node.IsRed = false;
			this.ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, node);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00042BBD File Offset: 0x00041BBD
		private static bool Is2Node(TreeSet<T>.Node node)
		{
			return TreeSet<T>.IsBlack(node) && TreeSet<T>.IsNullOrBlack(node.Left) && TreeSet<T>.IsNullOrBlack(node.Right);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00042BE1 File Offset: 0x00041BE1
		private static bool Is4Node(TreeSet<T>.Node node)
		{
			return TreeSet<T>.IsRed(node.Left) && TreeSet<T>.IsRed(node.Right);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00042BFD File Offset: 0x00041BFD
		private static bool IsBlack(TreeSet<T>.Node node)
		{
			return node != null && !node.IsRed;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00042C0D File Offset: 0x00041C0D
		private static bool IsNullOrBlack(TreeSet<T>.Node node)
		{
			return node == null || !node.IsRed;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00042C1D File Offset: 0x00041C1D
		private static bool IsRed(TreeSet<T>.Node node)
		{
			return node != null && node.IsRed;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00042C2A File Offset: 0x00041C2A
		private static void Merge2Nodes(TreeSet<T>.Node parent, TreeSet<T>.Node child1, TreeSet<T>.Node child2)
		{
			parent.IsRed = false;
			child1.IsRed = true;
			child2.IsRed = true;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00042C41 File Offset: 0x00041C41
		private void ReplaceChildOfNodeOrRoot(TreeSet<T>.Node parent, TreeSet<T>.Node child, TreeSet<T>.Node newChild)
		{
			if (parent == null)
			{
				this.root = newChild;
				return;
			}
			if (parent.Left == child)
			{
				parent.Left = newChild;
				return;
			}
			parent.Right = newChild;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00042C68 File Offset: 0x00041C68
		private void ReplaceNode(TreeSet<T>.Node match, TreeSet<T>.Node parentOfMatch, TreeSet<T>.Node succesor, TreeSet<T>.Node parentOfSuccesor)
		{
			if (succesor == match)
			{
				succesor = match.Left;
			}
			else
			{
				if (succesor.Right != null)
				{
					succesor.Right.IsRed = false;
				}
				if (parentOfSuccesor != match)
				{
					parentOfSuccesor.Left = succesor.Right;
					succesor.Right = match.Right;
				}
				succesor.Left = match.Left;
			}
			if (succesor != null)
			{
				succesor.IsRed = match.IsRed;
			}
			this.ReplaceChildOfNodeOrRoot(parentOfMatch, match, succesor);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00042CD9 File Offset: 0x00041CD9
		internal void UpdateVersion()
		{
			this.version++;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00042CEC File Offset: 0x00041CEC
		private static TreeSet<T>.Node RotateLeft(TreeSet<T>.Node node)
		{
			TreeSet<T>.Node right = node.Right;
			node.Right = right.Left;
			right.Left = node;
			return right;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00042D14 File Offset: 0x00041D14
		private static TreeSet<T>.Node RotateLeftRight(TreeSet<T>.Node node)
		{
			TreeSet<T>.Node left = node.Left;
			TreeSet<T>.Node right = left.Right;
			node.Left = right.Right;
			right.Right = node;
			left.Right = right.Left;
			right.Left = left;
			return right;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00042D58 File Offset: 0x00041D58
		private static TreeSet<T>.Node RotateRight(TreeSet<T>.Node node)
		{
			TreeSet<T>.Node left = node.Left;
			node.Left = left.Right;
			left.Right = node;
			return left;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00042D80 File Offset: 0x00041D80
		private static TreeSet<T>.Node RotateRightLeft(TreeSet<T>.Node node)
		{
			TreeSet<T>.Node right = node.Right;
			TreeSet<T>.Node left = right.Left;
			node.Right = left.Left;
			left.Left = node;
			right.Left = left.Right;
			left.Right = right;
			return left;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00042DC2 File Offset: 0x00041DC2
		private static TreeRotation RotationNeeded(TreeSet<T>.Node parent, TreeSet<T>.Node current, TreeSet<T>.Node sibling)
		{
			if (TreeSet<T>.IsRed(sibling.Left))
			{
				if (parent.Left == current)
				{
					return TreeRotation.RightLeftRotation;
				}
				return TreeRotation.RightRotation;
			}
			else
			{
				if (parent.Left == current)
				{
					return TreeRotation.LeftRotation;
				}
				return TreeRotation.LeftRightRotation;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00042DEA File Offset: 0x00041DEA
		private static void Split4Node(TreeSet<T>.Node node)
		{
			node.IsRed = true;
			node.Left.IsRed = false;
			node.Right.IsRed = false;
		}

		// Token: 0x04001154 RID: 4436
		private const string ComparerName = "Comparer";

		// Token: 0x04001155 RID: 4437
		private const string CountName = "Count";

		// Token: 0x04001156 RID: 4438
		private const string ItemsName = "Items";

		// Token: 0x04001157 RID: 4439
		private const string VersionName = "Version";

		// Token: 0x04001158 RID: 4440
		private TreeSet<T>.Node root;

		// Token: 0x04001159 RID: 4441
		private IComparer<T> comparer;

		// Token: 0x0400115A RID: 4442
		private int count;

		// Token: 0x0400115B RID: 4443
		private int version;

		// Token: 0x0400115C RID: 4444
		private object _syncRoot;

		// Token: 0x0400115D RID: 4445
		private SerializationInfo siInfo;

		// Token: 0x02000249 RID: 585
		internal class Node
		{
			// Token: 0x06001408 RID: 5128 RVA: 0x00042E0B File Offset: 0x00041E0B
			public Node(T item)
			{
				this.item = item;
				this.isRed = true;
			}

			// Token: 0x06001409 RID: 5129 RVA: 0x00042E21 File Offset: 0x00041E21
			public Node(T item, bool isRed)
			{
				this.item = item;
				this.isRed = isRed;
			}

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x0600140A RID: 5130 RVA: 0x00042E37 File Offset: 0x00041E37
			// (set) Token: 0x0600140B RID: 5131 RVA: 0x00042E3F File Offset: 0x00041E3F
			public T Item
			{
				get
				{
					return this.item;
				}
				set
				{
					this.item = value;
				}
			}

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x0600140C RID: 5132 RVA: 0x00042E48 File Offset: 0x00041E48
			// (set) Token: 0x0600140D RID: 5133 RVA: 0x00042E50 File Offset: 0x00041E50
			public TreeSet<T>.Node Left
			{
				get
				{
					return this.left;
				}
				set
				{
					this.left = value;
				}
			}

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x0600140E RID: 5134 RVA: 0x00042E59 File Offset: 0x00041E59
			// (set) Token: 0x0600140F RID: 5135 RVA: 0x00042E61 File Offset: 0x00041E61
			public TreeSet<T>.Node Right
			{
				get
				{
					return this.right;
				}
				set
				{
					this.right = value;
				}
			}

			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x06001410 RID: 5136 RVA: 0x00042E6A File Offset: 0x00041E6A
			// (set) Token: 0x06001411 RID: 5137 RVA: 0x00042E72 File Offset: 0x00041E72
			public bool IsRed
			{
				get
				{
					return this.isRed;
				}
				set
				{
					this.isRed = value;
				}
			}

			// Token: 0x0400115E RID: 4446
			private bool isRed;

			// Token: 0x0400115F RID: 4447
			private T item;

			// Token: 0x04001160 RID: 4448
			private TreeSet<T>.Node left;

			// Token: 0x04001161 RID: 4449
			private TreeSet<T>.Node right;
		}

		// Token: 0x0200024A RID: 586
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06001412 RID: 5138 RVA: 0x00042E7C File Offset: 0x00041E7C
			internal Enumerator(TreeSet<T> set)
			{
				this.tree = set;
				this.version = this.tree.version;
				this.stack = new Stack<TreeSet<T>.Node>(2 * (int)Math.Log((double)(set.Count + 1)));
				this.current = null;
				this.Intialize();
			}

			// Token: 0x06001413 RID: 5139 RVA: 0x00042ECC File Offset: 0x00041ECC
			private void Intialize()
			{
				this.current = null;
				for (TreeSet<T>.Node node = this.tree.root; node != null; node = node.Left)
				{
					this.stack.Push(node);
				}
			}

			// Token: 0x06001414 RID: 5140 RVA: 0x00042F04 File Offset: 0x00041F04
			public bool MoveNext()
			{
				if (this.version != this.tree.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.stack.Count == 0)
				{
					this.current = null;
					return false;
				}
				this.current = this.stack.Pop();
				for (TreeSet<T>.Node node = this.current.Right; node != null; node = node.Left)
				{
					this.stack.Push(node);
				}
				return true;
			}

			// Token: 0x06001415 RID: 5141 RVA: 0x00042F77 File Offset: 0x00041F77
			public void Dispose()
			{
			}

			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x06001416 RID: 5142 RVA: 0x00042F7C File Offset: 0x00041F7C
			public T Current
			{
				get
				{
					if (this.current != null)
					{
						return this.current.Item;
					}
					return default(T);
				}
			}

			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x06001417 RID: 5143 RVA: 0x00042FA6 File Offset: 0x00041FA6
			object IEnumerator.Current
			{
				get
				{
					if (this.current == null)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Item;
				}
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x06001418 RID: 5144 RVA: 0x00042FC7 File Offset: 0x00041FC7
			internal bool NotStartedOrEnded
			{
				get
				{
					return this.current == null;
				}
			}

			// Token: 0x06001419 RID: 5145 RVA: 0x00042FD2 File Offset: 0x00041FD2
			internal void Reset()
			{
				if (this.version != this.tree.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.stack.Clear();
				this.Intialize();
			}

			// Token: 0x0600141A RID: 5146 RVA: 0x00042FFF File Offset: 0x00041FFF
			void IEnumerator.Reset()
			{
				this.Reset();
			}

			// Token: 0x04001162 RID: 4450
			private const string TreeName = "Tree";

			// Token: 0x04001163 RID: 4451
			private const string NodeValueName = "Item";

			// Token: 0x04001164 RID: 4452
			private const string EnumStartName = "EnumStarted";

			// Token: 0x04001165 RID: 4453
			private const string VersionName = "Version";

			// Token: 0x04001166 RID: 4454
			private TreeSet<T> tree;

			// Token: 0x04001167 RID: 4455
			private int version;

			// Token: 0x04001168 RID: 4456
			private Stack<TreeSet<T>.Node> stack;

			// Token: 0x04001169 RID: 4457
			private TreeSet<T>.Node current;

			// Token: 0x0400116A RID: 4458
			private static TreeSet<T>.Node dummyNode = new TreeSet<T>.Node(default(T));
		}
	}
}

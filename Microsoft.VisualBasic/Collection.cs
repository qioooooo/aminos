using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(Collection.CollectionDebugView))]
	[Serializable]
	public sealed class Collection : ICollection, IList, ISerializable, IDeserializationCallback
	{
		public Collection()
		{
			this.Initialize(Utils.GetCultureInfo(), 0);
		}

		public void Add(object Item, string Key = null, object Before = null, object After = null)
		{
			if (Before != null && After != null)
			{
				throw new ArgumentException(Utils.GetResourceString("Collection_BeforeAfterExclusive"));
			}
			Collection.Node node = new Collection.Node(Key, Item);
			if (Key != null)
			{
				try
				{
					this.m_KeyedNodesHash.Add(Key, node);
				}
				catch (ArgumentException ex)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Collection_DuplicateKey")), 457);
				}
			}
			try
			{
				if (Before == null && After == null)
				{
					this.m_ItemsList.Add(node);
				}
				else if (Before != null)
				{
					string text = Before as string;
					if (text != null)
					{
						Collection.Node node2 = null;
						if (!this.m_KeyedNodesHash.TryGetValue(text, out node2))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Before" }));
						}
						this.m_ItemsList.InsertBefore(node, node2);
					}
					else
					{
						this.m_ItemsList.Insert(checked(Conversions.ToInteger(Before) - 1), node);
					}
				}
				else
				{
					string text2 = After as string;
					if (text2 != null)
					{
						Collection.Node node3 = null;
						if (!this.m_KeyedNodesHash.TryGetValue(text2, out node3))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "After" }));
						}
						this.m_ItemsList.InsertAfter(node, node3);
					}
					else
					{
						this.m_ItemsList.Insert(Conversions.ToInteger(After), node);
					}
				}
			}
			catch (OutOfMemoryException ex2)
			{
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				throw;
			}
			catch (StackOverflowException ex4)
			{
				throw;
			}
			catch (Exception ex5)
			{
				if (Key != null)
				{
					this.m_KeyedNodesHash.Remove(Key);
				}
				throw;
			}
			this.AdjustEnumeratorsOnNodeInserted(node);
		}

		public void Clear()
		{
			this.m_KeyedNodesHash.Clear();
			this.m_ItemsList.Clear();
			checked
			{
				for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
				{
					WeakReference weakReference = (WeakReference)this.m_Iterators[i];
					if (weakReference.IsAlive)
					{
						ForEachEnum forEachEnum = (ForEachEnum)weakReference.Target;
						if (forEachEnum != null)
						{
							forEachEnum.AdjustOnListCleared();
						}
					}
					else
					{
						this.m_Iterators.RemoveAt(i);
					}
				}
			}
		}

		public bool Contains(string Key)
		{
			if (Key == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Key" }));
			}
			return this.m_KeyedNodesHash.ContainsKey(Key);
		}

		public void Remove(string Key)
		{
			Collection.Node node = null;
			if (this.m_KeyedNodesHash.TryGetValue(Key, out node))
			{
				this.AdjustEnumeratorsOnNodeRemoved(node);
				this.m_KeyedNodesHash.Remove(Key);
				this.m_ItemsList.RemoveNode(node);
				node.m_Prev = null;
				node.m_Next = null;
				return;
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Key" }));
		}

		public void Remove(int Index)
		{
			this.IndexCheck(Index);
			Collection.Node node = this.m_ItemsList.RemoveAt(checked(Index - 1));
			this.AdjustEnumeratorsOnNodeRemoved(node);
			if (node.m_Key != null)
			{
				this.m_KeyedNodesHash.Remove(node.m_Key);
			}
			node.m_Prev = null;
			node.m_Next = null;
		}

		public object this[int Index]
		{
			get
			{
				this.IndexCheck(Index);
				Collection.Node node = this.m_ItemsList.get_Item(checked(Index - 1));
				return node.m_Value;
			}
		}

		public object this[string Key]
		{
			get
			{
				if (Key == null)
				{
					throw new IndexOutOfRangeException(Utils.GetResourceString("Argument_CollectionIndex"));
				}
				Collection.Node node = null;
				if (!this.m_KeyedNodesHash.TryGetValue(Key, out node))
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Index" }));
				}
				return node.m_Value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object this[object Index]
		{
			get
			{
				if (!(Index is string))
				{
					if (!(Index is char))
					{
						if (!(Index is char[]))
						{
							int num;
							try
							{
								num = Conversions.ToInteger(Index);
							}
							catch (StackOverflowException ex)
							{
								throw ex;
							}
							catch (OutOfMemoryException ex2)
							{
								throw ex2;
							}
							catch (ThreadAbortException ex3)
							{
								throw ex3;
							}
							catch (Exception)
							{
								throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Index" }));
							}
							return this[num];
						}
					}
				}
				string text = Conversions.ToString(Index);
				return this[text];
			}
		}

		public int Count
		{
			get
			{
				return this.m_ItemsList.Count();
			}
		}

		public IEnumerator GetEnumerator()
		{
			checked
			{
				for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
				{
					WeakReference weakReference = (WeakReference)this.m_Iterators[i];
					if (!weakReference.IsAlive)
					{
						this.m_Iterators.RemoveAt(i);
					}
				}
				ForEachEnum forEachEnum = new ForEachEnum(this);
				WeakReference weakReference2 = new WeakReference(forEachEnum);
				forEachEnum.WeakRef = weakReference2;
				this.m_Iterators.Add(weakReference2);
				return forEachEnum;
			}
		}

		internal void RemoveIterator(WeakReference weakref)
		{
			this.m_Iterators.Remove(weakref);
		}

		internal void AddIterator(WeakReference weakref)
		{
			this.m_Iterators.Add(weakref);
		}

		internal Collection.Node GetFirstListNode()
		{
			return this.m_ItemsList.GetFirstListNode();
		}

		private void Initialize(CultureInfo CultureInfo, int StartingHashCapacity = 0)
		{
			if (StartingHashCapacity > 0)
			{
				this.m_KeyedNodesHash = new Dictionary<string, Collection.Node>(StartingHashCapacity, StringComparer.Create(CultureInfo, true));
			}
			else
			{
				this.m_KeyedNodesHash = new Dictionary<string, Collection.Node>(StringComparer.Create(CultureInfo, true));
			}
			this.m_ItemsList = new Collection.FastList();
			this.m_Iterators = new ArrayList();
			this.m_CultureInfo = CultureInfo;
		}

		private void AdjustEnumeratorsOnNodeInserted(Collection.Node NewNode)
		{
			this.AdjustEnumeratorsHelper(NewNode, ForEachEnum.AdjustIndexType.Insert);
		}

		private void AdjustEnumeratorsOnNodeRemoved(Collection.Node RemovedNode)
		{
			this.AdjustEnumeratorsHelper(RemovedNode, ForEachEnum.AdjustIndexType.Remove);
		}

		private void AdjustEnumeratorsHelper(Collection.Node NewOrRemovedNode, ForEachEnum.AdjustIndexType Type)
		{
			checked
			{
				for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
				{
					WeakReference weakReference = (WeakReference)this.m_Iterators[i];
					if (weakReference.IsAlive)
					{
						ForEachEnum forEachEnum = (ForEachEnum)weakReference.Target;
						if (forEachEnum != null)
						{
							forEachEnum.Adjust(NewOrRemovedNode, Type);
						}
					}
					else
					{
						this.m_Iterators.RemoveAt(i);
					}
				}
			}
		}

		private void IndexCheck(int Index)
		{
			if (Index < 1 || Index > this.m_ItemsList.Count())
			{
				throw new IndexOutOfRangeException(Utils.GetResourceString("Argument_CollectionIndex"));
			}
		}

		private Collection.FastList InternalItemsList()
		{
			return this.m_ItemsList;
		}

		private Collection(SerializationInfo info, StreamingContext context)
		{
			this.m_DeserializationInfo = info;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			checked
			{
				string[] array = new string[this.Count - 1 + 1];
				object[] array2 = new object[this.Count - 1 + 1];
				Collection.Node node = this.GetFirstListNode();
				int num = 0;
				while (node != null)
				{
					if (node.m_Key != null)
					{
						num++;
					}
					int num2;
					array[num2] = node.m_Key;
					array2[num2] = node.m_Value;
					num2++;
					node = node.m_Next;
				}
				info.AddValue("Keys", array, typeof(string[]));
				info.AddValue("KeysCount", num, typeof(int));
				info.AddValue("Values", array2, typeof(object[]));
				info.AddValue("CultureInfo", this.m_CultureInfo);
			}
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			checked
			{
				try
				{
					CultureInfo cultureInfo = (CultureInfo)this.m_DeserializationInfo.GetValue("CultureInfo", typeof(CultureInfo));
					if (cultureInfo == null)
					{
						throw new SerializationException(Utils.GetResourceString("Serialization_MissingCultureInfo"));
					}
					string[] array = (string[])this.m_DeserializationInfo.GetValue("Keys", typeof(string[]));
					object[] array2 = (object[])this.m_DeserializationInfo.GetValue("Values", typeof(object[]));
					if (array == null)
					{
						throw new SerializationException(Utils.GetResourceString("Serialization_MissingKeys"));
					}
					if (array2 == null)
					{
						throw new SerializationException(Utils.GetResourceString("Serialization_MissingValues"));
					}
					if (array.Length != array2.Length)
					{
						throw new SerializationException(Utils.GetResourceString("Serialization_KeyValueDifferentSizes"));
					}
					int num = this.m_DeserializationInfo.GetInt32("KeysCount");
					if (num < 0 || num > array.Length)
					{
						num = 0;
					}
					this.Initialize(cultureInfo, num);
					int num2 = 0;
					int num3 = array.Length - 1;
					for (int i = num2; i <= num3; i++)
					{
						this.Add(array2[i], array[i], null, null);
					}
					this.m_DeserializationInfo = null;
				}
				finally
				{
					if (this.m_DeserializationInfo != null)
					{
						this.m_DeserializationInfo = null;
						this.Initialize(Utils.GetCultureInfo(), 0);
					}
				}
			}
		}

		IEnumerator IEnumerable.ICollectionGetEnumerator()
		{
			return this.GetEnumerator();
		}

		int ICollection.ICollectionCount
		{
			get
			{
				return this.m_ItemsList.Count();
			}
		}

		bool ICollection.ICollectionIsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.ICollectionSyncRoot
		{
			get
			{
				return this;
			}
		}

		bool IList.IListIsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool IList.IListIsReadOnly
		{
			get
			{
				return false;
			}
		}

		void ICollection.ICollectionCopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "array" }));
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1", new string[] { "array" }));
			}
			checked
			{
				if (index < 0 || array.Length - index < this.Count)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "index" }));
				}
				object[] array2 = array as object[];
				if (array2 != null)
				{
					int num = 1;
					int count = this.Count;
					for (int i = num; i <= count; i++)
					{
						array2[index + i - 1] = this[i];
					}
				}
				else
				{
					int num2 = 1;
					int count2 = this.Count;
					for (int i = num2; i <= count2; i++)
					{
						array.SetValue(this[i], index + i - 1);
					}
				}
			}
		}

		int IList.IListAdd(object value)
		{
			this.Add(value, null, null, null);
			return checked(this.m_ItemsList.Count() - 1);
		}

		void IList.IListInsert(int index, object value)
		{
			Collection.Node node = new Collection.Node(null, value);
			this.m_ItemsList.Insert(index, node);
			this.AdjustEnumeratorsOnNodeInserted(node);
		}

		void IList.IListRemoveAt(int index)
		{
			Collection.Node node = this.m_ItemsList.RemoveAt(index);
			this.AdjustEnumeratorsOnNodeRemoved(node);
			if (node.m_Key != null)
			{
				this.m_KeyedNodesHash.Remove(node.m_Key);
			}
			node.m_Prev = null;
			node.m_Next = null;
		}

		void IList.IListRemove(object value)
		{
			int num = this.IListIndexOf(value);
			if (num != -1)
			{
				this.IListRemoveAt(num);
			}
		}

		void IList.IListClear()
		{
			this.Clear();
		}

		object IList.this[int index]
		{
			get
			{
				Collection.Node node = this.m_ItemsList.get_Item(index);
				return node.m_Value;
			}
			set
			{
				Collection.Node node = this.m_ItemsList.get_Item(index);
				node.m_Value = value;
			}
		}

		bool IList.IListContains(object value)
		{
			return this.IListIndexOf(value) != -1;
		}

		int IList.IListIndexOf(object value)
		{
			return this.m_ItemsList.IndexOfValue(value);
		}

		private const string SERIALIZATIONKEY_KEYS = "Keys";

		private const string SERIALIZATIONKEY_KEYSCOUNT = "KeysCount";

		private const string SERIALIZATIONKEY_VALUES = "Values";

		private const string SERIALIZATIONKEY_CULTUREINFO = "CultureInfo";

		private SerializationInfo m_DeserializationInfo;

		private Dictionary<string, Collection.Node> m_KeyedNodesHash;

		private Collection.FastList m_ItemsList;

		private ArrayList m_Iterators;

		private CultureInfo m_CultureInfo;

		internal sealed class Node
		{
			internal Node(string Key, object Value)
			{
				this.m_Value = Value;
				this.m_Key = Key;
			}

			internal object m_Value;

			internal string m_Key;

			internal Collection.Node m_Next;

			internal Collection.Node m_Prev;
		}

		internal sealed class CollectionDebugView
		{
			public CollectionDebugView(Collection RealClass)
			{
				this.m_InstanceBeingWatched = RealClass;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					int count = this.m_InstanceBeingWatched.Count;
					if (count == 0)
					{
						return null;
					}
					checked
					{
						object[] array = new object[count + 1];
						array[0] = Utils.GetResourceString("EmptyPlaceHolderMessage");
						int num = 1;
						int num2 = count;
						for (int i = num; i <= num2; i++)
						{
							Collection.Node node = this.m_InstanceBeingWatched.InternalItemsList().get_Item(i - 1);
							object[] array2 = array;
							int num3 = i;
							Collection.KeyValuePair keyValuePair = new Collection.KeyValuePair(node.m_Key, node.m_Value);
							array2[num3] = keyValuePair;
						}
						return array;
					}
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Collection m_InstanceBeingWatched;
		}

		private sealed class FastList
		{
			internal FastList()
			{
				this.m_Count = 0;
			}

			internal void Add(Collection.Node Node)
			{
				if (this.m_StartOfList == null)
				{
					this.m_StartOfList = Node;
				}
				else
				{
					this.m_EndOfList.m_Next = Node;
					Node.m_Prev = this.m_EndOfList;
				}
				this.m_EndOfList = Node;
				checked
				{
					this.m_Count++;
				}
			}

			internal int IndexOfValue(object Value)
			{
				Collection.Node node = this.m_StartOfList;
				int num = 0;
				checked
				{
					while (node != null)
					{
						if (this.DataIsEqual(node.m_Value, Value))
						{
							return num;
						}
						node = node.m_Next;
						num++;
					}
					return -1;
				}
			}

			internal void RemoveNode(Collection.Node NodeToBeDeleted)
			{
				this.DeleteNode(NodeToBeDeleted, NodeToBeDeleted.m_Prev);
			}

			internal Collection.Node RemoveAt(int Index)
			{
				Collection.Node node = this.m_StartOfList;
				int num = 0;
				Collection.Node node2 = null;
				checked
				{
					while (num < Index && node != null)
					{
						node2 = node;
						node = node.m_Next;
						num++;
					}
					if (node == null)
					{
						throw new ArgumentOutOfRangeException("Index");
					}
					this.DeleteNode(node, node2);
					return node;
				}
			}

			internal int Count()
			{
				return this.m_Count;
			}

			internal void Clear()
			{
				this.m_StartOfList = null;
				this.m_EndOfList = null;
				this.m_Count = 0;
			}

			internal Collection.Node Item
			{
				get
				{
					Collection.Node node = null;
					Collection.Node nodeAtIndex = this.GetNodeAtIndex(Index, ref node);
					if (nodeAtIndex == null)
					{
						throw new ArgumentOutOfRangeException("Index");
					}
					return nodeAtIndex;
				}
			}

			internal void Insert(int Index, Collection.Node Node)
			{
				Collection.Node node = null;
				if (Index < 0 || Index > this.m_Count)
				{
					throw new ArgumentOutOfRangeException("Index");
				}
				Collection.Node nodeAtIndex = this.GetNodeAtIndex(Index, ref node);
				this.Insert(Node, node, nodeAtIndex);
			}

			internal void InsertBefore(Collection.Node Node, Collection.Node NodeToInsertBefore)
			{
				this.Insert(Node, NodeToInsertBefore.m_Prev, NodeToInsertBefore);
			}

			internal void InsertAfter(Collection.Node Node, Collection.Node NodeToInsertAfter)
			{
				this.Insert(Node, NodeToInsertAfter, NodeToInsertAfter.m_Next);
			}

			internal Collection.Node GetFirstListNode()
			{
				return this.m_StartOfList;
			}

			private bool DataIsEqual(object obj1, object obj2)
			{
				return obj1 == obj2 || (obj1.GetType() == obj2.GetType() && object.Equals(obj1, obj2));
			}

			private Collection.Node GetNodeAtIndex(int Index, ref Collection.Node PrevNode = null)
			{
				Collection.Node node = this.m_StartOfList;
				int num = 0;
				PrevNode = null;
				checked
				{
					while (num < Index && node != null)
					{
						PrevNode = node;
						node = node.m_Next;
						num++;
					}
					return node;
				}
			}

			private void Insert(Collection.Node Node, Collection.Node PrevNode, Collection.Node CurrentNode)
			{
				Node.m_Next = CurrentNode;
				if (CurrentNode != null)
				{
					CurrentNode.m_Prev = Node;
				}
				if (PrevNode == null)
				{
					this.m_StartOfList = Node;
				}
				else
				{
					PrevNode.m_Next = Node;
					Node.m_Prev = PrevNode;
				}
				if (Node.m_Next == null)
				{
					this.m_EndOfList = Node;
				}
				checked
				{
					this.m_Count++;
				}
			}

			private void DeleteNode(Collection.Node NodeToBeDeleted, Collection.Node PrevNode)
			{
				if (PrevNode == null)
				{
					this.m_StartOfList = this.m_StartOfList.m_Next;
					if (this.m_StartOfList == null)
					{
						this.m_EndOfList = null;
					}
					else
					{
						this.m_StartOfList.m_Prev = null;
					}
				}
				else
				{
					PrevNode.m_Next = NodeToBeDeleted.m_Next;
					if (PrevNode.m_Next == null)
					{
						this.m_EndOfList = PrevNode;
					}
					else
					{
						PrevNode.m_Next.m_Prev = PrevNode;
					}
				}
				checked
				{
					this.m_Count--;
				}
			}

			private Collection.Node m_StartOfList;

			private Collection.Node m_EndOfList;

			private int m_Count;
		}

		private struct KeyValuePair
		{
			internal KeyValuePair(object NewKey, object NewValue)
			{
				this = default(Collection.KeyValuePair);
				this.m_Key = NewKey;
				this.m_Value = NewValue;
			}

			public object Key
			{
				get
				{
					return this.m_Key;
				}
			}

			public object Value
			{
				get
				{
					return this.m_Value;
				}
			}

			private object m_Key;

			private object m_Value;
		}
	}
}

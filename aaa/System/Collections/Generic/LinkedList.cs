using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x0200022F RID: 559
	[DebuggerTypeProxy(typeof(System_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[Serializable]
	public class LinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06001295 RID: 4757 RVA: 0x0003E6CC File Offset: 0x0003D6CC
		public LinkedList()
		{
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0003E6D4 File Offset: 0x0003D6D4
		public LinkedList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (T t in collection)
			{
				this.AddLast(t);
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003E734 File Offset: 0x0003D734
		protected LinkedList(SerializationInfo info, StreamingContext context)
		{
			this.siInfo = info;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0003E743 File Offset: 0x0003D743
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0003E74B File Offset: 0x0003D74B
		public LinkedListNode<T> First
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0003E753 File Offset: 0x0003D753
		public LinkedListNode<T> Last
		{
			get
			{
				if (this.head != null)
				{
					return this.head.prev;
				}
				return null;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0003E76A File Offset: 0x0003D76A
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0003E76D File Offset: 0x0003D76D
		void ICollection<T>.Add(T value)
		{
			this.AddLast(value);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0003E778 File Offset: 0x0003D778
		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			this.ValidateNode(node);
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
			this.InternalInsertNodeBefore(node.next, linkedListNode);
			return linkedListNode;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0003E7A7 File Offset: 0x0003D7A7
		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.ValidateNode(node);
			this.ValidateNewNode(newNode);
			this.InternalInsertNodeBefore(node.next, newNode);
			newNode.list = this;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0003E7CC File Offset: 0x0003D7CC
		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			this.ValidateNode(node);
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
			this.InternalInsertNodeBefore(node, linkedListNode);
			if (node == this.head)
			{
				this.head = linkedListNode;
			}
			return linkedListNode;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0003E806 File Offset: 0x0003D806
		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.ValidateNode(node);
			this.ValidateNewNode(newNode);
			this.InternalInsertNodeBefore(node, newNode);
			newNode.list = this;
			if (node == this.head)
			{
				this.head = newNode;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0003E838 File Offset: 0x0003D838
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(linkedListNode);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, linkedListNode);
				this.head = linkedListNode;
			}
			return linkedListNode;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0003E873 File Offset: 0x0003D873
		public void AddFirst(LinkedListNode<T> node)
		{
			this.ValidateNewNode(node);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(node);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, node);
				this.head = node;
			}
			node.list = this;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0003E8A8 File Offset: 0x0003D8A8
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(linkedListNode);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, linkedListNode);
			}
			return linkedListNode;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0003E8DC File Offset: 0x0003D8DC
		public void AddLast(LinkedListNode<T> node)
		{
			this.ValidateNewNode(node);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(node);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, node);
			}
			node.list = this;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0003E90C File Offset: 0x0003D90C
		public void Clear()
		{
			LinkedListNode<T> next = this.head;
			while (next != null)
			{
				LinkedListNode<T> linkedListNode = next;
				next = next.Next;
				linkedListNode.Invalidate();
			}
			this.head = null;
			this.count = 0;
			this.version++;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0003E950 File Offset: 0x0003D950
		public bool Contains(T value)
		{
			return this.Find(value) != null;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0003E960 File Offset: 0x0003D960
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index }));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			LinkedListNode<T> next = this.head;
			if (next != null)
			{
				do
				{
					array[index++] = next.item;
					next = next.next;
				}
				while (next != this.head);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0003E9F4 File Offset: 0x0003D9F4
		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> linkedListNode = this.head;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (linkedListNode != null)
			{
				if (value != null)
				{
					while (!@default.Equals(linkedListNode.item, value))
					{
						linkedListNode = linkedListNode.next;
						if (linkedListNode == this.head)
						{
							goto IL_005A;
						}
					}
					return linkedListNode;
				}
				while (linkedListNode.item != null)
				{
					linkedListNode = linkedListNode.next;
					if (linkedListNode == this.head)
					{
						goto IL_005A;
					}
				}
				return linkedListNode;
			}
			IL_005A:
			return null;
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0003EA5C File Offset: 0x0003DA5C
		public LinkedListNode<T> FindLast(T value)
		{
			if (this.head == null)
			{
				return null;
			}
			LinkedListNode<T> prev = this.head.prev;
			LinkedListNode<T> linkedListNode = prev;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (linkedListNode != null)
			{
				if (value != null)
				{
					while (!@default.Equals(linkedListNode.item, value))
					{
						linkedListNode = linkedListNode.prev;
						if (linkedListNode == prev)
						{
							goto IL_0061;
						}
					}
					return linkedListNode;
				}
				while (linkedListNode.item != null)
				{
					linkedListNode = linkedListNode.prev;
					if (linkedListNode == prev)
					{
						goto IL_0061;
					}
				}
				return linkedListNode;
			}
			IL_0061:
			return null;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0003EACB File Offset: 0x0003DACB
		public LinkedList<T>.Enumerator GetEnumerator()
		{
			return new LinkedList<T>.Enumerator(this);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0003EAD3 File Offset: 0x0003DAD3
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0003EAE0 File Offset: 0x0003DAE0
		public bool Remove(T value)
		{
			LinkedListNode<T> linkedListNode = this.Find(value);
			if (linkedListNode != null)
			{
				this.InternalRemoveNode(linkedListNode);
				return true;
			}
			return false;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0003EB02 File Offset: 0x0003DB02
		public void Remove(LinkedListNode<T> node)
		{
			this.ValidateNode(node);
			this.InternalRemoveNode(node);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0003EB12 File Offset: 0x0003DB12
		public void RemoveFirst()
		{
			if (this.head == null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListEmpty"));
			}
			this.InternalRemoveNode(this.head);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0003EB38 File Offset: 0x0003DB38
		public void RemoveLast()
		{
			if (this.head == null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListEmpty"));
			}
			this.InternalRemoveNode(this.head.prev);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0003EB64 File Offset: 0x0003DB64
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("Version", this.version);
			info.AddValue("Count", this.count);
			if (this.count != 0)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("Data", array, typeof(T[]));
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0003EBD4 File Offset: 0x0003DBD4
		public virtual void OnDeserialization(object sender)
		{
			if (this.siInfo == null)
			{
				return;
			}
			int @int = this.siInfo.GetInt32("Version");
			int int2 = this.siInfo.GetInt32("Count");
			if (int2 != 0)
			{
				T[] array = (T[])this.siInfo.GetValue("Data", typeof(T[]));
				if (array == null)
				{
					throw new SerializationException(SR.GetString("Serialization_MissingValues"));
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.AddLast(array[i]);
				}
			}
			else
			{
				this.head = null;
			}
			this.version = @int;
			this.siInfo = null;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0003EC78 File Offset: 0x0003DC78
		private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			newNode.next = node;
			newNode.prev = node.prev;
			node.prev.next = newNode;
			node.prev = newNode;
			this.version++;
			this.count++;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0003ECC7 File Offset: 0x0003DCC7
		private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
		{
			newNode.next = newNode;
			newNode.prev = newNode;
			this.head = newNode;
			this.version++;
			this.count++;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0003ECFC File Offset: 0x0003DCFC
		internal void InternalRemoveNode(LinkedListNode<T> node)
		{
			if (node.next == node)
			{
				this.head = null;
			}
			else
			{
				node.next.prev = node.prev;
				node.prev.next = node.next;
				if (this.head == node)
				{
					this.head = node.next;
				}
			}
			node.Invalidate();
			this.count--;
			this.version++;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0003ED74 File Offset: 0x0003DD74
		internal void ValidateNewNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.list != null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListNodeIsAttached"));
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0003ED9C File Offset: 0x0003DD9C
		internal void ValidateNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.list != this)
			{
				throw new InvalidOperationException(SR.GetString("ExternalLinkedListNode"));
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x0003EDC5 File Offset: 0x0003DDC5
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0003EDC8 File Offset: 0x0003DDC8
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

		// Token: 0x060012B9 RID: 4793 RVA: 0x0003EDEC File Offset: 0x0003DDEC
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException(SR.GetString("Arg_NonZeroLowerBound"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index }));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
			LinkedListNode<T> next = this.head;
			try
			{
				if (next != null)
				{
					do
					{
						array3[index++] = next.item;
						next = next.next;
					}
					while (next != this.head);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0003EF44 File Offset: 0x0003DF44
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040010CB RID: 4299
		private const string VersionName = "Version";

		// Token: 0x040010CC RID: 4300
		private const string CountName = "Count";

		// Token: 0x040010CD RID: 4301
		private const string ValuesName = "Data";

		// Token: 0x040010CE RID: 4302
		internal LinkedListNode<T> head;

		// Token: 0x040010CF RID: 4303
		internal int count;

		// Token: 0x040010D0 RID: 4304
		internal int version;

		// Token: 0x040010D1 RID: 4305
		private object _syncRoot;

		// Token: 0x040010D2 RID: 4306
		private SerializationInfo siInfo;

		// Token: 0x02000230 RID: 560
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
		{
			// Token: 0x060012BB RID: 4795 RVA: 0x0003EF51 File Offset: 0x0003DF51
			internal Enumerator(LinkedList<T> list)
			{
				this.list = list;
				this.version = list.version;
				this.node = list.head;
				this.current = default(T);
				this.index = 0;
				this.siInfo = null;
			}

			// Token: 0x060012BC RID: 4796 RVA: 0x0003EF8C File Offset: 0x0003DF8C
			internal Enumerator(SerializationInfo info, StreamingContext context)
			{
				this.siInfo = info;
				this.list = null;
				this.version = 0;
				this.node = null;
				this.current = default(T);
				this.index = 0;
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x060012BD RID: 4797 RVA: 0x0003EFBD File Offset: 0x0003DFBD
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x060012BE RID: 4798 RVA: 0x0003EFC5 File Offset: 0x0003DFC5
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this.list.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current;
				}
			}

			// Token: 0x060012BF RID: 4799 RVA: 0x0003EFF8 File Offset: 0x0003DFF8
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.node == null)
				{
					this.index = this.list.Count + 1;
					return false;
				}
				this.index++;
				this.current = this.node.item;
				this.node = this.node.next;
				if (this.node == this.list.head)
				{
					this.node = null;
				}
				return true;
			}

			// Token: 0x060012C0 RID: 4800 RVA: 0x0003F090 File Offset: 0x0003E090
			void IEnumerator.Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this.current = default(T);
				this.node = this.list.head;
				this.index = 0;
			}

			// Token: 0x060012C1 RID: 4801 RVA: 0x0003F0E4 File Offset: 0x0003E0E4
			public void Dispose()
			{
			}

			// Token: 0x060012C2 RID: 4802 RVA: 0x0003F0E8 File Offset: 0x0003E0E8
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("LinkedList", this.list);
				info.AddValue("Version", this.version);
				info.AddValue("Current", this.current);
				info.AddValue("Index", this.index);
			}

			// Token: 0x060012C3 RID: 4803 RVA: 0x0003F14C File Offset: 0x0003E14C
			void IDeserializationCallback.OnDeserialization(object sender)
			{
				if (this.list != null)
				{
					return;
				}
				if (this.siInfo == null)
				{
					throw new SerializationException(SR.GetString("Serialization_InvalidOnDeser"));
				}
				this.list = (LinkedList<T>)this.siInfo.GetValue("LinkedList", typeof(LinkedList<T>));
				this.version = this.siInfo.GetInt32("Version");
				this.current = (T)((object)this.siInfo.GetValue("Current", typeof(T)));
				this.index = this.siInfo.GetInt32("Index");
				if (this.list.siInfo != null)
				{
					this.list.OnDeserialization(sender);
				}
				if (this.index == this.list.Count + 1)
				{
					this.node = null;
				}
				else
				{
					this.node = this.list.First;
					if (this.node != null && this.index != 0)
					{
						for (int i = 0; i < this.index; i++)
						{
							this.node = this.node.next;
						}
						if (this.node == this.list.First)
						{
							this.node = null;
						}
					}
				}
				this.siInfo = null;
			}

			// Token: 0x040010D3 RID: 4307
			private const string LinkedListName = "LinkedList";

			// Token: 0x040010D4 RID: 4308
			private const string CurrentValueName = "Current";

			// Token: 0x040010D5 RID: 4309
			private const string VersionName = "Version";

			// Token: 0x040010D6 RID: 4310
			private const string IndexName = "Index";

			// Token: 0x040010D7 RID: 4311
			private LinkedList<T> list;

			// Token: 0x040010D8 RID: 4312
			private LinkedListNode<T> node;

			// Token: 0x040010D9 RID: 4313
			private int version;

			// Token: 0x040010DA RID: 4314
			private T current;

			// Token: 0x040010DB RID: 4315
			private int index;

			// Token: 0x040010DC RID: 4316
			private SerializationInfo siInfo;
		}
	}
}

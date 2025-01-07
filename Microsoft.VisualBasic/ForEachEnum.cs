using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.VisualBasic
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class ForEachEnum : IEnumerator, IDisposable
	{
		void IDisposable.Dispose()
		{
			if (!this.mDisposed)
			{
				this.mCollectionObject.RemoveIterator(this.WeakRef);
				this.mDisposed = true;
			}
			this.mCurrent = null;
			this.mNext = null;
		}

		public ForEachEnum(Collection coll)
		{
			this.mDisposed = false;
			this.mCollectionObject = coll;
			this.Reset();
		}

		public bool MoveNext()
		{
			if (this.mDisposed)
			{
				return false;
			}
			if (this.mAtBeginning)
			{
				this.mAtBeginning = false;
				this.mNext = this.mCollectionObject.GetFirstListNode();
			}
			if (this.mNext == null)
			{
				this.Dispose();
				return false;
			}
			this.mCurrent = this.mNext;
			if (this.mCurrent != null)
			{
				this.mNext = this.mCurrent.m_Next;
				return true;
			}
			this.Dispose();
			return false;
		}

		public void Reset()
		{
			if (this.mDisposed)
			{
				this.mCollectionObject.AddIterator(this.WeakRef);
				this.mDisposed = false;
			}
			this.mCurrent = null;
			this.mNext = null;
			this.mAtBeginning = true;
		}

		public object Current
		{
			get
			{
				if (this.mCurrent == null)
				{
					return null;
				}
				return this.mCurrent.m_Value;
			}
		}

		public void Adjust(Collection.Node Node, ForEachEnum.AdjustIndexType Type)
		{
			if (Node == null)
			{
				return;
			}
			if (this.mDisposed)
			{
				return;
			}
			switch (Type)
			{
			case ForEachEnum.AdjustIndexType.Insert:
				if (this.mCurrent != null && Node == this.mCurrent.m_Next)
				{
					this.mNext = Node;
				}
				break;
			case ForEachEnum.AdjustIndexType.Remove:
				if (Node != this.mCurrent)
				{
					if (Node == this.mNext)
					{
						this.mNext = this.mNext.m_Next;
					}
				}
				break;
			}
		}

		internal void AdjustOnListCleared()
		{
			this.mNext = null;
		}

		private bool mDisposed;

		private Collection mCollectionObject;

		private Collection.Node mCurrent;

		private Collection.Node mNext;

		private bool mAtBeginning;

		internal WeakReference WeakRef;

		internal enum AdjustIndexType
		{
			Insert,
			Remove
		}
	}
}

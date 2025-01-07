using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	public class BehaviorServiceAdornerCollectionEnumerator : IEnumerator
	{
		public BehaviorServiceAdornerCollectionEnumerator(BehaviorServiceAdornerCollection mappings)
		{
			this.temp = mappings;
			this.baseEnumerator = this.temp.GetEnumerator();
		}

		public Adorner Current
		{
			get
			{
				return (Adorner)this.baseEnumerator.Current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		bool IEnumerator.MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		void IEnumerator.Reset()
		{
			this.baseEnumerator.Reset();
		}

		private IEnumerator baseEnumerator;

		private IEnumerable temp;
	}
}

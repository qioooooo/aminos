using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml
{
	internal struct IListEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
	{
		public IListEnumerator(IList<T> sequence)
		{
			this.sequence = sequence;
			this.index = 0;
			this.current = default(T);
		}

		public void Dispose()
		{
		}

		public T Current
		{
			get
			{
				return this.current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				if (this.index == 0)
				{
					throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
				}
				if (this.index > this.sequence.Count)
				{
					throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
				}
				return this.current;
			}
		}

		public bool MoveNext()
		{
			if (this.index < this.sequence.Count)
			{
				this.current = this.sequence[this.index];
				this.index++;
				return true;
			}
			this.current = default(T);
			return false;
		}

		void IEnumerator.Reset()
		{
			this.index = 0;
			this.current = default(T);
		}

		private IList<T> sequence;

		private int index;

		private T current;
	}
}

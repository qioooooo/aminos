using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	public sealed class DesignSurfaceCollection : ICollection, IEnumerable
	{
		internal DesignSurfaceCollection(DesignerCollection designers)
		{
			this._designers = designers;
			if (this._designers == null)
			{
				this._designers = new DesignerCollection(null);
			}
		}

		public int Count
		{
			get
			{
				return this._designers.Count;
			}
		}

		public DesignSurface this[int index]
		{
			get
			{
				IDesignerHost designerHost = this._designers[index];
				DesignSurface designSurface = designerHost.GetService(typeof(DesignSurface)) as DesignSurface;
				if (designSurface == null)
				{
					throw new NotSupportedException();
				}
				return designSurface;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return new DesignSurfaceCollection.DesignSurfaceEnumerator(this._designers.GetEnumerator());
		}

		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				DesignSurface designSurface = (DesignSurface)obj;
				array.SetValue(designSurface, index++);
			}
		}

		public void CopyTo(DesignSurface[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private DesignerCollection _designers;

		private class DesignSurfaceEnumerator : IEnumerator
		{
			internal DesignSurfaceEnumerator(IEnumerator designerEnumerator)
			{
				this._designerEnumerator = designerEnumerator;
			}

			public object Current
			{
				get
				{
					IDesignerHost designerHost = (IDesignerHost)this._designerEnumerator.Current;
					DesignSurface designSurface = designerHost.GetService(typeof(DesignSurface)) as DesignSurface;
					if (designSurface == null)
					{
						throw new NotSupportedException();
					}
					return designSurface;
				}
			}

			public bool MoveNext()
			{
				return this._designerEnumerator.MoveNext();
			}

			public void Reset()
			{
				this._designerEnumerator.Reset();
			}

			private IEnumerator _designerEnumerator;
		}
	}
}

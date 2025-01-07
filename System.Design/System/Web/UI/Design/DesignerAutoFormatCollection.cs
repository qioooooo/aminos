using System;
using System.Collections;
using System.Drawing;

namespace System.Web.UI.Design
{
	public sealed class DesignerAutoFormatCollection : IList, ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				return this._autoFormats.Count;
			}
		}

		public Size PreviewSize
		{
			get
			{
				int num = 200;
				int num2 = 200;
				foreach (object obj in this._autoFormats)
				{
					DesignerAutoFormat designerAutoFormat = (DesignerAutoFormat)obj;
					int num3 = (int)designerAutoFormat.Style.Height.Value;
					if (num3 > num)
					{
						num = num3;
					}
					int num4 = (int)designerAutoFormat.Style.Width.Value;
					if (num4 > num2)
					{
						num2 = num4;
					}
				}
				return new Size(num2, num);
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public DesignerAutoFormat this[int index]
		{
			get
			{
				return (DesignerAutoFormat)this._autoFormats[index];
			}
		}

		public int Add(DesignerAutoFormat format)
		{
			return this._autoFormats.Add(format);
		}

		public void Clear()
		{
			this._autoFormats.Clear();
		}

		public bool Contains(DesignerAutoFormat format)
		{
			return this._autoFormats.Contains(format);
		}

		public int IndexOf(DesignerAutoFormat format)
		{
			return this._autoFormats.IndexOf(format);
		}

		public void Insert(int index, DesignerAutoFormat format)
		{
			this._autoFormats.Insert(index, format);
		}

		public void Remove(DesignerAutoFormat format)
		{
			this._autoFormats.Remove(format);
		}

		public void RemoveAt(int index)
		{
			this._autoFormats.RemoveAt(index);
		}

		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this._autoFormats[index];
			}
			set
			{
				if (value is DesignerAutoFormat)
				{
					this._autoFormats[index] = value;
				}
			}
		}

		int IList.Add(object value)
		{
			if (value is DesignerAutoFormat)
			{
				return this.Add((DesignerAutoFormat)value);
			}
			return -1;
		}

		bool IList.Contains(object value)
		{
			return value is DesignerAutoFormat && this.Contains((DesignerAutoFormat)value);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this._autoFormats.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._autoFormats.GetEnumerator();
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOf((DesignerAutoFormat)value);
		}

		void IList.Insert(int index, object value)
		{
			if (value is DesignerAutoFormat)
			{
				this.Insert(index, (DesignerAutoFormat)value);
			}
		}

		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		void IList.Remove(object value)
		{
			if (value is DesignerAutoFormat)
			{
				this.Remove((DesignerAutoFormat)value);
			}
		}

		private ArrayList _autoFormats = new ArrayList();
	}
}

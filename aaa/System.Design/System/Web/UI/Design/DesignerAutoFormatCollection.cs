using System;
using System.Collections;
using System.Drawing;

namespace System.Web.UI.Design
{
	// Token: 0x02000359 RID: 857
	public sealed class DesignerAutoFormatCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000B63FA File Offset: 0x000B53FA
		public int Count
		{
			get
			{
				return this._autoFormats.Count;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000B6408 File Offset: 0x000B5408
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

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000B64B0 File Offset: 0x000B54B0
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170005A8 RID: 1448
		public DesignerAutoFormat this[int index]
		{
			get
			{
				return (DesignerAutoFormat)this._autoFormats[index];
			}
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000B64C6 File Offset: 0x000B54C6
		public int Add(DesignerAutoFormat format)
		{
			return this._autoFormats.Add(format);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000B64D4 File Offset: 0x000B54D4
		public void Clear()
		{
			this._autoFormats.Clear();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000B64E1 File Offset: 0x000B54E1
		public bool Contains(DesignerAutoFormat format)
		{
			return this._autoFormats.Contains(format);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x000B64EF File Offset: 0x000B54EF
		public int IndexOf(DesignerAutoFormat format)
		{
			return this._autoFormats.IndexOf(format);
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000B64FD File Offset: 0x000B54FD
		public void Insert(int index, DesignerAutoFormat format)
		{
			this._autoFormats.Insert(index, format);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000B650C File Offset: 0x000B550C
		public void Remove(DesignerAutoFormat format)
		{
			this._autoFormats.Remove(format);
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x000B651A File Offset: 0x000B551A
		public void RemoveAt(int index)
		{
			this._autoFormats.RemoveAt(index);
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x000B6528 File Offset: 0x000B5528
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x000B6530 File Offset: 0x000B5530
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000B6533 File Offset: 0x000B5533
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000B6536 File Offset: 0x000B5536
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005AD RID: 1453
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

		// Token: 0x06002032 RID: 8242 RVA: 0x000B655E File Offset: 0x000B555E
		int IList.Add(object value)
		{
			if (value is DesignerAutoFormat)
			{
				return this.Add((DesignerAutoFormat)value);
			}
			return -1;
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000B6576 File Offset: 0x000B5576
		bool IList.Contains(object value)
		{
			return value is DesignerAutoFormat && this.Contains((DesignerAutoFormat)value);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000B658E File Offset: 0x000B558E
		void ICollection.CopyTo(Array array, int index)
		{
			this._autoFormats.CopyTo(array, index);
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000B659D File Offset: 0x000B559D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._autoFormats.GetEnumerator();
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000B65AA File Offset: 0x000B55AA
		int IList.IndexOf(object value)
		{
			return this.IndexOf((DesignerAutoFormat)value);
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000B65B8 File Offset: 0x000B55B8
		void IList.Insert(int index, object value)
		{
			if (value is DesignerAutoFormat)
			{
				this.Insert(index, (DesignerAutoFormat)value);
			}
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000B65CF File Offset: 0x000B55CF
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000B65D8 File Offset: 0x000B55D8
		void IList.Remove(object value)
		{
			if (value is DesignerAutoFormat)
			{
				this.Remove((DesignerAutoFormat)value);
			}
		}

		// Token: 0x040017CB RID: 6091
		private ArrayList _autoFormats = new ArrayList();
	}
}

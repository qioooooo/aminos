using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200043E RID: 1086
	public class HtmlWindowCollection : ICollection, IEnumerable
	{
		// Token: 0x0600414C RID: 16716 RVA: 0x000EA14F File Offset: 0x000E914F
		internal HtmlWindowCollection(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLFramesCollection2 collection)
		{
			this.htmlFramesCollection2 = collection;
			this.shimManager = shimManager;
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x000EA165 File Offset: 0x000E9165
		private UnsafeNativeMethods.IHTMLFramesCollection2 NativeHTMLFramesCollection2
		{
			get
			{
				return this.htmlFramesCollection2;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		public HtmlWindow this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidBoundArgument", new object[]
					{
						"index",
						index,
						0,
						this.Count - 1
					}));
				}
				object obj = index;
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHTMLFramesCollection2.Item(ref obj) as UnsafeNativeMethods.IHTMLWindow2;
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.shimManager, ihtmlwindow);
			}
		}

		// Token: 0x17000CA5 RID: 3237
		public HtmlWindow this[string windowId]
		{
			get
			{
				object obj = windowId;
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = null;
				try
				{
					ihtmlwindow = this.htmlFramesCollection2.Item(ref obj) as UnsafeNativeMethods.IHTMLWindow2;
				}
				catch (COMException)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "windowId", windowId }));
				}
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.shimManager, ihtmlwindow);
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x000EA268 File Offset: 0x000E9268
		public int Count
		{
			get
			{
				return this.NativeHTMLFramesCollection2.GetLength();
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x000EA275 File Offset: 0x000E9275
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x000EA278 File Offset: 0x000E9278
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x000EA27C File Offset: 0x000E927C
		void ICollection.CopyTo(Array dest, int index)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				dest.SetValue(this[i], index++);
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x000EA2B0 File Offset: 0x000E92B0
		public IEnumerator GetEnumerator()
		{
			HtmlWindow[] array = new HtmlWindow[this.Count];
			((ICollection)this).CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x04001F79 RID: 8057
		private UnsafeNativeMethods.IHTMLFramesCollection2 htmlFramesCollection2;

		// Token: 0x04001F7A RID: 8058
		private HtmlShimManager shimManager;
	}
}

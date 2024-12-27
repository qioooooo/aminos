using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000433 RID: 1075
	public sealed class HtmlElementCollection : ICollection, IEnumerable
	{
		// Token: 0x060040A9 RID: 16553 RVA: 0x000E8C38 File Offset: 0x000E7C38
		internal HtmlElementCollection(HtmlShimManager shimManager)
		{
			this.htmlElementCollection = null;
			this.elementsArray = null;
			this.shimManager = shimManager;
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x000E8C55 File Offset: 0x000E7C55
		internal HtmlElementCollection(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLElementCollection elements)
		{
			this.htmlElementCollection = elements;
			this.elementsArray = null;
			this.shimManager = shimManager;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x000E8C72 File Offset: 0x000E7C72
		internal HtmlElementCollection(HtmlShimManager shimManager, HtmlElement[] array)
		{
			this.htmlElementCollection = null;
			this.elementsArray = array;
			this.shimManager = shimManager;
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x000E8C8F File Offset: 0x000E7C8F
		private UnsafeNativeMethods.IHTMLElementCollection NativeHtmlElementCollection
		{
			get
			{
				return this.htmlElementCollection;
			}
		}

		// Token: 0x17000C75 RID: 3189
		public HtmlElement this[int index]
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
				if (this.NativeHtmlElementCollection != null)
				{
					UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlElementCollection.Item(index, 0) as UnsafeNativeMethods.IHTMLElement;
					if (ihtmlelement == null)
					{
						return null;
					}
					return new HtmlElement(this.shimManager, ihtmlelement);
				}
				else
				{
					if (this.elementsArray != null)
					{
						return this.elementsArray[index];
					}
					return null;
				}
			}
		}

		// Token: 0x17000C76 RID: 3190
		public HtmlElement this[string elementId]
		{
			get
			{
				if (this.NativeHtmlElementCollection != null)
				{
					UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlElementCollection.Item(elementId, 0) as UnsafeNativeMethods.IHTMLElement;
					if (ihtmlelement == null)
					{
						return null;
					}
					return new HtmlElement(this.shimManager, ihtmlelement);
				}
				else
				{
					if (this.elementsArray != null)
					{
						int num = this.elementsArray.Length;
						for (int i = 0; i < num; i++)
						{
							HtmlElement htmlElement = this.elementsArray[i];
							if (htmlElement.Id == elementId)
							{
								return htmlElement;
							}
						}
						return null;
					}
					return null;
				}
			}
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x000E8DBC File Offset: 0x000E7DBC
		public HtmlElementCollection GetElementsByName(string name)
		{
			int count = this.Count;
			HtmlElement[] array = new HtmlElement[count];
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				HtmlElement htmlElement = this[i];
				if (htmlElement.GetAttribute("name") == name)
				{
					array[num] = htmlElement;
					num++;
				}
			}
			if (num == 0)
			{
				return new HtmlElementCollection(this.shimManager);
			}
			HtmlElement[] array2 = new HtmlElement[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[j];
			}
			return new HtmlElementCollection(this.shimManager, array2);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x000E8E48 File Offset: 0x000E7E48
		public int Count
		{
			get
			{
				if (this.NativeHtmlElementCollection != null)
				{
					return this.NativeHtmlElementCollection.GetLength();
				}
				if (this.elementsArray != null)
				{
					return this.elementsArray.Length;
				}
				return 0;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060040B1 RID: 16561 RVA: 0x000E8E70 File Offset: 0x000E7E70
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x000E8E73 File Offset: 0x000E7E73
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x000E8E78 File Offset: 0x000E7E78
		void ICollection.CopyTo(Array dest, int index)
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				dest.SetValue(this[i], index++);
			}
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x000E8EAC File Offset: 0x000E7EAC
		public IEnumerator GetEnumerator()
		{
			HtmlElement[] array = new HtmlElement[this.Count];
			((ICollection)this).CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x04001F5A RID: 8026
		private UnsafeNativeMethods.IHTMLElementCollection htmlElementCollection;

		// Token: 0x04001F5B RID: 8027
		private HtmlElement[] elementsArray;

		// Token: 0x04001F5C RID: 8028
		private HtmlShimManager shimManager;
	}
}

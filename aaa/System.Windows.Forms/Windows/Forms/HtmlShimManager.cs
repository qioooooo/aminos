using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	// Token: 0x0200043A RID: 1082
	internal sealed class HtmlShimManager : IDisposable
	{
		// Token: 0x060040EE RID: 16622 RVA: 0x000E93C2 File Offset: 0x000E83C2
		internal HtmlShimManager()
		{
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x000E93CC File Offset: 0x000E83CC
		public void AddDocumentShim(HtmlDocument doc)
		{
			HtmlDocument.HtmlDocumentShim htmlDocumentShim = null;
			if (this.htmlDocumentShims == null)
			{
				this.htmlDocumentShims = new Dictionary<HtmlDocument, HtmlDocument.HtmlDocumentShim>();
				htmlDocumentShim = new HtmlDocument.HtmlDocumentShim(doc);
				this.htmlDocumentShims[doc] = htmlDocumentShim;
			}
			else if (!this.htmlDocumentShims.ContainsKey(doc))
			{
				htmlDocumentShim = new HtmlDocument.HtmlDocumentShim(doc);
				this.htmlDocumentShims[doc] = htmlDocumentShim;
			}
			if (htmlDocumentShim != null)
			{
				this.OnShimAdded(htmlDocumentShim);
			}
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x000E9430 File Offset: 0x000E8430
		public void AddWindowShim(HtmlWindow window)
		{
			HtmlWindow.HtmlWindowShim htmlWindowShim = null;
			if (this.htmlWindowShims == null)
			{
				this.htmlWindowShims = new Dictionary<HtmlWindow, HtmlWindow.HtmlWindowShim>();
				htmlWindowShim = new HtmlWindow.HtmlWindowShim(window);
				this.htmlWindowShims[window] = htmlWindowShim;
			}
			else if (!this.htmlWindowShims.ContainsKey(window))
			{
				htmlWindowShim = new HtmlWindow.HtmlWindowShim(window);
				this.htmlWindowShims[window] = htmlWindowShim;
			}
			if (htmlWindowShim != null)
			{
				this.OnShimAdded(htmlWindowShim);
			}
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x000E9494 File Offset: 0x000E8494
		public void AddElementShim(HtmlElement element)
		{
			HtmlElement.HtmlElementShim htmlElementShim = null;
			if (this.htmlElementShims == null)
			{
				this.htmlElementShims = new Dictionary<HtmlElement, HtmlElement.HtmlElementShim>();
				htmlElementShim = new HtmlElement.HtmlElementShim(element);
				this.htmlElementShims[element] = htmlElementShim;
			}
			else if (!this.htmlElementShims.ContainsKey(element))
			{
				htmlElementShim = new HtmlElement.HtmlElementShim(element);
				this.htmlElementShims[element] = htmlElementShim;
			}
			if (htmlElementShim != null)
			{
				this.OnShimAdded(htmlElementShim);
			}
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000E94F8 File Offset: 0x000E84F8
		internal HtmlDocument.HtmlDocumentShim GetDocumentShim(HtmlDocument document)
		{
			if (this.htmlDocumentShims == null)
			{
				return null;
			}
			if (this.htmlDocumentShims.ContainsKey(document))
			{
				return this.htmlDocumentShims[document];
			}
			return null;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x000E9520 File Offset: 0x000E8520
		internal HtmlElement.HtmlElementShim GetElementShim(HtmlElement element)
		{
			if (this.htmlElementShims == null)
			{
				return null;
			}
			if (this.htmlElementShims.ContainsKey(element))
			{
				return this.htmlElementShims[element];
			}
			return null;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x000E9548 File Offset: 0x000E8548
		internal HtmlWindow.HtmlWindowShim GetWindowShim(HtmlWindow window)
		{
			if (this.htmlWindowShims == null)
			{
				return null;
			}
			if (this.htmlWindowShims.ContainsKey(window))
			{
				return this.htmlWindowShims[window];
			}
			return null;
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x000E9570 File Offset: 0x000E8570
		private void OnShimAdded(HtmlShim addedShim)
		{
			if (addedShim != null && !(addedShim is HtmlWindow.HtmlWindowShim))
			{
				this.AddWindowShim(new HtmlWindow(this, addedShim.AssociatedWindow));
			}
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x000E9590 File Offset: 0x000E8590
		internal void OnWindowUnloaded(HtmlWindow unloadedWindow)
		{
			if (unloadedWindow != null)
			{
				if (this.htmlDocumentShims != null)
				{
					HtmlDocument.HtmlDocumentShim[] array = new HtmlDocument.HtmlDocumentShim[this.htmlDocumentShims.Count];
					this.htmlDocumentShims.Values.CopyTo(array, 0);
					foreach (HtmlDocument.HtmlDocumentShim htmlDocumentShim in array)
					{
						if (htmlDocumentShim.AssociatedWindow == unloadedWindow.NativeHtmlWindow)
						{
							this.htmlDocumentShims.Remove(htmlDocumentShim.Document);
							htmlDocumentShim.Dispose();
						}
					}
				}
				if (this.htmlElementShims != null)
				{
					HtmlElement.HtmlElementShim[] array3 = new HtmlElement.HtmlElementShim[this.htmlElementShims.Count];
					this.htmlElementShims.Values.CopyTo(array3, 0);
					foreach (HtmlElement.HtmlElementShim htmlElementShim in array3)
					{
						if (htmlElementShim.AssociatedWindow == unloadedWindow.NativeHtmlWindow)
						{
							this.htmlElementShims.Remove(htmlElementShim.Element);
							htmlElementShim.Dispose();
						}
					}
				}
				if (this.htmlWindowShims != null && this.htmlWindowShims.ContainsKey(unloadedWindow))
				{
					HtmlWindow.HtmlWindowShim htmlWindowShim = this.htmlWindowShims[unloadedWindow];
					this.htmlWindowShims.Remove(unloadedWindow);
					htmlWindowShim.Dispose();
				}
			}
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x000E96BB File Offset: 0x000E86BB
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x000E96C4 File Offset: 0x000E86C4
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.htmlElementShims != null)
				{
					foreach (HtmlElement.HtmlElementShim htmlElementShim in this.htmlElementShims.Values)
					{
						htmlElementShim.Dispose();
					}
				}
				if (this.htmlDocumentShims != null)
				{
					foreach (HtmlDocument.HtmlDocumentShim htmlDocumentShim in this.htmlDocumentShims.Values)
					{
						htmlDocumentShim.Dispose();
					}
				}
				if (this.htmlWindowShims != null)
				{
					foreach (HtmlWindow.HtmlWindowShim htmlWindowShim in this.htmlWindowShims.Values)
					{
						htmlWindowShim.Dispose();
					}
				}
				this.htmlWindowShims = null;
				this.htmlDocumentShims = null;
				this.htmlWindowShims = null;
			}
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x000E97DC File Offset: 0x000E87DC
		~HtmlShimManager()
		{
			this.Dispose(false);
		}

		// Token: 0x04001F6A RID: 8042
		private Dictionary<HtmlWindow, HtmlWindow.HtmlWindowShim> htmlWindowShims;

		// Token: 0x04001F6B RID: 8043
		private Dictionary<HtmlElement, HtmlElement.HtmlElementShim> htmlElementShims;

		// Token: 0x04001F6C RID: 8044
		private Dictionary<HtmlDocument, HtmlDocument.HtmlDocumentShim> htmlDocumentShims;
	}
}

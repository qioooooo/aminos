using System;
using System.Design;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CA RID: 970
	[ClassInterface(ClassInterfaceType.None)]
	internal class TridentSite : NativeMethods.IOleClientSite, NativeMethods.IOleDocumentSite, NativeMethods.IOleInPlaceSite, NativeMethods.IOleInPlaceFrame, NativeMethods.IDocHostUIHandler
	{
		// Token: 0x06002376 RID: 9078 RVA: 0x000BF119 File Offset: 0x000BE119
		public TridentSite(Control parent)
		{
			this.parentControl = parent;
			this.resizeHandler = new EventHandler(this.OnParentResize);
			this.parentControl.Resize += this.resizeHandler;
			this.CreateDocument();
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000BF152 File Offset: 0x000BE152
		public NativeMethods.IHTMLDocument2 GetDocument()
		{
			return this.tridentDocument;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000BF15A File Offset: 0x000BE15A
		public void Activate()
		{
			this.ActivateDocument();
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000BF164 File Offset: 0x000BE164
		protected virtual void OnParentResize(object src, EventArgs e)
		{
			if (this.tridentView != null)
			{
				NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
				NativeMethods.GetClientRect(this.parentControl.Handle, comrect);
				this.tridentView.SetRect(comrect);
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000BF19D File Offset: 0x000BE19D
		public virtual void SaveObject()
		{
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000BF19F File Offset: 0x000BE19F
		public virtual object GetMoniker(int dwAssign, int dwWhichMoniker)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000BF1B0 File Offset: 0x000BE1B0
		public virtual int GetContainer(out NativeMethods.IOleContainer ppContainer)
		{
			ppContainer = null;
			return -2147467262;
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000BF1BA File Offset: 0x000BE1BA
		public virtual void ShowObject()
		{
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000BF1BC File Offset: 0x000BE1BC
		public virtual void OnShowWindow(int fShow)
		{
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000BF1BE File Offset: 0x000BE1BE
		public virtual void RequestNewObjectLayout()
		{
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000BF1C0 File Offset: 0x000BE1C0
		public virtual int ActivateMe(NativeMethods.IOleDocumentView pViewToActivate)
		{
			if (pViewToActivate == null)
			{
				return -2147024809;
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			NativeMethods.GetClientRect(this.parentControl.Handle, comrect);
			this.tridentView = pViewToActivate;
			this.tridentView.SetInPlaceSite(this);
			this.tridentView.UIActivate(1);
			this.tridentView.SetRect(comrect);
			this.tridentView.Show(1);
			return 0;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000BF226 File Offset: 0x000BE226
		public virtual IntPtr GetWindow()
		{
			return this.parentControl.Handle;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000BF233 File Offset: 0x000BE233
		public virtual void ContextSensitiveHelp(int fEnterMode)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000BF244 File Offset: 0x000BE244
		public virtual int CanInPlaceActivate()
		{
			return 0;
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000BF247 File Offset: 0x000BE247
		public virtual void OnInPlaceActivate()
		{
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000BF249 File Offset: 0x000BE249
		public virtual void OnUIActivate()
		{
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000BF24C File Offset: 0x000BE24C
		public virtual void GetWindowContext(out NativeMethods.IOleInPlaceFrame ppFrame, out NativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
		{
			ppFrame = this;
			ppDoc = null;
			NativeMethods.GetClientRect(this.parentControl.Handle, lprcPosRect);
			NativeMethods.GetClientRect(this.parentControl.Handle, lprcClipRect);
			lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
			lpFrameInfo.fMDIApp = 0;
			lpFrameInfo.hwndFrame = this.parentControl.Handle;
			lpFrameInfo.hAccel = IntPtr.Zero;
			lpFrameInfo.cAccelEntries = 0;
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000BF2C8 File Offset: 0x000BE2C8
		public virtual int Scroll(NativeMethods.tagSIZE scrollExtant)
		{
			return -2147467263;
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000BF2CF File Offset: 0x000BE2CF
		public virtual void OnUIDeactivate(int fUndoable)
		{
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000BF2D1 File Offset: 0x000BE2D1
		public virtual void OnInPlaceDeactivate()
		{
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000BF2D3 File Offset: 0x000BE2D3
		public virtual void DiscardUndoState()
		{
			throw new COMException("Not implemented", -2147467263);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000BF2E4 File Offset: 0x000BE2E4
		public virtual void DeactivateAndUndo()
		{
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000BF2E6 File Offset: 0x000BE2E6
		public virtual int OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return 0;
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000BF2E9 File Offset: 0x000BE2E9
		public virtual void GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000BF2FA File Offset: 0x000BE2FA
		public virtual void RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000BF30B File Offset: 0x000BE30B
		public virtual void SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000BF31C File Offset: 0x000BE31C
		public virtual void SetActiveObject(NativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000BF31E File Offset: 0x000BE31E
		public virtual void InsertMenus(IntPtr hmenuShared, object lpMenuWidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000BF32F File Offset: 0x000BE32F
		public virtual void SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000BF340 File Offset: 0x000BE340
		public virtual void RemoveMenus(IntPtr hmenuShared)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000BF351 File Offset: 0x000BE351
		public virtual void SetStatusText(string pszStatusText)
		{
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000BF353 File Offset: 0x000BE353
		public virtual void EnableModeless(int fEnable)
		{
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x000BF355 File Offset: 0x000BE355
		public virtual int TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
		{
			return 1;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000BF358 File Offset: 0x000BE358
		public virtual int ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
		{
			return 0;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000BF35B File Offset: 0x000BE35B
		public virtual int GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
		{
			info.dwDoubleClick = 0;
			info.dwFlags = 149;
			return 0;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000BF370 File Offset: 0x000BE370
		public virtual int EnableModeless(bool fEnable)
		{
			return 0;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000BF373 File Offset: 0x000BE373
		public virtual int ShowUI(int dwID, NativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, NativeMethods.IOleInPlaceFrame frame, NativeMethods.IOleInPlaceUIWindow doc)
		{
			return 0;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000BF376 File Offset: 0x000BE376
		public virtual int HideUI()
		{
			return 0;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000BF379 File Offset: 0x000BE379
		public virtual int UpdateUI()
		{
			return 0;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000BF37C File Offset: 0x000BE37C
		public virtual int OnDocWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000BF383 File Offset: 0x000BE383
		public virtual int OnFrameWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000BF38A File Offset: 0x000BE38A
		public virtual int ResizeBorder(NativeMethods.COMRECT rect, NativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
		{
			return -2147467263;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000BF391 File Offset: 0x000BE391
		public virtual int GetOptionKeyPath(string[] pbstrKey, int dw)
		{
			pbstrKey[0] = null;
			return 0;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000BF398 File Offset: 0x000BE398
		public virtual int GetDropTarget(NativeMethods.IOleDropTarget pDropTarget, out NativeMethods.IOleDropTarget ppDropTarget)
		{
			ppDropTarget = null;
			return 1;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000BF39E File Offset: 0x000BE39E
		public virtual int GetExternal(out object ppDispatch)
		{
			ppDispatch = null;
			return 0;
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000BF3A4 File Offset: 0x000BE3A4
		public virtual int TranslateAccelerator(ref NativeMethods.MSG msg, ref Guid group, int nCmdID)
		{
			return 0;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000BF3A7 File Offset: 0x000BE3A7
		public virtual int TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
		{
			pstrUrlOut = null;
			return -2147467263;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000BF3B1 File Offset: 0x000BE3B1
		public virtual int FilterDataObject(global::System.Runtime.InteropServices.ComTypes.IDataObject pDO, out global::System.Runtime.InteropServices.ComTypes.IDataObject ppDORet)
		{
			ppDORet = null;
			return 0;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000BF3B8 File Offset: 0x000BE3B8
		protected void CreateDocument()
		{
			try
			{
				this.tridentDocument = (NativeMethods.IHTMLDocument2)new NativeMethods.HTMLDocument();
				this.tridentOleObject = (NativeMethods.IOleObject)this.tridentDocument;
				this.tridentOleObject.SetClientSite(this);
				NativeMethods.IPersistStreamInit persistStreamInit = (NativeMethods.IPersistStreamInit)this.tridentDocument;
				persistStreamInit.InitNew();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000BF41C File Offset: 0x000BE41C
		protected void ActivateDocument()
		{
			try
			{
				NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
				NativeMethods.GetClientRect(this.parentControl.Handle, comrect);
				this.tridentOleObject.DoVerb(-4, IntPtr.Zero, this, 0, this.parentControl.Handle, comrect);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040018A2 RID: 6306
		protected Control parentControl;

		// Token: 0x040018A3 RID: 6307
		protected NativeMethods.IOleDocumentView tridentView;

		// Token: 0x040018A4 RID: 6308
		protected NativeMethods.IOleObject tridentOleObject;

		// Token: 0x040018A5 RID: 6309
		protected NativeMethods.IHTMLDocument2 tridentDocument;

		// Token: 0x040018A6 RID: 6310
		protected EventHandler resizeHandler;
	}
}

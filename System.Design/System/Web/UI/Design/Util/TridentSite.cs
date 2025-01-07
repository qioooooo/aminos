using System;
using System.Design;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	[ClassInterface(ClassInterfaceType.None)]
	internal class TridentSite : NativeMethods.IOleClientSite, NativeMethods.IOleDocumentSite, NativeMethods.IOleInPlaceSite, NativeMethods.IOleInPlaceFrame, NativeMethods.IDocHostUIHandler
	{
		public TridentSite(Control parent)
		{
			this.parentControl = parent;
			this.resizeHandler = new EventHandler(this.OnParentResize);
			this.parentControl.Resize += this.resizeHandler;
			this.CreateDocument();
		}

		public NativeMethods.IHTMLDocument2 GetDocument()
		{
			return this.tridentDocument;
		}

		public void Activate()
		{
			this.ActivateDocument();
		}

		protected virtual void OnParentResize(object src, EventArgs e)
		{
			if (this.tridentView != null)
			{
				NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
				NativeMethods.GetClientRect(this.parentControl.Handle, comrect);
				this.tridentView.SetRect(comrect);
			}
		}

		public virtual void SaveObject()
		{
		}

		public virtual object GetMoniker(int dwAssign, int dwWhichMoniker)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual int GetContainer(out NativeMethods.IOleContainer ppContainer)
		{
			ppContainer = null;
			return -2147467262;
		}

		public virtual void ShowObject()
		{
		}

		public virtual void OnShowWindow(int fShow)
		{
		}

		public virtual void RequestNewObjectLayout()
		{
		}

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

		public virtual IntPtr GetWindow()
		{
			return this.parentControl.Handle;
		}

		public virtual void ContextSensitiveHelp(int fEnterMode)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual int CanInPlaceActivate()
		{
			return 0;
		}

		public virtual void OnInPlaceActivate()
		{
		}

		public virtual void OnUIActivate()
		{
		}

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

		public virtual int Scroll(NativeMethods.tagSIZE scrollExtant)
		{
			return -2147467263;
		}

		public virtual void OnUIDeactivate(int fUndoable)
		{
		}

		public virtual void OnInPlaceDeactivate()
		{
		}

		public virtual void DiscardUndoState()
		{
			throw new COMException("Not implemented", -2147467263);
		}

		public virtual void DeactivateAndUndo()
		{
		}

		public virtual int OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return 0;
		}

		public virtual void GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void SetActiveObject(NativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
		}

		public virtual void InsertMenus(IntPtr hmenuShared, object lpMenuWidths)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void RemoveMenus(IntPtr hmenuShared)
		{
			throw new COMException(string.Empty, -2147467263);
		}

		public virtual void SetStatusText(string pszStatusText)
		{
		}

		public virtual void EnableModeless(int fEnable)
		{
		}

		public virtual int TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
		{
			return 1;
		}

		public virtual int ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
		{
			return 0;
		}

		public virtual int GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
		{
			info.dwDoubleClick = 0;
			info.dwFlags = 149;
			return 0;
		}

		public virtual int EnableModeless(bool fEnable)
		{
			return 0;
		}

		public virtual int ShowUI(int dwID, NativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, NativeMethods.IOleInPlaceFrame frame, NativeMethods.IOleInPlaceUIWindow doc)
		{
			return 0;
		}

		public virtual int HideUI()
		{
			return 0;
		}

		public virtual int UpdateUI()
		{
			return 0;
		}

		public virtual int OnDocWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		public virtual int OnFrameWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		public virtual int ResizeBorder(NativeMethods.COMRECT rect, NativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
		{
			return -2147467263;
		}

		public virtual int GetOptionKeyPath(string[] pbstrKey, int dw)
		{
			pbstrKey[0] = null;
			return 0;
		}

		public virtual int GetDropTarget(NativeMethods.IOleDropTarget pDropTarget, out NativeMethods.IOleDropTarget ppDropTarget)
		{
			ppDropTarget = null;
			return 1;
		}

		public virtual int GetExternal(out object ppDispatch)
		{
			ppDispatch = null;
			return 0;
		}

		public virtual int TranslateAccelerator(ref NativeMethods.MSG msg, ref Guid group, int nCmdID)
		{
			return 0;
		}

		public virtual int TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
		{
			pstrUrlOut = null;
			return -2147467263;
		}

		public virtual int FilterDataObject(global::System.Runtime.InteropServices.ComTypes.IDataObject pDO, out global::System.Runtime.InteropServices.ComTypes.IDataObject ppDORet)
		{
			ppDORet = null;
			return 0;
		}

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

		protected Control parentControl;

		protected NativeMethods.IOleDocumentView tridentView;

		protected NativeMethods.IOleObject tridentOleObject;

		protected NativeMethods.IHTMLDocument2 tridentDocument;

		protected EventHandler resizeHandler;
	}
}

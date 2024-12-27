using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x02000220 RID: 544
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[Designer("System.Windows.Forms.Design.AxHostDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignTimeVisible(false)]
	[DefaultEvent("Enter")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public abstract class AxHost : Control, ISupportInitialize, ICustomTypeDescriptor
	{
		// Token: 0x060018E3 RID: 6371 RVA: 0x0002C246 File Offset: 0x0002B246
		protected AxHost(string clsid)
			: this(clsid, 0)
		{
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0002C250 File Offset: 0x0002B250
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		protected AxHost(string clsid, int flags)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("AXMTAThread", new object[] { clsid }));
			}
			this.oleSite = new AxHost.OleInterfaces(this);
			this.selectionChangeHandler = new EventHandler(this.OnNewSelection);
			this.clsid = new Guid(clsid);
			this.flags = flags;
			this.axState[AxHost.assignUniqueID] = !base.GetType().GUID.Equals(AxHost.comctlImageCombo_Clsid);
			this.axState[AxHost.needLicenseKey] = true;
			this.axState[AxHost.rejectSelection] = true;
			this.isMaskEdit = this.clsid.Equals(AxHost.maskEdit_Clsid);
			this.onContainerVisibleChanged = new EventHandler(this.OnContainerVisibleChanged);
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0002C37A File Offset: 0x0002B37A
		private bool CanUIActivate
		{
			get
			{
				return this.IsUserMode() || this.editMode != 0;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x0002C394 File Offset: 0x0002B394
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (this.axState[AxHost.fOwnWindow] && this.IsUserMode())
				{
					createParams.Style &= -268435457;
				}
				return createParams;
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0002C3D5 File Offset: 0x0002B3D5
		private bool GetAxState(int mask)
		{
			return this.axState[mask];
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0002C3E3 File Offset: 0x0002B3E3
		private void SetAxState(int mask, bool value)
		{
			this.axState[mask] = value;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0002C3F2 File Offset: 0x0002B3F2
		protected virtual void AttachInterfaces()
		{
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0002C3F4 File Offset: 0x0002B3F4
		private void RealizeStyles()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			int num = 0;
			int miscStatus = this.GetOleObject().GetMiscStatus(1, out num);
			if (!NativeMethods.Failed(miscStatus))
			{
				this.miscStatusBits = num;
				this.ParseMiscBits(this.miscStatusBits);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x0002C435 File Offset: 0x0002B435
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x0002C43D File Offset: 0x0002B43D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x0002C446 File Offset: 0x0002B446
		// (set) Token: 0x060018EE RID: 6382 RVA: 0x0002C44E File Offset: 0x0002B44E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x0002C457 File Offset: 0x0002B457
		// (set) Token: 0x060018F0 RID: 6384 RVA: 0x0002C45F File Offset: 0x0002B45F
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x0002C468 File Offset: 0x0002B468
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x0002C470 File Offset: 0x0002B470
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060018F3 RID: 6387 RVA: 0x0002C47C File Offset: 0x0002B47C
		// (remove) Token: 0x060018F4 RID: 6388 RVA: 0x0002C4A8 File Offset: 0x0002B4A8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseClick" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060018F5 RID: 6389 RVA: 0x0002C4AC File Offset: 0x0002B4AC
		// (remove) Token: 0x060018F6 RID: 6390 RVA: 0x0002C4D8 File Offset: 0x0002B4D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseDoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseDoubleClick" }));
			}
			remove
			{
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x0002C4DA File Offset: 0x0002B4DA
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x0002C4E2 File Offset: 0x0002B4E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0002C4EB File Offset: 0x0002B4EB
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x0002C4F3 File Offset: 0x0002B4F3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0002C4FC File Offset: 0x0002B4FC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x0002C507 File Offset: 0x0002B507
		// (set) Token: 0x060018FD RID: 6397 RVA: 0x0002C50F File Offset: 0x0002B50F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new virtual bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x0002C518 File Offset: 0x0002B518
		// (set) Token: 0x060018FF RID: 6399 RVA: 0x0002C520 File Offset: 0x0002B520
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x0002C529 File Offset: 0x0002B529
		// (set) Token: 0x06001901 RID: 6401 RVA: 0x0002C531 File Offset: 0x0002B531
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x0002C53C File Offset: 0x0002B53C
		// (set) Token: 0x06001903 RID: 6403 RVA: 0x0002C554 File Offset: 0x0002B554
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public new virtual bool RightToLeft
		{
			get
			{
				RightToLeft rightToLeft = base.RightToLeft;
				return rightToLeft == global::System.Windows.Forms.RightToLeft.Yes;
			}
			set
			{
				base.RightToLeft = (value ? global::System.Windows.Forms.RightToLeft.Yes : global::System.Windows.Forms.RightToLeft.No);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0002C563 File Offset: 0x0002B563
		// (set) Token: 0x06001905 RID: 6405 RVA: 0x0002C56B File Offset: 0x0002B56B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x0002C574 File Offset: 0x0002B574
		internal override bool CanAccessProperties
		{
			get
			{
				int num = this.GetOcState();
				return (this.axState[AxHost.fOwnWindow] && (num > 2 || (this.IsUserMode() && num >= 2))) || num >= 4;
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0002C5B3 File Offset: 0x0002B5B3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected bool PropsValid()
		{
			return this.CanAccessProperties;
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0002C5BB File Offset: 0x0002B5BB
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void BeginInit()
		{
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0002C5C0 File Offset: 0x0002B5C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void EndInit()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.CreateControl(true);
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl != null)
				{
					containerControl.VisibleChanged += this.onContainerVisibleChanged;
				}
			}
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0002C5F8 File Offset: 0x0002B5F8
		private void OnContainerVisibleChanged(object sender, EventArgs e)
		{
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null)
			{
				if (containerControl.Visible && base.Visible && !this.axState[AxHost.fOwnWindow])
				{
					this.MakeVisibleWithShow();
					return;
				}
				if (!containerControl.Visible && base.Visible && base.IsHandleCreated && this.GetOcState() >= 4)
				{
					this.HideAxControl();
					return;
				}
				if (containerControl.Visible && !base.GetState(2) && base.IsHandleCreated && this.GetOcState() >= 4)
				{
					this.HideAxControl();
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600190B RID: 6411 RVA: 0x0002C688 File Offset: 0x0002B688
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool EditMode
		{
			get
			{
				return this.editMode != 0;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x0002C696 File Offset: 0x0002B696
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool HasAboutBox
		{
			get
			{
				return this.aboutBoxDelegate != null;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x0002C6A4 File Offset: 0x0002B6A4
		// (set) Token: 0x0600190E RID: 6414 RVA: 0x0002C6AC File Offset: 0x0002B6AC
		private int NoComponentChangeEvents
		{
			get
			{
				return this.noComponentChange;
			}
			set
			{
				this.noComponentChange = value;
			}
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0002C6B5 File Offset: 0x0002B6B5
		public void ShowAboutBox()
		{
			if (this.aboutBoxDelegate != null)
			{
				this.aboutBoxDelegate();
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06001910 RID: 6416 RVA: 0x0002C6CC File Offset: 0x0002B6CC
		// (remove) Token: 0x06001911 RID: 6417 RVA: 0x0002C6F8 File Offset: 0x0002B6F8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "BackColorChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06001912 RID: 6418 RVA: 0x0002C6FC File Offset: 0x0002B6FC
		// (remove) Token: 0x06001913 RID: 6419 RVA: 0x0002C728 File Offset: 0x0002B728
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "BackgroundImageChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06001914 RID: 6420 RVA: 0x0002C72C File Offset: 0x0002B72C
		// (remove) Token: 0x06001915 RID: 6421 RVA: 0x0002C758 File Offset: 0x0002B758
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "BackgroundImageLayoutChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06001916 RID: 6422 RVA: 0x0002C75C File Offset: 0x0002B75C
		// (remove) Token: 0x06001917 RID: 6423 RVA: 0x0002C788 File Offset: 0x0002B788
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BindingContextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "BindingContextChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06001918 RID: 6424 RVA: 0x0002C78C File Offset: 0x0002B78C
		// (remove) Token: 0x06001919 RID: 6425 RVA: 0x0002C7B8 File Offset: 0x0002B7B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ContextMenuChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "ContextMenuChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x0600191A RID: 6426 RVA: 0x0002C7BC File Offset: 0x0002B7BC
		// (remove) Token: 0x0600191B RID: 6427 RVA: 0x0002C7E8 File Offset: 0x0002B7E8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "CursorChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x0600191C RID: 6428 RVA: 0x0002C7EC File Offset: 0x0002B7EC
		// (remove) Token: 0x0600191D RID: 6429 RVA: 0x0002C818 File Offset: 0x0002B818
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "EnabledChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x0600191E RID: 6430 RVA: 0x0002C81C File Offset: 0x0002B81C
		// (remove) Token: 0x0600191F RID: 6431 RVA: 0x0002C848 File Offset: 0x0002B848
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler FontChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "FontChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06001920 RID: 6432 RVA: 0x0002C84C File Offset: 0x0002B84C
		// (remove) Token: 0x06001921 RID: 6433 RVA: 0x0002C878 File Offset: 0x0002B878
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "ForeColorChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06001922 RID: 6434 RVA: 0x0002C87C File Offset: 0x0002B87C
		// (remove) Token: 0x06001923 RID: 6435 RVA: 0x0002C8A8 File Offset: 0x0002B8A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "RightToLeftChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06001924 RID: 6436 RVA: 0x0002C8AC File Offset: 0x0002B8AC
		// (remove) Token: 0x06001925 RID: 6437 RVA: 0x0002C8D8 File Offset: 0x0002B8D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "TextChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06001926 RID: 6438 RVA: 0x0002C8DC File Offset: 0x0002B8DC
		// (remove) Token: 0x06001927 RID: 6439 RVA: 0x0002C908 File Offset: 0x0002B908
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler Click
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "Click" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06001928 RID: 6440 RVA: 0x0002C90C File Offset: 0x0002B90C
		// (remove) Token: 0x06001929 RID: 6441 RVA: 0x0002C938 File Offset: 0x0002B938
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "DragDrop" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600192A RID: 6442 RVA: 0x0002C93C File Offset: 0x0002B93C
		// (remove) Token: 0x0600192B RID: 6443 RVA: 0x0002C968 File Offset: 0x0002B968
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "DragEnter" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x0600192C RID: 6444 RVA: 0x0002C96C File Offset: 0x0002B96C
		// (remove) Token: 0x0600192D RID: 6445 RVA: 0x0002C998 File Offset: 0x0002B998
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event DragEventHandler DragOver
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "DragOver" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600192E RID: 6446 RVA: 0x0002C99C File Offset: 0x0002B99C
		// (remove) Token: 0x0600192F RID: 6447 RVA: 0x0002C9C8 File Offset: 0x0002B9C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DragLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "DragLeave" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06001930 RID: 6448 RVA: 0x0002C9CC File Offset: 0x0002B9CC
		// (remove) Token: 0x06001931 RID: 6449 RVA: 0x0002C9F8 File Offset: 0x0002B9F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "GiveFeedback" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06001932 RID: 6450 RVA: 0x0002C9FC File Offset: 0x0002B9FC
		// (remove) Token: 0x06001933 RID: 6451 RVA: 0x0002CA28 File Offset: 0x0002BA28
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event HelpEventHandler HelpRequested
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "HelpRequested" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06001934 RID: 6452 RVA: 0x0002CA2C File Offset: 0x0002BA2C
		// (remove) Token: 0x06001935 RID: 6453 RVA: 0x0002CA58 File Offset: 0x0002BA58
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event PaintEventHandler Paint
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "Paint" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06001936 RID: 6454 RVA: 0x0002CA5C File Offset: 0x0002BA5C
		// (remove) Token: 0x06001937 RID: 6455 RVA: 0x0002CA88 File Offset: 0x0002BA88
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "QueryContinueDrag" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06001938 RID: 6456 RVA: 0x0002CA8C File Offset: 0x0002BA8C
		// (remove) Token: 0x06001939 RID: 6457 RVA: 0x0002CAB8 File Offset: 0x0002BAB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "QueryAccessibilityHelp" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x0600193A RID: 6458 RVA: 0x0002CABC File Offset: 0x0002BABC
		// (remove) Token: 0x0600193B RID: 6459 RVA: 0x0002CAE8 File Offset: 0x0002BAE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "DoubleClick" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x0600193C RID: 6460 RVA: 0x0002CAEC File Offset: 0x0002BAEC
		// (remove) Token: 0x0600193D RID: 6461 RVA: 0x0002CB18 File Offset: 0x0002BB18
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "ImeModeChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x0600193E RID: 6462 RVA: 0x0002CB1C File Offset: 0x0002BB1C
		// (remove) Token: 0x0600193F RID: 6463 RVA: 0x0002CB48 File Offset: 0x0002BB48
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "KeyDown" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06001940 RID: 6464 RVA: 0x0002CB4C File Offset: 0x0002BB4C
		// (remove) Token: 0x06001941 RID: 6465 RVA: 0x0002CB78 File Offset: 0x0002BB78
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "KeyPress" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06001942 RID: 6466 RVA: 0x0002CB7C File Offset: 0x0002BB7C
		// (remove) Token: 0x06001943 RID: 6467 RVA: 0x0002CBA8 File Offset: 0x0002BBA8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "KeyUp" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06001944 RID: 6468 RVA: 0x0002CBAC File Offset: 0x0002BBAC
		// (remove) Token: 0x06001945 RID: 6469 RVA: 0x0002CBD8 File Offset: 0x0002BBD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event LayoutEventHandler Layout
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "Layout" }));
			}
			remove
			{
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06001946 RID: 6470 RVA: 0x0002CBDC File Offset: 0x0002BBDC
		// (remove) Token: 0x06001947 RID: 6471 RVA: 0x0002CC08 File Offset: 0x0002BC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseDown" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06001948 RID: 6472 RVA: 0x0002CC0C File Offset: 0x0002BC0C
		// (remove) Token: 0x06001949 RID: 6473 RVA: 0x0002CC38 File Offset: 0x0002BC38
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseEnter" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x0600194A RID: 6474 RVA: 0x0002CC3C File Offset: 0x0002BC3C
		// (remove) Token: 0x0600194B RID: 6475 RVA: 0x0002CC68 File Offset: 0x0002BC68
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseLeave" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600194C RID: 6476 RVA: 0x0002CC6C File Offset: 0x0002BC6C
		// (remove) Token: 0x0600194D RID: 6477 RVA: 0x0002CC98 File Offset: 0x0002BC98
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseHover
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseHover" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600194E RID: 6478 RVA: 0x0002CC9C File Offset: 0x0002BC9C
		// (remove) Token: 0x0600194F RID: 6479 RVA: 0x0002CCC8 File Offset: 0x0002BCC8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseMove" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06001950 RID: 6480 RVA: 0x0002CCCC File Offset: 0x0002BCCC
		// (remove) Token: 0x06001951 RID: 6481 RVA: 0x0002CCF8 File Offset: 0x0002BCF8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseUp" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06001952 RID: 6482 RVA: 0x0002CCFC File Offset: 0x0002BCFC
		// (remove) Token: 0x06001953 RID: 6483 RVA: 0x0002CD28 File Offset: 0x0002BD28
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseWheel
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "MouseWheel" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001954 RID: 6484 RVA: 0x0002CD2C File Offset: 0x0002BD2C
		// (remove) Token: 0x06001955 RID: 6485 RVA: 0x0002CD58 File Offset: 0x0002BD58
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "ChangeUICues" }));
			}
			remove
			{
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001956 RID: 6486 RVA: 0x0002CD5C File Offset: 0x0002BD5C
		// (remove) Token: 0x06001957 RID: 6487 RVA: 0x0002CD88 File Offset: 0x0002BD88
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler StyleChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[] { "StyleChanged" }));
			}
			remove
			{
			}
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0002CD8A File Offset: 0x0002BD8A
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AmbientChanged(-703);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0002CD9E File Offset: 0x0002BD9E
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.AmbientChanged(-704);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0002CDB2 File Offset: 0x0002BDB2
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.AmbientChanged(-701);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0002CDC8 File Offset: 0x0002BDC8
		private void AmbientChanged(int dispid)
		{
			if (this.GetOcx() != null)
			{
				try
				{
					base.Invalidate();
					this.GetOleControl().OnAmbientPropertyChange(dispid);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0002CE08 File Offset: 0x0002BE08
		private bool OwnWindow()
		{
			return this.axState[AxHost.fOwnWindow] || this.axState[AxHost.fFakingWindow];
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0002CE2E File Offset: 0x0002BE2E
		private IntPtr GetHandleNoCreate()
		{
			if (base.IsHandleCreated)
			{
				return base.Handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0002CE44 File Offset: 0x0002BE44
		private ISelectionService GetSelectionService()
		{
			return AxHost.GetSelectionService(this);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0002CE4C File Offset: 0x0002BE4C
		private static ISelectionService GetSelectionService(Control ctl)
		{
			ISite site = ctl.Site;
			if (site != null)
			{
				object service = site.GetService(typeof(ISelectionService));
				return service as ISelectionService;
			}
			return null;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0002CE7C File Offset: 0x0002BE7C
		private void AddSelectionHandler()
		{
			if (this.axState[AxHost.addedSelectionHandler])
			{
				return;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				selectionService.SelectionChanging += this.selectionChangeHandler;
			}
			this.axState[AxHost.addedSelectionHandler] = true;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0002CEC4 File Offset: 0x0002BEC4
		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (e.Component == this)
			{
				UnsafeNativeMethods.IOleControl oleControl = this.GetOcx() as UnsafeNativeMethods.IOleControl;
				if (oleControl != null)
				{
					oleControl.OnAmbientPropertyChange(-702);
				}
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0002CEF8 File Offset: 0x0002BEF8
		private bool RemoveSelectionHandler()
		{
			if (!this.axState[AxHost.addedSelectionHandler])
			{
				return false;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				selectionService.SelectionChanging -= this.selectionChangeHandler;
			}
			this.axState[AxHost.addedSelectionHandler] = false;
			return true;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0002CF44 File Offset: 0x0002BF44
		private void SyncRenameNotification(bool hook)
		{
			if (base.DesignMode && hook != this.axState[AxHost.renameEventHooked])
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					if (hook)
					{
						componentChangeService.ComponentRename += this.OnComponentRename;
					}
					else
					{
						componentChangeService.ComponentRename -= this.OnComponentRename;
					}
					this.axState[AxHost.renameEventHooked] = hook;
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (set) Token: 0x06001964 RID: 6500 RVA: 0x0002CFC0 File Offset: 0x0002BFC0
		public override ISite Site
		{
			set
			{
				if (this.axState[AxHost.disposed])
				{
					return;
				}
				bool flag = this.RemoveSelectionHandler();
				bool flag2 = this.IsUserMode();
				this.SyncRenameNotification(false);
				base.Site = value;
				bool flag3 = this.IsUserMode();
				if (!flag3)
				{
					this.GetOcxCreate();
				}
				if (flag)
				{
					this.AddSelectionHandler();
				}
				this.SyncRenameNotification(value != null);
				if (value != null && !flag3 && flag2 != flag3 && this.GetOcState() > 1)
				{
					this.TransitionDownTo(1);
					this.TransitionUpTo(4);
					ContainerControl containerControl = this.ContainingControl;
					if (containerControl != null && containerControl.Visible && base.Visible)
					{
						this.MakeVisibleWithShow();
					}
				}
				if (flag2 != flag3 && !base.IsHandleCreated && !this.axState[AxHost.disposed] && this.GetOcx() != null)
				{
					this.RealizeStyles();
				}
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0002D094 File Offset: 0x0002C094
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLostFocus(EventArgs e)
		{
			bool flag = this.GetHandleNoCreate() != this.hwndFocus;
			if (flag && base.IsHandleCreated)
			{
				flag = !UnsafeNativeMethods.IsChild(new HandleRef(this, this.GetHandleNoCreate()), new HandleRef(null, this.hwndFocus));
			}
			base.OnLostFocus(e);
			if (flag)
			{
				this.UiDeactivate();
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0002D0F0 File Offset: 0x0002C0F0
		private void OnNewSelection(object sender, EventArgs e)
		{
			if (this.IsUserMode())
			{
				return;
			}
			ISelectionService selectionService = this.GetSelectionService();
			if (selectionService != null)
			{
				if (this.GetOcState() >= 8 && !selectionService.GetComponentSelected(this))
				{
					int num = this.UiDeactivate();
					NativeMethods.Failed(num);
				}
				if (!selectionService.GetComponentSelected(this))
				{
					if (this.editMode != 0)
					{
						this.GetParentContainer().OnExitEditMode(this);
						this.editMode = 0;
					}
					this.SetSelectionStyle(1);
					this.RemoveSelectionHandler();
					return;
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
				{
					int num2 = (int)propertyDescriptor.GetValue(this);
					if (num2 != this.selectionStyle)
					{
						propertyDescriptor.SetValue(this, this.selectionStyle);
					}
				}
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0002D1B4 File Offset: 0x0002C1B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0002D1C0 File Offset: 0x0002C1C0
		protected override void CreateHandle()
		{
			if (!base.IsHandleCreated)
			{
				this.TransitionUpTo(2);
				if (!this.axState[AxHost.fOwnWindow])
				{
					if (this.axState[AxHost.fNeedOwnWindow])
					{
						this.axState[AxHost.fNeedOwnWindow] = false;
						this.axState[AxHost.fFakingWindow] = true;
						base.CreateHandle();
					}
					else
					{
						this.TransitionUpTo(4);
						if (this.axState[AxHost.fNeedOwnWindow])
						{
							this.CreateHandle();
							return;
						}
					}
				}
				else
				{
					base.SetState(2, false);
					base.CreateHandle();
				}
				this.GetParentContainer().ControlCreated(this);
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0002D267 File Offset: 0x0002C267
		private NativeMethods.COMRECT GetClipRect(NativeMethods.COMRECT clipRect)
		{
			if (clipRect != null)
			{
				AxHost.FillInRect(clipRect, new Rectangle(0, 0, 32000, 32000));
			}
			return clipRect;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0002D288 File Offset: 0x0002C288
		private static int SetupLogPixels(bool force)
		{
			if (AxHost.logPixelsX == -1 || force)
			{
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				if (dc == IntPtr.Zero)
				{
					return -2147467259;
				}
				AxHost.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
				AxHost.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
			return 0;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0002D2F8 File Offset: 0x0002C2F8
		private void HiMetric2Pixel(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			pointl.x = sz.cx;
			pointl.y = sz.cy;
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			((UnsafeNativeMethods.IOleControlSite)this.oleSite).TransformCoords(pointl, tagPOINTF, 6);
			szout.cx = (int)tagPOINTF.x;
			szout.cy = (int)tagPOINTF.y;
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0002D354 File Offset: 0x0002C354
		private void Pixel2hiMetric(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			tagPOINTF.x = (float)sz.cx;
			tagPOINTF.y = (float)sz.cy;
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			((UnsafeNativeMethods.IOleControlSite)this.oleSite).TransformCoords(pointl, tagPOINTF, 10);
			szout.cx = pointl.x;
			szout.cy = pointl.y;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0002D3B0 File Offset: 0x0002C3B0
		private static int Pixel2Twip(int v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = (xDirection ? AxHost.logPixelsX : AxHost.logPixelsY);
			return (int)((double)v / (double)num * 72.0 * 20.0);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0002D3F0 File Offset: 0x0002C3F0
		private static int Twip2Pixel(double v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = (xDirection ? AxHost.logPixelsX : AxHost.logPixelsY);
			return (int)(v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0002D430 File Offset: 0x0002C430
		private static int Twip2Pixel(int v, bool xDirection)
		{
			AxHost.SetupLogPixels(false);
			int num = (xDirection ? AxHost.logPixelsX : AxHost.logPixelsY);
			return (int)((double)v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0002D470 File Offset: 0x0002C470
		private Size SetExtent(int width, int height)
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			tagSIZEL.cx = width;
			tagSIZEL.cy = height;
			bool flag = !this.IsUserMode();
			try
			{
				this.Pixel2hiMetric(tagSIZEL, tagSIZEL);
				this.GetOleObject().SetExtent(1, tagSIZEL);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this.GetOleObject().GetExtent(1, tagSIZEL);
				try
				{
					this.GetOleObject().SetExtent(1, tagSIZEL);
				}
				catch (COMException)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0002D500 File Offset: 0x0002C500
		private Size GetExtent()
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			this.GetOleObject().GetExtent(1, tagSIZEL);
			this.HiMetric2Pixel(tagSIZEL, tagSIZEL);
			return new Size(tagSIZEL.cx, tagSIZEL.cy);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0002D53A File Offset: 0x0002C53A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			return bounds;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0002D53D File Offset: 0x0002C53D
		private void SetObjectRects(Rectangle bounds)
		{
			if (this.GetOcState() < 4)
			{
				return;
			}
			this.GetInPlaceObject().SetObjectRects(AxHost.FillInRect(new NativeMethods.COMRECT(), bounds), this.GetClipRect(new NativeMethods.COMRECT()));
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0002D56C File Offset: 0x0002C56C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.GetAxState(AxHost.handlePosRectChanged))
			{
				return;
			}
			this.axState[AxHost.handlePosRectChanged] = true;
			Size size = base.ApplySizeConstraints(width, height);
			width = size.Width;
			height = size.Height;
			try
			{
				if (this.axState[AxHost.fFakingWindow])
				{
					base.SetBoundsCore(x, y, width, height, specified);
				}
				else
				{
					Rectangle bounds = base.Bounds;
					if (bounds.X != x || bounds.Y != y || bounds.Width != width || bounds.Height != height)
					{
						if (!base.IsHandleCreated)
						{
							base.UpdateBounds(x, y, width, height);
						}
						else
						{
							if (this.GetOcState() > 2)
							{
								this.CheckSubclassing();
								if (width != bounds.Width || height != bounds.Height)
								{
									Size size2 = this.SetExtent(width, height);
									width = size2.Width;
									height = size2.Height;
								}
							}
							if (this.axState[AxHost.manualUpdate])
							{
								this.SetObjectRects(new Rectangle(x, y, width, height));
								this.CheckSubclassing();
								base.UpdateBounds();
							}
							else
							{
								this.SetObjectRects(new Rectangle(x, y, width, height));
								base.SetBoundsCore(x, y, width, height, specified);
								base.Invalidate();
							}
						}
					}
				}
			}
			finally
			{
				this.axState[AxHost.handlePosRectChanged] = false;
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0002D6EC File Offset: 0x0002C6EC
		private bool CheckSubclassing()
		{
			if (!base.IsHandleCreated || this.wndprocAddr == IntPtr.Zero)
			{
				return true;
			}
			IntPtr handle = base.Handle;
			IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
			if (windowLong == this.wndprocAddr)
			{
				return true;
			}
			if ((int)base.SendMessage(this.REGMSG_MSG, 0, 0) == 123)
			{
				this.wndprocAddr = windowLong;
				return true;
			}
			base.WindowReleaseHandle();
			UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -4, new HandleRef(this, windowLong));
			base.WindowAssignHandle(handle, this.axState[AxHost.assignUniqueID]);
			this.InformOfNewHandle();
			this.axState[AxHost.manualUpdate] = true;
			return false;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0002D7A6 File Offset: 0x0002C7A6
		protected override void DestroyHandle()
		{
			if (this.axState[AxHost.fOwnWindow])
			{
				base.DestroyHandle();
				return;
			}
			if (base.IsHandleCreated)
			{
				this.TransitionDownTo(2);
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0002D7D0 File Offset: 0x0002C7D0
		private void TransitionDownTo(int state)
		{
			if (this.axState[AxHost.inTransition])
			{
				return;
			}
			try
			{
				this.axState[AxHost.inTransition] = true;
				while (state < this.GetOcState())
				{
					int num = this.GetOcState();
					switch (num)
					{
					case 1:
						this.ReleaseAxControl();
						this.SetOcState(0);
						continue;
					case 2:
						this.StopEvents();
						this.DisposeAxControl();
						this.SetOcState(1);
						continue;
					case 3:
						break;
					case 4:
						if (this.axState[AxHost.fFakingWindow])
						{
							this.DestroyFakeWindow();
							this.SetOcState(2);
						}
						else
						{
							this.InPlaceDeactivate();
						}
						this.SetOcState(2);
						continue;
					default:
						if (num == 8)
						{
							this.UiDeactivate();
							this.SetOcState(4);
							continue;
						}
						if (num == 16)
						{
							this.SetOcState(8);
							continue;
						}
						break;
					}
					this.SetOcState(this.GetOcState() - 1);
				}
			}
			finally
			{
				this.axState[AxHost.inTransition] = false;
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0002D8DC File Offset: 0x0002C8DC
		private void TransitionUpTo(int state)
		{
			if (this.axState[AxHost.inTransition])
			{
				return;
			}
			try
			{
				this.axState[AxHost.inTransition] = true;
				while (state > this.GetOcState())
				{
					switch (this.GetOcState())
					{
					case 0:
						this.axState[AxHost.disposed] = false;
						this.GetOcxCreate();
						this.SetOcState(1);
						continue;
					case 1:
						this.ActivateAxControl();
						this.SetOcState(2);
						if (this.IsUserMode())
						{
							this.StartEvents();
							continue;
						}
						continue;
					case 2:
						this.axState[AxHost.ownDisposing] = false;
						if (!this.axState[AxHost.fOwnWindow])
						{
							this.InPlaceActivate();
							if (!base.Visible && this.ContainingControl != null && this.ContainingControl.Visible)
							{
								this.HideAxControl();
							}
							else
							{
								base.CreateControl(true);
								if (!this.IsUserMode() && !this.axState[AxHost.ocxStateSet])
								{
									Size extent = this.GetExtent();
									Rectangle bounds = base.Bounds;
									if (bounds.Size.Equals(this.DefaultSize) && !bounds.Size.Equals(extent))
									{
										bounds.Width = extent.Width;
										bounds.Height = extent.Height;
										base.Bounds = bounds;
									}
								}
							}
						}
						if (this.GetOcState() < 4)
						{
							this.SetOcState(4);
						}
						this.OnInPlaceActive();
						continue;
					case 4:
						this.DoVerb(-1);
						this.SetOcState(8);
						continue;
					}
					this.SetOcState(this.GetOcState() + 1);
				}
			}
			finally
			{
				this.axState[AxHost.inTransition] = false;
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0002DADC File Offset: 0x0002CADC
		protected virtual void OnInPlaceActive()
		{
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0002DAE0 File Offset: 0x0002CAE0
		private void InPlaceActivate()
		{
			try
			{
				this.DoVerb(-5);
			}
			catch (Exception ex)
			{
				throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[] { base.GetType().Name }), ex);
			}
			this.EnsureWindowPresent();
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0002DB38 File Offset: 0x0002CB38
		private void InPlaceDeactivate()
		{
			this.axState[AxHost.ownDisposing] = true;
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null && containerControl.ActiveControl == this)
			{
				containerControl.ActiveControl = null;
			}
			try
			{
				this.GetInPlaceObject().InPlaceDeactivate();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0002DB90 File Offset: 0x0002CB90
		private void UiActivate()
		{
			if (this.CanUIActivate)
			{
				this.DoVerb(-4);
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0002DBA2 File Offset: 0x0002CBA2
		private void DestroyFakeWindow()
		{
			this.axState[AxHost.fFakingWindow] = false;
			base.DestroyHandle();
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0002DBBC File Offset: 0x0002CBBC
		private void EnsureWindowPresent()
		{
			if (!base.IsHandleCreated)
			{
				try
				{
					((UnsafeNativeMethods.IOleClientSite)this.oleSite).ShowObject();
				}
				catch
				{
				}
			}
			if (base.IsHandleCreated)
			{
				return;
			}
			if (this.ParentInternal != null)
			{
				throw new InvalidOperationException(SR.GetString("AXNohWnd", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0002DC2C File Offset: 0x0002CC2C
		protected override void SetVisibleCore(bool value)
		{
			if (base.GetState(2) != value)
			{
				bool visible = base.Visible;
				if ((base.IsHandleCreated || value) && this.ParentInternal != null && this.ParentInternal.Created && !this.axState[AxHost.fOwnWindow])
				{
					this.TransitionUpTo(2);
					if (value)
					{
						if (this.axState[AxHost.fFakingWindow])
						{
							this.DestroyFakeWindow();
						}
						if (!base.IsHandleCreated)
						{
							try
							{
								this.SetExtent(base.Width, base.Height);
								this.InPlaceActivate();
								base.CreateControl(true);
								goto IL_00AF;
							}
							catch
							{
								this.MakeVisibleWithShow();
								goto IL_00AF;
							}
						}
						this.MakeVisibleWithShow();
					}
					else
					{
						this.HideAxControl();
					}
				}
				IL_00AF:
				if (!value)
				{
					this.axState[AxHost.fNeedOwnWindow] = false;
				}
				if (!this.axState[AxHost.fOwnWindow])
				{
					base.SetState(2, value);
					if (base.Visible != visible)
					{
						this.OnVisibleChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0002DD3C File Offset: 0x0002CD3C
		private void MakeVisibleWithShow()
		{
			ContainerControl containerControl = this.ContainingControl;
			Control control = ((containerControl == null) ? null : containerControl.ActiveControl);
			try
			{
				this.DoVerb(-1);
			}
			catch (Exception ex)
			{
				throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[] { base.GetType().Name }), ex);
			}
			this.EnsureWindowPresent();
			base.CreateControl(true);
			if (containerControl != null && containerControl.ActiveControl != control)
			{
				containerControl.ActiveControl = control;
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0002DDC0 File Offset: 0x0002CDC0
		private void HideAxControl()
		{
			this.DoVerb(-3);
			if (this.GetOcState() < 4)
			{
				this.axState[AxHost.fNeedOwnWindow] = true;
				this.SetOcState(4);
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0002DDEB File Offset: 0x0002CDEB
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0002DDEE File Offset: 0x0002CDEE
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return !this.ignoreDialogKeys && base.ProcessDialogKey(keyData);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0002DE04 File Offset: 0x0002CE04
		public override bool PreProcessMessage(ref Message msg)
		{
			if (this.IsUserMode())
			{
				if (this.axState[AxHost.siteProcessedInputKey])
				{
					return base.PreProcessMessage(ref msg);
				}
				NativeMethods.MSG msg2 = default(NativeMethods.MSG);
				msg2.message = msg.Msg;
				msg2.wParam = msg.WParam;
				msg2.lParam = msg.LParam;
				msg2.hwnd = msg.HWnd;
				this.axState[AxHost.siteProcessedInputKey] = false;
				try
				{
					UnsafeNativeMethods.IOleInPlaceActiveObject inPlaceActiveObject = this.GetInPlaceActiveObject();
					if (inPlaceActiveObject != null)
					{
						int num = inPlaceActiveObject.TranslateAccelerator(ref msg2);
						msg.Msg = msg2.message;
						msg.WParam = msg2.wParam;
						msg.LParam = msg2.lParam;
						msg.HWnd = msg2.hwnd;
						if (num == 0)
						{
							return true;
						}
						if (num == 1)
						{
							bool flag = false;
							this.ignoreDialogKeys = true;
							try
							{
								flag = base.PreProcessMessage(ref msg);
							}
							finally
							{
								this.ignoreDialogKeys = false;
							}
							return flag;
						}
						if (this.axState[AxHost.siteProcessedInputKey])
						{
							return base.PreProcessMessage(ref msg);
						}
						return false;
					}
				}
				finally
				{
					this.axState[AxHost.siteProcessedInputKey] = false;
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0002DF50 File Offset: 0x0002CF50
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.CanSelect)
			{
				try
				{
					NativeMethods.tagCONTROLINFO tagCONTROLINFO = new NativeMethods.tagCONTROLINFO();
					int controlInfo = this.GetOleControl().GetControlInfo(tagCONTROLINFO);
					if (NativeMethods.Failed(controlInfo))
					{
						return false;
					}
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					msg.hwnd = ((this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle);
					msg.message = 260;
					msg.wParam = (IntPtr)((int)char.ToUpper(charCode, CultureInfo.CurrentCulture));
					msg.lParam = (IntPtr)538443777;
					msg.time = SafeNativeMethods.GetTickCount();
					NativeMethods.POINT point = new NativeMethods.POINT();
					UnsafeNativeMethods.GetCursorPos(point);
					msg.pt_x = point.x;
					msg.pt_y = point.y;
					if (SafeNativeMethods.IsAccelerator(new HandleRef(tagCONTROLINFO, tagCONTROLINFO.hAccel), (int)tagCONTROLINFO.cAccel, ref msg, null))
					{
						this.GetOleControl().OnMnemonic(ref msg);
						base.Focus();
						return true;
					}
				}
				catch (Exception)
				{
					return false;
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0002E070 File Offset: 0x0002D070
		protected void SetAboutBoxDelegate(AxHost.AboutBoxDelegate d)
		{
			this.aboutBoxDelegate = (AxHost.AboutBoxDelegate)Delegate.Combine(this.aboutBoxDelegate, d);
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0002E089 File Offset: 0x0002D089
		// (set) Token: 0x06001988 RID: 6536 RVA: 0x0002E0B4 File Offset: 0x0002D0B4
		[RefreshProperties(RefreshProperties.All)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
		[Browsable(false)]
		public AxHost.State OcxState
		{
			get
			{
				if (this.IsDirty() || this.ocxState == null)
				{
					this.ocxState = this.CreateNewOcxState(this.ocxState);
				}
				return this.ocxState;
			}
			set
			{
				this.axState[AxHost.ocxStateSet] = true;
				if (value == null)
				{
					return;
				}
				if (this.storageType != -1 && this.storageType != value.type)
				{
					throw new InvalidOperationException(SR.GetString("AXOcxStateLoaded"));
				}
				if (this.ocxState == value)
				{
					return;
				}
				this.ocxState = value;
				if (this.ocxState != null)
				{
					this.axState[AxHost.manualUpdate] = this.ocxState._GetManualUpdate();
					this.licenseKey = this.ocxState._GetLicenseKey();
				}
				else
				{
					this.axState[AxHost.manualUpdate] = false;
					this.licenseKey = null;
				}
				if (this.ocxState != null && this.GetOcState() >= 2)
				{
					this.DepersistControl();
				}
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0002E174 File Offset: 0x0002D174
		private AxHost.State CreateNewOcxState(AxHost.State oldOcxState)
		{
			this.NoComponentChangeEvents++;
			try
			{
				if (this.GetOcState() < 2)
				{
					return null;
				}
				try
				{
					AxHost.PropertyBagStream propertyBagStream = null;
					if (this.iPersistPropBag != null)
					{
						propertyBagStream = new AxHost.PropertyBagStream();
						this.iPersistPropBag.Save(propertyBagStream, true, true);
					}
					switch (this.storageType)
					{
					case 0:
					case 1:
					{
						MemoryStream memoryStream = new MemoryStream();
						if (this.storageType == 0)
						{
							this.iPersistStream.Save(new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream), true);
						}
						else
						{
							this.iPersistStreamInit.Save(new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream), true);
						}
						if (memoryStream != null)
						{
							return new AxHost.State(memoryStream, this.storageType, this, propertyBagStream);
						}
						if (propertyBagStream != null)
						{
							return new AxHost.State(propertyBagStream);
						}
						break;
					}
					case 2:
						if (oldOcxState != null)
						{
							return oldOcxState.RefreshStorage(this.iPersistStorage);
						}
						return null;
					default:
						return null;
					}
				}
				catch (Exception)
				{
				}
			}
			finally
			{
				this.NoComponentChangeEvents--;
			}
			return null;
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x0002E280 File Offset: 0x0002D280
		// (set) Token: 0x0600198B RID: 6539 RVA: 0x0002E2A6 File Offset: 0x0002D2A6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public ContainerControl ContainingControl
		{
			get
			{
				IntSecurity.GetParent.Demand();
				if (this.containingControl == null)
				{
					this.containingControl = this.FindContainerControlInternal();
				}
				return this.containingControl;
			}
			set
			{
				this.containingControl = value;
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0002E2B0 File Offset: 0x0002D2B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeText()
		{
			bool flag = false;
			try
			{
				flag = this.Text.Length != 0;
			}
			catch (COMException)
			{
			}
			return flag;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0002E2E8 File Offset: 0x0002D2E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeContainingControl()
		{
			return this.ContainingControl != this.ParentInternal;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0002E2FC File Offset: 0x0002D2FC
		private ContainerControl FindContainerControlInternal()
		{
			if (this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ContainerControl containerControl = designerHost.RootComponent as ContainerControl;
					if (containerControl != null)
					{
						return containerControl;
					}
				}
			}
			ContainerControl containerControl2 = null;
			for (Control control = this; control != null; control = control.ParentInternal)
			{
				ContainerControl containerControl3 = control as ContainerControl;
				if (containerControl3 != null)
				{
					containerControl2 = containerControl3;
					break;
				}
			}
			return containerControl2;
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0002E364 File Offset: 0x0002D364
		private bool IsDirty()
		{
			if (this.GetOcState() < 2)
			{
				return false;
			}
			if (this.axState[AxHost.valueChanged])
			{
				this.axState[AxHost.valueChanged] = false;
				return true;
			}
			int num;
			switch (this.storageType)
			{
			case 0:
				num = this.iPersistStream.IsDirty();
				break;
			case 1:
				num = this.iPersistStreamInit.IsDirty();
				break;
			case 2:
				num = this.iPersistStorage.IsDirty();
				break;
			default:
				return true;
			}
			return num != 1 && (!NativeMethods.Failed(num) || true);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0002E400 File Offset: 0x0002D400
		internal bool IsUserMode()
		{
			ISite site = this.Site;
			return site == null || !site.DesignMode;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0002E424 File Offset: 0x0002D424
		private object GetAmbientProperty(int dispid)
		{
			Control parentInternal = this.ParentInternal;
			if (dispid != -732)
			{
				switch (dispid)
				{
				case -715:
					return true;
				case -713:
					return false;
				case -712:
					return false;
				case -711:
					return false;
				case -710:
					return false;
				case -709:
					return this.IsUserMode();
				case -706:
					return true;
				case -705:
					return Thread.CurrentThread.CurrentCulture.LCID;
				case -704:
					if (parentInternal != null)
					{
						return AxHost.GetOleColorFromColor(parentInternal.ForeColor);
					}
					return null;
				case -703:
					if (parentInternal != null)
					{
						return AxHost.GetIFontFromFont(parentInternal.Font);
					}
					return null;
				case -702:
				{
					string text = this.GetParentContainer().GetNameForControl(this);
					if (text == null)
					{
						text = "";
					}
					return text;
				}
				case -701:
					if (parentInternal != null)
					{
						return AxHost.GetOleColorFromColor(parentInternal.BackColor);
					}
					return null;
				}
				return null;
			}
			Control control = this;
			while (control != null)
			{
				if (control.RightToLeft == global::System.Windows.Forms.RightToLeft.No)
				{
					return false;
				}
				if (control.RightToLeft == global::System.Windows.Forms.RightToLeft.Yes)
				{
					return true;
				}
				if (control.RightToLeft == global::System.Windows.Forms.RightToLeft.Inherit)
				{
					control = control.Parent;
				}
			}
			return null;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0002E56C File Offset: 0x0002D56C
		public void DoVerb(int verb)
		{
			Control parentInternal = this.ParentInternal;
			this.GetOleObject().DoVerb(verb, IntPtr.Zero, this.oleSite, -1, (parentInternal != null) ? parentInternal.Handle : IntPtr.Zero, AxHost.FillInRect(new NativeMethods.COMRECT(), base.Bounds));
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0002E5B9 File Offset: 0x0002D5B9
		private bool AwaitingDefreezing()
		{
			return this.freezeCount > 0;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0002E5C4 File Offset: 0x0002D5C4
		private void Freeze(bool v)
		{
			if (v)
			{
				try
				{
					this.GetOleControl().FreezeEvents(-1);
				}
				catch (COMException)
				{
				}
				this.freezeCount++;
				return;
			}
			try
			{
				this.GetOleControl().FreezeEvents(0);
			}
			catch (COMException)
			{
			}
			this.freezeCount--;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0002E634 File Offset: 0x0002D634
		private int UiDeactivate()
		{
			bool flag = this.axState[AxHost.ownDisposing];
			this.axState[AxHost.ownDisposing] = true;
			int num = 0;
			try
			{
				num = this.GetInPlaceObject().UIDeactivate();
			}
			finally
			{
				this.axState[AxHost.ownDisposing] = flag;
			}
			return num;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0002E698 File Offset: 0x0002D698
		private int GetOcState()
		{
			return this.ocState;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0002E6A0 File Offset: 0x0002D6A0
		private void SetOcState(int nv)
		{
			this.ocState = nv;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0002E6A9 File Offset: 0x0002D6A9
		private string GetLicenseKey()
		{
			return this.GetLicenseKey(this.clsid);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0002E6B8 File Offset: 0x0002D6B8
		private string GetLicenseKey(Guid clsid)
		{
			if (this.licenseKey != null || !this.axState[AxHost.needLicenseKey])
			{
				return this.licenseKey;
			}
			try
			{
				UnsafeNativeMethods.IClassFactory2 classFactory = UnsafeNativeMethods.CoGetClassObject(ref clsid, 1, 0, ref AxHost.icf2_Guid);
				NativeMethods.tagLICINFO tagLICINFO = new NativeMethods.tagLICINFO();
				classFactory.GetLicInfo(tagLICINFO);
				if (tagLICINFO.fRuntimeAvailable != 0)
				{
					string[] array = new string[1];
					classFactory.RequestLicKey(0, array);
					this.licenseKey = array[0];
					return this.licenseKey;
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == AxHost.E_NOINTERFACE.ErrorCode)
				{
					return null;
				}
				this.axState[AxHost.needLicenseKey] = false;
			}
			catch (Exception)
			{
				this.axState[AxHost.needLicenseKey] = false;
			}
			return null;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0002E790 File Offset: 0x0002D790
		private void CreateWithoutLicense(Guid clsid)
		{
			object obj = UnsafeNativeMethods.CoCreateInstance(ref clsid, null, 1, ref NativeMethods.ActiveX.IID_IUnknown);
			this.instance = obj;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0002E7B8 File Offset: 0x0002D7B8
		private void CreateWithLicense(string license, Guid clsid)
		{
			if (license != null)
			{
				try
				{
					UnsafeNativeMethods.IClassFactory2 classFactory = UnsafeNativeMethods.CoGetClassObject(ref clsid, 1, 0, ref AxHost.icf2_Guid);
					if (classFactory != null)
					{
						classFactory.CreateInstanceLic(null, null, ref NativeMethods.ActiveX.IID_IUnknown, license, out this.instance);
					}
				}
				catch (Exception)
				{
				}
			}
			if (this.instance == null)
			{
				this.CreateWithoutLicense(clsid);
			}
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0002E814 File Offset: 0x0002D814
		private void CreateInstance()
		{
			try
			{
				this.instance = this.CreateInstanceCore(this.clsid);
			}
			catch (ExternalException ex)
			{
				if (ex.ErrorCode == -2147221230)
				{
					throw new LicenseException(base.GetType(), this, SR.GetString("AXNoLicenseToUse"));
				}
				throw;
			}
			this.SetOcState(1);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0002E874 File Offset: 0x0002D874
		protected virtual object CreateInstanceCore(Guid clsid)
		{
			if (this.IsUserMode())
			{
				this.CreateWithLicense(this.licenseKey, clsid);
			}
			else
			{
				this.CreateWithoutLicense(clsid);
			}
			return this.instance;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0002E89C File Offset: 0x0002D89C
		private CategoryAttribute GetCategoryForDispid(int dispid)
		{
			NativeMethods.ICategorizeProperties categorizeProperties = this.GetCategorizeProperties();
			if (categorizeProperties == null)
			{
				return null;
			}
			int num = 0;
			try
			{
				categorizeProperties.MapPropertyToCategory(dispid, ref num);
				if (num != 0)
				{
					int num2 = -num;
					if (num2 > 0 && num2 < AxHost.categoryNames.Length && AxHost.categoryNames[num2] != null)
					{
						return AxHost.categoryNames[num2];
					}
					num2 = -num2;
					int num3 = num2;
					if (this.objectDefinedCategoryNames != null)
					{
						CategoryAttribute categoryAttribute = (CategoryAttribute)this.objectDefinedCategoryNames[num3];
						if (categoryAttribute != null)
						{
							return categoryAttribute;
						}
					}
					string text = null;
					if (categorizeProperties.GetCategoryName(num2, CultureInfo.CurrentCulture.LCID, out text) == 0 && text != null)
					{
						CategoryAttribute categoryAttribute = new CategoryAttribute(text);
						if (this.objectDefinedCategoryNames == null)
						{
							this.objectDefinedCategoryNames = new Hashtable();
						}
						this.objectDefinedCategoryNames.Add(num3, categoryAttribute);
						return categoryAttribute;
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0002E98C File Offset: 0x0002D98C
		private void SetSelectionStyle(int selectionStyle)
		{
			if (!this.IsUserMode())
			{
				ISelectionService selectionService = this.GetSelectionService();
				this.selectionStyle = selectionStyle;
				if (selectionService != null && selectionService.GetComponentSelected(this))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						propertyDescriptor.SetValue(this, selectionStyle);
					}
				}
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0002E9F0 File Offset: 0x0002D9F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void InvokeEditMode()
		{
			if (this.editMode != 0)
			{
				return;
			}
			this.AddSelectionHandler();
			this.editMode = 2;
			this.SetSelectionStyle(2);
			UnsafeNativeMethods.GetFocus();
			try
			{
				this.UiActivate();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0002EA3C File Offset: 0x0002DA3C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			if (!this.axState[AxHost.editorRefresh] && this.HasPropertyPages())
			{
				this.axState[AxHost.editorRefresh] = true;
				TypeDescriptor.Refresh(base.GetType());
			}
			return TypeDescriptor.GetAttributes(this, true);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0002EA7B File Offset: 0x0002DA7B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0002EA7E File Offset: 0x0002DA7E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0002EA81 File Offset: 0x0002DA81
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0002EA84 File Offset: 0x0002DA84
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0002EA8D File Offset: 0x0002DA8D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0002EA98 File Offset: 0x0002DA98
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			if (editorBaseType != typeof(ComponentEditor))
			{
				return null;
			}
			if (this.editor != null)
			{
				return this.editor;
			}
			if (this.editor == null && this.HasPropertyPages())
			{
				this.editor = new AxHost.AxComponentEditor();
			}
			return this.editor;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0002EAE4 File Offset: 0x0002DAE4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0002EAED File Offset: 0x0002DAED
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0002EAF7 File Offset: 0x0002DAF7
		private void OnIdle(object sender, EventArgs e)
		{
			if (this.axState[AxHost.refreshProperties])
			{
				TypeDescriptor.Refresh(base.GetType());
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0002EB16 File Offset: 0x0002DB16
		// (set) Token: 0x060019AC RID: 6572 RVA: 0x0002EB28 File Offset: 0x0002DB28
		private bool RefreshAllProperties
		{
			get
			{
				return this.axState[AxHost.refreshProperties];
			}
			set
			{
				this.axState[AxHost.refreshProperties] = value;
				if (value && !this.axState[AxHost.listeningToIdle])
				{
					Application.Idle += this.OnIdle;
					this.axState[AxHost.listeningToIdle] = true;
					return;
				}
				if (!value && this.axState[AxHost.listeningToIdle])
				{
					Application.Idle -= this.OnIdle;
					this.axState[AxHost.listeningToIdle] = false;
				}
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0002EBB8 File Offset: 0x0002DBB8
		private PropertyDescriptorCollection FillProperties(Attribute[] attributes)
		{
			if (this.RefreshAllProperties)
			{
				this.RefreshAllProperties = false;
				this.propsStash = null;
				this.attribsStash = null;
			}
			else if (this.propsStash != null)
			{
				if (attributes == null && this.attribsStash == null)
				{
					return this.propsStash;
				}
				if (attributes != null && this.attribsStash != null && attributes.Length == this.attribsStash.Length)
				{
					bool flag = true;
					int num = 0;
					foreach (Attribute attribute in attributes)
					{
						if (!attribute.Equals(this.attribsStash[num++]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return this.propsStash;
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			if (this.properties == null)
			{
				this.properties = new Hashtable();
			}
			if (this.propertyInfos == null)
			{
				this.propertyInfos = new Hashtable();
				PropertyInfo[] array = base.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in array)
				{
					this.propertyInfos.Add(propertyInfo.Name, propertyInfo);
				}
			}
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(this, null, true);
			if (propertyDescriptorCollection != null)
			{
				for (int k = 0; k < propertyDescriptorCollection.Count; k++)
				{
					if (propertyDescriptorCollection[k].DesignTimeOnly)
					{
						arrayList.Add(propertyDescriptorCollection[k]);
					}
					else
					{
						string name = propertyDescriptorCollection[k].Name;
						PropertyInfo propertyInfo2 = (PropertyInfo)this.propertyInfos[name];
						if (propertyInfo2 == null || propertyInfo2.CanRead)
						{
							if (!this.properties.ContainsKey(name))
							{
								PropertyDescriptor propertyDescriptor;
								if (propertyInfo2 != null)
								{
									propertyDescriptor = new AxHost.AxPropertyDescriptor(propertyDescriptorCollection[k], this);
									((AxHost.AxPropertyDescriptor)propertyDescriptor).UpdateAttributes();
								}
								else
								{
									propertyDescriptor = propertyDescriptorCollection[k];
								}
								this.properties.Add(name, propertyDescriptor);
								arrayList.Add(propertyDescriptor);
							}
							else
							{
								PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)this.properties[name];
								AxHost.AxPropertyDescriptor axPropertyDescriptor = propertyDescriptor2 as AxHost.AxPropertyDescriptor;
								if ((propertyInfo2 != null || axPropertyDescriptor == null) && (propertyInfo2 == null || axPropertyDescriptor != null))
								{
									if (axPropertyDescriptor != null)
									{
										axPropertyDescriptor.UpdateAttributes();
									}
									arrayList.Add(propertyDescriptor2);
								}
							}
						}
					}
				}
				if (attributes != null)
				{
					Attribute attribute2 = null;
					foreach (Attribute attribute3 in attributes)
					{
						if (attribute3 is BrowsableAttribute)
						{
							attribute2 = attribute3;
						}
					}
					if (attribute2 != null)
					{
						ArrayList arrayList2 = null;
						foreach (object obj in arrayList)
						{
							PropertyDescriptor propertyDescriptor3 = (PropertyDescriptor)obj;
							if (propertyDescriptor3 is AxHost.AxPropertyDescriptor)
							{
								Attribute attribute4 = propertyDescriptor3.Attributes[typeof(BrowsableAttribute)];
								if (attribute4 != null && !attribute4.Equals(attribute2))
								{
									if (arrayList2 == null)
									{
										arrayList2 = new ArrayList();
									}
									arrayList2.Add(propertyDescriptor3);
								}
							}
						}
						if (arrayList2 != null)
						{
							foreach (object obj2 in arrayList2)
							{
								arrayList.Remove(obj2);
							}
						}
					}
				}
			}
			PropertyDescriptor[] array3 = new PropertyDescriptor[arrayList.Count];
			arrayList.CopyTo(array3, 0);
			this.propsStash = new PropertyDescriptorCollection(array3);
			this.attribsStash = attributes;
			return this.propsStash;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0002EF2C File Offset: 0x0002DF2C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.FillProperties(null);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0002EF35 File Offset: 0x0002DF35
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return this.FillProperties(attributes);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0002EF3E File Offset: 0x0002DF3E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0002EF44 File Offset: 0x0002DF44
		private AxHost.AxPropertyDescriptor GetPropertyDescriptorFromDispid(int dispid)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = this.FillProperties(null);
			foreach (object obj in propertyDescriptorCollection)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				AxHost.AxPropertyDescriptor axPropertyDescriptor = propertyDescriptor as AxHost.AxPropertyDescriptor;
				if (axPropertyDescriptor != null && axPropertyDescriptor.Dispid == dispid)
				{
					return axPropertyDescriptor;
				}
			}
			return null;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0002EFBC File Offset: 0x0002DFBC
		private void ActivateAxControl()
		{
			if (this.QuickActivate())
			{
				this.DepersistControl();
			}
			else
			{
				this.SlowActivate();
			}
			this.SetOcState(2);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0002EFDB File Offset: 0x0002DFDB
		private void DepersistFromIPropertyBag(UnsafeNativeMethods.IPropertyBag propBag)
		{
			this.iPersistPropBag.Load(propBag, null);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0002EFEA File Offset: 0x0002DFEA
		private void DepersistFromIStream(UnsafeNativeMethods.IStream istream)
		{
			this.storageType = 0;
			this.iPersistStream.Load(istream);
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0002EFFF File Offset: 0x0002DFFF
		private void DepersistFromIStreamInit(UnsafeNativeMethods.IStream istream)
		{
			this.storageType = 1;
			this.iPersistStreamInit.Load(istream);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0002F014 File Offset: 0x0002E014
		private void DepersistFromIStorage(UnsafeNativeMethods.IStorage storage)
		{
			this.storageType = 2;
			if (storage != null)
			{
				int num = this.iPersistStorage.Load(storage);
			}
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0002F03C File Offset: 0x0002E03C
		private void DepersistControl()
		{
			this.Freeze(true);
			if (this.ocxState != null)
			{
				switch (this.ocxState.Type)
				{
				case 0:
					try
					{
						this.iPersistStream = (UnsafeNativeMethods.IPersistStream)this.instance;
						this.DepersistFromIStream(this.ocxState.GetStream());
						goto IL_01D0;
					}
					catch (Exception)
					{
						goto IL_01D0;
					}
					break;
				case 1:
					break;
				case 2:
					try
					{
						this.iPersistStorage = (UnsafeNativeMethods.IPersistStorage)this.instance;
						this.DepersistFromIStorage(this.ocxState.GetStorage());
						goto IL_01D0;
					}
					catch (Exception)
					{
						goto IL_01D0;
					}
					goto IL_01C0;
				default:
					goto IL_01C0;
				}
				if (this.instance is UnsafeNativeMethods.IPersistStreamInit)
				{
					try
					{
						this.iPersistStreamInit = (UnsafeNativeMethods.IPersistStreamInit)this.instance;
						this.DepersistFromIStreamInit(this.ocxState.GetStream());
					}
					catch (Exception)
					{
					}
					this.GetControlEnabled();
					goto IL_01D0;
				}
				this.ocxState.Type = 0;
				this.DepersistControl();
				return;
				IL_01C0:
				throw new InvalidOperationException(SR.GetString("UnableToInitComponent"));
				IL_01D0:
				if (this.ocxState.GetPropBag() != null)
				{
					try
					{
						this.iPersistPropBag = (UnsafeNativeMethods.IPersistPropertyBag)this.instance;
						this.DepersistFromIPropertyBag(this.ocxState.GetPropBag());
					}
					catch (Exception)
					{
					}
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStreamInit)
			{
				this.iPersistStreamInit = (UnsafeNativeMethods.IPersistStreamInit)this.instance;
				try
				{
					this.storageType = 1;
					this.iPersistStreamInit.InitNew();
				}
				catch (Exception)
				{
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStream)
			{
				this.storageType = 0;
				this.iPersistStream = (UnsafeNativeMethods.IPersistStream)this.instance;
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistStorage)
			{
				this.storageType = 2;
				this.ocxState = new AxHost.State(this);
				this.iPersistStorage = (UnsafeNativeMethods.IPersistStorage)this.instance;
				try
				{
					this.iPersistStorage.InitNew(this.ocxState.GetStorage());
				}
				catch (Exception)
				{
				}
				return;
			}
			if (this.instance is UnsafeNativeMethods.IPersistPropertyBag)
			{
				this.iPersistPropBag = (UnsafeNativeMethods.IPersistPropertyBag)this.instance;
				try
				{
					this.iPersistPropBag.InitNew();
				}
				catch (Exception)
				{
				}
			}
			throw new InvalidOperationException(SR.GetString("UnableToInitComponent"));
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0002F2A8 File Offset: 0x0002E2A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object GetOcx()
		{
			return this.instance;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0002F2B0 File Offset: 0x0002E2B0
		private object GetOcxCreate()
		{
			if (this.instance == null)
			{
				this.CreateInstance();
				this.RealizeStyles();
				this.AttachInterfaces();
				this.oleSite.OnOcxCreate();
			}
			return this.instance;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0002F2E0 File Offset: 0x0002E2E0
		private void StartEvents()
		{
			if (!this.axState[AxHost.sinkAttached])
			{
				try
				{
					this.CreateSink();
					this.oleSite.StartEvents();
				}
				catch (Exception)
				{
				}
				this.axState[AxHost.sinkAttached] = true;
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0002F338 File Offset: 0x0002E338
		private void StopEvents()
		{
			if (this.axState[AxHost.sinkAttached])
			{
				try
				{
					this.DetachSink();
				}
				catch (Exception)
				{
				}
				this.axState[AxHost.sinkAttached] = false;
			}
			this.oleSite.StopEvents();
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0002F390 File Offset: 0x0002E390
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void CreateSink()
		{
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0002F392 File Offset: 0x0002E392
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void DetachSink()
		{
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0002F394 File Offset: 0x0002E394
		private bool CanShowPropertyPages()
		{
			return this.GetOcState() >= 2 && this.GetOcx() is NativeMethods.ISpecifyPropertyPages;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0002F3B0 File Offset: 0x0002E3B0
		public bool HasPropertyPages()
		{
			if (!this.CanShowPropertyPages())
			{
				return false;
			}
			NativeMethods.ISpecifyPropertyPages specifyPropertyPages = (NativeMethods.ISpecifyPropertyPages)this.GetOcx();
			try
			{
				NativeMethods.tagCAUUID tagCAUUID = new NativeMethods.tagCAUUID();
				try
				{
					specifyPropertyPages.GetPages(tagCAUUID);
					if (tagCAUUID.cElems > 0)
					{
						return true;
					}
				}
				finally
				{
					if (tagCAUUID.pElems != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(tagCAUUID.pElems);
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0002F434 File Offset: 0x0002E434
		private unsafe void ShowPropertyPageForDispid(int dispid, Guid guid)
		{
			try
			{
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this.GetOcx());
				UnsafeNativeMethods.OleCreatePropertyFrameIndirect(new NativeMethods.OCPFIPARAMS
				{
					hwndOwner = ((this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle),
					lpszCaption = base.Name,
					ppUnk = (IntPtr)(&iunknownForObject),
					uuid = (IntPtr)(&guid),
					dispidInitial = dispid
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0002F4C0 File Offset: 0x0002E4C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void MakeDirty()
		{
			ISite site = this.Site;
			if (site == null)
			{
				return;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
			if (componentChangeService == null)
			{
				return;
			}
			componentChangeService.OnComponentChanging(this, null);
			componentChangeService.OnComponentChanged(this, null, null, null);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0002F504 File Offset: 0x0002E504
		public void ShowPropertyPages()
		{
			if (this.ParentInternal == null)
			{
				return;
			}
			if (!this.ParentInternal.IsHandleCreated)
			{
				return;
			}
			this.ShowPropertyPages(this.ParentInternal);
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0002F52C File Offset: 0x0002E52C
		public void ShowPropertyPages(Control control)
		{
			try
			{
				if (this.CanShowPropertyPages())
				{
					NativeMethods.ISpecifyPropertyPages specifyPropertyPages = (NativeMethods.ISpecifyPropertyPages)this.GetOcx();
					NativeMethods.tagCAUUID tagCAUUID = new NativeMethods.tagCAUUID();
					try
					{
						specifyPropertyPages.GetPages(tagCAUUID);
						if (tagCAUUID.cElems <= 0)
						{
							return;
						}
					}
					catch
					{
						return;
					}
					IDesignerHost designerHost = null;
					if (this.Site != null)
					{
						designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
					}
					DesignerTransaction designerTransaction = null;
					try
					{
						if (designerHost != null)
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("AXEditProperties"));
						}
						string text = null;
						object ocx = this.GetOcx();
						IntPtr intPtr = ((this.ContainingControl == null) ? IntPtr.Zero : this.ContainingControl.Handle);
						SafeNativeMethods.OleCreatePropertyFrame(new HandleRef(this, intPtr), 0, 0, text, 1, ref ocx, tagCAUUID.cElems, new HandleRef(null, tagCAUUID.pElems), Application.CurrentCulture.LCID, 0, IntPtr.Zero);
					}
					finally
					{
						if (this.oleSite != null)
						{
							((UnsafeNativeMethods.IPropertyNotifySink)this.oleSite).OnChanged(-1);
						}
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
						}
						if (tagCAUUID.pElems != IntPtr.Zero)
						{
							Marshal.FreeCoTaskMem(tagCAUUID.pElems);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0002F6A0 File Offset: 0x0002E6A0
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (this.isMaskEdit)
			{
				return base.InitializeDCForWmCtlColor(dc, msg);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0002F6B8 File Offset: 0x0002E6B8
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 83)
			{
				if (msg <= 21)
				{
					if (msg != 2)
					{
						if (msg == 8)
						{
							this.hwndFocus = m.WParam;
							try
							{
								base.WndProc(ref m);
								return;
							}
							finally
							{
								this.hwndFocus = IntPtr.Zero;
							}
							goto IL_0100;
						}
						switch (msg)
						{
						case 20:
						case 21:
							break;
						default:
							goto IL_01D7;
						}
					}
					else
					{
						if (this.GetOcState() >= 4)
						{
							UnsafeNativeMethods.IOleInPlaceObject inPlaceObject = this.GetInPlaceObject();
							IntPtr intPtr;
							if (NativeMethods.Succeeded(inPlaceObject.GetWindow(out intPtr)))
							{
								Application.ParkHandle(new HandleRef(inPlaceObject, intPtr));
							}
						}
						bool state = base.GetState(2);
						this.TransitionDownTo(2);
						this.DetachAndForward(ref m);
						if (state != base.GetState(2))
						{
							base.SetState(2, state);
							return;
						}
						return;
					}
				}
				else if (msg != 32 && msg != 43)
				{
					if (msg != 83)
					{
						goto IL_01D7;
					}
					base.WndProc(ref m);
					this.DefWndProc(ref m);
					return;
				}
			}
			else if (msg <= 257)
			{
				if (msg != 123)
				{
					if (msg != 130)
					{
						if (msg != 257)
						{
							goto IL_01D7;
						}
						if (this.axState[AxHost.processingKeyUp])
						{
							return;
						}
						this.axState[AxHost.processingKeyUp] = true;
						try
						{
							if (base.PreProcessControlMessage(ref m) != PreProcessControlState.MessageProcessed)
							{
								this.DefWndProc(ref m);
							}
							return;
						}
						finally
						{
							this.axState[AxHost.processingKeyUp] = false;
						}
					}
					this.DetachAndForward(ref m);
					return;
				}
				this.DefWndProc(ref m);
				return;
			}
			else
			{
				if (msg == 273)
				{
					goto IL_0100;
				}
				switch (msg)
				{
				case 513:
				case 516:
				case 519:
					if (this.IsUserMode())
					{
						base.Focus();
					}
					this.DefWndProc(ref m);
					return;
				case 514:
				case 515:
				case 517:
				case 518:
				case 520:
				case 521:
					break;
				default:
					if (msg != 8277)
					{
						goto IL_01D7;
					}
					break;
				}
			}
			this.DefWndProc(ref m);
			return;
			IL_0100:
			if (!Control.ReflectMessageInternal(m.LParam, ref m))
			{
				this.DefWndProc(ref m);
				return;
			}
			return;
			IL_01D7:
			if (m.Msg == this.REGMSG_MSG)
			{
				m.Result = (IntPtr)123;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0002F8DC File Offset: 0x0002E8DC
		private void DetachAndForward(ref Message m)
		{
			IntPtr handleNoCreate = this.GetHandleNoCreate();
			this.DetachWindow();
			if (handleNoCreate != IntPtr.Zero)
			{
				IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handleNoCreate), -4);
				m.Result = UnsafeNativeMethods.CallWindowProc(windowLong, handleNoCreate, m.Msg, m.WParam, m.LParam);
			}
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0002F934 File Offset: 0x0002E934
		private void DetachWindow()
		{
			if (base.IsHandleCreated)
			{
				this.OnHandleDestroyed(EventArgs.Empty);
				for (Control control = this; control != null; control = control.ParentInternal)
				{
				}
				base.WindowReleaseHandle();
			}
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0002F968 File Offset: 0x0002E968
		private void InformOfNewHandle()
		{
			for (Control control = this; control != null; control = control.ParentInternal)
			{
			}
			this.wndprocAddr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -4);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x0002F99C File Offset: 0x0002E99C
		private void AttachWindow(IntPtr hwnd)
		{
			if (!this.axState[AxHost.fFakingWindow])
			{
				base.WindowAssignHandle(hwnd, this.axState[AxHost.assignUniqueID]);
			}
			base.UpdateZOrder();
			Size size = base.Size;
			base.UpdateBounds();
			Size extent = this.GetExtent();
			Point location = base.Location;
			if (size.Width < extent.Width || size.Height < extent.Height)
			{
				base.Bounds = new Rectangle(location.X, location.Y, extent.Width, extent.Height);
			}
			else
			{
				Size size2 = this.SetExtent(size.Width, size.Height);
				if (!size2.Equals(size))
				{
					base.Bounds = new Rectangle(location.X, location.Y, size2.Width, size2.Height);
				}
			}
			this.OnHandleCreated(EventArgs.Empty);
			this.InformOfNewHandle();
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0002FA9E File Offset: 0x0002EA9E
		protected override void OnHandleCreated(EventArgs e)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			base.SetAcceptDrops(this.AllowDrop);
			base.RaiseCreateHandleEvent(e);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0002FACA File Offset: 0x0002EACA
		private int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0002FAD9 File Offset: 0x0002EAD9
		private int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0002FAEC File Offset: 0x0002EAEC
		private bool QuickActivate()
		{
			if (!(this.instance is UnsafeNativeMethods.IQuickActivate))
			{
				return false;
			}
			UnsafeNativeMethods.IQuickActivate quickActivate = (UnsafeNativeMethods.IQuickActivate)this.instance;
			UnsafeNativeMethods.tagQACONTAINER tagQACONTAINER = new UnsafeNativeMethods.tagQACONTAINER();
			UnsafeNativeMethods.tagQACONTROL tagQACONTROL = new UnsafeNativeMethods.tagQACONTROL();
			tagQACONTAINER.pClientSite = this.oleSite;
			tagQACONTAINER.pPropertyNotifySink = this.oleSite;
			tagQACONTAINER.pFont = AxHost.GetIFontFromFont(this.GetParentContainer().parent.Font);
			tagQACONTAINER.dwAppearance = 0;
			tagQACONTAINER.lcid = Application.CurrentCulture.LCID;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				tagQACONTAINER.colorFore = AxHost.GetOleColorFromColor(parentInternal.ForeColor);
				tagQACONTAINER.colorBack = AxHost.GetOleColorFromColor(parentInternal.BackColor);
			}
			else
			{
				tagQACONTAINER.colorFore = AxHost.GetOleColorFromColor(SystemColors.WindowText);
				tagQACONTAINER.colorBack = AxHost.GetOleColorFromColor(SystemColors.Window);
			}
			tagQACONTAINER.dwAmbientFlags = 224;
			if (this.IsUserMode())
			{
				tagQACONTAINER.dwAmbientFlags |= 4;
			}
			try
			{
				quickActivate.QuickActivate(tagQACONTAINER, tagQACONTROL);
			}
			catch (Exception)
			{
				this.DisposeAxControl();
				return false;
			}
			this.miscStatusBits = tagQACONTROL.dwMiscStatus;
			this.ParseMiscBits(this.miscStatusBits);
			return true;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x0002FC1C File Offset: 0x0002EC1C
		internal override void DisposeAxControls()
		{
			this.axState[AxHost.rejectSelection] = true;
			base.DisposeAxControls();
			this.TransitionDownTo(0);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0002FC3C File Offset: 0x0002EC3C
		private bool GetControlEnabled()
		{
			bool flag;
			try
			{
				flag = base.IsHandleCreated;
			}
			catch (Exception)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0002FC68 File Offset: 0x0002EC68
		internal override bool CanSelectCore()
		{
			return this.GetControlEnabled() && !this.axState[AxHost.rejectSelection] && base.CanSelectCore();
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0002FC8C File Offset: 0x0002EC8C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.TransitionDownTo(0);
				if (this.newParent != null)
				{
					this.newParent.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0002FCB2 File Offset: 0x0002ECB2
		private bool GetSiteOwnsDeactivation()
		{
			return this.axState[AxHost.ownDisposing];
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0002FCC4 File Offset: 0x0002ECC4
		private void DisposeAxControl()
		{
			if (this.GetParentContainer() != null)
			{
				this.GetParentContainer().RemoveControl(this);
			}
			this.TransitionDownTo(2);
			if (this.GetOcState() == 2)
			{
				this.GetOleObject().SetClientSite(null);
				this.SetOcState(1);
			}
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0002FD00 File Offset: 0x0002ED00
		private void ReleaseAxControl()
		{
			this.NoComponentChangeEvents++;
			ContainerControl containerControl = this.ContainingControl;
			if (containerControl != null)
			{
				containerControl.VisibleChanged -= this.onContainerVisibleChanged;
			}
			try
			{
				if (this.instance != null)
				{
					Marshal.FinalReleaseComObject(this.instance);
					this.instance = null;
					this.iOleInPlaceObject = null;
					this.iOleObject = null;
					this.iOleControl = null;
					this.iOleInPlaceActiveObject = null;
					this.iOleInPlaceActiveObjectExternal = null;
					this.iPerPropertyBrowsing = null;
					this.iCategorizeProperties = null;
					this.iPersistStream = null;
					this.iPersistStreamInit = null;
					this.iPersistStorage = null;
				}
				this.axState[AxHost.checkedIppb] = false;
				this.axState[AxHost.checkedCP] = false;
				this.axState[AxHost.disposed] = true;
				this.freezeCount = 0;
				this.axState[AxHost.sinkAttached] = false;
				this.wndprocAddr = IntPtr.Zero;
				this.SetOcState(0);
			}
			finally
			{
				this.NoComponentChangeEvents--;
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0002FE10 File Offset: 0x0002EE10
		private void ParseMiscBits(int bits)
		{
			this.axState[AxHost.fOwnWindow] = (bits & 1024) != 0 && this.IsUserMode();
			this.axState[AxHost.fSimpleFrame] = (bits & 65536) != 0;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0002FE5C File Offset: 0x0002EE5C
		private void SlowActivate()
		{
			bool flag = false;
			if ((this.miscStatusBits & 131072) != 0)
			{
				this.GetOleObject().SetClientSite(this.oleSite);
				flag = true;
			}
			this.DepersistControl();
			if (!flag)
			{
				this.GetOleObject().SetClientSite(this.oleSite);
			}
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0002FEA8 File Offset: 0x0002EEA8
		private static NativeMethods.COMRECT FillInRect(NativeMethods.COMRECT dest, Rectangle source)
		{
			dest.left = source.X;
			dest.top = source.Y;
			dest.right = source.Width + source.X;
			dest.bottom = source.Height + source.Y;
			return dest;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0002FEFC File Offset: 0x0002EEFC
		private AxHost.AxContainer GetParentContainer()
		{
			IntSecurity.GetParent.Demand();
			if (this.container == null)
			{
				this.container = AxHost.AxContainer.FindContainerForControl(this);
			}
			if (this.container == null)
			{
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl == null)
				{
					if (this.newParent == null)
					{
						this.newParent = new ContainerControl();
						this.axContainer = this.newParent.CreateAxContainer();
						this.axContainer.AddControl(this);
					}
					return this.axContainer;
				}
				this.container = containerControl.CreateAxContainer();
				this.container.AddControl(this);
				this.containingControl = containerControl;
			}
			return this.container;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0002FF95 File Offset: 0x0002EF95
		private UnsafeNativeMethods.IOleControl GetOleControl()
		{
			if (this.iOleControl == null)
			{
				this.iOleControl = (UnsafeNativeMethods.IOleControl)this.instance;
			}
			return this.iOleControl;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0002FFB8 File Offset: 0x0002EFB8
		private UnsafeNativeMethods.IOleInPlaceActiveObject GetInPlaceActiveObject()
		{
			if (this.iOleInPlaceActiveObjectExternal != null)
			{
				return this.iOleInPlaceActiveObjectExternal;
			}
			if (this.iOleInPlaceActiveObject == null)
			{
				try
				{
					this.iOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this.instance;
				}
				catch (InvalidCastException)
				{
				}
			}
			return this.iOleInPlaceActiveObject;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00030008 File Offset: 0x0002F008
		private UnsafeNativeMethods.IOleObject GetOleObject()
		{
			if (this.iOleObject == null)
			{
				this.iOleObject = (UnsafeNativeMethods.IOleObject)this.instance;
			}
			return this.iOleObject;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00030029 File Offset: 0x0002F029
		private UnsafeNativeMethods.IOleInPlaceObject GetInPlaceObject()
		{
			if (this.iOleInPlaceObject == null)
			{
				this.iOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this.instance;
			}
			return this.iOleInPlaceObject;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0003004C File Offset: 0x0002F04C
		private NativeMethods.ICategorizeProperties GetCategorizeProperties()
		{
			if (this.iCategorizeProperties == null && !this.axState[AxHost.checkedCP] && this.instance != null)
			{
				this.axState[AxHost.checkedCP] = true;
				if (this.instance is NativeMethods.ICategorizeProperties)
				{
					this.iCategorizeProperties = (NativeMethods.ICategorizeProperties)this.instance;
				}
			}
			return this.iCategorizeProperties;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000300B0 File Offset: 0x0002F0B0
		private NativeMethods.IPerPropertyBrowsing GetPerPropertyBrowsing()
		{
			if (this.iPerPropertyBrowsing == null && !this.axState[AxHost.checkedIppb] && this.instance != null)
			{
				this.axState[AxHost.checkedIppb] = true;
				if (this.instance is NativeMethods.IPerPropertyBrowsing)
				{
					this.iPerPropertyBrowsing = (NativeMethods.IPerPropertyBrowsing)this.instance;
				}
			}
			return this.iPerPropertyBrowsing;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00030114 File Offset: 0x0002F114
		private static object GetPICTDESCFromPicture(Image image)
		{
			Bitmap bitmap = image as Bitmap;
			if (bitmap != null)
			{
				return new NativeMethods.PICTDESCbmp(bitmap);
			}
			Metafile metafile = image as Metafile;
			if (metafile != null)
			{
				return new NativeMethods.PICTDESCemf(metafile);
			}
			throw new ArgumentException(SR.GetString("AXUnknownImage"), "image");
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00030158 File Offset: 0x0002F158
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureFromPicture(Image image)
		{
			if (image == null)
			{
				return null;
			}
			object pictdescfromPicture = AxHost.GetPICTDESCFromPicture(image);
			return UnsafeNativeMethods.OleCreateIPictureIndirect(pictdescfromPicture, ref AxHost.ipicture_Guid, true);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00030180 File Offset: 0x0002F180
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureFromCursor(Cursor cursor)
		{
			if (cursor == null)
			{
				return null;
			}
			NativeMethods.PICTDESCicon pictdescicon = new NativeMethods.PICTDESCicon(Icon.FromHandle(cursor.Handle));
			return UnsafeNativeMethods.OleCreateIPictureIndirect(pictdescicon, ref AxHost.ipicture_Guid, true);
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000301B8 File Offset: 0x0002F1B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIPictureDispFromPicture(Image image)
		{
			if (image == null)
			{
				return null;
			}
			object pictdescfromPicture = AxHost.GetPICTDESCFromPicture(image);
			return UnsafeNativeMethods.OleCreateIPictureDispIndirect(pictdescfromPicture, ref AxHost.ipictureDisp_Guid, true);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000301E0 File Offset: 0x0002F1E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Image GetPictureFromIPicture(object picture)
		{
			if (picture == null)
			{
				return null;
			}
			IntPtr intPtr = IntPtr.Zero;
			UnsafeNativeMethods.IPicture picture2 = (UnsafeNativeMethods.IPicture)picture;
			int pictureType = (int)picture2.GetPictureType();
			if (pictureType == 1)
			{
				try
				{
					intPtr = picture2.GetHPal();
				}
				catch (COMException)
				{
				}
			}
			return AxHost.GetPictureFromParams(picture2, picture2.GetHandle(), pictureType, intPtr, picture2.GetWidth(), picture2.GetHeight());
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00030240 File Offset: 0x0002F240
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Image GetPictureFromIPictureDisp(object picture)
		{
			if (picture == null)
			{
				return null;
			}
			IntPtr intPtr = IntPtr.Zero;
			UnsafeNativeMethods.IPictureDisp pictureDisp = (UnsafeNativeMethods.IPictureDisp)picture;
			int pictureType = (int)pictureDisp.PictureType;
			if (pictureType == 1)
			{
				try
				{
					intPtr = pictureDisp.HPal;
				}
				catch (COMException)
				{
				}
			}
			return AxHost.GetPictureFromParams(pictureDisp, pictureDisp.Handle, pictureType, intPtr, pictureDisp.Width, pictureDisp.Height);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000302A0 File Offset: 0x0002F2A0
		private static Image GetPictureFromParams(object pict, IntPtr handle, int type, IntPtr paletteHandle, int width, int height)
		{
			switch (type)
			{
			case -1:
				return null;
			case 0:
				return null;
			case 1:
				return Image.FromHbitmap(handle, paletteHandle);
			case 2:
				return (Image)new Metafile(handle, new WmfPlaceableFileHeader
				{
					BboxRight = (short)width,
					BboxBottom = (short)height
				}, false).Clone();
			case 3:
				return (Image)Icon.FromHandle(handle).Clone();
			case 4:
				return (Image)new Metafile(handle, false).Clone();
			default:
				throw new ArgumentException(SR.GetString("AXUnknownImage"), "type");
			}
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00030340 File Offset: 0x0002F340
		private static NativeMethods.FONTDESC GetFONTDESCFromFont(Font font)
		{
			NativeMethods.FONTDESC fontdesc = null;
			if (AxHost.fontTable == null)
			{
				AxHost.fontTable = new Hashtable();
			}
			else
			{
				fontdesc = (NativeMethods.FONTDESC)AxHost.fontTable[font];
			}
			if (fontdesc == null)
			{
				fontdesc = new NativeMethods.FONTDESC();
				fontdesc.lpstrName = font.Name;
				fontdesc.cySize = (long)(font.SizeInPoints * 10000f);
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				font.ToLogFont(logfont);
				fontdesc.sWeight = (short)logfont.lfWeight;
				fontdesc.sCharset = (short)logfont.lfCharSet;
				fontdesc.fItalic = font.Italic;
				fontdesc.fUnderline = font.Underline;
				fontdesc.fStrikethrough = font.Strikeout;
				AxHost.fontTable[font] = fontdesc;
			}
			return fontdesc;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000303F2 File Offset: 0x0002F3F2
		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Color GetColorFromOleColor(uint color)
		{
			return ColorTranslator.FromOle((int)color);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x000303FA File Offset: 0x0002F3FA
		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static uint GetOleColorFromColor(Color color)
		{
			return (uint)ColorTranslator.ToOle(color);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00030404 File Offset: 0x0002F404
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIFontFromFont(Font font)
		{
			if (font == null)
			{
				return null;
			}
			if (font.Unit != GraphicsUnit.Point)
			{
				throw new ArgumentException(SR.GetString("AXFontUnitNotPoint"), "font");
			}
			object obj;
			try
			{
				obj = UnsafeNativeMethods.OleCreateIFontIndirect(AxHost.GetFONTDESCFromFont(font), ref AxHost.ifont_Guid);
			}
			catch
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00030460 File Offset: 0x0002F460
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Font GetFontFromIFont(object font)
		{
			if (font == null)
			{
				return null;
			}
			UnsafeNativeMethods.IFont font2 = (UnsafeNativeMethods.IFont)font;
			Font font4;
			try
			{
				Font font3 = Font.FromHfont(font2.GetHFont());
				if (font3.Unit != GraphicsUnit.Point)
				{
					font3 = new Font(font3.Name, font3.SizeInPoints, font3.Style, GraphicsUnit.Point, font3.GdiCharSet, font3.GdiVerticalFont);
				}
				font4 = font3;
			}
			catch (Exception)
			{
				font4 = Control.DefaultFont;
			}
			return font4;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x000304D4 File Offset: 0x0002F4D4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static object GetIFontDispFromFont(Font font)
		{
			if (font == null)
			{
				return null;
			}
			if (font.Unit != GraphicsUnit.Point)
			{
				throw new ArgumentException(SR.GetString("AXFontUnitNotPoint"), "font");
			}
			return SafeNativeMethods.OleCreateIFontDispIndirect(AxHost.GetFONTDESCFromFont(font), ref AxHost.ifontDisp_Guid);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00030518 File Offset: 0x0002F518
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static Font GetFontFromIFontDisp(object font)
		{
			if (font == null)
			{
				return null;
			}
			UnsafeNativeMethods.IFont font2 = font as UnsafeNativeMethods.IFont;
			if (font2 != null)
			{
				return AxHost.GetFontFromIFont(font2);
			}
			SafeNativeMethods.IFontDisp fontDisp = (SafeNativeMethods.IFontDisp)font;
			FontStyle fontStyle = FontStyle.Regular;
			Font font4;
			try
			{
				if (fontDisp.Bold)
				{
					fontStyle |= FontStyle.Bold;
				}
				if (fontDisp.Italic)
				{
					fontStyle |= FontStyle.Italic;
				}
				if (fontDisp.Underline)
				{
					fontStyle |= FontStyle.Underline;
				}
				if (fontDisp.Strikethrough)
				{
					fontStyle |= FontStyle.Strikeout;
				}
				if (fontDisp.Weight >= 700)
				{
					fontStyle |= FontStyle.Bold;
				}
				Font font3 = new Font(fontDisp.Name, (float)fontDisp.Size / 10000f, fontStyle, GraphicsUnit.Point, (byte)fontDisp.Charset);
				font4 = font3;
			}
			catch (Exception)
			{
				font4 = Control.DefaultFont;
			}
			return font4;
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x000305CC File Offset: 0x0002F5CC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static double GetOADateFromTime(DateTime time)
		{
			return time.ToOADate();
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x000305D5 File Offset: 0x0002F5D5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static DateTime GetTimeFromOADate(double date)
		{
			return DateTime.FromOADate(date);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000305DD File Offset: 0x0002F5DD
		private int Convert2int(object o, bool xDirection)
		{
			o = ((Array)o).GetValue(0);
			if (o.GetType() == typeof(float))
			{
				return AxHost.Twip2Pixel(Convert.ToDouble(o, CultureInfo.InvariantCulture), xDirection);
			}
			return Convert.ToInt32(o, CultureInfo.InvariantCulture);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0003061C File Offset: 0x0002F61C
		private short Convert2short(object o)
		{
			o = ((Array)o).GetValue(0);
			return Convert.ToInt16(o, CultureInfo.InvariantCulture);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00030637 File Offset: 0x0002F637
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseMove(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0003065E File Offset: 0x0002F65E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseMove(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00030679 File Offset: 0x0002F679
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseMove(short button, short shift, int x, int y)
		{
			base.OnMouseMove(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0003068F File Offset: 0x0002F68F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseUp(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000306B6 File Offset: 0x0002F6B6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseUp(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000306D1 File Offset: 0x0002F6D1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseUp(short button, short shift, int x, int y)
		{
			base.OnMouseUp(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000306E7 File Offset: 0x0002F6E7
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(object o1, object o2, object o3, object o4)
		{
			this.RaiseOnMouseDown(this.Convert2short(o1), this.Convert2short(o2), this.Convert2int(o3, true), this.Convert2int(o4, false));
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0003070E File Offset: 0x0002F70E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(short button, short shift, float x, float y)
		{
			this.RaiseOnMouseDown(button, shift, AxHost.Twip2Pixel((int)x, true), AxHost.Twip2Pixel((int)y, false));
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00030729 File Offset: 0x0002F729
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseOnMouseDown(short button, short shift, int x, int y)
		{
			base.OnMouseDown(new MouseEventArgs((MouseButtons)(button << 20), 1, x, y, 0));
		}

		// Token: 0x0400123B RID: 4667
		private const int INPROC_SERVER = 1;

		// Token: 0x0400123C RID: 4668
		private const int OC_PASSIVE = 0;

		// Token: 0x0400123D RID: 4669
		private const int OC_LOADED = 1;

		// Token: 0x0400123E RID: 4670
		private const int OC_RUNNING = 2;

		// Token: 0x0400123F RID: 4671
		private const int OC_INPLACE = 4;

		// Token: 0x04001240 RID: 4672
		private const int OC_UIACTIVE = 8;

		// Token: 0x04001241 RID: 4673
		private const int OC_OPEN = 16;

		// Token: 0x04001242 RID: 4674
		private const int EDITM_NONE = 0;

		// Token: 0x04001243 RID: 4675
		private const int EDITM_OBJECT = 1;

		// Token: 0x04001244 RID: 4676
		private const int EDITM_HOST = 2;

		// Token: 0x04001245 RID: 4677
		private const int STG_UNKNOWN = -1;

		// Token: 0x04001246 RID: 4678
		private const int STG_STREAM = 0;

		// Token: 0x04001247 RID: 4679
		private const int STG_STREAMINIT = 1;

		// Token: 0x04001248 RID: 4680
		private const int STG_STORAGE = 2;

		// Token: 0x04001249 RID: 4681
		private const int OLEIVERB_SHOW = -1;

		// Token: 0x0400124A RID: 4682
		private const int OLEIVERB_HIDE = -3;

		// Token: 0x0400124B RID: 4683
		private const int OLEIVERB_UIACTIVATE = -4;

		// Token: 0x0400124C RID: 4684
		private const int OLEIVERB_INPLACEACTIVATE = -5;

		// Token: 0x0400124D RID: 4685
		private const int OLEIVERB_PROPERTIES = -7;

		// Token: 0x0400124E RID: 4686
		private const int OLEIVERB_PRIMARY = 0;

		// Token: 0x0400124F RID: 4687
		private const int REGMSG_RETVAL = 123;

		// Token: 0x04001250 RID: 4688
		private const int HMperInch = 2540;

		// Token: 0x04001251 RID: 4689
		private static TraceSwitch AxHTraceSwitch = new TraceSwitch("AxHTrace", "ActiveX handle tracing");

		// Token: 0x04001252 RID: 4690
		private static TraceSwitch AxPropTraceSwitch = new TraceSwitch("AxPropTrace", "ActiveX property tracing");

		// Token: 0x04001253 RID: 4691
		private static TraceSwitch AxHostSwitch = new TraceSwitch("AxHost", "ActiveX host creation");

		// Token: 0x04001254 RID: 4692
		private static BooleanSwitch AxIgnoreTMSwitch = new BooleanSwitch("AxIgnoreTM", "ActiveX switch to ignore thread models");

		// Token: 0x04001255 RID: 4693
		private static BooleanSwitch AxAlwaysSaveSwitch = new BooleanSwitch("AxAlwaysSave", "ActiveX to save all controls regardless of their IsDirty function return value");

		// Token: 0x04001256 RID: 4694
		private static COMException E_NOTIMPL = new COMException(SR.GetString("AXNotImplemented"), -2147483647);

		// Token: 0x04001257 RID: 4695
		private static COMException E_INVALIDARG = new COMException(SR.GetString("AXInvalidArgument"), -2147024809);

		// Token: 0x04001258 RID: 4696
		private static COMException E_FAIL = new COMException(SR.GetString("AXUnknownError"), -2147467259);

		// Token: 0x04001259 RID: 4697
		private static COMException E_NOINTERFACE = new COMException(SR.GetString("AxInterfaceNotSupported"), -2147467262);

		// Token: 0x0400125A RID: 4698
		private readonly int REGMSG_MSG = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_subclassCheck");

		// Token: 0x0400125B RID: 4699
		private static int logPixelsX = -1;

		// Token: 0x0400125C RID: 4700
		private static int logPixelsY = -1;

		// Token: 0x0400125D RID: 4701
		private static Guid icf2_Guid = typeof(UnsafeNativeMethods.IClassFactory2).GUID;

		// Token: 0x0400125E RID: 4702
		private static Guid ifont_Guid = typeof(UnsafeNativeMethods.IFont).GUID;

		// Token: 0x0400125F RID: 4703
		private static Guid ifontDisp_Guid = typeof(SafeNativeMethods.IFontDisp).GUID;

		// Token: 0x04001260 RID: 4704
		private static Guid ipicture_Guid = typeof(UnsafeNativeMethods.IPicture).GUID;

		// Token: 0x04001261 RID: 4705
		private static Guid ipictureDisp_Guid = typeof(UnsafeNativeMethods.IPictureDisp).GUID;

		// Token: 0x04001262 RID: 4706
		private static Guid ivbformat_Guid = typeof(UnsafeNativeMethods.IVBFormat).GUID;

		// Token: 0x04001263 RID: 4707
		private static Guid ioleobject_Guid = typeof(UnsafeNativeMethods.IOleObject).GUID;

		// Token: 0x04001264 RID: 4708
		private static Guid dataSource_Guid = new Guid("{7C0FFAB3-CD84-11D0-949A-00A0C91110ED}");

		// Token: 0x04001265 RID: 4709
		private static Guid windowsMediaPlayer_Clsid = new Guid("{22d6f312-b0f6-11d0-94ab-0080c74c7e95}");

		// Token: 0x04001266 RID: 4710
		private static Guid comctlImageCombo_Clsid = new Guid("{a98a24c0-b06f-3684-8c12-c52ae341e0bc}");

		// Token: 0x04001267 RID: 4711
		private static Guid maskEdit_Clsid = new Guid("{c932ba85-4374-101b-a56c-00aa003668dc}");

		// Token: 0x04001268 RID: 4712
		private static Hashtable fontTable;

		// Token: 0x04001269 RID: 4713
		private static readonly int ocxStateSet = BitVector32.CreateMask();

		// Token: 0x0400126A RID: 4714
		private static readonly int editorRefresh = BitVector32.CreateMask(AxHost.ocxStateSet);

		// Token: 0x0400126B RID: 4715
		private static readonly int listeningToIdle = BitVector32.CreateMask(AxHost.editorRefresh);

		// Token: 0x0400126C RID: 4716
		private static readonly int refreshProperties = BitVector32.CreateMask(AxHost.listeningToIdle);

		// Token: 0x0400126D RID: 4717
		private static readonly int checkedIppb = BitVector32.CreateMask(AxHost.refreshProperties);

		// Token: 0x0400126E RID: 4718
		private static readonly int checkedCP = BitVector32.CreateMask(AxHost.checkedIppb);

		// Token: 0x0400126F RID: 4719
		private static readonly int fNeedOwnWindow = BitVector32.CreateMask(AxHost.checkedCP);

		// Token: 0x04001270 RID: 4720
		private static readonly int fOwnWindow = BitVector32.CreateMask(AxHost.fNeedOwnWindow);

		// Token: 0x04001271 RID: 4721
		private static readonly int fSimpleFrame = BitVector32.CreateMask(AxHost.fOwnWindow);

		// Token: 0x04001272 RID: 4722
		private static readonly int fFakingWindow = BitVector32.CreateMask(AxHost.fSimpleFrame);

		// Token: 0x04001273 RID: 4723
		private static readonly int rejectSelection = BitVector32.CreateMask(AxHost.fFakingWindow);

		// Token: 0x04001274 RID: 4724
		private static readonly int ownDisposing = BitVector32.CreateMask(AxHost.rejectSelection);

		// Token: 0x04001275 RID: 4725
		private static readonly int sinkAttached = BitVector32.CreateMask(AxHost.ownDisposing);

		// Token: 0x04001276 RID: 4726
		private static readonly int disposed = BitVector32.CreateMask(AxHost.sinkAttached);

		// Token: 0x04001277 RID: 4727
		private static readonly int manualUpdate = BitVector32.CreateMask(AxHost.disposed);

		// Token: 0x04001278 RID: 4728
		private static readonly int addedSelectionHandler = BitVector32.CreateMask(AxHost.manualUpdate);

		// Token: 0x04001279 RID: 4729
		private static readonly int valueChanged = BitVector32.CreateMask(AxHost.addedSelectionHandler);

		// Token: 0x0400127A RID: 4730
		private static readonly int handlePosRectChanged = BitVector32.CreateMask(AxHost.valueChanged);

		// Token: 0x0400127B RID: 4731
		private static readonly int siteProcessedInputKey = BitVector32.CreateMask(AxHost.handlePosRectChanged);

		// Token: 0x0400127C RID: 4732
		private static readonly int needLicenseKey = BitVector32.CreateMask(AxHost.siteProcessedInputKey);

		// Token: 0x0400127D RID: 4733
		private static readonly int inTransition = BitVector32.CreateMask(AxHost.needLicenseKey);

		// Token: 0x0400127E RID: 4734
		private static readonly int processingKeyUp = BitVector32.CreateMask(AxHost.inTransition);

		// Token: 0x0400127F RID: 4735
		private static readonly int assignUniqueID = BitVector32.CreateMask(AxHost.processingKeyUp);

		// Token: 0x04001280 RID: 4736
		private static readonly int renameEventHooked = BitVector32.CreateMask(AxHost.assignUniqueID);

		// Token: 0x04001281 RID: 4737
		private BitVector32 axState = default(BitVector32);

		// Token: 0x04001282 RID: 4738
		private int storageType = -1;

		// Token: 0x04001283 RID: 4739
		private int ocState;

		// Token: 0x04001284 RID: 4740
		private int miscStatusBits;

		// Token: 0x04001285 RID: 4741
		private int freezeCount;

		// Token: 0x04001286 RID: 4742
		private int flags;

		// Token: 0x04001287 RID: 4743
		private int selectionStyle;

		// Token: 0x04001288 RID: 4744
		private int editMode;

		// Token: 0x04001289 RID: 4745
		private int noComponentChange;

		// Token: 0x0400128A RID: 4746
		private IntPtr wndprocAddr = IntPtr.Zero;

		// Token: 0x0400128B RID: 4747
		private Guid clsid;

		// Token: 0x0400128C RID: 4748
		private string text = "";

		// Token: 0x0400128D RID: 4749
		private string licenseKey;

		// Token: 0x0400128E RID: 4750
		private readonly AxHost.OleInterfaces oleSite;

		// Token: 0x0400128F RID: 4751
		private AxHost.AxComponentEditor editor;

		// Token: 0x04001290 RID: 4752
		private AxHost.AxContainer container;

		// Token: 0x04001291 RID: 4753
		private ContainerControl containingControl;

		// Token: 0x04001292 RID: 4754
		private ContainerControl newParent;

		// Token: 0x04001293 RID: 4755
		private AxHost.AxContainer axContainer;

		// Token: 0x04001294 RID: 4756
		private AxHost.State ocxState;

		// Token: 0x04001295 RID: 4757
		private IntPtr hwndFocus = IntPtr.Zero;

		// Token: 0x04001296 RID: 4758
		private Hashtable properties;

		// Token: 0x04001297 RID: 4759
		private Hashtable propertyInfos;

		// Token: 0x04001298 RID: 4760
		private PropertyDescriptorCollection propsStash;

		// Token: 0x04001299 RID: 4761
		private Attribute[] attribsStash;

		// Token: 0x0400129A RID: 4762
		private object instance;

		// Token: 0x0400129B RID: 4763
		private UnsafeNativeMethods.IOleInPlaceObject iOleInPlaceObject;

		// Token: 0x0400129C RID: 4764
		private UnsafeNativeMethods.IOleObject iOleObject;

		// Token: 0x0400129D RID: 4765
		private UnsafeNativeMethods.IOleControl iOleControl;

		// Token: 0x0400129E RID: 4766
		private UnsafeNativeMethods.IOleInPlaceActiveObject iOleInPlaceActiveObject;

		// Token: 0x0400129F RID: 4767
		private UnsafeNativeMethods.IOleInPlaceActiveObject iOleInPlaceActiveObjectExternal;

		// Token: 0x040012A0 RID: 4768
		private NativeMethods.IPerPropertyBrowsing iPerPropertyBrowsing;

		// Token: 0x040012A1 RID: 4769
		private NativeMethods.ICategorizeProperties iCategorizeProperties;

		// Token: 0x040012A2 RID: 4770
		private UnsafeNativeMethods.IPersistPropertyBag iPersistPropBag;

		// Token: 0x040012A3 RID: 4771
		private UnsafeNativeMethods.IPersistStream iPersistStream;

		// Token: 0x040012A4 RID: 4772
		private UnsafeNativeMethods.IPersistStreamInit iPersistStreamInit;

		// Token: 0x040012A5 RID: 4773
		private UnsafeNativeMethods.IPersistStorage iPersistStorage;

		// Token: 0x040012A6 RID: 4774
		private AxHost.AboutBoxDelegate aboutBoxDelegate;

		// Token: 0x040012A7 RID: 4775
		private EventHandler selectionChangeHandler;

		// Token: 0x040012A8 RID: 4776
		private bool isMaskEdit;

		// Token: 0x040012A9 RID: 4777
		private bool ignoreDialogKeys;

		// Token: 0x040012AA RID: 4778
		private EventHandler onContainerVisibleChanged;

		// Token: 0x040012AB RID: 4779
		private static CategoryAttribute[] categoryNames = new CategoryAttribute[]
		{
			null,
			new WinCategoryAttribute("Default"),
			new WinCategoryAttribute("Default"),
			new WinCategoryAttribute("Font"),
			new WinCategoryAttribute("Layout"),
			new WinCategoryAttribute("Appearance"),
			new WinCategoryAttribute("Behavior"),
			new WinCategoryAttribute("Data"),
			new WinCategoryAttribute("List"),
			new WinCategoryAttribute("Text"),
			new WinCategoryAttribute("Scale"),
			new WinCategoryAttribute("DDE")
		};

		// Token: 0x040012AC RID: 4780
		private Hashtable objectDefinedCategoryNames;

		// Token: 0x02000221 RID: 545
		internal class AxFlags
		{
			// Token: 0x040012AD RID: 4781
			internal const int PreventEditMode = 1;

			// Token: 0x040012AE RID: 4782
			internal const int IncludePropertiesVerb = 2;

			// Token: 0x040012AF RID: 4783
			internal const int IgnoreThreadModel = 268435456;
		}

		// Token: 0x02000222 RID: 546
		[AttributeUsage(AttributeTargets.Class, Inherited = false)]
		public sealed class ClsidAttribute : Attribute
		{
			// Token: 0x060019FC RID: 6652 RVA: 0x00030AF4 File Offset: 0x0002FAF4
			public ClsidAttribute(string clsid)
			{
				this.val = clsid;
			}

			// Token: 0x17000320 RID: 800
			// (get) Token: 0x060019FD RID: 6653 RVA: 0x00030B03 File Offset: 0x0002FB03
			public string Value
			{
				get
				{
					return this.val;
				}
			}

			// Token: 0x040012B0 RID: 4784
			private string val;
		}

		// Token: 0x02000223 RID: 547
		[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
		public sealed class TypeLibraryTimeStampAttribute : Attribute
		{
			// Token: 0x060019FE RID: 6654 RVA: 0x00030B0B File Offset: 0x0002FB0B
			public TypeLibraryTimeStampAttribute(string timestamp)
			{
				this.val = DateTime.Parse(timestamp, CultureInfo.InvariantCulture);
			}

			// Token: 0x17000321 RID: 801
			// (get) Token: 0x060019FF RID: 6655 RVA: 0x00030B24 File Offset: 0x0002FB24
			public DateTime Value
			{
				get
				{
					return this.val;
				}
			}

			// Token: 0x040012B1 RID: 4785
			private DateTime val;
		}

		// Token: 0x02000224 RID: 548
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class ConnectionPointCookie
		{
			// Token: 0x06001A00 RID: 6656 RVA: 0x00030B2C File Offset: 0x0002FB2C
			public ConnectionPointCookie(object source, object sink, Type eventInterface)
				: this(source, sink, eventInterface, true)
			{
			}

			// Token: 0x06001A01 RID: 6657 RVA: 0x00030B38 File Offset: 0x0002FB38
			internal ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException)
			{
				if (source is UnsafeNativeMethods.IConnectionPointContainer)
				{
					UnsafeNativeMethods.IConnectionPointContainer connectionPointContainer = (UnsafeNativeMethods.IConnectionPointContainer)source;
					try
					{
						Guid guid = eventInterface.GUID;
						if (connectionPointContainer.FindConnectionPoint(ref guid, out this.connectionPoint) != 0)
						{
							this.connectionPoint = null;
						}
					}
					catch
					{
						this.connectionPoint = null;
					}
					if (this.connectionPoint == null)
					{
						if (throwException)
						{
							throw new ArgumentException(SR.GetString("AXNoEventInterface", new object[] { eventInterface.Name }));
						}
					}
					else if (sink == null || !eventInterface.IsInstanceOfType(sink))
					{
						if (throwException)
						{
							throw new InvalidCastException(SR.GetString("AXNoSinkImplementation", new object[] { eventInterface.Name }));
						}
					}
					else
					{
						int num = this.connectionPoint.Advise(sink, ref this.cookie);
						if (num == 0)
						{
							this.threadId = Thread.CurrentThread.ManagedThreadId;
						}
						else
						{
							this.cookie = 0;
							Marshal.ReleaseComObject(this.connectionPoint);
							this.connectionPoint = null;
							if (throwException)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, SR.GetString("AXNoSinkAdvise", new object[] { eventInterface.Name }), new object[] { num }));
							}
						}
					}
				}
				else if (throwException)
				{
					throw new InvalidCastException(SR.GetString("AXNoConnectionPointContainer"));
				}
				if (this.connectionPoint == null || this.cookie == 0)
				{
					if (this.connectionPoint != null)
					{
						Marshal.ReleaseComObject(this.connectionPoint);
					}
					if (throwException)
					{
						throw new ArgumentException(SR.GetString("AXNoConnectionPoint", new object[] { eventInterface.Name }));
					}
				}
			}

			// Token: 0x06001A02 RID: 6658 RVA: 0x00030CE8 File Offset: 0x0002FCE8
			public void Disconnect()
			{
				if (this.connectionPoint != null && this.cookie != 0)
				{
					try
					{
						this.connectionPoint.Unadvise(this.cookie);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					finally
					{
						this.cookie = 0;
					}
					try
					{
						Marshal.ReleaseComObject(this.connectionPoint);
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsCriticalException(ex2))
						{
							throw;
						}
					}
					finally
					{
						this.connectionPoint = null;
					}
				}
			}

			// Token: 0x06001A03 RID: 6659 RVA: 0x00030D8C File Offset: 0x0002FD8C
			protected override void Finalize()
			{
				try
				{
					if (this.connectionPoint != null && this.cookie != 0 && !AppDomain.CurrentDomain.IsFinalizingForUnload())
					{
						SynchronizationContext synchronizationContext = SynchronizationContext.Current;
						if (synchronizationContext != null)
						{
							synchronizationContext.Post(new SendOrPostCallback(this.AttemptDisconnect), null);
						}
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x06001A04 RID: 6660 RVA: 0x00030DEC File Offset: 0x0002FDEC
			private void AttemptDisconnect(object trash)
			{
				if (this.threadId == Thread.CurrentThread.ManagedThreadId)
				{
					this.Disconnect();
				}
			}

			// Token: 0x17000322 RID: 802
			// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00030E06 File Offset: 0x0002FE06
			internal bool Connected
			{
				get
				{
					return this.connectionPoint != null && this.cookie != 0;
				}
			}

			// Token: 0x040012B2 RID: 4786
			private UnsafeNativeMethods.IConnectionPoint connectionPoint;

			// Token: 0x040012B3 RID: 4787
			private int cookie;

			// Token: 0x040012B4 RID: 4788
			internal int threadId;
		}

		// Token: 0x02000225 RID: 549
		public enum ActiveXInvokeKind
		{
			// Token: 0x040012B6 RID: 4790
			MethodInvoke,
			// Token: 0x040012B7 RID: 4791
			PropertyGet,
			// Token: 0x040012B8 RID: 4792
			PropertySet
		}

		// Token: 0x02000226 RID: 550
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class InvalidActiveXStateException : Exception
		{
			// Token: 0x06001A06 RID: 6662 RVA: 0x00030E1E File Offset: 0x0002FE1E
			public InvalidActiveXStateException(string name, AxHost.ActiveXInvokeKind kind)
			{
				this.name = name;
				this.kind = kind;
			}

			// Token: 0x06001A07 RID: 6663 RVA: 0x00030E34 File Offset: 0x0002FE34
			public InvalidActiveXStateException()
			{
			}

			// Token: 0x06001A08 RID: 6664 RVA: 0x00030E3C File Offset: 0x0002FE3C
			public override string ToString()
			{
				switch (this.kind)
				{
				case AxHost.ActiveXInvokeKind.MethodInvoke:
					return SR.GetString("AXInvalidMethodInvoke", new object[] { this.name });
				case AxHost.ActiveXInvokeKind.PropertyGet:
					return SR.GetString("AXInvalidPropertyGet", new object[] { this.name });
				case AxHost.ActiveXInvokeKind.PropertySet:
					return SR.GetString("AXInvalidPropertySet", new object[] { this.name });
				default:
					return base.ToString();
				}
			}

			// Token: 0x040012B9 RID: 4793
			private string name;

			// Token: 0x040012BA RID: 4794
			private AxHost.ActiveXInvokeKind kind;
		}

		// Token: 0x02000227 RID: 551
		private class OleInterfaces : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.ISimpleFrameSite, UnsafeNativeMethods.IVBGetControl, UnsafeNativeMethods.IGetVBAObject, UnsafeNativeMethods.IPropertyNotifySink, IReflect
		{
			// Token: 0x06001A09 RID: 6665 RVA: 0x00030EBE File Offset: 0x0002FEBE
			internal OleInterfaces(AxHost host)
			{
				if (host == null)
				{
					throw new ArgumentNullException("host");
				}
				this.host = host;
			}

			// Token: 0x06001A0A RID: 6666 RVA: 0x00030EDC File Offset: 0x0002FEDC
			protected override void Finalize()
			{
				try
				{
					if (!AppDomain.CurrentDomain.IsFinalizingForUnload())
					{
						SynchronizationContext synchronizationContext = SynchronizationContext.Current;
						if (synchronizationContext != null)
						{
							synchronizationContext.Post(new SendOrPostCallback(this.AttemptStopEvents), null);
						}
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x06001A0B RID: 6667 RVA: 0x00030F2C File Offset: 0x0002FF2C
			internal AxHost GetAxHost()
			{
				return this.host;
			}

			// Token: 0x06001A0C RID: 6668 RVA: 0x00030F34 File Offset: 0x0002FF34
			internal void OnOcxCreate()
			{
				this.StartEvents();
			}

			// Token: 0x06001A0D RID: 6669 RVA: 0x00030F3C File Offset: 0x0002FF3C
			internal void StartEvents()
			{
				if (this.connectionPoint != null)
				{
					return;
				}
				object ocx = this.host.GetOcx();
				try
				{
					this.connectionPoint = new AxHost.ConnectionPointCookie(ocx, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
				}
				catch
				{
				}
			}

			// Token: 0x06001A0E RID: 6670 RVA: 0x00030F8C File Offset: 0x0002FF8C
			private void AttemptStopEvents(object trash)
			{
				if (this.connectionPoint == null)
				{
					return;
				}
				if (this.connectionPoint.threadId == Thread.CurrentThread.ManagedThreadId)
				{
					this.StopEvents();
				}
			}

			// Token: 0x06001A0F RID: 6671 RVA: 0x00030FB4 File Offset: 0x0002FFB4
			internal void StopEvents()
			{
				if (this.connectionPoint != null)
				{
					this.connectionPoint.Disconnect();
					this.connectionPoint = null;
				}
			}

			// Token: 0x06001A10 RID: 6672 RVA: 0x00030FD0 File Offset: 0x0002FFD0
			int UnsafeNativeMethods.IGetVBAObject.GetObject(ref Guid riid, UnsafeNativeMethods.IVBFormat[] rval, int dwReserved)
			{
				if (rval == null || riid.Equals(Guid.Empty))
				{
					return -2147024809;
				}
				if (riid.Equals(AxHost.ivbformat_Guid))
				{
					rval[0] = new AxHost.VBFormat();
					return 0;
				}
				rval[0] = null;
				return -2147467262;
			}

			// Token: 0x06001A11 RID: 6673 RVA: 0x00031008 File Offset: 0x00030008
			int UnsafeNativeMethods.IVBGetControl.EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = null;
				ppenum = this.host.GetParentContainer().EnumControls(this.host, dwOleContF, dwWhich);
				return 0;
			}

			// Token: 0x06001A12 RID: 6674 RVA: 0x00031028 File Offset: 0x00030028
			int UnsafeNativeMethods.ISimpleFrameSite.PreMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, ref int pdwCookie)
			{
				return 0;
			}

			// Token: 0x06001A13 RID: 6675 RVA: 0x0003102B File Offset: 0x0003002B
			int UnsafeNativeMethods.ISimpleFrameSite.PostMessageFilter(IntPtr hwnd, int msg, IntPtr wp, IntPtr lp, ref IntPtr plResult, int dwCookie)
			{
				return 1;
			}

			// Token: 0x06001A14 RID: 6676 RVA: 0x0003102E File Offset: 0x0003002E
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x06001A15 RID: 6677 RVA: 0x00031031 File Offset: 0x00030031
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A16 RID: 6678 RVA: 0x00031034 File Offset: 0x00030034
			MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
			{
				return new MethodInfo[0];
			}

			// Token: 0x06001A17 RID: 6679 RVA: 0x0003103C File Offset: 0x0003003C
			FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A18 RID: 6680 RVA: 0x0003103F File Offset: 0x0003003F
			FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
			{
				return new FieldInfo[0];
			}

			// Token: 0x06001A19 RID: 6681 RVA: 0x00031047 File Offset: 0x00030047
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A1A RID: 6682 RVA: 0x0003104A File Offset: 0x0003004A
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x06001A1B RID: 6683 RVA: 0x0003104D File Offset: 0x0003004D
			PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
			{
				return new PropertyInfo[0];
			}

			// Token: 0x06001A1C RID: 6684 RVA: 0x00031055 File Offset: 0x00030055
			MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06001A1D RID: 6685 RVA: 0x0003105D File Offset: 0x0003005D
			MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06001A1E RID: 6686 RVA: 0x00031068 File Offset: 0x00030068
			object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
			{
				if (name.StartsWith("[DISPID="))
				{
					int num = name.IndexOf("]");
					int num2 = int.Parse(name.Substring(8, num - 8), CultureInfo.InvariantCulture);
					object ambientProperty = this.host.GetAmbientProperty(num2);
					if (ambientProperty != null)
					{
						return ambientProperty;
					}
				}
				throw AxHost.E_FAIL;
			}

			// Token: 0x17000323 RID: 803
			// (get) Token: 0x06001A1F RID: 6687 RVA: 0x000310BA File Offset: 0x000300BA
			Type IReflect.UnderlyingSystemType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06001A20 RID: 6688 RVA: 0x000310BD File Offset: 0x000300BD
			int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
			{
				return 0;
			}

			// Token: 0x06001A21 RID: 6689 RVA: 0x000310C0 File Offset: 0x000300C0
			int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
			{
				return -2147467263;
			}

			// Token: 0x06001A22 RID: 6690 RVA: 0x000310C7 File Offset: 0x000300C7
			int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
			{
				ppDisp = this.host.GetParentContainer().GetProxyForControl(this.host);
				if (ppDisp == null)
				{
					return -2147467263;
				}
				return 0;
			}

			// Token: 0x06001A23 RID: 6691 RVA: 0x000310EC File Offset: 0x000300EC
			int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods._POINTL pPtlHimetric, NativeMethods.tagPOINTF pPtfContainer, int dwFlags)
			{
				int num = AxHost.SetupLogPixels(false);
				if (NativeMethods.Failed(num))
				{
					return num;
				}
				if ((dwFlags & 4) != 0)
				{
					if ((dwFlags & 2) != 0)
					{
						pPtfContainer.x = (float)this.host.HM2Pix(pPtlHimetric.x, AxHost.logPixelsX);
						pPtfContainer.y = (float)this.host.HM2Pix(pPtlHimetric.y, AxHost.logPixelsY);
					}
					else
					{
						if ((dwFlags & 1) == 0)
						{
							return -2147024809;
						}
						pPtfContainer.x = (float)this.host.HM2Pix(pPtlHimetric.x, AxHost.logPixelsX);
						pPtfContainer.y = (float)this.host.HM2Pix(pPtlHimetric.y, AxHost.logPixelsY);
					}
				}
				else
				{
					if ((dwFlags & 8) == 0)
					{
						return -2147024809;
					}
					if ((dwFlags & 2) != 0)
					{
						pPtlHimetric.x = this.host.Pix2HM((int)pPtfContainer.x, AxHost.logPixelsX);
						pPtlHimetric.y = this.host.Pix2HM((int)pPtfContainer.y, AxHost.logPixelsY);
					}
					else
					{
						if ((dwFlags & 1) == 0)
						{
							return -2147024809;
						}
						pPtlHimetric.x = this.host.Pix2HM((int)pPtfContainer.x, AxHost.logPixelsX);
						pPtlHimetric.y = this.host.Pix2HM((int)pPtfContainer.y, AxHost.logPixelsY);
					}
				}
				return 0;
			}

			// Token: 0x06001A24 RID: 6692 RVA: 0x00031238 File Offset: 0x00030238
			int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref NativeMethods.MSG pMsg, int grfModifiers)
			{
				this.host.SetAxState(AxHost.siteProcessedInputKey, true);
				Message message = default(Message);
				message.Msg = pMsg.message;
				message.WParam = pMsg.wParam;
				message.LParam = pMsg.lParam;
				message.HWnd = pMsg.hwnd;
				int num;
				try
				{
					num = (this.host.PreProcessMessage(ref message) ? 0 : 1);
				}
				finally
				{
					this.host.SetAxState(AxHost.siteProcessedInputKey, false);
				}
				return num;
			}

			// Token: 0x06001A25 RID: 6693 RVA: 0x000312D0 File Offset: 0x000302D0
			int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
			{
				return 0;
			}

			// Token: 0x06001A26 RID: 6694 RVA: 0x000312D3 File Offset: 0x000302D3
			int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
			{
				if (this.host.CanShowPropertyPages())
				{
					this.host.ShowPropertyPages();
					return 0;
				}
				return -2147467263;
			}

			// Token: 0x06001A27 RID: 6695 RVA: 0x000312F4 File Offset: 0x000302F4
			int UnsafeNativeMethods.IOleClientSite.SaveObject()
			{
				return -2147467263;
			}

			// Token: 0x06001A28 RID: 6696 RVA: 0x000312FB File Offset: 0x000302FB
			int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
			{
				moniker = null;
				return -2147467263;
			}

			// Token: 0x06001A29 RID: 6697 RVA: 0x00031305 File Offset: 0x00030305
			int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
			{
				container = this.host.GetParentContainer();
				return 0;
			}

			// Token: 0x06001A2A RID: 6698 RVA: 0x00031318 File Offset: 0x00030318
			int UnsafeNativeMethods.IOleClientSite.ShowObject()
			{
				if (this.host.GetAxState(AxHost.fOwnWindow))
				{
					return 0;
				}
				if (this.host.GetAxState(AxHost.fFakingWindow))
				{
					this.host.DestroyFakeWindow();
					this.host.TransitionDownTo(1);
					this.host.TransitionUpTo(4);
				}
				if (this.host.GetOcState() < 4)
				{
					return 0;
				}
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.host.GetInPlaceObject().GetWindow(out intPtr)))
				{
					if (this.host.GetHandleNoCreate() != intPtr)
					{
						this.host.DetachWindow();
						if (intPtr != IntPtr.Zero)
						{
							this.host.AttachWindow(intPtr);
						}
					}
				}
				else if (this.host.GetInPlaceObject() is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.GetString("AXWindowlessControl"));
				}
				return 0;
			}

			// Token: 0x06001A2B RID: 6699 RVA: 0x000313F3 File Offset: 0x000303F3
			int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
			{
				return 0;
			}

			// Token: 0x06001A2C RID: 6700 RVA: 0x000313F6 File Offset: 0x000303F6
			int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
			{
				return -2147467263;
			}

			// Token: 0x06001A2D RID: 6701 RVA: 0x00031400 File Offset: 0x00030400
			IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
			{
				IntPtr intPtr;
				try
				{
					Control parentInternal = this.host.ParentInternal;
					intPtr = ((parentInternal != null) ? parentInternal.Handle : IntPtr.Zero);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return intPtr;
			}

			// Token: 0x06001A2E RID: 6702 RVA: 0x00031440 File Offset: 0x00030440
			int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			// Token: 0x06001A2F RID: 6703 RVA: 0x00031447 File Offset: 0x00030447
			int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
			{
				return 0;
			}

			// Token: 0x06001A30 RID: 6704 RVA: 0x0003144A File Offset: 0x0003044A
			int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
			{
				this.host.SetAxState(AxHost.ownDisposing, false);
				this.host.SetAxState(AxHost.rejectSelection, false);
				this.host.SetOcState(4);
				return 0;
			}

			// Token: 0x06001A31 RID: 6705 RVA: 0x0003147B File Offset: 0x0003047B
			int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
			{
				this.host.SetOcState(8);
				this.host.GetParentContainer().OnUIActivate(this.host);
				return 0;
			}

			// Token: 0x06001A32 RID: 6706 RVA: 0x000314A0 File Offset: 0x000304A0
			int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
			{
				ppDoc = null;
				ppFrame = this.host.GetParentContainer();
				AxHost.FillInRect(lprcPosRect, this.host.Bounds);
				this.host.GetClipRect(lprcClipRect);
				if (lpFrameInfo != null)
				{
					lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
					lpFrameInfo.fMDIApp = false;
					lpFrameInfo.hAccel = IntPtr.Zero;
					lpFrameInfo.cAccelEntries = 0;
					lpFrameInfo.hwndFrame = this.host.ParentInternal.Handle;
				}
				return 0;
			}

			// Token: 0x06001A33 RID: 6707 RVA: 0x0003152C File Offset: 0x0003052C
			int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.tagSIZE scrollExtant)
			{
				return 1;
			}

			// Token: 0x06001A34 RID: 6708 RVA: 0x0003153A File Offset: 0x0003053A
			int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
			{
				this.host.GetParentContainer().OnUIDeactivate(this.host);
				if (this.host.GetOcState() > 4)
				{
					this.host.SetOcState(4);
				}
				return 0;
			}

			// Token: 0x06001A35 RID: 6709 RVA: 0x00031570 File Offset: 0x00030570
			int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
			{
				if (this.host.GetOcState() == 8)
				{
					((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
				}
				this.host.GetParentContainer().OnInPlaceDeactivate(this.host);
				this.host.DetachWindow();
				this.host.SetOcState(2);
				return 0;
			}

			// Token: 0x06001A36 RID: 6710 RVA: 0x000315C1 File Offset: 0x000305C1
			int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
			{
				return 0;
			}

			// Token: 0x06001A37 RID: 6711 RVA: 0x000315C4 File Offset: 0x000305C4
			int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
			{
				return this.host.GetInPlaceObject().UIDeactivate();
			}

			// Token: 0x06001A38 RID: 6712 RVA: 0x000315D8 File Offset: 0x000305D8
			int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
			{
				bool flag = true;
				if (AxHost.windowsMediaPlayer_Clsid.Equals(this.host.clsid))
				{
					flag = this.host.GetAxState(AxHost.handlePosRectChanged);
				}
				if (flag)
				{
					this.host.GetInPlaceObject().SetObjectRects(lprcPosRect, this.host.GetClipRect(new NativeMethods.COMRECT()));
					this.host.MakeDirty();
				}
				return 0;
			}

			// Token: 0x06001A39 RID: 6713 RVA: 0x00031640 File Offset: 0x00030640
			void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
			{
				if (this.host.NoComponentChangeEvents != 0)
				{
					return;
				}
				this.host.NoComponentChangeEvents++;
				try
				{
					AxHost.AxPropertyDescriptor axPropertyDescriptor = null;
					if (dispid != -1)
					{
						axPropertyDescriptor = this.host.GetPropertyDescriptorFromDispid(dispid);
						if (axPropertyDescriptor != null)
						{
							axPropertyDescriptor.OnValueChanged(this.host);
							if (!axPropertyDescriptor.SettingValue)
							{
								axPropertyDescriptor.UpdateTypeConverterAndTypeEditor(true);
							}
						}
					}
					else
					{
						PropertyDescriptorCollection properties = ((ICustomTypeDescriptor)this.host).GetProperties();
						foreach (object obj in properties)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							axPropertyDescriptor = propertyDescriptor as AxHost.AxPropertyDescriptor;
							if (axPropertyDescriptor != null && !axPropertyDescriptor.SettingValue)
							{
								axPropertyDescriptor.UpdateTypeConverterAndTypeEditor(true);
							}
						}
					}
					ISite site = this.host.Site;
					if (site != null)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							try
							{
								componentChangeService.OnComponentChanging(this.host, axPropertyDescriptor);
							}
							catch (CheckoutException ex)
							{
								if (ex == CheckoutException.Canceled)
								{
									return;
								}
								throw ex;
							}
							componentChangeService.OnComponentChanged(this.host, axPropertyDescriptor, null, (axPropertyDescriptor != null) ? axPropertyDescriptor.GetValue(this.host) : null);
						}
					}
				}
				catch (Exception ex2)
				{
					throw ex2;
				}
				finally
				{
					this.host.NoComponentChangeEvents--;
				}
			}

			// Token: 0x06001A3A RID: 6714 RVA: 0x000317EC File Offset: 0x000307EC
			int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
			{
				return 0;
			}

			// Token: 0x040012BB RID: 4795
			private AxHost host;

			// Token: 0x040012BC RID: 4796
			private AxHost.ConnectionPointCookie connectionPoint;
		}

		// Token: 0x02000228 RID: 552
		private class VBFormat : UnsafeNativeMethods.IVBFormat
		{
			// Token: 0x06001A3B RID: 6715 RVA: 0x000317F0 File Offset: 0x000307F0
			int UnsafeNativeMethods.IVBFormat.Format(ref object var, IntPtr pszFormat, IntPtr lpBuffer, short cpBuffer, int lcid, short firstD, short firstW, short[] result)
			{
				if (result == null)
				{
					return -2147024809;
				}
				result[0] = 0;
				if (lpBuffer == IntPtr.Zero || cpBuffer < 2)
				{
					return -2147024809;
				}
				IntPtr zero = IntPtr.Zero;
				UnsafeNativeMethods.VarFormat(ref var, new HandleRef(null, pszFormat), (int)firstD, (int)firstW, 32U, ref zero);
				try
				{
					int num = 0;
					if (zero != IntPtr.Zero)
					{
						cpBuffer -= 1;
						short num2;
						while (num < (int)cpBuffer && (num2 = Marshal.ReadInt16(zero, num * 2)) != 0)
						{
							Marshal.WriteInt16(lpBuffer, num * 2, num2);
							num++;
						}
					}
					Marshal.WriteInt16(lpBuffer, num * 2, 0);
					result[0] = (short)num;
				}
				finally
				{
					SafeNativeMethods.SysFreeString(new HandleRef(null, zero));
				}
				return 0;
			}
		}

		// Token: 0x02000229 RID: 553
		internal class EnumUnknown : UnsafeNativeMethods.IEnumUnknown
		{
			// Token: 0x06001A3D RID: 6717 RVA: 0x000318B4 File Offset: 0x000308B4
			internal EnumUnknown(object[] arr)
			{
				this.arr = arr;
				this.loc = 0;
				this.size = ((arr == null) ? 0 : arr.Length);
			}

			// Token: 0x06001A3E RID: 6718 RVA: 0x000318D9 File Offset: 0x000308D9
			private EnumUnknown(object[] arr, int loc)
				: this(arr)
			{
				this.loc = loc;
			}

			// Token: 0x06001A3F RID: 6719 RVA: 0x000318EC File Offset: 0x000308EC
			int UnsafeNativeMethods.IEnumUnknown.Next(int celt, IntPtr rgelt, IntPtr pceltFetched)
			{
				if (pceltFetched != IntPtr.Zero)
				{
					Marshal.WriteInt32(pceltFetched, 0, 0);
				}
				if (celt < 0)
				{
					return -2147024809;
				}
				int num = 0;
				if (this.loc >= this.size)
				{
					num = 0;
				}
				else
				{
					while (this.loc < this.size && num < celt)
					{
						if (this.arr[this.loc] != null)
						{
							Marshal.WriteIntPtr(rgelt, Marshal.GetIUnknownForObject(this.arr[this.loc]));
							rgelt = (IntPtr)((long)rgelt + (long)sizeof(IntPtr));
							num++;
						}
						this.loc++;
					}
				}
				if (pceltFetched != IntPtr.Zero)
				{
					Marshal.WriteInt32(pceltFetched, 0, num);
				}
				if (num != celt)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06001A40 RID: 6720 RVA: 0x000319A8 File Offset: 0x000309A8
			int UnsafeNativeMethods.IEnumUnknown.Skip(int celt)
			{
				this.loc += celt;
				if (this.loc >= this.size)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06001A41 RID: 6721 RVA: 0x000319C9 File Offset: 0x000309C9
			void UnsafeNativeMethods.IEnumUnknown.Reset()
			{
				this.loc = 0;
			}

			// Token: 0x06001A42 RID: 6722 RVA: 0x000319D2 File Offset: 0x000309D2
			void UnsafeNativeMethods.IEnumUnknown.Clone(out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = new AxHost.EnumUnknown(this.arr, this.loc);
			}

			// Token: 0x040012BD RID: 4797
			private object[] arr;

			// Token: 0x040012BE RID: 4798
			private int loc;

			// Token: 0x040012BF RID: 4799
			private int size;
		}

		// Token: 0x0200022A RID: 554
		internal class AxContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame, IReflect
		{
			// Token: 0x06001A43 RID: 6723 RVA: 0x000319E7 File Offset: 0x000309E7
			internal AxContainer(ContainerControl parent)
			{
				this.parent = parent;
				if (parent.Created)
				{
					this.FormCreated();
				}
			}

			// Token: 0x06001A44 RID: 6724 RVA: 0x00031A0F File Offset: 0x00030A0F
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x06001A45 RID: 6725 RVA: 0x00031A12 File Offset: 0x00030A12
			MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A46 RID: 6726 RVA: 0x00031A15 File Offset: 0x00030A15
			MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
			{
				return new MethodInfo[0];
			}

			// Token: 0x06001A47 RID: 6727 RVA: 0x00031A1D File Offset: 0x00030A1D
			FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A48 RID: 6728 RVA: 0x00031A20 File Offset: 0x00030A20
			FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
			{
				return new FieldInfo[0];
			}

			// Token: 0x06001A49 RID: 6729 RVA: 0x00031A28 File Offset: 0x00030A28
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
			{
				return null;
			}

			// Token: 0x06001A4A RID: 6730 RVA: 0x00031A2B File Offset: 0x00030A2B
			PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
			{
				return null;
			}

			// Token: 0x06001A4B RID: 6731 RVA: 0x00031A2E File Offset: 0x00030A2E
			PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
			{
				return new PropertyInfo[0];
			}

			// Token: 0x06001A4C RID: 6732 RVA: 0x00031A36 File Offset: 0x00030A36
			MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06001A4D RID: 6733 RVA: 0x00031A3E File Offset: 0x00030A3E
			MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
			{
				return new MemberInfo[0];
			}

			// Token: 0x06001A4E RID: 6734 RVA: 0x00031A48 File Offset: 0x00030A48
			object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
			{
				foreach (object obj in this.containerCache)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string nameForControl = this.GetNameForControl((Control)dictionaryEntry.Key);
					if (nameForControl.Equals(name))
					{
						return this.GetProxyForControl((Control)dictionaryEntry.Value);
					}
				}
				throw AxHost.E_FAIL;
			}

			// Token: 0x17000324 RID: 804
			// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00031AD8 File Offset: 0x00030AD8
			Type IReflect.UnderlyingSystemType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06001A50 RID: 6736 RVA: 0x00031ADC File Offset: 0x00030ADC
			internal UnsafeNativeMethods.IExtender GetProxyForControl(Control ctl)
			{
				UnsafeNativeMethods.IExtender extender = null;
				if (this.proxyCache == null)
				{
					this.proxyCache = new Hashtable();
				}
				else
				{
					extender = (UnsafeNativeMethods.IExtender)this.proxyCache[ctl];
				}
				if (extender == null)
				{
					if (ctl != this.parent && !this.GetControlBelongs(ctl))
					{
						AxHost.AxContainer axContainer = AxHost.AxContainer.FindContainerForControl(ctl);
						if (axContainer == null)
						{
							return null;
						}
						extender = new AxHost.AxContainer.ExtenderProxy(ctl, axContainer);
					}
					else
					{
						extender = new AxHost.AxContainer.ExtenderProxy(ctl, this);
					}
					this.proxyCache.Add(ctl, extender);
				}
				return extender;
			}

			// Token: 0x06001A51 RID: 6737 RVA: 0x00031B54 File Offset: 0x00030B54
			internal string GetNameForControl(Control ctl)
			{
				string text = ((ctl.Site != null) ? ctl.Site.Name : ctl.Name);
				if (text != null)
				{
					return text;
				}
				return "";
			}

			// Token: 0x06001A52 RID: 6738 RVA: 0x00031B87 File Offset: 0x00030B87
			internal object GetProxyForContainer()
			{
				return this;
			}

			// Token: 0x06001A53 RID: 6739 RVA: 0x00031B8C File Offset: 0x00030B8C
			internal void AddControl(Control ctl)
			{
				lock (this)
				{
					if (this.containerCache.Contains(ctl))
					{
						throw new ArgumentException(SR.GetString("AXDuplicateControl", new object[] { this.GetNameForControl(ctl) }), "ctl");
					}
					this.containerCache.Add(ctl, ctl);
					if (this.assocContainer == null)
					{
						ISite site = ctl.Site;
						if (site != null)
						{
							this.assocContainer = site.Container;
							IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
							if (componentChangeService != null)
							{
								componentChangeService.ComponentRemoved += this.OnComponentRemoved;
							}
						}
					}
				}
			}

			// Token: 0x06001A54 RID: 6740 RVA: 0x00031C48 File Offset: 0x00030C48
			internal void RemoveControl(Control ctl)
			{
				lock (this)
				{
					if (this.containerCache.Contains(ctl))
					{
						this.containerCache.Remove(ctl);
					}
				}
			}

			// Token: 0x06001A55 RID: 6741 RVA: 0x00031C90 File Offset: 0x00030C90
			private void LockComponents()
			{
				this.lockCount++;
			}

			// Token: 0x06001A56 RID: 6742 RVA: 0x00031CA0 File Offset: 0x00030CA0
			private void UnlockComponents()
			{
				this.lockCount--;
				if (this.lockCount == 0)
				{
					this.components = null;
				}
			}

			// Token: 0x06001A57 RID: 6743 RVA: 0x00031CC0 File Offset: 0x00030CC0
			internal UnsafeNativeMethods.IEnumUnknown EnumControls(Control ctl, int dwOleContF, int dwWhich)
			{
				this.GetComponents();
				this.LockComponents();
				UnsafeNativeMethods.IEnumUnknown enumUnknown;
				try
				{
					ArrayList arrayList = null;
					bool flag = (dwWhich & 1073741824) != 0;
					bool flag2 = (dwWhich & 134217728) != 0;
					bool flag3 = (dwWhich & 268435456) != 0;
					bool flag4 = (dwWhich & 536870912) != 0;
					dwWhich &= -2013265921;
					if (flag3 && flag4)
					{
						throw AxHost.E_INVALIDARG;
					}
					if ((dwWhich == 2 || dwWhich == 3) && (flag3 || flag4))
					{
						throw AxHost.E_INVALIDARG;
					}
					int num = 0;
					int num2 = -1;
					Control[] array = null;
					switch (dwWhich)
					{
					case 1:
					{
						Control parentInternal = ctl.ParentInternal;
						if (parentInternal != null)
						{
							array = parentInternal.GetChildControlsInTabOrder(false);
							if (flag4)
							{
								num2 = ctl.TabIndex;
							}
							else if (flag3)
							{
								num = ctl.TabIndex + 1;
							}
						}
						else
						{
							array = new Control[0];
						}
						ctl = null;
						break;
					}
					case 2:
						arrayList = new ArrayList();
						this.MaybeAdd(arrayList, ctl, flag, dwOleContF, false);
						while (ctl != null)
						{
							AxHost.AxContainer axContainer = AxHost.AxContainer.FindContainerForControl(ctl);
							if (axContainer == null)
							{
								break;
							}
							this.MaybeAdd(arrayList, axContainer.parent, flag, dwOleContF, true);
							ctl = axContainer.parent;
						}
						break;
					case 3:
						array = ctl.GetChildControlsInTabOrder(false);
						ctl = null;
						break;
					case 4:
					{
						Hashtable hashtable = this.GetComponents();
						array = new Control[hashtable.Keys.Count];
						hashtable.Keys.CopyTo(array, 0);
						ctl = this.parent;
						break;
					}
					default:
						throw AxHost.E_INVALIDARG;
					}
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						if (num2 == -1 && array != null)
						{
							num2 = array.Length;
						}
						if (ctl != null)
						{
							this.MaybeAdd(arrayList, ctl, flag, dwOleContF, false);
						}
						for (int i = num; i < num2; i++)
						{
							this.MaybeAdd(arrayList, array[i], flag, dwOleContF, false);
						}
					}
					object[] array2 = new object[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					if (flag2)
					{
						int j = 0;
						int num3 = array2.Length - 1;
						while (j < num3)
						{
							object obj = array2[j];
							array2[j] = array2[num3];
							array2[num3] = obj;
							j++;
							num3--;
						}
					}
					enumUnknown = new AxHost.EnumUnknown(array2);
				}
				finally
				{
					this.UnlockComponents();
				}
				return enumUnknown;
			}

			// Token: 0x06001A58 RID: 6744 RVA: 0x00031EF4 File Offset: 0x00030EF4
			private void MaybeAdd(ArrayList l, Control ctl, bool selected, int dwOleContF, bool ignoreBelong)
			{
				if (!ignoreBelong && ctl != this.parent && !this.GetControlBelongs(ctl))
				{
					return;
				}
				if (selected)
				{
					ISelectionService selectionService = AxHost.GetSelectionService(ctl);
					if (selectionService == null || !selectionService.GetComponentSelected(this))
					{
						return;
					}
				}
				AxHost axHost = ctl as AxHost;
				if (axHost != null && (dwOleContF & 1) != 0)
				{
					l.Add(axHost.GetOcx());
					return;
				}
				if ((dwOleContF & 4) != 0)
				{
					object proxyForControl = this.GetProxyForControl(ctl);
					if (proxyForControl != null)
					{
						l.Add(proxyForControl);
					}
				}
			}

			// Token: 0x06001A59 RID: 6745 RVA: 0x00031F68 File Offset: 0x00030F68
			private void FillComponentsTable(IContainer container)
			{
				if (container != null)
				{
					ComponentCollection componentCollection = container.Components;
					if (componentCollection != null)
					{
						this.components = new Hashtable();
						foreach (object obj in componentCollection)
						{
							IComponent component = (IComponent)obj;
							if (component is Control && component != this.parent && component.Site != null)
							{
								this.components.Add(component, component);
							}
						}
						return;
					}
				}
				bool flag = true;
				Control[] array = new Control[this.containerCache.Values.Count];
				this.containerCache.Values.CopyTo(array, 0);
				if (array != null)
				{
					if (array.Length > 0 && this.components == null)
					{
						this.components = new Hashtable();
						flag = false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (flag && !this.components.Contains(array[i]))
						{
							this.components.Add(array[i], array[i]);
						}
					}
				}
				this.GetAllChildren(this.parent);
			}

			// Token: 0x06001A5A RID: 6746 RVA: 0x00032088 File Offset: 0x00031088
			private void GetAllChildren(Control ctl)
			{
				if (ctl == null)
				{
					return;
				}
				if (this.components == null)
				{
					this.components = new Hashtable();
				}
				if (ctl != this.parent && !this.components.Contains(ctl))
				{
					this.components.Add(ctl, ctl);
				}
				foreach (object obj in ctl.Controls)
				{
					Control control = (Control)obj;
					this.GetAllChildren(control);
				}
			}

			// Token: 0x06001A5B RID: 6747 RVA: 0x0003211C File Offset: 0x0003111C
			private Hashtable GetComponents()
			{
				return this.GetComponents(this.GetParentsContainer());
			}

			// Token: 0x06001A5C RID: 6748 RVA: 0x0003212A File Offset: 0x0003112A
			private Hashtable GetComponents(IContainer cont)
			{
				if (this.lockCount == 0)
				{
					this.FillComponentsTable(cont);
				}
				return this.components;
			}

			// Token: 0x06001A5D RID: 6749 RVA: 0x00032144 File Offset: 0x00031144
			private bool GetControlBelongs(Control ctl)
			{
				Hashtable hashtable = this.GetComponents();
				return hashtable[ctl] != null;
			}

			// Token: 0x06001A5E RID: 6750 RVA: 0x00032168 File Offset: 0x00031168
			private IContainer GetParentIsDesigned()
			{
				ISite site = this.parent.Site;
				if (site != null && site.DesignMode)
				{
					return site.Container;
				}
				return null;
			}

			// Token: 0x06001A5F RID: 6751 RVA: 0x00032194 File Offset: 0x00031194
			private IContainer GetParentsContainer()
			{
				IContainer parentIsDesigned = this.GetParentIsDesigned();
				if (parentIsDesigned != null)
				{
					return parentIsDesigned;
				}
				return this.assocContainer;
			}

			// Token: 0x06001A60 RID: 6752 RVA: 0x000321B4 File Offset: 0x000311B4
			private bool RegisterControl(AxHost ctl)
			{
				ISite site = ctl.Site;
				if (site != null)
				{
					IContainer container = site.Container;
					if (container != null)
					{
						if (this.assocContainer != null)
						{
							return container == this.assocContainer;
						}
						this.assocContainer = container;
						IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							componentChangeService.ComponentRemoved += this.OnComponentRemoved;
						}
						return true;
					}
				}
				return false;
			}

			// Token: 0x06001A61 RID: 6753 RVA: 0x0003221C File Offset: 0x0003121C
			private void OnComponentRemoved(object sender, ComponentEventArgs e)
			{
				Control control = e.Component as Control;
				if (sender == this.assocContainer && control != null)
				{
					this.RemoveControl(control);
				}
			}

			// Token: 0x06001A62 RID: 6754 RVA: 0x00032248 File Offset: 0x00031248
			internal static AxHost.AxContainer FindContainerForControl(Control ctl)
			{
				AxHost axHost = ctl as AxHost;
				if (axHost != null)
				{
					if (axHost.container != null)
					{
						return axHost.container;
					}
					ContainerControl containingControl = axHost.ContainingControl;
					if (containingControl != null)
					{
						AxHost.AxContainer axContainer = containingControl.CreateAxContainer();
						if (axContainer.RegisterControl(axHost))
						{
							axContainer.AddControl(axHost);
							return axContainer;
						}
					}
				}
				return null;
			}

			// Token: 0x06001A63 RID: 6755 RVA: 0x00032292 File Offset: 0x00031292
			internal void OnInPlaceDeactivate(AxHost site)
			{
				if (this.siteActive == site)
				{
					this.siteActive = null;
					if (site.GetSiteOwnsDeactivation())
					{
						this.parent.ActiveControl = null;
					}
				}
			}

			// Token: 0x06001A64 RID: 6756 RVA: 0x000322B8 File Offset: 0x000312B8
			internal void OnUIDeactivate(AxHost site)
			{
				this.siteUIActive = null;
				site.RemoveSelectionHandler();
				site.SetSelectionStyle(1);
				site.editMode = 0;
				if (site.GetSiteOwnsDeactivation())
				{
					ContainerControl containingControl = site.ContainingControl;
				}
			}

			// Token: 0x06001A65 RID: 6757 RVA: 0x000322F4 File Offset: 0x000312F4
			internal void OnUIActivate(AxHost site)
			{
				if (this.siteUIActive == site)
				{
					return;
				}
				if (this.siteUIActive != null && this.siteUIActive != site)
				{
					AxHost axHost = this.siteUIActive;
					bool axState = axHost.GetAxState(AxHost.ownDisposing);
					try
					{
						axHost.SetAxState(AxHost.ownDisposing, true);
						axHost.GetInPlaceObject().UIDeactivate();
					}
					finally
					{
						axHost.SetAxState(AxHost.ownDisposing, axState);
					}
				}
				site.AddSelectionHandler();
				this.siteUIActive = site;
				ContainerControl containingControl = site.ContainingControl;
				if (containingControl != null)
				{
					containingControl.ActiveControl = site;
				}
			}

			// Token: 0x06001A66 RID: 6758 RVA: 0x00032384 File Offset: 0x00031384
			private void ListAxControls(ArrayList list, bool fuseOcx)
			{
				Hashtable hashtable = this.GetComponents();
				if (hashtable == null)
				{
					return;
				}
				Control[] array = new Control[hashtable.Keys.Count];
				hashtable.Keys.CopyTo(array, 0);
				if (array != null)
				{
					foreach (Control control in array)
					{
						AxHost axHost = control as AxHost;
						if (axHost != null)
						{
							if (fuseOcx)
							{
								list.Add(axHost.GetOcx());
							}
							else
							{
								list.Add(control);
							}
						}
					}
				}
			}

			// Token: 0x06001A67 RID: 6759 RVA: 0x000323F6 File Offset: 0x000313F6
			internal void ControlCreated(AxHost invoker)
			{
				if (this.formAlreadyCreated)
				{
					if (invoker.IsUserMode() && invoker.AwaitingDefreezing())
					{
						invoker.Freeze(false);
						return;
					}
				}
				else
				{
					this.parent.CreateAxContainer();
				}
			}

			// Token: 0x06001A68 RID: 6760 RVA: 0x00032424 File Offset: 0x00031424
			internal void FormCreated()
			{
				if (this.formAlreadyCreated)
				{
					return;
				}
				this.formAlreadyCreated = true;
				ArrayList arrayList = new ArrayList();
				this.ListAxControls(arrayList, false);
				AxHost[] array = new AxHost[arrayList.Count];
				arrayList.CopyTo(array, 0);
				foreach (AxHost axHost in array)
				{
					if (axHost.GetOcState() >= 2 && axHost.IsUserMode() && axHost.AwaitingDefreezing())
					{
						axHost.Freeze(false);
					}
				}
			}

			// Token: 0x06001A69 RID: 6761 RVA: 0x00032495 File Offset: 0x00031495
			int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
			{
				if (ppmkOut != null)
				{
					ppmkOut[0] = null;
				}
				return -2147467263;
			}

			// Token: 0x06001A6A RID: 6762 RVA: 0x000324A8 File Offset: 0x000314A8
			int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
			{
				ppenum = null;
				if ((grfFlags & 1) != 0)
				{
					ArrayList arrayList = new ArrayList();
					this.ListAxControls(arrayList, true);
					if (arrayList.Count > 0)
					{
						object[] array = new object[arrayList.Count];
						arrayList.CopyTo(array, 0);
						ppenum = new AxHost.EnumUnknown(array);
						return 0;
					}
				}
				ppenum = new AxHost.EnumUnknown(null);
				return 0;
			}

			// Token: 0x06001A6B RID: 6763 RVA: 0x000324FB File Offset: 0x000314FB
			int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
			{
				return -2147467263;
			}

			// Token: 0x06001A6C RID: 6764 RVA: 0x00032502 File Offset: 0x00031502
			IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
			{
				return this.parent.Handle;
			}

			// Token: 0x06001A6D RID: 6765 RVA: 0x0003250F File Offset: 0x0003150F
			int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
			{
				return 0;
			}

			// Token: 0x06001A6E RID: 6766 RVA: 0x00032512 File Offset: 0x00031512
			int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
			{
				return -2147467263;
			}

			// Token: 0x06001A6F RID: 6767 RVA: 0x00032519 File Offset: 0x00031519
			int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
			{
				return -2147467263;
			}

			// Token: 0x06001A70 RID: 6768 RVA: 0x00032520 File Offset: 0x00031520
			int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
			{
				return -2147467263;
			}

			// Token: 0x06001A71 RID: 6769 RVA: 0x00032527 File Offset: 0x00031527
			internal void OnExitEditMode(AxHost ctl)
			{
				if (this.ctlInEditMode == null || this.ctlInEditMode != ctl)
				{
					return;
				}
				this.ctlInEditMode = null;
			}

			// Token: 0x06001A72 RID: 6770 RVA: 0x00032544 File Offset: 0x00031544
			int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
			{
				if (this.siteUIActive != null && this.siteUIActive.iOleInPlaceActiveObjectExternal != pActiveObject)
				{
					if (this.siteUIActive.iOleInPlaceActiveObjectExternal != null)
					{
						Marshal.ReleaseComObject(this.siteUIActive.iOleInPlaceActiveObjectExternal);
					}
					this.siteUIActive.iOleInPlaceActiveObjectExternal = pActiveObject;
				}
				if (pActiveObject == null)
				{
					if (this.ctlInEditMode != null)
					{
						this.ctlInEditMode.editMode = 0;
						this.ctlInEditMode = null;
					}
					return 0;
				}
				AxHost axHost = null;
				if (pActiveObject is UnsafeNativeMethods.IOleObject)
				{
					UnsafeNativeMethods.IOleObject oleObject = (UnsafeNativeMethods.IOleObject)pActiveObject;
					try
					{
						UnsafeNativeMethods.IOleClientSite clientSite = oleObject.GetClientSite();
						if (clientSite is AxHost.OleInterfaces)
						{
							axHost = ((AxHost.OleInterfaces)clientSite).GetAxHost();
						}
					}
					catch (COMException)
					{
					}
					if (this.ctlInEditMode != null)
					{
						this.ctlInEditMode.SetSelectionStyle(1);
						this.ctlInEditMode.editMode = 0;
					}
					if (axHost == null)
					{
						this.ctlInEditMode = null;
					}
					else if (!axHost.IsUserMode())
					{
						this.ctlInEditMode = axHost;
						axHost.editMode = 1;
						axHost.AddSelectionHandler();
						axHost.SetSelectionStyle(2);
					}
				}
				return 0;
			}

			// Token: 0x06001A73 RID: 6771 RVA: 0x00032644 File Offset: 0x00031644
			int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
			{
				return 0;
			}

			// Token: 0x06001A74 RID: 6772 RVA: 0x00032647 File Offset: 0x00031647
			int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
			{
				return -2147467263;
			}

			// Token: 0x06001A75 RID: 6773 RVA: 0x0003264E File Offset: 0x0003164E
			int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
			{
				return -2147467263;
			}

			// Token: 0x06001A76 RID: 6774 RVA: 0x00032655 File Offset: 0x00031655
			int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
			{
				return -2147467263;
			}

			// Token: 0x06001A77 RID: 6775 RVA: 0x0003265C File Offset: 0x0003165C
			int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
			{
				return -2147467263;
			}

			// Token: 0x06001A78 RID: 6776 RVA: 0x00032663 File Offset: 0x00031663
			int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
			{
				return 1;
			}

			// Token: 0x040012C0 RID: 4800
			private const int GC_CHILD = 1;

			// Token: 0x040012C1 RID: 4801
			private const int GC_LASTSIBLING = 2;

			// Token: 0x040012C2 RID: 4802
			private const int GC_FIRSTSIBLING = 4;

			// Token: 0x040012C3 RID: 4803
			private const int GC_CONTAINER = 32;

			// Token: 0x040012C4 RID: 4804
			private const int GC_PREVSIBLING = 64;

			// Token: 0x040012C5 RID: 4805
			private const int GC_NEXTSIBLING = 128;

			// Token: 0x040012C6 RID: 4806
			internal ContainerControl parent;

			// Token: 0x040012C7 RID: 4807
			private IContainer assocContainer;

			// Token: 0x040012C8 RID: 4808
			private AxHost siteUIActive;

			// Token: 0x040012C9 RID: 4809
			private AxHost siteActive;

			// Token: 0x040012CA RID: 4810
			private bool formAlreadyCreated;

			// Token: 0x040012CB RID: 4811
			private Hashtable containerCache = new Hashtable();

			// Token: 0x040012CC RID: 4812
			private int lockCount;

			// Token: 0x040012CD RID: 4813
			private Hashtable components;

			// Token: 0x040012CE RID: 4814
			private Hashtable proxyCache;

			// Token: 0x040012CF RID: 4815
			private AxHost ctlInEditMode;

			// Token: 0x0200022B RID: 555
			private class ExtenderProxy : UnsafeNativeMethods.IExtender, UnsafeNativeMethods.IVBGetControl, UnsafeNativeMethods.IGetVBAObject, UnsafeNativeMethods.IGetOleObject, IReflect
			{
				// Token: 0x06001A79 RID: 6777 RVA: 0x00032666 File Offset: 0x00031666
				internal ExtenderProxy(Control principal, AxHost.AxContainer container)
				{
					this.pRef = new WeakReference(principal);
					this.pContainer = new WeakReference(container);
				}

				// Token: 0x06001A7A RID: 6778 RVA: 0x00032686 File Offset: 0x00031686
				private Control GetP()
				{
					return (Control)this.pRef.Target;
				}

				// Token: 0x06001A7B RID: 6779 RVA: 0x00032698 File Offset: 0x00031698
				private AxHost.AxContainer GetC()
				{
					return (AxHost.AxContainer)this.pContainer.Target;
				}

				// Token: 0x06001A7C RID: 6780 RVA: 0x000326AA File Offset: 0x000316AA
				int UnsafeNativeMethods.IVBGetControl.EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum)
				{
					ppenum = this.GetC().EnumControls(this.GetP(), dwOleContF, dwWhich);
					return 0;
				}

				// Token: 0x06001A7D RID: 6781 RVA: 0x000326C4 File Offset: 0x000316C4
				object UnsafeNativeMethods.IGetOleObject.GetOleObject(ref Guid riid)
				{
					if (!riid.Equals(AxHost.ioleobject_Guid))
					{
						throw AxHost.E_INVALIDARG;
					}
					Control p = this.GetP();
					if (p != null && p is AxHost)
					{
						return ((AxHost)p).GetOcx();
					}
					throw AxHost.E_FAIL;
				}

				// Token: 0x06001A7E RID: 6782 RVA: 0x00032707 File Offset: 0x00031707
				int UnsafeNativeMethods.IGetVBAObject.GetObject(ref Guid riid, UnsafeNativeMethods.IVBFormat[] rval, int dwReserved)
				{
					if (rval == null || riid.Equals(Guid.Empty))
					{
						return -2147024809;
					}
					if (riid.Equals(AxHost.ivbformat_Guid))
					{
						rval[0] = new AxHost.VBFormat();
						return 0;
					}
					rval[0] = null;
					return -2147467262;
				}

				// Token: 0x17000325 RID: 805
				// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00032740 File Offset: 0x00031740
				// (set) Token: 0x06001A80 RID: 6784 RVA: 0x00032764 File Offset: 0x00031764
				public int Align
				{
					get
					{
						int num = (int)this.GetP().Dock;
						if (num < 0 || num > 4)
						{
							num = 0;
						}
						return num;
					}
					set
					{
						this.GetP().Dock = (DockStyle)value;
					}
				}

				// Token: 0x17000326 RID: 806
				// (get) Token: 0x06001A81 RID: 6785 RVA: 0x00032772 File Offset: 0x00031772
				// (set) Token: 0x06001A82 RID: 6786 RVA: 0x00032784 File Offset: 0x00031784
				public uint BackColor
				{
					get
					{
						return AxHost.GetOleColorFromColor(this.GetP().BackColor);
					}
					set
					{
						this.GetP().BackColor = AxHost.GetColorFromOleColor(value);
					}
				}

				// Token: 0x17000327 RID: 807
				// (get) Token: 0x06001A83 RID: 6787 RVA: 0x00032797 File Offset: 0x00031797
				// (set) Token: 0x06001A84 RID: 6788 RVA: 0x000327A4 File Offset: 0x000317A4
				public bool Enabled
				{
					get
					{
						return this.GetP().Enabled;
					}
					set
					{
						this.GetP().Enabled = value;
					}
				}

				// Token: 0x17000328 RID: 808
				// (get) Token: 0x06001A85 RID: 6789 RVA: 0x000327B2 File Offset: 0x000317B2
				// (set) Token: 0x06001A86 RID: 6790 RVA: 0x000327C4 File Offset: 0x000317C4
				public uint ForeColor
				{
					get
					{
						return AxHost.GetOleColorFromColor(this.GetP().ForeColor);
					}
					set
					{
						this.GetP().ForeColor = AxHost.GetColorFromOleColor(value);
					}
				}

				// Token: 0x17000329 RID: 809
				// (get) Token: 0x06001A87 RID: 6791 RVA: 0x000327D7 File Offset: 0x000317D7
				// (set) Token: 0x06001A88 RID: 6792 RVA: 0x000327EA File Offset: 0x000317EA
				public int Height
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Height, false);
					}
					set
					{
						this.GetP().Height = AxHost.Twip2Pixel(value, false);
					}
				}

				// Token: 0x1700032A RID: 810
				// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000327FE File Offset: 0x000317FE
				// (set) Token: 0x06001A8A RID: 6794 RVA: 0x00032811 File Offset: 0x00031811
				public int Left
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Left, true);
					}
					set
					{
						this.GetP().Left = AxHost.Twip2Pixel(value, true);
					}
				}

				// Token: 0x1700032B RID: 811
				// (get) Token: 0x06001A8B RID: 6795 RVA: 0x00032825 File Offset: 0x00031825
				public object Parent
				{
					get
					{
						return this.GetC().GetProxyForControl(this.GetC().parent);
					}
				}

				// Token: 0x1700032C RID: 812
				// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0003283D File Offset: 0x0003183D
				// (set) Token: 0x06001A8D RID: 6797 RVA: 0x0003284B File Offset: 0x0003184B
				public short TabIndex
				{
					get
					{
						return (short)this.GetP().TabIndex;
					}
					set
					{
						this.GetP().TabIndex = (int)value;
					}
				}

				// Token: 0x1700032D RID: 813
				// (get) Token: 0x06001A8E RID: 6798 RVA: 0x00032859 File Offset: 0x00031859
				// (set) Token: 0x06001A8F RID: 6799 RVA: 0x00032866 File Offset: 0x00031866
				public bool TabStop
				{
					get
					{
						return this.GetP().TabStop;
					}
					set
					{
						this.GetP().TabStop = value;
					}
				}

				// Token: 0x1700032E RID: 814
				// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00032874 File Offset: 0x00031874
				// (set) Token: 0x06001A91 RID: 6801 RVA: 0x00032887 File Offset: 0x00031887
				public int Top
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Top, false);
					}
					set
					{
						this.GetP().Top = AxHost.Twip2Pixel(value, false);
					}
				}

				// Token: 0x1700032F RID: 815
				// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0003289B File Offset: 0x0003189B
				// (set) Token: 0x06001A93 RID: 6803 RVA: 0x000328A8 File Offset: 0x000318A8
				public bool Visible
				{
					get
					{
						return this.GetP().Visible;
					}
					set
					{
						this.GetP().Visible = value;
					}
				}

				// Token: 0x17000330 RID: 816
				// (get) Token: 0x06001A94 RID: 6804 RVA: 0x000328B6 File Offset: 0x000318B6
				// (set) Token: 0x06001A95 RID: 6805 RVA: 0x000328C9 File Offset: 0x000318C9
				public int Width
				{
					get
					{
						return AxHost.Pixel2Twip(this.GetP().Width, true);
					}
					set
					{
						this.GetP().Width = AxHost.Twip2Pixel(value, true);
					}
				}

				// Token: 0x17000331 RID: 817
				// (get) Token: 0x06001A96 RID: 6806 RVA: 0x000328DD File Offset: 0x000318DD
				public string Name
				{
					get
					{
						return this.GetC().GetNameForControl(this.GetP());
					}
				}

				// Token: 0x17000332 RID: 818
				// (get) Token: 0x06001A97 RID: 6807 RVA: 0x000328F0 File Offset: 0x000318F0
				public IntPtr Hwnd
				{
					get
					{
						return this.GetP().Handle;
					}
				}

				// Token: 0x17000333 RID: 819
				// (get) Token: 0x06001A98 RID: 6808 RVA: 0x000328FD File Offset: 0x000318FD
				public object Container
				{
					get
					{
						return this.GetC().GetProxyForContainer();
					}
				}

				// Token: 0x17000334 RID: 820
				// (get) Token: 0x06001A99 RID: 6809 RVA: 0x0003290A File Offset: 0x0003190A
				// (set) Token: 0x06001A9A RID: 6810 RVA: 0x00032917 File Offset: 0x00031917
				public string Text
				{
					get
					{
						return this.GetP().Text;
					}
					set
					{
						this.GetP().Text = value;
					}
				}

				// Token: 0x06001A9B RID: 6811 RVA: 0x00032925 File Offset: 0x00031925
				public void Move(object left, object top, object width, object height)
				{
				}

				// Token: 0x06001A9C RID: 6812 RVA: 0x00032927 File Offset: 0x00031927
				MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
				{
					return null;
				}

				// Token: 0x06001A9D RID: 6813 RVA: 0x0003292A File Offset: 0x0003192A
				MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
				{
					return null;
				}

				// Token: 0x06001A9E RID: 6814 RVA: 0x00032930 File Offset: 0x00031930
				MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
				{
					return new MethodInfo[] { base.GetType().GetMethod("Move") };
				}

				// Token: 0x06001A9F RID: 6815 RVA: 0x00032958 File Offset: 0x00031958
				FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
				{
					return null;
				}

				// Token: 0x06001AA0 RID: 6816 RVA: 0x0003295B File Offset: 0x0003195B
				FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
				{
					return new FieldInfo[0];
				}

				// Token: 0x06001AA1 RID: 6817 RVA: 0x00032964 File Offset: 0x00031964
				PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
				{
					PropertyInfo propertyInfo = this.GetP().GetType().GetProperty(name, bindingAttr);
					if (propertyInfo == null)
					{
						propertyInfo = base.GetType().GetProperty(name, bindingAttr);
					}
					return propertyInfo;
				}

				// Token: 0x06001AA2 RID: 6818 RVA: 0x00032998 File Offset: 0x00031998
				PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
				{
					PropertyInfo propertyInfo = this.GetP().GetType().GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
					if (propertyInfo == null)
					{
						propertyInfo = base.GetType().GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
					}
					return propertyInfo;
				}

				// Token: 0x06001AA3 RID: 6819 RVA: 0x000329D8 File Offset: 0x000319D8
				PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
				{
					PropertyInfo[] properties = base.GetType().GetProperties(bindingAttr);
					PropertyInfo[] properties2 = this.GetP().GetType().GetProperties(bindingAttr);
					if (properties == null)
					{
						return properties2;
					}
					if (properties2 == null)
					{
						return properties;
					}
					int num = 0;
					PropertyInfo[] array = new PropertyInfo[properties.Length + properties2.Length];
					foreach (PropertyInfo propertyInfo in properties)
					{
						array[num++] = propertyInfo;
					}
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						array[num++] = propertyInfo2;
					}
					return array;
				}

				// Token: 0x06001AA4 RID: 6820 RVA: 0x00032A6C File Offset: 0x00031A6C
				MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
				{
					MemberInfo[] array = this.GetP().GetType().GetMember(name, bindingAttr);
					if (array == null)
					{
						array = base.GetType().GetMember(name, bindingAttr);
					}
					return array;
				}

				// Token: 0x06001AA5 RID: 6821 RVA: 0x00032AA0 File Offset: 0x00031AA0
				MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
				{
					MemberInfo[] members = base.GetType().GetMembers(bindingAttr);
					MemberInfo[] members2 = this.GetP().GetType().GetMembers(bindingAttr);
					if (members == null)
					{
						return members2;
					}
					if (members2 == null)
					{
						return members;
					}
					MemberInfo[] array = new MemberInfo[members.Length + members2.Length];
					Array.Copy(members, 0, array, 0, members.Length);
					Array.Copy(members2, 0, array, members.Length, members2.Length);
					return array;
				}

				// Token: 0x06001AA6 RID: 6822 RVA: 0x00032B00 File Offset: 0x00031B00
				object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
				{
					object obj;
					try
					{
						obj = base.GetType().InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
					}
					catch (MissingMethodException)
					{
						obj = this.GetP().GetType().InvokeMember(name, invokeAttr, binder, this.GetP(), args, modifiers, culture, namedParameters);
					}
					return obj;
				}

				// Token: 0x17000335 RID: 821
				// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x00032B60 File Offset: 0x00031B60
				Type IReflect.UnderlyingSystemType
				{
					get
					{
						return null;
					}
				}

				// Token: 0x040012D0 RID: 4816
				private WeakReference pRef;

				// Token: 0x040012D1 RID: 4817
				private WeakReference pContainer;
			}
		}

		// Token: 0x0200022C RID: 556
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class StateConverter : TypeConverter
		{
			// Token: 0x06001AA8 RID: 6824 RVA: 0x00032B63 File Offset: 0x00031B63
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06001AA9 RID: 6825 RVA: 0x00032B7C File Offset: 0x00031B7C
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001AAA RID: 6826 RVA: 0x00032B98 File Offset: 0x00031B98
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is byte[])
				{
					MemoryStream memoryStream = new MemoryStream((byte[])value);
					return new AxHost.State(memoryStream);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x06001AAB RID: 6827 RVA: 0x00032BCC File Offset: 0x00031BCC
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (destinationType != typeof(byte[]))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					AxHost.State state = (AxHost.State)value;
					state.Save(memoryStream);
					memoryStream.Close();
					return memoryStream.ToArray();
				}
				return new byte[0];
			}
		}

		// Token: 0x0200022D RID: 557
		[TypeConverter(typeof(TypeConverter))]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[Serializable]
		public class State : ISerializable
		{
			// Token: 0x06001AAD RID: 6829 RVA: 0x00032C34 File Offset: 0x00031C34
			internal State(MemoryStream ms, int storageType, AxHost ctl, AxHost.PropertyBagStream propBag)
			{
				this.type = storageType;
				this.propBag = propBag;
				this.length = (int)ms.Length;
				this.ms = ms;
				this.manualUpdate = ctl.GetAxState(AxHost.manualUpdate);
				this.licenseKey = ctl.GetLicenseKey();
			}

			// Token: 0x06001AAE RID: 6830 RVA: 0x00032C8E File Offset: 0x00031C8E
			internal State(AxHost.PropertyBagStream propBag)
			{
				this.propBag = propBag;
			}

			// Token: 0x06001AAF RID: 6831 RVA: 0x00032CA4 File Offset: 0x00031CA4
			internal State(MemoryStream ms)
			{
				this.ms = ms;
				this.length = (int)ms.Length;
				this.InitializeFromStream(ms);
			}

			// Token: 0x06001AB0 RID: 6832 RVA: 0x00032CCE File Offset: 0x00031CCE
			internal State(AxHost ctl)
			{
				this.CreateStorage();
				this.manualUpdate = ctl.GetAxState(AxHost.manualUpdate);
				this.licenseKey = ctl.GetLicenseKey();
				this.type = 2;
			}

			// Token: 0x06001AB1 RID: 6833 RVA: 0x00032D07 File Offset: 0x00031D07
			public State(Stream ms, int storageType, bool manualUpdate, string licKey)
			{
				this.type = storageType;
				this.length = (int)ms.Length;
				this.manualUpdate = manualUpdate;
				this.licenseKey = licKey;
				this.InitializeBufferFromStream(ms);
			}

			// Token: 0x06001AB2 RID: 6834 RVA: 0x00032D40 File Offset: 0x00031D40
			protected State(SerializationInfo info, StreamingContext context)
			{
				SerializationInfoEnumerator enumerator = info.GetEnumerator();
				if (enumerator == null)
				{
					return;
				}
				while (enumerator.MoveNext())
				{
					if (string.Compare(enumerator.Name, "Data", true, CultureInfo.InvariantCulture) == 0)
					{
						try
						{
							byte[] array = (byte[])enumerator.Value;
							if (array != null)
							{
								this.InitializeFromStream(new MemoryStream(array));
							}
							continue;
						}
						catch (Exception)
						{
							continue;
						}
					}
					if (string.Compare(enumerator.Name, "PropertyBagBinary", true, CultureInfo.InvariantCulture) == 0)
					{
						try
						{
							byte[] array2 = (byte[])enumerator.Value;
							if (array2 != null)
							{
								this.propBag = new AxHost.PropertyBagStream();
								this.propBag.Read(new MemoryStream(array2));
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}

			// Token: 0x17000336 RID: 822
			// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x00032E10 File Offset: 0x00031E10
			// (set) Token: 0x06001AB4 RID: 6836 RVA: 0x00032E18 File Offset: 0x00031E18
			internal int Type
			{
				get
				{
					return this.type;
				}
				set
				{
					this.type = value;
				}
			}

			// Token: 0x06001AB5 RID: 6837 RVA: 0x00032E21 File Offset: 0x00031E21
			internal bool _GetManualUpdate()
			{
				return this.manualUpdate;
			}

			// Token: 0x06001AB6 RID: 6838 RVA: 0x00032E29 File Offset: 0x00031E29
			internal string _GetLicenseKey()
			{
				return this.licenseKey;
			}

			// Token: 0x06001AB7 RID: 6839 RVA: 0x00032E34 File Offset: 0x00031E34
			private void CreateStorage()
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.buffer != null)
				{
					intPtr = UnsafeNativeMethods.GlobalAlloc(2, this.length);
					IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
					try
					{
						if (intPtr2 != IntPtr.Zero)
						{
							Marshal.Copy(this.buffer, 0, intPtr2, this.length);
						}
					}
					finally
					{
						UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
					}
				}
				bool flag = false;
				try
				{
					this.iLockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(new HandleRef(null, intPtr), true);
					if (this.buffer == null)
					{
						this.storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(this.iLockBytes, 4114, 0);
					}
					else
					{
						this.storage = UnsafeNativeMethods.StgOpenStorageOnILockBytes(this.iLockBytes, null, 18, 0, 0);
					}
				}
				catch (Exception)
				{
					flag = true;
				}
				if (flag)
				{
					if (this.iLockBytes == null && intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.GlobalFree(new HandleRef(null, intPtr));
					}
					else
					{
						this.iLockBytes = null;
					}
					this.storage = null;
				}
			}

			// Token: 0x06001AB8 RID: 6840 RVA: 0x00032F40 File Offset: 0x00031F40
			internal UnsafeNativeMethods.IPropertyBag GetPropBag()
			{
				return this.propBag;
			}

			// Token: 0x06001AB9 RID: 6841 RVA: 0x00032F48 File Offset: 0x00031F48
			internal UnsafeNativeMethods.IStorage GetStorage()
			{
				if (this.storage == null)
				{
					this.CreateStorage();
				}
				return this.storage;
			}

			// Token: 0x06001ABA RID: 6842 RVA: 0x00032F60 File Offset: 0x00031F60
			internal UnsafeNativeMethods.IStream GetStream()
			{
				if (this.ms == null)
				{
					if (this.buffer == null)
					{
						return null;
					}
					this.ms = new MemoryStream(this.buffer);
				}
				else
				{
					this.ms.Seek(0L, SeekOrigin.Begin);
				}
				return new UnsafeNativeMethods.ComStreamFromDataStream(this.ms);
			}

			// Token: 0x06001ABB RID: 6843 RVA: 0x00032FAC File Offset: 0x00031FAC
			private void InitializeFromStream(Stream ids)
			{
				BinaryReader binaryReader = new BinaryReader(ids);
				this.type = binaryReader.ReadInt32();
				binaryReader.ReadInt32();
				this.manualUpdate = binaryReader.ReadBoolean();
				int num = binaryReader.ReadInt32();
				if (num != 0)
				{
					this.licenseKey = new string(binaryReader.ReadChars(num));
				}
				for (int i = binaryReader.ReadInt32(); i > 0; i--)
				{
					int num2 = binaryReader.ReadInt32();
					ids.Position += (long)num2;
				}
				this.length = binaryReader.ReadInt32();
				if (this.length > 0)
				{
					this.buffer = binaryReader.ReadBytes(this.length);
				}
			}

			// Token: 0x06001ABC RID: 6844 RVA: 0x0003304C File Offset: 0x0003204C
			private void InitializeBufferFromStream(Stream ids)
			{
				BinaryReader binaryReader = new BinaryReader(ids);
				this.length = binaryReader.ReadInt32();
				if (this.length > 0)
				{
					this.buffer = binaryReader.ReadBytes(this.length);
				}
			}

			// Token: 0x06001ABD RID: 6845 RVA: 0x00033088 File Offset: 0x00032088
			internal AxHost.State RefreshStorage(UnsafeNativeMethods.IPersistStorage iPersistStorage)
			{
				if (this.storage == null || this.iLockBytes == null)
				{
					return null;
				}
				iPersistStorage.Save(this.storage, true);
				this.storage.Commit(0);
				iPersistStorage.HandsOffStorage();
				try
				{
					this.buffer = null;
					this.ms = null;
					NativeMethods.STATSTG statstg = new NativeMethods.STATSTG();
					this.iLockBytes.Stat(statstg, 1);
					this.length = (int)statstg.cbSize;
					this.buffer = new byte[this.length];
					IntPtr hglobalFromILockBytes = UnsafeNativeMethods.GetHGlobalFromILockBytes(this.iLockBytes);
					IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, hglobalFromILockBytes));
					try
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.Copy(intPtr, this.buffer, 0, this.length);
						}
						else
						{
							this.length = 0;
							this.buffer = null;
						}
					}
					finally
					{
						UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, hglobalFromILockBytes));
					}
				}
				finally
				{
					iPersistStorage.SaveCompleted(this.storage);
				}
				return this;
			}

			// Token: 0x06001ABE RID: 6846 RVA: 0x0003318C File Offset: 0x0003218C
			internal void Save(MemoryStream stream)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				binaryWriter.Write(this.type);
				binaryWriter.Write(this.VERSION);
				binaryWriter.Write(this.manualUpdate);
				if (this.licenseKey != null)
				{
					binaryWriter.Write(this.licenseKey.Length);
					binaryWriter.Write(this.licenseKey.ToCharArray());
				}
				else
				{
					binaryWriter.Write(0);
				}
				binaryWriter.Write(0);
				binaryWriter.Write(this.length);
				if (this.buffer != null)
				{
					binaryWriter.Write(this.buffer);
					return;
				}
				if (this.ms != null)
				{
					this.ms.Position = 0L;
					this.ms.WriteTo(stream);
				}
			}

			// Token: 0x06001ABF RID: 6847 RVA: 0x00033240 File Offset: 0x00032240
			void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
			{
				IntSecurity.UnmanagedCode.Demand();
				MemoryStream memoryStream = new MemoryStream();
				this.Save(memoryStream);
				si.AddValue("Data", memoryStream.ToArray());
				if (this.propBag != null)
				{
					try
					{
						memoryStream = new MemoryStream();
						this.propBag.Write(memoryStream);
						si.AddValue("PropertyBagBinary", memoryStream.ToArray());
					}
					catch (Exception)
					{
					}
				}
			}

			// Token: 0x040012D2 RID: 4818
			private int VERSION = 1;

			// Token: 0x040012D3 RID: 4819
			private int length;

			// Token: 0x040012D4 RID: 4820
			private byte[] buffer;

			// Token: 0x040012D5 RID: 4821
			internal int type;

			// Token: 0x040012D6 RID: 4822
			private MemoryStream ms;

			// Token: 0x040012D7 RID: 4823
			private UnsafeNativeMethods.IStorage storage;

			// Token: 0x040012D8 RID: 4824
			private UnsafeNativeMethods.ILockBytes iLockBytes;

			// Token: 0x040012D9 RID: 4825
			private bool manualUpdate;

			// Token: 0x040012DA RID: 4826
			private string licenseKey;

			// Token: 0x040012DB RID: 4827
			private AxHost.PropertyBagStream propBag;
		}

		// Token: 0x0200022E RID: 558
		internal class PropertyBagStream : UnsafeNativeMethods.IPropertyBag
		{
			// Token: 0x06001AC0 RID: 6848 RVA: 0x000332B8 File Offset: 0x000322B8
			internal void Read(Stream stream)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				try
				{
					this.bag = (Hashtable)binaryFormatter.Deserialize(stream);
				}
				catch
				{
					this.bag = new Hashtable();
				}
			}

			// Token: 0x06001AC1 RID: 6849 RVA: 0x00033300 File Offset: 0x00032300
			int UnsafeNativeMethods.IPropertyBag.Read(string pszPropName, ref object pVar, UnsafeNativeMethods.IErrorLog pErrorLog)
			{
				if (!this.bag.Contains(pszPropName))
				{
					return -2147024809;
				}
				pVar = this.bag[pszPropName];
				if (pVar != null)
				{
					return 0;
				}
				return -2147024809;
			}

			// Token: 0x06001AC2 RID: 6850 RVA: 0x0003332F File Offset: 0x0003232F
			int UnsafeNativeMethods.IPropertyBag.Write(string pszPropName, ref object pVar)
			{
				if (pVar != null && !pVar.GetType().IsSerializable)
				{
					return 0;
				}
				this.bag[pszPropName] = pVar;
				return 0;
			}

			// Token: 0x06001AC3 RID: 6851 RVA: 0x00033354 File Offset: 0x00032354
			internal void Write(Stream stream)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, this.bag);
			}

			// Token: 0x040012DC RID: 4828
			private Hashtable bag = new Hashtable();
		}

		// Token: 0x0200022F RID: 559
		// (Invoke) Token: 0x06001AC6 RID: 6854
		protected delegate void AboutBoxDelegate();

		// Token: 0x02000231 RID: 561
		[ComVisible(false)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public class AxComponentEditor : WindowsFormsComponentEditor
		{
			// Token: 0x06001ACF RID: 6863 RVA: 0x000333E8 File Offset: 0x000323E8
			public override bool EditComponent(ITypeDescriptorContext context, object obj, IWin32Window parent)
			{
				AxHost axHost = obj as AxHost;
				if (axHost != null)
				{
					try
					{
						((UnsafeNativeMethods.IOleControlSite)axHost.oleSite).ShowPropertyFrame();
						return true;
					}
					catch (Exception)
					{
						throw;
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x02000232 RID: 562
		internal class AxPropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x06001AD1 RID: 6865 RVA: 0x00033430 File Offset: 0x00032430
			internal AxPropertyDescriptor(PropertyDescriptor baseProp, AxHost owner)
				: base(baseProp)
			{
				this.baseProp = baseProp;
				this.owner = owner;
				this.dispid = (DispIdAttribute)baseProp.Attributes[typeof(DispIdAttribute)];
				if (this.dispid != null)
				{
					if (!this.IsBrowsable && !this.IsReadOnly)
					{
						Guid propertyPage = this.GetPropertyPage(this.dispid.Value);
						if (!Guid.Empty.Equals(propertyPage))
						{
							this.AddAttribute(new BrowsableAttribute(true));
						}
					}
					CategoryAttribute categoryForDispid = owner.GetCategoryForDispid(this.dispid.Value);
					if (categoryForDispid != null)
					{
						this.AddAttribute(categoryForDispid);
					}
					if (this.PropertyType.GUID.Equals(AxHost.dataSource_Guid))
					{
						this.SetFlag(8, true);
					}
				}
			}

			// Token: 0x17000337 RID: 823
			// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x00033501 File Offset: 0x00032501
			public override Type ComponentType
			{
				get
				{
					return this.baseProp.ComponentType;
				}
			}

			// Token: 0x17000338 RID: 824
			// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0003350E File Offset: 0x0003250E
			public override TypeConverter Converter
			{
				get
				{
					if (this.dispid != null)
					{
						this.UpdateTypeConverterAndTypeEditorInternal(false, this.Dispid);
					}
					if (this.converter == null)
					{
						return base.Converter;
					}
					return this.converter;
				}
			}

			// Token: 0x17000339 RID: 825
			// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x0003353C File Offset: 0x0003253C
			internal int Dispid
			{
				get
				{
					DispIdAttribute dispIdAttribute = (DispIdAttribute)this.baseProp.Attributes[typeof(DispIdAttribute)];
					if (dispIdAttribute != null)
					{
						return dispIdAttribute.Value;
					}
					return -1;
				}
			}

			// Token: 0x1700033A RID: 826
			// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00033574 File Offset: 0x00032574
			public override bool IsReadOnly
			{
				get
				{
					return this.baseProp.IsReadOnly;
				}
			}

			// Token: 0x1700033B RID: 827
			// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x00033581 File Offset: 0x00032581
			public override Type PropertyType
			{
				get
				{
					return this.baseProp.PropertyType;
				}
			}

			// Token: 0x1700033C RID: 828
			// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x0003358E File Offset: 0x0003258E
			internal bool SettingValue
			{
				get
				{
					return this.GetFlag(16);
				}
			}

			// Token: 0x06001AD8 RID: 6872 RVA: 0x00033598 File Offset: 0x00032598
			private void AddAttribute(Attribute attr)
			{
				this.updateAttrs.Add(attr);
			}

			// Token: 0x06001AD9 RID: 6873 RVA: 0x000335A7 File Offset: 0x000325A7
			public override bool CanResetValue(object o)
			{
				return this.baseProp.CanResetValue(o);
			}

			// Token: 0x06001ADA RID: 6874 RVA: 0x000335B5 File Offset: 0x000325B5
			public override object GetEditor(Type editorBaseType)
			{
				this.UpdateTypeConverterAndTypeEditorInternal(false, this.dispid.Value);
				if (editorBaseType.Equals(typeof(UITypeEditor)) && this.editor != null)
				{
					return this.editor;
				}
				return base.GetEditor(editorBaseType);
			}

			// Token: 0x06001ADB RID: 6875 RVA: 0x000335F1 File Offset: 0x000325F1
			private bool GetFlag(int flagValue)
			{
				return (this.flags & flagValue) == flagValue;
			}

			// Token: 0x06001ADC RID: 6876 RVA: 0x00033600 File Offset: 0x00032600
			private Guid GetPropertyPage(int dispid)
			{
				try
				{
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					if (perPropertyBrowsing == null)
					{
						return Guid.Empty;
					}
					Guid guid;
					if (NativeMethods.Succeeded(perPropertyBrowsing.MapPropertyToPage(dispid, out guid)))
					{
						return guid;
					}
				}
				catch (COMException)
				{
				}
				catch (Exception)
				{
				}
				return Guid.Empty;
			}

			// Token: 0x06001ADD RID: 6877 RVA: 0x00033664 File Offset: 0x00032664
			public override object GetValue(object component)
			{
				if ((!this.GetFlag(8) && !this.owner.CanAccessProperties) || this.GetFlag(4))
				{
					return null;
				}
				object value;
				try
				{
					this.owner.NoComponentChangeEvents++;
					value = this.baseProp.GetValue(component);
				}
				catch (Exception ex)
				{
					if (!this.GetFlag(2))
					{
						this.SetFlag(2, true);
						this.AddAttribute(new BrowsableAttribute(false));
						this.owner.RefreshAllProperties = true;
						this.SetFlag(4, true);
					}
					throw ex;
				}
				finally
				{
					this.owner.NoComponentChangeEvents--;
				}
				return value;
			}

			// Token: 0x06001ADE RID: 6878 RVA: 0x0003371C File Offset: 0x0003271C
			public void OnValueChanged(object component)
			{
				this.OnValueChanged(component, EventArgs.Empty);
			}

			// Token: 0x06001ADF RID: 6879 RVA: 0x0003372A File Offset: 0x0003272A
			public override void ResetValue(object o)
			{
				this.baseProp.ResetValue(o);
			}

			// Token: 0x06001AE0 RID: 6880 RVA: 0x00033738 File Offset: 0x00032738
			private void SetFlag(int flagValue, bool value)
			{
				if (value)
				{
					this.flags |= flagValue;
					return;
				}
				this.flags &= ~flagValue;
			}

			// Token: 0x06001AE1 RID: 6881 RVA: 0x0003375C File Offset: 0x0003275C
			public override void SetValue(object component, object value)
			{
				if (!this.GetFlag(8) && !this.owner.CanAccessProperties)
				{
					return;
				}
				try
				{
					this.SetFlag(16, true);
					if (this.PropertyType.IsEnum && value.GetType() != this.PropertyType)
					{
						this.baseProp.SetValue(component, Enum.ToObject(this.PropertyType, value));
					}
					else
					{
						this.baseProp.SetValue(component, value);
					}
				}
				finally
				{
					this.SetFlag(16, false);
				}
				this.OnValueChanged(component);
				if (this.owner == component)
				{
					this.owner.SetAxState(AxHost.valueChanged, true);
				}
			}

			// Token: 0x06001AE2 RID: 6882 RVA: 0x00033808 File Offset: 0x00032808
			public override bool ShouldSerializeValue(object o)
			{
				return this.baseProp.ShouldSerializeValue(o);
			}

			// Token: 0x06001AE3 RID: 6883 RVA: 0x00033818 File Offset: 0x00032818
			internal void UpdateAttributes()
			{
				if (this.updateAttrs.Count == 0)
				{
					return;
				}
				ArrayList arrayList = new ArrayList(this.AttributeArray);
				foreach (object obj in this.updateAttrs)
				{
					Attribute attribute = (Attribute)obj;
					arrayList.Add(attribute);
				}
				Attribute[] array = new Attribute[arrayList.Count];
				arrayList.CopyTo(array, 0);
				this.AttributeArray = array;
				this.updateAttrs.Clear();
			}

			// Token: 0x06001AE4 RID: 6884 RVA: 0x000338B8 File Offset: 0x000328B8
			internal void UpdateTypeConverterAndTypeEditor(bool force)
			{
				if (this.GetFlag(1) && force)
				{
					this.SetFlag(1, false);
				}
			}

			// Token: 0x06001AE5 RID: 6885 RVA: 0x000338D0 File Offset: 0x000328D0
			internal void UpdateTypeConverterAndTypeEditorInternal(bool force, int dispid)
			{
				if (this.GetFlag(1) && !force)
				{
					return;
				}
				if (this.owner.GetOcx() == null)
				{
					return;
				}
				try
				{
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					if (perPropertyBrowsing != null)
					{
						NativeMethods.CA_STRUCT ca_STRUCT = new NativeMethods.CA_STRUCT();
						NativeMethods.CA_STRUCT ca_STRUCT2 = new NativeMethods.CA_STRUCT();
						int num = 0;
						try
						{
							num = perPropertyBrowsing.GetPredefinedStrings(dispid, ca_STRUCT, ca_STRUCT2);
						}
						catch (ExternalException ex)
						{
							num = ex.ErrorCode;
						}
						bool flag;
						if (num != 0)
						{
							flag = false;
							if (this.converter is Com2EnumConverter)
							{
								this.converter = null;
							}
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							OleStrCAMarshaler oleStrCAMarshaler = new OleStrCAMarshaler(ca_STRUCT);
							Int32CAMarshaler int32CAMarshaler = new Int32CAMarshaler(ca_STRUCT2);
							if (oleStrCAMarshaler.Count > 0 && int32CAMarshaler.Count > 0)
							{
								if (this.converter == null)
								{
									this.converter = new AxHost.AxEnumConverter(this, new AxHost.AxPerPropertyBrowsingEnum(this, this.owner, oleStrCAMarshaler, int32CAMarshaler, true));
								}
								else if (this.converter is AxHost.AxEnumConverter)
								{
									((AxHost.AxEnumConverter)this.converter).RefreshValues();
									AxHost.AxPerPropertyBrowsingEnum axPerPropertyBrowsingEnum = ((AxHost.AxEnumConverter)this.converter).com2Enum as AxHost.AxPerPropertyBrowsingEnum;
									if (axPerPropertyBrowsingEnum != null)
									{
										axPerPropertyBrowsingEnum.RefreshArrays(oleStrCAMarshaler, int32CAMarshaler);
									}
								}
							}
						}
						else if ((ComAliasNameAttribute)this.baseProp.Attributes[typeof(ComAliasNameAttribute)] == null)
						{
							Guid propertyPage = this.GetPropertyPage(dispid);
							if (!Guid.Empty.Equals(propertyPage))
							{
								this.editor = new AxHost.AxPropertyTypeEditor(this, propertyPage);
								if (!this.IsBrowsable)
								{
									this.AddAttribute(new BrowsableAttribute(true));
								}
							}
						}
					}
					this.SetFlag(1, true);
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x040012DD RID: 4829
			private const int FlagUpdatedEditorAndConverter = 1;

			// Token: 0x040012DE RID: 4830
			private const int FlagCheckGetter = 2;

			// Token: 0x040012DF RID: 4831
			private const int FlagGettterThrew = 4;

			// Token: 0x040012E0 RID: 4832
			private const int FlagIgnoreCanAccessProperties = 8;

			// Token: 0x040012E1 RID: 4833
			private const int FlagSettingValue = 16;

			// Token: 0x040012E2 RID: 4834
			private PropertyDescriptor baseProp;

			// Token: 0x040012E3 RID: 4835
			internal AxHost owner;

			// Token: 0x040012E4 RID: 4836
			private DispIdAttribute dispid;

			// Token: 0x040012E5 RID: 4837
			private TypeConverter converter;

			// Token: 0x040012E6 RID: 4838
			private UITypeEditor editor;

			// Token: 0x040012E7 RID: 4839
			private ArrayList updateAttrs = new ArrayList();

			// Token: 0x040012E8 RID: 4840
			private int flags;
		}

		// Token: 0x02000233 RID: 563
		private class AxPropertyTypeEditor : UITypeEditor
		{
			// Token: 0x06001AE6 RID: 6886 RVA: 0x00033A98 File Offset: 0x00032A98
			public AxPropertyTypeEditor(AxHost.AxPropertyDescriptor pd, Guid guid)
			{
				this.propDesc = pd;
				this.guid = guid;
			}

			// Token: 0x06001AE7 RID: 6887 RVA: 0x00033AB0 File Offset: 0x00032AB0
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				try
				{
					object instance = context.Instance;
					this.propDesc.owner.ShowPropertyPageForDispid(this.propDesc.Dispid, this.guid);
				}
				catch (Exception ex)
				{
					if (provider != null)
					{
						IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
						if (iuiservice != null)
						{
							iuiservice.ShowError(ex, SR.GetString("ErrorTypeConverterFailed"));
						}
					}
				}
				return value;
			}

			// Token: 0x06001AE8 RID: 6888 RVA: 0x00033B28 File Offset: 0x00032B28
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}

			// Token: 0x040012E9 RID: 4841
			private AxHost.AxPropertyDescriptor propDesc;

			// Token: 0x040012EA RID: 4842
			private Guid guid;
		}

		// Token: 0x02000235 RID: 565
		private class AxEnumConverter : Com2EnumConverter
		{
			// Token: 0x06001AF3 RID: 6899 RVA: 0x00033C69 File Offset: 0x00032C69
			public AxEnumConverter(AxHost.AxPropertyDescriptor target, Com2Enum com2Enum)
				: base(com2Enum)
			{
				this.target = target;
			}

			// Token: 0x06001AF4 RID: 6900 RVA: 0x00033C79 File Offset: 0x00032C79
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter converter = this.target.Converter;
				return base.GetStandardValues(context);
			}

			// Token: 0x040012ED RID: 4845
			private AxHost.AxPropertyDescriptor target;
		}

		// Token: 0x02000237 RID: 567
		private class AxPerPropertyBrowsingEnum : Com2Enum
		{
			// Token: 0x06001AFC RID: 6908 RVA: 0x00033E9C File Offset: 0x00032E9C
			public AxPerPropertyBrowsingEnum(AxHost.AxPropertyDescriptor targetObject, AxHost owner, OleStrCAMarshaler names, Int32CAMarshaler values, bool allowUnknowns)
				: base(new string[0], new object[0], allowUnknowns)
			{
				this.target = targetObject;
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.owner = owner;
				this.arraysFetched = false;
			}

			// Token: 0x17000340 RID: 832
			// (get) Token: 0x06001AFD RID: 6909 RVA: 0x00033ED6 File Offset: 0x00032ED6
			public override object[] Values
			{
				get
				{
					this.EnsureArrays();
					return base.Values;
				}
			}

			// Token: 0x17000341 RID: 833
			// (get) Token: 0x06001AFE RID: 6910 RVA: 0x00033EE4 File Offset: 0x00032EE4
			public override string[] Names
			{
				get
				{
					this.EnsureArrays();
					return base.Names;
				}
			}

			// Token: 0x06001AFF RID: 6911 RVA: 0x00033EF4 File Offset: 0x00032EF4
			private void EnsureArrays()
			{
				if (this.arraysFetched)
				{
					return;
				}
				this.arraysFetched = true;
				try
				{
					object[] items = this.nameMarshaller.Items;
					object[] items2 = this.valueMarshaller.Items;
					NativeMethods.IPerPropertyBrowsing perPropertyBrowsing = this.owner.GetPerPropertyBrowsing();
					int num = 0;
					if (items.Length > 0)
					{
						object[] array = new object[items2.Length];
						NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
						for (int i = 0; i < items.Length; i++)
						{
							int num2 = (int)items2[i];
							if (items[i] != null && items[i] is string)
							{
								variant.vt = 0;
								if (perPropertyBrowsing.GetPredefinedValue(this.target.Dispid, num2, variant) == 0 && variant.vt != 0)
								{
									array[i] = variant.ToObject();
								}
								variant.Clear();
								num++;
							}
						}
						if (num > 0)
						{
							string[] array2 = new string[num];
							Array.Copy(items, 0, array2, 0, num);
							base.PopulateArrays(array2, array);
						}
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06001B00 RID: 6912 RVA: 0x00033FF8 File Offset: 0x00032FF8
			internal void RefreshArrays(OleStrCAMarshaler names, Int32CAMarshaler values)
			{
				this.nameMarshaller = names;
				this.valueMarshaller = values;
				this.arraysFetched = false;
			}

			// Token: 0x06001B01 RID: 6913 RVA: 0x0003400F File Offset: 0x0003300F
			protected override void PopulateArrays(string[] names, object[] values)
			{
			}

			// Token: 0x06001B02 RID: 6914 RVA: 0x00034011 File Offset: 0x00033011
			public override object FromString(string s)
			{
				this.EnsureArrays();
				return base.FromString(s);
			}

			// Token: 0x06001B03 RID: 6915 RVA: 0x00034020 File Offset: 0x00033020
			public override string ToString(object v)
			{
				this.EnsureArrays();
				return base.ToString(v);
			}

			// Token: 0x040012F2 RID: 4850
			private AxHost.AxPropertyDescriptor target;

			// Token: 0x040012F3 RID: 4851
			private AxHost owner;

			// Token: 0x040012F4 RID: 4852
			private OleStrCAMarshaler nameMarshaller;

			// Token: 0x040012F5 RID: 4853
			private Int32CAMarshaler valueMarshaller;

			// Token: 0x040012F6 RID: 4854
			private bool arraysFetched;
		}
	}
}

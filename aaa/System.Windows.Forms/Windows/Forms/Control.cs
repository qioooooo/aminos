using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Internal;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Accessibility;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020001EA RID: 490
	[ComVisible(true)]
	[DefaultProperty("Text")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("Click")]
	[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class Control : Component, UnsafeNativeMethods.IOleControl, UnsafeNativeMethods.IOleObject, UnsafeNativeMethods.IOleInPlaceObject, UnsafeNativeMethods.IOleInPlaceActiveObject, UnsafeNativeMethods.IOleWindow, UnsafeNativeMethods.IViewObject, UnsafeNativeMethods.IViewObject2, UnsafeNativeMethods.IPersist, UnsafeNativeMethods.IPersistStreamInit, UnsafeNativeMethods.IPersistPropertyBag, UnsafeNativeMethods.IPersistStorage, UnsafeNativeMethods.IQuickActivate, ISupportOleDropSource, IDropTarget, ISynchronizeInvoke, IWin32Window, IArrangedElement, IBindableComponent, IComponent, IDisposable
	{
		// Token: 0x06001350 RID: 4944 RVA: 0x00014407 File Offset: 0x00013407
		public Control()
			: this(true)
		{
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00014410 File Offset: 0x00013410
		internal Control(bool autoInstallSyncContext)
		{
			this.propertyStore = new PropertyStore();
			this.window = new Control.ControlNativeWindow(this);
			this.RequiredScalingEnabled = true;
			this.RequiredScaling = BoundsSpecified.All;
			this.tabIndex = -1;
			this.state = 131086;
			this.state2 = 8;
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.Selectable | ControlStyles.StandardDoubleClick | ControlStyles.AllPaintingInWmPaint | ControlStyles.UseTextForAccessibility, true);
			this.InitMouseWheelSupport();
			if (this.DefaultMargin != CommonProperties.DefaultMargin)
			{
				this.Margin = this.DefaultMargin;
			}
			if (this.DefaultMinimumSize != CommonProperties.DefaultMinimumSize)
			{
				this.MinimumSize = this.DefaultMinimumSize;
			}
			if (this.DefaultMaximumSize != CommonProperties.DefaultMaximumSize)
			{
				this.MaximumSize = this.DefaultMaximumSize;
			}
			Size defaultSize = this.DefaultSize;
			this.width = defaultSize.Width;
			this.height = defaultSize.Height;
			CommonProperties.xClearPreferredSizeCache(this);
			if (this.width != 0 && this.height != 0)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				rect.left = (rect.right = (rect.top = (rect.bottom = 0)));
				CreateParams createParams = this.CreateParams;
				SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
				this.clientWidth = this.width - (rect.right - rect.left);
				this.clientHeight = this.height - (rect.bottom - rect.top);
			}
			if (autoInstallSyncContext)
			{
				WindowsFormsSynchronizationContext.InstallIfNeeded();
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00014599 File Offset: 0x00013599
		public Control(string text)
			: this(null, text)
		{
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x000145A3 File Offset: 0x000135A3
		public Control(string text, int left, int top, int width, int height)
			: this(null, text, left, top, width, height)
		{
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x000145B3 File Offset: 0x000135B3
		public Control(Control parent, string text)
			: this()
		{
			this.Parent = parent;
			this.Text = text;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000145C9 File Offset: 0x000135C9
		public Control(Control parent, string text, int left, int top, int width, int height)
			: this(parent, text)
		{
			this.Location = new Point(left, top);
			this.Size = new Size(width, height);
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x000145F0 File Offset: 0x000135F0
		[SRDescription("ControlAccessibilityObjectDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(Control.PropAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					if (!(accessibleObject is Control.ControlAccessibleObject))
					{
						return null;
					}
					this.Properties.SetObject(Control.PropAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0001463C File Offset: 0x0001363C
		private AccessibleObject NcAccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(Control.PropNcAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = new Control.ControlAccessibleObject(this, 0);
					this.Properties.SetObject(Control.PropNcAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0001467C File Offset: 0x0001367C
		private AccessibleObject GetAccessibilityObject(int accObjId)
		{
			AccessibleObject accessibleObject;
			if (accObjId != -4)
			{
				if (accObjId != 0)
				{
					if (accObjId > 0)
					{
						accessibleObject = this.GetAccessibilityObjectById(accObjId);
					}
					else
					{
						accessibleObject = null;
					}
				}
				else
				{
					accessibleObject = this.NcAccessibilityObject;
				}
			}
			else
			{
				accessibleObject = this.AccessibilityObject;
			}
			return accessibleObject;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000146B9 File Offset: 0x000136B9
		protected virtual AccessibleObject GetAccessibilityObjectById(int objectId)
		{
			return null;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x000146BC File Offset: 0x000136BC
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x000146D3 File Offset: 0x000136D3
		[SRDescription("ControlAccessibleDefaultActionDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatAccessibility")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string AccessibleDefaultActionDescription
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleDefaultActionDescription);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleDefaultActionDescription, value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x000146E6 File Offset: 0x000136E6
		// (set) Token: 0x0600135D RID: 4957 RVA: 0x000146FD File Offset: 0x000136FD
		[Localizable(true)]
		[SRDescription("ControlAccessibleDescriptionDescr")]
		[DefaultValue(null)]
		[SRCategory("CatAccessibility")]
		public string AccessibleDescription
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleDescription);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleDescription, value);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x00014710 File Offset: 0x00013710
		// (set) Token: 0x0600135F RID: 4959 RVA: 0x00014727 File Offset: 0x00013727
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("ControlAccessibleNameDescr")]
		[SRCategory("CatAccessibility")]
		public string AccessibleName
		{
			get
			{
				return (string)this.Properties.GetObject(Control.PropAccessibleName);
			}
			set
			{
				this.Properties.SetObject(Control.PropAccessibleName, value);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x0001473C File Offset: 0x0001373C
		// (set) Token: 0x06001361 RID: 4961 RVA: 0x00014762 File Offset: 0x00013762
		[SRDescription("ControlAccessibleRoleDescr")]
		[DefaultValue(AccessibleRole.Default)]
		[SRCategory("CatAccessibility")]
		public AccessibleRole AccessibleRole
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropAccessibleRole, out flag);
				if (flag)
				{
					return (AccessibleRole)integer;
				}
				return AccessibleRole.Default;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 64))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AccessibleRole));
				}
				this.Properties.SetInteger(Control.PropAccessibleRole, (int)value);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x0001479C File Offset: 0x0001379C
		private Color ActiveXAmbientBackColor
		{
			get
			{
				return this.ActiveXInstance.AmbientBackColor;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x000147A9 File Offset: 0x000137A9
		private Color ActiveXAmbientForeColor
		{
			get
			{
				return this.ActiveXInstance.AmbientForeColor;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x000147B6 File Offset: 0x000137B6
		private Font ActiveXAmbientFont
		{
			get
			{
				return this.ActiveXInstance.AmbientFont;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x000147C3 File Offset: 0x000137C3
		private bool ActiveXEventsFrozen
		{
			get
			{
				return this.ActiveXInstance.EventsFrozen;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x000147D0 File Offset: 0x000137D0
		private IntPtr ActiveXHWNDParent
		{
			get
			{
				return this.ActiveXInstance.HWNDParent;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x000147E0 File Offset: 0x000137E0
		private Control.ActiveXImpl ActiveXInstance
		{
			get
			{
				Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)this.Properties.GetObject(Control.PropActiveXImpl);
				if (activeXImpl == null)
				{
					if (this.GetState(524288))
					{
						throw new NotSupportedException(SR.GetString("AXTopLevelSource"));
					}
					activeXImpl = new Control.ActiveXImpl(this);
					this.SetState2(1024, true);
					this.Properties.SetObject(Control.PropActiveXImpl, activeXImpl);
				}
				return activeXImpl;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00014848 File Offset: 0x00013848
		// (set) Token: 0x06001369 RID: 4969 RVA: 0x00014854 File Offset: 0x00013854
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ControlAllowDropDescr")]
		public virtual bool AllowDrop
		{
			get
			{
				return this.GetState(64);
			}
			set
			{
				if (this.GetState(64) != value)
				{
					if (value && !this.IsHandleCreated)
					{
						IntSecurity.ClipboardRead.Demand();
					}
					this.SetState(64, value);
					if (this.IsHandleCreated)
					{
						try
						{
							this.SetAcceptDrops(value);
						}
						catch
						{
							this.SetState(64, !value);
							throw;
						}
					}
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x000148BC File Offset: 0x000138BC
		private AmbientProperties AmbientPropertiesService
		{
			get
			{
				bool flag;
				AmbientProperties ambientProperties = (AmbientProperties)this.Properties.GetObject(Control.PropAmbientPropertiesService, out flag);
				if (!flag)
				{
					if (this.Site != null)
					{
						ambientProperties = (AmbientProperties)this.Site.GetService(typeof(AmbientProperties));
					}
					else
					{
						ambientProperties = (AmbientProperties)this.GetService(typeof(AmbientProperties));
					}
					if (ambientProperties != null)
					{
						this.Properties.SetObject(Control.PropAmbientPropertiesService, ambientProperties);
					}
				}
				return ambientProperties;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00014934 File Offset: 0x00013934
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0001493C File Offset: 0x0001393C
		[SRDescription("ControlAnchorDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
		[SRCategory("CatLayout")]
		public virtual AnchorStyles Anchor
		{
			get
			{
				return DefaultLayout.GetAnchor(this);
			}
			set
			{
				DefaultLayout.SetAnchor(this.ParentInternal, this, value);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0001494B File Offset: 0x0001394B
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00014954 File Offset: 0x00013954
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[SRDescription("ControlAutoSizeDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public virtual bool AutoSize
		{
			get
			{
				return CommonProperties.GetAutoSize(this);
			}
			set
			{
				if (value != this.AutoSize)
				{
					CommonProperties.SetAutoSize(this, value);
					if (this.ParentInternal != null)
					{
						if (value && this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600136F RID: 4975 RVA: 0x000149BD File Offset: 0x000139BD
		// (remove) Token: 0x06001370 RID: 4976 RVA: 0x000149D0 File Offset: 0x000139D0
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		public event EventHandler AutoSizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventAutoSizeChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventAutoSizeChanged, value);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x000149E3 File Offset: 0x000139E3
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x00014A12 File Offset: 0x00013A12
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DefaultValue(typeof(Point), "0, 0")]
		public virtual Point AutoScrollOffset
		{
			get
			{
				if (this.Properties.ContainsObject(Control.PropAutoScrollOffset))
				{
					return (Point)this.Properties.GetObject(Control.PropAutoScrollOffset);
				}
				return Point.Empty;
			}
			set
			{
				if (this.AutoScrollOffset != value)
				{
					this.Properties.SetObject(Control.PropAutoScrollOffset, value);
				}
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00014A38 File Offset: 0x00013A38
		protected void SetAutoSizeMode(AutoSizeMode mode)
		{
			CommonProperties.SetAutoSizeMode(this, mode);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00014A41 File Offset: 0x00013A41
		protected AutoSizeMode GetAutoSizeMode()
		{
			return CommonProperties.GetAutoSizeMode(this);
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00014A49 File Offset: 0x00013A49
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public virtual LayoutEngine LayoutEngine
		{
			get
			{
				return DefaultLayout.Instance;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00014A50 File Offset: 0x00013A50
		internal IntPtr BackColorBrush
		{
			get
			{
				object @object = this.Properties.GetObject(Control.PropBackBrush);
				if (@object != null)
				{
					return (IntPtr)@object;
				}
				if (!this.Properties.ContainsObject(Control.PropBackColor) && this.parent != null && this.parent.BackColor == this.BackColor)
				{
					return this.parent.BackColorBrush;
				}
				Color backColor = this.BackColor;
				IntPtr intPtr;
				if (ColorTranslator.ToOle(backColor) < 0)
				{
					intPtr = SafeNativeMethods.GetSysColorBrush(ColorTranslator.ToOle(backColor) & 255);
					this.SetState(2097152, false);
				}
				else
				{
					intPtr = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
					this.SetState(2097152, true);
				}
				this.Properties.SetObject(Control.PropBackBrush, intPtr);
				return intPtr;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00014B18 File Offset: 0x00013B18
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00014BA0 File Offset: 0x00013BA0
		[SRDescription("ControlBackColorDescr")]
		[DispId(-501)]
		[SRCategory("CatAppearance")]
		public virtual Color BackColor
		{
			get
			{
				Color color = this.RawBackColor;
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					color = parentInternal.BackColor;
					if (this.IsValidBackColor(color))
					{
						return color;
					}
				}
				if (this.IsActiveX)
				{
					color = this.ActiveXAmbientBackColor;
				}
				if (color.IsEmpty)
				{
					AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
					if (ambientPropertiesService != null)
					{
						color = ambientPropertiesService.BackColor;
					}
				}
				if (!color.IsEmpty && this.IsValidBackColor(color))
				{
					return color;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				if (!value.Equals(Color.Empty) && !this.GetStyle(ControlStyles.SupportsTransparentBackColor) && value.A < 255)
				{
					throw new ArgumentException(SR.GetString("TransparentBackColorNotAllowed"));
				}
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(Control.PropBackColor))
				{
					this.Properties.SetColor(Control.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001379 RID: 4985 RVA: 0x00014C49 File Offset: 0x00013C49
		// (remove) Token: 0x0600137A RID: 4986 RVA: 0x00014C5C File Offset: 0x00013C5C
		[SRDescription("ControlOnBackColorChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackColor, value);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x00014C6F File Offset: 0x00013C6F
		// (set) Token: 0x0600137C RID: 4988 RVA: 0x00014C86 File Offset: 0x00013C86
		[SRDescription("ControlBackgroundImageDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[Localizable(true)]
		public virtual Image BackgroundImage
		{
			get
			{
				return (Image)this.Properties.GetObject(Control.PropBackgroundImage);
			}
			set
			{
				if (this.BackgroundImage != value)
				{
					this.Properties.SetObject(Control.PropBackgroundImage, value);
					this.OnBackgroundImageChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600137D RID: 4989 RVA: 0x00014CAD File Offset: 0x00013CAD
		// (remove) Token: 0x0600137E RID: 4990 RVA: 0x00014CC0 File Offset: 0x00013CC0
		[SRDescription("ControlOnBackgroundImageChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler BackgroundImageChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackgroundImage, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackgroundImage, value);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00014CD4 File Offset: 0x00013CD4
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x00014D0C File Offset: 0x00013D0C
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(ImageLayout.Tile)]
		public virtual ImageLayout BackgroundImageLayout
		{
			get
			{
				if (!this.Properties.ContainsObject(Control.PropBackgroundImageLayout))
				{
					return ImageLayout.Tile;
				}
				return (ImageLayout)this.Properties.GetObject(Control.PropBackgroundImageLayout);
			}
			set
			{
				if (this.BackgroundImageLayout != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ImageLayout));
					}
					if (value == ImageLayout.Center || value == ImageLayout.Zoom || value == ImageLayout.Stretch)
					{
						this.SetStyle(ControlStyles.ResizeRedraw, true);
						if (ControlPaint.IsImageTransparent(this.BackgroundImage))
						{
							this.DoubleBuffered = true;
						}
					}
					this.Properties.SetObject(Control.PropBackgroundImageLayout, value);
					this.OnBackgroundImageLayoutChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001381 RID: 4993 RVA: 0x00014D92 File Offset: 0x00013D92
		// (remove) Token: 0x06001382 RID: 4994 RVA: 0x00014DA5 File Offset: 0x00013DA5
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBackgroundImageLayoutChangedDescr")]
		public event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBackgroundImageLayout, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBackgroundImageLayout, value);
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00014DB8 File Offset: 0x00013DB8
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x00014DC2 File Offset: 0x00013DC2
		internal bool BecomingActiveControl
		{
			get
			{
				return this.GetState2(32);
			}
			set
			{
				if (value != this.BecomingActiveControl)
				{
					Application.ThreadContext.FromCurrent().ActivatingControl = (value ? this : null);
					this.SetState2(32, value);
				}
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00014DE8 File Offset: 0x00013DE8
		private bool ShouldSerializeAccessibleName()
		{
			string accessibleName = this.AccessibleName;
			return accessibleName != null && accessibleName.Length > 0;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00014E0C File Offset: 0x00013E0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBindings()
		{
			ControlBindingsCollection controlBindingsCollection = (ControlBindingsCollection)this.Properties.GetObject(Control.PropBindings);
			if (controlBindingsCollection != null)
			{
				controlBindingsCollection.Clear();
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00014E38 File Offset: 0x00013E38
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x00014E7C File Offset: 0x00013E7C
		internal BindingContext BindingContextInternal
		{
			get
			{
				BindingContext bindingContext = (BindingContext)this.Properties.GetObject(Control.PropBindingManager);
				if (bindingContext != null)
				{
					return bindingContext;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.BindingContext;
				}
				return null;
			}
			set
			{
				BindingContext bindingContext = (BindingContext)this.Properties.GetObject(Control.PropBindingManager);
				if (bindingContext != value)
				{
					this.Properties.SetObject(Control.PropBindingManager, value);
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00014EC1 File Offset: 0x00013EC1
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x00014EC9 File Offset: 0x00013EC9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlBindingContextDescr")]
		[Browsable(false)]
		public virtual BindingContext BindingContext
		{
			get
			{
				return this.BindingContextInternal;
			}
			set
			{
				this.BindingContextInternal = value;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600138B RID: 5003 RVA: 0x00014ED2 File Offset: 0x00013ED2
		// (remove) Token: 0x0600138C RID: 5004 RVA: 0x00014EE5 File Offset: 0x00013EE5
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnBindingContextChangedDescr")]
		public event EventHandler BindingContextChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventBindingContext, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventBindingContext, value);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00014EF8 File Offset: 0x00013EF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlBottomDescr")]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int Bottom
		{
			get
			{
				return this.y + this.height;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x00014F07 File Offset: 0x00013F07
		// (set) Token: 0x0600138F RID: 5007 RVA: 0x00014F26 File Offset: 0x00013F26
		[SRDescription("ControlBoundsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(this.x, this.y, this.width, this.height);
			}
			set
			{
				this.SetBounds(value.X, value.Y, value.Width, value.Height, BoundsSpecified.All);
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x00014F4C File Offset: 0x00013F4C
		internal virtual bool CanAccessProperties
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00014F50 File Offset: 0x00013F50
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlCanFocusDescr")]
		[Browsable(false)]
		[SRCategory("CatFocus")]
		public bool CanFocus
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return false;
				}
				bool flag = SafeNativeMethods.IsWindowVisible(new HandleRef(this.window, this.Handle));
				bool flag2 = SafeNativeMethods.IsWindowEnabled(new HandleRef(this.window, this.Handle));
				return flag && flag2;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00014F9B File Offset: 0x00013F9B
		protected override bool CanRaiseEvents
		{
			get
			{
				return !this.IsActiveX || !this.ActiveXEventsFrozen;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x00014FB0 File Offset: 0x00013FB0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatFocus")]
		[SRDescription("ControlCanSelectDescr")]
		public bool CanSelect
		{
			get
			{
				return this.CanSelectCore();
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00014FB8 File Offset: 0x00013FB8
		// (set) Token: 0x06001395 RID: 5013 RVA: 0x00014FC0 File Offset: 0x00013FC0
		[SRCategory("CatFocus")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ControlCaptureDescr")]
		public bool Capture
		{
			get
			{
				return this.CaptureInternal;
			}
			set
			{
				if (value)
				{
					IntSecurity.GetCapture.Demand();
				}
				this.CaptureInternal = value;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x00014FD6 File Offset: 0x00013FD6
		// (set) Token: 0x06001397 RID: 5015 RVA: 0x00014FF2 File Offset: 0x00013FF2
		internal bool CaptureInternal
		{
			get
			{
				return this.IsHandleCreated && UnsafeNativeMethods.GetCapture() == this.Handle;
			}
			set
			{
				if (this.CaptureInternal != value)
				{
					if (value)
					{
						UnsafeNativeMethods.SetCapture(new HandleRef(this, this.Handle));
						return;
					}
					SafeNativeMethods.ReleaseCapture();
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00015019 File Offset: 0x00014019
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x00015026 File Offset: 0x00014026
		[DefaultValue(true)]
		[SRDescription("ControlCausesValidationDescr")]
		[SRCategory("CatFocus")]
		public bool CausesValidation
		{
			get
			{
				return this.GetState(131072);
			}
			set
			{
				if (value != this.CausesValidation)
				{
					this.SetState(131072, value);
					this.OnCausesValidationChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600139A RID: 5018 RVA: 0x00015048 File Offset: 0x00014048
		// (remove) Token: 0x0600139B RID: 5019 RVA: 0x0001505B File Offset: 0x0001405B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnCausesValidationChangedDescr")]
		public event EventHandler CausesValidationChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventCausesValidation, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventCausesValidation, value);
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x00015070 File Offset: 0x00014070
		// (set) Token: 0x0600139D RID: 5021 RVA: 0x000150A4 File Offset: 0x000140A4
		internal bool CacheTextInternal
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropCacheTextCount, out flag);
				return integer > 0 || this.GetStyle(ControlStyles.CacheText);
			}
			set
			{
				if (this.GetStyle(ControlStyles.CacheText) || !this.IsHandleCreated)
				{
					return;
				}
				bool flag;
				int num = this.Properties.GetInteger(Control.PropCacheTextCount, out flag);
				if (value)
				{
					if (num == 0)
					{
						this.Properties.SetObject(Control.PropCacheTextField, this.text);
						if (this.text == null)
						{
							this.text = this.WindowText;
						}
					}
					num++;
				}
				else
				{
					num--;
					if (num == 0)
					{
						this.text = (string)this.Properties.GetObject(Control.PropCacheTextField, out flag);
					}
				}
				this.Properties.SetInteger(Control.PropCacheTextCount, num);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00015145 File Offset: 0x00014145
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x0001514C File Offset: 0x0001414C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlCheckForIllegalCrossThreadCalls")]
		public static bool CheckForIllegalCrossThreadCalls
		{
			get
			{
				return Control.checkForIllegalCrossThreadCalls;
			}
			set
			{
				Control.checkForIllegalCrossThreadCalls = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00015154 File Offset: 0x00014154
		[SRCategory("CatLayout")]
		[SRDescription("ControlClientRectangleDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle ClientRectangle
		{
			get
			{
				return new Rectangle(0, 0, this.clientWidth, this.clientHeight);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00015169 File Offset: 0x00014169
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0001517C File Offset: 0x0001417C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[SRDescription("ControlClientSizeDescr")]
		public Size ClientSize
		{
			get
			{
				return new Size(this.clientWidth, this.clientHeight);
			}
			set
			{
				this.SetClientSizeCore(value.Width, value.Height);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060013A3 RID: 5027 RVA: 0x00015192 File Offset: 0x00014192
		// (remove) Token: 0x060013A4 RID: 5028 RVA: 0x000151A5 File Offset: 0x000141A5
		[SRDescription("ControlOnClientSizeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ClientSizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventClientSize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventClientSize, value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x000151B8 File Offset: 0x000141B8
		[Browsable(false)]
		[Description("ControlCompanyNameDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CompanyName
		{
			get
			{
				return this.VersionInfo.CompanyName;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x000151C8 File Offset: 0x000141C8
		[SRDescription("ControlContainsFocusDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool ContainsFocus
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return false;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return !(focus == IntPtr.Zero) && (focus == this.Handle || UnsafeNativeMethods.IsChild(new HandleRef(this, this.Handle), new HandleRef(this, focus)));
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00015221 File Offset: 0x00014221
		// (set) Token: 0x060013A8 RID: 5032 RVA: 0x00015238 File Offset: 0x00014238
		[DefaultValue(null)]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ControlContextMenuDescr")]
		public virtual ContextMenu ContextMenu
		{
			get
			{
				return (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			}
			set
			{
				ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
				if (contextMenu != value)
				{
					EventHandler eventHandler = new EventHandler(this.DetachContextMenu);
					if (contextMenu != null)
					{
						contextMenu.Disposed -= eventHandler;
					}
					this.Properties.SetObject(Control.PropContextMenu, value);
					if (value != null)
					{
						value.Disposed += eventHandler;
					}
					this.OnContextMenuChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060013A9 RID: 5033 RVA: 0x0001529C File Offset: 0x0001429C
		// (remove) Token: 0x060013AA RID: 5034 RVA: 0x000152AF File Offset: 0x000142AF
		[SRCategory("CatPropertyChanged")]
		[Browsable(false)]
		[SRDescription("ControlOnContextMenuChangedDescr")]
		public event EventHandler ContextMenuChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventContextMenu, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventContextMenu, value);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x000152C2 File Offset: 0x000142C2
		// (set) Token: 0x060013AC RID: 5036 RVA: 0x000152DC File Offset: 0x000142DC
		[SRDescription("ControlContextMenuDescr")]
		[DefaultValue(null)]
		[SRCategory("CatBehavior")]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(Control.PropContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = this.Properties.GetObject(Control.PropContextMenuStrip) as ContextMenuStrip;
				if (contextMenuStrip != value)
				{
					EventHandler eventHandler = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= eventHandler;
					}
					this.Properties.SetObject(Control.PropContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += eventHandler;
					}
					this.OnContextMenuStripChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060013AD RID: 5037 RVA: 0x00015340 File Offset: 0x00014340
		// (remove) Token: 0x060013AE RID: 5038 RVA: 0x00015353 File Offset: 0x00014353
		[SRDescription("ControlContextMenuStripChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventContextMenuStrip, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventContextMenuStrip, value);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00015368 File Offset: 0x00014368
		[Browsable(false)]
		[SRDescription("ControlControlsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Control.ControlCollection Controls
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection == null)
				{
					controlCollection = this.CreateControlsInstance();
					this.Properties.SetObject(Control.PropControlsCollection, controlCollection);
				}
				return controlCollection;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x000153A7 File Offset: 0x000143A7
		[SRDescription("ControlCreatedDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Created
		{
			get
			{
				return (this.state & 1) != 0;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x000153B8 File Offset: 0x000143B8
		protected virtual CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.createParams == null)
				{
					this.createParams = new CreateParams();
				}
				CreateParams createParams = this.createParams;
				createParams.Style = 0;
				createParams.ExStyle = 0;
				createParams.ClassStyle = 0;
				createParams.Caption = this.text;
				createParams.X = this.x;
				createParams.Y = this.y;
				createParams.Width = this.width;
				createParams.Height = this.height;
				createParams.Style = 33554432;
				if (this.GetStyle(ControlStyles.ContainerControl))
				{
					createParams.ExStyle |= 65536;
				}
				createParams.ClassStyle = 8;
				if ((this.state & 524288) == 0)
				{
					createParams.Parent = ((this.parent == null) ? IntPtr.Zero : this.parent.InternalHandle);
					createParams.Style |= 1140850688;
				}
				else
				{
					createParams.Parent = IntPtr.Zero;
				}
				if ((this.state & 8) != 0)
				{
					createParams.Style |= 65536;
				}
				if ((this.state & 2) != 0)
				{
					createParams.Style |= 268435456;
				}
				if (!this.Enabled)
				{
					createParams.Style |= 134217728;
				}
				if (createParams.Parent == IntPtr.Zero && this.IsActiveX)
				{
					createParams.Parent = this.ActiveXHWNDParent;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					createParams.ExStyle |= 8192;
					createParams.ExStyle |= 4096;
					createParams.ExStyle |= 16384;
				}
				return createParams;
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00015562 File Offset: 0x00014562
		internal virtual void NotifyValidationResult(object sender, CancelEventArgs ev)
		{
			this.ValidationCancelled = ev.Cancel;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00015570 File Offset: 0x00014570
		internal bool ValidateActiveControl(out bool validatedControlAllowsFocusChange)
		{
			bool flag = true;
			validatedControlAllowsFocusChange = false;
			IContainerControl containerControlInternal = this.GetContainerControlInternal();
			if (containerControlInternal != null)
			{
				ContainerControl containerControl = containerControlInternal as ContainerControl;
				if (containerControl != null)
				{
					while (containerControl.ActiveControl == null)
					{
						Control parentInternal = containerControl.ParentInternal;
						if (parentInternal == null)
						{
							break;
						}
						ContainerControl containerControl2 = parentInternal.GetContainerControlInternal() as ContainerControl;
						if (containerControl2 == null)
						{
							break;
						}
						containerControl = containerControl2;
					}
					flag = containerControl.ValidateInternal(true, out validatedControlAllowsFocusChange);
				}
			}
			return flag;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x000155D8 File Offset: 0x000145D8
		// (set) Token: 0x060013B4 RID: 5044 RVA: 0x000155C8 File Offset: 0x000145C8
		internal bool ValidationCancelled
		{
			get
			{
				if (this.GetState(268435456))
				{
					return true;
				}
				Control parentInternal = this.ParentInternal;
				return parentInternal != null && parentInternal.ValidationCancelled;
			}
			set
			{
				this.SetState(268435456, value);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00015608 File Offset: 0x00014608
		internal int CreateThreadId
		{
			get
			{
				if (this.IsHandleCreated)
				{
					int num;
					return SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, this.Handle), out num);
				}
				return SafeNativeMethods.GetCurrentThreadId();
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00015638 File Offset: 0x00014638
		// (set) Token: 0x060013B8 RID: 5048 RVA: 0x000156C0 File Offset: 0x000146C0
		[SRDescription("ControlCursorDescr")]
		[SRCategory("CatAppearance")]
		[AmbientValue(null)]
		public virtual Cursor Cursor
		{
			get
			{
				if (this.GetState(1024))
				{
					return Cursors.WaitCursor;
				}
				Cursor cursor = (Cursor)this.Properties.GetObject(Control.PropCursor);
				if (cursor != null)
				{
					return cursor;
				}
				Cursor defaultCursor = this.DefaultCursor;
				if (defaultCursor != Cursors.Default)
				{
					return defaultCursor;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.Cursor;
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Cursor != null)
				{
					return ambientPropertiesService.Cursor;
				}
				return defaultCursor;
			}
			set
			{
				Cursor cursor = (Cursor)this.Properties.GetObject(Control.PropCursor);
				Cursor cursor2 = this.Cursor;
				if (cursor != value)
				{
					IntSecurity.ModifyCursor.Demand();
					this.Properties.SetObject(Control.PropCursor, value);
				}
				if (this.IsHandleCreated)
				{
					NativeMethods.POINT point = new NativeMethods.POINT();
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetCursorPos(point);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
					if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == this.Handle)
					{
						this.SendMessage(32, this.Handle, (IntPtr)1);
					}
				}
				if (!cursor2.Equals(value))
				{
					this.OnCursorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060013B9 RID: 5049 RVA: 0x000157B9 File Offset: 0x000147B9
		// (remove) Token: 0x060013BA RID: 5050 RVA: 0x000157CC File Offset: 0x000147CC
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnCursorChangedDescr")]
		public event EventHandler CursorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventCursor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventCursor, value);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x000157E0 File Offset: 0x000147E0
		[ParenthesizePropertyName(true)]
		[SRCategory("CatData")]
		[RefreshProperties(RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ControlBindingsDescr")]
		public ControlBindingsCollection DataBindings
		{
			get
			{
				ControlBindingsCollection controlBindingsCollection = (ControlBindingsCollection)this.Properties.GetObject(Control.PropBindings);
				if (controlBindingsCollection == null)
				{
					controlBindingsCollection = new ControlBindingsCollection(this);
					this.Properties.SetObject(Control.PropBindings, controlBindingsCollection);
				}
				return controlBindingsCollection;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0001581F File Offset: 0x0001481F
		public static Color DefaultBackColor
		{
			get
			{
				return SystemColors.Control;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x00015826 File Offset: 0x00014826
		protected virtual Cursor DefaultCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0001582D File Offset: 0x0001482D
		public static Font DefaultFont
		{
			get
			{
				if (Control.defaultFont == null)
				{
					Control.defaultFont = SystemFonts.DefaultFont;
				}
				return Control.defaultFont;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x00015845 File Offset: 0x00014845
		public static Color DefaultForeColor
		{
			get
			{
				return SystemColors.ControlText;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0001584C File Offset: 0x0001484C
		protected virtual Padding DefaultMargin
		{
			get
			{
				return CommonProperties.DefaultMargin;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00015853 File Offset: 0x00014853
		protected virtual Size DefaultMaximumSize
		{
			get
			{
				return CommonProperties.DefaultMaximumSize;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0001585A File Offset: 0x0001485A
		protected virtual Size DefaultMinimumSize
		{
			get
			{
				return CommonProperties.DefaultMinimumSize;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00015861 File Offset: 0x00014861
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00015868 File Offset: 0x00014868
		private RightToLeft DefaultRightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0001586B File Offset: 0x0001486B
		protected virtual Size DefaultSize
		{
			get
			{
				return Size.Empty;
			}
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00015872 File Offset: 0x00014872
		private void DetachContextMenu(object sender, EventArgs e)
		{
			this.ContextMenu = null;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0001587B File Offset: 0x0001487B
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStrip = null;
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00015884 File Offset: 0x00014884
		internal Color DisabledColor
		{
			get
			{
				Color color = this.BackColor;
				if (color.A == 0)
				{
					Control control = this.ParentInternal;
					while (color.A == 0)
					{
						if (control == null)
						{
							color = SystemColors.Control;
							break;
						}
						color = control.BackColor;
						control = control.ParentInternal;
					}
				}
				return color;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x000158CD File Offset: 0x000148CD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ControlDisplayRectangleDescr")]
		public virtual Rectangle DisplayRectangle
		{
			get
			{
				return new Rectangle(0, 0, this.clientWidth, this.clientHeight);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000158E2 File Offset: 0x000148E2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlDisposedDescr")]
		public bool IsDisposed
		{
			get
			{
				return this.GetState(2048);
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000158F0 File Offset: 0x000148F0
		private void DisposeFontHandle()
		{
			if (this.Properties.ContainsObject(Control.PropFontHandleWrapper))
			{
				Control.FontHandleWrapper fontHandleWrapper = this.Properties.GetObject(Control.PropFontHandleWrapper) as Control.FontHandleWrapper;
				if (fontHandleWrapper != null)
				{
					fontHandleWrapper.Dispose();
				}
				this.Properties.SetObject(Control.PropFontHandleWrapper, null);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x0001593F File Offset: 0x0001493F
		[SRDescription("ControlDisposingDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool Disposing
		{
			get
			{
				return this.GetState(4096);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x0001594C File Offset: 0x0001494C
		// (set) Token: 0x060013CE RID: 5070 RVA: 0x00015954 File Offset: 0x00014954
		[DefaultValue(DockStyle.None)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ControlDockDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		public virtual DockStyle Dock
		{
			get
			{
				return DefaultLayout.GetDock(this);
			}
			set
			{
				if (value != this.Dock)
				{
					this.SuspendLayout();
					try
					{
						DefaultLayout.SetDock(this, value);
						this.OnDockChanged(EventArgs.Empty);
					}
					finally
					{
						this.ResumeLayout();
					}
				}
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060013CF RID: 5071 RVA: 0x0001599C File Offset: 0x0001499C
		// (remove) Token: 0x060013D0 RID: 5072 RVA: 0x000159AF File Offset: 0x000149AF
		[SRDescription("ControlOnDockChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler DockChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventDock, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDock, value);
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x000159C2 File Offset: 0x000149C2
		// (set) Token: 0x060013D2 RID: 5074 RVA: 0x000159CF File Offset: 0x000149CF
		[SRCategory("CatBehavior")]
		[SRDescription("ControlDoubleBufferedDescr")]
		protected virtual bool DoubleBuffered
		{
			get
			{
				return this.GetStyle(ControlStyles.OptimizedDoubleBuffer);
			}
			set
			{
				if (value != this.DoubleBuffered)
				{
					if (value)
					{
						this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value);
						return;
					}
					this.SetStyle(ControlStyles.OptimizedDoubleBuffer, value);
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x000159F6 File Offset: 0x000149F6
		private bool DoubleBufferingEnabled
		{
			get
			{
				return this.GetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x00015A03 File Offset: 0x00014A03
		// (set) Token: 0x060013D5 RID: 5077 RVA: 0x00015A28 File Offset: 0x00014A28
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DispId(-514)]
		[SRDescription("ControlEnabledDescr")]
		public bool Enabled
		{
			get
			{
				return this.GetState(4) && (this.ParentInternal == null || this.ParentInternal.Enabled);
			}
			set
			{
				bool enabled = this.Enabled;
				this.SetState(4, value);
				if (enabled != value)
				{
					if (!value)
					{
						this.SelectNextIfFocused();
					}
					this.OnEnabledChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060013D6 RID: 5078 RVA: 0x00015A5C File Offset: 0x00014A5C
		// (remove) Token: 0x060013D7 RID: 5079 RVA: 0x00015A6F File Offset: 0x00014A6F
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnEnabledChangedDescr")]
		public event EventHandler EnabledChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventEnabled, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventEnabled, value);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x00015A82 File Offset: 0x00014A82
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ControlFocusedDescr")]
		public virtual bool Focused
		{
			get
			{
				return this.IsHandleCreated && UnsafeNativeMethods.GetFocus() == this.Handle;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x00015AA0 File Offset: 0x00014AA0
		// (set) Token: 0x060013DA RID: 5082 RVA: 0x00015B08 File Offset: 0x00014B08
		[SRDescription("ControlFontDescr")]
		[AmbientValue(null)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DispId(-512)]
		public virtual Font Font
		{
			[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Windows.Forms.Control/ActiveXFontMarshaler)]
			get
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					return font;
				}
				Font font2 = this.GetParentFont();
				if (font2 != null)
				{
					return font2;
				}
				if (this.IsActiveX)
				{
					font2 = this.ActiveXAmbientFont;
					if (font2 != null)
					{
						return font2;
					}
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Font != null)
				{
					return ambientPropertiesService.Font;
				}
				return Control.DefaultFont;
			}
			[param: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Windows.Forms.Control/ActiveXFontMarshaler)]
			set
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				Font font2 = this.Font;
				bool flag = false;
				if (value == null)
				{
					if (font != null)
					{
						flag = true;
					}
				}
				else
				{
					flag = font == null || !value.Equals(font);
				}
				if (flag)
				{
					this.Properties.SetObject(Control.PropFont, value);
					if (!font2.Equals(value))
					{
						this.DisposeFontHandle();
						if (this.Properties.ContainsInteger(Control.PropFontHeight))
						{
							this.Properties.SetInteger(Control.PropFontHeight, (value == null) ? (-1) : value.Height);
						}
						using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Font))
						{
							this.OnFontChanged(EventArgs.Empty);
							return;
						}
					}
					if (this.IsHandleCreated && !this.GetStyle(ControlStyles.UserPaint))
					{
						this.DisposeFontHandle();
						this.SetWindowFont();
					}
				}
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060013DB RID: 5083 RVA: 0x00015BFC File Offset: 0x00014BFC
		// (remove) Token: 0x060013DC RID: 5084 RVA: 0x00015C0F File Offset: 0x00014C0F
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnFontChangedDescr")]
		public event EventHandler FontChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventFont, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventFont, value);
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x00015C24 File Offset: 0x00014C24
		internal IntPtr FontHandle
		{
			get
			{
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					Control.FontHandleWrapper fontHandleWrapper = (Control.FontHandleWrapper)this.Properties.GetObject(Control.PropFontHandleWrapper);
					if (fontHandleWrapper == null)
					{
						fontHandleWrapper = new Control.FontHandleWrapper(font);
						this.Properties.SetObject(Control.PropFontHandleWrapper, fontHandleWrapper);
					}
					return fontHandleWrapper.Handle;
				}
				if (this.parent != null)
				{
					return this.parent.FontHandle;
				}
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				if (ambientPropertiesService != null && ambientPropertiesService.Font != null)
				{
					Control.FontHandleWrapper fontHandleWrapper2 = null;
					Font font2 = (Font)this.Properties.GetObject(Control.PropCurrentAmbientFont);
					if (font2 != null && font2 == ambientPropertiesService.Font)
					{
						fontHandleWrapper2 = (Control.FontHandleWrapper)this.Properties.GetObject(Control.PropFontHandleWrapper);
					}
					else
					{
						this.Properties.SetObject(Control.PropCurrentAmbientFont, ambientPropertiesService.Font);
					}
					if (fontHandleWrapper2 == null)
					{
						font = ambientPropertiesService.Font;
						fontHandleWrapper2 = new Control.FontHandleWrapper(font);
						this.Properties.SetObject(Control.PropFontHandleWrapper, fontHandleWrapper2);
					}
					return fontHandleWrapper2.Handle;
				}
				return Control.GetDefaultFontHandleWrapper().Handle;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00015D34 File Offset: 0x00014D34
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00015DD5 File Offset: 0x00014DD5
		protected int FontHeight
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropFontHeight, out flag);
				if (flag && integer != -1)
				{
					return integer;
				}
				Font font = (Font)this.Properties.GetObject(Control.PropFont);
				if (font != null)
				{
					integer = font.Height;
					this.Properties.SetInteger(Control.PropFontHeight, integer);
					return integer;
				}
				int num = -1;
				if (this.ParentInternal != null && this.ParentInternal.CanAccessProperties)
				{
					num = this.ParentInternal.FontHeight;
				}
				if (num == -1)
				{
					num = this.Font.Height;
					this.Properties.SetInteger(Control.PropFontHeight, num);
				}
				return num;
			}
			set
			{
				this.Properties.SetInteger(Control.PropFontHeight, value);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00015DE8 File Offset: 0x00014DE8
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00015E6C File Offset: 0x00014E6C
		[DispId(-513)]
		[SRDescription("ControlForeColorDescr")]
		[SRCategory("CatAppearance")]
		public virtual Color ForeColor
		{
			get
			{
				Color color = this.Properties.GetColor(Control.PropForeColor);
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal.CanAccessProperties)
				{
					return parentInternal.ForeColor;
				}
				Color color2 = Color.Empty;
				if (this.IsActiveX)
				{
					color2 = this.ActiveXAmbientForeColor;
				}
				if (color2.IsEmpty)
				{
					AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
					if (ambientPropertiesService != null)
					{
						color2 = ambientPropertiesService.ForeColor;
					}
				}
				if (!color2.IsEmpty)
				{
					return color2;
				}
				return Control.DefaultForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(Control.PropForeColor))
				{
					this.Properties.SetColor(Control.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060013E2 RID: 5090 RVA: 0x00015ED1 File Offset: 0x00014ED1
		// (remove) Token: 0x060013E3 RID: 5091 RVA: 0x00015EE4 File Offset: 0x00014EE4
		[SRDescription("ControlOnForeColorChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventForeColor, value);
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00015EF7 File Offset: 0x00014EF7
		private Font GetParentFont()
		{
			if (this.ParentInternal != null && this.ParentInternal.CanAccessProperties)
			{
				return this.ParentInternal.Font;
			}
			return null;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00015F1C File Offset: 0x00014F1C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual Size GetPreferredSize(Size proposedSize)
		{
			Size size;
			if (this.GetState(6144))
			{
				size = CommonProperties.xGetPreferredSizeCache(this);
			}
			else
			{
				proposedSize = LayoutUtils.ConvertZeroToUnbounded(proposedSize);
				proposedSize = this.ApplySizeConstraints(proposedSize);
				if (this.GetState2(2048))
				{
					Size size2 = CommonProperties.xGetPreferredSizeCache(this);
					if (!size2.IsEmpty && proposedSize == LayoutUtils.MaxSize)
					{
						return size2;
					}
				}
				this.CacheTextInternal = true;
				try
				{
					size = this.GetPreferredSizeCore(proposedSize);
				}
				finally
				{
					this.CacheTextInternal = false;
				}
				size = this.ApplySizeConstraints(size);
				if (this.GetState2(2048) && proposedSize == LayoutUtils.MaxSize)
				{
					CommonProperties.xSetPreferredSizeCache(this, size);
				}
			}
			return size;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00015FD0 File Offset: 0x00014FD0
		internal virtual Size GetPreferredSizeCore(Size proposedSize)
		{
			return CommonProperties.GetSpecifiedBounds(this).Size;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x00015FEC File Offset: 0x00014FEC
		[DispId(-515)]
		[SRDescription("ControlHandleDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IntPtr Handle
		{
			get
			{
				if (Control.checkForIllegalCrossThreadCalls && !Control.inCrossThreadSafeCall && this.InvokeRequired)
				{
					throw new InvalidOperationException(SR.GetString("IllegalCrossThreadCall", new object[] { this.Name }));
				}
				if (!this.IsHandleCreated)
				{
					this.CreateHandle();
				}
				return this.HandleInternal;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x00016044 File Offset: 0x00015044
		internal IntPtr HandleInternal
		{
			get
			{
				return this.window.Handle;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00016054 File Offset: 0x00015054
		[SRDescription("ControlHasChildrenDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HasChildren
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				return controlCollection != null && controlCollection.Count > 0;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00016085 File Offset: 0x00015085
		internal virtual bool HasMenu
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00016088 File Offset: 0x00015088
		// (set) Token: 0x060013EC RID: 5100 RVA: 0x00016090 File Offset: 0x00015090
		[Browsable(false)]
		[SRDescription("ControlHeightDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.SetBounds(this.x, this.y, this.width, value, BoundsSpecified.Height);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x000160AC File Offset: 0x000150AC
		internal bool HostedInWin32DialogManager
		{
			get
			{
				if (!this.GetState(16777216))
				{
					Control topMostParent = this.TopMostParent;
					if (this != topMostParent)
					{
						this.SetState(33554432, topMostParent.HostedInWin32DialogManager);
					}
					else
					{
						IntPtr intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle));
						IntPtr intPtr2 = intPtr;
						StringBuilder stringBuilder = new StringBuilder(32);
						this.SetState(33554432, false);
						while (intPtr != IntPtr.Zero)
						{
							int className = UnsafeNativeMethods.GetClassName(new HandleRef(null, intPtr2), null, 0);
							if (className > stringBuilder.Capacity)
							{
								stringBuilder.Capacity = className + 5;
							}
							UnsafeNativeMethods.GetClassName(new HandleRef(null, intPtr2), stringBuilder, stringBuilder.Capacity);
							if (stringBuilder.ToString() == "#32770")
							{
								this.SetState(33554432, true);
								break;
							}
							intPtr2 = intPtr;
							intPtr = UnsafeNativeMethods.GetParent(new HandleRef(null, intPtr));
						}
					}
					this.SetState(16777216, true);
				}
				return this.GetState(33554432);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0001619F File Offset: 0x0001519F
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHandleCreatedDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsHandleCreated
		{
			get
			{
				return this.window.Handle != IntPtr.Zero;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000161B6 File Offset: 0x000151B6
		internal bool IsLayoutSuspended
		{
			get
			{
				return this.layoutSuspendCount > 0;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x000161C4 File Offset: 0x000151C4
		internal bool IsWindowObscured
		{
			get
			{
				if (!this.IsHandleCreated || !this.Visible)
				{
					return false;
				}
				bool flag = false;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				Control control = this.ParentInternal;
				if (control != null)
				{
					while (control.ParentInternal != null)
					{
						control = control.ParentInternal;
					}
				}
				UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
				Region region = new Region(Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom));
				try
				{
					IntPtr intPtr;
					if (control != null)
					{
						intPtr = control.Handle;
					}
					else
					{
						intPtr = this.Handle;
					}
					IntPtr intPtr2 = intPtr;
					IntPtr intPtr3;
					while ((intPtr3 = UnsafeNativeMethods.GetWindow(new HandleRef(null, intPtr2), 3)) != IntPtr.Zero)
					{
						UnsafeNativeMethods.GetWindowRect(new HandleRef(null, intPtr3), ref rect);
						Rectangle rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
						if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr3)))
						{
							region.Exclude(rectangle);
						}
						intPtr2 = intPtr3;
					}
					Graphics graphics = this.CreateGraphics();
					try
					{
						flag = region.IsEmpty(graphics);
					}
					finally
					{
						graphics.Dispose();
					}
				}
				finally
				{
					region.Dispose();
				}
				return flag;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x0001630C File Offset: 0x0001530C
		internal IntPtr InternalHandle
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return IntPtr.Zero;
				}
				return this.Handle;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00016324 File Offset: 0x00015324
		[SRDescription("ControlInvokeRequiredDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool InvokeRequired
		{
			get
			{
				bool flag;
				using (new Control.MultithreadSafeCallScope())
				{
					HandleRef handleRef;
					if (this.IsHandleCreated)
					{
						handleRef = new HandleRef(this, this.Handle);
					}
					else
					{
						Control control = this.FindMarshalingControl();
						if (!control.IsHandleCreated)
						{
							return false;
						}
						handleRef = new HandleRef(control, control.Handle);
					}
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(handleRef, out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					flag = windowThreadProcessId != currentThreadId;
				}
				return flag;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x000163AC File Offset: 0x000153AC
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x000163B9 File Offset: 0x000153B9
		[SRDescription("ControlIsAccessibleDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAccessible
		{
			get
			{
				return this.GetState(1048576);
			}
			set
			{
				this.SetState(1048576, value);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x000163C7 File Offset: 0x000153C7
		internal bool IsActiveX
		{
			get
			{
				return this.GetState2(1024);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x000163D4 File Offset: 0x000153D4
		internal virtual bool IsContainerControl
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x000163D7 File Offset: 0x000153D7
		internal bool IsIEParent
		{
			get
			{
				return this.IsActiveX && this.ActiveXInstance.IsIE;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x000163F0 File Offset: 0x000153F0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("IsMirroredDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		public bool IsMirrored
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					CreateParams createParams = this.CreateParams;
					this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				}
				return this.GetState(1073741824);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00016434 File Offset: 0x00015434
		internal virtual bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00016437 File Offset: 0x00015437
		private bool IsValidBackColor(Color c)
		{
			return c.IsEmpty || this.GetStyle(ControlStyles.SupportsTransparentBackColor) || c.A >= byte.MaxValue;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00016460 File Offset: 0x00015460
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x00016468 File Offset: 0x00015468
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlLeftDescr")]
		[SRCategory("CatLayout")]
		public int Left
		{
			get
			{
				return this.x;
			}
			set
			{
				this.SetBounds(value, this.y, this.width, this.height, BoundsSpecified.X);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x00016484 File Offset: 0x00015484
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x00016497 File Offset: 0x00015497
		[SRDescription("ControlLocationDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		public Point Location
		{
			get
			{
				return new Point(this.x, this.y);
			}
			set
			{
				this.SetBounds(value.X, value.Y, this.width, this.height, BoundsSpecified.Location);
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060013FF RID: 5119 RVA: 0x000164BA File Offset: 0x000154BA
		// (remove) Token: 0x06001400 RID: 5120 RVA: 0x000164CD File Offset: 0x000154CD
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnLocationChangedDescr")]
		public event EventHandler LocationChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventLocation, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLocation, value);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x000164E0 File Offset: 0x000154E0
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x000164E8 File Offset: 0x000154E8
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ControlMarginDescr")]
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (value != this.Margin)
				{
					CommonProperties.SetMargin(this, value);
					this.OnMarginChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001403 RID: 5123 RVA: 0x00016512 File Offset: 0x00015512
		// (remove) Token: 0x06001404 RID: 5124 RVA: 0x00016525 File Offset: 0x00015525
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnMarginChangedDescr")]
		public event EventHandler MarginChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventMarginChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMarginChanged, value);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00016538 File Offset: 0x00015538
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x00016546 File Offset: 0x00015546
		[SRCategory("CatLayout")]
		[SRDescription("ControlMaximumSizeDescr")]
		[AmbientValue(typeof(Size), "0, 0")]
		public virtual Size MaximumSize
		{
			get
			{
				return CommonProperties.GetMaximumSize(this, this.DefaultMaximumSize);
			}
			set
			{
				if (value == Size.Empty)
				{
					CommonProperties.ClearMaximumSize(this);
					return;
				}
				if (value != this.MaximumSize)
				{
					CommonProperties.SetMaximumSize(this, value);
				}
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00016571 File Offset: 0x00015571
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x0001657F File Offset: 0x0001557F
		[SRDescription("ControlMinimumSizeDescr")]
		[SRCategory("CatLayout")]
		public virtual Size MinimumSize
		{
			get
			{
				return CommonProperties.GetMinimumSize(this, this.DefaultMinimumSize);
			}
			set
			{
				if (value != this.MinimumSize)
				{
					CommonProperties.SetMinimumSize(this, value);
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00016598 File Offset: 0x00015598
		public static Keys ModifierKeys
		{
			get
			{
				Keys keys = Keys.None;
				if (UnsafeNativeMethods.GetKeyState(16) < 0)
				{
					keys |= Keys.Shift;
				}
				if (UnsafeNativeMethods.GetKeyState(17) < 0)
				{
					keys |= Keys.Control;
				}
				if (UnsafeNativeMethods.GetKeyState(18) < 0)
				{
					keys |= Keys.Alt;
				}
				return keys;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x000165E0 File Offset: 0x000155E0
		public static MouseButtons MouseButtons
		{
			get
			{
				MouseButtons mouseButtons = MouseButtons.None;
				if (UnsafeNativeMethods.GetKeyState(1) < 0)
				{
					mouseButtons |= MouseButtons.Left;
				}
				if (UnsafeNativeMethods.GetKeyState(2) < 0)
				{
					mouseButtons |= MouseButtons.Right;
				}
				if (UnsafeNativeMethods.GetKeyState(4) < 0)
				{
					mouseButtons |= MouseButtons.Middle;
				}
				if (UnsafeNativeMethods.GetKeyState(5) < 0)
				{
					mouseButtons |= MouseButtons.XButton1;
				}
				if (UnsafeNativeMethods.GetKeyState(6) < 0)
				{
					mouseButtons |= MouseButtons.XButton2;
				}
				return mouseButtons;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00016648 File Offset: 0x00015648
		public static Point MousePosition
		{
			get
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				return new Point(point.x, point.y);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00016674 File Offset: 0x00015674
		// (set) Token: 0x0600140D RID: 5133 RVA: 0x000166BD File Offset: 0x000156BD
		[Browsable(false)]
		public string Name
		{
			get
			{
				string text = (string)this.Properties.GetObject(Control.PropName);
				if (string.IsNullOrEmpty(text))
				{
					if (this.Site != null)
					{
						text = this.Site.Name;
					}
					if (text == null)
					{
						text = "";
					}
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Properties.SetObject(Control.PropName, null);
					return;
				}
				this.Properties.SetObject(Control.PropName, value);
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x000166EA File Offset: 0x000156EA
		// (set) Token: 0x0600140F RID: 5135 RVA: 0x000166FC File Offset: 0x000156FC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlParentDescr")]
		[SRCategory("CatBehavior")]
		public Control Parent
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.ParentInternal;
			}
			set
			{
				this.ParentInternal = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00016705 File Offset: 0x00015705
		// (set) Token: 0x06001411 RID: 5137 RVA: 0x0001670D File Offset: 0x0001570D
		internal virtual Control ParentInternal
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					if (value != null)
					{
						value.Controls.Add(this);
						return;
					}
					this.parent.Controls.Remove(this);
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00016739 File Offset: 0x00015739
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlProductNameDescr")]
		public string ProductName
		{
			get
			{
				return this.VersionInfo.ProductName;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x00016746 File Offset: 0x00015746
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ControlProductVersionDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ProductVersion
		{
			get
			{
				return this.VersionInfo.ProductVersion;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00016753 File Offset: 0x00015753
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0001675B File Offset: 0x0001575B
		internal Color RawBackColor
		{
			get
			{
				return this.Properties.GetColor(Control.PropBackColor);
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0001676D File Offset: 0x0001576D
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ControlRecreatingHandleDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool RecreatingHandle
		{
			get
			{
				return (this.state & 16) != 0;
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0001677E File Offset: 0x0001577E
		internal virtual void AddReflectChild()
		{
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00016780 File Offset: 0x00015780
		internal virtual void RemoveReflectChild()
		{
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00016782 File Offset: 0x00015782
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x0001678C File Offset: 0x0001578C
		private Control ReflectParent
		{
			get
			{
				return this.reflectParent;
			}
			set
			{
				if (value != null)
				{
					value.AddReflectChild();
				}
				Control control = this.ReflectParent;
				this.reflectParent = value;
				if (control != null)
				{
					control.RemoveReflectChild();
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x000167B9 File Offset: 0x000157B9
		// (set) Token: 0x0600141C RID: 5148 RVA: 0x000167D0 File Offset: 0x000157D0
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlRegionDescr")]
		public Region Region
		{
			get
			{
				return (Region)this.Properties.GetObject(Control.PropRegion);
			}
			set
			{
				if (this.GetState(524288))
				{
					IntSecurity.ChangeWindowRegionForTopLevel.Demand();
				}
				Region region = this.Region;
				if (region != value)
				{
					this.Properties.SetObject(Control.PropRegion, value);
					if (region != null)
					{
						region.Dispose();
					}
					if (this.IsHandleCreated)
					{
						IntPtr intPtr = IntPtr.Zero;
						try
						{
							if (value != null)
							{
								intPtr = this.GetHRgn(value);
							}
							if (this.IsActiveX)
							{
								intPtr = this.ActiveXMergeRegion(intPtr);
							}
							if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, this.Handle), new HandleRef(this, intPtr), SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle))) != 0)
							{
								intPtr = IntPtr.Zero;
							}
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
							}
						}
					}
					this.OnRegionChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600141D RID: 5149 RVA: 0x000168B0 File Offset: 0x000158B0
		// (remove) Token: 0x0600141E RID: 5150 RVA: 0x000168C3 File Offset: 0x000158C3
		[SRDescription("ControlRegionChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RegionChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventRegionChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventRegionChanged, value);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x000168D6 File Offset: 0x000158D6
		[Obsolete("This property has been deprecated. Please use RightToLeft instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected internal bool RenderRightToLeft
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x000168DC File Offset: 0x000158DC
		internal bool RenderTransparent
		{
			get
			{
				return this.GetStyle(ControlStyles.SupportsTransparentBackColor) && this.BackColor.A < byte.MaxValue;
			}
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0001690D File Offset: 0x0001590D
		private bool RenderColorTransparent(Color c)
		{
			return this.GetStyle(ControlStyles.SupportsTransparentBackColor) && c.A < byte.MaxValue;
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0001692C File Offset: 0x0001592C
		internal virtual bool RenderTransparencyWithVisualStyles
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0001692F File Offset: 0x0001592F
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x00016948 File Offset: 0x00015948
		internal BoundsSpecified RequiredScaling
		{
			get
			{
				if ((this.requiredScaling & 16) != 0)
				{
					return (BoundsSpecified)(this.requiredScaling & 15);
				}
				return BoundsSpecified.None;
			}
			set
			{
				byte b = this.requiredScaling & 16;
				this.requiredScaling = (byte)((value & BoundsSpecified.All) | (BoundsSpecified)b);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0001696D File Offset: 0x0001596D
		// (set) Token: 0x06001426 RID: 5158 RVA: 0x00016980 File Offset: 0x00015980
		internal bool RequiredScalingEnabled
		{
			get
			{
				return (this.requiredScaling & 16) != 0;
			}
			set
			{
				byte b = this.requiredScaling & 15;
				this.requiredScaling = b;
				if (value)
				{
					this.requiredScaling |= 16;
				}
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x000169B2 File Offset: 0x000159B2
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x000169BC File Offset: 0x000159BC
		[SRDescription("ControlResizeRedrawDescr")]
		protected bool ResizeRedraw
		{
			get
			{
				return this.GetStyle(ControlStyles.ResizeRedraw);
			}
			set
			{
				this.SetStyle(ControlStyles.ResizeRedraw, value);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x000169C7 File Offset: 0x000159C7
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[SRDescription("ControlRightDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Right
		{
			get
			{
				return this.x + this.width;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x000169D8 File Offset: 0x000159D8
		// (set) Token: 0x0600142B RID: 5163 RVA: 0x00016A1C File Offset: 0x00015A1C
		[SRDescription("ControlRightToLeftDescr")]
		[Localizable(true)]
		[AmbientValue(RightToLeft.Inherit)]
		[SRCategory("CatAppearance")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				bool flag;
				int num = this.Properties.GetInteger(Control.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						num = (int)parentInternal.RightToLeft;
					}
					else
					{
						num = (int)this.DefaultRightToLeft;
					}
				}
				return (RightToLeft)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				RightToLeft rightToLeft = this.RightToLeft;
				if (this.Properties.ContainsInteger(Control.PropRightToLeft) || value != RightToLeft.Inherit)
				{
					this.Properties.SetInteger(Control.PropRightToLeft, (int)value);
				}
				if (rightToLeft != this.RightToLeft)
				{
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeft))
					{
						this.OnRightToLeftChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600142C RID: 5164 RVA: 0x00016ABC File Offset: 0x00015ABC
		// (remove) Token: 0x0600142D RID: 5165 RVA: 0x00016ACF File Offset: 0x00015ACF
		[SRDescription("ControlOnRightToLeftChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventRightToLeft, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventRightToLeft, value);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00016AE2 File Offset: 0x00015AE2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual bool ScaleChildren
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x00016AE5 File Offset: 0x00015AE5
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x00016AF0 File Offset: 0x00015AF0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				AmbientProperties ambientPropertiesService = this.AmbientPropertiesService;
				AmbientProperties ambientProperties = null;
				if (value != null)
				{
					ambientProperties = (AmbientProperties)value.GetService(typeof(AmbientProperties));
				}
				if (ambientPropertiesService != ambientProperties)
				{
					bool flag = !this.Properties.ContainsObject(Control.PropFont);
					bool flag2 = !this.Properties.ContainsObject(Control.PropBackColor);
					bool flag3 = !this.Properties.ContainsObject(Control.PropForeColor);
					bool flag4 = !this.Properties.ContainsObject(Control.PropCursor);
					Font font = null;
					Color color = Color.Empty;
					Color color2 = Color.Empty;
					Cursor cursor = null;
					if (flag)
					{
						font = this.Font;
					}
					if (flag2)
					{
						color = this.BackColor;
					}
					if (flag3)
					{
						color2 = this.ForeColor;
					}
					if (flag4)
					{
						cursor = this.Cursor;
					}
					this.Properties.SetObject(Control.PropAmbientPropertiesService, ambientProperties);
					base.Site = value;
					if (flag && !font.Equals(this.Font))
					{
						this.OnFontChanged(EventArgs.Empty);
					}
					if (flag3 && !color2.Equals(this.ForeColor))
					{
						this.OnForeColorChanged(EventArgs.Empty);
					}
					if (flag2 && !color.Equals(this.BackColor))
					{
						this.OnBackColorChanged(EventArgs.Empty);
					}
					if (flag4 && cursor.Equals(this.Cursor))
					{
						this.OnCursorChanged(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.Site = value;
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x00016C66 File Offset: 0x00015C66
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x00016C79 File Offset: 0x00015C79
		[Localizable(true)]
		[SRDescription("ControlSizeDescr")]
		[SRCategory("CatLayout")]
		public Size Size
		{
			get
			{
				return new Size(this.width, this.height);
			}
			set
			{
				this.SetBounds(this.x, this.y, value.Width, value.Height, BoundsSpecified.Size);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001433 RID: 5171 RVA: 0x00016C9D File Offset: 0x00015C9D
		// (remove) Token: 0x06001434 RID: 5172 RVA: 0x00016CB0 File Offset: 0x00015CB0
		[SRDescription("ControlOnSizeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler SizeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventSize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventSize, value);
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00016CC3 File Offset: 0x00015CC3
		// (set) Token: 0x06001436 RID: 5174 RVA: 0x00016CD8 File Offset: 0x00015CD8
		[Localizable(true)]
		[SRDescription("ControlTabIndexDescr")]
		[SRCategory("CatBehavior")]
		[MergableProperty(false)]
		public int TabIndex
		{
			get
			{
				if (this.tabIndex != -1)
				{
					return this.tabIndex;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("TabIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"TabIndex",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.tabIndex != value)
				{
					this.tabIndex = value;
					this.OnTabIndexChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001437 RID: 5175 RVA: 0x00016D49 File Offset: 0x00015D49
		// (remove) Token: 0x06001438 RID: 5176 RVA: 0x00016D5C File Offset: 0x00015D5C
		[SRDescription("ControlOnTabIndexChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TabIndexChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventTabIndex, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventTabIndex, value);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00016D6F File Offset: 0x00015D6F
		// (set) Token: 0x0600143A RID: 5178 RVA: 0x00016D7F File Offset: 0x00015D7F
		[SRDescription("ControlTabStopDescr")]
		[SRCategory("CatBehavior")]
		[DispId(-516)]
		[DefaultValue(true)]
		public bool TabStop
		{
			get
			{
				return (this.state & 8) != 0;
			}
			set
			{
				if (this.TabStop != value)
				{
					this.SetState(8, value);
					if (this.IsHandleCreated)
					{
						this.SetWindowStyle(65536, value);
					}
					this.OnTabStopChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600143B RID: 5179 RVA: 0x00016DB1 File Offset: 0x00015DB1
		// (remove) Token: 0x0600143C RID: 5180 RVA: 0x00016DC4 File Offset: 0x00015DC4
		[SRDescription("ControlOnTabStopChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TabStopChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventTabStop, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventTabStop, value);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00016DD7 File Offset: 0x00015DD7
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x00016DE9 File Offset: 0x00015DE9
		[Bindable(true)]
		[SRCategory("CatData")]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(Control.PropUserData);
			}
			set
			{
				this.Properties.SetObject(Control.PropUserData, value);
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00016DFC File Offset: 0x00015DFC
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x00016E24 File Offset: 0x00015E24
		[Bindable(true)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ControlTextDescr")]
		[DispId(-517)]
		public virtual string Text
		{
			get
			{
				if (!this.CacheTextInternal)
				{
					return this.WindowText;
				}
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value == this.Text)
				{
					return;
				}
				if (this.CacheTextInternal)
				{
					this.text = value;
				}
				this.WindowText = value;
				this.OnTextChanged(EventArgs.Empty);
				if (this.IsMnemonicsListenerAxSourced)
				{
					for (Control control = this; control != null; control = control.ParentInternal)
					{
						Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)control.Properties.GetObject(Control.PropActiveXImpl);
						if (activeXImpl != null)
						{
							activeXImpl.UpdateAccelTable();
							return;
						}
					}
				}
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001441 RID: 5185 RVA: 0x00016EA1 File Offset: 0x00015EA1
		// (remove) Token: 0x06001442 RID: 5186 RVA: 0x00016EB4 File Offset: 0x00015EB4
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnTextChangedDescr")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventText, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventText, value);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x00016EC7 File Offset: 0x00015EC7
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x00016ECF File Offset: 0x00015ECF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlTopDescr")]
		[SRCategory("CatLayout")]
		public int Top
		{
			get
			{
				return this.y;
			}
			set
			{
				this.SetBounds(this.x, value, this.width, this.height, BoundsSpecified.Y);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x00016EEB File Offset: 0x00015EEB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlTopLevelControlDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		public Control TopLevelControl
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.TopLevelControlInternal;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00016F00 File Offset: 0x00015F00
		internal Control TopLevelControlInternal
		{
			get
			{
				Control control = this;
				while (control != null && !control.GetTopLevel())
				{
					control = control.ParentInternal;
				}
				return control;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x00016F24 File Offset: 0x00015F24
		internal Control TopMostParent
		{
			get
			{
				Control control = this;
				while (control.ParentInternal != null)
				{
					control = control.ParentInternal;
				}
				return control;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00016F45 File Offset: 0x00015F45
		private BufferedGraphicsContext BufferContext
		{
			get
			{
				return BufferedGraphicsManager.Current;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00016F4C File Offset: 0x00015F4C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		protected internal virtual bool ShowKeyboardCues
		{
			get
			{
				if (!this.IsHandleCreated || base.DesignMode)
				{
					return true;
				}
				if ((this.uiCuesState & 240) == 0)
				{
					if (SystemInformation.MenuAccessKeysUnderlined)
					{
						this.uiCuesState |= 32;
					}
					else
					{
						int num = 196608;
						this.uiCuesState |= 16;
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TopMostParent, this.TopMostParent.Handle), 295, (IntPtr)(num | 1), IntPtr.Zero);
					}
				}
				return (this.uiCuesState & 240) == 32;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x00016FE4 File Offset: 0x00015FE4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal virtual bool ShowFocusCues
		{
			get
			{
				if (!this.IsHandleCreated)
				{
					return true;
				}
				if ((this.uiCuesState & 15) == 0)
				{
					if (SystemInformation.MenuAccessKeysUnderlined)
					{
						this.uiCuesState |= 2;
					}
					else
					{
						this.uiCuesState |= 1;
						int num = 196608;
						UnsafeNativeMethods.SendMessage(new HandleRef(this.TopMostParent, this.TopMostParent.Handle), 295, (IntPtr)(num | 1), IntPtr.Zero);
					}
				}
				return (this.uiCuesState & 15) == 2;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0001706B File Offset: 0x0001606B
		internal virtual int ShowParams
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0001706E File Offset: 0x0001606E
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x0001707C File Offset: 0x0001607C
		[SRCategory("CatAppearance")]
		[Browsable(true)]
		[SRDescription("ControlUseWaitCursorDescr")]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public bool UseWaitCursor
		{
			get
			{
				return this.GetState(1024);
			}
			set
			{
				if (this.GetState(1024) != value)
				{
					this.SetState(1024, value);
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							controlCollection[i].UseWaitCursor = value;
						}
					}
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x000170DC File Offset: 0x000160DC
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0001711C File Offset: 0x0001611C
		internal bool UseCompatibleTextRenderingInt
		{
			get
			{
				if (this.Properties.ContainsInteger(Control.PropUseCompatibleTextRendering))
				{
					bool flag;
					int integer = this.Properties.GetInteger(Control.PropUseCompatibleTextRendering, out flag);
					if (flag)
					{
						return integer == 1;
					}
				}
				return Control.UseCompatibleTextRenderingDefault;
			}
			set
			{
				if (this.SupportsUseCompatibleTextRendering && this.UseCompatibleTextRenderingInt != value)
				{
					this.Properties.SetInteger(Control.PropUseCompatibleTextRendering, value ? 1 : 0);
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.UseCompatibleTextRendering);
					this.Invalidate();
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0001716E File Offset: 0x0001616E
		internal virtual bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x00017174 File Offset: 0x00016174
		private Control.ControlVersionInfo VersionInfo
		{
			get
			{
				Control.ControlVersionInfo controlVersionInfo = (Control.ControlVersionInfo)this.Properties.GetObject(Control.PropControlVersionInfo);
				if (controlVersionInfo == null)
				{
					controlVersionInfo = new Control.ControlVersionInfo(this);
					this.Properties.SetObject(Control.PropControlVersionInfo, controlVersionInfo);
				}
				return controlVersionInfo;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x000171B3 File Offset: 0x000161B3
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x000171BB File Offset: 0x000161BB
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("ControlVisibleDescr")]
		public bool Visible
		{
			get
			{
				return this.GetVisibleCore();
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001454 RID: 5204 RVA: 0x000171C4 File Offset: 0x000161C4
		// (remove) Token: 0x06001455 RID: 5205 RVA: 0x000171D7 File Offset: 0x000161D7
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnVisibleChangedDescr")]
		public event EventHandler VisibleChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventVisible, value);
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x000171EC File Offset: 0x000161EC
		private void WaitForWaitHandle(WaitHandle waitHandle)
		{
			int createThreadId = this.CreateThreadId;
			Application.ThreadContext threadContext = Application.ThreadContext.FromId(createThreadId);
			if (threadContext == null)
			{
				return;
			}
			IntPtr handle = threadContext.GetHandle();
			bool flag = false;
			while (!flag)
			{
				uint num;
				bool exitCodeThread = UnsafeNativeMethods.GetExitCodeThread(handle, out num);
				if ((exitCodeThread && num != 259U) || AppDomain.CurrentDomain.IsFinalizingForUnload())
				{
					if (waitHandle.WaitOne(1, false))
					{
						return;
					}
					throw new InvalidAsynchronousStateException(SR.GetString("ThreadNoLongerValid"));
				}
				else
				{
					if (this.IsDisposed)
					{
						throw new InvalidOperationException(SR.GetString("ErrorNoMarshalingThread"));
					}
					flag = waitHandle.WaitOne(1000, false);
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0001727F File Offset: 0x0001627F
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x00017287 File Offset: 0x00016287
		[SRDescription("ControlWidthDescr")]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.SetBounds(this.x, this.y, value, this.height, BoundsSpecified.Width);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x000172A3 File Offset: 0x000162A3
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x000172BE File Offset: 0x000162BE
		private int WindowExStyle
		{
			get
			{
				return (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -20);
			}
			set
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -20, new HandleRef(null, (IntPtr)value));
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x000172E0 File Offset: 0x000162E0
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x000172FB File Offset: 0x000162FB
		internal int WindowStyle
		{
			get
			{
				return (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16);
			}
			set
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)value));
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0001731D File Offset: 0x0001631D
		// (set) Token: 0x0600145E RID: 5214 RVA: 0x0001732A File Offset: 0x0001632A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWindowTargetDescr")]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IWindowTarget WindowTarget
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.window.WindowTarget;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				this.window.WindowTarget = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x00017338 File Offset: 0x00016338
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x000173D8 File Offset: 0x000163D8
		internal virtual string WindowText
		{
			get
			{
				if (this.IsHandleCreated)
				{
					string text;
					using (new Control.MultithreadSafeCallScope())
					{
						int num = SafeNativeMethods.GetWindowTextLength(new HandleRef(this.window, this.Handle));
						if (SystemInformation.DbcsEnabled)
						{
							num = num * 2 + 1;
						}
						StringBuilder stringBuilder = new StringBuilder(num + 1);
						UnsafeNativeMethods.GetWindowText(new HandleRef(this.window, this.Handle), stringBuilder, stringBuilder.Capacity);
						text = stringBuilder.ToString();
					}
					return text;
				}
				if (this.text == null)
				{
					return "";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.WindowText.Equals(value))
				{
					if (this.IsHandleCreated)
					{
						UnsafeNativeMethods.SetWindowText(new HandleRef(this.window, this.Handle), value);
						return;
					}
					if (value.Length == 0)
					{
						this.text = null;
						return;
					}
					this.text = value;
				}
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001461 RID: 5217 RVA: 0x00017435 File Offset: 0x00016435
		// (remove) Token: 0x06001462 RID: 5218 RVA: 0x00017448 File Offset: 0x00016448
		[SRCategory("CatAction")]
		[SRDescription("ControlOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(Control.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventClick, value);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001463 RID: 5219 RVA: 0x0001745B File Offset: 0x0001645B
		// (remove) Token: 0x06001464 RID: 5220 RVA: 0x0001746E File Offset: 0x0001646E
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnControlAddedDescr")]
		public event ControlEventHandler ControlAdded
		{
			add
			{
				base.Events.AddHandler(Control.EventControlAdded, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventControlAdded, value);
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001465 RID: 5221 RVA: 0x00017481 File Offset: 0x00016481
		// (remove) Token: 0x06001466 RID: 5222 RVA: 0x00017494 File Offset: 0x00016494
		[SRDescription("ControlOnControlRemovedDescr")]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(true)]
		public event ControlEventHandler ControlRemoved
		{
			add
			{
				base.Events.AddHandler(Control.EventControlRemoved, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventControlRemoved, value);
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001467 RID: 5223 RVA: 0x000174A7 File Offset: 0x000164A7
		// (remove) Token: 0x06001468 RID: 5224 RVA: 0x000174BA File Offset: 0x000164BA
		[SRDescription("ControlOnDragDropDescr")]
		[SRCategory("CatDragDrop")]
		public event DragEventHandler DragDrop
		{
			add
			{
				base.Events.AddHandler(Control.EventDragDrop, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragDrop, value);
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001469 RID: 5225 RVA: 0x000174CD File Offset: 0x000164CD
		// (remove) Token: 0x0600146A RID: 5226 RVA: 0x000174E0 File Offset: 0x000164E0
		[SRDescription("ControlOnDragEnterDescr")]
		[SRCategory("CatDragDrop")]
		public event DragEventHandler DragEnter
		{
			add
			{
				base.Events.AddHandler(Control.EventDragEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragEnter, value);
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x0600146B RID: 5227 RVA: 0x000174F3 File Offset: 0x000164F3
		// (remove) Token: 0x0600146C RID: 5228 RVA: 0x00017506 File Offset: 0x00016506
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragOverDescr")]
		public event DragEventHandler DragOver
		{
			add
			{
				base.Events.AddHandler(Control.EventDragOver, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragOver, value);
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600146D RID: 5229 RVA: 0x00017519 File Offset: 0x00016519
		// (remove) Token: 0x0600146E RID: 5230 RVA: 0x0001752C File Offset: 0x0001652C
		[SRCategory("CatDragDrop")]
		[SRDescription("ControlOnDragLeaveDescr")]
		public event EventHandler DragLeave
		{
			add
			{
				base.Events.AddHandler(Control.EventDragLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDragLeave, value);
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x0600146F RID: 5231 RVA: 0x0001753F File Offset: 0x0001653F
		// (remove) Token: 0x06001470 RID: 5232 RVA: 0x00017552 File Offset: 0x00016552
		[SRDescription("ControlOnGiveFeedbackDescr")]
		[SRCategory("CatDragDrop")]
		public event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.Events.AddHandler(Control.EventGiveFeedback, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventGiveFeedback, value);
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001471 RID: 5233 RVA: 0x00017565 File Offset: 0x00016565
		// (remove) Token: 0x06001472 RID: 5234 RVA: 0x00017578 File Offset: 0x00016578
		[Browsable(false)]
		[SRCategory("CatPrivate")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnCreateHandleDescr")]
		public event EventHandler HandleCreated
		{
			add
			{
				base.Events.AddHandler(Control.EventHandleCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHandleCreated, value);
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001473 RID: 5235 RVA: 0x0001758B File Offset: 0x0001658B
		// (remove) Token: 0x06001474 RID: 5236 RVA: 0x0001759E File Offset: 0x0001659E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnDestroyHandleDescr")]
		[SRCategory("CatPrivate")]
		[Browsable(false)]
		public event EventHandler HandleDestroyed
		{
			add
			{
				base.Events.AddHandler(Control.EventHandleDestroyed, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHandleDestroyed, value);
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001475 RID: 5237 RVA: 0x000175B1 File Offset: 0x000165B1
		// (remove) Token: 0x06001476 RID: 5238 RVA: 0x000175C4 File Offset: 0x000165C4
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnHelpDescr")]
		public event HelpEventHandler HelpRequested
		{
			add
			{
				base.Events.AddHandler(Control.EventHelpRequested, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventHelpRequested, value);
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001477 RID: 5239 RVA: 0x000175D7 File Offset: 0x000165D7
		// (remove) Token: 0x06001478 RID: 5240 RVA: 0x000175EA File Offset: 0x000165EA
		[Browsable(false)]
		[SRDescription("ControlOnInvalidateDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatAppearance")]
		public event InvalidateEventHandler Invalidated
		{
			add
			{
				base.Events.AddHandler(Control.EventInvalidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventInvalidated, value);
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x000175FD File Offset: 0x000165FD
		[Browsable(false)]
		public Size PreferredSize
		{
			get
			{
				return this.GetPreferredSize(Size.Empty);
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0001760A File Offset: 0x0001660A
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x00017618 File Offset: 0x00016618
		[SRDescription("ControlPaddingDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		public Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (value != this.Padding)
				{
					CommonProperties.SetPadding(this, value);
					this.SetState(8388608, true);
					using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Padding))
					{
						this.OnPaddingChanged(EventArgs.Empty);
					}
					if (this.GetState(8388608))
					{
						LayoutTransaction.DoLayout(this, this, PropertyNames.Padding);
					}
				}
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600147C RID: 5244 RVA: 0x00017698 File Offset: 0x00016698
		// (remove) Token: 0x0600147D RID: 5245 RVA: 0x000176AB File Offset: 0x000166AB
		[SRDescription("ControlOnPaddingChangedDescr")]
		[SRCategory("CatLayout")]
		public event EventHandler PaddingChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventPaddingChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventPaddingChanged, value);
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600147E RID: 5246 RVA: 0x000176BE File Offset: 0x000166BE
		// (remove) Token: 0x0600147F RID: 5247 RVA: 0x000176D1 File Offset: 0x000166D1
		[SRDescription("ControlOnPaintDescr")]
		[SRCategory("CatAppearance")]
		public event PaintEventHandler Paint
		{
			add
			{
				base.Events.AddHandler(Control.EventPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventPaint, value);
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001480 RID: 5248 RVA: 0x000176E4 File Offset: 0x000166E4
		// (remove) Token: 0x06001481 RID: 5249 RVA: 0x000176F7 File Offset: 0x000166F7
		[SRDescription("ControlOnQueryContinueDragDescr")]
		[SRCategory("CatDragDrop")]
		public event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.Events.AddHandler(Control.EventQueryContinueDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventQueryContinueDrag, value);
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001482 RID: 5250 RVA: 0x0001770A File Offset: 0x0001670A
		// (remove) Token: 0x06001483 RID: 5251 RVA: 0x0001771D File Offset: 0x0001671D
		[SRDescription("ControlOnQueryAccessibilityHelpDescr")]
		[SRCategory("CatBehavior")]
		public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				base.Events.AddHandler(Control.EventQueryAccessibilityHelp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventQueryAccessibilityHelp, value);
			}
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06001484 RID: 5252 RVA: 0x00017730 File Offset: 0x00016730
		// (remove) Token: 0x06001485 RID: 5253 RVA: 0x00017743 File Offset: 0x00016743
		[SRDescription("ControlOnDoubleClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(Control.EventDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventDoubleClick, value);
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001486 RID: 5254 RVA: 0x00017756 File Offset: 0x00016756
		// (remove) Token: 0x06001487 RID: 5255 RVA: 0x00017769 File Offset: 0x00016769
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnEnterDescr")]
		public event EventHandler Enter
		{
			add
			{
				base.Events.AddHandler(Control.EventEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventEnter, value);
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06001488 RID: 5256 RVA: 0x0001777C File Offset: 0x0001677C
		// (remove) Token: 0x06001489 RID: 5257 RVA: 0x0001778F File Offset: 0x0001678F
		[SRCategory("CatFocus")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ControlOnGotFocusDescr")]
		public event EventHandler GotFocus
		{
			add
			{
				base.Events.AddHandler(Control.EventGotFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventGotFocus, value);
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600148A RID: 5258 RVA: 0x000177A2 File Offset: 0x000167A2
		// (remove) Token: 0x0600148B RID: 5259 RVA: 0x000177B5 File Offset: 0x000167B5
		[SRDescription("ControlOnKeyDownDescr")]
		[SRCategory("CatKey")]
		public event KeyEventHandler KeyDown
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyDown, value);
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600148C RID: 5260 RVA: 0x000177C8 File Offset: 0x000167C8
		// (remove) Token: 0x0600148D RID: 5261 RVA: 0x000177DB File Offset: 0x000167DB
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyPressDescr")]
		public event KeyPressEventHandler KeyPress
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyPress, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyPress, value);
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600148E RID: 5262 RVA: 0x000177EE File Offset: 0x000167EE
		// (remove) Token: 0x0600148F RID: 5263 RVA: 0x00017801 File Offset: 0x00016801
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyUpDescr")]
		public event KeyEventHandler KeyUp
		{
			add
			{
				base.Events.AddHandler(Control.EventKeyUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventKeyUp, value);
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06001490 RID: 5264 RVA: 0x00017814 File Offset: 0x00016814
		// (remove) Token: 0x06001491 RID: 5265 RVA: 0x00017827 File Offset: 0x00016827
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnLayoutDescr")]
		public event LayoutEventHandler Layout
		{
			add
			{
				base.Events.AddHandler(Control.EventLayout, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLayout, value);
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06001492 RID: 5266 RVA: 0x0001783A File Offset: 0x0001683A
		// (remove) Token: 0x06001493 RID: 5267 RVA: 0x0001784D File Offset: 0x0001684D
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnLeaveDescr")]
		public event EventHandler Leave
		{
			add
			{
				base.Events.AddHandler(Control.EventLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLeave, value);
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06001494 RID: 5268 RVA: 0x00017860 File Offset: 0x00016860
		// (remove) Token: 0x06001495 RID: 5269 RVA: 0x00017873 File Offset: 0x00016873
		[Browsable(false)]
		[SRDescription("ControlOnLostFocusDescr")]
		[SRCategory("CatFocus")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler LostFocus
		{
			add
			{
				base.Events.AddHandler(Control.EventLostFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventLostFocus, value);
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06001496 RID: 5270 RVA: 0x00017886 File Offset: 0x00016886
		// (remove) Token: 0x06001497 RID: 5271 RVA: 0x00017899 File Offset: 0x00016899
		[SRCategory("CatAction")]
		[SRDescription("ControlOnMouseClickDescr")]
		public event MouseEventHandler MouseClick
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseClick, value);
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06001498 RID: 5272 RVA: 0x000178AC File Offset: 0x000168AC
		// (remove) Token: 0x06001499 RID: 5273 RVA: 0x000178BF File Offset: 0x000168BF
		[SRCategory("CatAction")]
		[SRDescription("ControlOnMouseDoubleClickDescr")]
		public event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseDoubleClick, value);
			}
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600149A RID: 5274 RVA: 0x000178D2 File Offset: 0x000168D2
		// (remove) Token: 0x0600149B RID: 5275 RVA: 0x000178E5 File Offset: 0x000168E5
		[SRDescription("ControlOnMouseCaptureChangedDescr")]
		[SRCategory("CatAction")]
		public event EventHandler MouseCaptureChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseCaptureChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseCaptureChanged, value);
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x0600149C RID: 5276 RVA: 0x000178F8 File Offset: 0x000168F8
		// (remove) Token: 0x0600149D RID: 5277 RVA: 0x0001790B File Offset: 0x0001690B
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseDownDescr")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseDown, value);
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x0600149E RID: 5278 RVA: 0x0001791E File Offset: 0x0001691E
		// (remove) Token: 0x0600149F RID: 5279 RVA: 0x00017931 File Offset: 0x00016931
		[SRDescription("ControlOnMouseEnterDescr")]
		[SRCategory("CatMouse")]
		public event EventHandler MouseEnter
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseEnter, value);
			}
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x060014A0 RID: 5280 RVA: 0x00017944 File Offset: 0x00016944
		// (remove) Token: 0x060014A1 RID: 5281 RVA: 0x00017957 File Offset: 0x00016957
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseLeaveDescr")]
		public event EventHandler MouseLeave
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseLeave, value);
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x060014A2 RID: 5282 RVA: 0x0001796A File Offset: 0x0001696A
		// (remove) Token: 0x060014A3 RID: 5283 RVA: 0x0001797D File Offset: 0x0001697D
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseHoverDescr")]
		public event EventHandler MouseHover
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseHover, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseHover, value);
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x060014A4 RID: 5284 RVA: 0x00017990 File Offset: 0x00016990
		// (remove) Token: 0x060014A5 RID: 5285 RVA: 0x000179A3 File Offset: 0x000169A3
		[SRDescription("ControlOnMouseMoveDescr")]
		[SRCategory("CatMouse")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseMove, value);
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x060014A6 RID: 5286 RVA: 0x000179B6 File Offset: 0x000169B6
		// (remove) Token: 0x060014A7 RID: 5287 RVA: 0x000179C9 File Offset: 0x000169C9
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseUpDescr")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseUp, value);
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x060014A8 RID: 5288 RVA: 0x000179DC File Offset: 0x000169DC
		// (remove) Token: 0x060014A9 RID: 5289 RVA: 0x000179EF File Offset: 0x000169EF
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseWheelDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event MouseEventHandler MouseWheel
		{
			add
			{
				base.Events.AddHandler(Control.EventMouseWheel, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMouseWheel, value);
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060014AA RID: 5290 RVA: 0x00017A02 File Offset: 0x00016A02
		// (remove) Token: 0x060014AB RID: 5291 RVA: 0x00017A15 File Offset: 0x00016A15
		[SRDescription("ControlOnMoveDescr")]
		[SRCategory("CatLayout")]
		public event EventHandler Move
		{
			add
			{
				base.Events.AddHandler(Control.EventMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventMove, value);
			}
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060014AC RID: 5292 RVA: 0x00017A28 File Offset: 0x00016A28
		// (remove) Token: 0x060014AD RID: 5293 RVA: 0x00017A3B File Offset: 0x00016A3B
		[SRDescription("PreviewKeyDownDescr")]
		[SRCategory("CatKey")]
		public event PreviewKeyDownEventHandler PreviewKeyDown
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				base.Events.AddHandler(Control.EventPreviewKeyDown, value);
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				base.Events.RemoveHandler(Control.EventPreviewKeyDown, value);
			}
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060014AE RID: 5294 RVA: 0x00017A4E File Offset: 0x00016A4E
		// (remove) Token: 0x060014AF RID: 5295 RVA: 0x00017A61 File Offset: 0x00016A61
		[SRDescription("ControlOnResizeDescr")]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Resize
		{
			add
			{
				base.Events.AddHandler(Control.EventResize, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventResize, value);
			}
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060014B0 RID: 5296 RVA: 0x00017A74 File Offset: 0x00016A74
		// (remove) Token: 0x060014B1 RID: 5297 RVA: 0x00017A87 File Offset: 0x00016A87
		[SRDescription("ControlOnChangeUICuesDescr")]
		[SRCategory("CatBehavior")]
		public event UICuesEventHandler ChangeUICues
		{
			add
			{
				base.Events.AddHandler(Control.EventChangeUICues, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventChangeUICues, value);
			}
		}

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060014B2 RID: 5298 RVA: 0x00017A9A File Offset: 0x00016A9A
		// (remove) Token: 0x060014B3 RID: 5299 RVA: 0x00017AAD File Offset: 0x00016AAD
		[SRDescription("ControlOnStyleChangedDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler StyleChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventStyleChanged, value);
			}
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x060014B4 RID: 5300 RVA: 0x00017AC0 File Offset: 0x00016AC0
		// (remove) Token: 0x060014B5 RID: 5301 RVA: 0x00017AD3 File Offset: 0x00016AD3
		[SRCategory("CatBehavior")]
		[SRDescription("ControlOnSystemColorsChangedDescr")]
		public event EventHandler SystemColorsChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventSystemColorsChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventSystemColorsChanged, value);
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x060014B6 RID: 5302 RVA: 0x00017AE6 File Offset: 0x00016AE6
		// (remove) Token: 0x060014B7 RID: 5303 RVA: 0x00017AF9 File Offset: 0x00016AF9
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatingDescr")]
		public event CancelEventHandler Validating
		{
			add
			{
				base.Events.AddHandler(Control.EventValidating, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventValidating, value);
			}
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x060014B8 RID: 5304 RVA: 0x00017B0C File Offset: 0x00016B0C
		// (remove) Token: 0x060014B9 RID: 5305 RVA: 0x00017B1F File Offset: 0x00016B1F
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatedDescr")]
		public event EventHandler Validated
		{
			add
			{
				base.Events.AddHandler(Control.EventValidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventValidated, value);
			}
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00017B32 File Offset: 0x00016B32
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal void AccessibilityNotifyClients(AccessibleEvents accEvent, int childID)
		{
			this.AccessibilityNotifyClients(accEvent, -4, childID);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00017B3E File Offset: 0x00016B3E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void AccessibilityNotifyClients(AccessibleEvents accEvent, int objectID, int childID)
		{
			if (this.IsHandleCreated)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), objectID, childID + 1);
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00017B5E File Offset: 0x00016B5E
		private IntPtr ActiveXMergeRegion(IntPtr region)
		{
			return this.ActiveXInstance.MergeRegion(region);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00017B6C File Offset: 0x00016B6C
		private void ActiveXOnFocus(bool focus)
		{
			this.ActiveXInstance.OnFocus(focus);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00017B7A File Offset: 0x00016B7A
		private void ActiveXViewChanged()
		{
			this.ActiveXInstance.ViewChangedInternal();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00017B87 File Offset: 0x00016B87
		private void ActiveXUpdateBounds(ref int x, ref int y, ref int width, ref int height, int flags)
		{
			this.ActiveXInstance.UpdateBounds(ref x, ref y, ref width, ref height, flags);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00017B9C File Offset: 0x00016B9C
		internal virtual void AssignParent(Control value)
		{
			if (value != null)
			{
				this.RequiredScalingEnabled = value.RequiredScalingEnabled;
			}
			if (this.CanAccessProperties)
			{
				Font font = this.Font;
				Color foreColor = this.ForeColor;
				Color backColor = this.BackColor;
				RightToLeft rightToLeft = this.RightToLeft;
				bool enabled = this.Enabled;
				bool visible = this.Visible;
				this.parent = value;
				this.OnParentChanged(EventArgs.Empty);
				if (this.GetAnyDisposingInHierarchy())
				{
					return;
				}
				if (enabled != this.Enabled)
				{
					this.OnEnabledChanged(EventArgs.Empty);
				}
				bool visible2 = this.Visible;
				if (visible != visible2 && (visible || !visible2 || this.parent != null || this.GetTopLevel()))
				{
					this.OnVisibleChanged(EventArgs.Empty);
				}
				if (!font.Equals(this.Font))
				{
					this.OnFontChanged(EventArgs.Empty);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
				if (rightToLeft != this.RightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
				if (this.Properties.GetObject(Control.PropBindingManager) == null && this.Created)
				{
					this.OnBindingContextChanged(EventArgs.Empty);
				}
			}
			else
			{
				this.parent = value;
				this.OnParentChanged(EventArgs.Empty);
			}
			this.SetState(16777216, false);
			if (this.ParentInternal != null)
			{
				this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.All);
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x060014C1 RID: 5313 RVA: 0x00017D27 File Offset: 0x00016D27
		// (remove) Token: 0x060014C2 RID: 5314 RVA: 0x00017D3A File Offset: 0x00016D3A
		[SRDescription("ControlOnParentChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ParentChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventParent, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventParent, value);
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00017D4D File Offset: 0x00016D4D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IAsyncResult BeginInvoke(Delegate method)
		{
			return this.BeginInvoke(method, null);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00017D58 File Offset: 0x00016D58
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IAsyncResult BeginInvoke(Delegate method, params object[] args)
		{
			IAsyncResult asyncResult;
			using (new Control.MultithreadSafeCallScope())
			{
				Control control = this.FindMarshalingControl();
				asyncResult = (IAsyncResult)control.MarshaledInvoke(this, method, args, false);
			}
			return asyncResult;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00017DA0 File Offset: 0x00016DA0
		internal void BeginUpdateInternal()
		{
			if (!this.IsHandleCreated)
			{
				return;
			}
			if (this.updateCount == 0)
			{
				this.SendMessage(11, 0, 0);
			}
			this.updateCount += 1;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00017DD0 File Offset: 0x00016DD0
		public void BringToFront()
		{
			if (this.parent != null)
			{
				this.parent.Controls.SetChildIndex(this, 0);
				return;
			}
			if (this.IsHandleCreated && this.GetTopLevel() && SafeNativeMethods.IsWindowEnabled(new HandleRef(this.window, this.Handle)))
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.HWND_TOP, 0, 0, 0, 0, 3);
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00017E41 File Offset: 0x00016E41
		internal virtual bool CanProcessMnemonic()
		{
			return this.Enabled && this.Visible && (this.parent == null || this.parent.CanProcessMnemonic());
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00017E6C File Offset: 0x00016E6C
		internal virtual bool CanSelectCore()
		{
			if ((this.controlStyle & ControlStyles.Selectable) != ControlStyles.Selectable)
			{
				return false;
			}
			for (Control control = this; control != null; control = control.parent)
			{
				if (!control.Enabled || !control.Visible)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00017EB0 File Offset: 0x00016EB0
		internal static void CheckParentingCycle(Control bottom, Control toFind)
		{
			Form form = null;
			Control control = null;
			for (Control control2 = bottom; control2 != null; control2 = control2.ParentInternal)
			{
				control = control2;
				if (control2 == toFind)
				{
					throw new ArgumentException(SR.GetString("CircularOwner"));
				}
			}
			if (control != null && control is Form)
			{
				Form form2 = (Form)control;
				for (Form form3 = form2; form3 != null; form3 = form3.OwnerInternal)
				{
					form = form3;
					if (form3 == toFind)
					{
						throw new ArgumentException(SR.GetString("CircularOwner"));
					}
				}
			}
			if (form != null && form.ParentInternal != null)
			{
				Control.CheckParentingCycle(form.ParentInternal, toFind);
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00017F38 File Offset: 0x00016F38
		private void ChildGotFocus(Control child)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(true);
			}
			if (this.parent != null)
			{
				this.parent.ChildGotFocus(child);
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00017F5D File Offset: 0x00016F5D
		public bool Contains(Control ctl)
		{
			while (ctl != null)
			{
				ctl = ctl.ParentInternal;
				if (ctl == null)
				{
					return false;
				}
				if (ctl == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00017F78 File Offset: 0x00016F78
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new Control.ControlAccessibleObject(this);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00017F80 File Offset: 0x00016F80
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Control.ControlCollection CreateControlsInstance()
		{
			return new Control.ControlCollection(this);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00017F88 File Offset: 0x00016F88
		public Graphics CreateGraphics()
		{
			Graphics graphics;
			using (new Control.MultithreadSafeCallScope())
			{
				IntSecurity.CreateGraphicsForControl.Demand();
				graphics = this.CreateGraphicsInternal();
			}
			return graphics;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00017FCC File Offset: 0x00016FCC
		internal Graphics CreateGraphicsInternal()
		{
			return Graphics.FromHwndInternal(this.Handle);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00017FDC File Offset: 0x00016FDC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual void CreateHandle()
		{
			IntPtr intPtr = IntPtr.Zero;
			if (this.GetState(2048))
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.GetState(262144))
			{
				return;
			}
			Rectangle bounds;
			try
			{
				this.SetState(262144, true);
				bounds = this.Bounds;
				if (Application.UseVisualStyles)
				{
					intPtr = UnsafeNativeMethods.ThemingScope.Activate();
				}
				CreateParams createParams = this.CreateParams;
				this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				if (this.parent != null)
				{
					Rectangle clientRectangle = this.parent.ClientRectangle;
					if (!clientRectangle.IsEmpty)
					{
						if (createParams.X != -2147483648)
						{
							createParams.X -= clientRectangle.X;
						}
						if (createParams.Y != -2147483648)
						{
							createParams.Y -= clientRectangle.Y;
						}
					}
				}
				if (createParams.Parent == IntPtr.Zero && (createParams.Style & 1073741824) != 0)
				{
					Application.ParkHandle(createParams);
				}
				this.window.CreateHandle(createParams);
				this.UpdateReflectParent(true);
			}
			finally
			{
				this.SetState(262144, false);
				UnsafeNativeMethods.ThemingScope.Deactivate(intPtr);
			}
			if (this.Bounds != bounds)
			{
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0001813C File Offset: 0x0001713C
		public void CreateControl()
		{
			bool created = this.Created;
			this.CreateControl(false);
			if (this.Properties.GetObject(Control.PropBindingManager) == null && this.ParentInternal != null && !created)
			{
				this.OnBindingContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00018180 File Offset: 0x00017180
		internal void CreateControl(bool fIgnoreVisible)
		{
			bool flag = (this.state & 1) == 0;
			if ((flag && this.Visible) || fIgnoreVisible)
			{
				this.state |= 1;
				bool flag2 = false;
				try
				{
					if (!this.IsHandleCreated)
					{
						this.CreateHandle();
					}
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						Control[] array = new Control[controlCollection.Count];
						controlCollection.CopyTo(array, 0);
						foreach (Control control in array)
						{
							if (control.IsHandleCreated)
							{
								control.SetParentHandle(this.Handle);
							}
							control.CreateControl(fIgnoreVisible);
						}
					}
					flag2 = true;
				}
				finally
				{
					if (!flag2)
					{
						this.state &= -2;
					}
				}
				this.OnCreateControl();
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00018264 File Offset: 0x00017264
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void DefWndProc(ref Message m)
		{
			this.window.DefWndProc(ref m);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00018274 File Offset: 0x00017274
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual void DestroyHandle()
		{
			if (this.RecreatingHandle && this.threadCallbackList != null)
			{
				lock (this.threadCallbackList)
				{
					if (Control.threadCallbackMessage != 0)
					{
						NativeMethods.MSG msg = default(NativeMethods.MSG);
						if (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, this.Handle), Control.threadCallbackMessage, Control.threadCallbackMessage, 0))
						{
							this.SetState(32768, true);
						}
					}
				}
			}
			if (!this.RecreatingHandle && this.threadCallbackList != null)
			{
				lock (this.threadCallbackList)
				{
					Exception ex = new ObjectDisposedException(base.GetType().Name);
					while (this.threadCallbackList.Count > 0)
					{
						Control.ThreadMethodEntry threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
						threadMethodEntry.exception = ex;
						threadMethodEntry.Complete();
					}
				}
			}
			if ((64 & (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this.window, this.InternalHandle), -20)) != 0)
			{
				UnsafeNativeMethods.DefMDIChildProc(this.InternalHandle, 16, IntPtr.Zero, IntPtr.Zero);
			}
			else
			{
				this.window.DestroyHandle();
			}
			this.trackMouseEvent = null;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x000183B4 File Offset: 0x000173B4
		protected override void Dispose(bool disposing)
		{
			if (this.GetState(2097152))
			{
				object @object = this.Properties.GetObject(Control.PropBackBrush);
				if (@object != null)
				{
					IntPtr intPtr = (IntPtr)@object;
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
					}
					this.Properties.SetObject(Control.PropBackBrush, null);
				}
			}
			this.UpdateReflectParent(false);
			if (disposing)
			{
				if (this.GetState(4096))
				{
					return;
				}
				if (this.GetState(262144))
				{
					throw new InvalidOperationException(SR.GetString("ClosingWhileCreatingHandle", new object[] { "Dispose" }));
				}
				this.SetState(4096, true);
				this.SuspendLayout();
				try
				{
					this.DisposeAxControls();
					ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
					if (contextMenu != null)
					{
						contextMenu.Disposed -= this.DetachContextMenu;
					}
					this.ResetBindings();
					if (this.IsHandleCreated)
					{
						this.DestroyHandle();
					}
					if (this.parent != null)
					{
						this.parent.Controls.Remove(this);
					}
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							Control control = controlCollection[i];
							control.parent = null;
							control.Dispose();
						}
						this.Properties.SetObject(Control.PropControlsCollection, null);
					}
					base.Dispose(disposing);
					return;
				}
				finally
				{
					this.ResumeLayout(false);
					this.SetState(4096, false);
					this.SetState(2048, true);
				}
			}
			if (this.window != null)
			{
				this.window.ForceExitMessageLoop();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0001857C File Offset: 0x0001757C
		internal virtual void DisposeAxControls()
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].DisposeAxControls();
				}
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x000185C0 File Offset: 0x000175C0
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
		{
			int[] array = new int[1];
			int[] array2 = array;
			UnsafeNativeMethods.IOleDropSource oleDropSource = new DropSource(this);
			IDataObject dataObject;
			if (data is IDataObject)
			{
				dataObject = (IDataObject)data;
			}
			else
			{
				DataObject dataObject2;
				if (data is IDataObject)
				{
					dataObject2 = new DataObject((IDataObject)data);
				}
				else
				{
					dataObject2 = new DataObject();
					dataObject2.SetData(data);
				}
				dataObject = dataObject2;
			}
			try
			{
				SafeNativeMethods.DoDragDrop(dataObject, oleDropSource, (int)allowedEffects, array2);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return (DragDropEffects)array2[0];
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00018648 File Offset: 0x00017648
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			if (targetBounds.Width <= 0 || targetBounds.Height <= 0 || targetBounds.X < 0 || targetBounds.Y < 0)
			{
				throw new ArgumentException("targetBounds");
			}
			if (!this.IsHandleCreated)
			{
				this.CreateHandle();
			}
			int num = Math.Min(this.Width, targetBounds.Width);
			int num2 = Math.Min(this.Height, targetBounds.Height);
			Bitmap bitmap2 = new Bitmap(num, num2, bitmap.PixelFormat);
			using (Graphics graphics = Graphics.FromImage(bitmap2))
			{
				IntPtr hdc = graphics.GetHdc();
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 791, hdc, (IntPtr)30);
				using (Graphics graphics2 = Graphics.FromImage(bitmap))
				{
					IntPtr hdc2 = graphics2.GetHdc();
					SafeNativeMethods.BitBlt(new HandleRef(graphics2, hdc2), targetBounds.X, targetBounds.Y, num, num2, new HandleRef(graphics, hdc), 0, 0, 13369376);
					graphics2.ReleaseHdcInternal(hdc2);
				}
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00018790 File Offset: 0x00017790
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object EndInvoke(IAsyncResult asyncResult)
		{
			object retVal;
			using (new Control.MultithreadSafeCallScope())
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				Control.ThreadMethodEntry threadMethodEntry = asyncResult as Control.ThreadMethodEntry;
				if (threadMethodEntry == null)
				{
					throw new ArgumentException(SR.GetString("ControlBadAsyncResult"), "asyncResult");
				}
				if (!asyncResult.IsCompleted)
				{
					Control control = this.FindMarshalingControl();
					int num;
					if (SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(control, control.Handle), out num) == SafeNativeMethods.GetCurrentThreadId())
					{
						control.InvokeMarshaledCallbacks();
					}
					else
					{
						control = threadMethodEntry.marshaler;
						control.WaitForWaitHandle(asyncResult.AsyncWaitHandle);
					}
				}
				if (threadMethodEntry.exception != null)
				{
					throw threadMethodEntry.exception;
				}
				retVal = threadMethodEntry.retVal;
			}
			return retVal;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0001884C File Offset: 0x0001784C
		internal bool EndUpdateInternal()
		{
			return this.EndUpdateInternal(true);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00018855 File Offset: 0x00017855
		internal bool EndUpdateInternal(bool invalidate)
		{
			if (this.updateCount > 0)
			{
				this.updateCount -= 1;
				if (this.updateCount == 0)
				{
					this.SendMessage(11, -1, 0);
					if (invalidate)
					{
						this.Invalidate();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0001888E File Offset: 0x0001788E
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public Form FindForm()
		{
			return this.FindFormInternal();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00018898 File Offset: 0x00017898
		internal Form FindFormInternal()
		{
			Control control = this;
			while (control != null && !(control is Form))
			{
				control = control.ParentInternal;
			}
			return (Form)control;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000188C4 File Offset: 0x000178C4
		private Control FindMarshalingControl()
		{
			Control control2;
			lock (this)
			{
				Control control = this;
				while (control != null && !control.IsHandleCreated)
				{
					Control parentInternal = control.ParentInternal;
					control = parentInternal;
				}
				if (control == null)
				{
					control = this;
				}
				control2 = control;
			}
			return control2;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00018914 File Offset: 0x00017914
		protected bool GetTopLevel()
		{
			return (this.state & 524288) != 0;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00018928 File Offset: 0x00017928
		internal void RaiseCreateHandleEvent(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleCreated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00018958 File Offset: 0x00017958
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseKeyEvent(object key, KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[key];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00018984 File Offset: 0x00017984
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseMouseEvent(object key, MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[key];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x000189AE File Offset: 0x000179AE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool Focus()
		{
			IntSecurity.ModifyFocus.Demand();
			return this.FocusInternal();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000189C0 File Offset: 0x000179C0
		internal virtual bool FocusInternal()
		{
			if (this.CanFocus)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(this, this.Handle));
			}
			if (this.Focused && this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					if (containerControlInternal is ContainerControl)
					{
						((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
					}
					else
					{
						containerControlInternal.ActiveControl = this;
					}
				}
			}
			return this.Focused;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00018A29 File Offset: 0x00017A29
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Control FromChildHandle(IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return Control.FromChildHandleInternal(handle);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00018A3C File Offset: 0x00017A3C
		internal static Control FromChildHandleInternal(IntPtr handle)
		{
			while (handle != IntPtr.Zero)
			{
				Control control = Control.FromHandleInternal(handle);
				if (control != null)
				{
					return control;
				}
				handle = UnsafeNativeMethods.GetAncestor(new HandleRef(null, handle), 1);
			}
			return null;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00018A74 File Offset: 0x00017A74
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Control FromHandle(IntPtr handle)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return Control.FromHandleInternal(handle);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00018A88 File Offset: 0x00017A88
		internal static Control FromHandleInternal(IntPtr handle)
		{
			NativeWindow nativeWindow = NativeWindow.FromHandle(handle);
			while (nativeWindow != null && !(nativeWindow is Control.ControlNativeWindow))
			{
				nativeWindow = nativeWindow.PreviousWindow;
			}
			if (nativeWindow is Control.ControlNativeWindow)
			{
				return ((Control.ControlNativeWindow)nativeWindow).GetControl();
			}
			return null;
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00018AC8 File Offset: 0x00017AC8
		internal Size ApplySizeConstraints(int width, int height)
		{
			return this.ApplyBoundsConstraints(0, 0, width, height).Size;
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00018AE8 File Offset: 0x00017AE8
		internal Size ApplySizeConstraints(Size proposedSize)
		{
			return this.ApplyBoundsConstraints(0, 0, proposedSize.Width, proposedSize.Height).Size;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00018B14 File Offset: 0x00017B14
		internal virtual Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			if (this.MaximumSize != Size.Empty || this.MinimumSize != Size.Empty)
			{
				Size size = LayoutUtils.ConvertZeroToUnbounded(this.MaximumSize);
				Rectangle rectangle = new Rectangle(suggestedX, suggestedY, 0, 0);
				rectangle.Size = LayoutUtils.IntersectSizes(new Size(proposedWidth, proposedHeight), size);
				rectangle.Size = LayoutUtils.UnionSizes(rectangle.Size, this.MinimumSize);
				return rectangle;
			}
			return new Rectangle(suggestedX, suggestedY, proposedWidth, proposedHeight);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00018B98 File Offset: 0x00017B98
		public Control GetChildAtPoint(Point pt, GetChildAtPointSkip skipValue)
		{
			if ((skipValue < GetChildAtPointSkip.None) || skipValue > (GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Disabled | GetChildAtPointSkip.Transparent))
			{
				throw new InvalidEnumArgumentException("skipValue", (int)skipValue, typeof(GetChildAtPointSkip));
			}
			IntPtr intPtr = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, this.Handle), pt.X, pt.Y, (int)skipValue);
			Control control = Control.FromChildHandleInternal(intPtr);
			if (control != null && !this.IsDescendant(control))
			{
				IntSecurity.ControlFromHandleOrLocation.Demand();
			}
			if (control != this)
			{
				return control;
			}
			return null;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00018C0A File Offset: 0x00017C0A
		public Control GetChildAtPoint(Point pt)
		{
			return this.GetChildAtPoint(pt, GetChildAtPointSkip.None);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00018C14 File Offset: 0x00017C14
		public IContainerControl GetContainerControl()
		{
			IntSecurity.GetParent.Demand();
			return this.GetContainerControlInternal();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00018C26 File Offset: 0x00017C26
		private static bool IsFocusManagingContainerControl(Control ctl)
		{
			return (ctl.controlStyle & ControlStyles.ContainerControl) == ControlStyles.ContainerControl && ctl is IContainerControl;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00018C3E File Offset: 0x00017C3E
		internal bool IsUpdating()
		{
			return this.updateCount > 0;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00018C4C File Offset: 0x00017C4C
		internal IContainerControl GetContainerControlInternal()
		{
			Control control = this;
			if (control != null && this.IsContainerControl)
			{
				control = control.ParentInternal;
			}
			while (control != null && !Control.IsFocusManagingContainerControl(control))
			{
				control = control.ParentInternal;
			}
			return (IContainerControl)control;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00018C87 File Offset: 0x00017C87
		private static Control.FontHandleWrapper GetDefaultFontHandleWrapper()
		{
			if (Control.defaultFontHandleWrapper == null)
			{
				Control.defaultFontHandleWrapper = new Control.FontHandleWrapper(Control.DefaultFont);
			}
			return Control.defaultFontHandleWrapper;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00018CA4 File Offset: 0x00017CA4
		internal IntPtr GetHRgn(Region region)
		{
			Graphics graphics = this.CreateGraphicsInternal();
			IntPtr hrgn = region.GetHrgn(graphics);
			global::System.Internal.HandleCollector.Add(hrgn, NativeMethods.CommonHandles.GDI);
			graphics.Dispose();
			return hrgn;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00018CD4 File Offset: 0x00017CD4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
			CreateParams createParams = this.CreateParams;
			SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			float num = factor.Width;
			float num2 = factor.Height;
			int num3 = bounds.X;
			int num4 = bounds.Y;
			bool flag = !this.GetState(524288);
			if (flag)
			{
				ISite site = this.Site;
				if (site != null && site.DesignMode)
				{
					IDesignerHost designerHost = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null && designerHost.RootComponent == this)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
				{
					num3 = (int)Math.Round((double)((float)bounds.X * num));
				}
				if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
				{
					num4 = (int)Math.Round((double)((float)bounds.Y * num2));
				}
			}
			int num5 = bounds.Width;
			int num6 = bounds.Height;
			if ((this.controlStyle & ControlStyles.FixedWidth) != ControlStyles.FixedWidth && (specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				int num7 = rect.right - rect.left;
				int num8 = bounds.Width - num7;
				num5 = (int)Math.Round((double)((float)num8 * num)) + num7;
			}
			if ((this.controlStyle & ControlStyles.FixedHeight) != ControlStyles.FixedHeight && (specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				int num9 = rect.bottom - rect.top;
				int num10 = bounds.Height - num9;
				num6 = (int)Math.Round((double)((float)num10 * num2)) + num9;
			}
			return new Rectangle(num3, num4, num5, num6);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00018E5C File Offset: 0x00017E5C
		private MouseButtons GetXButton(int wparam)
		{
			switch (wparam)
			{
			case 1:
				return MouseButtons.XButton1;
			case 2:
				return MouseButtons.XButton2;
			default:
				return MouseButtons.None;
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00018E8A File Offset: 0x00017E8A
		internal virtual bool GetVisibleCore()
		{
			return this.GetState(2) && (this.ParentInternal == null || this.ParentInternal.GetVisibleCore());
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00018EAC File Offset: 0x00017EAC
		internal bool GetAnyDisposingInHierarchy()
		{
			Control control = this;
			bool flag = false;
			while (control != null)
			{
				if (control.Disposing)
				{
					flag = true;
					break;
				}
				control = control.parent;
			}
			return flag;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00018ED8 File Offset: 0x00017ED8
		private MenuItem GetMenuItemFromHandleId(IntPtr hmenu, int item)
		{
			MenuItem menuItem = null;
			int menuItemID = UnsafeNativeMethods.GetMenuItemID(new HandleRef(null, hmenu), item);
			if (menuItemID == -1)
			{
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.GetSubMenu(new HandleRef(null, hmenu), item);
				int menuItemCount = UnsafeNativeMethods.GetMenuItemCount(new HandleRef(null, intPtr));
				MenuItem menuItem2 = null;
				for (int i = 0; i < menuItemCount; i++)
				{
					menuItem2 = this.GetMenuItemFromHandleId(intPtr, i);
					if (menuItem2 != null)
					{
						Menu menu = menuItem2.Parent;
						if (menu != null && menu is MenuItem)
						{
							menuItem2 = (MenuItem)menu;
							break;
						}
						menuItem2 = null;
					}
				}
				menuItem = menuItem2;
			}
			else
			{
				Command commandFromID = Command.GetCommandFromID(menuItemID);
				if (commandFromID != null)
				{
					object target = commandFromID.Target;
					if (target != null && target is MenuItem.MenuItemData)
					{
						menuItem = ((MenuItem.MenuItemData)target).baseItem;
					}
				}
			}
			return menuItem;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00018F98 File Offset: 0x00017F98
		private ArrayList GetChildControlsTabOrderList(bool handleCreatedOnly)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				if (!handleCreatedOnly || control.IsHandleCreated)
				{
					arrayList.Add(new Control.ControlTabOrderHolder(arrayList.Count, control.TabIndex, control));
				}
			}
			arrayList.Sort(new Control.ControlTabOrderComparer());
			return arrayList;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00019020 File Offset: 0x00018020
		private int[] GetChildWindowsInTabOrder()
		{
			ArrayList childWindowsTabOrderList = this.GetChildWindowsTabOrderList();
			int[] array = new int[childWindowsTabOrderList.Count];
			for (int i = 0; i < childWindowsTabOrderList.Count; i++)
			{
				array[i] = ((Control.ControlTabOrderHolder)childWindowsTabOrderList[i]).oldOrder;
			}
			return array;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00019068 File Offset: 0x00018068
		internal Control[] GetChildControlsInTabOrder(bool handleCreatedOnly)
		{
			ArrayList childControlsTabOrderList = this.GetChildControlsTabOrderList(handleCreatedOnly);
			Control[] array = new Control[childControlsTabOrderList.Count];
			for (int i = 0; i < childControlsTabOrderList.Count; i++)
			{
				array[i] = ((Control.ControlTabOrderHolder)childControlsTabOrderList[i]).control;
			}
			return array;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000190B0 File Offset: 0x000180B0
		private static ArrayList GetChildWindows(IntPtr hWndParent)
		{
			ArrayList arrayList = new ArrayList();
			IntPtr intPtr = UnsafeNativeMethods.GetWindow(new HandleRef(null, hWndParent), 5);
			while (intPtr != IntPtr.Zero)
			{
				arrayList.Add(intPtr);
				intPtr = UnsafeNativeMethods.GetWindow(new HandleRef(null, intPtr), 2);
			}
			return arrayList;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000190FC File Offset: 0x000180FC
		private ArrayList GetChildWindowsTabOrderList()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList childWindows = Control.GetChildWindows(this.Handle);
			foreach (object obj in childWindows)
			{
				IntPtr intPtr = (IntPtr)obj;
				Control control = Control.FromHandleInternal(intPtr);
				int num = ((control == null) ? (-1) : control.TabIndex);
				arrayList.Add(new Control.ControlTabOrderHolder(arrayList.Count, num, control));
			}
			arrayList.Sort(new Control.ControlTabOrderComparer());
			return arrayList;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00019198 File Offset: 0x00018198
		internal virtual Control GetFirstChildControlInTabOrder(bool forward)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			Control control = null;
			if (controlCollection != null)
			{
				if (forward)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						if (control == null || control.tabIndex > controlCollection[i].tabIndex)
						{
							control = controlCollection[i];
						}
					}
				}
				else
				{
					for (int j = controlCollection.Count - 1; j >= 0; j--)
					{
						if (control == null || control.tabIndex < controlCollection[j].tabIndex)
						{
							control = controlCollection[j];
						}
					}
				}
			}
			return control;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00019228 File Offset: 0x00018228
		public Control GetNextControl(Control ctl, bool forward)
		{
			if (!this.Contains(ctl))
			{
				ctl = this;
			}
			if (forward)
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null && controlCollection.Count > 0 && (ctl == this || !Control.IsFocusManagingContainerControl(ctl)))
				{
					Control firstChildControlInTabOrder = ctl.GetFirstChildControlInTabOrder(true);
					if (firstChildControlInTabOrder != null)
					{
						return firstChildControlInTabOrder;
					}
				}
				while (ctl != this)
				{
					int num = ctl.tabIndex;
					bool flag = false;
					Control control = null;
					Control control2 = ctl.parent;
					int num2 = 0;
					Control.ControlCollection controlCollection2 = (Control.ControlCollection)control2.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection2 != null)
					{
						num2 = controlCollection2.Count;
					}
					for (int i = 0; i < num2; i++)
					{
						if (controlCollection2[i] != ctl)
						{
							if (controlCollection2[i].tabIndex >= num && (control == null || control.tabIndex > controlCollection2[i].tabIndex) && (controlCollection2[i].tabIndex != num || flag))
							{
								control = controlCollection2[i];
							}
						}
						else
						{
							flag = true;
						}
					}
					if (control != null)
					{
						return control;
					}
					ctl = ctl.parent;
				}
			}
			else
			{
				if (ctl != this)
				{
					int num3 = ctl.tabIndex;
					bool flag2 = false;
					Control control3 = null;
					Control control4 = ctl.parent;
					int num4 = 0;
					Control.ControlCollection controlCollection3 = (Control.ControlCollection)control4.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection3 != null)
					{
						num4 = controlCollection3.Count;
					}
					for (int j = num4 - 1; j >= 0; j--)
					{
						if (controlCollection3[j] != ctl)
						{
							if (controlCollection3[j].tabIndex <= num3 && (control3 == null || control3.tabIndex < controlCollection3[j].tabIndex) && (controlCollection3[j].tabIndex != num3 || flag2))
							{
								control3 = controlCollection3[j];
							}
						}
						else
						{
							flag2 = true;
						}
					}
					if (control3 != null)
					{
						ctl = control3;
					}
					else
					{
						if (control4 == this)
						{
							return null;
						}
						return control4;
					}
				}
				Control.ControlCollection controlCollection4 = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				while (controlCollection4 != null && controlCollection4.Count > 0 && (ctl == this || !Control.IsFocusManagingContainerControl(ctl)))
				{
					Control firstChildControlInTabOrder2 = ctl.GetFirstChildControlInTabOrder(false);
					if (firstChildControlInTabOrder2 == null)
					{
						break;
					}
					ctl = firstChildControlInTabOrder2;
					controlCollection4 = (Control.ControlCollection)ctl.Properties.GetObject(Control.PropControlsCollection);
				}
			}
			if (ctl != this)
			{
				return ctl;
			}
			return null;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00019480 File Offset: 0x00018480
		internal static IntPtr GetSafeHandle(IWin32Window window)
		{
			IntPtr intPtr = IntPtr.Zero;
			Control control = window as Control;
			if (control != null)
			{
				return control.Handle;
			}
			IntSecurity.AllWindows.Demand();
			intPtr = window.Handle;
			if (intPtr == IntPtr.Zero || UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
			{
				return intPtr;
			}
			throw new Win32Exception(6);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x000194DA File Offset: 0x000184DA
		internal bool GetState(int flag)
		{
			return (this.state & flag) != 0;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x000194EA File Offset: 0x000184EA
		private bool GetState2(int flag)
		{
			return (this.state2 & flag) != 0;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x000194FA File Offset: 0x000184FA
		protected bool GetStyle(ControlStyles flag)
		{
			return (this.controlStyle & flag) == flag;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00019507 File Offset: 0x00018507
		public void Hide()
		{
			this.Visible = false;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00019510 File Offset: 0x00018510
		private void HookMouseEvent()
		{
			if (!this.GetState(16384))
			{
				this.SetState(16384, true);
				if (this.trackMouseEvent == null)
				{
					this.trackMouseEvent = new NativeMethods.TRACKMOUSEEVENT();
					this.trackMouseEvent.dwFlags = 3;
					this.trackMouseEvent.hwndTrack = this.Handle;
				}
				SafeNativeMethods.TrackMouseEvent(this.trackMouseEvent);
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00019572 File Offset: 0x00018572
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void InitLayout()
		{
			this.LayoutEngine.InitLayout(this, BoundsSpecified.All);
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00019582 File Offset: 0x00018582
		private void InitScaling(BoundsSpecified specified)
		{
			this.requiredScaling |= (byte)(specified & BoundsSpecified.All);
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00019598 File Offset: 0x00018598
		internal virtual IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (!this.GetStyle(ControlStyles.UserPaint))
			{
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.ForeColor));
				SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.BackColor));
				return this.BackColorBrush;
			}
			return UnsafeNativeMethods.GetStockObject(5);
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x000195EC File Offset: 0x000185EC
		private void InitMouseWheelSupport()
		{
			if (!Control.mouseWheelInit)
			{
				Control.mouseWheelRoutingNeeded = !SystemInformation.NativeMouseWheelSupport;
				if (Control.mouseWheelRoutingNeeded)
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
					if (intPtr != IntPtr.Zero)
					{
						int num = SafeNativeMethods.RegisterWindowMessage("MSWHEEL_ROLLMSG");
						if (num != 0)
						{
							Control.mouseWheelMessage = num;
						}
					}
				}
				Control.mouseWheelInit = true;
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00019651 File Offset: 0x00018651
		public void Invalidate(Region region)
		{
			this.Invalidate(region, false);
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0001965C File Offset: 0x0001865C
		public void Invalidate(Region region, bool invalidateChildren)
		{
			if (region == null)
			{
				this.Invalidate(invalidateChildren);
				return;
			}
			if (this.IsHandleCreated)
			{
				IntPtr hrgn = this.GetHRgn(region);
				try
				{
					if (invalidateChildren)
					{
						SafeNativeMethods.RedrawWindow(new HandleRef(this, this.Handle), null, new HandleRef(region, hrgn), 133);
					}
					else
					{
						using (new Control.MultithreadSafeCallScope())
						{
							SafeNativeMethods.InvalidateRgn(new HandleRef(this, this.Handle), new HandleRef(region, hrgn), !this.GetStyle(ControlStyles.Opaque));
						}
					}
				}
				finally
				{
					SafeNativeMethods.DeleteObject(new HandleRef(region, hrgn));
				}
				Rectangle rectangle = Rectangle.Empty;
				using (Graphics graphics = this.CreateGraphicsInternal())
				{
					rectangle = Rectangle.Ceiling(region.GetBounds(graphics));
				}
				this.OnInvalidated(new InvalidateEventArgs(rectangle));
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0001974C File Offset: 0x0001874C
		public void Invalidate()
		{
			this.Invalidate(false);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00019758 File Offset: 0x00018758
		public void Invalidate(bool invalidateChildren)
		{
			if (this.IsHandleCreated)
			{
				if (invalidateChildren)
				{
					SafeNativeMethods.RedrawWindow(new HandleRef(this.window, this.Handle), null, NativeMethods.NullHandleRef, 133);
				}
				else
				{
					using (new Control.MultithreadSafeCallScope())
					{
						SafeNativeMethods.InvalidateRect(new HandleRef(this.window, this.Handle), null, (this.controlStyle & ControlStyles.Opaque) != ControlStyles.Opaque);
					}
				}
				this.NotifyInvalidate(this.ClientRectangle);
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x000197E8 File Offset: 0x000187E8
		public void Invalidate(Rectangle rc)
		{
			this.Invalidate(rc, false);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x000197F4 File Offset: 0x000187F4
		public void Invalidate(Rectangle rc, bool invalidateChildren)
		{
			if (rc.IsEmpty)
			{
				this.Invalidate(invalidateChildren);
				return;
			}
			if (this.IsHandleCreated)
			{
				if (invalidateChildren)
				{
					NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(rc.X, rc.Y, rc.Width, rc.Height);
					SafeNativeMethods.RedrawWindow(new HandleRef(this.window, this.Handle), ref rect, NativeMethods.NullHandleRef, 133);
				}
				else
				{
					NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(rc.X, rc.Y, rc.Width, rc.Height);
					using (new Control.MultithreadSafeCallScope())
					{
						SafeNativeMethods.InvalidateRect(new HandleRef(this.window, this.Handle), ref rect2, (this.controlStyle & ControlStyles.Opaque) != ControlStyles.Opaque);
					}
				}
				this.NotifyInvalidate(rc);
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x000198DC File Offset: 0x000188DC
		public object Invoke(Delegate method)
		{
			return this.Invoke(method, null);
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x000198E8 File Offset: 0x000188E8
		public object Invoke(Delegate method, params object[] args)
		{
			object obj;
			using (new Control.MultithreadSafeCallScope())
			{
				Control control = this.FindMarshalingControl();
				obj = control.MarshaledInvoke(this, method, args, true);
			}
			return obj;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0001992C File Offset: 0x0001892C
		private void InvokeMarshaledCallback(Control.ThreadMethodEntry tme)
		{
			if (tme.executionContext != null)
			{
				if (Control.invokeMarshaledCallbackHelperDelegate == null)
				{
					Control.invokeMarshaledCallbackHelperDelegate = new ContextCallback(Control.InvokeMarshaledCallbackHelper);
				}
				if (SynchronizationContext.Current == null)
				{
					WindowsFormsSynchronizationContext.InstallIfNeeded();
				}
				tme.syncContext = SynchronizationContext.Current;
				ExecutionContext.Run(tme.executionContext, Control.invokeMarshaledCallbackHelperDelegate, tme);
				return;
			}
			Control.InvokeMarshaledCallbackHelper(tme);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00019988 File Offset: 0x00018988
		private static void InvokeMarshaledCallbackHelper(object obj)
		{
			Control.ThreadMethodEntry threadMethodEntry = (Control.ThreadMethodEntry)obj;
			if (threadMethodEntry.syncContext != null)
			{
				SynchronizationContext synchronizationContext = SynchronizationContext.Current;
				try
				{
					SynchronizationContext.SetSynchronizationContext(threadMethodEntry.syncContext);
					Control.InvokeMarshaledCallbackDo(threadMethodEntry);
					return;
				}
				finally
				{
					SynchronizationContext.SetSynchronizationContext(synchronizationContext);
				}
			}
			Control.InvokeMarshaledCallbackDo(threadMethodEntry);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000199DC File Offset: 0x000189DC
		private static void InvokeMarshaledCallbackDo(Control.ThreadMethodEntry tme)
		{
			if (tme.method is EventHandler)
			{
				if (tme.args == null || tme.args.Length < 1)
				{
					((EventHandler)tme.method)(tme.caller, EventArgs.Empty);
					return;
				}
				if (tme.args.Length < 2)
				{
					((EventHandler)tme.method)(tme.args[0], EventArgs.Empty);
					return;
				}
				((EventHandler)tme.method)(tme.args[0], (EventArgs)tme.args[1]);
				return;
			}
			else
			{
				if (tme.method is MethodInvoker)
				{
					((MethodInvoker)tme.method)();
					return;
				}
				if (tme.method is WaitCallback)
				{
					((WaitCallback)tme.method)(tme.args[0]);
					return;
				}
				tme.retVal = tme.method.DynamicInvoke(tme.args);
				return;
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00019AD0 File Offset: 0x00018AD0
		private void InvokeMarshaledCallbacks()
		{
			Control.ThreadMethodEntry threadMethodEntry = null;
			lock (this.threadCallbackList)
			{
				if (this.threadCallbackList.Count > 0)
				{
					threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
				}
				goto IL_00D6;
			}
			IL_003A:
			if (threadMethodEntry.method != null)
			{
				try
				{
					if (NativeWindow.WndProcShouldBeDebuggable && !threadMethodEntry.synchronous)
					{
						this.InvokeMarshaledCallback(threadMethodEntry);
					}
					else
					{
						try
						{
							this.InvokeMarshaledCallback(threadMethodEntry);
						}
						catch (Exception ex)
						{
							threadMethodEntry.exception = ex.GetBaseException();
						}
					}
				}
				finally
				{
					threadMethodEntry.Complete();
					if (!NativeWindow.WndProcShouldBeDebuggable && threadMethodEntry.exception != null && !threadMethodEntry.synchronous)
					{
						Application.OnThreadException(threadMethodEntry.exception);
					}
				}
			}
			lock (this.threadCallbackList)
			{
				if (this.threadCallbackList.Count > 0)
				{
					threadMethodEntry = (Control.ThreadMethodEntry)this.threadCallbackList.Dequeue();
				}
				else
				{
					threadMethodEntry = null;
				}
			}
			IL_00D6:
			if (threadMethodEntry == null)
			{
				return;
			}
			goto IL_003A;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00019BF0 File Offset: 0x00018BF0
		protected void InvokePaint(Control c, PaintEventArgs e)
		{
			c.OnPaint(e);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00019BF9 File Offset: 0x00018BF9
		protected void InvokePaintBackground(Control c, PaintEventArgs e)
		{
			c.OnPaintBackground(e);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00019C04 File Offset: 0x00018C04
		internal bool IsFontSet()
		{
			return (Font)this.Properties.GetObject(Control.PropFont) != null;
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00019C30 File Offset: 0x00018C30
		internal bool IsDescendant(Control descendant)
		{
			for (Control control = descendant; control != null; control = control.ParentInternal)
			{
				if (control == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00019C54 File Offset: 0x00018C54
		public static bool IsKeyLocked(Keys keyVal)
		{
			if (keyVal != Keys.Insert && keyVal != Keys.NumLock && keyVal != Keys.Capital && keyVal != Keys.Scroll)
			{
				throw new NotSupportedException(SR.GetString("ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly"));
			}
			int keyState = (int)UnsafeNativeMethods.GetKeyState((int)keyVal);
			if (keyVal == Keys.Insert || keyVal == Keys.Capital)
			{
				return (keyState & 1) != 0;
			}
			return (keyState & 32769) != 0;
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00019CB4 File Offset: 0x00018CB4
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool IsInputChar(char charCode)
		{
			int num;
			if (charCode == '\t')
			{
				num = 134;
			}
			else
			{
				num = 132;
			}
			return ((int)this.SendMessage(135, 0, 0) & num) != 0;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00019CF0 File Offset: 0x00018CF0
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			int num = 4;
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Tab)
			{
				switch (keys)
				{
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
					num = 5;
					break;
				}
			}
			else
			{
				num = 6;
			}
			return this.IsHandleCreated && ((int)this.SendMessage(135, 0, 0) & num) != 0;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00019D64 File Offset: 0x00018D64
		public static bool IsMnemonic(char charCode, string text)
		{
			if (charCode == '&')
			{
				return false;
			}
			if (text != null)
			{
				int num = -1;
				char c = char.ToUpper(charCode, CultureInfo.CurrentCulture);
				while (num + 1 < text.Length)
				{
					num = text.IndexOf('&', num + 1) + 1;
					if (num <= 0 || num >= text.Length)
					{
						break;
					}
					char c2 = char.ToUpper(text[num], CultureInfo.CurrentCulture);
					if (c2 == c || char.ToLower(c2, CultureInfo.CurrentCulture) == char.ToLower(c, CultureInfo.CurrentCulture))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00019DE0 File Offset: 0x00018DE0
		private void ListenToUserPreferenceChanged(bool listen)
		{
			if (this.GetState2(4))
			{
				if (!listen)
				{
					this.SetState2(4, false);
					SystemEvents.UserPreferenceChanged -= this.UserPreferenceChanged;
					return;
				}
			}
			else if (listen)
			{
				this.SetState2(4, true);
				SystemEvents.UserPreferenceChanged += this.UserPreferenceChanged;
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00019E30 File Offset: 0x00018E30
		private object MarshaledInvoke(Control caller, Delegate method, object[] args, bool synchronous)
		{
			if (!this.IsHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("ErrorNoMarshalingThread"));
			}
			Control.ActiveXImpl activeXImpl = (Control.ActiveXImpl)this.Properties.GetObject(Control.PropActiveXImpl);
			if (activeXImpl != null)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			bool flag = false;
			int num;
			if (SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, this.Handle), out num) == SafeNativeMethods.GetCurrentThreadId() && synchronous)
			{
				flag = true;
			}
			ExecutionContext executionContext = null;
			if (!flag)
			{
				executionContext = ExecutionContext.Capture();
			}
			Control.ThreadMethodEntry threadMethodEntry = new Control.ThreadMethodEntry(caller, this, method, args, synchronous, executionContext);
			lock (this)
			{
				if (this.threadCallbackList == null)
				{
					this.threadCallbackList = new Queue();
				}
			}
			lock (this.threadCallbackList)
			{
				if (Control.threadCallbackMessage == 0)
				{
					Control.threadCallbackMessage = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_ThreadCallbackMessage");
				}
				this.threadCallbackList.Enqueue(threadMethodEntry);
			}
			if (flag)
			{
				this.InvokeMarshaledCallbacks();
			}
			else
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, this.Handle), Control.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
			}
			if (!synchronous)
			{
				return threadMethodEntry;
			}
			if (!threadMethodEntry.IsCompleted)
			{
				this.WaitForWaitHandle(threadMethodEntry.AsyncWaitHandle);
			}
			if (threadMethodEntry.exception != null)
			{
				throw threadMethodEntry.exception;
			}
			return threadMethodEntry.retVal;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00019F9C File Offset: 0x00018F9C
		private void MarshalStringToMessage(string value, ref Message m)
		{
			if (m.LParam == IntPtr.Zero)
			{
				m.Result = (IntPtr)((value.Length + 1) * Marshal.SystemDefaultCharSize);
				return;
			}
			if ((int)(long)m.WParam < value.Length + 1)
			{
				m.Result = (IntPtr)(-1);
				return;
			}
			char[] array = new char[1];
			char[] array2 = array;
			byte[] array3;
			byte[] array4;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				array3 = Encoding.Default.GetBytes(value);
				array4 = Encoding.Default.GetBytes(array2);
			}
			else
			{
				array3 = Encoding.Unicode.GetBytes(value);
				array4 = Encoding.Unicode.GetBytes(array2);
			}
			Marshal.Copy(array3, 0, m.LParam, array3.Length);
			Marshal.Copy(array4, 0, (IntPtr)((long)m.LParam + (long)array3.Length), array4.Length);
			m.Result = (IntPtr)((array3.Length + array4.Length) / Marshal.SystemDefaultCharSize);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0001A082 File Offset: 0x00019082
		internal void NotifyEnter()
		{
			this.OnEnter(EventArgs.Empty);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0001A08F File Offset: 0x0001908F
		internal void NotifyLeave()
		{
			this.OnLeave(EventArgs.Empty);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0001A09C File Offset: 0x0001909C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void NotifyInvalidate(Rectangle invalidatedArea)
		{
			this.OnInvalidated(new InvalidateEventArgs(invalidatedArea));
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0001A0AC File Offset: 0x000190AC
		private bool NotifyValidating()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			this.OnValidating(cancelEventArgs);
			return cancelEventArgs.Cancel;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0001A0CC File Offset: 0x000190CC
		private void NotifyValidated()
		{
			this.OnValidated(EventArgs.Empty);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0001A0D9 File Offset: 0x000190D9
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeOnClick(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnClick(e);
			}
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0001A0E8 File Offset: 0x000190E8
		protected virtual void OnAutoSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventAutoSizeChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0001A118 File Offset: 0x00019118
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			object @object = this.Properties.GetObject(Control.PropBackBrush);
			if (@object != null)
			{
				if (this.GetState(2097152))
				{
					IntPtr intPtr = (IntPtr)@object;
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
					}
				}
				this.Properties.SetObject(Control.PropBackBrush, null);
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBackColorChanged(e);
				}
			}
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0001A1E4 File Offset: 0x000191E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackgroundImageChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackgroundImage] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBackgroundImageChanged(e);
				}
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0001A258 File Offset: 0x00019258
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackgroundImageLayoutChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventBackgroundImageLayout] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0001A298 File Offset: 0x00019298
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBindingContextChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropBindings) != null)
			{
				this.UpdateBindings();
			}
			EventHandler eventHandler = base.Events[Control.EventBindingContext] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentBindingContextChanged(e);
				}
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0001A318 File Offset: 0x00019318
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCausesValidationChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventCausesValidation] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0001A346 File Offset: 0x00019346
		internal virtual void OnChildLayoutResuming(Control child, bool performLayout)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.OnChildLayoutResuming(child, performLayout);
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0001A360 File Offset: 0x00019360
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnContextMenuChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventContextMenu] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0001A390 File Offset: 0x00019390
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnContextMenuStripChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventContextMenuStrip] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0001A3C0 File Offset: 0x000193C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCursorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventCursor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentCursorChanged(e);
				}
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0001A428 File Offset: 0x00019428
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDockChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventDock] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0001A458 File Offset: 0x00019458
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.IsHandleCreated)
			{
				SafeNativeMethods.EnableWindow(new HandleRef(this, this.Handle), this.Enabled);
				if (this.GetStyle(ControlStyles.UserPaint))
				{
					this.Invalidate();
					this.Update();
				}
			}
			EventHandler eventHandler = base.Events[Control.EventEnabled] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentEnabledChanged(e);
				}
			}
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0001A4FB File Offset: 0x000194FB
		internal virtual void OnFrameWindowActivate(bool fActivate)
		{
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0001A500 File Offset: 0x00019500
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFontChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			if (this.Properties.ContainsInteger(Control.PropFontHeight))
			{
				this.Properties.SetInteger(Control.PropFontHeight, -1);
			}
			this.DisposeFontHandle();
			if (this.IsHandleCreated && !this.GetStyle(ControlStyles.UserPaint))
			{
				this.SetWindowFont();
			}
			EventHandler eventHandler = base.Events[Control.EventFont] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			using (new LayoutTransaction(this, this, PropertyNames.Font, false))
			{
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						controlCollection[i].OnParentFontChanged(e);
					}
				}
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Font);
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0001A5EC File Offset: 0x000195EC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.Invalidate();
			EventHandler eventHandler = base.Events[Control.EventForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentForeColorChanged(e);
				}
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0001A660 File Offset: 0x00019660
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			this.SetState2(2, true);
			this.RecreateHandle();
			EventHandler eventHandler = base.Events[Control.EventRightToLeft] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentRightToLeftChanged(e);
				}
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0001A6DC File Offset: 0x000196DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnNotifyMessage(Message m)
		{
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0001A6E0 File Offset: 0x000196E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(Control.PropBackColor).IsEmpty)
			{
				this.OnBackColorChanged(e);
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0001A70E File Offset: 0x0001970E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackgroundImageChanged(EventArgs e)
		{
			this.OnBackgroundImageChanged(e);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0001A717 File Offset: 0x00019717
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBindingContextChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropBindingManager) == null)
			{
				this.OnBindingContextChanged(e);
			}
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0001A732 File Offset: 0x00019732
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentCursorChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropCursor) == null)
			{
				this.OnCursorChanged(e);
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0001A74D File Offset: 0x0001974D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentEnabledChanged(EventArgs e)
		{
			if (this.GetState(4))
			{
				this.OnEnabledChanged(e);
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0001A75F File Offset: 0x0001975F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentFontChanged(EventArgs e)
		{
			if (this.Properties.GetObject(Control.PropFont) == null)
			{
				this.OnFontChanged(e);
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0001A77C File Offset: 0x0001977C
		internal virtual void OnParentHandleRecreated()
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null && this.IsHandleCreated)
			{
				UnsafeNativeMethods.SetParent(new HandleRef(this, this.Handle), new HandleRef(parentInternal, parentInternal.Handle));
				this.UpdateZOrder();
			}
			this.SetState(536870912, false);
			if (this.ReflectParent == this.ParentInternal)
			{
				this.RecreateHandle();
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0001A7DF File Offset: 0x000197DF
		internal virtual void OnParentHandleRecreating()
		{
			this.SetState(536870912, true);
			if (this.IsHandleCreated)
			{
				Application.ParkHandle(new HandleRef(this, this.Handle));
			}
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0001A808 File Offset: 0x00019808
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentForeColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(Control.PropForeColor).IsEmpty)
			{
				this.OnForeColorChanged(e);
			}
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0001A836 File Offset: 0x00019836
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentRightToLeftChanged(EventArgs e)
		{
			if (!this.Properties.ContainsInteger(Control.PropRightToLeft) || this.Properties.GetInteger(Control.PropRightToLeft) == 2)
			{
				this.OnRightToLeftChanged(e);
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0001A864 File Offset: 0x00019864
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentVisibleChanged(EventArgs e)
		{
			if (this.GetState(2))
			{
				this.OnVisibleChanged(e);
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0001A878 File Offset: 0x00019878
		internal virtual void OnParentBecameInvisible()
		{
			if (this.GetState(2))
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						Control control = controlCollection[i];
						control.OnParentBecameInvisible();
					}
				}
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0001A8C8 File Offset: 0x000198C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPrint(PaintEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				this.PaintWithErrorHandling(e, 1);
				e.ResetGraphics();
				this.PaintWithErrorHandling(e, 2);
				return;
			}
			Control.PrintPaintEventArgs printPaintEventArgs = e as Control.PrintPaintEventArgs;
			bool flag = false;
			IntPtr intPtr = IntPtr.Zero;
			Message message;
			if (printPaintEventArgs == null)
			{
				IntPtr intPtr2 = (IntPtr)30;
				intPtr = e.HDC;
				if (intPtr == IntPtr.Zero)
				{
					intPtr = e.Graphics.GetHdc();
					flag = true;
				}
				message = Message.Create(this.Handle, 792, intPtr, intPtr2);
			}
			else
			{
				message = printPaintEventArgs.Message;
			}
			try
			{
				this.DefWndProc(ref message);
			}
			finally
			{
				if (flag)
				{
					e.Graphics.ReleaseHdcInternal(intPtr);
				}
			}
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0001A988 File Offset: 0x00019988
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTabIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventTabIndex] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0001A9B8 File Offset: 0x000199B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTabStopChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventTabStop] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0001A9E8 File Offset: 0x000199E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0001AA18 File Offset: 0x00019A18
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnVisibleChanged(EventArgs e)
		{
			bool visible = this.Visible;
			if (visible)
			{
				this.UnhookMouseEvent();
				this.trackMouseEvent = null;
			}
			if (this.parent != null && visible && !this.Created && !this.GetAnyDisposingInHierarchy())
			{
				this.CreateControl();
			}
			EventHandler eventHandler = base.Events[Control.EventVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					Control control = controlCollection[i];
					if (control.Visible)
					{
						control.OnParentVisibleChanged(e);
					}
					if (!visible)
					{
						control.OnParentBecameInvisible();
					}
				}
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0001AAD4 File Offset: 0x00019AD4
		internal virtual void OnTopMostActiveXParentChanged(EventArgs e)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnTopMostActiveXParentChanged(e);
				}
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0001AB18 File Offset: 0x00019B18
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventParent] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.TopMostParent.IsActiveX)
			{
				this.OnTopMostActiveXParentChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0001AB60 File Offset: 0x00019B60
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0001AB90 File Offset: 0x00019B90
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClientSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventClientSize] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0001ABC0 File Offset: 0x00019BC0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnControlAdded(ControlEventArgs e)
		{
			ControlEventHandler controlEventHandler = (ControlEventHandler)base.Events[Control.EventControlAdded];
			if (controlEventHandler != null)
			{
				controlEventHandler(this, e);
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0001ABF0 File Offset: 0x00019BF0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnControlRemoved(ControlEventArgs e)
		{
			ControlEventHandler controlEventHandler = (ControlEventHandler)base.Events[Control.EventControlRemoved];
			if (controlEventHandler != null)
			{
				controlEventHandler(this, e);
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0001AC1E File Offset: 0x00019C1E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCreateControl()
		{
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0001AC20 File Offset: 0x00019C20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHandleCreated(EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				if (!this.GetStyle(ControlStyles.UserPaint))
				{
					this.SetWindowFont();
				}
				this.SetAcceptDrops(this.AllowDrop);
				Region region = (Region)this.Properties.GetObject(Control.PropRegion);
				if (region != null)
				{
					IntPtr intPtr = this.GetHRgn(region);
					try
					{
						if (this.IsActiveX)
						{
							intPtr = this.ActiveXMergeRegion(intPtr);
						}
						if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, this.Handle), new HandleRef(this, intPtr), SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle))) != 0)
						{
							intPtr = IntPtr.Zero;
						}
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						}
					}
				}
				Control.ControlAccessibleObject controlAccessibleObject = this.Properties.GetObject(Control.PropAccessibility) as Control.ControlAccessibleObject;
				Control.ControlAccessibleObject controlAccessibleObject2 = this.Properties.GetObject(Control.PropNcAccessibility) as Control.ControlAccessibleObject;
				IntPtr handle = this.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (controlAccessibleObject != null)
					{
						controlAccessibleObject.Handle = handle;
					}
					if (controlAccessibleObject2 != null)
					{
						controlAccessibleObject2.Handle = handle;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (this.text != null && this.text.Length != 0)
				{
					UnsafeNativeMethods.SetWindowText(new HandleRef(this, this.Handle), this.text);
				}
				if (!(this is ScrollableControl) && !this.IsMirrored && this.GetState2(2) && !this.GetState2(1))
				{
					this.BeginInvoke(new EventHandler(this.OnSetScrollPosition));
					this.SetState2(1, true);
					this.SetState2(2, false);
				}
				if (this.GetState2(8))
				{
					this.ListenToUserPreferenceChanged(this.GetTopLevel());
				}
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleCreated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.IsHandleCreated && this.GetState(32768))
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, this.Handle), Control.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
				this.SetState(32768, false);
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0001AE38 File Offset: 0x00019E38
		private void OnSetScrollPosition(object sender, EventArgs e)
		{
			this.SetState2(1, false);
			this.OnInvokedSetScrollPosition(sender, e);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0001AE4C File Offset: 0x00019E4C
		internal virtual void OnInvokedSetScrollPosition(object sender, EventArgs e)
		{
			if (!(this is ScrollableControl) && !this.IsMirrored)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 1;
				if (UnsafeNativeMethods.GetScrollInfo(new HandleRef(this, this.Handle), 0, scrollinfo))
				{
					scrollinfo.nPos = ((this.RightToLeft == RightToLeft.Yes) ? scrollinfo.nMax : scrollinfo.nMin);
					this.SendMessage(276, NativeMethods.Util.MAKELPARAM(4, scrollinfo.nPos), 0);
				}
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0001AED8 File Offset: 0x00019ED8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.OnMove(EventArgs.Empty);
			EventHandler eventHandler = base.Events[Control.EventLocation] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0001AF14 File Offset: 0x00019F14
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHandleDestroyed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventHandleDestroyed];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			this.UpdateReflectParent(false);
			if (!this.RecreatingHandle)
			{
				if (this.GetState(2097152))
				{
					object @object = this.Properties.GetObject(Control.PropBackBrush);
					if (@object != null)
					{
						IntPtr intPtr = (IntPtr)@object;
						if (intPtr != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
						}
						this.Properties.SetObject(Control.PropBackBrush, null);
					}
				}
				this.ListenToUserPreferenceChanged(false);
			}
			try
			{
				if (!this.GetAnyDisposingInHierarchy())
				{
					this.text = this.Text;
					if (this.text != null && this.text.Length == 0)
					{
						this.text = null;
					}
				}
				this.SetAcceptDrops(false);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0001B004 File Offset: 0x0001A004
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDoubleClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDoubleClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0001B034 File Offset: 0x0001A034
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragEnter(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragEnter];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0001B064 File Offset: 0x0001A064
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragOver(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragOver];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0001B094 File Offset: 0x0001A094
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventDragLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0001B0C4 File Offset: 0x0001A0C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragDrop(DragEventArgs drgevent)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[Control.EventDragDrop];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, drgevent);
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0001B0F4 File Offset: 0x0001A0F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
		{
			GiveFeedbackEventHandler giveFeedbackEventHandler = (GiveFeedbackEventHandler)base.Events[Control.EventGiveFeedback];
			if (giveFeedbackEventHandler != null)
			{
				giveFeedbackEventHandler(this, gfbevent);
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0001B124 File Offset: 0x0001A124
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnEnter(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventEnter];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0001B152 File Offset: 0x0001A152
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeGotFocus(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnGotFocus(e);
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0001B160 File Offset: 0x0001A160
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGotFocus(EventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(true);
			}
			if (this.parent != null)
			{
				this.parent.ChildGotFocus(this);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventGotFocus];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0001B1B4 File Offset: 0x0001A1B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHelpRequested(HelpEventArgs hevent)
		{
			HelpEventHandler helpEventHandler = (HelpEventHandler)base.Events[Control.EventHelpRequested];
			if (helpEventHandler != null)
			{
				helpEventHandler(this, hevent);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.ParentInternal != null)
			{
				this.ParentInternal.OnHelpRequested(hevent);
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0001B208 File Offset: 0x0001A208
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInvalidated(InvalidateEventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXViewChanged();
			}
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnParentInvalidated(e);
				}
			}
			InvalidateEventHandler invalidateEventHandler = (InvalidateEventHandler)base.Events[Control.EventInvalidated];
			if (invalidateEventHandler != null)
			{
				invalidateEventHandler(this, e);
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0001B27C File Offset: 0x0001A27C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[Control.EventKeyDown];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0001B2AC File Offset: 0x0001A2AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
			KeyPressEventHandler keyPressEventHandler = (KeyPressEventHandler)base.Events[Control.EventKeyPress];
			if (keyPressEventHandler != null)
			{
				keyPressEventHandler(this, e);
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0001B2DC File Offset: 0x0001A2DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[Control.EventKeyUp];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0001B30C File Offset: 0x0001A30C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLayout(LayoutEventArgs levent)
		{
			if (this.IsActiveX)
			{
				this.ActiveXViewChanged();
			}
			LayoutEventHandler layoutEventHandler = (LayoutEventHandler)base.Events[Control.EventLayout];
			if (layoutEventHandler != null)
			{
				layoutEventHandler(this, levent);
			}
			bool flag = this.LayoutEngine.Layout(this, levent);
			if (flag && this.ParentInternal != null)
			{
				this.ParentInternal.SetState(8388608, true);
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0001B372 File Offset: 0x0001A372
		internal virtual void OnLayoutResuming(bool performLayout)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.OnChildLayoutResuming(this, performLayout);
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0001B389 File Offset: 0x0001A389
		internal virtual void OnLayoutSuspended()
		{
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0001B38C File Offset: 0x0001A38C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0001B3BA File Offset: 0x0001A3BA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void InvokeLostFocus(Control toInvoke, EventArgs e)
		{
			if (toInvoke != null)
			{
				toInvoke.OnLostFocus(e);
			}
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0001B3C8 File Offset: 0x0001A3C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLostFocus(EventArgs e)
		{
			if (this.IsActiveX)
			{
				this.ActiveXOnFocus(false);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventLostFocus];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0001B408 File Offset: 0x0001A408
		protected virtual void OnMarginChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMarginChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0001B438 File Offset: 0x0001A438
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseDoubleClick(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseDoubleClick];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0001B468 File Offset: 0x0001A468
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseClick(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseClick];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0001B498 File Offset: 0x0001A498
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseCaptureChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseCaptureChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0001B4C8 File Offset: 0x0001A4C8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseDown];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0001B4F8 File Offset: 0x0001A4F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseEnter(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseEnter];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0001B528 File Offset: 0x0001A528
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseLeave(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseLeave];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0001B558 File Offset: 0x0001A558
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseHover(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMouseHover];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0001B588 File Offset: 0x0001A588
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseMove];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0001B5B8 File Offset: 0x0001A5B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseUp];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0001B5E8 File Offset: 0x0001A5E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMouseWheel(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[Control.EventMouseWheel];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0001B618 File Offset: 0x0001A618
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMove(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventMove];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.RenderTransparent)
			{
				this.Invalidate();
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0001B654 File Offset: 0x0001A654
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPaint(PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[Control.EventPaint];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0001B684 File Offset: 0x0001A684
		protected virtual void OnPaddingChanged(EventArgs e)
		{
			if (this.GetStyle(ControlStyles.ResizeRedraw))
			{
				this.Invalidate();
			}
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventPaddingChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0001B6C4 File Offset: 0x0001A6C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnPaintBackground(PaintEventArgs pevent)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			this.PaintBackground(pevent, new Rectangle(rect.left, rect.top, rect.right, rect.bottom));
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0001B71C File Offset: 0x0001A71C
		private void OnParentInvalidated(InvalidateEventArgs e)
		{
			if (!this.RenderTransparent)
			{
				return;
			}
			if (this.IsHandleCreated)
			{
				Rectangle rectangle = e.InvalidRect;
				Point location = this.Location;
				rectangle.Offset(-location.X, -location.Y);
				rectangle = Rectangle.Intersect(this.ClientRectangle, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				this.Invalidate(rectangle);
			}
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0001B780 File Offset: 0x0001A780
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
		{
			QueryContinueDragEventHandler queryContinueDragEventHandler = (QueryContinueDragEventHandler)base.Events[Control.EventQueryContinueDrag];
			if (queryContinueDragEventHandler != null)
			{
				queryContinueDragEventHandler(this, qcdevent);
			}
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0001B7B0 File Offset: 0x0001A7B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRegionChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Control.EventRegionChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0001B7E0 File Offset: 0x0001A7E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResize(EventArgs e)
		{
			if ((this.controlStyle & ControlStyles.ResizeRedraw) == ControlStyles.ResizeRedraw || this.GetState(4194304))
			{
				this.Invalidate();
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventResize];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0001B83C File Offset: 0x0001A83C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			PreviewKeyDownEventHandler previewKeyDownEventHandler = (PreviewKeyDownEventHandler)base.Events[Control.EventPreviewKeyDown];
			if (previewKeyDownEventHandler != null)
			{
				previewKeyDownEventHandler(this, e);
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0001B86C File Offset: 0x0001A86C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnSizeChanged(EventArgs e)
		{
			this.OnResize(EventArgs.Empty);
			EventHandler eventHandler = base.Events[Control.EventSize] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0001B8A8 File Offset: 0x0001A8A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnChangeUICues(UICuesEventArgs e)
		{
			UICuesEventHandler uicuesEventHandler = (UICuesEventHandler)base.Events[Control.EventChangeUICues];
			if (uicuesEventHandler != null)
			{
				uicuesEventHandler(this, e);
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0001B8D8 File Offset: 0x0001A8D8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventStyleChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0001B908 File Offset: 0x0001A908
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnSystemColorsChanged(EventArgs e)
		{
			Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
			if (controlCollection != null)
			{
				for (int i = 0; i < controlCollection.Count; i++)
				{
					controlCollection[i].OnSystemColorsChanged(EventArgs.Empty);
				}
			}
			this.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventSystemColorsChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0001B978 File Offset: 0x0001A978
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnValidating(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Control.EventValidating];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0001B9A8 File Offset: 0x0001A9A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnValidated(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventValidated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0001B9D6 File Offset: 0x0001A9D6
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle)
		{
			this.PaintBackground(e, rectangle, this.BackColor, Point.Empty);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0001B9EB File Offset: 0x0001A9EB
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle, Color backColor)
		{
			this.PaintBackground(e, rectangle, backColor, Point.Empty);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0001B9FC File Offset: 0x0001A9FC
		internal void PaintBackground(PaintEventArgs e, Rectangle rectangle, Color backColor, Point scrollOffset)
		{
			if (this.RenderColorTransparent(backColor))
			{
				this.PaintTransparentBackground(e, rectangle);
			}
			bool flag = (this is Form || this is MdiClient) && this.IsMirrored;
			if (this.BackgroundImage != null && !DisplayInformation.HighContrast && !flag)
			{
				if (this.BackgroundImageLayout == ImageLayout.Tile && ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					this.PaintTransparentBackground(e, rectangle);
				}
				Point point = scrollOffset;
				ScrollableControl scrollableControl = this as ScrollableControl;
				if (scrollableControl != null && point != Point.Empty)
				{
					point = ((ScrollableControl)this).AutoScrollPosition;
				}
				if (ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					Control.PaintBackColor(e, rectangle, backColor);
				}
				ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, backColor, this.BackgroundImageLayout, this.ClientRectangle, rectangle, point, this.RightToLeft);
				return;
			}
			Control.PaintBackColor(e, rectangle, backColor);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0001BAD8 File Offset: 0x0001AAD8
		private static void PaintBackColor(PaintEventArgs e, Rectangle rectangle, Color backColor)
		{
			Color color = backColor;
			if (color.A == 255)
			{
				using (WindowsGraphics windowsGraphics = ((e.HDC != IntPtr.Zero && DisplayInformation.BitsPerPixel > 8) ? WindowsGraphics.FromHdc(e.HDC) : WindowsGraphics.FromGraphics(e.Graphics)))
				{
					color = windowsGraphics.GetNearestColor(color);
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, color))
					{
						windowsGraphics.FillRectangle(windowsBrush, rectangle);
					}
					return;
				}
			}
			if (color.A > 0)
			{
				using (Brush brush = new SolidBrush(color))
				{
					e.Graphics.FillRectangle(brush, rectangle);
				}
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0001BBB0 File Offset: 0x0001ABB0
		private void PaintException(PaintEventArgs e)
		{
			int num = 2;
			using (Pen pen = new Pen(Color.Red, (float)num))
			{
				Rectangle clientRectangle = this.ClientRectangle;
				Rectangle rectangle = clientRectangle;
				rectangle.X++;
				rectangle.Y++;
				rectangle.Width--;
				rectangle.Height--;
				e.Graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				rectangle.Inflate(-1, -1);
				e.Graphics.FillRectangle(Brushes.White, rectangle);
				e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, clientRectangle.Bottom);
				e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom, clientRectangle.Right, clientRectangle.Top);
			}
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0001BCC4 File Offset: 0x0001ACC4
		internal void PaintTransparentBackground(PaintEventArgs e, Rectangle rectangle)
		{
			this.PaintTransparentBackground(e, rectangle, null);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0001BCD0 File Offset: 0x0001ACD0
		internal void PaintTransparentBackground(PaintEventArgs e, Rectangle rectangle, Region transparentRegion)
		{
			Graphics graphics = e.Graphics;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				if (Application.RenderWithVisualStyles && parentInternal.RenderTransparencyWithVisualStyles)
				{
					GraphicsState graphicsState = null;
					if (transparentRegion != null)
					{
						graphicsState = graphics.Save();
					}
					try
					{
						if (transparentRegion != null)
						{
							graphics.Clip = transparentRegion;
						}
						ButtonRenderer.DrawParentBackground(graphics, rectangle, this);
						return;
					}
					finally
					{
						if (graphicsState != null)
						{
							graphics.Restore(graphicsState);
						}
					}
				}
				Rectangle rectangle2 = new Rectangle(-this.Left, -this.Top, parentInternal.Width, parentInternal.Height);
				Rectangle rectangle3 = new Rectangle(rectangle.Left + this.Left, rectangle.Top + this.Top, rectangle.Width, rectangle.Height);
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics))
				{
					windowsGraphics.DeviceContext.TranslateTransform(-this.Left, -this.Top);
					using (PaintEventArgs paintEventArgs = new PaintEventArgs(windowsGraphics.GetHdc(), rectangle3))
					{
						if (transparentRegion != null)
						{
							paintEventArgs.Graphics.Clip = transparentRegion;
							paintEventArgs.Graphics.TranslateClip(-rectangle2.X, -rectangle2.Y);
						}
						try
						{
							this.InvokePaintBackground(parentInternal, paintEventArgs);
							this.InvokePaint(parentInternal, paintEventArgs);
						}
						finally
						{
							if (transparentRegion != null)
							{
								paintEventArgs.Graphics.TranslateClip(rectangle2.X, rectangle2.Y);
							}
						}
					}
					return;
				}
			}
			graphics.FillRectangle(SystemBrushes.Control, rectangle);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0001BE6C File Offset: 0x0001AE6C
		private void PaintWithErrorHandling(PaintEventArgs e, short layer)
		{
			try
			{
				this.CacheTextInternal = true;
				if (this.GetState(4194304))
				{
					if (layer == 1)
					{
						this.PaintException(e);
					}
				}
				else
				{
					bool flag = true;
					try
					{
						switch (layer)
						{
						case 1:
							if (!this.GetStyle(ControlStyles.Opaque))
							{
								this.OnPaintBackground(e);
							}
							break;
						case 2:
							this.OnPaint(e);
							break;
						}
						flag = false;
					}
					finally
					{
						if (flag)
						{
							this.SetState(4194304, true);
							this.Invalidate();
						}
					}
				}
			}
			finally
			{
				this.CacheTextInternal = false;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x0001BF0C File Offset: 0x0001AF0C
		internal ContainerControl ParentContainerControl
		{
			get
			{
				for (Control control = this.ParentInternal; control != null; control = control.ParentInternal)
				{
					if (control is ContainerControl)
					{
						return control as ContainerControl;
					}
				}
				return null;
			}
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0001BF3C File Offset: 0x0001AF3C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void PerformLayout()
		{
			if (this.cachedLayoutEventArgs != null)
			{
				this.PerformLayout(this.cachedLayoutEventArgs);
				this.cachedLayoutEventArgs = null;
				this.SetState2(64, false);
				return;
			}
			this.PerformLayout(null, null);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0001BF6B File Offset: 0x0001AF6B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void PerformLayout(Control affectedControl, string affectedProperty)
		{
			this.PerformLayout(new LayoutEventArgs(affectedControl, affectedProperty));
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0001BF7C File Offset: 0x0001AF7C
		internal void PerformLayout(LayoutEventArgs args)
		{
			if (this.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.layoutSuspendCount > 0)
			{
				this.SetState(512, true);
				if (this.cachedLayoutEventArgs == null || (this.GetState2(64) && args != null))
				{
					this.cachedLayoutEventArgs = args;
					if (this.GetState2(64))
					{
						this.SetState2(64, false);
					}
				}
				this.LayoutEngine.ProcessSuspendedLayoutEventArgs(this, args);
				return;
			}
			this.layoutSuspendCount = 1;
			try
			{
				this.CacheTextInternal = true;
				this.OnLayout(args);
			}
			finally
			{
				this.CacheTextInternal = false;
				this.SetState(8389120, false);
				this.layoutSuspendCount = 0;
				if (this.ParentInternal != null && this.ParentInternal.GetState(8388608))
				{
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.PreferredSize);
				}
			}
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0001C054 File Offset: 0x0001B054
		internal bool PerformControlValidation(bool bulkValidation)
		{
			if (!this.CausesValidation)
			{
				return false;
			}
			if (this.NotifyValidating())
			{
				return true;
			}
			if (bulkValidation || NativeWindow.WndProcShouldBeDebuggable)
			{
				this.NotifyValidated();
			}
			else
			{
				try
				{
					this.NotifyValidated();
				}
				catch (Exception ex)
				{
					Application.OnThreadException(ex);
				}
			}
			return false;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0001C0AC File Offset: 0x0001B0AC
		internal bool PerformContainerValidation(ValidationConstraints validationConstraints)
		{
			bool flag = false;
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				if ((validationConstraints & ValidationConstraints.ImmediateChildren) != ValidationConstraints.ImmediateChildren && control.ShouldPerformContainerValidation() && control.PerformContainerValidation(validationConstraints))
				{
					flag = true;
				}
				if (((validationConstraints & ValidationConstraints.Selectable) != ValidationConstraints.Selectable || control.GetStyle(ControlStyles.Selectable)) && ((validationConstraints & ValidationConstraints.Enabled) != ValidationConstraints.Enabled || control.Enabled) && ((validationConstraints & ValidationConstraints.Visible) != ValidationConstraints.Visible || control.Visible) && ((validationConstraints & ValidationConstraints.TabStop) != ValidationConstraints.TabStop || control.TabStop) && control.PerformControlValidation(true))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0001C164 File Offset: 0x0001B164
		public Point PointToClient(Point p)
		{
			return this.PointToClientInternal(p);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0001C170 File Offset: 0x0001B170
		internal Point PointToClientInternal(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(this, this.Handle), point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0001C1BC File Offset: 0x0001B1BC
		public Point PointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0001C208 File Offset: 0x0001B208
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual bool PreProcessMessage(ref Message msg)
		{
			if (msg.Msg == 256 || msg.Msg == 260)
			{
				if (!this.GetState2(512))
				{
					this.ProcessUICues(ref msg);
				}
				Keys keys = (Keys)(long)msg.WParam | Control.ModifierKeys;
				if (this.ProcessCmdKey(ref msg, keys))
				{
					return true;
				}
				if (this.IsInputKey(keys))
				{
					this.SetState2(128, true);
					return false;
				}
				IntSecurity.ModifyFocus.Assert();
				try
				{
					return this.ProcessDialogKey(keys);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			bool flag;
			if (msg.Msg == 258 || msg.Msg == 262)
			{
				if (msg.Msg == 258 && this.IsInputChar((char)(int)msg.WParam))
				{
					this.SetState2(256, true);
					flag = false;
				}
				else
				{
					flag = this.ProcessDialogChar((char)(int)msg.WParam);
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0001C310 File Offset: 0x0001B310
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public PreProcessControlState PreProcessControlMessage(ref Message msg)
		{
			return Control.PreProcessControlMessageInternal(null, ref msg);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0001C31C File Offset: 0x0001B31C
		internal static PreProcessControlState PreProcessControlMessageInternal(Control target, ref Message msg)
		{
			if (target == null)
			{
				target = Control.FromChildHandleInternal(msg.HWnd);
			}
			if (target == null)
			{
				return PreProcessControlState.MessageNotNeeded;
			}
			target.SetState2(128, false);
			target.SetState2(256, false);
			target.SetState2(512, true);
			PreProcessControlState preProcessControlState2;
			try
			{
				Keys keys = (Keys)(long)msg.WParam | Control.ModifierKeys;
				if (msg.Msg == 256 || msg.Msg == 260)
				{
					target.ProcessUICues(ref msg);
					PreviewKeyDownEventArgs previewKeyDownEventArgs = new PreviewKeyDownEventArgs(keys);
					target.OnPreviewKeyDown(previewKeyDownEventArgs);
					if (previewKeyDownEventArgs.IsInputKey)
					{
						return PreProcessControlState.MessageNeeded;
					}
				}
				PreProcessControlState preProcessControlState = PreProcessControlState.MessageNotNeeded;
				if (!target.PreProcessMessage(ref msg))
				{
					if (msg.Msg == 256 || msg.Msg == 260)
					{
						if (target.GetState2(128) || target.IsInputKey(keys))
						{
							preProcessControlState = PreProcessControlState.MessageNeeded;
						}
					}
					else if ((msg.Msg == 258 || msg.Msg == 262) && (target.GetState2(256) || target.IsInputChar((char)(int)msg.WParam)))
					{
						preProcessControlState = PreProcessControlState.MessageNeeded;
					}
				}
				else
				{
					preProcessControlState = PreProcessControlState.MessageProcessed;
				}
				preProcessControlState2 = preProcessControlState;
			}
			finally
			{
				target.SetState2(512, false);
			}
			return preProcessControlState2;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0001C458 File Offset: 0x0001B458
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			return (contextMenu != null && contextMenu.ProcessCmdKey(ref msg, keyData, this)) || (this.parent != null && this.parent.ProcessCmdKey(ref msg, keyData));
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0001C4A4 File Offset: 0x0001B4A4
		private void PrintToMetaFile(HandleRef hDC, IntPtr lParam)
		{
			lParam = (IntPtr)((long)lParam & -17L);
			NativeMethods.POINT point = new NativeMethods.POINT();
			SafeNativeMethods.GetViewportOrgEx(hDC, point);
			HandleRef handleRef = new HandleRef(null, SafeNativeMethods.CreateRectRgn(point.x, point.y, point.x + this.Width, point.y + this.Height));
			try
			{
				SafeNativeMethods.SelectClipRgn(hDC, handleRef);
				this.PrintToMetaFileRecursive(hDC, lParam, new Rectangle(Point.Empty, this.Size));
			}
			finally
			{
				SafeNativeMethods.DeleteObject(handleRef);
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0001C540 File Offset: 0x0001B540
		internal virtual void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				this.PrintToMetaFile_SendPrintMessage(hDC, (IntPtr)((long)lParam & -5L));
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(null, this.Handle), ref rect);
				Point point = this.PointToScreen(Point.Empty);
				point = new Point(point.X - rect.left, point.Y - rect.top);
				Rectangle rectangle = new Rectangle(point, this.ClientSize);
				using (new WindowsFormsUtils.DCMapping(hDC, rectangle))
				{
					this.PrintToMetaFile_SendPrintMessage(hDC, (IntPtr)((long)lParam & -3L));
					int count = this.Controls.Count;
					for (int i = count - 1; i >= 0; i--)
					{
						Control control = this.Controls[i];
						if (control.Visible)
						{
							control.PrintToMetaFileRecursive(hDC, lParam, control.Bounds);
						}
					}
				}
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0001C66C File Offset: 0x0001B66C
		private void PrintToMetaFile_SendPrintMessage(HandleRef hDC, IntPtr lParam)
		{
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				this.SendMessage(791, hDC.Handle, lParam);
				return;
			}
			if (this.Controls.Count == 0)
			{
				lParam = (IntPtr)((long)lParam | 16L);
			}
			using (Control.MetafileDCWrapper metafileDCWrapper = new Control.MetafileDCWrapper(hDC, this.Size))
			{
				this.SendMessage(791, metafileDCWrapper.HDC, lParam);
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0001C6F4 File Offset: 0x0001B6F4
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessDialogChar(char charCode)
		{
			return this.parent != null && this.parent.ProcessDialogChar(charCode);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0001C70C File Offset: 0x0001B70C
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessDialogKey(Keys keyData)
		{
			return this.parent != null && this.parent.ProcessDialogKey(keyData);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0001C724 File Offset: 0x0001B724
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessKeyEventArgs(ref Message m)
		{
			KeyEventArgs keyEventArgs = null;
			KeyPressEventArgs keyPressEventArgs = null;
			IntPtr intPtr = IntPtr.Zero;
			if (m.Msg == 258 || m.Msg == 262)
			{
				int num = this.ImeWmCharsToIgnore;
				if (num > 0)
				{
					num--;
					this.ImeWmCharsToIgnore = num;
					return false;
				}
				keyPressEventArgs = new KeyPressEventArgs((char)(long)m.WParam);
				this.OnKeyPress(keyPressEventArgs);
				intPtr = (IntPtr)((int)keyPressEventArgs.KeyChar);
			}
			else if (m.Msg == 646)
			{
				int num2 = this.ImeWmCharsToIgnore;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					char c = '\0';
					byte[] array = new byte[]
					{
						(byte)((int)(long)m.WParam >> 8),
						(byte)(long)m.WParam
					};
					char[] array2 = new char[1];
					int num3 = UnsafeNativeMethods.MultiByteToWideChar(0, 1, array, array.Length, array2, 0);
					if (num3 <= 0)
					{
						throw new Win32Exception();
					}
					array2 = new char[num3];
					UnsafeNativeMethods.MultiByteToWideChar(0, 1, array, array.Length, array2, array2.Length);
					if (array2[0] != '\0')
					{
						c = array2[0];
						num2 += 2;
					}
					else if (array2[0] == '\0' && array2.Length >= 2)
					{
						c = array2[1];
						num2++;
					}
					this.ImeWmCharsToIgnore = num2;
					keyPressEventArgs = new KeyPressEventArgs(c);
				}
				else
				{
					num2 += 3 - Marshal.SystemDefaultCharSize;
					this.ImeWmCharsToIgnore = num2;
					keyPressEventArgs = new KeyPressEventArgs((char)(long)m.WParam);
				}
				char keyChar = keyPressEventArgs.KeyChar;
				this.OnKeyPress(keyPressEventArgs);
				if (keyPressEventArgs.KeyChar == keyChar)
				{
					intPtr = m.WParam;
				}
				else if (Marshal.SystemDefaultCharSize == 1)
				{
					string text = new string(new char[] { keyPressEventArgs.KeyChar });
					int num4 = UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
					if (num4 >= 2)
					{
						byte[] array3 = new byte[num4];
						UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array3, array3.Length, IntPtr.Zero, IntPtr.Zero);
						int num5 = Marshal.SizeOf(typeof(IntPtr));
						if (num4 > num5)
						{
							num4 = num5;
						}
						long num6 = 0L;
						for (int i = 0; i < num4; i++)
						{
							num6 <<= 8;
							num6 |= (long)((ulong)array3[i]);
						}
						intPtr = (IntPtr)num6;
					}
					else if (num4 == 1)
					{
						byte[] array3 = new byte[num4];
						UnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array3, array3.Length, IntPtr.Zero, IntPtr.Zero);
						intPtr = (IntPtr)((int)array3[0]);
					}
					else
					{
						intPtr = m.WParam;
					}
				}
				else
				{
					intPtr = (IntPtr)((int)keyPressEventArgs.KeyChar);
				}
			}
			else
			{
				keyEventArgs = new KeyEventArgs((Keys)(long)m.WParam | Control.ModifierKeys);
				if (m.Msg == 256 || m.Msg == 260)
				{
					this.OnKeyDown(keyEventArgs);
				}
				else
				{
					this.OnKeyUp(keyEventArgs);
				}
			}
			if (keyPressEventArgs != null)
			{
				m.WParam = intPtr;
				return keyPressEventArgs.Handled;
			}
			if (keyEventArgs.SuppressKeyPress)
			{
				this.RemovePendingMessages(258, 258);
				this.RemovePendingMessages(262, 262);
				this.RemovePendingMessages(646, 646);
			}
			return keyEventArgs.Handled;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0001CA61 File Offset: 0x0001BA61
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessKeyMessage(ref Message m)
		{
			return (this.parent != null && this.parent.ProcessKeyPreview(ref m)) || this.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0001CA82 File Offset: 0x0001BA82
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual bool ProcessKeyPreview(ref Message m)
		{
			return this.parent != null && this.parent.ProcessKeyPreview(ref m);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0001CA9A File Offset: 0x0001BA9A
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessMnemonic(char charCode)
		{
			return false;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0001CAA0 File Offset: 0x0001BAA0
		internal void ProcessUICues(ref Message msg)
		{
			Keys keys = (Keys)((int)msg.WParam & 65535);
			if (keys != Keys.F10 && keys != Keys.Menu && keys != Keys.Tab)
			{
				return;
			}
			Control control = null;
			int num = (int)this.SendMessage(297, 0, 0);
			if (num == 0)
			{
				control = this.TopMostParent;
				num = (int)control.SendMessage(297, 0, 0);
			}
			int num2 = 0;
			if ((keys == Keys.F10 || keys == Keys.Menu) && (num & 2) != 0)
			{
				num2 |= 2;
			}
			if (keys == Keys.Tab && (num & 1) != 0)
			{
				num2 |= 1;
			}
			if (num2 != 0)
			{
				if (control == null)
				{
					control = this.TopMostParent;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(control, control.Handle), (UnsafeNativeMethods.GetParent(new HandleRef(null, control.Handle)) == IntPtr.Zero) ? 295 : 296, (IntPtr)(2 | (num2 << 16)), IntPtr.Zero);
			}
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0001CB7C File Offset: 0x0001BB7C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaiseDragEvent(object key, DragEventArgs e)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[key];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, e);
			}
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0001CBA8 File Offset: 0x0001BBA8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RaisePaintEvent(object key, PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[Control.EventPaint];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0001CBD8 File Offset: 0x0001BBD8
		private void RemovePendingMessages(int msgMin, int msgMax)
		{
			if (!this.IsDisposed)
			{
				NativeMethods.MSG msg = default(NativeMethods.MSG);
				IntPtr handle = this.Handle;
				while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, handle), msgMin, msgMax, 1))
				{
				}
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0001CC0F File Offset: 0x0001BC0F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetBackColor()
		{
			this.BackColor = Color.Empty;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0001CC1C File Offset: 0x0001BC1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetCursor()
		{
			this.Cursor = null;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0001CC25 File Offset: 0x0001BC25
		private void ResetEnabled()
		{
			this.Enabled = true;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0001CC2E File Offset: 0x0001BC2E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetFont()
		{
			this.Font = null;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0001CC37 File Offset: 0x0001BC37
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetForeColor()
		{
			this.ForeColor = Color.Empty;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0001CC44 File Offset: 0x0001BC44
		private void ResetLocation()
		{
			this.Location = new Point(0, 0);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0001CC53 File Offset: 0x0001BC53
		private void ResetMargin()
		{
			this.Margin = this.DefaultMargin;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0001CC61 File Offset: 0x0001BC61
		private void ResetMinimumSize()
		{
			this.MinimumSize = this.DefaultMinimumSize;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0001CC6F File Offset: 0x0001BC6F
		private void ResetPadding()
		{
			CommonProperties.ResetPadding(this);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0001CC77 File Offset: 0x0001BC77
		private void ResetSize()
		{
			this.Size = this.DefaultSize;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0001CC85 File Offset: 0x0001BC85
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.Inherit;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0001CC8E File Offset: 0x0001BC8E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void RecreateHandle()
		{
			this.RecreateHandleCore();
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0001CC98 File Offset: 0x0001BC98
		internal virtual void RecreateHandleCore()
		{
			lock (this)
			{
				if (this.IsHandleCreated)
				{
					bool containsFocus = this.ContainsFocus;
					bool flag = (this.state & 1) != 0;
					if (this.GetState(16384))
					{
						this.SetState(8192, true);
						this.UnhookMouseEvent();
					}
					HandleRef handleRef = new HandleRef(this, UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle)));
					try
					{
						Control[] array = null;
						this.state |= 16;
						try
						{
							Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
							if (controlCollection != null && controlCollection.Count > 0)
							{
								array = new Control[controlCollection.Count];
								for (int i = 0; i < controlCollection.Count; i++)
								{
									Control control = controlCollection[i];
									if (control != null && control.IsHandleCreated)
									{
										control.OnParentHandleRecreating();
										array[i] = control;
									}
									else
									{
										array[i] = null;
									}
								}
							}
							this.DestroyHandle();
							this.CreateHandle();
						}
						finally
						{
							this.state &= -17;
							if (array != null)
							{
								foreach (Control control2 in array)
								{
									if (control2 != null && control2.IsHandleCreated)
									{
										control2.OnParentHandleRecreated();
									}
								}
							}
						}
						if (flag)
						{
							this.CreateControl();
						}
					}
					finally
					{
						if (handleRef.Handle != IntPtr.Zero && (Control.FromHandleInternal(handleRef.Handle) == null || this.parent == null) && UnsafeNativeMethods.IsWindow(handleRef))
						{
							UnsafeNativeMethods.SetParent(new HandleRef(this, this.Handle), handleRef);
						}
					}
					if (containsFocus)
					{
						this.FocusInternal();
					}
				}
			}
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0001CE8C File Offset: 0x0001BE8C
		public Rectangle RectangleToClient(Rectangle r)
		{
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
			UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(this, this.Handle), ref rect, 2);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0001CEF8 File Offset: 0x0001BEF8
		public Rectangle RectangleToScreen(Rectangle r)
		{
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, ref rect, 2);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0001CF62 File Offset: 0x0001BF62
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static bool ReflectMessage(IntPtr hWnd, ref Message m)
		{
			IntSecurity.SendMessages.Demand();
			return Control.ReflectMessageInternal(hWnd, ref m);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0001CF78 File Offset: 0x0001BF78
		internal static bool ReflectMessageInternal(IntPtr hWnd, ref Message m)
		{
			Control control = Control.FromHandleInternal(hWnd);
			if (control == null)
			{
				return false;
			}
			m.Result = control.SendMessage(8192 + m.Msg, m.WParam, m.LParam);
			return true;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0001CFB6 File Offset: 0x0001BFB6
		public virtual void Refresh()
		{
			this.Invalidate(true);
			this.Update();
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0001CFC5 File Offset: 0x0001BFC5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void ResetMouseEventArgs()
		{
			if (this.GetState(16384))
			{
				this.UnhookMouseEvent();
				this.HookMouseEvent();
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0001CFE0 File Offset: 0x0001BFE0
		public virtual void ResetText()
		{
			this.Text = string.Empty;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0001CFED File Offset: 0x0001BFED
		private void ResetVisible()
		{
			this.Visible = true;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0001CFF6 File Offset: 0x0001BFF6
		public void ResumeLayout()
		{
			this.ResumeLayout(true);
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0001D000 File Offset: 0x0001C000
		public void ResumeLayout(bool performLayout)
		{
			bool flag = false;
			if (this.layoutSuspendCount > 0)
			{
				if (this.layoutSuspendCount == 1)
				{
					this.layoutSuspendCount += 1;
					try
					{
						this.OnLayoutResuming(performLayout);
					}
					finally
					{
						this.layoutSuspendCount -= 1;
					}
				}
				this.layoutSuspendCount -= 1;
				if (this.layoutSuspendCount == 0 && this.GetState(512) && performLayout)
				{
					this.PerformLayout();
					flag = true;
				}
			}
			if (!flag)
			{
				this.SetState2(64, true);
			}
			if (!performLayout)
			{
				CommonProperties.xClearPreferredSizeCache(this);
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						this.LayoutEngine.InitLayout(controlCollection[i], BoundsSpecified.All);
						CommonProperties.xClearPreferredSizeCache(controlCollection[i]);
					}
				}
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0001D0E8 File Offset: 0x0001C0E8
		internal void SetAcceptDrops(bool accept)
		{
			if (accept != this.GetState(128) && this.IsHandleCreated)
			{
				try
				{
					if (Application.OleRequired() != ApartmentState.STA)
					{
						throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
					}
					if (accept)
					{
						IntSecurity.ClipboardRead.Demand();
						int num = UnsafeNativeMethods.RegisterDragDrop(new HandleRef(this, this.Handle), new DropTarget(this));
						if (num != 0 && num != -2147221247)
						{
							throw new Win32Exception(num);
						}
					}
					else
					{
						int num2 = UnsafeNativeMethods.RevokeDragDrop(new HandleRef(this, this.Handle));
						if (num2 != 0 && num2 != -2147221248)
						{
							throw new Win32Exception(num2);
						}
					}
					this.SetState(128, accept);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), ex);
				}
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0001D1B4 File Offset: 0x0001C1B4
		[Obsolete("This method has been deprecated. Use the Scale(SizeF ratio) method instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Scale(float ratio)
		{
			this.ScaleCore(ratio, ratio);
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0001D1C0 File Offset: 0x0001C1C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method has been deprecated. Use the Scale(SizeF ratio) method instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void Scale(float dx, float dy)
		{
			this.SuspendLayout();
			try
			{
				this.ScaleCore(dx, dy);
			}
			finally
			{
				this.ResumeLayout();
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0001D1F4 File Offset: 0x0001C1F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Scale(SizeF factor)
		{
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				this.ScaleControl(factor, factor, this);
				if (this.ScaleChildren)
				{
					Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
					if (controlCollection != null)
					{
						for (int i = 0; i < controlCollection.Count; i++)
						{
							Control control = controlCollection[i];
							control.Scale(factor);
						}
					}
				}
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0001D284 File Offset: 0x0001C284
		internal virtual void Scale(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				this.ScaleControl(includedFactor, excludedFactor, requestingControl);
				this.ScaleChildControls(includedFactor, excludedFactor, requestingControl);
			}
			LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0001D2DC File Offset: 0x0001C2DC
		internal void ScaleChildControls(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			if (this.ScaleChildren)
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						Control control = controlCollection[i];
						control.Scale(includedFactor, excludedFactor, requestingControl);
					}
				}
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0001D32C File Offset: 0x0001C32C
		internal void ScaleControl(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			BoundsSpecified boundsSpecified = BoundsSpecified.None;
			BoundsSpecified boundsSpecified2 = BoundsSpecified.None;
			if (!includedFactor.IsEmpty)
			{
				boundsSpecified = this.RequiredScaling;
			}
			if (!excludedFactor.IsEmpty)
			{
				boundsSpecified2 |= ~this.RequiredScaling & BoundsSpecified.All;
			}
			if (boundsSpecified != BoundsSpecified.None)
			{
				this.ScaleControl(includedFactor, boundsSpecified);
			}
			if (boundsSpecified2 != BoundsSpecified.None)
			{
				this.ScaleControl(excludedFactor, boundsSpecified2);
			}
			if (!includedFactor.IsEmpty)
			{
				this.RequiredScaling = BoundsSpecified.None;
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0001D38C File Offset: 0x0001C38C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			CreateParams createParams = this.CreateParams;
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
			SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			Size size = this.MinimumSize;
			Size size2 = this.MaximumSize;
			this.MinimumSize = Size.Empty;
			this.MaximumSize = Size.Empty;
			Rectangle scaledBounds = this.GetScaledBounds(this.Bounds, factor, specified);
			float num = factor.Width;
			float num2 = factor.Height;
			Padding padding = this.Padding;
			Padding margin = this.Margin;
			if (num == 1f)
			{
				specified &= ~(BoundsSpecified.X | BoundsSpecified.Width);
			}
			if (num2 == 1f)
			{
				specified &= ~(BoundsSpecified.Y | BoundsSpecified.Height);
			}
			if (num != 1f)
			{
				padding.Left = (int)Math.Round((double)((float)padding.Left * num));
				padding.Right = (int)Math.Round((double)((float)padding.Right * num));
				margin.Left = (int)Math.Round((double)((float)margin.Left * num));
				margin.Right = (int)Math.Round((double)((float)margin.Right * num));
			}
			if (num2 != 1f)
			{
				padding.Top = (int)Math.Round((double)((float)padding.Top * num2));
				padding.Bottom = (int)Math.Round((double)((float)padding.Bottom * num2));
				margin.Top = (int)Math.Round((double)((float)margin.Top * num2));
				margin.Bottom = (int)Math.Round((double)((float)margin.Bottom * num2));
			}
			this.Padding = padding;
			this.Margin = margin;
			Size size3 = rect.Size;
			if (!size.IsEmpty)
			{
				size -= size3;
				size = this.ScaleSize(LayoutUtils.UnionSizes(Size.Empty, size), factor.Width, factor.Height) + size3;
			}
			if (!size2.IsEmpty)
			{
				size2 -= size3;
				size2 = this.ScaleSize(LayoutUtils.UnionSizes(Size.Empty, size2), factor.Width, factor.Height) + size3;
			}
			Size size4 = LayoutUtils.ConvertZeroToUnbounded(size2);
			Size size5 = LayoutUtils.IntersectSizes(scaledBounds.Size, size4);
			size5 = LayoutUtils.UnionSizes(size5, size);
			this.SetBoundsCore(scaledBounds.X, scaledBounds.Y, size5.Width, size5.Height, BoundsSpecified.All);
			this.MaximumSize = size2;
			this.MinimumSize = size;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0001D5F8 File Offset: 0x0001C5F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void ScaleCore(float dx, float dy)
		{
			this.SuspendLayout();
			try
			{
				int num = (int)Math.Round((double)((float)this.x * dx));
				int num2 = (int)Math.Round((double)((float)this.y * dy));
				int num3 = this.width;
				if ((this.controlStyle & ControlStyles.FixedWidth) != ControlStyles.FixedWidth)
				{
					num3 = (int)Math.Round((double)((float)(this.x + this.width) * dx)) - num;
				}
				int num4 = this.height;
				if ((this.controlStyle & ControlStyles.FixedHeight) != ControlStyles.FixedHeight)
				{
					num4 = (int)Math.Round((double)((float)(this.y + this.height) * dy)) - num2;
				}
				this.SetBounds(num, num2, num3, num4, BoundsSpecified.All);
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection != null)
				{
					for (int i = 0; i < controlCollection.Count; i++)
					{
						controlCollection[i].Scale(dx, dy);
					}
				}
			}
			finally
			{
				this.ResumeLayout();
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0001D6F0 File Offset: 0x0001C6F0
		internal Size ScaleSize(Size startSize, float x, float y)
		{
			Size size = startSize;
			if (!this.GetStyle(ControlStyles.FixedWidth))
			{
				size.Width = (int)Math.Round((double)((float)size.Width * x));
			}
			if (!this.GetStyle(ControlStyles.FixedHeight))
			{
				size.Height = (int)Math.Round((double)((float)size.Height * y));
			}
			return size;
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0001D744 File Offset: 0x0001C744
		public void Select()
		{
			this.Select(false, false);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0001D750 File Offset: 0x0001C750
		protected virtual void Select(bool directed, bool forward)
		{
			IContainerControl containerControlInternal = this.GetContainerControlInternal();
			if (containerControlInternal != null)
			{
				containerControlInternal.ActiveControl = this;
			}
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0001D770 File Offset: 0x0001C770
		public bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!this.Contains(ctl) || (!nested && ctl.parent != this))
			{
				ctl = null;
			}
			bool flag = false;
			Control control = ctl;
			for (;;)
			{
				ctl = this.GetNextControl(ctl, forward);
				if (ctl == null)
				{
					if (!wrap)
					{
						return false;
					}
					if (flag)
					{
						break;
					}
					flag = true;
				}
				else if (ctl.CanSelect && (!tabStopOnly || ctl.TabStop) && (nested || ctl.parent == this))
				{
					goto IL_0057;
				}
				if (ctl == control)
				{
					return false;
				}
			}
			return false;
			IL_0057:
			ctl.Select(true, forward);
			return true;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0001D7E3 File Offset: 0x0001C7E3
		internal bool SelectNextControlInternal(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			return this.SelectNextControl(ctl, forward, tabStopOnly, nested, wrap);
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0001D7F4 File Offset: 0x0001C7F4
		private void SelectNextIfFocused()
		{
			if (this.ContainsFocus && this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					((Control)containerControlInternal).SelectNextControlInternal(this, true, true, true, true);
				}
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0001D831 File Offset: 0x0001C831
		internal IntPtr SendMessage(int msg, int wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0001D847 File Offset: 0x0001C847
		internal IntPtr SendMessage(int msg, ref int wparam, ref int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, ref wparam, ref lparam);
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0001D85D File Offset: 0x0001C85D
		internal IntPtr SendMessage(int msg, int wparam, IntPtr lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, (IntPtr)wparam, lparam);
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0001D878 File Offset: 0x0001C878
		internal IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0001D88E File Offset: 0x0001C88E
		internal IntPtr SendMessage(int msg, IntPtr wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, (IntPtr)lparam);
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0001D8A9 File Offset: 0x0001C8A9
		internal IntPtr SendMessage(int msg, int wparam, ref NativeMethods.RECT lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, ref lparam);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0001D8BF File Offset: 0x0001C8BF
		internal IntPtr SendMessage(int msg, bool wparam, int lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0001D8D5 File Offset: 0x0001C8D5
		internal IntPtr SendMessage(int msg, int wparam, string lparam)
		{
			return UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), msg, wparam, lparam);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0001D8EC File Offset: 0x0001C8EC
		public void SendToBack()
		{
			if (this.parent != null)
			{
				this.parent.Controls.SetChildIndex(this, -1);
				return;
			}
			if (this.IsHandleCreated && this.GetTopLevel())
			{
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.HWND_BOTTOM, 0, 0, 0, 0, 3);
			}
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0001D948 File Offset: 0x0001C948
		public void SetBounds(int x, int y, int width, int height)
		{
			if (this.x != x || this.y != y || this.width != width || this.height != height)
			{
				this.SetBoundsCore(x, y, width, height, BoundsSpecified.All);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
				return;
			}
			this.InitScaling(BoundsSpecified.All);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0001D9A4 File Offset: 0x0001C9A4
		public void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.X) == BoundsSpecified.None)
			{
				x = this.x;
			}
			if ((specified & BoundsSpecified.Y) == BoundsSpecified.None)
			{
				y = this.y;
			}
			if ((specified & BoundsSpecified.Width) == BoundsSpecified.None)
			{
				width = this.width;
			}
			if ((specified & BoundsSpecified.Height) == BoundsSpecified.None)
			{
				height = this.height;
			}
			if (this.x != x || this.y != y || this.width != width || this.height != height)
			{
				this.SetBoundsCore(x, y, width, height, specified);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
				return;
			}
			this.InitScaling(specified);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0001DA38 File Offset: 0x0001CA38
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.SuspendLayout();
			}
			try
			{
				if (this.x != x || this.y != y || this.width != width || this.height != height)
				{
					CommonProperties.UpdateSpecifiedBounds(this, x, y, width, height, specified);
					Rectangle rectangle = this.ApplyBoundsConstraints(x, y, width, height);
					width = rectangle.Width;
					height = rectangle.Height;
					x = rectangle.X;
					y = rectangle.Y;
					if (!this.IsHandleCreated)
					{
						this.UpdateBounds(x, y, width, height);
					}
					else if (!this.GetState(65536))
					{
						int num = 20;
						if (this.x == x && this.y == y)
						{
							num |= 2;
						}
						if (this.width == width && this.height == height)
						{
							num |= 1;
						}
						this.OnBoundsUpdate(x, y, width, height);
						SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, x, y, width, height, num);
					}
				}
			}
			finally
			{
				this.InitScaling(specified);
				if (this.ParentInternal != null)
				{
					CommonProperties.xClearPreferredSizeCache(this.ParentInternal);
					this.ParentInternal.LayoutEngine.InitLayout(this, specified);
					this.ParentInternal.ResumeLayout(true);
				}
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0001DB8C File Offset: 0x0001CB8C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void SetClientSizeCore(int x, int y)
		{
			this.Size = this.SizeFromClientSize(x, y);
			this.clientWidth = x;
			this.clientHeight = y;
			this.OnClientSizeChanged(EventArgs.Empty);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0001DBB5 File Offset: 0x0001CBB5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Size SizeFromClientSize(Size clientSize)
		{
			return this.SizeFromClientSize(clientSize.Width, clientSize.Height);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0001DBCC File Offset: 0x0001CBCC
		internal Size SizeFromClientSize(int width, int height)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, width, height);
			CreateParams createParams = this.CreateParams;
			SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
			return rect.Size;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0001DC11 File Offset: 0x0001CC11
		private void SetHandle(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				this.SetState(1, false);
			}
			this.UpdateRoot();
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0001DC30 File Offset: 0x0001CC30
		private void SetParentHandle(IntPtr value)
		{
			if (this.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.Handle));
				bool topLevel = this.GetTopLevel();
				if (intPtr != value || (intPtr == IntPtr.Zero && !topLevel))
				{
					bool flag = (intPtr == IntPtr.Zero && !topLevel) || (value == IntPtr.Zero && topLevel);
					if (flag)
					{
						Form form = this as Form;
						if (form != null && !form.CanRecreateHandle())
						{
							flag = false;
							this.UpdateStyles();
						}
					}
					if (flag)
					{
						this.RecreateHandle();
					}
					if (!this.GetTopLevel())
					{
						if (value == IntPtr.Zero)
						{
							Application.ParkHandle(new HandleRef(this.window, this.Handle));
							this.UpdateRoot();
							return;
						}
						UnsafeNativeMethods.SetParent(new HandleRef(this.window, this.Handle), new HandleRef(null, value));
						if (this.parent != null)
						{
							this.parent.UpdateChildZOrder(this);
						}
						Application.UnparkHandle(new HandleRef(this.window, this.Handle));
						return;
					}
				}
				else if (value == IntPtr.Zero && intPtr == IntPtr.Zero && topLevel)
				{
					UnsafeNativeMethods.SetParent(new HandleRef(this.window, this.Handle), new HandleRef(null, IntPtr.Zero));
					Application.UnparkHandle(new HandleRef(this.window, this.Handle));
				}
			}
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0001DDA3 File Offset: 0x0001CDA3
		internal void SetState(int flag, bool value)
		{
			this.state = (value ? (this.state | flag) : (this.state & ~flag));
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0001DDC1 File Offset: 0x0001CDC1
		internal void SetState2(int flag, bool value)
		{
			this.state2 = (value ? (this.state2 | flag) : (this.state2 & ~flag));
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0001DDDF File Offset: 0x0001CDDF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void SetStyle(ControlStyles flag, bool value)
		{
			if ((flag & ControlStyles.EnableNotifyMessage) != (ControlStyles)0 && value)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			this.controlStyle = (value ? (this.controlStyle | flag) : (this.controlStyle & ~flag));
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0001DE14 File Offset: 0x0001CE14
		internal static IntPtr SetUpPalette(IntPtr dc, bool force, bool realizePalette)
		{
			IntPtr halftonePalette = Graphics.GetHalftonePalette();
			IntPtr intPtr = SafeNativeMethods.SelectPalette(new HandleRef(null, dc), new HandleRef(null, halftonePalette), force ? 0 : 1);
			if (intPtr != IntPtr.Zero && realizePalette)
			{
				SafeNativeMethods.RealizePalette(new HandleRef(null, dc));
			}
			return intPtr;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0001DE60 File Offset: 0x0001CE60
		protected void SetTopLevel(bool value)
		{
			if (value && this.IsActiveX)
			{
				throw new InvalidOperationException(SR.GetString("TopLevelNotAllowedIfActiveX"));
			}
			if (value)
			{
				if (this is Form)
				{
					IntSecurity.TopLevelWindow.Demand();
				}
				else
				{
					IntSecurity.UnrestrictedWindows.Demand();
				}
			}
			this.SetTopLevelInternal(value);
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0001DEB0 File Offset: 0x0001CEB0
		internal void SetTopLevelInternal(bool value)
		{
			if (this.GetTopLevel() != value)
			{
				if (this.parent != null)
				{
					throw new ArgumentException(SR.GetString("TopLevelParentedControl"), "value");
				}
				this.SetState(524288, value);
				if (this.IsHandleCreated && this.GetState2(8))
				{
					this.ListenToUserPreferenceChanged(value);
				}
				this.UpdateStyles();
				this.SetParentHandle(IntPtr.Zero);
				if (value && this.Visible)
				{
					this.CreateControl();
				}
				this.UpdateRoot();
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0001DF30 File Offset: 0x0001CF30
		protected virtual void SetVisibleCore(bool value)
		{
			try
			{
				global::System.Internal.HandleCollector.SuspendCollect();
				if (this.GetVisibleCore() != value)
				{
					if (!value)
					{
						this.SelectNextIfFocused();
					}
					bool flag = false;
					if (this.GetTopLevel())
					{
						if (this.IsHandleCreated || value)
						{
							SafeNativeMethods.ShowWindow(new HandleRef(this, this.Handle), value ? this.ShowParams : 0);
						}
					}
					else if (this.IsHandleCreated || (value && this.parent != null && this.parent.Created))
					{
						this.SetState(2, value);
						flag = true;
						try
						{
							if (value)
							{
								this.CreateControl();
							}
							SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 23 | (value ? 64 : 128));
						}
						catch
						{
							this.SetState(2, !value);
							throw;
						}
					}
					if (this.GetVisibleCore() != value)
					{
						this.SetState(2, value);
						flag = true;
					}
					if (flag)
					{
						using (new LayoutTransaction(this.parent, this, PropertyNames.Visible))
						{
							this.OnVisibleChanged(EventArgs.Empty);
						}
					}
					this.UpdateRoot();
				}
				else if (this.GetState(2) || value || !this.IsHandleCreated || SafeNativeMethods.IsWindowVisible(new HandleRef(this, this.Handle)))
				{
					this.SetState(2, value);
					if (this.IsHandleCreated)
					{
						SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 23 | (value ? 64 : 128));
					}
				}
			}
			finally
			{
				global::System.Internal.HandleCollector.ResumeCollect();
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0001E104 File Offset: 0x0001D104
		internal static AutoValidate GetAutoValidateForControl(Control control)
		{
			ContainerControl parentContainerControl = control.ParentContainerControl;
			if (parentContainerControl == null)
			{
				return AutoValidate.EnablePreventFocusChange;
			}
			return parentContainerControl.AutoValidate;
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0001E123 File Offset: 0x0001D123
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldAutoValidate
		{
			get
			{
				return Control.GetAutoValidateForControl(this) != AutoValidate.Disable;
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0001E131 File Offset: 0x0001D131
		internal virtual bool ShouldPerformContainerValidation()
		{
			return this.GetStyle(ControlStyles.ContainerControl);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0001E13C File Offset: 0x0001D13C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeBackColor()
		{
			return !this.Properties.GetColor(Control.PropBackColor).IsEmpty;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0001E164 File Offset: 0x0001D164
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeCursor()
		{
			bool flag;
			object @object = this.Properties.GetObject(Control.PropCursor, out flag);
			return flag && @object != null;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0001E190 File Offset: 0x0001D190
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeEnabled()
		{
			return !this.GetState(4);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0001E19C File Offset: 0x0001D19C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeForeColor()
		{
			return !this.Properties.GetColor(Control.PropForeColor).IsEmpty;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0001E1C4 File Offset: 0x0001D1C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeFont()
		{
			bool flag;
			object @object = this.Properties.GetObject(Control.PropFont, out flag);
			return flag && @object != null;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0001E1F0 File Offset: 0x0001D1F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeRightToLeft()
		{
			bool flag;
			int integer = this.Properties.GetInteger(Control.PropRightToLeft, out flag);
			return flag && integer != 2;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0001E21C File Offset: 0x0001D21C
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisible()
		{
			return !this.GetState(2);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0001E228 File Offset: 0x0001D228
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected HorizontalAlignment RtlTranslateAlignment(HorizontalAlignment align)
		{
			return this.RtlTranslateHorizontal(align);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0001E231 File Offset: 0x0001D231
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected LeftRightAlignment RtlTranslateAlignment(LeftRightAlignment align)
		{
			return this.RtlTranslateLeftRight(align);
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0001E23A File Offset: 0x0001D23A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected ContentAlignment RtlTranslateAlignment(ContentAlignment align)
		{
			return this.RtlTranslateContent(align);
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0001E243 File Offset: 0x0001D243
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected HorizontalAlignment RtlTranslateHorizontal(HorizontalAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if (align == HorizontalAlignment.Left)
				{
					return HorizontalAlignment.Right;
				}
				if (HorizontalAlignment.Right == align)
				{
					return HorizontalAlignment.Left;
				}
			}
			return align;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0001E25A File Offset: 0x0001D25A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected LeftRightAlignment RtlTranslateLeftRight(LeftRightAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if (align == LeftRightAlignment.Left)
				{
					return LeftRightAlignment.Right;
				}
				if (LeftRightAlignment.Right == align)
				{
					return LeftRightAlignment.Left;
				}
			}
			return align;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0001E274 File Offset: 0x0001D274
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal ContentAlignment RtlTranslateContent(ContentAlignment align)
		{
			if (RightToLeft.Yes == this.RightToLeft)
			{
				if ((align & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.TopLeft)
					{
						return ContentAlignment.TopRight;
					}
					if (align == ContentAlignment.TopRight)
					{
						return ContentAlignment.TopLeft;
					}
				}
				if ((align & WindowsFormsUtils.AnyMiddleAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.MiddleLeft)
					{
						return ContentAlignment.MiddleRight;
					}
					if (align == ContentAlignment.MiddleRight)
					{
						return ContentAlignment.MiddleLeft;
					}
				}
				if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
				{
					if (align == ContentAlignment.BottomLeft)
					{
						return ContentAlignment.BottomRight;
					}
					if (align == ContentAlignment.BottomRight)
					{
						return ContentAlignment.BottomLeft;
					}
				}
			}
			return align;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0001E2EA File Offset: 0x0001D2EA
		private void SetWindowFont()
		{
			this.SendMessage(48, this.FontHandle, 0);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0001E2FC File Offset: 0x0001D2FC
		private void SetWindowStyle(int flag, bool value)
		{
			int num = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16);
			UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)(value ? (num | flag) : (num & ~flag))));
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0001E34E File Offset: 0x0001D34E
		public void Show()
		{
			this.Visible = true;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0001E358 File Offset: 0x0001D358
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeMargin()
		{
			return !this.Margin.Equals(this.DefaultMargin);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0001E387 File Offset: 0x0001D387
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeMaximumSize()
		{
			return this.MaximumSize != this.DefaultMaximumSize;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0001E39A File Offset: 0x0001D39A
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeMinimumSize()
		{
			return this.MinimumSize != this.DefaultMinimumSize;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0001E3B0 File Offset: 0x0001D3B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializePadding()
		{
			return !this.Padding.Equals(this.DefaultPadding);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0001E3E0 File Offset: 0x0001D3E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeSize()
		{
			Size defaultSize = this.DefaultSize;
			return this.width != defaultSize.Width || this.height != defaultSize.Height;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0001E417 File Offset: 0x0001D417
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeText()
		{
			return this.Text.Length != 0;
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0001E42A File Offset: 0x0001D42A
		public void SuspendLayout()
		{
			this.layoutSuspendCount += 1;
			if (this.layoutSuspendCount == 1)
			{
				this.OnLayoutSuspended();
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0001E44A File Offset: 0x0001D44A
		private void UnhookMouseEvent()
		{
			this.SetState(16384, false);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0001E458 File Offset: 0x0001D458
		public void Update()
		{
			SafeNativeMethods.UpdateWindow(new HandleRef(this.window, this.InternalHandle));
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0001E474 File Offset: 0x0001D474
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal void UpdateBounds()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			int right = rect.right;
			int bottom = rect.bottom;
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this.window, this.InternalHandle), ref rect);
			if (!this.GetTopLevel())
			{
				UnsafeNativeMethods.MapWindowPoints(NativeMethods.NullHandleRef, new HandleRef(null, UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.InternalHandle))), ref rect, 2);
			}
			this.UpdateBounds(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, right, bottom);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0001E534 File Offset: 0x0001D534
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateBounds(int x, int y, int width, int height)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (rect.right = (rect.top = (rect.bottom = 0)));
			CreateParams createParams = this.CreateParams;
			SafeNativeMethods.AdjustWindowRectEx(ref rect, createParams.Style, false, createParams.ExStyle);
			int num = width - (rect.right - rect.left);
			int num2 = height - (rect.bottom - rect.top);
			this.UpdateBounds(x, y, width, height, num, num2);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0001E5C4 File Offset: 0x0001D5C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateBounds(int x, int y, int width, int height, int clientWidth, int clientHeight)
		{
			bool flag = this.x != x || this.y != y;
			bool flag2 = this.Width != width || this.Height != height || this.clientWidth != clientWidth || this.clientHeight != clientHeight;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.clientWidth = clientWidth;
			this.clientHeight = clientHeight;
			if (flag)
			{
				this.OnLocationChanged(EventArgs.Empty);
			}
			if (flag2)
			{
				this.OnSizeChanged(EventArgs.Empty);
				this.OnClientSizeChanged(EventArgs.Empty);
				CommonProperties.xClearPreferredSizeCache(this);
				LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0001E684 File Offset: 0x0001D684
		private void UpdateBindings()
		{
			for (int i = 0; i < this.DataBindings.Count; i++)
			{
				BindingContext.UpdateBinding(this.BindingContext, this.DataBindings[i]);
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0001E6C0 File Offset: 0x0001D6C0
		private void UpdateChildControlIndex(Control ctl)
		{
			int num = 0;
			int childIndex = this.Controls.GetChildIndex(ctl);
			IntPtr internalHandle = ctl.InternalHandle;
			while ((internalHandle = UnsafeNativeMethods.GetWindow(new HandleRef(null, internalHandle), 3)) != IntPtr.Zero)
			{
				Control control = Control.FromHandleInternal(internalHandle);
				if (control != null)
				{
					num = this.Controls.GetChildIndex(control, false) + 1;
					break;
				}
			}
			if (num > childIndex)
			{
				num--;
			}
			if (num != childIndex)
			{
				this.Controls.SetChildIndex(ctl, num);
			}
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0001E738 File Offset: 0x0001D738
		private void UpdateReflectParent(bool findNewParent)
		{
			if (!this.Disposing && findNewParent && this.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, this.Handle));
				if (intPtr != IntPtr.Zero)
				{
					this.ReflectParent = Control.FromHandleInternal(intPtr);
					return;
				}
			}
			this.ReflectParent = null;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0001E78B File Offset: 0x0001D78B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateZOrder()
		{
			if (this.parent != null)
			{
				this.parent.UpdateChildZOrder(this);
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0001E7A4 File Offset: 0x0001D7A4
		private void UpdateChildZOrder(Control ctl)
		{
			if (!this.IsHandleCreated || !ctl.IsHandleCreated || ctl.parent != this)
			{
				return;
			}
			IntPtr intPtr = (IntPtr)NativeMethods.HWND_TOP;
			int num = this.Controls.GetChildIndex(ctl);
			while (--num >= 0)
			{
				Control control = this.Controls[num];
				if (control.IsHandleCreated && control.parent == this)
				{
					intPtr = control.Handle;
					break;
				}
			}
			if (UnsafeNativeMethods.GetWindow(new HandleRef(ctl.window, ctl.Handle), 3) != intPtr)
			{
				this.state |= 256;
				try
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(ctl.window, ctl.Handle), new HandleRef(null, intPtr), 0, 0, 0, 0, 3);
				}
				finally
				{
					this.state &= -257;
				}
			}
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0001E890 File Offset: 0x0001D890
		private void UpdateRoot()
		{
			this.window.LockReference(this.GetTopLevel() && this.Visible);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0001E8AE File Offset: 0x0001D8AE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void UpdateStyles()
		{
			this.UpdateStylesCore();
			this.OnStyleChanged(EventArgs.Empty);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0001E8C4 File Offset: 0x0001D8C4
		internal virtual void UpdateStylesCore()
		{
			if (this.IsHandleCreated)
			{
				CreateParams createParams = this.CreateParams;
				int windowStyle = this.WindowStyle;
				int windowExStyle = this.WindowExStyle;
				if ((this.state & 2) != 0)
				{
					createParams.Style |= 268435456;
				}
				if (windowStyle != createParams.Style)
				{
					this.WindowStyle = createParams.Style;
				}
				if (windowExStyle != createParams.ExStyle)
				{
					this.WindowExStyle = createParams.ExStyle;
					this.SetState(1073741824, (createParams.ExStyle & 4194304) != 0);
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 55);
				this.Invalidate(true);
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0001E978 File Offset: 0x0001D978
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Color)
			{
				Control.defaultFont = null;
				this.OnSystemColorsChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0001E994 File Offset: 0x0001D994
		internal virtual void OnBoundsUpdate(int x, int y, int width, int height)
		{
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0001E996 File Offset: 0x0001D996
		internal void WindowAssignHandle(IntPtr handle, bool value)
		{
			this.window.AssignHandle(handle, value);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0001E9A5 File Offset: 0x0001D9A5
		internal void WindowReleaseHandle()
		{
			this.window.ReleaseHandle();
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0001E9B4 File Offset: 0x0001D9B4
		private void WmClose(ref Message m)
		{
			if (this.ParentInternal != null)
			{
				IntPtr handle = this.Handle;
				IntPtr intPtr = handle;
				while (handle != IntPtr.Zero)
				{
					intPtr = handle;
					handle = UnsafeNativeMethods.GetParent(new HandleRef(null, handle));
					int num = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(null, intPtr), -16);
					if ((num & 1073741824) == 0)
					{
						break;
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(null, intPtr), 16, IntPtr.Zero, IntPtr.Zero);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0001EA3B File Offset: 0x0001DA3B
		private void WmCaptureChanged(ref Message m)
		{
			this.OnMouseCaptureChanged(EventArgs.Empty);
			this.DefWndProc(ref m);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0001EA4F File Offset: 0x0001DA4F
		private void WmCommand(ref Message m)
		{
			if (IntPtr.Zero == m.LParam)
			{
				if (Command.DispatchID(NativeMethods.Util.LOWORD(m.WParam)))
				{
					return;
				}
			}
			else if (Control.ReflectMessageInternal(m.LParam, ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0001EA8C File Offset: 0x0001DA8C
		internal virtual void WmContextMenu(ref Message m)
		{
			this.WmContextMenu(ref m, this);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0001EA98 File Offset: 0x0001DA98
		internal void WmContextMenu(ref Message m, Control sourceControl)
		{
			ContextMenu contextMenu = this.Properties.GetObject(Control.PropContextMenu) as ContextMenu;
			ContextMenuStrip contextMenuStrip = ((contextMenu != null) ? null : (this.Properties.GetObject(Control.PropContextMenuStrip) as ContextMenuStrip));
			if (contextMenu == null && contextMenuStrip == null)
			{
				this.DefWndProc(ref m);
				return;
			}
			int num = NativeMethods.Util.SignedLOWORD(m.LParam);
			int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
			bool flag = false;
			Point point;
			if ((int)(long)m.LParam == -1)
			{
				flag = true;
				point = new Point(this.Width / 2, this.Height / 2);
			}
			else
			{
				point = this.PointToClientInternal(new Point(num, num2));
			}
			if (!this.ClientRectangle.Contains(point))
			{
				this.DefWndProc(ref m);
				return;
			}
			if (contextMenu != null)
			{
				contextMenu.Show(sourceControl, point);
				return;
			}
			if (contextMenuStrip != null)
			{
				contextMenuStrip.ShowInternal(sourceControl, point, flag);
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0001EB7C File Offset: 0x0001DB7C
		private void WmCtlColorControl(ref Message m)
		{
			Control control = Control.FromHandleInternal(m.LParam);
			if (control != null)
			{
				m.Result = control.InitializeDCForWmCtlColor(m.WParam, m.Msg);
				if (m.Result != IntPtr.Zero)
				{
					return;
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0001EBCA File Offset: 0x0001DBCA
		private void WmDisplayChange(ref Message m)
		{
			BufferedGraphicsManager.Current.Invalidate();
			this.DefWndProc(ref m);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0001EBDD File Offset: 0x0001DBDD
		private void WmDrawItem(ref Message m)
		{
			if (m.WParam == IntPtr.Zero)
			{
				this.WmDrawItemMenuItem(ref m);
				return;
			}
			this.WmOwnerDraw(ref m);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0001EC00 File Offset: 0x0001DC00
		private void WmDrawItemMenuItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(drawitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmDrawItem(ref m);
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0001EC3C File Offset: 0x0001DC3C
		private void WmEraseBkgnd(ref Message m)
		{
			if (this.GetStyle(ControlStyles.UserPaint))
			{
				if (!this.GetStyle(ControlStyles.AllPaintingInWmPaint))
				{
					IntPtr wparam = m.WParam;
					if (wparam == IntPtr.Zero)
					{
						m.Result = (IntPtr)0;
						return;
					}
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetClientRect(new HandleRef(this, this.Handle), ref rect);
					using (PaintEventArgs paintEventArgs = new PaintEventArgs(wparam, Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom)))
					{
						this.PaintWithErrorHandling(paintEventArgs, 1);
					}
				}
				m.Result = (IntPtr)1;
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0001ED00 File Offset: 0x0001DD00
		private void WmExitMenuLoop(ref Message m)
		{
			bool flag = (int)(long)m.WParam != 0;
			if (flag)
			{
				ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
				if (contextMenu != null)
				{
					contextMenu.OnCollapse(EventArgs.Empty);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0001ED50 File Offset: 0x0001DD50
		private void WmGetControlName(ref Message m)
		{
			string text;
			if (this.Site != null)
			{
				text = this.Site.Name;
			}
			else
			{
				text = this.Name;
			}
			if (text == null)
			{
				text = "";
			}
			this.MarshalStringToMessage(text, ref m);
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0001ED8C File Offset: 0x0001DD8C
		private void WmGetControlType(ref Message m)
		{
			string assemblyQualifiedName = base.GetType().AssemblyQualifiedName;
			this.MarshalStringToMessage(assemblyQualifiedName, ref m);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0001EDB0 File Offset: 0x0001DDB0
		private void WmGetObject(ref Message m)
		{
			InternalAccessibleObject internalAccessibleObject = null;
			AccessibleObject accessibilityObject = this.GetAccessibilityObject((int)(long)m.LParam);
			if (accessibilityObject != null)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					internalAccessibleObject = new InternalAccessibleObject(accessibilityObject);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (internalAccessibleObject != null)
			{
				Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
				try
				{
					object obj = internalAccessibleObject;
					IAccessible accessible = obj as IAccessible;
					if (accessible != null)
					{
						throw new InvalidOperationException(SR.GetString("ControlAccessibileObjectInvalid"));
					}
					UnsafeNativeMethods.IAccessibleInternal accessibleInternal = internalAccessibleObject;
					if (accessibleInternal == null)
					{
						m.Result = (IntPtr)0;
					}
					else
					{
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(accessibleInternal);
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							m.Result = UnsafeNativeMethods.LresultFromObject(ref guid, m.WParam, new HandleRef(accessibilityObject, iunknownForObject));
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							Marshal.Release(iunknownForObject);
						}
					}
					return;
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException(SR.GetString("RichControlLresult"), ex);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0001EEBC File Offset: 0x0001DEBC
		private void WmHelp(ref Message m)
		{
			HelpInfo helpInfo = MessageBox.HelpInfo;
			if (helpInfo != null)
			{
				switch (helpInfo.Option)
				{
				case 1:
					Help.ShowHelp(this, helpInfo.HelpFilePath);
					break;
				case 2:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Keyword);
					break;
				case 3:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Navigator);
					break;
				case 4:
					Help.ShowHelp(this, helpInfo.HelpFilePath, helpInfo.Navigator, helpInfo.Param);
					break;
				}
			}
			NativeMethods.HELPINFO helpinfo = (NativeMethods.HELPINFO)m.GetLParam(typeof(NativeMethods.HELPINFO));
			HelpEventArgs helpEventArgs = new HelpEventArgs(new Point(helpinfo.MousePos.x, helpinfo.MousePos.y));
			this.OnHelpRequested(helpEventArgs);
			if (!helpEventArgs.Handled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0001EF90 File Offset: 0x0001DF90
		private void WmInitMenuPopup(ref Message m)
		{
			ContextMenu contextMenu = (ContextMenu)this.Properties.GetObject(Control.PropContextMenu);
			if (contextMenu != null && contextMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0001EFCC File Offset: 0x0001DFCC
		private void WmMeasureItem(ref Message m)
		{
			if (m.WParam == IntPtr.Zero)
			{
				NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
				MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(measureitemstruct.itemData);
				if (menuItemFromItemData != null)
				{
					menuItemFromItemData.WmMeasureItem(ref m);
					return;
				}
			}
			else
			{
				this.WmOwnerDraw(ref m);
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0001F020 File Offset: 0x0001E020
		private void WmMenuChar(ref Message m)
		{
			Menu contextMenu = this.ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.WmMenuChar(ref m);
				if (m.Result != IntPtr.Zero)
				{
				}
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0001F050 File Offset: 0x0001E050
		private void WmMenuSelect(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			int num2 = NativeMethods.Util.HIWORD(m.WParam);
			IntPtr lparam = m.LParam;
			MenuItem menuItem = null;
			if ((num2 & 8192) == 0)
			{
				if ((num2 & 16) == 0)
				{
					Command commandFromID = Command.GetCommandFromID(num);
					if (commandFromID != null)
					{
						object target = commandFromID.Target;
						if (target != null && target is MenuItem.MenuItemData)
						{
							menuItem = ((MenuItem.MenuItemData)target).baseItem;
						}
					}
				}
				else
				{
					menuItem = this.GetMenuItemFromHandleId(lparam, num);
				}
			}
			if (menuItem != null)
			{
				menuItem.PerformSelect();
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0001F0D8 File Offset: 0x0001E0D8
		private void WmCreate(ref Message m)
		{
			this.DefWndProc(ref m);
			if (this.parent != null)
			{
				this.parent.UpdateChildZOrder(this);
			}
			this.UpdateBounds();
			this.OnHandleCreated(EventArgs.Empty);
			if (!this.GetStyle(ControlStyles.CacheText))
			{
				this.text = null;
			}
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0001F128 File Offset: 0x0001E128
		private void WmDestroy(ref Message m)
		{
			if (!this.RecreatingHandle && !this.Disposing && !this.IsDisposed && this.GetState(16384))
			{
				this.OnMouseLeave(EventArgs.Empty);
				this.UnhookMouseEvent();
			}
			this.OnHandleDestroyed(EventArgs.Empty);
			if (!this.Disposing)
			{
				if (!this.RecreatingHandle)
				{
					this.SetState(1, false);
				}
			}
			else
			{
				this.SetState(2, false);
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0001F19F File Offset: 0x0001E19F
		private void WmKeyChar(ref Message m)
		{
			if (this.ProcessKeyMessage(ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0001F1B2 File Offset: 0x0001E1B2
		private void WmKillFocus(ref Message m)
		{
			this.WmImeKillFocus();
			this.DefWndProc(ref m);
			this.OnLostFocus(EventArgs.Empty);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0001F1CC File Offset: 0x0001E1CC
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			MouseButtons mouseButtons = Control.MouseButtons;
			this.SetState(134217728, true);
			if (!this.GetStyle(ControlStyles.UserMouse))
			{
				this.DefWndProc(ref m);
				if (this.IsDisposed)
				{
					return;
				}
			}
			else if (button == MouseButtons.Left && this.GetStyle(ControlStyles.Selectable))
			{
				this.FocusInternal();
			}
			if (mouseButtons != Control.MouseButtons)
			{
				return;
			}
			if (!this.GetState2(16))
			{
				this.CaptureInternal = true;
			}
			if (mouseButtons != Control.MouseButtons)
			{
				return;
			}
			if (this.Enabled)
			{
				this.OnMouseDown(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			}
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0001F273 File Offset: 0x0001E273
		private void WmMouseEnter(ref Message m)
		{
			this.DefWndProc(ref m);
			this.OnMouseEnter(EventArgs.Empty);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0001F287 File Offset: 0x0001E287
		private void WmMouseLeave(ref Message m)
		{
			this.DefWndProc(ref m);
			this.OnMouseLeave(EventArgs.Empty);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0001F29B File Offset: 0x0001E29B
		private void WmMouseHover(ref Message m)
		{
			this.DefWndProc(ref m);
			this.OnMouseHover(EventArgs.Empty);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0001F2AF File Offset: 0x0001E2AF
		private void WmMouseMove(ref Message m)
		{
			if (!this.GetStyle(ControlStyles.UserMouse))
			{
				this.DefWndProc(ref m);
			}
			this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0001F2F0 File Offset: 0x0001E2F0
		private void WmMouseUp(ref Message m, MouseButtons button, int clicks)
		{
			try
			{
				int num = NativeMethods.Util.SignedLOWORD(m.LParam);
				int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
				Point point = new Point(num, num2);
				point = this.PointToScreen(point);
				if (!this.GetStyle(ControlStyles.UserMouse))
				{
					this.DefWndProc(ref m);
				}
				else if (button == MouseButtons.Right)
				{
					this.SendMessage(123, this.Handle, NativeMethods.Util.MAKELPARAM(point.X, point.Y));
				}
				bool flag = false;
				if ((this.controlStyle & ControlStyles.StandardClick) == ControlStyles.StandardClick && this.GetState(134217728) && !this.IsDisposed && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == this.Handle)
				{
					flag = true;
				}
				if (flag && !this.ValidationCancelled)
				{
					if (!this.GetState(67108864))
					{
						this.OnClick(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.OnMouseClick(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
					else
					{
						this.OnDoubleClick(new MouseEventArgs(button, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.OnMouseDoubleClick(new MouseEventArgs(button, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
				}
				this.OnMouseUp(new MouseEventArgs(button, clicks, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
			}
			finally
			{
				this.SetState(67108864, false);
				this.SetState(134217728, false);
				this.SetState(268435456, false);
				this.CaptureInternal = false;
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0001F4D0 File Offset: 0x0001E4D0
		private void WmMouseWheel(ref Message m)
		{
			Point point = new Point(NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
			point = this.PointToClient(point);
			HandledMouseEventArgs handledMouseEventArgs = new HandledMouseEventArgs(MouseButtons.None, 0, point.X, point.Y, NativeMethods.Util.SignedHIWORD(m.WParam));
			this.OnMouseWheel(handledMouseEventArgs);
			m.Result = (IntPtr)(handledMouseEventArgs.Handled ? 0 : 1);
			if (!handledMouseEventArgs.Handled)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0001F550 File Offset: 0x0001E550
		private void WmMove(ref Message m)
		{
			this.DefWndProc(ref m);
			this.UpdateBounds();
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0001F560 File Offset: 0x0001E560
		private unsafe void WmNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)(void*)m.LParam;
			if (!Control.ReflectMessageInternal(ptr->hwndFrom, ref m))
			{
				if (ptr->code == -521)
				{
					m.Result = UnsafeNativeMethods.SendMessage(new HandleRef(null, ptr->hwndFrom), 8192 + m.Msg, m.WParam, m.LParam);
					return;
				}
				if (ptr->code == -522)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(null, ptr->hwndFrom), 8192 + m.Msg, m.WParam, m.LParam);
				}
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0001F602 File Offset: 0x0001E602
		private void WmNotifyFormat(ref Message m)
		{
			if (!Control.ReflectMessageInternal(m.WParam, ref m))
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0001F61C File Offset: 0x0001E61C
		private void WmOwnerDraw(ref Message m)
		{
			bool flag = false;
			int num = (int)(long)m.WParam;
			IntPtr intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, m.HWnd), num);
			if (intPtr == IntPtr.Zero)
			{
				intPtr = (IntPtr)((long)num);
			}
			if (!Control.ReflectMessageInternal(intPtr, ref m))
			{
				IntPtr handleFromID = this.window.GetHandleFromID((short)NativeMethods.Util.LOWORD(m.WParam));
				if (handleFromID != IntPtr.Zero)
				{
					Control control = Control.FromHandleInternal(handleFromID);
					if (control != null)
					{
						m.Result = control.SendMessage(8192 + m.Msg, handleFromID, m.LParam);
						flag = true;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0001F6CC File Offset: 0x0001E6CC
		private void WmPaint(ref Message m)
		{
			bool flag = this.DoubleBuffered || (this.GetStyle(ControlStyles.AllPaintingInWmPaint) && this.DoubleBufferingEnabled);
			IntPtr intPtr = IntPtr.Zero;
			NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
			bool flag2 = false;
			try
			{
				IntPtr intPtr2;
				Rectangle rectangle;
				if (m.WParam == IntPtr.Zero)
				{
					intPtr = this.Handle;
					intPtr2 = UnsafeNativeMethods.BeginPaint(new HandleRef(this, intPtr), ref paintstruct);
					flag2 = true;
					rectangle = new Rectangle(paintstruct.rcPaint_left, paintstruct.rcPaint_top, paintstruct.rcPaint_right - paintstruct.rcPaint_left, paintstruct.rcPaint_bottom - paintstruct.rcPaint_top);
				}
				else
				{
					intPtr2 = m.WParam;
					rectangle = this.ClientRectangle;
				}
				if (!flag || (rectangle.Width > 0 && rectangle.Height > 0))
				{
					IntPtr intPtr3 = IntPtr.Zero;
					BufferedGraphics bufferedGraphics = null;
					PaintEventArgs paintEventArgs = null;
					GraphicsState graphicsState = null;
					try
					{
						if (flag || m.WParam == IntPtr.Zero)
						{
							intPtr3 = Control.SetUpPalette(intPtr2, false, false);
						}
						if (flag)
						{
							try
							{
								bufferedGraphics = this.BufferContext.Allocate(intPtr2, this.ClientRectangle);
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsCriticalException(ex))
								{
									throw;
								}
								flag = false;
							}
						}
						if (bufferedGraphics != null)
						{
							bufferedGraphics.Graphics.SetClip(rectangle);
							paintEventArgs = new PaintEventArgs(bufferedGraphics.Graphics, rectangle);
							graphicsState = paintEventArgs.Graphics.Save();
						}
						else
						{
							paintEventArgs = new PaintEventArgs(intPtr2, rectangle);
						}
						using (paintEventArgs)
						{
							try
							{
								if ((m.WParam == IntPtr.Zero && this.GetStyle(ControlStyles.AllPaintingInWmPaint)) || flag)
								{
									this.PaintWithErrorHandling(paintEventArgs, 1);
								}
							}
							finally
							{
								if (graphicsState != null)
								{
									paintEventArgs.Graphics.Restore(graphicsState);
								}
								else
								{
									paintEventArgs.ResetGraphics();
								}
							}
							this.PaintWithErrorHandling(paintEventArgs, 2);
							if (bufferedGraphics != null)
							{
								bufferedGraphics.Render();
							}
						}
					}
					finally
					{
						if (intPtr3 != IntPtr.Zero)
						{
							SafeNativeMethods.SelectPalette(new HandleRef(null, intPtr2), new HandleRef(null, intPtr3), 0);
						}
						if (bufferedGraphics != null)
						{
							bufferedGraphics.Dispose();
						}
					}
				}
			}
			finally
			{
				if (flag2)
				{
					UnsafeNativeMethods.EndPaint(new HandleRef(this, intPtr), ref paintstruct);
				}
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0001F95C File Offset: 0x0001E95C
		private void WmPrintClient(ref Message m)
		{
			using (PaintEventArgs paintEventArgs = new Control.PrintPaintEventArgs(m, m.WParam, this.ClientRectangle))
			{
				this.OnPrint(paintEventArgs);
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0001F9A4 File Offset: 0x0001E9A4
		private void WmQueryNewPalette(ref Message m)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(new HandleRef(this, this.Handle));
			try
			{
				Control.SetUpPalette(dc, true, true);
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.Handle), new HandleRef(null, dc));
			}
			this.Invalidate(true);
			m.Result = (IntPtr)1;
			this.DefWndProc(ref m);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0001FA14 File Offset: 0x0001EA14
		private void WmSetCursor(ref Message m)
		{
			if (m.WParam == this.InternalHandle && NativeMethods.Util.LOWORD(m.LParam) == 1)
			{
				Cursor.CurrentInternal = this.Cursor;
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0001FA4C File Offset: 0x0001EA4C
		private unsafe void WmWindowPosChanging(ref Message m)
		{
			if (this.IsActiveX)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)(void*)m.LParam;
				bool flag = false;
				if ((ptr->flags & 2) == 0 && (ptr->x != this.Left || ptr->y != this.Top))
				{
					flag = true;
				}
				if ((ptr->flags & 1) == 0 && (ptr->cx != this.Width || ptr->cy != this.Height))
				{
					flag = true;
				}
				if (flag)
				{
					this.ActiveXUpdateBounds(ref ptr->x, ref ptr->y, ref ptr->cx, ref ptr->cy, ptr->flags);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0001FAF0 File Offset: 0x0001EAF0
		private void WmParentNotify(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			IntPtr intPtr = IntPtr.Zero;
			switch (num)
			{
			case 1:
				intPtr = m.LParam;
				break;
			case 2:
				break;
			default:
				intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(this, this.Handle), NativeMethods.Util.HIWORD(m.WParam));
				break;
			}
			if (intPtr == IntPtr.Zero || !Control.ReflectMessageInternal(intPtr, ref m))
			{
				this.DefWndProc(ref m);
			}
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0001FB68 File Offset: 0x0001EB68
		private void WmSetFocus(ref Message m)
		{
			this.WmImeSetFocus();
			if (!this.HostedInWin32DialogManager)
			{
				IContainerControl containerControlInternal = this.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					ContainerControl containerControl = containerControlInternal as ContainerControl;
					bool flag;
					if (containerControl != null)
					{
						flag = containerControl.ActivateControlInternal(this);
					}
					else
					{
						IntSecurity.ModifyFocus.Assert();
						try
						{
							flag = containerControlInternal.ActivateControl(this);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (!flag)
					{
						return;
					}
				}
			}
			this.DefWndProc(ref m);
			this.OnGotFocus(EventArgs.Empty);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0001FBE4 File Offset: 0x0001EBE4
		private void WmShowWindow(ref Message m)
		{
			this.DefWndProc(ref m);
			if ((this.state & 16) == 0)
			{
				bool flag = m.WParam != IntPtr.Zero;
				bool visible = this.Visible;
				if (flag)
				{
					bool flag2 = this.GetState(2);
					this.SetState(2, true);
					bool flag3 = false;
					try
					{
						this.CreateControl();
						flag3 = true;
						goto IL_0081;
					}
					finally
					{
						if (!flag3)
						{
							this.SetState(2, flag2);
						}
					}
				}
				bool flag4 = this.GetTopLevel();
				if (this.ParentInternal != null)
				{
					flag4 = this.ParentInternal.Visible;
				}
				if (flag4)
				{
					this.SetState(2, false);
				}
				IL_0081:
				if (!this.GetState(536870912) && visible != flag)
				{
					this.OnVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0001FCA0 File Offset: 0x0001ECA0
		private void WmUpdateUIState(ref Message m)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = (this.uiCuesState & 240) != 0;
			bool flag4 = (this.uiCuesState & 15) != 0;
			if (flag3)
			{
				flag = this.ShowKeyboardCues;
			}
			if (flag4)
			{
				flag2 = this.ShowFocusCues;
			}
			this.DefWndProc(ref m);
			int num = NativeMethods.Util.LOWORD(m.WParam);
			if (num == 3)
			{
				return;
			}
			UICues uicues = UICues.None;
			if ((NativeMethods.Util.HIWORD(m.WParam) & 2) != 0)
			{
				bool flag5 = num == 2;
				if (flag5 != flag || !flag3)
				{
					uicues |= UICues.ChangeKeyboard;
					this.uiCuesState &= -241;
					this.uiCuesState |= (flag5 ? 32 : 16);
				}
				if (flag5)
				{
					uicues |= UICues.ShowKeyboard;
				}
			}
			if ((NativeMethods.Util.HIWORD(m.WParam) & 1) != 0)
			{
				bool flag6 = num == 2;
				if (flag6 != flag2 || !flag4)
				{
					uicues |= UICues.ChangeFocus;
					this.uiCuesState &= -16;
					this.uiCuesState |= (flag6 ? 2 : 1);
				}
				if (flag6)
				{
					uicues |= UICues.ShowFocus;
				}
			}
			if ((uicues & UICues.Changed) != UICues.None)
			{
				this.OnChangeUICues(new UICuesEventArgs(uicues));
				this.Invalidate(true);
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0001FDCC File Offset: 0x0001EDCC
		private unsafe void WmWindowPosChanged(ref Message m)
		{
			this.DefWndProc(ref m);
			this.UpdateBounds();
			if (this.parent != null && UnsafeNativeMethods.GetParent(new HandleRef(this.window, this.InternalHandle)) == this.parent.InternalHandle && (this.state & 256) == 0)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)(void*)m.LParam;
				if ((ptr->flags & 4) == 0)
				{
					this.parent.UpdateChildControlIndex(this);
				}
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0001FE48 File Offset: 0x0001EE48
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void WndProc(ref Message m)
		{
			if ((this.controlStyle & ControlStyles.EnableNotifyMessage) == ControlStyles.EnableNotifyMessage)
			{
				this.OnNotifyMessage(m);
			}
			int msg = m.Msg;
			if (msg <= 261)
			{
				if (msg <= 47)
				{
					if (msg <= 20)
					{
						switch (msg)
						{
						case 1:
							this.WmCreate(ref m);
							return;
						case 2:
							this.WmDestroy(ref m);
							return;
						case 3:
							this.WmMove(ref m);
							return;
						case 4:
						case 5:
						case 6:
							goto IL_062F;
						case 7:
							this.WmSetFocus(ref m);
							return;
						case 8:
							this.WmKillFocus(ref m);
							return;
						default:
							switch (msg)
							{
							case 15:
								if (this.GetStyle(ControlStyles.UserPaint))
								{
									this.WmPaint(ref m);
									return;
								}
								this.DefWndProc(ref m);
								return;
							case 16:
								this.WmClose(ref m);
								return;
							default:
								if (msg != 20)
								{
									goto IL_062F;
								}
								this.WmEraseBkgnd(ref m);
								return;
							}
							break;
						}
					}
					else
					{
						switch (msg)
						{
						case 24:
							this.WmShowWindow(ref m);
							return;
						case 25:
							break;
						default:
							if (msg == 32)
							{
								this.WmSetCursor(ref m);
								return;
							}
							switch (msg)
							{
							case 43:
								this.WmDrawItem(ref m);
								return;
							case 44:
								this.WmMeasureItem(ref m);
								return;
							case 45:
							case 46:
							case 47:
								goto IL_0432;
							default:
								goto IL_062F;
							}
							break;
						}
					}
				}
				else if (msg <= 71)
				{
					if (msg == 57)
					{
						goto IL_0432;
					}
					if (msg == 61)
					{
						this.WmGetObject(ref m);
						return;
					}
					switch (msg)
					{
					case 70:
						this.WmWindowPosChanging(ref m);
						return;
					case 71:
						this.WmWindowPosChanged(ref m);
						return;
					default:
						goto IL_062F;
					}
				}
				else if (msg <= 123)
				{
					switch (msg)
					{
					case 78:
						this.WmNotify(ref m);
						return;
					case 79:
					case 82:
					case 84:
						goto IL_062F;
					case 80:
						this.WmInputLangChangeRequest(ref m);
						return;
					case 81:
						this.WmInputLangChange(ref m);
						return;
					case 83:
						this.WmHelp(ref m);
						return;
					case 85:
						this.WmNotifyFormat(ref m);
						return;
					default:
						if (msg != 123)
						{
							goto IL_062F;
						}
						this.WmContextMenu(ref m);
						return;
					}
				}
				else
				{
					if (msg == 126)
					{
						this.WmDisplayChange(ref m);
						return;
					}
					switch (msg)
					{
					case 256:
					case 257:
					case 258:
					case 260:
					case 261:
						this.WmKeyChar(ref m);
						return;
					case 259:
						goto IL_062F;
					default:
						goto IL_062F;
					}
				}
			}
			else if (msg <= 642)
			{
				if (msg <= 296)
				{
					switch (msg)
					{
					case 269:
						this.WmImeStartComposition(ref m);
						return;
					case 270:
						this.WmImeEndComposition(ref m);
						return;
					case 271:
					case 272:
					case 275:
					case 278:
						goto IL_062F;
					case 273:
						this.WmCommand(ref m);
						return;
					case 274:
						if (((int)(long)m.WParam & 65520) == 61696 && ToolStripManager.ProcessMenuKey(ref m))
						{
							m.Result = IntPtr.Zero;
							return;
						}
						this.DefWndProc(ref m);
						return;
					case 276:
					case 277:
						goto IL_0432;
					case 279:
						this.WmInitMenuPopup(ref m);
						return;
					default:
						switch (msg)
						{
						case 287:
							this.WmMenuSelect(ref m);
							return;
						case 288:
							this.WmMenuChar(ref m);
							return;
						default:
							if (msg != 296)
							{
								goto IL_062F;
							}
							this.WmUpdateUIState(ref m);
							return;
						}
						break;
					}
				}
				else
				{
					switch (msg)
					{
					case 306:
					case 307:
					case 308:
					case 309:
					case 310:
					case 311:
					case 312:
						break;
					default:
						switch (msg)
						{
						case 512:
							this.WmMouseMove(ref m);
							return;
						case 513:
							this.WmMouseDown(ref m, MouseButtons.Left, 1);
							return;
						case 514:
							this.WmMouseUp(ref m, MouseButtons.Left, 1);
							return;
						case 515:
							this.WmMouseDown(ref m, MouseButtons.Left, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 516:
							this.WmMouseDown(ref m, MouseButtons.Right, 1);
							return;
						case 517:
							this.WmMouseUp(ref m, MouseButtons.Right, 1);
							return;
						case 518:
							this.WmMouseDown(ref m, MouseButtons.Right, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 519:
							this.WmMouseDown(ref m, MouseButtons.Middle, 1);
							return;
						case 520:
							this.WmMouseUp(ref m, MouseButtons.Middle, 1);
							return;
						case 521:
							this.WmMouseDown(ref m, MouseButtons.Middle, 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 522:
							this.WmMouseWheel(ref m);
							return;
						case 523:
							this.WmMouseDown(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 1);
							return;
						case 524:
							this.WmMouseUp(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 1);
							return;
						case 525:
							this.WmMouseDown(ref m, this.GetXButton(NativeMethods.Util.HIWORD(m.WParam)), 2);
							if (this.GetStyle(ControlStyles.StandardDoubleClick))
							{
								this.SetState(67108864, true);
								return;
							}
							return;
						case 526:
						case 527:
						case 529:
						case 531:
						case 532:
							goto IL_062F;
						case 528:
							this.WmParentNotify(ref m);
							return;
						case 530:
							this.WmExitMenuLoop(ref m);
							return;
						case 533:
							this.WmCaptureChanged(ref m);
							return;
						default:
							if (msg != 642)
							{
								goto IL_062F;
							}
							this.WmImeNotify(ref m);
							return;
						}
						break;
					}
				}
			}
			else if (msg <= 783)
			{
				if (msg == 646)
				{
					this.WmImeChar(ref m);
					return;
				}
				switch (msg)
				{
				case 673:
					this.WmMouseHover(ref m);
					return;
				case 674:
					goto IL_062F;
				case 675:
					this.WmMouseLeave(ref m);
					return;
				default:
					if (msg != 783)
					{
						goto IL_062F;
					}
					this.WmQueryNewPalette(ref m);
					return;
				}
			}
			else if (msg <= 8217)
			{
				if (msg != 792)
				{
					if (msg != 8217)
					{
						goto IL_062F;
					}
				}
				else
				{
					if (this.GetStyle(ControlStyles.UserPaint))
					{
						this.WmPrintClient(ref m);
						return;
					}
					this.DefWndProc(ref m);
					return;
				}
			}
			else
			{
				if (msg == 8277)
				{
					m.Result = (IntPtr)((Marshal.SystemDefaultCharSize == 1) ? 1 : 2);
					return;
				}
				switch (msg)
				{
				case 8498:
				case 8499:
				case 8500:
				case 8501:
				case 8502:
				case 8503:
				case 8504:
					break;
				default:
					goto IL_062F;
				}
			}
			this.WmCtlColorControl(ref m);
			return;
			IL_0432:
			if (!Control.ReflectMessageInternal(m.LParam, ref m))
			{
				this.DefWndProc(ref m);
				return;
			}
			return;
			IL_062F:
			if (m.Msg == Control.threadCallbackMessage && m.Msg != 0)
			{
				this.InvokeMarshaledCallbacks();
				return;
			}
			if (m.Msg == Control.WM_GETCONTROLNAME)
			{
				this.WmGetControlName(ref m);
				return;
			}
			if (m.Msg == Control.WM_GETCONTROLTYPE)
			{
				this.WmGetControlType(ref m);
				return;
			}
			if (Control.mouseWheelRoutingNeeded && m.Msg == Control.mouseWheelMessage)
			{
				Keys keys = Keys.None;
				keys |= ((UnsafeNativeMethods.GetKeyState(17) < 0) ? Keys.Back : Keys.None);
				keys |= ((UnsafeNativeMethods.GetKeyState(16) < 0) ? Keys.MButton : Keys.None);
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				if (focus == IntPtr.Zero)
				{
					this.SendMessage(m.Msg, (IntPtr)(((int)(long)m.WParam << 16) | (int)keys), m.LParam);
				}
				else
				{
					IntPtr intPtr = IntPtr.Zero;
					IntPtr desktopWindow = UnsafeNativeMethods.GetDesktopWindow();
					while (intPtr == IntPtr.Zero && focus != IntPtr.Zero && focus != desktopWindow)
					{
						intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(null, focus), 522, ((int)(long)m.WParam << 16) | (int)keys, m.LParam);
						focus = UnsafeNativeMethods.GetParent(new HandleRef(null, focus));
					}
				}
			}
			if (m.Msg == NativeMethods.WM_MOUSEENTER)
			{
				this.WmMouseEnter(ref m);
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000205C9 File Offset: 0x0001F5C9
		private void WndProcException(Exception e)
		{
			Application.OnThreadException(e);
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x000205D4 File Offset: 0x0001F5D4
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				Control.ControlCollection controlCollection = (Control.ControlCollection)this.Properties.GetObject(Control.PropControlsCollection);
				if (controlCollection == null)
				{
					return ArrangedElementCollection.Empty;
				}
				return controlCollection;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x00020601 File Offset: 0x0001F601
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ParentInternal;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x00020609 File Offset: 0x0001F609
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.GetState(2);
			}
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00020612 File Offset: 0x0001F612
		void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string affectedProperty)
		{
			this.PerformLayout(new LayoutEventArgs(affectedElement, affectedProperty));
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00020621 File Offset: 0x0001F621
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0002062C File Offset: 0x0001F62C
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			ISite site = this.Site;
			IComponentChangeService componentChangeService = null;
			PropertyDescriptor propertyDescriptor = null;
			PropertyDescriptor propertyDescriptor2 = null;
			bool flag = false;
			bool flag2 = false;
			if (site != null && site.DesignMode)
			{
				componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(this)[PropertyNames.Size];
					propertyDescriptor2 = TypeDescriptor.GetProperties(this)[PropertyNames.Location];
					try
					{
						if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly && (bounds.Width != this.Width || bounds.Height != this.Height))
						{
							if (!(site is INestedSite))
							{
								componentChangeService.OnComponentChanging(this, propertyDescriptor);
							}
							flag = true;
						}
						if (propertyDescriptor2 != null && !propertyDescriptor2.IsReadOnly && (bounds.X != this.x || bounds.Y != this.y))
						{
							if (!(site is INestedSite))
							{
								componentChangeService.OnComponentChanging(this, propertyDescriptor2);
							}
							flag2 = true;
						}
					}
					catch (InvalidOperationException)
					{
					}
				}
			}
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
			if (site != null && componentChangeService != null)
			{
				try
				{
					if (flag)
					{
						componentChangeService.OnComponentChanged(this, propertyDescriptor, null, null);
					}
					if (flag2)
					{
						componentChangeService.OnComponentChanged(this, propertyDescriptor2, null, null);
					}
				}
				catch (InvalidOperationException)
				{
				}
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00020780 File Offset: 0x0001F780
		void IDropTarget.OnDragEnter(DragEventArgs drgEvent)
		{
			this.OnDragEnter(drgEvent);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00020789 File Offset: 0x0001F789
		void IDropTarget.OnDragOver(DragEventArgs drgEvent)
		{
			this.OnDragOver(drgEvent);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00020792 File Offset: 0x0001F792
		void IDropTarget.OnDragLeave(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0002079B File Offset: 0x0001F79B
		void IDropTarget.OnDragDrop(DragEventArgs drgEvent)
		{
			this.OnDragDrop(drgEvent);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000207A4 File Offset: 0x0001F7A4
		void ISupportOleDropSource.OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEventArgs)
		{
			this.OnGiveFeedback(giveFeedbackEventArgs);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x000207AD File Offset: 0x0001F7AD
		void ISupportOleDropSource.OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEventArgs)
		{
			this.OnQueryContinueDrag(queryContinueDragEventArgs);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000207B8 File Offset: 0x0001F7B8
		int UnsafeNativeMethods.IOleControl.GetControlInfo(NativeMethods.tagCONTROLINFO pCI)
		{
			pCI.cb = Marshal.SizeOf(typeof(NativeMethods.tagCONTROLINFO));
			pCI.hAccel = IntPtr.Zero;
			pCI.cAccel = 0;
			pCI.dwFlags = 0;
			if (this.IsInputKey(Keys.Return))
			{
				pCI.dwFlags |= 1;
			}
			if (this.IsInputKey(Keys.Escape))
			{
				pCI.dwFlags |= 2;
			}
			this.ActiveXInstance.GetControlInfo(pCI);
			return 0;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00020831 File Offset: 0x0001F831
		int UnsafeNativeMethods.IOleControl.OnMnemonic(ref NativeMethods.MSG pMsg)
		{
			this.ProcessMnemonic((char)(int)pMsg.wParam);
			return 0;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00020847 File Offset: 0x0001F847
		int UnsafeNativeMethods.IOleControl.OnAmbientPropertyChange(int dispID)
		{
			this.ActiveXInstance.OnAmbientPropertyChange(dispID);
			return 0;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00020856 File Offset: 0x0001F856
		int UnsafeNativeMethods.IOleControl.FreezeEvents(int bFreeze)
		{
			this.ActiveXInstance.EventsFrozen = bFreeze != 0;
			return 0;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0002086B File Offset: 0x0001F86B
		int UnsafeNativeMethods.IOleInPlaceActiveObject.GetWindow(out IntPtr hwnd)
		{
			return ((UnsafeNativeMethods.IOleInPlaceObject)this).GetWindow(out hwnd);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00020874 File Offset: 0x0001F874
		void UnsafeNativeMethods.IOleInPlaceActiveObject.ContextSensitiveHelp(int fEnterMode)
		{
			((UnsafeNativeMethods.IOleInPlaceObject)this).ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0002087D File Offset: 0x0001F87D
		int UnsafeNativeMethods.IOleInPlaceActiveObject.TranslateAccelerator(ref NativeMethods.MSG lpmsg)
		{
			return this.ActiveXInstance.TranslateAccelerator(ref lpmsg);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0002088B File Offset: 0x0001F88B
		void UnsafeNativeMethods.IOleInPlaceActiveObject.OnFrameWindowActivate(bool fActivate)
		{
			this.OnFrameWindowActivate(fActivate);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00020894 File Offset: 0x0001F894
		void UnsafeNativeMethods.IOleInPlaceActiveObject.OnDocWindowActivate(int fActivate)
		{
			this.ActiveXInstance.OnDocWindowActivate(fActivate);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000208A2 File Offset: 0x0001F8A2
		void UnsafeNativeMethods.IOleInPlaceActiveObject.ResizeBorder(NativeMethods.COMRECT prcBorder, UnsafeNativeMethods.IOleInPlaceUIWindow pUIWindow, bool fFrameWindow)
		{
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000208A4 File Offset: 0x0001F8A4
		void UnsafeNativeMethods.IOleInPlaceActiveObject.EnableModeless(int fEnable)
		{
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000208A8 File Offset: 0x0001F8A8
		int UnsafeNativeMethods.IOleInPlaceObject.GetWindow(out IntPtr hwnd)
		{
			return this.ActiveXInstance.GetWindow(out hwnd);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000208C3 File Offset: 0x0001F8C3
		void UnsafeNativeMethods.IOleInPlaceObject.ContextSensitiveHelp(int fEnterMode)
		{
			if (fEnterMode != 0)
			{
				this.OnHelpRequested(new HelpEventArgs(Control.MousePosition));
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000208D8 File Offset: 0x0001F8D8
		void UnsafeNativeMethods.IOleInPlaceObject.InPlaceDeactivate()
		{
			this.ActiveXInstance.InPlaceDeactivate();
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000208E5 File Offset: 0x0001F8E5
		int UnsafeNativeMethods.IOleInPlaceObject.UIDeactivate()
		{
			return this.ActiveXInstance.UIDeactivate();
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x000208F2 File Offset: 0x0001F8F2
		void UnsafeNativeMethods.IOleInPlaceObject.SetObjectRects(NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect)
		{
			this.ActiveXInstance.SetObjectRects(lprcPosRect, lprcClipRect);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00020901 File Offset: 0x0001F901
		void UnsafeNativeMethods.IOleInPlaceObject.ReactivateAndUndo()
		{
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00020903 File Offset: 0x0001F903
		int UnsafeNativeMethods.IOleObject.SetClientSite(UnsafeNativeMethods.IOleClientSite pClientSite)
		{
			this.ActiveXInstance.SetClientSite(pClientSite);
			return 0;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00020912 File Offset: 0x0001F912
		UnsafeNativeMethods.IOleClientSite UnsafeNativeMethods.IOleObject.GetClientSite()
		{
			return this.ActiveXInstance.GetClientSite();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0002091F File Offset: 0x0001F91F
		int UnsafeNativeMethods.IOleObject.SetHostNames(string szContainerApp, string szContainerObj)
		{
			return 0;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00020922 File Offset: 0x0001F922
		int UnsafeNativeMethods.IOleObject.Close(int dwSaveOption)
		{
			this.ActiveXInstance.Close(dwSaveOption);
			return 0;
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00020931 File Offset: 0x0001F931
		int UnsafeNativeMethods.IOleObject.SetMoniker(int dwWhichMoniker, object pmk)
		{
			return -2147467263;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00020938 File Offset: 0x0001F938
		int UnsafeNativeMethods.IOleObject.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00020942 File Offset: 0x0001F942
		int UnsafeNativeMethods.IOleObject.InitFromData(IDataObject pDataObject, int fCreation, int dwReserved)
		{
			return -2147467263;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00020949 File Offset: 0x0001F949
		int UnsafeNativeMethods.IOleObject.GetClipboardData(int dwReserved, out IDataObject data)
		{
			data = null;
			return -2147467263;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00020954 File Offset: 0x0001F954
		int UnsafeNativeMethods.IOleObject.DoVerb(int iVerb, IntPtr lpmsg, UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, NativeMethods.COMRECT lprcPosRect)
		{
			short num = (short)iVerb;
			iVerb = (int)num;
			try
			{
				this.ActiveXInstance.DoVerb(iVerb, lpmsg, pActiveSite, lindex, hwndParent, lprcPosRect);
			}
			catch (Exception)
			{
				throw;
			}
			return 0;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00020994 File Offset: 0x0001F994
		int UnsafeNativeMethods.IOleObject.EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e)
		{
			return Control.ActiveXImpl.EnumVerbs(out e);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0002099C File Offset: 0x0001F99C
		int UnsafeNativeMethods.IOleObject.OleUpdate()
		{
			return 0;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0002099F File Offset: 0x0001F99F
		int UnsafeNativeMethods.IOleObject.IsUpToDate()
		{
			return 0;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000209A2 File Offset: 0x0001F9A2
		int UnsafeNativeMethods.IOleObject.GetUserClassID(ref Guid pClsid)
		{
			pClsid = base.GetType().GUID;
			return 0;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x000209B6 File Offset: 0x0001F9B6
		int UnsafeNativeMethods.IOleObject.GetUserType(int dwFormOfType, out string userType)
		{
			if (dwFormOfType == 1)
			{
				userType = base.GetType().FullName;
			}
			else
			{
				userType = base.GetType().Name;
			}
			return 0;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000209D9 File Offset: 0x0001F9D9
		int UnsafeNativeMethods.IOleObject.SetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.SetExtent(dwDrawAspect, pSizel);
			return 0;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000209E9 File Offset: 0x0001F9E9
		int UnsafeNativeMethods.IOleObject.GetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.GetExtent(dwDrawAspect, pSizel);
			return 0;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000209F9 File Offset: 0x0001F9F9
		int UnsafeNativeMethods.IOleObject.Advise(IAdviseSink pAdvSink, out int cookie)
		{
			cookie = this.ActiveXInstance.Advise(pAdvSink);
			return 0;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00020A0A File Offset: 0x0001FA0A
		int UnsafeNativeMethods.IOleObject.Unadvise(int dwConnection)
		{
			this.ActiveXInstance.Unadvise(dwConnection);
			return 0;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00020A19 File Offset: 0x0001FA19
		int UnsafeNativeMethods.IOleObject.EnumAdvise(out IEnumSTATDATA e)
		{
			e = null;
			return -2147467263;
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00020A24 File Offset: 0x0001FA24
		int UnsafeNativeMethods.IOleObject.GetMiscStatus(int dwAspect, out int cookie)
		{
			if ((dwAspect & 1) != 0)
			{
				int num = 131456;
				if (this.GetStyle(ControlStyles.ResizeRedraw))
				{
					num |= 1;
				}
				if (this is IButtonControl)
				{
					num |= 4096;
				}
				cookie = num;
				return 0;
			}
			cookie = 0;
			return -2147221397;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00020A69 File Offset: 0x0001FA69
		int UnsafeNativeMethods.IOleObject.SetColorScheme(NativeMethods.tagLOGPALETTE pLogpal)
		{
			return 0;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00020A6C File Offset: 0x0001FA6C
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			return ((UnsafeNativeMethods.IOleInPlaceObject)this).GetWindow(out hwnd);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00020A75 File Offset: 0x0001FA75
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			((UnsafeNativeMethods.IOleInPlaceObject)this).ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00020A7E File Offset: 0x0001FA7E
		void UnsafeNativeMethods.IPersist.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00020A91 File Offset: 0x0001FA91
		void UnsafeNativeMethods.IPersistPropertyBag.InitNew()
		{
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00020A93 File Offset: 0x0001FA93
		void UnsafeNativeMethods.IPersistPropertyBag.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00020AA6 File Offset: 0x0001FAA6
		void UnsafeNativeMethods.IPersistPropertyBag.Load(UnsafeNativeMethods.IPropertyBag pPropBag, UnsafeNativeMethods.IErrorLog pErrorLog)
		{
			this.ActiveXInstance.Load(pPropBag, pErrorLog);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00020AB5 File Offset: 0x0001FAB5
		void UnsafeNativeMethods.IPersistPropertyBag.Save(UnsafeNativeMethods.IPropertyBag pPropBag, bool fClearDirty, bool fSaveAllProperties)
		{
			this.ActiveXInstance.Save(pPropBag, fClearDirty, fSaveAllProperties);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00020AC5 File Offset: 0x0001FAC5
		void UnsafeNativeMethods.IPersistStorage.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00020AD8 File Offset: 0x0001FAD8
		int UnsafeNativeMethods.IPersistStorage.IsDirty()
		{
			return this.ActiveXInstance.IsDirty();
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00020AE5 File Offset: 0x0001FAE5
		void UnsafeNativeMethods.IPersistStorage.InitNew(UnsafeNativeMethods.IStorage pstg)
		{
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00020AE7 File Offset: 0x0001FAE7
		int UnsafeNativeMethods.IPersistStorage.Load(UnsafeNativeMethods.IStorage pstg)
		{
			this.ActiveXInstance.Load(pstg);
			return 0;
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00020AF6 File Offset: 0x0001FAF6
		void UnsafeNativeMethods.IPersistStorage.Save(UnsafeNativeMethods.IStorage pstg, bool fSameAsLoad)
		{
			this.ActiveXInstance.Save(pstg, fSameAsLoad);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00020B05 File Offset: 0x0001FB05
		void UnsafeNativeMethods.IPersistStorage.SaveCompleted(UnsafeNativeMethods.IStorage pStgNew)
		{
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00020B07 File Offset: 0x0001FB07
		void UnsafeNativeMethods.IPersistStorage.HandsOffStorage()
		{
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00020B09 File Offset: 0x0001FB09
		void UnsafeNativeMethods.IPersistStreamInit.GetClassID(out Guid pClassID)
		{
			pClassID = base.GetType().GUID;
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00020B1C File Offset: 0x0001FB1C
		int UnsafeNativeMethods.IPersistStreamInit.IsDirty()
		{
			return this.ActiveXInstance.IsDirty();
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00020B29 File Offset: 0x0001FB29
		void UnsafeNativeMethods.IPersistStreamInit.Load(UnsafeNativeMethods.IStream pstm)
		{
			this.ActiveXInstance.Load(pstm);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00020B37 File Offset: 0x0001FB37
		void UnsafeNativeMethods.IPersistStreamInit.Save(UnsafeNativeMethods.IStream pstm, bool fClearDirty)
		{
			this.ActiveXInstance.Save(pstm, fClearDirty);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00020B46 File Offset: 0x0001FB46
		void UnsafeNativeMethods.IPersistStreamInit.GetSizeMax(long pcbSize)
		{
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00020B48 File Offset: 0x0001FB48
		void UnsafeNativeMethods.IPersistStreamInit.InitNew()
		{
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00020B4A File Offset: 0x0001FB4A
		void UnsafeNativeMethods.IQuickActivate.QuickActivate(UnsafeNativeMethods.tagQACONTAINER pQaContainer, UnsafeNativeMethods.tagQACONTROL pQaControl)
		{
			this.ActiveXInstance.QuickActivate(pQaContainer, pQaControl);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00020B59 File Offset: 0x0001FB59
		void UnsafeNativeMethods.IQuickActivate.SetContentExtent(NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.SetExtent(1, pSizel);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00020B68 File Offset: 0x0001FB68
		void UnsafeNativeMethods.IQuickActivate.GetContentExtent(NativeMethods.tagSIZEL pSizel)
		{
			this.ActiveXInstance.GetExtent(1, pSizel);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00020B78 File Offset: 0x0001FB78
		int UnsafeNativeMethods.IViewObject.Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT lprcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
		{
			try
			{
				this.ActiveXInstance.Draw(dwDrawAspect, lindex, pvAspect, ptd, hdcTargetDev, hdcDraw, lprcBounds, lprcWBounds, pfnContinue, dwContinue);
			}
			catch (ExternalException ex)
			{
				return ex.ErrorCode;
			}
			return 0;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00020BC0 File Offset: 0x0001FBC0
		int UnsafeNativeMethods.IViewObject.GetColorSet(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, NativeMethods.tagLOGPALETTE ppColorSet)
		{
			return -2147467263;
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00020BC7 File Offset: 0x0001FBC7
		int UnsafeNativeMethods.IViewObject.Freeze(int dwDrawAspect, int lindex, IntPtr pvAspect, IntPtr pdwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00020BCE File Offset: 0x0001FBCE
		int UnsafeNativeMethods.IViewObject.Unfreeze(int dwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00020BD5 File Offset: 0x0001FBD5
		void UnsafeNativeMethods.IViewObject.SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
		{
			this.ActiveXInstance.SetAdvise(aspects, advf, pAdvSink);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00020BE5 File Offset: 0x0001FBE5
		void UnsafeNativeMethods.IViewObject.GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
		{
			this.ActiveXInstance.GetAdvise(paspects, padvf, pAdvSink);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00020BF8 File Offset: 0x0001FBF8
		void UnsafeNativeMethods.IViewObject2.Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT lprcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
		{
			this.ActiveXInstance.Draw(dwDrawAspect, lindex, pvAspect, ptd, hdcTargetDev, hdcDraw, lprcBounds, lprcWBounds, pfnContinue, dwContinue);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00020C21 File Offset: 0x0001FC21
		int UnsafeNativeMethods.IViewObject2.GetColorSet(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, NativeMethods.tagLOGPALETTE ppColorSet)
		{
			return -2147467263;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00020C28 File Offset: 0x0001FC28
		int UnsafeNativeMethods.IViewObject2.Freeze(int dwDrawAspect, int lindex, IntPtr pvAspect, IntPtr pdwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00020C2F File Offset: 0x0001FC2F
		int UnsafeNativeMethods.IViewObject2.Unfreeze(int dwFreeze)
		{
			return -2147467263;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00020C36 File Offset: 0x0001FC36
		void UnsafeNativeMethods.IViewObject2.SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
		{
			this.ActiveXInstance.SetAdvise(aspects, advf, pAdvSink);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00020C46 File Offset: 0x0001FC46
		void UnsafeNativeMethods.IViewObject2.GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
		{
			this.ActiveXInstance.GetAdvise(paspects, padvf, pAdvSink);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00020C56 File Offset: 0x0001FC56
		void UnsafeNativeMethods.IViewObject2.GetExtent(int dwDrawAspect, int lindex, NativeMethods.tagDVTARGETDEVICE ptd, NativeMethods.tagSIZEL lpsizel)
		{
			((UnsafeNativeMethods.IOleObject)this).GetExtent(dwDrawAspect, lpsizel);
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x00020C64 File Offset: 0x0001FC64
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x00020CA8 File Offset: 0x0001FCA8
		internal ImeMode CachedImeMode
		{
			get
			{
				bool flag;
				ImeMode imeMode = (ImeMode)this.Properties.GetInteger(Control.PropImeMode, out flag);
				if (!flag)
				{
					imeMode = this.DefaultImeMode;
				}
				if (imeMode == ImeMode.Inherit)
				{
					Control parentInternal = this.ParentInternal;
					if (parentInternal != null)
					{
						imeMode = parentInternal.CachedImeMode;
					}
					else
					{
						imeMode = ImeMode.NoControl;
					}
				}
				return imeMode;
			}
			set
			{
				this.Properties.SetInteger(Control.PropImeMode, (int)value);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x00020CBB File Offset: 0x0001FCBB
		protected virtual bool CanEnableIme
		{
			get
			{
				return this.ImeSupported;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001695 RID: 5781 RVA: 0x00020CC3 File Offset: 0x0001FCC3
		internal ImeMode CurrentImeContextMode
		{
			get
			{
				if (this.IsHandleCreated)
				{
					return ImeContext.GetImeMode(this.Handle);
				}
				return ImeMode.Inherit;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x00020CDA File Offset: 0x0001FCDA
		protected virtual ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Inherit;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x00020CE0 File Offset: 0x0001FCE0
		// (set) Token: 0x06001698 RID: 5784 RVA: 0x00020D01 File Offset: 0x0001FD01
		internal int DisableImeModeChangedCount
		{
			get
			{
				bool flag;
				return this.Properties.GetInteger(Control.PropDisableImeModeChangedCount, out flag);
			}
			set
			{
				this.Properties.SetInteger(Control.PropDisableImeModeChangedCount, value);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x00020D14 File Offset: 0x0001FD14
		// (set) Token: 0x0600169A RID: 5786 RVA: 0x00020D28 File Offset: 0x0001FD28
		private static bool IgnoreWmImeNotify
		{
			get
			{
				return Control.ignoreWmImeNotify;
			}
			set
			{
				Control.ignoreWmImeNotify = value;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600169B RID: 5787 RVA: 0x00020D30 File Offset: 0x0001FD30
		// (set) Token: 0x0600169C RID: 5788 RVA: 0x00020D4C File Offset: 0x0001FD4C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[AmbientValue(ImeMode.Inherit)]
		[SRDescription("ControlIMEModeDescr")]
		public ImeMode ImeMode
		{
			get
			{
				ImeMode imeMode = this.ImeModeBase;
				if (imeMode == ImeMode.OnHalf)
				{
					imeMode = ImeMode.On;
				}
				return imeMode;
			}
			set
			{
				this.ImeModeBase = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x00020D58 File Offset: 0x0001FD58
		// (set) Token: 0x0600169E RID: 5790 RVA: 0x00020D70 File Offset: 0x0001FD70
		protected virtual ImeMode ImeModeBase
		{
			get
			{
				return this.CachedImeMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 12))
				{
					throw new InvalidEnumArgumentException("ImeMode", (int)value, typeof(ImeMode));
				}
				ImeMode cachedImeMode = this.CachedImeMode;
				this.CachedImeMode = value;
				if (cachedImeMode != value)
				{
					Control control = null;
					if (!base.DesignMode && ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
					{
						if (this.Focused)
						{
							control = this;
						}
						else if (this.ContainsFocus)
						{
							control = Control.FromChildHandleInternal(UnsafeNativeMethods.GetFocus());
						}
						if (control != null && control.CanEnableIme)
						{
							this.DisableImeModeChangedCount++;
							try
							{
								control.UpdateImeContextMode();
							}
							finally
							{
								this.DisableImeModeChangedCount--;
							}
						}
					}
					this.VerifyImeModeChanged(cachedImeMode, this.CachedImeMode);
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x00020E38 File Offset: 0x0001FE38
		private bool ImeSupported
		{
			get
			{
				return this.DefaultImeMode != ImeMode.Disable;
			}
		}

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x060016A0 RID: 5792 RVA: 0x00020E46 File Offset: 0x0001FE46
		// (remove) Token: 0x060016A1 RID: 5793 RVA: 0x00020E59 File Offset: 0x0001FE59
		[WinCategory("Behavior")]
		[SRDescription("ControlOnImeModeChangedDescr")]
		public event EventHandler ImeModeChanged
		{
			add
			{
				base.Events.AddHandler(Control.EventImeModeChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Control.EventImeModeChanged, value);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00020E6C File Offset: 0x0001FE6C
		// (set) Token: 0x060016A3 RID: 5795 RVA: 0x00020E7E File Offset: 0x0001FE7E
		internal int ImeWmCharsToIgnore
		{
			get
			{
				return this.Properties.GetInteger(Control.PropImeWmCharsToIgnore);
			}
			set
			{
				if (this.ImeWmCharsToIgnore != -1)
				{
					this.Properties.SetInteger(Control.PropImeWmCharsToIgnore, value);
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00020E9C File Offset: 0x0001FE9C
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x00020EC9 File Offset: 0x0001FEC9
		private bool LastCanEnableIme
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(Control.PropLastCanEnableIme, out flag);
				flag = !flag || integer == 1;
				return flag;
			}
			set
			{
				this.Properties.SetInteger(Control.PropLastCanEnableIme, value ? 1 : 0);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00020EE4 File Offset: 0x0001FEE4
		// (set) Token: 0x060016A7 RID: 5799 RVA: 0x00020F48 File Offset: 0x0001FF48
		private protected static ImeMode PropagatingImeMode
		{
			protected get
			{
				if (Control.propagatingImeMode == ImeMode.Inherit)
				{
					ImeMode imeMode = ImeMode.Inherit;
					IntPtr intPtr = UnsafeNativeMethods.GetFocus();
					if (intPtr != IntPtr.Zero)
					{
						imeMode = ImeContext.GetImeMode(intPtr);
						if (imeMode == ImeMode.Disable)
						{
							intPtr = UnsafeNativeMethods.GetAncestor(new HandleRef(null, intPtr), 2);
							if (intPtr != IntPtr.Zero)
							{
								imeMode = ImeContext.GetImeMode(intPtr);
							}
						}
					}
					Control.PropagatingImeMode = imeMode;
				}
				return Control.propagatingImeMode;
			}
			private set
			{
				if (Control.propagatingImeMode != value)
				{
					if (value == ImeMode.NoControl || value == ImeMode.Disable)
					{
						return;
					}
					Control.propagatingImeMode = value;
				}
			}
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00020F70 File Offset: 0x0001FF70
		internal void UpdateImeContextMode()
		{
			ImeMode[] inputLanguageTable = ImeModeConversion.InputLanguageTable;
			if (!base.DesignMode && inputLanguageTable != ImeModeConversion.UnsupportedTable && this.Focused)
			{
				ImeMode imeMode = ImeMode.Disable;
				ImeMode cachedImeMode = this.CachedImeMode;
				if (this.ImeSupported && this.CanEnableIme)
				{
					imeMode = ((cachedImeMode == ImeMode.NoControl) ? Control.PropagatingImeMode : cachedImeMode);
				}
				if (this.CurrentImeContextMode != imeMode && imeMode != ImeMode.Inherit)
				{
					this.DisableImeModeChangedCount++;
					ImeMode imeMode2 = Control.PropagatingImeMode;
					try
					{
						ImeContext.SetImeStatus(imeMode, this.Handle);
					}
					finally
					{
						this.DisableImeModeChangedCount--;
						if (imeMode == ImeMode.Disable && inputLanguageTable == ImeModeConversion.ChineseTable)
						{
							Control.PropagatingImeMode = imeMode2;
						}
					}
					if (cachedImeMode == ImeMode.NoControl)
					{
						if (this.CanEnableIme)
						{
							Control.PropagatingImeMode = this.CurrentImeContextMode;
							return;
						}
					}
					else
					{
						if (this.CanEnableIme)
						{
							this.CachedImeMode = this.CurrentImeContextMode;
						}
						this.VerifyImeModeChanged(imeMode, this.CachedImeMode);
					}
				}
			}
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00021064 File Offset: 0x00020064
		private void VerifyImeModeChanged(ImeMode oldMode, ImeMode newMode)
		{
			if (this.ImeSupported && this.DisableImeModeChangedCount == 0 && newMode != ImeMode.NoControl && oldMode != newMode)
			{
				this.OnImeModeChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00021088 File Offset: 0x00020088
		internal void VerifyImeRestrictedModeChanged()
		{
			bool canEnableIme = this.CanEnableIme;
			if (this.LastCanEnableIme != canEnableIme)
			{
				if (this.Focused)
				{
					this.DisableImeModeChangedCount++;
					try
					{
						this.UpdateImeContextMode();
					}
					finally
					{
						this.DisableImeModeChangedCount--;
					}
				}
				ImeMode imeMode = this.CachedImeMode;
				ImeMode imeMode2 = ImeMode.Disable;
				if (canEnableIme)
				{
					imeMode2 = imeMode;
					imeMode = ImeMode.Disable;
				}
				this.VerifyImeModeChanged(imeMode, imeMode2);
				this.LastCanEnableIme = canEnableIme;
			}
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00021104 File Offset: 0x00020104
		internal void OnImeContextStatusChanged(IntPtr handle)
		{
			ImeMode imeMode = ImeContext.GetImeMode(handle);
			if (imeMode != ImeMode.Inherit)
			{
				ImeMode cachedImeMode = this.CachedImeMode;
				if (this.CanEnableIme)
				{
					if (cachedImeMode != ImeMode.NoControl)
					{
						this.CachedImeMode = imeMode;
						this.VerifyImeModeChanged(cachedImeMode, this.CachedImeMode);
						return;
					}
					Control.PropagatingImeMode = imeMode;
				}
			}
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0002114C File Offset: 0x0002014C
		protected virtual void OnImeModeChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventImeModeChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0002117A File Offset: 0x0002017A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetImeMode()
		{
			this.ImeMode = this.DefaultImeMode;
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00021188 File Offset: 0x00020188
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeImeMode()
		{
			bool flag;
			int integer = this.Properties.GetInteger(Control.PropImeMode, out flag);
			return flag && integer != (int)this.DefaultImeMode;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000211BC File Offset: 0x000201BC
		private void WmInputLangChange(ref Message m)
		{
			this.UpdateImeContextMode();
			if (ImeModeConversion.InputLanguageTable == ImeModeConversion.UnsupportedTable)
			{
				Control.PropagatingImeMode = ImeMode.Off;
			}
			Form form = this.FindFormInternal();
			if (form != null)
			{
				InputLanguageChangedEventArgs inputLanguageChangedEventArgs = InputLanguage.CreateInputLanguageChangedEventArgs(m);
				form.PerformOnInputLanguageChanged(inputLanguageChangedEventArgs);
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00021208 File Offset: 0x00020208
		private void WmInputLangChangeRequest(ref Message m)
		{
			InputLanguageChangingEventArgs inputLanguageChangingEventArgs = InputLanguage.CreateInputLanguageChangingEventArgs(m);
			Form form = this.FindFormInternal();
			if (form != null)
			{
				form.PerformOnInputLanguageChanging(inputLanguageChangingEventArgs);
			}
			if (!inputLanguageChangingEventArgs.Cancel)
			{
				this.DefWndProc(ref m);
				return;
			}
			m.Result = IntPtr.Zero;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0002124D File Offset: 0x0002024D
		private void WmImeChar(ref Message m)
		{
			if (this.ProcessKeyEventArgs(ref m))
			{
				return;
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00021260 File Offset: 0x00020260
		private void WmImeEndComposition(ref Message m)
		{
			this.ImeWmCharsToIgnore = -1;
			this.DefWndProc(ref m);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00021270 File Offset: 0x00020270
		private void WmImeNotify(ref Message m)
		{
			if (this.ImeSupported && ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable && !Control.IgnoreWmImeNotify)
			{
				int num = (int)m.WParam;
				if (num == 6 || num == 8)
				{
					this.OnImeContextStatusChanged(this.Handle);
				}
			}
			this.DefWndProc(ref m);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x000212BF File Offset: 0x000202BF
		internal void WmImeSetFocus()
		{
			if (ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable)
			{
				this.UpdateImeContextMode();
			}
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000212D3 File Offset: 0x000202D3
		private void WmImeStartComposition(ref Message m)
		{
			this.Properties.SetInteger(Control.PropImeWmCharsToIgnore, 0);
			this.DefWndProc(ref m);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000212F0 File Offset: 0x000202F0
		private void WmImeKillFocus()
		{
			Control topMostParent = this.TopMostParent;
			Form form = topMostParent as Form;
			if ((form == null || form.Modal) && !topMostParent.ContainsFocus && Control.propagatingImeMode != ImeMode.Inherit)
			{
				Control.IgnoreWmImeNotify = true;
				try
				{
					ImeContext.SetImeStatus(Control.PropagatingImeMode, topMostParent.Handle);
					Control.PropagatingImeMode = ImeMode.Inherit;
				}
				finally
				{
					Control.IgnoreWmImeNotify = false;
				}
			}
		}

		// Token: 0x0400106E RID: 4206
		internal const int STATE_CREATED = 1;

		// Token: 0x0400106F RID: 4207
		internal const int STATE_VISIBLE = 2;

		// Token: 0x04001070 RID: 4208
		internal const int STATE_ENABLED = 4;

		// Token: 0x04001071 RID: 4209
		internal const int STATE_TABSTOP = 8;

		// Token: 0x04001072 RID: 4210
		internal const int STATE_RECREATE = 16;

		// Token: 0x04001073 RID: 4211
		internal const int STATE_MODAL = 32;

		// Token: 0x04001074 RID: 4212
		internal const int STATE_ALLOWDROP = 64;

		// Token: 0x04001075 RID: 4213
		internal const int STATE_DROPTARGET = 128;

		// Token: 0x04001076 RID: 4214
		internal const int STATE_NOZORDER = 256;

		// Token: 0x04001077 RID: 4215
		internal const int STATE_LAYOUTDEFERRED = 512;

		// Token: 0x04001078 RID: 4216
		internal const int STATE_USEWAITCURSOR = 1024;

		// Token: 0x04001079 RID: 4217
		internal const int STATE_DISPOSED = 2048;

		// Token: 0x0400107A RID: 4218
		internal const int STATE_DISPOSING = 4096;

		// Token: 0x0400107B RID: 4219
		internal const int STATE_MOUSEENTERPENDING = 8192;

		// Token: 0x0400107C RID: 4220
		internal const int STATE_TRACKINGMOUSEEVENT = 16384;

		// Token: 0x0400107D RID: 4221
		internal const int STATE_THREADMARSHALLPENDING = 32768;

		// Token: 0x0400107E RID: 4222
		internal const int STATE_SIZELOCKEDBYOS = 65536;

		// Token: 0x0400107F RID: 4223
		internal const int STATE_CAUSESVALIDATION = 131072;

		// Token: 0x04001080 RID: 4224
		internal const int STATE_CREATINGHANDLE = 262144;

		// Token: 0x04001081 RID: 4225
		internal const int STATE_TOPLEVEL = 524288;

		// Token: 0x04001082 RID: 4226
		internal const int STATE_ISACCESSIBLE = 1048576;

		// Token: 0x04001083 RID: 4227
		internal const int STATE_OWNCTLBRUSH = 2097152;

		// Token: 0x04001084 RID: 4228
		internal const int STATE_EXCEPTIONWHILEPAINTING = 4194304;

		// Token: 0x04001085 RID: 4229
		internal const int STATE_LAYOUTISDIRTY = 8388608;

		// Token: 0x04001086 RID: 4230
		internal const int STATE_CHECKEDHOST = 16777216;

		// Token: 0x04001087 RID: 4231
		internal const int STATE_HOSTEDINDIALOG = 33554432;

		// Token: 0x04001088 RID: 4232
		internal const int STATE_DOUBLECLICKFIRED = 67108864;

		// Token: 0x04001089 RID: 4233
		internal const int STATE_MOUSEPRESSED = 134217728;

		// Token: 0x0400108A RID: 4234
		internal const int STATE_VALIDATIONCANCELLED = 268435456;

		// Token: 0x0400108B RID: 4235
		internal const int STATE_PARENTRECREATING = 536870912;

		// Token: 0x0400108C RID: 4236
		internal const int STATE_MIRRORED = 1073741824;

		// Token: 0x0400108D RID: 4237
		private const int STATE2_HAVEINVOKED = 1;

		// Token: 0x0400108E RID: 4238
		private const int STATE2_SETSCROLLPOS = 2;

		// Token: 0x0400108F RID: 4239
		private const int STATE2_LISTENINGTOUSERPREFERENCECHANGED = 4;

		// Token: 0x04001090 RID: 4240
		internal const int STATE2_INTERESTEDINUSERPREFERENCECHANGED = 8;

		// Token: 0x04001091 RID: 4241
		internal const int STATE2_MAINTAINSOWNCAPTUREMODE = 16;

		// Token: 0x04001092 RID: 4242
		private const int STATE2_BECOMINGACTIVECONTROL = 32;

		// Token: 0x04001093 RID: 4243
		private const int STATE2_CLEARLAYOUTARGS = 64;

		// Token: 0x04001094 RID: 4244
		private const int STATE2_INPUTKEY = 128;

		// Token: 0x04001095 RID: 4245
		private const int STATE2_INPUTCHAR = 256;

		// Token: 0x04001096 RID: 4246
		private const int STATE2_UICUES = 512;

		// Token: 0x04001097 RID: 4247
		private const int STATE2_ISACTIVEX = 1024;

		// Token: 0x04001098 RID: 4248
		internal const int STATE2_USEPREFERREDSIZECACHE = 2048;

		// Token: 0x04001099 RID: 4249
		private const short PaintLayerBackground = 1;

		// Token: 0x0400109A RID: 4250
		private const short PaintLayerForeground = 2;

		// Token: 0x0400109B RID: 4251
		private const byte RequiredScalingEnabledMask = 16;

		// Token: 0x0400109C RID: 4252
		private const byte RequiredScalingMask = 15;

		// Token: 0x0400109D RID: 4253
		private const int UISTATE_FOCUS_CUES_MASK = 15;

		// Token: 0x0400109E RID: 4254
		private const int UISTATE_FOCUS_CUES_HIDDEN = 1;

		// Token: 0x0400109F RID: 4255
		private const int UISTATE_FOCUS_CUES_SHOW = 2;

		// Token: 0x040010A0 RID: 4256
		private const int UISTATE_KEYBOARD_CUES_MASK = 240;

		// Token: 0x040010A1 RID: 4257
		private const int UISTATE_KEYBOARD_CUES_HIDDEN = 16;

		// Token: 0x040010A2 RID: 4258
		private const int UISTATE_KEYBOARD_CUES_SHOW = 32;

		// Token: 0x040010A3 RID: 4259
		private const int ImeCharsToIgnoreDisabled = -1;

		// Token: 0x040010A4 RID: 4260
		private const int ImeCharsToIgnoreEnabled = 0;

		// Token: 0x040010A5 RID: 4261
		internal static readonly TraceSwitch ControlKeyboardRouting;

		// Token: 0x040010A6 RID: 4262
		internal static readonly TraceSwitch PaletteTracing;

		// Token: 0x040010A7 RID: 4263
		internal static readonly TraceSwitch FocusTracing;

		// Token: 0x040010A8 RID: 4264
		internal static readonly BooleanSwitch BufferPinkRect;

		// Token: 0x040010A9 RID: 4265
		private static int WM_GETCONTROLNAME = SafeNativeMethods.RegisterWindowMessage("WM_GETCONTROLNAME");

		// Token: 0x040010AA RID: 4266
		private static int WM_GETCONTROLTYPE = SafeNativeMethods.RegisterWindowMessage("WM_GETCONTROLTYPE");

		// Token: 0x040010AB RID: 4267
		private static readonly object EventAutoSizeChanged = new object();

		// Token: 0x040010AC RID: 4268
		private static readonly object EventKeyDown = new object();

		// Token: 0x040010AD RID: 4269
		private static readonly object EventKeyPress = new object();

		// Token: 0x040010AE RID: 4270
		private static readonly object EventKeyUp = new object();

		// Token: 0x040010AF RID: 4271
		private static readonly object EventMouseDown = new object();

		// Token: 0x040010B0 RID: 4272
		private static readonly object EventMouseEnter = new object();

		// Token: 0x040010B1 RID: 4273
		private static readonly object EventMouseLeave = new object();

		// Token: 0x040010B2 RID: 4274
		private static readonly object EventMouseHover = new object();

		// Token: 0x040010B3 RID: 4275
		private static readonly object EventMouseMove = new object();

		// Token: 0x040010B4 RID: 4276
		private static readonly object EventMouseUp = new object();

		// Token: 0x040010B5 RID: 4277
		private static readonly object EventMouseWheel = new object();

		// Token: 0x040010B6 RID: 4278
		private static readonly object EventClick = new object();

		// Token: 0x040010B7 RID: 4279
		private static readonly object EventClientSize = new object();

		// Token: 0x040010B8 RID: 4280
		private static readonly object EventDoubleClick = new object();

		// Token: 0x040010B9 RID: 4281
		private static readonly object EventMouseClick = new object();

		// Token: 0x040010BA RID: 4282
		private static readonly object EventMouseDoubleClick = new object();

		// Token: 0x040010BB RID: 4283
		private static readonly object EventMouseCaptureChanged = new object();

		// Token: 0x040010BC RID: 4284
		private static readonly object EventMove = new object();

		// Token: 0x040010BD RID: 4285
		private static readonly object EventResize = new object();

		// Token: 0x040010BE RID: 4286
		private static readonly object EventLayout = new object();

		// Token: 0x040010BF RID: 4287
		private static readonly object EventGotFocus = new object();

		// Token: 0x040010C0 RID: 4288
		private static readonly object EventLostFocus = new object();

		// Token: 0x040010C1 RID: 4289
		private static readonly object EventEnabledChanged = new object();

		// Token: 0x040010C2 RID: 4290
		private static readonly object EventEnter = new object();

		// Token: 0x040010C3 RID: 4291
		private static readonly object EventLeave = new object();

		// Token: 0x040010C4 RID: 4292
		private static readonly object EventHandleCreated = new object();

		// Token: 0x040010C5 RID: 4293
		private static readonly object EventHandleDestroyed = new object();

		// Token: 0x040010C6 RID: 4294
		private static readonly object EventVisibleChanged = new object();

		// Token: 0x040010C7 RID: 4295
		private static readonly object EventControlAdded = new object();

		// Token: 0x040010C8 RID: 4296
		private static readonly object EventControlRemoved = new object();

		// Token: 0x040010C9 RID: 4297
		private static readonly object EventChangeUICues = new object();

		// Token: 0x040010CA RID: 4298
		private static readonly object EventSystemColorsChanged = new object();

		// Token: 0x040010CB RID: 4299
		private static readonly object EventValidating = new object();

		// Token: 0x040010CC RID: 4300
		private static readonly object EventValidated = new object();

		// Token: 0x040010CD RID: 4301
		private static readonly object EventStyleChanged = new object();

		// Token: 0x040010CE RID: 4302
		private static readonly object EventImeModeChanged = new object();

		// Token: 0x040010CF RID: 4303
		private static readonly object EventHelpRequested = new object();

		// Token: 0x040010D0 RID: 4304
		private static readonly object EventPaint = new object();

		// Token: 0x040010D1 RID: 4305
		private static readonly object EventInvalidated = new object();

		// Token: 0x040010D2 RID: 4306
		private static readonly object EventQueryContinueDrag = new object();

		// Token: 0x040010D3 RID: 4307
		private static readonly object EventGiveFeedback = new object();

		// Token: 0x040010D4 RID: 4308
		private static readonly object EventDragEnter = new object();

		// Token: 0x040010D5 RID: 4309
		private static readonly object EventDragLeave = new object();

		// Token: 0x040010D6 RID: 4310
		private static readonly object EventDragOver = new object();

		// Token: 0x040010D7 RID: 4311
		private static readonly object EventDragDrop = new object();

		// Token: 0x040010D8 RID: 4312
		private static readonly object EventQueryAccessibilityHelp = new object();

		// Token: 0x040010D9 RID: 4313
		private static readonly object EventBackgroundImage = new object();

		// Token: 0x040010DA RID: 4314
		private static readonly object EventBackgroundImageLayout = new object();

		// Token: 0x040010DB RID: 4315
		private static readonly object EventBindingContext = new object();

		// Token: 0x040010DC RID: 4316
		private static readonly object EventBackColor = new object();

		// Token: 0x040010DD RID: 4317
		private static readonly object EventParent = new object();

		// Token: 0x040010DE RID: 4318
		private static readonly object EventVisible = new object();

		// Token: 0x040010DF RID: 4319
		private static readonly object EventText = new object();

		// Token: 0x040010E0 RID: 4320
		private static readonly object EventTabStop = new object();

		// Token: 0x040010E1 RID: 4321
		private static readonly object EventTabIndex = new object();

		// Token: 0x040010E2 RID: 4322
		private static readonly object EventSize = new object();

		// Token: 0x040010E3 RID: 4323
		private static readonly object EventRightToLeft = new object();

		// Token: 0x040010E4 RID: 4324
		private static readonly object EventLocation = new object();

		// Token: 0x040010E5 RID: 4325
		private static readonly object EventForeColor = new object();

		// Token: 0x040010E6 RID: 4326
		private static readonly object EventFont = new object();

		// Token: 0x040010E7 RID: 4327
		private static readonly object EventEnabled = new object();

		// Token: 0x040010E8 RID: 4328
		private static readonly object EventDock = new object();

		// Token: 0x040010E9 RID: 4329
		private static readonly object EventCursor = new object();

		// Token: 0x040010EA RID: 4330
		private static readonly object EventContextMenu = new object();

		// Token: 0x040010EB RID: 4331
		private static readonly object EventContextMenuStrip = new object();

		// Token: 0x040010EC RID: 4332
		private static readonly object EventCausesValidation = new object();

		// Token: 0x040010ED RID: 4333
		private static readonly object EventRegionChanged = new object();

		// Token: 0x040010EE RID: 4334
		private static readonly object EventMarginChanged = new object();

		// Token: 0x040010EF RID: 4335
		internal static readonly object EventPaddingChanged = new object();

		// Token: 0x040010F0 RID: 4336
		private static readonly object EventPreviewKeyDown = new object();

		// Token: 0x040010F1 RID: 4337
		private static int mouseWheelMessage = 522;

		// Token: 0x040010F2 RID: 4338
		private static bool mouseWheelRoutingNeeded;

		// Token: 0x040010F3 RID: 4339
		private static bool mouseWheelInit;

		// Token: 0x040010F4 RID: 4340
		private static int threadCallbackMessage;

		// Token: 0x040010F5 RID: 4341
		private static bool checkForIllegalCrossThreadCalls = Debugger.IsAttached;

		// Token: 0x040010F6 RID: 4342
		private static ContextCallback invokeMarshaledCallbackHelperDelegate;

		// Token: 0x040010F7 RID: 4343
		[ThreadStatic]
		private static bool inCrossThreadSafeCall = false;

		// Token: 0x040010F8 RID: 4344
		[ThreadStatic]
		internal static HelpInfo currentHelpInfo = null;

		// Token: 0x040010F9 RID: 4345
		private static Control.FontHandleWrapper defaultFontHandleWrapper;

		// Token: 0x040010FA RID: 4346
		private static Font defaultFont;

		// Token: 0x040010FB RID: 4347
		private static readonly int PropName = PropertyStore.CreateKey();

		// Token: 0x040010FC RID: 4348
		private static readonly int PropBackBrush = PropertyStore.CreateKey();

		// Token: 0x040010FD RID: 4349
		private static readonly int PropFontHeight = PropertyStore.CreateKey();

		// Token: 0x040010FE RID: 4350
		private static readonly int PropCurrentAmbientFont = PropertyStore.CreateKey();

		// Token: 0x040010FF RID: 4351
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x04001100 RID: 4352
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x04001101 RID: 4353
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x04001102 RID: 4354
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x04001103 RID: 4355
		private static readonly int PropBackgroundImage = PropertyStore.CreateKey();

		// Token: 0x04001104 RID: 4356
		private static readonly int PropFontHandleWrapper = PropertyStore.CreateKey();

		// Token: 0x04001105 RID: 4357
		private static readonly int PropUserData = PropertyStore.CreateKey();

		// Token: 0x04001106 RID: 4358
		private static readonly int PropContextMenu = PropertyStore.CreateKey();

		// Token: 0x04001107 RID: 4359
		private static readonly int PropCursor = PropertyStore.CreateKey();

		// Token: 0x04001108 RID: 4360
		private static readonly int PropRegion = PropertyStore.CreateKey();

		// Token: 0x04001109 RID: 4361
		private static readonly int PropRightToLeft = PropertyStore.CreateKey();

		// Token: 0x0400110A RID: 4362
		private static readonly int PropBindings = PropertyStore.CreateKey();

		// Token: 0x0400110B RID: 4363
		private static readonly int PropBindingManager = PropertyStore.CreateKey();

		// Token: 0x0400110C RID: 4364
		private static readonly int PropAccessibleDefaultActionDescription = PropertyStore.CreateKey();

		// Token: 0x0400110D RID: 4365
		private static readonly int PropAccessibleDescription = PropertyStore.CreateKey();

		// Token: 0x0400110E RID: 4366
		private static readonly int PropAccessibility = PropertyStore.CreateKey();

		// Token: 0x0400110F RID: 4367
		private static readonly int PropNcAccessibility = PropertyStore.CreateKey();

		// Token: 0x04001110 RID: 4368
		private static readonly int PropAccessibleName = PropertyStore.CreateKey();

		// Token: 0x04001111 RID: 4369
		private static readonly int PropAccessibleRole = PropertyStore.CreateKey();

		// Token: 0x04001112 RID: 4370
		private static readonly int PropPaintingException = PropertyStore.CreateKey();

		// Token: 0x04001113 RID: 4371
		private static readonly int PropActiveXImpl = PropertyStore.CreateKey();

		// Token: 0x04001114 RID: 4372
		private static readonly int PropControlVersionInfo = PropertyStore.CreateKey();

		// Token: 0x04001115 RID: 4373
		private static readonly int PropBackgroundImageLayout = PropertyStore.CreateKey();

		// Token: 0x04001116 RID: 4374
		private static readonly int PropAccessibleHelpProvider = PropertyStore.CreateKey();

		// Token: 0x04001117 RID: 4375
		private static readonly int PropContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04001118 RID: 4376
		private static readonly int PropAutoScrollOffset = PropertyStore.CreateKey();

		// Token: 0x04001119 RID: 4377
		private static readonly int PropUseCompatibleTextRendering = PropertyStore.CreateKey();

		// Token: 0x0400111A RID: 4378
		private static readonly int PropImeWmCharsToIgnore = PropertyStore.CreateKey();

		// Token: 0x0400111B RID: 4379
		private static readonly int PropImeMode = PropertyStore.CreateKey();

		// Token: 0x0400111C RID: 4380
		private static readonly int PropDisableImeModeChangedCount = PropertyStore.CreateKey();

		// Token: 0x0400111D RID: 4381
		private static readonly int PropLastCanEnableIme = PropertyStore.CreateKey();

		// Token: 0x0400111E RID: 4382
		private static readonly int PropCacheTextCount = PropertyStore.CreateKey();

		// Token: 0x0400111F RID: 4383
		private static readonly int PropCacheTextField = PropertyStore.CreateKey();

		// Token: 0x04001120 RID: 4384
		private static readonly int PropAmbientPropertiesService = PropertyStore.CreateKey();

		// Token: 0x04001121 RID: 4385
		internal static bool UseCompatibleTextRenderingDefault = true;

		// Token: 0x04001122 RID: 4386
		private Control.ControlNativeWindow window;

		// Token: 0x04001123 RID: 4387
		private Control parent;

		// Token: 0x04001124 RID: 4388
		private Control reflectParent;

		// Token: 0x04001125 RID: 4389
		private CreateParams createParams;

		// Token: 0x04001126 RID: 4390
		private int x;

		// Token: 0x04001127 RID: 4391
		private int y;

		// Token: 0x04001128 RID: 4392
		private int width;

		// Token: 0x04001129 RID: 4393
		private int height;

		// Token: 0x0400112A RID: 4394
		private int clientWidth;

		// Token: 0x0400112B RID: 4395
		private int clientHeight;

		// Token: 0x0400112C RID: 4396
		private int state;

		// Token: 0x0400112D RID: 4397
		private int state2;

		// Token: 0x0400112E RID: 4398
		private ControlStyles controlStyle;

		// Token: 0x0400112F RID: 4399
		private int tabIndex;

		// Token: 0x04001130 RID: 4400
		private string text;

		// Token: 0x04001131 RID: 4401
		private byte layoutSuspendCount;

		// Token: 0x04001132 RID: 4402
		private byte requiredScaling;

		// Token: 0x04001133 RID: 4403
		private PropertyStore propertyStore;

		// Token: 0x04001134 RID: 4404
		private NativeMethods.TRACKMOUSEEVENT trackMouseEvent;

		// Token: 0x04001135 RID: 4405
		private short updateCount;

		// Token: 0x04001136 RID: 4406
		private LayoutEventArgs cachedLayoutEventArgs;

		// Token: 0x04001137 RID: 4407
		private Queue threadCallbackList;

		// Token: 0x04001138 RID: 4408
		private int uiCuesState;

		// Token: 0x04001139 RID: 4409
		private static ImeMode propagatingImeMode = ImeMode.Inherit;

		// Token: 0x0400113A RID: 4410
		private static bool ignoreWmImeNotify;

		// Token: 0x020001EB RID: 491
		private class ControlTabOrderHolder
		{
			// Token: 0x060016B7 RID: 5815 RVA: 0x0002135C File Offset: 0x0002035C
			internal ControlTabOrderHolder(int oldOrder, int newOrder, Control control)
			{
				this.oldOrder = oldOrder;
				this.newOrder = newOrder;
				this.control = control;
			}

			// Token: 0x0400113B RID: 4411
			internal readonly int oldOrder;

			// Token: 0x0400113C RID: 4412
			internal readonly int newOrder;

			// Token: 0x0400113D RID: 4413
			internal readonly Control control;
		}

		// Token: 0x020001EC RID: 492
		private class ControlTabOrderComparer : IComparer
		{
			// Token: 0x060016B8 RID: 5816 RVA: 0x0002137C File Offset: 0x0002037C
			int IComparer.Compare(object x, object y)
			{
				Control.ControlTabOrderHolder controlTabOrderHolder = (Control.ControlTabOrderHolder)x;
				Control.ControlTabOrderHolder controlTabOrderHolder2 = (Control.ControlTabOrderHolder)y;
				int num = controlTabOrderHolder.newOrder - controlTabOrderHolder2.newOrder;
				if (num == 0)
				{
					num = controlTabOrderHolder.oldOrder - controlTabOrderHolder2.oldOrder;
				}
				return num;
			}
		}

		// Token: 0x020001F1 RID: 497
		internal sealed class ControlNativeWindow : NativeWindow, IWindowTarget
		{
			// Token: 0x060016EC RID: 5868 RVA: 0x00022F9A File Offset: 0x00021F9A
			internal ControlNativeWindow(Control control)
			{
				this.control = control;
				this.target = this;
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x00022FB0 File Offset: 0x00021FB0
			internal Control GetControl()
			{
				return this.control;
			}

			// Token: 0x060016EE RID: 5870 RVA: 0x00022FB8 File Offset: 0x00021FB8
			protected override void OnHandleChange()
			{
				this.target.OnHandleChange(base.Handle);
			}

			// Token: 0x060016EF RID: 5871 RVA: 0x00022FCB File Offset: 0x00021FCB
			public void OnHandleChange(IntPtr newHandle)
			{
				this.control.SetHandle(newHandle);
			}

			// Token: 0x060016F0 RID: 5872 RVA: 0x00022FD9 File Offset: 0x00021FD9
			internal void LockReference(bool locked)
			{
				if (locked)
				{
					if (!this.rootRef.IsAllocated)
					{
						this.rootRef = GCHandle.Alloc(this.GetControl(), GCHandleType.Normal);
						return;
					}
				}
				else if (this.rootRef.IsAllocated)
				{
					this.rootRef.Free();
				}
			}

			// Token: 0x060016F1 RID: 5873 RVA: 0x00023016 File Offset: 0x00022016
			protected override void OnThreadException(Exception e)
			{
				this.control.WndProcException(e);
			}

			// Token: 0x060016F2 RID: 5874 RVA: 0x00023024 File Offset: 0x00022024
			public void OnMessage(ref Message m)
			{
				this.control.WndProc(ref m);
			}

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x060016F3 RID: 5875 RVA: 0x00023032 File Offset: 0x00022032
			// (set) Token: 0x060016F4 RID: 5876 RVA: 0x0002303A File Offset: 0x0002203A
			internal IWindowTarget WindowTarget
			{
				get
				{
					return this.target;
				}
				set
				{
					this.target = value;
				}
			}

			// Token: 0x060016F5 RID: 5877 RVA: 0x00023044 File Offset: 0x00022044
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 512)
				{
					if (msg != 522)
					{
						if (msg == 675)
						{
							this.control.UnhookMouseEvent();
						}
					}
					else
					{
						this.control.ResetMouseEventArgs();
					}
				}
				else if (!this.control.GetState(16384))
				{
					this.control.HookMouseEvent();
					if (!this.control.GetState(8192))
					{
						this.control.SendMessage(NativeMethods.WM_MOUSEENTER, 0, 0);
					}
					else
					{
						this.control.SetState(8192, false);
					}
				}
				this.target.OnMessage(ref m);
			}

			// Token: 0x0400116C RID: 4460
			private Control control;

			// Token: 0x0400116D RID: 4461
			private GCHandle rootRef;

			// Token: 0x0400116E RID: 4462
			internal IWindowTarget target;
		}

		// Token: 0x020001F3 RID: 499
		[ComVisible(false)]
		[ListBindable(false)]
		public class ControlCollection : ArrangedElementCollection, IList, ICollection, IEnumerable, ICloneable
		{
			// Token: 0x06001710 RID: 5904 RVA: 0x00023367 File Offset: 0x00022367
			public ControlCollection(Control owner)
			{
				this.owner = owner;
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x0002337D File Offset: 0x0002237D
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x06001712 RID: 5906 RVA: 0x0002338C File Offset: 0x0002238C
			public virtual void Add(Control value)
			{
				if (value == null)
				{
					return;
				}
				if (value.GetTopLevel())
				{
					throw new ArgumentException(SR.GetString("TopLevelControlAdd"));
				}
				if (this.owner.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"));
				}
				Control.CheckParentingCycle(this.owner, value);
				if (value.parent == this.owner)
				{
					value.SendToBack();
					return;
				}
				if (value.parent != null)
				{
					value.parent.Controls.Remove(value);
				}
				base.InnerList.Add(value);
				if (value.tabIndex == -1)
				{
					int num = 0;
					for (int i = 0; i < this.Count - 1; i++)
					{
						int tabIndex = this[i].TabIndex;
						if (num <= tabIndex)
						{
							num = tabIndex + 1;
						}
					}
					value.tabIndex = num;
				}
				this.owner.SuspendLayout();
				try
				{
					Control parent = value.parent;
					try
					{
						value.AssignParent(this.owner);
					}
					finally
					{
						if (parent != value.parent && (this.owner.state & 1) != 0)
						{
							value.SetParentHandle(this.owner.InternalHandle);
							if (value.Visible)
							{
								value.CreateControl();
							}
						}
					}
					value.InitLayout();
				}
				finally
				{
					this.owner.ResumeLayout(false);
				}
				LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
				this.owner.OnControlAdded(new ControlEventArgs(value));
			}

			// Token: 0x06001713 RID: 5907 RVA: 0x00023504 File Offset: 0x00022504
			int IList.Add(object control)
			{
				if (control is Control)
				{
					this.Add((Control)control);
					return this.IndexOf((Control)control);
				}
				throw new ArgumentException(SR.GetString("ControlBadControl"), "control");
			}

			// Token: 0x06001714 RID: 5908 RVA: 0x0002353C File Offset: 0x0002253C
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public virtual void AddRange(Control[] controls)
			{
				if (controls == null)
				{
					throw new ArgumentNullException("controls");
				}
				if (controls.Length > 0)
				{
					this.owner.SuspendLayout();
					try
					{
						for (int i = 0; i < controls.Length; i++)
						{
							this.Add(controls[i]);
						}
					}
					finally
					{
						this.owner.ResumeLayout(true);
					}
				}
			}

			// Token: 0x06001715 RID: 5909 RVA: 0x000235A0 File Offset: 0x000225A0
			object ICloneable.Clone()
			{
				Control.ControlCollection controlCollection = this.owner.CreateControlsInstance();
				controlCollection.InnerList.AddRange(this);
				return controlCollection;
			}

			// Token: 0x06001716 RID: 5910 RVA: 0x000235C6 File Offset: 0x000225C6
			public bool Contains(Control control)
			{
				return base.InnerList.Contains(control);
			}

			// Token: 0x06001717 RID: 5911 RVA: 0x000235D4 File Offset: 0x000225D4
			public Control[] Find(string key, bool searchAllChildren)
			{
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
				}
				ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
				Control[] array = new Control[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x00023624 File Offset: 0x00022624
			private ArrayList FindInternal(string key, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
			{
				if (controlsToLookIn == null || foundControls == null)
				{
					return null;
				}
				try
				{
					for (int i = 0; i < controlsToLookIn.Count; i++)
					{
						if (controlsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(controlsToLookIn[i].Name, key, true))
						{
							foundControls.Add(controlsToLookIn[i]);
						}
					}
					if (searchAllChildren)
					{
						for (int j = 0; j < controlsToLookIn.Count; j++)
						{
							if (controlsToLookIn[j] != null && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
							{
								foundControls = this.FindInternal(key, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return foundControls;
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x000236F0 File Offset: 0x000226F0
			public override IEnumerator GetEnumerator()
			{
				return new Control.ControlCollection.ControlCollectionEnumerator(this);
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x000236F8 File Offset: 0x000226F8
			public int IndexOf(Control control)
			{
				return base.InnerList.IndexOf(control);
			}

			// Token: 0x0600171B RID: 5915 RVA: 0x00023708 File Offset: 0x00022708
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x00023785 File Offset: 0x00022785
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x0600171D RID: 5917 RVA: 0x00023796 File Offset: 0x00022796
			public Control Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x0600171E RID: 5918 RVA: 0x000237A0 File Offset: 0x000227A0
			public virtual void Remove(Control value)
			{
				if (value == null)
				{
					return;
				}
				if (value.ParentInternal == this.owner)
				{
					value.SetParentHandle(IntPtr.Zero);
					base.InnerList.Remove(value);
					value.AssignParent(null);
					LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
					this.owner.OnControlRemoved(new ControlEventArgs(value));
					ContainerControl containerControl = this.owner.GetContainerControlInternal() as ContainerControl;
					if (containerControl != null)
					{
						containerControl.AfterControlRemoved(value, this.owner);
					}
				}
			}

			// Token: 0x0600171F RID: 5919 RVA: 0x00023820 File Offset: 0x00022820
			void IList.Remove(object control)
			{
				if (control is Control)
				{
					this.Remove((Control)control);
				}
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x00023836 File Offset: 0x00022836
			public void RemoveAt(int index)
			{
				this.Remove(this[index]);
			}

			// Token: 0x06001721 RID: 5921 RVA: 0x00023848 File Offset: 0x00022848
			public virtual void RemoveByKey(string key)
			{
				int num = this.IndexOfKey(key);
				if (this.IsValidIndex(num))
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x170002A6 RID: 678
			public virtual Control this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
					}
					return (Control)base.InnerList[index];
				}
			}

			// Token: 0x170002A7 RID: 679
			public virtual Control this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x06001724 RID: 5924 RVA: 0x00023900 File Offset: 0x00022900
			public virtual void Clear()
			{
				this.owner.SuspendLayout();
				CommonProperties.xClearAllPreferredSizeCaches(this.owner);
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					this.owner.ResumeLayout();
				}
			}

			// Token: 0x06001725 RID: 5925 RVA: 0x0002395C File Offset: 0x0002295C
			public int GetChildIndex(Control child)
			{
				return this.GetChildIndex(child, true);
			}

			// Token: 0x06001726 RID: 5926 RVA: 0x00023968 File Offset: 0x00022968
			public virtual int GetChildIndex(Control child, bool throwException)
			{
				int num = this.IndexOf(child);
				if (num == -1 && throwException)
				{
					throw new ArgumentException(SR.GetString("ControlNotChild"));
				}
				return num;
			}

			// Token: 0x06001727 RID: 5927 RVA: 0x00023998 File Offset: 0x00022998
			internal virtual void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child == null)
				{
					throw new ArgumentNullException("child");
				}
				int childIndex = this.GetChildIndex(child);
				if (childIndex == newIndex)
				{
					return;
				}
				if (newIndex >= this.Count || newIndex == -1)
				{
					newIndex = this.Count - 1;
				}
				base.MoveElement(child, childIndex, newIndex);
				child.UpdateZOrder();
				LayoutTransaction.DoLayout(this.owner, child, PropertyNames.ChildIndex);
			}

			// Token: 0x06001728 RID: 5928 RVA: 0x000239F7 File Offset: 0x000229F7
			public virtual void SetChildIndex(Control child, int newIndex)
			{
				this.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x04001171 RID: 4465
			private Control owner;

			// Token: 0x04001172 RID: 4466
			private int lastAccessedIndex = -1;

			// Token: 0x020001F4 RID: 500
			private class ControlCollectionEnumerator : IEnumerator
			{
				// Token: 0x06001729 RID: 5929 RVA: 0x00023A01 File Offset: 0x00022A01
				public ControlCollectionEnumerator(Control.ControlCollection controls)
				{
					this.controls = controls;
					this.originalCount = controls.Count;
					this.current = -1;
				}

				// Token: 0x0600172A RID: 5930 RVA: 0x00023A23 File Offset: 0x00022A23
				public bool MoveNext()
				{
					if (this.current < this.controls.Count - 1 && this.current < this.originalCount - 1)
					{
						this.current++;
						return true;
					}
					return false;
				}

				// Token: 0x0600172B RID: 5931 RVA: 0x00023A5B File Offset: 0x00022A5B
				public void Reset()
				{
					this.current = -1;
				}

				// Token: 0x170002A8 RID: 680
				// (get) Token: 0x0600172C RID: 5932 RVA: 0x00023A64 File Offset: 0x00022A64
				public object Current
				{
					get
					{
						if (this.current == -1)
						{
							return null;
						}
						return this.controls[this.current];
					}
				}

				// Token: 0x04001173 RID: 4467
				private Control.ControlCollection controls;

				// Token: 0x04001174 RID: 4468
				private int current;

				// Token: 0x04001175 RID: 4469
				private int originalCount;
			}
		}

		// Token: 0x020001F5 RID: 501
		private class ActiveXImpl : MarshalByRefObject, IWindowTarget
		{
			// Token: 0x0600172D RID: 5933 RVA: 0x00023A84 File Offset: 0x00022A84
			internal ActiveXImpl(Control control)
			{
				this.control = control;
				this.controlWindowTarget = control.WindowTarget;
				control.WindowTarget = this;
				this.adviseList = new ArrayList();
				this.activeXState = default(BitVector32);
				this.ambientProperties = new Control.AmbientProperty[]
				{
					new Control.AmbientProperty("Font", -703),
					new Control.AmbientProperty("BackColor", -701),
					new Control.AmbientProperty("ForeColor", -704)
				};
			}

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x0600172E RID: 5934 RVA: 0x00023B14 File Offset: 0x00022B14
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			internal Color AmbientBackColor
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-701);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-701, ref obj) && obj != null)
						{
							try
							{
								ambientProperty.Value = ColorTranslator.FromOle(Convert.ToInt32(obj, CultureInfo.InvariantCulture));
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
							}
						}
					}
					if (ambientProperty.Value == null)
					{
						return Color.Empty;
					}
					return (Color)ambientProperty.Value;
				}
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x0600172F RID: 5935 RVA: 0x00023B9C File Offset: 0x00022B9C
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[Browsable(false)]
			internal Font AmbientFont
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-703);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-703, ref obj))
						{
							try
							{
								IntPtr intPtr = IntPtr.Zero;
								UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)obj;
								IntSecurity.ObjectFromWin32Handle.Assert();
								Font font2 = null;
								try
								{
									intPtr = font.GetHFont();
									font2 = Font.FromHfont(intPtr);
								}
								finally
								{
									CodeAccessPermission.RevertAssert();
								}
								ambientProperty.Value = font2;
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
								ambientProperty.Value = null;
							}
						}
					}
					return (Font)ambientProperty.Value;
				}
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06001730 RID: 5936 RVA: 0x00023C48 File Offset: 0x00022C48
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			internal Color AmbientForeColor
			{
				get
				{
					Control.AmbientProperty ambientProperty = this.LookupAmbient(-704);
					if (ambientProperty.Empty)
					{
						object obj = null;
						if (this.GetAmbientProperty(-704, ref obj) && obj != null)
						{
							try
							{
								ambientProperty.Value = ColorTranslator.FromOle(Convert.ToInt32(obj, CultureInfo.InvariantCulture));
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsSecurityOrCriticalException(ex))
								{
									throw;
								}
							}
						}
					}
					if (ambientProperty.Value == null)
					{
						return Color.Empty;
					}
					return (Color)ambientProperty.Value;
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06001731 RID: 5937 RVA: 0x00023CD0 File Offset: 0x00022CD0
			// (set) Token: 0x06001732 RID: 5938 RVA: 0x00023CE2 File Offset: 0x00022CE2
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			internal bool EventsFrozen
			{
				get
				{
					return this.activeXState[Control.ActiveXImpl.eventsFrozen];
				}
				set
				{
					this.activeXState[Control.ActiveXImpl.eventsFrozen] = value;
				}
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06001733 RID: 5939 RVA: 0x00023CF5 File Offset: 0x00022CF5
			internal IntPtr HWNDParent
			{
				get
				{
					return this.hwndParent;
				}
			}

			// Token: 0x170002AE RID: 686
			// (get) Token: 0x06001734 RID: 5940 RVA: 0x00023D00 File Offset: 0x00022D00
			internal bool IsIE
			{
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (!Control.ActiveXImpl.checkedIE)
					{
						if (this.clientSite == null)
						{
							return false;
						}
						if (Assembly.GetEntryAssembly() == null)
						{
							UnsafeNativeMethods.IOleContainer oleContainer;
							if (NativeMethods.Succeeded(this.clientSite.GetContainer(out oleContainer)) && oleContainer is NativeMethods.IHTMLDocument)
							{
								Control.ActiveXImpl.isIE = true;
							}
							if (oleContainer != null && UnsafeNativeMethods.IsComObject(oleContainer))
							{
								UnsafeNativeMethods.ReleaseComObject(oleContainer);
							}
						}
						Control.ActiveXImpl.checkedIE = true;
					}
					return Control.ActiveXImpl.isIE;
				}
			}

			// Token: 0x170002AF RID: 687
			// (get) Token: 0x06001735 RID: 5941 RVA: 0x00023D64 File Offset: 0x00022D64
			private Point LogPixels
			{
				get
				{
					if (Control.ActiveXImpl.logPixels.IsEmpty)
					{
						Control.ActiveXImpl.logPixels = default(Point);
						IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
						Control.ActiveXImpl.logPixels.X = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						Control.ActiveXImpl.logPixels.Y = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
					return Control.ActiveXImpl.logPixels;
				}
			}

			// Token: 0x06001736 RID: 5942 RVA: 0x00023DDA File Offset: 0x00022DDA
			internal int Advise(IAdviseSink pAdvSink)
			{
				this.adviseList.Add(pAdvSink);
				return this.adviseList.Count;
			}

			// Token: 0x06001737 RID: 5943 RVA: 0x00023DF4 File Offset: 0x00022DF4
			internal void Close(int dwSaveOption)
			{
				if (this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					this.InPlaceDeactivate();
				}
				if ((dwSaveOption == 0 || dwSaveOption == 2) && this.activeXState[Control.ActiveXImpl.isDirty])
				{
					if (this.clientSite != null)
					{
						this.clientSite.SaveObject();
					}
					this.SendOnSave();
				}
			}

			// Token: 0x06001738 RID: 5944 RVA: 0x00023E4C File Offset: 0x00022E4C
			internal void DoVerb(int iVerb, IntPtr lpmsg, UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, NativeMethods.COMRECT lprcPosRect)
			{
				switch (iVerb)
				{
				case -5:
				case -4:
				case -1:
				case 0:
				{
					this.InPlaceActivate(iVerb);
					if (!(lpmsg != IntPtr.Zero))
					{
						return;
					}
					NativeMethods.MSG msg = (NativeMethods.MSG)UnsafeNativeMethods.PtrToStructure(lpmsg, typeof(NativeMethods.MSG));
					Control control = this.control;
					if (msg.hwnd != this.control.Handle && msg.message >= 512 && msg.message <= 522)
					{
						IntPtr intPtr = ((msg.hwnd == IntPtr.Zero) ? hwndParent : msg.hwnd);
						NativeMethods.POINT point = new NativeMethods.POINT();
						point.x = NativeMethods.Util.LOWORD(msg.lParam);
						point.y = NativeMethods.Util.HIWORD(msg.lParam);
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(null, intPtr), new HandleRef(this.control, this.control.Handle), point, 1);
						Control childAtPoint = control.GetChildAtPoint(new Point(point.x, point.y));
						if (childAtPoint != null && childAtPoint != control)
						{
							UnsafeNativeMethods.MapWindowPoints(new HandleRef(control, control.Handle), new HandleRef(childAtPoint, childAtPoint.Handle), point, 1);
							control = childAtPoint;
						}
						msg.lParam = NativeMethods.Util.MAKELPARAM(point.x, point.y);
					}
					if (msg.message == 256 && msg.wParam == (IntPtr)9)
					{
						control.SelectNextControl(null, Control.ModifierKeys != Keys.Shift, true, true, true);
						return;
					}
					control.SendMessage(msg.message, msg.wParam, msg.lParam);
					return;
				}
				case -3:
					this.UIDeactivate();
					this.InPlaceDeactivate();
					if (this.activeXState[Control.ActiveXImpl.inPlaceVisible])
					{
						this.SetInPlaceVisible(false);
						return;
					}
					return;
				}
				Control.ActiveXImpl.ThrowHr(-2147467263);
			}

			// Token: 0x06001739 RID: 5945 RVA: 0x00024050 File Offset: 0x00023050
			internal void Draw(int dwDrawAspect, int lindex, IntPtr pvAspect, NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, NativeMethods.COMRECT prcBounds, NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, int dwContinue)
			{
				if (dwDrawAspect != 1 && dwDrawAspect != 16 && dwDrawAspect != 32)
				{
					Control.ActiveXImpl.ThrowHr(-2147221397);
				}
				int objectType = UnsafeNativeMethods.GetObjectType(new HandleRef(null, hdcDraw));
				if (objectType == 4)
				{
					Control.ActiveXImpl.ThrowHr(-2147221184);
				}
				NativeMethods.POINT point = new NativeMethods.POINT();
				NativeMethods.POINT point2 = new NativeMethods.POINT();
				NativeMethods.SIZE size = new NativeMethods.SIZE();
				NativeMethods.SIZE size2 = new NativeMethods.SIZE();
				int num = 1;
				if (!this.control.IsHandleCreated)
				{
					this.control.CreateHandle();
				}
				if (prcBounds != null)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(prcBounds.left, prcBounds.top, prcBounds.right, prcBounds.bottom);
					SafeNativeMethods.LPtoDP(new HandleRef(null, hdcDraw), ref rect, 2);
					num = SafeNativeMethods.SetMapMode(new HandleRef(null, hdcDraw), 8);
					SafeNativeMethods.SetWindowOrgEx(new HandleRef(null, hdcDraw), 0, 0, point2);
					SafeNativeMethods.SetWindowExtEx(new HandleRef(null, hdcDraw), this.control.Width, this.control.Height, size);
					SafeNativeMethods.SetViewportOrgEx(new HandleRef(null, hdcDraw), rect.left, rect.top, point);
					SafeNativeMethods.SetViewportExtEx(new HandleRef(null, hdcDraw), rect.right - rect.left, rect.bottom - rect.top, size2);
				}
				try
				{
					IntPtr intPtr = (IntPtr)30;
					if (objectType != 12)
					{
						this.control.SendMessage(791, hdcDraw, intPtr);
					}
					else
					{
						this.control.PrintToMetaFile(new HandleRef(null, hdcDraw), intPtr);
					}
				}
				finally
				{
					if (prcBounds != null)
					{
						SafeNativeMethods.SetWindowOrgEx(new HandleRef(null, hdcDraw), point2.x, point2.y, null);
						SafeNativeMethods.SetWindowExtEx(new HandleRef(null, hdcDraw), size.cx, size.cy, null);
						SafeNativeMethods.SetViewportOrgEx(new HandleRef(null, hdcDraw), point.x, point.y, null);
						SafeNativeMethods.SetViewportExtEx(new HandleRef(null, hdcDraw), size2.cx, size2.cy, null);
						SafeNativeMethods.SetMapMode(new HandleRef(null, hdcDraw), num);
					}
				}
			}

			// Token: 0x0600173A RID: 5946 RVA: 0x00024278 File Offset: 0x00023278
			internal static int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e)
			{
				if (Control.ActiveXImpl.axVerbs == null)
				{
					NativeMethods.tagOLEVERB tagOLEVERB = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB2 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB3 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB4 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB5 = new NativeMethods.tagOLEVERB();
					NativeMethods.tagOLEVERB tagOLEVERB6 = new NativeMethods.tagOLEVERB();
					tagOLEVERB.lVerb = -1;
					tagOLEVERB2.lVerb = -5;
					tagOLEVERB3.lVerb = -4;
					tagOLEVERB4.lVerb = -3;
					tagOLEVERB5.lVerb = 0;
					tagOLEVERB6.lVerb = -7;
					tagOLEVERB6.lpszVerbName = SR.GetString("AXProperties");
					tagOLEVERB6.grfAttribs = 2;
					Control.ActiveXImpl.axVerbs = new NativeMethods.tagOLEVERB[] { tagOLEVERB, tagOLEVERB2, tagOLEVERB3, tagOLEVERB4, tagOLEVERB5 };
				}
				e = new Control.ActiveXVerbEnum(Control.ActiveXImpl.axVerbs);
				return 0;
			}

			// Token: 0x0600173B RID: 5947 RVA: 0x00024340 File Offset: 0x00023340
			private static byte[] FromBase64WrappedString(string text)
			{
				if (text.IndexOfAny(new char[] { ' ', '\r', '\n' }) != -1)
				{
					StringBuilder stringBuilder = new StringBuilder(text.Length);
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c != '\n' && c != '\r' && c != ' ')
						{
							stringBuilder.Append(text[i]);
						}
					}
					return Convert.FromBase64String(stringBuilder.ToString());
				}
				return Convert.FromBase64String(text);
			}

			// Token: 0x0600173C RID: 5948 RVA: 0x000243BC File Offset: 0x000233BC
			internal void GetAdvise(int[] paspects, int[] padvf, IAdviseSink[] pAdvSink)
			{
				if (paspects != null)
				{
					paspects[0] = 1;
				}
				if (padvf != null)
				{
					padvf[0] = 0;
					if (this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce])
					{
						padvf[0] |= 4;
					}
					if (this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst])
					{
						padvf[0] |= 2;
					}
				}
				if (pAdvSink != null)
				{
					pAdvSink[0] = this.viewAdviseSink;
				}
			}

			// Token: 0x0600173D RID: 5949 RVA: 0x00024430 File Offset: 0x00023430
			private bool GetAmbientProperty(int dispid, ref object obj)
			{
				if (this.clientSite is UnsafeNativeMethods.IDispatch)
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)this.clientSite;
					object[] array = new object[1];
					Guid empty = Guid.Empty;
					int num = -2147467259;
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						num = dispatch.Invoke(dispid, ref empty, NativeMethods.LOCALE_USER_DEFAULT, 2, new NativeMethods.tagDISPPARAMS(), array, null, null);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Succeeded(num))
					{
						obj = array[0];
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600173E RID: 5950 RVA: 0x000244B4 File Offset: 0x000234B4
			internal UnsafeNativeMethods.IOleClientSite GetClientSite()
			{
				return this.clientSite;
			}

			// Token: 0x0600173F RID: 5951 RVA: 0x000244BC File Offset: 0x000234BC
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal int GetControlInfo(NativeMethods.tagCONTROLINFO pCI)
			{
				if (this.accelCount == -1)
				{
					ArrayList arrayList = new ArrayList();
					this.GetMnemonicList(this.control, arrayList);
					this.accelCount = (short)arrayList.Count;
					if (this.accelCount > 0)
					{
						int num = UnsafeNativeMethods.SizeOf(typeof(NativeMethods.ACCEL));
						IntPtr intPtr = Marshal.AllocHGlobal(num * (int)this.accelCount * 2);
						try
						{
							NativeMethods.ACCEL accel = new NativeMethods.ACCEL();
							accel.cmd = 0;
							this.accelCount = 0;
							foreach (object obj in arrayList)
							{
								char c = (char)obj;
								IntPtr intPtr2 = (IntPtr)((long)intPtr + (long)((int)this.accelCount * num));
								if (c >= 'A' && c <= 'Z')
								{
									accel.fVirt = 17;
									accel.key = UnsafeNativeMethods.VkKeyScan(c) & 255;
									Marshal.StructureToPtr(accel, intPtr2, false);
									this.accelCount += 1;
									intPtr2 = (IntPtr)((long)intPtr2 + (long)num);
									accel.fVirt = 21;
									Marshal.StructureToPtr(accel, intPtr2, false);
								}
								else
								{
									accel.fVirt = 17;
									short num2 = UnsafeNativeMethods.VkKeyScan(c);
									if ((num2 & 256) != 0)
									{
										NativeMethods.ACCEL accel2 = accel;
										accel2.fVirt |= 4;
									}
									accel.key = num2 & 255;
									Marshal.StructureToPtr(accel, intPtr2, false);
								}
								NativeMethods.ACCEL accel3 = accel;
								accel3.cmd += 1;
								this.accelCount += 1;
							}
							if (this.accelTable != IntPtr.Zero)
							{
								UnsafeNativeMethods.DestroyAcceleratorTable(new HandleRef(this, this.accelTable));
								this.accelTable = IntPtr.Zero;
							}
							this.accelTable = UnsafeNativeMethods.CreateAcceleratorTable(new HandleRef(null, intPtr), (int)this.accelCount);
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								Marshal.FreeHGlobal(intPtr);
							}
						}
					}
				}
				pCI.cAccel = this.accelCount;
				pCI.hAccel = this.accelTable;
				return 0;
			}

			// Token: 0x06001740 RID: 5952 RVA: 0x000246F8 File Offset: 0x000236F8
			internal void GetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
			{
				if ((dwDrawAspect & 1) != 0)
				{
					Size size = this.control.Size;
					Point point = this.PixelToHiMetric(size.Width, size.Height);
					pSizel.cx = point.X;
					pSizel.cy = point.Y;
					return;
				}
				Control.ActiveXImpl.ThrowHr(-2147221397);
			}

			// Token: 0x06001741 RID: 5953 RVA: 0x00024750 File Offset: 0x00023750
			private void GetMnemonicList(Control control, ArrayList mnemonicList)
			{
				char mnemonic = WindowsFormsUtils.GetMnemonic(control.Text, true);
				if (mnemonic != '\0')
				{
					mnemonicList.Add(mnemonic);
				}
				foreach (object obj in control.Controls)
				{
					Control control2 = (Control)obj;
					if (control2 != null)
					{
						this.GetMnemonicList(control2, mnemonicList);
					}
				}
			}

			// Token: 0x06001742 RID: 5954 RVA: 0x000247CC File Offset: 0x000237CC
			private string GetStreamName()
			{
				string text = this.control.GetType().FullName;
				int length = text.Length;
				if (length > 31)
				{
					text = text.Substring(length - 31);
				}
				return text;
			}

			// Token: 0x06001743 RID: 5955 RVA: 0x00024802 File Offset: 0x00023802
			internal int GetWindow(out IntPtr hwnd)
			{
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					hwnd = IntPtr.Zero;
					return -2147467259;
				}
				hwnd = this.control.Handle;
				return 0;
			}

			// Token: 0x06001744 RID: 5956 RVA: 0x0002483C File Offset: 0x0002383C
			private Point HiMetricToPixel(int x, int y)
			{
				return new Point
				{
					X = (this.LogPixels.X * x + Control.ActiveXImpl.hiMetricPerInch / 2) / Control.ActiveXImpl.hiMetricPerInch,
					Y = (this.LogPixels.Y * y + Control.ActiveXImpl.hiMetricPerInch / 2) / Control.ActiveXImpl.hiMetricPerInch
				};
			}

			// Token: 0x06001745 RID: 5957 RVA: 0x0002489C File Offset: 0x0002389C
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void InPlaceActivate(int verb)
			{
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite == null)
				{
					return;
				}
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					int num = oleInPlaceSite.CanInPlaceActivate();
					if (num != 0)
					{
						if (NativeMethods.Succeeded(num))
						{
							num = -2147467259;
						}
						Control.ActiveXImpl.ThrowHr(num);
					}
					oleInPlaceSite.OnInPlaceActivate();
					this.activeXState[Control.ActiveXImpl.inPlaceActive] = true;
				}
				if (!this.activeXState[Control.ActiveXImpl.inPlaceVisible])
				{
					NativeMethods.tagOIFI tagOIFI = new NativeMethods.tagOIFI();
					tagOIFI.cb = UnsafeNativeMethods.SizeOf(typeof(NativeMethods.tagOIFI));
					IntPtr intPtr = IntPtr.Zero;
					intPtr = oleInPlaceSite.GetWindow();
					NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
					NativeMethods.COMRECT comrect2 = new NativeMethods.COMRECT();
					if (this.inPlaceUiWindow != null && UnsafeNativeMethods.IsComObject(this.inPlaceUiWindow))
					{
						UnsafeNativeMethods.ReleaseComObject(this.inPlaceUiWindow);
						this.inPlaceUiWindow = null;
					}
					if (this.inPlaceFrame != null && UnsafeNativeMethods.IsComObject(this.inPlaceFrame))
					{
						UnsafeNativeMethods.ReleaseComObject(this.inPlaceFrame);
						this.inPlaceFrame = null;
					}
					UnsafeNativeMethods.IOleInPlaceFrame oleInPlaceFrame;
					UnsafeNativeMethods.IOleInPlaceUIWindow oleInPlaceUIWindow;
					oleInPlaceSite.GetWindowContext(out oleInPlaceFrame, out oleInPlaceUIWindow, comrect, comrect2, tagOIFI);
					this.SetObjectRects(comrect, comrect2);
					this.inPlaceFrame = oleInPlaceFrame;
					this.inPlaceUiWindow = oleInPlaceUIWindow;
					this.hwndParent = intPtr;
					UnsafeNativeMethods.SetParent(new HandleRef(this.control, this.control.Handle), new HandleRef(null, intPtr));
					this.control.CreateControl();
					this.clientSite.ShowObject();
					this.SetInPlaceVisible(true);
				}
				if (verb != 0 && verb != -4)
				{
					return;
				}
				if (!this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.activeXState[Control.ActiveXImpl.uiActive] = true;
					oleInPlaceSite.OnUIActivate();
					if (!this.control.ContainsFocus)
					{
						this.control.FocusInternal();
					}
					this.inPlaceFrame.SetActiveObject(this.control, null);
					if (this.inPlaceUiWindow != null)
					{
						this.inPlaceUiWindow.SetActiveObject(this.control, null);
					}
					int num2 = this.inPlaceFrame.SetBorderSpace(null);
					if (NativeMethods.Failed(num2) && num2 != -2147221491 && num2 != -2147221087 && num2 != -2147467263)
					{
						UnsafeNativeMethods.ThrowExceptionForHR(num2);
					}
					if (this.inPlaceUiWindow != null)
					{
						num2 = this.inPlaceFrame.SetBorderSpace(null);
						if (NativeMethods.Failed(num2) && num2 != -2147221491 && num2 != -2147221087 && num2 != -2147467263)
						{
							UnsafeNativeMethods.ThrowExceptionForHR(num2);
						}
					}
				}
			}

			// Token: 0x06001746 RID: 5958 RVA: 0x00024B04 File Offset: 0x00023B04
			internal void InPlaceDeactivate()
			{
				if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
				{
					return;
				}
				if (this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.UIDeactivate();
				}
				this.activeXState[Control.ActiveXImpl.inPlaceActive] = false;
				this.activeXState[Control.ActiveXImpl.inPlaceVisible] = false;
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						oleInPlaceSite.OnInPlaceDeactivate();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				this.control.Visible = false;
				this.hwndParent = IntPtr.Zero;
				if (this.inPlaceUiWindow != null && UnsafeNativeMethods.IsComObject(this.inPlaceUiWindow))
				{
					UnsafeNativeMethods.ReleaseComObject(this.inPlaceUiWindow);
					this.inPlaceUiWindow = null;
				}
				if (this.inPlaceFrame != null && UnsafeNativeMethods.IsComObject(this.inPlaceFrame))
				{
					UnsafeNativeMethods.ReleaseComObject(this.inPlaceFrame);
					this.inPlaceFrame = null;
				}
			}

			// Token: 0x06001747 RID: 5959 RVA: 0x00024C00 File Offset: 0x00023C00
			internal int IsDirty()
			{
				if (this.activeXState[Control.ActiveXImpl.isDirty])
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06001748 RID: 5960 RVA: 0x00024C18 File Offset: 0x00023C18
			private bool IsResourceProp(PropertyDescriptor prop)
			{
				TypeConverter converter = prop.Converter;
				Type[] array = new Type[]
				{
					typeof(string),
					typeof(byte[])
				};
				foreach (Type type in array)
				{
					if (converter.CanConvertTo(type) && converter.CanConvertFrom(type))
					{
						return false;
					}
				}
				return prop.GetValue(this.control) is ISerializable;
			}

			// Token: 0x06001749 RID: 5961 RVA: 0x00024C9C File Offset: 0x00023C9C
			internal void Load(UnsafeNativeMethods.IStorage stg)
			{
				UnsafeNativeMethods.IStream stream = null;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					stream = stg.OpenStream(this.GetStreamName(), IntPtr.Zero, 16, 0);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147287038)
					{
						throw;
					}
					stream = stg.OpenStream(base.GetType().FullName, IntPtr.Zero, 16, 0);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.Load(stream);
				stream = null;
				if (UnsafeNativeMethods.IsComObject(stg))
				{
					UnsafeNativeMethods.ReleaseComObject(stg);
				}
			}

			// Token: 0x0600174A RID: 5962 RVA: 0x00024D38 File Offset: 0x00023D38
			internal void Load(UnsafeNativeMethods.IStream stream)
			{
				Control.ActiveXImpl.PropertyBagStream propertyBagStream = new Control.ActiveXImpl.PropertyBagStream();
				propertyBagStream.Read(stream);
				this.Load(propertyBagStream, null);
				if (UnsafeNativeMethods.IsComObject(stream))
				{
					UnsafeNativeMethods.ReleaseComObject(stream);
				}
			}

			// Token: 0x0600174B RID: 5963 RVA: 0x00024D6C File Offset: 0x00023D6C
			internal void Load(UnsafeNativeMethods.IPropertyBag pPropBag, UnsafeNativeMethods.IErrorLog pErrorLog)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.control, new Attribute[] { DesignerSerializationVisibilityAttribute.Visible });
				for (int i = 0; i < properties.Count; i++)
				{
					try
					{
						object obj = null;
						int num = -2147467259;
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							num = pPropBag.Read(properties[i].Name, ref obj, pErrorLog);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (NativeMethods.Succeeded(num) && obj != null)
						{
							string text = null;
							int num2 = 0;
							try
							{
								if (obj.GetType() != typeof(string))
								{
									obj = Convert.ToString(obj, CultureInfo.InvariantCulture);
								}
								if (this.IsResourceProp(properties[i]))
								{
									byte[] array = Convert.FromBase64String(obj.ToString());
									MemoryStream memoryStream = new MemoryStream(array);
									BinaryFormatter binaryFormatter = new BinaryFormatter();
									properties[i].SetValue(this.control, binaryFormatter.Deserialize(memoryStream));
								}
								else
								{
									TypeConverter converter = properties[i].Converter;
									object obj2 = null;
									if (converter.CanConvertFrom(typeof(string)))
									{
										obj2 = converter.ConvertFromInvariantString(obj.ToString());
									}
									else if (converter.CanConvertFrom(typeof(byte[])))
									{
										string text2 = obj.ToString();
										obj2 = converter.ConvertFrom(null, CultureInfo.InvariantCulture, Control.ActiveXImpl.FromBase64WrappedString(text2));
									}
									properties[i].SetValue(this.control, obj2);
								}
							}
							catch (Exception ex)
							{
								text = ex.ToString();
								if (ex is ExternalException)
								{
									num2 = ((ExternalException)ex).ErrorCode;
								}
								else
								{
									num2 = -2147467259;
								}
							}
							if (text != null && pErrorLog != null)
							{
								NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
								tagEXCEPINFO.bstrSource = this.control.GetType().FullName;
								tagEXCEPINFO.bstrDescription = text;
								tagEXCEPINFO.scode = num2;
								pErrorLog.AddError(properties[i].Name, tagEXCEPINFO);
							}
						}
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex2))
						{
							throw;
						}
					}
				}
				if (UnsafeNativeMethods.IsComObject(pPropBag))
				{
					UnsafeNativeMethods.ReleaseComObject(pPropBag);
				}
			}

			// Token: 0x0600174C RID: 5964 RVA: 0x00024FC0 File Offset: 0x00023FC0
			private Control.AmbientProperty LookupAmbient(int dispid)
			{
				for (int i = 0; i < this.ambientProperties.Length; i++)
				{
					if (this.ambientProperties[i].DispID == dispid)
					{
						return this.ambientProperties[i];
					}
				}
				return this.ambientProperties[0];
			}

			// Token: 0x0600174D RID: 5965 RVA: 0x00025004 File Offset: 0x00024004
			internal IntPtr MergeRegion(IntPtr region)
			{
				if (this.clipRegion == IntPtr.Zero)
				{
					return region;
				}
				if (region == IntPtr.Zero)
				{
					return this.clipRegion;
				}
				IntPtr intPtr2;
				try
				{
					IntPtr intPtr = SafeNativeMethods.CreateRectRgn(0, 0, 0, 0);
					try
					{
						SafeNativeMethods.CombineRgn(new HandleRef(null, intPtr), new HandleRef(null, region), new HandleRef(this, this.clipRegion), 4);
						SafeNativeMethods.DeleteObject(new HandleRef(null, region));
					}
					catch
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						throw;
					}
					intPtr2 = intPtr;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					intPtr2 = region;
				}
				return intPtr2;
			}

			// Token: 0x0600174E RID: 5966 RVA: 0x000250B4 File Offset: 0x000240B4
			private void CallParentPropertyChanged(Control control, string propName)
			{
				switch (propName)
				{
				case "BackColor":
					control.OnParentBackColorChanged(EventArgs.Empty);
					return;
				case "BackgroundImage":
					control.OnParentBackgroundImageChanged(EventArgs.Empty);
					return;
				case "BindingContext":
					control.OnParentBindingContextChanged(EventArgs.Empty);
					return;
				case "Enabled":
					control.OnParentEnabledChanged(EventArgs.Empty);
					return;
				case "Font":
					control.OnParentFontChanged(EventArgs.Empty);
					return;
				case "ForeColor":
					control.OnParentForeColorChanged(EventArgs.Empty);
					return;
				case "RightToLeft":
					control.OnParentRightToLeftChanged(EventArgs.Empty);
					return;
				case "Visible":
					control.OnParentVisibleChanged(EventArgs.Empty);
					break;

					return;
				}
			}

			// Token: 0x0600174F RID: 5967 RVA: 0x000251DC File Offset: 0x000241DC
			internal void OnAmbientPropertyChange(int dispID)
			{
				if (dispID != -1)
				{
					for (int i = 0; i < this.ambientProperties.Length; i++)
					{
						if (this.ambientProperties[i].DispID == dispID)
						{
							this.ambientProperties[i].ResetValue();
							this.CallParentPropertyChanged(this.control, this.ambientProperties[i].Name);
							return;
						}
					}
					object obj = new object();
					if (dispID != -713)
					{
						if (dispID != -710)
						{
							return;
						}
						if (this.GetAmbientProperty(-710, ref obj))
						{
							this.activeXState[Control.ActiveXImpl.uiDead] = (bool)obj;
							return;
						}
					}
					else
					{
						IButtonControl buttonControl = this.control as IButtonControl;
						if (buttonControl != null && this.GetAmbientProperty(-713, ref obj))
						{
							buttonControl.NotifyDefault((bool)obj);
							return;
						}
					}
				}
				else
				{
					for (int j = 0; j < this.ambientProperties.Length; j++)
					{
						this.ambientProperties[j].ResetValue();
						this.CallParentPropertyChanged(this.control, this.ambientProperties[j].Name);
					}
				}
			}

			// Token: 0x06001750 RID: 5968 RVA: 0x000252E0 File Offset: 0x000242E0
			internal void OnDocWindowActivate(int fActivate)
			{
				if (this.activeXState[Control.ActiveXImpl.uiActive] && fActivate != 0 && this.inPlaceFrame != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					int num;
					try
					{
						num = this.inPlaceFrame.SetBorderSpace(null);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Failed(num) && num != -2147221087 && num != -2147467263)
					{
						UnsafeNativeMethods.ThrowExceptionForHR(num);
					}
				}
			}

			// Token: 0x06001751 RID: 5969 RVA: 0x00025358 File Offset: 0x00024358
			internal void OnFocus(bool focus)
			{
				if (this.activeXState[Control.ActiveXImpl.inPlaceActive] && this.clientSite is UnsafeNativeMethods.IOleControlSite)
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						((UnsafeNativeMethods.IOleControlSite)this.clientSite).OnFocus(focus ? 1 : 0);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				if (focus && this.activeXState[Control.ActiveXImpl.inPlaceActive] && !this.activeXState[Control.ActiveXImpl.uiActive])
				{
					this.InPlaceActivate(-4);
				}
			}

			// Token: 0x06001752 RID: 5970 RVA: 0x000253F0 File Offset: 0x000243F0
			private Point PixelToHiMetric(int x, int y)
			{
				return new Point
				{
					X = (Control.ActiveXImpl.hiMetricPerInch * x + (this.LogPixels.X >> 1)) / this.LogPixels.X,
					Y = (Control.ActiveXImpl.hiMetricPerInch * y + (this.LogPixels.Y >> 1)) / this.LogPixels.Y
				};
			}

			// Token: 0x06001753 RID: 5971 RVA: 0x00025464 File Offset: 0x00024464
			internal void QuickActivate(UnsafeNativeMethods.tagQACONTAINER pQaContainer, UnsafeNativeMethods.tagQACONTROL pQaControl)
			{
				Control.AmbientProperty ambientProperty = this.LookupAmbient(-701);
				ambientProperty.Value = ColorTranslator.FromOle((int)pQaContainer.colorBack);
				ambientProperty = this.LookupAmbient(-704);
				ambientProperty.Value = ColorTranslator.FromOle((int)pQaContainer.colorFore);
				if (pQaContainer.pFont != null)
				{
					ambientProperty = this.LookupAmbient(-703);
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						IntPtr intPtr = IntPtr.Zero;
						object pFont = pQaContainer.pFont;
						UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)pFont;
						intPtr = font.GetHFont();
						Font font2 = Font.FromHfont(intPtr);
						ambientProperty.Value = font2;
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
						ambientProperty.Value = null;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				pQaControl.cbSize = UnsafeNativeMethods.SizeOf(typeof(UnsafeNativeMethods.tagQACONTROL));
				this.SetClientSite(pQaContainer.pClientSite);
				if (pQaContainer.pAdviseSink != null)
				{
					this.SetAdvise(1, 0, (IAdviseSink)pQaContainer.pAdviseSink);
				}
				IntSecurity.UnmanagedCode.Assert();
				int num;
				try
				{
					((UnsafeNativeMethods.IOleObject)this.control).GetMiscStatus(1, out num);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				pQaControl.dwMiscStatus = num;
				if (pQaContainer.pUnkEventSink != null && this.control is UserControl)
				{
					Type defaultEventsInterface = Control.ActiveXImpl.GetDefaultEventsInterface(this.control.GetType());
					if (defaultEventsInterface != null)
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							Control.ActiveXImpl.AdviseHelper.AdviseConnectionPoint(this.control, pQaContainer.pUnkEventSink, defaultEventsInterface, out pQaControl.dwEventCookie);
						}
						catch (Exception ex2)
						{
							if (ClientUtils.IsSecurityOrCriticalException(ex2))
							{
								throw;
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				if (pQaContainer.pPropertyNotifySink != null && UnsafeNativeMethods.IsComObject(pQaContainer.pPropertyNotifySink))
				{
					UnsafeNativeMethods.ReleaseComObject(pQaContainer.pPropertyNotifySink);
				}
				if (pQaContainer.pUnkEventSink != null && UnsafeNativeMethods.IsComObject(pQaContainer.pUnkEventSink))
				{
					UnsafeNativeMethods.ReleaseComObject(pQaContainer.pUnkEventSink);
				}
			}

			// Token: 0x06001754 RID: 5972 RVA: 0x00025670 File Offset: 0x00024670
			private static Type GetDefaultEventsInterface(Type controlType)
			{
				Type type = null;
				object[] customAttributes = controlType.GetCustomAttributes(typeof(ComSourceInterfacesAttribute), false);
				if (customAttributes.Length > 0)
				{
					ComSourceInterfacesAttribute comSourceInterfacesAttribute = (ComSourceInterfacesAttribute)customAttributes[0];
					string value = comSourceInterfacesAttribute.Value;
					char[] array = new char[1];
					string text = value.Split(array)[0];
					type = controlType.Module.Assembly.GetType(text, false);
					if (type == null)
					{
						type = Type.GetType(text, false);
					}
				}
				return type;
			}

			// Token: 0x06001755 RID: 5973 RVA: 0x000256D8 File Offset: 0x000246D8
			internal void Save(UnsafeNativeMethods.IStorage stg, bool fSameAsLoad)
			{
				UnsafeNativeMethods.IStream stream = null;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					stream = stg.CreateStream(this.GetStreamName(), 4113, 0, 0);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.Save(stream, true);
				UnsafeNativeMethods.ReleaseComObject(stream);
			}

			// Token: 0x06001756 RID: 5974 RVA: 0x0002572C File Offset: 0x0002472C
			internal void Save(UnsafeNativeMethods.IStream stream, bool fClearDirty)
			{
				Control.ActiveXImpl.PropertyBagStream propertyBagStream = new Control.ActiveXImpl.PropertyBagStream();
				this.Save(propertyBagStream, fClearDirty, false);
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					propertyBagStream.Write(stream);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (UnsafeNativeMethods.IsComObject(stream))
				{
					UnsafeNativeMethods.ReleaseComObject(stream);
				}
			}

			// Token: 0x06001757 RID: 5975 RVA: 0x00025780 File Offset: 0x00024780
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void Save(UnsafeNativeMethods.IPropertyBag pPropBag, bool fClearDirty, bool fSaveAllProperties)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.control, new Attribute[] { DesignerSerializationVisibilityAttribute.Visible });
				for (int i = 0; i < properties.Count; i++)
				{
					if (fSaveAllProperties || properties[i].ShouldSerializeValue(this.control))
					{
						if (this.IsResourceProp(properties[i]))
						{
							MemoryStream memoryStream = new MemoryStream();
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							binaryFormatter.Serialize(memoryStream, properties[i].GetValue(this.control));
							byte[] array = new byte[(int)memoryStream.Length];
							memoryStream.Position = 0L;
							memoryStream.Read(array, 0, array.Length);
							object obj = Convert.ToBase64String(array);
							pPropBag.Write(properties[i].Name, ref obj);
						}
						else
						{
							TypeConverter converter = properties[i].Converter;
							if (converter.CanConvertFrom(typeof(string)))
							{
								object obj = converter.ConvertToInvariantString(properties[i].GetValue(this.control));
								pPropBag.Write(properties[i].Name, ref obj);
							}
							else if (converter.CanConvertFrom(typeof(byte[])))
							{
								byte[] array2 = (byte[])converter.ConvertTo(null, CultureInfo.InvariantCulture, properties[i].GetValue(this.control), typeof(byte[]));
								object obj = Convert.ToBase64String(array2);
								pPropBag.Write(properties[i].Name, ref obj);
							}
						}
					}
				}
				if (UnsafeNativeMethods.IsComObject(pPropBag))
				{
					UnsafeNativeMethods.ReleaseComObject(pPropBag);
				}
				if (fClearDirty)
				{
					this.activeXState[Control.ActiveXImpl.isDirty] = false;
				}
			}

			// Token: 0x06001758 RID: 5976 RVA: 0x00025930 File Offset: 0x00024930
			private void SendOnSave()
			{
				int count = this.adviseList.Count;
				IntSecurity.UnmanagedCode.Assert();
				for (int i = 0; i < count; i++)
				{
					IAdviseSink adviseSink = (IAdviseSink)this.adviseList[i];
					adviseSink.OnSave();
				}
			}

			// Token: 0x06001759 RID: 5977 RVA: 0x00025978 File Offset: 0x00024978
			internal void SetAdvise(int aspects, int advf, IAdviseSink pAdvSink)
			{
				if ((aspects & 1) == 0)
				{
					Control.ActiveXImpl.ThrowHr(-2147221397);
				}
				this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst] = (advf & 2) != 0;
				this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce] = (advf & 4) != 0;
				if (this.viewAdviseSink != null && UnsafeNativeMethods.IsComObject(this.viewAdviseSink))
				{
					UnsafeNativeMethods.ReleaseComObject(this.viewAdviseSink);
				}
				this.viewAdviseSink = pAdvSink;
				if (this.activeXState[Control.ActiveXImpl.viewAdvisePrimeFirst])
				{
					this.ViewChanged();
				}
			}

			// Token: 0x0600175A RID: 5978 RVA: 0x00025A08 File Offset: 0x00024A08
			internal void SetClientSite(UnsafeNativeMethods.IOleClientSite value)
			{
				if (this.clientSite != null)
				{
					if (value == null)
					{
						Control.ActiveXImpl.globalActiveXCount--;
						if (Control.ActiveXImpl.globalActiveXCount == 0 && this.IsIE)
						{
							new PermissionSet(PermissionState.Unrestricted).Assert();
							try
							{
								MethodInfo method = typeof(SystemEvents).GetMethod("Shutdown", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[0], new ParameterModifier[0]);
								if (method != null)
								{
									method.Invoke(null, null);
								}
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
					if (UnsafeNativeMethods.IsComObject(this.clientSite))
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							Marshal.FinalReleaseComObject(this.clientSite);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				this.clientSite = value;
				if (this.clientSite != null)
				{
					this.control.Site = new Control.AxSourcingSite(this.control, this.clientSite, "ControlAxSourcingSite");
				}
				else
				{
					this.control.Site = null;
				}
				object obj = new object();
				if (this.GetAmbientProperty(-710, ref obj))
				{
					this.activeXState[Control.ActiveXImpl.uiDead] = (bool)obj;
				}
				if (this.control is IButtonControl && this.GetAmbientProperty(-710, ref obj))
				{
					((IButtonControl)this.control).NotifyDefault((bool)obj);
				}
				if (this.clientSite == null)
				{
					if (this.accelTable != IntPtr.Zero)
					{
						UnsafeNativeMethods.DestroyAcceleratorTable(new HandleRef(this, this.accelTable));
						this.accelTable = IntPtr.Zero;
						this.accelCount = -1;
					}
					if (this.IsIE)
					{
						this.control.Dispose();
					}
				}
				else
				{
					Control.ActiveXImpl.globalActiveXCount++;
					if (Control.ActiveXImpl.globalActiveXCount == 1 && this.IsIE)
					{
						new PermissionSet(PermissionState.Unrestricted).Assert();
						try
						{
							MethodInfo method2 = typeof(SystemEvents).GetMethod("Startup", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[0], new ParameterModifier[0]);
							if (method2 != null)
							{
								method2.Invoke(null, null);
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				this.control.OnTopMostActiveXParentChanged(EventArgs.Empty);
			}

			// Token: 0x0600175B RID: 5979 RVA: 0x00025C38 File Offset: 0x00024C38
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal void SetExtent(int dwDrawAspect, NativeMethods.tagSIZEL pSizel)
			{
				if ((dwDrawAspect & 1) != 0)
				{
					if (this.activeXState[Control.ActiveXImpl.changingExtents])
					{
						return;
					}
					this.activeXState[Control.ActiveXImpl.changingExtents] = true;
					try
					{
						Size size = new Size(this.HiMetricToPixel(pSizel.cx, pSizel.cy));
						if (this.activeXState[Control.ActiveXImpl.inPlaceActive])
						{
							UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
							if (oleInPlaceSite != null)
							{
								Rectangle bounds = this.control.Bounds;
								bounds.Location = new Point(bounds.X, bounds.Y);
								Size size2 = new Size(size.Width, size.Height);
								bounds.Width = size2.Width;
								bounds.Height = size2.Height;
								oleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT.FromXYWH(bounds.X, bounds.Y, bounds.Width, bounds.Height));
							}
						}
						this.control.Size = size;
						if (!this.control.Size.Equals(size))
						{
							this.activeXState[Control.ActiveXImpl.isDirty] = true;
							if (!this.activeXState[Control.ActiveXImpl.inPlaceActive])
							{
								this.ViewChanged();
							}
							if (!this.activeXState[Control.ActiveXImpl.inPlaceActive] && this.clientSite != null)
							{
								this.clientSite.RequestNewObjectLayout();
							}
						}
						return;
					}
					finally
					{
						this.activeXState[Control.ActiveXImpl.changingExtents] = false;
					}
				}
				Control.ActiveXImpl.ThrowHr(-2147221397);
			}

			// Token: 0x0600175C RID: 5980 RVA: 0x00025DE4 File Offset: 0x00024DE4
			private void SetInPlaceVisible(bool visible)
			{
				this.activeXState[Control.ActiveXImpl.inPlaceVisible] = visible;
				this.control.Visible = visible;
			}

			// Token: 0x0600175D RID: 5981 RVA: 0x00025E04 File Offset: 0x00024E04
			internal void SetObjectRects(NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect)
			{
				Rectangle rectangle = Rectangle.FromLTRB(lprcPosRect.left, lprcPosRect.top, lprcPosRect.right, lprcPosRect.bottom);
				if (this.activeXState[Control.ActiveXImpl.adjustingRect])
				{
					this.adjustRect.left = rectangle.X;
					this.adjustRect.top = rectangle.Y;
					this.adjustRect.right = rectangle.Width + rectangle.X;
					this.adjustRect.bottom = rectangle.Height + rectangle.Y;
				}
				else
				{
					this.activeXState[Control.ActiveXImpl.adjustingRect] = true;
					try
					{
						this.control.Bounds = rectangle;
					}
					finally
					{
						this.activeXState[Control.ActiveXImpl.adjustingRect] = false;
					}
				}
				bool flag = false;
				if (this.clipRegion != IntPtr.Zero)
				{
					this.clipRegion = IntPtr.Zero;
					flag = true;
				}
				if (lprcClipRect != null)
				{
					Rectangle rectangle2 = Rectangle.FromLTRB(lprcClipRect.left, lprcClipRect.top, lprcClipRect.right, lprcClipRect.bottom);
					Rectangle rectangle3;
					if (!rectangle2.IsEmpty)
					{
						rectangle3 = Rectangle.Intersect(rectangle, rectangle2);
					}
					else
					{
						rectangle3 = rectangle;
					}
					if (!rectangle3.Equals(rectangle))
					{
						NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height);
						IntPtr parent = UnsafeNativeMethods.GetParent(new HandleRef(this.control, this.control.Handle));
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(null, parent), new HandleRef(this.control, this.control.Handle), ref rect, 2);
						this.clipRegion = SafeNativeMethods.CreateRectRgn(rect.left, rect.top, rect.right, rect.bottom);
						flag = true;
					}
				}
				if (flag && this.control.IsHandleCreated)
				{
					IntPtr intPtr = this.clipRegion;
					Region region = this.control.Region;
					if (region != null)
					{
						IntPtr hrgn = this.control.GetHRgn(region);
						intPtr = this.MergeRegion(hrgn);
					}
					UnsafeNativeMethods.SetWindowRgn(new HandleRef(this.control, this.control.Handle), new HandleRef(this, intPtr), SafeNativeMethods.IsWindowVisible(new HandleRef(this.control, this.control.Handle)));
				}
				this.control.Invalidate();
			}

			// Token: 0x0600175E RID: 5982 RVA: 0x0002606C File Offset: 0x0002506C
			internal static void ThrowHr(int hr)
			{
				ExternalException ex = new ExternalException(SR.GetString("ExternalException"), hr);
				throw ex;
			}

			// Token: 0x0600175F RID: 5983 RVA: 0x0002608C File Offset: 0x0002508C
			internal int TranslateAccelerator(ref NativeMethods.MSG lpmsg)
			{
				bool flag = false;
				switch (lpmsg.message)
				{
				case 256:
				case 258:
				case 260:
				case 262:
					flag = true;
					break;
				}
				Message message = Message.Create(lpmsg.hwnd, lpmsg.message, lpmsg.wParam, lpmsg.lParam);
				if (flag)
				{
					Control control = Control.FromChildHandleInternal(lpmsg.hwnd);
					if (control != null && (this.control == control || this.control.Contains(control)))
					{
						switch (Control.PreProcessControlMessageInternal(control, ref message))
						{
						case PreProcessControlState.MessageProcessed:
							lpmsg.message = message.Msg;
							lpmsg.wParam = message.WParam;
							lpmsg.lParam = message.LParam;
							return 0;
						case PreProcessControlState.MessageNeeded:
							UnsafeNativeMethods.TranslateMessage(ref lpmsg);
							if (SafeNativeMethods.IsWindowUnicode(new HandleRef(null, lpmsg.hwnd)))
							{
								UnsafeNativeMethods.DispatchMessageW(ref lpmsg);
							}
							else
							{
								UnsafeNativeMethods.DispatchMessageA(ref lpmsg);
							}
							return 0;
						}
					}
				}
				int num = 1;
				UnsafeNativeMethods.IOleControlSite oleControlSite = this.clientSite as UnsafeNativeMethods.IOleControlSite;
				if (oleControlSite != null)
				{
					int num2 = 0;
					if (UnsafeNativeMethods.GetKeyState(16) < 0)
					{
						num2 |= 1;
					}
					if (UnsafeNativeMethods.GetKeyState(17) < 0)
					{
						num2 |= 2;
					}
					if (UnsafeNativeMethods.GetKeyState(18) < 0)
					{
						num2 |= 4;
					}
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						num = oleControlSite.TranslateAccelerator(ref lpmsg, num2);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return num;
			}

			// Token: 0x06001760 RID: 5984 RVA: 0x0002620C File Offset: 0x0002520C
			internal int UIDeactivate()
			{
				if (!this.activeXState[Control.ActiveXImpl.uiActive])
				{
					return 0;
				}
				this.activeXState[Control.ActiveXImpl.uiActive] = false;
				if (this.inPlaceUiWindow != null)
				{
					this.inPlaceUiWindow.SetActiveObject(null, null);
				}
				IntSecurity.UnmanagedCode.Assert();
				this.inPlaceFrame.SetActiveObject(null, null);
				UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
				if (oleInPlaceSite != null)
				{
					oleInPlaceSite.OnUIDeactivate(0);
				}
				return 0;
			}

			// Token: 0x06001761 RID: 5985 RVA: 0x00026284 File Offset: 0x00025284
			internal void Unadvise(int dwConnection)
			{
				if (dwConnection > this.adviseList.Count || this.adviseList[dwConnection - 1] == null)
				{
					Control.ActiveXImpl.ThrowHr(-2147221500);
				}
				IAdviseSink adviseSink = (IAdviseSink)this.adviseList[dwConnection - 1];
				this.adviseList.RemoveAt(dwConnection - 1);
				if (adviseSink != null && UnsafeNativeMethods.IsComObject(adviseSink))
				{
					UnsafeNativeMethods.ReleaseComObject(adviseSink);
				}
			}

			// Token: 0x06001762 RID: 5986 RVA: 0x000262F0 File Offset: 0x000252F0
			internal void UpdateBounds(ref int x, ref int y, ref int width, ref int height, int flags)
			{
				if (!this.activeXState[Control.ActiveXImpl.adjustingRect] && this.activeXState[Control.ActiveXImpl.inPlaceVisible])
				{
					UnsafeNativeMethods.IOleInPlaceSite oleInPlaceSite = this.clientSite as UnsafeNativeMethods.IOleInPlaceSite;
					if (oleInPlaceSite != null)
					{
						NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
						if ((flags & 2) != 0)
						{
							comrect.left = this.control.Left;
							comrect.top = this.control.Top;
						}
						else
						{
							comrect.left = x;
							comrect.top = y;
						}
						if ((flags & 1) != 0)
						{
							comrect.right = comrect.left + this.control.Width;
							comrect.bottom = comrect.top + this.control.Height;
						}
						else
						{
							comrect.right = comrect.left + width;
							comrect.bottom = comrect.top + height;
						}
						this.adjustRect = comrect;
						this.activeXState[Control.ActiveXImpl.adjustingRect] = true;
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							oleInPlaceSite.OnPosRectChange(comrect);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							this.adjustRect = null;
							this.activeXState[Control.ActiveXImpl.adjustingRect] = false;
						}
						if ((flags & 2) == 0)
						{
							x = comrect.left;
							y = comrect.top;
						}
						if ((flags & 1) == 0)
						{
							width = comrect.right - comrect.left;
							height = comrect.bottom - comrect.top;
						}
					}
				}
			}

			// Token: 0x06001763 RID: 5987 RVA: 0x00026468 File Offset: 0x00025468
			internal void UpdateAccelTable()
			{
				this.accelCount = -1;
				UnsafeNativeMethods.IOleControlSite oleControlSite = this.clientSite as UnsafeNativeMethods.IOleControlSite;
				if (oleControlSite != null)
				{
					IntSecurity.UnmanagedCode.Assert();
					oleControlSite.OnControlInfoChanged();
				}
			}

			// Token: 0x06001764 RID: 5988 RVA: 0x0002649C File Offset: 0x0002549C
			internal void ViewChangedInternal()
			{
				this.ViewChanged();
			}

			// Token: 0x06001765 RID: 5989 RVA: 0x000264A4 File Offset: 0x000254A4
			private void ViewChanged()
			{
				if (this.viewAdviseSink != null && !this.activeXState[Control.ActiveXImpl.saving])
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						this.viewAdviseSink.OnViewChange(1, -1);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (this.activeXState[Control.ActiveXImpl.viewAdviseOnlyOnce])
					{
						if (UnsafeNativeMethods.IsComObject(this.viewAdviseSink))
						{
							UnsafeNativeMethods.ReleaseComObject(this.viewAdviseSink);
						}
						this.viewAdviseSink = null;
					}
				}
			}

			// Token: 0x06001766 RID: 5990 RVA: 0x0002652C File Offset: 0x0002552C
			void IWindowTarget.OnHandleChange(IntPtr newHandle)
			{
				this.controlWindowTarget.OnHandleChange(newHandle);
			}

			// Token: 0x06001767 RID: 5991 RVA: 0x0002653C File Offset: 0x0002553C
			void IWindowTarget.OnMessage(ref Message m)
			{
				if (this.activeXState[Control.ActiveXImpl.uiDead])
				{
					if (m.Msg >= 512 && m.Msg <= 522)
					{
						return;
					}
					if (m.Msg >= 161 && m.Msg <= 169)
					{
						return;
					}
					if (m.Msg >= 256 && m.Msg <= 264)
					{
						return;
					}
				}
				IntSecurity.UnmanagedCode.Assert();
				this.controlWindowTarget.OnMessage(ref m);
			}

			// Token: 0x04001176 RID: 4470
			private static readonly int hiMetricPerInch = 2540;

			// Token: 0x04001177 RID: 4471
			private static readonly int viewAdviseOnlyOnce = BitVector32.CreateMask();

			// Token: 0x04001178 RID: 4472
			private static readonly int viewAdvisePrimeFirst = BitVector32.CreateMask(Control.ActiveXImpl.viewAdviseOnlyOnce);

			// Token: 0x04001179 RID: 4473
			private static readonly int eventsFrozen = BitVector32.CreateMask(Control.ActiveXImpl.viewAdvisePrimeFirst);

			// Token: 0x0400117A RID: 4474
			private static readonly int changingExtents = BitVector32.CreateMask(Control.ActiveXImpl.eventsFrozen);

			// Token: 0x0400117B RID: 4475
			private static readonly int saving = BitVector32.CreateMask(Control.ActiveXImpl.changingExtents);

			// Token: 0x0400117C RID: 4476
			private static readonly int isDirty = BitVector32.CreateMask(Control.ActiveXImpl.saving);

			// Token: 0x0400117D RID: 4477
			private static readonly int inPlaceActive = BitVector32.CreateMask(Control.ActiveXImpl.isDirty);

			// Token: 0x0400117E RID: 4478
			private static readonly int inPlaceVisible = BitVector32.CreateMask(Control.ActiveXImpl.inPlaceActive);

			// Token: 0x0400117F RID: 4479
			private static readonly int uiActive = BitVector32.CreateMask(Control.ActiveXImpl.inPlaceVisible);

			// Token: 0x04001180 RID: 4480
			private static readonly int uiDead = BitVector32.CreateMask(Control.ActiveXImpl.uiActive);

			// Token: 0x04001181 RID: 4481
			private static readonly int adjustingRect = BitVector32.CreateMask(Control.ActiveXImpl.uiDead);

			// Token: 0x04001182 RID: 4482
			private static Point logPixels = Point.Empty;

			// Token: 0x04001183 RID: 4483
			private static NativeMethods.tagOLEVERB[] axVerbs;

			// Token: 0x04001184 RID: 4484
			private static int globalActiveXCount = 0;

			// Token: 0x04001185 RID: 4485
			private static bool checkedIE;

			// Token: 0x04001186 RID: 4486
			private static bool isIE;

			// Token: 0x04001187 RID: 4487
			private Control control;

			// Token: 0x04001188 RID: 4488
			private IWindowTarget controlWindowTarget;

			// Token: 0x04001189 RID: 4489
			private IntPtr clipRegion;

			// Token: 0x0400118A RID: 4490
			private UnsafeNativeMethods.IOleClientSite clientSite;

			// Token: 0x0400118B RID: 4491
			private UnsafeNativeMethods.IOleInPlaceUIWindow inPlaceUiWindow;

			// Token: 0x0400118C RID: 4492
			private UnsafeNativeMethods.IOleInPlaceFrame inPlaceFrame;

			// Token: 0x0400118D RID: 4493
			private ArrayList adviseList;

			// Token: 0x0400118E RID: 4494
			private IAdviseSink viewAdviseSink;

			// Token: 0x0400118F RID: 4495
			private BitVector32 activeXState;

			// Token: 0x04001190 RID: 4496
			private Control.AmbientProperty[] ambientProperties;

			// Token: 0x04001191 RID: 4497
			private IntPtr hwndParent;

			// Token: 0x04001192 RID: 4498
			private IntPtr accelTable;

			// Token: 0x04001193 RID: 4499
			private short accelCount = -1;

			// Token: 0x04001194 RID: 4500
			private NativeMethods.COMRECT adjustRect;

			// Token: 0x020001F6 RID: 502
			internal static class AdviseHelper
			{
				// Token: 0x06001769 RID: 5993 RVA: 0x0002668C File Offset: 0x0002568C
				public static bool AdviseConnectionPoint(object connectionPoint, object sink, Type eventInterface, out int cookie)
				{
					bool flag;
					using (Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer comConnectionPointContainer = new Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer(connectionPoint, true))
					{
						flag = Control.ActiveXImpl.AdviseHelper.AdviseConnectionPoint(comConnectionPointContainer, sink, eventInterface, out cookie);
					}
					return flag;
				}

				// Token: 0x0600176A RID: 5994 RVA: 0x000266C8 File Offset: 0x000256C8
				internal static bool AdviseConnectionPoint(Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer cpc, object sink, Type eventInterface, out int cookie)
				{
					bool flag;
					using (Control.ActiveXImpl.AdviseHelper.ComConnectionPoint comConnectionPoint = cpc.FindConnectionPoint(eventInterface))
					{
						using (Control.ActiveXImpl.AdviseHelper.SafeIUnknown safeIUnknown = new Control.ActiveXImpl.AdviseHelper.SafeIUnknown(sink, true))
						{
							flag = comConnectionPoint.Advise(safeIUnknown.DangerousGetHandle(), out cookie);
						}
					}
					return flag;
				}

				// Token: 0x020001F7 RID: 503
				internal class SafeIUnknown : SafeHandle
				{
					// Token: 0x0600176B RID: 5995 RVA: 0x00026728 File Offset: 0x00025728
					public SafeIUnknown(object obj, bool addRefIntPtr)
						: this(obj, addRefIntPtr, Guid.Empty)
					{
					}

					// Token: 0x0600176C RID: 5996 RVA: 0x00026738 File Offset: 0x00025738
					public SafeIUnknown(object obj, bool addRefIntPtr, Guid iid)
						: base(IntPtr.Zero, true)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							IntPtr intPtr;
							if (obj is IntPtr)
							{
								intPtr = (IntPtr)obj;
								if (addRefIntPtr)
								{
									Marshal.AddRef(intPtr);
								}
							}
							else
							{
								intPtr = Marshal.GetIUnknownForObject(obj);
							}
							if (iid != Guid.Empty)
							{
								IntPtr intPtr2 = intPtr;
								try
								{
									intPtr = Control.ActiveXImpl.AdviseHelper.SafeIUnknown.InternalQueryInterface(intPtr, ref iid);
								}
								finally
								{
									Marshal.Release(intPtr2);
								}
							}
							this.handle = intPtr;
						}
					}

					// Token: 0x0600176D RID: 5997 RVA: 0x000267C0 File Offset: 0x000257C0
					private static IntPtr InternalQueryInterface(IntPtr pUnk, ref Guid iid)
					{
						IntPtr intPtr;
						if (Marshal.QueryInterface(pUnk, ref iid, out intPtr) != 0 || intPtr == IntPtr.Zero)
						{
							throw new InvalidCastException(SR.GetString("AxInterfaceNotSupported"));
						}
						return intPtr;
					}

					// Token: 0x170002B0 RID: 688
					// (get) Token: 0x0600176E RID: 5998 RVA: 0x000267F8 File Offset: 0x000257F8
					public sealed override bool IsInvalid
					{
						get
						{
							return base.IsClosed || IntPtr.Zero == this.handle;
						}
					}

					// Token: 0x0600176F RID: 5999 RVA: 0x00026814 File Offset: 0x00025814
					protected sealed override bool ReleaseHandle()
					{
						IntPtr handle = this.handle;
						this.handle = IntPtr.Zero;
						if (IntPtr.Zero != handle)
						{
							Marshal.Release(handle);
						}
						return true;
					}

					// Token: 0x06001770 RID: 6000 RVA: 0x00026848 File Offset: 0x00025848
					protected V LoadVtable<V>()
					{
						IntPtr intPtr = Marshal.ReadIntPtr(this.handle, 0);
						return (V)((object)Marshal.PtrToStructure(intPtr, typeof(V)));
					}
				}

				// Token: 0x020001F8 RID: 504
				internal sealed class ComConnectionPointContainer : Control.ActiveXImpl.AdviseHelper.SafeIUnknown
				{
					// Token: 0x06001771 RID: 6001 RVA: 0x00026877 File Offset: 0x00025877
					public ComConnectionPointContainer(object obj, bool addRefIntPtr)
						: base(obj, addRefIntPtr, typeof(IConnectionPointContainer).GUID)
					{
						this.vtbl = base.LoadVtable<Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.VTABLE>();
					}

					// Token: 0x06001772 RID: 6002 RVA: 0x0002689C File Offset: 0x0002589C
					public Control.ActiveXImpl.AdviseHelper.ComConnectionPoint FindConnectionPoint(Type eventInterface)
					{
						Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD findConnectionPointD = (Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD)Marshal.GetDelegateForFunctionPointer(this.vtbl.FindConnectionPointPtr, typeof(Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.FindConnectionPointD));
						IntPtr zero = IntPtr.Zero;
						Guid guid = eventInterface.GUID;
						if (findConnectionPointD(this.handle, ref guid, out zero) != 0 || zero == IntPtr.Zero)
						{
							throw new ArgumentException(SR.GetString("AXNoConnectionPoint", new object[] { eventInterface.Name }));
						}
						return new Control.ActiveXImpl.AdviseHelper.ComConnectionPoint(zero, false);
					}

					// Token: 0x04001195 RID: 4501
					private Control.ActiveXImpl.AdviseHelper.ComConnectionPointContainer.VTABLE vtbl;

					// Token: 0x020001F9 RID: 505
					[StructLayout(LayoutKind.Sequential)]
					private class VTABLE
					{
						// Token: 0x04001196 RID: 4502
						public IntPtr QueryInterfacePtr;

						// Token: 0x04001197 RID: 4503
						public IntPtr AddRefPtr;

						// Token: 0x04001198 RID: 4504
						public IntPtr ReleasePtr;

						// Token: 0x04001199 RID: 4505
						public IntPtr EnumConnectionPointsPtr;

						// Token: 0x0400119A RID: 4506
						public IntPtr FindConnectionPointPtr;
					}

					// Token: 0x020001FA RID: 506
					// (Invoke) Token: 0x06001775 RID: 6005
					[UnmanagedFunctionPointer(CallingConvention.StdCall)]
					private delegate int FindConnectionPointD(IntPtr This, ref Guid iid, out IntPtr ppv);
				}

				// Token: 0x020001FB RID: 507
				internal sealed class ComConnectionPoint : Control.ActiveXImpl.AdviseHelper.SafeIUnknown
				{
					// Token: 0x06001778 RID: 6008 RVA: 0x0002692F File Offset: 0x0002592F
					public ComConnectionPoint(object obj, bool addRefIntPtr)
						: base(obj, addRefIntPtr, typeof(IConnectionPoint).GUID)
					{
						this.vtbl = base.LoadVtable<Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.VTABLE>();
					}

					// Token: 0x06001779 RID: 6009 RVA: 0x00026954 File Offset: 0x00025954
					public bool Advise(IntPtr punkEventSink, out int cookie)
					{
						Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD adviseD = (Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD)Marshal.GetDelegateForFunctionPointer(this.vtbl.AdvisePtr, typeof(Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.AdviseD));
						return adviseD(this.handle, punkEventSink, out cookie) == 0;
					}

					// Token: 0x0400119B RID: 4507
					private Control.ActiveXImpl.AdviseHelper.ComConnectionPoint.VTABLE vtbl;

					// Token: 0x020001FC RID: 508
					[StructLayout(LayoutKind.Sequential)]
					private class VTABLE
					{
						// Token: 0x0400119C RID: 4508
						public IntPtr QueryInterfacePtr;

						// Token: 0x0400119D RID: 4509
						public IntPtr AddRefPtr;

						// Token: 0x0400119E RID: 4510
						public IntPtr ReleasePtr;

						// Token: 0x0400119F RID: 4511
						public IntPtr GetConnectionInterfacePtr;

						// Token: 0x040011A0 RID: 4512
						public IntPtr GetConnectionPointContainterPtr;

						// Token: 0x040011A1 RID: 4513
						public IntPtr AdvisePtr;

						// Token: 0x040011A2 RID: 4514
						public IntPtr UnadvisePtr;

						// Token: 0x040011A3 RID: 4515
						public IntPtr EnumConnectionsPtr;
					}

					// Token: 0x020001FD RID: 509
					// (Invoke) Token: 0x0600177C RID: 6012
					[UnmanagedFunctionPointer(CallingConvention.StdCall)]
					private delegate int AdviseD(IntPtr This, IntPtr punkEventSink, out int cookie);
				}
			}

			// Token: 0x020001FE RID: 510
			private class PropertyBagStream : UnsafeNativeMethods.IPropertyBag
			{
				// Token: 0x0600177F RID: 6015 RVA: 0x0002699C File Offset: 0x0002599C
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				internal void Read(UnsafeNativeMethods.IStream istream)
				{
					Stream stream = new DataStreamFromComStream(istream);
					byte[] array = new byte[4096];
					int num = 0;
					int num2 = stream.Read(array, num, 4096);
					int num3 = num2;
					while (num2 == 4096)
					{
						byte[] array2 = new byte[array.Length + 4096];
						Array.Copy(array, array2, array.Length);
						array = array2;
						num += 4096;
						num2 = stream.Read(array, num, 4096);
						num3 += num2;
					}
					stream = new MemoryStream(array);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					try
					{
						this.bag = (Hashtable)binaryFormatter.Deserialize(stream);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
						this.bag = new Hashtable();
					}
				}

				// Token: 0x06001780 RID: 6016 RVA: 0x00026A60 File Offset: 0x00025A60
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				int UnsafeNativeMethods.IPropertyBag.Read(string pszPropName, ref object pVar, UnsafeNativeMethods.IErrorLog pErrorLog)
				{
					if (!this.bag.Contains(pszPropName))
					{
						return -2147024809;
					}
					pVar = this.bag[pszPropName];
					return 0;
				}

				// Token: 0x06001781 RID: 6017 RVA: 0x00026A85 File Offset: 0x00025A85
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				int UnsafeNativeMethods.IPropertyBag.Write(string pszPropName, ref object pVar)
				{
					this.bag[pszPropName] = pVar;
					return 0;
				}

				// Token: 0x06001782 RID: 6018 RVA: 0x00026A98 File Offset: 0x00025A98
				[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
				internal void Write(UnsafeNativeMethods.IStream istream)
				{
					Stream stream = new DataStreamFromComStream(istream);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(stream, this.bag);
				}

				// Token: 0x040011A4 RID: 4516
				private Hashtable bag = new Hashtable();
			}
		}

		// Token: 0x020001FF RID: 511
		private class AxSourcingSite : ISite, IServiceProvider
		{
			// Token: 0x06001784 RID: 6020 RVA: 0x00026AD2 File Offset: 0x00025AD2
			internal AxSourcingSite(IComponent component, UnsafeNativeMethods.IOleClientSite clientSite, string name)
			{
				this.component = component;
				this.clientSite = clientSite;
				this.name = name;
			}

			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x06001785 RID: 6021 RVA: 0x00026AEF File Offset: 0x00025AEF
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x06001786 RID: 6022 RVA: 0x00026AF7 File Offset: 0x00025AF7
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06001787 RID: 6023 RVA: 0x00026AFC File Offset: 0x00025AFC
			public object GetService(Type service)
			{
				object obj = null;
				if (service == typeof(HtmlDocument))
				{
					UnsafeNativeMethods.IOleContainer oleContainer;
					int container;
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						container = this.clientSite.GetContainer(out oleContainer);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (NativeMethods.Succeeded(container) && oleContainer is UnsafeNativeMethods.IHTMLDocument)
					{
						if (this.shimManager == null)
						{
							this.shimManager = new HtmlShimManager();
						}
						obj = new HtmlDocument(this.shimManager, oleContainer as UnsafeNativeMethods.IHTMLDocument);
					}
				}
				else if (this.clientSite.GetType().IsAssignableFrom(service))
				{
					IntSecurity.UnmanagedCode.Demand();
					obj = this.clientSite;
				}
				return obj;
			}

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06001788 RID: 6024 RVA: 0x00026BA4 File Offset: 0x00025BA4
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06001789 RID: 6025 RVA: 0x00026BA7 File Offset: 0x00025BA7
			// (set) Token: 0x0600178A RID: 6026 RVA: 0x00026BAF File Offset: 0x00025BAF
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					if (value == null || this.name == null)
					{
						this.name = value;
					}
				}
			}

			// Token: 0x040011A5 RID: 4517
			private IComponent component;

			// Token: 0x040011A6 RID: 4518
			private UnsafeNativeMethods.IOleClientSite clientSite;

			// Token: 0x040011A7 RID: 4519
			private string name;

			// Token: 0x040011A8 RID: 4520
			private HtmlShimManager shimManager;
		}

		// Token: 0x02000200 RID: 512
		private class ActiveXFontMarshaler : ICustomMarshaler
		{
			// Token: 0x0600178B RID: 6027 RVA: 0x00026BC3 File Offset: 0x00025BC3
			public void CleanUpManagedData(object obj)
			{
			}

			// Token: 0x0600178C RID: 6028 RVA: 0x00026BC5 File Offset: 0x00025BC5
			public void CleanUpNativeData(IntPtr pObj)
			{
				Marshal.Release(pObj);
			}

			// Token: 0x0600178D RID: 6029 RVA: 0x00026BCE File Offset: 0x00025BCE
			internal static ICustomMarshaler GetInstance(string cookie)
			{
				if (Control.ActiveXFontMarshaler.instance == null)
				{
					Control.ActiveXFontMarshaler.instance = new Control.ActiveXFontMarshaler();
				}
				return Control.ActiveXFontMarshaler.instance;
			}

			// Token: 0x0600178E RID: 6030 RVA: 0x00026BE6 File Offset: 0x00025BE6
			public int GetNativeDataSize()
			{
				return -1;
			}

			// Token: 0x0600178F RID: 6031 RVA: 0x00026BEC File Offset: 0x00025BEC
			public IntPtr MarshalManagedToNative(object obj)
			{
				Font font = (Font)obj;
				NativeMethods.tagFONTDESC tagFONTDESC = new NativeMethods.tagFONTDESC();
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					font.ToLogFont(logfont);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				tagFONTDESC.lpstrName = font.Name;
				tagFONTDESC.cySize = (long)(font.SizeInPoints * 10000f);
				tagFONTDESC.sWeight = (short)logfont.lfWeight;
				tagFONTDESC.sCharset = (short)logfont.lfCharSet;
				tagFONTDESC.fItalic = font.Italic;
				tagFONTDESC.fUnderline = font.Underline;
				tagFONTDESC.fStrikethrough = font.Strikeout;
				Guid guid = typeof(UnsafeNativeMethods.IFont).GUID;
				UnsafeNativeMethods.IFont font2 = UnsafeNativeMethods.OleCreateFontIndirect(tagFONTDESC, ref guid);
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(font2);
				IntPtr intPtr;
				int num = Marshal.QueryInterface(iunknownForObject, ref guid, out intPtr);
				Marshal.Release(iunknownForObject);
				if (NativeMethods.Failed(num))
				{
					Marshal.ThrowExceptionForHR(num);
				}
				return intPtr;
			}

			// Token: 0x06001790 RID: 6032 RVA: 0x00026CDC File Offset: 0x00025CDC
			public object MarshalNativeToManaged(IntPtr pObj)
			{
				UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)Marshal.GetObjectForIUnknown(pObj);
				IntPtr intPtr = IntPtr.Zero;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					intPtr = font.GetHFont();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				Font font2 = null;
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					font2 = Font.FromHfont(intPtr);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					font2 = Control.DefaultFont;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return font2;
			}

			// Token: 0x040011A9 RID: 4521
			private static Control.ActiveXFontMarshaler instance;
		}

		// Token: 0x02000201 RID: 513
		private class ActiveXVerbEnum : UnsafeNativeMethods.IEnumOLEVERB
		{
			// Token: 0x06001792 RID: 6034 RVA: 0x00026D78 File Offset: 0x00025D78
			internal ActiveXVerbEnum(NativeMethods.tagOLEVERB[] verbs)
			{
				this.verbs = verbs;
				this.current = 0;
			}

			// Token: 0x06001793 RID: 6035 RVA: 0x00026D90 File Offset: 0x00025D90
			public int Next(int celt, NativeMethods.tagOLEVERB rgelt, int[] pceltFetched)
			{
				int num = 0;
				if (celt != 1)
				{
					celt = 1;
				}
				while (celt > 0 && this.current < this.verbs.Length)
				{
					rgelt.lVerb = this.verbs[this.current].lVerb;
					rgelt.lpszVerbName = this.verbs[this.current].lpszVerbName;
					rgelt.fuFlags = this.verbs[this.current].fuFlags;
					rgelt.grfAttribs = this.verbs[this.current].grfAttribs;
					celt--;
					this.current++;
					num++;
				}
				if (pceltFetched != null)
				{
					pceltFetched[0] = num;
				}
				if (celt != 0)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06001794 RID: 6036 RVA: 0x00026E43 File Offset: 0x00025E43
			public int Skip(int celt)
			{
				if (this.current + celt < this.verbs.Length)
				{
					this.current += celt;
					return 0;
				}
				this.current = this.verbs.Length;
				return 1;
			}

			// Token: 0x06001795 RID: 6037 RVA: 0x00026E76 File Offset: 0x00025E76
			public void Reset()
			{
				this.current = 0;
			}

			// Token: 0x06001796 RID: 6038 RVA: 0x00026E7F File Offset: 0x00025E7F
			public void Clone(out UnsafeNativeMethods.IEnumOLEVERB ppenum)
			{
				ppenum = new Control.ActiveXVerbEnum(this.verbs);
			}

			// Token: 0x040011AA RID: 4522
			private NativeMethods.tagOLEVERB[] verbs;

			// Token: 0x040011AB RID: 4523
			private int current;
		}

		// Token: 0x02000202 RID: 514
		private class AmbientProperty
		{
			// Token: 0x06001797 RID: 6039 RVA: 0x00026E8E File Offset: 0x00025E8E
			internal AmbientProperty(string name, int dispID)
			{
				this.name = name;
				this.dispID = dispID;
				this.value = null;
				this.empty = true;
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06001798 RID: 6040 RVA: 0x00026EB2 File Offset: 0x00025EB2
			internal string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06001799 RID: 6041 RVA: 0x00026EBA File Offset: 0x00025EBA
			internal int DispID
			{
				get
				{
					return this.dispID;
				}
			}

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x0600179A RID: 6042 RVA: 0x00026EC2 File Offset: 0x00025EC2
			internal bool Empty
			{
				get
				{
					return this.empty;
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x0600179B RID: 6043 RVA: 0x00026ECA File Offset: 0x00025ECA
			// (set) Token: 0x0600179C RID: 6044 RVA: 0x00026ED2 File Offset: 0x00025ED2
			internal object Value
			{
				get
				{
					return this.value;
				}
				set
				{
					this.value = value;
					this.empty = false;
				}
			}

			// Token: 0x0600179D RID: 6045 RVA: 0x00026EE2 File Offset: 0x00025EE2
			internal void ResetValue()
			{
				this.empty = true;
				this.value = null;
			}

			// Token: 0x040011AC RID: 4524
			private string name;

			// Token: 0x040011AD RID: 4525
			private int dispID;

			// Token: 0x040011AE RID: 4526
			private object value;

			// Token: 0x040011AF RID: 4527
			private bool empty;
		}

		// Token: 0x02000203 RID: 515
		private class MetafileDCWrapper : IDisposable
		{
			// Token: 0x0600179E RID: 6046 RVA: 0x00026EF4 File Offset: 0x00025EF4
			internal MetafileDCWrapper(HandleRef hOriginalDC, Size size)
			{
				if (size.Width < 0 || size.Height < 0)
				{
					throw new ArgumentException("size", SR.GetString("ControlMetaFileDCWrapperSizeInvalid"));
				}
				this.hMetafileDC = hOriginalDC;
				this.destRect = new NativeMethods.RECT(0, 0, size.Width, size.Height);
				this.hBitmapDC = new HandleRef(this, UnsafeNativeMethods.CreateCompatibleDC(NativeMethods.NullHandleRef));
				int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(this.hBitmapDC, 14);
				int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(this.hBitmapDC, 12);
				this.hBitmap = new HandleRef(this, SafeNativeMethods.CreateBitmap(size.Width, size.Height, deviceCaps, deviceCaps2, IntPtr.Zero));
				this.hOriginalBmp = new HandleRef(this, SafeNativeMethods.SelectObject(this.hBitmapDC, this.hBitmap));
			}

			// Token: 0x0600179F RID: 6047 RVA: 0x00026FF4 File Offset: 0x00025FF4
			~MetafileDCWrapper()
			{
				((IDisposable)this).Dispose();
			}

			// Token: 0x060017A0 RID: 6048 RVA: 0x00027020 File Offset: 0x00026020
			void IDisposable.Dispose()
			{
				if (this.hBitmapDC.Handle == IntPtr.Zero || this.hMetafileDC.Handle == IntPtr.Zero || this.hBitmap.Handle == IntPtr.Zero)
				{
					return;
				}
				try
				{
					this.DICopy(this.hMetafileDC, this.hBitmapDC, this.destRect, true);
					SafeNativeMethods.SelectObject(this.hBitmapDC, this.hOriginalBmp);
					SafeNativeMethods.DeleteObject(this.hBitmap);
					UnsafeNativeMethods.DeleteCompatibleDC(this.hBitmapDC);
				}
				finally
				{
					this.hBitmapDC = NativeMethods.NullHandleRef;
					this.hBitmap = NativeMethods.NullHandleRef;
					this.hOriginalBmp = NativeMethods.NullHandleRef;
					GC.SuppressFinalize(this);
				}
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x060017A1 RID: 6049 RVA: 0x000270F4 File Offset: 0x000260F4
			internal IntPtr HDC
			{
				get
				{
					return this.hBitmapDC.Handle;
				}
			}

			// Token: 0x060017A2 RID: 6050 RVA: 0x00027104 File Offset: 0x00026104
			private unsafe bool DICopy(HandleRef hdcDest, HandleRef hdcSrc, NativeMethods.RECT rect, bool bStretch)
			{
				bool flag = false;
				HandleRef handleRef = new HandleRef(this, SafeNativeMethods.CreateBitmap(1, 1, 1, 1, IntPtr.Zero));
				if (handleRef.Handle == IntPtr.Zero)
				{
					return flag;
				}
				try
				{
					HandleRef handleRef2 = new HandleRef(this, SafeNativeMethods.SelectObject(hdcSrc, handleRef));
					if (handleRef2.Handle == IntPtr.Zero)
					{
						return flag;
					}
					SafeNativeMethods.SelectObject(hdcSrc, handleRef2);
					NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
					if (UnsafeNativeMethods.GetObject(handleRef2, Marshal.SizeOf(bitmap), bitmap) == 0)
					{
						return flag;
					}
					NativeMethods.BITMAPINFO_FLAT bitmapinfo_FLAT = default(NativeMethods.BITMAPINFO_FLAT);
					bitmapinfo_FLAT.bmiHeader_biSize = Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
					bitmapinfo_FLAT.bmiHeader_biWidth = bitmap.bmWidth;
					bitmapinfo_FLAT.bmiHeader_biHeight = bitmap.bmHeight;
					bitmapinfo_FLAT.bmiHeader_biPlanes = 1;
					bitmapinfo_FLAT.bmiHeader_biBitCount = bitmap.bmBitsPixel;
					bitmapinfo_FLAT.bmiHeader_biCompression = 0;
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
					bitmapinfo_FLAT.bmiHeader_biXPelsPerMeter = 0;
					bitmapinfo_FLAT.bmiHeader_biYPelsPerMeter = 0;
					bitmapinfo_FLAT.bmiHeader_biClrUsed = 0;
					bitmapinfo_FLAT.bmiHeader_biClrImportant = 0;
					bitmapinfo_FLAT.bmiColors = new byte[1024];
					long num = 1L << (int)((bitmap.bmBitsPixel * bitmap.bmPlanes) & 31);
					if (num <= 256L)
					{
						byte[] array = new byte[Marshal.SizeOf(typeof(NativeMethods.PALETTEENTRY)) * 256];
						SafeNativeMethods.GetSystemPaletteEntries(hdcSrc, 0, (int)num, array);
						try
						{
							fixed (byte* ptr = bitmapinfo_FLAT.bmiColors)
							{
								try
								{
									fixed (byte* ptr2 = array)
									{
										NativeMethods.RGBQUAD* ptr3 = (NativeMethods.RGBQUAD*)ptr;
										NativeMethods.PALETTEENTRY* ptr4 = (NativeMethods.PALETTEENTRY*)ptr2;
										for (long num2 = 0L; num2 < (long)((int)num); num2 += 1L)
										{
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbRed = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peRed;
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbBlue = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peBlue;
											ptr3[num2 * (long)sizeof(NativeMethods.RGBQUAD) / (long)sizeof(NativeMethods.RGBQUAD)].rgbGreen = ptr4[num2 * (long)sizeof(NativeMethods.PALETTEENTRY) / (long)sizeof(NativeMethods.PALETTEENTRY)].peGreen;
										}
									}
								}
								finally
								{
									byte* ptr2 = null;
								}
							}
						}
						finally
						{
							byte* ptr = null;
						}
					}
					long num3 = (long)bitmap.bmBitsPixel * (long)bitmap.bmWidth;
					long num4 = (num3 + 7L) / 8L;
					long num5 = num4 * (long)bitmap.bmHeight;
					byte[] array2 = new byte[num5];
					if (SafeNativeMethods.GetDIBits(hdcSrc, handleRef2, 0, bitmap.bmHeight, array2, ref bitmapinfo_FLAT, 0) == 0)
					{
						return flag;
					}
					int num6;
					int num7;
					int num8;
					int num9;
					if (bStretch)
					{
						num6 = rect.left;
						num7 = rect.top;
						num8 = rect.right - rect.left;
						num9 = rect.bottom - rect.top;
					}
					else
					{
						num6 = rect.left;
						num7 = rect.top;
						num8 = bitmap.bmWidth;
						num9 = bitmap.bmHeight;
					}
					int num10 = SafeNativeMethods.StretchDIBits(hdcDest, num6, num7, num8, num9, 0, 0, bitmap.bmWidth, bitmap.bmHeight, array2, ref bitmapinfo_FLAT, 0, 13369376);
					if (num10 == -1)
					{
						return flag;
					}
					flag = true;
				}
				finally
				{
					SafeNativeMethods.DeleteObject(handleRef);
				}
				return flag;
			}

			// Token: 0x040011B0 RID: 4528
			private HandleRef hBitmapDC = NativeMethods.NullHandleRef;

			// Token: 0x040011B1 RID: 4529
			private HandleRef hBitmap = NativeMethods.NullHandleRef;

			// Token: 0x040011B2 RID: 4530
			private HandleRef hOriginalBmp = NativeMethods.NullHandleRef;

			// Token: 0x040011B3 RID: 4531
			private HandleRef hMetafileDC = NativeMethods.NullHandleRef;

			// Token: 0x040011B4 RID: 4532
			private NativeMethods.RECT destRect;
		}

		// Token: 0x02000204 RID: 516
		[ComVisible(true)]
		public class ControlAccessibleObject : AccessibleObject
		{
			// Token: 0x060017A3 RID: 6051 RVA: 0x00027488 File Offset: 0x00026488
			public ControlAccessibleObject(Control ownerControl)
			{
				if (ownerControl == null)
				{
					throw new ArgumentNullException("ownerControl");
				}
				this.ownerControl = ownerControl;
				IntPtr intPtr = ownerControl.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.Handle = intPtr;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x060017A4 RID: 6052 RVA: 0x000274EC File Offset: 0x000264EC
			internal ControlAccessibleObject(Control ownerControl, int accObjId)
			{
				if (ownerControl == null)
				{
					throw new ArgumentNullException("ownerControl");
				}
				base.AccessibleObjectId = accObjId;
				this.ownerControl = ownerControl;
				IntPtr intPtr = ownerControl.Handle;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					this.Handle = intPtr;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}

			// Token: 0x060017A5 RID: 6053 RVA: 0x00027558 File Offset: 0x00026558
			internal override int[] GetSysChildOrder()
			{
				if (this.ownerControl.GetStyle(ControlStyles.ContainerControl))
				{
					return this.ownerControl.GetChildWindowsInTabOrder();
				}
				return base.GetSysChildOrder();
			}

			// Token: 0x060017A6 RID: 6054 RVA: 0x0002757C File Offset: 0x0002657C
			internal override bool GetSysChild(AccessibleNavigation navdir, out AccessibleObject accessibleObject)
			{
				accessibleObject = null;
				Control parentInternal = this.ownerControl.ParentInternal;
				int num = -1;
				Control[] array = null;
				switch (navdir)
				{
				case AccessibleNavigation.Next:
					if (base.IsNonClientObject && parentInternal != null)
					{
						array = parentInternal.GetChildControlsInTabOrder(true);
						num = Array.IndexOf<Control>(array, this.ownerControl);
						if (num != -1)
						{
							num++;
						}
					}
					break;
				case AccessibleNavigation.Previous:
					if (base.IsNonClientObject && parentInternal != null)
					{
						array = parentInternal.GetChildControlsInTabOrder(true);
						num = Array.IndexOf<Control>(array, this.ownerControl);
						if (num != -1)
						{
							num--;
						}
					}
					break;
				case AccessibleNavigation.FirstChild:
					if (base.IsClientObject)
					{
						array = this.ownerControl.GetChildControlsInTabOrder(true);
						num = 0;
					}
					break;
				case AccessibleNavigation.LastChild:
					if (base.IsClientObject)
					{
						array = this.ownerControl.GetChildControlsInTabOrder(true);
						num = array.Length - 1;
					}
					break;
				}
				if (array == null || array.Length == 0)
				{
					return false;
				}
				if (num >= 0 && num < array.Length)
				{
					accessibleObject = array[num].NcAccessibilityObject;
				}
				return true;
			}

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x060017A7 RID: 6055 RVA: 0x00027668 File Offset: 0x00026668
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = this.ownerControl.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return base.DefaultAction;
				}
			}

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x060017A8 RID: 6056 RVA: 0x0002768C File Offset: 0x0002668C
			public override string Description
			{
				get
				{
					string accessibleDescription = this.ownerControl.AccessibleDescription;
					if (accessibleDescription != null)
					{
						return accessibleDescription;
					}
					return base.Description;
				}
			}

			// Token: 0x170002BC RID: 700
			// (get) Token: 0x060017A9 RID: 6057 RVA: 0x000276B0 File Offset: 0x000266B0
			// (set) Token: 0x060017AA RID: 6058 RVA: 0x000276B8 File Offset: 0x000266B8
			public IntPtr Handle
			{
				get
				{
					return this.handle;
				}
				set
				{
					IntSecurity.UnmanagedCode.Demand();
					if (this.handle != value)
					{
						this.handle = value;
						if (Control.ControlAccessibleObject.oleAccAvailable == IntPtr.Zero)
						{
							return;
						}
						bool flag = false;
						if (Control.ControlAccessibleObject.oleAccAvailable == NativeMethods.InvalidIntPtr)
						{
							Control.ControlAccessibleObject.oleAccAvailable = UnsafeNativeMethods.LoadLibrary("oleacc.dll");
							flag = Control.ControlAccessibleObject.oleAccAvailable != IntPtr.Zero;
						}
						if (this.handle != IntPtr.Zero && Control.ControlAccessibleObject.oleAccAvailable != IntPtr.Zero)
						{
							base.UseStdAccessibleObjects(this.handle);
						}
						if (flag)
						{
							UnsafeNativeMethods.FreeLibrary(new HandleRef(null, Control.ControlAccessibleObject.oleAccAvailable));
						}
					}
				}
			}

			// Token: 0x170002BD RID: 701
			// (get) Token: 0x060017AB RID: 6059 RVA: 0x00027770 File Offset: 0x00026770
			public override string Help
			{
				get
				{
					QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[Control.EventQueryAccessibilityHelp];
					if (queryAccessibilityHelpEventHandler != null)
					{
						QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
						queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
						return queryAccessibilityHelpEventArgs.HelpString;
					}
					return base.Help;
				}
			}

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x060017AC RID: 6060 RVA: 0x000277BC File Offset: 0x000267BC
			public override string KeyboardShortcut
			{
				get
				{
					char mnemonic = WindowsFormsUtils.GetMnemonic(this.TextLabel, false);
					if (mnemonic != '\0')
					{
						return "Alt+" + mnemonic;
					}
					return null;
				}
			}

			// Token: 0x170002BF RID: 703
			// (get) Token: 0x060017AD RID: 6061 RVA: 0x000277EC File Offset: 0x000267EC
			// (set) Token: 0x060017AE RID: 6062 RVA: 0x00027815 File Offset: 0x00026815
			public override string Name
			{
				get
				{
					string accessibleName = this.ownerControl.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return WindowsFormsUtils.TextWithoutMnemonics(this.TextLabel);
				}
				set
				{
					this.ownerControl.AccessibleName = value;
				}
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x060017AF RID: 6063 RVA: 0x00027823 File Offset: 0x00026823
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return base.Parent;
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0002782C File Offset: 0x0002682C
			internal string TextLabel
			{
				get
				{
					if (this.ownerControl.GetStyle(ControlStyles.UseTextForAccessibility))
					{
						string text = this.ownerControl.Text;
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
					Label previousLabel = this.PreviousLabel;
					if (previousLabel != null)
					{
						string text2 = previousLabel.Text;
						if (!string.IsNullOrEmpty(text2))
						{
							return text2;
						}
					}
					return null;
				}
			}

			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0002787D File Offset: 0x0002687D
			public Control Owner
			{
				get
				{
					return this.ownerControl;
				}
			}

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00027888 File Offset: 0x00026888
			internal Label PreviousLabel
			{
				get
				{
					Control parentInternal = this.Owner.ParentInternal;
					if (parentInternal == null)
					{
						return null;
					}
					ContainerControl containerControl = parentInternal.GetContainerControlInternal() as ContainerControl;
					if (containerControl == null)
					{
						return null;
					}
					for (Control control = containerControl.GetNextControl(this.Owner, false); control != null; control = containerControl.GetNextControl(control, false))
					{
						if (control is Label)
						{
							return control as Label;
						}
						if (control.Visible && control.TabStop)
						{
							break;
						}
					}
					return null;
				}
			}

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x060017B3 RID: 6067 RVA: 0x000278F4 File Offset: 0x000268F4
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerControl.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return base.Role;
				}
			}

			// Token: 0x060017B4 RID: 6068 RVA: 0x0002791C File Offset: 0x0002691C
			public override int GetHelpTopic(out string fileName)
			{
				int num = 0;
				QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[Control.EventQueryAccessibilityHelp];
				if (queryAccessibilityHelpEventHandler != null)
				{
					QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
					queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
					fileName = queryAccessibilityHelpEventArgs.HelpNamespace;
					if (!string.IsNullOrEmpty(fileName))
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					try
					{
						num = int.Parse(queryAccessibilityHelpEventArgs.HelpKeyword, CultureInfo.InvariantCulture);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsSecurityOrCriticalException(ex))
						{
							throw;
						}
					}
					return num;
				}
				return base.GetHelpTopic(out fileName);
			}

			// Token: 0x060017B5 RID: 6069 RVA: 0x000279B0 File Offset: 0x000269B0
			public void NotifyClients(AccessibleEvents accEvent)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), -4, 0);
			}

			// Token: 0x060017B6 RID: 6070 RVA: 0x000279C7 File Offset: 0x000269C7
			public void NotifyClients(AccessibleEvents accEvent, int childID)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), -4, childID + 1);
			}

			// Token: 0x060017B7 RID: 6071 RVA: 0x000279E0 File Offset: 0x000269E0
			public void NotifyClients(AccessibleEvents accEvent, int objectID, int childID)
			{
				UnsafeNativeMethods.NotifyWinEvent((int)accEvent, new HandleRef(this, this.Handle), objectID, childID + 1);
			}

			// Token: 0x060017B8 RID: 6072 RVA: 0x000279F8 File Offset: 0x000269F8
			public override string ToString()
			{
				if (this.Owner != null)
				{
					return "ControlAccessibleObject: Owner = " + this.Owner.ToString();
				}
				return "ControlAccessibleObject: Owner = null";
			}

			// Token: 0x040011B5 RID: 4533
			private static IntPtr oleAccAvailable = NativeMethods.InvalidIntPtr;

			// Token: 0x040011B6 RID: 4534
			private IntPtr handle = IntPtr.Zero;

			// Token: 0x040011B7 RID: 4535
			private Control ownerControl;
		}

		// Token: 0x02000205 RID: 517
		internal sealed class FontHandleWrapper : MarshalByRefObject, IDisposable
		{
			// Token: 0x060017BA RID: 6074 RVA: 0x00027A29 File Offset: 0x00026A29
			internal FontHandleWrapper(Font font)
			{
				this.handle = font.ToHfont();
				global::System.Internal.HandleCollector.Add(this.handle, NativeMethods.CommonHandles.GDI);
			}

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x060017BB RID: 6075 RVA: 0x00027A4E File Offset: 0x00026A4E
			internal IntPtr Handle
			{
				get
				{
					return this.handle;
				}
			}

			// Token: 0x060017BC RID: 6076 RVA: 0x00027A56 File Offset: 0x00026A56
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060017BD RID: 6077 RVA: 0x00027A65 File Offset: 0x00026A65
			private void Dispose(bool disposing)
			{
				if (this.handle != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(this, this.handle));
					this.handle = IntPtr.Zero;
				}
			}

			// Token: 0x060017BE RID: 6078 RVA: 0x00027A98 File Offset: 0x00026A98
			~FontHandleWrapper()
			{
				this.Dispose(false);
			}

			// Token: 0x040011B8 RID: 4536
			private IntPtr handle;
		}

		// Token: 0x02000206 RID: 518
		private class ThreadMethodEntry : IAsyncResult
		{
			// Token: 0x060017BF RID: 6079 RVA: 0x00027AC8 File Offset: 0x00026AC8
			internal ThreadMethodEntry(Control caller, Control marshaler, Delegate method, object[] args, bool synchronous, ExecutionContext executionContext)
			{
				this.caller = caller;
				this.marshaler = marshaler;
				this.method = method;
				this.args = args;
				this.exception = null;
				this.retVal = null;
				this.synchronous = synchronous;
				this.isCompleted = false;
				this.resetEvent = null;
				this.executionContext = executionContext;
			}

			// Token: 0x060017C0 RID: 6080 RVA: 0x00027B30 File Offset: 0x00026B30
			~ThreadMethodEntry()
			{
				if (this.resetEvent != null)
				{
					this.resetEvent.Close();
				}
			}

			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00027B6C File Offset: 0x00026B6C
			public object AsyncState
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00027B70 File Offset: 0x00026B70
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					if (this.resetEvent == null)
					{
						lock (this.invokeSyncObject)
						{
							if (this.resetEvent == null)
							{
								this.resetEvent = new ManualResetEvent(false);
								if (this.isCompleted)
								{
									this.resetEvent.Set();
								}
							}
						}
					}
					return this.resetEvent;
				}
			}

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00027BDC File Offset: 0x00026BDC
			public bool CompletedSynchronously
			{
				get
				{
					return this.isCompleted && this.synchronous;
				}
			}

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x060017C4 RID: 6084 RVA: 0x00027BF1 File Offset: 0x00026BF1
			public bool IsCompleted
			{
				get
				{
					return this.isCompleted;
				}
			}

			// Token: 0x060017C5 RID: 6085 RVA: 0x00027BFC File Offset: 0x00026BFC
			internal void Complete()
			{
				lock (this.invokeSyncObject)
				{
					this.isCompleted = true;
					if (this.resetEvent != null)
					{
						this.resetEvent.Set();
					}
				}
			}

			// Token: 0x040011B9 RID: 4537
			internal Control caller;

			// Token: 0x040011BA RID: 4538
			internal Control marshaler;

			// Token: 0x040011BB RID: 4539
			internal Delegate method;

			// Token: 0x040011BC RID: 4540
			internal object[] args;

			// Token: 0x040011BD RID: 4541
			internal object retVal;

			// Token: 0x040011BE RID: 4542
			internal Exception exception;

			// Token: 0x040011BF RID: 4543
			internal bool synchronous;

			// Token: 0x040011C0 RID: 4544
			private bool isCompleted;

			// Token: 0x040011C1 RID: 4545
			private ManualResetEvent resetEvent;

			// Token: 0x040011C2 RID: 4546
			private object invokeSyncObject = new object();

			// Token: 0x040011C3 RID: 4547
			internal ExecutionContext executionContext;

			// Token: 0x040011C4 RID: 4548
			internal SynchronizationContext syncContext;
		}

		// Token: 0x02000207 RID: 519
		private class ControlVersionInfo
		{
			// Token: 0x060017C6 RID: 6086 RVA: 0x00027C4C File Offset: 0x00026C4C
			internal ControlVersionInfo(Control owner)
			{
				this.owner = owner;
			}

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x060017C7 RID: 6087 RVA: 0x00027C5C File Offset: 0x00026C5C
			internal string CompanyName
			{
				get
				{
					if (this.companyName == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
						if (customAttributes != null && customAttributes.Length > 0)
						{
							this.companyName = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
						}
						if (this.companyName == null || this.companyName.Length == 0)
						{
							this.companyName = this.GetFileVersionInfo().CompanyName;
							if (this.companyName != null)
							{
								this.companyName = this.companyName.Trim();
							}
						}
						if (this.companyName == null || this.companyName.Length == 0)
						{
							string text = this.owner.GetType().Namespace;
							if (text == null)
							{
								text = string.Empty;
							}
							int num = text.IndexOf("/");
							if (num != -1)
							{
								this.companyName = text.Substring(0, num);
							}
							else
							{
								this.companyName = text;
							}
						}
					}
					return this.companyName;
				}
			}

			// Token: 0x170002CB RID: 715
			// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00027D54 File Offset: 0x00026D54
			internal string ProductName
			{
				get
				{
					if (this.productName == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
						if (customAttributes != null && customAttributes.Length > 0)
						{
							this.productName = ((AssemblyProductAttribute)customAttributes[0]).Product;
						}
						if (this.productName == null || this.productName.Length == 0)
						{
							this.productName = this.GetFileVersionInfo().ProductName;
							if (this.productName != null)
							{
								this.productName = this.productName.Trim();
							}
						}
						if (this.productName == null || this.productName.Length == 0)
						{
							string text = this.owner.GetType().Namespace;
							if (text == null)
							{
								text = string.Empty;
							}
							int num = text.IndexOf(".");
							if (num != -1)
							{
								this.productName = text.Substring(num + 1);
							}
							else
							{
								this.productName = text;
							}
						}
					}
					return this.productName;
				}
			}

			// Token: 0x170002CC RID: 716
			// (get) Token: 0x060017C9 RID: 6089 RVA: 0x00027E4C File Offset: 0x00026E4C
			internal string ProductVersion
			{
				get
				{
					if (this.productVersion == null)
					{
						object[] customAttributes = this.owner.GetType().Module.Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
						if (customAttributes != null && customAttributes.Length > 0)
						{
							this.productVersion = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
						}
						if (this.productVersion == null || this.productVersion.Length == 0)
						{
							this.productVersion = this.GetFileVersionInfo().ProductVersion;
							if (this.productVersion != null)
							{
								this.productVersion = this.productVersion.Trim();
							}
						}
						if (this.productVersion.Length == 0)
						{
							this.productVersion = "1.0.0.0";
						}
					}
					return this.productVersion;
				}
			}

			// Token: 0x060017CA RID: 6090 RVA: 0x00027F04 File Offset: 0x00026F04
			private FileVersionInfo GetFileVersionInfo()
			{
				if (this.versionInfo == null)
				{
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Assert();
					string fullyQualifiedName;
					try
					{
						fullyQualifiedName = this.owner.GetType().Module.FullyQualifiedName;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					new FileIOPermission(FileIOPermissionAccess.Read, fullyQualifiedName).Assert();
					try
					{
						this.versionInfo = FileVersionInfo.GetVersionInfo(fullyQualifiedName);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return this.versionInfo;
			}

			// Token: 0x040011C5 RID: 4549
			private string companyName;

			// Token: 0x040011C6 RID: 4550
			private string productName;

			// Token: 0x040011C7 RID: 4551
			private string productVersion;

			// Token: 0x040011C8 RID: 4552
			private FileVersionInfo versionInfo;

			// Token: 0x040011C9 RID: 4553
			private Control owner;
		}

		// Token: 0x02000208 RID: 520
		private sealed class MultithreadSafeCallScope : IDisposable
		{
			// Token: 0x060017CB RID: 6091 RVA: 0x00027F90 File Offset: 0x00026F90
			internal MultithreadSafeCallScope()
			{
				if (Control.checkForIllegalCrossThreadCalls && !Control.inCrossThreadSafeCall)
				{
					Control.inCrossThreadSafeCall = true;
					this.resultedInSet = true;
					return;
				}
				this.resultedInSet = false;
			}

			// Token: 0x060017CC RID: 6092 RVA: 0x00027FBB File Offset: 0x00026FBB
			void IDisposable.Dispose()
			{
				if (this.resultedInSet)
				{
					Control.inCrossThreadSafeCall = false;
				}
			}

			// Token: 0x040011CA RID: 4554
			private bool resultedInSet;
		}

		// Token: 0x0200020A RID: 522
		private sealed class PrintPaintEventArgs : PaintEventArgs
		{
			// Token: 0x060017D6 RID: 6102 RVA: 0x000281B1 File Offset: 0x000271B1
			internal PrintPaintEventArgs(Message m, IntPtr dc, Rectangle clipRect)
				: base(dc, clipRect)
			{
				this.m = m;
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x060017D7 RID: 6103 RVA: 0x000281C2 File Offset: 0x000271C2
			internal Message Message
			{
				get
				{
					return this.m;
				}
			}

			// Token: 0x040011D0 RID: 4560
			private Message m;
		}
	}
}

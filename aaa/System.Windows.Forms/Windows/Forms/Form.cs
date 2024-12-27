using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000407 RID: 1031
	[Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
	[DesignerCategory("Form")]
	[DefaultEvent("Load")]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[ComVisible(true)]
	[InitializationEvent("Load")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class Form : ContainerControl
	{
		// Token: 0x06003CBE RID: 15550 RVA: 0x000DA5B0 File Offset: 0x000D95B0
		public Form()
		{
			bool isRestrictedWindow = this.IsRestrictedWindow;
			this.formStateEx[Form.FormStateExShowIcon] = 1;
			base.SetState(2, false);
			base.SetState(524288, true);
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06003CBF RID: 15551 RVA: 0x000DA63F File Offset: 0x000D963F
		// (set) Token: 0x06003CC0 RID: 15552 RVA: 0x000DA656 File Offset: 0x000D9656
		[SRDescription("FormAcceptButtonDescr")]
		[DefaultValue(null)]
		public IButtonControl AcceptButton
		{
			get
			{
				return (IButtonControl)base.Properties.GetObject(Form.PropAcceptButton);
			}
			set
			{
				if (this.AcceptButton != value)
				{
					base.Properties.SetObject(Form.PropAcceptButton, value);
					this.UpdateDefaultButton();
				}
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06003CC1 RID: 15553 RVA: 0x000DA678 File Offset: 0x000D9678
		// (set) Token: 0x06003CC2 RID: 15554 RVA: 0x000DA6B8 File Offset: 0x000D96B8
		internal bool Active
		{
			get
			{
				Form parentFormInternal = base.ParentFormInternal;
				if (parentFormInternal == null)
				{
					return this.formState[Form.FormStateIsActive] != 0;
				}
				return parentFormInternal.ActiveControl == this && parentFormInternal.Active;
			}
			set
			{
				if (this.formState[Form.FormStateIsActive] != 0 != value)
				{
					if (value && !this.CanRecreateHandle())
					{
						return;
					}
					this.formState[Form.FormStateIsActive] = (value ? 1 : 0);
					if (value)
					{
						this.formState[Form.FormStateIsWindowActivated] = 1;
						if (this.IsRestrictedWindow)
						{
							this.WindowText = this.userWindowText;
						}
						if (!base.ValidationCancelled)
						{
							if (base.ActiveControl == null)
							{
								base.SelectNextControlInternal(null, true, true, true, false);
							}
							base.InnerMostActiveContainerControl.FocusActiveControlInternal();
						}
						this.OnActivated(EventArgs.Empty);
						return;
					}
					this.formState[Form.FormStateIsWindowActivated] = 0;
					if (this.IsRestrictedWindow)
					{
						this.Text = this.userWindowText;
					}
					this.OnDeactivate(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003CC3 RID: 15555 RVA: 0x000DA790 File Offset: 0x000D9790
		public static Form ActiveForm
		{
			get
			{
				IntSecurity.GetParent.Demand();
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				Control control = Control.FromHandleInternal(foregroundWindow);
				if (control != null && control is Form)
				{
					return (Form)control;
				}
				return null;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003CC4 RID: 15556 RVA: 0x000DA7C8 File Offset: 0x000D97C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("FormActiveMDIChildDescr")]
		public Form ActiveMdiChild
		{
			get
			{
				Form form = this.ActiveMdiChildInternal;
				if (form == null && this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					IntPtr intPtr = this.ctlClient.SendMessage(553, 0, 0);
					form = Control.FromHandleInternal(intPtr) as Form;
				}
				if (form != null && form.Visible && form.Enabled)
				{
					return form;
				}
				return null;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x000DA829 File Offset: 0x000D9829
		// (set) Token: 0x06003CC6 RID: 15558 RVA: 0x000DA840 File Offset: 0x000D9840
		internal Form ActiveMdiChildInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropActiveMdiChild);
			}
			set
			{
				base.Properties.SetObject(Form.PropActiveMdiChild, value);
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x000DA853 File Offset: 0x000D9853
		// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x000DA86A File Offset: 0x000D986A
		private Form FormerlyActiveMdiChild
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropFormerlyActiveMdiChild);
			}
			set
			{
				base.Properties.SetObject(Form.PropFormerlyActiveMdiChild, value);
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x000DA87D File Offset: 0x000D987D
		// (set) Token: 0x06003CCA RID: 15562 RVA: 0x000DA898 File Offset: 0x000D9898
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlAllowTransparencyDescr")]
		public bool AllowTransparency
		{
			get
			{
				return this.formState[Form.FormStateAllowTransparency] != 0;
			}
			set
			{
				if (value != (this.formState[Form.FormStateAllowTransparency] != 0) && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
				{
					this.formState[Form.FormStateAllowTransparency] = (value ? 1 : 0);
					this.formState[Form.FormStateLayered] = this.formState[Form.FormStateAllowTransparency];
					base.UpdateStyles();
					if (!value)
					{
						if (base.Properties.ContainsObject(Form.PropOpacity))
						{
							base.Properties.SetObject(Form.PropOpacity, 1f);
						}
						if (base.Properties.ContainsObject(Form.PropTransparencyKey))
						{
							base.Properties.SetObject(Form.PropTransparencyKey, Color.Empty);
						}
						this.UpdateLayered();
					}
				}
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x000DA973 File Offset: 0x000D9973
		// (set) Token: 0x06003CCC RID: 15564 RVA: 0x000DA98C File Offset: 0x000D998C
		[SRCategory("CatLayout")]
		[Obsolete("This property has been deprecated. Use the AutoScaleMode property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[SRDescription("FormAutoScaleDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoScale
		{
			get
			{
				return this.formState[Form.FormStateAutoScaling] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExSettingAutoScale] = 1;
				try
				{
					if (value)
					{
						this.formState[Form.FormStateAutoScaling] = 1;
						base.AutoScaleMode = AutoScaleMode.None;
					}
					else
					{
						this.formState[Form.FormStateAutoScaling] = 0;
					}
				}
				finally
				{
					this.formStateEx[Form.FormStateExSettingAutoScale] = 0;
				}
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x000DA9FC File Offset: 0x000D99FC
		// (set) Token: 0x06003CCE RID: 15566 RVA: 0x000DAA4A File Offset: 0x000D9A4A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public virtual Size AutoScaleBaseSize
		{
			get
			{
				if (this.autoScaleBaseSize.IsEmpty)
				{
					SizeF autoScaleSize = Form.GetAutoScaleSize(this.Font);
					return new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
				}
				return this.autoScaleBaseSize;
			}
			set
			{
				this.autoScaleBaseSize = value;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x000DAA53 File Offset: 0x000D9A53
		// (set) Token: 0x06003CD0 RID: 15568 RVA: 0x000DAA5B File Offset: 0x000D9A5B
		[Localizable(true)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				if (value)
				{
					this.IsMdiContainer = false;
				}
				base.AutoScroll = value;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x000DAA6E File Offset: 0x000D9A6E
		// (set) Token: 0x06003CD2 RID: 15570 RVA: 0x000DAA88 File Offset: 0x000D9A88
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return this.formStateEx[Form.FormStateExAutoSize] != 0;
			}
			set
			{
				if (value != this.AutoSize)
				{
					this.formStateEx[Form.FormStateExAutoSize] = (value ? 1 : 0);
					if (!this.AutoSize)
					{
						this.minAutoSize = Size.Empty;
						this.Size = CommonProperties.GetSpecifiedBounds(this).Size;
					}
					LayoutTransaction.DoLayout(this, this, PropertyNames.AutoSize);
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001F8 RID: 504
		// (add) Token: 0x06003CD3 RID: 15571 RVA: 0x000DAAF3 File Offset: 0x000D9AF3
		// (remove) Token: 0x06003CD4 RID: 15572 RVA: 0x000DAAFC File Offset: 0x000D9AFC
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x000DAB05 File Offset: 0x000D9B05
		// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x000DAB10 File Offset: 0x000D9B10
		[SRDescription("ControlAutoSizeModeDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		public AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					Control control = ((base.DesignMode || this.ParentInternal == null) ? this : this.ParentInternal);
					if (control != null)
					{
						if (control.LayoutEngine == DefaultLayout.Instance)
						{
							control.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(control, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003CD7 RID: 15575 RVA: 0x000DAB97 File Offset: 0x000D9B97
		// (set) Token: 0x06003CD8 RID: 15576 RVA: 0x000DAB9F File Offset: 0x000D9B9F
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		// Token: 0x140001F9 RID: 505
		// (add) Token: 0x06003CD9 RID: 15577 RVA: 0x000DABA8 File Offset: 0x000D9BA8
		// (remove) Token: 0x06003CDA RID: 15578 RVA: 0x000DABB1 File Offset: 0x000D9BB1
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x000DABBC File Offset: 0x000D9BBC
		// (set) Token: 0x06003CDC RID: 15580 RVA: 0x000DABE0 File Offset: 0x000D9BE0
		public override Color BackColor
		{
			get
			{
				Color rawBackColor = base.RawBackColor;
				if (!rawBackColor.IsEmpty)
				{
					return rawBackColor;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x000DABE9 File Offset: 0x000D9BE9
		// (set) Token: 0x06003CDE RID: 15582 RVA: 0x000DAC01 File Offset: 0x000D9C01
		private bool CalledClosing
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledClosing] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledClosing] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x000DAC1A File Offset: 0x000D9C1A
		// (set) Token: 0x06003CE0 RID: 15584 RVA: 0x000DAC32 File Offset: 0x000D9C32
		private bool CalledCreateControl
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledCreateControl] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledCreateControl] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x000DAC4B File Offset: 0x000D9C4B
		// (set) Token: 0x06003CE2 RID: 15586 RVA: 0x000DAC63 File Offset: 0x000D9C63
		private bool CalledMakeVisible
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledMakeVisible] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledMakeVisible] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x000DAC7C File Offset: 0x000D9C7C
		// (set) Token: 0x06003CE4 RID: 15588 RVA: 0x000DAC94 File Offset: 0x000D9C94
		private bool CalledOnLoad
		{
			get
			{
				return this.formStateEx[Form.FormStateExCalledOnLoad] != 0;
			}
			set
			{
				this.formStateEx[Form.FormStateExCalledOnLoad] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x000DACAD File Offset: 0x000D9CAD
		// (set) Token: 0x06003CE6 RID: 15590 RVA: 0x000DACC0 File Offset: 0x000D9CC0
		[DefaultValue(FormBorderStyle.Sizable)]
		[DispId(-504)]
		[SRDescription("FormBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		public FormBorderStyle FormBorderStyle
		{
			get
			{
				return (FormBorderStyle)this.formState[Form.FormStateBorderStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormBorderStyle));
				}
				if (this.IsRestrictedWindow)
				{
					switch (value)
					{
					case FormBorderStyle.None:
						value = FormBorderStyle.FixedSingle;
						break;
					case FormBorderStyle.FixedSingle:
					case FormBorderStyle.Fixed3D:
					case FormBorderStyle.FixedDialog:
					case FormBorderStyle.Sizable:
						break;
					case FormBorderStyle.FixedToolWindow:
						value = FormBorderStyle.FixedSingle;
						break;
					case FormBorderStyle.SizableToolWindow:
						value = FormBorderStyle.Sizable;
						break;
					default:
						value = FormBorderStyle.Sizable;
						break;
					}
				}
				this.formState[Form.FormStateBorderStyle] = (int)value;
				if (this.formState[Form.FormStateSetClientSize] == 1 && !base.IsHandleCreated)
				{
					this.ClientSize = this.ClientSize;
				}
				Rectangle rectangle = this.restoredWindowBounds;
				BoundsSpecified boundsSpecified = this.restoredWindowBoundsSpecified;
				int num = this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize];
				int num2 = this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize];
				this.UpdateFormStyles();
				if (this.formState[Form.FormStateIconSet] == 0 && !this.IsRestrictedWindow)
				{
					this.UpdateWindowIcon(false);
				}
				if (this.WindowState != FormWindowState.Normal)
				{
					this.restoredWindowBounds = rectangle;
					this.restoredWindowBoundsSpecified = boundsSpecified;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = num;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = num2;
				}
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x000DADFC File Offset: 0x000D9DFC
		// (set) Token: 0x06003CE8 RID: 15592 RVA: 0x000DAE13 File Offset: 0x000D9E13
		[DefaultValue(null)]
		[SRDescription("FormCancelButtonDescr")]
		public IButtonControl CancelButton
		{
			get
			{
				return (IButtonControl)base.Properties.GetObject(Form.PropCancelButton);
			}
			set
			{
				base.Properties.SetObject(Form.PropCancelButton, value);
				if (value != null && value.DialogResult == DialogResult.None)
				{
					value.DialogResult = DialogResult.Cancel;
				}
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06003CE9 RID: 15593 RVA: 0x000DAE38 File Offset: 0x000D9E38
		// (set) Token: 0x06003CEA RID: 15594 RVA: 0x000DAE40 File Offset: 0x000D9E40
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Localizable(true)]
		public new Size ClientSize
		{
			get
			{
				return base.ClientSize;
			}
			set
			{
				base.ClientSize = value;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003CEB RID: 15595 RVA: 0x000DAE49 File Offset: 0x000D9E49
		// (set) Token: 0x06003CEC RID: 15596 RVA: 0x000DAE61 File Offset: 0x000D9E61
		[SRDescription("FormControlBoxDescr")]
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		public bool ControlBox
		{
			get
			{
				return this.formState[Form.FormStateControlBox] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (value)
				{
					this.formState[Form.FormStateControlBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateControlBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003CED RID: 15597 RVA: 0x000DAE9C File Offset: 0x000D9E9C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (base.IsHandleCreated && (base.WindowStyle & 134217728) != 0)
				{
					createParams.Style |= 134217728;
				}
				else if (this.TopLevel)
				{
					createParams.Style &= -134217729;
				}
				if (this.TopLevel && this.formState[Form.FormStateLayered] != 0)
				{
					createParams.ExStyle |= 524288;
				}
				IWin32Window win32Window = (IWin32Window)base.Properties.GetObject(Form.PropDialogOwner);
				if (win32Window != null)
				{
					createParams.Parent = Control.GetSafeHandle(win32Window);
				}
				this.FillInCreateParamsBorderStyles(createParams);
				this.FillInCreateParamsWindowState(createParams);
				this.FillInCreateParamsBorderIcons(createParams);
				if (this.formState[Form.FormStateTaskBar] != 0)
				{
					createParams.ExStyle |= 262144;
				}
				FormBorderStyle formBorderStyle = this.FormBorderStyle;
				if (!this.ShowIcon && (formBorderStyle == FormBorderStyle.Sizable || formBorderStyle == FormBorderStyle.Fixed3D || formBorderStyle == FormBorderStyle.FixedSingle))
				{
					createParams.ExStyle |= 1;
				}
				if (this.IsMdiChild)
				{
					if (base.Visible && (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Normal))
					{
						Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
						Form activeMdiChildInternal = form.ActiveMdiChildInternal;
						if (activeMdiChildInternal != null && activeMdiChildInternal.WindowState == FormWindowState.Maximized)
						{
							createParams.Style |= 16777216;
							this.formState[Form.FormStateWindowState] = 2;
							base.SetState(65536, true);
						}
					}
					if (this.formState[Form.FormStateMdiChildMax] != 0)
					{
						createParams.Style |= 16777216;
					}
					createParams.ExStyle |= 64;
				}
				if (this.TopLevel || this.IsMdiChild)
				{
					this.FillInCreateParamsStartPosition(createParams);
					if ((createParams.Style & 268435456) != 0)
					{
						this.formState[Form.FormStateShowWindowOnCreate] = 1;
						createParams.Style &= -268435457;
					}
					else
					{
						this.formState[Form.FormStateShowWindowOnCreate] = 0;
					}
				}
				if (this.IsRestrictedWindow)
				{
					createParams.Caption = this.RestrictedWindowText(createParams.Caption);
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003CEE RID: 15598 RVA: 0x000DB104 File Offset: 0x000DA104
		// (set) Token: 0x06003CEF RID: 15599 RVA: 0x000DB10C File Offset: 0x000DA10C
		internal CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
			set
			{
				this.closeReason = value;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06003CF0 RID: 15600 RVA: 0x000DB118 File Offset: 0x000DA118
		internal static Icon DefaultIcon
		{
			get
			{
				if (Form.defaultIcon == null)
				{
					lock (Form.internalSyncObject)
					{
						if (Form.defaultIcon == null)
						{
							Form.defaultIcon = new Icon(typeof(Form), "wfc.ico");
						}
					}
				}
				return Form.defaultIcon;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003CF1 RID: 15601 RVA: 0x000DB178 File Offset: 0x000DA178
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.NoControl;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06003CF2 RID: 15602 RVA: 0x000DB17C File Offset: 0x000DA17C
		private static Icon DefaultRestrictedIcon
		{
			get
			{
				if (Form.defaultRestrictedIcon == null)
				{
					lock (Form.internalSyncObject)
					{
						if (Form.defaultRestrictedIcon == null)
						{
							Form.defaultRestrictedIcon = new Icon(typeof(Form), "wfsecurity.ico");
						}
					}
				}
				return Form.defaultRestrictedIcon;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06003CF3 RID: 15603 RVA: 0x000DB1DC File Offset: 0x000DA1DC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(300, 300);
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06003CF4 RID: 15604 RVA: 0x000DB1F0 File Offset: 0x000DA1F0
		// (set) Token: 0x06003CF5 RID: 15605 RVA: 0x000DB235 File Offset: 0x000DA235
		[SRDescription("FormDesktopBoundsDescr")]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle DesktopBounds
		{
			get
			{
				Rectangle workingArea = SystemInformation.WorkingArea;
				Rectangle bounds = base.Bounds;
				bounds.X -= workingArea.X;
				bounds.Y -= workingArea.Y;
				return bounds;
			}
			set
			{
				this.SetDesktopBounds(value.X, value.Y, value.Width, value.Height);
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06003CF6 RID: 15606 RVA: 0x000DB25C File Offset: 0x000DA25C
		// (set) Token: 0x06003CF7 RID: 15607 RVA: 0x000DB2A1 File Offset: 0x000DA2A1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormDesktopLocationDescr")]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		public Point DesktopLocation
		{
			get
			{
				Rectangle workingArea = SystemInformation.WorkingArea;
				Point location = this.Location;
				location.X -= workingArea.X;
				location.Y -= workingArea.Y;
				return location;
			}
			set
			{
				this.SetDesktopLocation(value.X, value.Y);
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x000DB2B7 File Offset: 0x000DA2B7
		// (set) Token: 0x06003CF9 RID: 15609 RVA: 0x000DB2BF File Offset: 0x000DA2BF
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormDialogResultDescr")]
		[Browsable(false)]
		public DialogResult DialogResult
		{
			get
			{
				return this.dialogResult;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DialogResult));
				}
				this.dialogResult = value;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06003CFA RID: 15610 RVA: 0x000DB2F0 File Offset: 0x000DA2F0
		internal override bool HasMenu
		{
			get
			{
				bool flag = false;
				Menu menu = this.Menu;
				if (this.TopLevel && menu != null && menu.ItemCount > 0)
				{
					flag = true;
				}
				return flag;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06003CFB RID: 15611 RVA: 0x000DB31D File Offset: 0x000DA31D
		// (set) Token: 0x06003CFC RID: 15612 RVA: 0x000DB335 File Offset: 0x000DA335
		[DefaultValue(false)]
		[SRDescription("FormHelpButtonDescr")]
		[SRCategory("CatWindowStyle")]
		public bool HelpButton
		{
			get
			{
				return this.formState[Form.FormStateHelpButton] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateHelpButton] = 1;
				}
				else
				{
					this.formState[Form.FormStateHelpButton] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		// Token: 0x140001FA RID: 506
		// (add) Token: 0x06003CFD RID: 15613 RVA: 0x000DB364 File Offset: 0x000DA364
		// (remove) Token: 0x06003CFE RID: 15614 RVA: 0x000DB377 File Offset: 0x000DA377
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("FormHelpButtonClickedDescr")]
		[Browsable(true)]
		[SRCategory("CatBehavior")]
		public event CancelEventHandler HelpButtonClicked
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_HELPBUTTONCLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_HELPBUTTONCLICKED, value);
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06003CFF RID: 15615 RVA: 0x000DB38A File Offset: 0x000DA38A
		// (set) Token: 0x06003D00 RID: 15616 RVA: 0x000DB3B8 File Offset: 0x000DA3B8
		[Localizable(true)]
		[AmbientValue(null)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormIconDescr")]
		public Icon Icon
		{
			get
			{
				if (this.formState[Form.FormStateIconSet] != 0)
				{
					return this.icon;
				}
				if (this.IsRestrictedWindow)
				{
					return Form.DefaultRestrictedIcon;
				}
				return Form.DefaultIcon;
			}
			set
			{
				if (this.icon != value && !this.IsRestrictedWindow)
				{
					if (value == Form.defaultIcon)
					{
						value = null;
					}
					this.formState[Form.FormStateIconSet] = ((value == null) ? 0 : 1);
					this.icon = value;
					if (this.smallIcon != null)
					{
						this.smallIcon.Dispose();
						this.smallIcon = null;
					}
					this.UpdateWindowIcon(true);
				}
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x000DB420 File Offset: 0x000DA420
		// (set) Token: 0x06003D02 RID: 15618 RVA: 0x000DB435 File Offset: 0x000DA435
		private bool IsClosing
		{
			get
			{
				return this.formStateEx[Form.FormStateExWindowClosing] == 1;
			}
			set
			{
				this.formStateEx[Form.FormStateExWindowClosing] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x000DB44E File Offset: 0x000DA44E
		private bool IsMaximized
		{
			get
			{
				return this.WindowState == FormWindowState.Maximized || (this.IsMdiChild && this.formState[Form.FormStateMdiChildMax] == 1);
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06003D04 RID: 15620 RVA: 0x000DB478 File Offset: 0x000DA478
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormIsMDIChildDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsMdiChild
		{
			get
			{
				return base.Properties.GetObject(Form.PropFormMdiParent) != null;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003D05 RID: 15621 RVA: 0x000DB490 File Offset: 0x000DA490
		// (set) Token: 0x06003D06 RID: 15622 RVA: 0x000DB4BB File Offset: 0x000DA4BB
		internal bool IsMdiChildFocusable
		{
			get
			{
				return base.Properties.ContainsObject(Form.PropMdiChildFocusable) && (bool)base.Properties.GetObject(Form.PropMdiChildFocusable);
			}
			set
			{
				if (value != this.IsMdiChildFocusable)
				{
					base.Properties.SetObject(Form.PropMdiChildFocusable, value);
				}
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06003D07 RID: 15623 RVA: 0x000DB4DC File Offset: 0x000DA4DC
		// (set) Token: 0x06003D08 RID: 15624 RVA: 0x000DB4EA File Offset: 0x000DA4EA
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormIsMDIContainerDescr")]
		[DefaultValue(false)]
		public bool IsMdiContainer
		{
			get
			{
				return this.ctlClient != null;
			}
			set
			{
				if (value == this.IsMdiContainer)
				{
					return;
				}
				if (value)
				{
					this.AllowTransparency = false;
					base.Controls.Add(new MdiClient());
				}
				else
				{
					this.ActiveMdiChildInternal = null;
					this.ctlClient.Dispose();
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x000DB52C File Offset: 0x000DA52C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsRestrictedWindow
		{
			get
			{
				if (this.formState[Form.FormStateIsRestrictedWindowChecked] == 0)
				{
					this.formState[Form.FormStateIsRestrictedWindow] = 0;
					try
					{
						IntSecurity.WindowAdornmentModification.Demand();
					}
					catch (SecurityException)
					{
						this.formState[Form.FormStateIsRestrictedWindow] = 1;
					}
					catch
					{
						this.formState[Form.FormStateIsRestrictedWindow] = 1;
						this.formState[Form.FormStateIsRestrictedWindowChecked] = 1;
						throw;
					}
					this.formState[Form.FormStateIsRestrictedWindowChecked] = 1;
				}
				return this.formState[Form.FormStateIsRestrictedWindow] != 0;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x000DB5E4 File Offset: 0x000DA5E4
		// (set) Token: 0x06003D0B RID: 15627 RVA: 0x000DB5FC File Offset: 0x000DA5FC
		[DefaultValue(false)]
		[SRDescription("FormKeyPreviewDescr")]
		public bool KeyPreview
		{
			get
			{
				return this.formState[Form.FormStateKeyPreview] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateKeyPreview] = 1;
					return;
				}
				this.formState[Form.FormStateKeyPreview] = 0;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x000DB624 File Offset: 0x000DA624
		// (set) Token: 0x06003D0D RID: 15629 RVA: 0x000DB62C File Offset: 0x000DA62C
		[SettingsBindable(true)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x000DB635 File Offset: 0x000DA635
		// (set) Token: 0x06003D0F RID: 15631 RVA: 0x000DB647 File Offset: 0x000DA647
		protected Rectangle MaximizedBounds
		{
			get
			{
				return base.Properties.GetRectangle(Form.PropMaximizedBounds);
			}
			set
			{
				if (!value.Equals(this.MaximizedBounds))
				{
					base.Properties.SetRectangle(Form.PropMaximizedBounds, value);
					this.OnMaximizedBoundsChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001FB RID: 507
		// (add) Token: 0x06003D10 RID: 15632 RVA: 0x000DB67F File Offset: 0x000DA67F
		// (remove) Token: 0x06003D11 RID: 15633 RVA: 0x000DB692 File Offset: 0x000DA692
		[SRDescription("FormOnMaximizedBoundsChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler MaximizedBoundsChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MAXIMIZEDBOUNDSCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MAXIMIZEDBOUNDSCHANGED, value);
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x000DB6A5 File Offset: 0x000DA6A5
		// (set) Token: 0x06003D13 RID: 15635 RVA: 0x000DB6E4 File Offset: 0x000DA6E4
		[DefaultValue(typeof(Size), "0, 0")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatLayout")]
		[SRDescription("FormMaximumSizeDescr")]
		public override Size MaximumSize
		{
			get
			{
				if (base.Properties.ContainsInteger(Form.PropMaxTrackSizeWidth))
				{
					return new Size(base.Properties.GetInteger(Form.PropMaxTrackSizeWidth), base.Properties.GetInteger(Form.PropMaxTrackSizeHeight));
				}
				return Size.Empty;
			}
			set
			{
				if (!value.Equals(this.MaximumSize))
				{
					if (value.Width < 0 || value.Height < 0)
					{
						throw new ArgumentOutOfRangeException("MaximumSize");
					}
					base.Properties.SetInteger(Form.PropMaxTrackSizeWidth, value.Width);
					base.Properties.SetInteger(Form.PropMaxTrackSizeHeight, value.Height);
					if (!this.MinimumSize.IsEmpty && !value.IsEmpty)
					{
						if (base.Properties.GetInteger(Form.PropMinTrackSizeWidth) > value.Width)
						{
							base.Properties.SetInteger(Form.PropMinTrackSizeWidth, value.Width);
						}
						if (base.Properties.GetInteger(Form.PropMinTrackSizeHeight) > value.Height)
						{
							base.Properties.SetInteger(Form.PropMinTrackSizeHeight, value.Height);
						}
					}
					Size size = this.Size;
					if (!value.IsEmpty && (size.Width > value.Width || size.Height > value.Height))
					{
						this.Size = new Size(Math.Min(size.Width, value.Width), Math.Min(size.Height, value.Height));
					}
					this.OnMaximumSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001FC RID: 508
		// (add) Token: 0x06003D14 RID: 15636 RVA: 0x000DB840 File Offset: 0x000DA840
		// (remove) Token: 0x06003D15 RID: 15637 RVA: 0x000DB853 File Offset: 0x000DA853
		[SRCategory("CatPropertyChanged")]
		[SRDescription("FormOnMaximumSizeChangedDescr")]
		public event EventHandler MaximumSizeChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MAXIMUMSIZECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MAXIMUMSIZECHANGED, value);
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06003D16 RID: 15638 RVA: 0x000DB866 File Offset: 0x000DA866
		// (set) Token: 0x06003D17 RID: 15639 RVA: 0x000DB87D File Offset: 0x000DA87D
		[SRDescription("FormMenuStripDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatWindowStyle")]
		[DefaultValue(null)]
		public MenuStrip MainMenuStrip
		{
			get
			{
				return (MenuStrip)base.Properties.GetObject(Form.PropMainMenuStrip);
			}
			set
			{
				base.Properties.SetObject(Form.PropMainMenuStrip, value);
				if (base.IsHandleCreated && this.Menu == null)
				{
					this.UpdateMenuHandles();
				}
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x000DB8A6 File Offset: 0x000DA8A6
		// (set) Token: 0x06003D19 RID: 15641 RVA: 0x000DB8AE File Offset: 0x000DA8AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Padding Margin
		{
			get
			{
				return base.Margin;
			}
			set
			{
				base.Margin = value;
			}
		}

		// Token: 0x140001FD RID: 509
		// (add) Token: 0x06003D1A RID: 15642 RVA: 0x000DB8B7 File Offset: 0x000DA8B7
		// (remove) Token: 0x06003D1B RID: 15643 RVA: 0x000DB8C0 File Offset: 0x000DA8C0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MarginChanged
		{
			add
			{
				base.MarginChanged += value;
			}
			remove
			{
				base.MarginChanged -= value;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003D1C RID: 15644 RVA: 0x000DB8C9 File Offset: 0x000DA8C9
		// (set) Token: 0x06003D1D RID: 15645 RVA: 0x000DB8E0 File Offset: 0x000DA8E0
		[SRDescription("FormMenuDescr")]
		[Browsable(false)]
		[DefaultValue(null)]
		[SRCategory("CatWindowStyle")]
		[TypeConverter(typeof(ReferenceConverter))]
		public MainMenu Menu
		{
			get
			{
				return (MainMenu)base.Properties.GetObject(Form.PropMainMenu);
			}
			set
			{
				MainMenu menu = this.Menu;
				if (menu != value)
				{
					if (menu != null)
					{
						menu.form = null;
					}
					base.Properties.SetObject(Form.PropMainMenu, value);
					if (value != null)
					{
						if (value.form != null)
						{
							value.form.Menu = null;
						}
						value.form = this;
					}
					if (this.formState[Form.FormStateSetClientSize] == 1 && !base.IsHandleCreated)
					{
						this.ClientSize = this.ClientSize;
					}
					this.MenuChanged(0, value);
				}
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06003D1E RID: 15646 RVA: 0x000DB960 File Offset: 0x000DA960
		// (set) Token: 0x06003D1F RID: 15647 RVA: 0x000DB9A0 File Offset: 0x000DA9A0
		[SRCategory("CatLayout")]
		[SRDescription("FormMinimumSizeDescr")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public override Size MinimumSize
		{
			get
			{
				if (base.Properties.ContainsInteger(Form.PropMinTrackSizeWidth))
				{
					return new Size(base.Properties.GetInteger(Form.PropMinTrackSizeWidth), base.Properties.GetInteger(Form.PropMinTrackSizeHeight));
				}
				return this.DefaultMinimumSize;
			}
			set
			{
				if (!value.Equals(this.MinimumSize))
				{
					if (value.Width < 0 || value.Height < 0)
					{
						throw new ArgumentOutOfRangeException("MinimumSize");
					}
					Rectangle bounds = base.Bounds;
					bounds.Size = value;
					value = WindowsFormsUtils.ConstrainToScreenWorkingAreaBounds(bounds).Size;
					base.Properties.SetInteger(Form.PropMinTrackSizeWidth, value.Width);
					base.Properties.SetInteger(Form.PropMinTrackSizeHeight, value.Height);
					if (!this.MaximumSize.IsEmpty && !value.IsEmpty)
					{
						if (base.Properties.GetInteger(Form.PropMaxTrackSizeWidth) < value.Width)
						{
							base.Properties.SetInteger(Form.PropMaxTrackSizeWidth, value.Width);
						}
						if (base.Properties.GetInteger(Form.PropMaxTrackSizeHeight) < value.Height)
						{
							base.Properties.SetInteger(Form.PropMaxTrackSizeHeight, value.Height);
						}
					}
					Size size = this.Size;
					if (size.Width < value.Width || size.Height < value.Height)
					{
						this.Size = new Size(Math.Max(size.Width, value.Width), Math.Max(size.Height, value.Height));
					}
					if (base.IsHandleCreated)
					{
						SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef, this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height, 4);
					}
					this.OnMinimumSizeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001FE RID: 510
		// (add) Token: 0x06003D20 RID: 15648 RVA: 0x000DBB6E File Offset: 0x000DAB6E
		// (remove) Token: 0x06003D21 RID: 15649 RVA: 0x000DBB81 File Offset: 0x000DAB81
		[SRDescription("FormOnMinimumSizeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler MinimumSizeChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MINIMUMSIZECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MINIMUMSIZECHANGED, value);
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003D22 RID: 15650 RVA: 0x000DBB94 File Offset: 0x000DAB94
		// (set) Token: 0x06003D23 RID: 15651 RVA: 0x000DBBAC File Offset: 0x000DABAC
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		[SRDescription("FormMaximizeBoxDescr")]
		public bool MaximizeBox
		{
			get
			{
				return this.formState[Form.FormStateMaximizeBox] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateMaximizeBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateMaximizeBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003D24 RID: 15652 RVA: 0x000DBBDB File Offset: 0x000DABDB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMDIChildrenDescr")]
		[SRCategory("CatWindowStyle")]
		public Form[] MdiChildren
		{
			get
			{
				if (this.ctlClient != null)
				{
					return this.ctlClient.MdiChildren;
				}
				return new Form[0];
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x000DBBF7 File Offset: 0x000DABF7
		internal MdiClient MdiClient
		{
			get
			{
				return this.ctlClient;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06003D26 RID: 15654 RVA: 0x000DBBFF File Offset: 0x000DABFF
		// (set) Token: 0x06003D27 RID: 15655 RVA: 0x000DBC11 File Offset: 0x000DAC11
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMDIParentDescr")]
		[Browsable(false)]
		[SRCategory("CatWindowStyle")]
		public Form MdiParent
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.MdiParentInternal;
			}
			set
			{
				this.MdiParentInternal = value;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x000DBC1A File Offset: 0x000DAC1A
		// (set) Token: 0x06003D29 RID: 15657 RVA: 0x000DBC34 File Offset: 0x000DAC34
		private Form MdiParentInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			}
			set
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (value == form && (value != null || this.ParentInternal == null))
				{
					return;
				}
				if (value != null && base.CreateThreadId != value.CreateThreadId)
				{
					throw new ArgumentException(SR.GetString("AddDifferentThreads"), "value");
				}
				bool state = base.GetState(2);
				base.Visible = false;
				try
				{
					if (value == null)
					{
						this.ParentInternal = null;
						base.SetTopLevel(true);
					}
					else
					{
						if (this.IsMdiContainer)
						{
							throw new ArgumentException(SR.GetString("FormMDIParentAndChild"), "value");
						}
						if (!value.IsMdiContainer)
						{
							throw new ArgumentException(SR.GetString("MDIParentNotContainer"), "value");
						}
						this.Dock = DockStyle.None;
						base.Properties.SetObject(Form.PropFormMdiParent, value);
						base.SetState(524288, false);
						this.ParentInternal = value.MdiClient;
						if (this.ParentInternal.IsHandleCreated && this.IsMdiChild && base.IsHandleCreated)
						{
							this.DestroyHandle();
						}
					}
					this.InvalidateMergedMenu();
					this.UpdateMenuHandles();
				}
				finally
				{
					base.UpdateStyles();
					base.Visible = state;
				}
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x000DBD6C File Offset: 0x000DAD6C
		// (set) Token: 0x06003D2B RID: 15659 RVA: 0x000DBD83 File Offset: 0x000DAD83
		private MdiWindowListStrip MdiWindowListStrip
		{
			get
			{
				return base.Properties.GetObject(Form.PropMdiWindowListStrip) as MdiWindowListStrip;
			}
			set
			{
				base.Properties.SetObject(Form.PropMdiWindowListStrip, value);
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003D2C RID: 15660 RVA: 0x000DBD96 File Offset: 0x000DAD96
		// (set) Token: 0x06003D2D RID: 15661 RVA: 0x000DBDAD File Offset: 0x000DADAD
		private MdiControlStrip MdiControlStrip
		{
			get
			{
				return base.Properties.GetObject(Form.PropMdiControlStrip) as MdiControlStrip;
			}
			set
			{
				base.Properties.SetObject(Form.PropMdiControlStrip, value);
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000DBDC0 File Offset: 0x000DADC0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormMergedMenuDescr")]
		[SRCategory("CatWindowStyle")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public MainMenu MergedMenu
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.MergedMenuPrivate;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x000DBDC8 File Offset: 0x000DADC8
		private MainMenu MergedMenuPrivate
		{
			get
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form == null)
				{
					return null;
				}
				MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
				if (mainMenu != null)
				{
					return mainMenu;
				}
				MainMenu menu = form.Menu;
				MainMenu menu2 = this.Menu;
				if (menu2 == null)
				{
					return menu;
				}
				if (menu == null)
				{
					return menu2;
				}
				mainMenu = new MainMenu();
				mainMenu.ownerForm = this;
				mainMenu.MergeMenu(menu);
				mainMenu.MergeMenu(menu2);
				base.Properties.SetObject(Form.PropMergedMenu, mainMenu);
				return mainMenu;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003D30 RID: 15664 RVA: 0x000DBE50 File Offset: 0x000DAE50
		// (set) Token: 0x06003D31 RID: 15665 RVA: 0x000DBE68 File Offset: 0x000DAE68
		[SRCategory("CatWindowStyle")]
		[DefaultValue(true)]
		[SRDescription("FormMinimizeBoxDescr")]
		public bool MinimizeBox
		{
			get
			{
				return this.formState[Form.FormStateMinimizeBox] != 0;
			}
			set
			{
				if (value)
				{
					this.formState[Form.FormStateMinimizeBox] = 1;
				}
				else
				{
					this.formState[Form.FormStateMinimizeBox] = 0;
				}
				this.UpdateFormStyles();
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003D32 RID: 15666 RVA: 0x000DBE97 File Offset: 0x000DAE97
		[SRDescription("FormModalDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatWindowStyle")]
		[Browsable(false)]
		public bool Modal
		{
			get
			{
				return base.GetState(32);
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003D33 RID: 15667 RVA: 0x000DBEA4 File Offset: 0x000DAEA4
		// (set) Token: 0x06003D34 RID: 15668 RVA: 0x000DBEDC File Offset: 0x000DAEDC
		[DefaultValue(1.0)]
		[TypeConverter(typeof(OpacityConverter))]
		[SRDescription("FormOpacityDescr")]
		[SRCategory("CatWindowStyle")]
		public double Opacity
		{
			get
			{
				object @object = base.Properties.GetObject(Form.PropOpacity);
				if (@object != null)
				{
					return Convert.ToDouble(@object, CultureInfo.InvariantCulture);
				}
				return 1.0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					value = Math.Max(value, 0.5);
				}
				if (value > 1.0)
				{
					value = 1.0;
				}
				else if (value < 0.0)
				{
					value = 0.0;
				}
				base.Properties.SetObject(Form.PropOpacity, value);
				bool flag = this.formState[Form.FormStateLayered] != 0;
				if (this.OpacityAsByte < 255 && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
				{
					this.AllowTransparency = true;
					if (this.formState[Form.FormStateLayered] != 1)
					{
						this.formState[Form.FormStateLayered] = 1;
						if (!flag)
						{
							base.UpdateStyles();
						}
					}
				}
				else
				{
					this.formState[Form.FormStateLayered] = ((this.TransparencyKey != Color.Empty) ? 1 : 0);
					if (flag != (this.formState[Form.FormStateLayered] != 0))
					{
						int num = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -20);
						CreateParams createParams = this.CreateParams;
						if (num != createParams.ExStyle)
						{
							UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -20, new HandleRef(null, (IntPtr)createParams.ExStyle));
						}
					}
				}
				this.UpdateLayered();
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003D35 RID: 15669 RVA: 0x000DC050 File Offset: 0x000DB050
		private byte OpacityAsByte
		{
			get
			{
				return (byte)(this.Opacity * 255.0);
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003D36 RID: 15670 RVA: 0x000DC064 File Offset: 0x000DB064
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("FormOwnedFormsDescr")]
		[SRCategory("CatWindowStyle")]
		public Form[] OwnedForms
		{
			get
			{
				Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				Form[] array2 = new Form[integer];
				if (integer > 0)
				{
					Array.Copy(array, 0, array2, 0, integer);
				}
				return array2;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003D37 RID: 15671 RVA: 0x000DC0AE File Offset: 0x000DB0AE
		// (set) Token: 0x06003D38 RID: 15672 RVA: 0x000DC0C0 File Offset: 0x000DB0C0
		[Browsable(false)]
		[SRCategory("CatWindowStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormOwnerDescr")]
		public Form Owner
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.OwnerInternal;
			}
			set
			{
				Form ownerInternal = this.OwnerInternal;
				if (ownerInternal == value)
				{
					return;
				}
				if (value != null && !this.TopLevel)
				{
					throw new ArgumentException(SR.GetString("NonTopLevelCantHaveOwner"), "value");
				}
				Control.CheckParentingCycle(this, value);
				Control.CheckParentingCycle(value, this);
				base.Properties.SetObject(Form.PropOwner, null);
				if (ownerInternal != null)
				{
					ownerInternal.RemoveOwnedForm(this);
				}
				base.Properties.SetObject(Form.PropOwner, value);
				if (value != null)
				{
					value.AddOwnedForm(this);
				}
				this.UpdateHandleWithOwner();
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x000DC143 File Offset: 0x000DB143
		internal Form OwnerInternal
		{
			get
			{
				return (Form)base.Properties.GetObject(Form.PropOwner);
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003D3A RID: 15674 RVA: 0x000DC15C File Offset: 0x000DB15C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Rectangle RestoreBounds
		{
			get
			{
				if (this.restoreBounds.Width == -1 && this.restoreBounds.Height == -1 && this.restoreBounds.X == -1 && this.restoreBounds.Y == -1)
				{
					return base.Bounds;
				}
				return this.restoreBounds;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003D3B RID: 15675 RVA: 0x000DC1AE File Offset: 0x000DB1AE
		// (set) Token: 0x06003D3C RID: 15676 RVA: 0x000DC1B8 File Offset: 0x000DB1B8
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003D3D RID: 15677 RVA: 0x000DC20C File Offset: 0x000DB20C
		// (set) Token: 0x06003D3E RID: 15678 RVA: 0x000DC214 File Offset: 0x000DB214
		internal override Control ParentInternal
		{
			get
			{
				return base.ParentInternal;
			}
			set
			{
				if (value != null)
				{
					this.Owner = null;
				}
				base.ParentInternal = value;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06003D3F RID: 15679 RVA: 0x000DC227 File Offset: 0x000DB227
		// (set) Token: 0x06003D40 RID: 15680 RVA: 0x000DC240 File Offset: 0x000DB240
		[DefaultValue(true)]
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormShowInTaskbarDescr")]
		public bool ShowInTaskbar
		{
			get
			{
				return this.formState[Form.FormStateTaskBar] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (this.ShowInTaskbar != value)
				{
					if (value)
					{
						this.formState[Form.FormStateTaskBar] = 1;
					}
					else
					{
						this.formState[Form.FormStateTaskBar] = 0;
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06003D41 RID: 15681 RVA: 0x000DC294 File Offset: 0x000DB294
		// (set) Token: 0x06003D42 RID: 15682 RVA: 0x000DC2AC File Offset: 0x000DB2AC
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormShowIconDescr")]
		[DefaultValue(true)]
		public bool ShowIcon
		{
			get
			{
				return this.formStateEx[Form.FormStateExShowIcon] != 0;
			}
			set
			{
				if (value)
				{
					this.formStateEx[Form.FormStateExShowIcon] = 1;
				}
				else
				{
					if (this.IsRestrictedWindow)
					{
						return;
					}
					this.formStateEx[Form.FormStateExShowIcon] = 0;
					base.UpdateStyles();
				}
				this.UpdateWindowIcon(true);
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x000DC2EC File Offset: 0x000DB2EC
		internal override int ShowParams
		{
			get
			{
				switch (this.WindowState)
				{
				case FormWindowState.Minimized:
					return 2;
				case FormWindowState.Maximized:
					return 3;
				default:
					if (this.ShowWithoutActivation)
					{
						return 4;
					}
					return 5;
				}
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003D44 RID: 15684 RVA: 0x000DC321 File Offset: 0x000DB321
		[Browsable(false)]
		protected virtual bool ShowWithoutActivation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06003D45 RID: 15685 RVA: 0x000DC324 File Offset: 0x000DB324
		// (set) Token: 0x06003D46 RID: 15686 RVA: 0x000DC32C File Offset: 0x000DB32C
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06003D47 RID: 15687 RVA: 0x000DC335 File Offset: 0x000DB335
		// (set) Token: 0x06003D48 RID: 15688 RVA: 0x000DC348 File Offset: 0x000DB348
		[DefaultValue(SizeGripStyle.Auto)]
		[SRDescription("FormSizeGripStyleDescr")]
		[SRCategory("CatWindowStyle")]
		public SizeGripStyle SizeGripStyle
		{
			get
			{
				return (SizeGripStyle)this.formState[Form.FormStateSizeGripStyle];
			}
			set
			{
				if (this.SizeGripStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(SizeGripStyle));
					}
					this.formState[Form.FormStateSizeGripStyle] = (int)value;
					this.UpdateRenderSizeGrip();
				}
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06003D49 RID: 15689 RVA: 0x000DC39B File Offset: 0x000DB39B
		// (set) Token: 0x06003D4A RID: 15690 RVA: 0x000DC3AD File Offset: 0x000DB3AD
		[SRCategory("CatLayout")]
		[SRDescription("FormStartPositionDescr")]
		[Localizable(true)]
		[DefaultValue(FormStartPosition.WindowsDefaultLocation)]
		public FormStartPosition StartPosition
		{
			get
			{
				return (FormStartPosition)this.formState[Form.FormStateStartPos];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormStartPosition));
				}
				this.formState[Form.FormStateStartPos] = (int)value;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x000DC3E6 File Offset: 0x000DB3E6
		// (set) Token: 0x06003D4C RID: 15692 RVA: 0x000DC3EE File Offset: 0x000DB3EE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		// Token: 0x140001FF RID: 511
		// (add) Token: 0x06003D4D RID: 15693 RVA: 0x000DC3F7 File Offset: 0x000DB3F7
		// (remove) Token: 0x06003D4E RID: 15694 RVA: 0x000DC400 File Offset: 0x000DB400
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x000DC409 File Offset: 0x000DB409
		// (set) Token: 0x06003D50 RID: 15696 RVA: 0x000DC411 File Offset: 0x000DB411
		[DispId(-516)]
		[SRDescription("ControlTabStopDescr")]
		[Browsable(false)]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x14000200 RID: 512
		// (add) Token: 0x06003D51 RID: 15697 RVA: 0x000DC41A File Offset: 0x000DB41A
		// (remove) Token: 0x06003D52 RID: 15698 RVA: 0x000DC423 File Offset: 0x000DB423
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x000DC42C File Offset: 0x000DB42C
		private HandleRef TaskbarOwner
		{
			get
			{
				if (this.ownerWindow == null)
				{
					this.ownerWindow = new NativeWindow();
				}
				if (this.ownerWindow.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.ExStyle = 128;
					this.ownerWindow.CreateHandle(createParams);
				}
				return new HandleRef(this.ownerWindow, this.ownerWindow.Handle);
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x000DC496 File Offset: 0x000DB496
		// (set) Token: 0x06003D55 RID: 15701 RVA: 0x000DC49E File Offset: 0x000DB49E
		[SettingsBindable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x000DC4A7 File Offset: 0x000DB4A7
		// (set) Token: 0x06003D57 RID: 15703 RVA: 0x000DC4AF File Offset: 0x000DB4AF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool TopLevel
		{
			get
			{
				return base.GetTopLevel();
			}
			set
			{
				if (!value && this.IsMdiContainer && !base.DesignMode)
				{
					throw new ArgumentException(SR.GetString("MDIContainerMustBeTopLevel"), "value");
				}
				base.SetTopLevel(value);
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06003D58 RID: 15704 RVA: 0x000DC4E0 File Offset: 0x000DB4E0
		// (set) Token: 0x06003D59 RID: 15705 RVA: 0x000DC4F8 File Offset: 0x000DB4F8
		[SRCategory("CatWindowStyle")]
		[DefaultValue(false)]
		[SRDescription("FormTopMostDescr")]
		public bool TopMost
		{
			get
			{
				return this.formState[Form.FormStateTopMost] != 0;
			}
			set
			{
				if (this.IsRestrictedWindow)
				{
					return;
				}
				if (base.IsHandleCreated && this.TopLevel)
				{
					HandleRef handleRef = (value ? NativeMethods.HWND_TOPMOST : NativeMethods.HWND_NOTOPMOST);
					SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), handleRef, 0, 0, 0, 0, 3);
				}
				if (value)
				{
					this.formState[Form.FormStateTopMost] = 1;
					return;
				}
				this.formState[Form.FormStateTopMost] = 0;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x000DC56C File Offset: 0x000DB56C
		// (set) Token: 0x06003D5B RID: 15707 RVA: 0x000DC59C File Offset: 0x000DB59C
		[SRCategory("CatWindowStyle")]
		[SRDescription("FormTransparencyKeyDescr")]
		public Color TransparencyKey
		{
			get
			{
				object @object = base.Properties.GetObject(Form.PropTransparencyKey);
				if (@object != null)
				{
					return (Color)@object;
				}
				return Color.Empty;
			}
			set
			{
				base.Properties.SetObject(Form.PropTransparencyKey, value);
				if (!this.IsMdiContainer)
				{
					bool flag = this.formState[Form.FormStateLayered] == 1;
					if (value != Color.Empty)
					{
						IntSecurity.TransparentWindows.Demand();
						this.AllowTransparency = true;
						this.formState[Form.FormStateLayered] = 1;
					}
					else
					{
						this.formState[Form.FormStateLayered] = ((this.OpacityAsByte < byte.MaxValue) ? 1 : 0);
					}
					if (flag != (this.formState[Form.FormStateLayered] != 0))
					{
						base.UpdateStyles();
					}
					this.UpdateLayered();
				}
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x000DC658 File Offset: 0x000DB658
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetVisibleCore(bool value)
		{
			if (this.GetVisibleCore() == value && this.dialogResult == DialogResult.OK)
			{
				return;
			}
			if (this.GetVisibleCore() == value && (!value || this.CalledMakeVisible))
			{
				base.SetVisibleCore(value);
				return;
			}
			if (value)
			{
				this.CalledMakeVisible = true;
				if (this.CalledCreateControl)
				{
					if (this.CalledOnLoad)
					{
						if (!Application.OpenFormsInternal.Contains(this))
						{
							Application.OpenFormsInternalAdd(this);
						}
					}
					else
					{
						this.CalledOnLoad = true;
						this.OnLoad(EventArgs.Empty);
						if (this.dialogResult != DialogResult.None)
						{
							value = false;
						}
					}
				}
			}
			else
			{
				this.ResetSecurityTip(true);
			}
			if (!this.IsMdiChild)
			{
				base.SetVisibleCore(value);
				if (this.formState[Form.FormStateSWCalled] == 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 24, value ? 1 : 0, 0);
				}
			}
			else
			{
				if (base.IsHandleCreated)
				{
					this.DestroyHandle();
				}
				if (!value)
				{
					this.InvalidateMergedMenu();
					base.SetState(2, false);
				}
				else
				{
					base.SetState(2, true);
					this.MdiParentInternal.MdiClient.PerformLayout();
					if (this.ParentInternal != null && this.ParentInternal.Visible)
					{
						base.SuspendLayout();
						try
						{
							SafeNativeMethods.ShowWindow(new HandleRef(this, base.Handle), 5);
							base.CreateControl();
							if (this.WindowState == FormWindowState.Maximized)
							{
								this.MdiParentInternal.UpdateWindowIcon(true);
							}
						}
						finally
						{
							base.ResumeLayout();
						}
					}
				}
				this.OnVisibleChanged(EventArgs.Empty);
			}
			if (value && !this.IsMdiChild && (this.WindowState == FormWindowState.Maximized || this.TopMost))
			{
				if (base.ActiveControl == null)
				{
					base.SelectNextControlInternal(null, true, true, true, false);
				}
				base.FocusActiveControlInternal();
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06003D5D RID: 15709 RVA: 0x000DC80C File Offset: 0x000DB80C
		// (set) Token: 0x06003D5E RID: 15710 RVA: 0x000DC820 File Offset: 0x000DB820
		[SRCategory("CatLayout")]
		[DefaultValue(FormWindowState.Normal)]
		[SRDescription("FormWindowStateDescr")]
		public FormWindowState WindowState
		{
			get
			{
				return (FormWindowState)this.formState[Form.FormStateWindowState];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FormWindowState));
				}
				if (this.TopLevel && this.IsRestrictedWindow && value != FormWindowState.Normal)
				{
					return;
				}
				switch (value)
				{
				case FormWindowState.Normal:
					base.SetState(65536, false);
					break;
				case FormWindowState.Minimized:
				case FormWindowState.Maximized:
					base.SetState(65536, true);
					break;
				}
				if (base.IsHandleCreated && base.Visible)
				{
					IntPtr handle = base.Handle;
					switch (value)
					{
					case FormWindowState.Normal:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 1);
						break;
					case FormWindowState.Minimized:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 6);
						break;
					case FormWindowState.Maximized:
						SafeNativeMethods.ShowWindow(new HandleRef(this, handle), 3);
						break;
					}
				}
				this.formState[Form.FormStateWindowState] = (int)value;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x000DC903 File Offset: 0x000DB903
		// (set) Token: 0x06003D60 RID: 15712 RVA: 0x000DC93C File Offset: 0x000DB93C
		internal override string WindowText
		{
			get
			{
				if (!this.IsRestrictedWindow || this.formState[Form.FormStateIsWindowActivated] != 1)
				{
					return base.WindowText;
				}
				if (this.userWindowText == null)
				{
					return "";
				}
				return this.userWindowText;
			}
			set
			{
				string windowText = this.WindowText;
				this.userWindowText = value;
				if (this.IsRestrictedWindow && this.formState[Form.FormStateIsWindowActivated] == 1)
				{
					if (value == null)
					{
						value = "";
					}
					base.WindowText = this.RestrictedWindowText(value);
				}
				else
				{
					base.WindowText = value;
				}
				if (windowText == null || windowText.Length == 0 || value == null || value.Length == 0)
				{
					this.UpdateFormStyles();
				}
			}
		}

		// Token: 0x14000201 RID: 513
		// (add) Token: 0x06003D61 RID: 15713 RVA: 0x000DC9AE File Offset: 0x000DB9AE
		// (remove) Token: 0x06003D62 RID: 15714 RVA: 0x000DC9C1 File Offset: 0x000DB9C1
		[SRCategory("CatFocus")]
		[SRDescription("FormOnActivateDescr")]
		public event EventHandler Activated
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_ACTIVATED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_ACTIVATED, value);
			}
		}

		// Token: 0x14000202 RID: 514
		// (add) Token: 0x06003D63 RID: 15715 RVA: 0x000DC9D4 File Offset: 0x000DB9D4
		// (remove) Token: 0x06003D64 RID: 15716 RVA: 0x000DC9E7 File Offset: 0x000DB9E7
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRDescription("FormOnClosingDescr")]
		[Browsable(false)]
		public event CancelEventHandler Closing
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_CLOSING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_CLOSING, value);
			}
		}

		// Token: 0x14000203 RID: 515
		// (add) Token: 0x06003D65 RID: 15717 RVA: 0x000DC9FA File Offset: 0x000DB9FA
		// (remove) Token: 0x06003D66 RID: 15718 RVA: 0x000DCA0D File Offset: 0x000DBA0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnClosedDescr")]
		[Browsable(false)]
		public event EventHandler Closed
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_CLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_CLOSED, value);
			}
		}

		// Token: 0x14000204 RID: 516
		// (add) Token: 0x06003D67 RID: 15719 RVA: 0x000DCA20 File Offset: 0x000DBA20
		// (remove) Token: 0x06003D68 RID: 15720 RVA: 0x000DCA33 File Offset: 0x000DBA33
		[SRDescription("FormOnDeactivateDescr")]
		[SRCategory("CatFocus")]
		public event EventHandler Deactivate
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_DEACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_DEACTIVATE, value);
			}
		}

		// Token: 0x14000205 RID: 517
		// (add) Token: 0x06003D69 RID: 15721 RVA: 0x000DCA46 File Offset: 0x000DBA46
		// (remove) Token: 0x06003D6A RID: 15722 RVA: 0x000DCA59 File Offset: 0x000DBA59
		[SRDescription("FormOnFormClosingDescr")]
		[SRCategory("CatBehavior")]
		public event FormClosingEventHandler FormClosing
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_FORMCLOSING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_FORMCLOSING, value);
			}
		}

		// Token: 0x14000206 RID: 518
		// (add) Token: 0x06003D6B RID: 15723 RVA: 0x000DCA6C File Offset: 0x000DBA6C
		// (remove) Token: 0x06003D6C RID: 15724 RVA: 0x000DCA7F File Offset: 0x000DBA7F
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnFormClosedDescr")]
		public event FormClosedEventHandler FormClosed
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_FORMCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_FORMCLOSED, value);
			}
		}

		// Token: 0x14000207 RID: 519
		// (add) Token: 0x06003D6D RID: 15725 RVA: 0x000DCA92 File Offset: 0x000DBA92
		// (remove) Token: 0x06003D6E RID: 15726 RVA: 0x000DCAA5 File Offset: 0x000DBAA5
		[SRDescription("FormOnLoadDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_LOAD, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_LOAD, value);
			}
		}

		// Token: 0x14000208 RID: 520
		// (add) Token: 0x06003D6F RID: 15727 RVA: 0x000DCAB8 File Offset: 0x000DBAB8
		// (remove) Token: 0x06003D70 RID: 15728 RVA: 0x000DCACB File Offset: 0x000DBACB
		[SRCategory("CatLayout")]
		[SRDescription("FormOnMDIChildActivateDescr")]
		public event EventHandler MdiChildActivate
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MDI_CHILD_ACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MDI_CHILD_ACTIVATE, value);
			}
		}

		// Token: 0x14000209 RID: 521
		// (add) Token: 0x06003D71 RID: 15729 RVA: 0x000DCADE File Offset: 0x000DBADE
		// (remove) Token: 0x06003D72 RID: 15730 RVA: 0x000DCAF1 File Offset: 0x000DBAF1
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnMenuCompleteDescr")]
		public event EventHandler MenuComplete
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MENUCOMPLETE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MENUCOMPLETE, value);
			}
		}

		// Token: 0x1400020A RID: 522
		// (add) Token: 0x06003D73 RID: 15731 RVA: 0x000DCB04 File Offset: 0x000DBB04
		// (remove) Token: 0x06003D74 RID: 15732 RVA: 0x000DCB17 File Offset: 0x000DBB17
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnMenuStartDescr")]
		public event EventHandler MenuStart
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_MENUSTART, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_MENUSTART, value);
			}
		}

		// Token: 0x1400020B RID: 523
		// (add) Token: 0x06003D75 RID: 15733 RVA: 0x000DCB2A File Offset: 0x000DBB2A
		// (remove) Token: 0x06003D76 RID: 15734 RVA: 0x000DCB3D File Offset: 0x000DBB3D
		[SRDescription("FormOnInputLangChangeDescr")]
		[SRCategory("CatBehavior")]
		public event InputLanguageChangedEventHandler InputLanguageChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_INPUTLANGCHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_INPUTLANGCHANGE, value);
			}
		}

		// Token: 0x1400020C RID: 524
		// (add) Token: 0x06003D77 RID: 15735 RVA: 0x000DCB50 File Offset: 0x000DBB50
		// (remove) Token: 0x06003D78 RID: 15736 RVA: 0x000DCB63 File Offset: 0x000DBB63
		[SRCategory("CatBehavior")]
		[SRDescription("FormOnInputLangChangeRequestDescr")]
		public event InputLanguageChangingEventHandler InputLanguageChanging
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_INPUTLANGCHANGEREQUEST, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_INPUTLANGCHANGEREQUEST, value);
			}
		}

		// Token: 0x1400020D RID: 525
		// (add) Token: 0x06003D79 RID: 15737 RVA: 0x000DCB76 File Offset: 0x000DBB76
		// (remove) Token: 0x06003D7A RID: 15738 RVA: 0x000DCB89 File Offset: 0x000DBB89
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		// Token: 0x1400020E RID: 526
		// (add) Token: 0x06003D7B RID: 15739 RVA: 0x000DCB9C File Offset: 0x000DBB9C
		// (remove) Token: 0x06003D7C RID: 15740 RVA: 0x000DCBAF File Offset: 0x000DBBAF
		[SRDescription("FormOnShownDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler Shown
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_SHOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_SHOWN, value);
			}
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x000DCBC4 File Offset: 0x000DBBC4
		public void Activate()
		{
			IntSecurity.ModifyFocus.Demand();
			if (base.Visible && base.IsHandleCreated)
			{
				if (this.IsMdiChild)
				{
					this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
					return;
				}
				UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this, base.Handle));
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x000DCC23 File Offset: 0x000DBC23
		protected void ActivateMdiChild(Form form)
		{
			IntSecurity.ModifyFocus.Demand();
			this.ActivateMdiChildInternal(form);
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x000DCC38 File Offset: 0x000DBC38
		private void ActivateMdiChildInternal(Form form)
		{
			if (this.FormerlyActiveMdiChild != null && !this.FormerlyActiveMdiChild.IsClosing)
			{
				this.FormerlyActiveMdiChild.UpdateWindowIcon(true);
				this.FormerlyActiveMdiChild = null;
			}
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			if (activeMdiChildInternal == form)
			{
				return;
			}
			if (activeMdiChildInternal != null)
			{
				activeMdiChildInternal.Active = false;
			}
			this.ActiveMdiChildInternal = form;
			if (form != null)
			{
				form.IsMdiChildFocusable = true;
				form.Active = true;
			}
			else if (this.Active)
			{
				base.ActivateControlInternal(this);
			}
			this.OnMdiChildActivate(EventArgs.Empty);
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x000DCCBC File Offset: 0x000DBCBC
		public void AddOwnedForm(Form ownedForm)
		{
			if (ownedForm == null)
			{
				return;
			}
			if (ownedForm.OwnerInternal != this)
			{
				ownedForm.Owner = this;
				return;
			}
			Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
			int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
			for (int i = 0; i < integer; i++)
			{
				if (array[i] == ownedForm)
				{
					return;
				}
			}
			if (array == null)
			{
				array = new Form[4];
				base.Properties.SetObject(Form.PropOwnedForms, array);
			}
			else if (array.Length == integer)
			{
				Form[] array2 = new Form[integer * 2];
				Array.Copy(array, 0, array2, 0, integer);
				array = array2;
				base.Properties.SetObject(Form.PropOwnedForms, array);
			}
			array[integer] = ownedForm;
			base.Properties.SetInteger(Form.PropOwnedFormsCount, integer + 1);
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x000DCD78 File Offset: 0x000DBD78
		private float AdjustScale(float scale)
		{
			if (scale < 0.92f)
			{
				return scale + 0.08f;
			}
			if (scale < 1f)
			{
				return 1f;
			}
			if (scale > 1.01f)
			{
				return scale + 0.08f;
			}
			return scale;
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x000DCDA9 File Offset: 0x000DBDA9
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void AdjustFormScrollbars(bool displayScrollbars)
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				base.AdjustFormScrollbars(displayScrollbars);
			}
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x000DCDBC File Offset: 0x000DBDBC
		private void AdjustSystemMenu(IntPtr hmenu)
		{
			this.UpdateWindowState();
			FormWindowState windowState = this.WindowState;
			FormBorderStyle formBorderStyle = this.FormBorderStyle;
			bool flag = formBorderStyle == FormBorderStyle.SizableToolWindow || formBorderStyle == FormBorderStyle.Sizable;
			bool flag2 = this.MinimizeBox && windowState != FormWindowState.Minimized;
			bool flag3 = this.MaximizeBox && windowState != FormWindowState.Maximized;
			bool controlBox = this.ControlBox;
			bool flag4 = windowState != FormWindowState.Normal;
			bool flag5 = flag && windowState != FormWindowState.Minimized && windowState != FormWindowState.Maximized;
			if (!flag2)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61472, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61472, 0);
			}
			if (!flag3)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61488, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61488, 0);
			}
			if (!controlBox)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61536, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61536, 0);
			}
			if (!flag4)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61728, 1);
			}
			else
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61728, 0);
			}
			if (!flag5)
			{
				UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61440, 1);
				return;
			}
			UnsafeNativeMethods.EnableMenuItem(new HandleRef(this, hmenu), 61440, 0);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x000DCF10 File Offset: 0x000DBF10
		private void AdjustSystemMenu()
		{
			if (base.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.GetSystemMenu(new HandleRef(this, base.Handle), false);
				this.AdjustSystemMenu(intPtr);
				intPtr = IntPtr.Zero;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x000DCF48 File Offset: 0x000DBF48
		[Obsolete("This method has been deprecated. Use the ApplyAutoScaling method instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void ApplyAutoScaling()
		{
			if (!this.autoScaleBaseSize.IsEmpty)
			{
				Size size = this.AutoScaleBaseSize;
				SizeF autoScaleSize = Form.GetAutoScaleSize(this.Font);
				Size size2 = new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
				if (size.Equals(size2))
				{
					return;
				}
				float num = this.AdjustScale((float)size2.Height / (float)size.Height);
				float num2 = this.AdjustScale((float)size2.Width / (float)size.Width);
				base.Scale(num2, num);
				this.AutoScaleBaseSize = size2;
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x000DCFF4 File Offset: 0x000DBFF4
		private void ApplyClientSize()
		{
			if (this.formState[Form.FormStateWindowState] != 0 || !base.IsHandleCreated)
			{
				return;
			}
			Size clientSize = this.ClientSize;
			bool hscroll = base.HScroll;
			bool vscroll = base.VScroll;
			bool flag = false;
			if (this.formState[Form.FormStateSetClientSize] != 0)
			{
				flag = true;
				this.formState[Form.FormStateSetClientSize] = 0;
			}
			if (flag)
			{
				if (hscroll)
				{
					clientSize.Height += SystemInformation.HorizontalScrollBarHeight;
				}
				if (vscroll)
				{
					clientSize.Width += SystemInformation.VerticalScrollBarWidth;
				}
			}
			IntPtr handle = base.Handle;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetClientRect(new HandleRef(this, handle), ref rect);
			Rectangle rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			Rectangle bounds = base.Bounds;
			if (clientSize.Width != rectangle.Width)
			{
				Size size = this.ComputeWindowSize(clientSize);
				if (vscroll)
				{
					size.Width += SystemInformation.VerticalScrollBarWidth;
				}
				if (hscroll)
				{
					size.Height += SystemInformation.HorizontalScrollBarHeight;
				}
				bounds.Width = size.Width;
				bounds.Height = size.Height;
				base.Bounds = bounds;
				SafeNativeMethods.GetClientRect(new HandleRef(this, handle), ref rect);
				rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
			if (clientSize.Height != rectangle.Height)
			{
				int num = clientSize.Height - rectangle.Height;
				bounds.Height += num;
				base.Bounds = bounds;
			}
			base.UpdateBounds();
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x000DD1B0 File Offset: 0x000DC1B0
		internal override void AssignParent(Control value)
		{
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null && form.MdiClient != value)
			{
				base.Properties.SetObject(Form.PropFormMdiParent, null);
			}
			base.AssignParent(value);
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x000DD1F8 File Offset: 0x000DC1F8
		internal bool CheckCloseDialog(bool closingOnly)
		{
			if (this.dialogResult == DialogResult.None && base.Visible)
			{
				return false;
			}
			try
			{
				FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(this.closeReason, false);
				if (!this.CalledClosing)
				{
					this.OnClosing(formClosingEventArgs);
					this.OnFormClosing(formClosingEventArgs);
					if (formClosingEventArgs.Cancel)
					{
						this.dialogResult = DialogResult.None;
					}
					else
					{
						this.CalledClosing = true;
					}
				}
				if (!closingOnly && this.dialogResult != DialogResult.None)
				{
					FormClosedEventArgs formClosedEventArgs = new FormClosedEventArgs(this.closeReason);
					this.OnClosed(formClosedEventArgs);
					this.OnFormClosed(formClosedEventArgs);
					this.CalledClosing = false;
				}
			}
			catch (Exception ex)
			{
				this.dialogResult = DialogResult.None;
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					throw;
				}
				Application.OnThreadException(ex);
			}
			return this.dialogResult != DialogResult.None || !base.Visible;
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x000DD2BC File Offset: 0x000DC2BC
		public void Close()
		{
			if (base.GetState(262144))
			{
				throw new InvalidOperationException(SR.GetString("ClosingWhileCreatingHandle", new object[] { "Close" }));
			}
			if (base.IsHandleCreated)
			{
				this.closeReason = CloseReason.UserClosing;
				base.SendMessage(16, 0, 0);
				return;
			}
			base.Dispose();
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x000DD318 File Offset: 0x000DC318
		private Size ComputeWindowSize(Size clientSize)
		{
			CreateParams createParams = this.CreateParams;
			return this.ComputeWindowSize(clientSize, createParams.Style, createParams.ExStyle);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x000DD340 File Offset: 0x000DC340
		private Size ComputeWindowSize(Size clientSize, int style, int exStyle)
		{
			NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, clientSize.Width, clientSize.Height);
			SafeNativeMethods.AdjustWindowRectEx(ref rect, style, this.HasMenu, exStyle);
			return new Size(rect.right - rect.left, rect.bottom - rect.top);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x000DD39C File Offset: 0x000DC39C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new Form.ControlCollection(this);
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x000DD3A4 File Offset: 0x000DC3A4
		internal override void AfterControlRemoved(Control control, Control oldParent)
		{
			base.AfterControlRemoved(control, oldParent);
			if (control == this.AcceptButton)
			{
				this.AcceptButton = null;
			}
			if (control == this.CancelButton)
			{
				this.CancelButton = null;
			}
			if (control == this.ctlClient)
			{
				this.ctlClient = null;
				this.UpdateMenuHandles();
			}
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x000DD3E4 File Offset: 0x000DC3E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void CreateHandle()
		{
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null)
			{
				form.SuspendUpdateMenuHandles();
			}
			try
			{
				if (this.IsMdiChild && this.MdiParentInternal.IsHandleCreated)
				{
					MdiClient mdiClient = this.MdiParentInternal.MdiClient;
					if (mdiClient != null && !mdiClient.IsHandleCreated)
					{
						mdiClient.CreateControl();
					}
				}
				if (this.IsMdiChild && this.formState[Form.FormStateWindowState] == 2)
				{
					this.formState[Form.FormStateWindowState] = 0;
					this.formState[Form.FormStateMdiChildMax] = 1;
					base.CreateHandle();
					this.formState[Form.FormStateWindowState] = 2;
					this.formState[Form.FormStateMdiChildMax] = 0;
				}
				else
				{
					base.CreateHandle();
				}
				this.UpdateHandleWithOwner();
				this.UpdateWindowIcon(false);
				this.AdjustSystemMenu();
				if (this.formState[Form.FormStateStartPos] != 3)
				{
					this.ApplyClientSize();
				}
				if (this.formState[Form.FormStateShowWindowOnCreate] == 1)
				{
					base.Visible = true;
				}
				if (this.Menu != null || !this.TopLevel || this.IsMdiContainer)
				{
					this.UpdateMenuHandles();
				}
				if (!this.ShowInTaskbar && this.OwnerInternal == null && this.TopLevel)
				{
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, this.TaskbarOwner);
					Icon icon = this.Icon;
					if (icon != null && this.TaskbarOwner.Handle != IntPtr.Zero)
					{
						UnsafeNativeMethods.SendMessage(this.TaskbarOwner, 128, 1, icon.Handle);
					}
				}
				if (this.formState[Form.FormStateTopMost] != 0)
				{
					this.TopMost = true;
				}
			}
			finally
			{
				if (form != null)
				{
					form.ResumeUpdateMenuHandles();
				}
				base.UpdateStyles();
			}
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x000DD5C8 File Offset: 0x000DC5C8
		private void DeactivateMdiChild()
		{
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			if (activeMdiChildInternal != null)
			{
				Form mdiParentInternal = activeMdiChildInternal.MdiParentInternal;
				activeMdiChildInternal.Active = false;
				activeMdiChildInternal.IsMdiChildFocusable = false;
				this.FormerlyActiveMdiChild = activeMdiChildInternal;
				bool flag = true;
				foreach (Form form in mdiParentInternal.MdiChildren)
				{
					if (form != this && form.Visible)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					mdiParentInternal.ActivateMdiChildInternal(null);
				}
				this.ActiveMdiChildInternal = null;
				this.UpdateMenuHandles();
				this.UpdateToolStrip();
			}
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x000DD64C File Offset: 0x000DC64C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void DefWndProc(ref Message m)
		{
			if (this.ctlClient != null && this.ctlClient.IsHandleCreated && this.ctlClient.ParentInternal == this)
			{
				m.Result = UnsafeNativeMethods.DefFrameProc(m.HWnd, this.ctlClient.Handle, m.Msg, m.WParam, m.LParam);
				return;
			}
			if (this.formStateEx[Form.FormStateExUseMdiChildProc] != 0)
			{
				m.Result = UnsafeNativeMethods.DefMDIChildProc(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			base.DefWndProc(ref m);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x000DD6E8 File Offset: 0x000DC6E8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.CalledOnLoad = false;
				this.CalledMakeVisible = false;
				this.CalledCreateControl = false;
				if (base.Properties.ContainsObject(Form.PropAcceptButton))
				{
					base.Properties.SetObject(Form.PropAcceptButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropCancelButton))
				{
					base.Properties.SetObject(Form.PropCancelButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropDefaultButton))
				{
					base.Properties.SetObject(Form.PropDefaultButton, null);
				}
				if (base.Properties.ContainsObject(Form.PropActiveMdiChild))
				{
					base.Properties.SetObject(Form.PropActiveMdiChild, null);
				}
				if (this.MdiWindowListStrip != null)
				{
					this.MdiWindowListStrip.Dispose();
					this.MdiWindowListStrip = null;
				}
				if (this.MdiControlStrip != null)
				{
					this.MdiControlStrip.Dispose();
					this.MdiControlStrip = null;
				}
				if (this.MainMenuStrip != null)
				{
					this.MainMenuStrip = null;
				}
				Form form = (Form)base.Properties.GetObject(Form.PropOwner);
				if (form != null)
				{
					form.RemoveOwnedForm(this);
					base.Properties.SetObject(Form.PropOwner, null);
				}
				Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				for (int i = integer - 1; i >= 0; i--)
				{
					if (array[i] != null)
					{
						array[i].Dispose();
					}
				}
				if (this.smallIcon != null)
				{
					this.smallIcon.Dispose();
					this.smallIcon = null;
				}
				this.ResetSecurityTip(false);
				base.Dispose(disposing);
				this.ctlClient = null;
				MainMenu menu = this.Menu;
				if (menu != null && menu.ownerForm == this)
				{
					menu.Dispose();
					base.Properties.SetObject(Form.PropMainMenu, null);
				}
				if (base.Properties.GetObject(Form.PropCurMenu) != null)
				{
					base.Properties.SetObject(Form.PropCurMenu, null);
				}
				this.MenuChanged(0, null);
				MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
				if (mainMenu != null)
				{
					mainMenu.Dispose();
					base.Properties.SetObject(Form.PropDummyMenu, null);
				}
				MainMenu mainMenu2 = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
				if (mainMenu2 != null)
				{
					if (mainMenu2.ownerForm == this || mainMenu2.form == null)
					{
						mainMenu2.Dispose();
					}
					base.Properties.SetObject(Form.PropMergedMenu, null);
					return;
				}
			}
			else
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x000DD95C File Offset: 0x000DC95C
		private void FillInCreateParamsBorderIcons(CreateParams cp)
		{
			if (this.FormBorderStyle != FormBorderStyle.None)
			{
				if (this.Text != null && this.Text.Length != 0)
				{
					cp.Style |= 12582912;
				}
				if (this.ControlBox || this.IsRestrictedWindow)
				{
					cp.Style |= 13107200;
				}
				else
				{
					cp.Style &= -524289;
				}
				if (this.MaximizeBox || this.IsRestrictedWindow)
				{
					cp.Style |= 65536;
				}
				else
				{
					cp.Style &= -65537;
				}
				if (this.MinimizeBox || this.IsRestrictedWindow)
				{
					cp.Style |= 131072;
				}
				else
				{
					cp.Style &= -131073;
				}
				if (this.HelpButton && !this.MaximizeBox && !this.MinimizeBox && this.ControlBox)
				{
					cp.ExStyle |= 1024;
					return;
				}
				cp.ExStyle &= -1025;
			}
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x000DDA84 File Offset: 0x000DCA84
		private void FillInCreateParamsBorderStyles(CreateParams cp)
		{
			switch (this.formState[Form.FormStateBorderStyle])
			{
			case 0:
				if (!this.IsRestrictedWindow)
				{
					return;
				}
				break;
			case 1:
				break;
			case 2:
				cp.Style |= 8388608;
				cp.ExStyle |= 512;
				return;
			case 3:
				cp.Style |= 8388608;
				cp.ExStyle |= 1;
				return;
			case 4:
				cp.Style |= 8650752;
				return;
			case 5:
				cp.Style |= 8388608;
				cp.ExStyle |= 128;
				return;
			case 6:
				cp.Style |= 8650752;
				cp.ExStyle |= 128;
				return;
			default:
				return;
			}
			cp.Style |= 8388608;
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x000DDB88 File Offset: 0x000DCB88
		private void FillInCreateParamsStartPosition(CreateParams cp)
		{
			if (this.formState[Form.FormStateSetClientSize] != 0)
			{
				int num = cp.Style & -553648129;
				Size size = this.ComputeWindowSize(this.ClientSize, num, cp.ExStyle);
				if (this.IsRestrictedWindow)
				{
					size = this.ApplyBoundsConstraints(cp.X, cp.Y, size.Width, size.Height).Size;
				}
				cp.Width = size.Width;
				cp.Height = size.Height;
			}
			switch (this.formState[Form.FormStateStartPos])
			{
			case 1:
			{
				if (this.IsMdiChild)
				{
					Control mdiClient = this.MdiParentInternal.MdiClient;
					Rectangle clientRectangle = mdiClient.ClientRectangle;
					cp.X = Math.Max(clientRectangle.X, clientRectangle.X + (clientRectangle.Width - cp.Width) / 2);
					cp.Y = Math.Max(clientRectangle.Y, clientRectangle.Y + (clientRectangle.Height - cp.Height) / 2);
					return;
				}
				IWin32Window win32Window = (IWin32Window)base.Properties.GetObject(Form.PropDialogOwner);
				Screen screen;
				if (this.OwnerInternal != null || win32Window != null)
				{
					IntPtr intPtr = ((win32Window != null) ? Control.GetSafeHandle(win32Window) : this.OwnerInternal.Handle);
					screen = Screen.FromHandleInternal(intPtr);
				}
				else
				{
					screen = Screen.FromPoint(Control.MousePosition);
				}
				Rectangle workingArea = screen.WorkingArea;
				if (this.WindowState != FormWindowState.Maximized)
				{
					cp.X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - cp.Width) / 2);
					cp.Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - cp.Height) / 2);
				}
				return;
			}
			case 2:
			case 4:
				break;
			case 3:
				cp.Width = int.MinValue;
				cp.Height = int.MinValue;
				break;
			default:
				return;
			}
			if (this.IsMdiChild && this.Dock != DockStyle.None)
			{
				return;
			}
			cp.X = int.MinValue;
			cp.Y = int.MinValue;
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x000DDDAC File Offset: 0x000DCDAC
		private void FillInCreateParamsWindowState(CreateParams cp)
		{
			switch (this.formState[Form.FormStateWindowState])
			{
			case 1:
				cp.Style |= 536870912;
				return;
			case 2:
				cp.Style |= 16777216;
				return;
			default:
				return;
			}
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x000DDE00 File Offset: 0x000DCE00
		internal override bool FocusInternal()
		{
			if (this.IsMdiChild)
			{
				this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
				return this.Focused;
			}
			return base.FocusInternal();
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x000DDE34 File Offset: 0x000DCE34
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static SizeF GetAutoScaleSize(Font font)
		{
			float num = (float)font.Height;
			float num2 = 9f;
			try
			{
				using (Graphics graphics = Graphics.FromHwndInternal(IntPtr.Zero))
				{
					string text = "The quick brown fox jumped over the lazy dog.";
					double num3 = 44.54999694824219;
					float width = graphics.MeasureString(text, font).Width;
					num2 = (float)((double)width / num3);
				}
			}
			catch
			{
			}
			return new SizeF(num2, num);
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x000DDEBC File Offset: 0x000DCEBC
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			return base.GetPreferredSizeCore(proposedSize);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x000DDED4 File Offset: 0x000DCED4
		private void ResolveZoneAndSiteNames(ArrayList sites, ref string securityZone, ref string securitySite)
		{
			securityZone = SR.GetString("SecurityRestrictedWindowTextUnknownZone");
			securitySite = SR.GetString("SecurityRestrictedWindowTextUnknownSite");
			try
			{
				if (sites != null && sites.Count != 0)
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in sites)
					{
						if (obj == null)
						{
							return;
						}
						string text = obj.ToString();
						if (text.Length == 0)
						{
							return;
						}
						Zone zone = Zone.CreateFromUrl(text);
						if (!zone.SecurityZone.Equals(SecurityZone.MyComputer))
						{
							string text2 = zone.SecurityZone.ToString();
							if (!arrayList.Contains(text2))
							{
								arrayList.Add(text2);
							}
						}
					}
					if (arrayList.Count == 0)
					{
						securityZone = SecurityZone.MyComputer.ToString();
					}
					else if (arrayList.Count == 1)
					{
						securityZone = arrayList[0].ToString();
					}
					else
					{
						securityZone = SR.GetString("SecurityRestrictedWindowTextMixedZone");
					}
					ArrayList arrayList2 = new ArrayList();
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Assert();
					try
					{
						foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
						{
							if (assembly.GlobalAssemblyCache)
							{
								arrayList2.Add(assembly.CodeBase.ToUpper(CultureInfo.InvariantCulture));
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					ArrayList arrayList3 = new ArrayList();
					foreach (object obj2 in sites)
					{
						Uri uri = new Uri(obj2.ToString());
						if (!arrayList2.Contains(uri.AbsoluteUri.ToUpper(CultureInfo.InvariantCulture)))
						{
							string host = uri.Host;
							if (host.Length > 0 && !arrayList3.Contains(host))
							{
								arrayList3.Add(host);
							}
						}
					}
					if (arrayList3.Count == 0)
					{
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						try
						{
							securitySite = Environment.MachineName;
							goto IL_023E;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (arrayList3.Count == 1)
					{
						securitySite = arrayList3[0].ToString();
					}
					else
					{
						securitySite = SR.GetString("SecurityRestrictedWindowTextMultipleSites");
					}
					IL_023E:;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x000DE1A0 File Offset: 0x000DD1A0
		private string RestrictedWindowText(string original)
		{
			this.EnsureSecurityInformation();
			return string.Format(CultureInfo.CurrentCulture, Application.SafeTopLevelCaptionFormat, new object[] { original, this.securityZone, this.securitySite });
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x000DE1E0 File Offset: 0x000DD1E0
		private void EnsureSecurityInformation()
		{
			if (this.securityZone == null || this.securitySite == null)
			{
				ArrayList arrayList;
				ArrayList arrayList2;
				SecurityManager.GetZoneAndOrigin(out arrayList, out arrayList2);
				this.ResolveZoneAndSiteNames(arrayList2, ref this.securityZone, ref this.securitySite);
			}
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x000DE219 File Offset: 0x000DD219
		private void CallShownEvent()
		{
			this.OnShown(EventArgs.Empty);
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x000DE226 File Offset: 0x000DD226
		internal override bool CanSelectCore()
		{
			return base.GetStyle(ControlStyles.Selectable) && base.Enabled && base.Visible;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x000DE248 File Offset: 0x000DD248
		internal bool CanRecreateHandle()
		{
			return !this.IsMdiChild || (base.GetState(2) && base.IsHandleCreated);
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x000DE265 File Offset: 0x000DD265
		internal override bool CanProcessMnemonic()
		{
			return (!this.IsMdiChild || (this.formStateEx[Form.FormStateExMnemonicProcessed] != 1 && this == this.MdiParentInternal.ActiveMdiChildInternal && this.WindowState != FormWindowState.Minimized)) && base.CanProcessMnemonic();
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x000DE2A4 File Offset: 0x000DD2A4
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.ProcessMnemonic(charCode))
			{
				return true;
			}
			if (this.IsMdiContainer)
			{
				if (base.Controls.Count > 1)
				{
					for (int i = 0; i < base.Controls.Count; i++)
					{
						Control control = base.Controls[i];
						if (!(control is MdiClient) && control.ProcessMnemonic(charCode))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x000DE30C File Offset: 0x000DD30C
		protected void CenterToParent()
		{
			if (this.TopLevel)
			{
				Point point = default(Point);
				Size size = this.Size;
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				if (intPtr != IntPtr.Zero)
				{
					Screen screen = Screen.FromHandleInternal(intPtr);
					Rectangle workingArea = screen.WorkingArea;
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(null, intPtr), ref rect);
					point.X = (rect.left + rect.right - size.Width) / 2;
					if (point.X < workingArea.X)
					{
						point.X = workingArea.X;
					}
					else if (point.X + size.Width > workingArea.X + workingArea.Width)
					{
						point.X = workingArea.X + workingArea.Width - size.Width;
					}
					point.Y = (rect.top + rect.bottom - size.Height) / 2;
					if (point.Y < workingArea.Y)
					{
						point.Y = workingArea.Y;
					}
					else if (point.Y + size.Height > workingArea.Y + workingArea.Height)
					{
						point.Y = workingArea.Y + workingArea.Height - size.Height;
					}
					this.Location = point;
					return;
				}
				this.CenterToScreen();
			}
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x000DE490 File Offset: 0x000DD490
		protected void CenterToScreen()
		{
			Point point = default(Point);
			Screen screen;
			if (this.OwnerInternal != null)
			{
				screen = Screen.FromControl(this.OwnerInternal);
			}
			else
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.TopLevel)
				{
					intPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				}
				if (intPtr != IntPtr.Zero)
				{
					screen = Screen.FromHandleInternal(intPtr);
				}
				else
				{
					screen = Screen.FromPoint(Control.MousePosition);
				}
			}
			Rectangle workingArea = screen.WorkingArea;
			point.X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - base.Width) / 2);
			point.Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - base.Height) / 2);
			this.Location = point;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x000DE564 File Offset: 0x000DD564
		private void InvalidateMergedMenu()
		{
			if (base.Properties.ContainsObject(Form.PropMergedMenu))
			{
				MainMenu mainMenu = base.Properties.GetObject(Form.PropMergedMenu) as MainMenu;
				if (mainMenu != null && mainMenu.ownerForm == this)
				{
					mainMenu.Dispose();
				}
				base.Properties.SetObject(Form.PropMergedMenu, null);
			}
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null)
			{
				parentFormInternal.MenuChanged(0, parentFormInternal.Menu);
			}
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x000DE5D3 File Offset: 0x000DD5D3
		public void LayoutMdi(MdiLayout value)
		{
			if (this.ctlClient == null)
			{
				return;
			}
			this.ctlClient.LayoutMdi(value);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x000DE5EC File Offset: 0x000DD5EC
		internal void MenuChanged(int change, Menu menu)
		{
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null && this == parentFormInternal.ActiveMdiChildInternal)
			{
				parentFormInternal.MenuChanged(change, menu);
				return;
			}
			switch (change)
			{
			case 0:
			case 3:
				if (this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					if (base.IsHandleCreated)
					{
						this.UpdateMenuHandles(null, false);
					}
					Control.ControlCollection controls = this.ctlClient.Controls;
					int count = controls.Count;
					while (count-- > 0)
					{
						Control control = controls[count];
						if (control is Form && control.Properties.ContainsObject(Form.PropMergedMenu))
						{
							MainMenu mainMenu = control.Properties.GetObject(Form.PropMergedMenu) as MainMenu;
							if (mainMenu != null && mainMenu.ownerForm == control)
							{
								mainMenu.Dispose();
							}
							control.Properties.SetObject(Form.PropMergedMenu, null);
						}
					}
					this.UpdateMenuHandles();
					return;
				}
				if (menu == this.Menu && change == 0)
				{
					this.UpdateMenuHandles();
					return;
				}
				break;
			case 1:
				if (menu == this.Menu || (this.ActiveMdiChildInternal != null && menu == this.ActiveMdiChildInternal.Menu))
				{
					this.UpdateMenuHandles();
					return;
				}
				break;
			case 2:
				if (this.ctlClient != null && this.ctlClient.IsHandleCreated)
				{
					this.UpdateMenuHandles();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x000DE734 File Offset: 0x000DD734
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnActivated(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_ACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x000DE762 File Offset: 0x000DD762
		internal override void OnAutoScaleModeChanged()
		{
			base.OnAutoScaleModeChanged();
			if (this.formStateEx[Form.FormStateExSettingAutoScale] != 1)
			{
				this.AutoScale = false;
			}
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000DE784 File Offset: 0x000DD784
		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			base.OnBackgroundImageChanged(e);
			if (this.IsMdiContainer)
			{
				this.MdiClient.BackgroundImage = this.BackgroundImage;
				this.MdiClient.Invalidate();
			}
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x000DE7B1 File Offset: 0x000DD7B1
		protected override void OnBackgroundImageLayoutChanged(EventArgs e)
		{
			base.OnBackgroundImageLayoutChanged(e);
			if (this.IsMdiContainer)
			{
				this.MdiClient.BackgroundImageLayout = this.BackgroundImageLayout;
				this.MdiClient.Invalidate();
			}
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x000DE7E0 File Offset: 0x000DD7E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClosing(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Form.EVENT_CLOSING];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000DE810 File Offset: 0x000DD810
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnClosed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_CLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x000DE840 File Offset: 0x000DD840
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFormClosing(FormClosingEventArgs e)
		{
			FormClosingEventHandler formClosingEventHandler = (FormClosingEventHandler)base.Events[Form.EVENT_FORMCLOSING];
			if (formClosingEventHandler != null)
			{
				formClosingEventHandler(this, e);
			}
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x000DE870 File Offset: 0x000DD870
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFormClosed(FormClosedEventArgs e)
		{
			Application.OpenFormsInternalRemove(this);
			FormClosedEventHandler formClosedEventHandler = (FormClosedEventHandler)base.Events[Form.EVENT_FORMCLOSED];
			if (formClosedEventHandler != null)
			{
				formClosedEventHandler(this, e);
			}
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x000DE8A4 File Offset: 0x000DD8A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnCreateControl()
		{
			this.CalledCreateControl = true;
			base.OnCreateControl();
			if (this.CalledMakeVisible && !this.CalledOnLoad)
			{
				this.CalledOnLoad = true;
				this.OnLoad(EventArgs.Empty);
			}
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x000DE8D8 File Offset: 0x000DD8D8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDeactivate(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_DEACTIVATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x000DE908 File Offset: 0x000DD908
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!base.DesignMode && base.Enabled && this.Active)
			{
				if (base.ActiveControl == null)
				{
					base.SelectNextControlInternal(this, true, true, true, true);
					return;
				}
				base.FocusActiveControlInternal();
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x000DE951 File Offset: 0x000DD951
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (this.IsMdiChild)
			{
				base.UpdateFocusedControl();
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x000DE968 File Offset: 0x000DD968
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnFontChanged(EventArgs e)
		{
			if (base.DesignMode)
			{
				this.UpdateAutoScaleBaseSize();
			}
			base.OnFontChanged(e);
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x000DE97F File Offset: 0x000DD97F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			this.formStateEx[Form.FormStateExUseMdiChildProc] = ((this.IsMdiChild && base.Visible) ? 1 : 0);
			base.OnHandleCreated(e);
			this.UpdateLayered();
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x000DE9B2 File Offset: 0x000DD9B2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			this.formStateEx[Form.FormStateExUseMdiChildProc] = 0;
			Application.OpenFormsInternalRemove(this);
			this.ResetSecurityTip(true);
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x000DE9DC File Offset: 0x000DD9DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnHelpButtonClicked(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[Form.EVENT_HELPBUTTONCLICKED];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000DEA0C File Offset: 0x000DDA0C
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.AutoSize)
			{
				Size preferredSize = base.PreferredSize;
				this.minAutoSize = preferredSize;
				Size size = ((this.AutoSizeMode == AutoSizeMode.GrowAndShrink) ? preferredSize : LayoutUtils.UnionSizes(preferredSize, this.Size));
				if (this != null)
				{
					((IArrangedElement)this).SetBounds(new Rectangle(base.Left, base.Top, size.Width, size.Height), BoundsSpecified.None);
				}
			}
			base.OnLayout(levent);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x000DEA7C File Offset: 0x000DDA7C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			Application.OpenFormsInternalAdd(this);
			if (Application.UseWaitCursor)
			{
				base.UseWaitCursor = true;
			}
			if (this.formState[Form.FormStateAutoScaling] == 1 && !base.DesignMode)
			{
				this.formState[Form.FormStateAutoScaling] = 0;
				this.ApplyAutoScaling();
			}
			if (base.GetState(32))
			{
				FormStartPosition formStartPosition = (FormStartPosition)this.formState[Form.FormStateStartPos];
				if (formStartPosition == FormStartPosition.CenterParent)
				{
					this.CenterToParent();
				}
				else if (formStartPosition == FormStartPosition.CenterScreen)
				{
					this.CenterToScreen();
				}
			}
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_LOAD];
			if (eventHandler != null)
			{
				string text = this.Text;
				eventHandler(this, e);
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					control.Invalidate();
				}
			}
			if (base.IsHandleCreated)
			{
				base.BeginInvoke(new MethodInvoker(this.CallShownEvent));
			}
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x000DEB94 File Offset: 0x000DDB94
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaximizedBoundsChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MAXIMIZEDBOUNDSCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x000DEBC4 File Offset: 0x000DDBC4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaximumSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MAXIMUMSIZECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x000DEBF4 File Offset: 0x000DDBF4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMinimumSizeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Form.EVENT_MINIMUMSIZECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x000DEC24 File Offset: 0x000DDC24
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInputLanguageChanged(InputLanguageChangedEventArgs e)
		{
			InputLanguageChangedEventHandler inputLanguageChangedEventHandler = (InputLanguageChangedEventHandler)base.Events[Form.EVENT_INPUTLANGCHANGE];
			if (inputLanguageChangedEventHandler != null)
			{
				inputLanguageChangedEventHandler(this, e);
			}
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x000DEC54 File Offset: 0x000DDC54
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnInputLanguageChanging(InputLanguageChangingEventArgs e)
		{
			InputLanguageChangingEventHandler inputLanguageChangingEventHandler = (InputLanguageChangingEventHandler)base.Events[Form.EVENT_INPUTLANGCHANGEREQUEST];
			if (inputLanguageChangingEventHandler != null)
			{
				inputLanguageChangingEventHandler(this, e);
			}
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x000DEC84 File Offset: 0x000DDC84
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnVisibleChanged(EventArgs e)
		{
			this.UpdateRenderSizeGrip();
			Form mdiParentInternal = this.MdiParentInternal;
			if (mdiParentInternal != null)
			{
				mdiParentInternal.UpdateMdiWindowListStrip();
			}
			base.OnVisibleChanged(e);
			bool flag = false;
			if (base.IsHandleCreated && base.Visible && this.AcceptButton != null && UnsafeNativeMethods.SystemParametersInfo(95, 0, ref flag, 0) && flag)
			{
				Control control = this.AcceptButton as Control;
				NativeMethods.POINT point = new NativeMethods.POINT(control.Left + control.Width / 2, control.Top + control.Height / 2);
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
				if (!control.IsWindowObscured)
				{
					IntSecurity.AdjustCursorPosition.Assert();
					try
					{
						Cursor.Position = new Point(point.x, point.y);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x000DED64 File Offset: 0x000DDD64
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMdiChildActivate(EventArgs e)
		{
			this.UpdateMenuHandles();
			this.UpdateToolStrip();
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MDI_CHILD_ACTIVATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x000DEDA0 File Offset: 0x000DDDA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMenuStart(EventArgs e)
		{
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip != null)
			{
				securityToolTip.Pop(true);
			}
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MENUSTART];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x000DEDF0 File Offset: 0x000DDDF0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMenuComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_MENUCOMPLETE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x000DEE20 File Offset: 0x000DDE20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				Size clientSize = this.ClientSize;
				if (Application.RenderWithVisualStyles)
				{
					if (this.sizeGripRenderer == null)
					{
						this.sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
					}
					this.sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(clientSize.Width - 16, clientSize.Height - 16, 16, 16));
				}
				else
				{
					ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, clientSize.Width - 16, clientSize.Height - 16, 16, 16);
				}
			}
			if (this.IsMdiContainer)
			{
				e.Graphics.FillRectangle(SystemBrushes.AppWorkspace, base.ClientRectangle);
			}
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x000DEEE3 File Offset: 0x000DDEE3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x000DEF04 File Offset: 0x000DDF04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			EventHandler eventHandler = base.Events[Form.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					control.RecreateHandleCore();
				}
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x000DEFA0 File Offset: 0x000DDFA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnShown(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_SHOWN];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x000DEFD0 File Offset: 0x000DDFD0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			int num = ((this.Text.Length == 0) ? 1 : 0);
			if (!this.ControlBox && this.formState[Form.FormStateIsTextEmpty] != num)
			{
				base.RecreateHandle();
			}
			this.formState[Form.FormStateIsTextEmpty] = num;
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x000DF028 File Offset: 0x000DE028
		internal void PerformOnInputLanguageChanged(InputLanguageChangedEventArgs iplevent)
		{
			this.OnInputLanguageChanged(iplevent);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x000DF031 File Offset: 0x000DE031
		internal void PerformOnInputLanguageChanging(InputLanguageChangingEventArgs iplcevent)
		{
			this.OnInputLanguageChanging(iplcevent);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x000DF03C File Offset: 0x000DE03C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (base.ProcessCmdKey(ref msg, keyData))
			{
				return true;
			}
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu != null && mainMenu.ProcessCmdKey(ref msg, keyData))
			{
				return true;
			}
			bool flag = false;
			NativeMethods.MSG msg2 = default(NativeMethods.MSG);
			msg2.message = msg.Msg;
			msg2.wParam = msg.WParam;
			msg2.lParam = msg.LParam;
			msg2.hwnd = msg.HWnd;
			if (this.ctlClient != null && this.ctlClient.Handle != IntPtr.Zero && UnsafeNativeMethods.TranslateMDISysAccel(this.ctlClient.Handle, ref msg2))
			{
				flag = true;
			}
			msg.Msg = msg2.message;
			msg.WParam = msg2.wParam;
			msg.LParam = msg2.lParam;
			msg.HWnd = msg2.hwnd;
			return flag;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x000DF124 File Offset: 0x000DE124
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keys;
				if (keys2 != Keys.Return)
				{
					if (keys2 == Keys.Escape)
					{
						IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropCancelButton);
						if (buttonControl != null)
						{
							buttonControl.PerformClick();
							return true;
						}
					}
				}
				else
				{
					IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropDefaultButton);
					if (buttonControl != null)
					{
						if (buttonControl is Control)
						{
							buttonControl.PerformClick();
						}
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x000DF1A4 File Offset: 0x000DE1A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogChar(char charCode)
		{
			if (this.IsMdiChild && charCode != ' ')
			{
				if (this.ProcessMnemonic(charCode))
				{
					return true;
				}
				this.formStateEx[Form.FormStateExMnemonicProcessed] = 1;
				try
				{
					return base.ProcessDialogChar(charCode);
				}
				finally
				{
					this.formStateEx[Form.FormStateExMnemonicProcessed] = 0;
				}
			}
			return base.ProcessDialogChar(charCode);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x000DF210 File Offset: 0x000DE210
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			return (this.formState[Form.FormStateKeyPreview] != 0 && this.ProcessKeyEventArgs(ref m)) || base.ProcessKeyPreview(ref m);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x000DF238 File Offset: 0x000DE238
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (base.SelectNextControl(base.ActiveControl, forward, true, true, true))
			{
				return true;
			}
			if (this.IsMdiChild || base.ParentFormInternal == null)
			{
				bool flag = base.SelectNextControl(null, forward, true, true, false);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x000DF27C File Offset: 0x000DE27C
		internal void RaiseFormClosedOnAppExit()
		{
			if (!this.Modal)
			{
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				if (integer > 0)
				{
					Form[] ownedForms = this.OwnedForms;
					FormClosedEventArgs formClosedEventArgs = new FormClosedEventArgs(CloseReason.FormOwnerClosing);
					for (int i = integer - 1; i >= 0; i--)
					{
						if (ownedForms[i] != null && !Application.OpenFormsInternal.Contains(ownedForms[i]))
						{
							ownedForms[i].OnFormClosed(formClosedEventArgs);
						}
					}
				}
			}
			this.OnFormClosed(new FormClosedEventArgs(CloseReason.ApplicationExitCall));
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x000DF2EC File Offset: 0x000DE2EC
		internal bool RaiseFormClosingOnAppExit()
		{
			FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
			if (!this.Modal)
			{
				int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				if (integer > 0)
				{
					Form[] ownedForms = this.OwnedForms;
					FormClosingEventArgs formClosingEventArgs2 = new FormClosingEventArgs(CloseReason.FormOwnerClosing, false);
					for (int i = integer - 1; i >= 0; i--)
					{
						if (ownedForms[i] != null && !Application.OpenFormsInternal.Contains(ownedForms[i]))
						{
							ownedForms[i].OnFormClosing(formClosingEventArgs2);
							if (formClosingEventArgs2.Cancel)
							{
								formClosingEventArgs.Cancel = true;
								break;
							}
						}
					}
				}
			}
			this.OnFormClosing(formClosingEventArgs);
			return formClosingEventArgs.Cancel;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x000DF380 File Offset: 0x000DE380
		internal override void RecreateHandleCore()
		{
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			FormStartPosition formStartPosition = FormStartPosition.Manual;
			if (!this.IsMdiChild && (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized))
			{
				windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
				UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
			}
			if (this.StartPosition != FormStartPosition.Manual)
			{
				formStartPosition = this.StartPosition;
				this.StartPosition = FormStartPosition.Manual;
			}
			Form.EnumThreadWindowsCallback enumThreadWindowsCallback = null;
			SafeNativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback2 = null;
			if (base.IsHandleCreated)
			{
				enumThreadWindowsCallback = new Form.EnumThreadWindowsCallback();
				if (enumThreadWindowsCallback != null)
				{
					enumThreadWindowsCallback2 = new SafeNativeMethods.EnumThreadWindowsCallback(enumThreadWindowsCallback.Callback);
					UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(enumThreadWindowsCallback2.Invoke), new HandleRef(this, base.Handle));
					enumThreadWindowsCallback.ResetOwners();
				}
			}
			base.RecreateHandleCore();
			if (enumThreadWindowsCallback != null)
			{
				enumThreadWindowsCallback.SetOwners(new HandleRef(this, base.Handle));
			}
			if (formStartPosition != FormStartPosition.Manual)
			{
				this.StartPosition = formStartPosition;
			}
			if (windowplacement.length > 0)
			{
				UnsafeNativeMethods.SetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
			}
			if (enumThreadWindowsCallback2 != null)
			{
				GC.KeepAlive(enumThreadWindowsCallback2);
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x000DF488 File Offset: 0x000DE488
		public void RemoveOwnedForm(Form ownedForm)
		{
			if (ownedForm == null)
			{
				return;
			}
			if (ownedForm.OwnerInternal != null)
			{
				ownedForm.Owner = null;
				return;
			}
			Form[] array = (Form[])base.Properties.GetObject(Form.PropOwnedForms);
			int num = base.Properties.GetInteger(Form.PropOwnedFormsCount);
			if (array != null)
			{
				for (int i = 0; i < num; i++)
				{
					if (ownedForm.Equals(array[i]))
					{
						array[i] = null;
						if (i + 1 < num)
						{
							Array.Copy(array, i + 1, array, i, num - i - 1);
							array[num - 1] = null;
						}
						num--;
					}
				}
				base.Properties.SetInteger(Form.PropOwnedFormsCount, num);
			}
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x000DF51F File Offset: 0x000DE51F
		private void ResetIcon()
		{
			this.icon = null;
			if (this.smallIcon != null)
			{
				this.smallIcon.Dispose();
				this.smallIcon = null;
			}
			this.formState[Form.FormStateIconSet] = 0;
			this.UpdateWindowIcon(true);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x000DF55C File Offset: 0x000DE55C
		private void ResetSecurityTip(bool modalOnly)
		{
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip != null && ((modalOnly && securityToolTip.Modal) || !modalOnly))
			{
				securityToolTip.Dispose();
				base.Properties.SetObject(Form.PropSecurityTip, null);
			}
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x000DF5A9 File Offset: 0x000DE5A9
		private void ResetTransparencyKey()
		{
			this.TransparencyKey = Color.Empty;
		}

		// Token: 0x1400020F RID: 527
		// (add) Token: 0x06003DD4 RID: 15828 RVA: 0x000DF5B6 File Offset: 0x000DE5B6
		// (remove) Token: 0x06003DD5 RID: 15829 RVA: 0x000DF5C9 File Offset: 0x000DE5C9
		[SRCategory("CatAction")]
		[SRDescription("FormOnResizeBeginDescr")]
		public event EventHandler ResizeBegin
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RESIZEBEGIN, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RESIZEBEGIN, value);
			}
		}

		// Token: 0x14000210 RID: 528
		// (add) Token: 0x06003DD6 RID: 15830 RVA: 0x000DF5DC File Offset: 0x000DE5DC
		// (remove) Token: 0x06003DD7 RID: 15831 RVA: 0x000DF5EF File Offset: 0x000DE5EF
		[SRCategory("CatAction")]
		[SRDescription("FormOnResizeEndDescr")]
		public event EventHandler ResizeEnd
		{
			add
			{
				base.Events.AddHandler(Form.EVENT_RESIZEEND, value);
			}
			remove
			{
				base.Events.RemoveHandler(Form.EVENT_RESIZEEND, value);
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x000DF602 File Offset: 0x000DE602
		private void ResumeLayoutFromMinimize()
		{
			if (this.formState[Form.FormStateWindowState] == 1)
			{
				base.ResumeLayout();
			}
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x000DF620 File Offset: 0x000DE620
		private void RestoreWindowBoundsIfNecessary()
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				Size size = this.restoredWindowBounds.Size;
				if ((this.restoredWindowBoundsSpecified & BoundsSpecified.Size) != BoundsSpecified.None)
				{
					size = base.SizeFromClientSize(size.Width, size.Height);
				}
				base.SetBounds(this.restoredWindowBounds.X, this.restoredWindowBounds.Y, (this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] == 1) ? size.Width : this.restoredWindowBounds.Width, (this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] == 1) ? size.Height : this.restoredWindowBounds.Height, this.restoredWindowBoundsSpecified);
				this.restoredWindowBoundsSpecified = BoundsSpecified.None;
				this.restoredWindowBounds = new Rectangle(-1, -1, -1, -1);
				this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 0;
				this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 0;
			}
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x000DF70C File Offset: 0x000DE70C
		private void RestrictedProcessNcActivate()
		{
			if (base.IsDisposed || base.Disposing)
			{
				return;
			}
			Form.SecurityToolTip securityToolTip = (Form.SecurityToolTip)base.Properties.GetObject(Form.PropSecurityTip);
			if (securityToolTip == null)
			{
				if (base.IsHandleCreated && UnsafeNativeMethods.GetForegroundWindow() == base.Handle)
				{
					securityToolTip = new Form.SecurityToolTip(this);
					base.Properties.SetObject(Form.PropSecurityTip, securityToolTip);
					return;
				}
			}
			else
			{
				if (!base.IsHandleCreated || UnsafeNativeMethods.GetForegroundWindow() != base.Handle)
				{
					securityToolTip.Pop(false);
					return;
				}
				securityToolTip.Show();
			}
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x000DF7A0 File Offset: 0x000DE7A0
		private void ResumeUpdateMenuHandles()
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			if (num <= 0)
			{
				throw new InvalidOperationException(SR.GetString("TooManyResumeUpdateMenuHandles"));
			}
			if ((this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount] = num - 1) == 0 && this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] != 0)
			{
				this.UpdateMenuHandles();
			}
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x000DF802 File Offset: 0x000DE802
		protected override void Select(bool directed, bool forward)
		{
			IntSecurity.ModifyFocus.Demand();
			this.SelectInternal(directed, forward);
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x000DF818 File Offset: 0x000DE818
		private void SelectInternal(bool directed, bool forward)
		{
			IntSecurity.ModifyFocus.Assert();
			if (directed)
			{
				base.SelectNextControl(null, forward, true, true, false);
			}
			if (this.TopLevel)
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, base.Handle));
				return;
			}
			if (this.IsMdiChild)
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this.MdiParentInternal, this.MdiParentInternal.Handle));
				this.MdiParentInternal.MdiClient.SendMessage(546, base.Handle, 0);
				return;
			}
			Form parentFormInternal = base.ParentFormInternal;
			if (parentFormInternal != null)
			{
				parentFormInternal.ActiveControl = this;
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x000DF8AC File Offset: 0x000DE8AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float x, float y)
		{
			base.SuspendLayout();
			try
			{
				if (this.WindowState == FormWindowState.Normal)
				{
					Size clientSize = this.ClientSize;
					Size minimumSize = this.MinimumSize;
					Size maximumSize = this.MaximumSize;
					if (!this.MinimumSize.IsEmpty)
					{
						this.MinimumSize = base.ScaleSize(minimumSize, x, y);
					}
					if (!this.MaximumSize.IsEmpty)
					{
						this.MaximumSize = base.ScaleSize(maximumSize, x, y);
					}
					this.ClientSize = base.ScaleSize(clientSize, x, y);
				}
				base.ScaleDockPadding(x, y);
				foreach (object obj in base.Controls)
				{
					Control control = (Control)obj;
					if (control != null)
					{
						control.Scale(x, y);
					}
				}
			}
			finally
			{
				base.ResumeLayout();
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x000DF9A0 File Offset: 0x000DE9A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			if (this.WindowState != FormWindowState.Normal)
			{
				bounds = this.RestoreBounds;
			}
			return base.GetScaledBounds(bounds, factor, specified);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x000DF9BC File Offset: 0x000DE9BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.formStateEx[Form.FormStateExInScale] = 1;
			try
			{
				if (this.MdiParentInternal != null)
				{
					specified &= ~(BoundsSpecified.X | BoundsSpecified.Y);
				}
				base.ScaleControl(factor, specified);
			}
			finally
			{
				this.formStateEx[Form.FormStateExInScale] = 0;
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x000DFA14 File Offset: 0x000DEA14
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.WindowState != FormWindowState.Normal)
			{
				if (x != -1 || y != -1)
				{
					this.restoredWindowBoundsSpecified |= specified & BoundsSpecified.Location;
				}
				this.restoredWindowBoundsSpecified |= specified & BoundsSpecified.Size;
				if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.X = x;
				}
				if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Y = y;
				}
				if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Width = width;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 0;
				}
				if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
				{
					this.restoredWindowBounds.Height = height;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 0;
				}
			}
			if ((specified & BoundsSpecified.X) != BoundsSpecified.None)
			{
				this.restoreBounds.X = x;
			}
			if ((specified & BoundsSpecified.Y) != BoundsSpecified.None)
			{
				this.restoreBounds.Y = y;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None || this.restoreBounds.Width == -1)
			{
				this.restoreBounds.Width = width;
			}
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None || this.restoreBounds.Height == -1)
			{
				this.restoreBounds.Height = height;
			}
			if (this.WindowState == FormWindowState.Normal && (base.Height != height || base.Width != width))
			{
				Size maxWindowTrackSize = SystemInformation.MaxWindowTrackSize;
				if (height > maxWindowTrackSize.Height)
				{
					height = maxWindowTrackSize.Height;
				}
				if (width > maxWindowTrackSize.Width)
				{
					width = maxWindowTrackSize.Width;
				}
			}
			FormBorderStyle formBorderStyle = this.FormBorderStyle;
			if (formBorderStyle != FormBorderStyle.None && formBorderStyle != FormBorderStyle.FixedToolWindow && formBorderStyle != FormBorderStyle.SizableToolWindow && this.ParentInternal == null)
			{
				Size minWindowTrackSize = SystemInformation.MinWindowTrackSize;
				if (height < minWindowTrackSize.Height)
				{
					height = minWindowTrackSize.Height;
				}
				if (width < minWindowTrackSize.Width)
				{
					width = minWindowTrackSize.Width;
				}
			}
			if (this.IsRestrictedWindow)
			{
				Rectangle rectangle = this.ApplyBoundsConstraints(x, y, width, height);
				if (rectangle != new Rectangle(x, y, width, height))
				{
					base.SetBoundsCore(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, BoundsSpecified.All);
					return;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x000DFC10 File Offset: 0x000DEC10
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			Rectangle rectangle = base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, proposedHeight);
			if (this.IsRestrictedWindow)
			{
				Screen[] allScreens = Screen.AllScreens;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				for (int i = 0; i < allScreens.Length; i++)
				{
					Rectangle workingArea = allScreens[i].WorkingArea;
					if (workingArea.Contains(suggestedX, suggestedY))
					{
						flag = true;
					}
					if (workingArea.Contains(suggestedX + proposedWidth, suggestedY))
					{
						flag2 = true;
					}
					if (workingArea.Contains(suggestedX, suggestedY + proposedHeight))
					{
						flag3 = true;
					}
					if (workingArea.Contains(suggestedX + proposedWidth, suggestedY + proposedHeight))
					{
						flag4 = true;
					}
				}
				if (!flag || !flag2 || !flag3 || !flag4)
				{
					if (this.formStateEx[Form.FormStateExInScale] == 1)
					{
						rectangle = WindowsFormsUtils.ConstrainToScreenWorkingAreaBounds(rectangle);
					}
					else
					{
						rectangle.X = base.Left;
						rectangle.Y = base.Top;
						rectangle.Width = base.Width;
						rectangle.Height = base.Height;
					}
				}
			}
			return rectangle;
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x000DFD00 File Offset: 0x000DED00
		private void SetDefaultButton(IButtonControl button)
		{
			IButtonControl buttonControl = (IButtonControl)base.Properties.GetObject(Form.PropDefaultButton);
			if (buttonControl != button)
			{
				if (buttonControl != null)
				{
					buttonControl.NotifyDefault(false);
				}
				base.Properties.SetObject(Form.PropDefaultButton, button);
				if (button != null)
				{
					button.NotifyDefault(true);
				}
			}
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x000DFD4C File Offset: 0x000DED4C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetClientSizeCore(int x, int y)
		{
			bool hscroll = base.HScroll;
			bool vscroll = base.VScroll;
			base.SetClientSizeCore(x, y);
			if (base.IsHandleCreated)
			{
				if (base.VScroll != vscroll && base.VScroll)
				{
					x += SystemInformation.VerticalScrollBarWidth;
				}
				if (base.HScroll != hscroll && base.HScroll)
				{
					y += SystemInformation.HorizontalScrollBarHeight;
				}
				if (x != this.ClientSize.Width || y != this.ClientSize.Height)
				{
					base.SetClientSizeCore(x, y);
				}
			}
			this.formState[Form.FormStateSetClientSize] = 1;
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x000DFDE8 File Offset: 0x000DEDE8
		public void SetDesktopBounds(int x, int y, int width, int height)
		{
			Rectangle workingArea = SystemInformation.WorkingArea;
			base.SetBounds(x + workingArea.X, y + workingArea.Y, width, height, BoundsSpecified.All);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x000DFE18 File Offset: 0x000DEE18
		public void SetDesktopLocation(int x, int y)
		{
			Rectangle workingArea = SystemInformation.WorkingArea;
			this.Location = new Point(workingArea.X + x, workingArea.Y + y);
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x000DFE48 File Offset: 0x000DEE48
		public void Show(IWin32Window owner)
		{
			if (owner == this)
			{
				throw new InvalidOperationException(SR.GetString("OwnsSelfOrOwner", new object[] { "Show" }));
			}
			if (base.Visible)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnVisible", new object[] { "Show" }));
			}
			if (!base.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnDisabled", new object[] { "Show" }));
			}
			if (!this.TopLevel)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnNonTopLevel", new object[] { "Show" }));
			}
			if (!SystemInformation.UserInteractive)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && ((int)UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, Control.GetSafeHandle(owner)), -20) & 8) == 0 && owner is Control)
			{
				owner = ((Control)owner).TopLevelControlInternal;
			}
			IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
			IntPtr intPtr = ((owner == null) ? activeWindow : Control.GetSafeHandle(owner));
			IntPtr zero = IntPtr.Zero;
			base.Properties.SetObject(Form.PropDialogOwner, owner);
			Form ownerInternal = this.OwnerInternal;
			if (owner is Form && owner != ownerInternal)
			{
				this.Owner = (Form)owner;
			}
			if (intPtr != IntPtr.Zero && intPtr != base.Handle)
			{
				if (UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, intPtr), -8) == base.Handle)
				{
					throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[] { "show" }), "owner");
				}
				UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(owner, intPtr));
			}
			base.Visible = true;
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x000E0025 File Offset: 0x000DF025
		public DialogResult ShowDialog()
		{
			return this.ShowDialog(null);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x000E0030 File Offset: 0x000DF030
		public DialogResult ShowDialog(IWin32Window owner)
		{
			if (owner == this)
			{
				throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[] { "showDialog" }), "owner");
			}
			if (base.Visible)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnVisible", new object[] { "showDialog" }));
			}
			if (!base.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnDisabled", new object[] { "showDialog" }));
			}
			if (!this.TopLevel)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnNonTopLevel", new object[] { "showDialog" }));
			}
			if (this.Modal)
			{
				throw new InvalidOperationException(SR.GetString("ShowDialogOnModal", new object[] { "showDialog" }));
			}
			if (!SystemInformation.UserInteractive)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && ((int)UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, Control.GetSafeHandle(owner)), -20) & 8) == 0 && owner is Control)
			{
				owner = ((Control)owner).TopLevelControlInternal;
			}
			this.CalledOnLoad = false;
			this.CalledMakeVisible = false;
			this.CloseReason = CloseReason.None;
			IntPtr capture = UnsafeNativeMethods.GetCapture();
			if (capture != IntPtr.Zero)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(null, capture), 31, IntPtr.Zero, IntPtr.Zero);
				SafeNativeMethods.ReleaseCapture();
			}
			IntPtr intPtr = UnsafeNativeMethods.GetActiveWindow();
			IntPtr intPtr2 = ((owner == null) ? intPtr : Control.GetSafeHandle(owner));
			IntPtr zero = IntPtr.Zero;
			base.Properties.SetObject(Form.PropDialogOwner, owner);
			Form ownerInternal = this.OwnerInternal;
			if (owner is Form && owner != ownerInternal)
			{
				this.Owner = (Form)owner;
			}
			try
			{
				base.SetState(32, true);
				this.dialogResult = DialogResult.None;
				base.CreateControl();
				if (intPtr2 != IntPtr.Zero && intPtr2 != base.Handle)
				{
					if (UnsafeNativeMethods.GetWindowLong(new HandleRef(owner, intPtr2), -8) == base.Handle)
					{
						throw new ArgumentException(SR.GetString("OwnsSelfOrOwner", new object[] { "showDialog" }), "owner");
					}
					UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -8);
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(owner, intPtr2));
				}
				try
				{
					if (this.dialogResult == DialogResult.None)
					{
						Application.RunDialog(this);
					}
				}
				finally
				{
					if (!UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
					{
						intPtr = intPtr2;
					}
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)) && SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, intPtr));
					}
					else if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr2)) && SafeNativeMethods.IsWindowVisible(new HandleRef(null, intPtr2)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, intPtr2));
					}
					this.SetVisibleCore(false);
					if (base.IsHandleCreated)
					{
						if (this.OwnerInternal != null && this.OwnerInternal.IsMdiContainer)
						{
							this.OwnerInternal.Invalidate(true);
							this.OwnerInternal.Update();
						}
						this.DestroyHandle();
					}
					base.SetState(32, false);
				}
			}
			finally
			{
				this.Owner = ownerInternal;
				base.Properties.SetObject(Form.PropDialogOwner, null);
			}
			return this.DialogResult;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x000E03B0 File Offset: 0x000DF3B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAutoScaleBaseSize()
		{
			return this.formState[Form.FormStateAutoScaling] != 0;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x000E03C8 File Offset: 0x000DF3C8
		private bool ShouldSerializeClientSize()
		{
			return true;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x000E03CB File Offset: 0x000DF3CB
		private bool ShouldSerializeIcon()
		{
			return this.formState[Form.FormStateIconSet] == 1;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x000E03E0 File Offset: 0x000DF3E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLocation()
		{
			return base.Left != 0 || base.Top != 0;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x000E03F8 File Offset: 0x000DF3F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeSize()
		{
			return false;
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x000E03FC File Offset: 0x000DF3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeTransparencyKey()
		{
			return !this.TransparencyKey.Equals(Color.Empty);
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x000E042A File Offset: 0x000DF42A
		private void SuspendLayoutForMinimize()
		{
			if (this.formState[Form.FormStateWindowState] != 1)
			{
				base.SuspendLayout();
			}
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x000E0448 File Offset: 0x000DF448
		private void SuspendUpdateMenuHandles()
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount] = num + 1;
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x000E047C File Offset: 0x000DF47C
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", Text: " + this.Text;
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x000E04A1 File Offset: 0x000DF4A1
		private void UpdateAutoScaleBaseSize()
		{
			this.autoScaleBaseSize = Size.Empty;
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x000E04B0 File Offset: 0x000DF4B0
		private void UpdateRenderSizeGrip()
		{
			int num = this.formState[Form.FormStateRenderSizeGrip];
			switch (this.FormBorderStyle)
			{
			case FormBorderStyle.None:
			case FormBorderStyle.FixedSingle:
			case FormBorderStyle.Fixed3D:
			case FormBorderStyle.FixedDialog:
			case FormBorderStyle.FixedToolWindow:
				this.formState[Form.FormStateRenderSizeGrip] = 0;
				break;
			case FormBorderStyle.Sizable:
			case FormBorderStyle.SizableToolWindow:
				switch (this.SizeGripStyle)
				{
				case SizeGripStyle.Auto:
					if (base.GetState(32))
					{
						this.formState[Form.FormStateRenderSizeGrip] = 1;
					}
					else
					{
						this.formState[Form.FormStateRenderSizeGrip] = 0;
					}
					break;
				case SizeGripStyle.Show:
					this.formState[Form.FormStateRenderSizeGrip] = 1;
					break;
				case SizeGripStyle.Hide:
					this.formState[Form.FormStateRenderSizeGrip] = 0;
					break;
				}
				break;
			}
			if (this.formState[Form.FormStateRenderSizeGrip] != num)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x000E0598 File Offset: 0x000DF598
		protected override void UpdateDefaultButton()
		{
			ContainerControl containerControl = this;
			while (containerControl.ActiveControl is ContainerControl)
			{
				containerControl = containerControl.ActiveControl as ContainerControl;
				if (containerControl is Form)
				{
					containerControl = this;
					break;
				}
			}
			if (containerControl.ActiveControl is IButtonControl)
			{
				this.SetDefaultButton((IButtonControl)containerControl.ActiveControl);
				return;
			}
			this.SetDefaultButton(this.AcceptButton);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x000E05FC File Offset: 0x000DF5FC
		private void UpdateHandleWithOwner()
		{
			if (base.IsHandleCreated && this.TopLevel)
			{
				HandleRef handleRef = NativeMethods.NullHandleRef;
				Form form = (Form)base.Properties.GetObject(Form.PropOwner);
				if (form != null)
				{
					handleRef = new HandleRef(form, form.Handle);
				}
				else if (!this.ShowInTaskbar)
				{
					handleRef = this.TaskbarOwner;
				}
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, handleRef);
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x000E066C File Offset: 0x000DF66C
		private void UpdateLayered()
		{
			if (this.formState[Form.FormStateLayered] != 0 && base.IsHandleCreated && this.TopLevel && OSFeature.Feature.IsPresent(OSFeature.LayeredWindows))
			{
				Color transparencyKey = this.TransparencyKey;
				bool flag;
				if (transparencyKey.IsEmpty)
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), 0, this.OpacityAsByte, 2);
				}
				else if (this.OpacityAsByte == 255)
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), ColorTranslator.ToWin32(transparencyKey), 0, 1);
				}
				else
				{
					flag = UnsafeNativeMethods.SetLayeredWindowAttributes(new HandleRef(this, base.Handle), ColorTranslator.ToWin32(transparencyKey), this.OpacityAsByte, 3);
				}
				if (!flag)
				{
					throw new Win32Exception();
				}
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x000E0734 File Offset: 0x000DF734
		private void UpdateMenuHandles()
		{
			if (base.Properties.GetObject(Form.PropCurMenu) != null)
			{
				base.Properties.SetObject(Form.PropCurMenu, null);
			}
			if (base.IsHandleCreated)
			{
				if (!this.TopLevel)
				{
					this.UpdateMenuHandles(null, true);
					return;
				}
				Form activeMdiChildInternal = this.ActiveMdiChildInternal;
				if (activeMdiChildInternal != null)
				{
					this.UpdateMenuHandles(activeMdiChildInternal.MergedMenuPrivate, true);
					return;
				}
				this.UpdateMenuHandles(this.Menu, true);
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x000E07A4 File Offset: 0x000DF7A4
		private void UpdateMenuHandles(MainMenu menu, bool forceRedraw)
		{
			int num = this.formStateEx[Form.FormStateExUpdateMenuHandlesSuspendCount];
			if (num > 0 && menu != null)
			{
				this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] = 1;
				return;
			}
			if (menu != null)
			{
				menu.form = this;
			}
			if (menu != null || base.Properties.ContainsObject(Form.PropCurMenu))
			{
				base.Properties.SetObject(Form.PropCurMenu, menu);
			}
			if (this.ctlClient == null || !this.ctlClient.IsHandleCreated)
			{
				if (menu != null)
				{
					UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), new HandleRef(menu, menu.Handle));
				}
				else
				{
					UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef);
				}
			}
			else
			{
				MenuStrip mainMenuStrip = this.MainMenuStrip;
				if (mainMenuStrip == null || menu != null)
				{
					MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
					if (mainMenu == null)
					{
						mainMenu = new MainMenu();
						mainMenu.ownerForm = this;
						base.Properties.SetObject(Form.PropDummyMenu, mainMenu);
					}
					UnsafeNativeMethods.SendMessage(new HandleRef(this.ctlClient, this.ctlClient.Handle), 560, mainMenu.Handle, IntPtr.Zero);
					if (menu != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this.ctlClient, this.ctlClient.Handle), 560, menu.Handle, IntPtr.Zero);
					}
				}
				if (menu == null && mainMenuStrip != null)
				{
					IntPtr menu2 = UnsafeNativeMethods.GetMenu(new HandleRef(this, base.Handle));
					if (menu2 != IntPtr.Zero)
					{
						UnsafeNativeMethods.SetMenu(new HandleRef(this, base.Handle), NativeMethods.NullHandleRef);
						Form activeMdiChildInternal = this.ActiveMdiChildInternal;
						if (activeMdiChildInternal != null && activeMdiChildInternal.WindowState == FormWindowState.Maximized)
						{
							activeMdiChildInternal.RecreateHandle();
						}
						CommonProperties.xClearPreferredSizeCache(this);
					}
				}
			}
			if (forceRedraw)
			{
				SafeNativeMethods.DrawMenuBar(new HandleRef(this, base.Handle));
			}
			this.formStateEx[Form.FormStateExUpdateMenuHandlesDeferred] = 0;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x000E0990 File Offset: 0x000DF990
		internal void UpdateFormStyles()
		{
			Size clientSize = this.ClientSize;
			base.UpdateStyles();
			if (!this.ClientSize.Equals(clientSize))
			{
				this.ClientSize = clientSize;
			}
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x000E09D0 File Offset: 0x000DF9D0
		private static Type FindClosestStockType(Type type)
		{
			Type[] array = new Type[] { typeof(MenuStrip) };
			foreach (Type type2 in array)
			{
				if (type2.IsAssignableFrom(type))
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x000E0A20 File Offset: 0x000DFA20
		private void UpdateToolStrip()
		{
			ToolStrip mainMenuStrip = this.MainMenuStrip;
			ArrayList arrayList = ToolStripManager.FindMergeableToolStrips(this.ActiveMdiChildInternal);
			if (mainMenuStrip != null)
			{
				ToolStripManager.RevertMerge(mainMenuStrip);
			}
			this.UpdateMdiWindowListStrip();
			if (this.ActiveMdiChildInternal != null)
			{
				foreach (object obj in arrayList)
				{
					ToolStrip toolStrip = (ToolStrip)obj;
					Type type = Form.FindClosestStockType(toolStrip.GetType());
					if (mainMenuStrip != null)
					{
						Type type2 = Form.FindClosestStockType(mainMenuStrip.GetType());
						if (type2 != null && type != null && type == type2 && mainMenuStrip.GetType().IsAssignableFrom(toolStrip.GetType()))
						{
							ToolStripManager.Merge(toolStrip, mainMenuStrip);
							break;
						}
					}
				}
			}
			Form activeMdiChildInternal = this.ActiveMdiChildInternal;
			this.UpdateMdiControlStrip(activeMdiChildInternal != null && activeMdiChildInternal.IsMaximized);
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x000E0B00 File Offset: 0x000DFB00
		private void UpdateMdiControlStrip(bool maximized)
		{
			if (this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] != 0)
			{
				return;
			}
			this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] = 1;
			try
			{
				MdiControlStrip mdiControlStrip = this.MdiControlStrip;
				if (this.MdiControlStrip != null)
				{
					if (mdiControlStrip.MergedMenu != null)
					{
						ToolStripManager.RevertMergeInternal(mdiControlStrip.MergedMenu, mdiControlStrip, true);
					}
					mdiControlStrip.MergedMenu = null;
					mdiControlStrip.Dispose();
					this.MdiControlStrip = null;
				}
				if (this.ActiveMdiChildInternal != null && maximized && this.ActiveMdiChildInternal.ControlBox && this.Menu == null)
				{
					IntPtr menu = UnsafeNativeMethods.GetMenu(new HandleRef(this, base.Handle));
					if (menu == IntPtr.Zero)
					{
						MenuStrip mainMenuStrip = ToolStripManager.GetMainMenuStrip(this);
						if (mainMenuStrip != null)
						{
							this.MdiControlStrip = new MdiControlStrip(this.ActiveMdiChildInternal);
							ToolStripManager.Merge(this.MdiControlStrip, mainMenuStrip);
							this.MdiControlStrip.MergedMenu = mainMenuStrip;
						}
					}
				}
			}
			finally
			{
				this.formStateEx[Form.FormStateExInUpdateMdiControlStrip] = 0;
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x000E0C04 File Offset: 0x000DFC04
		internal void UpdateMdiWindowListStrip()
		{
			if (this.IsMdiContainer)
			{
				if (this.MdiWindowListStrip != null && this.MdiWindowListStrip.MergedMenu != null)
				{
					ToolStripManager.RevertMergeInternal(this.MdiWindowListStrip.MergedMenu, this.MdiWindowListStrip, true);
				}
				MenuStrip mainMenuStrip = ToolStripManager.GetMainMenuStrip(this);
				if (mainMenuStrip != null && mainMenuStrip.MdiWindowListItem != null)
				{
					if (this.MdiWindowListStrip == null)
					{
						this.MdiWindowListStrip = new MdiWindowListStrip();
					}
					int count = mainMenuStrip.MdiWindowListItem.DropDownItems.Count;
					bool flag = count > 0 && !(mainMenuStrip.MdiWindowListItem.DropDownItems[count - 1] is ToolStripSeparator);
					this.MdiWindowListStrip.PopulateItems(this, mainMenuStrip.MdiWindowListItem, flag);
					ToolStripManager.Merge(this.MdiWindowListStrip, mainMenuStrip);
					this.MdiWindowListStrip.MergedMenu = mainMenuStrip;
				}
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x000E0CD4 File Offset: 0x000DFCD4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResizeBegin(EventArgs e)
		{
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_RESIZEBEGIN];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x000E0D0C File Offset: 0x000DFD0C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnResizeEnd(EventArgs e)
		{
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = (EventHandler)base.Events[Form.EVENT_RESIZEEND];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x000E0D42 File Offset: 0x000DFD42
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnStyleChanged(EventArgs e)
		{
			base.OnStyleChanged(e);
			this.AdjustSystemMenu();
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x000E0D54 File Offset: 0x000DFD54
		private void UpdateWindowIcon(bool redrawFrame)
		{
			if (base.IsHandleCreated)
			{
				Icon icon;
				if ((this.FormBorderStyle == FormBorderStyle.FixedDialog && this.formState[Form.FormStateIconSet] == 0 && !this.IsRestrictedWindow) || !this.ShowIcon)
				{
					icon = null;
				}
				else
				{
					icon = this.Icon;
				}
				if (icon != null)
				{
					if (this.smallIcon == null)
					{
						try
						{
							this.smallIcon = new Icon(icon, SystemInformation.SmallIconSize);
						}
						catch
						{
						}
					}
					if (this.smallIcon != null)
					{
						base.SendMessage(128, 0, this.smallIcon.Handle);
					}
					base.SendMessage(128, 1, icon.Handle);
				}
				else
				{
					base.SendMessage(128, 0, 0);
					base.SendMessage(128, 1, 0);
				}
				if (redrawFrame)
				{
					SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
				}
			}
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x000E0E44 File Offset: 0x000DFE44
		private void UpdateWindowState()
		{
			if (base.IsHandleCreated)
			{
				FormWindowState windowState = this.WindowState;
				NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
				windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
				UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, base.Handle), ref windowplacement);
				switch (windowplacement.showCmd)
				{
				case 1:
				case 4:
				case 5:
				case 8:
				case 9:
					if (this.formState[Form.FormStateWindowState] != 0)
					{
						this.formState[Form.FormStateWindowState] = 0;
					}
					break;
				case 2:
				case 6:
				case 7:
					if (this.formState[Form.FormStateMdiChildMax] == 0)
					{
						this.formState[Form.FormStateWindowState] = 1;
					}
					break;
				case 3:
					if (this.formState[Form.FormStateMdiChildMax] == 0)
					{
						this.formState[Form.FormStateWindowState] = 2;
					}
					break;
				}
				if (windowState == FormWindowState.Normal && this.WindowState != FormWindowState.Normal)
				{
					if (this.WindowState == FormWindowState.Minimized)
					{
						this.SuspendLayoutForMinimize();
					}
					this.restoredWindowBounds.Size = this.ClientSize;
					this.formStateEx[Form.FormStateExWindowBoundsWidthIsClientSize] = 1;
					this.formStateEx[Form.FormStateExWindowBoundsHeightIsClientSize] = 1;
					this.restoredWindowBoundsSpecified = BoundsSpecified.Size;
					this.restoredWindowBounds.Location = this.Location;
					this.restoredWindowBoundsSpecified |= BoundsSpecified.Location;
					this.restoreBounds.Size = this.Size;
					this.restoreBounds.Location = this.Location;
				}
				if (windowState == FormWindowState.Minimized && this.WindowState != FormWindowState.Minimized)
				{
					this.ResumeLayoutFromMinimize();
				}
				switch (this.WindowState)
				{
				case FormWindowState.Normal:
					base.SetState(65536, false);
					break;
				case FormWindowState.Minimized:
				case FormWindowState.Maximized:
					base.SetState(65536, true);
					break;
				}
				if (windowState != this.WindowState)
				{
					this.AdjustSystemMenu();
				}
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x000E102D File Offset: 0x000E002D
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override bool ValidateChildren()
		{
			return base.ValidateChildren();
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x000E1035 File Offset: 0x000E0035
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			return base.ValidateChildren(validationConstraints);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x000E103E File Offset: 0x000E003E
		private void WmActivate(ref Message m)
		{
			Application.FormActivated(this.Modal, true);
			this.Active = NativeMethods.Util.LOWORD(m.WParam) != 0;
			Application.FormActivated(this.Modal, this.Active);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x000E1074 File Offset: 0x000E0074
		private void WmEnterSizeMove(ref Message m)
		{
			this.formStateEx[Form.FormStateExInModalSizingLoop] = 1;
			this.OnResizeBegin(EventArgs.Empty);
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x000E1092 File Offset: 0x000E0092
		private void WmExitSizeMove(ref Message m)
		{
			this.formStateEx[Form.FormStateExInModalSizingLoop] = 0;
			this.OnResizeEnd(EventArgs.Empty);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x000E10B0 File Offset: 0x000E00B0
		private void WmCreate(ref Message m)
		{
			base.WndProc(ref m);
			NativeMethods.STARTUPINFO_I startupinfo_I = new NativeMethods.STARTUPINFO_I();
			UnsafeNativeMethods.GetStartupInfo(startupinfo_I);
			if (this.TopLevel && (startupinfo_I.dwFlags & 1) != 0)
			{
				short wShowWindow = startupinfo_I.wShowWindow;
				if (wShowWindow == 3)
				{
					this.WindowState = FormWindowState.Maximized;
					return;
				}
				if (wShowWindow != 6)
				{
					return;
				}
				this.WindowState = FormWindowState.Minimized;
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x000E1104 File Offset: 0x000E0104
		private void WmClose(ref Message m)
		{
			FormClosingEventArgs formClosingEventArgs = new FormClosingEventArgs(this.CloseReason, false);
			if (m.Msg != 22)
			{
				if (this.Modal)
				{
					if (this.dialogResult == DialogResult.None)
					{
						this.dialogResult = DialogResult.Cancel;
					}
					this.CalledClosing = false;
					formClosingEventArgs.Cancel = !this.CheckCloseDialog(true);
				}
				else
				{
					formClosingEventArgs.Cancel = !base.Validate(true);
					if (this.IsMdiContainer)
					{
						FormClosingEventArgs formClosingEventArgs2 = new FormClosingEventArgs(CloseReason.MdiFormClosing, formClosingEventArgs.Cancel);
						foreach (Form form in this.MdiChildren)
						{
							if (form.IsHandleCreated)
							{
								form.OnClosing(formClosingEventArgs2);
								form.OnFormClosing(formClosingEventArgs2);
								if (formClosingEventArgs2.Cancel)
								{
									formClosingEventArgs.Cancel = true;
									break;
								}
							}
						}
					}
					Form[] ownedForms = this.OwnedForms;
					int integer = base.Properties.GetInteger(Form.PropOwnedFormsCount);
					for (int j = integer - 1; j >= 0; j--)
					{
						FormClosingEventArgs formClosingEventArgs3 = new FormClosingEventArgs(CloseReason.FormOwnerClosing, formClosingEventArgs.Cancel);
						if (ownedForms[j] != null)
						{
							ownedForms[j].OnFormClosing(formClosingEventArgs3);
							if (formClosingEventArgs3.Cancel)
							{
								formClosingEventArgs.Cancel = true;
								break;
							}
						}
					}
					this.OnClosing(formClosingEventArgs);
					this.OnFormClosing(formClosingEventArgs);
				}
				if (m.Msg == 17)
				{
					m.Result = (IntPtr)(formClosingEventArgs.Cancel ? 0 : 1);
				}
				if (this.Modal)
				{
					return;
				}
			}
			else
			{
				formClosingEventArgs.Cancel = m.WParam == IntPtr.Zero;
			}
			if (m.Msg != 17 && !formClosingEventArgs.Cancel)
			{
				this.IsClosing = true;
				FormClosedEventArgs formClosedEventArgs;
				if (this.IsMdiContainer)
				{
					formClosedEventArgs = new FormClosedEventArgs(CloseReason.MdiFormClosing);
					foreach (Form form2 in this.MdiChildren)
					{
						if (form2.IsHandleCreated)
						{
							form2.OnClosed(formClosedEventArgs);
							form2.OnFormClosed(formClosedEventArgs);
						}
					}
				}
				Form[] ownedForms2 = this.OwnedForms;
				int integer2 = base.Properties.GetInteger(Form.PropOwnedFormsCount);
				for (int l = integer2 - 1; l >= 0; l--)
				{
					formClosedEventArgs = new FormClosedEventArgs(CloseReason.FormOwnerClosing);
					if (ownedForms2[l] != null)
					{
						ownedForms2[l].OnClosed(formClosedEventArgs);
						ownedForms2[l].OnFormClosed(formClosedEventArgs);
					}
				}
				formClosedEventArgs = new FormClosedEventArgs(this.CloseReason);
				this.OnClosed(formClosedEventArgs);
				this.OnFormClosed(formClosedEventArgs);
				base.Dispose();
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x000E1359 File Offset: 0x000E0359
		private void WmEnterMenuLoop(ref Message m)
		{
			this.OnMenuStart(EventArgs.Empty);
			base.WndProc(ref m);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x000E136D File Offset: 0x000E036D
		private void WmEraseBkgnd(ref Message m)
		{
			this.UpdateWindowState();
			base.WndProc(ref m);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x000E137C File Offset: 0x000E037C
		private void WmExitMenuLoop(ref Message m)
		{
			this.OnMenuComplete(EventArgs.Empty);
			base.WndProc(ref m);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x000E1390 File Offset: 0x000E0390
		private void WmGetMinMaxInfo(ref Message m)
		{
			Size size = ((this.AutoSize && this.formStateEx[Form.FormStateExInModalSizingLoop] == 1) ? LayoutUtils.UnionSizes(this.minAutoSize, this.MinimumSize) : this.MinimumSize);
			Size maximumSize = this.MaximumSize;
			Rectangle maximizedBounds = this.MaximizedBounds;
			if (!size.IsEmpty || !maximumSize.IsEmpty || !maximizedBounds.IsEmpty || this.IsRestrictedWindow)
			{
				this.WmGetMinMaxInfoHelper(ref m, size, maximumSize, maximizedBounds);
			}
			if (this.IsMdiChild)
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x000E141C File Offset: 0x000E041C
		private void WmGetMinMaxInfoHelper(ref Message m, Size minTrack, Size maxTrack, Rectangle maximizedBounds)
		{
			NativeMethods.MINMAXINFO minmaxinfo = (NativeMethods.MINMAXINFO)m.GetLParam(typeof(NativeMethods.MINMAXINFO));
			if (!minTrack.IsEmpty)
			{
				minmaxinfo.ptMinTrackSize.x = minTrack.Width;
				minmaxinfo.ptMinTrackSize.y = minTrack.Height;
				if (maxTrack.IsEmpty)
				{
					Size size = SystemInformation.VirtualScreen.Size;
					if (minTrack.Height > size.Height)
					{
						minmaxinfo.ptMaxTrackSize.y = int.MaxValue;
					}
					if (minTrack.Width > size.Width)
					{
						minmaxinfo.ptMaxTrackSize.x = int.MaxValue;
					}
				}
			}
			if (!maxTrack.IsEmpty)
			{
				Size minWindowTrackSize = SystemInformation.MinWindowTrackSize;
				minmaxinfo.ptMaxTrackSize.x = Math.Max(maxTrack.Width, minWindowTrackSize.Width);
				minmaxinfo.ptMaxTrackSize.y = Math.Max(maxTrack.Height, minWindowTrackSize.Height);
			}
			if (!maximizedBounds.IsEmpty && !this.IsRestrictedWindow)
			{
				minmaxinfo.ptMaxPosition.x = maximizedBounds.X;
				minmaxinfo.ptMaxPosition.y = maximizedBounds.Y;
				minmaxinfo.ptMaxSize.x = maximizedBounds.Width;
				minmaxinfo.ptMaxSize.y = maximizedBounds.Height;
			}
			if (this.IsRestrictedWindow)
			{
				minmaxinfo.ptMinTrackSize.x = Math.Max(minmaxinfo.ptMinTrackSize.x, 100);
				minmaxinfo.ptMinTrackSize.y = Math.Max(minmaxinfo.ptMinTrackSize.y, SystemInformation.CaptionButtonSize.Height * 3);
			}
			Marshal.StructureToPtr(minmaxinfo, m.LParam, false);
			m.Result = IntPtr.Zero;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x000E15D0 File Offset: 0x000E05D0
		private void WmInitMenuPopup(ref Message m)
		{
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu != null && mainMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x000E160C File Offset: 0x000E060C
		private void WmMenuChar(ref Message m)
		{
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			if (mainMenu == null)
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form != null && form.Menu != null)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(form, form.Handle), 274, new IntPtr(61696), m.WParam);
					m.Result = (IntPtr)NativeMethods.Util.MAKELONG(0, 1);
					return;
				}
			}
			if (mainMenu != null)
			{
				mainMenu.WmMenuChar(ref m);
				if (m.Result != IntPtr.Zero)
				{
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x000E16B4 File Offset: 0x000E06B4
		private void WmMdiActivate(ref Message m)
		{
			base.WndProc(ref m);
			Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
			if (form != null)
			{
				if (base.Handle == m.WParam)
				{
					form.DeactivateMdiChild();
					return;
				}
				if (base.Handle == m.LParam)
				{
					form.ActivateMdiChildInternal(this);
				}
			}
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x000E1718 File Offset: 0x000E0718
		private void WmNcButtonDown(ref Message m)
		{
			if (this.IsMdiChild)
			{
				Form form = (Form)base.Properties.GetObject(Form.PropFormMdiParent);
				if (form.ActiveMdiChildInternal == this && base.ActiveControl != null && !base.ActiveControl.ContainsFocus)
				{
					base.InnerMostActiveContainerControl.FocusActiveControlInternal();
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x000E1774 File Offset: 0x000E0774
		private void WmNCDestroy(ref Message m)
		{
			MainMenu menu = this.Menu;
			MainMenu mainMenu = (MainMenu)base.Properties.GetObject(Form.PropDummyMenu);
			MainMenu mainMenu2 = (MainMenu)base.Properties.GetObject(Form.PropCurMenu);
			MainMenu mainMenu3 = (MainMenu)base.Properties.GetObject(Form.PropMergedMenu);
			if (menu != null)
			{
				menu.ClearHandles();
			}
			if (mainMenu2 != null)
			{
				mainMenu2.ClearHandles();
			}
			if (mainMenu3 != null)
			{
				mainMenu3.ClearHandles();
			}
			if (mainMenu != null)
			{
				mainMenu.ClearHandles();
			}
			base.WndProc(ref m);
			if (this.ownerWindow != null)
			{
				this.ownerWindow.DestroyHandle();
				this.ownerWindow = null;
			}
			if (this.Modal && this.dialogResult == DialogResult.None)
			{
				this.DialogResult = DialogResult.Cancel;
			}
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x000E1828 File Offset: 0x000E0828
		private void WmNCHitTest(ref Message m)
		{
			if (this.formState[Form.FormStateRenderSizeGrip] != 0)
			{
				int num = NativeMethods.Util.LOWORD(m.LParam);
				int num2 = NativeMethods.Util.HIWORD(m.LParam);
				NativeMethods.POINT point = new NativeMethods.POINT(num, num2);
				UnsafeNativeMethods.ScreenToClient(new HandleRef(this, base.Handle), point);
				Size clientSize = this.ClientSize;
				if (point.x >= clientSize.Width - 16 && point.y >= clientSize.Height - 16 && clientSize.Height >= 16)
				{
					m.Result = (base.IsMirrored ? ((IntPtr)16) : ((IntPtr)17));
					return;
				}
			}
			base.WndProc(ref m);
			if (this.AutoSizeMode == AutoSizeMode.GrowAndShrink)
			{
				int num3 = (int)m.Result;
				if (num3 >= 10 && num3 <= 17)
				{
					m.Result = (IntPtr)18;
				}
			}
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x000E1907 File Offset: 0x000E0907
		private void WmShowWindow(ref Message m)
		{
			this.formState[Form.FormStateSWCalled] = 1;
			base.WndProc(ref m);
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x000E1924 File Offset: 0x000E0924
		private void WmSysCommand(ref Message m)
		{
			bool flag = true;
			int num = NativeMethods.Util.LOWORD(m.WParam) & 65520;
			int num2 = num;
			if (num2 <= 61456)
			{
				if (num2 == 61440 || num2 == 61456)
				{
					this.formStateEx[Form.FormStateExInModalSizingLoop] = 1;
				}
			}
			else if (num2 != 61536)
			{
				if (num2 != 61696)
				{
					if (num2 == 61824)
					{
						CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
						this.OnHelpButtonClicked(cancelEventArgs);
						if (cancelEventArgs.Cancel)
						{
							flag = false;
						}
					}
				}
				else if (this.IsMdiChild && !this.ControlBox)
				{
					flag = false;
				}
			}
			else
			{
				this.CloseReason = CloseReason.UserClosing;
				if (this.IsMdiChild && !this.ControlBox)
				{
					flag = false;
				}
			}
			if (Command.DispatchID(NativeMethods.Util.LOWORD(m.WParam)))
			{
				flag = false;
			}
			if (flag)
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x000E19F4 File Offset: 0x000E09F4
		private void WmSize(ref Message m)
		{
			if (this.ctlClient == null)
			{
				base.WndProc(ref m);
				if (this.MdiControlStrip == null && this.MdiParentInternal != null && this.MdiParentInternal.ActiveMdiChildInternal == this)
				{
					int num = m.WParam.ToInt32();
					this.MdiParentInternal.UpdateMdiControlStrip(num == 2);
				}
			}
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x000E1A4C File Offset: 0x000E0A4C
		private void WmUnInitMenuPopup(ref Message m)
		{
			if (this.Menu != null)
			{
				this.Menu.OnCollapse(EventArgs.Empty);
			}
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x000E1A66 File Offset: 0x000E0A66
		private void WmWindowPosChanged(ref Message m)
		{
			this.UpdateWindowState();
			base.WndProc(ref m);
			this.RestoreWindowBoundsIfNecessary();
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x000E1A7C File Offset: 0x000E0A7C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 164)
			{
				if (msg <= 36)
				{
					if (msg <= 6)
					{
						if (msg == 1)
						{
							this.WmCreate(ref m);
							return;
						}
						switch (msg)
						{
						case 5:
							this.WmSize(ref m);
							return;
						case 6:
							this.WmActivate(ref m);
							return;
						default:
							goto IL_0278;
						}
					}
					else
					{
						switch (msg)
						{
						case 16:
							if (this.CloseReason == CloseReason.None)
							{
								this.CloseReason = CloseReason.TaskManagerClosing;
							}
							this.WmClose(ref m);
							return;
						case 17:
						case 22:
							this.CloseReason = CloseReason.WindowsShutDown;
							this.WmClose(ref m);
							return;
						case 18:
						case 19:
						case 21:
						case 23:
							goto IL_0278;
						case 20:
							this.WmEraseBkgnd(ref m);
							return;
						case 24:
							this.WmShowWindow(ref m);
							return;
						default:
							if (msg != 36)
							{
								goto IL_0278;
							}
							this.WmGetMinMaxInfo(ref m);
							return;
						}
					}
				}
				else if (msg <= 134)
				{
					if (msg == 71)
					{
						this.WmWindowPosChanged(ref m);
						return;
					}
					switch (msg)
					{
					case 130:
						this.WmNCDestroy(ref m);
						return;
					case 131:
					case 133:
						goto IL_0278;
					case 132:
						this.WmNCHitTest(ref m);
						return;
					case 134:
						if (this.IsRestrictedWindow)
						{
							base.BeginInvoke(new MethodInvoker(this.RestrictedProcessNcActivate));
						}
						base.WndProc(ref m);
						return;
					default:
						goto IL_0278;
					}
				}
				else if (msg != 161 && msg != 164)
				{
					goto IL_0278;
				}
			}
			else if (msg <= 279)
			{
				if (msg <= 171)
				{
					if (msg != 167 && msg != 171)
					{
						goto IL_0278;
					}
				}
				else
				{
					if (msg == 274)
					{
						this.WmSysCommand(ref m);
						return;
					}
					if (msg != 279)
					{
						goto IL_0278;
					}
					this.WmInitMenuPopup(ref m);
					return;
				}
			}
			else if (msg <= 293)
			{
				if (msg == 288)
				{
					this.WmMenuChar(ref m);
					return;
				}
				if (msg != 293)
				{
					goto IL_0278;
				}
				this.WmUnInitMenuPopup(ref m);
				return;
			}
			else
			{
				switch (msg)
				{
				case 529:
					this.WmEnterMenuLoop(ref m);
					return;
				case 530:
					this.WmExitMenuLoop(ref m);
					return;
				case 531:
				case 532:
					goto IL_0278;
				case 533:
					base.WndProc(ref m);
					if (base.CaptureInternal && Control.MouseButtons == MouseButtons.None)
					{
						base.CaptureInternal = false;
						return;
					}
					return;
				default:
					if (msg == 546)
					{
						this.WmMdiActivate(ref m);
						return;
					}
					switch (msg)
					{
					case 561:
						this.WmEnterSizeMove(ref m);
						this.DefWndProc(ref m);
						return;
					case 562:
						this.WmExitSizeMove(ref m);
						this.DefWndProc(ref m);
						return;
					default:
						goto IL_0278;
					}
					break;
				}
			}
			this.WmNcButtonDown(ref m);
			return;
			IL_0278:
			base.WndProc(ref m);
		}

		// Token: 0x04001E2D RID: 7725
		private const int SizeGripSize = 16;

		// Token: 0x04001E2E RID: 7726
		private static readonly object EVENT_ACTIVATED = new object();

		// Token: 0x04001E2F RID: 7727
		private static readonly object EVENT_CLOSING = new object();

		// Token: 0x04001E30 RID: 7728
		private static readonly object EVENT_CLOSED = new object();

		// Token: 0x04001E31 RID: 7729
		private static readonly object EVENT_FORMCLOSING = new object();

		// Token: 0x04001E32 RID: 7730
		private static readonly object EVENT_FORMCLOSED = new object();

		// Token: 0x04001E33 RID: 7731
		private static readonly object EVENT_DEACTIVATE = new object();

		// Token: 0x04001E34 RID: 7732
		private static readonly object EVENT_LOAD = new object();

		// Token: 0x04001E35 RID: 7733
		private static readonly object EVENT_MDI_CHILD_ACTIVATE = new object();

		// Token: 0x04001E36 RID: 7734
		private static readonly object EVENT_INPUTLANGCHANGE = new object();

		// Token: 0x04001E37 RID: 7735
		private static readonly object EVENT_INPUTLANGCHANGEREQUEST = new object();

		// Token: 0x04001E38 RID: 7736
		private static readonly object EVENT_MENUSTART = new object();

		// Token: 0x04001E39 RID: 7737
		private static readonly object EVENT_MENUCOMPLETE = new object();

		// Token: 0x04001E3A RID: 7738
		private static readonly object EVENT_MAXIMUMSIZECHANGED = new object();

		// Token: 0x04001E3B RID: 7739
		private static readonly object EVENT_MINIMUMSIZECHANGED = new object();

		// Token: 0x04001E3C RID: 7740
		private static readonly object EVENT_HELPBUTTONCLICKED = new object();

		// Token: 0x04001E3D RID: 7741
		private static readonly object EVENT_SHOWN = new object();

		// Token: 0x04001E3E RID: 7742
		private static readonly object EVENT_RESIZEBEGIN = new object();

		// Token: 0x04001E3F RID: 7743
		private static readonly object EVENT_RESIZEEND = new object();

		// Token: 0x04001E40 RID: 7744
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x04001E41 RID: 7745
		private static readonly BitVector32.Section FormStateAllowTransparency = BitVector32.CreateSection(1);

		// Token: 0x04001E42 RID: 7746
		private static readonly BitVector32.Section FormStateBorderStyle = BitVector32.CreateSection(6, Form.FormStateAllowTransparency);

		// Token: 0x04001E43 RID: 7747
		private static readonly BitVector32.Section FormStateTaskBar = BitVector32.CreateSection(1, Form.FormStateBorderStyle);

		// Token: 0x04001E44 RID: 7748
		private static readonly BitVector32.Section FormStateControlBox = BitVector32.CreateSection(1, Form.FormStateTaskBar);

		// Token: 0x04001E45 RID: 7749
		private static readonly BitVector32.Section FormStateKeyPreview = BitVector32.CreateSection(1, Form.FormStateControlBox);

		// Token: 0x04001E46 RID: 7750
		private static readonly BitVector32.Section FormStateLayered = BitVector32.CreateSection(1, Form.FormStateKeyPreview);

		// Token: 0x04001E47 RID: 7751
		private static readonly BitVector32.Section FormStateMaximizeBox = BitVector32.CreateSection(1, Form.FormStateLayered);

		// Token: 0x04001E48 RID: 7752
		private static readonly BitVector32.Section FormStateMinimizeBox = BitVector32.CreateSection(1, Form.FormStateMaximizeBox);

		// Token: 0x04001E49 RID: 7753
		private static readonly BitVector32.Section FormStateHelpButton = BitVector32.CreateSection(1, Form.FormStateMinimizeBox);

		// Token: 0x04001E4A RID: 7754
		private static readonly BitVector32.Section FormStateStartPos = BitVector32.CreateSection(4, Form.FormStateHelpButton);

		// Token: 0x04001E4B RID: 7755
		private static readonly BitVector32.Section FormStateWindowState = BitVector32.CreateSection(2, Form.FormStateStartPos);

		// Token: 0x04001E4C RID: 7756
		private static readonly BitVector32.Section FormStateShowWindowOnCreate = BitVector32.CreateSection(1, Form.FormStateWindowState);

		// Token: 0x04001E4D RID: 7757
		private static readonly BitVector32.Section FormStateAutoScaling = BitVector32.CreateSection(1, Form.FormStateShowWindowOnCreate);

		// Token: 0x04001E4E RID: 7758
		private static readonly BitVector32.Section FormStateSetClientSize = BitVector32.CreateSection(1, Form.FormStateAutoScaling);

		// Token: 0x04001E4F RID: 7759
		private static readonly BitVector32.Section FormStateTopMost = BitVector32.CreateSection(1, Form.FormStateSetClientSize);

		// Token: 0x04001E50 RID: 7760
		private static readonly BitVector32.Section FormStateSWCalled = BitVector32.CreateSection(1, Form.FormStateTopMost);

		// Token: 0x04001E51 RID: 7761
		private static readonly BitVector32.Section FormStateMdiChildMax = BitVector32.CreateSection(1, Form.FormStateSWCalled);

		// Token: 0x04001E52 RID: 7762
		private static readonly BitVector32.Section FormStateRenderSizeGrip = BitVector32.CreateSection(1, Form.FormStateMdiChildMax);

		// Token: 0x04001E53 RID: 7763
		private static readonly BitVector32.Section FormStateSizeGripStyle = BitVector32.CreateSection(2, Form.FormStateRenderSizeGrip);

		// Token: 0x04001E54 RID: 7764
		private static readonly BitVector32.Section FormStateIsRestrictedWindow = BitVector32.CreateSection(1, Form.FormStateSizeGripStyle);

		// Token: 0x04001E55 RID: 7765
		private static readonly BitVector32.Section FormStateIsRestrictedWindowChecked = BitVector32.CreateSection(1, Form.FormStateIsRestrictedWindow);

		// Token: 0x04001E56 RID: 7766
		private static readonly BitVector32.Section FormStateIsWindowActivated = BitVector32.CreateSection(1, Form.FormStateIsRestrictedWindowChecked);

		// Token: 0x04001E57 RID: 7767
		private static readonly BitVector32.Section FormStateIsTextEmpty = BitVector32.CreateSection(1, Form.FormStateIsWindowActivated);

		// Token: 0x04001E58 RID: 7768
		private static readonly BitVector32.Section FormStateIsActive = BitVector32.CreateSection(1, Form.FormStateIsTextEmpty);

		// Token: 0x04001E59 RID: 7769
		private static readonly BitVector32.Section FormStateIconSet = BitVector32.CreateSection(1, Form.FormStateIsActive);

		// Token: 0x04001E5A RID: 7770
		private static readonly BitVector32.Section FormStateExCalledClosing = BitVector32.CreateSection(1);

		// Token: 0x04001E5B RID: 7771
		private static readonly BitVector32.Section FormStateExUpdateMenuHandlesSuspendCount = BitVector32.CreateSection(8, Form.FormStateExCalledClosing);

		// Token: 0x04001E5C RID: 7772
		private static readonly BitVector32.Section FormStateExUpdateMenuHandlesDeferred = BitVector32.CreateSection(1, Form.FormStateExUpdateMenuHandlesSuspendCount);

		// Token: 0x04001E5D RID: 7773
		private static readonly BitVector32.Section FormStateExUseMdiChildProc = BitVector32.CreateSection(1, Form.FormStateExUpdateMenuHandlesDeferred);

		// Token: 0x04001E5E RID: 7774
		private static readonly BitVector32.Section FormStateExCalledOnLoad = BitVector32.CreateSection(1, Form.FormStateExUseMdiChildProc);

		// Token: 0x04001E5F RID: 7775
		private static readonly BitVector32.Section FormStateExCalledMakeVisible = BitVector32.CreateSection(1, Form.FormStateExCalledOnLoad);

		// Token: 0x04001E60 RID: 7776
		private static readonly BitVector32.Section FormStateExCalledCreateControl = BitVector32.CreateSection(1, Form.FormStateExCalledMakeVisible);

		// Token: 0x04001E61 RID: 7777
		private static readonly BitVector32.Section FormStateExAutoSize = BitVector32.CreateSection(1, Form.FormStateExCalledCreateControl);

		// Token: 0x04001E62 RID: 7778
		private static readonly BitVector32.Section FormStateExInUpdateMdiControlStrip = BitVector32.CreateSection(1, Form.FormStateExAutoSize);

		// Token: 0x04001E63 RID: 7779
		private static readonly BitVector32.Section FormStateExShowIcon = BitVector32.CreateSection(1, Form.FormStateExInUpdateMdiControlStrip);

		// Token: 0x04001E64 RID: 7780
		private static readonly BitVector32.Section FormStateExMnemonicProcessed = BitVector32.CreateSection(1, Form.FormStateExShowIcon);

		// Token: 0x04001E65 RID: 7781
		private static readonly BitVector32.Section FormStateExInScale = BitVector32.CreateSection(1, Form.FormStateExMnemonicProcessed);

		// Token: 0x04001E66 RID: 7782
		private static readonly BitVector32.Section FormStateExInModalSizingLoop = BitVector32.CreateSection(1, Form.FormStateExInScale);

		// Token: 0x04001E67 RID: 7783
		private static readonly BitVector32.Section FormStateExSettingAutoScale = BitVector32.CreateSection(1, Form.FormStateExInModalSizingLoop);

		// Token: 0x04001E68 RID: 7784
		private static readonly BitVector32.Section FormStateExWindowBoundsWidthIsClientSize = BitVector32.CreateSection(1, Form.FormStateExSettingAutoScale);

		// Token: 0x04001E69 RID: 7785
		private static readonly BitVector32.Section FormStateExWindowBoundsHeightIsClientSize = BitVector32.CreateSection(1, Form.FormStateExWindowBoundsWidthIsClientSize);

		// Token: 0x04001E6A RID: 7786
		private static readonly BitVector32.Section FormStateExWindowClosing = BitVector32.CreateSection(1, Form.FormStateExWindowBoundsHeightIsClientSize);

		// Token: 0x04001E6B RID: 7787
		private static Icon defaultIcon = null;

		// Token: 0x04001E6C RID: 7788
		private static Icon defaultRestrictedIcon = null;

		// Token: 0x04001E6D RID: 7789
		private static Padding FormPadding = new Padding(9);

		// Token: 0x04001E6E RID: 7790
		private static object internalSyncObject = new object();

		// Token: 0x04001E6F RID: 7791
		private static readonly int PropAcceptButton = PropertyStore.CreateKey();

		// Token: 0x04001E70 RID: 7792
		private static readonly int PropCancelButton = PropertyStore.CreateKey();

		// Token: 0x04001E71 RID: 7793
		private static readonly int PropDefaultButton = PropertyStore.CreateKey();

		// Token: 0x04001E72 RID: 7794
		private static readonly int PropDialogOwner = PropertyStore.CreateKey();

		// Token: 0x04001E73 RID: 7795
		private static readonly int PropMainMenu = PropertyStore.CreateKey();

		// Token: 0x04001E74 RID: 7796
		private static readonly int PropDummyMenu = PropertyStore.CreateKey();

		// Token: 0x04001E75 RID: 7797
		private static readonly int PropCurMenu = PropertyStore.CreateKey();

		// Token: 0x04001E76 RID: 7798
		private static readonly int PropMergedMenu = PropertyStore.CreateKey();

		// Token: 0x04001E77 RID: 7799
		private static readonly int PropOwner = PropertyStore.CreateKey();

		// Token: 0x04001E78 RID: 7800
		private static readonly int PropOwnedForms = PropertyStore.CreateKey();

		// Token: 0x04001E79 RID: 7801
		private static readonly int PropMaximizedBounds = PropertyStore.CreateKey();

		// Token: 0x04001E7A RID: 7802
		private static readonly int PropOwnedFormsCount = PropertyStore.CreateKey();

		// Token: 0x04001E7B RID: 7803
		private static readonly int PropMinTrackSizeWidth = PropertyStore.CreateKey();

		// Token: 0x04001E7C RID: 7804
		private static readonly int PropMinTrackSizeHeight = PropertyStore.CreateKey();

		// Token: 0x04001E7D RID: 7805
		private static readonly int PropMaxTrackSizeWidth = PropertyStore.CreateKey();

		// Token: 0x04001E7E RID: 7806
		private static readonly int PropMaxTrackSizeHeight = PropertyStore.CreateKey();

		// Token: 0x04001E7F RID: 7807
		private static readonly int PropFormMdiParent = PropertyStore.CreateKey();

		// Token: 0x04001E80 RID: 7808
		private static readonly int PropActiveMdiChild = PropertyStore.CreateKey();

		// Token: 0x04001E81 RID: 7809
		private static readonly int PropFormerlyActiveMdiChild = PropertyStore.CreateKey();

		// Token: 0x04001E82 RID: 7810
		private static readonly int PropMdiChildFocusable = PropertyStore.CreateKey();

		// Token: 0x04001E83 RID: 7811
		private static readonly int PropMainMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04001E84 RID: 7812
		private static readonly int PropMdiWindowListStrip = PropertyStore.CreateKey();

		// Token: 0x04001E85 RID: 7813
		private static readonly int PropMdiControlStrip = PropertyStore.CreateKey();

		// Token: 0x04001E86 RID: 7814
		private static readonly int PropSecurityTip = PropertyStore.CreateKey();

		// Token: 0x04001E87 RID: 7815
		private static readonly int PropOpacity = PropertyStore.CreateKey();

		// Token: 0x04001E88 RID: 7816
		private static readonly int PropTransparencyKey = PropertyStore.CreateKey();

		// Token: 0x04001E89 RID: 7817
		private BitVector32 formState = new BitVector32(135992);

		// Token: 0x04001E8A RID: 7818
		private BitVector32 formStateEx = default(BitVector32);

		// Token: 0x04001E8B RID: 7819
		private Icon icon;

		// Token: 0x04001E8C RID: 7820
		private Icon smallIcon;

		// Token: 0x04001E8D RID: 7821
		private Size autoScaleBaseSize = Size.Empty;

		// Token: 0x04001E8E RID: 7822
		private Size minAutoSize = Size.Empty;

		// Token: 0x04001E8F RID: 7823
		private Rectangle restoredWindowBounds = new Rectangle(-1, -1, -1, -1);

		// Token: 0x04001E90 RID: 7824
		private BoundsSpecified restoredWindowBoundsSpecified;

		// Token: 0x04001E91 RID: 7825
		private DialogResult dialogResult;

		// Token: 0x04001E92 RID: 7826
		private MdiClient ctlClient;

		// Token: 0x04001E93 RID: 7827
		private NativeWindow ownerWindow;

		// Token: 0x04001E94 RID: 7828
		private string userWindowText;

		// Token: 0x04001E95 RID: 7829
		private string securityZone;

		// Token: 0x04001E96 RID: 7830
		private string securitySite;

		// Token: 0x04001E97 RID: 7831
		private bool rightToLeftLayout;

		// Token: 0x04001E98 RID: 7832
		private Rectangle restoreBounds = new Rectangle(-1, -1, -1, -1);

		// Token: 0x04001E99 RID: 7833
		private CloseReason closeReason;

		// Token: 0x04001E9A RID: 7834
		private VisualStyleRenderer sizeGripRenderer;

		// Token: 0x04001E9B RID: 7835
		private static readonly object EVENT_MAXIMIZEDBOUNDSCHANGED = new object();

		// Token: 0x02000408 RID: 1032
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			// Token: 0x06003E1D RID: 15901 RVA: 0x000E2199 File Offset: 0x000E1199
			public ControlCollection(Form owner)
				: base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06003E1E RID: 15902 RVA: 0x000E21AC File Offset: 0x000E11AC
			public override void Add(Control value)
			{
				if (value is MdiClient && this.owner.ctlClient == null)
				{
					if (!this.owner.TopLevel && !this.owner.DesignMode)
					{
						throw new ArgumentException(SR.GetString("MDIContainerMustBeTopLevel"), "value");
					}
					this.owner.AutoScroll = false;
					if (this.owner.IsMdiChild)
					{
						throw new ArgumentException(SR.GetString("FormMDIParentAndChild"), "value");
					}
					this.owner.ctlClient = (MdiClient)value;
				}
				if (value is Form && ((Form)value).MdiParentInternal != null)
				{
					throw new ArgumentException(SR.GetString("FormMDIParentCannotAdd"), "value");
				}
				base.Add(value);
				if (this.owner.ctlClient != null)
				{
					this.owner.ctlClient.SendToBack();
				}
			}

			// Token: 0x06003E1F RID: 15903 RVA: 0x000E228A File Offset: 0x000E128A
			public override void Remove(Control value)
			{
				if (value == this.owner.ctlClient)
				{
					this.owner.ctlClient = null;
				}
				base.Remove(value);
			}

			// Token: 0x04001E9C RID: 7836
			private Form owner;
		}

		// Token: 0x02000409 RID: 1033
		private class EnumThreadWindowsCallback
		{
			// Token: 0x06003E20 RID: 15904 RVA: 0x000E22AD File Offset: 0x000E12AD
			internal EnumThreadWindowsCallback()
			{
			}

			// Token: 0x06003E21 RID: 15905 RVA: 0x000E22B8 File Offset: 0x000E12B8
			internal bool Callback(IntPtr hWnd, IntPtr lParam)
			{
				HandleRef handleRef = new HandleRef(null, hWnd);
				IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(handleRef, -8);
				if (windowLong == lParam)
				{
					if (this.ownedWindows == null)
					{
						this.ownedWindows = new List<HandleRef>();
					}
					this.ownedWindows.Add(handleRef);
				}
				return true;
			}

			// Token: 0x06003E22 RID: 15906 RVA: 0x000E2300 File Offset: 0x000E1300
			internal void ResetOwners()
			{
				if (this.ownedWindows != null)
				{
					foreach (HandleRef handleRef in this.ownedWindows)
					{
						UnsafeNativeMethods.SetWindowLong(handleRef, -8, NativeMethods.NullHandleRef);
					}
				}
			}

			// Token: 0x06003E23 RID: 15907 RVA: 0x000E2364 File Offset: 0x000E1364
			internal void SetOwners(HandleRef hRefOwner)
			{
				if (this.ownedWindows != null)
				{
					foreach (HandleRef handleRef in this.ownedWindows)
					{
						UnsafeNativeMethods.SetWindowLong(handleRef, -8, hRefOwner);
					}
				}
			}

			// Token: 0x04001E9D RID: 7837
			private List<HandleRef> ownedWindows;
		}

		// Token: 0x0200040A RID: 1034
		private class SecurityToolTip : IDisposable
		{
			// Token: 0x06003E24 RID: 15908 RVA: 0x000E23C4 File Offset: 0x000E13C4
			internal SecurityToolTip(Form owner)
			{
				this.owner = owner;
				this.SetupText();
				this.window = new Form.SecurityToolTip.ToolTipNativeWindow(this);
				this.SetupToolTip();
				owner.LocationChanged += this.FormLocationChanged;
				owner.HandleCreated += this.FormHandleCreated;
			}

			// Token: 0x17000BE7 RID: 3047
			// (get) Token: 0x06003E25 RID: 15909 RVA: 0x000E2424 File Offset: 0x000E1424
			private CreateParams CreateParams
			{
				get
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
					CreateParams createParams = new CreateParams();
					createParams.Parent = this.owner.Handle;
					createParams.ClassName = "tooltips_class32";
					createParams.Style |= 65;
					createParams.ExStyle = 0;
					createParams.Caption = null;
					return createParams;
				}
			}

			// Token: 0x17000BE8 RID: 3048
			// (get) Token: 0x06003E26 RID: 15910 RVA: 0x000E2485 File Offset: 0x000E1485
			internal bool Modal
			{
				get
				{
					return this.first;
				}
			}

			// Token: 0x06003E27 RID: 15911 RVA: 0x000E2490 File Offset: 0x000E1490
			public void Dispose()
			{
				if (this.owner != null)
				{
					this.owner.LocationChanged -= this.FormLocationChanged;
				}
				if (this.window.Handle != IntPtr.Zero)
				{
					this.window.DestroyHandle();
					this.window = null;
				}
			}

			// Token: 0x06003E28 RID: 15912 RVA: 0x000E24E8 File Offset: 0x000E14E8
			private NativeMethods.TOOLINFO_T GetTOOLINFO()
			{
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				toolinfo_T.uFlags |= 16;
				toolinfo_T.lpszText = this.toolTipText;
				if (this.owner.RightToLeft == RightToLeft.Yes)
				{
					toolinfo_T.uFlags |= 4;
				}
				if (!this.first)
				{
					toolinfo_T.uFlags |= 256;
					toolinfo_T.hwnd = this.owner.Handle;
					Size captionButtonSize = SystemInformation.CaptionButtonSize;
					Rectangle rectangle = new Rectangle(this.owner.Left, this.owner.Top, captionButtonSize.Width, SystemInformation.CaptionHeight);
					rectangle = this.owner.RectangleToClient(rectangle);
					rectangle.Width -= rectangle.X;
					rectangle.Y++;
					toolinfo_T.rect = NativeMethods.RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
					toolinfo_T.uId = IntPtr.Zero;
				}
				else
				{
					toolinfo_T.uFlags |= 33;
					toolinfo_T.hwnd = IntPtr.Zero;
					toolinfo_T.uId = this.owner.Handle;
				}
				return toolinfo_T;
			}

			// Token: 0x06003E29 RID: 15913 RVA: 0x000E2638 File Offset: 0x000E1638
			private void SetupText()
			{
				this.owner.EnsureSecurityInformation();
				string @string = SR.GetString("SecurityToolTipMainText");
				string string2 = SR.GetString("SecurityToolTipSourceInformation", new object[] { this.owner.securitySite });
				this.toolTipText = SR.GetString("SecurityToolTipTextFormat", new object[] { @string, string2 });
			}

			// Token: 0x06003E2A RID: 15914 RVA: 0x000E269C File Offset: 0x000E169C
			private void SetupToolTip()
			{
				this.window.CreateHandle(this.CreateParams);
				SafeNativeMethods.SetWindowPos(new HandleRef(this.window, this.window.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1048, 0, this.owner.Width);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_SETTITLE, 2, SR.GetString("SecurityToolTipCaption"));
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1025, 1, 0);
				this.Show();
			}

			// Token: 0x06003E2B RID: 15915 RVA: 0x000E2790 File Offset: 0x000E1790
			private void RecreateHandle()
			{
				if (this.window != null)
				{
					if (this.window.Handle != IntPtr.Zero)
					{
						this.window.DestroyHandle();
					}
					this.SetupToolTip();
				}
			}

			// Token: 0x06003E2C RID: 15916 RVA: 0x000E27C2 File Offset: 0x000E17C2
			private void FormHandleCreated(object sender, EventArgs e)
			{
				this.RecreateHandle();
			}

			// Token: 0x06003E2D RID: 15917 RVA: 0x000E27CC File Offset: 0x000E17CC
			private void FormLocationChanged(object sender, EventArgs e)
			{
				if (this.window == null || !this.first)
				{
					this.Pop(true);
					return;
				}
				Size captionButtonSize = SystemInformation.CaptionButtonSize;
				if (this.owner.WindowState == FormWindowState.Minimized)
				{
					this.Pop(true);
					return;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1042, 0, NativeMethods.Util.MAKELONG(this.owner.Left + captionButtonSize.Width / 2, this.owner.Top + SystemInformation.CaptionHeight));
			}

			// Token: 0x06003E2E RID: 15918 RVA: 0x000E285C File Offset: 0x000E185C
			internal void Pop(bool noLongerFirst)
			{
				if (noLongerFirst)
				{
					this.first = false;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1041, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetTOOLINFO());
				UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO());
			}

			// Token: 0x06003E2F RID: 15919 RVA: 0x000E28EC File Offset: 0x000E18EC
			internal void Show()
			{
				if (this.first)
				{
					Size captionButtonSize = SystemInformation.CaptionButtonSize;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1042, 0, NativeMethods.Util.MAKELONG(this.owner.Left + captionButtonSize.Width / 2, this.owner.Top + SystemInformation.CaptionHeight));
					UnsafeNativeMethods.SendMessage(new HandleRef(this.window, this.window.Handle), 1041, 1, this.GetTOOLINFO());
				}
			}

			// Token: 0x06003E30 RID: 15920 RVA: 0x000E297C File Offset: 0x000E197C
			private void WndProc(ref Message msg)
			{
				if (this.first && (msg.Msg == 513 || msg.Msg == 516 || msg.Msg == 519 || msg.Msg == 523))
				{
					this.Pop(true);
				}
				this.window.DefWndProc(ref msg);
			}

			// Token: 0x04001E9E RID: 7838
			private Form owner;

			// Token: 0x04001E9F RID: 7839
			private string toolTipText;

			// Token: 0x04001EA0 RID: 7840
			private bool first = true;

			// Token: 0x04001EA1 RID: 7841
			private Form.SecurityToolTip.ToolTipNativeWindow window;

			// Token: 0x0200040B RID: 1035
			private sealed class ToolTipNativeWindow : NativeWindow
			{
				// Token: 0x06003E31 RID: 15921 RVA: 0x000E29D8 File Offset: 0x000E19D8
				internal ToolTipNativeWindow(Form.SecurityToolTip control)
				{
					this.control = control;
				}

				// Token: 0x06003E32 RID: 15922 RVA: 0x000E29E7 File Offset: 0x000E19E7
				protected override void WndProc(ref Message m)
				{
					if (this.control != null)
					{
						this.control.WndProc(ref m);
					}
				}

				// Token: 0x04001EA2 RID: 7842
				private Form.SecurityToolTip control;
			}
		}
	}
}

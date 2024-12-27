using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000210 RID: 528
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class ContainerControl : ScrollableControl, IContainerControl
	{
		// Token: 0x06001833 RID: 6195 RVA: 0x00029F54 File Offset: 0x00028F54
		public ContainerControl()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			base.SetState2(2048, true);
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00029FAF File Offset: 0x00028FAF
		// (set) Token: 0x06001835 RID: 6197 RVA: 0x00029FB8 File Offset: 0x00028FB8
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SizeF AutoScaleDimensions
		{
			get
			{
				return this.autoScaleDimensions;
			}
			set
			{
				if (value.Width < 0f || value.Height < 0f)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("ContainerControlInvalidAutoScaleDimensions"), "value");
				}
				this.autoScaleDimensions = value;
				if (!this.autoScaleDimensions.IsEmpty)
				{
					this.LayoutScalingNeeded();
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x0002A010 File Offset: 0x00029010
		protected SizeF AutoScaleFactor
		{
			get
			{
				SizeF sizeF = this.CurrentAutoScaleDimensions;
				SizeF sizeF2 = this.AutoScaleDimensions;
				if (sizeF2.IsEmpty)
				{
					return new SizeF(1f, 1f);
				}
				return new SizeF(sizeF.Width / sizeF2.Width, sizeF.Height / sizeF2.Height);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x0002A067 File Offset: 0x00029067
		// (set) Token: 0x06001838 RID: 6200 RVA: 0x0002A070 File Offset: 0x00029070
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ContainerControlAutoScaleModeDescr")]
		public AutoScaleMode AutoScaleMode
		{
			get
			{
				return this.autoScaleMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoScaleMode));
				}
				bool flag = false;
				if (value != this.autoScaleMode)
				{
					if (this.autoScaleMode != AutoScaleMode.Inherit)
					{
						this.autoScaleDimensions = SizeF.Empty;
					}
					this.currentAutoScaleDimensions = SizeF.Empty;
					this.autoScaleMode = value;
					flag = true;
				}
				this.OnAutoScaleModeChanged();
				if (flag)
				{
					this.LayoutScalingNeeded();
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x0002A0E5 File Offset: 0x000290E5
		// (set) Token: 0x0600183A RID: 6202 RVA: 0x0002A100 File Offset: 0x00029100
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[AmbientValue(AutoValidate.Inherit)]
		[SRDescription("ContainerControlAutoValidate")]
		public virtual AutoValidate AutoValidate
		{
			get
			{
				if (this.autoValidate == AutoValidate.Inherit)
				{
					return Control.GetAutoValidateForControl(this);
				}
				return this.autoValidate;
			}
			set
			{
				switch (value)
				{
				case AutoValidate.Inherit:
				case AutoValidate.Disable:
				case AutoValidate.EnablePreventFocusChange:
				case AutoValidate.EnableAllowFocusChange:
					if (this.autoValidate != value)
					{
						this.autoValidate = value;
						this.OnAutoValidateChanged(EventArgs.Empty);
					}
					return;
				default:
					throw new InvalidEnumArgumentException("AutoValidate", (int)value, typeof(AutoValidate));
				}
			}
		}

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x0600183B RID: 6203 RVA: 0x0002A158 File Offset: 0x00029158
		// (remove) Token: 0x0600183C RID: 6204 RVA: 0x0002A171 File Offset: 0x00029171
		[Browsable(false)]
		[SRDescription("ContainerControlOnAutoValidateChangedDescr")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler AutoValidateChanged
		{
			add
			{
				this.autoValidateChanged = (EventHandler)Delegate.Combine(this.autoValidateChanged, value);
			}
			remove
			{
				this.autoValidateChanged = (EventHandler)Delegate.Remove(this.autoValidateChanged, value);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600183D RID: 6205 RVA: 0x0002A18C File Offset: 0x0002918C
		// (set) Token: 0x0600183E RID: 6206 RVA: 0x0002A1B1 File Offset: 0x000291B1
		[SRDescription("ContainerControlBindingContextDescr")]
		[Browsable(false)]
		public override BindingContext BindingContext
		{
			get
			{
				BindingContext bindingContext = base.BindingContext;
				if (bindingContext == null)
				{
					bindingContext = new BindingContext();
					this.BindingContext = bindingContext;
				}
				return bindingContext;
			}
			set
			{
				base.BindingContext = value;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x0002A1BA File Offset: 0x000291BA
		protected override bool CanEnableIme
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x0002A1BD File Offset: 0x000291BD
		// (set) Token: 0x06001841 RID: 6209 RVA: 0x0002A1C5 File Offset: 0x000291C5
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ContainerControlActiveControlDescr")]
		public Control ActiveControl
		{
			get
			{
				return this.activeControl;
			}
			set
			{
				this.SetActiveControl(value);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x0002A1D0 File Offset: 0x000291D0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				return createParams;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x0002A1F8 File Offset: 0x000291F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		public SizeF CurrentAutoScaleDimensions
		{
			get
			{
				if (this.currentAutoScaleDimensions.IsEmpty)
				{
					switch (this.AutoScaleMode)
					{
					case AutoScaleMode.Font:
						this.currentAutoScaleDimensions = this.GetFontAutoScaleDimensions();
						break;
					case AutoScaleMode.Dpi:
						this.currentAutoScaleDimensions = WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.Dpi;
						break;
					default:
						this.currentAutoScaleDimensions = this.AutoScaleDimensions;
						break;
					}
				}
				return this.currentAutoScaleDimensions;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x0002A267 File Offset: 0x00029267
		[SRDescription("ContainerControlParentFormDescr")]
		[Browsable(false)]
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Form ParentForm
		{
			get
			{
				IntSecurity.GetParent.Demand();
				return this.ParentFormInternal;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001845 RID: 6213 RVA: 0x0002A279 File Offset: 0x00029279
		internal Form ParentFormInternal
		{
			get
			{
				if (this.ParentInternal != null)
				{
					return this.ParentInternal.FindFormInternal();
				}
				if (this is Form)
				{
					return null;
				}
				return base.FindFormInternal();
			}
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0002A29F File Offset: 0x0002929F
		bool IContainerControl.ActivateControl(Control control)
		{
			IntSecurity.ModifyFocus.Demand();
			return this.ActivateControlInternal(control, true);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0002A2B3 File Offset: 0x000292B3
		internal bool ActivateControlInternal(Control control)
		{
			return this.ActivateControlInternal(control, true);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0002A2C0 File Offset: 0x000292C0
		internal bool ActivateControlInternal(Control control, bool originator)
		{
			bool flag = true;
			bool flag2 = false;
			ContainerControl containerControl = null;
			Control parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				containerControl = parentInternal.GetContainerControlInternal() as ContainerControl;
				if (containerControl != null)
				{
					flag2 = containerControl.ActiveControl != this;
				}
			}
			if (control != this.activeControl || flag2)
			{
				if (flag2 && !containerControl.ActivateControlInternal(this, false))
				{
					return false;
				}
				flag = this.AssignActiveControlInternal((control == this) ? null : control);
			}
			if (originator)
			{
				this.ScrollActiveControlIntoView();
			}
			return flag;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0002A330 File Offset: 0x00029330
		internal bool HasFocusableChild()
		{
			Control control = null;
			do
			{
				control = base.GetNextControl(control, true);
			}
			while ((control == null || !control.CanSelect || !control.TabStop) && control != null);
			return control != null;
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0002A365 File Offset: 0x00029365
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void AdjustFormScrollbars(bool displayScrollbars)
		{
			base.AdjustFormScrollbars(displayScrollbars);
			if (!base.GetScrollState(8))
			{
				this.ScrollActiveControlIntoView();
			}
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0002A380 File Offset: 0x00029380
		internal virtual void AfterControlRemoved(Control control, Control oldParent)
		{
			ContainerControl containerControl;
			if (control == this.activeControl || control.Contains(this.activeControl))
			{
				IntSecurity.ModifyFocus.Assert();
				bool flag;
				try
				{
					flag = base.SelectNextControl(control, true, true, true, true);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (flag)
				{
					this.FocusActiveControlInternal();
				}
				else
				{
					this.SetActiveControlInternal(null);
				}
			}
			else if (this.activeControl == null && this.ParentInternal != null)
			{
				containerControl = this.ParentInternal.GetContainerControlInternal() as ContainerControl;
				if (containerControl != null && containerControl.ActiveControl == this)
				{
					Form form = base.FindFormInternal();
					if (form != null)
					{
						IntSecurity.ModifyFocus.Assert();
						try
						{
							form.SelectNextControl(this, true, true, true, true);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
			containerControl = this;
			while (containerControl != null)
			{
				Control parentInternal = containerControl.ParentInternal;
				if (parentInternal == null)
				{
					break;
				}
				containerControl = parentInternal.GetContainerControlInternal() as ContainerControl;
				if (containerControl != null && containerControl.unvalidatedControl != null && (containerControl.unvalidatedControl == control || control.Contains(containerControl.unvalidatedControl)))
				{
					containerControl.unvalidatedControl = oldParent;
				}
			}
			if (control == this.unvalidatedControl || control.Contains(this.unvalidatedControl))
			{
				this.unvalidatedControl = null;
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0002A4AC File Offset: 0x000294AC
		private bool AssignActiveControlInternal(Control value)
		{
			if (this.activeControl != value)
			{
				try
				{
					if (value != null)
					{
						value.BecomingActiveControl = true;
					}
					this.activeControl = value;
					this.UpdateFocusedControl();
				}
				finally
				{
					if (value != null)
					{
						value.BecomingActiveControl = false;
					}
				}
				if (this.activeControl == value)
				{
					Form form = base.FindFormInternal();
					if (form != null)
					{
						form.UpdateDefaultButton();
					}
				}
			}
			else
			{
				this.focusedControl = this.activeControl;
			}
			return this.activeControl == value;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0002A528 File Offset: 0x00029528
		private void AxContainerFormCreated()
		{
			((AxHost.AxContainer)base.Properties.GetObject(ContainerControl.PropAxContainer)).FormCreated();
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0002A544 File Offset: 0x00029544
		internal override bool CanProcessMnemonic()
		{
			return this.state[ContainerControl.stateProcessingMnemonic] || base.CanProcessMnemonic();
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0002A560 File Offset: 0x00029560
		internal AxHost.AxContainer CreateAxContainer()
		{
			object obj = base.Properties.GetObject(ContainerControl.PropAxContainer);
			if (obj == null)
			{
				obj = new AxHost.AxContainer(this);
				base.Properties.SetObject(ContainerControl.PropAxContainer, obj);
			}
			return (AxHost.AxContainer)obj;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0002A59F File Offset: 0x0002959F
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.activeControl = null;
			}
			base.Dispose(disposing);
			this.focusedControl = null;
			this.unvalidatedControl = null;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0002A5C0 File Offset: 0x000295C0
		private void EnableRequiredScaling(Control start, bool enable)
		{
			start.RequiredScalingEnabled = enable;
			foreach (object obj in start.Controls)
			{
				Control control = (Control)obj;
				this.EnableRequiredScaling(control, enable);
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0002A624 File Offset: 0x00029624
		internal void FocusActiveControlInternal()
		{
			if (this.activeControl != null && this.activeControl.Visible)
			{
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				if (focus == IntPtr.Zero || Control.FromChildHandleInternal(focus) != this.activeControl)
				{
					UnsafeNativeMethods.SetFocus(new HandleRef(this.activeControl, this.activeControl.Handle));
					return;
				}
			}
			else
			{
				ContainerControl containerControl = this;
				while (containerControl != null && !containerControl.Visible)
				{
					Control parentInternal = containerControl.ParentInternal;
					if (parentInternal == null)
					{
						break;
					}
					containerControl = parentInternal.GetContainerControlInternal() as ContainerControl;
				}
				if (containerControl != null && containerControl.Visible)
				{
					UnsafeNativeMethods.SetFocus(new HandleRef(containerControl, containerControl.Handle));
				}
			}
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0002A6C8 File Offset: 0x000296C8
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size size = this.SizeFromClientSize(Size.Empty);
			Size size2 = size + base.Padding.Size;
			return this.LayoutEngine.GetPreferredSize(this, proposedSize - size2) + size2;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0002A710 File Offset: 0x00029710
		private SizeF GetFontAutoScaleDimensions()
		{
			SizeF empty = SizeF.Empty;
			IntPtr intPtr = UnsafeNativeMethods.CreateCompatibleDC(NativeMethods.NullHandleRef);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			HandleRef handleRef = new HandleRef(this, intPtr);
			try
			{
				HandleRef handleRef2 = new HandleRef(this, base.FontHandle);
				HandleRef handleRef3 = new HandleRef(this, SafeNativeMethods.SelectObject(handleRef, handleRef2));
				try
				{
					NativeMethods.TEXTMETRIC textmetric = default(NativeMethods.TEXTMETRIC);
					SafeNativeMethods.GetTextMetrics(handleRef, ref textmetric);
					empty.Height = (float)textmetric.tmHeight;
					if ((textmetric.tmPitchAndFamily & 1) != 0)
					{
						IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
						IntUnsafeNativeMethods.GetTextExtentPoint32(handleRef, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", size);
						empty.Width = (float)((int)Math.Round((double)((float)size.cx / (float)"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)));
					}
					else
					{
						empty.Width = (float)textmetric.tmAveCharWidth;
					}
				}
				finally
				{
					SafeNativeMethods.SelectObject(handleRef, handleRef3);
				}
			}
			finally
			{
				UnsafeNativeMethods.DeleteCompatibleDC(handleRef);
			}
			return empty;
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0002A810 File Offset: 0x00029810
		private void LayoutScalingNeeded()
		{
			this.EnableRequiredScaling(this, true);
			this.state[ContainerControl.stateScalingNeededOnLayout] = true;
			if (!base.IsLayoutSuspended)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.Bounds);
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0002A83F File Offset: 0x0002983F
		internal virtual void OnAutoScaleModeChanged()
		{
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0002A841 File Offset: 0x00029841
		protected virtual void OnAutoValidateChanged(EventArgs e)
		{
			if (this.autoValidateChanged != null)
			{
				this.autoValidateChanged(this, e);
			}
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0002A858 File Offset: 0x00029858
		internal override void OnFrameWindowActivate(bool fActivate)
		{
			if (fActivate)
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					if (this.ActiveControl == null)
					{
						base.SelectNextControl(null, true, true, true, false);
					}
					this.InnerMostActiveContainerControl.FocusActiveControlInternal();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0002A8AC File Offset: 0x000298AC
		internal override void OnChildLayoutResuming(Control child, bool performLayout)
		{
			base.OnChildLayoutResuming(child, performLayout);
			if (!this.state[ContainerControl.stateScalingChild] && !performLayout && this.AutoScaleMode != AutoScaleMode.None && this.AutoScaleMode != AutoScaleMode.Inherit && this.state[ContainerControl.stateScalingNeededOnLayout])
			{
				this.state[ContainerControl.stateScalingChild] = true;
				try
				{
					child.Scale(this.AutoScaleFactor, SizeF.Empty, this);
				}
				finally
				{
					this.state[ContainerControl.stateScalingChild] = false;
				}
			}
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0002A940 File Offset: 0x00029940
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (base.Properties.GetObject(ContainerControl.PropAxContainer) != null)
			{
				this.AxContainerFormCreated();
			}
			this.OnBindingContextChanged(EventArgs.Empty);
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0002A96C File Offset: 0x0002996C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnFontChanged(EventArgs e)
		{
			if (this.AutoScaleMode == AutoScaleMode.Font)
			{
				this.currentAutoScaleDimensions = SizeF.Empty;
				this.SuspendAllLayout(this);
				try
				{
					this.PerformAutoScale(!base.RequiredScalingEnabled, true);
				}
				finally
				{
					this.ResumeAllLayout(this, false);
				}
			}
			base.OnFontChanged(e);
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0002A9C8 File Offset: 0x000299C8
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.PerformNeededAutoScaleOnLayout();
			base.OnLayout(e);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0002A9D7 File Offset: 0x000299D7
		internal override void OnLayoutResuming(bool performLayout)
		{
			this.PerformNeededAutoScaleOnLayout();
			base.OnLayoutResuming(performLayout);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0002A9E6 File Offset: 0x000299E6
		protected override void OnParentChanged(EventArgs e)
		{
			this.state[ContainerControl.stateParentChanged] = !base.RequiredScalingEnabled;
			base.OnParentChanged(e);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0002AA08 File Offset: 0x00029A08
		public void PerformAutoScale()
		{
			this.PerformAutoScale(true, true);
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0002AA14 File Offset: 0x00029A14
		private void PerformAutoScale(bool includedBounds, bool excludedBounds)
		{
			bool flag = false;
			try
			{
				if (this.AutoScaleMode != AutoScaleMode.None && this.AutoScaleMode != AutoScaleMode.Inherit)
				{
					this.SuspendAllLayout(this);
					flag = true;
					SizeF sizeF = SizeF.Empty;
					SizeF sizeF2 = SizeF.Empty;
					if (includedBounds)
					{
						sizeF = this.AutoScaleFactor;
					}
					if (excludedBounds)
					{
						sizeF2 = this.AutoScaleFactor;
					}
					this.Scale(sizeF, sizeF2, this);
					this.autoScaleDimensions = this.CurrentAutoScaleDimensions;
				}
			}
			finally
			{
				if (includedBounds)
				{
					this.state[ContainerControl.stateScalingNeededOnLayout] = false;
					this.EnableRequiredScaling(this, false);
				}
				this.state[ContainerControl.stateParentChanged] = false;
				if (flag)
				{
					this.ResumeAllLayout(this, false);
				}
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0002AAC0 File Offset: 0x00029AC0
		private void PerformNeededAutoScaleOnLayout()
		{
			if (this.state[ContainerControl.stateScalingNeededOnLayout])
			{
				this.PerformAutoScale(this.state[ContainerControl.stateScalingNeededOnLayout], false);
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0002AAEC File Offset: 0x00029AEC
		internal void ResumeAllLayout(Control start, bool performLayout)
		{
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.ResumeAllLayout(controls[i], performLayout);
			}
			start.ResumeLayout(performLayout);
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0002AB28 File Offset: 0x00029B28
		internal void SuspendAllLayout(Control start)
		{
			start.SuspendLayout();
			CommonProperties.xClearPreferredSizeCache(start);
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.SuspendAllLayout(controls[i]);
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0002AB68 File Offset: 0x00029B68
		internal override void Scale(SizeF includedFactor, SizeF excludedFactor, Control requestingControl)
		{
			if (this.AutoScaleMode == AutoScaleMode.Inherit)
			{
				base.Scale(includedFactor, excludedFactor, requestingControl);
				return;
			}
			SizeF sizeF = excludedFactor;
			SizeF sizeF2 = includedFactor;
			if (!sizeF.IsEmpty)
			{
				sizeF = this.AutoScaleFactor;
			}
			if (this.AutoScaleMode == AutoScaleMode.None)
			{
				sizeF2 = this.AutoScaleFactor;
			}
			using (new LayoutTransaction(this, this, PropertyNames.Bounds, false))
			{
				SizeF sizeF3 = sizeF;
				if (!excludedFactor.IsEmpty && this.ParentInternal != null)
				{
					sizeF3 = SizeF.Empty;
					bool flag = requestingControl != this || this.state[ContainerControl.stateParentChanged];
					if (!flag)
					{
						bool flag2 = false;
						bool flag3 = false;
						ISite site = this.Site;
						ISite site2 = this.ParentInternal.Site;
						if (site != null)
						{
							flag2 = site.DesignMode;
						}
						if (site2 != null)
						{
							flag3 = site2.DesignMode;
						}
						if (flag2 && !flag3)
						{
							flag = true;
						}
					}
					if (flag)
					{
						sizeF3 = excludedFactor;
					}
				}
				base.ScaleControl(includedFactor, sizeF3, requestingControl);
				base.ScaleChildControls(sizeF2, sizeF, requestingControl);
			}
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0002AC64 File Offset: 0x00029C64
		private bool ProcessArrowKey(bool forward)
		{
			Control control = this;
			if (this.activeControl != null)
			{
				control = this.activeControl.ParentInternal;
			}
			return control.SelectNextControl(this.activeControl, forward, false, false, true);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0002AC98 File Offset: 0x00029C98
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogChar(char charCode)
		{
			ContainerControl containerControl = base.GetContainerControlInternal() as ContainerControl;
			return (containerControl != null && charCode != ' ' && this.ProcessMnemonic(charCode)) || base.ProcessDialogChar(charCode);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0002ACCC File Offset: 0x00029CCC
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keys;
				if (keys2 != Keys.Tab)
				{
					switch (keys2)
					{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						if (this.ProcessArrowKey(keys == Keys.Right || keys == Keys.Down))
						{
							return true;
						}
						break;
					}
				}
				else if (this.ProcessTabKey((keyData & Keys.Shift) == Keys.None))
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0002AD3E File Offset: 0x00029D3E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			return base.ProcessCmdKey(ref msg, keyData) || (this.ParentInternal == null && ToolStripManager.ProcessCmdKey(ref msg, keyData));
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0002AD60 File Offset: 0x00029D60
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (!this.CanProcessMnemonic())
			{
				return false;
			}
			if (base.Controls.Count == 0)
			{
				return false;
			}
			Control control = this.ActiveControl;
			this.state[ContainerControl.stateProcessingMnemonic] = true;
			bool flag = false;
			try
			{
				bool flag2 = false;
				Control control2 = control;
				for (;;)
				{
					control2 = base.GetNextControl(control2, true);
					if (control2 != null)
					{
						if (control2.ProcessMnemonic(charCode))
						{
							break;
						}
					}
					else
					{
						if (flag2)
						{
							goto IL_0059;
						}
						flag2 = true;
					}
					if (control2 == control)
					{
						goto IL_0059;
					}
				}
				flag = true;
				IL_0059:;
			}
			finally
			{
				this.state[ContainerControl.stateProcessingMnemonic] = false;
			}
			return flag;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0002ADEC File Offset: 0x00029DEC
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected virtual bool ProcessTabKey(bool forward)
		{
			return base.SelectNextControl(this.activeControl, forward, true, true, false);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0002AE04 File Offset: 0x00029E04
		private ScrollableControl FindScrollableParent(Control ctl)
		{
			Control control = ctl.ParentInternal;
			while (control != null && !(control is ScrollableControl))
			{
				control = control.ParentInternal;
			}
			if (control != null)
			{
				return (ScrollableControl)control;
			}
			return null;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0002AE38 File Offset: 0x00029E38
		private void ScrollActiveControlIntoView()
		{
			Control control = this.activeControl;
			if (control != null)
			{
				for (ScrollableControl scrollableControl = this.FindScrollableParent(control); scrollableControl != null; scrollableControl = this.FindScrollableParent(scrollableControl))
				{
					scrollableControl.ScrollControlIntoView(this.activeControl);
				}
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0002AE74 File Offset: 0x00029E74
		protected override void Select(bool directed, bool forward)
		{
			bool flag = true;
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					containerControlInternal.ActiveControl = this;
					flag = containerControlInternal.ActiveControl == this;
				}
			}
			if (directed && flag)
			{
				base.SelectNextControl(null, forward, true, true, false);
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0002AEBD File Offset: 0x00029EBD
		private void SetActiveControl(Control ctl)
		{
			this.SetActiveControlInternal(ctl);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0002AEC8 File Offset: 0x00029EC8
		internal void SetActiveControlInternal(Control value)
		{
			if (this.activeControl != value || (value != null && !value.Focused))
			{
				if (value != null && !base.Contains(value))
				{
					throw new ArgumentException(SR.GetString("CannotActivateControl"));
				}
				ContainerControl containerControl = this;
				if (value != null && value.ParentInternal != null)
				{
					containerControl = value.ParentInternal.GetContainerControlInternal() as ContainerControl;
				}
				bool flag;
				if (containerControl != null)
				{
					flag = containerControl.ActivateControlInternal(value, false);
				}
				else
				{
					flag = this.AssignActiveControlInternal(value);
				}
				if (containerControl != null && flag)
				{
					ContainerControl containerControl2 = this;
					while (containerControl2.ParentInternal != null && containerControl2.ParentInternal.GetContainerControlInternal() is ContainerControl)
					{
						containerControl2 = containerControl2.ParentInternal.GetContainerControlInternal() as ContainerControl;
					}
					if (containerControl2.ContainsFocus && (value == null || !(value is UserControl) || (value is UserControl && !((UserControl)value).HasFocusableChild())))
					{
						containerControl.FocusActiveControlInternal();
					}
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x0002AFA4 File Offset: 0x00029FA4
		internal ContainerControl InnerMostActiveContainerControl
		{
			get
			{
				ContainerControl containerControl = this;
				while (containerControl.ActiveControl is ContainerControl)
				{
					containerControl = (ContainerControl)containerControl.ActiveControl;
				}
				return containerControl;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x0002AFD0 File Offset: 0x00029FD0
		internal ContainerControl InnerMostFocusedContainerControl
		{
			get
			{
				ContainerControl containerControl = this;
				while (containerControl.focusedControl is ContainerControl)
				{
					containerControl = (ContainerControl)containerControl.focusedControl;
				}
				return containerControl;
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0002AFFB File Offset: 0x00029FFB
		protected virtual void UpdateDefaultButton()
		{
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0002B000 File Offset: 0x0002A000
		internal void UpdateFocusedControl()
		{
			this.EnsureUnvalidatedControl(this.focusedControl);
			Control control = this.focusedControl;
			while (this.activeControl != control)
			{
				if (control == null || control.IsDescendant(this.activeControl))
				{
					Control parentInternal = this.activeControl;
					for (;;)
					{
						Control parentInternal2 = parentInternal.ParentInternal;
						if (parentInternal2 == this || parentInternal2 == control)
						{
							break;
						}
						parentInternal = parentInternal.ParentInternal;
					}
					Control control2 = (this.focusedControl = control);
					this.EnterValidation(parentInternal);
					if (this.focusedControl != control2)
					{
						control = this.focusedControl;
						continue;
					}
					control = parentInternal;
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						control.NotifyEnter();
						continue;
					}
					try
					{
						control.NotifyEnter();
						continue;
					}
					catch (Exception ex)
					{
						Application.OnThreadException(ex);
						continue;
					}
				}
				ContainerControl innerMostFocusedContainerControl = this.InnerMostFocusedContainerControl;
				Control control3 = null;
				if (innerMostFocusedContainerControl.focusedControl != null)
				{
					control = innerMostFocusedContainerControl.focusedControl;
					control3 = innerMostFocusedContainerControl;
					if (innerMostFocusedContainerControl != this)
					{
						innerMostFocusedContainerControl.focusedControl = null;
						if (innerMostFocusedContainerControl.ParentInternal == null || !(innerMostFocusedContainerControl.ParentInternal is MdiClient))
						{
							innerMostFocusedContainerControl.activeControl = null;
						}
					}
				}
				else
				{
					control = innerMostFocusedContainerControl;
					if (innerMostFocusedContainerControl.ParentInternal != null)
					{
						ContainerControl containerControl = innerMostFocusedContainerControl.ParentInternal.GetContainerControlInternal() as ContainerControl;
						control3 = containerControl;
						if (containerControl != null && containerControl != this)
						{
							containerControl.focusedControl = null;
							containerControl.activeControl = null;
						}
					}
				}
				do
				{
					Control control4 = control;
					if (control != null)
					{
						control = control.ParentInternal;
					}
					if (control == this)
					{
						control = null;
					}
					if (control4 != null)
					{
						if (NativeWindow.WndProcShouldBeDebuggable)
						{
							control4.NotifyLeave();
						}
						else
						{
							try
							{
								control4.NotifyLeave();
							}
							catch (Exception ex2)
							{
								Application.OnThreadException(ex2);
							}
						}
					}
				}
				while (control != null && control != control3 && !control.IsDescendant(this.activeControl));
			}
			this.focusedControl = this.activeControl;
			if (this.activeControl != null)
			{
				this.EnterValidation(this.activeControl);
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0002B1D0 File Offset: 0x0002A1D0
		private void EnsureUnvalidatedControl(Control candidate)
		{
			if (this.state[ContainerControl.stateValidating])
			{
				return;
			}
			if (this.unvalidatedControl != null)
			{
				return;
			}
			if (candidate == null)
			{
				return;
			}
			if (!candidate.ShouldAutoValidate)
			{
				return;
			}
			this.unvalidatedControl = candidate;
			while (this.unvalidatedControl is ContainerControl)
			{
				ContainerControl containerControl = this.unvalidatedControl as ContainerControl;
				if (containerControl.unvalidatedControl != null && containerControl.unvalidatedControl.ShouldAutoValidate)
				{
					this.unvalidatedControl = containerControl.unvalidatedControl;
				}
				else
				{
					if (containerControl.activeControl == null || !containerControl.activeControl.ShouldAutoValidate)
					{
						break;
					}
					this.unvalidatedControl = containerControl.activeControl;
				}
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0002B26C File Offset: 0x0002A26C
		private void EnterValidation(Control enterControl)
		{
			if (this.unvalidatedControl == null)
			{
				return;
			}
			if (!enterControl.CausesValidation)
			{
				return;
			}
			AutoValidate autoValidateForControl = Control.GetAutoValidateForControl(this.unvalidatedControl);
			if (autoValidateForControl == AutoValidate.Disable)
			{
				return;
			}
			Control control = enterControl;
			while (control != null && !control.IsDescendant(this.unvalidatedControl))
			{
				control = control.ParentInternal;
			}
			bool flag = autoValidateForControl == AutoValidate.EnablePreventFocusChange;
			this.ValidateThroughAncestor(control, flag);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0002B2C5 File Offset: 0x0002A2C5
		public bool Validate()
		{
			return this.Validate(false);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0002B2D0 File Offset: 0x0002A2D0
		public bool Validate(bool checkAutoValidate)
		{
			bool flag;
			return this.ValidateInternal(checkAutoValidate, out flag);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0002B2E8 File Offset: 0x0002A2E8
		internal bool ValidateInternal(bool checkAutoValidate, out bool validatedControlAllowsFocusChange)
		{
			validatedControlAllowsFocusChange = false;
			if (this.AutoValidate == AutoValidate.EnablePreventFocusChange || (this.activeControl != null && this.activeControl.CausesValidation))
			{
				if (this.unvalidatedControl == null)
				{
					if (this.focusedControl is ContainerControl && this.focusedControl.CausesValidation)
					{
						ContainerControl containerControl = (ContainerControl)this.focusedControl;
						if (!containerControl.ValidateInternal(checkAutoValidate, out validatedControlAllowsFocusChange))
						{
							return false;
						}
					}
					else
					{
						this.unvalidatedControl = this.focusedControl;
					}
				}
				bool flag = true;
				Control control = ((this.unvalidatedControl != null) ? this.unvalidatedControl : this.focusedControl);
				if (control != null)
				{
					AutoValidate autoValidateForControl = Control.GetAutoValidateForControl(control);
					if (checkAutoValidate && autoValidateForControl == AutoValidate.Disable)
					{
						return true;
					}
					flag = autoValidateForControl == AutoValidate.EnablePreventFocusChange;
					validatedControlAllowsFocusChange = autoValidateForControl == AutoValidate.EnableAllowFocusChange;
				}
				return this.ValidateThroughAncestor(null, flag);
			}
			return true;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0002B3A2 File Offset: 0x0002A3A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ValidateChildren()
		{
			return this.ValidateChildren(ValidationConstraints.Selectable);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0002B3AB File Offset: 0x0002A3AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			if ((validationConstraints < ValidationConstraints.None) || validationConstraints > (ValidationConstraints.Selectable | ValidationConstraints.Enabled | ValidationConstraints.Visible | ValidationConstraints.TabStop | ValidationConstraints.ImmediateChildren))
			{
				throw new InvalidEnumArgumentException("validationConstraints", (int)validationConstraints, typeof(ValidationConstraints));
			}
			return !base.PerformContainerValidation(validationConstraints);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0002B3D8 File Offset: 0x0002A3D8
		private bool ValidateThroughAncestor(Control ancestorControl, bool preventFocusChangeOnError)
		{
			if (ancestorControl == null)
			{
				ancestorControl = this;
			}
			if (this.state[ContainerControl.stateValidating])
			{
				return false;
			}
			if (this.unvalidatedControl == null)
			{
				this.unvalidatedControl = this.focusedControl;
			}
			if (this.unvalidatedControl == null)
			{
				return true;
			}
			if (!ancestorControl.IsDescendant(this.unvalidatedControl))
			{
				return false;
			}
			this.state[ContainerControl.stateValidating] = true;
			bool flag = false;
			Control control = this.activeControl;
			Control parentInternal = this.unvalidatedControl;
			if (control != null)
			{
				control.ValidationCancelled = false;
				if (control is ContainerControl)
				{
					ContainerControl containerControl = control as ContainerControl;
					containerControl.ResetValidationFlag();
				}
			}
			try
			{
				while (parentInternal != null && parentInternal != ancestorControl)
				{
					try
					{
						flag = parentInternal.PerformControlValidation(false);
					}
					catch
					{
						flag = true;
						throw;
					}
					if (flag)
					{
						break;
					}
					parentInternal = parentInternal.ParentInternal;
				}
				if (flag && preventFocusChangeOnError)
				{
					if (this.unvalidatedControl == null && parentInternal != null && ancestorControl.IsDescendant(parentInternal))
					{
						this.unvalidatedControl = parentInternal;
					}
					if (control == this.activeControl && control != null)
					{
						control.NotifyValidationResult(parentInternal, new CancelEventArgs
						{
							Cancel = true
						});
						if (control is ContainerControl)
						{
							ContainerControl containerControl2 = control as ContainerControl;
							if (containerControl2.focusedControl != null)
							{
								containerControl2.focusedControl.ValidationCancelled = true;
							}
							containerControl2.ResetActiveAndFocusedControlsRecursive();
						}
					}
					this.SetActiveControlInternal(this.unvalidatedControl);
				}
			}
			finally
			{
				this.unvalidatedControl = null;
				this.state[ContainerControl.stateValidating] = false;
			}
			return !flag;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0002B548 File Offset: 0x0002A548
		private void ResetValidationFlag()
		{
			Control.ControlCollection controls = base.Controls;
			int count = controls.Count;
			for (int i = 0; i < count; i++)
			{
				controls[i].ValidationCancelled = false;
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0002B57C File Offset: 0x0002A57C
		internal void ResetActiveAndFocusedControlsRecursive()
		{
			if (this.activeControl is ContainerControl)
			{
				((ContainerControl)this.activeControl).ResetActiveAndFocusedControlsRecursive();
			}
			this.activeControl = null;
			this.focusedControl = null;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0002B5A9 File Offset: 0x0002A5A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAutoValidate()
		{
			return this.autoValidate != AutoValidate.Inherit;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0002B5B8 File Offset: 0x0002A5B8
		private void WmSetFocus(ref Message m)
		{
			if (base.HostedInWin32DialogManager)
			{
				base.WndProc(ref m);
				return;
			}
			if (this.ActiveControl != null)
			{
				base.WmImeSetFocus();
				if (!this.ActiveControl.Visible)
				{
					this.OnGotFocus(EventArgs.Empty);
				}
				this.FocusActiveControlInternal();
				return;
			}
			if (this.ParentInternal != null)
			{
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					bool flag = false;
					ContainerControl containerControl = containerControlInternal as ContainerControl;
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
			base.WndProc(ref m);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0002B668 File Offset: 0x0002A668
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 7)
			{
				this.WmSetFocus(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x040011E7 RID: 4583
		private const string fontMeasureString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x040011E8 RID: 4584
		private Control activeControl;

		// Token: 0x040011E9 RID: 4585
		private Control focusedControl;

		// Token: 0x040011EA RID: 4586
		private Control unvalidatedControl;

		// Token: 0x040011EB RID: 4587
		private AutoValidate autoValidate = AutoValidate.Inherit;

		// Token: 0x040011EC RID: 4588
		private EventHandler autoValidateChanged;

		// Token: 0x040011ED RID: 4589
		private SizeF autoScaleDimensions = SizeF.Empty;

		// Token: 0x040011EE RID: 4590
		private SizeF currentAutoScaleDimensions = SizeF.Empty;

		// Token: 0x040011EF RID: 4591
		private AutoScaleMode autoScaleMode = AutoScaleMode.Inherit;

		// Token: 0x040011F0 RID: 4592
		private BitVector32 state = default(BitVector32);

		// Token: 0x040011F1 RID: 4593
		private static readonly int stateScalingNeededOnLayout = BitVector32.CreateMask();

		// Token: 0x040011F2 RID: 4594
		private static readonly int stateValidating = BitVector32.CreateMask(ContainerControl.stateScalingNeededOnLayout);

		// Token: 0x040011F3 RID: 4595
		private static readonly int stateProcessingMnemonic = BitVector32.CreateMask(ContainerControl.stateValidating);

		// Token: 0x040011F4 RID: 4596
		private static readonly int stateScalingChild = BitVector32.CreateMask(ContainerControl.stateProcessingMnemonic);

		// Token: 0x040011F5 RID: 4597
		private static readonly int stateParentChanged = BitVector32.CreateMask(ContainerControl.stateScalingChild);

		// Token: 0x040011F6 RID: 4598
		private static readonly int PropAxContainer = PropertyStore.CreateKey();
	}
}

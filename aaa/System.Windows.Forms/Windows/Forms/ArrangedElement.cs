using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000217 RID: 535
	internal abstract class ArrangedElement : Component, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x060018A9 RID: 6313 RVA: 0x0002BDA8 File Offset: 0x0002ADA8
		internal ArrangedElement()
		{
			this.Padding = this.DefaultPadding;
			this.Margin = this.DefaultMargin;
			this.state[ArrangedElement.stateVisible] = true;
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x0002BE11 File Offset: 0x0002AE11
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x0002BE19 File Offset: 0x0002AE19
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.GetChildren();
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x0002BE21 File Offset: 0x0002AE21
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.GetContainer();
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0002BE29 File Offset: 0x0002AE29
		protected virtual Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0002BE30 File Offset: 0x0002AE30
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0002BE38 File Offset: 0x0002AE38
		public virtual Rectangle DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060018B0 RID: 6320
		public abstract LayoutEngine LayoutEngine { get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0002BE4D File Offset: 0x0002AE4D
		// (set) Token: 0x060018B2 RID: 6322 RVA: 0x0002BE55 File Offset: 0x0002AE55
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (this.Margin != value)
				{
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x0002BE74 File Offset: 0x0002AE74
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x0002BE82 File Offset: 0x0002AE82
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				value = LayoutUtils.ClampNegativePaddingToZero(value);
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0002BEA1 File Offset: 0x0002AEA1
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0002BEA9 File Offset: 0x0002AEA9
		public virtual IArrangedElement Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0002BEB2 File Offset: 0x0002AEB2
		public virtual bool ParticipatesInLayout
		{
			get
			{
				return this.Visible;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x0002BEBA File Offset: 0x0002AEBA
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0002BEC2 File Offset: 0x0002AEC2
		private PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x0002BECA File Offset: 0x0002AECA
		// (set) Token: 0x060018BB RID: 6331 RVA: 0x0002BEDC File Offset: 0x0002AEDC
		public virtual bool Visible
		{
			get
			{
				return this.state[ArrangedElement.stateVisible];
			}
			set
			{
				if (this.state[ArrangedElement.stateVisible] != value)
				{
					this.state[ArrangedElement.stateVisible] = value;
					if (this.Parent != null)
					{
						LayoutTransaction.DoLayout(this.Parent, this, PropertyNames.Visible);
					}
				}
			}
		}

		// Token: 0x060018BC RID: 6332
		protected abstract IArrangedElement GetContainer();

		// Token: 0x060018BD RID: 6333
		protected abstract ArrangedElementCollection GetChildren();

		// Token: 0x060018BE RID: 6334 RVA: 0x0002BF1C File Offset: 0x0002AF1C
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			return this.LayoutEngine.GetPreferredSize(this, constrainingSize - this.Padding.Size) + this.Padding.Size;
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0002BF5E File Offset: 0x0002AF5E
		public virtual void PerformLayout(IArrangedElement container, string propertyName)
		{
			if (this.suspendCount <= 0)
			{
				this.OnLayout(new LayoutEventArgs(container, propertyName));
			}
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0002BF76 File Offset: 0x0002AF76
		protected virtual void OnLayout(LayoutEventArgs e)
		{
			this.LayoutEngine.Layout(this, e);
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0002BF86 File Offset: 0x0002AF86
		protected virtual void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			((IArrangedElement)this).PerformLayout(this, PropertyNames.Size);
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0002BF94 File Offset: 0x0002AF94
		public void SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds, specified);
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0002BFA0 File Offset: 0x0002AFA0
		protected virtual void SetBoundsCore(Rectangle bounds, BoundsSpecified specified)
		{
			if (bounds != this.bounds)
			{
				Rectangle rectangle = this.bounds;
				this.bounds = bounds;
				this.OnBoundsChanged(rectangle, bounds);
			}
		}

		// Token: 0x04001208 RID: 4616
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x04001209 RID: 4617
		private IArrangedElement parent;

		// Token: 0x0400120A RID: 4618
		private BitVector32 state = default(BitVector32);

		// Token: 0x0400120B RID: 4619
		private PropertyStore propertyStore = new PropertyStore();

		// Token: 0x0400120C RID: 4620
		private int suspendCount;

		// Token: 0x0400120D RID: 4621
		private static readonly int stateVisible = BitVector32.CreateMask();

		// Token: 0x0400120E RID: 4622
		private static readonly int stateDisposing = BitVector32.CreateMask(ArrangedElement.stateVisible);

		// Token: 0x0400120F RID: 4623
		private static readonly int stateLocked = BitVector32.CreateMask(ArrangedElement.stateDisposing);

		// Token: 0x04001210 RID: 4624
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x04001211 RID: 4625
		private Control spacer = new Control();
	}
}

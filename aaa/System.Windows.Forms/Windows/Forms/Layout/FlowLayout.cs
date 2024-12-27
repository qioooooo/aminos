using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020002A6 RID: 678
	internal class FlowLayout : LayoutEngine
	{
		// Token: 0x0600257E RID: 9598 RVA: 0x00057A08 File Offset: 0x00056A08
		internal static FlowLayoutSettings CreateSettings(IArrangedElement owner)
		{
			return new FlowLayoutSettings(owner);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00057A10 File Offset: 0x00056A10
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs args)
		{
			CommonProperties.SetLayoutBounds(container, this.xLayout(container, container.DisplayRectangle, false));
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00057A2C File Offset: 0x00056A2C
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			Rectangle rectangle = new Rectangle(new Point(0, 0), proposedConstraints);
			Size size = this.xLayout(container, rectangle, true);
			if (size.Width > proposedConstraints.Width || size.Height > proposedConstraints.Height)
			{
				rectangle.Size = size;
				size = this.xLayout(container, rectangle, true);
			}
			return size;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00057A88 File Offset: 0x00056A88
		private static FlowLayout.ContainerProxy CreateContainerProxy(IArrangedElement container, FlowDirection flowDirection)
		{
			switch (flowDirection)
			{
			case FlowDirection.TopDown:
				return new FlowLayout.TopDownProxy(container);
			case FlowDirection.RightToLeft:
				return new FlowLayout.RightToLeftProxy(container);
			case FlowDirection.BottomUp:
				return new FlowLayout.BottomUpProxy(container);
			}
			return new FlowLayout.ContainerProxy(container);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00057ACC File Offset: 0x00056ACC
		private Size xLayout(IArrangedElement container, Rectangle displayRect, bool measureOnly)
		{
			FlowDirection flowDirection = FlowLayout.GetFlowDirection(container);
			bool wrapContents = FlowLayout.GetWrapContents(container);
			FlowLayout.ContainerProxy containerProxy = FlowLayout.CreateContainerProxy(container, flowDirection);
			containerProxy.DisplayRect = displayRect;
			displayRect = containerProxy.DisplayRect;
			FlowLayout.ElementProxy elementProxy = containerProxy.ElementProxy;
			Size empty = Size.Empty;
			if (!wrapContents)
			{
				displayRect.Width = int.MaxValue - displayRect.X;
			}
			int num;
			for (int i = 0; i < container.Children.Count; i = num)
			{
				Size size = Size.Empty;
				Rectangle rectangle = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, displayRect.Height - empty.Height);
				size = this.MeasureRow(containerProxy, elementProxy, i, rectangle, out num);
				if (!measureOnly)
				{
					Rectangle rectangle2 = new Rectangle(displayRect.X, empty.Height + displayRect.Y, size.Width, size.Height);
					this.LayoutRow(containerProxy, elementProxy, i, num, rectangle2);
				}
				empty.Width = Math.Max(empty.Width, size.Width);
				empty.Height += size.Height;
			}
			if (container.Children.Count != 0)
			{
			}
			return LayoutUtils.FlipSizeIf(flowDirection == FlowDirection.TopDown || FlowLayout.GetFlowDirection(container) == FlowDirection.BottomUp, empty);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00057C14 File Offset: 0x00056C14
		private void LayoutRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, int endIndex, Rectangle rowBounds)
		{
			int num;
			this.xLayoutRow(containerProxy, elementProxy, startIndex, endIndex, rowBounds, out num, false);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00057C32 File Offset: 0x00056C32
		private Size MeasureRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, Rectangle displayRectangle, out int breakIndex)
		{
			return this.xLayoutRow(containerProxy, elementProxy, startIndex, containerProxy.Container.Children.Count, displayRectangle, out breakIndex, true);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00057C54 File Offset: 0x00056C54
		private Size xLayoutRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, int endIndex, Rectangle rowBounds, out int breakIndex, bool measureOnly)
		{
			Point location = rowBounds.Location;
			Size empty = Size.Empty;
			int num = 0;
			breakIndex = startIndex;
			bool wrapContents = FlowLayout.GetWrapContents(containerProxy.Container);
			bool flag = false;
			ArrangedElementCollection children = containerProxy.Container.Children;
			int i = startIndex;
			while (i < endIndex)
			{
				elementProxy.Element = children[i];
				if (elementProxy.ParticipatesInLayout)
				{
					Size size2;
					if (elementProxy.AutoSize)
					{
						Size size = new Size(int.MaxValue, rowBounds.Height - elementProxy.Margin.Size.Height);
						if (i == startIndex)
						{
							size.Width = rowBounds.Width - empty.Width - elementProxy.Margin.Size.Width;
						}
						size = LayoutUtils.UnionSizes(new Size(1, 1), size);
						size2 = elementProxy.GetPreferredSize(size);
					}
					else
					{
						size2 = elementProxy.SpecifiedSize;
						if (elementProxy.Stretches)
						{
							size2.Height = 0;
						}
						if (size2.Height < elementProxy.MinimumSize.Height)
						{
							size2.Height = elementProxy.MinimumSize.Height;
						}
					}
					Size size3 = size2 + elementProxy.Margin.Size;
					if (!measureOnly)
					{
						Rectangle rectangle = new Rectangle(location, new Size(size3.Width, rowBounds.Height));
						rectangle = LayoutUtils.DeflateRect(rectangle, elementProxy.Margin);
						AnchorStyles anchorStyles = elementProxy.AnchorStyles;
						containerProxy.Bounds = LayoutUtils.AlignAndStretch(size2, rectangle, anchorStyles);
					}
					location.X += size3.Width;
					if (num > 0 && location.X > rowBounds.Right)
					{
						break;
					}
					empty.Width = location.X - rowBounds.X;
					empty.Height = Math.Max(empty.Height, size3.Height);
					if (wrapContents)
					{
						if (flag)
						{
							break;
						}
						if (i + 1 < endIndex && CommonProperties.GetFlowBreak(elementProxy.Element))
						{
							if (num != 0)
							{
								breakIndex++;
								break;
							}
							flag = true;
						}
					}
					num++;
				}
				i++;
				breakIndex++;
			}
			return empty;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00057E80 File Offset: 0x00056E80
		public static bool GetWrapContents(IArrangedElement container)
		{
			int integer = container.Properties.GetInteger(FlowLayout._wrapContentsProperty);
			return integer == 0;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00057EA2 File Offset: 0x00056EA2
		public static void SetWrapContents(IArrangedElement container, bool value)
		{
			container.Properties.SetInteger(FlowLayout._wrapContentsProperty, value ? 0 : 1);
			LayoutTransaction.DoLayout(container, container, PropertyNames.WrapContents);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00057EC7 File Offset: 0x00056EC7
		public static FlowDirection GetFlowDirection(IArrangedElement container)
		{
			return (FlowDirection)container.Properties.GetInteger(FlowLayout._flowDirectionProperty);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00057EDC File Offset: 0x00056EDC
		public static void SetFlowDirection(IArrangedElement container, FlowDirection value)
		{
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
			{
				throw new InvalidEnumArgumentException("value", (int)value, typeof(FlowDirection));
			}
			container.Properties.SetInteger(FlowLayout._flowDirectionProperty, (int)value);
			LayoutTransaction.DoLayout(container, container, PropertyNames.FlowDirection);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00057F2C File Offset: 0x00056F2C
		[Conditional("DEBUG_VERIFY_ALIGNMENT")]
		private void Debug_VerifyAlignment(IArrangedElement container, FlowDirection flowDirection)
		{
		}

		// Token: 0x040015EB RID: 5611
		internal static readonly FlowLayout Instance = new FlowLayout();

		// Token: 0x040015EC RID: 5612
		private static readonly int _wrapContentsProperty = PropertyStore.CreateKey();

		// Token: 0x040015ED RID: 5613
		private static readonly int _flowDirectionProperty = PropertyStore.CreateKey();

		// Token: 0x020002A7 RID: 679
		private class ContainerProxy
		{
			// Token: 0x0600258D RID: 9613 RVA: 0x00057F56 File Offset: 0x00056F56
			public ContainerProxy(IArrangedElement container)
			{
				this._container = container;
				this._isContainerRTL = false;
				if (this._container is Control)
				{
					this._isContainerRTL = ((Control)this._container).RightToLeft == RightToLeft.Yes;
				}
			}

			// Token: 0x170005D8 RID: 1496
			// (set) Token: 0x0600258E RID: 9614 RVA: 0x00057F94 File Offset: 0x00056F94
			public virtual Rectangle Bounds
			{
				set
				{
					if (this.IsContainerRTL)
					{
						if (this.IsVertical)
						{
							value.Y = this.DisplayRect.Bottom - value.Bottom;
						}
						else
						{
							value.X = this.DisplayRect.Right - value.Right;
						}
						FlowLayoutPanel flowLayoutPanel = this.Container as FlowLayoutPanel;
						if (flowLayoutPanel != null)
						{
							Point autoScrollPosition = flowLayoutPanel.AutoScrollPosition;
							if (autoScrollPosition != Point.Empty)
							{
								Point point = new Point(value.X, value.Y);
								if (this.IsVertical)
								{
									point.Offset(0, autoScrollPosition.X);
								}
								else
								{
									point.Offset(autoScrollPosition.X, 0);
								}
								value.Location = point;
							}
						}
					}
					this.ElementProxy.Bounds = value;
				}
			}

			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x0600258F RID: 9615 RVA: 0x00058065 File Offset: 0x00057065
			public IArrangedElement Container
			{
				get
				{
					return this._container;
				}
			}

			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x06002590 RID: 9616 RVA: 0x0005806D File Offset: 0x0005706D
			protected virtual bool IsVertical
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170005DB RID: 1499
			// (get) Token: 0x06002591 RID: 9617 RVA: 0x00058070 File Offset: 0x00057070
			protected bool IsContainerRTL
			{
				get
				{
					return this._isContainerRTL;
				}
			}

			// Token: 0x170005DC RID: 1500
			// (get) Token: 0x06002592 RID: 9618 RVA: 0x00058078 File Offset: 0x00057078
			// (set) Token: 0x06002593 RID: 9619 RVA: 0x00058080 File Offset: 0x00057080
			public Rectangle DisplayRect
			{
				get
				{
					return this._displayRect;
				}
				set
				{
					if (this._displayRect != value)
					{
						this._displayRect = LayoutUtils.FlipRectangleIf(this.IsVertical, value);
					}
				}
			}

			// Token: 0x170005DD RID: 1501
			// (get) Token: 0x06002594 RID: 9620 RVA: 0x000580A2 File Offset: 0x000570A2
			public FlowLayout.ElementProxy ElementProxy
			{
				get
				{
					if (this._elementProxy == null)
					{
						this._elementProxy = (this.IsVertical ? new FlowLayout.VerticalElementProxy() : new FlowLayout.ElementProxy());
					}
					return this._elementProxy;
				}
			}

			// Token: 0x06002595 RID: 9621 RVA: 0x000580CC File Offset: 0x000570CC
			protected Rectangle RTLTranslateNoMarginSwap(Rectangle bounds)
			{
				Rectangle rectangle = bounds;
				rectangle.X = this.DisplayRect.Right - bounds.X - bounds.Width + this.ElementProxy.Margin.Left - this.ElementProxy.Margin.Right;
				FlowLayoutPanel flowLayoutPanel = this.Container as FlowLayoutPanel;
				if (flowLayoutPanel != null)
				{
					Point autoScrollPosition = flowLayoutPanel.AutoScrollPosition;
					if (autoScrollPosition != Point.Empty)
					{
						Point point = new Point(rectangle.X, rectangle.Y);
						if (this.IsVertical)
						{
							point.Offset(autoScrollPosition.Y, 0);
						}
						else
						{
							point.Offset(autoScrollPosition.X, 0);
						}
						rectangle.Location = point;
					}
				}
				return rectangle;
			}

			// Token: 0x040015EE RID: 5614
			private IArrangedElement _container;

			// Token: 0x040015EF RID: 5615
			private FlowLayout.ElementProxy _elementProxy;

			// Token: 0x040015F0 RID: 5616
			private Rectangle _displayRect;

			// Token: 0x040015F1 RID: 5617
			private bool _isContainerRTL;
		}

		// Token: 0x020002A8 RID: 680
		private class RightToLeftProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x06002596 RID: 9622 RVA: 0x00058194 File Offset: 0x00057194
			public RightToLeftProxy(IArrangedElement container)
				: base(container)
			{
			}

			// Token: 0x170005DE RID: 1502
			// (set) Token: 0x06002597 RID: 9623 RVA: 0x0005819D File Offset: 0x0005719D
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = base.RTLTranslateNoMarginSwap(value);
				}
			}
		}

		// Token: 0x020002A9 RID: 681
		private class TopDownProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x06002598 RID: 9624 RVA: 0x000581AC File Offset: 0x000571AC
			public TopDownProxy(IArrangedElement container)
				: base(container)
			{
			}

			// Token: 0x170005DF RID: 1503
			// (get) Token: 0x06002599 RID: 9625 RVA: 0x000581B5 File Offset: 0x000571B5
			protected override bool IsVertical
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x020002AA RID: 682
		private class BottomUpProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x0600259A RID: 9626 RVA: 0x000581B8 File Offset: 0x000571B8
			public BottomUpProxy(IArrangedElement container)
				: base(container)
			{
			}

			// Token: 0x170005E0 RID: 1504
			// (get) Token: 0x0600259B RID: 9627 RVA: 0x000581C1 File Offset: 0x000571C1
			protected override bool IsVertical
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170005E1 RID: 1505
			// (set) Token: 0x0600259C RID: 9628 RVA: 0x000581C4 File Offset: 0x000571C4
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = base.RTLTranslateNoMarginSwap(value);
				}
			}
		}

		// Token: 0x020002AB RID: 683
		private class ElementProxy
		{
			// Token: 0x170005E2 RID: 1506
			// (get) Token: 0x0600259D RID: 9629 RVA: 0x000581D4 File Offset: 0x000571D4
			public virtual AnchorStyles AnchorStyles
			{
				get
				{
					AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(this.Element);
					bool flag = (unifiedAnchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom);
					bool flag2 = (unifiedAnchor & AnchorStyles.Top) != AnchorStyles.None;
					bool flag3 = (unifiedAnchor & AnchorStyles.Bottom) != AnchorStyles.None;
					if (flag)
					{
						return AnchorStyles.Top | AnchorStyles.Bottom;
					}
					if (flag2)
					{
						return AnchorStyles.Top;
					}
					if (flag3)
					{
						return AnchorStyles.Bottom;
					}
					return AnchorStyles.None;
				}
			}

			// Token: 0x170005E3 RID: 1507
			// (get) Token: 0x0600259E RID: 9630 RVA: 0x00058218 File Offset: 0x00057218
			public bool AutoSize
			{
				get
				{
					return CommonProperties.GetAutoSize(this._element);
				}
			}

			// Token: 0x170005E4 RID: 1508
			// (set) Token: 0x0600259F RID: 9631 RVA: 0x00058225 File Offset: 0x00057225
			public virtual Rectangle Bounds
			{
				set
				{
					this._element.SetBounds(value, BoundsSpecified.None);
				}
			}

			// Token: 0x170005E5 RID: 1509
			// (get) Token: 0x060025A0 RID: 9632 RVA: 0x00058234 File Offset: 0x00057234
			// (set) Token: 0x060025A1 RID: 9633 RVA: 0x0005823C File Offset: 0x0005723C
			public IArrangedElement Element
			{
				get
				{
					return this._element;
				}
				set
				{
					this._element = value;
				}
			}

			// Token: 0x170005E6 RID: 1510
			// (get) Token: 0x060025A2 RID: 9634 RVA: 0x00058248 File Offset: 0x00057248
			public bool Stretches
			{
				get
				{
					AnchorStyles anchorStyles = this.AnchorStyles;
					return ((AnchorStyles.Top | AnchorStyles.Bottom) & anchorStyles) == (AnchorStyles.Top | AnchorStyles.Bottom);
				}
			}

			// Token: 0x170005E7 RID: 1511
			// (get) Token: 0x060025A3 RID: 9635 RVA: 0x00058265 File Offset: 0x00057265
			public virtual Padding Margin
			{
				get
				{
					return CommonProperties.GetMargin(this.Element);
				}
			}

			// Token: 0x170005E8 RID: 1512
			// (get) Token: 0x060025A4 RID: 9636 RVA: 0x00058272 File Offset: 0x00057272
			public virtual Size MinimumSize
			{
				get
				{
					return CommonProperties.GetMinimumSize(this.Element, Size.Empty);
				}
			}

			// Token: 0x170005E9 RID: 1513
			// (get) Token: 0x060025A5 RID: 9637 RVA: 0x00058284 File Offset: 0x00057284
			public bool ParticipatesInLayout
			{
				get
				{
					return this._element.ParticipatesInLayout;
				}
			}

			// Token: 0x170005EA RID: 1514
			// (get) Token: 0x060025A6 RID: 9638 RVA: 0x00058294 File Offset: 0x00057294
			public virtual Size SpecifiedSize
			{
				get
				{
					return CommonProperties.GetSpecifiedBounds(this._element).Size;
				}
			}

			// Token: 0x060025A7 RID: 9639 RVA: 0x000582B4 File Offset: 0x000572B4
			public virtual Size GetPreferredSize(Size proposedSize)
			{
				return this._element.GetPreferredSize(proposedSize);
			}

			// Token: 0x040015F2 RID: 5618
			private IArrangedElement _element;
		}

		// Token: 0x020002AC RID: 684
		private class VerticalElementProxy : FlowLayout.ElementProxy
		{
			// Token: 0x170005EB RID: 1515
			// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000582CC File Offset: 0x000572CC
			public override AnchorStyles AnchorStyles
			{
				get
				{
					AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(base.Element);
					bool flag = (unifiedAnchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right);
					bool flag2 = (unifiedAnchor & AnchorStyles.Left) != AnchorStyles.None;
					bool flag3 = (unifiedAnchor & AnchorStyles.Right) != AnchorStyles.None;
					if (flag)
					{
						return AnchorStyles.Top | AnchorStyles.Bottom;
					}
					if (flag2)
					{
						return AnchorStyles.Top;
					}
					if (flag3)
					{
						return AnchorStyles.Bottom;
					}
					return AnchorStyles.None;
				}
			}

			// Token: 0x170005EC RID: 1516
			// (set) Token: 0x060025AA RID: 9642 RVA: 0x00058312 File Offset: 0x00057312
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = LayoutUtils.FlipRectangle(value);
				}
			}

			// Token: 0x170005ED RID: 1517
			// (get) Token: 0x060025AB RID: 9643 RVA: 0x00058320 File Offset: 0x00057320
			public override Padding Margin
			{
				get
				{
					return LayoutUtils.FlipPadding(base.Margin);
				}
			}

			// Token: 0x170005EE RID: 1518
			// (get) Token: 0x060025AC RID: 9644 RVA: 0x0005832D File Offset: 0x0005732D
			public override Size MinimumSize
			{
				get
				{
					return LayoutUtils.FlipSize(base.MinimumSize);
				}
			}

			// Token: 0x170005EF RID: 1519
			// (get) Token: 0x060025AD RID: 9645 RVA: 0x0005833A File Offset: 0x0005733A
			public override Size SpecifiedSize
			{
				get
				{
					return LayoutUtils.FlipSize(base.SpecifiedSize);
				}
			}

			// Token: 0x060025AE RID: 9646 RVA: 0x00058347 File Offset: 0x00057347
			public override Size GetPreferredSize(Size proposedSize)
			{
				return LayoutUtils.FlipSize(base.GetPreferredSize(LayoutUtils.FlipSize(proposedSize)));
			}
		}
	}
}

using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013C RID: 316
	internal sealed class FilterQuery : BaseAxisQuery
	{
		// Token: 0x0600120C RID: 4620 RVA: 0x0004F41C File Offset: 0x0004E41C
		public FilterQuery(Query qyParent, Query cond, bool noPosition)
			: base(qyParent)
		{
			this.cond = cond;
			this.noPosition = noPosition;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0004F433 File Offset: 0x0004E433
		private FilterQuery(FilterQuery other)
			: base(other)
		{
			this.cond = Query.Clone(other.cond);
			this.noPosition = other.noPosition;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0004F459 File Offset: 0x0004E459
		public override void Reset()
		{
			this.cond.Reset();
			base.Reset();
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0004F46C File Offset: 0x0004E46C
		public Query Condition
		{
			get
			{
				return this.cond;
			}
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0004F474 File Offset: 0x0004E474
		public override void SetXsltContext(XsltContext input)
		{
			base.SetXsltContext(input);
			this.cond.SetXsltContext(input);
			if (this.cond.StaticType != XPathResultType.Number && this.cond.StaticType != XPathResultType.Any && this.noPosition)
			{
				ReversePositionQuery reversePositionQuery = this.qyInput as ReversePositionQuery;
				if (reversePositionQuery != null)
				{
					this.qyInput = reversePositionQuery.input;
				}
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0004F4D4 File Offset: 0x0004E4D4
		public override XPathNavigator Advance()
		{
			while ((this.currentNode = this.qyInput.Advance()) != null)
			{
				if (this.EvaluatePredicate())
				{
					this.position++;
					return this.currentNode;
				}
			}
			return null;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004F518 File Offset: 0x0004E518
		internal bool EvaluatePredicate()
		{
			object obj = this.cond.Evaluate(this.qyInput);
			if (obj is XPathNodeIterator)
			{
				return this.cond.Advance() != null;
			}
			if (obj is string)
			{
				return ((string)obj).Length != 0;
			}
			if (obj is double)
			{
				return (double)obj == (double)this.qyInput.CurrentPosition;
			}
			return !(obj is bool) || (bool)obj;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004F598 File Offset: 0x0004E598
		public override XPathNavigator MatchNode(XPathNavigator current)
		{
			if (current == null)
			{
				return null;
			}
			XPathNavigator xpathNavigator = this.qyInput.MatchNode(current);
			if (xpathNavigator != null)
			{
				switch (this.cond.StaticType)
				{
				case XPathResultType.Number:
				{
					OperandQuery operandQuery = this.cond as OperandQuery;
					if (operandQuery != null)
					{
						double num = (double)operandQuery.val;
						ChildrenQuery childrenQuery = this.qyInput as ChildrenQuery;
						if (childrenQuery != null)
						{
							XPathNavigator xpathNavigator2 = current.Clone();
							xpathNavigator2.MoveToParent();
							int num2 = 0;
							xpathNavigator2.MoveToFirstChild();
							for (;;)
							{
								if (childrenQuery.matches(xpathNavigator2))
								{
									num2++;
									if (current.IsSamePosition(xpathNavigator2))
									{
										break;
									}
								}
								if (!xpathNavigator2.MoveToNext())
								{
									goto Block_9;
								}
							}
							if (num != (double)num2)
							{
								return null;
							}
							return xpathNavigator;
							Block_9:
							return null;
						}
						AttributeQuery attributeQuery = this.qyInput as AttributeQuery;
						if (attributeQuery != null)
						{
							XPathNavigator xpathNavigator3 = current.Clone();
							xpathNavigator3.MoveToParent();
							int num3 = 0;
							xpathNavigator3.MoveToFirstAttribute();
							for (;;)
							{
								if (attributeQuery.matches(xpathNavigator3))
								{
									num3++;
									if (current.IsSamePosition(xpathNavigator3))
									{
										break;
									}
								}
								if (!xpathNavigator3.MoveToNextAttribute())
								{
									goto Block_14;
								}
							}
							if (num != (double)num3)
							{
								return null;
							}
							return xpathNavigator;
							Block_14:
							return null;
						}
					}
					break;
				}
				case XPathResultType.String:
					if (this.noPosition)
					{
						if (((string)this.cond.Evaluate(new XPathSingletonIterator(current, true))).Length == 0)
						{
							return null;
						}
						return xpathNavigator;
					}
					break;
				case XPathResultType.Boolean:
					if (this.noPosition)
					{
						if (!(bool)this.cond.Evaluate(new XPathSingletonIterator(current, true)))
						{
							return null;
						}
						return xpathNavigator;
					}
					break;
				case XPathResultType.NodeSet:
					this.cond.Evaluate(new XPathSingletonIterator(current, true));
					if (this.cond.Advance() == null)
					{
						return null;
					}
					return xpathNavigator;
				case (XPathResultType)4:
					return xpathNavigator;
				default:
					return null;
				}
				this.Evaluate(new XPathSingletonIterator(xpathNavigator, true));
				XPathNavigator xpathNavigator4;
				while ((xpathNavigator4 = this.Advance()) != null)
				{
					if (xpathNavigator4.IsSamePosition(current))
					{
						return xpathNavigator;
					}
				}
			}
			return null;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x0004F75F File Offset: 0x0004E75F
		public override QueryProps Properties
		{
			get
			{
				return QueryProps.Position | (this.qyInput.Properties & (QueryProps)24);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0004F771 File Offset: 0x0004E771
		public override XPathNodeIterator Clone()
		{
			return new FilterQuery(this);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004F77C File Offset: 0x0004E77C
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			if (!this.noPosition)
			{
				w.WriteAttributeString("position", "yes");
			}
			this.qyInput.PrintQuery(w);
			this.cond.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000B5E RID: 2910
		private Query cond;

		// Token: 0x04000B5F RID: 2911
		private bool noPosition;
	}
}

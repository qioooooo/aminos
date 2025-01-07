﻿using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	internal sealed class AbsoluteQuery : ContextQuery
	{
		public AbsoluteQuery()
		{
		}

		private AbsoluteQuery(AbsoluteQuery other)
			: base(other)
		{
		}

		public override object Evaluate(XPathNodeIterator context)
		{
			this.contextNode = context.Current.Clone();
			this.contextNode.MoveToRoot();
			this.count = 0;
			return this;
		}

		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			if (context != null && context.NodeType == XPathNodeType.Root)
			{
				return context;
			}
			return null;
		}

		public override XPathNodeIterator Clone()
		{
			return new AbsoluteQuery(this);
		}
	}
}

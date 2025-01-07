﻿using System;

namespace System.Xml.Schema
{
	internal class ForwardAxis
	{
		internal DoubleLinkAxis RootNode
		{
			get
			{
				return this.rootNode;
			}
		}

		internal DoubleLinkAxis TopNode
		{
			get
			{
				return this.topNode;
			}
		}

		internal bool IsAttribute
		{
			get
			{
				return this.isAttribute;
			}
		}

		internal bool IsDss
		{
			get
			{
				return this.isDss;
			}
		}

		internal bool IsSelfAxis
		{
			get
			{
				return this.isSelfAxis;
			}
		}

		public ForwardAxis(DoubleLinkAxis axis, bool isdesorself)
		{
			this.isDss = isdesorself;
			this.isAttribute = Asttree.IsAttribute(axis);
			this.topNode = axis;
			this.rootNode = axis;
			while (this.rootNode.Input != null)
			{
				this.rootNode = (DoubleLinkAxis)this.rootNode.Input;
			}
			this.isSelfAxis = Asttree.IsSelf(this.topNode);
		}

		private DoubleLinkAxis topNode;

		private DoubleLinkAxis rootNode;

		private bool isAttribute;

		private bool isDss;

		private bool isSelfAxis;
	}
}

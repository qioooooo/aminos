﻿using System;

namespace System.Xml.XPath
{
	internal class XmlEmptyNavigator : XPathNavigator
	{
		private XmlEmptyNavigator()
		{
		}

		public static XmlEmptyNavigator Singleton
		{
			get
			{
				if (XmlEmptyNavigator.singleton == null)
				{
					XmlEmptyNavigator.singleton = new XmlEmptyNavigator();
				}
				return XmlEmptyNavigator.singleton;
			}
		}

		public override XPathNodeType NodeType
		{
			get
			{
				return XPathNodeType.All;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return string.Empty;
			}
		}

		public override string LocalName
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Name
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Prefix
		{
			get
			{
				return string.Empty;
			}
		}

		public override string BaseURI
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Value
		{
			get
			{
				return string.Empty;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return false;
			}
		}

		public override string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return false;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return false;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return new NameTable();
			}
		}

		public override bool MoveToFirstChild()
		{
			return false;
		}

		public override void MoveToRoot()
		{
		}

		public override bool MoveToNext()
		{
			return false;
		}

		public override bool MoveToPrevious()
		{
			return false;
		}

		public override bool MoveToFirst()
		{
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			return false;
		}

		public override bool MoveToNextAttribute()
		{
			return false;
		}

		public override bool MoveToId(string id)
		{
			return false;
		}

		public override string GetAttribute(string localName, string namespaceName)
		{
			return null;
		}

		public override bool MoveToAttribute(string localName, string namespaceName)
		{
			return false;
		}

		public override string GetNamespace(string name)
		{
			return null;
		}

		public override bool MoveToNamespace(string prefix)
		{
			return false;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		public override bool MoveToParent()
		{
			return false;
		}

		public override bool MoveTo(XPathNavigator other)
		{
			return this == other;
		}

		public override XmlNodeOrder ComparePosition(XPathNavigator other)
		{
			if (this != other)
			{
				return XmlNodeOrder.Unknown;
			}
			return XmlNodeOrder.Same;
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			return this == other;
		}

		public override XPathNavigator Clone()
		{
			return this;
		}

		private static XmlEmptyNavigator singleton;
	}
}

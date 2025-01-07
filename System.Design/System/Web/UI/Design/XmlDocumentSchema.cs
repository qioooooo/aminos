using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class XmlDocumentSchema : IDataSourceSchema
	{
		public XmlDocumentSchema(XmlDocument xmlDocument, string xPath)
			: this(xmlDocument, xPath, false)
		{
		}

		internal XmlDocumentSchema(XmlDocument xmlDocument, string xPath, bool includeSpecialSchema)
		{
			if (xmlDocument == null)
			{
				throw new ArgumentNullException("xmlDocument");
			}
			this._includeSpecialSchema = includeSpecialSchema;
			this._rootSchema = new OrderedDictionary();
			XPathNavigator xpathNavigator = xmlDocument.CreateNavigator();
			if (!string.IsNullOrEmpty(xPath))
			{
				XPathNodeIterator xpathNodeIterator = xpathNavigator.Select(xPath);
				while (xpathNodeIterator.MoveNext())
				{
					XPathNavigator xpathNavigator2 = xpathNodeIterator.Current;
					XPathNodeIterator xpathNodeIterator2 = xpathNavigator2.SelectDescendants(XPathNodeType.Element, true);
					while (xpathNodeIterator2.MoveNext())
					{
						XPathNavigator xpathNavigator3 = xpathNodeIterator2.Current;
						this.AddSchemaElement(xpathNavigator3, xpathNodeIterator.Current);
					}
				}
				return;
			}
			XPathNodeIterator xpathNodeIterator3 = xpathNavigator.SelectDescendants(XPathNodeType.Element, true);
			while (xpathNodeIterator3.MoveNext())
			{
				XPathNavigator xpathNavigator4 = xpathNodeIterator3.Current;
				this.AddSchemaElement(xpathNavigator4, xpathNavigator);
			}
		}

		private void AddSchemaElement(XPathNavigator nav, XPathNavigator rootNav)
		{
			List<string> list = new List<string>();
			XPathNodeIterator xpathNodeIterator = nav.SelectAncestors(XPathNodeType.Element, true);
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathNodeIterator.Current;
				list.Add(xpathNavigator.Name);
				if (xpathNodeIterator.Current.IsSamePosition(rootNav))
				{
					break;
				}
			}
			list.Reverse();
			OrderedDictionary orderedDictionary = this._rootSchema;
			Pair pair = null;
			foreach (string text in list)
			{
				pair = orderedDictionary[text] as Pair;
				if (pair == null)
				{
					pair = new Pair(new OrderedDictionary(), new ArrayList());
					orderedDictionary.Add(text, pair);
				}
				orderedDictionary = (OrderedDictionary)pair.First;
			}
			this.AddAttributeList(nav, (ArrayList)pair.Second);
		}

		private void AddAttributeList(XPathNavigator nav, ArrayList attrs)
		{
			if (!nav.HasAttributes)
			{
				return;
			}
			nav.MoveToFirstAttribute();
			do
			{
				if (!attrs.Contains(nav.Name))
				{
					attrs.Add(nav.Name);
				}
			}
			while (nav.MoveToNextAttribute());
			nav.MoveToParent();
		}

		public IDataSourceViewSchema[] GetViews()
		{
			if (this._viewSchemas == null)
			{
				this._viewSchemas = new IDataSourceViewSchema[this._rootSchema.Count];
				int num = 0;
				foreach (object obj in this._rootSchema)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					this._viewSchemas[num] = new XmlDocumentViewSchema((string)dictionaryEntry.Key, (Pair)dictionaryEntry.Value, this._includeSpecialSchema);
					num++;
				}
			}
			return this._viewSchemas;
		}

		private OrderedDictionary _rootSchema;

		private IDataSourceViewSchema[] _viewSchemas;

		private bool _includeSpecialSchema;
	}
}

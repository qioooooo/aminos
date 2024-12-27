using System;
using System.Collections;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;

namespace System.Web.UI
{
	// Token: 0x02000490 RID: 1168
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class XPathBinder
	{
		// Token: 0x060036B3 RID: 14003 RVA: 0x000EC3E1 File Offset: 0x000EB3E1
		private XPathBinder()
		{
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000EC3EC File Offset: 0x000EB3EC
		public static object Eval(object container, string xPath)
		{
			IXmlNamespaceResolver xmlNamespaceResolver = null;
			return XPathBinder.Eval(container, xPath, xmlNamespaceResolver);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000EC404 File Offset: 0x000EB404
		public static object Eval(object container, string xPath, IXmlNamespaceResolver resolver)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (string.IsNullOrEmpty(xPath))
			{
				throw new ArgumentNullException("xPath");
			}
			IXPathNavigable ixpathNavigable = container as IXPathNavigable;
			if (ixpathNavigable == null)
			{
				throw new ArgumentException(SR.GetString("XPathBinder_MustBeIXPathNavigable", new object[] { container.GetType().FullName }));
			}
			XPathNavigator xpathNavigator = ixpathNavigable.CreateNavigator();
			object obj = xpathNavigator.Evaluate(xPath, resolver);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				if (xpathNodeIterator.MoveNext())
				{
					obj = xpathNodeIterator.Current.Value;
				}
				else
				{
					obj = null;
				}
			}
			return obj;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000EC498 File Offset: 0x000EB498
		public static string Eval(object container, string xPath, string format)
		{
			return XPathBinder.Eval(container, xPath, format, null);
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000EC4A4 File Offset: 0x000EB4A4
		public static string Eval(object container, string xPath, string format, IXmlNamespaceResolver resolver)
		{
			object obj = XPathBinder.Eval(container, xPath, resolver);
			if (obj == null)
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(format))
			{
				return obj.ToString();
			}
			return string.Format(format, obj);
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000EC4D9 File Offset: 0x000EB4D9
		public static IEnumerable Select(object container, string xPath)
		{
			return XPathBinder.Select(container, xPath, null);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000EC4E4 File Offset: 0x000EB4E4
		public static IEnumerable Select(object container, string xPath, IXmlNamespaceResolver resolver)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (string.IsNullOrEmpty(xPath))
			{
				throw new ArgumentNullException("xPath");
			}
			ArrayList arrayList = new ArrayList();
			IXPathNavigable ixpathNavigable = container as IXPathNavigable;
			if (ixpathNavigable == null)
			{
				throw new ArgumentException(SR.GetString("XPathBinder_MustBeIXPathNavigable", new object[] { container.GetType().FullName }));
			}
			XPathNavigator xpathNavigator = ixpathNavigable.CreateNavigator();
			XPathNodeIterator xpathNodeIterator = xpathNavigator.Select(xPath, resolver);
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator2 = xpathNodeIterator.Current;
				IHasXmlNode hasXmlNode = xpathNavigator2 as IHasXmlNode;
				if (hasXmlNode == null)
				{
					throw new InvalidOperationException(SR.GetString("XPathBinder_MustHaveXmlNodes"));
				}
				arrayList.Add(hasXmlNode.GetNode());
			}
			return arrayList;
		}
	}
}

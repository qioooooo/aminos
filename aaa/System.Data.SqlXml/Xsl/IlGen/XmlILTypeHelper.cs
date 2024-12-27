using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000043 RID: 67
	internal class XmlILTypeHelper
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0001EA68 File Offset: 0x0001DA68
		private XmlILTypeHelper()
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001EA70 File Offset: 0x0001DA70
		public static Type GetStorageType(XmlQueryType qyTyp)
		{
			Type type;
			if (qyTyp.IsSingleton)
			{
				type = XmlILTypeHelper.TypeCodeToStorage[(int)qyTyp.TypeCode];
				if (!qyTyp.IsStrict && type != typeof(XPathNavigator))
				{
					return typeof(XPathItem);
				}
			}
			else
			{
				type = XmlILTypeHelper.TypeCodeToCachedStorage[(int)qyTyp.TypeCode];
				if (!qyTyp.IsStrict && type != typeof(IList<XPathNavigator>))
				{
					return typeof(IList<XPathItem>);
				}
			}
			return type;
		}

		// Token: 0x04000379 RID: 889
		private static readonly Type[] TypeCodeToStorage = new Type[]
		{
			typeof(XPathItem),
			typeof(XPathItem),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathNavigator),
			typeof(XPathItem),
			typeof(string),
			typeof(string),
			typeof(bool),
			typeof(decimal),
			typeof(float),
			typeof(double),
			typeof(string),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(DateTime),
			typeof(byte[]),
			typeof(byte[]),
			typeof(string),
			typeof(XmlQualifiedName),
			typeof(XmlQualifiedName),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(long),
			typeof(decimal),
			typeof(decimal),
			typeof(long),
			typeof(int),
			typeof(int),
			typeof(int),
			typeof(decimal),
			typeof(decimal),
			typeof(long),
			typeof(int),
			typeof(int),
			typeof(decimal),
			typeof(TimeSpan),
			typeof(TimeSpan)
		};

		// Token: 0x0400037A RID: 890
		private static readonly Type[] TypeCodeToCachedStorage = new Type[]
		{
			typeof(IList<XPathItem>),
			typeof(IList<XPathItem>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathNavigator>),
			typeof(IList<XPathItem>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<bool>),
			typeof(IList<decimal>),
			typeof(IList<float>),
			typeof(IList<double>),
			typeof(IList<string>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<DateTime>),
			typeof(IList<byte[]>),
			typeof(IList<byte[]>),
			typeof(IList<string>),
			typeof(IList<XmlQualifiedName>),
			typeof(IList<XmlQualifiedName>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<string>),
			typeof(IList<long>),
			typeof(IList<decimal>),
			typeof(IList<decimal>),
			typeof(IList<long>),
			typeof(IList<int>),
			typeof(IList<int>),
			typeof(IList<int>),
			typeof(IList<decimal>),
			typeof(IList<decimal>),
			typeof(IList<long>),
			typeof(IList<int>),
			typeof(IList<int>),
			typeof(IList<decimal>),
			typeof(IList<TimeSpan>),
			typeof(IList<TimeSpan>)
		};
	}
}

using System;

namespace System.Xml
{
	internal class XmlNullResolver : XmlUrlResolver
	{
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			throw new XmlException("Xml_NullResolver", string.Empty);
		}

		public static readonly XmlNullResolver Singleton = new XmlNullResolver();
	}
}

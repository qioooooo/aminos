using System;
using System.Collections.Generic;

namespace System.Xml
{
	internal class XmlCharCheckingReaderWithNS : XmlCharCheckingReader, IXmlNamespaceResolver
	{
		internal XmlCharCheckingReaderWithNS(XmlReader reader, IXmlNamespaceResolver readerAsNSResolver, bool checkCharacters, bool ignoreWhitespace, bool ignoreComments, bool ignorePis, bool prohibitDtd)
			: base(reader, checkCharacters, ignoreWhitespace, ignoreComments, ignorePis, prohibitDtd)
		{
			this.readerAsNSResolver = readerAsNSResolver;
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.readerAsNSResolver.GetNamespacesInScope(scope);
		}

		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.readerAsNSResolver.LookupNamespace(prefix);
		}

		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.readerAsNSResolver.LookupPrefix(namespaceName);
		}

		internal IXmlNamespaceResolver readerAsNSResolver;
	}
}

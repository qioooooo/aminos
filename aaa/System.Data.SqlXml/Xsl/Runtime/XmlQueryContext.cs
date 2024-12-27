using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B1 RID: 177
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryContext
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x000289C8 File Offset: 0x000279C8
		internal XmlQueryContext(XmlQueryRuntime runtime, object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, WhitespaceRuleLookup wsRules)
		{
			this.runtime = runtime;
			this.dataSources = dataSources;
			this.dataSourceCache = new Hashtable();
			this.argList = argList;
			this.wsRules = wsRules;
			if (defaultDataSource is XmlReader)
			{
				this.readerSettings = new QueryReaderSettings((XmlReader)defaultDataSource);
			}
			else
			{
				this.readerSettings = new QueryReaderSettings(new NameTable());
			}
			if (defaultDataSource is string)
			{
				this.defaultDataSource = this.GetDataSource(defaultDataSource as string, null);
				if (this.defaultDataSource == null)
				{
					throw new XslTransformException("XmlIl_UnknownDocument", new string[] { defaultDataSource as string });
				}
			}
			else if (defaultDataSource != null)
			{
				this.defaultDataSource = this.ConstructDocument(defaultDataSource, null, null);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00028A80 File Offset: 0x00027A80
		public XmlNameTable QueryNameTable
		{
			get
			{
				return this.readerSettings.NameTable;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00028A8D File Offset: 0x00027A8D
		public XmlNameTable DefaultNameTable
		{
			get
			{
				if (this.defaultDataSource == null)
				{
					return null;
				}
				return this.defaultDataSource.NameTable;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00028AA4 File Offset: 0x00027AA4
		public XPathNavigator DefaultDataSource
		{
			get
			{
				if (this.defaultDataSource == null)
				{
					throw new XslTransformException("XmlIl_NoDefaultDocument", new string[] { string.Empty });
				}
				return this.defaultDataSource;
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00028ADC File Offset: 0x00027ADC
		public XPathNavigator GetDataSource(string uriRelative, string uriBase)
		{
			XPathNavigator xpathNavigator = null;
			try
			{
				Uri uri = ((uriBase != null) ? this.dataSources.ResolveUri(null, uriBase) : null);
				Uri uri2 = this.dataSources.ResolveUri(uri, uriRelative);
				if (uri2 != null)
				{
					xpathNavigator = this.dataSourceCache[uri2] as XPathNavigator;
				}
				if (xpathNavigator == null)
				{
					object entity = this.dataSources.GetEntity(uri2, null, null);
					if (entity != null)
					{
						xpathNavigator = this.ConstructDocument(entity, uriRelative, uri2);
						this.dataSourceCache.Add(uri2, xpathNavigator);
					}
				}
			}
			catch (XslTransformException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (!XmlException.IsCatchableException(ex))
				{
					throw;
				}
				throw new XslTransformException(ex, "XmlIl_DocumentLoadError", new string[] { uriRelative });
			}
			return xpathNavigator;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00028BA0 File Offset: 0x00027BA0
		private XPathNavigator ConstructDocument(object dataSource, string uriRelative, Uri uriResolved)
		{
			Stream stream = dataSource as Stream;
			if (stream != null)
			{
				XmlReader xmlReader = this.readerSettings.CreateReader(stream, (uriResolved != null) ? uriResolved.ToString() : null);
				try
				{
					return new XPathDocument(WhitespaceRuleReader.CreateReader(xmlReader, this.wsRules), XmlSpace.Preserve).CreateNavigator();
				}
				finally
				{
					xmlReader.Close();
				}
			}
			if (dataSource is XmlReader)
			{
				return new XPathDocument(WhitespaceRuleReader.CreateReader(dataSource as XmlReader, this.wsRules), XmlSpace.Preserve).CreateNavigator();
			}
			if (!(dataSource is IXPathNavigable))
			{
				throw new XslTransformException("XmlIl_CantResolveEntity", new string[]
				{
					uriRelative,
					dataSource.GetType().ToString()
				});
			}
			if (this.wsRules != null)
			{
				throw new XslTransformException("XmlIl_CantStripNav", new string[] { string.Empty });
			}
			return (dataSource as IXPathNavigable).CreateNavigator();
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00028C90 File Offset: 0x00027C90
		public object GetParameter(string localName, string namespaceUri)
		{
			if (this.argList == null)
			{
				return null;
			}
			return this.argList.GetParam(localName, namespaceUri);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00028CA9 File Offset: 0x00027CA9
		public object GetLateBoundObject(string namespaceUri)
		{
			if (this.argList == null)
			{
				return null;
			}
			return this.argList.GetExtensionObject(namespaceUri);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00028CC4 File Offset: 0x00027CC4
		public bool LateBoundFunctionExists(string name, string namespaceUri)
		{
			if (this.argList == null)
			{
				return false;
			}
			object extensionObject = this.argList.GetExtensionObject(namespaceUri);
			return extensionObject != null && new XmlExtensionFunction(name, namespaceUri, -1, extensionObject.GetType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).CanBind();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00028D04 File Offset: 0x00027D04
		public IList<XPathItem> InvokeXsltLateBoundFunction(string name, string namespaceUri, IList<XPathItem>[] args)
		{
			object obj = ((this.argList != null) ? this.argList.GetExtensionObject(namespaceUri) : null);
			if (obj == null)
			{
				throw new XslTransformException("Xslt_ScriptInvalidPrefix", new string[] { namespaceUri });
			}
			if (this.extFuncsLate == null)
			{
				this.extFuncsLate = new XmlExtensionFunctionTable();
			}
			XmlExtensionFunction xmlExtensionFunction = this.extFuncsLate.Bind(name, namespaceUri, args.Length, obj.GetType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			object[] array = new object[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				XmlQueryType xmlArgumentType = xmlExtensionFunction.GetXmlArgumentType(i);
				XmlTypeCode typeCode = xmlArgumentType.TypeCode;
				switch (typeCode)
				{
				case XmlTypeCode.Item:
					array[i] = args[i];
					break;
				case XmlTypeCode.Node:
					if (xmlArgumentType.IsSingleton)
					{
						array[i] = XsltConvert.ToNode(args[i]);
					}
					else
					{
						array[i] = XsltConvert.ToNodeSet(args[i]);
					}
					break;
				default:
					switch (typeCode)
					{
					case XmlTypeCode.String:
						array[i] = XsltConvert.ToString(args[i]);
						break;
					case XmlTypeCode.Boolean:
						array[i] = XsltConvert.ToBoolean(args[i]);
						break;
					case XmlTypeCode.Double:
						array[i] = XsltConvert.ToDouble(args[i]);
						break;
					}
					break;
				}
				Type clrArgumentType = xmlExtensionFunction.GetClrArgumentType(i);
				if (xmlArgumentType.TypeCode == XmlTypeCode.Item || !clrArgumentType.IsAssignableFrom(array[i].GetType()))
				{
					array[i] = this.runtime.ChangeTypeXsltArgument(xmlArgumentType, array[i], clrArgumentType);
				}
			}
			object obj2 = xmlExtensionFunction.Invoke(obj, array);
			if (obj2 == null && xmlExtensionFunction.ClrReturnType == XsltConvert.VoidType)
			{
				return XmlQueryNodeSequence.Empty;
			}
			return (IList<XPathItem>)this.runtime.ChangeTypeXsltResult(XmlQueryTypeFactory.ItemS, obj2);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00028EB0 File Offset: 0x00027EB0
		public void OnXsltMessageEncountered(string message)
		{
			XsltMessageEncounteredEventHandler xsltMessageEncounteredEventHandler = ((this.argList != null) ? this.argList.xsltMessageEncountered : null);
			if (xsltMessageEncounteredEventHandler != null)
			{
				xsltMessageEncounteredEventHandler(this, new XmlILQueryEventArgs(message));
				return;
			}
			Console.WriteLine(message);
		}

		// Token: 0x04000573 RID: 1395
		private XmlQueryRuntime runtime;

		// Token: 0x04000574 RID: 1396
		private XPathNavigator defaultDataSource;

		// Token: 0x04000575 RID: 1397
		private XmlResolver dataSources;

		// Token: 0x04000576 RID: 1398
		private Hashtable dataSourceCache;

		// Token: 0x04000577 RID: 1399
		private XsltArgumentList argList;

		// Token: 0x04000578 RID: 1400
		private XmlExtensionFunctionTable extFuncsLate;

		// Token: 0x04000579 RID: 1401
		private WhitespaceRuleLookup wsRules;

		// Token: 0x0400057A RID: 1402
		private QueryReaderSettings readerSettings;
	}
}

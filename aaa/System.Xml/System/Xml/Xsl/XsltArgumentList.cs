using System;
using System.Collections;

namespace System.Xml.Xsl
{
	// Token: 0x02000174 RID: 372
	public class XsltArgumentList
	{
		// Token: 0x060013E2 RID: 5090 RVA: 0x00055F33 File Offset: 0x00054F33
		public object GetParam(string name, string namespaceUri)
		{
			return this.parameters[new XmlQualifiedName(name, namespaceUri)];
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x00055F47 File Offset: 0x00054F47
		public object GetExtensionObject(string namespaceUri)
		{
			return this.extensions[namespaceUri];
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00055F58 File Offset: 0x00054F58
		public void AddParam(string name, string namespaceUri, object parameter)
		{
			XsltArgumentList.CheckArgumentNull(name, "name");
			XsltArgumentList.CheckArgumentNull(namespaceUri, "namespaceUri");
			XsltArgumentList.CheckArgumentNull(parameter, "parameter");
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, namespaceUri);
			xmlQualifiedName.Verify();
			this.parameters.Add(xmlQualifiedName, parameter);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00055FA1 File Offset: 0x00054FA1
		public void AddExtensionObject(string namespaceUri, object extension)
		{
			XsltArgumentList.CheckArgumentNull(namespaceUri, "namespaceUri");
			XsltArgumentList.CheckArgumentNull(extension, "extension");
			this.extensions.Add(namespaceUri, extension);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00055FC8 File Offset: 0x00054FC8
		public object RemoveParam(string name, string namespaceUri)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, namespaceUri);
			object obj = this.parameters[xmlQualifiedName];
			this.parameters.Remove(xmlQualifiedName);
			return obj;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00055FF8 File Offset: 0x00054FF8
		public object RemoveExtensionObject(string namespaceUri)
		{
			object obj = this.extensions[namespaceUri];
			this.extensions.Remove(namespaceUri);
			return obj;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060013E8 RID: 5096 RVA: 0x0005601F File Offset: 0x0005501F
		// (remove) Token: 0x060013E9 RID: 5097 RVA: 0x00056038 File Offset: 0x00055038
		public event XsltMessageEncounteredEventHandler XsltMessageEncountered
		{
			add
			{
				this.xsltMessageEncountered = (XsltMessageEncounteredEventHandler)Delegate.Combine(this.xsltMessageEncountered, value);
			}
			remove
			{
				this.xsltMessageEncountered = (XsltMessageEncounteredEventHandler)Delegate.Remove(this.xsltMessageEncountered, value);
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00056051 File Offset: 0x00055051
		public void Clear()
		{
			this.parameters.Clear();
			this.extensions.Clear();
			this.xsltMessageEncountered = null;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00056070 File Offset: 0x00055070
		private static void CheckArgumentNull(object param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x04000C3A RID: 3130
		private Hashtable parameters = new Hashtable();

		// Token: 0x04000C3B RID: 3131
		private Hashtable extensions = new Hashtable();

		// Token: 0x04000C3C RID: 3132
		internal XsltMessageEncounteredEventHandler xsltMessageEncountered;
	}
}

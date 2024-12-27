using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000732 RID: 1842
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public sealed class SoapTypeAttribute : SoapAttribute
	{
		// Token: 0x0600423F RID: 16959 RVA: 0x000E2377 File Offset: 0x000E1377
		internal bool IsInteropXmlElement()
		{
			return (this._explicitlySet & (SoapTypeAttribute.ExplicitlySet.XmlElementName | SoapTypeAttribute.ExplicitlySet.XmlNamespace)) != SoapTypeAttribute.ExplicitlySet.None;
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x000E2387 File Offset: 0x000E1387
		internal bool IsInteropXmlType()
		{
			return (this._explicitlySet & (SoapTypeAttribute.ExplicitlySet.XmlTypeName | SoapTypeAttribute.ExplicitlySet.XmlTypeNamespace)) != SoapTypeAttribute.ExplicitlySet.None;
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06004241 RID: 16961 RVA: 0x000E2398 File Offset: 0x000E1398
		// (set) Token: 0x06004242 RID: 16962 RVA: 0x000E23A0 File Offset: 0x000E13A0
		public SoapOption SoapOptions
		{
			get
			{
				return this._SoapOptions;
			}
			set
			{
				this._SoapOptions = value;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06004243 RID: 16963 RVA: 0x000E23A9 File Offset: 0x000E13A9
		// (set) Token: 0x06004244 RID: 16964 RVA: 0x000E23D7 File Offset: 0x000E13D7
		public string XmlElementName
		{
			get
			{
				if (this._XmlElementName == null && this.ReflectInfo != null)
				{
					this._XmlElementName = SoapTypeAttribute.GetTypeName((Type)this.ReflectInfo);
				}
				return this._XmlElementName;
			}
			set
			{
				this._XmlElementName = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlElementName;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004245 RID: 16965 RVA: 0x000E23EE File Offset: 0x000E13EE
		// (set) Token: 0x06004246 RID: 16966 RVA: 0x000E2412 File Offset: 0x000E1412
		public override string XmlNamespace
		{
			get
			{
				if (this.ProtXmlNamespace == null && this.ReflectInfo != null)
				{
					this.ProtXmlNamespace = this.XmlTypeNamespace;
				}
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlNamespace;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004247 RID: 16967 RVA: 0x000E2429 File Offset: 0x000E1429
		// (set) Token: 0x06004248 RID: 16968 RVA: 0x000E2457 File Offset: 0x000E1457
		public string XmlTypeName
		{
			get
			{
				if (this._XmlTypeName == null && this.ReflectInfo != null)
				{
					this._XmlTypeName = SoapTypeAttribute.GetTypeName((Type)this.ReflectInfo);
				}
				return this._XmlTypeName;
			}
			set
			{
				this._XmlTypeName = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlTypeName;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004249 RID: 16969 RVA: 0x000E246E File Offset: 0x000E146E
		// (set) Token: 0x0600424A RID: 16970 RVA: 0x000E249D File Offset: 0x000E149D
		public string XmlTypeNamespace
		{
			get
			{
				if (this._XmlTypeNamespace == null && this.ReflectInfo != null)
				{
					this._XmlTypeNamespace = XmlNamespaceEncoder.GetXmlNamespaceForTypeNamespace((Type)this.ReflectInfo, null);
				}
				return this._XmlTypeNamespace;
			}
			set
			{
				this._XmlTypeNamespace = value;
				this._explicitlySet |= SoapTypeAttribute.ExplicitlySet.XmlTypeNamespace;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600424B RID: 16971 RVA: 0x000E24B4 File Offset: 0x000E14B4
		// (set) Token: 0x0600424C RID: 16972 RVA: 0x000E24BC File Offset: 0x000E14BC
		public XmlFieldOrderOption XmlFieldOrder
		{
			get
			{
				return this._XmlFieldOrder;
			}
			set
			{
				this._XmlFieldOrder = value;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x000E24C5 File Offset: 0x000E14C5
		// (set) Token: 0x0600424E RID: 16974 RVA: 0x000E24C8 File Offset: 0x000E14C8
		public override bool UseAttribute
		{
			get
			{
				return false;
			}
			set
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Attribute_UseAttributeNotsettable"));
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x000E24DC File Offset: 0x000E14DC
		private static string GetTypeName(Type t)
		{
			if (!t.IsNested)
			{
				return t.Name;
			}
			string fullName = t.FullName;
			string @namespace = t.Namespace;
			if (@namespace == null || @namespace.Length == 0)
			{
				return fullName;
			}
			return fullName.Substring(@namespace.Length + 1);
		}

		// Token: 0x04002120 RID: 8480
		private SoapTypeAttribute.ExplicitlySet _explicitlySet;

		// Token: 0x04002121 RID: 8481
		private SoapOption _SoapOptions;

		// Token: 0x04002122 RID: 8482
		private string _XmlElementName;

		// Token: 0x04002123 RID: 8483
		private string _XmlTypeName;

		// Token: 0x04002124 RID: 8484
		private string _XmlTypeNamespace;

		// Token: 0x04002125 RID: 8485
		private XmlFieldOrderOption _XmlFieldOrder;

		// Token: 0x02000733 RID: 1843
		[Flags]
		[Serializable]
		private enum ExplicitlySet
		{
			// Token: 0x04002127 RID: 8487
			None = 0,
			// Token: 0x04002128 RID: 8488
			XmlElementName = 1,
			// Token: 0x04002129 RID: 8489
			XmlNamespace = 2,
			// Token: 0x0400212A RID: 8490
			XmlTypeName = 4,
			// Token: 0x0400212B RID: 8491
			XmlTypeNamespace = 8
		}
	}
}

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000734 RID: 1844
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class SoapMethodAttribute : SoapAttribute
	{
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x000E252B File Offset: 0x000E152B
		internal bool SoapActionExplicitySet
		{
			get
			{
				return this._bSoapActionExplicitySet;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x000E2533 File Offset: 0x000E1533
		// (set) Token: 0x06004253 RID: 16979 RVA: 0x000E2569 File Offset: 0x000E1569
		public string SoapAction
		{
			get
			{
				if (this._SoapAction == null)
				{
					this._SoapAction = this.XmlTypeNamespaceOfDeclaringType + "#" + ((MemberInfo)this.ReflectInfo).Name;
				}
				return this._SoapAction;
			}
			set
			{
				this._SoapAction = value;
				this._bSoapActionExplicitySet = true;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x000E2579 File Offset: 0x000E1579
		// (set) Token: 0x06004255 RID: 16981 RVA: 0x000E257C File Offset: 0x000E157C
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

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004256 RID: 16982 RVA: 0x000E258D File Offset: 0x000E158D
		// (set) Token: 0x06004257 RID: 16983 RVA: 0x000E25A9 File Offset: 0x000E15A9
		public override string XmlNamespace
		{
			get
			{
				if (this.ProtXmlNamespace == null)
				{
					this.ProtXmlNamespace = this.XmlTypeNamespaceOfDeclaringType;
				}
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004258 RID: 16984 RVA: 0x000E25B2 File Offset: 0x000E15B2
		// (set) Token: 0x06004259 RID: 16985 RVA: 0x000E25EA File Offset: 0x000E15EA
		public string ResponseXmlElementName
		{
			get
			{
				if (this._responseXmlElementName == null && this.ReflectInfo != null)
				{
					this._responseXmlElementName = ((MemberInfo)this.ReflectInfo).Name + "Response";
				}
				return this._responseXmlElementName;
			}
			set
			{
				this._responseXmlElementName = value;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600425A RID: 16986 RVA: 0x000E25F3 File Offset: 0x000E15F3
		// (set) Token: 0x0600425B RID: 16987 RVA: 0x000E260F File Offset: 0x000E160F
		public string ResponseXmlNamespace
		{
			get
			{
				if (this._responseXmlNamespace == null)
				{
					this._responseXmlNamespace = this.XmlNamespace;
				}
				return this._responseXmlNamespace;
			}
			set
			{
				this._responseXmlNamespace = value;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x000E2618 File Offset: 0x000E1618
		// (set) Token: 0x0600425D RID: 16989 RVA: 0x000E2633 File Offset: 0x000E1633
		public string ReturnXmlElementName
		{
			get
			{
				if (this._returnXmlElementName == null)
				{
					this._returnXmlElementName = "return";
				}
				return this._returnXmlElementName;
			}
			set
			{
				this._returnXmlElementName = value;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x000E263C File Offset: 0x000E163C
		private string XmlTypeNamespaceOfDeclaringType
		{
			get
			{
				if (this.ReflectInfo != null)
				{
					Type declaringType = ((MemberInfo)this.ReflectInfo).DeclaringType;
					return XmlNamespaceEncoder.GetXmlNamespaceForType(declaringType, null);
				}
				return null;
			}
		}

		// Token: 0x0400212C RID: 8492
		private string _SoapAction;

		// Token: 0x0400212D RID: 8493
		private string _responseXmlElementName;

		// Token: 0x0400212E RID: 8494
		private string _responseXmlNamespace;

		// Token: 0x0400212F RID: 8495
		private string _returnXmlElementName;

		// Token: 0x04002130 RID: 8496
		private bool _bSoapActionExplicitySet;
	}
}

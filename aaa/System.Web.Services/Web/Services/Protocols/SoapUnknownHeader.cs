using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000085 RID: 133
	public sealed class SoapUnknownHeader : SoapHeader
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00011924 File Offset: 0x00010924
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00011A5C File Offset: 0x00010A5C
		[XmlIgnore]
		public XmlElement Element
		{
			get
			{
				if (this.element == null)
				{
					return null;
				}
				if (this.version == SoapProtocolVersion.Soap12)
				{
					if (this.InternalMustUnderstand)
					{
						this.element.SetAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", "1");
					}
					this.element.RemoveAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/");
					string internalActor = this.InternalActor;
					if (internalActor != null && internalActor.Length != 0)
					{
						this.element.SetAttribute("role", "http://www.w3.org/2003/05/soap-envelope", internalActor);
					}
					this.element.RemoveAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/");
				}
				else if (this.version == SoapProtocolVersion.Soap11)
				{
					if (this.InternalMustUnderstand)
					{
						this.element.SetAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", "1");
					}
					this.element.RemoveAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope");
					string internalActor2 = this.InternalActor;
					if (internalActor2 != null && internalActor2.Length != 0)
					{
						this.element.SetAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/", internalActor2);
					}
					this.element.RemoveAttribute("role", "http://www.w3.org/2003/05/soap-envelope");
					this.element.RemoveAttribute("relay", "http://www.w3.org/2003/05/soap-envelope");
				}
				return this.element;
			}
			set
			{
				if (value == null && this.element != null)
				{
					base.InternalMustUnderstand = this.InternalMustUnderstand;
					base.InternalActor = this.InternalActor;
				}
				this.element = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00011A88 File Offset: 0x00010A88
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00011B1C File Offset: 0x00010B1C
		internal override bool InternalMustUnderstand
		{
			get
			{
				if (this.element == null)
				{
					return base.InternalMustUnderstand;
				}
				string text = this.GetElementAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", this.element);
				if (text == null)
				{
					text = this.GetElementAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", this.element);
					if (text == null)
					{
						return false;
					}
				}
				string text2;
				if ((text2 = text) != null)
				{
					if (text2 == "false" || text2 == "0")
					{
						return false;
					}
					if (text2 == "true" || text2 == "1")
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				base.InternalMustUnderstand = value;
				if (this.element != null)
				{
					if (value)
					{
						this.element.SetAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", "1");
					}
					else
					{
						this.element.RemoveAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/");
					}
					this.element.RemoveAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope");
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00011B84 File Offset: 0x00010B84
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00011BDC File Offset: 0x00010BDC
		internal override string InternalActor
		{
			get
			{
				if (this.element == null)
				{
					return base.InternalActor;
				}
				string text = this.GetElementAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/", this.element);
				if (text == null)
				{
					text = this.GetElementAttribute("role", "http://www.w3.org/2003/05/soap-envelope", this.element);
					if (text == null)
					{
						return "";
					}
				}
				return text;
			}
			set
			{
				base.InternalActor = value;
				if (this.element != null)
				{
					if (value == null || value.Length == 0)
					{
						this.element.RemoveAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/");
					}
					else
					{
						this.element.SetAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/", value);
					}
					this.element.RemoveAttribute("role", "http://www.w3.org/2003/05/soap-envelope");
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00011C48 File Offset: 0x00010C48
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00011CC0 File Offset: 0x00010CC0
		internal override bool InternalRelay
		{
			get
			{
				if (this.element == null)
				{
					return base.InternalRelay;
				}
				string elementAttribute = this.GetElementAttribute("relay", "http://www.w3.org/2003/05/soap-envelope", this.element);
				if (elementAttribute == null)
				{
					return false;
				}
				string text;
				if ((text = elementAttribute) != null)
				{
					if (text == "false" || text == "0")
					{
						return false;
					}
					if (text == "true" || text == "1")
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				base.InternalRelay = value;
				if (this.element != null)
				{
					if (value)
					{
						this.element.SetAttribute("relay", "http://www.w3.org/2003/05/soap-envelope", "1");
						return;
					}
					this.element.RemoveAttribute("relay", "http://www.w3.org/2003/05/soap-envelope");
				}
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00011D10 File Offset: 0x00010D10
		private string GetElementAttribute(string name, string ns, XmlElement element)
		{
			if (element == null)
			{
				return null;
			}
			if (element.Prefix.Length == 0 && element.NamespaceURI == ns)
			{
				if (element.HasAttribute(name))
				{
					return element.GetAttribute(name);
				}
				return null;
			}
			else
			{
				if (element.HasAttribute(name, ns))
				{
					return element.GetAttribute(name, ns);
				}
				return null;
			}
		}

		// Token: 0x0400038A RID: 906
		private XmlElement element;
	}
}

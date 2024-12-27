using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006C RID: 108
	[SoapType(IncludeInSchema = false)]
	[XmlType(IncludeInSchema = false)]
	public abstract class SoapHeader
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000D7B5 File Offset: 0x0000C7B5
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000D7D4 File Offset: 0x0000C7D4
		[SoapAttribute("mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		[DefaultValue("0")]
		[XmlAttribute("mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public string EncodedMustUnderstand
		{
			get
			{
				if (this.version == SoapProtocolVersion.Soap12 || !this.MustUnderstand)
				{
					return "0";
				}
				return "1";
			}
			set
			{
				if (value != null)
				{
					if (value == "false" || value == "0")
					{
						this.MustUnderstand = false;
						return;
					}
					if (value == "true" || value == "1")
					{
						this.MustUnderstand = true;
						return;
					}
				}
				throw new ArgumentException(Res.GetString("WebHeaderInvalidMustUnderstand", new object[] { value }));
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000D847 File Offset: 0x0000C847
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000D865 File Offset: 0x0000C865
		[XmlAttribute("mustUnderstand", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		[SoapAttribute("mustUnderstand", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		[DefaultValue("0")]
		[ComVisible(false)]
		public string EncodedMustUnderstand12
		{
			get
			{
				if (this.version == SoapProtocolVersion.Soap11 || !this.MustUnderstand)
				{
					return "0";
				}
				return "1";
			}
			set
			{
				this.EncodedMustUnderstand = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000D86E File Offset: 0x0000C86E
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000D876 File Offset: 0x0000C876
		[SoapIgnore]
		[XmlIgnore]
		public bool MustUnderstand
		{
			get
			{
				return this.InternalMustUnderstand;
			}
			set
			{
				this.InternalMustUnderstand = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000D87F File Offset: 0x0000C87F
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000D887 File Offset: 0x0000C887
		internal virtual bool InternalMustUnderstand
		{
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			get
			{
				return this.mustUnderstand;
			}
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			set
			{
				this.mustUnderstand = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000D890 File Offset: 0x0000C890
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000D8A7 File Offset: 0x0000C8A7
		[XmlAttribute("actor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		[SoapAttribute("actor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		[DefaultValue("")]
		public string Actor
		{
			get
			{
				if (this.version == SoapProtocolVersion.Soap12)
				{
					return "";
				}
				return this.InternalActor;
			}
			set
			{
				this.InternalActor = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000D8B0 File Offset: 0x0000C8B0
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000D8C7 File Offset: 0x0000C8C7
		[ComVisible(false)]
		[XmlAttribute("role", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		[DefaultValue("")]
		[SoapAttribute("role", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		public string Role
		{
			get
			{
				if (this.version == SoapProtocolVersion.Soap11)
				{
					return "";
				}
				return this.InternalActor;
			}
			set
			{
				this.InternalActor = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000D8D0 File Offset: 0x0000C8D0
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000D8E6 File Offset: 0x0000C8E6
		internal virtual string InternalActor
		{
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			get
			{
				if (this.actor != null)
				{
					return this.actor;
				}
				return string.Empty;
			}
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			set
			{
				this.actor = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000D8EF File Offset: 0x0000C8EF
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000D8F7 File Offset: 0x0000C8F7
		[XmlIgnore]
		[SoapIgnore]
		public bool DidUnderstand
		{
			get
			{
				return this.didUnderstand;
			}
			set
			{
				this.didUnderstand = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000D900 File Offset: 0x0000C900
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000D920 File Offset: 0x0000C920
		[SoapAttribute("relay", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		[DefaultValue("0")]
		[XmlAttribute("relay", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
		[ComVisible(false)]
		public string EncodedRelay
		{
			get
			{
				if (this.version == SoapProtocolVersion.Soap11 || !this.Relay)
				{
					return "0";
				}
				return "1";
			}
			set
			{
				if (value != null)
				{
					if (value == "false" || value == "0")
					{
						this.Relay = false;
						return;
					}
					if (value == "true" || value == "1")
					{
						this.Relay = true;
						return;
					}
				}
				throw new ArgumentException(Res.GetString("WebHeaderInvalidRelay", new object[] { value }));
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000D993 File Offset: 0x0000C993
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000D99B File Offset: 0x0000C99B
		[ComVisible(false)]
		[XmlIgnore]
		[SoapIgnore]
		public bool Relay
		{
			get
			{
				return this.InternalRelay;
			}
			set
			{
				this.InternalRelay = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000D9A4 File Offset: 0x0000C9A4
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000D9AC File Offset: 0x0000C9AC
		internal virtual bool InternalRelay
		{
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			get
			{
				return this.relay;
			}
			[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
			set
			{
				this.relay = value;
			}
		}

		// Token: 0x04000322 RID: 802
		private string actor;

		// Token: 0x04000323 RID: 803
		private bool mustUnderstand;

		// Token: 0x04000324 RID: 804
		private bool didUnderstand;

		// Token: 0x04000325 RID: 805
		private bool relay;

		// Token: 0x04000326 RID: 806
		internal SoapProtocolVersion version;
	}
}

using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000063 RID: 99
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class Pkcs9AttributeObject : AsnEncodedData
	{
		// Token: 0x0600010D RID: 269 RVA: 0x000066E6 File Offset: 0x000056E6
		internal Pkcs9AttributeObject(string oid)
		{
			base.Oid = new Oid(oid);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000066FA File Offset: 0x000056FA
		public Pkcs9AttributeObject()
		{
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006702 File Offset: 0x00005702
		public Pkcs9AttributeObject(string oid, byte[] encodedData)
			: this(new AsnEncodedData(oid, encodedData))
		{
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006711 File Offset: 0x00005711
		public Pkcs9AttributeObject(Oid oid, byte[] encodedData)
			: this(new AsnEncodedData(oid, encodedData))
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006720 File Offset: 0x00005720
		public Pkcs9AttributeObject(AsnEncodedData asnEncodedData)
			: base(asnEncodedData)
		{
			if (asnEncodedData.Oid == null)
			{
				throw new ArgumentNullException("asnEncodedData.Oid");
			}
			string value = base.Oid.Value;
			if (value == null)
			{
				throw new ArgumentNullException("oid.Value");
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_EmptyOrNullString"), "oid.Value");
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000677E File Offset: 0x0000577E
		public new Oid Oid
		{
			get
			{
				return base.Oid;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006788 File Offset: 0x00005788
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (!(asnEncodedData is Pkcs9AttributeObject))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Pkcs9_AttributeMismatch"));
			}
			base.CopyFrom(asnEncodedData);
		}
	}
}

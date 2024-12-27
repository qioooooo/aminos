using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000496 RID: 1174
	[ComVisible(true)]
	[Serializable]
	public sealed class PermissionRequestEvidence : IBuiltInEvidence
	{
		// Token: 0x06002F25 RID: 12069 RVA: 0x000A0E20 File Offset: 0x0009FE20
		public PermissionRequestEvidence(PermissionSet request, PermissionSet optional, PermissionSet denied)
		{
			if (request == null)
			{
				this.m_request = null;
			}
			else
			{
				this.m_request = request.Copy();
			}
			if (optional == null)
			{
				this.m_optional = null;
			}
			else
			{
				this.m_optional = optional.Copy();
			}
			if (denied == null)
			{
				this.m_denied = null;
				return;
			}
			this.m_denied = denied.Copy();
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000A0E7A File Offset: 0x0009FE7A
		internal PermissionRequestEvidence()
		{
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002F27 RID: 12071 RVA: 0x000A0E82 File Offset: 0x0009FE82
		public PermissionSet RequestedPermissions
		{
			get
			{
				return this.m_request;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002F28 RID: 12072 RVA: 0x000A0E8A File Offset: 0x0009FE8A
		public PermissionSet OptionalPermissions
		{
			get
			{
				return this.m_optional;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002F29 RID: 12073 RVA: 0x000A0E92 File Offset: 0x0009FE92
		public PermissionSet DeniedPermissions
		{
			get
			{
				return this.m_denied;
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000A0E9A File Offset: 0x0009FE9A
		public PermissionRequestEvidence Copy()
		{
			return new PermissionRequestEvidence(this.m_request, this.m_optional, this.m_denied);
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000A0EB4 File Offset: 0x0009FEB4
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.PermissionRequestEvidence");
			securityElement.AddAttribute("version", "1");
			if (this.m_request != null)
			{
				SecurityElement securityElement2 = new SecurityElement("Request");
				securityElement2.AddChild(this.m_request.ToXml());
				securityElement.AddChild(securityElement2);
			}
			if (this.m_optional != null)
			{
				SecurityElement securityElement2 = new SecurityElement("Optional");
				securityElement2.AddChild(this.m_optional.ToXml());
				securityElement.AddChild(securityElement2);
			}
			if (this.m_denied != null)
			{
				SecurityElement securityElement2 = new SecurityElement("Denied");
				securityElement2.AddChild(this.m_denied.ToXml());
				securityElement.AddChild(securityElement2);
			}
			return securityElement;
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000A0F60 File Offset: 0x0009FF60
		internal void CreateStrings()
		{
			if (this.m_strRequest == null && this.m_request != null)
			{
				this.m_strRequest = this.m_request.ToXml().ToString();
			}
			if (this.m_strOptional == null && this.m_optional != null)
			{
				this.m_strOptional = this.m_optional.ToXml().ToString();
			}
			if (this.m_strDenied == null && this.m_denied != null)
			{
				this.m_strDenied = this.m_denied.ToXml().ToString();
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000A0FE0 File Offset: 0x0009FFE0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			this.CreateStrings();
			int num = 0;
			int num2 = 0;
			int num3 = position + 1;
			buffer[position] = '\a';
			if (verbose)
			{
				num = num3;
				num3 += 2;
			}
			if (this.m_strRequest != null)
			{
				int num4 = this.m_strRequest.Length;
				if (verbose)
				{
					buffer[num3++] = '\0';
					BuiltInEvidenceHelper.CopyIntToCharArray(num4, buffer, num3);
					num3 += 2;
					num2++;
				}
				this.m_strRequest.CopyTo(0, buffer, num3, num4);
				num3 += num4;
			}
			if (this.m_strOptional != null)
			{
				int num4 = this.m_strOptional.Length;
				if (verbose)
				{
					buffer[num3++] = '\u0001';
					BuiltInEvidenceHelper.CopyIntToCharArray(num4, buffer, num3);
					num3 += 2;
					num2++;
				}
				this.m_strOptional.CopyTo(0, buffer, num3, num4);
				num3 += num4;
			}
			if (this.m_strDenied != null)
			{
				int num4 = this.m_strDenied.Length;
				if (verbose)
				{
					buffer[num3++] = '\u0002';
					BuiltInEvidenceHelper.CopyIntToCharArray(num4, buffer, num3);
					num3 += 2;
					num2++;
				}
				this.m_strDenied.CopyTo(0, buffer, num3, num4);
				num3 += num4;
			}
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(num2, buffer, num);
			}
			return num3;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000A10DC File Offset: 0x000A00DC
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			this.CreateStrings();
			int num = 1;
			if (this.m_strRequest != null)
			{
				if (verbose)
				{
					num += 3;
				}
				num += this.m_strRequest.Length;
			}
			if (this.m_strOptional != null)
			{
				if (verbose)
				{
					num += 3;
				}
				num += this.m_strOptional.Length;
			}
			if (this.m_strDenied != null)
			{
				if (verbose)
				{
					num += 3;
				}
				num += this.m_strDenied.Length;
			}
			if (verbose)
			{
				num += 2;
			}
			return num;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000A1150 File Offset: 0x000A0150
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			for (int i = 0; i < intFromCharArray; i++)
			{
				char c = buffer[position++];
				int intFromCharArray2 = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
				position += 2;
				string text = new string(buffer, position, intFromCharArray2);
				position += intFromCharArray2;
				Parser parser = new Parser(text);
				PermissionSet permissionSet = new PermissionSet();
				permissionSet.FromXml(parser.GetTopElement());
				switch (c)
				{
				case '\0':
					this.m_strRequest = text;
					this.m_request = permissionSet;
					break;
				case '\u0001':
					this.m_strOptional = text;
					this.m_optional = permissionSet;
					break;
				case '\u0002':
					this.m_strDenied = text;
					this.m_denied = permissionSet;
					break;
				default:
					throw new SerializationException(Environment.GetResourceString("Serialization_UnableToFixup"));
				}
			}
			return position;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000A121E File Offset: 0x000A021E
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x040017D1 RID: 6097
		private const char idRequest = '\0';

		// Token: 0x040017D2 RID: 6098
		private const char idOptional = '\u0001';

		// Token: 0x040017D3 RID: 6099
		private const char idDenied = '\u0002';

		// Token: 0x040017D4 RID: 6100
		private PermissionSet m_request;

		// Token: 0x040017D5 RID: 6101
		private PermissionSet m_optional;

		// Token: 0x040017D6 RID: 6102
		private PermissionSet m_denied;

		// Token: 0x040017D7 RID: 6103
		private string m_strRequest;

		// Token: 0x040017D8 RID: 6104
		private string m_strOptional;

		// Token: 0x040017D9 RID: 6105
		private string m_strDenied;
	}
}

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A1 RID: 1185
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongName : IIdentityPermissionFactory, IBuiltInEvidence, IDelayEvaluatedEvidence
	{
		// Token: 0x06002FC0 RID: 12224 RVA: 0x000A4927 File Offset: 0x000A3927
		internal StrongName()
		{
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000A492F File Offset: 0x000A392F
		public StrongName(StrongNamePublicKeyBlob blob, string name, Version version)
			: this(blob, name, version, null)
		{
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000A493C File Offset: 0x000A393C
		internal StrongName(StrongNamePublicKeyBlob blob, string name, Version version, Assembly assembly)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyStrongName"));
			}
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this.m_publicKeyBlob = blob;
			this.m_name = name;
			this.m_version = version;
			this.m_assembly = assembly;
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000A49B4 File Offset: 0x000A39B4
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				return this.m_publicKeyBlob;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000A49BC File Offset: 0x000A39BC
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x000A49C4 File Offset: 0x000A39C4
		public Version Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000A49CC File Offset: 0x000A39CC
		bool IDelayEvaluatedEvidence.IsVerified
		{
			get
			{
				return this.m_assembly == null || this.m_assembly.IsStrongNameVerified();
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000A49E3 File Offset: 0x000A39E3
		bool IDelayEvaluatedEvidence.WasUsed
		{
			get
			{
				return this.m_wasUsed;
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000A49EB File Offset: 0x000A39EB
		void IDelayEvaluatedEvidence.MarkUsed()
		{
			this.m_wasUsed = true;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000A49F4 File Offset: 0x000A39F4
		internal static bool CompareNames(string asmName, string mcName)
		{
			if (mcName.Length > 0 && mcName[mcName.Length - 1] == '*' && mcName.Length - 1 <= asmName.Length)
			{
				return string.Compare(mcName, 0, asmName, 0, mcName.Length - 1, StringComparison.OrdinalIgnoreCase) == 0;
			}
			return string.Compare(mcName, asmName, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000A4A4D File Offset: 0x000A3A4D
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new StrongNameIdentityPermission(this.m_publicKeyBlob, this.m_name, this.m_version);
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000A4A66 File Offset: 0x000A3A66
		public object Copy()
		{
			return new StrongName(this.m_publicKeyBlob, this.m_name, this.m_version);
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000A4A80 File Offset: 0x000A3A80
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("StrongName");
			securityElement.AddAttribute("version", "1");
			if (this.m_publicKeyBlob != null)
			{
				securityElement.AddAttribute("Key", Hex.EncodeHexString(this.m_publicKeyBlob.PublicKey));
			}
			if (this.m_name != null)
			{
				securityElement.AddAttribute("Name", this.m_name);
			}
			if (this.m_version != null)
			{
				securityElement.AddAttribute("Version", this.m_version.ToString());
			}
			return securityElement;
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000A4B0C File Offset: 0x000A3B0C
		internal void FromXml(SecurityElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (string.Compare(element.Tag, "StrongName", StringComparison.Ordinal) != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXML"));
			}
			this.m_publicKeyBlob = null;
			this.m_version = null;
			string text = element.Attribute("Key");
			if (text != null)
			{
				this.m_publicKeyBlob = new StrongNamePublicKeyBlob(Hex.DecodeHexString(text));
			}
			this.m_name = element.Attribute("Name");
			string text2 = element.Attribute("Version");
			if (text2 != null)
			{
				this.m_version = new Version(text2);
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000A4BA4 File Offset: 0x000A3BA4
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000A4BB4 File Offset: 0x000A3BB4
		public override bool Equals(object o)
		{
			StrongName strongName = o as StrongName;
			return strongName != null && object.Equals(this.m_publicKeyBlob, strongName.m_publicKeyBlob) && object.Equals(this.m_name, strongName.m_name) && object.Equals(this.m_version, strongName.m_version);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000A4C04 File Offset: 0x000A3C04
		public override int GetHashCode()
		{
			if (this.m_publicKeyBlob != null)
			{
				return this.m_publicKeyBlob.GetHashCode();
			}
			if (this.m_name != null || this.m_version != null)
			{
				return ((this.m_name == null) ? 0 : this.m_name.GetHashCode()) + ((this.m_version == null) ? 0 : this.m_version.GetHashCode());
			}
			return typeof(StrongName).GetHashCode();
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000A4C80 File Offset: 0x000A3C80
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0002';
			int num = this.m_publicKeyBlob.PublicKey.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(num, buffer, position);
				position += 2;
			}
			Buffer.InternalBlockCopy(this.m_publicKeyBlob.PublicKey, 0, buffer, position * 2, num);
			position += (num - 1) / 2 + 1;
			BuiltInEvidenceHelper.CopyIntToCharArray(this.m_version.Major, buffer, position);
			BuiltInEvidenceHelper.CopyIntToCharArray(this.m_version.Minor, buffer, position + 2);
			BuiltInEvidenceHelper.CopyIntToCharArray(this.m_version.Build, buffer, position + 4);
			BuiltInEvidenceHelper.CopyIntToCharArray(this.m_version.Revision, buffer, position + 6);
			position += 8;
			int length = this.m_name.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			this.m_name.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000A4D58 File Offset: 0x000A3D58
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			int num = (this.m_publicKeyBlob.PublicKey.Length - 1) / 2 + 1;
			if (verbose)
			{
				num += 2;
			}
			num += 8;
			num += this.m_name.Length;
			if (verbose)
			{
				num += 2;
			}
			return num + 1;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000A4DA0 File Offset: 0x000A3DA0
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int num = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_publicKeyBlob = new StrongNamePublicKeyBlob();
			this.m_publicKeyBlob.PublicKey = new byte[num];
			int num2 = (num - 1) / 2 + 1;
			Buffer.InternalBlockCopy(buffer, position * 2, this.m_publicKeyBlob.PublicKey, 0, num);
			position += num2;
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			int intFromCharArray2 = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position + 2);
			int intFromCharArray3 = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position + 4);
			int intFromCharArray4 = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position + 6);
			this.m_version = new Version(intFromCharArray, intFromCharArray2, intFromCharArray3, intFromCharArray4);
			position += 8;
			num = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_name = new string(buffer, position, num);
			return position + num;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000A4E58 File Offset: 0x000A3E58
		internal object Normalize()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.m_publicKeyBlob.PublicKey);
			binaryWriter.Write(this.m_version.Major);
			binaryWriter.Write(this.m_name);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x0400180A RID: 6154
		private StrongNamePublicKeyBlob m_publicKeyBlob;

		// Token: 0x0400180B RID: 6155
		private string m_name;

		// Token: 0x0400180C RID: 6156
		private Version m_version;

		// Token: 0x0400180D RID: 6157
		[NonSerialized]
		private Assembly m_assembly;

		// Token: 0x0400180E RID: 6158
		[NonSerialized]
		private bool m_wasUsed;
	}
}

using System;
using System.Collections;
using System.Deployment.Internal.Isolation;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x02000011 RID: 17
	internal class DefinitionIdentity : ICloneable
	{
		// Token: 0x06000086 RID: 134 RVA: 0x000050A1 File Offset: 0x000040A1
		public DefinitionIdentity()
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.CreateDefinition();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000050B9 File Offset: 0x000040B9
		public DefinitionIdentity(string text)
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.TextToDefinition(0U, text);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000050D3 File Offset: 0x000040D3
		public DefinitionIdentity(IDefinitionIdentity idComPtr)
		{
			this._idComPtr = idComPtr;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000050E4 File Offset: 0x000040E4
		public DefinitionIdentity(ReferenceIdentity refId)
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.CreateDefinition();
			IDENTITY_ATTRIBUTE[] attributes = refId.Attributes;
			foreach (IDENTITY_ATTRIBUTE identity_ATTRIBUTE in attributes)
			{
				this[identity_ATTRIBUTE.Namespace, identity_ATTRIBUTE.Name] = identity_ATTRIBUTE.Value;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005148 File Offset: 0x00004148
		public DefinitionIdentity(AssemblyName asmName)
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.CreateDefinition();
			this["name"] = asmName.Name;
			this["version"] = asmName.Version.ToString();
			if (asmName.CultureInfo != null)
			{
				this["culture"] = asmName.CultureInfo.Name;
			}
			byte[] publicKeyToken = asmName.GetPublicKeyToken();
			if (publicKeyToken != null && publicKeyToken.Length > 0)
			{
				this["publicKeyToken"] = HexString.FromBytes(publicKeyToken);
			}
		}

		// Token: 0x17000020 RID: 32
		public string this[string name]
		{
			get
			{
				return this._idComPtr.GetAttribute(null, name);
			}
			set
			{
				this._idComPtr.SetAttribute(null, name, value);
			}
		}

		// Token: 0x17000021 RID: 33
		public string this[string ns, string name]
		{
			set
			{
				this._idComPtr.SetAttribute(ns, name, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00005200 File Offset: 0x00004200
		// (set) Token: 0x0600008F RID: 143 RVA: 0x0000520D File Offset: 0x0000420D
		public string Name
		{
			get
			{
				return this["name"];
			}
			set
			{
				this["name"] = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000521C File Offset: 0x0000421C
		public Version Version
		{
			get
			{
				string text = this["version"];
				if (text == null)
				{
					return null;
				}
				return new Version(text);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00005240 File Offset: 0x00004240
		public string PublicKeyToken
		{
			get
			{
				return this["publicKeyToken"];
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000524D File Offset: 0x0000424D
		public string ProcessorArchitecture
		{
			get
			{
				return this["processorArchitecture"];
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000525A File Offset: 0x0000425A
		public ulong Hash
		{
			get
			{
				return IsolationInterop.IdentityAuthority.HashDefinition(0U, this._idComPtr);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000526D File Offset: 0x0000426D
		public string KeyForm
		{
			get
			{
				return IsolationInterop.IdentityAuthority.GenerateDefinitionKey(0U, this._idComPtr);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005280 File Offset: 0x00004280
		public bool Matches(ReferenceIdentity refId, bool exact)
		{
			return IsolationInterop.IdentityAuthority.DoesDefinitionMatchReference(exact ? 1U : 0U, this._idComPtr, refId.ComPointer) && this.Version == refId.Version;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000052B4 File Offset: 0x000042B4
		public IDENTITY_ATTRIBUTE[] Attributes
		{
			get
			{
				IEnumIDENTITY_ATTRIBUTE enumIDENTITY_ATTRIBUTE = null;
				IDENTITY_ATTRIBUTE[] array2;
				try
				{
					ArrayList arrayList = new ArrayList();
					enumIDENTITY_ATTRIBUTE = this._idComPtr.EnumAttributes();
					IDENTITY_ATTRIBUTE[] array = new IDENTITY_ATTRIBUTE[1];
					while (enumIDENTITY_ATTRIBUTE.Next(1U, array) == 1U)
					{
						arrayList.Add(array[0]);
					}
					array2 = (IDENTITY_ATTRIBUTE[])arrayList.ToArray(typeof(IDENTITY_ATTRIBUTE));
				}
				finally
				{
					if (enumIDENTITY_ATTRIBUTE != null)
					{
						Marshal.ReleaseComObject(enumIDENTITY_ATTRIBUTE);
					}
				}
				return array2;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005338 File Offset: 0x00004338
		public bool IsEmpty
		{
			get
			{
				IDENTITY_ATTRIBUTE[] attributes = this.Attributes;
				foreach (IDENTITY_ATTRIBUTE identity_ATTRIBUTE in attributes)
				{
					if (!string.IsNullOrEmpty(identity_ATTRIBUTE.Value))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005384 File Offset: 0x00004384
		public DefinitionIdentity ToSubscriptionId()
		{
			DefinitionIdentity definitionIdentity = (DefinitionIdentity)this.Clone();
			definitionIdentity["version"] = null;
			return definitionIdentity;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000053AC File Offset: 0x000043AC
		public DefinitionIdentity ToPKTGroupId()
		{
			DefinitionIdentity definitionIdentity = (DefinitionIdentity)this.Clone();
			definitionIdentity["version"] = null;
			definitionIdentity["publicKeyToken"] = null;
			return definitionIdentity;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000053DE File Offset: 0x000043DE
		public IDefinitionIdentity ComPointer
		{
			get
			{
				return this._idComPtr;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000053E6 File Offset: 0x000043E6
		public override bool Equals(object obj)
		{
			return obj is DefinitionIdentity && IsolationInterop.IdentityAuthority.AreDefinitionsEqual(0U, this.ComPointer, ((DefinitionIdentity)obj).ComPointer);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000540E File Offset: 0x0000440E
		public override int GetHashCode()
		{
			return (int)this.Hash;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005417 File Offset: 0x00004417
		public override string ToString()
		{
			return IsolationInterop.IdentityAuthority.DefinitionToText(0U, this._idComPtr);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000542A File Offset: 0x0000442A
		public object Clone()
		{
			return new DefinitionIdentity(this._idComPtr.Clone(IntPtr.Zero, null));
		}

		// Token: 0x04000050 RID: 80
		private IDefinitionIdentity _idComPtr;
	}
}

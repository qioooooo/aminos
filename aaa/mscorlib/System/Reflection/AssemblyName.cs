using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x020002DB RID: 731
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_AssemblyName))]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public sealed class AssemblyName : _AssemblyName, ICloneable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x00049ACE File Offset: 0x00048ACE
		public AssemblyName()
		{
			this._HashAlgorithm = AssemblyHashAlgorithm.None;
			this._VersionCompatibility = AssemblyVersionCompatibility.SameMachine;
			this._Flags = AssemblyNameFlags.None;
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x00049AEB File Offset: 0x00048AEB
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x00049AF3 File Offset: 0x00048AF3
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x00049AFC File Offset: 0x00048AFC
		// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x00049B04 File Offset: 0x00048B04
		public Version Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x00049B0D File Offset: 0x00048B0D
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x00049B15 File Offset: 0x00048B15
		public CultureInfo CultureInfo
		{
			get
			{
				return this._CultureInfo;
			}
			set
			{
				this._CultureInfo = value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x00049B1E File Offset: 0x00048B1E
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x00049B26 File Offset: 0x00048B26
		public string CodeBase
		{
			get
			{
				return this._CodeBase;
			}
			set
			{
				this._CodeBase = value;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x00049B2F File Offset: 0x00048B2F
		public string EscapedCodeBase
		{
			get
			{
				if (this._CodeBase == null)
				{
					return null;
				}
				return AssemblyName.EscapeCodeBase(this._CodeBase);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x00049B48 File Offset: 0x00048B48
		// (set) Token: 0x06001CDD RID: 7389 RVA: 0x00049B68 File Offset: 0x00048B68
		public ProcessorArchitecture ProcessorArchitecture
		{
			get
			{
				int num = (int)((this._Flags & (AssemblyNameFlags)112) >> 4);
				if (num > 4)
				{
					num = 0;
				}
				return (ProcessorArchitecture)num;
			}
			set
			{
				int num = (int)(value & (ProcessorArchitecture)7);
				if (num <= 4)
				{
					this._Flags = (AssemblyNameFlags)((long)this._Flags & (long)((ulong)(-241)));
					this._Flags |= (AssemblyNameFlags)(num << 4);
				}
			}
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x00049BA4 File Offset: 0x00048BA4
		public object Clone()
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Init(this._Name, this._PublicKey, this._PublicKeyToken, this._Version, this._CultureInfo, this._HashAlgorithm, this._VersionCompatibility, this._CodeBase, this._Flags, this._StrongNameKeyPair);
			assemblyName._HashForControl = this._HashForControl;
			assemblyName._HashAlgorithmForControl = this._HashAlgorithmForControl;
			return assemblyName;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00049C14 File Offset: 0x00048C14
		public static AssemblyName GetAssemblyName(string assemblyFile)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			string fullPathInternal = Path.GetFullPathInternal(assemblyFile);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullPathInternal).Demand();
			return AssemblyName.nGetFileInformation(fullPathInternal);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x00049C48 File Offset: 0x00048C48
		internal void SetHashControl(byte[] hash, AssemblyHashAlgorithm hashAlgorithm)
		{
			this._HashForControl = hash;
			this._HashAlgorithmForControl = hashAlgorithm;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00049C58 File Offset: 0x00048C58
		public byte[] GetPublicKey()
		{
			return this._PublicKey;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00049C60 File Offset: 0x00048C60
		public void SetPublicKey(byte[] publicKey)
		{
			this._PublicKey = publicKey;
			if (publicKey == null)
			{
				this._Flags ^= AssemblyNameFlags.PublicKey;
				return;
			}
			this._Flags |= AssemblyNameFlags.PublicKey;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x00049C89 File Offset: 0x00048C89
		public byte[] GetPublicKeyToken()
		{
			if (this._PublicKeyToken == null)
			{
				this._PublicKeyToken = this.nGetPublicKeyToken();
			}
			return this._PublicKeyToken;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00049CA5 File Offset: 0x00048CA5
		public void SetPublicKeyToken(byte[] publicKeyToken)
		{
			this._PublicKeyToken = publicKeyToken;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x00049CAE File Offset: 0x00048CAE
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x00049CBC File Offset: 0x00048CBC
		public AssemblyNameFlags Flags
		{
			get
			{
				return this._Flags & (AssemblyNameFlags)(-241);
			}
			set
			{
				this._Flags &= (AssemblyNameFlags)240;
				this._Flags |= value & (AssemblyNameFlags)(-241);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00049CE4 File Offset: 0x00048CE4
		// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x00049CEC File Offset: 0x00048CEC
		public AssemblyHashAlgorithm HashAlgorithm
		{
			get
			{
				return this._HashAlgorithm;
			}
			set
			{
				this._HashAlgorithm = value;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x00049CF5 File Offset: 0x00048CF5
		// (set) Token: 0x06001CEA RID: 7402 RVA: 0x00049CFD File Offset: 0x00048CFD
		public AssemblyVersionCompatibility VersionCompatibility
		{
			get
			{
				return this._VersionCompatibility;
			}
			set
			{
				this._VersionCompatibility = value;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x00049D06 File Offset: 0x00048D06
		// (set) Token: 0x06001CEC RID: 7404 RVA: 0x00049D0E File Offset: 0x00048D0E
		public StrongNameKeyPair KeyPair
		{
			get
			{
				return this._StrongNameKeyPair;
			}
			set
			{
				this._StrongNameKeyPair = value;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00049D17 File Offset: 0x00048D17
		public string FullName
		{
			get
			{
				return this.nToString();
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00049D20 File Offset: 0x00048D20
		public override string ToString()
		{
			string fullName = this.FullName;
			if (fullName == null)
			{
				return base.ToString();
			}
			return fullName;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00049D40 File Offset: 0x00048D40
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_Name", this._Name);
			info.AddValue("_PublicKey", this._PublicKey, typeof(byte[]));
			info.AddValue("_PublicKeyToken", this._PublicKeyToken, typeof(byte[]));
			info.AddValue("_CultureInfo", (this._CultureInfo == null) ? (-1) : this._CultureInfo.LCID);
			info.AddValue("_CodeBase", this._CodeBase);
			info.AddValue("_Version", this._Version);
			info.AddValue("_HashAlgorithm", this._HashAlgorithm, typeof(AssemblyHashAlgorithm));
			info.AddValue("_HashAlgorithmForControl", this._HashAlgorithmForControl, typeof(AssemblyHashAlgorithm));
			info.AddValue("_StrongNameKeyPair", this._StrongNameKeyPair, typeof(StrongNameKeyPair));
			info.AddValue("_VersionCompatibility", this._VersionCompatibility, typeof(AssemblyVersionCompatibility));
			info.AddValue("_Flags", this._Flags, typeof(AssemblyNameFlags));
			info.AddValue("_HashForControl", this._HashForControl, typeof(byte[]));
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00049E9C File Offset: 0x00048E9C
		public void OnDeserialization(object sender)
		{
			if (this.m_siInfo == null)
			{
				return;
			}
			this._Name = this.m_siInfo.GetString("_Name");
			this._PublicKey = (byte[])this.m_siInfo.GetValue("_PublicKey", typeof(byte[]));
			this._PublicKeyToken = (byte[])this.m_siInfo.GetValue("_PublicKeyToken", typeof(byte[]));
			int @int = this.m_siInfo.GetInt32("_CultureInfo");
			if (@int != -1)
			{
				this._CultureInfo = new CultureInfo(@int);
			}
			this._CodeBase = this.m_siInfo.GetString("_CodeBase");
			this._Version = (Version)this.m_siInfo.GetValue("_Version", typeof(Version));
			this._HashAlgorithm = (AssemblyHashAlgorithm)this.m_siInfo.GetValue("_HashAlgorithm", typeof(AssemblyHashAlgorithm));
			this._StrongNameKeyPair = (StrongNameKeyPair)this.m_siInfo.GetValue("_StrongNameKeyPair", typeof(StrongNameKeyPair));
			this._VersionCompatibility = (AssemblyVersionCompatibility)this.m_siInfo.GetValue("_VersionCompatibility", typeof(AssemblyVersionCompatibility));
			this._Flags = (AssemblyNameFlags)this.m_siInfo.GetValue("_Flags", typeof(AssemblyNameFlags));
			try
			{
				this._HashAlgorithmForControl = (AssemblyHashAlgorithm)this.m_siInfo.GetValue("_HashAlgorithmForControl", typeof(AssemblyHashAlgorithm));
				this._HashForControl = (byte[])this.m_siInfo.GetValue("_HashForControl", typeof(byte[]));
			}
			catch (SerializationException)
			{
				this._HashAlgorithmForControl = AssemblyHashAlgorithm.None;
				this._HashForControl = null;
			}
			this.m_siInfo = null;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x0004A078 File Offset: 0x00049078
		public AssemblyName(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyName.Length == 0 || assemblyName[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Format_StringZeroLength"));
			}
			this._Name = assemblyName;
			this.nInit();
		}

		// Token: 0x06001CF2 RID: 7410
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ReferenceMatchesDefinition(AssemblyName reference, AssemblyName definition);

		// Token: 0x06001CF3 RID: 7411
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int nInit(out Assembly assembly, bool forIntrospection, bool raiseResolveEvent);

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0004A0C8 File Offset: 0x000490C8
		internal void nInit()
		{
			Assembly assembly = null;
			this.nInit(out assembly, false, false);
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x0004A0E2 File Offset: 0x000490E2
		internal AssemblyName(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x0004A0F4 File Offset: 0x000490F4
		internal void Init(string name, byte[] publicKey, byte[] publicKeyToken, Version version, CultureInfo cultureInfo, AssemblyHashAlgorithm hashAlgorithm, AssemblyVersionCompatibility versionCompatibility, string codeBase, AssemblyNameFlags flags, StrongNameKeyPair keyPair)
		{
			this._Name = name;
			if (publicKey != null)
			{
				this._PublicKey = new byte[publicKey.Length];
				Array.Copy(publicKey, this._PublicKey, publicKey.Length);
			}
			if (publicKeyToken != null)
			{
				this._PublicKeyToken = new byte[publicKeyToken.Length];
				Array.Copy(publicKeyToken, this._PublicKeyToken, publicKeyToken.Length);
			}
			if (version != null)
			{
				this._Version = (Version)version.Clone();
			}
			this._CultureInfo = cultureInfo;
			this._HashAlgorithm = hashAlgorithm;
			this._VersionCompatibility = versionCompatibility;
			this._CodeBase = codeBase;
			this._Flags = flags;
			this._StrongNameKeyPair = keyPair;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x0004A194 File Offset: 0x00049194
		void _AssemblyName.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x0004A19B File Offset: 0x0004919B
		void _AssemblyName.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x0004A1A2 File Offset: 0x000491A2
		void _AssemblyName.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x0004A1A9 File Offset: 0x000491A9
		void _AssemblyName.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001CFB RID: 7419
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AssemblyName nGetFileInformation(string s);

		// Token: 0x06001CFC RID: 7420
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string nToString();

		// Token: 0x06001CFD RID: 7421
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] nGetPublicKeyToken();

		// Token: 0x06001CFE RID: 7422
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string EscapeCodeBase(string codeBase);

		// Token: 0x04000A8B RID: 2699
		private string _Name;

		// Token: 0x04000A8C RID: 2700
		private byte[] _PublicKey;

		// Token: 0x04000A8D RID: 2701
		private byte[] _PublicKeyToken;

		// Token: 0x04000A8E RID: 2702
		private CultureInfo _CultureInfo;

		// Token: 0x04000A8F RID: 2703
		private string _CodeBase;

		// Token: 0x04000A90 RID: 2704
		private Version _Version;

		// Token: 0x04000A91 RID: 2705
		private StrongNameKeyPair _StrongNameKeyPair;

		// Token: 0x04000A92 RID: 2706
		private SerializationInfo m_siInfo;

		// Token: 0x04000A93 RID: 2707
		private byte[] _HashForControl;

		// Token: 0x04000A94 RID: 2708
		private AssemblyHashAlgorithm _HashAlgorithm;

		// Token: 0x04000A95 RID: 2709
		private AssemblyHashAlgorithm _HashAlgorithmForControl;

		// Token: 0x04000A96 RID: 2710
		private AssemblyVersionCompatibility _VersionCompatibility;

		// Token: 0x04000A97 RID: 2711
		private AssemblyNameFlags _Flags;
	}
}

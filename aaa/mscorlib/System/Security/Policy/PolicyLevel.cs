using System;
using System.Collections;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Util;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Security.Policy
{
	// Token: 0x0200049A RID: 1178
	[ComVisible(true)]
	[Serializable]
	public sealed class PolicyLevel
	{
		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000A12A8 File Offset: 0x000A02A8
		private static object InternalSyncObject
		{
			get
			{
				if (PolicyLevel.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref PolicyLevel.s_InternalSyncObject, obj, null);
				}
				return PolicyLevel.s_InternalSyncObject;
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000A12D4 File Offset: 0x000A02D4
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_label != null)
			{
				this.DeriveTypeFromLabel();
			}
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000A12E4 File Offset: 0x000A02E4
		private void DeriveTypeFromLabel()
		{
			if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_User")))
			{
				this.m_type = PolicyLevelType.User;
				return;
			}
			if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_Machine")))
			{
				this.m_type = PolicyLevelType.Machine;
				return;
			}
			if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_Enterprise")))
			{
				this.m_type = PolicyLevelType.Enterprise;
				return;
			}
			if (this.m_label.Equals(Environment.GetResourceString("Policy_PL_AppDomain")))
			{
				this.m_type = PolicyLevelType.AppDomain;
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Policy_Default"));
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000A137C File Offset: 0x000A037C
		private string DeriveLabelFromType()
		{
			switch (this.m_type)
			{
			case PolicyLevelType.User:
				return Environment.GetResourceString("Policy_PL_User");
			case PolicyLevelType.Machine:
				return Environment.GetResourceString("Policy_PL_Machine");
			case PolicyLevelType.Enterprise:
				return Environment.GetResourceString("Policy_PL_Enterprise");
			case PolicyLevelType.AppDomain:
				return Environment.GetResourceString("Policy_PL_AppDomain");
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)this.m_type }));
			}
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000A1403 File Offset: 0x000A0403
		private PolicyLevel()
		{
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000A140B File Offset: 0x000A040B
		internal PolicyLevel(PolicyLevelType type)
			: this(type, PolicyLevel.GetLocationFromType(type))
		{
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000A141A File Offset: 0x000A041A
		internal PolicyLevel(PolicyLevelType type, string path)
			: this(type, path, ConfigId.None)
		{
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000A1428 File Offset: 0x000A0428
		internal PolicyLevel(PolicyLevelType type, string path, ConfigId configId)
		{
			this.m_type = type;
			this.m_path = path;
			this.m_loaded = path == null;
			if (this.m_path == null)
			{
				this.m_rootCodeGroup = this.CreateDefaultAllGroup();
				this.SetFactoryPermissionSets();
				this.SetDefaultFullTrustAssemblies();
			}
			this.m_configId = configId;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000A147C File Offset: 0x000A047C
		internal static string GetLocationFromType(PolicyLevelType type)
		{
			switch (type)
			{
			case PolicyLevelType.User:
				return Config.UserDirectory + "security.config";
			case PolicyLevelType.Machine:
				return Config.MachineDirectory + "security.config";
			case PolicyLevelType.Enterprise:
				return Config.MachineDirectory + "enterprisesec.config";
			default:
				return null;
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000A14D0 File Offset: 0x000A04D0
		public static PolicyLevel CreateAppDomainLevel()
		{
			return new PolicyLevel(PolicyLevelType.AppDomain);
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x000A14D8 File Offset: 0x000A04D8
		public string Label
		{
			get
			{
				if (this.m_label == null)
				{
					this.m_label = this.DeriveLabelFromType();
				}
				return this.m_label;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002F43 RID: 12099 RVA: 0x000A14F4 File Offset: 0x000A04F4
		[ComVisible(false)]
		public PolicyLevelType Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000A14FC File Offset: 0x000A04FC
		internal ConfigId ConfigId
		{
			get
			{
				return this.m_configId;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002F45 RID: 12101 RVA: 0x000A1504 File Offset: 0x000A0504
		internal string Path
		{
			get
			{
				return this.m_path;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x000A150C File Offset: 0x000A050C
		public string StoreLocation
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return PolicyLevel.GetLocationFromType(this.m_type);
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002F47 RID: 12103 RVA: 0x000A1519 File Offset: 0x000A0519
		// (set) Token: 0x06002F48 RID: 12104 RVA: 0x000A1527 File Offset: 0x000A0527
		public CodeGroup RootCodeGroup
		{
			get
			{
				this.CheckLoaded();
				return this.m_rootCodeGroup;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("RootCodeGroup");
				}
				this.CheckLoaded();
				this.m_rootCodeGroup = value.Copy();
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002F49 RID: 12105 RVA: 0x000A154C File Offset: 0x000A054C
		public IList NamedPermissionSets
		{
			get
			{
				this.CheckLoaded();
				this.LoadAllPermissionSets();
				ArrayList arrayList = new ArrayList(this.m_namedPermissionSets.Count);
				foreach (object obj in this.m_namedPermissionSets)
				{
					arrayList.Add(((NamedPermissionSet)obj).Copy());
				}
				return arrayList;
			}
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000A15A4 File Offset: 0x000A05A4
		public CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			return this.RootCodeGroup.ResolveMatchingCodeGroups(evidence);
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000A15C0 File Offset: 0x000A05C0
		[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
		public void AddFullTrustAssembly(StrongName sn)
		{
			if (sn == null)
			{
				throw new ArgumentNullException("sn");
			}
			this.AddFullTrustAssembly(new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version));
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000A15F0 File Offset: 0x000A05F0
		[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
		public void AddFullTrustAssembly(StrongNameMembershipCondition snMC)
		{
			if (snMC == null)
			{
				throw new ArgumentNullException("snMC");
			}
			this.CheckLoaded();
			IEnumerator enumerator = this.m_fullTrustAssemblies.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (((StrongNameMembershipCondition)enumerator.Current).Equals(snMC))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_AssemblyAlreadyFullTrust"));
				}
			}
			lock (this.m_fullTrustAssemblies)
			{
				this.m_fullTrustAssemblies.Add(snMC);
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000A1680 File Offset: 0x000A0680
		[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
		public void RemoveFullTrustAssembly(StrongName sn)
		{
			if (sn == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.RemoveFullTrustAssembly(new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version));
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000A16B0 File Offset: 0x000A06B0
		[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
		public void RemoveFullTrustAssembly(StrongNameMembershipCondition snMC)
		{
			if (snMC == null)
			{
				throw new ArgumentNullException("snMC");
			}
			this.CheckLoaded();
			object obj = null;
			IEnumerator enumerator = this.m_fullTrustAssemblies.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (((StrongNameMembershipCondition)enumerator.Current).Equals(snMC))
				{
					obj = enumerator.Current;
					break;
				}
			}
			if (obj == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_AssemblyNotFullTrust"));
			}
			lock (this.m_fullTrustAssemblies)
			{
				this.m_fullTrustAssemblies.Remove(obj);
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000A174C File Offset: 0x000A074C
		[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
		public IList FullTrustAssemblies
		{
			get
			{
				this.CheckLoaded();
				return new ArrayList(this.m_fullTrustAssemblies);
			}
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000A1760 File Offset: 0x000A0760
		public void AddNamedPermissionSet(NamedPermissionSet permSet)
		{
			if (permSet == null)
			{
				throw new ArgumentNullException("permSet");
			}
			this.CheckLoaded();
			this.LoadAllPermissionSets();
			lock (this)
			{
				IEnumerator enumerator = this.m_namedPermissionSets.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (((NamedPermissionSet)enumerator.Current).Name.Equals(permSet.Name))
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateName"));
					}
				}
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)permSet.Copy();
				namedPermissionSet.IgnoreTypeLoadFailures = true;
				this.m_namedPermissionSets.Add(namedPermissionSet);
			}
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000A180C File Offset: 0x000A080C
		public NamedPermissionSet RemoveNamedPermissionSet(NamedPermissionSet permSet)
		{
			if (permSet == null)
			{
				throw new ArgumentNullException("permSet");
			}
			return this.RemoveNamedPermissionSet(permSet.Name);
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000A1828 File Offset: 0x000A0828
		public NamedPermissionSet RemoveNamedPermissionSet(string name)
		{
			this.CheckLoaded();
			this.LoadAllPermissionSets();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			int num = -1;
			for (int i = 0; i < PolicyLevel.s_reservedNamedPermissionSets.Length; i++)
			{
				if (PolicyLevel.s_reservedNamedPermissionSets[i].Equals(name))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ReservedNPMS"), new object[] { name }));
				}
			}
			ArrayList namedPermissionSets = this.m_namedPermissionSets;
			for (int j = 0; j < namedPermissionSets.Count; j++)
			{
				if (((NamedPermissionSet)namedPermissionSets[j]).Name.Equals(name))
				{
					num = j;
					break;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoNPMS"));
			}
			ArrayList arrayList = new ArrayList();
			arrayList.Add(this.m_rootCodeGroup);
			for (int k = 0; k < arrayList.Count; k++)
			{
				CodeGroup codeGroup = (CodeGroup)arrayList[k];
				if (codeGroup.PermissionSetName != null && codeGroup.PermissionSetName.Equals(name))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NPMSInUse"), new object[] { name }));
				}
				IEnumerator enumerator = codeGroup.Children.GetEnumerator();
				if (enumerator != null)
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						arrayList.Add(obj);
					}
				}
			}
			NamedPermissionSet namedPermissionSet = (NamedPermissionSet)namedPermissionSets[num];
			namedPermissionSets.RemoveAt(num);
			return namedPermissionSet;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000A19AC File Offset: 0x000A09AC
		public NamedPermissionSet ChangeNamedPermissionSet(string name, PermissionSet pSet)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (pSet == null)
			{
				throw new ArgumentNullException("pSet");
			}
			for (int i = 0; i < PolicyLevel.s_reservedNamedPermissionSets.Length; i++)
			{
				if (PolicyLevel.s_reservedNamedPermissionSets[i].Equals(name))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ReservedNPMS"), new object[] { name }));
				}
			}
			NamedPermissionSet namedPermissionSetInternal = this.GetNamedPermissionSetInternal(name);
			if (namedPermissionSetInternal == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoNPMS"));
			}
			NamedPermissionSet namedPermissionSet = (NamedPermissionSet)namedPermissionSetInternal.Copy();
			namedPermissionSetInternal.Reset();
			namedPermissionSetInternal.SetUnrestricted(pSet.IsUnrestricted());
			foreach (object obj in pSet)
			{
				namedPermissionSetInternal.SetPermission(((IPermission)obj).Copy());
			}
			if (pSet is NamedPermissionSet)
			{
				namedPermissionSetInternal.Description = ((NamedPermissionSet)pSet).Description;
			}
			return namedPermissionSet;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000A1A9C File Offset: 0x000A0A9C
		public NamedPermissionSet GetNamedPermissionSet(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			NamedPermissionSet namedPermissionSetInternal = this.GetNamedPermissionSetInternal(name);
			if (namedPermissionSetInternal != null)
			{
				return new NamedPermissionSet(namedPermissionSetInternal);
			}
			return null;
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000A1ACC File Offset: 0x000A0ACC
		public void Recover()
		{
			if (this.m_configId == ConfigId.None)
			{
				throw new PolicyException(Environment.GetResourceString("Policy_RecoverNotFileBased"));
			}
			lock (this)
			{
				if (!Config.RecoverData(this.m_configId))
				{
					throw new PolicyException(Environment.GetResourceString("Policy_RecoverNoConfigFile"));
				}
				this.m_loaded = false;
				this.m_rootCodeGroup = null;
				this.m_namedPermissionSets = null;
				this.m_fullTrustAssemblies = new ArrayList();
			}
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000A1B50 File Offset: 0x000A0B50
		public void Reset()
		{
			this.SetDefault();
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000A1B58 File Offset: 0x000A0B58
		public PolicyStatement Resolve(Evidence evidence)
		{
			return this.Resolve(evidence, 0, null);
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000A1B64 File Offset: 0x000A0B64
		public SecurityElement ToXml()
		{
			this.CheckLoaded();
			this.LoadAllPermissionSets();
			SecurityElement securityElement = new SecurityElement("PolicyLevel");
			securityElement.AddAttribute("version", "1");
			Hashtable hashtable = new Hashtable();
			lock (this)
			{
				SecurityElement securityElement2 = new SecurityElement("NamedPermissionSets");
				foreach (object obj in this.m_namedPermissionSets)
				{
					securityElement2.AddChild(this.NormalizeClassDeep(((NamedPermissionSet)obj).ToXml(), hashtable));
				}
				SecurityElement securityElement3 = this.NormalizeClassDeep(this.m_rootCodeGroup.ToXml(this), hashtable);
				SecurityElement securityElement4 = new SecurityElement("FullTrustAssemblies");
				foreach (object obj2 in this.m_fullTrustAssemblies)
				{
					securityElement4.AddChild(this.NormalizeClassDeep(((StrongNameMembershipCondition)obj2).ToXml(), hashtable));
				}
				SecurityElement securityElement5 = new SecurityElement("SecurityClasses");
				IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					SecurityElement securityElement6 = new SecurityElement("SecurityClass");
					securityElement6.AddAttribute("Name", (string)enumerator2.Value);
					securityElement6.AddAttribute("Description", (string)enumerator2.Key);
					securityElement5.AddChild(securityElement6);
				}
				securityElement.AddChild(securityElement5);
				securityElement.AddChild(securityElement2);
				securityElement.AddChild(securityElement3);
				securityElement.AddChild(securityElement4);
			}
			return securityElement;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000A1CE4 File Offset: 0x000A0CE4
		public void FromXml(SecurityElement e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			lock (this)
			{
				ArrayList arrayList = new ArrayList();
				SecurityElement securityElement = e.SearchForChildByTag("SecurityClasses");
				Hashtable hashtable;
				if (securityElement != null)
				{
					hashtable = new Hashtable();
					foreach (object obj in securityElement.Children)
					{
						SecurityElement securityElement2 = (SecurityElement)obj;
						if (securityElement2.Tag.Equals("SecurityClass"))
						{
							string text = securityElement2.Attribute("Name");
							string text2 = securityElement2.Attribute("Description");
							if (text != null && text2 != null)
							{
								hashtable.Add(text, text2);
							}
						}
					}
				}
				else
				{
					hashtable = null;
				}
				SecurityElement securityElement3 = e.SearchForChildByTag("FullTrustAssemblies");
				if (securityElement3 != null && securityElement3.InternalChildren != null)
				{
					string assemblyQualifiedName = typeof(StrongNameMembershipCondition).AssemblyQualifiedName;
					IEnumerator enumerator2 = securityElement3.Children.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						StrongNameMembershipCondition strongNameMembershipCondition = new StrongNameMembershipCondition();
						strongNameMembershipCondition.FromXml((SecurityElement)enumerator2.Current);
						arrayList.Add(strongNameMembershipCondition);
					}
				}
				this.m_fullTrustAssemblies = arrayList;
				ArrayList arrayList2 = new ArrayList();
				SecurityElement securityElement4 = e.SearchForChildByTag("NamedPermissionSets");
				SecurityElement securityElement5 = null;
				if (securityElement4 != null && securityElement4.InternalChildren != null)
				{
					securityElement5 = this.UnnormalizeClassDeep(securityElement4, hashtable);
					this.FindElement(securityElement5, "FullTrust");
					this.FindElement(securityElement5, "SkipVerification");
					this.FindElement(securityElement5, "Execution");
					this.FindElement(securityElement5, "Nothing");
					this.FindElement(securityElement5, "Internet");
					this.FindElement(securityElement5, "LocalIntranet");
				}
				if (securityElement5 == null)
				{
					securityElement5 = new SecurityElement("NamedPermissionSets");
				}
				arrayList2.Add(PolicyLevel.CreateFullTrustSet());
				arrayList2.Add(PolicyLevel.CreateSkipVerificationSet());
				arrayList2.Add(PolicyLevel.CreateExecutionSet());
				arrayList2.Add(PolicyLevel.CreateNothingSet());
				securityElement5.AddChild(PolicyLevel.GetInternetElement());
				securityElement5.AddChild(PolicyLevel.GetLocalIntranetElement());
				foreach (object obj2 in arrayList2)
				{
					PermissionSet permissionSet = (PermissionSet)obj2;
					permissionSet.IgnoreTypeLoadFailures = true;
				}
				this.m_namedPermissionSets = arrayList2;
				this.m_permSetElement = securityElement5;
				SecurityElement securityElement6 = e.SearchForChildByTag("CodeGroup");
				if (securityElement6 == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
					{
						"CodeGroup",
						base.GetType().FullName
					}));
				}
				CodeGroup codeGroup = XMLUtil.CreateCodeGroup(this.UnnormalizeClassDeep(securityElement6, hashtable));
				if (codeGroup == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
					{
						"CodeGroup",
						base.GetType().FullName
					}));
				}
				codeGroup.FromXml(securityElement6, this);
				this.m_rootCodeGroup = codeGroup;
			}
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000A2018 File Offset: 0x000A1018
		internal static PermissionSet GetBuiltInSet(string name)
		{
			if (name == null)
			{
				return null;
			}
			if (name.Equals("FullTrust"))
			{
				return PolicyLevel.CreateFullTrustSet();
			}
			if (name.Equals("Nothing"))
			{
				return PolicyLevel.CreateNothingSet();
			}
			if (name.Equals("Execution"))
			{
				return PolicyLevel.CreateExecutionSet();
			}
			if (name.Equals("SkipVerification"))
			{
				return PolicyLevel.CreateSkipVerificationSet();
			}
			if (name.Equals("Internet"))
			{
				return PolicyLevel.CreateInternetSet();
			}
			if (name.Equals("LocalIntranet"))
			{
				return PolicyLevel.CreateLocalIntranetSet();
			}
			return null;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000A20A0 File Offset: 0x000A10A0
		internal NamedPermissionSet GetNamedPermissionSetInternal(string name)
		{
			this.CheckLoaded();
			NamedPermissionSet namedPermissionSet3;
			lock (PolicyLevel.InternalSyncObject)
			{
				foreach (object obj in this.m_namedPermissionSets)
				{
					NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
					if (namedPermissionSet.Name.Equals(name))
					{
						return namedPermissionSet;
					}
				}
				if (this.m_permSetElement != null)
				{
					SecurityElement securityElement = this.FindElement(name);
					if (securityElement != null)
					{
						NamedPermissionSet namedPermissionSet2 = new NamedPermissionSet();
						namedPermissionSet2.Name = name;
						this.m_namedPermissionSets.Add(namedPermissionSet2);
						try
						{
							namedPermissionSet2.FromXml(securityElement, false, true);
						}
						catch
						{
							this.m_namedPermissionSets.Remove(namedPermissionSet2);
							return null;
						}
						if (namedPermissionSet2.Name != null)
						{
							return namedPermissionSet2;
						}
						this.m_namedPermissionSets.Remove(namedPermissionSet2);
						return null;
					}
				}
				namedPermissionSet3 = null;
			}
			return namedPermissionSet3;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000A2188 File Offset: 0x000A1188
		internal PolicyStatement Resolve(Evidence evidence, int count, char[] serializedEvidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			PolicyStatement policyStatement = null;
			if (serializedEvidence != null)
			{
				policyStatement = this.CheckCache(count, serializedEvidence);
			}
			if (policyStatement == null)
			{
				this.CheckLoaded();
				bool flag = this.m_fullTrustAssemblies != null && PolicyLevel.IsFullTrustAssembly(this.m_fullTrustAssemblies, evidence);
				bool flag2;
				if (flag)
				{
					policyStatement = new PolicyStatement(new PermissionSet(true), PolicyStatementAttribute.Nothing);
					flag2 = true;
				}
				else
				{
					ArrayList arrayList = this.GenericResolve(evidence, out flag2);
					policyStatement = new PolicyStatement();
					policyStatement.PermissionSet = null;
					foreach (object obj in arrayList)
					{
						PolicyStatement policy = ((CodeGroupStackFrame)obj).policy;
						if (policy != null)
						{
							policyStatement.GetPermissionSetNoCopy().InplaceUnion(policy.GetPermissionSetNoCopy());
							policyStatement.Attributes |= policy.Attributes;
							if (policy.HasDependentEvidence)
							{
								foreach (IDelayEvaluatedEvidence delayEvaluatedEvidence in policy.DependentEvidence)
								{
									delayEvaluatedEvidence.MarkUsed();
								}
							}
						}
					}
				}
				if (flag2 && serializedEvidence != null)
				{
					serializedEvidence = PolicyManager.MakeEvidenceArray(evidence, false);
					this.Cache(count, serializedEvidence, policyStatement);
				}
			}
			return policyStatement;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000A22C0 File Offset: 0x000A12C0
		private void CheckLoaded()
		{
			if (!this.m_loaded)
			{
				lock (PolicyLevel.InternalSyncObject)
				{
					if (!this.m_loaded)
					{
						this.LoadPolicyLevel();
					}
				}
			}
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000A2308 File Offset: 0x000A1308
		private static byte[] ReadFile(string fileName)
		{
			byte[] array2;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				int num = (int)fileStream.Length;
				byte[] array = new byte[num];
				num = fileStream.Read(array, 0, num);
				fileStream.Close();
				array2 = array;
			}
			return array2;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000A235C File Offset: 0x000A135C
		private void LoadPolicyLevel()
		{
			Exception ex = null;
			CodeAccessPermission.AssertAllPossible();
			if (File.InternalExists(this.m_path))
			{
				Encoding utf = Encoding.UTF8;
				SecurityElement securityElement;
				try
				{
					string @string = utf.GetString(PolicyLevel.ReadFile(this.m_path));
					securityElement = SecurityElement.FromString(@string);
				}
				catch (Exception ex2)
				{
					string text;
					if (!string.IsNullOrEmpty(ex2.Message))
					{
						text = ex2.Message;
					}
					else
					{
						text = ex2.GetType().AssemblyQualifiedName;
					}
					ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParseEx"), new object[] { this.Label, text }));
					goto IL_022A;
				}
				if (securityElement == null)
				{
					ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
				}
				else
				{
					SecurityElement securityElement2 = securityElement.SearchForChildByTag("mscorlib");
					if (securityElement2 == null)
					{
						ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
					}
					else
					{
						SecurityElement securityElement3 = securityElement2.SearchForChildByTag("security");
						if (securityElement3 == null)
						{
							ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
						}
						else
						{
							SecurityElement securityElement4 = securityElement3.SearchForChildByTag("policy");
							if (securityElement4 == null)
							{
								ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
							}
							else
							{
								SecurityElement securityElement5 = securityElement4.SearchForChildByTag("PolicyLevel");
								if (securityElement5 != null)
								{
									try
									{
										this.FromXml(securityElement5);
										goto IL_0222;
									}
									catch (Exception)
									{
										ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
										goto IL_022A;
									}
									goto IL_01F1;
									IL_0222:
									this.m_loaded = true;
									return;
								}
								IL_01F1:
								ex = this.LoadError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Error_SecurityPolicyFileParse"), new object[] { this.Label }));
							}
						}
					}
				}
			}
			IL_022A:
			this.SetDefault();
			this.m_loaded = true;
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000A25C4 File Offset: 0x000A15C4
		private Exception LoadError(string message)
		{
			if (this.m_type != PolicyLevelType.User && this.m_type != PolicyLevelType.Machine && this.m_type != PolicyLevelType.Enterprise)
			{
				return new ArgumentException(message);
			}
			Config.WriteToEventLog(message);
			return null;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000A25F0 File Offset: 0x000A15F0
		private void Cache(int count, char[] serializedEvidence, PolicyStatement policy)
		{
			if (this.m_configId == ConfigId.None)
			{
				return;
			}
			byte[] data = new SecurityDocument(policy.ToXml(null, true)).m_data;
			Config.AddCacheEntry(this.m_configId, count, serializedEvidence, data);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000A2628 File Offset: 0x000A1628
		private PolicyStatement CheckCache(int count, char[] serializedEvidence)
		{
			if (this.m_configId == ConfigId.None)
			{
				return null;
			}
			byte[] array;
			if (!Config.GetCacheEntry(this.m_configId, count, serializedEvidence, out array))
			{
				return null;
			}
			PolicyStatement policyStatement = new PolicyStatement();
			SecurityDocument securityDocument = new SecurityDocument(array);
			policyStatement.FromXml(securityDocument, 0, null, true);
			return policyStatement;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000A266C File Offset: 0x000A166C
		private static NamedPermissionSet CreateFullTrustSet()
		{
			return new NamedPermissionSet("FullTrust", PermissionState.Unrestricted)
			{
				m_descrResource = "Policy_PS_FullTrust"
			};
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000A2694 File Offset: 0x000A1694
		private static NamedPermissionSet CreateNothingSet()
		{
			return new NamedPermissionSet("Nothing", PermissionState.None)
			{
				m_descrResource = "Policy_PS_Nothing"
			};
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000A26BC File Offset: 0x000A16BC
		private static NamedPermissionSet CreateExecutionSet()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("Execution", PermissionState.None);
			namedPermissionSet.m_descrResource = "Policy_PS_Execution";
			namedPermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			return namedPermissionSet;
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000A26F0 File Offset: 0x000A16F0
		private static NamedPermissionSet CreateSkipVerificationSet()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("SkipVerification", PermissionState.None);
			namedPermissionSet.m_descrResource = "Policy_PS_SkipVerification";
			namedPermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.SkipVerification));
			return namedPermissionSet;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000A2724 File Offset: 0x000A1724
		private static NamedPermissionSet CreateInternetSet()
		{
			PolicyLevel policyLevel = new PolicyLevel(PolicyLevelType.User);
			return policyLevel.GetNamedPermissionSet("Internet");
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000A2744 File Offset: 0x000A1744
		private static NamedPermissionSet CreateLocalIntranetSet()
		{
			PolicyLevel policyLevel = new PolicyLevel(PolicyLevelType.User);
			return policyLevel.GetNamedPermissionSet("LocalIntranet");
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000A2764 File Offset: 0x000A1764
		private static bool IsFullTrustAssembly(ArrayList fullTrustAssemblies, Evidence evidence)
		{
			if (fullTrustAssemblies.Count == 0)
			{
				return false;
			}
			if (evidence != null)
			{
				lock (fullTrustAssemblies)
				{
					foreach (object obj in fullTrustAssemblies)
					{
						StrongNameMembershipCondition strongNameMembershipCondition = (StrongNameMembershipCondition)obj;
						if (strongNameMembershipCondition.Check(evidence))
						{
							if (Environment.GetCompatibilityFlag(CompatibilityFlag.FullTrustListAssembliesInGac))
							{
								if (new ZoneMembershipCondition().Check(evidence))
								{
									return true;
								}
							}
							else if (new GacMembershipCondition().Check(evidence))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000A27F4 File Offset: 0x000A17F4
		private static SecurityElement GetInternetElement()
		{
			string[] array = new string[PolicyLevel.s_InternetPolicySearchStrings.Length];
			array[0] = "2.0.0.0";
			array[1] = Environment.GetResourceString("Policy_PS_Internet");
			return new Parser(PolicyLevel.s_internetPermissionSet, PolicyLevel.s_InternetPolicySearchStrings, array).GetTopElement();
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000A283C File Offset: 0x000A183C
		private static SecurityElement GetLocalIntranetElement()
		{
			string[] array = new string[PolicyLevel.s_LocalIntranetPolicySearchStrings.Length];
			array[0] = "2.0.0.0";
			array[1] = Environment.GetResourceString("Policy_PS_LocalIntranet");
			return new Parser(PolicyLevel.s_localIntranetPermissionSet, PolicyLevel.s_LocalIntranetPolicySearchStrings, array).GetTopElement();
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000A2884 File Offset: 0x000A1884
		private CodeGroup CreateDefaultAllGroup()
		{
			UnionCodeGroup unionCodeGroup = new UnionCodeGroup();
			unionCodeGroup.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new AllMembershipCondition().ToXml()), this);
			unionCodeGroup.Name = Environment.GetResourceString("Policy_AllCode_Name");
			unionCodeGroup.Description = Environment.GetResourceString("Policy_AllCode_DescriptionFullTrust");
			return unionCodeGroup;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000A28D8 File Offset: 0x000A18D8
		private CodeGroup CreateDefaultMachinePolicy()
		{
			UnionCodeGroup unionCodeGroup = new UnionCodeGroup();
			unionCodeGroup.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "Nothing", new AllMembershipCondition().ToXml()), this);
			unionCodeGroup.Name = Environment.GetResourceString("Policy_AllCode_Name");
			unionCodeGroup.Description = Environment.GetResourceString("Policy_AllCode_DescriptionNothing");
			UnionCodeGroup unionCodeGroup2 = new UnionCodeGroup();
			unionCodeGroup2.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new ZoneMembershipCondition(SecurityZone.MyComputer).ToXml()), this);
			unionCodeGroup2.Name = Environment.GetResourceString("Policy_MyComputer_Name");
			unionCodeGroup2.Description = Environment.GetResourceString("Policy_MyComputer_Description");
			StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293");
			UnionCodeGroup unionCodeGroup3 = new UnionCodeGroup();
			unionCodeGroup3.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new StrongNameMembershipCondition(strongNamePublicKeyBlob, null, null).ToXml()), this);
			unionCodeGroup3.Name = Environment.GetResourceString("Policy_Microsoft_Name");
			unionCodeGroup3.Description = Environment.GetResourceString("Policy_Microsoft_Description");
			unionCodeGroup2.AddChildInternal(unionCodeGroup3);
			strongNamePublicKeyBlob = new StrongNamePublicKeyBlob("00000000000000000400000000000000");
			UnionCodeGroup unionCodeGroup4 = new UnionCodeGroup();
			unionCodeGroup4.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "FullTrust", new StrongNameMembershipCondition(strongNamePublicKeyBlob, null, null).ToXml()), this);
			unionCodeGroup4.Name = Environment.GetResourceString("Policy_Ecma_Name");
			unionCodeGroup4.Description = Environment.GetResourceString("Policy_Ecma_Description");
			unionCodeGroup2.AddChildInternal(unionCodeGroup4);
			unionCodeGroup.AddChildInternal(unionCodeGroup2);
			CodeGroup codeGroup = new UnionCodeGroup();
			codeGroup.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "LocalIntranet", new ZoneMembershipCondition(SecurityZone.Intranet).ToXml()), this);
			codeGroup.Name = Environment.GetResourceString("Policy_Intranet_Name");
			codeGroup.Description = Environment.GetResourceString("Policy_Intranet_Description");
			codeGroup.AddChildInternal(new NetCodeGroup(new AllMembershipCondition())
			{
				Name = Environment.GetResourceString("Policy_IntranetNet_Name"),
				Description = Environment.GetResourceString("Policy_IntranetNet_Description")
			});
			codeGroup.AddChildInternal(new FileCodeGroup(new AllMembershipCondition(), FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)
			{
				Name = Environment.GetResourceString("Policy_IntranetFile_Name"),
				Description = Environment.GetResourceString("Policy_IntranetFile_Description")
			});
			unionCodeGroup.AddChildInternal(codeGroup);
			CodeGroup codeGroup2 = new UnionCodeGroup();
			codeGroup2.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "Internet", new ZoneMembershipCondition(SecurityZone.Internet).ToXml()), this);
			codeGroup2.Name = Environment.GetResourceString("Policy_Internet_Name");
			codeGroup2.Description = Environment.GetResourceString("Policy_Internet_Description");
			codeGroup2.AddChildInternal(new NetCodeGroup(new AllMembershipCondition())
			{
				Name = Environment.GetResourceString("Policy_InternetNet_Name"),
				Description = Environment.GetResourceString("Policy_InternetNet_Description")
			});
			unionCodeGroup.AddChildInternal(codeGroup2);
			CodeGroup codeGroup3 = new UnionCodeGroup();
			codeGroup3.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "Nothing", new ZoneMembershipCondition(SecurityZone.Untrusted).ToXml()), this);
			codeGroup3.Name = Environment.GetResourceString("Policy_Untrusted_Name");
			codeGroup3.Description = Environment.GetResourceString("Policy_Untrusted_Description");
			unionCodeGroup.AddChildInternal(codeGroup3);
			CodeGroup codeGroup4 = new UnionCodeGroup();
			codeGroup4.FromXml(PolicyLevel.CreateCodeGroupElement("UnionCodeGroup", "Internet", new ZoneMembershipCondition(SecurityZone.Trusted).ToXml()), this);
			codeGroup4.Name = Environment.GetResourceString("Policy_Trusted_Name");
			codeGroup4.Description = Environment.GetResourceString("Policy_Trusted_Description");
			codeGroup4.AddChildInternal(new NetCodeGroup(new AllMembershipCondition())
			{
				Name = Environment.GetResourceString("Policy_TrustedNet_Name"),
				Description = Environment.GetResourceString("Policy_TrustedNet_Description")
			});
			unionCodeGroup.AddChildInternal(codeGroup4);
			return unionCodeGroup;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000A2C60 File Offset: 0x000A1C60
		private static SecurityElement CreateCodeGroupElement(string codeGroupType, string permissionSetName, SecurityElement mshipElement)
		{
			SecurityElement securityElement = new SecurityElement("CodeGroup");
			securityElement.AddAttribute("class", "System.Security." + codeGroupType + ", mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("PermissionSetName", permissionSetName);
			securityElement.AddChild(mshipElement);
			return securityElement;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000A2CB8 File Offset: 0x000A1CB8
		private void SetDefaultFullTrustAssemblies()
		{
			this.m_fullTrustAssemblies = new ArrayList();
			StrongNamePublicKeyBlob strongNamePublicKeyBlob = new StrongNamePublicKeyBlob("00000000000000000400000000000000");
			for (int i = 0; i < PolicyLevel.EcmaFullTrustAssemblies.Length; i++)
			{
				StrongNameMembershipCondition strongNameMembershipCondition = new StrongNameMembershipCondition(strongNamePublicKeyBlob, PolicyLevel.EcmaFullTrustAssemblies[i], PolicyLevel.s_mscorlibVersion);
				this.m_fullTrustAssemblies.Add(strongNameMembershipCondition);
			}
			StrongNamePublicKeyBlob strongNamePublicKeyBlob2 = new StrongNamePublicKeyBlob("002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293");
			for (int j = 0; j < PolicyLevel.MicrosoftFullTrustAssemblies.Length; j++)
			{
				StrongNameMembershipCondition strongNameMembershipCondition2 = new StrongNameMembershipCondition(strongNamePublicKeyBlob2, PolicyLevel.MicrosoftFullTrustAssemblies[j], PolicyLevel.s_mscorlibVersion);
				this.m_fullTrustAssemblies.Add(strongNameMembershipCondition2);
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000A2D54 File Offset: 0x000A1D54
		private void SetDefault()
		{
			lock (this)
			{
				string text = PolicyLevel.GetLocationFromType(this.m_type) + ".default";
				if (File.InternalExists(text))
				{
					PolicyLevel policyLevel = new PolicyLevel(this.m_type, text);
					this.m_rootCodeGroup = policyLevel.RootCodeGroup;
					this.m_namedPermissionSets = (ArrayList)policyLevel.NamedPermissionSets;
					this.m_fullTrustAssemblies = (ArrayList)policyLevel.FullTrustAssemblies;
					this.m_loaded = true;
				}
				else
				{
					this.m_namedPermissionSets = null;
					this.m_rootCodeGroup = null;
					this.m_permSetElement = null;
					this.m_rootCodeGroup = ((this.m_type == PolicyLevelType.Machine) ? this.CreateDefaultMachinePolicy() : this.CreateDefaultAllGroup());
					this.SetFactoryPermissionSets();
					this.SetDefaultFullTrustAssemblies();
					this.m_loaded = true;
				}
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000A2E2C File Offset: 0x000A1E2C
		private void SetFactoryPermissionSets()
		{
			lock (this)
			{
				this.m_namedPermissionSets = new ArrayList();
				string[] array = new string[PolicyLevel.s_FactoryPolicySearchStrings.Length];
				array[0] = "2.0.0.0";
				array[1] = Environment.GetResourceString("Policy_PS_FullTrust");
				array[2] = Environment.GetResourceString("Policy_PS_Everything");
				array[3] = Environment.GetResourceString("Policy_PS_Nothing");
				array[4] = Environment.GetResourceString("Policy_PS_SkipVerification");
				array[5] = Environment.GetResourceString("Policy_PS_Execution");
				this.m_permSetElement = new Parser(PolicyLevelData.s_defaultPermissionSets, PolicyLevel.s_FactoryPolicySearchStrings, array).GetTopElement();
				this.m_permSetElement.AddChild(PolicyLevel.GetInternetElement());
				this.m_permSetElement.AddChild(PolicyLevel.GetLocalIntranetElement());
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000A2EF8 File Offset: 0x000A1EF8
		private SecurityElement FindElement(string name)
		{
			SecurityElement securityElement = this.FindElement(this.m_permSetElement, name);
			if (this.m_permSetElement.InternalChildren.Count == 0)
			{
				this.m_permSetElement = null;
			}
			return securityElement;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000A2F30 File Offset: 0x000A1F30
		private SecurityElement FindElement(SecurityElement element, string name)
		{
			foreach (object obj in element.Children)
			{
				SecurityElement securityElement = (SecurityElement)obj;
				if (securityElement.Tag.Equals("PermissionSet"))
				{
					string text = securityElement.Attribute("Name");
					if (text != null && text.Equals(name))
					{
						element.InternalChildren.Remove(securityElement);
						return securityElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000A2F98 File Offset: 0x000A1F98
		private void LoadAllPermissionSets()
		{
			if (this.m_permSetElement != null && this.m_permSetElement.InternalChildren != null)
			{
				lock (PolicyLevel.InternalSyncObject)
				{
					while (this.m_permSetElement != null && this.m_permSetElement.InternalChildren.Count != 0)
					{
						SecurityElement securityElement = (SecurityElement)this.m_permSetElement.Children[this.m_permSetElement.InternalChildren.Count - 1];
						this.m_permSetElement.InternalChildren.RemoveAt(this.m_permSetElement.InternalChildren.Count - 1);
						if (securityElement.Tag.Equals("PermissionSet") && securityElement.Attribute("class").Equals("System.Security.NamedPermissionSet"))
						{
							NamedPermissionSet namedPermissionSet = new NamedPermissionSet();
							namedPermissionSet.FromXmlNameOnly(securityElement);
							if (namedPermissionSet.Name != null)
							{
								this.m_namedPermissionSets.Add(namedPermissionSet);
								try
								{
									namedPermissionSet.FromXml(securityElement, false, true);
								}
								catch
								{
									this.m_namedPermissionSets.Remove(namedPermissionSet);
								}
							}
						}
					}
					this.m_permSetElement = null;
				}
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000A30CC File Offset: 0x000A20CC
		private static void ReadNamedPermissionSetExtensionsFromRegistry()
		{
			if (PolicyLevel.s_extensionsReadFromRegistry)
			{
				return;
			}
			lock (PolicyLevel.InternalSyncObject)
			{
				bool flag = false;
				if (!PolicyLevel.s_extensionsReadFromRegistry)
				{
					string[][] array = null;
					new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE").Assert();
					RegistryKey localMachine = Registry.LocalMachine;
					using (RegistryKey registryKey = localMachine.OpenSubKey("Software\\Microsoft\\.NETFramework", false))
					{
						if (registryKey != null)
						{
							using (RegistryKey registryKey2 = registryKey.OpenSubKey("Security\\Policy\\Extensions\\NamedPermissionSets", false))
							{
								if (registryKey2 != null)
								{
									array = new string[PolicyLevel.s_extensibleNamedPermissionSets.Length][];
									for (int i = 0; i < PolicyLevel.s_extensibleNamedPermissionSets.Length; i++)
									{
										using (RegistryKey registryKey3 = registryKey2.OpenSubKey(PolicyLevel.s_extensibleNamedPermissionSets[i], false))
										{
											if (registryKey3 != null)
											{
												string[] array2 = registryKey3.InternalGetSubKeyNames();
												array[i] = new string[array2.Length];
												for (int j = 0; j < array2.Length; j++)
												{
													using (RegistryKey registryKey4 = registryKey3.OpenSubKey(array2[j], false))
													{
														string text = registryKey4.GetValue("Xml") as string;
														array[i][j] = text;
														flag = true;
													}
												}
											}
										}
									}
								}
							}
						}
					}
					if (flag)
					{
						PolicyLevel.s_extensibleNamedPermissionSetRegistryInfo = array;
					}
					PolicyLevel.s_extensionsReadFromRegistry = true;
				}
			}
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000A329C File Offset: 0x000A229C
		private static bool DependentAssembliesContainPermission(IAssemblyReferenceEntry[] asmEntries, SecurityElement se)
		{
			string text;
			string text2;
			string text3;
			bool flag = XMLUtil.ParseElementForAssemblyIdentification(se, out text, out text2, out text3);
			if (!flag)
			{
				return flag;
			}
			foreach (object obj in asmEntries)
			{
				IAssemblyReferenceEntry assemblyReferenceEntry = (IAssemblyReferenceEntry)obj;
				if (assemblyReferenceEntry != null)
				{
					IReferenceIdentity referenceIdentity = assemblyReferenceEntry.ReferenceIdentity;
					if (referenceIdentity != null)
					{
						string attribute = referenceIdentity.GetAttribute(null, "Name");
						string attribute2 = referenceIdentity.GetAttribute(null, "Version");
						if (string.Compare(attribute, text2, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(attribute2, text3, StringComparison.OrdinalIgnoreCase) == 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000A3324 File Offset: 0x000A2324
		private static PermissionSet GetNamedPermissionSetExtensions(IAssemblyReferenceEntry[] asmEntries, string[] extnList)
		{
			if (extnList == null)
			{
				return null;
			}
			SecurityElement securityElement = null;
			foreach (string text in extnList)
			{
				if (!string.IsNullOrEmpty(text))
				{
					SecurityElement securityElement2 = SecurityElement.FromString(text);
					if (PolicyLevel.DependentAssembliesContainPermission(asmEntries, securityElement2))
					{
						if (securityElement == null)
						{
							securityElement = PermissionSet.CreateEmptyPermissionSetXml();
						}
						securityElement.AddChild(securityElement2);
					}
				}
			}
			PermissionSet permissionSet = null;
			if (securityElement != null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.FromXml(securityElement);
			}
			return permissionSet;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000A338C File Offset: 0x000A238C
		private static void ExtendNamedPermissionSetsIfApplicable(PolicyStatement ps, Evidence evidence)
		{
			if (!PolicyLevel.s_extensionsReadFromRegistry)
			{
				PolicyLevel.ReadNamedPermissionSetExtensionsFromRegistry();
			}
			if (PolicyLevel.s_extensibleNamedPermissionSetRegistryInfo == null)
			{
				return;
			}
			NamedPermissionSet namedPermissionSet = ps.GetPermissionSetNoCopy() as NamedPermissionSet;
			if (namedPermissionSet == null)
			{
				return;
			}
			int num = Array.IndexOf<string>(PolicyLevel.s_extensibleNamedPermissionSets, namedPermissionSet.Name);
			if (num == -1)
			{
				return;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			ActivationArguments activationArguments = null;
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				activationArguments = obj as ActivationArguments;
				if (activationArguments != null)
				{
					break;
				}
			}
			if (activationArguments == null)
			{
				return;
			}
			ActivationContext activationContext = activationArguments.ActivationContext;
			if (activationContext == null)
			{
				return;
			}
			IAssemblyReferenceEntry[] dependentAssemblies = CmsUtils.GetDependentAssemblies(activationContext);
			PermissionSet namedPermissionSetExtensions = PolicyLevel.GetNamedPermissionSetExtensions(dependentAssemblies, PolicyLevel.s_extensibleNamedPermissionSetRegistryInfo[num]);
			ps.GetPermissionSetNoCopy().InplaceUnion(namedPermissionSetExtensions);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000A342C File Offset: 0x000A242C
		private ArrayList GenericResolve(Evidence evidence, out bool allConst)
		{
			CodeGroupStack codeGroupStack = new CodeGroupStack();
			CodeGroup rootCodeGroup = this.m_rootCodeGroup;
			if (rootCodeGroup == null)
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NonFullTrustAssembly"));
			}
			CodeGroupStackFrame codeGroupStackFrame = new CodeGroupStackFrame();
			codeGroupStackFrame.current = rootCodeGroup;
			codeGroupStackFrame.parent = null;
			codeGroupStack.Push(codeGroupStackFrame);
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			allConst = true;
			bool flag2 = true;
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext() && flag2)
			{
				IDelayEvaluatedEvidence delayEvaluatedEvidence = hostEnumerator.Current as IDelayEvaluatedEvidence;
				if (delayEvaluatedEvidence != null && !delayEvaluatedEvidence.IsVerified)
				{
					flag2 = false;
				}
			}
			Exception ex = null;
			while (!codeGroupStack.IsEmpty())
			{
				codeGroupStackFrame = codeGroupStack.Pop();
				IUnionSemanticCodeGroup unionSemanticCodeGroup = null;
				if (flag2)
				{
					unionSemanticCodeGroup = codeGroupStackFrame.current as IUnionSemanticCodeGroup;
				}
				FirstMatchCodeGroup firstMatchCodeGroup = codeGroupStackFrame.current as FirstMatchCodeGroup;
				if (!(codeGroupStackFrame.current.MembershipCondition is IConstantMembershipCondition) || (unionSemanticCodeGroup == null && firstMatchCodeGroup == null))
				{
					allConst = false;
				}
				try
				{
					if (unionSemanticCodeGroup != null)
					{
						codeGroupStackFrame.policy = unionSemanticCodeGroup.InternalResolve(evidence);
					}
					else
					{
						codeGroupStackFrame.policy = PolicyManager.ResolveCodeGroup(codeGroupStackFrame.current, evidence);
					}
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				if (codeGroupStackFrame.policy != null)
				{
					PolicyLevel.ExtendNamedPermissionSetsIfApplicable(codeGroupStackFrame.policy, evidence);
					if ((codeGroupStackFrame.policy.Attributes & PolicyStatementAttribute.Exclusive) != PolicyStatementAttribute.Nothing)
					{
						if (flag)
						{
							throw new PolicyException(Environment.GetResourceString("Policy_MultipleExclusive"));
						}
						arrayList.RemoveRange(0, arrayList.Count);
						arrayList.Add(codeGroupStackFrame);
						flag = true;
					}
					if (unionSemanticCodeGroup != null)
					{
						IList childrenInternal = codeGroupStackFrame.current.GetChildrenInternal();
						if (childrenInternal != null && childrenInternal.Count > 0)
						{
							foreach (object obj in childrenInternal)
							{
								codeGroupStack.Push(new CodeGroupStackFrame
								{
									current = (CodeGroup)obj,
									parent = codeGroupStackFrame
								});
							}
						}
					}
					if (!flag)
					{
						arrayList.Add(codeGroupStackFrame);
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			return arrayList;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000A3618 File Offset: 0x000A2618
		private static string GenerateFriendlyName(string className, Hashtable classes)
		{
			if (classes.ContainsKey(className))
			{
				return (string)classes[className];
			}
			Type type = global::System.Type.GetType(className, false, false);
			if (type != null && !type.IsVisible)
			{
				type = null;
			}
			if (type == null)
			{
				return className;
			}
			if (!classes.ContainsValue(type.Name))
			{
				classes.Add(className, type.Name);
				return type.Name;
			}
			if (!classes.ContainsValue(type.FullName))
			{
				classes.Add(className, type.FullName);
				return type.FullName;
			}
			classes.Add(className, type.AssemblyQualifiedName);
			return type.AssemblyQualifiedName;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000A36B0 File Offset: 0x000A26B0
		private SecurityElement NormalizeClassDeep(SecurityElement elem, Hashtable classes)
		{
			this.NormalizeClass(elem, classes);
			if (elem.InternalChildren != null && elem.InternalChildren.Count > 0)
			{
				foreach (object obj in elem.Children)
				{
					this.NormalizeClassDeep((SecurityElement)obj, classes);
				}
			}
			return elem;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000A3708 File Offset: 0x000A2708
		private SecurityElement NormalizeClass(SecurityElement elem, Hashtable classes)
		{
			if (elem.m_lAttributes == null || elem.m_lAttributes.Count == 0)
			{
				return elem;
			}
			int count = elem.m_lAttributes.Count;
			for (int i = 0; i < count; i += 2)
			{
				string text = (string)elem.m_lAttributes[i];
				if (text.Equals("class"))
				{
					string text2 = (string)elem.m_lAttributes[i + 1];
					elem.m_lAttributes[i + 1] = PolicyLevel.GenerateFriendlyName(text2, classes);
					break;
				}
			}
			return elem;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000A3790 File Offset: 0x000A2790
		private SecurityElement UnnormalizeClassDeep(SecurityElement elem, Hashtable classes)
		{
			this.UnnormalizeClass(elem, classes);
			if (elem.InternalChildren != null && elem.InternalChildren.Count > 0)
			{
				foreach (object obj in elem.Children)
				{
					this.UnnormalizeClassDeep((SecurityElement)obj, classes);
				}
			}
			return elem;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000A37E8 File Offset: 0x000A27E8
		private SecurityElement UnnormalizeClass(SecurityElement elem, Hashtable classes)
		{
			if (classes == null || elem.m_lAttributes == null || elem.m_lAttributes.Count == 0)
			{
				return elem;
			}
			int count = elem.m_lAttributes.Count;
			int i = 0;
			while (i < count)
			{
				string text = (string)elem.m_lAttributes[i];
				if (text.Equals("class"))
				{
					string text2 = (string)elem.m_lAttributes[i + 1];
					string text3 = (string)classes[text2];
					if (text3 != null)
					{
						elem.m_lAttributes[i + 1] = text3;
						break;
					}
					break;
				}
				else
				{
					i += 2;
				}
			}
			return elem;
		}

		// Token: 0x040017E0 RID: 6112
		private ArrayList m_fullTrustAssemblies;

		// Token: 0x040017E1 RID: 6113
		private ArrayList m_namedPermissionSets;

		// Token: 0x040017E2 RID: 6114
		private CodeGroup m_rootCodeGroup;

		// Token: 0x040017E3 RID: 6115
		private string m_label;

		// Token: 0x040017E4 RID: 6116
		[OptionalField(VersionAdded = 2)]
		private PolicyLevelType m_type;

		// Token: 0x040017E5 RID: 6117
		private ConfigId m_configId;

		// Token: 0x040017E6 RID: 6118
		private bool m_useDefaultCodeGroupsOnReset;

		// Token: 0x040017E7 RID: 6119
		private bool m_generateQuickCacheOnLoad;

		// Token: 0x040017E8 RID: 6120
		private bool m_caching;

		// Token: 0x040017E9 RID: 6121
		private bool m_throwOnLoadError;

		// Token: 0x040017EA RID: 6122
		private Encoding m_encoding;

		// Token: 0x040017EB RID: 6123
		private bool m_loaded;

		// Token: 0x040017EC RID: 6124
		private SecurityElement m_permSetElement;

		// Token: 0x040017ED RID: 6125
		private string m_path;

		// Token: 0x040017EE RID: 6126
		private static object s_InternalSyncObject;

		// Token: 0x040017EF RID: 6127
		private static readonly string[] s_FactoryPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_FullTrust}", "{Policy_PS_Everything}", "{Policy_PS_Nothing}", "{Policy_PS_SkipVerification}", "{Policy_PS_Execution}" };

		// Token: 0x040017F0 RID: 6128
		private static readonly string[] s_InternetPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_Internet}" };

		// Token: 0x040017F1 RID: 6129
		private static readonly string[] s_LocalIntranetPolicySearchStrings = new string[] { "{VERSION}", "{Policy_PS_LocalIntranet}" };

		// Token: 0x040017F2 RID: 6130
		private static readonly string s_internetPermissionSet = "<PermissionSet class=\"System.Security.NamedPermissionSet\"version=\"1\" Name=\"Internet\" Description=\"{Policy_PS_Internet}\"><Permission class=\"System.Security.Permissions.FileDialogPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Access=\"Open\"/><Permission class=\"System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" UserQuota=\"512000\" Allowed=\"ApplicationIsolationByUser\"/><Permission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"Execution\"/><Permission class=\"System.Security.Permissions.UIPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Window=\"SafeTopLevelWindows\" Clipboard=\"OwnClipboard\"/><IPermission class=\"System.Drawing.Printing.PrintingPermission, System.Drawing, Version={VERSION}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"version=\"1\"Level=\"SafePrinting\"/></PermissionSet>";

		// Token: 0x040017F3 RID: 6131
		private static readonly string s_localIntranetPermissionSet = "<PermissionSet class=\"System.Security.NamedPermissionSet\"version=\"1\" Name=\"LocalIntranet\" Description=\"{Policy_PS_LocalIntranet}\"><Permission class=\"System.Security.Permissions.EnvironmentPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Read=\"USERNAME\"/><Permission class=\"System.Security.Permissions.FileDialogPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><Permission class=\"System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Allowed=\"AssemblyIsolationByUser\" UserQuota=\"9223372036854775807\" Expiry=\"9223372036854775807\" Permanent=\"true\"/><Permission class=\"System.Security.Permissions.ReflectionPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"ReflectionEmit\"/><Permission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Flags=\"Execution, Assertion, BindingRedirects\"/><Permission class=\"System.Security.Permissions.UIPermission, mscorlib, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><IPermission class=\"System.Net.DnsPermission, System, Version={VERSION}, Culture=neutral, PublicKeyToken=b77a5c561934e089\"version=\"1\" Unrestricted=\"true\"/><IPermission class=\"System.Drawing.Printing.PrintingPermission, System.Drawing, Version={VERSION}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"version=\"1\"Level=\"DefaultPrinting\"/></PermissionSet>";

		// Token: 0x040017F4 RID: 6132
		private static readonly Version s_mscorlibVersion = Assembly.GetExecutingAssembly().GetVersion();

		// Token: 0x040017F5 RID: 6133
		private static readonly string[] s_reservedNamedPermissionSets = new string[] { "FullTrust", "Nothing", "Execution", "SkipVerification", "Internet", "LocalIntranet" };

		// Token: 0x040017F6 RID: 6134
		private static readonly string[] s_extensibleNamedPermissionSets = new string[] { "Internet", "LocalIntranet" };

		// Token: 0x040017F7 RID: 6135
		private static string[][] s_extensibleNamedPermissionSetRegistryInfo;

		// Token: 0x040017F8 RID: 6136
		private static bool s_extensionsReadFromRegistry;

		// Token: 0x040017F9 RID: 6137
		private static string[] EcmaFullTrustAssemblies = new string[] { "mscorlib.resources", "System", "System.resources", "System.Xml", "System.Xml.resources", "System.Windows.Forms", "System.Windows.Forms.resources", "System.Data", "System.Data.resources" };

		// Token: 0x040017FA RID: 6138
		private static string[] MicrosoftFullTrustAssemblies = new string[]
		{
			"System.Security", "System.Security.resources", "System.Drawing", "System.Drawing.resources", "System.Messaging", "System.Messaging.resources", "System.ServiceProcess", "System.ServiceProcess.resources", "System.DirectoryServices", "System.DirectoryServices.resources",
			"System.Deployment", "System.Deployment.resources"
		};
	}
}

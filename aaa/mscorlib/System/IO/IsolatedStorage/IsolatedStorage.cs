using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000795 RID: 1941
	[ComVisible(true)]
	public abstract class IsolatedStorage : MarshalByRefObject
	{
		// Token: 0x06004570 RID: 17776 RVA: 0x000ECFB9 File Offset: 0x000EBFB9
		internal static bool IsRoaming(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x000ECFC4 File Offset: 0x000EBFC4
		internal bool IsRoaming()
		{
			return (this.m_Scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x000ECFD4 File Offset: 0x000EBFD4
		internal static bool IsDomain(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x000ECFDF File Offset: 0x000EBFDF
		internal bool IsDomain()
		{
			return (this.m_Scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x000ECFEF File Offset: 0x000EBFEF
		internal static bool IsMachine(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Machine) != IsolatedStorageScope.None;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x000ECFFB File Offset: 0x000EBFFB
		internal bool IsAssembly()
		{
			return (this.m_Scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None;
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x000ED00B File Offset: 0x000EC00B
		internal static bool IsApp(IsolatedStorageScope scope)
		{
			return (scope & IsolatedStorageScope.Application) != IsolatedStorageScope.None;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x000ED017 File Offset: 0x000EC017
		internal bool IsApp()
		{
			return (this.m_Scope & IsolatedStorageScope.Application) != IsolatedStorageScope.None;
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x000ED028 File Offset: 0x000EC028
		private string GetNameFromID(string typeID, string instanceID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(typeID);
			stringBuilder.Append(this.SeparatorInternal);
			stringBuilder.Append(instanceID);
			return stringBuilder.ToString();
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x000ED060 File Offset: 0x000EC060
		private static string GetPredefinedTypeName(object o)
		{
			if (o is Publisher)
			{
				return "Publisher";
			}
			if (o is StrongName)
			{
				return "StrongName";
			}
			if (o is Url)
			{
				return "Url";
			}
			if (o is Site)
			{
				return "Site";
			}
			if (o is Zone)
			{
				return "Zone";
			}
			return null;
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x000ED0B4 File Offset: 0x000EC0B4
		internal static string GetHash(Stream s)
		{
			string text;
			using (SHA1 sha = new SHA1CryptoServiceProvider())
			{
				byte[] array = sha.ComputeHash(s);
				text = IsolatedStorage.ToBase32StringSuitableForDirName(array);
			}
			return text;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x000ED0F4 File Offset: 0x000EC0F4
		internal static string ToBase32StringSuitableForDirName(byte[] buff)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = buff.Length;
			int num2 = 0;
			do
			{
				byte b = ((num2 < num) ? buff[num2++] : 0);
				byte b2 = ((num2 < num) ? buff[num2++] : 0);
				byte b3 = ((num2 < num) ? buff[num2++] : 0);
				byte b4 = ((num2 < num) ? buff[num2++] : 0);
				byte b5 = ((num2 < num) ? buff[num2++] : 0);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b2 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b3 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b4 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)(b5 & 31)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[((b & 224) >> 5) | ((b4 & 96) >> 2)]);
				stringBuilder.Append(IsolatedStorage.s_Base32Char[((b2 & 224) >> 5) | ((b5 & 96) >> 2)]);
				b3 = (byte)(b3 >> 5);
				if ((b4 & 128) != 0)
				{
					b3 |= 8;
				}
				if ((b5 & 128) != 0)
				{
					b3 |= 16;
				}
				stringBuilder.Append(IsolatedStorage.s_Base32Char[(int)b3]);
			}
			while (num2 < num);
			return stringBuilder.ToString();
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x000ED244 File Offset: 0x000EC244
		private static bool IsValidName(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsLetter(s[i]) && !char.IsDigit(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x000ED281 File Offset: 0x000EC281
		private static PermissionSet GetReflectionPermission()
		{
			if (IsolatedStorage.s_PermReflection == null)
			{
				IsolatedStorage.s_PermReflection = new PermissionSet(PermissionState.Unrestricted);
			}
			return IsolatedStorage.s_PermReflection;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x000ED29A File Offset: 0x000EC29A
		private static SecurityPermission GetControlEvidencePermission()
		{
			if (IsolatedStorage.s_PermControlEvidence == null)
			{
				IsolatedStorage.s_PermControlEvidence = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
			}
			return IsolatedStorage.s_PermControlEvidence;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x000ED2B4 File Offset: 0x000EC2B4
		private static PermissionSet GetExecutionPermission()
		{
			if (IsolatedStorage.s_PermExecution == null)
			{
				IsolatedStorage.s_PermExecution = new PermissionSet(PermissionState.None);
				IsolatedStorage.s_PermExecution.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			}
			return IsolatedStorage.s_PermExecution;
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x000ED2DE File Offset: 0x000EC2DE
		private static PermissionSet GetUnrestricted()
		{
			if (IsolatedStorage.s_PermUnrestricted == null)
			{
				IsolatedStorage.s_PermUnrestricted = new PermissionSet(PermissionState.Unrestricted);
			}
			return IsolatedStorage.s_PermUnrestricted;
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004581 RID: 17793 RVA: 0x000ED2F7 File Offset: 0x000EC2F7
		protected virtual char SeparatorExternal
		{
			get
			{
				return '\\';
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x000ED2FB File Offset: 0x000EC2FB
		protected virtual char SeparatorInternal
		{
			get
			{
				return '.';
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x000ED2FF File Offset: 0x000EC2FF
		[CLSCompliant(false)]
		public virtual ulong MaximumSize
		{
			get
			{
				if (this.m_ValidQuota)
				{
					return this.m_Quota;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_QuotaIsUndefined"));
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x000ED31F File Offset: 0x000EC31F
		[CLSCompliant(false)]
		public virtual ulong CurrentSize
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_CurrentSizeUndefined"));
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x000ED330 File Offset: 0x000EC330
		public object DomainIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsDomain())
				{
					return this.m_DomainIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_DomainUndefined"));
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x000ED350 File Offset: 0x000EC350
		[ComVisible(false)]
		public object ApplicationIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsApp())
				{
					return this.m_AppIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_ApplicationUndefined"));
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x000ED370 File Offset: 0x000EC370
		public object AssemblyIdentity
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (this.IsAssembly())
				{
					return this.m_AssemIdentity;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_AssemblyUndefined"));
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x000ED390 File Offset: 0x000EC390
		internal MemoryStream GetIdentityStream(IsolatedStorageScope scope)
		{
			IsolatedStorage.GetReflectionPermission().Assert();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			object obj;
			if (IsolatedStorage.IsApp(scope))
			{
				obj = this.m_AppIdentity;
			}
			else if (IsolatedStorage.IsDomain(scope))
			{
				obj = this.m_DomainIdentity;
			}
			else
			{
				obj = this.m_AssemIdentity;
			}
			if (obj != null)
			{
				binaryFormatter.Serialize(memoryStream, obj);
			}
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004589 RID: 17801 RVA: 0x000ED3F0 File Offset: 0x000EC3F0
		public IsolatedStorageScope Scope
		{
			get
			{
				return this.m_Scope;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x000ED3F8 File Offset: 0x000EC3F8
		internal string DomainName
		{
			get
			{
				if (this.IsDomain())
				{
					return this.m_DomainName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_DomainUndefined"));
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600458B RID: 17803 RVA: 0x000ED418 File Offset: 0x000EC418
		internal string AssemName
		{
			get
			{
				if (this.IsAssembly())
				{
					return this.m_AssemName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_AssemblyUndefined"));
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x000ED438 File Offset: 0x000EC438
		internal string AppName
		{
			get
			{
				if (this.IsApp())
				{
					return this.m_AppName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_ApplicationUndefined"));
			}
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x000ED458 File Offset: 0x000EC458
		protected void InitStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet permissionSet2 = null;
			Assembly assembly = IsolatedStorage.nGetCaller();
			IsolatedStorage.GetControlEvidencePermission().Assert();
			if (IsolatedStorage.IsDomain(scope))
			{
				AppDomain domain = Thread.GetDomain();
				if (!IsolatedStorage.IsRoaming(scope))
				{
					domain.nGetGrantSet(out permissionSet, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				this._InitStore(scope, domain.Evidence, domainEvidenceType, assembly.Evidence, assemblyEvidenceType, null, null);
			}
			else
			{
				if (!IsolatedStorage.IsRoaming(scope))
				{
					assembly.nGetGrantSet(out permissionSet, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
					}
				}
				this._InitStore(scope, null, null, assembly.Evidence, assemblyEvidenceType, null, null);
			}
			this.SetQuota(permissionSet, permissionSet2);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x000ED504 File Offset: 0x000EC504
		protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet permissionSet2 = null;
			IsolatedStorage.nGetCaller();
			IsolatedStorage.GetControlEvidencePermission().Assert();
			if (IsolatedStorage.IsApp(scope))
			{
				AppDomain domain = Thread.GetDomain();
				if (!IsolatedStorage.IsRoaming(scope))
				{
					domain.nGetGrantSet(out permissionSet, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
				if (activationContext == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
				ApplicationSecurityInfo applicationSecurityInfo = new ApplicationSecurityInfo(activationContext);
				this._InitStore(scope, null, null, null, null, applicationSecurityInfo.ApplicationEvidence, appEvidenceType);
			}
			this.SetQuota(permissionSet, permissionSet2);
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x000ED59C File Offset: 0x000EC59C
		internal void InitStore(IsolatedStorageScope scope, object domain, object assem, object app)
		{
			PermissionSet permissionSet = null;
			PermissionSet permissionSet2 = null;
			Evidence evidence = null;
			Evidence evidence2 = null;
			Evidence evidence3 = null;
			if (IsolatedStorage.IsApp(scope))
			{
				evidence3 = new Evidence();
				evidence3.AddHost(app);
			}
			else
			{
				evidence2 = new Evidence();
				evidence2.AddHost(assem);
				if (IsolatedStorage.IsDomain(scope))
				{
					evidence = new Evidence();
					evidence.AddHost(domain);
				}
			}
			this._InitStore(scope, evidence, null, evidence2, null, evidence3, null);
			if (!IsolatedStorage.IsRoaming(scope))
			{
				Assembly assembly = IsolatedStorage.nGetCaller();
				IsolatedStorage.GetControlEvidencePermission().Assert();
				assembly.nGetGrantSet(out permissionSet, out permissionSet2);
				if (permissionSet == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
				}
			}
			this.SetQuota(permissionSet, permissionSet2);
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x000ED640 File Offset: 0x000EC640
		internal void InitStore(IsolatedStorageScope scope, Evidence domainEv, Type domainEvidenceType, Evidence assemEv, Type assemEvidenceType, Evidence appEv, Type appEvidenceType)
		{
			PermissionSet permissionSet = null;
			PermissionSet permissionSet2 = null;
			if (!IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorage.IsApp(scope))
				{
					permissionSet = SecurityManager.ResolvePolicy(appEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationGrantSet"));
					}
				}
				else if (IsolatedStorage.IsDomain(scope))
				{
					permissionSet = SecurityManager.ResolvePolicy(domainEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainGrantSet"));
					}
				}
				else
				{
					permissionSet = SecurityManager.ResolvePolicy(assemEv, IsolatedStorage.GetExecutionPermission(), IsolatedStorage.GetUnrestricted(), null, out permissionSet2);
					if (permissionSet == null)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyGrantSet"));
					}
				}
			}
			this._InitStore(scope, domainEv, domainEvidenceType, assemEv, assemEvidenceType, appEv, appEvidenceType);
			this.SetQuota(permissionSet, permissionSet2);
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x000ED6FC File Offset: 0x000EC6FC
		internal bool InitStore(IsolatedStorageScope scope, Stream domain, Stream assem, Stream app, string domainName, string assemName, string appName)
		{
			try
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				if (IsolatedStorage.IsApp(scope))
				{
					this.m_AppIdentity = binaryFormatter.Deserialize(app);
					this.m_AppName = appName;
				}
				else
				{
					this.m_AssemIdentity = binaryFormatter.Deserialize(assem);
					this.m_AssemName = assemName;
					if (IsolatedStorage.IsDomain(scope))
					{
						this.m_DomainIdentity = binaryFormatter.Deserialize(domain);
						this.m_DomainName = domainName;
					}
				}
			}
			catch
			{
				return false;
			}
			this.m_Scope = scope;
			return true;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x000ED78C File Offset: 0x000EC78C
		private void _InitStore(IsolatedStorageScope scope, Evidence domainEv, Type domainEvidenceType, Evidence assemEv, Type assemblyEvidenceType, Evidence appEv, Type appEvidenceType)
		{
			IsolatedStorage.VerifyScope(scope);
			if (IsolatedStorage.IsApp(scope))
			{
				if (appEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
			}
			else
			{
				if (assemEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyMissingIdentity"));
				}
				if (IsolatedStorage.IsDomain(scope) && domainEv == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainMissingIdentity"));
				}
			}
			IsolatedStorage.DemandPermission(scope);
			string text = null;
			string text2 = null;
			if (IsolatedStorage.IsApp(scope))
			{
				this.m_AppIdentity = IsolatedStorage.GetAccountingInfo(appEv, appEvidenceType, IsolatedStorageScope.Application, out text, out text2);
				this.m_AppName = this.GetNameFromID(text, text2);
			}
			else
			{
				this.m_AssemIdentity = IsolatedStorage.GetAccountingInfo(assemEv, assemblyEvidenceType, IsolatedStorageScope.Assembly, out text, out text2);
				this.m_AssemName = this.GetNameFromID(text, text2);
				if (IsolatedStorage.IsDomain(scope))
				{
					this.m_DomainIdentity = IsolatedStorage.GetAccountingInfo(domainEv, domainEvidenceType, IsolatedStorageScope.Domain, out text, out text2);
					this.m_DomainName = this.GetNameFromID(text, text2);
				}
			}
			this.m_Scope = scope;
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x000ED874 File Offset: 0x000EC874
		private static object GetAccountingInfo(Evidence evidence, Type evidenceType, IsolatedStorageScope fAssmDomApp, out string typeName, out string instanceName)
		{
			object obj = null;
			object obj2 = IsolatedStorage._GetAccountingInfo(evidence, evidenceType, fAssmDomApp, out obj);
			typeName = IsolatedStorage.GetPredefinedTypeName(obj2);
			if (typeName == null)
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				MemoryStream memoryStream = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj2.GetType());
				memoryStream.Position = 0L;
				typeName = IsolatedStorage.GetHash(memoryStream);
			}
			instanceName = null;
			if (obj != null)
			{
				if (obj is Stream)
				{
					instanceName = IsolatedStorage.GetHash((Stream)obj);
				}
				else if (obj is string)
				{
					if (IsolatedStorage.IsValidName((string)obj))
					{
						instanceName = (string)obj;
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream();
						BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
						binaryWriter.Write((string)obj);
						memoryStream.Position = 0L;
						instanceName = IsolatedStorage.GetHash(memoryStream);
					}
				}
			}
			else
			{
				obj = obj2;
			}
			if (instanceName == null)
			{
				IsolatedStorage.GetReflectionPermission().Assert();
				MemoryStream memoryStream = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = 0L;
				instanceName = IsolatedStorage.GetHash(memoryStream);
			}
			return obj2;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x000ED974 File Offset: 0x000EC974
		private static object _GetAccountingInfo(Evidence evidence, Type evidenceType, IsolatedStorageScope fAssmDomApp, out object oNormalized)
		{
			object obj = null;
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			if (evidenceType == null)
			{
				Publisher publisher = null;
				StrongName strongName = null;
				Url url = null;
				Site site = null;
				Zone zone = null;
				while (hostEnumerator.MoveNext())
				{
					obj = hostEnumerator.Current;
					if (obj is Publisher)
					{
						publisher = (Publisher)obj;
						break;
					}
					if (obj is StrongName)
					{
						strongName = (StrongName)obj;
					}
					else if (obj is Url)
					{
						url = (Url)obj;
					}
					else if (obj is Site)
					{
						site = (Site)obj;
					}
					else if (obj is Zone)
					{
						zone = (Zone)obj;
					}
				}
				if (publisher != null)
				{
					obj = publisher;
				}
				else if (strongName != null)
				{
					obj = strongName;
				}
				else if (url != null)
				{
					obj = url;
				}
				else if (site != null)
				{
					obj = site;
				}
				else if (zone != null)
				{
					obj = zone;
				}
				else
				{
					if (fAssmDomApp == IsolatedStorageScope.Domain)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainNoEvidence"));
					}
					if (fAssmDomApp == IsolatedStorageScope.Application)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationNoEvidence"));
					}
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyNoEvidence"));
				}
			}
			else
			{
				while (hostEnumerator.MoveNext())
				{
					object obj2 = hostEnumerator.Current;
					if (obj2.GetType().Equals(evidenceType))
					{
						obj = obj2;
						break;
					}
				}
				if (obj == null)
				{
					if (fAssmDomApp == IsolatedStorageScope.Domain)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DomainNoEvidence"));
					}
					if (fAssmDomApp == IsolatedStorageScope.Application)
					{
						throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationNoEvidence"));
					}
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_AssemblyNoEvidence"));
				}
			}
			if (obj is INormalizeForIsolatedStorage)
			{
				oNormalized = ((INormalizeForIsolatedStorage)obj).Normalize();
			}
			else if (obj is Publisher)
			{
				oNormalized = ((Publisher)obj).Normalize();
			}
			else if (obj is StrongName)
			{
				oNormalized = ((StrongName)obj).Normalize();
			}
			else if (obj is Url)
			{
				oNormalized = ((Url)obj).Normalize();
			}
			else if (obj is Site)
			{
				oNormalized = ((Site)obj).Normalize();
			}
			else if (obj is Zone)
			{
				oNormalized = ((Zone)obj).Normalize();
			}
			else
			{
				oNormalized = null;
			}
			return obj;
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x000EDB64 File Offset: 0x000ECB64
		private static void DemandPermission(IsolatedStorageScope scope)
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission = null;
			if (scope <= (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
			{
				switch (scope)
				{
				case IsolatedStorageScope.User | IsolatedStorageScope.Assembly:
					if (IsolatedStorage.s_PermAssem == null)
					{
						IsolatedStorage.s_PermAssem = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermAssem;
					break;
				case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
					break;
				case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
					if (IsolatedStorage.s_PermDomain == null)
					{
						IsolatedStorage.s_PermDomain = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermDomain;
					break;
				default:
					switch (scope)
					{
					case IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						if (IsolatedStorage.s_PermAssemRoaming == null)
						{
							IsolatedStorage.s_PermAssemRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByRoamingUser, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermAssemRoaming;
						break;
					case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						break;
					case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
						if (IsolatedStorage.s_PermDomainRoaming == null)
						{
							IsolatedStorage.s_PermDomainRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByRoamingUser, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermDomainRoaming;
						break;
					default:
						switch (scope)
						{
						case IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
							if (IsolatedStorage.s_PermMachineAssem == null)
							{
								IsolatedStorage.s_PermMachineAssem = new IsolatedStorageFilePermission(IsolatedStorageContainment.AssemblyIsolationByMachine, 0L, false);
							}
							isolatedStorageFilePermission = IsolatedStorage.s_PermMachineAssem;
							break;
						case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
							if (IsolatedStorage.s_PermMachineDomain == null)
							{
								IsolatedStorage.s_PermMachineDomain = new IsolatedStorageFilePermission(IsolatedStorageContainment.DomainIsolationByMachine, 0L, false);
							}
							isolatedStorageFilePermission = IsolatedStorage.s_PermMachineDomain;
							break;
						}
						break;
					}
					break;
				}
			}
			else if (scope != (IsolatedStorageScope.User | IsolatedStorageScope.Application))
			{
				if (scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
				{
					if (scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application))
					{
						if (IsolatedStorage.s_PermAppMachine == null)
						{
							IsolatedStorage.s_PermAppMachine = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByMachine, 0L, false);
						}
						isolatedStorageFilePermission = IsolatedStorage.s_PermAppMachine;
					}
				}
				else
				{
					if (IsolatedStorage.s_PermAppUserRoaming == null)
					{
						IsolatedStorage.s_PermAppUserRoaming = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByRoamingUser, 0L, false);
					}
					isolatedStorageFilePermission = IsolatedStorage.s_PermAppUserRoaming;
				}
			}
			else
			{
				if (IsolatedStorage.s_PermAppUser == null)
				{
					IsolatedStorage.s_PermAppUser = new IsolatedStorageFilePermission(IsolatedStorageContainment.ApplicationIsolationByUser, 0L, false);
				}
				isolatedStorageFilePermission = IsolatedStorage.s_PermAppUser;
			}
			isolatedStorageFilePermission.Demand();
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x000EDCF8 File Offset: 0x000ECCF8
		internal static void VerifyScope(IsolatedStorageScope scope)
		{
			if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming) || scope == (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine) || scope == (IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
			{
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_Scope_Invalid"));
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x000EDD38 File Offset: 0x000ECD38
		internal void SetQuota(PermissionSet psAllowed, PermissionSet psDenied)
		{
			IsolatedStoragePermission permission = this.GetPermission(psAllowed);
			this.m_Quota = 0UL;
			if (permission != null)
			{
				if (permission.IsUnrestricted())
				{
					this.m_Quota = 9223372036854775807UL;
				}
				else
				{
					this.m_Quota = (ulong)permission.UserQuota;
				}
			}
			if (psDenied != null)
			{
				IsolatedStoragePermission permission2 = this.GetPermission(psDenied);
				if (permission2 != null)
				{
					if (permission2.IsUnrestricted())
					{
						this.m_Quota = 0UL;
					}
					else
					{
						ulong userQuota = (ulong)permission2.UserQuota;
						if (userQuota > this.m_Quota)
						{
							this.m_Quota = 0UL;
						}
						else
						{
							this.m_Quota -= userQuota;
						}
					}
				}
			}
			this.m_ValidQuota = true;
		}

		// Token: 0x06004598 RID: 17816
		public abstract void Remove();

		// Token: 0x06004599 RID: 17817
		protected abstract IsolatedStoragePermission GetPermission(PermissionSet ps);

		// Token: 0x0600459A RID: 17818
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Assembly nGetCaller();

		// Token: 0x0400225E RID: 8798
		internal const IsolatedStorageScope c_Assembly = IsolatedStorageScope.User | IsolatedStorageScope.Assembly;

		// Token: 0x0400225F RID: 8799
		internal const IsolatedStorageScope c_Domain = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly;

		// Token: 0x04002260 RID: 8800
		internal const IsolatedStorageScope c_AssemblyRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;

		// Token: 0x04002261 RID: 8801
		internal const IsolatedStorageScope c_DomainRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;

		// Token: 0x04002262 RID: 8802
		internal const IsolatedStorageScope c_MachineAssembly = IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;

		// Token: 0x04002263 RID: 8803
		internal const IsolatedStorageScope c_MachineDomain = IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;

		// Token: 0x04002264 RID: 8804
		internal const IsolatedStorageScope c_AppUser = IsolatedStorageScope.User | IsolatedStorageScope.Application;

		// Token: 0x04002265 RID: 8805
		internal const IsolatedStorageScope c_AppMachine = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;

		// Token: 0x04002266 RID: 8806
		internal const IsolatedStorageScope c_AppUserRoaming = IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application;

		// Token: 0x04002267 RID: 8807
		private const string s_Publisher = "Publisher";

		// Token: 0x04002268 RID: 8808
		private const string s_StrongName = "StrongName";

		// Token: 0x04002269 RID: 8809
		private const string s_Site = "Site";

		// Token: 0x0400226A RID: 8810
		private const string s_Url = "Url";

		// Token: 0x0400226B RID: 8811
		private const string s_Zone = "Zone";

		// Token: 0x0400226C RID: 8812
		private static char[] s_Base32Char = new char[]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
			'4', '5'
		};

		// Token: 0x0400226D RID: 8813
		private ulong m_Quota;

		// Token: 0x0400226E RID: 8814
		private bool m_ValidQuota;

		// Token: 0x0400226F RID: 8815
		private object m_DomainIdentity;

		// Token: 0x04002270 RID: 8816
		private object m_AssemIdentity;

		// Token: 0x04002271 RID: 8817
		private object m_AppIdentity;

		// Token: 0x04002272 RID: 8818
		private string m_DomainName;

		// Token: 0x04002273 RID: 8819
		private string m_AssemName;

		// Token: 0x04002274 RID: 8820
		private string m_AppName;

		// Token: 0x04002275 RID: 8821
		private IsolatedStorageScope m_Scope;

		// Token: 0x04002276 RID: 8822
		private static IsolatedStorageFilePermission s_PermDomain;

		// Token: 0x04002277 RID: 8823
		private static IsolatedStorageFilePermission s_PermMachineDomain;

		// Token: 0x04002278 RID: 8824
		private static IsolatedStorageFilePermission s_PermDomainRoaming;

		// Token: 0x04002279 RID: 8825
		private static IsolatedStorageFilePermission s_PermAssem;

		// Token: 0x0400227A RID: 8826
		private static IsolatedStorageFilePermission s_PermMachineAssem;

		// Token: 0x0400227B RID: 8827
		private static IsolatedStorageFilePermission s_PermAssemRoaming;

		// Token: 0x0400227C RID: 8828
		private static IsolatedStorageFilePermission s_PermAppUser;

		// Token: 0x0400227D RID: 8829
		private static IsolatedStorageFilePermission s_PermAppMachine;

		// Token: 0x0400227E RID: 8830
		private static IsolatedStorageFilePermission s_PermAppUserRoaming;

		// Token: 0x0400227F RID: 8831
		private static SecurityPermission s_PermControlEvidence;

		// Token: 0x04002280 RID: 8832
		private static PermissionSet s_PermReflection;

		// Token: 0x04002281 RID: 8833
		private static PermissionSet s_PermUnrestricted;

		// Token: 0x04002282 RID: 8834
		private static PermissionSet s_PermExecution;
	}
}

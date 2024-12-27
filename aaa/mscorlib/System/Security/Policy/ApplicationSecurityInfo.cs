using System;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;
using System.Threading;

namespace System.Security.Policy
{
	// Token: 0x02000481 RID: 1153
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ApplicationSecurityInfo
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x0009CB08 File Offset: 0x0009BB08
		internal ApplicationSecurityInfo()
		{
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x0009CB10 File Offset: 0x0009BB10
		public ApplicationSecurityInfo(ActivationContext activationContext)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			this.m_context = activationContext;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x0009CB30 File Offset: 0x0009BB30
		// (set) Token: 0x06002E3D RID: 11837 RVA: 0x0009CB79 File Offset: 0x0009BB79
		public ApplicationId ApplicationId
		{
			get
			{
				if (this.m_appId == null && this.m_context != null)
				{
					ICMS applicationComponentManifest = this.m_context.ApplicationComponentManifest;
					ApplicationId applicationId = ApplicationSecurityInfo.ParseApplicationId(applicationComponentManifest);
					Interlocked.CompareExchange(ref this.m_appId, applicationId, null);
				}
				return this.m_appId as ApplicationId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_appId = value;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x0009CB90 File Offset: 0x0009BB90
		// (set) Token: 0x06002E3F RID: 11839 RVA: 0x0009CBD9 File Offset: 0x0009BBD9
		public ApplicationId DeploymentId
		{
			get
			{
				if (this.m_deployId == null && this.m_context != null)
				{
					ICMS deploymentComponentManifest = this.m_context.DeploymentComponentManifest;
					ApplicationId applicationId = ApplicationSecurityInfo.ParseApplicationId(deploymentComponentManifest);
					Interlocked.CompareExchange(ref this.m_deployId, applicationId, null);
				}
				return this.m_deployId as ApplicationId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_deployId = value;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x0009CBF0 File Offset: 0x0009BBF0
		// (set) Token: 0x06002E41 RID: 11841 RVA: 0x0009CDA3 File Offset: 0x0009BDA3
		public PermissionSet DefaultRequestSet
		{
			get
			{
				if (this.m_defaultRequest == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					if (this.m_context != null)
					{
						ICMS applicationComponentManifest = this.m_context.ApplicationComponentManifest;
						string defaultPermissionSetID = ((IMetadataSectionEntry)applicationComponentManifest.MetadataSectionEntry).defaultPermissionSetID;
						object obj = null;
						if (defaultPermissionSetID != null && defaultPermissionSetID.Length > 0)
						{
							((ISectionWithStringKey)applicationComponentManifest.PermissionSetSection).Lookup(defaultPermissionSetID, out obj);
							IPermissionSetEntry permissionSetEntry = obj as IPermissionSetEntry;
							if (permissionSetEntry != null)
							{
								SecurityElement securityElement = SecurityElement.FromString(permissionSetEntry.AllData.XmlSegment);
								string text = securityElement.Attribute("temp:Unrestricted");
								if (text != null)
								{
									securityElement.AddAttribute("Unrestricted", text);
								}
								permissionSet = new PermissionSet(PermissionState.None);
								permissionSet.FromXml(securityElement);
								string text2 = securityElement.Attribute("SameSite");
								if (string.Compare(text2, "Site", StringComparison.OrdinalIgnoreCase) == 0)
								{
									NetCodeGroup netCodeGroup = new NetCodeGroup(new AllMembershipCondition());
									Url url = new Url(this.m_context.Identity.CodeBase);
									PolicyStatement policyStatement = netCodeGroup.CalculatePolicy(url.GetURLString().Host, url.GetURLString().Scheme, url.GetURLString().Port);
									if (policyStatement != null)
									{
										PermissionSet permissionSet2 = policyStatement.PermissionSet;
										if (permissionSet2 != null)
										{
											permissionSet.InplaceUnion(permissionSet2);
										}
									}
									if (string.Compare("file:", 0, this.m_context.Identity.CodeBase, 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
									{
										FileCodeGroup fileCodeGroup = new FileCodeGroup(new AllMembershipCondition(), FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery);
										policyStatement = fileCodeGroup.CalculatePolicy(url);
										if (policyStatement != null)
										{
											PermissionSet permissionSet3 = policyStatement.PermissionSet;
											if (permissionSet3 != null)
											{
												permissionSet.InplaceUnion(permissionSet3);
											}
										}
									}
								}
							}
						}
					}
					Interlocked.CompareExchange(ref this.m_defaultRequest, permissionSet, null);
				}
				return this.m_defaultRequest as PermissionSet;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_defaultRequest = value;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x0009CDBC File Offset: 0x0009BDBC
		// (set) Token: 0x06002E43 RID: 11843 RVA: 0x0009CEB9 File Offset: 0x0009BEB9
		public Evidence ApplicationEvidence
		{
			get
			{
				if (this.m_appEvidence == null)
				{
					Evidence evidence = new Evidence();
					if (this.m_context != null)
					{
						evidence = new Evidence();
						Url url = new Url(this.m_context.Identity.CodeBase);
						evidence.AddHost(url);
						evidence.AddHost(Zone.CreateFromUrl(this.m_context.Identity.CodeBase));
						if (string.Compare("file:", 0, this.m_context.Identity.CodeBase, 0, 5, StringComparison.OrdinalIgnoreCase) != 0)
						{
							evidence.AddHost(Site.CreateFromUrl(this.m_context.Identity.CodeBase));
						}
						evidence.AddHost(new StrongName(new StrongNamePublicKeyBlob(this.DeploymentId.m_publicKeyToken), this.DeploymentId.Name, this.DeploymentId.Version));
						evidence.AddHost(new ActivationArguments(this.m_context));
					}
					Interlocked.CompareExchange(ref this.m_appEvidence, evidence, null);
				}
				return this.m_appEvidence as Evidence;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_appEvidence = value;
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0009CED0 File Offset: 0x0009BED0
		private static ApplicationId ParseApplicationId(ICMS manifest)
		{
			if (manifest.Identity == null)
			{
				return null;
			}
			return new ApplicationId(Hex.DecodeHexString(manifest.Identity.GetAttribute("", "publicKeyToken")), manifest.Identity.GetAttribute("", "name"), new Version(manifest.Identity.GetAttribute("", "version")), manifest.Identity.GetAttribute("", "processorArchitecture"), manifest.Identity.GetAttribute("", "culture"));
		}

		// Token: 0x0400177E RID: 6014
		private ActivationContext m_context;

		// Token: 0x0400177F RID: 6015
		private object m_appId;

		// Token: 0x04001780 RID: 6016
		private object m_deployId;

		// Token: 0x04001781 RID: 6017
		private object m_defaultRequest;

		// Token: 0x04001782 RID: 6018
		private object m_appEvidence;
	}
}

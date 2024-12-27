using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000095 RID: 149
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class Domain : ActiveDirectoryPartition
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x0001ADBD File Offset: 0x00019DBD
		internal Domain(DirectoryContext context, string domainName, DirectoryEntryManager directoryEntryMgr)
			: base(context, domainName)
		{
			this.directoryEntryMgr = directoryEntryMgr;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001ADD5 File Offset: 0x00019DD5
		internal Domain(DirectoryContext context, string domainName)
			: this(context, domainName, new DirectoryEntryManager(context))
		{
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001ADE8 File Offset: 0x00019DE8
		public static Domain GetDomain(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain && context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeServerORDomain"), "context");
			}
			if (context.Name == null && !context.isDomain())
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ContextNotAssociatedWithDomain"), typeof(Domain), null);
			}
			if (context.Name == null || context.isDomain() || context.isServer())
			{
				context = new DirectoryContext(context);
				DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(context);
				string text = null;
				try
				{
					DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
					if (context.isServer() && !Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectory))
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(Domain), null);
					}
					text = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.DefaultNamingContext);
				}
				catch (COMException ex)
				{
					int errorCode = ex.ErrorCode;
					if (errorCode != -2147016646)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
					}
					if (context.ContextType == DirectoryContextType.Domain)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainNotFound"), typeof(Domain), context.Name);
					}
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(Domain), null);
				}
				return new Domain(context, Utils.GetDnsNameFromDN(text), directoryEntryManager);
			}
			if (context.ContextType == DirectoryContextType.Domain)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainNotFound"), typeof(Domain), context.Name);
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(Domain), null);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001AFC0 File Offset: 0x00019FC0
		public static Domain GetComputerDomain()
		{
			string dnsDomainName = DirectoryContext.GetDnsDomainName(null);
			if (dnsDomainName == null)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ComputerNotJoinedToDomain"), typeof(Domain), null);
			}
			return Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, dnsDomainName));
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001B000 File Offset: 0x0001A000
		public void RaiseDomainFunctionality(DomainMode domainMode)
		{
			base.CheckIfDisposed();
			if (domainMode < DomainMode.Windows2000MixedDomain || domainMode > DomainMode.Windows2008R2Domain)
			{
				throw new InvalidEnumArgumentException("domainMode", (int)domainMode, typeof(DomainMode));
			}
			DomainMode domainMode2 = this.GetDomainMode();
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext));
				switch (domainMode2)
				{
				case DomainMode.Windows2000MixedDomain:
					if (domainMode == DomainMode.Windows2000NativeDomain)
					{
						directoryEntry.Properties[PropertyManager.NTMixedDomain].Value = 0;
					}
					else if (domainMode == DomainMode.Windows2003InterimDomain)
					{
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 1;
					}
					else
					{
						if (domainMode != DomainMode.Windows2003Domain)
						{
							throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
						}
						directoryEntry.Properties[PropertyManager.NTMixedDomain].Value = 0;
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 2;
					}
					break;
				case DomainMode.Windows2000NativeDomain:
					if (domainMode == DomainMode.Windows2003Domain)
					{
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 2;
					}
					else if (domainMode == DomainMode.Windows2008Domain)
					{
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 3;
					}
					else
					{
						if (domainMode != DomainMode.Windows2008R2Domain)
						{
							throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
						}
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 4;
					}
					break;
				case DomainMode.Windows2003InterimDomain:
					if (domainMode != DomainMode.Windows2003Domain)
					{
						throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
					}
					directoryEntry.Properties[PropertyManager.NTMixedDomain].Value = 0;
					directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 2;
					break;
				case DomainMode.Windows2003Domain:
					if (domainMode == DomainMode.Windows2008Domain)
					{
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 3;
					}
					else
					{
						if (domainMode != DomainMode.Windows2008R2Domain)
						{
							throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
						}
						directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 4;
					}
					break;
				case DomainMode.Windows2008Domain:
					if (domainMode != DomainMode.Windows2008R2Domain)
					{
						throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
					}
					directoryEntry.Properties[PropertyManager.MsDSBehaviorVersion].Value = 4;
					break;
				case DomainMode.Windows2008R2Domain:
					throw new ArgumentException(Res.GetString("InvalidMode"), "domainMode");
				default:
					throw new ActiveDirectoryOperationException();
				}
				directoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147016694)
				{
					throw new ArgumentException(Res.GetString("NoW2K3DCs"), "domainMode");
				}
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			this.currentDomainMode = (DomainMode)(-1);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001B314 File Offset: 0x0001A314
		public DomainController FindDomainController()
		{
			base.CheckIfDisposed();
			return DomainController.FindOneInternal(this.context, base.Name, null, (LocatorOptions)0L);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001B330 File Offset: 0x0001A330
		public DomainController FindDomainController(string siteName)
		{
			base.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return DomainController.FindOneInternal(this.context, base.Name, siteName, (LocatorOptions)0L);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001B35A File Offset: 0x0001A35A
		public DomainController FindDomainController(LocatorOptions flag)
		{
			base.CheckIfDisposed();
			return DomainController.FindOneInternal(this.context, base.Name, null, flag);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001B375 File Offset: 0x0001A375
		public DomainController FindDomainController(string siteName, LocatorOptions flag)
		{
			base.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return DomainController.FindOneInternal(this.context, base.Name, siteName, flag);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001B39E File Offset: 0x0001A39E
		public DomainControllerCollection FindAllDomainControllers()
		{
			base.CheckIfDisposed();
			return DomainController.FindAllInternal(this.context, base.Name, true, null);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001B3B9 File Offset: 0x0001A3B9
		public DomainControllerCollection FindAllDomainControllers(string siteName)
		{
			base.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return DomainController.FindAllInternal(this.context, base.Name, true, siteName);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001B3E4 File Offset: 0x0001A3E4
		public DomainControllerCollection FindAllDiscoverableDomainControllers()
		{
			long num = 4096L;
			base.CheckIfDisposed();
			return new DomainControllerCollection(Locator.EnumerateDomainControllers(this.context, base.Name, null, num));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001B418 File Offset: 0x0001A418
		public DomainControllerCollection FindAllDiscoverableDomainControllers(string siteName)
		{
			long num = 4096L;
			base.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			if (siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			return new DomainControllerCollection(Locator.EnumerateDomainControllers(this.context, base.Name, siteName, num));
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001B475 File Offset: 0x0001A475
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override DirectoryEntry GetDirectoryEntry()
		{
			base.CheckIfDisposed();
			return DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001B494 File Offset: 0x0001A494
		public TrustRelationshipInformationCollection GetAllTrustRelationships()
		{
			base.CheckIfDisposed();
			ArrayList trustsHelper = this.GetTrustsHelper(null);
			return new TrustRelationshipInformationCollection(this.context, base.Name, trustsHelper);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001B4C4 File Offset: 0x0001A4C4
		public TrustRelationshipInformation GetTrustRelationship(string targetDomainName)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			ArrayList trustsHelper = this.GetTrustsHelper(targetDomainName);
			TrustRelationshipInformationCollection trustRelationshipInformationCollection = new TrustRelationshipInformationCollection(this.context, base.Name, trustsHelper);
			if (trustRelationshipInformationCollection.Count == 0)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DomainTrustDoesNotExist", new object[] { base.Name, targetDomainName }), typeof(TrustRelationshipInformation), null);
			}
			return trustRelationshipInformationCollection[0];
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001B55C File Offset: 0x0001A55C
		public bool GetSelectiveAuthenticationStatus(string targetDomainName)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			return TrustHelper.GetTrustedDomainInfoStatus(this.context, base.Name, targetDomainName, TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION, false);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001B5B0 File Offset: 0x0001A5B0
		public void SetSelectiveAuthenticationStatus(string targetDomainName, bool enable)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			TrustHelper.SetTrustedDomainInfoStatus(this.context, base.Name, targetDomainName, TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_CROSS_ORGANIZATION, enable, false);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001B604 File Offset: 0x0001A604
		public bool GetSidFilteringStatus(string targetDomainName)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			return TrustHelper.GetTrustedDomainInfoStatus(this.context, base.Name, targetDomainName, TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN, false);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001B658 File Offset: 0x0001A658
		public void SetSidFilteringStatus(string targetDomainName, bool enable)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			TrustHelper.SetTrustedDomainInfoStatus(this.context, base.Name, targetDomainName, TRUST_ATTRIBUTE.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN, enable, false);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001B6AC File Offset: 0x0001A6AC
		public void DeleteLocalSideOfTrustRelationship(string targetDomainName)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			TrustHelper.DeleteTrust(this.context, base.Name, targetDomainName, false);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001B700 File Offset: 0x0001A700
		public void DeleteTrustRelationship(Domain targetDomain)
		{
			base.CheckIfDisposed();
			if (targetDomain == null)
			{
				throw new ArgumentNullException("targetDomain");
			}
			TrustHelper.DeleteTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, false);
			TrustHelper.DeleteTrust(this.context, base.Name, targetDomain.Name, false);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001B754 File Offset: 0x0001A754
		public void VerifyOutboundTrustRelationship(string targetDomainName)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			TrustHelper.VerifyTrust(this.context, base.Name, targetDomainName, false, TrustDirection.Outbound, false, null);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001B7A8 File Offset: 0x0001A7A8
		public void VerifyTrustRelationship(Domain targetDomain, TrustDirection direction)
		{
			base.CheckIfDisposed();
			if (targetDomain == null)
			{
				throw new ArgumentNullException("targetDomain");
			}
			if (direction < TrustDirection.Inbound || direction > TrustDirection.Bidirectional)
			{
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(TrustDirection));
			}
			if ((direction & TrustDirection.Outbound) != (TrustDirection)0)
			{
				try
				{
					TrustHelper.VerifyTrust(this.context, base.Name, targetDomain.Name, false, TrustDirection.Outbound, false, null);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[] { base.Name, targetDomain.Name, direction }), typeof(TrustRelationshipInformation), null);
				}
			}
			if ((direction & TrustDirection.Inbound) != (TrustDirection)0)
			{
				try
				{
					TrustHelper.VerifyTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, false, TrustDirection.Outbound, false, null);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[] { base.Name, targetDomain.Name, direction }), typeof(TrustRelationshipInformation), null);
				}
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001B8C8 File Offset: 0x0001A8C8
		public void CreateLocalSideOfTrustRelationship(string targetDomainName, TrustDirection direction, string trustPassword)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			if (direction < TrustDirection.Inbound || direction > TrustDirection.Bidirectional)
			{
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(TrustDirection));
			}
			if (trustPassword == null)
			{
				throw new ArgumentNullException("trustPassword");
			}
			if (trustPassword.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "trustPassword");
			}
			Locator.GetDomainControllerInfo(null, targetDomainName, null, 16L);
			DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(targetDomainName, DirectoryContextType.Domain, this.context);
			TrustHelper.CreateTrust(this.context, base.Name, newDirectoryContext, targetDomainName, false, direction, trustPassword);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001B980 File Offset: 0x0001A980
		public void CreateTrustRelationship(Domain targetDomain, TrustDirection direction)
		{
			base.CheckIfDisposed();
			if (targetDomain == null)
			{
				throw new ArgumentNullException("targetDomain");
			}
			if (direction < TrustDirection.Inbound || direction > TrustDirection.Bidirectional)
			{
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(TrustDirection));
			}
			string text = TrustHelper.CreateTrustPassword();
			TrustHelper.CreateTrust(this.context, base.Name, targetDomain.GetDirectoryContext(), targetDomain.Name, false, direction, text);
			int num = 0;
			if ((direction & TrustDirection.Inbound) != (TrustDirection)0)
			{
				num |= 2;
			}
			if ((direction & TrustDirection.Outbound) != (TrustDirection)0)
			{
				num |= 1;
			}
			TrustHelper.CreateTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, this.context, base.Name, false, (TrustDirection)num, text);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001BA1C File Offset: 0x0001AA1C
		public void UpdateLocalSideOfTrustRelationship(string targetDomainName, string newTrustPassword)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			if (newTrustPassword == null)
			{
				throw new ArgumentNullException("newTrustPassword");
			}
			if (newTrustPassword.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "newTrustPassword");
			}
			TrustHelper.UpdateTrust(this.context, base.Name, targetDomainName, newTrustPassword, false);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001BA9C File Offset: 0x0001AA9C
		public void UpdateLocalSideOfTrustRelationship(string targetDomainName, TrustDirection newTrustDirection, string newTrustPassword)
		{
			base.CheckIfDisposed();
			if (targetDomainName == null)
			{
				throw new ArgumentNullException("targetDomainName");
			}
			if (targetDomainName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "targetDomainName");
			}
			if (newTrustDirection < TrustDirection.Inbound || newTrustDirection > TrustDirection.Bidirectional)
			{
				throw new InvalidEnumArgumentException("newTrustDirection", (int)newTrustDirection, typeof(TrustDirection));
			}
			if (newTrustPassword == null)
			{
				throw new ArgumentNullException("newTrustPassword");
			}
			if (newTrustPassword.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "newTrustPassword");
			}
			TrustHelper.UpdateTrustDirection(this.context, base.Name, targetDomainName, newTrustPassword, false, newTrustDirection);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001BB38 File Offset: 0x0001AB38
		public void UpdateTrustRelationship(Domain targetDomain, TrustDirection newTrustDirection)
		{
			base.CheckIfDisposed();
			if (targetDomain == null)
			{
				throw new ArgumentNullException("targetDomain");
			}
			if (newTrustDirection < TrustDirection.Inbound || newTrustDirection > TrustDirection.Bidirectional)
			{
				throw new InvalidEnumArgumentException("newTrustDirection", (int)newTrustDirection, typeof(TrustDirection));
			}
			string text = TrustHelper.CreateTrustPassword();
			TrustHelper.UpdateTrustDirection(this.context, base.Name, targetDomain.Name, text, false, newTrustDirection);
			TrustDirection trustDirection = (TrustDirection)0;
			if ((newTrustDirection & TrustDirection.Inbound) != (TrustDirection)0)
			{
				trustDirection |= TrustDirection.Outbound;
			}
			if ((newTrustDirection & TrustDirection.Outbound) != (TrustDirection)0)
			{
				trustDirection |= TrustDirection.Inbound;
			}
			TrustHelper.UpdateTrustDirection(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, text, false, trustDirection);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001BBC8 File Offset: 0x0001ABC8
		public void RepairTrustRelationship(Domain targetDomain)
		{
			TrustDirection trustDirection = TrustDirection.Bidirectional;
			base.CheckIfDisposed();
			if (targetDomain == null)
			{
				throw new ArgumentNullException("targetDomain");
			}
			try
			{
				trustDirection = this.GetTrustRelationship(targetDomain.Name).TrustDirection;
				if ((trustDirection & TrustDirection.Outbound) != (TrustDirection)0)
				{
					TrustHelper.VerifyTrust(this.context, base.Name, targetDomain.Name, false, TrustDirection.Outbound, true, null);
				}
				if ((trustDirection & TrustDirection.Inbound) != (TrustDirection)0)
				{
					TrustHelper.VerifyTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, false, TrustDirection.Outbound, true, null);
				}
			}
			catch (ActiveDirectoryOperationException)
			{
				this.RepairTrustHelper(targetDomain, trustDirection);
			}
			catch (UnauthorizedAccessException)
			{
				this.RepairTrustHelper(targetDomain, trustDirection);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[] { base.Name, targetDomain.Name, trustDirection }), typeof(TrustRelationshipInformation), null);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001BCBC File Offset: 0x0001ACBC
		public static Domain GetCurrentDomain()
		{
			return Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain));
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001BCCC File Offset: 0x0001ACCC
		public Forest Forest
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedForest == null)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
					string text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.RootDomainNamingContext);
					string dnsNameFromDN = Utils.GetDnsNameFromDN(text);
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(dnsNameFromDN, DirectoryContextType.Forest, this.context);
					this.cachedForest = new Forest(newDirectoryContext, dnsNameFromDN);
				}
				return this.cachedForest;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001BD33 File Offset: 0x0001AD33
		public DomainControllerCollection DomainControllers
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedDomainControllers == null)
				{
					this.cachedDomainControllers = this.FindAllDomainControllers();
				}
				return this.cachedDomainControllers;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001BD55 File Offset: 0x0001AD55
		public DomainCollection Children
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedChildren == null)
				{
					this.cachedChildren = new DomainCollection(this.GetChildDomains());
				}
				return this.cachedChildren;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001BD7C File Offset: 0x0001AD7C
		public DomainMode DomainMode
		{
			get
			{
				base.CheckIfDisposed();
				if (this.currentDomainMode == (DomainMode)(-1))
				{
					this.currentDomainMode = this.GetDomainMode();
				}
				return this.currentDomainMode;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001BD9F File Offset: 0x0001AD9F
		public Domain Parent
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.isParentInitialized)
				{
					this.cachedParent = this.GetParent();
					this.isParentInitialized = true;
				}
				return this.cachedParent;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001BDC8 File Offset: 0x0001ADC8
		public DomainController PdcRoleOwner
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedPdcRoleOwner == null)
				{
					this.cachedPdcRoleOwner = this.GetRoleOwner(ActiveDirectoryRole.PdcRole);
				}
				return this.cachedPdcRoleOwner;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001BDEB File Offset: 0x0001ADEB
		public DomainController RidRoleOwner
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedRidRoleOwner == null)
				{
					this.cachedRidRoleOwner = this.GetRoleOwner(ActiveDirectoryRole.RidRole);
				}
				return this.cachedRidRoleOwner;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0001BE0E File Offset: 0x0001AE0E
		public DomainController InfrastructureRoleOwner
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedInfrastructureRoleOwner == null)
				{
					this.cachedInfrastructureRoleOwner = this.GetRoleOwner(ActiveDirectoryRole.InfrastructureRole);
				}
				return this.cachedInfrastructureRoleOwner;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001BE31 File Offset: 0x0001AE31
		internal DirectoryContext GetDirectoryContext()
		{
			return this.context;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001BE3C File Offset: 0x0001AE3C
		private DomainMode GetDomainMode()
		{
			DirectoryEntry directoryEntry = null;
			DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
			int num = 0;
			DomainMode domainMode;
			try
			{
				if (directoryEntry2.Properties.Contains(PropertyManager.DomainFunctionality))
				{
					num = int.Parse((string)PropertyManager.GetPropertyValue(this.context, directoryEntry2, PropertyManager.DomainFunctionality), NumberFormatInfo.InvariantInfo);
				}
				switch (num)
				{
				case 0:
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext));
					if ((int)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.NTMixedDomain) == 0)
					{
						domainMode = DomainMode.Windows2000NativeDomain;
					}
					else
					{
						domainMode = DomainMode.Windows2000MixedDomain;
					}
					break;
				case 1:
					domainMode = DomainMode.Windows2003InterimDomain;
					break;
				case 2:
					domainMode = DomainMode.Windows2003Domain;
					break;
				case 3:
					domainMode = DomainMode.Windows2008Domain;
					break;
				case 4:
					domainMode = DomainMode.Windows2008R2Domain;
					break;
				default:
					throw new ActiveDirectoryOperationException(Res.GetString("InvalidMode"));
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				directoryEntry2.Dispose();
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			return domainMode;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001BF4C File Offset: 0x0001AF4C
		private DomainController GetRoleOwner(ActiveDirectoryRole role)
		{
			DirectoryEntry directoryEntry = null;
			string text = null;
			try
			{
				switch (role)
				{
				case ActiveDirectoryRole.PdcRole:
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext));
					break;
				case ActiveDirectoryRole.RidRole:
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.RidManager));
					break;
				case ActiveDirectoryRole.InfrastructureRole:
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.Infrastructure));
					break;
				}
				text = Utils.GetDnsHostNameFromNTDSA(this.context, (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.FsmoRoleOwner));
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, this.context);
			return new DomainController(newDirectoryContext, text);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C034 File Offset: 0x0001B034
		private void LoadCrossRefAttributes()
		{
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.PartitionsContainer));
				StringBuilder stringBuilder = new StringBuilder(15);
				stringBuilder.Append("(&(");
				stringBuilder.Append(PropertyManager.ObjectCategory);
				stringBuilder.Append("=crossRef)(");
				stringBuilder.Append(PropertyManager.SystemFlags);
				stringBuilder.Append(":1.2.840.113556.1.4.804:=");
				stringBuilder.Append(1);
				stringBuilder.Append(")(");
				stringBuilder.Append(PropertyManager.SystemFlags);
				stringBuilder.Append(":1.2.840.113556.1.4.804:=");
				stringBuilder.Append(2);
				stringBuilder.Append(")(");
				stringBuilder.Append(PropertyManager.DnsRoot);
				stringBuilder.Append("=");
				stringBuilder.Append(Utils.GetEscapedFilterValue(this.partitionName));
				stringBuilder.Append("))");
				string text = stringBuilder.ToString();
				ADSearcher adsearcher = new ADSearcher(directoryEntry, text, new string[]
				{
					PropertyManager.DistinguishedName,
					PropertyManager.TrustParent
				}, SearchScope.OneLevel, false, false);
				SearchResult searchResult = adsearcher.FindOne();
				this.crossRefDN = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName);
				if (searchResult.Properties[PropertyManager.TrustParent].Count > 0)
				{
					this.trustParent = (string)searchResult.Properties[PropertyManager.TrustParent][0];
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C1F0 File Offset: 0x0001B1F0
		private Domain GetParent()
		{
			if (this.crossRefDN == null)
			{
				this.LoadCrossRefAttributes();
			}
			if (this.trustParent != null)
			{
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.trustParent);
				string text = null;
				DirectoryContext directoryContext = null;
				try
				{
					text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.DnsRoot);
					directoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.Domain, this.context);
				}
				finally
				{
					directoryEntry.Dispose();
				}
				return new Domain(directoryContext, text);
			}
			return null;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001C270 File Offset: 0x0001B270
		private ArrayList GetChildDomains()
		{
			ArrayList arrayList = new ArrayList();
			if (this.crossRefDN == null)
			{
				this.LoadCrossRefAttributes();
			}
			DirectoryEntry directoryEntry = null;
			SearchResultCollection searchResultCollection = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.PartitionsContainer));
				StringBuilder stringBuilder = new StringBuilder(15);
				stringBuilder.Append("(&(");
				stringBuilder.Append(PropertyManager.ObjectCategory);
				stringBuilder.Append("=crossRef)(");
				stringBuilder.Append(PropertyManager.SystemFlags);
				stringBuilder.Append(":1.2.840.113556.1.4.804:=");
				stringBuilder.Append(1);
				stringBuilder.Append(")(");
				stringBuilder.Append(PropertyManager.SystemFlags);
				stringBuilder.Append(":1.2.840.113556.1.4.804:=");
				stringBuilder.Append(2);
				stringBuilder.Append(")(");
				stringBuilder.Append(PropertyManager.TrustParent);
				stringBuilder.Append("=");
				stringBuilder.Append(Utils.GetEscapedFilterValue(this.crossRefDN));
				stringBuilder.Append("))");
				string text = stringBuilder.ToString();
				ADSearcher adsearcher = new ADSearcher(directoryEntry, text, new string[] { PropertyManager.DnsRoot }, SearchScope.OneLevel);
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text2 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsRoot);
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text2, DirectoryContextType.Domain, this.context);
					arrayList.Add(new Domain(newDirectoryContext, text2));
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (searchResultCollection != null)
				{
					searchResultCollection.Dispose();
				}
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			return arrayList;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001C474 File Offset: 0x0001B474
		private ArrayList GetTrustsHelper(string targetDomainName)
		{
			string text = null;
			IntPtr intPtr = (IntPtr)0;
			int num = 0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			new TrustRelationshipInformationCollection();
			int num2 = 0;
			string text2 = null;
			int num3 = 0;
			bool flag = false;
			if (this.context.isServer())
			{
				text = this.context.Name;
			}
			else
			{
				text = DomainController.FindOne(this.context).Name;
			}
			flag = Utils.Impersonate(this.context);
			try
			{
				try
				{
					num3 = UnsafeNativeMethods.DsEnumerateDomainTrustsW(text, 35, out intPtr, out num);
				}
				finally
				{
					if (flag)
					{
						Utils.Revert();
					}
				}
			}
			catch
			{
				throw;
			}
			if (num3 != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num3, text);
			}
			ArrayList arrayList3;
			try
			{
				if (intPtr != (IntPtr)0 && num != 0)
				{
					IntPtr intPtr2 = (IntPtr)0;
					int num4 = 0;
					for (int i = 0; i < num; i++)
					{
						intPtr2 = Utils.AddToIntPtr(intPtr, i * Marshal.SizeOf(typeof(DS_DOMAIN_TRUSTS)));
						DS_DOMAIN_TRUSTS ds_DOMAIN_TRUSTS = new DS_DOMAIN_TRUSTS();
						Marshal.PtrToStructure(intPtr2, ds_DOMAIN_TRUSTS);
						arrayList.Add(ds_DOMAIN_TRUSTS);
					}
					for (int j = 0; j < arrayList.Count; j++)
					{
						DS_DOMAIN_TRUSTS ds_DOMAIN_TRUSTS2 = (DS_DOMAIN_TRUSTS)arrayList[j];
						if ((ds_DOMAIN_TRUSTS2.Flags & 42) != 0 && ds_DOMAIN_TRUSTS2.TrustType != TrustHelper.TRUST_TYPE_DOWNLEVEL)
						{
							TrustObject trustObject = new TrustObject();
							trustObject.TrustType = TrustType.Unknown;
							if (ds_DOMAIN_TRUSTS2.DnsDomainName != (IntPtr)0)
							{
								trustObject.DnsDomainName = Marshal.PtrToStringUni(ds_DOMAIN_TRUSTS2.DnsDomainName);
							}
							if (ds_DOMAIN_TRUSTS2.NetbiosDomainName != (IntPtr)0)
							{
								trustObject.NetbiosDomainName = Marshal.PtrToStringUni(ds_DOMAIN_TRUSTS2.NetbiosDomainName);
							}
							trustObject.Flags = ds_DOMAIN_TRUSTS2.Flags;
							trustObject.TrustAttributes = ds_DOMAIN_TRUSTS2.TrustAttributes;
							trustObject.OriginalIndex = j;
							trustObject.ParentIndex = ds_DOMAIN_TRUSTS2.ParentIndex;
							if (targetDomainName != null)
							{
								bool flag2 = false;
								if (trustObject.DnsDomainName != null && Utils.Compare(targetDomainName, trustObject.DnsDomainName) == 0)
								{
									flag2 = true;
								}
								else if (trustObject.NetbiosDomainName != null && Utils.Compare(targetDomainName, trustObject.NetbiosDomainName) == 0)
								{
									flag2 = true;
								}
								if (!flag2 && (trustObject.Flags & 8) == 0)
								{
									goto IL_0284;
								}
							}
							if ((trustObject.Flags & 8) != 0)
							{
								num2 = num4;
								if ((trustObject.Flags & 4) == 0)
								{
									DS_DOMAIN_TRUSTS ds_DOMAIN_TRUSTS3 = (DS_DOMAIN_TRUSTS)arrayList[trustObject.ParentIndex];
									if (ds_DOMAIN_TRUSTS3.DnsDomainName != (IntPtr)0)
									{
										text2 = Marshal.PtrToStringUni(ds_DOMAIN_TRUSTS3.DnsDomainName);
									}
								}
								trustObject.TrustType = (TrustType)7;
							}
							else if (ds_DOMAIN_TRUSTS2.TrustType == 3)
							{
								trustObject.TrustType = TrustType.Kerberos;
							}
							num4++;
							arrayList2.Add(trustObject);
						}
						IL_0284:;
					}
					for (int k = 0; k < arrayList2.Count; k++)
					{
						TrustObject trustObject2 = (TrustObject)arrayList2[k];
						if (k != num2 && trustObject2.TrustType != TrustType.Kerberos)
						{
							if (text2 != null && Utils.Compare(text2, trustObject2.DnsDomainName) == 0)
							{
								trustObject2.TrustType = TrustType.ParentChild;
							}
							else if ((trustObject2.Flags & 1) != 0)
							{
								if (trustObject2.ParentIndex == ((TrustObject)arrayList2[num2]).OriginalIndex)
								{
									trustObject2.TrustType = TrustType.ParentChild;
								}
								else if ((trustObject2.Flags & 4) != 0 && (((TrustObject)arrayList2[num2]).Flags & 4) != 0)
								{
									string text3 = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.RootDomainNamingContext);
									string dnsNameFromDN = Utils.GetDnsNameFromDN(text3);
									DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(this.context.Name, DirectoryContextType.Forest, this.context);
									if (newDirectoryContext.isRootDomain() || Utils.Compare(trustObject2.DnsDomainName, dnsNameFromDN) == 0)
									{
										trustObject2.TrustType = TrustType.TreeRoot;
									}
									else
									{
										trustObject2.TrustType = TrustType.CrossLink;
									}
								}
								else
								{
									trustObject2.TrustType = TrustType.CrossLink;
								}
							}
							else if ((trustObject2.TrustAttributes & 8) != 0)
							{
								trustObject2.TrustType = TrustType.Forest;
							}
							else
							{
								trustObject2.TrustType = TrustType.External;
							}
						}
					}
				}
				arrayList3 = arrayList2;
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					UnsafeNativeMethods.NetApiBufferFree(intPtr);
				}
			}
			return arrayList3;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C8C8 File Offset: 0x0001B8C8
		private void RepairTrustHelper(Domain targetDomain, TrustDirection direction)
		{
			string text = TrustHelper.CreateTrustPassword();
			string text2 = TrustHelper.UpdateTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, text, false);
			string text3 = TrustHelper.UpdateTrust(this.context, base.Name, targetDomain.Name, text, false);
			if ((direction & TrustDirection.Outbound) != (TrustDirection)0)
			{
				try
				{
					TrustHelper.VerifyTrust(this.context, base.Name, targetDomain.Name, false, TrustDirection.Outbound, true, text2);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[] { base.Name, targetDomain.Name, direction }), typeof(TrustRelationshipInformation), null);
				}
			}
			if ((direction & TrustDirection.Inbound) != (TrustDirection)0)
			{
				try
				{
					TrustHelper.VerifyTrust(targetDomain.GetDirectoryContext(), targetDomain.Name, base.Name, false, TrustDirection.Outbound, true, text3);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("WrongTrustDirection", new object[] { base.Name, targetDomain.Name, direction }), typeof(TrustRelationshipInformation), null);
				}
			}
		}

		// Token: 0x04000408 RID: 1032
		private string crossRefDN;

		// Token: 0x04000409 RID: 1033
		private string trustParent;

		// Token: 0x0400040A RID: 1034
		private DomainControllerCollection cachedDomainControllers;

		// Token: 0x0400040B RID: 1035
		private DomainCollection cachedChildren;

		// Token: 0x0400040C RID: 1036
		private DomainMode currentDomainMode = (DomainMode)(-1);

		// Token: 0x0400040D RID: 1037
		private DomainController cachedPdcRoleOwner;

		// Token: 0x0400040E RID: 1038
		private DomainController cachedRidRoleOwner;

		// Token: 0x0400040F RID: 1039
		private DomainController cachedInfrastructureRoleOwner;

		// Token: 0x04000410 RID: 1040
		private Domain cachedParent;

		// Token: 0x04000411 RID: 1041
		private Forest cachedForest;

		// Token: 0x04000412 RID: 1042
		private bool isParentInitialized;
	}
}

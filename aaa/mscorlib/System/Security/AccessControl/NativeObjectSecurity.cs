using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FF RID: 2303
	public abstract class NativeObjectSecurity : CommonObjectSecurity
	{
		// Token: 0x060053E9 RID: 21481 RVA: 0x00131768 File Offset: 0x00130768
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType)
			: base(isContainer)
		{
			if (!Win32.IsLsaPolicySupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			this._resourceType = resourceType;
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x001317C6 File Offset: 0x001307C6
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
			: this(isContainer, resourceType)
		{
			this._exceptionContext = exceptionContext;
			this._exceptionFromErrorCode = exceptionFromErrorCode;
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x001317DF File Offset: 0x001307DF
		internal NativeObjectSecurity(ResourceType resourceType, CommonSecurityDescriptor securityDescriptor)
			: this(resourceType, securityDescriptor, null)
		{
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x001317EC File Offset: 0x001307EC
		internal NativeObjectSecurity(ResourceType resourceType, CommonSecurityDescriptor securityDescriptor, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode)
			: base(securityDescriptor)
		{
			if (!Win32.IsLsaPolicySupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			this._resourceType = resourceType;
			this._exceptionFromErrorCode = exceptionFromErrorCode;
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x00131854 File Offset: 0x00130854
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
			: this(resourceType, NativeObjectSecurity.CreateInternal(resourceType, isContainer, name, null, includeSections, true, exceptionFromErrorCode, exceptionContext), exceptionFromErrorCode)
		{
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x0013187A File Offset: 0x0013087A
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections)
			: this(isContainer, resourceType, name, includeSections, null, null)
		{
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x0013188C File Offset: 0x0013088C
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle handle, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
			: this(resourceType, NativeObjectSecurity.CreateInternal(resourceType, isContainer, null, handle, includeSections, false, exceptionFromErrorCode, exceptionContext), exceptionFromErrorCode)
		{
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x001318B2 File Offset: 0x001308B2
		protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle handle, AccessControlSections includeSections)
			: this(isContainer, resourceType, handle, includeSections, null, null)
		{
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x001318C4 File Offset: 0x001308C4
		private static CommonSecurityDescriptor CreateInternal(ResourceType resourceType, bool isContainer, string name, SafeHandle handle, AccessControlSections includeSections, bool createByName, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
		{
			if (createByName && name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!createByName && handle == null)
			{
				throw new ArgumentNullException("handle");
			}
			RawSecurityDescriptor rawSecurityDescriptor;
			int securityInfo = Win32.GetSecurityInfo(resourceType, name, handle, includeSections, out rawSecurityDescriptor);
			if (securityInfo != 0)
			{
				Exception ex = null;
				if (exceptionFromErrorCode != null)
				{
					ex = exceptionFromErrorCode(securityInfo, name, handle, exceptionContext);
				}
				if (ex == null)
				{
					if (securityInfo == 5)
					{
						ex = new UnauthorizedAccessException();
					}
					else if (securityInfo == 1307)
					{
						ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_InvalidOwner"));
					}
					else if (securityInfo == 1308)
					{
						ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_InvalidGroup"));
					}
					else if (securityInfo == 87)
					{
						ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_UnexpectedError", new object[] { securityInfo }));
					}
					else if (securityInfo == 123)
					{
						ex = new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
					}
					else if (securityInfo == 2)
					{
						ex = ((name == null) ? new FileNotFoundException() : new FileNotFoundException(name));
					}
					else if (securityInfo == 1350)
					{
						ex = new NotSupportedException(Environment.GetResourceString("AccessControl_NoAssociatedSecurity"));
					}
					else
					{
						ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_UnexpectedError", new object[] { securityInfo }));
					}
				}
				throw ex;
			}
			return new CommonSecurityDescriptor(isContainer, false, rawSecurityDescriptor, true);
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x00131A10 File Offset: 0x00130A10
		private void Persist(string name, SafeHandle handle, AccessControlSections includeSections, object exceptionContext)
		{
			base.WriteLock();
			try
			{
				SecurityInfos securityInfos = (SecurityInfos)0;
				SecurityIdentifier securityIdentifier = null;
				SecurityIdentifier securityIdentifier2 = null;
				SystemAcl systemAcl = null;
				DiscretionaryAcl discretionaryAcl = null;
				if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None && this._securityDescriptor.Owner != null)
				{
					securityInfos |= SecurityInfos.Owner;
					securityIdentifier = this._securityDescriptor.Owner;
				}
				if ((includeSections & AccessControlSections.Group) != AccessControlSections.None && this._securityDescriptor.Group != null)
				{
					securityInfos |= SecurityInfos.Group;
					securityIdentifier2 = this._securityDescriptor.Group;
				}
				if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
				{
					securityInfos |= SecurityInfos.SystemAcl;
					if (this._securityDescriptor.IsSystemAclPresent && this._securityDescriptor.SystemAcl != null && this._securityDescriptor.SystemAcl.Count > 0)
					{
						systemAcl = this._securityDescriptor.SystemAcl;
					}
					else
					{
						systemAcl = null;
					}
					if ((this._securityDescriptor.ControlFlags & ControlFlags.SystemAclProtected) != ControlFlags.None)
					{
						securityInfos |= (SecurityInfos)this.ProtectedSystemAcl;
					}
					else
					{
						securityInfos |= (SecurityInfos)this.UnprotectedSystemAcl;
					}
				}
				if ((includeSections & AccessControlSections.Access) != AccessControlSections.None && this._securityDescriptor.IsDiscretionaryAclPresent)
				{
					securityInfos |= SecurityInfos.DiscretionaryAcl;
					if (this._securityDescriptor.DiscretionaryAcl.EveryOneFullAccessForNullDacl)
					{
						discretionaryAcl = null;
					}
					else
					{
						discretionaryAcl = this._securityDescriptor.DiscretionaryAcl;
					}
					if ((this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != ControlFlags.None)
					{
						securityInfos |= (SecurityInfos)this.ProtectedDiscretionaryAcl;
					}
					else
					{
						securityInfos |= (SecurityInfos)this.UnprotectedDiscretionaryAcl;
					}
				}
				if (securityInfos != (SecurityInfos)0)
				{
					int num = Win32.SetSecurityInfo(this._resourceType, name, handle, securityInfos, securityIdentifier, securityIdentifier2, systemAcl, discretionaryAcl);
					if (num != 0)
					{
						Exception ex = null;
						if (this._exceptionFromErrorCode != null)
						{
							ex = this._exceptionFromErrorCode(num, name, handle, exceptionContext);
						}
						if (ex == null)
						{
							if (num == 5)
							{
								ex = new UnauthorizedAccessException();
							}
							else if (num == 1307)
							{
								ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_InvalidOwner"));
							}
							else if (num == 1308)
							{
								ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_InvalidGroup"));
							}
							else if (num == 123)
							{
								ex = new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
							}
							else if (num == 6)
							{
								ex = new NotSupportedException(Environment.GetResourceString("AccessControl_InvalidHandle"));
							}
							else if (num == 2)
							{
								ex = new FileNotFoundException();
							}
							else if (num == 1350)
							{
								ex = new NotSupportedException(Environment.GetResourceString("AccessControl_NoAssociatedSecurity"));
							}
							else
							{
								ex = new InvalidOperationException(Environment.GetResourceString("AccessControl_UnexpectedError", new object[] { num }));
							}
						}
						throw ex;
					}
					base.OwnerModified = false;
					base.GroupModified = false;
					base.AccessRulesModified = false;
					base.AuditRulesModified = false;
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00131CA8 File Offset: 0x00130CA8
		protected sealed override void Persist(string name, AccessControlSections includeSections)
		{
			this.Persist(name, includeSections, this._exceptionContext);
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x00131CB8 File Offset: 0x00130CB8
		protected void Persist(string name, AccessControlSections includeSections, object exceptionContext)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Persist(name, null, includeSections, exceptionContext);
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00131CD2 File Offset: 0x00130CD2
		protected sealed override void Persist(SafeHandle handle, AccessControlSections includeSections)
		{
			this.Persist(handle, includeSections, this._exceptionContext);
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x00131CE2 File Offset: 0x00130CE2
		protected void Persist(SafeHandle handle, AccessControlSections includeSections, object exceptionContext)
		{
			if (handle == null)
			{
				throw new ArgumentNullException("handle");
			}
			this.Persist(null, handle, includeSections, exceptionContext);
		}

		// Token: 0x04002B42 RID: 11074
		private readonly ResourceType _resourceType;

		// Token: 0x04002B43 RID: 11075
		private NativeObjectSecurity.ExceptionFromErrorCode _exceptionFromErrorCode;

		// Token: 0x04002B44 RID: 11076
		private object _exceptionContext;

		// Token: 0x04002B45 RID: 11077
		private readonly uint ProtectedDiscretionaryAcl = 2147483648U;

		// Token: 0x04002B46 RID: 11078
		private readonly uint ProtectedSystemAcl = 1073741824U;

		// Token: 0x04002B47 RID: 11079
		private readonly uint UnprotectedDiscretionaryAcl = 536870912U;

		// Token: 0x04002B48 RID: 11080
		private readonly uint UnprotectedSystemAcl = 268435456U;

		// Token: 0x02000900 RID: 2304
		// (Invoke) Token: 0x060053F8 RID: 21496
		protected internal delegate Exception ExceptionFromErrorCode(int errorCode, string name, SafeHandle handle, object context);
	}
}

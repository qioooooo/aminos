using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AF RID: 175
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class GlobalCatalog : DomainController
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x000217BC File Offset: 0x000207BC
		internal GlobalCatalog(DirectoryContext context, string globalCatalogName)
			: base(context, globalCatalogName)
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000217C6 File Offset: 0x000207C6
		internal GlobalCatalog(DirectoryContext context, string globalCatalogName, DirectoryEntryManager directoryEntryMgr)
			: base(context, globalCatalogName, directoryEntryMgr)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000217D4 File Offset: 0x000207D4
		public static GlobalCatalog GetGlobalCatalog(DirectoryContext context)
		{
			string text = null;
			DirectoryEntryManager directoryEntryManager = null;
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeGC"), "context");
			}
			if (!context.isServer())
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("GCNotFound", new object[] { context.Name }), typeof(GlobalCatalog), context.Name);
			}
			context = new DirectoryContext(context);
			try
			{
				directoryEntryManager = new DirectoryEntryManager(context);
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				if (!Utils.CheckCapability(directoryEntry, Capability.ActiveDirectory))
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("GCNotFound", new object[] { context.Name }), typeof(GlobalCatalog), context.Name);
				}
				text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.DnsHostName);
				if (!bool.Parse((string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.IsGlobalCatalogReady)))
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("GCNotFound", new object[] { context.Name }), typeof(GlobalCatalog), context.Name);
				}
			}
			catch (COMException ex)
			{
				int errorCode = ex.ErrorCode;
				if (errorCode == -2147016646)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("GCNotFound", new object[] { context.Name }), typeof(GlobalCatalog), context.Name);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			return new GlobalCatalog(context, text, directoryEntryManager);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00021970 File Offset: 0x00020970
		public new static GlobalCatalog FindOne(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			return GlobalCatalog.FindOneWithCredentialValidation(context, null, (LocatorOptions)0L);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000219A8 File Offset: 0x000209A8
		public new static GlobalCatalog FindOne(DirectoryContext context, string siteName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return GlobalCatalog.FindOneWithCredentialValidation(context, siteName, (LocatorOptions)0L);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000219F8 File Offset: 0x000209F8
		public new static GlobalCatalog FindOne(DirectoryContext context, LocatorOptions flag)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			return GlobalCatalog.FindOneWithCredentialValidation(context, null, flag);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021A30 File Offset: 0x00020A30
		public new static GlobalCatalog FindOne(DirectoryContext context, string siteName, LocatorOptions flag)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return GlobalCatalog.FindOneWithCredentialValidation(context, siteName, flag);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00021A7F File Offset: 0x00020A7F
		public new static GlobalCatalogCollection FindAll(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			context = new DirectoryContext(context);
			return GlobalCatalog.FindAllInternal(context, null);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021ABC File Offset: 0x00020ABC
		public new static GlobalCatalogCollection FindAll(DirectoryContext context, string siteName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeForest"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			context = new DirectoryContext(context);
			return GlobalCatalog.FindAllInternal(context, siteName);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00021B12 File Offset: 0x00020B12
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override GlobalCatalog EnableGlobalCatalog()
		{
			base.CheckIfDisposed();
			throw new InvalidOperationException(Res.GetString("CannotPerformOnGCObject"));
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021B2C File Offset: 0x00020B2C
		public DomainController DisableGlobalCatalog()
		{
			base.CheckIfDisposed();
			this.CheckIfDisabled();
			DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(base.NtdsaObjectName);
			int num = 0;
			try
			{
				if (cachedDirectoryEntry.Properties[PropertyManager.Options].Value != null)
				{
					num = (int)cachedDirectoryEntry.Properties[PropertyManager.Options].Value;
				}
				cachedDirectoryEntry.Properties[PropertyManager.Options].Value = num & -2;
				cachedDirectoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			this.disabled = true;
			return new DomainController(this.context, base.Name);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00021BE8 File Offset: 0x00020BE8
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override bool IsGlobalCatalog()
		{
			base.CheckIfDisposed();
			this.CheckIfDisabled();
			return true;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021BF8 File Offset: 0x00020BF8
		public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties()
		{
			base.CheckIfDisposed();
			this.CheckIfDisabled();
			if (this.schema == null)
			{
				string text = null;
				try
				{
					text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SchemaNamingContext);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				Utils.GetNewDirectoryContext(base.Name, DirectoryContextType.DirectoryServer, this.context);
				this.schema = new ActiveDirectorySchema(this.context, text);
			}
			return this.schema.FindAllProperties(PropertyTypes.InGlobalCatalog);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00021C7C File Offset: 0x00020C7C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override DirectorySearcher GetDirectorySearcher()
		{
			base.CheckIfDisposed();
			this.CheckIfDisabled();
			return this.InternalGetDirectorySearcher();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021C90 File Offset: 0x00020C90
		private void CheckIfDisabled()
		{
			if (this.disabled)
			{
				throw new InvalidOperationException(Res.GetString("GCDisabled"));
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021CAC File Offset: 0x00020CAC
		internal new static GlobalCatalog FindOneWithCredentialValidation(DirectoryContext context, string siteName, LocatorOptions flag)
		{
			/*
An exception occurred when decompiling this method (060005D8)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.DirectoryServices.ActiveDirectory.GlobalCatalog System.DirectoryServices.ActiveDirectory.GlobalCatalog::FindOneWithCredentialValidation(System.DirectoryServices.ActiveDirectory.DirectoryContext,System.String,System.DirectoryServices.ActiveDirectory.LocatorOptions)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1045
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 888
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00021DEC File Offset: 0x00020DEC
		internal new static GlobalCatalog FindOneInternal(DirectoryContext context, string forestName, string siteName, LocatorOptions flag)
		{
			int num = 0;
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			if ((flag & ~(LocatorOptions.ForceRediscovery | LocatorOptions.KdcRequired | LocatorOptions.TimeServerRequired | LocatorOptions.WriteableRequired | LocatorOptions.AvoidSelf)) != (LocatorOptions)0L)
			{
				throw new ArgumentException(Res.GetString("InvalidFlags"), "flag");
			}
			if (forestName == null)
			{
				DomainControllerInfo domainControllerInfo;
				int num2 = Locator.DsGetDcNameWrapper(null, DirectoryContext.GetLoggedOnDomain(), null, 16L, out domainControllerInfo);
				if (num2 == 1355)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ContextNotAssociatedWithDomain"), typeof(GlobalCatalog), null);
				}
				if (num2 != 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(num);
				}
				forestName = domainControllerInfo.DnsForestName;
			}
			DomainControllerInfo domainControllerInfo2;
			num = Locator.DsGetDcNameWrapper(null, forestName, siteName, (long)(flag | (LocatorOptions)80L), out domainControllerInfo2);
			if (num == 1355)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("GCNotFoundInForest", new object[] { forestName }), typeof(GlobalCatalog), null);
			}
			if (num == 1004)
			{
				throw new ArgumentException(Res.GetString("InvalidFlags"), "flag");
			}
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num);
			}
			string text = domainControllerInfo2.DomainControllerName.Substring(2);
			DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
			return new GlobalCatalog(newDirectoryContext, text);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00021F14 File Offset: 0x00020F14
		internal static GlobalCatalogCollection FindAllInternal(DirectoryContext context, string siteName)
		{
			ArrayList arrayList = new ArrayList();
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			foreach (object obj in Utils.GetReplicaList(context, null, siteName, false, false, true))
			{
				string text = (string)obj;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
				arrayList.Add(new GlobalCatalog(newDirectoryContext, text));
			}
			return new GlobalCatalogCollection(arrayList);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021FB4 File Offset: 0x00020FB4
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		private DirectorySearcher InternalGetDirectorySearcher()
		{
			DirectoryEntry directoryEntry = new DirectoryEntry("GC://" + base.Name);
			if (DirectoryContext.ServerBindSupported)
			{
				directoryEntry.AuthenticationType = Utils.DefaultAuthType | AuthenticationTypes.ServerBind;
			}
			else
			{
				directoryEntry.AuthenticationType = Utils.DefaultAuthType;
			}
			directoryEntry.Username = this.context.UserName;
			directoryEntry.Password = this.context.Password;
			return new DirectorySearcher(directoryEntry);
		}

		// Token: 0x0400047B RID: 1147
		private ActiveDirectorySchema schema;

		// Token: 0x0400047C RID: 1148
		private bool disabled;
	}
}

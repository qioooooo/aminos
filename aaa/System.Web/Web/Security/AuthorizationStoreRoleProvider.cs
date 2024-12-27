using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000329 RID: 809
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AuthorizationStoreRoleProvider : RoleProvider
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x000AD8CB File Offset: 0x000AC8CB
		// (set) Token: 0x060027B8 RID: 10168 RVA: 0x000AD8D3 File Offset: 0x000AC8D3
		public override string ApplicationName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				if (this._AppName != value)
				{
					if (value.Length > 256)
					{
						throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
					}
					this._AppName = value;
					this._InitAppDone = false;
				}
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x000AD90E File Offset: 0x000AC90E
		// (set) Token: 0x060027BA RID: 10170 RVA: 0x000AD916 File Offset: 0x000AC916
		public string ScopeName
		{
			get
			{
				return this._ScopeName;
			}
			set
			{
				if (this._ScopeName != value)
				{
					this._ScopeName = value;
					this._InitAppDone = false;
				}
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x000AD934 File Offset: 0x000AC934
		public int CacheRefreshInterval
		{
			get
			{
				return this._CacheRefreshInterval;
			}
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000AD93C File Offset: 0x000AC93C
		public override void Initialize(string name, NameValueCollection config)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (string.IsNullOrEmpty(name))
			{
				name = "AuthorizationStoreRoleProvider";
			}
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("RoleAuthStoreProvider_description"));
			}
			base.Initialize(name, config);
			this._CacheRefreshInterval = SecUtility.GetIntValue(config, "cacheRefreshInterval", 60, false, 0);
			this._ScopeName = config["scopeName"];
			if (this._ScopeName != null && this._ScopeName.Length == 0)
			{
				this._ScopeName = null;
			}
			this._ConnectionString = config["connectionStringName"];
			if (this._ConnectionString == null || this._ConnectionString.Length < 1)
			{
				throw new ProviderException(SR.GetString("Connection_name_not_specified"));
			}
			ConnectionStringsSection connectionStrings = RuntimeConfig.GetAppConfig().ConnectionStrings;
			ConnectionStringSettings connectionStringSettings = connectionStrings.ConnectionStrings[this._ConnectionString];
			if (connectionStringSettings == null)
			{
				throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { this._ConnectionString }));
			}
			if (string.IsNullOrEmpty(connectionStringSettings.ConnectionString))
			{
				throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { this._ConnectionString }));
			}
			this._ConnectionString = connectionStringSettings.ConnectionString;
			this._AppName = config["applicationName"];
			if (string.IsNullOrEmpty(this._AppName))
			{
				this._AppName = SecUtility.GetDefaultAppName();
			}
			if (this._AppName.Length > 256)
			{
				throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
			}
			config.Remove("connectionStringName");
			config.Remove("cacheRefreshInterval");
			config.Remove("applicationName");
			config.Remove("scopeName");
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ProviderException(SR.GetString("Provider_unrecognized_attribute", new object[] { key }));
				}
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000ADB58 File Offset: 0x000ACB58
		public override bool IsUserInRole(string username, string roleName)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 0, "username");
			if (username.Length < 1)
			{
				return false;
			}
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			return this.IsUserInRoleCore(username, roleName);
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000ADB9C File Offset: 0x000ACB9C
		public override string[] GetRolesForUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 0, "username");
			if (username.Length < 1)
			{
				return new string[0];
			}
			return this.GetRolesForUserCore(username);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000ADBD4 File Offset: 0x000ACBD4
		public override void CreateRole(string roleName)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "API_not_supported_at_this_level");
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			this.InitApp();
			object[] array = new object[] { roleName, null };
			object obj = this.CallMethod((this._ObjAzScope != null) ? this._ObjAzScope : this._ObjAzApplication, "CreateRole", array);
			array[0] = 0;
			array[1] = null;
			try
			{
				try
				{
					this.CallMethod(obj, "Submit", array);
				}
				finally
				{
					Marshal.FinalReleaseComObject(obj);
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000ADC80 File Offset: 0x000ACC80
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "API_not_supported_at_this_level");
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			this.InitApp();
			if (throwOnPopulatedRole)
			{
				string[] usersInRole;
				try
				{
					usersInRole = this.GetUsersInRole(roleName);
				}
				catch
				{
					return false;
				}
				if (usersInRole.Length != 0)
				{
					throw new ProviderException(SR.GetString("Role_is_not_empty"));
				}
			}
			IL_004C:
			object[] array = new object[] { roleName, null };
			this.CallMethod((this._ObjAzScope != null) ? this._ObjAzScope : this._ObjAzApplication, "DeleteRole", array);
			array[0] = 0;
			array[1] = null;
			this.CallMethod((this._ObjAzScope != null) ? this._ObjAzScope : this._ObjAzApplication, "Submit", array);
			return true;
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000ADD50 File Offset: 0x000ACD50
		public override bool RoleExists(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			bool flag = false;
			object obj = null;
			try
			{
				obj = this.GetRole(roleName);
				flag = obj != null;
			}
			catch (TargetInvocationException ex)
			{
				COMException ex2 = ex.InnerException as COMException;
				if (ex2 != null && ex2.ErrorCode == -2147023728)
				{
					return false;
				}
				throw;
			}
			finally
			{
				if (obj != null)
				{
					Marshal.FinalReleaseComObject(obj);
				}
			}
			return flag;
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000ADDD4 File Offset: 0x000ACDD4
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "API_not_supported_at_this_level");
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			int num = 0;
			object[] array = new object[2];
			object[] array2 = new object[roleNames.Length];
			foreach (string text in roleNames)
			{
				array2[num++] = this.GetRole(text);
			}
			try
			{
				try
				{
					foreach (object obj in array2)
					{
						foreach (string text2 in usernames)
						{
							array[0] = text2;
							array[1] = null;
							this.CallMethod(obj, "AddMemberName", array);
						}
					}
					foreach (object obj2 in array2)
					{
						array[0] = 0;
						array[1] = null;
						this.CallMethod(obj2, "Submit", array);
					}
				}
				finally
				{
					foreach (object obj3 in array2)
					{
						Marshal.FinalReleaseComObject(obj3);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000ADF20 File Offset: 0x000ACF20
		public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "API_not_supported_at_this_level");
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			SecUtility.CheckArrayParameter(ref userNames, true, true, true, 0, "userNames");
			int num = 0;
			object[] array = new object[2];
			object[] array2 = new object[roleNames.Length];
			foreach (string text in roleNames)
			{
				array2[num++] = this.GetRole(text);
			}
			try
			{
				try
				{
					foreach (object obj in array2)
					{
						foreach (string text2 in userNames)
						{
							array[0] = text2;
							array[1] = null;
							this.CallMethod(obj, "DeleteMemberName", array);
						}
					}
					foreach (object obj2 in array2)
					{
						array[0] = 0;
						array[1] = null;
						this.CallMethod(obj2, "Submit", array);
					}
				}
				finally
				{
					foreach (object obj3 in array2)
					{
						Marshal.FinalReleaseComObject(obj3);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000AE06C File Offset: 0x000AD06C
		public override string[] GetUsersInRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			object role = this.GetRole(roleName);
			object obj;
			try
			{
				try
				{
					obj = this.CallProperty(role, "MembersName", null);
				}
				finally
				{
					Marshal.FinalReleaseComObject(role);
				}
			}
			catch
			{
				throw;
			}
			StringCollection stringCollection = new StringCollection();
			try
			{
				if (HostingEnvironment.IsHosted && this._XmlFileName != null)
				{
					InternalSecurityPermissions.Unrestricted.Assert();
				}
				try
				{
					IEnumerable enumerable = (IEnumerable)obj;
					foreach (object obj2 in enumerable)
					{
						stringCollection.Add((string)obj2);
					}
				}
				finally
				{
					if (HostingEnvironment.IsHosted && this._XmlFileName != null)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			catch
			{
				throw;
			}
			string[] array = new string[stringCollection.Count];
			stringCollection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000AE18C File Offset: 0x000AD18C
		public override string[] GetAllRoles()
		{
			this.InitApp();
			object obj = this.CallProperty((this._ObjAzScope != null) ? this._ObjAzScope : this._ObjAzApplication, "Roles", null);
			StringCollection stringCollection = new StringCollection();
			try
			{
				if (HostingEnvironment.IsHosted && this._XmlFileName != null)
				{
					InternalSecurityPermissions.Unrestricted.Assert();
				}
				try
				{
					IEnumerable enumerable = (IEnumerable)obj;
					foreach (object obj2 in enumerable)
					{
						string text = (string)this.CallProperty(obj2, "Name", null);
						stringCollection.Add(text);
					}
				}
				finally
				{
					if (HostingEnvironment.IsHosted && this._XmlFileName != null)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			catch
			{
				throw;
			}
			string[] array = new string[stringCollection.Count];
			stringCollection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000AE294 File Offset: 0x000AD294
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000AE29C File Offset: 0x000AD29C
		private object CallMethod(object objectToCallOn, string methodName, object[] args)
		{
			if (HostingEnvironment.IsHosted && this._XmlFileName != null)
			{
				InternalSecurityPermissions.Unrestricted.Assert();
			}
			object obj;
			try
			{
				using (new ApplicationImpersonationContext())
				{
					obj = objectToCallOn.GetType().InvokeMember(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, objectToCallOn, args, CultureInfo.InvariantCulture);
				}
			}
			catch
			{
				throw;
			}
			return obj;
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x000AE310 File Offset: 0x000AD310
		private object CallProperty(object objectToCallOn, string propName, object[] args)
		{
			if (HostingEnvironment.IsHosted && this._XmlFileName != null)
			{
				InternalSecurityPermissions.Unrestricted.Assert();
			}
			object obj;
			try
			{
				using (new ApplicationImpersonationContext())
				{
					obj = objectToCallOn.GetType().InvokeMember(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, objectToCallOn, args, CultureInfo.InvariantCulture);
				}
			}
			catch
			{
				throw;
			}
			return obj;
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000AE384 File Offset: 0x000AD384
		private void InitApp()
		{
			try
			{
				using (new ApplicationImpersonationContext())
				{
					if (this._InitAppDone)
					{
						if (DateTime.Now > this._LastUpdateCacheDate.AddMinutes((double)this.CacheRefreshInterval))
						{
							this._LastUpdateCacheDate = DateTime.Now;
							this.CallMethod(this._ObjAzAuthorizationStoreClass, "UpdateCache", null);
						}
					}
					else
					{
						lock (this)
						{
							if (!this._InitAppDone)
							{
								if (this._ConnectionString.ToLower(CultureInfo.InvariantCulture).StartsWith("msxml://", StringComparison.Ordinal))
								{
									if (this._ConnectionString.Contains("/~/"))
									{
										string text = null;
										if (HostingEnvironment.IsHosted)
										{
											text = HttpRuntime.AppDomainAppPath;
										}
										else
										{
											Process currentProcess = Process.GetCurrentProcess();
											ProcessModule processModule = ((currentProcess != null) ? currentProcess.MainModule : null);
											string text2 = ((processModule != null) ? processModule.FileName : null);
											if (text2 != null)
											{
												text = Path.GetDirectoryName(text2);
											}
											if (text == null || text.Length < 1)
											{
												text = Environment.CurrentDirectory;
											}
										}
										text = text.Replace('\\', '/');
										this._ConnectionString = this._ConnectionString.Replace("~", text);
									}
									string text3 = this._ConnectionString.Substring("msxml://".Length).Replace('/', '\\');
									if (HostingEnvironment.IsHosted)
									{
										HttpRuntime.CheckFilePermission(text3, false);
									}
									if (!FileUtil.FileExists(text3))
									{
										throw new FileNotFoundException(SR.GetString("AuthStore_policy_file_not_found", new object[] { HttpRuntime.GetSafePath(text3) }));
									}
									this._XmlFileName = text3;
								}
								Type type = null;
								try
								{
									this._NewAuthInterface = true;
									type = Type.GetType("Microsoft.Interop.Security.AzRoles.AzAuthorizationStoreClass, Microsoft.Interop.Security.AzRoles, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
									if (type == null)
									{
										type = Type.GetType("Microsoft.Interop.Security.AzRoles.AzAuthorizationStoreClass, Microsoft.Interop.Security.AzRoles, Version=1.2.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
									}
									if (type == null)
									{
										this._NewAuthInterface = false;
										type = Type.GetType("Microsoft.Interop.Security.AzRoles.AzAuthorizationStoreClass, Microsoft.Interop.Security.AzRoles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", true);
									}
								}
								catch (FileNotFoundException ex)
								{
									HttpContext httpContext = HttpContext.Current;
									if (httpContext == null)
									{
										throw new ProviderException(SR.GetString("AuthStoreNotInstalled_Title"), ex);
									}
									httpContext.Response.Clear();
									httpContext.Response.StatusCode = 500;
									httpContext.Response.Write(AuthStoreErrorFormatter.GetErrorText());
									httpContext.Response.End();
								}
								if (HostingEnvironment.IsHosted && this._XmlFileName != null)
								{
									InternalSecurityPermissions.Unrestricted.Assert();
								}
								this._ObjAzAuthorizationStoreClass = Activator.CreateInstance(type);
								object[] array = new object[3];
								array[0] = 0;
								array[1] = this._ConnectionString;
								object[] array2 = array;
								this.CallMethod(this._ObjAzAuthorizationStoreClass, "Initialize", array2);
								array2 = new object[] { this._AppName, null };
								if (this._NewAuthInterface)
								{
									this._ObjAzApplication = this.CallMethod(this._ObjAzAuthorizationStoreClass, "OpenApplication2", array2);
								}
								else
								{
									this._ObjAzApplication = this.CallMethod(this._ObjAzAuthorizationStoreClass, "OpenApplication", array2);
								}
								if (this._ObjAzApplication == null)
								{
									throw new ProviderException(SR.GetString("AuthStore_Application_not_found"));
								}
								this._ObjAzScope = null;
								if (!string.IsNullOrEmpty(this._ScopeName))
								{
									array2[0] = this._ScopeName;
									array2[1] = null;
									this._ObjAzScope = this.CallMethod(this._ObjAzApplication, "OpenScope", array2);
									if (this._ObjAzScope == null)
									{
										throw new ProviderException(SR.GetString("AuthStore_Scope_not_found"));
									}
								}
								this._LastUpdateCacheDate = DateTime.Now;
								this._InitAppDone = true;
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000AE748 File Offset: 0x000AD748
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private IntPtr GetWindowsTokenWithAssert(string userName)
		{
			if (HostingEnvironment.IsHosted)
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null && httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity is WindowsIdentity && StringUtil.EqualsIgnoreCase(userName, httpContext.User.Identity.Name))
				{
					return ((WindowsIdentity)httpContext.User.Identity).Token;
				}
			}
			IPrincipal currentPrincipal = Thread.CurrentPrincipal;
			if (currentPrincipal != null && currentPrincipal.Identity != null && currentPrincipal.Identity is WindowsIdentity && StringUtil.EqualsIgnoreCase(userName, currentPrincipal.Identity.Name))
			{
				return ((WindowsIdentity)currentPrincipal.Identity).Token;
			}
			return IntPtr.Zero;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000AE804 File Offset: 0x000AD804
		private object GetClientContext(string userName)
		{
			this.InitApp();
			IntPtr windowsTokenWithAssert = this.GetWindowsTokenWithAssert(userName);
			if (windowsTokenWithAssert != IntPtr.Zero)
			{
				return this.GetClientContextFromToken(windowsTokenWithAssert);
			}
			return this.GetClientContextFromName(userName);
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000AE83C File Offset: 0x000AD83C
		private object GetClientContextFromToken(IntPtr token)
		{
			if (this._NewAuthInterface)
			{
				object[] array = new object[]
				{
					(uint)(int)token,
					0,
					null
				};
				return this.CallMethod(this._ObjAzApplication, "InitializeClientContextFromToken2", array);
			}
			object[] array2 = new object[]
			{
				(ulong)(long)token,
				null
			};
			return this.CallMethod(this._ObjAzApplication, "InitializeClientContextFromToken", array2);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000AE8B4 File Offset: 0x000AD8B4
		private object GetClientContextFromName(string userName)
		{
			string[] array = userName.Split(new char[] { '\\' });
			string text = null;
			if (array.Length > 1)
			{
				text = array[0];
				userName = array[1];
			}
			object[] array2 = new object[] { userName, text, null };
			return this.CallMethod(this._ObjAzApplication, "InitializeClientContextFromName", array2);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000AE90C File Offset: 0x000AD90C
		private bool IsUserInRoleCore(string username, string roleName)
		{
			object clientContext = this.GetClientContext(username);
			if (clientContext == null)
			{
				return false;
			}
			object obj = this.CallMethod(clientContext, "GetRoles", new object[] { this._ScopeName });
			if (obj == null || !(obj is IEnumerable))
			{
				return false;
			}
			bool flag;
			try
			{
				if (HostingEnvironment.IsHosted && this._XmlFileName != null)
				{
					InternalSecurityPermissions.Unrestricted.Assert();
				}
				try
				{
					IEnumerable enumerable = (IEnumerable)obj;
					foreach (object obj2 in enumerable)
					{
						string text = (string)obj2;
						if (text != null && StringUtil.EqualsIgnoreCase(text, roleName))
						{
							return true;
						}
					}
					flag = false;
				}
				finally
				{
					if (HostingEnvironment.IsHosted && this._XmlFileName != null)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000AEA08 File Offset: 0x000ADA08
		private string[] GetRolesForUserCore(string username)
		{
			object clientContext = this.GetClientContext(username);
			if (clientContext == null)
			{
				return new string[0];
			}
			object obj = this.CallMethod(clientContext, "GetRoles", new object[] { this._ScopeName });
			if (obj == null || !(obj is IEnumerable))
			{
				return new string[0];
			}
			StringCollection stringCollection = new StringCollection();
			try
			{
				if (HostingEnvironment.IsHosted && this._XmlFileName != null)
				{
					InternalSecurityPermissions.Unrestricted.Assert();
				}
				try
				{
					IEnumerable enumerable = (IEnumerable)obj;
					foreach (object obj2 in enumerable)
					{
						string text = (string)obj2;
						if (text != null)
						{
							stringCollection.Add(text);
						}
					}
				}
				finally
				{
					if (HostingEnvironment.IsHosted && this._XmlFileName != null)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			catch
			{
				throw;
			}
			string[] array = new string[stringCollection.Count];
			stringCollection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000AEB28 File Offset: 0x000ADB28
		private object GetRole(string roleName)
		{
			this.InitApp();
			object[] array = new object[] { roleName, null };
			return this.CallMethod((this._ObjAzScope != null) ? this._ObjAzScope : this._ObjAzApplication, "OpenRole", array);
		}

		// Token: 0x04001E63 RID: 7779
		private string _AppName;

		// Token: 0x04001E64 RID: 7780
		private string _ConnectionString;

		// Token: 0x04001E65 RID: 7781
		private int _CacheRefreshInterval;

		// Token: 0x04001E66 RID: 7782
		private string _ScopeName;

		// Token: 0x04001E67 RID: 7783
		private object _ObjAzApplication;

		// Token: 0x04001E68 RID: 7784
		private bool _InitAppDone;

		// Token: 0x04001E69 RID: 7785
		private object _ObjAzScope;

		// Token: 0x04001E6A RID: 7786
		private DateTime _LastUpdateCacheDate;

		// Token: 0x04001E6B RID: 7787
		private object _ObjAzAuthorizationStoreClass;

		// Token: 0x04001E6C RID: 7788
		private bool _NewAuthInterface;

		// Token: 0x04001E6D RID: 7789
		private string _XmlFileName;
	}
}

using System;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Design;
using System.DirectoryServices.Interop;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.DirectoryServices
{
	// Token: 0x02000020 RID: 32
	[DSDescription("DirectoryEntryDesc")]
	[TypeConverter(typeof(DirectoryEntryConverter))]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class DirectoryEntry : global::System.ComponentModel.Component
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003300 File Offset: 0x00002300
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryEntry()
		{
			this.options = new DirectoryEntryConfiguration(this);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000332D File Offset: 0x0000232D
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryEntry(string path)
			: this()
		{
			this.Path = path;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000333C File Offset: 0x0000233C
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryEntry(string path, string username, string password)
			: this(path, username, password, AuthenticationTypes.Secure)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003348 File Offset: 0x00002348
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryEntry(string path, string username, string password, AuthenticationTypes authenticationType)
			: this(path)
		{
			this.credentials = new NetworkCredential(username, password);
			if (username == null)
			{
				this.userNameIsNull = true;
			}
			if (password == null)
			{
				this.passwordIsNull = true;
			}
			this.authenticationType = authenticationType;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000337C File Offset: 0x0000237C
		internal DirectoryEntry(string path, bool useCache, string username, string password, AuthenticationTypes authenticationType)
		{
			this.path = path;
			this.useCache = useCache;
			this.credentials = new NetworkCredential(username, password);
			if (username == null)
			{
				this.userNameIsNull = true;
			}
			if (password == null)
			{
				this.passwordIsNull = true;
			}
			this.authenticationType = authenticationType;
			this.options = new DirectoryEntryConfiguration(this);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000033ED File Offset: 0x000023ED
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryEntry(object adsObject)
			: this(adsObject, true, null, null, AuthenticationTypes.Secure, true)
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000033FB File Offset: 0x000023FB
		internal DirectoryEntry(object adsObject, bool useCache, string username, string password, AuthenticationTypes authenticationType)
			: this(adsObject, useCache, username, password, authenticationType, false)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000340C File Offset: 0x0000240C
		internal DirectoryEntry(object adsObject, bool useCache, string username, string password, AuthenticationTypes authenticationType, bool AdsObjIsExternal)
		{
			this.adsObject = adsObject as global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds;
			if (this.adsObject == null)
			{
				throw new ArgumentException(Res.GetString("DSDoesNotImplementIADs"));
			}
			this.path = this.adsObject.ADsPath;
			this.useCache = useCache;
			this.authenticationType = authenticationType;
			this.credentials = new NetworkCredential(username, password);
			if (username == null)
			{
				this.userNameIsNull = true;
			}
			if (password == null)
			{
				this.passwordIsNull = true;
			}
			if (!useCache)
			{
				this.CommitChanges();
			}
			this.options = new DirectoryEntryConfiguration(this);
			if (!AdsObjIsExternal)
			{
				this.InitADsObjectOptions();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000034BE File Offset: 0x000024BE
		internal global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds AdsObject
		{
			get
			{
				this.Bind();
				return this.adsObject;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000034CC File Offset: 0x000024CC
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000034D4 File Offset: 0x000024D4
		[DefaultValue(AuthenticationTypes.Secure)]
		[DSDescription("DSAuthenticationType")]
		public AuthenticationTypes AuthenticationType
		{
			get
			{
				return this.authenticationType;
			}
			set
			{
				if (this.authenticationType == value)
				{
					return;
				}
				this.authenticationType = value;
				this.Unbind();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000034ED File Offset: 0x000024ED
		private bool Bound
		{
			get
			{
				return this.adsObject != null;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000034FB File Offset: 0x000024FB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DSDescription("DSChildren")]
		[Browsable(false)]
		public DirectoryEntries Children
		{
			get
			{
				return new DirectoryEntries(this);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003503 File Offset: 0x00002503
		internal global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsContainer ContainerObject
		{
			get
			{
				this.Bind();
				return (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsContainer)this.adsObject;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003518 File Offset: 0x00002518
		[DSDescription("DSGuid")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Guid Guid
		{
			get
			{
				string nativeGuid = this.NativeGuid;
				if (nativeGuid.Length == 32)
				{
					byte[] array = new byte[16];
					for (int i = 0; i < 16; i++)
					{
						array[i] = Convert.ToByte(new string(new char[]
						{
							nativeGuid[i * 2],
							nativeGuid[i * 2 + 1]
						}), 16);
					}
					return new Guid(array);
				}
				return new Guid(nativeGuid);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003589 File Offset: 0x00002589
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000035AC File Offset: 0x000025AC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DSDescription("DSObjectSecurity")]
		public ActiveDirectorySecurity ObjectSecurity
		{
			get
			{
				if (!this.objectSecurityInitialized)
				{
					this.objectSecurity = this.GetObjectSecurityFromCache();
					this.objectSecurityInitialized = true;
				}
				return this.objectSecurity;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.objectSecurity = value;
				this.objectSecurityInitialized = true;
				this.objectSecurityModified = true;
				this.CommitIfNotCaching();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000035D7 File Offset: 0x000025D7
		internal bool IsContainer
		{
			get
			{
				this.Bind();
				return this.adsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsContainer;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000035ED File Offset: 0x000025ED
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000035F5 File Offset: 0x000025F5
		internal bool JustCreated
		{
			get
			{
				return this.justCreated;
			}
			set
			{
				this.justCreated = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003600 File Offset: 0x00002600
		[DSDescription("DSName")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Name
		{
			get
			{
				this.Bind();
				string name = this.adsObject.Name;
				GC.KeepAlive(this);
				return name;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003628 File Offset: 0x00002628
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DSDescription("DSNativeGuid")]
		[Browsable(false)]
		public string NativeGuid
		{
			get
			{
				this.FillCache("GUID");
				string guid = this.adsObject.GUID;
				GC.KeepAlive(this);
				return guid;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003653 File Offset: 0x00002653
		[DSDescription("DSNativeObject")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object NativeObject
		{
			get
			{
				this.Bind();
				return this.adsObject;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003661 File Offset: 0x00002661
		[DSDescription("DSParent")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DirectoryEntry Parent
		{
			get
			{
				this.Bind();
				return new DirectoryEntry(this.adsObject.Parent, this.UsePropertyCache, this.GetUsername(), this.GetPassword(), this.AuthenticationType);
			}
		}

		// Token: 0x1700001E RID: 30
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003694 File Offset: 0x00002694
		[DefaultValue(null)]
		[Browsable(false)]
		[DSDescription("DSPassword")]
		public string Password
		{
			set
			{
				if (value == this.GetPassword())
				{
					return;
				}
				if (this.credentials == null)
				{
					this.credentials = new NetworkCredential();
					this.userNameIsNull = true;
				}
				if (value == null)
				{
					this.passwordIsNull = true;
				}
				else
				{
					this.passwordIsNull = false;
				}
				this.credentials.Password = value;
				this.Unbind();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000036EF File Offset: 0x000026EF
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000036F7 File Offset: 0x000026F7
		[SettingsBindable(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DSDescription("DSPath")]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (Utils.Compare(this.path, value) == 0)
				{
					return;
				}
				this.path = value;
				this.Unbind();
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000371F File Offset: 0x0000271F
		[DSDescription("DSProperties")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public PropertyCollection Properties
		{
			get
			{
				if (this.propertyCollection == null)
				{
					this.propertyCollection = new PropertyCollection(this);
				}
				return this.propertyCollection;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000373C File Offset: 0x0000273C
		[DSDescription("DSSchemaClassName")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SchemaClassName
		{
			get
			{
				this.Bind();
				string @class = this.adsObject.Class;
				GC.KeepAlive(this);
				return @class;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003762 File Offset: 0x00002762
		[Browsable(false)]
		[DSDescription("DSSchemaEntry")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DirectoryEntry SchemaEntry
		{
			get
			{
				this.Bind();
				return new DirectoryEntry(this.adsObject.Schema, this.UsePropertyCache, this.GetUsername(), this.GetPassword(), this.AuthenticationType);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003792 File Offset: 0x00002792
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000379A File Offset: 0x0000279A
		[DefaultValue(true)]
		[DSDescription("DSUsePropertyCache")]
		public bool UsePropertyCache
		{
			get
			{
				return this.useCache;
			}
			set
			{
				if (value == this.useCache)
				{
					return;
				}
				if (!value)
				{
					this.CommitChanges();
				}
				this.cacheFilled = false;
				this.useCache = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000037BD File Offset: 0x000027BD
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000037DC File Offset: 0x000027DC
		[DSDescription("DSUsername")]
		[DefaultValue(null)]
		[Browsable(false)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Username
		{
			get
			{
				if (this.credentials == null || this.userNameIsNull)
				{
					return null;
				}
				return this.credentials.UserName;
			}
			set
			{
				if (value == this.GetUsername())
				{
					return;
				}
				if (this.credentials == null)
				{
					this.credentials = new NetworkCredential();
					this.passwordIsNull = true;
				}
				if (value == null)
				{
					this.userNameIsNull = true;
				}
				else
				{
					this.userNameIsNull = false;
				}
				this.credentials.UserName = value;
				this.Unbind();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003837 File Offset: 0x00002837
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[DSDescription("DSOptions")]
		[Browsable(false)]
		public DirectoryEntryConfiguration Options
		{
			get
			{
				if (!(this.AdsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsObjectOptions))
				{
					return null;
				}
				return this.options;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003850 File Offset: 0x00002850
		internal void InitADsObjectOptions()
		{
			if (this.adsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsObjectOptions2)
			{
				object obj = null;
				int option = ((global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsObjectOptions2)this.adsObject).GetOption(8, out obj);
				if (option != 0)
				{
					if (option == -2147467263 || option == -2147463160)
					{
						return;
					}
					throw COMExceptionHelper.CreateFormattedComException(option);
				}
				else
				{
					Variant variant = default(Variant);
					variant.varType = 11;
					variant.boolvalue = -1;
					((global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsObjectOptions2)this.adsObject).SetOption(8, variant);
					this.allowMultipleChange = true;
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000038CF File Offset: 0x000028CF
		private void Bind()
		{
			this.Bind(true);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000038D8 File Offset: 0x000028D8
		internal void Bind(bool throwIfFail)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.adsObject == null)
			{
				string text = this.Path;
				if (text == null || text.Length == 0)
				{
					DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE", true, null, null, AuthenticationTypes.Secure);
					string text2 = (string)directoryEntry.Properties["defaultNamingContext"][0];
					directoryEntry.Dispose();
					text = "LDAP://" + text2;
				}
				if (Thread.CurrentThread.GetApartmentState() == ApartmentState.Unknown)
				{
					Thread.CurrentThread.SetApartmentState(ApartmentState.MTA);
				}
				Guid guid = new Guid("00000000-0000-0000-c000-000000000046");
				object obj = null;
				int num = global::System.DirectoryServices.Interop.UnsafeNativeMethods.ADsOpenObject(text, this.GetUsername(), this.GetPassword(), (int)this.authenticationType, ref guid, out obj);
				if (num != 0)
				{
					if (throwIfFail)
					{
						throw COMExceptionHelper.CreateFormattedComException(num);
					}
				}
				else
				{
					this.adsObject = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds)obj;
				}
				this.InitADsObjectOptions();
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000039C4 File Offset: 0x000029C4
		internal DirectoryEntry CloneBrowsable()
		{
			return new DirectoryEntry(this.Path, this.UsePropertyCache, this.GetUsername(), this.GetPassword(), this.AuthenticationType);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000039F6 File Offset: 0x000029F6
		public void Close()
		{
			this.Unbind();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003A00 File Offset: 0x00002A00
		public void CommitChanges()
		{
			if (this.justCreated)
			{
				try
				{
					this.SetObjectSecurityInCache();
					this.adsObject.SetInfo();
				}
				catch (COMException ex)
				{
					throw COMExceptionHelper.CreateFormattedComException(ex);
				}
				this.justCreated = false;
				this.objectSecurityInitialized = false;
				this.objectSecurityModified = false;
				this.propertyCollection = null;
				return;
			}
			if (!this.useCache && (this.objectSecurity == null || !this.objectSecurity.IsModified()))
			{
				return;
			}
			if (!this.Bound)
			{
				return;
			}
			try
			{
				this.SetObjectSecurityInCache();
				this.adsObject.SetInfo();
				this.objectSecurityInitialized = false;
				this.objectSecurityModified = false;
			}
			catch (COMException ex2)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex2);
			}
			this.propertyCollection = null;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003AC0 File Offset: 0x00002AC0
		internal void CommitIfNotCaching()
		{
			if (this.justCreated)
			{
				return;
			}
			if (this.useCache)
			{
				return;
			}
			if (!this.Bound)
			{
				return;
			}
			new DirectoryServicesPermission(PermissionState.Unrestricted).Demand();
			try
			{
				this.SetObjectSecurityInCache();
				this.adsObject.SetInfo();
				this.objectSecurityInitialized = false;
				this.objectSecurityModified = false;
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			this.propertyCollection = null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003B34 File Offset: 0x00002B34
		public DirectoryEntry CopyTo(DirectoryEntry newParent)
		{
			return this.CopyTo(newParent, null);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003B40 File Offset: 0x00002B40
		public DirectoryEntry CopyTo(DirectoryEntry newParent, string newName)
		{
			if (!newParent.IsContainer)
			{
				throw new InvalidOperationException(Res.GetString("DSNotAContainer", new object[] { newParent.Path }));
			}
			object obj = null;
			try
			{
				obj = newParent.ContainerObject.CopyHere(this.Path, newName);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			return new DirectoryEntry(obj, newParent.UsePropertyCache, this.GetUsername(), this.GetPassword(), this.AuthenticationType);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003BC4 File Offset: 0x00002BC4
		public void DeleteTree()
		{
			if (!(this.AdsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsDeleteOps))
			{
				throw new InvalidOperationException(Res.GetString("DSCannotDelete"));
			}
			global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsDeleteOps adsDeleteOps = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsDeleteOps)this.AdsObject;
			try
			{
				adsDeleteOps.DeleteObject(0);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003C24 File Offset: 0x00002C24
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.Unbind();
				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003C44 File Offset: 0x00002C44
		public static bool Exists(string path)
		{
			DirectoryEntry directoryEntry = new DirectoryEntry(path);
			bool flag;
			try
			{
				directoryEntry.Bind(true);
				flag = directoryEntry.Bound;
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147016656 && ex.ErrorCode != -2147024893 && ex.ErrorCode != -2147022676)
				{
					throw;
				}
				flag = false;
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return flag;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003CBC File Offset: 0x00002CBC
		internal void FillCache(string propertyName)
		{
			if (!this.UsePropertyCache)
			{
				this.Bind();
				try
				{
					if (propertyName.Length > 0)
					{
						this.adsObject.GetInfoEx(new object[] { propertyName }, 0);
					}
					else
					{
						this.adsObject.GetInfo();
					}
				}
				catch (COMException ex)
				{
					throw COMExceptionHelper.CreateFormattedComException(ex);
				}
				return;
			}
			if (this.cacheFilled)
			{
				return;
			}
			this.RefreshCache();
			this.cacheFilled = true;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003D38 File Offset: 0x00002D38
		public object Invoke(string methodName, params object[] args)
		{
			object nativeObject = this.NativeObject;
			Type type = nativeObject.GetType();
			object obj = null;
			try
			{
				obj = type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, nativeObject, args, CultureInfo.InvariantCulture);
				GC.KeepAlive(this);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			catch (TargetInvocationException ex2)
			{
				if (ex2.InnerException != null && ex2.InnerException is COMException)
				{
					COMException ex3 = (COMException)ex2.InnerException;
					throw new TargetInvocationException(ex2.Message, COMExceptionHelper.CreateFormattedComException(ex3));
				}
				throw ex2;
			}
			if (obj is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds)
			{
				return new DirectoryEntry(obj, this.UsePropertyCache, this.GetUsername(), this.GetPassword(), this.AuthenticationType);
			}
			return obj;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003DFC File Offset: 0x00002DFC
		[ComVisible(false)]
		public object InvokeGet(string propertyName)
		{
			object nativeObject = this.NativeObject;
			Type type = nativeObject.GetType();
			object obj = null;
			try
			{
				obj = type.InvokeMember(propertyName, BindingFlags.GetProperty, null, nativeObject, null, CultureInfo.InvariantCulture);
				GC.KeepAlive(this);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			catch (TargetInvocationException ex2)
			{
				if (ex2.InnerException != null && ex2.InnerException is COMException)
				{
					COMException ex3 = (COMException)ex2.InnerException;
					throw new TargetInvocationException(ex2.Message, COMExceptionHelper.CreateFormattedComException(ex3));
				}
				throw ex2;
			}
			return obj;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003E9C File Offset: 0x00002E9C
		[ComVisible(false)]
		public void InvokeSet(string propertyName, params object[] args)
		{
			object nativeObject = this.NativeObject;
			Type type = nativeObject.GetType();
			try
			{
				type.InvokeMember(propertyName, BindingFlags.SetProperty, null, nativeObject, args, CultureInfo.InvariantCulture);
				GC.KeepAlive(this);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			catch (TargetInvocationException ex2)
			{
				if (ex2.InnerException != null && ex2.InnerException is COMException)
				{
					COMException ex3 = (COMException)ex2.InnerException;
					throw new TargetInvocationException(ex2.Message, COMExceptionHelper.CreateFormattedComException(ex3));
				}
				throw ex2;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003F30 File Offset: 0x00002F30
		public void MoveTo(DirectoryEntry newParent)
		{
			this.MoveTo(newParent, null);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003F3C File Offset: 0x00002F3C
		public void MoveTo(DirectoryEntry newParent, string newName)
		{
			object obj = null;
			if (!(newParent.AdsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsContainer))
			{
				throw new InvalidOperationException(Res.GetString("DSNotAContainer", new object[] { newParent.Path }));
			}
			try
			{
				if (this.AdsObject.ADsPath.StartsWith("WinNT:", StringComparison.Ordinal))
				{
					string text = this.AdsObject.ADsPath;
					string adsPath = newParent.AdsObject.ADsPath;
					if (Utils.Compare(text, 0, adsPath.Length, adsPath, 0, adsPath.Length) == 0)
					{
						uint num = Utils.NORM_IGNORENONSPACE | Utils.NORM_IGNOREKANATYPE | Utils.NORM_IGNOREWIDTH | Utils.SORT_STRINGSORT;
						if (Utils.Compare(text, 0, adsPath.Length, adsPath, 0, adsPath.Length, num) != 0)
						{
							text = adsPath + text.Substring(adsPath.Length);
						}
					}
					obj = newParent.ContainerObject.MoveHere(text, newName);
				}
				else
				{
					obj = newParent.ContainerObject.MoveHere(this.Path, newName);
				}
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			if (this.Bound)
			{
				Marshal.ReleaseComObject(this.adsObject);
			}
			this.adsObject = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds)obj;
			this.path = this.adsObject.ADsPath;
			this.InitADsObjectOptions();
			if (!this.useCache)
			{
				this.CommitChanges();
				return;
			}
			this.RefreshCache();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004094 File Offset: 0x00003094
		public void RefreshCache()
		{
			this.Bind();
			try
			{
				this.adsObject.GetInfo();
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			this.cacheFilled = true;
			this.propertyCollection = null;
			this.objectSecurityInitialized = false;
			this.objectSecurityModified = false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000040E8 File Offset: 0x000030E8
		public void RefreshCache(string[] propertyNames)
		{
			this.Bind();
			object[] array = new object[propertyNames.Length];
			for (int i = 0; i < propertyNames.Length; i++)
			{
				array[i] = propertyNames[i];
			}
			try
			{
				this.AdsObject.GetInfoEx(array, 0);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			this.cacheFilled = true;
			if (this.propertyCollection != null && propertyNames != null)
			{
				for (int j = 0; j < propertyNames.Length; j++)
				{
					if (propertyNames[j] != null)
					{
						string text = propertyNames[j].ToLower(CultureInfo.InvariantCulture);
						this.propertyCollection.valueTable.Remove(text);
						string[] array2 = text.Split(new char[] { ';' });
						if (array2.Length != 1)
						{
							string text2 = "";
							for (int k = 0; k < array2.Length; k++)
							{
								if (!array2[k].StartsWith("range=", StringComparison.Ordinal))
								{
									text2 += array2[k];
									text2 += ";";
								}
							}
							text2 = text2.Remove(text2.Length - 1, 1);
							this.propertyCollection.valueTable.Remove(text2);
						}
						if (string.Compare(propertyNames[j], DirectoryEntry.securityDescriptorProperty, StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.objectSecurityInitialized = false;
							this.objectSecurityModified = false;
						}
					}
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004240 File Offset: 0x00003240
		public void Rename(string newName)
		{
			this.MoveTo(this.Parent, newName);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000424F File Offset: 0x0000324F
		private void Unbind()
		{
			if (this.adsObject != null)
			{
				Marshal.ReleaseComObject(this.adsObject);
			}
			this.adsObject = null;
			this.propertyCollection = null;
			this.objectSecurityInitialized = false;
			this.objectSecurityModified = false;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004281 File Offset: 0x00003281
		internal string GetUsername()
		{
			if (this.credentials == null || this.userNameIsNull)
			{
				return null;
			}
			return this.credentials.UserName;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000042A0 File Offset: 0x000032A0
		internal string GetPassword()
		{
			if (this.credentials == null || this.passwordIsNull)
			{
				return null;
			}
			return this.credentials.Password;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000042C0 File Offset: 0x000032C0
		private ActiveDirectorySecurity GetObjectSecurityFromCache()
		{
			ActiveDirectorySecurity activeDirectorySecurity;
			try
			{
				if (!this.JustCreated)
				{
					SecurityMasks securityMasks = this.Options.SecurityMasks;
					this.RefreshCache(new string[] { DirectoryEntry.securityDescriptorProperty });
					if (!(this.NativeObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyList))
					{
						throw new NotSupportedException(Res.GetString("DSPropertyListUnsupported"));
					}
					global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyList adsPropertyList = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyList)this.NativeObject;
					global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyEntry adsPropertyEntry = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyEntry)adsPropertyList.GetPropertyItem(DirectoryEntry.securityDescriptorProperty, 8);
					GC.KeepAlive(this);
					object[] array = (object[])adsPropertyEntry.Values;
					if (array.Length < 1)
					{
						throw new InvalidOperationException(Res.GetString("DSSDNoValues"));
					}
					if (array.Length > 1)
					{
						throw new NotSupportedException(Res.GetString("DSMultipleSDNotSupported"));
					}
					global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyValue adsPropertyValue = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyValue)array[0];
					activeDirectorySecurity = new ActiveDirectorySecurity((byte[])adsPropertyValue.OctetString, securityMasks);
				}
				else
				{
					activeDirectorySecurity = null;
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147463155)
				{
					throw;
				}
				activeDirectorySecurity = null;
			}
			return activeDirectorySecurity;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000043C4 File Offset: 0x000033C4
		private void SetObjectSecurityInCache()
		{
			if (this.objectSecurity != null && (this.objectSecurityModified || this.objectSecurity.IsModified()))
			{
				global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyValue adsPropertyValue = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyValue)new global::System.DirectoryServices.Interop.UnsafeNativeMethods.PropertyValue();
				adsPropertyValue.ADsType = 8;
				adsPropertyValue.OctetString = this.objectSecurity.GetSecurityDescriptorBinaryForm();
				global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyEntry adsPropertyEntry = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyEntry)new global::System.DirectoryServices.Interop.UnsafeNativeMethods.PropertyEntry();
				adsPropertyEntry.Name = DirectoryEntry.securityDescriptorProperty;
				adsPropertyEntry.ADsType = 8;
				adsPropertyEntry.ControlCode = 2;
				adsPropertyEntry.Values = new object[] { adsPropertyValue };
				((global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAdsPropertyList)this.NativeObject).PutPropertyItem(adsPropertyEntry);
			}
		}

		// Token: 0x0400016D RID: 365
		private string path = "";

		// Token: 0x0400016E RID: 366
		private global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds adsObject;

		// Token: 0x0400016F RID: 367
		private bool useCache = true;

		// Token: 0x04000170 RID: 368
		private bool cacheFilled;

		// Token: 0x04000171 RID: 369
		internal bool propertiesAlreadyEnumerated;

		// Token: 0x04000172 RID: 370
		private bool justCreated;

		// Token: 0x04000173 RID: 371
		private bool disposed;

		// Token: 0x04000174 RID: 372
		private AuthenticationTypes authenticationType = AuthenticationTypes.Secure;

		// Token: 0x04000175 RID: 373
		private NetworkCredential credentials;

		// Token: 0x04000176 RID: 374
		private DirectoryEntryConfiguration options;

		// Token: 0x04000177 RID: 375
		private PropertyCollection propertyCollection;

		// Token: 0x04000178 RID: 376
		internal bool allowMultipleChange;

		// Token: 0x04000179 RID: 377
		private bool userNameIsNull;

		// Token: 0x0400017A RID: 378
		private bool passwordIsNull;

		// Token: 0x0400017B RID: 379
		private bool objectSecurityInitialized;

		// Token: 0x0400017C RID: 380
		private bool objectSecurityModified;

		// Token: 0x0400017D RID: 381
		private ActiveDirectorySecurity objectSecurity;

		// Token: 0x0400017E RID: 382
		private static string securityDescriptorProperty = "ntSecurityDescriptor";
	}
}

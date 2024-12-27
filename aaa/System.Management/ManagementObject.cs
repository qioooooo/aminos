using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class ManagementObject : ManagementBaseObject, ICloneable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600002C RID: 44 RVA: 0x00002AA8 File Offset: 0x00001AA8
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x00002AC1 File Offset: 0x00001AC1
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x0600002E RID: 46 RVA: 0x00002ADA File Offset: 0x00001ADA
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002AE4 File Offset: 0x00001AE4
		public new void Dispose()
		{
			if (this.wmiClass != null)
			{
				this.wmiClass.Dispose();
				this.wmiClass = null;
			}
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B0C File Offset: 0x00001B0C
		internal void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002B23 File Offset: 0x00001B23
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002B2B File Offset: 0x00001B2B
		internal bool PutButNotGot
		{
			get
			{
				return this.putButNotGot;
			}
			set
			{
				this.putButNotGot = value;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B34 File Offset: 0x00001B34
		private void HandleIdentifierChange(object sender, IdentifierChangedEventArgs e)
		{
			base.wbemObject = null;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002B3D File Offset: 0x00001B3D
		internal bool IsBound
		{
			get
			{
				return this._wbemObject != null;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B4C File Offset: 0x00001B4C
		internal static ManagementObject GetManagementObject(IWbemClassObjectFreeThreaded wbemObject, ManagementObject mgObj)
		{
			ManagementObject managementObject = new ManagementObject();
			managementObject.wbemObject = wbemObject;
			if (mgObj != null)
			{
				managementObject.scope = ManagementScope._Clone(mgObj.scope);
				if (mgObj.path != null)
				{
					managementObject.path = ManagementPath._Clone(mgObj.path);
				}
				if (mgObj.options != null)
				{
					managementObject.options = ObjectGetOptions._Clone(mgObj.options);
				}
			}
			return managementObject;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002BB0 File Offset: 0x00001BB0
		internal static ManagementObject GetManagementObject(IWbemClassObjectFreeThreaded wbemObject, ManagementScope scope)
		{
			ManagementObject managementObject = new ManagementObject();
			managementObject.wbemObject = wbemObject;
			managementObject.path = new ManagementPath(ManagementPath.GetManagementPath(wbemObject));
			managementObject.path.IdentifierChanged += managementObject.HandleIdentifierChange;
			managementObject.scope = ManagementScope._Clone(scope, new IdentifierChangedEventHandler(managementObject.HandleIdentifierChange));
			return managementObject;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C0B File Offset: 0x00001C0B
		public ManagementObject()
			: this(null, null, null)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C16 File Offset: 0x00001C16
		public ManagementObject(ManagementPath path)
			: this(null, path, null)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C21 File Offset: 0x00001C21
		public ManagementObject(string path)
			: this(null, new ManagementPath(path), null)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C31 File Offset: 0x00001C31
		public ManagementObject(ManagementPath path, ObjectGetOptions options)
			: this(null, path, options)
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C3C File Offset: 0x00001C3C
		public ManagementObject(string path, ObjectGetOptions options)
			: this(new ManagementPath(path), options)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C4B File Offset: 0x00001C4B
		public ManagementObject(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
			: base(null)
		{
			this.ManagementObjectCTOR(scope, path, options);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C60 File Offset: 0x00001C60
		private void ManagementObjectCTOR(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
		{
			string text = string.Empty;
			if (path != null && !path.IsEmpty)
			{
				if (base.GetType() == typeof(ManagementObject) && path.IsClass)
				{
					throw new ArgumentOutOfRangeException("path");
				}
				if (base.GetType() == typeof(ManagementClass) && path.IsInstance)
				{
					throw new ArgumentOutOfRangeException("path");
				}
				text = path.GetNamespacePath(8);
				if (scope != null && scope.Path.NamespacePath.Length > 0)
				{
					path = new ManagementPath(path.RelativePath);
					path.NamespacePath = scope.Path.GetNamespacePath(8);
				}
				if (path.IsClass || path.IsInstance)
				{
					this.path = ManagementPath._Clone(path, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
				}
				else
				{
					this.path = ManagementPath._Clone(null, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
				}
			}
			if (options != null)
			{
				this.options = ObjectGetOptions._Clone(options, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			if (scope != null)
			{
				this.scope = ManagementScope._Clone(scope, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			else if (text.Length > 0)
			{
				this.scope = new ManagementScope(text);
				this.scope.IdentifierChanged += this.HandleIdentifierChange;
			}
			this.IdentifierChanged = (IdentifierChangedEventHandler)Delegate.Combine(this.IdentifierChanged, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			this.putButNotGot = false;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DDA File Offset: 0x00001DDA
		public ManagementObject(string scopeString, string pathString, ObjectGetOptions options)
			: this(new ManagementScope(scopeString), new ManagementPath(pathString), options)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002DEF File Offset: 0x00001DEF
		protected ManagementObject(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.ManagementObjectCTOR(null, null, null);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002E04 File Offset: 0x00001E04
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002E30 File Offset: 0x00001E30
		public ManagementScope Scope
		{
			get
			{
				if (this.scope == null)
				{
					return this.scope = ManagementScope._Clone(null);
				}
				return this.scope;
			}
			set
			{
				if (value != null)
				{
					if (this.scope != null)
					{
						this.scope.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.scope = ManagementScope._Clone(value, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
					this.FireIdentifierChanged();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002E88 File Offset: 0x00001E88
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002EB4 File Offset: 0x00001EB4
		public virtual ManagementPath Path
		{
			get
			{
				if (this.path == null)
				{
					return this.path = ManagementPath._Clone(null);
				}
				return this.path;
			}
			set
			{
				ManagementPath managementPath = ((value != null) ? value : new ManagementPath());
				string namespacePath = managementPath.GetNamespacePath(8);
				if (namespacePath.Length > 0 && this.scope != null && this.scope.IsDefaulted)
				{
					this.Scope = new ManagementScope(namespacePath);
				}
				if ((base.GetType() == typeof(ManagementObject) && managementPath.IsInstance) || (base.GetType() == typeof(ManagementClass) && managementPath.IsClass) || managementPath.IsEmpty)
				{
					if (this.path != null)
					{
						this.path.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.path = ManagementPath._Clone(value, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
					this.FireIdentifierChanged();
					return;
				}
				throw new ArgumentOutOfRangeException("value");
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002F84 File Offset: 0x00001F84
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002FB0 File Offset: 0x00001FB0
		public ObjectGetOptions Options
		{
			get
			{
				if (this.options == null)
				{
					return this.options = ObjectGetOptions._Clone(null);
				}
				return this.options;
			}
			set
			{
				if (value != null)
				{
					if (this.options != null)
					{
						this.options.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.options = ObjectGetOptions._Clone(value, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
					this.FireIdentifierChanged();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003008 File Offset: 0x00002008
		public override ManagementPath ClassPath
		{
			get
			{
				object obj = null;
				object obj2 = null;
				object obj3 = null;
				int num = 0;
				int num2 = 0;
				if (this.PutButNotGot)
				{
					this.Get();
					this.PutButNotGot = false;
				}
				int num3 = base.wbemObject.Get_("__SERVER", 0, ref obj, ref num, ref num2);
				if (num3 >= 0)
				{
					num3 = base.wbemObject.Get_("__NAMESPACE", 0, ref obj2, ref num, ref num2);
					if (num3 >= 0)
					{
						num3 = base.wbemObject.Get_("__CLASS", 0, ref obj3, ref num, ref num2);
					}
				}
				if (num3 < 0)
				{
					if (((long)num3 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				ManagementPath managementPath = new ManagementPath();
				managementPath.Server = string.Empty;
				managementPath.NamespacePath = string.Empty;
				managementPath.ClassName = string.Empty;
				try
				{
					managementPath.Server = (string)((obj is DBNull) ? "" : obj);
					managementPath.NamespacePath = (string)((obj2 is DBNull) ? "" : obj2);
					managementPath.ClassName = (string)((obj3 is DBNull) ? "" : obj3);
				}
				catch
				{
				}
				return managementPath;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003154 File Offset: 0x00002154
		public void Get()
		{
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			this.Initialize(false);
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			ObjectGetOptions objectGetOptions = ((this.options == null) ? new ObjectGetOptions() : this.options);
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				int object_ = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).GetObject_(this.path.RelativePath, objectGetOptions.Flags, objectGetOptions.GetContext(), ref wbemClassObjectFreeThreaded, IntPtr.Zero);
				if (object_ < 0)
				{
					if (((long)object_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)object_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(object_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				base.wbemObject = wbemClassObjectFreeThreaded;
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000323C File Offset: 0x0000223C
		public void Get(ManagementOperationObserver watcher)
		{
			this.Initialize(false);
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			ObjectGetOptions objectGetOptions = ObjectGetOptions._Clone(this.options);
			WmiGetEventSink newGetSink = watcher.GetNewGetSink(this.scope, objectGetOptions.Context, this);
			if (watcher.HaveListenersForProgress)
			{
				objectGetOptions.SendStatus = true;
			}
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int objectAsync_ = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).GetObjectAsync_(this.path.RelativePath, objectGetOptions.Flags, objectGetOptions.GetContext(), newGetSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (objectAsync_ < 0)
			{
				watcher.RemoveSink(newGetSink);
				if (((long)objectAsync_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)objectAsync_);
					return;
				}
				Marshal.ThrowExceptionForHR(objectAsync_, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000333A File Offset: 0x0000233A
		public ManagementObjectCollection GetRelated()
		{
			return this.GetRelated(null);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003344 File Offset: 0x00002344
		public ManagementObjectCollection GetRelated(string relatedClass)
		{
			return this.GetRelated(relatedClass, null, null, null, null, null, false, null);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003360 File Offset: 0x00002360
		public ManagementObjectCollection GetRelated(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options != null) ? options : new EnumerationOptions());
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(this.path.Path, relatedClass, relationshipClass, relationshipQualifier, relatedQualifier, relatedRole, thisRole, classDefinitionsOnly);
			enumerationOptions.EnumerateDeep = true;
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQuery_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			return new ManagementObjectCollection(this.scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000346C File Offset: 0x0000246C
		public void GetRelated(ManagementOperationObserver watcher)
		{
			this.GetRelated(watcher, null);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003478 File Offset: 0x00002478
		public void GetRelated(ManagementOperationObserver watcher, string relatedClass)
		{
			this.GetRelated(watcher, relatedClass, null, null, null, null, null, false, null);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003494 File Offset: 0x00002494
		public void GetRelated(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(true);
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			EnumerationOptions enumerationOptions = ((options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions());
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(this.scope, enumerationOptions.Context);
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(this.path.Path, relatedClass, relationshipClass, relationshipQualifier, relatedQualifier, relatedRole, thisRole, classDefinitionsOnly);
			enumerationOptions.EnumerateDeep = true;
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQueryAsync_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			securityHandler.Reset();
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000035C4 File Offset: 0x000025C4
		public ManagementObjectCollection GetRelationships()
		{
			return this.GetRelationships(null);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000035CD File Offset: 0x000025CD
		public ManagementObjectCollection GetRelationships(string relationshipClass)
		{
			return this.GetRelationships(relationshipClass, null, null, false, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000035DC File Offset: 0x000025DC
		public ManagementObjectCollection GetRelationships(string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options != null) ? options : new EnumerationOptions());
			RelationshipQuery relationshipQuery = new RelationshipQuery(this.path.Path, relationshipClass, relationshipQualifier, thisRole, classDefinitionsOnly);
			enumerationOptions.EnumerateDeep = true;
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQuery_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			return new ManagementObjectCollection(this.scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000036E0 File Offset: 0x000026E0
		public void GetRelationships(ManagementOperationObserver watcher)
		{
			this.GetRelationships(watcher, null);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000036EA File Offset: 0x000026EA
		public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass)
		{
			this.GetRelationships(watcher, relationshipClass, null, null, false, null);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000036F8 File Offset: 0x000026F8
		public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(false);
			EnumerationOptions enumerationOptions = ((options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions());
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(this.scope, enumerationOptions.Context);
			RelationshipQuery relationshipQuery = new RelationshipQuery(this.path.Path, relationshipClass, relationshipQualifier, thisRole, classDefinitionsOnly);
			enumerationOptions.EnumerateDeep = true;
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQueryAsync_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003825 File Offset: 0x00002825
		public ManagementPath Put()
		{
			return this.Put(null);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003830 File Offset: 0x00002830
		public ManagementPath Put(PutOptions options)
		{
			ManagementPath managementPath = null;
			this.Initialize(true);
			PutOptions putOptions = ((options != null) ? options : new PutOptions());
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IWbemCallResult wbemCallResult = null;
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				intPtr = Marshal.AllocHGlobal(IntPtr.Size);
				Marshal.WriteIntPtr(intPtr, IntPtr.Zero);
				int num;
				if (base.IsClass)
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutClass_(base.wbemObject, putOptions.Flags | 16, putOptions.GetContext(), intPtr);
				}
				else
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutInstance_(base.wbemObject, putOptions.Flags | 16, putOptions.GetContext(), intPtr);
				}
				intPtr2 = Marshal.ReadIntPtr(intPtr);
				wbemCallResult = (IWbemCallResult)Marshal.GetObjectForIUnknown(intPtr2);
				int num2;
				num = wbemCallResult.GetCallStatus_(-1, out num2);
				if (num >= 0)
				{
					num = num2;
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				managementPath = this.GetPath(wbemCallResult);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.Release(intPtr2);
				}
				if (wbemCallResult != null)
				{
					Marshal.ReleaseComObject(wbemCallResult);
				}
			}
			this.putButNotGot = true;
			this.path.SetRelativePath(managementPath.RelativePath);
			return managementPath;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000039C4 File Offset: 0x000029C4
		private ManagementPath GetPath(IWbemCallResult callResult)
		{
			ManagementPath managementPath = null;
			try
			{
				string text = null;
				int resultString_ = callResult.GetResultString_(-1, out text);
				if (resultString_ >= 0)
				{
					managementPath = new ManagementPath(this.scope.Path.Path);
					managementPath.RelativePath = text;
				}
				else
				{
					object obj = base.GetPropertyValue("__PATH");
					if (obj != null)
					{
						managementPath = new ManagementPath((string)obj);
					}
					else
					{
						obj = base.GetPropertyValue("__RELPATH");
						if (obj != null)
						{
							managementPath = new ManagementPath(this.scope.Path.Path);
							managementPath.RelativePath = (string)obj;
						}
					}
				}
			}
			catch
			{
			}
			if (managementPath == null)
			{
				managementPath = new ManagementPath();
			}
			return managementPath;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003A74 File Offset: 0x00002A74
		public void Put(ManagementOperationObserver watcher)
		{
			this.Put(watcher, null);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003A80 File Offset: 0x00002A80
		public void Put(ManagementOperationObserver watcher, PutOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(false);
			PutOptions putOptions = ((options == null) ? new PutOptions() : ((PutOptions)options.Clone()));
			if (watcher.HaveListenersForProgress)
			{
				putOptions.SendStatus = true;
			}
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			WmiEventSink newPutSink = watcher.GetNewPutSink(this.scope, putOptions.Context, this.scope.Path.GetNamespacePath(8), base.ClassName);
			newPutSink.InternalObjectPut += this.HandleObjectPut;
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num;
			if (base.IsClass)
			{
				num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutClassAsync_(base.wbemObject, putOptions.Flags, putOptions.GetContext(), newPutSink.Stub);
			}
			else
			{
				num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutInstanceAsync_(base.wbemObject, putOptions.Flags, putOptions.GetContext(), newPutSink.Stub);
			}
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				newPutSink.InternalObjectPut -= this.HandleObjectPut;
				watcher.RemoveSink(newPutSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003BD8 File Offset: 0x00002BD8
		internal void HandleObjectPut(object sender, InternalObjectPutEventArgs e)
		{
			try
			{
				if (sender is WmiEventSink)
				{
					((WmiEventSink)sender).InternalObjectPut -= this.HandleObjectPut;
					this.putButNotGot = true;
					this.path.SetRelativePath(e.Path.RelativePath);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003C38 File Offset: 0x00002C38
		public ManagementPath CopyTo(ManagementPath path)
		{
			return this.CopyTo(path, null);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003C42 File Offset: 0x00002C42
		public ManagementPath CopyTo(string path)
		{
			return this.CopyTo(new ManagementPath(path), null);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003C51 File Offset: 0x00002C51
		public ManagementPath CopyTo(string path, PutOptions options)
		{
			return this.CopyTo(new ManagementPath(path), options);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003C60 File Offset: 0x00002C60
		public ManagementPath CopyTo(ManagementPath path, PutOptions options)
		{
			this.Initialize(false);
			ManagementScope managementScope = new ManagementScope(path, this.scope);
			managementScope.Initialize();
			PutOptions putOptions = ((options != null) ? options : new PutOptions());
			IWbemServices iwbemServices = managementScope.GetIWbemServices();
			ManagementPath managementPath = null;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IWbemCallResult wbemCallResult = null;
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = managementScope.GetSecurityHandler();
				intPtr = Marshal.AllocHGlobal(IntPtr.Size);
				Marshal.WriteIntPtr(intPtr, IntPtr.Zero);
				int num;
				if (base.IsClass)
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutClass_(base.wbemObject, putOptions.Flags | 16, putOptions.GetContext(), intPtr);
				}
				else
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).PutInstance_(base.wbemObject, putOptions.Flags | 16, putOptions.GetContext(), intPtr);
				}
				intPtr2 = Marshal.ReadIntPtr(intPtr);
				wbemCallResult = (IWbemCallResult)Marshal.GetObjectForIUnknown(intPtr2);
				int num2;
				num = wbemCallResult.GetCallStatus_(-1, out num2);
				if (num >= 0)
				{
					num = num2;
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				managementPath = this.GetPath(wbemCallResult);
				managementPath.NamespacePath = path.GetNamespacePath(8);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.Release(intPtr2);
				}
				if (wbemCallResult != null)
				{
					Marshal.ReleaseComObject(wbemCallResult);
				}
			}
			return managementPath;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003DFC File Offset: 0x00002DFC
		public void CopyTo(ManagementOperationObserver watcher, ManagementPath path)
		{
			this.CopyTo(watcher, path, null);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003E07 File Offset: 0x00002E07
		public void CopyTo(ManagementOperationObserver watcher, string path)
		{
			this.CopyTo(watcher, new ManagementPath(path), null);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003E17 File Offset: 0x00002E17
		public void CopyTo(ManagementOperationObserver watcher, string path, PutOptions options)
		{
			this.CopyTo(watcher, new ManagementPath(path), options);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003E28 File Offset: 0x00002E28
		public void CopyTo(ManagementOperationObserver watcher, ManagementPath path, PutOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(false);
			ManagementScope managementScope = new ManagementScope(path, this.scope);
			managementScope.Initialize();
			PutOptions putOptions = ((options != null) ? ((PutOptions)options.Clone()) : new PutOptions());
			if (watcher.HaveListenersForProgress)
			{
				putOptions.SendStatus = true;
			}
			WmiEventSink newPutSink = watcher.GetNewPutSink(managementScope, putOptions.Context, path.GetNamespacePath(8), base.ClassName);
			IWbemServices iwbemServices = managementScope.GetIWbemServices();
			SecurityHandler securityHandler = managementScope.GetSecurityHandler();
			int num;
			if (base.IsClass)
			{
				num = managementScope.GetSecurityIWbemServicesHandler(iwbemServices).PutClassAsync_(base.wbemObject, putOptions.Flags, putOptions.GetContext(), newPutSink.Stub);
			}
			else
			{
				num = managementScope.GetSecurityIWbemServicesHandler(iwbemServices).PutInstanceAsync_(base.wbemObject, putOptions.Flags, putOptions.GetContext(), newPutSink.Stub);
			}
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newPutSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003F4C File Offset: 0x00002F4C
		public void Delete()
		{
			this.Delete(null);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003F58 File Offset: 0x00002F58
		public void Delete(DeleteOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			DeleteOptions deleteOptions = ((options != null) ? options : new DeleteOptions());
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				int num;
				if (base.IsClass)
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).DeleteClass_(this.path.RelativePath, deleteOptions.Flags, deleteOptions.GetContext(), IntPtr.Zero);
				}
				else
				{
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).DeleteInstance_(this.path.RelativePath, deleteOptions.Flags, deleteOptions.GetContext(), IntPtr.Zero);
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004064 File Offset: 0x00003064
		public void Delete(ManagementOperationObserver watcher)
		{
			this.Delete(watcher, null);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004070 File Offset: 0x00003070
		public void Delete(ManagementOperationObserver watcher, DeleteOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(false);
			DeleteOptions deleteOptions = ((options != null) ? ((DeleteOptions)options.Clone()) : new DeleteOptions());
			if (watcher.HaveListenersForProgress)
			{
				deleteOptions.SendStatus = true;
			}
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			WmiEventSink newSink = watcher.GetNewSink(this.scope, deleteOptions.Context);
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num;
			if (base.IsClass)
			{
				num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).DeleteClassAsync_(this.path.RelativePath, deleteOptions.Flags, deleteOptions.GetContext(), newSink.Stub);
			}
			else
			{
				num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).DeleteInstanceAsync_(this.path.RelativePath, deleteOptions.Flags, deleteOptions.GetContext(), newSink.Stub);
			}
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000041B4 File Offset: 0x000031B4
		public object InvokeMethod(string methodName, object[] args)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			this.Initialize(false);
			ManagementBaseObject managementBaseObject;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2;
			this.GetMethodParameters(methodName, out managementBaseObject, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
			ManagementObject.MapInParameters(args, managementBaseObject, wbemClassObjectFreeThreaded);
			ManagementBaseObject managementBaseObject2 = this.InvokeMethod(methodName, managementBaseObject, null);
			return ManagementObject.MapOutParameters(args, managementBaseObject2, wbemClassObjectFreeThreaded2);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004224 File Offset: 0x00003224
		public void InvokeMethod(ManagementOperationObserver watcher, string methodName, object[] args)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			this.Initialize(false);
			ManagementBaseObject managementBaseObject;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2;
			this.GetMethodParameters(methodName, out managementBaseObject, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
			ManagementObject.MapInParameters(args, managementBaseObject, wbemClassObjectFreeThreaded);
			this.InvokeMethod(watcher, methodName, managementBaseObject, null);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004294 File Offset: 0x00003294
		public ManagementBaseObject InvokeMethod(string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
		{
			ManagementBaseObject managementBaseObject = null;
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			this.Initialize(false);
			InvokeMethodOptions invokeMethodOptions = ((options != null) ? options : new InvokeMethodOptions());
			this.scope.GetIWbemServices();
			SecurityHandler securityHandler = null;
			try
			{
				securityHandler = this.scope.GetSecurityHandler();
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = ((inParameters == null) ? null : inParameters.wbemObject);
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
				int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecMethod_(this.path.RelativePath, methodName, invokeMethodOptions.Flags, invokeMethodOptions.GetContext(), wbemClassObjectFreeThreaded, ref wbemClassObjectFreeThreaded2, IntPtr.Zero);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				if (wbemClassObjectFreeThreaded2 != null)
				{
					managementBaseObject = new ManagementBaseObject(wbemClassObjectFreeThreaded2);
				}
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			return managementBaseObject;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000043A4 File Offset: 0x000033A4
		public void InvokeMethod(ManagementOperationObserver watcher, string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
		{
			if (this.path == null || this.path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			this.Initialize(false);
			InvokeMethodOptions invokeMethodOptions = ((options != null) ? ((InvokeMethodOptions)options.Clone()) : new InvokeMethodOptions());
			if (watcher.HaveListenersForProgress)
			{
				invokeMethodOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(this.scope, invokeMethodOptions.Context);
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			if (inParameters != null)
			{
				wbemClassObjectFreeThreaded = inParameters.wbemObject;
			}
			int num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecMethodAsync_(this.path.RelativePath, methodName, invokeMethodOptions.Flags, invokeMethodOptions.GetContext(), wbemClassObjectFreeThreaded, newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000044C4 File Offset: 0x000034C4
		public ManagementBaseObject GetMethodParameters(string methodName)
		{
			ManagementBaseObject managementBaseObject;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2;
			this.GetMethodParameters(methodName, out managementBaseObject, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
			return managementBaseObject;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000044E0 File Offset: 0x000034E0
		private void GetMethodParameters(string methodName, out ManagementBaseObject inParameters, out IWbemClassObjectFreeThreaded inParametersClass, out IWbemClassObjectFreeThreaded outParametersClass)
		{
			inParameters = null;
			inParametersClass = null;
			outParametersClass = null;
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			this.Initialize(false);
			if (this.wmiClass == null)
			{
				ManagementPath classPath = this.ClassPath;
				if (classPath == null || !classPath.IsClass)
				{
					throw new InvalidOperationException();
				}
				ManagementClass managementClass = new ManagementClass(this.scope, classPath, null);
				managementClass.Get();
				this.wmiClass = managementClass.wbemObject;
			}
			int num = this.wmiClass.GetMethod_(methodName, 0, out inParametersClass, out outParametersClass);
			if (num == -2147217406)
			{
				num = -2147217323;
			}
			if (num >= 0 && inParametersClass != null)
			{
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
				num = inParametersClass.SpawnInstance_(0, out wbemClassObjectFreeThreaded);
				if (num >= 0)
				{
					inParameters = new ManagementBaseObject(wbemClassObjectFreeThreaded);
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000045BC File Offset: 0x000035BC
		public override object Clone()
		{
			if (this.PutButNotGot)
			{
				this.Get();
				this.PutButNotGot = false;
			}
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			int num = base.wbemObject.Clone_(out wbemClassObjectFreeThreaded);
			if (num < 0)
			{
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return ManagementObject.GetManagementObject(wbemClassObjectFreeThreaded, this);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004622 File Offset: 0x00003622
		public override string ToString()
		{
			if (this.path != null)
			{
				return this.path.Path;
			}
			return "";
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004640 File Offset: 0x00003640
		internal override void Initialize(bool getObject)
		{
			bool flag = false;
			lock (this)
			{
				if (this.path == null)
				{
					this.path = new ManagementPath();
					this.path.IdentifierChanged += this.HandleIdentifierChange;
				}
				if (!this.IsBound && getObject)
				{
					flag = true;
				}
				if (this.scope == null)
				{
					string namespacePath = this.path.GetNamespacePath(8);
					if (0 < namespacePath.Length)
					{
						this.scope = new ManagementScope(namespacePath);
					}
					else
					{
						this.scope = new ManagementScope();
					}
					this.scope.IdentifierChanged += this.HandleIdentifierChange;
				}
				else if (this.scope.Path == null || this.scope.Path.IsEmpty)
				{
					string namespacePath2 = this.path.GetNamespacePath(8);
					if (0 < namespacePath2.Length)
					{
						this.scope.Path = new ManagementPath(namespacePath2);
					}
					else
					{
						this.scope.Path = ManagementPath.DefaultPath;
					}
				}
				lock (this.scope)
				{
					if (!this.scope.IsConnected)
					{
						this.scope.Initialize();
						if (getObject)
						{
							flag = true;
						}
					}
					if (flag)
					{
						if (this.options == null)
						{
							this.options = new ObjectGetOptions();
							this.options.IdentifierChanged += this.HandleIdentifierChange;
						}
						IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
						IWbemServices iwbemServices = this.scope.GetIWbemServices();
						SecurityHandler securityHandler = null;
						try
						{
							securityHandler = this.scope.GetSecurityHandler();
							string text = null;
							string relativePath = this.path.RelativePath;
							if (relativePath.Length > 0)
							{
								text = relativePath;
							}
							int num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).GetObject_(text, this.options.Flags, this.options.GetContext(), ref wbemClassObjectFreeThreaded, IntPtr.Zero);
							if (num >= 0)
							{
								base.wbemObject = wbemClassObjectFreeThreaded;
								object obj = null;
								int num2 = 0;
								int num3 = 0;
								num = base.wbemObject.Get_("__PATH", 0, ref obj, ref num2, ref num3);
								if (num >= 0)
								{
									this.path = ((DBNull.Value != obj) ? new ManagementPath((string)obj) : new ManagementPath());
									this.path.IdentifierChanged += this.HandleIdentifierChange;
								}
							}
							if (num < 0)
							{
								if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
								{
									ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
								}
								else
								{
									Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
								}
							}
						}
						finally
						{
							if (securityHandler != null)
							{
								securityHandler.Reset();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004910 File Offset: 0x00003910
		private static void MapInParameters(object[] args, ManagementBaseObject inParams, IWbemClassObjectFreeThreaded inParamsClass)
		{
			int num = 0;
			if (inParamsClass != null && args != null && 0 < args.Length)
			{
				int upperBound = args.GetUpperBound(0);
				int lowerBound = args.GetLowerBound(0);
				int num2 = upperBound - lowerBound;
				num = inParamsClass.BeginEnumeration_(64);
				if (num >= 0)
				{
					do
					{
						object obj = null;
						int num3 = 0;
						string text = null;
						IWbemQualifierSetFreeThreaded wbemQualifierSetFreeThreaded = null;
						num = inParamsClass.Next_(0, ref text, ref obj, ref num3, ref num3);
						if (num >= 0)
						{
							if (text == null)
							{
								break;
							}
							num = inParamsClass.GetPropertyQualifierSet_(text, out wbemQualifierSetFreeThreaded);
							if (num >= 0)
							{
								try
								{
									object obj2 = 0;
									wbemQualifierSetFreeThreaded.Get_("ID", 0, ref obj2, ref num3);
									int num4 = (int)obj2;
									if (0 <= num4 && num2 >= num4)
									{
										inParams[text] = args[lowerBound + num4];
									}
								}
								finally
								{
									wbemQualifierSetFreeThreaded.Dispose();
								}
							}
						}
					}
					while (num >= 0);
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						return;
					}
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004A10 File Offset: 0x00003A10
		private static object MapOutParameters(object[] args, ManagementBaseObject outParams, IWbemClassObjectFreeThreaded outParamsClass)
		{
			object obj = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (outParamsClass != null)
			{
				if (args != null && 0 < args.Length)
				{
					int upperBound = args.GetUpperBound(0);
					num = args.GetLowerBound(0);
					num2 = upperBound - num;
				}
				num3 = outParamsClass.BeginEnumeration_(64);
				if (num3 >= 0)
				{
					do
					{
						object obj2 = null;
						int num4 = 0;
						string text = null;
						IWbemQualifierSetFreeThreaded wbemQualifierSetFreeThreaded = null;
						num3 = outParamsClass.Next_(0, ref text, ref obj2, ref num4, ref num4);
						if (num3 >= 0)
						{
							if (text == null)
							{
								break;
							}
							if (string.Compare(text, "RETURNVALUE", StringComparison.OrdinalIgnoreCase) == 0)
							{
								obj = outParams["RETURNVALUE"];
							}
							else
							{
								num3 = outParamsClass.GetPropertyQualifierSet_(text, out wbemQualifierSetFreeThreaded);
								if (num3 >= 0)
								{
									try
									{
										object obj3 = 0;
										wbemQualifierSetFreeThreaded.Get_("ID", 0, ref obj3, ref num4);
										int num5 = (int)obj3;
										if (0 <= num5 && num2 >= num5)
										{
											args[num + num5] = outParams[text];
										}
									}
									finally
									{
										wbemQualifierSetFreeThreaded.Dispose();
									}
								}
							}
						}
					}
					while (num3 >= 0);
				}
				if (num3 < 0)
				{
					if (((long)num3 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return obj;
		}

		// Token: 0x04000072 RID: 114
		internal const string ID = "ID";

		// Token: 0x04000073 RID: 115
		internal const string RETURNVALUE = "RETURNVALUE";

		// Token: 0x04000074 RID: 116
		private IWbemClassObjectFreeThreaded wmiClass;

		// Token: 0x04000075 RID: 117
		internal ManagementScope scope;

		// Token: 0x04000076 RID: 118
		internal ManagementPath path;

		// Token: 0x04000077 RID: 119
		internal ObjectGetOptions options;

		// Token: 0x04000078 RID: 120
		private bool putButNotGot;
	}
}

using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class ManagementClass : ManagementObject
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00004B40 File Offset: 0x00003B40
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004B4C File Offset: 0x00003B4C
		internal static ManagementClass GetManagementClass(IWbemClassObjectFreeThreaded wbemObject, ManagementClass mgObj)
		{
			ManagementClass managementClass = new ManagementClass();
			managementClass.wbemObject = wbemObject;
			if (mgObj != null)
			{
				managementClass.scope = ManagementScope._Clone(mgObj.scope);
				ManagementPath path = mgObj.Path;
				if (path != null)
				{
					managementClass.path = ManagementPath._Clone(path);
				}
				object obj = null;
				int num = 0;
				int num2 = wbemObject.Get_("__CLASS", 0, ref obj, ref num, ref num);
				if (num2 < 0)
				{
					if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				if (obj != DBNull.Value)
				{
					managementClass.path.internalClassName = (string)obj;
				}
				ObjectGetOptions options = mgObj.Options;
				if (options != null)
				{
					managementClass.options = ObjectGetOptions._Clone(options);
				}
			}
			return managementClass;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004C10 File Offset: 0x00003C10
		internal static ManagementClass GetManagementClass(IWbemClassObjectFreeThreaded wbemObject, ManagementScope scope)
		{
			ManagementClass managementClass = new ManagementClass();
			managementClass.path = new ManagementPath(ManagementPath.GetManagementPath(wbemObject));
			if (scope != null)
			{
				managementClass.scope = ManagementScope._Clone(scope);
			}
			managementClass.wbemObject = wbemObject;
			return managementClass;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004C4B File Offset: 0x00003C4B
		public ManagementClass()
			: this(null, null, null)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004C56 File Offset: 0x00003C56
		public ManagementClass(ManagementPath path)
			: this(null, path, null)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004C61 File Offset: 0x00003C61
		public ManagementClass(string path)
			: this(null, new ManagementPath(path), null)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004C71 File Offset: 0x00003C71
		public ManagementClass(ManagementPath path, ObjectGetOptions options)
			: this(null, path, options)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004C7C File Offset: 0x00003C7C
		public ManagementClass(string path, ObjectGetOptions options)
			: this(null, new ManagementPath(path), options)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004C8C File Offset: 0x00003C8C
		public ManagementClass(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
			: base(scope, path, options)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004C97 File Offset: 0x00003C97
		public ManagementClass(string scope, string path, ObjectGetOptions options)
			: base(new ManagementScope(scope), new ManagementPath(path), options)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004CAC File Offset: 0x00003CAC
		protected ManagementClass(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00004CB6 File Offset: 0x00003CB6
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00004CBE File Offset: 0x00003CBE
		public override ManagementPath Path
		{
			get
			{
				return base.Path;
			}
			set
			{
				if (value == null || value.IsClass || value.IsEmpty)
				{
					base.Path = value;
					return;
				}
				throw new ArgumentOutOfRangeException("value");
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00004CE8 File Offset: 0x00003CE8
		public StringCollection Derivation
		{
			get
			{
				StringCollection stringCollection = new StringCollection();
				int num = 0;
				int num2 = 0;
				object obj = null;
				int num3 = base.wbemObject.Get_("__DERIVATION", 0, ref obj, ref num, ref num2);
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
				if (obj != null)
				{
					stringCollection.AddRange((string[])obj);
				}
				return stringCollection;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004D5B File Offset: 0x00003D5B
		public MethodDataCollection Methods
		{
			get
			{
				this.Initialize(true);
				if (this.methods == null)
				{
					this.methods = new MethodDataCollection(this);
				}
				return this.methods;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004D7E File Offset: 0x00003D7E
		public ManagementObjectCollection GetInstances()
		{
			return this.GetInstances(null);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004D88 File Offset: 0x00003D88
		public ManagementObjectCollection GetInstances(EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone()));
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateInstanceEnum_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
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
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004E88 File Offset: 0x00003E88
		public void GetInstances(ManagementOperationObserver watcher)
		{
			this.GetInstances(watcher, null);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004E94 File Offset: 0x00003E94
		public void GetInstances(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			EnumerationOptions enumerationOptions = ((options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone()));
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateInstanceEnumAsync_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
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

		// Token: 0x06000085 RID: 133 RVA: 0x00004FB0 File Offset: 0x00003FB0
		public ManagementObjectCollection GetSubclasses()
		{
			return this.GetSubclasses(null);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004FBC File Offset: 0x00003FBC
		public ManagementObjectCollection GetSubclasses(EnumerationOptions options)
		{
			if (this.Path == null)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone()));
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateClassEnum_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
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
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000509C File Offset: 0x0000409C
		public void GetSubclasses(ManagementOperationObserver watcher)
		{
			this.GetSubclasses(watcher, null);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000050A8 File Offset: 0x000040A8
		public void GetSubclasses(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (this.Path == null)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			EnumerationOptions enumerationOptions = ((options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone()));
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateClassEnumAsync_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
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

		// Token: 0x06000089 RID: 137 RVA: 0x000051A8 File Offset: 0x000041A8
		public ManagementClass Derive(string newClassName)
		{
			ManagementClass managementClass = null;
			if (newClassName == null)
			{
				throw new ArgumentNullException("newClassName");
			}
			ManagementPath managementPath = new ManagementPath();
			try
			{
				managementPath.ClassName = newClassName;
			}
			catch
			{
				throw new ArgumentOutOfRangeException("newClassName");
			}
			if (!managementPath.IsClass)
			{
				throw new ArgumentOutOfRangeException("newClassName");
			}
			if (base.PutButNotGot)
			{
				base.Get();
				base.PutButNotGot = false;
			}
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			int num = base.wbemObject.SpawnDerivedClass_(0, out wbemClassObjectFreeThreaded);
			if (num >= 0)
			{
				object obj = newClassName;
				num = wbemClassObjectFreeThreaded.Put_("__CLASS", 0, ref obj, 0);
				if (num >= 0)
				{
					managementClass = ManagementClass.GetManagementClass(wbemClassObjectFreeThreaded, this);
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
			return managementClass;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000527C File Offset: 0x0000427C
		public ManagementObject CreateInstance()
		{
			ManagementObject managementObject = null;
			if (base.PutButNotGot)
			{
				base.Get();
				base.PutButNotGot = false;
			}
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			int num = base.wbemObject.SpawnInstance_(0, out wbemClassObjectFreeThreaded);
			if (num >= 0)
			{
				managementObject = ManagementObject.GetManagementObject(wbemClassObjectFreeThreaded, base.Scope);
			}
			else if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
			}
			else
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
			return managementObject;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000052F0 File Offset: 0x000042F0
		public override object Clone()
		{
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
			return ManagementClass.GetManagementClass(wbemClassObjectFreeThreaded, this);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005341 File Offset: 0x00004341
		public ManagementObjectCollection GetRelatedClasses()
		{
			return this.GetRelatedClasses(null);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000534A File Offset: 0x0000434A
		public ManagementObjectCollection GetRelatedClasses(string relatedClass)
		{
			return this.GetRelatedClasses(relatedClass, null, null, null, null, null, null);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000535C File Offset: 0x0000435C
		public ManagementObjectCollection GetRelatedClasses(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions());
			enumerationOptions.EnumerateDeep = true;
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(true, this.Path.Path, relatedClass, relationshipClass, relatedQualifier, relationshipQualifier, relatedRole, thisRole);
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQuery_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
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
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000547C File Offset: 0x0000447C
		public void GetRelatedClasses(ManagementOperationObserver watcher)
		{
			this.GetRelatedClasses(watcher, null);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005488 File Offset: 0x00004488
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass)
		{
			this.GetRelatedClasses(watcher, relatedClass, null, null, null, null, null, null);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000054A4 File Offset: 0x000044A4
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(true);
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			EnumerationOptions enumerationOptions = ((options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions());
			enumerationOptions.EnumerateDeep = true;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(true, this.Path.Path, relatedClass, relationshipClass, relatedQualifier, relationshipQualifier, relatedRole, thisRole);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQueryAsync_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
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

		// Token: 0x06000092 RID: 146 RVA: 0x000055E3 File Offset: 0x000045E3
		public ManagementObjectCollection GetRelationshipClasses()
		{
			return this.GetRelationshipClasses(null);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000055EC File Offset: 0x000045EC
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass)
		{
			return this.GetRelationshipClasses(relationshipClass, null, null, null);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000055F8 File Offset: 0x000045F8
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbemClassObject = null;
			EnumerationOptions enumerationOptions = ((options != null) ? options : new EnumerationOptions());
			enumerationOptions.EnumerateDeep = true;
			RelationshipQuery relationshipQuery = new RelationshipQuery(true, this.Path.Path, relationshipClass, relationshipQualifier, thisRole);
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQuery_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
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
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbemClassObject);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005708 File Offset: 0x00004708
		public void GetRelationshipClasses(ManagementOperationObserver watcher)
		{
			this.GetRelationshipClasses(watcher, null);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005712 File Offset: 0x00004712
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass)
		{
			this.GetRelationshipClasses(watcher, relationshipClass, null, null, null);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005720 File Offset: 0x00004720
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(true);
			EnumerationOptions enumerationOptions = ((options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions());
			enumerationOptions.EnumerateDeep = true;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			RelationshipQuery relationshipQuery = new RelationshipQuery(true, this.Path.Path, relationshipClass, relationshipQualifier, thisRole);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecurityIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQueryAsync_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
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

		// Token: 0x06000098 RID: 152 RVA: 0x0000585C File Offset: 0x0000485C
		public CodeTypeDeclaration GetStronglyTypedClassCode(bool includeSystemClassInClassDef, bool systemPropertyClass)
		{
			base.Get();
			ManagementClassGenerator managementClassGenerator = new ManagementClassGenerator(this);
			return managementClassGenerator.GenerateCode(includeSystemClassInClassDef, systemPropertyClass);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005880 File Offset: 0x00004880
		public bool GetStronglyTypedClassCode(CodeLanguage lang, string filePath, string classNamespace)
		{
			base.Get();
			ManagementClassGenerator managementClassGenerator = new ManagementClassGenerator(this);
			return managementClassGenerator.GenerateCode(lang, filePath, classNamespace);
		}

		// Token: 0x0400007A RID: 122
		private MethodDataCollection methods;
	}
}

using System;
using System.ComponentModel;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management
{
	// Token: 0x02000009 RID: 9
	[ToolboxItem(false)]
	[Serializable]
	public class ManagementBaseObject : Component, ICloneable, ISerializable
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000022B4 File Offset: 0x000012B4
		internal IWbemClassObjectFreeThreaded wbemObject
		{
			get
			{
				if (this._wbemObject == null)
				{
					this.Initialize(true);
				}
				return this._wbemObject;
			}
			set
			{
				this._wbemObject = value;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022C0 File Offset: 0x000012C0
		protected ManagementBaseObject(SerializationInfo info, StreamingContext context)
		{
			this._wbemObject = info.GetValue("wbemObject", typeof(IWbemClassObjectFreeThreaded)) as IWbemClassObjectFreeThreaded;
			if (this._wbemObject == null)
			{
				throw new SerializationException();
			}
			this.properties = null;
			this.systemProperties = null;
			this.qualifiers = null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002316 File Offset: 0x00001316
		public new void Dispose()
		{
			if (this._wbemObject != null)
			{
				this._wbemObject.Dispose();
				this._wbemObject = null;
			}
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000233E File Offset: 0x0000133E
		public static explicit operator IntPtr(ManagementBaseObject managementObject)
		{
			if (managementObject == null)
			{
				return IntPtr.Zero;
			}
			return managementObject.wbemObject;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002354 File Offset: 0x00001354
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("wbemObject", this.wbemObject, typeof(IWbemClassObjectFreeThreaded));
			info.AssemblyName = typeof(ManagementBaseObject).Assembly.FullName;
			info.FullTypeName = typeof(ManagementBaseObject).ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023AB File Offset: 0x000013AB
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			((ISerializable)this).GetObjectData(info, context);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023B8 File Offset: 0x000013B8
		internal static ManagementBaseObject GetBaseObject(IWbemClassObjectFreeThreaded wbemObject, ManagementScope scope)
		{
			ManagementBaseObject managementBaseObject;
			if (ManagementBaseObject._IsClass(wbemObject))
			{
				managementBaseObject = ManagementClass.GetManagementClass(wbemObject, scope);
			}
			else
			{
				managementBaseObject = ManagementObject.GetManagementObject(wbemObject, scope);
			}
			return managementBaseObject;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023E2 File Offset: 0x000013E2
		internal ManagementBaseObject(IWbemClassObjectFreeThreaded wbemObject)
		{
			this.wbemObject = wbemObject;
			this.properties = null;
			this.systemProperties = null;
			this.qualifiers = null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002408 File Offset: 0x00001408
		public virtual object Clone()
		{
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			int num = this.wbemObject.Clone_(out wbemClassObjectFreeThreaded);
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
			return new ManagementBaseObject(wbemClassObjectFreeThreaded);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002458 File Offset: 0x00001458
		internal virtual void Initialize(bool getObject)
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000245A File Offset: 0x0000145A
		public virtual PropertyDataCollection Properties
		{
			get
			{
				this.Initialize(true);
				if (this.properties == null)
				{
					this.properties = new PropertyDataCollection(this, false);
				}
				return this.properties;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000247E File Offset: 0x0000147E
		public virtual PropertyDataCollection SystemProperties
		{
			get
			{
				this.Initialize(false);
				if (this.systemProperties == null)
				{
					this.systemProperties = new PropertyDataCollection(this, true);
				}
				return this.systemProperties;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000024A2 File Offset: 0x000014A2
		public virtual QualifierDataCollection Qualifiers
		{
			get
			{
				this.Initialize(true);
				if (this.qualifiers == null)
				{
					this.qualifiers = new QualifierDataCollection(this);
				}
				return this.qualifiers;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000024C8 File Offset: 0x000014C8
		public virtual ManagementPath ClassPath
		{
			get
			{
				object obj = null;
				object obj2 = null;
				object obj3 = null;
				int num = 0;
				int num2 = 0;
				int num3 = this.wbemObject.Get_("__SERVER", 0, ref obj, ref num, ref num2);
				if (num3 == 0)
				{
					num3 = this.wbemObject.Get_("__NAMESPACE", 0, ref obj2, ref num, ref num2);
					if (num3 == 0)
					{
						num3 = this.wbemObject.Get_("__CLASS", 0, ref obj3, ref num, ref num2);
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

		// Token: 0x1700000A RID: 10
		public object this[string propertyName]
		{
			get
			{
				return this.GetPropertyValue(propertyName);
			}
			set
			{
				this.Initialize(true);
				try
				{
					this.SetPropertyValue(propertyName, value);
				}
				catch (COMException ex)
				{
					ManagementException.ThrowWithExtendedInfo(ex);
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002644 File Offset: 0x00001644
		public object GetPropertyValue(string propertyName)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (propertyName.StartsWith("__", StringComparison.Ordinal))
			{
				return this.SystemProperties[propertyName].Value;
			}
			return this.Properties[propertyName].Value;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002690 File Offset: 0x00001690
		public object GetQualifierValue(string qualifierName)
		{
			return this.Qualifiers[qualifierName].Value;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026A3 File Offset: 0x000016A3
		public void SetQualifierValue(string qualifierName, object qualifierValue)
		{
			this.Qualifiers[qualifierName].Value = qualifierValue;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026B7 File Offset: 0x000016B7
		public object GetPropertyQualifierValue(string propertyName, string qualifierName)
		{
			return this.Properties[propertyName].Qualifiers[qualifierName].Value;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026D5 File Offset: 0x000016D5
		public void SetPropertyQualifierValue(string propertyName, string qualifierName, object qualifierValue)
		{
			this.Properties[propertyName].Qualifiers[qualifierName].Value = qualifierValue;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026F4 File Offset: 0x000016F4
		public string GetText(TextFormat format)
		{
			string text = null;
			switch (format)
			{
			case TextFormat.Mof:
			{
				int num = this.wbemObject.GetObjectText_(0, out text);
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
				return text;
			}
			case TextFormat.CimDtd20:
			case TextFormat.WmiDtd20:
			{
				IWbemObjectTextSrc wbemObjectTextSrc = (IWbemObjectTextSrc)new WbemObjectTextSrc();
				IWbemContext wbemContext = (IWbemContext)new WbemContext();
				object obj = true;
				wbemContext.SetValue_("IncludeQualifiers", 0, ref obj);
				wbemContext.SetValue_("IncludeClassOrigin", 0, ref obj);
				if (wbemObjectTextSrc != null)
				{
					int num = wbemObjectTextSrc.GetText_(0, (IWbemClassObject_DoNotMarshal)Marshal.GetObjectForIUnknown(this.wbemObject), (uint)format, wbemContext, out text);
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
				return text;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027F0 File Offset: 0x000017F0
		public override bool Equals(object obj)
		{
			bool flag = false;
			try
			{
				if (!(obj is ManagementBaseObject))
				{
					return false;
				}
				flag = this.CompareTo((ManagementBaseObject)obj, ComparisonSettings.IncludeAll);
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode == ManagementStatus.NotFound && this is ManagementObject && obj is ManagementObject)
				{
					int num = string.Compare(((ManagementObject)this).Path.Path, ((ManagementObject)obj).Path.Path, StringComparison.OrdinalIgnoreCase);
					return num == 0;
				}
				return false;
			}
			catch
			{
				return false;
			}
			return flag;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002894 File Offset: 0x00001894
		public override int GetHashCode()
		{
			int num = 0;
			try
			{
				num = this.GetText(TextFormat.Mof).GetHashCode();
			}
			catch (ManagementException)
			{
				num = string.Empty.GetHashCode();
			}
			catch (COMException)
			{
				num = string.Empty.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028EC File Offset: 0x000018EC
		public bool CompareTo(ManagementBaseObject otherObject, ComparisonSettings settings)
		{
			if (otherObject == null)
			{
				throw new ArgumentNullException("otherObject");
			}
			bool flag = false;
			if (this.wbemObject != null)
			{
				int num = this.wbemObject.CompareTo_((int)settings, otherObject.wbemObject);
				if (262147 == num)
				{
					flag = false;
				}
				else if (num == 0)
				{
					flag = true;
				}
				else if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return flag;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002968 File Offset: 0x00001968
		internal string ClassName
		{
			get
			{
				object obj = null;
				int num = 0;
				int num2 = 0;
				int num3 = this.wbemObject.Get_("__CLASS", 0, ref obj, ref num, ref num2);
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
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return (string)obj;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029D8 File Offset: 0x000019D8
		private static bool _IsClass(IWbemClassObjectFreeThreaded wbemObject)
		{
			object obj = null;
			int num = 0;
			int num2 = 0;
			int num3 = wbemObject.Get_("__GENUS", 0, ref obj, ref num, ref num2);
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
			return (int)obj == 1;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002A34 File Offset: 0x00001A34
		internal bool IsClass
		{
			get
			{
				return ManagementBaseObject._IsClass(this.wbemObject);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A44 File Offset: 0x00001A44
		public void SetPropertyValue(string propertyName, object propertyValue)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (propertyName.StartsWith("__", StringComparison.Ordinal))
			{
				this.SystemProperties[propertyName].Value = propertyValue;
				return;
			}
			this.Properties[propertyName].Value = propertyValue;
		}

		// Token: 0x0400006D RID: 109
		private static WbemContext lockOnFastProx = (WMICapabilities.IsWindowsXPOrHigher() ? null : new WbemContext());

		// Token: 0x0400006E RID: 110
		internal IWbemClassObjectFreeThreaded _wbemObject;

		// Token: 0x0400006F RID: 111
		private PropertyDataCollection properties;

		// Token: 0x04000070 RID: 112
		private PropertyDataCollection systemProperties;

		// Token: 0x04000071 RID: 113
		private QualifierDataCollection qualifiers;
	}
}

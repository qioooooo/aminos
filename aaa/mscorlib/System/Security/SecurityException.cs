using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200067A RID: 1658
	[ComVisible(true)]
	[Serializable]
	public class SecurityException : SystemException
	{
		// Token: 0x06003C5A RID: 15450 RVA: 0x000CF379 File Offset: 0x000CE379
		internal static string GetResString(string sResourceName)
		{
			PermissionSet.s_fullTrust.Assert();
			return Environment.GetResourceString(sResourceName);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x000CF38C File Offset: 0x000CE38C
		internal static Exception MakeSecurityException(AssemblyName asmName, Evidence asmEvidence, PermissionSet granted, PermissionSet refused, RuntimeMethodHandle rmh, SecurityAction action, object demand, IPermission permThatFailed)
		{
			HostProtectionPermission hostProtectionPermission = permThatFailed as HostProtectionPermission;
			if (hostProtectionPermission != null)
			{
				return new HostProtectionException(SecurityException.GetResString("HostProtection_HostProtection"), HostProtectionPermission.protectedResources, hostProtectionPermission.Resources);
			}
			string text = "";
			MethodInfo methodInfo = null;
			try
			{
				if (granted == null && refused == null && demand == null)
				{
					text = SecurityException.GetResString("Security_NoAPTCA");
				}
				else if (demand != null && demand is IPermission)
				{
					text = string.Format(CultureInfo.InvariantCulture, SecurityException.GetResString("Security_Generic"), new object[] { demand.GetType().AssemblyQualifiedName });
				}
				else if (permThatFailed != null)
				{
					text = string.Format(CultureInfo.InvariantCulture, SecurityException.GetResString("Security_Generic"), new object[] { permThatFailed.GetType().AssemblyQualifiedName });
				}
				else
				{
					text = SecurityException.GetResString("Security_GenericNoType");
				}
				methodInfo = SecurityRuntime.GetMethodInfo(rmh);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
				{
					throw;
				}
			}
			return new SecurityException(text, asmName, granted, refused, methodInfo, action, demand, permThatFailed, asmEvidence);
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x000CF494 File Offset: 0x000CE494
		private static byte[] ObjectToByteArray(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			byte[] array2;
			try
			{
				binaryFormatter.Serialize(memoryStream, obj);
				byte[] array = memoryStream.ToArray();
				array2 = array;
			}
			catch (NotSupportedException)
			{
				array2 = null;
			}
			return array2;
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x000CF4DC File Offset: 0x000CE4DC
		private static object ByteArrayToObject(byte[] array)
		{
			if (array == null || array.Length == 0)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream(array);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x000CF509 File Offset: 0x000CE509
		public SecurityException()
			: base(SecurityException.GetResString("Arg_SecurityException"))
		{
			base.SetErrorCode(-2146233078);
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x000CF526 File Offset: 0x000CE526
		public SecurityException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233078);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000CF53A File Offset: 0x000CE53A
		public SecurityException(string message, Type type)
			: base(message)
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			this.m_typeOfPermissionThatFailed = type;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x000CF55F File Offset: 0x000CE55F
		public SecurityException(string message, Type type, string state)
			: base(message)
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			this.m_typeOfPermissionThatFailed = type;
			this.m_demanded = state;
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x000CF58B File Offset: 0x000CE58B
		public SecurityException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233078);
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x000CF5A0 File Offset: 0x000CE5A0
		internal SecurityException(PermissionSet grantedSetObj, PermissionSet refusedSetObj)
			: base(SecurityException.GetResString("Arg_SecurityException"))
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			if (grantedSetObj != null)
			{
				this.m_granted = grantedSetObj.ToXml().ToString();
			}
			if (refusedSetObj != null)
			{
				this.m_refused = refusedSetObj.ToXml().ToString();
			}
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x000CF5FC File Offset: 0x000CE5FC
		internal SecurityException(string message, PermissionSet grantedSetObj, PermissionSet refusedSetObj)
			: base(message)
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			if (grantedSetObj != null)
			{
				this.m_granted = grantedSetObj.ToXml().ToString();
			}
			if (refusedSetObj != null)
			{
				this.m_refused = refusedSetObj.ToXml().ToString();
			}
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x000CF650 File Offset: 0x000CE650
		protected SecurityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			try
			{
				this.m_action = (SecurityAction)info.GetValue("Action", typeof(SecurityAction));
				this.m_permissionThatFailed = (string)info.GetValueNoThrow("FirstPermissionThatFailed", typeof(string));
				this.m_demanded = (string)info.GetValueNoThrow("Demanded", typeof(string));
				this.m_granted = (string)info.GetValueNoThrow("GrantedSet", typeof(string));
				this.m_refused = (string)info.GetValueNoThrow("RefusedSet", typeof(string));
				this.m_denied = (string)info.GetValueNoThrow("Denied", typeof(string));
				this.m_permitOnly = (string)info.GetValueNoThrow("PermitOnly", typeof(string));
				this.m_assemblyName = (AssemblyName)info.GetValueNoThrow("Assembly", typeof(AssemblyName));
				this.m_serializedMethodInfo = (byte[])info.GetValueNoThrow("Method", typeof(byte[]));
				this.m_strMethodInfo = (string)info.GetValueNoThrow("Method_String", typeof(string));
				this.m_zone = (SecurityZone)info.GetValue("Zone", typeof(SecurityZone));
				this.m_url = (string)info.GetValueNoThrow("Url", typeof(string));
			}
			catch
			{
				this.m_action = (SecurityAction)0;
				this.m_permissionThatFailed = "";
				this.m_demanded = "";
				this.m_granted = "";
				this.m_refused = "";
				this.m_denied = "";
				this.m_permitOnly = "";
				this.m_assemblyName = null;
				this.m_serializedMethodInfo = null;
				this.m_strMethodInfo = null;
				this.m_zone = SecurityZone.NoZone;
				this.m_url = "";
			}
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x000CF884 File Offset: 0x000CE884
		public SecurityException(string message, AssemblyName assemblyName, PermissionSet grant, PermissionSet refused, MethodInfo method, SecurityAction action, object demanded, IPermission permThatFailed, Evidence evidence)
			: base(message)
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			this.Action = action;
			if (permThatFailed != null)
			{
				this.m_typeOfPermissionThatFailed = permThatFailed.GetType();
			}
			this.FirstPermissionThatFailed = permThatFailed;
			this.Demanded = demanded;
			this.m_granted = ((grant == null) ? "" : grant.ToXml().ToString());
			this.m_refused = ((refused == null) ? "" : refused.ToXml().ToString());
			this.m_denied = "";
			this.m_permitOnly = "";
			this.m_assemblyName = assemblyName;
			this.Method = method;
			this.m_url = "";
			this.m_zone = SecurityZone.NoZone;
			if (evidence != null)
			{
				Url url = (Url)evidence.FindType(typeof(Url));
				if (url != null)
				{
					this.m_url = url.GetURLString().ToString();
				}
				Zone zone = (Zone)evidence.FindType(typeof(Zone));
				if (zone != null)
				{
					this.m_zone = zone.SecurityZone;
				}
			}
			this.m_debugString = this.ToString(true, false);
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x000CF9A8 File Offset: 0x000CE9A8
		public SecurityException(string message, object deny, object permitOnly, MethodInfo method, object demanded, IPermission permThatFailed)
			: base(message)
		{
			PermissionSet.s_fullTrust.Assert();
			base.SetErrorCode(-2146233078);
			this.Action = SecurityAction.Demand;
			if (permThatFailed != null)
			{
				this.m_typeOfPermissionThatFailed = permThatFailed.GetType();
			}
			this.FirstPermissionThatFailed = permThatFailed;
			this.Demanded = demanded;
			this.m_granted = "";
			this.m_refused = "";
			this.DenySetInstance = deny;
			this.PermitOnlySetInstance = permitOnly;
			this.m_assemblyName = null;
			this.Method = method;
			this.m_zone = SecurityZone.NoZone;
			this.m_url = "";
			this.m_debugString = this.ToString(true, false);
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003C68 RID: 15464 RVA: 0x000CFA4C File Offset: 0x000CEA4C
		// (set) Token: 0x06003C69 RID: 15465 RVA: 0x000CFA54 File Offset: 0x000CEA54
		[ComVisible(false)]
		public SecurityAction Action
		{
			get
			{
				return this.m_action;
			}
			set
			{
				this.m_action = value;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06003C6A RID: 15466 RVA: 0x000CFA60 File Offset: 0x000CEA60
		// (set) Token: 0x06003C6B RID: 15467 RVA: 0x000CFAA5 File Offset: 0x000CEAA5
		public Type PermissionType
		{
			get
			{
				if (this.m_typeOfPermissionThatFailed == null)
				{
					object obj = XMLUtil.XmlStringToSecurityObject(this.m_permissionThatFailed);
					if (obj == null)
					{
						obj = XMLUtil.XmlStringToSecurityObject(this.m_demanded);
					}
					if (obj != null)
					{
						this.m_typeOfPermissionThatFailed = obj.GetType();
					}
				}
				return this.m_typeOfPermissionThatFailed;
			}
			set
			{
				this.m_typeOfPermissionThatFailed = value;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003C6C RID: 15468 RVA: 0x000CFAAE File Offset: 0x000CEAAE
		// (set) Token: 0x06003C6D RID: 15469 RVA: 0x000CFAC0 File Offset: 0x000CEAC0
		public IPermission FirstPermissionThatFailed
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return (IPermission)XMLUtil.XmlStringToSecurityObject(this.m_permissionThatFailed);
			}
			set
			{
				this.m_permissionThatFailed = XMLUtil.SecurityObjectToXmlString(value);
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x000CFACE File Offset: 0x000CEACE
		// (set) Token: 0x06003C6F RID: 15471 RVA: 0x000CFAD6 File Offset: 0x000CEAD6
		public string PermissionState
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.m_demanded;
			}
			set
			{
				this.m_demanded = value;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x000CFADF File Offset: 0x000CEADF
		// (set) Token: 0x06003C71 RID: 15473 RVA: 0x000CFAEC File Offset: 0x000CEAEC
		[ComVisible(false)]
		public object Demanded
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return XMLUtil.XmlStringToSecurityObject(this.m_demanded);
			}
			set
			{
				this.m_demanded = XMLUtil.SecurityObjectToXmlString(value);
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x000CFAFA File Offset: 0x000CEAFA
		// (set) Token: 0x06003C73 RID: 15475 RVA: 0x000CFB02 File Offset: 0x000CEB02
		public string GrantedSet
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.m_granted;
			}
			set
			{
				this.m_granted = value;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x000CFB0B File Offset: 0x000CEB0B
		// (set) Token: 0x06003C75 RID: 15477 RVA: 0x000CFB13 File Offset: 0x000CEB13
		public string RefusedSet
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.m_refused;
			}
			set
			{
				this.m_refused = value;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x000CFB1C File Offset: 0x000CEB1C
		// (set) Token: 0x06003C77 RID: 15479 RVA: 0x000CFB29 File Offset: 0x000CEB29
		[ComVisible(false)]
		public object DenySetInstance
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return XMLUtil.XmlStringToSecurityObject(this.m_denied);
			}
			set
			{
				this.m_denied = XMLUtil.SecurityObjectToXmlString(value);
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x000CFB37 File Offset: 0x000CEB37
		// (set) Token: 0x06003C79 RID: 15481 RVA: 0x000CFB44 File Offset: 0x000CEB44
		[ComVisible(false)]
		public object PermitOnlySetInstance
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return XMLUtil.XmlStringToSecurityObject(this.m_permitOnly);
			}
			set
			{
				this.m_permitOnly = XMLUtil.SecurityObjectToXmlString(value);
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06003C7A RID: 15482 RVA: 0x000CFB52 File Offset: 0x000CEB52
		// (set) Token: 0x06003C7B RID: 15483 RVA: 0x000CFB5A File Offset: 0x000CEB5A
		[ComVisible(false)]
		public AssemblyName FailedAssemblyInfo
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.m_assemblyName;
			}
			set
			{
				this.m_assemblyName = value;
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000CFB63 File Offset: 0x000CEB63
		private MethodInfo getMethod()
		{
			return (MethodInfo)SecurityException.ByteArrayToObject(this.m_serializedMethodInfo);
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000CFB75 File Offset: 0x000CEB75
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x000CFB80 File Offset: 0x000CEB80
		[ComVisible(false)]
		public MethodInfo Method
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.getMethod();
			}
			set
			{
				RuntimeMethodInfo runtimeMethodInfo = value as RuntimeMethodInfo;
				this.m_serializedMethodInfo = SecurityException.ObjectToByteArray(runtimeMethodInfo);
				if (runtimeMethodInfo != null)
				{
					this.m_strMethodInfo = runtimeMethodInfo.ToString();
				}
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003C7F RID: 15487 RVA: 0x000CFBAF File Offset: 0x000CEBAF
		// (set) Token: 0x06003C80 RID: 15488 RVA: 0x000CFBB7 File Offset: 0x000CEBB7
		public SecurityZone Zone
		{
			get
			{
				return this.m_zone;
			}
			set
			{
				this.m_zone = value;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003C81 RID: 15489 RVA: 0x000CFBC0 File Offset: 0x000CEBC0
		// (set) Token: 0x06003C82 RID: 15490 RVA: 0x000CFBC8 File Offset: 0x000CEBC8
		public string Url
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x000CFBD4 File Offset: 0x000CEBD4
		private void ToStringHelper(StringBuilder sb, string resourceString, object attr)
		{
			if (attr == null)
			{
				return;
			}
			string text = attr as string;
			if (text == null)
			{
				text = attr.ToString();
			}
			if (text.Length == 0)
			{
				return;
			}
			sb.Append(Environment.NewLine);
			sb.Append(SecurityException.GetResString(resourceString));
			sb.Append(Environment.NewLine);
			sb.Append(text);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x000CFC2C File Offset: 0x000CEC2C
		private string ToString(bool includeSensitiveInfo, bool includeBaseInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (includeBaseInfo)
			{
				try
				{
					stringBuilder.Append(base.ToString());
				}
				catch (SecurityException)
				{
				}
			}
			PermissionSet.s_fullTrust.Assert();
			if (this.Action > (SecurityAction)0)
			{
				this.ToStringHelper(stringBuilder, "Security_Action", this.Action);
			}
			this.ToStringHelper(stringBuilder, "Security_TypeFirstPermThatFailed", this.PermissionType);
			if (includeSensitiveInfo)
			{
				this.ToStringHelper(stringBuilder, "Security_FirstPermThatFailed", this.m_permissionThatFailed);
				this.ToStringHelper(stringBuilder, "Security_Demanded", this.m_demanded);
				this.ToStringHelper(stringBuilder, "Security_GrantedSet", this.m_granted);
				this.ToStringHelper(stringBuilder, "Security_RefusedSet", this.m_refused);
				this.ToStringHelper(stringBuilder, "Security_Denied", this.m_denied);
				this.ToStringHelper(stringBuilder, "Security_PermitOnly", this.m_permitOnly);
				this.ToStringHelper(stringBuilder, "Security_Assembly", this.m_assemblyName);
				this.ToStringHelper(stringBuilder, "Security_Method", this.m_strMethodInfo);
			}
			if (this.m_zone != SecurityZone.NoZone)
			{
				this.ToStringHelper(stringBuilder, "Security_Zone", this.m_zone);
			}
			if (includeSensitiveInfo)
			{
				this.ToStringHelper(stringBuilder, "Security_Url", this.m_url);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x000CFD74 File Offset: 0x000CED74
		private bool CanAccessSensitiveInfo()
		{
			bool flag = false;
			try
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy).Demand();
				flag = true;
			}
			catch (SecurityException)
			{
			}
			return flag;
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x000CFDA8 File Offset: 0x000CEDA8
		public override string ToString()
		{
			return this.ToString(this.CanAccessSensitiveInfo(), true);
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x000CFDB8 File Offset: 0x000CEDB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Action", this.m_action, typeof(SecurityAction));
			info.AddValue("FirstPermissionThatFailed", this.m_permissionThatFailed, typeof(string));
			info.AddValue("Demanded", this.m_demanded, typeof(string));
			info.AddValue("GrantedSet", this.m_granted, typeof(string));
			info.AddValue("RefusedSet", this.m_refused, typeof(string));
			info.AddValue("Denied", this.m_denied, typeof(string));
			info.AddValue("PermitOnly", this.m_permitOnly, typeof(string));
			info.AddValue("Assembly", this.m_assemblyName, typeof(AssemblyName));
			info.AddValue("Method", this.m_serializedMethodInfo, typeof(byte[]));
			info.AddValue("Method_String", this.m_strMethodInfo, typeof(string));
			info.AddValue("Zone", this.m_zone, typeof(SecurityZone));
			info.AddValue("Url", this.m_url, typeof(string));
		}

		// Token: 0x04001ED9 RID: 7897
		private const string ActionName = "Action";

		// Token: 0x04001EDA RID: 7898
		private const string FirstPermissionThatFailedName = "FirstPermissionThatFailed";

		// Token: 0x04001EDB RID: 7899
		private const string DemandedName = "Demanded";

		// Token: 0x04001EDC RID: 7900
		private const string GrantedSetName = "GrantedSet";

		// Token: 0x04001EDD RID: 7901
		private const string RefusedSetName = "RefusedSet";

		// Token: 0x04001EDE RID: 7902
		private const string DeniedName = "Denied";

		// Token: 0x04001EDF RID: 7903
		private const string PermitOnlyName = "PermitOnly";

		// Token: 0x04001EE0 RID: 7904
		private const string Assembly_Name = "Assembly";

		// Token: 0x04001EE1 RID: 7905
		private const string MethodName_Serialized = "Method";

		// Token: 0x04001EE2 RID: 7906
		private const string MethodName_String = "Method_String";

		// Token: 0x04001EE3 RID: 7907
		private const string ZoneName = "Zone";

		// Token: 0x04001EE4 RID: 7908
		private const string UrlName = "Url";

		// Token: 0x04001EE5 RID: 7909
		private string m_debugString;

		// Token: 0x04001EE6 RID: 7910
		private SecurityAction m_action;

		// Token: 0x04001EE7 RID: 7911
		[NonSerialized]
		private Type m_typeOfPermissionThatFailed;

		// Token: 0x04001EE8 RID: 7912
		private string m_permissionThatFailed;

		// Token: 0x04001EE9 RID: 7913
		private string m_demanded;

		// Token: 0x04001EEA RID: 7914
		private string m_granted;

		// Token: 0x04001EEB RID: 7915
		private string m_refused;

		// Token: 0x04001EEC RID: 7916
		private string m_denied;

		// Token: 0x04001EED RID: 7917
		private string m_permitOnly;

		// Token: 0x04001EEE RID: 7918
		private AssemblyName m_assemblyName;

		// Token: 0x04001EEF RID: 7919
		private byte[] m_serializedMethodInfo;

		// Token: 0x04001EF0 RID: 7920
		private string m_strMethodInfo;

		// Token: 0x04001EF1 RID: 7921
		private SecurityZone m_zone;

		// Token: 0x04001EF2 RID: 7922
		private string m_url;
	}
}

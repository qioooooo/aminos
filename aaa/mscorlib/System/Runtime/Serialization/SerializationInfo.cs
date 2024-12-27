using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x02000362 RID: 866
	[ComVisible(true)]
	public sealed class SerializationInfo
	{
		// Token: 0x0600225D RID: 8797 RVA: 0x00057486 File Offset: 0x00056486
		[CLSCompliant(false)]
		public SerializationInfo(Type type, IFormatterConverter converter)
			: this(type, converter, false)
		{
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00057494 File Offset: 0x00056494
		[CLSCompliant(false)]
		public SerializationInfo(Type type, IFormatterConverter converter, bool requireSameTokenInPartialTrust)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			this.objectType = type;
			this.m_fullTypeName = type.FullName;
			this.m_assemName = type.Module.Assembly.FullName;
			this.m_members = new string[4];
			this.m_data = new object[4];
			this.m_types = new Type[4];
			this.m_converter = converter;
			this.m_currMember = 0;
			this.requireSameTokenInPartialTrust = requireSameTokenInPartialTrust;
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x00057525 File Offset: 0x00056525
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x0005752D File Offset: 0x0005652D
		public string FullTypeName
		{
			get
			{
				return this.m_fullTypeName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_fullTypeName = value;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x00057544 File Offset: 0x00056544
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x0005754C File Offset: 0x0005654C
		public string AssemblyName
		{
			get
			{
				return this.m_assemName;
			}
			[SecuritySafeCritical]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.requireSameTokenInPartialTrust)
				{
					SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.m_assemName, value);
				}
				this.m_assemName = value;
			}
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00057578 File Offset: 0x00056578
		[SecuritySafeCritical]
		public void SetType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this.requireSameTokenInPartialTrust)
			{
				SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.objectType.Assembly.FullName, type.Assembly.FullName);
			}
			if (!object.ReferenceEquals(this.objectType, type))
			{
				this.objectType = type;
				this.m_fullTypeName = type.FullName;
				this.m_assemName = type.Module.Assembly.FullName;
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000575F4 File Offset: 0x000565F4
		private static bool Compare(byte[] a, byte[] b)
		{
			if (a == null || b == null || a.Length == 0 || b.Length == 0 || a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00057634 File Offset: 0x00056634
		[SecuritySafeCritical]
		internal static void DemandForUnsafeAssemblyNameAssignments(string originalAssemblyName, string newAssemblyName)
		{
			if (!SerializationInfo.IsAssemblyNameAssignmentSafe(originalAssemblyName, newAssemblyName))
			{
				CodeAccessPermission.DemandInternal(PermissionType.SecuritySerialization);
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x00057648 File Offset: 0x00056648
		internal static bool IsAssemblyNameAssignmentSafe(string originalAssemblyName, string newAssemblyName)
		{
			if (originalAssemblyName == newAssemblyName)
			{
				return true;
			}
			AssemblyName assemblyName = new AssemblyName(originalAssemblyName);
			AssemblyName assemblyName2 = new AssemblyName(newAssemblyName);
			return !string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) && !string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) && SerializationInfo.Compare(assemblyName.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x000576A7 File Offset: 0x000566A7
		public int MemberCount
		{
			get
			{
				return this.m_currMember;
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000576AF File Offset: 0x000566AF
		public SerializationInfoEnumerator GetEnumerator()
		{
			return new SerializationInfoEnumerator(this.m_members, this.m_data, this.m_types, this.m_currMember);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000576D0 File Offset: 0x000566D0
		private void ExpandArrays()
		{
			int num = this.m_currMember * 2;
			if (num < this.m_currMember && 2147483647 > this.m_currMember)
			{
				num = int.MaxValue;
			}
			string[] array = new string[num];
			object[] array2 = new object[num];
			Type[] array3 = new Type[num];
			Array.Copy(this.m_members, array, this.m_currMember);
			Array.Copy(this.m_data, array2, this.m_currMember);
			Array.Copy(this.m_types, array3, this.m_currMember);
			this.m_members = array;
			this.m_data = array2;
			this.m_types = array3;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00057764 File Offset: 0x00056764
		public void AddValue(string name, object value, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (int i = 0; i < this.m_currMember; i++)
			{
				if (this.m_members[i].Equals(name))
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_SameNameTwice"));
				}
			}
			this.AddValue(name, value, type, this.m_currMember);
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000577CD File Offset: 0x000567CD
		public void AddValue(string name, object value)
		{
			if (value == null)
			{
				this.AddValue(name, value, typeof(object));
				return;
			}
			this.AddValue(name, value, value.GetType());
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000577F3 File Offset: 0x000567F3
		public void AddValue(string name, bool value)
		{
			this.AddValue(name, value, typeof(bool));
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x0005780C File Offset: 0x0005680C
		public void AddValue(string name, char value)
		{
			this.AddValue(name, value, typeof(char));
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x00057825 File Offset: 0x00056825
		[CLSCompliant(false)]
		public void AddValue(string name, sbyte value)
		{
			this.AddValue(name, value, typeof(sbyte));
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0005783E File Offset: 0x0005683E
		public void AddValue(string name, byte value)
		{
			this.AddValue(name, value, typeof(byte));
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00057857 File Offset: 0x00056857
		public void AddValue(string name, short value)
		{
			this.AddValue(name, value, typeof(short));
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x00057870 File Offset: 0x00056870
		[CLSCompliant(false)]
		public void AddValue(string name, ushort value)
		{
			this.AddValue(name, value, typeof(ushort));
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00057889 File Offset: 0x00056889
		public void AddValue(string name, int value)
		{
			this.AddValue(name, value, typeof(int));
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000578A2 File Offset: 0x000568A2
		[CLSCompliant(false)]
		public void AddValue(string name, uint value)
		{
			this.AddValue(name, value, typeof(uint));
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000578BB File Offset: 0x000568BB
		public void AddValue(string name, long value)
		{
			this.AddValue(name, value, typeof(long));
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000578D4 File Offset: 0x000568D4
		[CLSCompliant(false)]
		public void AddValue(string name, ulong value)
		{
			this.AddValue(name, value, typeof(ulong));
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000578ED File Offset: 0x000568ED
		public void AddValue(string name, float value)
		{
			this.AddValue(name, value, typeof(float));
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00057906 File Offset: 0x00056906
		public void AddValue(string name, double value)
		{
			this.AddValue(name, value, typeof(double));
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x0005791F File Offset: 0x0005691F
		public void AddValue(string name, decimal value)
		{
			this.AddValue(name, value, typeof(decimal));
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00057938 File Offset: 0x00056938
		public void AddValue(string name, DateTime value)
		{
			this.AddValue(name, value, typeof(DateTime));
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00057951 File Offset: 0x00056951
		internal void AddValue(string name, object value, Type type, int index)
		{
			if (index >= this.m_members.Length)
			{
				this.ExpandArrays();
			}
			this.m_members[index] = name;
			this.m_data[index] = value;
			this.m_types[index] = type;
			this.m_currMember++;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00057994 File Offset: 0x00056994
		internal void UpdateValue(string name, object value, Type type)
		{
			int num = this.FindElement(name);
			if (num < 0)
			{
				this.AddValue(name, value, type, this.m_currMember);
				return;
			}
			this.m_members[num] = name;
			this.m_data[num] = value;
			this.m_types[num] = type;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000579D8 File Offset: 0x000569D8
		private int FindElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			for (int i = 0; i < this.m_currMember; i++)
			{
				if (this.m_members[i].Equals(name))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00057A18 File Offset: 0x00056A18
		private object GetElement(string name, out Type foundType)
		{
			int num = this.FindElement(name);
			if (num == -1)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NotFound"), new object[] { name }));
			}
			foundType = this.m_types[num];
			return this.m_data[num];
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00057A6C File Offset: 0x00056A6C
		[ComVisible(true)]
		private object GetElementNoThrow(string name, out Type foundType)
		{
			int num = this.FindElement(name);
			if (num == -1)
			{
				foundType = null;
				return null;
			}
			foundType = this.m_types[num];
			return this.m_data[num];
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00057A9C File Offset: 0x00056A9C
		public object GetValue(string name, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type type2;
			object element = this.GetElement(name, out type2);
			if (RemotingServices.IsTransparentProxy(element))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(element);
				if (RemotingServices.ProxyCheckCast(realProxy, type))
				{
					return element;
				}
			}
			else if (type2 == type || type.IsAssignableFrom(type2) || element == null)
			{
				return element;
			}
			return this.m_converter.Convert(element, type);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x00057AFC File Offset: 0x00056AFC
		[ComVisible(true)]
		internal object GetValueNoThrow(string name, Type type)
		{
			Type type2;
			object elementNoThrow = this.GetElementNoThrow(name, out type2);
			if (elementNoThrow == null)
			{
				return null;
			}
			if (RemotingServices.IsTransparentProxy(elementNoThrow))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(elementNoThrow);
				if (RemotingServices.ProxyCheckCast(realProxy, type))
				{
					return elementNoThrow;
				}
			}
			else if (type2 == type || type.IsAssignableFrom(type2) || elementNoThrow == null)
			{
				return elementNoThrow;
			}
			return this.m_converter.Convert(elementNoThrow, type);
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00057B54 File Offset: 0x00056B54
		public bool GetBoolean(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(bool))
			{
				return (bool)element;
			}
			return this.m_converter.ToBoolean(element);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00057B8C File Offset: 0x00056B8C
		public char GetChar(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(char))
			{
				return (char)element;
			}
			return this.m_converter.ToChar(element);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00057BC4 File Offset: 0x00056BC4
		[CLSCompliant(false)]
		public sbyte GetSByte(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(sbyte))
			{
				return (sbyte)element;
			}
			return this.m_converter.ToSByte(element);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00057BFC File Offset: 0x00056BFC
		public byte GetByte(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(byte))
			{
				return (byte)element;
			}
			return this.m_converter.ToByte(element);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00057C34 File Offset: 0x00056C34
		public short GetInt16(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(short))
			{
				return (short)element;
			}
			return this.m_converter.ToInt16(element);
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00057C6C File Offset: 0x00056C6C
		[CLSCompliant(false)]
		public ushort GetUInt16(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(ushort))
			{
				return (ushort)element;
			}
			return this.m_converter.ToUInt16(element);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00057CA4 File Offset: 0x00056CA4
		public int GetInt32(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(int))
			{
				return (int)element;
			}
			return this.m_converter.ToInt32(element);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00057CDC File Offset: 0x00056CDC
		[CLSCompliant(false)]
		public uint GetUInt32(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(uint))
			{
				return (uint)element;
			}
			return this.m_converter.ToUInt32(element);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00057D14 File Offset: 0x00056D14
		public long GetInt64(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(long))
			{
				return (long)element;
			}
			return this.m_converter.ToInt64(element);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00057D4C File Offset: 0x00056D4C
		[CLSCompliant(false)]
		public ulong GetUInt64(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(ulong))
			{
				return (ulong)element;
			}
			return this.m_converter.ToUInt64(element);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00057D84 File Offset: 0x00056D84
		public float GetSingle(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(float))
			{
				return (float)element;
			}
			return this.m_converter.ToSingle(element);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00057DBC File Offset: 0x00056DBC
		public double GetDouble(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(double))
			{
				return (double)element;
			}
			return this.m_converter.ToDouble(element);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00057DF4 File Offset: 0x00056DF4
		public decimal GetDecimal(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(decimal))
			{
				return (decimal)element;
			}
			return this.m_converter.ToDecimal(element);
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00057E2C File Offset: 0x00056E2C
		public DateTime GetDateTime(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(DateTime))
			{
				return (DateTime)element;
			}
			return this.m_converter.ToDateTime(element);
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00057E64 File Offset: 0x00056E64
		public string GetString(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(string) || element == null)
			{
				return (string)element;
			}
			return this.m_converter.ToString(element);
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x00057E9E File Offset: 0x00056E9E
		internal string[] MemberNames
		{
			get
			{
				return this.m_members;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x00057EA6 File Offset: 0x00056EA6
		internal object[] MemberValues
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x04000E48 RID: 3656
		private const int defaultSize = 4;

		// Token: 0x04000E49 RID: 3657
		private const string s_mscorlibAssemblySimpleName = "mscorlib";

		// Token: 0x04000E4A RID: 3658
		private const string s_mscorlibFileName = "mscorlib.dll";

		// Token: 0x04000E4B RID: 3659
		internal string[] m_members;

		// Token: 0x04000E4C RID: 3660
		internal object[] m_data;

		// Token: 0x04000E4D RID: 3661
		internal Type[] m_types;

		// Token: 0x04000E4E RID: 3662
		internal string m_fullTypeName;

		// Token: 0x04000E4F RID: 3663
		internal int m_currMember;

		// Token: 0x04000E50 RID: 3664
		internal string m_assemName;

		// Token: 0x04000E51 RID: 3665
		private Type objectType;

		// Token: 0x04000E52 RID: 3666
		internal IFormatterConverter m_converter;

		// Token: 0x04000E53 RID: 3667
		private bool requireSameTokenInPartialTrust;
	}
}

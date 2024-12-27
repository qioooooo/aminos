using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x0200045E RID: 1118
	[ComVisible(true)]
	public sealed class RegistryKey : MarshalByRefObject, IDisposable
	{
		// Token: 0x06002D03 RID: 11523 RVA: 0x00097072 File Offset: 0x00096072
		private RegistryKey(SafeRegistryHandle hkey, bool writable)
			: this(hkey, writable, false, false, false)
		{
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x00097080 File Offset: 0x00096080
		private RegistryKey(SafeRegistryHandle hkey, bool writable, bool systemkey, bool remoteKey, bool isPerfData)
		{
			this.hkey = hkey;
			this.keyName = "";
			this.remoteKey = remoteKey;
			if (systemkey)
			{
				this.state |= 2;
			}
			if (writable)
			{
				this.state |= 4;
			}
			if (isPerfData)
			{
				this.state |= 8;
			}
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000970E1 File Offset: 0x000960E1
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000970EC File Offset: 0x000960EC
		private void Dispose(bool disposing)
		{
			if (this.hkey != null)
			{
				bool flag = this.IsPerfDataKey();
				if (this.IsSystemKey())
				{
					if (!flag)
					{
						return;
					}
				}
				try
				{
					this.hkey.Dispose();
				}
				catch (IOException)
				{
				}
				if (flag)
				{
					this.hkey = new SafeRegistryHandle(RegistryKey.HKEY_PERFORMANCE_DATA, !RegistryKey.IsWin9x());
					return;
				}
				this.hkey = null;
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x00097158 File Offset: 0x00096158
		public void Flush()
		{
			if (this.hkey != null && this.IsDirty())
			{
				Win32Native.RegFlushKey(this.hkey);
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x00097176 File Offset: 0x00096176
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x0009717F File Offset: 0x0009617F
		public RegistryKey CreateSubKey(string subkey)
		{
			return this.CreateSubKey(subkey, this.checkMode);
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x0009718E File Offset: 0x0009618E
		[ComVisible(false)]
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
		{
			return this.CreateSubKey(subkey, permissionCheck, null);
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x0009719C File Offset: 0x0009619C
		[ComVisible(false)]
		public unsafe RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
		{
			RegistryKey.ValidateKeyName(subkey);
			RegistryKey.ValidateKeyMode(permissionCheck);
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			if (!this.remoteKey)
			{
				RegistryKey registryKey = this.InternalOpenSubKey(subkey, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree);
				if (registryKey != null)
				{
					this.CheckSubKeyWritePermission(subkey);
					this.CheckSubTreePermission(subkey, permissionCheck);
					registryKey.checkMode = permissionCheck;
					return registryKey;
				}
			}
			this.CheckSubKeyCreatePermission(subkey);
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if (registrySecurity != null)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				byte[] securityDescriptorBinaryForm = registrySecurity.GetSecurityDescriptorBinaryForm();
				byte* ptr = stackalloc byte[1 * securityDescriptorBinaryForm.Length];
				Buffer.memcpy(securityDescriptorBinaryForm, 0, ptr, 0, securityDescriptorBinaryForm.Length);
				security_ATTRIBUTES.pSecurityDescriptor = ptr;
			}
			int num = 0;
			SafeRegistryHandle safeRegistryHandle = null;
			int num2 = Win32Native.RegCreateKeyEx(this.hkey, subkey, 0, null, 0, RegistryKey.GetRegistryKeyAccess(permissionCheck != RegistryKeyPermissionCheck.ReadSubTree), security_ATTRIBUTES, out safeRegistryHandle, out num);
			if (num2 == 0 && !safeRegistryHandle.IsInvalid)
			{
				RegistryKey registryKey2 = new RegistryKey(safeRegistryHandle, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, false, this.remoteKey, false);
				this.CheckSubTreePermission(subkey, permissionCheck);
				registryKey2.checkMode = permissionCheck;
				if (subkey.Length == 0)
				{
					registryKey2.keyName = this.keyName;
				}
				else
				{
					registryKey2.keyName = this.keyName + "\\" + subkey;
				}
				return registryKey2;
			}
			if (num2 != 0)
			{
				this.Win32Error(num2, this.keyName + "\\" + subkey);
			}
			return null;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000972E2 File Offset: 0x000962E2
		public void DeleteSubKey(string subkey)
		{
			this.DeleteSubKey(subkey, true);
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000972EC File Offset: 0x000962EC
		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			RegistryKey.ValidateKeyName(subkey);
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			this.CheckSubKeyWritePermission(subkey);
			RegistryKey registryKey = this.InternalOpenSubKey(subkey, false);
			if (registryKey != null)
			{
				try
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_RegRemoveSubKey);
					}
				}
				finally
				{
					registryKey.Close();
				}
				int num = Win32Native.RegDeleteKey(this.hkey, subkey);
				if (num != 0)
				{
					if (num != 2)
					{
						this.Win32Error(num, null);
						return;
					}
					if (throwOnMissingSubKey)
					{
						ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSubKeyAbsent);
						return;
					}
				}
			}
			else if (throwOnMissingSubKey)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSubKeyAbsent);
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0009737C File Offset: 0x0009637C
		public void DeleteSubKeyTree(string subkey)
		{
			RegistryKey.ValidateKeyName(subkey);
			if (subkey.Length == 0 && this.IsSystemKey())
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegKeyDelHive);
			}
			this.EnsureWriteable();
			subkey = RegistryKey.FixupName(subkey);
			this.CheckSubTreeWritePermission(subkey);
			RegistryKey registryKey = this.InternalOpenSubKey(subkey, true);
			if (registryKey != null)
			{
				try
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						string[] array = registryKey.InternalGetSubKeyNames();
						for (int i = 0; i < array.Length; i++)
						{
							registryKey.DeleteSubKeyTreeInternal(array[i]);
						}
					}
				}
				finally
				{
					registryKey.Close();
				}
				int num = Win32Native.RegDeleteKey(this.hkey, subkey);
				if (num != 0)
				{
					this.Win32Error(num, null);
					return;
				}
			}
			else
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSubKeyAbsent);
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x00097428 File Offset: 0x00096428
		private void DeleteSubKeyTreeInternal(string subkey)
		{
			RegistryKey registryKey = this.InternalOpenSubKey(subkey, true);
			if (registryKey != null)
			{
				try
				{
					if (registryKey.InternalSubKeyCount() > 0)
					{
						string[] array = registryKey.InternalGetSubKeyNames();
						for (int i = 0; i < array.Length; i++)
						{
							registryKey.DeleteSubKeyTreeInternal(array[i]);
						}
					}
				}
				finally
				{
					registryKey.Close();
				}
				int num = Win32Native.RegDeleteKey(this.hkey, subkey);
				if (num != 0)
				{
					this.Win32Error(num, null);
					return;
				}
			}
			else
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSubKeyAbsent);
			}
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000974A4 File Offset: 0x000964A4
		public void DeleteValue(string name)
		{
			this.DeleteValue(name, true);
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000974B0 File Offset: 0x000964B0
		public void DeleteValue(string name, bool throwOnMissingValue)
		{
			if (name == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.name);
			}
			this.EnsureWriteable();
			this.CheckValueWritePermission(name);
			int num = Win32Native.RegDeleteValue(this.hkey, name);
			if ((num == 2 || num == 206) && throwOnMissingValue)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSubKeyValueAbsent);
			}
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000974F8 File Offset: 0x000964F8
		internal static RegistryKey GetBaseKey(IntPtr hKey)
		{
			int num = (int)hKey & 268435455;
			bool flag = hKey == RegistryKey.HKEY_PERFORMANCE_DATA;
			SafeRegistryHandle safeRegistryHandle = new SafeRegistryHandle(hKey, flag && !RegistryKey.IsWin9x());
			return new RegistryKey(safeRegistryHandle, true, true, false, flag)
			{
				checkMode = RegistryKeyPermissionCheck.Default,
				keyName = RegistryKey.hkeyNames[num]
			};
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x00097554 File Offset: 0x00096554
		public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			int num = (int)(hKey & (RegistryHive)268435455);
			if (num < 0 || num >= RegistryKey.hkeyNames.Length || ((long)hKey & (long)((ulong)(-16))) != (long)((ulong)(-2147483648)))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RegKeyOutOfRange"));
			}
			RegistryKey.CheckUnmanagedCodePermission();
			SafeRegistryHandle safeRegistryHandle = null;
			int num2 = Win32Native.RegConnectRegistry(machineName, new SafeRegistryHandle(new IntPtr((int)hKey), false), out safeRegistryHandle);
			if (num2 == 1114)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_DllInitFailure"));
			}
			if (num2 != 0)
			{
				RegistryKey.Win32ErrorStatic(num2, null);
			}
			if (safeRegistryHandle.IsInvalid)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RegKeyNoRemoteConnect", new object[] { machineName }));
			}
			return new RegistryKey(safeRegistryHandle, true, false, true, (IntPtr)((long)hKey) == RegistryKey.HKEY_PERFORMANCE_DATA)
			{
				checkMode = RegistryKeyPermissionCheck.Default,
				keyName = RegistryKey.hkeyNames[num]
			};
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x00097638 File Offset: 0x00096638
		public RegistryKey OpenSubKey(string name, bool writable)
		{
			RegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			name = RegistryKey.FixupName(name);
			this.CheckOpenSubKeyPermission(name, writable);
			SafeRegistryHandle safeRegistryHandle = null;
			int num = Win32Native.RegOpenKeyEx(this.hkey, name, 0, RegistryKey.GetRegistryKeyAccess(writable), out safeRegistryHandle);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, writable, false, this.remoteKey, false)
				{
					checkMode = this.GetSubKeyPermissonCheck(writable),
					keyName = this.keyName + "\\" + name
				};
			}
			if (num == 5 || num == 1346)
			{
				ThrowHelper.ThrowSecurityException(ExceptionResource.Security_RegistryPermission);
			}
			return null;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000976CE File Offset: 0x000966CE
		[ComVisible(false)]
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
		{
			RegistryKey.ValidateKeyMode(permissionCheck);
			return this.InternalOpenSubKey(name, permissionCheck, RegistryKey.GetRegistryKeyAccess(permissionCheck));
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000976E4 File Offset: 0x000966E4
		[ComVisible(false)]
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
		{
			return this.InternalOpenSubKey(name, permissionCheck, (int)rights);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000976F0 File Offset: 0x000966F0
		private RegistryKey InternalOpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, int rights)
		{
			RegistryKey.ValidateKeyName(name);
			RegistryKey.ValidateKeyMode(permissionCheck);
			RegistryKey.ValidateKeyRights(rights);
			this.EnsureNotDisposed();
			name = RegistryKey.FixupName(name);
			this.CheckOpenSubKeyPermission(name, permissionCheck);
			SafeRegistryHandle safeRegistryHandle = null;
			int num = Win32Native.RegOpenKeyEx(this.hkey, name, 0, rights, out safeRegistryHandle);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, permissionCheck == RegistryKeyPermissionCheck.ReadWriteSubTree, false, this.remoteKey, false)
				{
					keyName = this.keyName + "\\" + name,
					checkMode = permissionCheck
				};
			}
			if (num == 5 || num == 1346)
			{
				ThrowHelper.ThrowSecurityException(ExceptionResource.Security_RegistryPermission);
			}
			return null;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x0009778C File Offset: 0x0009678C
		internal RegistryKey InternalOpenSubKey(string name, bool writable)
		{
			RegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			int registryKeyAccess = RegistryKey.GetRegistryKeyAccess(writable);
			SafeRegistryHandle safeRegistryHandle = null;
			if (Win32Native.RegOpenKeyEx(this.hkey, name, 0, registryKeyAccess, out safeRegistryHandle) == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new RegistryKey(safeRegistryHandle, writable, false, this.remoteKey, false)
				{
					keyName = this.keyName + "\\" + name
				};
			}
			return null;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000977F4 File Offset: 0x000967F4
		public RegistryKey OpenSubKey(string name)
		{
			return this.OpenSubKey(name, false);
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x000977FE File Offset: 0x000967FE
		public int SubKeyCount
		{
			get
			{
				this.CheckKeyReadPermission();
				return this.InternalSubKeyCount();
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0009780C File Offset: 0x0009680C
		internal int InternalSubKeyCount()
		{
			this.EnsureNotDisposed();
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryInfoKey(this.hkey, null, null, Win32Native.NULL, ref num, null, null, ref num2, null, null, null, null);
			if (num3 != 0)
			{
				this.Win32Error(num3, null);
			}
			return num;
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0009784C File Offset: 0x0009684C
		public string[] GetSubKeyNames()
		{
			this.CheckKeyReadPermission();
			return this.InternalGetSubKeyNames();
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x0009785C File Offset: 0x0009685C
		internal string[] InternalGetSubKeyNames()
		{
			this.EnsureNotDisposed();
			int num = this.InternalSubKeyCount();
			string[] array = new string[num];
			if (num > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < num; i++)
				{
					int capacity = stringBuilder.Capacity;
					int num2 = Win32Native.RegEnumKeyEx(this.hkey, i, stringBuilder, out capacity, null, null, null, null);
					if (num2 != 0)
					{
						this.Win32Error(num2, null);
					}
					array[i] = stringBuilder.ToString();
				}
			}
			return array;
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000978D1 File Offset: 0x000968D1
		public int ValueCount
		{
			get
			{
				this.CheckKeyReadPermission();
				return this.InternalValueCount();
			}
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000978E0 File Offset: 0x000968E0
		internal int InternalValueCount()
		{
			this.EnsureNotDisposed();
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryInfoKey(this.hkey, null, null, Win32Native.NULL, ref num2, null, null, ref num, null, null, null, null);
			if (num3 != 0)
			{
				this.Win32Error(num3, null);
			}
			return num;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x00097920 File Offset: 0x00096920
		public string[] GetValueNames()
		{
			this.CheckKeyReadPermission();
			this.EnsureNotDisposed();
			int num = this.InternalValueCount();
			string[] array = new string[num];
			if (num > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < num; i++)
				{
					int capacity = stringBuilder.Capacity;
					int num2 = Win32Native.RegEnumValue(this.hkey, i, stringBuilder, ref capacity, Win32Native.NULL, null, null, null);
					if (num2 == 234 && !this.IsPerfDataKey() && this.remoteKey)
					{
						int[] array2 = new int[1];
						byte[] array3 = new byte[5];
						array2[0] = 5;
						num2 = Win32Native.RegEnumValueA(this.hkey, i, stringBuilder, ref capacity, Win32Native.NULL, null, array3, array2);
						if (num2 == 234)
						{
							array2[0] = 0;
							num2 = Win32Native.RegEnumValueA(this.hkey, i, stringBuilder, ref capacity, Win32Native.NULL, null, null, array2);
						}
					}
					if (num2 != 0 && (!this.IsPerfDataKey() || num2 != 234))
					{
						this.Win32Error(num2, null);
					}
					array[i] = stringBuilder.ToString();
				}
			}
			return array;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x00097A2C File Offset: 0x00096A2C
		public object GetValue(string name)
		{
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, null, false, true);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x00097A3F File Offset: 0x00096A3F
		public object GetValue(string name, object defaultValue)
		{
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, defaultValue, false, true);
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x00097A54 File Offset: 0x00096A54
		[ComVisible(false)]
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			if (options < RegistryValueOptions.None || options > RegistryValueOptions.DoNotExpandEnvironmentNames)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)options }), "options");
			}
			bool flag = options == RegistryValueOptions.DoNotExpandEnvironmentNames;
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, defaultValue, flag, true);
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x00097AA8 File Offset: 0x00096AA8
		internal object InternalGetValue(string name, object defaultValue, bool doNotExpand, bool checkSecurity)
		{
			if (checkSecurity)
			{
				this.EnsureNotDisposed();
			}
			object obj = defaultValue;
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			if (num3 != 0)
			{
				if (this.IsPerfDataKey())
				{
					int num4 = 65000;
					int num5 = num4;
					byte[] array = new byte[num4];
					int num6;
					while (234 == (num6 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array, ref num5)))
					{
						num4 *= 2;
						num5 = num4;
						array = new byte[num4];
					}
					if (num6 != 0)
					{
						this.Win32Error(num6, name);
					}
					return array;
				}
				if (num3 != 234)
				{
					return obj;
				}
			}
			switch (num)
			{
			case 1:
				if (RegistryKey._SystemDefaultCharSize != 1)
				{
					StringBuilder stringBuilder = new StringBuilder(num2 / 2);
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, stringBuilder, ref num2);
					obj = stringBuilder.ToString();
				}
				else
				{
					byte[] array2 = new byte[num2];
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array2, ref num2);
					obj = Encoding.Default.GetString(array2, 0, array2.Length - 1);
				}
				break;
			case 2:
				if (RegistryKey._SystemDefaultCharSize != 1)
				{
					StringBuilder stringBuilder2 = new StringBuilder(num2 / 2);
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, stringBuilder2, ref num2);
					if (doNotExpand)
					{
						obj = stringBuilder2.ToString();
					}
					else
					{
						obj = Environment.ExpandEnvironmentVariables(stringBuilder2.ToString());
					}
				}
				else
				{
					byte[] array3 = new byte[num2];
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array3, ref num2);
					string @string = Encoding.Default.GetString(array3, 0, array3.Length - 1);
					if (doNotExpand)
					{
						obj = @string;
					}
					else
					{
						obj = Environment.ExpandEnvironmentVariables(@string);
					}
				}
				break;
			case 3:
			case 5:
			{
				byte[] array4 = new byte[num2];
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array4, ref num2);
				obj = array4;
				break;
			}
			case 4:
			{
				int num7 = 0;
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, ref num7, ref num2);
				obj = num7;
				break;
			}
			case 7:
			{
				bool flag = RegistryKey._SystemDefaultCharSize != 1;
				IList list = new ArrayList();
				if (flag)
				{
					char[] array5 = new char[num2 / 2];
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array5, ref num2);
					int num8 = 0;
					int num9 = array5.Length;
					while (num3 == 0)
					{
						if (num8 >= num9)
						{
							break;
						}
						int num10 = num8;
						while (num10 < num9 && array5[num10] != '\0')
						{
							num10++;
						}
						if (num10 < num9)
						{
							if (num10 - num8 > 0)
							{
								list.Add(new string(array5, num8, num10 - num8));
							}
							else if (num10 != num9 - 1)
							{
								list.Add(string.Empty);
							}
						}
						else
						{
							list.Add(new string(array5, num8, num9 - num8));
						}
						num8 = num10 + 1;
					}
				}
				else
				{
					byte[] array6 = new byte[num2];
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array6, ref num2);
					int num11 = 0;
					int num12 = array6.Length;
					while (num3 == 0 && num11 < num12)
					{
						int num13 = num11;
						while (num13 < num12 && array6[num13] != 0)
						{
							num13++;
						}
						if (num13 < num12)
						{
							if (num13 - num11 > 0)
							{
								list.Add(Encoding.Default.GetString(array6, num11, num13 - num11));
							}
							else if (num13 != num12 - 1)
							{
								list.Add(string.Empty);
							}
						}
						else
						{
							list.Add(Encoding.Default.GetString(array6, num11, num12 - num11));
						}
						num11 = num13 + 1;
					}
				}
				obj = new string[list.Count];
				list.CopyTo((Array)obj, 0);
				break;
			}
			case 11:
			{
				long num14 = 0L;
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, ref num14, ref num2);
				obj = num14;
				break;
			}
			}
			return obj;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x00097E7C File Offset: 0x00096E7C
		[ComVisible(false)]
		public RegistryValueKind GetValueKind(string name)
		{
			this.CheckValueReadPermission(name);
			this.EnsureNotDisposed();
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			if (num3 != 0)
			{
				this.Win32Error(num3, null);
			}
			if (!Enum.IsDefined(typeof(RegistryValueKind), num))
			{
				return RegistryValueKind.Unknown;
			}
			return (RegistryValueKind)num;
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x00097ED2 File Offset: 0x00096ED2
		private bool IsDirty()
		{
			return (this.state & 1) != 0;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x00097EE2 File Offset: 0x00096EE2
		private bool IsSystemKey()
		{
			return (this.state & 2) != 0;
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x00097EF2 File Offset: 0x00096EF2
		private bool IsWritable()
		{
			return (this.state & 4) != 0;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x00097F02 File Offset: 0x00096F02
		private bool IsPerfDataKey()
		{
			return (this.state & 8) != 0;
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x00097F12 File Offset: 0x00096F12
		private static bool IsWin9x()
		{
			return (Environment.OSInfo & Environment.OSName.Win9x) != Environment.OSName.Invalid;
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x00097F22 File Offset: 0x00096F22
		public string Name
		{
			get
			{
				this.EnsureNotDisposed();
				return this.keyName;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x00097F30 File Offset: 0x00096F30
		private void SetDirty()
		{
			this.state |= 1;
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x00097F40 File Offset: 0x00096F40
		public void SetValue(string name, object value)
		{
			this.SetValue(name, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x00097F4C File Offset: 0x00096F4C
		[ComVisible(false)]
		public unsafe void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (name != null && name.Length > 255)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RegKeyStrLenBug"));
			}
			if (!Enum.IsDefined(typeof(RegistryValueKind), valueKind))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RegBadKeyKind"), "valueKind");
			}
			this.EnsureWriteable();
			if (!this.remoteKey && this.ContainsRegistryValue(name))
			{
				this.CheckValueWritePermission(name);
			}
			else
			{
				this.CheckValueCreatePermission(name);
			}
			if (valueKind == RegistryValueKind.Unknown)
			{
				valueKind = this.CalculateValueKind(value);
			}
			int num = 0;
			try
			{
				switch (valueKind)
				{
				case RegistryValueKind.String:
				case RegistryValueKind.ExpandString:
				{
					string text = value.ToString();
					if (RegistryKey._SystemDefaultCharSize == 1)
					{
						byte[] bytes = Encoding.Default.GetBytes(text);
						byte[] array = new byte[bytes.Length + 1];
						Array.Copy(bytes, 0, array, 0, bytes.Length);
						num = Win32Native.RegSetValueEx(this.hkey, name, 0, valueKind, array, array.Length);
						goto IL_0374;
					}
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, valueKind, text, text.Length * 2 + 2);
					goto IL_0374;
				}
				case RegistryValueKind.Binary:
					break;
				case RegistryValueKind.DWord:
				{
					int num2 = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.DWord, ref num2, 4);
					goto IL_0374;
				}
				case (RegistryValueKind)5:
				case (RegistryValueKind)6:
				case (RegistryValueKind)8:
				case (RegistryValueKind)9:
				case (RegistryValueKind)10:
					goto IL_0374;
				case RegistryValueKind.MultiString:
				{
					string[] array2 = (string[])((string[])value).Clone();
					bool flag = RegistryKey._SystemDefaultCharSize != 1;
					int num3 = 0;
					if (flag)
					{
						for (int i = 0; i < array2.Length; i++)
						{
							if (array2[i] == null)
							{
								ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetStrArrNull);
							}
							num3 += (array2[i].Length + 1) * 2;
						}
						num3 += 2;
					}
					else
					{
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j] == null)
							{
								ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetStrArrNull);
							}
							num3 += Encoding.Default.GetByteCount(array2[j]) + 1;
						}
						num3++;
					}
					byte[] array3 = new byte[num3];
					try
					{
						fixed (byte* ptr = array3)
						{
							IntPtr intPtr = new IntPtr((void*)ptr);
							for (int k = 0; k < array2.Length; k++)
							{
								if (flag)
								{
									string.InternalCopy(array2[k], intPtr, array2[k].Length * 2);
									intPtr = new IntPtr((long)intPtr + (long)(array2[k].Length * 2));
									*(short*)intPtr.ToPointer() = 0;
									intPtr = new IntPtr((long)intPtr + 2L);
								}
								else
								{
									byte[] bytes2 = Encoding.Default.GetBytes(array2[k]);
									Buffer.memcpy(bytes2, 0, (byte*)intPtr.ToPointer(), 0, bytes2.Length);
									intPtr = new IntPtr((long)intPtr + (long)bytes2.Length);
									*(byte*)intPtr.ToPointer() = 0;
									intPtr = new IntPtr((long)intPtr + 1L);
								}
							}
							if (flag)
							{
								*(short*)intPtr.ToPointer() = 0;
								intPtr = new IntPtr((long)intPtr + 2L);
							}
							else
							{
								*(byte*)intPtr.ToPointer() = 0;
								intPtr = new IntPtr((long)intPtr + 1L);
							}
							num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.MultiString, array3, num3);
							goto IL_0374;
						}
					}
					finally
					{
						byte* ptr = null;
					}
					break;
				}
				case RegistryValueKind.QWord:
				{
					long num4 = Convert.ToInt64(value, CultureInfo.InvariantCulture);
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.QWord, ref num4, 8);
					goto IL_0374;
				}
				default:
					goto IL_0374;
				}
				byte[] array4 = (byte[])value;
				num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.Binary, array4, array4.Length);
				IL_0374:;
			}
			catch (OverflowException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetMismatchedKind);
			}
			catch (InvalidOperationException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetMismatchedKind);
			}
			catch (FormatException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetMismatchedKind);
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegSetMismatchedKind);
			}
			if (num == 0)
			{
				this.SetDirty();
				return;
			}
			this.Win32Error(num, null);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x00098388 File Offset: 0x00097388
		private RegistryValueKind CalculateValueKind(object value)
		{
			if (value is int)
			{
				return RegistryValueKind.DWord;
			}
			if (!(value is Array))
			{
				return RegistryValueKind.String;
			}
			if (value is byte[])
			{
				return RegistryValueKind.Binary;
			}
			if (value is string[])
			{
				return RegistryValueKind.MultiString;
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_RegSetBadArrType", new object[] { value.GetType().Name }));
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000983E2 File Offset: 0x000973E2
		public override string ToString()
		{
			this.EnsureNotDisposed();
			return this.keyName;
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000983F0 File Offset: 0x000973F0
		public RegistrySecurity GetAccessControl()
		{
			return this.GetAccessControl(AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000983FA File Offset: 0x000973FA
		public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
		{
			this.EnsureNotDisposed();
			return new RegistrySecurity(this.hkey, this.keyName, includeSections);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x00098414 File Offset: 0x00097414
		public void SetAccessControl(RegistrySecurity registrySecurity)
		{
			this.EnsureWriteable();
			if (registrySecurity == null)
			{
				throw new ArgumentNullException("registrySecurity");
			}
			registrySecurity.Persist(this.hkey, this.keyName);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x0009843C File Offset: 0x0009743C
		internal void Win32Error(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
				throw new IOException(Environment.GetResourceString("Arg_RegKeyNotFound"), errorCode);
			case 3:
			case 4:
				break;
			case 5:
				if (str != null)
				{
					throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_RegistryKeyGeneric_Key", new object[] { str }));
				}
				throw new UnauthorizedAccessException();
			case 6:
				this.hkey.SetHandleAsInvalid();
				this.hkey = null;
				break;
			default:
				if (errorCode == 234)
				{
					if (this.remoteKey)
					{
						return;
					}
				}
				break;
			}
			throw new IOException(Win32Native.GetMessage(errorCode), errorCode);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000984D0 File Offset: 0x000974D0
		internal static void Win32ErrorStatic(int errorCode, string str)
		{
			if (errorCode != 5)
			{
				throw new IOException(Win32Native.GetMessage(errorCode), errorCode);
			}
			if (str != null)
			{
				throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_RegistryKeyGeneric_Key", new object[] { str }));
			}
			throw new UnauthorizedAccessException();
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x00098514 File Offset: 0x00097514
		internal static string FixupName(string name)
		{
			if (name.IndexOf('\\') == -1)
			{
				return name;
			}
			StringBuilder stringBuilder = new StringBuilder(name);
			RegistryKey.FixupPath(stringBuilder);
			int num = stringBuilder.Length - 1;
			if (stringBuilder[num] == '\\')
			{
				stringBuilder.Length = num;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x0009855C File Offset: 0x0009755C
		private static void FixupPath(StringBuilder path)
		{
			int length = path.Length;
			bool flag = false;
			char maxValue = char.MaxValue;
			for (int i = 1; i < length - 1; i++)
			{
				if (path[i] == '\\')
				{
					i++;
					while (i < length && path[i] == '\\')
					{
						path[i] = maxValue;
						i++;
						flag = true;
					}
				}
			}
			if (flag)
			{
				int i = 0;
				int num = 0;
				while (i < length)
				{
					if (path[i] == maxValue)
					{
						i++;
					}
					else
					{
						path[num] = path[i];
						i++;
						num++;
					}
				}
				path.Length += num - i;
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000985FC File Offset: 0x000975FC
		private void CheckOpenSubKeyPermission(string subkeyName, bool subKeyWritable)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				this.CheckSubKeyReadPermission(subkeyName);
			}
			if (subKeyWritable && this.checkMode == RegistryKeyPermissionCheck.ReadSubTree)
			{
				this.CheckSubTreeReadWritePermission(subkeyName);
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x00098620 File Offset: 0x00097620
		private void CheckOpenSubKeyPermission(string subkeyName, RegistryKeyPermissionCheck subKeyCheck)
		{
			if (subKeyCheck == RegistryKeyPermissionCheck.Default && this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				this.CheckSubKeyReadPermission(subkeyName);
			}
			this.CheckSubTreePermission(subkeyName, subKeyCheck);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x0009863C File Offset: 0x0009763C
		private void CheckSubTreePermission(string subkeyName, RegistryKeyPermissionCheck subKeyCheck)
		{
			if (subKeyCheck == RegistryKeyPermissionCheck.ReadSubTree)
			{
				if (this.checkMode == RegistryKeyPermissionCheck.Default)
				{
					this.CheckSubTreeReadPermission(subkeyName);
					return;
				}
			}
			else if (subKeyCheck == RegistryKeyPermissionCheck.ReadWriteSubTree && this.checkMode != RegistryKeyPermissionCheck.ReadWriteSubTree)
			{
				this.CheckSubTreeReadWritePermission(subkeyName);
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x00098666 File Offset: 0x00097666
		private void CheckSubKeyWritePermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName + "\\.").Demand();
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x0009869F File Offset: 0x0009769F
		private void CheckSubKeyReadPermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + subkeyName + "\\.").Demand();
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000986D0 File Offset: 0x000976D0
		private void CheckSubKeyCreatePermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Create, this.keyName + "\\" + subkeyName + "\\.").Demand();
			}
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x00098709 File Offset: 0x00097709
		private void CheckSubTreeReadPermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + subkeyName + "\\").Demand();
			}
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x00098742 File Offset: 0x00097742
		private void CheckSubTreeWritePermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName + "\\").Demand();
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x0009877B File Offset: 0x0009777B
		private void CheckSubTreeReadWritePermission(string subkeyName)
		{
			if (this.remoteKey)
			{
				RegistryKey.CheckUnmanagedCodePermission();
				return;
			}
			new RegistryPermission(RegistryPermissionAccess.Read | RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName).Demand();
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000987A7 File Offset: 0x000977A7
		private static void CheckUnmanagedCodePermission()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000987B4 File Offset: 0x000977B4
		private void CheckValueWritePermission(string valueName)
		{
			if (this.remoteKey)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000987EE File Offset: 0x000977EE
		private void CheckValueCreatePermission(string valueName)
		{
			if (this.remoteKey)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return;
			}
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Create, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x00098828 File Offset: 0x00097828
		private void CheckValueReadPermission(string valueName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x0009884E File Offset: 0x0009784E
		private void CheckKeyReadPermission()
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\.").Demand();
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x00098874 File Offset: 0x00097874
		private bool ContainsRegistryValue(string name)
		{
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			return num3 == 0;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x0009889C File Offset: 0x0009789C
		private void EnsureNotDisposed()
		{
			if (this.hkey == null)
			{
				ThrowHelper.ThrowObjectDisposedException(this.keyName, ExceptionResource.ObjectDisposed_RegKeyClosed);
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000988B3 File Offset: 0x000978B3
		private void EnsureWriteable()
		{
			this.EnsureNotDisposed();
			if (!this.IsWritable())
			{
				ThrowHelper.ThrowUnauthorizedAccessException(ExceptionResource.UnauthorizedAccess_RegistryNoWrite);
			}
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000988CC File Offset: 0x000978CC
		private static int GetRegistryKeyAccess(bool isWritable)
		{
			int num;
			if (!isWritable)
			{
				num = 131097;
			}
			else
			{
				num = 131103;
			}
			return num;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000988EC File Offset: 0x000978EC
		private static int GetRegistryKeyAccess(RegistryKeyPermissionCheck mode)
		{
			int num = 0;
			switch (mode)
			{
			case RegistryKeyPermissionCheck.Default:
			case RegistryKeyPermissionCheck.ReadSubTree:
				num = 131097;
				break;
			case RegistryKeyPermissionCheck.ReadWriteSubTree:
				num = 131103;
				break;
			}
			return num;
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x00098920 File Offset: 0x00097920
		private RegistryKeyPermissionCheck GetSubKeyPermissonCheck(bool subkeyWritable)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				return this.checkMode;
			}
			if (subkeyWritable)
			{
				return RegistryKeyPermissionCheck.ReadWriteSubTree;
			}
			return RegistryKeyPermissionCheck.ReadSubTree;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x00098938 File Offset: 0x00097938
		private static void ValidateKeyName(string name)
		{
			if (name == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.name);
			}
			int num = name.IndexOf("\\", StringComparison.OrdinalIgnoreCase);
			int num2 = 0;
			while (num != -1)
			{
				if (num - num2 > 255)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegKeyStrLenBug);
				}
				num2 = num + 1;
				num = name.IndexOf("\\", num2, StringComparison.OrdinalIgnoreCase);
			}
			if (name.Length - num2 > 255)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RegKeyStrLenBug);
			}
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x0009899D File Offset: 0x0009799D
		private static void ValidateKeyMode(RegistryKeyPermissionCheck mode)
		{
			if (mode < RegistryKeyPermissionCheck.Default || mode > RegistryKeyPermissionCheck.ReadWriteSubTree)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck, ExceptionArgument.mode);
			}
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000989AF File Offset: 0x000979AF
		private static void ValidateKeyRights(int rights)
		{
			if ((rights & -983104) != 0)
			{
				ThrowHelper.ThrowSecurityException(ExceptionResource.Security_RegistryPermission);
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000989C4 File Offset: 0x000979C4
		// Note: this type is marked as 'beforefieldinit'.
		static RegistryKey()
		{
			int num = 3;
			sbyte[] array = new sbyte[4];
			array[0] = 65;
			array[1] = 65;
			RegistryKey._SystemDefaultCharSize = num - Win32Native.lstrlen(array);
		}

		// Token: 0x04001723 RID: 5923
		private const int STATE_DIRTY = 1;

		// Token: 0x04001724 RID: 5924
		private const int STATE_SYSTEMKEY = 2;

		// Token: 0x04001725 RID: 5925
		private const int STATE_WRITEACCESS = 4;

		// Token: 0x04001726 RID: 5926
		private const int STATE_PERF_DATA = 8;

		// Token: 0x04001727 RID: 5927
		private const int MaxKeyLength = 255;

		// Token: 0x04001728 RID: 5928
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04001729 RID: 5929
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x0400172A RID: 5930
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x0400172B RID: 5931
		internal static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);

		// Token: 0x0400172C RID: 5932
		internal static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);

		// Token: 0x0400172D RID: 5933
		internal static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);

		// Token: 0x0400172E RID: 5934
		internal static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

		// Token: 0x0400172F RID: 5935
		internal static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);

		// Token: 0x04001730 RID: 5936
		internal static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);

		// Token: 0x04001731 RID: 5937
		internal static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);

		// Token: 0x04001732 RID: 5938
		private static readonly string[] hkeyNames = new string[] { "HKEY_CLASSES_ROOT", "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE", "HKEY_USERS", "HKEY_PERFORMANCE_DATA", "HKEY_CURRENT_CONFIG", "HKEY_DYN_DATA" };

		// Token: 0x04001733 RID: 5939
		private SafeRegistryHandle hkey;

		// Token: 0x04001734 RID: 5940
		private int state;

		// Token: 0x04001735 RID: 5941
		private string keyName;

		// Token: 0x04001736 RID: 5942
		private bool remoteKey;

		// Token: 0x04001737 RID: 5943
		private RegistryKeyPermissionCheck checkMode;

		// Token: 0x04001738 RID: 5944
		private static readonly int _SystemDefaultCharSize;
	}
}

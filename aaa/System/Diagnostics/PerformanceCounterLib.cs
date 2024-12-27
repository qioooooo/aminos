using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000768 RID: 1896
	internal class PerformanceCounterLib
	{
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06003A47 RID: 14919 RVA: 0x000F6B10 File Offset: 0x000F5B10
		private static object InternalSyncObject
		{
			get
			{
				if (PerformanceCounterLib.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref PerformanceCounterLib.s_InternalSyncObject, obj, null);
				}
				return PerformanceCounterLib.s_InternalSyncObject;
			}
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x000F6B3C File Offset: 0x000F5B3C
		internal PerformanceCounterLib(string machineName, string lcid)
		{
			this.machineName = machineName;
			this.perfLcid = lcid;
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06003A49 RID: 14921 RVA: 0x000F6B74 File Offset: 0x000F5B74
		internal static string ComputerName
		{
			get
			{
				if (PerformanceCounterLib.computerName == null)
				{
					lock (PerformanceCounterLib.InternalSyncObject)
					{
						if (PerformanceCounterLib.computerName == null)
						{
							StringBuilder stringBuilder = new StringBuilder(256);
							SafeNativeMethods.GetComputerName(stringBuilder, new int[] { stringBuilder.Capacity });
							PerformanceCounterLib.computerName = stringBuilder.ToString();
						}
					}
				}
				return PerformanceCounterLib.computerName;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x000F6BE8 File Offset: 0x000F5BE8
		private unsafe Hashtable CategoryTable
		{
			get
			{
				if (this.categoryTable == null)
				{
					lock (this.CategoryTableLock)
					{
						if (this.categoryTable == null)
						{
							byte[] performanceData = this.GetPerformanceData("Global");
							fixed (byte* ptr = performanceData)
							{
								IntPtr intPtr = new IntPtr((void*)ptr);
								NativeMethods.PERF_DATA_BLOCK perf_DATA_BLOCK = new NativeMethods.PERF_DATA_BLOCK();
								Marshal.PtrToStructure(intPtr, perf_DATA_BLOCK);
								intPtr = (IntPtr)((long)intPtr + (long)perf_DATA_BLOCK.HeaderLength);
								int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
								long num = ptr + (long)perf_DATA_BLOCK.TotalByteLength;
								Hashtable hashtable = new Hashtable(numObjectTypes, StringComparer.OrdinalIgnoreCase);
								int num2 = 0;
								while (num2 < numObjectTypes && (long)intPtr < num)
								{
									NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = new NativeMethods.PERF_OBJECT_TYPE();
									Marshal.PtrToStructure(intPtr, perf_OBJECT_TYPE);
									CategoryEntry categoryEntry = new CategoryEntry(perf_OBJECT_TYPE);
									IntPtr intPtr2 = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.TotalByteLength);
									intPtr = (IntPtr)((long)intPtr + (long)perf_OBJECT_TYPE.HeaderLength);
									int num3 = 0;
									int num4 = -1;
									for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
									{
										NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new NativeMethods.PERF_COUNTER_DEFINITION();
										Marshal.PtrToStructure(intPtr, perf_COUNTER_DEFINITION);
										if (perf_COUNTER_DEFINITION.CounterNameTitleIndex != num4)
										{
											categoryEntry.CounterIndexes[num3] = perf_COUNTER_DEFINITION.CounterNameTitleIndex;
											categoryEntry.HelpIndexes[num3] = perf_COUNTER_DEFINITION.CounterHelpTitleIndex;
											num4 = perf_COUNTER_DEFINITION.CounterNameTitleIndex;
											num3++;
										}
										intPtr = (IntPtr)((long)intPtr + (long)perf_COUNTER_DEFINITION.ByteLength);
									}
									if (num3 < categoryEntry.CounterIndexes.Length)
									{
										int[] array = new int[num3];
										int[] array2 = new int[num3];
										Array.Copy(categoryEntry.CounterIndexes, array, num3);
										Array.Copy(categoryEntry.HelpIndexes, array2, num3);
										categoryEntry.CounterIndexes = array;
										categoryEntry.HelpIndexes = array2;
									}
									string text = (string)this.NameTable[categoryEntry.NameIndex];
									if (text != null)
									{
										hashtable[text] = categoryEntry;
									}
									intPtr = intPtr2;
									num2++;
								}
								this.categoryTable = hashtable;
							}
						}
					}
				}
				return this.categoryTable;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06003A4B RID: 14923 RVA: 0x000F6E28 File Offset: 0x000F5E28
		internal Hashtable HelpTable
		{
			get
			{
				if (this.helpTable == null)
				{
					lock (this.HelpTableLock)
					{
						if (this.helpTable == null)
						{
							this.helpTable = this.GetStringTable(true);
						}
					}
				}
				return this.helpTable;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x000F6E80 File Offset: 0x000F5E80
		private static string IniFilePath
		{
			get
			{
				if (PerformanceCounterLib.iniFilePath == null)
				{
					lock (PerformanceCounterLib.InternalSyncObject)
					{
						if (PerformanceCounterLib.iniFilePath == null)
						{
							EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
							environmentPermission.Assert();
							try
							{
								PerformanceCounterLib.iniFilePath = Path.GetTempFileName();
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
				}
				return PerformanceCounterLib.iniFilePath;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x000F6EF0 File Offset: 0x000F5EF0
		internal Hashtable NameTable
		{
			get
			{
				if (this.nameTable == null)
				{
					lock (this.NameTableLock)
					{
						if (this.nameTable == null)
						{
							this.nameTable = this.GetStringTable(false);
						}
					}
				}
				return this.nameTable;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x000F6F48 File Offset: 0x000F5F48
		private static string SymbolFilePath
		{
			get
			{
				if (PerformanceCounterLib.symbolFilePath == null)
				{
					lock (PerformanceCounterLib.InternalSyncObject)
					{
						if (PerformanceCounterLib.symbolFilePath == null)
						{
							EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
							environmentPermission.Assert();
							string tempPath = Path.GetTempPath();
							CodeAccessPermission.RevertAssert();
							PermissionSet permissionSet = new PermissionSet(PermissionState.None);
							permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
							permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, tempPath));
							permissionSet.Assert();
							try
							{
								PerformanceCounterLib.symbolFilePath = Path.GetTempFileName();
							}
							finally
							{
								PermissionSet.RevertAssert();
							}
						}
					}
				}
				return PerformanceCounterLib.symbolFilePath;
			}
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x000F6FEC File Offset: 0x000F5FEC
		internal static bool CategoryExists(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				if (performanceCounterLib.CategoryExists(category))
				{
					return true;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			return performanceCounterLib.CategoryExists(category);
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x000F703F File Offset: 0x000F603F
		internal bool CategoryExists(string category)
		{
			return this.CategoryTable.ContainsKey(category);
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x000F7050 File Offset: 0x000F6050
		internal static void CloseAllLibraries()
		{
			if (PerformanceCounterLib.libraryTable != null)
			{
				foreach (object obj in PerformanceCounterLib.libraryTable.Values)
				{
					PerformanceCounterLib performanceCounterLib = (PerformanceCounterLib)obj;
					performanceCounterLib.Close();
				}
				PerformanceCounterLib.libraryTable = null;
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x000F70BC File Offset: 0x000F60BC
		internal static void CloseAllTables()
		{
			if (PerformanceCounterLib.libraryTable != null)
			{
				foreach (object obj in PerformanceCounterLib.libraryTable.Values)
				{
					PerformanceCounterLib performanceCounterLib = (PerformanceCounterLib)obj;
					performanceCounterLib.CloseTables();
				}
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x000F7120 File Offset: 0x000F6120
		internal void CloseTables()
		{
			this.nameTable = null;
			this.helpTable = null;
			this.categoryTable = null;
			this.customCategoryTable = null;
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x000F713E File Offset: 0x000F613E
		internal void Close()
		{
			if (this.performanceMonitor != null)
			{
				this.performanceMonitor.Close();
				this.performanceMonitor = null;
			}
			this.CloseTables();
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x000F7160 File Offset: 0x000F6160
		internal static bool CounterExists(string machine, string category, string counter)
		{
			bool flag = false;
			bool flag2 = false;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				flag2 = performanceCounterLib.CounterExists(category, counter, ref flag);
				if (flag2)
				{
					break;
				}
			}
			if (!flag2)
			{
				PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
				flag2 = performanceCounterLib.CounterExists(category, counter, ref flag);
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return flag2;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x000F71D0 File Offset: 0x000F61D0
		private bool CounterExists(string category, string counter, ref bool categoryExists)
		{
			categoryExists = false;
			if (!this.CategoryTable.ContainsKey(category))
			{
				return false;
			}
			categoryExists = true;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
			{
				int num = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num];
				if (text == null)
				{
					text = string.Empty;
				}
				if (string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x000F7250 File Offset: 0x000F6250
		private static void CreateIniFile(string categoryName, string categoryHelp, CounterCreationDataCollection creationData, string[] languageIds)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				StreamWriter streamWriter = new StreamWriter(PerformanceCounterLib.IniFilePath, false, Encoding.Unicode);
				try
				{
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[info]");
					streamWriter.Write("drivername");
					streamWriter.Write("=");
					streamWriter.WriteLine(categoryName);
					streamWriter.Write("symbolfile");
					streamWriter.Write("=");
					streamWriter.WriteLine(Path.GetFileName(PerformanceCounterLib.SymbolFilePath));
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[languages]");
					foreach (string text in languageIds)
					{
						streamWriter.Write(text);
						streamWriter.Write("=");
						streamWriter.Write("language");
						streamWriter.WriteLine(text);
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[objects]");
					foreach (string text2 in languageIds)
					{
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text2);
						streamWriter.Write("_NAME");
						streamWriter.Write("=");
						streamWriter.WriteLine(categoryName);
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[text]");
					foreach (string text3 in languageIds)
					{
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text3);
						streamWriter.Write("_NAME");
						streamWriter.Write("=");
						streamWriter.WriteLine(categoryName);
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text3);
						streamWriter.Write("_HELP");
						streamWriter.Write("=");
						if (categoryHelp == null || categoryHelp == string.Empty)
						{
							streamWriter.WriteLine(SR.GetString("HelpNotAvailable"));
						}
						else
						{
							streamWriter.WriteLine(categoryHelp);
						}
						int num = 0;
						foreach (object obj in creationData)
						{
							CounterCreationData counterCreationData = (CounterCreationData)obj;
							num++;
							streamWriter.WriteLine("");
							streamWriter.Write("DEVICE_COUNTER_");
							streamWriter.Write(num.ToString(CultureInfo.InvariantCulture));
							streamWriter.Write("_");
							streamWriter.Write(text3);
							streamWriter.Write("_NAME");
							streamWriter.Write("=");
							streamWriter.WriteLine(counterCreationData.CounterName);
							streamWriter.Write("DEVICE_COUNTER_");
							streamWriter.Write(num.ToString(CultureInfo.InvariantCulture));
							streamWriter.Write("_");
							streamWriter.Write(text3);
							streamWriter.Write("_HELP");
							streamWriter.Write("=");
							streamWriter.WriteLine(counterCreationData.CounterHelp);
						}
					}
					streamWriter.WriteLine("");
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x000F75C8 File Offset: 0x000F65C8
		private static void CreateRegistryEntry(string categoryName, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection creationData, ref bool iniRegistered)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryKey registryKey3 = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true);
				registryKey2 = registryKey.OpenSubKey(categoryName + "\\Performance", true);
				if (registryKey2 == null)
				{
					registryKey2 = registryKey.CreateSubKey(categoryName + "\\Performance");
				}
				registryKey2.SetValue("Open", "OpenPerformanceData");
				registryKey2.SetValue("Collect", "CollectPerformanceData");
				registryKey2.SetValue("Close", "ClosePerformanceData");
				registryKey2.SetValue("Library", "netfxperf.dll");
				registryKey2.SetValue("IsMultiInstance", (int)categoryType, RegistryValueKind.DWord);
				registryKey2.SetValue("CategoryOptions", 3, RegistryValueKind.DWord);
				string[] array = new string[creationData.Count];
				string[] array2 = new string[creationData.Count];
				for (int i = 0; i < creationData.Count; i++)
				{
					array[i] = creationData[i].CounterName;
					array2[i] = ((int)creationData[i].CounterType).ToString(CultureInfo.InvariantCulture);
				}
				registryKey3 = registryKey.OpenSubKey(categoryName + "\\Linkage", true);
				if (registryKey3 == null)
				{
					registryKey3 = registryKey.CreateSubKey(categoryName + "\\Linkage");
				}
				registryKey3.SetValue("Export", new string[] { categoryName });
				registryKey2.SetValue("Counter Types", array2);
				registryKey2.SetValue("Counter Names", array);
				object value = registryKey2.GetValue("First Counter");
				if (value != null)
				{
					iniRegistered = true;
				}
				else
				{
					iniRegistered = false;
				}
			}
			finally
			{
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey3 != null)
				{
					registryKey3.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x000F779C File Offset: 0x000F679C
		private static void CreateSymbolFile(CounterCreationDataCollection creationData)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				StreamWriter streamWriter = new StreamWriter(PerformanceCounterLib.SymbolFilePath);
				try
				{
					streamWriter.Write("#define");
					streamWriter.Write(" ");
					streamWriter.Write("OBJECT_");
					streamWriter.WriteLine("1 0;");
					for (int i = 1; i <= creationData.Count; i++)
					{
						streamWriter.Write("#define");
						streamWriter.Write(" ");
						streamWriter.Write("DEVICE_COUNTER_");
						streamWriter.Write(i.ToString(CultureInfo.InvariantCulture));
						streamWriter.Write(" ");
						streamWriter.Write((i * 2).ToString(CultureInfo.InvariantCulture));
						streamWriter.WriteLine(";");
					}
					streamWriter.WriteLine("");
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x000F7898 File Offset: 0x000F6898
		private static void DeleteRegistryEntry(string categoryName)
		{
			RegistryKey registryKey = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true);
				bool flag = false;
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(categoryName, true))
				{
					if (registryKey2 != null)
					{
						if (registryKey2.GetValueNames().Length == 0)
						{
							flag = true;
						}
						else
						{
							registryKey2.DeleteSubKeyTree("Linkage");
							registryKey2.DeleteSubKeyTree("Performance");
						}
					}
				}
				if (flag)
				{
					registryKey.DeleteSubKeyTree(categoryName);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x000F793C File Offset: 0x000F693C
		private static void DeleteTemporaryFiles()
		{
			try
			{
				File.Delete(PerformanceCounterLib.IniFilePath);
			}
			catch
			{
			}
			try
			{
				File.Delete(PerformanceCounterLib.SymbolFilePath);
			}
			catch
			{
			}
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x000F7984 File Offset: 0x000F6984
		internal bool FindCustomCategory(string category, out PerformanceCounterCategoryType categoryType)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			categoryType = PerformanceCounterCategoryType.Unknown;
			if (this.customCategoryTable == null)
			{
				this.customCategoryTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			if (this.customCategoryTable.ContainsKey(category))
			{
				categoryType = (PerformanceCounterCategoryType)this.customCategoryTable[category];
				return true;
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			try
			{
				string text = "SYSTEM\\CurrentControlSet\\Services\\" + category + "\\Performance";
				if (this.machineName == "." || string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					registryKey = Registry.LocalMachine.OpenSubKey(text);
				}
				else
				{
					registryKey2 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "\\\\" + this.machineName);
					if (registryKey2 != null)
					{
						try
						{
							registryKey = registryKey2.OpenSubKey(text);
						}
						catch (SecurityException)
						{
							categoryType = PerformanceCounterCategoryType.Unknown;
							this.customCategoryTable[category] = categoryType;
							return false;
						}
					}
				}
				if (registryKey != null)
				{
					object value = registryKey.GetValue("Library", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
					if (value != null && value is string && (string.Compare((string)value, "netfxperf.dll", StringComparison.OrdinalIgnoreCase) == 0 || ((string)value).EndsWith("\\netfxperf.dll", StringComparison.OrdinalIgnoreCase)))
					{
						object value2 = registryKey.GetValue("IsMultiInstance");
						if (value2 != null)
						{
							categoryType = (PerformanceCounterCategoryType)value2;
							if (categoryType < PerformanceCounterCategoryType.Unknown || categoryType > PerformanceCounterCategoryType.MultiInstance)
							{
								categoryType = PerformanceCounterCategoryType.Unknown;
							}
						}
						else
						{
							categoryType = PerformanceCounterCategoryType.Unknown;
						}
						object value3 = registryKey.GetValue("First Counter");
						if (value3 != null)
						{
							int num = (int)value3;
							this.customCategoryTable[category] = categoryType;
							return true;
						}
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				PermissionSet.RevertAssert();
			}
			return false;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x000F7B84 File Offset: 0x000F6B84
		internal static string[] GetCategories(string machineName)
		{
			PerformanceCounterLib performanceCounterLib;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machineName, cultureInfo);
				string[] categories = performanceCounterLib.GetCategories();
				if (categories.Length != 0)
				{
					return categories;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machineName, new CultureInfo(9));
			return performanceCounterLib.GetCategories();
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x000F7BD4 File Offset: 0x000F6BD4
		internal string[] GetCategories()
		{
			ICollection keys = this.CategoryTable.Keys;
			string[] array = new string[keys.Count];
			keys.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000F7C04 File Offset: 0x000F6C04
		internal static string GetCategoryHelp(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib;
			string text;
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					text = performanceCounterLib.GetCategoryHelp(category);
					if (text != null)
					{
						return text;
					}
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			text = performanceCounterLib.GetCategoryHelp(category);
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			text = performanceCounterLib.GetCategoryHelp(category);
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return text;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x000F7C90 File Offset: 0x000F6C90
		private string GetCategoryHelp(string category)
		{
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			return (string)this.HelpTable[categoryEntry.HelpIndex];
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x000F7CD0 File Offset: 0x000F6CD0
		internal static CategorySample GetCategorySample(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib;
			CategorySample categorySample;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				categorySample = performanceCounterLib.GetCategorySample(category);
				if (categorySample != null)
				{
					return categorySample;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			categorySample = performanceCounterLib.GetCategorySample(category);
			if (categorySample == null)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return categorySample;
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x000F7D34 File Offset: 0x000F6D34
		private CategorySample GetCategorySample(string category)
		{
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			byte[] performanceData = this.GetPerformanceData(categoryEntry.NameIndex.ToString(CultureInfo.InvariantCulture));
			if (performanceData == null)
			{
				throw new InvalidOperationException(SR.GetString("CantReadCategory", new object[] { category }));
			}
			return new CategorySample(performanceData, categoryEntry, this);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x000F7D9C File Offset: 0x000F6D9C
		internal static string[] GetCounters(string machine, string category)
		{
			bool flag = false;
			PerformanceCounterLib performanceCounterLib;
			string[] array;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				array = performanceCounterLib.GetCounters(category, ref flag);
				if (flag)
				{
					return array;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			array = performanceCounterLib.GetCounters(category, ref flag);
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return array;
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x000F7E08 File Offset: 0x000F6E08
		private string[] GetCounters(string category, ref bool categoryExists)
		{
			categoryExists = false;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			categoryExists = true;
			int num = 0;
			string[] array = new string[categoryEntry.CounterIndexes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num2];
				if (text != null && text != string.Empty)
				{
					array[num] = text;
					num++;
				}
			}
			if (num < array.Length)
			{
				string[] array2 = new string[num];
				Array.Copy(array, array2, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x000F7EAC File Offset: 0x000F6EAC
		internal static string GetCounterHelp(string machine, string category, string counter)
		{
			bool flag = false;
			PerformanceCounterLib performanceCounterLib;
			string text;
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					text = performanceCounterLib.GetCounterHelp(category, counter, ref flag);
					if (flag)
					{
						return text;
					}
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			text = performanceCounterLib.GetCounterHelp(category, counter, ref flag);
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategoryDetail", new object[] { category }));
			}
			return text;
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x000F7F3C File Offset: 0x000F6F3C
		private string GetCounterHelp(string category, string counter, ref bool categoryExists)
		{
			categoryExists = false;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			categoryExists = true;
			int num = -1;
			for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
			{
				int num2 = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num2];
				if (text == null)
				{
					text = string.Empty;
				}
				if (string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					num = categoryEntry.HelpIndexes[i];
					break;
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("MissingCounter", new object[] { counter }));
			}
			string text2 = (string)this.HelpTable[num];
			if (text2 == null)
			{
				return string.Empty;
			}
			return text2;
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x000F8004 File Offset: 0x000F7004
		internal string GetCounterName(int index)
		{
			if (this.NameTable.ContainsKey(index))
			{
				return (string)this.NameTable[index];
			}
			return "";
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x000F8038 File Offset: 0x000F7038
		private static string[] GetLanguageIds()
		{
			RegistryKey registryKey = null;
			string[] array = new string[0];
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Perflib");
				if (registryKey != null)
				{
					array = registryKey.GetSubKeyNames();
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return array;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x000F8098 File Offset: 0x000F7098
		internal static PerformanceCounterLib GetPerformanceCounterLib(string machineName, CultureInfo culture)
		{
			SharedUtils.CheckEnvironment();
			string text;
			if ((culture.LCID & 65280) == 0)
			{
				text = culture.LCID.ToString("X3", CultureInfo.InvariantCulture);
			}
			else
			{
				text = culture.LCID.ToString("X4", CultureInfo.InvariantCulture);
			}
			if (machineName.CompareTo(".") == 0)
			{
				machineName = PerformanceCounterLib.ComputerName.ToLower(CultureInfo.InvariantCulture);
			}
			else
			{
				machineName = machineName.ToLower(CultureInfo.InvariantCulture);
			}
			if (PerformanceCounterLib.libraryTable == null)
			{
				PerformanceCounterLib.libraryTable = new Hashtable();
			}
			string text2 = machineName + ":" + text;
			if (PerformanceCounterLib.libraryTable.Contains(text2))
			{
				return (PerformanceCounterLib)PerformanceCounterLib.libraryTable[text2];
			}
			PerformanceCounterLib performanceCounterLib = new PerformanceCounterLib(machineName, text);
			PerformanceCounterLib.libraryTable[text2] = performanceCounterLib;
			return performanceCounterLib;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000F816C File Offset: 0x000F716C
		internal byte[] GetPerformanceData(string item)
		{
			if (this.performanceMonitor == null)
			{
				lock (this)
				{
					if (this.performanceMonitor == null)
					{
						this.performanceMonitor = new PerformanceMonitor(this.machineName);
					}
				}
			}
			return this.performanceMonitor.GetData(item);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x000F81C8 File Offset: 0x000F71C8
		private Hashtable GetStringTable(bool isHelp)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			RegistryKey registryKey;
			if (string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				registryKey = Registry.PerformanceData;
			}
			else
			{
				registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.PerformanceData, this.machineName);
			}
			Hashtable hashtable;
			try
			{
				string[] array = null;
				int i = 14;
				int num = 0;
				while (i > 0)
				{
					try
					{
						if (!isHelp)
						{
							array = (string[])registryKey.GetValue("Counter " + this.perfLcid);
						}
						else
						{
							array = (string[])registryKey.GetValue("Explain " + this.perfLcid);
						}
						if (array != null && array.Length != 0)
						{
							break;
						}
						i--;
						if (num == 0)
						{
							num = 10;
						}
						else
						{
							Thread.Sleep(num);
							num *= 2;
						}
					}
					catch (IOException)
					{
						array = null;
						break;
					}
				}
				if (array == null)
				{
					hashtable = new Hashtable();
				}
				else
				{
					hashtable = new Hashtable(array.Length / 2);
					for (int j = 0; j < array.Length / 2; j++)
					{
						string text = array[j * 2 + 1];
						if (text == null)
						{
							text = string.Empty;
						}
						hashtable[int.Parse(array[j * 2], CultureInfo.InvariantCulture)] = text;
					}
				}
			}
			finally
			{
				registryKey.Close();
			}
			return hashtable;
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x000F8328 File Offset: 0x000F7328
		internal static bool IsCustomCategory(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				if (performanceCounterLib.IsCustomCategory(category))
				{
					return true;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			return performanceCounterLib.IsCustomCategory(category);
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x000F8379 File Offset: 0x000F7379
		internal static bool IsBaseCounter(int type)
		{
			return type == 1073939458 || type == 1107494144 || type == 1073939459 || type == 1073939712 || type == 1073939457;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000F83A8 File Offset: 0x000F73A8
		private bool IsCustomCategory(string category)
		{
			PerformanceCounterCategoryType performanceCounterCategoryType;
			return this.FindCustomCategory(category, out performanceCounterCategoryType);
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000F83C0 File Offset: 0x000F73C0
		internal static PerformanceCounterCategoryType GetCategoryType(string machine, string category)
		{
			PerformanceCounterCategoryType performanceCounterCategoryType = PerformanceCounterCategoryType.Unknown;
			PerformanceCounterLib performanceCounterLib;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
				if (performanceCounterLib.FindCustomCategory(category, out performanceCounterCategoryType))
				{
					return performanceCounterCategoryType;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			performanceCounterLib.FindCustomCategory(category, out performanceCounterCategoryType);
			return performanceCounterCategoryType;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000F8414 File Offset: 0x000F7414
		internal static void RegisterCategory(string categoryName, PerformanceCounterCategoryType categoryType, string categoryHelp, CounterCreationDataCollection creationData)
		{
			try
			{
				bool flag = false;
				PerformanceCounterLib.CreateRegistryEntry(categoryName, categoryType, creationData, ref flag);
				if (!flag)
				{
					string[] languageIds = PerformanceCounterLib.GetLanguageIds();
					PerformanceCounterLib.CreateIniFile(categoryName, categoryHelp, creationData, languageIds);
					PerformanceCounterLib.CreateSymbolFile(creationData);
					PerformanceCounterLib.RegisterFiles(PerformanceCounterLib.IniFilePath, false);
				}
				PerformanceCounterLib.CloseAllTables();
				PerformanceCounterLib.CloseAllLibraries();
			}
			finally
			{
				PerformanceCounterLib.DeleteTemporaryFiles();
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000F8474 File Offset: 0x000F7474
		private static void RegisterFiles(string arg0, bool unregister)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.ErrorDialog = false;
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.WorkingDirectory = Environment.SystemDirectory;
			if (unregister)
			{
				processStartInfo.FileName = Environment.SystemDirectory + "\\unlodctr.exe";
			}
			else
			{
				processStartInfo.FileName = Environment.SystemDirectory + "\\lodctr.exe";
			}
			int num = 0;
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				processStartInfo.Arguments = "\"" + arg0 + "\"";
				Process process = Process.Start(processStartInfo);
				process.WaitForExit();
				num = process.ExitCode;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (unregister && num == 2)
			{
				num = 0;
			}
			if (num != 0)
			{
				throw SharedUtils.CreateSafeWin32Exception(num);
			}
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000F8540 File Offset: 0x000F7540
		internal static void UnregisterCategory(string categoryName)
		{
			PerformanceCounterLib.RegisterFiles(categoryName, true);
			PerformanceCounterLib.DeleteRegistryEntry(categoryName);
			PerformanceCounterLib.CloseAllTables();
			PerformanceCounterLib.CloseAllLibraries();
		}

		// Token: 0x04003308 RID: 13064
		internal const string PerfShimName = "netfxperf.dll";

		// Token: 0x04003309 RID: 13065
		private const string PerfShimFullNameSuffix = "\\netfxperf.dll";

		// Token: 0x0400330A RID: 13066
		internal const string OpenEntryPoint = "OpenPerformanceData";

		// Token: 0x0400330B RID: 13067
		internal const string CollectEntryPoint = "CollectPerformanceData";

		// Token: 0x0400330C RID: 13068
		internal const string CloseEntryPoint = "ClosePerformanceData";

		// Token: 0x0400330D RID: 13069
		internal const string SingleInstanceName = "systemdiagnosticsperfcounterlibsingleinstance";

		// Token: 0x0400330E RID: 13070
		private const string PerflibPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Perflib";

		// Token: 0x0400330F RID: 13071
		internal const string ServicePath = "SYSTEM\\CurrentControlSet\\Services";

		// Token: 0x04003310 RID: 13072
		private const string categorySymbolPrefix = "OBJECT_";

		// Token: 0x04003311 RID: 13073
		private const string conterSymbolPrefix = "DEVICE_COUNTER_";

		// Token: 0x04003312 RID: 13074
		private const string helpSufix = "_HELP";

		// Token: 0x04003313 RID: 13075
		private const string nameSufix = "_NAME";

		// Token: 0x04003314 RID: 13076
		private const string textDefinition = "[text]";

		// Token: 0x04003315 RID: 13077
		private const string infoDefinition = "[info]";

		// Token: 0x04003316 RID: 13078
		private const string languageDefinition = "[languages]";

		// Token: 0x04003317 RID: 13079
		private const string objectDefinition = "[objects]";

		// Token: 0x04003318 RID: 13080
		private const string driverNameKeyword = "drivername";

		// Token: 0x04003319 RID: 13081
		private const string symbolFileKeyword = "symbolfile";

		// Token: 0x0400331A RID: 13082
		private const string defineKeyword = "#define";

		// Token: 0x0400331B RID: 13083
		private const string languageKeyword = "language";

		// Token: 0x0400331C RID: 13084
		private const string DllName = "netfxperf.dll";

		// Token: 0x0400331D RID: 13085
		private static string computerName;

		// Token: 0x0400331E RID: 13086
		private static string iniFilePath;

		// Token: 0x0400331F RID: 13087
		private static string symbolFilePath;

		// Token: 0x04003320 RID: 13088
		private PerformanceMonitor performanceMonitor;

		// Token: 0x04003321 RID: 13089
		private string machineName;

		// Token: 0x04003322 RID: 13090
		private string perfLcid;

		// Token: 0x04003323 RID: 13091
		private Hashtable customCategoryTable;

		// Token: 0x04003324 RID: 13092
		private static Hashtable libraryTable;

		// Token: 0x04003325 RID: 13093
		private Hashtable categoryTable;

		// Token: 0x04003326 RID: 13094
		private Hashtable nameTable;

		// Token: 0x04003327 RID: 13095
		private Hashtable helpTable;

		// Token: 0x04003328 RID: 13096
		private readonly object CategoryTableLock = new object();

		// Token: 0x04003329 RID: 13097
		private readonly object NameTableLock = new object();

		// Token: 0x0400332A RID: 13098
		private readonly object HelpTableLock = new object();

		// Token: 0x0400332B RID: 13099
		private static object s_InternalSyncObject;
	}
}

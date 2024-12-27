using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x02000764 RID: 1892
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	public sealed class PerformanceCounterCategory
	{
		// Token: 0x06003A25 RID: 14885 RVA: 0x000F5EA8 File Offset: 0x000F4EA8
		public PerformanceCounterCategory()
		{
			this.machineName = ".";
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x000F5EBB File Offset: 0x000F4EBB
		public PerformanceCounterCategory(string categoryName)
			: this(categoryName, ".")
		{
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000F5ECC File Offset: 0x000F4ECC
		public PerformanceCounterCategory(string categoryName, string machineName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			this.categoryName = categoryName;
			this.machineName = machineName;
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x000F5F62 File Offset: 0x000F4F62
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x000F5F6C File Offset: 0x000F4F6C
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "CategoryName", value }));
				}
				lock (this)
				{
					PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, this.machineName, value);
					performanceCounterPermission.Demand();
					this.categoryName = value;
				}
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x000F5FF0 File Offset: 0x000F4FF0
		public string CategoryHelp
		{
			get
			{
				if (this.categoryName == null)
				{
					throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
				}
				if (this.categoryHelp == null)
				{
					this.categoryHelp = PerformanceCounterLib.GetCategoryHelp(this.machineName, this.categoryName);
				}
				return this.categoryHelp;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x000F6030 File Offset: 0x000F5030
		public PerformanceCounterCategoryType CategoryType
		{
			get
			{
				CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
				if (categorySample.IsMultiInstance)
				{
					return PerformanceCounterCategoryType.MultiInstance;
				}
				if (PerformanceCounterLib.IsCustomCategory(".", this.categoryName))
				{
					return PerformanceCounterLib.GetCategoryType(".", this.categoryName);
				}
				return PerformanceCounterCategoryType.SingleInstance;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x000F607D File Offset: 0x000F507D
		// (set) Token: 0x06003A2D RID: 14893 RVA: 0x000F6088 File Offset: 0x000F5088
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "MachineName", value }));
				}
				lock (this)
				{
					if (this.categoryName != null)
					{
						PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, value, this.categoryName);
						performanceCounterPermission.Demand();
					}
					this.machineName = value;
				}
			}
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000F6108 File Offset: 0x000F5108
		public bool CounterExists(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			return PerformanceCounterLib.CounterExists(this.machineName, this.categoryName, counterName);
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000F6142 File Offset: 0x000F5142
		public static bool CounterExists(string counterName, string categoryName)
		{
			return PerformanceCounterCategory.CounterExists(counterName, categoryName, ".");
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000F6150 File Offset: 0x000F5150
		public static bool CounterExists(string counterName, string categoryName, string machineName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			return PerformanceCounterLib.CounterExists(machineName, categoryName, counterName);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x000F61E8 File Offset: 0x000F51E8
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, string counterName, string counterHelp)
		{
			CounterCreationData counterCreationData = new CounterCreationData(counterName, counterHelp, PerformanceCounterType.NumberOfItems32);
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.Unknown, new CounterCreationDataCollection(new CounterCreationData[] { counterCreationData }));
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x000F621C File Offset: 0x000F521C
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp)
		{
			CounterCreationData counterCreationData = new CounterCreationData(counterName, counterHelp, PerformanceCounterType.NumberOfItems32);
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, categoryType, new CounterCreationDataCollection(new CounterCreationData[] { counterCreationData }));
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x000F6250 File Offset: 0x000F5250
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, CounterCreationDataCollection counterData)
		{
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.Unknown, counterData);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x000F625C File Offset: 0x000F525C
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData)
		{
			if (categoryType < PerformanceCounterCategoryType.Unknown || categoryType > PerformanceCounterCategoryType.MultiInstance)
			{
				throw new ArgumentOutOfRangeException("categoryType");
			}
			if (counterData == null)
			{
				throw new ArgumentNullException("counterData");
			}
			PerformanceCounterCategory.CheckValidCategory(categoryName);
			string text = ".";
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Administer, text, categoryName);
			performanceCounterPermission.Demand();
			SharedUtils.CheckNtEnvironment();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			PerformanceCounterCategory performanceCounterCategory;
			try
			{
				SharedUtils.EnterMutex("netfxperf.1.0", ref mutex);
				if (PerformanceCounterLib.IsCustomCategory(text, categoryName) || PerformanceCounterLib.CategoryExists(text, categoryName))
				{
					throw new InvalidOperationException(SR.GetString("PerformanceCategoryExists", new object[] { categoryName }));
				}
				PerformanceCounterCategory.CheckValidCounterLayout(counterData);
				PerformanceCounterLib.RegisterCategory(categoryName, categoryType, categoryHelp, counterData);
				performanceCounterCategory = new PerformanceCounterCategory(categoryName, text);
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
			return performanceCounterCategory;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x000F6328 File Offset: 0x000F5328
		internal static void CheckValidCategory(string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (!PerformanceCounterCategory.CheckValidId(categoryName))
			{
				throw new ArgumentException(SR.GetString("PerfInvalidCategoryName", new object[] { 1, 80 }));
			}
			if (categoryName.Length > 1024 - "netfxcustomperfcounters.1.0".Length)
			{
				throw new ArgumentException(SR.GetString("CategoryNameTooLong"));
			}
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x000F63A0 File Offset: 0x000F53A0
		internal static void CheckValidCounter(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (!PerformanceCounterCategory.CheckValidId(counterName))
			{
				throw new ArgumentException(SR.GetString("PerfInvalidCounterName", new object[] { 1, 80 }));
			}
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x000F63F0 File Offset: 0x000F53F0
		internal static bool CheckValidId(string id)
		{
			if (id.Length == 0 || id.Length > 80)
			{
				return false;
			}
			for (int i = 0; i < id.Length; i++)
			{
				char c = id[i];
				if ((i == 0 || i == id.Length - 1) && c == ' ')
				{
					return false;
				}
				if (c == '"')
				{
					return false;
				}
				if (char.IsControl(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x000F6454 File Offset: 0x000F5454
		internal static void CheckValidHelp(string help)
		{
			if (help == null)
			{
				throw new ArgumentNullException("help");
			}
			if (help.Length > 255)
			{
				throw new ArgumentException(SR.GetString("PerfInvalidHelp", new object[] { 0, 255 }));
			}
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x000F64AC File Offset: 0x000F54AC
		internal static void CheckValidCounterLayout(CounterCreationDataCollection counterData)
		{
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < counterData.Count; i++)
			{
				if (counterData[i].CounterName == null || counterData[i].CounterName.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidCounterName"));
				}
				int num = (int)counterData[i].CounterType;
				if (num == 1073874176 || num == 575735040 || num == 592512256 || num == 574686464 || num == 591463680 || num == 537003008 || num == 549585920 || num == 805438464)
				{
					if (counterData.Count <= i + 1)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					num = (int)counterData[i + 1].CounterType;
					if (!PerformanceCounterLib.IsBaseCounter(num))
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
				}
				else if (PerformanceCounterLib.IsBaseCounter(num))
				{
					if (i == 0)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					num = (int)counterData[i - 1].CounterType;
					if (num != 1073874176 && num != 575735040 && num != 592512256 && num != 574686464 && num != 591463680 && num != 537003008 && num != 549585920 && num != 805438464)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
				}
				if (hashtable.ContainsKey(counterData[i].CounterName))
				{
					throw new ArgumentException(SR.GetString("DuplicateCounterName", new object[] { counterData[i].CounterName }));
				}
				hashtable.Add(counterData[i].CounterName, string.Empty);
				if (counterData[i].CounterHelp == null || counterData[i].CounterHelp.Length == 0)
				{
					counterData[i].CounterHelp = counterData[i].CounterName;
				}
			}
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x000F66A0 File Offset: 0x000F56A0
		public static void Delete(string categoryName)
		{
			PerformanceCounterCategory.CheckValidCategory(categoryName);
			string text = ".";
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Administer, text, categoryName);
			performanceCounterPermission.Demand();
			SharedUtils.CheckNtEnvironment();
			categoryName = categoryName.ToLower(CultureInfo.InvariantCulture);
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxperf.1.0", ref mutex);
				if (!PerformanceCounterLib.IsCustomCategory(text, categoryName))
				{
					throw new InvalidOperationException(SR.GetString("CantDeleteCategory"));
				}
				SharedPerformanceCounter.RemoveAllInstances(categoryName);
				PerformanceCounterLib.UnregisterCategory(categoryName);
				PerformanceCounterLib.CloseAllLibraries();
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x000F673C File Offset: 0x000F573C
		public static bool Exists(string categoryName)
		{
			return PerformanceCounterCategory.Exists(categoryName, ".");
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x000F674C File Offset: 0x000F574C
		public static bool Exists(string categoryName, string machineName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			return PerformanceCounterLib.IsCustomCategory(machineName, categoryName) || PerformanceCounterLib.CategoryExists(machineName, categoryName);
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x000F67E0 File Offset: 0x000F57E0
		internal static string[] GetCounterInstances(string categoryName, string machineName)
		{
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(machineName, categoryName);
			if (categorySample.InstanceNameTable.Count == 0)
			{
				return new string[0];
			}
			string[] array = new string[categorySample.InstanceNameTable.Count];
			categorySample.InstanceNameTable.Keys.CopyTo(array, 0);
			if (array.Length == 1 && array[0].CompareTo("systemdiagnosticsperfcounterlibsingleinstance") == 0)
			{
				return new string[0];
			}
			return array;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x000F6858 File Offset: 0x000F5858
		public PerformanceCounter[] GetCounters()
		{
			if (this.GetInstanceNames().Length != 0)
			{
				throw new ArgumentException(SR.GetString("InstanceNameRequired"));
			}
			return this.GetCounters("");
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000F6880 File Offset: 0x000F5880
		public PerformanceCounter[] GetCounters(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			if (instanceName.Length != 0 && !this.InstanceExists(instanceName))
			{
				throw new InvalidOperationException(SR.GetString("MissingInstance", new object[] { instanceName, this.categoryName }));
			}
			string[] counters = PerformanceCounterLib.GetCounters(this.machineName, this.categoryName);
			PerformanceCounter[] array = new PerformanceCounter[counters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PerformanceCounter(this.categoryName, counters[i], instanceName, this.machineName, true);
			}
			return array;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000F692C File Offset: 0x000F592C
		public static PerformanceCounterCategory[] GetCategories()
		{
			return PerformanceCounterCategory.GetCategories(".");
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x000F6938 File Offset: 0x000F5938
		public static PerformanceCounterCategory[] GetCategories(string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, "*");
			performanceCounterPermission.Demand();
			string[] categories = PerformanceCounterLib.GetCategories(machineName);
			PerformanceCounterCategory[] array = new PerformanceCounterCategory[categories.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PerformanceCounterCategory(categories[i], machineName);
			}
			return array;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x000F69B3 File Offset: 0x000F59B3
		public string[] GetInstanceNames()
		{
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			return PerformanceCounterCategory.GetCounterInstances(this.categoryName, this.machineName);
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x000F69E0 File Offset: 0x000F59E0
		public bool InstanceExists(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
			return categorySample.InstanceNameTable.ContainsKey(instanceName);
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000F6A31 File Offset: 0x000F5A31
		public static bool InstanceExists(string instanceName, string categoryName)
		{
			return PerformanceCounterCategory.InstanceExists(instanceName, categoryName, ".");
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000F6A40 File Offset: 0x000F5A40
		public static bool InstanceExists(string instanceName, string categoryName, string machineName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(categoryName, machineName);
			return performanceCounterCategory.InstanceExists(instanceName);
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000F6AD0 File Offset: 0x000F5AD0
		public InstanceDataCollectionCollection ReadCategory()
		{
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
			return categorySample.ReadCategory();
		}

		// Token: 0x040032F8 RID: 13048
		internal const int MaxNameLength = 80;

		// Token: 0x040032F9 RID: 13049
		internal const int MaxHelpLength = 255;

		// Token: 0x040032FA RID: 13050
		private const string perfMutexName = "netfxperf.1.0";

		// Token: 0x040032FB RID: 13051
		private string categoryName;

		// Token: 0x040032FC RID: 13052
		private string categoryHelp;

		// Token: 0x040032FD RID: 13053
		private string machineName;
	}
}

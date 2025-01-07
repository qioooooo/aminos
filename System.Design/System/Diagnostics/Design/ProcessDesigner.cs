using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ProcessDesigner : ComponentDesigner
	{
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			ICollection collection = null;
			ICollection collection2 = new string[]
			{
				"SynchronizingObject", "EnableRaisingEvents", "StartInfo", "BasePriority", "HandleCount", "Id", "MainWindowHandle", "MainWindowTitle", "MaxWorkingSet", "MinWorkingSet",
				"NonpagedSystemMemorySize", "PagedMemorySize", "PagedSystemMemorySize", "PeakPagedMemorySize", "PeakWorkingSet", "PeakVirtualMemorySize", "PriorityBoostEnabled", "PriorityClass", "PrivateMemorySize", "PrivilegedProcessorTime",
				"ProcessName", "ProcessorAffinity", "Responding", "StartTime", "TotalProcessorTime", "UserProcessorTime", "VirtualMemorySize", "WorkingSet"
			};
			bool[] array = new bool[28];
			array[1] = true;
			array[2] = true;
			RuntimeComponentFilter.FilterProperties(properties, collection, collection2, array);
		}
	}
}

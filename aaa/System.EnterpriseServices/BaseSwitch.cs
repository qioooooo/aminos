using System;
using Microsoft.Win32;

namespace System.EnterpriseServices
{
	// Token: 0x0200009A RID: 154
	internal class BaseSwitch
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000C01C File Offset: 0x0000B01C
		internal static string Path
		{
			get
			{
				return "SOFTWARE\\Microsoft\\COM3\\System.EnterpriseServices";
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000C024 File Offset: 0x0000B024
		internal BaseSwitch(string name)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BaseSwitch.Path);
			this._name = name;
			if (registryKey == null)
			{
				this._value = 0;
				return;
			}
			object value = registryKey.GetValue(name);
			if (value != null)
			{
				this._value = (int)value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000C070 File Offset: 0x0000B070
		protected int Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000C078 File Offset: 0x0000B078
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x040001A0 RID: 416
		private int _value;

		// Token: 0x040001A1 RID: 417
		private string _name;
	}
}

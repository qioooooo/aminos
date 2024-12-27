using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	internal class SerializableRegistryKey
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004F09 File Offset: 0x00003F09
		public SerializableRegistryKey(RegistryKey keyToSave)
		{
			this.CopyFromRegistry(keyToSave);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004F18 File Offset: 0x00003F18
		public void CopyFromRegistry(RegistryKey keyToSave)
		{
			if (keyToSave == null)
			{
				throw new ArgumentNullException("keyToSave");
			}
			this.ValueNames = keyToSave.GetValueNames();
			if (this.ValueNames == null)
			{
				this.ValueNames = new string[0];
			}
			this.Values = new object[this.ValueNames.Length];
			for (int i = 0; i < this.ValueNames.Length; i++)
			{
				this.Values[i] = keyToSave.GetValue(this.ValueNames[i]);
			}
			this.KeyNames = keyToSave.GetSubKeyNames();
			if (this.KeyNames == null)
			{
				this.KeyNames = new string[0];
			}
			this.Keys = new SerializableRegistryKey[this.KeyNames.Length];
			for (int j = 0; j < this.KeyNames.Length; j++)
			{
				this.Keys[j] = new SerializableRegistryKey(keyToSave.OpenSubKey(this.KeyNames[j]));
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004FF0 File Offset: 0x00003FF0
		public void CopyToRegistry(RegistryKey baseKey)
		{
			if (baseKey == null)
			{
				throw new ArgumentNullException("baseKey");
			}
			if (this.Values != null)
			{
				for (int i = 0; i < this.Values.Length; i++)
				{
					baseKey.SetValue(this.ValueNames[i], this.Values[i]);
				}
			}
			if (this.Keys != null)
			{
				for (int j = 0; j < this.Keys.Length; j++)
				{
					RegistryKey registryKey = baseKey.CreateSubKey(this.KeyNames[j]);
					this.Keys[j].CopyToRegistry(registryKey);
				}
			}
		}

		// Token: 0x040000FD RID: 253
		public string[] ValueNames;

		// Token: 0x040000FE RID: 254
		public object[] Values;

		// Token: 0x040000FF RID: 255
		public string[] KeyNames;

		// Token: 0x04000100 RID: 256
		public SerializableRegistryKey[] Keys;
	}
}

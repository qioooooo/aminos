using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000091 RID: 145
	internal class DistinguishedName
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x00019CA2 File Offset: 0x00018CA2
		public DistinguishedName(string dn)
		{
			this.components = Utils.GetDNComponents(dn);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00019CB6 File Offset: 0x00018CB6
		public Component[] Components
		{
			get
			{
				return this.components;
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00019CC0 File Offset: 0x00018CC0
		public bool Equals(DistinguishedName dn)
		{
			bool flag = true;
			if (dn == null || this.components.GetLength(0) != dn.Components.GetLength(0))
			{
				flag = false;
			}
			else
			{
				for (int i = 0; i < this.components.GetLength(0); i++)
				{
					if (Utils.Compare(this.components[i].Name, dn.Components[i].Name) != 0 || Utils.Compare(this.components[i].Value, dn.Components[i].Value) != 0)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00019D5E File Offset: 0x00018D5E
		public override bool Equals(object obj)
		{
			return obj != null && obj is DistinguishedName && this.Equals((DistinguishedName)obj);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00019D7C File Offset: 0x00018D7C
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.components.GetLength(0); i++)
			{
				num = num + this.components[i].Name.ToUpperInvariant().GetHashCode() + this.components[i].Value.ToUpperInvariant().GetHashCode();
			}
			return num;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00019DE0 File Offset: 0x00018DE0
		public override string ToString()
		{
			string text = this.components[0].Name + "=" + this.components[0].Value;
			for (int i = 1; i < this.components.GetLength(0); i++)
			{
				text = string.Concat(new string[]
				{
					text,
					",",
					this.components[i].Name,
					"=",
					this.components[i].Value
				});
			}
			return text;
		}

		// Token: 0x040003F3 RID: 1011
		private Component[] components;
	}
}

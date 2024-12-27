using System;

namespace System.Web.UI.Design
{
	// Token: 0x020003BE RID: 958
	internal sealed class XmlDocumentFieldSchema : IDataSourceFieldSchema
	{
		// Token: 0x06002340 RID: 9024 RVA: 0x000BE7B3 File Offset: 0x000BD7B3
		public XmlDocumentFieldSchema(string name)
		{
			this._name = name;
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x000BE7C2 File Offset: 0x000BD7C2
		public Type DataType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x000BE7CE File Offset: 0x000BD7CE
		public bool Identity
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x000BE7D1 File Offset: 0x000BD7D1
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000BE7D4 File Offset: 0x000BD7D4
		public bool IsUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000BE7D7 File Offset: 0x000BD7D7
		public int Length
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x000BE7DA File Offset: 0x000BD7DA
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x000BE7E2 File Offset: 0x000BD7E2
		public bool Nullable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000BE7E5 File Offset: 0x000BD7E5
		public int Precision
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000BE7E8 File Offset: 0x000BD7E8
		public bool PrimaryKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x000BE7EB File Offset: 0x000BD7EB
		public int Scale
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x04001893 RID: 6291
		private string _name;
	}
}

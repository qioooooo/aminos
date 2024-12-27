using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x0200001B RID: 27
	internal class FileAssociation
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00005D58 File Offset: 0x00004D58
		public FileAssociation(FileAssociationEntry fileAssociationEntry)
		{
			this._extension = fileAssociationEntry.Extension;
			this._description = fileAssociationEntry.Description;
			this._progId = fileAssociationEntry.ProgID;
			this._defaultIcon = fileAssociationEntry.DefaultIcon;
			this._parameter = fileAssociationEntry.Parameter;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005DA7 File Offset: 0x00004DA7
		public string Extension
		{
			get
			{
				return this._extension;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005DAF File Offset: 0x00004DAF
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005DB7 File Offset: 0x00004DB7
		public string ProgID
		{
			get
			{
				return this._progId;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005DBF File Offset: 0x00004DBF
		public string DefaultIcon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005DC7 File Offset: 0x00004DC7
		public string Parameter
		{
			get
			{
				return this._parameter;
			}
		}

		// Token: 0x0400008F RID: 143
		private readonly string _extension;

		// Token: 0x04000090 RID: 144
		private readonly string _description;

		// Token: 0x04000091 RID: 145
		private readonly string _progId;

		// Token: 0x04000092 RID: 146
		private readonly string _defaultIcon;

		// Token: 0x04000093 RID: 147
		private readonly string _parameter;
	}
}

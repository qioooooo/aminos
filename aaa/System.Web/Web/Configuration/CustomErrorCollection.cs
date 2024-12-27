using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001CE RID: 462
	[ConfigurationCollection(typeof(CustomError), AddItemName = "error", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CustomErrorCollection : ConfigurationElementCollection
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x0007AED0 File Offset: 0x00079ED0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CustomErrorCollection._properties;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001A1E RID: 6686 RVA: 0x0007AED8 File Offset: 0x00079ED8
		public string[] AllKeys
		{
			get
			{
				object[] array = base.BaseGetAllKeys();
				string[] array2 = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = ((int)array[i]).ToString(CultureInfo.InvariantCulture);
				}
				return array2;
			}
		}

		// Token: 0x170004EA RID: 1258
		public CustomError this[string statusCode]
		{
			get
			{
				return (CustomError)base.BaseGet(int.Parse(statusCode, CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x170004EB RID: 1259
		public CustomError this[int index]
		{
			get
			{
				return (CustomError)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0007AF61 File Offset: 0x00079F61
		protected override ConfigurationElement CreateNewElement()
		{
			return new CustomError();
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0007AF68 File Offset: 0x00079F68
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((CustomError)element).StatusCode;
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0007AF7A File Offset: 0x00079F7A
		protected override string ElementName
		{
			get
			{
				return "error";
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0007AF81 File Offset: 0x00079F81
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0007AF84 File Offset: 0x00079F84
		public void Add(CustomError customError)
		{
			this.BaseAdd(customError);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0007AF8D File Offset: 0x00079F8D
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0007AF95 File Offset: 0x00079F95
		public CustomError Get(int index)
		{
			return (CustomError)base.BaseGet(index);
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0007AFA3 File Offset: 0x00079FA3
		public CustomError Get(string statusCode)
		{
			return (CustomError)base.BaseGet(int.Parse(statusCode, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x0007AFC0 File Offset: 0x00079FC0
		public string GetKey(int index)
		{
			return ((int)base.BaseGetKey(index)).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0007AFE6 File Offset: 0x00079FE6
		public void Remove(string statusCode)
		{
			base.BaseRemove(int.Parse(statusCode, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0007AFFE File Offset: 0x00079FFE
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0007B007 File Offset: 0x0007A007
		public void Set(CustomError customError)
		{
			base.BaseAdd(customError, false);
		}

		// Token: 0x040017BB RID: 6075
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}

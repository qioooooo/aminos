using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200000E RID: 14
	public class DirectoryAttributeModificationCollection : CollectionBase
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000038ED File Offset: 0x000028ED
		public DirectoryAttributeModificationCollection()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x1700000C RID: 12
		public DirectoryAttributeModification this[int index]
		{
			get
			{
				return (DirectoryAttributeModification)base.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
				}
				base.List[index] = value;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000392F File Offset: 0x0000292F
		public int Add(DirectoryAttributeModification attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			return base.List.Add(attribute);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003950 File Offset: 0x00002950
		public void AddRange(DirectoryAttributeModification[] attributes)
		{
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i] == null)
				{
					throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
				}
			}
			base.InnerList.AddRange(attributes);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000039A0 File Offset: 0x000029A0
		public void AddRange(DirectoryAttributeModificationCollection attributeCollection)
		{
			if (attributeCollection == null)
			{
				throw new ArgumentNullException("attributeCollection");
			}
			int count = attributeCollection.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(attributeCollection[i]);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000039DC File Offset: 0x000029DC
		public bool Contains(DirectoryAttributeModification value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000039EA File Offset: 0x000029EA
		public void CopyTo(DirectoryAttributeModification[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000039F9 File Offset: 0x000029F9
		public int IndexOf(DirectoryAttributeModification value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003A07 File Offset: 0x00002A07
		public void Insert(int index, DirectoryAttributeModification value)
		{
			if (value == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			base.List.Insert(index, value);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003A29 File Offset: 0x00002A29
		public void Remove(DirectoryAttributeModification value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003A38 File Offset: 0x00002A38
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			if (!(value is DirectoryAttributeModification))
			{
				throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryAttributeModification" }), "value");
			}
		}
	}
}

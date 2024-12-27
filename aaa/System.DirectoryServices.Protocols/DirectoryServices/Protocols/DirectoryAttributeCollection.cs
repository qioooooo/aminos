using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200000D RID: 13
	public class DirectoryAttributeCollection : CollectionBase
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003753 File Offset: 0x00002753
		public DirectoryAttributeCollection()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x1700000B RID: 11
		public DirectoryAttribute this[int index]
		{
			get
			{
				return (DirectoryAttribute)base.List[index];
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003795 File Offset: 0x00002795
		public int Add(DirectoryAttribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			return base.List.Add(attribute);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000037B8 File Offset: 0x000027B8
		public void AddRange(DirectoryAttribute[] attributes)
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

		// Token: 0x06000042 RID: 66 RVA: 0x00003808 File Offset: 0x00002808
		public void AddRange(DirectoryAttributeCollection attributeCollection)
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

		// Token: 0x06000043 RID: 67 RVA: 0x00003844 File Offset: 0x00002844
		public bool Contains(DirectoryAttribute value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003852 File Offset: 0x00002852
		public void CopyTo(DirectoryAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003861 File Offset: 0x00002861
		public int IndexOf(DirectoryAttribute value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000386F File Offset: 0x0000286F
		public void Insert(int index, DirectoryAttribute value)
		{
			if (value == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			base.List.Insert(index, value);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003891 File Offset: 0x00002891
		public void Remove(DirectoryAttribute value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000038A0 File Offset: 0x000028A0
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentException(Res.GetString("NullDirectoryAttributeCollection"));
			}
			if (!(value is DirectoryAttribute))
			{
				throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryAttribute" }), "value");
			}
		}
	}
}

using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200008D RID: 141
	public class AttributeMetadataCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x0001808E File Offset: 0x0001708E
		internal AttributeMetadataCollection()
		{
		}

		// Token: 0x17000110 RID: 272
		public AttributeMetadata this[int index]
		{
			get
			{
				return (AttributeMetadata)base.InnerList[index];
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000180AC File Offset: 0x000170AC
		public bool Contains(AttributeMetadata metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException("metadata");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				AttributeMetadata attributeMetadata = (AttributeMetadata)base.InnerList[i];
				string name = attributeMetadata.Name;
				if (Utils.Compare(name, metadata.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018108 File Offset: 0x00017108
		public int IndexOf(AttributeMetadata metadata)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException("metadata");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				AttributeMetadata attributeMetadata = (AttributeMetadata)base.InnerList[i];
				if (Utils.Compare(attributeMetadata.Name, metadata.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018161 File Offset: 0x00017161
		public void CopyTo(AttributeMetadata[] metadata, int index)
		{
			base.InnerList.CopyTo(metadata, index);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00018170 File Offset: 0x00017170
		internal int Add(AttributeMetadata metadata)
		{
			return base.InnerList.Add(metadata);
		}
	}
}

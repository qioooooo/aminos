using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006FE RID: 1790
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TransformerTypeCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06005765 RID: 22373 RVA: 0x00160CEA File Offset: 0x0015FCEA
		public TransformerTypeCollection()
		{
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x00160CF2 File Offset: 0x0015FCF2
		public TransformerTypeCollection(ICollection transformerTypes)
		{
			this.Initialize(null, transformerTypes);
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x00160D02 File Offset: 0x0015FD02
		public TransformerTypeCollection(TransformerTypeCollection existingTransformerTypes, ICollection transformerTypes)
		{
			this.Initialize(existingTransformerTypes, transformerTypes);
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x00160D14 File Offset: 0x0015FD14
		internal int Add(Type value)
		{
			if (!value.IsSubclassOf(typeof(WebPartTransformer)))
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerAttribute_NotTransformer", new object[] { value.Name }));
			}
			return base.InnerList.Add(value);
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x00160D60 File Offset: 0x0015FD60
		private void Initialize(TransformerTypeCollection existingTransformerTypes, ICollection transformerTypes)
		{
			if (existingTransformerTypes != null)
			{
				foreach (object obj in existingTransformerTypes)
				{
					Type type = (Type)obj;
					base.InnerList.Add(type);
				}
			}
			if (transformerTypes != null)
			{
				foreach (object obj2 in transformerTypes)
				{
					if (obj2 == null)
					{
						throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "transformerTypes");
					}
					if (!(obj2 is Type))
					{
						throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "Type" }), "transformerTypes");
					}
					if (!((Type)obj2).IsSubclassOf(typeof(WebPartTransformer)))
					{
						throw new ArgumentException(SR.GetString("WebPartTransformerAttribute_NotTransformer", new object[] { ((Type)obj2).Name }), "transformerTypes");
					}
					base.InnerList.Add(obj2);
				}
			}
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x00160EA4 File Offset: 0x0015FEA4
		public bool Contains(Type value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x00160EB2 File Offset: 0x0015FEB2
		public int IndexOf(Type value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x1700168B RID: 5771
		public Type this[int index]
		{
			get
			{
				return (Type)base.InnerList[index];
			}
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x00160ED3 File Offset: 0x0015FED3
		public void CopyTo(Type[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x04002F9C RID: 12188
		public static readonly TransformerTypeCollection Empty = new TransformerTypeCollection();
	}
}

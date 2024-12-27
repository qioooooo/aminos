using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000025 RID: 37
	internal struct StorageDescriptor
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000CCB0 File Offset: 0x0000BCB0
		public static StorageDescriptor None()
		{
			return default(StorageDescriptor);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000CCC8 File Offset: 0x0000BCC8
		public static StorageDescriptor Stack(Type itemStorageType, bool isCached)
		{
			return new StorageDescriptor
			{
				location = ItemLocation.Stack,
				itemStorageType = itemStorageType,
				isCached = isCached
			};
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000CCF8 File Offset: 0x0000BCF8
		public static StorageDescriptor Parameter(int paramIndex, Type itemStorageType, bool isCached)
		{
			return new StorageDescriptor
			{
				location = ItemLocation.Parameter,
				locationObject = paramIndex,
				itemStorageType = itemStorageType,
				isCached = isCached
			};
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000CD34 File Offset: 0x0000BD34
		public static StorageDescriptor Local(LocalBuilder loc, Type itemStorageType, bool isCached)
		{
			return new StorageDescriptor
			{
				location = ItemLocation.Local,
				locationObject = loc,
				itemStorageType = itemStorageType,
				isCached = isCached
			};
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000CD6C File Offset: 0x0000BD6C
		public static StorageDescriptor Current(LocalBuilder locIter, Type itemStorageType)
		{
			return new StorageDescriptor
			{
				location = ItemLocation.Current,
				locationObject = locIter,
				itemStorageType = itemStorageType
			};
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000CD9C File Offset: 0x0000BD9C
		public static StorageDescriptor Global(MethodInfo methGlobal, Type itemStorageType, bool isCached)
		{
			return new StorageDescriptor
			{
				location = ItemLocation.Global,
				locationObject = methGlobal,
				itemStorageType = itemStorageType,
				isCached = isCached
			};
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000CDD2 File Offset: 0x0000BDD2
		public StorageDescriptor ToStack()
		{
			return StorageDescriptor.Stack(this.itemStorageType, this.isCached);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000CDE5 File Offset: 0x0000BDE5
		public StorageDescriptor ToLocal(LocalBuilder loc)
		{
			return StorageDescriptor.Local(loc, this.itemStorageType, this.isCached);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000CDFC File Offset: 0x0000BDFC
		public StorageDescriptor ToStorageType(Type itemStorageType)
		{
			StorageDescriptor storageDescriptor = this;
			storageDescriptor.itemStorageType = itemStorageType;
			return storageDescriptor;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000CE19 File Offset: 0x0000BE19
		public ItemLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000CE21 File Offset: 0x0000BE21
		public int ParameterLocation
		{
			get
			{
				return (int)this.locationObject;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000CE2E File Offset: 0x0000BE2E
		public LocalBuilder LocalLocation
		{
			get
			{
				return this.locationObject as LocalBuilder;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000CE3B File Offset: 0x0000BE3B
		public LocalBuilder CurrentLocation
		{
			get
			{
				return this.locationObject as LocalBuilder;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000CE48 File Offset: 0x0000BE48
		public MethodInfo GlobalLocation
		{
			get
			{
				return this.locationObject as MethodInfo;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000CE55 File Offset: 0x0000BE55
		public bool IsCached
		{
			get
			{
				return this.isCached;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000CE5D File Offset: 0x0000BE5D
		public Type ItemStorageType
		{
			get
			{
				return this.itemStorageType;
			}
		}

		// Token: 0x04000255 RID: 597
		private ItemLocation location;

		// Token: 0x04000256 RID: 598
		private object locationObject;

		// Token: 0x04000257 RID: 599
		private Type itemStorageType;

		// Token: 0x04000258 RID: 600
		private bool isCached;
	}
}

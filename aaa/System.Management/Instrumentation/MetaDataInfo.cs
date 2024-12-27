using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B5 RID: 181
	internal class MetaDataInfo : IDisposable
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x000264D7 File Offset: 0x000254D7
		public MetaDataInfo(Assembly assembly)
			: this(assembly.Location)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000264E8 File Offset: 0x000254E8
		public MetaDataInfo(string assemblyName)
		{
			Guid guid = new Guid(((GuidAttribute)Attribute.GetCustomAttribute(typeof(IMetaDataImportInternalOnly), typeof(GuidAttribute), false)).Value);
			IMetaDataDispenser metaDataDispenser = (IMetaDataDispenser)new CorMetaDataDispenser();
			this.importInterface = (IMetaDataImportInternalOnly)metaDataDispenser.OpenScope(assemblyName, 0U, ref guid);
			Marshal.ReleaseComObject(metaDataDispenser);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00026554 File Offset: 0x00025554
		private void InitNameAndMvid()
		{
			if (this.name == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Capacity = 0;
				uint num;
				this.importInterface.GetScopeProps(stringBuilder, (uint)stringBuilder.Capacity, out num, out this.mvid);
				stringBuilder.Capacity = (int)num;
				this.importInterface.GetScopeProps(stringBuilder, (uint)stringBuilder.Capacity, out num, out this.mvid);
				this.name = stringBuilder.ToString();
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x000265BD File Offset: 0x000255BD
		public string Name
		{
			get
			{
				this.InitNameAndMvid();
				return this.name;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x000265CB File Offset: 0x000255CB
		public Guid Mvid
		{
			get
			{
				this.InitNameAndMvid();
				return this.mvid;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000265D9 File Offset: 0x000255D9
		public void Dispose()
		{
			if (this.importInterface == null)
			{
				Marshal.ReleaseComObject(this.importInterface);
			}
			this.importInterface = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000265FC File Offset: 0x000255FC
		~MetaDataInfo()
		{
			this.Dispose();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00026628 File Offset: 0x00025628
		public static Guid GetMvid(Assembly assembly)
		{
			Guid guid;
			using (MetaDataInfo metaDataInfo = new MetaDataInfo(assembly))
			{
				guid = metaDataInfo.Mvid;
			}
			return guid;
		}

		// Token: 0x040002CE RID: 718
		private IMetaDataImportInternalOnly importInterface;

		// Token: 0x040002CF RID: 719
		private string name;

		// Token: 0x040002D0 RID: 720
		private Guid mvid;
	}
}

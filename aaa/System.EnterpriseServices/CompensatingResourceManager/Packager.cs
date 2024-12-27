using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AC RID: 172
	internal class Packager
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x0000CFCC File Offset: 0x0000BFCC
		private static void Init()
		{
			if (!Packager._initialized)
			{
				lock (typeof(Packager))
				{
					if (!Packager._initialized)
					{
						StreamingContext streamingContext = new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence);
						Packager._ser = new BinaryFormatter(null, streamingContext);
						Packager._initialized = true;
					}
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000D034 File Offset: 0x0000C034
		internal static object Deserialize(BlobPackage b)
		{
			Packager.Init();
			byte[] bits = b.GetBits();
			return Packager._ser.Deserialize(new MemoryStream(bits, false));
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000D060 File Offset: 0x0000C060
		internal static byte[] Serialize(object o)
		{
			Packager.Init();
			MemoryStream memoryStream = new MemoryStream();
			Packager._ser.Serialize(memoryStream, o);
			return memoryStream.GetBuffer();
		}

		// Token: 0x040001DC RID: 476
		private static BinaryFormatter _ser;

		// Token: 0x040001DD RID: 477
		private static volatile bool _initialized;
	}
}

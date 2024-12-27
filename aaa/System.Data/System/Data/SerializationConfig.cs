using System;
using System.Collections.Generic;
using System.Configuration;

namespace System.Data
{
	// Token: 0x0200019E RID: 414
	internal sealed class SerializationConfig
	{
		// Token: 0x06001846 RID: 6214 RVA: 0x00236B00 File Offset: 0x00235F00
		private SerializationConfig()
		{
			AllowedTypesSectionHandler.Data data = ((AllowedTypesSectionHandler.Data)ConfigurationManager.GetSection("system.data.dataset.serialization/allowedTypes")) ?? new AllowedTypesSectionHandler.Data();
			this.m_auditMode = data.AuditMode;
			foreach (string text in data.AllowedTypes)
			{
				if (text != null && !(text.Trim() == ""))
				{
					this.m_allowedTypeList.Add(Type.GetType(text.Trim(), true));
				}
			}
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00236BB4 File Offset: 0x00235FB4
		private static void EnsureInitialized()
		{
			if (SerializationConfig.s_instance == null)
			{
				SerializationConfig.s_instance = new SerializationConfig();
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00236BD8 File Offset: 0x00235FD8
		public static bool IsAuditMode()
		{
			SerializationConfig.EnsureInitialized();
			return SerializationConfig.s_instance.m_auditMode;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00236BF8 File Offset: 0x00235FF8
		public static bool IsTypeAllowed(Type type)
		{
			if (type == null)
			{
				return true;
			}
			SerializationConfig.EnsureInitialized();
			return SerializationConfig.s_instance.m_allowedTypeList.Contains(type);
		}

		// Token: 0x04000D10 RID: 3344
		private static volatile SerializationConfig s_instance;

		// Token: 0x04000D11 RID: 3345
		private readonly bool m_auditMode;

		// Token: 0x04000D12 RID: 3346
		private readonly List<Type> m_allowedTypeList = new List<Type>();
	}
}

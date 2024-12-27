using System;
using System.Globalization;
using System.IO;

namespace System.Reflection.Emit
{
	// Token: 0x02000825 RID: 2085
	[Serializable]
	internal class ModuleBuilderData
	{
		// Token: 0x06004B7D RID: 19325 RVA: 0x00107FDF File Offset: 0x00106FDF
		internal ModuleBuilderData(ModuleBuilder module, string strModuleName, string strFileName)
		{
			this.Init(module, strModuleName, strFileName);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x00107FF0 File Offset: 0x00106FF0
		internal virtual void Init(ModuleBuilder module, string strModuleName, string strFileName)
		{
			this.m_fGlobalBeenCreated = false;
			this.m_fHasGlobal = false;
			this.m_globalTypeBuilder = new TypeBuilder(module);
			this.m_module = module;
			this.m_strModuleName = strModuleName;
			this.m_tkFile = 0;
			this.m_isSaved = false;
			this.m_embeddedRes = null;
			this.m_strResourceFileName = null;
			this.m_resourceBytes = null;
			if (strFileName == null)
			{
				this.m_strFileName = strModuleName;
				this.m_isTransient = true;
			}
			else
			{
				string extension = Path.GetExtension(strFileName);
				if (extension == null || extension == string.Empty)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NoModuleFileExtension"), new object[] { strFileName }));
				}
				this.m_strFileName = strFileName;
				this.m_isTransient = false;
			}
			this.m_module.InternalSetModuleProps(this.m_strModuleName);
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x001080B7 File Offset: 0x001070B7
		internal virtual bool IsTransient()
		{
			return this.m_isTransient;
		}

		// Token: 0x04002639 RID: 9785
		internal const string MULTI_BYTE_VALUE_CLASS = "$ArrayType$";

		// Token: 0x0400263A RID: 9786
		internal string m_strModuleName;

		// Token: 0x0400263B RID: 9787
		internal string m_strFileName;

		// Token: 0x0400263C RID: 9788
		internal bool m_fGlobalBeenCreated;

		// Token: 0x0400263D RID: 9789
		internal bool m_fHasGlobal;

		// Token: 0x0400263E RID: 9790
		[NonSerialized]
		internal TypeBuilder m_globalTypeBuilder;

		// Token: 0x0400263F RID: 9791
		[NonSerialized]
		internal ModuleBuilder m_module;

		// Token: 0x04002640 RID: 9792
		internal int m_tkFile;

		// Token: 0x04002641 RID: 9793
		internal bool m_isSaved;

		// Token: 0x04002642 RID: 9794
		[NonSerialized]
		internal ResWriterData m_embeddedRes;

		// Token: 0x04002643 RID: 9795
		internal bool m_isTransient;

		// Token: 0x04002644 RID: 9796
		internal string m_strResourceFileName;

		// Token: 0x04002645 RID: 9797
		internal byte[] m_resourceBytes;
	}
}

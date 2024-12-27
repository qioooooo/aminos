using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Xml.Serialization.Configuration;

namespace System.Xml.Serialization
{
	// Token: 0x020002B4 RID: 692
	internal sealed class XmlSerializerCompilerParameters
	{
		// Token: 0x06002134 RID: 8500 RVA: 0x0009D721 File Offset: 0x0009C721
		private XmlSerializerCompilerParameters(CompilerParameters parameters, bool needTempDirAccess)
		{
			this.needTempDirAccess = needTempDirAccess;
			this.parameters = parameters;
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002135 RID: 8501 RVA: 0x0009D737 File Offset: 0x0009C737
		internal bool IsNeedTempDirAccess
		{
			get
			{
				return this.needTempDirAccess;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x0009D73F File Offset: 0x0009C73F
		internal CompilerParameters CodeDomParameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0009D748 File Offset: 0x0009C748
		internal static XmlSerializerCompilerParameters Create(string location)
		{
			CompilerParameters compilerParameters = new CompilerParameters();
			compilerParameters.GenerateInMemory = true;
			if (string.IsNullOrEmpty(location))
			{
				XmlSerializerSection xmlSerializerSection = ConfigurationManager.GetSection(ConfigurationStrings.XmlSerializerSectionPath) as XmlSerializerSection;
				location = ((xmlSerializerSection == null) ? location : xmlSerializerSection.TempFilesLocation);
				if (!string.IsNullOrEmpty(location))
				{
					location = location.Trim();
				}
			}
			compilerParameters.TempFiles = new TempFileCollection(location);
			return new XmlSerializerCompilerParameters(compilerParameters, string.IsNullOrEmpty(location));
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0009D7B0 File Offset: 0x0009C7B0
		internal static XmlSerializerCompilerParameters Create(CompilerParameters parameters, bool needTempDirAccess)
		{
			return new XmlSerializerCompilerParameters(parameters, needTempDirAccess);
		}

		// Token: 0x04001442 RID: 5186
		private bool needTempDirAccess;

		// Token: 0x04001443 RID: 5187
		private CompilerParameters parameters;
	}
}

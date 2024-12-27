using System;
using System.Configuration;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x02000230 RID: 560
	internal class ProtocolsConfigurationEntry
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x00087272 File Offset: 0x00086272
		internal ProtocolsConfigurationEntry(string id, string processHandlerType, string appDomainHandlerType, bool validate, string configFileName, int configFileLine)
		{
			this._id = id;
			this._processHandlerTypeName = processHandlerType;
			this._appDomainHandlerTypeName = appDomainHandlerType;
			this._configFileName = configFileName;
			this._configFileLine = configFileLine;
			if (validate)
			{
				this.ValidateTypes();
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000872AC File Offset: 0x000862AC
		private void ValidateTypes()
		{
			if (this._typesValidated)
			{
				return;
			}
			Type type;
			try
			{
				type = Type.GetType(this._processHandlerTypeName, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(ex.Message, ex, this._configFileName, this._configFileLine);
			}
			HandlerBase.CheckAssignableType(this._configFileName, this._configFileLine, typeof(ProcessProtocolHandler), type);
			Type type2;
			try
			{
				type2 = Type.GetType(this._appDomainHandlerTypeName, true);
			}
			catch (Exception ex2)
			{
				throw new ConfigurationErrorsException(ex2.Message, ex2, this._configFileName, this._configFileLine);
			}
			HandlerBase.CheckAssignableType(this._configFileName, this._configFileLine, typeof(AppDomainProtocolHandler), type2);
			this._processHandlerType = type;
			this._appDomainHandlerType = type2;
			this._typesValidated = true;
		}

		// Token: 0x040019A4 RID: 6564
		private string _id;

		// Token: 0x040019A5 RID: 6565
		private string _processHandlerTypeName;

		// Token: 0x040019A6 RID: 6566
		private Type _processHandlerType;

		// Token: 0x040019A7 RID: 6567
		private string _appDomainHandlerTypeName;

		// Token: 0x040019A8 RID: 6568
		private Type _appDomainHandlerType;

		// Token: 0x040019A9 RID: 6569
		private bool _typesValidated;

		// Token: 0x040019AA RID: 6570
		private string _configFileName;

		// Token: 0x040019AB RID: 6571
		private int _configFileLine;
	}
}

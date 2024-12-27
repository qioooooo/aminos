using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000234 RID: 564
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProtocolElement : ConfigurationElement
	{
		// Token: 0x06001E31 RID: 7729 RVA: 0x000874B0 File Offset: 0x000864B0
		static ProtocolElement()
		{
			ProtocolElement._properties.Add(ProtocolElement._propName);
			ProtocolElement._properties.Add(ProtocolElement._propProcessHandlerType);
			ProtocolElement._properties.Add(ProtocolElement._propAppDomainHandlerType);
			ProtocolElement._properties.Add(ProtocolElement._propValidate);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00087577 File Offset: 0x00086577
		public ProtocolElement(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			base[ProtocolElement._propName] = name;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0008759E File Offset: 0x0008659E
		public ProtocolElement()
		{
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x000875A6 File Offset: 0x000865A6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProtocolElement._properties;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000875AD File Offset: 0x000865AD
		// (set) Token: 0x06001E36 RID: 7734 RVA: 0x000875BF File Offset: 0x000865BF
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[ProtocolElement._propName];
			}
			set
			{
				base[ProtocolElement._propName] = value;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000875CD File Offset: 0x000865CD
		// (set) Token: 0x06001E38 RID: 7736 RVA: 0x000875DF File Offset: 0x000865DF
		[ConfigurationProperty("processHandlerType")]
		public string ProcessHandlerType
		{
			get
			{
				return (string)base[ProtocolElement._propProcessHandlerType];
			}
			set
			{
				base[ProtocolElement._propProcessHandlerType] = value;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000875ED File Offset: 0x000865ED
		// (set) Token: 0x06001E3A RID: 7738 RVA: 0x000875FF File Offset: 0x000865FF
		[ConfigurationProperty("appDomainHandlerType")]
		public string AppDomainHandlerType
		{
			get
			{
				return (string)base[ProtocolElement._propAppDomainHandlerType];
			}
			set
			{
				base[ProtocolElement._propAppDomainHandlerType] = value;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0008760D File Offset: 0x0008660D
		// (set) Token: 0x06001E3C RID: 7740 RVA: 0x0008761F File Offset: 0x0008661F
		[ConfigurationProperty("validate", DefaultValue = false)]
		public bool Validate
		{
			get
			{
				return (bool)base[ProtocolElement._propValidate];
			}
			set
			{
				base[ProtocolElement._propValidate] = value;
			}
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x00087634 File Offset: 0x00086634
		private void ValidateTypes()
		{
			Type type;
			try
			{
				type = Type.GetType(this.ProcessHandlerType, true);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(ex.Message, ex, base.ElementInformation.Properties["ProcessHandlerType"].Source, base.ElementInformation.Properties["ProcessHandlerType"].LineNumber);
			}
			ConfigUtil.CheckAssignableType(typeof(ProcessProtocolHandler), type, this, "ProcessHandlerType");
			Type type2;
			try
			{
				type2 = Type.GetType(this.AppDomainHandlerType, true);
			}
			catch (Exception ex2)
			{
				throw new ConfigurationErrorsException(ex2.Message, ex2, base.ElementInformation.Properties["AppDomainHandlerType"].Source, base.ElementInformation.Properties["AppDomainHandlerType"].LineNumber);
			}
			ConfigUtil.CheckAssignableType(typeof(AppDomainProtocolHandler), type2, this, "AppDomainHandlerType");
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0008772C File Offset: 0x0008672C
		protected override void PostDeserialize()
		{
			if (this.Validate)
			{
				this.ValidateTypes();
			}
		}

		// Token: 0x040019AF RID: 6575
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040019B0 RID: 6576
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040019B1 RID: 6577
		private static readonly ConfigurationProperty _propProcessHandlerType = new ConfigurationProperty("processHandlerType", typeof(string), null);

		// Token: 0x040019B2 RID: 6578
		private static readonly ConfigurationProperty _propAppDomainHandlerType = new ConfigurationProperty("appDomainHandlerType", typeof(string), null);

		// Token: 0x040019B3 RID: 6579
		private static readonly ConfigurationProperty _propValidate = new ConfigurationProperty("validate", typeof(bool), false);
	}
}

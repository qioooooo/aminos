using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001C5 RID: 453
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CodeSubDirectory : ConfigurationElement
	{
		// Token: 0x060019AF RID: 6575 RVA: 0x000795C8 File Offset: 0x000785C8
		static CodeSubDirectory()
		{
			CodeSubDirectory._properties.Add(CodeSubDirectory._propDirectoryName);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00079608 File Offset: 0x00078608
		internal CodeSubDirectory()
		{
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00079610 File Offset: 0x00078610
		public CodeSubDirectory(string directoryName)
		{
			this.DirectoryName = directoryName;
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0007961F File Offset: 0x0007861F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CodeSubDirectory._properties;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x00079626 File Offset: 0x00078626
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x00079638 File Offset: 0x00078638
		[TypeConverter(typeof(WhiteSpaceTrimStringConverter))]
		[ConfigurationProperty("directoryName", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string DirectoryName
		{
			get
			{
				return (string)base[CodeSubDirectory._propDirectoryName];
			}
			set
			{
				base[CodeSubDirectory._propDirectoryName] = value;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x00079646 File Offset: 0x00078646
		internal string AssemblyName
		{
			get
			{
				return this.DirectoryName;
			}
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00079650 File Offset: 0x00078650
		internal void DoRuntimeValidation()
		{
			string directoryName = this.DirectoryName;
			if (BuildManager.IsPrecompiledApp)
			{
				return;
			}
			if (!Util.IsValidFileName(directoryName))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_CodeSubDirectory", new object[] { directoryName }), base.ElementInformation.Properties["directoryName"].Source, base.ElementInformation.Properties["directoryName"].LineNumber);
			}
			VirtualPath virtualPath = HttpRuntime.CodeDirectoryVirtualPath.SimpleCombineWithDir(directoryName);
			if (!VirtualPathProvider.DirectoryExistsNoThrow(virtualPath))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_CodeSubDirectory_Not_Exist", new object[] { virtualPath }), base.ElementInformation.Properties["directoryName"].Source, base.ElementInformation.Properties["directoryName"].LineNumber);
			}
			string text = virtualPath.MapPathInternal();
			FindFileData findFileData;
			FindFileData.FindFile(text, out findFileData);
			if (!StringUtil.EqualsIgnoreCase(directoryName, findFileData.FileNameLong))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_CodeSubDirectory", new object[] { directoryName }), base.ElementInformation.Properties["directoryName"].Source, base.ElementInformation.Properties["directoryName"].LineNumber);
			}
			if (BuildManager.IsReservedAssemblyName(directoryName))
			{
				throw new ConfigurationErrorsException(SR.GetString("Reserved_AssemblyName", new object[] { directoryName }), base.ElementInformation.Properties["directoryName"].Source, base.ElementInformation.Properties["directoryName"].LineNumber);
			}
		}

		// Token: 0x04001786 RID: 6022
		private const string dirNameAttribName = "directoryName";

		// Token: 0x04001787 RID: 6023
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001788 RID: 6024
		private static readonly ConfigurationProperty _propDirectoryName = new ConfigurationProperty("directoryName", typeof(string), null, StdValidatorsAndConverters.WhiteSpaceTrimStringConverter, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
	}
}

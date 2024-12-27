using System;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Application.Win32InterOp;
using System.Deployment.Internal.CodeSigning;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x0200001E RID: 30
	internal class AssemblyManifest
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x0000600E File Offset: 0x0000500E
		public AssemblyManifest(FileStream fileStream)
		{
			this.LoadCMSFromStream(fileStream);
			this._rawXmlFilePath = fileStream.Name;
			this._manifestSourceFormat = ManifestSourceFormat.XmlFile;
			this._sizeInBytes = (ulong)fileStream.Length;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006043 File Offset: 0x00005043
		public AssemblyManifest(Stream stream)
		{
			this.LoadCMSFromStream(stream);
			this._manifestSourceFormat = ManifestSourceFormat.Stream;
			this._sizeInBytes = (ulong)stream.Length;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000606C File Offset: 0x0000506C
		public AssemblyManifest(string filePath)
		{
			string extension = Path.GetExtension(filePath);
			StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
			if (extension.Equals(".application", stringComparison) || extension.Equals(".manifest", stringComparison))
			{
				this.LoadFromRawXmlFile(filePath);
				return;
			}
			if (extension.Equals(".dll", stringComparison) || extension.Equals(".exe", stringComparison))
			{
				this.LoadFromInternalManifestFile(filePath);
				return;
			}
			this.LoadFromUnknownFormatFile(filePath);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000060DE File Offset: 0x000050DE
		public AssemblyManifest(ICMS cms)
		{
			if (cms == null)
			{
				throw new ArgumentNullException("cms");
			}
			this._cms = cms;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00006102 File Offset: 0x00005102
		public string RawXmlFilePath
		{
			get
			{
				return this._rawXmlFilePath;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000610A File Offset: 0x0000510A
		public byte[] RawXmlBytes
		{
			get
			{
				return this._rawXmlBytes;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00006114 File Offset: 0x00005114
		public DefinitionIdentity Identity
		{
			get
			{
				if (this._identity == null && this._cms != null)
				{
					DefinitionIdentity definitionIdentity;
					if (this._cms.Identity == null)
					{
						definitionIdentity = new DefinitionIdentity();
					}
					else
					{
						definitionIdentity = new DefinitionIdentity(this._cms.Identity);
					}
					Interlocked.CompareExchange(ref this._identity, definitionIdentity, null);
				}
				return (DefinitionIdentity)this._identity;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006172 File Offset: 0x00005172
		public ulong SizeInBytes
		{
			get
			{
				return this._sizeInBytes;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000617A File Offset: 0x0000517A
		public DefinitionIdentity Id1Identity
		{
			get
			{
				return this._id1Identity;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006182 File Offset: 0x00005182
		public DefinitionIdentity ComplibIdentity
		{
			get
			{
				return this._complibIdentity;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000618A File Offset: 0x0000518A
		public bool Id1ManifestPresent
		{
			get
			{
				return this._id1ManifestPresent;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006192 File Offset: 0x00005192
		public string Id1RequestedExecutionLevel
		{
			get
			{
				return this._id1RequestedExecutionLevel;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000619C File Offset: 0x0000519C
		public uint ManifestFlags
		{
			get
			{
				if (this._manifestFlags == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					uint manifestFlags = metadataSectionEntry.ManifestFlags;
					Interlocked.CompareExchange(ref this._manifestFlags, manifestFlags, null);
				}
				return (uint)this._manifestFlags;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000061F4 File Offset: 0x000051F4
		public string RequestedExecutionLevel
		{
			get
			{
				if (this._requestedExecutionLevel == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					string requestedExecutionLevel = metadataSectionEntry.RequestedExecutionLevel;
					Interlocked.CompareExchange(ref this._requestedExecutionLevel, requestedExecutionLevel, null);
				}
				return (string)this._requestedExecutionLevel;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00006244 File Offset: 0x00005244
		public bool RequestedExecutionLevelUIAccess
		{
			get
			{
				if (this._requestedExecutionLevelUIAccess == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					bool requestedExecutionLevelUIAccess = metadataSectionEntry.RequestedExecutionLevelUIAccess;
					Interlocked.CompareExchange(ref this._requestedExecutionLevelUIAccess, requestedExecutionLevelUIAccess, null);
				}
				return (bool)this._requestedExecutionLevelUIAccess;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00006299 File Offset: 0x00005299
		public bool Application
		{
			get
			{
				return (this.ManifestFlags & 4U) != 0U;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000062A9 File Offset: 0x000052A9
		public bool UseManifestForTrust
		{
			get
			{
				return (this.ManifestFlags & 8U) != 0U;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000062BC File Offset: 0x000052BC
		public Description Description
		{
			get
			{
				if (this._description == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					IDescriptionMetadataEntry descriptionData = metadataSectionEntry.DescriptionData;
					if (descriptionData != null)
					{
						Description description = new Description(descriptionData.AllData);
						Interlocked.CompareExchange(ref this._description, description, null);
					}
				}
				return (Description)this._description;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000631C File Offset: 0x0000531C
		public Deployment Deployment
		{
			get
			{
				if (this._deployment == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					IDeploymentMetadataEntry deploymentData = metadataSectionEntry.DeploymentData;
					if (deploymentData != null)
					{
						Deployment deployment = new Deployment(deploymentData.AllData);
						Interlocked.CompareExchange(ref this._deployment, deployment, null);
					}
				}
				return (Deployment)this._deployment;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000637C File Offset: 0x0000537C
		public DependentOS DependentOS
		{
			get
			{
				if (this._dependentOS == null && this._cms != null)
				{
					IMetadataSectionEntry metadataSectionEntry = (IMetadataSectionEntry)this._cms.MetadataSectionEntry;
					IDependentOSMetadataEntry dependentOSData = metadataSectionEntry.DependentOSData;
					if (dependentOSData != null)
					{
						DependentOS dependentOS = new DependentOS(dependentOSData.AllData);
						Interlocked.CompareExchange(ref this._dependentOS, dependentOS, null);
					}
				}
				return (DependentOS)this._dependentOS;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000063DC File Offset: 0x000053DC
		public DependentAssembly[] DependentAssemblies
		{
			get
			{
				if (this._dependentAssemblies == null)
				{
					ISection section = ((this._cms != null) ? this._cms.AssemblyReferenceSection : null);
					uint num = ((section != null) ? section.Count : 0U);
					DependentAssembly[] array = new DependentAssembly[num];
					if (num > 0U)
					{
						uint num2 = 0U;
						IAssemblyReferenceEntry[] array2 = new IAssemblyReferenceEntry[num];
						IEnumUnknown enumUnknown = (IEnumUnknown)section._NewEnum;
						int num3 = enumUnknown.Next(num, array2, ref num2);
						Marshal.ThrowExceptionForHR(num3);
						if (num2 != num)
						{
							throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_IsoEnumFetchNotEqualToCount"));
						}
						for (uint num4 = 0U; num4 < num; num4 += 1U)
						{
							array[(int)((UIntPtr)num4)] = new DependentAssembly(array2[(int)((UIntPtr)num4)].AllData);
						}
					}
					Interlocked.CompareExchange(ref this._dependentAssemblies, array, null);
				}
				return (DependentAssembly[])this._dependentAssemblies;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000064A8 File Offset: 0x000054A8
		public FileAssociation[] FileAssociations
		{
			get
			{
				if (this._fileAssociations == null)
				{
					ISection section = ((this._cms != null) ? this._cms.FileAssociationSection : null);
					uint num = ((section != null) ? section.Count : 0U);
					FileAssociation[] array = new FileAssociation[num];
					if (num > 0U)
					{
						uint num2 = 0U;
						IFileAssociationEntry[] array2 = new IFileAssociationEntry[num];
						IEnumUnknown enumUnknown = (IEnumUnknown)section._NewEnum;
						int num3 = enumUnknown.Next(num, array2, ref num2);
						Marshal.ThrowExceptionForHR(num3);
						if (num2 != num)
						{
							throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_IsoEnumFetchNotEqualToCount"));
						}
						for (uint num4 = 0U; num4 < num; num4 += 1U)
						{
							array[(int)((UIntPtr)num4)] = new FileAssociation(array2[(int)((UIntPtr)num4)].AllData);
						}
					}
					Interlocked.CompareExchange(ref this._fileAssociations, array, null);
				}
				return (FileAssociation[])this._fileAssociations;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006574 File Offset: 0x00005574
		public File[] Files
		{
			get
			{
				if (this._files == null)
				{
					ISection section = ((this._cms != null) ? this._cms.FileSection : null);
					uint num = ((section != null) ? section.Count : 0U);
					File[] array = new File[num];
					if (num > 0U)
					{
						uint num2 = 0U;
						IFileEntry[] array2 = new IFileEntry[num];
						IEnumUnknown enumUnknown = (IEnumUnknown)section._NewEnum;
						int num3 = enumUnknown.Next(num, array2, ref num2);
						Marshal.ThrowExceptionForHR(num3);
						if (num2 != num)
						{
							throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_IsoEnumFetchNotEqualToCount"));
						}
						for (uint num4 = 0U; num4 < num; num4 += 1U)
						{
							array[(int)((UIntPtr)num4)] = new File(array2[(int)((UIntPtr)num4)].AllData);
						}
					}
					Interlocked.CompareExchange(ref this._files, array, null);
				}
				return (File[])this._files;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006640 File Offset: 0x00005640
		public EntryPoint[] EntryPoints
		{
			get
			{
				if (this._entryPoints == null)
				{
					ISection section = ((this._cms != null) ? this._cms.EntryPointSection : null);
					uint num = ((section != null) ? section.Count : 0U);
					EntryPoint[] array = new EntryPoint[num];
					if (num > 0U)
					{
						uint num2 = 0U;
						IEntryPointEntry[] array2 = new IEntryPointEntry[num];
						IEnumUnknown enumUnknown = (IEnumUnknown)section._NewEnum;
						int num3 = enumUnknown.Next(num, array2, ref num2);
						Marshal.ThrowExceptionForHR(num3);
						if (num2 != num)
						{
							throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_IsoEnumFetchNotEqualToCount"));
						}
						for (uint num4 = 0U; num4 < num; num4 += 1U)
						{
							array[(int)((UIntPtr)num4)] = new EntryPoint(array2[(int)((UIntPtr)num4)].AllData, this);
						}
					}
					Interlocked.CompareExchange(ref this._entryPoints, array, null);
				}
				return (EntryPoint[])this._entryPoints;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000670A File Offset: 0x0000570A
		public DependentAssembly MainDependentAssembly
		{
			get
			{
				return this.DependentAssemblies[0];
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006714 File Offset: 0x00005714
		public bool RequiredHashMissing
		{
			get
			{
				return this._unhashedDependencyPresent || this._unhashedFilePresent;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00006726 File Offset: 0x00005726
		public bool Signed
		{
			get
			{
				return this._signed;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006730 File Offset: 0x00005730
		public void ValidateSemantics(AssemblyManifest.ManifestType manifestType)
		{
			switch (manifestType)
			{
			case AssemblyManifest.ManifestType.Application:
				this.ValidateSemanticsForApplicationRole();
				return;
			case AssemblyManifest.ManifestType.Deployment:
				this.ValidateSemanticsForDeploymentRole();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000675C File Offset: 0x0000575C
		public File[] GetFilesInGroup(string group, bool optionalOnly)
		{
			StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
			ArrayList arrayList = new ArrayList();
			foreach (File file in this.Files)
			{
				if ((group == null && !file.IsOptional) || (group != null && group.Equals(file.Group, stringComparison) && (file.IsOptional || !optionalOnly)))
				{
					arrayList.Add(file);
				}
			}
			return (File[])arrayList.ToArray(typeof(File));
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000067D4 File Offset: 0x000057D4
		private static bool IsResourceReference(DependentAssembly dependentAssembly)
		{
			return dependentAssembly.ResourceFallbackCulture != null && dependentAssembly.Identity != null && dependentAssembly.Identity.Culture == null;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000067F8 File Offset: 0x000057F8
		public DependentAssembly[] GetPrivateAssembliesInGroup(string group, bool optionalOnly)
		{
			StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
			Hashtable hashtable = new Hashtable();
			foreach (DependentAssembly dependentAssembly in this.DependentAssemblies)
			{
				if (!dependentAssembly.IsPreRequisite && ((group == null && !dependentAssembly.IsOptional) || (group != null && group.Equals(dependentAssembly.Group, stringComparison) && (dependentAssembly.IsOptional || !optionalOnly))))
				{
					if (AssemblyManifest.IsResourceReference(dependentAssembly))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_SatelliteResourcesNotSupported"));
					}
					DependentAssembly dependentAssembly2 = dependentAssembly;
					if (dependentAssembly2 != null && !hashtable.Contains(dependentAssembly2.Identity))
					{
						hashtable.Add(dependentAssembly2.Identity, dependentAssembly2);
					}
				}
			}
			DependentAssembly[] array = new DependentAssembly[hashtable.Count];
			hashtable.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000068B8 File Offset: 0x000058B8
		public DependentAssembly GetDependentAssemblyByIdentity(IReferenceIdentity refid)
		{
			object obj = null;
			try
			{
				ISectionWithReferenceIdentityKey sectionWithReferenceIdentityKey = (ISectionWithReferenceIdentityKey)this._cms.AssemblyReferenceSection;
				sectionWithReferenceIdentityKey.Lookup(refid, out obj);
			}
			catch (ArgumentException)
			{
				return null;
			}
			IAssemblyReferenceEntry assemblyReferenceEntry = (IAssemblyReferenceEntry)obj;
			return new DependentAssembly(assemblyReferenceEntry.AllData);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000690C File Offset: 0x0000590C
		public File GetFileFromName(string fileName)
		{
			object obj = null;
			try
			{
				ISectionWithStringKey sectionWithStringKey = (ISectionWithStringKey)this._cms.FileSection;
				sectionWithStringKey.Lookup(fileName, out obj);
			}
			catch (ArgumentException)
			{
				return null;
			}
			IFileEntry fileEntry = (IFileEntry)obj;
			return new File(fileEntry.AllData);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006960 File Offset: 0x00005960
		public ulong CalculateDependenciesSize()
		{
			ulong num = 0UL;
			File[] filesInGroup = this.GetFilesInGroup(null, true);
			foreach (File file in filesInGroup)
			{
				num += file.Size;
			}
			DependentAssembly[] privateAssembliesInGroup = this.GetPrivateAssembliesInGroup(null, true);
			foreach (DependentAssembly dependentAssembly in privateAssembliesInGroup)
			{
				num += dependentAssembly.Size;
			}
			return num;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000069CF File Offset: 0x000059CF
		public ManifestSourceFormat ManifestSourceFormat
		{
			get
			{
				return this._manifestSourceFormat;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000069D8 File Offset: 0x000059D8
		private void LoadCMSFromStream(Stream stream)
		{
			ICMS icms = null;
			AssemblyManifest.ManifestParseErrors manifestParseErrors = new AssemblyManifest.ManifestParseErrors();
			int num;
			try
			{
				num = (int)stream.Length;
				this._rawXmlBytes = new byte[num];
				if (stream.CanSeek)
				{
					stream.Seek(0L, SeekOrigin.Begin);
				}
				stream.Read(this._rawXmlBytes, 0, num);
			}
			catch (IOException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, Resources.GetString("Ex_ManifestReadException"), ex);
			}
			try
			{
				icms = (ICMS)IsolationInterop.CreateCMSFromXml(this._rawXmlBytes, (uint)num, manifestParseErrors, ref IsolationInterop.IID_ICMS);
			}
			catch (COMException ex2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (AssemblyManifest.ManifestParseErrors.ManifestParseError manifestParseError in manifestParseErrors)
				{
					stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestParseCMSErrorMessage"), new object[] { manifestParseError.hr, manifestParseError.StartLine, manifestParseError.nStartColumn, manifestParseError.ErrorStatusHostFile });
				}
				throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestCMSParsingException"), new object[] { stringBuilder.ToString() }), ex2);
			}
			catch (SEHException ex3)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (AssemblyManifest.ManifestParseErrors.ManifestParseError manifestParseError2 in manifestParseErrors)
				{
					stringBuilder2.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestParseCMSErrorMessage"), new object[] { manifestParseError2.hr, manifestParseError2.StartLine, manifestParseError2.nStartColumn, manifestParseError2.ErrorStatusHostFile });
				}
				throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestCMSParsingException"), new object[] { stringBuilder2.ToString() }), ex3);
			}
			catch (ArgumentException ex4)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				foreach (AssemblyManifest.ManifestParseErrors.ManifestParseError manifestParseError3 in manifestParseErrors)
				{
					stringBuilder3.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestParseCMSErrorMessage"), new object[] { manifestParseError3.hr, manifestParseError3.StartLine, manifestParseError3.nStartColumn, manifestParseError3.ErrorStatusHostFile });
				}
				throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestCMSParsingException"), new object[] { stringBuilder3.ToString() }), ex4);
			}
			if (icms == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, Resources.GetString("Ex_IsoNullCmsCreated"));
			}
			this._cms = icms;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006D2C File Offset: 0x00005D2C
		private void LoadFromRawXmlFile(string filePath)
		{
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			{
				this.LoadCMSFromStream(fileStream);
				this._rawXmlFilePath = filePath;
				this._manifestSourceFormat = ManifestSourceFormat.XmlFile;
				this._sizeInBytes = (ulong)fileStream.Length;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006D80 File Offset: 0x00005D80
		private bool LoadFromPEResources(string filePath)
		{
			byte[] array = null;
			try
			{
				array = SystemUtils.GetManifestFromPEResources(filePath);
			}
			catch (Win32Exception ex)
			{
				AssemblyManifest.ManifestLoadExceptionHelper(ex, filePath);
			}
			if (array != null)
			{
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					this.LoadCMSFromStream(memoryStream);
				}
				this._id1Identity = (DefinitionIdentity)this.Identity.Clone();
				this._id1RequestedExecutionLevel = this.RequestedExecutionLevel;
				this._manifestSourceFormat = ManifestSourceFormat.ID_1;
				return true;
			}
			return false;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006E08 File Offset: 0x00005E08
		private static DefinitionIdentity ExtractIdentityFromCompLibAssembly(string filePath)
		{
			DefinitionIdentity definitionIdentity;
			try
			{
				using (AssemblyMetaDataImport assemblyMetaDataImport = new AssemblyMetaDataImport(filePath))
				{
					AssemblyName name = assemblyMetaDataImport.Name;
					definitionIdentity = SystemUtils.GetDefinitionIdentityFromManagedAssembly(filePath);
				}
			}
			catch (BadImageFormatException)
			{
				definitionIdentity = null;
			}
			catch (COMException)
			{
				definitionIdentity = null;
			}
			catch (SEHException)
			{
				definitionIdentity = null;
			}
			return definitionIdentity;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006E7C File Offset: 0x00005E7C
		private bool LoadFromCompLibAssembly(string filePath)
		{
			bool flag;
			try
			{
				using (AssemblyMetaDataImport assemblyMetaDataImport = new AssemblyMetaDataImport(filePath))
				{
					AssemblyName name = assemblyMetaDataImport.Name;
					this._identity = SystemUtils.GetDefinitionIdentityFromManagedAssembly(filePath);
					this._complibIdentity = (DefinitionIdentity)this.Identity.Clone();
					AssemblyModule[] files = assemblyMetaDataImport.Files;
					AssemblyReference[] references = assemblyMetaDataImport.References;
					File[] array = new File[files.Length + 1];
					array[0] = new File(Path.GetFileName(filePath), 0UL);
					for (int i = 0; i < files.Length; i++)
					{
						array[i + 1] = new File(files[i].Name, files[i].Hash, 0UL);
					}
					this._files = array;
					DependentAssembly[] array2 = new DependentAssembly[references.Length];
					for (int j = 0; j < references.Length; j++)
					{
						array2[j] = new DependentAssembly(new ReferenceIdentity(references[j].Name.ToString()));
					}
					this._dependentAssemblies = array2;
					this._manifestSourceFormat = ManifestSourceFormat.CompLib;
					flag = true;
				}
			}
			catch (BadImageFormatException)
			{
				flag = false;
			}
			catch (COMException)
			{
				flag = false;
			}
			catch (SEHException)
			{
				flag = false;
			}
			catch (IOException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006FCC File Offset: 0x00005FCC
		private void LoadFromInternalManifestFile(string filePath)
		{
			PEStream pestream = null;
			MemoryStream memoryStream = null;
			AssemblyManifest assemblyManifest = null;
			bool flag = true;
			try
			{
				pestream = new PEStream(filePath, true);
				byte[] defaultId1ManifestResource = pestream.GetDefaultId1ManifestResource();
				if (defaultId1ManifestResource != null)
				{
					memoryStream = new MemoryStream(defaultId1ManifestResource);
					assemblyManifest = new AssemblyManifest(memoryStream);
					this._id1ManifestPresent = true;
				}
				flag = pestream.IsImageFileDll;
			}
			catch (IOException ex)
			{
				AssemblyManifest.ManifestLoadExceptionHelper(ex, filePath);
			}
			catch (Win32Exception ex2)
			{
				AssemblyManifest.ManifestLoadExceptionHelper(ex2, filePath);
			}
			catch (InvalidDeploymentException ex3)
			{
				AssemblyManifest.ManifestLoadExceptionHelper(ex3, filePath);
			}
			finally
			{
				if (pestream != null)
				{
					pestream.Close();
				}
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			if (assemblyManifest == null)
			{
				if (!this.LoadFromCompLibAssembly(filePath))
				{
					AssemblyManifest.ManifestLoadExceptionHelper(new DeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_CannotLoadInternalManifest")), filePath);
				}
				return;
			}
			if (!assemblyManifest.Identity.IsEmpty)
			{
				if (!this.LoadFromPEResources(filePath))
				{
					AssemblyManifest.ManifestLoadExceptionHelper(new DeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_CannotLoadInternalManifest")), filePath);
				}
				this._complibIdentity = AssemblyManifest.ExtractIdentityFromCompLibAssembly(filePath);
				return;
			}
			if (!flag)
			{
				if (!this.LoadFromCompLibAssembly(filePath))
				{
					AssemblyManifest.ManifestLoadExceptionHelper(new DeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_CannotLoadInternalManifest")), filePath);
				}
				this._id1Identity = assemblyManifest.Identity;
				this._id1RequestedExecutionLevel = assemblyManifest.RequestedExecutionLevel;
				return;
			}
			AssemblyManifest.ManifestLoadExceptionHelper(new DeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_EmptyIdentityInternalManifest")), filePath);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007138 File Offset: 0x00006138
		private void LoadFromUnknownFormatFile(string filePath)
		{
			try
			{
				this.LoadFromRawXmlFile(filePath);
			}
			catch (InvalidDeploymentException ex)
			{
				if (ex.SubType != ExceptionTypes.ManifestParse && ex.SubType != ExceptionTypes.ManifestSemanticValidation)
				{
					throw;
				}
				this.LoadFromInternalManifestFile(filePath);
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007180 File Offset: 0x00006180
		internal void ValidateSignature(Stream s)
		{
			if (string.Equals(this.Identity.PublicKeyToken, "0000000000000000", StringComparison.Ordinal) && !PolicyKeys.RequireSignedManifests())
			{
				Logger.AddWarningInformation(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UnsignedManifest"), new object[0]));
				this._signed = false;
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			if (s != null)
			{
				xmlDocument.Load(s);
			}
			else
			{
				xmlDocument.Load(this._rawXmlFilePath);
			}
			try
			{
				SignedCmiManifest signedCmiManifest = new SignedCmiManifest(xmlDocument);
				signedCmiManifest.Verify(CmiManifestVerifyFlags.StrongNameOnly);
			}
			catch (CryptographicException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, Resources.GetString("Ex_InvalidXmlSignature"), ex);
			}
			if (this.RequiredHashMissing)
			{
				throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, Resources.GetString("Ex_SignedManifestUnhashedComponent"));
			}
			this._signed = true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007254 File Offset: 0x00006254
		internal static void ReValidateManifestSignatures(AssemblyManifest depManifest, AssemblyManifest appManifest)
		{
			if (depManifest.Signed && !appManifest.Signed)
			{
				throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, Resources.GetString("Ex_DepSignedAppUnsigned"));
			}
			if (!depManifest.Signed && appManifest.Signed)
			{
				throw new InvalidDeploymentException(ExceptionTypes.SignatureValidation, Resources.GetString("Ex_AppSignedDepUnsigned"));
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000072A8 File Offset: 0x000062A8
		internal void ValidateSemanticsForDeploymentRole()
		{
			try
			{
				AssemblyManifest.ValidateAssemblyIdentity(this.Identity);
				if (this.Identity.PublicKeyToken == null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepNotStronglyNamed"));
				}
				if (!PlatformDetector.IsSupportedProcessorArchitecture(this.Identity.ProcessorArchitecture))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepProcArchNotSupported"));
				}
				if (this.Deployment == null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepMissingDeploymentSection"));
				}
				if (this.UseManifestForTrust)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepWithUseManifestForTrust"));
				}
				if (this.Description == null || string.IsNullOrEmpty(this.Description.FilteredPublisher) || string.IsNullOrEmpty(this.Description.FilteredProduct))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepPublisherProductRequired"));
				}
				if (this.Description.FilteredPublisher.Length + this.Description.FilteredProduct.Length > 260)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_PublisherProductNameTooLong"));
				}
				if (this.EntryPoints.Length != 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepEntryPointNotAllowed"));
				}
				if (this.Files.Length != 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepFileNotAllowed"));
				}
				if (this.FileAssociations.Length > 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepFileAssocNotAllowed"));
				}
				if (this.Description.IconFile != null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepIconFileNotAllowed"));
				}
				if (this.Deployment.DisallowUrlActivation && !this.Deployment.Install)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepOnlineOnlyAndDisallowUrlActivation"));
				}
				if (this.Deployment.DisallowUrlActivation && this.Deployment.TrustURLParameters)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepTrustUrlAndDisallowUrlActivation"));
				}
				if (this.Deployment.Install)
				{
					if (this.Deployment.ProviderCodebaseUri != null)
					{
						if (!this.Deployment.ProviderCodebaseUri.IsAbsoluteUri)
						{
							throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepProviderNotAbsolute"));
						}
						if (!UriHelper.IsSupportedScheme(this.Deployment.ProviderCodebaseUri))
						{
							throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepProviderNotSupportedUriScheme"));
						}
						if (this.Deployment.ProviderCodebaseUri.AbsoluteUri.Length > 16384)
						{
							throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepProviderTooLong"));
						}
					}
					if (this.Deployment.MinimumRequiredVersion != null && this.Deployment.MinimumRequiredVersion > this.Identity.Version)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_MinimumRequiredVersionExceedDeployment"));
					}
				}
				else if (this.Deployment.MinimumRequiredVersion != null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepNoMinVerForOnlineApps"));
				}
				if (this.DependentAssemblies.Length != 1)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepApplicationDependencyRequired"));
				}
				this.ValidateApplicationDependency(this.DependentAssemblies[0]);
				if (this.DependentAssemblies[0].HashCollection.Count == 0)
				{
					this._unhashedDependencyPresent = true;
				}
				if (this.Deployment.DeploymentUpdate.BeforeApplicationStartup && this.Deployment.DeploymentUpdate.MaximumAgeSpecified)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepBeforeStartupMaxAgeBothPresent"));
				}
				if (this.Deployment.DeploymentUpdate.MaximumAgeSpecified && this.Deployment.DeploymentUpdate.MaximumAgeAllowed > TimeSpan.FromDays(365.0))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_MaxAgeTooLarge"));
				}
			}
			catch (InvalidDeploymentException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_SemanticallyInvalidDeploymentManifest"), ex);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007674 File Offset: 0x00006674
		internal void ValidateSemanticsForApplicationRole()
		{
			try
			{
				AssemblyManifest.ValidateAssemblyIdentity(this.Identity);
				if (this.EntryPoints.Length != 1)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppOneEntryPoint"));
				}
				EntryPoint entryPoint = this.EntryPoints[0];
				if (!entryPoint.CustomHostSpecified && (entryPoint.Assembly == null || entryPoint.Assembly.IsOptional || entryPoint.Assembly.IsPreRequisite || entryPoint.Assembly.Codebase == null || !UriHelper.IsValidRelativeFilePath(entryPoint.Assembly.Codebase) || UriHelper.PathContainDirectorySeparators(entryPoint.Assembly.Codebase) || !UriHelper.IsValidRelativeFilePath(entryPoint.CommandFile) || UriHelper.PathContainDirectorySeparators(entryPoint.CommandFile) || !entryPoint.CommandFile.Equals(entryPoint.Assembly.Codebase, StringComparison.OrdinalIgnoreCase) || string.Compare(this.Identity.ProcessorArchitecture, entryPoint.Assembly.Identity.ProcessorArchitecture, StringComparison.OrdinalIgnoreCase) != 0))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppInvalidEntryPoint"));
				}
				if (this.Application && entryPoint.CommandParameters != null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppInvalidEntryPointParameters"));
				}
				if (this.DependentAssemblies == null || this.DependentAssemblies.Length == 0)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppAtLeastOneDependency"));
				}
				if (this.Deployment != null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppNoDeploymentAllowed"));
				}
				if (this.UseManifestForTrust)
				{
					if (this.Description == null || (this.Description != null && (this.Description.Publisher == null || this.Description.Product == null)))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppNoOverridePublisherProduct"));
					}
				}
				else if (this.Description != null && (this.Description.Publisher != null || this.Description.Product != null))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppNoPublisherProductAllowed"));
				}
				if (this.Description != null && this.Description.IconFile != null && !UriHelper.IsValidRelativeFilePath(this.Description.IconFile))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_AppInvalidIconFile"));
				}
				if (this.Description != null && this.Description.SupportUri != null)
				{
					if (!this.Description.SupportUri.IsAbsoluteUri)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DescriptionSupportUrlNotAbsolute"));
					}
					if (!UriHelper.IsSupportedScheme(this.Description.SupportUri))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DescriptionSupportUrlNotSupportedUriScheme"));
					}
					if (this.Description.SupportUri.AbsoluteUri.Length > 16384)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DescriptionSupportUrlTooLong"));
					}
				}
				if (this.Files.Length > 24576)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_TooManyFilesInManifest"));
				}
				Hashtable hashtable = new Hashtable();
				foreach (File file in this.Files)
				{
					AssemblyManifest.ValidateFile(file);
					if (!file.IsOptional && !hashtable.Contains(file.Name))
					{
						hashtable.Add(file.Name.ToLower(), file);
					}
					if (file.HashCollection.Count == 0)
					{
						this._unhashedFilePresent = true;
					}
				}
				if (this.FileAssociations.Length > 0 && entryPoint.HostInBrowser)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_FileAssociationNotSupportedForHostInBrowser"));
				}
				if (this.FileAssociations.Length > 0 && entryPoint.CustomHostSpecified)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_FileAssociationNotSupportedForCustomHost"));
				}
				if (this.FileAssociations.Length > 8)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_TooManyFileAssociationsInManifest"), new object[] { 8 }));
				}
				Hashtable hashtable2 = new Hashtable();
				foreach (FileAssociation fileAssociation in this.FileAssociations)
				{
					if (string.IsNullOrEmpty(fileAssociation.Extension) || string.IsNullOrEmpty(fileAssociation.Description) || string.IsNullOrEmpty(fileAssociation.ProgID) || string.IsNullOrEmpty(fileAssociation.DefaultIcon))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_FileExtensionInfoMissing"));
					}
					if (fileAssociation.Extension.Length > 0)
					{
						char c = fileAssociation.Extension[0];
						if (c != '.')
						{
							throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssociationExtensionNoDot"), new object[] { fileAssociation.Extension }));
						}
					}
					string text = "file" + fileAssociation.Extension;
					if (!UriHelper.IsValidRelativeFilePath(text))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssociationInvalid"), new object[] { fileAssociation.Extension }));
					}
					if (fileAssociation.Extension.Length > 24)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileExtensionTooLong"), new object[] { fileAssociation.Extension }));
					}
					if (!hashtable.Contains(fileAssociation.DefaultIcon.ToLower()))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssociationIconFileNotFound"), new object[] { fileAssociation.DefaultIcon }));
					}
					if (hashtable2.Contains(fileAssociation.Extension))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_MultipleInstanceFileExtension"), new object[] { fileAssociation.Extension }));
					}
					hashtable2.Add(fileAssociation.Extension, fileAssociation);
				}
				if (this.DependentAssemblies.Length > 24576)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_TooManyAssembliesInManifest"));
				}
				bool flag = false;
				foreach (DependentAssembly dependentAssembly in this.DependentAssemblies)
				{
					AssemblyManifest.ValidateComponentDependency(dependentAssembly);
					if (dependentAssembly.IsPreRequisite && PlatformDetector.IsCLRDependencyText(dependentAssembly.Identity.Name))
					{
						flag = true;
					}
					if (!dependentAssembly.IsPreRequisite && dependentAssembly.HashCollection.Count == 0)
					{
						this._unhashedDependencyPresent = true;
					}
				}
				if (this.DependentOS != null && this.DependentOS.SupportUrl != null)
				{
					if (!this.DependentOS.SupportUrl.IsAbsoluteUri)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepenedentOSSupportUrlNotAbsolute"));
					}
					if (!UriHelper.IsSupportedScheme(this.DependentOS.SupportUrl))
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepenedentOSSupportUrlNotSupportedUriScheme"));
					}
					if (this.DependentOS.SupportUrl.AbsoluteUri.Length > 16384)
					{
						throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_DepenedentOSSupportUrlTooLong"));
					}
				}
				if (!flag)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_AppNoCLRDependency"), new object[0]));
				}
			}
			catch (InvalidDeploymentException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_SemanticallyInvalidApplicationManifest"), ex);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007D9C File Offset: 0x00006D9C
		internal static AssemblyManifest.CertificateStatus AnalyzeManifestCertificate(string manifestPath)
		{
			AssemblyManifest.CertificateStatus certificateStatus = AssemblyManifest.CertificateStatus.UnknownCertificateStatus;
			SignedCmiManifest signedCmiManifest = null;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.PreserveWhitespace = true;
				xmlDocument.Load(manifestPath);
				signedCmiManifest = new SignedCmiManifest(xmlDocument);
				signedCmiManifest.Verify(CmiManifestVerifyFlags.None);
				if (signedCmiManifest == null || signedCmiManifest.AuthenticodeSignerInfo == null)
				{
					certificateStatus = AssemblyManifest.CertificateStatus.NoCertificate;
				}
				else
				{
					certificateStatus = AssemblyManifest.CertificateStatus.TrustedPublisher;
				}
			}
			catch (Exception ex)
			{
				if (ex is CryptographicException && signedCmiManifest.AuthenticodeSignerInfo != null)
				{
					int errorCode = signedCmiManifest.AuthenticodeSignerInfo.ErrorCode;
					if (errorCode == -2146762479)
					{
						certificateStatus = AssemblyManifest.CertificateStatus.DistrustedPublisger;
					}
					else if (errorCode == -2146885616)
					{
						certificateStatus = AssemblyManifest.CertificateStatus.RevokedCertificate;
					}
					else if (errorCode == -2146762748)
					{
						certificateStatus = AssemblyManifest.CertificateStatus.AuthenticodedNotInTrustedList;
					}
					else
					{
						certificateStatus = AssemblyManifest.CertificateStatus.NoCertificate;
					}
				}
			}
			return certificateStatus;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007E3C File Offset: 0x00006E3C
		private static void ValidateAssemblyIdentity(DefinitionIdentity identity)
		{
			if (identity.Name != null && (identity.Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || identity.Name.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || identity.Name.IndexOfAny(AssemblyManifest.SpecificInvalidIdentityChars) >= 0))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_IdentityWithInvalidChars"), new object[] { identity.Name }));
			}
			try
			{
				if (identity.ToString().Length > 2048)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityTooLong"));
				}
			}
			catch (COMException)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityIsNotValid"));
			}
			catch (SEHException)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityIsNotValid"));
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007F20 File Offset: 0x00006F20
		private static void ValidateAssemblyIdentity(ReferenceIdentity identity)
		{
			if (identity.Name != null && (identity.Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || identity.Name.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || identity.Name.IndexOfAny(AssemblyManifest.SpecificInvalidIdentityChars) >= 0))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_IdentityWithInvalidChars"), new object[] { identity.Name }));
			}
			try
			{
				if (identity.ToString().Length > 2048)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityTooLong"));
				}
			}
			catch (COMException)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityIsNotValid"));
			}
			catch (SEHException)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_IdentityIsNotValid"));
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008004 File Offset: 0x00007004
		private void ValidateApplicationDependency(DependentAssembly da)
		{
			AssemblyManifest.ValidateAssemblyIdentity(da.Identity);
			if (da.Identity.PublicKeyToken == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_DepAppRefNotStrongNamed"));
			}
			if (AssemblyManifest.IsInvalidHash(da.HashCollection))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_DepAppRefHashInvalid"));
			}
			if (string.Compare(this.Identity.ProcessorArchitecture, da.Identity.ProcessorArchitecture, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DepAppRefProcArchMismatched"), new object[]
				{
					da.Identity.ProcessorArchitecture,
					this.Identity.ProcessorArchitecture
				}));
			}
			if (da.ResourceFallbackCulture != null || da.IsPreRequisite || da.IsOptional)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_DepAppRefPrereqOrOptionalOrResourceFallback"));
			}
			Uri uri = null;
			try
			{
				uri = new Uri(da.Codebase, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_DepAppRefInvalidCodebaseUri"), ex);
			}
			if (uri.IsAbsoluteUri && !UriHelper.IsSupportedScheme(uri))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, Resources.GetString("Ex_DepAppRefInvalidCodebaseUri"));
			}
			if (!UriHelper.IsValidRelativeFilePath(da.Identity.Name) || UriHelper.PathContainDirectorySeparators(da.Identity.Name))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DepAppRefInvalidIdentityName"), new object[] { da.Identity.Name }));
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008190 File Offset: 0x00007190
		private static void ValidateComponentDependency(DependentAssembly da)
		{
			AssemblyManifest.ValidateAssemblyIdentity(da.Identity);
			if (!da.IsPreRequisite)
			{
				if (da.ResourceFallbackCulture == null)
				{
					if (AssemblyManifest.IsInvalidHash(da.HashCollection))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyInvalidHash"), new object[] { da.Identity.ToString() }));
					}
					if (da.Codebase == null)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyNoCodebase"), new object[] { da.Identity.ToString() }));
					}
					if (!UriHelper.IsValidRelativeFilePath(da.Codebase))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyNotRelativePath"), new object[] { da.Identity.ToString() }));
					}
					if (da.IsOptional && da.Group == null)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyOptionalButNoGroup"), new object[] { da.Identity.ToString() }));
					}
				}
				else if (da.Identity.Culture == null)
				{
					if (da.Codebase != null)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyResourceWithCodebase"), new object[] { da.Identity.ToString() }));
					}
					if (da.HashCollection.Count > 0)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyResourceWithHash"), new object[] { da.Identity.ToString() }));
					}
				}
				else
				{
					if (AssemblyManifest.IsInvalidHash(da.HashCollection))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyInvalidHash"), new object[] { da.Identity.ToString() }));
					}
					if (da.Codebase == null)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyNoCodebase"), new object[] { da.Identity.ToString() }));
					}
					if (!UriHelper.IsValidRelativeFilePath(da.Codebase))
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyNotRelativePath"), new object[] { da.Identity.ToString() }));
					}
					if (da.ResourceFallbackCulture != null)
					{
						throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyResourceWithFallback"), new object[] { da.Identity.ToString() }));
					}
				}
			}
			else if (!PlatformDetector.IsCLRDependencyText(da.Identity.Name) && da.Identity.PublicKeyToken == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencyGACNoPKT"), new object[] { da.Identity.ToString() }));
			}
			if (da.SupportUrl != null)
			{
				if (!da.SupportUrl.IsAbsoluteUri)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencySupportUrlNoAbsolute"), new object[] { da.Identity.ToString() }));
				}
				if (!UriHelper.IsSupportedScheme(da.SupportUrl))
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencySupportUrlNotSupportedUriScheme"), new object[] { da.Identity.ToString() }));
				}
				if (da.SupportUrl.AbsoluteUri.Length > 16384)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DependencySupportUrlTooLong"), new object[] { da.Identity.ToString() }));
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008588 File Offset: 0x00007588
		private static void ValidateFile(File f)
		{
			if (AssemblyManifest.IsInvalidHash(f.HashCollection))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidFileHash"), new object[] { f.Name }));
			}
			if (!UriHelper.IsValidRelativeFilePath(f.Name))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FilePathNotRelative"), new object[] { f.Name }));
			}
			if (f.IsOptional && f.Group == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileOptionalButNoGroup"), new object[] { f.Name }));
			}
			if (f.IsOptional && f.IsData)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestComponentSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileOptionalAndData"), new object[] { f.Name }));
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008683 File Offset: 0x00007683
		private static bool IsInvalidHash(HashCollection hashCollection)
		{
			return !ComponentVerifier.IsVerifiableHashCollection(hashCollection);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008690 File Offset: 0x00007690
		internal static Uri UriFromMetadataEntry(string uriString, string exResourceStr)
		{
			Uri uri;
			try
			{
				uri = ((uriString != null) ? new Uri(uriString) : null);
			}
			catch (UriFormatException ex)
			{
				throw new InvalidDeploymentException(ExceptionTypes.Manifest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString(exResourceStr), new object[] { uriString }), ex);
			}
			return uri;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000086E4 File Offset: 0x000076E4
		private static void ManifestLoadExceptionHelper(Exception exception, string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			string text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestLoadFromFile"), new object[] { fileName });
			throw new InvalidDeploymentException(ExceptionTypes.ManifestLoad, text, exception);
		}

		// Token: 0x040000A2 RID: 162
		private string _rawXmlFilePath;

		// Token: 0x040000A3 RID: 163
		private byte[] _rawXmlBytes;

		// Token: 0x040000A4 RID: 164
		private ICMS _cms;

		// Token: 0x040000A5 RID: 165
		private object _identity;

		// Token: 0x040000A6 RID: 166
		private object _description;

		// Token: 0x040000A7 RID: 167
		private object _entryPoints;

		// Token: 0x040000A8 RID: 168
		private object _dependentAssemblies;

		// Token: 0x040000A9 RID: 169
		private object _files;

		// Token: 0x040000AA RID: 170
		private object _fileAssociations;

		// Token: 0x040000AB RID: 171
		private object _deployment;

		// Token: 0x040000AC RID: 172
		private object _dependentOS;

		// Token: 0x040000AD RID: 173
		private object _manifestFlags;

		// Token: 0x040000AE RID: 174
		private object _requestedExecutionLevel;

		// Token: 0x040000AF RID: 175
		private object _requestedExecutionLevelUIAccess;

		// Token: 0x040000B0 RID: 176
		private ManifestSourceFormat _manifestSourceFormat = ManifestSourceFormat.Unknown;

		// Token: 0x040000B1 RID: 177
		private DefinitionIdentity _id1Identity;

		// Token: 0x040000B2 RID: 178
		private DefinitionIdentity _complibIdentity;

		// Token: 0x040000B3 RID: 179
		private bool _id1ManifestPresent;

		// Token: 0x040000B4 RID: 180
		private string _id1RequestedExecutionLevel;

		// Token: 0x040000B5 RID: 181
		private ulong _sizeInBytes;

		// Token: 0x040000B6 RID: 182
		private bool _unhashedFilePresent;

		// Token: 0x040000B7 RID: 183
		private bool _unhashedDependencyPresent;

		// Token: 0x040000B8 RID: 184
		private bool _signed;

		// Token: 0x040000B9 RID: 185
		private static char[] SpecificInvalidIdentityChars = new char[] { '#', '&' };

		// Token: 0x0200001F RID: 31
		internal enum ManifestType
		{
			// Token: 0x040000BB RID: 187
			Application,
			// Token: 0x040000BC RID: 188
			Deployment
		}

		// Token: 0x02000021 RID: 33
		protected class ManifestParseErrors : IManifestParseErrorCallback, IEnumerable
		{
			// Token: 0x06000131 RID: 305 RVA: 0x00008748 File Offset: 0x00007748
			public void OnError(uint StartLine, uint nStartColumn, uint cCharacterCount, int hr, string ErrorStatusHostFile, uint ParameterCount, string[] Parameters)
			{
				AssemblyManifest.ManifestParseErrors.ManifestParseError manifestParseError = new AssemblyManifest.ManifestParseErrors.ManifestParseError();
				manifestParseError.StartLine = StartLine;
				manifestParseError.nStartColumn = nStartColumn;
				manifestParseError.cCharacterCount = cCharacterCount;
				manifestParseError.hr = hr;
				manifestParseError.ErrorStatusHostFile = ErrorStatusHostFile;
				manifestParseError.ParameterCount = ParameterCount;
				manifestParseError.Parameters = Parameters;
				this._parsingErrors.Add(manifestParseError);
			}

			// Token: 0x06000132 RID: 306 RVA: 0x0000879D File Offset: 0x0000779D
			public AssemblyManifest.ManifestParseErrors.ParseErrorEnumerator GetEnumerator()
			{
				return new AssemblyManifest.ManifestParseErrors.ParseErrorEnumerator(this);
			}

			// Token: 0x06000133 RID: 307 RVA: 0x000087A5 File Offset: 0x000077A5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040000BD RID: 189
			protected ArrayList _parsingErrors = new ArrayList();

			// Token: 0x02000022 RID: 34
			public class ManifestParseError
			{
				// Token: 0x040000BE RID: 190
				public uint StartLine;

				// Token: 0x040000BF RID: 191
				public uint nStartColumn;

				// Token: 0x040000C0 RID: 192
				public uint cCharacterCount;

				// Token: 0x040000C1 RID: 193
				public int hr;

				// Token: 0x040000C2 RID: 194
				public string ErrorStatusHostFile;

				// Token: 0x040000C3 RID: 195
				public uint ParameterCount;

				// Token: 0x040000C4 RID: 196
				public string[] Parameters;
			}

			// Token: 0x02000023 RID: 35
			public class ParseErrorEnumerator : IEnumerator
			{
				// Token: 0x06000136 RID: 310 RVA: 0x000087C8 File Offset: 0x000077C8
				public ParseErrorEnumerator(AssemblyManifest.ManifestParseErrors manifestParseErrors)
				{
					this._manifestParseErrors = manifestParseErrors;
					this._index = -1;
				}

				// Token: 0x06000137 RID: 311 RVA: 0x000087DE File Offset: 0x000077DE
				public void Reset()
				{
					this._index = -1;
				}

				// Token: 0x06000138 RID: 312 RVA: 0x000087E7 File Offset: 0x000077E7
				public bool MoveNext()
				{
					this._index++;
					return this._index < this._manifestParseErrors._parsingErrors.Count;
				}

				// Token: 0x17000087 RID: 135
				// (get) Token: 0x06000139 RID: 313 RVA: 0x0000880F File Offset: 0x0000780F
				public AssemblyManifest.ManifestParseErrors.ManifestParseError Current
				{
					get
					{
						return (AssemblyManifest.ManifestParseErrors.ManifestParseError)this._manifestParseErrors._parsingErrors[this._index];
					}
				}

				// Token: 0x17000088 RID: 136
				// (get) Token: 0x0600013A RID: 314 RVA: 0x0000882C File Offset: 0x0000782C
				object IEnumerator.Current
				{
					get
					{
						return this._manifestParseErrors._parsingErrors[this._index];
					}
				}

				// Token: 0x040000C5 RID: 197
				private int _index;

				// Token: 0x040000C6 RID: 198
				private AssemblyManifest.ManifestParseErrors _manifestParseErrors;
			}
		}

		// Token: 0x02000024 RID: 36
		internal enum CertificateStatus
		{
			// Token: 0x040000C8 RID: 200
			TrustedPublisher,
			// Token: 0x040000C9 RID: 201
			AuthenticodedNotInTrustedList,
			// Token: 0x040000CA RID: 202
			NoCertificate,
			// Token: 0x040000CB RID: 203
			DistrustedPublisger,
			// Token: 0x040000CC RID: 204
			RevokedCertificate,
			// Token: 0x040000CD RID: 205
			UnknownCertificateStatus
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Resources
{
	// Token: 0x0200041F RID: 1055
	[ComVisible(true)]
	[Serializable]
	public class ResourceManager
	{
		// Token: 0x06002B8B RID: 11147 RVA: 0x0009270C File Offset: 0x0009170C
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected ResourceManager()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00092730 File Offset: 0x00091730
		private ResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (resourceDir == null)
			{
				throw new ArgumentNullException("resourceDir");
			}
			this.BaseNameField = baseName;
			this.moduleDir = resourceDir;
			this._userResourceSet = usingResourceSet;
			this.ResourceSets = new Hashtable();
			this.UseManifest = false;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00092788 File Offset: 0x00091788
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.MainAssembly = assembly;
			this._locationInfo = null;
			this.BaseNameField = baseName;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (assembly == typeof(object).Assembly && this._callingAssembly != assembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x00092804 File Offset: 0x00091804
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.MainAssembly = assembly;
			this._locationInfo = null;
			this.BaseNameField = baseName;
			if (usingResourceSet != null && usingResourceSet != ResourceManager._minResourceSet && !usingResourceSet.IsSubclassOf(ResourceManager._minResourceSet))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ResMgrNotResSet"), "usingResourceSet");
			}
			this._userResourceSet = usingResourceSet;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (assembly == typeof(object).Assembly && this._callingAssembly != assembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000928B4 File Offset: 0x000918B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(Type resourceSource)
		{
			if (resourceSource == null)
			{
				throw new ArgumentNullException("resourceSource");
			}
			this._locationInfo = resourceSource;
			this.MainAssembly = this._locationInfo.Assembly;
			this.BaseNameField = resourceSource.Name;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (this.MainAssembly == typeof(object).Assembly && this._callingAssembly != this.MainAssembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x0009293A File Offset: 0x0009193A
		private void CommonSatelliteAssemblyInit()
		{
			this.UseManifest = true;
			this.UseSatelliteAssem = true;
			this.ResourceSets = new Hashtable();
			this._fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x0009295C File Offset: 0x0009195C
		public virtual string BaseName
		{
			get
			{
				return this.BaseNameField;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002B92 RID: 11154 RVA: 0x00092964 File Offset: 0x00091964
		// (set) Token: 0x06002B93 RID: 11155 RVA: 0x0009296C File Offset: 0x0009196C
		public virtual bool IgnoreCase
		{
			get
			{
				return this._ignoreCase;
			}
			set
			{
				this._ignoreCase = value;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002B94 RID: 11156 RVA: 0x00092975 File Offset: 0x00091975
		public virtual Type ResourceSetType
		{
			get
			{
				if (this._userResourceSet != null)
				{
					return this._userResourceSet;
				}
				return typeof(RuntimeResourceSet);
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x00092990 File Offset: 0x00091990
		// (set) Token: 0x06002B96 RID: 11158 RVA: 0x00092998 File Offset: 0x00091998
		protected UltimateResourceFallbackLocation FallbackLocation
		{
			get
			{
				return this._fallbackLoc;
			}
			set
			{
				this._fallbackLoc = value;
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000929A4 File Offset: 0x000919A4
		public virtual void ReleaseAllResources()
		{
			IDictionaryEnumerator enumerator = this.ResourceSets.GetEnumerator();
			this.ResourceSets = new Hashtable();
			while (enumerator.MoveNext())
			{
				((ResourceSet)enumerator.Value).Close();
			}
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000929E2 File Offset: 0x000919E2
		public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			return new ResourceManager(baseName, resourceDir, usingResourceSet);
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000929EC File Offset: 0x000919EC
		private string FindResourceFile(CultureInfo culture)
		{
			string resourceFileName = this.GetResourceFileName(culture);
			if (this.moduleDir != null)
			{
				string text = Path.Combine(this.moduleDir, resourceFileName);
				if (File.Exists(text))
				{
					return text;
				}
			}
			if (File.Exists(resourceFileName))
			{
				return resourceFileName;
			}
			return null;
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00092A2C File Offset: 0x00091A2C
		protected virtual string GetResourceFileName(CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			stringBuilder.Append(this.BaseNameField);
			if (!culture.Equals(CultureInfo.InvariantCulture))
			{
				CultureInfo.VerifyCultureName(culture, true);
				stringBuilder.Append('.');
				stringBuilder.Append(culture.Name);
			}
			stringBuilder.Append(".resources");
			return stringBuilder.ToString();
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00092A90 File Offset: 0x00091A90
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Hashtable resourceSets = this.ResourceSets;
			if (resourceSets != null)
			{
				ResourceSet resourceSet = (ResourceSet)resourceSets[culture];
				if (resourceSet != null)
				{
					return resourceSet;
				}
			}
			if (this.UseManifest && culture.Equals(CultureInfo.InvariantCulture))
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				string resourceFileName = this.GetResourceFileName(culture);
				Stream manifestResourceStream = this.MainAssembly.GetManifestResourceStream(this._locationInfo, resourceFileName, this._callingAssembly == this.MainAssembly, ref stackCrawlMark);
				if (createIfNotExists && manifestResourceStream != null)
				{
					ResourceSet resourceSet = this.CreateResourceSet(manifestResourceStream, this.MainAssembly);
					lock (resourceSets)
					{
						resourceSets.Add(culture, resourceSet);
					}
					return resourceSet;
				}
			}
			return this.InternalGetResourceSet(culture, createIfNotExists, tryParents);
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x00092B58 File Offset: 0x00091B58
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected virtual ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			Hashtable resourceSets = this.ResourceSets;
			ResourceSet resourceSet = (ResourceSet)resourceSets[culture];
			if (resourceSet != null)
			{
				return resourceSet;
			}
			Stream stream = null;
			Assembly assembly = null;
			if (this.UseManifest)
			{
				string text = this.GetResourceFileName(culture);
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				if (this.UseSatelliteAssem)
				{
					CultureInfo cultureInfo = culture;
					if (this._neutralResourcesCulture == null)
					{
						this._neutralResourcesCulture = ResourceManager.GetNeutralResourcesLanguage(this.MainAssembly, ref this._fallbackLoc);
					}
					if (culture.Equals(this._neutralResourcesCulture) && this.FallbackLocation == UltimateResourceFallbackLocation.MainAssembly)
					{
						cultureInfo = CultureInfo.InvariantCulture;
						text = this.GetResourceFileName(cultureInfo);
					}
					if (cultureInfo.Equals(CultureInfo.InvariantCulture))
					{
						if (this.FallbackLocation == UltimateResourceFallbackLocation.Satellite)
						{
							assembly = this.GetSatelliteAssembly(this._neutralResourcesCulture);
							if (assembly == null)
							{
								string text2 = this.MainAssembly.nGetSimpleName() + ".resources.dll";
								if (this._satelliteContractVersion != null)
								{
									text2 = text2 + ", Version=" + this._satelliteContractVersion.ToString();
								}
								AssemblyName assemblyName = new AssemblyName();
								assemblyName.SetPublicKey(this.MainAssembly.nGetPublicKey());
								byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
								int num = publicKeyToken.Length;
								StringBuilder stringBuilder = new StringBuilder(num * 2);
								for (int i = 0; i < num; i++)
								{
									stringBuilder.Append(publicKeyToken[i].ToString("x", CultureInfo.InvariantCulture));
								}
								text2 = text2 + ", PublicKeyToken=" + stringBuilder;
								string text3 = this._neutralResourcesCulture.Name;
								if (text3.Length == 0)
								{
									text3 = "<invariant>";
								}
								throw new MissingSatelliteAssemblyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingSatelliteAssembly_Culture_Name"), new object[] { this._neutralResourcesCulture, text2 }), text3);
							}
							text = this.GetResourceFileName(this._neutralResourcesCulture);
						}
						else
						{
							assembly = this.MainAssembly;
						}
					}
					else if (!this.TryLookingForSatellite(cultureInfo))
					{
						assembly = null;
					}
					else
					{
						assembly = this.GetSatelliteAssembly(cultureInfo);
					}
					if (assembly != null)
					{
						resourceSet = (ResourceSet)resourceSets[cultureInfo];
						if (resourceSet != null)
						{
							return resourceSet;
						}
						bool flag = this.MainAssembly == assembly && this._callingAssembly == this.MainAssembly;
						stream = assembly.GetManifestResourceStream(this._locationInfo, text, flag, ref stackCrawlMark);
						if (stream == null)
						{
							stream = this.CaseInsensitiveManifestResourceStreamLookup(assembly, text);
						}
					}
				}
				else
				{
					assembly = this.MainAssembly;
					stream = this.MainAssembly.GetManifestResourceStream(this._locationInfo, text, this._callingAssembly == this.MainAssembly, ref stackCrawlMark);
				}
				if (stream == null && tryParents)
				{
					if (!culture.Equals(CultureInfo.InvariantCulture))
					{
						CultureInfo parent = culture.Parent;
						resourceSet = this.InternalGetResourceSet(parent, createIfNotExists, tryParents);
						if (resourceSet != null)
						{
							ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
						}
						return resourceSet;
					}
					if (this.MainAssembly == typeof(object).Assembly && this.BaseName.Equals("mscorlib"))
					{
						throw new ExecutionEngineException("mscorlib.resources couldn't be found!  Large parts of the BCL won't work!");
					}
					string text4 = string.Empty;
					if (this._locationInfo != null && this._locationInfo.Namespace != null)
					{
						text4 = this._locationInfo.Namespace + Type.Delimiter;
					}
					text4 += text;
					throw new MissingManifestResourceException(Environment.GetResourceString("MissingManifestResource_NoNeutralAsm", new object[]
					{
						text4,
						this.MainAssembly.nGetSimpleName()
					}));
				}
			}
			else
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string text = this.FindResourceFile(culture);
				if (text != null)
				{
					resourceSet = this.CreateResourceSet(text);
					if (resourceSet != null)
					{
						ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
					}
					return resourceSet;
				}
				if (tryParents)
				{
					if (culture.Equals(CultureInfo.InvariantCulture))
					{
						throw new MissingManifestResourceException(string.Concat(new string[]
						{
							Environment.GetResourceString("MissingManifestResource_NoNeutralDisk"),
							Environment.NewLine,
							"baseName: ",
							this.BaseNameField,
							"  locationInfo: ",
							(this._locationInfo == null) ? "<null>" : this._locationInfo.FullName,
							"  fileName: ",
							this.GetResourceFileName(culture)
						}));
					}
					CultureInfo parent2 = culture.Parent;
					resourceSet = this.InternalGetResourceSet(parent2, createIfNotExists, tryParents);
					if (resourceSet != null)
					{
						ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
					}
					return resourceSet;
				}
			}
			if (createIfNotExists && stream != null && resourceSet == null)
			{
				resourceSet = this.CreateResourceSet(stream, assembly);
				ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
			}
			return resourceSet;
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x00092FB4 File Offset: 0x00091FB4
		private static void AddResourceSet(Hashtable localResourceSets, CultureInfo culture, ref ResourceSet rs)
		{
			lock (localResourceSets)
			{
				ResourceSet resourceSet = (ResourceSet)localResourceSets[culture];
				if (resourceSet != null)
				{
					if (!object.Equals(resourceSet, rs))
					{
						if (!localResourceSets.ContainsValue(rs))
						{
							rs.Dispose();
						}
						rs = resourceSet;
					}
				}
				else
				{
					localResourceSets.Add(culture, rs);
				}
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x0009301C File Offset: 0x0009201C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private Stream CaseInsensitiveManifestResourceStreamLookup(Assembly satellite, string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._locationInfo != null)
			{
				string @namespace = this._locationInfo.Namespace;
				if (@namespace != null)
				{
					stringBuilder.Append(@namespace);
					if (name != null)
					{
						stringBuilder.Append(Type.Delimiter);
					}
				}
			}
			stringBuilder.Append(name);
			string text = stringBuilder.ToString();
			CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
			string text2 = null;
			foreach (string text3 in satellite.GetManifestResourceNames())
			{
				if (compareInfo.Compare(text3, text, CompareOptions.IgnoreCase) == 0)
				{
					if (text2 != null)
					{
						throw new MissingManifestResourceException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingManifestResource_MultipleBlobs"), new object[]
						{
							text,
							satellite.ToString()
						}));
					}
					text2 = text3;
				}
			}
			if (text2 == null)
			{
				return null;
			}
			bool flag = this.MainAssembly == satellite && this._callingAssembly == this.MainAssembly;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return satellite.GetManifestResourceStream(text2, ref stackCrawlMark, flag);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x0009311C File Offset: 0x0009211C
		protected static Version GetSatelliteContractVersion(Assembly a)
		{
			string text = null;
			foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(a))
			{
				if (customAttributeData.Constructor.DeclaringType == typeof(SatelliteContractVersionAttribute))
				{
					text = (string)customAttributeData.ConstructorArguments[0].Value;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Version version;
			try
			{
				version = new Version(text);
			}
			catch (Exception ex)
			{
				if (a == typeof(object).Assembly)
				{
					return null;
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_InvalidSatelliteContract_Asm_Ver"), new object[]
				{
					a.ToString(),
					text
				}), ex);
			}
			return version;
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00093208 File Offset: 0x00092208
		protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
		{
			UltimateResourceFallbackLocation ultimateResourceFallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
			return ResourceManager.GetNeutralResourcesLanguage(a, ref ultimateResourceFallbackLocation);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00093224 File Offset: 0x00092224
		private static CultureInfo GetNeutralResourcesLanguage(Assembly a, ref UltimateResourceFallbackLocation fallbackLocation)
		{
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(a);
			CustomAttributeData customAttributeData = null;
			for (int i = 0; i < customAttributes.Count; i++)
			{
				if (customAttributes[i].Constructor.DeclaringType == typeof(NeutralResourcesLanguageAttribute))
				{
					customAttributeData = customAttributes[i];
					break;
				}
			}
			if (customAttributeData == null)
			{
				fallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
				return CultureInfo.InvariantCulture;
			}
			string text = null;
			if (customAttributeData.Constructor.GetParameters().Length == 2)
			{
				fallbackLocation = (UltimateResourceFallbackLocation)customAttributeData.ConstructorArguments[1].Value;
				if (fallbackLocation < UltimateResourceFallbackLocation.MainAssembly || fallbackLocation > UltimateResourceFallbackLocation.Satellite)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc", new object[] { fallbackLocation }));
				}
			}
			else
			{
				fallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
			}
			text = customAttributeData.ConstructorArguments[0].Value as string;
			CultureInfo cultureInfo2;
			try
			{
				CultureInfo cultureInfo = CultureInfo.GetCultureInfo(text);
				cultureInfo2 = cultureInfo;
			}
			catch (ArgumentException ex)
			{
				if (a != typeof(object).Assembly)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_Asm_Culture"), new object[]
					{
						a.ToString(),
						text
					}), ex);
				}
				cultureInfo2 = CultureInfo.InvariantCulture;
			}
			return cultureInfo2;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x0009336C File Offset: 0x0009236C
		private Assembly GetSatelliteAssembly(CultureInfo lookForCulture)
		{
			if (!this._lookedForSatelliteContractVersion)
			{
				this._satelliteContractVersion = ResourceManager.GetSatelliteContractVersion(this.MainAssembly);
				this._lookedForSatelliteContractVersion = true;
			}
			Assembly assembly = null;
			try
			{
				assembly = this.MainAssembly.InternalGetSatelliteAssembly(lookForCulture, this._satelliteContractVersion, false);
			}
			catch (FileLoadException ex)
			{
				int hrforException = Marshal.GetHRForException(ex);
				Win32Native.MakeHRFromErrorCode(5);
			}
			catch (BadImageFormatException)
			{
			}
			return assembly;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000933E4 File Offset: 0x000923E4
		private ResourceSet CreateResourceSet(string file)
		{
			if (this._userResourceSet == null)
			{
				return new RuntimeResourceSet(file);
			}
			object[] array = new object[] { file };
			ResourceSet resourceSet;
			try
			{
				resourceSet = (ResourceSet)Activator.CreateInstance(this._userResourceSet, array);
			}
			catch (MissingMethodException ex)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_ResMgrBadResSet_Type"), new object[] { this._userResourceSet.AssemblyQualifiedName }), ex);
			}
			return resourceSet;
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x00093464 File Offset: 0x00092464
		private ResourceSet CreateResourceSet(Stream store, Assembly assembly)
		{
			if (store.CanSeek && store.Length > 4L)
			{
				long position = store.Position;
				BinaryReader binaryReader = new BinaryReader(store);
				int num = binaryReader.ReadInt32();
				if (num == ResourceManager.MagicNumber)
				{
					int num2 = binaryReader.ReadInt32();
					string text;
					string text2;
					if (num2 == ResourceManager.HeaderVersionNumber)
					{
						binaryReader.ReadInt32();
						text = binaryReader.ReadString();
						text2 = binaryReader.ReadString();
					}
					else
					{
						if (num2 <= ResourceManager.HeaderVersionNumber)
						{
							throw new NotSupportedException(Environment.GetResourceString("NotSupported_ObsoleteResourcesFile", new object[] { this.MainAssembly.nGetSimpleName() }));
						}
						int num3 = binaryReader.ReadInt32();
						long num4 = binaryReader.BaseStream.Position + (long)num3;
						text = binaryReader.ReadString();
						text2 = binaryReader.ReadString();
						binaryReader.BaseStream.Seek(num4, SeekOrigin.Begin);
					}
					store.Position = position;
					if (this.CanUseDefaultResourceClasses(text, text2))
					{
						return new RuntimeResourceSet(store);
					}
					Type type = Type.GetType(text, true);
					IResourceReader resourceReader = (IResourceReader)Activator.CreateInstance(type, new object[] { store });
					object[] array = new object[] { resourceReader };
					Type type2;
					if (this._userResourceSet == null)
					{
						type2 = Type.GetType(text2, true, false);
					}
					else
					{
						type2 = this._userResourceSet;
					}
					return (ResourceSet)Activator.CreateInstance(type2, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, array, null, null);
				}
				else
				{
					store.Position = position;
				}
			}
			if (this._userResourceSet == null)
			{
				return new RuntimeResourceSet(store);
			}
			object[] array2 = new object[] { store, assembly };
			ResourceSet resourceSet2;
			try
			{
				try
				{
					return (ResourceSet)Activator.CreateInstance(this._userResourceSet, array2);
				}
				catch (MissingMethodException)
				{
				}
				array2 = new object[] { store };
				ResourceSet resourceSet = (ResourceSet)Activator.CreateInstance(this._userResourceSet, array2);
				resourceSet2 = resourceSet;
			}
			catch (MissingMethodException ex)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_ResMgrBadResSet_Type"), new object[] { this._userResourceSet.AssemblyQualifiedName }), ex);
			}
			return resourceSet2;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x00093694 File Offset: 0x00092694
		private bool CanUseDefaultResourceClasses(string readerTypeName, string resSetTypeName)
		{
			if (this._userResourceSet != null)
			{
				return false;
			}
			AssemblyName assemblyName = new AssemblyName(ResourceManager.MscorlibName);
			return (readerTypeName == null || ResourceManager.CompareNames(readerTypeName, ResourceManager.ResReaderTypeName, assemblyName)) && (resSetTypeName == null || ResourceManager.CompareNames(resSetTypeName, ResourceManager.ResSetTypeName, assemblyName));
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000936E0 File Offset: 0x000926E0
		internal static bool CompareNames(string asmTypeName1, string typeName2, AssemblyName asmName2)
		{
			int num = asmTypeName1.IndexOf(',');
			if (((num == -1) ? asmTypeName1.Length : num) != typeName2.Length)
			{
				return false;
			}
			if (string.Compare(asmTypeName1, 0, typeName2, 0, typeName2.Length, StringComparison.Ordinal) != 0)
			{
				return false;
			}
			if (num == -1)
			{
				return true;
			}
			while (char.IsWhiteSpace(asmTypeName1[++num]))
			{
			}
			AssemblyName assemblyName = new AssemblyName(asmTypeName1.Substring(num));
			if (string.Compare(assemblyName.Name, asmName2.Name, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return false;
			}
			if (assemblyName.CultureInfo != null && asmName2.CultureInfo != null && assemblyName.CultureInfo.LCID != asmName2.CultureInfo.LCID)
			{
				return false;
			}
			byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
			byte[] publicKeyToken2 = asmName2.GetPublicKeyToken();
			if (publicKeyToken != null && publicKeyToken2 != null)
			{
				if (publicKeyToken.Length != publicKeyToken2.Length)
				{
					return false;
				}
				for (int i = 0; i < publicKeyToken.Length; i++)
				{
					if (publicKeyToken[i] != publicKeyToken2[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000937C3 File Offset: 0x000927C3
		public virtual string GetString(string name)
		{
			return this.GetString(name, null);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000937D0 File Offset: 0x000927D0
		public virtual string GetString(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
			if (resourceSet != null)
			{
				string @string = resourceSet.GetString(name, this._ignoreCase);
				if (@string != null)
				{
					return @string;
				}
			}
			ResourceSet resourceSet2 = null;
			while (!culture.Equals(CultureInfo.InvariantCulture) && !culture.Equals(this._neutralResourcesCulture))
			{
				culture = culture.Parent;
				resourceSet = this.InternalGetResourceSet(culture, true, true);
				if (resourceSet == null)
				{
					break;
				}
				if (resourceSet != resourceSet2)
				{
					string string2 = resourceSet.GetString(name, this._ignoreCase);
					if (string2 != null)
					{
						return string2;
					}
					resourceSet2 = resourceSet;
				}
			}
			return null;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00093863 File Offset: 0x00092863
		public virtual object GetObject(string name)
		{
			return this.GetObject(name, null, true);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0009386E File Offset: 0x0009286E
		public virtual object GetObject(string name, CultureInfo culture)
		{
			return this.GetObject(name, culture, true);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x0009387C File Offset: 0x0009287C
		private object GetObject(string name, CultureInfo culture, bool wrapUnmanagedMemStream)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
			if (resourceSet != null)
			{
				object @object = resourceSet.GetObject(name, this._ignoreCase);
				if (@object != null)
				{
					UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
					if (unmanagedMemoryStream != null && wrapUnmanagedMemStream)
					{
						return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream);
					}
					return @object;
				}
			}
			ResourceSet resourceSet2 = null;
			while (!culture.Equals(CultureInfo.InvariantCulture) && !culture.Equals(this._neutralResourcesCulture))
			{
				culture = culture.Parent;
				resourceSet = this.InternalGetResourceSet(culture, true, true);
				if (resourceSet == null)
				{
					break;
				}
				if (resourceSet != resourceSet2)
				{
					object object2 = resourceSet.GetObject(name, this._ignoreCase);
					if (object2 != null)
					{
						UnmanagedMemoryStream unmanagedMemoryStream2 = object2 as UnmanagedMemoryStream;
						if (unmanagedMemoryStream2 != null && wrapUnmanagedMemStream)
						{
							return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream2);
						}
						return object2;
					}
					else
					{
						resourceSet2 = resourceSet;
					}
				}
			}
			return null;
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x0009393E File Offset: 0x0009293E
		[CLSCompliant(false)]
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name)
		{
			return this.GetStream(name, null);
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x00093948 File Offset: 0x00092948
		[ComVisible(false)]
		[CLSCompliant(false)]
		public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
		{
			object @object = this.GetObject(name, culture, false);
			UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
			if (unmanagedMemoryStream == null && @object != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotStream_Name", new object[] { name }));
			}
			return unmanagedMemoryStream;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x0009398C File Offset: 0x0009298C
		private bool TryLookingForSatellite(CultureInfo lookForCulture)
		{
			if (!ResourceManager._checkedConfigFile)
			{
				lock (this)
				{
					if (!ResourceManager._checkedConfigFile)
					{
						ResourceManager._checkedConfigFile = true;
						ResourceManager._installedSatelliteInfo = this.GetSatelliteAssembliesFromConfig();
					}
				}
			}
			if (ResourceManager._installedSatelliteInfo == null)
			{
				return true;
			}
			CultureInfo[] array = (CultureInfo[])ResourceManager._installedSatelliteInfo[this.MainAssembly.FullName];
			if (array == null)
			{
				return true;
			}
			int num = Array.IndexOf<CultureInfo>(array, lookForCulture);
			return num >= 0;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x00093A14 File Offset: 0x00092A14
		private Hashtable GetSatelliteAssembliesFromConfig()
		{
			string configurationFileInternal = AppDomain.CurrentDomain.FusionStore.ConfigurationFileInternal;
			if (configurationFileInternal == null)
			{
				return null;
			}
			if (configurationFileInternal.Length >= 2 && (configurationFileInternal[1] == Path.VolumeSeparatorChar || (configurationFileInternal[0] == Path.DirectorySeparatorChar && configurationFileInternal[1] == Path.DirectorySeparatorChar)) && !File.InternalExists(configurationFileInternal))
			{
				return null;
			}
			ConfigTreeParser configTreeParser = new ConfigTreeParser();
			string text = "/configuration/satelliteassemblies";
			ConfigNode configNode = null;
			try
			{
				configNode = configTreeParser.Parse(configurationFileInternal, text, true);
			}
			catch (Exception)
			{
			}
			if (configNode == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in configNode.Children)
			{
				ConfigNode configNode2 = (ConfigNode)obj;
				if (!string.Equals(configNode2.Name, "assembly"))
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTag", new object[]
					{
						Path.GetFileName(configurationFileInternal),
						configNode2.Name
					}));
				}
				if (configNode2.Attributes.Count != 1)
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTagBadAttr", new object[] { Path.GetFileName(configurationFileInternal) }));
				}
				DictionaryEntry dictionaryEntry = (DictionaryEntry)configNode2.Attributes[0];
				string text2 = (string)dictionaryEntry.Value;
				if (!object.Equals(dictionaryEntry.Key, "name") || text2 == null || text2.Length == 0)
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTagBadAttr", new object[]
					{
						Path.GetFileName(configurationFileInternal),
						dictionaryEntry.Key,
						dictionaryEntry.Value
					}));
				}
				ArrayList arrayList = new ArrayList(5);
				foreach (object obj2 in configNode2.Children)
				{
					ConfigNode configNode3 = (ConfigNode)obj2;
					if (configNode3.Value != null)
					{
						arrayList.Add(configNode3.Value);
					}
				}
				CultureInfo[] array = new CultureInfo[arrayList.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureInfo.GetCultureInfo((string)arrayList[i]);
				}
				hashtable.Add(text2, array);
			}
			return hashtable;
		}

		// Token: 0x0400150C RID: 5388
		internal const string ResFileExtension = ".resources";

		// Token: 0x0400150D RID: 5389
		internal const int ResFileExtensionLength = 10;

		// Token: 0x0400150E RID: 5390
		protected string BaseNameField;

		// Token: 0x0400150F RID: 5391
		protected Hashtable ResourceSets;

		// Token: 0x04001510 RID: 5392
		private string moduleDir;

		// Token: 0x04001511 RID: 5393
		protected Assembly MainAssembly;

		// Token: 0x04001512 RID: 5394
		private Type _locationInfo;

		// Token: 0x04001513 RID: 5395
		private Type _userResourceSet;

		// Token: 0x04001514 RID: 5396
		private CultureInfo _neutralResourcesCulture;

		// Token: 0x04001515 RID: 5397
		private bool _ignoreCase;

		// Token: 0x04001516 RID: 5398
		private bool UseManifest;

		// Token: 0x04001517 RID: 5399
		private bool UseSatelliteAssem;

		// Token: 0x04001518 RID: 5400
		private static Hashtable _installedSatelliteInfo;

		// Token: 0x04001519 RID: 5401
		private static bool _checkedConfigFile;

		// Token: 0x0400151A RID: 5402
		[OptionalField]
		private UltimateResourceFallbackLocation _fallbackLoc;

		// Token: 0x0400151B RID: 5403
		[OptionalField]
		private Version _satelliteContractVersion;

		// Token: 0x0400151C RID: 5404
		[OptionalField]
		private bool _lookedForSatelliteContractVersion;

		// Token: 0x0400151D RID: 5405
		private Assembly _callingAssembly;

		// Token: 0x0400151E RID: 5406
		public static readonly int MagicNumber = -1091581234;

		// Token: 0x0400151F RID: 5407
		public static readonly int HeaderVersionNumber = 1;

		// Token: 0x04001520 RID: 5408
		private static readonly Type _minResourceSet = typeof(ResourceSet);

		// Token: 0x04001521 RID: 5409
		internal static readonly string ResReaderTypeName = typeof(ResourceReader).FullName;

		// Token: 0x04001522 RID: 5410
		internal static readonly string ResSetTypeName = typeof(RuntimeResourceSet).FullName;

		// Token: 0x04001523 RID: 5411
		internal static readonly string MscorlibName = typeof(ResourceReader).Assembly.FullName;

		// Token: 0x04001524 RID: 5412
		internal static readonly int DEBUG = 0;
	}
}

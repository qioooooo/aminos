using System;
using System.Collections;
using System.Globalization;
using System.Security;

namespace System.Reflection.Emit
{
	// Token: 0x020007EB RID: 2027
	internal class AssemblyBuilderData
	{
		// Token: 0x06004854 RID: 18516 RVA: 0x000FC554 File Offset: 0x000FB554
		internal AssemblyBuilderData(Assembly assembly, string strAssemblyName, AssemblyBuilderAccess access, string dir)
		{
			this.m_assembly = assembly;
			this.m_strAssemblyName = strAssemblyName;
			this.m_access = access;
			this.m_moduleBuilderList = new ArrayList();
			this.m_resWriterList = new ArrayList();
			this.m_publicComTypeList = null;
			this.m_CABuilders = null;
			this.m_CABytes = null;
			this.m_CACons = null;
			this.m_iPublicComTypeCount = 0;
			this.m_iCABuilder = 0;
			this.m_iCAs = 0;
			this.m_entryPointModule = null;
			this.m_isSaved = false;
			if (dir == null && access != AssemblyBuilderAccess.Run)
			{
				this.m_strDir = Environment.CurrentDirectory;
			}
			else
			{
				this.m_strDir = dir;
			}
			this.m_RequiredPset = null;
			this.m_OptionalPset = null;
			this.m_RefusedPset = null;
			this.m_isSynchronized = true;
			this.m_hasUnmanagedVersionInfo = false;
			this.m_OverrideUnmanagedVersionInfo = false;
			this.m_InMemoryAssemblyModule = null;
			this.m_OnDiskAssemblyModule = null;
			this.m_peFileKind = PEFileKinds.Dll;
			this.m_strResourceFileName = null;
			this.m_resourceBytes = null;
			this.m_nativeVersion = null;
			this.m_entryPointMethod = null;
			this.m_ISymWrapperAssembly = null;
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x000FC650 File Offset: 0x000FB650
		internal void AddModule(ModuleBuilder dynModule)
		{
			this.m_moduleBuilderList.Add(dynModule);
			if (this.m_assembly != null)
			{
				this.m_assembly.nAddFileToInMemoryFileList(dynModule.m_moduleData.m_strFileName, dynModule);
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x000FC67F File Offset: 0x000FB67F
		internal void AddResWriter(ResWriterData resData)
		{
			this.m_resWriterList.Add(resData);
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x000FC690 File Offset: 0x000FB690
		internal void AddCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.m_CABuilders == null)
			{
				this.m_CABuilders = new CustomAttributeBuilder[16];
			}
			if (this.m_iCABuilder == this.m_CABuilders.Length)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.m_iCABuilder * 2];
				Array.Copy(this.m_CABuilders, array, this.m_iCABuilder);
				this.m_CABuilders = array;
			}
			this.m_CABuilders[this.m_iCABuilder] = customBuilder;
			this.m_iCABuilder++;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x000FC708 File Offset: 0x000FB708
		internal void AddCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.m_CABytes == null)
			{
				this.m_CABytes = new byte[16][];
				this.m_CACons = new ConstructorInfo[16];
			}
			if (this.m_iCAs == this.m_CABytes.Length)
			{
				byte[][] array = new byte[this.m_iCAs * 2][];
				ConstructorInfo[] array2 = new ConstructorInfo[this.m_iCAs * 2];
				for (int i = 0; i < this.m_iCAs; i++)
				{
					array[i] = this.m_CABytes[i];
					array2[i] = this.m_CACons[i];
				}
				this.m_CABytes = array;
				this.m_CACons = array2;
			}
			byte[] array3 = new byte[binaryAttribute.Length];
			Array.Copy(binaryAttribute, array3, binaryAttribute.Length);
			this.m_CABytes[this.m_iCAs] = array3;
			this.m_CACons[this.m_iCAs] = con;
			this.m_iCAs++;
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x000FC7D8 File Offset: 0x000FB7D8
		internal void FillUnmanagedVersionInfo()
		{
			CultureInfo locale = this.m_assembly.GetLocale();
			if (locale != null)
			{
				this.m_nativeVersion.m_lcid = locale.LCID;
			}
			for (int i = 0; i < this.m_iCABuilder; i++)
			{
				Type declaringType = this.m_CABuilders[i].m_con.DeclaringType;
				if (this.m_CABuilders[i].m_constructorArgs.Length != 0 && this.m_CABuilders[i].m_constructorArgs[0] != null)
				{
					if (declaringType.Equals(typeof(AssemblyCopyrightAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strCopyright = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyTrademarkAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strTrademark = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyProductAttribute)))
					{
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strProduct = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyCompanyAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strCompany = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyDescriptionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						this.m_nativeVersion.m_strDescription = this.m_CABuilders[i].m_constructorArgs[0].ToString();
					}
					else if (declaringType.Equals(typeof(AssemblyTitleAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						this.m_nativeVersion.m_strTitle = this.m_CABuilders[i].m_constructorArgs[0].ToString();
					}
					else if (declaringType.Equals(typeof(AssemblyInformationalVersionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strProductVersion = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyCultureAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						CultureInfo cultureInfo = new CultureInfo(this.m_CABuilders[i].m_constructorArgs[0].ToString());
						this.m_nativeVersion.m_lcid = cultureInfo.LCID;
					}
					else if (declaringType.Equals(typeof(AssemblyFileVersionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[] { this.m_CABuilders[i].m_con.ReflectedType.Name }));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strFileVersion = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
				}
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x000FCD1C File Offset: 0x000FBD1C
		internal void CheckResNameConflict(string strNewResName)
		{
			int count = this.m_resWriterList.Count;
			for (int i = 0; i < count; i++)
			{
				ResWriterData resWriterData = (ResWriterData)this.m_resWriterList[i];
				if (resWriterData.m_strName.Equals(strNewResName))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateResourceName"));
				}
			}
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x000FCD74 File Offset: 0x000FBD74
		internal void CheckNameConflict(string strNewModuleName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName.Equals(strNewModuleName))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateModuleName"));
				}
			}
			if (!(this.m_assembly is AssemblyBuilder) && this.m_assembly.GetModule(strNewModuleName) != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateModuleName"));
			}
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x000FCDFC File Offset: 0x000FBDFC
		internal void CheckTypeNameConflict(string strTypeName, TypeBuilder enclosingType)
		{
			for (int i = 0; i < this.m_moduleBuilderList.Count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				for (int j = 0; j < moduleBuilder.m_TypeBuilderList.Count; j++)
				{
					Type type = (Type)moduleBuilder.m_TypeBuilderList[j];
					if (type.FullName.Equals(strTypeName) && type.DeclaringType == enclosingType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateTypeName"));
					}
				}
			}
			if (enclosingType == null && !(this.m_assembly is AssemblyBuilder) && this.m_assembly.GetType(strTypeName, false, false) != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateTypeName"));
			}
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x000FCEB0 File Offset: 0x000FBEB0
		internal void CheckFileNameConflict(string strFileName)
		{
			int num = this.m_moduleBuilderList.Count;
			for (int i = 0; i < num; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strFileName != null && string.Compare(moduleBuilder.m_moduleData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicatedFileName"));
				}
			}
			num = this.m_resWriterList.Count;
			for (int i = 0; i < num; i++)
			{
				ResWriterData resWriterData = (ResWriterData)this.m_resWriterList[i];
				if (resWriterData.m_strFileName != null && string.Compare(resWriterData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicatedFileName"));
				}
			}
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x000FCF6C File Offset: 0x000FBF6C
		internal ModuleBuilder FindModuleWithFileName(string strFileName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strFileName != null && string.Compare(moduleBuilder.m_moduleData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x000FCFC8 File Offset: 0x000FBFC8
		internal ModuleBuilder FindModuleWithName(string strName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName != null && string.Compare(moduleBuilder.m_moduleData.m_strModuleName, strName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x000FD023 File Offset: 0x000FC023
		internal void AddPublicComType(Type type)
		{
			if (this.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			this.EnsurePublicComTypeCapacity();
			this.m_publicComTypeList[this.m_iPublicComTypeCount] = type;
			this.m_iPublicComTypeCount++;
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x000FD05F File Offset: 0x000FC05F
		internal void AddPermissionRequests(PermissionSet required, PermissionSet optional, PermissionSet refused)
		{
			if (this.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			this.m_RequiredPset = required;
			this.m_OptionalPset = optional;
			this.m_RefusedPset = refused;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x000FD090 File Offset: 0x000FC090
		internal void EnsurePublicComTypeCapacity()
		{
			if (this.m_publicComTypeList == null)
			{
				this.m_publicComTypeList = new Type[16];
			}
			if (this.m_iPublicComTypeCount == this.m_publicComTypeList.Length)
			{
				Type[] array = new Type[this.m_iPublicComTypeCount * 2];
				Array.Copy(this.m_publicComTypeList, array, this.m_iPublicComTypeCount);
				this.m_publicComTypeList = array;
			}
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x000FD0EC File Offset: 0x000FC0EC
		internal ModuleBuilder GetInMemoryAssemblyModule()
		{
			if (this.m_InMemoryAssemblyModule == null)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_assembly.nGetInMemoryAssemblyModule();
				moduleBuilder.Init("RefEmit_InMemoryManifestModule", null, null);
				this.m_InMemoryAssemblyModule = moduleBuilder;
			}
			return this.m_InMemoryAssemblyModule;
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x000FD12C File Offset: 0x000FC12C
		internal ModuleBuilder GetOnDiskAssemblyModule()
		{
			if (this.m_OnDiskAssemblyModule == null)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_assembly.nGetOnDiskAssemblyModule();
				moduleBuilder.Init("RefEmit_OnDiskManifestModule", null, null);
				this.m_OnDiskAssemblyModule = moduleBuilder;
			}
			return this.m_OnDiskAssemblyModule;
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x000FD16C File Offset: 0x000FC16C
		internal void SetOnDiskAssemblyModule(ModuleBuilder modBuilder)
		{
			this.m_OnDiskAssemblyModule = modBuilder;
		}

		// Token: 0x04002506 RID: 9478
		internal const int m_iInitialSize = 16;

		// Token: 0x04002507 RID: 9479
		internal const int m_tkAssembly = 536870913;

		// Token: 0x04002508 RID: 9480
		internal ArrayList m_moduleBuilderList;

		// Token: 0x04002509 RID: 9481
		internal ArrayList m_resWriterList;

		// Token: 0x0400250A RID: 9482
		internal string m_strAssemblyName;

		// Token: 0x0400250B RID: 9483
		internal AssemblyBuilderAccess m_access;

		// Token: 0x0400250C RID: 9484
		internal Assembly m_assembly;

		// Token: 0x0400250D RID: 9485
		internal Type[] m_publicComTypeList;

		// Token: 0x0400250E RID: 9486
		internal int m_iPublicComTypeCount;

		// Token: 0x0400250F RID: 9487
		internal ModuleBuilder m_entryPointModule;

		// Token: 0x04002510 RID: 9488
		internal bool m_isSaved;

		// Token: 0x04002511 RID: 9489
		internal string m_strDir;

		// Token: 0x04002512 RID: 9490
		internal PermissionSet m_RequiredPset;

		// Token: 0x04002513 RID: 9491
		internal PermissionSet m_OptionalPset;

		// Token: 0x04002514 RID: 9492
		internal PermissionSet m_RefusedPset;

		// Token: 0x04002515 RID: 9493
		internal bool m_isSynchronized;

		// Token: 0x04002516 RID: 9494
		internal CustomAttributeBuilder[] m_CABuilders;

		// Token: 0x04002517 RID: 9495
		internal int m_iCABuilder;

		// Token: 0x04002518 RID: 9496
		internal byte[][] m_CABytes;

		// Token: 0x04002519 RID: 9497
		internal ConstructorInfo[] m_CACons;

		// Token: 0x0400251A RID: 9498
		internal int m_iCAs;

		// Token: 0x0400251B RID: 9499
		internal PEFileKinds m_peFileKind;

		// Token: 0x0400251C RID: 9500
		private ModuleBuilder m_InMemoryAssemblyModule;

		// Token: 0x0400251D RID: 9501
		private ModuleBuilder m_OnDiskAssemblyModule;

		// Token: 0x0400251E RID: 9502
		internal MethodInfo m_entryPointMethod;

		// Token: 0x0400251F RID: 9503
		internal Assembly m_ISymWrapperAssembly;

		// Token: 0x04002520 RID: 9504
		internal string m_strResourceFileName;

		// Token: 0x04002521 RID: 9505
		internal byte[] m_resourceBytes;

		// Token: 0x04002522 RID: 9506
		internal NativeVersionInfo m_nativeVersion;

		// Token: 0x04002523 RID: 9507
		internal bool m_hasUnmanagedVersionInfo;

		// Token: 0x04002524 RID: 9508
		internal bool m_OverrideUnmanagedVersionInfo;
	}
}

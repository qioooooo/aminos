using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051B RID: 1307
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("475E398F-8AFA-43a7-A3BE-F4EF8D6787C9")]
	[ComVisible(true)]
	public class RegistrationServices : IRegistrationServices
	{
		// Token: 0x060032AB RID: 12971 RVA: 0x000AB83C File Offset: 0x000AA83C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual bool RegisterAssembly(Assembly assembly, AssemblyRegistrationFlags flags)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsmLoadedForReflectionOnly"));
			}
			string fullName = assembly.FullName;
			if (fullName == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoAsmName"));
			}
			string text = null;
			if ((flags & AssemblyRegistrationFlags.SetCodeBase) != AssemblyRegistrationFlags.None)
			{
				text = assembly.nGetCodeBase(false);
				if (text == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoAsmCodeBase"));
				}
			}
			Type[] registrableTypesInAssembly = this.GetRegistrableTypesInAssembly(assembly);
			int num = registrableTypesInAssembly.Length;
			string text2 = assembly.GetVersion().ToString();
			string imageRuntimeVersion = assembly.ImageRuntimeVersion;
			for (int i = 0; i < num; i++)
			{
				if (this.IsRegisteredAsValueType(registrableTypesInAssembly[i]))
				{
					this.RegisterValueType(registrableTypesInAssembly[i], fullName, text2, text, imageRuntimeVersion);
				}
				else if (this.TypeRepresentsComType(registrableTypesInAssembly[i]))
				{
					this.RegisterComImportedType(registrableTypesInAssembly[i], fullName, text2, text, imageRuntimeVersion);
				}
				else
				{
					this.RegisterManagedType(registrableTypesInAssembly[i], fullName, text2, text, imageRuntimeVersion);
				}
				this.CallUserDefinedRegistrationMethod(registrableTypesInAssembly[i], true);
			}
			object[] customAttributes = assembly.GetCustomAttributes(typeof(PrimaryInteropAssemblyAttribute), false);
			int num2 = customAttributes.Length;
			for (int j = 0; j < num2; j++)
			{
				this.RegisterPrimaryInteropAssembly(assembly, text, (PrimaryInteropAssemblyAttribute)customAttributes[j]);
			}
			return registrableTypesInAssembly.Length > 0 || num2 > 0;
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x000AB980 File Offset: 0x000AA980
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual bool UnregisterAssembly(Assembly assembly)
		{
			bool flag = true;
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsmLoadedForReflectionOnly"));
			}
			Type[] registrableTypesInAssembly = this.GetRegistrableTypesInAssembly(assembly);
			int num = registrableTypesInAssembly.Length;
			string text = assembly.GetVersion().ToString();
			for (int i = 0; i < num; i++)
			{
				this.CallUserDefinedRegistrationMethod(registrableTypesInAssembly[i], false);
				if (this.IsRegisteredAsValueType(registrableTypesInAssembly[i]))
				{
					if (!this.UnregisterValueType(registrableTypesInAssembly[i], text))
					{
						flag = false;
					}
				}
				else if (this.TypeRepresentsComType(registrableTypesInAssembly[i]))
				{
					if (!this.UnregisterComImportedType(registrableTypesInAssembly[i], text))
					{
						flag = false;
					}
				}
				else if (!this.UnregisterManagedType(registrableTypesInAssembly[i], text))
				{
					flag = false;
				}
			}
			object[] customAttributes = assembly.GetCustomAttributes(typeof(PrimaryInteropAssemblyAttribute), false);
			int num2 = customAttributes.Length;
			if (flag)
			{
				for (int j = 0; j < num2; j++)
				{
					this.UnregisterPrimaryInteropAssembly(assembly, (PrimaryInteropAssemblyAttribute)customAttributes[j]);
				}
			}
			return registrableTypesInAssembly.Length > 0 || num2 > 0;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000ABA80 File Offset: 0x000AAA80
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual Type[] GetRegistrableTypesInAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			Type[] exportedTypes = assembly.GetExportedTypes();
			int num = exportedTypes.Length;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < num; i++)
			{
				Type type = exportedTypes[i];
				if (this.TypeRequiresRegistration(type))
				{
					arrayList.Add(type);
				}
			}
			Type[] array = new Type[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000ABAE7 File Offset: 0x000AAAE7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual string GetProgIdForType(Type type)
		{
			return Marshal.GenerateProgIdForType(type);
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000ABAF0 File Offset: 0x000AAAF0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void RegisterTypeForComClients(Type type, ref Guid g)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (!this.TypeRequiresRegistration(type))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeMustBeComCreatable"), "type");
			}
			RegistrationServices.RegisterTypeForComClientsNative(type, ref g);
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x000ABB4D File Offset: 0x000AAB4D
		public virtual Guid GetManagedCategoryGuid()
		{
			return RegistrationServices.s_ManagedCategoryGuid;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000ABB54 File Offset: 0x000AAB54
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual bool TypeRequiresRegistration(Type type)
		{
			return RegistrationServices.TypeRequiresRegistrationHelper(type);
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000ABB5C File Offset: 0x000AAB5C
		public virtual bool TypeRepresentsComType(Type type)
		{
			if (!type.IsCOMObject)
			{
				return false;
			}
			if (type.IsImport)
			{
				return true;
			}
			Type baseComImportType = this.GetBaseComImportType(type);
			return Marshal.GenerateGuidForType(type) == Marshal.GenerateGuidForType(baseComImportType);
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000ABB9C File Offset: 0x000AAB9C
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual int RegisterTypeForComClients(Type type, RegistrationClassContext classContext, RegistrationConnectionType flags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "type");
			}
			if (!this.TypeRequiresRegistration(type))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeMustBeComCreatable"), "type");
			}
			return RegistrationServices.RegisterTypeForComClientsExNative(type, classContext, flags);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000ABBFA File Offset: 0x000AABFA
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public virtual void UnregisterTypeForComClients(int cookie)
		{
			RegistrationServices.CoRevokeClassObject(cookie);
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000ABC02 File Offset: 0x000AAC02
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static bool TypeRequiresRegistrationHelper(Type type)
		{
			return (type.IsClass || type.IsValueType) && !type.IsAbstract && (type.IsValueType || type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null) != null) && Marshal.IsTypeVisibleFromCom(type);
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000ABC44 File Offset: 0x000AAC44
		private void RegisterValueType(Type type, string strAsmName, string strAsmVersion, string strAsmCodeBase, string strRuntimeVersion)
		{
			string text = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey("Record"))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey(text))
				{
					using (RegistryKey registryKey3 = registryKey2.CreateSubKey(strAsmVersion))
					{
						registryKey3.SetValue("Class", type.FullName);
						registryKey3.SetValue("Assembly", strAsmName);
						registryKey3.SetValue("RuntimeVersion", strRuntimeVersion);
						if (strAsmCodeBase != null)
						{
							registryKey3.SetValue("CodeBase", strAsmCodeBase);
						}
					}
				}
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x000ABD28 File Offset: 0x000AAD28
		private void RegisterManagedType(Type type, string strAsmName, string strAsmVersion, string strAsmCodeBase, string strRuntimeVersion)
		{
			string text = "" + type.FullName;
			string text2 = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			string progIdForType = this.GetProgIdForType(type);
			if (progIdForType != string.Empty)
			{
				using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey(progIdForType))
				{
					registryKey.SetValue("", text);
					using (RegistryKey registryKey2 = registryKey.CreateSubKey("CLSID"))
					{
						registryKey2.SetValue("", text2);
					}
				}
			}
			using (RegistryKey registryKey3 = Registry.ClassesRoot.CreateSubKey("CLSID"))
			{
				using (RegistryKey registryKey4 = registryKey3.CreateSubKey(text2))
				{
					registryKey4.SetValue("", text);
					using (RegistryKey registryKey5 = registryKey4.CreateSubKey("InprocServer32"))
					{
						registryKey5.SetValue("", "mscoree.dll");
						registryKey5.SetValue("ThreadingModel", "Both");
						registryKey5.SetValue("Class", type.FullName);
						registryKey5.SetValue("Assembly", strAsmName);
						registryKey5.SetValue("RuntimeVersion", strRuntimeVersion);
						if (strAsmCodeBase != null)
						{
							registryKey5.SetValue("CodeBase", strAsmCodeBase);
						}
						using (RegistryKey registryKey6 = registryKey5.CreateSubKey(strAsmVersion))
						{
							registryKey6.SetValue("Class", type.FullName);
							registryKey6.SetValue("Assembly", strAsmName);
							registryKey6.SetValue("RuntimeVersion", strRuntimeVersion);
							if (strAsmCodeBase != null)
							{
								registryKey6.SetValue("CodeBase", strAsmCodeBase);
							}
						}
						if (progIdForType != string.Empty)
						{
							using (RegistryKey registryKey7 = registryKey4.CreateSubKey("ProgId"))
							{
								registryKey7.SetValue("", progIdForType);
							}
						}
					}
					using (RegistryKey registryKey8 = registryKey4.CreateSubKey("Implemented Categories"))
					{
						using (registryKey8.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}"))
						{
						}
					}
				}
			}
			this.EnsureManagedCategoryExists();
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000AC040 File Offset: 0x000AB040
		private void RegisterComImportedType(Type type, string strAsmName, string strAsmVersion, string strAsmCodeBase, string strRuntimeVersion)
		{
			string text = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey("CLSID"))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey(text))
				{
					using (RegistryKey registryKey3 = registryKey2.CreateSubKey("InprocServer32"))
					{
						registryKey3.SetValue("Class", type.FullName);
						registryKey3.SetValue("Assembly", strAsmName);
						registryKey3.SetValue("RuntimeVersion", strRuntimeVersion);
						if (strAsmCodeBase != null)
						{
							registryKey3.SetValue("CodeBase", strAsmCodeBase);
						}
						using (RegistryKey registryKey4 = registryKey3.CreateSubKey(strAsmVersion))
						{
							registryKey4.SetValue("Class", type.FullName);
							registryKey4.SetValue("Assembly", strAsmName);
							registryKey4.SetValue("RuntimeVersion", strRuntimeVersion);
							if (strAsmCodeBase != null)
							{
								registryKey4.SetValue("CodeBase", strAsmCodeBase);
							}
						}
					}
				}
			}
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000AC18C File Offset: 0x000AB18C
		private bool UnregisterValueType(Type type, string strAsmVersion)
		{
			bool flag = true;
			string text = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("Record", true))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(text, true))
					{
						if (registryKey2 != null)
						{
							registryKey2.DeleteValue("Assembly", false);
							registryKey2.DeleteValue("Class", false);
							registryKey2.DeleteValue("CodeBase", false);
							using (RegistryKey registryKey3 = registryKey2.OpenSubKey(strAsmVersion, true))
							{
								if (registryKey3 != null)
								{
									registryKey3.DeleteValue("Assembly", false);
									registryKey3.DeleteValue("Class", false);
									registryKey3.DeleteValue("CodeBase", false);
									if (registryKey3.SubKeyCount == 0 && registryKey3.ValueCount == 0)
									{
										registryKey2.DeleteSubKey(strAsmVersion);
									}
								}
							}
							if (registryKey2.SubKeyCount != 0)
							{
								flag = false;
							}
							if (registryKey2.SubKeyCount == 0 && registryKey2.ValueCount == 0)
							{
								registryKey.DeleteSubKey(text);
							}
						}
					}
					if (registryKey.SubKeyCount == 0 && registryKey.ValueCount == 0)
					{
						Registry.ClassesRoot.DeleteSubKey("Record");
					}
				}
			}
			return flag;
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000AC2F4 File Offset: 0x000AB2F4
		private bool UnregisterManagedType(Type type, string strAsmVersion)
		{
			bool flag = true;
			string text = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			string progIdForType = this.GetProgIdForType(type);
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("CLSID", true))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(text, true))
					{
						if (registryKey2 != null)
						{
							using (RegistryKey registryKey3 = registryKey2.OpenSubKey("InprocServer32", true))
							{
								if (registryKey3 != null)
								{
									using (RegistryKey registryKey4 = registryKey3.OpenSubKey(strAsmVersion, true))
									{
										if (registryKey4 != null)
										{
											registryKey4.DeleteValue("Assembly", false);
											registryKey4.DeleteValue("Class", false);
											registryKey4.DeleteValue("RuntimeVersion", false);
											registryKey4.DeleteValue("CodeBase", false);
											if (registryKey4.SubKeyCount == 0 && registryKey4.ValueCount == 0)
											{
												registryKey3.DeleteSubKey(strAsmVersion);
											}
										}
									}
									if (registryKey3.SubKeyCount != 0)
									{
										flag = false;
									}
									if (flag)
									{
										registryKey3.DeleteValue("", false);
										registryKey3.DeleteValue("ThreadingModel", false);
									}
									registryKey3.DeleteValue("Assembly", false);
									registryKey3.DeleteValue("Class", false);
									registryKey3.DeleteValue("RuntimeVersion", false);
									registryKey3.DeleteValue("CodeBase", false);
									if (registryKey3.SubKeyCount == 0 && registryKey3.ValueCount == 0)
									{
										registryKey2.DeleteSubKey("InprocServer32");
									}
								}
							}
							if (flag)
							{
								registryKey2.DeleteValue("", false);
								if (progIdForType != string.Empty)
								{
									using (RegistryKey registryKey5 = registryKey2.OpenSubKey("ProgId", true))
									{
										if (registryKey5 != null)
										{
											registryKey5.DeleteValue("", false);
											if (registryKey5.SubKeyCount == 0 && registryKey5.ValueCount == 0)
											{
												registryKey2.DeleteSubKey("ProgId");
											}
										}
									}
								}
								using (RegistryKey registryKey6 = registryKey2.OpenSubKey("Implemented Categories", true))
								{
									if (registryKey6 != null)
									{
										using (RegistryKey registryKey7 = registryKey6.OpenSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}", true))
										{
											if (registryKey7 != null && registryKey7.SubKeyCount == 0 && registryKey7.ValueCount == 0)
											{
												registryKey6.DeleteSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
											}
										}
										if (registryKey6.SubKeyCount == 0 && registryKey6.ValueCount == 0)
										{
											registryKey2.DeleteSubKey("Implemented Categories");
										}
									}
								}
							}
							if (registryKey2.SubKeyCount == 0 && registryKey2.ValueCount == 0)
							{
								registryKey.DeleteSubKey(text);
							}
						}
					}
					if (registryKey.SubKeyCount == 0 && registryKey.ValueCount == 0)
					{
						Registry.ClassesRoot.DeleteSubKey("CLSID");
					}
				}
				if (flag && progIdForType != string.Empty)
				{
					using (RegistryKey registryKey8 = Registry.ClassesRoot.OpenSubKey(progIdForType, true))
					{
						if (registryKey8 != null)
						{
							registryKey8.DeleteValue("", false);
							using (RegistryKey registryKey9 = registryKey8.OpenSubKey("CLSID", true))
							{
								if (registryKey9 != null)
								{
									registryKey9.DeleteValue("", false);
									if (registryKey9.SubKeyCount == 0 && registryKey9.ValueCount == 0)
									{
										registryKey8.DeleteSubKey("CLSID");
									}
								}
							}
							if (registryKey8.SubKeyCount == 0 && registryKey8.ValueCount == 0)
							{
								Registry.ClassesRoot.DeleteSubKey(progIdForType);
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000AC730 File Offset: 0x000AB730
		private bool UnregisterComImportedType(Type type, string strAsmVersion)
		{
			bool flag = true;
			string text = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("CLSID", true))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(text, true))
					{
						if (registryKey2 != null)
						{
							using (RegistryKey registryKey3 = registryKey2.OpenSubKey("InprocServer32", true))
							{
								if (registryKey3 != null)
								{
									registryKey3.DeleteValue("Assembly", false);
									registryKey3.DeleteValue("Class", false);
									registryKey3.DeleteValue("RuntimeVersion", false);
									registryKey3.DeleteValue("CodeBase", false);
									using (RegistryKey registryKey4 = registryKey3.OpenSubKey(strAsmVersion, true))
									{
										if (registryKey4 != null)
										{
											registryKey4.DeleteValue("Assembly", false);
											registryKey4.DeleteValue("Class", false);
											registryKey4.DeleteValue("RuntimeVersion", false);
											registryKey4.DeleteValue("CodeBase", false);
											if (registryKey4.SubKeyCount == 0 && registryKey4.ValueCount == 0)
											{
												registryKey3.DeleteSubKey(strAsmVersion);
											}
										}
									}
									if (registryKey3.SubKeyCount != 0)
									{
										flag = false;
									}
									if (registryKey3.SubKeyCount == 0 && registryKey3.ValueCount == 0)
									{
										registryKey2.DeleteSubKey("InprocServer32");
									}
								}
							}
							if (registryKey2.SubKeyCount == 0 && registryKey2.ValueCount == 0)
							{
								registryKey.DeleteSubKey(text);
							}
						}
					}
					if (registryKey.SubKeyCount == 0 && registryKey.ValueCount == 0)
					{
						Registry.ClassesRoot.DeleteSubKey("CLSID");
					}
				}
			}
			return flag;
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x000AC934 File Offset: 0x000AB934
		private void RegisterPrimaryInteropAssembly(Assembly assembly, string strAsmCodeBase, PrimaryInteropAssemblyAttribute attr)
		{
			if (assembly.nGetPublicKey().Length == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_PIAMustBeStrongNamed"));
			}
			string text = "{" + Marshal.GetTypeLibGuidForAssembly(assembly).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			string text2 = attr.MajorVersion.ToString("x", CultureInfo.InvariantCulture) + "." + attr.MinorVersion.ToString("x", CultureInfo.InvariantCulture);
			using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey("TypeLib"))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey(text))
				{
					using (RegistryKey registryKey3 = registryKey2.CreateSubKey(text2))
					{
						registryKey3.SetValue("PrimaryInteropAssemblyName", assembly.FullName);
						if (strAsmCodeBase != null)
						{
							registryKey3.SetValue("PrimaryInteropAssemblyCodeBase", strAsmCodeBase);
						}
					}
				}
			}
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000ACA58 File Offset: 0x000ABA58
		private void UnregisterPrimaryInteropAssembly(Assembly assembly, PrimaryInteropAssemblyAttribute attr)
		{
			string text = "{" + Marshal.GetTypeLibGuidForAssembly(assembly).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			string text2 = attr.MajorVersion.ToString("x", CultureInfo.InvariantCulture) + "." + attr.MinorVersion.ToString("x", CultureInfo.InvariantCulture);
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("TypeLib", true))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(text, true))
					{
						if (registryKey2 != null)
						{
							using (RegistryKey registryKey3 = registryKey2.OpenSubKey(text2, true))
							{
								if (registryKey3 != null)
								{
									registryKey3.DeleteValue("PrimaryInteropAssemblyName", false);
									registryKey3.DeleteValue("PrimaryInteropAssemblyCodeBase", false);
									if (registryKey3.SubKeyCount == 0 && registryKey3.ValueCount == 0)
									{
										registryKey2.DeleteSubKey(text2);
									}
								}
							}
							if (registryKey2.SubKeyCount == 0 && registryKey2.ValueCount == 0)
							{
								registryKey.DeleteSubKey(text);
							}
						}
					}
					if (registryKey.SubKeyCount == 0 && registryKey.ValueCount == 0)
					{
						Registry.ClassesRoot.DeleteSubKey("TypeLib");
					}
				}
			}
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000ACBBC File Offset: 0x000ABBBC
		private void EnsureManagedCategoryExists()
		{
			using (RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey("Component Categories"))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}"))
				{
					registryKey2.SetValue("0", ".NET Category");
				}
			}
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x000ACC2C File Offset: 0x000ABC2C
		private void CallUserDefinedRegistrationMethod(Type type, bool bRegister)
		{
			bool flag = false;
			Type type2;
			if (bRegister)
			{
				type2 = typeof(ComRegisterFunctionAttribute);
			}
			else
			{
				type2 = typeof(ComUnregisterFunctionAttribute);
			}
			Type type3 = type;
			while (!flag && type3 != null)
			{
				MethodInfo[] methods = type3.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				int num = methods.Length;
				for (int i = 0; i < num; i++)
				{
					MethodInfo methodInfo = methods[i];
					if (methodInfo.GetCustomAttributes(type2, true).Length != 0)
					{
						if (!methodInfo.IsStatic)
						{
							if (bRegister)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_NonStaticComRegFunction"), new object[] { methodInfo.Name, type3.Name }));
							}
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_NonStaticComUnRegFunction"), new object[] { methodInfo.Name, type3.Name }));
						}
						else
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							if (methodInfo.ReturnType != typeof(void) || parameters == null || parameters.Length != 1 || (parameters[0].ParameterType != typeof(string) && parameters[0].ParameterType != typeof(Type)))
							{
								if (bRegister)
								{
									throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_InvalidComRegFunctionSig"), new object[] { methodInfo.Name, type3.Name }));
								}
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_InvalidComUnRegFunctionSig"), new object[] { methodInfo.Name, type3.Name }));
							}
							else if (flag)
							{
								if (bRegister)
								{
									throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_MultipleComRegFunctions"), new object[] { type3.Name }));
								}
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_MultipleComUnRegFunctions"), new object[] { type3.Name }));
							}
							else
							{
								object[] array = new object[1];
								if (parameters[0].ParameterType == typeof(string))
								{
									array[0] = "HKEY_CLASSES_ROOT\\CLSID\\{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
								}
								else
								{
									array[0] = type;
								}
								methodInfo.Invoke(null, array);
								flag = true;
							}
						}
					}
				}
				type3 = type3.BaseType;
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x000ACEB6 File Offset: 0x000ABEB6
		private Type GetBaseComImportType(Type type)
		{
			while (type != null && !type.IsImport)
			{
				type = type.BaseType;
			}
			return type;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000ACECE File Offset: 0x000ABECE
		private bool IsRegisteredAsValueType(Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060032C2 RID: 12994
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterTypeForComClientsNative(Type type, ref Guid g);

		// Token: 0x060032C3 RID: 12995
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int RegisterTypeForComClientsExNative(Type t, RegistrationClassContext clsContext, RegistrationConnectionType flags);

		// Token: 0x060032C4 RID: 12996
		[DllImport("ole32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
		private static extern void CoRevokeClassObject(int cookie);

		// Token: 0x040019E4 RID: 6628
		private const string strManagedCategoryGuid = "{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}";

		// Token: 0x040019E5 RID: 6629
		private const string strDocStringPrefix = "";

		// Token: 0x040019E6 RID: 6630
		private const string strManagedTypeThreadingModel = "Both";

		// Token: 0x040019E7 RID: 6631
		private const string strComponentCategorySubKey = "Component Categories";

		// Token: 0x040019E8 RID: 6632
		private const string strManagedCategoryDescription = ".NET Category";

		// Token: 0x040019E9 RID: 6633
		private const string strImplementedCategoriesSubKey = "Implemented Categories";

		// Token: 0x040019EA RID: 6634
		private const string strMsCorEEFileName = "mscoree.dll";

		// Token: 0x040019EB RID: 6635
		private const string strRecordRootName = "Record";

		// Token: 0x040019EC RID: 6636
		private const string strClsIdRootName = "CLSID";

		// Token: 0x040019ED RID: 6637
		private const string strTlbRootName = "TypeLib";

		// Token: 0x040019EE RID: 6638
		private static Guid s_ManagedCategoryGuid = new Guid("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
	}
}

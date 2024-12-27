using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x0200007A RID: 122
	internal class AssemblyMetaDataImport : DisposableBase
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x000151D7 File Offset: 0x000141D7
		public AssemblyMetaDataImport(string sourceFile)
		{
			this._metaDispenser = (IMetaDataDispenser)new CorMetaDataDispenser();
			this._assemblyImport = (IMetaDataAssemblyImport)this._metaDispenser.OpenScope(sourceFile, 0U, ref AssemblyMetaDataImport._importerGuid);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001520C File Offset: 0x0001420C
		public AssemblyModule[] Files
		{
			get
			{
				if (this._modules == null)
				{
					lock (this)
					{
						if (this._modules == null)
						{
							this._modules = this.ImportAssemblyFiles();
						}
					}
				}
				return this._modules;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001525C File Offset: 0x0001425C
		public AssemblyName Name
		{
			get
			{
				if (this._name == null)
				{
					lock (this)
					{
						if (this._name == null)
						{
							this._name = this.ImportIdentity();
						}
					}
				}
				return this._name;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000152AC File Offset: 0x000142AC
		public AssemblyReference[] References
		{
			get
			{
				if (this._asmRefs == null)
				{
					lock (this)
					{
						if (this._asmRefs == null)
						{
							this._asmRefs = this.ImportAssemblyReferences();
						}
					}
				}
				return this._asmRefs;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000152FC File Offset: 0x000142FC
		protected override void DisposeUnmanagedResources()
		{
			if (this._assemblyImport != null)
			{
				Marshal.ReleaseComObject(this._assemblyImport);
			}
			if (this._metaDispenser != null)
			{
				Marshal.ReleaseComObject(this._metaDispenser);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00015328 File Offset: 0x00014328
		private AssemblyModule[] ImportAssemblyFiles()
		{
			ArrayList arrayList = new ArrayList();
			IntPtr zero = IntPtr.Zero;
			uint[] array = new uint[16];
			char[] array2 = new char[1024];
			try
			{
				uint num;
				do
				{
					this._assemblyImport.EnumFiles(ref zero, array, (uint)array.Length, out num);
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						uint num3;
						IntPtr intPtr;
						uint num4;
						uint num5;
						this._assemblyImport.GetFileProps(array[(int)((UIntPtr)num2)], array2, (uint)array2.Length, out num3, out intPtr, out num4, out num5);
						byte[] array3 = new byte[num4];
						Marshal.Copy(intPtr, array3, 0, (int)num4);
						arrayList.Add(new AssemblyModule(new string(array2, 0, (int)(num3 - 1U)), array3));
					}
				}
				while (num > 0U);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					this._assemblyImport.CloseEnum(zero);
				}
			}
			return (AssemblyModule[])arrayList.ToArray(typeof(AssemblyModule));
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001540C File Offset: 0x0001440C
		private AssemblyName ImportIdentity()
		{
			uint num;
			this._assemblyImport.GetAssemblyFromScope(out num);
			IntPtr intPtr;
			uint num2;
			uint num3;
			uint num4;
			uint num5;
			this._assemblyImport.GetAssemblyProps(num, out intPtr, out num2, out num3, null, 0U, out num4, IntPtr.Zero, out num5);
			char[] array = new char[num4 + 1U];
			IntPtr intPtr2 = IntPtr.Zero;
			AssemblyName assemblyName;
			try
			{
				intPtr2 = this.AllocAsmMeta();
				this._assemblyImport.GetAssemblyProps(num, out intPtr, out num2, out num3, array, (uint)array.Length, out num4, intPtr2, out num5);
				assemblyName = this.ConstructAssemblyName(intPtr2, array, num4, intPtr, num2, num5);
			}
			finally
			{
				this.FreeAsmMeta(intPtr2);
			}
			return assemblyName;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000154AC File Offset: 0x000144AC
		private AssemblyReference[] ImportAssemblyReferences()
		{
			ArrayList arrayList = new ArrayList();
			IntPtr zero = IntPtr.Zero;
			uint[] array = new uint[16];
			try
			{
				uint num;
				do
				{
					this._assemblyImport.EnumAssemblyRefs(ref zero, array, (uint)array.Length, out num);
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						IntPtr intPtr;
						uint num3;
						uint num4;
						IntPtr intPtr2;
						uint num5;
						uint num6;
						this._assemblyImport.GetAssemblyRefProps(array[(int)((UIntPtr)num2)], out intPtr, out num3, null, 0U, out num4, IntPtr.Zero, out intPtr2, out num5, out num6);
						char[] array2 = new char[num4 + 1U];
						IntPtr intPtr3 = IntPtr.Zero;
						try
						{
							intPtr3 = this.AllocAsmMeta();
							this._assemblyImport.GetAssemblyRefProps(array[(int)((UIntPtr)num2)], out intPtr, out num3, array2, (uint)array2.Length, out num4, intPtr3, out intPtr2, out num5, out num6);
							AssemblyName assemblyName = this.ConstructAssemblyName(intPtr3, array2, num4, intPtr, num3, num6);
							arrayList.Add(new AssemblyReference(assemblyName));
						}
						finally
						{
							this.FreeAsmMeta(intPtr3);
						}
					}
				}
				while (num > 0U);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					this._assemblyImport.CloseEnum(zero);
				}
			}
			return (AssemblyReference[])arrayList.ToArray(typeof(AssemblyReference));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000155DC File Offset: 0x000145DC
		private IntPtr AllocAsmMeta()
		{
			ASSEMBLYMETADATA assemblymetadata;
			assemblymetadata.usMajorVersion = (assemblymetadata.usMinorVersion = (assemblymetadata.usBuildNumber = (assemblymetadata.usRevisionNumber = 0)));
			assemblymetadata.cOses = (assemblymetadata.cProcessors = 0U);
			assemblymetadata.rOses = (assemblymetadata.rpProcessors = IntPtr.Zero);
			assemblymetadata.rpLocale = Marshal.AllocCoTaskMem(128);
			assemblymetadata.cchLocale = 64U;
			int num = Marshal.SizeOf(typeof(ASSEMBLYMETADATA));
			IntPtr intPtr = Marshal.AllocCoTaskMem(num);
			Marshal.StructureToPtr(assemblymetadata, intPtr, false);
			return intPtr;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00015680 File Offset: 0x00014680
		private AssemblyName ConstructAssemblyName(IntPtr asmMetaPtr, char[] asmNameBuf, uint asmNameLength, IntPtr pubKeyPtr, uint pubKeyBytes, uint flags)
		{
			ASSEMBLYMETADATA assemblymetadata = (ASSEMBLYMETADATA)Marshal.PtrToStructure(asmMetaPtr, typeof(ASSEMBLYMETADATA));
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = new string(asmNameBuf, 0, (int)(asmNameLength - 1U));
			assemblyName.Version = new Version((int)assemblymetadata.usMajorVersion, (int)assemblymetadata.usMinorVersion, (int)assemblymetadata.usBuildNumber, (int)assemblymetadata.usRevisionNumber);
			string text = Marshal.PtrToStringUni(assemblymetadata.rpLocale);
			assemblyName.CultureInfo = new CultureInfo(text);
			if (pubKeyBytes > 0U)
			{
				byte[] array = new byte[pubKeyBytes];
				Marshal.Copy(pubKeyPtr, array, 0, (int)pubKeyBytes);
				if ((flags & 1U) != 0U)
				{
					assemblyName.SetPublicKey(array);
				}
				else
				{
					assemblyName.SetPublicKeyToken(array);
				}
			}
			return assemblyName;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001572C File Offset: 0x0001472C
		private void FreeAsmMeta(IntPtr asmMetaPtr)
		{
			if (asmMetaPtr != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(((ASSEMBLYMETADATA)Marshal.PtrToStructure(asmMetaPtr, typeof(ASSEMBLYMETADATA))).rpLocale);
				Marshal.DestroyStructure(asmMetaPtr, typeof(ASSEMBLYMETADATA));
				Marshal.FreeCoTaskMem(asmMetaPtr);
			}
		}

		// Token: 0x040002A5 RID: 677
		private const int GENMAN_STRING_BUF_SIZE = 1024;

		// Token: 0x040002A6 RID: 678
		private const int GENMAN_LOCALE_BUF_SIZE = 64;

		// Token: 0x040002A7 RID: 679
		private const int GENMAN_ENUM_TOKEN_BUF_SIZE = 16;

		// Token: 0x040002A8 RID: 680
		private AssemblyModule[] _modules;

		// Token: 0x040002A9 RID: 681
		private AssemblyName _name;

		// Token: 0x040002AA RID: 682
		private AssemblyReference[] _asmRefs;

		// Token: 0x040002AB RID: 683
		private IMetaDataDispenser _metaDispenser;

		// Token: 0x040002AC RID: 684
		private IMetaDataAssemblyImport _assemblyImport;

		// Token: 0x040002AD RID: 685
		private static Guid _importerGuid = new Guid(((GuidAttribute)Attribute.GetCustomAttribute(typeof(IMetaDataImport), typeof(GuidAttribute), false)).Value);
	}
}

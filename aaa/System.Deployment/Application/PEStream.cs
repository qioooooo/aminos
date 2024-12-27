using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Deployment.Application
{
	// Token: 0x0200009E RID: 158
	internal class PEStream : Stream
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x00015E88 File Offset: 0x00014E88
		public PEStream(string filePath)
		{
			this.ConstructFromFile(filePath, true);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00015EC4 File Offset: 0x00014EC4
		public PEStream(string filePath, bool partialConstruct)
		{
			this.ConstructFromFile(filePath, partialConstruct);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00015F00 File Offset: 0x00014F00
		private void ConstructFromFile(string filePath, bool partialConstruct)
		{
			string fileName = Path.GetFileName(filePath);
			bool flag = false;
			try
			{
				this._peFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				this.ConstructPEImage(this._peFile, partialConstruct);
				flag = true;
			}
			catch (IOException ex)
			{
				throw new IOException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidPEImage"), new object[] { fileName }), ex);
			}
			catch (Win32Exception ex2)
			{
				throw new IOException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidPEImage"), new object[] { fileName }), ex2);
			}
			catch (NotSupportedException ex3)
			{
				throw new IOException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidPEImage"), new object[] { fileName }), ex3);
			}
			finally
			{
				if (!flag && this._peFile != null)
				{
					this._peFile.Close();
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001600C File Offset: 0x0001500C
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00016014 File Offset: 0x00015014
		public override bool CanSeek
		{
			get
			{
				return this._canSeek;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001601C File Offset: 0x0001501C
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001601F File Offset: 0x0001501F
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00016027 File Offset: 0x00015027
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0001602F File Offset: 0x0001502F
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001603A File Offset: 0x0001503A
		public bool IsImageFileDll
		{
			get
			{
				return this._fileHeader.IsImageFileDll;
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00016047 File Offset: 0x00015047
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00016050 File Offset: 0x00015050
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			int num = count;
			long num2 = 0L;
			int num3 = offset;
			foreach (object obj in this._streamComponents)
			{
				PEStream.PEComponent pecomponent = (PEStream.PEComponent)obj;
				if (!flag)
				{
					long num4 = pecomponent.Address + pecomponent.Size - 1L;
					if (this._position <= num4)
					{
						num2 = this._position - pecomponent.Address;
						if (num2 < 0L)
						{
							throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEImage"));
						}
						flag = true;
					}
				}
				if (flag)
				{
					int num5 = pecomponent.Read(buffer, num3, num2, num);
					num3 += num5;
					this._position += (long)num5;
					num -= num5;
					num2 = 0L;
				}
				if (num <= 0)
				{
					break;
				}
			}
			return count - num;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00016140 File Offset: 0x00015140
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
			{
				this._position = offset;
			}
			else if (origin == SeekOrigin.Current)
			{
				this._position += offset;
			}
			else if (origin == SeekOrigin.End)
			{
				this._position = this._length + offset;
			}
			if (this._position < 0L)
			{
				this._position = 0L;
			}
			if (this._position > this._length)
			{
				this._position = this._length;
			}
			return this._position;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000161B1 File Offset: 0x000151B1
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000161B8 File Offset: 0x000151B8
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000161C0 File Offset: 0x000151C0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._peFile != null)
				{
					this._peFile.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00016200 File Offset: 0x00015200
		public void ZeroOutOptionalHeaderCheckSum()
		{
			this._optionalHeader.CheckSum = 0U;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00016210 File Offset: 0x00015210
		public void ZeroOutManifestResource(ushort manifestId, ushort languageId)
		{
			PEStream.ResourceComponent resourceComponent = this.RetrieveResource(new object[] { 24, manifestId, languageId });
			if (resourceComponent != null && resourceComponent is PEStream.ResourceData)
			{
				((PEStream.ResourceData)resourceComponent).ZeroData();
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00016260 File Offset: 0x00015260
		public byte[] GetManifestResource(ushort manifestId, ushort languageId)
		{
			PEStream.ResourceComponent resourceComponent = this.RetrieveResource(new object[] { 24, manifestId, languageId });
			if (resourceComponent != null && resourceComponent is PEStream.ResourceData)
			{
				return ((PEStream.ResourceData)resourceComponent).Data;
			}
			return null;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000162B0 File Offset: 0x000152B0
		public static ushort Id1ManifestId
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000162B3 File Offset: 0x000152B3
		public static ushort Id1ManifestLanguageId
		{
			get
			{
				return 1033;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000162BC File Offset: 0x000152BC
		public byte[] GetDefaultId1ManifestResource()
		{
			PEStream.ResourceData id1ManifestResource = this.GetId1ManifestResource();
			if (id1ManifestResource != null)
			{
				return id1ManifestResource.Data;
			}
			return null;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000162DC File Offset: 0x000152DC
		public void ZeroOutDefaultId1ManifestResource()
		{
			PEStream.ResourceData id1ManifestResource = this.GetId1ManifestResource();
			if (id1ManifestResource != null)
			{
				id1ManifestResource.ZeroData();
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000162FC File Offset: 0x000152FC
		protected PEStream.ResourceData GetId1ManifestResource()
		{
			PEStream.ResourceComponent resourceComponent = this.RetrieveResource(new object[]
			{
				24,
				PEStream.Id1ManifestId
			});
			if (resourceComponent != null && resourceComponent is PEStream.ResourceDirectory)
			{
				PEStream.ResourceDirectory resourceDirectory = (PEStream.ResourceDirectory)resourceComponent;
				if (resourceDirectory.ResourceComponentCount > 1)
				{
					throw new Win32Exception(11, Resources.GetString("Ex_MultipleId1Manifest"));
				}
				if (resourceDirectory.ResourceComponentCount == 1)
				{
					PEStream.ResourceComponent resourceComponent2 = resourceDirectory.GetResourceComponent(0);
					if (resourceComponent2 != null && resourceComponent2 is PEStream.ResourceData)
					{
						return (PEStream.ResourceData)resourceComponent2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00016380 File Offset: 0x00015380
		protected PEStream.ResourceComponent RetrieveResource(object[] keys)
		{
			if (this._resourceSection == null)
			{
				return null;
			}
			PEStream.ResourceDirectory rootResourceDirectory = this._resourceSection.RootResourceDirectory;
			if (rootResourceDirectory == null)
			{
				return null;
			}
			return this.RetrieveResource(rootResourceDirectory, keys, 0U);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000163B4 File Offset: 0x000153B4
		protected PEStream.ResourceComponent RetrieveResource(PEStream.ResourceDirectory resourcesDirectory, object[] keys, uint keyIndex)
		{
			PEStream.ResourceComponent resourceComponent = resourcesDirectory[keys[(int)((UIntPtr)keyIndex)]];
			if ((ulong)keyIndex == (ulong)((long)(keys.Length - 1)))
			{
				return resourceComponent;
			}
			if (resourceComponent is PEStream.ResourceDirectory)
			{
				return this.RetrieveResource((PEStream.ResourceDirectory)resourceComponent, keys, keyIndex + 1U);
			}
			return null;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000163F4 File Offset: 0x000153F4
		protected void ConstructPEImage(FileStream file, bool partialConstruct)
		{
			this._partialConstruct = partialConstruct;
			this._dosHeader = new PEStream.DosHeader(file);
			long num = (long)((ulong)this._dosHeader.NtHeaderPosition - (ulong)(this._dosHeader.Address + this._dosHeader.Size));
			if (num < 0L)
			{
				throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEFormat"));
			}
			this._dosStub = new PEStream.DosStub(file, this._dosHeader.Address + this._dosHeader.Size, num);
			this._ntSignature = new PEStream.NtSignature(file, (long)((ulong)this._dosHeader.NtHeaderPosition));
			this._fileHeader = new PEStream.FileHeader(file, this._ntSignature.Address + this._ntSignature.Size);
			this._optionalHeader = new PEStream.OptionalHeader(file, this._fileHeader.Address + this._fileHeader.Size);
			long num2 = this._optionalHeader.Address + this._optionalHeader.Size;
			int i = 0;
			while ((long)i < (long)((ulong)this._optionalHeader.NumberOfRvaAndSizes))
			{
				PEStream.DataDirectory dataDirectory = new PEStream.DataDirectory(file, num2);
				num2 += dataDirectory.Size;
				this._dataDirectories.Add(dataDirectory);
				i++;
			}
			if ((ulong)this._fileHeader.SizeOfOptionalHeader < (ulong)(this._optionalHeader.Size + (long)((ulong)this._optionalHeader.NumberOfRvaAndSizes * (ulong)((long)Marshal.SizeOf(typeof(PEStream.IMAGE_DATA_DIRECTORY))))))
			{
				throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEFormat"));
			}
			bool flag = false;
			uint num3 = 0U;
			if (this._optionalHeader.NumberOfRvaAndSizes > 2U)
			{
				num3 = ((PEStream.DataDirectory)this._dataDirectories[2]).VirtualAddress;
				flag = true;
			}
			long num4 = this._optionalHeader.Address + (long)((ulong)this._fileHeader.SizeOfOptionalHeader);
			for (i = 0; i < (int)this._fileHeader.NumberOfSections; i++)
			{
				PEStream.SectionHeader sectionHeader = new PEStream.SectionHeader(file, num4);
				PEStream.Section section;
				if (flag && sectionHeader.VirtualAddress == num3)
				{
					section = (this._resourceSection = new PEStream.ResourceSection(file, sectionHeader, partialConstruct));
				}
				else
				{
					section = new PEStream.Section(file, sectionHeader);
				}
				sectionHeader.Section = section;
				this._sectionHeaders.Add(sectionHeader);
				this._sections.Add(section);
				num4 += sectionHeader.Size;
			}
			this.ConstructStream();
			ArrayList arrayList = new ArrayList();
			long num5 = 0L;
			foreach (object obj in this._streamComponents)
			{
				PEStream.PEComponent pecomponent = (PEStream.PEComponent)obj;
				if (pecomponent.Address < num5)
				{
					throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEFormat"));
				}
				if (pecomponent.Address > num5)
				{
					PEStream.PEComponent pecomponent2 = new PEStream.PEComponent(file, num5, pecomponent.Address - num5);
					arrayList.Add(pecomponent2);
				}
				num5 = pecomponent.Address + pecomponent.Size;
			}
			if (num5 < file.Length)
			{
				PEStream.PEComponent pecomponent3 = new PEStream.PEComponent(file, num5, file.Length - num5);
				arrayList.Add(pecomponent3);
			}
			this._streamComponents.AddRange(arrayList);
			this._streamComponents.Sort(new PEStream.PEComponentComparer());
			this._canRead = true;
			this._canSeek = true;
			this._length = file.Length;
			this._position = 0L;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00016758 File Offset: 0x00015758
		protected void ConstructStream()
		{
			this._streamComponents.Clear();
			this._streamComponents.Add(this._dosHeader);
			this._streamComponents.Add(this._dosStub);
			this._streamComponents.Add(this._ntSignature);
			this._streamComponents.Add(this._fileHeader);
			this._streamComponents.Add(this._optionalHeader);
			foreach (object obj in this._dataDirectories)
			{
				PEStream.DataDirectory dataDirectory = (PEStream.DataDirectory)obj;
				this._streamComponents.Add(dataDirectory);
			}
			foreach (object obj2 in this._sectionHeaders)
			{
				PEStream.SectionHeader sectionHeader = (PEStream.SectionHeader)obj2;
				this._streamComponents.Add(sectionHeader);
			}
			foreach (object obj3 in this._sections)
			{
				PEStream.Section section = (PEStream.Section)obj3;
				section.AddComponentsToStream(this._streamComponents);
			}
			this._streamComponents.Sort(new PEStream.PEComponentComparer());
		}

		// Token: 0x04000349 RID: 841
		protected const ushort _id1ManifestId = 1;

		// Token: 0x0400034A RID: 842
		protected const ushort _id1ManifestLanguageId = 1033;

		// Token: 0x0400034B RID: 843
		internal const ushort IMAGE_DOS_SIGNATURE = 23117;

		// Token: 0x0400034C RID: 844
		internal const uint IMAGE_NT_SIGNATURE = 17744U;

		// Token: 0x0400034D RID: 845
		internal const uint IMAGE_NT_OPTIONAL_HDR32_MAGIC = 267U;

		// Token: 0x0400034E RID: 846
		internal const uint IMAGE_NT_OPTIONAL_HDR64_MAGIC = 523U;

		// Token: 0x0400034F RID: 847
		internal const uint IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16U;

		// Token: 0x04000350 RID: 848
		internal const uint IMAGE_FILE_DLL = 8192U;

		// Token: 0x04000351 RID: 849
		protected const uint IMAGE_DIRECTORY_ENTRY_EXPORT = 0U;

		// Token: 0x04000352 RID: 850
		protected const uint IMAGE_DIRECTORY_ENTRY_IMPORT = 1U;

		// Token: 0x04000353 RID: 851
		protected const uint IMAGE_DIRECTORY_ENTRY_RESOURCE = 2U;

		// Token: 0x04000354 RID: 852
		protected const uint IMAGE_DIRECTORY_ENTRY_EXCEPTION = 3U;

		// Token: 0x04000355 RID: 853
		protected const uint IMAGE_DIRECTORY_ENTRY_SECURITY = 4U;

		// Token: 0x04000356 RID: 854
		protected const uint IMAGE_DIRECTORY_ENTRY_BASERELOC = 5U;

		// Token: 0x04000357 RID: 855
		protected const uint IMAGE_DIRECTORY_ENTRY_DEBUG = 6U;

		// Token: 0x04000358 RID: 856
		protected const uint IMAGE_DIRECTORY_ENTRY_ARCHITECTURE = 7U;

		// Token: 0x04000359 RID: 857
		protected const uint IMAGE_DIRECTORY_ENTRY_GLOBALPTR = 8U;

		// Token: 0x0400035A RID: 858
		protected const uint IMAGE_DIRECTORY_ENTRY_TLS = 9U;

		// Token: 0x0400035B RID: 859
		protected const uint IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG = 10U;

		// Token: 0x0400035C RID: 860
		protected const uint IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT = 11U;

		// Token: 0x0400035D RID: 861
		protected const uint IMAGE_DIRECTORY_ENTRY_IAT = 12U;

		// Token: 0x0400035E RID: 862
		protected const uint IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT = 13U;

		// Token: 0x0400035F RID: 863
		protected const uint IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14U;

		// Token: 0x04000360 RID: 864
		protected const uint IMAGE_RESOURCE_NAME_IS_STRING = 2147483648U;

		// Token: 0x04000361 RID: 865
		protected const uint IMAGE_RESOURCE_DATA_IS_DIRECTORY = 2147483648U;

		// Token: 0x04000362 RID: 866
		protected const ushort ManifestDirId = 24;

		// Token: 0x04000363 RID: 867
		protected const int ErrorBadFormat = 11;

		// Token: 0x04000364 RID: 868
		protected bool _canRead;

		// Token: 0x04000365 RID: 869
		protected bool _canSeek;

		// Token: 0x04000366 RID: 870
		protected FileStream _peFile;

		// Token: 0x04000367 RID: 871
		protected long _length;

		// Token: 0x04000368 RID: 872
		protected long _position;

		// Token: 0x04000369 RID: 873
		protected PEStream.StreamComponentList _streamComponents = new PEStream.StreamComponentList();

		// Token: 0x0400036A RID: 874
		protected PEStream.DosHeader _dosHeader;

		// Token: 0x0400036B RID: 875
		protected PEStream.DosStub _dosStub;

		// Token: 0x0400036C RID: 876
		protected PEStream.NtSignature _ntSignature;

		// Token: 0x0400036D RID: 877
		protected PEStream.FileHeader _fileHeader;

		// Token: 0x0400036E RID: 878
		protected PEStream.OptionalHeader _optionalHeader;

		// Token: 0x0400036F RID: 879
		protected ArrayList _dataDirectories = new ArrayList();

		// Token: 0x04000370 RID: 880
		protected ArrayList _sectionHeaders = new ArrayList();

		// Token: 0x04000371 RID: 881
		protected ArrayList _sections = new ArrayList();

		// Token: 0x04000372 RID: 882
		protected PEStream.ResourceSection _resourceSection;

		// Token: 0x04000373 RID: 883
		protected bool _partialConstruct;

		// Token: 0x0200009F RID: 159
		protected class StreamComponentList : ArrayList
		{
			// Token: 0x0600046F RID: 1135 RVA: 0x000168D8 File Offset: 0x000158D8
			public int Add(PEStream.PEComponent peComponent)
			{
				if (peComponent.Size > 0L)
				{
					return this.Add(peComponent);
				}
				return -1;
			}
		}

		// Token: 0x020000A0 RID: 160
		protected class PEComponentComparer : IComparer
		{
			// Token: 0x06000471 RID: 1137 RVA: 0x000168F8 File Offset: 0x000158F8
			public int Compare(object a, object b)
			{
				PEStream.PEComponent pecomponent = (PEStream.PEComponent)a;
				PEStream.PEComponent pecomponent2 = (PEStream.PEComponent)b;
				if (pecomponent.Address > pecomponent2.Address)
				{
					return 1;
				}
				if (pecomponent.Address < pecomponent2.Address)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x020000A1 RID: 161
		protected struct IMAGE_DOS_HEADER
		{
			// Token: 0x04000374 RID: 884
			public ushort e_magic;

			// Token: 0x04000375 RID: 885
			public ushort e_cblp;

			// Token: 0x04000376 RID: 886
			public ushort e_cp;

			// Token: 0x04000377 RID: 887
			public ushort e_crlc;

			// Token: 0x04000378 RID: 888
			public ushort e_cparhdr;

			// Token: 0x04000379 RID: 889
			public ushort e_minalloc;

			// Token: 0x0400037A RID: 890
			public ushort e_maxalloc;

			// Token: 0x0400037B RID: 891
			public ushort e_ss;

			// Token: 0x0400037C RID: 892
			public ushort e_sp;

			// Token: 0x0400037D RID: 893
			public ushort e_csum;

			// Token: 0x0400037E RID: 894
			public ushort e_ip;

			// Token: 0x0400037F RID: 895
			public ushort e_cs;

			// Token: 0x04000380 RID: 896
			public ushort e_lfarlc;

			// Token: 0x04000381 RID: 897
			public ushort e_ovno;

			// Token: 0x04000382 RID: 898
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public ushort[] e_res;

			// Token: 0x04000383 RID: 899
			public ushort e_oemid;

			// Token: 0x04000384 RID: 900
			public ushort e_oeminfo;

			// Token: 0x04000385 RID: 901
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public ushort[] e_res2;

			// Token: 0x04000386 RID: 902
			public uint e_lfanew;
		}

		// Token: 0x020000A2 RID: 162
		protected struct IMAGE_FILE_HEADER
		{
			// Token: 0x04000387 RID: 903
			public ushort Machine;

			// Token: 0x04000388 RID: 904
			public ushort NumberOfSections;

			// Token: 0x04000389 RID: 905
			public uint TimeDateStamp;

			// Token: 0x0400038A RID: 906
			public uint PointerToSymbolTable;

			// Token: 0x0400038B RID: 907
			public uint NumberOfSymbols;

			// Token: 0x0400038C RID: 908
			public ushort SizeOfOptionalHeader;

			// Token: 0x0400038D RID: 909
			public ushort Characteristics;
		}

		// Token: 0x020000A3 RID: 163
		protected struct IMAGE_OPTIONAL_HEADER32
		{
			// Token: 0x0400038E RID: 910
			public ushort Magic;

			// Token: 0x0400038F RID: 911
			public byte MajorLinkerVersion;

			// Token: 0x04000390 RID: 912
			public byte MinorLinkerVersion;

			// Token: 0x04000391 RID: 913
			public uint SizeOfCode;

			// Token: 0x04000392 RID: 914
			public uint SizeOfInitializedData;

			// Token: 0x04000393 RID: 915
			public uint SizeOfUninitializedData;

			// Token: 0x04000394 RID: 916
			public uint AddressOfEntryPoint;

			// Token: 0x04000395 RID: 917
			public uint BaseOfCode;

			// Token: 0x04000396 RID: 918
			public uint BaseOfData;

			// Token: 0x04000397 RID: 919
			public uint ImageBase;

			// Token: 0x04000398 RID: 920
			public uint SectionAlignment;

			// Token: 0x04000399 RID: 921
			public uint FileAlignment;

			// Token: 0x0400039A RID: 922
			public ushort MajorOperatingSystemVersion;

			// Token: 0x0400039B RID: 923
			public ushort MinorOperatingSystemVersion;

			// Token: 0x0400039C RID: 924
			public ushort MajorImageVersion;

			// Token: 0x0400039D RID: 925
			public ushort MinorImageVersion;

			// Token: 0x0400039E RID: 926
			public ushort MajorSubsystemVersion;

			// Token: 0x0400039F RID: 927
			public ushort MinorSubsystemVersion;

			// Token: 0x040003A0 RID: 928
			public uint Win32VersionValue;

			// Token: 0x040003A1 RID: 929
			public uint SizeOfImage;

			// Token: 0x040003A2 RID: 930
			public uint SizeOfHeaders;

			// Token: 0x040003A3 RID: 931
			public uint CheckSum;

			// Token: 0x040003A4 RID: 932
			public ushort Subsystem;

			// Token: 0x040003A5 RID: 933
			public ushort DllCharacteristics;

			// Token: 0x040003A6 RID: 934
			public uint SizeOfStackReserve;

			// Token: 0x040003A7 RID: 935
			public uint SizeOfStackCommit;

			// Token: 0x040003A8 RID: 936
			public uint SizeOfHeapReserve;

			// Token: 0x040003A9 RID: 937
			public uint SizeOfHeapCommit;

			// Token: 0x040003AA RID: 938
			public uint LoaderFlags;

			// Token: 0x040003AB RID: 939
			public uint NumberOfRvaAndSizes;
		}

		// Token: 0x020000A4 RID: 164
		[Serializable]
		protected struct IMAGE_OPTIONAL_HEADER64
		{
			// Token: 0x040003AC RID: 940
			internal ushort Magic;

			// Token: 0x040003AD RID: 941
			internal byte MajorLinkerVersion;

			// Token: 0x040003AE RID: 942
			internal byte MinorLinkerVersion;

			// Token: 0x040003AF RID: 943
			internal uint SizeOfCode;

			// Token: 0x040003B0 RID: 944
			internal uint SizeOfInitializedData;

			// Token: 0x040003B1 RID: 945
			internal uint SizeOfUninitializedData;

			// Token: 0x040003B2 RID: 946
			internal uint AddressOfEntryPoint;

			// Token: 0x040003B3 RID: 947
			internal uint BaseOfCode;

			// Token: 0x040003B4 RID: 948
			internal ulong ImageBase;

			// Token: 0x040003B5 RID: 949
			internal uint SectionAlignment;

			// Token: 0x040003B6 RID: 950
			internal uint FileAlignment;

			// Token: 0x040003B7 RID: 951
			internal ushort MajorOperatingSystemVersion;

			// Token: 0x040003B8 RID: 952
			internal ushort MinorOperatingSystemVersion;

			// Token: 0x040003B9 RID: 953
			internal ushort MajorImageVersion;

			// Token: 0x040003BA RID: 954
			internal ushort MinorImageVersion;

			// Token: 0x040003BB RID: 955
			internal ushort MajorSubsystemVersion;

			// Token: 0x040003BC RID: 956
			internal ushort MinorSubsystemVersion;

			// Token: 0x040003BD RID: 957
			internal uint Win32VersionValue;

			// Token: 0x040003BE RID: 958
			internal uint SizeOfImage;

			// Token: 0x040003BF RID: 959
			internal uint SizeOfHeaders;

			// Token: 0x040003C0 RID: 960
			internal uint CheckSum;

			// Token: 0x040003C1 RID: 961
			internal ushort Subsystem;

			// Token: 0x040003C2 RID: 962
			internal ushort DllCharacteristics;

			// Token: 0x040003C3 RID: 963
			internal ulong SizeOfStackReserve;

			// Token: 0x040003C4 RID: 964
			internal ulong SizeOfStackCommit;

			// Token: 0x040003C5 RID: 965
			internal ulong SizeOfHeapReserve;

			// Token: 0x040003C6 RID: 966
			internal ulong SizeOfHeapCommit;

			// Token: 0x040003C7 RID: 967
			internal uint LoaderFlags;

			// Token: 0x040003C8 RID: 968
			internal uint NumberOfRvaAndSizes;
		}

		// Token: 0x020000A5 RID: 165
		[Serializable]
		protected struct IMAGE_DATA_DIRECTORY
		{
			// Token: 0x040003C9 RID: 969
			public uint VirtualAddress;

			// Token: 0x040003CA RID: 970
			public uint Size;
		}

		// Token: 0x020000A6 RID: 166
		[Serializable]
		protected struct IMAGE_SECTION_HEADER
		{
			// Token: 0x040003CB RID: 971
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Name;

			// Token: 0x040003CC RID: 972
			public uint VirtualSize;

			// Token: 0x040003CD RID: 973
			public uint VirtualAddress;

			// Token: 0x040003CE RID: 974
			public uint SizeOfRawData;

			// Token: 0x040003CF RID: 975
			public uint PointerToRawData;

			// Token: 0x040003D0 RID: 976
			public uint PointerToRelocations;

			// Token: 0x040003D1 RID: 977
			public uint PointerToLinenumbers;

			// Token: 0x040003D2 RID: 978
			public ushort NumberOfRelocations;

			// Token: 0x040003D3 RID: 979
			public ushort NumberOfLinenumbers;

			// Token: 0x040003D4 RID: 980
			public uint Characteristics;
		}

		// Token: 0x020000A7 RID: 167
		[Serializable]
		protected struct IMAGE_RESOURCE_DIRECTORY
		{
			// Token: 0x040003D5 RID: 981
			public uint Characteristics;

			// Token: 0x040003D6 RID: 982
			public uint TimeDateStamp;

			// Token: 0x040003D7 RID: 983
			public ushort MajorVersion;

			// Token: 0x040003D8 RID: 984
			public ushort MinorVersion;

			// Token: 0x040003D9 RID: 985
			public ushort NumberOfNamedEntries;

			// Token: 0x040003DA RID: 986
			public ushort NumberOfIdEntries;
		}

		// Token: 0x020000A8 RID: 168
		[Serializable]
		protected struct IMAGE_RESOURCE_DATA_ENTRY
		{
			// Token: 0x040003DB RID: 987
			public uint OffsetToData;

			// Token: 0x040003DC RID: 988
			public uint Size;

			// Token: 0x040003DD RID: 989
			public uint CodePage;

			// Token: 0x040003DE RID: 990
			public uint Reserved;
		}

		// Token: 0x020000A9 RID: 169
		[Serializable]
		protected struct IMAGE_RESOURCE_DIRECTORY_ENTRY
		{
			// Token: 0x040003DF RID: 991
			public uint Name;

			// Token: 0x040003E0 RID: 992
			public uint OffsetToData;
		}

		// Token: 0x020000AA RID: 170
		protected class PEComponent
		{
			// Token: 0x06000473 RID: 1139 RVA: 0x0001693C File Offset: 0x0001593C
			public PEComponent()
			{
				this._address = 0L;
				this._size = 0L;
				this._data = null;
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x0001695B File Offset: 0x0001595B
			public PEComponent(FileStream file, long address, long size)
			{
				this._address = address;
				this._size = size;
				this._data = new PEStream.DiskDataBlock(file, address, size);
			}

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x06000475 RID: 1141 RVA: 0x0001697F File Offset: 0x0001597F
			public long Address
			{
				get
				{
					return this._address;
				}
			}

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x06000476 RID: 1142 RVA: 0x00016987 File Offset: 0x00015987
			public long Size
			{
				get
				{
					return this._size;
				}
			}

			// Token: 0x06000477 RID: 1143 RVA: 0x00016990 File Offset: 0x00015990
			public virtual int Read(byte[] buffer, int bufferOffset, long sourceOffset, int count)
			{
				int num2;
				if (this._data is PEStream.DataComponent)
				{
					PEStream.DataComponent dataComponent = (PEStream.DataComponent)this._data;
					long num = Math.Min((long)count, this._size - sourceOffset);
					if (num < 0L)
					{
						throw new ArgumentException(Resources.GetString("Ex_InvalidCopyRequest"));
					}
					num2 = dataComponent.Read(buffer, bufferOffset, sourceOffset, (int)num);
				}
				else
				{
					byte[] array = PEStream.PEComponent.ToByteArray(this._data);
					long num3 = Math.Min((long)count, (long)array.Length - sourceOffset);
					if (num3 < 0L)
					{
						throw new ArgumentException(Resources.GetString("Ex_InvalidCopyRequest"));
					}
					Array.Copy(array, (int)sourceOffset, buffer, bufferOffset, (int)num3);
					num2 = (int)num3;
				}
				return num2;
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x00016A30 File Offset: 0x00015A30
			protected static byte[] ToByteArray(object data)
			{
				int num = Marshal.SizeOf(data);
				IntPtr intPtr = Marshal.AllocCoTaskMem(num);
				Marshal.StructureToPtr(data, intPtr, false);
				byte[] array = new byte[num];
				Marshal.Copy(intPtr, array, 0, array.Length);
				Marshal.FreeCoTaskMem(intPtr);
				return array;
			}

			// Token: 0x06000479 RID: 1145 RVA: 0x00016A6C File Offset: 0x00015A6C
			protected static object ReadData(FileStream file, long position, Type dataType)
			{
				int num = Marshal.SizeOf(dataType);
				byte[] array = new byte[num];
				long num2 = file.Seek(position, SeekOrigin.Begin);
				if (num2 != position)
				{
					throw new IOException(Resources.GetString("Ex_NotEnoughDataInFile"));
				}
				int num3 = file.Read(array, 0, array.Length);
				if (num3 < num)
				{
					throw new IOException(Resources.GetString("Ex_NotEnoughDataInFile"));
				}
				IntPtr intPtr = Marshal.AllocCoTaskMem(num);
				Marshal.Copy(array, 0, intPtr, num);
				object obj = Marshal.PtrToStructure(intPtr, dataType);
				Marshal.FreeCoTaskMem(intPtr);
				return obj;
			}

			// Token: 0x0600047A RID: 1146 RVA: 0x00016AE9 File Offset: 0x00015AE9
			protected long CalculateSize(object data)
			{
				return (long)Marshal.SizeOf(data);
			}

			// Token: 0x040003E1 RID: 993
			protected long _address;

			// Token: 0x040003E2 RID: 994
			protected long _size;

			// Token: 0x040003E3 RID: 995
			protected object _data;
		}

		// Token: 0x020000AB RID: 171
		protected class DosHeader : PEStream.PEComponent
		{
			// Token: 0x0600047B RID: 1147 RVA: 0x00016AF4 File Offset: 0x00015AF4
			public DosHeader(FileStream file)
			{
				file.Seek(0L, SeekOrigin.Begin);
				this._dosHeader = (PEStream.IMAGE_DOS_HEADER)PEStream.PEComponent.ReadData(file, 0L, this._dosHeader.GetType());
				if (this._dosHeader.e_magic != 23117)
				{
					throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEImage"));
				}
				this._data = this._dosHeader;
				this._address = 0L;
				this._size = base.CalculateSize(this._dosHeader);
			}

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x0600047C RID: 1148 RVA: 0x00016B88 File Offset: 0x00015B88
			public uint NtHeaderPosition
			{
				get
				{
					return this._dosHeader.e_lfanew;
				}
			}

			// Token: 0x040003E4 RID: 996
			protected PEStream.IMAGE_DOS_HEADER _dosHeader;
		}

		// Token: 0x020000AC RID: 172
		protected class DosStub : PEStream.PEComponent
		{
			// Token: 0x0600047D RID: 1149 RVA: 0x00016B95 File Offset: 0x00015B95
			public DosStub(FileStream file, long startAddress, long size)
			{
				this._address = startAddress;
				this._size = size;
				this._data = new PEStream.DiskDataBlock(file, this._address, this._size);
			}
		}

		// Token: 0x020000AD RID: 173
		protected class NtSignature : PEStream.PEComponent
		{
			// Token: 0x0600047E RID: 1150 RVA: 0x00016BC4 File Offset: 0x00015BC4
			public NtSignature(FileStream file, long address)
			{
				uint num = 0U;
				num = (uint)PEStream.PEComponent.ReadData(file, address, num.GetType());
				if (num != 17744U)
				{
					throw new Win32Exception(11, Resources.GetString("Ex_InvalidPEFormat"));
				}
				this._address = address;
				this._size = base.CalculateSize(num);
				this._data = num;
			}
		}

		// Token: 0x020000AE RID: 174
		protected class FileHeader : PEStream.PEComponent
		{
			// Token: 0x0600047F RID: 1151 RVA: 0x00016C30 File Offset: 0x00015C30
			public FileHeader(FileStream file, long address)
			{
				this._fileHeader = (PEStream.IMAGE_FILE_HEADER)PEStream.PEComponent.ReadData(file, address, this._fileHeader.GetType());
				this._address = address;
				this._size = base.CalculateSize(this._fileHeader);
				this._data = this._fileHeader;
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x06000480 RID: 1152 RVA: 0x00016C94 File Offset: 0x00015C94
			public ushort SizeOfOptionalHeader
			{
				get
				{
					return this._fileHeader.SizeOfOptionalHeader;
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000481 RID: 1153 RVA: 0x00016CA1 File Offset: 0x00015CA1
			public ushort NumberOfSections
			{
				get
				{
					return this._fileHeader.NumberOfSections;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000482 RID: 1154 RVA: 0x00016CAE File Offset: 0x00015CAE
			public bool IsImageFileDll
			{
				get
				{
					return (this._fileHeader.Characteristics & 8192) != 0;
				}
			}

			// Token: 0x040003E5 RID: 997
			protected PEStream.IMAGE_FILE_HEADER _fileHeader;
		}

		// Token: 0x020000AF RID: 175
		protected class OptionalHeader : PEStream.PEComponent
		{
			// Token: 0x06000483 RID: 1155 RVA: 0x00016CC8 File Offset: 0x00015CC8
			public OptionalHeader(FileStream file, long address)
			{
				this._optionalHeader32 = (PEStream.IMAGE_OPTIONAL_HEADER32)PEStream.PEComponent.ReadData(file, address, this._optionalHeader32.GetType());
				if (this._optionalHeader32.Magic == 523)
				{
					this._is64Bit = true;
					this._optionalHeader64 = (PEStream.IMAGE_OPTIONAL_HEADER64)PEStream.PEComponent.ReadData(file, address, this._optionalHeader64.GetType());
					this._size = base.CalculateSize(this._optionalHeader64);
					this._data = this._optionalHeader64;
				}
				else
				{
					if (this._optionalHeader32.Magic != 267)
					{
						throw new NotSupportedException(Resources.GetString("Ex_PEImageTypeNotSupported"));
					}
					this._is64Bit = false;
					this._size = base.CalculateSize(this._optionalHeader32);
					this._data = this._optionalHeader32;
				}
				this._address = address;
			}

			// Token: 0x17000103 RID: 259
			// (set) Token: 0x06000484 RID: 1156 RVA: 0x00016DBC File Offset: 0x00015DBC
			public uint CheckSum
			{
				set
				{
					if (this._is64Bit)
					{
						this._optionalHeader64.CheckSum = value;
						this._data = this._optionalHeader64;
						return;
					}
					this._optionalHeader32.CheckSum = value;
					this._data = this._optionalHeader32;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000485 RID: 1157 RVA: 0x00016E0C File Offset: 0x00015E0C
			public uint NumberOfRvaAndSizes
			{
				get
				{
					if (this._is64Bit)
					{
						return this._optionalHeader64.NumberOfRvaAndSizes;
					}
					return this._optionalHeader32.NumberOfRvaAndSizes;
				}
			}

			// Token: 0x040003E6 RID: 998
			protected PEStream.IMAGE_OPTIONAL_HEADER32 _optionalHeader32;

			// Token: 0x040003E7 RID: 999
			protected PEStream.IMAGE_OPTIONAL_HEADER64 _optionalHeader64;

			// Token: 0x040003E8 RID: 1000
			protected bool _is64Bit;
		}

		// Token: 0x020000B0 RID: 176
		protected class DataDirectory : PEStream.PEComponent
		{
			// Token: 0x06000486 RID: 1158 RVA: 0x00016E30 File Offset: 0x00015E30
			public DataDirectory(FileStream file, long address)
			{
				this._dataDirectory = (PEStream.IMAGE_DATA_DIRECTORY)PEStream.PEComponent.ReadData(file, address, this._dataDirectory.GetType());
				this._address = address;
				this._size = base.CalculateSize(this._dataDirectory);
				this._data = this._dataDirectory;
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000487 RID: 1159 RVA: 0x00016E94 File Offset: 0x00015E94
			public uint VirtualAddress
			{
				get
				{
					return this._dataDirectory.VirtualAddress;
				}
			}

			// Token: 0x040003E9 RID: 1001
			private PEStream.IMAGE_DATA_DIRECTORY _dataDirectory;
		}

		// Token: 0x020000B1 RID: 177
		protected class SectionHeader : PEStream.PEComponent
		{
			// Token: 0x06000488 RID: 1160 RVA: 0x00016EA4 File Offset: 0x00015EA4
			public SectionHeader(FileStream file, long address)
			{
				this._imageSectionHeader = (PEStream.IMAGE_SECTION_HEADER)PEStream.PEComponent.ReadData(file, address, this._imageSectionHeader.GetType());
				this._address = address;
				this._size = base.CalculateSize(this._imageSectionHeader);
				this._data = this._imageSectionHeader;
			}

			// Token: 0x17000106 RID: 262
			// (set) Token: 0x06000489 RID: 1161 RVA: 0x00016F08 File Offset: 0x00015F08
			public PEStream.Section Section
			{
				set
				{
					this._section = value;
				}
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x0600048A RID: 1162 RVA: 0x00016F11 File Offset: 0x00015F11
			public uint VirtualAddress
			{
				get
				{
					return this._imageSectionHeader.VirtualAddress;
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x0600048B RID: 1163 RVA: 0x00016F1E File Offset: 0x00015F1E
			public uint PointerToRawData
			{
				get
				{
					return this._imageSectionHeader.PointerToRawData;
				}
			}

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x0600048C RID: 1164 RVA: 0x00016F2B File Offset: 0x00015F2B
			public uint SizeOfRawData
			{
				get
				{
					return this._imageSectionHeader.SizeOfRawData;
				}
			}

			// Token: 0x040003EA RID: 1002
			protected PEStream.IMAGE_SECTION_HEADER _imageSectionHeader;

			// Token: 0x040003EB RID: 1003
			protected PEStream.Section _section;
		}

		// Token: 0x020000B2 RID: 178
		protected class Section : PEStream.PEComponent
		{
			// Token: 0x0600048D RID: 1165 RVA: 0x00016F38 File Offset: 0x00015F38
			public Section(FileStream file, PEStream.SectionHeader sectionHeader)
			{
				this._address = (long)((ulong)sectionHeader.PointerToRawData);
				this._size = (long)((ulong)sectionHeader.SizeOfRawData);
				this._data = new PEStream.DiskDataBlock(file, this._address, this._size);
				this._sectionHeader = sectionHeader;
			}

			// Token: 0x0600048E RID: 1166 RVA: 0x00016F84 File Offset: 0x00015F84
			public virtual void AddComponentsToStream(PEStream.StreamComponentList stream)
			{
				stream.Add(this);
			}

			// Token: 0x040003EC RID: 1004
			public PEStream.SectionHeader _sectionHeader;
		}

		// Token: 0x020000B3 RID: 179
		protected class ResourceComponent : PEStream.PEComponent
		{
			// Token: 0x0600048F RID: 1167 RVA: 0x00016F8E File Offset: 0x00015F8E
			public virtual void AddComponentsToStream(PEStream.StreamComponentList stream)
			{
				stream.Add(this);
			}
		}

		// Token: 0x020000B4 RID: 180
		protected class ResourceDirectory : PEStream.ResourceComponent
		{
			// Token: 0x06000491 RID: 1169 RVA: 0x00016FA0 File Offset: 0x00015FA0
			public ResourceDirectory(PEStream.ResourceSection resourceSection, FileStream file, long rootResourceAddress, long resourceAddress, long addressDelta, bool partialConstruct)
			{
				this._imageResourceDirectory = (PEStream.IMAGE_RESOURCE_DIRECTORY)PEStream.PEComponent.ReadData(file, resourceAddress, this._imageResourceDirectory.GetType());
				this._address = resourceAddress;
				this._size = base.CalculateSize(this._imageResourceDirectory);
				this._data = this._imageResourceDirectory;
				long num = this._address + this._size;
				for (int i = 0; i < (int)this._imageResourceDirectory.NumberOfIdEntries; i++)
				{
					PEStream.ResourceDirectoryEntry resourceDirectoryEntry = new PEStream.ResourceDirectoryEntry(file, num);
					this._resourceDirectoryEntries.Add(resourceDirectoryEntry);
					num += resourceDirectoryEntry.Size;
				}
				for (int i = 0; i < (int)this._imageResourceDirectory.NumberOfNamedEntries; i++)
				{
					PEStream.ResourceDirectoryEntry resourceDirectoryEntry2 = new PEStream.ResourceDirectoryEntry(file, num);
					this._resourceDirectoryEntries.Add(resourceDirectoryEntry2);
					num += resourceDirectoryEntry2.Size;
				}
				foreach (object obj in this._resourceDirectoryEntries)
				{
					PEStream.ResourceDirectoryEntry resourceDirectoryEntry3 = (PEStream.ResourceDirectoryEntry)obj;
					bool flag = false;
					object obj2;
					if (resourceDirectoryEntry3.NameIsString)
					{
						PEStream.ResourceDirectoryString resourceDirectoryString = resourceSection.CreateResourceDirectoryString(file, rootResourceAddress + resourceDirectoryEntry3.NameOffset);
						obj2 = resourceDirectoryString.NameString;
					}
					else
					{
						obj2 = resourceDirectoryEntry3.Id;
						if (rootResourceAddress == resourceAddress && resourceDirectoryEntry3.Id == 24)
						{
							flag = true;
						}
					}
					resourceDirectoryEntry3.Key = obj2;
					object obj3 = null;
					if (resourceDirectoryEntry3.IsDirectory)
					{
						if (!partialConstruct || (partialConstruct && flag))
						{
							obj3 = new PEStream.ResourceDirectory(resourceSection, file, rootResourceAddress, rootResourceAddress + resourceDirectoryEntry3.OffsetToData, addressDelta, false);
						}
					}
					else
					{
						obj3 = new PEStream.ResourceData(file, rootResourceAddress, rootResourceAddress + resourceDirectoryEntry3.OffsetToData, addressDelta);
					}
					if (obj3 != null)
					{
						this._resourceDirectoryItems.Add(obj2, obj3);
					}
				}
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x00017194 File Offset: 0x00016194
			public override void AddComponentsToStream(PEStream.StreamComponentList stream)
			{
				stream.Add(this);
				foreach (object obj in this._resourceDirectoryEntries)
				{
					PEStream.ResourceDirectoryEntry resourceDirectoryEntry = (PEStream.ResourceDirectoryEntry)obj;
					resourceDirectoryEntry.AddComponentsToStream(stream);
				}
				foreach (object obj2 in this._resourceDirectoryItems.Values)
				{
					PEStream.ResourceComponent resourceComponent = (PEStream.ResourceComponent)obj2;
					resourceComponent.AddComponentsToStream(stream);
				}
			}

			// Token: 0x1700010A RID: 266
			public PEStream.ResourceComponent this[object key]
			{
				get
				{
					if (this._resourceDirectoryItems.Contains(key))
					{
						return (PEStream.ResourceComponent)this._resourceDirectoryItems[key];
					}
					return null;
				}
			}

			// Token: 0x1700010B RID: 267
			// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001726F File Offset: 0x0001626F
			public int ResourceComponentCount
			{
				get
				{
					return this._resourceDirectoryItems.Count;
				}
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x0001727C File Offset: 0x0001627C
			public PEStream.ResourceComponent GetResourceComponent(int index)
			{
				PEStream.ResourceDirectoryEntry resourceDirectoryEntry = (PEStream.ResourceDirectoryEntry)this._resourceDirectoryEntries[index];
				return this[resourceDirectoryEntry.Key];
			}

			// Token: 0x040003ED RID: 1005
			protected PEStream.IMAGE_RESOURCE_DIRECTORY _imageResourceDirectory;

			// Token: 0x040003EE RID: 1006
			protected Hashtable _resourceDirectoryItems = new Hashtable();

			// Token: 0x040003EF RID: 1007
			protected ArrayList _resourceDirectoryEntries = new ArrayList();
		}

		// Token: 0x020000B5 RID: 181
		protected class ResourceDirectoryEntry : PEStream.ResourceComponent
		{
			// Token: 0x06000496 RID: 1174 RVA: 0x000172A8 File Offset: 0x000162A8
			public ResourceDirectoryEntry(FileStream file, long address)
			{
				this._imageResourceDirectoryEntry = (PEStream.IMAGE_RESOURCE_DIRECTORY_ENTRY)PEStream.PEComponent.ReadData(file, address, this._imageResourceDirectoryEntry.GetType());
				this._address = address;
				this._size = base.CalculateSize(this._imageResourceDirectoryEntry);
				this._data = this._imageResourceDirectoryEntry;
			}

			// Token: 0x1700010C RID: 268
			// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001730C File Offset: 0x0001630C
			public long NameOffset
			{
				get
				{
					return (long)((ulong)(this._imageResourceDirectoryEntry.Name & 2147483647U));
				}
			}

			// Token: 0x1700010D RID: 269
			// (get) Token: 0x06000498 RID: 1176 RVA: 0x00017320 File Offset: 0x00016320
			public bool NameIsString
			{
				get
				{
					return (this._imageResourceDirectoryEntry.Name & 2147483648U) != 0U;
				}
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000499 RID: 1177 RVA: 0x00017339 File Offset: 0x00016339
			public ushort Id
			{
				get
				{
					return (ushort)(this._imageResourceDirectoryEntry.Name & 65535U);
				}
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001734D File Offset: 0x0001634D
			public long OffsetToData
			{
				get
				{
					return (long)((ulong)(this._imageResourceDirectoryEntry.OffsetToData & 2147483647U));
				}
			}

			// Token: 0x17000110 RID: 272
			// (get) Token: 0x0600049B RID: 1179 RVA: 0x00017361 File Offset: 0x00016361
			public bool IsDirectory
			{
				get
				{
					return (this._imageResourceDirectoryEntry.OffsetToData & 2147483648U) != 0U;
				}
			}

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001737A File Offset: 0x0001637A
			// (set) Token: 0x0600049D RID: 1181 RVA: 0x00017382 File Offset: 0x00016382
			public object Key
			{
				get
				{
					return this._key;
				}
				set
				{
					this._key = value;
				}
			}

			// Token: 0x040003F0 RID: 1008
			protected PEStream.IMAGE_RESOURCE_DIRECTORY_ENTRY _imageResourceDirectoryEntry;

			// Token: 0x040003F1 RID: 1009
			protected object _key;
		}

		// Token: 0x020000B6 RID: 182
		protected class ResourceDirectoryString : PEStream.ResourceComponent
		{
			// Token: 0x0600049E RID: 1182 RVA: 0x0001738C File Offset: 0x0001638C
			public ResourceDirectoryString(FileStream file, long offset)
			{
				this._length = (ushort)PEStream.PEComponent.ReadData(file, offset, this._length.GetType());
				if (this._length > 0)
				{
					long num = (long)((int)this._length * Marshal.SizeOf(typeof(ushort)));
					this._nameStringBuffer = new byte[num];
					long num2 = offset + base.CalculateSize(this._length);
					long num3 = file.Seek(num2, SeekOrigin.Begin);
					if (num3 != num2)
					{
						throw new IOException(Resources.GetString("Ex_NotEnoughDataInFile"));
					}
					int num4 = file.Read(this._nameStringBuffer, 0, this._nameStringBuffer.Length);
					if ((long)num4 < num)
					{
						throw new IOException(Resources.GetString("Ex_NotEnoughDataInFile"));
					}
					this._nameString = Encoding.Unicode.GetString(this._nameStringBuffer);
					this._address = offset;
					this._size = num + base.CalculateSize(this._length);
				}
				else
				{
					this._nameStringBuffer = null;
					this._nameString = null;
					this._address = offset;
					this._size = base.CalculateSize(this._length);
				}
				this._data = new PEStream.DiskDataBlock(file, this._address, this._size);
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x0600049F RID: 1183 RVA: 0x000174CA File Offset: 0x000164CA
			public string NameString
			{
				get
				{
					return this._nameString;
				}
			}

			// Token: 0x040003F2 RID: 1010
			protected ushort _length;

			// Token: 0x040003F3 RID: 1011
			protected byte[] _nameStringBuffer;

			// Token: 0x040003F4 RID: 1012
			protected string _nameString;
		}

		// Token: 0x020000B7 RID: 183
		protected class ResourceData : PEStream.ResourceComponent
		{
			// Token: 0x060004A0 RID: 1184 RVA: 0x000174D4 File Offset: 0x000164D4
			public ResourceData(FileStream file, long rootResourceAddress, long address, long addressDelta)
			{
				this._resourceDataEntry = (PEStream.IMAGE_RESOURCE_DATA_ENTRY)PEStream.PEComponent.ReadData(file, address, this._resourceDataEntry.GetType());
				this._resourceRawData = new PEStream.ResourceRawData(file, (long)((ulong)this._resourceDataEntry.OffsetToData - (ulong)addressDelta), (long)((ulong)this._resourceDataEntry.Size));
				this._address = address;
				this._size = base.CalculateSize(this._resourceDataEntry);
				this._data = this._resourceDataEntry;
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x0001755F File Offset: 0x0001655F
			public override void AddComponentsToStream(PEStream.StreamComponentList stream)
			{
				stream.Add(this);
				stream.Add(this._resourceRawData);
			}

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00017576 File Offset: 0x00016576
			public byte[] Data
			{
				get
				{
					return this._resourceRawData.Data;
				}
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x00017583 File Offset: 0x00016583
			public void ZeroData()
			{
				this._resourceRawData.ZeroData();
			}

			// Token: 0x040003F5 RID: 1013
			protected PEStream.IMAGE_RESOURCE_DATA_ENTRY _resourceDataEntry;

			// Token: 0x040003F6 RID: 1014
			protected PEStream.ResourceRawData _resourceRawData;
		}

		// Token: 0x020000B8 RID: 184
		protected class ResourceRawData : PEStream.ResourceComponent
		{
			// Token: 0x060004A4 RID: 1188 RVA: 0x00017590 File Offset: 0x00016590
			public ResourceRawData(FileStream file, long address, long size)
			{
				this._address = address;
				this._size = size;
				this._data = new PEStream.DiskDataBlock(file, address, size);
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x000175B4 File Offset: 0x000165B4
			public void ZeroData()
			{
				this._data = new PEStream.BlankDataBlock(this._size);
			}

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x060004A6 RID: 1190 RVA: 0x000175C8 File Offset: 0x000165C8
			public byte[] Data
			{
				get
				{
					byte[] array = new byte[this._size];
					if (this._data is PEStream.DataComponent)
					{
						((PEStream.DataComponent)this._data).Read(array, 0, 0L, array.Length);
						return array;
					}
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x020000B9 RID: 185
		protected class ResourceSection : PEStream.Section
		{
			// Token: 0x060004A7 RID: 1191 RVA: 0x00017610 File Offset: 0x00016610
			public ResourceSection(FileStream file, PEStream.SectionHeader sectionHeader, bool partialConstruct)
				: base(file, sectionHeader)
			{
				this._resourceDirectory = new PEStream.ResourceDirectory(this, file, (long)((ulong)sectionHeader.PointerToRawData), (long)((ulong)sectionHeader.PointerToRawData), (long)((ulong)sectionHeader.VirtualAddress - (ulong)sectionHeader.PointerToRawData), partialConstruct);
				this._address = 0L;
				this._size = 0L;
				this._data = null;
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x00017674 File Offset: 0x00016674
			public PEStream.ResourceDirectoryString CreateResourceDirectoryString(FileStream file, long offset)
			{
				foreach (object obj in this._resourceDirectoryStrings)
				{
					PEStream.ResourceDirectoryString resourceDirectoryString = (PEStream.ResourceDirectoryString)obj;
					if (resourceDirectoryString.Address == offset)
					{
						return resourceDirectoryString;
					}
				}
				PEStream.ResourceDirectoryString resourceDirectoryString2 = new PEStream.ResourceDirectoryString(file, offset);
				this._resourceDirectoryStrings.Add(resourceDirectoryString2);
				return resourceDirectoryString2;
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x000176F0 File Offset: 0x000166F0
			public override void AddComponentsToStream(PEStream.StreamComponentList stream)
			{
				this._resourceDirectory.AddComponentsToStream(stream);
				foreach (object obj in this._resourceDirectoryStrings)
				{
					PEStream.ResourceDirectoryString resourceDirectoryString = (PEStream.ResourceDirectoryString)obj;
					resourceDirectoryString.AddComponentsToStream(stream);
				}
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x060004AA RID: 1194 RVA: 0x00017758 File Offset: 0x00016758
			public PEStream.ResourceDirectory RootResourceDirectory
			{
				get
				{
					return this._resourceDirectory;
				}
			}

			// Token: 0x040003F7 RID: 1015
			protected PEStream.ResourceDirectory _resourceDirectory;

			// Token: 0x040003F8 RID: 1016
			protected ArrayList _resourceDirectoryStrings = new ArrayList();
		}

		// Token: 0x020000BA RID: 186
		protected abstract class DataComponent
		{
			// Token: 0x060004AB RID: 1195
			public abstract int Read(byte[] buffer, int bufferOffset, long sourceOffset, int count);
		}

		// Token: 0x020000BB RID: 187
		protected class DiskDataBlock : PEStream.DataComponent
		{
			// Token: 0x060004AD RID: 1197 RVA: 0x00017768 File Offset: 0x00016768
			public DiskDataBlock(FileStream file, long address, long size)
			{
				this._address = address;
				this._size = size;
				this._file = file;
			}

			// Token: 0x060004AE RID: 1198 RVA: 0x00017788 File Offset: 0x00016788
			public override int Read(byte[] buffer, int bufferOffset, long sourceOffset, int count)
			{
				int num = (int)Math.Min((long)count, this._size - sourceOffset);
				if (num < 0)
				{
					throw new ArgumentException(Resources.GetString("Ex_InvalidCopyRequest"));
				}
				this._file.Seek(this._address + sourceOffset, SeekOrigin.Begin);
				return this._file.Read(buffer, bufferOffset, num);
			}

			// Token: 0x040003F9 RID: 1017
			public long _address;

			// Token: 0x040003FA RID: 1018
			public long _size;

			// Token: 0x040003FB RID: 1019
			public FileStream _file;
		}

		// Token: 0x020000BC RID: 188
		protected class BlankDataBlock : PEStream.DataComponent
		{
			// Token: 0x060004AF RID: 1199 RVA: 0x000177E2 File Offset: 0x000167E2
			public BlankDataBlock(long size)
			{
				this._size = size;
			}

			// Token: 0x060004B0 RID: 1200 RVA: 0x000177F4 File Offset: 0x000167F4
			public override int Read(byte[] buffer, int bufferOffset, long sourceOffset, int count)
			{
				int num = (int)Math.Min((long)count, this._size - sourceOffset);
				if (num < 0)
				{
					throw new ArgumentException(Resources.GetString("Ex_InvalidCopyRequest"));
				}
				for (int i = 0; i < num; i++)
				{
					buffer[bufferOffset + i] = 0;
				}
				return num;
			}

			// Token: 0x040003FC RID: 1020
			public long _size;
		}
	}
}

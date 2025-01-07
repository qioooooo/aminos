using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public sealed class FileSystem
	{
		private static DateTimeFormatInfo InitializeWriteDateFormatInfo()
		{
			return new DateTimeFormatInfo
			{
				DateSeparator = "-",
				ShortDatePattern = "\\#yyyy-MM-dd\\#",
				LongTimePattern = "\\#HH:mm:ss\\#",
				FullDateTimePattern = "\\#yyyy-MM-dd HH:mm:ss\\#"
			};
		}

		public static void ChDir(string Path)
		{
			Path = Strings.RTrim(Path);
			if (Path == null || Path.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty")), 52);
			}
			if (Operators.CompareString(Path, "\\", false) == 0)
			{
				Path = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
			}
			try
			{
				Directory.SetCurrentDirectory(Path);
			}
			catch (FileNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(new FileNotFoundException(Utils.GetResourceString("FileSystem_PathNotFound1", new string[] { Path })), 76);
			}
		}

		public static void ChDrive(char Drive)
		{
			Drive = char.ToUpper(Drive, CultureInfo.InvariantCulture);
			if (Drive < 'A' || Drive > 'Z')
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Drive" }));
			}
			if (!FileSystem.UnsafeValidDrive(Drive))
			{
				throw ExceptionUtils.VbMakeException(new IOException(Utils.GetResourceString("FileSystem_DriveNotFound1", new string[] { Conversions.ToString(Drive) })), 68);
			}
			Directory.SetCurrentDirectory(Conversions.ToString(Drive) + Conversions.ToString(Path.VolumeSeparatorChar));
		}

		public static void ChDrive(string Drive)
		{
			if (Drive != null)
			{
				if (Drive.Length == 0)
				{
					return;
				}
				FileSystem.ChDrive(Drive[0]);
			}
		}

		public static string CurDir()
		{
			return Directory.GetCurrentDirectory();
		}

		public static string CurDir(char Drive)
		{
			Drive = char.ToUpper(Drive, CultureInfo.InvariantCulture);
			if (Drive < 'A' || Drive > 'Z')
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Drive" })), 68);
			}
			string fullPath = Path.GetFullPath(Conversions.ToString(Drive) + Conversions.ToString(Path.VolumeSeparatorChar) + ".");
			if (!FileSystem.UnsafeValidDrive(Drive))
			{
				throw ExceptionUtils.VbMakeException(new IOException(Utils.GetResourceString("FileSystem_DriveNotFound1", new string[] { Conversions.ToString(Drive) })), 68);
			}
			return fullPath;
		}

		public static string Dir()
		{
			return IOUtils.FindNextFile(Assembly.GetCallingAssembly());
		}

		public static string Dir(string PathName, FileAttribute Attributes = FileAttribute.Normal)
		{
			if (Attributes != FileAttribute.Volume)
			{
				FileAttributes fileAttributes = (FileAttributes)(Attributes | (FileAttribute)128);
				return IOUtils.FindFirstFile(Assembly.GetCallingAssembly(), PathName, fileAttributes);
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			string text = null;
			if (PathName.Length > 0)
			{
				text = Path.GetPathRoot(PathName);
				if (text[checked(text.Length - 1)] != Path.DirectorySeparatorChar)
				{
					text += Conversions.ToString(Path.DirectorySeparatorChar);
				}
			}
			string text2 = text;
			StringBuilder stringBuilder2 = stringBuilder;
			int num = 256;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			IntPtr intPtr;
			int volumeInformation = NativeMethods.GetVolumeInformation(text2, stringBuilder2, num, ref num2, ref num3, ref num4, intPtr, 0);
			if (volumeInformation != 0)
			{
				return stringBuilder.ToString();
			}
			return "";
		}

		public static void MkDir(string Path)
		{
			if (Path == null || Path.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty")), 52);
			}
			if (Directory.Exists(Path))
			{
				throw ExceptionUtils.VbMakeException(75);
			}
			Directory.CreateDirectory(Path);
		}

		public static void RmDir(string Path)
		{
			if (Path == null || Path.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty")), 52);
			}
			try
			{
				Directory.Delete(Path);
			}
			catch (DirectoryNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(ex, 76);
			}
			catch (StackOverflowException ex2)
			{
				throw ex2;
			}
			catch (OutOfMemoryException ex3)
			{
				throw ex3;
			}
			catch (ThreadAbortException ex4)
			{
				throw ex4;
			}
			catch (Exception ex5)
			{
				throw ExceptionUtils.VbMakeException(ex5, 75);
			}
		}

		private static bool PathContainsWildcards(string Path)
		{
			return Path != null && (Path.IndexOf('*') != -1 || Path.IndexOf('?') != -1);
		}

		public static void FileCopy(string Source, string Destination)
		{
			if (Source == null || Source.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty1", new string[] { "Source" })), 52);
			}
			if (Destination == null || Destination.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty1", new string[] { "Destination" })), 52);
			}
			if (FileSystem.PathContainsWildcards(Source))
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Source" })), 52);
			}
			if (FileSystem.PathContainsWildcards(Destination))
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Destination" })), 52);
			}
			AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(Assembly.GetCallingAssembly());
			if (FileSystem.CheckFileOpen(assemblyData, Destination, OpenModeTypes.Output))
			{
				throw ExceptionUtils.VbMakeException(new IOException(Utils.GetResourceString("FileSystem_FileAlreadyOpen1", new string[] { Destination })), 55);
			}
			if (FileSystem.CheckFileOpen(assemblyData, Source, OpenModeTypes.Input))
			{
				throw ExceptionUtils.VbMakeException(new IOException(Utils.GetResourceString("FileSystem_FileAlreadyOpen1", new string[] { Source })), 55);
			}
			try
			{
				File.Copy(Source, Destination, true);
				File.SetAttributes(Destination, FileAttributes.Archive);
			}
			catch (FileNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(ex, 53);
			}
			catch (IOException ex2)
			{
				throw ExceptionUtils.VbMakeException(ex2, 55);
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
		}

		public static DateTime FileDateTime(string PathName)
		{
			if (FileSystem.PathContainsWildcards(PathName))
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "PathName" })), 52);
			}
			if (File.Exists(PathName))
			{
				return new FileInfo(PathName).LastWriteTime;
			}
			throw new FileNotFoundException(Utils.GetResourceString("FileSystem_FileNotFound1", new string[] { PathName }));
		}

		public static long FileLen(string PathName)
		{
			if (File.Exists(PathName))
			{
				return new FileInfo(PathName).Length;
			}
			throw new FileNotFoundException(Utils.GetResourceString("FileSystem_FileNotFound1", new string[] { PathName }));
		}

		public static FileAttribute GetAttr(string PathName)
		{
			char[] array = new char[] { '*', '?' };
			if (PathName.IndexOfAny(array) >= 0)
			{
				throw ExceptionUtils.VbMakeException(52);
			}
			FileInfo fileInfo = new FileInfo(PathName);
			if (fileInfo.Exists)
			{
				return (FileAttribute)(fileInfo.Attributes & (FileAttributes)63);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(PathName);
			if (directoryInfo.Exists)
			{
				return (FileAttribute)(directoryInfo.Attributes & (FileAttributes)63);
			}
			if (Path.GetFileName(PathName).Length == 0)
			{
				throw ExceptionUtils.VbMakeException(52);
			}
			throw new FileNotFoundException(Utils.GetResourceString("FileSystem_FileNotFound1", new string[] { PathName }));
		}

		public static void Kill(string PathName)
		{
			string text = Path.GetDirectoryName(PathName);
			string text2;
			if (text == null || text.Length == 0)
			{
				text = Environment.CurrentDirectory;
				text2 = PathName;
			}
			else
			{
				text2 = Path.GetFileName(PathName);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			FileInfo[] files = directoryInfo.GetFiles(text2);
			text += Conversions.ToString(Path.PathSeparator);
			checked
			{
				int num2;
				if (files != null)
				{
					int num = 0;
					int upperBound = files.GetUpperBound(0);
					for (int i = num; i <= upperBound; i++)
					{
						FileInfo fileInfo = files[i];
						if ((fileInfo.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == (FileAttributes)0)
						{
							text2 = fileInfo.FullName;
							AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(Assembly.GetCallingAssembly());
							if (FileSystem.CheckFileOpen(assemblyData, text2, OpenModeTypes.Any))
							{
								throw ExceptionUtils.VbMakeException(new IOException(Utils.GetResourceString("FileSystem_FileAlreadyOpen1", new string[] { text2 })), 55);
							}
							try
							{
								File.Delete(text2);
								num2++;
							}
							catch (IOException ex)
							{
								throw ExceptionUtils.VbMakeException(ex, 55);
							}
							catch (Exception ex2)
							{
								throw ex2;
							}
						}
					}
				}
				if (num2 == 0)
				{
					throw new FileNotFoundException(Utils.GetResourceString("KILL_NoFilesFound1", new string[] { PathName }));
				}
			}
		}

		public static void SetAttr(string PathName, FileAttribute Attributes)
		{
			if (PathName == null || PathName.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty")), 52);
			}
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(callingAssembly);
			FileSystem.VB6CheckPathname(assemblyData, PathName, OpenMode.Input);
			if ((Attributes | (FileAttribute.ReadOnly | FileAttribute.Hidden | FileAttribute.System | FileAttribute.Archive)) != (FileAttribute.ReadOnly | FileAttribute.Hidden | FileAttribute.System | FileAttribute.Archive))
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Attributes" }));
			}
			File.SetAttributes(PathName, (FileAttributes)Attributes);
		}

		private static bool UnsafeValidDrive(char cDrive)
		{
			int num = (int)(checked(cDrive - 'A'));
			return ((long)UnsafeNativeMethods.GetLogicalDrives() & checked((long)Math.Round(Math.Pow(2.0, (double)num)))) != 0L;
		}

		private static void ValidateAccess(OpenAccess Access)
		{
			if (Access != OpenAccess.Default && Access != OpenAccess.Read && Access != OpenAccess.ReadWrite && Access != OpenAccess.Write)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Access" }));
			}
		}

		private static void ValidateShare(OpenShare Share)
		{
			if (Share != OpenShare.Default && Share != OpenShare.Shared && Share != OpenShare.LockRead && Share != OpenShare.LockReadWrite && Share != OpenShare.LockWrite)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Share" }));
			}
		}

		private static void ValidateMode(OpenMode Mode)
		{
			if (Mode != OpenMode.Input && Mode != OpenMode.Output && Mode != OpenMode.Random && Mode != OpenMode.Append && Mode != OpenMode.Binary)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Mode" }));
			}
		}

		public static void FileOpen(int FileNumber, string FileName, OpenMode Mode, OpenAccess Access = OpenAccess.Default, OpenShare Share = OpenShare.Default, int RecordLength = -1)
		{
			try
			{
				FileSystem.ValidateMode(Mode);
				FileSystem.ValidateAccess(Access);
				FileSystem.ValidateShare(Share);
				if (FileNumber < 1 || FileNumber > 255)
				{
					throw ExceptionUtils.VbMakeException(52);
				}
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.vbIOOpenFile(callingAssembly, FileNumber, FileName, Mode, Access, Share, RecordLength);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileClose(params int[] FileNumbers)
		{
			checked
			{
				try
				{
					Assembly callingAssembly = Assembly.GetCallingAssembly();
					AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(callingAssembly);
					if (FileNumbers == null || FileNumbers.Length == 0)
					{
						FileSystem.CloseAllFiles(assemblyData);
					}
					else
					{
						int num = 0;
						int upperBound = FileNumbers.GetUpperBound(0);
						for (int i = num; i <= upperBound; i++)
						{
							FileSystem.InternalCloseFile(assemblyData, FileNumbers[i]);
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		private static void ValidateGetPutRecordNumber(long RecordNumber)
		{
			if (RecordNumber < 1L && RecordNumber != -1L)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "RecordNumber" })), 63);
			}
		}

		public static void FileGetObject(int FileNumber, ref object Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).GetObject(ref Value, RecordNumber, true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref ValueType Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref Array Value, long RecordNumber = -1L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber, ArrayIsDynamic, StringIsFixedLength);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref bool Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref byte Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref short Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref int Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref long Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref char Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref float Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref double Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref decimal Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref string Value, long RecordNumber = -1L, bool StringIsFixedLength = false)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber, StringIsFixedLength);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FileGet(int FileNumber, ref DateTime Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Get(ref Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePutObject(int FileNumber, object Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).PutObject(Value, RecordNumber, true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[Obsolete("This member has been deprecated. Please use FilePutObject to write Object types, or coerce FileNumber and RecordNumber to Integer for writing non-Object types. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static void FilePut(object FileNumber, object Value, object RecordNumber = -1)
		{
			throw new ArgumentException(Utils.GetResourceString("UseFilePutObject"));
		}

		public static void FilePut(int FileNumber, ValueType Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, Array Value, long RecordNumber = -1L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber, ArrayIsDynamic, StringIsFixedLength);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, bool Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, byte Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, short Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, int Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, long Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, char Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, float Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, double Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, decimal Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, string Value, long RecordNumber = -1L, bool StringIsFixedLength = false)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber, StringIsFixedLength);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void FilePut(int FileNumber, DateTime Value, long RecordNumber = -1L)
		{
			try
			{
				FileSystem.ValidateGetPutRecordNumber(RecordNumber);
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber, (OpenModeTypes)36).Put(Value, RecordNumber);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Print(int FileNumber, params object[] Output)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Print(Output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void PrintLine(int FileNumber, params object[] Output)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).PrintLine(Output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref object Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref bool Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref byte Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref short Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref int Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref long Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref char Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref float Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref double Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref decimal Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref string Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Input(int FileNumber, ref DateTime Value)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).Input(ref Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void Write(int FileNumber, params object[] Output)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).WriteHelper(Output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void WriteLine(int FileNumber, params object[] Output)
		{
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				FileSystem.GetStream(callingAssembly, FileNumber).WriteLineHelper(Output);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string InputString(int FileNumber, int CharCount)
		{
			string text;
			try
			{
				if (CharCount < 0 || (double)CharCount > 1073741823.5)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "CharCount" }));
				}
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				VB6File channelObj = FileSystem.GetChannelObj(callingAssembly, FileNumber);
				channelObj.Lock();
				try
				{
					text = channelObj.InputString(CharCount);
				}
				finally
				{
					channelObj.Unlock();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static string LineInput(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			VB6File stream = FileSystem.GetStream(callingAssembly, FileNumber);
			FileSystem.CheckInputCapable(stream);
			if (stream.EOF())
			{
				throw ExceptionUtils.VbMakeException(62);
			}
			return stream.LineInput();
		}

		public static void Lock(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Lock();
		}

		public static void Lock(int FileNumber, long Record)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Lock(Record);
		}

		public static void Lock(int FileNumber, long FromRecord, long ToRecord)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Lock(FromRecord, ToRecord);
		}

		public static void Unlock(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Unlock();
		}

		public static void Unlock(int FileNumber, long Record)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Unlock(Record);
		}

		public static void Unlock(int FileNumber, long FromRecord, long ToRecord)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Unlock(FromRecord, ToRecord);
		}

		public static void FileWidth(int FileNumber, int RecordWidth)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).SetWidth(RecordWidth);
		}

		public static int FreeFile()
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(callingAssembly);
			int num = 1;
			checked
			{
				while (assemblyData.GetChannelObj(num) != null)
				{
					num++;
					if (num > 255)
					{
						throw ExceptionUtils.VbMakeException(67);
					}
				}
				return num;
			}
		}

		public static void Seek(int FileNumber, long Position)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			FileSystem.GetStream(callingAssembly, FileNumber).Seek(Position);
		}

		public static long Seek(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			return FileSystem.GetStream(callingAssembly, FileNumber).Seek();
		}

		public static bool EOF(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			return FileSystem.GetStream(callingAssembly, FileNumber).EOF();
		}

		public static long Loc(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			return FileSystem.GetStream(callingAssembly, FileNumber).LOC();
		}

		public static long LOF(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			return FileSystem.GetStream(callingAssembly, FileNumber).LOF();
		}

		public static TabInfo TAB()
		{
			TabInfo tabInfo;
			tabInfo.Column = -1;
			return tabInfo;
		}

		public static TabInfo TAB(short Column)
		{
			if (Column < 1)
			{
				Column = 1;
			}
			TabInfo tabInfo;
			tabInfo.Column = Column;
			return tabInfo;
		}

		public static SpcInfo SPC(short Count)
		{
			if (Count < 1)
			{
				Count = 0;
			}
			SpcInfo spcInfo;
			spcInfo.Count = Count;
			return spcInfo;
		}

		public static OpenMode FileAttr(int FileNumber)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			return FileSystem.GetStream(callingAssembly, FileNumber).GetMode();
		}

		public static void Reset()
		{
			FileSystem.CloseAllFiles(Assembly.GetCallingAssembly());
		}

		public static void Rename(string OldPath, string NewPath)
		{
			AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(Assembly.GetCallingAssembly());
			OldPath = FileSystem.VB6CheckPathname(assemblyData, OldPath, (OpenMode)(-1));
			NewPath = FileSystem.VB6CheckPathname(assemblyData, NewPath, (OpenMode)(-1));
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, OldPath).Demand();
			new FileIOPermission(FileIOPermissionAccess.Write, NewPath).Demand();
			int num = UnsafeNativeMethods.MoveFile(OldPath, NewPath);
			if (num != 0)
			{
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			int num2 = lastWin32Error;
			if (num2 == 2)
			{
				throw ExceptionUtils.VbMakeException(53);
			}
			if (num2 == 80 || num2 == 183)
			{
				throw ExceptionUtils.VbMakeException(58);
			}
			if (num2 == 12)
			{
				throw ExceptionUtils.VbMakeException(75);
			}
			if (num2 == 17)
			{
				throw ExceptionUtils.VbMakeException(74);
			}
			throw ExceptionUtils.VbMakeException(5);
		}

		private static VB6File GetStream(Assembly assem, int FileNumber)
		{
			return FileSystem.GetStream(assem, FileNumber, (OpenModeTypes)47);
		}

		private static VB6File GetStream(Assembly assem, int FileNumber, OpenModeTypes mode)
		{
			if (FileNumber < 1 || FileNumber > 255)
			{
				throw ExceptionUtils.VbMakeException(52);
			}
			VB6File channelObj = FileSystem.GetChannelObj(assem, FileNumber);
			if ((FileSystem.OpenModeTypesFromOpenMode(channelObj.GetMode()) | mode) == (OpenModeTypes)0)
			{
				throw ExceptionUtils.VbMakeException(54);
			}
			return channelObj;
		}

		private static OpenModeTypes OpenModeTypesFromOpenMode(OpenMode om)
		{
			if (om == OpenMode.Input)
			{
				return OpenModeTypes.Input;
			}
			if (om == OpenMode.Output)
			{
				return OpenModeTypes.Output;
			}
			if (om == OpenMode.Append)
			{
				return OpenModeTypes.Append;
			}
			if (om == OpenMode.Binary)
			{
				return OpenModeTypes.Binary;
			}
			if (om == OpenMode.Random)
			{
				return OpenModeTypes.Random;
			}
			if (om == (OpenMode)(-1))
			{
				return OpenModeTypes.Any;
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"), "om");
		}

		internal static void CloseAllFiles(Assembly assem)
		{
			FileSystem.CloseAllFiles(ProjectData.GetProjectData().GetAssemblyData(assem));
		}

		internal static void CloseAllFiles(AssemblyData oAssemblyData)
		{
			int num = 1;
			checked
			{
				do
				{
					FileSystem.InternalCloseFile(oAssemblyData, num);
					num++;
				}
				while (num <= 255);
			}
		}

		private static void InternalCloseFile(AssemblyData oAssemblyData, int FileNumber)
		{
			if (FileNumber == 0)
			{
				FileSystem.CloseAllFiles(oAssemblyData);
				return;
			}
			VB6File channelOrNull = FileSystem.GetChannelOrNull(oAssemblyData, FileNumber);
			if (channelOrNull != null)
			{
				oAssemblyData.SetChannelObj(FileNumber, null);
				if (channelOrNull != null)
				{
					channelOrNull.CloseFile();
				}
			}
		}

		internal static string VB6CheckPathname(AssemblyData oAssemblyData, string sPath, OpenMode mode)
		{
			if (sPath.IndexOf('?') != -1 || sPath.IndexOf('*') != -1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidPathChars1", new string[] { sPath }));
			}
			string fullName = new FileInfo(sPath).FullName;
			if (FileSystem.CheckFileOpen(oAssemblyData, fullName, FileSystem.OpenModeTypesFromOpenMode(mode)))
			{
				throw ExceptionUtils.VbMakeException(55);
			}
			return fullName;
		}

		internal static bool CheckFileOpen(AssemblyData oAssemblyData, string sPath, OpenModeTypes NewFileMode)
		{
			int num = 255;
			int num2 = 1;
			int num3 = num;
			checked
			{
				for (int i = num2; i <= num3; i++)
				{
					VB6File channelOrNull = FileSystem.GetChannelOrNull(oAssemblyData, i);
					if (channelOrNull != null)
					{
						OpenMode mode = channelOrNull.GetMode();
						if (string.Compare(sPath, channelOrNull.GetAbsolutePath(), StringComparison.OrdinalIgnoreCase) == 0)
						{
							if (NewFileMode == OpenModeTypes.Any)
							{
								return true;
							}
							if ((NewFileMode | (OpenModeTypes)mode) != OpenModeTypes.Input && (NewFileMode | (OpenModeTypes)mode | OpenModeTypes.Binary | OpenModeTypes.Random) != (OpenModeTypes)36)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		private static void vbIOOpenFile(Assembly assem, int FileNumber, string FileName, OpenMode Mode, OpenAccess Access, OpenShare Share, int RecordLength)
		{
			AssemblyData assemblyData = ProjectData.GetProjectData().GetAssemblyData(assem);
			if (FileSystem.GetChannelOrNull(assemblyData, FileNumber) != null)
			{
				throw ExceptionUtils.VbMakeException(55);
			}
			if (FileName == null || FileName.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(75);
			}
			FileName = new FileInfo(FileName).FullName;
			if (FileSystem.CheckFileOpen(assemblyData, FileName, FileSystem.OpenModeTypesFromOpenMode(Mode)))
			{
				throw ExceptionUtils.VbMakeException(55);
			}
			if (RecordLength != -1 && RecordLength <= 0)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			if (Mode == OpenMode.Binary)
			{
				RecordLength = 1;
			}
			else if (RecordLength == -1)
			{
				if (Mode == OpenMode.Random)
				{
					RecordLength = 128;
				}
				else
				{
					RecordLength = 512;
				}
			}
			if (Share == OpenShare.Default)
			{
				Share = OpenShare.LockReadWrite;
			}
			VB6File vb6File;
			if (Mode == OpenMode.Input)
			{
				if (Access != OpenAccess.Read && Access != OpenAccess.Default)
				{
					throw new ArgumentException(Utils.GetResourceString("FileSystem_IllegalInputAccess"));
				}
				vb6File = new VB6InputFile(FileName, Share);
			}
			else if (Mode == OpenMode.Output)
			{
				if (Access != OpenAccess.Write && Access != OpenAccess.Default)
				{
					throw new ArgumentException(Utils.GetResourceString("FileSystem_IllegalOutputAccess"));
				}
				vb6File = new VB6OutputFile(FileName, Share, false);
			}
			else if (Mode == OpenMode.Random)
			{
				if (Access == OpenAccess.Default)
				{
					Access = OpenAccess.ReadWrite;
				}
				vb6File = new VB6RandomFile(FileName, Access, Share, RecordLength);
			}
			else if (Mode == OpenMode.Append)
			{
				if (Access != OpenAccess.Write && Access != OpenAccess.ReadWrite && Access != OpenAccess.Default)
				{
					throw new ArgumentException(Utils.GetResourceString("FileSystem_IllegalAppendAccess"));
				}
				vb6File = new VB6OutputFile(FileName, Share, true);
			}
			else
			{
				if (Mode != OpenMode.Binary)
				{
					throw ExceptionUtils.VbMakeException(51);
				}
				if (Access == OpenAccess.Default)
				{
					Access = OpenAccess.ReadWrite;
				}
				vb6File = new VB6BinaryFile(FileName, Access, Share);
			}
			FileSystem.AddFileToList(assemblyData, FileNumber, vb6File);
		}

		private static void AddFileToList(AssemblyData oAssemblyData, int FileNumber, VB6File oFile)
		{
			if (oFile == null)
			{
				throw ExceptionUtils.VbMakeException(51);
			}
			oFile.OpenFile();
			oAssemblyData.SetChannelObj(FileNumber, oFile);
		}

		internal static VB6File GetChannelObj(Assembly assem, int FileNumber)
		{
			VB6File channelOrNull = FileSystem.GetChannelOrNull(ProjectData.GetProjectData().GetAssemblyData(assem), FileNumber);
			if (channelOrNull == null)
			{
				throw ExceptionUtils.VbMakeException(52);
			}
			return channelOrNull;
		}

		private static VB6File GetChannelOrNull(AssemblyData oAssemblyData, int FileNumber)
		{
			return oAssemblyData.GetChannelObj(FileNumber);
		}

		private static void CheckInputCapable(VB6File oFile)
		{
			if (!oFile.CanInput())
			{
				throw ExceptionUtils.VbMakeException(54);
			}
		}

		private const int ERROR_ACCESS_DENIED = 5;

		private const int ERROR_FILE_NOT_FOUND = 2;

		private const int ERROR_BAD_NETPATH = 53;

		private const int ERROR_INVALID_PARAMETER = 87;

		private const int ERROR_WRITE_PROTECT = 19;

		private const int ERROR_FILE_EXISTS = 80;

		private const int ERROR_ALREADY_EXISTS = 183;

		private const int ERROR_INVALID_ACCESS = 12;

		private const int ERROR_NOT_SAME_DEVICE = 17;

		internal const int FIRST_LOCAL_CHANNEL = 1;

		internal const int LAST_LOCAL_CHANNEL = 255;

		private const int A_NORMAL = 0;

		private const int A_RDONLY = 1;

		private const int A_HIDDEN = 2;

		private const int A_SYSTEM = 4;

		private const int A_VOLID = 8;

		private const int A_SUBDIR = 16;

		private const int A_ARCH = 32;

		private const int A_ALLBITS = 63;

		internal const string sTimeFormat = "T";

		internal const string sDateFormat = "d";

		internal const string sDateTimeFormat = "F";

		internal static readonly DateTimeFormatInfo m_WriteDateFormatInfo = FileSystem.InitializeWriteDateFormatInfo();

		internal enum vbFileType
		{
			vbPrintFile,
			vbWriteFile
		}
	}
}

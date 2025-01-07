using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.FileIO
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class FileSystem
	{
		public static ReadOnlyCollection<DriveInfo> Drives
		{
			get
			{
				Collection<DriveInfo> collection = new Collection<DriveInfo>();
				foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
				{
					collection.Add(driveInfo);
				}
				return new ReadOnlyCollection<DriveInfo>(collection);
			}
		}

		public static string CurrentDirectory
		{
			get
			{
				return FileSystem.NormalizePath(Directory.GetCurrentDirectory());
			}
			set
			{
				Directory.SetCurrentDirectory(value);
			}
		}

		public static string CombinePath(string baseDirectory, string relativePath)
		{
			if (Operators.CompareString(baseDirectory, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("baseDirectory", "General_ArgumentEmptyOrNothing_Name", new string[] { "baseDirectory" });
			}
			if (Operators.CompareString(relativePath, "", false) == 0)
			{
				return baseDirectory;
			}
			baseDirectory = Path.GetFullPath(baseDirectory);
			return FileSystem.NormalizePath(Path.Combine(baseDirectory, relativePath));
		}

		public static bool DirectoryExists(string directory)
		{
			return Directory.Exists(directory);
		}

		public static bool FileExists(string file)
		{
			return (string.IsNullOrEmpty(file) || !(file.EndsWith(Conversions.ToString(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase) | file.EndsWith(Conversions.ToString(Path.AltDirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))) && File.Exists(file);
		}

		public static ReadOnlyCollection<string> FindInFiles(string directory, string containsText, bool ignoreCase, SearchOption searchType)
		{
			return FileSystem.FindInFiles(directory, containsText, ignoreCase, searchType, null);
		}

		public static ReadOnlyCollection<string> FindInFiles(string directory, string containsText, bool ignoreCase, SearchOption searchType, params string[] fileWildcards)
		{
			ReadOnlyCollection<string> readOnlyCollection = FileSystem.FindFilesOrDirectories(FileSystem.FileOrDirectory.File, directory, searchType, fileWildcards);
			if (Operators.CompareString(containsText, "", false) != 0)
			{
				Collection<string> collection = new Collection<string>();
				try
				{
					foreach (string text in readOnlyCollection)
					{
						if (FileSystem.FileContainsText(text, containsText, ignoreCase))
						{
							collection.Add(text);
						}
					}
				}
				finally
				{
					IEnumerator<string> enumerator;
					if (enumerator != null)
					{
						enumerator.Dispose();
					}
				}
				return new ReadOnlyCollection<string>(collection);
			}
			return readOnlyCollection;
		}

		public static ReadOnlyCollection<string> GetDirectories(string directory)
		{
			return FileSystem.FindFilesOrDirectories(FileSystem.FileOrDirectory.Directory, directory, SearchOption.SearchTopLevelOnly, null);
		}

		public static ReadOnlyCollection<string> GetDirectories(string directory, SearchOption searchType, params string[] wildcards)
		{
			return FileSystem.FindFilesOrDirectories(FileSystem.FileOrDirectory.Directory, directory, searchType, wildcards);
		}

		public static DirectoryInfo GetDirectoryInfo(string directory)
		{
			return new DirectoryInfo(directory);
		}

		public static DriveInfo GetDriveInfo(string drive)
		{
			return new DriveInfo(drive);
		}

		public static FileInfo GetFileInfo(string file)
		{
			file = FileSystem.NormalizeFilePath(file, "file");
			return new FileInfo(file);
		}

		public static ReadOnlyCollection<string> GetFiles(string directory)
		{
			return FileSystem.FindFilesOrDirectories(FileSystem.FileOrDirectory.File, directory, SearchOption.SearchTopLevelOnly, null);
		}

		public static ReadOnlyCollection<string> GetFiles(string directory, SearchOption searchType, params string[] wildcards)
		{
			return FileSystem.FindFilesOrDirectories(FileSystem.FileOrDirectory.File, directory, searchType, wildcards);
		}

		public static string GetName(string path)
		{
			return Path.GetFileName(path);
		}

		public static string GetParentPath(string path)
		{
			Path.GetFullPath(path);
			if (FileSystem.IsRoot(path))
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("path", "IO_GetParentPathIsRoot_Path", new string[] { path });
			}
			return Path.GetDirectoryName(path.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}));
		}

		public static string GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		public static TextFieldParser OpenTextFieldParser(string file)
		{
			return new TextFieldParser(file);
		}

		public static TextFieldParser OpenTextFieldParser(string file, params string[] delimiters)
		{
			TextFieldParser textFieldParser = new TextFieldParser(file);
			textFieldParser.SetDelimiters(delimiters);
			textFieldParser.TextFieldType = FieldType.Delimited;
			return textFieldParser;
		}

		public static TextFieldParser OpenTextFieldParser(string file, params int[] fieldWidths)
		{
			TextFieldParser textFieldParser = new TextFieldParser(file);
			textFieldParser.SetFieldWidths(fieldWidths);
			textFieldParser.TextFieldType = FieldType.FixedWidth;
			return textFieldParser;
		}

		public static StreamReader OpenTextFileReader(string file)
		{
			return FileSystem.OpenTextFileReader(file, Encoding.UTF8);
		}

		public static StreamReader OpenTextFileReader(string file, Encoding encoding)
		{
			file = FileSystem.NormalizeFilePath(file, "file");
			return new StreamReader(file, encoding, true);
		}

		public static StreamWriter OpenTextFileWriter(string file, bool append)
		{
			return FileSystem.OpenTextFileWriter(file, append, Encoding.UTF8);
		}

		public static StreamWriter OpenTextFileWriter(string file, bool append, Encoding encoding)
		{
			file = FileSystem.NormalizeFilePath(file, "file");
			return new StreamWriter(file, append, encoding);
		}

		public static byte[] ReadAllBytes(string file)
		{
			return File.ReadAllBytes(file);
		}

		public static string ReadAllText(string file)
		{
			return File.ReadAllText(file);
		}

		public static string ReadAllText(string file, Encoding encoding)
		{
			return File.ReadAllText(file, encoding);
		}

		public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, false, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, overwrite, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, false, FileSystem.ToUIOptionInternal(showUI), UICancelOption.ThrowException);
		}

		public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI, UICancelOption onUserCancel)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, false, FileSystem.ToUIOptionInternal(showUI), onUserCancel);
		}

		public static void CopyFile(string sourceFileName, string destinationFileName)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Copy, sourceFileName, destinationFileName, false, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Copy, sourceFileName, destinationFileName, overwrite, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void CopyFile(string sourceFileName, string destinationFileName, UIOption showUI)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Copy, sourceFileName, destinationFileName, false, FileSystem.ToUIOptionInternal(showUI), UICancelOption.ThrowException);
		}

		public static void CopyFile(string sourceFileName, string destinationFileName, UIOption showUI, UICancelOption onUserCancel)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Copy, sourceFileName, destinationFileName, false, FileSystem.ToUIOptionInternal(showUI), onUserCancel);
		}

		public static void CreateDirectory(string directory)
		{
			directory = Path.GetFullPath(directory);
			if (File.Exists(directory))
			{
				throw ExceptionUtils.GetIOException("IO_FileExists_Path", new string[] { directory });
			}
			Directory.CreateDirectory(directory);
		}

		public static void DeleteDirectory(string directory, DeleteDirectoryOption onDirectoryNotEmpty)
		{
			FileSystem.DeleteDirectoryInternal(directory, onDirectoryNotEmpty, FileSystem.UIOptionInternal.NoUI, RecycleOption.DeletePermanently, UICancelOption.ThrowException);
		}

		public static void DeleteDirectory(string directory, UIOption showUI, RecycleOption recycle)
		{
			FileSystem.DeleteDirectoryInternal(directory, DeleteDirectoryOption.DeleteAllContents, FileSystem.ToUIOptionInternal(showUI), recycle, UICancelOption.ThrowException);
		}

		public static void DeleteDirectory(string directory, UIOption showUI, RecycleOption recycle, UICancelOption onUserCancel)
		{
			FileSystem.DeleteDirectoryInternal(directory, DeleteDirectoryOption.DeleteAllContents, FileSystem.ToUIOptionInternal(showUI), recycle, onUserCancel);
		}

		public static void DeleteFile(string file)
		{
			FileSystem.DeleteFileInternal(file, FileSystem.UIOptionInternal.NoUI, RecycleOption.DeletePermanently, UICancelOption.ThrowException);
		}

		public static void DeleteFile(string file, UIOption showUI, RecycleOption recycle)
		{
			FileSystem.DeleteFileInternal(file, FileSystem.ToUIOptionInternal(showUI), recycle, UICancelOption.ThrowException);
		}

		public static void DeleteFile(string file, UIOption showUI, RecycleOption recycle, UICancelOption onUserCancel)
		{
			FileSystem.DeleteFileInternal(file, FileSystem.ToUIOptionInternal(showUI), recycle, onUserCancel);
		}

		public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, false, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, overwrite, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, false, FileSystem.ToUIOptionInternal(showUI), UICancelOption.ThrowException);
		}

		public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI, UICancelOption onUserCancel)
		{
			FileSystem.CopyOrMoveDirectory(FileSystem.CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, false, FileSystem.ToUIOptionInternal(showUI), onUserCancel);
		}

		public static void MoveFile(string sourceFileName, string destinationFileName)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Move, sourceFileName, destinationFileName, false, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void MoveFile(string sourceFileName, string destinationFileName, bool overwrite)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Move, sourceFileName, destinationFileName, overwrite, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
		}

		public static void MoveFile(string sourceFileName, string destinationFileName, UIOption showUI)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Move, sourceFileName, destinationFileName, false, FileSystem.ToUIOptionInternal(showUI), UICancelOption.ThrowException);
		}

		public static void MoveFile(string sourceFileName, string destinationFileName, UIOption showUI, UICancelOption onUserCancel)
		{
			FileSystem.CopyOrMoveFile(FileSystem.CopyOrMove.Move, sourceFileName, destinationFileName, false, FileSystem.ToUIOptionInternal(showUI), onUserCancel);
		}

		public static void RenameDirectory(string directory, string newName)
		{
			directory = Path.GetFullPath(directory);
			FileSystem.ThrowIfDevicePath(directory);
			if (FileSystem.IsRoot(directory))
			{
				throw ExceptionUtils.GetIOException("IO_DirectoryIsRoot_Path", new string[] { directory });
			}
			if (!Directory.Exists(directory))
			{
				throw ExceptionUtils.GetDirectoryNotFoundException("IO_DirectoryNotFound_Path", new string[] { directory });
			}
			if (Operators.CompareString(newName, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("newName", "General_ArgumentEmptyOrNothing_Name", new string[] { "newName" });
			}
			string fullPathFromNewName = FileSystem.GetFullPathFromNewName(FileSystem.GetParentPath(directory), newName, "newName");
			FileSystem.EnsurePathNotExist(fullPathFromNewName);
			Directory.Move(directory, fullPathFromNewName);
		}

		public static void RenameFile(string file, string newName)
		{
			file = FileSystem.NormalizeFilePath(file, "file");
			FileSystem.ThrowIfDevicePath(file);
			if (!File.Exists(file))
			{
				throw ExceptionUtils.GetFileNotFoundException(file, "IO_FileNotFound_Path", new string[] { file });
			}
			if (Operators.CompareString(newName, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("newName", "General_ArgumentEmptyOrNothing_Name", new string[] { "newName" });
			}
			string fullPathFromNewName = FileSystem.GetFullPathFromNewName(FileSystem.GetParentPath(file), newName, "newName");
			FileSystem.EnsurePathNotExist(fullPathFromNewName);
			File.Move(file, fullPathFromNewName);
		}

		public static void WriteAllBytes(string file, byte[] data, bool append)
		{
			FileSystem.CheckFilePathTrailingSeparator(file, "file");
			FileStream fileStream = null;
			try
			{
				FileMode fileMode;
				if (append)
				{
					fileMode = FileMode.Append;
				}
				else
				{
					fileMode = FileMode.Create;
				}
				fileStream = new FileStream(file, fileMode, FileAccess.Write, FileShare.Read);
				fileStream.Write(data, 0, data.Length);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		public static void WriteAllText(string file, string text, bool append)
		{
			FileSystem.WriteAllText(file, text, append, Encoding.UTF8);
		}

		public static void WriteAllText(string file, string text, bool append, Encoding encoding)
		{
			FileSystem.CheckFilePathTrailingSeparator(file, "file");
			StreamWriter streamWriter = null;
			try
			{
				if (append && File.Exists(file))
				{
					StreamReader streamReader = null;
					try
					{
						streamReader = new StreamReader(file, encoding, true);
						char[] array = new char[10];
						streamReader.Read(array, 0, 10);
						encoding = streamReader.CurrentEncoding;
					}
					catch (IOException ex)
					{
					}
					finally
					{
						if (streamReader != null)
						{
							streamReader.Close();
						}
					}
				}
				streamWriter = new StreamWriter(file, append, encoding);
				streamWriter.Write(text);
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
		}

		internal static string NormalizeFilePath(string Path, string ParamName)
		{
			FileSystem.CheckFilePathTrailingSeparator(Path, ParamName);
			return FileSystem.NormalizePath(Path);
		}

		internal static string NormalizePath(string Path)
		{
			return FileSystem.GetLongPath(FileSystem.RemoveEndingSeparator(Path.GetFullPath(Path)));
		}

		internal static void CheckFilePathTrailingSeparator(string path, string paramName)
		{
			if (Operators.CompareString(path, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException(paramName);
			}
			if (path.EndsWith(Conversions.ToString(Path.DirectorySeparatorChar), StringComparison.Ordinal) | path.EndsWith(Conversions.ToString(Path.AltDirectorySeparatorChar), StringComparison.Ordinal))
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName(paramName, "IO_FilePathException", new string[0]);
			}
		}

		private static void AddToStringCollection(Collection<string> StrCollection, string[] StrArray)
		{
			if (StrArray != null)
			{
				foreach (string text in StrArray)
				{
					if (!StrCollection.Contains(text))
					{
						StrCollection.Add(text);
					}
				}
			}
		}

		private static void CopyOrMoveDirectory(FileSystem.CopyOrMove operation, string sourceDirectoryName, string destinationDirectoryName, bool overwrite, FileSystem.UIOptionInternal showUI, UICancelOption onUserCancel)
		{
			FileSystem.VerifyUICancelOption("onUserCancel", onUserCancel);
			string text = FileSystem.NormalizePath(sourceDirectoryName);
			string text2 = FileSystem.NormalizePath(destinationDirectoryName);
			FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.Read;
			if (operation == FileSystem.CopyOrMove.Move)
			{
				fileIOPermissionAccess |= FileIOPermissionAccess.Write;
			}
			FileSystem.DemandDirectoryPermission(text, fileIOPermissionAccess);
			FileSystem.DemandDirectoryPermission(text2, FileIOPermissionAccess.Read | FileIOPermissionAccess.Write);
			FileSystem.ThrowIfDevicePath(text);
			FileSystem.ThrowIfDevicePath(text2);
			if (!Directory.Exists(text))
			{
				throw ExceptionUtils.GetDirectoryNotFoundException("IO_DirectoryNotFound_Path", new string[] { sourceDirectoryName });
			}
			if (FileSystem.IsRoot(text))
			{
				throw ExceptionUtils.GetIOException("IO_DirectoryIsRoot_Path", new string[] { sourceDirectoryName });
			}
			if (File.Exists(text2))
			{
				throw ExceptionUtils.GetIOException("IO_FileExists_Path", new string[] { destinationDirectoryName });
			}
			if (text2.Equals(text, StringComparison.OrdinalIgnoreCase))
			{
				throw ExceptionUtils.GetIOException("IO_SourceEqualsTargetDirectory", new string[0]);
			}
			if (text2.Length > text.Length && text2.Substring(0, text.Length).Equals(text, StringComparison.OrdinalIgnoreCase) && text2[text.Length] == Path.DirectorySeparatorChar)
			{
				throw ExceptionUtils.GetInvalidOperationException("IO_CyclicOperation", new string[0]);
			}
			if (showUI != FileSystem.UIOptionInternal.NoUI && Environment.UserInteractive)
			{
				FileSystem.ShellCopyOrMove(operation, FileSystem.FileOrDirectory.Directory, text, text2, showUI, onUserCancel);
			}
			else
			{
				FileSystem.FxCopyOrMoveDirectory(operation, text, text2, overwrite);
			}
		}

		private static void FxCopyOrMoveDirectory(FileSystem.CopyOrMove operation, string sourceDirectoryPath, string targetDirectoryPath, bool overwrite)
		{
			if ((operation == FileSystem.CopyOrMove.Move) & !Directory.Exists(targetDirectoryPath) & FileSystem.IsOnSameDrive(sourceDirectoryPath, targetDirectoryPath))
			{
				Directory.CreateDirectory(FileSystem.GetParentPath(targetDirectoryPath));
				try
				{
					Directory.Move(sourceDirectoryPath, targetDirectoryPath);
					return;
				}
				catch (IOException ex)
				{
				}
				catch (UnauthorizedAccessException ex2)
				{
				}
			}
			Directory.CreateDirectory(targetDirectoryPath);
			FileSystem.DirectoryNode directoryNode = new FileSystem.DirectoryNode(sourceDirectoryPath, targetDirectoryPath);
			ListDictionary listDictionary = new ListDictionary();
			FileSystem.CopyOrMoveDirectoryNode(operation, directoryNode, overwrite, listDictionary);
			if (listDictionary.Count > 0)
			{
				IOException ex3 = new IOException(Utils.GetResourceString("IO_CopyMoveRecursive"));
				foreach (object obj in listDictionary)
				{
					DictionaryEntry dictionaryEntry2;
					DictionaryEntry dictionaryEntry = ((obj != null) ? ((DictionaryEntry)obj) : dictionaryEntry2);
					ex3.Data.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
				throw ex3;
			}
		}

		private static void CopyOrMoveDirectoryNode(FileSystem.CopyOrMove Operation, FileSystem.DirectoryNode SourceDirectoryNode, bool Overwrite, ListDictionary Exceptions)
		{
			try
			{
				if (!Directory.Exists(SourceDirectoryNode.TargetPath))
				{
					Directory.CreateDirectory(SourceDirectoryNode.TargetPath);
				}
			}
			catch (Exception ex)
			{
				if (!(ex is IOException))
				{
					if (!(ex is UnauthorizedAccessException))
					{
						if (!(ex is DirectoryNotFoundException))
						{
							if (!(ex is NotSupportedException))
							{
								if (!(ex is SecurityException))
								{
									throw;
								}
							}
						}
					}
				}
				Exceptions.Add(SourceDirectoryNode.Path, ex.Message);
				return;
			}
			if (!Directory.Exists(SourceDirectoryNode.TargetPath))
			{
				Exceptions.Add(SourceDirectoryNode.TargetPath, ExceptionUtils.GetDirectoryNotFoundException("IO_DirectoryNotFound_Path", new string[] { SourceDirectoryNode.TargetPath }));
				return;
			}
			foreach (string text in Directory.GetFiles(SourceDirectoryNode.Path))
			{
				try
				{
					FileSystem.CopyOrMoveFile(Operation, text, Path.Combine(SourceDirectoryNode.TargetPath, Path.GetFileName(text)), Overwrite, FileSystem.UIOptionInternal.NoUI, UICancelOption.ThrowException);
				}
				catch (Exception ex2)
				{
					if (!(ex2 is IOException))
					{
						if (!(ex2 is UnauthorizedAccessException))
						{
							if (!(ex2 is SecurityException))
							{
								if (!(ex2 is NotSupportedException))
								{
									throw;
								}
							}
						}
					}
					Exceptions.Add(text, ex2.Message);
				}
			}
			try
			{
				foreach (FileSystem.DirectoryNode directoryNode in SourceDirectoryNode.SubDirs)
				{
					FileSystem.CopyOrMoveDirectoryNode(Operation, directoryNode, Overwrite, Exceptions);
				}
			}
			finally
			{
				IEnumerator<FileSystem.DirectoryNode> enumerator;
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			if (Operation == FileSystem.CopyOrMove.Move)
			{
				try
				{
					Directory.Delete(SourceDirectoryNode.Path, false);
				}
				catch (Exception ex3)
				{
					if (!(ex3 is IOException))
					{
						if (!(ex3 is UnauthorizedAccessException))
						{
							if (!(ex3 is SecurityException))
							{
								if (!(ex3 is DirectoryNotFoundException))
								{
									throw;
								}
							}
						}
					}
					Exceptions.Add(SourceDirectoryNode.Path, ex3.Message);
				}
			}
		}

		private static void CopyOrMoveFile(FileSystem.CopyOrMove operation, string sourceFileName, string destinationFileName, bool overwrite, FileSystem.UIOptionInternal showUI, UICancelOption onUserCancel)
		{
			FileSystem.VerifyUICancelOption("onUserCancel", onUserCancel);
			string text = FileSystem.NormalizeFilePath(sourceFileName, "sourceFileName");
			string text2 = FileSystem.NormalizeFilePath(destinationFileName, "destinationFileName");
			FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.Read;
			if (operation == FileSystem.CopyOrMove.Move)
			{
				fileIOPermissionAccess |= FileIOPermissionAccess.Write;
			}
			new FileIOPermission(fileIOPermissionAccess, text).Demand();
			new FileIOPermission(FileIOPermissionAccess.Write, text2).Demand();
			FileSystem.ThrowIfDevicePath(text);
			FileSystem.ThrowIfDevicePath(text2);
			if (!File.Exists(text))
			{
				throw ExceptionUtils.GetFileNotFoundException(sourceFileName, "IO_FileNotFound_Path", new string[] { sourceFileName });
			}
			if (Directory.Exists(text2))
			{
				throw ExceptionUtils.GetIOException("IO_DirectoryExists_Path", new string[] { destinationFileName });
			}
			Directory.CreateDirectory(FileSystem.GetParentPath(text2));
			if (showUI != FileSystem.UIOptionInternal.NoUI && Environment.UserInteractive)
			{
				FileSystem.ShellCopyOrMove(operation, FileSystem.FileOrDirectory.File, text, text2, showUI, onUserCancel);
				return;
			}
			if (operation == FileSystem.CopyOrMove.Copy || text.Equals(text2, StringComparison.OrdinalIgnoreCase))
			{
				File.Copy(text, text2, overwrite);
			}
			else if (overwrite)
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						if (!NativeMethods.MoveFileEx(text, text2, 11))
						{
							FileSystem.ThrowWinIOError(Marshal.GetLastWin32Error());
						}
						return;
					}
					catch (Exception)
					{
						throw;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				File.Delete(text2);
				File.Move(text, text2);
			}
			else
			{
				File.Move(text, text2);
			}
		}

		private static void DeleteDirectoryInternal(string directory, DeleteDirectoryOption onDirectoryNotEmpty, FileSystem.UIOptionInternal showUI, RecycleOption recycle, UICancelOption onUserCancel)
		{
			FileSystem.VerifyDeleteDirectoryOption("onDirectoryNotEmpty", onDirectoryNotEmpty);
			FileSystem.VerifyRecycleOption("recycle", recycle);
			FileSystem.VerifyUICancelOption("onUserCancel", onUserCancel);
			string fullPath = Path.GetFullPath(directory);
			FileSystem.DemandDirectoryPermission(fullPath, FileIOPermissionAccess.Write);
			FileSystem.ThrowIfDevicePath(fullPath);
			if (!Directory.Exists(fullPath))
			{
				throw ExceptionUtils.GetDirectoryNotFoundException("IO_DirectoryNotFound_Path", new string[] { directory });
			}
			if (FileSystem.IsRoot(fullPath))
			{
				throw ExceptionUtils.GetIOException("IO_DirectoryIsRoot_Path", new string[] { directory });
			}
			if (showUI != FileSystem.UIOptionInternal.NoUI && Environment.UserInteractive)
			{
				FileSystem.ShellDelete(fullPath, showUI, recycle, onUserCancel, FileSystem.FileOrDirectory.Directory);
				return;
			}
			Directory.Delete(fullPath, onDirectoryNotEmpty == DeleteDirectoryOption.DeleteAllContents);
		}

		private static void DeleteFileInternal(string file, FileSystem.UIOptionInternal showUI, RecycleOption recycle, UICancelOption onUserCancel)
		{
			FileSystem.VerifyRecycleOption("recycle", recycle);
			FileSystem.VerifyUICancelOption("onUserCancel", onUserCancel);
			string text = FileSystem.NormalizeFilePath(file, "file");
			new FileIOPermission(FileIOPermissionAccess.Write, text).Demand();
			FileSystem.ThrowIfDevicePath(text);
			if (!File.Exists(text))
			{
				throw ExceptionUtils.GetFileNotFoundException(file, "IO_FileNotFound_Path", new string[] { file });
			}
			if (showUI != FileSystem.UIOptionInternal.NoUI && Environment.UserInteractive)
			{
				FileSystem.ShellDelete(text, showUI, recycle, onUserCancel, FileSystem.FileOrDirectory.File);
				return;
			}
			File.Delete(text);
		}

		private static void DemandDirectoryPermission(string fullDirectoryPath, FileIOPermissionAccess access)
		{
			if (!(fullDirectoryPath.EndsWith(Conversions.ToString(Path.DirectorySeparatorChar), StringComparison.Ordinal) | fullDirectoryPath.EndsWith(Conversions.ToString(Path.AltDirectorySeparatorChar), StringComparison.Ordinal)))
			{
				fullDirectoryPath += Conversions.ToString(Path.DirectorySeparatorChar);
			}
			FileIOPermission fileIOPermission = new FileIOPermission(access, fullDirectoryPath);
			fileIOPermission.Demand();
		}

		private static void EnsurePathNotExist(string Path)
		{
			if (File.Exists(Path))
			{
				throw ExceptionUtils.GetIOException("IO_FileExists_Path", new string[] { Path });
			}
			if (Directory.Exists(Path))
			{
				throw ExceptionUtils.GetIOException("IO_DirectoryExists_Path", new string[] { Path });
			}
		}

		private static bool FileContainsText(string FilePath, string Text, bool IgnoreCase)
		{
			int num = 1024;
			FileStream fileStream = null;
			checked
			{
				bool flag;
				try
				{
					fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					Encoding encoding = Encoding.Default;
					byte[] array = new byte[num - 1 + 1];
					int num2 = fileStream.Read(array, 0, array.Length);
					if (num2 > 0)
					{
						MemoryStream memoryStream = new MemoryStream(array, 0, num2);
						StreamReader streamReader = new StreamReader(memoryStream, encoding, true);
						streamReader.ReadLine();
						encoding = streamReader.CurrentEncoding;
					}
					int maxByteCount = encoding.GetMaxByteCount(Text.Length);
					int num3 = Math.Max(maxByteCount, num);
					FileSystem.TextSearchHelper textSearchHelper = new FileSystem.TextSearchHelper(encoding, Text, IgnoreCase);
					if (num3 > num)
					{
						array = (byte[])Utils.CopyArray((Array)array, new byte[num3 - 1 + 1]);
						int num4 = fileStream.Read(array, num2, array.Length - num2);
						num2 += num4;
					}
					while (num2 <= 0 || !textSearchHelper.IsTextFound(array, num2))
					{
						num2 = fileStream.Read(array, 0, array.Length);
						if (num2 <= 0)
						{
							return false;
						}
					}
					flag = true;
				}
				catch (Exception ex)
				{
					if (!((ex is IOException) | (ex is NotSupportedException) | (ex is SecurityException) | (ex is UnauthorizedAccessException)))
					{
						throw;
					}
					flag = false;
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				return flag;
			}
		}

		private static ReadOnlyCollection<string> FindFilesOrDirectories(FileSystem.FileOrDirectory FileOrDirectory, string directory, SearchOption searchType, string[] wildcards)
		{
			Collection<string> collection = new Collection<string>();
			FileSystem.FindFilesOrDirectories(FileOrDirectory, directory, searchType, wildcards, collection);
			return new ReadOnlyCollection<string>(collection);
		}

		private static void FindFilesOrDirectories(FileSystem.FileOrDirectory FileOrDirectory, string directory, SearchOption searchType, string[] wildcards, Collection<string> Results)
		{
			FileSystem.VerifySearchOption("searchType", searchType);
			directory = FileSystem.NormalizePath(directory);
			if (wildcards != null)
			{
				foreach (string text in wildcards)
				{
					if (Operators.CompareString(text.TrimEnd(new char[0]), "", false) == 0)
					{
						throw ExceptionUtils.GetArgumentNullException("wildcards", "IO_GetFiles_NullPattern", new string[0]);
					}
				}
			}
			if (wildcards == null || wildcards.Length == 0)
			{
				FileSystem.AddToStringCollection(Results, FileSystem.FindPaths(FileOrDirectory, directory, null));
			}
			else
			{
				foreach (string text2 in wildcards)
				{
					FileSystem.AddToStringCollection(Results, FileSystem.FindPaths(FileOrDirectory, directory, text2));
				}
			}
			if (searchType == SearchOption.SearchAllSubDirectories)
			{
				foreach (string text3 in Directory.GetDirectories(directory))
				{
					FileSystem.FindFilesOrDirectories(FileOrDirectory, text3, searchType, wildcards, Results);
				}
			}
		}

		private static string[] FindPaths(FileSystem.FileOrDirectory FileOrDirectory, string directory, string wildCard)
		{
			if (FileOrDirectory == FileSystem.FileOrDirectory.Directory)
			{
				if (Operators.CompareString(wildCard, "", false) == 0)
				{
					return Directory.GetDirectories(directory);
				}
				return Directory.GetDirectories(directory, wildCard);
			}
			else
			{
				if (Operators.CompareString(wildCard, "", false) == 0)
				{
					return Directory.GetFiles(directory);
				}
				return Directory.GetFiles(directory, wildCard);
			}
		}

		private static string GetFullPathFromNewName(string Path, string NewName, string ArgumentName)
		{
			if (NewName.IndexOfAny(FileSystem.m_SeparatorChars) >= 0)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName(ArgumentName, "IO_ArgumentIsPath_Name_Path", new string[] { ArgumentName, NewName });
			}
			string text = FileSystem.RemoveEndingSeparator(Path.GetFullPath(Path.Combine(Path, NewName)));
			if (!FileSystem.GetParentPath(text).Equals(Path, StringComparison.OrdinalIgnoreCase))
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName(ArgumentName, "IO_ArgumentIsPath_Name_Path", new string[] { ArgumentName, NewName });
			}
			return text;
		}

		private static string GetLongPath(string FullPath)
		{
			string text;
			try
			{
				if (FileSystem.IsRoot(FullPath))
				{
					text = FullPath;
				}
				else
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(FileSystem.GetParentPath(FullPath));
					if (File.Exists(FullPath))
					{
						text = directoryInfo.GetFiles(Path.GetFileName(FullPath))[0].FullName;
					}
					else if (Directory.Exists(FullPath))
					{
						text = directoryInfo.GetDirectories(Path.GetFileName(FullPath))[0].FullName;
					}
					else
					{
						text = FullPath;
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ArgumentException))
				{
					if (!(ex is ArgumentNullException))
					{
						if (!(ex is PathTooLongException))
						{
							if (!(ex is NotSupportedException))
							{
								if (!(ex is DirectoryNotFoundException))
								{
									if (!(ex is SecurityException))
									{
										if (!(ex is UnauthorizedAccessException))
										{
											throw;
										}
									}
								}
							}
						}
					}
				}
				text = FullPath;
			}
			return text;
		}

		private static bool IsOnSameDrive(string Path1, string Path2)
		{
			Path1 = Path1.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			Path2 = Path2.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			return string.Compare(Path.GetPathRoot(Path1), Path.GetPathRoot(Path2), StringComparison.OrdinalIgnoreCase) == 0;
		}

		private static bool IsRoot(string Path)
		{
			if (!Path.IsPathRooted(Path))
			{
				return false;
			}
			Path = Path.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			return string.Compare(Path, Path.GetPathRoot(Path), StringComparison.OrdinalIgnoreCase) == 0;
		}

		private static string RemoveEndingSeparator(string Path)
		{
			if (Path.IsPathRooted(Path) && Path.Equals(Path.GetPathRoot(Path), StringComparison.OrdinalIgnoreCase))
			{
				return Path;
			}
			return Path.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
		}

		private static void ShellCopyOrMove(FileSystem.CopyOrMove Operation, FileSystem.FileOrDirectory TargetType, string FullSourcePath, string FullTargetPath, FileSystem.UIOptionInternal ShowUI, UICancelOption OnUserCancel)
		{
			NativeMethods.SHFileOperationType shfileOperationType;
			if (Operation == FileSystem.CopyOrMove.Copy)
			{
				shfileOperationType = NativeMethods.SHFileOperationType.FO_COPY;
			}
			else
			{
				shfileOperationType = NativeMethods.SHFileOperationType.FO_MOVE;
			}
			NativeMethods.ShFileOperationFlags operationFlags = FileSystem.GetOperationFlags(ShowUI);
			string text = FullSourcePath;
			if (TargetType == FileSystem.FileOrDirectory.Directory)
			{
				if (Directory.Exists(FullTargetPath))
				{
					text = Path.Combine(FullSourcePath, "*");
				}
				else
				{
					Directory.CreateDirectory(FileSystem.GetParentPath(FullTargetPath));
				}
			}
			FileSystem.ShellFileOperation(shfileOperationType, operationFlags, text, FullTargetPath, OnUserCancel, TargetType);
			if (((Operation == FileSystem.CopyOrMove.Move) & (TargetType == FileSystem.FileOrDirectory.Directory)) && Directory.Exists(FullSourcePath) && Directory.GetDirectories(FullSourcePath).Length == 0 && Directory.GetFiles(FullSourcePath).Length == 0)
			{
				Directory.Delete(FullSourcePath, false);
			}
		}

		private static void ShellDelete(string FullPath, FileSystem.UIOptionInternal ShowUI, RecycleOption recycle, UICancelOption OnUserCancel, FileSystem.FileOrDirectory FileOrDirectory)
		{
			NativeMethods.ShFileOperationFlags shFileOperationFlags = FileSystem.GetOperationFlags(ShowUI);
			if (recycle == RecycleOption.SendToRecycleBin)
			{
				shFileOperationFlags |= NativeMethods.ShFileOperationFlags.FOF_ALLOWUNDO;
			}
			FileSystem.ShellFileOperation(NativeMethods.SHFileOperationType.FO_DELETE, shFileOperationFlags, FullPath, null, OnUserCancel, FileOrDirectory);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt, UI = true)]
		private static void ShellFileOperation(NativeMethods.SHFileOperationType OperationType, NativeMethods.ShFileOperationFlags OperationFlags, string FullSource, string FullTarget, UICancelOption OnUserCancel, FileSystem.FileOrDirectory FileOrDirectory)
		{
			UIPermission uipermission = new UIPermission(UIPermissionWindow.SafeSubWindows);
			uipermission.Demand();
			FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.NoAccess;
			if (OperationType == NativeMethods.SHFileOperationType.FO_COPY)
			{
				fileIOPermissionAccess = FileIOPermissionAccess.Read;
			}
			else if (OperationType == NativeMethods.SHFileOperationType.FO_MOVE)
			{
				fileIOPermissionAccess = FileIOPermissionAccess.Read | FileIOPermissionAccess.Write;
			}
			else if (OperationType == NativeMethods.SHFileOperationType.FO_DELETE)
			{
				fileIOPermissionAccess = FileIOPermissionAccess.Write;
			}
			string text = FullSource;
			if ((OperationType == NativeMethods.SHFileOperationType.FO_COPY || OperationType == NativeMethods.SHFileOperationType.FO_MOVE) && text.EndsWith("*", StringComparison.Ordinal))
			{
				text = FileSystem.RemoveEndingSeparator(FullSource.TrimEnd(new char[] { '*' }));
			}
			if (FileOrDirectory == FileSystem.FileOrDirectory.Directory)
			{
				FileSystem.DemandDirectoryPermission(text, fileIOPermissionAccess);
			}
			else
			{
				new FileIOPermission(fileIOPermissionAccess, text).Demand();
			}
			if (OperationType != NativeMethods.SHFileOperationType.FO_DELETE)
			{
				if (FileOrDirectory == FileSystem.FileOrDirectory.Directory)
				{
					FileSystem.DemandDirectoryPermission(FullTarget, FileIOPermissionAccess.Write);
				}
				else
				{
					new FileIOPermission(FileIOPermissionAccess.Write, FullTarget).Demand();
				}
			}
			NativeMethods.SHFILEOPSTRUCT shellOperationInfo = FileSystem.GetShellOperationInfo(OperationType, OperationFlags, FullSource, FullTarget);
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			int num;
			try
			{
				num = NativeMethods.SHFileOperation(ref shellOperationInfo);
				NativeMethods.SHChangeNotify(145439U, 3U, IntPtr.Zero, IntPtr.Zero);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (shellOperationInfo.fAnyOperationsAborted)
			{
				if (OnUserCancel == UICancelOption.ThrowException)
				{
					throw new OperationCanceledException();
				}
			}
			else if (num != 0)
			{
				FileSystem.ThrowWinIOError(num);
			}
		}

		private static NativeMethods.SHFILEOPSTRUCT GetShellOperationInfo(NativeMethods.SHFileOperationType OperationType, NativeMethods.ShFileOperationFlags OperationFlags, string SourcePath, string TargetPath = null)
		{
			return FileSystem.GetShellOperationInfo(OperationType, OperationFlags, new string[] { SourcePath }, TargetPath);
		}

		private static NativeMethods.SHFILEOPSTRUCT GetShellOperationInfo(NativeMethods.SHFileOperationType OperationType, NativeMethods.ShFileOperationFlags OperationFlags, string[] SourcePaths, string TargetPath = null)
		{
			NativeMethods.SHFILEOPSTRUCT shfileopstruct;
			shfileopstruct.wFunc = (uint)OperationType;
			shfileopstruct.fFlags = (ushort)OperationFlags;
			shfileopstruct.pFrom = FileSystem.GetShellPath(SourcePaths);
			if (TargetPath == null)
			{
				shfileopstruct.pTo = null;
			}
			else
			{
				shfileopstruct.pTo = FileSystem.GetShellPath(TargetPath);
			}
			shfileopstruct.hNameMappings = IntPtr.Zero;
			try
			{
				shfileopstruct.hwnd = Process.GetCurrentProcess().MainWindowHandle;
			}
			catch (Exception ex)
			{
				if (!(ex is SecurityException))
				{
					if (!(ex is InvalidOperationException))
					{
						if (!(ex is NotSupportedException))
						{
							throw;
						}
					}
				}
				shfileopstruct.hwnd = IntPtr.Zero;
			}
			shfileopstruct.lpszProgressTitle = string.Empty;
			return shfileopstruct;
		}

		private static NativeMethods.ShFileOperationFlags GetOperationFlags(FileSystem.UIOptionInternal ShowUI)
		{
			NativeMethods.ShFileOperationFlags shFileOperationFlags = NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMMKDIR | NativeMethods.ShFileOperationFlags.FOF_NO_CONNECTED_ELEMENTS;
			if (ShowUI == FileSystem.UIOptionInternal.OnlyErrorDialogs)
			{
				shFileOperationFlags |= NativeMethods.ShFileOperationFlags.FOF_SILENT | NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMATION;
			}
			return shFileOperationFlags;
		}

		private static string GetShellPath(string FullPath)
		{
			return FileSystem.GetShellPath(new string[] { FullPath });
		}

		private static string GetShellPath(string[] FullPaths)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in FullPaths)
			{
				stringBuilder.Append(text + "\0");
			}
			return stringBuilder.ToString();
		}

		private static void ThrowIfDevicePath(string path)
		{
			if (path.StartsWith("\\\\.\\", StringComparison.Ordinal))
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("path", "IO_DevicePath", new string[0]);
			}
		}

		private static void ThrowWinIOError(int errorCode)
		{
			if (errorCode == 2)
			{
				throw new FileNotFoundException();
			}
			if (errorCode == 3)
			{
				throw new DirectoryNotFoundException();
			}
			if (errorCode == 5)
			{
				throw new UnauthorizedAccessException();
			}
			if (errorCode == 206)
			{
				throw new PathTooLongException();
			}
			if (errorCode == 15)
			{
				throw new DriveNotFoundException();
			}
			if (errorCode == 995 || errorCode == 1223)
			{
				throw new OperationCanceledException();
			}
			throw new IOException(new Win32Exception(errorCode).Message, Marshal.GetHRForLastWin32Error());
		}

		private static FileSystem.UIOptionInternal ToUIOptionInternal(UIOption showUI)
		{
			switch (showUI)
			{
			case UIOption.OnlyErrorDialogs:
				return FileSystem.UIOptionInternal.OnlyErrorDialogs;
			case UIOption.AllDialogs:
				return FileSystem.UIOptionInternal.AllDialogs;
			default:
				throw new InvalidEnumArgumentException("showUI", (int)showUI, typeof(UIOption));
			}
		}

		private static void VerifyDeleteDirectoryOption(string argName, DeleteDirectoryOption argValue)
		{
			if (argValue == DeleteDirectoryOption.DeleteAllContents)
			{
				return;
			}
			if (argValue == DeleteDirectoryOption.ThrowIfDirectoryNonEmpty)
			{
				return;
			}
			throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(DeleteDirectoryOption));
		}

		private static void VerifyRecycleOption(string argName, RecycleOption argValue)
		{
			if (argValue == RecycleOption.DeletePermanently)
			{
				return;
			}
			if (argValue == RecycleOption.SendToRecycleBin)
			{
				return;
			}
			throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(RecycleOption));
		}

		private static void VerifySearchOption(string argName, SearchOption argValue)
		{
			if (argValue == SearchOption.SearchAllSubDirectories)
			{
				return;
			}
			if (argValue == SearchOption.SearchTopLevelOnly)
			{
				return;
			}
			throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(SearchOption));
		}

		private static void VerifyUICancelOption(string argName, UICancelOption argValue)
		{
			if (argValue == UICancelOption.DoNothing)
			{
				return;
			}
			if (argValue == UICancelOption.ThrowException)
			{
				return;
			}
			throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(UICancelOption));
		}

		private const NativeMethods.ShFileOperationFlags m_SHELL_OPERATION_FLAGS_BASE = NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMMKDIR | NativeMethods.ShFileOperationFlags.FOF_NO_CONNECTED_ELEMENTS;

		private const NativeMethods.ShFileOperationFlags m_SHELL_OPERATION_FLAGS_HIDE_UI = NativeMethods.ShFileOperationFlags.FOF_SILENT | NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMATION;

		private const int m_MOVEFILEEX_FLAGS = 11;

		private static readonly char[] m_SeparatorChars = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar,
			Path.VolumeSeparatorChar,
			Path.PathSeparator
		};

		private enum CopyOrMove
		{
			Copy,
			Move
		}

		private enum FileOrDirectory
		{
			File,
			Directory
		}

		private enum UIOptionInternal
		{
			OnlyErrorDialogs = 2,
			AllDialogs,
			NoUI
		}

		private class DirectoryNode
		{
			internal DirectoryNode(string DirectoryPath, string TargetDirectoryPath)
			{
				this.m_Path = DirectoryPath;
				this.m_TargetPath = TargetDirectoryPath;
				this.m_SubDirs = new Collection<FileSystem.DirectoryNode>();
				foreach (string text in Directory.GetDirectories(this.m_Path))
				{
					string text2 = global::System.IO.Path.Combine(this.m_TargetPath, global::System.IO.Path.GetFileName(text));
					this.m_SubDirs.Add(new FileSystem.DirectoryNode(text, text2));
				}
			}

			internal string Path
			{
				get
				{
					return this.m_Path;
				}
			}

			internal string TargetPath
			{
				get
				{
					return this.m_TargetPath;
				}
			}

			internal Collection<FileSystem.DirectoryNode> SubDirs
			{
				get
				{
					return this.m_SubDirs;
				}
			}

			private string m_Path;

			private string m_TargetPath;

			private Collection<FileSystem.DirectoryNode> m_SubDirs;
		}

		private class TextSearchHelper
		{
			internal TextSearchHelper(Encoding Encoding, string Text, bool IgnoreCase)
			{
				this.m_PreviousCharBuffer = new char[0];
				this.m_CheckPreamble = true;
				this.m_Decoder = Encoding.GetDecoder();
				this.m_Preamble = Encoding.GetPreamble();
				this.m_IgnoreCase = IgnoreCase;
				if (this.m_IgnoreCase)
				{
					this.m_SearchText = Text.ToUpper(CultureInfo.CurrentCulture);
				}
				else
				{
					this.m_SearchText = Text;
				}
			}

			internal bool IsTextFound(byte[] ByteBuffer, int Count)
			{
				int num = 0;
				checked
				{
					if (this.m_CheckPreamble)
					{
						if (FileSystem.TextSearchHelper.BytesMatch(ByteBuffer, this.m_Preamble))
						{
							num = this.m_Preamble.Length;
							Count -= this.m_Preamble.Length;
						}
						this.m_CheckPreamble = false;
						if (Count <= 0)
						{
							return false;
						}
					}
					int charCount = this.m_Decoder.GetCharCount(ByteBuffer, num, Count);
					char[] array = new char[this.m_PreviousCharBuffer.Length + charCount - 1 + 1];
					Array.Copy(this.m_PreviousCharBuffer, 0, array, 0, this.m_PreviousCharBuffer.Length);
					int chars = this.m_Decoder.GetChars(ByteBuffer, num, Count, array, this.m_PreviousCharBuffer.Length);
					if (array.Length > this.m_SearchText.Length)
					{
						if (this.m_PreviousCharBuffer.Length != this.m_SearchText.Length)
						{
							this.m_PreviousCharBuffer = new char[this.m_SearchText.Length - 1 + 1];
						}
						Array.Copy(array, array.Length - this.m_SearchText.Length, this.m_PreviousCharBuffer, 0, this.m_SearchText.Length);
					}
					else
					{
						this.m_PreviousCharBuffer = array;
					}
					if (this.m_IgnoreCase)
					{
						return new string(array).ToUpper(CultureInfo.CurrentCulture).Contains(this.m_SearchText);
					}
					return new string(array).Contains(this.m_SearchText);
				}
			}

			private TextSearchHelper()
			{
				this.m_PreviousCharBuffer = new char[0];
				this.m_CheckPreamble = true;
			}

			private static bool BytesMatch(byte[] BigBuffer, byte[] SmallBuffer)
			{
				if ((BigBuffer.Length < SmallBuffer.Length) | (SmallBuffer.Length == 0))
				{
					return false;
				}
				int num = 0;
				checked
				{
					int num2 = SmallBuffer.Length - 1;
					for (int i = num; i <= num2; i++)
					{
						if (BigBuffer[i] != SmallBuffer[i])
						{
							return false;
						}
					}
					return true;
				}
			}

			private string m_SearchText;

			private bool m_IgnoreCase;

			private Decoder m_Decoder;

			private char[] m_PreviousCharBuffer;

			private bool m_CheckPreamble;

			private byte[] m_Preamble;
		}
	}
}

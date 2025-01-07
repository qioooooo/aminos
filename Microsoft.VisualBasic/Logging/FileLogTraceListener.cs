using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Logging
{
	[ComVisible(false)]
	public class FileLogTraceListener : TraceListener
	{
		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public FileLogTraceListener(string name)
			: base(name)
		{
			this.m_Location = LogFileLocation.LocalUserApplicationDirectory;
			this.m_AutoFlush = false;
			this.m_Append = true;
			this.m_IncludeHostName = false;
			this.m_DiskSpaceExhaustedBehavior = DiskSpaceExhaustedOption.DiscardMessages;
			this.m_BaseFileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			this.m_LogFileDateStamp = LogFileCreationScheduleOption.None;
			this.m_MaxFileSize = 5000000L;
			this.m_ReserveDiskSpace = 10000000L;
			this.m_Delimiter = "\t";
			this.m_Encoding = Encoding.UTF8;
			this.m_CustomLocation = Application.UserAppDataPath;
			this.m_Day = DateAndTime.Now.Date;
			this.m_FirstDayOfWeek = FileLogTraceListener.GetFirstDayOfWeek(DateAndTime.Now.Date);
			this.m_PropertiesSet = new BitArray(12, false);
			this.m_SupportedAttributes = new string[]
			{
				"append", "Append", "autoflush", "AutoFlush", "autoFlush", "basefilename", "BaseFilename", "baseFilename", "BaseFileName", "baseFileName",
				"customlocation", "CustomLocation", "customLocation", "delimiter", "Delimiter", "diskspaceexhaustedbehavior", "DiskSpaceExhaustedBehavior", "diskSpaceExhaustedBehavior", "encoding", "Encoding",
				"includehostname", "IncludeHostName", "includeHostName", "location", "Location", "logfilecreationschedule", "LogFileCreationSchedule", "logFileCreationSchedule", "maxfilesize", "MaxFileSize",
				"maxFileSize", "reservediskspace", "ReserveDiskSpace", "reserveDiskSpace"
			};
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public FileLogTraceListener()
			: this("FileLogTraceListener")
		{
		}

		public LogFileLocation Location
		{
			get
			{
				if (!this.m_PropertiesSet[8] && this.Attributes.ContainsKey("location"))
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(LogFileLocation));
					this.Location = (LogFileLocation)converter.ConvertFromInvariantString(this.Attributes["location"]);
				}
				return this.m_Location;
			}
			set
			{
				this.ValidateLogFileLocationEnumValue(value, "value");
				if (this.m_Location != value)
				{
					this.CloseCurrentStream();
				}
				this.m_Location = value;
				this.m_PropertiesSet[8] = true;
			}
		}

		public bool AutoFlush
		{
			get
			{
				if (!this.m_PropertiesSet[1] && this.Attributes.ContainsKey("autoflush"))
				{
					this.AutoFlush = Convert.ToBoolean(this.Attributes["autoflush"], CultureInfo.InvariantCulture);
				}
				return this.m_AutoFlush;
			}
			set
			{
				this.DemandWritePermission();
				this.m_AutoFlush = value;
				this.m_PropertiesSet[1] = true;
			}
		}

		public bool IncludeHostName
		{
			get
			{
				if (!this.m_PropertiesSet[7] && this.Attributes.ContainsKey("includehostname"))
				{
					this.IncludeHostName = Convert.ToBoolean(this.Attributes["includehostname"], CultureInfo.InvariantCulture);
				}
				return this.m_IncludeHostName;
			}
			set
			{
				this.DemandWritePermission();
				this.m_IncludeHostName = value;
				this.m_PropertiesSet[7] = true;
			}
		}

		public bool Append
		{
			get
			{
				if (!this.m_PropertiesSet[0] && this.Attributes.ContainsKey("append"))
				{
					this.Append = Convert.ToBoolean(this.Attributes["append"], CultureInfo.InvariantCulture);
				}
				return this.m_Append;
			}
			set
			{
				this.DemandWritePermission();
				if (value != this.m_Append)
				{
					this.CloseCurrentStream();
				}
				this.m_Append = value;
				this.m_PropertiesSet[0] = true;
			}
		}

		public DiskSpaceExhaustedOption DiskSpaceExhaustedBehavior
		{
			get
			{
				if (!this.m_PropertiesSet[5] && this.Attributes.ContainsKey("diskspaceexhaustedbehavior"))
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(DiskSpaceExhaustedOption));
					this.DiskSpaceExhaustedBehavior = (DiskSpaceExhaustedOption)converter.ConvertFromInvariantString(this.Attributes["diskspaceexhaustedbehavior"]);
				}
				return this.m_DiskSpaceExhaustedBehavior;
			}
			set
			{
				this.DemandWritePermission();
				this.ValidateDiskSpaceExhaustedOptionEnumValue(value, "value");
				this.m_DiskSpaceExhaustedBehavior = value;
				this.m_PropertiesSet[5] = true;
			}
		}

		public string BaseFileName
		{
			get
			{
				if (!this.m_PropertiesSet[2] && this.Attributes.ContainsKey("basefilename"))
				{
					this.BaseFileName = this.Attributes["basefilename"];
				}
				return this.m_BaseFileName;
			}
			set
			{
				if (Operators.CompareString(value, "", false) == 0)
				{
					throw ExceptionUtils.GetArgumentNullException("value", "ApplicationLogBaseNameNull", new string[0]);
				}
				Path.GetFullPath(value);
				if (string.Compare(value, this.m_BaseFileName, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.CloseCurrentStream();
					this.m_BaseFileName = value;
				}
				this.m_PropertiesSet[2] = true;
			}
		}

		public string FullLogFileName
		{
			get
			{
				this.EnsureStreamIsOpen();
				string fullFileName = this.m_FullFileName;
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullFileName);
				fileIOPermission.Demand();
				return fullFileName;
			}
		}

		public LogFileCreationScheduleOption LogFileCreationSchedule
		{
			get
			{
				if (!this.m_PropertiesSet[9] && this.Attributes.ContainsKey("logfilecreationschedule"))
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(LogFileCreationScheduleOption));
					this.LogFileCreationSchedule = (LogFileCreationScheduleOption)converter.ConvertFromInvariantString(this.Attributes["logfilecreationschedule"]);
				}
				return this.m_LogFileDateStamp;
			}
			set
			{
				this.ValidateLogFileCreationScheduleOptionEnumValue(value, "value");
				if (value != this.m_LogFileDateStamp)
				{
					this.CloseCurrentStream();
					this.m_LogFileDateStamp = value;
				}
				this.m_PropertiesSet[9] = true;
			}
		}

		public long MaxFileSize
		{
			get
			{
				if (!this.m_PropertiesSet[10] && this.Attributes.ContainsKey("maxfilesize"))
				{
					this.MaxFileSize = Convert.ToInt64(this.Attributes["maxfilesize"], CultureInfo.InvariantCulture);
				}
				return this.m_MaxFileSize;
			}
			set
			{
				this.DemandWritePermission();
				if (value < 1000L)
				{
					throw ExceptionUtils.GetArgumentExceptionWithArgName("value", "ApplicationLogNumberTooSmall", new string[] { "MaxFileSize" });
				}
				this.m_MaxFileSize = value;
				this.m_PropertiesSet[10] = true;
			}
		}

		public long ReserveDiskSpace
		{
			get
			{
				if (!this.m_PropertiesSet[11] && this.Attributes.ContainsKey("reservediskspace"))
				{
					this.ReserveDiskSpace = Convert.ToInt64(this.Attributes["reservediskspace"], CultureInfo.InvariantCulture);
				}
				return this.m_ReserveDiskSpace;
			}
			set
			{
				this.DemandWritePermission();
				if (value < 0L)
				{
					throw ExceptionUtils.GetArgumentExceptionWithArgName("value", "ApplicationLog_NegativeNumber", new string[] { "ReserveDiskSpace" });
				}
				this.m_ReserveDiskSpace = value;
				this.m_PropertiesSet[11] = true;
			}
		}

		public string Delimiter
		{
			get
			{
				if (!this.m_PropertiesSet[4] && this.Attributes.ContainsKey("delimiter"))
				{
					this.Delimiter = this.Attributes["delimiter"];
				}
				return this.m_Delimiter;
			}
			set
			{
				this.m_Delimiter = value;
				this.m_PropertiesSet[4] = true;
			}
		}

		public Encoding Encoding
		{
			get
			{
				if (!this.m_PropertiesSet[6] && this.Attributes.ContainsKey("encoding"))
				{
					this.Encoding = Encoding.GetEncoding(this.Attributes["encoding"]);
				}
				return this.m_Encoding;
			}
			set
			{
				if (value == null)
				{
					throw ExceptionUtils.GetArgumentNullException("value");
				}
				this.m_Encoding = value;
				this.m_PropertiesSet[6] = true;
			}
		}

		public string CustomLocation
		{
			get
			{
				if (!this.m_PropertiesSet[3] && this.Attributes.ContainsKey("customlocation"))
				{
					this.CustomLocation = this.Attributes["customlocation"];
				}
				string fullPath = Path.GetFullPath(this.m_CustomLocation);
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullPath);
				fileIOPermission.Demand();
				return fullPath;
			}
			set
			{
				string fullPath = Path.GetFullPath(value);
				if (!Directory.Exists(fullPath))
				{
					Directory.CreateDirectory(fullPath);
				}
				if ((this.Location == LogFileLocation.Custom) & (string.Compare(fullPath, this.m_CustomLocation, StringComparison.OrdinalIgnoreCase) != 0))
				{
					this.CloseCurrentStream();
				}
				this.Location = LogFileLocation.Custom;
				this.m_CustomLocation = fullPath;
				this.m_PropertiesSet[3] = true;
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void Write(string message)
		{
			try
			{
				this.HandleDateChange();
				long num = (long)this.Encoding.GetByteCount(message);
				if (this.ResourcesAvailable(num))
				{
					this.ListenerStream.Write(message);
					if (this.AutoFlush)
					{
						this.ListenerStream.Flush();
					}
				}
			}
			catch (Exception)
			{
				this.CloseCurrentStream();
				throw;
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void WriteLine(string message)
		{
			try
			{
				this.HandleDateChange();
				long num = (long)this.Encoding.GetByteCount(message + "\r\n");
				if (this.ResourcesAvailable(num))
				{
					this.ListenerStream.WriteLine(message);
					if (this.AutoFlush)
					{
						this.ListenerStream.Flush();
					}
				}
			}
			catch (Exception)
			{
				this.CloseCurrentStream();
				throw;
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(source + this.Delimiter);
			stringBuilder.Append(Enum.GetName(typeof(TraceEventType), eventType) + this.Delimiter);
			stringBuilder.Append(id.ToString(CultureInfo.InvariantCulture) + this.Delimiter);
			stringBuilder.Append(message);
			if ((this.TraceOutputOptions & TraceOptions.Callstack) == TraceOptions.Callstack)
			{
				stringBuilder.Append(this.Delimiter + eventCache.Callstack);
			}
			if ((this.TraceOutputOptions & TraceOptions.LogicalOperationStack) == TraceOptions.LogicalOperationStack)
			{
				stringBuilder.Append(this.Delimiter + FileLogTraceListener.StackToString(eventCache.LogicalOperationStack));
			}
			if ((this.TraceOutputOptions & TraceOptions.DateTime) == TraceOptions.DateTime)
			{
				stringBuilder.Append(this.Delimiter + eventCache.DateTime.ToString("u", CultureInfo.InvariantCulture));
			}
			if ((this.TraceOutputOptions & TraceOptions.ProcessId) == TraceOptions.ProcessId)
			{
				stringBuilder.Append(this.Delimiter + eventCache.ProcessId.ToString(CultureInfo.InvariantCulture));
			}
			if ((this.TraceOutputOptions & TraceOptions.ThreadId) == TraceOptions.ThreadId)
			{
				stringBuilder.Append(this.Delimiter + eventCache.ThreadId);
			}
			if ((this.TraceOutputOptions & TraceOptions.Timestamp) == TraceOptions.Timestamp)
			{
				stringBuilder.Append(this.Delimiter + eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
			}
			if (this.IncludeHostName)
			{
				stringBuilder.Append(this.Delimiter + this.HostName);
			}
			this.WriteLine(stringBuilder.ToString());
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			string text;
			if (args != null)
			{
				text = string.Format(CultureInfo.InvariantCulture, format, args);
			}
			else
			{
				text = format;
			}
			this.TraceEvent(eventCache, source, eventType, id, text);
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			string text = "";
			if (data != null)
			{
				text = data.ToString();
			}
			this.TraceEvent(eventCache, source, eventType, id, text);
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			checked
			{
				if (data != null)
				{
					int num = data.Length - 1;
					int num2 = 0;
					int num3 = num;
					for (int i = num2; i <= num3; i++)
					{
						stringBuilder.Append(data[i].ToString());
						if (i != num)
						{
							stringBuilder.Append(this.Delimiter);
						}
					}
				}
				this.TraceEvent(eventCache, source, eventType, id, stringBuilder.ToString());
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void Flush()
		{
			if (this.m_Stream != null)
			{
				this.m_Stream.Flush();
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public override void Close()
		{
			this.Dispose(true);
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		protected override string[] GetSupportedAttributes()
		{
			return this.m_SupportedAttributes;
		}

		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.CloseCurrentStream();
			}
		}

		private string LogFileName
		{
			get
			{
				string text;
				switch (this.Location)
				{
				case LogFileLocation.TempDirectory:
					text = Path.GetTempPath();
					break;
				case LogFileLocation.LocalUserApplicationDirectory:
					text = Application.UserAppDataPath;
					break;
				case LogFileLocation.CommonApplicationDirectory:
					text = Application.CommonAppDataPath;
					break;
				case LogFileLocation.ExecutableDirectory:
					text = Path.GetDirectoryName(Application.ExecutablePath);
					break;
				case LogFileLocation.Custom:
					if (Operators.CompareString(this.CustomLocation, "", false) == 0)
					{
						text = Application.UserAppDataPath;
					}
					else
					{
						text = this.CustomLocation;
					}
					break;
				default:
					text = Application.UserAppDataPath;
					break;
				}
				string text2 = this.BaseFileName;
				switch (this.LogFileCreationSchedule)
				{
				case LogFileCreationScheduleOption.Daily:
					text2 = text2 + "-" + DateAndTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
					break;
				case LogFileCreationScheduleOption.Weekly:
					this.m_FirstDayOfWeek = DateAndTime.Now.AddDays((double)(checked(DayOfWeek.Sunday - DateAndTime.Now.DayOfWeek)));
					text2 = text2 + "-" + this.m_FirstDayOfWeek.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
					break;
				}
				return Path.Combine(text, text2);
			}
		}

		private FileLogTraceListener.ReferencedStream ListenerStream
		{
			get
			{
				this.EnsureStreamIsOpen();
				return this.m_Stream;
			}
		}

		private FileLogTraceListener.ReferencedStream GetStream()
		{
			int num = 0;
			FileLogTraceListener.ReferencedStream referencedStream = null;
			string fullPath = Path.GetFullPath(this.LogFileName + ".log");
			checked
			{
				while (referencedStream == null && num < 2147483647)
				{
					string text;
					if (num == 0)
					{
						text = Path.GetFullPath(this.LogFileName + ".log");
					}
					else
					{
						text = Path.GetFullPath(this.LogFileName + "-" + num.ToString(CultureInfo.InvariantCulture) + ".log");
					}
					string text2 = text.ToUpper(CultureInfo.InvariantCulture);
					Dictionary<string, FileLogTraceListener.ReferencedStream> streams = FileLogTraceListener.m_Streams;
					lock (streams)
					{
						if (FileLogTraceListener.m_Streams.ContainsKey(text2))
						{
							referencedStream = FileLogTraceListener.m_Streams[text2];
							if (!referencedStream.IsInUse)
							{
								FileLogTraceListener.m_Streams.Remove(text2);
								referencedStream = null;
							}
							else
							{
								if (this.Append)
								{
									FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Write, text);
									fileIOPermission.Demand();
									referencedStream.AddReference();
									this.m_FullFileName = text;
									return referencedStream;
								}
								num++;
								referencedStream = null;
								continue;
							}
						}
						Encoding encoding = this.Encoding;
						try
						{
							if (this.Append)
							{
								encoding = this.GetFileEncoding(text);
								if (encoding == null)
								{
									encoding = this.Encoding;
								}
							}
							StreamWriter streamWriter = new StreamWriter(text, this.Append, encoding);
							referencedStream = new FileLogTraceListener.ReferencedStream(streamWriter);
							referencedStream.AddReference();
							FileLogTraceListener.m_Streams.Add(text2, referencedStream);
							this.m_FullFileName = text;
							return referencedStream;
						}
						catch (IOException ex)
						{
						}
						num++;
					}
				}
				throw ExceptionUtils.GetInvalidOperationException("ApplicationLog_ExhaustedPossibleStreamNames", new string[] { fullPath });
			}
		}

		private void EnsureStreamIsOpen()
		{
			if (this.m_Stream == null)
			{
				this.m_Stream = this.GetStream();
			}
		}

		private void CloseCurrentStream()
		{
			if (this.m_Stream != null)
			{
				Dictionary<string, FileLogTraceListener.ReferencedStream> streams = FileLogTraceListener.m_Streams;
				lock (streams)
				{
					this.m_Stream.CloseStream();
					if (!this.m_Stream.IsInUse)
					{
						FileLogTraceListener.m_Streams.Remove(this.m_FullFileName.ToUpper(CultureInfo.InvariantCulture));
					}
					this.m_Stream = null;
				}
			}
		}

		private bool DayChanged()
		{
			return DateTime.Compare(this.m_Day.Date, DateAndTime.Now.Date) != 0;
		}

		private bool WeekChanged()
		{
			return DateTime.Compare(this.m_FirstDayOfWeek.Date, FileLogTraceListener.GetFirstDayOfWeek(DateAndTime.Now.Date)) != 0;
		}

		private static DateTime GetFirstDayOfWeek(DateTime checkDate)
		{
			return checkDate.AddDays((double)(checked(DayOfWeek.Sunday - checkDate.DayOfWeek))).Date;
		}

		private void HandleDateChange()
		{
			if (this.LogFileCreationSchedule == LogFileCreationScheduleOption.Daily)
			{
				if (this.DayChanged())
				{
					this.CloseCurrentStream();
				}
			}
			else if (this.LogFileCreationSchedule == LogFileCreationScheduleOption.Weekly && this.WeekChanged())
			{
				this.CloseCurrentStream();
			}
		}

		private bool ResourcesAvailable(long newEntrySize)
		{
			checked
			{
				if (this.ListenerStream.FileSize + newEntrySize > this.MaxFileSize)
				{
					if (this.DiskSpaceExhaustedBehavior == DiskSpaceExhaustedOption.ThrowException)
					{
						throw new InvalidOperationException(Utils.GetResourceString("ApplicationLog_FileExceedsMaximumSize"));
					}
					return false;
				}
				else
				{
					if (this.GetFreeDiskSpace() - newEntrySize >= this.ReserveDiskSpace)
					{
						return true;
					}
					if (this.DiskSpaceExhaustedBehavior == DiskSpaceExhaustedOption.ThrowException)
					{
						throw new InvalidOperationException(Utils.GetResourceString("ApplicationLog_ReservedSpaceEncroached"));
					}
					return false;
				}
			}
		}

		private long GetFreeDiskSpace()
		{
			string pathRoot = Path.GetPathRoot(Path.GetFullPath(this.FullLogFileName));
			long num = -1L;
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, pathRoot);
			fileIOPermission.Demand();
			long num2;
			long num3;
			if (UnsafeNativeMethods.GetDiskFreeSpaceEx(pathRoot, ref num, ref num2, ref num3) && num > -1L)
			{
				return num;
			}
			throw ExceptionUtils.GetWin32Exception("ApplicationLog_FreeSpaceError", new string[0]);
		}

		private Encoding GetFileEncoding(string fileName)
		{
			if (File.Exists(fileName))
			{
				StreamReader streamReader = null;
				try
				{
					streamReader = new StreamReader(fileName, this.Encoding, true);
					if (streamReader.BaseStream.Length > 0L)
					{
						streamReader.ReadLine();
						return streamReader.CurrentEncoding;
					}
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
			}
			return null;
		}

		private string HostName
		{
			get
			{
				if (Operators.CompareString(this.m_HostName, "", false) == 0)
				{
					this.m_HostName = Environment.MachineName;
				}
				return this.m_HostName;
			}
		}

		private void DemandWritePermission()
		{
			string directoryName = Path.GetDirectoryName(this.LogFileName);
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Write, directoryName);
			fileIOPermission.Demand();
		}

		private void ValidateLogFileLocationEnumValue(LogFileLocation value, string paramName)
		{
			if (value < LogFileLocation.TempDirectory || value > LogFileLocation.Custom)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(LogFileLocation));
			}
		}

		private void ValidateDiskSpaceExhaustedOptionEnumValue(DiskSpaceExhaustedOption value, string paramName)
		{
			if (value < DiskSpaceExhaustedOption.ThrowException || value > DiskSpaceExhaustedOption.DiscardMessages)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(DiskSpaceExhaustedOption));
			}
		}

		private void ValidateLogFileCreationScheduleOptionEnumValue(LogFileCreationScheduleOption value, string paramName)
		{
			if (value < LogFileCreationScheduleOption.None || value > LogFileCreationScheduleOption.Weekly)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(LogFileCreationScheduleOption));
			}
		}

		private static string StackToString(Stack stack)
		{
			int length = ", ".Length;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				foreach (object obj in stack)
				{
					stringBuilder.Append(obj.ToString() + ", ");
				}
			}
			finally
			{
				IEnumerator enumerator;
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
			stringBuilder.Replace("\"", "\"\"");
			if (stringBuilder.Length >= length)
			{
				stringBuilder.Remove(checked(stringBuilder.Length - length), length);
			}
			return "\"" + stringBuilder.ToString() + "\"";
		}

		private LogFileLocation m_Location;

		private bool m_AutoFlush;

		private bool m_Append;

		private bool m_IncludeHostName;

		private DiskSpaceExhaustedOption m_DiskSpaceExhaustedBehavior;

		private string m_BaseFileName;

		private LogFileCreationScheduleOption m_LogFileDateStamp;

		private long m_MaxFileSize;

		private long m_ReserveDiskSpace;

		private string m_Delimiter;

		private Encoding m_Encoding;

		private string m_FullFileName;

		private string m_CustomLocation;

		private FileLogTraceListener.ReferencedStream m_Stream;

		private DateTime m_Day;

		private DateTime m_FirstDayOfWeek;

		private string m_HostName;

		private BitArray m_PropertiesSet;

		private static Dictionary<string, FileLogTraceListener.ReferencedStream> m_Streams = new Dictionary<string, FileLogTraceListener.ReferencedStream>();

		private string[] m_SupportedAttributes;

		private const int PROPERTY_COUNT = 12;

		private const int APPEND_INDEX = 0;

		private const int AUTOFLUSH_INDEX = 1;

		private const int BASEFILENAME_INDEX = 2;

		private const int CUSTOMLOCATION_INDEX = 3;

		private const int DELIMITER_INDEX = 4;

		private const int DISKSPACEEXHAUSTEDBEHAVIOR_INDEX = 5;

		private const int ENCODING_INDEX = 6;

		private const int INCLUDEHOSTNAME_INDEX = 7;

		private const int LOCATION_INDEX = 8;

		private const int LOGFILECREATIONSCHEDULE_INDEX = 9;

		private const int MAXFILESIZE_INDEX = 10;

		private const int RESERVEDISKSPACE_INDEX = 11;

		private const string DATE_FORMAT = "yyyy-MM-dd";

		private const string FILE_EXTENSION = ".log";

		private const int MAX_OPEN_ATTEMPTS = 2147483647;

		private const string DEFAULT_NAME = "FileLogTraceListener";

		private const int MIN_FILE_SIZE = 1000;

		private const string KEY_APPEND = "append";

		private const string KEY_APPEND_PASCAL = "Append";

		private const string KEY_AUTOFLUSH = "autoflush";

		private const string KEY_AUTOFLUSH_PASCAL = "AutoFlush";

		private const string KEY_AUTOFLUSH_CAMEL = "autoFlush";

		private const string KEY_BASEFILENAME = "basefilename";

		private const string KEY_BASEFILENAME_PASCAL = "BaseFilename";

		private const string KEY_BASEFILENAME_CAMEL = "baseFilename";

		private const string KEY_BASEFILENAME_PASCAL_ALT = "BaseFileName";

		private const string KEY_BASEFILENAME_CAMEL_ALT = "baseFileName";

		private const string KEY_CUSTOMLOCATION = "customlocation";

		private const string KEY_CUSTOMLOCATION_PASCAL = "CustomLocation";

		private const string KEY_CUSTOMLOCATION_CAMEL = "customLocation";

		private const string KEY_DELIMITER = "delimiter";

		private const string KEY_DELIMITER_PASCAL = "Delimiter";

		private const string KEY_DISKSPACEEXHAUSTEDBEHAVIOR = "diskspaceexhaustedbehavior";

		private const string KEY_DISKSPACEEXHAUSTEDBEHAVIOR_PASCAL = "DiskSpaceExhaustedBehavior";

		private const string KEY_DISKSPACEEXHAUSTEDBEHAVIOR_CAMEL = "diskSpaceExhaustedBehavior";

		private const string KEY_ENCODING = "encoding";

		private const string KEY_ENCODING_PASCAL = "Encoding";

		private const string KEY_INCLUDEHOSTNAME = "includehostname";

		private const string KEY_INCLUDEHOSTNAME_PASCAL = "IncludeHostName";

		private const string KEY_INCLUDEHOSTNAME_CAMEL = "includeHostName";

		private const string KEY_LOCATION = "location";

		private const string KEY_LOCATION_PASCAL = "Location";

		private const string KEY_LOGFILECREATIONSCHEDULE = "logfilecreationschedule";

		private const string KEY_LOGFILECREATIONSCHEDULE_PASCAL = "LogFileCreationSchedule";

		private const string KEY_LOGFILECREATIONSCHEDULE_CAMEL = "logFileCreationSchedule";

		private const string KEY_MAXFILESIZE = "maxfilesize";

		private const string KEY_MAXFILESIZE_PASCAL = "MaxFileSize";

		private const string KEY_MAXFILESIZE_CAMEL = "maxFileSize";

		private const string KEY_RESERVEDISKSPACE = "reservediskspace";

		private const string KEY_RESERVEDISKSPACE_PASCAL = "ReserveDiskSpace";

		private const string KEY_RESERVEDISKSPACE_CAMEL = "reserveDiskSpace";

		private const string STACK_DELIMITER = ", ";

		internal class ReferencedStream : IDisposable
		{
			internal ReferencedStream(StreamWriter stream)
			{
				this.m_ReferenceCount = 0;
				this.m_SyncObject = new object();
				this.m_Disposed = false;
				this.m_Stream = stream;
			}

			internal void Write(string message)
			{
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				lock (syncObject)
				{
					this.m_Stream.Write(message);
				}
			}

			internal void WriteLine(string message)
			{
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				lock (syncObject)
				{
					this.m_Stream.WriteLine(message);
				}
			}

			internal void AddReference()
			{
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				checked
				{
					lock (syncObject)
					{
						this.m_ReferenceCount++;
					}
				}
			}

			internal void Flush()
			{
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				lock (syncObject)
				{
					this.m_Stream.Flush();
				}
			}

			internal void CloseStream()
			{
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				checked
				{
					lock (syncObject)
					{
						try
						{
							this.m_ReferenceCount--;
							this.m_Stream.Flush();
						}
						finally
						{
							if (this.m_ReferenceCount <= 0)
							{
								this.m_Stream.Close();
								this.m_Stream = null;
							}
						}
					}
				}
			}

			internal bool IsInUse
			{
				get
				{
					return this.m_Stream != null;
				}
			}

			internal long FileSize
			{
				get
				{
					return this.m_Stream.BaseStream.Length;
				}
			}

			private void Dispose(bool disposing)
			{
				if (disposing && !this.m_Disposed)
				{
					if (this.m_Stream != null)
					{
						this.m_Stream.Close();
					}
					this.m_Disposed = true;
				}
			}

			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected override void Finalize()
			{
				this.Dispose(false);
				base.Finalize();
			}

			private StreamWriter m_Stream;

			private int m_ReferenceCount;

			private object m_SyncObject;

			private bool m_Disposed;
		}
	}
}

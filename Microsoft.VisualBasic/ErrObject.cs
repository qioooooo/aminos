using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	public sealed class ErrObject
	{
		internal ErrObject()
		{
			this.Clear();
		}

		public int Erl
		{
			get
			{
				return this.m_curErl;
			}
		}

		public int Number
		{
			get
			{
				if (this.m_NumberIsSet)
				{
					return this.m_curNumber;
				}
				if (this.m_curException != null)
				{
					this.Number = this.MapExceptionToNumber(this.m_curException);
					return this.m_curNumber;
				}
				return 0;
			}
			set
			{
				this.m_curNumber = this.MapErrorNumber(value);
				this.m_NumberIsSet = true;
			}
		}

		public string Source
		{
			get
			{
				if (this.m_SourceIsSet)
				{
					return this.m_curSource;
				}
				if (this.m_curException != null)
				{
					this.Source = this.m_curException.Source;
					return this.m_curSource;
				}
				return "";
			}
			set
			{
				this.m_curSource = value;
				this.m_SourceIsSet = true;
			}
		}

		private string FilterDefaultMessage(string Msg)
		{
			if (this.m_curException == null)
			{
				return Msg;
			}
			int number = this.Number;
			if (Msg == null || Msg.Length == 0)
			{
				Msg = Utils.GetResourceString("ID" + Conversions.ToString(number));
			}
			else if (string.CompareOrdinal("Exception from HRESULT: 0x", 0, Msg, 0, Math.Min(Msg.Length, 26)) == 0)
			{
				string resourceString = Utils.GetResourceString("ID" + Conversions.ToString(this.m_curNumber), false);
				if (resourceString != null)
				{
					Msg = resourceString;
				}
			}
			return Msg;
		}

		public string Description
		{
			get
			{
				if (this.m_DescriptionIsSet)
				{
					return this.m_curDescription;
				}
				if (this.m_curException != null)
				{
					this.Description = this.FilterDefaultMessage(this.m_curException.Message);
					return this.m_curDescription;
				}
				return "";
			}
			set
			{
				this.m_curDescription = value;
				this.m_DescriptionIsSet = true;
			}
		}

		public string HelpFile
		{
			get
			{
				if (this.m_HelpFileIsSet)
				{
					return this.m_curHelpFile;
				}
				if (this.m_curException != null)
				{
					this.ParseHelpLink(this.m_curException.HelpLink);
					return this.m_curHelpFile;
				}
				return "";
			}
			set
			{
				this.m_curHelpFile = value;
				this.m_HelpFileIsSet = true;
			}
		}

		private string MakeHelpLink(string HelpFile, int HelpContext)
		{
			return HelpFile + "#" + Conversions.ToString(HelpContext);
		}

		private void ParseHelpLink(string HelpLink)
		{
			if (HelpLink == null || HelpLink.Length == 0)
			{
				if (!this.m_HelpContextIsSet)
				{
					this.HelpContext = 0;
				}
				if (!this.m_HelpFileIsSet)
				{
					this.HelpFile = "";
				}
			}
			else
			{
				int num = Strings.m_InvariantCompareInfo.IndexOf(HelpLink, "#", CompareOptions.Ordinal);
				if (num != -1)
				{
					if (!this.m_HelpContextIsSet)
					{
						if (num < HelpLink.Length)
						{
							this.HelpContext = Conversions.ToInteger(HelpLink.Substring(checked(num + 1)));
						}
						else
						{
							this.HelpContext = 0;
						}
					}
					if (!this.m_HelpFileIsSet)
					{
						this.HelpFile = HelpLink.Substring(0, num);
					}
				}
				else
				{
					if (!this.m_HelpContextIsSet)
					{
						this.HelpContext = 0;
					}
					if (!this.m_HelpFileIsSet)
					{
						this.HelpFile = HelpLink;
					}
				}
			}
		}

		public int HelpContext
		{
			get
			{
				if (this.m_HelpContextIsSet)
				{
					return this.m_curHelpContext;
				}
				if (this.m_curException != null)
				{
					this.ParseHelpLink(this.m_curException.HelpLink);
					return this.m_curHelpContext;
				}
				return 0;
			}
			set
			{
				this.m_curHelpContext = value;
				this.m_HelpContextIsSet = true;
			}
		}

		public Exception GetException()
		{
			return this.m_curException;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Clear()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.m_curException = null;
				this.m_curNumber = 0;
				this.m_curSource = "";
				this.m_curDescription = "";
				this.m_curHelpFile = "";
				this.m_curHelpContext = 0;
				this.m_curErl = 0;
				this.m_NumberIsSet = false;
				this.m_SourceIsSet = false;
				this.m_DescriptionIsSet = false;
				this.m_HelpFileIsSet = false;
				this.m_HelpContextIsSet = false;
				this.m_ClearOnCapture = true;
			}
		}

		public void Raise(int Number, object Source = null, object Description = null, object HelpFile = null, object HelpContext = null)
		{
			if (Number == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Number" }));
			}
			this.Number = Number;
			if (Source != null)
			{
				this.Source = Conversions.ToString(Source);
			}
			else
			{
				IVbHost vbhost = HostServices.VBHost;
				if (vbhost == null)
				{
					string fullName = Assembly.GetCallingAssembly().FullName;
					int num = Strings.InStr(fullName, ",", CompareMethod.Binary);
					if (num < 1)
					{
						this.Source = fullName;
					}
					else
					{
						this.Source = Strings.Left(fullName, checked(num - 1));
					}
				}
				else
				{
					this.Source = vbhost.GetWindowTitle();
				}
			}
			if (HelpFile != null)
			{
				this.HelpFile = Conversions.ToString(HelpFile);
			}
			if (HelpContext != null)
			{
				this.HelpContext = Conversions.ToInteger(HelpContext);
			}
			if (Description != null)
			{
				this.Description = Conversions.ToString(Description);
			}
			else if (!this.m_DescriptionIsSet)
			{
				this.Description = Utils.GetResourceString((vbErrors)this.m_curNumber);
			}
			Exception ex = this.MapNumberToException(this.m_curNumber, this.m_curDescription);
			ex.Source = this.m_curSource;
			ex.HelpLink = this.MakeHelpLink(this.m_curHelpFile, this.m_curHelpContext);
			this.m_ClearOnCapture = false;
			throw ex;
		}

		public int LastDllError
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return Marshal.GetLastWin32Error();
			}
		}

		internal void SetUnmappedError(int Number)
		{
			this.Clear();
			this.Number = Number;
			this.m_ClearOnCapture = false;
		}

		internal Exception CreateException(int Number, string Description)
		{
			this.Clear();
			this.Number = Number;
			if (Number == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Number" }));
			}
			Exception ex = this.MapNumberToException(this.m_curNumber, Description);
			this.m_ClearOnCapture = false;
			return ex;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void CaptureException(Exception ex)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (ex != this.m_curException)
				{
					if (this.m_ClearOnCapture)
					{
						this.Clear();
					}
					else
					{
						this.m_ClearOnCapture = true;
					}
					this.m_curException = ex;
				}
			}
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void CaptureException(Exception ex, int lErl)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.CaptureException(ex);
				this.m_curErl = lErl;
			}
		}

		private int MapExceptionToNumber(Exception e)
		{
			Type type = e.GetType();
			if (type == typeof(IndexOutOfRangeException))
			{
				return 9;
			}
			if (type == typeof(RankException))
			{
				return 9;
			}
			if (type == typeof(DivideByZeroException))
			{
				return 11;
			}
			if (type == typeof(OverflowException))
			{
				return 6;
			}
			if (type == typeof(NotFiniteNumberException))
			{
				NotFiniteNumberException ex = (NotFiniteNumberException)e;
				if (ex.OffendingNumber == 0.0)
				{
					return 11;
				}
				return 6;
			}
			else
			{
				if (type == typeof(NullReferenceException))
				{
					return 91;
				}
				if (e is AccessViolationException)
				{
					return -2147467261;
				}
				if (type == typeof(InvalidCastException))
				{
					return 13;
				}
				if (type == typeof(NotSupportedException))
				{
					return 13;
				}
				if (type == typeof(COMException))
				{
					COMException ex2 = (COMException)e;
					return Utils.MapHRESULT(ex2.ErrorCode);
				}
				if (type == typeof(SEHException))
				{
					return 99;
				}
				if (type == typeof(DllNotFoundException))
				{
					return 53;
				}
				if (type == typeof(EntryPointNotFoundException))
				{
					return 453;
				}
				if (type == typeof(TypeLoadException))
				{
					return 429;
				}
				if (type == typeof(OutOfMemoryException))
				{
					return 7;
				}
				if (type == typeof(FormatException))
				{
					return 13;
				}
				if (type == typeof(DirectoryNotFoundException))
				{
					return 76;
				}
				if (type == typeof(IOException))
				{
					return 57;
				}
				if (type == typeof(FileNotFoundException))
				{
					return 53;
				}
				if (e is MissingMemberException)
				{
					return 438;
				}
				if (e is InvalidOleVariantTypeException)
				{
					return 458;
				}
				return 5;
			}
		}

		private Exception MapNumberToException(int Number, string Description)
		{
			bool flag = false;
			return ExceptionUtils.BuildException(Number, Description, ref flag);
		}

		internal int MapErrorNumber(int Number)
		{
			if (Number > 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Number" }));
			}
			if (Number >= 0)
			{
				return Number;
			}
			if ((Number & 536805376) == 655360)
			{
				return Number & 65535;
			}
			if (Number == -2147467263)
			{
				return 445;
			}
			if (Number == -2147467262)
			{
				return 430;
			}
			if (Number == -2147467261)
			{
				return -2147467261;
			}
			if (Number == -2147467260)
			{
				return 287;
			}
			if (Number == -2147352575)
			{
				return 438;
			}
			if (Number == -2147352573)
			{
				return 438;
			}
			if (Number == -2147352572)
			{
				return 448;
			}
			if (Number == -2147352571)
			{
				return 13;
			}
			if (Number == -2147352570)
			{
				return 438;
			}
			if (Number == -2147352569)
			{
				return 446;
			}
			if (Number == -2147352568)
			{
				return 458;
			}
			if (Number == -2147352566)
			{
				return 6;
			}
			if (Number == -2147352565)
			{
				return 9;
			}
			if (Number == -2147352564)
			{
				return 447;
			}
			if (Number == -2147352563)
			{
				return 10;
			}
			if (Number == -2147352562)
			{
				return 450;
			}
			if (Number == -2147352561)
			{
				return 449;
			}
			if (Number == -2147352559)
			{
				return 451;
			}
			if (Number == -2147352558)
			{
				return 11;
			}
			if (Number == -2147319786)
			{
				return 32790;
			}
			if (Number == -2147319785)
			{
				return 461;
			}
			if (Number == -2147319784)
			{
				return 32792;
			}
			if (Number == -2147319783)
			{
				return 32793;
			}
			if (Number == -2147319780)
			{
				return 32796;
			}
			if (Number == -2147319779)
			{
				return 32797;
			}
			if (Number == -2147319769)
			{
				return 32807;
			}
			if (Number == -2147319768)
			{
				return 32808;
			}
			if (Number == -2147319767)
			{
				return 32809;
			}
			if (Number == -2147319766)
			{
				return 32810;
			}
			if (Number == -2147319765)
			{
				return 32811;
			}
			if (Number == -2147319764)
			{
				return 32812;
			}
			if (Number == -2147319763)
			{
				return 32813;
			}
			if (Number == -2147319762)
			{
				return 32814;
			}
			if (Number == -2147319761)
			{
				return 453;
			}
			if (Number == -2147317571)
			{
				return 35005;
			}
			if (Number == -2147317563)
			{
				return 35013;
			}
			if (Number == -2147316576)
			{
				return 13;
			}
			if (Number == -2147316575)
			{
				return 9;
			}
			if (Number == -2147316574)
			{
				return 57;
			}
			if (Number == -2147316573)
			{
				return 322;
			}
			if (Number == -2147312566)
			{
				return 48;
			}
			if (Number == -2147312509)
			{
				return 40067;
			}
			if (Number == -2147312508)
			{
				return 40068;
			}
			if (Number == -2147287039)
			{
				return 32774;
			}
			if (Number == -2147287038)
			{
				return 53;
			}
			if (Number == -2147287037)
			{
				return 76;
			}
			if (Number == -2147287036)
			{
				return 67;
			}
			if (Number == -2147287035)
			{
				return 70;
			}
			if (Number == -2147287034)
			{
				return 32772;
			}
			if (Number == -2147287032)
			{
				return 7;
			}
			if (Number == -2147287022)
			{
				return 67;
			}
			if (Number == -2147287021)
			{
				return 70;
			}
			if (Number == -2147287015)
			{
				return 32771;
			}
			if (Number == -2147287011)
			{
				return 32773;
			}
			if (Number == -2147287010)
			{
				return 32772;
			}
			if (Number == -2147287008)
			{
				return 75;
			}
			if (Number == -2147287007)
			{
				return 70;
			}
			if (Number == -2147286960)
			{
				return 58;
			}
			if (Number == -2147286928)
			{
				return 61;
			}
			if (Number == -2147286789)
			{
				return 32792;
			}
			if (Number == -2147286788)
			{
				return 53;
			}
			if (Number == -2147286787)
			{
				return 32792;
			}
			if (Number == -2147286786)
			{
				return 32768;
			}
			if (Number == -2147286784)
			{
				return 70;
			}
			if (Number == -2147286783)
			{
				return 70;
			}
			if (Number == -2147286782)
			{
				return 32773;
			}
			if (Number == -2147286781)
			{
				return 57;
			}
			if (Number == -2147286780)
			{
				return 32793;
			}
			if (Number == -2147286779)
			{
				return 32793;
			}
			if (Number == -2147286778)
			{
				return 32789;
			}
			if (Number == -2147286777)
			{
				return 32793;
			}
			if (Number == -2147286776)
			{
				return 32793;
			}
			if (Number == -2147221230)
			{
				return 429;
			}
			if (Number == -2147221164)
			{
				return 429;
			}
			if (Number == -2147221021)
			{
				return 429;
			}
			if (Number == -2147221018)
			{
				return 432;
			}
			if (Number == -2147221014)
			{
				return 432;
			}
			if (Number == -2147221005)
			{
				return 429;
			}
			if (Number == -2147221003)
			{
				return 429;
			}
			if (Number == -2147220994)
			{
				return 429;
			}
			if (Number == -2147024891)
			{
				return 70;
			}
			if (Number == -2147024882)
			{
				return 7;
			}
			if (Number == -2147024809)
			{
				return 5;
			}
			if (Number == -2147023174)
			{
				return 462;
			}
			if (Number == -2146959355)
			{
				return 429;
			}
			return Number;
		}

		private Exception m_curException;

		private int m_curErl;

		private int m_curNumber;

		private string m_curSource;

		private string m_curDescription;

		private string m_curHelpFile;

		private int m_curHelpContext;

		private bool m_NumberIsSet;

		private bool m_ClearOnCapture;

		private bool m_SourceIsSet;

		private bool m_DescriptionIsSet;

		private bool m_HelpFileIsSet;

		private bool m_HelpContextIsSet;
	}
}

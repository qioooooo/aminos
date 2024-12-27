using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Ports
{
	// Token: 0x020007B5 RID: 1973
	internal sealed class SerialStream : Stream
	{
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06003C9B RID: 15515 RVA: 0x00102F88 File Offset: 0x00101F88
		// (remove) Token: 0x06003C9C RID: 15516 RVA: 0x00102FA1 File Offset: 0x00101FA1
		internal event SerialDataReceivedEventHandler DataReceived;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06003C9D RID: 15517 RVA: 0x00102FBA File Offset: 0x00101FBA
		// (remove) Token: 0x06003C9E RID: 15518 RVA: 0x00102FD3 File Offset: 0x00101FD3
		internal event SerialPinChangedEventHandler PinChanged;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06003C9F RID: 15519 RVA: 0x00102FEC File Offset: 0x00101FEC
		// (remove) Token: 0x06003CA0 RID: 15520 RVA: 0x00103005 File Offset: 0x00102005
		internal event SerialErrorReceivedEventHandler ErrorReceived;

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0010301E File Offset: 0x0010201E
		public override bool CanRead
		{
			get
			{
				return this._handle != null;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x0010302C File Offset: 0x0010202C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x0010302F File Offset: 0x0010202F
		public override bool CanTimeout
		{
			get
			{
				return this._handle != null;
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x0010303D File Offset: 0x0010203D
		public override bool CanWrite
		{
			get
			{
				return this._handle != null;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x0010304B File Offset: 0x0010204B
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x0010305C File Offset: 0x0010205C
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x0010306D File Offset: 0x0010206D
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (set) Token: 0x06003CA8 RID: 15528 RVA: 0x00103080 File Offset: 0x00102080
		internal int BaudRate
		{
			set
			{
				if (value > 0 && (value <= this.commProp.dwMaxBaud || this.commProp.dwMaxBaud <= 0))
				{
					if ((long)value != (long)((ulong)this.dcb.BaudRate))
					{
						int baudRate = (int)this.dcb.BaudRate;
						this.dcb.BaudRate = (uint)value;
						if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
						{
							this.dcb.BaudRate = (uint)baudRate;
							InternalResources.WinIOError();
						}
					}
					return;
				}
				if (this.commProp.dwMaxBaud == 0)
				{
					throw new ArgumentOutOfRangeException("baudRate", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
				}
				throw new ArgumentOutOfRangeException("baudRate", SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[]
				{
					0,
					this.commProp.dwMaxBaud
				}));
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06003CA9 RID: 15529 RVA: 0x00103154 File Offset: 0x00102154
		// (set) Token: 0x06003CAA RID: 15530 RVA: 0x0010315C File Offset: 0x0010215C
		public bool BreakState
		{
			get
			{
				return this.inBreak;
			}
			set
			{
				if (value)
				{
					if (!UnsafeNativeMethods.SetCommBreak(this._handle))
					{
						InternalResources.WinIOError();
					}
					this.inBreak = true;
					return;
				}
				if (!UnsafeNativeMethods.ClearCommBreak(this._handle))
				{
					InternalResources.WinIOError();
				}
				this.inBreak = false;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (set) Token: 0x06003CAB RID: 15531 RVA: 0x00103194 File Offset: 0x00102194
		internal int DataBits
		{
			set
			{
				if (value != (int)this.dcb.ByteSize)
				{
					byte byteSize = this.dcb.ByteSize;
					this.dcb.ByteSize = (byte)value;
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.dcb.ByteSize = byteSize;
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (set) Token: 0x06003CAC RID: 15532 RVA: 0x001031EC File Offset: 0x001021EC
		internal bool DiscardNull
		{
			set
			{
				int dcbFlag = this.GetDcbFlag(11);
				if ((value && dcbFlag == 0) || (!value && dcbFlag == 1))
				{
					int num = dcbFlag;
					this.SetDcbFlag(11, value ? 1 : 0);
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.SetDcbFlag(11, num);
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06003CAD RID: 15533 RVA: 0x00103244 File Offset: 0x00102244
		// (set) Token: 0x06003CAE RID: 15534 RVA: 0x00103260 File Offset: 0x00102260
		internal bool DtrEnable
		{
			get
			{
				int dcbFlag = this.GetDcbFlag(4);
				return dcbFlag == 1;
			}
			set
			{
				int dcbFlag = this.GetDcbFlag(4);
				this.SetDcbFlag(4, value ? 1 : 0);
				if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
				{
					this.SetDcbFlag(4, dcbFlag);
					InternalResources.WinIOError();
				}
				if (!UnsafeNativeMethods.EscapeCommFunction(this._handle, value ? 5 : 6))
				{
					InternalResources.WinIOError();
				}
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (set) Token: 0x06003CAF RID: 15535 RVA: 0x001032BC File Offset: 0x001022BC
		internal Handshake Handshake
		{
			set
			{
				if (value != this.handshake)
				{
					Handshake handshake = this.handshake;
					int dcbFlag = this.GetDcbFlag(9);
					int dcbFlag2 = this.GetDcbFlag(2);
					int dcbFlag3 = this.GetDcbFlag(12);
					this.handshake = value;
					int num = ((this.handshake == Handshake.XOnXOff || this.handshake == Handshake.RequestToSendXOnXOff) ? 1 : 0);
					this.SetDcbFlag(9, num);
					this.SetDcbFlag(8, num);
					this.SetDcbFlag(2, (this.handshake == Handshake.RequestToSend || this.handshake == Handshake.RequestToSendXOnXOff) ? 1 : 0);
					if (this.handshake == Handshake.RequestToSend || this.handshake == Handshake.RequestToSendXOnXOff)
					{
						this.SetDcbFlag(12, 2);
					}
					else if (this.rtsEnable)
					{
						this.SetDcbFlag(12, 1);
					}
					else
					{
						this.SetDcbFlag(12, 0);
					}
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.handshake = handshake;
						this.SetDcbFlag(9, dcbFlag);
						this.SetDcbFlag(8, dcbFlag);
						this.SetDcbFlag(2, dcbFlag2);
						this.SetDcbFlag(12, dcbFlag3);
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06003CB0 RID: 15536 RVA: 0x001033BF File Offset: 0x001023BF
		internal bool IsOpen
		{
			get
			{
				return this._handle != null && !this.eventRunner.ShutdownLoop;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (set) Token: 0x06003CB1 RID: 15537 RVA: 0x001033DC File Offset: 0x001023DC
		internal Parity Parity
		{
			set
			{
				if ((byte)value != this.dcb.Parity)
				{
					byte parity = this.dcb.Parity;
					int dcbFlag = this.GetDcbFlag(1);
					byte errorChar = this.dcb.ErrorChar;
					int dcbFlag2 = this.GetDcbFlag(10);
					this.dcb.Parity = (byte)value;
					int num = ((this.dcb.Parity == 0) ? 0 : 1);
					this.SetDcbFlag(1, num);
					if (num == 1)
					{
						this.SetDcbFlag(10, (this.parityReplace != 0) ? 1 : 0);
						this.dcb.ErrorChar = this.parityReplace;
					}
					else
					{
						this.SetDcbFlag(10, 0);
						this.dcb.ErrorChar = 0;
					}
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.dcb.Parity = parity;
						this.SetDcbFlag(1, dcbFlag);
						this.dcb.ErrorChar = errorChar;
						this.SetDcbFlag(10, dcbFlag2);
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (set) Token: 0x06003CB2 RID: 15538 RVA: 0x001034D0 File Offset: 0x001024D0
		internal byte ParityReplace
		{
			set
			{
				if (value != this.parityReplace)
				{
					byte b = this.parityReplace;
					byte errorChar = this.dcb.ErrorChar;
					int dcbFlag = this.GetDcbFlag(10);
					this.parityReplace = value;
					if (this.GetDcbFlag(1) == 1)
					{
						this.SetDcbFlag(10, (this.parityReplace != 0) ? 1 : 0);
						this.dcb.ErrorChar = this.parityReplace;
					}
					else
					{
						this.SetDcbFlag(10, 0);
						this.dcb.ErrorChar = 0;
					}
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.parityReplace = b;
						this.SetDcbFlag(10, dcbFlag);
						this.dcb.ErrorChar = errorChar;
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x00103588 File Offset: 0x00102588
		// (set) Token: 0x06003CB4 RID: 15540 RVA: 0x001035AC File Offset: 0x001025AC
		public override int ReadTimeout
		{
			get
			{
				int readTotalTimeoutConstant = this.commTimeouts.ReadTotalTimeoutConstant;
				if (readTotalTimeoutConstant == -2)
				{
					return -1;
				}
				return readTotalTimeoutConstant;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("ReadTimeout", SR.GetString("ArgumentOutOfRange_Timeout"));
				}
				if (this._handle == null)
				{
					InternalResources.FileNotOpen();
				}
				int readTotalTimeoutConstant = this.commTimeouts.ReadTotalTimeoutConstant;
				int readIntervalTimeout = this.commTimeouts.ReadIntervalTimeout;
				int readTotalTimeoutMultiplier = this.commTimeouts.ReadTotalTimeoutMultiplier;
				if (value == 0)
				{
					this.commTimeouts.ReadTotalTimeoutConstant = 0;
					this.commTimeouts.ReadTotalTimeoutMultiplier = 0;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				else if (value == -1)
				{
					this.commTimeouts.ReadTotalTimeoutConstant = -2;
					this.commTimeouts.ReadTotalTimeoutMultiplier = -1;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				else
				{
					this.commTimeouts.ReadTotalTimeoutConstant = value;
					this.commTimeouts.ReadTotalTimeoutMultiplier = -1;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				if (!UnsafeNativeMethods.SetCommTimeouts(this._handle, ref this.commTimeouts))
				{
					this.commTimeouts.ReadTotalTimeoutConstant = readTotalTimeoutConstant;
					this.commTimeouts.ReadTotalTimeoutMultiplier = readTotalTimeoutMultiplier;
					this.commTimeouts.ReadIntervalTimeout = readIntervalTimeout;
					InternalResources.WinIOError();
				}
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x001036BC File Offset: 0x001026BC
		// (set) Token: 0x06003CB6 RID: 15542 RVA: 0x001036EC File Offset: 0x001026EC
		internal bool RtsEnable
		{
			get
			{
				int dcbFlag = this.GetDcbFlag(12);
				if (dcbFlag == 2)
				{
					throw new InvalidOperationException(SR.GetString("CantSetRtsWithHandshaking"));
				}
				return dcbFlag == 1;
			}
			set
			{
				if (this.handshake == Handshake.RequestToSend || this.handshake == Handshake.RequestToSendXOnXOff)
				{
					throw new InvalidOperationException(SR.GetString("CantSetRtsWithHandshaking"));
				}
				if (value != this.rtsEnable)
				{
					int dcbFlag = this.GetDcbFlag(12);
					this.rtsEnable = value;
					if (value)
					{
						this.SetDcbFlag(12, 1);
					}
					else
					{
						this.SetDcbFlag(12, 0);
					}
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.SetDcbFlag(12, dcbFlag);
						this.rtsEnable = !this.rtsEnable;
						InternalResources.WinIOError();
					}
					if (!UnsafeNativeMethods.EscapeCommFunction(this._handle, value ? 3 : 4))
					{
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (set) Token: 0x06003CB7 RID: 15543 RVA: 0x00103794 File Offset: 0x00102794
		internal StopBits StopBits
		{
			set
			{
				byte b;
				if (value == StopBits.One)
				{
					b = 0;
				}
				else if (value == StopBits.OnePointFive)
				{
					b = 1;
				}
				else
				{
					b = 2;
				}
				if (b != this.dcb.StopBits)
				{
					byte stopBits = this.dcb.StopBits;
					this.dcb.StopBits = b;
					if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
					{
						this.dcb.StopBits = stopBits;
						InternalResources.WinIOError();
					}
				}
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x00103800 File Offset: 0x00102800
		// (set) Token: 0x06003CB9 RID: 15545 RVA: 0x00103820 File Offset: 0x00102820
		public override int WriteTimeout
		{
			get
			{
				int writeTotalTimeoutConstant = this.commTimeouts.WriteTotalTimeoutConstant;
				if (writeTotalTimeoutConstant != 0)
				{
					return writeTotalTimeoutConstant;
				}
				return -1;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("WriteTimeout", SR.GetString("ArgumentOutOfRange_WriteTimeout"));
				}
				if (this._handle == null)
				{
					InternalResources.FileNotOpen();
				}
				int writeTotalTimeoutConstant = this.commTimeouts.WriteTotalTimeoutConstant;
				this.commTimeouts.WriteTotalTimeoutConstant = ((value == -1) ? 0 : value);
				if (!UnsafeNativeMethods.SetCommTimeouts(this._handle, ref this.commTimeouts))
				{
					this.commTimeouts.WriteTotalTimeoutConstant = writeTotalTimeoutConstant;
					InternalResources.WinIOError();
				}
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x0010389C File Offset: 0x0010289C
		internal bool CDHolding
		{
			get
			{
				int num = 0;
				if (!UnsafeNativeMethods.GetCommModemStatus(this._handle, ref num))
				{
					InternalResources.WinIOError();
				}
				return (128 & num) != 0;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06003CBB RID: 15547 RVA: 0x001038CC File Offset: 0x001028CC
		internal bool CtsHolding
		{
			get
			{
				int num = 0;
				if (!UnsafeNativeMethods.GetCommModemStatus(this._handle, ref num))
				{
					InternalResources.WinIOError();
				}
				return (16 & num) != 0;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06003CBC RID: 15548 RVA: 0x001038FC File Offset: 0x001028FC
		internal bool DsrHolding
		{
			get
			{
				int num = 0;
				if (!UnsafeNativeMethods.GetCommModemStatus(this._handle, ref num))
				{
					InternalResources.WinIOError();
				}
				return (32 & num) != 0;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06003CBD RID: 15549 RVA: 0x0010392C File Offset: 0x0010292C
		internal int BytesToRead
		{
			get
			{
				int num = 0;
				if (!UnsafeNativeMethods.ClearCommError(this._handle, ref num, ref this.comStat))
				{
					InternalResources.WinIOError();
				}
				return (int)this.comStat.cbInQue;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06003CBE RID: 15550 RVA: 0x00103960 File Offset: 0x00102960
		internal int BytesToWrite
		{
			get
			{
				int num = 0;
				if (!UnsafeNativeMethods.ClearCommError(this._handle, ref num, ref this.comStat))
				{
					InternalResources.WinIOError();
				}
				return (int)this.comStat.cbOutQue;
			}
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00103994 File Offset: 0x00102994
		internal SerialStream(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, int readTimeout, int writeTimeout, Handshake handshake, bool dtrEnable, bool rtsEnable, bool discardNull, byte parityReplace)
		{
			int num = 1073741824;
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				num = 128;
				this.isAsync = false;
			}
			if (portName == null || !portName.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(SR.GetString("Arg_InvalidSerialPort"), "portName");
			}
			SafeFileHandle safeFileHandle = UnsafeNativeMethods.CreateFile("\\\\.\\" + portName, -1073741824, 0, IntPtr.Zero, 3, num, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				InternalResources.WinIOError(portName);
			}
			try
			{
				int fileType = UnsafeNativeMethods.GetFileType(safeFileHandle);
				if (fileType != 2 && fileType != 0)
				{
					throw new ArgumentException(SR.GetString("Arg_InvalidSerialPort"), "portName");
				}
				this._handle = safeFileHandle;
				this.portName = portName;
				this.handshake = handshake;
				this.parityReplace = parityReplace;
				this.tempBuf = new byte[1];
				this.commProp = default(UnsafeNativeMethods.COMMPROP);
				int num2 = 0;
				if (!UnsafeNativeMethods.GetCommProperties(this._handle, ref this.commProp) || !UnsafeNativeMethods.GetCommModemStatus(this._handle, ref num2))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 87 || lastWin32Error == 6)
					{
						throw new ArgumentException(SR.GetString("Arg_InvalidSerialPortExtended"), "portName");
					}
					InternalResources.WinIOError(lastWin32Error, string.Empty);
				}
				if (this.commProp.dwMaxBaud != 0 && baudRate > this.commProp.dwMaxBaud)
				{
					throw new ArgumentOutOfRangeException("baudRate", SR.GetString("Max_Baud", new object[] { this.commProp.dwMaxBaud }));
				}
				this.comStat = default(UnsafeNativeMethods.COMSTAT);
				this.dcb = default(UnsafeNativeMethods.DCB);
				this.InitializeDCB(baudRate, parity, dataBits, stopBits, discardNull);
				this.DtrEnable = dtrEnable;
				this.rtsEnable = this.GetDcbFlag(12) == 1;
				if (handshake != Handshake.RequestToSend && handshake != Handshake.RequestToSendXOnXOff)
				{
					this.RtsEnable = rtsEnable;
				}
				if (readTimeout == 0)
				{
					this.commTimeouts.ReadTotalTimeoutConstant = 0;
					this.commTimeouts.ReadTotalTimeoutMultiplier = 0;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				else if (readTimeout == -1)
				{
					this.commTimeouts.ReadTotalTimeoutConstant = -2;
					this.commTimeouts.ReadTotalTimeoutMultiplier = -1;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				else
				{
					this.commTimeouts.ReadTotalTimeoutConstant = readTimeout;
					this.commTimeouts.ReadTotalTimeoutMultiplier = -1;
					this.commTimeouts.ReadIntervalTimeout = -1;
				}
				this.commTimeouts.WriteTotalTimeoutMultiplier = 0;
				this.commTimeouts.WriteTotalTimeoutConstant = ((writeTimeout == -1) ? 0 : writeTimeout);
				if (!UnsafeNativeMethods.SetCommTimeouts(this._handle, ref this.commTimeouts))
				{
					InternalResources.WinIOError();
				}
				if (this.isAsync && !ThreadPool.BindHandle(this._handle))
				{
					throw new IOException(SR.GetString("IO_BindHandleFailed"));
				}
				UnsafeNativeMethods.SetCommMask(this._handle, 507);
				this.eventRunner = new SerialStream.EventLoopRunner(this);
				new Thread(new ThreadStart(this.eventRunner.WaitForCommEvent))
				{
					IsBackground = true
				}.Start();
			}
			catch
			{
				safeFileHandle.Close();
				this._handle = null;
				throw;
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00103CC8 File Offset: 0x00102CC8
		~SerialStream()
		{
			this.Dispose(false);
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00103CF8 File Offset: 0x00102CF8
		protected override void Dispose(bool disposing)
		{
			if (this._handle != null && !this._handle.IsInvalid)
			{
				try
				{
					this.eventRunner.endEventLoop = true;
					Thread.MemoryBarrier();
					bool flag = false;
					UnsafeNativeMethods.SetCommMask(this._handle, 0);
					if (!UnsafeNativeMethods.EscapeCommFunction(this._handle, 6))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 5 && !disposing)
						{
							flag = true;
						}
						else
						{
							InternalResources.WinIOError();
						}
					}
					if (!flag && !this._handle.IsClosed)
					{
						this.Flush();
					}
					this.eventRunner.waitCommEventWaitHandle.Set();
					if (!flag)
					{
						this.DiscardInBuffer();
						this.DiscardOutBuffer();
					}
					if (disposing && this.eventRunner != null)
					{
						this.eventRunner.eventLoopEndedSignal.WaitOne();
						this.eventRunner.eventLoopEndedSignal.Close();
						this.eventRunner.waitCommEventWaitHandle.Close();
					}
				}
				finally
				{
					if (disposing)
					{
						lock (this)
						{
							this._handle.Close();
							this._handle = null;
							goto IL_010B;
						}
					}
					this._handle.Close();
					this._handle = null;
					IL_010B:
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00103E34 File Offset: 0x00102E34
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			int readTimeout = this.ReadTimeout;
			this.ReadTimeout = -1;
			IAsyncResult asyncResult;
			try
			{
				if (!this.isAsync)
				{
					asyncResult = base.BeginRead(array, offset, numBytes, userCallback, stateObject);
				}
				else
				{
					asyncResult = this.BeginReadCore(array, offset, numBytes, userCallback, stateObject);
				}
			}
			finally
			{
				this.ReadTimeout = readTimeout;
			}
			return asyncResult;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00103EF8 File Offset: 0x00102EF8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (this.inBreak)
			{
				throw new InvalidOperationException(SR.GetString("In_Break_State"));
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			int writeTimeout = this.WriteTimeout;
			this.WriteTimeout = -1;
			IAsyncResult asyncResult;
			try
			{
				if (!this.isAsync)
				{
					asyncResult = base.BeginWrite(array, offset, numBytes, userCallback, stateObject);
				}
				else
				{
					asyncResult = this.BeginWriteCore(array, offset, numBytes, userCallback, stateObject);
				}
			}
			finally
			{
				this.WriteTimeout = writeTimeout;
			}
			return asyncResult;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00103FD4 File Offset: 0x00102FD4
		internal void DiscardInBuffer()
		{
			if (!UnsafeNativeMethods.PurgeComm(this._handle, 10U))
			{
				InternalResources.WinIOError();
			}
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00103FEA File Offset: 0x00102FEA
		internal void DiscardOutBuffer()
		{
			if (!UnsafeNativeMethods.PurgeComm(this._handle, 5U))
			{
				InternalResources.WinIOError();
			}
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00104000 File Offset: 0x00103000
		public unsafe override int EndRead(IAsyncResult asyncResult)
		{
			if (!this.isAsync)
			{
				return base.EndRead(asyncResult);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = asyncResult as SerialStream.SerialStreamAsyncResult;
			if (serialStreamAsyncResult == null || serialStreamAsyncResult._isWrite)
			{
				InternalResources.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref serialStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				InternalResources.EndReadCalledTwice();
			}
			bool flag = false;
			WaitHandle waitHandle = serialStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
					if (serialStreamAsyncResult._numBytes == 0 && this.ReadTimeout == -1 && serialStreamAsyncResult._errorCode == 0)
					{
						flag = true;
					}
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = serialStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			serialStreamAsyncResult.UnpinBuffer();
			if (serialStreamAsyncResult._errorCode != 0)
			{
				InternalResources.WinIOError(serialStreamAsyncResult._errorCode, this.portName);
			}
			if (flag)
			{
				throw new IOException(SR.GetString("IO_OperationAborted"));
			}
			return serialStreamAsyncResult._numBytes;
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x001040E8 File Offset: 0x001030E8
		public unsafe override void EndWrite(IAsyncResult asyncResult)
		{
			if (!this.isAsync)
			{
				base.EndWrite(asyncResult);
				return;
			}
			if (this.inBreak)
			{
				throw new InvalidOperationException(SR.GetString("In_Break_State"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = asyncResult as SerialStream.SerialStreamAsyncResult;
			if (serialStreamAsyncResult == null || !serialStreamAsyncResult._isWrite)
			{
				InternalResources.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref serialStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				InternalResources.EndWriteCalledTwice();
			}
			WaitHandle waitHandle = serialStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = serialStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			serialStreamAsyncResult.UnpinBuffer();
			if (serialStreamAsyncResult._errorCode != 0)
			{
				InternalResources.WinIOError(serialStreamAsyncResult._errorCode, this.portName);
			}
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x001041B4 File Offset: 0x001031B4
		public override void Flush()
		{
			if (this._handle == null)
			{
				throw new ObjectDisposedException(SR.GetString("Port_not_open"));
			}
			UnsafeNativeMethods.FlushFileBuffers(this._handle);
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x001041DA File Offset: 0x001031DA
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			return this.Read(array, offset, count, this.ReadTimeout);
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x001041EC File Offset: 0x001031EC
		internal int Read([In] [Out] byte[] array, int offset, int count, int timeout)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (count == 0)
			{
				return 0;
			}
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			int num;
			if (this.isAsync)
			{
				IAsyncResult asyncResult = this.BeginReadCore(array, offset, count, null, null);
				num = this.EndRead(asyncResult);
			}
			else
			{
				int num2;
				num = this.ReadFileNative(array, offset, count, null, out num2);
				if (num == -1)
				{
					InternalResources.WinIOError();
				}
			}
			if (num == 0)
			{
				throw new TimeoutException();
			}
			return num;
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x001042AE File Offset: 0x001032AE
		public override int ReadByte()
		{
			return this.ReadByte(this.ReadTimeout);
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x001042BC File Offset: 0x001032BC
		internal int ReadByte(int timeout)
		{
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			int num;
			if (this.isAsync)
			{
				IAsyncResult asyncResult = this.BeginReadCore(this.tempBuf, 0, 1, null, null);
				num = this.EndRead(asyncResult);
			}
			else
			{
				int num2;
				num = this.ReadFileNative(this.tempBuf, 0, 1, null, out num2);
				if (num == -1)
				{
					InternalResources.WinIOError();
				}
			}
			if (num == 0)
			{
				throw new TimeoutException();
			}
			return (int)this.tempBuf[0];
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00104328 File Offset: 0x00103328
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x00104339 File Offset: 0x00103339
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0010434A File Offset: 0x0010334A
		internal void SetBufferSizes(int readBufferSize, int writeBufferSize)
		{
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			if (!UnsafeNativeMethods.SetupComm(this._handle, readBufferSize, writeBufferSize))
			{
				InternalResources.WinIOError();
			}
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0010436D File Offset: 0x0010336D
		public override void Write(byte[] array, int offset, int count)
		{
			this.Write(array, offset, count, this.WriteTimeout);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00104380 File Offset: 0x00103380
		internal void Write(byte[] array, int offset, int count, int timeout)
		{
			if (this.inBreak)
			{
				throw new InvalidOperationException(SR.GetString("In_Break_State"));
			}
			if (array == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Array"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (count == 0)
			{
				return;
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("count", SR.GetString("ArgumentOutOfRange_OffsetOut"));
			}
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			int num;
			if (this.isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(array, offset, count, null, null);
				this.EndWrite(asyncResult);
				SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = asyncResult as SerialStream.SerialStreamAsyncResult;
				num = serialStreamAsyncResult._numBytes;
			}
			else
			{
				int num2;
				num = this.WriteFileNative(array, offset, count, null, out num2);
				if (num == -1)
				{
					if (num2 == 1121)
					{
						throw new TimeoutException(SR.GetString("Write_timed_out"));
					}
					InternalResources.WinIOError();
				}
			}
			if (num == 0)
			{
				throw new TimeoutException(SR.GetString("Write_timed_out"));
			}
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0010448A File Offset: 0x0010348A
		public override void WriteByte(byte value)
		{
			this.WriteByte(value, this.WriteTimeout);
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0010449C File Offset: 0x0010349C
		internal void WriteByte(byte value, int timeout)
		{
			if (this.inBreak)
			{
				throw new InvalidOperationException(SR.GetString("In_Break_State"));
			}
			if (this._handle == null)
			{
				InternalResources.FileNotOpen();
			}
			this.tempBuf[0] = value;
			int num;
			if (this.isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(this.tempBuf, 0, 1, null, null);
				this.EndWrite(asyncResult);
				SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = asyncResult as SerialStream.SerialStreamAsyncResult;
				num = serialStreamAsyncResult._numBytes;
			}
			else
			{
				int num2;
				num = this.WriteFileNative(this.tempBuf, 0, 1, null, out num2);
				if (num == -1)
				{
					if (Marshal.GetLastWin32Error() == 1121)
					{
						throw new TimeoutException(SR.GetString("Write_timed_out"));
					}
					InternalResources.WinIOError();
				}
			}
			if (num == 0)
			{
				throw new TimeoutException(SR.GetString("Write_timed_out"));
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00104554 File Offset: 0x00103554
		private void InitializeDCB(int baudRate, Parity parity, int dataBits, StopBits stopBits, bool discardNull)
		{
			if (!UnsafeNativeMethods.GetCommState(this._handle, ref this.dcb))
			{
				InternalResources.WinIOError();
			}
			this.dcb.DCBlength = (uint)Marshal.SizeOf(this.dcb);
			this.dcb.BaudRate = (uint)baudRate;
			this.dcb.ByteSize = (byte)dataBits;
			switch (stopBits)
			{
			case StopBits.One:
				this.dcb.StopBits = 0;
				break;
			case StopBits.Two:
				this.dcb.StopBits = 2;
				break;
			case StopBits.OnePointFive:
				this.dcb.StopBits = 1;
				break;
			}
			this.dcb.Parity = (byte)parity;
			this.SetDcbFlag(1, (parity == Parity.None) ? 0 : 1);
			this.SetDcbFlag(0, 1);
			this.SetDcbFlag(2, (this.handshake == Handshake.RequestToSend || this.handshake == Handshake.RequestToSendXOnXOff) ? 1 : 0);
			this.SetDcbFlag(3, 0);
			this.SetDcbFlag(4, 0);
			this.SetDcbFlag(6, 0);
			this.SetDcbFlag(9, (this.handshake == Handshake.XOnXOff || this.handshake == Handshake.RequestToSendXOnXOff) ? 1 : 0);
			this.SetDcbFlag(8, (this.handshake == Handshake.XOnXOff || this.handshake == Handshake.RequestToSendXOnXOff) ? 1 : 0);
			if (parity != Parity.None)
			{
				this.SetDcbFlag(10, (this.parityReplace != 0) ? 1 : 0);
				this.dcb.ErrorChar = this.parityReplace;
			}
			else
			{
				this.SetDcbFlag(10, 0);
				this.dcb.ErrorChar = 0;
			}
			this.SetDcbFlag(11, discardNull ? 1 : 0);
			if (this.handshake == Handshake.RequestToSend || this.handshake == Handshake.RequestToSendXOnXOff)
			{
				this.SetDcbFlag(12, 2);
			}
			else if (this.GetDcbFlag(12) == 2)
			{
				this.SetDcbFlag(12, 0);
			}
			this.dcb.XonChar = 17;
			this.dcb.XoffChar = 19;
			this.dcb.XonLim = (this.dcb.XoffLim = (ushort)(this.commProp.dwCurrentRxQueue / 4));
			this.dcb.EofChar = 26;
			this.dcb.EvtChar = 26;
			if (!UnsafeNativeMethods.SetCommState(this._handle, ref this.dcb))
			{
				InternalResources.WinIOError();
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00104774 File Offset: 0x00103774
		internal int GetDcbFlag(int whichFlag)
		{
			uint num;
			if (whichFlag == 4 || whichFlag == 12)
			{
				num = 3U;
			}
			else if (whichFlag == 15)
			{
				num = 131071U;
			}
			else
			{
				num = 1U;
			}
			uint num2 = this.dcb.Flags & (num << whichFlag);
			return (int)(num2 >> whichFlag);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x001047B8 File Offset: 0x001037B8
		internal void SetDcbFlag(int whichFlag, int setting)
		{
			setting <<= whichFlag;
			uint num;
			if (whichFlag == 4 || whichFlag == 12)
			{
				num = 3U;
			}
			else if (whichFlag == 15)
			{
				num = 131071U;
			}
			else
			{
				num = 1U;
			}
			this.dcb.Flags = this.dcb.Flags & ~(num << whichFlag);
			this.dcb.Flags = this.dcb.Flags | (uint)setting;
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00104818 File Offset: 0x00103818
		private unsafe SerialStream.SerialStreamAsyncResult BeginReadCore(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = new SerialStream.SerialStreamAsyncResult();
			serialStreamAsyncResult._userCallback = userCallback;
			serialStreamAsyncResult._userStateObject = stateObject;
			serialStreamAsyncResult._isWrite = false;
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			serialStreamAsyncResult._waitHandle = manualResetEvent;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, serialStreamAsyncResult);
			NativeOverlapped* ptr = overlapped.Pack(SerialStream.IOCallback, null);
			serialStreamAsyncResult._overlapped = ptr;
			serialStreamAsyncResult.PinBuffer(array);
			int num = 0;
			int num2 = this.ReadFileNative(array, offset, numBytes, ptr, out num);
			if (num2 == -1 && num != 997)
			{
				if (num == 38)
				{
					InternalResources.EndOfFile();
				}
				else
				{
					InternalResources.WinIOError(num, string.Empty);
				}
			}
			return serialStreamAsyncResult;
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x001048B4 File Offset: 0x001038B4
		private unsafe SerialStream.SerialStreamAsyncResult BeginWriteCore(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = new SerialStream.SerialStreamAsyncResult();
			serialStreamAsyncResult._userCallback = userCallback;
			serialStreamAsyncResult._userStateObject = stateObject;
			serialStreamAsyncResult._isWrite = true;
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			serialStreamAsyncResult._waitHandle = manualResetEvent;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, serialStreamAsyncResult);
			NativeOverlapped* ptr = overlapped.Pack(SerialStream.IOCallback, null);
			serialStreamAsyncResult._overlapped = ptr;
			serialStreamAsyncResult.PinBuffer(array);
			int num = 0;
			int num2 = this.WriteFileNative(array, offset, numBytes, ptr, out num);
			if (num2 == -1 && num != 997)
			{
				if (num == 38)
				{
					InternalResources.EndOfFile();
				}
				else
				{
					InternalResources.WinIOError(num, string.Empty);
				}
			}
			return serialStreamAsyncResult;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00104950 File Offset: 0x00103950
		private unsafe int ReadFileNative(byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(SR.GetString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int num = 0;
			int num2;
			fixed (byte* ptr = bytes)
			{
				if (this.isAsync)
				{
					num2 = UnsafeNativeMethods.ReadFile(this._handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num2 = UnsafeNativeMethods.ReadFile(this._handle, ptr + offset, count, out num, IntPtr.Zero);
				}
			}
			if (num2 == 0)
			{
				hr = Marshal.GetLastWin32Error();
				if (hr == 6)
				{
					this._handle.SetHandleAsInvalid();
				}
				return -1;
			}
			hr = 0;
			return num;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x001049FC File Offset: 0x001039FC
		private unsafe int WriteFileNative(byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(SR.GetString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int num = 0;
			int num2;
			fixed (byte* ptr = bytes)
			{
				if (this.isAsync)
				{
					num2 = UnsafeNativeMethods.WriteFile(this._handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num2 = UnsafeNativeMethods.WriteFile(this._handle, ptr + offset, count, out num, IntPtr.Zero);
				}
			}
			if (num2 == 0)
			{
				hr = Marshal.GetLastWin32Error();
				if (hr == 6)
				{
					this._handle.SetHandleAsInvalid();
				}
				return -1;
			}
			hr = 0;
			return num;
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00104AA8 File Offset: 0x00103AA8
		private unsafe static void AsyncFSCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(pOverlapped);
			SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = (SerialStream.SerialStreamAsyncResult)overlapped.AsyncResult;
			serialStreamAsyncResult._numBytes = (int)numBytes;
			serialStreamAsyncResult._errorCode = (int)errorCode;
			serialStreamAsyncResult._completedSynchronously = false;
			serialStreamAsyncResult._isComplete = true;
			ManualResetEvent waitHandle = serialStreamAsyncResult._waitHandle;
			if (waitHandle != null && !waitHandle.Set())
			{
				InternalResources.WinIOError();
			}
			AsyncCallback userCallback = serialStreamAsyncResult._userCallback;
			if (userCallback != null)
			{
				userCallback(serialStreamAsyncResult);
			}
		}

		// Token: 0x0400355C RID: 13660
		private const int errorEvents = 271;

		// Token: 0x0400355D RID: 13661
		private const int receivedEvents = 3;

		// Token: 0x0400355E RID: 13662
		private const int pinChangedEvents = 376;

		// Token: 0x0400355F RID: 13663
		private const int infiniteTimeoutConst = -2;

		// Token: 0x04003560 RID: 13664
		private const int maxDataBits = 8;

		// Token: 0x04003561 RID: 13665
		private const int minDataBits = 5;

		// Token: 0x04003562 RID: 13666
		private string portName;

		// Token: 0x04003563 RID: 13667
		private byte parityReplace = 63;

		// Token: 0x04003564 RID: 13668
		private bool inBreak;

		// Token: 0x04003565 RID: 13669
		private bool isAsync = true;

		// Token: 0x04003566 RID: 13670
		private Handshake handshake;

		// Token: 0x04003567 RID: 13671
		private bool rtsEnable;

		// Token: 0x04003568 RID: 13672
		private UnsafeNativeMethods.DCB dcb;

		// Token: 0x04003569 RID: 13673
		private UnsafeNativeMethods.COMMTIMEOUTS commTimeouts;

		// Token: 0x0400356A RID: 13674
		private UnsafeNativeMethods.COMSTAT comStat;

		// Token: 0x0400356B RID: 13675
		private UnsafeNativeMethods.COMMPROP commProp;

		// Token: 0x0400356C RID: 13676
		internal SafeFileHandle _handle;

		// Token: 0x0400356D RID: 13677
		internal SerialStream.EventLoopRunner eventRunner;

		// Token: 0x0400356E RID: 13678
		private byte[] tempBuf;

		// Token: 0x0400356F RID: 13679
		private static readonly IOCompletionCallback IOCallback = new IOCompletionCallback(SerialStream.AsyncFSCallback);

		// Token: 0x020007B6 RID: 1974
		internal sealed class EventLoopRunner
		{
			// Token: 0x06003CDD RID: 15581 RVA: 0x00104B24 File Offset: 0x00103B24
			internal EventLoopRunner(SerialStream stream)
			{
				this.handle = stream._handle;
				this.streamWeakReference = new WeakReference(stream);
				this.callErrorEvents = new WaitCallback(this.CallErrorEvents);
				this.callReceiveEvents = new WaitCallback(this.CallReceiveEvents);
				this.callPinEvents = new WaitCallback(this.CallPinEvents);
				this.freeNativeOverlappedCallback = new IOCompletionCallback(this.FreeNativeOverlappedCallback);
				this.isAsync = stream.isAsync;
			}

			// Token: 0x17000E4D RID: 3661
			// (get) Token: 0x06003CDE RID: 15582 RVA: 0x00104BBB File Offset: 0x00103BBB
			internal bool ShutdownLoop
			{
				get
				{
					return this.endEventLoop;
				}
			}

			// Token: 0x06003CDF RID: 15583 RVA: 0x00104BC4 File Offset: 0x00103BC4
			internal unsafe void WaitForCommEvent()
			{
				int num = 0;
				bool flag = false;
				NativeOverlapped* ptr = null;
				while (!this.ShutdownLoop)
				{
					SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = null;
					if (this.isAsync)
					{
						serialStreamAsyncResult = new SerialStream.SerialStreamAsyncResult();
						serialStreamAsyncResult._userCallback = null;
						serialStreamAsyncResult._userStateObject = null;
						serialStreamAsyncResult._isWrite = false;
						serialStreamAsyncResult._numBytes = 2;
						serialStreamAsyncResult._waitHandle = this.waitCommEventWaitHandle;
						this.waitCommEventWaitHandle.Reset();
						Overlapped overlapped = new Overlapped(0, 0, this.waitCommEventWaitHandle.SafeWaitHandle.DangerousGetHandle(), serialStreamAsyncResult);
						ptr = overlapped.Pack(this.freeNativeOverlappedCallback, null);
					}
					try
					{
						fixed (int* ptr2 = &this.eventsOccurred)
						{
							if (!UnsafeNativeMethods.WaitCommEvent(this.handle, ptr2, ptr))
							{
								int lastWin32Error = Marshal.GetLastWin32Error();
								if (lastWin32Error == 5)
								{
									flag = true;
									break;
								}
								if (lastWin32Error == 997)
								{
									bool flag2 = this.waitCommEventWaitHandle.WaitOne();
									int lastWin32Error2;
									do
									{
										flag2 = UnsafeNativeMethods.GetOverlappedResult(this.handle, ptr, ref num, false);
										lastWin32Error2 = Marshal.GetLastWin32Error();
									}
									while (lastWin32Error2 == 996 && !this.ShutdownLoop && !flag2);
									if (!flag2 && (lastWin32Error2 == 996 || lastWin32Error2 == 87) && !this.ShutdownLoop)
									{
									}
								}
							}
						}
					}
					finally
					{
						int* ptr2 = null;
					}
					if (!this.ShutdownLoop)
					{
						this.CallEvents(this.eventsOccurred);
					}
					if (this.isAsync && Interlocked.Decrement(ref serialStreamAsyncResult._numBytes) == 0)
					{
						Overlapped.Free(ptr);
					}
				}
				if (flag)
				{
					this.endEventLoop = true;
					Overlapped.Free(ptr);
				}
				this.eventLoopEndedSignal.Set();
			}

			// Token: 0x06003CE0 RID: 15584 RVA: 0x00104D4C File Offset: 0x00103D4C
			private unsafe void FreeNativeOverlappedCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(pOverlapped);
				SerialStream.SerialStreamAsyncResult serialStreamAsyncResult = (SerialStream.SerialStreamAsyncResult)overlapped.AsyncResult;
				if (Interlocked.Decrement(ref serialStreamAsyncResult._numBytes) == 0)
				{
					Overlapped.Free(pOverlapped);
				}
			}

			// Token: 0x06003CE1 RID: 15585 RVA: 0x00104D80 File Offset: 0x00103D80
			private void CallEvents(int nativeEvents)
			{
				if ((nativeEvents & 128) != 0)
				{
					int num = 0;
					if (!UnsafeNativeMethods.ClearCommError(this.handle, ref num, IntPtr.Zero))
					{
						InternalResources.WinIOError();
					}
					num &= 271;
					if (num != 0)
					{
						ThreadPool.QueueUserWorkItem(this.callErrorEvents, num);
					}
				}
				if ((nativeEvents & 376) != 0)
				{
					ThreadPool.QueueUserWorkItem(this.callPinEvents, nativeEvents);
				}
				if ((nativeEvents & 3) != 0)
				{
					ThreadPool.QueueUserWorkItem(this.callReceiveEvents, nativeEvents);
				}
			}

			// Token: 0x06003CE2 RID: 15586 RVA: 0x00104E00 File Offset: 0x00103E00
			private void CallErrorEvents(object state)
			{
				int num = (int)state;
				SerialStream serialStream = (SerialStream)this.streamWeakReference.Target;
				if (serialStream == null)
				{
					return;
				}
				if (serialStream.ErrorReceived != null)
				{
					if ((num & 256) != 0)
					{
						serialStream.ErrorReceived(serialStream, new SerialErrorReceivedEventArgs(SerialError.TXFull));
					}
					if ((num & 1) != 0)
					{
						serialStream.ErrorReceived(serialStream, new SerialErrorReceivedEventArgs(SerialError.RXOver));
					}
					if ((num & 2) != 0)
					{
						serialStream.ErrorReceived(serialStream, new SerialErrorReceivedEventArgs(SerialError.Overrun));
					}
					if ((num & 4) != 0)
					{
						serialStream.ErrorReceived(serialStream, new SerialErrorReceivedEventArgs(SerialError.RXParity));
					}
					if ((num & 8) != 0)
					{
						serialStream.ErrorReceived(serialStream, new SerialErrorReceivedEventArgs(SerialError.Frame));
					}
				}
			}

			// Token: 0x06003CE3 RID: 15587 RVA: 0x00104EB0 File Offset: 0x00103EB0
			private void CallReceiveEvents(object state)
			{
				int num = (int)state;
				SerialStream serialStream = (SerialStream)this.streamWeakReference.Target;
				if (serialStream == null)
				{
					return;
				}
				if (serialStream.DataReceived != null)
				{
					if ((num & 1) != 0)
					{
						serialStream.DataReceived(serialStream, new SerialDataReceivedEventArgs(SerialData.Chars));
					}
					if ((num & 2) != 0)
					{
						serialStream.DataReceived(serialStream, new SerialDataReceivedEventArgs(SerialData.Eof));
					}
				}
			}

			// Token: 0x06003CE4 RID: 15588 RVA: 0x00104F14 File Offset: 0x00103F14
			private void CallPinEvents(object state)
			{
				int num = (int)state;
				SerialStream serialStream = (SerialStream)this.streamWeakReference.Target;
				if (serialStream == null)
				{
					return;
				}
				if (serialStream.PinChanged != null)
				{
					if ((num & 8) != 0)
					{
						serialStream.PinChanged(serialStream, new SerialPinChangedEventArgs(SerialPinChange.CtsChanged));
					}
					if ((num & 16) != 0)
					{
						serialStream.PinChanged(serialStream, new SerialPinChangedEventArgs(SerialPinChange.DsrChanged));
					}
					if ((num & 32) != 0)
					{
						serialStream.PinChanged(serialStream, new SerialPinChangedEventArgs(SerialPinChange.CDChanged));
					}
					if ((num & 256) != 0)
					{
						serialStream.PinChanged(serialStream, new SerialPinChangedEventArgs(SerialPinChange.Ring));
					}
					if ((num & 64) != 0)
					{
						serialStream.PinChanged(serialStream, new SerialPinChangedEventArgs(SerialPinChange.Break));
					}
				}
			}

			// Token: 0x04003573 RID: 13683
			private WeakReference streamWeakReference;

			// Token: 0x04003574 RID: 13684
			internal ManualResetEvent eventLoopEndedSignal = new ManualResetEvent(false);

			// Token: 0x04003575 RID: 13685
			internal ManualResetEvent waitCommEventWaitHandle = new ManualResetEvent(false);

			// Token: 0x04003576 RID: 13686
			private SafeFileHandle handle;

			// Token: 0x04003577 RID: 13687
			private bool isAsync;

			// Token: 0x04003578 RID: 13688
			internal bool endEventLoop;

			// Token: 0x04003579 RID: 13689
			private int eventsOccurred;

			// Token: 0x0400357A RID: 13690
			private WaitCallback callErrorEvents;

			// Token: 0x0400357B RID: 13691
			private WaitCallback callReceiveEvents;

			// Token: 0x0400357C RID: 13692
			private WaitCallback callPinEvents;

			// Token: 0x0400357D RID: 13693
			private IOCompletionCallback freeNativeOverlappedCallback;
		}

		// Token: 0x020007B7 RID: 1975
		internal sealed class SerialStreamAsyncResult : IAsyncResult
		{
			// Token: 0x17000E4E RID: 3662
			// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00104FCB File Offset: 0x00103FCB
			public object AsyncState
			{
				get
				{
					return this._userStateObject;
				}
			}

			// Token: 0x17000E4F RID: 3663
			// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x00104FD3 File Offset: 0x00103FD3
			public bool IsCompleted
			{
				get
				{
					return this._isComplete;
				}
			}

			// Token: 0x17000E50 RID: 3664
			// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x00104FDB File Offset: 0x00103FDB
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return this._waitHandle;
				}
			}

			// Token: 0x17000E51 RID: 3665
			// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x00104FE3 File Offset: 0x00103FE3
			public bool CompletedSynchronously
			{
				get
				{
					return this._completedSynchronously;
				}
			}

			// Token: 0x06003CE9 RID: 15593 RVA: 0x00104FEB File Offset: 0x00103FEB
			internal void PinBuffer(byte[] buffer)
			{
				this._bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				this._bufferIsPinned = true;
			}

			// Token: 0x06003CEA RID: 15594 RVA: 0x00105001 File Offset: 0x00104001
			internal void UnpinBuffer()
			{
				if (this._bufferIsPinned)
				{
					this._bufferHandle.Free();
					this._bufferIsPinned = false;
				}
			}

			// Token: 0x0400357E RID: 13694
			internal AsyncCallback _userCallback;

			// Token: 0x0400357F RID: 13695
			internal object _userStateObject;

			// Token: 0x04003580 RID: 13696
			internal GCHandle _bufferHandle;

			// Token: 0x04003581 RID: 13697
			internal bool _isWrite;

			// Token: 0x04003582 RID: 13698
			internal bool _isComplete;

			// Token: 0x04003583 RID: 13699
			internal bool _completedSynchronously;

			// Token: 0x04003584 RID: 13700
			internal bool _bufferIsPinned;

			// Token: 0x04003585 RID: 13701
			internal ManualResetEvent _waitHandle;

			// Token: 0x04003586 RID: 13702
			internal int _EndXxxCalled;

			// Token: 0x04003587 RID: 13703
			internal int _numBytes;

			// Token: 0x04003588 RID: 13704
			internal int _errorCode;

			// Token: 0x04003589 RID: 13705
			internal unsafe NativeOverlapped* _overlapped;
		}
	}
}

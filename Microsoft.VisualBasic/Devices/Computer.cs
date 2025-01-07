using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic.MyServices;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Computer : ServerComputer
	{
		public Audio Audio
		{
			get
			{
				if (this.m_Audio != null)
				{
					return this.m_Audio;
				}
				this.m_Audio = new Audio();
				return this.m_Audio;
			}
		}

		public ClipboardProxy Clipboard
		{
			get
			{
				if (Computer.m_Clipboard == null)
				{
					Computer.m_Clipboard = new ClipboardProxy();
				}
				return Computer.m_Clipboard;
			}
		}

		public Ports Ports
		{
			get
			{
				if (this.m_Ports == null)
				{
					this.m_Ports = new Ports();
				}
				return this.m_Ports;
			}
		}

		public Mouse Mouse
		{
			get
			{
				if (Computer.m_Mouse != null)
				{
					return Computer.m_Mouse;
				}
				Computer.m_Mouse = new Mouse();
				return Computer.m_Mouse;
			}
		}

		public Keyboard Keyboard
		{
			get
			{
				if (Computer.m_KeyboardInstance != null)
				{
					return Computer.m_KeyboardInstance;
				}
				Computer.m_KeyboardInstance = new Keyboard();
				return Computer.m_KeyboardInstance;
			}
		}

		public Screen Screen
		{
			get
			{
				return Screen.PrimaryScreen;
			}
		}

		private Audio m_Audio;

		private Ports m_Ports;

		private static ClipboardProxy m_Clipboard;

		private static Mouse m_Mouse;

		private static Keyboard m_KeyboardInstance;
	}
}

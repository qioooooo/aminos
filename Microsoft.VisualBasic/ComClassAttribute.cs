using System;

namespace Microsoft.VisualBasic
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class ComClassAttribute : Attribute
	{
		public ComClassAttribute()
		{
			this.m_InterfaceShadows = false;
		}

		public ComClassAttribute(string _ClassID)
		{
			this.m_InterfaceShadows = false;
			this.m_ClassID = _ClassID;
		}

		public ComClassAttribute(string _ClassID, string _InterfaceID)
		{
			this.m_InterfaceShadows = false;
			this.m_ClassID = _ClassID;
			this.m_InterfaceID = _InterfaceID;
		}

		public ComClassAttribute(string _ClassID, string _InterfaceID, string _EventId)
		{
			this.m_InterfaceShadows = false;
			this.m_ClassID = _ClassID;
			this.m_InterfaceID = _InterfaceID;
			this.m_EventID = _EventId;
		}

		public string ClassID
		{
			get
			{
				return this.m_ClassID;
			}
		}

		public string InterfaceID
		{
			get
			{
				return this.m_InterfaceID;
			}
		}

		public string EventID
		{
			get
			{
				return this.m_EventID;
			}
		}

		public bool InterfaceShadows
		{
			get
			{
				return this.m_InterfaceShadows;
			}
			set
			{
				this.m_InterfaceShadows = value;
			}
		}

		private string m_ClassID;

		private string m_InterfaceID;

		private string m_EventID;

		private bool m_InterfaceShadows;
	}
}

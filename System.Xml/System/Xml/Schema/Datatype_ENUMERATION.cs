﻿using System;

namespace System.Xml.Schema
{
	internal class Datatype_ENUMERATION : Datatype_NMTOKEN
	{
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ENUMERATION;
			}
		}
	}
}
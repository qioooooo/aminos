﻿using System;

namespace System.Data.Design
{
	public class TypedDataSetSchemaImporterExtensionFx35 : TypedDataSetSchemaImporterExtension
	{
		public TypedDataSetSchemaImporterExtensionFx35()
			: base(TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets)
		{
		}
	}
}

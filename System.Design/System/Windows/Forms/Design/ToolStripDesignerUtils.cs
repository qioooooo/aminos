using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	internal sealed class ToolStripDesignerUtils
	{
		private ToolStripDesignerUtils()
		{
		}

		public static void GetAdjustedBounds(ToolStripItem item, ref Rectangle r)
		{
			if (!(item is ToolStripControlHost) || !item.IsOnDropDown)
			{
				if (item is ToolStripMenuItem && item.IsOnDropDown)
				{
					r.Inflate(-3, -2);
					r.Width++;
					return;
				}
				if (item is ToolStripControlHost && !item.IsOnDropDown)
				{
					r.Inflate(0, -2);
					return;
				}
				if (item is ToolStripMenuItem && !item.IsOnDropDown)
				{
					r.Inflate(-3, -3);
					return;
				}
				r.Inflate(-1, -1);
			}
		}

		private static ToolStrip GetToolStripFromComponent(IComponent component)
		{
			ToolStripItem toolStripItem = component as ToolStripItem;
			ToolStrip toolStrip;
			if (toolStripItem != null)
			{
				if (!(toolStripItem is ToolStripDropDownItem))
				{
					toolStrip = toolStripItem.Owner;
				}
				else
				{
					toolStrip = ((ToolStripDropDownItem)toolStripItem).DropDown;
				}
			}
			else
			{
				toolStrip = component as ToolStrip;
			}
			return toolStrip;
		}

		private static ToolboxItem GetCachedToolboxItem(Type itemType)
		{
			ToolboxItem toolboxItem = null;
			if (ToolStripDesignerUtils.CachedToolboxItems == null)
			{
				ToolStripDesignerUtils.CachedToolboxItems = new Dictionary<Type, ToolboxItem>();
			}
			else if (ToolStripDesignerUtils.CachedToolboxItems.ContainsKey(itemType))
			{
				return ToolStripDesignerUtils.CachedToolboxItems[itemType];
			}
			if (toolboxItem == null)
			{
				toolboxItem = ToolboxService.GetToolboxItem(itemType);
				if (toolboxItem == null)
				{
					toolboxItem = new ToolboxItem(itemType);
				}
			}
			ToolStripDesignerUtils.CachedToolboxItems[itemType] = toolboxItem;
			if (ToolStripDesignerUtils.CustomToolStripItemCount > 0 && ToolStripDesignerUtils.CustomToolStripItemCount * 2 < ToolStripDesignerUtils.CachedToolboxItems.Count)
			{
				ToolStripDesignerUtils.CachedToolboxItems.Clear();
			}
			return toolboxItem;
		}

		private static Bitmap GetKnownToolboxBitmap(Type itemType)
		{
			if (ToolStripDesignerUtils.CachedWinformsImages == null)
			{
				ToolStripDesignerUtils.CachedWinformsImages = new Dictionary<Type, Bitmap>();
			}
			if (!ToolStripDesignerUtils.CachedWinformsImages.ContainsKey(itemType))
			{
				Bitmap bitmap = ToolboxBitmapAttribute.GetImageFromResource(itemType, null, false) as Bitmap;
				ToolStripDesignerUtils.CachedWinformsImages[itemType] = bitmap;
				return bitmap;
			}
			return ToolStripDesignerUtils.CachedWinformsImages[itemType];
		}

		public static Bitmap GetToolboxBitmap(Type itemType)
		{
			if (itemType.Namespace == ToolStripDesignerUtils.systemWindowsFormsNamespace)
			{
				return ToolStripDesignerUtils.GetKnownToolboxBitmap(itemType);
			}
			ToolboxItem cachedToolboxItem = ToolStripDesignerUtils.GetCachedToolboxItem(itemType);
			if (cachedToolboxItem != null)
			{
				return cachedToolboxItem.Bitmap;
			}
			return ToolStripDesignerUtils.GetKnownToolboxBitmap(typeof(Component));
		}

		public static string GetToolboxDescription(Type itemType)
		{
			string text = null;
			ToolboxItem cachedToolboxItem = ToolStripDesignerUtils.GetCachedToolboxItem(itemType);
			if (cachedToolboxItem != null)
			{
				text = cachedToolboxItem.DisplayName;
			}
			if (text == null)
			{
				text = itemType.Name;
			}
			if (text.StartsWith("ToolStrip"))
			{
				return text.Substring(9);
			}
			return text;
		}

		public static Type[] GetStandardItemTypes(IComponent component)
		{
			ToolStrip toolStripFromComponent = ToolStripDesignerUtils.GetToolStripFromComponent(component);
			if (toolStripFromComponent is MenuStrip)
			{
				return ToolStripDesignerUtils.NewItemTypesForMenuStrip;
			}
			if (toolStripFromComponent is ToolStripDropDownMenu)
			{
				return ToolStripDesignerUtils.NewItemTypesForToolStripDropDownMenu;
			}
			if (toolStripFromComponent is StatusStrip)
			{
				return ToolStripDesignerUtils.NewItemTypesForStatusStrip;
			}
			return ToolStripDesignerUtils.NewItemTypesForToolStrip;
		}

		private static ToolStripItemDesignerAvailability GetDesignerVisibility(ToolStrip toolStrip)
		{
			ToolStripItemDesignerAvailability toolStripItemDesignerAvailability;
			if (toolStrip is StatusStrip)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.StatusStrip;
			}
			else if (toolStrip is MenuStrip)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.MenuStrip;
			}
			else if (toolStrip is ToolStripDropDownMenu)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.ContextMenuStrip;
			}
			else
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.ToolStrip;
			}
			return toolStripItemDesignerAvailability;
		}

		public static Type[] GetCustomItemTypes(IComponent component, IServiceProvider serviceProvider)
		{
			ITypeDiscoveryService typeDiscoveryService = null;
			if (serviceProvider != null)
			{
				typeDiscoveryService = serviceProvider.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
			}
			return ToolStripDesignerUtils.GetCustomItemTypes(component, typeDiscoveryService);
		}

		public static Type[] GetCustomItemTypes(IComponent component, ITypeDiscoveryService discoveryService)
		{
			if (discoveryService != null)
			{
				ICollection types = discoveryService.GetTypes(ToolStripDesignerUtils.toolStripItemType, false);
				ToolStrip toolStripFromComponent = ToolStripDesignerUtils.GetToolStripFromComponent(component);
				ToolStripItemDesignerAvailability designerVisibility = ToolStripDesignerUtils.GetDesignerVisibility(toolStripFromComponent);
				Type[] standardItemTypes = ToolStripDesignerUtils.GetStandardItemTypes(component);
				if (designerVisibility != ToolStripItemDesignerAvailability.None)
				{
					ArrayList arrayList = new ArrayList(types.Count);
					foreach (object obj in types)
					{
						Type type = (Type)obj;
						if (!type.IsAbstract && (type.IsPublic || type.IsNestedPublic) && !type.ContainsGenericParameters)
						{
							ConstructorInfo constructor = type.GetConstructor(new Type[0]);
							if (constructor != null)
							{
								ToolStripItemDesignerAvailabilityAttribute toolStripItemDesignerAvailabilityAttribute = (ToolStripItemDesignerAvailabilityAttribute)TypeDescriptor.GetAttributes(type)[typeof(ToolStripItemDesignerAvailabilityAttribute)];
								if (toolStripItemDesignerAvailabilityAttribute != null && (toolStripItemDesignerAvailabilityAttribute.ItemAdditionVisibility & designerVisibility) == designerVisibility)
								{
									bool flag = false;
									foreach (Type type2 in standardItemTypes)
									{
										if (type2 == type)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										arrayList.Add(type);
									}
								}
							}
						}
					}
					if (arrayList.Count > 0)
					{
						Type[] array2 = new Type[arrayList.Count];
						arrayList.CopyTo(array2, 0);
						ToolStripDesignerUtils.CustomToolStripItemCount = arrayList.Count;
						return array2;
					}
				}
			}
			ToolStripDesignerUtils.CustomToolStripItemCount = 0;
			return new Type[0];
		}

		public static ToolStripItem[] GetStandardItemMenuItems(IComponent component, EventHandler onClick, bool convertTo)
		{
			Type[] standardItemTypes = ToolStripDesignerUtils.GetStandardItemTypes(component);
			ToolStripItem[] array = new ToolStripItem[standardItemTypes.Length];
			for (int i = 0; i < standardItemTypes.Length; i++)
			{
				ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = new ItemTypeToolStripMenuItem(standardItemTypes[i]);
				itemTypeToolStripMenuItem.ConvertTo = convertTo;
				if (onClick != null)
				{
					itemTypeToolStripMenuItem.Click += onClick;
				}
				array[i] = itemTypeToolStripMenuItem;
			}
			return array;
		}

		public static ToolStripItem[] GetCustomItemMenuItems(IComponent component, EventHandler onClick, bool convertTo, IServiceProvider serviceProvider)
		{
			Type[] customItemTypes = ToolStripDesignerUtils.GetCustomItemTypes(component, serviceProvider);
			ToolStripItem[] array = new ToolStripItem[customItemTypes.Length];
			for (int i = 0; i < customItemTypes.Length; i++)
			{
				ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = new ItemTypeToolStripMenuItem(customItemTypes[i]);
				itemTypeToolStripMenuItem.ConvertTo = convertTo;
				if (onClick != null)
				{
					itemTypeToolStripMenuItem.Click += onClick;
				}
				array[i] = itemTypeToolStripMenuItem;
			}
			return array;
		}

		public static ToolStripDropDown GetNewItemDropDown(IComponent component, ToolStripItem currentItem, EventHandler onClick, bool convertTo, IServiceProvider serviceProvider)
		{
			NewItemsContextMenuStrip newItemsContextMenuStrip = new NewItemsContextMenuStrip(component, currentItem, onClick, convertTo, serviceProvider);
			newItemsContextMenuStrip.GroupOrdering.Add("StandardList");
			newItemsContextMenuStrip.GroupOrdering.Add("CustomList");
			foreach (ToolStripItem toolStripItem in ToolStripDesignerUtils.GetStandardItemMenuItems(component, onClick, convertTo))
			{
				newItemsContextMenuStrip.Groups["StandardList"].Items.Add(toolStripItem);
				if (convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = toolStripItem as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem != null && currentItem != null && itemTypeToolStripMenuItem.ItemType == currentItem.GetType())
					{
						itemTypeToolStripMenuItem.Enabled = false;
					}
				}
			}
			foreach (ToolStripItem toolStripItem2 in ToolStripDesignerUtils.GetCustomItemMenuItems(component, onClick, convertTo, serviceProvider))
			{
				newItemsContextMenuStrip.Groups["CustomList"].Items.Add(toolStripItem2);
				if (convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem2 = toolStripItem2 as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem2 != null && currentItem != null && itemTypeToolStripMenuItem2.ItemType == currentItem.GetType())
					{
						itemTypeToolStripMenuItem2.Enabled = false;
					}
				}
			}
			IUIService iuiservice = serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				newItemsContextMenuStrip.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
				newItemsContextMenuStrip.Font = (Font)iuiservice.Styles["DialogFont"];
			}
			newItemsContextMenuStrip.Populate();
			return newItemsContextMenuStrip;
		}

		public static void InvalidateSelection(ArrayList originalSelComps, ToolStripItem nextSelection, IServiceProvider provider, bool shiftPressed)
		{
			if (nextSelection == null || provider == null)
			{
				return;
			}
			Region region = null;
			Region region2 = null;
			int num = 1;
			int num2 = 2;
			bool flag = false;
			try
			{
				Rectangle rectangle = Rectangle.Empty;
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					foreach (object obj in originalSelComps)
					{
						Component component = (Component)obj;
						ToolStripItem toolStripItem = component as ToolStripItem;
						if (toolStripItem != null && (originalSelComps.Count > 1 || (originalSelComps.Count == 1 && toolStripItem.GetCurrentParent() != nextSelection.GetCurrentParent()) || toolStripItem is ToolStripSeparator || toolStripItem is ToolStripControlHost || !toolStripItem.IsOnDropDown || toolStripItem.IsOnOverflow))
						{
							ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(toolStripItem) as ToolStripItemDesigner;
							if (toolStripItemDesigner != null)
							{
								rectangle = toolStripItemDesigner.GetGlyphBounds();
								ToolStripDesignerUtils.GetAdjustedBounds(toolStripItem, ref rectangle);
								rectangle.Inflate(num, num);
								if (region == null)
								{
									region = new Region(rectangle);
									rectangle.Inflate(-num2, -num2);
									region.Exclude(rectangle);
								}
								else
								{
									region2 = new Region(rectangle);
									rectangle.Inflate(-num2, -num2);
									region2.Exclude(rectangle);
									region.Union(region2);
								}
							}
							else if (toolStripItem is DesignerToolStripControlHost)
							{
								flag = true;
							}
						}
					}
				}
				if (region != null || flag || shiftPressed)
				{
					BehaviorService behaviorService = (BehaviorService)provider.GetService(typeof(BehaviorService));
					if (behaviorService != null)
					{
						if (region != null)
						{
							behaviorService.Invalidate(region);
						}
						ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(nextSelection) as ToolStripItemDesigner;
						if (toolStripItemDesigner != null)
						{
							rectangle = toolStripItemDesigner.GetGlyphBounds();
							ToolStripDesignerUtils.GetAdjustedBounds(nextSelection, ref rectangle);
							rectangle.Inflate(num, num);
							region = new Region(rectangle);
							rectangle.Inflate(-num2, -num2);
							region.Exclude(rectangle);
							behaviorService.Invalidate(region);
						}
					}
				}
			}
			finally
			{
				if (region != null)
				{
					region.Dispose();
				}
				if (region2 != null)
				{
					region2.Dispose();
				}
			}
		}

		private const int TOOLSTRIPCHARCOUNT = 9;

		private static Type toolStripItemType = typeof(ToolStripItem);

		[ThreadStatic]
		private static Dictionary<Type, ToolboxItem> CachedToolboxItems;

		[ThreadStatic]
		private static int CustomToolStripItemCount = 0;

		public static ArrayList originalSelComps;

		[ThreadStatic]
		private static Dictionary<Type, Bitmap> CachedWinformsImages;

		private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;

		private static readonly Type[] NewItemTypesForToolStrip = new Type[]
		{
			typeof(ToolStripButton),
			typeof(ToolStripLabel),
			typeof(ToolStripSplitButton),
			typeof(ToolStripDropDownButton),
			typeof(ToolStripSeparator),
			typeof(ToolStripComboBox),
			typeof(ToolStripTextBox),
			typeof(ToolStripProgressBar)
		};

		private static readonly Type[] NewItemTypesForStatusStrip = new Type[]
		{
			typeof(ToolStripStatusLabel),
			typeof(ToolStripProgressBar),
			typeof(ToolStripDropDownButton),
			typeof(ToolStripSplitButton)
		};

		private static readonly Type[] NewItemTypesForMenuStrip = new Type[]
		{
			typeof(ToolStripMenuItem),
			typeof(ToolStripComboBox),
			typeof(ToolStripTextBox)
		};

		private static readonly Type[] NewItemTypesForToolStripDropDownMenu = new Type[]
		{
			typeof(ToolStripMenuItem),
			typeof(ToolStripComboBox),
			typeof(ToolStripSeparator),
			typeof(ToolStripTextBox)
		};

		internal static class DisplayInformation
		{
			static DisplayInformation()
			{
				SystemEvents.UserPreferenceChanged += ToolStripDesignerUtils.DisplayInformation.UserPreferenceChanged;
				SystemEvents.DisplaySettingsChanged += ToolStripDesignerUtils.DisplayInformation.DisplaySettingChanged;
			}

			public static short BitsPerPixel
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.bitsPerPixel == 0)
					{
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						try
						{
							foreach (Screen screen in Screen.AllScreens)
							{
								if (ToolStripDesignerUtils.DisplayInformation.bitsPerPixel == 0)
								{
									ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = (short)screen.BitsPerPixel;
								}
								else
								{
									ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = (short)Math.Min(screen.BitsPerPixel, (int)ToolStripDesignerUtils.DisplayInformation.bitsPerPixel);
								}
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					return ToolStripDesignerUtils.DisplayInformation.bitsPerPixel;
				}
			}

			public static bool LowResolution
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.lowResSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.lowRes;
					}
					ToolStripDesignerUtils.DisplayInformation.lowRes = ToolStripDesignerUtils.DisplayInformation.BitsPerPixel <= 8;
					ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.lowRes;
				}
			}

			public static bool HighContrast
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.highContrast;
					}
					ToolStripDesignerUtils.DisplayInformation.highContrast = SystemInformation.HighContrast;
					ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.highContrast;
				}
			}

			public static bool IsDropShadowEnabled
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled;
					}
					ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled = SystemInformation.IsDropShadowEnabled;
					ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled;
				}
			}

			public static bool TerminalServer
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.terminalSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession;
					}
					ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession = SystemInformation.TerminalServerSession;
					ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession;
				}
			}

			private static void DisplaySettingChanged(object obj, EventArgs ea)
			{
				ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = false;
			}

			private static void UserPreferenceChanged(object obj, UserPreferenceChangedEventArgs ea)
			{
				ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = 0;
			}

			private static bool highContrast;

			private static bool lowRes;

			private static bool isTerminalServerSession;

			private static bool highContrastSettingValid;

			private static bool lowResSettingValid;

			private static bool terminalSettingValid;

			private static short bitsPerPixel;

			private static bool dropShadowSettingValid;

			private static bool dropShadowEnabled;
		}
	}
}

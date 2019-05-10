using System;

namespace Standard
{
	internal static class CLSID
	{
		public const string ApplicationAssociationRegistration = "591209c7-767b-42b2-9fba-44ee4615f2c7";

		public const string DragDropHelper = "4657278A-411B-11d2-839A-00C04FD918D0";

		public const string FileOpenDialog = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7";

		public const string FileSaveDialog = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B";

		public const string TaskbarList = "56FDF344-FD6D-11d0-958A-006097C9A090";

		public const string EnumerableObjectCollection = "2d3468c1-36a7-43b6-ac24-d3f02fd9607a";

		public const string ShellLink = "00021401-0000-0000-C000-000000000046";

		public const string WICImagingFactory = "cacaf262-9370-4615-a13b-9f5539da4c0a";

		public const string DestinationList = "77f10cf0-3db5-4966-b520-b7c54fd35ed6";

		public const string ApplicationDestinations = "86c14003-4d6b-4ef3-a7b4-0506663b2e68";

		public const string ApplicationDocumentLists = "86bec222-30f2-47e0-9f25-60d11cd75c28";

		public static T CoCreateInstance<T>(string clsid)
		{
			return (T)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(clsid)));
		}
	}
}
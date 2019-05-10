using System;

namespace Standard
{
	internal struct PKEY
	{
		private readonly Guid _fmtid;

		private readonly uint _pid;

		public readonly static PKEY Title;

		public readonly static PKEY AppUserModel_ID;

		public readonly static PKEY AppUserModel_IsDestListSeparator;

		public readonly static PKEY AppUserModel_RelaunchCommand;

		public readonly static PKEY AppUserModel_RelaunchDisplayNameResource;

		public readonly static PKEY AppUserModel_RelaunchIconResource;

		static PKEY()
		{
			Class6.yDnXvgqzyB5jw();
			PKEY.Title = new PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2);
			PKEY.AppUserModel_ID = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
			PKEY.AppUserModel_IsDestListSeparator = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6);
			PKEY.AppUserModel_RelaunchCommand = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2);
			PKEY.AppUserModel_RelaunchDisplayNameResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4);
			PKEY.AppUserModel_RelaunchIconResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3);
		}

		public PKEY(Guid fmtid, uint pid)
		{
			Class6.yDnXvgqzyB5jw();
			this._fmtid = fmtid;
			this._pid = pid;
		}
	}
}
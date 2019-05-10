using System;

namespace Standard
{
	internal enum DROPIMAGETYPE
	{
		INVALID = -1,
		NONE = 0,
		COPY = 1,
		MOVE = 2,
		LINK = 4,
		LABEL = 6,
		WARNING = 7,
		NOIMAGE = 8
	}
}
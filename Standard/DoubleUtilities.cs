using System;
using System.Runtime.CompilerServices;

namespace Standard
{
	internal static class DoubleUtilities
	{
		public static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			if (num >= 1.53E-06)
			{
				return false;
			}
			return num > -1.53E-06;
		}

		public static bool IsCloseTo(this double value1, double value2)
		{
			return DoubleUtilities.AreClose(value1, value2);
		}

		public static bool IsFinite(this double value)
		{
			if (double.IsNaN(value))
			{
				return false;
			}
			return !double.IsInfinity(value);
		}

		public static bool IsFiniteAndNonNegative(this double d)
		{
			if (!double.IsNaN(d) && !double.IsInfinity(d) && d >= 0)
			{
				return true;
			}
			return false;
		}

		public static bool IsGreaterThanOrCloseTo(this double value1, double value2)
		{
			if (value1 > value2)
			{
				return true;
			}
			return DoubleUtilities.AreClose(value1, value2);
		}

		public static bool IsLessThanOrCloseTo(this double value1, double value2)
		{
			if (value1 < value2)
			{
				return true;
			}
			return DoubleUtilities.AreClose(value1, value2);
		}

		public static bool IsStrictlyGreaterThan(this double value1, double value2)
		{
			if (value1 <= value2)
			{
				return false;
			}
			return !DoubleUtilities.AreClose(value1, value2);
		}

		public static bool IsStrictlyLessThan(this double value1, double value2)
		{
			if (value1 >= value2)
			{
				return false;
			}
			return !DoubleUtilities.AreClose(value1, value2);
		}

		public static bool IsValidSize(this double value)
		{
			if (!value.IsFinite())
			{
				return false;
			}
			return value.IsGreaterThanOrCloseTo(0);
		}
	}
}
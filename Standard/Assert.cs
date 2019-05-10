using System;
using System.Diagnostics;
using System.Threading;

namespace Standard
{
	internal static class Assert
	{
		[Conditional("DEBUG")]
		public static void AreEqual<T>(T expected, T actual)
		{
			if (expected == null)
			{
				if (actual != null && !actual.Equals(expected))
				{
					Debugger.Break();
					return;
				}
			}
			else if (!expected.Equals(actual))
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void AreNotEqual<T>(T notExpected, T actual)
		{
			if (notExpected == null)
			{
				if (actual == null || actual.Equals(notExpected))
				{
					Debugger.Break();
					return;
				}
			}
			else if (notExpected.Equals(actual))
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		[Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
		public static void Equals<T>(T expected, T actual)
		{
		}

		[Conditional("DEBUG")]
		public static void Evaluate(Standard.Assert.EvaluateFunction argument)
		{
			argument();
		}

		[Conditional("DEBUG")]
		public static void Fail()
		{
			Debugger.Break();
		}

		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Debugger.Break();
		}

		[Conditional("DEBUG")]
		public static void Implies(bool condition, bool result)
		{
			if (condition && !result)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void Implies(bool condition, Standard.Assert.ImplicationFunction result)
		{
			if (condition && !result())
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsApartmentState(ApartmentState expectedState)
		{
			if (Thread.CurrentThread.GetApartmentState() != expectedState)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsDefault<T>(T value)
		where T : struct
		{
			value.Equals(default(T));
		}

		[Conditional("DEBUG")]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsNeitherNullNorEmpty(string value)
		{
		}

		[Conditional("DEBUG")]
		public static void IsNeitherNullNorWhitespace(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Debugger.Break();
			}
			if (value.Trim().Length == 0)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsNotDefault<T>(T value)
		where T : struct
		{
			value.Equals(default(T));
		}

		[Conditional("DEBUG")]
		public static void IsNotNull<T>(T value)
		where T : class
		{
			if (value == null)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsNull<T>(T item)
		where T : class
		{
			if (item != null)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue<T>(Predicate<T> predicate, T arg)
		{
			if (!predicate(arg))
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void LazyAreEqual<T>(Func<T> expectedResult, Func<T> actualResult)
		{
			T t = actualResult();
			T t1 = expectedResult();
			if (t1 == null)
			{
				if (t != null && !t.Equals(t1))
				{
					Debugger.Break();
					return;
				}
			}
			else if (!t1.Equals(t))
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void NullableIsNotNull<T>(Nullable<T> value)
		where T : struct
		{
			if (!value.HasValue)
			{
				Debugger.Break();
			}
		}

		[Conditional("DEBUG")]
		public static void NullableIsNull<T>(Nullable<T> value)
		where T : struct
		{
			if (value.HasValue)
			{
				Debugger.Break();
			}
		}

		public delegate void EvaluateFunction();

		public delegate bool ImplicationFunction();
	}
}
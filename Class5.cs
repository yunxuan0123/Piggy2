using System;
using System.Reflection;

internal class Class5
{
	internal static Module module_0;

	static Class5()
	{
		Class6.yDnXvgqzyB5jw();
		Class5.module_0 = typeof(Class5).Assembly.ManifestModule;
	}

	public Class5()
	{
		Class6.yDnXvgqzyB5jw();
		base();
	}

	internal static void mXTXvgqqxiWYe(int typemdt)
	{
		Type type = Class5.module_0.ResolveType(33554432 + typemdt);
		FieldInfo[] fields = type.GetFields();
		for (int i = 0; i < (int)fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
			MethodInfo methodInfo = (MethodInfo)Class5.module_0.ResolveMethod(fieldInfo.MetadataToken + 100663296);
			fieldInfo.SetValue(null, (MulticastDelegate)Delegate.CreateDelegate(type, methodInfo));
		}
	}

	internal delegate void Delegate0(object o);
}
using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	[IsReadOnly]
	internal struct StructMultiKey<T1, T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		public readonly T1 Value1;

		public readonly T2 Value2;

		public StructMultiKey(T1 v1, T2 v2)
		{
			Class6.yDnXvgqzyB5jw();
			this.Value1 = v1;
			this.Value2 = v2;
		}

		public override bool Equals(object obj)
		{
			object obj1 = obj;
			object obj2 = obj1;
			if (!(obj1 is StructMultiKey<T1, T2>))
			{
				return false;
			}
			return this.Equals((StructMultiKey<T1, T2>)obj2);
		}

		public bool Equals(StructMultiKey<T1, T2> other)
		{
			if (!object.Equals(this.Value1, other.Value1))
			{
				return false;
			}
			return object.Equals(this.Value2, other.Value2);
		}

		public override int GetHashCode()
		{
			// 
			// Current member / type: System.Int32 Newtonsoft.Json.Utilities.StructMultiKey`2::GetHashCode()
			// File path: C:\Users\Msi\Desktop\小喵\小喵谷登入器.exe
			// 
			// Product version: 2019.1.118.0
			// Exception in: System.Int32 GetHashCode()
			// 
			// Managed pointer usage not in SSA
			//    於 ..(BinaryExpression ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs: 行 100
			//    於 ..(BinaryExpression ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs: 行 76
			//    於 ..Visit(ICodeNode ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs: 行 141
			//    於 ..() 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs: 行 38
			//    於 ..(DecompilationContext ,  ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs: 行 20
			//    於 ..(MethodBody ,  , ILanguage ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs: 行 88
			//    於 ..(MethodBody , ILanguage ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs: 行 70
			//    於 Telerik.JustDecompiler.Decompiler.Extensions.( , ILanguage , MethodBody , DecompilationContext& ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs: 行 95
			//    於 Telerik.JustDecompiler.Decompiler.Extensions.(MethodBody , ILanguage , DecompilationContext& ,  ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs: 行 58
			//    於 ..(ILanguage , MethodDefinition ,  ) 於 C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs: 行 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}
	}
}
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;
using ReSharper.PackageV3;

namespace TestsV10
{
	[TestFixture]
	public class NullObjectContextActionTests : CSharpContextActionExecuteTestBase<NullObjectContextAction>
	{
		protected override string ExtraPath => "NullObjectContextActionTests";

		protected override string RelativeTestDataPath => "NullObjectContextActionTests";

		[Test]
		public void Test01()
		{
			DoTestFiles("Test01.cs");
		}

		[Test]
		public void Test02()
		{
			DoTestFiles("Test02.cs");
		}
	}
}

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
		public void Test_Execute()
		{
			DoTestFiles("Test_Execute.cs");
		}

		[Test]
		public void Test_Fail()
		{
			DoTestFiles("Test_Fail.cs");
		}
	}
}

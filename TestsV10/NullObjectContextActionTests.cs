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
		public void Test_ListOfObject_Success()
		{
			DoTestFiles("Test_ListOfObject_Success.cs");
		}

		[Test]
		public void Test_ListOfInt_Success()
		{
			DoTestFiles("Test_ListOfInt_Success.cs");
		}

		[Test]
		public void Test_Fail()
		{
			DoTestFiles("Test_Fail.cs");
		}
	}
}

using Ksnm.Numerics;

namespace TestProject
{
    [TestClass()]
    public class BigFractionTests
    {
        [TestMethod()]
        public void ConstantTest()
        {

        }
        [TestMethod()]
        public void ConstructorTest()
        {
            {
                BigFraction bigFraction = new BigFraction();
                Assert.AreEqual("0/0", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(1);
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(1, 2);
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
        }
    }
}

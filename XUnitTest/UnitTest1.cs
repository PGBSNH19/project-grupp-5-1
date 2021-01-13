using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("Hello")]
        public void CIFakeTest(string input)
        {
            string actual = "Hello";

            Assert.Equal(input, actual);
        }
    }
}
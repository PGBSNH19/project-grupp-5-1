using System;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("Hello")]
        public void CIFakeTest(string input)
        {
            string actual = "Hell";

            Assert.Equal(input, actual);
        }
    }
}

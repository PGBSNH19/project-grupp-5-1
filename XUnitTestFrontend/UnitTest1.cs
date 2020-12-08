using System;
using Xunit;

namespace XUnitTestFrontend
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("SuperMarkt")]
        public void CIFakeTest(string input)
        {
            string actual = "SuperMarkt";

            Assert.Equal(input, actual);
        }
    }
}

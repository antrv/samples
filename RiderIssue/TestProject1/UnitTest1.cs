using ClassLibrary1;
using FluentAssertions;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MyEnum[] a = {MyEnum.Item1};

            a[0].Should().Be(MyEnum.Item1);
        }
    }
}

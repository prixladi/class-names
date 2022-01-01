using System;
using Xunit;

namespace ClassNames.UnitTests
{
    public partial class CNTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("", null)]
        [InlineData("c1", "c1")]
        [InlineData("c1 c2 c3 c4 c5 c6", "c1", "c2", "c3", "c4", "c5 c6")]
        [InlineData("c1 c2 c3 c4 c5 c6", "c1", null, "c2", "c3", null, "c4", "c5 c6")]
        [InlineData("c1 c2 c3 c4 c5 c6", "c1", null, "", "c2", "c3", null, "c4", "", "c5 c6", "")]
        public void Compose_strings(string className, params string?[]? classes)
        {
            var cn = CN.New();

            foreach(var c in classes ?? Array.Empty<string>())
            {
                cn.Add(c);
            }

            var result = cn.Compile();

            Assert.Equal(className, result);
        }

        [Fact]
        public void Compose_strings_nullIfWhitespace()
        {
            var cn = CN.New();

            var result = cn.Compile(true);

            Assert.Null(result);
        }

        [Fact]
        public void Compose_objects_simple()
        {
            var obj = new { c3 = false, c4 = true, c5 = "true", c6 = "false", c7 = true };
            var result = CN.New("c1 c2").Add(obj).Compile();

            Assert.Equal("c1 c2 c4 c5 c7", result);
        }

        [Fact]
        public void Compose_simple()
        {
            var result = CN.New("c1 c2")
                .Add("c3 c4", true)
                .Add("c5", false)
                .Add("c6", true)
                .Add("c7", false)
                .Compile();

            Assert.Equal("c1 c2 c3 c4 c6", result);
        }
    }
}
using System;
using Xunit;

namespace ClassNames.UnitTests
{
    public partial class ClassNameTests
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
            var cn = ClassName.New();

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
            var cn = ClassName.New();

            var result = cn.Compile(true);

            Assert.Null(result);
        }

        [Fact]
        public void Compose_stringArrays()
        {
            var cn = ClassName.New().Add("c1", "c2", "c3");

            var result = cn.Compile(true);

            Assert.Equal("c1 c2 c3", result);
        }

        [Fact]
        public void Compose_stringArrays_emapty()
        {
            var cn = ClassName.New().Add("   ", "", null, "");

            var result = cn.Compile();

            Assert.Equal(string.Empty,result);
        }

        [Fact]
        public void Compose_stringArrays_nullIfWhitespace()
        {
            var cn = ClassName.New().Add("   ", "", null, "");

            var result = cn.Compile(true);

            Assert.Null(result);
        }

        [Fact]
        public void Compose_objects_simple()
        {
            var obj = new { c3 = false, c4 = true, c5 = "true", c6 = "false", c7 = true };
            var result = ClassName.New("c1 c2").Add(obj).Compile();

            Assert.Equal("c1 c2 c4 c5 c7", result);
        }

        [Fact]
        public void Compose_cns_simple1()
        {
            var c1 = ClassName.New("c1 c2");
            var c2 = ClassName.New().Add("c3", false).Add("c4", true);

            var result = ClassName.New().Add(c1).Add(c2).Compile(); 

            Assert.Equal("c1 c2 c4", result);
        }

        [Fact]
        public void Compose_cns_simple2()
        {
            var c1 = ClassName.New("c1 c2");
            var c2 = ClassName.New().Add("c3", false).Add("c4", true);

            var result = ClassName.New().Add(c1, false).Add(c2).Compile();

            Assert.Equal("c4", result);
        }

        [Fact]
        public void Compose_ternary_simple()
        {
            var cn = ClassName.New()
                .Ternary("c1", "c2", true)
                .Ternary("c3", "c4", false)
                .Ternary(null!, "c5", true)
                .Ternary("c6", "   ", false);

            var result = cn.Compile();

            Assert.Equal("c1 c4", result);
        }

        [Fact]
        public void Compose_simple()
        {
            var result = ClassName.New("c1 c2")
                .Add("c3 c4", true)
                .Add("c5", false)
                .Add("c6", true)
                .Add("c7", false)
                .Compile();

            Assert.Equal("c1 c2 c3 c4 c6", result);
        }

        [Fact]
        public void Compose_complex()
        {
            var result = ClassName.New("c1 c2")
                .Add("c3 c4", true)
                .Add("c5", false)
                .Add(new[] { ("c6", false), ("c7", true), ("c8", true) })
                .Add("c9")
                .Add(new { c10 = false, c11 = true, c12 = true })
                .Add("c13 c14")
                .Compile();

            Assert.Equal("c1 c2 c3 c4 c7 c8 c9 c11 c12 c13 c14", result);
        }
    }
}
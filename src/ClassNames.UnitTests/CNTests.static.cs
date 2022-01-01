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
        public void Merge_strings(string className, params string?[]? classes)
        {
            var result = CN.Merge(classes);

            Assert.Equal(className, result);
        }

        [Fact]
        public void Merge_objects_simple()
        {
            var result = CN.Merge(new { c1 = false, c2 = true, c3 = "true", c4 = "false", c5 = true });

            Assert.Equal("c2 c3 c5", result);
        }

        [Fact]
        public void Merge_objects_complex()
        {
            var result = CN.Merge(
                new { c1 = false, c2 = true, c3 = "true", c4 = "false", c5 = true },
                null,
                new { c6 = true, c7 = "0", c8 = "true" },
                new { c9 = false, c10 = true, c11 = "false" },
                new { },
                null);

            Assert.Equal("c2 c3 c5 c6 c8 c10", result);
        }


        [Fact]
        public void Merge_tuples_simple()
        {
            var result = CN.Merge(new[] { ("c1", true), ("c2", false), ("c3", true) });

            Assert.Equal("c1 c3", result);
        }

        [Fact]
        public void Merge_tuples_complex()
        {
            var result = CN.Merge(
                new[] { ("c1", true), ("c2", false), ("c3", true) },
                new[] { ("c4", true), ("c5", false), ("c6", true) },
                new[] { ("c7", true), ("c8", false), ("c9", true) });

            Assert.Equal("c1 c3 c4 c6 c7 c9", result);
        }

        [Fact]
        public void Merge_cns_simple()
        {
            var result = CN.Merge(CN.New().Add("c1", true).Add("c2", true).Add("c3", false));

            Assert.Equal("c1 c2", result);
        }

        [Fact]
        public void Merge_cns_complex()
        {
            var result = CN.Merge(
                CN.New().Add("c1").Add("c2").Add("c3", false),
                CN.New("c4").Add("c5", false).Add("c6", false).Add("c7", false),
                CN.New("c8 c9").Add("c10", true).Add("c11").Add("c12", true)
            );

            Assert.Equal("c1 c2 c4 c8 c9 c10 c11 c12", result);
        }

        [Fact]
        public void Merge_all_complex()
        {
            var result = CN.Merge(
                null,
                new
                {
                    c1 = true,
                    c2 = false,
                    c3 = true,
                },
                "",
                CN.New("c4 c5").Add("c6", true).Add("c7", true).Add("c8", false),
                "c9",
                new[] { ("c10", true), ("c11", false) }
            );

            Assert.Equal("c1 c3 c4 c5 c6 c7 c9 c10", result);
        }
    }
}
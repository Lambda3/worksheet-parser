using FluentAssertions;
using NUnit.Framework;
using System;

namespace Worksheet.Parser.Tests
{
    [SetUpFixture]
    public class SetUp
    {
        [OneTimeSetUp]
        public void OneTimeSetUp() => AssertionOptions.AssertEquivalencyUsing(options =>
                                    {
                                        options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000)).WhenTypeIs<DateTime>();
                                        options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000)).WhenTypeIs<DateTimeOffset>();
                                        options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.00000000000001M)).WhenTypeIs<decimal>();
                                        return options;
                                    });
    }
}

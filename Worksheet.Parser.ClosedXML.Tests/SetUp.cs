using FluentAssertions;
using NUnit.Framework;
using System;

namespace Worksheet.Parser.ClosedXML.Tests
{
    [SetUpFixture]
    public class SetUp
    {
        [OneTimeSetUp]
        public void OneTimeSetUp() => AssertionOptions.AssertEquivalencyUsing(options =>
                                    {
                                        options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 999)).WhenTypeIs<DateTime>();
                                        options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 999)).WhenTypeIs<DateTimeOffset>();
                                        return options;
                                    });
    }
}

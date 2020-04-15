using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using Worksheet.Parser.Tests.Fakers;

namespace Worksheet.Parser.Tests
{
    public partial class ExpressionsExtensionsTests
    {
        private readonly MemberExpression body;

        public ExpressionsExtensionsTests()
        {
            Expression<Func<FakeClass, object>> expression = x => x.Id;
            body = expression.GetBody();
        }

        [Test]
        public void ShouldGetNameProperty() => body.Member.Name.Should().Be("Id");

        [Test]
        public void ShouldGetFullNameType() => body.Type.FullName.Should().Be("System.Int32");
    }
}

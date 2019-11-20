using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using Worksheet.Parser.Tests.Shared;

namespace Worksheet.Parser.Tests
{
    public class PropertyInfoTests
    {
        private readonly PropertyInfo propertyInfo;

        public PropertyInfoTests() => propertyInfo = PropertyInfo.Create<FakeClass>(x => x.Id);

        [Test]
        public void ShouldGetNameProperty() => propertyInfo.Name.Should().Be("Id");

        [Test]
        public void ShouldGetType() => propertyInfo.Type.Should().Be(typeof(int));
    }
}

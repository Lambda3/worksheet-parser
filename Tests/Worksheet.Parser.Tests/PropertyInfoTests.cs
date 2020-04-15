using FluentAssertions;
using NUnit.Framework;
using Worksheet.Parser.Tests.Fakers;

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

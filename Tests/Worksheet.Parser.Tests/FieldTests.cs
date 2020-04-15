using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Worksheet.Parser.Tests.Fakers;

namespace Worksheet.Parser.Tests
{
    public class FieldTests
    {
        private Field<FakeClass> field;
        private readonly Action<FakeClass, object> convertionAction = (fake, value) => fake.Id = (int)value;
        private const string fieldName = "name";

        [SetUp]
        public void SetUp() => field = new Field<FakeClass>(x => x.Id);

        [Test]
        public void ShouldInitializeValidationsOnInstantiate() => field.GetValidations().Should().BeEmpty();

        [Test]
        public void ShouldCreatePropertyInfoOnInstantiate()
            => field.PropertyInfo.Should().BeEquivalentTo(PropertyInfo.Create<FakeClass>(x => x.Id));

        [Test]
        public void ShouldInitializeWithIgnoredFalse() => field.ShouldBeIgnored.Should().BeFalse();

        [Test]
        public void ShouldMapToFieldName() => field.ToFieldName(fieldName).Name.Should().Be(fieldName);

        [Test]
        public void ShouldMapToRequiredFieldName() => field.ToRequiredField(fieldName).Name.Should().Be(fieldName);

        [Test]
        public void ShouldSetRequiredField() => field.ToRequiredField(A.Dummy<string>()).Required.Should().BeTrue();

        [Test]
        public void ShouldAddCustomConverter()
            => field.WithCustomConverter(convertionAction).Converter.Should().BeEquivalentTo(convertionAction);

        [Test]
        public void ShouldHaveCustomConverterIfInformed()
            => field.WithCustomConverter(convertionAction).HasCustomConverter().Should().BeTrue();

        [Test]
        public void ShouldSetIgnored() => field.Ignored().ShouldBeIgnored.Should().BeTrue();

        [Test]
        public void ShouldAddValidations()
        {
            var validations = new List<Validation> { A.Fake<Validation>(), A.Fake<Validation>() };
            validations.ForEach(v => field.WithValidation(v));
            field.GetValidations().Should().BeEquivalentTo(validations);
        }
    }
}

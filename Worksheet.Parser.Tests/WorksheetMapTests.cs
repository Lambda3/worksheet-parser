using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Worksheet.Parser.Tests.Shared;

namespace Worksheet.Parser.Tests
{
    public class WorksheetMapTests
    {
        private FakeWorksheetMap worksheetMap;

        [SetUp]
        public void SetUp() => worksheetMap = new FakeWorksheetMap();

        [Test]
        public void ShouldAddFieldOnMap() => worksheetMap.GetFields().Should()
            .BeEquivalentTo(new List<Field<FakeClass>>
            {
                new Field<FakeClass>(x=>x.Id),
                new Field<FakeClass>(x=>x.Name)
            });

        public class FakeWorksheetMap : WorksheetMap<FakeClass>
        {
            public FakeWorksheetMap()
            {
                Map(x => x.Id);
                Map(x => x.Name);
            }
        }
    }
}

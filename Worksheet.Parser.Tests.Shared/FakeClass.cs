using System;

namespace Worksheet.Parser.Tests.Shared
{
    public class FakeClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool Enable { get; set; }
        public decimal? Bonus { get; set; }

        public static FakeClass CreateItem(bool nullBonus = false, bool nullFinishDate = false) => new FakeClass
        {
            Bonus = nullBonus ? MyFaker.Faker.Random.Decimal() : default,
            Enable = MyFaker.Faker.Random.Bool(),
            FinishDate = nullFinishDate ? MyFaker.Faker.Date.Future() : default,
            Id = MyFaker.Faker.Random.Int(),
            Name = MyFaker.Faker.Name.FirstName()
        };
    }
}

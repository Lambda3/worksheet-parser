namespace Worksheet.Parser.Tests.Fakers
{
    public class MyWorksheetMap : WorksheetMap<FakeClass>
    {
        public MyWorksheetMap()
        {
            Map(x => x.Id).ToRequiredField("NU_ID");
            Map(x => x.Name).ToRequiredField("TX_NAME");
            Map(x => x.FinishDate).ToFieldName("DT_FINISH");
            Map(x => x.Enable).ToFieldName("BO_ENABLE");
            Map(x => x.Bonus).ToFieldName("DB_BONUS")
                .WithCustomConverter((item, valor) => item.Bonus = valor != null ? (decimal)valor * 0.1M : default);
        }
    }
}

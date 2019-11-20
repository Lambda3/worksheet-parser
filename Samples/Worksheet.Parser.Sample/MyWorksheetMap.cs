namespace Worksheet.Parser.Sample
{
    public class MyWorksheetMap : WorksheetMap<RowFake>
    {
        public MyWorksheetMap()
        {
            Map(x => x.Id).ToRequiredField("NU_ID");
            Map(x => x.Name).ToRequiredField("TX_NAME");
            Map(x => x.CreationDate).ToRequiredField("DT_CREATION");
            Map(x => x.FinishDate).ToFieldName("DT_FINISH");
            Map(x => x.RegisterNumber).ToFieldName("NU_REGISTER");
            Map(x => x.Enable).ToFieldName("BO_ENABLE");
            Map(x => x.Value).ToRequiredField("DE_VALUE").WithValidation(new RangeValidation());
            Map(x => x.Bonus).ToFieldName("DB_BONUS")
                .WithCustomConverter((item, valor) => item.Bonus = valor != null ? (double)valor * 0.1 : default);
        }
    }
}

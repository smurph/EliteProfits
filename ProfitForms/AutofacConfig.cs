namespace ProfitForms
{
    using System;
    using Autofac;
    using EliteProfits;
    using System.Configuration;

    public static class AutofacConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterInstance(
                    new PilotJournalReader(ConfigurationManager.AppSettings["EliteDangerousJournalPath"])
                ).As<IJournalReader>().SingleInstance();

            builder.Register(c => new TradeData(c.Resolve<IJournalReader>()));
            builder.Register(c => new MissionData(c.Resolve<IJournalReader>()));
            return builder.Build();
        }
    }
}

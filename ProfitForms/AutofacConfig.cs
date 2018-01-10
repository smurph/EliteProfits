namespace ProfitForms
{
    using System;
    using Autofac;
    using EliteProfits;
    using System.Configuration;

    public static class AutofacConfig
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            var reader = new PilotJournalReader(ConfigurationManager.AppSettings["EliteDangerousJournalPath"]);

            builder.RegisterInstance(reader).SingleInstance();

            builder.Register(c => new TradeData(reader));
            builder.Register(c => new MissionData(reader));
            return builder.Build();
        }
    }
}

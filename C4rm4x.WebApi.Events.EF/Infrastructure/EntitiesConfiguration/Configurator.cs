using System.Data.Entity;

namespace C4rm4x.WebApi.Events.EF.Infrastructure.EntitiesConfiguration
{
    internal static class Configurator
    {
        public static void Configure(DbModelBuilder modelBuilder)        {            modelBuilder.Configurations.Add(new EventMap());        }
    }
}

using JetBrains.Annotations;

using Lamar;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace UsefulWpfLibrary.Logic
{
    public static class Ioc
    {
        public static Container? Container { get; private set; }

        public static void Configure(Action<IServiceCollection> configure)
        {
            Container!.Configure(configure);
        }

        public static object Get(Type type)
        {
            return Container!.GetService(type);
        }

        public static T? Get<T>()
        {
            return Container!.GetService<T>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage",
            "DF0026:Marks undisposed objects assinged to a property, originated in an object creation.",
            Justification = "<挂起>")]
        public static void Init([NotNull] Action<ServiceRegistry> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (Container is not null)
                throw new InvalidOperationException($"{nameof(Container)}已经初始化完毕");
            Container = new Container(configuration);
        }
    }
}
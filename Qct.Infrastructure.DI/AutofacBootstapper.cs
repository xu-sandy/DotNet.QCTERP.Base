using Autofac;

namespace Qct.Infrastructure.DI
{
    public abstract class AutofacBootstapper
    {
        public void Run()
        {
            var builder = CreateContainerBuilder();
            ConfigureContainerBuilder(builder);
            CurrentContainer = CreateContainer(builder);
        }


        /// <summary>
        /// 实现分区或者模块化注册
        /// </summary>
        /// <param name="areas"></param>
        public abstract void ConfigureContainerBuilder(ContainerBuilder builder);
        /// <summary>
        /// 当前实例容器
        /// </summary>
        public static IContainer CurrentContainer { get; private set; }
        /// <summary>
        /// 创建实例容器
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public virtual IContainer CreateContainer(ContainerBuilder containerBuilder)
        {
            return containerBuilder.Build();
        }
        /// <summary>
        /// 创建实例容器的Builder
        /// </summary>
        /// <returns></returns>
        public virtual ContainerBuilder CreateContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();
            return builder;
        }
    }
}

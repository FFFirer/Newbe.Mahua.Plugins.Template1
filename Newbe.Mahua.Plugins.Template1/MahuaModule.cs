using Autofac;
using Newbe.Mahua.MahuaEvents;
using Newbe.Mahua.Plugins.Template1.MahuaEvents;
using Newbe.Mahua.Plugins.Template1.Services;
using Newbe.Mahua.Plugins.Template1.Services.Impl;

namespace Newbe.Mahua.Plugins.Template1
{
    /// <summary>
    /// Ioc容器注册
    /// </summary>
    public class MahuaModule : IMahuaModule
    {
        public Module[] GetModules()
        {
            // 可以按照功能模块进行划分，此处可以改造为基于文件配置进行构造。实现模块化编程。
            return new Module[]
            {
                new PluginModule(),
                new MahuaEventsModule(),
                new MyServiceMoudle(),
            };
        }

        /// <summary>
        /// 基本模块
        /// </summary>
        private class PluginModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                // 将实现类与接口的关系注入到Autofac的Ioc容器中。如果此处缺少注册将无法启动插件。
                // 注意！！！PluginInfo是插件运行必须注册的，其他内容则不是必要的！！！
                builder.RegisterType<PluginInfo>()
                    .As<IPluginInfo>();

                //注册在“设置中心”中注册菜单，若想订阅菜单点击事件，可以查看教程。http://www.newbe.pro/docs/mahua/2017/12/24/Newbe-Mahua-Navigations.html
                builder.RegisterType<MyMenuProvider>()
                    .As<IMahuaMenuProvider>();
            }
        }

        /// <summary>
        /// <see cref="IMahuaEvent"/> 事件处理模块
        /// </summary>
        private class MahuaEventsModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                // 将需要监听的事件注册，若缺少此注册，则不会调用相关的实现类
                builder.RegisterType<PrivateMessageFromFriendReceivedMahuaEvent1>()
                    .As<IPrivateMessageFromFriendReceivedMahuaEvent>();
                builder.RegisterType<MahuaMenuClickedMahuaEventClickMenu>()
                    .As<IMahuaMenuClickedMahuaEvent>();
                builder.RegisterType<FriendAddingRequestMahuaEvent1>()
                    .As<IFriendAddingRequestMahuaEvent>();
                builder.RegisterType<FriendAddedMahuaEvent1>()
                    .As<IFriendAddedMahuaEvent>();
                builder.RegisterType<PrivateMessageFromGroupReceivedMahuaEvent1>()
                    .As<IPrivateMessageFromGroupReceivedMahuaEvent>();
                builder.RegisterType<InitializationMahuaEvent1>()
                    .As<IInitializationMahuaEvent>();
                builder.RegisterType<GroupMemberIncreasedMahuaEvent1>()
                    .As<IGroupMemberIncreasedMahuaEvent>();
            }
        }

        /// <summary>
        /// 服务处理模块
        /// </summary>
        private class MyServiceMoudle : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                //确保服务时单例
                builder.RegisterType<OwinWebHost>()
                    .As<IWebHost>()
                    .SingleInstance();

                //AsSelf是为了Hangfire能够初始化这个类
                builder.RegisterType<PublishQuan>()
                    .As<IPublishQuan>().
                    AsSelf();

                builder.RegisterType<AdminControl>()
                    .As<IAdminControl>();

                builder.RegisterType<FaQuanStorege>()
                    .As<IFaQuanStorege>();

                builder.RegisterType<SqliteDbHelper>()
                    .As<IDbHelper>();
            }
        }
    }
}

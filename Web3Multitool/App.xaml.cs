using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web3MultiTool.Domain.Commands;
using Web3MultiTool.Domain.Queries;
using Web3Multitool.EntityFramework;
using Web3Multitool.EntityFramework.Commands;
using Web3Multitool.EntityFramework.Queries;
using Web3Multitool.HostBuilders;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    string connectionString = context.Configuration.GetConnectionString("sqlite");
                    services.AddSingleton(new DbContextOptionsBuilder().UseSqlite(connectionString, b => b.MigrationsAssembly("Web3Multitool.EntityFramework")).Options);
                    services.AddSingleton<AccountInfoDbContextFactory>();
                    
                    services.AddSingleton<ICreateAccountInfoCommand, CreateAccountInfoCommand>();
                    services.AddSingleton<IUpdateAccountInfoCommand, UpdateAccountInfoCommand>();
                    services.AddSingleton<IDeleteAccountInfoCommand, DeleteAccountInfoCommand>();
                    services.AddSingleton<IClearAccountInfosCommand, ClearAccountInfosCommand>();
                    services.AddSingleton<IGetAllAccountsQuery, GetAllAccountsQuery>();

                    services.AddSingleton<AccountInfosStore>();

                    services.AddTransient(CreateViewTabViewModel);
                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton<MainWindow>((serviceProvider) => new MainWindow()
                    {
                        DataContext = serviceProvider.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        private ViewTabViewModel CreateViewTabViewModel(IServiceProvider serviceProvider)
        {
            return ViewTabViewModel.LoadViewModel(serviceProvider.GetRequiredService<AccountInfosStore>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var accountInfoDbContextFactory = _host.Services.GetRequiredService<AccountInfoDbContextFactory>();
            using (var context = accountInfoDbContextFactory.Create())
            {
                context.Database.Migrate();
            }

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            _host.Dispose();
            
            base.OnExit(e);
        }
    }
}
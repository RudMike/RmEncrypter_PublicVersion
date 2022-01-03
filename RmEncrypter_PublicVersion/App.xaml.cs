// <copyright file="App.xaml.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.IO;
    using System.Windows;
    using System.Windows.Threading;
    using Prism.DryIoc;
    using Prism.Ioc;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.MessageBox;
    using RmEncrypter_PublicVersion.Models;
    using RmEncrypter_PublicVersion.ViewModels;
    using RmEncrypter_PublicVersion.Views;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        private readonly SplashScreen splashScreen = new (@"Resources\SplashScreen.png");
        private IConnectionProvider connection;

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.splashScreen.Show(true, true);
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            LocalizationService.Language = RmEncrypter_PublicVersion.Properties.Settings.Default.CurrentLanguage;
            this.connection = new SqlLiteDatabaseConnection();
            this.CreateDbIfNotExist();
            base.OnStartup(e);
        }

        /// <inheritdoc/>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ChangeLanguageView, ChangeLanguageViewModel>();
            containerRegistry.RegisterForNavigation<AuthorizationView, AuthorizationViewModel>();
            containerRegistry.RegisterForNavigation<UserRegistrationView, UserRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<KeyRegistrationView, KeyRegistrationViewModel>();
            containerRegistry.RegisterForNavigation<TopPanelView, TopPanelViewModel>();
            containerRegistry.RegisterForNavigation<BottomPanelView>();
            containerRegistry.RegisterForNavigation<OpenFileView, OpenFileViewModel>();
            containerRegistry.RegisterForNavigation<ContentPanelView, ContentPanelViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordView, ChangePasswordViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmAccountPasswordView, ConfirmAccountPasswordViewModel>();
            containerRegistry.RegisterForNavigation<SelectFilesView, SelectFilesViewModel>();
            containerRegistry.RegisterForNavigation<ReEncryptFilesView, ReEncryptFilesViewModel>();
            containerRegistry.RegisterForNavigation<AccountTransferView, AccountTransferViewModel>();
            containerRegistry.RegisterForNavigation<SelectRecoveryFileView, SelectRecoveryFileViewModel>();
            containerRegistry.RegisterForNavigation<AccountRecoveryView, AccountRecoveryViewModel>();
            containerRegistry.RegisterForNavigation<AccountDeleteView, AccountDeleteViewModel>();
            containerRegistry.RegisterForNavigation<ShowEditRecordView, ShowEditRecordViewModel>();
            containerRegistry.RegisterForNavigation<AddRecordView, AddRecordViewModel>();

            containerRegistry.RegisterSingleton<IShellService, ShellService>();
            containerRegistry.RegisterSingleton<IAuthorizedUser, CurrentAuthorizedUser>();
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
            containerRegistry.RegisterSingleton<IMessageBoxService, MessageBoxService>();

            containerRegistry.RegisterInstance(typeof(IConnectionProvider), this.connection);

            containerRegistry.Register<IStringHasher, DoNothingPasswordHasher>();
            containerRegistry.Register<IDatabaseWriter, DatabaseWriter>();
            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IModelValidator<AuthenticationData>, AuthDataValidator>();
            containerRegistry.Register<IModelValidator<RsaKey>, RsaKeyValidator>();
            containerRegistry.Register<IModelValidator<SiteAuthData>, SiteAuthDataValidator>();
            containerRegistry.Register<IPrimeGenerator, PrimeGenerator>();
            containerRegistry.Register<IRsaKeyGenerator, RsaKeyGenerator>();
            containerRegistry.Register<IRsaKeyService, RsaKeyService>();
            containerRegistry.Register<IRsaKeyObfuscator, DoNothingRsaKeyObfuscator>();
            containerRegistry.Register<IDatabaseReader, DatabaseReader>();
            containerRegistry.Register<IFileDialogService, Win32FileDialog>();
            containerRegistry.Register<IFileService<SiteAuthData>, JsonFileService>();
            containerRegistry.Register<IEncryptionService<IRsaKey, SiteAuthData>, RsaEncryptionService>();
            containerRegistry.Register<IEncryptedListManager<SiteAuthData>, SiteAuthDataListManager>();
            containerRegistry.Register<IAccountReencrypter, AuthorizedUserReencrypter>();
            containerRegistry.Register<IAccountTransfer, AccountTransferService>();
            containerRegistry.Register<IFileService<string>, StringFileService>();
            containerRegistry.Register<IStringObfuscator, DoNothingStringObfuscator>();
            containerRegistry.Register<INavigationRepository, NavigationRepository>();

            containerRegistry.RegisterDialogWindow<MessageBoxWindow>("MessageBox");
            containerRegistry.RegisterDialogWindow<UserRecordsShell>("UserRecords");

            containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            containerRegistry.RegisterDialog<AddRecordView, AddRecordViewModel>();
            containerRegistry.RegisterDialog<ShowEditRecordView, ShowEditRecordViewModel>();
        }

        /// <inheritdoc/>
        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
        }

        /// <inheritdoc/>
        protected override Window CreateShell()
        {
            return this.Container.Resolve<DialogShell>();
        }

        /// <inheritdoc/>
        protected override void InitializeShell(Window shell)
        {
            var regionManager = RegionManager.GetRegionManager(shell);
            regionManager.RequestNavigate(AppRegions.DialogShellContentRegion, nameof(AuthorizationView));
            RegionManagerAware.SetRegionManagerAware(shell, regionManager);
            base.InitializeShell(shell);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string message = LocalizationService.GetString("UnhandledExceptionNotify") +
                $"\r\nType: {e.Exception.GetType()}" +
                $"\r\nMessage: {e.Exception.Message}" +
                $"\r\nStackTrace: {e.Exception.StackTrace}";

            if (e.Exception.InnerException != null)
            {
                message += $"\r\nInnerException: {e.Exception.InnerException}";
            }

            System.Windows.MessageBox.Show(
                message,
                LocalizationService.GetString("Error"),
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void CreateDbIfNotExist()
        {
            if (!File.Exists(this.connection.FilePath))
            {
                using AccountContext context = new (this.connection);
                _ = context.Database.EnsureCreated();
            }
        }
    }
}

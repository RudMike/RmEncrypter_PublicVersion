// <copyright file="NavigationRepository.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Prism.Regions;
    using Prism.Services.Dialogs;
    using RmEncrypter_PublicVersion.Models;
    using RmEncrypter_PublicVersion.Views;

    /// <summary>
    /// Provides functionality for navigation between views.
    /// The most part of views will be opened in the <see cref="AppRegions.DialogShellContentRegion"/>.
    /// If region does not exist in the <see cref="RegionManager"/> the new <see cref="DialogShell"/> will be opened.
    /// Views the return value will be opened in the new dialog window.
    /// </summary>
    public class NavigationRepository : INavigationRepository
    {
        private readonly IShellService shellService;
        private readonly IDialogService dialogService;
        private IEnumerable<RegionNavigationInfo> mainShellNavigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRepository"/> class.
        /// </summary>
        /// <param name="shellService">A service for shell management.</param>
        /// <param name="dialogService">Service for showing a modal dialog.</param>
        public NavigationRepository(IShellService shellService, IDialogService dialogService)
        {
            this.shellService = shellService;
            this.dialogService = dialogService;
            this.SetMainShellNavigation();
        }

        /// <inheritdoc/>
        public IRegionManager RegionManager
        { get; set; }

        /// <inheritdoc/>
        public virtual void ShowMainShell()
        {
            this.shellService.ShowShell<MainShell>(this.mainShellNavigation, false);
        }

        /// <inheritdoc/>
        public virtual void ToView(NavigationInfo navigationInfo)
        {
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        /// <remarks>You will need to confirm the password before.</remarks>
        public virtual void ToAccountDelete()
        {
            var navigationInfo = new NavigationInfo(nameof(AccountDeleteView));
            this.ToConfirmAccountPassword(navigationInfo, false);
        }

        /// <inheritdoc/>
        public virtual void ToAccountRecovery(string recoveryFile)
        {
            var navigationParameter = new NavigationParameters()
            {
                { NavigationParameterKeys.FilePath, recoveryFile },
            };
            var navigationInfo = new NavigationInfo(nameof(AccountRecoveryView), navigationParameter);
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        /// <remarks>You will need to confirm the password before.</remarks>
        public virtual void ToAccountTransfer()
        {
            this.ToConfirmAccountPassword(new NavigationInfo(nameof(AccountTransferView)));
        }

        /// <inheritdoc/>
        /// <remarks>The view will be opened in dialog window.</remarks>
        public virtual SiteAuthData ToAddRecord()
        {
            SiteAuthData result = null;
            this.dialogService.ShowDialog(
                nameof(AddRecordView),
                null,
                returns =>
                {
                    result = returns.Parameters.GetValue<SiteAuthData>(DialogParameterKeys.Entity);
                },
                "UserRecords");

            return result;
        }

        /// <inheritdoc/>
        public virtual void ToAuthorization()
        {
            var navigationInfo = new NavigationInfo(nameof(AuthorizationView));
            this.NavigateInDialogShell(navigationInfo, false);
        }

        /// <inheritdoc/>
        /// <remarks>The view opens in the main shell.</remarks>
        public virtual void ToBottom()
        {
            var navigationInfo = new NavigationInfo(nameof(BottomPanelView));
            var regionNavigationInfo = new RegionNavigationInfo(AppRegions.BottomRegion, navigationInfo);
            this.NavigateInMainShell(regionNavigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToChangeLanguage()
        {
            var navigationInfo = new NavigationInfo(nameof(ChangeLanguageView));
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToChangePassword()
        {
            var navigationInfo = new NavigationInfo(nameof(ChangePasswordView));
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToConfirmAccountPassword(NavigationInfo nextView, bool isDialog = true)
        {
            var navigationParameter = new NavigationParameters()
            {
                { NavigationParameterKeys.NextViewInfo, nextView },
            };
            var navigationInfo = new NavigationInfo(nameof(ConfirmAccountPasswordView), navigationParameter);
            this.NavigateInDialogShell(navigationInfo, isDialog);
        }

        /// <inheritdoc/>
        /// <remarks>The view opens in the main shell.</remarks>
        public virtual void ToContentPanel(string filePath)
        {
            var navigationParameter = new NavigationParameters()
            {
                { NavigationParameterKeys.FilePath, filePath },
            };
            var navigationInfo = new NavigationInfo(nameof(ContentPanelView), navigationParameter);
            var regionNavigationInfo = new RegionNavigationInfo(AppRegions.ContentRegion, navigationInfo);
            this.NavigateInMainShell(regionNavigationInfo);
        }

        /// <inheritdoc/>
        /// <remarks>You will need to confirm the password before is the registration without key saving.</remarks>
        public virtual void ToKeyRegistration(string userName, bool isSaveKey)
        {
            var navigationParameters = new NavigationParameters()
            {
                { NavigationParameterKeys.UserName, userName },
                { NavigationParameterKeys.IsSaveKey, isSaveKey },
            };

            var navigationInfo = new NavigationInfo(nameof(KeyRegistrationView), navigationParameters);
            if (isSaveKey)
            {
                this.NavigateInDialogShell(navigationInfo, false);
            }
            else
            {
                this.ToConfirmAccountPassword(navigationInfo, false);
            }
        }

        /// <inheritdoc/>
        /// <remarks>The view opens in the main shell.</remarks>
        public virtual void ToOpenFile()
        {
            var regionNavigationInfo = new RegionNavigationInfo(AppRegions.ContentRegion, nameof(OpenFileView));
            this.NavigateInMainShell(regionNavigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToReEncryptFiles(IEnumerable<string> filePaths, RsaKey key)
        {
            var navigationParameters = new NavigationParameters()
            {
                { NavigationParameterKeys.Key, key },
                { NavigationParameterKeys.FilePaths, filePaths },
            };
            var navigationInfo = new NavigationInfo(nameof(ReEncryptFilesView), navigationParameters);
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToSelectFiles(RsaKey key)
        {
            var navigationParameters = new NavigationParameters()
            {
                { NavigationParameterKeys.Key, key },
            };
            var navigationInfo = new NavigationInfo(nameof(SelectFilesView), navigationParameters);
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToSelectRecoveryFile()
        {
            var navigationInfo = new NavigationInfo(nameof(SelectRecoveryFileView));
            this.NavigateInDialogShell(navigationInfo);
        }

        /// <inheritdoc/>
        /// <remarks>The view will be opened in dialog window.</remarks>
        public virtual (SiteAuthData resultEntity, bool isChanged) ToShowEditRecord(SiteAuthData entity, EntityStates state)
        {
            (SiteAuthData resultEntity, bool isChanged) result = (null, true);
            var param = new DialogParameters
            {
                { DialogParameterKeys.Entity, entity },
                { DialogParameterKeys.State, state },
            };

            this.dialogService.ShowDialog(
                nameof(ShowEditRecordView),
                param,
                returns =>
                {
                    result.resultEntity = returns.Parameters.GetValue<SiteAuthData>(DialogParameterKeys.Entity);
                    result.isChanged = returns.Parameters.GetValue<bool>(DialogParameterKeys.IsChanged);
                },
                "UserRecords");

            return result;
        }

        /// <inheritdoc/>
        /// <remarks>The view opens in the main shell.</remarks>
        public virtual void ToTopPanel()
        {
            var regionNavigationInfo = new RegionNavigationInfo(AppRegions.TopRegion, nameof(TopPanelView));
            this.NavigateInMainShell(regionNavigationInfo);
        }

        /// <inheritdoc/>
        public virtual void ToUserRegistration()
        {
            var navigationInfo = new NavigationInfo(nameof(UserRegistrationView));
            this.NavigateInDialogShell(navigationInfo);
        }

        private void NavigateInDialogShell(NavigationInfo navigationInfo, bool isDialogMode = true)
        {
            if (this.IsRegionExistInManager(AppRegions.DialogShellContentRegion))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.RegionManager.RequestNavigate(AppRegions.DialogShellContentRegion, navigationInfo.ViewName, navigationInfo.Parameters);
                });
            }
            else
            {
                this.ShowDialogShell(navigationInfo, isDialogMode);
            }
        }

        private void NavigateInMainShell(RegionNavigationInfo navigationInfo)
        {
            if (this.IsRegionExistInManager(navigationInfo.RegionName))
            {
                this.RegionManager.RequestNavigate(
                    navigationInfo.RegionName,
                    navigationInfo.NavigationInfo.ViewName,
                    navigationInfo.NavigationInfo.Parameters);
            }
            else
            {
                string message = $"Couldn't open {navigationInfo.NavigationInfo.ViewName} in the main shell because the shell is not shows. Call ShowMainShell() method before navigation.";
                throw new InvalidOperationException(message);
            }
        }

        private void SetMainShellNavigation()
        {
            this.mainShellNavigation = new List<RegionNavigationInfo>()
            {
                new RegionNavigationInfo(AppRegions.TopRegion, nameof(TopPanelView)),
                new RegionNavigationInfo(AppRegions.BottomRegion, nameof(BottomPanelView)),
                new RegionNavigationInfo(AppRegions.ContentRegion, nameof(OpenFileView)),
            };
        }

        private void ShowDialogShell(NavigationInfo navigationInfo, bool isDialogMode = true)
        {
            this.shellService.ShowShell<DialogShell>(new RegionNavigationInfo(AppRegions.DialogShellContentRegion, navigationInfo), isDialogMode);
        }

        private bool IsRegionExistInManager(string regionName)
        {
            return this.RegionManager.Regions.ContainsRegionWithName(regionName);
        }
    }
}

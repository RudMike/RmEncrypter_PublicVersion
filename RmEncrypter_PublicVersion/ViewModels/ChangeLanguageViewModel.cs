// <copyright file="ChangeLanguageViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using Prism.Commands;
    using Prism.Regions;

    /// <summary>
    /// ViewModel for interaction with an changing language view.
    /// </summary>
    public class ChangeLanguageViewModel : ViewModelBase
    {
        private DelegateCommand<Languages?> acceptLanguageCommand;
        private DelegateCommand backCommand;
        private Languages chosenLanguage;
        private bool requestClose = false;
        private IRegionNavigationJournal journal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeLanguageViewModel"/> class.
        /// </summary>
        public ChangeLanguageViewModel()
        {
            this.ChosenLanguage = LocalizationService.Language;
        }

        /// <summary>
        /// Gets or sets chosen language of the program.
        /// </summary>
        public Languages ChosenLanguage
        {
            get => this.chosenLanguage;
            set => this.SetProperty(ref this.chosenLanguage, value);
        }

        /// <summary>
        /// Gets a value indicating whether the current window should be closed.
        /// </summary>
        public bool RequestClose
        {
            get => this.requestClose;
            private set => this.SetProperty(ref this.requestClose, value);
        }

        /// <summary>
        /// Gets command which go to previous view in the navigation journal.
        /// </summary>
        public DelegateCommand BackCommand
        {
            get => this.backCommand ??= new DelegateCommand(this.Back);
        }

        /// <summary>
        /// Gets the navigation journal.
        /// </summary>
        public IRegionNavigationJournal Journal
        {
            get => this.journal;
            private set => this.SetProperty(ref this.journal, value);
        }

        /// <summary>
        /// Gets the command for changing current language of the program and tries to navigate back.
        /// </summary>
        public DelegateCommand<Languages?> AcceptLanguageCommand
        {
            get => this.acceptLanguageCommand ??= new DelegateCommand<Languages?>(this.AcceptLanguage, this.IsLanguageChanged)
                .ObservesProperty(() => this.ChosenLanguage);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.journal = navigationContext.NavigationService.Journal;
        }

        private void AcceptLanguage(Languages? newLanguage)
        {
            LocalizationService.Language = newLanguage.Value;
            this.Back();
        }

        private bool IsLanguageChanged(Languages? obj)
        {
            return this.ChosenLanguage != LocalizationService.Language;
        }

        private void Back()
        {
            if (this.journal.CanGoBack)
            {
                this.journal.GoBack();
            }
            else
            {
                this.RequestClose = true;
            }
        }
    }
}

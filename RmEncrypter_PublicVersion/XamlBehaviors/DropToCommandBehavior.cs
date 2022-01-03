// <copyright file="DropToCommandBehavior.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Allows the behavior for an <see cref="UIElement"/>
    /// which converts <see cref="UIElement.Drop"/> event
    /// to a binded command and passing <see langword="IEnumerable{string}"/> with dropped files paths in.
    /// </summary>
    public class DropToCommandBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// The attached property which indicates on executable command when the drop event raised.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(DropToCommandBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// The attached property which indicates on a file extensions which can be dragged in.
        /// If it is <see langword="null"/> files with any extensions can be dragged in.
        /// </summary>
        public static readonly DependencyProperty ExtensionsProperty = DependencyProperty.Register(
            "Extensions",
            typeof(IEnumerable<string>),
            typeof(DropToCommandBehavior),
            new PropertyMetadata(null, OnExtensionsChanged));

        /// <summary>
        /// The attached property which indicates whether you can drag a single file or multiple files. Default value is <see langword="true"/>.
        /// </summary>
        public static readonly DependencyProperty IsSingleProperty = DependencyProperty.Register(
            "IsSingle",
            typeof(bool),
            typeof(DropToCommandBehavior),
            new PropertyMetadata(true));

        private IEnumerable<string> draggableFiles;

        /// <summary>
        /// Gets or sets file extensions which can be dragged in.
        /// If it is <see langword="null"/> files with any extensions can be dragged in.
        /// </summary>
        public IEnumerable<string> Extensions
        {
            get { return (IEnumerable<string>)this.GetValue(ExtensionsProperty); }
            set { this.SetValue(ExtensionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the executable command when the drop event raised.
        /// If <see cref="IsSingle"/> value is <see langword="true"/> it execute command with <see cref="string"/> parameter.
        /// Otherwise the parameter is <see cref="IEnumerable{String}">IEnumerable&lt;string&gt;</see>.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether you can drag a single file or multiple files.
        /// </summary>
        public bool IsSingle
        {
            get { return (bool)this.GetValue(IsSingleProperty); }
            set { this.SetValue(IsSingleProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Drop += this.AssociatedObject_Drop;
            this.AssociatedObject.DragOver += this.AssociatedObject_DragOver;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Drop -= this.AssociatedObject_Drop;
            this.AssociatedObject.DragOver -= this.AssociatedObject_DragOver;
            base.OnDetaching();
        }

        private static void OnExtensionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var extensions = (IEnumerable<string>)e.NewValue;
            var wrongExtensions = extensions.SkipWhile(extension => extension[0] == '.');

            if (!DesignerProperties.GetIsInDesignMode(d) && wrongExtensions.Any())
            {
                throw new FormatException("Wrong file extension. It must match the template dot with file extension (e.g., .exe).");
            }
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            bool isCanDrag;
            this.draggableFiles = e.Data.GetData(DataFormats.FileDrop, true) as IEnumerable<string>;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                isCanDrag = this.IsSingle ? this.SingleDragCheck() : this.MultipleDragCheck();
            }
            else
            {
                isCanDrag = false;
            }

            if (!isCanDrag)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private bool SingleDragCheck()
        {
            bool isCanDrag = true;
            if (this.draggableFiles.Count() == 1)
            {
                if (this.Extensions != null)
                {
                    isCanDrag = this.Extensions.Contains(
                        Path.GetExtension(this.draggableFiles.Single()),
                        StringComparer.OrdinalIgnoreCase);
                }
            }
            else
            {
                isCanDrag = false;
            }

            return isCanDrag;
        }

        private bool MultipleDragCheck()
        {
            bool isCanDrag = true;
            if (this.Extensions != null)
            {
                var filteredFilesCount = this.GetFilesByExtensionCount(this.draggableFiles);
                if (filteredFilesCount != this.draggableFiles.Count())
                {
                    isCanDrag = false;
                }
            }

            return isCanDrag;
        }

        private int GetFilesByExtensionCount(IEnumerable<string> filePaths)
        {
            var filteredFiles = filePaths.Join(
                this.Extensions,
                file => Path.GetExtension(file),
                extension => extension,
                (file, extension) => true,
                StringComparer.OrdinalIgnoreCase);

            return filteredFiles.Count();
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            object commandParameter = this.IsSingle ? this.draggableFiles.First() : (object)this.draggableFiles;
            if (this.Command != null && this.Command.CanExecute(commandParameter))
            {
                this.Command.Execute(commandParameter);
            }
        }
    }
}

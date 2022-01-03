// <copyright file="EventToCommandBehavior.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Allows the behavior for an <see cref="FrameworkElement"/>
    /// which converts from associated object's event to command with providing the event's event args.
    /// </summary>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// The attached property which indicates on the event name.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            "EventName",
            typeof(string),
            typeof(EventToCommandBehavior),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// The attached property which indicates on executable command when the event raised.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EventToCommandBehavior),
            new PropertyMetadata(null));

        private Delegate eventHandler;
        private EventInfo eventInfo;

        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        public string EventName
        {
            get { return (string)this.GetValue(EventNameProperty); }
            set { this.SetValue(EventNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the executable command when the event raised.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.RegisterEvent(this.EventName);
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            this.UnregisterEvent(this.EventName);
            base.OnDetaching();
        }

        private void RegisterEvent(string name)
        {
            this.eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            MethodInfo methodInfo = ((Action<object, object>)((sender, eventArgs) => this.OnEvent(sender, eventArgs))).Method;
            this.eventHandler = methodInfo.CreateDelegate(this.eventInfo.EventHandlerType, this);
            this.eventInfo.AddEventHandler(this.AssociatedObject, this.eventHandler);
        }

        private void UnregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name)
                || this.eventHandler == null)
            {
                return;
            }

            this.eventInfo.RemoveEventHandler(this.AssociatedObject, this.eventHandler);
            this.eventHandler = null;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            if (this.Command != null && this.Command.CanExecute(eventArgs))
            {
                this.Command.Execute(eventArgs);
            }
        }
    }
}

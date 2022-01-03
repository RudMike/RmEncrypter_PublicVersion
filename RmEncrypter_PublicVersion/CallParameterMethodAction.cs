// <copyright file="CallParameterMethodAction.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Reflection;
    using System.Windows;
    using Microsoft.Xaml.Behaviors.Core;

    /// <summary>
    ///  Calls a parameter method on a specified object when invoked.
    /// </summary>
    public class CallParameterMethodAction : CallMethodAction
    {
        /// <summary>
        /// Identifies the <see cref="Parameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(CallParameterMethodAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the passing in method parameter.
        /// </summary>
        public object Parameter
        {
            get { return (object)this.GetValue(ParameterProperty); }
            set { this.SetValue(ParameterProperty, value); }
        }

        /// <inheritdoc/>
        protected override void Invoke(object parameter)
        {
            object target = this.TargetObject ?? this.AssociatedObject;
            if (target == null)
            {
                return;
            }

            var methodInfo = this.FindMethod(ref target);
            this.TryCallMethod(methodInfo, target);
        }

        private MethodInfo FindMethod(ref object target)
        {
            var parameterType = this.Parameter.GetType();
            var methodInfo = target.GetType().GetMethod(this.MethodName, new[] { parameterType });
            if (methodInfo == null)
            {
                if (target is FrameworkElement frameworkElement)
                {
                    target = frameworkElement.DataContext;
                    methodInfo = frameworkElement.DataContext.GetType().GetMethod(this.MethodName, new[] { parameterType });
                }
            }

            return methodInfo;
        }

        private void TryCallMethod(MethodInfo methodInfo, object target)
        {
            if (methodInfo != null)
            {
                methodInfo.Invoke(target, new object[] { this.Parameter });
            }
            else
            {
                throw new ArgumentException($"Method {this.MethodName} with matching parameter not found in the type and in the data context.");
            }
        }
    }
}

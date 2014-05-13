﻿/*  
 * Papercut
 *
 *  Copyright © 2008 - 2012 Ken Robertson
 *  Copyright © 2013 - 2014 Jaben Cargman
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 */

namespace Papercut
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;

    using Autofac;

    using Papercut.Core;
    using Papercut.Core.Events;
    using Papercut.Helpers;

    public partial class App : Application
    {
        public const string GlobalName = "Papercut.App";

        public static string ExecutablePath = Assembly.GetExecutingAssembly().Location;

        Lazy<ILifetimeScope> _lifetimeScope =
            new Lazy<ILifetimeScope>(
                () => PapercutContainer.Instance.BeginLifetimeScope(PapercutContainer.UIScopeTag));

        static App()
        {
            // nothing can be called or loaded before this call is done.
            AssemblyResolutionHelper.SetupEmbeddedAssemblyResolve();
        }

        public ILifetimeScope Container
        {
            get
            {
                return _lifetimeScope.Value;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var publishEvent = Container.Resolve<IPublishEvent>();

            var appPreStartEvent = new AppPreStartEvent();
            publishEvent.Publish(appPreStartEvent);

            if (appPreStartEvent.CancelStart)
            {
                // force shut down...
                publishEvent.Publish(new AppForceShutdownEvent());
                return;
            }

            base.OnStartup(e);

            // startup app
            publishEvent.Publish(new AppReadyEvent());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Debug.WriteLine("App.OnExit()");

            using (Container)
            {
                Container.Resolve<IPublishEvent>().Publish(new AppExitEvent());
            }

            _lifetimeScope = null;
            base.OnExit(e);
        }
    }
}
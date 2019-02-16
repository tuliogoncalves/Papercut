﻿// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2019 Jaben Cargman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License. 

namespace Papercut.Infrastructure.Smtp
{
    using System;

    using Autofac;
    using Autofac.Core;

    using Papercut.Core.Annotations;
    using Papercut.Core.Infrastructure.Lifecycle;
    using Papercut.Core.Infrastructure.Plugins;

    using global::SmtpServer;
    using global::SmtpServer.Storage;

    [PublicAPI]
    public class PapercutSmtpModule : Module, IDiscoverableModule
    {
        public IModule Module => this;

        public Guid Id => new Guid("EB5FD401-FADE-4ADC-8FBB-78069BD85C38");

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmtpMessageStore>().As<MessageStore>().AsSelf();
            builder.RegisterType<SerilogSmtpServerLoggingBridge>().As<ILogger>();
            builder.RegisterType<PapercutSmtpServer>().AsSelf();
        }
    }
}
﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Test
{
    public sealed class GenerationTestsFixture : IDisposable
    {
        internal string TestRunPath = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\U\\{DateTime.Now.ToString("hhmm")}\\";
        internal string TestProjectsPath => Path.GetFullPath(Path.Combine(TestRunPath, "P"));
        
        private static readonly Lazy<TemplatesRepository> _repos = new Lazy<TemplatesRepository>(() => CreateNewRepos(), true);
        public static IEnumerable<ITemplateInfo> Templates => _repos.Value.GetAll();

        private static TemplatesRepository CreateNewRepos()
        {
            var source = new LocalTemplatesSource();

            CodeGen.Initialize(source.Id, "0.0");

            var repos = new TemplatesRepository(source, Version.Parse("0.0.0.0"));

            repos.SynchronizeAsync(true).Wait();

            return repos;
        }

        public GenerationTestsFixture()
        {
        }

        public void Dispose()
        {
            if (Directory.Exists(TestRunPath))
            {
                if (!Directory.Exists(TestProjectsPath) || Directory.EnumerateDirectories(TestProjectsPath).Count() == 0)
                {
                    Directory.Delete(TestRunPath, true);
                }
            }
        }
    }
}

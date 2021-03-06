﻿using System;
using System.IO;
using F2F.Sandbox;
using Leak.Common;
using Leak.Completion;
using Leak.Files;
using Leak.Meta.Info;
using Leak.Tasks;
using File = System.IO.File;

namespace Leak.Data.Store.Tests
{
    public class RepositoryFixture : IDisposable
    {
        private readonly LeakPipeline pipeline;
        private readonly CompletionThread completion;
        private readonly FileFactory files;

        public RepositoryFixture()
        {
            pipeline = new LeakPipeline();
            pipeline.Start();

            completion = new CompletionThread();
            completion.Start();

            files = new FileFactory(completion);
        }

        public RepositorySession Start()
        {
            Metainfo metainfo;
            RepositoryData data = new RepositoryData(20000);

            using (FileSandbox temp = new FileSandbox(new EmptyFileLocator()))
            {
                MetainfoBuilder builder = new MetainfoBuilder(temp.Directory);
                string path = temp.CreateFile("debian-8.5.0-amd64-CD-1.iso");

                File.WriteAllBytes(path, data.ToBytes());
                builder.AddFile(path);

                metainfo = builder.ToMetainfo();
            }

            FileSandbox sandbox = new FileSandbox(new EmptyFileLocator());
            string destination = Path.Combine(sandbox.Directory, metainfo.Hash.ToString());

            RepositoryService service =
                new RepositoryBuilder()
                    .WithHash(metainfo.Hash)
                    .WithDestination(destination)
                    .WithFiles(files)
                    .WithPipeline(pipeline)
                    .WithMemory(new RepositoryMemoryImplementation())
                    .Build();

            return new RepositorySession(metainfo, service, sandbox, data);
        }

        public void Dispose()
        {
            completion.Dispose();
            pipeline.Stop();
        }
    }
}
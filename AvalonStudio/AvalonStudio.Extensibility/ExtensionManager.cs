﻿using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Extensibility
{
    public sealed class ExtensionManager
    {
        private const string ExtensionManifestFilename = "extension.json";

        private IEnumerable<IExtensionManifest> _installedExtensions;

        private ExtensionManager() { }

        public static void Initialise()
        {
            if (IoC.Get<ExtensionManager>() == null)
            {
                var extensionManager = new ExtensionManager();
                IoC.RegisterConstant(extensionManager);
            }
        }

        public IEnumerable<IExtensionManifest> GetInstalledExtensions()
        {
            if (_installedExtensions == null)
            {
                _installedExtensions = LoadExtensions();
            }

            return _installedExtensions;
        }

        private List<IExtensionManifest> LoadExtensions()
        {
            var extensions = new List<IExtensionManifest>();

            foreach (var directory in Directory.GetDirectories(Platform.ExtensionsFolder))
            {
                var extensionManifest = Path.Combine(directory, ExtensionManifestFilename);

                if (File.Exists(extensionManifest))
                {
                    try
                    {
                        var extension = new ExtensionManifest(extensionManifest);
                        extensions.Add(extension);
                    }
                    catch (Exception e)
                    {
                        // todo: log exception
                    }
                }
            }

            return extensions;
        }
    }
}

using Microsoft.Extensions.Logging;
using MyCompany.NServiceBus.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.Services
{
    public class RegisterService : IRegisterService
    {
        //private readonly ILogger<RegisterService> _logger;
        private IDictionary<string, Type> _contractRegistration = new Dictionary<string, Type>();
        private IDictionary<string, List<string>> _dynamicStorages = new Dictionary<string, List<string>>();
        FileSystemWatcher folderWatcher;
        string obsoluteContractPath = "";

        public RegisterService(string obsoluteContractPath)
                               // ILogger<RegisterService> logger)
        {
            //_logger = logger;
            this.obsoluteContractPath = obsoluteContractPath;
            LoadContractFromDirectory(this.obsoluteContractPath);
            RegisterContractWatcher(this.obsoluteContractPath);
        }

        public Type FindContract(string contractKey)
        {
            _contractRegistration.TryGetValue(contractKey, out Type type);
            return type;
        }

        private void LoadContractFromDirectory(string folder)
        {
            var pathToFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + folder;
            if (Directory.Exists(pathToFolder))
            {
                foreach(var file in Directory.EnumerateFiles(pathToFolder,"*.dll", SearchOption.AllDirectories))
                {
                    LoadContract(file);
                }
            }
            else
            {
                Directory.CreateDirectory(pathToFolder);
            }
        }

        private void LoadContract(string filePath)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(filePath);
                var types = assembly.GetTypes();
                if(types.Length > 0)
                {
                    var contractStorage = new List<string>();
                    foreach (var busType in types)
                    {
                        var key = busType.GetAttributeValue((ContractAttribute cat) => cat.Command);
                        if (key != null)
                        {
                            _contractRegistration.Add(key, busType);
                            if (!contractStorage.Contains(key))
                            {
                                contractStorage.Add(key);
                            }
                        }
                    }
                    _dynamicStorages.Add(filePath, contractStorage);
                }
                // _logger.LogInformation(@"Load contracts from: @c", filePath);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
            }
        }

        private void RegisterContractWatcher(string folder)
        {
            var pathToFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + folder;
            folderWatcher = new FileSystemWatcher() { Path = pathToFolder};

            folderWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess
                                | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            folderWatcher.Filter = "*.dll";
            folderWatcher.EnableRaisingEvents = true;
            folderWatcher.IncludeSubdirectories = true;

            folderWatcher.Changed += new FileSystemEventHandler(OnContractChanged);
            folderWatcher.Created += new FileSystemEventHandler(OnContractCreated);
            folderWatcher.Deleted += new FileSystemEventHandler(OnContractDeleted);
        }

        private void OnContractDeleted(object sender, FileSystemEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void OnContractCreated(object sender, FileSystemEventArgs e)
        {
            LoadContract(e.FullPath);
        }

        private void OnContractChanged(object sender, FileSystemEventArgs e)
        {
            UnloadContract(e.FullPath);
        }

        private void UnloadContract(string filePath)
        {
            try
            {
                if (!_dynamicStorages.ContainsKey(filePath))
                {
                    return;
                }

                _dynamicStorages.TryGetValue(filePath, out List<string> contractKeys);

                foreach (var key in contractKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (_contractRegistration.ContainsKey(key))
                        {
                            _contractRegistration.Remove(key);
                        }
                    }
                }
                _dynamicStorages.Remove(filePath);
                // _logger.LogInformation(@"Remove contracts from: @c", filePath);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
            }
        }
    }
}

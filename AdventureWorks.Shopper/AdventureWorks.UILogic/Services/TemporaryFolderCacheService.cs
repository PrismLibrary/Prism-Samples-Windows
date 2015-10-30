using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AdventureWorks.UILogic.Services
{
    /// <summary>
    /// This cache service is a simple service that stores data into the application's TemporaryFolder for cache purposes
    /// and offline retrieval. This service does not fully implement a comprehensive expiration/eviction policy that you 
    /// would expect from a production cache service.
    /// </summary>
    public class TemporaryFolderCacheService : ICacheService
    {
        private static readonly StorageFolder _cacheFolder = ApplicationData.Current.TemporaryFolder;
        private static TimeSpan _expirationPolicy = new TimeSpan(0, 5, 0); // 5 minutes

        // We remember the most recently started task for each cache key, since only one I/O operation at a time may 
        // access that key. Cache read and write operations always wait for the prior task of the current cache key
        // to complete before they start. 
        // For a more general purpose approach to protecting read/write operations, please read this blog post by Stephen Toub
        // http://go.microsoft.com/fwlink/?LinkID=288843
        private Dictionary<string, Task> _cacheKeyPreviousTask = new Dictionary<string, Task>();

        public async Task<T> GetDataAsync<T>(string cacheKey)
        {
            await CacheKeyPreviousTask(cacheKey);

            ////throw new FileNotFoundException();

            // TODO: fix issue with serializer on Phone
            var result = GetDataAsyncInternal<T>(cacheKey);
            SetCacheKeyPreviousTask(cacheKey, result);
            return await result;
        }

        public async Task SaveDataAsync<T>(string cacheKey, T content)
        {
            await CacheKeyPreviousTask(cacheKey);

            // TODO: fix issue with serializer on Phone
            var result = SaveDataAsyncInternal<T>(cacheKey, content);
            SetCacheKeyPreviousTask(cacheKey, result);
            await result;
        }

        private static T Deserialize<T>(string json)
        {
            var jsonBytes = Encoding.Unicode.GetBytes(json);
            using (var jsonStream = new MemoryStream(jsonBytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var toReturn = (T)serializer.ReadObject(jsonStream);

                return toReturn;
            }
        }

        private static string Serialize<T>(T entity)
        {
            var stream = new MemoryStream();
            StreamReader streamReader = null;
            try
            {
                var serializer = new DataContractJsonSerializer(entity.GetType());
                serializer.WriteObject(stream, entity);

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
                else
                {
                    stream.Dispose();
                }
            }
        }

        private async Task<T> GetDataAsyncInternal<T>(string cacheKey)
        {
            StorageFile file = await _cacheFolder.GetFileAsync(cacheKey);
            if (file == null)
            {
                throw new FileNotFoundException("File does not exist");
            }

            // Check if the file has expired
            var fileBasicProperties = await file.GetBasicPropertiesAsync();
            var expirationDate = fileBasicProperties.DateModified.Add(_expirationPolicy).DateTime;
            bool fileIsValid = DateTime.Now.CompareTo(expirationDate) < 0;
            if (!fileIsValid)
            {
                throw new FileNotFoundException("Cache entry has expired.");
            }

            string text = await FileIO.ReadTextAsync(file);
            var toReturn = Deserialize<T>(text);

            return toReturn;
        }

        private async Task SaveDataAsyncInternal<T>(string cacheKey, T content)
        {
            StorageFile file = await _cacheFolder.CreateFileAsync(cacheKey, CreationCollisionOption.ReplaceExisting);

            // TODO: figure out why serializing ReadOnlyCollection<Category> fails
            var textContent = Serialize<T>(content);
            await FileIO.WriteTextAsync(file, textContent);
        }

        // Note: This method assumes that we are controlling the interleaving of async methods on a single thread.
        private async Task CacheKeyPreviousTask(string cacheKey)
        {
            if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
            {
                Task previousTask = null;

                // Wait for prior I/O task to complete. The while loop ensures that if multiple operations wait 
                // for the same task, the operations will be run one after another.
                while (_cacheKeyPreviousTask[cacheKey] != previousTask)
                {
                    previousTask = _cacheKeyPreviousTask[cacheKey];
                    try
                    {
                        await previousTask;
                    }
                    catch (Exception)
                    {
                        // Catch all exceptions; we can continue regardless of the exception status of the prior task.
                        // This exception will be handled by the original creator of the task.
                    }
                }
            }
        }

        // Note: This method assumes that we are controlling the interleaving of async methods on a single thread.
        // We require fully synchronous execution between the return of "await CacheKeyPreviousTask(key)" and the invocation of this method
        // with argument "key".
        private void SetCacheKeyPreviousTask(string cacheKey, Task task)
        {
            if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
            {
                _cacheKeyPreviousTask[cacheKey] = task;
            }
            else
            {
                _cacheKeyPreviousTask.Add(cacheKey, task);
            }
        }
    }
}

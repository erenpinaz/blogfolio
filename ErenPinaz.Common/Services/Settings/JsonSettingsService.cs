using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace ErenPinaz.Common.Services.Settings
{
    /// <summary>
    /// Json IO implementation of <see cref="ISettingsService" />
    /// </summary>
    public class JsonSettingsService : ISettingsService
    {
        public string SettingsFilePath => HttpContext.Current.Server.MapPath("~/App_Data/Settings/");

        /// <summary>
        /// De-serializes and returns the content of the
        /// specified json settings file
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>A <see cref="TEntity" /> settings class</returns>
        public TEntity GetByName<TEntity>(string fileName) where TEntity : class
        {
            var file = Path.Combine(SettingsFilePath, fileName + ".json");

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileContent = File.ReadAllText(file);
            var json = JsonConvert.DeserializeObject<TEntity>(fileContent);

            return json;
        }

        /// <summary>
        /// Serializes and writes given values to specified
        /// json settings file
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="settings"></param>
        /// <param name="fileName"></param>
        public void Save<TEntity>(TEntity settings, string fileName) where TEntity : class
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            var file = Path.Combine(SettingsFilePath, fileName + ".json");

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            File.WriteAllText(file, json);
        }
    }
}
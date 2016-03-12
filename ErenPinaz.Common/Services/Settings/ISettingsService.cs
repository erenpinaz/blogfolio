namespace ErenPinaz.Common.Services.Settings
{
    /// <summary>
    ///     Interface for storing managing settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        ///     De-serializes and returns the content of the
        ///     specified json settings file
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>A <see cref="TEntity" /> settings class</returns>
        TEntity GetByName<TEntity>(string fileName) where TEntity : class;

        /// <summary>
        ///     Serializes and writes given values to specified
        ///     json settings file
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="setting"></param>
        /// <param name="fileName"></param>
        void Save<TEntity>(TEntity setting, string fileName) where TEntity : class;
    }
}
namespace Piloteer
{
    public interface IAppSettingsProvider
    {
        /// <summary>
        /// Gets the value of an app setting array. If the value is not set, it returns default(T).
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="key">Name of the section containing the array</param>
        /// <returns>All array elements as T</returns>
        T[]? GetOptionalAppSettingsArray<T>(string key);

        /// <summary>
        /// Gets the value of an app setting array. If the value is not set, it will throw an exception.
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="key">Name of the section containing the array</param>
        /// <returns>All array elements as T</returns>
        T[]? GetRequiredAppSettingsArray<T>(string key);

        /// <summary>
        /// Gets the value of an app setting. If the value is not set, it returns null.
        /// </summary>
        /// <param name="key">Key of the app setting</param>
        /// <returns>Value of the app setting</returns>
        string? GetOptionalAppSettingsValue(string key);

        /// <summary>
        /// Gets the value of an app setting. If the value is not set, it will throw an exception.
        /// </summary>
        /// <param name="key">Key of the app setting</param>
        /// <returns>Value of the app setting</returns>
        string GetRequiredAppSettingsValue(string key);
    }
}

namespace Saving
{
    /// <summary>
    /// это штука, которая может сохранять хуйни
    /// </summary>
    /// <typeparam name="T">тип, в котором будет сохраняться объект, например <code>string</code></typeparam>
    public interface IFileSaver<T>
    {
        /// <summary>
        /// хуйня, которую может сохранять <code>IFileSaver</code>
        /// </summary>
        public interface ISavable
        {
            /// <summary>
            /// произвести превращение в тип <c>T</c>
            /// </summary>
            /// <returns>объект типа <c>T</c>, должна быть возможность обратного превращения с помощью <c>ISavable.Deconvert()</c></returns>
            T Convert();

            /// <summary>
            /// произвести обратное превращение объекта типа <c>T</c> в ISavable
            /// </summary>
            /// <param name="converted">объект типа <c>T</c> для обратного превращения</param>
            /// <param name="saver">штука, которая может сохранять хуйни типа <c>T</c></param>
            /// <returns></returns>
            ISavable Deconvert(T converted, IFileSaver<T> saver);
        }

        /// <summary>
        /// сохранить объект ISavable куда-то, например на диск или в облако хз
        /// </summary>
        /// <param name="savable">хуйня для сохранения</param>
        void Save(ISavable savable);

        /// <summary>
        /// получит объект типа <c>T</c> с диска или облака хз
        /// </summary>
        /// <param name="path">путь до объекта типа <c>T</c>, хз может кому надо</param>
        /// <returns>объект типа <c>T</c></returns>
        T Read(string path);
    }
}
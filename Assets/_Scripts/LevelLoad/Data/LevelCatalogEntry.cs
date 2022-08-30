using System;
using Common.Enum;
using Utilities;

namespace LevelLoad.Data
{
    [Serializable]
    public class LevelCatalogEntry
    {
        public string LevelName;
        public ContentType ContentType;

        #region Properties

        public string ContentPath => $"{Constants.Path.Level.ContentTypeToString[ContentType]}{LevelName}";

        #endregion
    }
}
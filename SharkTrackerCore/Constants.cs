namespace SharkTrackerCore
{
    public static class Constants
    {
        // ATTRIBUTES
#if DEBUG
        public const string PATH_CACHE_ARTWORK = @"../../../Data/en_us/img/cards/";

        public const string PATH_CARD_SET_1 = @"../../../Data/en_us/data/set1-en_us.json";

        public const string PATH_CARD_SET_2 = @"../../../Data/en_us/data/set2-en_us.json";

        public const string PATH_CARD_SET_3 = @"../../../Data/en_us/data/set3-en_us.json";

        public const string PATH_COLLECTION = @"../../../Data/en_us/data/collection.json";

        public const string PATH_USER_RESOURCES = @"../../../Data/en_us/data/user_resources.json";

#elif RELEASE
        public const string PATH_CACHE_ARTWORK = @"./img/cards/";

        public const string PATH_CARD_SET_1 = @"set1-en_us.json";

        public const string PATH_CARD_SET_2 = @"set2-en_us.json";

        public const string PATH_CARD_SET_3 = @"set3-en_us.json";

        public const string PATH_COLLECTION = @"collection.json";

        public const string PATH_USER_RESOURCES = @"user_resources.json";
#endif

        public const string ARTWORK_SUFFIX = @".png";

        public const string URL_DL_CARD_IMG_START = @"https://dd.b.pvp.net/latest/set";

        public const string URL_DL_CARD_IMG_END = @"/en_us/img/cards/";
    }
}
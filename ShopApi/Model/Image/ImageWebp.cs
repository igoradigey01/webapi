namespace ShopAPI.Model;

   public struct ImageWebp
    {
        private readonly string _syffics;
        private readonly int _width;
        private readonly int _quality;


        public ImageWebp(string syffics, int width, int quality)
        {
            _syffics = syffics;
            _width = width;
            _quality = quality;

        }
        public readonly string Syffics { get { return _syffics; } }
        public readonly int Width { get { return _width; } }
        public readonly int Quality { get { return _quality; } }

        public readonly string WebpPreffic { get { return ".webp"; } }

    }


 
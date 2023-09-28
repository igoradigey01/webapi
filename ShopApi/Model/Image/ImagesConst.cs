namespace ShopAPI.Model;

    /// <summary>
    /// CONST FOM SAVE IMG AND SRC PICTURE 
    /// </summary>
    public struct ImageConst
    {
        public readonly ImageWebp Small { get { return new ImageWebp("S", 200, 85); } }
        public readonly ImageWebp Medium { get { return new ImageWebp("M", 640, 85); } }
        public readonly ImageWebp Lagre { get { return new ImageWebp("L", 1080, 95); } }


    }
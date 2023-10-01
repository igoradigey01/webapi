using ImageMagick;

namespace ShopAPI.Model;

    public class ImageRepository
    {

        private readonly string _imgDir;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ImageConst _syfficsImg = new();


        // dir host img wwwroot/images
        private string GetImgPaht
        {
            get
            {
                string wwwroot = _hostingEnvironment.WebRootPath;
                return System.IO.Path.Combine(wwwroot, _imgDir);
            }
        }
    
        public string RamdomName
        {
            get
            {
                var name = Guid.NewGuid().ToString();

                return name;
            }
        }

        


        public ImageRepository(IWebHostEnvironment environment)
        {
            _imgDir = "images";
            _hostingEnvironment = environment;

        }

      

        public void Save(string imgName, Stream fileStream)
        {


            ResizeAndSave(imgName, fileStream);


        }
       
   
        public void Update(string imgName, Stream fileStream)
        {
            ResizeAndSave(imgName, fileStream);
        }

        
        public void Delete(string imgName)
        {
            IEnumerable<string> imgList()
            {
                yield return _syfficsImg.Lagre.Syffics + imgName + ".webp";
                yield return _syfficsImg.Medium.Syffics + imgName + ".webp"; // Can be executed
                yield return _syfficsImg.Small.Syffics + imgName + ".webp";

            }

            foreach (var i in imgList())
            {


                var imgPath = System.IO.Path.Combine(GetImgPaht, i);
                var fileInf = new FileInfo(imgPath);

                try
                {
                    if (fileInf.Exists)
                    {
                        File.Delete(imgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---UploadImageRepository----Ошибка Delete file {imgPath}");
                    Console.WriteLine(ex.Message);
                }
            }
        }
             
        private void ResizeAndSave(string name, Stream stream)
        {

            using var image = new MagickImage(stream);
            var pathLWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Lagre.Syffics + name + _syfficsImg.Lagre.WebpPreffic);

            using var img = image.Clone();


            img.Write(pathLWebp);

            var pathMWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Medium.Syffics + name + _syfficsImg.Medium.WebpPreffic);
            //image.Resize()


            // imgM.Format = MagickFormat.WebM;
            int heightM = image.Width / image.Height * _syfficsImg.Medium.Width;
            img.Resize(_syfficsImg.Medium.Width, heightM);
            img.Strip();
            img.Quality = _syfficsImg.Medium.Quality;

            img.Write(pathMWebp);


            var pathSWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Small.Syffics + name + _syfficsImg.Small.WebpPreffic);
            // imgS.Format = MagickFormat.WebM;
            //image.Resize()
            var height = image.Width / image.Height * _syfficsImg.Small.Width;
            img.Resize(_syfficsImg.Small.Width, height);
            img.Strip();
            img.Quality = _syfficsImg.Medium.Quality;
            img.Write(pathSWebp);
        }

        private void WriteNewFile(string pathPhoto, byte[] img)
        {

        using FileStream f = File.Create(pathPhoto);

        f.Write(img);
        f.Flush();


    }

      
        /// <summary>
        /// template for capasity email or ....
        /// </summary>
        /// <param name="capacity"></param>
        private void CapacityImg(string capacity)
        {
            string InputImagePath = ""; // project.Variables["InputImagePath"].Value;
            string SaveImagePath = ""; // project.Variables["SaveImagePath"].Value;
        using MagickImage image = new MagickImage(InputImagePath);
        MagickReadSettings readSettings = new MagickReadSettings
        {
            FillColor = MagickColors.Blue, // цвет текста
            BackgroundColor = MagickColors.Transparent, // фон текста
            Font = "Arial", // Шрифт текста (только те, что установлены в Windows)
            Width = 350, // Ширина текста
            Height = 500
        }; // Высота текста
        image.Alpha(AlphaOption.Opaque);
        
        using MagickImage label = new MagickImage("label:Тут какой то текст", readSettings);
        image.Composite(label, 200, 100, CompositeOperator.Over); // расположение текста на картинке 200 слева, 100 сверху
        image.Write(SaveImagePath);
    }

    }

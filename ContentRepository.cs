using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace DAL
{
    public class ContentRepository
    {
        public List<Image> List()
        {
            var ctx = new NewsEntities();
            return ctx.Image.ToList();
        }
        public bool Delete(int id)
        {
            Image ımage = new Image();
            var ctx =new NewsEntities();
            ımage = ctx.Image.FirstOrDefault(x => x.Id == id);
            ctx.Image.Remove(ımage);
            ctx.SaveChanges();
            return true;
        }

        public Image GetId(int id)
        {
            Image ımage = new Image();
            var ctx = new NewsEntities();
            ımage = ctx.Image.FirstOrDefault(x => x.Id == id);
            return ımage;
        }
        public Image GetName(string name)
        {
            Image ımage = new Image();
            var ctx = new NewsEntities();
            ımage = ctx.Image.FirstOrDefault(x => x.Name == name);
            return ımage;
        }

        public int UploadImageInDataBase(HttpPostedFileBase file, Image image)
        {
            image.Image1 = ConvertToBytes(file);
            var images = new Image
            {
                Name = image.Name,
                FileUrl = image.FileUrl,
                Image1 = image.Image1
            };
            var ctx = new NewsEntities();
            ctx.Image.Add(images);
            int i = ctx.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImageShare.Data
{
   public class ImagesRepository
    {
        private readonly string _connectionString;

        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Image image)
        {
            using var context = new ImageDbContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public List<Image> GetImages()
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.ToList();
        }

        public Image GetImageById(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public void AddLikes(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"update Images Set Likes = Likes + 1 WHERE Id = {id}");
        }

        public int GetLikes(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.Where(i => i.Id == id).Select(i => i.Likes).FirstOrDefault();
        }
    }
}

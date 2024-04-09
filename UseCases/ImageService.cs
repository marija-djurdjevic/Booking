using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

//Service that serves for saving images to the project, deleting, obtaining absolute paths from relative ones.
namespace BookingApp.UseCases
{
    public class ImageService
    {
        private static string projectAbsolutePath;

        public ImageService()
        {
            string projectPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            projectAbsolutePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(projectPath)));
        }
        public static string GetAbsolutePath(string relativePath)
        {
            if(relativePath == null)
            {
                return null;
            }
            string destinationFilePath = System.IO.Path.Combine(projectAbsolutePath, relativePath);
            return destinationFilePath;
        }

        //saves images in a folder within the project and returns a list of relative paths.
        public List<string> SaveImages(string []images,string relativPath)
        {
            List<string> imageRelativePaths = new List<string>();
            foreach(string image in images)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image);

                string destinationFilePath = System.IO.Path.Combine(GetAbsolutePath(relativPath), uniqueFileName);

                System.IO.File.Copy(image, destinationFilePath);

                imageRelativePaths.Add(System.IO.Path.Combine(relativPath,uniqueFileName));
            }
            return imageRelativePaths;
        }

        public static void DeleteImage(string absoluteImagePath)
        {
            //string destinationFilePath = GetAbsolutePath(relativeImagePath);

            if (System.IO.File.Exists(absoluteImagePath))
            {
                System.IO.File.Delete(absoluteImagePath);
            }
        }
    }
}

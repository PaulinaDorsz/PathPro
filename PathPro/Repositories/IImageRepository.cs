using PathPro.Migrations;
using PathPro.Models.Domain;
using System.Threading.Tasks;

namespace PathPro.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}

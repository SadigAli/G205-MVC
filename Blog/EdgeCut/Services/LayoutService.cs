using EdgeCut.DAL;
using EdgeCut.Models;

namespace EdgeCut.Services
{
    public class LayoutService
    {
        private readonly ApplicationContext _context;
        public LayoutService(ApplicationContext context)
        {
            _context = context;
        }
        public Setting GetSetting()
        {
            return _context.Settings.FirstOrDefault();
        }
    }
}

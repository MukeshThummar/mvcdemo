using empmgmt.Data;
using empmgmt.Models;
using empmgmt.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace empmgmt.Controllers
{
    public class SkillController : Controller
    {
        private readonly AppDBContext _context;

        public SkillController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var skill = await _context.skills.ToListAsync();
            return View(skill);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SkillViewModel skillPayload)
        {
            skills skill = new skills
            {
                name = skillPayload.name
            };
            await _context.skills.AddAsync(skill);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var skill = await _context.skills.FirstOrDefaultAsync(x => x.id == id);
            if (Equals(skill, null))
                return RedirectToAction("Index");

            SkillViewModel skillView = new SkillViewModel
            {
                id = skill.id,
                name = skill.name
            };

            return View(skillView);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SkillViewModel skillPayload)
        {
            skills skill = new skills
            {
                id = skillPayload.id,
                name = skillPayload.name
            };
            _context.skills.Update(skill);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _context.skills.FirstOrDefaultAsync(x => x.id == id);
            if (Equals(skill, null))
                return RedirectToAction("Index");

            _context.skills.Remove(skill);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

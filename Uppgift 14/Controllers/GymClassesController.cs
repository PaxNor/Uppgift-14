using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uppgift_14.Data;
using Uppgift_14.Models;

namespace Uppgift_14.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: GymClasses
        public async Task<IActionResult> Index() 
        {
              return _context.GymClass != null ? 
                          View(await _context.GymClass.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
        }

        // booking
        [Authorize]
        public async Task<IActionResult> BookingToggle(int? id) 
        {
            if (id == null) return BadRequest();

            var userId = userManager.GetUserId(User);

            if(userId == null) return BadRequest();

            // method 1
            // get the selected gym class
            var selectedGymClass = await _context.GymClass.Include(a => a.AttendingMembers).FirstOrDefaultAsync(g => g.Id == id);
            if (selectedGymClass == null) return BadRequest();

            // find out if user is already attending the class
            var attending = selectedGymClass.AttendingMembers.FirstOrDefault(a => a.ApplicationUserId == userId);

            // method 2
            // Create a DbSet of ApplicationUserGymClass and search directly, then add / remove booking directly from this DbSet
            //
            // var attending = await _context.ApplicationUserGymClasses.FindAsync(userId, id);
        

            // book / unbook member to gym pass
            if(attending == null) {
                var booking = new ApplicationUserGymClass() {
                    ApplicationUserId = userId,
                    GymClassId = (int)id
                };

                // add the booking to the selected gym class
                selectedGymClass.AttendingMembers.Add(booking);
            }
            else {
                selectedGymClass.AttendingMembers.Remove(attending);
            }

            // update data base with changes
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: GymClasses/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .Include(a => a.AttendingMembers) // added this
                .FirstOrDefaultAsync(m => m.Id == id);

            if(gymClass == null) return NotFound(); 

            // new, use AutoMapper?
            var gymClassVM = new GymClassViewModel() {
                Name = gymClass.Name,
                StartTime = gymClass.StartTime,
                Duration = gymClass.Duration,
                EndTime = gymClass.EndTime,
                Description = gymClass.Description
            };

            foreach(var compositeKey in gymClass.AttendingMembers) {
                var name = await _context.ApplicationUsers
                                            .Where(i => i.Id == compositeKey.ApplicationUserId)
                                            .Select(n => n.FullName)
                                            .FirstOrDefaultAsync();

                // maybe return BadRequest
                if(name == null) throw new Exception("ApplicationUserId in gymClass not found in AppliationUsers");

                gymClassVM.AttendingMemberNames.Add(name);
            }

            return View(gymClassVM);
        }

        // GET: GymClasses/Create
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClass == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
            }
            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClass.Remove(gymClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
          return (_context.GymClass?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // moved from home controller
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error() {
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

    }
}

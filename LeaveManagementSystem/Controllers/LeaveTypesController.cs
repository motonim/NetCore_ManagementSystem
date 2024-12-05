using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService) : Controller
    {
        private const string NameExistsValidationMessage = "This leave type already exists in the database";
        //private readonly ILeaveTypesService _leaveTypesService = leaveTypesService;

        //public LeaveTypesController(ApplicationDbContext context, IMapper mapper) // it's a part of the pattern called dependency injection.
        //                                                          // 'context' here represents a connection to the database
        //{
        //    _context = context;
        //    this._mapper = mapper;
        //}

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            //// var data = SELECT * FROM LeaveTypes
            //var data = await _context.LeaveTypes.ToListAsync();
            ////return View(data);
            //// these two lines are equal to the next line(LINQ)
            ////return View(await _context.LeaveTypes.ToListAsync()); // LinQ

            //// convert the datamodel into a view model using AutoMapper
            //var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);

            //// return the view model to the view

            var viewData = await _leaveTypesService.GetAllAsync();
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //// Select * from LeaveTypes WHERE Id = @id
            //var leaveType = await _context.LeaveTypes // connects to the database(context) and go to the LeaveTypes table
            //    .FirstOrDefaultAsync(m => m.Id == id); // Lambda Expression
            //                                           // Parameterization - securely pass over ID. a key for preventing SQL injection attacks

            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);
            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            // custom error message
            //if(leaveTypeCreate.Name.Contains("vacation"))
            //{
            //    ModelState.AddModelError(nameof(leaveTypeCreate.Name), "Name should not contain vacation");
            //}

            // Adding custom validation and model state error
            if (await _leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                await _leaveTypesService.CreateAsync(leaveTypeCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate); // going back to create page but this time, let's send the data(leaveType) typed in as well
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Select * from LeaveTypes WHERE Id = @id
            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeEditVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType);

            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            // Adding custom validation and model state error
            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                    //_context.Update(leaveType);
                    await _leaveTypesService.EditAsync(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
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
            return View(leaveTypeEdit);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //    var leaveType = await _context.LeaveTypes.FindAsync(id);
            //    if (leaveType != null)
            //    {
            //        _context.LeaveTypes.Remove(leaveType);
            //    }

            //    await _context.SaveChangesAsync();
            await _leaveTypesService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TigerTix.Data;
using TigerTix.Data.Entities;
using TigerTix.Models;


namespace TigerTix.Controllers
{
    public class AppController : Controller
    {

        private readonly IHttpContextAccessor contxt;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;

        public AppController(IUserRepository userRepository,
                             IEventRepository eventRepository,
                             ITicketRepository ticketRepository,
                             IHttpContextAccessor httpcontextaccessor)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            contxt = httpcontextaccessor;
        }


        /* Index login controllers */

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            Console.WriteLine("System: Index called\n");

            return View();
        }

        public IActionResult GetUser(string userID)
        {
            var user = _userRepository.GetUserById(userID);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /* Login page controllers */

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(IndexViewModel inputUser)
        {
            var dbUser = _userRepository.GetUserById(inputUser.UserName);
            string id = dbUser.UserName;
            string pass = dbUser.FirstName;


            if (dbUser != null && inputUser.FirstName == dbUser.FirstName)
            {
                return RedirectToAction("Home");
            }

            Console.WriteLine(id + "\n");
            Console.WriteLine(pass + "\n");

            ModelState.AddModelError(string.Empty, "Invalid username or FirstName");

            return View(inputUser);
        }

        /* Create Account controllers */

        public IActionResult ShowUsers()
        {
            var results = from user in _userRepository.GetAllUsers()
                          select user;

            Console.WriteLine("System: ShowUsers called\n");
            return View(results.ToList());
        }


        [HttpPost("/App/ShowUsers")]
        public IActionResult AddUser(User user)
        {
            var userList = _userRepository.GetAllUsers();
            foreach (var u in userList)
            {
                if (u.UserName == user.UserName)
                {
                    ViewData["Message"] = $"CUID is taken";
                    return View("ShowUsers");
                }
            }
            _userRepository.SaveUser(user);
            _userRepository.SaveAll();
            Console.WriteLine("System: AddUser User user called\n");
            return RedirectToAction("Index");
        }

        public IActionResult UserAuthentication(string CUID, string Password)
        {
            var userList = _userRepository.GetAllUsers();
            var tempCUID = "";
            var tempPassword = "";

            bool isIdValid;

            // define pattern
            string pattern = @"C\d{8}$";
            // use regex to match pattern
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(CUID))
                Console.WriteLine("System: CUID did not match formatting.\n");

            foreach (var u in userList)
            {
                tempCUID = u.UserName;
                foreach (var j in userList)
                {
                    tempPassword = j.Password;

                    if (CUID == tempCUID && Password == tempPassword)
                    {
                        contxt.HttpContext.Session.SetString("CUID", CUID);
                        contxt.HttpContext.Session.SetString("FirstName", j.FirstName);
                        contxt.HttpContext.Session.SetString("LastName", j.LastName);
                        contxt.HttpContext.Session.SetString("Password", j.Password);
                        string temp = Convert.ToBase64String(j.userImage);
                        contxt.HttpContext.Session.SetString("UserImage", temp);

                        return RedirectToAction("Home");
                    }
                }
            }
            ViewData["Message"] = $"Password and/or username are incorrect";
            return View("Index");
        }


        /* Main page controllers */

        public IActionResult Home()
        {
            var events = _eventRepository.GetAllEvents();
            return View(events);
        }


        /* EVENT controlers */
        public IActionResult ShowEvents(int? id)
        {
            // checks if event has ID
            if (id.HasValue)
            {   // gets event by ID
                var eventToEdit = _eventRepository.GetEventById(id.Value);
                if (eventToEdit == null)
                {
                    return NotFound();
                }
                // stores event in the ViewBag
                ViewBag.EventToEdit = eventToEdit;
            }
            // gets all events from repository
            var events = _eventRepository.GetAllEvents();
            return View(events);
        }

        // [HttpPost]
        public IActionResult AddEvent(Event eventData)
        {   // saves new event to repo
            _eventRepository.SaveEvent(eventData);
            // saves changes to database
            _eventRepository.SaveAll();
            // redirects to display updated events
            return RedirectToAction("ShowEvents");
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost("/App/ShowEvents")]
        public IActionResult UpdateEvent(Event eventData)
        {
            if (eventData.Id == 0)
            {
                // checks if new event(id=0) and then adds it to repo
                _eventRepository.SaveEvent(eventData);
            }
            else
            {
                // get existing event from repo
                var existingEvent = _eventRepository.GetEventById(eventData.Id);
                if (existingEvent == null)
                {
                    return NotFound();
                }
                // delete existing event
                _eventRepository.DeleteEvent(existingEvent);
                // save to database
                _eventRepository.SaveAll();

                // Create a new event with the updated data (excluding the Id property)
                var newEvent = new Event
                {
                    EventName = eventData.EventName,
                    EventTime = eventData.EventTime,
                    EventDescription = eventData.EventDescription,
                    EventAddress = eventData.EventAddress,
                    EventCost = eventData.EventCost
                };
                // save to repo
                _eventRepository.SaveEvent(newEvent);
            }
            // save changes to database
            _eventRepository.SaveAll();
            // redirect to display list of updated events
            return RedirectToAction("ShowEvents");
        }

        // not currently in use
        public IActionResult EditEvent(int id)
        {   // get event to be edited based on ID
            var eventToEdit = _eventRepository.GetEventById(id);
            if (eventToEdit == null)
            {
                return NotFound();
            }
            // store event in ViewBag
            ViewBag.EventToEdit = eventToEdit;
            return View("ShowEvents", _eventRepository.GetAllEvents());
        }


        [HttpPost("/App/UserProfile")]
        public async Task<IActionResult> EditUser(User user, IFormFile useImage)
        {
            string CUID = contxt.HttpContext.Session.GetString("CUID");

            var userList = _userRepository.GetAllUsers();

            foreach (var u in userList)
            {
                if (u.UserName == user.UserName)
                {
                    ViewData["Message"] = $"CUID is taken";
                    return View("UserProfile");
                }
            }

            if (useImage != null && useImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await useImage.CopyToAsync(memoryStream);
                    user.userImage = memoryStream.ToArray();
                }
            }

            _userRepository.UpdateUser(user, CUID);
            _userRepository.SaveAll();

            return RedirectToAction("UserProfile");
        }

        public IActionResult DeleteEvent()
        {

            return View();
        }

        public IActionResult AddCustomer()
        {

            return View();
        }

        public IActionResult EditCutomer()
        {
            return View();
        }

        public IActionResult DeleteCustomer()
        {
            return View();
        }

        /* User Profile controllers */

        public IActionResult UserProfile()
        {
            return View();
        }

        /* Ticket controllers */

        public IActionResult ShowTickets()
        {
            var tickets = _ticketRepository.GetAllTickets();
            return View();
        }

        [HttpPost("/App/ShowTickets")]
        public IActionResult AddTicket(Ticket ticket)
        {
            // var ticketList = _ticketRepository.GetAllTickets();

            _ticketRepository.CreateTicket(ticket);
            _ticketRepository.SaveAll();
            return RedirectToAction("ShowTickets");
        }
    }

}

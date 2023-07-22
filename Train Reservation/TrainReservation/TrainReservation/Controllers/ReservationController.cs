using Microsoft.AspNetCore.Mvc;
using TrainReservation.Data;
using TrainReservation.Entities;
using TrainReservation.Models.ViewModel;
using TrainReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace TrainReservation.Controllers
{
    public class ReservationController : Controller
    {

        Repos<Train> _repos;
        ApplicationDbContext _context;
        public ReservationController(Repos<Train> trainRepos, ApplicationDbContext context)
        {
            _repos = trainRepos;
            _context = context;
        }

        public IActionResult List()
        {
            return View(_repos.GetAll());
        }

        [HttpPost]

        public IActionResult Add(ReservationRequest request, int Id)
        {
            Train train = _context.Trains.Include(t => t.Wagons).FirstOrDefault(t => t.Id == 1);

            var response = new ReservationResponse { RezervasyonYapilabilir = true, YerlesimAyrinti = new List<ReservationDetail>() };

            int remainingPassengers = request.RezervasyonYapilacakKisiSayisi;
            bool isNextWagonRequest = request.KisilerFarkliVagonlaraYerlestirilebilir;

            foreach (var vagon in train.Wagons)
            {

                int maxPassengerCapacity = (int)Math.Floor((vagon.Capacity * 0.7) - vagon.OccupiedSeats);


                if (maxPassengerCapacity <= 0)
                {
                    if (vagon.Id == train.Wagons.Count)
                        response.RezervasyonYapilabilir = false;
                    continue;
                }
                if (remainingPassengers >= maxPassengerCapacity)
                {
                    if (isNextWagonRequest)
                    {
                        response.YerlesimAyrinti.Add(new ReservationDetail { VagonAdi = vagon.Name, KisiSayisi = maxPassengerCapacity });

                        vagon.OccupiedSeats += maxPassengerCapacity;
                        remainingPassengers -= maxPassengerCapacity;

                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    response.YerlesimAyrinti.Add(new ReservationDetail { VagonAdi = vagon.Name, KisiSayisi = remainingPassengers });
                    vagon.OccupiedSeats += remainingPassengers;
                    remainingPassengers -= remainingPassengers;
                }
            }
            if (remainingPassengers > 0)
            {
                response.RezervasyonYapilabilir = false;
                response.YerlesimAyrinti.Clear();
            }
            else
            {
                _context.SaveChanges();
            }

            return RedirectToAction("List", response);
        }
    }
}

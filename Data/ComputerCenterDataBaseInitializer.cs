using Data.Model;
using System;
using System.Collections.Generic;

namespace Data
{
    public class ComputerCenterDatabaseInitializer :
        System.Data.Entity.CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            var c = new Course()
            {
                DateOpened = DateTime.Now,
                Description =
                    "Osnovne akademske studije",
                Id = "SW",
                Name = "Softversko inženjerstvo i informacione tehnologije",
            };

            var vs = new Software()
            {
                Id = "VS2015",
                Name = "Visual Studio 2015",
                Description = "",
                Maker = "Microsoft",
                Platform = Constants.Platform.Windows,
                MakerWebsite = "www.microsoft.com",
                YearReleased = "2015",
                Price = 1000
            };
            context.Softwares.Add(vs);

            var ij = new Software()
            {
                Id = "IJ",
                Name = "IntelliJ IDEA",
                Description = "",
                Maker = "Jet Brains",
                Platform = Constants.Platform.Both,
                MakerWebsite = "www.jetbrains.com",
                YearReleased = "2017",
                Price = 499
            };
            context.Softwares.Add(vs);

            var s = new Subject()
            {
                Name = "Pisana i govorna komunikacija u tehnici",
                Course = c,
                Description =
                    "Ciljne grupe komunikacije. Principi komunikacije. Sredstva komunikacije. Komunikacija u timu. Komunikacija sa korisnicima. Komunikacija pisane dokumentacije. Elektronska komunikacija. govorna komunikacija.",
                Id = "SES103",
                RequiredTerms = 10,
                MinimumTermsPerSession = 2,
                RequiredPlatform = Constants.Platform.Any,
                GroupSize = 16
            };
            context.Subjects.Add(s);

            s = new Subject()
            {
                Name = "Internet softverske arhitekture",
                Course = c,
                Description =
                    "Osposobljavanje studenata za dizajn i konstrukciju višeslojnih klijent/server sistema zasnovanih na tehnologijama distribuiranih objekata.",
                Id = "SEI41",
                RequiredTerms = 12,
                MinimumTermsPerSession = 3,
                RequiredPlatform = Constants.Platform.Any,
                RequiredSoftware = new List<Software>() { ij },
                GroupSize = 16
            };
            context.Subjects.Add(s);

            s = new Subject()
            {
                Name = "Interakcija čovek računar",
                Course = c,
                Description =
                    "Osposobljavanje studenata za projektovanje i implementaciju osnovnih nosilaca interakcije čovek računar.",
                Id = "E243",
                RequiredTerms = 8,
                MinimumTermsPerSession = 3,
                RequiredPlatform = Constants.Platform.Windows,
                GroupSize = 16
            };
            s.RequiredSoftware = new List<Software>() { vs };
            context.Subjects.Add(s);

            s = new Subject()
            {
                Name = "Programski prevodioci",
                Course = c,
                Description =
                    "Upoznavanje studenata sa principima rada kompajlera, konceptima prevođenja sa jednog programskog jezika na drugi, alatima za njihovo pravljenje i načinom njihove implementacije. Ovladavanje pravljenjem kompajlera na početničkom nivou.",
                Id = "SE0034",
                RequiredTerms = 11,
                MinimumTermsPerSession = 1,
                RequiredPlatform = Constants.Platform.Linux,
                GroupSize = 16
            };
            context.Subjects.Add(s);

            s = new Subject()
            {
                Name = "Metodologije razvoja softvera",
                Course = c,
                Description =
                    "Upoznavanje studenata sa životnim ciklusom softverskog proizvoda i različitim metodologijama, standardima i alatima koji podržavaju životni ciklus softverskog proizvoda u celini ili u nekoj od njegovih faza.",
                Id = "SE0017",
                RequiredTerms = 14,
                MinimumTermsPerSession = 4,
                RequiredPlatform = Constants.Platform.Any,
                GroupSize = 16
            };
            context.Subjects.Add(s);

            s = new Subject()
            {
                Name = "Osnovi računarske inteligencije",
                Course = c,
                Description =
                    "Ovladavanje osnovnim principima i tehnikama \"klasične\" veštačke inteligencije i mekog\" računarstva (soft computing).",
                Id = "E236A",
                RequiredTerms = 10,
                MinimumTermsPerSession = 2,
                RequiredPlatform = Constants.Platform.Windows,
                GroupSize = 8
            };
            context.Subjects.Add(s);

            context.Classrooms.Add(new Classroom()
            {
                Id = "CR1",
                Description = "Učionica 1",
                HasProjector = true,
                Platform = Constants.Platform.Windows,
                InstalledSoftware = new List<Software>() { vs },
                Seats = 15
            });

            context.Classrooms.Add(new Classroom()
            {
                Id = "CR2",
                Description = "Učionica 2",
                HasProjector = true,
                Platform = Constants.Platform.Linux,
                Seats = 20
            });

            context.Classrooms.Add(new Classroom()
            {
                Id = "CR3",
                Description = "Učionica 3",
                HasProjector = true,
                Platform = Constants.Platform.Both,
                Seats = 20,
                InstalledSoftware = new List<Software>() { ij, vs }
            });

            context.SaveChanges();
        }
    }
}
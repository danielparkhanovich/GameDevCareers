using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;

namespace JobBoardPlatform.BLL.Utils
{
    /// <summary>
    /// For tests only
    /// </summary>
    internal class JobOffersGenerator
    {
        private readonly string[] TitleSeniority = new string[] { "Trainee", "Junior", "Middle", "Senior", "Regular", "Lead" };
        private readonly string[] TitleGameplayPrefixTechnology = new string[] { "Unity", "Unreal Engine", "C#", "Rust", "C++", "Python Machine Learning" };
        private readonly string[] TitleGameplayPostfix = new string[] { "Developer", "Gameplay Programmer", "AI Programmer", "Programmer", "Engineer" };
        private readonly string[] TitleArtAndAnimation = new string[] { "3D Art", "3D Animator", "Animator" };
        private readonly string[] TitleDesign = new string[] { "Level Designer", "Quest Designer", "Game Designer", "Writer" };
        private readonly string[] TitleAudio = new string[] { "Audio", "Sound Designer" };
        private readonly string[] TitleTesting = new string[] { "QA Tester", "Game Tester", "QA Analyst" };
        private readonly string[] TitleManagement = new string[] { "PR & Marketing", "Producer", "Co-Founder", "Game Producer", "Technical Producer", "Associate Producer", "Executive Producer" };
        private readonly string[] TitleOther = new string[] { "Narrative Designer", "Creative Copywriter", "Community Manager", "Narrative Designer", "Data Specialist", "Translator" };

        private readonly string[] Countries = new string[] { "Poland", "United States", "United Kingdom", "France", "Germany" };
        private readonly string[] Cities = new string[] { "Warsaw", "New York", "London", "Paris", "Berlin" };
        private readonly string[] Addresses = new string[] { "3545 Lochmere Lane", "344 Pratt Avenue", "ul. Podskarbińska 89" };
        private readonly string Description = "\r\n  <hr>\r\n  <p>Hey everyone! We're Crunching Koalas and you may recognize us because of:</p>\r\n\r\n<ul>\r\n<li><a href=\"https://www.nintendo.com/games/detail/darkwood-switch/\">Darkwood</a>, <a href=\"https://www.nintendo.com/games/detail/darkwood-switch/\">Project Warlock</a>, <a href=\"https://www.nintendo.com/games/detail/butcher-switch/\">Butcher</a> or <a href=\"https://www.nintendo.com/games/detail/lichtspeer-double-speer-edition-switch/\">Lichtspeer</a>, so some of the games that we ported and published on consoles;</li>\r\n<li><a href=\"https://www.nintendo.com/games/detail/thronebreaker-the-witcher-tales-switch/\">Thronebreaker</a>, <a href=\"https://store.playstation.com/pl-pl/product/EP4361-CUSA15690_00-0000000000000003\">Frostpunk</a>, <a href=\"https://www.nintendo.com/games/detail/this-war-of-mine-complete-edition-switch/\">This War of Mine</a>, <a href=\"https://www.nintendo.com/games/detail/ruiner-switch/\">Ruiner</a>, so some of the titles we ported and brought to consoles;</li>\r\n<li>or <a href=\"https://store.steampowered.com/app/252750/MouseCraft/\">MouseCraft</a>, a game which we developed by ourselves.</li>\r\n</ul>\r\n\r\n<p>We are looking for Programmer to join our team.</p>\r\n\r\n<h3>Requirements:</h3>\r\n\r\n<ul>\r\n<li>2 years of experience on a similar position;</li>\r\n<li>good English;</li>\r\n<li>good knowledge of UE4/5 or C++;</li>\r\n<li>code samples or at least one released title;</li>\r\n</ul>\r\n\r\n<h3>Responsibilities:</h3>\r\n\r\n<p><strong>Porting games for all major platforms available:</strong></p>\r\n\r\n<ul>\r\n<li>Switch;</li>\r\n<li>PlayStation 5;</li>\r\n<li>Xbox Series X;</li>\r\n<li>PlayStation 4;</li>\r\n<li>Xbox One;</li>\r\n<li>Steam;</li>\r\n<li>Android;</li>\r\n<li>iOS; etc.</li>\r\n</ul>\r\n\r\n<p><strong>Co-developing games and game related software for Koala's publishing and our partners</strong>:</p>\r\n\r\n<ul>\r\n<li>implementing gameplay and engine elements;</li>\r\n<li>providing optimization services;</li>\r\n<li>creating digital soundtracks, themes, artbooks, etc. from the code side;</li>\r\n</ul>\r\n\r\n<p><strong>Providing code and console consultancy services</strong>:</p>\r\n\r\n<ul>\r\n<li>consulting porting topics for our publishing partners;</li>\r\n<li>creating tutorials and documentation for new console specific features we implement;</li>\r\n<li>sharing important knowledge with teammates in lecture sessions;</li>\r\n</ul>\r\n\r\n<h3>Pluses:</h3>\r\n\r\n<ul>\r\n<li>knowledge of any of the console's porting process;</li>\r\n<li>experience in one or more: rendering pipeline, shaders, sound, fmod, wwise, memory management;</li>\r\n<li>experience in creating multiplayer games;</li>\r\n</ul>\r\n\r\n<h3>What are we offering?</h3>\r\n\r\n<ul>\r\n<li>an opportunity to work on interesting and relevant projects (just check our portfolio or the 1st paragraph of this job offer);</li>\r\n<li>salary between 6.000 and 10.000 PLN net of tax (Umowa o pracę) depending on the experience and the contract type;</li>\r\n<li>available types of contracts: Umowa o pracę, B2B;</li>\r\n<li>Partially financed Medicover and Multisport</li>\r\n<li>core hours 11-14;</li>\r\n<li>possibility to work for a well-known, established company that knows where it wants to be.</li>\r\n</ul>\r\n\r\n<p>Cheers!</p>\r\n\r\n  <hr>";
        private readonly int[] SalaryRanges = new int[] { 4500, 7000, 13000, 25000, 30000, 40000 };
        private readonly string ExternalLink = "http://localhost:3000/";


        public IOfferData GenerateData(CompanyIdentity companyIdentity)
        {
            var random = new Random();

            var mainTechnology = GetRandomTechnology(random);
            var address = GetAddress(random);
            string? seniority = GetSeniority(random);
            var details = GetEmploymentDetails(seniority, random)!;
            var contact = GetContactType(companyIdentity, random);
            string title = GetTitle(mainTechnology, random, seniority);
            string[] techKeywords = title.Split();
            var workLocationType = GetWorkLocation(random);

            var newOfferData = new MockOfferData()
            {
                MainTechnologyType = (int)mainTechnology + 1,
                Country = address.Item1,
                City = address.Item2,
                Street = address.Item3,
                EmploymentTypes = details,
                ApplicationsContactType = (int)contact.Item1 + 1,
                ApplicationsContactEmail = contact.Item2,
                JobDescription = Description,
                JobTitle = title,
                TechKeywords = techKeywords,
                WorkLocationType = (int)workLocationType + 1,
            };

            return newOfferData;
        }

        private MainTechnologyTypeEnum GetRandomTechnology(Random random)
        {
            var mainTechnology = MainTechnologyTypeEnum.Programming;

            if (random.NextDouble() < 0.5)
            {
                mainTechnology = MainTechnologyTypeEnum.Programming;
            }
            else
            {
                var technologies = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                    .Cast<MainTechnologyTypeEnum>()
                    .ToList();
                int selected = random.Next(technologies.Count);
                mainTechnology = technologies[selected];
            }

            return mainTechnology;
        }

        private (string, string, string?) GetAddress(Random random)
        {
            int randomAddress = random.Next(Countries.Length);
            string country = Countries[randomAddress];
            string city = Cities[randomAddress];

            string? address = null;

            if (random.NextDouble() < 0.75)
            {
                address = null;
            }
            else
            {
                randomAddress = random.Next(Addresses.Length);
                address = Addresses[randomAddress];
            }

            return (country, city, address);
        }

        private string? GetSeniority(Random random)
        {
            string? seniority = null;
            if (random.NextDouble() < 0.75)
            {
                int randomIndex = random.Next(TitleSeniority.Length);
                seniority = TitleSeniority[randomIndex];
            }
            return seniority;
        }

        private EmploymentType[] GetEmploymentDetails(string? seniority, Random random)
        {
            // TODO: change to 0.25
            if (random.NextDouble() < 0)
            {
                return null;
            }

            var employmentTypes = Enum.GetValues(typeof(EmploymentTypeEnum))
                .Cast<EmploymentTypeEnum>()
                .ToList();

            int employmentTypesCount = random.Next(1, 4);
            var details = new List<EmploymentType>(employmentTypesCount);

            for (int i = 0; i < employmentTypes.Count; i++)
            {
                // setup salary range
                int fromIndex = 0;
                if (string.IsNullOrEmpty(seniority))
                {
                    fromIndex = random.Next(SalaryRanges.Length);
                }
                else
                {
                    fromIndex = Array.IndexOf(TitleSeniority, seniority);
                }

                int fromSalary = SalaryRanges[fromIndex];
                int toSalary = fromSalary;

                if (fromIndex + 1 < SalaryRanges.Length)
                {
                    toSalary = fromSalary + random.Next(10, 20) * 1_000;
                }

                // select employment type & modify salary range
                var employmentType = EmploymentTypeEnum.Permanent;

                if (random.NextDouble() < 0.5f)
                {
                    employmentType = EmploymentTypeEnum.B2B;
                    fromSalary = (int)(fromSalary * 1.5f);
                    toSalary = (int)(toSalary * 1.5f);
                }
                else if (random.NextDouble() < 0.6f)
                {
                    employmentType = EmploymentTypeEnum.MandateContract;
                }

                if (details.Any(x => x.TypeId == (int)employmentType + 1))
                {
                    var exclude = details.First(x => x.TypeId == (int)employmentType + 1);
                    var cleared = employmentTypes.Where(x => exclude.TypeId != (int)x + 1).ToArray();
                    int randomIndex = random.Next(cleared.Length);
                    employmentType = cleared[randomIndex];
                }

                // select currency & modify salary range
                var currency = CurrencyTypeEnum.PLN;

                if (random.NextDouble() < 0.5)
                {
                    currency = CurrencyTypeEnum.EUR;
                    fromSalary = (int)(fromSalary * 0.22f * 1.35f);
                    toSalary = (int)(toSalary * 0.22f * 0.35f);
                }

                details.Add(new MockEmploymentType()
                {
                    SalaryFromRange = fromSalary,
                    SalaryToRange = toSalary,
                    EmploymentType = (int)employmentType + 1,
                    SalaryCurrencyType = (int)currency + 1
                });
            }

            return details.ToArray();
        }

        private (ContactTypeEnum, string?) GetContactType(CompanyIdentity company, Random random)
        {
            ContactTypeEnum contactType = ContactTypeEnum.PrivateApplications;
            string? contactAddress = null;

            if (random.NextDouble() < 0.45f)
            {
                contactType = ContactTypeEnum.Mail;
                contactAddress = company.Email;
            }
            else if (random.NextDouble() < 0.7f)
            {
                contactType = ContactTypeEnum.ExternalForm;
                contactAddress = ExternalLink;
            }

            return (contactType, contactAddress);
        }

        private string GetTitle(MainTechnologyTypeEnum mainTechnology, Random random, string? seniority)
        {
            if (seniority == null)
            {
                seniority = string.Empty;
            }

            string postfix = string.Empty;
            if (mainTechnology == MainTechnologyTypeEnum.Programming)
            {
                int randomIndex = random.Next(TitleGameplayPrefixTechnology.Length);
                string prefixTech = TitleGameplayPrefixTechnology[randomIndex];

                randomIndex = random.Next(TitleGameplayPostfix.Length);
                string postfixTech = TitleGameplayPostfix[randomIndex];
                postfix = $"{prefixTech} {postfixTech}";
            }
            else if (mainTechnology == MainTechnologyTypeEnum.Audio)
            {
                int randomIndex = random.Next(TitleAudio.Length);
                string prefixTech = TitleAudio[randomIndex];
                postfix = prefixTech;
            }
            else if (mainTechnology == MainTechnologyTypeEnum.Graphics3D)
            {
                int randomIndex = random.Next(TitleArtAndAnimation.Length);
                string prefixTech = TitleArtAndAnimation[randomIndex];
                postfix = prefixTech;
            }
            else if (mainTechnology == MainTechnologyTypeEnum.LevelDesign)
            {
                int randomIndex = random.Next(TitleDesign.Length);
                string prefixTech = TitleDesign[randomIndex];
                postfix = prefixTech;
            }

            string title = $"{seniority} {postfix}";

            return title;
        }

        private WorkLocationTypeEnum GetWorkLocation(Random random)
        {
            var workLocationTypes = Enum.GetValues(typeof(WorkLocationTypeEnum))
                .Cast<WorkLocationTypeEnum>()
                .ToList();

            int randomIndex = random.Next(workLocationTypes.Count);
            return workLocationTypes[randomIndex];
        }
    }
}

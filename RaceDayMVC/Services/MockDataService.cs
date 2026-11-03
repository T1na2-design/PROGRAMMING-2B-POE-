using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Services
{
    public class MockDataService
    {
        /// <summary>
        /// Returns a list of mock racing events for development/testing purposes.
        /// </summary>
        public List<EventViewModel> GetMockEvents()
        {
            return new List<EventViewModel>
            {
                new EventViewModel
                {
                    EventId = 1,
                    EventName = "Comrades Marathon 2024",
                    Description = "The world's largest and oldest ultra-marathon, run between Pietermaritzburg and Durban.",
                    EventDate = new DateTime(2024, 6, 9),
                    Location = "KwaZulu-Natal",
                    RaceType = "Ultra Marathon",
                    Distance = 89,
                    EntryFee = 350,
                    MaxParticipants = 20000,
                    RegisteredParticipants = 18500,
                    OrganizerName = "Comrades Marathon Association",
                    Status = "Registration Closed",
                    ParticipantCount = 18500
                },
                new EventViewModel
                {
                    EventId = 2,
                    EventName = "Two Oceans Marathon",
                    Description = "A 56 km ultra-marathon run annually in Cape Town, South Africa.",
                    EventDate = new DateTime(2024, 3, 24),
                    Location = "Western Cape",
                    RaceType = "Ultra Marathon",
                    Distance = 56,
                    EntryFee = 450,
                    MaxParticipants = 15000,
                    RegisteredParticipants = 14200,
                    OrganizerName = "Two Oceans Marathon Organisation",
                    Status = "Registration Closed",
                    ParticipantCount = 14200
                },
                new EventViewModel
                {
                    EventId = 3,
                    EventName = "Soweto Marathon",
                    Description = "Annual marathon celebrating running culture in Soweto, Johannesburg.",
                    EventDate = new DateTime(2024, 11, 3),
                    Location = "Gauteng",
                    RaceType = "Marathon",
                    Distance = 42.2m,
                    EntryFee = 250,
                    MaxParticipants = 10000,
                    RegisteredParticipants = 7500,
                    OrganizerName = "Soweto Running Club",
                    Status = "Open",
                    ParticipantCount = 7500
                },
                new EventViewModel
                {
                    EventId = 4,
                    EventName = "Durban Half Marathon",
                    Description = "Scenic half marathon along the Durban beachfront.",
                    EventDate = new DateTime(2024, 8, 18),
                    Location = "KwaZulu-Natal",
                    RaceType = "Half Marathon",
                    Distance = 21.1m,
                    EntryFee = 180,
                    MaxParticipants = 5000,
                    RegisteredParticipants = 3200,
                    OrganizerName = "Durban Athletics Club",
                    Status = "Open",
                    ParticipantCount = 3200
                },
                new EventViewModel
                {
                    EventId = 5,
                    EventName = "Cape Town Trail Run",
                    Description = "Challenging trail run with stunning Table Mountain views.",
                    EventDate = new DateTime(2024, 9, 22),
                    Location = "Western Cape",
                    RaceType = "Trail Run",
                    Distance = 25,
                    EntryFee = 150,
                    MaxParticipants = 2000,
                    RegisteredParticipants = 1250,
                    OrganizerName = "Cape Trail Running Society",
                    Status = "Open",
                    ParticipantCount = 1250
                }
            };
        }

        /// <summary>
        /// Returns mock details for a specific event with categories.
        /// </summary>
        public EventDetailsViewModel? GetMockEventDetails(int eventId)
        {
            var mockEvents = new Dictionary<int, EventDetailsViewModel>
            {
                {
                    1, new EventDetailsViewModel
                    {
                        EventID = 1,
                        Title = "Comrades Marathon 2024",
                        Description = "The world's largest and oldest ultra-marathon, run between Pietermaritzburg and Durban. Join thousands of runners in this iconic 89 km endurance race.",
                        EventDate = new DateTime(2024, 6, 9),
                        Venue = "Pietermaritzburg to Durban",
                        City = "Durban",
                        Province = "KwaZulu-Natal",
                        EventType = "Ultra Marathon",
                        Distance = 89,
                        DistanceUnit = "km",
                        MaxParticipants = 20000,
                        Status = "Registration Closed",
                        Categories = new List<CategoryViewModel>
                        {
                            new CategoryViewModel
                            {
                                CategoryID = 101,
                                CategoryName = "Elite Runners",
                                Description = "For experienced ultra-marathon runners with proven track record.",
                                Distance = 89,
                                DistanceUnit = "km",
                                EntryFee = 500,
                                StartTime = new DateTime(2024, 6, 9, 5, 30, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 102,
                                CategoryName = "Age Group 18-35",
                                Description = "For runners aged 18 to 35 years.",
                                Distance = 89,
                                DistanceUnit = "km",
                                EntryFee = 350,
                                StartTime = new DateTime(2024, 6, 9, 6, 0, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 103,
                                CategoryName = "Age Group 35-50",
                                Description = "For runners aged 35 to 50 years.",
                                Distance = 89,
                                DistanceUnit = "km",
                                EntryFee = 350,
                                StartTime = new DateTime(2024, 6, 9, 6, 15, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 104,
                                CategoryName = "Age Group 50+",
                                Description = "For runners aged 50 years and above.",
                                Distance = 89,
                                DistanceUnit = "km",
                                EntryFee = 300,
                                StartTime = new DateTime(2024, 6, 9, 6, 30, 0)
                            }
                        }
                    }
                },
                {
                    2, new EventDetailsViewModel
                    {
                        EventID = 2,
                        Title = "Two Oceans Marathon",
                        Description = "A 56 km ultra-marathon run annually in Cape Town. Experience the beauty of Cape Town's coastal routes in this prestigious event.",
                        EventDate = new DateTime(2024, 3, 24),
                        Venue = "False Bay to Atlantic Ocean",
                        City = "Cape Town",
                        Province = "Western Cape",
                        EventType = "Ultra Marathon",
                        Distance = 56,
                        DistanceUnit = "km",
                        MaxParticipants = 15000,
                        Status = "Registration Closed",
                        Categories = new List<CategoryViewModel>
                        {
                            new CategoryViewModel
                            {
                                CategoryID = 201,
                                CategoryName = "Fast Race",
                                Description = "For sub-5 hour runners.",
                                Distance = 56,
                                DistanceUnit = "km",
                                EntryFee = 600,
                                StartTime = new DateTime(2024, 3, 24, 6, 0, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 202,
                                CategoryName = "Standard Race",
                                Description = "For runners targeting 5-6 hours.",
                                Distance = 56,
                                DistanceUnit = "km",
                                EntryFee = 450,
                                StartTime = new DateTime(2024, 3, 24, 6, 30, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 203,
                                CategoryName = "Leisure Race",
                                Description = "For all fitness levels.",
                                Distance = 56,
                                DistanceUnit = "km",
                                EntryFee = 350,
                                StartTime = new DateTime(2024, 3, 24, 7, 0, 0)
                            }
                        }
                    }
                },
                {
                    3, new EventDetailsViewModel
                    {
                        EventID = 3,
                        Title = "Soweto Marathon",
                        Description = "Annual marathon celebrating the rich running culture and spirit of Soweto. A community-driven event promoting health and wellness.",
                        EventDate = new DateTime(2024, 11, 3),
                        Venue = "Soweto Stadium",
                        City = "Johannesburg",
                        Province = "Gauteng",
                        EventType = "Marathon",
                        Distance = 42.2m,
                        DistanceUnit = "km",
                        MaxParticipants = 10000,
                        Status = "Open",
                        Categories = new List<CategoryViewModel>
                        {
                            new CategoryViewModel
                            {
                                CategoryID = 301,
                                CategoryName = "Men's Open",
                                Description = "Open category for all men.",
                                Distance = 42.2m,
                                DistanceUnit = "km",
                                EntryFee = 250,
                                StartTime = new DateTime(2024, 11, 3, 6, 0, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 302,
                                CategoryName = "Women's Open",
                                Description = "Open category for all women.",
                                Distance = 42.2m,
                                DistanceUnit = "km",
                                EntryFee = 200,
                                StartTime = new DateTime(2024, 11, 3, 6, 15, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 303,
                                CategoryName = "Half Marathon",
                                Description = "21.1 km alternative race.",
                                Distance = 21.1m,
                                DistanceUnit = "km",
                                EntryFee = 150,
                                StartTime = new DateTime(2024, 11, 3, 6, 30, 0)
                            }
                        }
                    }
                },
                {
                    4, new EventDetailsViewModel
                    {
                        EventID = 4,
                        Title = "Durban Half Marathon",
                        Description = "Scenic half marathon along the beautiful Durban beachfront with ocean views and coastal atmosphere.",
                        EventDate = new DateTime(2024, 8, 18),
                        Venue = "Durban Beachfront",
                        City = "Durban",
                        Province = "KwaZulu-Natal",
                        EventType = "Half Marathon",
                        Distance = 21.1m,
                        DistanceUnit = "km",
                        MaxParticipants = 5000,
                        Status = "Open",
                        Categories = new List<CategoryViewModel>
                        {
                            new CategoryViewModel
                            {
                                CategoryID = 401,
                                CategoryName = "Competitive",
                                Description = "For competitive runners.",
                                Distance = 21.1m,
                                DistanceUnit = "km",
                                EntryFee = 220,
                                StartTime = new DateTime(2024, 8, 18, 5, 30, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 402,
                                CategoryName = "Fun Run",
                                Description = "For recreational runners.",
                                Distance = 21.1m,
                                DistanceUnit = "km",
                                EntryFee = 150,
                                StartTime = new DateTime(2024, 8, 18, 6, 0, 0)
                            }
                        }
                    }
                },
                {
                    5, new EventDetailsViewModel
                    {
                        EventID = 5,
                        Title = "Cape Town Trail Run",
                        Description = "Challenging trail run with stunning Table Mountain views. Experience nature while testing your trail running skills.",
                        EventDate = new DateTime(2024, 9, 22),
                        Venue = "Table Mountain National Park",
                        City = "Cape Town",
                        Province = "Western Cape",
                        EventType = "Trail Run",
                        Distance = 25,
                        DistanceUnit = "km",
                        MaxParticipants = 2000,
                        Status = "Open",
                        Categories = new List<CategoryViewModel>
                        {
                            new CategoryViewModel
                            {
                                CategoryID = 501,
                                CategoryName = "Long Trail (25km)",
                                Description = "Full trail run distance.",
                                Distance = 25,
                                DistanceUnit = "km",
                                EntryFee = 200,
                                StartTime = new DateTime(2024, 9, 22, 7, 0, 0)
                            },
                            new CategoryViewModel
                            {
                                CategoryID = 502,
                                CategoryName = "Short Trail (12km)",
                                Description = "Shorter trail option.",
                                Distance = 12,
                                DistanceUnit = "km",
                                EntryFee = 120,
                                StartTime = new DateTime(2024, 9, 22, 7, 30, 0)
                            }
                        }
                    }
                }
            };

            return mockEvents.ContainsKey(eventId) ? mockEvents[eventId] : null;
        }
    }
}

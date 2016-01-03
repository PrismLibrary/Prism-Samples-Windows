

using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private static string ImageServerPath = System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];
        private static readonly IEnumerable<Category> _categories = PopulateCategories();

        public IEnumerable<Category> GetAll()
        {
            lock (_categories)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _categories.ToArray();
            }
        }

        public Category GetItem(int id)
        {
            lock (_categories)
            {
                return _categories.FirstOrDefault(c => c.Id == id);
            }
        }

        public Category Create(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Category> PopulateCategories()
        {
            return new List<Category>
             {
                 new Category {Title = "Today's Deals", Id = 0, ImageUri = new Uri(ImageServerPath + "hotrodbike_red_large.jpg", UriKind.Absolute) },
                 new Category {Title = "Accessories", Id = 4000, ImageUri = new Uri(ImageServerPath + "water_bottle_large.jpg", UriKind.Absolute) },
                 new Category {Title = "Bikes", Id = 1000, ImageUri = new Uri(ImageServerPath + "julianax_r_02_blue_large.jpg", UriKind.Absolute) },
                 new Category {Title = "Clothing", Id = 3000, ImageUri = new Uri(ImageServerPath + "awc_jersey_male_large.jpg", UriKind.Absolute) },
                 new Category {Title = "Components", Id = 2000, ImageUri = new Uri(ImageServerPath + "chain_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Mountain Bikes", Id=1, ParentId=1000, ImageUri = new Uri(ImageServerPath + "hotrodbike_red_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Road Bikes", Id=2, ParentId=1000, ImageUri = new Uri(ImageServerPath + "roadster_black_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Touring Bikes", Id=3, ParentId=1000, ImageUri = new Uri(ImageServerPath + "julianax_r_02_blue_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Handlebars", Id=4, ParentId=2000, ImageUri = new Uri(ImageServerPath + "handlebar_large.jpg", UriKind.Absolute) },                 
                 new Category { Title = "Brakes", Id=6, ParentId=2000, ImageUri = new Uri(ImageServerPath + "front_brakes.jpg", UriKind.Absolute) },
                 new Category { Title = "Chains", Id=7, ParentId=2000, ImageUri = new Uri(ImageServerPath + "chain_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Cranksets", Id=8, ParentId=2000, ImageUri = new Uri(ImageServerPath + "Crankset.jpg", UriKind.Absolute) },
                 new Category { Title = "Derailleurs", Id=9, ParentId=2000, ImageUri = new Uri(ImageServerPath + "sprocket_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Forks", Id=10, ParentId=2000, ImageUri = new Uri(ImageServerPath + "fork_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Headsets", Id=11, ParentId=2000, ImageUri = new Uri(ImageServerPath + "headset.jpg", UriKind.Absolute) },
                 new Category { Title = "Mountain Frames", Id=12, ParentId=2000, ImageUri = new Uri(ImageServerPath + "hotrod_frame_black_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Pedals", Id=13, ParentId=2000, ImageUri = new Uri(ImageServerPath + "pedal_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Road Frames", Id=14, ParentId=2000, ImageUri = new Uri(ImageServerPath + "road_frame_yellow_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Saddles", Id=15, ParentId=2000, ImageUri = new Uri(ImageServerPath + "road_seat.jpg", UriKind.Absolute) },
                 new Category { Title = "Touring Frames", Id=16, ParentId=2000, ImageUri = new Uri(ImageServerPath + "touring_frame_blue_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Wheels", Id=17, ParentId=2000, ImageUri = new Uri(ImageServerPath + "wheel_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Bib-Shorts", Id=18, ParentId=3000, ImageUri = new Uri(ImageServerPath + "shorts_male_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Caps", Id=19, ParentId=3000, ImageUri = new Uri(ImageServerPath + "logo_cap.jpg", UriKind.Absolute) },
                 new Category { Title = "Gloves", Id=20, ParentId=3000, ImageUri = new Uri(ImageServerPath + "half_finger_gloves.jpg", UriKind.Absolute) },
                 new Category { Title = "Jerseys", Id=21, ParentId=3000, ImageUri = new Uri(ImageServerPath + "awc_tee_male_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Shorts", Id=22, ParentId=3000, ImageUri = new Uri(ImageServerPath + "shorts_female_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Socks", Id=23, ParentId=3000, ImageUri = new Uri(ImageServerPath + "mtn _socks.jpg", UriKind.Absolute) },
                 new Category { Title = "Tights", Id=24, ParentId=3000, ImageUri = new Uri(ImageServerPath + "tights_female.jpg", UriKind.Absolute) },
                 new Category { Title = "Vests", Id=25, ParentId=3000, ImageUri = new Uri(ImageServerPath + "classic_vest.jpg", UriKind.Absolute) },
                 new Category { Title = "Bike Racks", Id=26, ParentId=4000, ImageUri = new Uri(ImageServerPath + "hitch_rack.jpg", UriKind.Absolute) },
                 new Category { Title = "Bike Stands", Id=27, ParentId=4000, ImageUri = new Uri(ImageServerPath + "bike_stand.jpg", UriKind.Absolute) },
                 new Category { Title = "Bottles and Cages", Id=28, ParentId=4000, ImageUri = new Uri(ImageServerPath + "water_bottle_cage_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Cleaners", Id=29, ParentId=4000, ImageUri = new Uri(ImageServerPath + "bike_wash.jpg", UriKind.Absolute) },
                 new Category { Title = "Fenders", Id=30, ParentId=4000, ImageUri = new Uri(ImageServerPath + "mtn_fender_set.jpg", UriKind.Absolute) },
                 new Category { Title = "Helmets", Id=31, ParentId=4000, ImageUri = new Uri(ImageServerPath + "sport_helmet_red.jpg", UriKind.Absolute) },
                 new Category { Title = "Hydration Packs", Id=32, ParentId=4000, ImageUri = new Uri(ImageServerPath + "hydration_pack.jpg", UriKind.Absolute) },
                 new Category { Title = "Lights", Id=33, ParentId=4000, ImageUri = new Uri(ImageServerPath + "tail_lights_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Locks", Id=34, ParentId=4000, ImageUri = new Uri(ImageServerPath + "bike_lock_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Panniers", Id=35, ParentId=4000, ImageUri = new Uri(ImageServerPath + "touring_panniers.jpg", UriKind.Absolute) },
                 new Category { Title = "Pumps", Id=36, ParentId=4000, ImageUri = new Uri(ImageServerPath + "handpump_large.jpg", UriKind.Absolute) },
                 new Category { Title = "Tires and Tubes", Id=37, ParentId=4000, ImageUri = new Uri(ImageServerPath + "mb_tires_large.jpg", UriKind.Absolute) },

            };
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
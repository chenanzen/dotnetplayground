using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    public interface IRealEstateListing
    {
        int ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        int Price { get; set; }
        string Location { get; set; }
    }

    public interface IRealEstateApp
    {
        void AddListing(IRealEstateListing listing);
        void RemoveListing(int listingID);
        void UpdateListing(IRealEstateListing listing);
        List<IRealEstateListing> GetListings();
        List<IRealEstateListing> GetListingsByLocation(string location);
        List<IRealEstateListing> GetListingsByPriceRange(int minPrice, int maxPrice);
    }

    class RealEstateListing : IRealEstateListing
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
    }

    class RealEstateApp : IRealEstateApp
    {
        public void AddListing(IRealEstateListing listing)
        {
            throw new NotImplementedException();
        }

        public List<IRealEstateListing> GetListings()
        {
            throw new NotImplementedException();
        }

        public List<IRealEstateListing> GetListingsByLocation(string location)
        {
            throw new NotImplementedException();
        }

        public List<IRealEstateListing> GetListingsByPriceRange(int minPrice, int maxPrice)
        {
            throw new NotImplementedException();
        }

        public void RemoveListing(int listingID)
        {
            throw new NotImplementedException();
        }

        public void UpdateListing(IRealEstateListing listing)
        {
            throw new NotImplementedException();
        }
    }

}

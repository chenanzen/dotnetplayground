using ConsoleApp2;

static void testing()
{
    using TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);
    IRealEstateApp app = new RealEstateApp();
    int lCount = Convert.ToInt32(Console.ReadLine().Trim());
    for (int i = 1; i <= lCount; i++)
    {
        var a = Console.ReadLine().Trim().Split(" ");
        IRealEstateListing e = new RealEstateListing();
        e.ID = Convert.ToInt32(a[0]);
        e.Title = a[1];
        e.Description = a[2];
        e.Price = Convert.ToInt32(a[3]);
        e.Location = a[4];
        app.AddListing(e);
    }

    textWriter.WriteLine("All Listings:");
    List<IRealEstateListing> allListings = app.GetListings();
    foreach (var listing in allListings)
    {
        textWriter.WriteLine($"ID: {listing.ID}, Title: {listing.Title}, Price: {listing.Price} , Location: {listing.Location}");
    }

    var b = Console.ReadLine().Trim().Split(" ");
    var location = b[0];
    textWriter.WriteLine($"Listings in {location}:");
    List<IRealEstateListing> listingsByLocation = app.GetListingsByLocation(location);
    foreach (var listing in listingsByLocation)
    {
        textWriter.WriteLine($"ID: {listing.ID}, Title: {listing.Title}, Price: {listing.Price}");
    }
    var c = Console.ReadLine().Trim().Split(" ");
    var minPrice = Convert.ToInt32(c[0]);
    var maxPrice = Convert.ToInt32(c[1]);
    var getListingsByPriceRange = app.GetListingsByPriceRange(minPrice, maxPrice);
    textWriter.WriteLine($"Listings By Price Range ({minPrice} - {maxPrice}):");
    foreach (var item in getListingsByPriceRange)
    {
        textWriter.WriteLine($"ID: {item.ID}, Title: {item.Title}, Price: {item.Price}");
    }



    textWriter.Flush();
    textWriter.Close();
}

testing();
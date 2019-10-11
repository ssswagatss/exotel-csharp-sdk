using ExotelSdk;
using System;

namespace ExotelSample.Phone
{
    class Program
    {
        static void Main(string[] args)
        {
            ExotelCall c = new ExotelCall("<your_sid>", "<your_api_key>", "<your_api_token>");
            var response = c.ConnectTwoNumbersAsync("<your_from_phone_number>", "<your_to_phone_number>", "<your_exotel_phone_number>").GetAwaiter().GetResult();
            if (response.IsSuccess)
                Console.WriteLine("Success");
            else
                Console.WriteLine("Fail");
            Console.ReadKey();
        }
    }
}

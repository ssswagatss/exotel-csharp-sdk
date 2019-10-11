# Exotel SDK (C-Sharp)

ExotelSdk is a C# library built on top of .Net Standard 2.0. This makes it extremely easy to implement Exotel's RESTful APIs to integrate Call and Sending SMS using Exotel's Rest API. For more details please visit [Exotel Developer portal](https://developer.exotel.com/) 

## Installation
The library is hosted on NuGet. You can install the same to your project using both Package Manager and .Net CLI. 

Installing **ExotelSdk** using [NuGet Package Manager Console](https://www.nuget.org/) 
```bash
PM>Install-Package ExotelSdk
```

This will install the packages and its dependencies to your project and you can start using the methods just by importing the **ExotelSdk** namespace. 

```csharp
    using ExotelSdk; // This is important
    
    
    // Other codes are removed for clarity
    ExotelCall c = new ExotelCall("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.ConnectTwoNumbersAsync("<your_from_phone_number>", "<your_to_phone_number>", "<your_exotel_phone_number>");
    if (response.IsSuccess)
        Console.WriteLine("Success");
    else
        Console.WriteLine("Fail");
```
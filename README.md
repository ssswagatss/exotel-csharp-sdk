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
    ExotelConnect c = new ExotelConnect("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.ConnectTwoNumbersAsync("<your_from_phone_number>", "<your_to_phone_number>", "<your_exotel_phone_number>");
    if (response.IsSuccess)
        Console.WriteLine("Success");
    else
        Console.WriteLine("Fail");
```

## API References

The project contains extension methods for the followings. 

- [Outgoing call to connect two numbers](#connecttwonumbersasync)
- [Call details](#getcalldetails)
- [Send SMS](#sendsms)
- [SMS details](#getsmsdetails)

## Api Reference Details

* ### ConnectTwoNumbersAsync()
This API will connect two numbers. It connects From Number first. Once the person at the From end picks up the phone, it will connect to the number provided as To. You can choose which number should be connected first by adding that number in the From field.

```csharp
/// from : The phone number that will be called first. If not set, our system will try to match it with a country and make a call. If landline number, prefix it with STD code; Ex: 080XXXX2400
/// to : Your customer's phone number. If landline number, prefix it with STD code; Ex: 080XXXX2400
/// callerId : This is your ExoPhone/Exotel Virtual Number
/// timeLimit : The time limit (in seconds) that you want this call to last. The call will be cut after this time
/// timeOut : The time (in seconds) to ring the called parties (both first and second call leg)
/// statusCallback : An HTTP POST request will be made to this URL depending on what events are subscribed using ‘StatusCallbackEvents’.
/// isRecord : Record the conversation of your call. The RecordingUrl will be sent to the StatusCallback URL if this is set to 'true' and the call conversation happens.
public async Task<ExotelResponse> ConnectTwoNumbersAsync(string from, string to, string callerId, int? timeLimit = null,
                                              int? timeOut = null, string statusCallback = null, bool isRecord = true);
```
#### Example
```csharp
    // Other codes are removed for clarity
    ExotelConnect c = new ExotelConnect("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.ConnectTwoNumbersAsync("<your_from_phone_number>", "<your_to_phone_number>", "<your_exotel_phone_number>");
```
Click here to check the [Official documentation](https://developer.exotel.com/api/#call-agent)

* ### GetCallDetails()
Gets the call details(including Status, Price, etc.).

```csharp
/// callReferenceId : Call Reference Id
public async Task<ExotelResponse> GetCallDetails(string callReferenceId);
```
#### Example
```csharp
    // Other codes are removed for clarity
    ExotelConnect c = new ExotelConnect("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.GetCallDetails("<sid_for_call>");
```
Click here to check the [Official documentation](https://developer.exotel.com/api/#call-details)

* ### SendSms()
This API will send an SMS to the specified To number.

```csharp
/// from : Specify one of your ExoPhone
/// to : Mobile number to which SMS has to be sent.
/// messageBody : Content of your SMS; Max Length of the body cannot exceed 2000 characters
/// encodingType : Message type of SMS; "plain" or "unicode"
/// priority : Priority of the SMS; "normal" or "high". Business critical operations like sending verification codes, confirming appointments etc which require immediate SMS delivery should opt for high priority.
public async Task<ExotelResponse> SendSms(string from, string to, string messageBody, string encodingType = "", string priority = "");
```
#### Example
```csharp
    // Other codes are removed for clarity
    ExotelConnect c = new ExotelConnect("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.SendSms("<your_from_phone_number>","<your_to_phone_number","<your_message_body>");
```
Click here to check the [Official documentation](https://developer.exotel.com/api/#send-sms)

* ### GetSmsDetails()
Gets the SMS details

```csharp
/// smsReferenceId : SMS reference Id
public async Task<ExotelResponse> GetSmsDetails(string smsReferenceId);
```
#### Example
```csharp
    // Other codes are removed for clarity
    ExotelConnect c = new ExotelConnect("<your_sid>", "<your_api_key>", "<your_api_token>");
    var response =await c.GetSmsDetails("<your_sms_sid>");
```
Click here to check the [Official documentation](https://developer.exotel.com/api/#send-details)
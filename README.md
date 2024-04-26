# WorkCast REST API HTTP client example

## This repo was created using

```shell
dotnet new console -n repapi-clienthttp
```

---

## Background

### Installing versions of .NET & .NETCore

- For Windows
  - <https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net80>
- For Linux (Ubuntu)
  -  <https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install>
- For Mac OS
  - <https://learn.microsoft.com/en-us/dotnet/core/install/macos>

### .NET/Core Versions

There is a `global.json`, which dictates which version of dotnet SDK to build with and what C# language to be build to.

To discover what version you currently have perform the following command:-

```sh
dotnet --version
8.0.202
```

To discover what versions of the .NET SDK you currently have installed on your machine perform the following command:-

```sh
dotnet --list-sdks
3.1.410 [C:\Program Files\dotnet\sdk]
5.0.416 [C:\Program Files\dotnet\sdk]
6.0.201 [C:\Program Files\dotnet\sdk]
6.0.300 [C:\Program Files\dotnet\sdk]
8.0.202 [C:\Program Files\dotnet\sdk]
```

For which version of language you can use with each .NET SDK version use the following documentation guide:-

<https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version>

You can then change the contents of the `global.json` to your requirements:-

```json
{
  "sdk": {
    "version": "8.0.202"
  },
  "langversion": "12"
}
```

There are some examples with `dotnet-versions` folder for some various versions of dotnet SDK and the `global.json` file.

If you change the `global.json` then you may also need the change the `TargetFramework` within the dotnet `.csproj` files

See <https://learn.microsoft.com/en-us/dotnet/standard/frameworks> for details on what the `TargetFramework` should be for each dotnet version.

e.g.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <RootNamespace>repapi_clienthttp</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

Validate that the change is made by going into your terminal in the root folder of the project and running:-

```shell
dotnet --version
```

> **Important:** Once you have changed your `global.json` contents always re-run the `.\clean-build.ps1` script.
> **Important:** .NetCore `<3.1` are obsolete and out of support by Microsoft and have high vulnerabilities, so examples are not shown here as they do not function correctly.

---

## Building

### Windows 10/11

```shell
.\clean-build.ps1
```

### Linux & Mac OS

```shell
./clean-build.sh
```

---

## Standard Usage

Authenticate with APIKEY to gain BearerToken at the endpoint:-

`GET https://auth-rest-endpoint.com/v1.0/signin`

```json
{"idToken":"returnedBearerToken","expiresIn":3600,"paging":null,"responseCode":200,"message":"Ok"}
```

---

## Example without Paging

```powershell
dotnet run `
https://reporting-rest-endpoint.com/v1.0/events/12345/sessions/ `
returnedBearerToken `
0 `
200 `
> .\output\non-paging-response.json
```

### Non Paging Response

```json
{
  "totalCount": 1,
  "sessions": [
    {
      "sessionId": 12345,
      "sessionPak": "2569516622532149",
      "title": "mr broadcast on-demand media test",
      "sessionType": "On-demand",
      "presentationType": "Slides Only",
      "sessionDateTime": "2020-06-13T19:00:00.000Z",
      "durationHHMM": "01:00",
      "totalRegPageVisits": 3,
      "totalRegistrations": 3,
      "totalAttendees": 3,
      "totalAttendeePageVisits": 4,
      "pollType": "normal",
      "pollPassPcent": "",
      "totalPollsAsked": 0,
      "totalPollResponses": 0,
      "totalChat": 0,
      "totalQnA": 0,
      "totalEngagementScore": "6",
      "createdDate": "2020-06-13T06:24:14.227Z",
      "updatedDate": "2020-06-13T06:24:16.180Z"
    }
  ],
  "paging": null,
  "responseCode": 200,
  "message": "Ok"
}
```

---

## Example with paging

### First Request (page 1)

```powershell
dotnet run `
https://reporting-rest-endpoint.com/v1.0/events/ `
returnedBearerToken `
0 `
200 `
> .\output\paging-response-1.json
```

#### Paging Response (page 1)

```json
{
  "totalCount": 100,
  "events": [
    {
      "eventId": 12345,
      "eventPak": "4850279148362910",
      "title": "event 1",
      "eventType": "Multi Session",
      "eventDateTime": "2021-03-18T11:15:00.000Z",
      "durationHHMM": "00:20",
      "totalSessions": 3,
      "createdDate": "2019-08-02T09:54:26.573Z",
      "updatedDate": "2021-01-27T13:33:42.203Z"
    },
    // ...
    // Lots of Responses here
    // ...
    {
      "eventId": 12345,
      "eventPak": "7018462950381624",
      "title": "2020 -06-09 - scheduled replay",
      "eventType": "Live",
      "eventDateTime": "2020-06-09T17:00:00.000Z",
      "durationHHMM": "01:00",
      "totalSessions": 1,
      "createdDate": "2020-06-09T14:39:29.433Z",
      "updatedDate": "2020-06-09T14:39:29.593Z"
    }
  ],
  "paging": {
    "previous": null,
    "next": "https://reporting-rest-endpoint.com/v1.0/events/3e246728-8ff0-4a15-8c4d-9e04fd9b9e95"
  },
  "responseCode": 200,
  "message": "Ok"
}
```

---

### Next Request (page 2)

Take the `next` value (`https://reporting-rest-endpoint.com/v1.0/events/3e246728-8ff0-4a15-8c4d-9e04fd9b9e95`) to use as your next page url

```powershell
dotnet run `
https://reporting-rest-endpoint.com/v1.0/events/3e246728-8ff0-4a15-8c4d-9e04fd9b9e95 `
returnedBearerToken `
0 `
200 `
> .\output\paging-response-2.json
```

#### Paging Response (page 2)

```json
{
  "totalCount": 100,
  "events": [
    {
      "eventId": 12345,
      "eventPak": "4850279148362910",
      "title": "event 1",
      "eventType": "Multi Session",
      "eventDateTime": "2021-03-18T11:15:00.000Z",
      "durationHHMM": "00:20",
      "totalSessions": 3,
      "createdDate": "2019-08-02T09:54:26.573Z",
      "updatedDate": "2021-01-27T13:33:42.203Z"
    },
    // ...
    // Lots of Responses here
    // ...
    {
      "eventId": 12345,
      "eventPak": "7018462950381624",
      "title": "2020 -06-09 - scheduled replay",
      "eventType": "Live",
      "eventDateTime": "2020-06-09T17:00:00.000Z",
      "durationHHMM": "01:00",
      "totalSessions": 1,
      "createdDate": "2020-06-09T14:39:29.433Z",
      "updatedDate": "2020-06-09T14:39:29.593Z"
    }
  ],
  "paging": {
    "previous": "https://reporting-rest-endpoint.com/v1.0/events/d29e7f86-7d2a-4718-a15a-1be6b8baf194",
    "next": "https://reporting-rest-endpoint.com/v1.0/events/56e9b6c7-2f5d-4b0c-bb73-186f4a8d9e0e"
  },
  "responseCode": 200,
  "message": "Ok"
}
```

> **Important:** You can use the `"previous"` and `"next"` urls above to navigate around the data.


---

### Expired/Invalid Paging Request

Use an old or invalid guid as the paging parameter e.g. `213abfa9-1919-4f5f-a325-25483cf2b8aa`

```powershell
dotnet run `
https://reporting-rest-endpoint.com/v1.0/events/213abfa9-1919-4f5f-a325-25483cf2b8aa `
returnedBearerToken `
0 `
200 `
> .\output\expired-paging-response-2.json
```

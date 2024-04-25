# wc- rest-api http client example

## Building

```powershell


dotnet new console -n repapi-clienthttp

.\clean-build.ps1
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

### None-Paging Response

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
    "next": "https://reporting-rest-endpoint.com/v1.0/events/213abfa9-1919-4f5f-a325-25483cf2b8aa"
  },
  "responseCode": 200,
  "message": "Ok"
}
```

---

### Next Request (page 2)

Take the `next` value (`https://reporting-rest-endpoint.com/v1.0/events/213abfa9-1919-4f5f-a325-25483cf2b8aa`) to use as your next page url

```powershell
dotnet run `
https://reporting-rest-endpoint.com/v1.0/events/213abfa9-1919-4f5f-a325-25483cf2b8aa `
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
    "previous": "https://reporting-rest-endpoint.com/v1.0/events/213abfa9-1919-4f5f-a325-25483cf2b8aa",
    "next": "https://reporting-rest-endpoint.com/v1.0/events/56e9b6c7-2f5d-4b0c-bb73-186f4a8d9e0e"
  },
  "responseCode": 200,
  "message": "Ok"
}
```

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

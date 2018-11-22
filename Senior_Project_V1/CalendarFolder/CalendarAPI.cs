using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Senior_Project_V1
{
    public class CalendarAPI
    {
        //define what user can do with scopes
        string[] scopes = new string[] { "calendars.read", "calendars.readwrite", "calendars.read.shared"};

        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// <returns>String containing the results of the GET operation</returns>
        private async Task<string> GetHttpContentWithToken(string url, string token)
        {
            //provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
            var httpClient = new System.Net.Http.HttpClient();
            //Represents an HTTP response message including the status code and data
            System.Net.Http.HttpResponseMessage response;
            try
            {
                //perform a query using an http verb and identified url
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                // represents authentication information in authorization
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = "";
                if ((int)response.StatusCode == 200)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// Perform an HTTP POST request to a URL using an HTTP Authorization header
        /// <returns>String containing the results of the GET operation</returns>
        private async Task<string> PostHttpContentWithToken(string url, string token)
        {
            //provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
            var httpClient = new System.Net.Http.HttpClient();
            //Represents an HTTP response message including the status code and data
            System.Net.Http.HttpResponseMessage response;
            try
            {
                //perform a query using an http verb and identified url
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, url);
                // represents authentication information in authorization
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = "";
                if ((int)response.StatusCode == 201)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// Perform an HTTP DELETE request to a URL using an HTTP Authorization header
        /// <returns>String containing the results of the GET operation</returns>
        private async Task<bool> DeleteHttpContentWithToken(string url, string token)
        {
            //provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
            var httpClient = new System.Net.Http.HttpClient();
            //Represents an HTTP response message including the status code and data
            System.Net.Http.HttpResponseMessage response;

            //perform a query using an http verb and identified url
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, url);
            // represents authentication information in authorization
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            response = await httpClient.SendAsync(request);
            bool content = false;
            if ((int)response.StatusCode == 204)
            {
                content = true;
            }
            return content;
        }

        /// Perform an HTTP PATCH request to a URL using an HTTP Authorization header
        /// <returns>String containing the results of the GET operation</returns>
        private async Task<string> PATCHHttpContentWithToken(string url, string token)
        {
            System.Uri uri = new System.Uri(url);
            //provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI
            var httpClient = new Windows.Web.Http.HttpClient();
            //Represents an HTTP response message including the status code and data
            Windows.Web.Http.HttpResponseMessage response;
            try
            {
                //perform a query using an http verb and identified uri
                var request = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Patch, uri);
                response = await httpClient.SendRequestAsync(request);
                var content = "";
                if ((int)response.StatusCode == 200)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //Post
        public async Task<RootObject> CreateCalendar(AuthenticationResult authResult)
        {
            string createCalendarEndPnt = "https://outlook.office.com/api/v2.0/me/calendars";

            //holds json response
            var result = await PostHttpContentWithToken(createCalendarEndPnt, authResult.AccessToken);
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

        //Get
        public async Task<RootObject> getCalendar(AuthenticationResult authResult)
        {
            string getCalendarEndPnt = "https://outlook.office.com/api/v2.0/me/calendars";

            //container to hold json response
            var result = await GetHttpContentWithToken(getCalendarEndPnt, authResult.AccessToken);
            //class serializes objects to JSON and deserializes JSON data to objects
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            //initializes a new non-resizable instance of the class based on specified byte array
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            //cast to ReadObject and read data from memorystream to access other data contracts and data members
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

        //Post
        public async Task<RootObject> createEvent(AuthenticationResult authResult)
        {
            string createEventEndPnt = "https://outlook.office.com/api/v2.0/me/events";

            //holds json response
            var result = await PostHttpContentWithToken(createEventEndPnt, authResult.AccessToken);
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

        //Post
        public async Task<RootObject> updateEvent(AuthenticationResult authResult)
        {
            string updateEventEndPnt = "https://outlook.office.com/api/v2.0/me/events/{event_id}";

            //holds json response
            var result = await PostHttpContentWithToken(updateEventEndPnt, authResult.AccessToken);
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }

        //Delete
        public async Task<bool> deleteEvent(AuthenticationResult authResult, string id)
        {
            string deleteEventEndPnt = "https://outlook.office.com/api/v2.0/me/events/{event_id}";

            //holds json response
            var result = await DeleteHttpContentWithToken(deleteEventEndPnt, authResult.AccessToken);

            return result;
        }

        //Get
        public async Task<RootObject> readEvent(AuthenticationResult authResult)
        {
            string readEventEndPnt = "https://outlook.office.com/api/v2.0/me/calendarview?startDateTime={start_datetime}&endDateTime={end_datetime}";

            //holds json response
            var result = await GetHttpContentWithToken(readEventEndPnt, authResult.AccessToken);
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }
    }

    [DataContract]
    public class Owner
    {
        //creating and getting calendar 
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        //creating calendar
        [DataMember]
        public string Id { get; set; }              //and updating event
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ChangeKey { get; set; }       //and updating event
        [DataMember]
        public bool CanShare { get; set; }
        [DataMember]
        public bool CanViewPrivateItems { get; set; }
        [DataMember]
        public bool CanEdit { get; set; }
        [DataMember]
        public Owner Owner { get; set; }
        //getting calendar
        public List<Value> value { get; set; }          //and reading event
        //create event
        [DataMember]
        public string Subject { get; set; }             //and updating event
        [DataMember]
        public Body Body { get; set; }                  //and updating event
        [DataMember]
        public Start Start { get; set; }                //and updating event
        [DataMember]
        public End End { get; set; }                    //and updating event
        [DataMember]
        public List<Attendee> Attendees { get; set; }   //and updating event
        //update event
        [DataMember]
        public List<object> Categories { get; set; }
        [DataMember]
        public DateTime CreatedDateTime { get; set; }
        [DataMember]
        public DateTime LastModifiedDateTime { get; set; }
        [DataMember]
        public string BodyPreview { get; set; }
        [DataMember]
        public string Importance { get; set; }
        [DataMember]
        public bool HasAttachments { get; set; }
        [DataMember]
        public Location Location { get; set; }
        [DataMember]
        public string ShowAs { get; set; }
        [DataMember]
        public bool IsAllDay { get; set; }
        [DataMember]
        public bool IsCancelled { get; set; }
        [DataMember]
        public bool IsOrganizer { get; set; }
        [DataMember]
        public bool ResponseRequested { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public object SeriesMasterId { get; set; }
        [DataMember]
        public object Recurrence { get; set; }
        [DataMember]
        public string OriginalEndTimeZone { get; set; }
        [DataMember]
        public string OriginalStartTimeZone { get; set; }
        [DataMember]
        public Organizer Organizer { get; set; }
        [DataMember]
        public object OnlineMeetingUrl { get; set; }
    }

    [DataContract]
    public class Value
    {
        //getting calendar
        [DataMember]
        public string Id { get; set; }          //and reading event
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string ChangeKey { get; set; }
        [DataMember]
        public bool CanShare { get; set; }
        [DataMember]
        public bool CanViewPrivateItems { get; set; }
        [DataMember]
        public bool CanEdit { get; set; }
        [DataMember]
        public Owner Owner { get; set; }
        //reading event
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public Start Start { get; set; }
        [DataMember]
        public End End { get; set; }
        [DataMember]
        public Organizer Organizer { get; set; }
    }

    [DataContract]
    public class Body
    {
        //create and updating event
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public string Content { get; set; }
    }

    [DataContract]
    public class Start
    {
        //create, updating, and reading event
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public string TimeZone { get; set; }
    }

    [DataContract]
    public class End
    {
        //create, updating, and reading event
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public string TimeZone { get; set; }
    }

    [DataContract]
    public class EmailAddress
    {
        //create, updating, and reading event
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class Attendee
    {
        //create event
        [DataMember]
        public EmailAddress EmailAddress { get; set; }
        [DataMember]
        public string Type { get; set; }
    }

    [DataContract]
    public class Location
    {
        //updating event
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public object Address { get; set; }
    }

    [DataContract]
    public class Organizer
    {
        //updating and reading event
        [DataMember]
        public EmailAddress EmailAddress { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBSSkillsTracker_WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace CBSSkillsTracker_WebUI.Pages
{
    public class ScoreSubmissionFormModel : PageModel
    {

        string Baseurl = "https://capabilitytracker.azurewebsites.net";


        [BindProperty(SupportsGet = true)]
        public List<CapabilitiesModel> CapabilitiesList { get; set; }
        
      
        [BindProperty] 
        public List<ScoreModel> Scores { get; set; }

        public async Task<IActionResult> OnGet()
        {

           

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetUsers using HttpClient
                HttpResponseMessage Res =  await client.GetAsync("/api/Capabilities");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    CapabilitiesList = JsonConvert.DeserializeObject<List<CapabilitiesModel>>(UserResponse);
                }
              
            }
            return Page();
        }


        public IActionResult OnPost(List<ScoreModel> Scores) 
        {

            using (var client = new HttpClient())
            {


                if (!ModelState.IsValid)
                {
                    return Page();
                }

                List<ScoreModel> scores = Scores.Select(s => new ScoreModel
                        {
                        Capability = s.Id,
                        Score  = s.Score,
                        })
                    .ToList();


                client.BaseAddress = new Uri(Baseurl + "/api/Scores");


                //HTTP POST
                var httpContent = new StringContent(JsonConvert.SerializeObject(scores), Encoding.UTF8, "application/json");
                var postTask = client.PostAsync(client.BaseAddress.ToString(), httpContent);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return Page();
                }
                else
                { //ModelState.AddModelError(string.Empty, "An Error Occured in 'public ActionResult NewCapability'- Please contact Dev Support ");
                }
            }



            return RedirectToPage ("/Index");


        }

    }
}

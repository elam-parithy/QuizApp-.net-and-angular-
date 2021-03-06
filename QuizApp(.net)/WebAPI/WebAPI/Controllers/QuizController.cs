using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class QuizController : ApiController
    {
        [HttpGet]
        [Route("api/Questions")]
        public HttpResponseMessage GetQuestions() {
            using (QuizDBEntities db = new QuizDBEntities())
            {
                var Qns = db.Questions
                    .Select(x => new { QnID = x.QnID, Qn = x.Qn, ImageName = x.ImageName, x.Option1, x.Option2, x.Option3, x.Option4 })
                    .OrderBy(y => Guid.NewGuid())
                    .Take(10)
                    .ToArray();
                var updated = Qns.AsEnumerable()
                    .Select(x => new
                    {
                        QnID = x.QnID,
                        Qn = x.Qn,
                        ImageName = x.ImageName,
                        Options = new string[] { x.Option1, x.Option2, x.Option3, x.Option4 }
                    }).ToList();
                return this.Request.CreateResponse(HttpStatusCode.OK, updated);
            }
        }
    [HttpGet]
    [Route("api/GetAllScores")]
    public HttpResponseMessage GetAllScores()
    {
      using (QuizDBEntities db = new QuizDBEntities())
      {
        var Pt = db.Participants
            .Select(x => new { ParticipantID = x.ParticipantID, Name = x.Name, Email = x.Email , Score = x.Score })
            .OrderBy(y => Guid.NewGuid())
            .Take(10)
            .ToArray();
        var updated = Pt.AsEnumerable()
            .Select(x => new
            {
              ParticipantID = x.ParticipantID,
              Email = x.Email,
              Name = x.Name,
              Score = x.Score
            }).ToList();
        return this.Request.CreateResponse(HttpStatusCode.OK, updated);
      }
    }

    [HttpPost]
        [Route("api/Answers")]
        public HttpResponseMessage GetAnswers(int[] qIDs) {
            using (QuizDBEntities db = new QuizDBEntities())
            {
               var result = db.Questions
                    .AsEnumerable()
                    .Where(y => qIDs.Contains(y.QnID))
                    .OrderBy(x => { return Array.IndexOf(qIDs, x.QnID); })
                    .Select(z => z.Answer)
                    .ToArray();
                return this.Request.CreateResponse(HttpStatusCode.OK, result); 
            }
        }
    }
}
